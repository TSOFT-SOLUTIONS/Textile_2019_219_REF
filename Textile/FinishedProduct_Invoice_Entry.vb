Public Class FinishedProduct_Invoice_Entry
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "FPINV-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control

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
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1
    Private vcbo_KeyDwnVal As Double

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
        pnl_BaleDetails.Visible = False

        vmskOldText = ""
        vmskSelStrt = -1


        lbl_InvoiceNo.Text = ""
        lbl_InvoiceNo.ForeColor = Color.Black
        msk_Date.Text = ""
        dtp_Date.Text = ""
        cbo_Ledger.Text = ""

        txt_OrderNo.Text = ""
        dtp_OrderDate.Text = ""
        txt_DcNo.Text = ""
        cbo_Area.Text = ""

        cbo_Agent.Text = ""
        cbo_SalesAc.Text = ""
        cbo_Through.Text = "DIRECT"

        txt_LrNo.Text = ""
        dtp_LrDate.Text = ""
        cbo_Transport.Text = ""

        cbo_VatAc.Text = ""
        txt_PreparedBy.Text = Common_Procedures.User.Name
        txt_Note.Text = ""
        chk_NetBill.Checked = False
        chk_Inc_Tax.Checked = False

        lbl_BaleNos.Text = ""
        lbl_GrossAmount.Text = ""
        lbl_AssessableValue.Text = ""
        lbl_OrderCode.Text = ""

        txt_DiscPerc.Text = ""
        lbl_DiscAmount.Text = ""
        cbo_TaxType.Text = "-NIL-"
        cbo_Type.Text = "ORDER"

        txt_TaxPerc.Text = ""
        lbl_TaxAmount.Text = ""

        txt_Packing.Text = ""
        txt_AddLess.Text = ""
        lbl_RoundOff.Text = ""
        lbl_NetAmount.Text = "0.00"
        lbl_AmountInWords.Text = "Rupees  :  "  ' "Amount In Words : "

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        dgv_BaleDetails.Rows.Clear()
        dgv_BaleDetails_Total.Rows.Clear()
        dgv_BaleDetails_Total.Rows.Add()

        dgv_OrderDetails.Rows.Clear()
        dgv_OrderDetails_Total.Rows.Clear()
        dgv_OrderDetails_Total.Rows.Add()

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

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as PartyName from FinishedProduct_Invoice_Head a INNER JOIN Ledger_Head b ON a.Ledger_IdNo = b.Ledger_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.FinishedProduct_Invoice_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_InvoiceNo.Text = dt1.Rows(0).Item("FinishedProduct_Invoice_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("FinishedProduct_Invoice_Date").ToString
                msk_Date.Text = dtp_Date.Text
                cbo_Ledger.Text = dt1.Rows(0).Item("PartyName").ToString

                txt_OrderNo.Text = dt1.Rows(0).Item("Order_No").ToString
                dtp_OrderDate.Text = dt1.Rows(0).Item("Order_Date").ToString
                txt_DcNo.Text = dt1.Rows(0).Item("Dc_No").ToString
                cbo_Area.Text = Common_Procedures.Area_IdNoToName(con, Val(dt1.Rows(0).Item("Area_IdNo").ToString))

                cbo_Agent.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Agent_IdNo").ToString))
                cbo_SalesAc.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("SalesAc_IdNo").ToString))
                cbo_Through.Text = dt1.Rows(0).Item("Through_Name").ToString

                txt_LrNo.Text = dt1.Rows(0).Item("Lr_No").ToString
                dtp_LrDate.Text = dt1.Rows(0).Item("Lr_Date").ToString
                cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))
                cbo_Type.Text = dt1.Rows(0).Item("Selection_Type").ToString

                lbl_BaleNos.Text = dt1.Rows(0).Item("Bale_Nos").ToString

                lbl_GrossAmount.Text = Format(Val(dt1.Rows(0).Item("Total_Amount").ToString), "#########0.00")
                txt_DiscPerc.Text = Val(dt1.Rows(0).Item("Discount_Percentage").ToString)
                lbl_DiscAmount.Text = Format(Val(dt1.Rows(0).Item("Discount_Amount").ToString), "#########0.00")
                lbl_AssessableValue.Text = Format(Val(dt1.Rows(0).Item("Assessable_Value").ToString), "#########0.00")
                cbo_TaxType.Text = dt1.Rows(0).Item("Tax_Type").ToString
                If Trim(cbo_TaxType.Text) = "" Then cbo_TaxType.Text = "-NIL-"
                txt_TaxPerc.Text = Val(dt1.Rows(0).Item("Tax_Percentage").ToString)
                lbl_TaxAmount.Text = Format(Val(dt1.Rows(0).Item("Tax_Amount").ToString), "#########0.00")

                txt_Packing.Text = Format(Val(dt1.Rows(0).Item("Packing_Amount").ToString), "#########0.00")
                txt_AddLess.Text = Format(Val(dt1.Rows(0).Item("AddLess_Amount").ToString), "#########0.00")
                lbl_RoundOff.Text = Format(Val(dt1.Rows(0).Item("RoundOff_Amount").ToString), "#########0.00")
                lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(dt1.Rows(0).Item("Net_Amount").ToString))

                cbo_VatAc.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("VatAc_IdNo").ToString))
                txt_PreparedBy.Text = dt1.Rows(0).Item("Prepared_By").ToString
                txt_Note.Text = dt1.Rows(0).Item("Note").ToString
                If Val(dt1.Rows(0).Item("NetBill_Status").ToString) = 1 Then chk_NetBill.Checked = True
                If Val(dt1.Rows(0).Item("Include_Tax").ToString) = 1 Then chk_Inc_Tax.Checked = True

                lbl_OrderCode.Text = dt1.Rows(0).Item("FinishedProduct_Order_Code").ToString

                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Processed_Item_Name, c.Processed_Item_SalesName, d.Unit_Name from FinishedProduct_Invoice_Details a INNER JOIN Processed_Item_Head b ON a.FinishedProduct_IdNo = b.Processed_Item_IdNo LEFT OUTER JOIN Processed_Item_SalesName_Head c ON a.Processed_Item_SalesIdNo = c.Processed_Item_SalesIdNo Left Outer join Unit_Head d ON a.Unit_IdNo = d.Unit_IdNo Where a.FinishedProduct_Invoice_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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
                            .Rows(n).Cells(1).Value = dt2.Rows(i).Item("Processed_Item_Name").ToString
                            .Rows(n).Cells(2).Value = dt2.Rows(i).Item("Processed_Item_SalesName").ToString
                            .Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Quantity").ToString)
                            .Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                            .Rows(n).Cells(5).Value = dt2.Rows(i).Item("Unit_Name").ToString
                            .Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")
                            .Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")

                        Next i

                    End If

                    If .RowCount = 0 Then .Rows.Add()

                End With

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(3).Value = Val(dt1.Rows(0).Item("Total_Quantity").ToString)
                    .Rows(0).Cells(4).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                    .Rows(0).Cells(7).Value = Format(Val(dt1.Rows(0).Item("Total_Amount").ToString), "########0.00")
                End With

                da2 = New SqlClient.SqlDataAdapter("Select a.* from FinishedProduct_Invoice_Bale_Details a Where a.FinishedProduct_Invoice_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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
                    .Rows(0).Cells(2).Value = Val(dt1.Rows(0).Item("Total_Quantity").ToString)
                    .Rows(0).Cells(3).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                End With


                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Processed_Item_Name from FinishedProduct_Invoice_Order_Details a INNER JOIN Processed_Item_Head b ON b.Processed_Item_IdNo <> 0 and a.FinishedProduct_IdNo = b.Processed_Item_IdNo INNER JOIN FinishedProduct_Order_Head c ON a.FinishedProduct_Order_Code = c.FinishedProduct_Order_Code Where a.FinishedProduct_Invoice_Code = '" & Trim(NewCode) & "' Order by b.Processed_Item_Name, c.FinishedProduct_Order_Date, c.for_OrderBy, c.FinishedProduct_Order_No, c.FinishedProduct_Order_Code", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_OrderDetails

                    .Rows.Clear()
                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1
                            .Rows(n).Cells(0).Value = Val(SNo)
                            .Rows(n).Cells(1).Value = dt2.Rows(i).Item("Processed_Item_Name").ToString
                            .Rows(n).Cells(2).Value = dt2.Rows(i).Item("FinishedProduct_Order_No").ToString
                            .Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Quantity").ToString)
                            .Rows(n).Cells(4).Value = dt2.Rows(i).Item("FinishedProduct_Order_Code").ToString

                        Next i

                    End If

                End With

                With dgv_OrderDetails_Total
                    If .RowCount = 0 Then .Rows.Add()
                    NoCalc_Status = False
                    Total_OrderItemCalculation()
                    NoCalc_Status = True
                    '.Rows(0).Cells(3).Value = Val(dt1.Rows(0).Item("Total_Quantity").ToString)
                End With

                dt2.Dispose()
                da2.Dispose()

            End If

            dt1.Dispose()
            da1.Dispose()

            Grid_Cell_DeSelect()

            dgv_ActCtrlName = ""

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        NoCalc_Status = False

        If msk_Date.Enabled = True And msk_Date.Visible = True Then msk_Date.Focus()

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim msktxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is Button Or TypeOf Me.ActiveControl Is CheckBox Or TypeOf Me.ActiveControl Is MaskedTextBox Then
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
            msktxbx = Me.ActiveControl
            msktxbx.SelectionStart = 0
        End If
        If Me.ActiveControl.Name <> dgv_Details.Name Then
            Common_Procedures.Hide_CurrentStock_Display()
        End If

        Grid_Cell_DeSelect()

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is CheckBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
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
        dgv_Details.CurrentCell.Selected = False
        dgv_Details_Total.CurrentCell.Selected = False
        dgv_OrderDetails.CurrentCell.Selected = False
        dgv_OrderDetails_Total.CurrentCell.Selected = False
        dgv_BaleDetails.CurrentCell.Selected = False
        dgv_BaleDetails_Total.CurrentCell.Selected = False
        dgv_Selection.CurrentCell.Selected = False
        dgv_Filter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub FinishedProduct_Invoice_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Ledger.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Ledger.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Agent.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "AGENT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Agent.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Area.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "AREA" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Area.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable

        Me.Text = ""

        con.Open()

        cbo_TaxType.Items.Clear()
        cbo_TaxType.Items.Add("-NIL-")
        cbo_TaxType.Items.Add("VAT")
        cbo_TaxType.Items.Add("CST")

        cbo_Through.Items.Clear()
        cbo_Through.Items.Add(" ")
        cbo_Through.Items.Add("DIRECT")
        cbo_Through.Items.Add("BANK")
        cbo_Through.Items.Add("AGENT")

        cbo_Type.Items.Clear()
        cbo_Type.Items.Add(" ")
        cbo_Type.Items.Add("DIRECT")
        cbo_Type.Items.Add("ORDER")

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


        pnl_Print.Visible = False
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2
        pnl_Print.BringToFront()

        pnl_BaleDetails.Visible = False
        pnl_BaleDetails.Left = (Me.Width - pnl_BaleDetails.Width) \ 2
        pnl_BaleDetails.Top = (Me.Height - pnl_BaleDetails.Height) \ 2
        pnl_BaleDetails.BringToFront()

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Ledger.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OrderNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Through.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_OrderDate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DcNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Area.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Type.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Agent.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_SalesAc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LrNo.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_LrDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DiscPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TaxPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Packing.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLess.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TaxType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VatAc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PreparedBy.GotFocus, AddressOf ControlGotFocus
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

        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Ledger.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OrderNo.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_OrderDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DcNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Area.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Agent.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Through.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_SalesAc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LrNo.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_LrDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DiscPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TaxType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TaxPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Packing.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AddLess.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VatAc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PreparedBy.LostFocus, AddressOf ControlLostFocus
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

        AddHandler dtp_OrderDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DcNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_LrNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_LrDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TaxPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Packing.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AddLess.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PreparedBy.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Note.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_OrderNo.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_OrderNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_OrderDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DcNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_LrNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_LrDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DiscPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TaxPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Packing.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AddLess.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PreparedBy.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress

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
        Common_Procedures.Hide_CurrentStock_Display()

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

                ElseIf pnl_BaleDetails.Visible = True Then
                    btn_CloseBaleDetails_Click(sender, e)
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

            ElseIf ActiveControl.Name = dgv_BaleDetails.Name Then
                dgv1 = dgv_BaleDetails

            ElseIf dgv_BaleDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_BaleDetails

            ElseIf Trim(UCase(dgv_ActCtrlName)) = Trim(UCase(dgv_Details.Name.ToString)) Then
                dgv1 = dgv_Details

            ElseIf Trim(UCase(dgv_ActCtrlName)) = Trim(UCase(dgv_OrderDetails.Name.ToString)) Then
                dgv1 = dgv_OrderDetails

            ElseIf Trim(UCase(dgv_ActCtrlName)) = Trim(UCase(dgv_BaleDetails.Name.ToString)) Then
                dgv1 = dgv_BaleDetails

            End If

            If IsNothing(dgv1) = False Then

                With dgv1
                    If dgv1.Name = dgv_Details.Name Then

                        If keyData = Keys.Enter Or keyData = Keys.Down Then
                            If .CurrentCell.ColumnIndex >= .ColumnCount - 5 Then
                                If .CurrentCell.RowIndex = .RowCount - 1 Then

                                    If dgv_OrderDetails.Rows.Count > 0 Then
                                        dgv_OrderDetails.Focus()
                                        dgv_OrderDetails.CurrentCell = dgv_OrderDetails.Rows(0).Cells(3)

                                    Else
                                        txt_DiscPerc.Focus()

                                    End If

                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(6)

                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(6)

                            End If

                            Return True

                        ElseIf keyData = Keys.Up Then

                            If .CurrentCell.ColumnIndex <= .ColumnCount - 3 Then
                                If .CurrentCell.RowIndex = 0 Then
                                    cbo_Transport.Focus()

                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 3)

                                End If

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
                                        cbo_Transport.Focus()
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

        'f Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Invoice_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Invoice_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

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

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), trans) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)

            cmd.CommandText = "Update FinishedProduct_Order_Details set Invoice_Quantity = a.Invoice_Quantity - b.Quantity from FinishedProduct_Order_Details a, FinishedProduct_Invoice_Order_Details b Where b.FinishedProduct_Invoice_Code = '" & Trim(NewCode) & "' and a.FinishedProduct_Order_Code = b.FinishedProduct_Order_Code and a.FinishedProduct_IdNo = b.FinishedProduct_IdNo"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Item_PackingSlip_Head set Invoice_Code = '', Invoice_Increment = Invoice_Increment - 1 Where Invoice_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from FinishedProduct_Invoice_Order_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and FinishedProduct_Invoice_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from FinishedProduct_Invoice_Bale_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and FinishedProduct_Invoice_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from FinishedProduct_Invoice_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and FinishedProduct_Invoice_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from FinishedProduct_Invoice_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and FinishedProduct_Invoice_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            'If Val(Common_Procedures.settings.NegativeStock_Restriction) = 1 Then

            '    If Common_Procedures.Check_is_Negative_Stock_Status(con, trans) = True Then Exit Sub

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
            If InStr(1, Trim(LCase(ex.Message)), Trim(LCase("ck_finishedproduct_order_details_1"))) > 0 Then
                MessageBox.Show("Invalid Quantity - Invocie Quantity greater than Order Quantity", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf InStr(1, Trim(LCase(ex.Message)), Trim(LCase("ck_finishedproduct_order_details_2"))) > 0 Then
                MessageBox.Show("Invalid Invoice Quantity in Order Details", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If

        Finally
            If msk_Date.Enabled = True And msk_Date.Visible = True Then msk_Date.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or (Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ) order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
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

            da = New SqlClient.SqlDataAdapter("select top 1 FinishedProduct_Invoice_No from FinishedProduct_Invoice_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and FinishedProduct_Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_VAT_GST_Type <> 'GST'  Order by for_Orderby, FinishedProduct_Invoice_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 FinishedProduct_Invoice_No from FinishedProduct_Invoice_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and FinishedProduct_Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_VAT_GST_Type <> 'GST' Order by for_Orderby, FinishedProduct_Invoice_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 FinishedProduct_Invoice_No from FinishedProduct_Invoice_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and FinishedProduct_Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_VAT_GST_Type <> 'GST' Order by for_Orderby desc, FinishedProduct_Invoice_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 FinishedProduct_Invoice_No from FinishedProduct_Invoice_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and FinishedProduct_Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Entry_VAT_GST_Type <> 'GST' Order by for_Orderby desc, FinishedProduct_Invoice_No desc", con)
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

            lbl_InvoiceNo.Text = Common_Procedures.get_MaxCode(con, "FinishedProduct_Invoice_Head", "FinishedProduct_Invoice_Code", "For_OrderBy", "Entry_VAT_GST_Type <> 'GST'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_InvoiceNo.ForeColor = Color.Red
            msk_Date.Text = Date.Today.ToShortDateString
            Da1 = New SqlClient.SqlDataAdapter("select Top 1 a.*, b.ledger_name as SalesAcName, c.ledger_name as TaxAcName from FinishedProduct_Invoice_Head a LEFT OUTER JOIN Ledger_Head b ON a.SalesAc_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.VatAc_IdNo = c.Ledger_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.FinishedProduct_Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by a.for_Orderby desc, a.FinishedProduct_Invoice_No desc", con)
            Da1.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then
                If Dt1.Rows.Count > 0 Then
                    If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)

                        If Dt1.Rows(0).Item("FinishedProduct_Invoice_Date").ToString <> "" Then msk_Date.Text = Dt1.Rows(0).Item("FinishedProduct_Invoice_Date").ToString
                    End If
                End If
                Dt1.Clear()
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

            If msk_Date.Enabled And msk_Date.Visible Then
                msk_Date.Focus()
                msk_Date.SelectionStart = 0
            End If

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        Try

            inpno = InputBox("Enter Invoice No.", "FOR FINDING...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select FinishedProduct_Invoice_No from FinishedProduct_Invoice_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and FinishedProduct_Invoice_Code = '" & Trim(InvCode) & "' and Entry_VAT_GST_Type <> 'GST'", con)
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

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Invoice_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Invoice_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Invoice No.", "FOR NEW INVOICE NO. INSERTION...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select FinishedProduct_Invoice_No from FinishedProduct_Invoice_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and FinishedProduct_Invoice_Code = '" & Trim(InvCode) & "'", con)
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
        Dim FP_ID As Integer = 0
        Dim PSalNm_ID As Integer = 0
        Dim Trans_ID As Integer
        Dim Ag_ID As Integer = 0
        Dim VatAc_ID As Integer = 0
        Dim Ar_ID As Integer = 0
        Dim Unt_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim EntID As String = ""
        Dim Dup_FPname As String = ""
        Dim PBlNo As String = ""
        Dim vTotBls As Single, vTotQty As Single, vTotMtrs As Single
        Dim vBlsTotQty As Single, vBlsTotMtrs As Single
        Dim vOrdTotQty As Single
        Dim Nr As Long
        Dim Cr_ID As Integer = 0
        Dim Dr_ID As Integer = 0
        Dim NtBl_STS As Integer = 0
        Dim IncluTax_STS As Integer = 0
        Dim eXmSG As String = ""
        Dim fpitmnm As String = ""


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Invoice_Entry, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
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
        Ar_ID = Common_Procedures.Area_NameToIdNo(con, cbo_Area.Text)

        If SalAc_ID = 0 And Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            MessageBox.Show("Invalid Sales A/c name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_SalesAc.Enabled And cbo_SalesAc.Visible Then cbo_SalesAc.Focus()
            Exit Sub
        End If

        With dgv_Details

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(4).Value) <> 0 Then

                    FP_ID = Common_Procedures.Processed_Item_NameToIdNo(con, .Rows(i).Cells(1).Value)
                    If FP_ID = 0 Then
                        MessageBox.Show("Invalid Finished Product Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    If InStr(1, Trim(UCase(Dup_FPname)), "~" & Trim(UCase(.Rows(i).Cells(1).Value)) & "~") > 0 Then
                        MessageBox.Show("Duplicate FINISHED PRODUCT NAME ", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    Dup_FPname = Trim(Dup_FPname) & "~" & Trim(UCase(.Rows(i).Cells(1).Value)) & "~"

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

        If VatAc_ID = 0 And Val(lbl_TaxAmount.Text) <> 0 Then
            MessageBox.Show("Invalid Vat A/c name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_VatAc.Enabled And cbo_VatAc.Visible Then cbo_VatAc.Focus()
            Exit Sub
        End If

        NoCalc_Status = False
        Total_Calculation()

        vTotBls = 0 : vTotQty = 0 : vTotMtrs = 0
        vBlsTotQty = 0 : vBlsTotMtrs = 0
        vOrdTotQty = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotQty = Val(dgv_Details_Total.Rows(0).Cells(3).Value())
            vTotMtrs = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
        End If

        If dgv_BaleDetails_Total.RowCount > 0 Then
            vTotBls = Val(dgv_BaleDetails_Total.Rows(0).Cells(1).Value())
            vBlsTotQty = Val(dgv_BaleDetails_Total.Rows(0).Cells(2).Value())
            vBlsTotMtrs = Val(dgv_BaleDetails_Total.Rows(0).Cells(3).Value())
        End If

        If dgv_OrderDetails_Total.RowCount > 0 Then
            vOrdTotQty = Val(dgv_OrderDetails_Total.Rows(0).Cells(3).Value())
        End If

        If Val(vTotQty) <> Val(vBlsTotQty) Or Val(vTotMtrs) <> Val(vBlsTotMtrs) Then
            MessageBox.Show("Mismatch of Quantity in Invoice and Bale Details", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
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

        NtBl_STS = 0
        If chk_NetBill.Checked = True Then NtBl_STS = 1

        IncluTax_STS = 0
        If chk_Inc_Tax.Checked = True Then IncluTax_STS = 1


        If Trim(UCase(cbo_Type.Text)) <> "ORDER" Then
            lbl_OrderCode.Text = ""
        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_InvoiceNo.Text = Common_Procedures.get_MaxCode(con, "FinishedProduct_Invoice_Head", "FinishedProduct_Invoice_Code", "For_OrderBy", "Entry_VAT_GST_Type <> 'GST'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@InvoiceDate", dtp_Date.Value.Date)

            If New_Entry = True Then

                cmd.CommandText = "Insert into FinishedProduct_Invoice_Head ( FinishedProduct_Invoice_Code ,               Company_IdNo       ,     FinishedProduct_Invoice_No    ,                     for_OrderBy                                            , FinishedProduct_Invoice_Date  ,          Ledger_IdNo    ,          Area_IdNo     ,             Order_No            ,             Order_Date            ,            Dc_No             ,          Agent_IdNo    ,            SalesAc_IdNo   ,           Lr_No              ,               Lr_Date          ,        Transport_IdNo     ,           Total_Bales    ,          Total_Quantity  ,          Total_Meters     ,               Total_Amount            ,             Discount_Percentage    ,              Discount_Amount         ,              Assessable_Value             ,             Tax_Type            ,             Tax_Percentage        ,             Tax_Amount              ,           VatAc_IdNo      ,              Packing_Amount       ,              AddLess_Amount       ,               RoundOff_Amount      ,              Net_Amount             ,               Prepared_By          ,               Note           ,               NetBill_Status    ,           Bale_Nos               ,            Through_Name         ,         Selection_Type       ,  FinishedProduct_Order_Code        , Include_Tax ) " & _
                                    "   Values                              (   '" & Trim(NewCode) & "'    , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ",       @InvoiceDate            , " & Str(Val(Led_ID)) & ", " & Str(Val(Ar_ID)) & ", '" & Trim(txt_OrderNo.Text) & "', '" & Trim(dtp_OrderDate.Text) & "', '" & Trim(txt_DcNo.Text) & "', " & Str(Val(Ag_ID)) & ", " & Str(Val(SalAc_ID)) & ", '" & Trim(txt_LrNo.Text) & "', '" & Trim(dtp_LrDate.Text) & "', " & Str(Val(Trans_ID)) & ", " & Str(Val(vTotBls)) & ", " & Str(Val(vTotQty)) & ", " & Str(Val(vTotMtrs)) & ", " & Str(Val(lbl_GrossAmount.Text)) & ", " & Str(Val(txt_DiscPerc.Text)) & ", " & Str(Val(lbl_DiscAmount.Text)) & ", " & Str(Val(lbl_AssessableValue.Text)) & ", '" & Trim(cbo_TaxType.Text) & "', " & Str(Val(txt_TaxPerc.Text)) & ", " & Str(Val(lbl_TaxAmount.Text)) & ", " & Str(Val(VatAc_ID)) & ", " & Str(Val(txt_Packing.Text)) & ", " & Str(Val(txt_AddLess.Text)) & ", " & Str(Val(lbl_RoundOff.Text)) & ", " & Str(Val(CSng(lbl_NetAmount.Text))) & ", '" & Trim(txt_PreparedBy.Text) & "', '" & Trim(txt_Note.Text) & "', " & Str(Val(NtBl_STS)) & ", '" & Trim(lbl_BaleNos.Text) & "' , '" & Trim(cbo_Through.Text) & "', '" & Trim(cbo_Type.Text) & "', '" & Trim(lbl_OrderCode.Text) & "' ,  " & Str(Val(IncluTax_STS)) & ") "
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update FinishedProduct_Invoice_Head set FinishedProduct_Invoice_Date = @InvoiceDate, Ledger_IdNo = " & Str(Val(Led_ID)) & ", Area_IdNo = " & Str(Val(Ar_ID)) & ", Order_No = '" & Trim(txt_OrderNo.Text) & "', Order_Date = '" & Trim(dtp_OrderDate.Text) & "', Dc_No = '" & Trim(txt_DcNo.Text) & "', Agent_IdNo = " & Str(Val(Ag_ID)) & ", SalesAc_IdNo = " & Str(Val(SalAc_ID)) & ", Lr_No = '" & Trim(txt_LrNo.Text) & "', Lr_Date = '" & Trim(dtp_LrDate.Text) & "', Transport_IdNo = " & Str(Val(Trans_ID)) & ", Total_Bales = " & Str(Val(vTotBls)) & ", Total_Quantity = " & Str(Val(vTotQty)) & ", Total_Meters = " & Str(Val(vTotMtrs)) & ", Total_Amount = " & Str(Val(lbl_GrossAmount.Text)) & ", Discount_Percentage = " & Str(Val(txt_DiscPerc.Text)) & ", Discount_Amount = " & Str(Val(lbl_DiscAmount.Text)) & ", Assessable_Value = " & Str(Val(lbl_AssessableValue.Text)) & ", Tax_Type = '" & Trim(cbo_TaxType.Text) & "', Tax_Percentage = " & Str(Val(txt_TaxPerc.Text)) & ", Tax_Amount = " & Str(Val(lbl_TaxAmount.Text)) & ", VatAc_IdNo = " & Str(Val(VatAc_ID)) & ", Packing_Amount = " & Str(Val(txt_Packing.Text)) & ", AddLess_Amount = " & Str(Val(txt_AddLess.Text)) & ", RoundOff_Amount = " & Str(Val(lbl_RoundOff.Text)) & ", Selection_Type = '" & Trim(cbo_Type.Text) & "' , Net_Amount = " & Str(Val(CSng(lbl_NetAmount.Text))) & ", Prepared_By = '" & Trim(txt_PreparedBy.Text) & "', Note = '" & Trim(txt_Note.Text) & "', NetBill_Status = " & Str(Val(NtBl_STS)) & " ,Include_Tax =  " & Str(Val(IncluTax_STS)) & " ,   Bale_Nos = '" & Trim(lbl_BaleNos.Text) & "' , Through_Name = '" & Trim(cbo_Through.Text) & "', FinishedProduct_Order_Code  = '" & Trim(lbl_OrderCode.Text) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and FinishedProduct_Invoice_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Item_PackingSlip_Head set Invoice_Code = '', Invoice_Increment = Invoice_Increment - 1 Where Invoice_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update FinishedProduct_Order_Details set Invoice_Quantity = a.Invoice_Quantity - b.Quantity from FinishedProduct_Order_Details a, FinishedProduct_Invoice_Order_Details b Where b.FinishedProduct_Invoice_Code = '" & Trim(NewCode) & "' and a.FinishedProduct_Order_Code = b.FinishedProduct_Order_Code and a.FinishedProduct_IdNo = b.FinishedProduct_IdNo"
                cmd.ExecuteNonQuery()

            End If

            Partcls = "Bill : Inv.No. " & Trim(lbl_InvoiceNo.Text)
            PBlNo = Trim(lbl_InvoiceNo.Text)
            EntID = Trim(Pk_Condition) & Trim(lbl_InvoiceNo.Text)

            cmd.CommandText = "Delete from FinishedProduct_Invoice_Order_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and FinishedProduct_Invoice_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from FinishedProduct_Invoice_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and FinishedProduct_Invoice_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from FinishedProduct_Invoice_Bale_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and FinishedProduct_Invoice_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Details

                Sno = 0

                eXmSG = ""
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(4).Value) <> 0 Then

                        Sno = Sno + 1

                        eXmSG = Trim(.Rows(i).Cells(1).Value)

                        FP_ID = Common_Procedures.Processed_Item_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                        PSalNm_ID = Common_Procedures.Processed_Item_SalesNameToIdNo(con, .Rows(i).Cells(2).Value, tr)

                        Unt_ID = Common_Procedures.Unit_NameToIdNo(con, .Rows(i).Cells(5).Value, tr)

                        cmd.CommandText = "Insert into FinishedProduct_Invoice_Details ( FinishedProduct_Invoice_Code ,               Company_IdNo       ,     FinishedProduct_Invoice_No    ,                     for_OrderBy                                            , FinishedProduct_Invoice_Date  ,          Selection_Type      ,          Ledger_IdNo    ,          Sl_No     ,        FinishedProduct_IdNo,     Processed_Item_SalesIdNo,                     Quantity             ,               Meters                     ,            Unit_IdNo    ,                   Rate                   ,                     Amount                ,  FinishedProduct_Order_Code        ) " & _
                                            "   Values                                 (   '" & Trim(NewCode) & "'    , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ",       @InvoiceDate            , '" & Trim(cbo_Type.Text) & "', " & Str(Val(Led_ID)) & ", " & Str(Val(Sno)) & ", '" & Trim(FP_ID) & "'    , " & Str(Val(PSalNm_ID)) & " , " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(Unt_ID)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & " , '" & Trim(lbl_OrderCode.Text) & "' ) "
                        cmd.ExecuteNonQuery()

                        'If Trim(UCase(cbo_Type.Text)) = "ORDER" Then

                        '    If Trim(lbl_OrderCode.Text) <> "" Then
                        '        Nr = 0
                        '        cmd.CommandText = "Update FinishedProduct_Order_Details Set Invoice_Quantity = Invoice_Quantity + " & Str(Val(.Rows(i).Cells(3).Value)) & " Where FinishedProduct_Order_Code = '" & Trim(.Rows(i).Cells(8).Value) & "' and FinishedProduct_IdNo = " & Str(Val(FP_ID)) & " and Ledger_IdNo = " & Str(Val(Led_ID))
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


            eXmSG = ""
            With dgv_OrderDetails

                Sno = 0

                For i = 0 To .RowCount - 1

                    If Trim(UCase(cbo_Type.Text)) = "ORDER" And Val(.Rows(i).Cells(3).Value) <> 0 And Trim(.Rows(i).Cells(4).Value) <> "" Then

                        Sno = Sno + 1

                        eXmSG = "ItemName  :  " & Trim(.Rows(i).Cells(1).Value) & "    -    Ord.No  :  " & Trim(.Rows(i).Cells(2).Value)

                        FP_ID = Common_Procedures.Processed_Item_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                        cmd.CommandText = "Insert into FinishedProduct_Invoice_Order_Details ( FinishedProduct_Invoice_Code ,               Company_IdNo       ,     FinishedProduct_Invoice_No    ,                               for_OrderBy                                  , FinishedProduct_Invoice_Date  ,          Ledger_IdNo    ,          Sl_No       ,    FinishedProduct_IdNo  ,             FinishedProduct_Order_No     ,                     Quantity              ,     FinishedProduct_Order_Code          ) " & _
                                            "          Values                                (   '" & Trim(NewCode) & "'    , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ",       @InvoiceDate            , " & Str(Val(Led_ID)) & ", " & Str(Val(Sno)) & ", '" & Trim(FP_ID) & "'    ,  '" & Trim(.Rows(i).Cells(2).Value) & "' , " & Str(Val(.Rows(i).Cells(3).Value)) & " , '" & Trim(.Rows(i).Cells(4).Value) & "' ) "
                        cmd.ExecuteNonQuery()

                        Nr = 0
                        cmd.CommandText = "Update FinishedProduct_Order_Details Set Invoice_Quantity = Invoice_Quantity + " & Str(Val(.Rows(i).Cells(3).Value)) & " Where FinishedProduct_Order_Code = '" & Trim(.Rows(i).Cells(4).Value) & "' and FinishedProduct_IdNo = " & Str(Val(FP_ID)) & " and Ledger_IdNo = " & Str(Val(Led_ID))
                        Nr = cmd.ExecuteNonQuery()

                        If Nr = 0 Then
                            Throw New ApplicationException("Mismatch of Order Indent Details " & Chr(13) & "Ord.No : " & .Rows(i).Cells(2).Value & "      -      Item Name : " & .Rows(i).Cells(1).Value)
                            Exit Sub
                        End If

                    End If

                Next

            End With

            Sno = 0
            With dgv_BaleDetails

                For i = 0 To .RowCount - 1
                    Sno = Sno + 1

                    If (Val(.Rows(i).Cells(2).Value) <> 0 Or Val(.Rows(i).Cells(3).Value) <> 0) And Trim(.Rows(i).Cells(4).Value) <> "" Then

                        cmd.CommandText = "Insert into FinishedProduct_Invoice_Bale_Details ( FinishedProduct_Invoice_Code ,               Company_IdNo       ,     FinishedProduct_Invoice_No    ,                     for_OrderBy                                            , FinishedProduct_Invoice_Date  ,           Sl_No     ,              Item_PackingSlip_No        ,                  Quantity                ,                     Meters               ,               Item_PackingSlip_Code       ) " & _
                                            "   Values                                      (   '" & Trim(NewCode) & "'    , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvoiceNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvoiceNo.Text))) & ",       @InvoiceDate            , " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', " & Str(Val(.Rows(i).Cells(2).Value)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", '" & Trim(.Rows(i).Cells(4).Value) & "'   ) "
                        cmd.ExecuteNonQuery()

                        Nr = 0
                        cmd.CommandText = "Update Item_PackingSlip_Head set Invoice_Code = '" & Trim(NewCode) & "', Invoice_Increment = Invoice_Increment + 1 Where Item_PackingSlip_Code = '" & Trim(.Rows(i).Cells(4).Value) & "' and Ledger_IdNo = " & Str(Val(Led_ID))
                        Nr = cmd.ExecuteNonQuery()

                        If Nr = 0 Then
                            MessageBox.Show("Invalid Bale Details - Mismatch of details", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            tr.Rollback()
                            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
                            Exit Sub
                        End If

                    End If

                Next

            End With

            Dim vBill_No As String = ""
            If Val(NtBl_STS) = 1 Then
                vBill_No = Trim(lbl_InvoiceNo.Text) & "/ NetRate"
            Else
                vBill_No = Trim(lbl_InvoiceNo.Text)
            End If

            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
            vLed_IdNos = Led_ID & "|" & SalAc_ID & "|" & VatAc_ID
            vVou_Amts = -1 * Val(CSng(lbl_NetAmount.Text)) & "|" & (Val(CSng(lbl_NetAmount.Text)) - Val(CSng(lbl_TaxAmount.Text))) & "|" & Val(CSng(lbl_TaxAmount.Text))

            If Common_Procedures.Voucher_Updation(con, "FP.Invoice", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_InvoiceNo.Text), dtp_Date.Value.Date, "Inv No : " & Trim(vBill_No), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

            Dim VouBil As String = ""
            VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), dtp_Date.Value.Date, Led_ID, Trim(vBill_No), Ag_ID, Val(CSng(lbl_NetAmount.Text)), "DR", Trim(Pk_Condition) & Trim(NewCode), tr)
            If Trim(UCase(VouBil)) = "ERROR" Then
                Throw New ApplicationException("Error on Voucher Bill Posting")
                Exit Sub
            End If

            If Trim(UCase(cbo_Type.Text)) = "ORDER" Then

                cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
                cmd.ExecuteNonQuery()

                cmd.CommandText = "insert into " & Trim(Common_Procedures.EntryTempTable) & "(int1, weight1) select FinishedProduct_IdNo, Quantity from FinishedProduct_Invoice_Order_Details Where FinishedProduct_Invoice_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "insert into " & Trim(Common_Procedures.EntryTempTable) & "(int1, weight1) select FinishedProduct_IdNo, -1*Quantity from FinishedProduct_Invoice_Details Where FinishedProduct_Invoice_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                Da = New SqlClient.SqlDataAdapter("select int1 as Fp_IdNo, sum(weight1) from " & Trim(Common_Procedures.EntryTempTable) & " group by int1 having sum(weight1) <> 0", con)
                Da.SelectCommand.Transaction = tr
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    fpitmnm = Common_Procedures.Processed_Item_IdNoToName(con, Dt1.Rows(0).Item("Fp_IdNo").ToString, tr)
                    Throw New ApplicationException("Mismatch of Quantity in Invoice and Order Details" & Chr(13) & "ItemName  :  " & Trim(fpitmnm))
                    Exit Sub
                End If
                Dt1.Clear()

            End If

            tr.Commit()

            move_record(lbl_InvoiceNo.Text)

            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception

            tr.Rollback()

            If InStr(1, Trim(LCase(ex.Message)), Trim(LCase("ck_finishedproduct_order_details_1"))) > 0 Then
                MessageBox.Show("Invalid Quantity - Invocie Quantity greater than Order Quantity - " & (eXmSG), "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf InStr(1, Trim(LCase(ex.Message)), Trim(LCase("ck_finishedproduct_order_details_2"))) > 0 Then
                MessageBox.Show("Invalid Invoice Quantity in Order Details - " & (eXmSG), "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If

        Finally

            Dt1.Dispose()
            Da.Dispose()
            cmd.Dispose()
            tr.Dispose()

            If msk_Date.Enabled = True And msk_Date.Visible = True Then msk_Date.Focus()

        End Try

    End Sub

    Private Sub cbo_Ledger_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Ledger.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) and Verified_Status = 1)", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Ledger_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Ledger, cbo_Area, txt_DcNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) and Verified_Status = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Ledger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Ledger.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim AgNm As String
        Dim Led_Idno As Integer = 0
        Dim Area_Idno As Integer = 0
        Dim trpt_Idno As Integer = 0

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Ledger, txt_DcNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) and Verified_Status = 1)", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(cbo_Type.Text)) = "ORDER" Then

                If MessageBox.Show("Do you want to select Order", "FOR ORDER SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    btn_OrderSelection_Click(sender, e)
                Else
                    txt_DcNo.Focus()
                End If

            Else

                Led_Idno = Common_Procedures.Ledger_AlaisNameToIdNo(con, Trim(cbo_Ledger.Text))

                da = New SqlClient.SqlDataAdapter("select a.* from ledger_head a where a.ledger_idno = " & Str(Val(Led_Idno)) & "  ", con)
                dt = New DataTable
                da.Fill(dt)

                AgNm = ""
                Area_Idno = 0
                trpt_Idno = 0

                If dt.Rows.Count > 0 Then
                    If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                        AgNm = Common_Procedures.Ledger_IdNoToName(con, Val(dt.Rows(0)("Ledger_AgentIdNo").ToString))
                        Area_Idno = Val(dt.Rows(0).Item("Area_IdNo").ToString)
                        trpt_Idno = Val(dt.Rows(0).Item("Transport_IdNo").ToString)
                    End If
                End If

                dt.Dispose()
                da.Dispose()

                If Trim(AgNm) <> "" Then cbo_Agent.Text = AgNm
                If Trim(Area_Idno) <> 0 Then cbo_Area.Text = Common_Procedures.Area_IdNoToName(con, Val(Area_Idno))
                If Val(trpt_Idno) <> 0 Then cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, Val(trpt_Idno))

                txt_DcNo.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, dtp_LrDate, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            If MessageBox.Show("Do you want to select Packing Sip?", "FOR BALE SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)

            Else
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(6)
                    dgv_Details.CurrentCell.Selected = True

                Else
                    txt_DiscPerc.Focus()

                End If

            End If

        End If

    End Sub

    Private Sub cbo_Through_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Through.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub

    Private Sub cbo_Through_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Through.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Through, cbo_Agent, cbo_SalesAc, "", "", "", "")
    End Sub

    Private Sub cbo_Through_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Through.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Through, cbo_SalesAc, "", "", "", "")
    End Sub

    Private Sub cbo_SalesAc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SalesAc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28 and Verified_Status = 1)", "(Ledger_IdNo = 0)")

    End Sub
    Private Sub cbo_SalesAc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SalesAc.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_SalesAc, cbo_Through, txt_LrNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28 and Verified_Status = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_SalesAc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_SalesAc.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_SalesAc, txt_LrNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28 and Verified_Status = 1)", "(Ledger_IdNo = 0)")
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
                Condt = "a.FinishedProduct_Invoice_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.FinishedProduct_Invoice_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.FinishedProduct_Invoice_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Ledger_IdNo = " & Str(Val(Led_IdNo))
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name from FinishedProduct_Invoice_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.FinishedProduct_Invoice_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " and Entry_VAT_GST_Type <> 'GST'  Order by a.for_orderby, a.FinishedProduct_Invoice_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("FinishedProduct_Invoice_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("FinishedProduct_Invoice_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Total_Bales").ToString)
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Total_Quantity").ToString)
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Net_Amount").ToString), "########0.00")

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
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) and Verified_Status = 1)", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) and Verified_Status = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) and Verified_Status = 1)", "(Ledger_IdNo = 0)")

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
                    If e.ColumnIndex = 6 Then
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
                        If e.ColumnIndex = 3 Or e.ColumnIndex = 4 Or e.ColumnIndex = 6 Then

                            If InStr(1, Trim(UCase(.CurrentRow.Cells(5).Value)), "MTR") > 0 Or InStr(1, Trim(UCase(.CurrentRow.Cells(5).Value)), "METER") > 0 Or InStr(1, Trim(UCase(.CurrentRow.Cells(5).Value)), "METRE") > 0 Then
                                q = Val(.CurrentRow.Cells(4).Value)
                            Else
                                q = Val(.CurrentRow.Cells(3).Value)
                            End If
                            .CurrentRow.Cells(7).Value = Format(Val(q) * Val(.CurrentRow.Cells(6).Value), "#########0.00")
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
        dgv_ActCtrlName = dgv_ActCtrlName = ""
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress

        With dgv_Details
            If .Visible Then
                If .Rows.Count > 0 Then
                    If .CurrentCell.ColumnIndex = 6 Then
                        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                            e.Handled = True
                        End If
                    End If
                End If
            End If
        End With

    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Ledger.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New LedgerCreation_Processing

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
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT BALE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Ledger.Enabled And cbo_Ledger.Visible Then cbo_Ledger.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        With dgv_Selection

            .Rows.Clear()

            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.* from Item_PackingSlip_Head a Where a.Invoice_Code = '" & Trim(NewCode) & "' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.Item_PackingSlip_Date, a.for_orderby, a.Item_PackingSlip_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Item_PackingSlip_No").ToString
                    .Rows(n).Cells(2).Value = Val(Dt1.Rows(i).Item("Total_Quantity").ToString)
                    .Rows(n).Cells(3).Value = Format(Val(Dt1.Rows(i).Item("Total_Meters").ToString), "#########0.00")
                    .Rows(n).Cells(4).Value = "1"
                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Item_PackingSlip_Code").ToString

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()

            Da = New SqlClient.SqlDataAdapter("select a.* from Item_PackingSlip_Head a Where a.Invoice_Code = '' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.Item_PackingSlip_Date, a.for_orderby, a.Item_PackingSlip_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Item_PackingSlip_No").ToString
                    .Rows(n).Cells(2).Value = Val(Dt1.Rows(i).Item("Total_Quantity").ToString)
                    .Rows(n).Cells(3).Value = Format(Val(Dt1.Rows(i).Item("Total_Meters").ToString), "#########0.00")
                    .Rows(n).Cells(4).Value = ""
                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Item_PackingSlip_Code").ToString

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

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(4).Value) = 1 Then

                n = dgv_BaleDetails.Rows.Add()

                sno = sno + 1
                dgv_BaleDetails.Rows(n).Cells(0).Value = Val(sno)
                dgv_BaleDetails.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(1).Value
                dgv_BaleDetails.Rows(n).Cells(2).Value = Val(dgv_Selection.Rows(i).Cells(2).Value)
                dgv_BaleDetails.Rows(n).Cells(3).Value = Format(Val(dgv_Selection.Rows(i).Cells(3).Value), "#########0.00")
                dgv_BaleDetails.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(5).Value

                Cmd.CommandText = "insert into " & Trim(Common_Procedures.EntryTempTable) & " (Int1, Int2, Weight1, Meters1) Select Company_Idno, Item_IdNo, Quantity, Meters from Item_PackingSlip_Details where Item_PackingSlip_Code = '" & Trim(dgv_Selection.Rows(i).Cells(5).Value) & "'"
                Cmd.ExecuteNonQuery()

                Cmd.CommandText = "insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Name1 ) values ('" & Trim(dgv_Selection.Rows(i).Cells(5).Value) & "')"
                Cmd.ExecuteNonQuery()

            End If

        Next i

        Da = New SqlClient.SqlDataAdapter("select a.Int1 as Company_IdNo, a.Int2 as Item_IdNo, b.Processed_Item_Name, c.Unit_Name, e.Processed_Item_SalesName, b.Sales_Rate, sum(a.Weight1) as qty, sum(a.Meters1) as meters from " & Trim(Common_Procedures.EntryTempTable) & " a INNER JOIN Processed_Item_Head b ON a.Int2 = b.Processed_Item_IdNo LEFT OUTER JOIN Unit_Head c ON b.Unit_IdNo = c.Unit_IdNo LEFT OUTER JOIN Processed_Item_SalesName_Details d ON a.Int1 = d.Company_IdNo and a.Int2 = d.Processed_Item_IdNo LEFT OUTER JOIN Processed_Item_SalesName_Head e ON d.Processed_Item_SalesIdNo = e.Processed_Item_SalesIdNo group by a.int1, a.Int2, b.Processed_Item_Name, c.Unit_Name, e.Processed_Item_SalesName, b.Sales_Rate Order by b.Processed_Item_Name, a.int1, a.Int2, c.Unit_Name, e.Processed_Item_SalesName, b.Sales_Rate", con)
        'Da = New SqlClient.SqlDataAdapter("select a.Int1 as Company_IdNo, a.Int2 as Item_IdNo, b.Processed_Item_Name, c.Unit_Name, e.Processed_Item_SalesName, b.Sales_Rate, sum(a.Weight1) as qty, sum(a.Meters1) as meters from " & Trim(Common_Procedures.EntryTempTable) & " a INNER JOIN Processed_Item_Head b ON a.Int2 = b.Processed_Item_IdNo LEFT OUTER JOIN Unit_Head c ON b.Unit_IdNo = c.Unit_IdNo LEFT OUTER JOIN Processed_Item_SalesName_Details d ON a.Int1 = d.Company_IdNo and a.Int2 = d.Processed_Item_IdNo LEFT OUTER JOIN Processed_Item_SalesName_Head e ON d.Processed_Item_SalesIdNo = e.Processed_Item_SalesIdNo group by a.int1, a.Int2, b.Processed_Item_Name, c.Unit_Name, e.Processed_Item_SalesName, b.Sales_Rate Order by a.int1, a.Int2, b.Processed_Item_Name, c.Unit_Name, e.Processed_Item_SalesName, b.Sales_Rate", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        sno = 0

        If Dt1.Rows.Count > 0 Then

            For i = 0 To Dt1.Rows.Count - 1

                Rt = 0

                Da = New SqlClient.SqlDataAdapter("Select a.* from FinishedProduct_Invoice_Details a Where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.FinishedProduct_Invoice_Code = '" & Trim(NewCode) & "' and a.FinishedProduct_IdNo = " & Str(Val(Dt1.Rows(i).Item("Item_IdNo").ToString)) & " Order by a.sl_no", con)
                Dt2 = New DataTable
                Da.Fill(Dt2)

                If Dt2.Rows.Count > 0 Then
                    If IsDBNull(Dt2.Rows(0).Item("Rate").ToString) = False Then
                        Rt = Val(Dt2.Rows(0).Item("Rate").ToString)
                    End If
                End If
                Dt2.Clear()

                n = dgv_Details.Rows.Add()

                sno = sno + 1
                dgv_Details.Rows(n).Cells(0).Value = Val(sno)
                dgv_Details.Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Processed_Item_Name").ToString
                dgv_Details.Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Processed_Item_SalesName").ToString
                dgv_Details.Rows(n).Cells(3).Value = Val(Dt1.Rows(i).Item("qty").ToString)
                dgv_Details.Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("meters").ToString), "#########0.00")
                dgv_Details.Rows(n).Cells(5).Value = Dt1.Rows(i).Item("Unit_Name").ToString

                If Rt = 0 Then
                    Rt = Val(Dt1.Rows(i).Item("Sales_Rate").ToString)
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

        Da = New SqlClient.SqlDataAdapter("Select b.Item_PackingSlip_No, b.For_OrderBy from " & Trim(Common_Procedures.ReportTempTable) & " a, Item_PackingSlip_Head b where a.Name1 = b.Item_PackingSlip_Code order by b.For_OrderBy, b.Item_PackingSlip_No", con)
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

        lbl_BaleNos.Text = Trim(vBl_No)
        txt_DcNo.Text = Trim(vBl_No)

        NoCalc_Status = False
        Total_Calculation()

        Grid_Cell_DeSelect()

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False

        If dgv_Details.Rows.Count > 0 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(6)
            dgv_Details.CurrentCell.Selected = True

        Else
            txt_DiscPerc.Focus()

        End If

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
            If dgv_OrderDetails.Rows.Count > 0 Then
                dgv_OrderDetails.Focus()
                dgv_OrderDetails.CurrentCell = dgv_OrderDetails.Rows(0).Cells(3)
                dgv_OrderDetails.CurrentCell.Selected = True

            ElseIf dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(6)
                dgv_Details.CurrentCell.Selected = True

            Else
                cbo_Transport.Focus()

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
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Note_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Note.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
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
        Dim TotMtrs As Single, TotAmt As Single

        If NoCalc_Status = True Or FrmLdSTS = True Then Exit Sub

        Sno = 0
        TotQty = 0 : TotMtrs = 0 : TotAmt = 0

        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Trim(.Rows(i).Cells(1).Value) <> "" And (Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(4).Value) <> 0) Then

                    TotQty = TotQty + Val(.Rows(i).Cells(3).Value)
                    TotMtrs = TotMtrs + Val(.Rows(i).Cells(4).Value)
                    TotAmt = TotAmt + Val(.Rows(i).Cells(7).Value)

                End If

            Next

        End With

        lbl_GrossAmount.Text = Format(Val(TotAmt), "########0.00")

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(3).Value = Val(TotQty)
            .Rows(0).Cells(4).Value = Format(Val(TotMtrs), "########0.00")
            .Rows(0).Cells(7).Value = Format(Val(TotAmt), "########0.00")
        End With

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

        NetAmount_Calculation()

    End Sub

    Private Sub NetAmount_Calculation()
        Dim NtAmt As Single

        If NoCalc_Status = True Then Exit Sub

        lbl_DiscAmount.Text = Format(Val(lbl_GrossAmount.Text) * Val(txt_DiscPerc.Text) / 100, "########0.00")

        lbl_AssessableValue.Text = Format(Val(lbl_GrossAmount.Text) - Val(lbl_DiscAmount.Text), "########0.00")

        lbl_TaxAmount.Text = Format(Val(lbl_AssessableValue.Text) * Val(txt_TaxPerc.Text) / 100, "########0.00")

        NtAmt = Val(lbl_AssessableValue.Text) + Val(lbl_TaxAmount.Text) + Val(txt_Packing.Text) + Val(txt_AddLess.Text)

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
        pnl_Print.Visible = True
        pnl_Back.Enabled = False
        If btn_Print_Preprint_J.Enabled And btn_Print_Preprint_J.Visible Then
            btn_Print_Preprint_J.Focus()
        End If
    End Sub

    Public Sub printing_invoice()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim ps As Printing.PaperSize

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from FinishedProduct_Invoice_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and FinishedProduct_Invoice_Code = '" & Trim(NewCode) & "'", con)
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
        If prn_Status = 3 Then
            prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "                  1. Original" & Chr(13) & "                  2. Duplicate" & Chr(13) & "                  3. Triplicate" & Chr(13) & "                  4. Extra Copy" & Space(10) & "                  5. All", "FOR INVOICE PRINTING...", "1")
            prn_InpOpts = Replace(Trim(prn_InpOpts), "5", "1234")
        End If

        If prn_Status = 1 Then
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    Exit For
                End If
            Next

        ElseIf prn_Status = 2 Then

            Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 10X12", 1000, 1200)
            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

        Else
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    Exit For
                End If
            Next
        End If

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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_Count = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Ledger_Name as TransportName, e.Ledger_Name as Agent_Name , f.Ledger_Name as SalesAcc_Name , g.* from FinishedProduct_Invoice_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON d.Ledger_IdNo = a.Transport_IdNo LEFT OUTER JOIN Ledger_Head e ON e.Ledger_IdNo =a.Agent_IdNo LEFT OUTER JOIN Ledger_Head f ON f.Ledger_IdNo =a.SalesAc_IdNo LEFT OUTER JOIN Area_Head g ON g.Area_IdNo =a.Area_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.FinishedProduct_Invoice_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Processed_Item_Name , b.Meter_Qty , c.Processed_Item_SalesName, d.Unit_Name from FinishedProduct_Invoice_Details a INNER JOIN Processed_Item_Head b ON  a.FinishedProduct_IdNo = b.Processed_Item_IdNo LEFT OUTER JOIN Processed_Item_SalesName_Head c ON a.Processed_Item_SalesIdNo = c.Processed_Item_SalesIdNo Left Outer join Unit_Head d ON a.Unit_IdNo = d.Unit_IdNo Where a.FinishedProduct_Invoice_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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
        If prn_Status = 1 Then
            Printing_Format1(e)
        ElseIf prn_Status = 2 Then
            Printing_Format2(e)
        ElseIf prn_Status = 3 Then
            Printing_Format3(e)
        End If
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
        Dim ItmNm1 As String, ItmNm2 As String
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer = 0
        Dim flperc As Single = 0
        Dim flmtr As Single = 0
        Dim fmtr As Single = 0

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

        NoofItems_PerPage = 10 ' 8

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(50) : ClAr(2) = 230 : ClAr(3) = 80 : ClAr(4) = 90 : ClAr(5) = 80 : ClAr(6) = 80
        ClAr(7) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6))

        TxtHgt = 18.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        TxtHgt = 19  ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        ''=========================================================================================================
        ''------  START OF PREPRINT POINTS
        ''=========================================================================================================

        'pFont = New Font("Calibri", 11, FontStyle.Regular)

        'Dim CurX As Single = 0
        'Dim pFont1 As Font

        'pFont1 = New Font("Calibri", 8, FontStyle.Regular)

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

        ''=========================================================================================================
        ''------  END OF PREPRINT POINTS
        ''=========================================================================================================

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


                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Processed_Item_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 35 Then
                            For I = 15 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 35
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Quantity").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Quantity").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Meter_Qty").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)



                        NoofDets = NoofDets + 1

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
        Dim p1Font As Font
        Dim strHeight As Single
        Dim C1 As Single, W1, C2, W2 As Single, S1, S2 As Single
        Dim Cmp_Name, Desc As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String

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
        Desc = ""
        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

        Desc = prn_HdDt.Rows(0).Item("Company_Description").ToString
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If

        p1Font = New Font("Calibri", 15, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + TxtHgt + 5
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Desc, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
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
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("FinishedProduct_Invoice_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "INVOICE DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("FinishedProduct_Invoice_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Dc_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "DC NO : " & prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt

            If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
                Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
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

            Common_Procedures.Print_To_PrintDocument(e, "Lr.No  ", LMargin + C2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + C2 + W2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lr_No").ToString, LMargin + C2 + W2 + 30, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Lr_No").ToString) <> "" And Trim(prn_HdDt.Rows(0).Item("Lr_Date").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Lr_Date").ToString, LMargin + C2 + 220, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Order No ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)

            If Trim(prn_HdDt.Rows(0).Item("Order_Date").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Order Date ", LMargin + C2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + C2 + W2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_Date").ToString, LMargin + C2 + W2 + 30, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Transport ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            ' Common_Procedures.Print_To_PrintDocument(e, "Doc.Through ", LMargin + C2 + 10, CurY, 0, 0, pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + C2 + W2 + 10, CurY, 0, 0, pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Name").ToString, LMargin + C2 + W2 + +30, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Doc.Through ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Through_Name").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

            CurY = CurY + 10
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DESCRIPTION", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "QTY", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "MYRS/PC", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)

            CurY = CurY + TxtHgt + 10
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

        Try

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt

                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            'LnAr(6) = CurY

            CurY = CurY + 10
            Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Quantity").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))



            CurY = CurY + 10  ' TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Discount Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt


            If Val(prn_HdDt.Rows(0).Item("Tax_Percentage").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Tax_Type").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, "Tax Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Tax_Percentage").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Tax_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt

            If Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Packing Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Packing_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY, 0, 0, p1Font)
            If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add/Less Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY, 0, 0, p1Font)
            If Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "RoundOff Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY, 0, 0, p1Font)
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)
            LnAr(8) = CurY

            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            CurY = CurY + 10

            NetBilTxt = ""
            If IsDBNull(prn_HdDt.Rows(0).Item("NetBill_Status").ToString) = False Then
                If Val(prn_HdDt.Rows(0).Item("NetBill_Status").ToString) = 1 Then NetBilTxt = "NET BILL"
            End If

            Common_Procedures.Print_To_PrintDocument(e, NetBilTxt, LMargin + ClAr(1) + 20, CurY, 0, 0, p1Font)

            'Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Quantity").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Net_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))

            CurY = CurY + 10

            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
            BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "", "")

            Common_Procedures.Print_To_PrintDocument(e, "Rupees  : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 5
            p1Font = New Font("Calibri", 12, FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 5
            Common_Procedures.Print_To_PrintDocument(e, "1.Payment Should Be Made Within 30 Days", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "2.PAYMENT SHOULD BE PAID BY CHEQUE OR DRAFT PAYABLE AT COIMBATORE", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "3.Subject to Coimbatore jurisdiction Only ", LMargin + 10, CurY, 0, 0, pFont)


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
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("FinishedProduct_Invoice_No").ToString, CurX, CurY, 0, 0, p1Font)
                CurX = LMargin + 770
                Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("FinishedProduct_Invoice_Date").ToString), "dd-MM-yyyy"), CurX, CurY, 0, 0, pFont)

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
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Bales").ToString, CurX, CurY, 0, 0, pFont)

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

                                ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Processed_Item_Name").ToString)
                                ItmNm2 = ""
                                If Len(ItmNm1) > 35 Then
                                    For I = 20 To 1 Step -1
                                        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 35
                                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                                End If

                                ItmDesc1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Processed_Item_SalesName").ToString)
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
                If IsDBNull(prn_HdDt.Rows(0).Item("NetBill_Status").ToString) = False Then
                    If Val(prn_HdDt.Rows(0).Item("NetBill_Status").ToString) = 1 Then NetBilTxt = "NET BILL"
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
        Dim ps As Printing.PaperSize
        Dim Rup1 As String, Rup2 As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim NetBilTxt As String = ""
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

            ''For I = 100 To 1100 Step 300

            ''    CurY = I
            ''    For J = 1 To 850 Step 40

            ''        CurX = J
            ''        Common_Procedures.Print_To_PrintDocument(e, CurX, CurX, CurY - 20, 0, 0, pFont1)
            ''        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY, 0, 0, pFont)

            ''        CurX = J + 20
            ''        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY, 0, 0, pFont)
            ''        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY + 20, 0, 0, pFont)
            ''        Common_Procedures.Print_To_PrintDocument(e, CurX, CurX, CurY + 40, 0, 0, pFont1)

            ''    Next

            ''Next

            ''For I = 200 To 800 Step 250

            ''    CurX = I
            ''    For J = 1 To 1200 Step 40

            ''        CurY = J
            ''        Common_Procedures.Print_To_PrintDocument(e, "-", CurX, CurY, 0, 0, pFont)
            ''        Common_Procedures.Print_To_PrintDocument(e, "   " & CurY, CurX, CurY, 0, 0, pFont1)

            ''        CurY = J + 20
            ''        Common_Procedures.Print_To_PrintDocument(e, "--", CurX, CurY, 0, 0, pFont)
            ''        Common_Procedures.Print_To_PrintDocument(e, "   " & CurY, CurX, CurY, 0, 0, pFont1)

            ''    Next

            ''Next

            ''e.HasMorePages = False
            ''Exit Sub
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
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("FinishedProduct_Invoice_No").ToString, CurX, CurY, 0, 0, p1Font)
                CurX = LMargin + 670
                Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("FinishedProduct_Invoice_Date").ToString), "dd-MM-yyyy"), CurX, CurY, 0, 0, pFont)

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
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Bales").ToString, CurX, CurY, 0, 0, pFont)

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

                                ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Processed_Item_Name").ToString)
                                ItmNm2 = ""
                                If Len(ItmNm1) > 35 Then
                                    For I = 20 To 1 Step -1
                                        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 35
                                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                                End If

                                ItmDesc1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Processed_Item_SalesName").ToString)
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
                If IsDBNull(prn_HdDt.Rows(0).Item("NetBill_Status").ToString) = False Then
                    If Val(prn_HdDt.Rows(0).Item("NetBill_Status").ToString) = 1 Then NetBilTxt = "NET BILL"
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
    Private Sub cbo_Area_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Area.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Area_Head", "Area_Name", "", "(Area_IdNo = 0)")

    End Sub

    Private Sub cbo_Area_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Area.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Area, cbo_Type, cbo_Ledger, "Area_Head", "Area_Name", "", "(Area_IdNo = 0)")
    End Sub

    Private Sub cbo_Area_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Area.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Area, cbo_Ledger, "Area_Head", "Area_Name", "", "(Area_IdNo = 0)")
    End Sub

    Private Sub cbo_Agent_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Agent.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Agent_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Agent, dtp_OrderDate, cbo_Through, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Agent_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Agent.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Agent, cbo_Through, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_VatAc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VatAc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 12)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_VatAc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VatAc.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VatAc, txt_AddLess, txt_PreparedBy, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 12)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_VatAc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VatAc.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VatAc, txt_PreparedBy, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 12)", "(Ledger_IdNo = 0)")
    End Sub


    Private Sub cbo_Area_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Area.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Area_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Area.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Agent_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Agent_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Agent.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
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
                    txt_DiscPerc.Focus()
                End If
            End If

        End With

    End Sub

    Private Sub dgv_BaleDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_BaleDetails.LostFocus
        On Error Resume Next
        dgv_BaleDetails.CurrentCell.Selected = False
    End Sub

    Private Sub cbo_TaxType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub

    Private Sub cbo_TaxType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TaxType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TaxType, txt_Packing, txt_TaxPerc, "", "", "", "")
    End Sub

    Private Sub cbo_TaxType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TaxType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TaxType, txt_TaxPerc, "", "", "", "")
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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Type, msk_Date, cbo_Area, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Type, cbo_Area, "", "", "", "")
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

        If Trim(UCase(cbo_Type.Text)) <> "ORDER" Then
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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        With dgv_OrderSelection

            .Rows.Clear()

            SNo = 0

            '---1
            Da = New SqlClient.SqlDataAdapter("Select a.*, e.Ledger_Name as Transportname, f.Ledger_Name as Agentname, I.area_Name, " & _
                                                " (select sum(z2.Quantity - z2.Invoice_Quantity) as Balance_Qty from FinishedProduct_Order_Details z2 where z2.FinishedProduct_Order_Code = a.FinishedProduct_Order_Code ) as Balance_Qty, " & _
                                                " (select sum(z3.Quantity) from FinishedProduct_Invoice_Order_Details z3 where z3.FinishedProduct_Invoice_Code = '" & Trim(NewCode) & "' and z3.FinishedProduct_Order_Code = a.FinishedProduct_Order_Code ) as Ent_Qty " & _
                                                " from FinishedProduct_Order_Head a " & _
                                                " LEFT OUTER JOIN Ledger_Head e ON e.Ledger_IdNo <> 0 and a.Transport_IdNo = e.Ledger_IdNo " & _
                                                " LEFT OUTER JOIN Ledger_Head f ON f.Ledger_IdNo <> 0 and a.Agent_IdNo = f.Ledger_IdNo " & _
                                                " LEFT OUTER JOIN Area_Head I ON I.Area_Idno <> 0 and I.Area_Idno = a.Area_Idno " & _
                                                " Where " & _
                                                " a.ledger_Idno = " & Str(Val(LedIdNo)) & " and a.FinishedProduct_Order_Code IN (select z1.FinishedProduct_Order_Code from FinishedProduct_Invoice_Order_Details z1 Where z1.FinishedProduct_Invoice_Code = '" & Trim(NewCode) & "' ) " & _
                                                " order by a.FinishedProduct_Order_Date, a.for_orderby, a.FinishedProduct_Order_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            Ent_OrdCd = "'0'"

            If Dt1.Rows.Count > 0 Then

                For I = 0 To Dt1.Rows.Count - 1

                    'BalQty = 0
                    'Da = New SqlClient.SqlDataAdapter("select sum(Quantity - Invoice_Quantity) as Balance_Qty from FinishedProduct_Invoice_Details z1 where FinishedProduct_Invoice_Code = '" & Trim(Dt1.Rows(i).Item("FinishedProduct_Invoice_Code").ToString) & "'  ", con)
                    'Dt2 = New DataTable
                    'nr = Da.Fill(Dt2)
                    'If Dt2.Rows.Count > 0 Then
                    '    BalQty = Val(Dt1.Rows(i).Item("Balance_Qty").ToString)
                    'End If
                    'dt2.clear()

                    n = .Rows.Add()

                    Ent_OrdCd = Trim(Ent_OrdCd) & IIf(Trim(Ent_OrdCd) <> "", ", ", "") & "'" & Dt1.Rows(I).Item("FinishedProduct_Order_Code").ToString & "'"

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(I).Item("FinishedProduct_Order_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(I).Item("FinishedProduct_Order_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(I).Item("Order_No").ToString
                    .Rows(n).Cells(4).Value = Val(Dt1.Rows(I).Item("Total_Quantity").ToString)
                    .Rows(n).Cells(5).Value = Val(Dt1.Rows(I).Item("Balance_Qty").ToString) + Val(Dt1.Rows(I).Item("Ent_Qty").ToString)
                    .Rows(n).Cells(6).Value = "1"
                    .Rows(n).Cells(7).Value = Dt1.Rows(I).Item("Agentname").ToString
                    .Rows(n).Cells(8).Value = Dt1.Rows(I).Item("Through_Name").ToString
                    .Rows(n).Cells(9).Value = Dt1.Rows(I).Item("Area_Name").ToString
                    .Rows(n).Cells(10).Value = Dt1.Rows(I).Item("Transportname").ToString
                    .Rows(n).Cells(11).Value = Dt1.Rows(I).Item("FinishedProduct_Order_Code").ToString

                    For J = 0 To .ColumnCount - 1
                        .Rows(I).Cells(J).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()

            Ent_OrdCd = "(" & Trim(Ent_OrdCd) & ")"

            '---2
            Da = New SqlClient.SqlDataAdapter("Select a.*, e.Ledger_Name as Transportname, f.Ledger_Name as Agentname, I.area_Name, " & _
                                                " (select sum(z2.Quantity - z2.Invoice_Quantity) as Balance_Qty from FinishedProduct_Order_Details z2 where z2.FinishedProduct_Order_Code = a.FinishedProduct_Order_Code ) as Balance_Qty " & _
                                                " from FinishedProduct_Order_Head a " & _
                                                " LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo " & _
                                                " LEFT OUTER JOIN Ledger_Head f ON a.Agent_IdNo = f.Ledger_IdNo " & _
                                                " LEFT OUTER JOIN Area_Head I ON I.Area_Idno = A.Area_Idno " & _
                                                " Where a.ledger_Idno = " & Str(Val(LedIdNo)) & " and a.FinishedProduct_Order_Code IN (select z1.FinishedProduct_Order_Code from FinishedProduct_Order_Details z1 where z1.FinishedProduct_Order_Code NOT IN " & Trim(Ent_OrdCd) & " and (z1.Quantity - z1.Invoice_Quantity) > 0 ) " & _
                                                " Order by a.FinishedProduct_Order_Date, a.for_orderby, a.FinishedProduct_Order_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For I = 0 To Dt1.Rows.Count - 1

                    'BalQty = 0
                    'Da = New SqlClient.SqlDataAdapter("select sum(Invoice_Quantity) as Balance_Qty from FinishedProduct_Invoice_Details z1 where FinishedProduct_Invoice_Code = '" & Trim(Dt1.Rows(i).Item("FinishedProduct_Invoice_Code").ToString) & "'  ", con)
                    'Dt2 = New DataTable
                    'nr = Da.Fill(Dt2)
                    'If Dt2.Rows.Count > 0 Then
                    '    BalQty = Val(Dt1.Rows(i).Item("Balance_Qty").ToString)
                    'End If
                    'dt2.clear()

                    n = .Rows.Add()

                    Ent_OrdCd = Trim(Ent_OrdCd) & IIf(Trim(Ent_OrdCd) <> "", ", ", "") & "'" & Dt1.Rows(I).Item("FinishedProduct_Order_Code").ToString & "'"

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(I).Item("FinishedProduct_Order_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(I).Item("FinishedProduct_Order_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(I).Item("Order_No").ToString
                    .Rows(n).Cells(4).Value = Val(Dt1.Rows(I).Item("Total_Quantity").ToString)
                    .Rows(n).Cells(5).Value = Val(Dt1.Rows(I).Item("Balance_Qty").ToString)
                    .Rows(n).Cells(6).Value = ""
                    .Rows(n).Cells(7).Value = Dt1.Rows(I).Item("Agentname").ToString
                    .Rows(n).Cells(8).Value = Dt1.Rows(I).Item("Through_Name").ToString
                    .Rows(n).Cells(9).Value = Dt1.Rows(I).Item("Area_Name").ToString
                    .Rows(n).Cells(10).Value = Dt1.Rows(I).Item("Transportname").ToString
                    .Rows(n).Cells(11).Value = Dt1.Rows(I).Item("FinishedProduct_Order_Code").ToString

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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvoiceNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        pnl_Back.Enabled = True
        pnl_OrderSelection.Visible = False

        dgv_OrderDetails.Rows.Clear()

        If dgv_OrderSelection.Rows.Count > 0 Then

            Dup_OrdNo = ""
            Dup_OrdDt = ""
            OrdSNo = 0


            txt_OrderNo.Text = ""
            lbl_OrderCode.Text = ""
            dtp_OrderDate.Text = ""
            cbo_Agent.Text = ""
            cbo_Area.Text = ""
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
                        dtp_OrderDate.Text = Trim(dgv_OrderSelection.Rows(i).Cells(2).Value)
                        cbo_Agent.Text = Trim(dgv_OrderSelection.Rows(i).Cells(7).Value)
                        cbo_Area.Text = Trim(dgv_OrderSelection.Rows(i).Cells(9).Value)
                        cbo_Through.Text = Trim(dgv_OrderSelection.Rows(i).Cells(8).Value)
                        cbo_Transport.Text = Trim(dgv_OrderSelection.Rows(i).Cells(10).Value)

                        Dup_OrdDt = Trim(dgv_OrderSelection.Rows(i).Cells(2).Value)

                    End If


                    With dgv_OrderDetails

                        Da = New SqlClient.SqlDataAdapter("select a.*, c.Processed_Item_Name from FinishedProduct_Order_Details a INNER JOIN Processed_Item_Head c ON c.Processed_Item_IdNo <> 0 and c.Processed_Item_IdNo = a.FinishedProduct_IdNo Where a.FinishedProduct_Order_Code = '" & Trim(dgv_OrderSelection.Rows(i).Cells(11).Value) & "' order by a.FinishedProduct_Order_Date, a.for_orderby, a.FinishedProduct_Order_No, c.Processed_Item_Name", con)
                        Dt2 = New DataTable
                        Da.Fill(Dt2)

                        If Dt2.Rows.Count > 0 Then

                            For k = 0 To Dt2.Rows.Count - 1

                                Ent_Qty = Val(Dt2.Rows(k).Item("Quantity").ToString) - Val(Dt2.Rows(k).Item("Invoice_Quantity").ToString)

                                Da = New SqlClient.SqlDataAdapter("select a.* from FinishedProduct_Invoice_Order_Details a Where a.FinishedProduct_Invoice_Code = '" & Trim(NewCode) & "' and a.FinishedProduct_Order_Code = '" & Trim(Dt2.Rows(k).Item("FinishedProduct_Order_Code").ToString) & "'  and a.FinishedProduct_IdNo = " & Str(Val(Dt2.Rows(k).Item("FinishedProduct_IdNo").ToString)) & " order by a.Sl_No", con)
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
                                    .Rows(n).Cells(1).Value = Dt2.Rows(k).Item("Processed_Item_Name").ToString
                                    .Rows(n).Cells(2).Value = Dt2.Rows(k).Item("FinishedProduct_Order_No").ToString
                                    .Rows(n).Cells(3).Value = Val(Ent_Qty)
                                    .Rows(n).Cells(4).Value = Dt2.Rows(k).Item("FinishedProduct_Order_Code").ToString
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

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyValue = 38 Then SendKeys.Send("{TAB}")
        If e.KeyValue = 40 Then
            cbo_Type.Focus()
        End If
    End Sub

    Private Sub dtp_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) = 13 Then
            cbo_Type.Focus()
        End If
    End Sub

    Private Sub Show_Item_CurrentStock(ByVal Rw As Integer)
        Dim vItemID As Integer

        If Val(Rw) < 0 Then Exit Sub

        With dgv_Details

            vItemID = Common_Procedures.Processed_Item_NameToIdNo(con, .Rows(Rw).Cells(1).Value)

            If Val(vItemID) = 0 Then Exit Sub

            If Val(vItemID) <> Val(.Tag) Then
                'Common_Procedures.Show_ProcessedItem_CurrentStock_Display(con, Val(lbl_Company.Tag), Val(Common_Procedures.CommonLedger.Godown_Ac), vItemID)
                .Tag = Val(Rw)
            End If

        End With


    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Try
            With dgv_Details
                dgv_ActCtrlName = dgv_Details.Name
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT ENTER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_SalesAc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SalesAc.KeyUp
        If e.Control = False And e.KeyValue = 17 Then

            Common_Procedures.MDI_LedType = ""
            Dim f As New LedgerCreation_Processing

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
            Dim f As New LedgerCreation_Processing

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
            smstxt = smstxt & " DATE : " & Trim(dtp_Date.Text) & Chr(13)
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
        With dgv_BaleDetails
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
End Class
