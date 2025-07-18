Public Class Cloth_Purchase_Entry

    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "CLPUR-"
    Private Pk_Condition2 As String = "CPAGC-"
    Private Pk_Condition3 As String = "CPREC-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
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

    Private SaveAll_STS As Boolean = False
    Private LastNo As String = ""
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1


    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False

        vmskOldText = ""
        vmskSelStrt = -1

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        msk_date.Text = ""
        dtp_Date.Text = ""
        cbo_PartyName.Text = ""
        cbo_Type.Text = "DIRECT"
        cbo_Agent.Text = ""
        cbo_Com_Type.Text = "%"

        cbo_PurchaseAc.Text = ""

        cbo_Transport.Text = ""
        cbo_Grid_ClothName.Text = ""
        cbo_Grid_Clothtype.Text = ""

        txt_BillNo.Text = ""
        txt_com_per.Text = ""
        txt_CommAmt.Text = ""
        txt_RecNo.Text = ""
        txt_RecDate.Text = ""
        txt_Bundles.Text = ""
        txt_Pcs.Text = ""
        txt_ReceiptMeters.Text = ""
        txt_Note.Text = ""
        txt_ExcShtMeters.Text = ""
        txt_Vechile.Text = ""
        lbl_TotalMeters.Text = ""
        txt_ReturnMeters.Text = ""
        txt_Cash_Disc.Text = ""
        lbl_Cash_Disc_Perc.Text = ""
        txt_Trade_Disc.Text = ""
        lbl_Trade_Disc_Perc.Text = ""
        txt_Packing.Text = ""
        lbl_Net_Amt.Text = ""
        txt_Freight.Text = ""
        txt_Insurance.Text = ""

        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))

        chk_No_Folding.Checked = False

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

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
        cbo_Grid_ClothName.Tag = -100
        cbo_Grid_Clothtype.Visible = False
        cbo_Grid_Clothtype.Tag = -100

        NoCalc_Status = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim Msktxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is MaskedTextBox Then
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

        If Me.ActiveControl.Name <> cbo_Grid_ClothName.Name Then
            cbo_Grid_ClothName.Visible = False
            cbo_Grid_ClothName.Tag = -100
        End If

        If Me.ActiveControl.Name <> cbo_Grid_Clothtype.Name Then
            cbo_Grid_Clothtype.Visible = False
            cbo_Grid_Clothtype.Tag = -100
        End If

        'If Me.ActiveControl.Name <> dgv_Details.Name And Not (TypeOf ActiveControl Is DataGridViewTextBoxEditingControl) Then
        '    pnl_BaleSelection_ToolTip.Visible = False
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

    Private Sub Cloth_Purchae_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Agent.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "AGENT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Agent.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PurchaseAc.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "SALES" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PurchaseAc.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_ClothName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_ClothName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Clothtype.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH TYPE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Clothtype.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Cloth_Purchae_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Cloth_Purchae_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Cloth_Purchae_Entry_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
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

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead Where (Ledger_Type = '' and ( Ledger_IdNo = 0 or AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) order by Ledger_DisplayName", con)
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

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or AccountsGroup_IdNo = 27 ) order by Ledger_DisplayName", con)
        da.Fill(dt7)
        cbo_PurchaseAc.DataSource = dt7
        cbo_PurchaseAc.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
        da.Fill(dt4)
        cbo_Grid_ClothName.DataSource = dt4
        cbo_Grid_ClothName.DisplayMember = "Cloth_Name"

        da = New SqlClient.SqlDataAdapter("select ClothType_Name from ClothType_Head order by ClothType_Name", con)
        da.Fill(dt5)
        cbo_Grid_Clothtype.DataSource = dt5
        cbo_Grid_Clothtype.DisplayMember = "ClothType_Name"

        cbo_Com_Type.Items.Clear()
        cbo_Com_Type.Items.Add(" ")
        cbo_Com_Type.Items.Add("%")
        cbo_Com_Type.Items.Add("MTR")

        cbo_Type.Items.Clear()
        cbo_Type.Items.Add(" ")
        cbo_Type.Items.Add("DIRECT")
        cbo_Type.Items.Add("RECEIPT")

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

    
        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Agent.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Type.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Clothtype.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Com_Type.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PurchaseAc.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_BillNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Pcs.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Bundles.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Note.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ReceiptMeters.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ExcShtMeters.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ReturnMeters.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_com_per.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CommAmt.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Cash_Disc.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_RecDate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RecNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Insurance.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Packing.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Trade_Disc.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Vechile.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TradeDic_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Insurance_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Packing_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CashDic_Name.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight_Name.GotFocus, AddressOf ControlGotFocus


        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ClothName.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus


        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Agent.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Clothtype.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Com_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PurchaseAc.LostFocus, AddressOf ControlLostFocus


        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Pcs.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Bundles.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_ExcShtMeters.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Note.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BillNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ReturnMeters.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_RecDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RecNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ReceiptMeters.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Cash_Disc.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_com_per.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CommAmt.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Insurance.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Packing.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Trade_Disc.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Vechile.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Insurance_Name.LostFocus, AddressOf ControlLostFocus1
        AddHandler txt_Packing_Name.LostFocus, AddressOf ControlLostFocus1
        AddHandler txt_TradeDic_Name.LostFocus, AddressOf ControlLostFocus1
        AddHandler txt_CashDic_Name.LostFocus, AddressOf ControlLostFocus1
        AddHandler txt_Freight_Name.LostFocus, AddressOf ControlLostFocus1

        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ClothName.LostFocus, AddressOf ControlLostFocus

        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus

        AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ExcShtMeters.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_BillNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Bundles.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Pcs.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Trade_Disc.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_RecDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_RecNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Packing.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_ReceiptMeters.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_com_per.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CommAmt.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_Vechile.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Cash_Disc.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler txt_Freight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Insurance.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_TradeDic_Name.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CashDic_Name.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_ClthDetail_Name.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Insurance_Name.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Freight_Name.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Packing_Name.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_ReturnMeters.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_ExcShtMeters.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BillNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Pcs.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Bundles.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_RecDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_RecNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_ReceiptMeters.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_com_per.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CommAmt.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Packing.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Cash_Disc.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Trade_Disc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Insurance.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Freight.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_CashDic_Name.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TradeDic_Name.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Freight_Name.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Insurance_Name.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Packing_Name.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler txt_ClthDetail_Name.KeyPress, AddressOf TextBoxControlKeyPress

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
                        If .CurrentCell.ColumnIndex >= 6 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                txt_ReturnMeters.Focus()

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
                                txt_ReturnMeters.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(6)

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
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer

        If Val(no) = 0 Then Exit Sub

        clear()

        NoCalc_Status = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from Cloth_Purchase_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cloth_Purchase_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Cloth_Purchase_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Cloth_Purchase_Date").ToString
                msk_date.Text = dtp_Date.Text

                cbo_PartyName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                cbo_Agent.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Agent_IdNo").ToString))
                cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))
                cbo_PurchaseAc.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("PurchaseAc_IdNo").ToString))

                cbo_Type.Text = dt1.Rows(0).Item("Purchase_Selection_Type").ToString
                cbo_Com_Type.Text = dt1.Rows(0).Item("Agent_Comm_Type").ToString

                txt_Bundles.Text = dt1.Rows(0).Item("Bundles").ToString
                txt_Pcs.Text = dt1.Rows(0).Item("Noof_Pcs").ToString
                txt_BillNo.Text = dt1.Rows(0).Item("Bill_No").ToString
                txt_ExcShtMeters.Text = dt1.Rows(0).Item("Excsht_Meters").ToString

                txt_com_per.Text = dt1.Rows(0).Item("Agent_Comm_Perc").ToString
                txt_CommAmt.Text = dt1.Rows(0).Item("Agent_Comm_Total").ToString

                txt_ReceiptMeters.Text = Format(Val(dt1.Rows(0).Item("Receipt_Meters").ToString), "########0.00")
                txt_ReturnMeters.Text = Format(Val(dt1.Rows(0).Item("Return_Meters").ToString), "########0.00")
                lbl_TotalMeters.Text = Format(Val(dt1.Rows(0).Item("Net_Meter").ToString), "########0.00")

                lbl_ReceiptCode.Text = dt1.Rows(0).Item("Cloth_Purchase_Receipt_Code").ToString
                txt_RecDate.Text = dt1.Rows(0).Item("Rec_Date").ToString
                txt_RecNo.Text = dt1.Rows(0).Item("Rec_No").ToString
                txt_Vechile.Text = dt1.Rows(0).Item("Vechile_No").ToString
                If Val(dt1.Rows(0).Item("FoldingRate_Status").ToString) = 1 Then chk_No_Folding.Checked = True

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
                lbl_Net_Amt.Text = dt1.Rows(0).Item("Net_Amount").ToString
                txt_Note.Text = dt1.Rows(0).Item("Note").ToString
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Cloth_Name, c.ClothType_Name from Cloth_Purchase_Details a LEFT OUTER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo LEFT OUTER JOIN ClothType_Head c ON a.ClothType_IdNo = c.ClothType_IdNo Where a.Cloth_Purchase_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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
                            .Rows(n).Cells(2).Value = dt2.Rows(i).Item("ClothType_Name").ToString
                            .Rows(n).Cells(3).Value = Format(Val(dt2.Rows(i).Item("Fold_Perc").ToString), "########0.00")
                            .Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Pcs").ToString)
                            .Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                            .Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")
                            .Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")


                        Next i

                    End If

                End With

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(4).Value = Val(dt1.Rows(0).Item("Total_Pcs").ToString)
                    .Rows(0).Cells(5).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                    .Rows(0).Cells(7).Value = Format(Val(dt1.Rows(0).Item("Total_Amount").ToString), "########0.00")

                End With

            End If

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()

            If msk_date.Visible And msk_date.Enabled Then msk_date.Focus()

        End Try

        NoCalc_Status = False

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Cloth_Purchase_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Cloth_Purchase_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Cloth_Purchase_Entry, New_Entry, Me, con, "Cloth_Purchase_Head", "Cloth_Purchase_Code", NewCode, "Cloth_Purchase_Date", "(Cloth_Purchase_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

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

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            If Common_Procedures.VoucherBill_Deletion(con, Trim(Pk_Condition) & Trim(NewCode), trans) = False Then
                Throw New ApplicationException("Error on Voucher Bill Deletion")
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), trans)

            cmd.CommandText = "Update Stock_Cloth_Processing_Details set Reference_Date = b.Cloth_Purchase_Receipt_Date, UnChecked_Meters = b.Receipt_Meters, Meters_Type1 = 0, Meters_Type2 = 0, Meters_Type3 = 0, Meters_Type4 = 0, Meters_Type5 = 0 from Stock_Cloth_Processing_Details a, Cloth_Purchase_Receipt_Head b Where b.Cloth_Purchase_code = '" & Trim(NewCode) & "' and b.Weaver_Piece_Checking_Code = '' and a.Reference_Code = '" & Trim(Pk_Condition3) & "' + b.Cloth_Purchase_Receipt_Code"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Cloth_Purchase_Receipt_Head set Cloth_Purchase_code = '' where Cloth_Purchase_code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from AgentCommission_Processing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Cloth_Purchase_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Purchase_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Cloth_Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Purchase_Code = '" & Trim(NewCode) & "'"
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

            If msk_date.Enabled = True And msk_date.Visible = True Then msk_date.Focus()

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
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Cloth_Purchase_No from Cloth_Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Cloth_Purchase_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Cloth_Purchase_No from Cloth_Purchase_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Cloth_Purchase_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Cloth_Purchase_No from Cloth_Purchase_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Cloth_Purchase_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Cloth_Purchase_No from Cloth_Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Cloth_Purchase_No desc", con)
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

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Cloth_Purchase_Head", "Cloth_Purchase_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_RefNo.ForeColor = Color.Red

            msk_date.Text = Date.Today.ToShortDateString
            Da1 = New SqlClient.SqlDataAdapter("select top 1 a.*, b.ledger_name as SalesAcName from Cloth_Purchase_Head a LEFT OUTER JOIN Ledger_Head b ON a.PurchaseAc_IdNo = b.Ledger_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Cloth_Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by a.for_Orderby desc, a.Cloth_Purchase_No desc", con)
            Da1.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If Dt1.Rows(0).Item("Cloth_Purchase_Date").ToString <> "" Then msk_date.Text = Dt1.Rows(0).Item("Cloth_Purchase_Date").ToString
                End If
                If Dt1.Rows(0).Item("SalesAcName").ToString <> "" Then cbo_PurchaseAc.Text = Dt1.Rows(0).Item("SalesAcName").ToString
                If Dt1.Rows(0).Item("CashDisc_Name").ToString <> "" Then txt_CashDic_Name.Text = Dt1.Rows(0).Item("CashDisc_Name").ToString
                If Dt1.Rows(0).Item("TradeDisc_Name").ToString <> "" Then txt_TradeDic_Name.Text = Dt1.Rows(0).Item("TradeDisc_Name").ToString
                If Dt1.Rows(0).Item("Freight_Name").ToString <> "" Then txt_Freight_Name.Text = Dt1.Rows(0).Item("Freight_Name").ToString
                If Dt1.Rows(0).Item("Packing_Name").ToString <> "" Then txt_Packing_Name.Text = Dt1.Rows(0).Item("Packing_Name").ToString
                If Dt1.Rows(0).Item("Insurance_Name").ToString <> "" Then txt_Insurance_Name.Text = Dt1.Rows(0).Item("Insurance_Name").ToString
            End If

            Dt1.Clear()


        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da1.Dispose()

            If msk_date.Enabled And msk_date.Visible Then
                msk_date.Focus()
                msk_date.SelectionStart = 0
            End If

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        Try

            inpno = InputBox("Enter Ref No.", "FOR FINDING...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Cloth_Purchase_No from Cloth_Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Purchase_Code = '" & Trim(InvCode) & "'", con)
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
                MessageBox.Show("Ref No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Cloth_Purchase_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Cloth_Purchase_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Cloth_Purchase_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Ref No.", "FOR NEW REF NO. INSERTION...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Cloth_Purchase_No from Cloth_Purchase_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Purchase_Code = '" & Trim(InvCode) & "'", con)
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
                    MessageBox.Show("Invalid Ref No.", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

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
        Dim clthtyp_ID As Integer = 0
        Dim Trans_ID As Integer
        Dim Led_ID As Integer = 0
        Dim Agt_Idno As Integer = 0
        Dim Sno As Integer = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Partcls As String = ""
        Dim vTotPcs As Single, vTotMtrs As Single, vTotBals As Single, vTotAmt As Single
        Dim PurcAc_ID As Integer = 0
        Dim OnAc_ID As Integer = 0
        Dim YrnClthNm As String = ""
        Dim Nr As Integer = 0
        Dim PurCd As String = ""
        Dim PurSlNo As Long = 0
        Dim DcCd As String = ""
        Dim DcSlNo As Long = 0
        Dim PcsChkCode As String = ""

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Cloth_Purchase_Entry, New_Entry) = False Then Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Cloth_Purchase_Entry, New_Entry, Me, con, "Cloth_Purchase_Head", "Cloth_Purchase_Code", NewCode, "Cloth_Purchase_Date", "(Cloth_Purchase_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Purchase_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Cloth_Purchase_No desc", dtp_Date.Value.Date) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
            Exit Sub
        End If

        If Not (Convert.ToDateTime(msk_date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
            Exit Sub
        End If


        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)

        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled Then cbo_PartyName.Focus()
            Exit Sub
        End If

        If Trim(UCase(cbo_Type.Text)) = "" Or Trim(UCase(cbo_Type.Text)) <> "RECEIPT" Then
            cbo_Type.Text = "DIRECT"
        End If

        Agt_Idno = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Agent.Text)
        Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)
        PurcAc_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PurchaseAc.Text)
        lbl_UserName.Text = Common_Procedures.User.IdNo
        If PurcAc_ID = 0 And Val(CSng(lbl_Net_Amt.Text)) <> 0 Then
            MessageBox.Show("Invalid Purchase A/c", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_PurchaseAc.Enabled And cbo_PurchaseAc.Visible Then cbo_PurchaseAc.Focus()
            Exit Sub
        End If

        For i = 0 To dgv_Details.RowCount - 1

            If Val(dgv_Details.Rows(i).Cells(5).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(7).Value) <> 0 Then

                clth_ID = Common_Procedures.Cloth_NameToIdNo(con, dgv_Details.Rows(i).Cells(1).Value)
                If clth_ID = 0 Then
                    MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(1)
                    End If
                    Exit Sub
                End If

                clthtyp_ID = Common_Procedures.ClothType_NameToIdNo(con, dgv_Details.Rows(i).Cells(2).Value)
                If clthtyp_ID = 0 Then
                    MessageBox.Show("Invalid Cloth Type Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(2)
                    End If
                    Exit Sub
                End If

                If Val(dgv_Details.Rows(i).Cells(3).Value) = 0 Then
                    MessageBox.Show("Invalid Folding", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(3)
                    End If
                    Exit Sub
                End If


                If Val(dgv_Details.Rows(i).Cells(5).Value) = 0 Then
                    MessageBox.Show("Invalid Purchase metres", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(5)
                    End If
                    Exit Sub
                End If

            End If

        Next

        NoCalc_Status = False
        Total_Calculation()

        vTotPcs = 0 : vTotMtrs = 0 : vTotAmt = 0

        If dgv_Details_Total.RowCount > 0 Then
            vTotPcs = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
            vTotMtrs = Val(dgv_Details_Total.Rows(0).Cells(5).Value())
            vTotAmt = Val(dgv_Details_Total.Rows(0).Cells(7).Value())
        End If

        'If vTotMtrs = 0 Then
        '    MessageBox.Show("Invalid METERS", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    If dgv_Details.Enabled And dgv_Details.Visible Then
        '        dgv_Details.Focus()
        '        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(7)
        '    End If
        '    Exit Sub
        'End If

        NoFo_STS = 0
        If chk_No_Folding.Checked = True Then NoFo_STS = 1

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Cloth_Purchase_Head", "Cloth_Purchase_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            Da = New SqlClient.SqlDataAdapter("select Weaver_Piece_Checking_Code, Cloth_Purchase_Code from Cloth_Purchase_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Purchase_Receipt_Code = '" & Trim(NewCode) & "'", con)
            Da.SelectCommand.Transaction = tr
            Dt1 = New DataTable
            Da.Fill(Dt1)

            PcsChkCode = ""
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) = False Then
                    PcsChkCode = Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString
                End If
            End If
            Dt1.Clear()

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@PurchaseDate", Convert.ToDateTime(msk_date.Text))

            If New_Entry = True Then
                cmd.CommandText = "Insert into Cloth_Purchase_Head (   Cloth_Purchase_Code  ,               Company_IdNo       ,          Cloth_Purchase_No    ,                       for_OrderBy                                      ,  Cloth_Purchase_Date,        Ledger_IdNo      ,          Purchase_Selection_Type     ,             Bill_No             ,           Agent_IdNo      ,         Agent_Comm_Perc           ,          Agent_Comm_Type        ,              Agent_Comm_Total     ,        Cloth_Purchase_Receipt_Code   ,               Rec_No          ,             Rec_Date            ,           Bundles            ,          noof_Pcs        ,           Receipt_Meters               ,           PurchaseAc_IdNo  ,       FoldingRate_Status   ,       Transport_IdNo      ,      Vechile_No                 ,       Return_Meters                     ,          Excsht_Meters                  ,         Net_Meter                      ,           TradeDisc_Name                 ,       Trade_Discount                 ,       CashDisc_Name                  ,               Cash_Discount          ,            Trade_Discount_Perc            ,             Cash_Discount_Perc           ,        Packing_Name                  ,            Packing_Amount         ,             Freight_Name             ,             Freight                ,           Insurance_Name              ,           Insurance                 ,               Net_Amount                ,             Note             ,          Total_Pcs        ,        Total_Meters        ,           Total_Amount     ,  user_idNo   ) " & _
                                    "     Values                   ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",       @PurchaseDate , " & Str(Val(Led_ID)) & ",  '" & Trim(UCase(cbo_Type.Text)) & "',  '" & Trim(txt_BillNo.Text) & "', " & Str(Val(Agt_Idno)) & ", " & Str(Val(txt_com_per.Text)) & ",'" & Trim(cbo_Com_Type.Text) & "', " & Str(Val(txt_CommAmt.Text)) & ",  '" & Trim(lbl_ReceiptCode.Text) & "', '" & Trim(txt_RecNo.Text) & "', '" & Trim(txt_RecDate.Text) & "', " & Val(txt_Bundles.Text) & ", " & Val(txt_Pcs.Text) & "," & Str(Val(txt_ReceiptMeters.Text)) & ", " & Str(Val(PurcAc_ID)) & ",  " & Str(Val(NoFo_STS)) & ", " & Str(Val(Trans_ID)) & ", '" & Trim(txt_Vechile.Text) & "', " & Str(Val(txt_ReturnMeters.Text)) & " , " & Str(Val(txt_ExcShtMeters.Text)) & " , " & Str(Val(lbl_TotalMeters.Text)) & " ,    '" & Trim(txt_TradeDic_Name.Text) & "', " & Str(Val(txt_Trade_Disc.Text)) & ", '" & Trim(txt_CashDic_Name.Text) & "',  " & Str(Val(txt_Cash_Disc.Text)) & ", " & Str(Val(lbl_Trade_Disc_Perc.Text)) & ", " & Str(Val(lbl_Cash_Disc_Perc.Text)) & ", '" & Trim(txt_Packing_Name.Text) & "', " & Str(Val(txt_Packing.Text)) & ", '" & Trim(txt_Freight_Name.Text) & "',  " & Str(Val(txt_Freight.Text)) & ",'" & Trim(txt_Insurance_Name.Text) & "', " & Str(Val(txt_Insurance.Text)) & ", " & Str(Val(CSng(lbl_Net_Amt.Text))) & ", '" & Trim(txt_Note.Text) & "',  " & Str(Val(vTotPcs)) & ", " & Str(Val(vTotMtrs)) & " , " & Str(Val(vTotAmt)) & " ," & Val(lbl_UserName.Text) & " ) "
                cmd.ExecuteNonQuery()

            Else
                cmd.CommandText = "Update Cloth_Purchase_Head set Cloth_Purchase_Date = @PurchaseDate, Ledger_IdNo =  " & Str(Val(Led_ID)) & " ,    Purchase_Selection_Type = '" & Trim(UCase(cbo_Type.Text)) & "' , Bill_No =  '" & Trim(txt_BillNo.Text) & "',  Agent_IdNo = " & Str(Val(Agt_Idno)) & " ,Agent_Comm_Perc =  " & Str(Val(txt_com_per.Text)) & ", Agent_Comm_Type = '" & Trim(cbo_Com_Type.Text) & "' , Agent_Comm_Total = " & Str(Val(txt_CommAmt.Text)) & ", Cloth_Purchase_Receipt_Code = '" & Trim(lbl_ReceiptCode.Text) & "', Rec_No = '" & Trim(txt_RecNo.Text) & "', Rec_Date = '" & Trim(txt_RecDate.Text) & "',  Bundles = " & Val(txt_Bundles.Text) & " ,  Noof_Pcs = " & Val(txt_Pcs.Text) & ",  Receipt_Meters = " & Str(Val(txt_ReceiptMeters.Text)) & ",   PurchaseAc_IdNo = " & Str(Val(PurcAc_ID)) & ", FoldingRate_Status  = " & Str(Val(NoFo_STS)) & " , Transport_IdNo = " & Str(Val(Trans_ID)) & "  ,Vechile_No = '" & Trim(txt_Vechile.Text) & "',  Return_Meters =  " & Str(Val(txt_ReturnMeters.Text)) & ",Excsht_Meters =  " & Str(Val(txt_ExcShtMeters.Text)) & ", Net_Meter =  " & Str(Val(lbl_TotalMeters.Text)) & ", TradeDisc_Name = '" & Trim(txt_TradeDic_Name.Text) & "', Trade_Discount =  " & Str(Val(txt_Trade_Disc.Text)) & " , CashDisc_Name ='" & Trim(txt_CashDic_Name.Text) & "'  , Cash_Discount = " & Str(Val(txt_Cash_Disc.Text)) & " , Trade_Discount_Perc = " & Str(Val(lbl_Trade_Disc_Perc.Text)) & "   , Cash_Discount_Perc = " & Str(Val(lbl_Cash_Disc_Perc.Text)) & " , Packing_Name ='" & Trim(txt_Packing_Name.Text) & "', Packing_Amount = " & Str(Val(txt_Packing.Text)) & " , Freight_Name = '" & Trim(txt_Freight_Name.Text) & "' , Freight =" & Str(Val(txt_Freight.Text)) & " , Insurance_Name ='" & Trim(txt_Insurance_Name.Text) & "' , Insurance =  " & Str(Val(txt_Insurance.Text)) & ", Net_Amount = " & Str(Val(CSng(lbl_Net_Amt.Text))) & ", Note = '" & Trim(txt_Note.Text) & "', Total_Bales = " & Str(Val(vTotBals)) & ", Total_Pcs = " & Str(Val(vTotPcs)) & ", Total_Meters = " & Str(Val(vTotMtrs)) & " , Total_Amount = " & Str(Val(vTotAmt)) & "  , user_idno = " & Val(lbl_UserName.Text) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Purchase_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Stock_Cloth_Processing_Details set Reference_Date = b.Cloth_Purchase_Receipt_Date, UnChecked_Meters = b.Receipt_Meters, Meters_Type1 = 0, Meters_Type2 = 0, Meters_Type3 = 0, Meters_Type4 = 0, Meters_Type5 = 0 from Stock_Cloth_Processing_Details a, Cloth_Purchase_Receipt_Head b Where b.Cloth_Purchase_code = '" & Trim(NewCode) & "' and b.Weaver_Piece_Checking_Code = '' and a.Reference_Code = '" & Trim(Pk_Condition3) & "' + b.Cloth_Purchase_Receipt_Code"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Cloth_Purchase_Receipt_Head set Cloth_Purchase_code = '' where Cloth_Purchase_code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            If Trim(UCase(cbo_Type.Text)) = "RECEIPT" And Trim(lbl_ReceiptCode.Text) <> "" Then

                cmd.CommandText = "Update Cloth_Purchase_Receipt_Head set Cloth_Purchase_code = '" & Trim(NewCode) & "' where Cloth_Purchase_Receipt_Code = '" & Trim(lbl_ReceiptCode.Text) & "' and Ledger_IdNo = " & Str(Val(Led_ID))
                Nr = cmd.ExecuteNonQuery()
                If Nr = 0 Then
                    Throw New ApplicationException("Mismatch of Party & Receipt Details")
                    'tr.Rollback()
                    'MessageBox.Show("Mismatch of Party & Receipt Details", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If

                If Trim(PcsChkCode) = "" Then
                    cmd.CommandText = "Update Stock_Cloth_Processing_Details set UnChecked_Meters = 0, Meters_Type1 = 0, Meters_Type2 = 0, Meters_Type3 = 0, Meters_Type4 = 0, Meters_Type5 = 0 where Reference_Code = '" & Trim(Pk_Condition3) & Trim(lbl_ReceiptCode.Text) & "'"
                    cmd.ExecuteNonQuery()
                End If

            End If

            EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)
            PBlNo = Trim(lbl_RefNo.Text)
            Partcls = "ClothPurc : Ref.No. " & Trim(lbl_RefNo.Text)

            cmd.CommandText = "Delete from Cloth_Purchase_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Purchase_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Details

                Sno = 0
                YrnClthNm = ""

                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(5).Value) <> 0 Then

                        Sno = Sno + 1

                        clth_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                        clthtyp_ID = Common_Procedures.ClothType_NameToIdNo(con, .Rows(i).Cells(2).Value, tr)
                        PurCd = ""

                        If Trim(YrnClthNm) = "" Then YrnClthNm = Trim(.Rows(i).Cells(1).Value)


                        cmd.CommandText = "Insert into Cloth_Purchase_Details (    Cloth_Purchase_Code ,               Company_IdNo       ,      Cloth_Purchase_No        ,                               for_OrderBy                              , Cloth_Purchase_Date,         Purchase_Selection_Type     ,         Sl_No        ,        Cloth_IdNo        ,       ClothType_IdNo        ,                      Fold_Perc           ,                             Pcs           ,                      Meters              ,                     Rate                 ,                       Amount              ) " & _
                                                "     Values                  ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",    @PurchaseDate   , '" & Trim(UCase(cbo_Type.Text)) & "', " & Str(Val(Sno)) & ", " & Str(Val(clth_ID)) & ", " & Str(Val(clthtyp_ID)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ",  " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & " ) "
                        cmd.ExecuteNonQuery()


                        If Trim(UCase(cbo_Type.Text)) = "RECEIPT" Then
                            If Trim(PcsChkCode) = "" Then
                                cmd.CommandText = "Update Stock_Cloth_Processing_Details set Reference_Date = @PurchaseDate , Meters_Type" & Trim(Val(clthtyp_ID)) & " = Meters_Type" & Trim(Val(clthtyp_ID)) & " + " & Str(Val(.Rows(i).Cells(5).Value)) & " where Reference_Code = '" & Trim(Pk_Condition3) & Trim(lbl_ReceiptCode.Text) & "'"
                                cmd.ExecuteNonQuery()
                            End If

                        Else

                            cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code,             Company_IdNo         ,           Reference_No        ,                               for_OrderBy                              , Reference_Date,                                            StockOff_IdNo  ,                                      DeliveryTo_Idno      ,     ReceivedFrom_Idno   ,         Entry_ID     ,       Party_Bill_No  ,       Particulars      ,           Sl_No      ,           Cloth_Idno     ,                      Folding             ,   Meters_Type" & Trim(Val(clthtyp_ID)) & ") " & _
                                                    " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",  @PurchaseDate, " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & ", " & Str(Val(Led_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(clth_ID)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & " ) "
                            cmd.ExecuteNonQuery()

                        End If

                    End If

                Next

            End With


            'AgentCommission Posting
            cmd.CommandText = "delete from AgentCommission_Processing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If Val(Agt_Idno) <> 0 Then

                cmd.CommandText = "Insert into AgentCommission_Processing_Details (  Reference_Code   ,             Company_IdNo         ,            Reference_No       ,                               For_OrderBy                              , Reference_Date, Commission_For,     Ledger_IdNo    ,           Agent_IdNo      ,         Entry_ID     ,      Party_BillNo    ,       Particulars      ,      Yarn_Cloth_Name     ,         Bags_Meters       ,               Amount              ,             Commission_Type      ,       Commission_Rate              ,            Commission_Amount     ) " & _
                                                " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",   @PurchaseDate,     'CLOTH'   , " & Str(Led_ID) & ", " & Str(Val(Agt_Idno)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', '" & Trim(YrnClthNm) & "', " & Str(Val(vTotMtrs)) & ", " & Str(Val(lbl_Net_Amt.Text)) & ", '" & Trim(cbo_Com_Type.Text) & "', " & Str(Val(txt_com_per.Text)) & ", " & Str(Val(txt_CommAmt.Text)) & ")"
                cmd.ExecuteNonQuery()

            End If


            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
            vLed_IdNos = Led_ID & "|" & PurcAc_ID
            vVou_Amts = Val(CSng(lbl_Net_Amt.Text)) & "|" & -1 * Val(CSng(lbl_Net_Amt.Text))
            If Common_Procedures.Voucher_Updation(con, "Clo.Purc", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_RefNo.Text), Convert.ToDateTime(msk_date.Text), "Bill No : " & Trim(txt_BillNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

            vLed_IdNos = Agt_Idno & "|" & Val(Common_Procedures.CommonLedger.Agent_Commission_Ac)
            vVou_Amts = Val(txt_CommAmt.Text) & "|" & -1 * Val(txt_CommAmt.Text)
            If Common_Procedures.Voucher_Updation(con, "AgComm.CloPurc", Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), Trim(lbl_RefNo.Text), Convert.ToDateTime(msk_date.Text), "Bill No : " & Trim(txt_BillNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                Throw New ApplicationException(ErrMsg)
            End If


            Dim VouBil As String = ""
            VouBil = Common_Procedures.VoucherBill_Posting(con, Val(lbl_Company.Tag), msk_date.Text, Led_ID, Trim(txt_BillNo.Text), Agt_Idno, Val(CSng(lbl_Net_Amt.Text)), "CR", Trim(Pk_Condition) & Trim(NewCode), tr)
            If Trim(UCase(VouBil)) = "ERROR" Then
                Throw New ApplicationException("Error on Voucher Bill Posting")
            End If

            tr.Commit()


            If SaveAll_STS <> True Then
                MessageBox.Show("Saved Successfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            End If

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_RefNo.Text)
                End If
            Else
                move_record(lbl_RefNo.Text)
            End If

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Dt1.Dispose()
            Da.Dispose()
            cmd.Dispose()
            tr.Dispose()

            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

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
                    tlamt = (Val(.Rows(0).Cells(7).Value))
                    tlmtr = (Val(.Rows(0).Cells(5).Value))

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
        Dim GrsAmt As Single
        Dim NtAmt As Single

        If NoCalc_Status = True Then Exit Sub

        GrsAmt = 0

        With dgv_Details_Total
            If .Rows.Count > 0 Then
                GrsAmt = Val(.Rows(0).Cells(7).Value)
            End If
        End With

        If Val(txt_Trade_Disc.Text) <> 0 Then
            lbl_Trade_Disc_Perc.Text = Format(Val(GrsAmt) * Val(txt_Trade_Disc.Text) / 100, "########0.00")
        End If
        If Val(txt_Cash_Disc.Text) <> 0 Then
            lbl_Cash_Disc_Perc.Text = Format(Val(GrsAmt) * Val(txt_Cash_Disc.Text) / 100, "########0.00")
        End If

        NtAmt = Val(GrsAmt) + Val(txt_Insurance.Text) + Val(txt_Freight.Text) + Val(txt_Packing.Text) - Val(lbl_Trade_Disc_Perc.Text) - Val(lbl_Cash_Disc_Perc.Text)

        lbl_Net_Amt.Text = Format(Val(NtAmt), "#########0")

        lbl_Net_Amt.Text = Common_Procedures.Currency_Format(Val(CSng(lbl_Net_Amt.Text)))

    End Sub

    Private Sub Amount_Calculation(ByVal CurRow As Integer, ByVal CurCol As Integer)
        Dim fldmtr As Single = 0
        Dim fmt As Single = 0

        On Error Resume Next

        With dgv_Details
            If .Visible Then

                If CurCol = 3 Or CurCol = 4 Or CurCol = 5 Or CurCol = 6 Or CurCol = 7 Then

                    If CurCol = 3 Or CurCol = 5 Or CurCol = 6 Then
                        If Val(.Rows(CurRow).Cells(3).Value) = 0 Or Val(.Rows(CurRow).Cells(3).Value) = 100 Or chk_No_Folding.Checked = True Then

                            .Rows(CurRow).Cells(7).Value = Format(Val(.Rows(CurRow).Cells(5).Value) * Val(.Rows(CurRow).Cells(6).Value), "#########0.00")

                        Else

                            fmt = ((100 - Val(.Rows(CurRow).Cells(3).Value)) / 100) * Val(.Rows(CurRow).Cells(5).Value)
                            fmt = Format(Math.Abs(Val(fmt)), "######0.00")

                            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1035" Then
                                fmt = Common_Procedures.Meter_RoundOff(fmt)
                            End If
                            
                            If (100 - Val(.Rows(CurRow).Cells(3).Value)) > 0 Then
                                fldmtr = Format(Val(.Rows(CurRow).Cells(5).Value) - Val(fmt), "#########0.00")
                            Else
                                fldmtr = Format(Val(.Rows(CurRow).Cells(5).Value) + Val(fmt), "#########0.00")
                            End If

                            .Rows(CurRow).Cells(7).Value = Format(Val(fldmtr) * Val(.Rows(CurRow).Cells(6).Value), "#########0.00")

                        End If
                    End If

                    Total_Calculation()

                End If

            End If
        End With
    End Sub

    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotPcs As Single
        Dim TotBals As Single
        Dim TotMtrs As Single
        Dim TotAmt As Single

        If NoCalc_Status = True Then Exit Sub

        Sno = 0
        TotPcs = 0 : TotBals = 0 : TotMtrs = 0 : TotAmt = 0

        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(5).Value) <> 0 Then

                    TotPcs = TotPcs + Val(.Rows(i).Cells(4).Value())
                    TotMtrs = TotMtrs + Val(.Rows(i).Cells(5).Value())
                    TotAmt = TotAmt + Val(.Rows(i).Cells(7).Value())

                End If

            Next i

        End With


        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(4).Value = Val(TotPcs)
            .Rows(0).Cells(5).Value = Format(Val(TotMtrs), "########0.00")
            .Rows(0).Cells(7).Value = Format(Val(TotAmt), "########0.00")
        End With

        AgentCommision_Calculation()

        NetAmount_Calculation()

    End Sub

    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( ( Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1 ) and Close_status = 0 ) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName, msk_date, cbo_Type, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( ( Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName, cbo_Type, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( ( Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1 ) and Close_status = 0 ) ", "(Ledger_idno = 0)")
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

    Private Sub cbo_Agent_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Agent, txt_BillNo, txt_com_per, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Agent_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Agent.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Agent, txt_com_per, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
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

    Private Sub cbo_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Type.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Type, cbo_PartyName, txt_BillNo, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Type, txt_BillNo, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(cbo_Type.Text)) = "RECEIPT" Then
                If MessageBox.Show("Do you want to select Cloth Receipt :", "FOR CLOTH RECEIPT SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    btn_ReceiptSelection_Click(sender, e)
                Else
                    txt_BillNo.Focus()
                End If

            Else
                txt_BillNo.Focus()

            End If

        End If

    End Sub

    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, cbo_PurchaseAc, txt_Vechile, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, txt_Vechile, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
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

    Private Sub cbo_Grid_ClothName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_ClothName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_ClothName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_ClothName.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_ClothName, Nothing, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_ClothName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If .CurrentCell.RowIndex <= 0 Then
                    txt_Vechile.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(6)
                    .CurrentCell.Selected = True
                End If

            End If

            If (e.KeyValue = 40 And cbo_Grid_ClothName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    txt_ReturnMeters.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    .CurrentCell.Selected = True

                End If

            End If

        End With
    End Sub

    Private Sub cbo_Grid_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_ClothName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_ClothName, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            e.Handled = True

            With dgv_Details
                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    txt_ReturnMeters.Focus()

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

    Private Sub cbo_Grid_Clothtype_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Clothtype.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothType_Head", "ClothType_Name", "( ClothType_IdNo between 1 and 5 )", "(ClothType_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_Clothtype_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Clothtype.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Clothtype, Nothing, Nothing, "ClothType_Head", "ClothType_Name", "( ClothType_IdNo between 1 and 5 )", "(ClothType_IdNo = 0)")
        vcbo_KeyDwnVal = e.KeyValue

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_Clothtype.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And cbo_Grid_Clothtype.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With

    End Sub

    Private Sub cbo_Grid_Clothtype_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Clothtype.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Clothtype, Nothing, "ClothType_Head", "ClothType_Name", "( ClothType_IdNo between 1 and 5 )", "(ClothType_IdNo = 0)")

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
        Dim Rect As Rectangle

        With dgv_Details

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If Trim(.CurrentRow.Cells(2).Value) = "" Then
                .CurrentRow.Cells(2).Value = Common_Procedures.ClothType_IdNoToName(con, 1)
            End If

            If Val(.CurrentRow.Cells(3).Value) = 0 Then
                .CurrentRow.Cells(3).Value = "100"
            End If

            If e.ColumnIndex = 1 Then

                If cbo_Grid_ClothName.Visible = False Or Val(cbo_Grid_ClothName.Tag) <> e.RowIndex Then

                    cbo_Grid_ClothName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_ClothName.DataSource = Dt1
                    cbo_Grid_ClothName.DisplayMember = "Cloth_Name"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_ClothName.Left = .Left + Rect.Left
                    cbo_Grid_ClothName.Top = .Top + Rect.Top

                    cbo_Grid_ClothName.Width = Rect.Width
                    cbo_Grid_ClothName.Height = Rect.Height
                    cbo_Grid_ClothName.Text = .CurrentCell.Value

                    cbo_Grid_ClothName.Tag = Val(e.RowIndex)
                    cbo_Grid_ClothName.Visible = True

                    cbo_Grid_ClothName.BringToFront()
                    cbo_Grid_ClothName.Focus()

                Else

                    'If cbo_Grid_ClothName.Visible = True Then
                    '    cbo_Grid_ClothName.BringToFront()
                    '    cbo_Grid_ClothName.Focus()
                    'End If

                End If

            Else
                cbo_Grid_ClothName.Visible = False

            End If

            If e.ColumnIndex = 2 Then

                If cbo_Grid_Clothtype.Visible = False Or Val(cbo_Grid_Clothtype.Tag) <> e.RowIndex Then

                    cbo_Grid_Clothtype.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select ClothType_Name from ClothType_Head Where ClothType_IdNo between 1 and 5 order by ClothType_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_Clothtype.DataSource = Dt1
                    cbo_Grid_Clothtype.DisplayMember = "ClothType_Name"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_Clothtype.Left = .Left + Rect.Left
                    cbo_Grid_Clothtype.Top = .Top + Rect.Top

                    cbo_Grid_Clothtype.Width = Rect.Width
                    cbo_Grid_Clothtype.Height = Rect.Height
                    cbo_Grid_Clothtype.Text = .CurrentCell.Value

                    cbo_Grid_Clothtype.Tag = Val(e.RowIndex)
                    cbo_Grid_Clothtype.Visible = True

                    cbo_Grid_Clothtype.BringToFront()
                    cbo_Grid_Clothtype.Focus()

                Else
                    'If cbo_Grid_Clothtype.Visible = True Then
                    '    cbo_Grid_Clothtype.BringToFront()
                    '    cbo_Grid_Clothtype.Focus()
                    'End If

                End If

            Else
                cbo_Grid_Clothtype.Visible = False

            End If

        End With

    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details
            If .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Then
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
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details
            If .Visible Then

                If .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Then

                    Amount_Calculation(.CurrentCell.RowIndex, .CurrentCell.ColumnIndex)

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
    End Sub

    Private Sub dgtxt_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyDown
        With dgv_Details
            vcbo_KeyDwnVal = e.KeyValue
            If .Visible Then
                If e.KeyValue = Keys.Delete Then

                    'If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Then
                    '    If Trim(UCase(cbo_Type.Text)) = "DIRECT" Or Trim(UCase(cbo_Type.Text)) = "RECEIPT" Then
                    '        e.Handled = True
                    '        e.SuppressKeyPress = True
                    '    End If
                    'End If

                End If
            End If
        End With

    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress

        With dgv_Details
            If .Visible Then

                If .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 7 Then

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

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            With dgv_Details

                n = .CurrentRow.Index

                If n = .Rows.Count - 1 Then
                    For i = 0 To .ColumnCount - 1
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
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
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

    Private Sub cbo_Grid_Clothtype_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Clothtype.TextChanged
        Try
            If cbo_Grid_Clothtype.Visible Then
                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_Grid_Clothtype.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Clothtype.Text)
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
                Condt = "a.Cloth_Purchase_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Cloth_Purchase_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Cloth_Purchase_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
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
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " b.Cloth_IdNo = " & Str(Val(Clth_IdNo))
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, c.Cloth_Name, d.ClothType_name, e.Ledger_Name from Cloth_Purchase_Head a LEFT OUTER JOIN Cloth_Purchase_Details b on a.Cloth_Purchase_Code = b.Cloth_Purchase_Code LEFT OUTER join Cloth_head c on b.Cloth_idno = c.Cloth_idno left outer join ClothType_head d on b.ClothType_idno = d.ClothType_idno INNER JOIN Ledger_head e on a.Ledger_idno = e.Ledger_idno where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.Cloth_Purchase_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by Cloth_Purchase_Date, for_orderby, Cloth_Purchase_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Cloth_Purchase_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Cloth_Purchase_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Bill_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(6).Value = dt2.Rows(i).Item("ClothType_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Net_Amount").ToString), "########0.00")

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

    Private Sub cbo_Filter_ClothName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_ClothName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_Filter_ClothName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ClothName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ClothName, cbo_Filter_PartyName, btn_Filter_Show, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_Filter_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ClothName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ClothName, btn_Filter_Show, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_ClothName, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_ClothName, "Ledger_AlaisHead", "Ledger_DisplayName", " (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub txt_ReturnMeters_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_ReturnMeters.KeyDown
        If e.KeyValue = 38 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

            Else
                txt_Vechile.Focus()

            End If
        End If
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub cbo_PurchaseAc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PurchaseAc.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 27)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PurchaseAc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PurchaseAc.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PurchaseAc, txt_ReceiptMeters, cbo_Transport, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 27)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PurchaseAc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PurchaseAc.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PurchaseAc, cbo_Transport, "Ledger_AlaisHead", "Ledger_DisplayName", "(AccountsGroup_IdNo = 27)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PurchaseAc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PurchaseAc.KeyUp
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

    Private Sub cbo_Com_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Com_Type.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Com_Type, txt_com_per, txt_RecNo, "", "", "", "")
    End Sub

    Private Sub cbo_Com_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Com_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Com_Type, txt_RecNo, "", "", "", "")
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

    Private Sub txt_Packing_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Packing.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Trade_Disc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Trade_Disc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_ReceiptMeters_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ReceiptMeters.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Insurance_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Insurance.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
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
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Cash_Disc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Cash_Disc.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Insurance_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Insurance.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Packing_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Packing.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Freight_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Freight.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_com_per_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_com_per.TextChanged
        AgentCommision_Calculation()
    End Sub

    Private Sub cbo_Com_Type_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Com_Type.TextChanged
        AgentCommision_Calculation()
    End Sub

    Private Sub btn_ReceiptSelection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_ReceiptSelection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim CompIDCondt As String
        Dim Ent_Bls As Single = 0
        Dim Ent_BlNos As String = ""
        Dim Ent_Pcs As Single = 0
        Dim Ent_Mtrs As Single = 0
        Dim Ent_Rate As Single = 0


        If Trim(UCase(cbo_Type.Text)) <> "RECEIPT" Then Exit Sub

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)

        If LedIdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT RECEIPT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled And cbo_PartyName.Visible Then cbo_PartyName.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Trim(UCase(Common_Procedures.settings.CompanyName)) = "-----~~~" Then
            CompIDCondt = ""
        End If


        With dgv_Selection

            lbl_Heading_Selection.Text = "RECEIPT SELECTION"

            .Rows.Clear()
            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.*, c.Cloth_Name from Cloth_Purchase_Receipt_Head a INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo  where a.Ledger_IdNo = " & Str(Val(LedIdNo)) & " and a.Cloth_Purchase_Code = '" & Trim(NewCode) & "' order by a.Cloth_Purchase_Receipt_Date, a.for_orderby, a.Cloth_Purchase_Receipt_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Cloth_Purchase_Receipt_No").ToString

                    .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                    .Rows(n).Cells(3).Value = Val(Dt1.Rows(i).Item("noof_Pcs").ToString)
                    .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("receipt_Meters").ToString))

                    .Rows(n).Cells(5).Value = "1"
                    .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Cloth_Purchase_Receipt_Code").ToString

                    .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Bill_No").ToString
                    .Rows(n).Cells(8).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Cloth_Purchase_Receipt_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(9).Value = Val(Dt1.Rows(i).Item("Folding").ToString)

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next
            End If


            Da = New SqlClient.SqlDataAdapter("select a.*, c.Cloth_Name from Cloth_Purchase_Receipt_Head a INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo where a.Ledger_IdNo = " & Str(Val(LedIdNo)) & " and a.Cloth_Purchase_Code = '' order by a.Cloth_Purchase_Receipt_Date, a.for_orderby, a.Cloth_Purchase_Receipt_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1

                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Cloth_Purchase_Receipt_No").ToString
                    .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                    .Rows(n).Cells(3).Value = Val(Dt1.Rows(i).Item("Noof_Pcs").ToString)
                    .Rows(n).Cells(4).Value = Format(Val(Dt1.Rows(i).Item("Receipt_Meters").ToString))
                    .Rows(n).Cells(5).Value = ""
                    .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("Cloth_Purchase_Receipt_Code").ToString
                    .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("bill_No").ToString
                    .Rows(n).Cells(8).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Cloth_Purchase_Receipt_date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(9).Value = Val(Dt1.Rows(i).Item("Folding").ToString)

                Next
            End If
            Dt1.Clear()
        End With

        pnl_Selection.Visible = True
        pnl_Back.Enabled = False
        dgv_Selection.Focus()
        If dgv_Selection.Rows.Count > 0 Then
            dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
            dgv_Selection.CurrentCell.Selected = True
        End If

    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_ClothReceipt(e.RowIndex)
    End Sub

    Private Sub Select_ClothReceipt(ByVal RwIndx As Integer)
        Dim i As Integer, j As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(5).Value = ""
                    For j = 0 To .Columns.Count - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Black
                    Next
                Next

                .Rows(RwIndx).Cells(5).Value = 1

                For i = 0 To .ColumnCount - 1
                    .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                Next

                Close_Receipt_Selection()

            End If

        End With

    End Sub

    Private Sub dgv_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Selection.KeyDown
        Dim n As Integer

        On Error Resume Next

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_Selection.CurrentCell.RowIndex >= 0 Then

                n = dgv_Selection.CurrentCell.RowIndex

                Select_ClothReceipt(n)

                e.Handled = True

            End If
        End If
    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Close_Receipt_Selection()
    End Sub

    Private Sub Close_Receipt_Selection()
        Dim Da2 As New SqlClient.SqlDataAdapter
        Dim Dt2 As New DataTable
        Dim n As Integer, i As Integer, j As Integer
        Dim SNo As Integer
        Dim NewCode As String

        dgv_Details.Rows.Clear()

        txt_BillNo.Text = ""
        txt_Pcs.Text = ""
        txt_ReceiptMeters.Text = ""
        txt_RecDate.Text = ""
        txt_RecNo.Text = ""
        lbl_ReceiptCode.Text = ""

        pnl_Back.Enabled = True

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(5).Value) = 1 Then

                txt_BillNo.Text = dgv_Selection.Rows(i).Cells(7).Value
                txt_Pcs.Text = dgv_Selection.Rows(i).Cells(3).Value
                txt_ReceiptMeters.Text = dgv_Selection.Rows(i).Cells(4).Value
                txt_RecDate.Text = dgv_Selection.Rows(i).Cells(8).Value
                txt_RecNo.Text = dgv_Selection.Rows(i).Cells(1).Value
                lbl_ReceiptCode.Text = dgv_Selection.Rows(i).Cells(6).Value

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

                Da2 = New SqlClient.SqlDataAdapter("Select a.*, c.Cloth_Name, d.ClothType_Name from Cloth_Purchase_Details a INNER JOIN Cloth_Purchase_Head b ON a.Company_IdNo = b.Company_IdNo and a.Cloth_Purchase_Code = b.Cloth_Purchase_Code INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo LEFT OUTER JOIN ClothType_Head d ON a.ClothType_IdNo = d.ClothType_IdNo Where a.Cloth_Purchase_Code = '" & Trim(NewCode) & "' and b.Cloth_Purchase_Receipt_Code = '" & Trim(lbl_ReceiptCode.Text) & "' Order by a.sl_no", con)
                Dt2 = New DataTable
                Da2.Fill(Dt2)

                SNo = 0

                If Dt2.Rows.Count > 0 Then

                    For j = 0 To Dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1

                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(1).Value = Dt2.Rows(j).Item("Cloth_Name").ToString
                        dgv_Details.Rows(n).Cells(2).Value = Dt2.Rows(j).Item("ClothType_Name").ToString
                        dgv_Details.Rows(n).Cells(3).Value = Format(Val(Dt2.Rows(j).Item("Fold_Perc").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(4).Value = Val(Dt2.Rows(j).Item("Pcs").ToString)
                        dgv_Details.Rows(n).Cells(5).Value = Format(Val(Dt2.Rows(j).Item("Meters").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(6).Value = Format(Val(Dt2.Rows(j).Item("Rate").ToString), "########0.00")
                        dgv_Details.Rows(n).Cells(7).Value = Format(Val(Dt2.Rows(j).Item("Amount").ToString), "########0.00")

                    Next

                Else

                    n = dgv_Details.Rows.Add()

                    SNo = SNo + 1
                    dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                    dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(2).Value
                    dgv_Details.Rows(n).Cells(2).Value = Common_Procedures.ClothType.Type1
                    dgv_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(9).Value
                    dgv_Details.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(3).Value
                    dgv_Details.Rows(n).Cells(5).Value = Format(Val(dgv_Selection.Rows(i).Cells(4).Value), "#########0.00")
                    dgv_Details.Rows(n).Cells(6).Value = ""
                    dgv_Details.Rows(n).Cells(7).Value = ""

                End If

                Exit For

            End If

        Next

        Total_Calculation()

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        If txt_BillNo.Enabled And txt_BillNo.Visible Then txt_BillNo.Focus()

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Cloth_Purchase_Entry, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Cloth_Purchase_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Cloth_Purchase_Code = '" & Trim(NewCode) & "'", con)
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

    Private Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        print_record()
    End Sub




    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Ledger_Name as Transport ,e.Ledger_Name as Agent_Name from Cloth_Purchase_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo Left outer JOIN Ledger_Head d ON a.Transport_IdNo = d.Ledger_IdNo Left outer JOIN Ledger_Head e ON a.Agent_IdNo = e.Ledger_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cloth_Purchase_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_name, d.ClothType_name from Cloth_Purchase_Details a INNER JOIN Cloth_Head b ON a.Cloth_idno = b.Cloth_idno LEFT OUTER JOIN ClothType_Head d ON a.ClothType_idno = d.ClothType_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cloth_Purchase_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
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

        Printing_Format1(e)

    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim I As Integer
        Dim NoofItems_PerPage As Integer, NoofDets As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim ItmNm1 As String, ItmNm2 As String

       
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

        NoofItems_PerPage = 18 ' 6

        Erase LnAr
        Erase ClAr
        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(35) : ClAr(2) = 190 : ClAr(3) = 100 : ClAr(4) = 70 : ClAr(5) = 80 : ClAr(6) = 100 : ClAr(7) = 80
        ClAr(8) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7))

        TxtHgt = 18

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

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

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 25 Then
                            For I = 15 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 25
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("ClothType_Name").ToString, LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Fold_Perc").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

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
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim strHeight As Single
        Dim C1 As Single
        Dim W1, w2 As Single
        Dim S1, s2 As Single

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_name, d.ClothType_name from Cloth_Purchase_Details a INNER JOIN Cloth_Head b ON a.Cloth_idno = b.Cloth_idno LEFT OUTER JOIN ClothType_Head d ON a.ClothType_idno = d.ClothType_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Cloth_Purchase_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
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

        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "CLOTH PURCHASE", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
        W1 = e.Graphics.MeasureString("ORDER NO : ", pFont).Width
        w2 = e.Graphics.MeasureString("DESP.TO : ", pFont).Width
        S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width
        s2 = e.Graphics.MeasureString("TRANSPORT :  ", pFont).Width

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cloth_Purchase_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Cloth_Purchase_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt + 10


        Common_Procedures.Print_To_PrintDocument(e, "BILL NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Bill_NO").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)


        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        CurY = CurY + 10

        Common_Procedures.Print_To_PrintDocument(e, "AGENT ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + s2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "REC.NO ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rec_No").ToString, LMargin + s2 + 30, CurY, 0, 0, pFont)


        Common_Procedures.Print_To_PrintDocument(e, "REC.DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Rec_Date").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " :  ", LMargin + s2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport").ToString, LMargin + s2 + 30, CurY, 0, 0, pFont)


        Common_Procedures.Print_To_PrintDocument(e, "VECHILE NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vechile_No").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)




        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "CLOTH NAME", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TYPE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "FOLD%", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY


    End Sub

    
    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim W1 As Single = 0
        Dim vprn_BlNos As String = ""
        Dim rndoff As Double, TtAmt As Double
        Dim BmsInWrds As String


        For i = NoofDets + 1 To NoofItems_PerPage

            CurY = CurY + TxtHgt

            prn_DetIndx = prn_DetIndx + 1

        Next


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        CurY = CurY + TxtHgt - 10

        Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 30, CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)


        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
        CurY = CurY + 10


        If Val(prn_HdDt.Rows(0).Item("Trade_Discount_Perc").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("TradeDisc_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Trade_Discount").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Trade_Discount_Perc").ToString), PageWidth - 10, CurY, 1, 0, pFont)
        End If

        CurY = CurY + TxtHgt


        If Val(prn_HdDt.Rows(0).Item("Cash_Discount_Perc").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("CashDisc_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Cash_Discount").ToString) & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Cash_Discount_Perc").ToString), PageWidth - 10, CurY, 1, 0, pFont)
        End If

        CurY = CurY + TxtHgt

        If Val(prn_HdDt.Rows(0).Item("Packing_Amount").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Packing_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Packing_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
        End If

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 11, FontStyle.Bold)

        If Val(prn_HdDt.Rows(0).Item("Freight").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Freight_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Freight").ToString), PageWidth - 10, CurY, 1, 0, pFont)
        End If

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 11, FontStyle.Bold)

        If Val(prn_HdDt.Rows(0).Item("Insurance").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Insurance_Name").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Insurance").ToString), PageWidth - 10, CurY, 1, 0, pFont)
        End If


        TtAmt = Format(Val(prn_HdDt.Rows(0).Item("total_Amount").ToString) + Val(prn_HdDt.Rows(0).Item("Freight").ToString) + Val(prn_HdDt.Rows(0).Item("Insurance").ToString) + Val(prn_HdDt.Rows(0).Item("Packing_amount").ToString) - Val(prn_HdDt.Rows(0).Item("Trade_Discount_Perc").ToString) - Val(prn_HdDt.Rows(0).Item("Cash_Discount_Perc").ToString), "#########0.00")

        rndoff = 0
        rndoff = Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString) - Val(TtAmt)

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 11, FontStyle.Bold)

        If Val(rndoff) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "ROUND OFF", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 0, 0, pFont)
            If Val(rndoff) >= 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 20, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 20, CurY, 0, 0, pFont)
            End If
            Common_Procedures.Print_To_PrintDocument(e, " " & Format(Math.Abs(Val(rndoff)), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
        End If

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 11, FontStyle.Bold)

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, PageWidth, CurY)
        LnAr(8) = CurY

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        CurY = CurY + TxtHgt ' 10
        Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, " " & Trim(Common_Procedures.Currency_Format(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))), PageWidth - 10, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(9) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))

        BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
        CurY = CurY + 10
        Common_Procedures.Print_To_PrintDocument(e, "Rupees  : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)






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

    End Sub
    Private Sub txt_Vechile_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Vechile.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")

        If (e.KeyValue = 40) Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

            Else
                txt_ReturnMeters.Focus()

            End If
        End If

    End Sub

    Private Sub txt_Vechile_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Vechile.KeyPress

        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                dgv_Details.CurrentCell.Selected = True

            Else
                cbo_Transport.Focus()

            End If


        End If
    End Sub

    Private Sub txt_Note_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Note.KeyDown
        If e.KeyValue = 38 Then
            txt_Insurance.Focus()
        End If

        If e.KeyValue = 40 Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Note_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Note.KeyPress
        If Asc(e.KeyChar) = 13 Then


            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_ExcShtMeters_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ExcShtMeters.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_ReturnMeters_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ReturnMeters.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Pcs_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Pcs.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Bundles_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Bundles.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_ExcShtMeters_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_ExcShtMeters.TextChanged
        TotalMeter_Calculation()
    End Sub

    Private Sub txt_ReturnMeters_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_ReturnMeters.TextChanged
        TotalMeter_Calculation()
    End Sub

    Private Sub TotalMeter_Calculation()
        Dim PurcMtrs As Single = 0

        'Total_Calculation()
        PurcMtrs = 0
        If dgv_Details_Total.Rows.Count > 0 Then
            PurcMtrs = Val(dgv_Details_Total.Rows(0).Cells(5).Value)
        End If
        lbl_TotalMeters.Text = Format(Val(PurcMtrs) + Val(txt_ReturnMeters.Text) - Val(txt_ExcShtMeters.Text), "#########0.00")

    End Sub

    Private Sub chk_No_Folding_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_No_Folding.Click
        Dim i As Integer = 0

        On Error Resume Next

        With dgv_Details
            If .Visible Then

                For i = 0 To .Rows.Count - 1
                    Amount_Calculation(i, 3)
                Next

            End If
        End With
    End Sub

    Private Sub msk_date_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles msk_date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_date.Text = Date.Today
            msk_date.SelectionStart = 0
        End If
    End Sub


    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    msk_date.Text = Date.Today
        'End If
        If e.KeyCode = 107 Then
            msk_date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_date.Text))
        ElseIf e.KeyCode = 109 Then
            msk_date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_date.Text))
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then

            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)

        End If
    End Sub

    Private Sub dtp_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_Date.Text = Date.Today
        End If
    End Sub
    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        If IsDate(dtp_Date.Text) = True Then

            msk_date.Text = dtp_Date.Text
            msk_date.SelectionStart = 0
        End If
    End Sub

    Private Sub msk_Date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_date.LostFocus

        If IsDate(msk_date.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_date.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_date.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_date.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_date.Text)) >= 2000 Then
                    dtp_Date.Value = Convert.ToDateTime(msk_date.Text)
                End If
            End If

        End If
    End Sub
    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_date.Text
            vmskSelStrt = msk_date.SelectionStart
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

        LastNo = lbl_RefNo.Text

        movefirst_record()
        Timer1.Enabled = True

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        save_record()
        If Trim(UCase(LastNo)) = Trim(UCase(lbl_RefNo.Text)) Then
            Timer1.Enabled = False
            SaveAll_STS = False
            MessageBox.Show("All entries saved Successfully", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Else
            movenext_record()

        End If
    End Sub

    Private Sub txt_Vechile_TextChanged(sender As Object, e As EventArgs) Handles txt_Vechile.TextChanged

    End Sub
End Class