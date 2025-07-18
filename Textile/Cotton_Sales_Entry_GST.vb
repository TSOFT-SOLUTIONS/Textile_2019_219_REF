Imports System.Drawing.Printing
Imports System.IO
Public Class Cotton_Sales_Entry_GST
    Implements Interface_MDIActions


    Private Con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private New_Entry As Boolean = False
    Private PrevAct_Ctrl As New Control
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private FrmLdSts As Boolean = False
    Private Pk_Condition As String = "GCOSE-"
    Private Pk_Condition2 As String = "GCSAL-"
    Private vcbo_KeyDwnVal As Double
    Private vmskOldText As String = ""
    Private vmsSelStrt As Integer = -1
    Private WithEvents dgtxtdetails As New DataGridViewTextBoxEditingControl
    Private dgv_ActCtrlName As String = ""
    Private NoCalc_Status As Boolean = False


    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private prn_Status As Integer = 0
    Private prn_InpOpts As String = ""
    Private prn_OriDupTri As String = ""
    Private prn_Count As Integer
    Public CHk_Details_Cnt As Integer = 0
    Private Print_PDF_Status As Boolean = False



    Public Sub New()
        FrmLdSts = True
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub Clear()

        New_Entry = False

        NoCalc_Status = True

        pnl_Filter.Visible = False
        pnl_Back.Enabled = True


        Print_PDF_Status = False

        lbl_InvNo.Text = ""
        lbl_InvNo.ForeColor = Color.Black
        lbl_HSNCode.ForeColor = Color.White
        lbl_GstPerc.ForeColor = Color.White

        txt_PrefixNo.Text = ""

        dtp_Date.Text = ""
        msk_Date.Text = ""
        dtp_RemDate.Text = ""

        lbl_HSNCode.Text = ""
        lbl_GstPerc.Text = ""
        lbl_CGSTPerc.Text = ""
        lbl_CGSTAmount.Text = ""
        lbl_SGSTPerc.Text = ""
        lbl_SGSTAmount.Text = ""
        lbl_IGSTPerc.Text = ""
        lbl_IGSTAmount.Text = ""
        lbl_RoundOff.Text = ""
        lbl_Cmsn_Amount.Text = ""
        'lbl_Amount.Text = "0.00"
        lbl_Amount.Text = ""
        lbl_AmountInWords.Text = "Rupees : "
        lbl_Company.Text = ""
        lbl_DiscAmount.Text = ""
        lbl_NetWeight.Text = ""
        lbl_TaxableValue.Text = ""
        lbl_NetAmount.Text = ""

        txt_AddLess.Text = ""
        txt_CommissionPerKG.Text = ""
        txt_DateTimeofSupply.Text = ""
        txt_Description.Text = ""
        txt_DiscPerc.Text = ""
        txt_Freight.Text = ""
        txt_Rate.Text = ""
        txt_TareWgt.Text = ""
        txt_Cmsn_Perc.Text = ""
        txt_PrefixNo.Text = ""
        txt_RemTime.Text = ""

        cbo_Agent.Text = ""
        cbo_DeliveryTo.Text = ""
        cbo_PartyName.Text = ""
        cbo_SalesAc.Text = "SALES A/C"
        cbo_TaxType.Text = "GST"
        cbo_VehicleNo.Text = ""

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


        cbo_Variety_Name.Text = ""
        cbo_SufixNo.Text = ""

        txt_CottonSales_BaleNoSelection.Text = ""
        txt_CottonSales_LotNoSelection.Text = ""

        txt_EWay_Bill_No.Text = ""
        txt_IR_No.Text = ""


        dgv_Details.Rows.Clear()
        dgv_TotalDetails.Rows.Add()
        dgv_TotalDetails.Rows.Clear()


        chk_Printed.Checked = False
        chk_Printed.Enabled = False
        chk_Printed.Visible = False

        If Filter_Status = False Then

            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_Agent.Text = ""
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_Variety.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_Variety.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()

        End If


        Grid_Cell_DeSelect()

        NoCalc_Status = False


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
        lbl_Invoice_Value_Before_TCS.Text = ""
        lbl_RoundOff_Invoice_Value_Before_TCS.Text = ""
        chk_TCS_Tax.Checked = True
    End Sub

    Private Sub Control_GotFocus(ByVal sender As Object, ByVal e As EventArgs)
        Dim txt As TextBox
        Dim cbox As ComboBox
        Dim msk As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is MaskedTextBox Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txt = Me.ActiveControl
            txt.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            cbox = Me.ActiveControl
            cbox.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is MaskedTextBox Then
            msk = Me.ActiveControl
            msk.SelectionStart = 0
        End If

        PrevAct_Ctrl = Me.ActiveControl

    End Sub

    Private Sub Control_LostFocus(ByVal sender As Object, ByVal e As EventArgs)
        On Error Resume Next

        If IsDBNull(PrevAct_Ctrl) = False Then
            If TypeOf PrevAct_Ctrl Is TextBox Or TypeOf PrevAct_Ctrl Is ComboBox Or TypeOf PrevAct_Ctrl Is MaskedTextBox Then
                PrevAct_Ctrl.BackColor = Color.White
                PrevAct_Ctrl.ForeColor = Color.Black
            End If
        End If
    End Sub

    Private Sub TextBoxControl_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        On Error Resume Next
        If e.KeyCode = 38 Then e.Handled = True : SendKeys.Send("+{TAB}")
        If e.KeyCode = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub TextBoxControl_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        On Error Resume Next
        If Asc(e.KeyChar) = 13 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub Grid_DeSelect(ByVal sender As Object, ByVal e As EventArgs)
        On Error Resume Next
        If Filter_Status = True Then Exit Sub

        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_TotalDetails.CurrentCell) Then dgv_TotalDetails.CurrentCell.Selected = False

    End Sub

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next

        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_TotalDetails.CurrentCell) Then dgv_TotalDetails.CurrentCell.Selected = False

        dgv_ActCtrlName = ""
    End Sub

    Private Sub Cotton_Sales_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Try
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(UCase(Common_Procedures.Master_Return.Return_Value)) <> "" Then
                cbo_PartyName.Text = Trim(UCase(Common_Procedures.Master_Return.Return_Value))
            ElseIf Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Agent.Text)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "AGENT" And Trim(UCase(Common_Procedures.Master_Return.Return_Value)) <> "" Then
                cbo_Agent.Text = Trim(UCase(Common_Procedures.Master_Return.Return_Value))
            ElseIf Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_DeliveryTo.Text)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(UCase(Common_Procedures.Master_Return.Return_Value)) <> "" Then
                cbo_DeliveryTo.Text = Trim(UCase(cbo_DeliveryTo.Text))
            ElseIf Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_SalesAc.Text)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(UCase(Common_Procedures.Master_Return.Return_Value)) <> "" Then
                cbo_SalesAc.Text = Trim(UCase(cbo_SalesAc.Text))
            End If

            Common_Procedures.Master_Return.Form_Name = ""
            Common_Procedures.Master_Return.Control_Name = ""
            Common_Procedures.Master_Return.Master_Type = ""
            Common_Procedures.Master_Return.Return_Value = ""


            If FrmLdSts = True Then
                lbl_Company.Text = ""
                lbl_Company.Tag = 0
                Common_Procedures.CompIdNo = 0

                Me.Text = ""
                lbl_Company.Text = Common_Procedures.get_Company_From_CompanySelection(Con)
                lbl_Company.Tag = Val(Common_Procedures.CompIdNo)

                Me.Text = lbl_Company.Text

            End If
        Catch ex As Exception
        End Try
        FrmLdSts = False
        'new_record()
    End Sub

    Private Sub Cotton_Sales_Entry_GST_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        Con.Dispose()
        Con.Close()
        Common_Procedures.Last_Closed_FormName = Me.Name
        dgv_ActCtrlName = ""
    End Sub

    Private Sub Cotton_Sales_Entry_GST_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If pnl_Filter.Visible = True Then
                btn_Filter_Close_Click(sender, e)
            ElseIf MessageBox.Show("Do you want to Close?", "FOR CLOSE", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                Me.Close()
            Else
                Exit Sub
            End If
        End If
    End Sub

    Private Sub Cotton_Sales_Entry_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Con.Open()

        cbo_TaxType.Items.Clear()
        cbo_TaxType.Items.Add("")
        cbo_TaxType.Items.Add("GST")
        cbo_TaxType.Items.Add("NO TAX")

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Back.Width) / 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) / 2
        pnl_Filter.BringToFront()

        cbo_SufixNo.Items.Clear()
        cbo_SufixNo.Items.Add("")
        cbo_SufixNo.Items.Add("/" & Common_Procedures.FnYearCode)
        cbo_SufixNo.Items.Add("/" & Year(Common_Procedures.Company_FromDate) & "-" & Year(Common_Procedures.Company_ToDate))
        cbo_SufixNo.Items.Add("/" & Year(Common_Procedures.Company_FromDate))
        cbo_SufixNo.Items.Add("/" & Trim(Year(Common_Procedures.Company_FromDate)) & "-" & Trim(Microsoft.VisualBasic.Right(Year(Common_Procedures.Company_ToDate), 2)))

        chk_Printed.Enabled = False
        If Val(Common_Procedures.User.IdNo) = 1 Then
            chk_Printed.Enabled = True
        End If


        AddHandler msk_Date.GotFocus, AddressOf Control_GotFocus
        AddHandler txt_AddLess.GotFocus, AddressOf Control_GotFocus
        AddHandler txt_Freight.GotFocus, AddressOf Control_GotFocus
        AddHandler txt_DiscPerc.GotFocus, AddressOf Control_GotFocus
        AddHandler txt_Description.GotFocus, AddressOf Control_GotFocus
        AddHandler txt_DateTimeofSupply.GotFocus, AddressOf Control_GotFocus
        AddHandler txt_CommissionPerKG.GotFocus, AddressOf Control_GotFocus
        AddHandler txt_PrefixNo.GotFocus, AddressOf Control_GotFocus
        AddHandler txt_Rate.GotFocus, AddressOf Control_GotFocus
        AddHandler txt_TareWgt.GotFocus, AddressOf Control_GotFocus
        AddHandler cbo_VehicleNo.GotFocus, AddressOf Control_GotFocus
        AddHandler txt_Cmsn_Perc.GotFocus, AddressOf Control_GotFocus

        AddHandler txt_Tcs_Name.GotFocus, AddressOf Control_GotFocus
        AddHandler txt_TcsPerc.GotFocus, AddressOf Control_GotFocus
        AddHandler txt_TCS_TaxableValue.GotFocus, AddressOf Control_GotFocus
        AddHandler cbo_SufixNo.GotFocus, AddressOf Control_GotFocus


        AddHandler cbo_DeliveryTo.GotFocus, AddressOf Control_GotFocus
        AddHandler cbo_Agent.GotFocus, AddressOf Control_GotFocus
        AddHandler cbo_PartyName.GotFocus, AddressOf Control_GotFocus
        AddHandler cbo_SalesAc.GotFocus, AddressOf Control_GotFocus
        AddHandler cbo_TaxType.GotFocus, AddressOf Control_GotFocus

        AddHandler cbo_Variety_Name.GotFocus, AddressOf Control_GotFocus
        AddHandler cbo_Filter_Agent.GotFocus, AddressOf Control_GotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf Control_GotFocus
        AddHandler cbo_Filter_Variety.GotFocus, AddressOf Control_GotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf Control_GotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf Control_GotFocus


        AddHandler txt_Rate.GotFocus, AddressOf Control_GotFocus
        AddHandler dtp_RemDate.GotFocus, AddressOf Control_GotFocus
        AddHandler txt_EWay_Bill_No.GotFocus, AddressOf Control_GotFocus
        AddHandler txt_IR_No.GotFocus, AddressOf Control_GotFocus

        AddHandler msk_Date.LostFocus, AddressOf Control_LostFocus
        AddHandler txt_AddLess.LostFocus, AddressOf Control_LostFocus
        AddHandler txt_Freight.LostFocus, AddressOf Control_LostFocus
        AddHandler txt_DiscPerc.LostFocus, AddressOf Control_LostFocus
        AddHandler txt_Description.LostFocus, AddressOf Control_LostFocus
        AddHandler txt_DateTimeofSupply.LostFocus, AddressOf Control_LostFocus
        AddHandler txt_CommissionPerKG.LostFocus, AddressOf Control_LostFocus
        AddHandler txt_PrefixNo.LostFocus, AddressOf Control_LostFocus
        AddHandler txt_Rate.LostFocus, AddressOf Control_LostFocus
        AddHandler txt_TareWgt.LostFocus, AddressOf Control_LostFocus
        AddHandler cbo_VehicleNo.LostFocus, AddressOf Control_LostFocus
        AddHandler txt_Cmsn_Perc.LostFocus, AddressOf Control_LostFocus
        AddHandler cbo_SufixNo.LostFocus, AddressOf Control_LostFocus
        AddHandler cbo_Agent.LostFocus, AddressOf Control_LostFocus
        AddHandler cbo_DeliveryTo.LostFocus, AddressOf Control_LostFocus
        AddHandler cbo_PartyName.LostFocus, AddressOf Control_LostFocus
        AddHandler cbo_SalesAc.LostFocus, AddressOf Control_LostFocus
        AddHandler cbo_TaxType.LostFocus, AddressOf Control_LostFocus

        AddHandler cbo_Variety_Name.LostFocus, AddressOf Control_LostFocus
        AddHandler cbo_Filter_Agent.LostFocus, AddressOf Control_LostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf Control_LostFocus
        AddHandler cbo_Filter_Variety.LostFocus, AddressOf Control_LostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf Control_LostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf Control_LostFocus

        AddHandler txt_RemTime.LostFocus, AddressOf Control_LostFocus
        AddHandler dtp_RemDate.LostFocus, AddressOf Control_LostFocus
        AddHandler txt_EWay_Bill_No.LostFocus, AddressOf Control_LostFocus

        AddHandler txt_Tcs_Name.LostFocus, AddressOf Control_LostFocus
        AddHandler txt_TcsPerc.LostFocus, AddressOf Control_LostFocus
        AddHandler txt_TCS_TaxableValue.LostFocus, AddressOf Control_LostFocus
        AddHandler txt_IR_No.LostFocus, AddressOf Control_LostFocus


        AddHandler txt_AddLess.KeyDown, AddressOf TextBoxControl_KeyDown
        AddHandler txt_CommissionPerKG.KeyDown, AddressOf TextBoxControl_KeyDown
        AddHandler txt_DateTimeofSupply.KeyDown, AddressOf TextBoxControl_KeyDown
        AddHandler txt_Description.KeyDown, AddressOf TextBoxControl_KeyDown
        AddHandler txt_DiscPerc.KeyDown, AddressOf TextBoxControl_KeyDown
        AddHandler txt_Freight.KeyDown, AddressOf TextBoxControl_KeyDown
        AddHandler txt_PrefixNo.KeyDown, AddressOf TextBoxControl_KeyDown
        AddHandler txt_Rate.KeyDown, AddressOf TextBoxControl_KeyDown
        'AddHandler txt_TareWgt.KeyDown, AddressOf TextBoxControl_KeyDown
        AddHandler txt_Cmsn_Perc.KeyDown, AddressOf TextBoxControl_KeyDown
        AddHandler dtp_RemDate.KeyDown, AddressOf TextBoxControl_KeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControl_KeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControl_KeyDown
        AddHandler txt_EWay_Bill_No.KeyDown, AddressOf TextBoxControl_KeyDown

        AddHandler txt_Tcs_Name.KeyDown, AddressOf TextBoxControl_KeyDown
        AddHandler txt_TcsPerc.KeyDown, AddressOf TextBoxControl_KeyDown
        AddHandler txt_TCS_TaxableValue.KeyDown, AddressOf TextBoxControl_KeyDown
        AddHandler txt_IR_No.KeyDown, AddressOf TextBoxControl_KeyDown

        AddHandler txt_Tcs_Name.KeyPress, AddressOf TextBoxControl_KeyPress
        AddHandler txt_TcsPerc.KeyPress, AddressOf TextBoxControl_KeyPress
        AddHandler txt_TCS_TaxableValue.KeyPress, AddressOf TextBoxControl_KeyPress

        AddHandler txt_AddLess.KeyPress, AddressOf TextBoxControl_KeyPress
        AddHandler txt_CommissionPerKG.KeyPress, AddressOf TextBoxControl_KeyPress
        AddHandler txt_DateTimeofSupply.KeyPress, AddressOf TextBoxControl_KeyPress
        AddHandler txt_Description.KeyPress, AddressOf TextBoxControl_KeyPress
        AddHandler txt_DiscPerc.KeyPress, AddressOf TextBoxControl_KeyPress
        AddHandler txt_Freight.KeyPress, AddressOf TextBoxControl_KeyPress
        AddHandler txt_PrefixNo.KeyPress, AddressOf TextBoxControl_KeyPress
        AddHandler txt_Rate.KeyPress, AddressOf TextBoxControl_KeyPress
        AddHandler txt_TareWgt.KeyPress, AddressOf TextBoxControl_KeyPress
        AddHandler txt_Cmsn_Perc.KeyPress, AddressOf TextBoxControl_KeyPress
        AddHandler dtp_RemDate.KeyPress, AddressOf TextBoxControl_KeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControl_KeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControl_KeyPress
        AddHandler txt_EWay_Bill_No.KeyPress, AddressOf TextBoxControl_KeyPress
        AddHandler txt_IR_No.KeyPress, AddressOf TextBoxControl_KeyPress

        new_record()

    End Sub

    Private Sub move_record(ByVal InvNo As String)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim i As Integer = 0
        Dim SNo As Integer = 0
        Dim NewCode As String = ""
        Dim n As Integer = 0

        Clear()
        New_Entry = False

        NoCalc_Status = True
        If Trim(InvNo) = "" Then Exit Sub

        NewCode = Trim(lbl_Company.Tag) & "-" & Trim(InvNo) & "/" & Trim(Common_Procedures.FnYearCode)

        Try


            da = New SqlClient.SqlDataAdapter("SELECT a.* FROM Cotton_Sales_Head a WHERE a. Company_IdNo = '" & Str(Val(lbl_Company.Tag)) & "' AND a.Cotton_Sales_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", Con)
            dt = New DataTable
            da.Fill(dt)

            If dt.Rows.Count > 0 Then

                lbl_InvNo.Text = dt.Rows(0).Item("Cotton_Sales_RefNo").ToString
                txt_PrefixNo.Text = dt.Rows(0).Item("Cotton_Sales_PreFixNo").ToString
                cbo_SufixNo.Text = dt.Rows(0).Item("Cotton_Sales_SufFixNo").ToString
                dtp_Date.Text = dt.Rows(0).Item("Cotton_Sales_Date").ToString
                msk_Date.Text = dtp_Date.Text
                cbo_PartyName.Text = Common_Procedures.Ledger_IdNoToName(Con, Val(dt.Rows(0).Item("Ledger_IdNo").ToString))
                txt_Description.Text = dt.Rows(0).Item("Description").ToString
                cbo_Agent.Text = Common_Procedures.Ledger_IdNoToName(Con, Val(dt.Rows(0).Item("Agent_IdNo").ToString))
                cbo_SalesAc.Text = Common_Procedures.Ledger_IdNoToName(Con, Val(dt.Rows(0).Item("Sales_AcIdNo").ToString))
                txt_CommissionPerKG.Text = Format(Val(dt.Rows(0).Item("Commission_Kg").ToString), "#######0.000")
                cbo_VehicleNo.Text = dt.Rows(0).Item("Vehicle_No").ToString
                cbo_TaxType.Text = dt.Rows(0).Item("Tax_Type").ToString
                lbl_HSNCode.Text = dt.Rows(0).Item("HSN_Code").ToString
                txt_DateTimeofSupply.Text = dt.Rows(0).Item("Date_Time_Of_Supply").ToString
                cbo_DeliveryTo.Text = Common_Procedures.Ledger_IdNoToName(Con, Val(dt.Rows(0).Item("DeliveryTo_IdNo").ToString))
                txt_TareWgt.Text = Format(Val(dt.Rows(0).Item("Tare_Weight").ToString), "#######0.00")
                lbl_NetWeight.Text = Format(Val(dt.Rows(0).Item("Net_Weight").ToString), "#######0.00")
                txt_Rate.Text = Format(Val(dt.Rows(0).Item("Rate").ToString), "#######0.00")
                lbl_NetAmount.Text = Format(Val(dt.Rows(0).Item("Net_Amount").ToString), "#######0.00")

                txt_DiscPerc.Text = dt.Rows(0).Item("Discount_Percentage").ToString
                lbl_DiscAmount.Text = Format(Val(dt.Rows(0).Item("Discount_Amount").ToString), "#######0.00")

                txt_Freight.Text = Format(Val(dt.Rows(0).Item("Freight").ToString), "#######0.00")
                txt_AddLess.Text = Format(Val(dt.Rows(0).Item("Add_Less").ToString), "#######0.00")
                lbl_TaxableValue.Text = Format(Val(dt.Rows(0).Item("Taxable_Value").ToString), "#######0.00")

                lbl_CGSTPerc.Text = dt.Rows(0).Item("CGST_Percentage").ToString
                lbl_CGSTAmount.Text = Format(Val(dt.Rows(0).Item("CGST_Amount").ToString), "#######0.00")
                lbl_SGSTPerc.Text = dt.Rows(0).Item("SGST_Percentage").ToString
                lbl_SGSTAmount.Text = Format(Val(dt.Rows(0).Item("SGST_Amount").ToString), "#######0.00")
                lbl_IGSTPerc.Text = dt.Rows(0).Item("IGST_Percentage").ToString
                lbl_IGSTAmount.Text = Format(Val(dt.Rows(0).Item("IGST_Amount").ToString), "#######0.00")

                lbl_GstPerc.Text = dt.Rows(0).Item("GST_Percentage").ToString
                'lbl_GstPerc.Text = Format(Val(lbl_GstPerc.Text) / 2, "##########0.0")
                'lbl_GstPerc.Text = Format(Val(lbl_GstPerc.Text) / 2, "##########0.0")


                txt_Cmsn_Perc.Text = dt.Rows(0).Item("Commission_Percentage").ToString
                lbl_Cmsn_Amount.Text = Format(Val(dt.Rows(0).Item("Commission_Amount").ToString), "#######0.00")

                lbl_RoundOff.Text = Format(Val(dt.Rows(0).Item("Round_Off").ToString), "########0.00")
                lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(dt.Rows(0).Item("Net_Amount").ToString))

                dtp_RemDate.Text = dt.Rows(0).Item("Rem_Date").ToString
                txt_RemTime.Text = dt.Rows(0).Item("Rem_Time").ToString

                lbl_Amount.Text = dt.Rows(0).Item("Amount").ToString
                txt_EWay_Bill_No.Text = dt.Rows(0).Item("EWay_Bill_No").ToString

                chk_Printed.Checked = False
                chk_Printed.Enabled = False
                chk_Printed.Visible = False
                If Val(dt.Rows(0).Item("PrintOut_Status").ToString) = 1 Then
                    chk_Printed.Checked = True
                    chk_Printed.Visible = True
                    If Val(Common_Procedures.User.IdNo) = 1 Then
                        chk_Printed.Enabled = True
                    End If
                End If
                If Val(dt.Rows(0).Item("Tcs_Tax_Status").ToString) = 1 Then chk_TCS_Tax.Checked = True Else chk_TCS_Tax.Checked = False
                txt_TCS_TaxableValue.Text = dt.Rows(0).Item("TCS_Taxable_Value").ToString
                If Val(dt.Rows(0).Item("EDIT_TCS_TaxableValue").ToString) = 1 Then
                    txt_TcsPerc.Enabled = True
                    txt_TCS_TaxableValue.Enabled = True
                End If
                If IsDBNull(dt.Rows(0).Item("TCSAmount_RoundOff_Status").ToString) = False Then
                    If Val(dt.Rows(0).Item("TCSAmount_RoundOff_Status").ToString) = 1 Then chk_TCSAmount_RoundOff_STS.Checked = True Else chk_TCSAmount_RoundOff_STS.Checked = False
                End If
                txt_TcsPerc.Text = Val(dt.Rows(0).Item("Tcs_Percentage").ToString)
                lbl_TcsAmount.Text = dt.Rows(0).Item("TCS_Amount").ToString


                txt_IR_No.Text = Trim(dt.Rows(0).Item("E_Invoice_IRNO").ToString)
                txt_eInvoiceNo.Text = Trim(dt.Rows(0).Item("E_Invoice_IRNO").ToString)
                If Not IsDBNull(dt.Rows(0).Item("E_Invoice_ACK_No")) Then txt_eInvoiceAckNo.Text = Trim(dt.Rows(0).Item("E_Invoice_ACK_No").ToString)
                If Not IsDBNull(dt.Rows(0).Item("E_Invoice_ACK_Date")) Then txt_eInvoiceAckDate.Text = Trim(dt.Rows(0).Item("E_Invoice_ACK_Date").ToString)
                If Not IsDBNull(dt.Rows(0).Item("E_Invoice_Cancelled_Status")) Then txt_eInvoice_CancelStatus.Text = IIf(dt.Rows(0).Item("E_Invoice_Cancelled_Status") = True, "Cancelled", "Active")


                If IsDBNull(dt.Rows(0).Item("E_Invoice_QR_Image")) = False Then
                    Dim imageData As Byte() = DirectCast(dt.Rows(0).Item("E_Invoice_QR_Image"), Byte())
                    If Not imageData Is Nothing Then
                        Using ms As New MemoryStream(imageData, 0, imageData.Length)
                            ms.Write(imageData, 0, imageData.Length)
                            If imageData.Length > 0 Then

                                pic_IRN_QRCode_Image.BackgroundImage = Image.FromStream(ms)

                            End If
                        End Using
                    End If
                End If

                If Not IsDBNull(dt.Rows(0).Item("E_Invoice_Cancellation_Reason")) Then txt_EInvoiceCancellationReson.Text = Trim(dt.Rows(0).Item("E_Invoice_Cancellation_Reason").ToString)

                If Not IsDBNull(dt.Rows(0).Item("EWB_No")) Then txt_eWayBill_No.Text = Trim(dt.Rows(0).Item("EWB_No").ToString)
                If Not IsDBNull(dt.Rows(0).Item("EWB_Date")) Then txt_EWB_Date.Text = Trim(dt.Rows(0).Item("EWB_Date").ToString)
                If Not IsDBNull(dt.Rows(0).Item("EWB_Valid_Upto")) Then txt_EWB_ValidUpto.Text = Trim(dt.Rows(0).Item("EWB_Valid_Upto").ToString)
                If Not IsDBNull(dt.Rows(0).Item("EWB_Cancelled")) Then
                    If dt.Rows(0).Item("EWB_Cancelled") = True Then
                        txt_EWB_Cancel_Status.Text = "Cancelled"
                    Else
                        txt_EWB_Cancel_Status.Text = "Active"
                    End If
                End If

                If Not IsDBNull(dt.Rows(0).Item("E_Invoice_Cancellation_Reason")) Then txt_EWB_Canellation_Reason.Text = Trim(dt.Rows(0).Item("E_Invoice_Cancellation_Reason").ToString)




                da1 = New SqlClient.SqlDataAdapter("Select a.*, b.Variety_Name from Cotton_Sales_Details a LEFT OUTER JOIN Variety_Head b ON a.Variety_IdNo = b.Variety_IdNo  Where a.Cotton_Sales_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.Sl_No", Con)
                dt1 = New DataTable
                da1.Fill(dt1)

                With dgv_Details

                    .Rows.Clear()
                    SNo = 0

                    If dt1.Rows.Count > 0 Then

                        For i = 0 To dt1.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(0).Value = Val(SNo)

                            .Rows(n).Cells(1).Value = dt1.Rows(i).Item("Variety_Name").ToString
                            .Rows(n).Cells(2).Value = dt1.Rows(i).Item("Lot_No").ToString
                            .Rows(n).Cells(3).Value = Format(Val(dt1.Rows(i).Item("Bale").ToString), "###########0")
                            .Rows(n).Cells(4).Value = dt1.Rows(i).Item("Bale_Nos").ToString
                            .Rows(n).Cells(5).Value = Format(Val(dt1.Rows(i).Item("Weight").ToString), "########0.000")

                        Next i

                    End If

                    If .RowCount = 0 Then .Rows.Add()

                End With


                With dgv_TotalDetails

                    If .RowCount = 0 Then .Rows.Add()

                    .Rows(0).Cells(3).Value = Val(dt.Rows(0).Item("Total_Bale").ToString)
                    .Rows(0).Cells(4).Value = Val(dt.Rows(0).Item("Total_BaleNos").ToString)
                    .Rows(0).Cells(5).Value = Format(Val(dt.Rows(0).Item("Total_Weight").ToString), "########0.00")

                End With
                get_Ledger_TotalSales()

            End If



            Grid_Cell_DeSelect()

            da.Dispose()
            dt.Dispose()
            dt.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE,.....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try

        NoCalc_Status = False

        If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = "'"

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Cotton_Sales_GST, "~L~") = 0 And InStr(Common_Procedures.UR.Cotton_Sales_GST, "~D~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        NewCode = Trim(Pk_Condition) & Trim(lbl_Company.Tag) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        'If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Cotton_Sales_GST, New_Entry, Me, Con, "Cotton_Sales_Head", "Cotton_Sales_Code", NewCode, "Cotton_Sales_Date", "(Cotton_Sales_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

        If MessageBox.Show("Do you want to Delete?..", "FOR DELETE", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, Windows.Forms.MessageBoxDefaultButton.Button3) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If
        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company!....", "DOES NOT DELETE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If
        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows!.....", "DOES NOT DELETE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If
        If New_Entry = True Then
            MessageBox.Show("This is New Entry!...", "DOES NOT DELETE,...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            msk_Date.Focus()
            Exit Sub
        End If

        tr = Con.BeginTransaction

        Try

            Cmd.Connection = Con
            Cmd.Transaction = tr

            NewCode = Trim(Pk_Condition) & Trim(lbl_Company.Tag) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


            If Common_Procedures.VoucherBill_Deletion(Con, Trim(Pk_Condition) & Trim(NewCode), tr) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            Common_Procedures.Voucher_Deletion(Con, Val(lbl_Company.Tag), Trim(NewCode), tr)
            Common_Procedures.Voucher_Deletion(Con, Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), tr)

            Cmd.CommandText = "DELETE FROM Cotton_Sales_Head WHERE Company_IdNo = " & Val(lbl_Company.Tag) & " AND Cotton_Sales_Code = '" & Trim(NewCode) & "'"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "DELETE FROM Cotton_Sales_Details WHERE Company_IdNo = " & Val(lbl_Company.Tag) & " AND Cotton_Sales_Code = '" & Trim(NewCode) & "'"
            Cmd.ExecuteNonQuery()

            tr.Commit()

            MessageBox.Show("Deleted Successfully", "FOR DELETE", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            new_record()

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try

        Cmd.Dispose()
        tr.Dispose()

        If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
    End Sub

    Private Sub Open_Filter_Entry()
        Dim move As String = ""
        On Error Resume Next

        move = Trim(dgv_Filter_Details.CurrentRow.Cells(0).Value)

        If Trim(move) <> "" Then
            Filter_Status = True
            move_record(move)
            pnl_Back.Enabled = True
            pnl_Filter.Visible = False
        End If
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        Filter_Status = True
        pnl_Back.Enabled = False
        pnl_Filter.Visible = True
        pnl_Filter.BringToFront()

        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim NewCode As String


        Dim vCSMovNo As String
        Dim vCSInvCode As String = ""

        Dim InvCode As String = ""
        Dim vInvNo As String = ""

        Dim vYSInvCode As String = ""
        Dim vYSMovNo As String = ""

        Dim vOSmovCode As String = ""
        Dim vOSmovNo As String = ""

        Dim vJWmovCode As String = ""
        Dim vJWmovNo As String = ""

        Dim vCOTSmovCode As String = ""
        Dim vCOTSmovNo As String = ""

        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Cotton_Sales_GST, "~L~") = 0 And InStr(Common_Procedures.UR.Cotton_Sales_GST, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        ' If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Cotton_Sales_GST, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Invoice No.", "FOR NEW INVOCIE NO. INSERTION...")

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Cotton_Sales_RefNo from Cotton_Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Sales_Code = '" & Trim(NewCode) & "'", Con)
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()


            '--------------

            vYSMovNo = ""
            If Common_Procedures.settings.Cloth_Yarn_General_Sales_Invoice_ContinousNo_Status = 1 Then

                vCSInvCode = "GCINV-" & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

                Da = New SqlClient.SqlDataAdapter("select ClothSales_Invoice_RefNo from ClothSales_Invoice_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Invoice_Code = '" & Trim(vCSInvCode) & "' and ClothSales_Invoice_Code LIKE 'GCINV-%' ", Con)
                Dt = New DataTable
                Da.Fill(Dt)

                vCSMovNo = ""
                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        vCSMovNo = Trim(Dt.Rows(0)(0).ToString)
                    End If
                End If

                Dt.Clear()

                vYSInvCode = "GYNSL-" & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

                Da = New SqlClient.SqlDataAdapter("select Yarn_Sales_No from Yarn_Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Sales_Code = '" & Trim(vYSInvCode) & "'", Con)
                Dt = New DataTable
                Da.Fill(Dt)
                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        vYSMovNo = Trim(Dt.Rows(0)(0).ToString)
                    End If
                End If

                Dt.Clear()
                vOSmovCode = "GSSAL-" & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

                Da = New SqlClient.SqlDataAdapter("select Other_GST_Entry_No from Other_GST_Entry_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Other_GST_Entry_Reference_Code = '" & Trim(vOSmovCode) & "' and Other_GST_Entry_Reference_Code LIKE 'GSSAL-%' ", Con)
                Dt = New DataTable
                Da.Fill(Dt)

                vOSmovNo = ""
                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        vOSmovNo = Trim(Dt.Rows(0)(0).ToString)
                    End If
                End If

                Dt.Clear()


                vJWmovCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

                Da = New SqlClient.SqlDataAdapter("select JobWork_ConversionBill_RefNo from JobWork_ConversionBill_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_ConversionBill_Code = '" & Trim(vJWmovCode) & "'  ", Con)
                Dt = New DataTable
                Da.Fill(Dt)

                vJWmovNo = ""
                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        vJWmovNo = Trim(Dt.Rows(0)(0).ToString)
                    End If
                End If

                Dt.Clear()

            End If

            '--------------


            If Trim(movno) <> "" Then
                move_record(movno)
            ElseIf Val(vYSMovNo) <> 0 Then
                MessageBox.Show("This Invoice No. is in Yarn Invoice", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf Val(vOSmovNo) <> 0 Then
                MessageBox.Show("Already this Invoice No. in Other Sales Invoice", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf Val(vJWmovNo) <> 0 Then
                MessageBox.Show("Already this Invoice No. in Jobwork Invoice", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf Val(vCSMovNo) <> 0 Then
                MessageBox.Show("This Invoice No. is in Cloth Invoice", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid Ref No.", "DOES NOT INSERT NEW Ref...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_InvNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW INVOICE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim move As String = ""


        Try
            Da = New SqlClient.SqlDataAdapter("SELECT TOP 1 Cotton_Sales_RefNo FROM Cotton_Sales_Head WHERE Company_IdNo = " & Val(lbl_Company.Tag) & " AND Cotton_Sales_Code Like '%/" & Trim(Common_Procedures.FnYearCode) & "' ORDER BY for_OrderBy , Cotton_Sales_RefNo", Con)
            dt = New DataTable
            Da.Fill(dt)


            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    move = Trim(dt.Rows(0)(0).ToString)
                End If
            End If

            dt.Dispose()
            Da.Dispose()
            dt.Clear()

            'If Trim(move) <> "" Then
            If Val(move) <> 0 Then
                move_record(move)
            Else
                new_record()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try

        If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim move As String = ""

        Try
            Da = New SqlClient.SqlDataAdapter("SELECT TOP 1 Cotton_Sales_RefNo FROM Cotton_Sales_Head WHERE Company_IdNo = " & Val(lbl_Company.Tag) & " AND Cotton_Sales_Code Like '%/" & Trim(Common_Procedures.FnYearCode) & "' ORDER BY for_OrderBy DESC, Cotton_Sales_RefNo DESC", Con)
            dt = New DataTable
            Da.Fill(dt)

            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    move = Trim(dt.Rows(0)(0).ToString)
                End If
            End If

            dt.Dispose()
            Da.Dispose()
            dt.Clear()

            If Val(move) <> 0 Then

                move_record(move)
            Else
                new_record()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try

        If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim move As String = ""
        Dim vOrderBy As String = ""

        Try
            vOrderBy = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_InvNo.Text))

            Da = New SqlClient.SqlDataAdapter("SELECT TOP 1 Cotton_Sales_RefNo FROM Cotton_Sales_Head WHERE for_OrderBy >" & Str(Val(vOrderBy)) & " AND Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " AND Cotton_Sales_Code Like '" & Trim(Pk_Condition) & "%' AND Cotton_Sales_Code Like '%/" & Trim(Common_Procedures.FnYearCode) & "' ORDER BY for_OrderBy , Cotton_Sales_RefNo", Con)
            dt = New DataTable
            Da.Fill(dt)

            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    move = Trim(dt.Rows(0)(0).ToString)
                End If
            End If

            dt.Dispose()
            Da.Dispose()
            dt.Clear()

            If Val(move) <> 0 Then

                move_record(move)
            Else
                new_record()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try

        If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim move As String = ""
        Dim vOrderBy As String = ""

        Try
            vOrderBy = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_InvNo.Text))

            Da = New SqlClient.SqlDataAdapter("SELECT TOP 1 Cotton_Sales_RefNo FROM Cotton_Sales_Head WHERE for_OrderBy <" & Str(Val(vOrderBy)) & " AND Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " AND Cotton_Sales_Code Like '" & Trim(Pk_Condition) & "%' AND Cotton_Sales_Code Like '%/" & Trim(Common_Procedures.FnYearCode) & "' ORDER BY for_OrderBy DESC , Cotton_Sales_RefNo DESC", Con)
            dt = New DataTable
            Da.Fill(dt)

            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    move = Trim(dt.Rows(0)(0).ToString)
                End If
            End If

            dt.Dispose()
            Da.Dispose()
            dt.Clear()

            If Val(move) <> 0 Then
                move_record(move)
            End If

            'Else
            '    new_record()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
        If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Try
            Clear()
            New_Entry = True
            If Common_Procedures.settings.Cloth_Yarn_General_Sales_Invoice_ContinousNo_Status = 1 Then
                lbl_InvNo.Text = Common_Procedures.get_CloYarn_MaxCode(Con, Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            Else
                lbl_InvNo.Text = Common_Procedures.get_MaxCode(Con, "Cotton_Sales_Head", "Cotton_Sales_Code", "for_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            End If

            lbl_InvNo.ForeColor = Color.Red

            Da1 = New SqlClient.SqlDataAdapter("select Top 1 a.* from Cotton_Sales_Head a  where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Cotton_Sales_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by a.for_Orderby desc, a.Cotton_Sales_RefNo desc", Con)
            Da1.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then
                '  If Dt1.Rows(0).Item("SalesAcName").ToString <> "" Then cbo_SalesAc.Text = Dt1.Rows(0).Item("SalesAcName").ToString
                'If Dt1.Rows(0).Item("Vat_Type").ToString <> "" Then cbo_Vatype.Text = Dt1.Rows(0).Item("Vat_Type").ToString
                'If Dt1.Rows(0).Item("Vat_Percentage").ToString <> "" Then txt_VatPerc.Text = Val(Dt1.Rows(0).Item("Vat_Percentage").ToString)
                'If Dt1.Rows(0).Item("VatAcName").ToString <> "" Then cbo_VatAc.Text = Dt1.Rows(0).Item("VatAcName").ToString


                If IsDBNull(Dt1.Rows(0).Item("Tcs_Tax_Status").ToString) = False Then


                    If Val(Dt1.Rows(0).Item("Tcs_Tax_Status").ToString) = 1 Then chk_TCS_Tax.Checked = True Else chk_TCS_Tax.Checked = False

                End If

                If IsDBNull(Dt1.Rows(0).Item("TCSAmount_RoundOff_Status").ToString) = False Then
                    If Val(Dt1.Rows(0).Item("TCSAmount_RoundOff_Status").ToString) = 1 Then chk_TCSAmount_RoundOff_STS.Checked = True Else chk_TCSAmount_RoundOff_STS.Checked = False
                End If

                If IsDBNull(Dt1.Rows(0).Item("Cotton_Sales_PreFixNo").ToString) = False Then
                    If Dt1.Rows(0).Item("Cotton_Sales_PreFixNo").ToString <> "" Then txt_PrefixNo.Text = Dt1.Rows(0).Item("Cotton_Sales_PreFixNo").ToString
                End If

                If IsDBNull(Dt1.Rows(0).Item("Cotton_Sales_SufFixNo").ToString) = False Then
                    If Dt1.Rows(0).Item("Cotton_Sales_SufFixNo").ToString <> "" Then cbo_SufixNo.Text = Dt1.Rows(0).Item("Cotton_Sales_SufFixNo").ToString
                End If

            End If

                Dt1.Clear()

            If dtp_Date.Enabled Then
                msk_Date.Text = Date.Today.ToShortDateString
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try

        msk_Date.SelectionStart = 0
        If txt_PrefixNo.Enabled And txt_PrefixNo.Visible Then txt_PrefixNo.Focus()

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String = "", InpNo As String = "", RefCode As String = ""

        Dim vCSMovNo As String
        Dim vCSInvCode As String = ""

        Dim InvCode As String = ""
        Dim vInvNo As String = ""

        Dim vYSInvCode As String = ""
        Dim vYSMovNo As String = ""

        Dim vOSmovCode As String = ""
        Dim vOSmovNo As String = ""

        Dim vJWmovCode As String = ""
        Dim vJWmovNo As String = ""

        Dim vCOTSmovCode As String = ""
        Dim vCOTSmovNo As String = ""

        Try

            InpNo = InputBox("Enter Invoice No.", "FOR FINDING,.....")

            RefCode = Trim(Pk_Condition) & Trim(lbl_Company.Tag) & "-" & Trim(InpNo) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("SELECT Cotton_Sales_RefNo FROM Cotton_Sales_Head WHERE Company_IdNo =" & Val(lbl_Company.Tag) & " AND Cotton_Sales_Code = '" & Trim(RefCode) & "'", Con)
            Dt = New DataTable
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()

            '--------------

            vYSMovNo = ""
            If Common_Procedures.settings.Cloth_Yarn_General_Sales_Invoice_ContinousNo_Status = 1 Then

                vCSInvCode = "GCINV-" & Trim(Val(lbl_Company.Tag)) & "-" & Trim(InpNo) & "/" & Trim(Common_Procedures.FnYearCode)

                Da = New SqlClient.SqlDataAdapter("select ClothSales_Invoice_RefNo from ClothSales_Invoice_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothSales_Invoice_Code = '" & Trim(vCSInvCode) & "' and ClothSales_Invoice_Code LIKE 'GCINV-%' ", Con)
                Dt = New DataTable
                Da.Fill(Dt)

                vCSMovNo = ""
                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        vCSMovNo = Trim(Dt.Rows(0)(0).ToString)
                    End If
                End If

                Dt.Clear()

                vYSInvCode = "GYNSL-" & Trim(Val(lbl_Company.Tag)) & "-" & Trim(InpNo) & "/" & Trim(Common_Procedures.FnYearCode)

                Da = New SqlClient.SqlDataAdapter("select Yarn_Sales_No from Yarn_Sales_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Yarn_Sales_Code = '" & Trim(vYSInvCode) & "'", Con)
                Dt = New DataTable
                Da.Fill(Dt)
                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        vYSMovNo = Trim(Dt.Rows(0)(0).ToString)
                    End If
                End If

                Dt.Clear()
                vOSmovCode = "GSSAL-" & Trim(Val(lbl_Company.Tag)) & "-" & Trim(InpNo) & "/" & Trim(Common_Procedures.FnYearCode)

                Da = New SqlClient.SqlDataAdapter("select Other_GST_Entry_No from Other_GST_Entry_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Other_GST_Entry_Reference_Code = '" & Trim(vOSmovCode) & "' and Other_GST_Entry_Reference_Code LIKE 'GSSAL-%' ", Con)
                Dt = New DataTable
                Da.Fill(Dt)

                vOSmovNo = ""
                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        vOSmovNo = Trim(Dt.Rows(0)(0).ToString)
                    End If
                End If

                Dt.Clear()


                vJWmovCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(InpNo) & "/" & Trim(Common_Procedures.FnYearCode)

                Da = New SqlClient.SqlDataAdapter("select JobWork_ConversionBill_RefNo from JobWork_ConversionBill_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and JobWork_ConversionBill_Code = '" & Trim(vJWmovCode) & "'  ", Con)
                Dt = New DataTable
                Da.Fill(Dt)

                vJWmovNo = ""
                If Dt.Rows.Count > 0 Then
                    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                        vJWmovNo = Trim(Dt.Rows(0)(0).ToString)
                    End If
                End If

                Dt.Clear()

            End If

            '--------------

            If Trim(movno) <> "" Then
                move_record(movno)
            ElseIf Val(vYSMovNo) <> 0 Then
                MessageBox.Show("This Invoice No. is in Yarn Invoice", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf Val(vOSmovNo) <> 0 Then
                MessageBox.Show("Already this Invoice No. in Other Sales Invoice", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf Val(vJWmovNo) <> 0 Then
                MessageBox.Show("Already this Invoice No. in Jobwork Invoice", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf Val(vCSMovNo) <> 0 Then
                MessageBox.Show("This Invoice No. is in Cloth Invoice", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            Else
            MessageBox.Show("Inv No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

            'Else
            '    new_record()
            'End If

            Dt.Dispose()
            Da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT OPEN", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try

        If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim ps As Printing.PaperSize
        Dim PpSzSTS As Boolean = False

        ' If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Cotton_Sales_GST, New_Entry) = False Then Exit Sub
        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Cotton_Sales_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Sales_Code = '" & Trim(NewCode) & "'", Con)
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

                If Print_PDF_Status = True Then
                    '--This is actual & correct 
                    'PrintDocument1.DocumentName = "Invoice"
                    'PrintDocument1.PrinterSettings.PrinterName = "doPDF v7"
                    'PrintDocument1.PrinterSettings.PrintFileName = "c:\Invoice.pdf"
                    'PrintDocument1.Print()



                    'MessageBox.Show("Printing_Invoice - 11")
                    PrintDocument1.DocumentName = "Invoice"
                    'MessageBox.Show("Printing_Invoice - 12")
                    PrintDocument1.PrinterSettings.PrinterName = "doPDF v7"
                    'MessageBox.Show("Printing_Invoice - 13")
                    PrintDocument1.PrinterSettings.PrintFileName = "c:\Invoice.pdf"
                    'MessageBox.Show("Printing_Invoice - 14")
                    PrintDocument1.Print()
                    'MessageBox.Show("Printing_Invoice - 15")

                Else

                    'PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    'If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    '    PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
                    '    PrintDocument1.Print()
                    'End If

                    If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                        PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                        If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                            PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings


                            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                                    Exit For
                                End If
                            Next

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

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim Cmd As New SqlClient.SqlCommand
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim vOrderByNo As Integer = 0
        Dim Led_ID As Integer = 0
        Dim LedTo_ID As Integer = 0
        Dim Agt_ID As Integer = 0
        Dim SlsAc_Id As Integer = 0
        Dim Tax_Id As Integer = 0
        Dim vTotBleNo As Integer = 0, vTotWght As Integer = 0, vTotBleS As Integer = 0
        Dim Vrty_IdNo As Integer = 0, Lot_Id As Integer = 0
        Dim i As Integer = 0
        Dim Sno As Integer = 0
        Dim vInvNo As String = ""
        Dim Partcls As String = ""
        Dim Bill_Details As String = ""
        Dim vTotWegth As Single
        Dim vVou_BlAmt As Single
        Dim VouBil As String = ""
        Dim PurAc_ID As Integer = 0
        'Dim Agt_Idno As Integer = 0
        Dim vTCS_AssVal_EditSTS As Integer = 0
        Dim vTCS_Tax_Sts As Integer = 0
        Dim vTCSAmtRndOff_STS As Integer = 0
        Dim vEInvAckDate As String = ""

        '   If Common_Procedures.UserRight_Check(Common_Procedures.UR.Cotton_Sales_GST, New_Entry) = False Then Exit Sub

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection!.....", "DOET NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Pk_Condition) & Trim(lbl_Company.Tag) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        'If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Cotton_Sales_GST, New_Entry, Me, Con, "Cotton_Sales_Head", "Cotton_Sales_Code", NewCode, "Cotton_Sales_Date", "(Cotton_Sales_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Sales_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Cotton_Sales_No desc", dtp_Date.Value.Date) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close other Windows!...", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If
        If IsDate(msk_Date.Text) = False Then
            MessageBox.Show("Invalid Date!....", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            msk_Date.Focus()
            Exit Sub
        End If
        If Not (Convert.ToDateTime(msk_Date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_Date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Out of Financial Year!....", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            msk_Date.Focus()
            Exit Sub
        End If

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(Con, cbo_PartyName.Text)
        If Val(Led_ID) = 0 Then
            MessageBox.Show("Select Party Name!...", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            cbo_PartyName.Focus()
            Exit Sub
        End If

        LedTo_ID = Common_Procedures.Ledger_AlaisNameToIdNo(Con, cbo_DeliveryTo.Text)
        Agt_ID = Common_Procedures.Ledger_AlaisNameToIdNo(Con, cbo_Agent.Text)
        SlsAc_Id = Common_Procedures.Ledger_AlaisNameToIdNo(Con, cbo_SalesAc.Text)


        If Val(SlsAc_Id) = 0 Then
            MessageBox.Show("Please Select Sales A/c Group!...", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            cbo_SalesAc.Focus()
            Exit Sub
        End If

        With dgv_Details

            For i = 0 To .Rows.Count - 1
                'Vrty_IdNo = Common_Procedures.Variety_NameToIdNo(Con, .Rows(i).Cells(1).Value)

                If Val(.Rows(i).Cells(5).Value) <> 0 Then
                    Vrty_IdNo = Common_Procedures.Variety_NameToIdNo(Con, .Rows(i).Cells(1).Value)
                    If Val(Vrty_IdNo) = 0 Then
                        MessageBox.Show("Select Variety Name!....", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If dgv_Details.Enabled And dgv_Details.Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(1)
                        End If
                        Exit Sub
                    End If
                End If

            Next
        End With

        NoCalc_Status = False
        Total_Calculation()

        vTotBleNo = 0 : vTotWght = 0 : vTotBleS = 0

        If dgv_TotalDetails.RowCount > 0 Then
            vTotBleS = Val(dgv_TotalDetails.Rows(0).Cells(3).Value())
            vTotBleNo = Val(dgv_TotalDetails.Rows(0).Cells(4).Value())
            vTotWght = Val(dgv_TotalDetails.Rows(0).Cells(5).Value())
        End If

        Net_Amount_Calculation()


        vTCS_Tax_Sts = 0
        If chk_TCS_Tax.Checked = True Then vTCS_Tax_Sts = 1
        vTCS_AssVal_EditSTS = 0
        If txt_TCS_TaxableValue.Enabled = True Then vTCS_AssVal_EditSTS = 1
        vTCSAmtRndOff_STS = 0
        If chk_TCSAmount_RoundOff_STS.Checked = True Then vTCSAmtRndOff_STS = 1

        Cmd.Parameters.Clear()
        Cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(msk_Date.Text))
        Cmd.Parameters.AddWithValue("@Remdate", dtp_RemDate.Value.Date)

        Dim ms As New MemoryStream()
        If IsNothing(pic_IRN_QRCode_Image.BackgroundImage) = False Then
            Dim bitmp As New Bitmap(pic_IRN_QRCode_Image.BackgroundImage)
            bitmp.Save(ms, Drawing.Imaging.ImageFormat.Jpeg)
        End If
        Dim data As Byte() = ms.GetBuffer()
        Dim p As New SqlClient.SqlParameter("@QrCode", SqlDbType.Image)
        p.Value = data
        Cmd.Parameters.Add(p)
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
            Cmd.Parameters.AddWithValue("@EInvoiceAckDate", Convert.ToDateTime(vEInvAckDate))
        End If


        Dim eiCancel As String = "0"
        If txt_eInvoice_CancelStatus.Text = "Cancelled" Then
            eiCancel = "1"
        End If
        Dim EWBCancel As String = "0"
        If txt_EWB_Cancel_Status.Text = "Cancelled" Then
            eiCancel = "1"
        End If

        tr = Con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then

                NewCode = Trim(Pk_Condition) & Trim(lbl_Company.Tag) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else
                If Common_Procedures.settings.Cloth_Yarn_General_Sales_Invoice_ContinousNo_Status = 1 Then
                    lbl_InvNo.Text = Common_Procedures.get_CloYarn_MaxCode(Con, Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)
                Else

                    lbl_InvNo.Text = Common_Procedures.get_MaxCode(Con, "Cotton_Sales_Head", "Cotton_Sales_Code", "for_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)
                End If

                NewCode = Trim(Pk_Condition) & Trim(lbl_Company.Tag) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            End If


            vInvNo = Trim(txt_PrefixNo.Text) & Trim(lbl_InvNo.Text) & Trim(cbo_SufixNo.Text)


            Cmd.Connection = Con
            Cmd.Transaction = tr



            vOrderByNo = Common_Procedures.OrderBy_CodeToValue(lbl_InvNo.Text)

            If New_Entry = True Then

                If Trim(txt_DateTimeofSupply.Text) = "" Then txt_DateTimeofSupply.Text = Format(Now, "dd-MM-yyyy hh:mm tt")

                Cmd.CommandText = "INSERT INTO Cotton_SaleS_Head ( Company_IdNo               ,   Cotton_Sales_Code     ,       Cotton_Sales_RefNo    ,          Cotton_Sales_PreFixNo             ,        Cotton_Sales_InvNo        ,         for_OrderBy           , Cotton_Sales_Date ,        Ledger_IdNo       ,                    Description              ,           Agent_IdNo      ,           Sales_AcIdNo      ,             Commission_Kg             ,                      Vehicle_No           ,               Tax_Type           ,               HSN_Code           ,           GST_Percentage      ,               Date_Time_of_Supply         ,       DeliveryTo_IdNo ,            Tare_Weight        ,             Net_Weight          ,             Rate           ,             Amount           ,         Discount_Percentage    ,             Discount_Amount      ,             Freight           ,             Add_Less          ,             Taxable_Value          ,         CGST_Percentage        ,             CGST_Amount          ,         SGST_Percentage        ,             SGST_Amount          ,         IGST_Percentage        ,             IGST_Amount          ,             Round_Off          ,      Commission_Percentage      ,             Commission_Amount     ,                      Net_Amount           , Rem_Date   ,                Rem_Time            ,    Total_Bale         , Total_BaleNos          ,         Total_Weight   ,               EWay_Bill_No            , Tcs_Name_caption           ,              Tcs_percentage       ,                    Tcs_Amount    ,                     TCS_Taxable_Value,                            EDIT_TCS_TaxableValue ,             Tcs_Tax_Status,             TCSAmount_RoundOff_Status,                         Invoice_Value_Before_TCS ,                            RoundOff_Invoice_Value_Before_TCS                ,  E_Invoice_IRNO     ,   E_Invoice_QR_Image          ,        Cotton_Sales_SufFixNo       ) " &
                                  "VALUES                       (" & Val(lbl_Company.Tag) & " , '" & Trim(NewCode) & "' , " & Val(lbl_InvNo.Text) & " ,  '" & Trim(UCase(txt_PrefixNo.Text)) & "'  ,    '" & Trim(UCase(vInvNo)) & "' ,  " & Str(Val(vOrderByNo)) & " ,   @EntryDate      , " & Str(Val(Led_ID)) & " , '" & Trim(UCase(txt_Description.Text)) & "' , " & Str(Val(Agt_ID)) & "  ,  " & Str(Val(SlsAc_Id)) & " , " & Val(txt_CommissionPerKG.Text) & " , '" & Trim(UCase(cbo_VehicleNo.Text)) & "' , '" & Trim(cbo_TaxType.Text) & "' , '" & Trim(lbl_HSNCode.Text) & "' , " & Val(lbl_GstPerc.Text) & " , '" & Trim(txt_DateTimeofSupply.Text) & "' , " & Val(LedTo_ID) & " , " & Val(txt_TareWgt.Text) & " , " & Val(lbl_NetWeight.Text) & " , " & Val(txt_Rate.Text) & " , " & Val(lbl_Amount.Text) & " , " & Val(txt_DiscPerc.Text) & " , " & Val(lbl_DiscAmount.Text) & " , " & Val(txt_Freight.Text) & " , " & Val(txt_AddLess.Text) & " , " & Val(lbl_TaxableValue.Text) & " , " & Val(lbl_CGSTPerc.Text) & " , " & Val(lbl_CGSTAmount.Text) & " , " & Val(lbl_SGSTPerc.Text) & " , " & Val(lbl_SGSTAmount.Text) & " , " & Val(lbl_IGSTPerc.Text) & " , " & Val(lbl_IGSTAmount.Text) & " , " & Val(lbl_RoundOff.Text) & " , " & Val(txt_Cmsn_Perc.Text) & " , " & Val(lbl_Cmsn_Amount.Text) & " , " & Str(Val(CSng(lbl_NetAmount.Text))) & ", @Remdate   , '" & (Trim(txt_RemTime.Text)) & "' , " & Val(vTotBleS) & " , " & Val(vTotBleNo) & " ,  " & Val(vTotWght) & " , '" & Trim(txt_EWay_Bill_No.Text) & "', '" & Trim(txt_Tcs_Name.Text) & "',       " & Str(Val(txt_TcsPerc.Text)) & ",    " & Str(Val(lbl_TcsAmount.Text)) & " ,  " & Str(Val(txt_TCS_TaxableValue.Text)) & ", " & Str(Val(vTCS_AssVal_EditSTS)) & ", " & Str(Val(vTCS_Tax_Sts)) & ", " & Str(Val(vTCSAmtRndOff_STS)) & ", " & Str(Val(lbl_Invoice_Value_Before_TCS.Text)) & ", " & Str(Val(lbl_RoundOff_Invoice_Value_Before_TCS.Text)) & " ,      '" & Trim(txt_IR_No.Text) & "' ,     @QrCode ,  '" & Trim(cbo_SufixNo.Text) & "'  ) "
                Cmd.ExecuteNonQuery()
            Else
                Cmd.CommandText = "UPDATE Cotton_Sales_Head SET Cotton_Sales_PreFixNo = '" & Trim(UCase(txt_PrefixNo.Text)) & "', Cotton_Sales_SufFixNo = '" & Trim(cbo_SufixNo.Text) & "' , Cotton_Sales_InvNo ='" & Trim(UCase(vInvNo)) & "', Cotton_Sales_Date = @EntryDate , Ledger_IdNo = " & Str(Val(Led_ID)) & " , Description = '" & Trim(UCase(txt_Description.Text)) & "' , Agent_IdNo = " & Str(Val(Agt_ID)) & " , Sales_AcIdNo = " & Str(Val(SlsAc_Id)) & " , Commission_Kg = " & Val(txt_CommissionPerKG.Text) & " , Vehicle_No = '" & Trim(UCase(cbo_VehicleNo.Text)) & "' , Tax_Type = '" & Trim(cbo_TaxType.Text) & "' , HSN_Code = '" & Trim(lbl_HSNCode.Text) & "' , GST_Percentage = " & Val(lbl_GstPerc.Text) & " , Date_Time_of_Supply = '" & Trim(txt_DateTimeofSupply.Text) & "' , DeliveryTo_IdNo = " & Val(LedTo_ID) & " , Tare_Weight = " & Val(txt_TareWgt.Text) & " , Net_weight = " & Val(lbl_NetWeight.Text) & " , Rate = " & Val(txt_Rate.Text) & " , Amount = " & Val(lbl_Amount.Text) & " , Discount_Percentage = " & Val(txt_DiscPerc.Text) & " , Discount_Amount = " & Val(lbl_DiscAmount.Text) & " , Freight = " & Val(txt_Freight.Text) & " , Add_Less = " & Val(txt_AddLess.Text) & " , Taxable_Value = " & Val(lbl_TaxableValue.Text) & " , CGST_Amount = " & Val(lbl_CGSTAmount.Text) & " , SGST_Amount = " & Val(lbl_SGSTAmount.Text) & " , IGST_Amount = " & Val(lbl_IGSTAmount.Text) & " , Round_Off = " & Val(lbl_RoundOff.Text) & " , Commission_Percentage = " & Val(txt_Cmsn_Perc.Text) & " , " &
                                  "Commission_Amount = " & Val(lbl_Cmsn_Amount.Text) & " , Net_Amount = " & Str(Val(CSng(lbl_NetAmount.Text))) & ", Rem_Date = @Remdate , Rem_Time = '" & Trim(txt_RemTime.Text) & "', Total_Bale = " & Val(vTotBleS) & " , Total_BaleNos = " & Val(vTotBleNo) & " , Total_Weight = " & Val(vTotWght) & " , CGST_Percentage = " & Val(lbl_CGSTPerc.Text) & " , SGST_Percentage = " & Val(lbl_SGSTPerc.Text) & " , IGST_Percentage = " & Val(lbl_IGSTPerc.Text) & " , EWay_Bill_No = '" & Trim(txt_EWay_Bill_No.Text) & "' ,  Tcs_Name_caption = '" & Trim(txt_Tcs_Name.Text) & "', Tcs_percentage=" & Str(Val(txt_TcsPerc.Text)) & ",Tcs_Amount= " & Str(Val(lbl_TcsAmount.Text)) & " , TCS_Taxable_Value = " & Str(Val(txt_TCS_TaxableValue.Text)) & ", EDIT_TCS_TaxableValue = " & Str(Val(vTCS_AssVal_EditSTS)) & " , Tcs_Tax_Status = " & Str(Val(vTCS_Tax_Sts)) & " , TCSAmount_RoundOff_Status = " & Str(Val(vTCSAmtRndOff_STS)) & " , Invoice_Value_Before_TCS = " & Str(Val(lbl_Invoice_Value_Before_TCS.Text)) & ", RoundOff_Invoice_Value_Before_TCS = " & Str(Val(lbl_RoundOff_Invoice_Value_Before_TCS.Text)) & " , E_Invoice_IRNO = '" & Trim(txt_IR_No.Text) & "' , E_Invoice_QR_Image =  @QrCode , E_Invoice_ACK_No = '" & txt_eInvoiceAckNo.Text & "' , E_Invoice_ACK_Date = " & IIf(Trim(vEInvAckDate) <> "", "@EInvoiceAckDate", "Null") & " , " &
                                  " E_Invoice_Cancelled_Status = " & eiCancel.ToString & " ,  E_Invoice_Cancellation_Reason = '" & txt_EInvoiceCancellationReson.Text & "'  ,    EWB_No = '" & txt_EWay_Bill_No.Text & "',EWB_Date = '" & txt_EWB_Date.Text & "',EWB_Valid_Upto = '" & txt_EWB_ValidUpto.Text & "',EWB_Cancelled = " & EWBCancel.ToString & " ,  EWBCancellation_Reason = '" & txt_EWB_Canellation_Reason.Text & "' WHERE Cotton_Sales_code = '" & Trim(NewCode) & "'"
                Cmd.ExecuteNonQuery()
            End If

            Partcls = "Sale : Lot No. " & Trim(lbl_InvNo.Text)

            Cmd.CommandText = "Delete from Stock_Cotton_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "DELETE FROM Cotton_Sales_Details Where Company_IdNo = " & Val(lbl_Company.Tag) & " AND Cotton_Sales_Code = '" & Trim(NewCode) & "'"
            Cmd.ExecuteNonQuery()

            'Vrty_IdNo = Common_Procedures.Variety_NameToIdNo(Con, dgv_Details.Rows(i).Cells(1).Value, tr)

            With dgv_Details


                Sno = 0

                For i = 0 To .Rows.Count - 1

                    Vrty_IdNo = Common_Procedures.Variety_NameToIdNo(Con, dgv_Details.Rows(i).Cells(1).Value, tr)


                    If Trim(.Rows(i).Cells(1).Value) <> "" Then

                        Sno = Sno + 1


                        Cmd.CommandText = "INSERT INTO Cotton_Sales_Details (            Company_IdNo     ,     Cotton_Sales_Code    ,        Cotton_Sales_RefNo   ,   Cotton_Sales_Date ,           for_OrderBy     ,                   Sl_No              ,       Variety_IdNo     ,         Lot_No                       ,                   Bale               ,                     Bale_Nos            ,                   Weight            )" &
                                          "VALUES                           (" & Val(lbl_Company.Tag) & " ,  '" & Trim(NewCode) & "' , '" & Trim(lbl_InvNo.Text) & "' ,     @EntryDate      ,   " & Val(vOrderByNo) & " , " & Val(.Rows(i).Cells(0).Value) & " , " & Val(Vrty_IdNo) & " , " & Val(.Rows(i).Cells(2).Value) & " , " & Val(.Rows(i).Cells(3).Value) & " , '" & Trim(.Rows(i).Cells(4).Value) & "' , " & Val(.Rows(i).Cells(5).Value) & ")"
                        Cmd.ExecuteNonQuery()


                        Cmd.CommandText = "Insert into Stock_Cotton_Processing_Details ( Reference_Code           ,             Company_IdNo         ,           Reference_No        ,                               For_OrderBy                              ,        Reference_Date ,   Entry_ID               ,     Sl_No      , Ledger_Idno        ,        Variety_IdNo       ,  Bale                                          ,         Weight  ) " &
                                                        "    Values  ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvNo.Text))) & ",    @Remdate           ,  '" & Trim(Partcls) & "' ," & Val(Sno) & "," & Val(Led_ID) & " ," & Str(Val(Vrty_IdNo)) & ", " & Str(-1 * Val(.Rows(i).Cells(3).Value)) & " , " & Str(-1 * Val(.Rows(i).Cells(5).Value)) & " )"
                        Cmd.ExecuteNonQuery()

                    End If
                Next

            End With

            If Val(txt_Rate.Text) <> 0 Then
                Bill_Details = Bill_Details & IIf(Bill_Details <> "", ", ", "") & " Rate : " & Trim(txt_Rate.Text)
            End If

            If Val(vTotWegth) <> 0 Then
                Bill_Details = Bill_Details & IIf(Bill_Details <> "", ", ", "") & " netWeight : " & Val(lbl_NetWeight.Text)
            End If


            '---acc posting

            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""

            'If Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            vLed_IdNos = Led_ID & "|" & SlsAc_Id & "|24|25|26|32"

            vVou_Amts = -1 * (Val(CSng(lbl_NetAmount.Text))) & "|" & (Val(CSng(lbl_NetAmount.Text)) - Val(lbl_CGSTAmount.Text) - Val(lbl_SGSTAmount.Text) - Val(lbl_IGSTAmount.Text) - Val(lbl_TcsAmount.Text)) & "|" & Val(lbl_CGSTAmount.Text) & "|" & Val(lbl_SGSTAmount.Text) & "|" & Val(lbl_IGSTAmount.Text) & "|" & Val(lbl_TcsAmount.Text)

            If Common_Procedures.Voucher_Updation(Con, "GST.Cotton.Sales", Val(lbl_Company.Tag), Trim(NewCode), Trim(UCase(txt_PrefixNo.Text)) & "- " & Trim(lbl_InvNo.Text), Convert.ToDateTime(dtp_Date.Text), Trim(Bill_Details), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                Throw New ApplicationException(ErrMsg)
            End If
            'End If

            Common_Procedures.Voucher_Deletion(Con, Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), tr)

            vVou_BlAmt = Val(CSng(lbl_NetAmount.Text))

            VouBil = Common_Procedures.VoucherBill_Posting(Con, Val(lbl_Company.Tag), dtp_Date.Text, Led_ID, Trim(UCase(txt_PrefixNo.Text)) & "- " & Trim(lbl_InvNo.Text), Agt_ID, Val(vVou_BlAmt), "DR", Trim(Pk_Condition) & Trim(NewCode), tr)
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

            If New_Entry = False Then
                move_record(lbl_InvNo.Text)
            Else
                new_record()
            End If

            MessageBox.Show("Saved Successfully", "FOR SAVE,.......", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)



        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try

        Cmd.Dispose()
        tr.Dispose()

        If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            On Error Resume Next

            dgv1 = Nothing

            If ActiveControl.Name = dgv_Details.Name Then
                dgv1 = dgv_Details

            ElseIf dgv_ActCtrlName = dgv_Details.Name Then
                dgv1 = dgv_Details

            ElseIf pnl_Back.Enabled = True Then
                dgv1 = dgv_Details

            End If

            If IsNothing(dgv1) = False Then

                With dgv1

                    If keyData = Keys.Enter Or keyData = Keys.Down Then

                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                txt_TareWgt.Focus()
                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If



                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                                txt_TareWgt.Focus()
                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                If cbo_DeliveryTo.Visible Then cbo_DeliveryTo.Focus() Else txt_DateTimeofSupply.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.Columns.Count - 1)

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

    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyCode

        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            cbo_PartyName.Focus()
        End If
        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            txt_PrefixNo.Focus()
        End If

        vmskOldText = ""
        vmsSelStrt = -1

        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_Date.Text
            vmsSelStrt = msk_Date.SelectionStart
        End If
    End Sub

    Private Sub msk_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_Date.KeyPress
        If Trim(UCase(Asc(e.KeyChar))) = "D" Then
            msk_Date.Text = Date.Today
            msk_Date.SelectionStart = 0
        End If

        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            cbo_PartyName.Focus()
        End If
    End Sub

    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyUp
        Dim vmdRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        If e.KeyCode = 107 Then
            e.Handled = True
            msk_Date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_Date.Text))
            msk_Date.SelectionStart = 0
        End If
        If e.KeyCode = 109 Then
            e.Handled = True
            msk_Date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_Date.Text))
            msk_Date.SelectionStart = 0
        End If

        If e.KeyCode = 106 Or e.KeyCode = 8 Then
            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmsSelStrt)
        End If
    End Sub

    Private Sub msk_Date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_Date.LostFocus
        If IsDate(msk_Date.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_Date.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_Date.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Date.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Date.Text)) <= 2000 Then
                    dtp_Date.Value = Convert.ToDateTime(msk_Date.Text)
                End If
            End If
        End If
    End Sub

    Private Sub cbo_Agent_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Agent.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Agent_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Agent, txt_IR_No, cbo_SalesAc, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Agent_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Agent.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Agent, cbo_SalesAc, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_DelieveryTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DeliveryTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "Ledger_AlaisHead", "Ledger_DisplayName", " ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_DelieveryTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyDown
        vcbo_KeyDwnVal = e.KeyCode
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_DeliveryTo, txt_DateTimeofSupply, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", " ", "(Ledger_idno = 0)")
        If e.KeyCode = 40 And cbo_DeliveryTo.DroppedDown = False Or (e.Control = True And e.KeyCode = 40) Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            Else
                txt_TareWgt.Focus()
            End If
        End If
        'If e.KeyCode = 38 And cbo_DelieveryTo.DroppedDown = False And (e.Control = False And e.KeyCode = 40) Then
        '    If txt_DateTimeofSupply.Visible = True Then
        '        txt_DateTimeofSupply.Focus()
        '    End If
        'End If
    End Sub

    Private Sub cbo_DelieveryTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DeliveryTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_DeliveryTo, txt_DateTimeofSupply, "Ledger_AlaisHead", "Ledger_DisplayName", " ", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 And cbo_DeliveryTo.DroppedDown = False Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            Else
                txt_TareWgt.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_DelieveryTo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyUp
        If e.Control = False And e.KeyCode = 17 Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_DeliveryTo.Name
            Common_Procedures.Master_Return.Master_Type = ""
            Common_Procedures.Master_Return.Return_Value = ""

            f.MdiParent = MdiParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_PartyName, msk_Date, txt_Description, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_PartyName, txt_Description, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            get_Ledger_TotalSales()
        End If
    End Sub

    Private Sub cbo_PartyName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyUp
        If e.Control = False And e.KeyCode = 17 Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PartyName.Name
            Common_Procedures.Master_Return.Master_Type = ""
            Common_Procedures.Master_Return.Return_Value = ""

            f.MdiParent = MdiParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_SalesAc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SalesAc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "Ledger_Head", "Ledger_Name", "(AccountsGroup_IdNo = '28')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_SalesAc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SalesAc.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_SalesAc, cbo_Agent, txt_CommissionPerKG, "Ledger_Head", "Ledger_Name", "(AccountsGroup_IdNo = '28')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_SalesAc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_SalesAc.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_SalesAc, txt_CommissionPerKG, "Ledger_Head", "Ledger_Name", "(AccountsGroup_IdNo = '28')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_SalesAc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SalesAc.KeyUp
        If e.Control = False And e.KeyCode = 17 Then

            Dim f As New Ledger_Creation
            'Dim f As New Accounts_Group_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_SalesAc.Name
            Common_Procedures.Master_Return.Master_Type = ""
            Common_Procedures.Master_Return.Return_Value = ""

            f.MdiParent = MdiParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_TaxType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "", "", "", "")
    End Sub

    Private Sub cbo_TaxType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TaxType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_TaxType, txt_CommissionPerKG, cbo_VehicleNo, "", "", "", "")
    End Sub

    Private Sub cbo_TaxType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TaxType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_TaxType, cbo_VehicleNo, "", "", "", "", True)
    End Sub

    Private Sub cbo_VehicleNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VehicleNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "Cotton_Sales_Head", "Vehicle_No", "", "")
    End Sub

    Private Sub cbo_VehicleNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VehicleNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_VehicleNo, cbo_TaxType, txt_DateTimeofSupply, "Cotton_Sales_Head", "Vehicle_No", "", "")
    End Sub

    Private Sub cbo_VehicleNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VehicleNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_VehicleNo, txt_DateTimeofSupply, "Cotton_Sales_Head", "Vehicle_No", "", "", False)
    End Sub

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown
        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_Date.Focus()
        End If
        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_Date.Focus()
        End If
    End Sub

    Private Sub dtp_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            msk_Date.Focus()
        End If
    End Sub

    Private Sub dtp_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyUp
        If e.Control = False And e.KeyCode = 17 And vcbo_KeyDwnVal = e.KeyCode Then
            dtp_Date.Text = Date.Today
        End If
    End Sub

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        If IsDate(dtp_Date.Text) = True Then
            msk_Date.Text = dtp_Date.Text
            msk_Date.SelectionStart = 0
        End If
        Net_Amount_Calculation()

    End Sub

    Private Sub cbo_Agent_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.MDI_LedType = "AGENT"
            Dim f As New Agent_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Agent.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MdiParent1
            f.Show()
        End If
    End Sub

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        dgv_Details_CellLeave(sender, e)
    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim rect As Rectangle


        With dgv_Details
            dgv_ActCtrlName = .Name
            'If Val(.Rows(e.RowIndex).Cells(0).Value) = 0 Then
            '    .Rows(e.RowIndex).Cells(0).Value = e.RowIndex + 1
            'End If

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If e.ColumnIndex = 1 Then

                If cbo_Variety_Name.Visible = False Or Val(cbo_Variety_Name.Tag) <> e.RowIndex Then

                    cbo_Variety_Name.Tag = -1
                    da = New SqlClient.SqlDataAdapter("select Variety_Name from Variety_Head WHERE variety_type <> 'WASTE'  order by Variety_Name", Con)
                    dt1 = New DataTable
                    da.Fill(dt1)
                    cbo_Variety_Name.DataSource = dt1
                    cbo_Variety_Name.DisplayMember = "Variety_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Variety_Name.Left = .Left + rect.Left
                    cbo_Variety_Name.Top = .Top + rect.Top

                    cbo_Variety_Name.Width = rect.Width
                    cbo_Variety_Name.Height = rect.Height
                    cbo_Variety_Name.Text = .CurrentCell.Value

                    cbo_Variety_Name.Tag = Val(e.RowIndex)
                    cbo_Variety_Name.Visible = True

                    cbo_Variety_Name.BringToFront()
                    cbo_Variety_Name.Focus()

                End If
            Else
                cbo_Variety_Name.Visible = False
            End If


        End With
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details
            If .CurrentCell.ColumnIndex = 5 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        On Error Resume Next

        If Not IsNothing(dgv_Details.CurrentCell) Then
            With dgv_Details
                If .Visible = True Then
                    If .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
                        'If .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then

                        Total_Calculation()
                    End If
                End If
            End With
        End If

    End Sub

    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotBleNo As Single
        Dim TotWgt As Single
        Dim TotBales As Single


        If NoCalc_Status = True Then Exit Sub

        Sno = 0

        TotBleNo = 0 : TotWgt = 0 : TotBales = 0

        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Trim(.Rows(i).Cells(1).Value) <> "" Or Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(5).Value) <> 0 Then

                    TotBleNo = TotBleNo + 1
                    TotBales = TotBales + Val(.Rows(i).Cells(3).Value)
                    TotWgt = TotWgt + Val(.Rows(i).Cells(5).Value)


                End If

            Next

        End With

        lbl_NetWeight.Text = Format(Val(TotWgt) - Val(txt_TareWgt.Text), "########0.000")
        lbl_Amount.Text = Format(Val(lbl_NetWeight.Text) * Val(txt_Rate.Text), "########0.00")

        With dgv_TotalDetails
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(3).Value = Val(TotBales)
            .Rows(0).Cells(4).Value = Val(TotBleNo)
            .Rows(0).Cells(5).Value = Format(Val(TotWgt), "########0.000")
        End With

        ' Agent_Commission_Calculation()

        Net_Amount_Calculation()

    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxtdetails = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        With dgv_Details
            If e.KeyCode = Keys.Up Then
                If .CurrentCell.ColumnIndex <= 1 Then
                    If .CurrentCell.RowIndex = 0 Then
                        cbo_DeliveryTo.Focus()
                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)
                    End If
                End If
            End If

            If e.KeyCode = Keys.Right Then
                If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                    If .CurrentCell.RowIndex >= .Rows.Count - 1 Then
                        txt_TareWgt.Focus()
                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                    End If
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim i As Integer
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            With dgv_Details

                n = .CurrentRow.Index

                If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                    For i = 0 To .Columns.Count - 1
                        .Rows(n).Cells(i).Value = ""
                    Next
                Else
                    .Rows.RemoveAt(n)
                End If
                Total_Calculation()
            End With
        End If
    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgtxtdetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxtdetails.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgtxtdetails.SelectAll()
    End Sub
    Private Sub dgtxtdetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxtdetails.KeyPress
        Try
            With dgv_Details
                If .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 5 Then
                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If
                End If
            End With
        Catch ex As Exception

        End Try
    End Sub
    Private Sub dgtxtdetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxtdetails.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If
    End Sub

    Private Sub txt_TareWgt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TareWgt.KeyDown
        If e.KeyCode = 38 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                dgv_Details.CurrentCell.Selected = True
            Else
                cbo_DeliveryTo.Focus()
            End If
        End If
        If e.KeyCode = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_RemTime_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_RemTime.KeyDown
        If e.KeyCode = 38 Then e.Handled = True : SendKeys.Send("+{tab}")
        If e.KeyCode = 40 Then
            e.Handled = True
            If MessageBox.Show("Do you want to Save?", "FOR SAVE", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                Exit Sub
            End If
        End If
    End Sub

    Private Sub txt_RemTime_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_RemTime.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            If MessageBox.Show("Do you want to Save?", "FOR SAVE", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                Exit Sub
            End If
        End If

    End Sub

    Private Sub btn_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
    End Sub

    Private Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer = 0

        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Private Sub cbo_Variety_Name_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Variety_Name.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "Variety_Head", "Variety_Name", "(variety_type <> 'WASTE')", "(Variety_IdNo = 0)")
    End Sub

    Private Sub cbo_Variety_Name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Variety_Name.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Variety_Name, Nothing, Nothing, "Variety_Head", "Variety_Name", "(variety_type <> 'WASTE')", "(Variety_IdNo = 0)")
        With dgv_Details

            If (e.KeyValue = 38 And cbo_Variety_Name.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Variety_Name.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                If .CurrentRow.Index = .Rows.Count - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then

                    txt_TareWgt.Focus()
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    .CurrentCell.Selected = True
                End If
            End If
        End With
    End Sub

    Private Sub cbo_Variety_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Variety_Name.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Variety_Name, Nothing, "Variety_Head", "Variety_Name", "(variety_type <> 'WASTE')", "(Variety_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            With dgv_Details
                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    txt_TareWgt.Focus()
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(2)
                End If
            End With
        End If
    End Sub

    Private Sub cbo_Variety_Name_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Variety_Name.KeyUp
        If e.Control = False And e.KeyCode = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Variety_Creation("")

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Variety_Name.Name
            Common_Procedures.Master_Return.Master_Type = ""
            Common_Procedures.Master_Return.Return_Value = ""

            f.MdiParent = MdiParent1
            f.Show()
        End If
    End Sub

    Private Sub Get_State_Code(ByVal Ledger_IDno As Integer, ByRef Ledger_State_Code As String, ByRef Company_State_Code As String)

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        'Try

        da = New SqlClient.SqlDataAdapter("Select * from Ledger_Head a LEFT OUTER JOIN State_Head b ON a.Ledger_State_IdNo = b.State_IdNo where a.Ledger_IdNo = " & Str(Val(Ledger_IDno)), Con)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0).Item("State_Code").ToString) = False Then
                Ledger_State_Code = Trim(dt.Rows(0).Item("State_Code").ToString)
            End If

        End If
        dt.Clear()
        dt.Dispose()
        da.Dispose()

        da = New SqlClient.SqlDataAdapter("Select * from Company_Head a LEFT OUTER JOIN State_Head b ON a.Company_State_IdNo = b.State_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)), Con)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0).Item("State_Code").ToString) = False Then
                Company_State_Code = Trim(dt.Rows(0).Item("State_Code").ToString)
            End If
        End If
        dt.Clear()
        dt.Dispose()
        da.Dispose()

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        'Finally
        '    dt.Dispose()
        '    da.Dispose()

        'End Try
    End Sub

    Private Sub cbo_TaxType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TaxType.TextChanged
        Net_Amount_Calculation()
    End Sub

    Private Sub Net_Amount_Calculation()
        Dim NtAmt As Single = 0
        Dim cgst As Single = 0
        Dim sgst As Single = 0
        Dim igst As Single = 0
        Dim AsblAmt As Single = 0
        Dim State_Code As String = ""
        Dim Company_State_Code As String = ""
        Dim Led_IdNo As Integer = 0
        Dim Tax_Amt As Double = 0

        Dim vTCS_AssVal As String = 0
        Dim vTOT_SalAmt As String = 0
        Dim vTCS_Amt As String = 0
        Dim vInvAmt_Bfr_TCS As String = 0
        If NoCalc_Status = True Then Exit Sub

        lbl_Amount.Text = Format(Val(lbl_NetWeight.Text) * Val(txt_Rate.Text), "########0.00")

        lbl_DiscAmount.Text = Format(Val(lbl_Amount.Text) * Val(txt_DiscPerc.Text) / 100, "########0.00")

        lbl_TaxableValue.Text = Format(Val(lbl_Amount.Text) - Val(lbl_DiscAmount.Text) + Val(txt_AddLess.Text) + Val(txt_Freight.Text), "########0.00")

        lbl_Cmsn_Amount.Text = Format(Val((lbl_TaxableValue.Text) * Val(txt_Cmsn_Perc.Text) / 100), "########0.00")

        lbl_CGSTAmount.Text = 0
        lbl_SGSTAmount.Text = 0
        lbl_IGSTAmount.Text = 0
        lbl_CGSTPerc.Text = 0
        lbl_SGSTPerc.Text = 0
        lbl_IGSTPerc.Text = 0

        If Trim(cbo_TaxType.Text) = "GST" Then
            lbl_HSNCode.Enabled = True
            lbl_GstPerc.Enabled = True
            Led_IdNo = Val(Common_Procedures.get_FieldValue(Con, "Ledger_Head", "Ledger_IdNo", "Ledger_Name ='" & Trim(cbo_PartyName.Text) & "'"))
            Get_State_Code(Led_IdNo, State_Code, Company_State_Code)

            lbl_HSNCode.Text = Common_Procedures.get_FieldValue(Con, "Variety_Head", "HSN_Code", "Variety_Name = '" & Trim(dgv_Details.Rows(0).Cells(1).Value) & "'")

            lbl_GstPerc.Text = Val(Common_Procedures.get_FieldValue(Con, "Variety_Head", "GST_Percentege", "Variety_Name = '" & Trim(dgv_Details.Rows(0).Cells(1).Value) & "'"))


            If Trim(Company_State_Code) = Trim(State_Code) Then

                '------CGST Calculation--------
                lbl_CGSTPerc.Text = Format(Val(lbl_GstPerc.Text) / 2, "##########0.00")
                lbl_CGSTAmount.Text = Format(Val(lbl_TaxableValue.Text) * (Val(lbl_GstPerc.Text) / 2) / 100, "########0.00")

                '------SGST Calculation--------
                lbl_SGSTPerc.Text = Format(Val(Val(lbl_GstPerc.Text) / 2), "#########0.00")
                lbl_SGSTAmount.Text = Format(Val(lbl_TaxableValue.Text) * (Val(lbl_GstPerc.Text) / 2) / 100, "########0.00")
            ElseIf Trim(Company_State_Code) <> Trim(State_Code) Then

                '------IGST Calculation--------
                lbl_IGSTPerc.Text = Format(Val(lbl_GstPerc.Text), "##########0.00")
                lbl_IGSTAmount.Text = Format(Val(lbl_TaxableValue.Text) * Val(lbl_GstPerc.Text) / 100, "########0.00")

            End If
        Else

            lbl_HSNCode.Enabled = False
            lbl_GstPerc.Enabled = False
            lbl_CGSTAmount.Text = 0
            lbl_SGSTAmount.Text = 0
            lbl_IGSTAmount.Text = 0
            lbl_CGSTPerc.Text = 0
            lbl_SGSTPerc.Text = 0
            lbl_IGSTPerc.Text = 0
        End If

        Tax_Amt = Val(lbl_CGSTAmount.Text) + Val(lbl_SGSTAmount.Text) + Val(lbl_IGSTAmount.Text)
        Dim vTCS_StartDate As Date = #9/30/2020#
        Dim vMIN_TCS_assval As String = "5000000"

        If chk_TCS_Tax.Checked = True Then

            If DateDiff("d", vTCS_StartDate.Date, dtp_Date.Value.Date) > 0 Then

                If txt_TCS_TaxableValue.Enabled = False Then

                    vTOT_SalAmt = Format(Val(lbl_TaxableValue.Text) + Val(Tax_Amt), "###########0")

                    vTCS_AssVal = 0

                    If Val(CDbl(lbl_TotalSales_Amount_Previous_Year.Text)) > Val(vMIN_TCS_assval) Then

                        vTCS_AssVal = Format(Val(vTOT_SalAmt), "############0")

                    ElseIf Val(CDbl(lbl_TotalSales_Amount_Current_Year.Text)) > Val(vMIN_TCS_assval) Then
                        vTCS_AssVal = Format(Val(vTOT_SalAmt), "############0")

                    ElseIf (Val(CDbl(lbl_TotalSales_Amount_Current_Year.Text)) + Val(vTOT_SalAmt)) > Val(vMIN_TCS_assval) Then
                        vTCS_AssVal = Format(Val(CDbl(lbl_TotalSales_Amount_Current_Year.Text)) + Val(vTOT_SalAmt) - Val(vMIN_TCS_assval), "############0")

                    End If
                    txt_TCS_TaxableValue.Text = Format(Val(vTCS_AssVal), "############0.00")

                    If Val(txt_TCS_TaxableValue.Text) > 0 Then
                        If Val(txt_TcsPerc.Text) = 0 Then
                            txt_TcsPerc.Text = "0.075"
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

        vInvAmt_Bfr_TCS = Format(Val(lbl_TaxableValue.Text) + Val(Tax_Amt), "###########0.00")
        lbl_Invoice_Value_Before_TCS.Text = Format(Val(vInvAmt_Bfr_TCS), "###########0")
        lbl_RoundOff_Invoice_Value_Before_TCS.Text = Format(Val(lbl_Invoice_Value_Before_TCS.Text) - Val(vInvAmt_Bfr_TCS), "###########0.00")



        NtAmt = Val(lbl_TaxableValue.Text) + Val(lbl_CGSTAmount.Text) + Val(lbl_SGSTAmount.Text) + Val(lbl_IGSTAmount.Text) + Val(lbl_Cmsn_Amount.Text) + Val(lbl_TcsAmount.Text)

        lbl_NetAmount.Text = Format(Val(NtAmt), "##########0")
        lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(lbl_NetAmount.Text))

        lbl_RoundOff.Text = Format(Val(CSng(lbl_NetAmount.Text)) - Val(NtAmt), "##########0.00")

        If Val(lbl_RoundOff.Text) = 0 Then lbl_RoundOff.Text = ""


        lbl_AmountInWords.Text = "Rupees  :  "
        If Val(CSng(lbl_NetAmount.Text)) <> 0 Then
            lbl_AmountInWords.Text = "Rupees  :  " & Common_Procedures.Rupees_Converstion(Val(CSng(lbl_NetAmount.Text)))
        End If
    End Sub

    Private Sub dgv_Filter_Details_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellDoubleClick
        Open_Filter_Entry()
    End Sub

    Private Sub cbo_Variety_Name_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Variety_Name.TextChanged
        Try
            If cbo_Variety_Name.Visible Then
                With dgv_Details
                    If Val(cbo_Variety_Name.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Variety_Name.Text)
                    End If
                End With
            End If
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Net_Amount_Calculation()
    End Sub

    Private Sub txt_Cmsn_Perc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Cmsn_Perc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Cmsn_Perc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Cmsn_Perc.TextChanged
        Net_Amount_Calculation()
    End Sub

    Private Sub cbo_PartyName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.SelectedIndexChanged
        Net_Amount_Calculation()
    End Sub

    Private Sub txt_AddLess_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AddLess.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_AddLess_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_AddLess.TextChanged
        Net_Amount_Calculation()
    End Sub

    Private Sub txt_DiscPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DiscPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_DiscPerc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_DiscPerc.TextChanged
        Net_Amount_Calculation()
    End Sub

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Freight_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Freight.TextChanged
        Net_Amount_Calculation()
    End Sub

    Private Sub txt_TareWgt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TareWgt.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_TareWgt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_TareWgt.TextChanged
        Total_Calculation()
    End Sub

    Private Sub txt_Rate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Rate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Rate.TextChanged
        Net_Amount_Calculation()
    End Sub

    Private Sub txt_CommissionPerKG_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CommissionPerKG.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
        If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim Led_IdNo As Integer, Agt_IdNo As Integer, Vty_IdNo As String = ""
        Dim Condt As String = ""

        Try

            Condt = ""
            Agt_IdNo = 0
            Vty_IdNo = 0
            Led_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Cotton_Sales_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Cotton_Sales_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Cotton_Sales_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(Con, cbo_Filter_PartyName.Text)
            End If
            If Trim(cbo_Filter_Variety.Text) <> "" Then
                Vty_IdNo = Common_Procedures.Variety_NameToIdNo(Con, cbo_Filter_Variety.Text)
            End If
            If Trim(cbo_Filter_Agent.Text) <> "" Then
                Agt_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(Con, cbo_Filter_Agent.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Ledger_IdNo = " & Str(Val(Led_IdNo)) & " "
            End If

            If Val(Vty_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Cotton_Sales_Code IN (select ts1.Cotton_Sales_Code from Cotton_Sales_Details ts1 Where ts1.Variety_IdNo = " & Str(Val(Vty_IdNo)) & " and ts1.Cotton_Sales_Code = a.Cotton_Sales_Code ))"
            End If
            If Val(Agt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Agent_IdNo = " & Str(Val(Agt_IdNo)) & " "
            End If


            da = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Ledger_Name as PartyName, d.Ledger_Name as Agent_Name from Cotton_Sales_Head a INNER JOIN Cotton_Sales_Details b ON a.Cotton_Sales_Code = b.Cotton_Sales_Code  INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo LEFT OUTER JOIN Ledger_Head d ON a.Agent_Idno = d.Ledger_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cotton_Sales_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Cotton_Sales_RefNo", Con)

            dt2 = New DataTable
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    'dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Cotton_Sales_RefNo").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Cotton_Sales_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("PartyName").ToString
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Agent_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Total_BaleNos").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Total_Weight").ToString), "########0.000")
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Net_Weight").ToString), "########0.000")
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Net_Amount").ToString), "########0.00")

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
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "Ledger_AlaisHead", "Ledger_DisplayName", " ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_Agent, "Ledger_AlaisHead", "Ledger_DisplayName", " ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Filter_PartyName, cbo_Filter_Agent, "Ledger_AlaisHead", "Ledger_DisplayName", " ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_Variety_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Variety.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "Variety_Head", "Variety_Name", "(variety_type <> 'WASTE')", "(Variety_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_Variety_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Variety.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Filter_Variety, cbo_Filter_Agent, Nothing, "Variety_Head", "Variety_Name", "(variety_type <> 'WASTE')", "(Variety_IdNo = 0)")
        If e.KeyCode = 40 And cbo_Filter_Variety.DroppedDown = False And (e.Control = True And e.KeyCode = 40) Then
            btn_Filter_Show_Click(sender, e)
        End If
    End Sub

    Private Sub cbo_Filter_Variety_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Variety.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Filter_Variety, Nothing, "Variety_Head", "Variety_Name", "(variety_type <> 'WASTE')", "(Variety_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            btn_Filter_Show_Click(sender, e)
        End If
    End Sub

    Private Sub cbo_Filter_Agent_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Agent.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_Agent_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Agent.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Filter_Agent, cbo_Filter_PartyName, cbo_Filter_Variety, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_Agent_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Agent.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Filter_Agent, cbo_Filter_Variety, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub dgv_Filter_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgv_Filter_Details.KeyPress
        If Asc(e.KeyChar) = 13 Then
            Open_Filter_Entry()
        End If
    End Sub

    Private Sub Printing_GST_Format9(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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

        ClArr(1) = 30 : ClArr(2) = 100 : ClArr(3) = 130 : ClArr(4) = 90 : ClArr(5) = 60 : ClArr(6) = 60 : ClArr(7) = 70 : ClArr(8) = 100
        ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

        'ClArr(1) = 30 : ClArr(2) = 100 : ClArr(3) = 200 : ClArr(4) = 75 : ClArr(5) = 50 : ClArr(6) = 50 : ClArr(7) = 75 : ClArr(8) = 75
        'ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

        TxtHgt = e.Graphics.MeasureString("A", pFont).Height
        TxtHgt = 18.6 ' 18.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_GST_Format9_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


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

                            Printing_GST_Format9_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1


                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Variety_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 35 Then
                            For I = 35 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 35
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

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
                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, LMargin + ClArr(1) + 5, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + 5, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 5, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("HSN_Code").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("bale").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Bale_Nos").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)


                        'Wgt_Bag = "0"
                        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1256" Then '---- SOUTHERN COT SPINNERS
                        '    If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString) <> 0 Then
                        '        Wgt_Bag = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString) / Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), "#########0.000")
                        '    End If
                        'End If
                        'If Val(Wgt_Bag) <> 0 Then
                        '    Common_Procedures.Print_To_PrintDocument(e, Val(Wgt_Bag), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)
                        'End If

                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Rate").ToString), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("AMOUNT").ToString), "#########0.00"), PageWidth - 5, CurY, 1, 0, pFont)

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

                Printing_GST_Format9_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True)

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

    Private Sub Printing_GST_Format9_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
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
        If Trim(prn_HdDt.Rows(0).Item("Company_logo_Image").ToString) <> "" Then

            If IsDBNull(prn_HdDt.Rows(0).Item("Company_logo_Image")) = False Then

                Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("Company_logo_Image"), Byte())
                If Not imageData Is Nothing Then
                    Using ms As New MemoryStream(imageData, 0, imageData.Length)
                        ms.Write(imageData, 0, imageData.Length)

                        If imageData.Length > 0 Then

                            '.BackgroundImage = Image.FromStream(ms)

                            ' e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 108, CurY + 10, 90, 90)
                            e.Graphics.DrawImage(DirectCast(Image.FromStream(ms), Drawing.Image), LMargin + 15, CurY + 10, 110, 110)

                        End If

                    End Using

                End If

            End If

        End If
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

        If Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString) <> "" Then

            If IsDBNull(prn_HdDt.Rows(0).Item("E_Invoice_QR_Image")) = False Then
                Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("E_Invoice_QR_Image"), Byte())
                If Not imageData Is Nothing Then
                    Using ms As New MemoryStream(imageData, 0, imageData.Length)
                        ms.Write(imageData, 0, imageData.Length)

                        If imageData.Length > 0 Then

                            pic_IRN_QRCode_Image_forPrinting.BackgroundImage = Image.FromStream(ms)

                            e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 110, CurY + 35, 80, 80)

                        End If

                    End Using
                End If
            End If

        End If

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


        If Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString) <> "" Then
            ItmNm1 = Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString)

            ItmNm2 = ""
            If Len(ItmNm1) > 35 Then
                For i = 35 To 1 Step -1
                    If Mid$(Trim(ItmNm1), i, 1) = " " Or Mid$(Trim(ItmNm1), i, 1) = "," Or Mid$(Trim(ItmNm1), i, 1) = "." Or Mid$(Trim(ItmNm1), i, 1) = "-" Or Mid$(Trim(ItmNm1), i, 1) = "/" Or Mid$(Trim(ItmNm1), i, 1) = "_" Or Mid$(Trim(ItmNm1), i, 1) = "(" Or Mid$(Trim(ItmNm1), i, 1) = ")" Or Mid$(Trim(ItmNm1), i, 1) = "\" Or Mid$(Trim(ItmNm1), i, 1) = "[" Or Mid$(Trim(ItmNm1), i, 1) = "]" Or Mid$(Trim(ItmNm1), i, 1) = "{" Or Mid$(Trim(ItmNm1), i, 1) = "}" Then Exit For
                Next i
                If i = 0 Then i = 35

                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - i)
                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), i - 1)
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

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Y1 = CurY + 0.5
        Y2 = CurY + TxtHgt - 10 + TxtHgt + 5
        Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)



        vHeading = "TAX INVOICE"

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
            ' Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Yarn_Sales_No").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, p1Font)
            If prn_HdDt.Rows(0).Item("Cotton_Sales_PreFixNo").ToString <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cotton_Sales_PreFixNo").ToString & "-" & prn_HdDt.Rows(0).Item("Cotton_Sales_RefNo").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, p1Font)
            Else
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cotton_Sales_RefNo").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, p1Font)
            End If

            'Common_Procedures.Print_To_PrintDocument(e, "Transport Mode", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Mode").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "E-Way Bill No.", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("EWay_Bill_No").ToString), LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Cotton_Sales_Date").ToString), "dd-MM-yyyy").ToString, LMargin + W1 + 30, CurY1, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Vehicle Number", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt



            'Common_Procedures.Print_To_PrintDocument(e, "PO No", LMargin + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)
            'If Trim(prn_HdDt.Rows(0).Item("Order_Date").ToString) <> "" Then
            '    strWidth = e.Graphics.MeasureString("     " & prn_HdDt.Rows(0).Item("Order_No").ToString, pFont).Width
            '    Common_Procedures.Print_To_PrintDocument(e, "  Date : " & prn_HdDt.Rows(0).Item("Order_Date").ToString, LMargin + W1 + 30 + strWidth, CurY1 - 3, 0, 0, pFont)
            'End If
            ''Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString & "Date : " & prn_HdDt.Rows(0).Item("Order_Date").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "Transport Name", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Transport_IdNo").ToString)), LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            ' CurY1 = CurY1 + TxtHgt
            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1066" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1256" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "Lr No", LMargin + 10, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lr_No").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)
            '    If Trim(prn_HdDt.Rows(0).Item("Lr_Date").ToString) <> "" Then
            '        strWidth = e.Graphics.MeasureString("     " & prn_HdDt.Rows(0).Item("Lr_No").ToString, pFont).Width
            '        Common_Procedures.Print_To_PrintDocument(e, "  Date : " & prn_HdDt.Rows(0).Item("Lr_Date").ToString, LMargin + W1 + 30 + strWidth, CurY1 - 3, 0, 0, pFont)
            '    End If
            'Else
            '    ' CurY1 = CurY1 + TxtHgt
            '    'Common_Procedures.Print_To_PrintDocument(e, "DC No", LMargin + 10, CurY1, 0, 0, pFont)
            '    'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            '    'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)
            '    'If Trim(prn_HdDt.Rows(0).Item("Dc_Date").ToString) <> "" Then
            '    '    strWidth = e.Graphics.MeasureString("     " & prn_HdDt.Rows(0).Item("Dc_No").ToString, pFont).Width
            '    '    Common_Procedures.Print_To_PrintDocument(e, "  Date : " & prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + W1 + 30 + strWidth, CurY1 - 3, 0, 0, pFont)
            '    'End If

            'End If

            Common_Procedures.Print_To_PrintDocument(e, "Date Of Supply", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Date_Time_of_Supply").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)


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


            vLedPanNo = Common_Procedures.get_FieldValue(Con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & ")")

            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Or Trim(vLedPanNo) <> "" Then

                If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)
                End If

                C3 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
                If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + S1 + C3 + 10 + strWidth, CurY, 0, PrintWidth, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C3 + 50 + strWidth, CurY, 0, PrintWidth, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelGSTinNo").ToString, LMargin + S1 + C3 + 80 + strWidth, CurY, 0, PrintWidth, pFont)
                End If

                If Trim(vLedPanNo) <> "" Then
                    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
                    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & vLedPanNo, LMargin + S1 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
                End If

            End If

            If Val(prn_HdDt.Rows(0).Item("DeliveryTo_IdNo").ToString) <> 0 Then
                vDelvPanNo = Common_Procedures.get_FieldValue(Con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("DeliveryTo_IdNo").ToString)) & ")")
            Else
                vDelvPanNo = Common_Procedures.get_FieldValue(Con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & ")")
            End If

            'If Trim(prn_HdDt.Rows(0).Item("DelGSTinNo").ToString) <> "" Or Trim(vDelvPanNo) <> "" Then
            '    If Trim(prn_HdDt.Rows(0).Item("DelGSTinNo").ToString) <> "" Then
            '        Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + C2 + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelGSTinNo").ToString, LMargin + S1 + C2 + 30, CurY, 0, 0, pFont)
            '    End If
            'If Trim(vDelvPanNo) <> "" Then
            '    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
            '    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & vDelvPanNo, LMargin + S1 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
            'End If
            'End If

            'If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)

            '    'If Trim(prn_HdDt.Rows(0).Item("Ledger_PanNo").ToString) <> "" Then
            '    '    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
            '    '    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("Ledger_PanNo").ToString, LMargin + S1 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
            '    'End If

            'End If

            'If Trim(prn_HdDt.Rows(0).Item("DelGSTinNo").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + C2 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelGSTinNo").ToString, LMargin + S1 + C2 + 30, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("DelPanNo").ToString) <> "" Then
                strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
                Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("DelPanNo").ToString, LMargin + S1 + C2 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
            End If
            'End If

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

            If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "State", LMargin + C2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelState_Name").ToString, LMargin + S1 + C2 + 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Code     " & prn_HdDt.Rows(0).Item("Delivery_State_Code").ToString, LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 40, CurY, 0, 0, pFont)
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
            'Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DESCRIPTION OF GOODS", LMargin + ClAr(1), CurY, 2, ClAr(2) + ClAr(3), pFont)


            Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "CODE", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY + 5, 2, ClAr(4), p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "BALES", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY - TxtHgt + 5, 2, ClAr(5), pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, "BALES", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + 5, 2, ClAr(5), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "GST %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)

            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1256" Then '---- SOUTHERN COT SPINNERS
            '    Common_Procedures.Print_To_PrintDocument(e, "BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY - TxtHgt + 5, 2, ClAr(6), pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + 5, 2, ClAr(6), pFont)
            '    'Common_Procedures.Print_To_PrintDocument(e, " BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            'End If

            Common_Procedures.Print_To_PrintDocument(e, "BALE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY - TxtHgt + 5, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + 5, 2, ClAr(6), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + 5, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY - TxtHgt + 5, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
            'p1Font = New Font("Calibri", 9, FontStyle.Regular)
            'Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "PRODUCT DESCRIPTION", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "GST%", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "QUANTITY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "UNIT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "TAXABLE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

            CurY = CurY + TxtHgt + 15
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_GST_Format9_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
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
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Taxable_Value").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 5, CurY, 1, 0, pFont)

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
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), LnAr(5), LMargin + ClAr(1) + ClAr(2), LnAr(4))
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


            'vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)


            'Y1 = CurY + 5 '0.5
            'Y2 = CurY + TxtHgt - 15 + TxtHgt
            ''   Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + C1, Y2)

            'If IsDBNull(dt1.Rows(0).Item("Freight_Name").ToString) = False Then
            '    If Trim(dt1.Rows(0).Item("Freight_Name").ToString) <> "" Then
            '        txt_Freight_Name.Text = dt1.Rows(0).Item("Freight_Name").ToString
            '    End If
            'End If
            'txt_FreightAmount.Text = Format(Val(dt1.Rows(0).Item("Freight_Amount").ToString), "#########0.00")
            'If IsDBNull(dt1.Rows(0).Item("Packing_Name").ToString) = False Then
            '    If Trim(dt1.Rows(0).Item("Packing_Name").ToString) <> "" Then
            '        txt_Packing_Name.Text = dt1.Rows(0).Item("Packing_Name").ToString
            '    End If
            'End If
            'txt_Packing.Text = Format(Val(dt1.Rows(0).Item("Packing_Amount").ToString), "#########0.00")
            'If IsDBNull(dt1.Rows(0).Item("AddLess_Name").ToString) = False Then
            '    If Trim(dt1.Rows(0).Item("AddLess_Name").ToString) <> "" Then
            '        txt_AddLess_Name.Text = dt1.Rows(0).Item("AddLess_Name").ToString
            '    End If
            'End If
            'txt_AddLessAmount.Text = Format(Val(dt1.Rows(0).Item("AddLess_Amount").ToString), "#########0.00")


            '  CurY = CurY + TxtHgt - 15

            'CurY = CurY + TxtHgt
            ' e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            'CurY = CurY + TxtHgt - 15
            'Common_Procedures.Print_To_PrintDocument(e, "BANK NAME  :  " & BankNm1, LMargin + 10, CurY, 0, 0, pFont)

            'If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
            '    If is_LastPage = True Then
            '        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Freight_Name").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            '    End If
            'End If


            'CurY = CurY + TxtHgt
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            'CurY = CurY + TxtHgt - 15
            'Common_Procedures.Print_To_PrintDocument(e, "ACCOUNT No.  :  " & BankNm2, LMargin + 10, CurY, 0, 0, pFont)

            'If Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) <> 0 Then
            '    If is_LastPage = True Then
            '        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Packing_Name").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, Format(Math.Abs(Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString)), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

            '    End If
            'End If


            'CurY = CurY + TxtHgt
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            'LnAr(9) = CurY
            'CurY = CurY + TxtHgt - 15
            'Common_Procedures.Print_To_PrintDocument(e, "BRANCH  :  " & BankNm3, LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "IFSC CODE  :  " & BankNm4, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 80 + 15, CurY, 0, 0, pFont)


            'If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
            '    If is_LastPage = True Then
            '        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("AddLess_Name").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, Format(Math.Abs(Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString)), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            '    End If
            'End If

            'CurY = CurY + TxtHgt
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 80, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 80, LnAr(9))
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)


            If Val(prn_HdDt.Rows(0).Item("Freight").ToString) <> 0 Then

                Common_Procedures.Print_To_PrintDocument(e, "Freight", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                CurY = CurY + TxtHgt
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)


                Y1 = CurY - 18
                Y2 = CurY + TxtHgt - 15 + TxtHgt
                Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + C1, Y2)

                CurY = CurY + TxtHgt - 15
                Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY - 5, 0, 0, pFont)

            Else


                Y1 = CurY
                Y2 = CurY + TxtHgt - 15 + TxtHgt
                Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + C1, Y2)

                CurY = CurY + TxtHgt - 15
                Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, pFont)

            End If


            'CurY = CurY + TxtHgt - 15
            'Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Total  Before Tax  ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Taxable_Value").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt

            CurY1 = CurY
            '***************************************
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY


            CurY = CurY + TxtHgt - 15

            p1Font = New Font("Calibri", 8, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "1. We are responsible for the quality of yarn only;If any running fault or", LMargin + 10, CurY, 0, 0, p1Font)



            If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ " & Format(Val(lbl_CGSTPerc.Text), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)

            If Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ " & Format(Val(lbl_SGSTPerc.Text), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If


            p1Font = New Font("Calibri", 8, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "quality defect noted in yarn please inform with firat fabric roll at once. We will", LMargin + 25, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)
            p1Font = New Font("Calibri", 8, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "accept only one roll at defect otherwise we do not hold ourself responsible.", LMargin + 25, CurY, 0, 0, p1Font)

            If Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ " & Format(Val(lbl_IGSTPerc.Text), "##########0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If


            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 8, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "2. Our responsibility ceases when goods leave our permises.", LMargin + 10, CurY, 0, 0, p1Font)

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)

            If Val(prn_HdDt.Rows(0).Item("Commission_Amount").ToString) <> 0 Then
                'Common_Procedures.Print_To_PrintDocument(e, "Add : " & Trim(prn_HdDt.Rows(0).Item("Cmc_Name").ToString) & "  @ " & Format(Val(prn_HdDt.Rows(0).Item("Cmc_Percentage").ToString), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Add : " & "Cess" & "  @ " & Format(Val(prn_HdDt.Rows(0).Item("Commission_Percentage").ToString), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Commission_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                CurY = CurY + TxtHgt

                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)
            End If


            Common_Procedures.Print_To_PrintDocument(e, "Total  TAX Amount", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 8, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "3. Interest at the rate of 24% will be charge from the due date.", LMargin + 10, CurY, 0, 0, p1Font)
            If Val(prn_HdDt.Rows(0).Item("TCS_AMOUNT").ToString) <> 0 Then

                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)

                Common_Procedures.Print_To_PrintDocument(e, (prn_HdDt.Rows(0).Item("TCs_name_caption").ToString) & "  @ " & (prn_HdDt.Rows(0).Item("Tcs_Percentage").ToString) & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("TCS_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                CurY = CurY + TxtHgt
            End If

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)

            If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "RoundOff ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Round_Off").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If

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
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + TxtHgt + 10, PageWidth, CurY + TxtHgt + 10)
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY + TxtHgt + 10, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))

            CurY = CurY + TxtHgt

            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + TxtHgt + 10, PageWidth, CurY + TxtHgt + 10)
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))

            ''e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 20, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 20, LnAr(10))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            CurY = CurY + 5

            'p1Font = New Font("Calibri", 9, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, "GST on Reverse Charge", LMargin + C1 + 10, CurY + 5, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY + 5, 1, 0, p1Font)
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + TxtHgt + 10, PageWidth, CurY + TxtHgt + 10)
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY + TxtHgt + 10, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))



            If is_LastPage = True Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- Logu textile
                    BmsInWrds = Trim(UCase(BmsInWrds))
                Else
                    BmsInWrds = Trim(StrConv(BmsInWrds, VbStrConv.ProperCase))
                End If

                p1Font = New Font("Calibri", 11, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, " " & BmsInWrds, LMargin + 10, CurY, 0, 0, p1Font)

                'Rup2 = ""
                'Rup1 = BmsInWrds
                'If Len(Rup1) > 60 Then
                '    For M = 60 To 1 Step -1
                '        If Mid$(Trim(Rup1), M, 1) = " " Then Exit For
                '    Next M
                '    If M = 0 Then M = 60
                '    Rup2 = Microsoft.VisualBasic.Right(Trim(Rup1), Len(Rup1) - M)
                '    Rup1 = Microsoft.VisualBasic.Left(Trim(Rup1), M - 1)
                'End If

                'p1Font = New Font("Calibri", 11, FontStyle.Bold)
                'Common_Procedures.Print_To_PrintDocument(e, " " & Rup1, LMargin + 10, CurY, 0, 0, p1Font)
                'If Trim(Rup2) <> "" Then
                '    CurY = CurY + TxtHgt - 2
                '    Common_Procedures.Print_To_PrintDocument(e, " " & Rup2, LMargin + 10, CurY, 0, 0, p1Font)
                '    CurY = CurY - 10
                'End If

            End If



            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY)


            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1176" Then

            '    CurY = CurY + TxtHgt - 5
            '    Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS : " & Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), LMargin + 10, CurY, 0, 0, p1Font)
            '    CurY = CurY + TxtHgt + 10
            '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            'End If


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





            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- Logu textile
                Common_Procedures.Print_To_PrintDocument(e, "Received by", LMargin + 35, CurY, 0, 0, pFont)

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1176" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1256" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1286" Then ' KALPANA COTTON

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1370" Then

                    Common_Procedures.Print_To_PrintDocument(e, "Prepared by", LMargin + ClAr(1) + ClAr(2) + 90, CurY, 0, 0, pFont)

                Else

                    Common_Procedures.Print_To_PrintDocument(e, "Prepared by", LMargin + 35, CurY, 0, 0, pFont)

                End If

            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1176" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1256" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1286" Then ' KALPANA COTTON

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1370" Then

                    Common_Procedures.Print_To_PrintDocument(e, "Checked by", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 80, CurY, 1, 0, pFont)

                Else

                    Common_Procedures.Print_To_PrintDocument(e, "Checked by", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                End If

            End If




            CurY = CurY + TxtHgt + 10
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1176" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1256" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1286" Then

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1370" Then

                    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + 80, CurY, LMargin + ClAr(1) + ClAr(2) + 80, LnAr(14))
                    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 40, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 40, LnAr(14))

                Else

                    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + 15, CurY, LMargin + ClAr(1) + ClAr(2) + 15, LnAr(14))
                    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(14))

                End If

            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1370" Then

                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 30, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 30, LnAr(14))
            Else
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(14))
            End If

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

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
    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String
        Dim W1 As Single = 0

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        prn_Count = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.* , e.Ledger_Name as Agent_Name ,SH.* ,Lsh.State_Name as Ledger_State_Name ,Lsh.State_Code as Ledger_State_Code, f.Ledger_MainName as DelName , f.Ledger_Address1 as DelAdd1 ,f.Ledger_Address2 as DelAdd2, f.Ledger_Address3 as DelAdd3 ,f.Ledger_Address4 as DelAdd4, f.Pan_No DelPanNo, f.Ledger_GSTinNo as DelGSTinNo, DSH.State_Name as DelState_Name, DSH.State_Code as Delivery_State_Code from Cotton_Sales_Head a " &
                                               "  INNER JOIN Company_Head b        ON a.Company_IdNo        = b.Company_IdNo " &
                                               "  INNER JOIN Ledger_Head c         ON a.Ledger_IdNo         = c.Ledger_IdNo " &
                                               "  LEFT OUTER JOIN Ledger_Head e    ON e.Ledger_IdNo         = a.Agent_IdNo " &
                                               "  LEFT OUTER JOIN State_Head Lsh   ON c.Ledger_State_Idno   = Lsh.State_IDno " &
                                               "  LEFT OUTER JOIN Ledger_Head f    ON (case when a.DeliveryTo_IdNo <> 0 then a.DeliveryTo_IdNo else a.Ledger_IdNo end) = f.Ledger_IdNo " &
                                               "  LEFT OUTER JOIN State_HEad DSH   on f.Ledger_State_IdNo = DSH.State_IdNo " &
                                               "  LEFT OUTER JOIN State_Head SH    ON b.Company_State_IdNo  = SH.State_Idno  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cotton_Sales_Code = '" & Trim(NewCode) & "'", Con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.* , b.Variety_Name  from Cotton_Sales_Details a INNER JOIN Variety_Head b ON a.Variety_IdNo = b.Variety_IdNo  where a.Cotton_Sales_Code = '" & Trim(NewCode) & "' Order by a.for_orderby, a.Cotton_Sales_RefNo", Con)
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

    Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1370" Then '-----AKHIL IMPEX
            Printing_GST_Format_1370(e)
        Else
            Printing_GST_Format9(e)
        End If


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

    Private Sub PrintPreview_Shown(ByVal sender As Object, ByVal e As System.EventArgs)
        'Capture the click events for the toolstrip in the dialog box when the dialog is shown
        Dim ts As ToolStrip = CType(sender.Controls(1), ToolStrip)
        AddHandler ts.ItemClicked, AddressOf PrintPreview_Toolstrip_ItemClicked
    End Sub

    Private Sub Update_PrintOut_Status(Optional ByVal sqltr As SqlClient.SqlTransaction = Nothing)
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String = ""
        Dim vPrnSTS As Integer = 0

        Try
            cmd.Connection = Con
            If IsNothing(sqltr) = False Then
                cmd.Transaction = sqltr
            End If

            vPrnSTS = 0
            If chk_Printed.Checked = True Then
                vPrnSTS = 1
            End If

            NewCode = Trim(Pk_Condition) & Trim(lbl_Company.Tag) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.CommandText = "Update Cotton_Sales_Head set PrintOut_Status = " & Str(Val(vPrnSTS)) & " where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cotton_Sales_Code  = '" & Trim(NewCode) & "'"
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




    Private Sub txt_TCS_TaxableValue_TextChanged(sender As System.Object, e As System.EventArgs) Handles txt_TCS_TaxableValue.TextChanged
        Net_Amount_Calculation()
    End Sub

    Private Sub lbl_TotalSales_Amount_Current_Year_TextChanged(sender As Object, e As System.EventArgs) Handles lbl_TotalSales_Amount_Current_Year.TextChanged
        Net_Amount_Calculation()
    End Sub

    Private Sub lbl_TotalSales_Amount_Previous_Year_TextChanged(sender As Object, e As System.EventArgs) Handles lbl_TotalSales_Amount_Previous_Year.TextChanged
        Net_Amount_Calculation()
    End Sub

    Private Sub btn_EDIT_TCS_TaxableValue_Click(sender As System.Object, e As System.EventArgs) Handles btn_EDIT_TCS_TaxableValue.Click
        txt_TCS_TaxableValue.Enabled = Not txt_TCS_TaxableValue.Enabled
        txt_TcsPerc.Enabled = Not txt_TcsPerc.Enabled

        If txt_TCS_TaxableValue.Enabled Then
            txt_TCS_TaxableValue.Focus()

        Else
            ' txt_addless.Focus()
            btn_Save.Focus()

        End If
    End Sub

    Private Sub chk_TCS_Tax_CheckedChanged(sender As Object, e As System.EventArgs) Handles chk_TCS_Tax.CheckedChanged
        Net_Amount_Calculation()
    End Sub

    Private Sub chk_TCSAmount_RoundOff_STS_CheckedChanged(sender As Object, e As System.EventArgs) Handles chk_TCSAmount_RoundOff_STS.CheckedChanged
        Net_Amount_Calculation()
    End Sub



    Private Sub txt_TcsPerc_TextChanged(sender As Object, e As System.EventArgs) Handles txt_TcsPerc.TextChanged
        Net_Amount_Calculation()
    End Sub






    Private Sub get_Ledger_TotalSales()
        Dim cmd As New SqlClient.SqlCommand
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim TtSalAmt_CurrYr As String = 0
        Dim TtSalAmt_PrevYr As String = 0
        Dim GpCd As String = ""
        Dim Datcondt As String = ""
        Dim n As Integer = 0
        Dim I As Integer = 0
        Dim Led_ID As Integer = 0
        Dim vPrevYrCode As String = ""
        Dim NewCode As String = ""


        Try


            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            lbl_TotalSales_Amount_Current_Year.Text = "0.00"
            lbl_TotalSales_Amount_Previous_Year.Text = "0.00"
            '-----------TOTAL SALES

            cmd.Connection = Con

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@entrydate", dtp_Date.Value.Date)

            Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(Con, cbo_PartyName.Text)

            If Led_ID <> 0 Then

                cmd.CommandText = "select sum(abs(a.Voucher_amount)) as BalAmount from voucher_details a WHERE a.Voucher_amount < 0 and a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ledger_idno = " & Str(Val(Led_ID)) & " and a.Voucher_date <= @entrydate and a.Voucher_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and a.Voucher_Code NOT LIKE '%" & Trim(NewCode) & "' and (a.Voucher_Code LIKE 'GCINV-%' OR a.Voucher_Code LIKE 'GSSINS-%' OR a.Voucher_Code LIKE 'GYNSL-%'  OR a.Voucher_Code LIKE 'GPVSA-%'  OR a.Voucher_Code LIKE 'GSSAL-%' OR a.Voucher_Code LIKE 'GYPSL-%' OR a.Voucher_Code LIKE 'GSCWS-%' OR a.Voucher_Code LIKE 'GCOSE-%') "
                'cmd.CommandText = "select sum(abs(a.Voucher_amount)) as BalAmount from voucher_details a WHERE a.Voucher_amount < 0 and a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ledger_idno = " & Str(Val(Led_ID)) & " and a.Voucher_date <= @entrydate and a.Voucher_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and a.Voucher_Code NOT LIKE '%" & Trim(NewCode) & "' and (a.Voucher_Code LIKE 'GCINV-%' OR a.Voucher_Code LIKE 'GSSINS-%' OR a.Voucher_Code LIKE 'GYNSL-%'  OR a.Voucher_Code LIKE 'GPVSA-%'  OR a.Voucher_Code LIKE 'GCOSE-%') "
                da = New SqlClient.SqlDataAdapter(cmd)
                dt1 = New DataTable
                da.Fill(dt1)

                TtSalAmt_CurrYr = 0
                If dt1.Rows.Count > 0 Then
                    If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                        TtSalAmt_CurrYr = Val(dt1.Rows(0).Item("BalAmount").ToString)
                    End If
                End If
                dt1.Clear()


                vPrevYrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnYearCode), 2)
                vPrevYrCode = Trim(Format(Val(vPrevYrCode) - 1, "00")) & "-" & Trim(Format(Val(vPrevYrCode), "00"))

                cmd.CommandText = "select sum(abs(a.Voucher_amount)) as BalAmount from voucher_details a WHERE a.Voucher_amount < 0 and a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ledger_idno = " & Str(Val(Led_ID)) & " and a.Voucher_date <= @entrydate and a.Voucher_Code LIKE '%/" & Trim(vPrevYrCode) & "' and (a.Voucher_Code LIKE 'GCINV-%' OR a.Voucher_Code LIKE 'GSSINS-%' OR a.Voucher_Code LIKE 'GYNSL-%'  OR a.Voucher_Code LIKE 'GPVSA-%'  OR a.Voucher_Code LIKE 'GSSAL-%' OR a.Voucher_Code LIKE 'GYPSL-%' OR a.Voucher_Code LIKE 'GSCWS-%' OR a.Voucher_Code LIKE 'GCOSE-%') "
                da = New SqlClient.SqlDataAdapter(cmd)
                dt1 = New DataTable
                da.Fill(dt1)

                TtSalAmt_PrevYr = 0
                If dt1.Rows.Count > 0 Then
                    If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                        TtSalAmt_PrevYr = Val(dt1.Rows(0).Item("BalAmount").ToString)
                    End If
                End If
                dt1.Clear()

                dt1.Dispose()
                da.Dispose()
                cmd.Dispose()

                lbl_TotalSales_Amount_Current_Year.Text = Trim(Common_Procedures.Currency_Format(Math.Abs(Val(TtSalAmt_CurrYr))))
                lbl_TotalSales_Amount_Previous_Year.Text = Trim(Common_Procedures.Currency_Format(Math.Abs(Val(TtSalAmt_PrevYr))))


            End If


        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE GETTIG TOTAL SALES....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

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

    Private Sub btn_EInvoice_Generation_Click(sender As Object, e As EventArgs) Handles btn_EInvoice_Generation.Click
        'rtbeInvoiceResponse.Text = ""
        'txt_EWBNo.Text = txt_Electronic_RefNo.Text
        grp_EInvoice.Visible = True
        grp_EInvoice.BringToFront()
        grp_EInvoice.Left = (Me.Width - grp_EInvoice.Width) / 2
        grp_EInvoice.Top = (Me.Height - grp_EInvoice.Height) / 2
    End Sub

    Private Sub btn_CheckConnectivity1_Click(sender As Object, e As EventArgs) Handles btn_CheckConnectivity1.Click

        Dim einv As New eInvoice(Val(lbl_Company.Tag))
        einv.GetAuthToken(rtbeInvoiceResponse)
        'rtbeInvoiceResponse.Text = einv.AuthTokenReturnMsg
    End Sub

    Private Sub btn_Generate_eInvoice_Click(sender As Object, e As EventArgs) Handles btn_Generate_eInvoice.Click

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim Cmd As New SqlClient.SqlCommand
        Cmd.Connection = Con
        Cmd.CommandText = "Select count(*) from Cotton_Sales_Head Where Cotton_Sales_Code = '" & Trim(NewCode) & "'"

        Dim c As Int16 = Cmd.ExecuteScalar

        If c <= 0 Then
            MsgBox("Please Save the Invoice Before Generating IRN ", vbOKOnly, "Save")
            Exit Sub
        End If

        Cmd.CommandText = "Select count(*) from Cotton_Sales_Head Where Cotton_Sales_Code = '" & Trim(NewCode) & "' and Len(E_Invoice_IRNO) >0"
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

        tr = Con.BeginTransaction
        Cmd.Transaction = tr

        Try

            Cmd.CommandText = "Delete from e_Invoice_Head  where Ref_Sales_Code = '" & Trim(NewCode) & "'"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "Delete from e_Invoice_Details  where Ref_Sales_Code = '" & Trim(NewCode) & "'"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "Insert into e_Invoice_Head (       e_Invoice_No  ,                   e_Invoice_date ,     Buyer_IdNo,     Consignee_IdNo,        Assessable_Value  , CGST            , SGST            , IGST     , Cess, State_Cess,     Round_Off    , Nett_Invoice_Value,        Ref_Sales_Code          ,   Other_Charges           )" &
                              "Select               (Cotton_Sales_RefNo + '-' + Cotton_Sales_PreFixNo ) , Cotton_Sales_Date,  Ledger_IdNo,     DeliveryTo_Idno,   Taxable_Value,     CGST_Amount,     SGST_Amount,     IGST_Amount,      Commission_Amount   ,    0          ,Round_Off    ,   Net_Amount          , '" & Trim(NewCode) & "',  0           from Cotton_Sales_Head where Cotton_Sales_Code = '" & Trim(NewCode) & "'"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "Insert into e_Invoice_Details (Sl_No, IsService,           Product_Description                    ,              HSN_Code,   Batch_Details,     Quantity,     Unit,    Unit_Price,     Total_Amount                                         ,     Discount     ,      Assessable_Amount ,     GST_Rate       , SGST_Amount, IGST_Amount, CGST_Amount,  Cess_rate, Cess_Amount, CessNonAdvlAmount, State_Cess_Rate, State_Cess_Amount, StateCessNonAdvlAmount,  Other_Charge,   Total_Item_Value,    AttributesDetails,     Ref_Sales_Code )" &
                                                    " Select a.Sl_No, 0,       (c.Variety_Name + ' ' + b.Description ) as producDescription , b.HSN_Code,     ''          , b.Total_Weight  ,'KGS',   b.Rate     , (b.Taxable_Value - b.Discount_Amount ) as total_amount  , b.Discount_Amount,   b.Taxable_Value      ,    b.GST_Percentage,     0           ,0           ,0           ,0          ,0           ,0                 ,0               ,0                 ,0                  ,   0         , 0 as    Total_Item_Value,       ''               , '" & Trim(NewCode) & "' " &
                                                    " from Cotton_Sales_Details a inner join Cotton_Sales_Head b ON a.Cotton_Sales_Code = b.Cotton_Sales_Code " &
                                                    " inner join Variety_Head C on a.Variety_IdNo = c.Variety_IdNo " &
                                                   " Where b.Cotton_Sales_Code = '" & Trim(NewCode) & "'"
            Cmd.ExecuteNonQuery()


            'Cmd.CommandText = "Insert into e_Invoice_Details (Sl_No, IsService,           Product_Description                    ,              HSN_Code,   Batch_Details,     Quantity,     Unit,    Unit_Price,    Total_Amount ,     Discount     ,      Assessable_Amount ,     GST_Rate       , SGST_Amount, IGST_Amount, CGST_Amount,  Cess_rate, Cess_Amount, CessNonAdvlAmount, State_Cess_Rate, State_Cess_Amount, StateCessNonAdvlAmount,  Other_Charge,   Total_Item_Value,    AttributesDetails,     Ref_Sales_Code )" &
            '                                        " Select a.Sl_No, 0,       (c.Variety_Name + ' ' + b.Description ) as producDescription , b.HSN_Code,     ''          , b.Total_Weight  ,'KGS',   b.Rate     , b.Amount       , b.Discount_Amount,   b.Taxable_Value      ,    b.GST_Percentage,     0           ,0           ,0           ,0          ,0           ,0                 ,0               ,0                 ,0                  ,   0         , 0 as    Total_Item_Value,       ''               , '" & Trim(NewCode) & "' " &
            '                                        " from Cotton_Sales_Details a inner join Cotton_Sales_Head b ON a.Cotton_Sales_Code = b.Cotton_Sales_Code " &
            '                                        " inner join Variety_Head C on a.Variety_IdNo = c.Variety_IdNo " &
            '                                       " Where b.Cotton_Sales_Code = '" & Trim(NewCode) & "'"
            'Cmd.ExecuteNonQuery()

            tr.Commit()


        Catch ex As Exception

            tr.Rollback()
            MsgBox(ex.Message + " Cannot Generate IRN.", vbOKOnly, "Error !")

            Exit Sub

        End Try

        Dim einv As New eInvoice(Val(lbl_Company.Tag))
        einv.GenerateIRN(Val(lbl_Company.Tag), NewCode, Con, rtbeInvoiceResponse, pic_IRN_QRCode_Image, txt_eInvoiceNo, txt_eInvoiceAckNo, txt_eInvoiceAckDate, txt_eInvoice_CancelStatus, "Cotton_Sales_Head", "Cotton_Sales_Code", Pk_Condition)

    End Sub

    Private Sub btn_Close_eInvoice_Click(sender As Object, e As EventArgs) Handles btn_Close_eInvoice.Click
        grp_EInvoice.Visible = False
    End Sub

    Private Sub btn_Delete_eInvoice_Click(sender As Object, e As EventArgs) Handles btn_Delete_eInvoice.Click

        If Len(Trim(txt_EInvoiceCancellationReson.Text)) = 0 Then
            MsgBox("Please provode the reason for cancellation", vbOKCancel, "Provide Reason !")
            Exit Sub
        End If

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Dim einv As New eInvoice(Val(lbl_Company.Tag))
        einv.CancelIRNByIRN(txt_eInvoiceNo.Text, rtbeInvoiceResponse, "Cotton_Sales_Head", "Cotton_Sales_Code", Con, txt_eInvoice_CancelStatus, NewCode, txt_EInvoiceCancellationReson.Text)


    End Sub

    Private Sub txt_eInvoiceNo_TextChanged(sender As Object, e As EventArgs) Handles txt_eInvoiceNo.TextChanged
        txt_IR_No.Text = txt_eInvoiceNo.Text
    End Sub

    Private Sub btn_Get_QR_Code_Click(sender As Object, e As EventArgs) Handles btn_Get_QR_Code.Click

        Dim CMD As New SqlClient.SqlCommand
        CMD.Connection = Con

        CMD.CommandText = "DELETE FROM " & Common_Procedures.CompanyDetailsDataBaseName & "..e_Invoice_refresh where IRN = '" & txt_eInvoiceNo.Text & "'"
        CMD.ExecuteNonQuery()

        CMD.CommandText = " INSERT INTO " & Common_Procedures.CompanyDetailsDataBaseName & "..e_Invoice_Refresh ([IRN] ,[ACK_No] , [DOC_No] , [SEARCH_BY]  , [COMPANY_IDNO],[Update_Table] ,[Update_table_Unique_Code] ) VALUES " &
                          "('" & txt_eInvoiceNo.Text & "' ,'','','I'," & Val(Common_Procedures.CompGroupIdNo).ToString & ",'Cotton_Sales_Head', 'E_Invoice_IRNO')"
        CMD.ExecuteNonQuery()

        Shell(Application.StartupPath & "\Refresh_IRN.EXE")

    End Sub

    Private Sub btn_refresh_Click(sender As Object, e As EventArgs) Handles btn_refresh.Click
        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim da As New SqlClient.SqlDataAdapter("Select E_Invoice_QR_Image,E_Invoice_IRNO,E_Invoice_ACK_No,E_Invoice_ACK_Date,E_Invoice_Cancelled_Status FROM Cotton_Sales_Head WHERE Cotton_Sales_Code = '" & NewCode & "'", Con)

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
        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim Cmd As New SqlClient.SqlCommand
        Cmd.Connection = Con
        Cmd.CommandText = "Select count(*) from Cotton_Sales_Details Where Cotton_Sales_Code = '" & NewCode & "'"
        Dim c As Int16 = Cmd.ExecuteScalar

        If c <= 0 Then
            MsgBox("Please Save the Invoice Before Generating IRN ", vbOKOnly, "Save")
            Exit Sub
        End If

        Cmd.CommandText = "Select count(*) from Cotton_Sales_Head Where Cotton_Sales_Code = '" & NewCode & "' and (Len(EWay_Bill_No) > 0 or Len(E_Invoice_IRNO) = 0 OR E_Invoice_IRNO IS NULL )"
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

        tr = Con.BeginTransaction
        Cmd.Transaction = tr

        Try

            Cmd.CommandText = "Delete from EWB_By_IRN  where InvCode = '" & NewCode & "'"
            Cmd.ExecuteNonQuery()

            Cmd.CommandText = "Insert into EWB_By_IRN  (	[IRN]                ,	[TransID]        ,	[TransMode] ,	[TransDocNo] ,[TransDocDate]  ,	[VehicleNo]  , [Distance],	[VehType]  ,	[TransName]         , [InvCode] )   Select a.E_Invoice_IRNO  ,  '' as Ledger_GSTINNo ,        '1'    ,        '' as Lr_No  ,   '' as Lr_Date     ,       a.Vehicle_No     , (CASE WHEN a.DeliveryTo_IdNo <> 0 THEN  D.Distance ELSE L.Distance END),      'R'    ,  ''     ,'" & NewCode & "' " &
                                                       " from Cotton_Sales_Head a INNER JOIN Ledger_Head L on a.Ledger_IdNo = L.Ledger_IdNo LEFT OUTER JOIN Ledger_Head D on a.DeliveryTo_IdNo = D.Ledger_IdNo  Where a.Cotton_Sales_Code = '" & NewCode & "'"

            Cmd.ExecuteNonQuery()

            'Cmd.CommandText = "Insert into EWB_By_IRN  (	[IRN]                ,	[TransID]        ,	[TransMode] ,	[TransDocNo] ,[TransDocDate]  ,	[VehicleNo]  , [Distance],	[VehType]  ,	[TransName]         , [InvCode] )   Select A.E_Invoice_IRNO  ,  t.Ledger_GSTINNo,        '1'    ,        ''   ,   Null     ,       a.Vehicle_No     , (CASE WHEN a.DeliveryTo_IdNo <> 0 THEN  D.Distance ELSE L.Distance END),      'R'    ,  t.Ledger_Mainname     ,'" & NewCode & "' " &
            '                                           " from Cotton_Sales_Head a INNER JOIN Ledger_Head L on a.Ledger_IdNo = L.Ledger_IdNo LEFT OUTER JOIN Ledger_Head D on a.DeliveryTo_IdNo = D.Ledger_IdNo LEFT OUTER JOIN Ledger_Head T on a.Transport_IdNo = T.Ledger_IdNo Where a.Cotton_Sales_Code = '" & NewCode & "'"
            'Cmd.ExecuteNonQuery()

            tr.Commit()

            'Exit Sub

            'rtbeInvoiceResponse.Text = einv.AuthTokenReturnMsg

        Catch ex As Exception

            tr.Rollback()
            MsgBox(ex.Message + " Cannot Generate IRN.", vbOKOnly, "Error !")

            Exit Sub

        End Try


        Dim einv As New eInvoice(Val(lbl_Company.Tag))
        einv.GenerateEWBByIRN(NewCode, rtbeInvoiceResponse, txt_eWayBill_No, txt_EWB_Date, txt_EWB_ValidUpto, Con, "Cotton_Sales_Head", "Cotton_Sales_Code", txt_EWB_Canellation_Reason, txt_EWB_Cancel_Status, Pk_Condition)

        Cmd.CommandText = "DELETE FROM EWB_By_IRN WHERE INVCODE = '" & NewCode & "'"
        Cmd.ExecuteNonQuery()

    End Sub

    Private Sub btn_Cancel_EWB_Click(sender As Object, e As EventArgs) Handles btn_Cancel_EWB.Click

        If Len(Trim(txt_EWB_Canellation_Reason.Text)) = 0 Then
            MsgBox("Please provode the reason for cancellation", vbOKCancel, "Provide Reason !")
            Exit Sub
        End If

        Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Dim einv As New eInvoice(Val(lbl_Company.Tag))

        einv.Cancel_EWB_IRN(NewCode, txt_eWayBill_No.Text, rtbeInvoiceResponse, txt_eInvoice_CancelStatus, Con, "Cotton_Sales_Head", "Cotton_Sales_Code", txt_EWB_Canellation_Reason.Text)

    End Sub

    Private Sub txt_eWayBill_No_TextChanged(sender As Object, e As EventArgs) Handles txt_eWayBill_No.TextChanged
        txt_EWay_Bill_No.Text = txt_eWayBill_No.Text
    End Sub
    Private Sub btn_Print_EWB_Click(sender As Object, e As EventArgs) Handles btn_Print_EWB.Click

        'Dim ewb As New EWB(Val(lbl_Company.Tag))
        'EWB.PrintEWB(txt_EWayBillNo.Text, rtbeInvoiceResponse)

        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.PrintEWB(txt_eWayBill_No.Text, rtbeInvoiceResponse, 0, txt_IR_No.Text)

    End Sub
    Private Sub btn_Detail_PRINT_EWB_Click(sender As Object, e As EventArgs) Handles btn_Detail_PRINT_EWB.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.PrintEWB(txt_eWayBill_No.Text, rtbeInvoiceResponse, 1, Trim(txt_IR_No.Text))
    End Sub



    Private Sub Printing_GST_Format_1370(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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

        ClArr(1) = 30 : ClArr(2) = 100 : ClArr(3) = 130 : ClArr(4) = 90 : ClArr(5) = 60 : ClArr(6) = 60 : ClArr(7) = 70 : ClArr(8) = 100
        ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

        'ClArr(1) = 30 : ClArr(2) = 100 : ClArr(3) = 200 : ClArr(4) = 75 : ClArr(5) = 50 : ClArr(6) = 50 : ClArr(7) = 75 : ClArr(8) = 75
        'ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

        TxtHgt = e.Graphics.MeasureString("A", pFont).Height
        TxtHgt = 17.5 '18.6 ' 18.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_GST_Format_1370_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


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

                            Printing_GST_Format_1370_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If





                        prn_DetSNo = prn_DetSNo + 1

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Variety_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 35 Then
                            For I = 35 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 35
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

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
                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, LMargin + ClArr(1) + 5, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + ClArr(2) + 5, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 5, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("HSN_Code").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("bale").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Bale_Nos").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)


                        'Wgt_Bag = "0"
                        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1256" Then '---- SOUTHERN COT SPINNERS
                        '    If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString) <> 0 Then
                        '        Wgt_Bag = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString) / Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), "#########0.000")
                        '    End If
                        'End If
                        'If Val(Wgt_Bag) <> 0 Then
                        '    Common_Procedures.Print_To_PrintDocument(e, Val(Wgt_Bag), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)
                        'End If

                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Rate").ToString), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("AMOUNT").ToString), "#########0.00"), PageWidth - 5, CurY, 1, 0, pFont)

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

                Printing_GST_Format_1370_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True)

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

    Private Sub Printing_GST_Format_1370_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
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
        If Trim(prn_HdDt.Rows(0).Item("Company_logo_Image").ToString) <> "" Then

            If IsDBNull(prn_HdDt.Rows(0).Item("Company_logo_Image")) = False Then

                Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("Company_logo_Image"), Byte())
                If Not imageData Is Nothing Then
                    Using ms As New MemoryStream(imageData, 0, imageData.Length)
                        ms.Write(imageData, 0, imageData.Length)

                        If imageData.Length > 0 Then

                            '.BackgroundImage = Image.FromStream(ms)

                            ' e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 108, CurY + 10, 90, 90)
                            e.Graphics.DrawImage(DirectCast(Image.FromStream(ms), Drawing.Image), LMargin + 15, CurY + 10, 110, 110)

                        End If

                    End Using

                End If

            End If

        End If
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

        If Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString) <> "" Then

            If IsDBNull(prn_HdDt.Rows(0).Item("E_Invoice_QR_Image")) = False Then
                Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("E_Invoice_QR_Image"), Byte())
                If Not imageData Is Nothing Then
                    Using ms As New MemoryStream(imageData, 0, imageData.Length)
                        ms.Write(imageData, 0, imageData.Length)

                        If imageData.Length > 0 Then

                            pic_IRN_QRCode_Image_forPrinting.BackgroundImage = Image.FromStream(ms)

                            e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 110, CurY + 35, 80, 80)

                        End If

                    End Using
                End If
            End If

        End If

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


        If Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString) <> "" Then
            ItmNm1 = Trim(prn_HdDt.Rows(0).Item("E_Invoice_IRNO").ToString)

            ItmNm2 = ""
            If Len(ItmNm1) > 35 Then
                For i = 35 To 1 Step -1
                    If Mid$(Trim(ItmNm1), i, 1) = " " Or Mid$(Trim(ItmNm1), i, 1) = "," Or Mid$(Trim(ItmNm1), i, 1) = "." Or Mid$(Trim(ItmNm1), i, 1) = "-" Or Mid$(Trim(ItmNm1), i, 1) = "/" Or Mid$(Trim(ItmNm1), i, 1) = "_" Or Mid$(Trim(ItmNm1), i, 1) = "(" Or Mid$(Trim(ItmNm1), i, 1) = ")" Or Mid$(Trim(ItmNm1), i, 1) = "\" Or Mid$(Trim(ItmNm1), i, 1) = "[" Or Mid$(Trim(ItmNm1), i, 1) = "]" Or Mid$(Trim(ItmNm1), i, 1) = "{" Or Mid$(Trim(ItmNm1), i, 1) = "}" Then Exit For
                Next i
                If i = 0 Then i = 35

                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - i)
                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), i - 1)
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

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Y1 = CurY + 0.5
        Y2 = CurY + TxtHgt - 10 + TxtHgt + 5
        Common_Procedures.FillRegionRectangle(e, LMargin, Y1, PageWidth, Y2)



        vHeading = "TAX INVOICE"

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
            ' Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Yarn_Sales_No").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, p1Font)
            If prn_HdDt.Rows(0).Item("Cotton_Sales_PreFixNo").ToString <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cotton_Sales_PreFixNo").ToString & "-" & prn_HdDt.Rows(0).Item("Cotton_Sales_RefNo").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, p1Font)
            Else
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cotton_Sales_RefNo").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, p1Font)
            End If

            'Common_Procedures.Print_To_PrintDocument(e, "Transport Mode", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Mode").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "E-Way Bill No.", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("EWay_Bill_No").ToString), LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Invoice Date", LMargin + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Cotton_Sales_Date").ToString), "dd-MM-yyyy").ToString, LMargin + W1 + 30, CurY1, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Vehicle Number", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt



            'Common_Procedures.Print_To_PrintDocument(e, "PO No", LMargin + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)
            'If Trim(prn_HdDt.Rows(0).Item("Order_Date").ToString) <> "" Then
            '    strWidth = e.Graphics.MeasureString("     " & prn_HdDt.Rows(0).Item("Order_No").ToString, pFont).Width
            '    Common_Procedures.Print_To_PrintDocument(e, "  Date : " & prn_HdDt.Rows(0).Item("Order_Date").ToString, LMargin + W1 + 30 + strWidth, CurY1 - 3, 0, 0, pFont)
            'End If
            ''Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Order_No").ToString & "Date : " & prn_HdDt.Rows(0).Item("Order_Date").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "Transport Name", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Transport_IdNo").ToString)), LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)

            ' CurY1 = CurY1 + TxtHgt
            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1066" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1256" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "Lr No", LMargin + 10, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lr_No").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)
            '    If Trim(prn_HdDt.Rows(0).Item("Lr_Date").ToString) <> "" Then
            '        strWidth = e.Graphics.MeasureString("     " & prn_HdDt.Rows(0).Item("Lr_No").ToString, pFont).Width
            '        Common_Procedures.Print_To_PrintDocument(e, "  Date : " & prn_HdDt.Rows(0).Item("Lr_Date").ToString, LMargin + W1 + 30 + strWidth, CurY1 - 3, 0, 0, pFont)
            '    End If
            'Else
            '    ' CurY1 = CurY1 + TxtHgt
            '    'Common_Procedures.Print_To_PrintDocument(e, "DC No", LMargin + 10, CurY1, 0, 0, pFont)
            '    'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY1, 0, 0, pFont)
            '    'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + W1 + 30, CurY1 - 3, 0, 0, pFont)
            '    'If Trim(prn_HdDt.Rows(0).Item("Dc_Date").ToString) <> "" Then
            '    '    strWidth = e.Graphics.MeasureString("     " & prn_HdDt.Rows(0).Item("Dc_No").ToString, pFont).Width
            '    '    Common_Procedures.Print_To_PrintDocument(e, "  Date : " & prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + W1 + 30 + strWidth, CurY1 - 3, 0, 0, pFont)
            '    'End If

            'End If

            Common_Procedures.Print_To_PrintDocument(e, "Date Of Supply", LMargin + C2 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Date_Time_of_Supply").ToString, LMargin + C2 + W1 + 30, CurY1, 0, 0, pFont)


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


            vLedPanNo = Common_Procedures.get_FieldValue(Con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & ")")

            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Or Trim(vLedPanNo) <> "" Then

                If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)
                End If

                C3 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
                If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + S1 + C3 + 10 + strWidth, CurY, 0, PrintWidth, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C3 + 50 + strWidth, CurY, 0, PrintWidth, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelGSTinNo").ToString, LMargin + S1 + C3 + 80 + strWidth, CurY, 0, PrintWidth, pFont)
                End If

                If Trim(vLedPanNo) <> "" Then
                    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
                    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & vLedPanNo, LMargin + S1 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
                End If

            End If

            If Val(prn_HdDt.Rows(0).Item("DeliveryTo_IdNo").ToString) <> 0 Then
                vDelvPanNo = Common_Procedures.get_FieldValue(Con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("DeliveryTo_IdNo").ToString)) & ")")
            Else
                vDelvPanNo = Common_Procedures.get_FieldValue(Con, "Ledger_Head", "Pan_No", "(Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString)) & ")")
            End If

            'If Trim(prn_HdDt.Rows(0).Item("DelGSTinNo").ToString) <> "" Or Trim(vDelvPanNo) <> "" Then
            '    If Trim(prn_HdDt.Rows(0).Item("DelGSTinNo").ToString) <> "" Then
            '        Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + C2 + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelGSTinNo").ToString, LMargin + S1 + C2 + 30, CurY, 0, 0, pFont)
            '    End If
            'If Trim(vDelvPanNo) <> "" Then
            '    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
            '    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & vDelvPanNo, LMargin + S1 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
            'End If
            'End If

            'If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 30, CurY, 0, 0, pFont)

            '    'If Trim(prn_HdDt.Rows(0).Item("Ledger_PanNo").ToString) <> "" Then
            '    '    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
            '    '    Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("Ledger_PanNo").ToString, LMargin + S1 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
            '    'End If

            'End If

            'If Trim(prn_HdDt.Rows(0).Item("DelGSTinNo").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + C2 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelGSTinNo").ToString, LMargin + S1 + C2 + 30, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("DelPanNo").ToString) <> "" Then
                strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
                Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("DelPanNo").ToString, LMargin + S1 + C2 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
            End If
            'End If

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

            If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "State", LMargin + C2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + S1 + C2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("DelState_Name").ToString, LMargin + S1 + C2 + 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Code     " & prn_HdDt.Rows(0).Item("Delivery_State_Code").ToString, LMargin + C2 + ClAr(5) + ClAr(6) + ClAr(7) + 40, CurY, 0, 0, pFont)
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
            'Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DESCRIPTION OF GOODS", LMargin + ClAr(1), CurY, 2, ClAr(2) + ClAr(3), pFont)


            Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "CODE", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY + 5, 2, ClAr(4), p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "BALES", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY - TxtHgt + 5, 2, ClAr(5), pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, "BALES", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + 5, 2, ClAr(5), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "GST %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)

            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1256" Then '---- SOUTHERN COT SPINNERS
            '    Common_Procedures.Print_To_PrintDocument(e, "BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY - TxtHgt + 5, 2, ClAr(6), pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + 5, 2, ClAr(6), pFont)
            '    'Common_Procedures.Print_To_PrintDocument(e, " BAG", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            'End If

            Common_Procedures.Print_To_PrintDocument(e, "BALE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY - TxtHgt + 5, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + 5, 2, ClAr(6), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + 5, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY - TxtHgt + 5, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
            'p1Font = New Font("Calibri", 9, FontStyle.Regular)
            'Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "PRODUCT DESCRIPTION", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "GST%", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "QUANTITY", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "UNIT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "TAXABLE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

            CurY = CurY + TxtHgt + 15
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY


            If Trim(prn_HdDt.Rows(0).Item("Description")) <> "" Then
                p1Font = New Font("Calibri", 9, FontStyle.Bold)
                CurY = CurY + 10
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Description")), LMargin + ClAr(1) + 5, CurY, 0, 0, p1Font)
                CurY = CurY + TxtHgt - 10
            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_GST_Format_1370_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
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
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Taxable_Value").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 5, CurY, 1, 0, pFont)

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
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), LnAr(5), LMargin + ClAr(1) + ClAr(2), LnAr(4))
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


            'vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)


            'Y1 = CurY + 5 '0.5
            'Y2 = CurY + TxtHgt - 15 + TxtHgt
            ''   Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + C1, Y2)

            'If IsDBNull(dt1.Rows(0).Item("Freight_Name").ToString) = False Then
            '    If Trim(dt1.Rows(0).Item("Freight_Name").ToString) <> "" Then
            '        txt_Freight_Name.Text = dt1.Rows(0).Item("Freight_Name").ToString
            '    End If
            'End If
            'txt_FreightAmount.Text = Format(Val(dt1.Rows(0).Item("Freight_Amount").ToString), "#########0.00")
            'If IsDBNull(dt1.Rows(0).Item("Packing_Name").ToString) = False Then
            '    If Trim(dt1.Rows(0).Item("Packing_Name").ToString) <> "" Then
            '        txt_Packing_Name.Text = dt1.Rows(0).Item("Packing_Name").ToString
            '    End If
            'End If
            'txt_Packing.Text = Format(Val(dt1.Rows(0).Item("Packing_Amount").ToString), "#########0.00")
            'If IsDBNull(dt1.Rows(0).Item("AddLess_Name").ToString) = False Then
            '    If Trim(dt1.Rows(0).Item("AddLess_Name").ToString) <> "" Then
            '        txt_AddLess_Name.Text = dt1.Rows(0).Item("AddLess_Name").ToString
            '    End If
            'End If
            'txt_AddLessAmount.Text = Format(Val(dt1.Rows(0).Item("AddLess_Amount").ToString), "#########0.00")


            '  CurY = CurY + TxtHgt - 15

            'CurY = CurY + TxtHgt
            ' e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            'CurY = CurY + TxtHgt - 15
            'Common_Procedures.Print_To_PrintDocument(e, "BANK NAME  :  " & BankNm1, LMargin + 10, CurY, 0, 0, pFont)

            'If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
            '    If is_LastPage = True Then
            '        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Freight_Name").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            '    End If
            'End If


            'CurY = CurY + TxtHgt
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            'CurY = CurY + TxtHgt - 15
            'Common_Procedures.Print_To_PrintDocument(e, "ACCOUNT No.  :  " & BankNm2, LMargin + 10, CurY, 0, 0, pFont)

            'If Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) <> 0 Then
            '    If is_LastPage = True Then
            '        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Packing_Name").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, Format(Math.Abs(Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString)), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

            '    End If
            'End If


            'CurY = CurY + TxtHgt
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            'LnAr(9) = CurY
            'CurY = CurY + TxtHgt - 15
            'Common_Procedures.Print_To_PrintDocument(e, "BRANCH  :  " & BankNm3, LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "IFSC CODE  :  " & BankNm4, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 80 + 15, CurY, 0, 0, pFont)


            'If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
            '    If is_LastPage = True Then
            '        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("AddLess_Name").ToString), LMargin + C1 + 10, CurY, 0, 0, pFont)
            '        Common_Procedures.Print_To_PrintDocument(e, Format(Math.Abs(Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString)), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            '    End If
            'End If

            'CurY = CurY + TxtHgt
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 80, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 80, LnAr(9))
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)


            If Val(prn_HdDt.Rows(0).Item("Freight").ToString) <> 0 Then

                Common_Procedures.Print_To_PrintDocument(e, "Freight", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Freight").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                CurY = CurY + TxtHgt
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)


                Y1 = CurY - 18
                Y2 = CurY + TxtHgt - 15 + TxtHgt
                Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + C1, Y2)

                CurY = CurY + TxtHgt - 15
                Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY - 5, 0, 0, pFont)

            Else


                Y1 = CurY
                Y2 = CurY + TxtHgt - 15 + TxtHgt
                Common_Procedures.FillRegionRectangle(e, LMargin, Y1, LMargin + C1, Y2)

                CurY = CurY + TxtHgt - 15
                Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, pFont)

            End If


            'CurY = CurY + TxtHgt - 15
            'Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Total  Before Tax  ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Taxable_Value").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt

            CurY1 = CurY
            '***************************************
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY


            CurY = CurY + TxtHgt - 15

            p1Font = New Font("Calibri", 8, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "1. We are responsible for the quality of yarn only;If any running fault or", LMargin + 10, CurY, 0, 0, p1Font)



            If Val(prn_HdDt.Rows(0).Item("CGst_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ " & Format(Val(lbl_CGSTPerc.Text), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)

            If Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ " & Format(Val(lbl_SGSTPerc.Text), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If


            p1Font = New Font("Calibri", 8, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "quality defect noted in yarn please inform with firat fabric roll at once. We will", LMargin + 25, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)
            p1Font = New Font("Calibri", 8, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "accept only one roll at defect otherwise we do not hold ourself responsible.", LMargin + 25, CurY, 0, 0, p1Font)

            If Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ " & Format(Val(lbl_IGSTPerc.Text), "##########0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If


            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 8, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "2. Our responsibility ceases when goods leave our permises.", LMargin + 10, CurY, 0, 0, p1Font)

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)

            If Val(prn_HdDt.Rows(0).Item("Commission_Amount").ToString) <> 0 Then
                'Common_Procedures.Print_To_PrintDocument(e, "Add : " & Trim(prn_HdDt.Rows(0).Item("Cmc_Name").ToString) & "  @ " & Format(Val(prn_HdDt.Rows(0).Item("Cmc_Percentage").ToString), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Add : " & "Cess" & "  @ " & Format(Val(prn_HdDt.Rows(0).Item("Commission_Percentage").ToString), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Commission_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                CurY = CurY + TxtHgt

                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)
            End If


            Common_Procedures.Print_To_PrintDocument(e, "Total  TAX Amount", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("SGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("IGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 8, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "3. Interest at the rate of 24% will be charge from the due date.", LMargin + 10, CurY, 0, 0, p1Font)
            If Val(prn_HdDt.Rows(0).Item("TCS_AMOUNT").ToString) <> 0 Then

                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)

                Common_Procedures.Print_To_PrintDocument(e, (prn_HdDt.Rows(0).Item("TCs_name_caption").ToString) & "  @ " & (prn_HdDt.Rows(0).Item("Tcs_Percentage").ToString) & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("TCS_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                CurY = CurY + TxtHgt
            End If

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)

            If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "RoundOff ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Round_Off").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            End If

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
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + TxtHgt + 10, PageWidth, CurY + TxtHgt + 10)
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY + TxtHgt + 10, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))

            CurY = CurY + TxtHgt

            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + TxtHgt + 10, PageWidth, CurY + TxtHgt + 10)
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))

            ''e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 20, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 20, LnAr(10))
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            CurY = CurY + 5

            'p1Font = New Font("Calibri", 9, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, "GST on Reverse Charge", LMargin + C1 + 10, CurY + 5, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY + 5, 1, 0, p1Font)
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + TxtHgt + 10, PageWidth, CurY + TxtHgt + 10)
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY + TxtHgt + 10, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))



            If is_LastPage = True Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- Logu textile
                    BmsInWrds = Trim(UCase(BmsInWrds))
                Else
                    BmsInWrds = Trim(StrConv(BmsInWrds, VbStrConv.ProperCase))
                End If

                p1Font = New Font("Calibri", 11, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, " " & BmsInWrds, LMargin + 10, CurY, 0, 0, p1Font)

                'Rup2 = ""
                'Rup1 = BmsInWrds
                'If Len(Rup1) > 60 Then
                '    For M = 60 To 1 Step -1
                '        If Mid$(Trim(Rup1), M, 1) = " " Then Exit For
                '    Next M
                '    If M = 0 Then M = 60
                '    Rup2 = Microsoft.VisualBasic.Right(Trim(Rup1), Len(Rup1) - M)
                '    Rup1 = Microsoft.VisualBasic.Left(Trim(Rup1), M - 1)
                'End If

                'p1Font = New Font("Calibri", 11, FontStyle.Bold)
                'Common_Procedures.Print_To_PrintDocument(e, " " & Rup1, LMargin + 10, CurY, 0, 0, p1Font)
                'If Trim(Rup2) <> "" Then
                '    CurY = CurY + TxtHgt - 2
                '    Common_Procedures.Print_To_PrintDocument(e, " " & Rup2, LMargin + 10, CurY, 0, 0, p1Font)
                '    CurY = CurY - 10
                'End If

            End If



            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY)


            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1176" Then

            '    CurY = CurY + TxtHgt - 5
            '    Common_Procedures.Print_To_PrintDocument(e, "BANK DETAILS : " & Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), LMargin + 10, CurY, 0, 0, p1Font)
            '    CurY = CurY + TxtHgt + 10
            '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            'End If


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





            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- Logu textile
                Common_Procedures.Print_To_PrintDocument(e, "Received by", LMargin + 35, CurY, 0, 0, pFont)

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1176" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1256" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1286" Then ' KALPANA COTTON

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1370" Then

                    'Common_Procedures.Print_To_PrintDocument(e, "Prepared by", LMargin + ClAr(1) + ClAr(2) + 90, CurY, 0, 0, pFont)

                Else

                    Common_Procedures.Print_To_PrintDocument(e, "Prepared by", LMargin + 35, CurY, 0, 0, pFont)

                End If

            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1176" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1256" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1286" Then ' KALPANA COTTON

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1370" Then

                    'Common_Procedures.Print_To_PrintDocument(e, "Checked by", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 80, CurY, 1, 0, pFont)

                Else

                    Common_Procedures.Print_To_PrintDocument(e, "Checked by", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                End If

            End If




            CurY = CurY + TxtHgt + 10
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1176" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1256" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1286" Then

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1370" Then

                    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + 90, CurY, LMargin + ClAr(1) + ClAr(2) + 90, LnAr(14))
                    'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 40, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 40, LnAr(14))

                Else

                    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + 15, CurY, LMargin + ClAr(1) + ClAr(2) + 15, LnAr(14))
                    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(14))

                End If

            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1370" Then

                'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 30, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 30, LnAr(14))
            Else
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(14))
            End If

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub txt_Description_TextChanged(sender As Object, e As EventArgs) Handles txt_Description.TextChanged

    End Sub

    Private Sub txt_PrefixNo_TextChanged(sender As Object, e As EventArgs) Handles txt_PrefixNo.TextChanged

    End Sub

    Private Sub btn_PDF_Click(sender As Object, e As EventArgs) Handles btn_PDF.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = True
        print_record()

    End Sub

    Private Sub btn_Print_Click(sender As Object, e As EventArgs) Handles btn_Print.Click
        Print_PDF_Status = False
    End Sub
End Class