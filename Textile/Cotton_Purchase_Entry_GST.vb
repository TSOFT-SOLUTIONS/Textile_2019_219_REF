Public Class Cotton_Purchase_Entry_GST
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False

    Private Pk_Condition As String = "GCOPU-"
    Private Pk_Condition2 As String = "GCOAG-"
    Private PkCondition3_TDSCO As String = "TDSCO-"

    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String
    Private dgv_ActiveCtrl_Name As String

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_Direct_BaleDetails As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Public Sub New()
        ' This call is required by the designer.
        FrmLdSTS = True
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
        pnl_BaleSelection_ToolTip.Visible = False
        pnl_Direct_BaleDetails.Visible = False
        vmskOldText = ""
        vmskSelStrt = -1
        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black
        msk_Date.Text = ""
        dtp_Date.Text = ""
        cbo_PartyName.Text = ""
        cbo_TaxType.Text = "GST"
        cbo_Agent.Text = ""
        cbo_PurchaseAc.Text = ""
        cbo_Transport.Text = ""


        cbo_Variety.Text = ""
        txt_BillNo.Text = ""
        txt_CommKg.Text = "0.00"
        lbl_CommAmount.Text = ""

        lbl_Amount.Text = "0.00"

        txt_AddLess.Text = "0.00"
        txt_Freight.Text = "0.00"
        lbl_NetAmount.Text = "0.00"
        lbl_AmountInWords.Text = "Rupees  :  "
        txt_AssessableValue.Text = ""
        cbo_Variety.Visible = False
        cbo_Variety.Tag = -1
        lbl_CGST_Amount.Text = ""
        lbl_SGST_Amount.Text = ""
        lbl_IGST_Amount.Text = ""

        txt_VehicleNo.Text = ""


        dgv_Details.Rows.Clear()

        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        dgv_Direct_BaleDetails.Rows.Clear()
        dgv_Direct_BaleDetails.Rows.Add()
        dgv_Direct_BaleDetails_Total.Rows.Clear()
        dgv_Direct_BaleDetails_Total.Rows.Add()

        dgv_BaleDetails.Rows.Clear()
        dgv_BaleDetails.Rows.Add()
        dgv_BaleDetails.Enabled = True

        dgv_Tax_Details.Rows.Clear()
        dgv_Tax_Details.Rows.Add()
        dgv_Tax_Details.Enabled = True

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_AgentName.Text = ""
            txt_BillNo.Text = ""
            cbo_Filter_AgentName.SelectedIndex = -1
            cbo_Filter_PartyName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        cbo_PartyName.Enabled = True
        cbo_PartyName.BackColor = Color.White

        cbo_Variety.Enabled = True
        cbo_Variety.BackColor = Color.White

        Grid_Cell_DeSelect()
        dgv_ActiveCtrl_Name = ""

        NoCalc_Status = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim msktxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is Button Or TypeOf Me.ActiveControl Is MaskedTextBox Then
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

        If Me.ActiveControl.Name <> cbo_Variety.Name Then
            cbo_Variety.Visible = False
        End If

        If Me.ActiveControl.Name <> dgv_Details.Name And Not (TypeOf ActiveControl Is DataGridViewTextBoxEditingControl) Then
            pnl_BaleSelection_ToolTip.Visible = False
        End If

        Grid_Cell_DeSelect()

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.DeepPink
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
    End Sub

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim da4 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer
        Dim SLNo As Integer
        Dim LockSTS As Boolean = False

        If Val(no) = 0 Then Exit Sub

        clear()

        NoCalc_Status = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from Cotton_purchase_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cotton_Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Cotton_Purchase_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Cotton_Purchase_Date").ToString
                msk_Date.Text = dtp_Date.Text
                cbo_PartyName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                txt_BillNo.Text = dt1.Rows(0).Item("Bill_No").ToString
                cbo_TaxType.Text = dt1.Rows(0).Item("GST_Tax_Type").ToString

                cbo_Agent.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Agent_IdNo").ToString))
                txt_CommKg.Text = Format(Val(dt1.Rows(0).Item("Commission_Kg").ToString), "#########0.00")
                lbl_CommAmount.Text = dt1.Rows(0).Item("Commission_Amount").ToString
                cbo_PurchaseAc.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("PurchaseAc_IdNo").ToString))
                cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))
                txt_Freight.Text = Format(Val(dt1.Rows(0).Item("Freight").ToString), "#########0.00")

                lbl_Amount.Text = Format(Val(dt1.Rows(0).Item("Amount").ToString), "#########0.00")
                txt_AddLess.Text = Format(Val(dt1.Rows(0).Item("AddLess_Amount").ToString), "#########0.00")
                lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(dt1.Rows(0).Item("Net_Amount").ToString))
                txt_VehicleNo.Text = dt1.Rows(0).Item("Vehicle_NO").ToString


                txt_AssessableValue.Text = Format(Val(dt1.Rows(0).Item("Assessable_Value").ToString), "#########0.00")
                lbl_CGST_Amount.Text = Format(Val(dt1.Rows(0).Item("Total_CGST_Amount").ToString), "#########0.00")

                lbl_SGST_Amount.Text = Format(Val(dt1.Rows(0).Item("Total_SGST_Amount").ToString), "#########0.00")

                lbl_IGST_Amount.Text = Format(Val(dt1.Rows(0).Item("Total_IGST_Amount").ToString), "#########0.00")

                da2 = New SqlClient.SqlDataAdapter("Select a.*  from Cotton_Purchase_Details a  Where a.Cotton_Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.sl_no", con)
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
                            .Rows(n).Cells(1).Value = Common_Procedures.Variety_IdNoToName(con, Val(dt2.Rows(i).Item("Variety_Idno").ToString))
                            .Rows(n).Cells(2).Value = dt2.Rows(i).Item("BalES").ToString
                            .Rows(n).Cells(3).Value = dt2.Rows(i).Item("Bale_Nos").ToString
                            .Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")
                            .Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Actual_Weight").ToString), "##########0.000")
                            .Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.000")
                            .Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.000")
                            .Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Detail_SlNo").ToString), "########0.000")
                            .Rows(n).Cells(9).Value = dt2.Rows(0).Item("Taxable_Value").ToString
                            .Rows(n).Cells(10).Value = Val(dt2.Rows(0).Item("GST_Percentage").ToString)
                            .Rows(n).Cells(11).Value = dt2.Rows(0).Item("HSN_Code").ToString

                        Next i

                    End If

                    If .RowCount = 0 Then .Rows.Add()

                End With

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()

                    .Rows(0).Cells(2).Value = dt1.Rows(0).Item("Total_Bales").ToString
                    .Rows(0).Cells(4).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.00")
                    .Rows(0).Cells(5).Value = Format(Val(dt1.Rows(0).Item("Tot_Actual_Weight").ToString), "########0.00")
                    .Rows(0).Cells(7).Value = Format(Val(dt1.Rows(0).Item("Total_amount").ToString), "########0.00")
                End With

                da2 = New SqlClient.SqlDataAdapter("select a.*  from Cotton_Purchase_Bale_Details a  where a.Cotton_Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'  Order by Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt3)

                dgv_BaleDetails.Rows.Clear()
                SLNo = 0

                If dt3.Rows.Count > 0 Then

                    For k = 0 To dt3.Rows.Count - 1

                        n = dgv_BaleDetails.Rows.Add()

                        SLNo = SLNo + 1
                        dgv_BaleDetails.Rows(n).Cells(0).Value = Val(dt3.Rows(k).Item("Detail_SlNo").ToString)
                        dgv_BaleDetails.Rows(n).Cells(1).Value = dt3.Rows(k).Item("Bale_No").ToString
                        dgv_BaleDetails.Rows(n).Cells(2).Value = Format(Val(dt3.Rows(k).Item("Weight").ToString), "#########0.000")
                        dgv_BaleDetails.Rows(n).Cells(3).Value = (dt3.Rows(k).Item("Mixing_Code").ToString)
                        If Val(dgv_BaleDetails.Rows(n).Cells(3).Value) <> 0 Then
                            For j = 0 To dgv_BaleDetails.ColumnCount - 1
                                dgv_BaleDetails.Rows(n).Cells(j).Style.BackColor = Color.LightGray
                            Next j
                            LockSTS = True
                        End If
                    Next k

                End If
                dt3.Clear()
                dt3.Dispose()
            End If

            da4 = New SqlClient.SqlDataAdapter("Select a.* from Cotton_purchase_GST_Tax_Details a Where a.Cotton_purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' ", con)
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


            If LockSTS = True Then
                cbo_PartyName.Enabled = False
                cbo_PartyName.BackColor = Color.LightGray

                cbo_Variety.Enabled = False
                cbo_Variety.BackColor = Color.LightGray

                dgv_BaleDetails.Enabled = False

            End If
            dgv_ActiveCtrl_Name = ""
            Grid_Cell_DeSelect()

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

    Private Sub Cotton_Purchase_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Agent.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "AGENT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Agent.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PurchaseAc.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PurchaseAc.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Variety.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "VARIETY" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Variety.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Cotton_Purchase_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Me.Text = ""

        con.Open()

        ' Common_Procedures.get_CashPartyName_From_All_Entries(con)




        pnl_BaleSelection_ToolTip.Visible = False

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()


        pnl_Direct_BaleDetails.Visible = False
        pnl_Direct_BaleDetails.Left = (Me.Width - pnl_Direct_BaleDetails.Width) \ 2
        pnl_Direct_BaleDetails.Top = (Me.Height - pnl_Direct_BaleDetails.Height) \ 2
        pnl_Direct_BaleDetails.BringToFront()


        pnl_Tax.Visible = False
        pnl_Tax.Left = (Me.Width - pnl_Tax.Width) \ 2
        pnl_Tax.Top = (Me.Height - pnl_Tax.Height) \ 2
        pnl_Tax.BringToFront()

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Agent.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PurchaseAc.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Variety.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BillNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CommKg.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_CommAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TaxType.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_AddLess.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_VehicleNo.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_AgentName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_VarietyName.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus


        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Agent.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PurchaseAc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Variety.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BillNo.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_CommAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CommKg.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AddLess.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_VehicleNo.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_AgentName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_VarietyName.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TaxType.LostFocus, AddressOf ControlLostFocus


        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CommKg.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AddLess.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CommKg.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_AddLess.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Freight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress


        cbo_TaxType.Items.Clear()
        cbo_TaxType.Items.Add("-NIL-")
        cbo_TaxType.Items.Add("GST")
        cbo_TaxType.Items.Add("NO TAX")

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Cotton_Purchase_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Cotton_Purchase_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Direct_BaleDetails.Visible = True Then
                    btn_Close_Direct_BaleDetails_Click(sender, e)
                    Exit Sub

                Else
                    Close_Form()

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView
        Dim i As Integer

        If ActiveControl.Name = dgv_Details.Name Or ActiveControl.Name = dgv_Direct_BaleDetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            On Error Resume Next

            dgv1 = Nothing

            If ActiveControl.Name = dgv_Details.Name Then
                dgv1 = dgv_Details

            ElseIf dgv_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Details

            ElseIf dgv_ActiveCtrl_Name = dgv_Details.Name Then
                dgv1 = dgv_Details


            ElseIf ActiveControl.Name = dgv_Direct_BaleDetails.Name Then
                dgv1 = dgv_Direct_BaleDetails

            ElseIf dgv_Direct_BaleDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_Direct_BaleDetails

            ElseIf dgv_ActiveCtrl_Name = dgv_Direct_BaleDetails.Name Then
                dgv1 = dgv_Direct_BaleDetails
            End If

            If IsNothing(dgv1) = True Then
                Return MyBase.ProcessCmdKey(msg, keyData)
                Exit Function
            End If

            With dgv1

                If dgv1.Name = dgv_Details.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= 6 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                txt_Freight.Focus()

                            Else
                                .Focus()
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And ((.CurrentCell.ColumnIndex <> 1 And Val(.CurrentRow.Cells(1).Value) = 0) Or (.CurrentCell.ColumnIndex = 1 And Val(dgtxt_Details.Text) = 0)) Then
                                For i = 0 To .Columns.Count - 1
                                    .Rows(.CurrentCell.RowIndex).Cells(i).Value = ""
                                Next

                                txt_Freight.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If
                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then

                                txt_BillNo.Focus()

                            Else
                                .Focus()
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 2)

                            End If


                        Else
                            .Focus()
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                        End If

                        Return True

                    Else
                        Return MyBase.ProcessCmdKey(msg, keyData)

                    End If


                ElseIf dgv1.Name = dgv_Direct_BaleDetails.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 2 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then

                                Close_Direct_BaleDetails()

                            Else
                                .Focus()
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)


                            End If

                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And ((.CurrentCell.ColumnIndex <> 1 And Val(.CurrentRow.Cells(1).Value) = 0) Or (.CurrentCell.ColumnIndex = 1 And Val(dgtxt_Direct_BaleDetails.Text) = 0)) Then
                                For i = 0 To .Columns.Count - 1
                                    .Rows(.CurrentCell.RowIndex).Cells(i).Value = ""
                                Next

                                Close_Direct_BaleDetails()
                            Else
                                .Focus()
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If


                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                dgv_Details.Focus()
                                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(6)


                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 2)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

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


    End Function

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Cotton_Purchase_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Cotton_Purchase_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

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


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Da = New SqlClient.SqlDataAdapter("select * from Cotton_Purchase_Bale_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cotton_purchase_code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and  Mixing_Code <> '' ", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("Mixing_Code").ToString) = False Then
                If Trim(Dt1.Rows(0).Item("Mixing_Code").ToString) <> "" Then
                    MessageBox.Show("Already Mixing Prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()


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

            cmd.CommandText = "delete from AgentCommission_Processing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cotton_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Cotton_Purchase_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from Cotton_purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Cotton_Purchase_Bale_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'  "
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Cotton_purchase_GST_Tax_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cotton_purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            trans.Commit()

            new_record()

            MessageBox.Show("Deleted Sucessfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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


            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_AgentName.Text = ""
            cbo_Filter_VarietyName.Text = ""

            cbo_Filter_AgentName.SelectedIndex = -1
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_VarietyName.SelectedIndex = -1
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

            da = New SqlClient.SqlDataAdapter("select top 1 Cotton_Purchase_No from Cotton_purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Purchase_Code like '" & Trim(Pk_Condition) & "%'  and Cotton_Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Cotton_Purchase_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Cotton_Purchase_No from Cotton_purchase_Head where for_orderby > " & Str(Val(OrdByNo)) & " and Cotton_Purchase_Code like '" & Trim(Pk_Condition) & "%'  and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Cotton_Purchase_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Cotton_Purchase_No from Cotton_purchase_Head where for_orderby < " & Str(Val(OrdByNo)) & " and Cotton_Purchase_Code like '" & Trim(Pk_Condition) & "%'  and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Cotton_Purchase_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Cotton_Purchase_No from Cotton_purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Purchase_Code like '" & Trim(Pk_Condition) & "%'  and Cotton_Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Cotton_Purchase_No desc", con)
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

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Cotton_purchase_Head", "Cotton_Purchase_Code", "For_OrderBy", " Cotton_Purchase_Code like '" & Trim(Pk_Condition) & "%' ", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_RefNo.ForeColor = Color.Red
            msk_Date.Text = Date.Today.ToShortDateString
            Da1 = New SqlClient.SqlDataAdapter("select Top 1 a.*, b.ledger_name as PurchaseAcName, c.ledger_name as VatAcName from Cotton_purchase_Head a LEFT OUTER JOIN Ledger_Head b ON a.PurchaseAc_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.VatAc_IdNo = c.Ledger_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Cotton_Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by a.for_Orderby desc, a.Cotton_Purchase_No desc", con)
            Da1.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)

                    If Dt1.Rows(0).Item("Cotton_Purchase_Date").ToString <> "" Then msk_Date.Text = Dt1.Rows(0).Item("Cotton_Purchase_Date").ToString
                End If
                If Dt1.Rows(0).Item("PurchaseAcName").ToString <> "" Then cbo_PurchaseAc.Text = Dt1.Rows(0).Item("PurchaseAcName").ToString
                'If Dt1.Rows(0).Item("Vat_Percentage").ToString <> "" Then txt_VatPerc.Text = Val(Dt1.Rows(0).Item("Vat_Percentage").ToString)
                '   If Dt1.Rows(0).Item("VatAcName").ToString <> "" Then cbo_VatAc.Text = Dt1.Rows(0).Item("VatAcName").ToString




            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Dt2.Dispose()
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
        Dim RefCode As String

        Try

            inpno = InputBox("Enter Lot No.", "FOR FINDING...")

            RefCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Cotton_Purchase_No from Cotton_purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Purchase_Code like '" & Trim(Pk_Condition) & "%' and  Cotton_Purchase_Code = '" & Trim(RefCode) & "'", con)
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                MessageBox.Show("Lot No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        Dim movno As String, inpno As String
        Dim InvCode As String

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Cotton_Purchase_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Cotton_Purchase_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Lot No.", "FOR NEW LOT NO. INSERTION...")

            InvCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Cotton_Purchase_No from Cotton_purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Purchase_Code like '" & Trim(Pk_Condition) & "%'  and Cotton_Purchase_Code = '" & Trim(InvCode) & "'", con)
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid Lot No.", "DOES NOT INSERT NEW Lot...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

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
        Dim PurAc_ID As Integer = 0
        Dim Rck_IdNo As Integer = 0
        Dim Fp_Id As Integer = 0
        Dim Led_ID As Integer = 0
        Dim Cnt_ID As Integer = 0
        Dim Mill_ID As Integer = 0
        Dim clr_ID As Integer = 0
        Dim Vrty_ID As Integer
        Dim Agt_Idno As Integer = 0
        Dim VatAc_ID As Integer = 0
        Dim Trans_ID As Integer = 0
        Dim Unt_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Slno As Integer = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Partcls As String = ""
        Dim vTotBale As Single, vTotWght As Single, vTotAmt As Single, vTotActWght As Single
        Dim Nr As Integer = 0

        Dim VouBil As String = ""
        Dim YrnClthNm As String = ""

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Cotton_Purchase_Entry, New_Entry) = False Then Exit Sub

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
        If Val(Led_ID) = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled And cbo_PartyName.Visible Then cbo_PartyName.Focus()
            Exit Sub
        End If

        If Trim(txt_BillNo.Text) = "" Then
            MessageBox.Show("Invalid Bill No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_BillNo.Enabled And txt_BillNo.Visible Then txt_BillNo.Focus()
            Exit Sub
        End If



        Agt_Idno = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Agent.Text)

        PurAc_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PurchaseAc.Text)

        Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)

        If PurAc_ID = 0 And Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            MessageBox.Show("Invalid Purchase A/c name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_PurchaseAc.Enabled And cbo_PurchaseAc.Visible Then cbo_PurchaseAc.Focus()
            Exit Sub
        End If

        With dgv_Details

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(4).Value) <> 0 Then
                    Vrty_ID = Common_Procedures.Variety_NameToIdNo(con, dgv_Details.Rows(i).Cells(1).Value)

                    If Val(Vrty_ID) = 0 Then
                        MessageBox.Show("Invalid Variety Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                        End If
                        Exit Sub
                    End If
                    If Val(.Rows(i).Cells(2).Value) = 0 Then
                        MessageBox.Show("Invalid Bales", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(2)
                        End If
                        Exit Sub
                    End If


                    If Val(.Rows(i).Cells(4).Value) = 0 Then
                        MessageBox.Show("Invalid Weight", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(4)
                        End If
                        Exit Sub
                    End If

                End If

            Next

        End With

        'If VatAc_ID = 0 And Val(lbl_VatAmount.Text) <> 0 Then
        '    MessageBox.Show("Invalid Vat A/c name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If cbo_VatAc.Enabled And cbo_VatAc.Visible Then cbo_VatAc.Focus()
        '    Exit Sub
        'End If

        'If Val(lbl_VatAmount.Text) <> 0 And (Trim(cbo_VatType.Text) = "" Or Trim(cbo_VatType.Text) = "-NIL-") Then
        '    MessageBox.Show("Invalid Vat Type", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If cbo_VatType.Enabled And cbo_VatType.Visible Then cbo_VatType.Focus()
        '    Exit Sub
        'End If

        NoCalc_Status = False
        Total_Calculation()

        vTotBale = 0 : vTotWght = 0

        If dgv_Details_Total.RowCount > 0 Then
            vTotBale = Val(dgv_Details_Total.Rows(0).Cells(2).Value())
            vTotWght = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
            vTotAmt = Val(dgv_Details_Total.Rows(0).Cells(7).Value())
            vTotActWght = Val(dgv_Details_Total.Rows(0).Cells(5).Value())
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        Da = New SqlClient.SqlDataAdapter("select * from Cotton_Purchase_Bale_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cotton_purchase_code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and  Mixing_Code <> '' ", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("Mixing_Code").ToString) = False Then
                If Trim(Dt1.Rows(0).Item("Mixing_Code").ToString) <> "" Then
                    MessageBox.Show("Already Mixing Prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()


        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Cotton_purchase_Head", "Cotton_Purchase_Code", "For_OrderBy", "Cotton_Purchase_Code like '" & Trim(Pk_Condition) & "%' ", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If



            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@CotnDate", dtp_Date.Value.Date)

            If New_Entry = True Then
                cmd.CommandText = "Insert into Cotton_purchase_Head (       Cotton_Purchase_Code ,               Company_IdNo       ,           Cotton_Purchase_No    ,                               for_OrderBy                              , Cotton_Purchase_Date,        Ledger_IdNo      ,           Bill_No              ,                       Commission_Kg      ,      Commission_Amount  ,           Agent_IdNo    ,        PurchaseAc_IdNo    ,         Total_Bales    ,         Total_Weight      ,  Total_Amount   ,            Freight        ,                 Amount               ,           Transport_IdNo          ,              AddLess_Amount       ,                         Net_Amount               ,                Vehicle_No              , Assessable_Value                       ,Total_CGST_Amount                 ,Total_SGST_Amount                ,Total_IGST_Amount                  ,     GST_Tax_Type                  ,                        Tot_Actual_Weight        ) " &
                                    "     Values                  (   '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",      @CotnDate    , " & Str(Val(Led_ID)) & ", '" & Trim(txt_BillNo.Text) & "',    " & Val(txt_CommKg.Text) & ",  " & Val(lbl_CommAmount.Text) & ",  " & Str(Val(Agt_Idno)) & ", " & Str(Val(PurAc_ID)) & ", " & Val(vTotBale) & ", " & Str(Val(vTotWght)) & ",   " & Val(vTotAmt) & " , " & Str(Val(txt_Freight.Text)) & ", " & Str(Val(lbl_Amount.Text)) & ",  " & Str(Val(Trans_ID)) & ", " & Str(Val(txt_AddLess.Text)) & ", " & Str(Val(CSng(lbl_NetAmount.Text))) & ", '" & Trim(txt_VehicleNo.Text) & "' ," & Val(txt_AssessableValue.Text) & " ," & Val(lbl_CGST_Amount.Text) & " ," & Val(lbl_SGST_Amount.Text) & "," & Val(lbl_IGST_Amount.Text) & " ,'" & Trim(cbo_TaxType.Text) & "',                 " & Str(Val(vTotActWght)) & "   ) "
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update Cotton_purchase_Head set Cotton_Purchase_Date = @CotnDate, Ledger_IdNo = " & Str(Val(Led_ID)) & ", Agent_IdNo = " & Str(Val(Agt_Idno)) & ", PurchaseAc_IdNo = " & Str(Val(PurAc_ID)) & ", Bill_No = '" & Trim(txt_BillNo.Text) & "',   Commission_Kg = " & Val(txt_CommKg.Text) & ",  Commission_Amount =" & Val(lbl_CommAmount.Text) & ", Total_Bales = " & Val(vTotBale) & ", Total_Weight = " & Str(Val(vTotWght)) & ", Freight = " & Str(Val(txt_Freight.Text)) & ", Amount = " & Str(Val(lbl_Amount.Text)) & ", Transport_IdNo = " & Str(Val(Trans_ID)) & ", AddLess_Amount = " & Str(Val(txt_AddLess.Text)) & ",  Net_Amount = " & Str(Val(CSng(lbl_NetAmount.Text))) & ",  Vehicle_No = '" & Trim(txt_VehicleNo.Text) & "',Assessable_Value = " & Val(txt_AssessableValue.Text) & " , Total_CGST_Amount = " & Val(lbl_CGST_Amount.Text) & " ,Total_SGST_Amount = " & Val(lbl_SGST_Amount.Text) & ",Total_IGST_Amount =" & Val(lbl_IGST_Amount.Text) & ", GST_Tax_Type = '" & Trim(cbo_TaxType.Text) & "',Tot_Actual_Weight =" & Str(Val(vTotActWght)) & "   Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()


            End If

            EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)
            PBlNo = Trim(txt_BillNo.Text)
            Partcls = "Purc : Lot No. " & Trim(lbl_RefNo.Text)

            cmd.CommandText = "Delete from Cotton_Purchase_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Cotton_Purchase_Bale_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Mixing_Code =''  "
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cotton_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            With dgv_Details

                Sno = 0
                YrnClthNm = ""
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(4).Value) <> 0 Then

                        Sno = Sno + 1
                        Vrty_ID = Common_Procedures.Variety_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)
                        'Nr = 0
                        'cmd.CommandText = "Update  Cotton_Purchase_Details set Cotton_Purchase_Date = @CotnDate , Ledger_idNo =  " & Str(Val(Led_ID)) & ", Variety_idNo = " & Str(Val(Vrty_ID)) & ", Sl_No  = " & Str(Val(Sno)) & " , Bales =  " & Str(Val(.Rows(i).Cells(2).Value)) & ",Bale_nOS = '" & Trim(.Rows(i).Cells(3).Value) & "', Weight =  " & Str(Val(.Rows(i).Cells(4).Value)) & ",Rate =  " & Str(Val(.Rows(i).Cells(6).Value)) & ",Amount =  " & Str(Val(.Rows(i).Cells(7).Value)) & "  where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Purchase_Code = '" & Trim(NewCode) & "'  and Cotton_Purchase_Details_SlNo = " & Val(.Rows(i).Cells(8).Value)
                        'Nr = cmd.ExecuteNonQuery()

                        'If Nr = 0 Then
                        cmd.CommandText = "Insert into Cotton_Purchase_Details ( Cotton_Purchase_Code ,               Company_IdNo       ,   Cotton_Purchase_No    ,                     for_OrderBy                                            ,              Cotton_Purchase_Date,             Sl_No     ,    Ledger_idNo           ,   Variety_idNo    ,                      Bales               ,                  Bale_Nos                ,                        Weight                                    , Actual_Weight                            ,                Rate                       ,                 Amount                 ,     Detail_SlNo                           ,               Taxable_Value             ,           GST_Percentage                 ,           HSN_Code                 ) " &
                                            "     Values                 (   '" & Trim(Pk_Condition) & Trim(NewCode) & "'      , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",       @CotnDate            ,  " & Str(Val(Sno)) & ", " & Str(Val(Led_ID)) & ", " & Str(Val(Vrty_ID)) & "," & Str(Val(.Rows(i).Cells(2).Value)) & " ,'" & Trim(.Rows(i).Cells(3).Value) & "', " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & "," & Str(Val(.Rows(i).Cells(7).Value)) & "," & Str(Val(.Rows(i).Cells(8).Value)) & "  ," & Str(Val(.Rows(i).Cells(9).Value)) & "," & Str(Val(.Rows(i).Cells(10).Value)) & ",'" & Trim(.Rows(i).Cells(11).Value) & "') "
                        cmd.ExecuteNonQuery()
                        ' End If

                        With dgv_BaleDetails

                            For j = 0 To .RowCount - 1

                                If Val(.Rows(j).Cells(0).Value) = Val(dgv_Details.Rows(i).Cells(8).Value) Then

                                    Slno = Slno + 1

                                    'Nr = 0
                                    'cmd.CommandText = "Update  Cotton_Purchase_Bale_Details set Cotton_Purchase_Date = @CotnDate , Ledger_idNo =  " & Str(Val(Led_ID)) & ", Variety_idNo = " & Str(Val(Vrty_ID)) & ", Sl_No  = " & Str(Val(Sno)) & " , Bale_No = " & Val(.Rows(j).Cells(1).Value) & ", Weight =  " & Val(.Rows(j).Cells(2).Value) & " where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Purchase_Code = '" & Trim(NewCode) & "' and Sl_No =" & Val(Slno) & " "
                                    'Nr = cmd.ExecuteNonQuery()

                                    '  If Nr = 0 Then

                                    cmd.CommandText = "Insert into Cotton_Purchase_Bale_Details(Cotton_Purchase_Code, Company_IdNo, Cotton_Purchase_No, for_OrderBy, Cotton_Purchase_Date ,   Ledger_IdNo ,Variety_IdNo, Detail_SlNo ,Sl_No   ,Bale_No  ,  Weight ) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",    @CotnDate, " & Str(Val(Led_ID)) & ", " & Str(Val(Vrty_ID)) & "," & Val(.Rows(j).Cells(0).Value) & ", " & Val(Slno) & " , " & Val(.Rows(j).Cells(1).Value) & ", " & Str(Val(.Rows(j).Cells(2).Value)) & " )"
                                    cmd.ExecuteNonQuery()
                                    '  End If


                                    'cmd.CommandText = "Insert into Stock_Cotton_Processing_Details ( Reference_Code                        ,             Company_IdNo         ,           Reference_No        ,                               For_OrderBy                         ,        Reference_Date,     Party_Bill_No   ,   Sl_No      ,            Ledger_idNo      ,        Variety_IdNo       ,  Bale ,         Weight  ) " & _
                                    '                           "   Values  ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",    @CotnDate   , '" & Trim(PBlNo) & "'," & Val(Slno) & ", " & Str(Val(Led_ID)) & "," & Str(Val(Vrty_ID)) & ", 1 , " & Str(Val(.Rows(j).Cells(2).Value)) & " )"
                                    'cmd.ExecuteNonQuery()

                                End If

                            Next j


                        End With

                        cmd.CommandText = "Insert into Stock_Cotton_Processing_Details ( Reference_Code                        ,             Company_IdNo         ,           Reference_No        ,                               For_OrderBy          ,        Reference_Date,  Entry_ID               ,   Party_Bill_No   ,   Sl_No      ,            Ledger_idNo      ,        Variety_IdNo       ,  Bale ,         Weight  ) " &
                                                        "   Values  ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",    @CotnDate   , '" & Trim(Partcls) & "' ,'" & Trim(PBlNo) & "'," & Val(Sno) & ", " & Str(Val(Led_ID)) & "," & Str(Val(Vrty_ID)) & ", " & Str(Val(.Rows(i).Cells(2).Value)) & " , " & Str(Val(.Rows(i).Cells(4).Value)) & " )"
                        cmd.ExecuteNonQuery()

                    End If
                Next

            End With
            cmd.CommandText = "Delete from Cotton_purchase_GST_Tax_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cotton_purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Tax_Details

                Sno = 0
                For i = 0 To .Rows.Count - 1

                    If Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(6).Value) <> 0 Or Val(.Rows(i).Cells(8).Value) <> 0 Then

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into Cotton_purchase_GST_Tax_Details   ( Cotton_purchase_Code                     ,    Company_IdNo                  ,    Cotton_purchase_No         ,                               for_OrderBy                              , Cotton_purchase_Date    ,         Ledger_IdNo     ,            Sl_No     , HSN_Code                               ,      Taxable_Amount                      ,      CGST_Percentage                    ,       CGST_Amount                         ,      SGST_Percentage                     ,                    SGST_Amount          ,                    IGST_Percentage       ,                   IGST_Amount ) " &
                                                "     Values                        (   '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",       @CotnDate         , " & Str(Val(Led_ID)) & ", " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', " & Str(Val(.Rows(i).Cells(2).Value)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & "," & Str(Val(.Rows(i).Cells(5).Value)) & "  ," & Str(Val(.Rows(i).Cells(6).Value)) & "," & Str(Val(.Rows(i).Cells(7).Value)) & "," & Str(Val(.Rows(i).Cells(8).Value)) & ") "
                        cmd.ExecuteNonQuery()

                    End If

                Next i

            End With


            'AgentCommission Posting
            cmd.CommandText = "delete from AgentCommission_Processing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If Val(Agt_Idno) <> 0 Then

                cmd.CommandText = "Insert into AgentCommission_Processing_Details (  Reference_Code   ,             Company_IdNo         ,            Reference_No       ,                               For_OrderBy                              , Reference_Date,    Ledger_IdNo     ,      Agent_IdNo      ,         Entry_ID     ,      Party_BillNo    ,       Particulars         ,            Amount                          ,                       Commission_Amount     ,Commission_Type   ,Commission_For  , Commission_Rate) " &
                                                " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",   @CotnDate   ,  " & Str(Led_ID) & ", " & Str(Agt_Idno) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',   " & Str(Val(CSng(lbl_NetAmount.Text))) & ", " & Str(Val(lbl_CommAmount.Text)) & "       ,  'KG'            ,'COTTON'        , " & Val(txt_CommKg.Text) & ")"
                cmd.ExecuteNonQuery()

                'cmd.CommandText = "Insert into AgentCommission_Processing_Details (  Reference_Code   ,             Company_IdNo         ,            Reference_No       ,                               For_OrderBy                              , Reference_Date,    Ledger_IdNo     ,      Agent_IdNo      ,         Entry_ID     ,      Party_BillNo    ,       Particulars      ,       Variety_IdNo   ,            Amount                          ,                       Commission_Amount     ,Commission_Type  ,  Weight              ,Commission_For  , Commission_Rate) " & _
                '                            " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",   @CotnDate   ,  " & Str(Led_ID) & ", " & Str(Agt_Idno) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Val(Vrty_ID) & ",  " & Str(Val(CSng(lbl_NetAmount.Text))) & ", " & Str(Val(lbl_CommAmount.Text)) & "       ,  'KG'           ," & Val(vTotWght) & " ,'COTTON'        , " & Val(txt_CommKg.Text) & ")"
                'cmd.ExecuteNonQuery()
            End If


            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
            Dim vVouNarr As String = ""

            vLed_IdNos = Led_ID & "|" & PurAc_ID & "|24|25|26"

            vVouNarr = "Bill No : " & Trim(txt_BillNo.Text)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1176" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1256" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1286" Then
                If dgv_Details.Rows.Count > 0 Then
                    'If Trim(dgv_Details.Rows(0).Cells(1).Value) <> "" Then
                    '    vVouNarr = vVouNarr & IIf(vVouNarr <> "", ", ", "") & Trim(dgv_Details.Rows(0).Cells(1).Value)
                    'End If
                    If Trim(dgv_Details.Rows(0).Cells(6).Value) <> "" Then
                        vVouNarr = vVouNarr & IIf(vVouNarr <> "", ", ", "") & " Rate : " & Trim(dgv_Details.Rows(0).Cells(6).Value)
                    End If
                    If Val(vTotBale) <> 0 Then
                        vVouNarr = vVouNarr & IIf(vVouNarr <> "", ", ", "") & " Bales : " & Val(vTotBale)
                    End If
                    If Val(vTotWght) <> 0 Then
                        vVouNarr = vVouNarr & IIf(vVouNarr <> "", ", ", "") & " Weight : " & Val(vTotWght)
                    End If
                End If

            End If

            vVou_Amts = Val(CSng(lbl_NetAmount.Text)) & "|" & -1 * (Val(CSng(lbl_NetAmount.Text)) - Val(lbl_CGST_Amount.Text) - Val(lbl_SGST_Amount.Text) - Val(lbl_IGST_Amount.Text)) & "|" & -1 * Val(lbl_CGST_Amount.Text) & "|" & -1 * Val(lbl_SGST_Amount.Text) & "|" & -1 * Val(lbl_IGST_Amount.Text)
            If Common_Procedures.Voucher_Updation(con, "Gst.Cotn.Purc", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_RefNo.Text), Convert.ToDateTime(msk_Date.Text), vVouNarr, vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.OE_Software) = False Then
                Throw New ApplicationException(ErrMsg)
            End If



            'If Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            '    vLed_IdNos = Led_ID & "|" & PurAc_ID & "|" & VatAc_ID
            '    vVou_Amts = Val(CSng(lbl_NetAmount.Text)) & "|" & -1 * (Val(CSng(lbl_NetAmount.Text)) - Val(lbl_VatAmount.Text)) & "|" & -1 * Val(lbl_VatAmount.Text)
            '    If Common_Procedures.Voucher_Updation(con, "Cotn.Purc", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_RefNo.Text), dtp_Date.Value.Date, "Bill No : " & Trim(txt_BillNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr,Common_Procedures.SoftwareTypes.OE_Software) = False Then
            '        Throw New ApplicationException(ErrMsg)
            '    End If
            'End If

            If Val(lbl_CommAmount.Text) <> 0 Then
                vLed_IdNos = Agt_Idno & "|" & Val(Common_Procedures.CommonLedger.Agent_Commission_Ac)
                vVou_Amts = Val(lbl_CommAmount.Text) & "|" & -1 * Val(lbl_CommAmount.Text)
                If Common_Procedures.Voucher_Updation(con, "Ag.Comm", Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), Trim(lbl_RefNo.Text), dtp_Date.Value.Date, "Bill No : " & Trim(txt_BillNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.OE_Software) = False Then
                    Throw New ApplicationException(ErrMsg)
                End If

            End If



            'BILL POSTING
            VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), dtp_Date.Value.Date, Led_ID, Trim(txt_BillNo.Text), Agt_Idno, Val(CSng(lbl_NetAmount.Text)), "CR", Trim(Pk_Condition) & Trim(NewCode), tr, Common_Procedures.SoftwareTypes.OE_Software)
            If Trim(UCase(VouBil)) = "ERROR" Then
                Throw New ApplicationException("Error on Voucher Bill Posting")
            End If

            tr.Commit()


            MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            move_record(lbl_RefNo.Text)
        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            cmd.Dispose()
            tr.Dispose()
            Dt1.Clear()

            If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()


        End Try

    End Sub

    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")
    End Sub
    Private Sub cbo_Party_Name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName, msk_Date, cbo_TaxType, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Party_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName, cbo_TaxType, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Variety_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Variety.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Variety_Head", "Variety_Name", "(variety_type = '')", "(Variety_IdNo = 0)")
    End Sub

    Private Sub cbo_Variety_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Variety.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Variety, Nothing, Nothing, "Variety_Head", "Variety_Name", "(variety_type = '')", "(Variety_IdNo = 0)")
        With dgv_Details
            If (e.KeyValue = 38 And cbo_Variety.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                If Val(.CurrentCell.RowIndex) <= 0 Then
                    txt_BillNo.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)
                    .CurrentCell.Selected = True

                End If
            End If

            If (e.KeyValue = 40 And cbo_Variety.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then

                    txt_Freight.Focus()



                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If

            End If
        End With

    End Sub

    Private Sub cbo_Variety_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Variety.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Variety, Nothing, "Variety_Head", "Variety_Name", "(variety_type = '')", "(Variety_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_Details

                .Rows(.CurrentCell.RowIndex).Cells.Item(1).Value = Trim(cbo_Variety.Text)
                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then

                    txt_Freight.Focus()


                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If
            End With
        End If
    End Sub

    Private Sub cbo_PurchaseAc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PurchaseAc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 27)", "(Ledger_IdNo = 0)")
    End Sub


    Private Sub cbo_PurchaseAc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PurchaseAc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PurchaseAc, txt_CommKg, txt_BillNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 27)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PurchaseAc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PurchaseAc.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PurchaseAc, txt_BillNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 27)", "(Ledger_IdNo = 0)")
    End Sub
    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

    End Sub
    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, txt_Freight, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
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
        Dim n As Integer
        Dim Led_IdNo As Integer, Agt_IdNo As Integer, Varty_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Agt_IdNo = 0
            Varty_IdNo = 0
            Led_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Cotton_Purchase_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Cotton_Purchase_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Cotton_Purchase_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If
            If Trim(cbo_Filter_AgentName.Text) <> "" Then
                Agt_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_AgentName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_IdNo)) & " "
            End If
            If Val(Agt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Agent_Idno = " & Str(Val(Agt_IdNo)) & " "
            End If
            If Trim(cbo_Filter_VarietyName.Text) <> "" Then
                Varty_IdNo = Common_Procedures.Variety_NameToIdNo(con, cbo_Filter_VarietyName.Text)
            End If

            If Val(Varty_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " b.Variety_IdNo = " & Str(Val(Varty_IdNo)) & " "
            End If


            da = New SqlClient.SqlDataAdapter("select a.*,b.* , c.Ledger_Name as PartyName, d.Ledger_Name as Agent_Name ,  e.Variety_Name from Cotton_purchase_Head a  INNER JOIN Cotton_Purchase_Details b ON a.Cotton_Purchase_Code = b.Cotton_Purchase_Code INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON a.Agent_Idno = d.Ledger_IdNo  LEFT OUTER JOIN Variety_Head e ON a.Variety_Idno = e.Variety_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cotton_Purchase_Code like '" & Trim(Pk_Condition) & "%'  and a.Cotton_Purchase_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Cotton_Purchase_No", con)
            'da = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Ledger_Name as Delv_Name from Cotton_purchase_Head a INNER JOIN Cotton_Purchase_Details b ON a.Cotton_Purchase_Code = b.Cotton_Purchase_Code LEFT OUTER JOIN Ledger_Head c ON a.DeliveryTo_Idno = c.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cotton_Purchase_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Cotton_Purchase_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    'dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Cotton_Purchase_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Cotton_Purchase_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("PartyName").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Variety_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Agent_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(7).Value = dt2.Rows(i).Item("Bill_No").ToString
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
    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")
    End Sub
    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        ' Common_Procedures.get_CashPartyName_From_All_Entries(con)
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_VarietyName, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_VarietyName, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 )", "(Ledger_IdNo = 0)")



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

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        dgv_Details_CellLeave(sender, e)

    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Rect As Rectangle

        With dgv_Details
            dgv_ActiveCtrl_Name = .Name

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If
            If Val(.Rows(e.RowIndex).Cells(8).Value) = 0 Then
                If e.RowIndex = 0 Then
                    .Rows(e.RowIndex).Cells(8).Value = 1
                Else
                    .Rows(e.RowIndex).Cells(8).Value = Val(.Rows(e.RowIndex - 1).Cells(8).Value) + 1
                End If
            End If

            If e.ColumnIndex = 1 Then

                If cbo_Variety.Visible = False Or Val(cbo_Variety.Tag) <> e.RowIndex Then

                    cbo_Variety.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select variety_Name from variety_Head  where  variety_type = '' order by variety_Name", con)
                    Dt2 = New DataTable
                    Da.Fill(Dt2)
                    cbo_Variety.DataSource = Dt2
                    cbo_Variety.DisplayMember = "variety_Name"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Variety.Left = .Left + Rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_Variety.Top = .Top + Rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_Variety.Width = Rect.Width  ' .CurrentCell.Size.Width
                    cbo_Variety.Height = Rect.Height  ' rect.Height

                    cbo_Variety.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_Variety.Tag = Val(e.RowIndex)
                    cbo_Variety.Visible = True

                    cbo_Variety.BringToFront()
                    cbo_Variety.Focus()

                End If

            Else


                cbo_Variety.Visible = False

            End If



            'If e.RowIndex > 0 And e.ColumnIndex = 2 Then
            '    If Val(.CurrentRow.Cells(2).Value) = 0 And e.RowIndex = .RowCount - 1 Then
            '        .CurrentRow.Cells(2).Value = Val(.Rows(e.RowIndex - 1).Cells(2).Value) + 1

            '    End If
            '    If e.ColumnIndex = 2 And e.RowIndex = .RowCount - 1 And Val(.CurrentRow.Cells(3).Value) = 0 Then
            '        .CurrentRow.Cells(2).Value = Val(.Rows(e.RowIndex - 1).Cells(2).Value) + 1

            '    End If
            'End If

            If e.ColumnIndex = 2 Then

                Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                pnl_BaleSelection_ToolTip.Left = .Left + Rect.Left
                pnl_BaleSelection_ToolTip.Top = .Top + Rect.Top + Rect.Height + 3

                pnl_BaleSelection_ToolTip.Visible = True

            Else
                pnl_BaleSelection_ToolTip.Visible = False

            End If
        End With

    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details
            If .CurrentCell.ColumnIndex = 4 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If

            If .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
        End With
    End Sub



    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        On Error Resume Next

        With dgv_Details
            If .Visible Then
                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                If e.ColumnIndex = 4 Or e.ColumnIndex = 5 Or e.ColumnIndex = 6 Then
                    If Val(.Rows(e.RowIndex).Cells(5).Value) <> 0 Then
                        .Rows(e.RowIndex).Cells(7).Value = Format(Val(.Rows(e.RowIndex).Cells(5).Value) * Format(Val(.Rows(e.RowIndex).Cells(6).Value), "##########0.00"))
                    Else
                        .Rows(e.RowIndex).Cells(7).Value = Format(Val(.Rows(e.RowIndex).Cells(4).Value) * Format(Val(.Rows(e.RowIndex).Cells(6).Value), "##########0.00"))
                    End If
                    Total_Calculation()
                End If

            End If
        End With

    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT KEYDOWN....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_ActiveCtrl_Name = dgv_Details.Name
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        Try
            With dgv_Details
                If .Visible Then
                    'If Val(dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(9).Value) <> 0 Then
                    '    e.Handled = True
                    'End If
                    If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Then
                        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                            e.Handled = True
                        End If
                    End If

                End If

            End With


        Catch ex As Exception
            '----

        End Try

    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Try
            With dgv_Details
                vcbo_KeyDwnVal = e.KeyValue
                If e.KeyValue = Keys.Delete Then
                    If Val(dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(9).Value) <> 0 Then
                        e.Handled = True
                    End If
                End If
            End With

        Catch ex As Exception
            '----

        End Try

    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim i As Integer
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

        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            If dgv_Details.CurrentCell.ColumnIndex = 2 Then
                Bale_Selection()
            End If
        End If

    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer = 0

        With dgv_Details

            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
            If Val(.Rows(e.RowIndex).Cells(8).Value) = 0 Then
                If e.RowIndex = 0 Then
                    .Rows(e.RowIndex).Cells(8).Value = 1
                Else
                    .Rows(e.RowIndex).Cells(8).Value = Val(.Rows(e.RowIndex - 1).Cells(8).Value) + 1
                End If
            End If
        End With
    End Sub

    Private Sub cbo_Ledger_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyUp
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

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub txt_RateKg_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_AddLess_BeforeTax_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        If Val(txt_Freight.Text) <> 0 Then
            txt_Freight.Text = Format(Val(txt_Freight.Text), "#########0.00")
        Else
            txt_Freight.Text = ""
        End If
    End Sub

    Private Sub txt_RateKg_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        NetAmount_Calculation()
    End Sub

    Private Sub txt_AddLess_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AddLess.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_AddLess_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_AddLess.LostFocus
        If Val(txt_AddLess.Text) <> 0 Then
            txt_AddLess.Text = Format(Val(txt_AddLess.Text), "#########0.00")
        Else
            txt_AddLess.Text = ""
        End If
    End Sub

    Private Sub txt_AddLess_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_AddLess.TextChanged
        Total_Calculation()
    End Sub

    Private Sub txt_BillKg_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Freight.KeyDown
        If e.KeyValue = 40 Then txt_Freight.Focus()
        If e.KeyValue = 38 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.CurrentCell.Selected = True
        End If
    End Sub

    Private Sub txt_BillKg_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        Commission_Calculation()
    End Sub

    Private Sub txt_BillKg_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Freight.LostFocus
        If Val(txt_Freight.Text) <> 0 Then
            txt_Freight.Text = Format(Val(txt_Freight.Text), "#########0.000")
        Else
            txt_Freight.Text = ""
        End If

    End Sub

    Private Sub txt_BillKg_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Freight.TextChanged
        Total_Calculation()
    End Sub



    Private Sub txt_Note_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_VehicleNo.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Note_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_VehicleNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()


            Else
                msk_Date.Focus()
            End If
        End If
    End Sub

    Private Sub Amount_Calculation(ByVal CurRow As Integer, ByVal CurCol As Integer)
        On Error Resume Next

        With dgv_Details
            If .Visible Then
                If CurCol = 4 Or CurCol = 5 Or CurCol = 6 Then
                    .Rows(CurRow).Cells(7).Value = Format(Val(.Rows(CurRow).Cells(4).Value) * Val(.Rows(CurRow).Cells(6).Value), "#########0.00")
                End If

                Total_Calculation()

            End If

        End With

    End Sub

    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotBle As Single
        Dim TotWgt As Single
        Dim TotAmt As Single
        Dim Ttl_Taxable_Amount As Double

        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

        Sno = 0
        TotBle = 0 : TotWgt = 0 : TotAmt = 0

        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Trim(.Rows(i).Cells(1).Value) <> "" And Val(.Rows(i).Cells(4).Value) <> 0 Then
                    TotBle = TotBle + Val(.Rows(i).Cells(2).Value)
                    TotWgt = TotWgt + Val(.Rows(i).Cells(4).Value)
                    TotAmt = TotAmt + Val(.Rows(i).Cells(7).Value)
                    Ttl_Taxable_Amount = Ttl_Taxable_Amount + Val(.Rows(i).Cells(9).Value())

                End If

            Next

        End With

        lbl_Amount.Text = Format(Val(TotAmt), "########0.00")
        txt_AssessableValue.Text = Format(Val(TotAmt), "########0.00")

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(2).Value = Val(TotBle)
            .Rows(0).Cells(4).Value = Format(Val(TotWgt), "########0.000")
            .Rows(0).Cells(7).Value = Format(Val(TotAmt), "########0.00")
            .Rows(0).Cells(9).Value = Format(Val(Ttl_Taxable_Amount), "########0.00")
        End With

        Commission_Calculation()

        GST_Calculation()
        NetAmount_Calculation()

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


                        .Rows(RowIndx).Cells(9).Value = ""  'TAXABLE VALUE
                        .Rows(RowIndx).Cells(10).Value = ""   'GST %
                        .Rows(RowIndx).Cells(11).Value = ""  ' HSN CODE

                        If Trim(.Rows(RowIndx).Cells(1).Value) <> "" Or Val(.Rows(RowIndx).Cells(7).Value) <> 0 Then

                            HSN_Code = ""
                            GST_Per = 0
                            Get_GST_Percentage_From_ItemGroup(Trim(.Rows(RowIndx).Cells(1).Value), HSN_Code, GST_Per)


                            '-- Taxable value 
                            Taxable_Amount = Taxable_Amount + Val(.Rows(RowIndx).Cells(7).Value)


                            .Rows(RowIndx).Cells(9).Value = Format(Val(Taxable_Amount), "##########0.00")
                            .Rows(RowIndx).Cells(10).Value = Format(Val(GST_Per), "########0.00")
                            .Rows(RowIndx).Cells(11).Value = Trim(HSN_Code)

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

            da = New SqlClient.SqlDataAdapter("select * from Variety_Head Where Variety_Name ='" & Trim(ItemName) & "'", con)
            dt = New DataTable
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0).Item("HSN_Code").ToString) = False Then
                    HSN_Code = Trim(dt.Rows(0).Item("HSN_Code").ToString)
                End If
                If IsDBNull(dt.Rows(0).Item("GST_Percentege").ToString) = False Then
                    GST_PerCent = Val(dt.Rows(0).Item("GST_Percentege").ToString)
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

            cmd.CommandText = "Truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
            cmd.ExecuteNonQuery()

            If cbo_TaxType.Text = "GST" Then

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1176" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1286" Then '---- SOMANUR KALPANA COTTON (INDIA) PVT LTD (KANIYUR)  --SPINNING MILL
                    AssVal_Pack_Frgt_Ins_Amt = Format(Val(txt_AddLess.Text), "#########0.00")
                Else
                    AssVal_Pack_Frgt_Ins_Amt = Format(Val(txt_AddLess.Text) + Val(txt_Freight.Text), "#########0.00")
                End If


                With dgv_Details

                    If .Rows.Count > 0 Then
                        For i = 0 To .Rows.Count - 1
                            If Trim(.Rows(i).Cells(1).Value) <> "" Then
                                cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & " (                    Name1                ,                  Currency1            ,                       Currency2                                             ) " &
                                                    "          Values     ( '" & Trim(.Rows(i).Cells(11).Value) & "', " & Val(.Rows(i).Cells(10).Value) & " ,  " & Str(Val(.Rows(i).Cells(9).Value) + Val(AssVal_Pack_Frgt_Ins_Amt)) & " ) "
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

                da = New SqlClient.SqlDataAdapter("select Name1 as HSN_Code, Currency1 as GST_Percentage, sum(Currency2) as TaxableAmount from " & Trim(Common_Procedures.EntryTempTable) & " group by name1, Currency1 Having sum(Currency2) <> 0 order by Name1, Currency1", con)
                dt = New DataTable
                da.Fill(dt)

                If dt.Rows.Count > 0 Then

                    Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)
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

        txt_AssessableValue.Text = Format(Val(TotAss_Val), "##########0.00")
        lbl_CGST_Amount.Text = IIf(Val(TotCGST_amt) <> 0, Format(Val(TotCGST_amt), "##########0.00"), "")
        lbl_SGST_Amount.Text = IIf(Val(TotSGST_amt) <> 0, Format(Val(TotSGST_amt), "##########0.00"), "")
        lbl_IGST_Amount.Text = IIf(Val(TotIGST_amt) <> 0, Format(Val(TotIGST_amt), "##########0.00"), "")

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
    Private Sub Commission_Calculation()
        Dim CommAmt As Single = 0

        If dgv_Details_Total.RowCount > 0 Then
            CommAmt = Val(txt_CommKg.Text) * Val(dgv_Details_Total.Rows(0).Cells(4).Value)
        End If


        lbl_CommAmount.Text = Format(Val(CommAmt), "#########0.00")

    End Sub

    Private Sub NetAmount_Calculation()
        Dim GrsAmt As Single
        Dim NtAmt As Single
        Dim GST_Amt As Single = 0
        Dim vTOT_SalAmt As String = 0
        Dim vTDS_AssVal As String = 0
        Dim vTDS_Amt As String = 0

        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

        GrsAmt = Val(lbl_CGST_Amount.Text) + Val(lbl_SGST_Amount.Text) + Val(lbl_IGST_Amount.Text)

        With dgv_Details_Total
            If .Rows.Count > 0 Then
                GrsAmt = Val(.Rows(0).Cells(9).Value)
            End If
        End With

        'GST_Amt = Val(lbl_CGST_Amount.Text) + Val(lbl_SGST_Amount.Text) + Val(lbl_IGST_Amount.Text)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1176" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1286" Then '---- SOMANUR KALPANA COTTON (INDIA) PVT LTD (KANIYUR)  --SPINNING MILL
            NtAmt = Val(GrsAmt) + Val(lbl_CGST_Amount.Text) + Val(lbl_SGST_Amount.Text) + Val(lbl_IGST_Amount.Text) + Val(txt_AddLess.Text)
        Else
            NtAmt = Val(GrsAmt) + Val(lbl_CGST_Amount.Text) + Val(lbl_SGST_Amount.Text) + Val(lbl_IGST_Amount.Text) + Val(txt_Freight.Text) + Val(txt_AddLess.Text)
        End If


        lbl_NetAmount.Text = Format(Val(NtAmt), "#########0")

        lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(CSng(lbl_NetAmount.Text)))

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Cotton_purchase_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Ledger_Name as Transport_Name , e.Ledger_Name as Agent_Name  from Cotton_purchase_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON d.Ledger_IdNo = a.Transport_IdNo LEFT OUTER JOIN Ledger_Head e ON e.Ledger_IdNo = a.Agent_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cotton_Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.count_name , c.Mill_Name  from Cotton_Purchase_Details a INNER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo LEFT OUTER JOIN Mill_Head c ON a.Mill_IdNo = c.Mill_IdNo where a.Cotton_Purchase_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.for_orderby, a.Cotton_Purchase_No", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)


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
        'If Trim(UCase(Common_Procedures.settings.CompanyName)) = "" Then
        Printing_Format1(e)
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
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
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
            .Left = 40
            .Right = 45
            .Top = 45
            .Bottom = 45
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 11, FontStyle.Regular)

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

        NoofItems_PerPage = 15 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(50) : ClArr(2) = 60 : ClArr(3) = 220 : ClArr(4) = 80 : ClArr(5) = 120 : ClArr(6) = 100
        ClArr(7) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6))

        TxtHgt = 18.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


                ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

                NoofDets = 0

                CurY = CurY - 10

                'CurY = CurY + TxtHgt
                'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Mill_Name").ToString, LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1


                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 18 Then
                            For I = 18 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 18
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt

                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If




                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If


                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)


            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim C1 As Single, C2 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim DelvToName As String

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Ledger_Name as Transport_Name from Cotton_purchase_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Transport_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cotton_Purchase_Code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

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

        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, " YARN PURCHASE RECEIPT", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 50
            C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 50
            W1 = e.Graphics.MeasureString("PURCHASE NO   : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "FROM :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "Purchase No", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cotton_Purchase_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Purchase Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Cotton_Purchase_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Bill NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Bill_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))
            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Agent Name", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "Transport Name : " & Trim(prn_HdDt.Rows(0).Item("Transport_Name").ToString), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Transport Name", LMargin + C2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Name").ToString, LMargin + C2 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt + 10

            DelvToName = Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("DeliveryTo_Idno").ToString))

            'Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Delivery_Address2").ToString), LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Delivery At", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, DelvToName, LMargin + W1 + 30, CurY, 0, 0, pFont)


            Common_Procedures.Print_To_PrintDocument(e, "Rec No", LMargin + C2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Delivery_Receipt_No").ToString, LMargin + C2 + W1 + 30, CurY, 0, 0, pFont)
            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Delivery_Address3").ToString), LMargin + S1 + 10, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "MILL NAME", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, " BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TOT WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE/KG ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim C1 As Single, W1 As Single
        'Dim W1 As Single = 0
        Dim BmsInWrds As String

        ' W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

        Try

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt

                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Weight").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)


            CurY = CurY + TxtHgt

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 50
            W1 = e.Graphics.MeasureString("DISCOUNT    : ", pFont).Width
            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Discount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( - )", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Tax_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "VAT. 5 % ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Tax_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Freight", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "AddLess", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If


            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "RoundOff ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)
            LnAr(8) = CurY
            CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "E & O.E", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Total", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Net_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 15
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            CurY = CurY + TxtHgt
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
            BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "", "")

            Common_Procedures.Print_To_PrintDocument(e, "Rupees            : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            'CurY = CurY + 10
            'p1Font = New Font("Calibri", 12, FontStyle.Underline)
            'Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)

            'CurY = CurY + TxtHgt + 10
            'Common_Procedures.Print_To_PrintDocument(e, "1. We are responsible for the quality of yarn only;If any running fault or quality  ", LMargin + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "   defect noted in yarn please inform with firat fabric roll at once.We will", LMargin + 10, CurY, 0, 0, pFont)
            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "   accept only one roll at defect otherwise we do not hold ourself responsible. ", LMargin + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "2. Our responsibility ceases when goods leave our permises.", LMargin + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'If Val(Common_Procedures.User.IdNo) <> 1 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            'End If


            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "3. Subject to Tirupur jurisdiction. ", LMargin + 10, CurY, 0, 0, pFont)

            '' Common_Procedures.Print_To_PrintDocument(e, "For ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 30, CurY, 1, 0, p1Font)
            CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "4. Interest at the rate of 24% will be charge from the due date.", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "4. All payment should be made by A/C payesr cheque or draft.", LMargin + 10, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)



            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Agent_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Agent.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Agent_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Agent, cbo_PartyName, txt_CommKg, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Agent_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Agent.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Agent, txt_CommKg, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
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

    Private Sub cbo_SalesAc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PurchaseAc.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PurchaseAc.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Variety_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Variety.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then


            Dim f As New Variety_Creation("")

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Variety.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub




    Private Sub cbo_Filter_AgentName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_AgentName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_AgentName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_AgentName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_AgentName, cbo_Filter_VarietyName, btn_Filter_Show, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_AgentName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_AgentName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_AgentName, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            btn_Filter_Show_Click(sender, e)
        End If
    End Sub

    Private Sub txt_CommKg_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CommKg.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub





    Private Sub txt_CommRate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_CommKg.TextChanged
        Commission_Calculation()
    End Sub


    Private Sub lbl_NetAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbl_NetAmount.TextChanged
        lbl_AmountInWords.Text = "Rupees  :  "
        If Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            lbl_AmountInWords.Text = "Rupees  :  " & Common_Procedures.Rupees_Converstion(Val(CSng(lbl_NetAmount.Text)))
        End If
    End Sub


    Private Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        print_record()
    End Sub

    Private Sub btn_save_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        Try
            With dgv_Details
                If .Rows.Count > 0 Then
                    If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
                        dgv_Details_KeyUp(sender, e)
                    End If

                    If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
                        dgv_Details_KeyUp(sender, e)
                    End If
                End If
            End With

        Catch ex As Exception
            '----

        End Try

    End Sub

    Private Sub cbo_Filter_VarietyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_VarietyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Variety_Head", "Variety_Name", "(variety_type = '')", "(Variety_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_VarietyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_VarietyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_VarietyName, cbo_Filter_PartyName, cbo_Filter_AgentName, "Variety_Head", "Variety_Name", "(variety_type = '')", "(Variety_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_VarietyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_VarietyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_VarietyName, cbo_Filter_AgentName, "Variety_Head", "Variety_Name", "(variety_type = '')", "(Variety_IdNo = 0)")
    End Sub

    Private Sub cbo_Variety_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Variety.SelectedIndexChanged
        Total_Calculation()
    End Sub

    Private Sub cbo_Variety_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Variety.TextChanged
        Try
            If cbo_Variety.Visible Then
                With dgv_Details
                    If .Rows.Count > 0 Then
                        If Val(cbo_Variety.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                            .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Variety.Text)
                        End If
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub txt_BillNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_BillNo.KeyDown
        If e.KeyValue = 38 Then cbo_Agent.Focus()
        If e.KeyValue = 40 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.CurrentCell.Selected = True
        End If
    End Sub

    Private Sub txt_BillNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_BillNo.KeyPress

        If Asc(e.KeyChar) = 13 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            dgv_Details.CurrentCell.Selected = True
        End If
    End Sub

    Private Sub btn_Close_Direct_BaleDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close_Direct_BaleDetails.Click
        Close_Direct_BaleDetails()
    End Sub

    Private Sub Close_Direct_BaleDetails()
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da1 As SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim dgvDet_CurRow As Integer = 0
        Dim TotPcs As Single = 0
        Dim TotBals As Single = 0
        Dim TotMtrs As Single = 0
        Dim TotWgt As Single = 0
        Dim FsNo As Single, LsNo As Single
        Dim FsBaleNo As String, LsBaleNo As String
        Dim BlNo As String
        Dim fldmtr As Double = 0
        Dim fmt As Double = 0
        Dim Det_SlNo As Integer = 0
        Dim n As Integer = 0
        Try


            Det_SlNo = Val(dgv_Details.CurrentRow.Cells(8).Value)

            Cmd.Connection = con



            Cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
            Cmd.ExecuteNonQuery()

            For I = 0 To dgv_Direct_BaleDetails.Rows.Count - 1

                If Trim(dgv_Direct_BaleDetails.Rows(I).Cells(1).Value) <> "" And Val(dgv_Direct_BaleDetails.Rows(I).Cells(2).Value) <> 0 Then

                    Cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Meters1) values ('" & Trim(dgv_Direct_BaleDetails.Rows(I).Cells(1).Value) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(dgv_Direct_BaleDetails.Rows(I).Cells(1).Value))) & " ) "
                    Cmd.ExecuteNonQuery()

                End If

            Next


            BlNo = ""
            FsNo = 0 : LsNo = 0
            FsBaleNo = "" : LsBaleNo = ""

            Da1 = New SqlClient.SqlDataAdapter("Select Name1 as Bale_No, Meters1 as fororderby_baleno from " & Trim(Common_Procedures.EntryTempTable) & " where Name1 <> '' order by Meters1, Name1", con)
            Dt1 = New DataTable
            Da1.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                FsNo = Val(Dt1.Rows(0).Item("fororderby_baleno").ToString)
                LsNo = Val(Dt1.Rows(0).Item("fororderby_baleno").ToString)

                FsBaleNo = Trim(UCase(Dt1.Rows(0).Item("Bale_No").ToString))
                LsBaleNo = Trim(UCase(Dt1.Rows(0).Item("Bale_No").ToString))

                For I = 1 To Dt1.Rows.Count - 1
                    If LsNo + 1 = Val(Dt1.Rows(I).Item("fororderby_baleno").ToString) Then
                        LsNo = Val(Dt1.Rows(I).Item("fororderby_baleno").ToString)
                        LsBaleNo = Trim(UCase(Dt1.Rows(I).Item("Bale_No").ToString))

                    Else
                        If FsNo = LsNo Then
                            BlNo = BlNo & Trim(FsBaleNo) & ","
                        Else
                            BlNo = BlNo & Trim(FsBaleNo) & "-" & Trim(LsBaleNo) & ","
                        End If
                        FsNo = Dt1.Rows(I).Item("fororderby_baleno").ToString
                        LsNo = Dt1.Rows(I).Item("fororderby_baleno").ToString

                        FsBaleNo = Trim(UCase(Dt1.Rows(I).Item("Bale_No").ToString))
                        LsBaleNo = Trim(UCase(Dt1.Rows(I).Item("Bale_No").ToString))

                    End If

                Next

                If FsNo = LsNo Then BlNo = BlNo & Trim(FsBaleNo) Else BlNo = BlNo & Trim(FsBaleNo) & "-" & Trim(LsBaleNo)

            End If
            Dt1.Clear()

            Dt1.Dispose()
            Da1.Dispose()




            With dgv_BaleDetails


LOOP1:
                For I = 0 To .RowCount - 1

                    If Val(.Rows(I).Cells(0).Value) = Val(Det_SLNo) Then

                        If I = .Rows.Count - 1 Then
                            For J = 0 To .ColumnCount - 1
                                .Rows(I).Cells(J).Value = ""
                            Next

                        Else
                            .Rows.RemoveAt(I)

                        End If

                        GoTo LOOP1

                    End If

                Next I

                For I = 0 To dgv_Direct_BaleDetails.RowCount - 1

                    If Trim(dgv_Direct_BaleDetails.Rows(I).Cells(1).Value) <> "" And Val(dgv_Direct_BaleDetails.Rows(I).Cells(2).Value) <> 0 Then

                        n = .Rows.Add()

                        .Rows(n).Cells(0).Value = Val(Det_SlNo)
                        .Rows(n).Cells(1).Value = dgv_Direct_BaleDetails.Rows(I).Cells(1).Value
                        .Rows(n).Cells(2).Value = Val(dgv_Direct_BaleDetails.Rows(I).Cells(2).Value)

                    End If
                Next I

            End With



            pnl_Back.Enabled = True
            pnl_Direct_BaleDetails.Visible = False
            Total_Direct_BaleDetailsEntry_Calculation()

            TotBals = 0
            TotWgt = 0
            With dgv_Direct_BaleDetails_Total
                If .RowCount > 0 Then
                    TotBals = Val(.Rows(0).Cells(1).Value)

                    TotWgt = Val(.Rows(0).Cells(2).Value)
                End If
            End With





            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                If Val(TotBals) <> 0 And Val(TotWgt) <> 0 Then
                    dgv_Details.CurrentRow.Cells(2).Value = TotBals
                    dgv_Details.CurrentRow.Cells(3).Value = BlNo

                    dgv_Details.CurrentRow.Cells(4).Value = Format(Val(TotWgt), "#########0.00")


                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(6)
                    dgv_Details.CurrentCell.Selected = True


                End If


            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "INVALID BALAE DETAILS ENTRY...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Direct_BaleDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Direct_BaleDetails.CellEnter
        With dgv_Direct_BaleDetails
            dgv_ActiveCtrl_Name = .Name

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If e.RowIndex > 0 And e.ColumnIndex = 1 Then
                If Val(.CurrentRow.Cells(1).Value) = 0 And e.RowIndex = .RowCount - 1 Then
                    .CurrentRow.Cells(1).Value = Val(.Rows(e.RowIndex - 1).Cells(1).Value) + 1

                End If
                If e.ColumnIndex = 1 And e.RowIndex = .RowCount - 1 And Val(.CurrentRow.Cells(2).Value) = 0 Then
                    .CurrentRow.Cells(1).Value = Val(.Rows(e.RowIndex - 1).Cells(1).Value) + 1

                End If
            End If

        End With


    End Sub

    Private Sub dgv_Direct_BaleDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Direct_BaleDetails.EditingControlShowing
        '   dgtxt_Direct_BaleDetails = Nothing

        dgtxt_Direct_BaleDetails = CType(dgv_Direct_BaleDetails.EditingControl, DataGridViewTextBoxEditingControl)

    End Sub

    Private Sub dgtxt_Direct_BaleDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Direct_BaleDetails.Enter
        dgv_ActiveCtrl_Name = dgv_Direct_BaleDetails.Name
        dgv_Direct_BaleDetails.EditingControl.BackColor = Color.Lime
        dgv_Direct_BaleDetails.EditingControl.ForeColor = Color.Blue
        dgtxt_Direct_BaleDetails.SelectAll()
    End Sub

    Private Sub dgtxt_Direct_BaleDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Direct_BaleDetails.KeyDown
        Try
            vcbo_KeyDwnVal = e.KeyValue

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT KEYDOWN....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgtxt_Direct_BaleDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Direct_BaleDetails.KeyPress

        Try
            With dgv_Direct_BaleDetails
                If .Visible Then

                    If .Rows.Count > 0 Then

                        If .CurrentCell.ColumnIndex = 2 Then

                            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                                e.Handled = True
                            End If

                        End If

                    End If

                End If

            End With

        Catch ex As Exception
            '---

        End Try

    End Sub

    Private Sub dgtxt_Direct_BaleDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Direct_BaleDetails.KeyUp

        Try
            With dgv_Direct_BaleDetails
                If .Rows.Count > 0 Then
                    If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
                        dgv_Direct_BaleDetails_KeyUp(sender, e)
                    End If
                End If
            End With

        Catch ex As Exception
            '----

        End Try

    End Sub

    Private Sub dgv_Direct_BaleDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Direct_BaleDetails.CellEndEdit
        Try
            dgv_Direct_BaleDetails_CellLeave(sender, e)
        Catch ex As Exception
            '-----
        End Try

    End Sub

    Private Sub dgv_Direct_BaleDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Direct_BaleDetails.CellLeave
        Try
            With dgv_Direct_BaleDetails
                If .CurrentCell.ColumnIndex = 2 Then
                    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                    Else
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                    End If
                End If
            End With

        Catch ex As Exception
            '------

        End Try


    End Sub

    Private Sub dgv_Direct_BaleDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Direct_BaleDetails.CellValueChanged

        Try
            With dgv_Direct_BaleDetails
                If .Visible Then

                    If .CurrentCell.ColumnIndex = 2 And Val(.CurrentCell.Value) <> 0 Then
                        If .CurrentRow.Index = .Rows.Count - 1 Then
                            .Rows.Add()
                        End If
                    End If

                    If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Then

                        Total_Direct_BaleDetailsEntry_Calculation()

                    End If

                End If
            End With

        Catch ex As Exception
            '-----
        End Try

    End Sub

    Private Sub dgv_Direct_BaleDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Direct_BaleDetails.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dgv_Direct_BaleDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Direct_BaleDetails.KeyUp
        Dim n As Integer = -1


        Try
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
                With dgv_Direct_BaleDetails

                    n = .CurrentRow.Index

                    If n = .Rows.Count - 1 Then
                        For i = 0 To .ColumnCount - 1
                            .Rows(n).Cells(i).Value = ""
                        Next

                    Else
                        .Rows.RemoveAt(n)

                    End If

                    Total_Direct_BaleDetailsEntry_Calculation()

                End With

            End If

        Catch ex As Exception
            '-----

        End Try

    End Sub

    Private Sub dgv_Direct_BaleDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Direct_BaleDetails.LostFocus
        On Error Resume Next
        dgv_Direct_BaleDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Direct_BaleDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Direct_BaleDetails.RowsAdded
        Dim n As Integer = -1

        Try
            With dgv_Direct_BaleDetails
                n = .RowCount
                .Rows(n - 1).Cells(0).Value = Val(n)
            End With

        Catch ex As Exception
            '---
        End Try

    End Sub

    Private Sub Total_Direct_BaleDetailsEntry_Calculation()
        Dim Sno As Integer
        Dim TotBals As Single
        Dim TotWgt As Single

        If NoCalc_Status = True Then Exit Sub

        Sno = 0
        TotBals = 0 : TotWgt = 0

        With dgv_Direct_BaleDetails
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(2).Value) <> 0 Then
                    TotBals = TotBals + 1

                    TotWgt = TotWgt + Val(.Rows(i).Cells(2).Value())

                End If

            Next i

        End With

        With dgv_Direct_BaleDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(1).Value = Val(TotBals)

            .Rows(0).Cells(2).Value = Format(Val(TotWgt), "########0.000")
        End With

    End Sub
    Private Sub Bale_Selection()

        Dim Det_SLNo As Integer
        Dim n As Integer, SNo As Integer
        Dim Vrty_ID As Integer = 0

        Try

            Vrty_ID = Common_Procedures.Variety_NameToIdNo(con, dgv_Details.CurrentRow.Cells(1).Value)
            If Vrty_ID = 0 Then
                MessageBox.Show("Invalid Variety Name", "DOES NOT SHOW BALE DETAILS...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                dgv_Details.Focus()
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                    dgv_Details.CurrentCell.Selected = True
                End If
                Exit Sub
            End If

            Det_SLNo = Val(dgv_Details.CurrentRow.Cells(8).Value)

            With dgv_Direct_BaleDetails

                SNo = 0
                .Rows.Clear()

                For i = 0 To dgv_BaleDetails.RowCount - 1
                    If Det_SLNo = Val(dgv_BaleDetails.Rows(i).Cells(0).Value) Then

                        SNo = SNo + 1

                        n = .Rows.Add()
                        .Rows(n).Cells(0).Value = SNo
                        .Rows(n).Cells(1).Value = Trim(dgv_BaleDetails.Rows(i).Cells(1).Value)
                        .Rows(n).Cells(2).Value = Val(dgv_BaleDetails.Rows(i).Cells(2).Value)

                    End If
                Next i

            End With

            Total_Direct_BaleDetailsEntry_Calculation()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT BALE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        pnl_Direct_BaleDetails.Visible = True
        pnl_Back.Enabled = False
        dgv_Direct_BaleDetails.Focus()
        If dgv_Direct_BaleDetails.Rows.Count > 0 Then
            dgv_Direct_BaleDetails.CurrentCell = dgv_Direct_BaleDetails.Rows(0).Cells(1)
            dgv_Direct_BaleDetails.CurrentCell.Selected = True
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

    Private Sub cbo_TaxType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TaxType.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_TaxType, cbo_PartyName, cbo_Agent, "", "", "", "")
    End Sub

    Private Sub cbo_TaxType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TaxType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TaxType, cbo_Agent, "", "", "", "")
    End Sub


    Private Sub cbo_TaxType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.SelectedIndexChanged
        Total_Calculation()
    End Sub

    Private Sub cbo_PartyName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.SelectedIndexChanged
        Total_Calculation()
    End Sub

    Private Sub dgtxt_Details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.TextChanged
        Try
            With dgv_Details
                If .Rows.Count <> 0 Then
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)
                End If
            End With

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

End Class