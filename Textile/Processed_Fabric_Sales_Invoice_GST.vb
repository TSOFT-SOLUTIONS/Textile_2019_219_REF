Public Class Processed_Fabric_Sales_Invoice_GST

    Implements Interface_MDIActions

    Private Con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_RowNo As Integer = -1
    Private Filter_Status As Boolean = False
    Private SaveAll_STS As Boolean = False
    Private Pk_Condition As String = "GPFIN-"
    Private Pk_Condition2 As String = "GFSAG-"
    Private Pk_Condition4 As String = "GFSAG-"
    Private InvPrintFrmt As String = ""

    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_Direct_BaleDetails As New DataGridViewTextBoxEditingControl

    Private Print_PDF_Status As Boolean = False
    Private Printing_Bale_Status As Integer = 0
    Private prn_DetDt_sub As New DataTable
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_Status As Integer = 0
    Private prn_DetIndx As Integer
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private NoFo_STS As Integer = 0
    Private prn_HdIndx As Integer
    Private prn_HdMxIndx As Integer
    Private prn_Count As Integer
    Private prn_HdAr(100, 10) As String
    Private prn_DetAr(100, 50, 10) As String
    Private prn_InpOpts As String = ""
    Private prn_OriDupTri As String = ""
    Private Total_mtrs As Single = 0

    Private LastNo As String = ""
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False
        Print_PDF_Status = False
        chk_SelectAll.Checked = False
        Chk_NoStockPosting.Checked = False
     
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False
        pnl_BaleSelection.Visible = False
        pnl_BaleSelection_ToolTip.Visible = False
        pnl_BuyerOffer_Details.Visible = False
        pnl_BuyerOffer_Selection.Visible = False
        pnl_Print.Visible = False
        pnl_PrintFormat_Selection.Visible = False
        pnl_Direct_BaleDetails.Visible = False

        txt_InvoicePrefixNo.Text = ""
        lbl_InvNo.Text = ""
        lbl_InvNo.ForeColor = Color.Black

        msk_Date.Text = ""
        dtp_Date.Text = ""
        cbo_PartyName.Text = ""
        cbo_Type.Text = "DIRECT"
        cbo_Agent.Text = ""
        cbo_LotNo.Text = ""
        cbo_Com_Type.Text = "%"
        cbo_Through.Text = "DIRECT"
        cbo_RollBundle.Text = "ROLL"
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1035" Then
            cbo_SalesAcc.Text = ""
        End If
       
        cbo_OnAcc.Text = ""

        cbo_Transport.Text = ""
        cbo_Grid_ClothName.Text = ""
        cbo_Grid_Colour.Text = ""
        cbo_Grid_Process.Text = ""
        cbo_Grid_Currency.Text = ""

        txt_OrderNo.Text = ""
        msk_OrderDate.Text = ""
        txt_com_per.Text = ""
        txt_CommAmt.Text = ""
        txt_DcNo.Text = ""
        msk_DcDate.Text = ""
        txt_LrNo.Text = ""
        msk_Lr_Date.Text = ""
        txt_LcNo.Text = ""
        msk_LcDate.Text = ""
        txt_GrTime.Text = ""
        msk_GrDate.Text = ""
       txt_Vechile.Text = ""
        txt_BaleWeight.Text = ""
        txt_Cash_Disc.Text = ""
        lbl_Cash_Disc_Perc.Text = ""
        txt_Trade_Disc.Text = ""
        lbl_Trade_Disc_Perc.Text = ""
        txt_Packing.Text = ""
        lbl_Net_Amt.Text = ""
        txt_Freight.Text = ""
        txt_Insurance.Text = ""

        lbl_CGST_Amount.Text = "0.00"
        lbl_SGST_Amount.Text = "0.00"
        lbl_IGST_Amount.Text = "0.00"

        lbl_AssessableValue.Text = ""

        txt_ElectronicRefNo.Text = ""
        txt_DateAndTimeOFSupply.Text = ""
        cbo_TransportMode.Text = ""
        chk_GSTTax_Invocie.Checked = True



        txt_ClthDetail_Name.Text = "100% COTTON POWERLOOM GREY CLOTH"

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        dgv_Direct_BaleDetails.Rows.Clear()
        dgv_Direct_BaleDetails.Rows.Add()

        dgv_Direct_BaleDetails_Total.Rows.Clear()
        dgv_Direct_BaleDetails_Total.Rows.Add()

        dgv_Buyer_Offer_Detail.Rows.Clear()
        dgv_BuyerOffer_Selection.Rows.Clear()

        dgv_BaleSelectionDetails.Rows.Clear()

        cbo_PartyName.Enabled = True
        cbo_PartyName.BackColor = Color.White

        cbo_Grid_ClothName.Enabled = True
        cbo_Grid_ClothName.BackColor = Color.White

        msk_Date.Enabled = True
        msk_Date.BackColor = Color.White

        dtp_Date.Enabled = True
        dtp_Date.BackColor = Color.White

        btn_Selection.Enabled = True

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ClothName.Text = ""
            cbo_Filter_ClothName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        Grid_Cell_DeSelect()


        cbo_Grid_ClothName.Visible = False
        cbo_Grid_Process.Visible = False
        cbo_Grid_ClothName.Tag = -100
        cbo_Grid_Process.Tag = -100
        cbo_Grid_Colour.Visible = False
        cbo_Grid_Colour.Tag = -100
        cbo_Grid_Currency.Visible = False
        cbo_Grid_Currency.Tag = -100

        NoCalc_Status = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim mskdtxbx As MaskedTextBox
        Dim combobx As ComboBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is CheckBox Or TypeOf Me.ActiveControl Is Button Or TypeOf Me.ActiveControl Is MaskedTextBox Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is MaskedTextBox Then
            mskdtxbx = Me.ActiveControl
            mskdtxbx.SelectionStart = 0
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        End If

        If Me.ActiveControl.Name <> cbo_Grid_ClothName.Name Then
            cbo_Grid_ClothName.Visible = False
            cbo_Grid_ClothName.Tag = -100
        End If

        If Me.ActiveControl.Name <> cbo_Grid_Colour.Name Then
            cbo_Grid_Colour.Visible = False
            cbo_Grid_Colour.Tag = -100
        End If

        If Me.ActiveControl.Name <> cbo_Grid_Process.Name Then
            cbo_Grid_Process.Visible = False
            cbo_Grid_Process.Tag = -100
        End If

        If Me.ActiveControl.Name <> cbo_Grid_Currency.Name Then
            cbo_Grid_Currency.Visible = False
            cbo_Grid_Currency.Tag = -100
        End If

        If Me.ActiveControl.Name <> dgv_Details.Name And Not (TypeOf ActiveControl Is DataGridViewTextBoxEditingControl) Then
            pnl_BaleSelection_ToolTip.Visible = False
        End If

        'If Me.ActiveControl.Name <> dgv_Details.Name Then
        Grid_DeSelect()
        'End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Me.ActiveControl Is CheckBox Then
                Prec_ActCtrl.BackColor = Color.LightSkyBlue
                Prec_ActCtrl.ForeColor = Color.Blue
            ElseIf TypeOf Me.ActiveControl Is Button Then
                Prec_ActCtrl.BackColor = Color.FromArgb(41, 57, 85)
                Prec_ActCtrl.ForeColor = Color.White
            End If
        End If

    End Sub

    Private Sub ControlLostFocus1(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.LightSkyBlue
                Prec_ActCtrl.ForeColor = Color.Blue
            End If
        End If

    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
        If Not IsNothing(dgv_Direct_BaleDetails.CurrentCell) Then dgv_Direct_BaleDetails.CurrentCell.Selected = True
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

    Private Sub ClothSales_Cloth_Invoice_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_LotNo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LOT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_LotNo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Agent.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "AGENT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Agent.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_SalesAcc.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "SALES" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_SalesAcc.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_OnAcc.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ON" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_OnAcc.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_ClothName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH NAME" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_ClothName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Currency.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CURRENCY" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Currency.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            'If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Clothtype.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH TYPE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            '    cbo_Grid_Clothtype.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            'End If

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

    Private Sub ClothSales_Cloth_Invoice_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub ClothSales_Cloth_Invoice_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Direct_BaleDetails.Visible = True Then
                    btn_Close_Direct_BaleDetails_Click(sender, e)
                    Exit Sub

                ElseIf pnl_BaleSelection.Visible = True Then
                    btn_Close_BaleSelection_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Tax.Visible = True Then
                    btn_Tax_Close_Click(sender, e)
                    Exit Sub
                    'ElseIf pnl_Direct_BaleDetails.Visible = True Then
                    '    btn_Close_Direct_BaleDetails_Click(sender, e)
                    '    Exit Sub

                    'ElseIf pnl_BuyerOffer_Selection.Visible = True Then
                    '    btn_Close_BuyerOffer_Selection_Click(sender, e)
                    '    Exit Sub

                    'ElseIf pnl_BuyerOffer_Details.Visible = True Then
                    '    btn_Close_BuyerOffer_Details_Click(sender, e)
                    '    Exit Sub

                    'ElseIf pnl_Print.Visible = True Then
                    '    btn_print_Close_Click(sender, e)
                    '    Exit Sub

                    'ElseIf pnl_PrintFormat_Selection.Visible = True Then
                    '    btn_Close_PrintFormat_Selection_Click(sender, e)
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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub ClothSales_Cloth_Invoice_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Dim dt7 As New DataTable
        Dim dt8 As New DataTable

        Me.Text = ""

        con.Open()

        Common_Procedures.get_CashPartyName_From_All_Entries(con)

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead Where ( Ledger_IdNo = 0 or (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ) order by Ledger_DisplayName", con)
        da.Fill(dt1)
        cbo_PartyName.DataSource = dt1
        cbo_PartyName.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'TRANSPORT') order by Ledger_DisplayName", con)
        da.Fill(dt2)
        cbo_Transport.DataSource = dt2
        cbo_Transport.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'AGENT') order by Ledger_DisplayName", con)
        da.Fill(dt3)
        cbo_Agent.DataSource = dt3
        cbo_Agent.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or AccountsGroup_IdNo = 28 ) order by Ledger_DisplayName", con)
        da.Fill(dt7)
        cbo_SalesAcc.DataSource = dt7
        cbo_SalesAcc.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 or AccountsGroup_IdNo = 6 ) order by Ledger_DisplayName", con)
        da.Fill(dt8)
        cbo_OnAcc.DataSource = dt8
        cbo_OnAcc.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", Con)
        da.Fill(dt4)
        cbo_Grid_ClothName.DataSource = dt4
        cbo_Grid_ClothName.DisplayMember = "Cloth_Name"

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1019" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1027" Then
            Chk_NoStockPosting.Visible = True
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1081" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1114" Then
            btn_Print_PrePrint.Visible = False
        End If

        Label44.Visible = True
        Label43.Visible = True
        Label47.Visible = True
        txt_LcNo.Visible = True
        dtp_LcDate.Visible = True
        msk_LcDate.Visible = True
        cbo_RollBundle.Visible = True

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1081" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1114" Then
            Label44.Visible = False
            Label43.Visible = False
            Label47.Visible = False
            txt_LcNo.Visible = False
            dtp_LcDate.Visible = False
            msk_LcDate.Visible = False
            cbo_RollBundle.Visible = False

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1114" Then
            Label44.Visible = False
            Label43.Visible = False
            Label47.Visible = False
            txt_LcNo.Visible = False
            dtp_LcDate.Visible = False
            msk_LcDate.Visible = False
            cbo_RollBundle.Visible = False

            dgv_Details.Columns(4).ReadOnly = True
            dgv_Details.Columns(5).ReadOnly = True
            dgv_Details.Columns(6).ReadOnly = True
            dgv_Details.Columns(7).ReadOnly = True
            dgv_Details.Columns(11).ReadOnly = True
        End If

        cbo_Com_Type.Items.Clear()
        cbo_Com_Type.Items.Add(" ")
        cbo_Com_Type.Items.Add("%")
        cbo_Com_Type.Items.Add("MTR")

        cbo_Type.Items.Clear()
        cbo_Type.Items.Add(" ")
        cbo_Type.Items.Add("DIRECT")
        cbo_Type.Items.Add("ORDER")
        cbo_Type.Items.Add("DELIVERY")

        cbo_Through.Items.Clear()
        cbo_Through.Items.Add(" ")
        cbo_Through.Items.Add("DIRECT")
        cbo_Through.Items.Add("BANK")
        cbo_Through.Items.Add("AGENT")

        cbo_RollBundle.Items.Clear()
        cbo_RollBundle.Items.Add(" ")
        cbo_RollBundle.Items.Add("ROLL")
        cbo_RollBundle.Items.Add("BUNDLE")


        btn_SaveAll.Visible = False


        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        pnl_BaleSelection.Visible = False
        pnl_BaleSelection.Left = (Me.Width - pnl_BaleSelection.Width) \ 2
        pnl_BaleSelection.Top = (Me.Height - pnl_BaleSelection.Height) \ 2
        pnl_BaleSelection.BringToFront()

        pnl_Print.Visible = False
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2
        pnl_Print.BringToFront()

        pnl_PrintFormat_Selection.Visible = False
        pnl_PrintFormat_Selection.Left = (Me.Width - pnl_PrintFormat_Selection.Width) \ 2
        pnl_PrintFormat_Selection.Top = (Me.Height - pnl_PrintFormat_Selection.Height) \ 2
        pnl_PrintFormat_Selection.BringToFront()

        pnl_BuyerOffer_Selection.Visible = False
        pnl_BuyerOffer_Selection.Left = (Me.Width - pnl_BuyerOffer_Selection.Width) \ 2
        pnl_BuyerOffer_Selection.Top = (Me.Height - pnl_BuyerOffer_Selection.Height) \ 2
        pnl_BuyerOffer_Selection.BringToFront()

        pnl_BuyerOffer_Details.Visible = False
        pnl_BuyerOffer_Details.Left = (Me.Width - pnl_BuyerOffer_Details.Width) \ 2
        pnl_BuyerOffer_Details.Top = (Me.Height - pnl_BuyerOffer_Details.Height) \ 2
        pnl_BuyerOffer_Details.BringToFront()

        pnl_Direct_BaleDetails.Visible = False
        pnl_Direct_BaleDetails.Left = (Me.Width - pnl_Direct_BaleDetails.Width) \ 2
        pnl_Direct_BaleDetails.Top = (Me.Height - pnl_Direct_BaleDetails.Height) \ 2
        pnl_Direct_BaleDetails.BringToFront()


        pnl_Tax.Visible = False
        pnl_Tax.Left = (Me.Width - pnl_Tax.Width) \ 2
        pnl_Tax.Top = ((Me.Height - pnl_Tax.Height) \ 2) - 100
        pnl_Tax.BringToFront()

        dgv_BaleSelectionDetails.Visible = False

        pnl_BaleSelection_ToolTip.Visible = False


        AddHandler txt_InvoicePrefixNo.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Agent.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Type.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Colour.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Through.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Com_Type.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_SalesAcc.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_OnAcc.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Process.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_LotNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_RollBundle.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_OrderDate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OrderNo.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_Lr_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LrNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LcNo.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_LcDate.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_GrDate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_GrTime.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BaleWeight.GotFocus, AddressOf ControlGotFocus
        'AddHandler txt_BaleSelction.GotFocus, AddressOf ControlGotFocus
        'AddHandler btn_lot_Pcs_selection.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_com_per.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CommAmt.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Cash_Disc.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Currency.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_DcDate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DcNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Insurance.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Packing.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Trade_Disc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Vechile.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TradeDic_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Insurance_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Packing_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_NetAmt_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CashDic_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ClthDetail_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_PDF.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_SMS.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_EMail.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_buyerofferSelction.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Inv_Format1.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Inv_Format2.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Close_PrintFormat_Selection.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Bale.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Invoice.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Cancel.GotFocus, AddressOf ControlGotFocus
        'AddHandler txt_BaleSelction.LostFocus, AddressOf ControlLostFocus
        'AddHandler btn_lot_Pcs_selection.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ElectronicRefNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DateAndTimeOFSupply.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DeliveryTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TransportMode.GotFocus, AddressOf ControlGotFocus


        AddHandler txt_InvoicePrefixNo.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Agent.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Colour.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Through.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Com_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_OnAcc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_SalesAcc.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Process.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Currency.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_LotNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_RollBundle.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_Lr_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LrNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LcNo.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_LcDate.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_OrderDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OrderNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BaleWeight.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_DcDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DcNo.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_GrDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_GrTime.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Cash_Disc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_com_per.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CommAmt.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Insurance.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Packing.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Trade_Disc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Vechile.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_NetAmt_Name.LostFocus, AddressOf ControlLostFocus1
        AddHandler txt_Insurance_Name.LostFocus, AddressOf ControlLostFocus1
        AddHandler txt_Packing_Name.LostFocus, AddressOf ControlLostFocus1
        AddHandler txt_TradeDic_Name.LostFocus, AddressOf ControlLostFocus1
        AddHandler txt_CashDic_Name.LostFocus, AddressOf ControlLostFocus1
        AddHandler txt_Freight_Name.LostFocus, AddressOf ControlLostFocus1

        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ElectronicRefNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DateAndTimeOFSupply.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DeliveryTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TransportMode.LostFocus, AddressOf ControlLostFocus



        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_PDF.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_SMS.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_EMail.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_buyerofferSelction.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Bale.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Invoice.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Cancel.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Inv_Format1.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Inv_Format2.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Close_PrintFormat_Selection.LostFocus, AddressOf ControlLostFocus
        'AddHandler txt_BaleSelction.LostFocus, AddressOf ControlLostFocus
        'AddHandler btn_lot_Pcs_selection.LostFocus, AddressOf ControlLostFocus


        'AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_OrderNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_LrNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_Lr_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_BaleWeight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_LcNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_OrderDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_DcDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DcNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_GrDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_GrTime.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_com_per.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CommAmt.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Vechile.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Cash_Disc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Freight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Insurance.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TradeDic_Name.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CashDic_Name.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ClthDetail_Name.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Insurance_Name.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Freight_Name.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Packing_Name.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_NetAmt_Name.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ElectronicRefNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DateAndTimeOFSupply.KeyDown, AddressOf TextBoxControlKeyDown


        'AddHandler txt_InvoicePrefixNo.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_OrderNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_Lr_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_LrNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BaleWeight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_LcNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_OrderDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_DcDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DcNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_GrDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_GrTime.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_com_per.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CommAmt.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Vechile.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Cash_Disc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Trade_Disc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Insurance.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Freight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CashDic_Name.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TradeDic_Name.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Freight_Name.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Insurance_Name.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Packing_Name.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_NetAmt_Name.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_ClthDetail_Name.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_ElectronicRefNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DateAndTimeOFSupply.KeyPress, AddressOf TextBoxControlKeyPress


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

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim I As Integer = 0
        Dim dgv1 As New DataGridView

        If ActiveControl.Name = dgv_Details.Name Or ActiveControl.Name = dgv_Direct_BaleDetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            On Error Resume Next

            dgv1 = Nothing

            If ActiveControl.Name = dgv_Details.Name Then
                dgv1 = dgv_Details

            ElseIf ActiveControl.Name = dgv_Direct_BaleDetails.Name Then
                dgv1 = dgv_Direct_BaleDetails

            ElseIf dgv_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Details

            ElseIf dgv_Direct_BaleDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_Direct_BaleDetails

            ElseIf pnl_Direct_BaleDetails.Visible = True Then
                dgv1 = dgv_Direct_BaleDetails

            ElseIf pnl_Back.Enabled = True Then
                dgv1 = dgv_Details

            End If

            If IsNothing(dgv1) = False Then

                With dgv1

                    If dgv1.Name = dgv_Direct_BaleDetails.Name Then

                        If keyData = Keys.Enter Or keyData = Keys.Down Then
                            If .CurrentCell.ColumnIndex >= .ColumnCount - 2 Then
                                If .CurrentCell.RowIndex = .RowCount - 1 Then
                                    .Rows.Add()
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)
                                    'Close_Direct_BaleDetails()

                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                                End If

                            Else

                                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And ((.CurrentCell.ColumnIndex <> 1 And Val(.CurrentRow.Cells(1).Value) = 0) Or (.CurrentCell.ColumnIndex = 1 And Val(dgtxt_Direct_BaleDetails.Text) = 0)) Then
                                    For I = 0 To .Columns.Count - 1
                                        .Rows(.CurrentCell.RowIndex).Cells(I).Value = ""
                                    Next
                                    ' Close_Direct_BaleDetails()

                                Else
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                                End If


                            End If

                            Return True

                        ElseIf keyData = Keys.Up Then

                            If .CurrentCell.ColumnIndex <= 1 Then
                                If .CurrentCell.RowIndex = 0 Then
                                    'msk_LcDate.Focus()

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

                    Else


                        If keyData = Keys.Enter Or keyData = Keys.Down Then
                            If .CurrentCell.ColumnIndex >= .ColumnCount - 3 Then
                                If .CurrentCell.RowIndex = .RowCount - 1 Then
                                    txt_Trade_Disc.Focus()

                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                                End If
                                'ElseIf .CurrentCell.ColumnIndex = 8 Then

                                '    .CurrentCell = .Rows(.CurrentRow.Index).Cells(17)


                                'ElseIf .CurrentCell.ColumnIndex >= 17 Then

                                '    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If

                            Return True

                        ElseIf keyData = Keys.Up Then

                            If .CurrentCell.ColumnIndex <= 1 Then
                                If .CurrentCell.RowIndex = 0 Then
                                    msk_LcDate.Focus()

                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(11)

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

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt3 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer
        Dim LockSTS As Boolean = False
        Dim da4 As New SqlClient.SqlDataAdapter
        Dim dt4 As New DataTable

        If Val(no) = 0 Then Exit Sub

        clear()

        NoCalc_Status = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from Processed_Fabric_Sales_Invoice_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Processed_Fabric_Sales_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", Con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                txt_InvoicePrefixNo.Text = dt1.Rows(0).Item("Invoice_PrefixNo").ToString
                lbl_InvNo.Text = dt1.Rows(0).Item("Processed_Fabric_Sales_Invoice_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Processed_Fabric_Sales_Invoice_Date").ToString
                msk_Date.Text = dtp_Date.Text

                cbo_PartyName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                cbo_Agent.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Agent_IdNo").ToString))
                cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))
                cbo_SalesAcc.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("SalesAc_IdNo").ToString))
                cbo_OnAcc.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("OnAc_IdNo").ToString))

                cbo_Type.Text = dt1.Rows(0).Item("Invoice_Selection_Type").ToString
                cbo_Through.Text = dt1.Rows(0).Item("Through_Name").ToString
                cbo_Com_Type.Text = dt1.Rows(0).Item("Agent_Comm_Type").ToString
                cbo_RollBundle.Text = dt1.Rows(0).Item("Roll_Bundle").ToString

                txt_LrNo.Text = dt1.Rows(0).Item("Lr_No").ToString
                msk_Lr_Date.Text = dt1.Rows(0).Item("Lr_Date").ToString
                txt_LcNo.Text = dt1.Rows(0).Item("Lc_No").ToString
                msk_LcDate.Text = dt1.Rows(0).Item("Lc_Date").ToString

                msk_OrderDate.Text = dt1.Rows(0).Item("Party_OrderDate").ToString
                'txt_Order_Date.Text = dt1.Rows(0).Item("Party_OrderDate").ToString
                txt_OrderNo.Text = dt1.Rows(0).Item("Party_OrderNo").ToString
                txt_com_per.Text = dt1.Rows(0).Item("Agent_Comm_Perc").ToString
                txt_CommAmt.Text = dt1.Rows(0).Item("Agent_Comm_Total").ToString
                msk_GrDate.Text = dt1.Rows(0).Item("Gr_Date").ToString
                txt_GrTime.Text = dt1.Rows(0).Item("Gr_Time").ToString
                txt_BaleWeight.Text = dt1.Rows(0).Item("Bale_Weight").ToString
                msk_DcDate.Text = dt1.Rows(0).Item("Dc_Date").ToString
                txt_DcNo.Text = dt1.Rows(0).Item("Dc_No").ToString
                txt_Vechile.Text = dt1.Rows(0).Item("Vechile_No").ToString
                '   If Val(dt1.Rows(0).Item("FoldingRate_Status").ToString) = 1 Then chk_No_Folding.Checked = True
                If Val(dt1.Rows(0).Item("No_Stock_Posting_Status").ToString) = 1 Then Chk_NoStockPosting.Checked = True

                txt_ClthDetail_Name.Text = dt1.Rows(0).Item("Cloth_Details").ToString

                txt_Trade_Disc.Text = dt1.Rows(0).Item("Trade_Discount").ToString
                lbl_Trade_Disc_Perc.Text = dt1.Rows(0).Item("Trade_Discount_Perc").ToString
                txt_TradeDic_Name.Text = dt1.Rows(0).Item("TradeDisc_Name").ToString
                txt_Cash_Disc.Text = dt1.Rows(0).Item("Cash_Discount").ToString
                lbl_Cash_Disc_Perc.Text = dt1.Rows(0).Item("Cash_Discount_Perc").ToString
                txt_CashDic_Name.Text = dt1.Rows(0).Item("CashDisc_Name").ToString
                txt_Freight.Text = dt1.Rows(0).Item("Freight").ToString
                txt_Freight_Name.Text = dt1.Rows(0).Item("Freight_Name").ToString
                txt_Insurance.Text = dt1.Rows(0).Item("Insurance").ToString
                txt_Insurance_Name.Text = dt1.Rows(0).Item("Insurance_Name").ToString
                txt_Packing.Text = dt1.Rows(0).Item("Packing_Amount").ToString
                txt_Packing_Name.Text = dt1.Rows(0).Item("Packing_Name").ToString
                lbl_Net_Amt.Text = Common_Procedures.Currency_Format(dt1.Rows(0).Item("Net_Amount").ToString)
                lbl_AssessableValue.Text = Format(Val(dt1.Rows(0).Item("Total_Taxable_Value").ToString), "#########0.00")

                txt_NetAmt_Name.Text = dt1.Rows(0).Item("Net_Amount_Name").ToString
                If IsDBNull(dt1.Rows(0).Item("Gate_Pass_Code").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("Gate_Pass_Code").ToString) <> "" Then
                        LockSTS = True
                    End If
                End If
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

                cbo_LotNo.Text = Common_Procedures.Lot_IdNoToNo(Con, Val(dt1.Rows(0).Item("Lot_IdNo").ToString))

                'txt_CGST_Perc.Text = Format(Val(dt1.Rows(0).Item("CGST_Percentage").ToString), "#########0.00")
                'txt_SGST_Perc.Text = Format(Val(dt1.Rows(0).Item("SGST_Percentage").ToString), "#########0.00")

                lbl_CGST_Amount.Text = Format(Val(dt1.Rows(0).Item("Total_CGST_Amount").ToString), "#########0.00")
                lbl_SGST_Amount.Text = Format(Val(dt1.Rows(0).Item("Total_SGST_Amount").ToString), "#########0.00")
                lbl_IGST_Amount.Text = Format(Val(dt1.Rows(0).Item("Total_IGST_Amount").ToString), "#########0.00")

                If Val(dt1.Rows(0).Item("GST_Tax_Invoice_Status").ToString) = 1 Then chk_GSTTax_Invocie.Checked = True Else chk_GSTTax_Invocie.Checked = False
                txt_ElectronicRefNo.Text = Trim(dt1.Rows(0).Item("Electronic_Reference_No").ToString)
                txt_DateAndTimeOFSupply.Text = Trim(dt1.Rows(0).Item("Date_And_Time_Of_Supply").ToString)
                cbo_TransportMode.Text = Trim(dt1.Rows(0).Item("Transport_Mode").ToString)

                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Cloth_Name, c.Colour_Name,d.Process_Name , e.Currency_Name  from Processed_Fabric_Sales_Invoice_Details a INNER JOIN Cloth_Head b ON a.Fabric_IdNo = b.Cloth_IdNo LEFT OUTER JOIN Colour_Head c ON a.Colour_IdNo = c.Colour_IdNo LEFT OUTER JOIN Process_Head d ON a.Process_IdNo = d.Process_IdNo LEFT OUTER JOIN Currency_Head e ON a.Currency = e.Currency_IdNo Where a.Processed_Fabric_Sales_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.sl_no", Con)
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
                            .Rows(n).Cells(1).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                            .Rows(n).Cells(2).Value = dt2.Rows(i).Item("Colour_Name").ToString
                            .Rows(n).Cells(3).Value = (dt2.Rows(i).Item("Process_Name").ToString)
                            .Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("No_of_Rolls").ToString)
                            .Rows(n).Cells(5).Value = dt2.Rows(i).Item("Roll_Nos").ToString
                            .Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("Pcs").ToString)
                            .Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                            .Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")
                            .Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("Rate_Meter").ToString), "########0.00")
                            .Rows(n).Cells(10).Value = dt2.Rows(i).Item("Currency_Name").ToString
                            .Rows(n).Cells(11).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")

                            .Rows(n).Cells(12).Value = dt2.Rows(i).Item("Processed_Fabric_Sales_Invoice_SlNo").ToString
                            .Rows(n).Cells(13).Value = dt2.Rows(i).Item("Processed_Fabric_inspection_Code").ToString

                            '.Rows(n).Cells(12).Value = dt2.Rows(i).Item("ClothSales_Order_SlNo").ToString
                            '.Rows(n).Cells(13).Value = dt2.Rows(i).Item("ClothSales_Delivery_Code").ToString
                            '.Rows(n).Cells(14).Value = dt2.Rows(i).Item("ClothSales_Delivery_SlNo").ToString
                            '.Rows(n).Cells(15).Value = dt2.Rows(i).Item("ClothSales_Invoice_SlNo").ToString
                            '.Rows(n).Cells(16).Value = dt2.Rows(i).Item("PackingSlip_Codes").ToString
                            '.Rows(n).Cells(17).Value = Format(Val(dt2.Rows(i).Item("Short_Meters").ToString), "########0.00")
                            '.Rows(n).Cells(18).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString) + Val(dt2.Rows(i).Item("Short_Meters").ToString), "########0.00")

                        Next i

                    End If


                    'If .Rows.Count = 0 Then
                    '    .Rows.Add()

                    'Else

                    '    n = .Rows.Count - 1
                    '    If Trim(.Rows(n).Cells(1).Value) = "" And Val(.Rows(n).Cells(7).Value) = 0 Then
                    '        .Rows(n).Cells(15).Value = ""
                    '        If Val(.Rows(n).Cells(15).Value) = 0 Then
                    '            If n = 0 Then
                    '                .Rows(n).Cells(15).Value = 1
                    '            Else
                    '                .Rows(n).Cells(15).Value = Val(.Rows(n - 1).Cells(15).Value) + 1
                    '            End If
                    '        End If
                    '    End If

                    ' End If

                End With

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(4).Value = Val(dt1.Rows(0).Item("Total_No_of_Rolls").ToString)
                    .Rows(0).Cells(6).Value = Val(dt1.Rows(0).Item("Total_Pcs").ToString)
                    .Rows(0).Cells(7).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                    .Rows(0).Cells(8).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")
                    '  .Rows(0).Cells(10).Value = Format(Val(dt1.Rows(0).Item("Total_Currency").ToString), "########0.00")
                    .Rows(0).Cells(11).Value = Format(Val(dt1.Rows(0).Item("Total_Amount").ToString), "########0.00")
                End With


                da2 = New SqlClient.SqlDataAdapter("Select a.* from Processed_Fabric_inspection_Details a Where a.Sales_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.SalesInvoice_DetailsSlNo , a.Processed_Fabric_Inspection_Date, a.for_OrderBy, a.Processed_Fabric_Inspection_No, a.Roll_Code", Con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_BaleSelectionDetails

                    .Rows.Clear()
                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(0).Value = Val(dt2.Rows(i).Item("SalesInvoice_DetailsSlNo").ToString)
                            .Rows(n).Cells(1).Value = dt2.Rows(i).Item("Processed_Fabric_Inspection_No").ToString
                            .Rows(n).Cells(2).Value = ""
                            .Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Meters").ToString)
                            .Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Weight").ToString)
                            .Rows(n).Cells(5).Value = dt2.Rows(i).Item("Roll_Code").ToString
                            .Rows(n).Cells(6).Value = ""

                        Next i

                    End If

                End With
                da4 = New SqlClient.SqlDataAdapter("Select a.* from Processed_Fabric_SalesInvoice_GST_Tax_Details a Where a.ProcessedFabric_Sales_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' ", Con)
                dt4 = New DataTable
                da4.Fill(dt4)

                With dgv_Tax_Details

                    .Rows.Clear()
                    SNo = 0

                    If dt4.Rows.Count > 0 Then

                        For i = 0 To dt4.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(0).Value = SNo
                            .Rows(n).Cells(1).Value = Trim(dt4.Rows(i).Item("HSN_Code").ToString)
                            .Rows(n).Cells(2).Value = IIf(Val(dt4.Rows(i).Item("Taxable_Amount").ToString) <> 0, Format(Val(dt4.Rows(i).Item("Taxable_Amount").ToString), "############0.00"), "")
                            .Rows(n).Cells(3).Value = IIf(Val(dt4.Rows(i).Item("CGST_Percentage").ToString) <> 0, Val(dt4.Rows(i).Item("CGST_Percentage").ToString), "")
                            .Rows(n).Cells(4).Value = IIf(Val(dt4.Rows(i).Item("CGST_Amount").ToString) <> 0, Format(Val(dt4.Rows(i).Item("CGST_Amount").ToString), "##########0.00"), "")
                            .Rows(n).Cells(5).Value = IIf(Val(dt4.Rows(i).Item("SGST_Percentage").ToString) <> 0, Val(dt4.Rows(i).Item("SGST_Percentage").ToString), "")
                            .Rows(n).Cells(6).Value = IIf(Val(dt4.Rows(i).Item("SGST_Amount").ToString) <> 0, Format(Val(dt4.Rows(i).Item("SGST_Amount").ToString), "###########0.00"), "")
                            .Rows(n).Cells(7).Value = IIf(Val(dt4.Rows(i).Item("IGST_Percentage").ToString) <> 0, Val(dt4.Rows(i).Item("IGST_Percentage").ToString), "")
                            .Rows(n).Cells(8).Value = IIf(Val(dt4.Rows(i).Item("IGST_Amount").ToString) <> 0, Format(Val(dt4.Rows(i).Item("IGST_Amount").ToString), "###########0.00"), "")
                        Next i

                    End If

                End With

                da2 = New SqlClient.SqlDataAdapter("Select a.* from Processed_Fabric_Invoice_BaleEntry_Details a Where a.Sales_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.Sl_No", Con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_Direct_BaleDetails

                    .Rows.Clear()


                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(0).Value = Val(SNo)
                            .Rows(n).Cells(1).Value = dt2.Rows(i).Item("Pack_No").ToString
                            .Rows(n).Cells(2).Value = Val(dt2.Rows(i).Item("Pcs").ToString)
                            .Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                            .Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")

                        Next i

                    End If

                    .Rows.Add()
                    Total_Direct_BaleDetailsEntry_Calculation()

                End With

            End If

            'If LockSTS = True Then
            '    cbo_PartyName.Enabled = False
            '    cbo_PartyName.BackColor = Color.LightGray

            '    cbo_Grid_ClothName.Enabled = False
            '    cbo_Grid_ClothName.BackColor = Color.LightGray

            '    msk_Date.Enabled = False
            '    msk_Date.BackColor = Color.LightGray

            '    dtp_Date.Enabled = False
            '    dtp_Date.BackColor = Color.LightGray

            '    dgv_Details.AllowUserToAddRows = False

            '    btn_Selection.Enabled = False


            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1050" Then '---- Kumaravel Textiles (Palladam)
                If txt_InvoicePrefixNo.Enabled And txt_InvoicePrefixNo.Visible Then txt_InvoicePrefixNo.Focus()
            Else

                If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
                'If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

            End If

        End Try

        NoCalc_Status = False

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim nr As Integer = 0

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Entry_Processing_Fabric_Invoice, "~L~") = 0 And InStr(Common_Procedures.UR.Entry_Processing_Fabric_Invoice, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        'Da = New SqlClient.SqlDataAdapter("select * from Processed_Fabric_Sales_Invoice_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Processed_Fabric_Sales_Invoice_Code = '" & Trim(NewCode) & "'", con)
        'Dt1 = New DataTable
        'Da.Fill(Dt1)

        'If Dt1.Rows.Count > 0 Then
        '    If IsDBNull(Dt1.Rows(0).Item("Gate_Pass_Code").ToString) = False Then
        '        If Trim(Dt1.Rows(0).Item("Gate_Pass_Code").ToString) <> "" Then
        '            MessageBox.Show("Already Piece Delivered", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '            Exit Sub
        '        End If
        '    End If
        'End If

        'Dt1.Clear()
        'Dt1.Dispose()
        'Da.Dispose()
        trans = Con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = Con
            cmd.Transaction = trans

            If Common_Procedures.VoucherBill_Deletion(Con, Trim(Pk_Condition) & Trim(NewCode), trans) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            Common_Procedures.Voucher_Deletion(Con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)
            Common_Procedures.Voucher_Deletion(Con, Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), trans)

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Processed_Fabric_inspection_Details set Sales_Invoice_Code = '', SalesInvoice_DetailsSlNo = 0, Sales_Invoice_Increment = Sales_Invoice_Increment - 1 Where Sales_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Processed_Fabric_Sales_Invoice_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Processed_Fabric_Sales_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Processed_Fabric_Sales_Invoice_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Processed_Fabric_Sales_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            cmd.CommandText = "Delete from Processed_Fabric_Invoice_BaleEntry_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            trans.Commit()

            new_record()

            MessageBox.Show("Deleted Successfully!!!", "FOR DELETION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            trans.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            trans.Dispose()
            cmd.Dispose()

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1050" Then '---- Kumaravel Textiles (Palladam)
                If txt_InvoicePrefixNo.Enabled And txt_InvoicePrefixNo.Visible Then txt_InvoicePrefixNo.Focus()
            Else

                If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
                'If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            End If

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ClothName.Text = ""
            cbo_Filter_ClothName.SelectedIndex = -1
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

            da = New SqlClient.SqlDataAdapter("select top 1 Processed_Fabric_Sales_Invoice_No from Processed_Fabric_Sales_Invoice_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Processed_Fabric_Sales_Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' AND Tax_Type ='GST' Order by for_Orderby, Processed_Fabric_Sales_Invoice_No", Con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Processed_Fabric_Sales_Invoice_No from Processed_Fabric_Sales_Invoice_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Processed_Fabric_Sales_Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' AND Tax_Type ='GST' Order by for_Orderby, Processed_Fabric_Sales_Invoice_No", Con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Processed_Fabric_Sales_Invoice_No from Processed_Fabric_Sales_Invoice_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Processed_Fabric_Sales_Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'AND Tax_Type ='GST' Order by for_Orderby desc, Processed_Fabric_Sales_Invoice_No desc", Con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Processed_Fabric_Sales_Invoice_No from Processed_Fabric_Sales_Invoice_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Processed_Fabric_Sales_Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'AND Tax_Type ='GST' Order by for_Orderby desc, Processed_Fabric_Sales_Invoice_No desc", Con)
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

        Try

            clear()

            New_Entry = True

            lbl_InvNo.Text = Common_Procedures.get_MaxCode(Con, "Processed_Fabric_Sales_Invoice_Head", "Processed_Fabric_Sales_Invoice_Code", "For_OrderBy", "Tax_Type ='GST'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_InvNo.ForeColor = Color.Red

            Da1 = New SqlClient.SqlDataAdapter("select top 1 a.*, b.ledger_name as SalesAcName from Processed_Fabric_Sales_Invoice_Head a LEFT OUTER JOIN Ledger_Head b ON a.SalesAc_IdNo = b.Ledger_IdNo where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Processed_Fabric_Sales_Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Processed_Fabric_Sales_Invoice_No desc", con)
            Da1.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then
                If Dt1.Rows(0).Item("Invoice_PrefixNo").ToString <> "" Then txt_InvoicePrefixNo.Text = Dt1.Rows(0).Item("Invoice_PrefixNo").ToString
                If Dt1.Rows(0).Item("SalesAcName").ToString <> "" Then cbo_SalesAcc.Text = Dt1.Rows(0).Item("SalesAcName").ToString
                If Dt1.Rows(0).Item("CashDisc_Name").ToString <> "" Then txt_CashDic_Name.Text = Dt1.Rows(0).Item("CashDisc_Name").ToString
                If Dt1.Rows(0).Item("TradeDisc_Name").ToString <> "" Then txt_TradeDic_Name.Text = Dt1.Rows(0).Item("TradeDisc_Name").ToString
                If Dt1.Rows(0).Item("Freight_Name").ToString <> "" Then txt_Freight_Name.Text = Dt1.Rows(0).Item("Freight_Name").ToString
                If Dt1.Rows(0).Item("Packing_Name").ToString <> "" Then txt_Packing_Name.Text = Dt1.Rows(0).Item("Packing_Name").ToString
                If Dt1.Rows(0).Item("Insurance_Name").ToString <> "" Then txt_Insurance_Name.Text = Dt1.Rows(0).Item("Insurance_Name").ToString
                txt_ClthDetail_Name.Text = Dt1.Rows(0).Item("Cloth_Details").ToString
            End If

            Dt1.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da1.Dispose()

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1050" Then '---- Kumaravel Textiles (Palladam)
                If txt_InvoicePrefixNo.Enabled And txt_InvoicePrefixNo.Visible Then txt_InvoicePrefixNo.Focus()
            Else

                If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
                'If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            End If

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        Try

            inpno = InputBox("Enter Inv No.", "FOR FINDING...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Processed_Fabric_Sales_Invoice_No from Processed_Fabric_Sales_Invoice_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Processed_Fabric_Sales_Invoice_Code = '" & Trim(InvCode) & "'", con)
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
        Dim movno As String, inpno As String
        Dim InvCode As String

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Entry_Processing_Fabric_Invoice, "~L~") = 0 And InStr(Common_Procedures.UR.Entry_Processing_Fabric_Invoice, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Inv No.", "FOR NEW INV NO. INSERTION...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Processed_Fabric_Sales_Invoice_No from Processed_Fabric_Sales_Invoice_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Processed_Fabric_Sales_Invoice_Code = '" & Trim(Pk_Condition) & Trim(InvCode) & "'", Con)
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
                    MessageBox.Show("Invalid Inv No.", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_InvNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT ...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        Dim clth_ID As Integer = 0
        Dim Fb_ID As Integer = 0
        Dim Proc_ID As Integer = 0
        Dim Lot_ID As Integer = 0
        Dim Trans_ID As Integer
        Dim Led_ID As Integer = 0
        Dim Agt_Idno As Integer = 0
        Dim Sno As Integer = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Partcls As String = ""
        Dim vTotPcs As Single, vTotMtrs As Double, vTotNoRls As Single, vTotAmt As Double, vTotWgt As Double, vTotCrny
        Dim SlAc_ID As Integer = 0
        Dim OnAc_ID As Integer = 0
        Dim YrnClthNm As String = ""
        Dim Nr As Integer = 0
        Dim No_Stock_Posting As Integer = 0
        Dim OrdCd As String = ""
        Dim OrdSlNo As Long = 0
        Dim DcCd As String = ""
        Dim DcSlNo As Long = 0
        Dim OpYrCode As String = ""
        Dim vOrdDt As String = ""
        Dim vDcDt As String = ""
        Dim vLrDt As String = ""
        Dim vGrDt As String = ""
        Dim vLcDt As String = ""
        Dim InvMtrFld As Single = 0
        Dim InvMtrShtFld As Single = 0
        Dim Fold_Meter As Single = 0
        Dim Fold_Short_Meter As Single = 0
        Dim Col_ID As Integer
        Dim Curncy_ID As Integer = 0
        Dim PkCode As String = ""

        Dim Comm_Amt As Double = 0
        Dim ag_Comm As Double = 0
        Dim agtds_perc As Double = 0

        Dim Dt2 As New DataTable
        Dim YrnCons_For As String = ""

        Dim vGST_Tax_Inv_Sts As Integer = 0
        Dim vDelvTo_IdNo As Integer = 0



        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Entry_Processing_Fabric_Invoice, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1050" Then '---- Kumaravel Textiles (Palladam)
            If Trim(txt_InvoicePrefixNo.Text) = "" Then
                MessageBox.Show("Invalid Invoice Prefix No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If txt_InvoicePrefixNo.Enabled And txt_InvoicePrefixNo.Visible Then txt_InvoicePrefixNo.Focus()
                Exit Sub
            End If
        End If

        If IsDate(msk_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
            Exit Sub
        End If

        If Not (Convert.ToDateTime(msk_Date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_Date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
            Exit Sub
        End If


        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)
        lbl_UserName.Text = Common_Procedures.User.IdNo
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled Then cbo_PartyName.Focus()
            Exit Sub
        End If

        If Trim(UCase(cbo_Type.Text)) = "" Or (Trim(UCase(cbo_Type.Text)) <> "ORDER" And Trim(UCase(cbo_Type.Text)) <> "DELIVERY") Then
            cbo_Type.Text = "DIRECT"
        End If

        Agt_Idno = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Agent.Text)
        Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)
        SlAc_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_SalesAcc.Text)
        OnAc_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_OnAcc.Text)
        Lot_ID = Common_Procedures.Lot_NoToIdNo(Con, cbo_LotNo.Text)
        vDelvTo_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(Con, cbo_DeliveryTo.Text)

        If SlAc_ID = 0 And Val(lbl_Net_Amt.Text) <> 0 Then
            MessageBox.Show("Invalid Sales A/c", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_SalesAcc.Enabled Then cbo_SalesAcc.Focus()
            Exit Sub
        End If

        For i = 0 To dgv_Details.RowCount - 1

            If Val(dgv_Details.Rows(i).Cells(7).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(9).Value) <> 0 Then

                clth_ID = Common_Procedures.Cloth_NameToIdNo(Con, dgv_Details.Rows(i).Cells(1).Value)
                If clth_ID = 0 Then
                    MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(1)
                    End If
                    Exit Sub
                End If

                Col_ID = Common_Procedures.Colour_NameToIdNo(Con, dgv_Details.Rows(i).Cells(2).Value)
                If Col_ID = 0 Then
                    MessageBox.Show("Invalid Colour Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(2)
                    End If
                    Exit Sub
                End If

                If Val(dgv_Details.Rows(i).Cells(4).Value) = 0 Then
                    MessageBox.Show("Invalid No Of Rolls", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(4)
                    End If
                    Exit Sub
                End If


                If Val(dgv_Details.Rows(i).Cells(7).Value) = 0 Then
                    MessageBox.Show("Invalid Metres", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(7)
                    End If
                    Exit Sub
                End If

            End If

        Next
        vGST_Tax_Inv_Sts = 0
        If chk_GSTTax_Invocie.Checked = True Then vGST_Tax_Inv_Sts = 1

        NoCalc_Status = False
        Total_Calculation()

        vTotPcs = 0 : vTotMtrs = 0 : vTotWgt = 0 : vTotAmt = 0 : vTotCrny = 0 : vTotNoRls = 0

        If dgv_Details_Total.RowCount > 0 Then
            vTotNoRls = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
            vTotPcs = Val(dgv_Details_Total.Rows(0).Cells(6).Value())
            vTotMtrs = Val(dgv_Details_Total.Rows(0).Cells(7).Value())
            vTotWgt = Val(dgv_Details_Total.Rows(0).Cells(8).Value())
            ' vTotCrny = Val(dgv_Details_Total.Rows(0).Cells(10).Value())
            vTotAmt = Val(dgv_Details_Total.Rows(0).Cells(11).Value())
        End If

        'If vTotMtrs = 0 Then
        '    MessageBox.Show("Invalid METERS", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If dgv_Details.Enabled And dgv_Details.Visible Then
        '        dgv_Details.Focus()
        '        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(7)
        '    End If
        '    Exit Sub
        'End If


        vOrdDt = ""
        If Trim(msk_OrderDate.Text) <> "" Then
            If IsDate(msk_OrderDate.Text) = True Then
                vOrdDt = Trim(msk_OrderDate.Text)
            End If
        End If
        vDcDt = ""
        If Trim(msk_DcDate.Text) <> "" Then
            If IsDate(msk_DcDate.Text) = True Then
                vDcDt = Trim(msk_DcDate.Text)
            End If
        End If
        vGrDt = ""
        If Trim(msk_GrDate.Text) <> "" Then
            If IsDate(msk_GrDate.Text) = True Then
                vGrDt = Trim(msk_GrDate.Text)
            End If
        End If
        vLrDt = ""
        If Trim(msk_Lr_Date.Text) <> "" Then
            If IsDate(msk_Lr_Date.Text) = True Then
                vLrDt = Trim(msk_Lr_Date.Text)
            End If
        End If
        vLcDt = ""
        If Trim(msk_LcDate.Text) <> "" Then
            If IsDate(msk_LcDate.Text) = True Then
                vLcDt = Trim(msk_LcDate.Text)
            End If
        End If

        tr = Con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_InvNo.Text = Common_Procedures.get_MaxCode(Con, "Processed_Fabric_Sales_Invoice_Head", "Processed_Fabric_Sales_Invoice_Code", "For_OrderBy", "Tax_Type ='GST'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If



            cmd.Connection = Con
            cmd.Transaction = tr

            cmd.Parameters.Clear()

            cmd.Parameters.AddWithValue("@InvoiceDate", Convert.ToDateTime(msk_Date.Text))
            'cmd.Parameters.AddWithValue("@InvoiceDate", dtp_Date.Value.Date)

            If New_Entry = True Then

                cmd.CommandText = "Insert into Processed_Fabric_Sales_Invoice_Head ( Processed_Fabric_Sales_Invoice_Code ,               Company_IdNo       ,     Processed_Fabric_Sales_Invoice_No     ,                     for_OrderBy                                        ,                Invoice_PrefixNo                 , Processed_Fabric_Sales_Invoice_Date ,           Ledger_IdNo    ,               Invoice_Selection_Type  ,      Party_OrderNo                 ,    Party_OrderDate     ,      Agent_IdNo           , Agent_Comm_Perc                   , Agent_Comm_Type                 , Agent_Comm_Total                  , Dc_No                           ,         Dc_Date       ,        Through_Name              ,       Lr_No                  ,          Lr_Date     ,           Lc_No              ,          Lc_Date     ,        Gr_Time                   ,         Gr_Date      ,           SalesAc_IdNo   ,           OnAc_IdNo      ,       FoldingRate_Status       ,   Transport_IdNo            , Vechile_No                         ,            Bale_Weight                ,            Cloth_Details                 ,         TradeDisc_Name                 ,         Trade_Discount                ,         CashDisc_Name                ,           Cash_Discount              ,            Trade_Discount_Perc             ,          Cash_Discount_Perc              ,            Packing_Name              ,           Packing_Amount          ,            Freight_Name              ,             Freight                ,             Insurance_Name             ,             Insurance               ,            Net_Amount_Name           ,                Net_Amount               ,          Total_No_of_Rolls      ,         Total_Pcs        ,   Total_Meters      ,           Total_Weight   ,        Total_Amount      , Total_Currency                 , No_Stock_Posting_Status   ,                    Roll_Bundle                ,   user_idNo                    ,  Lot_idNo          ,Total_Taxable_Value                           ,Total_CGST_Amount               , Total_SGST_Amount            ,    Total_IGST_Amount ,     DeliveryTo_idNo        ,           Electronic_Reference_No         ,       Date_And_Time_Of_Supply             ,       Transport_Mode                  ,    GST_Tax_Invoice_Status          ,Tax_Type  ,Despatch_To,Delivery_Address1,Delivery_Address2 ) " & _
                                    "     Values                       (  '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvNo.Text))) & ", '" & Trim(UCase(txt_InvoicePrefixNo.Text)) & "' ,     @InvoiceDate        , " & Str(Val(Led_ID)) & " ,  '" & Trim(UCase(cbo_Type.Text)) & "' ,  '" & Trim(txt_OrderNo.Text) & "'  , '" & Trim(vOrdDt) & "' , " & Str(Val(Agt_Idno)) & ", " & Str(Val(txt_com_per.Text)) & ",'" & Trim(cbo_Com_Type.Text) & "', " & Str(Val(txt_CommAmt.Text)) & ",  '" & Trim(txt_DcNo.Text) & "'  , '" & Trim(vDcDt) & "' ,  '" & Trim(cbo_Through.Text) & "', '" & Trim(txt_LrNo.Text) & "', '" & Trim(vLrDt) & "', '" & Trim(txt_LcNo.Text) & "',                                                   '" & Trim(vLcDt) & "', " & Str(Val(txt_GrTime.Text)) & ", '" & Trim(vGrDt) & "', " & Str(Val(SlAc_ID)) & ", " & Str(Val(OnAc_ID)) & ",   " & Str(Val(NoFo_STS)) & "  ,  " & Str(Val(Trans_ID)) & " ,   '" & Trim(txt_Vechile.Text) & "' , " & Str(Val(txt_BaleWeight.Text)) & " ,  '" & Trim(txt_ClthDetail_Name.Text) & "',  '" & Trim(txt_TradeDic_Name.Text) & "', " & Str(Val(txt_Trade_Disc.Text)) & " , '" & Trim(txt_CashDic_Name.Text) & "',  " & Str(Val(txt_Cash_Disc.Text)) & ", " & Str(Val(lbl_Trade_Disc_Perc.Text)) & " , " & Str(Val(lbl_Cash_Disc_Perc.Text)) & ", '" & Trim(txt_Packing_Name.Text) & "', " & Str(Val(txt_Packing.Text)) & ", '" & Trim(txt_Freight_Name.Text) & "',  " & Str(Val(txt_Freight.Text)) & ", '" & Trim(txt_Insurance_Name.Text) & "', " & Str(Val(txt_Insurance.Text)) & ",  '" & Trim(txt_NetAmt_Name.Text) & "', " & Str(Val(CSng(lbl_Net_Amt.Text))) & ", " & Str(Val(vTotNoRls)) & ", " & Str(Val(vTotPcs)) & ", " & Str(Val(vTotMtrs)) & " ,  " & Str(Val(vTotWgt)) & ",   " & Str(Val(vTotAmt)) & ", " & Str(Val(vTotCrny)) & ", " & Str(Val(No_Stock_Posting)) & " , '" & Trim(cbo_RollBundle.Text) & "', " & Val(lbl_UserName.Text) & " ," & Val(Lot_ID) & " ," & Val(lbl_AssessableValue.Text) & " ," & Val(lbl_CGST_Amount.Text) & "," & Val(lbl_SGST_Amount.Text) & "," & Val(lbl_IGST_Amount.Text) & "," & Str(Val(vDelvTo_IdNo)) & ",'" & Trim(txt_ElectronicRefNo.Text) & "','" & Trim(txt_DateAndTimeOFSupply.Text) & "' ,'" & Trim(cbo_TransportMode.Text) & "'," & Str(Val(vGST_Tax_Inv_Sts)) & " ,   'GST' ,         ''  ,       ''      ,     ''             ) "
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Processed_Fabric_Sales_Invoice_Head set Processed_Fabric_Sales_Invoice_Date = @InvoiceDate, Invoice_PrefixNo = '" & Trim(UCase(txt_InvoicePrefixNo.Text)) & "' ,  Ledger_IdNo =  " & Str(Val(Led_ID)) & " ,    Invoice_Selection_Type = '" & Trim(UCase(cbo_Type.Text)) & "' , Party_OrderNo =  '" & Trim(txt_OrderNo.Text) & "',     Party_OrderDate = '" & Trim(vOrdDt) & "' ,  Through_Name = '" & Trim(cbo_Through.Text) & "'  ,  Agent_IdNo = " & Str(Val(Agt_Idno)) & " ,Agent_Comm_Perc =  " & Str(Val(txt_com_per.Text)) & ", Agent_Comm_Type = '" & Trim(cbo_Com_Type.Text) & "' , Agent_Comm_Total = " & Str(Val(txt_CommAmt.Text)) & ", Dc_No = '" & Trim(txt_DcNo.Text) & "', Dc_Date = '" & Trim(vDcDt) & "',  Lr_No = '" & Trim(txt_LrNo.Text) & "'  , Lr_Date  = '" & Trim(vLrDt) & "',  Lc_No = '" & Trim(txt_LcNo.Text) & "'  , Lc_Date  = '" & Trim(vLcDt) & "',  Gr_Time = " & Str(Val(txt_GrTime.Text)) & ", Gr_Date = '" & Trim(vGrDt) & "',  SalesAc_IdNo = " & Str(Val(SlAc_ID)) & ", OnAc_IdNo = " & Str(Val(OnAc_ID)) & " ,  FoldingRate_Status  = " & Str(Val(NoFo_STS)) & " , Transport_IdNo = " & Str(Val(Trans_ID)) & "  ,Vechile_No = '" & Trim(txt_Vechile.Text) & "',  Bale_Weight =  " & Str(Val(txt_BaleWeight.Text)) & ", Cloth_Details =  '" & Trim(txt_ClthDetail_Name.Text) & "', TradeDisc_Name = '" & Trim(txt_TradeDic_Name.Text) & "', Trade_Discount =  " & Str(Val(txt_Trade_Disc.Text)) & " , CashDisc_Name ='" & Trim(txt_CashDic_Name.Text) & "'  , Cash_Discount = " & Str(Val(txt_Cash_Disc.Text)) & " , Trade_Discount_Perc = " & Str(Val(lbl_Trade_Disc_Perc.Text)) & "   , Cash_Discount_Perc = " & Str(Val(lbl_Cash_Disc_Perc.Text)) & " , Packing_Name ='" & Trim(txt_Packing_Name.Text) & "', Packing_Amount = " & Str(Val(txt_Packing.Text)) & " , Freight_Name = '" & Trim(txt_Freight_Name.Text) & "' , Roll_Bundle = '" & Trim(cbo_RollBundle.Text) & "' , Lot_idNo = " & Val(Lot_ID) & " , Freight =" & Str(Val(txt_Freight.Text)) & " , Insurance_Name ='" & Trim(txt_Insurance_Name.Text) & "' , Insurance =  " & Str(Val(txt_Insurance.Text)) & ", Net_Amount_Name = '" & Trim(txt_NetAmt_Name.Text) & "' , Net_Amount = " & Str(Val(CSng(lbl_Net_Amt.Text))) & " , Total_No_Of_Rolls  = " & Str(Val(vTotNoRls)) & " ,   Total_Pcs =  " & Str(Val(vTotPcs)) & ", Total_Meters = " & Str(Val(vTotMtrs)) & " , Total_Weight = " & Str(Val(vTotWgt)) & "  ,Total_Amount = " & Str(Val(vTotAmt)) & "  ,Total_Currency = " & Str(Val(vTotCrny)) & " ,No_Stock_Posting_Status =  " & Str(Val(No_Stock_Posting)) & " , User_idNo = " & Val(lbl_UserName.Text) & ",Total_Taxable_Value =" & Val(lbl_AssessableValue.Text) & "  ,Total_CGST_Amount  =" & Val(lbl_CGST_Amount.Text) & "  , Total_SGST_Amount =" & Val(lbl_SGST_Amount.Text) & ", Total_IGST_Amount =" & Val(lbl_IGST_Amount.Text) & ",Electronic_Reference_No ='" & Trim(txt_ElectronicRefNo.Text) & "'   ,Date_And_Time_Of_Supply ='" & Trim(txt_DateAndTimeOFSupply.Text) & "' ,Transport_Mode ='" & Trim(cbo_TransportMode.Text) & "',GST_Tax_Invoice_Status = " & Str(Val(vGST_Tax_Inv_Sts)) & ", DeliveryTo_IdNo = " & Str(Val(vDelvTo_IdNo)) & ",  Tax_Type ='GST',Despatch_To='',Delivery_Address1='',Delivery_Address2=''     Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Processed_Fabric_Sales_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Processed_Fabric_inspection_Details set Sales_Invoice_Code = '', SalesInvoice_DetailsSlNo = 0, Sales_Invoice_Increment = Sales_Invoice_Increment - 1 Where Sales_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            EntID = Trim(Pk_Condition) & Trim(lbl_InvNo.Text)
            PBlNo = Trim(lbl_InvNo.Text)
            Partcls = "FabricSales : Inv.No. " & Trim(lbl_InvNo.Text)

            cmd.CommandText = "Delete from Processed_Fabric_Sales_Invoice_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Processed_Fabric_Sales_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Details

                Sno = 0
                YrnClthNm = ""

                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(7).Value) <> 0 Or Val(.Rows(i).Cells(9).Value) <> 0 Then

                        Sno = Sno + 1

                        InvMtrFld = 0
                        InvMtrShtFld = 0

                        Fb_ID = Common_Procedures.Cloth_NameToIdNo(Con, .Rows(i).Cells(1).Value, tr)

                        Col_ID = Common_Procedures.Colour_NameToIdNo(Con, .Rows(i).Cells(2).Value, tr)
                        Proc_ID = Common_Procedures.Process_NameToIdNo(Con, .Rows(i).Cells(3).Value, tr)
                        Curncy_ID = Common_Procedures.Currency_NameToIdNo(Con, .Rows(i).Cells(10).Value, tr)

                        cmd.CommandText = "Insert into Processed_Fabric_Sales_Invoice_Details ( Processed_Fabric_Sales_Invoice_Code  ,               Company_IdNo       ,      Processed_Fabric_Sales_Invoice_No    ,                               for_OrderBy      , Processed_Fabric_Sales_Invoice_Date       ,         Invoice_Selection_Type      ,                 Sl_No    ,        Fabric_IdNo        ,       Colour_IdNo        ,              Process_IdNo  ,                No_Of_Rolls                     ,                    Roll_Nos          ,                       Pcs                 ,                      Meters              ,                     Weight                ,                       Rate_Meter    ,       Currency             ,         Amount               , Processed_Fabric_Sales_Invoice_SlNo                   , Processed_Fabric_inspection_Code  ,              HSN_Code                     ,GST_Percentage   ) " & _
                                                "     Values                      (   '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvNo.Text))) & ",       @InvoiceDate            ,     '" & Trim(UCase(cbo_Type.Text)) & "',       " & Str(Val(Sno)) & ", " & Str(Val(Fb_ID)) & ",    " & Str(Val(Col_ID)) & ",       " & Str(Val(Proc_ID)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ",'" & Trim(.Rows(i).Cells(5).Value) & "',  " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & ", " & Str(Val(.Rows(i).Cells(8).Value)) & ", " & Str(Val(.Rows(i).Cells(9).Value)) & ", " & Val(Curncy_ID) & ",      " & Val(.Rows(i).Cells(11).Value) & ", " & Str(Val(.Rows(i).Cells(12).Value)) & ", " & Str(Val(.Rows(i).Cells(14).Value)) & "," & Str(Val(.Rows(i).Cells(20).Value)) & "," & Str(Val(.Rows(i).Cells(19).Value)) & " ) "
                        cmd.ExecuteNonQuery()

                        'cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code,             Company_IdNo         ,           Reference_No       ,                               for_OrderBy                             , Reference_Date ,                                            StockOff_IdNo  ,      DeliveryTo_Idno    ,                              ReceivedFrom_Idno            ,         Entry_ID     ,       Party_Bill_No  ,       Particulars      ,           Sl_No      ,           Cloth_Idno     ,                      Rolls                 ,   Meters_Type1                            ,                        Weight          ) " & _
                        '                          " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvNo.Text))) & ",  @InvoiceDate, " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", " & Str(Val(Led_ID)) & ", " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(clth_ID)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & " , " & Str(Val(.Rows(i).Cells(8).Value)) & ") "
                        'cmd.ExecuteNonQuery()

                    End If

                Next

            End With

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1081" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1114" Then '---- S.Ravichandran Textiles (Erode)

                cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code,         Company_IdNo                       ,           Reference_No        ,                               for_OrderBy                              , Reference_Date, DeliveryTo_Idno                                            ,  ReceivedFrom_Idno ,         Entry_ID     ,       Party_Bill_No  ,       Particulars      ,  Sl_No      , Cloth_Idno        , Pcs ,    Meters_Type1          ,StockOff_IdNo                                                    , Weight                      , Bundle                            ,Colour_IdNo        ,Process_IdNo    , Lot_Idno      ) " & _
                                                  " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvNo.Text))) & ",  @InvoiceDate     , " & Str(Val(Led_ID)) & ", " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & "                , '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 2       ," & Str(Fb_ID) & " , " & Str(Val(vTotPcs)) & " , " & Str(Val(vTotMtrs)) & ",    " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & "     ," & Str(Val(vTotWgt)) & "," & Str(Val(vTotPcs)) & "," & Str(Col_ID) & "," & Str(Proc_ID) & ", " & Str(Lot_ID) & ") "
                cmd.ExecuteNonQuery()

            Else
                If cbo_RollBundle.Text = "ROLL" Then
                    cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code,         Company_IdNo                       ,           Reference_No        ,                               for_OrderBy                              , Reference_Date, DeliveryTo_Idno                                            ,  ReceivedFrom_Idno ,         Entry_ID     ,       Party_Bill_No  ,       Particulars      ,  Sl_No      , Cloth_Idno        ,   Meters_Type2           ,StockOff_IdNo                                                  , Weight              , Rolls                            ,Colour_IdNo        ,Process_IdNo      ) " & _
                                                  " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvNo.Text))) & ",  @InvoiceDate     , " & Str(Val(Led_ID)) & ",  " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & "               , '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 2           ," & Str(Fb_ID) & " , " & Str(Val(vTotMtrs)) & ",    " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & "     ," & Str(Val(vTotWgt)) & "," & Str(Val(vTotPcs)) & "," & Str(Col_ID) & "," & Str(Proc_ID) & ") "
                    cmd.ExecuteNonQuery()
                Else
                    cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code,         Company_IdNo                       ,           Reference_No        ,                               for_OrderBy                              , Reference_Date, DeliveryTo_Idno                                            ,  ReceivedFrom_Idno ,         Entry_ID     ,       Party_Bill_No  ,       Particulars      ,  Sl_No      , Cloth_Idno        ,   Meters_Type3          ,StockOff_IdNo                                                    , Weight                      , Bundle                            ,Colour_IdNo        ,Process_IdNo          ) " & _
                                                  " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvNo.Text))) & ",  @InvoiceDate     , " & Str(Val(Led_ID)) & ", " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & "                , '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', 2           ," & Str(Fb_ID) & " , " & Str(Val(vTotMtrs)) & ",    " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & "     ," & Str(Val(vTotWgt)) & "," & Str(Val(vTotPcs)) & "," & Str(Col_ID) & "," & Str(Proc_ID) & ") "
                    cmd.ExecuteNonQuery()
                End If
            End If


            With dgv_BaleSelectionDetails

                Sno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(3).Value) <> 0 And Trim(.Rows(i).Cells(5).Value) <> "" Then

                        cmd.CommandText = "Update Processed_Fabric_inspection_Details set Sales_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "', SalesInvoice_DetailsSlNo = " & Str(Val(.Rows(i).Cells(0).Value)) & ", Sales_Invoice_Increment = Sales_Invoice_Increment + 1 Where Roll_Code = '" & Trim(.Rows(i).Cells(5).Value) & "'"
                        cmd.ExecuteNonQuery()

                    End If

                Next i

            End With

            cmd.CommandText = "Delete from Processed_Fabric_Invoice_BaleEntry_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Sales_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Direct_BaleDetails

                Sno = 0
                For i = 0 To .Rows.Count - 1

                    If Val(.Rows(i).Cells(3).Value) <> 0 Then

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into Processed_Fabric_Invoice_BaleEntry_Details   ( Sales_Invoice_Code  ,               Company_IdNo             ,      Sales_Invoice_No    ,                               for_OrderBy                              , Sales_Invoice_Date       ,         Ledger_IdNo     ,            Sl_No     ,                    Pack_No          ,                         Pcs                 ,                      Meters              ,                      Weight                ) " & _
                                                "     Values                                  (   '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvNo.Text))) & ",       @InvoiceDate     , " & Str(Val(Led_ID)) & ", " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', " & Str(Val(.Rows(i).Cells(2).Value)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & "  ) "
                        cmd.ExecuteNonQuery()

                    End If

                Next i

            End With
            '---Tax Details
            cmd.CommandText = "Delete from Processed_Fabric_SalesInvoice_GST_Tax_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ProcessedFabric_Sales_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Tax_Details

                Sno = 0
                For i = 0 To .Rows.Count - 1

                    If Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(5).Value) <> 0 Or Val(.Rows(i).Cells(7).Value) <> 0 Then

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into Processed_Fabric_SalesInvoice_GST_Tax_Details   ( ProcessedFabric_Sales_Invoice_Code  ,               Company_IdNo       ,      ProcessedFabric_Sales_Invoice_No    ,                               for_OrderBy                              , ProcessedFabric_Sales_Invoice_Date  ,         Ledger_IdNo     ,            Sl_No     , HSN_Code                               ,Taxable_Amount                            ,CGST_Percentage                           ,CGST_Amount                               ,SGST_Percentage                            ,SGST_Amount                              ,IGST_Percentage                          ,IGST_Amount ) " & _
                                                "     Values                                  (   '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvNo.Text))) & ",       @InvoiceDate     , " & Str(Val(Led_ID)) & ", " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', " & Str(Val(.Rows(i).Cells(2).Value)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & "," & Str(Val(.Rows(i).Cells(5).Value)) & "  ," & Str(Val(.Rows(i).Cells(6).Value)) & "," & Str(Val(.Rows(i).Cells(7).Value)) & "," & Str(Val(.Rows(i).Cells(8).Value)) & ") "
                        cmd.ExecuteNonQuery()

                    End If

                Next i

            End With

            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
            Dim AcPos_ID As Integer = 0

            If OnAc_ID <> 0 Then
                AcPos_ID = OnAc_ID
            Else
                AcPos_ID = Led_ID
            End If

            'If Format(Val(CSng(lbl_CGST_Amount.Text)), "#############0.00") = 0 Then lbl_CGST_Amount.Text = "0.00"
            'If Format(Val(CSng(lbl_SGST_Amount.Text)), "#############0.00") = 0 Then lbl_SGST_Amount.Text = "0.00"
            'If Format(Val(CSng(lbl_IGST_Amount.Text)), "#############0.00") = 0 Then lbl_IGST_Amount.Text = "0.00"

            Dim vNetAmt As Double = Format(Val(CSng(lbl_Net_Amt.Text)), "#############0.00")
            Dim vCGSTAmt As Double = Format(Val(CSng(lbl_CGST_Amount.Text)), "#############0.00")
            Dim vSGSTAmt As Double = Format(Val(CSng(lbl_SGST_Amount.Text)), "#############0.00")
            Dim vIGSTAmt As Double = Format(Val(CSng(lbl_IGST_Amount.Text)), "#############0.00")

            '---GST
            vLed_IdNos = AcPos_ID & "|" & SlAc_ID & "|" & "24|25|26"

            vVou_Amts = -1 * vNetAmt & "|" & vNetAmt - (vCGSTAmt + vSGSTAmt + vIGSTAmt) & "|" & vCGSTAmt & "|" & vSGSTAmt & "|" & vIGSTAmt


            'vLed_IdNos = AcPos_ID & "|" & SlAc_ID & "|" & TxAc_ID

            'vVou_Amts = -1 * Val(CSng(lbl_Net_Amt.Text)) & "|" & (Val(CSng(lbl_Net_Amt.Text)) - Val(CSng(lbl_TaxAmount.Text))) & "|" & Val(CSng(lbl_TaxAmount.Text))

            If Common_Procedures.Voucher_Updation(Con, "GST.Clo.Sale", Val(lbl_Company.Tag), Trim(NewCode), Trim(lbl_InvNo.Text), Convert.ToDateTime(msk_Date.Text), dgv_Details.Rows(0).Cells(1).Value & " -" & Trim(Format(Val(vTotMtrs), "#########0.00")) & " X" & dgv_Details.Rows(0).Cells(9).Value & " -" & vTotNoRls & " -" & dgv_Details.Rows(0).Cells(6).Value, vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

            Common_Procedures.Voucher_Deletion(Con, Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), tr)
            Common_Procedures.Voucher_Deletion(Con, Val(lbl_Company.Tag), Trim(Pk_Condition4) & Trim(NewCode), tr)


            Comm_Amt = 0
            ag_Comm = 0
            agtds_perc = 0

            agtds_perc = Val(Common_Procedures.get_FieldValue(Con, "Ledger_HEAD", "Tds_Percentage", "(Ledger_IdNo = " & Str(Val(Agt_Idno)) & ")", , tr))
            If Val(agtds_perc) <> 0 Then
                Comm_Amt = Val(txt_CommAmt.Text)
                ag_Comm = Val(txt_CommAmt.Text) * agtds_perc / 100
                '   Comm_Amt = Comm_Amt - ag_Comm

            Else
                Comm_Amt = Val(txt_CommAmt.Text)
                ag_Comm = 0

            End If

            vLed_IdNos = Agt_Idno & "|" & Val(Common_Procedures.CommonLedger.Agent_Commission_Ac)
            vVou_Amts = Val(txt_CommAmt.Text) & "|" & -1 * Val(txt_CommAmt.Text)
            If Common_Procedures.Voucher_Updation(Con, "GST.Ag.Comm", Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), Trim(lbl_InvNo.Text), Convert.ToDateTime(msk_Date.Text), "Inv No : " & Trim(lbl_InvNo.Text) & ", Mtrs : " & Trim(Format(Val(vTotMtrs), "#########0.00")), vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

            'vLed_IdNos = Agt_Idno & "|" & Val(Common_Procedures.CommonLedger.TDS_Payable_Ac)
            'vVou_Amts = -1 * Val(ag_Comm) & "|" & Val(ag_Comm)
            'If Common_Procedures.Voucher_Updation(Con, "GST.Agnt.Tds", Val(lbl_Company.Tag), Trim(Pk_Condition4) & Trim(PkCode), Trim(lbl_InvNo.Text), Convert.ToDateTime(msk_Date.Text), "Inv No : " & Trim(lbl_InvNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr,Common_Procedures.SoftwareTypes.Textile_Software) = False Then
            '    Throw New ApplicationException(ErrMsg)
            '    Exit Sub
            'End If

            Dim VouBil As String = ""
            VouBil = Common_Procedures.VoucherBill_Posting(Con, Val(lbl_Company.Tag), Convert.ToDateTime(msk_Date.Text), AcPos_ID, Trim(lbl_InvNo.Text), Agt_Idno, Val(CSng(lbl_Net_Amt.Text)), "DR", Trim(Pk_Condition) & Trim(NewCode), tr, Common_Procedures.SoftwareTypes.Textile_Software)
            If Trim(UCase(VouBil)) = "ERROR" Then
                Throw New ApplicationException("Error on Voucher Bill Posting")
            End If

            tr.Commit()

            If SaveAll_STS <> True Then
                MessageBox.Show("Saved Successfully!!!", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_InvNo.Text)
                End If
            Else
                move_record(lbl_InvNo.Text)
            End If

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Dt1.Dispose()
            Da.Dispose()
            cmd.Dispose()
            tr.Dispose()

        End Try

    End Sub

    Private Sub AgentCommision_Calculation()
        Dim tlamt As Single
        Dim tlmtr As Single
        With dgv_Details_Total


            tlamt = 0
            tlmtr = 0
            With dgv_Details_Total
                If .Rows.Count > 0 Then
                    tlamt = (Val(.Rows(0).Cells(9).Value))
                    tlmtr = (Val(.Rows(0).Cells(7).Value))

                End If
            End With

            If Trim(UCase(cbo_Com_Type.Text)) = "MTR" Then
                txt_CommAmt.Text = Format(Val(tlmtr) * Val(txt_com_per.Text), "########0.00")

            Else
                txt_CommAmt.Text = Format(Val(tlamt) * Val(txt_com_per.Text) / 100, "########0.00")

            End If

        End With
    End Sub

    Private Sub NetAmount_Calculation()
        'Dim GrsAmt As Double
        'Dim NtAmt As Double


        'If NoCalc_Status = True Then Exit Sub

        'GrsAmt = 0

        'With dgv_Details_Total
        '    If .Rows.Count > 0 Then
        '        GrsAmt = Val(.Rows(0).Cells(11).Value)
        '    End If
        'End With

        ''If Val(txt_Trade_Disc.Text) <> 0 Then
        'lbl_Trade_Disc_Perc.Text = Format(Val(GrsAmt) * Val(txt_Trade_Disc.Text) / 100, "########0.00")
        '' End If
        ''  If Val(txt_Cash_Disc.Text) <> 0 Then
        'lbl_Cash_Disc_Perc.Text = Format(Val(GrsAmt) * Val(txt_Cash_Disc.Text) / 100, "########0.00")
        ''  End If

        'lbl_AssessableValue.Text = Val(GrsAmt) + Val(txt_Insurance.Text) + Val(txt_Freight.Text) + Val(txt_Packing.Text) - Val(lbl_Trade_Disc_Perc.Text) - Val(lbl_Cash_Disc_Perc.Text)

        'lbl_CGST_Amount.Text = Val(lbl_TaxableValue.Text) * Val(txt_CGST_Perc.Text) / 100
        'lbl_SGST_Amount.Text = Val(lbl_TaxableValue.Text) * Val(txt_SGST_Perc.Text) / 100

        'NtAmt = Val(lbl_AssessableValue.Text) + Val(lbl_CGST_Amount.Text) + Val(lbl_SGST_Amount.Text)

        ''NtAmt = Val(GrsAmt) + Val(txt_Insurance.Text) + Val(txt_Freight.Text) + Val(txt_Packing.Text) - Val(lbl_Trade_Disc_Perc.Text) - Val(lbl_Cash_Disc_Perc.Text)

        'lbl_Net_Amt.Text = Format(Val(NtAmt), "#########0")

        'lbl_Net_Amt.Text = Common_Procedures.Currency_Format(Val(CSng(lbl_Net_Amt.Text)))

        Dim GrsAmt As Double = 0
        Dim AssVal As Double = 0
        Dim NtAmt As Double = 0
        Dim Tax_Amt As Double = 0

        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

        GrsAmt = 0

        With dgv_Details_Total
            If .Rows.Count > 0 Then
                GrsAmt = Val(.Rows(0).Cells(11).Value)
            End If
        End With

        lbl_Trade_Disc_Perc.Text = Format(Val(GrsAmt) * Val(txt_Trade_Disc.Text) / 100, "########0.00")

        lbl_Cash_Disc_Perc.Text = Format(Val(GrsAmt) * Val(txt_Cash_Disc.Text) / 100, "########0.00")

        AssVal = Val(GrsAmt) - Val(lbl_Trade_Disc_Perc.Text) - Val(lbl_Cash_Disc_Perc.Text) + (Val(txt_Insurance.Text) + Val(txt_Freight.Text) + Val(txt_Packing.Text))

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1139" Then '---- SIVAKUMAR Textiles (THEKKALUR)
            lbl_AssessableValue.Text = Format(Val(AssVal), "#########0")
        Else
            lbl_AssessableValue.Text = Format(Val(AssVal), "#########0.00")
        End If

        Tax_Amt = Val(lbl_CGST_Amount.Text) + Val(lbl_SGST_Amount.Text) + Val(lbl_IGST_Amount.Text)

        NtAmt = Val(GrsAmt) - Val(lbl_Trade_Disc_Perc.Text) - Val(lbl_Cash_Disc_Perc.Text) + Val(txt_Insurance.Text) + Val(txt_Freight.Text) + Val(txt_Packing.Text) + Tax_Amt

        lbl_Net_Amt.Text = Format(Val(NtAmt), "#########0")

        lbl_Net_Amt.Text = Common_Procedures.Currency_Format(Val(CSng(lbl_Net_Amt.Text)))

    End Sub

    Private Sub Amount_Calculation(ByVal CurRow As Integer, ByVal CurCol As Integer)
        Dim fldmtr As Double = 0
        Dim fmt As Double = 0
        Dim CloID As Integer
        Dim ConsYarn As Single
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt2 As New DataTable
        Dim StkIn_For As String = ""
        Dim mtr_pcs As Single = 0
        On Error Resume Next

        With dgv_Details
            If .Visible Then

                If CurCol = 4 Or CurCol = 6 Or CurCol = 7 Or CurCol = 8 Or CurCol = 9 Then

                    .Rows(CurRow).Cells(11).Value = Format(Val(.Rows(CurRow).Cells(7).Value) * Val(.Rows(CurRow).Cells(9).Value), "#########0.00")

                End If

                Total_Calculation()

            End If
        End With
    End Sub

    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotPcs As Single
        Dim TotNoRls As Single
        Dim TotMtrs As Double
        Dim TotAmt As Double
        Dim TotWgt As Double
        Dim TotCurny As Double
        Dim Ttl_TradeDisc As Double, Ttl_CashDisc As Double, Ttl_Taxable_Amount As Double

        If NoCalc_Status = True Then Exit Sub

        Sno = 0
        TotPcs = 0 : TotCurny = 0 : TotMtrs = 0 : TotAmt = 0 : TotWgt = 0 : TotNoRls = 0

        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(7).Value) <> 0 Then

                    TotNoRls = TotNoRls + Val(.Rows(i).Cells(4).Value())
                    TotPcs = TotPcs + Val(.Rows(i).Cells(6).Value())
                    TotMtrs = TotMtrs + Val(.Rows(i).Cells(7).Value())
                    TotWgt = TotWgt + Val(.Rows(i).Cells(8).Value())
                    ' TotCurny = TotCurny + Val(.Rows(i).Cells(10).Value())
                    TotAmt = TotAmt + Val(.Rows(i).Cells(11).Value())

                    Ttl_TradeDisc = Ttl_TradeDisc + Val(.Rows(i).Cells(14).Value())
                    Ttl_CashDisc = Ttl_CashDisc + Val(.Rows(i).Cells(16).Value())
                    Ttl_Taxable_Amount = Ttl_Taxable_Amount + Val(.Rows(i).Cells(18).Value())
                End If

            Next i

        End With


        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(4).Value = Val(TotNoRls)
            .Rows(0).Cells(6).Value = Val(TotPcs)
            .Rows(0).Cells(7).Value = Format(Val(TotMtrs), "########0.00")
            .Rows(0).Cells(8).Value = Format(Val(TotWgt), "########0.000")
            ' .Rows(0).Cells(10).Value = Format(Val(TotCurny), "########0.00")
            .Rows(0).Cells(11).Value = Format(Val(TotAmt), "########0.00")


            .Rows(0).Cells(14).Value = Format(Val(Ttl_TradeDisc), "########0.00")
            .Rows(0).Cells(16).Value = Format(Val(Ttl_CashDisc), "########0.00")
            .Rows(0).Cells(18).Value = Format(Val(Ttl_Taxable_Amount), "########0")
        End With




        AgentCommision_Calculation()


        GST_Calculation()
        'Amount_Calculation(dgv_Details.CurrentCell.RowIndex, dgv_Details.CurrentCell.ColumnIndex)
        NetAmount_Calculation()

    End Sub

    Private Sub GraceTime_Calculation()

        msk_GrDate.Text = ""
        If IsDate(msk_Date.Text) = True And Val(txt_GrTime.Text) >= 0 Then
            msk_GrDate.Text = DateAdd("d", Val(txt_GrTime.Text), Convert.ToDateTime(msk_Date.Text))
        End If

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

                        .Rows(RowIndx).Cells(14).Value = ""
                        .Rows(RowIndx).Cells(15).Value = ""
                        .Rows(RowIndx).Cells(16).Value = ""
                        .Rows(RowIndx).Cells(17).Value = ""
                        .Rows(RowIndx).Cells(18).Value = ""  ' Taxable value
                        .Rows(RowIndx).Cells(19).Value = ""  ' GST %
                        .Rows(RowIndx).Cells(20).Value = ""  ' HSN code

                        If Trim(.Rows(RowIndx).Cells(1).Value) <> "" Or Val(.Rows(RowIndx).Cells(6).Value) = 0 Or Val(.Rows(RowIndx).Cells(7).Value) = 0 Or Val(.Rows(RowIndx).Cells(9).Value) = 0 Then

                            HSN_Code = ""
                            GST_Per = 0
                            Get_GST_Percentage_From_ClothGroup(Trim(.Rows(RowIndx).Cells(1).Value), HSN_Code, GST_Per)

                            'CGST_Per = GST_Per / 2
                            'SGST_Per = GST_Per / 2
                            'IGST_Per = GST_Per

                            '--trade discount
                            .Rows(RowIndx).Cells(14).Value = Format(Val(txt_Trade_Disc.Text), "########0.00")
                            .Rows(RowIndx).Cells(15).Value = Format(Val(.Rows(RowIndx).Cells(10).Value) * (Val(.Rows(RowIndx).Cells(14).Value) / 100), "########0.00")

                            '--Cash discount
                            .Rows(RowIndx).Cells(16).Value = Format(Val(txt_Cash_Disc.Text), "########0.00")
                            .Rows(RowIndx).Cells(17).Value = Format(Val(.Rows(RowIndx).Cells(10).Value) * (Val(.Rows(RowIndx).Cells(16).Value) / 100), "########0.00")

                            '-- Taxable value = amount - (trade disc + cash disc)

                            Taxable_Amount = Val(.Rows(RowIndx).Cells(11).Value) - (Val(.Rows(RowIndx).Cells(15).Value) + Val(.Rows(RowIndx).Cells(17).Value))

                            ''--packing and only added to first row
                            'If RowIndx = 0 Then
                            '    Taxable_Amount = Taxable_Amount + Val(txt_Packing.Text) + Val(txt_Insurance.Text) + Val(txt_Freight.Text)
                            'End If

                            .Rows(RowIndx).Cells(18).Value = Format(Val(Taxable_Amount), "##########0.00")
                            .Rows(RowIndx).Cells(19).Value = Format(Val(GST_Per), "########0.00")
                            .Rows(RowIndx).Cells(20).Value = Trim(HSN_Code)

                        End If

                    Next RowIndx

                    Get_HSN_CodeWise_Tax_Details()

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DO GST CALCULATION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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

            cmd.Connection = Con

            cmd.CommandText = "Truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
            cmd.ExecuteNonQuery()

            If chk_GSTTax_Invocie.Checked = True Then

                AssVal_Pack_Frgt_Ins_Amt = Format(Val(txt_Packing.Text) + Val(txt_Insurance.Text) + Val(txt_Freight.Text), "#########0.00")

                With dgv_Details

                    If .Rows.Count > 0 Then
                        For i = 0 To .Rows.Count - 1
                            If Trim(.Rows(i).Cells(1).Value) <> "" And Val(.Rows(i).Cells(20).Value) <> 0 And Trim(.Rows(i).Cells(19).Value) <> "" Then
                                cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & " (                    Name1                ,                  Currency1            ,                       Currency2                                             ) " & _
                                                    "          Values     ( '" & Trim(.Rows(i).Cells(20).Value) & "', " & Val(.Rows(i).Cells(19).Value) & " ,  " & Str(Val(.Rows(i).Cells(18).Value) + Val(AssVal_Pack_Frgt_Ins_Amt)) & " ) "
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

                da = New SqlClient.SqlDataAdapter("select Name1 as HSN_Code, Currency1 as GST_Percentage, sum(Currency2) as TaxableAmount from " & Trim(Common_Procedures.EntryTempTable) & " group by name1, Currency1 Having sum(Currency2) <> 0 order by Name1, Currency1", Con)
                dt = New DataTable
                da.Fill(dt)

                If dt.Rows.Count > 0 Then

                    Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(Con, cbo_PartyName.Text)
                    InterStateStatus = Common_Procedures.Is_InterState_Party(Con, Val(lbl_Company.Tag), Led_IdNo)

                    For i = 0 To dt.Rows.Count - 1

                        n = .Rows.Add()

                        Sno = Sno + 1

                        .Rows(n).Cells(0).Value = Sno
                        .Rows(n).Cells(1).Value = dt.Rows(i).Item("HSN_Code").ToString

                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1139" Then '---- SIVAKUMAR Textiles (THEKKALUR)
                            .Rows(n).Cells(2).Value = Format(Val(dt.Rows(i).Item("TaxableAmount").ToString), "############0")
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

                            .Rows(n).Cells(4).Value = Format(Val(.Rows(n).Cells(2).Value) * Val(.Rows(n).Cells(3).Value) / 100, "#############0")
                            If Val(.Rows(n).Cells(4).Value) = 0 Then .Rows(n).Cells(4).Value = ""

                            .Rows(n).Cells(6).Value = Format(Val(.Rows(n).Cells(2).Value) * Val(.Rows(n).Cells(5).Value) / 100, "#############0")
                            If Val(.Rows(n).Cells(6).Value) = 0 Then .Rows(n).Cells(6).Value = ""

                            .Rows(n).Cells(8).Value = Format(Val(.Rows(n).Cells(2).Value) * Val(.Rows(n).Cells(7).Value) / 100, "#############0")
                            If Val(.Rows(n).Cells(8).Value) = 0 Then .Rows(n).Cells(8).Value = ""

                        Else

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

                        End If


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

        lbl_AssessableValue.Text = Format(Val(TotAss_Val), "##########0.00")
        lbl_CGST_Amount.Text = Format(Val(TotCGST_amt), "###########0.00")
        lbl_SGST_Amount.Text = Format(Val(TotSGST_amt), "###########0.00")
        lbl_IGST_Amount.Text = Format(Val(TotIGST_amt), "###########0.00")

        'lbl_CGST_Amount.Text = IIf(Val(TotCGST_amt) <> 0, Format(Val(lbl_CGST_Amount.Text), "##########0.00"), "")
        'lbl_SGST_Amount.Text = IIf(Val(TotSGST_amt) <> 0, Format(Val(lbl_SGST_Amount.Text), "##########0.00"), "")
        'lbl_IGST_Amount.Text = IIf(Val(TotIGST_amt) <> 0, Format(Val(lbl_IGST_Amount.Text), "##########0.00"), "")

    End Sub
    Private Sub Get_GST_Percentage_From_ClothGroup(ByVal ClothName As String, ByRef HSN_Code As String, ByRef GST_PerCent As Single)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Try

            HSN_Code = ""
            GST_PerCent = 0

            da = New SqlClient.SqlDataAdapter("select a.* from ItemGroup_Head a INNER JOIN Cloth_Head b ON a.ItemGroup_IdNo = b.ItemGroup_IdNo Where b.Cloth_Name ='" & Trim(ClothName) & "'", Con)
            dt = New DataTable
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0).Item("Item_HSN_Code").ToString) = False Then
                    HSN_Code = Trim(dt.Rows(0).Item("Item_HSN_Code").ToString)
                End If
                If IsDBNull(dt.Rows(0).Item("Item_GST_Percentage").ToString) = False Then
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

    Private Sub Get_State_Code(ByVal Ledger_IDno As Integer, ByRef Ledger_State_Code As String, ByRef Company_State_Code As String)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Try

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

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try
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


    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "Ledger_AlaisHead", "Ledger_DisplayName", " ( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")

    End Sub
    Private Sub cbo_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_PartyName, msk_Date, cbo_Type, "Ledger_AlaisHead", "Ledger_DisplayName", " ( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_PartyName, cbo_Type, "Ledger_AlaisHead", "Ledger_DisplayName", " ( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Agent, msk_OrderDate, txt_com_per, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Agent_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Agent.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Agent, txt_com_per, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Agent_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.MDI_LedType = "AGENT"
            Dim f As New Agent_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Agent.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub dtp_FromDate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        msk_OrderDate.Text = dtp_OrderDate.Text
    End Sub

    Private Sub dtp_FromDate_Enter(ByVal sender As Object, ByVal e As System.EventArgs)
        msk_OrderDate.Focus()
        msk_OrderDate.SelectionStart = 0
    End Sub

    Private Sub cbo_Type_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Type, cbo_PartyName, Nothing, "", "", "", "")
        'If (e.KeyCode = 40) Then
        '    If cbo_Type.Text = "ORDER" Then
        '        If MessageBox.Show("Do you want to select Cloth Receipt :", "FOR CLOTH RECEIPT SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
        '            btn_Selection_Click(sender, e)
        '        End If
        '    ElseIf cbo_Type.Text = "DELIVERY" Then

        '        If MessageBox.Show("Do you want to select Delivery Receipt :", "FOR CLOTH DELIVERY SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
        '            btn_Selection_Click(sender, e)
        '        End If

        '    Else

        '        txt_OrderNo.Focus()

        '    End If
        'End If
    End Sub

    Private Sub cbo_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Type, txt_ElectronicRefNo, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            'If cbo_Type.Text = "ORDER" Then
            '    If MessageBox.Show("Do you want to select Cloth Receipt :", "FOR CLOTH RECEIPT SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
            '        btn_Selection_Click(sender, e)
            '    End If
            'ElseIf cbo_Type.Text = "DELIVERY" Then

            '    If MessageBox.Show("Do you want to select Delivery Receipt :", "FOR CLOTH DELIVERY SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
            '        btn_Selection_Click(sender, e)
            '    End If

            'Else

            txt_OrderNo.Focus()

        End If

        ' End If
    End Sub
    Private Sub cbo_Through_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Through.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "", "", "", "")

    End Sub
    Private Sub cbo_Through_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Through.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Through, msk_Lr_Date, txt_GrTime, "", "", "", "")
    End Sub

    Private Sub cbo_Through_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Through.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Through, txt_GrTime, "", "", "", "")
    End Sub
    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

    End Sub
    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Transport, cbo_DeliveryTo, txt_Vechile, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, txt_Vechile, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
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

    Private Sub cbo_Grid_ClothName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_ClothName.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "Cloth_Head", "Cloth_Name", "(Cloth_Type = 'PROCESSED FABRIC')", "(Cloth_IdNo = 0)")

    End Sub
    Private Sub cbo_Grid_ClothName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_ClothName.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Grid_ClothName, Nothing, Nothing, "Cloth_Head", "Cloth_Name", "(Cloth_Type = 'PROCESSED FABRIC')", "(Cloth_IdNo = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_ClothName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If .CurrentCell.RowIndex <= 0 Then
                    cbo_RollBundle.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(11)
                    .CurrentCell.Selected = True
                End If

            End If

            If (e.KeyValue = 40 And cbo_Grid_ClothName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    txt_Trade_Disc.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    .CurrentCell.Selected = True

                End If

            End If

        End With
    End Sub

    Private Sub cbo_Grid_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_ClothName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim cLTH_Idno As Integer = 0
        Dim rATE As Single = 0
        Dim trpt_Idno As Integer = 0

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Grid_ClothName, cbo_Grid_Colour, "Cloth_Head", "Cloth_Name", "(Cloth_Type = 'PROCESSED FABRIC')", "(Cloth_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            'e.Handled = True

            'cLTH_Idno = Common_Procedures.Cloth_NameToIdNo(Con, Trim(cbo_Grid_ClothName.Text))

            'da = New SqlClient.SqlDataAdapter("select a.* from cLOTH_hEAD a where a.cLOTH_idno = " & Str(Val(cLTH_Idno)) & "  ", Con)
            'dt = New DataTable
            'da.Fill(dt)

            'rATE = 0

            'If dt.Rows.Count > 0 Then
            '    If IsDBNull(dt.Rows(0)(0).ToString) = False Then
            '        rATE = Val(dt.Rows(0).Item("Sound_Rate").ToString)
            '    End If
            'End If

            'dt.Dispose()
            'da.Dispose()

            'If Val(rATE) <> 0 Then
            '    With dgv_Details
            '        If Val(.Rows(.CurrentRow.Index).Cells(8).Value) = 0 Then
            '            .Rows(.CurrentRow.Index).Cells(8).Value = rATE
            '        End If
            '    End With
            'End If

            With dgv_Details
                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    txt_Trade_Disc.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(2)

                End If
            End With

        End If
    End Sub

    Private Sub cbo_Grid_ClothName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_ClothName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_ClothName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Grid_Colour_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Colour.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")

    End Sub
    Private Sub cbo_Grid_COLOUR_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Colour.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Grid_Colour, Nothing, Nothing, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")
        vcbo_KeyDwnVal = e.KeyValue

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Colour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_Colour.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With

    End Sub

    Private Sub cbo_Grid_Colour_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Colour.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Grid_Colour, Nothing, "Colour_Head", "Colour_Name", "", "(Colour_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details
                e.Handled = True
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
            End With

        End If

    End Sub

  

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim Rect As Rectangle

        With dgv_Details

            If Val(.Rows(e.RowIndex).Cells(12).Value) = 0 Then
                Set_Max_DetailsSlNo(e.RowIndex, 12)
                'If e.RowIndex = 0 Then
                '    .Rows(e.RowIndex).Cells(15).Value = 1
                'Else
                '    .Rows(e.RowIndex).Cells(15).Value = Val(.Rows(e.RowIndex - 1).Cells(15).Value) + 1
                'End If
            End If

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            'If Trim(.CurrentRow.Cells(2).Value) = "" Then
            '    .CurrentRow.Cells(2).Value = Common_Procedures.ClothType_IdNoToName(con, 1)
            'End If

            'If Val(.CurrentRow.Cells(3).Value) = 0 Then
            '    .CurrentRow.Cells(3).Value = "100"
            'End If

            If e.ColumnIndex = 1 Then

                If cbo_Grid_ClothName.Visible = False Or Val(cbo_Grid_ClothName.Tag) <> e.RowIndex Then

                    cbo_Grid_ClothName.Tag = -100
                    Da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head where Cloth_Type = 'PROCESSED_FABRIC' order by Cloth_Name", Con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_ClothName.DataSource = Dt1
                    cbo_Grid_ClothName.DisplayMember = "Cloth_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_ClothName.Left = .Left + rect.Left
                    cbo_Grid_ClothName.Top = .Top + rect.Top

                    cbo_Grid_ClothName.Width = rect.Width
                    cbo_Grid_ClothName.Height = rect.Height
                    cbo_Grid_ClothName.Text = .CurrentCell.Value

                    cbo_Grid_ClothName.Tag = Val(e.RowIndex)
                    cbo_Grid_ClothName.Visible = True

                    cbo_Grid_ClothName.BringToFront()
                    cbo_Grid_ClothName.Focus()

               

                End If

            Else
                cbo_Grid_ClothName.Visible = False

            End If

            If e.ColumnIndex = 2 Then

                If cbo_Grid_Colour.Visible = False Or Val(cbo_Grid_Colour.Tag) <> e.RowIndex Then

                    cbo_Grid_Colour.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Colour_Name from Colour_Head order by Colour_Name", Con)
                    Dt1 = New DataTable
                    Da.Fill(Dt2)
                    cbo_Grid_Colour.DataSource = Dt2
                    cbo_Grid_Colour.DisplayMember = "Colour_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_Colour.Left = .Left + rect.Left
                    cbo_Grid_Colour.Top = .Top + rect.Top

                    cbo_Grid_Colour.Width = rect.Width
                    cbo_Grid_Colour.Height = rect.Height
                    cbo_Grid_Colour.Text = .CurrentCell.Value

                    cbo_Grid_Colour.Tag = Val(e.RowIndex)
                    cbo_Grid_Colour.Visible = True

                    cbo_Grid_Colour.BringToFront()
                    cbo_Grid_Colour.Focus()

            
                End If

            Else
                cbo_Grid_Colour.Visible = False

            End If
            If e.ColumnIndex = 3 Then

                If cbo_Grid_Process.Visible = False Or Val(cbo_Grid_Process.Tag) <> e.RowIndex Then

                    cbo_Grid_Process.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Process_Name from Process_Head order by Process_Name", Con)
                    Dt1 = New DataTable
                    Da.Fill(Dt3)
                    cbo_Grid_Process.DataSource = Dt3
                    cbo_Grid_Process.DisplayMember = "Process_Name"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_Process.Left = .Left + Rect.Left
                    cbo_Grid_Process.Top = .Top + Rect.Top

                    cbo_Grid_Process.Width = Rect.Width
                    cbo_Grid_Process.Height = Rect.Height
                    cbo_Grid_Process.Text = .CurrentCell.Value

                    cbo_Grid_Process.Tag = Val(e.RowIndex)
                    cbo_Grid_Process.Visible = True

                    cbo_Grid_Process.BringToFront()
                    cbo_Grid_Process.Focus()


                End If

            Else
                cbo_Grid_Process.Visible = False

            End If

            If e.ColumnIndex = 10 Then

                If cbo_Grid_Currency.Visible = False Or Val(cbo_Grid_Currency.Tag) <> e.RowIndex Then

                    cbo_Grid_Currency.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Currency_Name from Currency_Head order by Currency_Name", Con)
                    Dt1 = New DataTable
                    Da.Fill(Dt4)
                    cbo_Grid_Currency.DataSource = Dt4
                    cbo_Grid_Currency.DisplayMember = "Currency_Name"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_Currency.Left = .Left + Rect.Left
                    cbo_Grid_Currency.Top = .Top + Rect.Top

                    cbo_Grid_Currency.Width = Rect.Width
                    cbo_Grid_Currency.Height = Rect.Height
                    cbo_Grid_Currency.Text = .CurrentCell.Value

                    cbo_Grid_Currency.Tag = Val(e.RowIndex)
                    cbo_Grid_Currency.Visible = True

                    cbo_Grid_Currency.BringToFront()
                    cbo_Grid_Currency.Focus()


                End If

            Else
                cbo_Grid_Currency.Visible = False

            End If

            If (e.ColumnIndex = 4 Or e.ColumnIndex = 5) And Trim(UCase(cbo_Type.Text)) <> "DELIVERY" Then

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
            If .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 9 Or .CurrentCell.ColumnIndex = 11 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
            If .CurrentCell.ColumnIndex = 8 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged


        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        Try
        With dgv_Details
            If .Visible Then

                    If e.ColumnIndex = 1 Or e.ColumnIndex = 6 Or e.ColumnIndex = 7 Or e.ColumnIndex = 9 Then


                        Amount_Calculation(.CurrentCell.RowIndex, e.ColumnIndex)

                    End If

                If .CurrentCell.ColumnIndex = 4 Then
                    If Val(Common_Procedures.settings.ClothInvoice_Packing_Charge_Per_Bale) <> 0 Then
                        txt_Packing.Text = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) * Val(Common_Procedures.settings.ClothInvoice_Packing_Charge_Per_Bale), "###########0.00")
                        NetAmount_Calculation()
                    End If
                End If

            End If
        End With

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL CHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = Nothing
        If dgv_Details.CurrentCell.ColumnIndex > 2 Then
            dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
        End If
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub

    Private Sub dgtxt_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyDown
        With dgv_Details
            vcbo_KeyDwnVal = e.KeyValue
            If .Visible Then
                If e.KeyValue = Keys.Delete Then
                    If .CurrentCell.ColumnIndex <= 7 And Trim(.Rows(.CurrentCell.RowIndex).Cells(16).Value) <> "" Then
                        e.Handled = True
                        e.SuppressKeyPress = True
                    End If

                    If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Then
                        If Trim(UCase(cbo_Type.Text)) = "ORDER" Or Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then
                            e.Handled = True
                            e.SuppressKeyPress = True
                        End If
                    End If
                End If
            End If
        End With

    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress

        Try
            With dgv_Details
                If .Visible Then

                    'If .Rows.Count > 0 Then

                    '    If .CurrentCell.ColumnIndex <= 7 And Trim(.Rows(.CurrentCell.RowIndex).Cells(16).Value) <> "" Then
                    '        e.Handled = True
                    '        Add_NewRow_ToGrid()
                    '    End If

                    '    If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Then
                    '        If Trim(UCase(cbo_Type.Text)) = "ORDER" Or Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then
                    '            e.Handled = True
                    '        End If
                    '    End If

                    If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Or .CurrentCell.ColumnIndex = 9 Or .CurrentCell.ColumnIndex = 11 Then

                        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                            e.Handled = True
                        End If

                    End If

                    '    End If

                End If
            End With

        Catch ex As Exception
            '---

        End Try

    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp

        Try
            With dgv_Details
                If .Rows.Count > 0 Then
                    If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
                        dgv_Details_KeyUp(sender, e)
                    End If

                    If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
                        If (.CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5) And Trim(UCase(cbo_Type.Text)) <> "DELIVERY" Then
                            btn_BaleSelection_Click(sender, e)
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
    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            With dgv_Details

                'If Val(.Rows(.CurrentCell.RowIndex).Cells(9).Value) = 0 And Val(.Rows(.CurrentCell.RowIndex).Cells(10).Value) = 0 Then

                n = .CurrentRow.Index

                If n = .Rows.Count - 1 Then
                    For i = 0 To .ColumnCount - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(n)

                End If

                Total_Calculation()

                'End If

            End With

        End If

        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    If (dgv_Details.CurrentCell.ColumnIndex = 4 Or dgv_Details.CurrentCell.ColumnIndex = 5) And Trim(UCase(cbo_Type.Text)) <> "DELIVERY" Then
        '        btn_BaleSelection_Click(sender, e)
        '    End If
        'End If

    End Sub

    Private Sub Set_Max_DetailsSlNo(ByVal RowNo As Integer, ByVal DetSlNo_ColNo As Integer)
        Dim MaxSlNo As Integer = 0
        Dim i As Integer

        With dgv_Details
            For i = 0 To .Rows.Count - 1
                If Val(.Rows(i).Cells(DetSlNo_ColNo).Value) > Val(MaxSlNo) Then
                    MaxSlNo = Val(.Rows(i).Cells(DetSlNo_ColNo).Value)
                End If
            Next
            .Rows(RowNo).Cells(DetSlNo_ColNo).Value = Val(MaxSlNo) + 1
        End With

    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer

        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)

            If Val(.Rows(e.RowIndex).Cells(12).Value) = 0 Then
                Set_Max_DetailsSlNo(e.RowIndex, 12)
                'If e.RowIndex = 0 Then
                '    .Rows(e.RowIndex).Cells(15).Value = 1
                'Else
                '    .Rows(e.RowIndex).Cells(15).Value = Val(.Rows(e.RowIndex - 1).Cells(15).Value) + 1
                'End If
            End If

        End With
    End Sub

    Private Sub cbo_Grid_ClothName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_ClothName.TextChanged
        Try
            If cbo_Grid_ClothName.Visible Then
                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

                With dgv_Details
                    If Val(cbo_Grid_ClothName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_ClothName.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub cbo_Grid_COLOUR_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Colour.TextChanged
        Try
            If cbo_Grid_Colour.Visible Then
                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_Grid_Colour.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Colour.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_Grid_Processs_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Process.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Process_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Process.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Grid_Process_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Process.TextChanged
        Try
            If cbo_Grid_Process.Visible Then
                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_Grid_Process.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Process.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim Led_IdNo As Integer, Clth_IdNo As Integer
        Dim Condt As String = ""

        Try

            Condt = ""
            Led_IdNo = 0
            Clth_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Processed_Fabric_Sales_Invoice_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Processed_Fabric_Sales_Invoice_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Processed_Fabric_Sales_Invoice_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_ClothName.Text) <> "" Then
                Clth_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Filter_ClothName.Text)
            End If


            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Ledger_Idno = " & Str(Val(Led_IdNo)) & ")"
            End If

            If Val(Clth_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " b.Fabric_IdNo = " & Str(Val(Clth_IdNo))
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, c.Cloth_Name, d.ClothType_name, e.Ledger_Name from Processed_Fabric_Sales_Invoice_Head a left outer join Processed_Fabric_Sales_Invoice_Details b on a.Processed_Fabric_Sales_Invoice_Code = b.Processed_Fabric_Sales_Invoice_Code left outer join Cloth_head c on b.Fabric_IdNo = c.Cloth_idno left outer join ClothType_head d on b.ClothType_idno = d.ClothType_idno left outer join Ledger_head e on a.Ledger_idno = e.Ledger_idno where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and Tax_Type ='GST' AND a.Processed_Fabric_Sales_Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by Processed_Fabric_Sales_Invoice_Date, for_orderby, Processed_Fabric_Sales_Invoice_No", Con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Processed_Fabric_Sales_Invoice_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Processed_Fabric_Sales_Invoice_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Party_OrderNo").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(6).Value = dt2.Rows(i).Item("ClothType_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Total_Amount").ToString), "########0.00")

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

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(1).Value)

        If Val(movno) <> 0 Then
            Filter_Status = True
            Filter_RowNo = dgv_Filter_Details.CurrentRow.Index
            move_record(movno)
            pnl_Back.Enabled = True
            pnl_Filter.Visible = False
        End If

    End Sub

    Private Sub dgv_Filter_Details_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        dgv_Details_CellLeave(sender, e)
    End Sub

    Private Sub cbo_Filter_ClothName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_ClothName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")

    End Sub
    Private Sub cbo_Filter_ClothName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ClothName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ClothName, cbo_Filter_PartyName, btn_Filter_Show, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_Filter_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ClothName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ClothName, btn_Filter_Show, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub
    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")

    End Sub
    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_ClothName, "Ledger_AlaisHead", "Ledger_DisplayName", " ( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Filter_PartyName, cbo_Filter_ClothName, "Ledger_AlaisHead", "Ledger_DisplayName", " ( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub txt_Trade_Disc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Trade_Disc.KeyDown
        If e.KeyValue = 38 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

            Else
                cbo_RollBundle.Focus()

            End If
        End If
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub msk_LcDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_LcDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyValue = 38 Then txt_LcNo.Focus() ' SendKeys.Send("+{TAB}")

        If (e.KeyValue = 40) Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

            Else
                txt_Trade_Disc.Focus()

            End If
        End If
    End Sub

    Private Sub msk_LcDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_LcDate.KeyPress

        If Asc(e.KeyChar) = 13 Then
            e.Handled = True

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Then '---- Asia Textiles (Tirupur)

                If MessageBox.Show("Do you want to enter Bale Details?", "FOR BALE DETAILS ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    'btn_Direct_BaleDetails_Click(sender, e)

                Else
                    If dgv_Details.Rows.Count > 0 Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                        dgv_Details.CurrentCell.Selected = True

                    Else
                        txt_Trade_Disc.Focus()

                    End If

                End If

            Else

                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                    dgv_Details.CurrentCell.Selected = True

                Else
                    txt_Trade_Disc.Focus()

                End If

            End If

        Else
            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                e.Handled = True
            End If

        End If

    End Sub

    Private Sub txt_ClthDetail_Name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_ClthDetail_Name.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If (e.KeyValue = 40) Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

            Else
                txt_Trade_Disc.Focus()

            End If
        End If
    End Sub

    Private Sub txt_ClthDetail_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ClthDetail_Name.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

            Else
                cbo_Transport.Focus()

            End If
        End If
    End Sub

    Private Sub cbo_OnAcc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_OnAcc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " ( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 or AccountsGroup_IdNo = 6 ) ) or Show_In_All_Entry = 1) ", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_OnAcc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_OnAcc.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_OnAcc, cbo_SalesAcc, cbo_DeliveryTo, "Ledger_AlaisHead", "Ledger_DisplayName", " ( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 or AccountsGroup_IdNo = 6 ) ) or Show_In_All_Entry = 1) ", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_OnAcc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_OnAcc.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_OnAcc, cbo_DeliveryTo, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 or AccountsGroup_IdNo = 6 ) ) or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_OnAcc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_OnAcc.KeyUp
        If e.Control = False And e.KeyValue = 17 Then

            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_OnAcc.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_SalesAcc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SalesAcc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_SalesAcc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SalesAcc.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_SalesAcc, txt_GrTime, cbo_OnAcc, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_SalesAcc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_SalesAcc.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_SalesAcc, cbo_OnAcc, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_SalesAcc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SalesAcc.KeyUp
        If e.Control = False And e.KeyValue = 17 Then

            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_SalesAcc.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Com_Type_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Com_Type.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub

    Private Sub cbo_Com_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Com_Type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Com_Type, txt_com_per, txt_DcNo, "", "", "", "")

    End Sub

    Private Sub cbo_Com_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Com_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Com_Type, txt_DcNo, "", "", "", "")
    End Sub

    Private Sub txt_ClthDetail_Name_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_ClthDetail_Name.LostFocus
        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.LightBlue
                Prec_ActCtrl.ForeColor = Color.Black
            End If
        End If
    End Sub

    Private Sub txt_Cash_Disc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Cash_Disc.KeyPress

        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If

    End Sub

    Private Sub txt_Cash_Disc_Perc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_com_per_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_com_per.KeyPress

        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Comm_Calc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CommAmt.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If

    End Sub

    Private Sub txt_Packing_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Packing.KeyDown
        If e.KeyValue = 38 Then
            txt_Insurance.Focus()
        End If

        If e.KeyValue = 40 Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()

            Else
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1050" Then '---- Kumaravel Textiles (Palladam)
                    If txt_InvoicePrefixNo.Enabled And txt_InvoicePrefixNo.Visible Then txt_InvoicePrefixNo.Focus()
                Else
                    If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
                End If

            End If
        End If
    End Sub

    Private Sub txt_Packing_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Packing.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then


            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1050" Then '---- Kumaravel Textiles (Palladam)
                    If txt_InvoicePrefixNo.Enabled And txt_InvoicePrefixNo.Visible Then txt_InvoicePrefixNo.Focus()
                Else
                    If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
                End If
            End If
        End If

    End Sub

    Private Sub txt_Trade_Disc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Trade_Disc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If

    End Sub

    Private Sub txt_GrTime_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_GrTime.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Insurance_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Insurance.KeyPress

        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If

    End Sub

    Private Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = False
        Printing_Bale_Status = 0
        print_record()
    End Sub

    Private Sub dgv_Filter_Details_CellDoubleClick1(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown1(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter_Details.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub

    Private Sub txt_Trade_Disc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Trade_Disc.TextChanged
        Total_Calculation()
    End Sub

    Private Sub txt_Cash_Disc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Cash_Disc.TextChanged
        Total_Calculation()
    End Sub

    Private Sub txt_Insurance_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Insurance.TextChanged
        Total_Calculation()
    End Sub

    Private Sub txt_Packing_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Packing.TextChanged
        Total_Calculation()
    End Sub

    Private Sub txt_Freight_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Freight.TextChanged
        Total_Calculation()
    End Sub

    Private Sub txt_com_per_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_com_per.TextChanged
        AgentCommision_Calculation()
    End Sub

    Private Sub cbo_Com_Type_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Com_Type.TextChanged
        AgentCommision_Calculation()
    End Sub

    Private Sub txt_GrTime_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_GrTime.TextChanged
        GraceTime_Calculation()
    End Sub



    Private Sub dtp_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_Date.Text = Date.Today
        End If
    End Sub


    Private Sub msk_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_Date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_date.Text = Date.Today
            msk_date.SelectionStart = 0
        End If
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            cbo_PartyName.Focus()
        End If

    End Sub

    Private Sub msk_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_Date.TextChanged
        GraceTime_Calculation()
    End Sub

    Private Sub chk_No_Folding_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim i As Integer = 0

        On Error Resume Next

        With dgv_Details
            If .Visible Then

                For i = 0 To .Rows.Count - 1
                    Amount_Calculation(i, 8)
                Next

            End If
        End With
    End Sub

    '    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
    '        Dim Da As New SqlClient.SqlDataAdapter
    '        Dim Dt1 As New DataTable
    '        Dim Dt2 As New DataTable
    '        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
    '        Dim LedIdNo As Integer
    '        Dim NewCode As String
    '        Dim CompIDCondt As String
    '        Dim Ent_Bls As Single = 0
    '        Dim Ent_BlNos As String = ""
    '        Dim Ent_Pcs As Single = 0
    '        Dim Ent_Mtrs As Single = 0
    '        Dim Ent_ShtMtrs As Single = 0
    '        Dim Ent_Rate As Single = 0
    '        Dim Ent_InvDetSlNo As Long
    '        Dim Ent_PackSlpCodes As String

    '        If Trim(UCase(cbo_Type.Text)) <> "ORDER" And Trim(UCase(cbo_Type.Text)) <> "DELIVERY" Then Exit Sub

    '        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)

    '        If LedIdNo = 0 Then
    '            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT ORDER...", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '            If cbo_PartyName.Enabled And cbo_PartyName.Visible Then cbo_PartyName.Focus()
    '            Exit Sub
    '        End If

    '        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

    '        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
    '        If Trim(UCase(Common_Procedures.settings.CompanyName)) = "-----~~~" Then
    '            CompIDCondt = ""
    '        End If


    '        If Trim(UCase(cbo_Type.Text)) = "ORDER" Then
    '            With dgv_Selection

    '                lbl_Heading_Selection.Text = "ORDER SELECTION"

    '                .Rows.Clear()
    '                SNo = 0

    '                Da = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Cloth_Name, d.Ledger_Name as agentname, e.Ledger_Name as Transportname,  g.ClothType_name, h.Bales as Ent_Bales, h.Bales_Nos as Ent_Bales_Nos, h.Pcs as Ent_Pcs, h.Meters as Ent_DcMeters , h.Rate as Ent_Rate, H.ClothSales_Invoice_SlNo as Ent_ClothSales_Invoice_SlNo, h.PackingSlip_Codes as Ent_PackingSlip_Codes from ClothSales_Order_Head a INNER JOIN Clothsales_Order_details b ON a.ClothSales_Order_Code = b.ClothSales_Order_Code INNER JOIN Cloth_Head c ON b.Cloth_IdNo = c.Cloth_IdNo INNER JOIN ClothType_Head g ON b.ClothType_IdNo = g.ClothType_IdNo LEFT OUTER JOIN Ledger_Head d ON a.Agent_IdNo = d.Ledger_IdNo LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo LEFT OUTER JOIN Processed_Fabric_Sales_Invoice_Details h ON h.Processed_Fabric_Sales_Invoice_Code = '" & Trim(NewCode) & "' and b.ClothSales_Order_Code = h.ClothSales_Order_Code and b.ClothSales_Order_SlNo = h.ClothSales_Order_SlNo Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.ledger_Idno = " & Str(Val(LedIdNo)) & " and ((b.Order_Meters - b.Order_Cancel_Meters - b.Invoice_Meters- b.Delivery_Meters) > 0 or h.Meters > 0 ) order by a.ClothSales_Order_Date, a.for_orderby, a.ClothSales_Order_No", con)
    '                Dt1 = New DataTable
    '                Da.Fill(Dt1)


    '                If Dt1.Rows.Count > 0 Then

    '                    For i = 0 To Dt1.Rows.Count - 1

    '                        n = .Rows.Add()

    '                        Ent_Bls = 0
    '                        Ent_BlNos = ""
    '                        Ent_Pcs = 0
    '                        Ent_Mtrs = 0
    '                        Ent_Rate = 0
    '                        Ent_InvDetSlNo = 0
    '                        Ent_PackSlpCodes = ""

    '                        If IsDBNull(Dt1.Rows(i).Item("Ent_Bales").ToString) = False Then
    '                            Ent_Bls = Val(Dt1.Rows(i).Item("Ent_Bales").ToString)
    '                        End If
    '                        If IsDBNull(Dt1.Rows(i).Item("Ent_Bales_Nos").ToString) = False Then
    '                            Ent_BlNos = Dt1.Rows(i).Item("Ent_Bales_Nos").ToString
    '                        End If
    '                        If IsDBNull(Dt1.Rows(i).Item("Ent_Pcs").ToString) = False Then
    '                            Ent_Pcs = Val(Dt1.Rows(i).Item("Ent_Pcs").ToString)
    '                        End If
    '                        If IsDBNull(Dt1.Rows(i).Item("Ent_DcMeters").ToString) = False Then
    '                            Ent_Mtrs = Val(Dt1.Rows(i).Item("Ent_DcMeters").ToString)
    '                        End If
    '                        If IsDBNull(Dt1.Rows(i).Item("Ent_ClothSales_Invoice_SlNo").ToString) = False Then
    '                            Ent_InvDetSlNo = Val(Dt1.Rows(i).Item("Ent_ClothSales_Invoice_SlNo").ToString)
    '                        End If
    '                        If IsDBNull(Dt1.Rows(i).Item("Ent_PackingSlip_Codes").ToString) = False Then
    '                            Ent_PackSlpCodes = Dt1.Rows(i).Item("Ent_PackingSlip_Codes").ToString
    '                        End If
    '                        If IsDBNull(Dt1.Rows(i).Item("Ent_Rate").ToString) = False Then
    '                            Ent_Rate = Val(Dt1.Rows(i).Item("Ent_Rate").ToString)
    '                        End If

    '                        SNo = SNo + 1
    '                        .Rows(n).Cells(0).Value = Val(SNo)

    '                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("ClothSales_Order_No").ToString
    '                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Party_OrderNo").ToString
    '                        .Rows(n).Cells(3).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("ClothSales_Order_Date").ToString), "dd-MM-yyyy")
    '                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
    '                        .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("ClothType_Name").ToString
    '                        .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Fold_Perc").ToString)
    '                        .Rows(n).Cells(7).Value = Val(Dt1.Rows(i).Item("Order_Pcs").ToString)
    '                        .Rows(n).Cells(8).Value = Format(Val(Dt1.Rows(i).Item("Order_Meters").ToString) - Val(Dt1.Rows(i).Item("Order_Cancel_Meters").ToString) - Val(Dt1.Rows(i).Item("Delivery_Meters").ToString) - Val(Dt1.Rows(i).Item("Invoice_Meters").ToString) + Val(Ent_Mtrs), "#########0.00")

    '                        If Ent_Mtrs > 0 Then
    '                            .Rows(n).Cells(9).Value = "1"
    '                            For j = 0 To .ColumnCount - 1
    '                                .Rows(n).Cells(j).Style.ForeColor = Color.Red
    '                            Next

    '                        Else
    '                            .Rows(n).Cells(9).Value = ""

    '                        End If

    '                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("agentname").ToString
    '                        .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Transportname").ToString
    '                        .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("Through_Name").ToString
    '                        .Rows(n).Cells(13).Value = Dt1.Rows(i).Item("Despatch_To").ToString
    '                        .Rows(n).Cells(14).Value = Dt1.Rows(i).Item("Delivery_Address1").ToString
    '                        .Rows(n).Cells(15).Value = Dt1.Rows(i).Item("Delivery_Address2").ToString
    '                        .Rows(n).Cells(16).Value = Dt1.Rows(i).Item("Agent_Comm_Perc").ToString
    '                        .Rows(n).Cells(17).Value = Dt1.Rows(i).Item("Agent_Comm_Type").ToString
    '                        .Rows(n).Cells(18).Value = Dt1.Rows(i).Item("Clothsales_Order_Code").ToString
    '                        .Rows(n).Cells(19).Value = Dt1.Rows(i).Item("Clothsales_Order_SlNo").ToString
    '                        .Rows(n).Cells(20).Value = Dt1.Rows(i).Item("Rate").ToString

    '                        .Rows(n).Cells(21).Value = Val(Ent_Bls)
    '                        .Rows(n).Cells(22).Value = Ent_BlNos
    '                        .Rows(n).Cells(23).Value = Ent_Pcs
    '                        .Rows(n).Cells(24).Value = Ent_Mtrs
    '                        .Rows(n).Cells(29).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("ClothSales_Order_Date").ToString), "dd-MM-yyyy")
    '                        .Rows(n).Cells(32).Value = Ent_InvDetSlNo
    '                        .Rows(n).Cells(33).Value = Ent_PackSlpCodes
    '                        .Rows(n).Cells(34).Value = Ent_Rate


    '                    Next
    '                End If
    '                Dt1.Clear()

    '                pnl_Selection.Visible = True
    '                pnl_Back.Enabled = False
    '                If .Enabled And .Visible Then
    '                    .Focus()
    '                    If .Rows.Count > 0 Then
    '                        .CurrentCell = .Rows(0).Cells(0)
    '                        .CurrentCell.Selected = True
    '                    End If
    '                End If

    '            End With

    '        ElseIf Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then

    '            With dgv_Selection

    '                lbl_Heading_Selection.Text = "DELIVERY SELECTION"

    '                .Rows.Clear()
    '                SNo = 0

    '                'Da = New SqlClient.SqlDataAdapter("select a.*, b.*, b.Meters as Delivery_Meters, c.Cloth_Name, c.Sound_Rate , d.Ledger_Name as agentname, e.Ledger_Name as Transportname,  g.ClothType_name, h.Bales as Ent_Bales, h.Bales_Nos as Ent_Bales_Nos, h.Pcs as Ent_Pcs,H.mETERS , H.sHORT_mETERS , h.Meters as Ent_DcMeters, h.Short_Meters as Ent_ShortMeters , h.Rate as Ent_Rate, H.ClothSales_Invoice_SlNo as Ent_ClothSales_Invoice_SlNo, h.PackingSlip_Codes as Ent_PackingSlip_Codes from ClothSales_Delivery_Head a INNER JOIN Clothsales_Delivery_details b ON a.ClothSales_Delivery_Code = b.ClothSales_Delivery_Code INNER JOIN Cloth_Head c ON b.Cloth_IdNo = c.Cloth_IdNo INNER JOIN ClothType_Head g ON b.ClothType_IdNo = g.ClothType_IdNo LEFT OUTER JOIN Ledger_Head d ON a.Agent_IdNo = d.Ledger_IdNo LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo LEFT OUTER JOIN Processed_Fabric_Sales_Invoice_Details h ON h.Processed_Fabric_Sales_Invoice_Code = '" & Trim(NewCode) & "' and b.ClothSales_Delivery_Code = h.ClothSales_Delivery_Code and b.ClothSales_Delivery_SlNo = h.ClothSales_Delivery_SlNo Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.ledger_Idno = " & Str(Val(LedIdNo)) & " and ((b.Meters - b.Invoice_Meters) > 0 or h.Meters > 0 ) order by a.ClothSales_Delivery_Date, a.for_orderby, a.ClothSales_Delivery_No", Con)
    '                'Dt1 = New DataTable
    '                'Da.Fill(Dt1)

    '                Da = New SqlClient.SqlDataAdapter("select a.*, b.*, b.Meters as Delivery_Meters, c.Cloth_Name, c.Sound_Rate , d.Ledger_Name as agentname, e.Ledger_Name as Transportname,  g.ClothType_name, h.Bales as Ent_Bales, h.Bales_Nos as Ent_Bales_Nos, h.Pcs as Ent_Pcs,H.fOLD_mETER , H.fOLD_sHORT_mETER , h.Meters as Ent_DcMeters, h.Short_Meters as Ent_ShortMeters , h.Rate as Ent_Rate, H.ClothSales_Invoice_SlNo as Ent_ClothSales_Invoice_SlNo, h.PackingSlip_Codes as Ent_PackingSlip_Codes from ClothSales_Delivery_Head a INNER JOIN Clothsales_Delivery_details b ON a.ClothSales_Delivery_Code = b.ClothSales_Delivery_Code INNER JOIN Cloth_Head c ON b.Cloth_IdNo = c.Cloth_IdNo INNER JOIN ClothType_Head g ON b.ClothType_IdNo = g.ClothType_IdNo LEFT OUTER JOIN Ledger_Head d ON a.Agent_IdNo = d.Ledger_IdNo LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo LEFT OUTER JOIN Processed_Fabric_Sales_Invoice_Details h ON h.Processed_Fabric_Sales_Invoice_Code = '" & Trim(NewCode) & "' and b.ClothSales_Delivery_Code = h.ClothSales_Delivery_Code and b.ClothSales_Delivery_SlNo = h.ClothSales_Delivery_SlNo Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.ledger_Idno = " & Str(Val(LedIdNo)) & " and ((b.Meters - b.Invoice_Meters- b.Return_Meters ) > 0.1 or h.Meters > 0 ) order by a.ClothSales_Delivery_Date, a.for_orderby, a.ClothSales_Delivery_No", Con)
    '                Dt1 = New DataTable
    '                Da.Fill(Dt1)

    '                If Dt1.Rows.Count > 0 Then

    '                    For i = 0 To Dt1.Rows.Count - 1

    '                        n = .Rows.Add()

    '                        Ent_Bls = 0
    '                        Ent_BlNos = ""
    '                        Ent_Pcs = 0
    '                        Ent_Mtrs = 0
    '                        Ent_ShtMtrs = 0
    '                        Ent_Rate = 0
    '                        Ent_InvDetSlNo = 0
    '                        Ent_PackSlpCodes = ""

    '                        If IsDBNull(Dt1.Rows(i).Item("Ent_Bales").ToString) = False Then
    '                            Ent_Bls = Val(Dt1.Rows(i).Item("Ent_Bales").ToString)
    '                        End If
    '                        If IsDBNull(Dt1.Rows(i).Item("Ent_Bales_Nos").ToString) = False Then
    '                            Ent_BlNos = Dt1.Rows(i).Item("Ent_Bales_Nos").ToString
    '                        End If
    '                        If IsDBNull(Dt1.Rows(i).Item("Ent_Pcs").ToString) = False Then
    '                            Ent_Pcs = Val(Dt1.Rows(i).Item("Ent_Pcs").ToString)
    '                        End If
    '                        If IsDBNull(Dt1.Rows(i).Item("Ent_DcMeters").ToString) = False Then
    '                            Ent_Mtrs = Val(Dt1.Rows(i).Item("Ent_DcMeters").ToString)
    '                        End If
    '                        If IsDBNull(Dt1.Rows(i).Item("Ent_ShortMeters").ToString) = False Then
    '                            Ent_ShtMtrs = Val(Dt1.Rows(i).Item("Ent_ShortMeters").ToString)
    '                        End If
    '                        If IsDBNull(Dt1.Rows(i).Item("Ent_ClothSales_Invoice_SlNo").ToString) = False Then
    '                            Ent_InvDetSlNo = Val(Dt1.Rows(i).Item("Ent_ClothSales_Invoice_SlNo").ToString)
    '                        End If
    '                        If IsDBNull(Dt1.Rows(i).Item("Ent_PackingSlip_Codes").ToString) = False Then
    '                            Ent_PackSlpCodes = Dt1.Rows(i).Item("Ent_PackingSlip_Codes").ToString
    '                        End If
    '                        If IsDBNull(Dt1.Rows(i).Item("Ent_Rate").ToString) = False Then
    '                            Ent_Rate = Val(Dt1.Rows(i).Item("Ent_Rate").ToString)
    '                        End If

    '                        SNo = SNo + 1
    '                        .Rows(n).Cells(0).Value = Val(SNo)

    '                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("ClothSales_Delivery_No").ToString
    '                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Party_OrderNo").ToString
    '                        .Rows(n).Cells(3).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("ClothSales_Delivery_Date").ToString), "dd-MM-yyyy")
    '                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
    '                        .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("ClothType_Name").ToString
    '                        .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Fold_Perc").ToString)
    '                        .Rows(n).Cells(7).Value = Val(Dt1.Rows(i).Item("Pcs").ToString)

    '                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then
    '                            ' .Rows(n).Cells(8).Value = Format(Val(Dt1.Rows(i).Item("Meters").ToString) - Val(Dt1.Rows(i).Item("Invoice_Meters").ToString) + Val(Dt1.Rows(i).Item("mETERS").ToString) + Val(Dt1.Rows(i).Item("sHORT_mETERS").ToString), "#########0.00")
    '                            .Rows(n).Cells(8).Value = Format(Val(Dt1.Rows(i).Item("Meters").ToString) - Val(Dt1.Rows(i).Item("Invoice_Meters").ToString) + Val(Dt1.Rows(i).Item("fOLD_mETER").ToString) + Val(Dt1.Rows(i).Item("fOLD_sHORT_mETER").ToString), "#########0.00")

    '                        Else
    '                            .Rows(n).Cells(8).Value = Format(Val(Dt1.Rows(i).Item("Meters").ToString) - Val(Dt1.Rows(i).Item("Invoice_Meters").ToString) + Val(Ent_Mtrs) + Val(Ent_ShtMtrs), "#########0.00")
    '                        End If

    '                        If Ent_Mtrs > 0 Then
    '                            .Rows(n).Cells(9).Value = "1"
    '                            For j = 0 To .ColumnCount - 1
    '                                .Rows(n).Cells(j).Style.ForeColor = Color.Red
    '                            Next

    '                        Else
    '                            .Rows(n).Cells(9).Value = ""

    '                        End If

    '                        .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("agentname").ToString
    '                        .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Transportname").ToString
    '                        .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("Through_Name").ToString
    '                        .Rows(n).Cells(13).Value = Dt1.Rows(i).Item("Despatch_To").ToString
    '                        .Rows(n).Cells(14).Value = Dt1.Rows(i).Item("Delivery_Address1").ToString
    '                        .Rows(n).Cells(15).Value = Dt1.Rows(i).Item("Delivery_Address2").ToString

    '                        Da = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Ledger_Name as agentname from ClothSales_Order_Head a INNER JOIN ClothSales_Order_Details b ON a.Company_IdNo = b.Company_IdNo and a.ClothSales_Order_Code = b.ClothSales_Order_Code LEFT OUTER JOIN Ledger_Head c ON a.agent_IdNo = c.Ledger_IdNo Where a.ClothSales_Order_Code = '" & Trim(Dt1.Rows(i).Item("ClothSales_Order_Code").ToString) & "' and ClothSales_Order_SlNo = " & Str(Val(Dt1.Rows(i).Item("ClothSales_Order_SlNo").ToString)), con)
    '                        Dt2 = New DataTable
    '                        Da.Fill(Dt2)
    '                        If Dt2.Rows.Count > 0 Then
    '                            If Trim(.Rows(n).Cells(10).Value) = "" Then
    '                                .Rows(n).Cells(10).Value = Dt2.Rows(0).Item("agentname").ToString
    '                            End If
    '                            .Rows(n).Cells(16).Value = Dt2.Rows(0).Item("Agent_Comm_Perc").ToString
    '                            .Rows(n).Cells(17).Value = Dt2.Rows(0).Item("Agent_Comm_Type").ToString
    '                            .Rows(n).Cells(20).Value = Dt2.Rows(0).Item("Rate").ToString
    '                        End If
    '                        Dt2.Clear()

    '                        .Rows(n).Cells(21).Value = Val(Ent_Bls)
    '                        .Rows(n).Cells(22).Value = Ent_BlNos
    '                        .Rows(n).Cells(23).Value = Ent_Pcs
    '                        .Rows(n).Cells(24).Value = Ent_Mtrs

    '                        .Rows(n).Cells(25).Value = Dt1.Rows(i).Item("Lr_No").ToString
    '                        .Rows(n).Cells(26).Value = Dt1.Rows(i).Item("Lr_Date").ToString
    '                        .Rows(n).Cells(27).Value = Dt1.Rows(i).Item("Clothsales_Delivery_Code").ToString
    '                        .Rows(n).Cells(28).Value = Dt1.Rows(i).Item("Clothsales_Delivery_SlNo").ToString
    '                        .Rows(n).Cells(29).Value = Dt1.Rows(i).Item("Party_OrderDate").ToString
    '                        .Rows(n).Cells(30).Value = Dt1.Rows(i).Item("Bales").ToString
    '                        .Rows(n).Cells(31).Value = Dt1.Rows(i).Item("Bales_Nos").ToString

    '                        .Rows(n).Cells(32).Value = Ent_InvDetSlNo
    '                        .Rows(n).Cells(33).Value = Ent_PackSlpCodes

    '                        .Rows(n).Cells(34).Value = Ent_Rate
    '                        .Rows(n).Cells(35).Value = Ent_ShtMtrs
    '                        .Rows(n).Cells(36).Value = Dt1.Rows(i).Item("Delivery_Meters").ToString

    '                        If Val(.Rows(n).Cells(20).Value) = 0 Then
    '                            .Rows(n).Cells(20).Value = Dt1.Rows(i).Item("Sound_Rate").ToString
    '                        End If

    '                    Next
    '                End If
    '                Dt1.Clear()
    '            End With

    '            pnl_Selection.Visible = True
    '            pnl_Back.Enabled = False
    '            If dgv_Selection.Enabled And dgv_Selection.Visible Then
    '                dgv_Selection.Focus()
    '                If dgv_Selection.Rows.Count > 0 Then
    '                    dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
    '                    dgv_Selection.CurrentCell.Selected = True
    '                End If
    '            End If

    '        End If

    '    End Sub

    '    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
    '        Select_Piece(e.RowIndex)
    '    End Sub

    '    Private Sub Select_Piece(ByVal RwIndx As Integer)
    '        Dim i As Integer

    '        With dgv_Selection

    '            If .RowCount > 0 And RwIndx >= 0 Then

    '                .Rows(RwIndx).Cells(9).Value = (Val(.Rows(RwIndx).Cells(9).Value) + 1) Mod 2

    '                If Val(.Rows(RwIndx).Cells(9).Value) = 1 Then

    '                    For i = 0 To .ColumnCount - 1
    '                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
    '                    Next

    '                Else
    '                    .Rows(RwIndx).Cells(9).Value = ""

    '                    For i = 0 To .ColumnCount - 1
    '                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
    '                    Next

    '                End If

    '            End If

    '        End With

    '    End Sub

    '    Private Sub dgv_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Selection.KeyDown
    '        Dim n As Integer

    '        On Error Resume Next

    '        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
    '            If dgv_Selection.CurrentCell.RowIndex >= 0 Then

    '                n = dgv_Selection.CurrentCell.RowIndex

    '                Select_Piece(n)

    '                e.Handled = True

    '            End If
    '        End If
    '    End Sub

    '    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
    '        Cloth_Invoice_Selection()
    '    End Sub

    '    Private Sub Cloth_Invoice_Selection()
    '        Dim Da1 As New SqlClient.SqlDataAdapter
    '        Dim Dt1 As New DataTable
    '        Dim n As Integer = 0
    '        Dim SNo As Integer = 0
    '        Dim i As Integer = 0
    '        Dim j As Integer = 0

    '        If Trim(UCase(cbo_Type.Text)) = "ORDER" Then

    '            dgv_Details.Rows.Clear()

    '            For i = 0 To dgv_Selection.RowCount - 1

    '                If Val(dgv_Selection.Rows(i).Cells(9).Value) = 1 Then

    '                    txt_OrderNo.Text = dgv_Selection.Rows(i).Cells(2).Value
    '                    msk_OrderDate.Text = dgv_Selection.Rows(i).Cells(3).Value
    '                    cbo_Agent.Text = dgv_Selection.Rows(i).Cells(10).Value
    '                    cbo_Through.Text = dgv_Selection.Rows(i).Cells(12).Value
    '                    cbo_DespTo.Text = dgv_Selection.Rows(i).Cells(13).Value
    '                    cbo_Transport.Text = dgv_Selection.Rows(i).Cells(11).Value

    '                    If Trim(txt_DelvAdd1.Text) = "" Then
    '                        If Trim(dgv_Selection.Rows(i).Cells(14).Value) <> "" Then
    '                            txt_DelvAdd1.Text = dgv_Selection.Rows(i).Cells(14).Value
    '                        End If
    '                    End If

    '                    If Trim(txt_DelvAdd2.Text) = "" Then
    '                        If Trim(dgv_Selection.Rows(i).Cells(15).Value) <> "" Then
    '                            txt_DelvAdd2.Text = dgv_Selection.Rows(i).Cells(15).Value
    '                        End If
    '                    End If

    '                    txt_com_per.Text = dgv_Selection.Rows(i).Cells(16).Value
    '                    cbo_Com_Type.Text = dgv_Selection.Rows(i).Cells(17).Value

    '                    n = dgv_Details.Rows.Add()
    '                    sno = sno + 1
    '                    dgv_Details.Rows(n).Cells(0).Value = Val(sno)
    '                    dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(4).Value
    '                    dgv_Details.Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(5).Value
    '                    dgv_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(6).Value

    '                    dgv_Details.Rows(n).Cells(10).Value = dgv_Selection.Rows(i).Cells(1).Value
    '                    dgv_Details.Rows(n).Cells(11).Value = dgv_Selection.Rows(i).Cells(18).Value
    '                    dgv_Details.Rows(n).Cells(12).Value = dgv_Selection.Rows(i).Cells(19).Value

    '                    dgv_Details.Rows(n).Cells(13).Value = ""
    '                    dgv_Details.Rows(n).Cells(14).Value = ""

    '                    If Val(dgv_Selection.Rows(i).Cells(32).Value) <> 0 Then
    '                        dgv_Details.Rows(n).Cells(15).Value = dgv_Selection.Rows(i).Cells(32).Value
    '                    Else
    '                        dgv_Details.Rows(n).Cells(15).Value = ""
    '                    End If

    '                    If Trim(dgv_Selection.Rows(i).Cells(33).Value) <> "" Then
    '                        dgv_Details.Rows(n).Cells(16).Value = dgv_Selection.Rows(i).Cells(33).Value
    '                    Else
    '                        dgv_Details.Rows(n).Cells(16).Value = ""
    '                    End If

    '                    If Val(dgv_Selection.Rows(i).Cells(21).Value) <> 0 Then
    '                        dgv_Details.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(21).Value
    '                    End If
    '                    dgv_Details.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(22).Value

    '                    If Val(dgv_Selection.Rows(i).Cells(23).Value) <> 0 Then
    '                        dgv_Details.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(23).Value
    '                    Else
    '                        dgv_Details.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(7).Value
    '                    End If



    '                    If Val(dgv_Selection.Rows(i).Cells(24).Value) <> 0 Then
    '                        dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(24).Value
    '                    Else
    '                        dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(8).Value
    '                    End If

    '                    If Val(dgv_Selection.Rows(i).Cells(34).Value) <> 0 Then
    '                        dgv_Details.Rows(n).Cells(8).Value = dgv_Selection.Rows(i).Cells(34).Value
    '                    Else
    '                        dgv_Details.Rows(n).Cells(8).Value = dgv_Selection.Rows(i).Cells(20).Value
    '                    End If

    '                    ' dgv_Details.Rows(n).Cells(17).Value = Val(dgv_Selection.Rows(i).Cells(35).Value)

    '                    Amount_Calculation(n, 7)

    '                End If

    '            Next

    '        ElseIf Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then

    '            dgv_Details.Rows.Clear()

    '            For i = 0 To dgv_Selection.RowCount - 1

    '                If Val(dgv_Selection.Rows(i).Cells(9).Value) = 1 Then


    '                    txt_OrderNo.Text = dgv_Selection.Rows(i).Cells(2).Value
    '                    msk_OrderDate.Text = dgv_Selection.Rows(i).Cells(29).Value
    '                    txt_DcNo.Text = dgv_Selection.Rows(i).Cells(1).Value
    '                    msk_DcDate.Text = dgv_Selection.Rows(i).Cells(3).Value
    '                    txt_LrNo.Text = dgv_Selection.Rows(i).Cells(25).Value
    '                    msk_Lr_Date.Text = dgv_Selection.Rows(i).Cells(26).Value


    '                    cbo_Agent.Text = dgv_Selection.Rows(i).Cells(10).Value

    '                    cbo_Through.Text = dgv_Selection.Rows(i).Cells(12).Value
    '                    cbo_DespTo.Text = dgv_Selection.Rows(i).Cells(13).Value
    '                    cbo_Transport.Text = dgv_Selection.Rows(i).Cells(11).Value


    '                    If txt_DelvAdd1.Text = "" Then
    '                        If (dgv_Selection.Rows(i).Cells(14).Value) <> "" Then
    '                            txt_DelvAdd1.Text = dgv_Selection.Rows(i).Cells(14).Value
    '                        End If
    '                    End If

    '                    If txt_DelvAdd2.Text = "" Then
    '                        If (dgv_Selection.Rows(i).Cells(15).Value) <> "" Then
    '                            txt_DelvAdd2.Text = dgv_Selection.Rows(i).Cells(15).Value
    '                        End If
    '                    End If

    '                    txt_com_per.Text = dgv_Selection.Rows(i).Cells(16).Value
    '                    cbo_Com_Type.Text = dgv_Selection.Rows(i).Cells(17).Value


    '                    n = dgv_Details.Rows.Add()
    '                    sno = sno + 1
    '                    dgv_Details.Rows(n).Cells(0).Value = Val(sno)
    '                    dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(4).Value
    '                    dgv_Details.Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(5).Value
    '                    dgv_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(6).Value

    '                    dgv_Details.Rows(n).Cells(10).Value = dgv_Selection.Rows(i).Cells(1).Value

    '                    dgv_Details.Rows(n).Cells(11).Value = ""
    '                    dgv_Details.Rows(n).Cells(12).Value = ""

    '                    dgv_Details.Rows(n).Cells(13).Value = dgv_Selection.Rows(i).Cells(27).Value
    '                    dgv_Details.Rows(n).Cells(14).Value = dgv_Selection.Rows(i).Cells(28).Value


    '                    If Val(dgv_Selection.Rows(i).Cells(32).Value) <> 0 Then
    '                        dgv_Details.Rows(n).Cells(15).Value = dgv_Selection.Rows(i).Cells(32).Value
    '                    Else
    '                        dgv_Details.Rows(n).Cells(15).Value = ""
    '                    End If

    '                    If Trim(dgv_Selection.Rows(i).Cells(33).Value) <> "" Then
    '                        dgv_Details.Rows(n).Cells(16).Value = dgv_Selection.Rows(i).Cells(33).Value
    '                    Else
    '                        dgv_Details.Rows(n).Cells(16).Value = ""
    '                    End If

    '                    If Val(dgv_Selection.Rows(i).Cells(21).Value) <> 0 Then
    '                        dgv_Details.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(21).Value
    '                    Else
    '                        dgv_Details.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(30).Value
    '                    End If

    '                    If dgv_Selection.Rows(i).Cells(22).Value <> "" Then
    '                        dgv_Details.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(22).Value
    '                    Else
    '                        dgv_Details.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(31).Value
    '                    End If

    '                    If Val(dgv_Selection.Rows(i).Cells(23).Value) <> 0 Then
    '                        dgv_Details.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(23).Value
    '                    Else
    '                        dgv_Details.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(7).Value
    '                    End If

    '                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then

    '                        If Val(dgv_Selection.Rows(i).Cells(24).Value) <> 0 Then

    '                            dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(24).Value

    '                        Else

    '                            dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(8).Value * dgv_Selection.Rows(i).Cells(6).Value / 100

    '                        End If

    '                    Else

    '                        If Val(dgv_Selection.Rows(i).Cells(24).Value) <> 0 Then
    '                            dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(24).Value
    '                        Else
    '                            dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(8).Value
    '                        End If

    '                    End If

    '                    'If Val(dgv_Selection.Rows(i).Cells(24).Value) <> 0 Then
    '                    '    dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(24).Value
    '                    'Else
    '                    '    dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(8).Value
    '                    'End If

    '                    If Val(dgv_Selection.Rows(i).Cells(34).Value) <> 0 Then
    '                        dgv_Details.Rows(n).Cells(8).Value = dgv_Selection.Rows(i).Cells(34).Value
    '                    Else
    '                        dgv_Details.Rows(n).Cells(8).Value = dgv_Selection.Rows(i).Cells(20).Value
    '                    End If

    '                    dgv_Details.Rows(n).Cells(17).Value = Val(dgv_Selection.Rows(i).Cells(35).Value)

    '                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then
    '                        dgv_Details.Rows(n).Cells(18).Value = dgv_Selection.Rows(i).Cells(36).Value * dgv_Selection.Rows(i).Cells(6).Value / 100
    '                    Else
    '                        dgv_Details.Rows(n).Cells(18).Value = dgv_Selection.Rows(i).Cells(36).Value

    '                    End If

    '                    Amount_Calculation(n, 7)

    '                End If

    '            Next

    '        End If

    '        For i = 0 To dgv_Details.Rows.Count - 1
    '            If Val(dgv_Details.Rows(i).Cells(15).Value) = 0 Then
    '                Set_Max_DetailsSlNo(i, 15)
    '            End If
    '        Next

    '        Total_Calculation()

    '        pnl_Back.Enabled = True
    '        pnl_Selection.Visible = False
    '        If txt_DcNo.Enabled And txt_DcNo.Visible Then txt_DcNo.Focus()

    '    End Sub

    '    Private Sub cbo_Type_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.TextChanged
    '        If Trim(UCase(cbo_Type.Text)) <> "ORDER" And Trim(UCase(cbo_Type.Text)) <> "DELIVERY" Then
    '            dgv_Details.AllowUserToAddRows = True
    '        Else
    '            dgv_Details.AllowUserToAddRows = False
    '        End If
    '    End Sub

    '    Private Sub btn_BaleSelection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_BaleSelection.Click
    '        Dim Da As New SqlClient.SqlDataAdapter
    '        Dim Dt1 As New DataTable
    '        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
    '        Dim Clo_ID As Integer, CloType_ID As Integer
    '        Dim NewCode As String
    '        Dim Fd_Perc As Integer
    '        Dim CompIDCondt As String
    '        Dim dgvDet_CurRow As Integer
    '        Dim dgv_DetSlNo As Long

    '        Try

    '            If Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then
    '                Exit Sub
    '            End If

    '            If dgv_Details.CurrentCell.RowIndex < 0 Then
    '                MessageBox.Show("Invalid Cloth Name & Type Selection", "DOES NOT SELECT BALE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '                If dgv_Details.Enabled And dgv_Details.Visible Then
    '                    If dgv_Details.Rows.Count > 0 Then
    '                        dgv_Details.Focus()
    '                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)
    '                        dgv_Details.CurrentCell.Selected = True
    '                    End If
    '                End If
    '                Exit Sub
    '            End If

    '            Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(1).Value)
    '            If Clo_ID = 0 Then
    '                MessageBox.Show("Invalid Cloth Name", "DOES NOT SELECT BALE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '                If dgv_Details.Enabled And dgv_Details.Visible Then
    '                    If dgv_Details.Rows.Count > 0 Then
    '                        dgv_Details.Focus()
    '                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
    '                        If cbo_Grid_ClothName.Visible And cbo_Grid_ClothName.Enabled Then cbo_Grid_ClothName.Focus()
    '                        'dgv_Details.CurrentCell.Selected = True
    '                        Exit Sub
    '                    End If
    '                End If
    '                Exit Sub
    '            End If

    '            CloType_ID = Common_Procedures.ClothType_NameToIdNo(con, dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(2).Value)
    '            If CloType_ID = 0 Then
    '                MessageBox.Show("Invalid Cloth Type ", "DOES NOT SELECT BALE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '                If dgv_Details.Enabled And dgv_Details.Visible Then
    '                    If dgv_Details.Rows.Count > 0 Then
    '                        dgv_Details.Focus()
    '                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(2)
    '                        If cbo_Grid_Colour.Visible And cbo_Grid_Colour.Enabled Then cbo_Grid_Colour.Focus()
    '                        Exit Sub
    '                    End If
    '                End If
    '                Exit Sub
    '            End If

    '            Fd_Perc = Val(dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(3).Value)
    '            If Val(Fd_Perc) = 0 Then
    '                MessageBox.Show("Invalid Folding", "DOES NOT SELECT BALE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '                If dgv_Details.Enabled And dgv_Details.Visible Then
    '                    If dgv_Details.Rows.Count > 0 Then
    '                        dgv_Details.Focus()
    '                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(3)
    '                        dgv_Details.CurrentCell.Selected = True
    '                    End If
    '                End If
    '                Exit Sub
    '            End If

    '            CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
    '            If Trim(UCase(Common_Procedures.settings.CompanyName)) = "-----~~~" Then
    '                CompIDCondt = ""
    '            End If

    '            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

    '            dgvDet_CurRow = dgv_Details.CurrentCell.RowIndex
    '            dgv_DetSlNo = Val(dgv_Details.Rows(dgvDet_CurRow).Cells(15).Value)

    '            With dgv_BaleSelection
    '                chk_SelectAll.Checked = False
    '                .Rows.Clear()
    '                SNo = 0

    '                Da = New SqlClient.SqlDataAdapter("Select a.* from Packing_Slip_Head a where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Delivery_DetailsSlNo = " & Str(Val(dgv_DetSlNo)) & " and a.Cloth_IdNo = " & Str(Val(Clo_ID)) & "  and a.ClothType_IdNo = " & Str(Val(CloType_ID)) & "  and a.Folding = " & Str(Val(Fd_Perc)) & " order by a.Packing_Slip_Date, a.for_orderby, a.Packing_Slip_No, a.Packing_Slip_Code", con)
    '                Dt1 = New DataTable
    '                Da.Fill(Dt1)

    '                If Dt1.Rows.Count > 0 Then

    '                    For i = 0 To Dt1.Rows.Count - 1

    '                        n = .Rows.Add()

    '                        SNo = SNo + 1
    '                        .Rows(n).Cells(0).Value = Val(SNo)
    '                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Packing_Slip_No").ToString
    '                        If Val(Dt1.Rows(i).Item("Total_Pcs").ToString) <> 0 Then
    '                            .Rows(n).Cells(2).Value = Val(Dt1.Rows(i).Item("Total_Pcs").ToString)
    '                        End If
    '                        If Val(Dt1.Rows(i).Item("Total_Meters").ToString) <> 0 Then
    '                            .Rows(n).Cells(3).Value = Format(Val(Dt1.Rows(i).Item("Total_Meters").ToString), "#########0.00")
    '                        End If
    '                        If Val(Dt1.Rows(i).Item("Total_Weight").ToString) <> 0 Then
    '                            .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Total_Weight").ToString), "#########0.000")
    '                        End If
    '                        .Rows(n).Cells(5).Value = "1"
    '                        .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Packing_Slip_Code").ToString
    '                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Bale_Bundle").ToString

    '                        For j = 0 To .ColumnCount - 1
    '                            .Rows(i).Cells(j).Style.ForeColor = Color.Red
    '                        Next

    '                    Next

    '                End If
    '                Dt1.Clear()

    '                Da = New SqlClient.SqlDataAdapter("select a.* from Packing_Slip_Head a where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Delivery_Code = '' and a.Cloth_IdNo = " & Str(Val(Clo_ID)) & "  and a.ClothType_IdNo = " & Str(Val(CloType_ID)) & "  and a.Folding = " & Str(Val(Fd_Perc)) & " order by a.Packing_Slip_Date, a.for_orderby, a.Packing_Slip_No, a.Packing_Slip_Code", con)
    '                Dt1 = New DataTable
    '                Da.Fill(Dt1)

    '                If Dt1.Rows.Count > 0 Then

    '                    For i = 0 To Dt1.Rows.Count - 1

    '                        n = .Rows.Add()

    '                        SNo = SNo + 1
    '                        .Rows(n).Cells(0).Value = Val(SNo)
    '                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Packing_Slip_No").ToString
    '                        If Val(Dt1.Rows(i).Item("Total_Pcs").ToString) <> 0 Then
    '                            .Rows(n).Cells(2).Value = Val(Dt1.Rows(i).Item("Total_Pcs").ToString)
    '                        End If
    '                        If Val(Dt1.Rows(i).Item("Total_Meters").ToString) <> 0 Then
    '                            .Rows(n).Cells(3).Value = Format(Val(Dt1.Rows(i).Item("Total_Meters").ToString), "#########0.00")
    '                        End If
    '                        If Val(Dt1.Rows(i).Item("Total_Weight").ToString) <> 0 Then
    '                            .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Total_Weight").ToString), "#########0.000")
    '                        End If
    '                        .Rows(n).Cells(5).Value = ""
    '                        .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Packing_Slip_Code").ToString
    '                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Bale_Bundle").ToString

    '                    Next

    '                End If
    '                Dt1.Clear()


    '            End With

    '            pnl_BaleSelection.Visible = True
    '            pnl_Back.Enabled = False
    '            dgv_BaleSelection.Focus()
    '            If dgv_BaleSelection.Rows.Count > 0 Then
    '                dgv_BaleSelection.CurrentCell = dgv_BaleSelection.Rows(0).Cells(0)
    '                dgv_BaleSelection.CurrentCell.Selected = True
    '            End If

    '        Catch ex As NullReferenceException
    '            MessageBox.Show("Select the ClothName for Bale Selection", "DOES NOT SELECT BALE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '        Catch ex As Exception
    '            MessageBox.Show(ex.Message, "DOES NOT SELECT BALE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '        End Try



    '    End Sub

    '    Private Sub dgv_BaleSelection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BaleSelection.CellClick
    '        Select_Bale(e.RowIndex)
    '    End Sub

    '    Private Sub Select_Bale(ByVal RwIndx As Integer)
    '        Dim i As Integer

    '        With dgv_BaleSelection

    '            If .RowCount > 0 And RwIndx >= 0 Then

    '                .Rows(RwIndx).Cells(5).Value = (Val(.Rows(RwIndx).Cells(5).Value) + 1) Mod 2

    '                If Val(.Rows(RwIndx).Cells(5).Value) = 0 Then .Rows(RwIndx).Cells(5).Value = ""

    '                For i = 0 To .ColumnCount - 1
    '                    .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
    '                Next

    '            End If

    '        End With

    '    End Sub

    '    Private Sub dgv_BaleSelection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_BaleSelection.KeyDown
    '        On Error Resume Next

    '        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
    '            If dgv_BaleSelection.CurrentCell.RowIndex >= 0 Then
    '                Select_Bale(dgv_BaleSelection.CurrentCell.RowIndex)
    '                e.Handled = True
    '            End If
    '        End If

    '        If e.KeyCode = Keys.Delete Or e.KeyCode = Keys.Back Then
    '            If dgv_BaleSelection.CurrentCell.RowIndex >= 0 Then
    '                If Val(dgv_BaleSelection.Rows(dgv_BaleSelection.CurrentCell.RowIndex).Cells(5).Value) = 1 Then
    '                    e.Handled = True
    '                    Select_Bale(dgv_BaleSelection.CurrentCell.RowIndex)
    '                End If
    '            End If
    '        End If

    '    End Sub

    '    Private Sub btn_Close_BaleSelection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_BaleSelection.Click
    '        Dim Cmd As New SqlClient.SqlCommand
    '        Dim Da1 As New SqlClient.SqlDataAdapter
    '        Dim Dt1 As New DataTable
    '        Dim I As Integer, J As Integer
    '        Dim n As Integer
    '        Dim sno As Integer
    '        Dim dgvDet_CurRow As Integer = 0
    '        Dim dgv_DetSlNo As Integer = 0
    '        Dim NoofBls As Integer
    '        Dim FsNo As Single, LsNo As Single
    '        Dim FsBaleNo As String, LsBaleNo As String
    '        Dim BlNo As String, PackSlpCodes As String
    '        Dim Tot_Pcs As Single, Tot_Mtrs As Single


    '        Cmd.Connection = con

    '        dgvDet_CurRow = dgv_Details.CurrentCell.RowIndex
    '        dgv_DetSlNo = Val(dgv_Details.Rows(dgvDet_CurRow).Cells(15).Value)

    '        With dgv_BaleSelectionDetails

    'LOOP1:
    '            For I = 0 To .RowCount - 1

    '                If Val(.Rows(I).Cells(0).Value) = Val(dgv_DetSlNo) Then

    '                    If I = .Rows.Count - 1 Then
    '                        For J = 0 To .ColumnCount - 1
    '                            .Rows(I).Cells(J).Value = ""
    '                        Next

    '                    Else
    '                        .Rows.RemoveAt(I)

    '                    End If

    '                    GoTo LOOP1

    '                End If

    '            Next I

    '            Cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
    '            Cmd.ExecuteNonQuery()

    '            NoofBls = 0 : Tot_Pcs = 0 : Tot_Mtrs = 0 : BlNo = "" : PackSlpCodes = ""

    '            For I = 0 To dgv_BaleSelection.RowCount - 1

    '                If Val(dgv_BaleSelection.Rows(I).Cells(5).Value) = 1 Then

    '                    n = .Rows.Add()

    '                    sno = sno + 1
    '                    .Rows(n).Cells(0).Value = Val(dgv_DetSlNo)
    '                    .Rows(n).Cells(1).Value = dgv_BaleSelection.Rows(I).Cells(1).Value
    '                    .Rows(n).Cells(2).Value = Val(dgv_BaleSelection.Rows(I).Cells(2).Value)
    '                    .Rows(n).Cells(3).Value = Format(Val(dgv_BaleSelection.Rows(I).Cells(3).Value), "#########0.00")
    '                    .Rows(n).Cells(4).Value = Format(Val(dgv_BaleSelection.Rows(I).Cells(4).Value), "#########0.000")
    '                    .Rows(n).Cells(5).Value = dgv_BaleSelection.Rows(I).Cells(6).Value
    '                    .Rows(n).Cells(6).Value = dgv_BaleSelection.Rows(I).Cells(7).Value

    '                    Cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Meters1) values ('" & Trim(dgv_BaleSelection.Rows(I).Cells(6).Value) & "', '" & Trim(dgv_BaleSelection.Rows(I).Cells(1).Value) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(dgv_BaleSelection.Rows(I).Cells(1).Value))) & " ) "
    '                    Cmd.ExecuteNonQuery()

    '                    NoofBls = NoofBls + 1
    '                    Tot_Pcs = Val(Tot_Pcs) + Val(dgv_BaleSelection.Rows(I).Cells(2).Value)
    '                    Tot_Mtrs = Val(Tot_Mtrs) + Val(dgv_BaleSelection.Rows(I).Cells(3).Value)
    '                    PackSlpCodes = Trim(PackSlpCodes) & IIf(Trim(PackSlpCodes) = "", "~", "") & Trim(dgv_BaleSelection.Rows(I).Cells(6).Value) & "~"

    '                End If

    '            Next

    '            BlNo = ""
    '            FsNo = 0 : LsNo = 0
    '            FsBaleNo = "" : LsBaleNo = ""

    '            Da1 = New SqlClient.SqlDataAdapter("Select Name1 as Bale_Code, Name2 as Bale_No, Meters1 as fororderby_baleno from " & Trim(Common_Procedures.EntryTempTable) & " where Name1 <> '' order by Meters1, Name2, Name1", con)
    '            Dt1 = New DataTable
    '            Da1.Fill(Dt1)

    '            If Dt1.Rows.Count > 0 Then

    '                FsNo = Val(Dt1.Rows(0).Item("fororderby_baleno").ToString)
    '                LsNo = Val(Dt1.Rows(0).Item("fororderby_baleno").ToString)

    '                FsBaleNo = Trim(UCase(Dt1.Rows(0).Item("Bale_No").ToString))
    '                LsBaleNo = Trim(UCase(Dt1.Rows(0).Item("Bale_No").ToString))

    '                For I = 1 To Dt1.Rows.Count - 1
    '                    If LsNo + 1 = Val(Dt1.Rows(I).Item("fororderby_baleno").ToString) Then
    '                        LsNo = Val(Dt1.Rows(I).Item("fororderby_baleno").ToString)
    '                        LsBaleNo = Trim(UCase(Dt1.Rows(I).Item("Bale_No").ToString))

    '                    Else
    '                        If FsNo = LsNo Then
    '                            BlNo = BlNo & Trim(FsBaleNo) & ","
    '                        Else
    '                            BlNo = BlNo & Trim(FsBaleNo) & "-" & Trim(LsBaleNo) & ","
    '                        End If
    '                        FsNo = Dt1.Rows(I).Item("fororderby_baleno").ToString
    '                        LsNo = Dt1.Rows(I).Item("fororderby_baleno").ToString

    '                        FsBaleNo = Trim(UCase(Dt1.Rows(I).Item("Bale_No").ToString))
    '                        LsBaleNo = Trim(UCase(Dt1.Rows(I).Item("Bale_No").ToString))

    '                    End If

    '                Next

    '                If FsNo = LsNo Then BlNo = BlNo & Trim(FsBaleNo) Else BlNo = BlNo & Trim(FsBaleNo) & "-" & Trim(LsBaleNo)

    '            End If
    '            Dt1.Clear()

    '            If Trim(dgv_Details.Rows(dgvDet_CurRow).Cells(16).Value) <> "" Then
    '                dgv_Details.Rows(dgvDet_CurRow).Cells(4).Value = ""
    '                dgv_Details.Rows(dgvDet_CurRow).Cells(5).Value = ""
    '                dgv_Details.Rows(dgvDet_CurRow).Cells(6).Value = ""
    '                dgv_Details.Rows(dgvDet_CurRow).Cells(7).Value = ""
    '                dgv_Details.Rows(dgvDet_CurRow).Cells(16).Value = ""
    '            End If
    '            If Val(NoofBls) <> 0 And Val(Tot_Mtrs) <> 0 Then
    '                dgv_Details.Rows(dgvDet_CurRow).Cells(4).Value = NoofBls
    '                dgv_Details.Rows(dgvDet_CurRow).Cells(5).Value = BlNo
    '                If Val(Tot_Pcs) <> 0 Then
    '                    dgv_Details.Rows(dgvDet_CurRow).Cells(6).Value = Val(Tot_Pcs)
    '                End If
    '                dgv_Details.Rows(dgvDet_CurRow).Cells(7).Value = Format(Val(Tot_Mtrs), "#########0.00")
    '                dgv_Details.Rows(dgvDet_CurRow).Cells(16).Value = PackSlpCodes

    '            End If

    '            Amount_Calculation(dgvDet_CurRow, 7)

    '            Add_NewRow_ToGrid()

    '            Total_Calculation()

    '        End With

    '        pnl_Back.Enabled = True
    '        pnl_BaleSelection.Visible = False
    '        If dgv_Details.Enabled And dgv_Details.Visible Then
    '            If dgv_Details.Rows.Count > 0 Then
    '                dgv_Details.Focus()
    '                If dgv_Details.CurrentCell.RowIndex >= 0 Then
    '                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(8)
    '                    dgv_Details.CurrentCell.Selected = True
    '                End If
    '            End If
    '        End If

    '    End Sub

    '    Private Sub Add_NewRow_ToGrid()
    '        On Error Resume Next

    '        Dim i As Integer
    '        Dim n As Integer = -1

    '        With dgv_Details
    '            If .Visible Then

    '                If .CurrentCell.RowIndex = .Rows.Count - 1 Then
    '                    If Trim(UCase(cbo_Type.Text)) <> "ORDER" And Trim(UCase(cbo_Type.Text)) <> "DELIVERY" Then
    '                        n = .Rows.Add()

    '                        For i = 0 To .Columns.Count - 1
    '                            .Rows(n).Cells(i).Value = .Rows(.CurrentCell.RowIndex).Cells(i).Value
    '                            .Rows(.CurrentCell.RowIndex).Cells(i).Value = ""
    '                        Next

    '                        For i = 0 To .Rows.Count - 1
    '                            .Rows(i).Cells(0).Value = i + 1
    '                        Next

    '                        .CurrentCell = .Rows(n).Cells(.CurrentCell.ColumnIndex)
    '                        .CurrentCell.Selected = True

    '                    End If
    '                End If

    '            End If

    '        End With

    '    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1081" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1114" Then  ' Kalaimagal Textile(Palaldam)
            pnl_Print.Visible = True
            pnl_Back.Enabled = False
        Else
            Printing_Invoice()
        End If

    End Sub

    Private Sub btn_Print_Delivery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Invoice.Click
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Then '---- Asia Textiles (Tirupur)
            pnl_PrintFormat_Selection.Visible = True
            pnl_Back.Enabled = False
            If btn_Print_Inv_Format1.Enabled And btn_Print_Inv_Format1.Visible Then
                btn_Print_Inv_Format1.Focus()
            End If
            btn_print_Close_Click(sender, e)

        Else
            Printing_Bale_Status = 0
            Printing_Invoice()
            btn_print_Close_Click(sender, e)

        End If
    End Sub

    Private Sub btn_Print_Bale_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Bale.Click

        Printing_Bale_Status = 0
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1114" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1081" Then
            Printing_Bale_Status = 1
            Printing_Bale_Estiamte()
            'Else

            '    Printing_Bale()
        End If

        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_Print_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Cancel.Click
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_print_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Print.Click
        pnl_Back.Enabled = True
        pnl_Print.Visible = False
    End Sub

    Public Sub Printing_Invoice()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim ps As Printing.PaperSize


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Processed_Fabric_Sales_Invoice_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Processed_Fabric_Sales_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", Con)
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
        If prn_Status <> 1 Then
            prn_InpOpts = ""
            ' If Trim(UCase(InvPrintFrmt)) <> "FORMAT-6" And Trim(UCase(InvPrintFrmt)) <> "FORMAT-7" Then
            prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "                  1. Original" & Chr(13) & "                  2. Duplicate" & Chr(13) & "                  3. Triplicate" & Chr(13) & "                  4. Extra Copy" & Space(10) & "                  5. All", "FOR INVOICE PRINTING...", "123")
            prn_InpOpts = Replace(Trim(prn_InpOpts), "5", "1234")
            'End If
        End If

       

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

                If Print_PDF_Status = True Then
                    '--This is actual & correct 
                    PrintDocument1.DocumentName = "Invoice"
                    PrintDocument1.PrinterSettings.PrinterName = "doPDF v7"
                    PrintDocument1.PrinterSettings.PrintFileName = "c:\Invoice.pdf"
                    PrintDocument1.Print()

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

                ppd.WindowState = FormWindowState.Normal
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.ClientSize = New Size(600, 600)

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
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String
        Dim W1 As Single = 0

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_Count = 0

        Try
            'da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Ledger_Name as TransportName , e.Ledger_Name as Agent_Name from Processed_Fabric_Sales_Invoice_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON (case when a.OnAc_IdNo <>0 then a.OnAc_IdNo else a.Ledger_IdNo end) = c.Ledger_IdNo Left outer JOIN Ledger_Head d ON a.Transport_IdNo = d.Ledger_IdNo Left outer JOIN Ledger_Head e ON a.Agent_IdNo = e.Ledger_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Processed_Fabric_Sales_Invoice_Code = '" & Trim(NewCode) & "'", con)

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Ledger_Name as TransportName, e.Ledger_Name as Agent_Name, Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code, f.Ledger_Name as DeliveryTo_LedgerName, f.Ledger_Address1 as DeliveryTo_LedgerAddress1, f.Ledger_Address2 as DeliveryTo_LedgerAddress2, f.Ledger_Address3 as DeliveryTo_LedgerAddress3, f.Ledger_Address4 as DeliveryTo_LedgerAddress4, f.Ledger_GSTinNo as DeliveryTo_LedgerGSTinNo, f.Ledger_pHONENo as DeliveryTo_LedgerPhoneNo, f.Pan_No as DeliveryTo_PanNo, Dsh.State_Name as DeliveryTo_State_Name, Dsh.State_Code as DeliveryTo_State_Code  from Processed_Fabric_Sales_Invoice_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo " & _
                                       " LEFT OUTER JOIN State_Head Csh ON b.Company_State_IdNo = Csh.State_IdNo " & _
                                       " INNER JOIN Ledger_Head c ON (case when a.OnAc_IdNo <>0 then a.OnAc_IdNo else a.Ledger_IdNo end) = c.Ledger_IdNo " & _
                                       "  LEFT OUTER JOIN State_Head Lsh ON c.Ledger_State_IdNo = Lsh.State_IdNo  " & _
                                       "Left outer JOIN Ledger_Head d ON a.Transport_IdNo = d.Ledger_IdNo  " & _
                                       "Left outer JOIN Ledger_Head e ON a.Agent_IdNo = e.Ledger_IdNo " & _
                                       " LEFT OUTER JOIN Ledger_Head f ON (case when a.DeliveryTo_IdNo <> 0 then a.DeliveryTo_IdNo else a.Ledger_IdNo end) = f.Ledger_IdNo " & _
                                       " LEFT OUTER JOIN State_Head Dsh ON f.Ledger_State_IdNo = Dsh.State_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Processed_Fabric_Sales_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' ", Con)

            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*,B.* from Processed_Fabric_Sales_Invoice_Details a INNER JOIN Cloth_Head b ON a.Fabric_IdNo = b.Cloth_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Processed_Fabric_Sales_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.Sl_No", Con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

                If Printing_Bale_Status <> 0 Then
                    da2 = New SqlClient.SqlDataAdapter("select a.* , b.Pack_No as BaleNo , b.Pcs as NoOfPcs , b.Meters as Mtrs from Processed_Fabric_Sales_Invoice_Details a INNER JOIN Processed_Fabric_Invoice_BaleEntry_Details b ON a.Processed_Fabric_Sales_Invoice_Code = b.Sales_Invoice_Code where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Processed_Fabric_Sales_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.Sl_No", Con)
                    prn_DetDt_sub = New DataTable
                    da2.Fill(prn_DetDt_sub)
                End If

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

        prn_OriDupTri = ""

        If Printing_Bale_Status = 1 Then  '---Sundara Mills
         
            Printing_Format19(e)
        Else
            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1081" Then

            '    Printing_Format1(e)
            'Else
            Printing_GST_Format3(e)
            'End If
        End If

        'Else
        'If prn_Status = 1 Then
        '    ' Printing_Format9(e)
        '    ' Else
        '    Printing_Format1(e)
        'End If
        'End If
    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        Dim ItmNm1 As String, ItmNm2 As String
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer = 0
        Dim flperc As Single = 0
        Dim flmtr As Single = 0
        Dim fmtr As Single = 0
        Dim VechDesc1 As String = "", VechDesc2 As String = ""

        PpSzSTS = False

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            'Debug.Print(ps.PaperName)
            If ps.Width = 800 And ps.Height = 600 Then
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                PpSzSTS = True
                Exit For
            End If
        Next

        If PpSzSTS = False Then

            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
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
                        e.PageSettings.PaperSize = ps
                        Exit For
                    End If
                Next
            End If

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1016" Then '---- Rajendra Textiles (Somanur)
            With PrintDocument1.DefaultPageSettings.Margins
                .Left = 20
                .Right = 65
                .Top = 50 ' 60
                .Bottom = 40
                LMargin = .Left
                RMargin = .Right
                TMargin = .Top
                BMargin = .Bottom
            End With

        Else
            With PrintDocument1.DefaultPageSettings.Margins
                .Left = 30 ' 40
                .Right = 45
                .Top = 50 ' 60
                .Bottom = 40
                LMargin = .Left
                RMargin = .Right
                TMargin = .Top
                BMargin = .Bottom
            End With

        End If


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

        NoofItems_PerPage = 2 ' 6

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(50) : ClAr(2) = 240 : ClAr(3) = 80 : ClAr(4) = 70 : ClAr(5) = 100 : ClAr(6) = 80
        ClAr(7) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6))

        TxtHgt = 18.75 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

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

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

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

                        If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString) <> "" Then
                            ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString)
                        Else
                            ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Name").ToString)
                        End If

                        ItmNm2 = ""
                        If Len(ItmNm1) > 35 Then
                            For I = 35 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 35
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If


                        CurY = CurY + TxtHgt + 10
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("No_of_Rolls").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("No_of_Rolls").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Pcs").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate_Meter").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

                        
                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

                       

                      

                        NoofDets = NoofDets + 1

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1009" Then
                        CurY = CurY + TxtHgt
                        CurY = CurY + TxtHgt - 5
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        NoofDets = NoofDets + 2
                    End If

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then

                        VechDesc1 = Trim(prn_HdDt.Rows(0).Item("Vechile_No").ToString)
                        VechDesc2 = ""

                        CurY = CurY + 5

                        Do

                            VechDesc2 = ""
                            If Len(VechDesc1) > 45 Then
                                For I = 45 To 1 Step -1
                                    If Mid$(Trim(VechDesc1), I, 1) = " " Or Mid$(Trim(VechDesc1), I, 1) = "," Or Mid$(Trim(VechDesc1), I, 1) = "." Or Mid$(Trim(VechDesc1), I, 1) = "-" Or Mid$(Trim(VechDesc1), I, 1) = "/" Or Mid$(Trim(VechDesc1), I, 1) = "_" Or Mid$(Trim(VechDesc1), I, 1) = "(" Or Mid$(Trim(VechDesc1), I, 1) = ")" Or Mid$(Trim(VechDesc1), I, 1) = "\" Or Mid$(Trim(VechDesc1), I, 1) = "[" Or Mid$(Trim(VechDesc1), I, 1) = "]" Or Mid$(Trim(VechDesc1), I, 1) = "{" Or Mid$(Trim(VechDesc1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 45
                                VechDesc2 = Microsoft.VisualBasic.Right(Trim(VechDesc1), Len(VechDesc1) - I)
                                VechDesc1 = Microsoft.VisualBasic.Left(Trim(VechDesc1), I - 1)
                            End If

                            CurY = CurY + TxtHgt - 5

                            p1Font = New Font("Calibri", 7, FontStyle.Regular)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(VechDesc1), LMargin + ClAr(1) + 10, CurY, 0, 0, p1Font)
                            NoofDets = NoofDets + 2

                            VechDesc1 = Trim(VechDesc2)
                            VechDesc2 = ""

                        Loop Until Trim(VechDesc1) = ""

                    End If

                End If

                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

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

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim C1 As Single, W1, W2 As Single, S1, S2 As Single
        Dim Cmp_Name, Desc As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim S As String

        PageNo = PageNo + 1

        CurY = TMargin

        'da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Ledger_Name, d.Company_Description as Transport_Name from Processed_Fabric_Sales_Invoice_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Transport_IdNo LEFT OUTER JOIN Company_Head d ON d.Company_IdNo = a.Company_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Processed_Fabric_Sales_Invoice_Code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
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

        p1Font = New Font("Calibri", 15, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY - TxtHgt - 5, 2, PrintWidth, p1Font)

        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY
        Desc = ""
        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = "" : Cmp_EMail = ""

        Desc = prn_HdDt.Rows(0).Item("Company_Description").ToString
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE : " & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
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
            Cmp_EMail = "MAIL ID : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
        End If

      
        CurY = CurY + TxtHgt - 3
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MK, Drawing.Image), LMargin + 20, CurY, 112, 80)
            'e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MK_2, Drawing.Image), LMargin + 20, CurY, 115, 80)
            'e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MK, Drawing.Image), LMargin + 20, CurY, 75, 75)
        End If

        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        'CurY = CurY + strHeight - 1
        'Common_Procedures.Print_To_PrintDocument(e, Desc, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, LMargin, CurY, 2, PrintWidth, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
            W1 = e.Graphics.MeasureString("INVOICE DATE  : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width
            W2 = e.Graphics.MeasureString("Despatch To   : ", pFont).Width
            S2 = e.Graphics.MeasureString("Sent Through  : ", pFont).Width

            CurY = CurY + 10
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "INVOICE NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            If prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString & "-" & prn_HdDt.Rows(0).Item("Processed_Fabric_Sales_Invoice_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
            Else
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Processed_Fabric_Sales_Invoice_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
            End If

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "INVOICE DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Processed_Fabric_Sales_Invoice_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            'If Trim(prn_HdDt.Rows(0).Item("Dc_No").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "DC NO : " & prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            'End If
            'If Trim(prn_HdDt.Rows(0).Item("Dc_Date").ToString) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "DC DATE : " & prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + C1 + 100, CurY, 0, 0, pFont)
            'End If

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " TIN : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
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


            Common_Procedures.Print_To_PrintDocument(e, "Transport ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + C1 + W2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + C1 + W2 + 30, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt - 1
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

            CurY = CurY + 10
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NO.OF", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BALES", LMargin + ClAr(1) + ClAr(2), CurY + TxtHgt, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NO.OF", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY + TxtHgt, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METER", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + TxtHgt, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE\", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + TxtHgt, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)

            CurY = CurY + TxtHgt + 15
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            CurY = CurY + 10
            p1Font = New Font("Calibri", 8, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cloth_Details").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, p1Font)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim rndoff As Double, TtAmt As Double
        Dim I As Integer
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim Cmp_Name As String
        Dim W1 As Single = 0
        Dim BmsInWrds As String
        Dim vprn_BlNos As String = ""
        Dim BLNo1 As String, BLNo2 As String
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""

        Try

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt

                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt + 20
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            CurY += 10

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


            vprn_BlNos = ""
            For I = 0 To prn_DetDt.Rows.Count - 1
                If Trim(prn_DetDt.Rows(I).Item("Roll_Nos").ToString) <> "" Then
                    vprn_BlNos = Trim(vprn_BlNos) & IIf(Trim(vprn_BlNos) <> "", ", ", "") & prn_DetDt.Rows(I).Item("Roll_Nos").ToString
                End If
            Next


            BLNo1 = Trim(vprn_BlNos)
            BLNo2 = ""
            If Len(BLNo1) > 30 Then
                For I = 30 To 1 Step -1
                    If Mid$(Trim(BLNo1), I, 1) = " " Or Mid$(Trim(BLNo1), I, 1) = "," Or Mid$(Trim(BLNo1), I, 1) = "." Or Mid$(Trim(BLNo1), I, 1) = "-" Or Mid$(Trim(BLNo1), I, 1) = "/" Or Mid$(Trim(BLNo1), I, 1) = "_" Or Mid$(Trim(BLNo1), I, 1) = "(" Or Mid$(Trim(BLNo1), I, 1) = ")" Or Mid$(Trim(BLNo1), I, 1) = "\" Or Mid$(Trim(BLNo1), I, 1) = "[" Or Mid$(Trim(BLNo1), I, 1) = "]" Or Mid$(Trim(BLNo1), I, 1) = "{" Or Mid$(Trim(BLNo1), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 30
                BLNo2 = Microsoft.VisualBasic.Right(Trim(BLNo1), Len(BLNo1) - I)
                BLNo1 = Microsoft.VisualBasic.Left(Trim(BLNo1), I - 1)
            End If

            If Trim(BLNo1) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "BALE No : " & BLNo1, LMargin + 10, CurY, 0, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Trade_Discount_Perc").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("TradeDisc_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Trade_Discount").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Trade_Discount_Perc").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If


            If Trim(BLNo2) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, BLNo2, LMargin + 10, CurY, 0, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Cash_Discount_Perc").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("CashDisc_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Cash_Discount").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Cash_Discount_Perc").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            'TtAmt = Format(Format(Val(prn_HdDt.Rows(0).Item("total_Amount").ToString), "#########0.00") + Format(Val(prn_HdDt.Rows(0).Item("Freight").ToString), "#########0.00") + Format(Val(prn_HdDt.Rows(0).Item("Insurance").ToString), "#########0.00") + Format(Val(prn_HdDt.Rows(0).Item("Packing_amount").ToString), "#########0.00") - Format(Val(prn_HdDt.Rows(0).Item("Trade_Discount_Perc").ToString), "#########0.00") - Format(Val(prn_HdDt.Rows(0).Item("Cash_Discount_Perc").ToString), "#########0.00"), "#########0.00")
            TtAmt = Format(Val(prn_HdDt.Rows(0).Item("total_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Freight").ToString) + Val(prn_HdDt.Rows(0).Item("Insurance").ToString) + Val(prn_HdDt.Rows(0).Item("Packing_amount").ToString) - Val(prn_HdDt.Rows(0).Item("Trade_Discount_Perc").ToString) - Val(prn_HdDt.Rows(0).Item("Cash_Discount_Perc").ToString), "#########0.00")

            rndoff = 0
            rndoff = Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) - Val(TtAmt)


            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            '  Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 10, CurY, 0, 0, p1Font)
            If Val(rndoff) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 0, 0, pFont)
                If Val(rndoff) >= 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 20, CurY, 0, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 20, CurY, 0, 0, pFont)
                End If
                Common_Procedures.Print_To_PrintDocument(e, " " & Format(Math.Abs(Val(rndoff)), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY, 0, 0, p1Font)
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)
            LnAr(8) = CurY

            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            CurY = CurY + TxtHgt ' 10
            Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Net_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("Gr_Time").ToString) <> 0 Then
                p1Font = New Font("Calibri", 10, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "Due Date : " & Trim(prn_HdDt.Rows(0).Item("Gr_Time").ToString) & " Days " & "(" & Trim(prn_HdDt.Rows(0).Item("Gr_Date").ToString) & ")", LMargin + 10, CurY, 0, 0, p1Font)
            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))

            ' CurY = CurY + 10

            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
            BmsInWrds = Replace(Trim(BmsInWrds), "", "")

            'Common_Procedures.Print_To_PrintDocument(e, "Rupees  : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)
            'CurY = CurY + TxtHgt + 10
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            'CurY = CurY + 10
            'p1Font = New Font("Calibri", 12, FontStyle.Regular)
            'Common_Procedures.Print_To_PrintDocument(e, "GOODS CLEARED UNDER EXEMPTION NOTIFICATION NO 30/2004 DT 09.07.2004 ", LMargin, CurY, 2, PageWidth, pFont)

            'CurY = CurY + TxtHgt
            'p1Font = New Font("Calibri", 12, FontStyle.Underline)
            'Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)


            'CurY = CurY + TxtHgt
            'If Val(prn_HdDt.Rows(0).Item("Gr_Time").ToString) <> 0 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "Overdue Interest will be charged at 24% from The  " & Trim(prn_HdDt.Rows(0).Item("Gr_Date").ToString), LMargin + 10, CurY, 0, 0, pFont)
            'Else
            '    Common_Procedures.Print_To_PrintDocument(e, "Overdue Interest will be charged at 24% from The invoice date ", LMargin + 10, CurY, 0, 0, pFont)
            'End If
            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "We are not responsible for any loss or damage in transit", LMargin + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "We will not accept any claim after processing of goods", LMargin + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "Subject to Tirupur jurisdiction ", LMargin + 10, CurY, 0, 0, pFont)


            'CurY = CurY + TxtHgt + 10
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            'LnAr(10) = CurY
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

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Then '---- Asia Textiles (Tirupur)
                CurY = CurY + TxtHgt - 10
                p1Font = New Font("Calibri", 9, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "Please send payment details of this bill to asiatextilestirupur@yahoo.in", LMargin + 10, CurY, 0, 0, p1Font)
            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    'Public Sub Printing_Bale()
    '    Dim prtFrm As Single = 0
    '    Dim prtTo As Single = 0
    '    Dim da1 As New SqlClient.SqlDataAdapter
    '    Dim dt1 As New DataTable
    '    Dim Condt As String = ""
    '    Dim PpSzSTS As Boolean = False
    '    Dim ps As Printing.PaperSize
    '    Dim NewCode As String

    '    NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

    '    Try

    '        da1 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name from Packing_Slip_Head a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
    '        dt1 = New DataTable
    '        da1.Fill(dt1)

    '        If dt1.Rows.Count <= 0 Then

    '            MessageBox.Show("No Entry Found", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '            Exit Sub

    '        End If

    '        dt1.Dispose()
    '        da1.Dispose()

    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try

    '    If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
    '        Try

    '            'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8.25X12", 850, 1200)
    '            'PrintDocument2.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
    '            'PrintDocument2.DefaultPageSettings.PaperSize = pkCustomSize1

    '            For I = 0 To PrintDocument2.PrinterSettings.PaperSizes.Count - 1
    '                If PrintDocument2.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
    '                    ps = PrintDocument2.PrinterSettings.PaperSizes(I)
    '                    PrintDocument2.DefaultPageSettings.PaperSize = ps
    '                    Exit For
    '                End If
    '            Next

    '            PrintDialog1.PrinterSettings = PrintDocument2.PrinterSettings
    '            If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
    '                PrintDocument2.PrinterSettings = PrintDialog1.PrinterSettings
    '                PrintDocument2.Print()
    '            End If

    '        Catch ex As Exception
    '            MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT SHOW PRINT PREVIEW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '        End Try

    '    Else

    '        Try

    '            Dim ppd As New PrintPreviewDialog

    '            For I = 0 To PrintDocument2.PrinterSettings.PaperSizes.Count - 1
    '                If PrintDocument2.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
    '                    ps = PrintDocument2.PrinterSettings.PaperSizes(I)
    '                    PrintDocument2.DefaultPageSettings.PaperSize = ps
    '                    Exit For
    '                End If
    '            Next

    '            ppd.Document = PrintDocument2

    '            ppd.WindowState = FormWindowState.Normal
    '            ppd.StartPosition = FormStartPosition.CenterScreen
    '            ppd.ClientSize = New Size(600, 600)

    '            ppd.ShowDialog()
    '            'If PageSetupDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
    '            '    PrintDocument2.DefaultPageSettings = PageSetupDialog1.PageSettings
    '            '    ppd.ShowDialog()
    '            'End If

    '        Catch ex As Exception
    '            MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

    '        End Try

    '    End If

    '    pnl_Back.Enabled = True
    '    pnl_Print.Visible = False

    'End Sub

    'Private Sub PrintDocument2_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument2.BeginPrint
    '    Dim da1 As New SqlClient.SqlDataAdapter
    '    Dim da2 As New SqlClient.SqlDataAdapter
    '    Dim NewCode As String = ""

    '    prn_HdDt.Clear()
    '    prn_DetDt.Clear()

    '    prn_PageNo = 0
    '    prn_HdIndx = 0
    '    prn_DetIndx = 0
    '    prn_HdMxIndx = 0
    '    prn_DetMxIndx = 0
    '    prn_Count = 1
    '    Erase prn_DetAr
    '    Erase prn_HdAr

    '    prn_HdAr = New String(100, 10) {}

    '    prn_DetAr = New String(100, 50, 10) {}

    '    NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

    '    Try
    '        Total_mtrs = 0

    '        da1 = New SqlClient.SqlDataAdapter("select a.*, tZ.*, c.Cloth_Name , d.* , E.* from Packing_Slip_Head a  INNER JOIN Processed_Fabric_Sales_Invoice_Head d ON d.Processed_Fabric_Sales_Invoice_Code =  '" & Trim(NewCode) & "' INNER JOIN Company_head tZ ON a.company_idno = tZ.Company_Idno INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo LEFT OUTER JOIN LEDGER_Head E ON D.Ledger_IdNo = E.Ledger_IdNo  Where a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.Packing_Slip_Date, a.for_OrderBy, a.Packing_Slip_No, a.Packing_Slip_Code", con)
    '        prn_HdDt = New DataTable
    '        da1.Fill(prn_HdDt)

    '        If prn_HdDt.Rows.Count > 0 Then
    '            For i = 0 To prn_HdDt.Rows.Count - 1

    '                prn_HdMxIndx = prn_HdMxIndx + 1

    '                prn_HdAr(prn_HdMxIndx, 1) = Trim(prn_HdDt.Rows(i).Item("Packing_Slip_No").ToString)
    '                prn_HdAr(prn_HdMxIndx, 2) = Trim(prn_HdDt.Rows(i).Item("Cloth_Name").ToString)
    '                prn_HdAr(prn_HdMxIndx, 3) = Val(prn_HdDt.Rows(i).Item("Total_Bales").ToString)
    '                prn_HdAr(prn_HdMxIndx, 4) = Format(Val(prn_HdDt.Rows(i).Item("Total_Meters").ToString), "#########0.00")

    '                prn_DetMxIndx = 0


    '                da2 = New SqlClient.SqlDataAdapter("select a.* from Packing_Slip_Details a where a.Packing_Slip_Code = '" & Trim(prn_HdDt.Rows(i).Item("Packing_Slip_Code").ToString) & "' order by a.Sl_No", con)
    '                prn_DetDt = New DataTable
    '                da2.Fill(prn_DetDt)
    '                If prn_DetDt.Rows.Count > 0 Then
    '                    For j = 0 To prn_DetDt.Rows.Count - 1
    '                        If Val(prn_DetDt.Rows(j).Item("Meters").ToString) <> 0 Then
    '                            prn_DetMxIndx = prn_DetMxIndx + 1

    '                            prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 1) = Trim(prn_DetDt.Rows(j).Item("Lot_No").ToString)
    '                            prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 2) = Trim(prn_DetDt.Rows(j).Item("Pcs_No").ToString)
    '                            prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 3) = Format(Val(prn_DetDt.Rows(j).Item("Meters").ToString), "#########0.00")
    '                            Total_mtrs = Total_mtrs + Format(Val(prn_DetDt.Rows(j).Item("Meters").ToString), "#########0.00")

    '                        End If
    '                    Next j
    '                End If

    '            Next i

    '        Else
    '            MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '        End If

    '        da1.Dispose()

    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try

    'End Sub

    'Private Sub PrintDocument2_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument2.PrintPage
    '    If prn_HdDt.Rows.Count <= 0 Then Exit Sub
    '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
    '        Printing_PackingSlip_Format2(PrintDocument2, e, prn_HdDt, prn_HdMxIndx, prn_DetMxIndx, prn_HdAr, prn_DetAr, prn_PageNo, prn_Count, prn_HdIndx, prn_DetIndx)
    '    Else
    '        Common_Procedures.Printing_PackingSlip_Format1(PrintDocument2, e, prn_HdDt, prn_HdMxIndx, prn_DetMxIndx, prn_HdAr, prn_DetAr, prn_PageNo, prn_Count, prn_HdIndx, prn_DetIndx)
    '    End If

    'End Sub

    'Private Sub Printing_PackingSlip_Format2(ByRef PrintDocument1 As Printing.PrintDocument, ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByRef prn_HdDt As DataTable, ByVal prn_HdMxIndx As Integer, ByVal prn_DetMxIndx As Integer, ByRef prn_HdAr(,) As String, ByRef prn_DetAr(,,) As String, ByRef prn_PageNo As Integer, ByRef prn_Count As Integer, ByRef prn_HdIndx As Integer, ByRef prn_DetIndx As Integer)
    '    Dim NoofDets As Integer, NoofItems_PerPage As Integer
    '    Dim pFont As Font, P1fONT As Font
    '    Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
    '    Dim PrintWidth As Single, PrintHeight As Single
    '    Dim PageWidth As Single, PageHeight As Single
    '    Dim CurY As Single, TxtHgt As Single
    '    Dim LnAr(15) As Single, ClArr(15) As Single
    '    Dim ps As Printing.PaperSize
    '    Dim strHeight As Single = 0
    '    Dim PpSzSTS As Boolean = False
    '    Dim LM As Single = 0, TM As Single = 0
    '    Dim PgWt As Single = 0, PrWt As Single = 0
    '    Dim PgHt As Single = 0, PrHt As Single = 0

    '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
    '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
    '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
    '            PrintDocument1.DefaultPageSettings.PaperSize = ps
    '            e.PageSettings.PaperSize = ps
    '            Exit For
    '        End If
    '    Next

    '    With PrintDocument1.DefaultPageSettings.Margins
    '        .Left = 20
    '        .Right = 40
    '        .Top = 30
    '        .Bottom = 40
    '        LMargin = .Left
    '        RMargin = .Right
    '        TMargin = .Top
    '        BMargin = .Bottom
    '    End With

    '    With PrintDocument1.DefaultPageSettings.PaperSize
    '        PrintWidth = .Width - RMargin - LMargin
    '        PrintHeight = .Height - TMargin - BMargin
    '        PageWidth = .Width - RMargin
    '        PageHeight = .Height - BMargin
    '    End With
    '    'With PrintDocument1.DefaultPageSettings.PaperSize
    '    '    PrintWidth = (.Width / 2) - RMargin - LMargin
    '    '    PrintHeight = (.Height / 2) - TMargin - BMargin
    '    '    PageWidth = (.Width / 2) - RMargin
    '    '    PageHeight = (.Height / 2) - BMargin
    '    'End With
    '    If PrintDocument1.DefaultPageSettings.Landscape = True Then
    '        With PrintDocument1.DefaultPageSettings.PaperSize
    '            PrintWidth = .Height - TMargin - BMargin
    '            PrintHeight = .Width - RMargin - LMargin
    '            PageWidth = .Height - TMargin
    '            PageHeight = .Width - RMargin
    '        End With
    '    End If

    '    e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

    '    pFont = New Font("Calibri", 10, FontStyle.Regular)

    '    NoofItems_PerPage = 28 ' 29 ' 17 ' 20 

    '    Erase ClArr
    '    Erase LnAr
    '    ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    '    LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

    '    ClArr(1) = 55 : ClArr(2) = 95 : ClArr(3) = 95 : ClArr(4) = 95 : ClArr(5) = 95 : ClArr(6) = 95 : ClArr(7) = 90 : ClArr(8) = 90
    '    ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

    '    'ClArr(1) = 100 : ClArr(2) = 80 : ClArr(3) = 80 : ClArr(4) = 80 : ClArr(5) = 80 : ClArr(6) = 80 : ClArr(7) = 80 : ClArr(8) = 80
    '    'ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

    '    TxtHgt = 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

    '    Try

    '        If prn_HdDt.Rows.Count > 0 Then

    '            If prn_HdMxIndx > 0 Then

    '                Erase LnAr
    '                LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

    '                Printing_PackingSlip_Format2_PageHeader(PrintDocument1, e, prn_HdDt, prn_HdAr, TxtHgt, pFont, LMargin, RMargin, TM, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr, prn_HdIndx)
    '                CurY = CurY - 10

    '                NoofDets = 0
    '                Do While prn_HdIndx < prn_HdMxIndx

    '                    If NoofDets >= NoofItems_PerPage Then

    '                        CurY = CurY + TxtHgt
    '                        Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
    '                        NoofDets = NoofDets + 1

    '                        Printing_PackingSlip_Format2_PageFooter(e, prn_HdAr, TxtHgt, pFont, LMargin, RMargin, TM, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, prn_HdIndx, False)

    '                        'prn_DetIndx = prn_DetIndx + NoofItems_PerPage

    '                        e.HasMorePages = True

    '                        NoofDets = 0
    '                        prn_Count = prn_Count + 1

    '                        Return

    '                    End If

    '                    prn_HdIndx = prn_HdIndx + 1

    '                    If Val(prn_HdAr(prn_HdIndx, 4)) <> 0 Then

    '                        CurY = CurY + TxtHgt

    '                        P1fONT = New Font("Calibri", 8, FontStyle.Regular)

    '                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdAr(prn_HdIndx, 1)), LMargin + 15, CurY, 0, 0, P1fONT)
    '                        If Val(prn_DetAr(prn_HdIndx, 1, 3)) <> 0 Then

    '                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_HdIndx, 1, 1)) & "/" & Trim(prn_DetAr(prn_HdIndx, 1, 2)), LMargin + ClArr(1) + 5, CurY, 0, 0, P1fONT)
    '                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_HdIndx, 1, 3)), LMargin + ClArr(1) + ClArr(2) - 2, CurY, 1, 0, P1fONT)

    '                        End If
    '                        If Val(prn_DetAr(prn_HdIndx, 2, 3)) <> 0 Then
    '                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_HdIndx, 2, 1)) & "/" & Trim(prn_DetAr(prn_HdIndx, 2, 2)), LMargin + ClArr(1) + ClArr(2) + 5, CurY, 0, 0, P1fONT)
    '                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_HdIndx, 2, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 2, CurY, 1, 0, P1fONT)

    '                        End If
    '                        If Val(prn_DetAr(prn_HdIndx, 3, 3)) <> 0 Then
    '                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_HdIndx, 3, 1)) & "/" & Trim(prn_DetAr(prn_HdIndx, 3, 2)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + 5, CurY, 0, 0, P1fONT)
    '                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_HdIndx, 3, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 2, CurY, 1, 0, P1fONT)

    '                        End If

    '                        If Val(prn_DetAr(prn_HdIndx, 4, 3)) <> 0 Then
    '                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_HdIndx, 4, 1)) & "/" & Trim(prn_DetAr(prn_HdIndx, 4, 2)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 5, CurY, 0, 0, P1fONT)
    '                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_HdIndx, 4, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 2, CurY, 1, 0, P1fONT)

    '                        End If
    '                        If Val(prn_DetAr(prn_HdIndx, 5, 3)) <> 0 Then
    '                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_HdIndx, 5, 1)) & "/" & Trim(prn_DetAr(prn_HdIndx, 5, 2)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 5, CurY, 0, 0, P1fONT)
    '                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_HdIndx, 5, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 2, CurY, 1, 0, P1fONT)

    '                        End If
    '                        If Val(prn_DetAr(prn_HdIndx, 6, 3)) <> 0 Then
    '                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_HdIndx, 6, 1)) & "/" & Trim(prn_DetAr(prn_HdIndx, 6, 2)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 5, CurY, 0, 0, P1fONT)
    '                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_HdIndx, 6, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 2, CurY, 1, 0, P1fONT)

    '                        End If
    '                        If Val(prn_DetAr(prn_HdIndx, 7, 3)) <> 0 Then
    '                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_HdIndx, 7, 1)) & "/" & Trim(prn_DetAr(prn_HdIndx, 7, 2)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + 5, CurY, 0, 0, P1fONT)
    '                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_HdIndx, 7, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 2, CurY, 1, 0, P1fONT)

    '                        End If

    '                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdAr(prn_HdIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 2, CurY, 1, 0, pFont)

    '                        NoofDets = NoofDets + 1

    '                    End If

    '                Loop

    '                Printing_PackingSlip_Format2_PageFooter(e, prn_HdAr, TxtHgt, pFont, LMargin, RMargin, TM, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, prn_HdIndx, True)

    '            End If

    '        End If

    '    Catch ex As Exception

    '        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try

    '    e.HasMorePages = False

    'End Sub

    'Private Sub Printing_PackingSlip_Format2_PageHeader(ByRef PrintDocument1 As Printing.PrintDocument, ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByRef prn_HdDt As DataTable, ByRef prn_HdAr(,) As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal prn_HdIndx As Integer)
    '    Dim da2 As New SqlClient.SqlDataAdapter
    '    Dim dt2 As New DataTable
    '    Dim dt3 As New DataTable
    '    Dim dt4 As New DataTable
    '    Dim p1Font As Font
    '    Dim strHeight As Single
    '    Dim Cmp_Add As String = ""
    '    Dim C1 As Single, W1, W2 As Single, S1, S2 As Single
    '    Dim Cmp_Name, Desc As String, Cmp_Add1 As String, Cmp_Add2 As String
    '    Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String

    '    PageNo = PageNo + 1

    '    CurY = TMargin + 30

    '    'da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Ledger_Name, d.Company_Description as Transport_Name from Processed_Fabric_Sales_Invoice_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Transport_IdNo LEFT OUTER JOIN Company_Head d ON d.Company_IdNo = a.Company_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Processed_Fabric_Sales_Invoice_Code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
    '    'da2.Fill(dt2)
    '    'If dt2.Rows.Count > NoofItems_PerPage Then
    '    '    Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
    '    'End If
    '    'dt2.Clear()

    '    prn_Count = prn_Count + 1

    '    p1Font = New Font("Calibri", 15, FontStyle.Bold)
    '    Common_Procedures.Print_To_PrintDocument(e, "PACKING SLIP", LMargin, CurY - TxtHgt - 5, 2, PrintWidth, p1Font)

    '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '    LnAr(1) = CurY
    '    Desc = ""
    '    Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
    '    Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_EMail = ""

    '    Desc = prn_HdDt.Rows(0).Item("Company_Description").ToString
    '    Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
    '    Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
    '    Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

    '    If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
    '        Cmp_PhNo = "PHONE : " & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
    '        Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
    '        Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
    '        Cmp_EMail = "MAIL ID : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
    '    End If

    '    p1Font = New Font("Calibri", 15, FontStyle.Bold)
    '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1018" Then '---- M.K Textiles (Palladam)
    '        p1Font = New Font("Calibri", 15, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, "PACKING SLIP", LMargin, CurY, 2, PrintWidth, p1Font)
    '    End If
    '    CurY = CurY + TxtHgt
    '    strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


    '    'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
    '    '    e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MK, Drawing.Image), LMargin + 20, CurY, 112, 80)
    '    '    'e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MK_2, Drawing.Image), LMargin + 20, CurY, 115, 80)
    '    '    'e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MK, Drawing.Image), LMargin + 20, CurY, 75, 75)
    '    'End If

    '    p1Font = New Font("Calibri", 18, FontStyle.Bold)
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
    '    strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


    '    CurY = CurY + strHeight - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Desc, LMargin, CurY, 2, PrintWidth, pFont)

    '    CurY = CurY + TxtHgt - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

    '    CurY = CurY + TxtHgt - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
    '    CurY = CurY + TxtHgt - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
    '    CurY = CurY + TxtHgt - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, LMargin, CurY, 2, PrintWidth, pFont)
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

    '    CurY = CurY + TxtHgt + 10
    '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '    LnAr(2) = CurY

    '    C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
    '    W1 = e.Graphics.MeasureString("INVOICE DATE  : ", pFont).Width
    '    S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width
    '    W2 = e.Graphics.MeasureString("Despatch To   : ", pFont).Width
    '    S2 = e.Graphics.MeasureString("Sent Through  : ", pFont).Width


    '    CurY = CurY + 10
    '    p1Font = New Font("Calibri", 12, FontStyle.Bold)
    '    Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
    '    Common_Procedures.Print_To_PrintDocument(e, "INVOICE NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
    '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
    '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Processed_Fabric_Sales_Invoice_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

    '    CurY = CurY + TxtHgt
    '    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '    p1Font = New Font("Calibri", 14, FontStyle.Bold)
    '    Common_Procedures.Print_To_PrintDocument(e, "INVOICE DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
    '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
    '    Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Processed_Fabric_Sales_Invoice_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

    '    CurY = CurY + TxtHgt
    '    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '    Common_Procedures.Print_To_PrintDocument(e, "ORDER NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
    '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
    '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_OrderNo").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

    '    CurY = CurY + TxtHgt
    '    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)


    '    CurY = CurY + TxtHgt
    '    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '    If Trim(prn_HdDt.Rows(0).Item("Dc_No").ToString) <> "" Then
    '        Common_Procedures.Print_To_PrintDocument(e, "DC NO : " & prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Dc_Date").ToString) <> "" Then
    '        Common_Procedures.Print_To_PrintDocument(e, "DC DATE : " & prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + C1 + 100, CurY, 0, 0, pFont)
    '    End If

    '    CurY = CurY + TxtHgt
    '    If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
    '        Common_Procedures.Print_To_PrintDocument(e, " TIN : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '    End If


    '    CurY = CurY + TxtHgt
    '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '    e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

    '    Try

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, "QUALITY", LMargin + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdAr(prn_HdMxIndx, 2), LMargin + W1 + 25, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt + 10
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(2) = CurY

    '        CurY = CurY + TxtHgt - 10
    '        Common_Procedures.Print_To_PrintDocument(e, "BALE NO", LMargin, CurY, 2, ClAr(1), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "PCS-1", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "PCS-2", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "PCS-3", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "PCS-4", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "PCS-5", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "PCS-6", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "PCS-7", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)

    '        CurY = CurY + TxtHgt + 10
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(3) = CurY

    '    Catch ex As Exception

    '        MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try

    'End Sub

    'Private Sub Printing_PackingSlip_Format2_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByRef prn_HdAr(,) As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal prn_HdIndx As Integer, ByVal is_LastPage As Boolean)
    '    Dim I As Integer
    '    Dim p1Font As Font

    '    Try

    '        For I = NoofDets + 1 To NoofItems_PerPage
    '            CurY = CurY + TxtHgt
    '        Next

    '        CurY = CurY + TxtHgt
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(4) = CurY


    '        ' Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_HdAr(prn_HdIndx, 3))), LMargin + ClAr(1) + 15, CurY, 0, 0, pFont)
    '        ' Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdAr(prn_HdIndx, 4)), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 15, CurY, 1, 0, pFont)

    '        CurY = CurY + TxtHgt - 10
    '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdAr(prn_HdIndx, 3))), LMargin + ClAr(1), CurY, 1, 0, pFont)

    '        Common_Procedures.Print_To_PrintDocument(e, Format(Val(Total_mtrs), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 2, CurY, 1, 0, pFont)

    '        CurY = CurY + TxtHgt + 5
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(5) = CurY

    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(2))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + 44, CurY, LMargin + ClAr(1) + 44, LnAr(3))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(2))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + 44, CurY, LMargin + ClAr(1) + ClAr(2) + 44, LnAr(3))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(2))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 44, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 44, LnAr(3))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(2))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 44, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 44, LnAr(3))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(2))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 44, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 44, LnAr(3))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(2))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 44, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 44, LnAr(3))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(2))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 44, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 44, LnAr(3))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(2))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(2))
    '        e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(2))


    '        'CurY = CurY + TxtHgt - 10
    '        'CurY = CurY + TxtHgt + 5
    '        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
    '        'LnAr(6) = CurY
    '        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(2))
    '        'CurY = CurY + TxtHgt
    '        CurY = CurY + TxtHgt - 10

    '        p1Font = New Font("Calibri", 12, FontStyle.Bold)

    '        Common_Procedures.Print_To_PrintDocument(e, "For " & Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString), PageWidth - 15, CurY, 1, 0, p1Font)
    '        CurY = CurY + TxtHgt
    '        CurY = CurY + TxtHgt
    '        CurY = CurY + TxtHgt

    '        p1Font = New Font("Calibri", 12, FontStyle.Bold)

    '        Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
    '        CurY = CurY + TxtHgt + 10

    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
    '        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)


    '    Catch ex As Exception

    '        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try

    'End Sub

    'Private Sub Set_Max_DetailsSlNo(ByVal RowNo As Integer, ByVal DetSlNo_ColNo As Integer)
    '    Dim MaxSlNo As Integer = 0
    '    Dim i As Integer

    '    With dgv_Details
    '        For i = 0 To .Rows.Count - 1
    '            If Val(.Rows(i).Cells(DetSlNo_ColNo).Value) > Val(MaxSlNo) Then
    '                MaxSlNo = Val(.Rows(i).Cells(DetSlNo_ColNo).Value)
    '            End If
    '        Next
    '        .Rows(RowNo).Cells(DetSlNo_ColNo).Value = Val(MaxSlNo) + 1
    '    End With

    'End Sub

    'Private Sub btn_PDF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_PDF.Click
    '    Common_Procedures.Print_OR_Preview_Status = 1
    '    Print_PDF_Status = True
    '    print_record()
    '    'Print_PDF_Status = False
    'End Sub

    'Private Sub btn_EMail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_EMail.Click
    '    Dim Led_IdNo As Integer
    '    Dim MailTxt As String

    '    Try

    '        Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)

    '        MailTxt = "INVOICE " & vbCrLf & vbCrLf
    '        MailTxt = MailTxt & "Invoice No.-" & Trim(lbl_InvNo.Text) & vbCrLf & "Date-" & Trim(msk_Date.Text)
    '        MailTxt = MailTxt & vbCrLf & "Lr No.-" & Trim(txt_LrNo.Text) & IIf(Trim(msk_Lr_Date.Text) <> "", " Dt.", "") & Trim(msk_Lr_Date.Text)
    '        MailTxt = MailTxt & vbCrLf & "Value-" & Trim(lbl_Net_Amt.Text)

    '        EMAIL_Entry.vMailID = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Mail", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")
    '        EMAIL_Entry.vSubJect = "Invocie : " & Trim(lbl_InvNo.Text)
    '        EMAIL_Entry.vMessage = Trim(MailTxt)

    '        Dim f1 As New EMAIL_Entry
    '        f1.MdiParent = MDIParent1
    '        f1.Show()

    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "DOES NOT SEND MAIL...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try

    'End Sub

    'Private Sub btn_SMS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SMS.Click
    '    Dim i As Integer = 0
    '    Dim smstxt As String = ""
    '    Dim PhNo As String = "", AgPNo As String = ""
    '    Dim Led_IdNo As Integer = 0, Agnt_IdNo As Integer = 0
    '    Dim SMS_SenderID As String = ""
    '    Dim SMS_Key As String = ""
    '    Dim SMS_RouteID As String = ""
    '    Dim SMS_Type As String = ""
    '    Dim BlNos As String = ""

    '    Try

    '        Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)

    '        PhNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "MobileNo_Frsms", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")

    '        Agnt_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Agent.Text)
    '        AgPNo = ""
    '        If Val(Agnt_IdNo) <> 0 Then
    '            AgPNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_PhoneNo", "(Ledger_IdNo = " & Str(Val(Agnt_IdNo)) & ")")
    '        End If

    '        If Trim(AgPNo) <> "" Then
    '            PhNo = Trim(PhNo) & IIf(Trim(PhNo) <> "", ",", "") & Trim(AgPNo)
    '        End If

    '        smstxt = Trim(cbo_PartyName.Text) & Chr(13)
    '        smstxt = smstxt & " Inv No : " & Trim(lbl_InvNo.Text) & Chr(13)
    '        smstxt = smstxt & " Date : " & Trim(msk_Date.Text) & Chr(13)
    '        If Trim(cbo_Transport.Text) <> "" Then
    '            smstxt = smstxt & " Transport : " & Trim(cbo_Transport.Text) & Chr(13)
    '        End If
    '        If Trim(txt_LrNo.Text) <> "" Then
    '            smstxt = smstxt & " Lr No : " & Trim(txt_LrNo.Text) & Chr(13)
    '            If Trim(msk_Lr_Date.Text) <> "" Then
    '                smstxt = smstxt & " Dt : " & Trim(msk_Lr_Date.Text) & Chr(13)
    '            End If
    '        End If
    '        If Trim(cbo_DespTo.Text) <> "" Then
    '            smstxt = smstxt & " Despatch To : " & Trim(cbo_DespTo.Text) & Chr(13)
    '        End If
    '        If dgv_Details_Total.RowCount > 0 Then
    '            smstxt = smstxt & " No.Of Bales : " & Val((dgv_Details_Total.Rows(0).Cells(4).Value())) & Chr(13)
    '            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Then '---- Prakash Textiles (Somanur)
    '                BlNos = ""
    '                For i = 0 To dgv_Details.Rows.Count - 1
    '                    If Val(dgv_Details_Total.Rows(0).Cells(7).Value()) <> 0 Then
    '                        BlNos = BlNos & IIf(Trim(BlNos) <> "", ", ", "") & Trim(dgv_Details.Rows(0).Cells(5).Value)
    '                    End If
    '                Next
    '                smstxt = smstxt & " Bales No.s : " & Trim(blnos) & Chr(13)
    '            End If
    '            smstxt = smstxt & " Meters : " & Val(dgv_Details_Total.Rows(0).Cells(7).Value()) & Chr(13)
    '        End If
    '        'If dgv_Details.RowCount > 0 Then
    '        '    smstxt = smstxt & " No.Of Bales : " & Val((dgv_Details.Rows(0).Cells(4).Value())) & Chr(13)
    '        '    smstxt = smstxt & " Meters : " & Val((dgv_Details.Rows(0).Cells(7).Value())) & Chr(13)
    '        'End If
    '        smstxt = smstxt & " Bill Amount : " & Trim(lbl_Net_Amt.Text) & Chr(13)
    '        smstxt = smstxt & " " & Chr(13)
    '        smstxt = smstxt & " Thanks! " & Chr(13)
    '        smstxt = smstxt & Common_Procedures.Company_IdNoToName(con, Val(lbl_Company.Tag))

    '        SMS_SenderID = ""
    '        SMS_Key = ""
    '        SMS_RouteID = ""
    '        SMS_Type = ""

    '        Common_Procedures.get_SMS_Provider_Details(con, Val(lbl_Company.Tag), SMS_SenderID, SMS_Key, SMS_RouteID, SMS_Type)


    '        Sms_Entry.vSmsPhoneNo = Trim(PhNo)
    '        Sms_Entry.vSmsMessage = Trim(smstxt)

    '        Sms_Entry.SMSProvider_SenderID = SMS_SenderID
    '        Sms_Entry.SMSProvider_Key = SMS_Key
    '        Sms_Entry.SMSProvider_RouteID = SMS_RouteID
    '        Sms_Entry.SMSProvider_Type = SMS_Type

    '        Dim f1 As New Sms_Entry
    '        f1.MdiParent = MDIParent1
    '        f1.Show()

    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "DOES NOT SEND SMS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try
    'End Sub

    'Private Sub btn_Buyer_Select_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_BuyerOffer_Select.Click
    '    Dim Da As New SqlClient.SqlDataAdapter
    '    Dim Dt1 As New DataTable
    '    Dim i As Integer, j As Integer, n As Integer, SNo As Integer
    '    Dim LedNo As Integer
    '    Dim NewCode As String

    '    LedNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)
    '    If LedNo = 0 Then
    '        MessageBox.Show("Invalid Party Name", "DOES NOT SELECT BUYER OFFER...", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        If cbo_PartyName.Enabled And cbo_PartyName.Visible Then cbo_PartyName.Focus()
    '        Exit Sub
    '    End If

    '    NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

    '    With dgv_BuyerOffer_Selection

    '        .Rows.Clear()

    '        SNo = 0

    '        Da = New SqlClient.SqlDataAdapter("select a.*, c.Cloth_Name, d.ClothType_Name from Buyer_Offer_Head a INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo INNER JOIN ClothType_Head d ON a.ClothType_IdNo = d.ClothType_IdNo where a.Ledger_IdNo = " & Str(Val(LedNo)) & " and a.Processed_Fabric_Sales_Invoice_Code = '" & Trim(NewCode) & "' Order by a.Buyer_Offer_Date, a.For_OrderBy, a.Buyer_Offer_No", con)
    '        Dt1 = New DataTable
    '        Da.Fill(Dt1)

    '        If Dt1.Rows.Count > 0 Then

    '            For i = 0 To Dt1.Rows.Count - 1

    '                n = .Rows.Add()

    '                SNo = SNo + 1
    '                .Rows(n).Cells(0).Value = Val(SNo)
    '                .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Buyer_offer_No").ToString
    '                .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Buyer_offer_Date").ToString), "dd-MM-yyyy")
    '                .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
    '                .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("ClothType_Name").ToString
    '                .Rows(n).Cells(5).Value = Val(Dt1.Rows(i).Item("Folding").ToString)
    '                .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Pcs").ToString)
    '                .Rows(n).Cells(7).Value = Val(Dt1.Rows(i).Item("Meters").ToString)
    '                .Rows(n).Cells(8).Value = "1"
    '                .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Buyer_Offer_Code").ToString

    '                For j = 0 To .ColumnCount - 1
    '                    .Rows(i).Cells(j).Style.ForeColor = Color.Red
    '                Next

    '            Next

    '        End If
    '        Dt1.Clear()

    '        Da = New SqlClient.SqlDataAdapter("select a.*, c.Cloth_Name, d.ClothType_Name from Buyer_Offer_Head a INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo INNER JOIN ClothType_Head d ON a.ClothType_IdNo = d.ClothType_IdNo where a.Ledger_IdNo = " & Str(Val(LedNo)) & " and a.Processed_Fabric_Sales_Invoice_Code = '' Order by a.Buyer_Offer_Date, a.For_OrderBy, a.Buyer_Offer_No", con)
    '        Dt1 = New DataTable
    '        Da.Fill(Dt1)

    '        If Dt1.Rows.Count > 0 Then

    '            For i = 0 To Dt1.Rows.Count - 1

    '                n = .Rows.Add()

    '                SNo = SNo + 1
    '                .Rows(n).Cells(0).Value = Val(SNo)
    '                .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Buyer_offer_No").ToString
    '                .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Buyer_offer_Date").ToString), "dd-MM-yyyy")
    '                .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
    '                .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("ClothType_Name").ToString
    '                .Rows(n).Cells(5).Value = Val(Dt1.Rows(i).Item("Folding").ToString)
    '                .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Pcs").ToString)
    '                .Rows(n).Cells(7).Value = Val(Dt1.Rows(i).Item("Meters").ToString)
    '                .Rows(n).Cells(8).Value = ""
    '                .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Buyer_Offer_Code").ToString


    '            Next

    '        End If
    '        Dt1.Clear()

    '        If .Rows.Count = 0 Then .Rows.Add()

    '        pnl_BuyerOffer_Selection.Visible = True
    '        pnl_BuyerOffer_Selection.BringToFront()
    '        pnl_BuyerOffer_Details.Enabled = False
    '        .Focus()
    '        .CurrentCell = .Rows(0).Cells(0)

    '    End With

    'End Sub

    'Private Sub dgv_Buyeroffer_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BuyerOffer_Selection.CellClick
    '    If dgv_BuyerOffer_Selection.Rows.Count > 0 Then
    '        If e.RowIndex >= 0 Then
    '            Select_BuyerOffer(e.RowIndex)
    '        End If
    '    End If
    'End Sub

    'Private Sub dgv_BuyerOffer_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_BuyerOffer_Selection.KeyDown
    '    Dim n As Integer

    '    Try
    '        With dgv_BuyerOffer_Selection
    '            If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
    '                If .CurrentCell.RowIndex >= 0 Then

    '                    n = .CurrentCell.RowIndex

    '                    Select_BuyerOffer(n)

    '                    e.Handled = True

    '                End If
    '            End If
    '        End With

    '    Catch ex As Exception
    '        '---

    '    End Try

    'End Sub

    'Private Sub Select_BuyerOffer(ByVal RwIndx As Integer)
    '    Dim i As Integer

    '    With dgv_BuyerOffer_Selection

    '        If .RowCount > 0 And RwIndx >= 0 Then

    '            .Rows(RwIndx).Cells(8).Value = (Val(.Rows(RwIndx).Cells(8).Value) + 1) Mod 2

    '            If Val(.Rows(RwIndx).Cells(8).Value) = 0 Then
    '                .Rows(RwIndx).Cells(8).Value = ""
    '                For i = 0 To .ColumnCount - 1
    '                    .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Blue
    '                Next

    '            Else
    '                For i = 0 To .ColumnCount - 1
    '                    .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
    '                Next

    '            End If

    '        End If

    '    End With

    'End Sub

    'Private Sub Buyer_Offer_Selection()
    '    Dim Da1 As New SqlClient.SqlDataAdapter
    '    Dim Dt1 As New DataTable
    '    Dim n As Integer = 0
    '    Dim sno As Integer = 0
    '    Dim i As Integer = 0
    '    Dim j As Integer = 0

    '    With dgv_Buyer_Offer_Detail
    '        .Rows.Clear()

    '        sno = 0
    '        For i = 0 To dgv_BuyerOffer_Selection.RowCount - 1

    '            If Val(dgv_BuyerOffer_Selection.Rows(i).Cells(8).Value) = 1 Then

    '                n = .Rows.Add()
    '                sno = sno + 1
    '                .Rows(n).Cells(0).Value = Val(sno)
    '                .Rows(n).Cells(1).Value = dgv_BuyerOffer_Selection.Rows(i).Cells(1).Value
    '                .Rows(n).Cells(2).Value = dgv_BuyerOffer_Selection.Rows(i).Cells(2).Value
    '                .Rows(n).Cells(3).Value = dgv_BuyerOffer_Selection.Rows(i).Cells(3).Value
    '                .Rows(n).Cells(4).Value = dgv_BuyerOffer_Selection.Rows(i).Cells(4).Value
    '                .Rows(n).Cells(5).Value = dgv_BuyerOffer_Selection.Rows(i).Cells(5).Value
    '                .Rows(n).Cells(6).Value = dgv_BuyerOffer_Selection.Rows(i).Cells(6).Value
    '                .Rows(n).Cells(7).Value = dgv_BuyerOffer_Selection.Rows(i).Cells(7).Value
    '                .Rows(n).Cells(8).Value = dgv_BuyerOffer_Selection.Rows(i).Cells(9).Value

    '            End If

    '        Next

    '        If .Rows.Count = 0 Then .Rows.Add()

    '        pnl_BuyerOffer_Details.Enabled = True
    '        pnl_BuyerOffer_Selection.Visible = False
    '        .Focus()
    '        .CurrentCell = .Rows(0).Cells(0)

    '    End With

    'End Sub

    'Private Sub btn_Close_BuyerOffer_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close_BuyerOffer_Selection.Click
    '    Buyer_Offer_Selection()
    'End Sub

    'Private Sub btn_buyerofferSelction_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_buyerofferSelction.Click
    '    pnl_BuyerOffer_Details.Enabled = True
    '    pnl_BuyerOffer_Details.Visible = True
    '    pnl_Back.Enabled = False

    '    If dgv_Buyer_Offer_Detail.Rows.Count = 0 Then dgv_Buyer_Offer_Detail.Rows.Add()

    '    dgv_Buyer_Offer_Detail.Focus()
    '    dgv_Buyer_Offer_Detail.CurrentCell = dgv_Buyer_Offer_Detail.Rows(0).Cells(0)

    '    'If btn_BuyerOffer_Select.Enabled And btn_BuyerOffer_Select.Visible Then btn_BuyerOffer_Select.Focus()
    'End Sub

    'Private Sub btn_BuyerOffer_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_BuyerOffer_Close.Click
    '    btn_Close_BuyerOffer_Details_Click(sender, e)
    'End Sub

    'Private Sub btn_Close_BuyerOffer_Details_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close_BuyerOffer_Details.Click
    '    pnl_Back.Enabled = True
    '    pnl_BuyerOffer_Details.Visible = False
    'End Sub

    'Private Sub txt_BaleSelction_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_BaleSelction.KeyDown
    '    If e.KeyValue = 40 Then
    '        If dgv_BaleSelection.Rows.Count > 0 Then
    '            dgv_BaleSelection.Focus()
    '            dgv_BaleSelection.CurrentCell = dgv_BaleSelection.Rows(0).Cells(0)
    '            dgv_BaleSelection.CurrentCell.Selected = True
    '        Else
    '            btn_lot_Pcs_selection.Focus()
    '        End If
    '    End If
    'End Sub

    'Private Sub txt_BaleSelction_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_BaleSelction.KeyPress
    '    If Asc(e.KeyChar) = 13 Then

    '        If Trim(txt_BaleSelction.Text) <> "" Then
    '            btn_lot_Pcs_selection_Click(sender, e)

    '        Else
    '            If dgv_BaleSelection.Rows.Count > 0 Then
    '                dgv_BaleSelection.Focus()
    '                dgv_BaleSelection.CurrentCell = dgv_BaleSelection.Rows(0).Cells(0)
    '                dgv_BaleSelection.CurrentCell.Selected = True
    '            End If

    '        End If

    '    End If
    'End Sub

    'Private Sub btn_lot_Pcs_selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_lot_Pcs_selection.Click
    '    Dim LtNo As String
    '    Dim i As Integer

    '    If Trim(txt_BaleSelction.Text) <> "" Then

    '        LtNo = Trim(txt_BaleSelction.Text)

    '        For i = 0 To dgv_BaleSelection.Rows.Count - 1
    '            If Trim(UCase(LtNo)) = Trim(UCase(dgv_BaleSelection.Rows(i).Cells(1).Value)) Then
    '                Call Select_Bale(i)
    '                dgv_BaleSelection.CurrentCell = dgv_BaleSelection.Rows(i).Cells(0)
    '                If i >= 9 Then dgv_BaleSelection.FirstDisplayedScrollingRowIndex = i - 8
    '                Exit For
    '            End If
    '        Next

    '        txt_BaleSelction.Text = ""
    '        If txt_BaleSelction.Enabled = True Then txt_BaleSelction.Focus()

    '    End If

    'End Sub

    'Private Sub chk_SelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk_SelectAll.CheckedChanged
    '    Dim i As Integer
    '    Dim J As Integer

    '    With dgv_BaleSelection

    '        For i = 0 To .Rows.Count - 1
    '            .Rows(i).Cells(5).Value = ""
    '            For J = 0 To .ColumnCount - 1
    '                .Rows(i).Cells(J).Style.ForeColor = Color.Black
    '            Next J
    '        Next i

    '        If chk_SelectAll.Checked = True Then
    '            For i = 0 To .Rows.Count - 1
    '                Select_Bale(i)
    '            Next i
    '        End If

    '        If .Rows.Count > 0 Then
    '            .Focus()
    '            .CurrentCell = .Rows(0).Cells(0)
    '            .CurrentCell.Selected = True
    '        End If

    '    End With

    'End Sub

    'Private Sub Printing_Format2(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
    '    Dim cmd As New SqlClient.SqlCommand
    '    Dim Da1 As New SqlClient.SqlDataAdapter
    '    Dim Dt1 As New DataTable
    '    Dim EntryCode As String
    '    Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
    '    Dim pFont As Font
    '    Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
    '    Dim PrintWidth As Single, PrintHeight As Single
    '    Dim PageWidth As Single, PageHeight As Single
    '    Dim CurY As Single, TxtHgt As Single
    '    Dim LnAr(15) As Single, ClAr(15) As Single
    '    Dim ItmNm1 As String, ItmNm2 As String
    '    Dim ps As Printing.PaperSize
    '    Dim strHeight As Single = 0
    '    Dim PpSzSTS As Boolean = False
    '    Dim W1 As Single = 0
    '    Dim SNo As Integer = 0
    '    Dim FldLessPerc As Single = 0
    '    Dim FldLessMtr As Single = 0
    '    Dim fmtr As Single = 0
    '    Dim FldPerc As Single = 0
    '    Dim strFldPerCM As String = ""

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
    '        .Left = 20 ' 30 
    '        .Right = 40
    '        .Top = 30 ' 50 
    '        .Bottom = 40
    '        LMargin = .Left
    '        RMargin = .Right
    '        TMargin = .Top
    '        BMargin = .Bottom
    '    End With

    '    pFont = New Font("Calibri", 10, FontStyle.Regular)

    '    e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

    '    With PrintDocument1.DefaultPageSettings.PaperSize
    '        PrintWidth = .Width - RMargin - LMargin
    '        PrintHeight = .Height - TMargin - BMargin
    '        PageWidth = .Width - RMargin
    '        PageHeight = .Height - BMargin
    '    End With

    '    NoofItems_PerPage = 5 ' 6

    '    Erase LnAr
    '    Erase ClAr

    '    LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    '    ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

    '    ClAr(1) = 45 : ClAr(2) = 260 : ClAr(3) = 80 : ClAr(4) = 150 : ClAr(5) = 85 ': ClAr(6) = 80
    '    ClAr(6) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5))

    '    'ClAr(1) = Val(50) : ClAr(2) = 240 : ClAr(3) = 80 : ClAr(4) = 70 : ClAr(5) = 100 : ClAr(6) = 80
    '    'ClAr(7) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6))

    '    TxtHgt = 19  ' e.Graphics.MeasureString("A", pFont).Height  ' 20

    '    ''=========================================================================================================
    '    ''------  START OF PREPRINT POINTS
    '    ''=========================================================================================================

    '    'pFont = New Font("Calibri", 11, FontStyle.Regular)

    '    'Dim CurX As Single = 0
    '    'Dim pFont1 As Font

    '    'pFont1 = New Font("Calibri", 8, FontStyle.Regular)

    '    'For I = 100 To 1100 Step 300

    '    '    CurY = I
    '    '    For J = 1 To 850 Step 40

    '    '        CurX = J
    '    '        Common_Procedures.Print_To_PrintDocument(e, CurX, CurX, CurY - 20, 0, 0, pFont1)
    '    '        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY, 0, 0, pFont)

    '    '        CurX = J + 20
    '    '        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY, 0, 0, pFont)
    '    '        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY + 20, 0, 0, pFont)
    '    '        Common_Procedures.Print_To_PrintDocument(e, CurX, CurX, CurY + 40, 0, 0, pFont1)

    '    '    Next

    '    'Next

    '    'For I = 200 To 800 Step 250

    '    '    CurX = I
    '    '    For J = 1 To 1200 Step 40

    '    '        CurY = J
    '    '        Common_Procedures.Print_To_PrintDocument(e, "-", CurX, CurY, 0, 0, pFont)
    '    '        Common_Procedures.Print_To_PrintDocument(e, "   " & CurY, CurX, CurY, 0, 0, pFont1)

    '    '        CurY = J + 20
    '    '        Common_Procedures.Print_To_PrintDocument(e, "--", CurX, CurY, 0, 0, pFont)
    '    '        Common_Procedures.Print_To_PrintDocument(e, "   " & CurY, CurX, CurY, 0, 0, pFont1)

    '    '    Next

    '    'Next

    '    'e.HasMorePages = False

    '    'Exit Sub

    '    ''=========================================================================================================
    '    ''------  END OF PREPRINT POINTS
    '    ''=========================================================================================================

    '    EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

    '    Try
    '        If prn_HdDt.Rows.Count > 0 Then

    '            Printing_Format2_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

    '            NoofDets = 0

    '            CurY = CurY - 10

    '            If prn_DetDt.Rows.Count > 0 Then

    '                Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

    '                    If NoofDets >= NoofItems_PerPage Then

    '                        CurY = CurY + TxtHgt

    '                        Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 10, CurY, 0, 0, pFont)

    '                        NoofDets = NoofDets + 1

    '                        Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

    '                        e.HasMorePages = True
    '                        Return

    '                    End If

    '                    If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString) <> "" Then
    '                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString)
    '                    Else
    '                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Name").ToString)
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

    '                    FldPerc = Val(prn_DetDt.Rows(prn_DetIndx).Item("Fold_Perc").ToString)
    '                    If Val(FldPerc) = 0 Then FldPerc = 100


    '                    If Val(FldPerc) = 0 Or Val(FldPerc) = 100 Or Trim(prn_HdDt.Rows(0).Item("FoldingRate_Status").ToString) = 1 Then
    '                        CurY = CurY + TxtHgt + 10
    '                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
    '                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
    '                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bales").ToString) <> 0 Then
    '                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bales").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 20, CurY, 1, 0, pFont)
    '                        End If

    '                        strFldPerCM = Val(FldPerc) & " cm"
    '                        Common_Procedures.Print_To_PrintDocument(e, strFldPerCM, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 13, CurY, 0, 0, pFont)
    '                        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 60, CurY, 0, 0, pFont)

    '                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
    '                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
    '                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

    '                    Else

    '                        CurY = CurY + TxtHgt + 10
    '                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
    '                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
    '                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bales").ToString) <> 0 Then
    '                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bales").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 20, CurY, 1, 0, pFont)
    '                        End If

    '                        strFldPerCM = Val(FldPerc) & " cm"
    '                        Common_Procedures.Print_To_PrintDocument(e, strFldPerCM, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 13, CurY, 0, 0, pFont)
    '                        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 60, CurY, 0, 0, pFont)

    '                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)

    '                        'fmt = ((100 - Val(.Rows(CurRow).Cells(3).Value)) / 100) * Val(.Rows(CurRow).Cells(7).Value)
    '                        'fmt = Format(Math.Abs(Val(fmt)), "######0.00")
    '                        'fmt = Common_Procedures.Meter_RoundOff(fmt)
    '                        If Trim(ItmNm2) <> "" Then
    '                            CurY = CurY + TxtHgt - 5
    '                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
    '                            NoofDets = NoofDets + 1
    '                        End If

    '                        FldLessPerc = 100 - Val(FldPerc)

    '                        FldLessMtr = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString) * FldLessPerc / 100, "#########0.00")

    '                        FldLessMtr = Math.Abs(Val(FldLessMtr))

    '                        FldLessMtr = Common_Procedures.Meter_RoundOff(FldLessMtr)

    '                        CurY = CurY + TxtHgt
    '                        If Val(FldLessPerc) > 0 Then
    '                            Common_Procedures.Print_To_PrintDocument(e, "Folding Less", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
    '                            'Common_Procedures.Print_To_PrintDocument(e, Val(FldLessPerc) & "%  Folding Less", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
    '                        Else
    '                            Common_Procedures.Print_To_PrintDocument(e, "Folding Add", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
    '                            'Common_Procedures.Print_To_PrintDocument(e, Val(FldLessPerc) & "%  Folding Add", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
    '                        End If

    '                        strFldPerCM = Val(FldLessPerc) & " cm"
    '                        Common_Procedures.Print_To_PrintDocument(e, strFldPerCM, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 13, CurY, 0, 0, pFont)
    '                        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 60, CurY, 0, 0, pFont)

    '                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(FldLessMtr), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)

    '                        CurY = CurY + TxtHgt
    '                        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY)

    '                        If Val(FldLessPerc) > 0 Then
    '                            fmtr = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString) - Val(FldLessMtr), "#########0.00")
    '                        Else
    '                            fmtr = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString) + Val(FldLessMtr), "#########0.00")
    '                        End If

    '                        strFldPerCM = "100 cm"
    '                        Common_Procedures.Print_To_PrintDocument(e, strFldPerCM, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 13, CurY, 0, 0, pFont)
    '                        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 60, CurY, 0, 0, pFont)

    '                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(fmtr), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
    '                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
    '                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

    '                    End If

    '                    NoofDets = NoofDets + 1

    '                    prn_DetIndx = prn_DetIndx + 1

    '                Loop

    '                'If Trim(UCase(Common_Procedures.settings.CompanyName)) = "1009" Or Trim(UCase(Common_Procedures.settings.CompanyName)) = "1018" Then
    '                '    CurY = CurY + TxtHgt
    '                '    CurY = CurY + TxtHgt - 5
    '                '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
    '                '    NoofDets = NoofDets + 2
    '                'End If

    '            End If

    '            Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

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

    'Private Sub Printing_Format2_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
    '    Dim da2 As New SqlClient.SqlDataAdapter
    '    Dim dt2 As New DataTable
    '    Dim p1Font As Font
    '    Dim strHeight As Single
    '    Dim C1 As Single, W1, W2, W3 As Single, S1, S2 As Single
    '    Dim Cmp_Name, Desc As String, Cmp_Add1 As String, Cmp_Add2 As String
    '    Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
    '    Dim S As String

    '    PageNo = PageNo + 1

    '    CurY = TMargin

    '    'da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Ledger_Name, d.Company_Description as Transport_Name from Processed_Fabric_Sales_Invoice_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Transport_IdNo LEFT OUTER JOIN Company_Head d ON d.Company_IdNo = a.Company_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Processed_Fabric_Sales_Invoice_Code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
    '    'da2.Fill(dt2)
    '    'If dt2.Rows.Count > NoofItems_PerPage Then
    '    '    Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
    '    'End If
    '    'dt2.Clear()

    '    prn_Count = prn_Count + 1

    '    prn_OriDupTri = ""
    '    If Trim(prn_InpOpts) <> "" Then
    '        If prn_Count <= Len(Trim(prn_InpOpts)) Then

    '            S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

    '            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
    '                If Val(S) = 1 Then
    '                    prn_OriDupTri = "ORIGINAL"
    '                ElseIf Val(S) = 2 Then
    '                    prn_OriDupTri = "TRANSPORT COPY"
    '                ElseIf Val(S) = 3 Then
    '                    prn_OriDupTri = "TRIPLICATE"
    '                ElseIf Val(S) = 4 Then
    '                    prn_OriDupTri = "EXTRA COPY"
    '                Else
    '                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
    '                        prn_OriDupTri = Trim(prn_InpOpts)
    '                    End If
    '                End If

    '            Else
    '                If Val(S) = 1 Then
    '                    prn_OriDupTri = "ORIGINAL"
    '                ElseIf Val(S) = 2 Then
    '                    prn_OriDupTri = "DUPLICATE"
    '                ElseIf Val(S) = 3 Then
    '                    prn_OriDupTri = "TRIPLICATE"
    '                ElseIf Val(S) = 4 Then
    '                    prn_OriDupTri = "EXTRA COPY"
    '                Else
    '                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
    '                        prn_OriDupTri = Trim(prn_InpOpts)
    '                    End If
    '                End If

    '            End If

    '        End If
    '    End If

    '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
    '        p1Font = New Font("Calibri", 15, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY - TxtHgt - 5, 2, PrintWidth, p1Font)
    '    End If
    '    If Trim(prn_OriDupTri) <> "" Then
    '        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
    '    End If

    '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '    LnAr(1) = CurY
    '    Desc = ""
    '    Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
    '    Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_EMail = ""

    '    Desc = prn_HdDt.Rows(0).Item("Company_Description").ToString
    '    Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
    '    Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
    '    Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

    '    If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
    '        Cmp_PhNo = "PHONE : " & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
    '        Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
    '        Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
    '        Cmp_EMail = "MAIL ID : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
    '    End If

    '    p1Font = New Font("Calibri", 15, FontStyle.Bold)
    '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1018" Then '---- M.K Textiles (Palladam)
    '        p1Font = New Font("Calibri", 15, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
    '    End If
    '    CurY = CurY + TxtHgt
    '    strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


    '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
    '        e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MK, Drawing.Image), LMargin + 20, CurY, 112, 80)
    '        'e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MK_2, Drawing.Image), LMargin + 20, CurY, 115, 80)
    '        'e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MK, Drawing.Image), LMargin + 20, CurY, 75, 75)
    '    End If

    '    p1Font = New Font("Calibri", 18, FontStyle.Bold)
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
    '    strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


    '    CurY = CurY + strHeight - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Desc, LMargin, CurY, 2, PrintWidth, pFont)

    '    CurY = CurY + TxtHgt - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

    '    CurY = CurY + TxtHgt - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
    '    CurY = CurY + TxtHgt - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
    '    CurY = CurY + TxtHgt - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, LMargin, CurY, 2, PrintWidth, pFont)
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)


    '    CurY = CurY + TxtHgt + 10
    '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '    LnAr(2) = CurY

    '    Try
    '        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
    '        W1 = e.Graphics.MeasureString("INVOICE DATE  : ", pFont).Width
    '        S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width
    '        W2 = e.Graphics.MeasureString("Despatch To   : ", pFont).Width
    '        S2 = e.Graphics.MeasureString("Sent Through  : ", pFont).Width


    '        CurY = CurY + 10
    '        p1Font = New Font("Calibri", 12, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
    '        Common_Procedures.Print_To_PrintDocument(e, "INVOICE NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
    '        If prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString & "-" & prn_HdDt.Rows(0).Item("Processed_Fabric_Sales_Invoice_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
    '        Else
    '            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Processed_Fabric_Sales_Invoice_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
    '        End If


    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '        p1Font = New Font("Calibri", 14, FontStyle.Bold)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "INVOICE DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Processed_Fabric_Sales_Invoice_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)


    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '        If Trim(prn_HdDt.Rows(0).Item("Dc_No").ToString) <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, "DC NO : " & prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
    '        End If
    '        If Trim(prn_HdDt.Rows(0).Item("Dc_Date").ToString) <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, "DC DATE : " & prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + C1 + 100, CurY, 0, 0, pFont)
    '        End If

    '        CurY = CurY + TxtHgt
    '        If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, " TIN : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '        End If

    '        CurY = CurY + TxtHgt
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
    '        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(2))
    '        LnAr(3) = CurY
    '        CurY = CurY + 10
    '        Common_Procedures.Print_To_PrintDocument(e, "Agent Name ", LMargin + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)


    '        Common_Procedures.Print_To_PrintDocument(e, "Transport ", LMargin + C1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + C1 + W2 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + C1 + W2 + 30, CurY, 0, 0, pFont)


    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, "Order No ", LMargin + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_OrderNo").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)

    '        Common_Procedures.Print_To_PrintDocument(e, "Lr.No  ", LMargin + C1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + C1 + W2 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lr_No").ToString, LMargin + C1 + W2 + 30, CurY, 0, 0, pFont)
    '        If Trim(prn_HdDt.Rows(0).Item("Lr_No").ToString) <> "" And Trim(prn_HdDt.Rows(0).Item("Lr_Date").ToString) <> "" Then
    '            W3 = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Lr_No").ToString, pFont).Width
    '            Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Lr_Date").ToString, LMargin + C1 + W2 + W3 + 40, CurY, 0, 0, pFont)
    '        End If


    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, "Lc No ", LMargin + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lc_No").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)
    '        If Trim(prn_HdDt.Rows(0).Item("Lc_No").ToString) <> "" And Trim(prn_HdDt.Rows(0).Item("Lc_Date").ToString) <> "" Then
    '            W3 = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Lc_No").ToString, pFont).Width
    '            Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Lc_Date").ToString, LMargin + S2 + W3 + 35, CurY, 0, 0, pFont)
    '        End If

    '        'Common_Procedures.Print_To_PrintDocument(e, "Transport ", LMargin + 10, CurY, 0, 0, pFont)
    '        'Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
    '        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "Despatch To", LMargin + C1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W2 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Despatch_To").ToString, LMargin + C1 + W2 + 30, CurY, 0, 0, pFont)


    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, "Sent Through ", LMargin + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Through_Name").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Delivery_Address1").ToString, LMargin + C1 + W2 + 30, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Delivery_Address2").ToString, LMargin + C1 + W2 + 30, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(4) = CurY

    '        CurY = CurY + 10
    '        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)

    '        Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)

    '        Common_Procedures.Print_To_PrintDocument(e, "BALES\", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "BUNDLES", LMargin + ClAr(1) + ClAr(2), CurY + TxtHgt - 3, 2, ClAr(3), pFont)

    '        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
    '        'Common_Procedures.Print_To_PrintDocument(e, "METER", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + TxtHgt, 2, ClAr(5), pFont)

    '        Common_Procedures.Print_To_PrintDocument(e, "RATE/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "METER", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + TxtHgt - 3, 2, ClAr(5), pFont)

    '        Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)

    '        CurY = CurY + TxtHgt + TxtHgt + 10
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(5) = CurY

    '        CurY = CurY + 10
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cloth_Details").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)

    '    Catch ex As Exception

    '        MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try

    'End Sub

    'Private Sub Printing_Format2_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
    '    Dim p1Font As Font
    '    Dim rndoff As Single, TtAmt As Double
    '    Dim I As Integer
    '    Dim BInc As Integer
    '    Dim BnkDetAr() As String
    '    Dim Cmp_Name As String
    '    Dim Lf1 As Single = 0
    '    Dim BmsInWrds As String
    '    Dim vprn_BlNos As String = ""
    '    Dim BLNo1 As String, BLNo2 As String
    '    Dim BankNm1 As String = ""
    '    Dim BankNm2 As String = ""
    '    Dim BankNm3 As String = ""
    '    Dim BankNm4 As String = ""

    '    Try

    '        For I = NoofDets + 1 To NoofItems_PerPage

    '            CurY = CurY + TxtHgt

    '            prn_DetIndx = prn_DetIndx + 1

    '        Next

    '        CurY = CurY + TxtHgt + 50
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(6) = CurY

    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
    '        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
    '        CurY += 10

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

    '        End If

    '        vprn_BlNos = ""
    '        For I = 0 To prn_DetDt.Rows.Count - 1
    '            If Trim(prn_DetDt.Rows(I).Item("Bales_Nos").ToString) <> "" Then
    '                vprn_BlNos = Trim(vprn_BlNos) & IIf(Trim(vprn_BlNos) <> "", ", ", "") & prn_DetDt.Rows(I).Item("Bales_Nos").ToString
    '            End If
    '        Next

    '        BLNo1 = Trim(vprn_BlNos)
    '        BLNo2 = ""
    '        If Len(BLNo1) > 30 Then
    '            For I = 30 To 1 Step -1
    '                If Mid$(Trim(BLNo1), I, 1) = " " Or Mid$(Trim(BLNo1), I, 1) = "," Or Mid$(Trim(BLNo1), I, 1) = "." Or Mid$(Trim(BLNo1), I, 1) = "-" Or Mid$(Trim(BLNo1), I, 1) = "/" Or Mid$(Trim(BLNo1), I, 1) = "_" Or Mid$(Trim(BLNo1), I, 1) = "(" Or Mid$(Trim(BLNo1), I, 1) = ")" Or Mid$(Trim(BLNo1), I, 1) = "\" Or Mid$(Trim(BLNo1), I, 1) = "[" Or Mid$(Trim(BLNo1), I, 1) = "]" Or Mid$(Trim(BLNo1), I, 1) = "{" Or Mid$(Trim(BLNo1), I, 1) = "}" Then Exit For
    '            Next I
    '            If I = 0 Then I = 30
    '            BLNo2 = Microsoft.VisualBasic.Right(Trim(BLNo1), Len(BLNo1) - I)
    '            BLNo1 = Microsoft.VisualBasic.Left(Trim(BLNo1), I - 1)
    '        End If

    '        If Trim(BLNo1) <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, "Bale/Bundle No : " & BLNo1, LMargin + 10, CurY, 0, 0, pFont)
    '        End If


    '        Lf1 = LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 50

    '        If Val(prn_HdDt.Rows(0).Item("Trade_Discount_Perc").ToString) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("TradeDisc_Name").ToString) & "  " & Trim(prn_HdDt.Rows(0).Item("Trade_Discount").ToString) & "%", Lf1, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 25, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Trade_Discount_Perc").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
    '        End If

    '        CurY = CurY + TxtHgt
    '        If Trim(BLNo2) <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, BLNo2, LMargin + 10, CurY, 0, 0, pFont)
    '        End If

    '        If Val(prn_HdDt.Rows(0).Item("Cash_Discount_Perc").ToString) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("CashDisc_Name").ToString) & "  " & Trim(prn_HdDt.Rows(0).Item("Cash_Discount").ToString) & "%", Lf1, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 25, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Cash_Discount_Perc").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
    '        End If

    '        CurY = CurY + TxtHgt
    '        If Val(prn_HdDt.Rows(0).Item("Bale_Weight").ToString) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, "Bale/Bundle Weight : " & Trim(prn_HdDt.Rows(0).Item("Bale_Weight").ToString), LMargin + 10, CurY, 0, 0, pFont)
    '        End If
    '        If Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Packing_Name").ToString), Lf1, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 25, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Packing_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
    '        End If

    '        CurY = CurY + TxtHgt
    '        p1Font = New Font("Calibri", 11, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY, 0, 0, p1Font)
    '        If Val(prn_HdDt.Rows(0).Item("Freight").ToString) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Freight_Name").ToString), Lf1, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 25, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Freight").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
    '        End If

    '        CurY = CurY + TxtHgt
    '        p1Font = New Font("Calibri", 11, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY, 0, 0, p1Font)
    '        If Val(prn_HdDt.Rows(0).Item("Insurance").ToString) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Insurance_Name").ToString), Lf1, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 25, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Insurance").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
    '        End If

    '        TtAmt = Val(prn_HdDt.Rows(0).Item("total_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Freight").ToString) + Val(prn_HdDt.Rows(0).Item("Insurance").ToString) + Val(prn_HdDt.Rows(0).Item("Packing_amount").ToString) - Val(prn_HdDt.Rows(0).Item("Trade_Discount_Perc").ToString) - Val(prn_HdDt.Rows(0).Item("Cash_Discount_Perc").ToString)

    '        rndoff = 0
    '        rndoff = Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) - Val(TtAmt)

    '        CurY = CurY + TxtHgt
    '        p1Font = New Font("Calibri", 11, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 10, CurY, 0, 0, p1Font)
    '        If Val(rndoff) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, "Round Off", Lf1, CurY, 0, 0, pFont)
    '            If Val(rndoff) >= 0 Then
    '                Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 25, CurY, 0, 0, pFont)
    '            Else
    '                Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 25, CurY, 0, 0, pFont)
    '            End If
    '            Common_Procedures.Print_To_PrintDocument(e, " " & Format(Math.Abs(Val(rndoff)), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
    '        End If

    '        CurY = CurY + TxtHgt + 5
    '        p1Font = New Font("Calibri", 11, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY, 0, 0, p1Font)
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)
    '        LnAr(8) = CurY

    '        p1Font = New Font("Calibri", 11, FontStyle.Bold)
    '        CurY = CurY + TxtHgt - 10
    '        Common_Procedures.Print_To_PrintDocument(e, "Net Amount", Lf1, CurY, 0, 0, p1Font)
    '        Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, p1Font)
    '        If Val(prn_HdDt.Rows(0).Item("Gr_Time").ToString) <> 0 Then
    '            p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '            Common_Procedures.Print_To_PrintDocument(e, "Due Date : " & Trim(prn_HdDt.Rows(0).Item("Gr_Time").ToString) & " Days " & "(" & Trim(prn_HdDt.Rows(0).Item("Gr_Date").ToString) & ")", LMargin + 10, CurY, 0, 0, p1Font)
    '        End If

    '        CurY = CurY + TxtHgt + 10
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(9) = CurY
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))

    '        CurY = CurY + 10

    '        BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
    '        BmsInWrds = Replace(Trim(BmsInWrds), "", "")

    '        Common_Procedures.Print_To_PrintDocument(e, "Rupees  : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)
    '        CurY = CurY + TxtHgt + 10
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

    '        CurY = CurY + 10
    '        p1Font = New Font("Calibri", 12, FontStyle.Regular)
    '        Common_Procedures.Print_To_PrintDocument(e, "GOODS CLEARED UNDER EXEMPTION NOTIFICATION NO 30/2004 DT 09.07.2004 ", LMargin, CurY, 2, PageWidth, pFont)

    '        CurY = CurY + TxtHgt + 2
    '        p1Font = New Font("Calibri", 12, FontStyle.Underline)
    '        Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)


    '        CurY = CurY + TxtHgt + 5
    '        If Val(prn_HdDt.Rows(0).Item("Gr_Time").ToString) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, "Overdue Interest will be charged at 24% from The  " & Trim(prn_HdDt.Rows(0).Item("Gr_Date").ToString), LMargin + 10, CurY, 0, 0, pFont)
    '        Else
    '            Common_Procedures.Print_To_PrintDocument(e, "Overdue Interest will be charged at 24% from The invoice date ", LMargin + 10, CurY, 0, 0, pFont)
    '        End If
    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, "We are not responsible for any loss or damage in transit", LMargin + 10, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, "We will not accept any claim after processing of goods", LMargin + 10, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, "Subject to Tirupur jurisdiction ", LMargin + 10, CurY, 0, 0, pFont)


    '        CurY = CurY + TxtHgt + 10
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(10) = CurY


    '        If Val(Common_Procedures.User.IdNo) <> 1 Then
    '            Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
    '        End If

    '        CurY = CurY + 10
    '        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
    '        p1Font = New Font("Calibri", 12, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
    '        CurY = CurY + TxtHgt
    '        CurY = CurY + TxtHgt
    '        CurY = CurY + TxtHgt

    '        Common_Procedures.Print_To_PrintDocument(e, "Prepared By", LMargin + 20, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 250, CurY, 0, 0, pFont)
    '        p1Font = New Font("Calibri", 12, FontStyle.Bold)

    '        Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
    '        CurY = CurY + TxtHgt + 10

    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
    '        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    '    Catch ex As Exception

    '        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try

    'End Sub

    'Private Sub Printing_Format3(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
    '    Dim LnAr(15) As Single, ClAr(15) As Single
    '    Dim ItmNm1 As String, ItmNm2 As String
    '    Dim ps As Printing.PaperSize
    '    Dim strHeight As Single = 0
    '    Dim PpSzSTS As Boolean = False
    '    Dim W1 As Single = 0
    '    Dim SNo As Integer = 0
    '    Dim flperc As Single = 0
    '    Dim flmtr As Single = 0
    '    Dim fmtr As Single = 0
    '    Dim VechDesc1 As String = "", VechDesc2 As String = ""

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
    '        .Left = 25 ' 30
    '        .Right = 40
    '        .Top = 40 ' 50
    '        .Bottom = 40
    '        LMargin = .Left
    '        RMargin = .Right
    '        TMargin = .Top
    '        BMargin = .Bottom
    '    End With

    '    pFont = New Font("Calibri", 10, FontStyle.Regular)

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

    '    NoofItems_PerPage = 5 ' 6

    '    Erase LnAr
    '    Erase ClAr

    '    LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    '    ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

    '    ClAr(1) = Val(50) : ClAr(2) = 240 : ClAr(3) = 80 : ClAr(4) = 70 : ClAr(5) = 100 : ClAr(6) = 80
    '    ClAr(7) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6))

    '    TxtHgt = 18.75 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20


    '    ''=========================================================================================================
    '    ''------  START OF PREPRINT POINTS
    '    ''=========================================================================================================

    '    'pFont = New Font("Calibri", 11, FontStyle.Regular)

    '    'Dim CurX As Single = 0
    '    'Dim pFont1 As Font

    '    'pFont1 = New Font("Calibri", 8, FontStyle.Regular)

    '    'For I = 100 To 1100 Step 300

    '    '    CurY = I
    '    '    For J = 1 To 850 Step 40

    '    '        CurX = J
    '    '        Common_Procedures.Print_To_PrintDocument(e, CurX, CurX, CurY - 20, 0, 0, pFont1)
    '    '        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY, 0, 0, pFont)

    '    '        CurX = J + 20
    '    '        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY, 0, 0, pFont)
    '    '        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY + 20, 0, 0, pFont)
    '    '        Common_Procedures.Print_To_PrintDocument(e, CurX, CurX, CurY + 40, 0, 0, pFont1)

    '    '    Next

    '    'Next

    '    'For I = 200 To 800 Step 250

    '    '    CurX = I
    '    '    For J = 1 To 1200 Step 40

    '    '        CurY = J
    '    '        Common_Procedures.Print_To_PrintDocument(e, "-", CurX, CurY, 0, 0, pFont)
    '    '        Common_Procedures.Print_To_PrintDocument(e, "   " & CurY, CurX, CurY, 0, 0, pFont1)

    '    '        CurY = J + 20
    '    '        Common_Procedures.Print_To_PrintDocument(e, "--", CurX, CurY, 0, 0, pFont)
    '    '        Common_Procedures.Print_To_PrintDocument(e, "   " & CurY, CurX, CurY, 0, 0, pFont1)

    '    '    Next

    '    'Next

    '    'e.HasMorePages = False

    '    'Exit Sub

    '    ''=========================================================================================================
    '    ''------  END OF PREPRINT POINTS
    '    ''=========================================================================================================

    '    EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

    '    Try
    '        If prn_HdDt.Rows.Count > 0 Then

    '            Printing_Format3_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

    '            NoofDets = 0

    '            CurY = CurY - 10

    '            If prn_DetDt.Rows.Count > 0 Then

    '                Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

    '                    If NoofDets >= NoofItems_PerPage Then

    '                        CurY = CurY + TxtHgt

    '                        Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)

    '                        NoofDets = NoofDets + 1

    '                        Printing_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

    '                        e.HasMorePages = True
    '                        Return

    '                    End If

    '                    If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString) <> "" Then
    '                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString)
    '                    Else
    '                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Name").ToString)
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

    '                    If Val(prn_DetDt.Rows(prn_DetIndx).Item("Fold_Perc").ToString) = 0 Or Val(prn_DetDt.Rows(prn_DetIndx).Item("Fold_Perc").ToString) = 100 Or Trim(prn_HdDt.Rows(0).Item("FoldingRate_Status").ToString) = 1 Then
    '                        CurY = CurY + TxtHgt + 10
    '                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
    '                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
    '                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bales").ToString) <> 0 Then
    '                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bales").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
    '                        End If
    '                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs").ToString) <> 0 Then
    '                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Pcs").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
    '                        End If
    '                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
    '                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
    '                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

    '                    Else

    '                        CurY = CurY + TxtHgt + 10
    '                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
    '                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
    '                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bales").ToString) <> 0 Then
    '                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bales").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
    '                        End If
    '                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs").ToString) <> 0 Then
    '                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Pcs").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
    '                        End If
    '                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)

    '                        'fmt = ((100 - Val(.Rows(CurRow).Cells(3).Value)) / 100) * Val(.Rows(CurRow).Cells(7).Value)
    '                        'fmt = Format(Math.Abs(Val(fmt)), "######0.00")
    '                        'fmt = Common_Procedures.Meter_RoundOff(fmt)
    '                        If Trim(ItmNm2) <> "" Then
    '                            CurY = CurY + TxtHgt - 5
    '                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
    '                            NoofDets = NoofDets + 1
    '                        End If

    '                        flperc = 100 - Val(prn_DetDt.Rows(prn_DetIndx).Item("Fold_Perc").ToString)

    '                        flmtr = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString) * flperc / 100, "#########0.00")

    '                        flmtr = Math.Abs(Val(flmtr))

    '                        flmtr = Common_Procedures.Meter_RoundOff(flmtr)

    '                        CurY = CurY + TxtHgt
    '                        If Val(flperc) > 0 Then
    '                            Common_Procedures.Print_To_PrintDocument(e, Val(flperc) & "%  Folding Less", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
    '                        Else
    '                            Common_Procedures.Print_To_PrintDocument(e, Val(flperc) & "%  Folding Add", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
    '                        End If
    '                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(flmtr), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)

    '                        CurY = CurY + TxtHgt
    '                        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY)

    '                        If Val(flperc) > 0 Then
    '                            fmtr = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString) - Val(flmtr), "#########0.00")
    '                        Else
    '                            fmtr = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString) + Val(flmtr), "#########0.00")
    '                        End If

    '                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(fmtr), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
    '                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
    '                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

    '                    End If

    '                    NoofDets = NoofDets + 1

    '                    prn_DetIndx = prn_DetIndx + 1

    '                Loop

    '                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1009" Then
    '                    CurY = CurY + TxtHgt
    '                    CurY = CurY + TxtHgt - 5
    '                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
    '                    NoofDets = NoofDets + 2
    '                End If

    '                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then

    '                    VechDesc1 = Trim(prn_HdDt.Rows(0).Item("Vechile_No").ToString)
    '                    VechDesc2 = ""

    '                    CurY = CurY + 5

    '                    Do

    '                        VechDesc2 = ""
    '                        If Len(VechDesc1) > 45 Then
    '                            For I = 45 To 1 Step -1
    '                                If Mid$(Trim(VechDesc1), I, 1) = " " Or Mid$(Trim(VechDesc1), I, 1) = "," Or Mid$(Trim(VechDesc1), I, 1) = "." Or Mid$(Trim(VechDesc1), I, 1) = "-" Or Mid$(Trim(VechDesc1), I, 1) = "/" Or Mid$(Trim(VechDesc1), I, 1) = "_" Or Mid$(Trim(VechDesc1), I, 1) = "(" Or Mid$(Trim(VechDesc1), I, 1) = ")" Or Mid$(Trim(VechDesc1), I, 1) = "\" Or Mid$(Trim(VechDesc1), I, 1) = "[" Or Mid$(Trim(VechDesc1), I, 1) = "]" Or Mid$(Trim(VechDesc1), I, 1) = "{" Or Mid$(Trim(VechDesc1), I, 1) = "}" Then Exit For
    '                            Next I
    '                            If I = 0 Then I = 45
    '                            VechDesc2 = Microsoft.VisualBasic.Right(Trim(VechDesc1), Len(VechDesc1) - I)
    '                            VechDesc1 = Microsoft.VisualBasic.Left(Trim(VechDesc1), I - 1)
    '                        End If

    '                        CurY = CurY + TxtHgt - 5

    '                        p1Font = New Font("Calibri", 7, FontStyle.Regular)
    '                        Common_Procedures.Print_To_PrintDocument(e, Trim(VechDesc1), LMargin + ClAr(1) + 10, CurY, 0, 0, p1Font)
    '                        NoofDets = NoofDets + 2

    '                        VechDesc1 = Trim(VechDesc2)
    '                        VechDesc2 = ""

    '                    Loop Until Trim(VechDesc1) = ""

    '                End If

    '            End If

    '            Printing_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

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

    'Private Sub Printing_Format3_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
    '    Dim da2 As New SqlClient.SqlDataAdapter
    '    Dim dt2 As New DataTable
    '    Dim p1Font As Font
    '    Dim strHeight As Single
    '    Dim C1 As Single, W1, W2, W3 As Single, S1, S2 As Single
    '    Dim Cmp_Name, Desc As String, Cmp_Add1 As String, Cmp_Add2 As String
    '    Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String, Cmp_PanNo As String
    '    Dim S As String

    '    PageNo = PageNo + 1

    '    CurY = TMargin

    '    'da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Ledger_Name, d.Company_Description as Transport_Name from Processed_Fabric_Sales_Invoice_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Transport_IdNo LEFT OUTER JOIN Company_Head d ON d.Company_IdNo = a.Company_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Processed_Fabric_Sales_Invoice_Code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
    '    'da2.Fill(dt2)
    '    'If dt2.Rows.Count > NoofItems_PerPage Then
    '    '    Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
    '    'End If
    '    'dt2.Clear()

    '    prn_Count = prn_Count + 1

    '    prn_OriDupTri = ""
    '    If Trim(prn_InpOpts) <> "" Then
    '        If prn_Count <= Len(Trim(prn_InpOpts)) Then

    '            S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

    '            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
    '                If Val(S) = 1 Then
    '                    prn_OriDupTri = "ORIGINAL"
    '                ElseIf Val(S) = 2 Then
    '                    prn_OriDupTri = "TRANSPORT COPY"
    '                ElseIf Val(S) = 3 Then
    '                    prn_OriDupTri = "TRIPLICATE"
    '                ElseIf Val(S) = 4 Then
    '                    prn_OriDupTri = "EXTRA COPY"
    '                Else
    '                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
    '                        prn_OriDupTri = Trim(prn_InpOpts)
    '                    End If
    '                End If

    '            Else
    '                If Val(S) = 1 Then
    '                    prn_OriDupTri = "ORIGINAL FOR BUYER"
    '                ElseIf Val(S) = 2 Then
    '                    prn_OriDupTri = "DUPLICATE FOR TRANSPORT"
    '                ElseIf Val(S) = 3 Then
    '                    prn_OriDupTri = "TRIPLICATE FOR ASSESSE"
    '                ElseIf Val(S) = 4 Then
    '                    prn_OriDupTri = "EXTRA COPY"
    '                Else
    '                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
    '                        prn_OriDupTri = Trim(prn_InpOpts)
    '                    End If

    '                End If

    '            End If

    '        End If
    '    End If

    '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
    '        p1Font = New Font("Calibri", 15, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY - TxtHgt - 5, 2, PrintWidth, p1Font)
    '    End If
    '    If Trim(prn_OriDupTri) <> "" Then
    '        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
    '    End If

    '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '    LnAr(1) = CurY
    '    Desc = ""
    '    Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
    '    Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = "" : Cmp_EMail = ""

    '    Desc = prn_HdDt.Rows(0).Item("Company_Description").ToString
    '    Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
    '    Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
    '    Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

    '    If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
    '        Cmp_PhNo = "PHONE : " & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
    '        Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
    '        Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
    '        Cmp_PanNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
    '        Cmp_EMail = "MAIL ID : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
    '    End If

    '    p1Font = New Font("Calibri", 15, FontStyle.Bold)
    '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1018" Then '---- M.K Textiles (Palladam)
    '        p1Font = New Font("Calibri", 15, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
    '    End If
    '    CurY = CurY + TxtHgt
    '    strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


    '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
    '        e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MK, Drawing.Image), LMargin + 20, CurY, 112, 80)
    '        'e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MK_2, Drawing.Image), LMargin + 20, CurY, 115, 80)
    '        'e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MK, Drawing.Image), LMargin + 20, CurY, 75, 75)
    '    End If

    '    p1Font = New Font("Calibri", 18, FontStyle.Bold)
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
    '    strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


    '    CurY = CurY + strHeight - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Desc, LMargin, CurY, 2, PrintWidth, pFont)

    '    CurY = CurY + TxtHgt - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

    '    CurY = CurY + TxtHgt - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
    '    CurY = CurY + TxtHgt - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
    '    CurY = CurY + TxtHgt - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, LMargin, CurY, 2, PrintWidth, pFont)
    '    CurY = CurY + TxtHgt - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, LMargin, CurY, 2, PrintWidth, pFont)
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

    '    CurY = CurY + TxtHgt + 10
    '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '    LnAr(2) = CurY

    '    Try
    '        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
    '        W1 = e.Graphics.MeasureString("INVOICE DATE  : ", pFont).Width
    '        S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width
    '        W2 = e.Graphics.MeasureString("Despatch To   : ", pFont).Width
    '        S2 = e.Graphics.MeasureString("Sent Through  : ", pFont).Width


    '        CurY = CurY + 10
    '        p1Font = New Font("Calibri", 12, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
    '        Common_Procedures.Print_To_PrintDocument(e, "INVOICE NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
    '        If prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString & "-" & prn_HdDt.Rows(0).Item("Processed_Fabric_Sales_Invoice_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
    '        Else
    '            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Processed_Fabric_Sales_Invoice_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
    '        End If

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '        p1Font = New Font("Calibri", 14, FontStyle.Bold)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "INVOICE DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Processed_Fabric_Sales_Invoice_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)


    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)


    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '        If Trim(prn_HdDt.Rows(0).Item("Dc_No").ToString) <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, "DC NO : " & prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
    '        End If
    '        If Trim(prn_HdDt.Rows(0).Item("Dc_Date").ToString) <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, "DC DATE : " & prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + C1 + 100, CurY, 0, 0, pFont)
    '        End If

    '        CurY = CurY + TxtHgt
    '        If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, " TIN : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '        End If

    '        CurY = CurY + TxtHgt
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
    '        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(2))
    '        LnAr(3) = CurY
    '        CurY = CurY + 10
    '        Common_Procedures.Print_To_PrintDocument(e, "Agent Name ", LMargin + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)


    '        Common_Procedures.Print_To_PrintDocument(e, "Transport ", LMargin + C1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + C1 + W2 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + C1 + W2 + 30, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, "Order No ", LMargin + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_OrderNo").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)
    '        If Trim(prn_HdDt.Rows(0).Item("Party_OrderNo").ToString) <> "" And Trim(prn_HdDt.Rows(0).Item("Party_OrderDate").ToString) <> "" Then
    '            W3 = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Party_OrderNo").ToString, pFont).Width
    '            Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Party_OrderDate").ToString, LMargin + S2 + W3 + 40, CurY, 0, 0, pFont)
    '        End If

    '        Common_Procedures.Print_To_PrintDocument(e, "Lr.No  ", LMargin + C1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + C1 + W2 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lr_No").ToString, LMargin + C1 + W2 + 30, CurY, 0, 0, pFont)
    '        If Trim(prn_HdDt.Rows(0).Item("Lr_No").ToString) <> "" And Trim(prn_HdDt.Rows(0).Item("Lr_Date").ToString) <> "" Then
    '            W3 = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Lr_No").ToString, pFont).Width
    '            Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Lr_Date").ToString, LMargin + C1 + W2 + W3 + 40, CurY, 0, 0, pFont)
    '        End If


    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, "Lc No ", LMargin + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lc_No").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)
    '        If Trim(prn_HdDt.Rows(0).Item("Lc_No").ToString) <> "" And Trim(prn_HdDt.Rows(0).Item("Lc_Date").ToString) <> "" Then
    '            W3 = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Lc_No").ToString, pFont).Width
    '            Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Lc_Date").ToString, LMargin + S2 + W3 + 35, CurY, 0, 0, pFont)
    '        End If

    '        'Common_Procedures.Print_To_PrintDocument(e, "Transport ", LMargin + 10, CurY, 0, 0, pFont)
    '        'Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
    '        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "Despatch To", LMargin + C1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W2 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Despatch_To").ToString, LMargin + C1 + W2 + 30, CurY, 0, 0, pFont)


    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, "Sent Through ", LMargin + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Through_Name").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Delivery_Address1").ToString, LMargin + C1 + W2 + 30, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Delivery_Address2").ToString, LMargin + C1 + W2 + 30, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(4) = CurY

    '        CurY = CurY + 10
    '        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "BALES\", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "BUNDLES", LMargin + ClAr(1) + ClAr(2), CurY + TxtHgt, 2, ClAr(3), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "NO.OF", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY + TxtHgt, 2, ClAr(4), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "METER", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + TxtHgt, 2, ClAr(5), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "RATE\", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + TxtHgt, 2, ClAr(6), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)

    '        CurY = CurY + TxtHgt + 20
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(5) = CurY

    '        p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '        CurY = CurY + 10
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cloth_Details").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, p1Font)
    '        CurY = CurY + 2

    '    Catch ex As Exception

    '        MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try

    'End Sub

    'Private Sub Printing_Format3_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
    '    Dim p1Font As Font
    '    Dim rndoff As Single, TtAmt As Double
    '    Dim I As Integer
    '    Dim BInc As Integer
    '    Dim BnkDetAr() As String
    '    Dim Cmp_Name As String, Cmp_EMail As String = ""
    '    Dim W1 As Single = 0
    '    Dim BmsInWrds As String
    '    Dim vprn_BlNos As String = ""
    '    Dim BLNo1 As String, BLNo2 As String
    '    Dim BankNm1 As String = ""
    '    Dim BankNm2 As String = ""
    '    Dim BankNm3 As String = ""
    '    Dim BankNm4 As String = ""
    '    Dim BankNm5 As String = ""
    '    Dim BankNm6 As String = ""

    '    Try

    '        For I = NoofDets + 1 To NoofItems_PerPage

    '            CurY = CurY + TxtHgt

    '            prn_DetIndx = prn_DetIndx + 1

    '        Next

    '        CurY = CurY + TxtHgt + 50
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(6) = CurY

    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
    '        CurY += 10

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

    '            BInc = BInc + 1
    '            If UBound(BnkDetAr) >= BInc Then
    '                BankNm6 = Trim(BnkDetAr(BInc))
    '            End If

    '        End If


    '        vprn_BlNos = ""
    '        For I = 0 To prn_DetDt.Rows.Count - 1
    '            If Trim(prn_DetDt.Rows(I).Item("Bales_Nos").ToString) <> "" Then
    '                vprn_BlNos = Trim(vprn_BlNos) & IIf(Trim(vprn_BlNos) <> "", ", ", "") & prn_DetDt.Rows(I).Item("Bales_Nos").ToString
    '            End If
    '        Next


    '        BLNo1 = Trim(vprn_BlNos)
    '        BLNo2 = ""
    '        If Len(BLNo1) > 30 Then
    '            For I = 30 To 1 Step -1
    '                If Mid$(Trim(BLNo1), I, 1) = " " Or Mid$(Trim(BLNo1), I, 1) = "," Or Mid$(Trim(BLNo1), I, 1) = "." Or Mid$(Trim(BLNo1), I, 1) = "-" Or Mid$(Trim(BLNo1), I, 1) = "/" Or Mid$(Trim(BLNo1), I, 1) = "_" Or Mid$(Trim(BLNo1), I, 1) = "(" Or Mid$(Trim(BLNo1), I, 1) = ")" Or Mid$(Trim(BLNo1), I, 1) = "\" Or Mid$(Trim(BLNo1), I, 1) = "[" Or Mid$(Trim(BLNo1), I, 1) = "]" Or Mid$(Trim(BLNo1), I, 1) = "{" Or Mid$(Trim(BLNo1), I, 1) = "}" Then Exit For
    '            Next I
    '            If I = 0 Then I = 30
    '            BLNo2 = Microsoft.VisualBasic.Right(Trim(BLNo1), Len(BLNo1) - I)
    '            BLNo1 = Microsoft.VisualBasic.Left(Trim(BLNo1), I - 1)
    '        End If

    '        If Trim(BLNo1) <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, "Bale/Bundle No : " & BLNo1, LMargin + 10, CurY, 0, 0, pFont)
    '        End If
    '        If Val(prn_HdDt.Rows(0).Item("Trade_Discount_Perc").ToString) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("TradeDisc_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Trade_Discount").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 20, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Trade_Discount_Perc").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
    '        End If

    '        CurY = CurY + TxtHgt
    '        If Trim(BLNo2) <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, BLNo2, LMargin + 10, CurY, 0, 0, pFont)
    '        End If

    '        If Val(prn_HdDt.Rows(0).Item("Cash_Discount_Perc").ToString) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("CashDisc_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Cash_Discount").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 20, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Cash_Discount_Perc").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
    '        End If

    '        CurY = CurY + TxtHgt
    '        p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY, 0, 0, p1Font)
    '        'If Val(prn_HdDt.Rows(0).Item("Bale_Weight").ToString) <> 0 Then
    '        '    Common_Procedures.Print_To_PrintDocument(e, "Bale/Bundle Weight : " & Trim(prn_HdDt.Rows(0).Item("Bale_Weight").ToString), LMargin + 10, CurY, 0, 0, pFont)
    '        'End If
    '        If Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Packing_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 20, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Packing_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
    '        End If

    '        CurY = CurY + TxtHgt
    '        p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY - 5, 0, 0, p1Font)
    '        If Val(prn_HdDt.Rows(0).Item("Freight").ToString) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Freight_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 20, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Freight").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
    '        End If

    '        CurY = CurY + TxtHgt
    '        p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 10, CurY, 0, 0, p1Font)
    '        If Val(prn_HdDt.Rows(0).Item("Insurance").ToString) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Insurance_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 20, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Insurance").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
    '        End If

    '        TtAmt = Val(prn_HdDt.Rows(0).Item("total_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Freight").ToString) + Val(prn_HdDt.Rows(0).Item("Insurance").ToString) + Val(prn_HdDt.Rows(0).Item("Packing_amount").ToString) - Val(prn_HdDt.Rows(0).Item("Trade_Discount_Perc").ToString) - Val(prn_HdDt.Rows(0).Item("Cash_Discount_Perc").ToString)

    '        rndoff = 0
    '        rndoff = Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) - Val(TtAmt)

    '        CurY = CurY + TxtHgt
    '        p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY - 5, 0, 0, p1Font)
    '        If Val(rndoff) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 0, 0, pFont)
    '            If Val(rndoff) >= 0 Then
    '                Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 20, CurY, 0, 0, pFont)
    '            Else
    '                Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 20, CurY, 0, 0, pFont)
    '            End If
    '            Common_Procedures.Print_To_PrintDocument(e, " " & Format(Math.Abs(Val(rndoff)), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
    '        End If

    '        CurY = CurY + TxtHgt
    '        p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, BankNm5, LMargin + 10, CurY, 0, 0, p1Font)
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)
    '        LnAr(8) = CurY

    '        CurY = CurY + TxtHgt ' 10
    '        p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, BankNm6, LMargin + 10, CurY - 5, 0, 0, p1Font)
    '        p1Font = New Font("Calibri", 11, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 0, 0, p1Font)
    '        Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Net_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
    '        If Val(prn_HdDt.Rows(0).Item("Gr_Time").ToString) <> 0 Then
    '            p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '            Common_Procedures.Print_To_PrintDocument(e, "Due Date : " & Trim(prn_HdDt.Rows(0).Item("Gr_Time").ToString) & " Days " & "(" & Trim(prn_HdDt.Rows(0).Item("Gr_Date").ToString) & ")", LMargin + 10, CurY, 0, 0, p1Font)
    '        End If

    '        CurY = CurY + TxtHgt
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(9) = CurY
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))

    '        CurY = CurY + 10

    '        BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
    '        'BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "", "")

    '        Common_Procedures.Print_To_PrintDocument(e, "Rupees  : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)
    '        CurY = CurY + TxtHgt + 10
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

    '        CurY = CurY + 10
    '        p1Font = New Font("Calibri", 12, FontStyle.Regular)
    '        Common_Procedures.Print_To_PrintDocument(e, "GOODS CLEARED UNDER EXEMPTION NOTIFICATION NO 30/2004 DT 09.07.2004 ", LMargin, CurY, 2, PageWidth, pFont)

    '        CurY = CurY + TxtHgt
    '        p1Font = New Font("Calibri", 12, FontStyle.Underline)
    '        Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)


    '        CurY = CurY + TxtHgt
    '        If Val(prn_HdDt.Rows(0).Item("Gr_Time").ToString) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, "Overdue Interest will be charged at 24% from The  " & Trim(prn_HdDt.Rows(0).Item("Gr_Date").ToString), LMargin + 10, CurY, 0, 0, pFont)
    '        Else
    '            Common_Procedures.Print_To_PrintDocument(e, "Overdue Interest will be charged at 24% from The invoice date ", LMargin + 10, CurY, 0, 0, pFont)
    '        End If
    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, "We are not responsible for any loss or damage in transit", LMargin + 10, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, "We will not accept any claim after processing of goods", LMargin + 10, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, "Subject to Tirupur jurisdiction ", LMargin + 10, CurY, 0, 0, pFont)


    '        CurY = CurY + TxtHgt + 10
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(10) = CurY


    '        If Val(Common_Procedures.User.IdNo) <> 1 Then
    '            Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
    '        End If

    '        CurY = CurY + 10
    '        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
    '        p1Font = New Font("Calibri", 12, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
    '        CurY = CurY + TxtHgt
    '        CurY = CurY + TxtHgt
    '        CurY = CurY + TxtHgt

    '        Common_Procedures.Print_To_PrintDocument(e, "Prepared By", LMargin + 20, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 250, CurY, 0, 0, pFont)
    '        p1Font = New Font("Calibri", 12, FontStyle.Bold)

    '        Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
    '        CurY = CurY + TxtHgt + 10

    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
    '        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    '        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Then '---- Asia Textiles (Tirupur)
    '            CurY = CurY + TxtHgt - 15
    '            p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '            Common_Procedures.Print_To_PrintDocument(e, "Please send payment details of this bill to asiatextilestirupur@yahoo.in", LMargin + 10, CurY, 0, 0, p1Font)
    '        Else

    '            Cmp_EMail = ""
    '            If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
    '                Cmp_EMail = prn_HdDt.Rows(0).Item("Company_EMail").ToString
    '            End If
    '            If Trim(Cmp_EMail) <> "" Then
    '                CurY = CurY + TxtHgt - 15
    '                p1Font = New Font("Calibri", 11, FontStyle.Bold)
    '                Common_Procedures.Print_To_PrintDocument(e, "Please send payment details of this bill to " & Trim(LCase(Cmp_EMail)), LMargin + 10, CurY, 0, 0, p1Font)
    '            End If
    '        End If

    '    Catch ex As Exception

    '        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try

    'End Sub

    'Private Sub Printing_Format4(ByRef e As System.Drawing.Printing.PrintPageEventArgs)  ' 10 x 12 
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
    '    Dim LnAr(15) As Single, ClAr(15) As Single
    '    Dim ItmNm1 As String, ItmNm2 As String
    '    'Dim ps As Printing.PaperSize
    '    Dim strHeight As Single = 0
    '    Dim PpSzSTS As Boolean = False
    '    Dim W1 As Single = 0
    '    Dim SNo As Integer = 0
    '    Dim flperc As Single = 0
    '    Dim flmtr As Single = 0
    '    Dim fmtr As Single = 0
    '    Dim VechDesc1 As String = "", VechDesc2 As String = ""

    '    Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 9X12", 900, 1200)
    '    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
    '    PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

    '    With PrintDocument1.DefaultPageSettings.Margins
    '        .Left = 20
    '        .Right = 30 ' 65
    '        .Top = 50 ' 60
    '        .Bottom = 40
    '        LMargin = .Left
    '        RMargin = .Right
    '        TMargin = .Top
    '        BMargin = .Bottom
    '    End With

    '    pFont = New Font("Arial", 11, FontStyle.Regular)
    '    'pFont = New Font("Calibri", 10, FontStyle.Regular)

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

    '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Then '---- Asia Textiles (Tirupur)
    '        NoofItems_PerPage = 4 ' 6
    '    Else
    '        NoofItems_PerPage = 5 ' 6
    '    End If

    '    Erase LnAr
    '    Erase ClAr

    '    LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    '    ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

    '    ClAr(1) = 45 : ClAr(2) = 300 : ClAr(3) = 85 : ClAr(4) = 75 : ClAr(5) = 110 : ClAr(6) = 90
    '    ClAr(7) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6))

    '    'ClAr(1) = Val(50) : ClAr(2) = 240 : ClAr(3) = 80 : ClAr(4) = 70 : ClAr(5) = 100 : ClAr(6) = 80
    '    'ClAr(7) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6))

    '    TxtHgt = 18.75 ' 19  ' e.Graphics.MeasureString("A", pFont).Height  ' 20

    '    ''=========================================================================================================
    '    ''------  START OF PREPRINT POINTS
    '    ''=========================================================================================================

    '    'pFont = New Font("Calibri", 11, FontStyle.Regular)

    '    'Dim CurX As Single = 0
    '    'Dim pFont1 As Font

    '    'pFont1 = New Font("Calibri", 8, FontStyle.Regular)

    '    'For I = 100 To 1100 Step 300

    '    '    CurY = I
    '    '    For J = 1 To 850 Step 40

    '    '        CurX = J
    '    '        Common_Procedures.Print_To_PrintDocument(e, CurX, CurX, CurY - 20, 0, 0, pFont1)
    '    '        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY, 0, 0, pFont)

    '    '        CurX = J + 20
    '    '        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY, 0, 0, pFont)
    '    '        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY + 20, 0, 0, pFont)
    '    '        Common_Procedures.Print_To_PrintDocument(e, CurX, CurX, CurY + 40, 0, 0, pFont1)

    '    '    Next

    '    'Next

    '    'For I = 200 To 800 Step 250

    '    '    CurX = I
    '    '    For J = 1 To 1200 Step 40

    '    '        CurY = J
    '    '        Common_Procedures.Print_To_PrintDocument(e, "-", CurX, CurY, 0, 0, pFont)
    '    '        Common_Procedures.Print_To_PrintDocument(e, "   " & CurY, CurX, CurY, 0, 0, pFont1)

    '    '        CurY = J + 20
    '    '        Common_Procedures.Print_To_PrintDocument(e, "--", CurX, CurY, 0, 0, pFont)
    '    '        Common_Procedures.Print_To_PrintDocument(e, "   " & CurY, CurX, CurY, 0, 0, pFont1)

    '    '    Next

    '    'Next

    '    'e.HasMorePages = False

    '    'Exit Sub

    '    ''=========================================================================================================
    '    ''------  END OF PREPRINT POINTS ---------
    '    ''=========================================================================================================

    '    EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

    '    Try

    '        If prn_HdDt.Rows.Count > 0 Then

    '            Printing_Format4_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

    '            NoofDets = 0

    '            CurY = CurY - 10

    '            If prn_DetDt.Rows.Count > 0 Then

    '                Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

    '                    If NoofDets >= NoofItems_PerPage Then

    '                        CurY = CurY + TxtHgt

    '                        Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)

    '                        NoofDets = NoofDets + 1

    '                        Printing_Format4_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

    '                        e.HasMorePages = True
    '                        Return

    '                    End If

    '                    If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString) <> "" Then
    '                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString)
    '                    Else
    '                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Name").ToString)
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

    '                    If Val(prn_DetDt.Rows(prn_DetIndx).Item("Fold_Perc").ToString) = 0 Or Val(prn_DetDt.Rows(prn_DetIndx).Item("Fold_Perc").ToString) = 100 Or Trim(prn_HdDt.Rows(0).Item("FoldingRate_Status").ToString) = 1 Then
    '                        CurY = CurY + TxtHgt + 10
    '                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
    '                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
    '                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bales").ToString) <> 0 Then
    '                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bales").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
    '                        End If
    '                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs").ToString) <> 0 Then
    '                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Pcs").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
    '                        End If
    '                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
    '                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
    '                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

    '                    Else

    '                        CurY = CurY + TxtHgt + 10
    '                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
    '                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
    '                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bales").ToString) <> 0 Then
    '                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bales").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
    '                        End If
    '                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs").ToString) <> 0 Then
    '                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Pcs").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
    '                        End If
    '                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)

    '                        'fmt = ((100 - Val(.Rows(CurRow).Cells(3).Value)) / 100) * Val(.Rows(CurRow).Cells(7).Value)
    '                        'fmt = Format(Math.Abs(Val(fmt)), "######0.00")
    '                        'fmt = Common_Procedures.Meter_RoundOff(fmt)
    '                        If Trim(ItmNm2) <> "" Then
    '                            CurY = CurY + TxtHgt - 5
    '                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
    '                            NoofDets = NoofDets + 1
    '                        End If

    '                        flperc = 100 - Val(prn_DetDt.Rows(prn_DetIndx).Item("Fold_Perc").ToString)

    '                        flmtr = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString) * flperc / 100, "#########0.00")

    '                        flmtr = Math.Abs(Val(flmtr))

    '                        flmtr = Common_Procedures.Meter_RoundOff(flmtr)

    '                        CurY = CurY + TxtHgt
    '                        If Val(flperc) > 0 Then
    '                            Common_Procedures.Print_To_PrintDocument(e, Val(flperc) & "%  Folding Less", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
    '                        Else
    '                            Common_Procedures.Print_To_PrintDocument(e, Val(flperc) & "%  Folding Add", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
    '                        End If
    '                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(flmtr), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)

    '                        CurY = CurY + TxtHgt + 2
    '                        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY)

    '                        If Val(flperc) > 0 Then
    '                            fmtr = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString) - Val(flmtr), "#########0.00")
    '                        Else
    '                            fmtr = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString) + Val(flmtr), "#########0.00")
    '                        End If

    '                        CurY = CurY + 5
    '                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(fmtr), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
    '                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
    '                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

    '                    End If

    '                    NoofDets = NoofDets + 1

    '                    prn_DetIndx = prn_DetIndx + 1

    '                Loop

    '                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1009" Then
    '                    CurY = CurY + TxtHgt
    '                    CurY = CurY + TxtHgt - 5
    '                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
    '                    NoofDets = NoofDets + 2
    '                End If

    '                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then

    '                    VechDesc1 = Trim(prn_HdDt.Rows(0).Item("Vechile_No").ToString)
    '                    VechDesc2 = ""

    '                    CurY = CurY + 5

    '                    Do

    '                        VechDesc2 = ""
    '                        If Len(VechDesc1) > 45 Then
    '                            For I = 45 To 1 Step -1
    '                                If Mid$(Trim(VechDesc1), I, 1) = " " Or Mid$(Trim(VechDesc1), I, 1) = "," Or Mid$(Trim(VechDesc1), I, 1) = "." Or Mid$(Trim(VechDesc1), I, 1) = "-" Or Mid$(Trim(VechDesc1), I, 1) = "/" Or Mid$(Trim(VechDesc1), I, 1) = "_" Or Mid$(Trim(VechDesc1), I, 1) = "(" Or Mid$(Trim(VechDesc1), I, 1) = ")" Or Mid$(Trim(VechDesc1), I, 1) = "\" Or Mid$(Trim(VechDesc1), I, 1) = "[" Or Mid$(Trim(VechDesc1), I, 1) = "]" Or Mid$(Trim(VechDesc1), I, 1) = "{" Or Mid$(Trim(VechDesc1), I, 1) = "}" Then Exit For
    '                            Next I
    '                            If I = 0 Then I = 45
    '                            VechDesc2 = Microsoft.VisualBasic.Right(Trim(VechDesc1), Len(VechDesc1) - I)
    '                            VechDesc1 = Microsoft.VisualBasic.Left(Trim(VechDesc1), I - 1)
    '                        End If

    '                        CurY = CurY + TxtHgt - 5

    '                        p1Font = New Font("Calibri", 7, FontStyle.Regular)
    '                        Common_Procedures.Print_To_PrintDocument(e, Trim(VechDesc1), LMargin + ClAr(1) + 10, CurY, 0, 0, p1Font)
    '                        NoofDets = NoofDets + 2

    '                        VechDesc1 = Trim(VechDesc2)
    '                        VechDesc2 = ""

    '                    Loop Until Trim(VechDesc1) = ""

    '                End If

    '            End If

    '            Printing_Format4_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

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

    'Private Sub Printing_Format4_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
    '    Dim da2 As New SqlClient.SqlDataAdapter
    '    Dim dt2 As New DataTable
    '    Dim p1Font As Font
    '    Dim strHeight As Single
    '    Dim C1 As Single, W1, W2, W3 As Single, S1, S2 As Single
    '    Dim Cmp_Name, Desc As String, Cmp_Add1 As String, Cmp_Add2 As String
    '    Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String, Cmp_PanNo As String
    '    Dim S As String

    '    PageNo = PageNo + 1

    '    CurY = TMargin

    '    'da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Ledger_Name, d.Company_Description as Transport_Name from Processed_Fabric_Sales_Invoice_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Transport_IdNo LEFT OUTER JOIN Company_Head d ON d.Company_IdNo = a.Company_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Processed_Fabric_Sales_Invoice_Code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
    '    'da2.Fill(dt2)
    '    'If dt2.Rows.Count > NoofItems_PerPage Then
    '    '    Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
    '    'End If
    '    'dt2.Clear()

    '    prn_Count = prn_Count + 1

    '    prn_OriDupTri = ""
    '    If Trim(prn_InpOpts) <> "" Then
    '        If prn_Count <= Len(Trim(prn_InpOpts)) Then

    '            S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

    '            If Val(S) = 1 Then
    '                prn_OriDupTri = "ORIGINAL FOR BUYER"
    '            ElseIf Val(S) = 2 Then
    '                prn_OriDupTri = "DUPLICATE FOR TRANSPORT"
    '            ElseIf Val(S) = 3 Then
    '                prn_OriDupTri = "TRIPLICATE FOR ASSESSE"
    '            ElseIf Val(S) = 4 Then
    '                prn_OriDupTri = "EXTRA COPY"
    '            Else
    '                If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
    '                    prn_OriDupTri = Trim(prn_InpOpts)
    '                End If
    '            End If

    '        End If
    '    End If

    '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
    '        p1Font = New Font("Calibri", 15, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY - TxtHgt - 5, 2, PrintWidth, p1Font)
    '    End If
    '    If Trim(prn_OriDupTri) <> "" Then
    '        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
    '    End If

    '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '    LnAr(1) = CurY
    '    Desc = ""
    '    Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
    '    Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = "" : Cmp_EMail = ""

    '    Desc = prn_HdDt.Rows(0).Item("Company_Description").ToString
    '    Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
    '    Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
    '    Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

    '    If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
    '        Cmp_PhNo = "PHONE : " & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
    '        Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
    '        Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
    '        Cmp_PanNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
    '        Cmp_EMail = "MAIL ID : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
    '    End If

    '    p1Font = New Font("Calibri", 15, FontStyle.Bold)
    '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1018" Then '---- M.K Textiles (Palladam)
    '        p1Font = New Font("Calibri", 15, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
    '    End If
    '    CurY = CurY + TxtHgt
    '    strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


    '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
    '        e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MK, Drawing.Image), LMargin + 20, CurY, 112, 80)
    '        'e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MK_2, Drawing.Image), LMargin + 20, CurY, 115, 80)
    '        'e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MK, Drawing.Image), LMargin + 20, CurY, 75, 75)
    '    End If

    '    p1Font = New Font("Calibri", 18, FontStyle.Bold)
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
    '    strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


    '    CurY = CurY + strHeight - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Desc, LMargin, CurY, 2, PrintWidth, pFont)

    '    CurY = CurY + TxtHgt - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

    '    CurY = CurY + TxtHgt - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
    '    CurY = CurY + TxtHgt - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
    '    CurY = CurY + TxtHgt - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, LMargin, CurY, 2, PrintWidth, pFont)
    '    CurY = CurY + TxtHgt - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, LMargin, CurY, 2, PrintWidth, pFont)
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

    '    'Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, LMargin, CurY, 2, PrintWidth, pFont)

    '    CurY = CurY + TxtHgt + 10
    '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '    LnAr(2) = CurY

    '    Try

    '        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 50

    '        W1 = e.Graphics.MeasureString("INVOICE DATE  : ", pFont).Width
    '        S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width
    '        W2 = e.Graphics.MeasureString("Despatch To   : ", pFont).Width
    '        S2 = e.Graphics.MeasureString("Sent Through  : ", pFont).Width


    '        CurY = CurY + 10
    '        p1Font = New Font("Calibri", 12, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
    '        Common_Procedures.Print_To_PrintDocument(e, "INVOICE NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
    '        If prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString & "-" & prn_HdDt.Rows(0).Item("Processed_Fabric_Sales_Invoice_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
    '        Else
    '            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Processed_Fabric_Sales_Invoice_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
    '        End If


    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '        p1Font = New Font("Calibri", 14, FontStyle.Bold)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "INVOICE DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Processed_Fabric_Sales_Invoice_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)


    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '        If Trim(prn_HdDt.Rows(0).Item("Dc_No").ToString) <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, "DC NO : " & prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
    '        End If
    '        If Trim(prn_HdDt.Rows(0).Item("Dc_Date").ToString) <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, "DC DATE : " & prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + C1 + 100, CurY, 0, 0, pFont)
    '        End If

    '        CurY = CurY + TxtHgt
    '        If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, " TIN : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '        End If

    '        CurY = CurY + TxtHgt
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
    '        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(2))

    '        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 100


    '        LnAr(3) = CurY
    '        CurY = CurY + 10
    '        Common_Procedures.Print_To_PrintDocument(e, "Agent Name ", LMargin + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)

    '        Common_Procedures.Print_To_PrintDocument(e, "Transport ", LMargin + C1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + C1 + W2 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + C1 + W2 + 30, CurY, 0, 0, pFont)


    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, "Order No ", LMargin + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_OrderNo").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)
    '        If Trim(prn_HdDt.Rows(0).Item("Party_OrderNo").ToString) <> "" And Trim(prn_HdDt.Rows(0).Item("Party_OrderDate").ToString) <> "" Then
    '            W3 = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Party_OrderNo").ToString, pFont).Width
    '            Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Party_OrderDate").ToString, LMargin + S2 + W3 + 40, CurY, 0, 0, pFont)
    '        End If

    '        Common_Procedures.Print_To_PrintDocument(e, "Lr.No  ", LMargin + C1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + C1 + W2 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lr_No").ToString, LMargin + C1 + W2 + 30, CurY, 0, 0, pFont)
    '        If Trim(prn_HdDt.Rows(0).Item("Lr_No").ToString) <> "" And Trim(prn_HdDt.Rows(0).Item("Lr_Date").ToString) <> "" Then
    '            W3 = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Lr_No").ToString, pFont).Width
    '            Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Lr_Date").ToString, LMargin + C1 + W2 + W3 + 40, CurY, 0, 0, pFont)
    '        End If


    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, "Lc No ", LMargin + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lc_No").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)
    '        If Trim(prn_HdDt.Rows(0).Item("Lc_No").ToString) <> "" And Trim(prn_HdDt.Rows(0).Item("Lc_Date").ToString) <> "" Then
    '            W3 = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Lc_No").ToString, pFont).Width
    '            Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Lc_Date").ToString, LMargin + S2 + W3 + 35, CurY, 0, 0, pFont)
    '        End If

    '        'Common_Procedures.Print_To_PrintDocument(e, "Transport ", LMargin + 10, CurY, 0, 0, pFont)
    '        'Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
    '        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "Despatch To", LMargin + C1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W2 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Despatch_To").ToString, LMargin + C1 + W2 + 30, CurY, 0, 0, pFont)


    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, "Sent Through ", LMargin + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Through_Name").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Delivery_Address1").ToString, LMargin + C1 + W2 + 30, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Delivery_Address2").ToString, LMargin + C1 + W2 + 30, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(4) = CurY

    '        CurY = CurY + 10
    '        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "BALES\", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "BUNDLES", LMargin + ClAr(1) + ClAr(2), CurY + TxtHgt, 2, ClAr(3), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "NO.OF", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY + TxtHgt, 2, ClAr(4), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "METER", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + TxtHgt, 2, ClAr(5), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "RATE\", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + TxtHgt, 2, ClAr(6), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)

    '        CurY = CurY + TxtHgt + 20
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(5) = CurY

    '        p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '        CurY = CurY + 10
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cloth_Details").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, p1Font)
    '        CurY = CurY + 2

    '    Catch ex As Exception

    '        MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try

    'End Sub

    'Private Sub Printing_Format4_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
    '    Dim p1Font As Font
    '    Dim rndoff As Single, TtAmt As Double
    '    Dim I As Integer
    '    Dim BInc As Integer
    '    Dim BnkDetAr() As String
    '    Dim Cmp_Name As String = "", Cmp_EMail As String = ""
    '    Dim W1 As Single = 0
    '    Dim BmsInWrds As String
    '    Dim vprn_BlNos As String = ""
    '    Dim BLNo1 As String, BLNo2 As String
    '    Dim BankNm1 As String = ""
    '    Dim BankNm2 As String = ""
    '    Dim BankNm3 As String = ""
    '    Dim BankNm4 As String = ""
    '    Dim BankNm5 As String = ""
    '    Dim BankNm6 As String = ""

    '    Try

    '        For I = NoofDets + 1 To NoofItems_PerPage

    '            CurY = CurY + TxtHgt

    '            prn_DetIndx = prn_DetIndx + 1

    '        Next

    '        CurY = CurY + TxtHgt + 50
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(6) = CurY

    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
    '        CurY += 10

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

    '            BInc = BInc + 1
    '            If UBound(BnkDetAr) >= BInc Then
    '                BankNm6 = Trim(BnkDetAr(BInc))
    '            End If

    '        End If


    '        vprn_BlNos = ""
    '        For I = 0 To prn_DetDt.Rows.Count - 1
    '            If Trim(prn_DetDt.Rows(I).Item("Bales_Nos").ToString) <> "" Then
    '                vprn_BlNos = Trim(vprn_BlNos) & IIf(Trim(vprn_BlNos) <> "", ", ", "") & prn_DetDt.Rows(I).Item("Bales_Nos").ToString
    '            End If
    '        Next


    '        BLNo1 = Trim(vprn_BlNos)
    '        BLNo2 = ""
    '        If Len(BLNo1) > 30 Then
    '            For I = 30 To 1 Step -1
    '                If Mid$(Trim(BLNo1), I, 1) = " " Or Mid$(Trim(BLNo1), I, 1) = "," Or Mid$(Trim(BLNo1), I, 1) = "." Or Mid$(Trim(BLNo1), I, 1) = "-" Or Mid$(Trim(BLNo1), I, 1) = "/" Or Mid$(Trim(BLNo1), I, 1) = "_" Or Mid$(Trim(BLNo1), I, 1) = "(" Or Mid$(Trim(BLNo1), I, 1) = ")" Or Mid$(Trim(BLNo1), I, 1) = "\" Or Mid$(Trim(BLNo1), I, 1) = "[" Or Mid$(Trim(BLNo1), I, 1) = "]" Or Mid$(Trim(BLNo1), I, 1) = "{" Or Mid$(Trim(BLNo1), I, 1) = "}" Then Exit For
    '            Next I
    '            If I = 0 Then I = 30
    '            BLNo2 = Microsoft.VisualBasic.Right(Trim(BLNo1), Len(BLNo1) - I)
    '            BLNo1 = Microsoft.VisualBasic.Left(Trim(BLNo1), I - 1)
    '        End If

    '        If Trim(BLNo1) <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, "Bale/Bundle No : " & BLNo1, LMargin + 10, CurY, 0, 0, pFont)
    '        End If
    '        If Val(prn_HdDt.Rows(0).Item("Trade_Discount_Perc").ToString) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("TradeDisc_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Trade_Discount").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 30, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Trade_Discount_Perc").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
    '        End If

    '        CurY = CurY + TxtHgt
    '        If Trim(BLNo2) <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, BLNo2, LMargin + 10, CurY, 0, 0, pFont)
    '        End If

    '        If Val(prn_HdDt.Rows(0).Item("Cash_Discount_Perc").ToString) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("CashDisc_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Cash_Discount").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 30, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Cash_Discount_Perc").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
    '        End If

    '        CurY = CurY + TxtHgt
    '        p1Font = New Font("Calibri", 11, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY, 0, 0, p1Font)
    '        'If Val(prn_HdDt.Rows(0).Item("Bale_Weight").ToString) <> 0 Then
    '        '    Common_Procedures.Print_To_PrintDocument(e, "Bale/Bundle Weight : " & Trim(prn_HdDt.Rows(0).Item("Bale_Weight").ToString), LMargin + 10, CurY, 0, 0, pFont)
    '        'End If
    '        If Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Packing_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 30, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Packing_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
    '        End If

    '        CurY = CurY + TxtHgt
    '        p1Font = New Font("Calibri", 11, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY - 5, 0, 0, p1Font)
    '        If Val(prn_HdDt.Rows(0).Item("Freight").ToString) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Freight_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 30, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Freight").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
    '        End If

    '        CurY = CurY + TxtHgt
    '        p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 10, CurY, 0, 0, p1Font)
    '        If Val(prn_HdDt.Rows(0).Item("Insurance").ToString) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Insurance_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 30, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Insurance").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
    '        End If

    '        TtAmt = Val(prn_HdDt.Rows(0).Item("total_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Freight").ToString) + Val(prn_HdDt.Rows(0).Item("Insurance").ToString) + Val(prn_HdDt.Rows(0).Item("Packing_amount").ToString) - Val(prn_HdDt.Rows(0).Item("Trade_Discount_Perc").ToString) - Val(prn_HdDt.Rows(0).Item("Cash_Discount_Perc").ToString)

    '        rndoff = 0
    '        rndoff = Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) - Val(TtAmt)

    '        CurY = CurY + TxtHgt
    '        p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY - 5, 0, 0, p1Font)
    '        If Val(rndoff) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 0, 0, pFont)
    '            If Val(rndoff) >= 0 Then
    '                Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 30, CurY, 0, 0, pFont)
    '            Else
    '                Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 30, CurY, 0, 0, pFont)
    '            End If
    '            Common_Procedures.Print_To_PrintDocument(e, " " & Format(Math.Abs(Val(rndoff)), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
    '        End If

    '        CurY = CurY + TxtHgt
    '        p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, BankNm5, LMargin + 10, CurY, 0, 0, p1Font)
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)
    '        LnAr(8) = CurY

    '        CurY = CurY + TxtHgt ' 10
    '        p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, BankNm6, LMargin + 10, CurY - 5, 0, 0, p1Font)
    '        p1Font = New Font("Calibri", 11, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 0, 0, p1Font)
    '        Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Net_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
    '        If Val(prn_HdDt.Rows(0).Item("Gr_Time").ToString) <> 0 Then
    '            p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '            Common_Procedures.Print_To_PrintDocument(e, "Due Date : " & Trim(prn_HdDt.Rows(0).Item("Gr_Time").ToString) & " Days " & "(" & Trim(prn_HdDt.Rows(0).Item("Gr_Date").ToString) & ")", LMargin + 10, CurY, 0, 0, p1Font)
    '        End If

    '        CurY = CurY + TxtHgt
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(9) = CurY
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))

    '        CurY = CurY + 10

    '        BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
    '        'BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "", "")

    '        Common_Procedures.Print_To_PrintDocument(e, "Rupees  : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)
    '        CurY = CurY + TxtHgt + 10
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

    '        CurY = CurY + 10
    '        p1Font = New Font("Calibri", 12, FontStyle.Regular)
    '        Common_Procedures.Print_To_PrintDocument(e, "GOODS CLEARED UNDER EXEMPTION NOTIFICATION NO 30/2004 DT 09.07.2004 ", LMargin, CurY, 2, PageWidth, pFont)

    '        CurY = CurY + TxtHgt
    '        p1Font = New Font("Calibri", 12, FontStyle.Underline)
    '        Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)


    '        CurY = CurY + TxtHgt
    '        If Val(prn_HdDt.Rows(0).Item("Gr_Time").ToString) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, "Overdue Interest will be charged at 24% from The  " & Trim(prn_HdDt.Rows(0).Item("Gr_Date").ToString), LMargin + 10, CurY, 0, 0, pFont)
    '        Else
    '            Common_Procedures.Print_To_PrintDocument(e, "Overdue Interest will be charged at 24% from The invoice date ", LMargin + 10, CurY, 0, 0, pFont)
    '        End If
    '        CurY = CurY + TxtHgt - 1
    '        Common_Procedures.Print_To_PrintDocument(e, "We are not responsible for any loss or damage in transit", LMargin + 10, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt - 1
    '        Common_Procedures.Print_To_PrintDocument(e, "We will not accept any claim after processing of goods", LMargin + 10, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt - 1
    '        Common_Procedures.Print_To_PrintDocument(e, "Subject to Tirupur jurisdiction ", LMargin + 10, CurY, 0, 0, pFont)


    '        CurY = CurY + TxtHgt + 10
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(10) = CurY


    '        If Val(Common_Procedures.User.IdNo) <> 1 Then
    '            Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
    '        End If

    '        CurY = CurY + 10
    '        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
    '        p1Font = New Font("Calibri", 12, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
    '        CurY = CurY + TxtHgt - 1
    '        CurY = CurY + TxtHgt - 1
    '        CurY = CurY + TxtHgt - 1

    '        Common_Procedures.Print_To_PrintDocument(e, "Prepared By", LMargin + 20, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 250, CurY, 0, 0, pFont)
    '        p1Font = New Font("Calibri", 12, FontStyle.Bold)

    '        Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
    '        CurY = CurY + TxtHgt + 10

    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
    '        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    '        If Trim(Cmp_EMail) <> "" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Then '---- Asia Textiles (Tirupur)
    '            CurY = CurY + TxtHgt - 15
    '            p1Font = New Font("Calibri", 11, FontStyle.Bold)
    '            Common_Procedures.Print_To_PrintDocument(e, "Please send payment details of this bill to asiatextilestirupur@yahoo.in", LMargin + 10, CurY, 0, 0, p1Font)

    '        Else

    '            Cmp_EMail = ""
    '            If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
    '                Cmp_EMail = prn_HdDt.Rows(0).Item("Company_EMail").ToString
    '            End If
    '            If Trim(Cmp_EMail) <> "" Then
    '                CurY = CurY + TxtHgt - 15
    '                p1Font = New Font("Calibri", 11, FontStyle.Bold)
    '                Common_Procedures.Print_To_PrintDocument(e, "Please send payment details of this bill to " & Trim(LCase(Cmp_EMail)), LMargin + 10, CurY, 0, 0, p1Font)
    '            End If

    '        End If

    '    Catch ex As Exception

    '        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try

    'End Sub

    'Private Sub Printing_Format5(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
    '    Dim LnAr(15) As Single, ClAr(15) As Single
    '    Dim ItmNm1 As String, ItmNm2 As String
    '    'Dim ps As Printing.PaperSize
    '    Dim strHeight As Single = 0
    '    Dim PpSzSTS As Boolean = False
    '    Dim W1 As Single = 0
    '    Dim SNo As Integer = 0
    '    Dim flperc As Single = 0
    '    Dim flmtr As Single = 0
    '    Dim fmtr As Single = 0
    '    Dim VechDesc1 As String = "", VechDesc2 As String = ""

    '    Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 14.5X8", 1450, 800)
    '    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
    '    PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

    '    With PrintDocument1.DefaultPageSettings.Margins
    '        .Left = 20
    '        .Right = 30 ' 65
    '        .Top = 50 ' 60
    '        .Bottom = 40
    '        LMargin = .Left
    '        RMargin = .Right
    '        TMargin = .Top
    '        BMargin = .Bottom
    '    End With

    '    pFont = New Font("Arial", 11, FontStyle.Regular)
    '    'pFont = New Font("Calibri", 10, FontStyle.Regular)

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

    '    NoofItems_PerPage = 5 ' 6

    '    Erase LnAr
    '    Erase ClAr

    '    LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    '    ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

    '    ClAr(1) = 45 : ClAr(2) = 300 : ClAr(3) = 85 : ClAr(4) = 75 : ClAr(5) = 110 : ClAr(6) = 90
    '    ClAr(7) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6))

    '    'ClAr(1) = Val(50) : ClAr(2) = 240 : ClAr(3) = 80 : ClAr(4) = 70 : ClAr(5) = 100 : ClAr(6) = 80
    '    'ClAr(7) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6))

    '    TxtHgt = 19  ' e.Graphics.MeasureString("A", pFont).Height  ' 20

    '    ''=========================================================================================================
    '    ''------  START OF PREPRINT POINTS
    '    ''=========================================================================================================

    '    'pFont = New Font("Calibri", 11, FontStyle.Regular)

    '    'Dim CurX As Single = 0
    '    'Dim pFont1 As Font

    '    'pFont1 = New Font("Calibri", 8, FontStyle.Regular)

    '    'For I = 100 To 1100 Step 300

    '    '    CurY = I
    '    '    For J = 1 To 1450 Step 40

    '    '        CurX = J
    '    '        Common_Procedures.Print_To_PrintDocument(e, CurX, CurX, CurY - 20, 0, 0, pFont1)
    '    '        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY, 0, 0, pFont)

    '    '        CurX = J + 20
    '    '        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY, 0, 0, pFont)
    '    '        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY + 20, 0, 0, pFont)
    '    '        Common_Procedures.Print_To_PrintDocument(e, CurX, CurX, CurY + 40, 0, 0, pFont1)

    '    '    Next

    '    'Next

    '    'For I = 200 To 800 Step 250

    '    '    CurX = I
    '    '    For J = 1 To 1200 Step 40

    '    '        CurY = J
    '    '        Common_Procedures.Print_To_PrintDocument(e, "-", CurX, CurY, 0, 0, pFont)
    '    '        Common_Procedures.Print_To_PrintDocument(e, "   " & CurY, CurX, CurY, 0, 0, pFont1)

    '    '        CurY = J + 20
    '    '        Common_Procedures.Print_To_PrintDocument(e, "--", CurX, CurY, 0, 0, pFont)
    '    '        Common_Procedures.Print_To_PrintDocument(e, "   " & CurY, CurX, CurY, 0, 0, pFont1)

    '    '    Next

    '    'Next

    '    'e.HasMorePages = False

    '    'Exit Sub

    '    ''=========================================================================================================
    '    ''------  END OF PREPRINT POINTS ---------
    '    ''=========================================================================================================

    '    EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

    '    Try

    '        If prn_HdDt.Rows.Count > 0 Then

    '            Printing_Format5_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

    '            NoofDets = 0

    '            CurY = CurY - 10

    '            If prn_DetDt.Rows.Count > 0 Then

    '                Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

    '                    If NoofDets >= NoofItems_PerPage Then

    '                        CurY = CurY + TxtHgt

    '                        Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)

    '                        NoofDets = NoofDets + 1

    '                        Printing_Format5_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

    '                        e.HasMorePages = True
    '                        Return

    '                    End If

    '                    If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString) <> "" Then
    '                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString)
    '                    Else
    '                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Name").ToString)
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

    '                    If Val(prn_DetDt.Rows(prn_DetIndx).Item("Fold_Perc").ToString) = 0 Or Val(prn_DetDt.Rows(prn_DetIndx).Item("Fold_Perc").ToString) = 100 Or Trim(prn_HdDt.Rows(0).Item("FoldingRate_Status").ToString) = 1 Then
    '                        CurY = CurY + TxtHgt + 10
    '                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
    '                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
    '                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bales").ToString) <> 0 Then
    '                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bales").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
    '                        End If
    '                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs").ToString) <> 0 Then
    '                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Pcs").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
    '                        End If
    '                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
    '                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
    '                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

    '                    Else

    '                        CurY = CurY + TxtHgt + 10
    '                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
    '                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
    '                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bales").ToString) <> 0 Then
    '                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bales").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
    '                        End If
    '                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs").ToString) <> 0 Then
    '                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Pcs").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
    '                        End If
    '                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)

    '                        'fmt = ((100 - Val(.Rows(CurRow).Cells(3).Value)) / 100) * Val(.Rows(CurRow).Cells(7).Value)
    '                        'fmt = Format(Math.Abs(Val(fmt)), "######0.00")
    '                        'fmt = Common_Procedures.Meter_RoundOff(fmt)
    '                        If Trim(ItmNm2) <> "" Then
    '                            CurY = CurY + TxtHgt - 5
    '                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
    '                            NoofDets = NoofDets + 1
    '                        End If

    '                        flperc = 100 - Val(prn_DetDt.Rows(prn_DetIndx).Item("Fold_Perc").ToString)

    '                        flmtr = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString) * flperc / 100, "#########0.00")

    '                        flmtr = Math.Abs(Val(flmtr))

    '                        flmtr = Common_Procedures.Meter_RoundOff(flmtr)

    '                        CurY = CurY + TxtHgt
    '                        If Val(flperc) > 0 Then
    '                            Common_Procedures.Print_To_PrintDocument(e, Val(flperc) & "%  Folding Less", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
    '                        Else
    '                            Common_Procedures.Print_To_PrintDocument(e, Val(flperc) & "%  Folding Add", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
    '                        End If
    '                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(flmtr), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)

    '                        CurY = CurY + TxtHgt + 2
    '                        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY)

    '                        If Val(flperc) > 0 Then
    '                            fmtr = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString) - Val(flmtr), "#########0.00")
    '                        Else
    '                            fmtr = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString) + Val(flmtr), "#########0.00")
    '                        End If

    '                        CurY = CurY + 5
    '                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(fmtr), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
    '                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
    '                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

    '                    End If

    '                    NoofDets = NoofDets + 1

    '                    prn_DetIndx = prn_DetIndx + 1

    '                Loop

    '                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1009" Then
    '                    CurY = CurY + TxtHgt
    '                    CurY = CurY + TxtHgt - 5
    '                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
    '                    NoofDets = NoofDets + 2
    '                End If

    '                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then

    '                    VechDesc1 = Trim(prn_HdDt.Rows(0).Item("Vechile_No").ToString)
    '                    VechDesc2 = ""

    '                    CurY = CurY + 5

    '                    Do

    '                        VechDesc2 = ""
    '                        If Len(VechDesc1) > 45 Then
    '                            For I = 45 To 1 Step -1
    '                                If Mid$(Trim(VechDesc1), I, 1) = " " Or Mid$(Trim(VechDesc1), I, 1) = "," Or Mid$(Trim(VechDesc1), I, 1) = "." Or Mid$(Trim(VechDesc1), I, 1) = "-" Or Mid$(Trim(VechDesc1), I, 1) = "/" Or Mid$(Trim(VechDesc1), I, 1) = "_" Or Mid$(Trim(VechDesc1), I, 1) = "(" Or Mid$(Trim(VechDesc1), I, 1) = ")" Or Mid$(Trim(VechDesc1), I, 1) = "\" Or Mid$(Trim(VechDesc1), I, 1) = "[" Or Mid$(Trim(VechDesc1), I, 1) = "]" Or Mid$(Trim(VechDesc1), I, 1) = "{" Or Mid$(Trim(VechDesc1), I, 1) = "}" Then Exit For
    '                            Next I
    '                            If I = 0 Then I = 45
    '                            VechDesc2 = Microsoft.VisualBasic.Right(Trim(VechDesc1), Len(VechDesc1) - I)
    '                            VechDesc1 = Microsoft.VisualBasic.Left(Trim(VechDesc1), I - 1)
    '                        End If

    '                        CurY = CurY + TxtHgt - 5

    '                        p1Font = New Font("Calibri", 7, FontStyle.Regular)
    '                        Common_Procedures.Print_To_PrintDocument(e, Trim(VechDesc1), LMargin + ClAr(1) + 10, CurY, 0, 0, p1Font)
    '                        NoofDets = NoofDets + 2

    '                        VechDesc1 = Trim(VechDesc2)
    '                        VechDesc2 = ""

    '                    Loop Until Trim(VechDesc1) = ""

    '                End If

    '            End If

    '            Printing_Format5_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

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

    'Private Sub Printing_Format5_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
    '    Dim da2 As New SqlClient.SqlDataAdapter
    '    Dim dt2 As New DataTable
    '    Dim p1Font As Font
    '    Dim strHeight As Single
    '    Dim C1 As Single, W1, W2, W3 As Single, S1, S2 As Single
    '    Dim Cmp_Name, Desc As String, Cmp_Add1 As String, Cmp_Add2 As String
    '    Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
    '    Dim S As String

    '    PageNo = PageNo + 1

    '    CurY = TMargin

    '    'da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Ledger_Name, d.Company_Description as Transport_Name from Processed_Fabric_Sales_Invoice_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Transport_IdNo LEFT OUTER JOIN Company_Head d ON d.Company_IdNo = a.Company_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Processed_Fabric_Sales_Invoice_Code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
    '    'da2.Fill(dt2)
    '    'If dt2.Rows.Count > NoofItems_PerPage Then
    '    '    Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
    '    'End If
    '    'dt2.Clear()

    '    prn_Count = prn_Count + 1

    '    prn_OriDupTri = ""
    '    If Trim(prn_InpOpts) <> "" Then
    '        If prn_Count <= Len(Trim(prn_InpOpts)) Then

    '            S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

    '            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
    '                If Val(S) = 1 Then
    '                    prn_OriDupTri = "ORIGINAL"
    '                ElseIf Val(S) = 2 Then
    '                    prn_OriDupTri = "TRANSPORT COPY"
    '                ElseIf Val(S) = 3 Then
    '                    prn_OriDupTri = "TRIPLICATE"
    '                ElseIf Val(S) = 4 Then
    '                    prn_OriDupTri = "EXTRA COPY"
    '                Else
    '                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
    '                        prn_OriDupTri = Trim(prn_InpOpts)
    '                    End If
    '                End If

    '            Else
    '                If Val(S) = 1 Then
    '                    prn_OriDupTri = "ORIGINAL"
    '                ElseIf Val(S) = 2 Then
    '                    prn_OriDupTri = "DUPLICATE"
    '                ElseIf Val(S) = 3 Then
    '                    prn_OriDupTri = "TRIPLICATE"
    '                ElseIf Val(S) = 4 Then
    '                    prn_OriDupTri = "EXTRA COPY"
    '                Else
    '                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
    '                        prn_OriDupTri = Trim(prn_InpOpts)
    '                    End If
    '                End If

    '            End If

    '        End If
    '    End If

    '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
    '        p1Font = New Font("Calibri", 15, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY - TxtHgt - 5, 2, PrintWidth, p1Font)
    '    End If
    '    If Trim(prn_OriDupTri) <> "" Then
    '        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
    '    End If

    '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '    LnAr(1) = CurY
    '    Desc = ""
    '    Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
    '    Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_EMail = ""

    '    Desc = prn_HdDt.Rows(0).Item("Company_Description").ToString
    '    Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
    '    Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
    '    Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

    '    If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
    '        Cmp_PhNo = "PHONE : " & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
    '        Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
    '        Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
    '    End If
    '    If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
    '        Cmp_EMail = "MAIL ID : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
    '    End If

    '    p1Font = New Font("Calibri", 15, FontStyle.Bold)
    '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1018" Then '---- M.K Textiles (Palladam)
    '        p1Font = New Font("Calibri", 15, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
    '    End If
    '    CurY = CurY + TxtHgt
    '    strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


    '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
    '        e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MK, Drawing.Image), LMargin + 20, CurY, 112, 80)
    '        'e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MK_2, Drawing.Image), LMargin + 20, CurY, 115, 80)
    '        'e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MK, Drawing.Image), LMargin + 20, CurY, 75, 75)
    '    End If

    '    p1Font = New Font("Calibri", 18, FontStyle.Bold)
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
    '    strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


    '    CurY = CurY + strHeight - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Desc, LMargin, CurY, 2, PrintWidth, pFont)

    '    CurY = CurY + TxtHgt - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

    '    CurY = CurY + TxtHgt - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
    '    CurY = CurY + TxtHgt - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
    '    CurY = CurY + TxtHgt - 1
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, LMargin, CurY, 2, PrintWidth, pFont)
    '    Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)


    '    CurY = CurY + TxtHgt + 10
    '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '    LnAr(2) = CurY

    '    Try
    '        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 50
    '        W1 = e.Graphics.MeasureString("INVOICE DATE  : ", pFont).Width
    '        S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width
    '        W2 = e.Graphics.MeasureString("Despatch To   : ", pFont).Width
    '        S2 = e.Graphics.MeasureString("Sent Through  : ", pFont).Width


    '        CurY = CurY + 10
    '        p1Font = New Font("Calibri", 12, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
    '        Common_Procedures.Print_To_PrintDocument(e, "INVOICE NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
    '        If prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString & "-" & prn_HdDt.Rows(0).Item("Processed_Fabric_Sales_Invoice_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
    '        Else
    '            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Processed_Fabric_Sales_Invoice_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
    '        End If


    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '        p1Font = New Font("Calibri", 14, FontStyle.Bold)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "INVOICE DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Processed_Fabric_Sales_Invoice_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)


    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '        If Trim(prn_HdDt.Rows(0).Item("Dc_No").ToString) <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, "DC NO : " & prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
    '        End If
    '        If Trim(prn_HdDt.Rows(0).Item("Dc_Date").ToString) <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, "DC DATE : " & prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + C1 + 100, CurY, 0, 0, pFont)
    '        End If

    '        CurY = CurY + TxtHgt
    '        If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, " TIN : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
    '        End If

    '        CurY = CurY + TxtHgt
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
    '        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(2))
    '        LnAr(3) = CurY
    '        CurY = CurY + 10
    '        Common_Procedures.Print_To_PrintDocument(e, "Agent Name ", LMargin + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)


    '        Common_Procedures.Print_To_PrintDocument(e, "Transport ", LMargin + C1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + C1 + W2 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + C1 + W2 + 30, CurY, 0, 0, pFont)



    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, "Order No ", LMargin + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_OrderNo").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)

    '        Common_Procedures.Print_To_PrintDocument(e, "Lr.No  ", LMargin + C1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + C1 + W2 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lr_No").ToString, LMargin + C1 + W2 + 30, CurY, 0, 0, pFont)
    '        If Trim(prn_HdDt.Rows(0).Item("Lr_No").ToString) <> "" And Trim(prn_HdDt.Rows(0).Item("Lr_Date").ToString) <> "" Then
    '            W3 = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Lr_No").ToString, pFont).Width
    '            Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Lr_Date").ToString, LMargin + C1 + W2 + W3 + 40, CurY, 0, 0, pFont)
    '        End If


    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, "Lc No ", LMargin + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lc_No").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)
    '        If Trim(prn_HdDt.Rows(0).Item("Lc_No").ToString) <> "" And Trim(prn_HdDt.Rows(0).Item("Lc_Date").ToString) <> "" Then
    '            W3 = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Lc_No").ToString, pFont).Width
    '            Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Lc_Date").ToString, LMargin + S2 + W3 + 35, CurY, 0, 0, pFont)
    '        End If

    '        'Common_Procedures.Print_To_PrintDocument(e, "Transport ", LMargin + 10, CurY, 0, 0, pFont)
    '        'Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
    '        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "Despatch To", LMargin + C1 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W2 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Despatch_To").ToString, LMargin + C1 + W2 + 30, CurY, 0, 0, pFont)


    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, "Sent Through ", LMargin + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Through_Name").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Delivery_Address1").ToString, LMargin + C1 + W2 + 30, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Delivery_Address2").ToString, LMargin + C1 + W2 + 30, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(4) = CurY

    '        CurY = CurY + 10
    '        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "BALES\", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "BUNDLES", LMargin + ClAr(1) + ClAr(2), CurY + TxtHgt, 2, ClAr(3), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "NO.OF", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY + TxtHgt, 2, ClAr(4), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "METER", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + TxtHgt, 2, ClAr(5), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "RATE\", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + TxtHgt, 2, ClAr(6), pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)

    '        CurY = CurY + TxtHgt + 20
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(5) = CurY

    '        CurY = CurY + 10
    '        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cloth_Details").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)

    '    Catch ex As Exception

    '        MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try

    'End Sub

    'Private Sub Printing_Format5_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
    '    Dim p1Font As Font
    '    Dim rndoff As Single, TtAmt As Double
    '    Dim I As Integer
    '    Dim BInc As Integer
    '    Dim BnkDetAr() As String
    '    Dim Cmp_Name As String = "", Cmp_EMail As String = ""
    '    Dim W1 As Single = 0
    '    Dim BmsInWrds As String
    '    Dim vprn_BlNos As String = ""
    '    Dim BLNo1 As String, BLNo2 As String
    '    Dim BankNm1 As String = ""
    '    Dim BankNm2 As String = ""
    '    Dim BankNm3 As String = ""
    '    Dim BankNm4 As String = ""
    '    Dim BankNm5 As String = ""
    '    Dim BankNm6 As String = ""

    '    Try

    '        For I = NoofDets + 1 To NoofItems_PerPage

    '            CurY = CurY + TxtHgt

    '            prn_DetIndx = prn_DetIndx + 1

    '        Next

    '        CurY = CurY + TxtHgt + 50
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(6) = CurY

    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
    '        CurY += 10

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

    '            BInc = BInc + 1
    '            If UBound(BnkDetAr) >= BInc Then
    '                BankNm6 = Trim(BnkDetAr(BInc))
    '            End If

    '        End If


    '        vprn_BlNos = ""
    '        For I = 0 To prn_DetDt.Rows.Count - 1
    '            If Trim(prn_DetDt.Rows(I).Item("Bales_Nos").ToString) <> "" Then
    '                vprn_BlNos = Trim(vprn_BlNos) & IIf(Trim(vprn_BlNos) <> "", ", ", "") & prn_DetDt.Rows(I).Item("Bales_Nos").ToString
    '            End If
    '        Next


    '        BLNo1 = Trim(vprn_BlNos)
    '        BLNo2 = ""
    '        If Len(BLNo1) > 30 Then
    '            For I = 30 To 1 Step -1
    '                If Mid$(Trim(BLNo1), I, 1) = " " Or Mid$(Trim(BLNo1), I, 1) = "," Or Mid$(Trim(BLNo1), I, 1) = "." Or Mid$(Trim(BLNo1), I, 1) = "-" Or Mid$(Trim(BLNo1), I, 1) = "/" Or Mid$(Trim(BLNo1), I, 1) = "_" Or Mid$(Trim(BLNo1), I, 1) = "(" Or Mid$(Trim(BLNo1), I, 1) = ")" Or Mid$(Trim(BLNo1), I, 1) = "\" Or Mid$(Trim(BLNo1), I, 1) = "[" Or Mid$(Trim(BLNo1), I, 1) = "]" Or Mid$(Trim(BLNo1), I, 1) = "{" Or Mid$(Trim(BLNo1), I, 1) = "}" Then Exit For
    '            Next I
    '            If I = 0 Then I = 30
    '            BLNo2 = Microsoft.VisualBasic.Right(Trim(BLNo1), Len(BLNo1) - I)
    '            BLNo1 = Microsoft.VisualBasic.Left(Trim(BLNo1), I - 1)
    '        End If

    '        If Trim(BLNo1) <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, "Bale/Bundle No : " & BLNo1, LMargin + 10, CurY, 0, 0, pFont)
    '        End If
    '        If Val(prn_HdDt.Rows(0).Item("Trade_Discount_Perc").ToString) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("TradeDisc_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Trade_Discount").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 30, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Trade_Discount_Perc").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
    '        End If

    '        CurY = CurY + TxtHgt
    '        If Trim(BLNo2) <> "" Then
    '            Common_Procedures.Print_To_PrintDocument(e, BLNo2, LMargin + 10, CurY, 0, 0, pFont)
    '        End If

    '        If Val(prn_HdDt.Rows(0).Item("Cash_Discount_Perc").ToString) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("CashDisc_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Cash_Discount").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 30, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Cash_Discount_Perc").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
    '        End If

    '        CurY = CurY + TxtHgt
    '        p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY, 0, 0, p1Font)
    '        'If Val(prn_HdDt.Rows(0).Item("Bale_Weight").ToString) <> 0 Then
    '        '    Common_Procedures.Print_To_PrintDocument(e, "Bale/Bundle Weight : " & Trim(prn_HdDt.Rows(0).Item("Bale_Weight").ToString), LMargin + 10, CurY, 0, 0, pFont)
    '        'End If
    '        If Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Packing_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 30, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Packing_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
    '        End If

    '        CurY = CurY + TxtHgt
    '        p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY - 5, 0, 0, p1Font)
    '        If Val(prn_HdDt.Rows(0).Item("Freight").ToString) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Freight_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 30, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Freight").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
    '        End If

    '        CurY = CurY + TxtHgt
    '        p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 10, CurY, 0, 0, p1Font)
    '        If Val(prn_HdDt.Rows(0).Item("Insurance").ToString) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Insurance_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 30, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Insurance").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
    '        End If

    '        TtAmt = Val(prn_HdDt.Rows(0).Item("total_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Freight").ToString) + Val(prn_HdDt.Rows(0).Item("Insurance").ToString) + Val(prn_HdDt.Rows(0).Item("Packing_amount").ToString) - Val(prn_HdDt.Rows(0).Item("Trade_Discount_Perc").ToString) - Val(prn_HdDt.Rows(0).Item("Cash_Discount_Perc").ToString)

    '        rndoff = 0
    '        rndoff = Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) - Val(TtAmt)

    '        CurY = CurY + TxtHgt
    '        p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY - 5, 0, 0, p1Font)
    '        If Val(rndoff) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 0, 0, pFont)
    '            If Val(rndoff) >= 0 Then
    '                Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 30, CurY, 0, 0, pFont)
    '            Else
    '                Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 30, CurY, 0, 0, pFont)
    '            End If
    '            Common_Procedures.Print_To_PrintDocument(e, " " & Format(Math.Abs(Val(rndoff)), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
    '        End If

    '        CurY = CurY + TxtHgt
    '        p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, BankNm5, LMargin + 10, CurY, 0, 0, p1Font)
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)
    '        LnAr(8) = CurY

    '        CurY = CurY + TxtHgt ' 10
    '        p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, BankNm6, LMargin + 10, CurY - 5, 0, 0, p1Font)
    '        p1Font = New Font("Calibri", 11, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 0, 0, p1Font)
    '        Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Net_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
    '        If Val(prn_HdDt.Rows(0).Item("Gr_Time").ToString) <> 0 Then
    '            p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '            Common_Procedures.Print_To_PrintDocument(e, "Due Date : " & Trim(prn_HdDt.Rows(0).Item("Gr_Time").ToString) & " Days " & "(" & Trim(prn_HdDt.Rows(0).Item("Gr_Date").ToString) & ")", LMargin + 10, CurY, 0, 0, p1Font)
    '        End If

    '        CurY = CurY + TxtHgt
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(9) = CurY
    '        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))

    '        CurY = CurY + 10

    '        BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
    '        'BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "", "")

    '        Common_Procedures.Print_To_PrintDocument(e, "Rupees  : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)
    '        CurY = CurY + TxtHgt + 10
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

    '        CurY = CurY + 10
    '        p1Font = New Font("Calibri", 12, FontStyle.Regular)
    '        Common_Procedures.Print_To_PrintDocument(e, "GOODS CLEARED UNDER EXEMPTION NOTIFICATION NO 30/2004 DT 09.07.2004 ", LMargin, CurY, 2, PageWidth, pFont)

    '        CurY = CurY + TxtHgt
    '        p1Font = New Font("Calibri", 12, FontStyle.Underline)
    '        Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)


    '        CurY = CurY + TxtHgt
    '        If Val(prn_HdDt.Rows(0).Item("Gr_Time").ToString) <> 0 Then
    '            Common_Procedures.Print_To_PrintDocument(e, "Overdue Interest will be charged at 24% from The  " & Trim(prn_HdDt.Rows(0).Item("Gr_Date").ToString), LMargin + 10, CurY, 0, 0, pFont)
    '        Else
    '            Common_Procedures.Print_To_PrintDocument(e, "Overdue Interest will be charged at 24% from The invoice date ", LMargin + 10, CurY, 0, 0, pFont)
    '        End If
    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, "We are not responsible for any loss or damage in transit", LMargin + 10, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, "We will not accept any claim after processing of goods", LMargin + 10, CurY, 0, 0, pFont)

    '        CurY = CurY + TxtHgt
    '        Common_Procedures.Print_To_PrintDocument(e, "Subject to Tirupur jurisdiction ", LMargin + 10, CurY, 0, 0, pFont)


    '        CurY = CurY + TxtHgt + 10
    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        LnAr(10) = CurY


    '        If Val(Common_Procedures.User.IdNo) <> 1 Then
    '            Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
    '        End If

    '        CurY = CurY + 10
    '        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
    '        p1Font = New Font("Calibri", 12, FontStyle.Bold)
    '        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
    '        CurY = CurY + TxtHgt
    '        CurY = CurY + TxtHgt
    '        CurY = CurY + TxtHgt

    '        Common_Procedures.Print_To_PrintDocument(e, "Prepared By", LMargin + 20, CurY, 0, 0, pFont)
    '        Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 250, CurY, 0, 0, pFont)
    '        p1Font = New Font("Calibri", 12, FontStyle.Bold)

    '        Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
    '        CurY = CurY + TxtHgt + 10

    '        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
    '        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
    '        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    '        If Trim(Cmp_EMail) <> "" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Then '---- Asia Textiles (Tirupur)
    '            CurY = CurY + TxtHgt - 15
    '            p1Font = New Font("Calibri", 11, FontStyle.Bold)
    '            Common_Procedures.Print_To_PrintDocument(e, "Please send payment details of this bill to asiatextilestirupur@yahoo.in", LMargin + 10, CurY, 0, 0, p1Font)

    '        Else

    '            Cmp_EMail = ""
    '            If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
    '                Cmp_EMail = prn_HdDt.Rows(0).Item("Company_EMail").ToString
    '            End If
    '            If Trim(Cmp_EMail) <> "" Then
    '                CurY = CurY + TxtHgt - 15
    '                p1Font = New Font("Calibri", 11, FontStyle.Bold)
    '                Common_Procedures.Print_To_PrintDocument(e, "Please send payment details of this bill to " & Trim(LCase(Cmp_EMail)), LMargin + 10, CurY, 0, 0, p1Font)
    '            End If

    '        End If

    '    Catch ex As Exception

    '        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try

    'End Sub

    'Private Sub Printing_Format6(ByRef e As System.Drawing.Printing.PrintPageEventArgs)    '------- Kalaimagal Textiles
    '    Dim pFont As Font, pFont1 As Font, p1Font As Font
    '    Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
    '    Dim PrintWidth As Single, PrintHeight As Single
    '    Dim PageWidth As Single, PageHeight As Single
    '    Dim CurX As Single = 0
    '    Dim CurY As Single, TxtHgt As Single
    '    Dim LnAr(15) As Single, ClArr(15) As Single
    '    Dim ItmNm1 As String, ItmNm2 As String
    '    'Dim ItmDesc1 As String, ItmDesc2 As String
    '    'Dim ps As Printing.PaperSize
    '    Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
    '    Dim NetBilTxt As String = ""
    '    Dim W1 As Single
    '    Dim flperc As Single = 0
    '    Dim flmtr As Single = 0
    '    Dim fmtr As Single = 0
    '    Dim BInc As Integer
    '    Dim BnkDetAr() As String
    '    Dim Cmp_Name As String = "", Cmp_EMail As String = ""
    '    Dim Z1 As Single = 0
    '    Dim BmsInWrds As String
    '    Dim vprn_BlNos As String = ""
    '    Dim BLNo1 As String, BLNo2 As String
    '    Dim BankNm1 As String = ""
    '    Dim BankNm2 As String = ""
    '    Dim BankNm3 As String = ""
    '    Dim BankNm4 As String = ""
    '    Dim BankNm5 As String = ""
    '    Dim BankNm6 As String = ""
    '    Dim BankNm7 As String = ""
    '    Dim BankNm8 As String = ""
    '    Dim rndoff As Single, TtAmt As Double

    '    Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 9X12", 900, 1200)
    '    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
    '    PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

    '    'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
    '    '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
    '    '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
    '    '        PrintDocument1.DefaultPageSettings.PaperSize = ps
    '    '        'PageSetupDialog1.PageSettings.PaperSize = ps
    '    '        Exit For
    '    '    End If
    '    'Next

    '    With PrintDocument1.DefaultPageSettings.Margins
    '        .Left = 0 ' 65
    '        .Right = 0 ' 50
    '        .Top = 20 ' 65
    '        .Bottom = 0 ' 50
    '        LMargin = .Left
    '        RMargin = .Right
    '        TMargin = .Top
    '        BMargin = .Bottom
    '    End With

    '    pFont = New Font("Calibri", 11, FontStyle.Regular)
    '    pFont1 = New Font("Calibri", 8, FontStyle.Regular)

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

    '    TxtHgt = 20 ' e.Graphics.MeasureString("A", pFont).Height  ' 20
    '    NoofItems_PerPage = 10

    '    Try

    '        'For I = 100 To 1100 Step 300

    '        '    CurY = I
    '        '    For J = 1 To 850 Step 40

    '        '        CurX = J
    '        '        Common_Procedures.Print_To_PrintDocument(e, CurX, CurX, CurY - 20, 0, 0, pFont1)
    '        '        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY, 0, 0, pFont)

    '        '        CurX = J + 20
    '        '        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY, 0, 0, pFont)
    '        '        Common_Procedures.Print_To_PrintDocument(e, "|", CurX, CurY + 20, 0, 0, pFont)
    '        '        Common_Procedures.Print_To_PrintDocument(e, CurX, CurX, CurY + 40, 0, 0, pFont1)

    '        '    Next

    '        'Next

    '        'For I = 200 To 800 Step 250

    '        '    CurX = I
    '        '    For J = 1 To 1200 Step 40

    '        '        CurY = J
    '        '        Common_Procedures.Print_To_PrintDocument(e, "-", CurX, CurY, 0, 0, pFont)
    '        '        Common_Procedures.Print_To_PrintDocument(e, "   " & CurY, CurX, CurY, 0, 0, pFont1)

    '        '        CurY = J + 20
    '        '        Common_Procedures.Print_To_PrintDocument(e, "--", CurX, CurY, 0, 0, pFont)
    '        '        Common_Procedures.Print_To_PrintDocument(e, "   " & CurY, CurX, CurY, 0, 0, pFont1)

    '        '    Next

    '        'Next

    '        'e.HasMorePages = False


    '        If prn_HdDt.Rows.Count > 0 Then

    '            CurX = LMargin + 45 ' 40  '150
    '            CurY = TMargin + 190 ' 122 ' 100
    '            p1Font = New Font("Calibri", 12, FontStyle.Bold)
    '            Common_Procedures.Print_To_PrintDocument(e, "TO   " & "M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, CurX, CurY, 0, 0, p1Font)

    '            CurY = CurY + TxtHgt
    '            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, CurX + 10, CurY, 0, 0, pFont)

    '            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString) <> "" Then
    '                CurY = CurY + TxtHgt
    '                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, CurX + 10, CurY, 0, 0, pFont)
    '            End If
    '            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) <> "" Then
    '                CurY = CurY + TxtHgt
    '                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, CurX + 10, CurY, 0, 0, pFont)
    '            End If
    '            If Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString) <> "" Then
    '                CurY = CurY + TxtHgt
    '                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, CurX + 10, CurY, 0, 0, pFont)
    '            End If

    '            If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
    '                CurY = CurY + TxtHgt
    '                Common_Procedures.Print_To_PrintDocument(e, "Tin No : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, CurX + 10, CurY, 0, 0, pFont)
    '            End If

    '            'If Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString) <> "" Then
    '            '    CurY = CurY + TxtHgt
    '            '    Common_Procedures.Print_To_PrintDocument(e, "Ph.No : " & prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString, CurX, CurY, 0, 0, pFont)
    '            'End If
    '            W1 = e.Graphics.MeasureString("INVOICE DATE : ", pFont).Width

    '            CurX = LMargin + 500
    '            CurY = TMargin + 190
    '            p1Font = New Font("Calibri", 14, FontStyle.Bold)
    '            Common_Procedures.Print_To_PrintDocument(e, "Dc.Date ", CurX, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, ":", CurX + W1, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_Date").ToString, CurX + W1 + 10, CurY, 0, 0, pFont)

    '            CurX = LMargin + 500
    '            CurY = CurY + TxtHgt
    '            p1Font = New Font("Calibri", 14, FontStyle.Bold)
    '            Common_Procedures.Print_To_PrintDocument(e, "Invoice No  ", CurX, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, ":", CurX + W1, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Processed_Fabric_Sales_Invoice_No").ToString, CurX + W1 + 10, CurY, 0, 0, pFont)

    '            CurX = LMargin + 500
    '            CurY = CurY + TxtHgt
    '            p1Font = New Font("Calibri", 14, FontStyle.Bold)
    '            Common_Procedures.Print_To_PrintDocument(e, "Invoice Date ", CurX, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, ":", CurX + W1, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Processed_Fabric_Sales_Invoice_Date").ToString), "dd-MM-yyyy"), CurX + W1 + 10, CurY, 0, 0, pFont)

    '            CurX = LMargin + 500
    '            CurY = CurY + TxtHgt
    '            vprn_BlNos = ""
    '            For I = 0 To prn_DetDt.Rows.Count - 1
    '                If Trim(prn_DetDt.Rows(I).Item("Bales_Nos").ToString) <> "" Then
    '                    vprn_BlNos = Trim(vprn_BlNos) & IIf(Trim(vprn_BlNos) <> "", ", ", "") & prn_DetDt.Rows(I).Item("Bales_Nos").ToString
    '                End If
    '            Next


    '            BLNo1 = Trim(vprn_BlNos)
    '            BLNo2 = ""
    '            If Len(BLNo1) > 30 Then
    '                For I = 30 To 1 Step -1
    '                    If Mid$(Trim(BLNo1), I, 1) = " " Or Mid$(Trim(BLNo1), I, 1) = "," Or Mid$(Trim(BLNo1), I, 1) = "." Or Mid$(Trim(BLNo1), I, 1) = "-" Or Mid$(Trim(BLNo1), I, 1) = "/" Or Mid$(Trim(BLNo1), I, 1) = "_" Or Mid$(Trim(BLNo1), I, 1) = "(" Or Mid$(Trim(BLNo1), I, 1) = ")" Or Mid$(Trim(BLNo1), I, 1) = "\" Or Mid$(Trim(BLNo1), I, 1) = "[" Or Mid$(Trim(BLNo1), I, 1) = "]" Or Mid$(Trim(BLNo1), I, 1) = "{" Or Mid$(Trim(BLNo1), I, 1) = "}" Then Exit For
    '                Next I
    '                If I = 0 Then I = 30
    '                BLNo2 = Microsoft.VisualBasic.Right(Trim(BLNo1), Len(BLNo1) - I)
    '                BLNo1 = Microsoft.VisualBasic.Left(Trim(BLNo1), I - 1)
    '            End If

    '            p1Font = New Font("Calibri", 14, FontStyle.Bold)
    '            Common_Procedures.Print_To_PrintDocument(e, "Bales Nos ", CurX, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, ":", CurX + W1, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, BLNo1, CurX + W1 + 10, CurY, 0, 0, pFont)

    '            CurX = LMargin + 500
    '            CurY = CurY + TxtHgt
    '            p1Font = New Font("Calibri", 14, FontStyle.Bold)
    '            Common_Procedures.Print_To_PrintDocument(e, "To ", CurX, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, ":", CurX + W1, CurY, 0, 0, pFont)
    '            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Despatch_To").ToString, CurX + W1 + 10, CurY, 0, 0, pFont)

    '            CurX = LMargin + 500
    '            CurY = CurY + TxtHgt
    '            p1Font = New Font("Calibri", 14, FontStyle.Bold)
    '            Common_Procedures.Print_To_PrintDocument(e, "Lr.No :" & prn_HdDt.Rows(0).Item("Lr_No").ToString & "  Lr.Date :" & prn_HdDt.Rows(0).Item("Lr_Date").ToString, CurX, CurY, 0, 0, pFont)

    '            CurX = LMargin + 45
    '            CurY = TMargin + 300
    '            p1Font = New Font("Calibri", 14, FontStyle.Bold)
    '            Common_Procedures.Print_To_PrintDocument(e, "Transport :" & prn_HdDt.Rows(0).Item("TransportName").ToString, CurX, CurY, 0, 0, pFont)


    '            If prn_HdDt.Rows.Count > 0 Then

    '                Try

    '                    NoofDets = 0

    '                    CurY = TMargin + 380 ' 370

    '                    CurY = CurY + 5
    '                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cloth_Details").ToString, LMargin + 100, CurY, 0, 0, pFont)


    '                    CurY = CurY + 10

    '                    If prn_DetDt.Rows.Count > 0 Then

    '                        Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1


    '                            If NoofDets >= NoofItems_PerPage Then

    '                                CurX = LMargin + 550
    '                                CurY = CurY + TxtHgt

    '                                Common_Procedures.Print_To_PrintDocument(e, "Continued...", CurX, CurY, 0, 0, pFont)

    '                                NoofDets = NoofDets + 1

    '                                e.HasMorePages = True
    '                                Return

    '                            End If


    '                            prn_DetSNo = prn_DetSNo + 1


    '                            If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString) <> "" Then
    '                                ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString)
    '                            Else
    '                                ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Name").ToString)
    '                            End If
    '                            ItmNm2 = ""

    '                            If Len(ItmNm1) > 35 Then
    '                                For I = 20 To 1 Step -1
    '                                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
    '                                Next I
    '                                If I = 0 Then I = 35
    '                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
    '                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
    '                            End If

    '                            'If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString) <> "" Then
    '                            '    ItmDesc1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString)

    '                            'Else
    '                            '    ItmDesc1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Name").ToString)
    '                            'End If

    '                            'ItmDesc2 = ""
    '                            'If Len(ItmDesc1) > 35 Then
    '                            '    For I = 20 To 1 Step -1
    '                            '        If Mid$(Trim(ItmDesc1), I, 1) = " " Or Mid$(Trim(ItmDesc1), I, 1) = "," Or Mid$(Trim(ItmDesc1), I, 1) = "." Or Mid$(Trim(ItmDesc1), I, 1) = "-" Or Mid$(Trim(ItmDesc1), I, 1) = "/" Or Mid$(Trim(ItmDesc1), I, 1) = "_" Or Mid$(Trim(ItmDesc1), I, 1) = "(" Or Mid$(Trim(ItmDesc1), I, 1) = ")" Or Mid$(Trim(ItmDesc1), I, 1) = "\" Or Mid$(Trim(ItmDesc1), I, 1) = "[" Or Mid$(Trim(ItmDesc1), I, 1) = "]" Or Mid$(Trim(ItmDesc1), I, 1) = "{" Or Mid$(Trim(ItmDesc1), I, 1) = "}" Then Exit For
    '                            '    Next I
    '                            '    If I = 0 Then I = 35
    '                            '    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmDesc1), Len(ItmDesc1) - I)
    '                            '    ItmDesc1 = Microsoft.VisualBasic.Left(Trim(ItmDesc1), I - 1)
    '                            'End If

    '                            CurY = CurY + TxtHgt

    '                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + 100, CurY, 0, 0, pFont)
    '                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString, LMargin + 45, CurY, 0, 0, pFont)

    '                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Fold_Perc").ToString) = 0 Or Val(prn_DetDt.Rows(prn_DetIndx).Item("Fold_Perc").ToString) = 100 Or Trim(prn_HdDt.Rows(0).Item("FoldingRate_Status").ToString) = 1 Then
    '                                CurX = LMargin + 490
    '                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString, CurX, CurY, 1, 0, pFont)
    '                                CurX = LMargin + 580
    '                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, CurX, CurY, 1, 0, pFont)
    '                                CurX = LMargin + 730
    '                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString, CurX, CurY, 1, 0, pFont)

    '                            Else

    '                                CurX = LMargin + 490
    '                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString, CurX, CurY, 1, 0, pFont)

    '                                If Trim(ItmNm2) <> "" Then
    '                                    CurY = CurY + TxtHgt - 5
    '                                    CurX = LMargin + 100
    '                                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), CurX, CurY, 0, 0, pFont)
    '                                    NoofDets = NoofDets + 1
    '                                End If

    '                                flperc = 100 - Val(prn_DetDt.Rows(prn_DetIndx).Item("Fold_Perc").ToString)

    '                                'flmtr = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString) * flperc / 100, "#########0.00")
    '                                flmtr = Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString) * flperc / 100

    '                                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1035" Then
    '                                    flmtr = Math.Abs(Val(flmtr))
    '                                    flmtr = Common_Procedures.Meter_RoundOff(flmtr)
    '                                End If

    '                                CurY = CurY + TxtHgt
    '                                CurX = LMargin + 100

    '                                If Val(flperc) > 0 Then
    '                                    Common_Procedures.Print_To_PrintDocument(e, Val(flperc) & "%  Folding Less", CurX, CurY, 0, 0, pFont)
    '                                Else
    '                                    Common_Procedures.Print_To_PrintDocument(e, Val(flperc) & "%  Folding Add", CurX, CurY, 0, 0, pFont)
    '                                End If

    '                                CurX = LMargin + 490
    '                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(flmtr), "#######0.00"), CurX, CurY, 1, 0, pFont)

    '                                CurY = CurY + TxtHgt + 2
    '                                CurX = LMargin + 380
    '                                e.Graphics.DrawLine(Pens.Black, CurX, CurY, CurX + 100, CurY)

    '                                If Val(flperc) > 0 Then
    '                                    fmtr = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString) - Val(flmtr), "#########0.00")
    '                                Else
    '                                    fmtr = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString) + Val(flmtr), "#########0.00")
    '                                End If

    '                                CurY = CurY + 5
    '                                CurX = LMargin + 490
    '                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(fmtr), "#######0.00"), CurX, CurY, 1, 0, pFont)
    '                                CurX = LMargin + 580
    '                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, CurX, CurY, 1, 0, pFont)
    '                                CurX = LMargin + 730
    '                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString, CurX, CurY, 1, 0, pFont)

    '                            End If

    '                            NoofDets = NoofDets + 1

    '                            If Trim(ItmNm2) <> "" Then
    '                                CurY = CurY + TxtHgt
    '                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + 75, CurY, 0, 0, pFont)
    '                                NoofDets = NoofDets + 1
    '                            End If

    '                            prn_DetIndx = prn_DetIndx + 1

    '                        Loop

    '                    End If

    '                Catch ex As Exception

    '                    MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '                End Try
    '            End If

    '            For I = NoofDets + 1 To NoofItems_PerPage
    '                CurY = CurY + TxtHgt
    '            Next

    '            CurY = CurY + 10

    '            If Val(prn_HdDt.Rows(0).Item("Total_Amount").ToString) <> 0 Then
    '                CurX = LMargin + 370
    '                Common_Procedures.Print_To_PrintDocument(e, "Gross Value", CurX, TMargin + 560, 0, 0, pFont)
    '                'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Trade_Discount").ToString) & "%", CurX + 180, CurY, 0, 0, pFont)
    '                'Common_Procedures.Print_To_PrintDocument(e, "(-)", CurX + 250, CurY, 0, 0, pFont)
    '                CurX = LMargin + 730
    '                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Amount").ToString), CurX, TMargin + 560, 1, 0, pFont)
    '            End If

    '            Erase BnkDetAr
    '            If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
    '                BnkDetAr = Split(Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), ",")

    '                BInc = -1

    '                BInc = BInc + 1
    '                If UBound(BnkDetAr) >= BInc Then
    '                    BankNm1 = Trim(BnkDetAr(BInc))
    '                End If

    '                BInc = BInc + 1
    '                If UBound(BnkDetAr) >= BInc Then
    '                    BankNm2 = Trim(BnkDetAr(BInc))
    '                End If

    '                BInc = BInc + 1
    '                If UBound(BnkDetAr) >= BInc Then
    '                    BankNm3 = Trim(BnkDetAr(BInc))
    '                End If

    '                BInc = BInc + 1
    '                If UBound(BnkDetAr) >= BInc Then
    '                    BankNm4 = Trim(BnkDetAr(BInc))
    '                End If

    '                BInc = BInc + 1
    '                If UBound(BnkDetAr) >= BInc Then
    '                    BankNm5 = Trim(BnkDetAr(BInc))
    '                End If

    '                BInc = BInc + 1
    '                If UBound(BnkDetAr) >= BInc Then
    '                    BankNm6 = Trim(BnkDetAr(BInc))
    '                End If

    '                BInc = BInc + 1
    '                If UBound(BnkDetAr) >= BInc Then
    '                    BankNm7 = Trim(BnkDetAr(BInc))
    '                End If

    '                BInc = BInc + 1
    '                If UBound(BnkDetAr) >= BInc Then
    '                    BankNm8 = Trim(BnkDetAr(BInc))
    '                End If

    '            End If

    '            CurY = TMargin + 600
    '            CurY = CurY + TxtHgt

    '            If prn_HdDt.Rows(0).Item("Agent_Name").ToString <> "" Then
    '                CurX = LMargin + 45
    '                Common_Procedures.Print_To_PrintDocument(e, "AGENT : " & Trim(prn_HdDt.Rows(0).Item("Agent_Name").ToString), CurX, CurY, 0, 0, pFont)
    '            Else
    '                CurX = LMargin + 45
    '                Common_Procedures.Print_To_PrintDocument(e, "AGENT : DIRECT", CurX, CurY, 0, 0, pFont)
    '            End If

    '            If Val(prn_HdDt.Rows(0).Item("Trade_Discount_Perc").ToString) <> 0 Then
    '                CurX = LMargin + 370
    '                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("TradeDisc_Name").ToString), CurX, CurY, 0, 0, pFont)
    '                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Trade_Discount").ToString) & "%", CurX + 180, CurY, 0, 0, pFont)
    '                Common_Procedures.Print_To_PrintDocument(e, "(-)", CurX + 250, CurY, 0, 0, pFont)
    '                CurX = LMargin + 730
    '                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Trade_Discount_Perc").ToString), CurX, CurY, 1, 0, pFont)
    '            End If

    '            CurY = CurY + TxtHgt

    '            If Val(prn_HdDt.Rows(0).Item("Cash_Discount_Perc").ToString) <> 0 Then
    '                CurX = LMargin + 370
    '                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("CashDisc_Name").ToString), CurX, CurY, 0, 0, pFont)
    '                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Cash_Discount").ToString) & "%", CurX + 180, CurY, 0, 0, pFont)
    '                Common_Procedures.Print_To_PrintDocument(e, "(-)", CurX + 250, CurY, 0, 0, pFont)
    '                CurX = LMargin + 730
    '                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Cash_Discount_Perc").ToString), CurX, CurY, 1, 0, pFont)
    '            End If

    '            CurY = CurY + TxtHgt
    '            p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '            Common_Procedures.Print_To_PrintDocument(e, BankNm1 & "," & BankNm2, LMargin + 45, CurY, 0, 0, p1Font)

    '            If Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) <> 0 Then
    '                CurX = LMargin + 370
    '                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Packing_Name").ToString), CurX, CurY, 0, 0, pFont)
    '                Common_Procedures.Print_To_PrintDocument(e, "(+)", CurX + 250, CurY, 0, 0, pFont)
    '                CurX = LMargin + 730
    '                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Packing_Amount").ToString), CurX, CurY, 1, 0, pFont)
    '            End If

    '            CurY = CurY + TxtHgt
    '            p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '            Common_Procedures.Print_To_PrintDocument(e, BankNm3 & "," & BankNm4, LMargin + 45, CurY - 5, 0, 0, p1Font)
    '            If Val(prn_HdDt.Rows(0).Item("Freight").ToString) <> 0 Then
    '                CurX = LMargin + 370
    '                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Freight_Name").ToString), CurX, CurY, 0, 0, pFont)
    '                Common_Procedures.Print_To_PrintDocument(e, "(+)", CurX + 250, CurY, 0, 0, pFont)
    '                CurX = LMargin + 730
    '                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Freight").ToString), CurX, CurY, 1, 0, pFont)
    '            End If

    '            CurY = CurY + TxtHgt
    '            p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '            Common_Procedures.Print_To_PrintDocument(e, BankNm5 & "," & BankNm6, LMargin + 45, CurY, 0, 0, p1Font)
    '            If Val(prn_HdDt.Rows(0).Item("Insurance").ToString) <> 0 Then
    '                CurX = LMargin + 370
    '                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Insurance_Name").ToString), CurX, CurY, 0, 0, pFont)
    '                Common_Procedures.Print_To_PrintDocument(e, "(+)", CurX + 250, CurY, 0, 0, pFont)
    '                CurX = LMargin + 730
    '                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Insurance").ToString), CurX, CurY, 1, 0, pFont)
    '            End If

    '            TtAmt = Val(prn_HdDt.Rows(0).Item("total_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Freight").ToString) + Val(prn_HdDt.Rows(0).Item("Insurance").ToString) + Val(prn_HdDt.Rows(0).Item("Packing_amount").ToString) - Val(prn_HdDt.Rows(0).Item("Trade_Discount_Perc").ToString) - Val(prn_HdDt.Rows(0).Item("Cash_Discount_Perc").ToString)

    '            rndoff = 0
    '            rndoff = Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) - Val(TtAmt)

    '            CurY = CurY + TxtHgt
    '            p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '            Common_Procedures.Print_To_PrintDocument(e, BankNm7 & "," & BankNm8, LMargin + 45, CurY - 5, 0, 0, p1Font)
    '            If Val(rndoff) <> 0 Then
    '                CurX = LMargin + 370
    '                Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF", CurX, CurY, 0, 0, pFont)
    '                If Val(rndoff) >= 0 Then
    '                    Common_Procedures.Print_To_PrintDocument(e, "(+)", CurX + 250, CurY, 0, 0, pFont)
    '                Else
    '                    Common_Procedures.Print_To_PrintDocument(e, "(-)", CurX + 250, CurY, 0, 0, pFont)
    '                End If
    '                CurX = LMargin + 730
    '                Common_Procedures.Print_To_PrintDocument(e, " " & Format(Math.Abs(Val(rndoff)), "########0.00"), CurX, CurY, 1, 0, pFont)
    '            End If

    '            CurY = CurY + TxtHgt
    '            p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '            ' Common_Procedures.Print_To_PrintDocument(e, BankNm5, LMargin + 45, CurY, 0, 0, p1Font)

    '            CurY = CurY + TxtHgt ' 10
    '            p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '            '  Common_Procedures.Print_To_PrintDocument(e, BankNm6, LMargin + 45, CurY - 5, 0, 0, p1Font)
    '            p1Font = New Font("Calibri", 11, FontStyle.Bold)
    '            CurY = TMargin + 820
    '            CurX = LMargin + 730
    '            p1Font = New Font("Calibri", 12, FontStyle.Bold)
    '            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Net_Amount").ToString), CurX, CurY, 1, 0, p1Font)
    '            If Val(prn_HdDt.Rows(0).Item("Gr_Time").ToString) <> 0 Then
    '                p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '                Common_Procedures.Print_To_PrintDocument(e, "Due Date : " & Trim(prn_HdDt.Rows(0).Item("Gr_Time").ToString) & " Days " & "(" & Trim(prn_HdDt.Rows(0).Item("Gr_Date").ToString) & ")", LMargin + 10, CurY, 0, 0, p1Font)
    '            End If
    '            p1Font = New Font("Calibri", 10, FontStyle.Bold)
    '            CurY = TMargin + 880
    '            CurX = LMargin + 430
    '            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
    '            'BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "", "")

    '            Common_Procedures.Print_To_PrintDocument(e, BmsInWrds, LMargin + 140, CurY, 0, 0, p1Font)

    '        End If

    '    Catch ex As Exception

    '        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try

    '    e.HasMorePages = False

    'End Sub

    'Private Sub btn_Close_PrintFormat_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close_PrintFormat_Selection.Click
    '    pnl_Back.Enabled = True
    '    pnl_PrintFormat_Selection.Visible = False
    'End Sub

    'Private Sub btn_Cancel_PrintFormat_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Cancel_PrintFormat_Selection.Click
    '    pnl_Back.Enabled = True
    '    pnl_PrintFormat_Selection.Visible = False
    'End Sub

    'Private Sub btn_Print_Inv_Format1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print_Inv_Format1.Click
    '    InvPrintFrmt = "FORMAT-4"

    '    Printing_Invoice()
    '    btn_Cancel_PrintFormat_Selection_Click(sender, e)
    'End Sub

    'Private Sub btn_Print_Inv_Format2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print_Inv_Format2.Click
    '    InvPrintFrmt = "FORMAT-3"

    '    Printing_Invoice()
    '    btn_Cancel_PrintFormat_Selection_Click(sender, e)
    'End Sub




    ''Private Sub txt_lcDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_LcDate.KeyDown
    '    vcbo_KeyDwnVal = e.KeyValue
    'End Sub
    Private Sub txt_LcDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_LcDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            msk_LcDate.Text = Date.Today
            msk_LcDate.SelectionStart = msk_LcDate.Text.Length
        End If
    End Sub

    Private Sub txt_Lr_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Lr_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub txt_Lr_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Lr_Date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            msk_Lr_Date.Text = Date.Today
            msk_Lr_Date.SelectionStart = msk_Lr_Date.Text.Length
        End If
    End Sub

    Private Sub txt_Order_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_OrderDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub msk_OrderDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_OrderDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            msk_OrderDate.Text = Date.Today
            msk_OrderDate.SelectionStart = msk_OrderDate.Text.Length
        End If
    End Sub

    Private Sub txt_DcDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_DcDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub txt_DcDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_DcDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            msk_DcDate.Text = Date.Today
            msk_DcDate.SelectionStart = msk_DcDate.Text.Length
        End If
    End Sub
    Private Sub txt_grDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_GrDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub
    Private Sub txt_grDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_GrDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            msk_GrDate.Text = Date.Today
            msk_GrDate.SelectionStart = msk_GrDate.Text.Length
        End If
    End Sub

    Private Sub dtp_OrderDate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtp_OrderDate.ValueChanged
        msk_OrderDate.Text = dtp_OrderDate.Text
    End Sub

    Private Sub dtp_OrderDate_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_OrderDate.Enter
        msk_OrderDate.Focus()
        msk_OrderDate.SelectionStart = 0
    End Sub

    Private Sub dtp_DcDate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtp_DcDate.ValueChanged
        msk_DcDate.Text = dtp_DcDate.Text
    End Sub

    Private Sub dtp_DcDate_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_DcDate.Enter
        msk_DcDate.Focus()
        msk_DcDate.SelectionStart = 0
    End Sub
    Private Sub dtp_GrDate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtp_GrDate.ValueChanged
        msk_GrDate.Text = dtp_OrderDate.Text
    End Sub

    Private Sub dtp_GrDate_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_GrDate.Enter
        msk_GrDate.Focus()
        msk_GrDate.SelectionStart = 0
    End Sub
    Private Sub dtp_Lrdate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtp_Lrdate.ValueChanged
        msk_Lr_Date.Text = dtp_Lrdate.Text
    End Sub

    Private Sub dtp_Lrdate_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Lrdate.Enter
        msk_Lr_Date.Focus()
        msk_Lr_Date.SelectionStart = 0
    End Sub
    Private Sub dtp_LcDate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtp_LcDate.ValueChanged
        msk_LcDate.Text = dtp_LcDate.Text
    End Sub

    Private Sub dtp_LcDate_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_LcDate.Enter
        msk_LcDate.Focus()
        msk_LcDate.SelectionStart = 0
    End Sub


    Private Sub dtp_Date_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtp_Date.ValueChanged
        msk_Date.Text = dtp_Date.Text
    End Sub

    Private Sub dtp_Date_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.Enter
        msk_Date.Focus()
        msk_Date.SelectionStart = 0
    End Sub

    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            cbo_PartyName.Focus()
        End If

        If e.KeyCode = 38 Then
            txt_InvoicePrefixNo.Focus()
        End If

        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_Date.Text
            vmskSelStrt = msk_Date.SelectionStart
        End If
    End Sub

    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        If e.KeyCode = 107 Then
            msk_Date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_Date.Text))
            msk_Date.SelectionStart = 0
        ElseIf e.KeyCode = 109 Then
            msk_Date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_Date.Text))
            msk_Date.SelectionStart = 0
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)
        End If
    End Sub

    Private Sub btn_Direct_BaleDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Direct_BaleDetails.Click
        pnl_Direct_BaleDetails.Visible = True
        pnl_Back.Enabled = False
        dgv_Direct_BaleDetails.Focus()
        If dgv_Direct_BaleDetails.Rows.Count > 0 Then
            dgv_Direct_BaleDetails.CurrentCell = dgv_Direct_BaleDetails.Rows(0).Cells(1)
            dgv_Direct_BaleDetails.CurrentCell.Selected = True
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

        Try

            Total_Direct_BaleDetailsEntry_Calculation()

            TotBals = 0
            TotPcs = 0
            TotMtrs = 0
            TotWgt = 0
            With dgv_Direct_BaleDetails_Total
                If .RowCount > 0 Then
                    TotBals = Val(.Rows(0).Cells(1).Value)
                    TotPcs = Val(.Rows(0).Cells(2).Value)
                    TotMtrs = Val(.Rows(0).Cells(3).Value)
                    TotWgt = Val(.Rows(0).Cells(4).Value)
                End If
            End With


            Cmd.Connection = Con

            Cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
            Cmd.ExecuteNonQuery()

            For I = 0 To dgv_Direct_BaleDetails.Rows.Count - 1

                If Trim(dgv_Direct_BaleDetails.Rows(I).Cells(1).Value) <> "" And Val(dgv_Direct_BaleDetails.Rows(I).Cells(3).Value) <> 0 Then

                    Cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Meters1) values ('" & Trim(dgv_Direct_BaleDetails.Rows(I).Cells(1).Value) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(dgv_Direct_BaleDetails.Rows(I).Cells(1).Value))) & " ) "
                    Cmd.ExecuteNonQuery()

                End If

            Next


            BlNo = ""
            FsNo = 0 : LsNo = 0
            FsBaleNo = "" : LsBaleNo = ""

            Da1 = New SqlClient.SqlDataAdapter("Select Name1 as Bale_No, Meters1 as fororderby_baleno from " & Trim(Common_Procedures.EntryTempTable) & " where Name1 <> '' order by Meters1, Name1", Con)
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


            If dgv_Details.Rows.Count > 0 Then
                dgvDet_CurRow = 0  ' dgv_Details.CurrentCell.RowIndex

                If Val(TotBals) <> 0 And Val(TotMtrs) <> 0 Then
                    dgv_Details.Rows(dgvDet_CurRow).Cells(4).Value = TotBals
                    dgv_Details.Rows(dgvDet_CurRow).Cells(5).Value = BlNo
                    If Val(TotPcs) <> 0 Then
                        dgv_Details.Rows(dgvDet_CurRow).Cells(6).Value = Val(TotPcs)
                    End If
                    dgv_Details.Rows(dgvDet_CurRow).Cells(7).Value = Format(Val(TotMtrs), "#########0.00")
                    dgv_Details.Rows(dgvDet_CurRow).Cells(8).Value = Format(Val(TotWgt), "#########0.000")
                    ' dgv_Details.Rows(dgvDet_CurRow).Cells(16).Value = ""

                    With dgv_Details
                        If .Visible Then
                           

                                'fldmtr = Format(Val(.Rows(dgvDet_CurRow).Cells(7).Value) * Val(.Rows(dgvDet_CurRow).Cells(3).Value) / 100, "#########0.00")

                            .Rows(dgvDet_CurRow).Cells(11).Value = Format(Val(.Rows(dgvDet_CurRow).Cells(7).Value) * Val(.Rows(dgvDet_CurRow).Cells(9).Value), "#########0.00")

                            ' End If

                            Total_Calculation()

                        End If

                    End With

                End If
            End If


            pnl_Back.Enabled = True
            pnl_Direct_BaleDetails.Visible = False

            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                dgv_Details.CurrentCell.Selected = True

            Else
                txt_Trade_Disc.Focus()

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "INVALID BALAE DETAILS ENTRY...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Direct_BaleDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Direct_BaleDetails.CellEnter
        With dgv_Direct_BaleDetails
            If e.RowIndex > 0 Then
                If Val(.CurrentRow.Cells(1).Value) = 0 And e.RowIndex = .RowCount - 1 Then
                    .Rows(e.RowIndex).Cells(1).Value = Val(.Rows(e.RowIndex - 1).Cells(1).Value) + 1
                End If
                If e.ColumnIndex = 1 And e.RowIndex = .RowCount - 1 And Val(.CurrentRow.Cells(2).Value) = 0 And Val(.CurrentRow.Cells(3).Value) = 0 Then
                    .Rows(e.RowIndex).Cells(1).Value = Val(.Rows(e.RowIndex - 1).Cells(1).Value) + 1
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Direct_BaleDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Direct_BaleDetails.EditingControlShowing
        dgtxt_Direct_BaleDetails = Nothing
        If dgv_Direct_BaleDetails.CurrentCell.ColumnIndex >= 1 Then
            dgtxt_Direct_BaleDetails = CType(dgv_Direct_BaleDetails.EditingControl, DataGridViewTextBoxEditingControl)
        End If
    End Sub

    Private Sub dgtxt_Direct_BaleDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Direct_BaleDetails.Enter
        dgv_Direct_BaleDetails.EditingControl.BackColor = Color.Lime
        dgv_Direct_BaleDetails.EditingControl.ForeColor = Color.Blue
        dgtxt_Direct_BaleDetails.SelectAll()
    End Sub

    Private Sub dgtxt_Direct_BaleDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Direct_BaleDetails.KeyPress

        Try
            With dgv_Direct_BaleDetails
                If .Visible Then

                    If .Rows.Count > 0 Then

                        If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Then

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
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value)
                    Else
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                    End If
                End If

                If .CurrentCell.ColumnIndex = 3 Then
                    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                    Else
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                    End If
                End If

                If .CurrentCell.ColumnIndex = 4 Then
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
            If IsNothing(dgv_Direct_BaleDetails.CurrentCell) Then Exit Sub
            With dgv_Direct_BaleDetails
                If .Visible Then

                    If (.CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3) And Val(.CurrentCell.Value) <> 0 Then
                        If .CurrentRow.Index = .Rows.Count - 1 Then
                            .Rows.Add()
                        End If
                    End If

                    If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Then

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
        If Not IsNothing(dgv_Direct_BaleDetails.CurrentCell) Then dgv_Direct_BaleDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Direct_BaleDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Direct_BaleDetails.RowsAdded
        Dim n As Integer = -1

        Try
            If IsNothing(dgv_Direct_BaleDetails.CurrentCell) Then Exit Sub
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
        Dim TotPcs As Single
        Dim TotBals As Single
        Dim TotMtrs As Single
        Dim TotWgt As Single

        If NoCalc_Status = True Then Exit Sub

        Sno = 0
        TotPcs = 0 : TotBals = 0 : TotMtrs = 0 : TotWgt = 0

        With dgv_Direct_BaleDetails
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(3).Value) <> 0 Then
                    TotBals = TotBals + 1
                    TotPcs = TotPcs + Val(.Rows(i).Cells(2).Value())
                    TotMtrs = TotMtrs + Val(.Rows(i).Cells(3).Value())
                    TotWgt = TotWgt + Val(.Rows(i).Cells(4).Value())

                End If

            Next i

        End With

        With dgv_Direct_BaleDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(1).Value = Val(TotBals)
            .Rows(0).Cells(2).Value = Val(TotPcs)
            .Rows(0).Cells(3).Value = Format(Val(TotMtrs), "########0.00")
            .Rows(0).Cells(4).Value = Format(Val(TotWgt), "########0.000")
        End With

    End Sub

    Private Sub btn_SaveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SaveAll.Click
        SaveAll_STS = True

        LastNo = ""
        movelast_record()

        LastNo = lbl_InvNo.Text

        movefirst_record()
        Timer1.Enabled = True

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        save_record()
        If Trim(UCase(LastNo)) = Trim(UCase(lbl_InvNo.Text)) Then
            Timer1.Enabled = False
            SaveAll_STS = False
            MessageBox.Show("All entries saved Successfully", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Else
            movenext_record()

        End If
    End Sub

    'Private Sub btn_Print_PrePrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_PrePrint.Click
    '    prn_Status = 1
    '    Printing_Invoice()
    '    btn_print_Close_Click(sender, e)
    'End Sub

    Private Sub btn_BaleSelection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_RollSelection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim FB_ID As Integer, Colr_ID As Integer, Procs_ID As Integer
        Dim NewCode As String
        ' Dim Fd_Perc As Integer
        Dim CompIDCondt As String
        Dim dgvDet_CurRow As Integer
        Dim dgv_DetSlNo As Long

        Try

            If dgv_Details.CurrentCell.RowIndex < 0 Then
                MessageBox.Show("Invalid Cloth Name", "DOES NOT SELECT BALE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If dgv_Details.Enabled And dgv_Details.Visible Then
                    If dgv_Details.Rows.Count > 0 Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                        dgv_Details.CurrentCell.Selected = True
                    End If
                End If
                Exit Sub
            End If

            FB_ID = Common_Procedures.Cloth_NameToIdNo(Con, dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(1).Value)
            If FB_ID = 0 Then
                MessageBox.Show("Invalid Cloth Name", "DOES NOT SELECT BALE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If dgv_Details.Enabled And dgv_Details.Visible Then
                    If dgv_Details.Rows.Count > 0 Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                        If cbo_Grid_ClothName.Visible And cbo_Grid_ClothName.Enabled Then cbo_Grid_ClothName.Focus()
                        'dgv_Details.CurrentCell.Selected = True
                        Exit Sub
                    End If
                End If
                Exit Sub
            End If

            Colr_ID = Common_Procedures.Colour_NameToIdNo(Con, dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(2).Value)
            If Colr_ID = 0 Then
                MessageBox.Show("Invalid Colour Name ", "DOES NOT SELECT BALE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If dgv_Details.Enabled And dgv_Details.Visible Then
                    If dgv_Details.Rows.Count > 0 Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(2)
                        If cbo_Grid_Colour.Visible And cbo_Grid_Colour.Enabled Then cbo_Grid_Colour.Focus()
                        Exit Sub
                    End If
                End If
                Exit Sub
            End If

         
            Procs_ID = Common_Procedures.Process_NameToIdNo(Con, dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(3).Value)
            If Procs_ID = 0 Then
                MessageBox.Show("Invalid Process Name ", "DOES NOT SELECT BALE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If dgv_Details.Enabled And dgv_Details.Visible Then
                    If dgv_Details.Rows.Count > 0 Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(3)
                        If cbo_Grid_Process.Visible And cbo_Grid_Process.Enabled Then cbo_Grid_Process.Focus()
                        Exit Sub
                    End If
                End If
                Exit Sub
            End If

            CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
            If Trim(UCase(Common_Procedures.settings.CompanyName)) = "-----~~~" Then
                CompIDCondt = ""
            End If

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            dgvDet_CurRow = dgv_Details.CurrentCell.RowIndex
            dgv_DetSlNo = Val(dgv_Details.Rows(dgvDet_CurRow).Cells(12).Value)

            With dgv_BaleSelection
                chk_SelectAll.Checked = False
                .Rows.Clear()
                SNo = 0

                Da = New SqlClient.SqlDataAdapter("Select a.* from Processed_Fabric_inspection_Details a where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Sales_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.SalesInvoice_DetailsSlNo = " & Str(Val(dgv_DetSlNo)) & " and a.Fabric_IdNo = " & Str(Val(FB_ID)) & "  and a.Colour_IdNo = " & Str(Val(Colr_ID)) & "  and a.Process_IdNo = " & Str(Val(Procs_ID)) & " order by a.Processed_Fabric_inspection_Date, a.for_orderby, a.Processed_Fabric_inspection_No, a.Processed_Fabric_inspection_Code", Con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Roll_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Pcs_No").ToString
                        .Rows(n).Cells(3).Value = Format(Val(Dt1.Rows(i).Item("Meters").ToString), "#########0.00")
                        .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Weight").ToString), "#########0.000")
                        .Rows(n).Cells(5).Value = "1"
                        .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Roll_Code").ToString
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Roll_Or_Bundle").ToString
                        
                        For j = 0 To .ColumnCount - 1
                            .Rows(i).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Next

                End If
                Dt1.Clear()

                Da = New SqlClient.SqlDataAdapter("Select a.* from Processed_Fabric_inspection_Details a where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Sales_Invoice_Code = '' and a.Fabric_IdNo = " & Str(Val(FB_ID)) & "  and a.Colour_IdNo = " & Str(Val(Colr_ID)) & "  and a.Process_IdNo = " & Str(Val(Procs_ID)) & " order by a.Processed_Fabric_inspection_Date, a.for_orderby, a.Processed_Fabric_inspection_No, a.Processed_Fabric_inspection_Code", Con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Roll_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Pcs_No").ToString
                        .Rows(n).Cells(3).Value = Format(Val(Dt1.Rows(i).Item("Meters").ToString), "#########0.00")
                        .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Weight").ToString), "#########0.000")
                        .Rows(n).Cells(5).Value = ""
                        .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Roll_Code").ToString
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Roll_Or_Bundle").ToString

                    Next

                End If
                Dt1.Clear()

            End With

            pnl_BaleSelection.Visible = True
            pnl_Back.Enabled = False
            dgv_BaleSelection.Focus()
            If dgv_BaleSelection.Rows.Count > 0 Then
                dgv_BaleSelection.CurrentCell = dgv_BaleSelection.Rows(0).Cells(0)
                dgv_BaleSelection.CurrentCell.Selected = True
            End If

        Catch ex As NullReferenceException
            MessageBox.Show("Select the ClothName for Bale Selection", "DOES NOT SELECT BALE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT BALE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try



    End Sub

    Private Sub dgv_BaleSelection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BaleSelection.CellClick
        Select_Bale(e.RowIndex)
    End Sub

    Private Sub Select_Bale(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_BaleSelection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(5).Value = (Val(.Rows(RwIndx).Cells(5).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(5).Value) = 0 Then .Rows(RwIndx).Cells(5).Value = ""

                For i = 0 To .ColumnCount - 1
                    .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                Next

            End If

        End With

    End Sub

    Private Sub dgv_BaleSelection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_BaleSelection.KeyDown
        On Error Resume Next

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_BaleSelection.CurrentCell.RowIndex >= 0 Then
                Select_Bale(dgv_BaleSelection.CurrentCell.RowIndex)
                e.Handled = True
            End If
        End If

        If e.KeyCode = Keys.Delete Or e.KeyCode = Keys.Back Then
            If dgv_BaleSelection.CurrentCell.RowIndex >= 0 Then
                If Val(dgv_BaleSelection.Rows(dgv_BaleSelection.CurrentCell.RowIndex).Cells(5).Value) = 1 Then
                    e.Handled = True
                    Select_Bale(dgv_BaleSelection.CurrentCell.RowIndex)
                End If
            End If
        End If

    End Sub

    Private Sub btn_Close_BaleSelection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_BaleSelection.Click
        Dim Cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim I As Integer, J As Integer
        Dim n As Integer
        Dim sno As Integer
        Dim dgvDet_CurRow As Integer = 0
        Dim dgv_DetSlNo As Integer = 0
        Dim NoofBls As Integer
        Dim FsNo As Single, LsNo As Single
        Dim FsBaleNo As String, LsBaleNo As String
        Dim BlNo As String, PackSlpCodes As String
        Dim Tot_Pcs As Single, Tot_Mtrs As Single, Tot_wGT As Single


        Cmd.Connection = Con

        dgvDet_CurRow = dgv_Details.CurrentCell.RowIndex
        dgv_DetSlNo = Val(dgv_Details.Rows(dgvDet_CurRow).Cells(12).Value)

        With dgv_BaleSelectionDetails

LOOP1:
            For I = 0 To .RowCount - 1

                If Val(.Rows(I).Cells(0).Value) = Val(dgv_DetSlNo) Then

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

            Cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
            Cmd.ExecuteNonQuery()

            NoofBls = 0 : Tot_Pcs = 0 : Tot_Mtrs = 0 : Tot_wGT = 0 : BlNo = "" : PackSlpCodes = ""

            For I = 0 To dgv_BaleSelection.RowCount - 1

                If Val(dgv_BaleSelection.Rows(I).Cells(5).Value) = 1 Then

                    n = .Rows.Add()

                    sno = sno + 1
                    .Rows(n).Cells(0).Value = Val(dgv_DetSlNo)
                    .Rows(n).Cells(1).Value = dgv_BaleSelection.Rows(I).Cells(1).Value
                    .Rows(n).Cells(2).Value = Val(dgv_BaleSelection.Rows(I).Cells(2).Value)
                    .Rows(n).Cells(3).Value = Format(Val(dgv_BaleSelection.Rows(I).Cells(3).Value), "#########0.00")
                    .Rows(n).Cells(4).Value = Format(Val(dgv_BaleSelection.Rows(I).Cells(4).Value), "#########0.000")
                    .Rows(n).Cells(5).Value = dgv_BaleSelection.Rows(I).Cells(6).Value
                    cbo_RollBundle.Text = dgv_BaleSelection.Rows(0).Cells(7).Value

                    Cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Meters1) values ('" & Trim(dgv_BaleSelection.Rows(I).Cells(6).Value) & "', '" & Trim(dgv_BaleSelection.Rows(I).Cells(1).Value) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(dgv_BaleSelection.Rows(I).Cells(1).Value))) & " ) "
                    Cmd.ExecuteNonQuery()

                    NoofBls = NoofBls + 1
                    Tot_Pcs = Val(Tot_Pcs) + Val(dgv_BaleSelection.Rows(I).Cells(2).Value)
                    Tot_Mtrs = Val(Tot_Mtrs) + Val(dgv_BaleSelection.Rows(I).Cells(3).Value)
                    Tot_wGT = Val(Tot_wGT) + Val(dgv_BaleSelection.Rows(I).Cells(4).Value)
                    PackSlpCodes = Trim(PackSlpCodes) & IIf(Trim(PackSlpCodes) = "", "~", "") & Trim(dgv_BaleSelection.Rows(I).Cells(6).Value) & "~"

                End If

            Next

            BlNo = ""
            FsNo = 0 : LsNo = 0
            FsBaleNo = "" : LsBaleNo = ""

            Da1 = New SqlClient.SqlDataAdapter("Select Name1 as Bale_Code, Name2 as Bale_No, Meters1 as fororderby_baleno from " & Trim(Common_Procedures.EntryTempTable) & " where Name1 <> '' order by Meters1, Name2, Name1", Con)
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

            If Trim(dgv_Details.Rows(dgvDet_CurRow).Cells(13).Value) <> "" Then
                dgv_Details.Rows(dgvDet_CurRow).Cells(4).Value = ""
                dgv_Details.Rows(dgvDet_CurRow).Cells(5).Value = ""
                dgv_Details.Rows(dgvDet_CurRow).Cells(6).Value = ""
                dgv_Details.Rows(dgvDet_CurRow).Cells(7).Value = ""
                dgv_Details.Rows(dgvDet_CurRow).Cells(13).Value = ""
            End If
            If Val(NoofBls) <> 0 And Val(Tot_Mtrs) <> 0 Then
                dgv_Details.Rows(dgvDet_CurRow).Cells(4).Value = NoofBls
                dgv_Details.Rows(dgvDet_CurRow).Cells(5).Value = BlNo
                If Val(Tot_Pcs) <> 0 Then
                    dgv_Details.Rows(dgvDet_CurRow).Cells(6).Value = Val(Tot_Pcs)
                End If
                dgv_Details.Rows(dgvDet_CurRow).Cells(7).Value = Format(Val(Tot_Mtrs), "#########0.00")
                dgv_Details.Rows(dgvDet_CurRow).Cells(8).Value = Format(Val(Tot_wGT), "#########0.000")
                dgv_Details.Rows(dgvDet_CurRow).Cells(13).Value = PackSlpCodes

            End If

            Amount_Calculation(dgvDet_CurRow, 7)

            Add_NewRow_ToGrid()

            Total_Calculation()

        End With

        pnl_Back.Enabled = True
        pnl_BaleSelection.Visible = False
        If dgv_Details.Enabled And dgv_Details.Visible Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                If dgv_Details.CurrentCell.RowIndex >= 0 Then
                    dgv_Details.CurrentCell = dgv_Details.Rows(dgv_Details.CurrentCell.RowIndex).Cells(9)
                    dgv_Details.CurrentCell.Selected = True
                End If
            End If
        End If

    End Sub

    Private Sub Add_NewRow_ToGrid()
        On Error Resume Next

        Dim i As Integer
        Dim n As Integer = -1

        With dgv_Details
            If .Visible Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                    If Trim(UCase(cbo_Type.Text)) <> "ORDER" And Trim(UCase(cbo_Type.Text)) <> "DELIVERY" Then
                        n = .Rows.Add()

                        For i = 0 To .Columns.Count - 1
                            .Rows(n).Cells(i).Value = .Rows(.CurrentCell.RowIndex).Cells(i).Value
                            .Rows(.CurrentCell.RowIndex).Cells(i).Value = ""
                        Next

                        For i = 0 To .Rows.Count - 1
                            .Rows(i).Cells(0).Value = i + 1
                        Next

                        .CurrentCell = .Rows(n).Cells(.CurrentCell.ColumnIndex)
                        .CurrentCell.Selected = True

                    End If
                End If

            End If

        End With

    End Sub
    Private Sub cbo_Grid_Process_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Process.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "Process_Head", "Process_Name", "", "(Process_IdNo = 0)")

    End Sub
    Private Sub cbo_Grid_Process_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Process.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Grid_Process, Nothing, Nothing, "Process_Head", "Process_Name", "", "(process_IdNo = 0)")
        vcbo_KeyDwnVal = e.KeyValue

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Process.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_Process.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With

    End Sub

    Private Sub cbo_Grid_Process_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Process.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Grid_Process, Nothing, "Process_Head", "Process_Name", "", "(Process_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details
                e.Handled = True
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
            End With

        End If

    End Sub

    Private Sub cbo_Grid_Currency_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Currency.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "Currency_Head", "Currency_Name", "", "(Currency_IdNo = 0)")

    End Sub
    Private Sub cbo_Grid_Currency_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Currency.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Grid_Currency, Nothing, Nothing, "Currency_Head", "Currency_Name", "", "(Currency_IdNo = 0)")
        vcbo_KeyDwnVal = e.KeyValue

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Currency.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_Currency.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With
    End Sub

    Private Sub cbo_Grid_Currency_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Currency.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Grid_Currency, Nothing, "Currency_Head", "Currency_Name", "", "(Currency_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details
                e.Handled = True
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
            End With

        End If
    End Sub

    Private Sub cbo_Grid_Currency_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Currency.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Currency_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Currency.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Grid_Currency_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Currency.TextChanged
        Try
            If cbo_Grid_Currency.Visible Then
                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_Grid_Currency.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 10 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Currency.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

  
    Private Sub cbo_RollBundle_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_RollBundle.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_RollBundle, msk_LcDate, Nothing, "", "", "", "")

        With dgv_Details

            If (e.KeyValue = 40 And cbo_Grid_ClothName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .RowCount > 0 Then
                    cbo_Grid_ClothName.Focus()

                Else
                    txt_Trade_Disc.Focus()
                End If

            End If

        End With
    End Sub

    Private Sub cbo_RollBundle_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_RollBundle.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_RollBundle, Nothing, "", "", "", "")

        With dgv_Details
            If Asc(e.KeyChar) = 13 Then

                If .RowCount > 0 Then
                    cbo_Grid_ClothName.Focus()

                Else
                    txt_Trade_Disc.Focus()
                End If
            End If
        End With

    End Sub
    Private Sub cbo_LotNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_LotNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Lot_head", "Lot_No", "", "(Lot_IdNo = 0)")
    End Sub

    Private Sub cbo_LotNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_LotNo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_LotNo, txt_Vechile, txt_LcNo, "Lot_head", "Lot_No", "", "(Lot_IdNo = 0)")
    End Sub

    Private Sub cbo_LotNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_LotNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_LotNo, txt_LcNo, "Lot_head", "Lot_No", "", "(Lot_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True

                If MessageBox.Show("Do you want to enter Bale Details?", "FOR BALE DETAILS ENTRY...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    btn_Direct_BaleDetails_Click(sender, e)

                Else
                    If dgv_Details.Rows.Count > 0 Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                        dgv_Details.CurrentCell.Selected = True

                    Else
                        txt_Trade_Disc.Focus()

                    End If

                End If
        End If

    End Sub
    Private Sub cbo_Lotno_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_LotNo.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New LotNo_creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_LotNo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub dgtxt_Details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.TextChanged
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

    Public Sub Printing_Bale_Estiamte()
        Dim prtFrm As Single = 0
        Dim prtTo As Single = 0
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim Condt As String = ""
        Dim PpSzSTS As Boolean = False
        Dim ps As Printing.PaperSize
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from Processed_Fabric_Invoice_BaleEntry_Details a  Where a.Sales_Invoice_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", Con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count <= 0 Then

                MessageBox.Show("No Entry Found", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub

            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            Try

                'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8.25X12", 850, 1200)
                'PrintDocument2.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
                'PrintDocument2.DefaultPageSettings.PaperSize = pkCustomSize1

                For I = 0 To PrintDocument2.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument2.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                        ps = PrintDocument2.PrinterSettings.PaperSizes(I)
                        PrintDocument2.DefaultPageSettings.PaperSize = ps
                        Exit For
                    End If
                Next

                PrintDialog1.PrinterSettings = PrintDocument2.PrinterSettings
                If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    PrintDocument2.PrinterSettings = PrintDialog1.PrinterSettings
                    PrintDocument2.Print()
                End If

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT SHOW PRINT PREVIEW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try

        Else

            Try

                Dim ppd As New PrintPreviewDialog

                For I = 0 To PrintDocument2.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument2.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                        ps = PrintDocument2.PrinterSettings.PaperSizes(I)
                        PrintDocument2.DefaultPageSettings.PaperSize = ps
                        Exit For
                    End If
                Next

                ppd.Document = PrintDocument1

                ppd.WindowState = FormWindowState.Normal
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.ClientSize = New Size(600, 600)

                ppd.ShowDialog()
                'If PageSetupDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                '    PrintDocument2.DefaultPageSettings = PageSetupDialog1.PageSettings
                '    ppd.ShowDialog()
                'End If

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If

        pnl_Back.Enabled = True
        pnl_Print.Visible = False

    End Sub

    Private Sub Printing_Format19(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer = 0
        Dim FldLessPerc As Single = 0
        Dim FldLessMtr As Single = 0
        Dim fmtr As Single = 0
        Dim FldPerc As Single = 0
        Dim strFldPerCM As String = ""
        Dim Half_Width As Single = 0
        Dim Half_Height As Single = 0


        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        Half_Width = 825
        Half_Height = 1167

        'With PrintDocument1.DefaultPageSettings.Margins
        '    .Left = 20 ' 30 
        '    .Right = 40
        '    .Top = 30 ' 50 
        '    .Bottom = 40
        '    LMargin = .Left
        '    RMargin = .Right
        '    TMargin = .Top
        '    BMargin = .Bottom
        'End With

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = (Half_Width - 600) / 2
            .Right = ((Half_Width - 600) / 2) + 50
            .Top = 5
            .Bottom = Half_Height - Half_Width
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With


        pFont = New Font("Calibri", 8, FontStyle.Regular)

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize

            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin

        End With

        NoofItems_PerPage = 3

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = 350 : ClAr(2) = 60 : ClAr(3) = 60
        ClAr(4) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3))

        TxtHgt = 19  ' e.Graphics.MeasureString("A", pFont).Height  ' 20


        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format19_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 10, CurY, 0, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format19_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If


                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Name").ToString), LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Rate_Meter").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        prn_DetIndx = prn_DetIndx + 1

                    Loop



                End If

                Printing_Bale_Details(e, ClAr(1), LMargin, CurY)

                Printing_Format19_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

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
    Private Sub Printing_Bale_Details(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal prn_width As Single, ByVal LMArgin As Single, ByRef CurY As Single)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim NoofDets As Integer, NoofItems_PerSubPage As Integer
        Dim pFont As Font
        Dim TxtHgt As Single
        Dim SubClAr(15) As Single
        Dim W1 As Single = 0
        Dim SNo As Integer = 0
        Dim Centr As Single = 0
        Dim Ttl_Bale As Single = 0
        Dim Ttl_Pcs As Single = 0
        Dim Ttl_Mtrs As Single = 0
        Dim Pos As Single = 0
        Dim CurY_Temp As Single = 0


        Dim prn_SubDetIndx As Integer = 0


        pFont = New Font("Calibri", 8, FontStyle.Regular)

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        NoofItems_PerSubPage = 15 ' 6

        Erase SubClAr

        SubClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        Centr = prn_width / 2
        ' LMArgin = 20
        SubClAr(1) = 40 : SubClAr(2) = 40 : SubClAr(3) = 70
        SubClAr(4) = Centr - (SubClAr(1) + SubClAr(2) + SubClAr(3))

        TxtHgt = 19  ' e.Graphics.MeasureString("A", pFont).Height  ' 20



        Try
            If prn_HdDt.Rows.Count > 0 Then

                NoofDets = 0

                CurY = CurY + TxtHgt
                e.Graphics.DrawLine(Pens.Black, LMArgin, CurY, LMArgin + prn_width, CurY)

                CurY = CurY + 5
                Common_Procedures.Print_To_PrintDocument(e, "Sl", LMArgin, CurY, 2, SubClAr(1), pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Bale No", LMArgin + SubClAr(1), CurY, 2, SubClAr(2), pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Meters", LMArgin + SubClAr(1) + SubClAr(2) + SubClAr(3) - 20, CurY, 1, SubClAr(3), pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Pcs", LMArgin + SubClAr(1) + SubClAr(2) + SubClAr(3), CurY, 2, SubClAr(4), pFont)

                Common_Procedures.Print_To_PrintDocument(e, "Sl", Centr + LMArgin, CurY, 2, SubClAr(1), pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Bale No", Centr + LMArgin + SubClAr(1), CurY, 2, SubClAr(2), pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Meters", Centr + LMArgin + SubClAr(1) + SubClAr(2) + SubClAr(3) - 20, CurY, 1, SubClAr(3), pFont)
                Common_Procedures.Print_To_PrintDocument(e, "Pcs", Centr + LMArgin + SubClAr(1) + SubClAr(2) + SubClAr(3), CurY, 2, SubClAr(4), pFont)

                CurY = CurY + TxtHgt
                e.Graphics.DrawLine(Pens.Black, LMArgin, CurY, LMArgin + prn_width, CurY)


                prn_SubDetIndx = 0


                CurY = CurY - 10
                CurY_Temp = CurY
                Pos = 0
                If prn_DetDt_sub.Rows.Count > 0 Then

                    Do While prn_SubDetIndx <= prn_DetDt_sub.Rows.Count - 1

                        If NoofItems_PerSubPage = NoofDets Then
                            Pos = Centr
                            CurY_Temp = CurY

                        End If

                        NoofDets = NoofDets + 1

                        SNo = SNo + 1
                        CurY_Temp = CurY_Temp + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, SNo, Pos + LMArgin + 15, CurY_Temp, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt_sub.Rows(prn_SubDetIndx).Item("BaleNo").ToString), Pos + LMArgin + SubClAr(1) + 10, CurY_Temp, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt_sub.Rows(prn_SubDetIndx).Item("Mtrs").ToString), "#########0.00"), Pos + LMArgin + SubClAr(1) + SubClAr(2) + SubClAr(3) - 20, CurY_Temp, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt_sub.Rows(prn_SubDetIndx).Item("NoOfPcs").ToString), "#########0.00"), Pos + LMArgin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY_Temp, 1, 0, pFont)


                        Ttl_Bale = Ttl_Bale + 1
                        Ttl_Pcs = Ttl_Pcs + Val(prn_DetDt_sub.Rows(prn_SubDetIndx).Item("Pcs").ToString)
                        Ttl_Mtrs = Ttl_Mtrs + Val(prn_DetDt_sub.Rows(prn_SubDetIndx).Item("Meters").ToString)


                        prn_SubDetIndx = prn_SubDetIndx + 1


                    Loop



                    For i = 1 To NoofItems_PerSubPage
                        CurY = CurY + TxtHgt
                    Next

                    CurY = CurY + TxtHgt
                    e.Graphics.DrawLine(Pens.Black, LMArgin, CurY, LMArgin + prn_width, CurY)
                    CurY = CurY + 10
                    Common_Procedures.Print_To_PrintDocument(e, "Total Bales : " & Ttl_Bale, LMArgin + 20, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "Total Pcs   : " & Ttl_Pcs, LMArgin + 120, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "Total Mtrs  : " & Format(Ttl_Mtrs, "#########0.00"), LMArgin + prn_width - 10, CurY, 1, 0, pFont)


                End If


            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try


    End Sub
    Private Sub Printing_Format19_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim C1 As Single, W1, W2, W3 As Single, S1, S2 As Single
        Dim Cmp_Name, Desc As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim CurY1 As Single = 0

        PageNo = PageNo + 1

        CurY = TMargin


        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        'If Trim(prn_InpOpts) <> "" Then
        '    If prn_Count <= Len(Trim(prn_InpOpts)) Then

        '        S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

        '        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
        '            If Val(S) = 1 Then
        '                prn_OriDupTri = "ORIGINAL"
        '            ElseIf Val(S) = 2 Then
        '                prn_OriDupTri = "TRANSPORT COPY"
        '            ElseIf Val(S) = 3 Then
        '                prn_OriDupTri = "TRIPLICATE"
        '            ElseIf Val(S) = 4 Then
        '                prn_OriDupTri = "EXTRA COPY"
        '            Else
        '                If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
        '                    prn_OriDupTri = Trim(prn_InpOpts)
        '                End If
        '            End If

        '        Else
        '            If Val(S) = 1 Then
        '                prn_OriDupTri = "ORIGINAL"
        '            ElseIf Val(S) = 2 Then
        '                prn_OriDupTri = "DUPLICATE"
        '            ElseIf Val(S) = 3 Then
        '                prn_OriDupTri = "TRIPLICATE"
        '            ElseIf Val(S) = 4 Then
        '                prn_OriDupTri = "EXTRA COPY"
        '            Else
        '                If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
        '                    prn_OriDupTri = Trim(prn_InpOpts)
        '                End If
        '            End If

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
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_EMail = ""

        Desc = prn_HdDt.Rows(0).Item("Company_Description").ToString

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_ShortName").ToString

        CurY = CurY + TxtHgt + 10

        p1Font = New Font("Calibri", 11, FontStyle.Bold Or FontStyle.Underline)
        Common_Procedures.Print_To_PrintDocument(e, "ESTIMATE", LMargin, CurY - TxtHgt - 5, 2, PrintWidth, p1Font)

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height



        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Desc, LMargin, CurY, 2, PrintWidth, pFont)



        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = ClAr(1)
            W1 = e.Graphics.MeasureString("INVOICE DA: ", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width
            W2 = e.Graphics.MeasureString("Despatch To: ", pFont).Width
            S2 = e.Graphics.MeasureString("Sent Through  : ", pFont).Width


            CurY1 = CurY

            '---Left Side
            CurY = CurY + 10

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            If Trim(prn_HdDt.Rows(0).Item("Dc_Date").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "DC DATE : " & prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + C1 + 100, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " TIN : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If


            '----Right Side
            CurY1 = CurY1 + 10

            p1Font = New Font("Calibri", 10, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + C1 + 10, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + 30, CurY1, 0, 0, pFont)
            If prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString & "-" & prn_HdDt.Rows(0).Item("Processed_Fabric_Sales_Invoice_No").ToString, LMargin + C1 + 70, CurY1, 1, 0, p1Font)
            Else
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Processed_Fabric_Sales_Invoice_No").ToString, LMargin + C1 + 70, CurY1, 1, 0, p1Font)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + W1 + 30, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 70, CurY1, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Processed_Fabric_Sales_Invoice_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 80, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY1, PageWidth, CurY1)
            CurY1 = CurY1 + 10

            If Trim(prn_HdDt.Rows(0).Item("TransportName").ToString) <> "" Then
                'CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Transport ", LMargin + C1 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + C1 + W2 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + C1 + W2 + 20, CurY1, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("Lr_No").ToString) <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Lr.No  ", LMargin + C1 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + C1 + W2 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lr_No").ToString, LMargin + C1 + W2 + 20, CurY1, 0, 0, pFont)
                If Trim(prn_HdDt.Rows(0).Item("Lr_No").ToString) <> "" And Trim(prn_HdDt.Rows(0).Item("Lr_Date").ToString) <> "" Then
                    W3 = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Lr_No").ToString, pFont).Width
                    Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Lr_Date").ToString, LMargin + C1 + W2 + W3 + 20, CurY1, 0, 0, pFont)
                End If
            End If

            If Trim(prn_HdDt.Rows(0).Item("Agent_Name").ToString) <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Agent Name ", LMargin + C1 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + C1 + W2 + 10, CurY1, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + C1 + W2 + 20, CurY1, 0, 0, pFont)
            End If

            'If Trim(prn_DetDt.Rows(0).Item("Bales_Nos").ToString) <> "" Then
            '    CurY1 = CurY1 + TxtHgt
            '    Common_Procedures.Print_To_PrintDocument(e, "Bale Nos", LMargin + C1 + 10, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + C1 + W2 + 10, CurY1, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Bales_Nos").ToString, LMargin + C1 + W2 + 20, CurY1, 0, 0, pFont)
            'End If


            If CurY1 > CurY Then CurY = CurY1


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
            LnAr(3) = CurY

            CurY = CurY + 10
            Common_Procedures.Print_To_PrintDocument(e, "Particulars", LMargin, CurY, 2, ClAr(1), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Linear", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Meters", LMargin + ClAr(1), CurY + TxtHgt - 3, 2, ClAr(2), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Rate", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Rs. P", LMargin + ClAr(1) + ClAr(2), CurY + TxtHgt - 3, 2, ClAr(3), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)

            CurY = CurY + TxtHgt + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY


        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format19_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Cmp_Name As String
        Dim Lf1 As Single = 0
        Dim BmsInWrds As String
        Dim vprn_BlNos As String = ""
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""

        Try

            'For I = NoofDets + 1 To NoofItems_PerPage

            '    CurY = CurY + TxtHgt

            '    prn_DetIndx = prn_DetIndx + 1

            'Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY


            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))


            p1Font = New Font("Calibri", 8, FontStyle.Bold)
            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + 10, CurY, 0, ClAr(2) + ClAr(3), p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, p1Font)


            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))




            ' e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))

            CurY = CurY + 10

            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
            BmsInWrds = Replace(Trim(BmsInWrds), "", "")

            Common_Procedures.Print_To_PrintDocument(e, "Rupees  : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY


            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If

            CurY = CurY + 10
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_ShortName").ToString
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt


            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Proprietor / Manager.", PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
   
    End Sub

    Private Sub Printing_Format3(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        Dim FldLessPerc As Single = 0
        Dim FldLessMtr As Single = 0
        Dim fmtr As Single = 0
        Dim FldPerc As Single = 0
        Dim strFldPerCM As String = ""

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
            .Left = 20 ' 30 
            .Right = 40
            .Top = 30 ' 50 
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

        NoofItems_PerPage = 5 ' 6

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = 45 : ClAr(2) = 260 : ClAr(3) = 80 : ClAr(4) = 150 : ClAr(5) = 85 ': ClAr(6) = 80
        ClAr(6) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5))

        'ClAr(1) = Val(50) : ClAr(2) = 240 : ClAr(3) = 80 : ClAr(4) = 70 : ClAr(5) = 100 : ClAr(6) = 80
        'ClAr(7) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6))

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

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format3_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 10, CurY, 0, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString) <> "" Then
                            ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString)
                        Else
                            ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Name").ToString)
                        End If

                        ItmNm2 = ""
                        If Len(ItmNm1) > 35 Then
                            For I = 35 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 35
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        '   FldPerc = Val(prn_DetDt.Rows(prn_DetIndx).Item("Fold_Perc").ToString)
                        '   If Val(FldPerc) = 0 Then FldPerc = 100


                        ' If Val(FldPerc) = 0 Or Val(FldPerc) = 100 Or Trim(prn_HdDt.Rows(0).Item("FoldingRate_Status").ToString) = 1 Then
                        CurY = CurY + TxtHgt + 10
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("No_of_Rolls").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("No_of_Rolls").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 20, CurY, 1, 0, pFont)
                        End If

                        ' strFldPerCM = Val(FldPerc) & " cm"
                        ' Common_Procedures.Print_To_PrintDocument(e, strFldPerCM, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 13, CurY, 0, 0, pFont)
                        ' Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 60, CurY, 0, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate_Meter").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

                        'Else

                        '    CurY = CurY + TxtHgt + 10
                        '    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                        '    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        '    If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bales").ToString) <> 0 Then
                        '        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bales").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 20, CurY, 1, 0, pFont)
                        '    End If

                        '    strFldPerCM = Val(FldPerc) & " cm"
                        '    Common_Procedures.Print_To_PrintDocument(e, strFldPerCM, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 13, CurY, 0, 0, pFont)
                        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 60, CurY, 0, 0, pFont)

                        '    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)

                        '    fmt = ((100 - Val(.Rows(CurRow).Cells(3).Value)) / 100) * Val(.Rows(CurRow).Cells(7).Value)
                        '    fmt = Format(Math.Abs(Val(fmt)), "######0.00")
                        '    fmt = Common_Procedures.Meter_RoundOff(fmt)
                        '    If Trim(ItmNm2) <> "" Then
                        '        CurY = CurY + TxtHgt - 5
                        '        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        '        NoofDets = NoofDets + 1
                        '    End If

                        '    FldLessPerc = 100 - Val(FldPerc)

                        '    FldLessMtr = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString) * FldLessPerc / 100, "#########0.00")

                        '    FldLessMtr = Math.Abs(Val(FldLessMtr))

                        '    FldLessMtr = Common_Procedures.Meter_RoundOff(FldLessMtr)

                        '    CurY = CurY + TxtHgt
                        '    If Val(FldLessPerc) > 0 Then
                        '        Common_Procedures.Print_To_PrintDocument(e, "Folding Less", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        '        Common_Procedures.Print_To_PrintDocument(e, Val(FldLessPerc) & "%  Folding Less", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        '    Else
                        '        Common_Procedures.Print_To_PrintDocument(e, "Folding Add", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        '        Common_Procedures.Print_To_PrintDocument(e, Val(FldLessPerc) & "%  Folding Add", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        '    End If

                        '    strFldPerCM = Val(FldLessPerc) & " cm"
                        '    Common_Procedures.Print_To_PrintDocument(e, strFldPerCM, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 13, CurY, 0, 0, pFont)
                        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 60, CurY, 0, 0, pFont)

                        '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(FldLessMtr), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)

                        '    CurY = CurY + TxtHgt
                        '    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY)

                        '    If Val(FldLessPerc) > 0 Then
                        '        fmtr = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString) - Val(FldLessMtr), "#########0.00")
                        '    Else
                        '        fmtr = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString) + Val(FldLessMtr), "#########0.00")
                        '    End If

                        '    strFldPerCM = "100 cm"
                        '    Common_Procedures.Print_To_PrintDocument(e, strFldPerCM, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 13, CurY, 0, 0, pFont)
                        '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 60, CurY, 0, 0, pFont)

                        '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(fmtr), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                        '    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        '    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

                        'End If

                        NoofDets = NoofDets + 1

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                    'If Trim(UCase(Common_Procedures.settings.CompanyName)) = "1009" Or Trim(UCase(Common_Procedures.settings.CompanyName)) = "1018" Then
                    '    CurY = CurY + TxtHgt
                    '    CurY = CurY + TxtHgt - 5
                    '    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                    '    NoofDets = NoofDets + 2
                    'End If

                End If

                Printing_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

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

    Private Sub Printing_Format3_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim C1 As Single, W1, W2, W3 As Single, S1, S2 As Single
        Dim Cmp_Name, Desc As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim S As String

        PageNo = PageNo + 1

        CurY = TMargin

        'da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Ledger_Name, d.Company_Description as Transport_Name from Processed_Fabric_Sales_Invoice_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Transport_IdNo LEFT OUTER JOIN Company_Head d ON d.Company_IdNo = a.Company_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Processed_Fabric_Sales_Invoice_Code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
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

            End If
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
            p1Font = New Font("Calibri", 15, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY - TxtHgt - 5, 2, PrintWidth, p1Font)
        End If
        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY
        Desc = ""
        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_EMail = ""

        Desc = prn_HdDt.Rows(0).Item("Company_Description").ToString
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE : " & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(0).Item("Company_CstNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_EMail = "MAIL ID : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
        End If

        p1Font = New Font("Calibri", 15, FontStyle.Bold)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1018" Then '---- M.K Textiles (Palladam)
            p1Font = New Font("Calibri", 15, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
        End If
        CurY = CurY + TxtHgt
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MK, Drawing.Image), LMargin + 20, CurY, 112, 80)
            'e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MK_2, Drawing.Image), LMargin + 20, CurY, 115, 80)
            'e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MK, Drawing.Image), LMargin + 20, CurY, 75, 75)
        End If

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
        Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, LMargin, CurY, 2, PrintWidth, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)


        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
            W1 = e.Graphics.MeasureString("INVOICE DATE  : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width
            W2 = e.Graphics.MeasureString("Despatch To   : ", pFont).Width
            S2 = e.Graphics.MeasureString("Sent Through  : ", pFont).Width


            CurY = CurY + 10
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "INVOICE NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            If prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString & "-" & prn_HdDt.Rows(0).Item("Processed_Fabric_Sales_Invoice_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
            Else
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Processed_Fabric_Sales_Invoice_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)
            End If


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "INVOICE DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Processed_Fabric_Sales_Invoice_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Dc_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "DC NO : " & prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + C1 + 10, CurY, 0, 0, pFont)
            End If
            If Trim(prn_HdDt.Rows(0).Item("Dc_Date").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "DC DATE : " & prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + C1 + 100, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " TIN : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
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


            Common_Procedures.Print_To_PrintDocument(e, "Transport ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + C1 + W2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + C1 + W2 + 30, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Order No ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_OrderNo").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Lr.No  ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + C1 + W2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lr_No").ToString, LMargin + C1 + W2 + 30, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Lr_No").ToString) <> "" And Trim(prn_HdDt.Rows(0).Item("Lr_Date").ToString) <> "" Then
                W3 = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Lr_No").ToString, pFont).Width
                Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Lr_Date").ToString, LMargin + C1 + W2 + W3 + 40, CurY, 0, 0, pFont)
            End If


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Lc No ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lc_No").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Lc_No").ToString) <> "" And Trim(prn_HdDt.Rows(0).Item("Lc_Date").ToString) <> "" Then
                W3 = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Lc_No").ToString, pFont).Width
                Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Lc_Date").ToString, LMargin + S2 + W3 + 35, CurY, 0, 0, pFont)
            End If

            'Common_Procedures.Print_To_PrintDocument(e, "Transport ", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Despatch To", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Despatch_To").ToString, LMargin + C1 + W2 + 30, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Sent Through ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + S2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Through_Name").ToString, LMargin + S2 + 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Delivery_Address1").ToString, LMargin + C1 + W2 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Delivery_Address2").ToString, LMargin + C1 + W2 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

            CurY = CurY + 10
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "BALES\", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BUNDLES", LMargin + ClAr(1) + ClAr(2), CurY + TxtHgt - 3, 2, ClAr(3), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "METER", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + TxtHgt, 2, ClAr(5), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "RATE/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METER", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + TxtHgt - 3, 2, ClAr(5), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)

            CurY = CurY + TxtHgt + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            CurY = CurY + 10
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cloth_Details").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format3_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim rndoff As Single, TtAmt As Double
        Dim I As Integer
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim Cmp_Name As String
        Dim Lf1 As Single = 0
        Dim BmsInWrds As String
        Dim vprn_BlNos As String = ""
        Dim BLNo2 As String = ""
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""

        Try

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt

                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt + 50
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            CurY += 10

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

            'vprn_BlNos = ""
            'For I = 0 To prn_DetDt.Rows.Count - 1
            '    If Trim(prn_DetDt.Rows(I).Item("Bales_Nos").ToString) <> "" Then
            '        vprn_BlNos = Trim(vprn_BlNos) & IIf(Trim(vprn_BlNos) <> "", ", ", "") & prn_DetDt.Rows(I).Item("Bales_Nos").ToString
            '    End If
            'Next

            'BLNo1 = Trim(vprn_BlNos)
            'BLNo2 = ""
            'If Len(BLNo1) > 30 Then
            '    For I = 30 To 1 Step -1
            '        If Mid$(Trim(BLNo1), I, 1) = " " Or Mid$(Trim(BLNo1), I, 1) = "," Or Mid$(Trim(BLNo1), I, 1) = "." Or Mid$(Trim(BLNo1), I, 1) = "-" Or Mid$(Trim(BLNo1), I, 1) = "/" Or Mid$(Trim(BLNo1), I, 1) = "_" Or Mid$(Trim(BLNo1), I, 1) = "(" Or Mid$(Trim(BLNo1), I, 1) = ")" Or Mid$(Trim(BLNo1), I, 1) = "\" Or Mid$(Trim(BLNo1), I, 1) = "[" Or Mid$(Trim(BLNo1), I, 1) = "]" Or Mid$(Trim(BLNo1), I, 1) = "{" Or Mid$(Trim(BLNo1), I, 1) = "}" Then Exit For
            '    Next I
            '    If I = 0 Then I = 30
            '    BLNo2 = Microsoft.VisualBasic.Right(Trim(BLNo1), Len(BLNo1) - I)
            '    BLNo1 = Microsoft.VisualBasic.Left(Trim(BLNo1), I - 1)
            'End If

            'If Trim(BLNo1) <> "" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "Bale/Bundle No : " & BLNo1, LMargin + 10, CurY, 0, 0, pFont)
            'End If


            Lf1 = LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 50

            If Val(prn_HdDt.Rows(0).Item("Trade_Discount_Perc").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("TradeDisc_Name").ToString) & "  " & Trim(prn_HdDt.Rows(0).Item("Trade_Discount").ToString) & "%", Lf1, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 25, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Trade_Discount_Perc").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Trim(BLNo2) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, BLNo2, LMargin + 10, CurY, 0, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Cash_Discount_Perc").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("CashDisc_Name").ToString) & "  " & Trim(prn_HdDt.Rows(0).Item("Cash_Discount").ToString) & "%", Lf1, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 25, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Cash_Discount_Perc").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Bale_Weight").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Bale/Bundle Weight : " & Trim(prn_HdDt.Rows(0).Item("Bale_Weight").ToString), LMargin + 10, CurY, 0, 0, pFont)
            End If
            If Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Packing_Name").ToString), Lf1, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 25, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Packing_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY, 0, 0, p1Font)
            If Val(prn_HdDt.Rows(0).Item("Freight").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Freight_Name").ToString), Lf1, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 25, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Freight").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY, 0, 0, p1Font)
            If Val(prn_HdDt.Rows(0).Item("Insurance").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Insurance_Name").ToString), Lf1, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 25, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Insurance").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            End If

            TtAmt = Val(prn_HdDt.Rows(0).Item("total_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Freight").ToString) + Val(prn_HdDt.Rows(0).Item("Insurance").ToString) + Val(prn_HdDt.Rows(0).Item("Packing_amount").ToString) - Val(prn_HdDt.Rows(0).Item("Trade_Discount_Perc").ToString) - Val(prn_HdDt.Rows(0).Item("Cash_Discount_Perc").ToString)

            rndoff = 0
            rndoff = Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) - Val(TtAmt)

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 10, CurY, 0, 0, p1Font)
            If Val(rndoff) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Round Off", Lf1, CurY, 0, 0, pFont)
                If Val(rndoff) >= 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 25, CurY, 0, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 25, CurY, 0, 0, pFont)
                End If
                Common_Procedures.Print_To_PrintDocument(e, " " & Format(Math.Abs(Val(rndoff)), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY, 0, 0, p1Font)
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)
            LnAr(8) = CurY

            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "Net Amount", Lf1, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, p1Font)
            If Val(prn_HdDt.Rows(0).Item("Gr_Time").ToString) <> 0 Then
                p1Font = New Font("Calibri", 10, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "Due Date : " & Trim(prn_HdDt.Rows(0).Item("Gr_Time").ToString) & " Days " & "(" & Trim(prn_HdDt.Rows(0).Item("Gr_Date").ToString) & ")", LMargin + 10, CurY, 0, 0, p1Font)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))

            CurY = CurY + 10

            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
            BmsInWrds = Replace(Trim(BmsInWrds), "", "")

            Common_Procedures.Print_To_PrintDocument(e, "Rupees  : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + 10
            p1Font = New Font("Calibri", 12, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "GOODS CLEARED UNDER EXEMPTION NOTIFICATION NO 30/2004 DT 09.07.2004 ", LMargin, CurY, 2, PageWidth, pFont)

            CurY = CurY + TxtHgt + 2
            p1Font = New Font("Calibri", 12, FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)


            CurY = CurY + TxtHgt + 5
            If Val(prn_HdDt.Rows(0).Item("Gr_Time").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Overdue Interest will be charged at 24% from The  " & Trim(prn_HdDt.Rows(0).Item("Gr_Date").ToString), LMargin + 10, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Overdue Interest will be charged at 24% from The invoice date ", LMargin + 10, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "We are not responsible for any loss or damage in transit", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "We will not accept any claim after processing of goods", LMargin + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Subject to Tirupur jurisdiction ", LMargin + 10, CurY, 0, 0, pFont)


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

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub txt_CGST_Perc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        NetAmount_Calculation()
    End Sub

    Private Sub txt_SGST_Perc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        NetAmount_Calculation()
    End Sub
    Private Sub cbo_DeliveryTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DeliveryTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "Ledger_AlaisHead", "Ledger_DisplayName", " ( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) ) or Show_In_All_Entry = 1) ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_DeliveryTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DeliveryTo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_DeliveryTo, cbo_OnAcc, cbo_Transport, "Ledger_AlaisHead", "Ledger_DisplayName", " ( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) ) or Show_In_All_Entry = 1) ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_DeliveryTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DeliveryTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_DeliveryTo, cbo_Transport, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) ) or Show_In_All_Entry = 1)", "(Ledger_IdNo = 0)")
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

    Private Sub cbo_TransportMode_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TransportMode.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "ClothSales_Invoice_Head", "Transport_Mode", "", "")
    End Sub

    Private Sub cbo_TransportMode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TransportMode.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_TransportMode, txt_DateAndTimeOFSupply, txt_OrderNo, "ClothSales_Invoice_Head", "Transport_Mode", "", "")
    End Sub

    Private Sub cbo_TransportMode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TransportMode.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_TransportMode, txt_OrderNo, "ClothSales_Invoice_Head", "Transport_Mode", "", "", False)
    End Sub
    Private Sub Printing_GST_Format3(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        Dim VechDesc1 As String = "", VechDesc2 As String = ""
        Dim vNoofHsnCodes As Integer = 0
        Dim vLine_Pen As Pen


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1016" Then '---- Rajendra Textiles (Somanur)
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    e.PageSettings.PaperSize = ps
                    Exit For
                End If
            Next

        Else
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

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1016" Then '---- Rajendra Textiles (Somanur)
            With PrintDocument1.DefaultPageSettings.Margins
                .Left = 10
                .Right = 65
                .Top = 50 ' 60
                .Bottom = 40
                LMargin = .Left
                RMargin = .Right
                TMargin = .Top
                BMargin = .Bottom
            End With

        Else
            With PrintDocument1.DefaultPageSettings.Margins
                .Left = 20 ' 40
                .Right = 45
                .Top = 20 '40 '50 ' 60
                .Bottom = 40
                LMargin = .Left
                RMargin = .Right
                TMargin = .Top
                BMargin = .Bottom
            End With

        End If

        pFont = New Font("Calibri", 9, FontStyle.Bold)
        'pFont = New Font("Calibri", 10, FontStyle.Regular)

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

        NoofItems_PerPage = 7 ' 4 

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = 30 : ClAr(2) = 220 : ClAr(3) = 80 : ClAr(4) = 45 : ClAr(5) = 50 : ClAr(6) = 50 : ClAr(7) = 85 : ClAr(8) = 80
        ClAr(9) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8))

        'ClAr(1) = 30 : ClAr(2) = 210 : ClAr(3) = 80 : ClAr(4) = 50 : ClAr(5) = 50 : ClAr(6) = 50 : ClAr(7) = 80 : ClAr(8) = 80
        'ClAr(9) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8))

        TxtHgt = e.Graphics.MeasureString("A", pFont).Height
        TxtHgt = 16.65 ' 17.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        vLine_Pen = New Pen(Color.Black, 2)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                'If Val(prn_HdDt.Rows(0).Item("Trade_Discount_Perc").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                'If Val(prn_HdDt.Rows(0).Item("Cash_Discount_Perc").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                'If Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                'If Val(prn_HdDt.Rows(0).Item("Freight").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1
                'If Val(prn_HdDt.Rows(0).Item("Insurance").ToString) = 0 Then NoofItems_PerPage = NoofItems_PerPage + 1

                vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)

                If vNoofHsnCodes = 0 Then
                    NoofItems_PerPage = NoofItems_PerPage + 5
                Else
                    If vNoofHsnCodes > 1 Then NoofItems_PerPage = NoofItems_PerPage - (vNoofHsnCodes - 1)
                End If

                Printing_GST_Format3_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr, vLine_Pen)

                If Trim(prn_HdDt.Rows(0).Item("Lc_No").ToString) <> "" Then NoofItems_PerPage = NoofItems_PerPage - 1

                NoofDets = 0
                CurY = CurY - 5
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

                        If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString) <> "" Then
                            ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString)
                        Else
                            ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Name").ToString)
                        End If

                        ItmNm2 = ""
                        If Len(ItmNm1) > 40 Then
                            For I = 40 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 40
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If


                        CurY = CurY + TxtHgt + 5
                        NoofDets = NoofDets + 1


                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("HSN_Code").ToString), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("GST_Percentage").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)

                        'If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bales").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("No_of_Rolls").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)
                        'End If

                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)
                        End If

                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate_Meter").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)



                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_GST_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True, vLine_Pen)

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

    Private Sub Printing_GST_Format3_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByRef NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal vLine_Pen As Pen)
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

        'p1Font = New Font("Calibri", 14, FontStyle.Bold)
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1018" Then '---- M.K Textiles (Palladam)
        '    p1Font = New Font("Calibri", 12, FontStyle.Bold)
        '    Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
        'End If

        CurY = CurY + TxtHgt
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)
            If Val(lbl_Company.Tag) = 1 Then
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_MK, Drawing.Image), LMargin + 20, CurY, 112, 80)

            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- Logu textile
            If InStr(1, Trim(UCase(Cmp_Name)), "GANAPATHY") > 0 Or InStr(1, Trim(UCase(Cmp_Name)), "GANAPATHI") > 0 Then                                    '---- Ganapathy Spinning textile
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.GSM_LOGO, Drawing.Image), LMargin + 20, CurY, 112, 80)
            ElseIf InStr(1, Trim(UCase(Cmp_Name)), "LOGU") > 0 Or InStr(1, Trim(UCase(Cmp_Name)), "LOGA") > 0 Then                                          '---- Logu textile
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Company_Logo_LogaTex, Drawing.Image), LMargin + 20, CurY, 112, 80)
            End If

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1098" Then '---- Bannari amman textiles
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.REVISED_LOGO_7___2_, Drawing.Image), LMargin + 20, CurY - 10, 130, 110)

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Then '---- m.s textiles
            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.ms_logo_2, Drawing.Image), LMargin + 20, CurY - 10, 130, 110)

        End If

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

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then '---- M.K Textiles (Palladam)

            If Cmp_StateNm <> "" Then
                CurY = CurY + TxtHgt - 1
                Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm & "  " & Cmp_StateCode, LMargin, CurY, 2, PrintWidth, pFont)
            End If
            If Cmp_EMail <> "" Then
                CurY = CurY + TxtHgt - 1
                Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, LMargin, CurY, 2, PrintWidth, pFont)
            End If
            If Cmp_GSTIN_No <> "" Then
                CurY = CurY + TxtHgt - 1
                p1Font = New Font("Calibri", 10, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_Cap & Cmp_GSTIN_No, LMargin, CurY, 2, PrintWidth, p1Font)
            End If
            If Cmp_PhNo <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin + 10, CurY, 0, 0, pFont)
            End If

        Else
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


        End If

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
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Invoice_PrefixNo").ToString & "-" & prn_HdDt.Rows(0).Item("Processed_Fabric_Sales_Invoice_NO").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font)
            Else
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Processed_Fabric_Sales_Invoice_NO").ToString, LMargin + W3 + 30, CurY, 0, 0, p1Font)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "REVERSE CHARGE (YES/NO)", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S3 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + C1 + S3 + 30, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "INVOICE DATE", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W3 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Processed_Fabric_Sales_Invoice_Date").ToString), "dd-MM-yyyy").ToString, LMargin + W3 + 30, CurY, 0, 0, pFont)

            If Trim(prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "ELECTRONIC REF.NO ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S3 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Electronic_Reference_No").ToString, LMargin + C1 + S3 + 30, CurY, 0, 0, pFont)
            End If

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
            Common_Procedures.Print_To_PrintDocument(e, "M/s. " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerName").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, p1Font)

            CurY2 = CurY2 + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress1").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)

            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString) <> "" Then
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress2").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString) <> "" Then
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress3").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4").ToString) <> "" Then
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_LedgerAddress4").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString) <> "" Then
                CurY2 = CurY2 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString & "  CODE : " & prn_HdDt.Rows(0).Item("DeliveryTo_State_Code").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            End If

            CurY2 = CurY2 + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "GSTIN : " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, LMargin + C1 + S1 + 10, CurY2, 0, 0, pFont)
            End If
            If Trim(prn_HdDt.Rows(0).Item("DeliveryTo_PanNo").ToString) <> "" Then
                strWidth = e.Graphics.MeasureString("GSTIN : " & prn_HdDt.Rows(0).Item("DeliveryTo_LedgerGSTinNo").ToString, pFont).Width
                CurX = LMargin + C1 + S1 + 10 + strWidth
                Common_Procedures.Print_To_PrintDocument(e, "      PAN : " & prn_HdDt.Rows(0).Item("DeliveryTo_PanNo").ToString, CurX, CurY2, 0, PrintWidth, pFont)
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

            Common_Procedures.Print_To_PrintDocument(e, "TRANSPORTATION MODE", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Mode").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "ORDER NO", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_OrderNo").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Party_OrderDate").ToString) <> "" Then
                strWidth = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Party_OrderNo").ToString, pFont).Width
                Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Party_OrderDate").ToString, LMargin + W2 + strWidth + 60, CurY, 0, 0, pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "VEHICLE NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "DC NO", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_No").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Dc_Date").ToString) <> "" Then
                strWidth = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Dc_No").ToString, pFont).Width
                Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Dc_Date").ToString, LMargin + strWidth + W1 + 60, CurY, 0, 0, pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT NAME", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "LR NO.", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lr_No").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("Lr_Date").ToString) <> "" Then
                strWidth = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Lr_No").ToString, pFont).Width
                Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Lr_Date").ToString, LMargin + W2 + strWidth + 60, CurY, 0, 0, pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "DATE & TIME OF SUPPLY", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Date_And_Time_Of_Supply").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "DOCUMENT THROUGH", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Through_Name").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "PLACE OF SUPPLY", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + S2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("DeliveryTo_State_Name").ToString, LMargin + C1 + S2 + 30, CurY, 0, 0, pFont)


            If Trim(prn_HdDt.Rows(0).Item("Lc_No").ToString) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "LC NO", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Lc_No").ToString, LMargin + W2 + 30, CurY, 0, 0, pFont)
                If Trim(prn_HdDt.Rows(0).Item("Lc_Date").ToString) <> "" Then
                    strWidth = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Lc_No").ToString, pFont).Width
                    Common_Procedures.Print_To_PrintDocument(e, "Date : " & prn_HdDt.Rows(0).Item("Lc_Date").ToString, LMargin + strWidth + W2 + 60, CurY, 0, 0, pFont)
                End If
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(vLine_Pen, LMargin + C1, CurY, LMargin + C1, LnAr(3))
            LnAr(4) = CurY


            CurY = CurY + 10
            Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY + (TxtHgt \ 2), 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PRODUCT DESCRIPTION", LMargin + ClAr(1), CurY + (TxtHgt \ 2), 2, ClAr(2), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "HSN", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CODE", LMargin + ClAr(1) + ClAr(2), CurY + TxtHgt, 2, ClAr(3), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "GST", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY + TxtHgt, 2, ClAr(4), pFont)

            'If Trim(prn_HdDt.Rows(0).Item("Packing_Type").ToString) = "ROLL" Then
            '    Common_Procedures.Print_To_PrintDocument(e, "ROLLS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + (TxtHgt \ 2), 2, ClAr(5), pFont)
            'Else
            Common_Procedures.Print_To_PrintDocument(e, "BALES", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + (TxtHgt \ 2), 2, ClAr(5), pFont)
            'End If
            Common_Procedures.Print_To_PrintDocument(e, "NO.OF", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + TxtHgt, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)

        
                Common_Procedures.Print_To_PrintDocument(e, "METER", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + TxtHgt, 2, ClAr(7), pFont)
                Common_Procedures.Print_To_PrintDocument(e, "RATE\", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
                Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY + TxtHgt, 2, ClAr(8), pFont)



            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY + (TxtHgt \ 2), 2, ClAr(9), pFont)

            CurY = CurY + TxtHgt + 20
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            CurY = CurY + 10
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Then '---- M.S Textiles (Tirupur)
            End If
            p1Font = New Font("Calibri", 8, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cloth_Details").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, p1Font)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_GST_Format3_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean, ByVal vLine_Pen As Pen)
        Dim p1Font As Font, p2Font As Font, p3Font As Font
        Dim rndoff As Double, TtAmt As Double
        Dim I As Integer
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim Cmp_Name As String
        Dim W1 As Single = 0
        Dim BmsInWrds As String
        Dim vprn_BlNos As String = ""
        Dim BLNo1 As String, BLNo2 As String, BLNo3 As String, BLNo4 As String, BLNo5 As String
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim CurY1 As Single = 0
        Dim SubClAr(15) As Single
        Dim vNoofHsnCodes As Integer = 0
        Dim vTaxPerc As Single = 0


        Try

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt

                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt + 7
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))



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

            End If


            'vprn_BlNos = ""
            'For I = 0 To prn_DetDt.Rows.Count - 1
            '    If Trim(prn_DetDt.Rows(I).Item("Bales_Nos").ToString) <> "" Then
            '        vprn_BlNos = Trim(vprn_BlNos) & IIf(Trim(vprn_BlNos) <> "", ", ", "") & prn_DetDt.Rows(I).Item("Bales_Nos").ToString
            '    End If
            'Next


            BLNo1 = Trim(vprn_BlNos)
            BLNo2 = ""
            BLNo3 = ""
            BLNo4 = ""
            BLNo5 = ""

            If Len(BLNo1) > 60 Then
                For I = 60 To 1 Step -1
                    If Mid$(Trim(BLNo1), I, 1) = " " Or Mid$(Trim(BLNo1), I, 1) = "," Or Mid$(Trim(BLNo1), I, 1) = "." Or Mid$(Trim(BLNo1), I, 1) = "-" Or Mid$(Trim(BLNo1), I, 1) = "/" Or Mid$(Trim(BLNo1), I, 1) = "_" Or Mid$(Trim(BLNo1), I, 1) = "(" Or Mid$(Trim(BLNo1), I, 1) = ")" Or Mid$(Trim(BLNo1), I, 1) = "\" Or Mid$(Trim(BLNo1), I, 1) = "[" Or Mid$(Trim(BLNo1), I, 1) = "]" Or Mid$(Trim(BLNo1), I, 1) = "{" Or Mid$(Trim(BLNo1), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 60
                BLNo2 = Microsoft.VisualBasic.Right(Trim(BLNo1), Len(BLNo1) - I)
                BLNo1 = Microsoft.VisualBasic.Left(Trim(BLNo1), I)
            End If

            If Len(BLNo2) > 60 Then
                For I = 60 To 1 Step -1
                    If Mid$(Trim(BLNo2), I, 1) = " " Or Mid$(Trim(BLNo2), I, 1) = "," Or Mid$(Trim(BLNo2), I, 1) = "." Or Mid$(Trim(BLNo2), I, 1) = "-" Or Mid$(Trim(BLNo2), I, 1) = "/" Or Mid$(Trim(BLNo2), I, 1) = "_" Or Mid$(Trim(BLNo2), I, 1) = "(" Or Mid$(Trim(BLNo2), I, 1) = ")" Or Mid$(Trim(BLNo2), I, 1) = "\" Or Mid$(Trim(BLNo2), I, 1) = "[" Or Mid$(Trim(BLNo2), I, 1) = "]" Or Mid$(Trim(BLNo2), I, 1) = "{" Or Mid$(Trim(BLNo2), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 60
                BLNo3 = Microsoft.VisualBasic.Right(Trim(BLNo2), Len(BLNo2) - I)
                BLNo2 = Microsoft.VisualBasic.Left(Trim(BLNo2), I)
            End If

            If Len(BLNo3) > 60 Then
                For I = 60 To 1 Step -1
                    If Mid$(Trim(BLNo3), I, 1) = " " Or Mid$(Trim(BLNo3), I, 1) = "," Or Mid$(Trim(BLNo3), I, 1) = "." Or Mid$(Trim(BLNo3), I, 1) = "-" Or Mid$(Trim(BLNo3), I, 1) = "/" Or Mid$(Trim(BLNo3), I, 1) = "_" Or Mid$(Trim(BLNo3), I, 1) = "(" Or Mid$(Trim(BLNo3), I, 1) = ")" Or Mid$(Trim(BLNo3), I, 1) = "\" Or Mid$(Trim(BLNo3), I, 1) = "[" Or Mid$(Trim(BLNo3), I, 1) = "]" Or Mid$(Trim(BLNo3), I, 1) = "{" Or Mid$(Trim(BLNo3), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 60
                BLNo4 = Microsoft.VisualBasic.Right(Trim(BLNo3), Len(BLNo3) - I)
                BLNo3 = Microsoft.VisualBasic.Left(Trim(BLNo3), I)
            End If

            If Len(BLNo4) > 60 Then
                For I = 60 To 1 Step -1
                    If Mid$(Trim(BLNo4), I, 1) = " " Or Mid$(Trim(BLNo4), I, 1) = "," Or Mid$(Trim(BLNo4), I, 1) = "." Or Mid$(Trim(BLNo4), I, 1) = "-" Or Mid$(Trim(BLNo4), I, 1) = "/" Or Mid$(Trim(BLNo4), I, 1) = "_" Or Mid$(Trim(BLNo4), I, 1) = "(" Or Mid$(Trim(BLNo4), I, 1) = ")" Or Mid$(Trim(BLNo4), I, 1) = "\" Or Mid$(Trim(BLNo4), I, 1) = "[" Or Mid$(Trim(BLNo4), I, 1) = "]" Or Mid$(Trim(BLNo4), I, 1) = "{" Or Mid$(Trim(BLNo4), I, 1) = "}" Then Exit For
                Next I
                If I = 0 Then I = 60
                BLNo5 = Microsoft.VisualBasic.Right(Trim(BLNo4), Len(BLNo4) - I)
                BLNo4 = Microsoft.VisualBasic.Left(Trim(BLNo4), I)
            End If


            '---Left Side
            CurY1 = CurY1 + 10
            'If Trim(prn_HdDt.Rows(0).Item("Packing_Type").ToString) = "ROLL" Then
            '    If Trim(BLNo1) <> "" Then
            '        Common_Procedures.Print_To_PrintDocument(e, "ROLL NO : " & BLNo1, LMargin + 10, CurY1, 0, 0, pFont)
            '    End If
            'Else
            '    If Trim(BLNo1) <> "" Then
            '        Common_Procedures.Print_To_PrintDocument(e, "BALE/BUNDLE NO : " & BLNo1, LMargin + 10, CurY1, 0, 0, pFont)
            '    End If
            'End If

            CurY1 = CurY1 + TxtHgt - 3
            Common_Procedures.Print_To_PrintDocument(e, BLNo2, LMargin + 30, CurY1, 0, 0, pFont)

            CurY1 = CurY1 + TxtHgt - 3
            Common_Procedures.Print_To_PrintDocument(e, BLNo3, LMargin + 30, CurY1, 0, 0, pFont)

            If Trim(BLNo4) <> "" Then
                CurY1 = CurY1 + TxtHgt - 3
                Common_Procedures.Print_To_PrintDocument(e, BLNo4, LMargin + 30, CurY1, 0, 0, pFont)
            End If
            If Trim(BLNo5) <> "" Then
                CurY1 = CurY1 + TxtHgt - 3
                Common_Procedures.Print_To_PrintDocument(e, BLNo5, LMargin + 30, CurY1, 0, 0, pFont)
            End If


            If Val(prn_HdDt.Rows(0).Item("Bale_Weight").ToString) <> 0 Then
                CurY1 = CurY1 + TxtHgt + 10
                If Trim(prn_HdDt.Rows(0).Item("Packing_Type").ToString) = "ROLL" Then
                    If Val(prn_HdDt.Rows(0).Item("Bale_Weight").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "Roll Weight : " & Trim(prn_HdDt.Rows(0).Item("Bale_Weight").ToString), LMargin + 10, CurY1, 0, 0, pFont)
                    End If
                Else
                    If Val(prn_HdDt.Rows(0).Item("Bale_Weight").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "Bale/Bundle Weight : " & Trim(prn_HdDt.Rows(0).Item("Bale_Weight").ToString), LMargin + 10, CurY1, 0, 0, pFont)
                    End If
                End If
            End If


            CurY1 = CurY1 + 10

            p3Font = New Font("Calibri", 10, FontStyle.Bold)
            If BankNm1 <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, BankNm1, LMargin + 10, CurY1, 0, 0, p3Font)
            End If
            If BankNm2 <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, BankNm2, LMargin + 10, CurY1, 0, 0, p3Font)
            End If
            If BankNm3 <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, BankNm3, LMargin + 10, CurY1, 0, 0, p3Font)
            End If
            If BankNm4 <> "" Then
                CurY1 = CurY1 + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, BankNm4, LMargin + 10, CurY1, 0, 0, p3Font)
            End If

            CurY1 = CurY1 + TxtHgt + 10
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1105" Then '---- Ganga Weaving (Dindugal)

                p1Font = New Font("Calibri", 10, FontStyle.Bold)
                If Val(prn_HdDt.Rows(0).Item("Gr_Time").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "DUE DATE : " & Trim(prn_HdDt.Rows(0).Item("Gr_Time").ToString) & " Days " & "(" & Trim(prn_HdDt.Rows(0).Item("Gr_Date").ToString) & ")", LMargin + 10, CurY1, 0, 0, p1Font)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "DUE DATE : IMMEDIATE", LMargin + 10, CurY1, 0, 0, p1Font)
                End If

            Else

                If Val(prn_HdDt.Rows(0).Item("Gr_Time").ToString) <> 0 Then
                    p1Font = New Font("Calibri", 10, FontStyle.Bold)
                    Common_Procedures.Print_To_PrintDocument(e, "DUE DATE : " & Trim(prn_HdDt.Rows(0).Item("Gr_Time").ToString) & " Days " & "(" & Trim(prn_HdDt.Rows(0).Item("Gr_Date").ToString) & ")", LMargin + 10, CurY1, 0, 0, p1Font)
                End If

            End If

            '---Right Side
            CurY = CurY - 5
            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Trade_Discount_Perc").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("TradeDisc_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 20, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Trade_Discount").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Trade_Discount_Perc").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Cash_Discount_Perc").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("CashDisc_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 20, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Cash_Discount").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Cash_Discount_Perc").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Packing_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 20, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Packing_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Freight").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Freight_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 20, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Freight").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
            End If

            If Val(prn_HdDt.Rows(0).Item("Insurance").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Insurance_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 20, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Insurance").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
            End If


            If Val(prn_HdDt.Rows(0).Item("Trade_Discount_Perc").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("Cash_Discount_Perc").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("Freight").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("Insurance").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
                CurY = CurY - 15
            End If

            vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString) <> 0 Then
                If Val(prn_HdDt.Rows(0).Item("Total_Taxable_Value").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "TAXABLE VALUE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 20, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Taxable_Value").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                End If
            End If


            '----Gst
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "CGST @ ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 20, CurY, 1, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, Val(vTaxPerc) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 0, 0, pFont)
            End If
            Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "SGST @ ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 20, CurY, 1, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, Val(vTaxPerc) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 0, 0, pFont)
            End If
            Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "IGST @ ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 20, CurY, 1, 0, pFont)
            If Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, Val(vTaxPerc) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 0, 0, pFont)
            End If
            Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)

            TtAmt = Format(Val(prn_HdDt.Rows(0).Item("total_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Freight").ToString) + Val(prn_HdDt.Rows(0).Item("Insurance").ToString) + Val(prn_HdDt.Rows(0).Item("Packing_amount").ToString) - Val(prn_HdDt.Rows(0).Item("Trade_Discount_Perc").ToString) - Val(prn_HdDt.Rows(0).Item("Cash_Discount_Perc").ToString), "#########0.00")

            rndoff = 0
            rndoff = Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) - Val(TtAmt)

            CurY = CurY + TxtHgt
            If Val(rndoff) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 20, CurY, 1, 0, pFont)
                If Val(rndoff) >= 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 20, CurY, 0, 0, pFont)
                End If
                Common_Procedures.Print_To_PrintDocument(e, " " & Format(Math.Abs(Val(rndoff)), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
            End If

            If CurY1 > CurY Then CurY = CurY1
            If CurY < 731 Then CurY = 731

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
            'LnAr(8) = CurY


            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "E & OE", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "NET AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, p1Font)


            'CurY = CurY + TxtHgt + 5
            'e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
            ''LnAr(8) = CurY

            'CurY = CurY + TxtHgt - 12
            'p1Font = New Font("Calibri", 9, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, "GST Payable on Reverse Charge", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, " 0.00", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            e.Graphics.DrawLine(vLine_Pen, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))

            CurY = CurY + 5
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
            BmsInWrds = Replace(Trim(BmsInWrds), "", "")

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- Logu textile
                BmsInWrds = Trim(UCase(BmsInWrds))
            End If

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Amount Chargeable(In Words)  : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY

            '=============GST SUMMARY============


            vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)
            If vNoofHsnCodes <> 0 Then
                Printing_GST_HSN_Details_Format3(e, EntryCode, TxtHgt, pFont, LMargin, PageWidth, PrintWidth, CurY, LnAr(10), vLine_Pen)
            End If




            '==========================

            CurY = CurY + TxtHgt - 15
            p1Font = New Font("Calibri", 9, FontStyle.Underline Or FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt

            p2Font = New Font("Webdings", 8, FontStyle.Bold)
            p1Font = New Font("Calibri", 8, FontStyle.Bold)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Then '---- Logu textile

                Common_Procedures.Print_To_PrintDocument(e, "Interest will be Charged at 24% P.A for the overdue payments from the Date of Invoice", LMargin + 10, CurY, 0, 0, p1Font)
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "We are not responsible for any delay , Loss Or Damage During the Transport", LMargin + 10, CurY, 0, 0, p1Font)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Quality Complaint Will be accepted only in Grey Stage for Fabrics and Cotton Yarn Stage for Yarns", LMargin + 10, CurY, 0, 0, p1Font)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Subject to Palladam jurisdiction Only", LMargin + 10, CurY, 0, 0, p1Font)

            Else

                '1
                If Val(prn_HdDt.Rows(0).Item("Gr_Time").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY, 0, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, "Overdue Interest will be charged at 24% from The  " & Trim(prn_HdDt.Rows(0).Item("Gr_Date").ToString), LMargin + 25, CurY, 0, 0, p1Font)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY, 0, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, "Overdue Interest will be charged at 24% from The invoice date. ", LMargin + 25, CurY, 0, 0, p1Font)
                End If
                '3
                Common_Procedures.Print_To_PrintDocument(e, "=", PrintWidth / 2 + 10, CurY, 0, 0, p2Font)
                Common_Procedures.Print_To_PrintDocument(e, "We will not accept any claim after processing of goods.", PrintWidth / 2 + 25, CurY, 0, 0, p1Font)

                '2
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "=", LMargin + 10, CurY, 0, 0, p2Font)
                Common_Procedures.Print_To_PrintDocument(e, "We are not responsible for any loss or damage in transit.", LMargin + 25, CurY, 0, 0, p1Font)
                '4
                Common_Procedures.Print_To_PrintDocument(e, "=", PrintWidth / 2 + 10, CurY, 0, 0, p2Font)
                Common_Procedures.Print_To_PrintDocument(e, "Subject to " & Trim(Common_Procedures.settings.Jurisdiction) & " jurisdiction. ", PrintWidth / 2 + 25, CurY, 0, 0, p1Font)

            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY

            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If

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
            '   CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Checked By ", LMargin + 250, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 10, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(vLine_Pen, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(vLine_Pen, PageWidth, LnAr(1), PageWidth, CurY)

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Then '---- Asia Textiles (Tirupur)
                CurY = CurY + TxtHgt - 10
                p1Font = New Font("Calibri", 9, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "Please send payment details of this bill to asiatextilestirupur@yahoo.in", LMargin + 10, CurY, 0, 0, p1Font)
            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub
    Private Function get_GST_Noof_HSN_Codes_For_Printing(ByVal EntryCode As String) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim NoofHsnCodes As Integer = 0

        NoofHsnCodes = 0

        Da = New SqlClient.SqlDataAdapter("Select * from Processed_Fabric_SalesInvoice_GST_Tax_Details Where ProcessedFabric_Sales_Invoice_Code = '" & Trim(EntryCode) & "'", Con)
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

        'Dt1 = New DataTable
        'Da.Fill(Dt1)
        'If Dt1.Rows.Count > 0 Then
        '    If Dt1.Rows.Count = 1 Then

        '        Da = New SqlClient.SqlDataAdapter("Select * from Processed_Fabric_SalesInvoice_GST_Tax_Details Where ProcessedFabric_Sales_Invoice_Code = '" & Trim(EntryCode) & "'", Con)
        '        Dt2 = New DataTable
        '        Da.Fill(Dt2)
        '        If Dt2.Rows.Count > 0 Then
        '            If Val(Dt2.Rows(0).Item("IGST_Amount").ToString) <> 0 Then
        '                TaxPerc = Val(Dt2.Rows(0).Item("IGST_Percentage").ToString)
        '            Else
        '                TaxPerc = Val(Dt2.Rows(0).Item("CGST_Percentage").ToString)
        '            End If
        '        End If
        '        Dt2.Clear()

        '    End If
        'End If
        Da = New SqlClient.SqlDataAdapter("Select * from Processed_Fabric_SalesInvoice_GST_Tax_Details Where ProcessedFabric_Sales_Invoice_Code = '" & Trim(EntryCode) & "'", Con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If Dt1.Rows.Count = 1 Then

                Da = New SqlClient.SqlDataAdapter("Select * from Processed_Fabric_SalesInvoice_GST_Tax_Details Where ProcessedFabric_Sales_Invoice_Code = '" & Trim(EntryCode) & "'", Con)
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
    Private Sub Printing_GST_HSN_Details_Format3(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Integer, ByVal PageWidth As Integer, ByVal PrintWidth As Double, ByRef CurY As Single, ByRef TopLnYAxis As Single, ByVal vLine_Pen As Pen)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim I As Integer = 0
        Dim p1Font As Font
        Dim SubClAr(15) As Single
        Dim ItmNm1 As String = "", ItmNm2 As String = ""
        Dim SNo As Integer = 0
        Dim Ttl_TaxAmt As Double, Ttl_CGst As Double, Ttl_Sgst As Double, Ttl_igst As Double
        Dim LnAr2 As Single
        Dim BmsInWrds As String = ""

        Try

            Ttl_TaxAmt = 0 : Ttl_CGst = 0 : Ttl_Sgst = 0

            Erase SubClAr

            SubClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

            SubClAr(1) = 120 : SubClAr(2) = 120 : SubClAr(3) = 45 : SubClAr(4) = 90 : SubClAr(5) = 45 : SubClAr(6) = 90 : SubClAr(7) = 45 : SubClAr(8) = 90
            SubClAr(9) = PageWidth - (LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8))

            CurY = CurY + 5

            Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin, CurY + 15, 2, SubClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TAXABLE AMOUNT", LMargin + SubClAr(1), CurY + 15, 2, SubClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CGST", LMargin + SubClAr(1) + SubClAr(2), CurY, 2, SubClAr(3) + SubClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SGST", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, 2, SubClAr(5) + SubClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "IGST", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, 2, SubClAr(7) + SubClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8), CurY, 2, SubClAr(9), pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(vLine_Pen, LMargin + SubClAr(1) + SubClAr(2), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8), CurY)
            LnAr2 = CurY
            CurY = CurY + 5
            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2), CurY, 2, SubClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), CurY, 2, SubClAr(4), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, 2, SubClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), CurY, 2, SubClAr(6), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, 2, SubClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), CurY, 2, SubClAr(8), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "TAX AMOUNT", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8), CurY, 2, SubClAr(9), pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)

            Da = New SqlClient.SqlDataAdapter("select * from Processed_Fabric_SalesInvoice_GST_Tax_Details where ProcessedFabric_Sales_Invoice_Code = '" & Trim(EntryCode) & "'", Con)
            Dt = New DataTable
            Da.Fill(Dt)

            If Dt.Rows.Count > 0 Then

                prn_DetIndx = 0

                CurY = CurY - 15

                Do While prn_DetIndx <= Dt.Rows.Count - 1

                    CurY = CurY + TxtHgt + 3

                    Common_Procedures.Print_To_PrintDocument(e, Trim(Dt.Rows(prn_DetIndx).Item("HSN_Code").ToString), LMargin + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("Taxable_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("Taxable_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("CGST_Percentage").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("CGST_Percentage").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("CGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("CGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) - 5, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("SGST_Percentage").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("SGST_Percentage").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("SGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("SGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) - 5, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("IGST_Percentage").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("IGST_Percentage").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Dt.Rows(prn_DetIndx).Item("IGST_Amount").ToString) <> 0, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("IGST_Amount").ToString)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) - 5, CurY, 1, 0, pFont)

                    Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(Dt.Rows(prn_DetIndx).Item("SGST_Amount").ToString) + Val(Dt.Rows(prn_DetIndx).Item("SGST_Amount").ToString) + Val(Dt.Rows(prn_DetIndx).Item("IGST_Amount").ToString)), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) + SubClAr(9) - 5, CurY, 1, 0, pFont)

                    Ttl_TaxAmt = Ttl_TaxAmt + Val(Dt.Rows(prn_DetIndx).Item("Taxable_Amount").ToString)
                    Ttl_CGst = Ttl_CGst + Val(Dt.Rows(prn_DetIndx).Item("CGST_Amount").ToString)
                    Ttl_Sgst = Ttl_Sgst + Val(Dt.Rows(prn_DetIndx).Item("SGST_Amount").ToString)
                    Ttl_igst = Ttl_igst + Val(Dt.Rows(prn_DetIndx).Item("IGST_Amount").ToString)

                    prn_DetIndx = prn_DetIndx + 1

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

            CurY = CurY + 5
            BmsInWrds = ""
            If (Val(Ttl_CGst) + Val(Ttl_Sgst) + Val(Ttl_igst)) <> 0 Then
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(Ttl_CGst) + Val(Ttl_Sgst) + Val(Ttl_igst))
            End If

            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Tax Amount(In Words) : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(vLine_Pen, LMargin, CurY, PageWidth, CurY)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles dtp_Date.KeyDown

        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_date.Focus()
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_date.Focus()
        End If
    End Sub

    Private Sub dtp_Date_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles dtp_Date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            msk_date.Focus()
        End If
    End Sub
    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        If IsDate(dtp_Date.Text) = True Then
            msk_date.Text = dtp_Date.Text
            msk_date.SelectionStart = 0
        End If
        GraceTime_Calculation()
    End Sub
    Private Sub txt_InvoicePrefixNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_InvoicePrefixNo.KeyDown
        On Error Resume Next
        If e.KeyValue = 38 Then txt_Packing.Focus()
        If e.KeyValue = 40 Then msk_Date.Focus()
    End Sub

    Private Sub txt_InvoicePrefixNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_InvoicePrefixNo.KeyPress
        If Asc(e.KeyChar) = 13 Then msk_Date.Focus()
    End Sub
End Class

