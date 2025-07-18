Public Class Ic_Invoice
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Filter_RowNo As Integer = -1
    Private Pk_Condition As String = "ICINV-"
    Private Pk_Condition2 As String = "ICAGC-"

    Private Pk_Condition_GST As String = "GSICI-"
    Private Pk_Condition2_GST As String = "GSIAG-"

    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private SaveAll_STS As Boolean = False
    Private LastNo As String = ""

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private Print_PDF_Status As Boolean = False

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private NoFo_STS As Integer = 0
    Private NoStk_STS As Integer = 0
    Private prn_HdIndx As Integer
    Private prn_HdMxIndx As Integer
    Private prn_Count As Integer
    Private prn_HdAr(100, 10) As String
    Private prn_DetAr(100, 50, 10) As String
    Private prn_InpOpts As String = ""
    Private prn_OriDupTri As String = ""

    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1
    Public vmskDcText As String = ""
    Public vmskDcStrt As Integer = -1
    Public vmskLrText As String = ""
    Public vmskLrStrt As Integer = -1



    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False
        Print_PDF_Status = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False

        pnl_Selection.Visible = False
        pnl_Cloth_Selection.Visible = False
        pnl_Ic_Selection.Visible = False
        pnl_Print.Visible = False


        vmskOldText = ""
        vmskSelStrt = -1
        vmskDcText = ""
        vmskDcStrt = -1
        vmskLrText = ""
        vmskLrStrt = -1


        lbl_InvNo.Text = ""
        lbl_InvNo.ForeColor = Color.Black

        dtp_Date.Text = ""
        msk_Date.Text = ""
        dtp_DcDate.Text = ""
        msk_DcDate.Text = ""
        dtp_LrDate.Text = ""
        msk_LrDate.Text = ""

        cbo_PartyName.Text = ""
        cbo_Type.Text = ""
        cbo_Agent.Text = ""
        cbo_SalesAcc.Text = ""
        cbo_DespTo.Text = ""
        cbo_Transport.Text = ""
        cbo_ClothName.Text = ""
        cbo_Com_Type.Text = "%"

        txt_FreightCharges.Text = ""
        txt_CommPerc.Text = ""
        txt_CommAmt.Text = ""

        txt_Days.Text = ""
        txt_LrNo.Text = ""

        txt_BaleNos.Text = ""
        txt_Folding.Text = ""
        txt_CashDisc_Perc.Text = ""
        lbl_CashDisc_Amount.Text = ""
        txt_TradeDisc_Perc.Text = ""
        lbl_TradeDisc_Amount.Text = ""
        lbl_GrossAmount.Text = ""
        lbl_Net_Amt.Text = ""
        txt_Rate.Text = ""
        txt_PackingCharges.Text = ""
        txt_IcNo.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        dgv_Cloth_DeliverySelection.Rows.Clear()


        ' ''cbo_PartyName.Enabled = True
        ' ''cbo_PartyName.BackColor = Color.White

        '' ''cbo_ClothName.Enabled = True
        '' ''cbo_ClothName.BackColor = Color.White

        ' ''msk_Date.Enabled = True
        ' ''msk_Date.BackColor = Color.White

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


        'cbo_ClothName.Visible = False
        'cbo_ClothName.Tag = -100
        'cbo_Grid_Clothtype.Visible = False
        'cbo_Grid_Clothtype.Tag = -100

        NoCalc_Status = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim Msktxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is CheckBox Or TypeOf Me.ActiveControl Is Button Or TypeOf Me.ActiveControl Is MaskedTextBox Then
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
            Msktxbx = Me.ActiveControl
            Msktxbx.SelectionStart = 0
        End If

        'If Me.ActiveControl.Name <> cbo_ClothName.Name Then
        '    cbo_ClothName.Visible = False
        '    cbo_ClothName.Tag = -100
        'End If

        'If Me.ActiveControl.Name <> cbo_Grid_Clothtype.Name Then
        '    cbo_Grid_Clothtype.Visible = False
        '    cbo_Grid_Clothtype.Tag = -100
        'End If



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
        dgv_Details.CurrentCell.Selected = False
        dgv_Details_Total.CurrentCell.Selected = False
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
        dgv_Details.CurrentCell.Selected = False
        dgv_Details_Total.CurrentCell.Selected = False
        dgv_Filter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub ClothSales_Cloth_Invoice_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Agent.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "AGENT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Agent.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_SalesAcc.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "SALES" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_SalesAcc.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ClothName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ClothName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub ClothSales_Cloth_Invoice_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub ClothSales_Cloth_Invoice_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Ic_Selection.Visible = True Then
                    btn_Cancel_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Cloth_Selection.Visible = True Then
                    btn_Close_ClothDelivery_Click(sender, e)
                    Exit Sub

                Else
                    Close_Form()

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

        da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
        da.Fill(dt4)
        cbo_ClothName.DataSource = dt4
        cbo_ClothName.DisplayMember = "Cloth_Name"

        da = New SqlClient.SqlDataAdapter("select distinct(Despatch_To) from Ic_Invoice_Head order by Despatch_To", con)
        da.Fill(dt6)
        cbo_DespTo.DataSource = dt6
        cbo_DespTo.DisplayMember = "Despatch_To"

        cbo_Com_Type.Items.Clear()
        cbo_Com_Type.Items.Add(" ")
        cbo_Com_Type.Items.Add("%")
        cbo_Com_Type.Items.Add("MTR")

        cbo_Type.Items.Clear()
        cbo_Type.Items.Add(" ")
        cbo_Type.Items.Add("IC")
        cbo_Type.Items.Add("INTERSTATE")

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        pnl_Ic_Selection.Visible = False
        pnl_Ic_Selection.Left = (Me.Width - pnl_Ic_Selection.Width) \ 2
        pnl_Ic_Selection.Top = (Me.Height - pnl_Ic_Selection.Height) \ 2
        pnl_Ic_Selection.BringToFront()

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        pnl_Print.Visible = False
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2
        pnl_Print.BringToFront()

        pnl_Ic_Selection.Visible = False
        pnl_Ic_Selection.Left = (Me.Width - pnl_Ic_Selection.Width) \ 2
        pnl_Ic_Selection.Top = (Me.Height - pnl_Ic_Selection.Height) \ 2
        pnl_Ic_Selection.BringToFront()

        pnl_Cloth_Selection.Visible = False
        pnl_Cloth_Selection.Left = (Me.Width - pnl_Cloth_Selection.Width) \ 2
        pnl_Cloth_Selection.Top = (Me.Height - pnl_Cloth_Selection.Height) \ 2
        pnl_Cloth_Selection.BringToFront()


        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Agent.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Type.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DespTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_IcNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_SalesAcc.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Com_Type.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Rate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_FreightCharges.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_LrDate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LrNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BaleNos.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Folding.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_CommPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CommAmt.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CashDisc_Perc.GotFocus, AddressOf ControlGotFocus

        AddHandler msk_DcDate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Days.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PackingCharges.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_GrossAmount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TradeDisc_Perc.GotFocus, AddressOf ControlGotFocus


        AddHandler txt_ClthDetail_Name.GotFocus, AddressOf ControlGotFocus


        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ClothName.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Bale.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Invoice.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Cancel.GotFocus, AddressOf ControlGotFocus


        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Agent.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_IcNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DespTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_SalesAcc.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Com_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rate.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_LrDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LrNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BaleNos.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_FreightCharges.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Folding.LostFocus, AddressOf ControlLostFocus

        AddHandler msk_DcDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Days.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CashDisc_Perc.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_CommPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CommAmt.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Rate.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_PackingCharges.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_GrossAmount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TradeDisc_Perc.LostFocus, AddressOf ControlLostFocus


        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ClothName.LostFocus, AddressOf ControlLostFocus



        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Bale.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Invoice.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Cancel.LostFocus, AddressOf ControlLostFocus

        AddHandler msk_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_FreightCharges.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_LrNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_LrDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Folding.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_BaleNos.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_IcNo.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler msk_DcDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Days.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CommPerc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CommAmt.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler txt_CashDisc_Perc.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Rate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PackingCharges.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler lbl_GrossAmount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CashDic_Name.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ClthDetail_Name.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler msk_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_FreightCharges.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_LrDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_LrNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Folding.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BaleNos.KeyPress, AddressOf TextBoxControlKeyPress


        AddHandler msk_DcDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Days.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CommPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CommAmt.KeyPress, AddressOf TextBoxControlKeyPress


        AddHandler txt_CashDisc_Perc.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_TradeDisc_Perc.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_PackingCharges.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Rate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Insurance_Name.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler lbl_GrossAmount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_ClthDetail_Name.KeyPress, AddressOf TextBoxControlKeyPress

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
                        If .CurrentCell.ColumnIndex >= 4 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                cbo_SalesAcc.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                cbo_PartyName.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(4)

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

        If Val(no) = 0 Then Exit Sub

        clear()

        NoCalc_Status = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from Ic_Invoice_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Ic_Invoice_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_InvNo.Text = dt1.Rows(0).Item("Ic_Invoice_No").ToString
                msk_Date.Text = dt1.Rows(0).Item("Ic_Invoice_Date").ToString
                cbo_Type.Text = dt1.Rows(0).Item("Invoice_Selection_Type").ToString
                cbo_PartyName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                cbo_SalesAcc.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("SalesAc_IdNo").ToString))
                cbo_DespTo.Text = dt1.Rows(0).Item("Despatch_To").ToString
                cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))
                txt_LrNo.Text = dt1.Rows(0).Item("Lr_No").ToString
                msk_LrDate.Text = dt1.Rows(0).Item("Lr_Date").ToString
                cbo_Agent.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Agent_IdNo").ToString))
                cbo_Com_Type.Text = dt1.Rows(0).Item("Agent_Comm_Type").ToString
                txt_Rate.Text = Format(Val(dt1.Rows(0).Item("Rate").ToString), "########0.00")

                txt_CommPerc.Text = Format(Val(dt1.Rows(0).Item("Agent_Comm_Perc").ToString), "########0.00")

                txt_CommAmt.Text = Format(Val(dt1.Rows(0).Item("Agent_Comm_Total").ToString), "########0.00")
                msk_DcDate.Text = dt1.Rows(0).Item("Dc_Date").ToString
                txt_Folding.Text = Format(Val(dt1.Rows(0).Item("Folding").ToString), "########0.00")
                cbo_ClothName.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_IdNo").ToString))
                lbl_GrossAmount.Text = Format(Val(dt1.Rows(0).Item("Gross_Amount").ToString), "########0.00")
                txt_BaleNos.Text = dt1.Rows(0).Item("Bale_No").ToString
                txt_CashDisc_Perc.Text = dt1.Rows(0).Item("Cash_Discount_Perc").ToString
                lbl_CashDisc_Amount.Text = dt1.Rows(0).Item("Cash_Discount").ToString
                txt_PackingCharges.Text = Format(Val(dt1.Rows(0).Item("Packing_Charges").ToString), "########0.00")
                txt_FreightCharges.Text = Format(Val(dt1.Rows(0).Item("Freight_Charges").ToString), "########0.00")
                txt_TradeDisc_Perc.Text = dt1.Rows(0).Item("Trade_Discount").ToString
                lbl_TradeDisc_Amount.Text = dt1.Rows(0).Item("Trade_Discount_Perc").ToString
                txt_Days.Text = dt1.Rows(0).Item("Days").ToString
                lbl_Net_Amt.Text = Format(Val(dt1.Rows(0).Item("Net_Amount").ToString), "#########0.00")
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

              
                da2 = New SqlClient.SqlDataAdapter("Select a.* from Ic_Invoice_Details a  Where a.Ic_Invoice_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_Details

                    .Rows.Clear()
                    SNo = 0
                    If Trim(UCase(cbo_Type.Text)) = "IC" Then
                        If dt2.Rows.Count > 0 Then

                            For i = 0 To dt2.Rows.Count - 1

                                n = .Rows.Add()

                                SNo = SNo + 1

                                .Rows(n).Cells(0).Value = Val(SNo)
                                .Rows(n).Cells(1).Value = dt2.Rows(i).Item("Ic_No").ToString
                                .Rows(n).Cells(2).Value = dt2.Rows(i).Item("Bale_No").ToString
                                .Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Pcs").ToString)
                                .Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                                .Rows(n).Cells(6).Value = dt2.Rows(i).Item("Ic_Delivery_Code").ToString
                                .Rows(n).Cells(10).Value = dt2.Rows(i).Item("Ic_Delivery_SlNo").ToString
                            Next i

                        End If
                    End If

                    If Trim(UCase(cbo_Type.Text)) = "INTERSTATE" Then

                        If dt2.Rows.Count > 0 Then

                            For i = 0 To dt2.Rows.Count - 1

                                n = .Rows.Add()

                                SNo = SNo + 1

                                .Rows(n).Cells(0).Value = Val(SNo)
                                .Rows(n).Cells(1).Value = dt2.Rows(i).Item("ClothSales_Delivery_No").ToString
                                .Rows(n).Cells(2).Value = Common_Procedures.ClothType_IdNoToName(con, Val(dt2.Rows(0).Item("ClothType_IdNo").ToString))
                                .Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Invoice_Pcs").ToString)
                                .Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Invoice_Meters").ToString), "########0.00")
                                .Rows(n).Cells(5).Value = dt2.Rows(i).Item("ClothSales_Delivery_Code").ToString
                                .Rows(n).Cells(8).Value = Common_Procedures.Cloth_IdNoToName(con, Val(dt2.Rows(0).Item("Cloth_IdNo").ToString))
                                .Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("Folding_Perc").ToString), "########0.00")
                                '.Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("Delivery_Meters").ToString), "########0.00")
                                .Rows(n).Cells(6).Value = dt2.Rows(i).Item("ClothSales_Delivery_SlNo").ToString
                            Next i

                        End If

                    End If

                    If .Rows.Count = 0 Then
                        .Rows.Add()
                    End If

                End With

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()

                    .Rows(0).Cells(3).Value = Val(dt1.Rows(0).Item("Total_Pcs").ToString)
                    .Rows(0).Cells(4).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")


                End With

                'da2 = New SqlClient.SqlDataAdapter("Select a.* from Packing_Slip_Head a Where a.Delivery_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' Order by a.Delivery_DetailsSlNo, a.Delivery_No, a.Packing_Slip_Date, a.for_OrderBy, a.Packing_Slip_No, a.Packing_Slip_Code", con)
                'dt2 = New DataTable
                'da2.Fill(dt2)

                'With dgv_BaleDetails

                '    .Rows.Clear()
                '    SNo = 0

                '    If dt2.Rows.Count > 0 Then

                '        For i = 0 To dt2.Rows.Count - 1

                '            n = .Rows.Add()

                '            SNo = SNo + 1

                '            .Rows(n).Cells(0).Value = Val(dt2.Rows(i).Item("Delivery_DetailsSlNo").ToString)
                '            .Rows(n).Cells(1).Value = dt2.Rows(i).Item("Packing_Slip_No").ToString
                '            .Rows(n).Cells(2).Value = Val(dt2.Rows(i).Item("Total_Pcs").ToString)
                '            .Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Total_Meters").ToString)
                '            .Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Total_Weight").ToString)
                '            .Rows(n).Cells(5).Value = dt2.Rows(i).Item("Packing_Slip_Code").ToString
                '            .Rows(n).Cells(6).Value = dt2.Rows(i).Item("Bale_Bundle").ToString

                '        Next i

                '    End If

                'End With


                'da3 = New SqlClient.SqlDataAdapter("Select a.*, b.cloth_name, c.ClothType_Name from ClothSales_Invoice_Buyer_Offer_Details a, Cloth_Head b, ClothType_Head c Where a.Ic_Invoice_Code = '" & Trim(NewCode) & "' and a.Cloth_IdNo = b.Cloth_IdNo and a.ClothType_IdNo = c.ClothType_IdNo Order by a.Sl_No", con)
                'dt3 = New DataTable
                'da3.Fill(dt3)

                'With dgv_Buyer_Offer_Detail

                '    .Rows.Clear()
                '    SNo = 0

                '    If dt3.Rows.Count > 0 Then

                '        For i = 0 To dt3.Rows.Count - 1

                '            n = .Rows.Add()

                '            SNo = SNo + 1

                '            .Rows(n).Cells(0).Value = Val(SNo)
                '            .Rows(n).Cells(1).Value = dt3.Rows(i).Item("Buyer_offer_No").ToString
                '            .Rows(n).Cells(2).Value = dt3.Rows(i).Item("Buyer_Offer_Date").ToString
                '            .Rows(n).Cells(3).Value = dt3.Rows(i).Item("Cloth_Name").ToString
                '            .Rows(n).Cells(4).Value = dt3.Rows(i).Item("ClothType_Name").ToString
                '            .Rows(n).Cells(5).Value = Val(dt3.Rows(i).Item("Folding").ToString)
                '            .Rows(n).Cells(6).Value = Val(dt3.Rows(i).Item("Pcs").ToString)
                '            .Rows(n).Cells(7).Value = dt3.Rows(i).Item("Meters").ToString
                '            .Rows(n).Cells(8).Value = dt3.Rows(i).Item("Buyer_Offer_Code").ToString

                '        Next i

                '    End If

                'End With


            End If

            If LockSTS = True Then
                cbo_PartyName.Enabled = False
                cbo_PartyName.BackColor = Color.LightGray

                cbo_ClothName.Enabled = False
                cbo_ClothName.BackColor = Color.LightGray

                msk_Date.Enabled = False
                msk_Date.BackColor = Color.LightGray

                dgv_Details.AllowUserToAddRows = False

                btn_Selection.Enabled = False

            End If

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

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim nr As Integer = 0

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.ClothSales_Cloth_Invoice_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.ClothSales_Cloth_Invoice_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

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

            Da = New SqlClient.SqlDataAdapter("select * from Ic_Invoice_Head where Ic_Invoice_code = '" & Trim(NewCode) & "'", con)
            Da.SelectCommand.Transaction = trans
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then
                If Trim(UCase(Dt1.Rows(0).Item("Invoice_Selection_Type").ToString)) = "INTERSTATE" Then
                    cmd.CommandText = "Update ClothSales_Delivery_Details set Invoice_Meters = a.Invoice_Meters - b.Invoice_Meters from ClothSales_Delivery_Details a, Ic_Invoice_Details b Where b.Ic_Invoice_Code = '" & Trim(NewCode) & "' and a.ClothSales_Delivery_code = b.ClothSales_Delivery_code and a.ClothSales_Delivery_SlNo = b.ClothSales_Delivery_SlNo"
                    cmd.ExecuteNonQuery()

                Else
                    cmd.CommandText = "Update Ic_Delivery_Details set Ic_Invoice_Code = '' where Ic_Invoice_Code = '" & Trim(NewCode) & "'"
                    cmd.ExecuteNonQuery()

                End If
            End If
            Dt1.Clear()

            'cmd.CommandText = "Update Ic_Delivery_Details set Ic_Invoice_Code = '' from Ic_Delivery_Details a,Ic_Invoice_Details b where b.Ic_Invoice_Code = '" & Trim(NewCode) & "' and a.Ic_Delivery_code = b.Ic_Delivery_code and a.Ic_Delivery_SlNo = b.Ic_Delivery_SlNo"
            'cmd.ExecuteNonQuery()

            'If Trim(UCase(cbo_Type.Text)) = "INTERSTATE" Then
            '    cmd.CommandText = "Update ClothSales_Delivery_Details set Invoice_Meters = a.Invoice_Meters - b.Invoice_Meters from ClothSales_Delivery_Details a, Ic_Invoice_Details b Where b.Ic_Invoice_Code = '" & Trim(NewCode) & "' and a.ClothSales_Delivery_code = b.ClothSales_Delivery_code and a.ClothSales_Delivery_SlNo = b.ClothSales_Delivery_SlNo"
            '    nr = cmd.ExecuteNonQuery()
            'End If

            cmd.CommandText = "delete from Ic_Invoice_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Ic_Invoice_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from Ic_Invoice_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Ic_Invoice_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from AgentCommission_Processing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
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

            If msk_Date.Enabled = True And msk_Date.Visible = True Then msk_Date.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead Where ( Ledger_IdNo = 0 or (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ) order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"


            da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
            da.Fill(dt2)
            cbo_Filter_ClothName.DataSource = dt2
            cbo_Filter_ClothName.DisplayMember = "Cloth_Name"

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

            da = New SqlClient.SqlDataAdapter("select top 1 Ic_Invoice_No from Ic_Invoice_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Ic_Invoice_Code NOT like '" & Trim(Pk_Condition_GST) & "%' and Ic_Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Ic_Invoice_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Ic_Invoice_No from Ic_Invoice_Head where for_orderby > " & Str(Val(OrdByNo)) & " and Ic_Invoice_Code NOT like '" & Trim(Pk_Condition_GST) & "%' and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Ic_Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Ic_Invoice_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Ic_Invoice_No from Ic_Invoice_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Ic_Invoice_Code NOT like '" & Trim(Pk_Condition_GST) & "%'  and Ic_Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Ic_Invoice_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Ic_Invoice_No from Ic_Invoice_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Ic_Invoice_Code NOT like '" & Trim(Pk_Condition_GST) & "%' and Ic_Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Ic_Invoice_No desc", con)
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

            lbl_InvNo.Text = Common_Procedures.get_MaxCode(con, "Ic_Invoice_Head", "Ic_Invoice_Code", "For_OrderBy", "Ic_Invoice_Code NOT like '" & Trim(Pk_Condition_GST) & "%'", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_InvNo.ForeColor = Color.Red

            msk_Date.Text = Date.Today.ToShortDateString
            Da1 = New SqlClient.SqlDataAdapter("select top 1 a.*, b.ledger_name as SalesAcName from Ic_Invoice_Head a LEFT OUTER JOIN Ledger_Head b ON a.SalesAc_IdNo = b.Ledger_IdNo where company_idno = " & Str(Val(lbl_Company.Tag)) & "  and Ic_Invoice_Code NOT like '" & Trim(Pk_Condition_GST) & "%' and Ic_Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  and Ic_Invoice_Code NOT like '" & Trim(Pk_Condition_GST) & "%' Order by for_Orderby desc, Ic_Invoice_No desc", con)
            Dt1 = New DataTable
            Da1.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If Dt1.Rows(0).Item("Ic_Invoice_Date").ToString <> "" Then msk_Date.Text = Dt1.Rows(0).Item("Ic_Invoice_Date").ToString
                End If
                If Dt1.Rows(0).Item("SalesAcName").ToString <> "" Then cbo_SalesAcc.Text = Dt1.Rows(0).Item("SalesAcName").ToString
                If Dt1.Rows(0).Item("Cloth_Details").ToString <> "" Then txt_ClthDetail_Name.Text = Dt1.Rows(0).Item("Cloth_Details").ToString
            End If

            Dt1.Clear()


        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

            inpno = InputBox("Enter Inv No.", "FOR FINDING...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Ic_Invoice_No from Ic_Invoice_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Ic_Invoice_Code = '" & Trim(InvCode) & "'", con)
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

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.ClothSales_Cloth_Invoice_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.ClothSales_Cloth_Invoice_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        Try

            inpno = InputBox("Enter New Inv No.", "FOR NEW INV NO. INSERTION...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Ic_Invoice_No from Ic_Invoice_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Ic_Invoice_Code = '" & Trim(InvCode) & "'", con)
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
        Dim FP_ID As Integer = 0
        Dim Grclthtyp_ID As Integer = 0
        Dim Grclth_ID As Integer = 0
        Dim Trans_ID As Integer
        Dim Led_ID As Integer = 0
        Dim Agt_Idno As Integer = 0
        Dim Sno As Integer = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Partcls As String = ""
        Dim vTotPcs As Single, vTotMtrs As Single, vTotBals As Single
        Dim SlAc_ID As Integer = 0
        Dim OnAc_ID As Integer = 0
        Dim YrnClthNm As String = ""
        Dim Nr As Integer = 0
        Dim IcCd As String = ""
        Dim IcSlNo As Long = 0
        Dim CltCd As String = ""
        Dim CltSlNo As Long = 0
        Dim DcCd As String = ""
        Dim DcSlNo As Long = 0
        Dim Usr_ID As Integer = 0

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Common_Procedures.UserRight_Check(Common_Procedures.UR.ClothSales_Cloth_Invoice_Entry, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
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

        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled Then cbo_PartyName.Focus()
            Exit Sub
        End If

        Agt_Idno = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Agent.Text)
        Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)
        SlAc_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_SalesAcc.Text)
        clth_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)
        lbl_UserName.Text = Common_Procedures.User.IdNo
        If SlAc_ID = 0 And Val(lbl_Net_Amt.Text) <> 0 Then
            MessageBox.Show("Invalid Sales A/c", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_SalesAcc.Enabled Then cbo_SalesAcc.Focus()
            Exit Sub
        End If

        For i = 0 To dgv_Details.RowCount - 1

            If Val(dgv_Details.Rows(i).Cells(4).Value) <> 0 Then

                'If Val(dgv_Details.Rows(i).Cells(3).Value) = 0 Then
                '    MessageBox.Show("Invalid Pcs", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                '    If dgv_Details.Enabled And dgv_Details.Visible Then
                '        dgv_Details.Focus()
                '        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(3)
                '    End If
                '    Exit Sub
                'End If

                If Val(dgv_Details.Rows(i).Cells(4).Value) = 0 Then
                    MessageBox.Show("Invalid  metres", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(4)
                    End If
                    Exit Sub
                End If

            End If

        Next

        NoCalc_Status = False
        Total_Calculation()

        vTotPcs = 0 : vTotMtrs = 0 : vTotBals = 0

        If dgv_Details_Total.RowCount > 0 Then
            vTotBals = Val(dgv_Details_Total.Rows(0).Cells(2).Value())
            vTotPcs = Val(dgv_Details_Total.Rows(0).Cells(3).Value())
            vTotMtrs = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
            'vTotAmt = Val(dgv_Details_Total.Rows(0).Cells(9).Value())
        End If

        If vTotMtrs = 0 Then
            MessageBox.Show("Invalid METERS", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dgv_Details.Enabled And dgv_Details.Visible Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)
            End If
            Exit Sub
        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_InvNo.Text = Common_Procedures.get_MaxCode(con, "Ic_Invoice_Head", "Ic_Invoice_Code", "For_OrderBy", "Ic_Invoice_Code not like '" & Trim(Pk_Condition_GST) & "%' ", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@InvoiceDate", Convert.ToDateTime(msk_Date.Text))

            If New_Entry = True Then

                cmd.CommandText = "Insert into Ic_Invoice_Head ( Ic_Invoice_Code ,               Company_IdNo       ,     Ic_Invoice_No  ,                     for_OrderBy                                                           ,                                Ic_Invoice_Date  ,              Ledger_IdNo,               Invoice_Selection_Type             ,    SalesAc_IdNo               , Despatch_To                 ,      Transport_IdNo            ,              Lr_No                  ,          Lr_Date       ,      Agent_IdNo           , Agent_Comm_Perc                , Agent_Comm_Total                    , Dc_Date                         ,    Folding                ,          Cloth_IdNo     ,               Rate                        ,  Cloth_Details                     ,          Gross_Amount           ,             Bale_No               ,      Cash_Discount_Perc                   ,  Cash_Discount                        , Trade_Discount_Perc                ,     Trade_Discount                        ,       Packing_Charges                        , Freight_Charges                   ,            Days                            , Net_Amount                        , Total_Pcs               ,                   Total_Meters         , Agent_Comm_Type                  ,       User_IdNo   ) " & _
                                    "     Values                              (   '" & Trim(NewCode) & "'      , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvNo.Text))) & ",       @InvoiceDate               , " & Str(Val(Led_ID)) & ",  '" & Trim(UCase(cbo_Type.Text)) & "' ,     " & Str(Val(SlAc_ID)) & ", '" & Trim(cbo_DespTo.Text) & "',   " & Str(Val(Trans_ID)) & " ,  '" & Trim(txt_LrNo.Text) & "', '" & Trim(msk_LrDate.Text) & "', " & Str(Val(Agt_Idno)) & ", " & Str(Val(txt_CommPerc.Text)) & ", " & Str(Val(txt_CommAmt.Text)) & ",  '" & Trim(msk_DcDate.Text) & "' , " & Val(txt_Folding.Text) & ",  " & Val(clth_ID) & " , " & Str(Val(txt_Rate.Text)) & " ,  '" & Trim(txt_ClthDetail_Name.Text) & "',  " & Str(Val(lbl_GrossAmount.Text)) & " ,  '" & Trim(txt_BaleNos.Text) & "', " & Str(Val(txt_CashDisc_Perc.Text)) & ", " & Str(Val(lbl_CashDisc_Amount.Text)) & ", " & Str(Val(txt_TradeDisc_Perc.Text)) & ", " & Str(Val(lbl_TradeDisc_Amount.Text)) & " , " & Val(txt_PackingCharges.Text) & ", " & Str(Val(txt_FreightCharges.Text)) & ",  " & Str(Val(txt_Days.Text)) & ", " & Str(Val(CSng(lbl_Net_Amt.Text))) & ",  " & Str(Val(vTotPcs)) & ", " & Str(Val(vTotMtrs)) & ", '" & Trim(cbo_Com_Type.Text) & "'," & Val(lbl_UserName.Text) & " ) "
                cmd.ExecuteNonQuery()

            Else

                Da = New SqlClient.SqlDataAdapter("select * from Ic_Invoice_Head where Ic_Invoice_code = '" & Trim(NewCode) & "'", con)
                Da.SelectCommand.Transaction = tr
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then
                    If Trim(UCase(Dt1.Rows(0).Item("Invoice_Selection_Type").ToString)) = "INTERSTATE" Then
                        cmd.CommandText = "Update ClothSales_Delivery_Details set Invoice_Meters = a.Invoice_Meters - b.Invoice_Meters from ClothSales_Delivery_Details a, Ic_Invoice_Details b Where b.Ic_Invoice_Code = '" & Trim(NewCode) & "' and a.ClothSales_Delivery_code = b.ClothSales_Delivery_code and a.ClothSales_Delivery_SlNo = b.ClothSales_Delivery_SlNo"
                        cmd.ExecuteNonQuery()

                    Else
                        cmd.CommandText = "Update Ic_Delivery_Details set Ic_Invoice_Code = '' where Ic_Invoice_Code = '" & Trim(NewCode) & "'"
                        cmd.ExecuteNonQuery()

                    End If
                End If
                Dt1.Clear()

                cmd.CommandText = "Update Ic_Invoice_Head set Ic_Invoice_Date = @InvoiceDate, Ledger_IdNo =  " & Str(Val(Led_ID)) & " , Invoice_Selection_Type = '" & Trim(UCase(cbo_Type.Text)) & "' ,  SalesAc_IdNo = " & Str(Val(SlAc_ID)) & " , Despatch_To = '" & Trim(cbo_DespTo.Text) & "',  Transport_IdNo = " & Str(Val(Trans_ID)) & ",  Lr_No = '" & Trim(txt_LrNo.Text) & "'  , Lr_Date  = '" & Trim(msk_LrDate.Text) & "', Agent_IdNo = " & Str(Val(Agt_Idno)) & ", Agent_Comm_Perc =  " & Str(Val(txt_CommPerc.Text)) & ",Agent_Comm_Type = '" & Trim(cbo_Com_Type.Text) & "',  Agent_Comm_Total = " & Str(Val(txt_CommAmt.Text)) & ",  Dc_Date = '" & Trim(msk_DcDate.Text) & "',   Folding =  " & Str(Val(txt_Folding.Text)) & ", Cloth_IdNo = " & Str(Val(clth_ID)) & " , Cloth_Details =  '" & Trim(txt_ClthDetail_Name.Text) & "',  Cash_Discount_Perc = " & Str(Val(txt_CashDisc_Perc.Text)) & " , Cash_Discount = " & Str(Val(lbl_CashDisc_Amount.Text)) & " , Trade_Discount_Perc = " & Str(Val(txt_TradeDisc_Perc.Text)) & " , Trade_Discount =  " & Str(Val(lbl_TradeDisc_Amount.Text)) & " ,  Packing_Charges = " & Str(Val(txt_PackingCharges.Text)) & " , Freight_Charges =" & Str(Val(txt_FreightCharges.Text)) & " , Rate =" & Val(txt_Rate.Text) & " , Gross_Amount =  " & Str(Val(lbl_GrossAmount.Text)) & ", Bale_No = '" & Trim(txt_BaleNos.Text) & "' , Net_Amount = " & Str(Val(CSng(lbl_Net_Amt.Text))) & " ,    Total_Pcs =  " & Str(Val(vTotPcs)) & ", Total_Meters = " & Str(Val(vTotMtrs)) & " ,User_idno = " & Val(lbl_UserName.Text) & "   Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Ic_Invoice_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            EntID = Trim(Pk_Condition) & Trim(lbl_InvNo.Text)
            PBlNo = Trim(lbl_InvNo.Text)
            Partcls = "IcInvoice : Inv.No. " & Trim(lbl_InvNo.Text)

            cmd.CommandText = "Delete from Ic_Invoice_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Ic_Invoice_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Details

                Sno = 0

                If Trim(UCase(cbo_Type.Text)) = "IC" Then
                    For i = 0 To .RowCount - 1

                        If Val(.Rows(i).Cells(4).Value) <> 0 Then

                            Sno = Sno + 1

                            cmd.CommandText = "Insert into Ic_Invoice_Details (         Ic_Invoice_Code  ,               Company_IdNo       ,           Ic_Invoice_No       ,                               for_OrderBy                              ,         Ic_Invoice_Date       ,              Sl_No        ,                 Ic_No                   ,             Bale_No                       ,                          Pcs                 ,                      Meters        ,               Ic_Delivery_Code              ,Ic_Delivery_SlNo                           ) " & _
                                                    "     Values              (   '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvNo.Text))) & ",       @InvoiceDate            ,  " & Str(Val(Sno)) & ",  '" & Trim(.Rows(i).Cells(1).Value) & "', '" & Trim(.Rows(i).Cells(2).Value) & "',  " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ",   '" & Trim(.Rows(i).Cells(6).Value) & "'  ," & Str(Val(.Rows(i).Cells(10).Value)) & ") "
                            cmd.ExecuteNonQuery()

                            If Trim(UCase(cbo_Type.Text)) = "IC" Then

                                Nr = 0
                                cmd.CommandText = "Update Ic_Delivery_Details set Invoice_No ='" & Trim(lbl_InvNo.Text) & "' , Ic_Invoice_Code = '" & Trim(NewCode) & "' Where Ic_Delivery_code = '" & Trim(.Rows(i).Cells(6).Value) & "' and Bale_No = '" & Trim(.Rows(i).Cells(2).Value) & "'"
                                Nr = cmd.ExecuteNonQuery()

                                If Nr = 0 Then
                                    Throw New ApplicationException("Mismatch of Delivery and Party Details")
                                    Exit Sub
                                End If

                            End If

                        End If

                    Next
                End If

                If Trim(UCase(cbo_Type.Text)) = "INTERSTATE" Then
                    For i = 0 To .RowCount - 1

                        If Val(.Rows(i).Cells(4).Value) <> 0 Then

                            DcCd = ""
                            DcSlNo = 0
                            If Trim(UCase(cbo_Type.Text)) = "INTERSTATE" Then
                                DcCd = Trim(.Rows(i).Cells(5).Value)
                                DcSlNo = Val(.Rows(i).Cells(6).Value)
                            End If

                            Grclth_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(8).Value, tr)
                            Grclthtyp_ID = Common_Procedures.ClothType_NameToIdNo(con, .Rows(i).Cells(2).Value, tr)
                            Sno = Sno + 1

                            cmd.CommandText = "Insert into Ic_Invoice_Details ( Ic_Invoice_Code  ,               Company_IdNo       ,                          Ic_Invoice_No    ,                               for_OrderBy                              ,         Ic_Invoice_Date       ,              Sl_No        ,                 ClothSales_Delivery_No     ,             ClothType_IdNo                   ,    Invoice_Pcs                 ,                 Invoice_Meters   ,                Cloth_IdNo       ,                             Folding_Perc            ,   ClothSales_Delivery_code , ClothSales_Delivery_SlNo         ) " & _
                                                    "     Values              (   '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvNo.Text))) & ",       @InvoiceDate            ,  " & Str(Val(Sno)) & ",  '" & Trim(.Rows(i).Cells(1).Value) & "',      " & Str(Val(Grclthtyp_ID)) & "  ,  " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ",   " & Str(Val(Grclth_ID)) & "   ," & Str(Val(.Rows(i).Cells(9).Value)) & " , '" & Trim(DcCd) & "'  , " & Str(Val(DcSlNo)) & ") "
                            cmd.ExecuteNonQuery()

                            If Trim(UCase(cbo_Type.Text)) = "INTERSTATE" And Trim(.Rows(i).Cells(5).Value) <> "" Then
                                Nr = 0
                                cmd.CommandText = "Update ClothSales_Delivery_Details set Invoice_Meters = Invoice_Meters + " & Str(Val(.Rows(i).Cells(4).Value)) & " Where ClothSales_Delivery_code = '" & Trim(.Rows(i).Cells(5).Value) & "' and ClothSales_Delivery_SlNo = " & Str(Val(.Rows(i).Cells(6).Value)) & " "
                                Nr = cmd.ExecuteNonQuery()
                                If Nr = 0 Then
                                    Throw New ApplicationException("Mismatch of Delivery and Party Details")
                                    Exit Sub
                                End If
                            End If
                        End If

                    Next
                End If

            End With

            'AgentCommission Posting
            cmd.CommandText = "delete from AgentCommission_Processing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If Val(Agt_Idno) <> 0 Then

                cmd.CommandText = "Insert into AgentCommission_Processing_Details (  Reference_Code   ,             Company_IdNo         ,            Reference_No       ,                               For_OrderBy                              , Reference_Date, Commission_For,     Ledger_IdNo    ,           Agent_IdNo      ,         Entry_ID     ,      Party_BillNo    ,       Particulars      ,      Yarn_Cloth_Name     ,         Bags_Meters       ,               Amount              ,             Commission_Rate         ,            Commission_Amount       ) " & _
                                                " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_InvNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_InvNo.Text))) & ",   @InvoiceDate,     'CLOTH'   , " & Str(Led_ID) & ", " & Str(Val(Agt_Idno)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', '" & Trim(YrnClthNm) & "', " & Str(Val(vTotMtrs)) & ", " & Str(Val(CSng(lbl_Net_Amt.Text))) & ",  " & Str(Val(txt_CommPerc.Text)) & ", " & Str(Val(txt_CommAmt.Text)) & " ) "
                cmd.ExecuteNonQuery()

            End If

            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
            vLed_IdNos = Led_ID & "|" & SlAc_ID
            vVou_Amts = -1 * Val(CSng(lbl_Net_Amt.Text)) & "|" & Val(CSng(lbl_Net_Amt.Text))
            If Common_Procedures.Voucher_Updation(con, "Ic.Sale", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_InvNo.Text), Convert.ToDateTime(msk_Date.Text), "Inv No : " & Trim(lbl_InvNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), tr)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1035" Then '---- Kalaimagal Textiles (Avinashi)
                vLed_IdNos = Agt_Idno & "|" & Val(Common_Procedures.CommonLedger.Agent_Commission_Ac)
                vVou_Amts = Val(txt_CommAmt.Text) & "|" & -1 * Val(txt_CommAmt.Text)
                If Common_Procedures.Voucher_Updation(con, "AgComm.IcSale", Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), Trim(lbl_InvNo.Text), Convert.ToDateTime(msk_Date.Text), "Inv No : " & Trim(lbl_InvNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                    Throw New ApplicationException(ErrMsg)
                End If
            End If

            Dim VouBil As String = ""
            VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), msk_Date.Text, Led_ID, Trim(lbl_InvNo.Text), Agt_Idno, Val(CSng(lbl_Net_Amt.Text)), "DR", Trim(Pk_Condition) & Trim(NewCode), tr, Common_Procedures.SoftwareTypes.Textile_Software)
            If Trim(UCase(VouBil)) = "ERROR" Then
                Throw New ApplicationException("Error on Voucher Bill Posting")
            End If

            tr.Commit()

            If SaveAll_STS <> True Then
                MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
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

            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()

        End Try

    End Sub


    Private Sub NetAmount_Calculation()
        Dim InvMtrs As Single = 0, TtMtrs As Single = 0, Fmt As Single = 0
        Dim GrsAmt As Single = 0
        Dim NtAmt As Single = 0

        If NoCalc_Status = True Then Exit Sub

        TtMtrs = 0
        With dgv_Details_Total
            If .RowCount > 0 Then
                TtMtrs = Val(.Rows(0).Cells(4).Value)
            End If
        End With

        If Val(txt_Folding.Text) <> 0 And Val(txt_Folding.Text) <> 100 Then
            Fmt = ((100 - Val(txt_Folding.Text)) / 100) * Val(TtMtrs)
            Fmt = Format(Math.Abs(Val(Fmt)), "######0.00")

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1035" Then
                Fmt = Common_Procedures.Meter_RoundOff(Fmt)

            End If

            'If Trim(Settings.Invoice_FoldingLess_MeterRoundOff) = "ONE DIGIT ROUNDOFF" Then
            '    Fmt = Format(Fmt, "#########0.0")
            'ElseIf Trim(Settings.Invoice_FoldingLess_MeterRoundOff) = "FULL ROUNDOFF" Then
            '    Fmt = Format(Fmt, "#########0")
            'ElseIf Trim(Settings.Invoice_FoldingLess_MeterRoundOff) = "METER ROUNDOFF" Then
            '    Select Case Fmt - Int(Fmt)
            '        Case Is < 0.13
            '            Fmt = Int(Fmt)
            '        Case Is < 0.25 + 0.13
            '            Fmt = Int(Fmt) + 0.25
            '        Case Is < 0.5 + 0.13
            '            Fmt = Int(Fmt) + 0.5
            '        Case Is < 0.75 + 0.13
            '            Fmt = Int(Fmt) + 0.75
            '        Case Else
            '            Fmt = Int(Fmt) + 1
            '    End Select
            'End If

            If (100 - Val(txt_Folding.Text)) > 0 Then
                InvMtrs = Format(Val(TtMtrs) - Val(Fmt), "#########0.00")
            Else
                InvMtrs = Format(Val(TtMtrs) + Val(Fmt), "#########0.00")
            End If

        Else
            InvMtrs = Val(TtMtrs)

        End If

        GrsAmt = Val(InvMtrs) * Val(txt_Rate.Text)

        lbl_GrossAmount.Text = Format(Val(GrsAmt), "########0.00")

        lbl_TradeDisc_Amount.Text = Format(Val(lbl_GrossAmount.Text) * Val(txt_TradeDisc_Perc.Text) / 100, "########0.00")

        lbl_CashDisc_Amount.Text = Format(Val(lbl_GrossAmount.Text) * Val(txt_CashDisc_Perc.Text) / 100, "########0.00")

        NtAmt = Val(lbl_GrossAmount.Text) + Val(txt_PackingCharges.Text) + Val(txt_FreightCharges.Text) - Val(lbl_TradeDisc_Amount.Text) - Val(lbl_CashDisc_Amount.Text)

        lbl_Net_Amt.Text = Format(Val(NtAmt), "#########0")

        lbl_Net_Amt.Text = Common_Procedures.Currency_Format(Val(CSng(lbl_Net_Amt.Text)))

    End Sub

    'Private Sub Amount_Calculation(ByVal CurRow As Integer, ByVal CurCol As Integer)
    '    Dim fldmtr As Single = 0
    '    Dim fmt As Single = 0

    '    On Error Resume Next

    '    With dgv_Details
    '        If .Visible Then

    '            If CurCol = 4 Or CurCol = 6 Or CurCol = 7 Or CurCol = 8 Then

    '                If CurCol = 7 Or CurCol = 8 Then
    '                    If Val(.Rows(CurRow).Cells(3).Value) = 0 Or Val(.Rows(CurRow).Cells(3).Value) = 100 Or chk_No_Folding.Checked = True Then

    '                        .Rows(CurRow).Cells(9).Value = Format(Val(.Rows(CurRow).Cells(7).Value) * Val(.Rows(CurRow).Cells(8).Value), "#########0.00")

    '                    Else

    '                        fmt = ((100 - Val(.Rows(CurRow).Cells(3).Value)) / 100) * Val(.Rows(CurRow).Cells(7).Value)
    '                        fmt = Format(Math.Abs(Val(fmt)), "######0.00")

    '                        fmt = Common_Procedures.Meter_RoundOff(fmt)

    '                        If (100 - Val(.Rows(CurRow).Cells(3).Value)) > 0 Then
    '                            fldmtr = Format(Val(.Rows(CurRow).Cells(7).Value) - Val(fmt), "#########0.00")
    '                        Else
    '                            fldmtr = Format(Val(.Rows(CurRow).Cells(7).Value) + Val(fmt), "#########0.00")
    '                        End If

    '                        'fldmtr = Format(Val(.Rows(CurRow).Cells(7).Value) * Val(.Rows(CurRow).Cells(3).Value) / 100, "#########0.00")

    '                        .Rows(CurRow).Cells(9).Value = Format(Val(fldmtr) * Val(.Rows(CurRow).Cells(8).Value), "#########0.00")

    '                    End If
    '                End If

    '                Total_Calculation()

    '            End If

    '        End If
    '    End With
    'End Sub

    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotPcs As Single
        Dim TotBals As Single
        Dim TotMtrs As Single

        If NoCalc_Status = True Then Exit Sub

        Sno = 0
        TotPcs = 0 : TotBals = 0 : TotMtrs = 0

        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(4).Value) <> 0 Then

                    TotBals = TotBals + 1
                    TotPcs = TotPcs + Val(.Rows(i).Cells(3).Value())
                    TotMtrs = TotMtrs + Val(.Rows(i).Cells(4).Value())

                End If

            Next i

        End With

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(2).Value = Val(TotBals)
            .Rows(0).Cells(3).Value = Val(TotPcs)
            .Rows(0).Cells(4).Value = Format(Val(TotMtrs), "########0.00")
        End With

        If Trim(UCase(cbo_Type.Text)) <> "INTERSTATE" Then
            txt_PackingCharges.Text = Val(TotBals) * 60
        End If

        NetAmount_Calculation()

    End Sub

    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName, cbo_Type, cbo_SalesAcc, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If cbo_Type.Text = "IC" Then
                If MessageBox.Show("Do you want to select Invoice Delivery :", "FOR INVOICE DELIVERY SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then

                    btn_Selection_Click(sender, e)
                End If
            ElseIf cbo_Type.Text = "INTERSTATE" Then

                If MessageBox.Show("Do you want to select Cloth Invoice :", "FOR COTH INVOICE SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    btn_Selection_Click(sender, e)
                End If

            Else

                cbo_SalesAcc.Focus()

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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Agent, msk_LrDate, txt_CommPerc, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Agent_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Agent.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Agent, txt_CommPerc, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Agent_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
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
    Private Sub cbo_Type_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub
    Private Sub cbo_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Type, msk_Date, cbo_PartyName, "", "", "", "")

    End Sub

    Private Sub cbo_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Type, cbo_PartyName, "", "", "", "")

    End Sub
    Private Sub cbo_DespTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DespTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ic_Invoice_Head", "Despatch_To", "", "")

    End Sub
    Private Sub cbo_DespTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DespTo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DespTo, cbo_SalesAcc, cbo_Transport, "Ic_Invoice_Head", "Despatch_To", "", "")

    End Sub

    Private Sub cbo_DespTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DespTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DespTo, cbo_Transport, "Ic_Invoice_Head", "Despatch_To", "", "", False)
    End Sub

    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

    End Sub
    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, cbo_DespTo, txt_LrNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, txt_LrNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
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

    Private Sub cbo_ClothName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothName.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

    End Sub
    Private Sub cbo_ClothName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothName.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothName, txt_Folding, txt_Rate, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

    End Sub

    Private Sub cbo_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ClothName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothName, txt_Rate, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")


    End Sub

    Private Sub cbo_ClothName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ClothName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    'Private Sub cbo_Grid_Clothtype_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Clothtype.GotFocus

    '    Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothType_Head", "ClothType_Name", "", "(ClothType_IdNo = 0)")

    'End Sub
    'Private Sub cbo_Grid_Clothtype_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Clothtype.KeyDown
    '    Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Clothtype, Nothing, Nothing, "ClothType_Head", "ClothType_Name", "( ClothType_IdNo between 1 and 5 )", "(ClothType_IdNo = 0)")
    '    vcbo_KeyDwnVal = e.KeyValue

    '    With dgv_Details

    '        If (e.KeyValue = 38 And cbo_Grid_Clothtype.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
    '            .Focus()
    '            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
    '        End If

    '        If (e.KeyValue = 40 And cbo_Grid_Clothtype.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
    '            .Focus()
    '            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
    '        End If

    '    End With

    'End Sub

    'Private Sub cbo_Grid_Clothtype_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Clothtype.KeyPress
    '    Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Clothtype, Nothing, "ClothType_Head", "ClothType_Name", "( ClothType_IdNo between 1 and 5 )", "(ClothType_IdNo = 0)")

    '    If Asc(e.KeyChar) = 13 Then

    '        With dgv_Details
    '            e.Handled = True
    '            .Focus()
    '            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
    '        End With

    '    End If

    'End Sub


    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        'Dim Rect As Rectangle

        With dgv_Details

            ''If Val(.Rows(e.RowIndex).Cells(15).Value) = 0 Then
            ''    Set_Max_DetailsSlNo(e.RowIndex, 15)
            ''    'If e.RowIndex = 0 Then
            ''    '    .Rows(e.RowIndex).Cells(15).Value = 1
            ''    'Else
            ''    '    .Rows(e.RowIndex).Cells(15).Value = Val(.Rows(e.RowIndex - 1).Cells(15).Value) + 1
            ''    'End If
            ''End If

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If Trim(.CurrentRow.Cells(2).Value) = "" Then
                .CurrentRow.Cells(2).Value = Common_Procedures.ClothType_IdNoToName(con, 1)
            End If

            'If Val(.CurrentRow.Cells(3).Value) = 0 Then
            '    .CurrentRow.Cells(3).Value = "100"
            'End If

            'If e.ColumnIndex = 1 Then

            '    If cbo_ClothName.Visible = False Or Val(cbo_ClothName.Tag) <> e.RowIndex Then

            '        cbo_ClothName.Tag = -100
            '        Da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
            '        Dt1 = New DataTable
            '        Da.Fill(Dt1)
            '        cbo_ClothName.DataSource = Dt1
            '        cbo_ClothName.DisplayMember = "Cloth_Name"

            '        Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

            '        cbo_ClothName.Left = .Left + Rect.Left
            '        cbo_ClothName.Top = .Top + Rect.Top

            '        cbo_ClothName.Width = Rect.Width
            '        cbo_ClothName.Height = Rect.Height
            '        cbo_ClothName.Text = .CurrentCell.Value

            '        cbo_ClothName.Tag = Val(e.RowIndex)
            '        cbo_ClothName.Visible = True

            '        cbo_ClothName.BringToFront()
            '        cbo_ClothName.Focus()

            '    Else

            '        'If cbo_Grid_ClothName.Visible = True Then
            '        '    cbo_Grid_ClothName.BringToFront()
            '        '    cbo_Grid_ClothName.Focus()
            '        'End If

            '    End If

            'Else
            '    cbo_ClothName.Visible = False

            'End If

            'If e.ColumnIndex = 2 Then

            '    If cbo_Grid_Clothtype.Visible = False Or Val(cbo_Grid_Clothtype.Tag) <> e.RowIndex Then

            '        cbo_Grid_Clothtype.Tag = -1
            '        Da = New SqlClient.SqlDataAdapter("select ClothType_Name from ClothType_Head Where ClothType_IdNo between 1 and 5 order by ClothType_Name", con)
            '        Dt1 = New DataTable
            '        Da.Fill(Dt1)
            '        cbo_Grid_Clothtype.DataSource = Dt1
            '        cbo_Grid_Clothtype.DisplayMember = "ClothType_Name"

            '        Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

            '        cbo_Grid_Clothtype.Left = .Left + Rect.Left
            '        cbo_Grid_Clothtype.Top = .Top + Rect.Top

            '        cbo_Grid_Clothtype.Width = Rect.Width
            '        cbo_Grid_Clothtype.Height = Rect.Height
            '        cbo_Grid_Clothtype.Text = .CurrentCell.Value

            '        cbo_Grid_Clothtype.Tag = Val(e.RowIndex)
            '        cbo_Grid_Clothtype.Visible = True

            '        cbo_Grid_Clothtype.BringToFront()
            '        cbo_Grid_Clothtype.Focus()

            '    Else
            '        'If cbo_Grid_Clothtype.Visible = True Then
            '        '    cbo_Grid_Clothtype.BringToFront()
            '        '    cbo_Grid_Clothtype.Focus()
            '        'End If

            '    End If

            'Else
            '    cbo_Grid_Clothtype.Visible = False

            'End If

            'If (e.ColumnIndex = 4 Or e.ColumnIndex = 5) And Trim(UCase(cbo_Type.Text)) <> "DELIVERY" Then

            '    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

            '    pnl_BaleSelection_ToolTip.Left = .Left + Rect.Left
            '    pnl_BaleSelection_ToolTip.Top = .Top + Rect.Top + Rect.Height + 3

            '    pnl_BaleSelection_ToolTip.Visible = True

            'Else
            '    pnl_BaleSelection_ToolTip.Visible = False

            'End If


        End With

    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details
            If .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
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

                If .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Then
                    Total_Calculation()
                    'Amount_Calculation(.CurrentCell.RowIndex, .CurrentCell.ColumnIndex)

                End If

            End If
        End With

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


                    'If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Then
                    '    If Trim(UCase(cbo_Type.Text)) = "ORDER" Or Trim(UCase(cbo_Type.Text)) = "DELIVERY" Then
                    '        e.Handled = True
                    '        e.SuppressKeyPress = True
                    '    End If
                    'End If
                End If
            End If
        End With

    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress

        Try
            With dgv_Details
                If .Visible Then

                    If .Rows.Count > 0 Then

                        If Trim(UCase(cbo_Type.Text)) = "IC" Then
                            e.Handled = True
                        End If

                        If .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Then

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

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp

        Try
            With dgv_Details
                If .Rows.Count > 0 Then
                    If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
                        dgv_Details_KeyUp(sender, e)
                    End If

                    If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
                        If (.CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5) And Trim(UCase(cbo_Type.Text)) <> "DELIVERY" Then
                            ' btn_BaleSelection_Click(sender, e)
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
        'Dim n As Integer

        'If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
        '    With dgv_Details

        '        'If Val(.Rows(.CurrentCell.RowIndex).Cells(9).Value) = 0 And Val(.Rows(.CurrentCell.RowIndex).Cells(10).Value) = 0 Then

        '        n = .CurrentRow.Index

        '        If n = .Rows.Count - 1 Then
        '            For i = 0 To .ColumnCount - 1
        '                .Rows(n).Cells(i).Value = ""
        '            Next

        '        Else
        '            .Rows.RemoveAt(n)

        '        End If

        '        Total_Calculation()

        '        'End If

        '    End With

        'End If

        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    If (dgv_Details.CurrentCell.ColumnIndex = 4 Or dgv_Details.CurrentCell.ColumnIndex = 5) And Trim(UCase(cbo_Type.Text)) <> "DELIVERY" Then
        '        ' btn_BaleSelection_Click(sender, e)
        '    End If
        'End If

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

            'If Val(.Rows(e.RowIndex).Cells(15).Value) = 0 Then
            '    Set_Max_DetailsSlNo(e.RowIndex, 15)
            '    'If e.RowIndex = 0 Then
            '    '    .Rows(e.RowIndex).Cells(15).Value = 1
            '    'Else
            '    '    .Rows(e.RowIndex).Cells(15).Value = Val(.Rows(e.RowIndex - 1).Cells(15).Value) + 1
            '    'End If
            'End If

        End With
    End Sub

    Private Sub cbo_Grid_ClothName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothName.TextChanged
        Try
            If cbo_ClothName.Visible Then
                With dgv_Details
                    If Val(cbo_ClothName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_ClothName.Text)
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
                Condt = "a.Ic_Invoice_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Ic_Invoice_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Ic_Invoice_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
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
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Cloth_IdNo = " & Str(Val(Clth_IdNo))
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, c.Cloth_Name, e.Ledger_Name from Ic_Invoice_Head a left outer join Cloth_head c on a.Cloth_idno = c.Cloth_idno  left outer join Ledger_head e on a.Ledger_idno = e.Ledger_idno where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and Ic_Invoice_Code NOT like '" & Trim(Pk_Condition_GST) & "%' and a.Ic_Invoice_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.Ic_Invoice_Date, a.for_orderby, a.Ic_Invoice_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Ic_Invoice_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Ic_Invoice_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                    ' dgv_Filter_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Ic_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(6).Value = dt2.Rows(i).Item("Bale_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(7).Value = dt2.Rows(i).Item("rate").ToString
                    dgv_Filter_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(9).Value = Format(Val(dt2.Rows(i).Item("Net_Amount").ToString), "########0.00")

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
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_idno = 0)")

    End Sub
    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_ClothName, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_ClothName, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub txt_Trade_Disc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TradeDisc_Perc.KeyDown

    End Sub





    Private Sub txt_ClthDetail_Name_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_ClthDetail_Name.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If (e.KeyValue = 40) Then
            txt_Rate.Focus()
        End If
    End Sub

    Private Sub txt_ClthDetail_Name_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ClthDetail_Name.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Rate.Focus()
        End If
    End Sub

    Private Sub cbo_SalesAcc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_SalesAcc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_SalesAcc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SalesAcc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_SalesAcc, Nothing, cbo_DespTo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")
        If (e.KeyValue = 38 And cbo_SalesAcc.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(4)

            Else
                cbo_PartyName.Focus()

            End If
        End If
    End Sub

    Private Sub cbo_SalesAcc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_SalesAcc.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_SalesAcc, cbo_DespTo, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 28)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_SalesAcc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_SalesAcc.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then

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





    Private Sub txt_ClthDetail_Name_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_ClthDetail_Name.LostFocus
        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.LightBlue
                Prec_ActCtrl.ForeColor = Color.Black
            End If
        End If
    End Sub

    Private Sub txt_Cash_Disc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CashDisc_Perc.KeyPress

        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If

    End Sub

    Private Sub txt_Cash_Disc_Perc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_com_per_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CommPerc.KeyPress

        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Comm_Calc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CommAmt.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Rate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If

    End Sub

    Private Sub txt_Days_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Days.KeyDown
        If e.KeyValue = 38 Then
            txt_TradeDisc_Perc.Focus()
        End If

        If e.KeyValue = 40 Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Days_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Days.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then


            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
        End If

    End Sub

    Private Sub txt_Trade_Disc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TradeDisc_Perc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If

    End Sub



    Private Sub txt_Packingcharges_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PackingCharges.KeyPress

        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If

    End Sub

    Private Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = False
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

    Private Sub txt_Trade_Disc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_TradeDisc_Perc.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Cash_Disc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_CashDisc_Perc.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_PackingCharges_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_PackingCharges.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub lbl_GrossAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Rate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Rate.TextChanged
        AgentCommision_Calculation()
        NetAmount_Calculation()
    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim CompIDCondt As String
        Dim Ent_Pcs As Single = 0
        Dim Ent_Mtrs As Single = 0
        Dim Ent_InvDetSlNo As Long

        'If Trim(UCase(cbo_Type.Text)) <> "IC" And Trim(UCase(cbo_Type.Text)) <> "INTERSTATE" Then Exit Sub

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)

        If LedIdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT ORDER...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled And cbo_PartyName.Visible Then cbo_PartyName.Focus()
            Exit Sub
        End If

        If Trim(UCase(cbo_Type.Text)) = "IC" Then
            pnl_Ic_Selection.Visible = True
            pnl_Back.Enabled = False
            If txt_IcNo.Enabled And txt_IcNo.Visible Then txt_IcNo.Focus()
            Exit Sub
        End If

        If Trim(UCase(cbo_Type.Text)) = "INTERSTATE" Then

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
            If Val(Common_Procedures.settings.EntrySelection_Combine_AllCompany) = 1 Then
                CompIDCondt = ""
            End If

            With dgv_Cloth_DeliverySelection

                '  lbl_Heading_Selection.Text = "ORDER SELECTION"

                .Rows.Clear()
                SNo = 0

                'Da = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Cloth_Name,  g.ClothType_name,  h.Pcs as Ent_Pcs, h.Meters as Ent_DcMeters, H.ClothSales_Invoice_SlNo as Ent_ClothSales_Invoice_SlNo from ClothSales_Delivery_Head a INNER JOIN Clothsales_Delivery_details b ON a.ClothSales_Delivery_Code = b.ClothSales_Delivery_Code INNER JOIN Cloth_Head c ON b.Cloth_IdNo = c.Cloth_IdNo INNER JOIN ClothType_Head g ON b.ClothType_IdNo = g.ClothType_IdNo  LEFT OUTER JOIN ClothSales_Invoice_Details h ON h.ClothSales_Invoice_Code = '" & Trim(NewCode) & "' and b.ClothSales_Delivery_Code = h.ClothSales_Delivery_Code and b.ClothSales_Delivery_SlNo = h.ClothSales_Delivery_SlNo Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.ledger_Idno = " & Str(Val(LedIdNo)) & " and ((b.Meters - b.Invoice_Meters) > 0 or h.Meters > 0 ) order by a.ClothSales_Delivery_Date, a.for_orderby, a.ClothSales_Delivery_No", con)
                'Dt1 = New DataTable
                'Da.Fill(Dt1)

                Da = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Cloth_Name,  g.ClothType_name, h.Invoice_Pcs as Ent_Pcs, h.Invoice_Meters as Ent_DcMeters, H.Ic_Invoice_SlNo as Ent_ClothSales_Invoice_SlNo from ClothSales_Delivery_Head a INNER JOIN Clothsales_Delivery_details b ON a.ClothSales_Delivery_Code = b.ClothSales_Delivery_Code INNER JOIN Cloth_Head c ON b.Cloth_IdNo = c.Cloth_IdNo INNER JOIN ClothType_Head g ON b.ClothType_IdNo = g.ClothType_IdNo LEFT OUTER JOIN Ic_Invoice_Details h ON h.Ic_Invoice_Code = '" & Trim(NewCode) & "' and b.ClothSales_Delivery_Code = h.ClothSales_Delivery_Code and b.ClothSales_Delivery_SlNo = h.ClothSales_Delivery_SlNo Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " ((b.Meters - b.Invoice_Meters) > 0 or h.Meters > 0 ) AND A.Ledger_IdNo = '" & Str(Val(LedIdNo)) & "' order by a.ClothSales_Delivery_Date, a.for_orderby, a.ClothSales_Delivery_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        Ent_Pcs = 0
                        Ent_Mtrs = 0
                        Ent_InvDetSlNo = 0

                        If IsDBNull(Dt1.Rows(i).Item("Ent_Pcs").ToString) = False Then
                            Ent_Pcs = Val(Dt1.Rows(i).Item("Ent_Pcs").ToString)
                        End If
                        If IsDBNull(Dt1.Rows(i).Item("Ent_DcMeters").ToString) = False Then
                            Ent_Mtrs = Val(Dt1.Rows(i).Item("Ent_DcMeters").ToString)
                        End If
                        'If IsDBNull(Dt1.Rows(i).Item("Ent_ClothSales_Invoice_SlNo").ToString) = False Then
                        '    Ent_InvDetSlNo = Val(Dt1.Rows(i).Item("Ent_ClothSales_Invoice_SlNo").ToString)
                        'End If

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("ClothSales_Delivery_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                        .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("ClothType_Name").ToString
                        .Rows(n).Cells(4).Value = Val(Dt1.Rows(i).Item("Fold_Perc").ToString)
                        .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Meters").ToString), "#########0.00")
                        .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Pcs").ToString)
                        .Rows(n).Cells(7).Value = Format(Val(Dt1.Rows(i).Item("Meters").ToString) - Val(Dt1.Rows(i).Item("Invoice_Meters").ToString) + Val(Ent_Mtrs), "#########0.00")

                        If Ent_Mtrs > 0 Then
                            .Rows(n).Cells(8).Value = "1"
                            For j = 0 To .ColumnCount - 1
                                .Rows(n).Cells(j).Style.ForeColor = Color.Red
                            Next

                        Else
                            .Rows(n).Cells(8).Value = ""

                        End If


                        .Rows(n).Cells(9).Value = Ent_Pcs
                        .Rows(n).Cells(10).Value = Ent_Mtrs

                        .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Clothsales_Delivery_Code").ToString
                        .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("Clothsales_Delivery_SlNo").ToString

                        '.Rows(n).Cells(13).Value = Ent_InvDetSlNo
                        .Rows(n).Cells(13).Value = Dt1.Rows(i).Item("Despatch_To").ToString
                        .Rows(n).Cells(14).Value = Dt1.Rows(i).Item("Lr_No").ToString
                        .Rows(n).Cells(15).Value = Dt1.Rows(i).Item("Lr_Date").ToString
                        .Rows(n).Cells(16).Value = Dt1.Rows(i).Item("Agent_IdNo").ToString
                        .Rows(n).Cells(17).Value = Dt1.Rows(i).Item("Transport_IdNo").ToString
                        .Rows(n).Cells(18).Value = Dt1.Rows(i).Item("Bales_Nos").ToString
                        .Rows(n).Cells(19).Value = Dt1.Rows(i).Item("ClothSales_Delivery_dATE").ToString


                    Next

                End If
                Dt1.Clear()

            End With

            pnl_Cloth_Selection.Visible = True
            pnl_Back.Enabled = False
            If dgv_Cloth_DeliverySelection.Rows.Count > 0 Then
                dgv_Cloth_DeliverySelection.Focus()
                dgv_Cloth_DeliverySelection.CurrentCell = dgv_Cloth_DeliverySelection.Rows(0).Cells(0)
            End If

        End If

    End Sub

    Private Sub dgv_Cloth_DeliverySelection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Cloth_DeliverySelection.CellClick
        Select_ClothPiece(e.RowIndex)
    End Sub

    Private Sub Select_ClothPiece(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Cloth_DeliverySelection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(8).Value = (Val(.Rows(RwIndx).Cells(8).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(8).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next


                Else
                    .Rows(RwIndx).Cells(8).Value = ""

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
                    Next

                End If

            End If

        End With

    End Sub

    Private Sub dgv_Cloth_DeliverySelection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Cloth_DeliverySelection.KeyDown
        Dim n As Integer

        Try
            If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
                If dgv_Cloth_DeliverySelection.CurrentCell.RowIndex >= 0 Then

                    n = dgv_Cloth_DeliverySelection.CurrentCell.RowIndex

                    Select_ClothPiece(n)

                    e.Handled = True

                End If
            End If

        Catch ex As Exception
            '-----
        End Try

    End Sub

    Private Sub btn_Close_ClothDelivery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_ClothDelivery.Click
        IC_Cloth_Selection()
    End Sub

    Private Sub IC_Cloth_Selection()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim SNo As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0

        If Trim(UCase(cbo_Type.Text)) = "INTERSTATE" Then

            dgv_Details.Rows.Clear()

            For i = 0 To dgv_Cloth_DeliverySelection.RowCount - 1

                If Val(dgv_Cloth_DeliverySelection.Rows(i).Cells(8).Value) = 1 Then

                    cbo_DespTo.Text = dgv_Cloth_DeliverySelection.Rows(i).Cells(13).Value
                    txt_LrNo.Text = dgv_Cloth_DeliverySelection.Rows(i).Cells(14).Value
                    msk_LrDate.Text = dgv_Cloth_DeliverySelection.Rows(i).Cells(15).Value
                    cbo_Agent.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dgv_Cloth_DeliverySelection.Rows(i).Cells(16).Value))
                    cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dgv_Cloth_DeliverySelection.Rows(i).Cells(17).Value))
                    txt_BaleNos.Text = dgv_Cloth_DeliverySelection.Rows(i).Cells(18).Value
                    cbo_ClothName.Text = dgv_Cloth_DeliverySelection.Rows(i).Cells(2).Value
                    txt_Folding.Text = dgv_Cloth_DeliverySelection.Rows(i).Cells(4).Value
                    msk_DcDate.Text = Format(Convert.ToDateTime(dgv_Cloth_DeliverySelection.Rows(i).Cells(19).Value), "dd/MM/yyyy")

                    n = dgv_Details.Rows.Add()
                    SNo = SNo + 1
                    dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                    dgv_Details.Rows(n).Cells(1).Value = dgv_Cloth_DeliverySelection.Rows(i).Cells(1).Value
                    dgv_Details.Rows(n).Cells(2).Value = dgv_Cloth_DeliverySelection.Rows(i).Cells(3).Value
                    dgv_Details.Rows(n).Cells(5).Value = dgv_Cloth_DeliverySelection.Rows(i).Cells(11).Value
                    dgv_Details.Rows(n).Cells(6).Value = dgv_Cloth_DeliverySelection.Rows(i).Cells(12).Value


                    dgv_Details.Rows(n).Cells(8).Value = dgv_Cloth_DeliverySelection.Rows(i).Cells(2).Value
                    dgv_Details.Rows(n).Cells(9).Value = dgv_Cloth_DeliverySelection.Rows(i).Cells(4).Value


                    If Val(dgv_Cloth_DeliverySelection.Rows(i).Cells(9).Value) <> 0 Then
                        dgv_Details.Rows(n).Cells(3).Value = dgv_Cloth_DeliverySelection.Rows(i).Cells(9).Value
                    Else
                        dgv_Details.Rows(n).Cells(3).Value = dgv_Cloth_DeliverySelection.Rows(i).Cells(6).Value
                    End If

                    If Val(dgv_Cloth_DeliverySelection.Rows(i).Cells(10).Value) <> 0 Then
                        dgv_Details.Rows(n).Cells(4).Value = dgv_Cloth_DeliverySelection.Rows(i).Cells(10).Value
                    Else
                        dgv_Details.Rows(n).Cells(4).Value = dgv_Cloth_DeliverySelection.Rows(i).Cells(7).Value
                    End If

                End If

            Next

        End If
        Total_Calculation()

        pnl_Back.Enabled = True
        pnl_Cloth_Selection.Visible = False

        If dgv_Details.Rows.Count > 0 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(3)

        Else
            If cbo_SalesAcc.Enabled And cbo_SalesAcc.Visible Then cbo_SalesAcc.Focus()

        End If

    End Sub

    Private Sub cbo_Type_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.TextChanged
        If Trim(UCase(cbo_Type.Text)) <> "IC" And Trim(UCase(cbo_Type.Text)) <> "INTERSTATE" Then
            dgv_Details.AllowUserToAddRows = True
        Else
            dgv_Details.AllowUserToAddRows = False
        End If
    End Sub

    Private Sub btn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel.Click
        ' pnl_Selection.Visible = True
        pnl_Ic_Selection.Visible = False
        pnl_Back.Enabled = True
    End Sub

    Private Sub btn_Accept_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Accept.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer, nr As Integer

        Dim NewCode As String
        Dim CompIDCondt As String
        Dim BaleNoCondt As String = ""

        If Trim(UCase(cbo_Type.Text)) <> "IC" And Trim(UCase(cbo_Type.Text)) <> "INTERSTATE" Then Exit Sub

        'If Trim(txt_IcNo.Text) = "" Then
        '    MessageBox.Show("Invalid Ic No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If txt_IcNo.Enabled And txt_IcNo.Visible Then txt_IcNo.Focus()
        '    Exit Sub
        'End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Trim(UCase(Common_Procedures.settings.CompanyName)) = "-----~~~" Then
            CompIDCondt = ""
        End If

        If Trim(txt_IcNo.Text) <> "" Then
            BaleNoCondt = Trim(BaleNoCondt) & IIf(Trim(BaleNoCondt) <> "", " and ", "") & " b.Ic_No = '" & Trim(txt_IcNo.Text) & "'"
        End If

        If Trim(UCase(cbo_Type.Text)) = "IC" Then


            With dgv_Selection

                'lbl_Heading_Selection.Text = "DELIVERY SELECTION"

                .Rows.Clear()
                SNo = 0

                Da = New SqlClient.SqlDataAdapter("select a.*, b.* from Ic_Delivery_Details a  INNER JOIN Ic_Delivery_Head b ON a.Ic_Delivery_Code = b.Ic_Delivery_Code LEFT OUTER JOIN Ic_Invoice_Details c ON a.Ic_Delivery_Code = c.Ic_Delivery_Code and a.Ic_Delivery_SlNo = c.Ic_Delivery_SlNo where " & BaleNoCondt & IIf(BaleNoCondt <> "", " and ", "") & " a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Ic_Invoice_Code = '" & Trim(NewCode) & "' and a.Bale_No = c.Bale_No order by a.for_orderby, a.Ic_Delivery_Code,  a.sl_no", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Ic_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Bale_No").ToString
                        .Rows(n).Cells(3).Value = Val(Dt1.Rows(i).Item("Pcs").ToString)
                        .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Meters").ToString), "#########0.00")
                        .Rows(n).Cells(5).Value = "1"
                        .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Ic_Delivery_Code").ToString
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Ic_Delivery_SlNo").ToString

                        For j = 0 To .ColumnCount - 1
                            .Rows(i).Cells(j).Style.ForeColor = Color.Red
                            If Val(.Rows(n).Cells(5).Value) <> Val(.Rows(n).Cells(5).Value) Then
                                .Rows(i).Cells(j).Style.BackColor = Color.Gray
                            End If
                        Next

                    Next

                End If
                Dt1.Clear()

                Da = New SqlClient.SqlDataAdapter("select a.*, b.* from Ic_Delivery_Details a  INNER JOIN Ic_Delivery_Head b ON a.Ic_Delivery_Code = b.Ic_Delivery_Code LEFT OUTER JOIN Ic_Invoice_Details c ON a.Ic_Delivery_Code = c.Ic_Delivery_Code and a.Ic_Delivery_SlNo = c.Ic_Delivery_SlNo where " & BaleNoCondt & IIf(BaleNoCondt <> "", " and ", "") & " a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Ic_Invoice_Code = '' order by a.for_orderby, a.Ic_Delivery_Code,  a.sl_no", con)
                Dt1 = New DataTable
                nr = Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Ic_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Bale_No").ToString
                        .Rows(n).Cells(3).Value = Val(Dt1.Rows(i).Item("Pcs").ToString)
                        .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Meters").ToString), "#########0.00")
                        .Rows(n).Cells(5).Value = ""
                        .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Ic_Delivery_Code").ToString
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Ic_Delivery_SlNo").ToString

                    Next

                End If
                Dt1.Clear()

            End With

            pnl_Selection.Visible = True
            pnl_Ic_Selection.Visible = False
            If dgv_Selection.Rows.Count > 0 Then
                dgv_Selection.Focus()
                dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
            End If

        End If
    End Sub
    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_Piece(e.RowIndex)
    End Sub

    Private Sub Select_Piece(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(5).Value = (Val(.Rows(RwIndx).Cells(5).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(5).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next


                Else
                    .Rows(RwIndx).Cells(5).Value = ""

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

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_BaleSelection.Click
        IC_Invoice_Selection()
    End Sub

    Private Sub IC_Invoice_Selection()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim SNo As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim IcDcNo As String = ""

        Dim Cmd As New SqlClient.SqlCommand
        Dim Da2 As SqlClient.SqlDataAdapter
        Dim Dt2 As New DataTable
        Dim dgvDet_CurRow As Integer = 0
        Dim FsNo As Single, LsNo As Single
        Dim FsBaleNo As String, LsBaleNo As String
        Dim BlNo As String

        If Trim(UCase(cbo_Type.Text)) = "IC" Then

            cbo_ClothName.Text = ""
            txt_Folding.Text = ""
            cbo_Transport.Text = ""
            txt_LrNo.Text = ""
            msk_LrDate.Text = ""
            cbo_DespTo.Text = ""
            dgv_Details.Rows.Clear()

            For i = 0 To dgv_Selection.RowCount - 1

                If Val(dgv_Selection.Rows(i).Cells(5).Value) = 1 Then

                    If Trim(IcDcNo) = "" Then IcDcNo = dgv_Selection.Rows(i).Cells(6).Value

                    n = dgv_Details.Rows.Add()
                    SNo = SNo + 1
                    dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                    dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(1).Value
                    dgv_Details.Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(2).Value
                    dgv_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(3).Value
                    dgv_Details.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(4).Value
                    dgv_Details.Rows(n).Cells(5).Value = ""
                    dgv_Details.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(6).Value
                    dgv_Details.Rows(n).Cells(10).Value = dgv_Selection.Rows(i).Cells(7).Value

                    'Amount_Calculation(n, 7)

                End If

            Next

        End If

        If Trim(IcDcNo) <> "" Then

            Da1 = New SqlClient.SqlDataAdapter("Select a.*, b.cloth_name, c.ledger_name as TransportName from Ic_Delivery_Head a INNER JOIN cloth_head b ON a.Cloth_Idno = b.Cloth_Idno LEFT OUTER JOIN ledger_head c ON a.Transport_Idno = c.ledger_idno Where a.Ic_Delivery_Code = '" & Trim(IcDcNo) & "'", con)
            Dt1 = New DataTable
            Da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                cbo_ClothName.Text = Dt1.Rows(0).Item("cloth_name").ToString
                txt_Folding.Text = 100
                If Dt1.Rows(0).Item("TransportName").ToString <> "" Then
                    cbo_Transport.Text = Dt1.Rows(0).Item("TransportName").ToString
                End If
                txt_LrNo.Text = Dt1.Rows(0).Item("Lr_No").ToString
                cbo_DespTo.Text = Dt1.Rows(0).Item("Despatch_To").ToString
                If Dt1.Rows(0).Item("Lr_Date").ToString <> "" Then
                    msk_LrDate.Text = Dt1.Rows(0).Item("Lr_Date").ToString
                End If

            End If
            Dt1.Clear()

        End If

        Cmd.Connection = con

        Cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
        Cmd.ExecuteNonQuery()

        For i = 0 To dgv_Details.Rows.Count - 1

            If Trim(dgv_Details.Rows(i).Cells(2).Value) <> "" And Val(dgv_Details.Rows(i).Cells(4).Value) <> 0 Then

                Cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Meters1) values ('" & Trim(dgv_Details.Rows(i).Cells(2).Value) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(dgv_Details.Rows(i).Cells(2).Value))) & " ) "
                Cmd.ExecuteNonQuery()

            End If

        Next


        BlNo = ""
        FsNo = 0 : LsNo = 0
        FsBaleNo = "" : LsBaleNo = ""

        Da2 = New SqlClient.SqlDataAdapter("Select Name1 as Bale_No, Meters1 as fororderby_baleno from " & Trim(Common_Procedures.EntryTempTable) & " where Name1 <> '' order by Meters1, Name1", con)
        Dt2 = New DataTable
        Da2.Fill(Dt2)

        If Dt2.Rows.Count > 0 Then

            FsNo = Val(Dt2.Rows(0).Item("fororderby_baleno").ToString)
            LsNo = Val(Dt2.Rows(0).Item("fororderby_baleno").ToString)

            FsBaleNo = Trim(UCase(Dt2.Rows(0).Item("Bale_No").ToString))
            LsBaleNo = Trim(UCase(Dt2.Rows(0).Item("Bale_No").ToString))

            For i = 1 To Dt2.Rows.Count - 1
                If LsNo + 1 = Val(Dt2.Rows(i).Item("fororderby_baleno").ToString) Then
                    LsNo = Val(Dt2.Rows(i).Item("fororderby_baleno").ToString)
                    LsBaleNo = Trim(UCase(Dt2.Rows(i).Item("Bale_No").ToString))

                Else
                    If FsNo = LsNo Then
                        BlNo = BlNo & Trim(FsBaleNo) & ","
                    Else
                        BlNo = BlNo & Trim(FsBaleNo) & "-" & Trim(LsBaleNo) & ","
                    End If
                    FsNo = Dt2.Rows(i).Item("fororderby_baleno").ToString
                    LsNo = Dt2.Rows(i).Item("fororderby_baleno").ToString

                    FsBaleNo = Trim(UCase(Dt2.Rows(i).Item("Bale_No").ToString))
                    LsBaleNo = Trim(UCase(Dt2.Rows(i).Item("Bale_No").ToString))

                End If

            Next

            If FsNo = LsNo Then BlNo = BlNo & Trim(FsBaleNo) Else BlNo = BlNo & Trim(FsBaleNo) & "-" & Trim(LsBaleNo)

        End If
        Dt2.Clear()

        Dt2.Dispose()
        Da2.Dispose()

        txt_BaleNos.Text = BlNo

        Total_Calculation()

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        If cbo_SalesAcc.Enabled And cbo_SalesAcc.Visible Then cbo_SalesAcc.Focus()

    End Sub

    Private Sub txt_IcNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_IcNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            'If MessageBox.Show("Do you want to select Invoice Delivery :", "FOR INVOICE DELIVERY SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
            btn_Accept_Click(sender, e)
            'End If
        End If
    End Sub

    Private Sub txt_FreightCharges_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_FreightCharges.TextChanged
        NetAmount_Calculation()
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim ps As Printing.PaperSize

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Ic_Invoice_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Ic_Invoice_Code = '" & Trim(NewCode) & "'", con)
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

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            Try

                If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                    PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
                        PrintDocument1.Print()
                    End If

                Else
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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_InvNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Ledger_Name as TransportName , e.Ledger_Name as Agent_Name , f.Cloth_name , f.Cloth_Description, g.Count_Name from Ic_Invoice_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo Left outer JOIN Ledger_Head c ON c.Ledger_IdNo = a.Ledger_IdNo Left outer JOIN Ledger_Head d ON a.Transport_IdNo = d.Ledger_IdNo Left outer JOIN Ledger_Head e ON a.Agent_IdNo = e.Ledger_IdNo INNER JOIN Cloth_Head f ON a.Cloth_idno = f.Cloth_idno LEFT OUTER JOIN Count_Head g ON f.Cloth_WarpCount_IdNo = g.Count_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Ic_Invoice_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_name , b.Cloth_Description from Ic_Invoice_Details a INNER JOIN Cloth_Head b ON a.Cloth_idno = b.Cloth_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Ic_Invoice_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
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
        Dim pFont As Font, pFont1 As Font, p1Font As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurX As Single = 0
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        'Dim ItmDesc1 As String, ItmDesc2 As String
        Dim ps As Printing.PaperSize
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim NetBilTxt As String = ""
        Dim W1 As Single
        Dim flperc As Single = 0
        Dim flmtr As Single = 0
        Dim fmtr As Single = 0
        Dim BInc As Integer
        Dim BnkDetAr() As String
        Dim Cmp_Name As String = "", Cmp_EMail As String = ""
        Dim Z1 As Single = 0
        Dim BmsInWrds As String
        Dim vprn_BlNos As String = ""
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim BankNm5 As String = ""
        Dim BankNm6 As String = ""
        Dim BankNm7 As String = ""
        Dim BankNm8 As String = ""
        Dim rndoff As Single, TtAmt As Single
        Dim BLNo1 As String, BLNo2 As String
        Dim Dup_SetNoBmNo As String = ""

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 0 ' 65
            .Right = 0 ' 50
            .Top = 20 ' 65
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
        NoofItems_PerPage = 10

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

                CurX = LMargin + 45 ' 40  '150
                CurY = TMargin + 190 ' 122 ' 100
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "TO   " & "M/s. " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, CurX, CurY, 0, 0, p1Font)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, CurX + 10, CurY, 0, 0, pFont)

                If Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, CurX + 10, CurY, 0, 0, pFont)
                End If
                If Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, CurX + 10, CurY, 0, 0, pFont)
                End If
                If Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, CurX + 10, CurY, 0, 0, pFont)
                End If

                If Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString) <> "" Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "Tin No : " & prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, CurX + 10, CurY, 0, 0, pFont)
                End If

                'If Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString) <> "" Then
                '    CurY = CurY + TxtHgt
                '    Common_Procedures.Print_To_PrintDocument(e, "Ph.No : " & prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString, CurX, CurY, 0, 0, pFont)
                'End If
                W1 = e.Graphics.MeasureString("INVOICE DATE : ", pFont).Width

                CurX = LMargin + 500
                CurY = TMargin + 180
                p1Font = New Font("Calibri", 14, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "Dc.Date ", CurX, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", CurX + W1, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Dc_Date").ToString, CurX + W1 + 10, CurY, 0, 0, pFont)

                CurX = LMargin + 500
                CurY = CurY + TxtHgt
                p1Font = New Font("Calibri", 14, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "Invoice No  ", CurX, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", CurX + W1, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ic_Invoice_No").ToString, CurX + W1 + 10, CurY, 0, 0, pFont)

                CurX = LMargin + 500
                CurY = CurY + TxtHgt
                p1Font = New Font("Calibri", 14, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "Invoice Date ", CurX, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", CurX + W1, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Ic_Invoice_Date").ToString), "dd-MM-yyyy"), CurX + W1 + 10, CurY, 0, 0, pFont)

                CurX = LMargin + 500
                CurY = CurY + TxtHgt
                p1Font = New Font("Calibri", 14, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "Bales Nos ", CurX, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", CurX + W1, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Bale_No").ToString, CurX + W1 + 10, CurY, 0, 0, pFont)

                CurX = LMargin + 500
                CurY = CurY + TxtHgt
                p1Font = New Font("Calibri", 14, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "To ", CurX, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", CurX + W1, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Despatch_To").ToString, CurX + W1 + 10, CurY, 0, 0, pFont)


                vprn_BlNos = ""
                For I = 0 To prn_DetDt.Rows.Count - 1
                    If InStr(1, Trim(UCase(Dup_SetNoBmNo)), "~" & Trim(UCase(prn_DetDt.Rows(I).Item("Ic_No").ToString)) & "~") > 0 Then
                        Dup_SetNoBmNo = Trim(UCase(prn_DetDt.Rows(I).Item("Ic_No").ToString))

                    Else
                        Dup_SetNoBmNo = Trim(UCase(prn_DetDt.Rows(I).Item("Ic_No").ToString))
                        vprn_BlNos = Trim(vprn_BlNos) & IIf(Trim(vprn_BlNos) <> "", ", ", "") & Dup_SetNoBmNo
                    End If

                    Dup_SetNoBmNo = Trim(Dup_SetNoBmNo) & "~" & Trim(UCase(prn_DetDt.Rows(I).Item("Ic_No").ToString)) & "~"
                Next

                If Trim(vprn_BlNos) = "" Then vprn_BlNos = prn_DetDt.Rows(0).Item("Ic_No").ToString
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

                CurX = LMargin + 500
                CurY = CurY + TxtHgt
                p1Font = New Font("Calibri", 14, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "Ic No ", CurX, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", CurX + W1, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, BLNo1, CurX + W1 + 10, CurY, 0, 0, pFont)


                CurX = LMargin + 500
                CurY = CurY + TxtHgt
                p1Font = New Font("Calibri", 14, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "Lr.No :" & prn_HdDt.Rows(0).Item("Lr_No").ToString & " Lr.Date :" & prn_HdDt.Rows(0).Item("Lr_Date").ToString, CurX, CurY, 0, 0, pFont)

                CurX = LMargin + 45
                CurY = TMargin + 300
                p1Font = New Font("Calibri", 14, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "Transport :" & prn_HdDt.Rows(0).Item("TransportName").ToString, CurX, CurY, 0, 0, pFont)

                If prn_HdDt.Rows.Count > 0 Then

                    Try

                        NoofDets = 0

                        CurY = TMargin + 380 ' 370

                        CurY = CurY + 5
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cloth_Details").ToString, LMargin + 100, CurY, 0, 0, pFont)

                        CurY = CurY + 10

                        If Trim(prn_HdDt.Rows(0).Item("Cloth_Description").ToString) <> "" Then
                            ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Cloth_Description").ToString)
                        Else
                            ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Cloth_Name").ToString)
                        End If
                        ItmNm2 = ""
                        If Len(ItmNm1) > 35 Then
                            For I = 20 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 35
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If


                        CurY = CurY + TxtHgt

                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + 100, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Count_Name").ToString, LMargin + 45, CurY, 0, 0, pFont)

                        If Val(prn_HdDt.Rows(0).Item("Folding").ToString) = 0 Or Val(prn_HdDt.Rows(0).Item("Folding").ToString) = 100 Then
                            CurX = LMargin + 490
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Meters").ToString, CurX, CurY, 1, 0, pFont)
                            CurX = LMargin + 580
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rate").ToString, CurX, CurY, 1, 0, pFont)
                            CurX = LMargin + 730
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Gross_Amount").ToString, CurX, CurY, 1, 0, pFont)

                        Else

                            CurX = LMargin + 490
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Total_Meters").ToString, CurX, CurY, 1, 0, pFont)

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                CurX = LMargin + 100
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), CurX, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            flperc = 100 - Val(prn_HdDt.Rows(0).Item("Folding").ToString)

                            flmtr = Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) * flperc / 100, "#########0.00")

                            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1035" Then
                                flmtr = Math.Abs(Val(flmtr))
                                flmtr = Common_Procedures.Meter_RoundOff(flmtr)
                            End If

                            CurY = CurY + TxtHgt
                            CurX = LMargin + 100

                            If Val(flperc) > 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Val(flperc) & "%  Folding Less", CurX, CurY, 0, 0, pFont)
                            Else
                                Common_Procedures.Print_To_PrintDocument(e, Val(flperc) & "%  Folding Add", CurX, CurY, 0, 0, pFont)
                            End If

                            CurX = LMargin + 490
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(flmtr), "#######0.00"), CurX, CurY, 1, 0, pFont)

                            CurY = CurY + TxtHgt + 2
                            CurX = LMargin + 380
                            e.Graphics.DrawLine(Pens.Black, CurX, CurY, CurX + 100, CurY)

                            If Val(flperc) > 0 Then
                                fmtr = Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) - Val(flmtr), "#########0.00")
                            Else
                                fmtr = Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) + Val(flmtr), "#########0.00")
                            End If

                            CurY = CurY + 5
                            CurX = LMargin + 490
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(fmtr), "#######0.00"), CurX, CurY, 1, 0, pFont)
                            CurX = LMargin + 580
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rate").ToString, CurX, CurY, 1, 0, pFont)
                            CurX = LMargin + 730
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Gross_Amount").ToString, CurX, CurY, 1, 0, pFont)

                        End If

                    Catch ex As Exception

                        MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                    End Try
                End If

                For I = NoofDets + 1 To NoofItems_PerPage
                    CurY = CurY + TxtHgt
                Next

                CurY = CurY + 15
                'CurY = CurY + 10

                If Val(prn_HdDt.Rows(0).Item("Gross_Amount").ToString) <> 0 Then
                    CurX = LMargin + 370
                    Common_Procedures.Print_To_PrintDocument(e, "GROSS VALUE", CurX, TMargin + 570, 0, 0, pFont)
                    'Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Trade_Discount").ToString) & "%", CurX + 180, CurY, 0, 0, pFont)
                    'Common_Procedures.Print_To_PrintDocument(e, "(-)", CurX + 250, CurY, 0, 0, pFont)
                    CurX = LMargin + 730
                    Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Gross_Amount").ToString), CurX, TMargin + 570, 1, 0, pFont)
                End If

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

                    BInc = BInc + 1
                    If UBound(BnkDetAr) >= BInc Then
                        BankNm6 = Trim(BnkDetAr(BInc))
                    End If

                    BInc = BInc + 1
                    If UBound(BnkDetAr) >= BInc Then
                        BankNm7 = Trim(BnkDetAr(BInc))
                    End If

                    BInc = BInc + 1
                    If UBound(BnkDetAr) >= BInc Then
                        BankNm8 = Trim(BnkDetAr(BInc))
                    End If
                End If


                CurY = CurY + TxtHgt - 50
                If prn_HdDt.Rows(0).Item("Agent_Name").ToString <> "" Then
                    CurX = LMargin + 45
                    Common_Procedures.Print_To_PrintDocument(e, "AGENT : " & Trim(prn_HdDt.Rows(0).Item("Agent_Name").ToString), CurX, CurY, 0, 0, pFont)
                Else
                    CurX = LMargin + 45
                    Common_Procedures.Print_To_PrintDocument(e, "AGENT : DIRECT", CurX, CurY, 0, 0, pFont)
                End If

                If Val(prn_HdDt.Rows(0).Item("Trade_Discount").ToString) <> 0 Then
                    CurX = LMargin + 370
                    Common_Procedures.Print_To_PrintDocument(e, "TRADE DISCOUNT ", CurX, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Trade_Discount_Perc").ToString) & "%", CurX + 180, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "(-)", CurX + 250, CurY, 0, 0, pFont)
                    CurX = LMargin + 730
                    Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Trade_Discount").ToString), CurX, CurY, 1, 0, pFont)
                End If

                CurY = CurY + TxtHgt
                p1Font = New Font("Calibri", 10, FontStyle.Bold)
                If BankNm1 <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, BankNm1 & "," & BankNm2, LMargin + 45, CurY, 0, 0, p1Font)

                End If
                If Val(prn_HdDt.Rows(0).Item("Cash_Discount").ToString) <> 0 Then
                    CurX = LMargin + 370
                    Common_Procedures.Print_To_PrintDocument(e, "CASH DISCOUNT", CurX, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Cash_Discount_Perc").ToString) & "%", CurX + 180, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "(-)", CurX + 250, CurY, 0, 0, pFont)
                    CurX = LMargin + 730
                    Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Cash_Discount").ToString), CurX, CurY, 1, 0, pFont)
                End If

                CurY = CurY + TxtHgt

                p1Font = New Font("Calibri", 10, FontStyle.Bold)
                If BankNm3 <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, BankNm3 & "," & BankNm4, LMargin + 45, CurY - 5, 0, 0, p1Font)

                End If
                If Val(prn_HdDt.Rows(0).Item("Packing_Charges").ToString) <> 0 Then
                    CurX = LMargin + 370
                    Common_Procedures.Print_To_PrintDocument(e, "PACKING CHARGE ", CurX, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "(+)", CurX + 250, CurY, 0, 0, pFont)
                    CurX = LMargin + 730
                    Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Packing_Charges").ToString), CurX, CurY, 1, 0, pFont)
                End If

                CurY = CurY + TxtHgt
                p1Font = New Font("Calibri", 10, FontStyle.Bold)
                If BankNm5 <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, BankNm5 & "," & BankNm6, LMargin + 45, CurY, 0, 0, p1Font)

                End If
                If Val(prn_HdDt.Rows(0).Item("Freight_Charges").ToString) <> 0 Then
                    CurX = LMargin + 370
                    Common_Procedures.Print_To_PrintDocument(e, "FREIGHT CHARGE ", CurX, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "(+)", CurX + 250, CurY, 0, 0, pFont)
                    CurX = LMargin + 730
                    Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Freight_Charges").ToString), CurX, CurY, 1, 0, pFont)
                End If

                CurY = CurY + TxtHgt
                p1Font = New Font("Calibri", 10, FontStyle.Bold)
                If BankNm7 <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, BankNm7 & "," & BankNm8, LMargin + 45, CurY - 5, 0, 0, p1Font)
                End If

                TtAmt = Val(prn_HdDt.Rows(0).Item("Gross_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Freight_Charges").ToString) + Val(prn_HdDt.Rows(0).Item("Packing_Charges").ToString) - Val(prn_HdDt.Rows(0).Item("Trade_Discount").ToString) - Val(prn_HdDt.Rows(0).Item("Cash_Discount").ToString)

                rndoff = 0
                rndoff = Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) - Val(TtAmt)

                ' CurY = CurY + TxtHgt - 5

                If Val(rndoff) <> 0 Then
                    CurX = LMargin + 370
                    Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF", CurX, CurY, 0, 0, pFont)
                    If Val(rndoff) >= 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "(+)", CurX + 250, CurY, 0, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "(-)", CurX + 250, CurY, 0, 0, pFont)
                    End If
                    CurX = LMargin + 730
                    Common_Procedures.Print_To_PrintDocument(e, " " & Format(Math.Abs(Val(rndoff)), "########0.00"), CurX, CurY, 1, 0, pFont)
                End If

                CurY = CurY + TxtHgt
                p1Font = New Font("Calibri", 10, FontStyle.Bold)
                ' Common_Procedures.Print_To_PrintDocument(e, BankNm5, LMargin + 45, CurY, 0, 0, p1Font)

                CurY = CurY + TxtHgt ' 10
                p1Font = New Font("Calibri", 10, FontStyle.Bold)
                '  Common_Procedures.Print_To_PrintDocument(e, BankNm6, LMargin + 45, CurY - 5, 0, 0, p1Font)

                CurX = LMargin + 730
                CurY = TMargin + 820
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Net_Amount").ToString), CurX, CurY, 1, 0, p1Font)
                'If Val(prn_HdDt.Rows(0).Item("Gr_Time").ToString) <> 0 Then
                p1Font = New Font("Calibri", 10, FontStyle.Bold)
                '    Common_Procedures.Print_To_PrintDocument(e, "Due Date : " & Trim(prn_HdDt.Rows(0).Item("Gr_Time").ToString) & " Days " & "(" & Trim(prn_HdDt.Rows(0).Item("Gr_Date").ToString) & ")", LMargin + 10, CurY, 0, 0, p1Font)
                'End If

                CurY = TMargin + 880
                CurX = LMargin + 440
                BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
                'BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "", "")

                Common_Procedures.Print_To_PrintDocument(e, BmsInWrds, LMargin + 140, CurY, 0, 0, p1Font)

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub
    Private Sub AgentCommision_Calculation()
        Dim tlamt As Single
        Dim tlmtr As Single
        With dgv_Details_Total


            tlamt = 0
            tlmtr = 0
            With dgv_Details_Total
                If .Rows.Count > 0 Then
                    tlamt = (Val(lbl_GrossAmount.Text))
                    tlmtr = (Val(.Rows(0).Cells(4).Value))

                End If
            End With

            If Trim(UCase(cbo_Com_Type.Text)) = "MTR" Then

                txt_CommAmt.Text = Format(Val(tlmtr) * Val(txt_CommPerc.Text), "########0.00")

            Else

                txt_CommAmt.Text = Format(Val(tlamt) * Val(txt_CommPerc.Text) / 100, "########0.00")

            End If

        End With
    End Sub
    Private Sub cbo_Com_Type_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Com_Type.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub

    Private Sub cbo_Com_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Com_Type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Com_Type, txt_CommPerc, msk_DcDate, "", "", "", "")

    End Sub

    Private Sub cbo_Com_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Com_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Com_Type, msk_DcDate, "", "", "", "")
    End Sub

    Private Sub txt_CommPerc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_CommPerc.TextChanged
        AgentCommision_Calculation()
    End Sub

    Private Sub cbo_Com_Type_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Com_Type.TextChanged
        AgentCommision_Calculation()
    End Sub

    Private Sub msk_Date_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles msk_Date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_Date.Text = Date.Today
            msk_Date.SelectionStart = 0
        End If
    End Sub

    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyUp
        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    msk_Date.Text = Date.Today
        'End If
        If IsDate(msk_Date.Text) = True Then
            If e.KeyCode = 107 Then
                msk_Date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_Date.Text))
            ElseIf e.KeyCode = 109 Then
                msk_Date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_Date.Text))
            End If
        End If
        If e.KeyCode = 46 Or e.KeyCode = 8 Then

            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)
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
    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        If IsDate(dtp_Date.Text) = True Then
            msk_Date.Text = dtp_Date.Text
            msk_Date.SelectionStart = 0
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



    Private Sub msk_DcDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_DcDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            msk_DcDate.Text = Date.Today
        End If
        If IsDate(msk_DcDate.Text) = True Then
            If e.KeyCode = 107 Then
                msk_DcDate.Text = DateAdd("D", 1, Convert.ToDateTime(msk_DcDate.Text))
            ElseIf e.KeyCode = 109 Then
                msk_DcDate.Text = DateAdd("D", -1, Convert.ToDateTime(msk_DcDate.Text))
            End If
        End If
        If e.KeyCode = 46 Or e.KeyCode = 8 Then

            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskDcText, vmskDcStrt)
        End If

    End Sub
    Private Sub msk_DcDate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_DcDate.LostFocus

        If IsDate(msk_DcDate.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_DcDate.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_DcDate.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_DcDate.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_DcDate.Text)) >= 2000 Then
                    dtp_DcDate.Value = Convert.ToDateTime(msk_DcDate.Text)
                End If
            End If

        End If
    End Sub

    Private Sub dtp_DcDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_DcDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_DcDate.Text = Date.Today
        End If
    End Sub
    Private Sub dtp_DcDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_DcDate.TextChanged
        If IsDate(dtp_DcDate.Text) = True Then
            msk_DcDate.Text = dtp_DcDate.Text
            msk_DcDate.SelectionStart = 0
        End If
    End Sub
    Private Sub msk_DcDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_DcDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        vmskDcText = ""
        vmskDcStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskDcText = msk_DcDate.Text
            vmskDcStrt = msk_DcDate.SelectionStart
        End If

    End Sub

    Private Sub msk_LrDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_LrDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            msk_LrDate.Text = Date.Today
        End If
        If IsDate(msk_LrDate.Text) = True Then
            If e.KeyCode = 107 Then
                msk_LrDate.Text = DateAdd("D", 1, Convert.ToDateTime(msk_LrDate.Text))
            ElseIf e.KeyCode = 109 Then
                msk_LrDate.Text = DateAdd("D", -1, Convert.ToDateTime(msk_LrDate.Text))
            End If
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then

            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskLrText, vmskLrStrt)
        End If

    End Sub
    Private Sub msk_LrDate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_LrDate.LostFocus

        If IsDate(msk_LrDate.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_LrDate.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_LrDate.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_LrDate.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_LrDate.Text)) >= 2000 Then
                    dtp_LrDate.Value = Convert.ToDateTime(msk_LrDate.Text)
                End If
            End If

        End If
    End Sub

    Private Sub dtp_LrDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_LrDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_LrDate.Text = Date.Today
        End If
    End Sub
    Private Sub dtp_LrDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_LrDate.TextChanged
        If IsDate(dtp_LrDate.Text) = True Then
            msk_LrDate.Text = dtp_LrDate.Text
            msk_LrDate.SelectionStart = 0
        End If
    End Sub
    Private Sub msk_LrDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_LrDate.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        vmskLrText = ""
        vmskLrStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskLrText = msk_LrDate.Text
            vmskLrStrt = msk_LrDate.SelectionStart
        End If

    End Sub

    Private Sub btn_SaveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SaveAll.Click
        Dim pwd As String = ""

        Dim g As New Password
        g.ShowDialog()

        pwd = Trim(Common_Procedures.Password_Input)

        If Trim(UCase(pwd)) <> "TSSA7417" Then
            MessageBox.Show("Invalid Password", "FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

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
            MessageBox.Show("All entries saved sucessfully", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Else
            movenext_record()

        End If
    End Sub

End Class