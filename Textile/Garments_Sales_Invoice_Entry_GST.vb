Public Class Garments_Sales_Invoice_Entry_GST

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "GSLIN-"

    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Single
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxtOrder_Details As New DataGridViewTextBoxEditingControl

    Private prn_InpOpts As String = ""
    Private prn_OriDupTri As String = ""
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private prn_Status As Integer
    Private prn_Count As Integer
    Private dgv_ActCtrlName As String = ""

    Private dgv_LevColNo As Integer
    Private Filter_RowNo As Integer = -1
    Public CHk_Details_Cnt As Integer = 0

    Private Property vCbo_GrdItmNm As String = ""

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
        pnl_Selection.Visible = False
        pnl_OrderSelection.Visible = False
        pnl_OrderQty.Visible = False
        pnl_Tax.Visible = False

        lbl_InvoiceNo.Text = ""
        lbl_InvoiceNo.ForeColor = Color.Black

        dtp_InvocieDate.Text = ""
        cbo_Ledger.Text = ""
        cbo_DeliveryTo.Text = ""
        txt_OrderNo.Text = ""
        txt_orderdate.Text = ""
        txt_DcNo.Text = ""
        cbo_VechileNo.Text = ""

        cbo_Agent.Text = ""

        cbo_Through.Text = "DIRECT"

        txt_LrNo.Text = ""
        txt_lrDate.Text = ""
        cbo_Transport.Text = ""
        cbo_SalesAc.Text = ""
        cbo_VatAc.Text = ""

        txt_Note.Text = ""
        chk_AgainstForm.Checked = False


        txt_Noof_Carton.Text = ""
        lbl_GrossAmount.Text = ""
        lbl_AssessableValue.Text = ""
        lbl_OrderCode.Text = ""

        txt_DiscPerc.Text = ""
        lbl_DiscAmount.Text = ""
        cbo_TaxType.Text = "-NIL-"
        cbo_Type.Text = "DIRECT"
        cbo_Entry_Tax_Type.Text = "GST"
        txt_TaxPerc.Text = ""
        lbl_TaxAmount.Text = ""
        cbo_DespTo.Text = ""
        txt_DelvAdd1.Text = ""
        txt_DelvAdd2.Text = ""
        txt_Freight.Text = ""

        txt_Packing.Text = ""
        txt_AddLess.Text = ""
        lbl_RoundOff.Text = ""
        lbl_NetAmount.Text = "0.00"
        lbl_AmountInWords.Text = "Rupees  :  "  ' "Amount In Words : "

        lbl_CGST_Amount.Text = ""
        lbl_SGST_Amount.Text = ""
        lbl_IGST_Amount.Text = ""
        txt_ElectronicRefNo.Text = ""
        txt_DateAndTimeOFSupply.Text = ""
        txt_TransportMode.Text = ""
        txt_Noof_Carton.Text = ""
        txt_DueDate.Text = ""
        txt_DueDays.Text = ""

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        dgv_CartonDetails.Rows.Clear()
        dgv_BaleDetails_Total.Rows.Clear()
        dgv_BaleDetails_Total.Rows.Add()

        dgv_OrderDetails.Rows.Clear()
        dgv_OrderDetails_Total.Rows.Clear()
        dgv_OrderDetails_Total.Rows.Add()

        dgv_Tax_Details.Rows.Clear()
        dgv_Tax_Total_Details.Rows.Clear()

        txt_BaleNo_Selection.Text = ""
        dgv_Selection.Rows.Clear()

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If
        dgv_Details.Tag = ""
        dgv_LevColNo = -1

        Grid_Cell_DeSelect()

        dgv_ActCtrlName = ""

        NoCalc_Status = False

        cbo_Grid_ItemName.Visible = False
        cbo_Grid_Size.Visible = False
        cbo_Grid_Unit.Visible = False

    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim da4 As New SqlClient.SqlDataAdapter
        Dim dt4 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer

        If Val(no) = 0 Then Exit Sub

        clear()

        NoCalc_Status = True

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as PartyName from Garments_Sales_Invoice_head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Invoice_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_InvoiceNo.Text = dt1.Rows(0).Item("Sales_Invoice_No").ToString
                dtp_InvocieDate.Text = dt1.Rows(0).Item("Sales_Invoice_Date").ToString

                cbo_Ledger.Text = dt1.Rows(0).Item("PartyName").ToString

                txt_OrderNo.Text = dt1.Rows(0).Item("Order_No").ToString
                txt_orderdate.Text = dt1.Rows(0).Item("Order_Date").ToString
                txt_DcNo.Text = dt1.Rows(0).Item("Dc_No").ToString
                txt_DcDate.Text = dt1.Rows(0).Item("Dc_Date").ToString
                cbo_VechileNo.Text = Common_Procedures.Vehicle_IdNoToName(con, Val(dt1.Rows(0).Item("Vechile_IdNo").ToString))
                cbo_DeliveryTo.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("DeliveryTo_IdNo").ToString))
                cbo_Agent.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Agent_IdNo").ToString))
                cbo_SalesAc.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("SalesAc_IdNo").ToString))
                cbo_Through.Text = dt1.Rows(0).Item("Through_Name").ToString

                txt_LrNo.Text = dt1.Rows(0).Item("Lr_No").ToString
                txt_lrDate.Text = dt1.Rows(0).Item("Lr_Date").ToString
                cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))
                cbo_Type.Text = dt1.Rows(0).Item("Selection_Type").ToString

                txt_Noof_Carton.Text = dt1.Rows(0).Item("Noof_Carton").ToString
                txt_DueDate.Text = dt1.Rows(0).Item("Due_Date").ToString
                txt_DueDays.Text = dt1.Rows(0).Item("Due_Days").ToString
                lbl_GrossAmount.Text = Format(Val(dt1.Rows(0).Item("Total_Amount").ToString), "#########0.00")
                txt_DiscPerc.Text = Val(dt1.Rows(0).Item("Discount_Percentage").ToString)
                lbl_DiscAmount.Text = Format(Val(dt1.Rows(0).Item("Discount_Amount").ToString), "#########0.00")
                lbl_AssessableValue.Text = Format(Val(dt1.Rows(0).Item("Assessable_Value").ToString), "#########0.00")
                cbo_TaxType.Text = dt1.Rows(0).Item("Tax_Type").ToString
                If Trim(cbo_TaxType.Text) = "" Then cbo_TaxType.Text = "-NIL-"
                txt_TaxPerc.Text = Val(dt1.Rows(0).Item("Tax_Percentage").ToString)
                lbl_TaxAmount.Text = Format(Val(dt1.Rows(0).Item("Tax_Amount").ToString), "#########0.00")
                txt_Packing_Name.Text = dt1.Rows(0).Item("Packing_Name").ToString
                txt_Packing.Text = Format(Val(dt1.Rows(0).Item("Packing_Amount").ToString), "#########0.00")
                txt_AddLess_Name.Text = dt1.Rows(0).Item("AddLess_Name").ToString
                txt_AddLess.Text = Format(Val(dt1.Rows(0).Item("AddLess_Amount").ToString), "#########0.00")
                lbl_RoundOff.Text = Format(Val(dt1.Rows(0).Item("RoundOff_Amount").ToString), "#########0.00")
                txt_Freight.Text = Format(Val(dt1.Rows(0).Item("Freight_Amount").ToString), "###############0.00")
                txt_Freight_Name.Text = dt1.Rows(0).Item("Freight_Name").ToString
                lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(dt1.Rows(0).Item("Net_Amount").ToString))
                cbo_VatAc.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("VatAc_IdNo").ToString))

                txt_Note.Text = dt1.Rows(0).Item("Note").ToString
                If Val(dt1.Rows(0).Item("AgainstForm_Status").ToString) = 1 Then chk_AgainstForm.Checked = True

                cbo_DespTo.Text = dt1.Rows(0).Item("Despatch_To").ToString
                txt_DelvAdd1.Text = dt1.Rows(0).Item("Delivery_Address1").ToString
                txt_DelvAdd2.Text = dt1.Rows(0).Item("Delivery_Address2").ToString

                lbl_OrderCode.Text = dt1.Rows(0).Item("Sales_Order_Code").ToString

                lbl_TaxableAmount.Text = Format(Val(dt1.Rows(0).Item("Total_Taxable_Value").ToString), "#########0.00")
                lbl_CGST_Amount.Text = Format(Val(dt1.Rows(0).Item("Total_CGST_Amount").ToString), "#########0.00")
                lbl_SGST_Amount.Text = Format(Val(dt1.Rows(0).Item("Total_SGST_Amount").ToString), "#########0.00")
                lbl_IGST_Amount.Text = Format(Val(dt1.Rows(0).Item("Total_IGST_Amount").ToString), "#########0.00")
                txt_ElectronicRefNo.Text = dt1.Rows(0).Item("Electronic_Reference_No").ToString
                txt_DateAndTimeOFSupply.Text = dt1.Rows(0).Item("Date_And_Time_Of_Supply").ToString
                txt_TransportMode.Text = dt1.Rows(0).Item("Transport_Mode").ToString

                da2 = New SqlClient.SqlDataAdapter("Select a.* from Garments_Sales_Invoice_Details a  Where a.Sales_Invoice_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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
                            .Rows(n).Cells(1).Value = Common_Procedures.Item_IdNoToName1(con, Val(dt2.Rows(i).Item("Item_IdNo").ToString))
                            .Rows(n).Cells(2).Value = Common_Procedures.Size_IdNoToName(con, Val(dt2.Rows(i).Item("SIze_IdNo").ToString))
                            .Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Quantity").ToString)
                            .Rows(n).Cells(4).Value = Common_Procedures.Unit_IdNoToName(con, Val(dt2.Rows(i).Item("Unit_IdNo").ToString))

                            .Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")
                            .Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")
                            .Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Cash_Discount_Percentage").ToString), "############0.0")
                            .Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Cash_Discount_Amount").ToString), "##########0.00")
                            .Rows(n).Cells(9).Value = Val(dt2.Rows(i).Item("Footer_Cash_Discount_Percentage").ToString)
                            .Rows(n).Cells(10).Value = Val(dt2.Rows(i).Item("Footer_Cash_Discount_Amount").ToString)
                            .Rows(n).Cells(11).Value = Val(dt2.Rows(i).Item("Taxable_Value").ToString)
                            .Rows(n).Cells(12).Value = Val(dt2.Rows(i).Item("GST_Percentage").ToString)
                            .Rows(n).Cells(13).Value = Val(dt2.Rows(i).Item("HSN_Code").ToString)
                        Next i

                    End If

                    If .RowCount = 0 Then .Rows.Add()

                End With

                'With dgv_Details_Total
                '    If .RowCount = 0 Then .Rows.Add()
                '    .Rows(0).Cells(3).Value = Val(dt1.Rows(0).Item("Total_Quantity").ToString)
                '    .Rows(0).Cells(6).Value = Format(Val(dt1.Rows(0).Item("Total_Amount").ToString), "########0.00")
                'End With
                NoCalc_Status = False
                Total_Calculation()
                NoCalc_Status = True

                'da2 = New SqlClient.SqlDataAdapter("Select a.* from Sales_Invoice_PackingSlip_Details a Where a.Sales_Invoice_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                'dt2 = New DataTable
                'da2.Fill(dt2)

                'With dgv_CartonDetails

                '    .Rows.Clear()
                '    SNo = 0

                '    If dt2.Rows.Count > 0 Then

                '        For i = 0 To dt2.Rows.Count - 1

                '            n = .Rows.Add()

                '            SNo = SNo + 1
                '            .Rows(n).Cells(0).Value = Val(SNo)
                '            .Rows(n).Cells(1).Value = dt2.Rows(i).Item("Item_PackingSlip_No").ToString
                '            .Rows(n).Cells(2).Value = Val(dt2.Rows(i).Item("Quantity").ToString)

                '            .Rows(n).Cells(3).Value = dt2.Rows(i).Item("Item_PackingSlip_Code").ToString

                '        Next i

                '    End If

                'End With

                If Trim(UCase(cbo_Type.Text)) = "PACKING SLIP" Then
                    With dgv_BaleDetails_Total
                        If .RowCount = 0 Then .Rows.Add()
                        .Rows(0).Cells(1).Value = Val(dt1.Rows(0).Item("Total_Carton").ToString)
                        .Rows(0).Cells(2).Value = Val(dt1.Rows(0).Item("Total_Quantity").ToString)
                    End With
                End If

                'da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Item_Name from Sales_Invoice_Order_Details a INNER JOIN Item_Head b ON b.Item_IdNo <> 0 and a.Item_IdNo = b.Item_IdNo INNER JOIN Sales_Order_Head c ON a.Sales_Order_Code = c.Sales_Order_Code Where a.Sales_Invoice_Code = '" & Trim(NewCode) & "' Order by b.Item_Name, c.Sales_Order_Date, c.for_OrderBy, c.Sales_Order_No, c.Sales_Order_Code", con)
                'dt2 = New DataTable
                'da2.Fill(dt2)

                'With dgv_OrderDetails

                '    .Rows.Clear()
                '    SNo = 0

                '    If dt2.Rows.Count > 0 Then

                '        For i = 0 To dt2.Rows.Count - 1

                '            n = .Rows.Add()

                '            SNo = SNo + 1
                '            .Rows(n).Cells(0).Value = Val(SNo)
                '            .Rows(n).Cells(1).Value = dt2.Rows(i).Item("Item_Name").ToString
                '            .Rows(n).Cells(2).Value = dt2.Rows(i).Item("Sales_Order_No").ToString
                '            .Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Quantity").ToString)
                '            .Rows(n).Cells(4).Value = dt2.Rows(i).Item("Sales_Order_Code").ToString

                '        Next i

                '    End If

                'End With

                With dgv_OrderDetails_Total
                    If .RowCount = 0 Then .Rows.Add()
                    NoCalc_Status = False
                    Total_OrderItemCalculation()
                    NoCalc_Status = True
                    '.Rows(0).Cells(3).Value = Val(dt1.Rows(0).Item("Total_Quantity").ToString)
                End With

                dt2.Dispose()
                da2.Dispose()

                da4 = New SqlClient.SqlDataAdapter("Select a.* from Garments_Sales_GST_Tax_Details a Where a.Sales_Invoice_Code = '" & Trim(NewCode) & "' ", con)
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

            dt1.Dispose()
            da1.Dispose()

            Grid_Cell_DeSelect()

            dgv_ActCtrlName = ""

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        NoCalc_Status = False

        If dtp_InvocieDate.Visible And dtp_InvocieDate.Enabled Then dtp_InvocieDate.Focus()

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox

        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is Button Or TypeOf Me.ActiveControl Is CheckBox Then
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
        'If Me.ActiveControl.Name <> dgv_Details.Name Then
        '    Common_Procedures.Hide_CurrentStock_Display()
        'End If
        If Me.ActiveControl.Name <> cbo_Grid_ItemName.Name Then
            cbo_Grid_ItemName.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_Size.Name Then
            cbo_Grid_Size.Visible = False
        End If

        If Me.ActiveControl.Name <> cbo_Grid_Unit.Name Then
            cbo_Grid_Unit.Visible = False
        End If

        Grid_Cell_DeSelect()

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is CheckBox Then
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
        'dgv_Details.CurrentCell.Selected = False
        'dgv_Details_Total.CurrentCell.Selected = False
        'dgv_OrderDetails.CurrentCell.Selected = False
        'dgv_OrderDetails_Total.CurrentCell.Selected = False
        'dgv_CartonDetails.CurrentCell.Selected = False
        'dgv_BaleDetails_Total.CurrentCell.Selected = False
        'dgv_Selection.CurrentCell.Selected = False
        'dgv_Filter_Details.CurrentCell.Selected = False


        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_OrderDetails.CurrentCell) Then dgv_OrderDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_OrderDetails_Total.CurrentCell) Then dgv_OrderDetails_Total.CurrentCell.Selected = False
        If Not IsNothing(dgv_CartonDetails.CurrentCell) Then dgv_CartonDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_BaleDetails_Total.CurrentCell) Then dgv_BaleDetails_Total.CurrentCell.Selected = False
        If Not IsNothing(dgv_Selection.CurrentCell) Then dgv_Selection.CurrentCell.Selected = False

    End Sub

    Private Sub FinishedProduct_Invoice_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Agent.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Agent.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_DeliveryTo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_DeliveryTo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_SalesAc.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_SalesAc.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_VatAc.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_VatAc.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_VechileNo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "VECHILE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_VechileNo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_ItemName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ITEM" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_ItemName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Size.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "SIZE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Size.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Unit.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "UNIT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Unit.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub FinishedProduct_Invoice_Entry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Me.Text = ""

        con.Open()

        cbo_TaxType.Items.Clear()
        cbo_TaxType.Items.Add("")
        cbo_TaxType.Items.Add("-NIL-")
        cbo_TaxType.Items.Add("VAT")
        cbo_TaxType.Items.Add("CST")
        cbo_TaxType.Items.Add("GST")

        cbo_Entry_Tax_Type.Items.Clear()
        cbo_Entry_Tax_Type.Items.Add("NO TAX")
        cbo_Entry_Tax_Type.Items.Add("GST")

        cbo_Through.Items.Clear()
        cbo_Through.Items.Add(" ")
        cbo_Through.Items.Add("DIRECT")
        cbo_Through.Items.Add("BANK")
        cbo_Through.Items.Add("AGENT")

        cbo_Type.Items.Clear()
        cbo_Type.Items.Add(" ")
        cbo_Type.Items.Add("DIRECT")
        'cbo_Type.Items.Add("ORDER")
        cbo_Type.Items.Add("PACKING SLIP")


        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        pnl_OrderSelection.Visible = False
        pnl_OrderSelection.Left = (Me.Width - pnl_OrderSelection.Width) \ 2
        pnl_OrderSelection.Top = (Me.Height - pnl_OrderSelection.Height) \ 2
        pnl_OrderSelection.BringToFront()

        pnl_Tax.Visible = False
        pnl_Tax.Left = (Me.Width - pnl_Tax.Width) \ 2
        pnl_Tax.Top = ((Me.Height - pnl_Tax.Height) \ 2) - 100
        pnl_Tax.BringToFront()

        pnl_Print.Visible = False
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2
        pnl_Print.BringToFront()

        'pnl_BaleDetails.Visible = False
        'pnl_BaleDetails.Left = (Me.Width - pnl_BaleDetails.Width) \ 2
        'pnl_BaleDetails.Top = (Me.Height - pnl_BaleDetails.Height) \ 2
        'pnl_BaleDetails.BringToFront()

        AddHandler dtp_InvocieDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DueDate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DueDays.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Noof_Carton.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OrderNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Through.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DeliveryTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_ItemName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Size.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Unit.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_orderdate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Carton_Weight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DcNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VechileNo.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Type.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Agent.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_SalesAc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LrNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_lrDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DiscPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TaxPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Packing.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Entry_Tax_Type.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLess.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TaxType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VatAc.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DespTo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DelvAdd1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DcDate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLess_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Packing_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DelvAdd2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Note.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BaleNo_Selection.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_SendSMS.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Preprint_J.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Preprint_A.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Invoice.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Cancel.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_ElectronicRefNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DateAndTimeOFSupply.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TransportMode.GotFocus, AddressOf ControlGotFocus


        AddHandler dtp_InvocieDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DeliveryTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OrderNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DueDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DueDays.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Noof_Carton.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_orderdate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DcNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Carton_Weight.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Size.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VechileNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DespTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Agent.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Through.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_SalesAc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LrNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_lrDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DiscPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TaxType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Packing_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AddLess_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Packing_Name.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DelvAdd1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DelvAdd2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DcDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TaxPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Packing.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AddLess.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VatAc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Entry_Tax_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Note.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BaleNo_Selection.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_SendSMS.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Preprint_J.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Preprint_A.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Invoice.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Cancel.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_ElectronicRefNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DateAndTimeOFSupply.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TransportMode.LostFocus, AddressOf ControlLostFocus


        AddHandler txt_orderdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DcNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_LrNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_lrDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TaxPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Packing.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AddLess.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Freight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DcDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Note.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Carton_Weight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DelvAdd2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_OrderNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DelvAdd1.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_ElectronicRefNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DateAndTimeOFSupply.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TransportMode.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Noof_Carton.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_DueDays.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler txt_OrderNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_orderdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DcNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_LrNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_lrDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DcDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DiscPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DelvAdd2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TaxPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Packing.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Carton_Weight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AddLess.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DelvAdd1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Freight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_ElectronicRefNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DateAndTimeOFSupply.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TransportMode.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_DueDays.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Noof_Carton.KeyPress, AddressOf TextBoxControlKeyPress

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub FinishedProduct_Invoice_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
        ' Common_Procedures.Hide_CurrentStock_Display()

    End Sub

    Private Sub FinishedProduct_Invoice_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

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

                ElseIf pnl_OrderSelection.Visible = True Then
                    btn_Close_OrderSelection_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Tax.Visible = True Then
                    btn_Tax_Close_Click(sender, e)
                    Exit Sub

                    'ElseIf pnl_Selection.Visible = True Then
                    '    btn_close_Click(sender, e)
                    '    Exit Sub

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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        If ActiveControl.Name = dgv_Details.Name Or ActiveControl.Name = dgv_OrderDetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            On Error Resume Next

            dgv1 = Nothing

            If ActiveControl.Name = dgv_Details.Name Then
                dgv1 = dgv_Details

            ElseIf dgv_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Details

            ElseIf ActiveControl.Name = dgv_OrderDetails.Name Then
                dgv1 = dgv_OrderDetails

            ElseIf dgv_OrderDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_OrderDetails

            ElseIf ActiveControl.Name = dgv_CartonDetails.Name Then
                dgv1 = dgv_CartonDetails

            ElseIf dgv_CartonDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_CartonDetails

            ElseIf Trim(UCase(dgv_ActCtrlName)) = Trim(UCase(dgv_Details.Name.ToString)) Then
                dgv1 = dgv_Details

            ElseIf Trim(UCase(dgv_ActCtrlName)) = Trim(UCase(dgv_OrderDetails.Name.ToString)) Then
                dgv1 = dgv_OrderDetails

            ElseIf Trim(UCase(dgv_ActCtrlName)) = Trim(UCase(dgv_CartonDetails.Name.ToString)) Then
                dgv1 = dgv_CartonDetails

            End If

            If IsNothing(dgv1) = False Then

                With dgv1
                    If dgv1.Name = dgv_Details.Name Then

                        If keyData = Keys.Enter Or keyData = Keys.Down Then
                            If .CurrentCell.ColumnIndex >= .ColumnCount - 7 Then
                                If .CurrentCell.RowIndex = .RowCount - 1 Then


                                    txt_DiscPerc.Focus()

                                Else
                                    If UCase(Trim(cbo_Type.Text)) = "PACKING SLIP" Then
                                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(5)
                                    Else

                                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                                    End If


                                End If
                            ElseIf .CurrentCell.ColumnIndex = 5 Then
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(7)
                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If

                            Return True

                        ElseIf keyData = Keys.Up Then

                            If .CurrentCell.ColumnIndex <= 1 Then
                                If .CurrentCell.RowIndex = 0 Then
                                    txt_DueDays.Focus()

                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 7)

                                End If
                            ElseIf .CurrentCell.ColumnIndex = 5 Then
                                If UCase(Trim(cbo_Type.Text)) = "PACKING SLIP" Then
                                    If .CurrentCell.ColumnIndex = 5 And .CurrentCell.RowIndex = 0 Then
                                        txt_DueDays.Focus()
                                    Else
                                        .CurrentCell = .Rows(.CurrentRow.Index - 1).Cells(5)
                                    End If
                                Else
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
                                End If
                            ElseIf .CurrentCell.ColumnIndex = 7 Then
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(5)
                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                            End If

                            Return True

                        Else
                            Return MyBase.ProcessCmdKey(msg, keyData)

                        End If

                    ElseIf dgv1.Name = dgv_OrderDetails.Name Then

                        If keyData = Keys.Enter Or keyData = Keys.Down Then
                            If .CurrentCell.ColumnIndex >= .ColumnCount - 2 Then
                                If .CurrentCell.RowIndex = .RowCount - 1 Then
                                    txt_DiscPerc.Focus()

                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(3)

                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index + 1).Cells(3)

                            End If

                            Return True

                        ElseIf keyData = Keys.Up Then

                            If .CurrentCell.ColumnIndex <= .ColumnCount - 2 Then
                                If .CurrentCell.RowIndex = 0 Then
                                    If dgv_Details.Rows.Count > 0 Then
                                        dgv_Details.Focus()
                                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(6)
                                    Else
                                        txt_DueDays.Focus()
                                    End If
                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(3)

                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(3)

                            End If

                            Return True

                        Else
                            Return MyBase.ProcessCmdKey(msg, keyData)

                        End If
                    End If

                End With

            Else

                Return MyBase.ProcessCmdKey(msg, keyData)

            End If

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

        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Invoice_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Invoice_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

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

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            If Common_Procedures.VoucherBill_Deletion(con, Trim(NewCode), trans) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(NewCode), trans)
            If Trim(UCase(cbo_Type.Text)) = "ORDER" Then
                cmd.CommandText = "Update Sales_Order_Details set Invoice_Quantity = a.Invoice_Quantity - b.Quantity from Sales_Order_Details a, Sales_Invoice_Order_Details b Where b.Sales_Invoice_Code = '" & Trim(NewCode) & "' and a.Sales_Order_Code = b.Sales_Order_Code and a.Item_IdNo = b.Item_IdNo"
                cmd.ExecuteNonQuery()
            End If
            If Trim(UCase(cbo_Type.Text)) = "PACKING SLIP" Then
                cmd.CommandText = "Update Garments_Item_PackingSlip_Head set Invoice_Code = '', Invoice_Increment = Invoice_Increment - 1 Where Invoice_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()
            End If

            'cmd.CommandText = "delete from Stock_FP_Item_Processing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(NewCode) & "' and Entry_Identification = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(NewCode) & "' and Entry_Identification = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            'cmd.CommandText = "Delete from Sales_Invoice_Order_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Invoice_Code = '" & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()
            'cmd.CommandText = "delete from Sales_Invoice_PackingSlip_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Invoice_Code = '" & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Garments_Sales_GST_Tax_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Invoice_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from Garments_Sales_Invoice_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Invoice_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from Garments_Sales_Invoice_head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Invoice_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            'If Val(Common_Procedures.settings.NegativeStock_Restriction) = 1 Then

            '    If Common_Procedures.Check_Negative_Stock_Status(con, trans) = True Then Exit Sub

            'End If

            trans.Commit()

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            If InStr(1, Trim(LCase(ex.Message)), Trim(LCase("ck_Sales_Order_Details_1"))) > 0 Then
                MessageBox.Show("Invalid Receipt Quantity, Must be greater than zero", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf InStr(1, Trim(LCase(ex.Message)), Trim(LCase("ck_Sales_Order_Details_2"))) > 0 Then
                MessageBox.Show("Invalid Quantity - Invocie Quantity greater than Order Quantity", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                ' MessageBox.Show("Invalid Invoice Quantity in Order Details", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If

        Finally
            If dtp_InvocieDate.Enabled = True And dtp_InvocieDate.Visible = True Then dtp_InvocieDate.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then


            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Back.Enabled = False
        If Filter_Status = True Then
            If dgv_Filter_Details.Rows.Count > 0 And Filter_RowNo >= 0 Then
                dgv_Filter_Details.Focus()
                dgv_Filter_Details.CurrentCell = dgv_Filter_Details.Rows(Filter_RowNo).Cells(0)
                dgv_Filter_Details.CurrentCell.Selected = True
            Else
                If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

            End If

        Else
            If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

        End If

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Sales_Invoice_No from Garments_Sales_Invoice_head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Invoice_Code like '" & Trim(Pk_Condition) & "%' and Sales_Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Sales_Invoice_No", con)
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_InvoiceNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Sales_Invoice_No from Garments_Sales_Invoice_head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Invoice_Code like '" & Trim(Pk_Condition) & "%' and Sales_Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Sales_Invoice_No", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_InvoiceNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Sales_Invoice_No from Garments_Sales_Invoice_head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Invoice_Code like '" & Trim(Pk_Condition) & "%' and Sales_Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Sales_Invoice_No desc", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try
            da = New SqlClient.SqlDataAdapter("select top 1 Sales_Invoice_No from Garments_Sales_Invoice_head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Invoice_Code like '" & Trim(Pk_Condition) & "%' and Sales_Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Sales_Invoice_No desc", con)
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = dt.Rows(0)(0).ToString
                End If
            End If

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        Try
            clear()

            New_Entry = True

            lbl_InvoiceNo.Text = Common_Procedures.get_MaxCode(con, "Garments_Sales_Invoice_head", "Sales_Invoice_Code", "For_OrderBy", "Sales_Invoice_Code like '" & Trim(Pk_Condition) & "%'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_InvoiceNo.ForeColor = Color.Red

            Da1 = New SqlClient.SqlDataAdapter("select Top 1 a.*, b.ledger_name as SalesAcName, c.ledger_name as TaxAcName from Garments_Sales_Invoice_head a LEFT OUTER JOIN Ledger_Head b ON a.SalesAc_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.VatAc_IdNo = c.Ledger_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by a.for_Orderby desc, a.Sales_Invoice_No desc", con)
            Da1.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then
                If Dt1.Rows(0).Item("SalesAcName").ToString <> "" Then cbo_SalesAc.Text = Dt1.Rows(0).Item("SalesAcName").ToString
                If Dt1.Rows(0).Item("Tax_Type").ToString <> "" Then cbo_TaxType.Text = Dt1.Rows(0).Item("Tax_Type").ToString
                If Dt1.Rows(0).Item("Tax_Percentage").ToString <> "" Then txt_TaxPerc.Text = Val(Dt1.Rows(0).Item("Tax_Percentage").ToString)
                If Dt1.Rows(0).Item("TaxAcName").ToString <> "" Then cbo_VatAc.Text = Dt1.Rows(0).Item("TaxAcName").ToString
            End If

            Dt1.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da1.Dispose()

            If dtp_InvocieDate.Enabled And dtp_InvocieDate.Visible Then dtp_InvocieDate.Focus()

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        Try

            inpno = InputBox("Enter Invoice No.", "FOR FINDING...")

            InvCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Sales_Invoice_No from Garments_Sales_Invoice_head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Invoice_Code = '" & Trim(InvCode) & "'", con)
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
                MessageBox.Show("Invoice No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Invoice_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Invoice_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Invoice No.", "FOR NEW INVOICE NO. INSERTION...")

            InvCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Sales_Invoice_No from Garments_Sales_Invoice_head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Sales_Invoice_Code = '" & Trim(InvCode) & "'", con)
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
                    MessageBox.Show("Invalid Invoice No.", "DOES NOT INSERT NEW BILL...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_InvoiceNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW BILL...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Led_ID As Integer = 0
        Dim SalAc_ID As Integer = 0
        Dim DelTo_ID As Integer = 0
        Dim Itm_Id As Integer = 0
        Dim Sz_Id As Integer = 0

        Dim Trans_ID As Integer
        Dim Ag_ID As Integer = 0
        Dim VatAc_ID As Integer = 0
        Dim Vec_Id As Integer = 0
        Dim Unt_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim EntID As String = ""
        Dim Dup_FPname As String = ""
        Dim PBlNo As String = ""
        Dim vTotCrtn As Single, vTotQty As Single, vTotAmt As Single
        Dim vBlsTotQty As Single, vBlsTotMtrs As Single
        Dim vOrdTotQty As Single
        Dim Nr As Long
        Dim Cr_ID As Integer = 0
        Dim Dr_ID As Integer = 0
        Dim Agnst_Sts As Integer = 0
        Dim IncluTax_STS As Integer = 0
        Dim eXmSG As String = ""
        Dim fpitmnm As String = ""
        Dim Rec_ID As Integer = 0
        Dim RecFrm_ID As Integer = 0
        Dim Del_ID As Integer = 0


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Invoice_Entry, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(dtp_InvocieDate.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_InvocieDate.Enabled And dtp_InvocieDate.Visible Then dtp_InvocieDate.Focus()
            Exit Sub
        End If

        If Not (dtp_InvocieDate.Value.Date >= Common_Procedures.Company_FromDate And dtp_InvocieDate.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_InvocieDate.Enabled And dtp_InvocieDate.Visible Then dtp_InvocieDate.Focus()
            Exit Sub
        End If

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        Ag_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Agent.Text)
        Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)
        SalAc_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_SalesAc.Text)
        VatAc_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_VatAc.Text)
        Vec_Id = Common_Procedures.Vehicle_NameToIdNo(con, cbo_VechileNo.Text)
        DelTo_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DeliveryTo.Text)
        If SalAc_ID = 0 And Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            MessageBox.Show("Invalid Sales A/c name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_SalesAc.Enabled And cbo_SalesAc.Visible Then cbo_SalesAc.Focus()
            Exit Sub
        End If
        Dim DupSz_Name As String = ""
        With dgv_Details

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(5).Value) <> 0 Then

                    Itm_Id = Common_Procedures.Item_NameToIdNo1(con, .Rows(i).Cells(1).Value)
                    If Itm_Id = 0 Then
                        MessageBox.Show("Invalid Item Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If


                    Sz_Id = Common_Procedures.Size_NameToIdNo(con, .Rows(i).Cells(2).Value)
                    If Sz_Id = 0 Then
                        MessageBox.Show("Invalid Size Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(2)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If


                    'If InStr(1, Trim(UCase(Dup_FPname)), "~" & Trim(UCase(.Rows(i).Cells(1).Value)) & "~" & Trim(UCase(.Rows(i).Cells(2).Value)) & " ~" & Trim(UCase(.Rows(i).Cells(4).Value)) & " ~") > 0 Then
                    '    MessageBox.Show("Duplicate Item ,Size and Unit Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    '    If .Enabled And .Visible Then
                    '        .Focus()
                    '        .CurrentCell = .Rows(i).Cells(1)
                    '        .CurrentCell.Selected = True
                    '    End If
                    '    Exit Sub
                    'End If

                    Dup_FPname = Trim(Dup_FPname) & "~" & Trim(UCase(.Rows(i).Cells(1).Value)) & "~" & Trim(UCase(.Rows(i).Cells(2).Value)) & " ~" & Trim(UCase(.Rows(i).Cells(4).Value)) & " ~"

                    If Val(.Rows(i).Cells(3).Value) = 0 Then
                        MessageBox.Show("Invalid Quantity", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(3)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                End If



            Next

        End With

        With dgv_CartonDetails

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(2).Value) <> 0 Or Val(.Rows(i).Cells(2).Value) <> 0 Then

                    If Trim(.Rows(i).Cells(1).Value) = "" Or Trim(.Rows(i).Cells(3).Value) = "" Then
                        MessageBox.Show("Invalid CartonNo", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
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

        If VatAc_ID = 0 And Val(lbl_TaxAmount.Text) <> 0 Then
            MessageBox.Show("Invalid Vat A/c name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_VatAc.Enabled And cbo_VatAc.Visible Then cbo_VatAc.Focus()
            Exit Sub
        End If

        NoCalc_Status = False
        Total_Calculation()

        vTotCrtn = 0 : vTotQty = 0 : vTotAmt = 0
        vBlsTotQty = 0 : vBlsTotMtrs = 0
        vOrdTotQty = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotQty = Val(dgv_Details_Total.Rows(0).Cells(3).Value())
            vTotAmt = Val(dgv_Details_Total.Rows(0).Cells(6).Value())
        End If

        If dgv_BaleDetails_Total.RowCount > 0 Then
            vTotCrtn = Val(dgv_BaleDetails_Total.Rows(0).Cells(1).Value())
            vBlsTotQty = Val(dgv_BaleDetails_Total.Rows(0).Cells(2).Value())
            '  vBlsTotMtrs = Val(dgv_BaleDetails_Total.Rows(0).Cells(3).Value())
        End If

        If dgv_OrderDetails_Total.RowCount > 0 Then
            vOrdTotQty = Val(dgv_OrderDetails_Total.Rows(0).Cells(3).Value())
        End If
        If Trim(UCase(cbo_Type.Text)) = "PACKING SLIP" Then
            If Val(vTotQty) <> Val(vBlsTotQty) Then
                MessageBox.Show("Mismatch of Quantity in Invoice and Carton Details", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If dtp_InvocieDate.Enabled And dtp_InvocieDate.Visible Then dtp_InvocieDate.Focus()
                Exit Sub
            End If
        End If

        'If Trim(UCase(cbo_Type.Text)) = "ORDER" Then
        '    If Val(vTotQty) <> Val(vOrdTotQty) Then
        '        MessageBox.Show("Mismatch of Quantity in Invoice and Order Details", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '        If dgv_OrderDetails.Enabled And dgv_OrderDetails.Visible And dgv_OrderDetails.Rows.Count > 0 Then
        '            dgv_OrderDetails.Focus()
        '            dgv_OrderDetails.CurrentCell = dgv_OrderDetails.Rows(0).Cells(3)
        '            'dgv_OrderDetails.CurrentCell.Selected = True
        '        End If
        '        Exit Sub
        '    End If
        'End If

        Agnst_Sts = 0
        If chk_AgainstForm.Checked = True Then Agnst_Sts = 1

        If Trim(UCase(cbo_Type.Text)) <> "ORDER" Then
            lbl_OrderCode.Text = ""
        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_InvoiceNo.Text = Common_Procedures.get_MaxCode(con, "Garments_Sales_Invoice_head", "Sales_Invoice_Code", "For_OrderBy", "Sales_Invoice_Code like '" & Trim(Pk_Condition) & "%'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)
                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@InvoiceDate", dtp_InvocieDate.Value.Date)

            If New_Entry = True Then

                If Trim(txt_DateAndTimeOFSupply.Text) = "" Then txt_DateAndTimeOFSupply.Text = Format(Now, "dd-MM-yyyy hh:mm tt")

                cmd.CommandText = "Insert into Garments_Sales_Invoice_head ( Sales_Invoice_Code                      ,               Company_IdNo       ,     Sales_Invoice_No    ,                     for_OrderBy                                                    , Sales_Invoice_Date                  ,    Ledger_IdNo    ,          Vechile_IdNo     ,             Order_No            ,             Order_Date            ,            Dc_No             ,     Dc_Date                   ,         Agent_IdNo    ,            SalesAc_IdNo   ,           Lr_No              ,               Lr_Date          ,        Transport_IdNo     ,           Total_Carton    ,          Total_Quantity  ,                    Total_Amount            ,             Discount_Percentage    ,              Discount_Amount         ,              Assessable_Value             ,             Tax_Type            ,             Tax_Percentage        ,             Tax_Amount              ,           VatAc_IdNo ,                   Packing_Name       ,     Packing_Amount       ,                     AddLess_Name               ,     AddLess_Amount       ,               RoundOff_Amount      ,              Net_Amount                        ,       Freight_Name                         , Freight_Amount                  ,               Note           ,       AgainstForm_Status    ,           Noof_Carton               ,            Through_Name         ,         Selection_Type       ,  Sales_Order_Code            , Carton_Weight                       ,       Despatch_To                   ,   Delivery_Address1             , Delivery_Address2          ,Electronic_Reference_No                 ,Date_And_Time_Of_Supply                     ,Transport_Mode                        ,Entry_GST_Tax_Type                 ,Total_Taxable_Value                ,Total_CGST_Amount                 ,Total_SGST_Amount                ,Total_IGST_Amount                           ,       DeliveryTo_IdNo   ,      Due_Date                   ,              Due_Days     ) " &
                                    "   Values                              (   '" & Trim(NewCode) & "'    , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ",       @InvoiceDate            , " & Str(Val(Led_ID)) & ", " & Str(Val(Vec_Id)) & ", '" & Trim(txt_OrderNo.Text) & "', '" & Trim(txt_orderdate.Text) & "', '" & Trim(txt_DcNo.Text) & "','" & Trim(txt_DcDate.Text) & "' , " & Str(Val(Ag_ID)) & ", " & Str(Val(SalAc_ID)) & ", '" & Trim(txt_LrNo.Text) & "', '" & Trim(txt_lrDate.Text) & "', " & Str(Val(Trans_ID)) & ", " & Str(Val(vTotCrtn)) & ", " & Str(Val(vTotQty)) & ",  " & Str(Val(lbl_GrossAmount.Text)) & ", " & Str(Val(txt_DiscPerc.Text)) & ", " & Str(Val(lbl_DiscAmount.Text)) & ", " & Str(Val(lbl_AssessableValue.Text)) & ",            'VAT'              , " & Str(Val(txt_TaxPerc.Text)) & ", " & Str(Val(lbl_TaxAmount.Text)) & ", " & Str(Val(VatAc_ID)) & ",  '" & Trim(txt_Packing_Name.Text) & "'," & Str(Val(txt_Packing.Text)) & ",  '" & Trim(txt_AddLess_Name.Text) & "'," & Str(Val(txt_AddLess.Text)) & ", " & Str(Val(lbl_RoundOff.Text)) & ", " & Str(Val(CSng(lbl_NetAmount.Text))) & ", '" & Trim(txt_Freight_Name.Text) & "'," & Val(txt_Freight.Text) & " , '" & Trim(txt_Note.Text) & "', " & Str(Val(Agnst_Sts)) & ", '" & Trim(txt_Noof_Carton.Text) & "' , '" & Trim(cbo_Through.Text) & "', '" & Trim(cbo_Type.Text) & "', '" & Trim(lbl_OrderCode.Text) & "' ," & Val(txt_Carton_Weight.Text) & ",  '" & Trim(cbo_DespTo.Text) & "', '" & Trim(txt_DelvAdd1.Text) & "', '" & Trim(txt_DelvAdd2.Text) & "','" & Trim(txt_ElectronicRefNo.Text) & "','" & Trim(txt_DateAndTimeOFSupply.Text) & "','" & Trim(txt_TransportMode.Text) & "', '" & Trim(cbo_Entry_Tax_Type.Text) & "'    ," & Val(lbl_TaxableAmount.Text) & "," & Val(lbl_CGST_Amount.Text) & " ," & Val(lbl_SGST_Amount.Text) & "," & Val(lbl_IGST_Amount.Text) & "," & Val(DelTo_ID) & "    , '" & Trim(txt_DueDate.Text) & "','" & Trim(txt_DueDays.Text) & "') "
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Garments_Sales_Invoice_head set Sales_Invoice_Date = @InvoiceDate, Ledger_IdNo = " & Str(Val(Led_ID)) & ", Vechile_IdNo = " & Str(Val(Vec_Id)) & ", Order_No = '" & Trim(txt_OrderNo.Text) & "', Order_Date = '" & Trim(txt_orderdate.Text) & "', Dc_No = '" & Trim(txt_DcNo.Text) & "',Dc_Date = '" & Trim(txt_DcDate.Text) & "' ,  Agent_IdNo = " & Str(Val(Ag_ID)) & ", SalesAc_IdNo = " & Str(Val(SalAc_ID)) & ", Lr_No = '" & Trim(txt_LrNo.Text) & "', Lr_Date = '" & Trim(txt_lrDate.Text) & "', Transport_IdNo = " & Str(Val(Trans_ID)) & ", Total_Carton = " & Str(Val(vTotCrtn)) & ", Total_Quantity = " & Str(Val(vTotQty)) & ", Total_Amount = " & Str(Val(lbl_GrossAmount.Text)) & ", Discount_Percentage = " & Str(Val(txt_DiscPerc.Text)) & ", Discount_Amount = " & Str(Val(lbl_DiscAmount.Text)) & ", Assessable_Value = " & Str(Val(lbl_AssessableValue.Text)) & ", Tax_Type = 'VAT', Tax_Percentage = " & Str(Val(txt_TaxPerc.Text)) & ", Tax_Amount = " & Str(Val(lbl_TaxAmount.Text)) & ", VatAc_IdNo = " & Str(Val(VatAc_ID)) & ",Packing_Name =  '" & Trim(txt_Packing_Name.Text) & "',  Packing_Amount = " & Str(Val(txt_Packing.Text)) & ",AddLess_Name =  '" & Trim(txt_AddLess_Name.Text) & "', AddLess_Amount = " & Str(Val(txt_AddLess.Text)) & ", RoundOff_Amount = " & Str(Val(lbl_RoundOff.Text)) & ", Selection_Type = '" & Trim(cbo_Type.Text) & "' , Net_Amount = " & Str(Val(CSng(lbl_NetAmount.Text))) & ", Freight_Name = '" & Trim(txt_Freight_Name.Text) & "',Freight_Amount = " & Val(txt_Freight.Text) & " , Note = '" & Trim(txt_Note.Text) & "', AgainstForm_Status = " & Str(Val(Agnst_Sts)) & " ,Despatch_To = '" & Trim(cbo_DespTo.Text) & "',Delivery_Address1 = '" & Trim(txt_DelvAdd1.Text) & "', Delivery_Address2 = '" & Trim(txt_DelvAdd2.Text) & "'  ,   Noof_Carton = '" & Trim(txt_Noof_Carton.Text) & "' , Through_Name = '" & Trim(cbo_Through.Text) & "', Sales_Order_Code  = '" & Trim(lbl_OrderCode.Text) & "' , Carton_Weight = " & Val(txt_Carton_Weight.Text) & ",Electronic_Reference_No = '" & Trim(txt_ElectronicRefNo.Text) & "',Date_And_Time_Of_Supply = '" & Trim(txt_DateAndTimeOFSupply.Text) & "',Transport_Mode = '" & Trim(txt_TransportMode.Text) & "', Entry_GST_Tax_Type = '" & Trim(cbo_Entry_Tax_Type.Text) & "'    ,Total_Taxable_Value =" & Val(lbl_TaxableAmount.Text) & ",Total_CGST_Amount = " & Val(lbl_CGST_Amount.Text) & " ,Total_SGST_Amount = " & Val(lbl_SGST_Amount.Text) & ",Total_IGST_Amount =" & Val(lbl_IGST_Amount.Text) & ", DeliveryTo_IdNo = " & Val(DelTo_ID) & ",Due_Days = '" & Trim(txt_DueDays.Text) & "',Due_Date='" & Trim(txt_DueDate.Text) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Invoice_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()
                If Trim(UCase(cbo_Type.Text)) = "PACKING SLIP" Then
                    cmd.CommandText = "Update Garments_Item_PackingSlip_Head set Invoice_Code = '', Invoice_Increment = Invoice_Increment - 1 Where Invoice_Code = '" & Trim(NewCode) & "'"
                    cmd.ExecuteNonQuery()
                End If
                If Trim(UCase(cbo_Type.Text)) = "ORDER" Then
                    'cmd.CommandText = "Update Sales_Order_Details set Invoice_Quantity = a.Invoice_Quantity - b.Quantity from Sales_Order_Details a, Sales_Invoice_Order_Details b Where b.Sales_Invoice_Code = '" & Trim(NewCode) & "' and a.Sales_Order_Code = b.Sales_Order_Code and a.Item_IdNo = b.Item_IdNo"
                    'cmd.ExecuteNonQuery()
                End If
            End If

            Partcls = "Bill : Inv.No. " & Trim(lbl_InvoiceNo.Text)
            PBlNo = Trim(lbl_InvoiceNo.Text)
            EntID = Trim(Pk_Condition) & Trim(lbl_InvoiceNo.Text)

            'cmd.CommandText = "Delete from Sales_Invoice_Order_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Invoice_Code = '" & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Garments_Sales_Invoice_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Invoice_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            'cmd.CommandText = "Delete from Sales_Invoice_PackingSlip_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Invoice_Code = '" & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()



            Rec_ID = 4
            Del_ID = 0

            'cmd.CommandText = "Delete from Stock_FP_Item_Processing_Details where company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            With dgv_Details

                Sno = 0

                eXmSG = ""
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(5).Value) <> 0 Then

                        Sno = Sno + 1

                        eXmSG = Trim(.Rows(i).Cells(1).Value)

                        Itm_Id = Common_Procedures.Item_NameToIdNo1(con, .Rows(i).Cells(1).Value, tr)

                        Sz_Id = Common_Procedures.Size_NameToIdNo(con, .Rows(i).Cells(2).Value, tr)

                        Unt_ID = Common_Procedures.Unit_NameToIdNo(con, .Rows(i).Cells(4).Value, tr)

                        cmd.CommandText = "Insert into Garments_Sales_Invoice_Details                     ( Sales_Invoice_Code ,               Company_IdNo       ,     Sales_Invoice_No              ,                     for_OrderBy                                            , Sales_Invoice_Date      ,          Selection_Type      ,          Ledger_IdNo    ,          Sl_No       ,        Item_IdNo          ,     Size_idno           ,                     Quantity             ,           Unit_IdNo            ,                   Rate                   ,                     Amount                ,                Sales_Order_Code   ,    Cash_Discount_Percentage             ,           Cash_Discount_Amount          , Footer_Cash_Discount_Percentage         ,      Footer_Cash_Discount_Amount         ,         Taxable_Value                    ,    GST_Percentage                        ,         HSN_Code) " &
                                            "   Values                                 (   '" & Trim(NewCode) & "'    , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ",       @InvoiceDate      , '" & Trim(cbo_Type.Text) & "', " & Str(Val(Led_ID)) & ", " & Str(Val(Sno)) & ", '" & Trim(Itm_Id) & "'    , " & Str(Val(Sz_Id)) & " , " & Str(Val(.Rows(i).Cells(3).Value)) & ",  " & Str(Val(Unt_ID)) & "      , " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & " , '" & Trim(lbl_OrderCode.Text) & "'," & Str(Val(.Rows(i).Cells(7).Value)) & "," & Str(Val(.Rows(i).Cells(8).Value)) & "," & Str(Val(.Rows(i).Cells(9).Value)) & "," & Str(Val(.Rows(i).Cells(10).Value)) & "," & Str(Val(.Rows(i).Cells(11).Value)) & "," & Str(Val(.Rows(i).Cells(12).Value)) & ",'" & Trim(.Rows(i).Cells(13).Value) & "' ) "
                        cmd.ExecuteNonQuery()

                        'cmd.CommandText = "Insert into Stock_FP_Item_Processing_Details  (    Reference_Code        ,               Company_IdNo       ,            Reference_No        ,              for_OrderBy                                              ,          Reference_Date        ,      Sl_No                 ,  Entry_Id          ,   DeliveryTo_StockIdNo   ,  ReceivedFrom_StockIdNo   , Received_PartyIdNo         ,         Item_IdNo      ,        Size_IdNo           ,            Quantity                                ) " &
                        '                            "     Values                         ( '" & Trim(NewCode) & "'  , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "'  , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ",    @InvoiceDate      ,  " & Str(Val(Sno)) & "     , '" & EntID & "'    ," & Str(Val(Del_ID)) & "  ,  " & Str(Val(Rec_ID)) & " , " & Str(Val(Led_ID)) & "   , '" & Trim(Itm_Id) & "' , " & Str(Val(Sz_Id)) & "    , " & Str(Val(.Rows(i).Cells(3).Value)) & "      ) "
                        'cmd.ExecuteNonQuery()



                        'If Trim(UCase(cbo_Type.Text)) = "ORDER" Then

                        '    If Trim(lbl_OrderCode.Text) <> "" Then
                        '        Nr = 0
                        '        cmd.CommandText = "Update Sales_Order_Details Set Invoice_Quantity = Invoice_Quantity + " & Str(Val(.Rows(i).Cells(3).Value)) & " Where Sales_Order_Code = '" & Trim(.Rows(i).Cells(8).Value) & "' and Item_IdNo = " & Str(Val(Itm_Id)) & " and Ledger_IdNo = " & Str(Val(Led_ID))
                        '        Nr = cmd.ExecuteNonQuery()

                        '        If Nr = 0 Then
                        '            Throw New ApplicationException("Mismatch of Order and Item Details - " & .Rows(i).Cells(1).Value)
                        '            Exit Sub
                        '        End If
                        '    End If

                        'End If

                    End If

                Next

            End With

            cmd.CommandText = "Delete from Garments_Sales_GST_Tax_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Invoice_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Tax_Details

                Sno = 0
                For i = 0 To .Rows.Count - 1

                    If Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(5).Value) <> 0 Or Val(.Rows(i).Cells(7).Value) <> 0 Then

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into Garments_Sales_GST_Tax_Details   (        Sales_Invoice_Code      ,               Company_IdNo       ,                Sales_invoice_No           ,                               for_OrderBy                                  , Sales_Invoice_Date ,         Ledger_IdNo     ,            Sl_No     ,                    HSN_Code            ,                      Taxable_Amount      ,                      CGST_Percentage     ,                      CGST_Amount         ,                      SGST_Percentage      ,                      SGST_Amount         ,                      IGST_Percentage     ,                      IGST_Amount          ) " &
                                            "          Values                  ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ", @InvoiceDate , " & Str(Val(Led_ID)) & ", " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', " & Str(Val(.Rows(i).Cells(2).Value)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & " , " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & ", " & Str(Val(.Rows(i).Cells(8).Value)) & " ) "
                        cmd.ExecuteNonQuery()

                    End If

                Next i

            End With

            'eXmSG = ""
            'With dgv_OrderDetails

            '    Sno = 0

            '    For i = 0 To .RowCount - 1

            '        If Trim(UCase(cbo_Type.Text)) = "ORDER" And Val(.Rows(i).Cells(3).Value) <> 0 And Trim(.Rows(i).Cells(4).Value) <> "" Then

            '            Sno = Sno + 1

            '            eXmSG = "ItemName  :  " & Trim(.Rows(i).Cells(1).Value) & "    -    Ord.No  :  " & Trim(.Rows(i).Cells(2).Value)

            '            Itm_Id = Common_Procedures.Item_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

            '            cmd.CommandText = "Insert into Sales_Invoice_Order_Details ( Sales_Invoice_Code ,               Company_IdNo       ,     Sales_Invoice_No    ,                               for_OrderBy                                                                 , Sales_Invoice_Date              ,          Ledger_IdNo    ,          Sl_No       ,    Item_IdNo       ,             Sales_Order_No     ,                     Quantity              ,     Sales_Order_Code          ) " & _
            '                                "          Values                                (   '" & Trim(NewCode) & "'    , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ",       @InvoiceDate            , " & Str(Val(Led_ID)) & ", " & Str(Val(Sno)) & ", '" & Trim(Itm_Id) & "'    ,  '" & Trim(.Rows(i).Cells(2).Value) & "' , " & Str(Val(.Rows(i).Cells(3).Value)) & " , '" & Trim(.Rows(i).Cells(4).Value) & "' ) "
            '            cmd.ExecuteNonQuery()

            '            Nr = 0
            '            cmd.CommandText = "Update Sales_Order_Details Set Invoice_Quantity = Invoice_Quantity + " & Str(Val(.Rows(i).Cells(3).Value)) & " Where Sales_Order_Code = '" & Trim(.Rows(i).Cells(4).Value) & "' and Item_IdNo = " & Str(Val(Itm_Id)) & " and Ledger_IdNo = " & Str(Val(Led_ID))
            '            Nr = cmd.ExecuteNonQuery()

            '            If Nr = 0 Then
            '                Throw New ApplicationException("Mismatch of Order Indent Details " & Chr(13) & "Ord.No : " & .Rows(i).Cells(2).Value & "      -      Item Name : " & .Rows(i).Cells(1).Value)
            '                Exit Sub
            '            End If

            '        End If

            '    Next

            'End With

            Sno = 0
            With dgv_CartonDetails

                For i = 0 To .RowCount - 1
                    Sno = Sno + 1

                    If Trim(UCase(cbo_Type.Text)) = "PACKING SLIP" Or (Val(.Rows(i).Cells(2).Value) <> 0) And Trim(.Rows(i).Cells(3).Value) <> "" Then

                        'cmd.CommandText = "Insert into Sales_Invoice_PackingSlip_Details ( Sales_Invoice_Code ,               Company_IdNo       ,     Sales_Invoice_No    ,                     for_OrderBy                                                                      , Sales_Invoice_Date  ,           Sl_No     ,              Item_PackingSlip_No        ,                  Quantity                ,                  Item_PackingSlip_Code       ) " &
                        '                    "   Values                                      (   '" & Trim(NewCode) & "'    , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ",       @InvoiceDate            , " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', " & Str(Val(.Rows(i).Cells(2).Value)) & ",  '" & Trim(.Rows(i).Cells(3).Value) & "'   ) "
                        'cmd.ExecuteNonQuery()

                        Nr = 0
                        cmd.CommandText = "Update Garments_Item_PackingSlip_Head set Invoice_Code = '" & Trim(NewCode) & "', Invoice_Increment = Invoice_Increment + 1 Where Item_PackingSlip_Code = '" & Trim(.Rows(i).Cells(3).Value) & "' and Ledger_IdNo = " & Str(Val(Led_ID))
                        Nr = cmd.ExecuteNonQuery()

                        If Nr = 0 Then
                            MessageBox.Show("Invalid Carton Details - Mismatch of details", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            tr.Rollback()
                            If dtp_InvocieDate.Enabled And dtp_InvocieDate.Visible Then dtp_InvocieDate.Focus()
                            Exit Sub
                        End If

                    End If

                Next

            End With

            Dim vBill_No As String = ""
            If Val(Agnst_Sts) = 1 Then
                vBill_No = Trim(lbl_InvoiceNo.Text) & "/ NetRate"
            Else
                vBill_No = Trim(lbl_InvoiceNo.Text)
            End If
            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""

            If Val(CSng(lbl_NetAmount.Text)) <> 0 Then
                vLed_IdNos = Led_ID & "|" & SalAc_ID & "|24|25|26"
                vVou_Amts = -1 * (Val(CSng(lbl_NetAmount.Text))) & "|" & (Val(CSng(lbl_NetAmount.Text)) - Val(lbl_CGST_Amount.Text) - Val(lbl_SGST_Amount.Text) - Val(lbl_IGST_Amount.Text)) & "|" & Val(lbl_CGST_Amount.Text) & "|" & Val(lbl_SGST_Amount.Text) & "|" & Val(lbl_IGST_Amount.Text)
                If Common_Procedures.Voucher_Updation(con, "GST.Sales.Inv", Val(lbl_Company.Tag), Trim(NewCode), Trim(lbl_InvoiceNo.Text), dtp_InvocieDate.Value.Date, "Bill No : " & Trim(vBill_No), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                    Throw New ApplicationException(ErrMsg)
                End If
            End If


            Dim VouBil As String = ""
            VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), dtp_InvocieDate.Value.Date, Led_ID, Trim(vBill_No), Ag_ID, Val(CSng(lbl_NetAmount.Text)), "DR", Trim(NewCode), tr)
            If Trim(UCase(VouBil)) = "ERROR" Then
                Throw New ApplicationException("Error on Voucher Bill Posting")
                Exit Sub
            End If

            'If Trim(UCase(cbo_Type.Text)) = "ORDER" Then

            '    cmd.CommandText = "truncate table entrytemp"
            '    cmd.ExecuteNonQuery()

            '    cmd.CommandText = "insert into entrytemp(int1, weight1) select Item_IdNo, Quantity from Sales_Invoice_Order_Details Where Sales_Invoice_Code = '" & Trim(NewCode) & "'"
            '    cmd.ExecuteNonQuery()

            '    cmd.CommandText = "insert into entrytemp(int1, weight1) select Item_IdNo, -1*Quantity from Garments_Sales_Invoice_Details Where Sales_Invoice_Code = '" & Trim(NewCode) & "'"
            '    cmd.ExecuteNonQuery()

            '    Da = New SqlClient.SqlDataAdapter("select int1 as Itm_IdNo, sum(weight1) from entrytemp group by int1 having sum(weight1) <> 0", con)
            '    Da.SelectCommand.Transaction = tr
            '    Dt1 = New DataTable
            '    Da.Fill(Dt1)
            '    If Dt1.Rows.Count > 0 Then
            '        fpitmnm = Common_Procedures.Item_NameToIdNo(con, Dt1.Rows(0).Item("Itm_IdNo").ToString, tr)
            '        Throw New ApplicationException("Mismatch of Quantity in Invoice and Order Details" & Chr(13) & "ItemName  :  " & Trim(fpitmnm))
            '        Exit Sub
            '    End If
            '    Dt1.Clear()

            'End If

            tr.Commit()

            move_record(lbl_InvoiceNo.Text)

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception

            tr.Rollback()

            If InStr(1, Trim(LCase(ex.Message)), Trim(LCase("ck_Sales_Order_Details_1"))) > 0 Then
                MessageBox.Show("Invalid Invoice Quantity, Must be greater than zero", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            ElseIf InStr(1, Trim(LCase(ex.Message)), Trim(LCase("ck_Sales_Order_Details_2"))) > 0 Then
                MessageBox.Show("Invalid Quantity - Invocie Quantity greater than Order Quantity - " & (eXmSG), "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                'MessageBox.Show("Invalid Invoice Quantity in Order Details - " & (eXmSG), "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If

        Finally

            Dt1.Dispose()
            Da.Dispose()
            cmd.Dispose()
            tr.Dispose()

            If dtp_InvocieDate.Enabled And dtp_InvocieDate.Visible Then dtp_InvocieDate.Focus()

        End Try

    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( AccountsGroup_IdNo = 10 ) or  Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, cbo_Type, txt_ElectronicRefNo, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( AccountsGroup_IdNo = 10 ) or  Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        ' Dim AgNm As String
        Dim Led_Idno As Integer = 0
        Dim Vechile_IdNo As Integer = 0
        Dim trpt_Idno As Integer = 0

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( AccountsGroup_IdNo = 10 ) or  Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(cbo_Type.Text)) = "PACKING SLIP" Then

                If MessageBox.Show("Do you want to select Carton", "FOR CARTON SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    btn_Selection_Click(sender, e)
                Else
                    txt_ElectronicRefNo.Focus()
                End If

            Else

                'Led_Idno = Common_Procedures.Ledger_AlaisNameToIdNo(con, Trim(cbo_Ledger.Text))

                'da = New SqlClient.SqlDataAdapter("select a.* from ledger_head a where a.ledger_idno = " & Str(Val(Led_Idno)) & "  ", con)
                'dt = New DataTable
                'da.Fill(dt)

                'AgNm = ""
                'Vechile_IdNo = 0
                'trpt_Idno = 0

                'If dt.Rows.Count > 0 Then
                '    If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                '        AgNm = Common_Procedures.Ledger_IdNoToName(con, Val(dt.Rows(0)("Ledger_AgentIdNo").ToString))
                '        ' Vechile_IdNo = Val(dt.Rows(0).Item("Vechile_IdNo").ToString)
                '        trpt_Idno = Val(dt.Rows(0).Item("Transport_IdNo").ToString)
                '    End If
                'End If

                'dt.Dispose()
                'da.Dispose()

                'If Trim(AgNm) <> "" Then cbo_Agent.Text = AgNm
                ''If Trim(Vechile_IdNo) <> 0 Then cbo_VechileNo.Text = Common_Procedures.Vechile_IdNoToName(con, Val(Vechile_IdNo))
                'If Val(trpt_Idno) <> 0 Then cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, Val(trpt_Idno))

                txt_ElectronicRefNo.Focus()
            End If

            If Trim(UCase(cbo_Ledger.Tag)) <> Trim(UCase(cbo_Ledger.Text)) Then
                cbo_Ledger.Tag = cbo_Ledger.Text
                GST_Calculation()
            End If
        End If

    End Sub


    Private Sub Cbo_DelTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DeliveryTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub Cbo_DelTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DeliveryTo, cbo_Entry_Tax_Type, txt_DueDays, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub Cbo_DelTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DeliveryTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DeliveryTo, txt_DueDays, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub Cbo_DelTo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
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

    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, cbo_Through, cbo_VechileNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, cbo_VechileNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

        'If Asc(e.KeyChar) = 13 Then

        '    If MessageBox.Show("Do you want to select Packing Sip?", "FOR BALE SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
        '        btn_Selection_Click(sender, e)

        '    Else
        '        If dgv_Details.Rows.Count > 0 Then
        '            dgv_Details.Focus()
        '            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(6)
        '            dgv_Details.CurrentCell.Selected = True

        '        Else
        '            txt_DiscPerc.Focus()

        '        End If

        '    End If

        'End If

    End Sub

    Private Sub cbo_Through_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Through.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub

    Private Sub cbo_Through_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Through.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Through, cbo_SalesAc, Nothing, "", "", "", "")
        If (e.KeyValue = 40 And cbo_Transport.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If Trim(UCase(cbo_Type.Text)) = "PACKING SLIP" Then
                cbo_VechileNo.Focus()
            Else
                cbo_Transport.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Through_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Through.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Through, Nothing, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(cbo_Type.Text)) = "PACKING SLIP" Then
                cbo_VechileNo.Focus()
            Else
                cbo_Transport.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_SalesAc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SalesAc.GotFocus
        'Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28 )", "(Ledger_IdNo = 0)")

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28 )", "(Ledger_IdNo = 0)")

    End Sub
    Private Sub cbo_SalesAc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SalesAc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_SalesAc, cbo_Agent, cbo_DespTo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_SalesAc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_SalesAc.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_SalesAc, cbo_DespTo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28 )", "(Ledger_IdNo = 0)")
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
        Dim Led_IdNo As Integer, Cnt_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Cnt_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Sales_Invoice_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Sales_Invoice_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Sales_Invoice_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from Garments_Sales_Invoice_head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Invoice_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Sales_Invoice_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Sales_Invoice_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Sales_Invoice_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Total_Carton").ToString)
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Total_Quantity").ToString)
                    'dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Net_Amount").ToString), "########0.00")

                Next i

            End If

            dt2.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt2.Dispose()
            da.Dispose()

            If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()

        End Try

    End Sub


    Private Sub dtp_Filter_Fromdate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_Fromdate.KeyPress
        'If Asc(e.KeyChar) = 13 Then
        '    dtp_Filter_ToDate.Focus()
        'End If
    End Sub

    Private Sub dtp_Filter_ToDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_ToDate.KeyPress
        'If Asc(e.KeyChar) = 13 Then
        '    cbo_Filter_PartyName.Focus()
        'End If
    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( AccountsGroup_IdNo = 14 ) or  Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( AccountsGroup_IdNo = 14 ) or  Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( AccountsGroup_IdNo = 14 ) or  Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")

        If Asc(e.KeyChar) = 13 Then
            btn_Filter_Show_Click(sender, e)
        End If

    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(1).Value)

        If Val(movno) <> 0 Then
            Filter_Status = True
            Filter_RowNo = dgv_Filter_Details.CurrentRow.Index
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
        Try
            With dgv_Details
                dgv_LevColNo = e.ColumnIndex
                If .Rows.Count > 0 Then
                    If e.ColumnIndex = 5 Or e.ColumnIndex = 6 Or e.ColumnIndex = 8 Then
                        If Val(.CurrentRow.Cells(e.ColumnIndex).Value) <> 0 Then
                            .CurrentRow.Cells(e.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(e.ColumnIndex).Value), "#########0.00")
                        Else
                            .CurrentRow.Cells(e.ColumnIndex).Value = ""
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT LEAVE CELL...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Dim q As Single = 0

        Try
            With dgv_Details
                If .Visible Then
                    If .Rows.Count > 0 Then
                        If e.ColumnIndex = 3 Or e.ColumnIndex = 5 Or e.ColumnIndex = 6 Or e.ColumnIndex = 7 Or e.ColumnIndex = 8 Then


                            .CurrentRow.Cells(6).Value = Format(Val(.CurrentRow.Cells(3).Value) * Val(.CurrentRow.Cells(5).Value), "#########0.00")
                            .CurrentRow.Cells(8).Value = Format(Val(.CurrentRow.Cells(6).Value) * (Val(.CurrentRow.Cells(7).Value) / 100), "########0.00")
                            Total_Calculation()

                        End If

                    End If
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CHANGE VALUE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_ActCtrlName = dgv_Details.Name
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress

        With dgv_Details
            If .Visible Then
                If .Rows.Count > 0 Then
                    ' If .CurrentCell.ColumnIndex = 6 Then
                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                        'End If
                    End If
                End If
            End If
        End With

    End Sub
    Private Sub dgtxt_details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.TextChanged
        Try
            With dgv_Details

                If .Visible Then
                    If .Rows.Count > 0 Then

                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)

                    End If
                End If
            End With

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
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

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If LedIdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT CARTON...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        With dgv_Selection

            .Rows.Clear()

            SNo = 0

            ' Da = New SqlClient.SqlDataAdapter("select a.*,b.* from Garments_Item_PackingSlip_Head a LEFT OUTER JOIN Sales_Order_Head b ON a.Item_PackingSlip_Code = b.Sales_Order_Code  Where a.Invoice_Code = '" & Trim(NewCode) & "' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.Item_PackingSlip_Date, a.for_orderby, a.Item_PackingSlip_No", con)

            Da = New SqlClient.SqlDataAdapter("select a.* from Garments_Item_PackingSlip_Head a  Where a.Invoice_Code = '" & Trim(NewCode) & "' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.Item_PackingSlip_Date, a.for_orderby, a.Item_PackingSlip_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Item_PackingSlip_No").ToString
                    .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Order_No").ToString
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Order_date").ToString
                    .Rows(n).Cells(4).Value = Val(Dt1.Rows(i).Item("Total_Quantity").ToString)

                    .Rows(n).Cells(5).Value = "1"
                    .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Item_PackingSlip_Code").ToString
                    .Rows(n).Cells(7).Value = Common_Procedures.Ledger_IdNoToName(con, Val(Dt1.Rows(i).Item("transport_IdNo").ToString))
                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next
                    .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Despatch_To").ToString
                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Delivery_Address1").ToString
                    .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Delivery_Address2").ToString
                    ' .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Rate").ToString
                Next

            End If
            Dt1.Clear()

            'Da = New SqlClient.SqlDataAdapter("select a.*,b.* from Garments_Item_PackingSlip_Head a LEFT OUTER JOIN Sales_Order_Head b ON a.Item_PackingSlip_Code = b.Sales_Order_Code  Where a.Invoice_Code = '' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.Item_PackingSlip_Date, a.for_orderby, a.Item_PackingSlip_No", con)

            Da = New SqlClient.SqlDataAdapter("select a.* from Garments_Item_PackingSlip_Head a  Where a.Invoice_Code = '' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.Item_PackingSlip_Date, a.for_orderby, a.Item_PackingSlip_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Item_PackingSlip_No").ToString
                    .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Order_No").ToString
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Order_date").ToString
                    .Rows(n).Cells(4).Value = Val(Dt1.Rows(i).Item("Total_Quantity").ToString)

                    .Rows(n).Cells(5).Value = ""
                    .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Item_PackingSlip_Code").ToString
                    .Rows(n).Cells(7).Value = Common_Procedures.Ledger_IdNoToName(con, Val(Dt1.Rows(i).Item("Transport_IdNo").ToString))
                    '.Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Despatch_To").ToString
                    '.Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Delivery_Address1").ToString
                    '.Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Delivery_Address2").ToString
                    '.Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Rate").ToString
                Next

            End If
            Dt1.Clear()

        End With

        pnl_Selection.Visible = True
        pnl_Selection.BringToFront()
        pnl_Back.Enabled = False
        If txt_BaleNo_Selection.Enabled And txt_BaleNo_Selection.Visible Then txt_BaleNo_Selection.Focus()

    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Grid_Selection(e.RowIndex)
    End Sub

    Private Sub Grid_Selection(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(5).Value = (Val(.Rows(RwIndx).Cells(5).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(5).Value) = 0 Then

                    .Rows(RwIndx).Cells(5).Value = ""

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
        Dim n As Integer

        Try
            If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
                If dgv_Selection.CurrentCell.RowIndex >= 0 Then

                    n = dgv_Selection.CurrentCell.RowIndex

                    Grid_Selection(n)

                    e.Handled = True

                End If
            End If

        Catch ex As Exception
            '---
        End Try

    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Cmd As New SqlClient.SqlCommand
        Dim i As Integer, n As Integer
        Dim sno As Integer
        Dim Q As Single = 0
        Dim Rt As Single = 0
        Dim NewCode As String = ""
        Dim FsNo As Single = 0, LsNo As Single = 0
        Dim FsBlNo As String = "", LsBlNo As String = ""
        Dim vBl_No As String = ""
        Dim Tot_Crtn As Single = 0
        pnl_Back.Enabled = True

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        dgv_Details.Rows.Clear()
        dgv_CartonDetails.Rows.Clear()

        NoCalc_Status = True
        sno = 0

        Cmd.Connection = con

        Cmd.CommandText = "truncate table EntryTemp"
        Cmd.ExecuteNonQuery()

        Cmd.CommandText = "truncate table ReportTemp"
        Cmd.ExecuteNonQuery()

        sno = 0

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(5).Value) = 1 Then
                txt_OrderNo.Text = dgv_Selection.Rows(i).Cells(2).Value
                txt_orderdate.Text = dgv_Selection.Rows(i).Cells(3).Value
                cbo_Transport.Text = dgv_Selection.Rows(i).Cells(7).Value
                cbo_DespTo.Text = dgv_Selection.Rows(i).Cells(8).Value
                txt_DelvAdd1.Text = dgv_Selection.Rows(i).Cells(9).Value
                txt_DelvAdd2.Text = dgv_Selection.Rows(i).Cells(10).Value

                n = dgv_CartonDetails.Rows.Add()

                sno = sno + 1
                dgv_CartonDetails.Rows(n).Cells(0).Value = Val(sno)
                dgv_CartonDetails.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(1).Value
                dgv_CartonDetails.Rows(n).Cells(2).Value = Val(dgv_Selection.Rows(i).Cells(4).Value)
                '  dgv_BaleDetails.Rows(n).Cells(3).Value = Format(Val(dgv_Selection.Rows(i).Cells(3).Value), "#########0.00")
                dgv_CartonDetails.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(6).Value

                Cmd.CommandText = "insert into EntryTemp (Int1, Int2,INT3, int4 , Weight1) Select a.Company_Idno, a.Item_IdNo,a.Size_idno , a.Unit_IdNo, a.Quantity from Garments_Item_PackingSlip_Details a  where a.Item_PackingSlip_Code = '" & Trim(dgv_Selection.Rows(i).Cells(6).Value) & "'  "
                Cmd.ExecuteNonQuery()

                Cmd.CommandText = "insert into ReportTemp ( Name1 ) values ('" & Trim(dgv_Selection.Rows(i).Cells(6).Value) & "')"
                Cmd.ExecuteNonQuery()

            End If

        Next i

        Da = New SqlClient.SqlDataAdapter("select a.Int1 as Company_IdNo, a.Int2 as Item_IdNo,a.Int3 as Size_idNo ,a.int4 as Unit_idNo, b.Item_Name, c.Unit_Name, e.Size_Name, sum(a.Weight1) as qty  from EntryTemp a INNER JOIN Item_Head b ON a.Int2 = b.Item_Idno LEFT OUTER JOIN Unit_Head c ON a.Int4 = c.Unit_IdNo  LEFT OUTER JOIN Size_Head e ON a.Int3 = e.Size_Idno group by a.int1, a.Int2,a.int3,a.int4, b.Item_Name, c.Unit_Name, e.Size_Name,a.Meters2 Order by b.Item_Name, a.int1, a.Int2, a.int3 , a.int4 ,c.Unit_Name, e.Size_Name", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        sno = 0

        If Dt1.Rows.Count > 0 Then

            For i = 0 To Dt1.Rows.Count - 1

                Rt = 0

                'Da = New SqlClient.SqlDataAdapter("Select a.* from Sales_Order_Details a Where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Order_Code = '" & Trim(NewCode) & "' and a.Item_idno = " & Str(Val(Dt1.Rows(i).Item("Item_IdNo").ToString)) & " and  a.Size_idno = " & Str(Val(Dt1.Rows(i).Item("Size_idno").ToString)) & " and a.Unit_IdNo =  " & Str(Val(Dt1.Rows(i).Item("Unit_idno").ToString)) & " Order by a.sl_no", con)
                'Dt2 = New DataTable
                'Da.Fill(Dt2)

                'If Dt2.Rows.Count > 0 Then
                '    If IsDBNull(Dt2.Rows(0).Item("Rate").ToString) = False Then
                '        Rt = Val(Dt2.Rows(0).Item("Rate").ToString)
                '    End If
                'End If
                'Dt2.Clear()

                n = dgv_Details.Rows.Add()

                sno = sno + 1
                dgv_Details.Rows(n).Cells(0).Value = Val(sno)
                dgv_Details.Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Item_Name").ToString
                dgv_Details.Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Size_Name").ToString
                dgv_Details.Rows(n).Cells(3).Value = Val(Dt1.Rows(i).Item("qty").ToString)
                '  dgv_Details.Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("meters").ToString), "#########0.00")
                dgv_Details.Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Unit_Name").ToString

                'If Rt = 0 Then
                ' Rt = Val(Dt1.Rows(i).Item("Meters2").ToString)
                'End If

                dgv_Details.Rows(n).Cells(5).Value = Format(Val(Rt), "#########0.00")

                'If InStr(1, Trim(UCase(Dt1.Rows(i).Item("Unit_Name").ToString)), "MTR") > 0 Or InStr(1, Trim(UCase(Dt1.Rows(i).Item("Unit_Name").ToString)), "METER") > 0 Or InStr(1, Trim(UCase(Dt1.Rows(i).Item("Unit_Name").ToString)), "METRE") > 0 Then
                '    Q = Val(Dt1.Rows(i).Item("meters").ToString)
                'Else
                Q = Val(Dt1.Rows(i).Item("qty").ToString)
                ' End If

                dgv_Details.Rows(n).Cells(6).Value = Format(Val(Q) * Val(Rt), "#########0.00")

            Next

        End If

        vBl_No = ""
        FsNo = 0 : LsNo = 0
        FsBlNo = "" : LsBlNo = ""

        Da = New SqlClient.SqlDataAdapter("Select b.Item_PackingSlip_No, b.For_OrderBy from ReportTemp a, Garments_Item_PackingSlip_Head b where a.Name1 = b.Item_PackingSlip_Code order by b.For_OrderBy, b.Item_PackingSlip_No", con)
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

        ' txt_Noof_Carton.Text = Trim(vBl_No)
        'txt_DcNo.Text = Trim(vBl_No)

        NoCalc_Status = False
        Total_Calculation()
        If dgv_BaleDetails_Total.Rows.Count > 0 Then
            Tot_Crtn = Val(dgv_BaleDetails_Total.Rows(0).Cells(1).Value)
        End If
        txt_Noof_Carton.Text = Val(Tot_Crtn)
        Grid_Cell_DeSelect()

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False

        'If dgv_Details.Rows.Count > 0 Then
        '    dgv_Details.Focus()
        '    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(5)
        '    dgv_Details.CurrentCell.Selected = True

        'Else
        '    txt_DiscPerc.Focus()

        'End If
        If txt_ElectronicRefNo.Enabled And txt_ElectronicRefNo.Visible Then txt_ElectronicRefNo.Focus()
    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub txt_AddLess_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AddLess.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_AddLess_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_AddLess.LostFocus
        txt_AddLess.Text = Format(Val(txt_AddLess.Text), "#########0.00")
    End Sub

    Private Sub txt_AddLess_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_AddLess.TextChanged
        Total_Calculation()
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Packing_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Packing.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Packing_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Packing.LostFocus
        txt_Packing.Text = Format(Val(txt_Packing.Text), "#########0.00")
    End Sub

    Private Sub txt_Packing_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Packing.TextChanged
        Total_Calculation()
        NetAmount_Calculation()
    End Sub

    Private Sub txt_TaxPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TaxPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_VatPerc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_TaxPerc.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_DiscPerc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DiscPerc.KeyDown
        If e.KeyValue = 38 Then
            If Trim(UCase(cbo_Type.Text)) = "PACKING SLIP" Then

                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(5)
                    dgv_Details.CurrentCell.Selected = True
                Else
                    txt_DueDays.Focus()
                End If

            Else
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                    dgv_Details.CurrentCell.Selected = True

                Else
                    txt_DueDays.Focus()


                End If
            End If
        End If
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")

    End Sub

    Private Sub txt_DiscPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DiscPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_DiscPerc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_DiscPerc.TextChanged
        Total_Calculation()
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Note_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Note.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                dtp_InvocieDate.Focus()
            End If
        End If
    End Sub

    Private Sub Total_OrderItemCalculation()
        Dim Sno As Integer
        Dim TotQty As Single

        If NoCalc_Status = True Or FrmLdSTS = True Then Exit Sub

        Sno = 0
        TotQty = 0
        With dgv_OrderDetails
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Trim(.Rows(i).Cells(1).Value) <> "" And Trim(.Rows(i).Cells(2).Value) <> "" Then
                    TotQty = TotQty + Val(.Rows(i).Cells(3).Value)
                End If
            Next
        End With

        With dgv_OrderDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(3).Value = Val(TotQty)
        End With
    End Sub

    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotBls As Single, TotQty As Single
        Dim TotMtrs As Single, TotAmt As Single, TotDisAmt As Single

        If NoCalc_Status = True Or FrmLdSTS = True Then Exit Sub

        Sno = 0
        TotQty = 0 : TotMtrs = 0 : TotAmt = 0 : TotDisAmt = 0

        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Trim(.Rows(i).Cells(1).Value) <> "" And Val(.Rows(i).Cells(3).Value) <> 0 Then

                    TotQty = TotQty + Val(.Rows(i).Cells(3).Value)
                    ' TotMtrs = TotMtrs + Val(.Rows(i).Cells(4).Value)
                    TotAmt = TotAmt + Val(.Rows(i).Cells(6).Value)
                    TotDisAmt = TotDisAmt + Val(.Rows(i).Cells(8).Value)
                End If

            Next

        End With

        lbl_GrossAmount.Text = Format(Val(TotAmt) - Val(TotDisAmt), "########0.00")

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(3).Value = Val(TotQty)
            '  .Rows(0).Cells(4).Value = Format(Val(TotMtrs), "########0.00")
            .Rows(0).Cells(6).Value = Format(Val(TotAmt), "########0.00")
            .Rows(0).Cells(8).Value = Format(Val(TotDisAmt), "########0.00")
        End With

        Sno = 0
        TotBls = 0 : TotQty = 0 : TotMtrs = 0

        With dgv_CartonDetails
            For i = 0 To .RowCount - 1

                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno

                If Trim(.Rows(i).Cells(1).Value) <> "" And Val(.Rows(i).Cells(2).Value) <> 0 Then

                    TotBls = TotBls + 1
                    TotQty = TotQty + Val(.Rows(i).Cells(2).Value)
                    '  TotMtrs = TotMtrs + Val(.Rows(i).Cells(3).Value)

                End If

            Next

        End With

        With dgv_BaleDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(1).Value = Val(TotBls)
            .Rows(0).Cells(2).Value = Val(TotQty)
            '.Rows(0).Cells(3).Value = Format(Val(TotMtrs), "########0.00")
        End With

        GST_Calculation()

        NetAmount_Calculation()

    End Sub

    Private Sub NetAmount_Calculation()
        Dim NtAmt As Single
        Dim GST_Amt As Single = 0

        If NoCalc_Status = True Then Exit Sub

        GST_Amt = Val(lbl_CGST_Amount.Text) + Val(lbl_SGST_Amount.Text) + Val(lbl_IGST_Amount.Text)
        lbl_DiscAmount.Text = Format(Val(lbl_GrossAmount.Text) * Val(txt_DiscPerc.Text) / 100, "########0.00")

        lbl_AssessableValue.Text = Format(Val(lbl_GrossAmount.Text) - Val(lbl_DiscAmount.Text), "########0.00")

        lbl_TaxAmount.Text = Format(Val(lbl_AssessableValue.Text) * Val(txt_TaxPerc.Text) / 100, "########0.00")

        NtAmt = Val(lbl_AssessableValue.Text) + Val(txt_Freight.Text) + Val(txt_Packing.Text) + Val(txt_AddLess.Text) + GST_Amt
        'NtAmt = Val(lbl_GrossAmount.Text) - Val(lbl_DiscAmount.Text) + Val(txt_Freight.Text) + Val(txt_Packing.Text) + Val(txt_AddLess.Text) + GST_Amt

        lbl_NetAmount.Text = Format(Val(NtAmt), "#########0")
        lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(lbl_NetAmount.Text))

        'lbl_NetAmount.Text = Format(Val(lbl_NetAmount.Text), "#########0.00")

        lbl_RoundOff.Text = Format(Val(CSng(lbl_NetAmount.Text)) - Val(NtAmt), "#########0.00")

        lbl_AmountInWords.Text = "Rupees  :  "
        If Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            lbl_AmountInWords.Text = "Rupees  :  " & Common_Procedures.Rupees_Converstion(Val(CSng(lbl_NetAmount.Text)))
        End If

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        'pnl_Print.Visible = True
        'pnl_Back.Enabled = False
        'If btn_Print_Preprint_J.Enabled And btn_Print_Preprint_J.Visible Then
        '    btn_Print_Preprint_J.Focus()
        'End If
        printing_invoice()
    End Sub

    Public Sub printing_invoice()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim ps As Printing.PaperSize

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Garments_Sales_Invoice_head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Invoice_Code = '" & Trim(NewCode) & "'", con)
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

        prn_InpOpts = ""
        prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "                  1. Original" & Chr(13) & "                  2. Duplicate" & Chr(13) & "                  3. Triplicate" & Chr(13) & "                  4. Extra Copy" & Space(10) & "                  5. All", "FOR INVOICE PRINTING...", "1")
        prn_InpOpts = Replace(Trim(prn_InpOpts), "5", "1234")

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
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

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint

        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String
        Dim W1 As Single = 0

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_Count = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Ledger_Name as TransportName, e.Ledger_Name as Agent_Name , f.Ledger_Name as SalesAcc_Name, CSH.State_Name as Company_State_Name, CSH.State_Code as Company_State_Code, LSH.State_Name as Ledger_State_Name, LSH.State_Code as Ledger_State_Code, g.Ledger_Name as DelName , g.Ledger_Address1 as DelAdd1 ,g.Ledger_Address2 as DelAdd2, g.Ledger_Address3 as DelAdd3 ,g.Ledger_Address4 as DelAdd4,g.Ledger_GSTinNo as DelGSTinNo, g.Pan_No as DelPanNo, DSH.State_Name as DelState_Name, DSH.State_Code as Delivery_State_Code  from Garments_Sales_Invoice_head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON d.Ledger_IdNo = a.Transport_IdNo LEFT OUTER JOIN Ledger_Head e ON e.Ledger_IdNo =a.Agent_IdNo LEFT OUTER JOIN Ledger_Head f ON f.Ledger_IdNo =a.SalesAc_IdNo LEFT OUTER JOIN State_HEad CSH on b.Company_State_IdNo = CSH.State_IdNo LEFT OUTER JOIN State_HEad LSH on c.Ledger_State_IdNo = LSH.State_IdNo LEFT OUTER JOIN Ledger_Head g ON (case when a.DeliveryTo_IdNo <> 0 then a.DeliveryTo_IdNo else a.Ledger_IdNo end)= g.Ledger_IdNo LEFT OUTER JOIN State_HEad DSH on g.Ledger_State_IdNo = DSH.State_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Invoice_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Item_Name ,  c.Size_Name, d.Unit_Name from Garments_Sales_Invoice_Details a INNER JOIN Item_Head b ON  a.Item_idno = b.Item_Idno LEFT OUTER JOIN Size_Head c ON a.Size_Idno = c.Size_Idno Left Outer join Unit_Head d ON a.Unit_IdNo = d.Unit_IdNo Where a.Sales_Invoice_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                'da2 = New SqlClient.SqlDataAdapter("Select Distinct a.Sales_Invoice_Code from Garments_Sales_Invoice_Details a Where a.Sales_Invoice_Code = '" & Trim(NewCode) & "'", con)
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

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage

        If prn_HdDt.Rows.Count <= 0 Then Exit Sub
        Printing_GST_Format3(e)
        ' If prn_Status = 1 Then
        '    Printing_Format1(e)
        'ElseIf prn_Status = 2 Then
        '    Printing_Format2(e)
        'ElseIf prn_Status = 3 Then
        '    Printing_Format3(e)
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
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim ItmNm1 As String = "", ItmNm2 As String = ""
        Dim SzNm1 As String, SzNm2 As String
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer = 0
        Dim Itm_Nm As String = ""
        Dim vprn_SZNm As String = ""
        Dim Qty As Single = 0
        Dim cnt As Integer = 0
        Dim Rate As Single = 0
        Dim SzNm As String = ""

        ItmNm1 = ""
        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 40
            .Right = 65 ' 40
            .Top = 50 ' 60
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

        NoofItems_PerPage = 13 ' 8

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(50) : ClAr(2) = 320 : ClAr(3) = 80 : ClAr(4) = 80 : ClAr(5) = 70
        ClAr(6) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5))

        TxtHgt = 18.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20


        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If







                        ' Dim cmd As New SqlClient.SqlCommand

                        cmd.Connection = con
                        cmd.CommandText = "Truncate table EntryTemp"
                        cmd.ExecuteNonQuery()


                        cmd.CommandText = "Insert into EntryTemp( Name1 , Name2 ,      Name3        , Meters2       , Meters3  )    " &
                          " Select                  b.Item_Name,  c.Size_Name        , d.Unit_Name ,sum(a.Quantity) ,a.Rate  from Garments_Sales_Invoice_Details a INNER JOIN Item_Head b ON  a.Item_idno = b.Item_Idno  INNER JOIN Size_Head c ON a.Size_idNo =c.Size_idNo  INNER JOIN Unit_Head d ON a.unit_idNo = d.unit_IdNo where A.iTEM_IDnO = " & Val(prn_DetDt.Rows(prn_DetIndx).Item("Item_IdNO").ToString) & " and  A.Sales_Invoice_Code = '" & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sales_Invoice_Code").ToString) & "' Group by b.Item_Name, c.Size_Name , d.unit_Name,a.rate  Having sum(a.Quantity) <> 0"
                        cmd.ExecuteNonQuery()


                        Da1 = New SqlClient.SqlDataAdapter(" select  Name1, name2,name3,   sum(Meters2) as Qty, Meters3 from EntryTemp where meters3 =" & Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString), "###########0.00") & " group by Name1, name2,Name3 ,Meters3", con)
                        Dt1 = New DataTable
                        Da1.Fill(Dt1)
                        vprn_SZNm = ""
                        cnt = 0
                        Qty = 0
                        If Dt1.Rows.Count > 0 Then
                            cnt = Dt1.Rows.Count
                            For j = 0 To Dt1.Rows.Count - 1
                                ItmNm1 = Trim(Dt1.Rows(j).Item("name1").ToString)
                                ItmNm2 = ""
                                If Len(ItmNm1) > 25 Then
                                    For I = 25 To 1 Step -1
                                        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 25
                                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                                End If

                                If Trim((Dt1.Rows(j).Item("name2").ToString)) <> "" Then
                                    vprn_SZNm = Trim(vprn_SZNm) & IIf(Trim(vprn_SZNm) <> "", ", ", "") & Trim(Dt1.Rows(j).Item("name2").ToString)
                                End If
                                Qty = (Qty + Val(Dt1.Rows(j).Item("Qty").ToString))

                                Rate = Format(Val(Dt1.Rows(0).Item("meters3").ToString), "############0.00")
                            Next

                        End If

                        SzNm1 = Trim(vprn_SZNm)
                        SzNm2 = ""
                        If Len(SzNm1) > 10 Then
                            For I = 10 To 1 Step -1
                                If Mid$(Trim(SzNm1), I, 1) = " " Or Mid$(Trim(SzNm1), I, 1) = "," Or Mid$(Trim(SzNm1), I, 1) = "." Or Mid$(Trim(SzNm1), I, 1) = "-" Or Mid$(Trim(SzNm1), I, 1) = "/" Or Mid$(Trim(SzNm1), I, 1) = "_" Or Mid$(Trim(SzNm1), I, 1) = "(" Or Mid$(Trim(SzNm1), I, 1) = ")" Or Mid$(Trim(SzNm1), I, 1) = "\" Or Mid$(Trim(SzNm1), I, 1) = "[" Or Mid$(Trim(SzNm1), I, 1) = "]" Or Mid$(Trim(SzNm1), I, 1) = "{" Or Mid$(Trim(SzNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 10
                            SzNm2 = Microsoft.VisualBasic.Right(Trim(SzNm1), Len(SzNm1) - I)
                            SzNm1 = Microsoft.VisualBasic.Left(Trim(SzNm1), I - 1)
                        End If
                        ' If Itm_Nm <> prn_DetDt.Rows(prn_DetIndx).Item("Item_name").ToString And cnt > 1 And SzNm <> SzNm1 Then
                        If cnt > 1 And SzNm <> SzNm1 Then
                            CurY = CurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, SzNm1, LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Qty, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Dt1.Rows(0).Item("name3").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(Rate), "#############0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(Qty * Rate), "###########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                        Else

                            If cnt = 1 Then
                                CurY = CurY + TxtHgt
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, SzNm1, LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Qty, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Dt1.Rows(0).Item("name3").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)
                                ' Common_Procedures.Print_To_PrintDocument(e, Dt1.Rows(0).Item("meters3").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)


                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(Rate), "############0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Qty * Rate, PageWidth - 10, CurY, 1, 0, pFont)
                            End If
                        End If



                        NoofDets = NoofDets + 1
                        Itm_Nm = prn_DetDt.Rows(prn_DetIndx).Item("Item_Name").ToString
                        SzNm = SzNm1
                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                    'If Trim(UCase(Common_Procedures.settings.CompanyName)) = "1009" Then
                    '    CurY = CurY + TxtHgt
                    '    CurY = CurY + TxtHgt - 5
                    '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                    '    'Common_Procedures.Print_To_PrintDocument(e, "(for Jobwork Purpose Only)", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                    '    NoofDets = NoofDets + 2
                    'End If

                End If

                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

                'If Trim(prn_InpOpts) <> "" Then
                '    If prn_Count < Len(Trim(prn_InpOpts)) Then

                '        prn_DetIndx = 0
                '        prn_DetSNo = 0
                '        prn_PageNo = 0

                '        e.HasMorePages = True
                '        Return
                '    End If
                'End If

            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim CInc As Integer
        Dim CstDetAr() As String
        Dim p1Font As Font
        Dim strHeight As Single
        Dim C1 As Single, W1, C2, W2 As Single, S1, S2 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String, Desc As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String, Cmp_CstNo1 As String

        PageNo = PageNo + 1

        CurY = TMargin

        'da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Ledger_Name, d.Company_Description as Transport_Name from ClothGarments_Sales_Invoice_head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Transport_IdNo LEFT OUTER JOIN Company_Head d ON d.Company_IdNo = a.Company_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Sales_Invoice_Code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
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
        Desc = ""

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = "" : Cmp_PanNo = "" : Cmp_CstNo1 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_EMail = ""


        Desc = prn_HdDt.Rows(0).Item("Company_Description").ToString
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        Erase CstDetAr
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            CstDetAr = Split(Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString), ",")

            CInc = -1

            CInc = CInc + 1
            If UBound(CstDetAr) >= CInc Then
                Cmp_CstNo = Trim(CstDetAr(CInc))
            End If

            CInc = CInc + 1
            If UBound(CstDetAr) >= CInc Then
                Cmp_CstNo1 = Trim(CstDetAr(CInc))
            End If



        End If
        'If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
        '    Common_Procedures.Print_To_PrintDocument(e, "CST NO :" & Cmp_CstNo, LMargin + 10, CurY, 0, 0, pFont)
        '    ' Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        'End If

        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_EMail = "MAIL ID : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_PanNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 20, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        ' e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.CompanyLOGO_RD, Drawing.Image), LMargin + 20, CurY, 112, 80)
        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, PageWidth - 10, CurY, 1, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then Common_Procedures.Print_To_PrintDocument(e, "CST NO :" & Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        ' Common_Procedures.Print_To_PrintDocument(e, "" & UCase(Common_Procedures.InHouseProcess_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("InHouseProcessing_Idno").ToString))) & "  DELIVERY", LMargin, CurY, 2, PrintWidth, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo1, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3) - 40
            C2 = ClAr(1) + ClAr(2) + ClAr(3)
            W1 = e.Graphics.MeasureString("INVOICE DATE  : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width
            W2 = e.Graphics.MeasureString("Doc.Through  : ", pFont).Width
            S2 = e.Graphics.MeasureString("Sent Through  : ", pFont).Width


            CurY = CurY + 10
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "INVOICE NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_Invoice_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "INVOICE DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Invoice_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "ORDER NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "ORDER DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_Date").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "LR NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lr_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt

            If prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Tin No : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If
            Common_Procedures.Print_To_PrintDocument(e, "LR DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lr_Date").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt - 5
            '  Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            '  Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Carton").ToString), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin, CurY)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "AGENT NAME ", LMargin + 10, CurY - 10, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY - 10, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + S2 + 30, CurY - 10, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
            CurY = CurY + 10
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SIZE", LMargin + ClAr(1) + ClAr(2) - 15, CurY, 1, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "QUANTITY", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "UNIT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)

            p1Font = New Font("Rupee Foradian", 10, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "RATE (`)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT (`)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), p1Font)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            'CurY = CurY + 10
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cloth_Details").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim W1 As Single = 0
        Dim BmsInWrds As String
        Dim vprn_BlNos As String = ""
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim NetBilTxt As String = ""
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim LN_HT As Single = 0

        Try

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt

                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            'LnAr(6) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Quantity").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) - 70, CurY, LMargin + ClAr(1) + ClAr(2) - 70, LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))




            CurY = CurY + 10
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


            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "No.Of.CARTON / BAG : " & Val(prn_HdDt.Rows(0).Item("Total_Carton").ToString), LMargin + 10, CurY, 0, 0, pFont)

            If Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Discount", LMargin + ClAr(1) + ClAr(2) + 50, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin + ClAr(1) + ClAr(2) - 70, CurY)
            LN_HT = CurY

            p1Font = New Font("Calibri", 11, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "TO BE PAID FULL IN A/C : ", LMargin + 10, CurY, 0, 0, p1Font)
            If Val(prn_HdDt.Rows(0).Item("Tax_Percentage").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Tax_Type").ToString), LMargin + ClAr(1) + ClAr(2) + 50, CurY, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, "Tax Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Tax_Percentage").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Tax_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 2
            p1Font = New Font("Calibri", 11, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY, 0, 0, p1Font)
            If Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Packing ", LMargin + ClAr(1) + ClAr(2) + 50, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Packing_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 11, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY, 0, 0, p1Font)
            If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Freight", LMargin + ClAr(1) + ClAr(2) + 50, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 11, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 10, CurY, 0, 0, p1Font)
            If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add/Less", LMargin + ClAr(1) + ClAr(2) + 50, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)

            End If
            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 11, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY, 0, 0, p1Font)
            If Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "RoundOff", LMargin + ClAr(1) + ClAr(2) + 50, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            ' e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin + ClAr(1) + ClAr(2) - 70, CurY)
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            ' Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY, 0, 0, p1Font)
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)
            LnAr(8) = CurY

            ' p1Font = New Font("Calibri", 11, FontStyle.Bold)
            CurY = CurY + 10
            p1Font = New Font("Rupee Foradian", 10, FontStyle.Regular)

            'Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Quantity").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Net Amount (`)", LMargin + ClAr(1) + ClAr(2) + 50, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Net_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) - 70, LN_HT, LMargin + ClAr(1) + ClAr(2) - 70, CurY)

            LnAr(9) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))

            CurY = CurY + 10

            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
            BmsInWrds = Replace(Trim((BmsInWrds)), "", "")
            StrConv(BmsInWrds, vbProperCase)
            Common_Procedures.Print_To_PrintDocument(e, "In Words (`) : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            'CurY = CurY + TxtHgt - 5
            'p1Font = New Font("Calibri", 12, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, "Declaration", LMargin + 10, CurY, 0, 0, p1Font)
            'CurY = CurY + TxtHgt + 5
            'Common_Procedures.Print_To_PrintDocument(e, "Payment to be made direct in our Bank - Current Account:", LMargin + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "ROHIT TEXTILE", LMargin + 10, CurY, 0, 0, p1Font)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "Cetral Bank of India,Tirupur(Tamil Nadu)", LMargin + 10, CurY, 0, 0, p1Font)
            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "C/A No: 3468019247;IFSC: CBIN02890910", LMargin + 10, CurY, 0, 0, p1Font)

            p1Font = New Font("Calibri", 12, FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "1.Goods once sold will not be taken or replaced", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            ' Common_Procedures.Print_To_PrintDocument(e, "2.All dispute subject to Tirupur Jurisdiction only", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "2.All dispute subject to Tirupur Jurisdiction only ", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "3.This invoice shows the actual price of the goods ", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "4.All particulars are true and correct ", LMargin + 10, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY

            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If

            CurY = CurY + 10
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Prepared By", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 250, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)
            Common_Procedures.Print_To_PrintDocument(e, "This is a Computer Generated Invoice", LMargin, CurY, PageWidth, 2, pFont)
        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format2(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font, pFont1 As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurX As Single = 0
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim ItmDesc1 As String, ItmDesc2 As String
        'Dim ps As Printing.PaperSize
        Dim Rup1 As String, Rup2 As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim NetBilTxt As String = ""

        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 10X12", 1000, 1200)
        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        'PageSetupDialog1.PageSettings.PaperSize = ps
        '        Exit For
        '    End If
        'Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 0 ' 65
            .Right = 0 ' 50
            .Top = 10 ' 65
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
        NoofItems_PerPage = 17

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

            If prn_HdDt.Rows.Count > 0 Then

                CurX = LMargin + 55 ' 40  '150
                CurY = TMargin + 210 ' 122 ' 100
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, CurX, CurY, 0, 0, p1Font)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, CurX, CurY, 0, 0, pFont)

                If Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, CurX, CurY, 0, 0, pFont)
                End If
                If Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, CurX, CurY, 0, 0, pFont)
                End If
                If Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, CurX, CurY, 0, 0, pFont)
                End If

                If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "Tin No : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, CurX, CurY, 0, 0, pFont)
                End If

                If Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "Ph.No : " & prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString, CurX, CurY, 0, 0, pFont)
                End If

                CurX = LMargin + 580
                CurY = TMargin + 230
                p1Font = New Font("Calibri", 14, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_Invoice_No").ToString, CurX, CurY, 0, 0, p1Font)
                CurX = LMargin + 770
                Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Invoice_Date").ToString), "dd-MM-yyyy"), CurX, CurY, 0, 0, pFont)

                CurX = LMargin + 580
                CurY = TMargin + 265
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString, CurX, CurY, 0, 0, pFont)
                CurX = LMargin + 770
                Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Order_Date").ToString), "dd-MM-yyyy"), CurX, CurY, 0, 0, pFont)

                CurX = LMargin + 580
                CurY = TMargin + 295
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString, CurX, CurY, 0, 0, pFont)

                CurX = LMargin + 580
                CurY = TMargin + 325
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, CurX, CurY, 0, 0, pFont)

                CurX = LMargin + 65
                CurY = TMargin + 355
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Agent_Name").ToString, CurX, CurY, 0, 0, pFont)

                CurX = LMargin + 415
                CurY = TMargin + 355
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Through_Name").ToString, CurX, CurY, 0, 0, pFont)

                CurX = LMargin + 685
                CurY = TMargin + 355
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Carton").ToString, CurX, CurY, 0, 0, pFont)

                If prn_HdDt.Rows.Count > 0 Then

                    If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                    'If Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                    If Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 2
                    If Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                    If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1

                    Try

                        NoofDets = 0

                        CurY = TMargin + 400 ' 370

                        If prn_DetDt.Rows.Count > 0 Then

                            Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1


                                If NoofDets >= NoofItems_PerPage Then

                                    CurY = CurY + TxtHgt

                                    Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + 745, CurY, 0, 0, pFont)

                                    NoofDets = NoofDets + 1

                                    e.HasMorePages = True
                                    Return

                                End If


                                prn_DetSNo = prn_DetSNo + 1

                                ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Item_Name").ToString)
                                ItmNm2 = ""
                                If Len(ItmNm1) > 35 Then
                                    For I = 20 To 1 Step -1
                                        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 35
                                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                                End If

                                ItmDesc1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Size_Name").ToString)
                                ItmDesc2 = ""
                                If Len(ItmDesc1) > 35 Then
                                    For I = 20 To 1 Step -1
                                        If Mid$(Trim(ItmDesc1), I, 1) = " " Or Mid$(Trim(ItmDesc1), I, 1) = "," Or Mid$(Trim(ItmDesc1), I, 1) = "." Or Mid$(Trim(ItmDesc1), I, 1) = "-" Or Mid$(Trim(ItmDesc1), I, 1) = "/" Or Mid$(Trim(ItmDesc1), I, 1) = "_" Or Mid$(Trim(ItmDesc1), I, 1) = "(" Or Mid$(Trim(ItmDesc1), I, 1) = ")" Or Mid$(Trim(ItmDesc1), I, 1) = "\" Or Mid$(Trim(ItmDesc1), I, 1) = "[" Or Mid$(Trim(ItmDesc1), I, 1) = "]" Or Mid$(Trim(ItmDesc1), I, 1) = "{" Or Mid$(Trim(ItmDesc1), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 35
                                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmDesc1), Len(ItmDesc1) - I)
                                    ItmDesc1 = Microsoft.VisualBasic.Left(Trim(ItmDesc1), I - 1)
                                End If

                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetSNo)), LMargin + 40, CurY, 0, 0, pFont)
                                If ItmNm1 <> "" Then
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + 75, CurY, 0, 0, pFont)
                                Else
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmDesc1), LMargin + 75, CurY, 0, 0, pFont)
                                End If
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Quantity").ToString), LMargin + 485, CurY, 1, 0, pFont)
                                If (prn_DetDt.Rows(prn_DetIndx).Item("Unit_Name").ToString) = "MTR" Then

                                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString), "########0.00"), LMargin + 610, CurY, 1, 0, pFont)

                                Else
                                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meter_Qty").ToString), "########0.00"), LMargin + 610, CurY, 1, 0, pFont)

                                End If
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString), "########0.00"), LMargin + 730, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "########0.00"), LMargin + 860, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1
                                If ItmNm1 <> "" Then
                                    If Trim(ItmNm2) <> "" Then
                                        CurY = CurY + TxtHgt
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + 75, CurY, 0, 0, pFont)
                                        NoofDets = NoofDets + 1
                                    End If
                                Else
                                    If Trim(ItmDesc1) <> "" Then
                                        CurY = CurY + TxtHgt
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmDesc2), LMargin + 75, CurY, 0, 0, pFont)

                                        NoofDets = NoofDets + 1
                                    End If
                                End If

                                prn_DetIndx = prn_DetIndx + 1

                            Loop

                        End If

                    Catch ex As Exception

                        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                    End Try
                End If

                For I = NoofDets + 1 To NoofItems_PerPage
                    CurY = CurY + TxtHgt
                Next

                CurY = CurY + 10

                If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "Discount " & Trim(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString)) & "%", LMargin + 505, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), "########0.00"), LMargin + 860, CurY, 1, 0, pFont)
                    e.Graphics.DrawLine(Pens.Black, LMargin + 750, CurY + TxtHgt + 1, LMargin + 850, CurY + TxtHgt + 1)

                End If

                'CurY = CurY + TxtHgt



                'If Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString) <> 0 Then
                '    Common_Procedures.Print_To_PrintDocument(e, "Ass.Value ", LMargin + 505, CurY, 0, 0, pFont)
                '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "########0.00"), LMargin + 800, CurY, 1, 0, pFont)
                'End If

                If Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString) <> 0 Then

                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "Ass.Value ", LMargin + 505, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "########0.00"), LMargin + 860, CurY, 1, 0, pFont)
                    CurY = CurY + TxtHgt

                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Tax_Type").ToString) & " @ " & Trim(Val(prn_HdDt.Rows(0).Item("Tax_Percentage").ToString)) & "%", LMargin + 505, CurY, 0, 0, pFont)
                    'Common_Procedures.Print_To_PrintDocument(e, "VAT @ " & Trim(Val(prn_HdDt.Rows(0).Item("Tax_Percentage").ToString)) & "%", LMargin + 505, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString), "########0.00"), LMargin + 860, CurY, 1, 0, pFont)
                End If


                If Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "Pack Charge", LMargin + 505, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString), "########0.00"), LMargin + 860, CurY, 1, 0, pFont)
                End If


                If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "Add/Less", LMargin + 505, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString), "########0.00"), LMargin + 860, CurY, 1, 0, pFont)
                End If



                CurY = TMargin + 895

                CurX = LMargin + 75

                If Trim(prn_HdDt.Rows(0).Item("Lr_No").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "Lr No : " & Trim(prn_HdDt.Rows(0).Item("Lr_No").ToString), CurX, CurY, 0, 0, pFont)
                End If

                If Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + 505, CurY, 0, 0, pFont)
                    If Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString) >= 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + 610, CurY, 0, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + 610, CurY, 0, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, " " & Format(Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString), "########0.00"), LMargin + 860, CurY, 1, 0, pFont)
                End If

                CurY = TMargin + 950
                p1Font = New Font("Calibri", 11, FontStyle.Bold)

                NetBilTxt = ""
                If IsDBNull(prn_HdDt.Rows(0).Item("AgainstForm_Status").ToString) = False Then
                    If Val(prn_HdDt.Rows(0).Item("AgainstForm_Status").ToString) = 1 Then NetBilTxt = "NET BILL"
                End If

                Common_Procedures.Print_To_PrintDocument(e, NetBilTxt, LMargin + 75, CurY, 0, 0, p1Font)

                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Quantity").ToString), LMargin + 485, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString), "########0.00"), LMargin + 860, CurY, 1, 0, p1Font)

            End If

            Rup1 = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
            Rup2 = ""
            If Len(Rup1) > 70 Then
                For I = 70 To 1 Step -1
                    If Mid$(Trim(Rup1), I, 1) = " " Then Exit For
                Next I
                If I = 0 Then I = 70
                Rup2 = Microsoft.VisualBasic.Right(Trim(Rup1), Len(Rup1) - I)
                Rup1 = Microsoft.VisualBasic.Left(Trim(Rup1), I - 1)
            End If

            CurX = LMargin + 130
            CurY = TMargin + 990
            Common_Procedures.Print_To_PrintDocument(e, Rup1, CurX, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Rup2, CurX, CurY, 0, 0, pFont)
            CurY = TMargin + 1080

            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Prepared_By").ToString, LMargin + 420, CurY, 1, 0, pFont)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub
    Private Sub Printing_Format3(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font, pFont1 As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurX As Single = 0
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim ItmDesc1 As String, ItmDesc2 As String
        'Dim ps As Printing.PaperSize
        Dim Rup1 As String, Rup2 As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim NetBilTxt As String = ""
        Dim ps As Printing.PaperSize
        Dim PpSzSTS As Boolean = False
        Dim S As String

        'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 10X12", 1000, 1200)
        'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        'PageSetupDialog1.PageSettings.PaperSize = ps
        '        Exit For
        '    End If
        'Next

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            'Debug.Print(ps.PaperName)
            If ps.Width = 800 And ps.Height = 600 Then
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PpSzSTS = True
                Exit For
            End If
        Next

        If PpSzSTS = False Then
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    PpSzSTS = True
                    Exit For
                End If
            Next

            If PpSzSTS = False Then
                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                        PrintDocument1.DefaultPageSettings.PaperSize = ps
                        Exit For
                    End If
                Next
            End If

        End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 15 ' 65
            .Right = 0 ' 50
            .Top = 10 ' 65
            .Bottom = 0 ' 50
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

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
        NoofItems_PerPage = 17

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
            'Exit Sub
            ' CurX = LMargin + 65 ' 40  '150
            CurY = TMargin  ' 122 ' 100
            If Trim(prn_OriDupTri) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 50, CurY, 1, 0, pFont)
            End If

            If prn_HdDt.Rows.Count > 0 Then

                CurX = LMargin + 65 ' 40  '150
                CurY = TMargin + 205
                ' 122 ' 100
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, CurX, CurY, 0, 0, p1Font)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, CurX, CurY, 0, 0, pFont)

                If Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, CurX, CurY, 0, 0, pFont)
                End If
                If Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, CurX, CurY, 0, 0, pFont)
                End If
                If Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, CurX, CurY, 0, 0, pFont)
                End If

                If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "Tin No : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, CurX, CurY, 0, 0, pFont)
                End If

                If Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "Ph.No : " & prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString, CurX, CurY, 0, 0, pFont)
                End If

                CurX = LMargin + 580
                CurY = TMargin + 210
                p1Font = New Font("Calibri", 14, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_Invoice_No").ToString, CurX, CurY, 0, 0, p1Font)
                CurX = LMargin + 670
                Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Invoice_Date").ToString), "dd-MM-yyyy"), CurX, CurY, 0, 0, pFont)

                CurX = LMargin + 580
                CurY = TMargin + 250
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString, CurX, CurY, 0, 0, pFont)
                CurX = LMargin + 670
                Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Order_Date").ToString), "dd-MM-yyyy"), CurX, CurY, 0, 0, pFont)

                CurX = LMargin + 580
                CurY = TMargin + 285
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString, CurX, CurY, 0, 0, pFont)

                CurX = LMargin + 580
                CurY = TMargin + 315
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, CurX, CurY, 0, 0, pFont)

                CurX = LMargin + 60
                CurY = TMargin + 345
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Agent_Name").ToString, CurX, CurY, 0, 0, pFont)

                'CurX = LMargin + 415
                'CurY = TMargin + 355
                'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Through_Name").ToString, CurX, CurY, 0, 0, pFont)

                CurX = LMargin + 560
                CurY = TMargin + 345
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Carton").ToString, CurX, CurY, 0, 0, pFont)

                If prn_HdDt.Rows.Count > 0 Then

                    If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                    'If Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                    If Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 2
                    If Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                    If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1

                    Try

                        NoofDets = 0

                        CurY = TMargin + 420 ' 370

                        If prn_DetDt.Rows.Count > 0 Then

                            Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1


                                If NoofDets >= NoofItems_PerPage Then

                                    CurY = CurY + TxtHgt

                                    Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + 745, CurY, 0, 0, pFont)

                                    NoofDets = NoofDets + 1

                                    e.HasMorePages = True
                                    Return

                                End If


                                prn_DetSNo = prn_DetSNo + 1

                                ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Item_Name").ToString)
                                ItmNm2 = ""
                                If Len(ItmNm1) > 35 Then
                                    For I = 20 To 1 Step -1
                                        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 35
                                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                                End If

                                ItmDesc1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Size_Name").ToString)
                                ItmDesc2 = ""
                                If Len(ItmDesc1) > 35 Then
                                    For I = 20 To 1 Step -1
                                        If Mid$(Trim(ItmDesc1), I, 1) = " " Or Mid$(Trim(ItmDesc1), I, 1) = "," Or Mid$(Trim(ItmDesc1), I, 1) = "." Or Mid$(Trim(ItmDesc1), I, 1) = "-" Or Mid$(Trim(ItmDesc1), I, 1) = "/" Or Mid$(Trim(ItmDesc1), I, 1) = "_" Or Mid$(Trim(ItmDesc1), I, 1) = "(" Or Mid$(Trim(ItmDesc1), I, 1) = ")" Or Mid$(Trim(ItmDesc1), I, 1) = "\" Or Mid$(Trim(ItmDesc1), I, 1) = "[" Or Mid$(Trim(ItmDesc1), I, 1) = "]" Or Mid$(Trim(ItmDesc1), I, 1) = "{" Or Mid$(Trim(ItmDesc1), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 35
                                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmDesc1), Len(ItmDesc1) - I)
                                    ItmDesc1 = Microsoft.VisualBasic.Left(Trim(ItmDesc1), I - 1)
                                End If

                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetSNo)), LMargin + 20, CurY, 0, 0, pFont)
                                If ItmNm1 <> "" Then
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + 65, CurY, 0, 0, pFont)
                                Else
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmDesc1), LMargin + 65, CurY, 0, 0, pFont)
                                End If
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Quantity").ToString), LMargin + 530, CurY, 1, 0, pFont)
                                If (prn_DetDt.Rows(prn_DetIndx).Item("Unit_Name").ToString) = "MTR" Then

                                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString), "########0.00"), LMargin + 450, CurY, 1, 0, pFont)

                                Else
                                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meter_Qty").ToString), "########0.00"), LMargin + 450, CurY, 1, 0, pFont)

                                End If
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString), "########0.00"), LMargin + 640, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "########0.00"), LMargin + 760, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1
                                If ItmNm1 <> "" Then
                                    If Trim(ItmNm2) <> "" Then
                                        CurY = CurY + TxtHgt
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + 65, CurY, 0, 0, pFont)
                                        NoofDets = NoofDets + 1
                                    End If
                                Else
                                    If Trim(ItmDesc1) <> "" Then
                                        CurY = CurY + TxtHgt
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmDesc2), LMargin + 65, CurY, 0, 0, pFont)

                                        NoofDets = NoofDets + 1
                                    End If
                                End If

                                prn_DetIndx = prn_DetIndx + 1

                            Loop

                        End If

                    Catch ex As Exception

                        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                    End Try
                End If

                For I = NoofDets + 1 To NoofItems_PerPage
                    CurY = CurY + TxtHgt
                Next

                CurY = CurY + 10

                If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "Discount " & Trim(Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString)) & "%", LMargin + 505, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), "########0.00"), LMargin + 760, CurY, 1, 0, pFont)
                    e.Graphics.DrawLine(Pens.Black, LMargin + 750, CurY + TxtHgt + 1, LMargin + 850, CurY + TxtHgt + 1)

                End If

                'CurY = CurY + TxtHgt
                'If Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString) <> 0 Then
                '    Common_Procedures.Print_To_PrintDocument(e, "Ass.Value ", LMargin + 505, CurY, 0, 0, pFont)
                '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "########0.00"), LMargin + 800, CurY, 1, 0, pFont)
                'End If

                If Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString) <> 0 Then

                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "Ass.Value ", LMargin + 505, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "########0.00"), LMargin + 760, CurY, 1, 0, pFont)
                    CurY = CurY + TxtHgt

                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Tax_Type").ToString) & " @ " & Trim(Val(prn_HdDt.Rows(0).Item("Tax_Percentage").ToString)) & "%", LMargin + 505, CurY, 0, 0, pFont)
                    'Common_Procedures.Print_To_PrintDocument(e, "VAT @ " & Trim(Val(prn_HdDt.Rows(0).Item("Tax_Percentage").ToString)) & "%", LMargin + 505, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString), "########0.00"), LMargin + 760, CurY, 1, 0, pFont)
                End If


                If Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "Pack Charge", LMargin + 505, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString), "########0.00"), LMargin + 760, CurY, 1, 0, pFont)
                End If


                If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "Add/Less", LMargin + 505, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString), "########0.00"), LMargin + 760, CurY, 1, 0, pFont)
                End If



                CurY = TMargin + 895

                CurX = LMargin + 75

                If Trim(prn_HdDt.Rows(0).Item("Lr_No").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "Lr No : " & Trim(prn_HdDt.Rows(0).Item("Lr_No").ToString), CurX, CurY, 0, 0, pFont)
                End If

                If Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + 505, CurY, 0, 0, pFont)
                    If Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString) >= 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + 610, CurY, 0, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + 610, CurY, 0, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, " " & Format(Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString), "########0.00"), LMargin + 760, CurY, 1, 0, pFont)
                End If

                CurY = TMargin + 950
                p1Font = New Font("Calibri", 11, FontStyle.Bold)

                NetBilTxt = ""
                If IsDBNull(prn_HdDt.Rows(0).Item("AgainstForm_Status").ToString) = False Then
                    If Val(prn_HdDt.Rows(0).Item("AgainstForm_Status").ToString) = 1 Then NetBilTxt = "NET BILL"
                End If

                Common_Procedures.Print_To_PrintDocument(e, NetBilTxt, LMargin + 75, CurY, 0, 0, p1Font)

                Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Quantity").ToString), LMargin + 540, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString), "########0.00"), LMargin + 760, CurY, 1, 0, p1Font)

            End If

            Rup1 = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
            Rup2 = ""
            If Len(Rup1) > 70 Then
                For I = 70 To 1 Step -1
                    If Mid$(Trim(Rup1), I, 1) = " " Then Exit For
                Next I
                If I = 0 Then I = 70
                Rup2 = Microsoft.VisualBasic.Right(Trim(Rup1), Len(Rup1) - I)
                Rup1 = Microsoft.VisualBasic.Left(Trim(Rup1), I - 1)
            End If

            CurX = LMargin + 130
            CurY = TMargin + 995
            Common_Procedures.Print_To_PrintDocument(e, Rup1, CurX, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Rup2, CurX, CurY, 0, 0, pFont)
            CurY = TMargin + 1060

            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Prepared_By").ToString, LMargin + 565, CurY, 1, 0, pFont)

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

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub
    Private Sub cbo_Area_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VechileNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Vechile_Head", "Vechile_Name", "", "(Vechile_IdNo = 0)")

    End Sub

    Private Sub cbo_Area_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VechileNo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VechileNo, Nothing, txt_Carton_Weight, "Vechile_Head", "Vechile_Name", "", "(Vechile_IdNo = 0)")
        If (e.KeyValue = 38 And cbo_VechileNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If Trim(UCase(cbo_Type.Text)) = "PACKING SLIP" Then
                cbo_Through.Focus()
            Else
                cbo_Transport.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Area_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VechileNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VechileNo, txt_Carton_Weight, "Vechile_Head", "Vechile_Name", "", "(Vechile_IdNo = 0)", False)
    End Sub

    Private Sub cbo_Agent_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Agent.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( AccountsGroup_IdNo = 14 ) or  Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")

    End Sub

    Private Sub cbo_Agent_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Agent, txt_lrDate, cbo_SalesAc, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( AccountsGroup_IdNo = 14 ) or  Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Agent_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Agent.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Agent, cbo_SalesAc, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( AccountsGroup_IdNo = 14 ) or  Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_VatAc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VatAc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 12)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_VatAc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VatAc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VatAc, txt_Freight, txt_AddLess, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 12)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_VatAc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VatAc.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VatAc, txt_AddLess, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 12)", "(Ledger_IdNo = 0)")
    End Sub


    Private Sub cbo_Area_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VechileNo.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New VehicleNo_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_VechileNo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Agent_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Agent.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub


    Private Sub cbo_Transport_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

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


    Private Sub txt_BaleNo_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_BaleNo_Selection.KeyDown
        If e.KeyValue = 40 Then
            If dgv_Selection.Rows.Count > 0 Then
                dgv_Selection.Focus()
                dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
                dgv_Selection.CurrentCell.Selected = True
            End If
        End If
    End Sub

    Private Sub txt_BaleNo_Selection_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_BaleNo_Selection.KeyPress

        If Asc(e.KeyChar) = 13 Then

            If Trim(txt_BaleNo_Selection.Text) <> "" Then
                btn_SelectBale_Click(sender, e)

            Else
                If dgv_Selection.Rows.Count > 0 Then
                    dgv_Selection.Focus()
                    dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
                    dgv_Selection.CurrentCell.Selected = True
                End If

            End If

        End If

    End Sub

    Private Sub btn_SelectBale_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_SelectBale.Click
        Dim BlNo As String
        Dim i As Integer

        If Trim(txt_BaleNo_Selection.Text) <> "" Then

            BlNo = Trim(txt_BaleNo_Selection.Text)

            For i = 0 To dgv_Selection.Rows.Count - 1
                If Trim(UCase(BlNo)) = Trim(UCase(dgv_Selection.Rows(i).Cells(1).Value)) Then
                    Call Grid_Selection(i)
                    Exit For
                End If
            Next

            txt_BaleNo_Selection.Text = ""

        End If

    End Sub

    Private Sub dgv_BaleDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_CartonDetails.CellEnter
        dgv_ActCtrlName = dgv_CartonDetails.Name
    End Sub

    Private Sub dgv_BaleDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_CartonDetails.KeyDown
        On Error Resume Next

        With dgv_CartonDetails

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
                    txt_DiscPerc.Focus()
                End If
            End If

        End With

    End Sub

    Private Sub dgv_BaleDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_CartonDetails.LostFocus
        On Error Resume Next
        dgv_CartonDetails.CurrentCell.Selected = False
    End Sub

    Private Sub cbo_TaxType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub

    Private Sub cbo_TaxType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TaxType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TaxType, txt_Carton_Weight, cbo_DeliveryTo, "", "", "", "")
    End Sub

    Private Sub cbo_TaxType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TaxType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TaxType, cbo_DeliveryTo, "", "", "", "")
    End Sub

    Private Sub cbo_TaxType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.TextChanged
        If Trim(UCase(cbo_TaxType.Text)) = "" Or Trim(UCase(cbo_TaxType.Text)) = "-NIL-" Then txt_TaxPerc.Text = ""
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

    Private Sub btn_Print_Preprint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Preprint_J.Click
        prn_Status = 2
        printing_invoice()
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub cbo_Type_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Type, dtp_InvocieDate, cbo_Ledger, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Type, cbo_Ledger, "", "", "", "")
    End Sub

    Private Sub btn_OrderSelection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_OrderSelection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim I As Integer, J As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim CompIDCondt As String = ""
        Dim Ent_OrdCd As String = ""
        Dim Ent_Qty As Single = 0
        Dim Ent_rte As Single = 0
        Dim Ent_amt As Single = 0
        Dim Ent_Mtrs As Single = 0
        Dim nr As Single = 0

        If Trim(UCase(cbo_Type.Text)) <> "ORDER" And Trim(UCase(cbo_Type.Text)) <> "PACKING SLIP" Then Exit Sub

        If Trim(UCase(cbo_Type.Text)) <> "ORDER" And Trim(UCase(cbo_Type.Text)) <> "PACKING SLIP" Then
            MessageBox.Show("Invalid Invoice Type", "DOES NOT SELECT ORDER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Type.Enabled And cbo_Type.Visible Then cbo_Type.Focus()
            Exit Sub
        End If




        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
        If LedIdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT ORDER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Trim(UCase(cbo_Type.Text)) = "ORDER" Then

            With dgv_OrderSelection

                ' lbl_Heading_Selection.Text = "ORDER SELECTION"

                .Rows.Clear()

                SNo = 0

                '---1
                Da = New SqlClient.SqlDataAdapter("Select a.*, e.Ledger_Name as Transportname, f.Ledger_Name as Agentname ," &
                                                    " (select sum(z2.Order_Quantity - z2.Invoice_Quantity) as Balance_Qty from Sales_Order_Details z2 where z2.Sales_Order_Code = a.Sales_Order_Code ) as Balance_Qty, " &
                                                    " (select sum(z3.Quantity) from Sales_Invoice_Order_Details z3 where z3.Sales_Invoice_Code = '" & Trim(NewCode) & "' and z3.Sales_Order_Code = a.Sales_Order_Code ) as Ent_Qty " &
                                                    " from Sales_Order_Head a " &
                                                    " LEFT OUTER JOIN Ledger_Head e ON e.Ledger_IdNo <> 0 and a.Transport_IdNo = e.Ledger_IdNo " &
                                                    " LEFT OUTER JOIN Ledger_Head f ON f.Ledger_IdNo <> 0 and a.Agent_IdNo = f.Ledger_IdNo " &
                                                    " Where " &
                                                    " a.ledger_Idno = " & Str(Val(LedIdNo)) & " and a.Sales_Order_Code IN (select z1.Sales_Order_Code from Sales_Invoice_Order_Details z1 Where z1.Sales_Invoice_Code = '" & Trim(NewCode) & "' ) " &
                                                    " order by a.Sales_Order_Date, a.for_orderby, a.Sales_Order_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                Ent_OrdCd = "'0'"

                If Dt1.Rows.Count > 0 Then

                    For I = 0 To Dt1.Rows.Count - 1

                        'BalQty = 0
                        'Da = New SqlClient.SqlDataAdapter("select sum(Quantity - Invoice_Quantity) as Balance_Qty from Garments_Sales_Invoice_Details z1 where Sales_Invoice_Code = '" & Trim(Dt1.Rows(i).Item("Sales_Invoice_Code").ToString) & "'  ", con)
                        'Dt2 = New DataTable
                        'nr = Da.Fill(Dt2)
                        'If Dt2.Rows.Count > 0 Then
                        '    BalQty = Val(Dt1.Rows(i).Item("Balance_Qty").ToString)
                        'End If
                        'dt2.clear()

                        n = .Rows.Add()

                        Ent_OrdCd = Trim(Ent_OrdCd) & IIf(Trim(Ent_OrdCd) <> "", ", ", "") & "'" & Dt1.Rows(I).Item("Sales_Order_Code").ToString & "'"

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(I).Item("Sales_Order_No").ToString
                        .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(I).Item("Sales_Order_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(3).Value = Dt1.Rows(I).Item("Order_No").ToString
                        .Rows(n).Cells(4).Value = Val(Dt1.Rows(I).Item("Total_Quantity").ToString)
                        .Rows(n).Cells(5).Value = Val(Dt1.Rows(I).Item("Balance_Qty").ToString) + Val(Dt1.Rows(I).Item("Ent_Qty").ToString)
                        .Rows(n).Cells(6).Value = "1"
                        .Rows(n).Cells(7).Value = Dt1.Rows(I).Item("Agentname").ToString
                        .Rows(n).Cells(8).Value = Dt1.Rows(I).Item("Through_Name").ToString
                        .Rows(n).Cells(9).Value = Dt1.Rows(I).Item("Area_Name").ToString
                        .Rows(n).Cells(10).Value = Dt1.Rows(I).Item("Transportname").ToString
                        .Rows(n).Cells(11).Value = Dt1.Rows(I).Item("Sales_Order_Code").ToString

                        For J = 0 To .ColumnCount - 1
                            .Rows(I).Cells(J).Style.ForeColor = Color.Red
                        Next

                    Next

                End If
                Dt1.Clear()

                Ent_OrdCd = "(" & Trim(Ent_OrdCd) & ")"

                '---2
                Da = New SqlClient.SqlDataAdapter("Select a.*, e.Ledger_Name as Transportname, f.Ledger_Name as Agentname, " &
                                                    " (select sum(z2.Order_Quantity - z2.Invoice_Quantity) as Balance_Qty from Sales_Order_Details z2 where z2.Sales_Order_Code = a.Sales_Order_Code ) as Balance_Qty " &
                                                    " from Sales_Order_Head a " &
                                                    " LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo " &
                                                    " LEFT OUTER JOIN Ledger_Head f ON a.Agent_IdNo = f.Ledger_IdNo " &
                                                    " Where a.ledger_Idno = " & Str(Val(LedIdNo)) & " and a.Sales_Order_Code IN (select z1.Sales_Order_Code from Sales_Order_Details z1 where z1.Sales_Order_Code NOT IN " & Trim(Ent_OrdCd) & " and (z1.Order_Quantity - z1.Invoice_Quantity) > 0 ) " &
                                                    " Order by a.Sales_Order_Date, a.for_orderby, a.Sales_Order_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For I = 0 To Dt1.Rows.Count - 1

                        'BalQty = 0
                        'Da = New SqlClient.SqlDataAdapter("select sum(Invoice_Quantity) as Balance_Qty from Garments_Sales_Invoice_Details z1 where Sales_Invoice_Code = '" & Trim(Dt1.Rows(i).Item("Sales_Invoice_Code").ToString) & "'  ", con)
                        'Dt2 = New DataTable
                        'nr = Da.Fill(Dt2)
                        'If Dt2.Rows.Count > 0 Then
                        '    BalQty = Val(Dt1.Rows(i).Item("Balance_Qty").ToString)
                        'End If
                        'dt2.clear()

                        n = .Rows.Add()

                        Ent_OrdCd = Trim(Ent_OrdCd) & IIf(Trim(Ent_OrdCd) <> "", ", ", "") & "'" & Dt1.Rows(I).Item("Sales_Order_Code").ToString & "'"

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(I).Item("Sales_Order_No").ToString
                        .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(I).Item("Sales_Order_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(3).Value = Dt1.Rows(I).Item("Party_OrderNo").ToString
                        .Rows(n).Cells(4).Value = Val(Dt1.Rows(I).Item("Total_Order_Quantity").ToString)
                        .Rows(n).Cells(5).Value = Val(Dt1.Rows(I).Item("Balance_Qty").ToString)
                        .Rows(n).Cells(6).Value = ""
                        .Rows(n).Cells(7).Value = Dt1.Rows(I).Item("Agentname").ToString
                        .Rows(n).Cells(8).Value = Dt1.Rows(I).Item("Through_Name").ToString
                        ' .Rows(n).Cells(9).Value = Dt1.Rows(I).Item("Area_Name").ToString
                        .Rows(n).Cells(10).Value = Dt1.Rows(I).Item("Transportname").ToString
                        .Rows(n).Cells(11).Value = Dt1.Rows(I).Item("Sales_Order_Code").ToString

                    Next

                End If
                Dt1.Clear()
                If .Rows.Count = 0 Then .Rows.Add()

                pnl_OrderSelection.Visible = True
                pnl_Back.Enabled = False

                .Focus()
                .CurrentCell = .Rows(0).Cells(0)
                .CurrentCell.Selected = True

            End With

        ElseIf Trim(UCase(cbo_Type.Text)) = "PACKING SLIP" Then

            With dgv_Selection

                .Rows.Clear()

                SNo = 0

                Da = New SqlClient.SqlDataAdapter("select a.* from Garments_Item_PackingSlip_Head a Where a.Invoice_Code = '" & Trim(NewCode) & "' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.Item_PackingSlip_Date, a.for_orderby, a.Item_PackingSlip_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For I = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(I).Item("Item_PackingSlip_No").ToString
                        .Rows(n).Cells(2).Value = Val(Dt1.Rows(I).Item("Total_Quantity").ToString)
                        '   .Rows(n).Cells(3).Value = Format(Val(Dt1.Rows(i).Item("Total_Meters").ToString), "#########0.00")
                        .Rows(n).Cells(3).Value = "1"
                        .Rows(n).Cells(4).Value = Dt1.Rows(I).Item("Item_PackingSlip_Code").ToString

                        For J = 0 To .ColumnCount - 1
                            .Rows(I).Cells(J).Style.ForeColor = Color.Red
                        Next

                    Next

                End If
                Dt1.Clear()

                Da = New SqlClient.SqlDataAdapter("select a.* from Garments_Item_PackingSlip_Head a Where a.Invoice_Code = '' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.Item_PackingSlip_Date, a.for_orderby, a.Item_PackingSlip_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For I = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(I).Item("Item_PackingSlip_No").ToString
                        .Rows(n).Cells(2).Value = Val(Dt1.Rows(I).Item("Total_Quantity").ToString)
                        '  .Rows(n).Cells(3).Value = Format(Val(Dt1.Rows(i).Item("Total_Meters").ToString), "#########0.00")
                        .Rows(n).Cells(3).Value = ""
                        .Rows(n).Cells(4).Value = Dt1.Rows(I).Item("Item_PackingSlip_Code").ToString

                    Next

                End If
                Dt1.Clear()

            End With

            pnl_Selection.Visible = True
            pnl_Selection.BringToFront()
            pnl_Back.Enabled = False
            If txt_BaleNo_Selection.Enabled And txt_BaleNo_Selection.Visible Then txt_BaleNo_Selection.Focus()

        End If
    End Sub

    Private Sub dgv_OrderSelection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_OrderSelection.CellClick
        If dgv_OrderSelection.Rows.Count > 0 And e.RowIndex >= 0 Then
            Select_Order(e.RowIndex)
        End If
    End Sub

    Private Sub Select_Order(ByVal RwIndx As Integer)
        Dim i As Integer = 0

        With dgv_OrderSelection

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


            'If .RowCount > 0 And RwIndx >= 0 Then

            '    For i = 0 To dgv_Selection.Rows.Count - 1
            '        dgv_Selection.Rows(i).Cells(6).Value = ""
            '    Next

            '    .Rows(RwIndx).Cells(6).Value = 1

            '    If Val(.Rows(RwIndx).Cells(6).Value) = 1 Then

            '        For i = 0 To .ColumnCount - 1
            '            .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
            '        Next


            '    Else
            '        .Rows(RwIndx).Cells(6).Value = ""

            '        For i = 0 To .ColumnCount - 1
            '            .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
            '        Next

            '    End If

            '    FP_Invoice_Selection()

            'End If

        End With

    End Sub

    Private Sub dgv_OrderSelection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_OrderSelection.KeyDown
        Dim n As Integer = 0

        Try
            If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
                If dgv_OrderSelection.CurrentCell.RowIndex >= 0 Then

                    n = dgv_OrderSelection.CurrentCell.RowIndex

                    Select_Order(n)

                    e.Handled = True

                End If
            End If

        Catch ex As Exception
            '---

        End Try

    End Sub

    Private Sub btn_Close_OrderSelection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_OrderSelection.Click
        FP_Invoice_Selection()
    End Sub

    Private Sub FP_Invoice_Selection()
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim OrdSNo As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim k As Integer = 0
        Dim Dup_OrdNo As String = ""
        Dim Dup_OrdDt As String = ""
        Dim Ent_Qty As Single = 0
        Dim NewCode As String = ""

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        pnl_Back.Enabled = True
        pnl_OrderSelection.Visible = False

        dgv_OrderDetails.Rows.Clear()

        If dgv_OrderSelection.Rows.Count > 0 Then

            Dup_OrdNo = ""
            Dup_OrdDt = ""
            OrdSNo = 0


            txt_OrderNo.Text = ""
            lbl_OrderCode.Text = ""
            txt_orderdate.Text = ""
            cbo_Agent.Text = ""
            cbo_VechileNo.Text = ""
            cbo_Through.Text = ""
            cbo_Transport.Text = ""


            For i = 0 To dgv_OrderSelection.Rows.Count - 1

                If Val(dgv_OrderSelection.Rows(i).Cells(6).Value) = 1 Then

                    If InStr(1, Trim(UCase(Dup_OrdNo)), "~" & Trim(UCase(dgv_OrderSelection.Rows(i).Cells(3).Value)) & "~") = 0 Then
                        txt_OrderNo.Text = Trim(txt_OrderNo.Text) & IIf(Trim(txt_OrderNo.Text) <> "", ", ", "") & Trim(dgv_OrderSelection.Rows(i).Cells(3).Value)
                        lbl_OrderCode.Text = Trim(lbl_OrderCode.Text) & IIf(Trim(lbl_OrderCode.Text) <> "", ", ", "") & Trim(dgv_OrderSelection.Rows(i).Cells(11).Value)
                        Dup_OrdNo = Dup_OrdNo & "~" & Trim(UCase(dgv_OrderSelection.Rows(i).Cells(3).Value)) & "~"
                    End If

                    If Trim(Dup_OrdDt) = "" Then
                        txt_orderdate.Text = Trim(dgv_OrderSelection.Rows(i).Cells(2).Value)
                        cbo_Agent.Text = Trim(dgv_OrderSelection.Rows(i).Cells(7).Value)
                        cbo_VechileNo.Text = Trim(dgv_OrderSelection.Rows(i).Cells(9).Value)
                        cbo_Through.Text = Trim(dgv_OrderSelection.Rows(i).Cells(8).Value)
                        cbo_Transport.Text = Trim(dgv_OrderSelection.Rows(i).Cells(10).Value)

                        Dup_OrdDt = Trim(dgv_OrderSelection.Rows(i).Cells(2).Value)

                    End If


                    With dgv_OrderDetails

                        Da = New SqlClient.SqlDataAdapter("select a.*, c.Item_Name from Sales_Order_Details a INNER JOIN Item_Head c ON c.Item_Idno <> 0 and c.Item_Idno = a.Item_idno Where a.Sales_Order_Code = '" & Trim(dgv_OrderSelection.Rows(i).Cells(11).Value) & "' order by a.Sales_Order_Date, a.for_orderby, a.Sales_Order_No, c.Item_Name", con)
                        Dt2 = New DataTable
                        Da.Fill(Dt2)

                        If Dt2.Rows.Count > 0 Then

                            For k = 0 To Dt2.Rows.Count - 1

                                Ent_Qty = Val(Dt2.Rows(k).Item("Order_Quantity").ToString) - Val(Dt2.Rows(k).Item("Invoice_Quantity").ToString)

                                Da = New SqlClient.SqlDataAdapter("select a.* from Sales_Invoice_Order_Details a Where a.Sales_Invoice_Code = '" & Trim(NewCode) & "' and a.Sales_Order_Code = '" & Trim(Dt2.Rows(k).Item("Sales_Order_Code").ToString) & "'  and a.Item_idno = " & Str(Val(Dt2.Rows(k).Item("Item_idno").ToString)) & " order by a.Sl_No", con)
                                Dt3 = New DataTable
                                Da.Fill(Dt3)
                                If Dt3.Rows.Count > 0 Then
                                    Ent_Qty = Val(Dt3.Rows(0).Item("Quantity").ToString)
                                End If
                                Dt3.Clear()

                                If Ent_Qty <> 0 Then
                                    OrdSNo = OrdSNo + 1
                                    n = .Rows.Add()
                                    .Rows(n).Cells(0).Value = Val(OrdSNo)
                                    .Rows(n).Cells(1).Value = Dt2.Rows(k).Item("Item_Name").ToString
                                    .Rows(n).Cells(2).Value = Dt2.Rows(k).Item("Sales_Order_No").ToString
                                    .Rows(n).Cells(3).Value = Val(Ent_Qty)
                                    .Rows(n).Cells(4).Value = Dt2.Rows(k).Item("Sales_Order_Code").ToString
                                End If

                            Next

                        End If

                    End With

                End If
            Next

        End If

        Total_OrderItemCalculation()

        pnl_Back.Enabled = True
        pnl_OrderSelection.Visible = False
        If txt_DcNo.Enabled And txt_DcNo.Visible Then txt_DcNo.Focus()

    End Sub

    Private Sub dtp_InvocieDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_InvocieDate.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("{TAB}")
        If e.KeyValue = 40 Then
            cbo_Type.Focus()
        End If
    End Sub

    Private Sub dtp_InvocieDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_InvocieDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_Type.Focus()
        End If
    End Sub

    Private Sub Show_Item_CurrentStock(ByVal Rw As Integer)
        Dim vItemID As Integer

        If Val(Rw) < 0 Then Exit Sub

        With dgv_Details

            vItemID = Common_Procedures.Item_NameToIdNo(con, .Rows(Rw).Cells(1).Value)

            If Val(vItemID) = 0 Then Exit Sub

            If Val(vItemID) <> Val(.Tag) Then
                'Common_Procedures.Show_ProcessedItem_CurrentStock_Display(con, Val(lbl_Company.Tag), Val(Common_Procedures.CommonLedger.Godown_Ac), vItemID)
                .Tag = Val(Rw)
            End If

        End With


    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim rect As Rectangle

        Try
            With dgv_Details
                dgv_ActCtrlName = dgv_Details.Name

                If Val(.CurrentRow.Cells(0).Value) = 0 Then
                    .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
                End If


                If e.ColumnIndex = 1 Then

                    If (cbo_Grid_ItemName.Visible = False And Trim(UCase(cbo_Type.Text)) <> "PACKING SLIP" Or Val(cbo_Grid_ItemName.Tag) <> e.RowIndex) Then

                        cbo_Grid_ItemName.Tag = -1
                        Da = New SqlClient.SqlDataAdapter("select Item_Name from Item_Head order by Item_Name", con)
                        Dt1 = New DataTable
                        Da.Fill(Dt1)
                        cbo_Grid_ItemName.DataSource = Dt1
                        cbo_Grid_ItemName.DisplayMember = "Item_Name"

                        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                        cbo_Grid_ItemName.Left = .Left + rect.Left
                        cbo_Grid_ItemName.Top = .Top + rect.Top

                        cbo_Grid_ItemName.Width = rect.Width
                        cbo_Grid_ItemName.Height = rect.Height
                        cbo_Grid_ItemName.Text = .CurrentCell.Value

                        cbo_Grid_ItemName.Tag = Val(e.RowIndex)
                        cbo_Grid_ItemName.Visible = True

                        cbo_Grid_ItemName.BringToFront()
                        cbo_Grid_ItemName.Focus()

                    End If

                Else
                    cbo_Grid_ItemName.Visible = False

                End If

                If e.ColumnIndex = 2 Then

                    If cbo_Grid_Size.Visible = False And Trim(UCase(cbo_Type.Text)) <> "PACKING SLIP" Or Val(cbo_Grid_Size.Tag) <> e.RowIndex Then

                        cbo_Grid_Size.Tag = -1
                        Da = New SqlClient.SqlDataAdapter("select Size_Name from Size_Head order by Size_Name", con)
                        Dt1 = New DataTable
                        Da.Fill(Dt1)
                        cbo_Grid_Size.DataSource = Dt1
                        cbo_Grid_Size.DisplayMember = "Size_Name"

                        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                        cbo_Grid_Size.Left = .Left + rect.Left
                        cbo_Grid_Size.Top = .Top + rect.Top

                        cbo_Grid_Size.Width = rect.Width
                        cbo_Grid_Size.Height = rect.Height
                        cbo_Grid_Size.Text = .CurrentCell.Value

                        cbo_Grid_Size.Tag = Val(e.RowIndex)
                        cbo_Grid_Size.Visible = True

                        cbo_Grid_Size.BringToFront()
                        cbo_Grid_Size.Focus()

                    End If

                Else
                    cbo_Grid_Size.Visible = False

                End If


                If e.ColumnIndex = 4 Then

                    If cbo_Grid_Unit.Visible = False And Trim(UCase(cbo_Type.Text)) <> "PACKING SLIP" Or Val(cbo_Grid_Unit.Tag) <> e.RowIndex Then

                        cbo_Grid_Unit.Tag = -1
                        Da = New SqlClient.SqlDataAdapter("select UNIT_Name from Unit_Head order by Unit_Name", con)
                        Dt1 = New DataTable
                        Da.Fill(Dt1)
                        cbo_Grid_Unit.DataSource = Dt1
                        cbo_Grid_Unit.DisplayMember = "Unit_Name"

                        rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                        cbo_Grid_Unit.Left = .Left + rect.Left
                        cbo_Grid_Unit.Top = .Top + rect.Top

                        cbo_Grid_Unit.Width = rect.Width
                        cbo_Grid_Unit.Height = rect.Height
                        cbo_Grid_Unit.Text = .CurrentCell.Value

                        cbo_Grid_Unit.Tag = Val(e.RowIndex)
                        cbo_Grid_Unit.Visible = True

                        cbo_Grid_Unit.BringToFront()
                        cbo_Grid_Unit.Focus()

                    End If

                Else
                    cbo_Grid_Unit.Visible = False

                End If



            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT ENTER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_SalesAc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SalesAc.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

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

    Private Sub cbo_VatAc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VatAc.KeyUp
        If e.Control = False And e.KeyValue = 17 Then

            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_VatAc.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub btn_SendSMS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SendSMS.Click
        Dim smstxt As String = ""
        Dim PhNo As String = ""
        Dim AgPNo As String = ""
        Dim Led_IdNo As Integer = 0
        Dim Agnt_IdNo As Integer = 0
        Dim SMS_SenderID As String = ""
        Dim SMS_Key As String = ""
        Dim SMS_RouteID As String = ""
        Dim SMS_Type As String = ""

        Try

            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
            'If Led_IdNo  = 0 Then Exit Sub
            PhNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_MobileNo", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")

            Agnt_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Agent.Text)
            If Val(Agnt_IdNo) <> 0 Then
                AgPNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_PhoneNo", "(Ledger_IdNo = " & Str(Val(Agnt_IdNo)) & ")")
            End If

            If Trim(AgPNo) <> "" Then
                If Trim(PhNo) <> "" Then
                    PhNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_MobileNo", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")") & "," & Trim(AgPNo)

                Else

                    PhNo = Trim(AgPNo)

                End If
            Else

                PhNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_MobileNo", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")

            End If

            smstxt = Trim(cbo_Ledger.Text) & Chr(13)
            smstxt = smstxt & " Inv No : " & Trim(lbl_InvoiceNo.Text) & Chr(13)
            smstxt = smstxt & " DATE : " & Trim(dtp_InvocieDate.Text) & Chr(13)
            smstxt = smstxt & " Lr No : " & Trim(txt_LrNo.Text) & Chr(13)
            smstxt = smstxt & " Bill Amount : " & Trim(lbl_NetAmount.Text) & Chr(13)
            If dgv_BaleDetails_Total.RowCount > 0 Then
                smstxt = smstxt & " No.Of Bales : " & Val((dgv_BaleDetails_Total.Rows(0).Cells(1).Value())) & Chr(13)
            End If
            smstxt = smstxt & " " & Chr(13)
            smstxt = smstxt & " Thanks! " & Chr(13)
            smstxt = smstxt & Common_Procedures.Company_IdNoToName(con, Val(lbl_Company.Tag))

            SMS_SenderID = ""
            SMS_Key = ""
            SMS_RouteID = ""
            SMS_Type = ""

            Common_Procedures.get_SMS_Provider_Details(con, Val(lbl_Company.Tag), SMS_SenderID, SMS_Key, SMS_RouteID, SMS_Type)

            Sms_Entry.SMSProvider_SenderID = SMS_SenderID
            Sms_Entry.SMSProvider_Key = SMS_Key
            Sms_Entry.SMSProvider_RouteID = SMS_RouteID
            Sms_Entry.SMSProvider_Type = SMS_Type

            Sms_Entry.vSmsPhoneNo = Trim(PhNo)
            Sms_Entry.vSmsMessage = Trim(smstxt)

            Dim f1 As New Sms_Entry
            f1.MdiParent = MDIParent1
            f1.Show()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND SMS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        '  Print_PDF_Status = False
        print_record()
    End Sub

    Private Sub btn_Print_Preprint_A_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Preprint_A.Click
        prn_Status = 3
        printing_invoice()
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_BaleDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_BaleDetails.Click
        pnl_BaleDetails.Visible = True
        pnl_BaleDetails.Enabled = True
        pnl_Back.Enabled = False
        With dgv_CartonDetails
            If .Rows.Count > 0 Then
                .Focus()
                .CurrentCell = .Rows(0).Cells(0)
            End If
        End With
    End Sub

    Private Sub btn_CloseBaleDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_CloseBaleDetails.Click
        pnl_Back.Enabled = True
        pnl_BaleDetails.Visible = False
    End Sub

    Private Sub dgv_OrderUpdate_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_OrderDetails.CellEnter
        Try
            With dgv_OrderDetails
                dgv_ActCtrlName = dgv_OrderDetails.Name
                If .Rows.Count > 0 Then
                    If (e.ColumnIndex = 3) Then
                        'If (e.ColumnIndex = 3 And dgv_LevColNo <> 3) Or (e.ColumnIndex = 5 And dgv_LevColNo <> 5) Then
                        'Show_Item_CurrentStock(e.RowIndex)
                        'Me.Activate()
                        .Focus()
                    End If
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT ENTER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_OrderUpdate_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_OrderDetails.CellLeave
        Try
            With dgv_OrderDetails
                dgv_LevColNo = e.ColumnIndex
                If e.ColumnIndex = 3 Then
                    If .Rows.Count > 0 Then
                        If Val(.CurrentRow.Cells(e.ColumnIndex).Value) <> 0 Then
                            .CurrentRow.Cells(e.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(e.ColumnIndex).Value), "#########0")
                            'Else
                            '    .CurrentRow.Cells(e.ColumnIndex).Value = ""
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT LEAVE CELL...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_OrderUpdate_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_OrderDetails.CellValueChanged
        Try
            With dgv_OrderDetails
                If .Visible Then
                    If .Rows.Count > 0 Then
                        If e.ColumnIndex = 3 Then
                            Total_OrderItemCalculation()
                        End If

                    End If
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CHANGE VALUE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgv_OrderUpdate_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_OrderDetails.EditingControlShowing
        dgtxtOrder_Details = CType(dgv_OrderDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxtOrder_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxtOrder_Details.Enter
        dgv_ActCtrlName = dgv_OrderDetails.Name
        dgv_OrderDetails.EditingControl.BackColor = Color.Lime
        dgv_OrderDetails.EditingControl.ForeColor = Color.Blue
        dgtxtOrder_Details.SelectAll()
    End Sub

    Private Sub dgtxtOrder_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxtOrder_Details.KeyPress
        With dgv_OrderDetails
            If .Visible Then
                If .Rows.Count > 0 Then
                    If .CurrentCell.ColumnIndex = 3 Then
                        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                            e.Handled = True
                        End If
                    End If
                End If
            End If
        End With
    End Sub

    Private Sub dgv_OrderDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_OrderDetails.LostFocus
        On Error Resume Next
        dgv_OrderDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_OrderDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_OrderDetails.KeyUp
        Dim i As Integer = 0
        Dim n As Integer = 0

        Try
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

                With dgv_OrderDetails
                    If .Rows.Count > 0 Then

                        n = .CurrentRow.Index

                        If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                            For i = 0 To .ColumnCount - 1
                                .Rows(n).Cells(i).Value = ""
                            Next

                        Else
                            .Rows.RemoveAt(n)

                        End If

                        Total_OrderItemCalculation()

                    End If

                End With

            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "ERROR WHILE DETAILS KEYUP....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgtxtOrder_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxtOrder_Details.KeyUp
        Try
            With dgv_OrderDetails
                If .Visible = True Then
                    If .Rows.Count > 0 Then
                        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
                            dgv_OrderDetails_KeyUp(sender, e)
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT KEYUP....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgtxtOrder_Details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxtOrder_Details.TextChanged
        Try
            If dgv_OrderDetails.Visible Then
                With dgv_OrderDetails
                    If .Rows.Count > 0 Then
                        If .CurrentCell.RowIndex >= 0 And .CurrentCell.ColumnIndex = 3 Then
                            .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxtOrder_Details.Text)
                        End If
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_ItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_ItemName.GotFocus
        vCbo_GrdItmNm = Trim(cbo_Grid_ItemName.Text)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Item_Head", "Item_Name", "", "(Item_idNo = 0)")
    End Sub

    Private Sub cbo_Grid_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_ItemName.KeyDown

        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_ItemName, Nothing, Nothing, "Item_Head", "Item_Name", "", "(Item_idNo = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_ItemName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If .CurrentCell.RowIndex <= 0 Then
                    txt_DueDays.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(5)
                    '  .CurrentCell.Selected = True
                End If

            End If

            If (e.KeyValue = 40 And cbo_Grid_ItemName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    txt_DiscPerc.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If

            End If

        End With
    End Sub

    Private Sub cbo_Grid_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_ItemName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_ItemName, Nothing, "Item_Head", "Item_Name", "", "(Item_idNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            e.Handled = True

            If Trim(UCase(vCbo_GrdItmNm)) <> Trim(UCase(cbo_Grid_ItemName.Text)) Then
                vCbo_GrdItmNm = cbo_Grid_ItemName.Text
                get_Item_Rate_Unit_from_Master()
            End If

            With dgv_Details
                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    txt_DiscPerc.Focus()
                Else
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(2)

                End If
            End With

        End If
    End Sub

    Private Sub cbo_Grid_ItemName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_ItemName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Item_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_ItemName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Grid_Size_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Size.GotFocus
        ' vCbo_ItmNm = Trim(cbo_Grid_Size.Text)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Size_Head", "Size_Name", "", "(Size_IdNo = 0)")

    End Sub

    Private Sub cbo_Grid_Size_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Size.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Size, Nothing, Nothing, "Size_Head", "Size_Name", "", "(Size_IdNo = 0)")
        vcbo_KeyDwnVal = e.KeyValue

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Size.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_Size.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With

    End Sub

    Private Sub cbo_Grid_Size_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Size.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Size, Nothing, "Size_Head", "Size_Name", "", "(Size_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If

    End Sub

    Private Sub cbo_Grid_Unit_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Unit.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Unit_Head", "Unit_Name", "", "(unit_IdNo = 0)")

    End Sub

    Private Sub cbo_Grid_Unit_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Unit.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Unit, Nothing, Nothing, "Unit_Head", "Unit_Name", "", "(unit_IdNo = 0)")
        vcbo_KeyDwnVal = e.KeyValue

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Unit.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_Unit.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With

    End Sub

    Private Sub cbo_Grid_Unit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Unit.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Unit, Nothing, "Unit_Head", "Unit_Name", "", "(unit_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If

    End Sub
    Private Sub cbo_Grid_ItemName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_ItemName.TextChanged
        Try
            If FrmLdSTS = True Then Exit Sub

            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
            If cbo_Grid_ItemName.Visible Then
                With dgv_Details
                    If Val(cbo_Grid_ItemName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_ItemName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_Size_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Size.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Size_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Size.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Grid_Size_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Size.TextChanged
        Try
            If FrmLdSTS = True Then Exit Sub

            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

            If cbo_Grid_Size.Visible Then
                With dgv_Details
                    If Val(cbo_Grid_Size.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Size.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_Unit_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Unit.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Unit_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Unit.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Grid_Unit_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Unit.TextChanged
        Try

            If FrmLdSTS = True Then Exit Sub
            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

            If cbo_Grid_Unit.Visible Then
                With dgv_Details
                    If Val(cbo_Grid_Unit.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 4 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Unit.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub



    Private Sub cbo_Type_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.TextChanged
        If Trim(UCase(cbo_Type.Text)) = "PACKING SLIP" Then
            dgv_Details.AllowUserToAddRows = False
            txt_orderdate.Enabled = False
            txt_OrderNo.Enabled = False
            cbo_Transport.Enabled = False
            txt_Noof_Carton.Enabled = False
            dgv_Details.Columns(1).ReadOnly = True
            dgv_Details.Columns(2).ReadOnly = True
            dgv_Details.Columns(3).ReadOnly = True
            dgv_Details.Columns(4).ReadOnly = True

            'dgv_Details.Columns(6).ReadOnly = True
            'dgv_Details.Columns(9).ReadOnly = True
            'dgv_Details.Columns(10).ReadOnly = True
            'dgv_Details.Columns(14).ReadOnly = True
            'dgv_Details.Columns(15).ReadOnly = True
            'cbo_LotNo.Enabled = False
            'cbo_Rate_for.Enabled = False
        Else
            dgv_Details.AllowUserToAddRows = True
            dgv_Details.EditMode = DataGridViewEditMode.EditOnEnter
            txt_orderdate.Enabled = True
            txt_OrderNo.Enabled = True
            cbo_Transport.Enabled = True
            txt_Noof_Carton.Enabled = True
            dgv_Details.Columns(1).ReadOnly = False
            dgv_Details.Columns(2).ReadOnly = False
            dgv_Details.Columns(3).ReadOnly = False
            dgv_Details.Columns(4).ReadOnly = False
            'dgv_Details.Columns(6).ReadOnly = False
            'dgv_Details.Columns(9).ReadOnly = False
            ''  dgv_Details.Columns(10).ReadOnly = False
            'dgv_Details.Columns(14).ReadOnly = False
            'cbo_LotNo.Enabled = True
            'cbo_Rate_for.Enabled = True
        End If
    End Sub

    Private Sub cbo_DespTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DespTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Garments_Sales_Invoice_head", "Despatch_To", "", "")
    End Sub
    Private Sub cbo_DespTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DespTo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DespTo, cbo_SalesAc, cbo_Transport, "Garments_Sales_Invoice_head", "Despatch_To", "", "")
    End Sub

    Private Sub cbo_DespTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DespTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DespTo, cbo_Transport, "Garments_Sales_Invoice_head", "Despatch_To", "", "", False)
    End Sub




    Private Sub lbl_NetAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_NetAmount.TextChanged
        lbl_AmountInWords.Text = "Rupees  :  "
        If Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            lbl_AmountInWords.Text = "Rupees  :  " & Common_Procedures.Rupees_Converstion(Val(CSng(lbl_NetAmount.Text)))
        End If
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

            cmd.CommandText = "Truncate table EntryTemp"
            cmd.ExecuteNonQuery()

            If Trim(UCase(cbo_Entry_Tax_Type.Text)) = "GST" Then

                AssVal_Pack_Frgt_Ins_Amt = Format(Val(txt_Packing.Text) + Val(txt_Freight.Text) + Val(txt_AddLess.Text), "#########0.00")

                With dgv_Details

                    If .Rows.Count > 0 Then
                        For i = 0 To .Rows.Count - 1
                            If Trim(.Rows(i).Cells(1).Value) <> "" And Val(.Rows(i).Cells(12).Value) <> 0 And Trim(.Rows(i).Cells(13).Value) <> "" Then
                                cmd.CommandText = "Insert into EntryTemp (                    Name1                ,                  Currency1            ,                       Currency2                                             ) " &
                                                    "          Values     ( '" & Trim(.Rows(i).Cells(13).Value) & "', " & Val(.Rows(i).Cells(12).Value) & " ,  " & Str(Val(.Rows(i).Cells(11).Value) + Val(AssVal_Pack_Frgt_Ins_Amt)) & " ) "
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

                da = New SqlClient.SqlDataAdapter("select Name1 as HSN_Code, Currency1 as GST_Percentage, sum(Currency2) as TaxableAmount from EntryTemp group by name1, Currency1 Having sum(Currency2) <> 0 order by Name1, Currency1", con)
                dt = New DataTable
                da.Fill(dt)

                If dt.Rows.Count > 0 Then

                    Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Ledger.Text)
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

                        .Rows(n).Cells(4).Value = Format(Val(.Rows(n).Cells(2).Value) * Val(.Rows(n).Cells(3).Value) / 100, "#############0.00")
                        If Val(.Rows(n).Cells(4).Value) = 0 Then .Rows(n).Cells(4).Value = ""

                        .Rows(n).Cells(6).Value = Format(Val(.Rows(n).Cells(2).Value) * Val(.Rows(n).Cells(5).Value) / 100, "#############0.00")
                        If Val(.Rows(n).Cells(6).Value) = 0 Then .Rows(n).Cells(6).Value = ""

                        .Rows(n).Cells(8).Value = Format(Val(.Rows(n).Cells(2).Value) * Val(.Rows(n).Cells(7).Value) / 100, "#############0.00")
                        If Val(.Rows(n).Cells(8).Value) = 0 Then .Rows(n).Cells(8).Value = ""

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

        lbl_TaxableAmount.Text = Format(Val(TotAss_Val), "##########0.00")
        lbl_CGST_Amount.Text = IIf(Val(TotCGST_amt) <> 0, Format(Val(TotCGST_amt), "##########0.00"), "")
        lbl_SGST_Amount.Text = IIf(Val(TotSGST_amt) <> 0, Format(Val(TotSGST_amt), "##########0.00"), "")
        lbl_IGST_Amount.Text = IIf(Val(TotIGST_amt) <> 0, Format(Val(TotIGST_amt), "##########0.00"), "")

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
                        .Rows(RowIndx).Cells(12).Value = ""  ' GST %
                        .Rows(RowIndx).Cells(13).Value = ""  ' HSN code

                        If Trim(.Rows(RowIndx).Cells(1).Value) <> "" Or Val(.Rows(RowIndx).Cells(6).Value) = 0 Then

                            HSN_Code = ""
                            GST_Per = 0
                            Get_GST_Percentage_From_ItemGroup(Trim(.Rows(RowIndx).Cells(1).Value), HSN_Code, GST_Per)


                            '--Cash discount
                            '.Rows(RowIndx).Cells(8).Value = Format(Val(.Rows(RowIndx).Cells(6).Value) * (Val(.Rows(RowIndx).Cells(7).Value) / 100), "########0.00")
                            '_ _ _ _Footer cash Discount
                            .Rows(RowIndx).Cells(9).Value = Format(Val(txt_DiscPerc.Text), "########0.00")
                            .Rows(RowIndx).Cells(10).Value = Format(Val(.Rows(RowIndx).Cells(6).Value) * (Val(.Rows(RowIndx).Cells(9).Value) / 100), "########0.00")

                            '-- Taxable value = amount -  cash disc
                            Taxable_Amount = Val(.Rows(RowIndx).Cells(6).Value) - Val(.Rows(RowIndx).Cells(8).Value) - Val(.Rows(RowIndx).Cells(10).Value)
                            .Rows(RowIndx).Cells(11).Value = Format(Val(Taxable_Amount), "##########0.00")
                            .Rows(RowIndx).Cells(12).Value = Format(Val(GST_Per), "########0.00")
                            .Rows(RowIndx).Cells(13).Value = Trim(HSN_Code)

                        End If

                    Next RowIndx

                    Get_HSN_CodeWise_Tax_Details()

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DO GST CALCULATION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Get_GST_Percentage_From_ItemGroup(ByVal ItemName As String, ByRef HSN_Code As String, ByRef GST_PerCent As Single)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Try

            HSN_Code = ""
            GST_PerCent = 0

            da = New SqlClient.SqlDataAdapter("select a.*,b.* from Item_Head a INNER JOIN ItemGroup_Head b ON a.ItemGroup_IdNo = b.ItemGroup_IdNo Where a.Item_Name ='" & Trim(ItemName) & "'", con)
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
    Private Sub cbo_Entry_Tax_Type_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Entry_Tax_Type.GotFocus
        cbo_Entry_Tax_Type.Tag = cbo_Entry_Tax_Type.Text
    End Sub

    Private Sub cbo_Entry_Tax_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Entry_Tax_Type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Entry_Tax_Type, txt_Carton_Weight, cbo_DeliveryTo, "", "", "", "")

    End Sub

    Private Sub cbo_Entry_Tax_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Entry_Tax_Type.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Entry_Tax_Type, cbo_DeliveryTo, "", "", "", "", True)
            If Asc(e.KeyChar) = 13 Then

                If Trim(UCase(cbo_Entry_Tax_Type.Tag)) <> Trim(UCase(cbo_Entry_Tax_Type.Text)) Then
                    cbo_Entry_Tax_Type.Tag = cbo_TaxType.Text
                    Total_Calculation()
                End If
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    '***** GST START *****
    Private Sub cbo_Entry_Tax_Type_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Entry_Tax_Type.LostFocus
        Try

            If Trim(UCase(cbo_Entry_Tax_Type.Tag)) <> Trim(UCase(cbo_Entry_Tax_Type.Text)) Then
                cbo_Entry_Tax_Type.Tag = cbo_Entry_Tax_Type.Text
                Total_Calculation()
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Entry_Tax_Type_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Entry_Tax_Type.SelectedIndexChanged
        Total_Calculation()
        cbo_Entry_Tax_Type.Tag = cbo_Entry_Tax_Type.Text
    End Sub

    Private Sub cbo_Ledger_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.LostFocus
        If Trim(UCase(cbo_Ledger.Tag)) <> Trim(UCase(cbo_Ledger.Text)) Then
            cbo_Ledger.Tag = cbo_Ledger.Text
            GST_Calculation()
        End If
    End Sub

    Private Sub cbo_Ledger_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.SelectedIndexChanged
        Total_Calculation()
    End Sub

    Private Sub Printing_GST_Format2(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        Dim LnAr(15) As Single, ClArr(20) As Single
        Dim ItmNm1 As String = "", ItmNm2 As String = ""
        Dim SzNm1 As String, SzNm2 As String
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer = 0
        Dim Tot_Amt As Single = 0
        Dim vprn_SZNm As String = ""
        Dim Qty As Single = 0
        Dim cnt As Integer = 0
        Dim Rate As Single = 0
        Dim Discount As Single = 0
        Dim Taxable As Single = 0
        Dim SzNm As String = ""

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20
            .Right = 45
            .Top = 30
            .Bottom = 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 9, FontStyle.Bold)

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

        NoofItems_PerPage = 15

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(20) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(30) : ClArr(2) = 200 : ClArr(3) = 80 : ClArr(4) = 80 : ClArr(5) = 75 : ClArr(6) = 50 : ClArr(7) = 75 : ClArr(8) = 80
        ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

        TxtHgt = e.Graphics.MeasureString("A", pFont).Height  ' 20
        TxtHgt = 17 ' 18.4

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_GST_Format2_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0
                CHk_Details_Cnt = 0
                CurY = CurY - 10

                '  CurY = CurY + TxtHgt
                '  Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Yarn_Description").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_GST_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        cmd.Connection = con
                        cmd.CommandText = "Truncate table EntryTemp"
                        cmd.ExecuteNonQuery()


                        cmd.CommandText = "Insert into EntryTemp( Name1 , Name2 ,      Name3        , Meters2       , Meters3 ,Meters4             ,    Name4        ,Meters5       )    " &
                          " Select                  b.Item_Name,  c.Size_Name        , d.Unit_Name ,sum(a.Quantity) ,a.Rate ,sum(a.Cash_Discount_Amount) ,a.Hsn_Code,sum(a.Taxable_Value) from Garments_Sales_Invoice_Details a INNER JOIN Item_Head b ON  a.Item_idno = b.Item_Idno  INNER JOIN Size_Head c ON a.Size_idNo =c.Size_idNo  INNER JOIN Unit_Head d ON a.unit_idNo = d.unit_IdNo where A.iTEM_IDnO = " & Val(prn_DetDt.Rows(prn_DetIndx).Item("Item_IdNO").ToString) & " and  A.Sales_Invoice_Code = '" & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sales_Invoice_Code").ToString) & "' Group by b.Item_Name, c.Size_Name , d.unit_Name,a.rate ,a.Hsn_Code Having sum(a.Quantity) <> 0 or sum(a.Cash_Discount_Amount)<>0 or sum(a.Taxable_Value)<>0"
                        cmd.ExecuteNonQuery()


                        Da1 = New SqlClient.SqlDataAdapter(" select  Name1, name2,name3,Name4,   sum(Meters2) as Qty, Meters3,sum(Meters4) as DisCount , sum(Meters5) as Taxable from EntryTemp where meters3 =" & Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString), "###########0.00") & " group by Name1, name2,Name3 ,Name4,Meters3,Meters4,Meters5", con)
                        Dt1 = New DataTable
                        Da1.Fill(Dt1)
                        vprn_SZNm = ""
                        cnt = 0
                        Qty = 0
                        Discount = 0
                        Taxable = 0
                        If Dt1.Rows.Count > 0 Then
                            cnt = Dt1.Rows.Count
                            For j = 0 To Dt1.Rows.Count - 1
                                ItmNm1 = Trim(Dt1.Rows(j).Item("name1").ToString)
                                ItmNm2 = ""
                                If Len(ItmNm1) > 25 Then
                                    For I = 25 To 1 Step -1
                                        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 25
                                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                                End If

                                If Trim((Dt1.Rows(j).Item("name2").ToString)) <> "" Then
                                    vprn_SZNm = Trim(vprn_SZNm) & IIf(Trim(vprn_SZNm) <> "", ", ", "") & Trim(Dt1.Rows(j).Item("name2").ToString)
                                End If
                                Qty = (Qty + Val(Dt1.Rows(j).Item("Qty").ToString))
                                Discount = (Discount + Val(Dt1.Rows(j).Item("Discount").ToString))
                                Taxable = (Taxable + Val(Dt1.Rows(j).Item("Taxable").ToString))
                                Rate = Format(Val(Dt1.Rows(0).Item("meters3").ToString), "############0.00")
                            Next

                        End If

                        SzNm1 = Trim(vprn_SZNm)
                        SzNm2 = ""
                        If Len(SzNm1) > 10 Then
                            For I = 10 To 1 Step -1
                                If Mid$(Trim(SzNm1), I, 1) = " " Or Mid$(Trim(SzNm1), I, 1) = "," Or Mid$(Trim(SzNm1), I, 1) = "." Or Mid$(Trim(SzNm1), I, 1) = "-" Or Mid$(Trim(SzNm1), I, 1) = "/" Or Mid$(Trim(SzNm1), I, 1) = "_" Or Mid$(Trim(SzNm1), I, 1) = "(" Or Mid$(Trim(SzNm1), I, 1) = ")" Or Mid$(Trim(SzNm1), I, 1) = "\" Or Mid$(Trim(SzNm1), I, 1) = "[" Or Mid$(Trim(SzNm1), I, 1) = "]" Or Mid$(Trim(SzNm1), I, 1) = "{" Or Mid$(Trim(SzNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 10
                            SzNm2 = Microsoft.VisualBasic.Right(Trim(SzNm1), Len(SzNm1) - I)
                            SzNm1 = Microsoft.VisualBasic.Left(Trim(SzNm1), I - 1)
                        End If
                        ' If Itm_Nm <> prn_DetDt.Rows(prn_DetIndx).Item("Item_name").ToString And cnt > 1 And SzNm <> SzNm1 Then
                        If cnt > 1 And SzNm <> SzNm1 Then
                            CurY = CurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Dt1.Rows(0).Item("name4").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, SzNm1, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Qty, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Dt1.Rows(0).Item("name3").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 10, CurY, 0, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(Rate), "#############0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Discount, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(Taxable), "###########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                        Else

                            If cnt = 1 Then
                                CurY = CurY + TxtHgt
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Dt1.Rows(0).Item("name4").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, SzNm1, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Qty, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Dt1.Rows(0).Item("name3").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 10, CurY, 0, 0, pFont)

                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(Rate), "#############0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Discount, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(Taxable), "###########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                            End If
                        End If

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If




                        prn_DetIndx = prn_DetIndx + 1
                        CHk_Details_Cnt = prn_DetIndx
                    Loop

                End If


                Printing_GST_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

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

    Private Sub Printing_GST_Format2_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim C1 As Single, C2 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String, Cmp_PanCap As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim S As String
        Dim CurY1 As Single = 0, CurX As Single = 0
        Dim Y1 As Single = 0, Y2 As Single = 0

        PageNo = PageNo + 1

        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Val(S) = 1 Then
                    prn_OriDupTri = "Original for Receipient"
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "Duplicate for Supplier/ Transporter"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "Triplicate for supplier"
                Else
                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 4 Then
                        prn_OriDupTri = Trim(prn_InpOpts)
                    End If
                End If

            End If
        End If

        CurY = TMargin

        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        'p1Font = New Font("Calibri", 12, FontStyle.Regular)
        'Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY - TxtHgt, 2, PrintWidth, p1Font)

        'da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Ledger_Name as Transport_Name from Garments_Sales_Invoice_head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Transport_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Sales_Code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
        'da2.Fill(dt2)
        'If dt2.Rows.Count > NoofItems_PerPage Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        'End If
        'dt2.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY


        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- Logu textile

        '    If InStr(1, Trim(UCase(Cmp_Name)), "GANAPATHY") > 0 Or InStr(1, Trim(UCase(Cmp_Name)), "GANAPATHI") > 0 Then                                    '---- Ganapathy Spinning textile
        '        e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.GSM_LOGO, Drawing.Image), LMargin + 20, CurY, 112, 80)
        '    ElseIf InStr(1, Trim(UCase(Cmp_Name)), "LOGU") > 0 Or InStr(1, Trim(UCase(Cmp_Name)), "LOGA") > 0 Then                                          '---- Logu textile
        '        e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_LogaTex, Drawing.Image), LMargin + 20, CurY, 112, 80)
        '    End If

        'End If



        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = "" : Cmp_PanCap = "" : Cmp_EMail = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO :" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO. : " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO. : " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
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
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Code").ToString) <> "" Then
            Cmp_StateCode = "CODE :" & prn_HdDt.Rows(0).Item("Company_State_Code").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If

        '    e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.CompanyLOGO_RD, Drawing.Image), LMargin + 20, CurY + 20, 112, 80)

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

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1084" Then
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, LMargin, CurY, 2, PrintWidth, pFont)
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

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, pFont)
        strWidth = e.Graphics.MeasureString(Cmp_StateCap, pFont).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont)

        strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, pFont)
        strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, pFont).Width
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
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_EMail), LMargin, CurY, 2, PrintWidth, pFont)

        'If Cmp_State <> "" Then
        '    CurY = CurY + TxtHgt - 1
        '    Common_Procedures.Print_To_PrintDocument(e, Cmp_State, LMargin, CurY, 2, PrintWidth, pFont)
        'End If



        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Y1 = CurY
        Y2 = CurY + TxtHgt - 10 + TxtHgt + 5
        Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE", LMargin, CurY, 2, PageWidth, p1Font)

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)

            W1 = e.Graphics.MeasureString("Reverse Charge (Y/N)    .", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width

            CurY1 = CurY + 10

            'Left Side
            Common_Procedures.Print_To_PrintDocument(e, "Invoice No", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_Invoice_No").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, p1Font)



            Common_Procedures.Print_To_PrintDocument(e, "Transport Mode", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Mode").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Invoice_Date").ToString), "dd-MM-yyyy").ToString, LMargin + W1 + 30, CurY1, 0, 0, pFont)


            Common_Procedures.Print_To_PrintDocument(e, "Vehicle Number", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Vehicle_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Vechile_IdNo").ToString)), LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Purchase Order No", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Transport Name", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Transport_IdNo").ToString)), LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)


            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Purchase Order Date", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_Date").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "G R No", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, (prn_HdDt.Rows(0).Item("Lr_No").ToString), LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "No.of Carton/Bag", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Noof_Carton").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Date Of Supply", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Date_And_Time_Of_Supply").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Reverse Charge (Yes/No)", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + W1 + 30, CurY1, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Place Of Supply", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DelState_Name").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)
            CurY = CurY1 + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 10 + TxtHgt + 5
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)


            CurY1 = CurY + TxtHgt - 10

            Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF RECEIVER  (BILLED TO)", LMargin, CurY1, 2, C2, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF CONSIGNEE  (SHIPPED TO)", LMargin + C2, CurY1, 2, PageWidth - C2, pFont)
            CurY = CurY1 + TxtHgt + 5


            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY
            CurY = CurY + 10

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("DelName").ToString, LMargin + C2 + 10, CurY, 0, 0, p1Font)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd1").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd2").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd3").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd4").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 12
            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)

                If Trim(prn_HdDt.Rows(0).Item("Pan_No").ToString) <> "" Then
                    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
                    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("Pan_No").ToString, LMargin + S1 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
                End If

            End If

            If Trim(prn_HdDt.Rows(0).Item("DelGSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + C2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelGSTinNo").ToString, LMargin + S1 + C2 + 30, CurY, 0, 0, pFont)
                If Trim(prn_HdDt.Rows(0).Item("Pan_No").ToString) <> "" Then
                    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
                    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("DelPanNo").ToString, LMargin + S1 + C2 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
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
                Common_Procedures.Print_To_PrintDocument(e, "Code      " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 5, CurY, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("DelState_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "State", LMargin + C2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelState_Name").ToString, LMargin + S1 + C2 + 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Code       " & prn_HdDt.Rows(0).Item("Delivery_State_Code").ToString, LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 70, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + C2, LnAr(4), LMargin + C2, LnAr(2))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 5, LnAr(4), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 5, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 40, LnAr(4), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 40, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 60, LnAr(4), LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 60, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 110, LnAr(4), LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 110, LnAr(3))

            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 10 + TxtHgt + 5
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)

            p1Font = New Font("Calibri", 9, FontStyle.Regular)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "SNo", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PRODUCT DESCRIPTION", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SIZE", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "QTY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "UNIT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DISCOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TAXABLE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_GST_Format2_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim C1 As Single, W1 As Single
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim BmsInWrds As String
        Dim vprn_BlNos As String = ""
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim CurY1 As Single = 0
        Dim TaxAmt As Single = 0
        Dim TOT As Single = 0
        Dim Y1 As Single = 0, Y2 As Single = 0
        Dim vTaxPerc As Single = 0

        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Try

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt

                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Quantity").ToString), "##########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), "##########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Taxable_Value").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt - 15

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY


            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
            '  e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(4))

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6)

            W1 = e.Graphics.MeasureString("DISCOUNT    : ", pFont).Width

            ' If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- LOGUtEX

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

            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 15 + TxtHgt
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + C1, Y2)

            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS", LMargin, CurY, 2, C1, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Packing ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Packing_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY




            CurY = CurY + TxtHgt - 15

            Common_Procedures.Print_To_PrintDocument(e, "BANK NAME  :  " & BankNm1, LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "To be Paid in Full  ", LMargin + 10, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "BANK NAME : " & BankNm1, C1 - 50, CurY, 1, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Freight", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 3
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) - 50, CurY, LMargin + ClAr(1) + ClAr(2) - 50, LnAr(7))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15
            LnAr(8) = CurY
            p1Font = New Font("Calibri", 11, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "ACCOUNT No.  :  " & BankNm2, LMargin + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Add / Less ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "BRANCH  :  " & BankNm3, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "IFSC CODE  :  " & BankNm4, LMargin + ClAr(1) + ClAr(2) + 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Total  Before Tax  ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, ": ", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Total_Taxable_Value").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + 20, CurY, LMargin + ClAr(1) + ClAr(2) + 20, LnAr(9))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            CurY = CurY + TxtHgt - 15
            p1Font = New Font("Calibri", 10, FontStyle.Bold Or FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)

            vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)

            If Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "1. Goods One sold will not be taken or replaced ", LMargin + 20, CurY, 0, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15

            Common_Procedures.Print_To_PrintDocument(e, "2. All Dispute subject to Tirupur Jurisdiction Only", LMargin + 20, CurY, 0, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ " & Format(Val(vTaxPerc), "##########0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15

            Common_Procedures.Print_To_PrintDocument(e, "3. Certified this Invoice shows the actual price of the goods described and particulars ", LMargin + 20, CurY, 0, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "RoundOff ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)

            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 15 + TxtHgt + TxtHgt - 15 + TxtHgt - 0.5
            Common_Procedures.FillRegionRectangle(e, LMargin + C1, Y1, PageWidth, Y2)


            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "    given above are true and correct", LMargin + 20, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 9, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "GRAND TOTAL ", LMargin + C1 + 10, CurY + 10, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "" & Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), PageWidth - 10, CurY + 10, 1, 0, p1Font)

            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth - (ClAr(9) + ClAr(8) + ClAr(7)), CurY)
            LnAr(10) = CurY

            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 15 + TxtHgt
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, Y2)

            CurY = CurY + TxtHgt - 15
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Amount in Words - INR", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "E. & O.E", LMargin + C1 - 30, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, LnAr(10))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 15 + TxtHgt - 0.5
            Common_Procedures.FillRegionRectangle(e, LMargin + C1, Y1, PageWidth, Y2)

            CurY = CurY + TxtHgt - 15

            Common_Procedures.Print_To_PrintDocument(e, "GST on Reverse Charge", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, p1Font)

            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
            Common_Procedures.Print_To_PrintDocument(e, " " & StrConv(BmsInWrds, VbStrConv.ProperCase), LMargin + 10, CurY + 5, 0, 0, p1Font)



            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth - (ClAr(9) + ClAr(8) + ClAr(7)), CurY)
            LnAr(14) = CurY


            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            CurY = CurY + TxtHgt - 15
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY - 5, 0, 0, p1Font)




            CurY = CurY + TxtHgt
            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 50, CurY, 0, 0, p1Font)
            End If
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt


            Common_Procedures.Print_To_PrintDocument(e, "Prepared by ", LMargin + 50, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Checked by ", LMargin + 350, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory ", PageWidth - 10, CurY, 1, 0, p1Font)
            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + 15, CurY, LMargin + ClAr(1) + ClAr(2) + 15, LnAr(14))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Function get_GST_Noof_HSN_Codes_For_Printing(ByVal EntryCode As String) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim NoofHsnCodes As Integer = 0

        NoofHsnCodes = 0

        Da = New SqlClient.SqlDataAdapter("Select * from Garments_Sales_GST_Tax_Details Where Sales_Invoice_Code = '" & Trim(EntryCode) & "'", con)
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
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim TaxPerc As Single = 0

        TaxPerc = 0

        Da = New SqlClient.SqlDataAdapter("Select * from Garments_Sales_GST_Tax_Details Where Sales_Invoice_Code = '" & Trim(EntryCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If Dt1.Rows.Count = 1 Then

                Da = New SqlClient.SqlDataAdapter("Select * from Garments_Sales_GST_Tax_Details Where Sales_Invoice_Code = '" & Trim(EntryCode) & "'", con)
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

    Private Sub Printing_GST_Format3(ByRef e As System.Drawing.Printing.PrintPageEventArgs)

        'Dim cmd As New SqlClient.SqlCommand
        'Dim Da1 As New SqlClient.SqlDataAdapter
        'Dim Dt1 As New DataTable

        'Dim Da2 As New SqlClient.SqlDataAdapter
        'Dim Dt2 As New DataTable

        'Dim EntryCode As String
        'Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        'Dim pFont As Font
        'Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        'Dim PrintWidth As Single, PrintHeight As Single
        'Dim PageWidth As Single, PageHeight As Single
        'Dim CurY As Single, TxtHgt As Single
        'Dim LnAr(15) As Single, ClArr(20) As Single
        'Dim ItmNm1 As String = "", ItmNm2 As String = ""
        'Dim SzNm1 As String, SzNm2 As String
        'Dim ps As Printing.PaperSize
        'Dim strHeight As Single = 0
        'Dim PpSzSTS As Boolean = False
        'Dim W1 As Single = 0
        'Dim SNo As Integer = 0
        'Dim Tot_Amt As Single = 0
        'Dim vprn_SZNm As String = ""
        'Dim Qty As Single = 0
        'Dim cnt As Integer = 0
        'Dim Rate As Single = 0
        'Dim Discount As Single = 0
        'Dim Taxable As Single = 0
        'Dim SzNm As String = ""
        'Dim Itm_Nm As String = ""
        'Dim ItmNm(10) As String
        'Dim ItmNmArLn As Integer = 0
        'Dim SizNm(10) As String
        'Dim SizNmArLn As Integer = 0

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        Exit For
        '    End If
        'Next

        'With PrintDocument1.DefaultPageSettings.Margins
        '    .Left = 20
        '    .Right = 45
        '    .Top = 30
        '    .Bottom = 30
        '    LMargin = .Left
        '    RMargin = .Right
        '    TMargin = .Top
        '    BMargin = .Bottom
        'End With

        'pFont = New Font("Calibri", 9, FontStyle.Bold)

        'e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        'With PrintDocument1.DefaultPageSettings.PaperSize
        '    PrintWidth = .Width - RMargin - LMargin
        '    PrintHeight = .Height - TMargin - BMargin
        '    PageWidth = .Width - RMargin
        '    PageHeight = .Height - BMargin
        'End With
        'If PrintDocument1.DefaultPageSettings.Landscape = True Then
        '    With PrintDocument1.DefaultPageSettings.PaperSize
        '        PrintWidth = .Height - TMargin - BMargin
        '        PrintHeight = .Width - RMargin - LMargin
        '        PageWidth = .Height - TMargin
        '        PageHeight = .Width - RMargin
        '    End With
        'End If

        'NoofItems_PerPage = 13

        'Erase LnAr
        'Erase ClArr

        'LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        'ClArr = New Single(20) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        'ClArr(1) = Val(35) : ClArr(2) = 200 : ClArr(3) = 80 : ClArr(4) = 80 : ClArr(5) = 80 : ClArr(6) = 50 : ClArr(7) = 70 : ClArr(8) = 70
        'ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

        'TxtHgt = e.Graphics.MeasureString("A", pFont).Height  ' 20
        'TxtHgt = 17 ' 18.4

        'EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        'Try

        '    If prn_HdDt.Rows.Count > 0 Then

        '        Printing_GST_Format3_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

        '        ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

        '        NoofDets = 0
        '        CHk_Details_Cnt = 0
        '        CurY = CurY - 10

        '        ' CurY = CurY + TxtHgt
        '        ' Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Yarn_Description").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)

        '        If prn_DetDt.Rows.Count > 0 Then

        '            Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

        '                If NoofDets >= NoofItems_PerPage Then
        '                    CurY = CurY + TxtHgt

        '                    Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

        '                    NoofDets = NoofDets + 1

        '                    Printing_GST_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

        '                    e.HasMorePages = True
        '                    Return

        '                End If

        '                cmd.Connection = con
        '                cmd.CommandText = "Truncate table EntryTemp"
        '                cmd.ExecuteNonQuery()


        '                cmd.CommandText = "Insert into EntryTemp( Name1 , Name2 ,      Name3        , Meters2       , Meters3 ,Meters4             ,    Name4        ,Meters5       )    " & _
        '                                " Select  b.Item_Name,  c.Size_Name        , d.Unit_Name ,sum(a.Quantity) ,a.Rate ,sum(a.Cash_Discount_Amount) ,a.Hsn_Code,sum(a.Amount-a.Cash_Discount_Amount) from Garments_Sales_Invoice_Details a INNER JOIN Item_Head b ON  a.Item_idno = b.Item_Idno  INNER JOIN Size_Head c ON a.Size_idNo =c.Size_idNo  INNER JOIN Unit_Head d ON a.unit_idNo = d.unit_IdNo where A.iTEM_IDnO = " & Val(prn_DetDt.Rows(prn_DetIndx).Item("Item_IdNO").ToString) & " and  A.Sales_Invoice_Code = '" & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sales_Invoice_Code").ToString) & "' Group by b.Item_Name, c.Size_Name , d.unit_Name,a.rate ,a.Hsn_Code Having sum(a.Quantity) <> 0 or sum(a.Cash_Discount_Amount)<>0 or sum(a.Amount-a.Cash_Discount_Amount)<>0"
        '                cmd.ExecuteNonQuery()


        '                Da1 = New SqlClient.SqlDataAdapter(" select  Name1, name3,Name4,sum(Meters2) as Qty, Meters3,sum(Meters4) as DisCount , sum(Meters5) as Amount from EntryTemp where meters3 =" & Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString), "###########0.00") & " group by Name1, Name3 ,Name4,Meters3", con)
        '                Dt1 = New DataTable
        '                Da1.Fill(Dt1)

        '                vprn_SZNm = ""
        '                cnt = 0
        '                Qty = 0
        '                Taxable = 0
        '                Discount = 0

        '                Dim RowsInThisRecord As Integer = 1

        '                If Dt1.Rows.Count > 0 Then
        '                    cnt = Dt1.Rows.Count

        '                    For j = 0 To Dt1.Rows.Count - 1

        '                        Da2 = New SqlClient.SqlDataAdapter(" select distinct isnull(Name2,'') from EntryTemp where meters3 = " & Dt1.Rows(j).Item("meters3") & " and name1 = '" & Dt1.Rows(j).Item("name1") & "'  Order by isnull(Name2,'')", con)
        '                        Dt2 = New DataTable
        '                        Da2.Fill(Dt2)

        '                        SzNm1 = ""
        '                        For k As Integer = 0 To Dt2.Rows.Count - 1
        '                            If Len(Trim(Dt2.Rows(k).Item(0))) > 0 Then
        '                                SzNm1 = SzNm1 + IIf(Len(SzNm1) > 0, ",", "") + Dt2.Rows(k).Item(0)
        '                            End If
        '                        Next


        '                        BreakString(Trim(Dt1.Rows(j).Item("name1").ToString), ItmNm, ItmNmArLn, 25)

        '                        BreakString(SzNm1, SizNm, SizNmArLn, 10)

        '                        If SizNmArLn > ItmNmArLn Then
        '                            RowsInThisRecord = SizNmArLn
        '                        Else
        '                            RowsInThisRecord = ItmNmArLn
        '                        End If

        '                        'ItmNm1 = Trim(Dt1.Rows(j).Item("name1").ToString)
        '                        'ItmNm2 = ""
        '                        'If Len(ItmNm1) > 25 Then
        '                        '    For I = 25 To 1 Step -1
        '                        '        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
        '                        '    Next I
        '                        '    If I = 0 Then I = 25
        '                        '    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
        '                        '    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
        '                        'End If

        '                        'If Trim((Dt1.Rows(j).Item("name2").ToString)) <> "" Then
        '                        '    vprn_SZNm = Trim(vprn_SZNm) & IIf(Trim(vprn_SZNm) <> "", ", ", "") & Trim(Dt1.Rows(j).Item("name2").ToString)
        '                        'End If

        '                        Qty = (Qty + Val(Dt1.Rows(j).Item("Qty").ToString))
        '                        Discount = (Discount + Val(Dt1.Rows(j).Item("Discount").ToString))
        '                        Taxable = (Taxable + Val(Dt1.Rows(j).Item("Amount").ToString))
        '                        Rate = Format(Val(Dt1.Rows(0).Item("meters3").ToString), "############0.00")

        '                    Next

        '                End If


        '                ' If Itm_Nm <> prn_DetDt.Rows(prn_DetIndx).Item("Item_name").ToString And cnt > 1 And SzNm <> SzNm1 Then

        '                CurY = CurY + TxtHgt
        '                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 10, CurY, 0, 0, pFont)
        '                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm(0)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
        '                Common_Procedures.Print_To_PrintDocument(e, Dt1.Rows(0).Item("name4").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
        '                Common_Procedures.Print_To_PrintDocument(e, Trim(SizNm(0)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
        '                Common_Procedures.Print_To_PrintDocument(e, Qty, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
        '                Common_Procedures.Print_To_PrintDocument(e, Dt1.Rows(0).Item("name3").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 10, CurY, 0, 0, pFont)

        '                Common_Procedures.Print_To_PrintDocument(e, Format(Val(Rate), "#############0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
        '                Common_Procedures.Print_To_PrintDocument(e, Discount, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
        '                Common_Procedures.Print_To_PrintDocument(e, Format(Val(Taxable), "###########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

        '                'Else

        '                '    If cnt = 1 Then
        '                '        CurY = CurY + TxtHgt
        '                '        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 10, CurY, 0, 0, pFont)
        '                '        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
        '                '        Common_Procedures.Print_To_PrintDocument(e, Dt1.Rows(0).Item("name4").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
        '                '        Common_Procedures.Print_To_PrintDocument(e, SzNm1, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
        '                '        Common_Procedures.Print_To_PrintDocument(e, Qty, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
        '                '        Common_Procedures.Print_To_PrintDocument(e, Dt1.Rows(0).Item("name3").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 10, CurY, 0, 0, pFont)

        '                '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(Rate), "#############0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
        '                '        Common_Procedures.Print_To_PrintDocument(e, Discount, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
        '                '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(Taxable), "###########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

        '                '    End If


        '                NoofDets = NoofDets + 1

        '                If RowsInThisRecord > 0 Then

        '                    For J As Integer = 1 To RowsInThisRecord

        '                        CurY = CurY + TxtHgt - 5
        '                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm(J)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
        '                        Common_Procedures.Print_To_PrintDocument(e, Trim(SizNm(J)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
        '                        NoofDets = NoofDets + 1

        '                    Next

        '                End If


        '                Erase ItmNm
        '                ReDim ItmNm(10)
        '                Erase SizNm
        '                ReDim SizNm(10)

        '                SizNmArLn = 0
        '                ItmNmArLn = 0

        '                Itm_Nm = prn_DetDt.Rows(prn_DetIndx).Item("Item_Name").ToString
        '                SzNm = Trim(vprn_SZNm)
        '                prn_DetIndx = prn_DetIndx + 1
        '                CHk_Details_Cnt = prn_DetIndx




        '            Loop

        '        End If


        '        Printing_GST_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

        '        If Trim(prn_InpOpts) <> "" Then
        '            If prn_Count < Len(Trim(prn_InpOpts)) Then


        '                If Val(prn_InpOpts) <> "0" Then
        '                    prn_DetIndx = 0
        '                    prn_DetSNo = 0
        '                    prn_PageNo = 0

        '                    e.HasMorePages = True
        '                    Return
        '                End If

        '            End If
        '        End If

        '    End If

        'Catch ex As Exception

        '    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'End Try

        'e.HasMorePages = False

        '-----------------------------------------------------------------------------------------------------------------

        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Da2 As New SqlClient.SqlDataAdapter
        Dim Dt2 As New DataTable
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(20) As Single
        Dim ItmNm1 As String = "", ItmNm2 As String = ""
        Dim SzNm1 As String, SzNm2 As String
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer = 0
        Dim Tot_Amt As Single = 0
        Dim vprn_SZNm As String = ""
        Dim Qty As Single = 0
        Dim cnt As Integer = 0
        Dim Rate As Single = 0
        Dim Discount As Single = 0
        Dim Taxable As Single = 0
        Dim SzNm As String = ""
        Dim Itm_Nm As String = ""
        Dim ItmNm(10) As String
        Dim ItmNmArLn As Integer = 0
        Dim SizNm(10) As String
        Dim SizNmArLn As Integer = 0
        Dim RowsInThisRecord As Integer = 0

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20
            .Right = 45
            .Top = 30
            .Bottom = 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 9, FontStyle.Bold)

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

        NoofItems_PerPage = 13

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(20) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(35) : ClArr(2) = 200 : ClArr(3) = 80 : ClArr(4) = 80 : ClArr(5) = 80 : ClArr(6) = 50 : ClArr(7) = 70 : ClArr(8) = 70
        ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

        TxtHgt = e.Graphics.MeasureString("A", pFont).Height  ' 20
        TxtHgt = 17 ' 18.4

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_GST_Format3_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0
                CHk_Details_Cnt = 0
                CurY = CurY - 10

                '  CurY = CurY + TxtHgt
                '  Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Yarn_Description").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1



                        'If NoofDets >= NoofItems_PerPage Then
                        '    CurY = CurY + TxtHgt

                        '    Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                        '    NoofDets = NoofDets + 1

                        '    Printing_GST_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                        '    e.HasMorePages = True
                        '    Return

                        'End If

                        cmd.Connection = con
                        cmd.CommandText = "Truncate table EntryTemp"
                        cmd.ExecuteNonQuery()


                        cmd.CommandText = "Insert into EntryTemp( Name1 , Name2 ,      Name3        , Meters2       , Meters3 ,Meters4             ,    Name4        ,Meters5       ,Int1)    " &
                          " Select                               b.Item_Name,  c.Size_Name        , d.Unit_Name ,sum(a.Quantity) ,a.Rate ,sum(a.Cash_Discount_Amount) ,a.Hsn_Code,sum(a.Amount-a.Cash_Discount_Amount),Min(A.Sl_No) from Garments_Sales_Invoice_Details a INNER JOIN Item_Head b ON  a.Item_idno = b.Item_Idno  INNER JOIN Size_Head c ON a.Size_idNo =c.Size_idNo  INNER JOIN Unit_Head d ON a.unit_idNo = d.unit_IdNo where A.Sales_Invoice_Code = '" & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sales_Invoice_Code").ToString) & "' Group by b.Item_Name, c.Size_Name , d.unit_Name,a.rate ,a.Hsn_Code Having sum(a.Quantity) <> 0 or sum(a.Cash_Discount_Amount)<>0 or sum(a.Amount-a.Cash_Discount_Amount)<>0"
                        cmd.ExecuteNonQuery()


                        Da1 = New SqlClient.SqlDataAdapter(" select  Name1, name3,Name4,   sum(Meters2) as Qty, Meters3,sum(Meters4) as DisCount , sum(Meters5) as Amount,Min(Int1) from EntryTemp  group by Name1, Name3 ,Name4,Meters3 Order By Min(Int1)", con)
                        Dt1 = New DataTable
                        Da1.Fill(Dt1)

                        vprn_SZNm = ""
                        SzNm1 = ""
                        cnt = 0
                        Qty = 0
                        Taxable = 0
                        Discount = 0

                        If Dt1.Rows.Count > 0 Then

                            cnt = Dt1.Rows.Count

                            For j = 0 To Dt1.Rows.Count - 1

                                j = prn_DetIndx

                                If NoofDets > NoofItems_PerPage Then
                                    CurY = CurY + TxtHgt

                                    Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                                    NoofDets = NoofDets + 1

                                    Printing_GST_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                                    e.HasMorePages = True
                                    Return

                                End If

                                RowsInThisRecord = 0
                                Qty = 0
                                Discount = 0
                                Taxable = 0
                                Rate = 0

                                Da2 = New SqlClient.SqlDataAdapter(" select distinct isnull(Name2,'') from EntryTemp where meters3 = " & Dt1.Rows(j).Item("meters3") & " and name1 = '" & Dt1.Rows(j).Item("name1") & "'  Order by isnull(Name2,'')", con)
                                Dt2 = New DataTable
                                Da2.Fill(Dt2)

                                SzNm1 = ""
                                For k As Integer = 0 To Dt2.Rows.Count - 1
                                    If Len(Trim(Dt2.Rows(k).Item(0))) > 0 Then
                                        SzNm1 = SzNm1 + IIf(Len(SzNm1) > 0, ",", "") + Dt2.Rows(k).Item(0)
                                    End If
                                Next


                                BreakString(Trim(Dt1.Rows(j).Item("name1").ToString), ItmNm, ItmNmArLn, 25)

                                BreakString(SzNm1, SizNm, SizNmArLn, 10)

                                If SizNmArLn > ItmNmArLn Then
                                    RowsInThisRecord = SizNmArLn
                                Else
                                    RowsInThisRecord = ItmNmArLn
                                End If

                                Qty = (Qty + Val(Dt1.Rows(j).Item("Qty").ToString))
                                Discount = (Discount + Val(Dt1.Rows(j).Item("Discount").ToString))
                                Taxable = (Taxable + Val(Dt1.Rows(j).Item("Amount").ToString))
                                Rate = Format(Val(Dt1.Rows(j).Item("meters3").ToString), "############0.00")

                                CurY = CurY + TxtHgt
                                Common_Procedures.Print_To_PrintDocument(e, Trim(j + 1.ToString), LMargin + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm(0)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Dt1.Rows(0).Item("name4").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, SizNm(0), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Qty, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Dt1.Rows(0).Item("name3").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(Rate), "#############0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                                If Discount <> 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, Discount, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                                End If
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(Taxable), "###########0.00"), PageWidth - 10, CurY, 1, 0, pFont)


                                NoofDets = NoofDets + 1

                                For K As Integer = 1 To RowsInThisRecord
                                    CurY = CurY + TxtHgt - 5
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm(K)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(SizNm(K)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                                    NoofDets = NoofDets + 1
                                Next

                                Itm_Nm = Trim(Dt1.Rows(j).Item("name1").ToString)
                                SzNm = Trim(SzNm1)
                                prn_DetIndx = prn_DetIndx + 1
                                CHk_Details_Cnt = prn_DetIndx
                            Next

                        End If




                        'If Trim(ItmNm2) <> "" Then
                        '    CurY = CurY + TxtHgt - 5
                        '    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        '    NoofDets = NoofDets + 1
                        'End If

                        'If SzNm <> Trim(vprn_SZNm) Then
                        '    If Trim(SzNm2) <> "" Then
                        '        CurY = CurY + TxtHgt - 5
                        '        Common_Procedures.Print_To_PrintDocument(e, Trim(SzNm2), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 10, CurY, 0, 0, pFont)
                        '        NoofDets = NoofDets + 1
                        '    End If
                        'End If



                    Loop

                End If


                Printing_GST_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

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

    Private Sub Printing_GST_Format3_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)

        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim p2Font As Font
        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim C1 As Single, C2 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String, Cmp_PanCap As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim S As String
        Dim CurY1 As Single = 0, CurX As Single = 0
        Dim Y1 As Single = 0, Y2 As Single = 0

        Dim Clr1 As New Color
        Clr1 = Color.FromArgb(37, 111, 48)
        Dim Brush1 As New SolidBrush(Clr1)

        Dim Clr2 As New Color
        Clr2 = Color.FromArgb(71, 71, 71)
        Dim Brush2 As New SolidBrush(Clr2)

        PageNo = PageNo + 1

        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

                If Val(S) = 1 Then
                    prn_OriDupTri = "Original for Receipient"
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "Duplicate for Supplier/ Transporter"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "Triplicate for supplier"
                Else
                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 4 Then
                        prn_OriDupTri = Trim(prn_InpOpts)
                    End If
                End If

            End If
        End If

        CurY = TMargin

        If PageNo > 1 Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        Else
            If Trim(prn_OriDupTri) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
            End If
        End If

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        p2Font = New Font("ITC Bauhaus Heavy", 22, FontStyle.Bold)
        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = "" : Cmp_PanCap = "" : Cmp_EMail = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO :" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO. : " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO. : " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
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
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_State_Code").ToString) <> "" Then
            Cmp_StateCode = "CODE :" & prn_HdDt.Rows(0).Item("Company_State_Code").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1076" Then '---- ROHIT Textile  (SP International) - Tirupur
        '    If InStr(1, Trim(UCase(Cmp_Name)), "HOMSPUN INDUSTRIES") > 0 Then
        '        e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.SHREE_LOGO1, Drawing.Image), LMargin + 20, CurY + 8, 112, 80)
        '    ElseIf InStr(1, Trim(UCase(Cmp_Name)), "SPACEWEAR") > 0 Then
        '        e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.space_wear_logo_2, Drawing.Image), LMargin + 25, CurY, 80, 80)
        '        p1Font = New Font("CAMBRIA", 6, FontStyle.Bold)
        '        Dim Twdt As Single = e.Graphics.MeasureString("World of Possibilities", p1Font).Width
        '        Common_Procedures.Print_To_PrintDocument(e, "World of Possibilities", (LMargin + 25 + 40) - (Twdt / 2), CurY + 75, 2, 80, p1Font, Brush2)
        '    Else
        '        e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.CompanyLOGO_RD, Drawing.Image), LMargin + 20, CurY + 20, 112, 80)
        '    End If
        'End If

        CurY = CurY + TxtHgt - 12
        p1Font = New Font("Calibri", 18, FontStyle.Bold)

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1076" And InStr(1, Trim(UCase(Cmp_Name)), "HOMSPUN INDUSTRIES") > 0 Then
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font, Brushes.Red)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1076" And InStr(1, Trim(UCase(Cmp_Name)), "SPACEWEAR") > 0 Then
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font, Brush1)
        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p2Font)
        End If
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight - 1
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1076" And InStr(1, Trim(UCase(Cmp_Name)), "HOMSPUN INDUSTRIES") > 0 Then
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont, Brushes.Red)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1076" And InStr(1, Trim(UCase(Cmp_Name)), "SPACEWEAR") > 0 Then
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont, Brush1)
        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)
        End If

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        ' CurY = CurY + TxtHgt - 1
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1084" Then
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, LMargin, CurY, 2, PrintWidth, pFont)
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

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1076" And InStr(1, Trim(UCase(Cmp_Name)), "HOMSPUN INDUSTRIES") > 0 Then
            Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, pFont, Brushes.Red)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1076" And InStr(1, Trim(UCase(Cmp_Name)), "SPACEWEAR") > 0 Then
            Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, pFont, Brush1)
        Else
            ' Common_Procedures.Print_To_PrintDocument(e, Cmp_StateCap, CurX, CurY, 0, 0, pFont)
        End If

        strWidth = e.Graphics.MeasureString(Cmp_StateCap, pFont).Width

        CurX = CurX + strWidth
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1076" And InStr(1, Trim(UCase(Cmp_Name)), "HOMSPUN INDUSTRIES") > 0 Then
            Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont, Brushes.Red)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1076" And InStr(1, Trim(UCase(Cmp_Name)), "SPACEWEAR") > 0 Then
            Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont, Brush1)
        Else
            ' Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, CurX, CurY, 0, 0, pFont)
        End If

        strWidth = e.Graphics.MeasureString(Cmp_StateNm, pFont).Width
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        CurX = CurX + strWidth
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1076" And InStr(1, Trim(UCase(Cmp_Name)), "HOMSPUN INDUSTRIES") > 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, pFont, Brushes.Red)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1076" And InStr(1, Trim(UCase(Cmp_Name)), "SPACEWEAR") > 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, pFont, Brush1)
        Else
            '  Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, pFont)
        End If

        strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, pFont).Width

        CurX = CurX + strWidth
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1076" And InStr(1, Trim(UCase(Cmp_Name)), "HOMSPUN INDUSTRIES") > 0 Then
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont, Brushes.Red)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1076" And InStr(1, Trim(UCase(Cmp_Name)), "SPACEWEAR") > 0 Then
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont, Brush1)
        Else
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont)
        End If

        strWidth = e.Graphics.MeasureString(Cmp_GSTIN_No, pFont).Width
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        CurX = CurX + strWidth
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1076" And InStr(1, Trim(UCase(Cmp_Name)), "HOMSPUN INDUSTRIES") > 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_PanCap, CurX, CurY, 0, PrintWidth, p1Font, Brushes.Red)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1076" And InStr(1, Trim(UCase(Cmp_Name)), "SPACEWEAR") > 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_PanCap, CurX, CurY, 0, PrintWidth, p1Font, Brush1)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_PanCap, CurX, CurY, 0, PrintWidth, p1Font)
        End If

        strWidth = e.Graphics.MeasureString("     " & Cmp_PanCap, p1Font).Width

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1076" And InStr(1, Trim(UCase(Cmp_Name)), "HOMSPUN INDUSTRIES") > 0 Then
            CurX = CurX + strWidth
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, CurX, CurY, 0, 0, pFont, Brushes.Red)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1076" And InStr(1, Trim(UCase(Cmp_Name)), "SPACEWEAR") > 0 Then
            CurX = CurX + strWidth
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, CurX, CurY, 0, 0, pFont, Brush1)
        Else
            CurX = CurX + strWidth
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, CurX, CurY, 0, 0, pFont)
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1076" And InStr(1, Trim(UCase(Cmp_Name)), "HOMSPUN INDUSTRIES") > 0 Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_EMail), LMargin, CurY, 2, PrintWidth, pFont, Brushes.Red)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1076" And InStr(1, Trim(UCase(Cmp_Name)), "SPACEWEAR") > 0 Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_EMail), LMargin, CurY, 2, PrintWidth, pFont, Brush1)
        Else
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_Cap, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, LMargin + 55, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_EMail), PageWidth - 20, CurY, 1, 0, pFont)
        End If

        'If Cmp_State <> "" Then
        '    CurY = CurY + TxtHgt - 1
        '    Common_Procedures.Print_To_PrintDocument(e, Cmp_State, LMargin, CurY, 2, PrintWidth, pFont)
        'End If



        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Y1 = CurY
        Y2 = CurY + TxtHgt - 10 + TxtHgt + 5
        Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)

        CurY = CurY + TxtHgt - 14
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1076" And InStr(1, Trim(UCase(Cmp_Name)), "HOMSPUN INDUSTRIES") > 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE", LMargin, CurY, 2, PageWidth, p1Font, Brushes.Red)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1076" And InStr(1, Trim(UCase(Cmp_Name)), "SPACEWEAR") > 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE", LMargin, CurY, 2, PageWidth, p1Font, Brush1)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "TAX INVOICE", LMargin, CurY, 2, PageWidth, p1Font)
        End If

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            C2 = (PageWidth \ 2)
            'C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)

            W1 = e.Graphics.MeasureString("Reverse Charge (Y/N)    .", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width

            CurY1 = CurY + 10

            'Left Side
            p1Font = New Font("Calibri", 13, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Invoice No", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Sales_Invoice_No").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, p1Font)

            p1Font = New Font("Calibri", 11, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "Transport Mode", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Mode").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Sales_Invoice_Date").ToString), "dd-MM-yyyy").ToString, LMargin + W1 + 30, CurY1, 0, 0, pFont)


            Common_Procedures.Print_To_PrintDocument(e, "Vehicle Number", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Vehicle_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Vechile_IdNo").ToString)), LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Packing Slip No", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Transport Name", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Transport_IdNo").ToString)), LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)


            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Purchase Order Date", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_Date").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "G R No", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, (prn_HdDt.Rows(0).Item("Lr_No").ToString), LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "No.of Carton/Bag", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Noof_Carton").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Date Of Supply", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Date_And_Time_Of_Supply").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Reverse Charge (Yes/No)", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + W1 + 30, CurY1, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Place of Supply", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Despatch_To").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)
            CurY = CurY1 + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 10 + TxtHgt + 5
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)


            CurY1 = CurY + TxtHgt - 12

            Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF RECEIVER  (BILLED TO)", LMargin, CurY1, 2, C2, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "DETAILS OF CONSIGNEE  (SHIPPED TO)", LMargin + C2, CurY1, 2, PageWidth - C2, pFont)
            CurY = CurY1 + TxtHgt + 3


            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY
            CurY = CurY + 10

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("DelName").ToString, LMargin + C2 + 10, CurY, 0, 0, p1Font)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd1").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd2").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd3").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelAdd4").ToString, LMargin + C2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 12
            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)

                If Trim(prn_HdDt.Rows(0).Item("Pan_No").ToString) <> "" Then
                    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
                    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("Pan_No").ToString, LMargin + S1 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
                End If

            End If

            If Trim(prn_HdDt.Rows(0).Item("DelGSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + C2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelGSTinNo").ToString, LMargin + S1 + C2 + 30, CurY, 0, 0, pFont)
                If Trim(prn_HdDt.Rows(0).Item("Pan_No").ToString) <> "" Then
                    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
                    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("DelPanNo").ToString, LMargin + S1 + C2 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
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
                'If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
                '    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, pFont).Width
                '    Common_Procedures.Print_To_PrintDocument(e, "   Code : " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, LMargin + S1 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
                'End If
                Common_Procedures.Print_To_PrintDocument(e, "Code  :  " & prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("DelState_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "State", LMargin + C2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelState_Name").ToString, LMargin + S1 + C2 + 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Code  :  " & prn_HdDt.Rows(0).Item("Delivery_State_Code").ToString, LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + C2, LnAr(4), LMargin + C2, LnAr(2))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 40, LnAr(4), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 40, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, PageWidth - ClAr(9), LnAr(4), PageWidth - ClAr(9), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 110, LnAr(4), LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 110, LnAr(3))

            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 10 + TxtHgt + 5
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)

            p1Font = New Font("Calibri", 9, FontStyle.Regular)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "SNo", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PRODUCT DESCRIPTION", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SIZE", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "QTY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "UNIT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DISCOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TAXABLE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_GST_Format3_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)

        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim C1 As Single, W1 As Single
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim BmsInWrds As String
        Dim vprn_BlNos As String = ""
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim CurY1 As Single = 0
        Dim TaxAmt As Single = 0
        Dim TOT As Single = 0
        Dim Y1 As Single = 0, Y2 As Single = 0
        Dim vTaxPerc As Single = 0
        Dim vDueDatetxt As String = ""
        Dim vDueDatetxtWidth As Single = 0
        Dim vDueDateCurY As Single = 0

        Dim Clr1 As New Color
        Clr1 = Color.FromArgb(37, 111, 48)
        Dim Brush1 As New SolidBrush(Clr1)

        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Try

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt

                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + 30, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Quantity").ToString), "##########0"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If
            CurY = CurY + TxtHgt - 15

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY


            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6)

            W1 = e.Graphics.MeasureString("DISCOUNT    : ", pFont).Width

            ' If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- LOGUtEX

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

            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 15 + TxtHgt
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + C1, Y2)

            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS", LMargin, CurY, 2, C1, pFont)


            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "Discount @ " & Trim(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString) & "%", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Discount ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            CurY = CurY + TxtHgt - 15

            Common_Procedures.Print_To_PrintDocument(e, "BANK NAME  :  " & BankNm1, LMargin + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Packing ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Packing_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 3
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)
            ' e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15
            LnAr(8) = CurY
            p1Font = New Font("Calibri", 11, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "ACCOUNT No.  :  " & BankNm2, LMargin + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Freight", LMargin + C1 + 10, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "BRANCH  :  " & BankNm3, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "IFSC CODE  :  " & BankNm4, LMargin + ClAr(1) + ClAr(2) + 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Add / Less ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + 20, CurY, LMargin + ClAr(1) + ClAr(2) + 20, LnAr(9))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            CurY = CurY + TxtHgt - 15
            p1Font = New Font("Calibri", 10, FontStyle.Bold Or FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "Total  Before Tax  ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Total_Taxable_Value").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "1. Goods One sold will not be taken or replaced ", LMargin + 20, CurY, 0, 0, pFont)

            vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)

            If Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If

            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)

            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15

            Common_Procedures.Print_To_PrintDocument(e, "2. All Dispute subject to Tirupur Jurisdiction Only", LMargin + 20, CurY, 0, 0, pFont)

            If Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If

            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If



            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15

            Common_Procedures.Print_To_PrintDocument(e, "3. Certified this Invoice shows the actual price of the goods described and particulars ", LMargin + 20, CurY, 0, 0, pFont)

            If Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ " & Format(Val(vTaxPerc), "##########0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)

            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)

            Common_Procedures.Print_To_PrintDocument(e, "    given above are true and correct", LMargin + 20, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "4. Any Cheque Return, Shall Be Charged @300 Per Instance.", LMargin + 20, CurY, 0, 0, pFont)

            If Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "RoundOff ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If

            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)

            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 15 + TxtHgt + TxtHgt - 15 + TxtHgt - 0.5
            Common_Procedures.FillRegionRectangle(e, LMargin + C1, Y1, PageWidth, Y2)


            CurY = CurY + TxtHgt - 15

            vDueDatetxt = ""
            If Val(prn_HdDt.Rows(0).Item("Due_Days").ToString) <> 0 Then
                vDueDatetxt = "DUE DATE : " & Trim(prn_HdDt.Rows(0).Item("Due_Days").ToString) & " Days " & "(" & Trim(prn_HdDt.Rows(0).Item("Due_Date").ToString) & ")"
                'Common_Procedures.Print_To_PrintDocument(e, "DUE DATE : " & Trim(prn_HdDt.Rows(0).Item("Due_Days").ToString) & " Days " & "(" & Trim(prn_HdDt.Rows(0).Item("Due_Date").ToString) & ")", LMargin + C1 - 10, CurY - 5, 1, 0, p1Font)
            Else
                vDueDatetxt = "DUE DATE : IMMEDIATE"
                'Common_Procedures.Print_To_PrintDocument(e, "DUE DATE : IMMEDIATE", LMargin + C1 - 10, CurY - 5, 1, 0, p1Font)
            End If
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            vDueDatetxtWidth = e.Graphics.MeasureString(vDueDatetxt, p1Font).Width
            vDueDateCurY = CurY - 5
            Common_Procedures.Print_To_PrintDocument(e, vDueDatetxt, LMargin + C1 - 10, vDueDateCurY, 1, 0, p1Font)


            p1Font = New Font("Calibri", 9, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "GRAND TOTAL ", LMargin + C1 + 10, CurY + 10, 0, 0, p1Font)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString)), PageWidth - 10, CurY + 10, 1, 0, p1Font)
            End If

            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth - (ClAr(9) + ClAr(8) + ClAr(7)), CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin + C1 - 10 - vDueDatetxtWidth - 10, vDueDateCurY - 6, LMargin + C1, vDueDateCurY - 6)
            e.Graphics.DrawLine(Pens.Black, LMargin + C1 - 10 - vDueDatetxtWidth - 10, CurY, LMargin + C1 - 10 - vDueDatetxtWidth - 10, vDueDateCurY - 6)


            LnAr(10) = CurY

            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 15 + TxtHgt
            Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, Y2)

            CurY = CurY + TxtHgt - 15
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Amount in Words - INR", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "E. & O.E", LMargin + C1 - 10, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, LnAr(10))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            Y1 = CurY + 0.5
            Y2 = CurY + TxtHgt - 15 + TxtHgt - 0.5
            Common_Procedures.FillRegionRectangle(e, LMargin + C1, Y1, PageWidth, Y2)

            CurY = CurY + TxtHgt - 15

            Common_Procedures.Print_To_PrintDocument(e, "GST on Reverse Charge", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, p1Font)

            If is_LastPage = True Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
                Common_Procedures.Print_To_PrintDocument(e, " " & StrConv(BmsInWrds, VbStrConv.ProperCase), LMargin + 10, CurY + 5, 0, 0, p1Font)
            End If



            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth - (ClAr(9) + ClAr(8) + ClAr(7)), CurY)
            LnAr(14) = CurY


            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            CurY = CurY + TxtHgt - 15
            p1Font = New Font("Calibri", 12, FontStyle.Bold)

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1076" Then
                If InStr(1, Trim(UCase(Cmp_Name)), "HOMSPUN INDUSTRIES") > 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY - 5, 1, 0, p1Font, Brushes.Red)
                ElseIf InStr(1, Trim(UCase(Cmp_Name)), "SPACEWEAR") > 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY - 5, 1, 0, p1Font, Brush1)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY - 5, 1, 0, p1Font)
                End If
            End If

            CurY = CurY + TxtHgt
            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 50, CurY, 0, 0, p1Font)
            End If
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Prepared by ", LMargin + 50, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Checked by ", LMargin + 350, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory ", PageWidth - 10, CurY, 1, 0, p1Font)
            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + 15, CurY, LMargin + ClAr(1) + ClAr(2) + 15, LnAr(14))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub txt_Freight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Freight.TextChanged
        Total_Calculation()
        NetAmount_Calculation()
    End Sub



    Private Sub txt_DueDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DueDate.KeyDown
        If e.KeyValue = 40 Then
            If Trim(UCase(cbo_Type.Text)) = "PACKING SLIP" Then

                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(5)
                    dgv_Details.CurrentCell.Selected = True
                Else
                    txt_DiscPerc.Focus()
                End If

            Else
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                    dgv_Details.CurrentCell.Selected = True
                Else

                    If dgv_OrderDetails.Rows.Count > 0 Then
                        dgv_OrderDetails.Focus()
                        dgv_OrderDetails.CurrentCell = dgv_OrderDetails.Rows(0).Cells(1)
                        dgv_OrderDetails.CurrentCell.Selected = True

                    Else
                        txt_DiscPerc.Focus()



                    End If
                End If
            End If
        End If
        If e.KeyValue = 38 Then
            txt_DueDays.Focus()
        End If

    End Sub

    Private Sub txt_DueDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DueDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(cbo_Type.Text)) = "PACKING SLIP" Then

                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(5)
                    dgv_Details.CurrentCell.Selected = True
                Else
                    txt_DiscPerc.Focus()
                End If

            Else
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                    dgv_Details.CurrentCell.Selected = True
                Else

                    If dgv_OrderDetails.Rows.Count > 0 Then
                        dgv_OrderDetails.Focus()
                        dgv_OrderDetails.CurrentCell = dgv_OrderDetails.Rows(0).Cells(1)
                        dgv_OrderDetails.CurrentCell.Selected = True

                    Else
                        txt_DiscPerc.Focus()



                    End If


                End If
            End If
        End If


    End Sub

    Private Sub txt_DueDays_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DueDays.KeyDown
        If e.KeyValue = 40 Then
            If Trim(UCase(cbo_Type.Text)) = "PACKING SLIP" Then

                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(5)
                    dgv_Details.CurrentCell.Selected = True
                Else
                    txt_DiscPerc.Focus()
                End If

            Else
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                    dgv_Details.CurrentCell.Selected = True
                Else

                    If dgv_OrderDetails.Rows.Count > 0 Then
                        dgv_OrderDetails.Focus()
                        dgv_OrderDetails.CurrentCell = dgv_OrderDetails.Rows(0).Cells(1)
                        dgv_OrderDetails.CurrentCell.Selected = True

                    Else
                        txt_DiscPerc.Focus()

                    End If
                End If
            End If
        End If
        If e.KeyValue = 38 Then
            cbo_DeliveryTo.Focus()
        End If

    End Sub

    Private Sub txt_DueDays_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DueDays.KeyPress
        If Asc(e.KeyChar) = 13 Then

            If Trim(UCase(cbo_Type.Text)) = "PACKING SLIP" Then

                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(5)
                    dgv_Details.CurrentCell.Selected = True
                Else
                    txt_DiscPerc.Focus()
                End If

            Else
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                    dgv_Details.CurrentCell.Selected = True
                Else

                    If dgv_OrderDetails.Rows.Count > 0 Then
                        dgv_OrderDetails.Focus()
                        dgv_OrderDetails.CurrentCell = dgv_OrderDetails.Rows(0).Cells(1)
                        dgv_OrderDetails.CurrentCell.Selected = True

                    Else
                        txt_DiscPerc.Focus()



                    End If


                End If
            End If
        End If

    End Sub

    Private Sub DueDate_Calculation()

        txt_DueDate.Text = ""
        If IsDate(dtp_InvocieDate.Text) = True And Val(txt_DueDays.Text) >= 0 Then
            txt_DueDate.Text = DateAdd("d", Val(txt_DueDays.Text), Convert.ToDateTime(dtp_InvocieDate.Text))
        End If

    End Sub

    Private Sub txt_DueDays_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_DueDays.TextChanged
        DueDate_Calculation()
    End Sub

    Private Sub dtp_InvocieDate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtp_InvocieDate.ValueChanged
        DueDate_Calculation()
    End Sub

    Public Sub BreakString(ByRef OriginalString As String, ByVal ResultArr() As String, ByRef ArrCnt As Integer, ByRef StrLength As Integer)

        If Len(Trim(OriginalString)) <= StrLength Then
            ResultArr(0) = OriginalString
            ArrCnt = 0
            Exit Sub
        End If

        Dim tmpEndPos As Integer = 0
        Dim tmpStr As String = ""

        For I As Integer = 0 To Len(OriginalString)
            If I > 0 Then
                ArrCnt = ArrCnt + 1
            End If
            tmpStr = Mid(OriginalString, tmpEndPos + 1, StrLength)
            If Len(tmpStr) < StrLength Then
                ResultArr(ArrCnt) = tmpStr
                Exit Sub
            Else
                For J As Integer = StrLength To 1 Step -1
                    If Mid$(tmpStr, J, 1) = " " Or Mid$(Trim(tmpStr), J, 1) = "," Or Mid$(Trim(tmpStr), J, 1) = "." Or Mid$(Trim(tmpStr), J, 1) = "-" Or Mid$(Trim(tmpStr), J, 1) = "/" Or Mid$(Trim(tmpStr), J, 1) = "_" Or Mid$(Trim(tmpStr), J, 1) = "(" Or Mid$(Trim(tmpStr), J, 1) = ")" Or Mid$(Trim(tmpStr), J, 1) = "\" Or Mid$(Trim(tmpStr), J, 1) = "[" Or Mid$(Trim(tmpStr), J, 1) = "]" Or Mid$(Trim(tmpStr), J, 1) = "{" Or Mid$(Trim(tmpStr), J, 1) = "}" Then
                        ResultArr(ArrCnt) = Microsoft.VisualBasic.Left(tmpStr, J)
                        tmpEndPos = tmpEndPos + J
                        I = J + 1
                        GoTo a
                    End If
                Next
                ResultArr(ArrCnt) = tmpStr
                tmpEndPos = tmpEndPos + StrLength
                I = I + StrLength
            End If
a:
        Next

    End Sub

    Private Sub get_Item_Rate_Unit_from_Master()
        'Dim da As SqlClient.SqlDataAdapter
        'Dim dt As New DataTable

        'If Trim(UCase(vCbo_GrdItmNm)) <> Trim(UCase(cbo_Grid_ItemName.Text)) Then
        '    vCbo_GrdItmNm = cbo_Grid_ItemName.Text
        '    da = New SqlClient.SqlDataAdapter("select a.*, b.unit_name from item_head a LEFT OUTER JOIN unit_head b ON a.unit_idno = b.unit_idno where a.item_name = '" & Trim(cbo_Grid_ItemName.Text) & "'", con)
        '    dt = New DataTable
        '    da.Fill(dt)
        '    If dt.Rows.Count > 0 Then
        '        If IsDBNull(dt.Rows(0)("unit_name").ToString) = False Then
        '            dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(4).Value = dt.Rows(0)("unit_name").ToString
        '        End If
        '        'If IsDBNull(dt.Rows(0)("sales_rate").ToString) = False Then
        '        '    txt_Rate.Text = dt.Rows(0)("Sales_Rate").ToString
        '        'End If
        '    End If
        '    dt.Dispose()
        '    da.Dispose()
        'End If

    End Sub

    Private Sub cbo_Through_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbo_Through.SelectedIndexChanged

    End Sub

    Private Sub cbo_DespTo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbo_DespTo.SelectedIndexChanged

    End Sub

    Private Sub Label33_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label33.Click

    End Sub

    Private Sub cbo_SalesAc_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbo_SalesAc.SelectedIndexChanged

    End Sub
End Class
