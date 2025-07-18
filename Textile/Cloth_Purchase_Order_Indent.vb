Public Class Cloth_Purchase_Order_Indent
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "CPORD-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Single
    Private vCbo_ItmNm As String
    Private Print_PDF_Status As Boolean = False
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Public Shared EntFnYrCode As String = ""
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1
    Private Enum dgvCol_DelvDetails As Integer
        SlNo
        Cloth_name
        Loom_Type
        Type
        Fold_perc
        bales
        Order_pCs
        Order_Mtrs
        RAte
        AMount
        Cancel_Mtrs
        Cloth_Purchase_Order_Slno
        Cloth_Purchase_meters
        Offer_No
        Cloth_Purchase_Offer_Code
        Cloth_Purchase_Offer_Slno
        Disc_perc
        Disc_AMt
        Taxable_value
        Gst_Perc
        Hsn_Code



    End Enum

    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Additional_Details.Visible = False

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        vmskOldText = ""
        vmskSelStrt = -1
        cbo_freight_Ac.Text = ""

        txt_delivery_due_days.Text = ""
        msk_delivery_date.Text = ""
        Print_PDF_Status = False
        txt_GrTime.Text = ""
        msk_GrDate.Text = ""
        msk_date.Text = ""
        dtp_Date.Text = ""
        cbo_PartyName.Text = ""
        cbo_Agent.Text = ""
        cbo_Through.Text = "DIRECT"
        cbo_Transport.Text = ""
        cbo_DespTo.Text = ""
        cbo_Grid_ClothName.Text = ""
        cbo_Grid_LoomType.Text = ""
        cbo_Grid_Clothtype.Text = ""
        cbo_Com_Type.Text = "%"
        cbo_Type.Text = "DIRECT"
        txt_com_per.Text = ""
        txt_OrderNo.Text = ""
        txt_DelvAdd1.Text = ""
        txt_DelvAdd2.Text = ""
        txt_Note.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))

        txt_delivery_Schedule.Text = ""
        txt_PaymentTerms.Text = ""

        cbo_taxType.Text = ""
        txt_Fabric_Po_PrefixNo.Text = ""
        cbo_Fabric_Po_SufixNo.Text = ""

        txt_Attend.Text = ""

        lbl_GrossAmount.Text = ""

        txt_DiscPerc.Text = ""
        lbl_DiscAmount.Text = ""

        txt_AssessableValue.Text = ""


        lbl_CGST_Amount.Text = ""
        lbl_SGST_Amount.Text = ""
        lbl_IGST_Amount.Text = ""

        txt_Freight.Text = ""
        txt_AddLessAfterTax_Text.Text = "Add/Less"
        txt_AddLess_AfterTax.Text = ""
        txt_AddLess_BeforeTax.Text = ""
        lbl_RoundOff.Text = ""
        lbl_NetAmount.Text = "0.00"

        ' ----------------


        txt_Payment_Terms_Details.Text = ""
        txt_Delivery_Terms.Text = ""
        txt_Po_Completion_Date.Text = ""
        txt_Packing.Text = ""
        txt_Roll_length.Text = ""
        txt_Quality_Standard.Text = ""
        txt_Reed_Count.Text = ""
        txt_Slevedge_Details.Text = ""
        txt_Qty_Tolenrance.Text = ""

        txt_TermsCond_1.Text = "Yarn Price  -  Warp Yarn – Rs. 0/- Exmill ; Weft Yarn – Rs. 0/- Exmill"
        txt_TermsCond_2.Text = "Width and construction of the fabric should be same as mentioned In the Po"
        txt_TermsCond_3.Text = "Order should be delivered within delivery date in the PO"
        txt_TermsCond_4.Text = "If any defect found we will return the fabric at your cost"
        txt_TermsCond_5.Text = ""
        txt_TermsCond_6.Text = ""


        ' ---------------


        cbo_PartyName.Enabled = True
        cbo_PartyName.BackColor = Color.White

        cbo_Agent.Enabled = True
        cbo_Agent.BackColor = Color.White

        'cbo_DespTo.Enabled = True
        'cbo_DespTo.BackColor = Color.White

        'cbo_Through.Enabled = True
        'cbo_Through.BackColor = Color.White

        cbo_Transport.Enabled = True
        cbo_Transport.BackColor = Color.White

        'txt_OrderNo.Enabled = True
        'txt_OrderNo.BackColor = Color.White

        'txt_DelvAdd1.Enabled = True
        'txt_DelvAdd1.BackColor = Color.White

        'txt_DelvAdd2.Enabled = True
        'txt_DelvAdd2.BackColor = Color.White

        txt_com_per.Enabled = True
        txt_com_per.BackColor = Color.White

        cbo_Com_Type.Enabled = True
        cbo_Com_Type.BackColor = Color.White

        cbo_Grid_ClothName.Enabled = True
        cbo_Grid_ClothName.BackColor = Color.White


        cbo_Grid_LoomType.Enabled = True
        cbo_Grid_LoomType.BackColor = Color.White

        cbo_Grid_Clothtype.Enabled = True
        cbo_Grid_Clothtype.BackColor = Color.White

        Transport.Rows.Clear()
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
        cbo_Grid_Clothtype.Visible = False

        cbo_Grid_LoomType.Visible = False

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
        End If
        If Me.ActiveControl.Name <> cbo_Grid_LoomType.Name Then
            cbo_Grid_LoomType.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_Clothtype.Name Then
            cbo_Grid_Clothtype.Visible = False
        End If

        If Me.ActiveControl.Name <> Transport.Name Then
            Grid_DeSelect()
        End If

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

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(Transport.CurrentCell) Then Transport.CurrentCell.Selected = False
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
        If Not IsNothing(Transport.CurrentCell) Then Transport.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False
        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub ClothSales_Order_Indent_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Agent.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "AGENT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Agent.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_ClothName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH NAME" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_ClothName.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Clothtype.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH TYPE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Clothtype.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_LoomType.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LOOM TYPE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_LoomType.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub ClothSales_Order_Indent_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub ClothSales_Order_Indent_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub
                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
                    Exit Sub
                ElseIf Pnl_Terms_and_Condition.Visible = True Then
                    btn_pnl_Terms_Condition_Close_Click(sender, e)
                    Exit Sub
                ElseIf pnl_Additional_Details.Visible = True Then
                    btn_Additional_Details_Close_Click(sender, e)
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

    Private Sub ClothSales_Order_Indent_Entry_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Dim dt7 As New DataTable
        Dim dt8 As New DataTable
        Dim OpYrCode As String = ""
        Me.Text = ""

        con.Open()

        If Trim(UCase(Common_Procedures.ClothOrder_Opening_OR_Entry)) = "OPENING" Then
            OpYrCode = Microsoft.VisualBasic.Left(Trim(Common_Procedures.FnRange), 4)
            OpYrCode = Trim(Mid(Val(OpYrCode) - 1, 3, 2)) & "-" & Trim(Microsoft.VisualBasic.Right(OpYrCode, 2))

            EntFnYrCode = OpYrCode


        Else
            EntFnYrCode = Common_Procedures.FnYearCode

        End If


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

        da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
        da.Fill(dt4)
        cbo_Grid_ClothName.DataSource = dt4
        cbo_Grid_ClothName.DisplayMember = "Cloth_Name"

        da = New SqlClient.SqlDataAdapter("select ClothType_Name from ClothType_Head order by ClothType_Name", con)
        da.Fill(dt5)
        cbo_Grid_Clothtype.DataSource = dt5
        cbo_Grid_Clothtype.DisplayMember = "ClothType_Name"

        'da = New SqlClient.SqlDataAdapter("select distinct(Despatch_To) from ClothPurchase_Order_Head order by Despatch_To", con)
        'da.Fill(dt6)
        'cbo_DespTo.DataSource = dt6
        'cbo_DespTo.DisplayMember = "Despatch_To"


        cbo_Com_Type.Items.Clear()
        cbo_Com_Type.Items.Add("%")
        cbo_Com_Type.Items.Add("MTR")

        cbo_Through.Items.Clear()
        cbo_Through.Items.Add(" ")
        cbo_Through.Items.Add("DIRECT")
        cbo_Through.Items.Add("BANK")
        cbo_Through.Items.Add("AGENT")

        cbo_Type.Items.Clear()
        cbo_Type.Items.Add(" ")
        cbo_Type.Items.Add("DIRECT")
        cbo_Type.Items.Add("OFFER")

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        pnl_Tax.Visible = False
        pnl_Tax.Left = (Me.Width - pnl_Tax.Width) \ 2
        pnl_Tax.Top = (Me.Height - pnl_Tax.Height) \ 2
        pnl_Tax.BringToFront()

        Btn_Additional_Details.Visible = False
        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If


        cbo_Fabric_Po_SufixNo.Items.Clear()
        cbo_Fabric_Po_SufixNo.Items.Add("")
        cbo_Fabric_Po_SufixNo.Items.Add("/" & Common_Procedures.FnYearCode)
        cbo_Fabric_Po_SufixNo.Items.Add("/" & Year(Common_Procedures.Company_FromDate) & "-" & Year(Common_Procedures.Company_ToDate))
        cbo_Fabric_Po_SufixNo.Items.Add("/" & Year(Common_Procedures.Company_FromDate))
        cbo_Fabric_Po_SufixNo.Items.Add("/" & Trim(Year(Common_Procedures.Company_FromDate)) & "-" & Trim(Microsoft.VisualBasic.Right(Year(Common_Procedures.Company_ToDate), 2)))

        txt_caption_attend.Visible = True
        txt_Attend.Visible = True
        Transport.Columns(dgvCol_DelvDetails.Loom_Type).Visible = False
        If Trim(Common_Procedures.settings.CustomerCode) = "1186" Then

            Label21.Text = "Cr Days"
            Label20.Text = "Due Date"
            Transport.Columns(dgvCol_DelvDetails.Cloth_name).Width = Transport.Columns(dgvCol_DelvDetails.Cloth_name).Width + Transport.Columns(dgvCol_DelvDetails.Order_pCs).Width + Transport.Columns(dgvCol_DelvDetails.Fold_perc).Width
            Transport.Columns(dgvCol_DelvDetails.Loom_Type).Visible = True
            Transport.Columns(dgvCol_DelvDetails.Fold_perc).Visible = False
            Transport.Columns(dgvCol_DelvDetails.Order_pCs).Visible = False
            Transport.Columns(dgvCol_DelvDetails.Type).Visible = False
            Label41.Visible = False
            cbo_Transport.Visible = False
            Label26.Visible = True
            txt_delivery_due_days.Visible = True

            Label33.Visible = True
            msk_delivery_date.Visible = True
            dtp_delivery_date.Visible = True

            Label35.Visible = True
            cbo_freight_Ac.Visible = True
            Label36.Visible = True
            cbo_taxType.Visible = True

        End If

        If Trim(Common_Procedures.settings.CustomerCode) = "1464" Then ' -- MOF 
            Btn_Additional_Details.Visible = True
        End If


        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Type.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Agent.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Com_Type.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DespTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Clothtype.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Through.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_com_per.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_OrderNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DelvAdd1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DelvAdd2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Note.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_GrDate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_GrTime.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DiscPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Attend.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_delivery_due_days.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_delivery_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_freight_Ac.GotFocus, AddressOf ControlGotFocus


        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLessAfterTax_Text.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLess_AfterTax.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_AddLess_BeforeTax.GotFocus, AddressOf ControlGotFocus


        AddHandler txt_PaymentTerms.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_delivery_Schedule.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_taxType.GotFocus, AddressOf ControlGotFocus



        AddHandler cbo_taxType.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_PaymentTerms.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_delivery_Schedule.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_delivery_due_days.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_delivery_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_freight_Ac.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Fabric_Po_SufixNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Fabric_Po_SufixNo.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Fabric_Po_PrefixNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Fabric_Po_PrefixNo.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Grid_LoomType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_LoomType.LostFocus, AddressOf ControlLostFocus


        AddHandler txt_Attend.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Attend.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Attend.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Agent.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Com_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DespTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Clothtype.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Through.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_com_per.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DelvAdd1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DelvAdd2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_OrderNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Note.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_GrDate.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_GrTime.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DiscPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_AddLessAfterTax_Text.LostFocus, AddressOf ControlLostFocus1

        AddHandler txt_AddLess_AfterTax.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_AddLess_BeforeTax.LostFocus, AddressOf ControlLostFocus

        AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_com_per.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DelvAdd1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_OrderNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_GrDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_GrTime.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AddLessAfterTax_Text.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_AddLess_AfterTax.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PaymentTerms.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_delivery_Schedule.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_delivery_due_days.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler msk_delivery_date.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_delivery_Schedule.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PaymentTerms.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_DiscPerc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Freight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AddLessAfterTax_Text.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_AddLess_AfterTax.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_com_per.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_DelvAdd1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_OrderNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler msk_GrDate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_GrTime.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_delivery_due_days.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler msk_delivery_date.KeyPress, AddressOf TextBoxControlKeyPress


        AddHandler txt_Payment_Terms_Details.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Delivery_Terms.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Po_Completion_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Packing.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Roll_length.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Quality_Standard.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Reed_Count.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Slevedge_Details.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Qty_Tolenrance.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TermsCond_1.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TermsCond_2.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TermsCond_3.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TermsCond_4.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TermsCond_5.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TermsCond_6.GotFocus, AddressOf ControlGotFocus


        AddHandler txt_Payment_Terms_Details.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Delivery_Terms.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Po_Completion_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Packing.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Roll_length.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Quality_Standard.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Reed_Count.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Slevedge_Details.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Qty_Tolenrance.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TermsCond_1.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TermsCond_2.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TermsCond_3.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TermsCond_4.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TermsCond_5.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TermsCond_6.LostFocus, AddressOf ControlLostFocus


        AddHandler txt_Payment_Terms_Details.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Delivery_Terms.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Po_Completion_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Packing.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Roll_length.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Quality_Standard.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Reed_Count.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Slevedge_Details.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler txt_Qty_Tolenrance.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TermsCond_1.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TermsCond_2.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TermsCond_3.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TermsCond_4.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_TermsCond_5.KeyPress, AddressOf TextBoxControlKeyPress
        '  AddHandler txt_TermsCond_6.KeyPress, AddressOf TextBoxControlKeyPress


        '   AddHandler txt_Payment_Terms_Details.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Delivery_Terms.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Po_Completion_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Packing.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Roll_length.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Quality_Standard.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Reed_Count.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Slevedge_Details.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_Qty_Tolenrance.KeyDown, AddressOf TextBoxControlKeyDown
        '   AddHandler txt_TermsCond_1.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TermsCond_2.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TermsCond_3.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TermsCond_4.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_TermsCond_5.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_TermsCond_6.KeyDown, AddressOf TextBoxControlKeyDown





        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()
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

        If ActiveControl.Name = Transport.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = Transport.Name Then
                dgv1 = Transport

            ElseIf Transport.IsCurrentRowDirty = True Then
                dgv1 = Transport

            ElseIf TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then
                dgv1 = Transport

            End If

            If IsNothing(dgv1) = False Then

                With dgv1


                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 6 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                txt_Note.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(dgvCol_DelvDetails.Cloth_name)

                            End If
                        ElseIf .CurrentCell.ColumnIndex = 6 Then
                            If Trim(UCase(cbo_Type.Text)) = "OFFER" Then
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.Cancel_Mtrs)
                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.Order_Mtrs)
                            End If
                        ElseIf .CurrentCell.ColumnIndex = dgvCol_DelvDetails.Cancel_Mtrs Then

                            txt_DiscPerc.Focus()
                        ElseIf .CurrentCell.ColumnIndex = dgvCol_DelvDetails.Fold_perc Then

                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.Order_pCs)

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                txt_DelvAdd2.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 6)

                            End If
                        ElseIf .CurrentCell.ColumnIndex = 8 Then
                            If Trim(UCase(cbo_Type.Text)) = "OFFER" Then
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.Order_Mtrs)
                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.RAte)
                            End If
                        ElseIf .CurrentCell.ColumnIndex = dgvCol_DelvDetails.Cancel_Mtrs Then

                            txt_DiscPerc.Focus()
                        ElseIf .CurrentCell.ColumnIndex = 5 Then

                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.Fold_perc)
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
        Dim da4 As New SqlClient.SqlDataAdapter
        Dim dt4 As New DataTable

        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer
        Dim LockSTS As Boolean = False
        If Val(no) = 0 Then Exit Sub

        clear()

        NoCalc_Status = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(EntFnYrCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from ClothPurchase_Order_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothPurchase_Order_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                cbo_Fabric_Po_SufixNo.Text = dt1.Rows(0).Item("ClothPurchase_Order_SuffixNo").ToString
                txt_Fabric_Po_PrefixNo.Text = dt1.Rows(0).Item("ClothPurchase_Order_PrefixNo").ToString
                lbl_RefNo.Text = dt1.Rows(0).Item("ClothPurchase_Order_RefNo").ToString
                'lbl_RefNo.Text = dt1.Rows(0).Item("ClothPurchase_Order_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("ClothPurchase_Order_Date").ToString
                msk_date.Text = dtp_Date.Text

                cbo_PartyName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                cbo_Agent.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Agent_IdNo").ToString))
                cbo_Transport.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Transport_IdNo").ToString))
                cbo_Type.Text = dt1.Rows(0).Item("Selection_Type").ToString
                cbo_Com_Type.Text = dt1.Rows(0).Item("Agent_Comm_Type").ToString
                cbo_DespTo.Text = Common_Procedures.Ledger_IdNoToName(con, dt1.Rows(0).Item("Despatch_IdNo").ToString)

                cbo_Through.Text = dt1.Rows(0).Item("Through_Name").ToString

                msk_GrDate.Text = dt1.Rows(0).Item("Gr_Date").ToString
                txt_GrTime.Text = dt1.Rows(0).Item("Gr_Time").ToString


                msk_delivery_date.Text = dt1.Rows(0).Item("Delivery_due_date").ToString
                txt_delivery_due_days.Text = dt1.Rows(0).Item("Delivery_Due_days").ToString
                cbo_freight_Ac.Text = dt1.Rows(0).Item("Transportations").ToString
                cbo_taxType.Text = dt1.Rows(0).Item("Tax_Type").ToString

                txt_com_per.Text = dt1.Rows(0).Item("Agent_Comm_Perc").ToString
                txt_OrderNo.Text = dt1.Rows(0).Item("Party_OrderNo").ToString
                txt_DelvAdd1.Text = dt1.Rows(0).Item("Delivery_Address1").ToString
                txt_DelvAdd2.Text = dt1.Rows(0).Item("Delivery_Address2").ToString
                txt_Note.Text = dt1.Rows(0).Item("Note").ToString
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

                txt_DiscPerc.Text = Val(dt1.Rows(0).Item("Discount_Percentage").ToString)
                lbl_DiscAmount.Text = Format(Val(dt1.Rows(0).Item("Discount_Amount").ToString), "#########0.00")
                txt_AssessableValue.Text = Format(Val(dt1.Rows(0).Item("Assessable_Value").ToString), "#########0.00")

                lbl_GrossAmount.Text = Format(Val(dt1.Rows(0).Item("Total_Amount").ToString), "#########0.00")

                lbl_CGST_Amount.Text = Format(Val(dt1.Rows(0).Item("Total_CGST_Amount").ToString), "#########0.00")
                lbl_SGST_Amount.Text = Format(Val(dt1.Rows(0).Item("Total_SGST_Amount").ToString), "#########0.00")
                lbl_IGST_Amount.Text = Format(Val(dt1.Rows(0).Item("Total_IGST_Amount").ToString), "#########0.00")
                txt_Attend.Text = dt1.Rows(0).Item("Attend").ToString
                txt_AddLess_BeforeTax.Text = Format(Val(dt1.Rows(0).Item("AddLess_BeforeTax_Amount").ToString), "#########0.00")
                txt_Freight.Text = Format(Val(dt1.Rows(0).Item("Freight_Amount").ToString), "#########0.00")
                txt_AddLessAfterTax_Text.Text = dt1.Rows(0).Item("AddLessAfterTax_Text").ToString
                If Trim(txt_AddLessAfterTax_Text.Text) = "" Then txt_AddLessAfterTax_Text.Text = "Add/Less"
                txt_AddLess_AfterTax.Text = Format(Val(dt1.Rows(0).Item("AddLess_Amount").ToString), "#########0.00")
                lbl_RoundOff.Text = Format(Val(dt1.Rows(0).Item("RoundOff_Amount").ToString), "#########0.00")
                lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(dt1.Rows(0).Item("Net_Amount").ToString))

                txt_delivery_Schedule.Text = dt1.Rows(0).Item("Delivery_Schedule").ToString
                txt_PaymentTerms.Text = dt1.Rows(0).Item("Payment_Terms").ToString


                txt_Payment_Terms_Details.Text = dt1.Rows(0).Item("Payment_Terms_Details").ToString
                txt_Delivery_Terms.Text = dt1.Rows(0).Item("Delivery_Terms_Details").ToString
                txt_Po_Completion_Date.Text = dt1.Rows(0).Item("Po_Completion_Date").ToString
                txt_Packing.Text = dt1.Rows(0).Item("Packing").ToString
                txt_Roll_length.Text = dt1.Rows(0).Item("Roll_Length").ToString
                txt_Quality_Standard.Text = dt1.Rows(0).Item("Quality_Standard").ToString
                txt_Reed_Count.Text = dt1.Rows(0).Item("Reed_Count").ToString
                txt_Slevedge_Details.Text = dt1.Rows(0).Item("Slevedge_Details").ToString
                txt_Qty_Tolenrance.Text = dt1.Rows(0).Item("Quantity_Tolerance").ToString

                txt_TermsCond_1.Text = dt1.Rows(0).Item("Terms_And_Condition_1").ToString
                txt_TermsCond_2.Text = dt1.Rows(0).Item("Terms_And_Condition_2").ToString
                txt_TermsCond_3.Text = dt1.Rows(0).Item("Terms_And_Condition_3").ToString
                txt_TermsCond_4.Text = dt1.Rows(0).Item("Terms_And_Condition_4").ToString
                txt_TermsCond_5.Text = dt1.Rows(0).Item("Terms_And_Condition_5").ToString
                txt_TermsCond_6.Text = dt1.Rows(0).Item("Terms_And_Condition_6").ToString







                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Cloth_Name, c.ClothType_Name,L.LoomType_Name from ClothPurchase_Order_Details a LEFT OUTER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo LEFT OUTER JOIN LoomType_Head L ON a.loomType_IdNo = L.LoomType_IdNo LEFT OUTER JOIN ClothType_Head c ON a.ClothType_IdNo = c.ClothType_IdNo Where a.ClothPurchase_Order_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With Transport

                    .Rows.Clear()
                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(dgvCol_DelvDetails.SlNo).Value = Val(SNo)
                            .Rows(n).Cells(dgvCol_DelvDetails.Cloth_name).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                            .Rows(n).Cells(dgvCol_DelvDetails.Type).Value = dt2.Rows(i).Item("ClothType_Name").ToString
                            If Val(dt2.Rows(i).Item("Fold_Perc").ToString) <> 0 Then
                                .Rows(n).Cells(dgvCol_DelvDetails.Fold_perc).Value = Format(Val(dt2.Rows(i).Item("Fold_Perc").ToString), "########0.00")
                            End If
                            If Val(dt2.Rows(i).Item("Bales").ToString) <> 0 Then
                                .Rows(n).Cells(dgvCol_DelvDetails.bales).Value = Val(dt2.Rows(i).Item("Bales").ToString)
                            End If
                            If Val(dt2.Rows(i).Item("Order_Pcs").ToString) <> 0 Then
                                .Rows(n).Cells(dgvCol_DelvDetails.Order_pCs).Value = Val(dt2.Rows(i).Item("Order_Pcs").ToString)
                            End If
                            .Rows(n).Cells(dgvCol_DelvDetails.Order_Mtrs).Value = Format(Val(dt2.Rows(i).Item("Order_Meters").ToString), "########0.00")
                            If Val(dt2.Rows(i).Item("Rate").ToString) <> 0 Then
                                .Rows(n).Cells(dgvCol_DelvDetails.RAte).Value = Format(Val(dt2.Rows(i).Item("Rate").ToString), "########0.00")
                            End If
                            If Val(dt2.Rows(i).Item("Order_Cancel_Meters").ToString) <> 0 Then
                                .Rows(n).Cells(dgvCol_DelvDetails.Cancel_Mtrs).Value = Format(Val(dt2.Rows(i).Item("Order_Cancel_Meters").ToString), "########0.00")
                            End If
                            .Rows(n).Cells(dgvCol_DelvDetails.Cloth_Purchase_Order_Slno).Value = dt2.Rows(i).Item("ClothPurchase_Order_SlNo").ToString
                            .Rows(n).Cells(dgvCol_DelvDetails.Cloth_Purchase_meters).Value = dt2.Rows(i).Item("Purchase_Meters").ToString

                            .Rows(n).Cells(dgvCol_DelvDetails.Offer_No).Value = dt2.Rows(i).Item("ClothPurchase_Offer_No").ToString
                            .Rows(n).Cells(dgvCol_DelvDetails.Cloth_Purchase_Offer_Code).Value = dt2.Rows(i).Item("ClothPurchase_Offer_Code").ToString
                            .Rows(n).Cells(dgvCol_DelvDetails.Cloth_Purchase_Offer_Slno).Value = dt2.Rows(i).Item("ClothPurchase_Offer_Slno").ToString
                            If Val(dt2.Rows(i).Item("Amount").ToString) <> 0 Then
                                .Rows(n).Cells(dgvCol_DelvDetails.AMount).Value = Format(Val(dt2.Rows(i).Item("Amount").ToString), "########0.00")
                            End If
                            If Str(Val(dt2.Rows(i).Item("Purchase_Meters").ToString)) <> 0 Then
                                For j = 0 To .ColumnCount - 1
                                    If j <> dgvCol_DelvDetails.RAte And j <> dgvCol_DelvDetails.Cancel_Mtrs Then
                                        .Rows(n).Cells(j).Style.BackColor = Color.LightGray
                                    End If
                                Next j
                                LockSTS = True
                            End If
                            .Rows(n).Cells(dgvCol_DelvDetails.Loom_Type).Value = dt2.Rows(i).Item("LoomType_name").ToString

                        Next i

                    End If

                    If .RowCount = 0 Then .Rows.Add()

                End With

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(4).Value = Val(dt1.Rows(0).Item("Total_Bales").ToString)
                    .Rows(0).Cells(5).Value = Val(dt1.Rows(0).Item("Total_Pcs").ToString)
                    .Rows(0).Cells(6).Value = Format(Val(dt1.Rows(0).Item("Total_Order_Meters").ToString), "########0.00")
                    .Rows(0).Cells(8).Value = Format(Val(dt1.Rows(0).Item("Total_Cancel_Meters").ToString), "########0.00")
                End With
                da4 = New SqlClient.SqlDataAdapter("Select a.* from cloth_Purchase_Order_GST_Tax_Details a Where a.cloth_Purchase_Order_Code = '" & Trim(NewCode) & "' ", con)
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

            Grid_Cell_DeSelect()

            If LockSTS = True Then

                cbo_PartyName.Enabled = False
                cbo_PartyName.BackColor = Color.LightGray

                cbo_Agent.Enabled = False
                cbo_Agent.BackColor = Color.LightGray

                'cbo_DespTo.Enabled = False
                'cbo_DespTo.BackColor = Color.LightGray

                'cbo_Through.Enabled = False
                'cbo_Through.BackColor = Color.LightGray


                cbo_Transport.Enabled = False
                cbo_Transport.BackColor = Color.LightGray

                'txt_OrderNo.Enabled = False
                'txt_OrderNo.BackColor = Color.LightGray

                'txt_DelvAdd1.Enabled = False
                'txt_DelvAdd1.BackColor = Color.LightGray

                cbo_Grid_LoomType.Enabled = False
                cbo_Grid_LoomType.BackColor = Color.LightGray

                txt_com_per.Enabled = True
                txt_com_per.BackColor = Color.LightGray

                cbo_Com_Type.Enabled = True
                cbo_Com_Type.BackColor = Color.LightGray

                cbo_Grid_ClothName.Enabled = True
                cbo_Grid_ClothName.BackColor = Color.LightGray

                cbo_Grid_Clothtype.Enabled = True
                cbo_Grid_Clothtype.BackColor = Color.LightGray

            End If

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

        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.ClothSales_Order_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.ClothSales_Order_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Cloth_Purchase_order_Entry, New_Entry, Me, con, "ClothPurchase_Order_Head", "ClothPurchase_Order_Code", NewCode, "ClothPurchase_Order_Date", "(ClothPurchase_Order_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub


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



        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        Da = New SqlClient.SqlDataAdapter("select sum(Purchase_Meters) from ClothPurchase_Order_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothPurchase_Order_Code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already some pieces Purchased for this order", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()



        trans = con.BeginTransaction
        Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "ClothPurchase_Order_Head", "ClothPurchase_Order_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "ClothPurchase_Order_Code, Company_IdNo, for_OrderBy", trans)

        Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "ClothPurchase_Order_Details", "ClothPurchase_Order_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "Cloth_IdNo,ClothType_IdNo,Fold_Perc,Bales,Order_Pcs,Order_Meters,Rate,Order_Cancel_Meters,ClothPurchase_Offer_No,ClothPurchase_Offer_Code,ClothPurchase_Offer_Slno ,Selection_Type", "Sl_No", "ClothPurchase_Order_Code, For_OrderBy, Company_IdNo, ClothPurchase_Order_No, ClothPurchase_Order_Date, Ledger_Idno", trans)

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

            cmd.Connection = con
            cmd.Transaction = trans
            cmd.CommandText = "Update ClothPurchase_Offer_Details set Order_Meters = a.Order_Meters - b.Order_Meters from ClothPurchase_Offer_Details a, ClothPurchase_Order_Details b Where b.ClothPurchase_Order_Code = '" & Trim(NewCode) & "' and b.Selection_Type = 'OFFER' and a.ClothPurchase_Offer_Code = b.ClothPurchase_Offer_Code and a.ClothPurchase_Offer_Slno = b.ClothPurchase_Offer_Slno"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from ClothPurchase_Order_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothPurchase_Order_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from ClothPurchase_Order_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothPurchase_Order_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from cloth_Purchase_Order_GST_Tax_Details  where company_idno = " & Str(Val(lbl_Company.Tag)) & " and cloth_Purchase_Order_Code = '" & Trim(NewCode) & "'"
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

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead  Where ( Ledger_IdNo = 0 or (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) ) order by Ledger_DisplayName", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 ClothPurchase_Order_RefNo from ClothPurchase_Order_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothPurchase_Order_Code like '%/" & Trim(EntFnYrCode) & "' Order by for_Orderby, ClothPurchase_Order_RefNo", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 ClothPurchase_Order_RefNo from ClothPurchase_Order_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothPurchase_Order_Code like '%/" & Trim(EntFnYrCode) & "' Order by for_Orderby, ClothPurchase_Order_RefNo", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 ClothPurchase_Order_RefNo from ClothPurchase_Order_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothPurchase_Order_Code like '%/" & Trim(EntFnYrCode) & "' Order by for_Orderby desc, ClothPurchase_Order_RefNo desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 ClothPurchase_Order_RefNo from ClothPurchase_Order_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothPurchase_Order_Code like '%/" & Trim(EntFnYrCode) & "' Order by for_Orderby desc, ClothPurchase_Order_RefNo desc", con)
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
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        Try
            clear()

            New_Entry = True

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "ClothPurchase_Order_Head", "ClothPurchase_Order_Code", "For_OrderBy", "", Val(lbl_Company.Tag), EntFnYrCode)
            lbl_RefNo.ForeColor = Color.Red

            msk_date.Text = Date.Today.ToShortDateString
            Da = New SqlClient.SqlDataAdapter("select top 1 * from ClothPurchase_Order_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothPurchase_Order_Code like '%/" & Trim(EntFnYrCode) & "' Order by for_Orderby desc, ClothPurchase_Order_RefNo desc", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If Dt1.Rows(0).Item("ClothPurchase_Order_Date").ToString <> "" Then msk_date.Text = Dt1.Rows(0).Item("ClothPurchase_Order_Date").ToString
                End If
                If Dt1.Rows(0).Item("Terms_And_Condition_1").ToString <> "" Then
                    txt_TermsCond_1.Text = Dt1.Rows(0).Item("Terms_And_Condition_1").ToString
                End If
                If Dt1.Rows(0).Item("Terms_And_Condition_2").ToString <> "" Then
                    txt_TermsCond_2.Text = Dt1.Rows(0).Item("Terms_And_Condition_2").ToString
                End If
                If Dt1.Rows(0).Item("Terms_And_Condition_3").ToString <> "" Then
                    txt_TermsCond_3.Text = Dt1.Rows(0).Item("Terms_And_Condition_3").ToString
                End If
                If Dt1.Rows(0).Item("Terms_And_Condition_4").ToString <> "" Then
                    txt_TermsCond_4.Text = Dt1.Rows(0).Item("Terms_And_Condition_4").ToString
                End If
                If Dt1.Rows(0).Item("Terms_And_Condition_5").ToString <> "" Then
                    txt_TermsCond_5.Text = Dt1.Rows(0).Item("Terms_And_Condition_5").ToString
                End If
                If Dt1.Rows(0).Item("Terms_And_Condition_6").ToString <> "" Then
                    txt_TermsCond_6.Text = Dt1.Rows(0).Item("Terms_And_Condition_6").ToString
                End If

                If Dt1.Rows(0).Item("ClothPurchase_Order_SuffixNo").ToString <> "" Then cbo_Fabric_Po_SufixNo.Text = Dt1.Rows(0).Item("ClothPurchase_Order_SuffixNo").ToString
                If Dt1.Rows(0).Item("ClothPurchase_Order_PrefixNo").ToString <> "" Then txt_Fabric_Po_PrefixNo.Text = Dt1.Rows(0).Item("ClothPurchase_Order_PrefixNo").ToString

            End If
            Dt1.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()

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

            inpno = InputBox("Enter Order Ref No.", "FOR FINDING...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(EntFnYrCode)

            Da = New SqlClient.SqlDataAdapter("select ClothPurchase_Order_RefNo from ClothPurchase_Order_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothPurchase_Order_Code = '" & Trim(InvCode) & "'", con)
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
                MessageBox.Show("Order Ref No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.ClothSales_Order_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.ClothSales_Order_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Cloth_Purchase_order_Entry, New_Entry, Me) = False Then Exit Sub


        Try

            inpno = InputBox("Enter New Order Ref No.", "FOR NEW REF NO. INSERTION...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(EntFnYrCode)

            Da = New SqlClient.SqlDataAdapter("select ClothPurchase_Order_RefNo from ClothPurchase_Order_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and ClothPurchase_Order_Code = '" & Trim(InvCode) & "'", con)
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
                    MessageBox.Show("Invalid Order Ref No.", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
        Dim clthtyp_ID As Integer = 0
        Dim Trans_ID As Integer
        Dim Led_ID As Integer = 0
        Dim Ag_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Partcls As String = ""
        Dim vTotBls As Integer, vTotPcs As Single, vTotOrdMtrs As Single, vTotCnlMtrs As Single
        Dim Cr_ID As Integer = 0
        Dim Dr_ID As Integer = 0
        Dim Usr_ID As Integer = 0
        Dim Nr As Integer = 0
        Dim vGrDt As String = ""
        Dim EnqCd As String = ""
        Dim EnqNo As String = ""
        Dim EnqSlno As Integer = 0
        Dim vOrdByNo As String = ""
        Dim Loom_Type As Integer = 0
        Dim despatch_to As Integer = 0
        Dim vDeliveryDt As String = ""
        Dim vDCDATE As String = ""
        Dim amount As Integer = 0
        Dim vFbPurcNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If


        '  If Common_Procedures.UserRight_Check(Common_Procedures.UR.ClothSales_Order_Entry, New_Entry) = False Then Exit Sub

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Cloth_Purchase_order_Entry, New_Entry, Me, con, "ClothPurchase_Order_Head", "ClothPurchase_Order_Code", NewCode, "ClothPurchase_Order_Date", "(ClothPurchase_Order_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothPurchase_Order_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, ClothPurchase_Order_No desc", dtp_Date.Value.Date) = False Then Exit Sub


        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
            Exit Sub
        End If

        If EntFnYrCode = Common_Procedures.FnYearCode Then
            If Not (Convert.ToDateTime(msk_date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_date.Text) <= Common_Procedures.Company_ToDate) Then
                MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
                Exit Sub
            End If
        End If

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled Then cbo_PartyName.Focus()
            Exit Sub
        End If

        Ag_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Agent.Text)

        despatch_to = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DespTo.Text)

        vFbPurcNo = Trim(txt_Fabric_Po_PrefixNo.Text) & Trim(lbl_RefNo.Text) & Trim(cbo_Fabric_Po_SufixNo.Text)

        Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)
        lbl_UserName.Text = Common_Procedures.User.IdNo

        If Trim(UCase(cbo_Type.Text)) = "" Or (Trim(UCase(cbo_Type.Text)) <> "OFFER") Then
            cbo_Type.Text = "DIRECT"
        End If
        For i = 0 To Transport.RowCount - 1

            If Val(Transport.Rows(i).Cells(6).Value) <> 0 Then

                clth_ID = Common_Procedures.Cloth_NameToIdNo(con, Transport.Rows(i).Cells(dgvCol_DelvDetails.Cloth_name).Value)
                If clth_ID = 0 Then
                    MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If Transport.Enabled And Transport.Visible Then
                        Transport.Focus()
                        Transport.CurrentCell = Transport.Rows(i).Cells(dgvCol_DelvDetails.Cloth_name)
                    End If
                    Exit Sub
                End If


                clthtyp_ID = Common_Procedures.ClothType_NameToIdNo(con, Transport.Rows(i).Cells(dgvCol_DelvDetails.Type).Value)
                If clthtyp_ID = 0 Then
                    MessageBox.Show("Invalid Cloth Type Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If Transport.Enabled And Transport.Visible Then
                        Transport.Focus()
                        Transport.CurrentCell = Transport.Rows(i).Cells(dgvCol_DelvDetails.Type)
                    End If
                    Exit Sub
                End If

                If Val(Transport.Rows(i).Cells(dgvCol_DelvDetails.Fold_perc).Value) = 0 Then
                    MessageBox.Show("Invalid Folding", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If Transport.Enabled And Transport.Visible Then
                        Transport.Focus()
                        Transport.CurrentCell = Transport.Rows(i).Cells(dgvCol_DelvDetails.Fold_perc)
                    End If
                    Exit Sub
                End If

                If Val(Transport.Rows(i).Cells(dgvCol_DelvDetails.Order_Mtrs).Value) = 0 Then
                    MessageBox.Show("Invalid Order metres", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If Transport.Enabled And Transport.Visible Then
                        Transport.Focus()
                        Transport.CurrentCell = Transport.Rows(i).Cells(dgvCol_DelvDetails.Order_Mtrs)
                    End If
                    Exit Sub
                End If

            End If

        Next

        vGrDt = ""
        If Trim(msk_GrDate.Text) <> "" Then
            If IsDate(msk_GrDate.Text) = True Then
                vGrDt = Trim(msk_GrDate.Text)
            End If
        End If

        vDeliveryDt = ""
        If Trim(msk_delivery_date.Text) <> "" Then
            If IsDate(msk_delivery_date.Text) = True Then
                vDeliveryDt = Trim(msk_delivery_date.Text)
            End If
        End If

        NoCalc_Status = False

        'If Common_Procedures.settings.CustomerCode <> "1186" Then
        Total_Calculation()
        'End If

        vTotBls = 0 : vTotPcs = 0 : vTotOrdMtrs = 0 : vTotCnlMtrs = 0

        If dgv_Details_Total.RowCount > 0 Then
            vTotBls = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
            vTotPcs = Val(dgv_Details_Total.Rows(0).Cells(5).Value())
            vTotOrdMtrs = Val(dgv_Details_Total.Rows(0).Cells(6).Value())
            vTotCnlMtrs = Val(dgv_Details_Total.Rows(0).Cells(8).Value())
            amount = Val(dgv_Details_Total.Rows(0).Cells(9).Value())
        End If

        If vTotOrdMtrs = 0 Then
            MessageBox.Show("Invalid Order Meters", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If Transport.Enabled And Transport.Visible Then
                Transport.Focus()
                Transport.CurrentCell = Transport.Rows(0).Cells(dgvCol_DelvDetails.Order_Mtrs)
            End If
            Exit Sub
        End If


        If Trim(txt_Po_Completion_Date.Text) <> "" Then
            If IsDate(txt_Po_Completion_Date.Text) = False Then
                MessageBox.Show("Invalid Po Completion Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If txt_Po_Completion_Date.Enabled And txt_Po_Completion_Date.Visible Then txt_Po_Completion_Date.Focus()
                Exit Sub
            End If
        End If


        tr = con.BeginTransaction

        Try

        If Insert_Entry = True Or New_Entry = False Then
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        Else

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "ClothPurchase_Order_Head", "ClothPurchase_Order_Code", "For_OrderBy", "", Val(lbl_Company.Tag), EntFnYrCode, tr)

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        End If

        cmd.Connection = con
        cmd.Transaction = tr

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@OrderDate", Convert.ToDateTime(msk_date.Text))

            'vDCDATE = ""
            'If Trim(msk_delivery_date.Text) <> "" Then
            '    If Trim(msk_delivery_date.Text) <> "-  -" Then
            '        If IsDate(msk_delivery_date.Text) = True Then
            '            cmd.Parameters.AddWithValue("@DELIVERYDate", Convert.ToDateTime(msk_delivery_date.Text))
            '            vDCDATE = Trim(msk_delivery_date.Text)
            '        End If
            '    End If
            'End If

            ' ----------------
            vDCDATE = ""
            If msk_delivery_date.Visible Then
            If Trim(msk_delivery_date.Text) <> "" Then
                If IsDate(msk_delivery_date.Text) = True Then
                        cmd.Parameters.AddWithValue("@DELIVERYDate", Convert.ToDateTime(msk_delivery_date.Text))
                        vDCDATE = Trim(msk_delivery_date.Text)
                    End If
                End If
            Else
                If Trim(msk_date.Text) <> "" Then
                    If IsDate(msk_date.Text) = True Then
                        cmd.Parameters.AddWithValue("@DELIVERYDate", Convert.ToDateTime(msk_date.Text))
                        vDCDATE = Trim(msk_date.Text)

                    End If
                End If
            End If

        If New_Entry = True Then
                cmd.CommandText = "Insert into ClothPurchase_Order_Head ( ClothPurchase_Order_Code       ,                               Company_IdNo       ,     ClothPurchase_Order_RefNo  ,            ClothPurchase_Order_SuffixNo    ,         ClothPurchase_Order_PrefixNo        ,             ClothPurchase_Order_No              ,                     for_OrderBy                                        , ClothPurchase_Order_Date  ,              Ledger_IdNo,          Party_OrderNo             ,            Through_Name          ,     Agent_IdNo          ,  Agent_Comm_Perc                , Agent_Comm_Type                 ,Despatch_to,   Despatch_IdNo                   ,   Transport_IdNo           ,  Delivery_Address1             , Delivery_Address2               ,                      Note      ,                 Total_Bales ,               Total_Pcs  ,          Total_Order_Meters,             Total_Cancel_Meters  ,  User_idNo     ,   Gr_Time                   ,         Gr_Date                            ,    Selection_Type  ,Total_amount            ,             Discount_Percentage     ,              Discount_Amount          ,                AddLess_BeforeTax_Amount      ,                 Assessable_Value           ,                  Freight_Amount     ,               AddLessAfterTax_Text            ,                   AddLess_Amount            ,               RoundOff_Amount      ,                  Net_Amount             ,          Total_CGST_Amount        ,              Total_SGST_Amount    ,          Total_IGST_Amount ,Attend,Delivery_Schedule,Payment_Terms                                                                            ,  delivery_Due_Days                          ,    delivery_Due_Date            ,Transportations                                                         ,    Tax_type       ,               Delivery_Terms_Details            ,               Payment_Terms_Details,                       Po_Completion_Date,                       Packing         ,            Roll_Length    ,                         Quality_Standard     ,                  Reed_Count      ,                       Slevedge_Details           ,            Quantity_Tolerance     ,           Terms_And_Condition_1,             Terms_And_Condition_2,                   Terms_And_Condition_3   ,           Terms_And_Condition_4  ,           Terms_And_Condition_5   ,           Terms_And_Condition_6      )   " &
                                        "     Values                    (   '" & Trim(NewCode) & "'      ,                 " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "' ,  '" & Trim(cbo_Fabric_Po_SufixNo.Text) & "',  '" & Trim(txt_Fabric_Po_PrefixNo.Text) & "' ,           '" & Trim(vFbPurcNo) & "'             , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",       @OrderDate          , " & Str(Val(Led_ID)) & ",   '" & Trim(txt_OrderNo.Text) & "'  , '" & Trim(cbo_Through.Text) & "', " & Str(Val(Ag_ID)) & ", " & Str(Val(txt_com_per.Text)) & ", '" & Trim(cbo_Com_Type.Text) & "','', " & Str(Val(despatch_to)) & ", " & Str(Val(Trans_ID)) & ", '" & Trim(txt_DelvAdd1.Text) & "', '" & Trim(txt_DelvAdd2.Text) & "', '" & Trim(txt_Note.Text) & "' , " & Str(Val(vTotBls)) & ", " & Str(Val(vTotPcs)) & ", " & Str(Val(vTotOrdMtrs)) & ", " & Str(Val(vTotCnlMtrs)) & "  ,  " & Val(lbl_UserName.Text) & " , " & Str(Val(txt_GrTime.Text)) & ", '" & Trim(vGrDt) & "', '" & Trim(cbo_Type.Text) & "'," & Str(Val(amount)) & "  ," & Str(Val(txt_DiscPerc.Text)) & " ,  " & Str(Val(lbl_DiscAmount.Text)) & " , " & Str(Val(txt_AddLess_BeforeTax.Text)) & " , " & Str(Val(txt_AssessableValue.Text)) & "  ,  " & Str(Val(txt_Freight.Text)) & " , '" & Trim(txt_AddLessAfterTax_Text.Text) & "' , " & Str(Val(txt_AddLess_AfterTax.Text)) & " , " & Str(Val(lbl_RoundOff.Text)) & ", " & Str(Val(CSng(lbl_NetAmount.Text))) & ", " & Val(lbl_CGST_Amount.Text) & " , " & Val(lbl_SGST_Amount.Text) & " , " & Val(lbl_IGST_Amount.Text) & " ,'" & Trim(txt_Attend.Text) & "','" & Trim(txt_delivery_Schedule.Text) & "','" & Trim(txt_PaymentTerms.Text) & "','" & Trim(txt_delivery_due_days.Text) & "', " & IIf(IsDate(vDCDATE) = True, "@DELIVERYDate", "Null") & ", '" & Trim(cbo_freight_Ac.Text) & "', '" & Trim(cbo_taxType.Text) & "' , '" & Trim(txt_Payment_Terms_Details.Text) & "', '" & Trim(txt_Delivery_Terms.Text) & "', '" & Trim(txt_Po_Completion_Date.Text) & "', '" & Trim(txt_Packing.Text) & "',  '" & Trim(txt_Roll_length.Text) & "','" & Trim(txt_Quality_Standard.Text) & "','" & Trim(txt_Reed_Count.Text) & "', '" & Trim(txt_Slevedge_Details.Text) & "','" & Trim(txt_Qty_Tolenrance.Text) & "', '" & Trim(txt_TermsCond_1.Text) & "', '" & Trim(txt_TermsCond_2.Text) & "', '" & Trim(txt_TermsCond_3.Text) & "', '" & Trim(txt_TermsCond_4.Text) & "','" & Trim(txt_TermsCond_5.Text) & "' , '" & Trim(txt_TermsCond_6.Text) & "' )  "
                cmd.ExecuteNonQuery()

        Else
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "ClothPurchase_Order_Head", "ClothPurchase_Order_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "ClothPurchase_Order_Code, Company_IdNo, for_OrderBy", tr)

            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "ClothPurchase_Order_Details", "ClothPurchase_Order_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Cloth_IdNo,ClothType_IdNo,Fold_Perc,Bales,Order_Pcs,Order_Meters,Rate,Order_Cancel_Meters,ClothPurchase_Offer_No,ClothPurchase_Offer_Code,ClothPurchase_Offer_Slno ,Selection_Type", "Sl_No", "ClothPurchase_Order_Code, For_OrderBy, Company_IdNo, ClothPurchase_Order_No, ClothPurchase_Order_Date, Ledger_Idno", tr)



                cmd.CommandText = "Update ClothPurchase_Order_Head set ClothPurchase_Order_Date = @OrderDate, ClothPurchase_Order_SuffixNo = '" & Trim(cbo_Fabric_Po_SufixNo.Text) & "' , ClothPurchase_Order_PrefixNo = '" & Trim(txt_Fabric_Po_PrefixNo.Text) & "' , ClothPurchase_Order_No = '" & Trim(vFbPurcNo) & "' ,  Ledger_IdNo =  " & Str(Val(Led_ID)) & " , Party_OrderNo =  '" & Trim(txt_OrderNo.Text) & "',            Through_Name = '" & Trim(cbo_Through.Text) & "'              ,     Agent_IdNo = " & Str(Val(Ag_ID)) & "    ,  Agent_Comm_Perc = " & Str(Val(txt_com_per.Text)) & "       , Agent_Comm_Type = '" & Trim(cbo_Com_Type.Text) & "' ,Despatch_to='',   Despatch_IdNo = " & Str(Val(despatch_to)) & ",   Transport_IdNo = " & Str(Val(Trans_ID)) & "       ,  Delivery_Address1 = '" & Trim(txt_DelvAdd1.Text) & "', Delivery_Address2 = '" & Trim(txt_DelvAdd2.Text) & "'  ,  Note = '" & Trim(txt_Note.Text) & "' , Total_Bales = " & Str(Val(vTotBls)) & "  ,    Gr_Time = " & Str(Val(txt_GrTime.Text)) & ", Gr_Date = '" & Trim(vGrDt) & "', Total_Pcs = " & Str(Val(vTotPcs)) & "  ,  Total_Order_Meters = " & Str(Val(vTotOrdMtrs)) & ", Total_Cancel_Meters = " & Str(Val(vTotCnlMtrs)) & " , User_IdNo =  " & Val(lbl_UserName.Text) & ", Selection_Type = '" & Trim(cbo_Type.Text) & "' ,Total_amount=" & Str(Val(amount)) & ", Discount_Percentage=" & Str(Val(txt_DiscPerc.Text)) & " ,  Discount_Amount=" & Str(Val(lbl_DiscAmount.Text)) & " , AddLess_BeforeTax_Amount=" & Str(Val(txt_AddLess_BeforeTax.Text)) & " , Assessable_Value=" & Str(Val(txt_AssessableValue.Text)) & "  , Freight_Amount= " & Str(Val(txt_Freight.Text)) & " , AddLessAfterTax_Text='" & Trim(txt_AddLessAfterTax_Text.Text) & "' ,AddLess_Amount= " & Str(Val(txt_AddLess_AfterTax.Text)) & " , RoundOff_Amount=" & Str(Val(lbl_RoundOff.Text)) & ",Net_Amount= " & Str(Val(CSng(lbl_NetAmount.Text))) & ", Total_CGST_Amount=" & Val(lbl_CGST_Amount.Text) & " ,Total_SGST_Amount= " & Val(lbl_SGST_Amount.Text) & " , Total_IGST_Amount=" & Val(lbl_IGST_Amount.Text) & " ,Attend='" & Trim(txt_Attend.Text) & "',Delivery_Schedule='" & Trim(txt_delivery_Schedule.Text) & "',Payment_Terms='" & Trim(txt_PaymentTerms.Text) & "', Delivery_due_days = '" & Trim(txt_delivery_due_days.Text) & "', Delivery_due_date = " & IIf(IsDate(vDCDATE) = True, "@DELIVERYDate", "Null") & ", Transportations='" & Trim(cbo_freight_Ac.Text) & "',Tax_type='" & Trim(cbo_taxType.Text) & "'  " &
                                ", Payment_Terms_Details = '" & Trim(txt_Payment_Terms_Details.Text) & "' , Delivery_Terms_Details ='" & Trim(txt_Delivery_Terms.Text) & "', Po_Completion_Date ='" & Trim(txt_Po_Completion_Date.Text) & "', Packing = '" & Trim(txt_Packing.Text) & "', Roll_Length = '" & Trim(txt_Roll_length.Text) & "', Quality_Standard = '" & Trim(txt_Quality_Standard.Text) & "', Reed_Count = '" & Trim(txt_Reed_Count.Text) & "', Slevedge_Details = '" & Trim(txt_Slevedge_Details.Text) & "', Quantity_Tolerance = '" & Trim(txt_Qty_Tolenrance.Text) & "', Terms_And_Condition_1 ='" & Trim(txt_TermsCond_1.Text) & "', Terms_And_Condition_2 ='" & Trim(txt_TermsCond_2.Text) & "', Terms_And_Condition_3 ='" & Trim(txt_TermsCond_3.Text) & "', Terms_And_Condition_4 ='" & Trim(txt_TermsCond_4.Text) & "', Terms_And_Condition_5 = '" & Trim(txt_TermsCond_5.Text) & "', Terms_And_Condition_6 ='" & Trim(txt_TermsCond_6.Text) & "'  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " And ClothPurchase_Order_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()
            cmd.CommandText = "Update ClothPurchase_Offer_Details set Order_Meters = a.Order_Meters - b.Order_Meters from ClothPurchase_Offer_Details a, ClothPurchase_Order_Details b Where b.ClothPurchase_Order_Code = '" & Trim(NewCode) & "' and b.Selection_Type = 'OFFER' and a.ClothPurchase_Offer_Code = b.ClothPurchase_Offer_Code and a.ClothPurchase_Offer_Slno = b.ClothPurchase_Offer_Slno"
            cmd.ExecuteNonQuery()
        End If
        Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "ClothPurchase_Order_Head", "ClothPurchase_Order_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "ClothPurchase_Order_Code, Company_IdNo, for_OrderBy", tr)


            cmd.CommandText = "Delete from ClothPurchase_Order_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothPurchase_Order_Code = '" & Trim(NewCode) & "' AND Purchase_Meters=0 "
        cmd.ExecuteNonQuery()

        With Transport

            Sno = 0
            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(dgvCol_DelvDetails.Order_Mtrs).Value) <> 0 Then

                    Sno = Sno + 1

                    clth_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(dgvCol_DelvDetails.Cloth_name).Value, tr)
                    Loom_Type = Common_Procedures.LoomType_NameToIdNo(con, Transport.Rows(i).Cells(dgvCol_DelvDetails.Loom_Type).Value, tr)
                    clthtyp_ID = Common_Procedures.ClothType_NameToIdNo(con, .Rows(i).Cells(dgvCol_DelvDetails.Type).Value, tr)
                    EnqCd = ""
                    EnqSlno = 0
                    EnqNo = ""
                    If Trim(UCase(cbo_Type.Text)) = "OFFER" Then
                        EnqNo = Trim(.Rows(i).Cells(dgvCol_DelvDetails.Offer_No).Value)
                        EnqCd = Trim(.Rows(i).Cells(dgvCol_DelvDetails.Cloth_Purchase_Offer_Code).Value)

                        EnqSlno = Val(.Rows(i).Cells(dgvCol_DelvDetails.Cloth_Purchase_Offer_Slno).Value)
                    End If
                    Nr = 0
                    cmd.CommandText = "Update  ClothPurchase_Order_Details set ClothPurchase_Order_Date = @OrderDate , Ledger_IdNo = " & Str(Val(Led_ID)) & ", Sl_No  = " & Str(Val(Sno)) & " , Cloth_IdNo = " & Str(Val(clth_ID)) & " , ClothType_IdNo = " & Str(Val(clthtyp_ID)) & " , Fold_Perc =  " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Fold_perc).Value)) & ", Bales = " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Fold_perc).Value)) & " , Order_Pcs = " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Order_pCs).Value)) & " ,       Order_Meters = " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Order_Mtrs).Value)) & " , Rate= " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.RAte).Value)) & "  , Order_Cancel_Meters =  " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Cancel_Mtrs).Value)) & " ,   ClothPurchase_Offer_No  ='" & Trim(EnqNo) & "'          ,  ClothPurchase_Offer_Code  = '" & Trim(EnqCd) & "'         ,    ClothPurchase_Offer_Slno = " & Val(EnqSlno) & ",Selection_Type= '" & Trim(cbo_Type.Text) & "',loomType_IdNo=" & Val(Loom_Type) & "  where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothPurchase_Order_Code = '" & Trim(NewCode) & "'  and ClothPurchase_Order_SlNo = " & Val(.Rows(i).Cells(dgvCol_DelvDetails.Cloth_Purchase_Order_Slno).Value)
                    Nr = cmd.ExecuteNonQuery()

                    If Nr = 0 Then
                        cmd.CommandText = "Insert into ClothPurchase_Order_Details ( ClothPurchase_Order_Code ,               Company_IdNo                 ,   ClothPurchase_Order_No    ,                     for_OrderBy                                            , ClothPurchase_Order_Date        ,      Ledger_IdNo       ,          Sl_No       ,        Cloth_IdNo          ,       ClothType_IdNo         ,                   Fold_Perc            ,                     Bales                   ,        Order_Pcs                     ,       Order_Meters            ,                      Rate                   ,           Order_Cancel_Meters                           ,     ClothPurchase_Offer_No        ,  ClothPurchase_Offer_Code        ,    ClothPurchase_Offer_Slno ,        Selection_Type,Amount,loomType_IdNo,Purchase_Meters) " &
                                                "     Values                        (   '" & Trim(NewCode) & "'      , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",       @OrderDate            , " & Str(Val(Led_ID)) & ", " & Str(Val(Sno)) & ", " & Str(Val(clth_ID)) & "   , " & Str(Val(clthtyp_ID)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Fold_perc).Value)) & "," & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.bales).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Order_pCs).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Order_Mtrs).Value)) & ",  " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.RAte).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Cancel_Mtrs).Value)) & "   ,  '" & Trim(EnqNo) & "'           ,  '" & Trim(EnqCd) & "'           , " & Val(EnqSlno) & "      , '" & Trim(cbo_Type.Text) & "', " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.AMount).Value)) & "," & Val(Loom_Type) & "," & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Cloth_Purchase_meters).Value)) & ") "
                        cmd.ExecuteNonQuery()
                    End If
                    If Trim(UCase(cbo_Type.Text)) = "OFFER" And Trim(.Rows(i).Cells(dgvCol_DelvDetails.Offer_No).Value) <> "" Then
                        Nr = 0
                        cmd.CommandText = "Update ClothPurchase_Offer_Details set Order_Meters = Order_Meters + " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Order_pCs).Value)) & " Where ClothPurchase_Offer_Code = '" & Trim(.Rows(i).Cells(dgvCol_DelvDetails.Cloth_Purchase_Offer_Code).Value) & "' and ClothPurchase_Offer_Slno = " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Cloth_Purchase_Offer_Slno).Value)) & " and Ledger_IdNo = " & Str(Val(Led_ID))
                        Nr = cmd.ExecuteNonQuery()
                        If Nr = 0 Then
                            Throw New ApplicationException("Mismatch of Order and Party Details")
                            Exit Sub
                        End If
                    End If
                End If

            Next
            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "ClothPurchase_Order_Details", "ClothPurchase_Order_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Cloth_IdNo,ClothType_IdNo,Fold_Perc,Bales,Order_Pcs,Order_Meters,Rate,Order_Cancel_Meters,ClothPurchase_Offer_No,ClothPurchase_Offer_Code,ClothPurchase_Offer_Slno ,Selection_Type", "Sl_No", "ClothPurchase_Order_Code, For_OrderBy, Company_IdNo, ClothPurchase_Order_No, ClothPurchase_Order_Date, Ledger_Idno", tr)

        End With
        cmd.CommandText = "Delete from cloth_Purchase_Order_GST_Tax_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and cloth_Purchase_Order_Code = '" & Trim(NewCode) & "'"
        cmd.ExecuteNonQuery()

        With dgv_Tax_Details

            Sno = 0
            For i = 0 To .Rows.Count - 1

                If Val(.Rows(i).Cells(3).Value) <> 0 Or Val(.Rows(i).Cells(5).Value) <> 0 Or Val(.Rows(i).Cells(7).Value) <> 0 Then

                Sno = Sno + 1

                    cmd.CommandText = "Insert into cloth_Purchase_Order_GST_Tax_Details   ( cloth_Purchase_Order_Code  ,               Company_IdNo       ,      cloth_Purchase_Order_No                ,                               for_OrderBy                                  , cloth_Purchase_Order_Date    ,         Ledger_IdNo     ,            Sl_No     , HSN_Code                               ,Taxable_Amount                            ,CGST_Percentage                           ,CGST_Amount                               ,SGST_Percentage                            ,SGST_Amount                              ,IGST_Percentage                          ,IGST_Amount ) " &
                        "Values                                                              (   '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(vOrdByNo)) & ",       @OrderDate    , " & Str(Val(Led_ID)) & ", " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', " & Str(Val(.Rows(i).Cells(2).Value)) & ", " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & "," & Str(Val(.Rows(i).Cells(5).Value)) & "  ," & Str(Val(.Rows(i).Cells(6).Value)) & "," & Str(Val(.Rows(i).Cells(7).Value)) & "," & Str(Val(.Rows(i).Cells(8).Value)) & ") "
                    cmd.ExecuteNonQuery()

                End If

            Next i

        End With
        tr.Commit()

        MessageBox.Show("Saved Successfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

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

    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotBls As Integer
        Dim TotPcs As Single
        Dim TotOrdMtrs As Double
        Dim TotCnlMtrs As Double
        Dim TotAmounts As Integer

        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

        Sno = 0
        TotBls = 0 : TotPcs = 0 : TotOrdMtrs = 0 : TotCnlMtrs = 0 : TotAmounts = 0

        With Transport
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(dgvCol_DelvDetails.Order_Mtrs).Value) <> 0 Then

                    TotBls = TotBls + Val(.Rows(i).Cells(dgvCol_DelvDetails.bales).Value())
                    TotPcs = TotPcs + Val(.Rows(i).Cells(dgvCol_DelvDetails.Order_pCs).Value())
                    TotOrdMtrs = TotOrdMtrs + Val(.Rows(i).Cells(dgvCol_DelvDetails.Order_Mtrs).Value())
                    TotCnlMtrs = TotCnlMtrs + Val(.Rows(i).Cells(dgvCol_DelvDetails.Cancel_Mtrs).Value())
                    TotAmounts = TotAmounts + Val(.Rows(i).Cells(dgvCol_DelvDetails.AMount).Value())
                End If

            Next i
            lbl_GrossAmount.Text = Format(Val(TotAmounts), "########0.00")

        End With


        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(4).Value = Val(TotBls)
            .Rows(0).Cells(5).Value = Val(TotPcs)
            .Rows(0).Cells(6).Value = Format(Val(TotOrdMtrs), "########0.00")
            .Rows(0).Cells(8).Value = Format(Val(TotCnlMtrs), "########0.00")
            .Rows(0).Cells(9).Value = Format(Val(TotAmounts), "########0.00")
        End With
        Get_AgentComm()
        If Common_Procedures.settings.CustomerCode <> "1186" Then
            GST_Calculation()
        End If

        NetAmount_Calculation()
    End Sub

    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " ( ( ( Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1 ) and Close_status = 0 ) ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName, txt_Attend, Nothing, "LedgEr_AlaisHead", "Ledger_DisplayName", " ( ( ( Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1 ) and Close_status = 0 ) ", "(Ledger_idno = 0)")
        If (e.KeyValue = 40 And cbo_PartyName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If Trim(UCase(cbo_Type.Text)) = "OFFER" Then
                If Transport.Rows.Count > 0 Then
                    Transport.Focus()
                    Transport.CurrentCell = Transport.Rows(0).Cells(dgvCol_DelvDetails.Order_Mtrs)

                Else
                    txt_Note.Focus()

                End If
            Else
                txt_OrderNo.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", " ( ( ( Ledger_Type = '' and (AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Show_In_All_Entry = 1 ) and Close_status = 0 ) ", "(Ledger_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            If cbo_Type.Text = "OFFER" Then
                If MessageBox.Show("Do you want to select Cloth Offer :", "FOR CLOTH OFFER SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                    btn_Selection_Click(sender, e)
                End If
            Else
                txt_OrderNo.Focus()
            End If
            If Trim(UCase(cbo_PartyName.Tag)) <> Trim(UCase(cbo_PartyName.Text)) Then
                cbo_PartyName.Tag = cbo_PartyName.Text
                GST_Calculation()
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
        cbo_Agent.Tag = cbo_Agent.Text
    End Sub
    Private Sub cbo_Agent_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Agent, cbo_Through, txt_com_per, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")

    End Sub

    Private Sub cbo_Agent_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Agent.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Agent, txt_com_per, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'AGENT')", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(cbo_Agent.Tag)) <> Trim(UCase(cbo_Agent.Text)) Then
                cbo_Agent.Tag = cbo_Agent.Text
                Get_AgentComm()
            End If
        End If
    End Sub

    Private Sub cbo_Agent_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Agent.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Agent_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Agent.Name
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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Com_Type, txt_com_per, txt_GrTime, "", "", "", "")
    End Sub

    Private Sub cbo_Com_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Com_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Com_Type, txt_GrTime, "", "", "", "")
    End Sub

    Private Sub cbo_DespTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DespTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", " ( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) ) or Show_In_All_Entry = 1) ", "(Ledger_IdNo = 0)")

    End Sub
    Private Sub cbo_DespTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DespTo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DespTo, msk_GrDate, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", " ( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) ) or Show_In_All_Entry = 1) ", "(Ledger_IdNo = 0)")

        If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If cbo_Transport.Visible Then
                cbo_Transport.Focus()
            Else
                txt_delivery_due_days.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_DespTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DespTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DespTo, txt_delivery_due_days, "Ledger_AlaisHead", "Ledger_DisplayName", " ( (Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14 ) ) or Show_In_All_Entry = 1) ", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If cbo_Transport.Visible Then
                cbo_Transport.Focus()
            Else
                txt_delivery_due_days.Focus()
            End If
        End If
    End Sub
    Private Sub cbo_Through_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Through.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub
    Private Sub cbo_Through_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Through.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Through, txt_OrderNo, cbo_Agent, "", "", "", "")
    End Sub

    Private Sub cbo_Through_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Through.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Through, cbo_Agent, "", "", "", "")
    End Sub




    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")

    End Sub
    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, cbo_DespTo, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        'If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If (e.KeyValue = 40) Then
            If txt_delivery_due_days.Visible = True Then
                txt_delivery_due_days.Focus()

            Else
                If Transport.Rows.Count > 0 Then
                    Transport.Focus()
                    Transport.CurrentCell = Transport.Rows(0).Cells(dgvCol_DelvDetails.Cloth_name)

                Else
                    txt_Note.Focus()

                End If
            End If

        End If
    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If txt_delivery_due_days.Visible = True Then
                txt_delivery_due_days.Focus()

            Else
                If Transport.Rows.Count > 0 Then
                    Transport.Focus()
                    Transport.CurrentCell = Transport.Rows(0).Cells(dgvCol_DelvDetails.Cloth_name)

                Else
                    txt_Note.Focus()

                End If
            End If
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

    Private Sub cbo_Grid_ClothName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_ClothName.GotFocus
        vCbo_ItmNm = Trim(cbo_Grid_ClothName.Text)

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

    End Sub

    Private Sub cbo_Grid_ClothName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_ClothName.KeyDown

        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_ClothName, Nothing, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

        With Transport

            If (e.KeyValue = 38 And cbo_Grid_ClothName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If .CurrentCell.RowIndex <= 0 Then
                    If Trim(UCase(cbo_Type.Text)) = "OFFER" Then
                        cbo_PartyName.Focus()
                    Else
                        If cbo_Transport.Visible = True Then
                            cbo_Transport.Focus()
                        Else
                            txt_DelvAdd2.Focus()
                        End If
                    End If
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(dgvCol_DelvDetails.Cancel_Mtrs)
                    .CurrentCell.Selected = True
                End If
            End If

            If (e.KeyValue = 40 And cbo_Grid_ClothName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(dgvCol_DelvDetails.Cloth_name).Value) = "" Then
                    txt_Note.Focus()

                Else

                    If Transport.Columns(dgvCol_DelvDetails.Loom_Type).Visible = True Then
                        .Focus()
                        .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_DelvDetails.Loom_Type)
                    Else
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                    End If

                End If

            End If

        End With
    End Sub

    Private Sub cbo_Grid_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_ClothName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_ClothName, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            e.Handled = True

            With Transport
                If Trim(.Rows(.CurrentRow.Index).Cells(dgvCol_DelvDetails.Cloth_name).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    txt_Note.Focus()
                Else
                    If .Columns(dgvCol_DelvDetails.Loom_Type).Visible = True Then

                        .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_DelvDetails.Loom_Type)
                    Else
                        .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCol_DelvDetails.Type)
                    End If


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
        vCbo_ItmNm = Trim(cbo_Grid_Clothtype.Text)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothType_Head", "ClothType_Name", "", "(ClothType_IdNo = 0)")

    End Sub

    Private Sub cbo_Grid_Clothtype_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Clothtype.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Clothtype, Nothing, Nothing, "ClothType_Head", "ClothType_Name", "", "(ClothType_IdNo = 0)")
        vcbo_KeyDwnVal = e.KeyValue

        With Transport

            If (e.KeyValue = 38 And cbo_Grid_Clothtype.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 2)
            End If

            If (e.KeyValue = 40 And cbo_Grid_Clothtype.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With

    End Sub

    Private Sub cbo_Grid_Clothtype_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Clothtype.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Clothtype, Nothing, "ClothType_Head", "ClothType_Name", "", "(ClothType_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            With Transport

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If

    End Sub


    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Transport.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim rect As Rectangle

        With Transport

            If Val(.CurrentRow.Cells(dgvCol_DelvDetails.SlNo).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If Trim(.CurrentRow.Cells(dgvCol_DelvDetails.Type).Value) = "" Then
                .CurrentRow.Cells(dgvCol_DelvDetails.Type).Value = "SOUND"
            End If

            If Val(.CurrentRow.Cells(dgvCol_DelvDetails.Fold_perc).Value) = 0 Then
                .CurrentRow.Cells(dgvCol_DelvDetails.Fold_perc).Value = "100"
            End If

            If e.ColumnIndex = dgvCol_DelvDetails.Cloth_name And Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.Cloth_Purchase_meters).Value) = 0 Then

                If (cbo_Grid_ClothName.Visible = False Or Val(cbo_Grid_ClothName.Tag) <> e.RowIndex) Then

                    cbo_Grid_ClothName.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
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



            If e.ColumnIndex = dgvCol_DelvDetails.Loom_Type And Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.Cloth_Purchase_meters).Value) = 0 Then

                If cbo_Grid_LoomType.Visible = False Or Val(cbo_Grid_LoomType.Tag) <> e.RowIndex Then

                    cbo_Grid_LoomType.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select LoomType_Name from LoomType_Head order by LoomType_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_LoomType.DataSource = Dt1
                    cbo_Grid_LoomType.DisplayMember = "LoomType_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_LoomType.Left = .Left + rect.Left
                    cbo_Grid_LoomType.Top = .Top + rect.Top

                    cbo_Grid_LoomType.Width = rect.Width
                    cbo_Grid_LoomType.Height = rect.Height
                    cbo_Grid_LoomType.Text = .CurrentCell.Value

                    cbo_Grid_LoomType.Tag = Val(e.RowIndex)
                    cbo_Grid_LoomType.Visible = True

                    cbo_Grid_LoomType.BringToFront()
                    cbo_Grid_LoomType.Focus()

                End If

            Else
                cbo_Grid_LoomType.Visible = False
            End If



            If e.ColumnIndex = dgvCol_DelvDetails.Type And Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.Cloth_Purchase_meters).Value) = 0 Then

                If cbo_Grid_Clothtype.Visible = False Or Val(cbo_Grid_Clothtype.Tag) <> e.RowIndex Then

                    cbo_Grid_Clothtype.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select ClothType_Name from ClothType_Head order by ClothType_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_Clothtype.DataSource = Dt1
                    cbo_Grid_Clothtype.DisplayMember = "ClothType_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_Clothtype.Left = .Left + rect.Left
                    cbo_Grid_Clothtype.Top = .Top + rect.Top

                    cbo_Grid_Clothtype.Width = rect.Width
                    cbo_Grid_Clothtype.Height = rect.Height
                    cbo_Grid_Clothtype.Text = .CurrentCell.Value

                    cbo_Grid_Clothtype.Tag = Val(e.RowIndex)
                    cbo_Grid_Clothtype.Visible = True

                    cbo_Grid_Clothtype.BringToFront()
                    cbo_Grid_Clothtype.Focus()

                End If

            Else
                cbo_Grid_Clothtype.Visible = False

            End If

        End With

    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Transport.CellLeave
        With Transport
            If .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 8 Or .CurrentCell.ColumnIndex = 7 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Transport.CellValueChanged
        ' On Error Resume Next

        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub
        Try

            If IsNothing(Transport.CurrentCell) Then Exit Sub

            With Transport
                If .Visible Then
                    If e.ColumnIndex = 6 Or e.ColumnIndex = 7 Or e.ColumnIndex = 8 Then

                        ' Total_Calculation()
                        Amount_Calculation(e.RowIndex, e.ColumnIndex)


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

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Transport.EditingControlShowing
        ' dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
        Try
            With Transport
                If .Rows.Count > 0 Then
                    dgtxt_Details = CType(Transport.EditingControl, DataGridViewTextBoxEditingControl)
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS EDITING SHOWING....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        Transport.EditingControl.BackColor = Color.Lime
        Transport.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub

    Private Sub dgtxt_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyDown
        With Transport
            'If e.KeyValue = Keys.Delete Then
            If .CurrentCell.ColumnIndex <> dgvCol_DelvDetails.Order_Mtrs And .CurrentCell.ColumnIndex <> dgvCol_DelvDetails.RAte And .CurrentCell.ColumnIndex <> dgvCol_DelvDetails.Cancel_Mtrs Then
                If Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.Cloth_Purchase_meters).Value) <> 0 Then
                    e.Handled = True
                    e.SuppressKeyPress = True
                End If
            End If
            'End If
        End With
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress

        With Transport
            If .Visible Then
                If .CurrentCell.ColumnIndex <> dgvCol_DelvDetails.Order_pCs And .CurrentCell.ColumnIndex <> dgvCol_DelvDetails.Order_Mtrs And .CurrentCell.ColumnIndex <> dgvCol_DelvDetails.Cancel_Mtrs Then
                    If Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.Cloth_Purchase_meters).Value) <> 0 Then
                        e.Handled = True
                        Exit Sub
                    End If
                End If
                If .CurrentCell.ColumnIndex = dgvCol_DelvDetails.Fold_perc Or .CurrentCell.ColumnIndex = dgvCol_DelvDetails.bales Or .CurrentCell.ColumnIndex = dgvCol_DelvDetails.Order_pCs Or .CurrentCell.ColumnIndex = dgvCol_DelvDetails.Order_Mtrs Or .CurrentCell.ColumnIndex = dgvCol_DelvDetails.RAte Or .CurrentCell.ColumnIndex = dgvCol_DelvDetails.Cancel_Mtrs Then
                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If
                End If
            End If
        End With

    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Transport.KeyDown
        With Transport

            If e.KeyCode = Keys.Left Then
                If .CurrentCell.ColumnIndex <= 1 Then
                    If .CurrentCell.RowIndex = 0 Then
                        txt_DelvAdd2.Focus()
                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)
                    End If
                End If
            End If

            If e.KeyCode = Keys.Right Then
                If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                    If .CurrentCell.RowIndex >= .Rows.Count - 1 Then
                        txt_Note.Focus()
                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(dgvCol_DelvDetails.Cloth_name)
                    End If
                End If
            End If

        End With

    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Transport.KeyUp
        Dim i As Integer
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With Transport

                If Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.Cloth_Purchase_Order_Slno).Value) = 0 And Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.Offer_No).Value) = 0 Then

                    n = .CurrentRow.Index

                    If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                        For i = 0 To .ColumnCount - 1
                            .Rows(n).Cells(i).Value = ""
                        Next

                    Else
                        .Rows.RemoveAt(n)

                    End If

                    For i = 0 To .Rows.Count - 1
                        .Rows(i).Cells(0).Value = i + 1
                    Next

                End If

            End With

        End If
    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Transport.LostFocus
        On Error Resume Next
        If Not IsNothing(Transport.CurrentCell) Then Transport.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles Transport.RowsAdded
        Dim n As Integer
        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

        If IsNothing(Transport.CurrentCell) Then Exit Sub
        With Transport
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Private Sub txt_DelvAdd2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_DelvAdd2.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If (e.KeyValue = 40) Then
            If Transport.Rows.Count > 0 Then
                Transport.Focus()
                Transport.CurrentCell = Transport.Rows(0).Cells(dgvCol_DelvDetails.Cloth_name)

            Else
                txt_Note.Focus()

            End If
        End If
    End Sub

    Private Sub txt_DelvAdd2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DelvAdd2.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If Transport.Rows.Count > 0 Then
                Transport.Focus()
                Transport.CurrentCell = Transport.Rows(0).Cells(dgvCol_DelvDetails.Cloth_name)

            Else
                txt_Note.Focus()

            End If
        End If
    End Sub

    Private Sub txt_Note_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Note.KeyDown
        If e.KeyValue = 38 Then
            If Transport.Rows.Count > 0 Then
                Transport.Focus()
                Transport.CurrentCell = Transport.Rows(0).Cells(dgvCol_DelvDetails.Cloth_name)
                Transport.CurrentCell.Selected = True

            Else
                txt_DelvAdd2.Focus()

            End If
        End If
        If e.KeyValue = 40 Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Grid_ClothName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_ClothName.TextChanged
        Try
            If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub
            If cbo_Grid_ClothName.Visible Then
                If IsNothing(Transport.CurrentCell) Then Exit Sub
                With Transport
                    If Val(cbo_Grid_ClothName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = dgvCol_DelvDetails.Cloth_name Then
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
            If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub
            If cbo_Grid_Clothtype.Visible Then
                With Transport
                    If Val(cbo_Grid_Clothtype.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = dgvCol_DelvDetails.Type Then
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

    Private Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
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
                Condt = "a.ClothPurchase_Order_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.ClothPurchase_Order_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.ClothPurchase_Order_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
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

            da = New SqlClient.SqlDataAdapter("select a.*, c.Cloth_Name, d.ClothType_name, e.Ledger_Name from ClothPurchase_Order_Head a left outer join ClothPurchase_Order_Details b on a.ClothPurchase_Order_Code = b.ClothPurchase_Order_Code left outer join Cloth_head c on b.Cloth_idno = c.Cloth_idno left outer join ClothType_head d on b.ClothType_idno = d.ClothType_idno left outer join Ledger_head e on a.Ledger_idno = e.Ledger_idno where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.ClothPurchase_Order_Code like '%/" & Trim(EntFnYrCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by ClothPurchase_Order_Date, for_orderby, ClothPurchase_Order_RefNo", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("ClothPurchase_Order_RefNo").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("ClothPurchase_Order_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Party_OrderNo").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(6).Value = dt2.Rows(i).Item("ClothType_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Total_Order_Meters").ToString), "########0.00")

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
    Private Sub dgv_Filter_Details_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Transport.CellEndEdit
        dgv_Details_CellLeave(sender, e)
    End Sub

    Private Sub cbo_Filter_ClothName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_ClothName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_Filter_ClothName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ClothName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ClothName, cbo_Filter_PartyName, btn_Filter_Show, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")
    End Sub

    Private Sub cbo_Filter_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ClothName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ClothName, btn_Filter_Show, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")
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

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim ps As Printing.PaperSize
        Dim PpSzSTS As Boolean = False
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Cloth_Purchase_Entry, New_Entry) = False Then Exit Sub


        Try

            da1 = New SqlClient.SqlDataAdapter("select * from ClothPurchase_Order_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and ClothPurchase_Order_Code = '" & Trim(NewCode) & "'", con)
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
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
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

                            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
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

    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Try
            'Ledger_GSTinNo
            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Ledger_Name as TransportName,DP.Ledger_MAINName as Delivery_partyname ,DP.Ledger_address1 as Delivery_add1,DP.Ledger_address2 as Delivery_add2,DP.Ledger_address3 as Delivery_add3,DP.Ledger_GSTinNo as Delivery_GSTinNo,DP.Ledger_address4 as Delivery_add4 from ClothPurchase_Order_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo INNER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_IdNo Left outer JOIN Ledger_Head d ON a.Transport_IdNo = d.Ledger_IdNo Left outer JOIN Ledger_Head dp ON a.Despatch_IdNo = dp.Ledger_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothPurchase_Order_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_name,b.ClothMain_Name, b.Weave,b.Weight_Meter_Fabric,b.Sort_No, d.ClothType_name, L.LoomType_name,I.Item_GST_Percentage,i.Item_HSN_Code from ClothPurchase_Order_Details a INNER JOIN Cloth_Head b ON a.Cloth_idno = b.Cloth_idno LEFT OUTER JOIN ClothType_Head d ON a.ClothType_idno = d.ClothType_idno LEFT OUTER JOIN LoomType_Head L ON a.loomType_idno = L.LoomType_idno  lEFT oUTER JOIN ItemGroup_Head i ON B.ItemGroup_IdNo = i.ItemGroup_IdNo where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothPurchase_Order_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
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
        If Common_Procedures.settings.CustomerCode = "1186" Then
            Printing_Format_1186(e)
        ElseIf Common_Procedures.settings.CustomerCode = "1464" Then ' -- MOF
            Printing_Format3_1464(e)
        Else
            Printing_Format_1333(e)
        End If



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
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
            If ps.Width = 800 And ps.Height = 600 Then
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
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
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
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
                        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                        PrintDocument1.DefaultPageSettings.PaperSize = ps
                        e.PageSettings.PaperSize = ps
                        Exit For
                    End If
                Next
            End If

        End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30
            .Right = 30
            .Top = 30
            .Bottom = 30
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

        NoofItems_PerPage = 4

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(35) : ClAr(2) = 275 : ClAr(3) = 120 : ClAr(4) = 80 : ClAr(5) = 120
        ClAr(6) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5))

        TxtHgt = 18

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

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
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Order_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

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
        Dim C1 As Single = 0
        Dim W1 As Single = 0, w2 As Single = 0
        Dim S1 As Single = 0, s2 As Single = 0

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_name, d.ClothType_name from ClothPurchase_Order_Details a INNER JOIN Cloth_Head b ON a.Cloth_idno = b.Cloth_idno LEFT OUTER JOIN ClothType_Head d ON a.ClothType_idno = d.ClothType_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothPurchase_Order_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
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
        Common_Procedures.Print_To_PrintDocument(e, "CLOTH PURCHASE ORDER INDENT", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3)
        W1 = e.Graphics.MeasureString("ORDER REF.NO : ", pFont).Width
        w2 = e.Graphics.MeasureString("DESP.TO : ", pFont).Width
        S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width


        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "ORDER REF.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("ClothPurchase_Order_RefNo").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("ClothPurchase_Order_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt + 10

        Common_Procedures.Print_To_PrintDocument(e, "ORDER NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_OrderNo").ToString.ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        CurY = CurY + 10

        Common_Procedures.Print_To_PrintDocument(e, "THROUGH     :  " & prn_HdDt.Rows(0).Item("Through_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DESP.TO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Despatch_To").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Delivery_Address1").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT  :  " & prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Delivery_Address2").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "CLOTH NAME", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TYPE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "FOLD%", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ORDER METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)


        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY


    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim W1 As Single = 0

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        CurY = CurY + TxtHgt - 10
        If is_LastPage = True Then
            Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 30, CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Order_Meters").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))


        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub dgv_Filter_Details_CellDoubleClick1(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown1(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter_Details.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        dgv_Details_KeyUp(sender, e)
    End Sub

    Private Sub msk_date_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles msk_date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_date.Text = Date.Today
            msk_date.SelectionStart = 0
        End If
    End Sub


    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyUp
        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    msk_date.Text = Date.Today
        'End If
        If IsDate(msk_date.Text) = True Then
            If e.KeyCode = 107 Then
                msk_date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_date.Text))
            ElseIf e.KeyCode = 109 Then
                msk_date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_date.Text))
            End If
        End If
        If e.KeyCode = 46 Or e.KeyCode = 8 Then

            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)
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

    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_date.Text
            vmskSelStrt = msk_date.SelectionStart
        End If

    End Sub

    Private Sub btn_SMS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SMS.Click
        Dim i As Integer = 0
        Dim smstxt As String = ""
        Dim PhNo As String = "", AgPNo As String = ""
        Dim Led_IdNo As Integer = 0, Agnt_IdNo As Integer = 0
        Dim SMS_SenderID As String = ""
        Dim SMS_Key As String = ""
        Dim SMS_RouteID As String = ""
        Dim SMS_Type As String = ""
        Dim BlNos As String = ""

        Try

            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)

            PhNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "MobileNo_Frsms", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")

            Agnt_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Agent.Text)
            AgPNo = ""
            If Val(Agnt_IdNo) <> 0 Then
                AgPNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_PhoneNo", "(Ledger_IdNo = " & Str(Val(Agnt_IdNo)) & ")")
            End If

            If Trim(AgPNo) <> "" Then
                PhNo = Trim(PhNo) & IIf(Trim(PhNo) <> "", ",", "") & Trim(AgPNo)
            End If

            smstxt = Trim(cbo_PartyName.Text) & Chr(13)
            smstxt = smstxt & " Order Ref.No : " & Trim(lbl_RefNo.Text) & Chr(13)
            smstxt = smstxt & " Order Date : " & Trim(msk_date.Text) & Chr(13)
            If Trim(txt_OrderNo.Text) <> "" Then
                smstxt = smstxt & " Party Order No : " & Trim(txt_OrderNo.Text) & Chr(13)
            End If
            If Trim(cbo_DespTo.Text) <> "" Then
                smstxt = smstxt & " Despatch To : " & Trim(cbo_DespTo.Text) & Chr(13)
            End If

            smstxt = smstxt & "  FABRIC DETAILS : " & Chr(13)

            If Transport.RowCount > 0 Then
                For i = 0 To Transport.RowCount - 1
                    If Val(Transport.Rows(i).Cells(6).Value) <> 0 Then
                        smstxt = smstxt & "  Cloth Name : " & Trim((Transport.Rows(i).Cells(dgvCol_DelvDetails.Cloth_name).Value)) & Chr(13)
                        smstxt = smstxt & "  Meters : " & Val(Transport.Rows(i).Cells(dgvCol_DelvDetails.Order_Mtrs).Value) & Chr(13)
                        smstxt = smstxt & "  Rate : " & Val(Transport.Rows(i).Cells(dgvCol_DelvDetails.RAte).Value) & Chr(13)
                    End If
                Next i
            End If

            'If dgv_Details.RowCount > 0 Then
            '    smstxt = smstxt & " No.Of Bales : " & Val((dgv_Details.Rows(0).Cells(4).Value())) & Chr(13)
            '    smstxt = smstxt & " Meters : " & Val((dgv_Details.Rows(0).Cells(7).Value())) & Chr(13)
            'End If
            '   smstxt = smstxt & " Bill Amount : " & Trim(lbl_Net_Amt.Text) & Chr(13)

            smstxt = smstxt & " " & Chr(13)
            smstxt = smstxt & " Thanks! " & Chr(13)
            smstxt = smstxt & Common_Procedures.Company_IdNoToName(con, Val(lbl_Company.Tag))

            SMS_SenderID = ""
            SMS_Key = ""
            SMS_RouteID = ""
            SMS_Type = ""

            Common_Procedures.get_SMS_Provider_Details(con, Val(lbl_Company.Tag), SMS_SenderID, SMS_Key, SMS_RouteID, SMS_Type)


            Sms_Entry.vSmsPhoneNo = Trim(PhNo)
            Sms_Entry.vSmsMessage = Trim(smstxt)

            Sms_Entry.SMSProvider_SenderID = SMS_SenderID
            Sms_Entry.SMSProvider_Key = SMS_Key
            Sms_Entry.SMSProvider_RouteID = SMS_RouteID
            Sms_Entry.SMSProvider_Type = SMS_Type

            Dim f1 As New Sms_Entry
            f1.MdiParent = MDIParent1
            f1.Show()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND SMS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Get_AgentComm()
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Agnt_ID As Integer = 0
        Dim Cloth_Comm_Percentage As Single = 0
        Dim Cloth_Comm_Mtr As Single = 0
        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub
        '  If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1155" Then Exit Sub '----KRG PALLADAM

        Agnt_ID = Val(Common_Procedures.Ledger_NameToIdNo(con, Trim(cbo_Agent.Text)))

        If Agnt_ID = 0 Then Exit Sub

        Try

            da = New SqlClient.SqlDataAdapter("select Cloth_Comm_Percentage ,Cloth_Comm_Meter from ledger_head  where  Ledger_IdNo = " & Str(Val(Agnt_ID)) & "", con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0).Item("Cloth_Comm_Percentage").ToString) = False Then
                    Cloth_Comm_Percentage = Val(dt.Rows(0).Item("Cloth_Comm_Percentage").ToString)
                End If
                If IsDBNull(dt.Rows(0).Item("Cloth_Comm_Meter").ToString) = False Then
                    Cloth_Comm_Mtr = Val(dt.Rows(0).Item("Cloth_Comm_Meter").ToString)
                End If
            End If
            dt.Clear()


            If Trim(cbo_Com_Type.Text) = "%" Then
                txt_com_per.Text = Val(Cloth_Comm_Percentage)
            Else
                txt_com_per.Text = Val(Cloth_Comm_Mtr)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try
    End Sub

    Private Sub cbo_Agent_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Agent.TextChanged
        Get_AgentComm()
    End Sub

    Private Sub GraceTime_Calculation()

        msk_GrDate.Text = ""
        If IsDate(msk_date.Text) = True And Val(txt_GrTime.Text) >= 0 Then
            msk_GrDate.Text = DateAdd("d", Val(txt_GrTime.Text), Convert.ToDateTime(msk_date.Text))
        End If
        msk_delivery_date.Text = ""
        If IsDate(msk_date.Text) = True And Val(txt_delivery_due_days.Text) >= 0 Then
            msk_delivery_date.Text = DateAdd("d", Val(txt_delivery_due_days.Text), Convert.ToDateTime(msk_date.Text))
        End If

    End Sub

    Private Sub txt_GrTime_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_GrTime.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_GrTime_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_GrTime.TextChanged
        GraceTime_Calculation()
    End Sub

    Private Sub dtp_GrDate_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_GrDate.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_GrDate.Text = Date.Today
        End If
    End Sub

    Private Sub dtp_GrDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_GrDate.TextChanged
        If IsDate(dtp_GrDate.Text) = True Then

            msk_GrDate.Text = dtp_GrDate.Text
            msk_GrDate.SelectionStart = 0
        End If
    End Sub

    Private Sub dtp_GrDate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtp_GrDate.ValueChanged
        msk_GrDate.Text = dtp_GrDate.Text
    End Sub

    Private Sub dtp_GrDate_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_GrDate.Enter
        msk_GrDate.Focus()
        msk_GrDate.SelectionStart = 0
    End Sub
    Private Sub cbo_Type_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub

    Private Sub cbo_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Type, msk_date, cbo_PartyName, "", "", "", "")

    End Sub

    Private Sub cbo_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Type, cbo_PartyName, "", "", "", "")



    End Sub
    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim CompIDCondt As String
        Dim Ent_Bls As Single = 0
        Dim Ent_BlNos As String = ""
        Dim Ent_Pcs As Single = 0
        Dim Ent_Mtrs As Single = 0
        Dim Ent_Rate As Single = 0

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName.Text)

        If LedIdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT ORDER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_PartyName.Enabled And cbo_PartyName.Visible Then cbo_PartyName.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Trim(UCase(Common_Procedures.settings.CompanyName)) = "-----~~~" Then
            CompIDCondt = ""
        End If


        With dgv_Selection

            .Rows.Clear()
            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Cloth_Name, d.Ledger_Name as agentname, e.Ledger_Name as Transportname,  g.ClothType_name, h.Bales as Ent_Bales,  h.Order_Pcs as Ent_Pcs, h.Order_Meters as Ent_Meters,h.Rate as Ent_Rate from ClothPurchase_Offer_Head a INNER JOIN ClothPurchase_Offer_Details b ON a.ClothPurchase_Offer_Code = b.ClothPurchase_Offer_Code INNER JOIN Cloth_Head c ON b.Cloth_IdNo = c.Cloth_IdNo INNER JOIN ClothType_Head g ON b.ClothType_IdNo = g.ClothType_IdNo LEFT OUTER JOIN Ledger_Head d ON a.Agent_IdNo = d.Ledger_IdNo LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo LEFT OUTER JOIN ClothPurchase_Order_Details h ON h.ClothPurchase_Order_Code = '" & Trim(NewCode) & "' and b.ClothPurchase_Offer_Code = h.ClothPurchase_Offer_Code and b.ClothPurchase_Offer_Slno = h.ClothPurchase_Offer_Slno Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.ledger_Idno = " & Str(Val(LedIdNo)) & " and ((b.OFFER_Meters - b.OFFER_Cancel_Meters - b.Order_Meters) > 0 or h.Order_Meters > 0 ) order by a.ClothPurchase_OFFER_Date, a.for_orderby, a.ClothPurchase_Offer_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()



                    Ent_Bls = 0
                    Ent_BlNos = ""
                    Ent_Pcs = 0
                    Ent_Mtrs = 0
                    Ent_Rate = 0
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Bales").ToString) = False Then
                        Ent_Bls = Val(Dt1.Rows(i).Item("Ent_Bales").ToString)
                    End If
                    'If IsDBNull(Dt1.Rows(i).Item("Ent_Bales_Nos").ToString) = False Then
                    '    Ent_BlNos = Dt1.Rows(i).Item("Ent_Bales_Nos").ToString
                    'End If
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Pcs").ToString) = False Then
                        Ent_Pcs = Val(Dt1.Rows(i).Item("Ent_Pcs").ToString)
                    End If
                    If IsDBNull(Dt1.Rows(i).Item("Ent_Meters").ToString) = False Then
                        Ent_Mtrs = Val(Dt1.Rows(i).Item("Ent_Meters").ToString)
                    End If

                    If IsDBNull(Dt1.Rows(i).Item("Ent_Rate").ToString) = False Then
                        Ent_Rate = Val(Dt1.Rows(i).Item("Ent_Rate").ToString)
                    End If

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("ClothPurchase_Offer_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("ClothPurchase_OFFER_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("ClothType_Name").ToString
                    .Rows(n).Cells(5).Value = Val(Dt1.Rows(i).Item("Fold_Perc").ToString)
                    .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Bales").ToString)
                    .Rows(n).Cells(7).Value = Val(Dt1.Rows(i).Item("OFFER_Pcs").ToString)
                    .Rows(n).Cells(8).Value = Format(Val(Dt1.Rows(i).Item("OFFER_Meters").ToString) - Val(Dt1.Rows(i).Item("Order_Meters").ToString) + Val(Ent_Mtrs), "#########0.00")
                    .Rows(n).Cells(9).Value = Format(Val(Dt1.Rows(i).Item("Rate").ToString), "#########0.00")
                    .Rows(n).Cells(10).Value = Format(Val(Dt1.Rows(i).Item("OFFER_Cancel_Meters").ToString), "#########0.00")
                    If Ent_Mtrs > 0 Then
                        .Rows(n).Cells(11).Value = "1"
                        For j = 0 To .ColumnCount - 1
                            .Rows(n).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Else
                        .Rows(n).Cells(11).Value = ""

                    End If

                    .Rows(n).Cells(12).Value = Dt1.Rows(i).Item("agentname").ToString
                    .Rows(n).Cells(13).Value = Dt1.Rows(i).Item("Transportname").ToString
                    .Rows(n).Cells(14).Value = Dt1.Rows(i).Item("Through_Name").ToString
                    .Rows(n).Cells(15).Value = Dt1.Rows(i).Item("Despatch_To").ToString
                    .Rows(n).Cells(16).Value = Dt1.Rows(i).Item("Delivery_Address1").ToString
                    .Rows(n).Cells(17).Value = Dt1.Rows(i).Item("Delivery_Address2").ToString
                    .Rows(n).Cells(18).Value = Dt1.Rows(i).Item("ClothPurchase_Offer_Code").ToString
                    .Rows(n).Cells(19).Value = Dt1.Rows(i).Item("ClothPurchase_Offer_Slno").ToString

                    .Rows(n).Cells(20).Value = Val(Ent_Bls)
                    .Rows(n).Cells(21).Value = Ent_Pcs
                    .Rows(n).Cells(22).Value = Ent_Mtrs
                    .Rows(n).Cells(23).Value = Ent_Rate
                    .Rows(n).Cells(24).Value = (Dt1.Rows(i).Item("Party_OrderNo").ToString)
                    .Rows(n).Cells(25).Value = (Dt1.Rows(i).Item("Gr_Time").ToString)
                    .Rows(n).Cells(26).Value = (Dt1.Rows(i).Item("Gr_Date").ToString)
                    .Rows(n).Cells(27).Value = (Dt1.Rows(i).Item("Agent_Comm_Perc").ToString)
                    .Rows(n).Cells(28).Value = (Dt1.Rows(i).Item("Agent_Comm_Type").ToString)
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

                .Rows(RwIndx).Cells(11).Value = (Val(.Rows(RwIndx).Cells(11).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(11).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next


                Else
                    .Rows(RwIndx).Cells(11).Value = ""

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
        Cloth_Invoice_Selection()
    End Sub

    Private Sub Cloth_Invoice_Selection()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0

        Transport.Rows.Clear()

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(11).Value) = 1 Then


                txt_OrderNo.Text = dgv_Selection.Rows(i).Cells(24).Value
                txt_GrTime.Text = dgv_Selection.Rows(i).Cells(25).Value
                msk_GrDate.Text = dgv_Selection.Rows(i).Cells(26).Value
                txt_com_per.Text = dgv_Selection.Rows(i).Cells(27).Value
                cbo_Com_Type.Text = dgv_Selection.Rows(i).Cells(28).Value
                cbo_Agent.Text = dgv_Selection.Rows(i).Cells(12).Value
                cbo_Through.Text = dgv_Selection.Rows(i).Cells(14).Value
                cbo_DespTo.Text = dgv_Selection.Rows(i).Cells(15).Value
                cbo_Transport.Text = dgv_Selection.Rows(i).Cells(13).Value

                If txt_DelvAdd1.Text = "" Then
                    If (dgv_Selection.Rows(i).Cells(16).Value) <> "" Then
                        txt_DelvAdd1.Text = dgv_Selection.Rows(i).Cells(16).Value
                    End If
                End If

                If txt_DelvAdd2.Text = "" Then
                    If (dgv_Selection.Rows(i).Cells(17).Value) <> "" Then
                        txt_DelvAdd2.Text = dgv_Selection.Rows(i).Cells(17).Value
                    End If
                End If

                n = Transport.Rows.Add()
                sno = sno + 1
                Transport.Rows(n).Cells(dgvCol_DelvDetails.SlNo).Value = Val(sno)
                Transport.Rows(n).Cells(dgvCol_DelvDetails.Cloth_name).Value = dgv_Selection.Rows(i).Cells(3).Value
                Transport.Rows(n).Cells(dgvCol_DelvDetails.Type).Value = dgv_Selection.Rows(i).Cells(4).Value
                Transport.Rows(n).Cells(dgvCol_DelvDetails.Fold_perc).Value = dgv_Selection.Rows(i).Cells(5).Value

                If Val(dgv_Selection.Rows(i).Cells(20).Value) <> 0 Then
                    Transport.Rows(n).Cells(dgvCol_DelvDetails.bales).Value = dgv_Selection.Rows(i).Cells(20).Value
                Else
                    Transport.Rows(n).Cells(dgvCol_DelvDetails.bales).Value = dgv_Selection.Rows(i).Cells(6).Value
                End If


                If Val(dgv_Selection.Rows(i).Cells(21).Value) <> 0 Then
                    Transport.Rows(n).Cells(dgvCol_DelvDetails.Order_pCs).Value = dgv_Selection.Rows(i).Cells(21).Value
                Else
                    Transport.Rows(n).Cells(dgvCol_DelvDetails.Order_pCs).Value = dgv_Selection.Rows(i).Cells(7).Value
                End If

                If Val(dgv_Selection.Rows(i).Cells(22).Value) <> 0 Then
                    Transport.Rows(n).Cells(dgvCol_DelvDetails.Order_Mtrs).Value = dgv_Selection.Rows(i).Cells(22).Value
                Else
                    Transport.Rows(n).Cells(dgvCol_DelvDetails.Order_Mtrs).Value = dgv_Selection.Rows(i).Cells(8).Value
                End If

                If Val(dgv_Selection.Rows(i).Cells(23).Value) <> 0 Then
                    Transport.Rows(n).Cells(dgvCol_DelvDetails.RAte).Value = dgv_Selection.Rows(i).Cells(23).Value
                Else
                    Transport.Rows(n).Cells(dgvCol_DelvDetails.RAte).Value = dgv_Selection.Rows(i).Cells(9).Value
                End If

                Transport.Rows(n).Cells(dgvCol_DelvDetails.Cancel_Mtrs).Value = dgv_Selection.Rows(i).Cells(10).Value
                Transport.Rows(n).Cells(dgvCol_DelvDetails.Offer_No).Value = dgv_Selection.Rows(i).Cells(1).Value
                Transport.Rows(n).Cells(dgvCol_DelvDetails.Cloth_Purchase_Offer_Code).Value = dgv_Selection.Rows(i).Cells(18).Value
                Transport.Rows(n).Cells(dgvCol_DelvDetails.Cloth_Purchase_Offer_Slno).Value = dgv_Selection.Rows(i).Cells(19).Value

            End If

        Next

        Total_Calculation()

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        If Transport.Rows.Count > 0 Then
            Transport.Focus()
            Transport.CurrentCell = Transport.Rows(0).Cells(dgvCol_DelvDetails.Order_Mtrs)

        Else
            txt_Note.Focus()

        End If

    End Sub

    Private Sub cbo_Type_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.TextChanged
        If Trim(UCase(cbo_Type.Text)) = "OFFER" Then
            Transport.AllowUserToAddRows = False
            txt_OrderNo.Enabled = False
            cbo_Through.Enabled = False
            cbo_Agent.Enabled = False
            txt_com_per.Enabled = False
            cbo_Com_Type.Enabled = False
            txt_GrTime.Enabled = False
            msk_GrDate.Enabled = False
            cbo_DespTo.Enabled = False
            cbo_Transport.Enabled = False
            txt_DelvAdd1.Enabled = False
            txt_DelvAdd2.Enabled = False
        Else
            Transport.AllowUserToAddRows = True
            txt_OrderNo.Enabled = True
            cbo_Through.Enabled = True
            cbo_Agent.Enabled = True
            txt_com_per.Enabled = True
            cbo_Com_Type.Enabled = True
            txt_GrTime.Enabled = True
            msk_GrDate.Enabled = True
            cbo_DespTo.Enabled = True
            cbo_Transport.Enabled = True
            txt_DelvAdd1.Enabled = True
            txt_DelvAdd2.Enabled = True
        End If
    End Sub


    Private Sub btn_UserModification_Click(sender As System.Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub


    Private Sub Printing_Format_1333(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        Dim amount As Integer = 0
        Dim Cmp_Name As String

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1

            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next


        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20
            .Right = 50
            .Top = 15
            .Bottom = 40
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


        NoofItems_PerPage = 12
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then

        '    Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

        '    If InStr(1, Trim(UCase(Cmp_Name)), "SRI BHAGAVAN TEXTILES") > 0 Then

        '        NoofItems_PerPage = 12
        '    Else
        '        NoofItems_PerPage = 10
        '    End If

        'End If



        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(35) : ClAr(2) = 275 : ClAr(3) = 70 : ClAr(4) = 70 : ClAr(5) = 100 : ClAr(6) = 80
        ClAr(7) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6))

        TxtHgt = 18

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format_1333_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format_1333_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

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
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Order_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

                        amount = Val(prn_DetDt.Rows(prn_DetIndx).Item("Order_Meters").ToString) * Val(prn_DetDt.Rows(prn_DetIndx).Item("Rate"))
                        Common_Procedures.Print_To_PrintDocument(e, Val(amount), PageWidth - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Format_1333_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format_1333_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_GstNo As String
        Dim strHeight As Single
        Dim C1 As Single = 0
        Dim W1 As Single = 0, w2 As Single = 0
        Dim S1 As Single = 0, s2 As Single = 0

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_name, d.ClothType_name from ClothPurchase_Order_Details a INNER JOIN Cloth_Head b ON a.Cloth_idno = b.Cloth_idno LEFT OUTER JOIN ClothType_Head d ON a.ClothType_idno = d.ClothType_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothPurchase_Order_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
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
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GstNo = "GSTIN : " & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
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
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GstNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        If Common_Procedures.settings.CustomerCode = "1186" Then
            Common_Procedures.Print_To_PrintDocument(e, "FABRIC PURCHASE ORDER", LMargin, CurY, 2, PrintWidth, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "CLOTH PURCHASE ORDER", LMargin, CurY, 2, PrintWidth, p1Font)
        End If

        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + 50
        W1 = e.Graphics.MeasureString("PARTY S.O NO: ", pFont).Width
        w2 = e.Graphics.MeasureString("DESP.TO : ", pFont).Width
        S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width


        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "PO.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("ClothPurchase_Order_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("ClothPurchase_Order_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt + 10
        'Delivery_GSTinNo
        Common_Procedures.Print_To_PrintDocument(e, " GSTIN NO : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PARTY S.O NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_OrderNo").ToString.ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        CurY = CurY + 10

        Common_Procedures.Print_To_PrintDocument(e, "THROUGH       :  " & prn_HdDt.Rows(0).Item("Through_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DESP.TO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Delivery_partyname").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Delivery_Add1").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "TRANSPORT    :  " & prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        If Val(prn_HdDt.Rows(0).Item("Agent_IdNo").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "AGENT NAME : " & Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Agent_IdNo").ToString)), LMargin + 10, CurY, 0, 0, pFont)
        End If
        '
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Delivery_Add2").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("Attend").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "Attend Mr. " & prn_HdDt.Rows(0).Item("Attend").ToString, LMargin + 10, CurY, 0, 0, pFont)
        End If
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Delivery_Add3").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Delivery_Add4").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        If Trim(prn_HdDt.Rows(0).Item("Delivery_GSTinNo").ToString) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " GSTIN NO : " & prn_HdDt.Rows(0).Item("Delivery_GSTinNo").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "CLOTH NAME", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TYPE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "FOLD%", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ORDER MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY


    End Sub

    Private Sub Printing_Format_1333_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim C1 As Single, W1 As Single
        Dim vTaxPerc As Single = 0
        Dim BmsInWrds As String
        Dim ItmNm1 As String = "", ItmNm2 As String = "", ItmNm3 As String = "", ItmNm4 As String = "", ItmNm5 As String = "", ItmNm6 As String = "", ItmNm7 As String = "", ItmNm8 As String = "", ItmNm9 As String = "", ItmNm10 As String = ""
        Dim DelvAddAr() As String
        Dim DelInc As Integer = 0

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        CurY = CurY + TxtHgt - 10
        If is_LastPage = True Then
            Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 30, CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Order_Meters").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))


        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10
        W1 = e.Graphics.MeasureString("DISCOUNT    : ", pFont).Width


        CurY = CurY + TxtHgt - 10
        If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> 0 Then
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "Discount @ " & Trim(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString) & "%", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Discount ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            End If
        End If
        'If Trim(prn_HdDt.Rows(0).Item("Delivery_Address1").ToString) <> "" Then


        '    p1Font = New Font("Calibri", 10, FontStyle.Bold Or FontStyle.Underline)
        '    Common_Procedures.Print_To_PrintDocument(e, "DELIVERY ADDRESS : ", LMargin + 10, CurY, 0, 0, p1Font)

        '    'Erase DelvAddAr
        '    'If Trim(prn_HdDt.Rows(0).Item("Delivery_Address").ToString) <> "" Then
        '    '    DelvAddAr = Split(Trim(prn_HdDt.Rows(0).Item("Delivery_Address").ToString), ",")
        '    '    DelInc = -1

        '    '    DelInc = DelInc + 1
        '    '    If UBound(DelvAddAr) >= DelInc Then
        '    '        ItmNm1 = Trim(DelvAddAr(DelInc))
        '    '    End If

        '    '    DelInc = DelInc + 1
        '    '    If UBound(DelvAddAr) >= DelInc Then
        '    '        ItmNm2 = Trim(DelvAddAr(DelInc))
        '    '    End If

        '    '    DelInc = DelInc + 1
        '    '    If UBound(DelvAddAr) >= DelInc Then
        '    '        ItmNm3 = Trim(DelvAddAr(DelInc))
        '    '    End If

        '    '    DelInc = DelInc + 1
        '    '    If UBound(DelvAddAr) >= DelInc Then
        '    '        ItmNm4 = Trim(DelvAddAr(DelInc))
        '    '    End If

        '    '    DelInc = DelInc + 1
        '    '    If UBound(DelvAddAr) >= DelInc Then
        '    '        ItmNm5 = Trim(DelvAddAr(DelInc))
        '    '    End If

        '    '    DelInc = DelInc + 1
        '    '    If UBound(DelvAddAr) >= DelInc Then
        '    '        ItmNm6 = Trim(DelvAddAr(DelInc))
        '    '    End If

        '    '    DelInc = DelInc + 1
        '    '    If UBound(DelvAddAr) >= DelInc Then
        '    '        ItmNm7 = Trim(DelvAddAr(DelInc))
        '    '    End If

        '    '    DelInc = DelInc + 1
        '    '    If UBound(DelvAddAr) >= DelInc Then
        '    '        ItmNm8 = Trim(DelvAddAr(DelInc))
        '    '    End If

        '    '    DelInc = DelInc + 1
        '    '    If UBound(DelvAddAr) >= DelInc Then
        '    '        ItmNm9 = Trim(DelvAddAr(DelInc))
        '    '    End If
        '    '    DelInc = DelInc + 1
        '    '    If UBound(DelvAddAr) >= DelInc Then
        '    '        ItmNm10 = Trim(DelvAddAr(DelInc))
        '    '    End If
        '    'End If

        '    p1Font = New Font("Calibri", 10, FontStyle.Bold)

        '    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Delivery_Address1").ToString), LMargin + 10, CurY + TxtHgt, 0, 0, p1Font)
        '    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Delivery_Address2").ToString), LMargin + 10, CurY + TxtHgt + TxtHgt, 0, 0, p1Font)
        'End If
        p1Font = New Font("Calibri", 10, FontStyle.Bold)

        'Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm3, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm4, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm5, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm6, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm7, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm8, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm9, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm10, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)


        Common_Procedures.Print_To_PrintDocument(e, " Payment Terms  :  " & prn_HdDt.Rows(0).Item("Gr_time").ToString & " Days", LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm8, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
        ' Common_Procedures.Print_To_PrintDocument(e, " Delivery Date : " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm10, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)


        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Due_Days").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)


        'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        If Val(prn_HdDt.Rows(0).Item("AddLess_BeforeTax_Amount").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "AddLess Before Tax", LMargin + C1 + 10, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("AddLess_BeforeTax_Amount").ToString), "########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            End If
        End If

        'Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("DelAdd1").ToString, LMargin + 10, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "Freight", LMargin + C1 + 10, CurY, 0, 0, pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            End If
        End If
        'Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("DelAdd2").ToString, LMargin + 10, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "AddLess After Tax", LMargin + C1 + 10, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString), "#######0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            End If
        End If


        'Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("DelAdd3").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("DelAdd4").ToString, LMargin + 10, CurY + TxtHgt, 0, 0, p1Font)



        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Total  Before Tax  ", LMargin + C1 + 10, CurY, 0, 0, pFont)
        If is_LastPage = True Then
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
        CurY = CurY + TxtHgt - 10



        vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)

        If Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            End If

        Else
            Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)

        End If
        CurY = CurY + TxtHgt

        If Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            End If

        Else
            Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
        End If

        CurY = CurY + TxtHgt
        If Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            End If

        Else
            Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
        End If
        CurY = CurY + TxtHgt

        If Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "RoundOff ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            End If

        End If


        CurY = CurY + TxtHgt + 5

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
        LnAr(8) = CurY
        CurY = CurY + TxtHgt - 10
        'Common_Procedures.Print_To_PrintDocument(e, "E & O.E", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "GRAND TOTAL", LMargin + C1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Net_Amount").ToString), PageWidth - 5, CurY, 1, 0, p1Font)

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1087" Then


            If InStr(1, Trim(UCase(Cmp_Name)), "BAGAVAN") > 0 And InStr(1, Trim(UCase(Cmp_Name)), "TEXTILE") > 0 Then



                CurY = CurY + TxtHgt + 10
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)
                LnAr(8) = CurY




                CurY = CurY + TxtHgt - 10
                Common_Procedures.Print_To_PrintDocument(e, "GST on Reverse Charge", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)

            End If
        End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY



        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + +ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
        CurY = CurY + TxtHgt - 10
        BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
        BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "", "")

        Common_Procedures.Print_To_PrintDocument(e, "Rupees   : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        If Trim(prn_HdDt.Rows(0).Item("Note").ToString) <> "" Then
            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, "Remarks :  " & Trim(prn_HdDt.Rows(0).Item("Note").ToString), LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        End If
        'CurY = CurY + 10
        'p1Font = New Font("Calibri", 12, FontStyle.Underline)
        'Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)

        'CurY = CurY + TxtHgt + 10
        'Common_Procedures.Print_To_PrintDocument(e, "1. We are responsible for the quality of cloth only;If any running fault or quality  ", LMargin + 10, CurY, 0, 0, pFont)

        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "   defect noted in cloth please inform with firat fabric roll at once.We will", LMargin + 10, CurY, 0, 0, pFont)
        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "   accept only one roll at defect otherwise we do not hold ourself responsible. ", LMargin + 10, CurY, 0, 0, pFont)

        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, "2. Our responsibility ceases when goods leave our permises.", LMargin + 10, CurY, 0, 0, pFont)

        'CurY = CurY + TxtHgt
        'If Val(Common_Procedures.User.IdNo) <> 1 Then
        '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
        'End If

        CurY = CurY + TxtHgt - 10
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt


        CurY = CurY + TxtHgt



        CurY = CurY + TxtHgt + 10
        Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)



        Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)



    End Sub
    Private Sub txt_AddLess_BeforeTax_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_AddLess_BeforeTax.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1035" Then
                txt_Freight.Focus()
            Else
                txt_AddLess_AfterTax.Focus()
            End If
        End If

    End Sub

    Private Sub txt_AddLess_BeforeTax_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AddLess_BeforeTax.KeyPress

        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1035" Then
                txt_Freight.Focus()
            Else
                txt_AddLess_AfterTax.Focus()
            End If

        End If

    End Sub

    Private Sub txt_AddLess_BeforeTax_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_AddLess_BeforeTax.LostFocus
        If Val(txt_AddLess_BeforeTax.Text) <> 0 Then
            txt_AddLess_BeforeTax.Text = Format(Val(txt_AddLess_BeforeTax.Text), "#########0.00")
        Else
            txt_AddLess_BeforeTax.Text = ""
        End If
    End Sub

    Private Sub txt_AddLess_BeforeTax_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_AddLess_BeforeTax.TextChanged
        NetAmount_Calculation()
    End Sub



    Private Sub txt_AddLess_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_AddLess_AfterTax.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        'If Asc(e.KeyChar) = 13 Then
        '    cbo_Transport.Focus()
        'End If
    End Sub

    Private Sub txt_AddLess_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_AddLess_AfterTax.LostFocus
        If Val(txt_AddLess_AfterTax.Text) <> 0 Then
            txt_AddLess_AfterTax.Text = Format(Val(txt_AddLess_AfterTax.Text), "#########0.00")
        Else
            txt_AddLess_AfterTax.Text = ""
        End If
    End Sub

    Private Sub txt_AddLess_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_AddLess_AfterTax.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Freight_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Freight.LostFocus
        If Val(txt_Freight.Text) <> 0 Then
            txt_Freight.Text = Format(Val(txt_Freight.Text), "#########0.00")
        Else
            txt_Freight.Text = ""
        End If

    End Sub

    Private Sub txt_Freight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Freight.TextChanged
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

            If Transport.Rows.Count > 0 Then


                Transport.Focus()
                Transport.CurrentCell = Transport.Rows(0).Cells(dgvCol_DelvDetails.Cloth_name)

            End If

        End If


        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")

    End Sub

    Private Sub txt_DiscPerc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_DiscPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_DiscPerc_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_DiscPerc.TextChanged
        NetAmount_Calculation()
    End Sub

    Private Sub NetAmount_Calculation()
        Dim NtAmt As Single
        Dim GST_Amt As Single = 0
        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1035" Then

            lbl_DiscAmount.Text = Format(Val(lbl_GrossAmount.Text) * Val(txt_DiscPerc.Text) / 100, "########0.00")

            txt_AssessableValue.Text = Format(Val(lbl_GrossAmount.Text) - Val(lbl_DiscAmount.Text) + Val(txt_AddLess_BeforeTax.Text) + Val(txt_Freight.Text), "########0.00")

        End If
        GST_Amt = Val(lbl_CGST_Amount.Text) + Val(lbl_SGST_Amount.Text) + Val(lbl_IGST_Amount.Text)

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1035" Or "1369" Then
        '    NtAmt = Val(lbl_GrossAmount.Text) - Val(lbl_DiscAmount.Text) + Val(txt_AddLess_BeforeTax.Text) + GST_Amt
        'Else
        NtAmt = Val(txt_AssessableValue.Text) + Val(txt_AddLess_AfterTax.Text) + GST_Amt
        ' End If



        lbl_NetAmount.Text = Format(Val(NtAmt), "##########0")
        lbl_NetAmount.Text = Common_Procedures.Currency_Format(Val(lbl_NetAmount.Text))

        lbl_RoundOff.Text = Format(Val(CSng(lbl_NetAmount.Text)) - Val(NtAmt), "#########0.00")
        If Val(lbl_RoundOff.Text) = 0 Then lbl_RoundOff.Text = ""



    End Sub
    Private Sub Amount_Calculation(ByVal CurRow As Integer, ByVal CurCol As Integer)
        Try
            With Transport
                If .Visible Then
                    If .Rows.Count > 0 Then
                        If CurCol = 6 Or CurCol = 7 Or CurCol = 8 Then


                            .Rows(CurRow).Cells(dgvCol_DelvDetails.AMount).Value = Format(Val(.Rows(CurRow).Cells(dgvCol_DelvDetails.Order_Mtrs).Value) * Val(.Rows(CurRow).Cells(dgvCol_DelvDetails.RAte).Value), "#########0.00")

                            'If Common_Procedures.settings.CustomerCode <> "1186" Then
                            Total_Calculation()
                            'End If


                        End If

                    End If

                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "AMOUNT CALCULATION....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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

    Private Sub GST_Calculation()
        Dim Ledger_State_Code As String = ""
        Dim Company_State_Code As String = ""
        Dim CGST_Per As Single = 0, SGST_Per As Single = 0, IGST_Per As Single = 0, GST_Per As Single = 0
        Dim HSN_Code As String = ""
        Dim Taxable_Amount As Double = 0
        Dim Led_IdNo As Integer = 0
        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub
        Try

            If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

            With Transport

                If Transport.Rows.Count > 0 Then

                    For RowIndx = 0 To Transport.Rows.Count - 1


                        .Rows(RowIndx).Cells(dgvCol_DelvDetails.Disc_perc).Value = ""
                        .Rows(RowIndx).Cells(dgvCol_DelvDetails.Disc_AMt).Value = ""
                        .Rows(RowIndx).Cells(dgvCol_DelvDetails.Taxable_value).Value = ""  ' Taxable value
                        .Rows(RowIndx).Cells(dgvCol_DelvDetails.Gst_Perc).Value = ""  ' GST %
                        .Rows(RowIndx).Cells(dgvCol_DelvDetails.Hsn_Code).Value = ""  ' HSN code

                        '    If Trim(.Rows(RowIndx).Cells(1).Value) <> "" Or Val(.Rows(RowIndx).Cells(3).Value) = 0 Or Val(.Rows(RowIndx).Cells(5).Value) = 0 Then

                        HSN_Code = ""
                        GST_Per = 0
                        Get_GST_Percentage_From_ItemGroup(Trim(.Rows(RowIndx).Cells(dgvCol_DelvDetails.Cloth_name).Value), HSN_Code, GST_Per)


                        '--Cash discount
                        .Rows(RowIndx).Cells(dgvCol_DelvDetails.Disc_perc).Value = Format(Val(txt_DiscPerc.Text), "########0.00")
                        .Rows(RowIndx).Cells(dgvCol_DelvDetails.Disc_AMt).Value = Format(Val(.Rows(RowIndx).Cells(dgvCol_DelvDetails.AMount).Value) * (Val(.Rows(RowIndx).Cells(dgvCol_DelvDetails.Disc_perc).Value) / 100), "########0.00")

                        '-- Taxable value = amount -  cash disc
                        Taxable_Amount = Val(.Rows(RowIndx).Cells(dgvCol_DelvDetails.AMount).Value) - Val(.Rows(RowIndx).Cells(dgvCol_DelvDetails.Disc_AMt).Value)


                        .Rows(RowIndx).Cells(dgvCol_DelvDetails.Taxable_value).Value = Format(Val(Taxable_Amount), "##########0.00")
                        .Rows(RowIndx).Cells(dgvCol_DelvDetails.Gst_Perc).Value = Format(Val(GST_Per), "########0.00")
                        .Rows(RowIndx).Cells(dgvCol_DelvDetails.Hsn_Code).Value = Trim(HSN_Code)

                        ' End If

                    Next

                    Get_HSN_CodeWise_Tax_Details()

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT DO GST CALCULATION...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Get_GST_Percentage_From_ItemGroup(ByVal ClothName As String, ByRef HSN_Code As String, ByRef GST_PerCent As Single)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub
        Try


            HSN_Code = ""
            GST_PerCent = 0
            da = New SqlClient.SqlDataAdapter("select a.* from ItemGroup_Head a INNER JOIN Cloth_Head b ON a.ItemGroup_IdNo = b.ItemGroup_IdNo Where b.Cloth_Name ='" & Trim(ClothName) & "'", con)
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

            '  If cbo_TaxType.Text = "GST" Then

            AssVal_Pack_Frgt_Ins_Amt = Format((Val(txt_Freight.Text)) + (Val(txt_AddLess_BeforeTax.Text)), "#########0.00")

            With Transport

                If .Rows.Count > 0 Then
                    For i = 0 To .Rows.Count - 1
                        If Trim(.Rows(i).Cells(dgvCol_DelvDetails.Cloth_name).Value) <> "" And Val(.Rows(i).Cells(dgvCol_DelvDetails.Gst_Perc).Value) <> 0 And Trim(.Rows(i).Cells(dgvCol_DelvDetails.Hsn_Code).Value) <> "" Then
                            cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & " (                    Name1                ,                  Currency1            ,                       Currency2                                             ) " &
                                                "          Values     ( '" & Trim(.Rows(i).Cells(dgvCol_DelvDetails.Hsn_Code).Value) & "', " & Val(.Rows(i).Cells(dgvCol_DelvDetails.Gst_Perc).Value) & " ,  " & Str(Val(.Rows(i).Cells(dgvCol_DelvDetails.Taxable_value).Value) + Val(AssVal_Pack_Frgt_Ins_Amt)) & " ) "
                            cmd.ExecuteNonQuery()

                            AssVal_Pack_Frgt_Ins_Amt = 0

                        End If
                    Next

                End If

            End With

            ' End If


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

    Private Sub cbo_PartyName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName.LostFocus
        If Trim(UCase(cbo_PartyName.Tag)) <> Trim(UCase(cbo_PartyName.Text)) Then
            cbo_PartyName.Tag = cbo_PartyName.Text
            GST_Calculation()
        End If
    End Sub

    Private Sub btn_Tax_Click(sender As System.Object, e As System.EventArgs) Handles btn_Tax.Click
        pnl_Back.Enabled = False
        pnl_Tax.Visible = True
        pnl_Tax.Focus()
    End Sub

    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub Printing_FormatGST_1334(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        Dim CntName1 As String, CntName2 As String


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
            .Left = 30
            .Right = 50
            .Top = 30
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

        NoofItems_PerPage = 12 ' 6

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(35) : ClArr(2) = 110 : ClArr(3) = 70 : ClArr(4) = 55 : ClArr(5) = 50 : ClArr(6) = 60 : ClArr(7) = 90 : ClArr(8) = 70
        ClArr(9) = 85
        ClArr(10) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9))

        TxtHgt = 18.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_FormatGST_PageHeader_1334(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_FormatGST_PageFooter_1334(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1


                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 20 Then
                            For I = 20 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 20
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CntName1 = prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString
                        CntName2 = ""
                        If Len(CntName1) > 10 Then
                            For I = 15 To 1 Step -1
                                If Mid$(Trim(CntName1), I, 1) = " " Or Mid$(Trim(CntName1), I, 1) = "," _
                                    Or Mid$(Trim(CntName1), I, 1) = "." Or Mid$(Trim(CntName1), I, 1) = "-" _
                                    Or Mid$(Trim(CntName1), I, 1) = "/" Or Mid$(Trim(CntName1), I, 1) = "_" _
                                    Or Mid$(Trim(CntName1), I, 1) = "(" Or Mid$(Trim(CntName1), I, 1) = ")" _
                                    Or Mid$(Trim(CntName1), I, 1) = "\" Or Mid$(Trim(CntName1), I, 1) = "[" _
                                    Or Mid$(Trim(CntName1), I, 1) = "]" Or Mid$(Trim(CntName1), I, 1) = "{" Or Mid$(Trim(CntName1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 20

                            CntName2 = Microsoft.VisualBasic.Right(Trim(CntName1), Len(CntName1) - I)
                            CntName1 = Microsoft.VisualBasic.Left(Trim(CntName1), I - 1)
                        End If

                        CurY = CurY + TxtHgt

                        SNo = SNo + 1
                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, CntName1, LMargin + ClArr(1) + 5, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("HSN_Code").ToString, LMargin + ClArr(1) + ClArr(2) + 5, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("GST_Percentage").ToString), "############0.0") & "%", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 5, CurY, 1, 0, pFont)
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Bags").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 5, CurY, 1, 0, pFont)
                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cones").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 5, CurY, 1, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Amount").ToString), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Description").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + 2, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Count_Description").ToString, PageWidth - 5, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        If Trim(CntName2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(CntName2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If


                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If


                Printing_FormatGST_PageFooter_1334(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)


            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_FormatGST_PageHeader_1334(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim C1 As Single, C2 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim DelvToName As String = ""
        Dim CurY1 As Single = 0, CurX As Single = 0
        PageNo = PageNo + 1

        CurY = TMargin

        'da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Ledger_Name as Transport_Name from cloth_Purchase_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Transport_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.cloth_Purchase_Code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
        'da2.Fill(dt2)
        'If dt2.Rows.Count > NoofItems_PerPage Then
        '    Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        'End If
        'dt2.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = "" : Cmp_EMail = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address1").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address2").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address3").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Company_Factory_Address4").ToString) <> "" Then
            Cmp_Add1 = "HO : " & prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
            Cmp_Add2 = "BO : " & prn_HdDt.Rows(0).Item("Company_Factory_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Factory_Address4").ToString

        Else
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
            If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
                Cmp_PanNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
            End If
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

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1084" Then
        '    Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, LMargin, CurY, 2, PrintWidth, pFont)
        'End If

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
        Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_GSTIN_Cap, CurX, CurY, 0, PrintWidth, p1Font)
        strWidth = e.Graphics.MeasureString("     " & Cmp_GSTIN_Cap, p1Font).Width
        CurX = CurX + strWidth
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, CurX, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_EMail), LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "cloth PURCHASE ORDER", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
            C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
            W1 = e.Graphics.MeasureString("PURCHASE NO     : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "PO No", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("cloth_Purchase_Order_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "PO Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("cloth_Purchase_Order_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Due Days", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Due_Days").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Delivery Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            'DelvToName = Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("DeliveryTo_Idno").ToString))
            'Common_Procedures.Print_To_PrintDocument(e, "Delivery To", LMargin + C1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, DelvToName, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                pFont = New Font("Calibri", 11, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, " GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If
            'Common_Procedures.Print_To_PrintDocument(e, "Reverse Charge (Y/N)", LMargin + C1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "YES", LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

            CurY = CurY + 10

            pFont = New Font("Calibri", 11, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "Transport Name", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("TransportName").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "Transport Name : " & Trim(prn_HdDt.Rows(0).Item("Transport_Name").ToString), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Agent Name", LMargin + C2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + C2 + W1 + 30, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Vehicle No", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

            CurY = CurY + TxtHgt - 5
            pFont = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "COUNT", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GST%", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BAGS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CONES", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Count Disc", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY
            pFont = New Font("Calibri", 11, FontStyle.Regular)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_FormatGST_PageFooter_1334(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim C1 As Single, W1 As Single
        Dim vTaxPerc As Single = 0
        Dim BmsInWrds As String
        Dim ItmNm1 As String = "", ItmNm2 As String = "", ItmNm3 As String = "", ItmNm4 As String = "", ItmNm5 As String = "", ItmNm6 As String = "", ItmNm7 As String = "", ItmNm8 As String = "", ItmNm9 As String = "", ItmNm10 As String = ""
        Dim DelvAddAr() As String
        Dim DelInc As Integer = 0


        Try

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt

                prn_DetIndx = prn_DetIndx + 1

            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("Total_Cones").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Weight").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Amount").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 1, 0, pFont)


            CurY = CurY + TxtHgt - 10

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(4))

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10
            W1 = e.Graphics.MeasureString("DISCOUNT    : ", pFont).Width


            CurY = CurY + TxtHgt - 10
            If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> 0 Then
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Discount @ " & Trim(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString) & "%", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "Discount ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                End If
            End If
            p1Font = New Font("Calibri", 10, FontStyle.Bold Or FontStyle.Underline)
            Common_Procedures.Print_To_PrintDocument(e, "DELIVERY ADDRESS : ", LMargin + 10, CurY, 0, 0, p1Font)

            Erase DelvAddAr
            If Trim(prn_HdDt.Rows(0).Item("Delivery_Address").ToString) <> "" Then
                DelvAddAr = Split(Trim(prn_HdDt.Rows(0).Item("Delivery_Address").ToString), ",")
                DelInc = -1

                DelInc = DelInc + 1
                If UBound(DelvAddAr) >= DelInc Then
                    ItmNm1 = Trim(DelvAddAr(DelInc))
                End If

                DelInc = DelInc + 1
                If UBound(DelvAddAr) >= DelInc Then
                    ItmNm2 = Trim(DelvAddAr(DelInc))
                End If

                DelInc = DelInc + 1
                If UBound(DelvAddAr) >= DelInc Then
                    ItmNm3 = Trim(DelvAddAr(DelInc))
                End If

                DelInc = DelInc + 1
                If UBound(DelvAddAr) >= DelInc Then
                    ItmNm4 = Trim(DelvAddAr(DelInc))
                End If

                DelInc = DelInc + 1
                If UBound(DelvAddAr) >= DelInc Then
                    ItmNm5 = Trim(DelvAddAr(DelInc))
                End If

                DelInc = DelInc + 1
                If UBound(DelvAddAr) >= DelInc Then
                    ItmNm6 = Trim(DelvAddAr(DelInc))
                End If

                DelInc = DelInc + 1
                If UBound(DelvAddAr) >= DelInc Then
                    ItmNm7 = Trim(DelvAddAr(DelInc))
                End If

                DelInc = DelInc + 1
                If UBound(DelvAddAr) >= DelInc Then
                    ItmNm8 = Trim(DelvAddAr(DelInc))
                End If

                DelInc = DelInc + 1
                If UBound(DelvAddAr) >= DelInc Then
                    ItmNm9 = Trim(DelvAddAr(DelInc))
                End If
                DelInc = DelInc + 1
                If UBound(DelvAddAr) >= DelInc Then
                    ItmNm10 = Trim(DelvAddAr(DelInc))
                End If
            End If

            p1Font = New Font("Calibri", 10, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm1, LMargin + 10, CurY + TxtHgt, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm2, LMargin + 10, CurY + TxtHgt + TxtHgt, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm3, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm4, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm5, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm6, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm7, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm8, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm9, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm10, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)


            Common_Procedures.Print_To_PrintDocument(e, " Payment Terms  :  " & prn_HdDt.Rows(0).Item("Due_Days").ToString & " Days", LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm8, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " Delivery Date : " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm10, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)


            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Due_Days").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)


            'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("AddLess_BeforeTax_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "AddLess Before Tax", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("AddLess_BeforeTax_Amount").ToString), PageWidth - 5, CurY, 1, 0, pFont)
                End If
            End If

            'Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("DelAdd1").ToString, LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Freight", LMargin + C1 + 10, CurY, 0, 0, pFont)
                ' Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), PageWidth - 5, CurY, 1, 0, pFont)
                End If
            End If
            'Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("DelAdd2").ToString, LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "AddLess After Tax", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString), PageWidth - 5, CurY, 1, 0, pFont)
                End If
            End If


            'Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("DelAdd3").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("DelAdd4").ToString, LMargin + 10, CurY + TxtHgt, 0, 0, p1Font)



            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "Total  Before Tax  ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 10



            vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)

            If Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
                End If

            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)

            End If
            CurY = CurY + TxtHgt

            If Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
                End If

            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
                End If

            Else
                Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
            End If
            CurY = CurY + TxtHgt

            If Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "RoundOff ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
                End If

            End If


            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
            LnAr(8) = CurY
            CurY = CurY + TxtHgt - 10
            'Common_Procedures.Print_To_PrintDocument(e, "E & O.E", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GRAND TOTAL", LMargin + C1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Net_Amount").ToString), PageWidth - 5, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)
            LnAr(8) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "GST on Reverse Charge", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + +ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
            CurY = CurY + TxtHgt - 10
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
            BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "", "")

            Common_Procedures.Print_To_PrintDocument(e, "Rupees            : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            'CurY = CurY + 10
            'p1Font = New Font("Calibri", 12, FontStyle.Underline)
            'Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)

            'CurY = CurY + TxtHgt + 10
            'Common_Procedures.Print_To_PrintDocument(e, "1. We are responsible for the quality of cloth only;If any running fault or quality  ", LMargin + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "   defect noted in cloth please inform with firat fabric roll at once.We will", LMargin + 10, CurY, 0, 0, pFont)
            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "   accept only one roll at defect otherwise we do not hold ourself responsible. ", LMargin + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'Common_Procedures.Print_To_PrintDocument(e, "2. Our responsibility ceases when goods leave our permises.", LMargin + 10, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt
            'If Val(Common_Procedures.User.IdNo) <> 1 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            'End If

            CurY = CurY + TxtHgt - 10
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt


            CurY = CurY + TxtHgt



            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)



            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub
    Private Function get_GST_Tax_Percentage_For_Printing(ByVal EntryCode As String) As Single
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim TaxPerc As Single = 0

        TaxPerc = 0

        Da = New SqlClient.SqlDataAdapter("Select * from cloth_Purchase_Order_GST_Tax_Details Where cloth_Purchase_Order_Code = '" & Trim(EntryCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If Dt1.Rows.Count = 1 Then

                Da = New SqlClient.SqlDataAdapter("Select * from cloth_Purchase_Order_GST_Tax_Details Where cloth_Purchase_Order_Code = '" & Trim(EntryCode) & "'", con)
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


    Private Sub cbo_DespTo_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_DespTo.KeyUp
        If e.Control = False And e.KeyValue = 17 Then

            Common_Procedures.MDI_LedType = ""
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_DespTo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Grid_loomType_GotFocus(sender As Object, e As EventArgs) Handles cbo_Grid_LoomType.GotFocus
        vCbo_ItmNm = Trim(cbo_Grid_LoomType.Text)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "LoomType_Head", "Loomtype_Name", "  ", "(LoomType_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_loomType_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Grid_LoomType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_LoomType, Nothing, Nothing, "LoomType_Head", "Loomtype_Name", "  ", "(LoomType_IdNo = 0)")
        If (e.KeyValue = 40 And cbo_Grid_LoomType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If Transport.Rows.Count > 0 Then
                Transport.Focus()
                Transport.CurrentCell = Transport.Rows(Transport.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.Cloth_name)
            End If

        ElseIf (e.KeyValue = 38 And cbo_Grid_LoomType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If Transport.Rows.Count > 0 Then
                Transport.Focus()
                Transport.CurrentCell = Transport.Rows(Transport.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.Order_Mtrs)
            End If

        End If
    End Sub

    Private Sub cbo_Grid_loomType_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Grid_LoomType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_LoomType, Nothing, "LoomType_Head", "Loomtype_Name", "  ", "(LoomType_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If Transport.Rows.Count > 0 Then
                Transport.Focus()

                Transport.CurrentCell = Transport.Rows(Transport.CurrentCell.RowIndex).Cells(dgvCol_DelvDetails.Order_Mtrs)

            End If
        End If
    End Sub

    Private Sub cbo_Grid_loomType_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_Grid_LoomType.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.MDI_LedType = ""
            Dim f As New LoomType_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_LoomType.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Grid_loomtype_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_LoomType.TextChanged
        Try
            If cbo_Grid_LoomType.Visible Then
                If IsNothing(Transport.CurrentCell) Then Exit Sub
                With Transport
                    If Val(cbo_Grid_LoomType.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = dgvCol_DelvDetails.Loom_Type Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_LoomType.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub


    Private Sub Printing_Format_1186(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        Dim amount As Integer = 0

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1

            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next


        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20
            .Right = 50
            .Top = 15
            .Bottom = 40
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

        NoofItems_PerPage = 5

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(35) : ClAr(2) = 320 : ClAr(3) = 80 : ClAr(4) = 0 : ClAr(5) = 100 : ClAr(6) = 80
        ClAr(7) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6))

        TxtHgt = 17.5

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format_1186_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format_1186_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        ItmNm1 = "Const.      " & ":  " & Trim(prn_DetDt.Rows(prn_DetIndx).Item("ClothMain_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 50 Then
                            For I = 50 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 50
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, "Sort No    " & ":  " & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sort_No").ToString), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Item_HSN_Code").ToString), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Fold_Perc").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Order_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

                        amount = Val(prn_DetDt.Rows(prn_DetIndx).Item("Order_Meters").ToString) * Val(prn_DetDt.Rows(prn_DetIndx).Item("Rate"))
                        Common_Procedures.Print_To_PrintDocument(e, Val(amount), PageWidth - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1
                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

                        If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Weave").ToString) <> "" Then
                            CurY = CurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, "Weave     " & ":  " & prn_DetDt.Rows(prn_DetIndx).Item("weave").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

                        If Trim(prn_DetDt.Rows(prn_DetIndx).Item("loomType_name").ToString) <> "" Then
                            CurY = CurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, "Quality    " & ":  " & prn_DetDt.Rows(prn_DetIndx).Item("loomType_name").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight_Meter_Fabric").ToString) <> 0 Then
                            CurY = CurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, "Weight     " & ": " & Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight_Meter_Fabric").ToString), "#####0.0000") & " Kgs", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If
                        'Weight_Meter_Fabric
                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Format_1186_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format_1186_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String, Cmp_Add3 As String, city As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String

        Dim strHeight As Single
        Dim C1 As Single = 0
        Dim W1 As Single = 0, w2 As Single = 0
        Dim S1 As Single = 0, s2 As Single = 0
        Dim strHeight1 As Single
        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_name, d.ClothType_name from ClothPurchase_Order_Details a INNER JOIN Cloth_Head b ON a.Cloth_idno = b.Cloth_idno LEFT OUTER JOIN ClothType_Head d ON a.ClothType_idno = d.ClothType_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothPurchase_Order_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY


        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = "" : Cmp_EMail = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""


        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString

        'Cmp_Add1 = "Regd. Off : " & prn_HdDt.Rows(0).Item("Company_Address1").ToString
        'Cmp_Add2 = "Factory : " & prn_HdDt.Rows(0).Item("Company_Address2").ToString

        If InStr(1, Trim(UCase(Cmp_Name)), "UNITED") > 0 And InStr(1, Trim(UCase(Cmp_Name)), "WEAVES") > 0 Then
            Cmp_Add1 = "Regd. Off : " & prn_HdDt.Rows(0).Item("Company_Address1").ToString
            Cmp_Add2 = "Factory : " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Else
            Cmp_Add1 = "" & prn_HdDt.Rows(0).Item("Company_Address1").ToString
            Cmp_Add2 = "" & prn_HdDt.Rows(0).Item("Company_Address2").ToString

        End If

        Cmp_Add3 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(0).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_CstNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_EMail").ToString) <> "" Then
            Cmp_EMail = "EMAIL ID : " & prn_HdDt.Rows(0).Item("Company_EMail").ToString
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_No = "GSTIN :" & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_City").ToString) <> "" Then
            City = "" & prn_HdDt.Rows(0).Item("Company_City").ToString
        End If
        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" Then
            If InStr(1, Trim(UCase(Cmp_Name)), "UNITED") > 0 And InStr(1, Trim(UCase(Cmp_Name)), "WEAVES") > 0 Then
                e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.united_weaves_logo_png, Drawing.Image), PageWidth - 150, CurY, 120, 100)
            End If
        End If

        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin + 10, CurY, 0, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin + 10, CurY, 0, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add3 & "," & City, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt

        'Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No & "  / " & Cmp_CstNo, LMargin + 10, CurY, 0, PrintWidth, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo & "/ " & Cmp_EMail, LMargin + 10, CurY, 0, PrintWidth, pFont)
        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_EMail, LMargin + 10, CurY, 0, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 12
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        CurY = CurY + TxtHgt - 12
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        If Common_Procedures.settings.CustomerCode = "1186" Then
            Common_Procedures.Print_To_PrintDocument(e, "FABRIC PURCHASE ORDER", LMargin, CurY, 2, PrintWidth, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, "CLOTH PURCHASE ORDER", LMargin, CurY, 2, PrintWidth, p1Font)
        End If
        C1 = ClAr(1) + ClAr(2) + ClAr(3) + 50
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(15) = CurY

        CurY = CurY + 10
        p1Font = New Font("Calibri", 10, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "PO No. :  GF/PO-" & prn_HdDt.Rows(0).Item("ClothPurchase_Order_No").ToString & "/" & Common_Procedures.FnYearCode, LMargin + 10, CurY, 0, 0, p1Font)
        'strHeight1 = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("ClothPurchase_Order_No").ToString, p1Font).Height

        Common_Procedures.Print_To_PrintDocument(e, "PO Date : " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("ClothPurchase_Order_Date").ToString), "dd-MM-yyyy").ToString, LMargin + ClAr(1) + (ClAr(2) / 2) + 20, CurY, 2, 0, p1Font)
        'strHeight1 = e.Graphics.MeasureString(Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("ClothPurchase_Order_Date").ToString), "dd-MM-yyyy").ToString, p1Font).Height

        Common_Procedures.Print_To_PrintDocument(e, "Party SO No. : " & prn_HdDt.Rows(0).Item("Party_OrderNo").ToString.ToString, LMargin + C1 + 10, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 50, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 50, LnAr(15))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + (ClAr(2) / 2) + 10, CurY, LMargin + ClAr(1) + (ClAr(2) / 2) + 10, LnAr(15))
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + (ClAr(3) / 2)
        W1 = e.Graphics.MeasureString("PARTY S.O NO: ", pFont).Width
        w2 = e.Graphics.MeasureString("Desp TO : ", pFont).Width
        S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width


        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold Or FontStyle.Italic Or FontStyle.Underline)
        Common_Procedures.Print_To_PrintDocument(e, "TO  :  ", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "DELIVERY AT : ", LMargin + C1 + 10, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt + 5
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "M/s." & prn_HdDt.Rows(0).Item("Ledger_MAINName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "M/s." & prn_HdDt.Rows(0).Item("Delivery_partyname").ToString, LMargin + C1 + 10, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Delivery_Add1").ToString, LMargin + C1 + 20, CurY, 0, 0, pFont)

        p1Font = New Font("Calibri", 14, FontStyle.Bold)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Delivery_Add2").ToString, LMargin + C1 + 20, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Delivery_Add3").ToString, LMargin + C1 + 20, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Delivery_Add4").ToString, LMargin + C1 + 20, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, " GSTIN : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

        End If

        If prn_HdDt.Rows(0).Item("Delivery_GSTinNo").ToString <> "" Then


            Common_Procedures.Print_To_PrintDocument(e, "GSTIN  : " & prn_HdDt.Rows(0).Item("Delivery_GSTinNo").ToString, LMargin + C1 + 20, CurY, 0, 0, pFont)

        End If
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        CurY = CurY + 10

        Common_Procedures.Print_To_PrintDocument(e, "THROUGH     :  " & prn_HdDt.Rows(0).Item("Through_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, " AGENT NAME  : Mr ." & Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Agent_IdNo").ToString)), LMargin + C1 + 20, CurY, 0, 0, pFont)


        'Delivery_GSTinNo
        CurY = CurY + TxtHgt



        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        CurY = CurY + 10
        If Trim(prn_HdDt.Rows(0).Item("Attend").ToString) <> "" Then
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Kind Attn . :  ", LMargin + ClAr(1) + ClAr(2) - 80, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Mr." & Trim(prn_HdDt.Rows(0).Item("Attend").ToString), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, p1Font)

        End If
        CurY = CurY + TxtHgt + 20

        Common_Procedures.Print_To_PrintDocument(e, "Dear Sir, ", LMargin + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt + 5
        Common_Procedures.Print_To_PrintDocument(e, "We are hereby confirming the purchase of cotton greige fabrics as follows :", LMargin + ClAr(1), CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt + 20

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY
        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DESCRIPTION", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        ' Common_Procedures.Print_To_PrintDocument(e, "FOLD%", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ORDER MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "RATE/MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "TOTAL AMOUNT (Rs)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        CurY = CurY + 15
        Common_Procedures.Print_To_PrintDocument(e, "(Excl.gst)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY


    End Sub

    Private Sub Printing_Format_1186_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim C1 As Single, W1 As Single
        Dim vTaxPerc As Single = 0
        Dim BmsInWrds As String
        Dim ItmNm1 As String = "", ItmNm2 As String = "", ItmNm3 As String = "", ItmNm4 As String = "", ItmNm5 As String = "", ItmNm6 As String = "", ItmNm7 As String = "", ItmNm8 As String = "", ItmNm9 As String = "", ItmNm10 As String = ""
        Dim DelvAddAr() As String
        Dim DelInc As Integer = 0

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        CurY = CurY + TxtHgt - 10
        If is_LastPage = True Then
            Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 30, CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Order_Meters").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))


        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10
        W1 = e.Graphics.MeasureString(" Delivery Schedule    : ", pFont).Width


        CurY = CurY + TxtHgt + 10

        '& prn_HdDt.Rows(0).Item("Attend").ToString


        p1Font = New Font("Calibri", 10, FontStyle.Bold)

        ' Common_Procedures.Print_To_PrintDocument(e, " Payment" & Trim(prn_HdDt.Rows(0).Item("Net_Amount").ToString), LMargin + 10, CurY, 0, 0, p1Font)

        If Trim(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, " Cash Discount", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " : " & Trim(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString) & " %", LMargin + W1 + 10, CurY, 0, 0, pFont)

        End If
        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, " Tax Type ", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, " : " & Trim(prn_HdDt.Rows(0).Item("Tax_Type").ToString), LMargin + W1 + 10, CurY, 0, 0, pFont)


        'AddLessAfterTax_Text
        If val(prn_HdDt.Rows(0).Item("Gr_time").ToString) <> 0 Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " Payment Days", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " : " & Format(Val(prn_HdDt.Rows(0).Item("Gr_time").ToString), "#####0") & "  Days  " & " / " & Trim(prn_HdDt.Rows(0).Item("Gr_Date").ToString), LMargin + W1 + 10, CurY, 0, 0, pFont)
        Else
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " Payment Days", LMargin + 10, CurY, 0, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, " : " & Format(Convert.ToDateTime((prn_HdDt.Rows(0).Item("Gr_Date").ToString)), "dd-MM-yyyy").ToString, LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " : AGAINST RTGS \ PROFORMA", LMargin + W1 + 10, CurY, 0, 0, pFont)
        End If
        'CurY = CurY + TxtHgt
        '    Common_Procedures.Print_To_PrintDocument(e, " Delivery Schedule", LMargin + 10, CurY, 0, 0, p1Font)
        '    Common_Procedures.Print_To_PrintDocument(e, " :  Before  " & Trim(prn_HdDt.Rows(0).Item("Delivery_Due_Days").ToString) & "   Days " & " / " & Format(Convert.ToDateTime((prn_HdDt.Rows(0).Item("Delivery_Due_Date").ToString)), "dd-MM-yyyy").ToString, LMargin + W1 + 10, CurY, 0, 0, pFont)
        If Val(prn_HdDt.Rows(0).Item("Delivery_Due_Days").ToString) <> 0 Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " Delivery Schedule", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " : Before  " & Trim(prn_HdDt.Rows(0).Item("Delivery_Due_Days").ToString) & "   Days " & " / " & Format(Convert.ToDateTime((prn_HdDt.Rows(0).Item("Delivery_Due_Date").ToString)), "dd-MM-yyyy").ToString, LMargin + W1 + 10, CurY, 0, 0, pFont)
        Else
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " Delivery Schedule", LMargin + 10, CurY, 0, 0, p1Font)
            ' Common_Procedures.Print_To_PrintDocument(e, " : " & Format(Convert.ToDateTime((prn_HdDt.Rows(0).Item("Delivery_Date").ToString)), "dd-MM-yyyy").ToString, LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, " :IMMEDIATE ", LMargin + W1 + 10, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, " Transportation", LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, " : " & Trim(prn_HdDt.Rows(0).Item("Transportations").ToString), LMargin + W1 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("Note").ToString) <> "" Then



            Common_Procedures.Print_To_PrintDocument(e, " Remarks", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, " : " & Trim(prn_HdDt.Rows(0).Item("Note").ToString), LMargin + W1 + 10, CurY, 0, 0, pFont)
        End If
        CurY = CurY + TxtHgt + 20
        p1Font = New Font("Calibri", 12, FontStyle.Bold Or FontStyle.Underline)
        Common_Procedures.Print_To_PrintDocument(e, "Terms & Conditions :", LMargin + 10, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt + 10
        Common_Procedures.Print_To_PrintDocument(e, "a). Fabric should be free from Weaving defects, Slubs & Stains & of top Dyeable Quality.", LMargin + 12, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "b). Piece Length: 80 % : 150 mtrs and above, 20 % : 40-79 mtrs. No Short Length Will be accepted.", LMargin + 12, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "c). No Variation in Count/Reed/Pick/Width is accepted.", LMargin + 12, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "d). Delivery Date should be Stictly adhered to.", LMargin + 12, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "e). Kindly mention the PO number in the Invoice.", LMargin + 12, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 20
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, LMargin + 10, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1186" And Print_PDF_Status = True Then

            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.UNITED_WEAVES_SIGN, Drawing.Image), LMargin + 10, CurY, 90, 55)
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt

        End If

        'Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 350, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", LMargin + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)



    End Sub

    Private Sub cbo_freight_Ac_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_freight_Ac.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothPurchase_Order_Head", "Transportations", "", "(ClothPurchase_Order_No=0)")

    End Sub
    Private Sub cbo_freight_Ac_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_freight_Ac.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_freight_Ac, msk_delivery_date, cbo_taxType, "ClothPurchase_Order_Head", "Transportations", "", "(ClothPurchase_Order_No=0)")

    End Sub

    Private Sub cbo_freight_Ac_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_freight_Ac.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_freight_Ac, cbo_taxType, "ClothPurchase_Order_Head", "Transportations", "", "(ClothPurchase_Order_No = 0)", False)

    End Sub

    Private Sub txt_delivery_due_days_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_delivery_due_days.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_delivery_due_days_TextChanged(sender As Object, e As EventArgs) Handles txt_delivery_due_days.TextChanged
        GraceTime_Calculation()

    End Sub

    Private Sub dtp_delivery_date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_delivery_date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_delivery_date.Text = Date.Today
        End If
    End Sub

    Private Sub dtp_delivery_date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_delivery_date.TextChanged
        If IsDate(dtp_delivery_date.Text) = True Then

            msk_delivery_date.Text = dtp_delivery_date.Text
            msk_delivery_date.SelectionStart = 0
        End If
    End Sub

    Private Sub dtp_delivery_date_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtp_delivery_date.ValueChanged
        msk_delivery_date.Text = dtp_delivery_date.Text
    End Sub

    Private Sub dtp_delivery_date_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_delivery_date.Enter
        msk_delivery_date.Focus()
        msk_delivery_date.SelectionStart = 0
    End Sub

    Private Sub cbo_taxType_GotFocus(sender As Object, e As EventArgs) Handles cbo_taxType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothPurchase_Order_Head", "tax_type", "", "(ClothPurchase_Order_No=0)")
    End Sub

    Private Sub cbo_taxType_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_taxType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_taxType, cbo_freight_Ac, Nothing, "ClothPurchase_Order_Head", "tax_type", "", "(ClothPurchase_Order_No=0)")
        If (e.KeyValue = 40) Then

            If Transport.Rows.Count > 0 Then
                Transport.Focus()
                Transport.CurrentCell = Transport.Rows(0).Cells(dgvCol_DelvDetails.Cloth_name)

            Else
                txt_Note.Focus()

            End If


        End If
    End Sub

    Private Sub cbo_taxType_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_taxType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_taxType, Nothing, "ClothPurchase_Order_Head", "tax_type", "", "(ClothPurchase_Order_No = 0)", False)
        If Asc(e.KeyChar) = 13 Then
            If Transport.Rows.Count > 0 Then
                Transport.Focus()
                Transport.CurrentCell = Transport.Rows(0).Cells(dgvCol_DelvDetails.Cloth_name)

            Else
                txt_Note.Focus()

            End If
        End If
    End Sub

    Private Sub btn_PDF_Click(sender As Object, e As EventArgs) Handles btn_PDF.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = True
        print_record()
    End Sub
    Private Sub Printing_Format3_1464(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        Dim amount As Integer = 0

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1

            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next


        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20
            .Right = 50
            .Top = 15
            .Bottom = 40
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

        NoofItems_PerPage = 8 '10

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(35) : ClAr(2) = 350 : ClAr(3) = 0 : ClAr(4) = 90 : ClAr(5) = 100 : ClAr(6) = 80
        ClAr(7) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6))

        TxtHgt = 18

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(EntFnYrCode)

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format3_1464_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format3_1464_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Name").ToString)
                        ItmNm2 = ""
                        If Len(ItmNm1) > 45 Then
                            For I = 45 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                            Next I
                            If I = 0 Then I = 45
                            ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                            ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        End If

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Sl_No").ToString), LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("ClothType_Name").ToString, LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Item_HSN_Code").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Order_Meters").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Rate").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

                        amount = Val(prn_DetDt.Rows(prn_DetIndx).Item("Order_Meters").ToString) * Val(prn_DetDt.Rows(prn_DetIndx).Item("Rate"))
                        Common_Procedures.Print_To_PrintDocument(e, Val(amount), PageWidth - 10, CurY, 1, 0, pFont)

                        NoofDets = NoofDets + 1

                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 5
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                            NoofDets = NoofDets + 1
                        End If

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

                Printing_Format3_1464_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format3_1464_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_GstNo As String, Cmp_UAM_No As String
        Dim strHeight As Single
        Dim C1 As Single = 0
        Dim W1 As Single = 0, w2 As Single = 0
        Dim S1 As Single = 0, s2 As Single = 0

        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_name, d.ClothType_name from ClothPurchase_Order_Details a INNER JOIN Cloth_Head b ON a.Cloth_idno = b.Cloth_idno LEFT OUTER JOIN ClothType_Head d ON a.ClothType_idno = d.ClothType_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.ClothPurchase_Order_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_UAM_No = ""

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
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GstNo = "GSTIN : " & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_UAM_No").ToString) <> "" Then
            Cmp_UAM_No = "MSME : " & prn_HdDt.Rows(0).Item("Company_UAM_No").ToString
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
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GstNo, LMargin, CurY, 2, PrintWidth, pFont)
        If Trim(Cmp_UAM_No) <> "" Then
            CurY = CurY + TxtHgt - 1
            Common_Procedures.Print_To_PrintDocument(e, Cmp_UAM_No, LMargin, CurY, 2, PrintWidth, pFont)
        End If
        If Trim(Cmp_TinNo) <> "" Or Trim(Cmp_CstNo) <> "" Then

            CurY = CurY + TxtHgt - 1
            Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)
        End If
        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "FABRIC PURCHASE ORDER", LMargin, CurY + 5, 2, PrintWidth, p1Font)


        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + 50
        W1 = e.Graphics.MeasureString("PARTY S.O NO: ", pFont).Width
        w2 = e.Graphics.MeasureString("DESP.TO : ", pFont).Width
        S1 = e.Graphics.MeasureString("TO  :  ", pFont).Width


        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "PO.NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("ClothPurchase_Order_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 14, FontStyle.Bold)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("ClothPurchase_Order_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt + 10
        'Delivery_GSTinNo
        Common_Procedures.Print_To_PrintDocument(e, "GSTIN NO : " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PARTY S.O NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Party_OrderNo").ToString.ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        CurY = CurY + 10

        Common_Procedures.Print_To_PrintDocument(e, "Payment Terms     :  " & prn_HdDt.Rows(0).Item("Payment_Terms_Details").ToString, LMargin + 10, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "DESP.TO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Delivery_partyname").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Delivery_Add1").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        Dim delvdet1 = ""
        Dim delvdet2 = ""
        Dim i As Integer = 0
        Dim vwidth = 0F

        delvdet1 = Trim(prn_HdDt.Rows(0).Item("Delivery_Terms_Details").ToString)
        delvdet2 = ""

        If Trim(delvdet1) <> "" Then
            If Len(delvdet1) > 35 Then

                For i = 35 To 1 Step -1
                    If Mid$(Trim(delvdet1), i, 1) = " " Or Mid$(Trim(delvdet1), i, 1) = "," Or Mid$(Trim(delvdet1), i, 1) = "." Or Mid$(Trim(delvdet1), i, 1) = "-" Or Mid$(Trim(delvdet1), i, 1) = "/" Or Mid$(Trim(delvdet1), i, 1) = "_" Or Mid$(Trim(delvdet1), i, 1) = "(" Or Mid$(Trim(delvdet1), i, 1) = ")" Or Mid$(Trim(delvdet1), i, 1) = "\" Or Mid$(Trim(delvdet1), i, 1) = "[" Or Mid$(Trim(delvdet1), i, 1) = "]" Or Mid$(Trim(delvdet1), i, 1) = "{" Or Mid$(Trim(delvdet1), i, 1) = "}" Then Exit For
                Next
                If i = 0 Then i = 35
                delvdet2 = Microsoft.VisualBasic.Right(Trim(delvdet1), Len(delvdet1) - i)
                delvdet1 = Microsoft.VisualBasic.Left(Trim(delvdet1), i - 1)
            End If

        End If



        Common_Procedures.Print_To_PrintDocument(e, "Delivery Terms      :  " & Trim(delvdet1), LMargin + 10, CurY + 5, 0, 0, pFont)

        vwidth = e.Graphics.MeasureString("Delivery Terms      :  ", pFont).Width

        CurY = CurY + TxtHgt
        'If Val(prn_HdDt.Rows(0).Item("Agent_IdNo").ToString) <> 0 Then
        '    Common_Procedures.Print_To_PrintDocument(e, " AGENT NAME : " & Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Agent_IdNo").ToString)), LMargin + 10, CurY, 0, 0, pFont)
        'End If

        If Trim(delvdet2) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(delvdet2), LMargin + vwidth + 10, CurY + 5, 0, 0, pFont)
        End If
        '
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Delivery_Add2").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        If Trim(prn_HdDt.Rows(0).Item("Attend").ToString) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, "Attend Mr. " & prn_HdDt.Rows(0).Item("Attend").ToString, LMargin + 10, CurY + 5, 0, 0, pFont)
        End If
        Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Delivery_Add3").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)
        If Trim(prn_HdDt.Rows(0).Item("Delivery_Add4").ToString) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Delivery_Add4").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)
        End If
        If Trim(prn_HdDt.Rows(0).Item("Delivery_GSTinNo").ToString) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " GSTIN NO : " & prn_HdDt.Rows(0).Item("Delivery_GSTinNo").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "CLOTH NAME", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        '  Common_Procedures.Print_To_PrintDocument(e, "TYPE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "ORDER MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY


    End Sub

    Private Sub Printing_Format3_1464_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim C1 As Single, W1 As Single
        Dim vTaxPerc As Single = 0
        Dim BmsInWrds As String
        Dim ItmNm1 As String = "", ItmNm2 As String = "", ItmNm3 As String = "", ItmNm4 As String = "", ItmNm5 As String = "", ItmNm6 As String = "", ItmNm7 As String = "", ItmNm8 As String = "", ItmNm9 As String = "", ItmNm10 As String = ""
        Dim DelvAddAr() As String
        Dim DelInc As Integer = 0
        Dim L1 As Single

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        CurY = CurY + TxtHgt - 10
        If is_LastPage = True Then
            Common_Procedures.Print_To_PrintDocument(e, " TOTAL", LMargin + ClAr(1) + 30, CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Order_Meters").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(6) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))


        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 20
        W1 = e.Graphics.MeasureString("DISCOUNT    : ", pFont).Width


        CurY = CurY + TxtHgt - 10
        If Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString) <> 0 Then
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "Discount @ " & Trim(prn_HdDt.Rows(0).Item("Discount_Percentage").ToString) & "%", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Discount_Amount").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "Discount ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            End If
        End If


        Dim curY3 = 0F

        curY3 = CurY

        curY3 = curY3 + TxtHgt - 15
        L1 = e.Graphics.MeasureString("POCOMPLETION DATE:", pFont).Width

        Common_Procedures.Print_To_PrintDocument(e, "Po Completion Date", LMargin + 10, curY3, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " : ", LMargin + L1, curY3, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Po_Completion_Date").ToString, LMargin + L1 + 20, curY3, 0, 0, pFont)

        '-------- Packing

        Dim vPacking_Det1 = ""
        Dim vPacking_Det2 = ""

        vPacking_Det1 = prn_HdDt.Rows(0).Item("Packing").ToString
        vPacking_Det2 = ""

        If Len(vPacking_Det1) > 40 Then

            For i = 40 To 1 Step -1
                If Mid$(Trim(vPacking_Det1), i, 1) = " " Or Mid$(Trim(vPacking_Det1), i, 1) = "," Or Mid$(Trim(vPacking_Det1), i, 1) = "." Or Mid$(Trim(vPacking_Det1), i, 1) = "-" Or Mid$(Trim(vPacking_Det1), i, 1) = "/" Or Mid$(Trim(vPacking_Det1), i, 1) = "_" Or Mid$(Trim(vPacking_Det1), i, 1) = "(" Or Mid$(Trim(vPacking_Det1), i, 1) = ")" Or Mid$(Trim(vPacking_Det1), i, 1) = "\" Or Mid$(Trim(vPacking_Det1), i, 1) = "[" Or Mid$(Trim(vPacking_Det1), i, 1) = "]" Or Mid$(Trim(vPacking_Det1), i, 1) = "{" Or Mid$(Trim(vPacking_Det1), i, 1) = "}" Then Exit For
            Next i
            If i = 0 Then i = 40
            vPacking_Det2 = Microsoft.VisualBasic.Right(Trim(vPacking_Det1), Len(vPacking_Det1) - i)
            vPacking_Det1 = Microsoft.VisualBasic.Left(Trim(vPacking_Det1), i - 1)


        End If

        curY3 = curY3 + TxtHgt + 5

        Common_Procedures.Print_To_PrintDocument(e, "Packing", LMargin + 10, curY3, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " : ", LMargin + L1, curY3, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(vPacking_Det1), LMargin + L1 + 20, curY3, 0, 0, pFont)

        If Trim(vPacking_Det2) <> "" Then
            curY3 = curY3 + TxtHgt '+ 5
            Common_Procedures.Print_To_PrintDocument(e, Trim(vPacking_Det2), LMargin + L1 + 20, curY3, 0, 0, pFont)
        End If


        '-------- Roll_Length

        Dim vRoll_Length_Det1 = ""
        Dim vRoll_Length_Det2 = ""

        vRoll_Length_Det1 = prn_HdDt.Rows(0).Item("Roll_Length").ToString
        vRoll_Length_Det2 = ""

        If Len(vRoll_Length_Det1) > 40 Then

            For i = 40 To 1 Step -1
                If Mid$(Trim(vRoll_Length_Det1), i, 1) = " " Or Mid$(Trim(vRoll_Length_Det1), i, 1) = "," Or Mid$(Trim(vRoll_Length_Det1), i, 1) = "." Or Mid$(Trim(vRoll_Length_Det1), i, 1) = "-" Or Mid$(Trim(vRoll_Length_Det1), i, 1) = "/" Or Mid$(Trim(vRoll_Length_Det1), i, 1) = "_" Or Mid$(Trim(vRoll_Length_Det1), i, 1) = "(" Or Mid$(Trim(vRoll_Length_Det1), i, 1) = ")" Or Mid$(Trim(vRoll_Length_Det1), i, 1) = "\" Or Mid$(Trim(vRoll_Length_Det1), i, 1) = "[" Or Mid$(Trim(vRoll_Length_Det1), i, 1) = "]" Or Mid$(Trim(vRoll_Length_Det1), i, 1) = "{" Or Mid$(Trim(vRoll_Length_Det1), i, 1) = "}" Then Exit For
            Next i
            If i = 0 Then i = 40
            vRoll_Length_Det2 = Microsoft.VisualBasic.Right(Trim(vRoll_Length_Det1), Len(vRoll_Length_Det1) - i)
            vRoll_Length_Det1 = Microsoft.VisualBasic.Left(Trim(vRoll_Length_Det1), i - 1)


        End If

        curY3 = curY3 + TxtHgt + 5

        Common_Procedures.Print_To_PrintDocument(e, "Roll Length", LMargin + 10, curY3, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " : ", LMargin + L1, curY3, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(vRoll_Length_Det1), LMargin + L1 + 20, curY3, 0, 0, pFont)

        If Trim(vRoll_Length_Det2) <> "" Then
            curY3 = curY3 + TxtHgt '+ 5
            Common_Procedures.Print_To_PrintDocument(e, Trim(vRoll_Length_Det2), LMargin + L1 + 20, curY3, 0, 0, pFont)
        End If

        '-------- Quality_Standard

        Dim Quality_Standard_Det1 = ""
        Dim Quality_Standard_Det2 = ""

        Quality_Standard_Det1 = prn_HdDt.Rows(0).Item("Quality_Standard").ToString
        Quality_Standard_Det2 = ""

        If Len(Quality_Standard_Det1) > 40 Then

            For i = 40 To 1 Step -1
                If Mid$(Trim(Quality_Standard_Det1), i, 1) = " " Or Mid$(Trim(Quality_Standard_Det1), i, 1) = "," Or Mid$(Trim(Quality_Standard_Det1), i, 1) = "." Or Mid$(Trim(Quality_Standard_Det1), i, 1) = "-" Or Mid$(Trim(Quality_Standard_Det1), i, 1) = "/" Or Mid$(Trim(Quality_Standard_Det1), i, 1) = "_" Or Mid$(Trim(Quality_Standard_Det1), i, 1) = "(" Or Mid$(Trim(Quality_Standard_Det1), i, 1) = ")" Or Mid$(Trim(Quality_Standard_Det1), i, 1) = "\" Or Mid$(Trim(Quality_Standard_Det1), i, 1) = "[" Or Mid$(Trim(Quality_Standard_Det1), i, 1) = "]" Or Mid$(Trim(Quality_Standard_Det1), i, 1) = "{" Or Mid$(Trim(Quality_Standard_Det1), i, 1) = "}" Then Exit For
            Next i
            If i = 0 Then i = 40
            Quality_Standard_Det2 = Microsoft.VisualBasic.Right(Trim(Quality_Standard_Det1), Len(Quality_Standard_Det1) - i)
            Quality_Standard_Det1 = Microsoft.VisualBasic.Left(Trim(Quality_Standard_Det1), i - 1)


        End If

        curY3 = curY3 + TxtHgt + 5
        Common_Procedures.Print_To_PrintDocument(e, "Quality Standard", LMargin + 10, curY3, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " : ", LMargin + L1, curY3, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Quality_Standard_Det1).ToString, LMargin + L1 + 20, curY3, 0, 0, pFont)

        If Trim(Trim(Quality_Standard_Det2)) <> "" Then
            curY3 = curY3 + TxtHgt ' + 5
            Common_Procedures.Print_To_PrintDocument(e, Trim(Quality_Standard_Det2).ToString, LMargin + L1 + 20, curY3, 0, 0, pFont)
        End If


        curY3 = curY3 + TxtHgt + 5

        Common_Procedures.Print_To_PrintDocument(e, "Reed Count", LMargin + 10, curY3, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " : ", LMargin + L1, curY3, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Reed_Count").ToString, LMargin + L1 + 20, curY3, 0, 0, pFont)


        '-------- Slevedge_Details

        Dim vSlevedge_Det1 = ""
        Dim vSlevedge_Det2 = ""

        vSlevedge_Det1 = prn_HdDt.Rows(0).Item("Slevedge_Details").ToString
        vSlevedge_Det2 = ""

        If Len(vSlevedge_Det1) > 40 Then
            For i = 40 To 1 Step -1
                If Mid$(Trim(vSlevedge_Det1), i, 1) = " " Or Mid$(Trim(vSlevedge_Det1), i, 1) = "," Or Mid$(Trim(vSlevedge_Det1), i, 1) = "." Or Mid$(Trim(vSlevedge_Det1), i, 1) = "-" Or Mid$(Trim(vSlevedge_Det1), i, 1) = "/" Or Mid$(Trim(vSlevedge_Det1), i, 1) = "_" Or Mid$(Trim(vSlevedge_Det1), i, 1) = "(" Or Mid$(Trim(vSlevedge_Det1), i, 1) = ")" Or Mid$(Trim(vSlevedge_Det1), i, 1) = "\" Or Mid$(Trim(vSlevedge_Det1), i, 1) = "[" Or Mid$(Trim(vSlevedge_Det1), i, 1) = "]" Or Mid$(Trim(vSlevedge_Det1), i, 1) = "{" Or Mid$(Trim(vSlevedge_Det1), i, 1) = "}" Then Exit For
            Next i
            If i = 0 Then i = 40
            vSlevedge_Det2 = Microsoft.VisualBasic.Right(Trim(vSlevedge_Det1), Len(vSlevedge_Det1) - i)
            vSlevedge_Det1 = Microsoft.VisualBasic.Left(Trim(vSlevedge_Det1), i - 1)

        End If

        curY3 = curY3 + TxtHgt + 5
        Common_Procedures.Print_To_PrintDocument(e, "Selvedge", LMargin + 10, curY3, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " : ", LMargin + L1, curY3, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(vSlevedge_Det1), LMargin + L1 + 20, curY3, 0, 0, pFont)

        If Trim(Trim(vSlevedge_Det2)) <> "" Then
            curY3 = curY3 + TxtHgt '+ 5
            Common_Procedures.Print_To_PrintDocument(e, Trim(vSlevedge_Det2).ToString, LMargin + L1 + 20, curY3, 0, 0, pFont)
        End If


        curY3 = curY3 + TxtHgt + 5
        Common_Procedures.Print_To_PrintDocument(e, "Quantity Tolerance", LMargin + 10, curY3, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, " : ", LMargin + L1, curY3, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Quantity_Tolerance").ToString, LMargin + L1 + 20, curY3, 0, 0, pFont)




        'If Trim(prn_HdDt.Rows(0).Item("Delivery_Address1").ToString) <> "" Then


        '    p1Font = New Font("Calibri", 10, FontStyle.Bold Or FontStyle.Underline)
        '    Common_Procedures.Print_To_PrintDocument(e, "DELIVERY ADDRESS : ", LMargin + 10, CurY, 0, 0, p1Font)

        '    'Erase DelvAddAr
        '    'If Trim(prn_HdDt.Rows(0).Item("Delivery_Address").ToString) <> "" Then
        '    '    DelvAddAr = Split(Trim(prn_HdDt.Rows(0).Item("Delivery_Address").ToString), ",")
        '    '    DelInc = -1

        '    '    DelInc = DelInc + 1
        '    '    If UBound(DelvAddAr) >= DelInc Then
        '    '        ItmNm1 = Trim(DelvAddAr(DelInc))
        '    '    End If

        '    '    DelInc = DelInc + 1
        '    '    If UBound(DelvAddAr) >= DelInc Then
        '    '        ItmNm2 = Trim(DelvAddAr(DelInc))
        '    '    End If

        '    '    DelInc = DelInc + 1
        '    '    If UBound(DelvAddAr) >= DelInc Then
        '    '        ItmNm3 = Trim(DelvAddAr(DelInc))
        '    '    End If

        '    '    DelInc = DelInc + 1
        '    '    If UBound(DelvAddAr) >= DelInc Then
        '    '        ItmNm4 = Trim(DelvAddAr(DelInc))
        '    '    End If

        '    '    DelInc = DelInc + 1
        '    '    If UBound(DelvAddAr) >= DelInc Then
        '    '        ItmNm5 = Trim(DelvAddAr(DelInc))
        '    '    End If

        '    '    DelInc = DelInc + 1
        '    '    If UBound(DelvAddAr) >= DelInc Then
        '    '        ItmNm6 = Trim(DelvAddAr(DelInc))
        '    '    End If

        '    '    DelInc = DelInc + 1
        '    '    If UBound(DelvAddAr) >= DelInc Then
        '    '        ItmNm7 = Trim(DelvAddAr(DelInc))
        '    '    End If

        '    '    DelInc = DelInc + 1
        '    '    If UBound(DelvAddAr) >= DelInc Then
        '    '        ItmNm8 = Trim(DelvAddAr(DelInc))
        '    '    End If

        '    '    DelInc = DelInc + 1
        '    '    If UBound(DelvAddAr) >= DelInc Then
        '    '        ItmNm9 = Trim(DelvAddAr(DelInc))
        '    '    End If
        '    '    DelInc = DelInc + 1
        '    '    If UBound(DelvAddAr) >= DelInc Then
        '    '        ItmNm10 = Trim(DelvAddAr(DelInc))
        '    '    End If
        '    'End If

        '    p1Font = New Font("Calibri", 10, FontStyle.Bold)

        '    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Delivery_Address1").ToString), LMargin + 10, CurY + TxtHgt, 0, 0, p1Font)
        '    Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Delivery_Address2").ToString), LMargin + 10, CurY + TxtHgt + TxtHgt, 0, 0, p1Font)
        'End If
        '   p1Font = New Font("Calibri", 10, FontStyle.Bold)

        'Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm3, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm4, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm5, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm6, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm7, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm8, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm9, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm10, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)


        ' Common_Procedures.Print_To_PrintDocument(e, " Payment Terms  :  " & prn_HdDt.Rows(0).Item("Gr_time").ToString & " Days", LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm8, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
        ' Common_Procedures.Print_To_PrintDocument(e, " Delivery Date : " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, " " & ItmNm10, LMargin + 10, CurY + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt + TxtHgt, 0, 0, p1Font)


        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Due_Days").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)


        'Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        If Val(prn_HdDt.Rows(0).Item("AddLess_BeforeTax_Amount").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "AddLess Before Tax", LMargin + C1 + 10, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("AddLess_BeforeTax_Amount").ToString), "########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            End If
        End If




        'Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("DelAdd1").ToString, LMargin + 10, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        If Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "Freight", LMargin + C1 + 10, CurY, 0, 0, pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Freight_Amount").ToString), "########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            End If
        End If



        'Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("DelAdd2").ToString, LMargin + 10, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt
        If Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "AddLess After Tax", LMargin + C1 + 10, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("AddLess_Amount").ToString), "#######0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            End If
        End If




        'Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("DelAdd3").ToString, LMargin + 10, CurY, 0, 0, p1Font)
        'Common_Procedures.Print_To_PrintDocument(e, "  " & prn_HdDt.Rows(0).Item("DelAdd4").ToString, LMargin + 10, CurY + TxtHgt, 0, 0, p1Font)



        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Total  Before Tax  ", LMargin + C1 + 10, CurY, 0, 0, pFont)
        If is_LastPage = True Then
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
        End If



        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)
        CurY = CurY + TxtHgt - 10


        vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)

        If Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Total_CGST_Amount").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            End If

        Else
            Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)

        End If



        CurY = CurY + TxtHgt

        If Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Total_SGST_Amount").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            End If

        Else
            Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
        End If


        CurY = CurY + TxtHgt
        If Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ " & Format(Val(vTaxPerc), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Total_IGST_Amount").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            End If

        Else
            Common_Procedures.Print_To_PrintDocument(e, "Add : IGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)
        End If



        CurY = CurY + TxtHgt

        If Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString) <> 0 Then
            Common_Procedures.Print_To_PrintDocument(e, "RoundOff ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("RoundOff_Amount").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            End If

        End If


        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
        LnAr(8) = CurY

        CurY = CurY + TxtHgt - 10
        If curY3 > CurY Then
            CurY = curY3
        End If
        'Common_Procedures.Print_To_PrintDocument(e, "E & O.E", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "GRAND TOTAL", LMargin + C1 + 10, CurY, 0, 0, pFont)
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Net_Amount").ToString), PageWidth - 5, CurY, 1, 0, p1Font)

        'CurY = CurY + TxtHgt + 10
        '' e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, PageWidth, CurY)
        'LnAr(8) = CurY

        'CurY = CurY + TxtHgt - 10
        'Common_Procedures.Print_To_PrintDocument(e, "GST on Reverse Charge", LMargin + C1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "##########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(9) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
        CurY = CurY + TxtHgt - 15
        BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Net_Amount").ToString))
        BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "", "")

        Common_Procedures.Print_To_PrintDocument(e, "Amount In Words   :  " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        If Trim(prn_HdDt.Rows(0).Item("Note").ToString) <> "" Then
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "Remarks :  " & Trim(prn_HdDt.Rows(0).Item("Note").ToString), LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        End If


        CurY = CurY + 3
        p1Font = New Font("Calibri", 12, FontStyle.Underline)
        Common_Procedures.Print_To_PrintDocument(e, "Term & Conditions : ", LMargin + 10, CurY, 0, 0, p1Font)

        If Trim(prn_HdDt.Rows(0).Item("Terms_And_Condition_1").ToString) <> "" Then
            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "1. " & Trim(prn_HdDt.Rows(0).Item("Terms_And_Condition_1").ToString), LMargin + 10, CurY, 0, 0, pFont)
        End If
        If Trim(prn_HdDt.Rows(0).Item("Terms_And_Condition_2").ToString) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "2. " & Trim(prn_HdDt.Rows(0).Item("Terms_And_Condition_2").ToString), LMargin + 10, CurY, 0, 0, pFont)
        End If

        If Trim(prn_HdDt.Rows(0).Item("Terms_And_Condition_3").ToString) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "3. " & Trim(prn_HdDt.Rows(0).Item("Terms_And_Condition_3").ToString), LMargin + 10, CurY, 0, 0, pFont)
        End If

        If Trim(prn_HdDt.Rows(0).Item("Terms_And_Condition_4").ToString) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "4. " & Trim(prn_HdDt.Rows(0).Item("Terms_And_Condition_4").ToString), LMargin + 10, CurY, 0, 0, pFont)
        End If

        If Trim(prn_HdDt.Rows(0).Item("Terms_And_Condition_5").ToString) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "5. " & Trim(prn_HdDt.Rows(0).Item("Terms_And_Condition_5").ToString), LMargin + 10, CurY, 0, 0, pFont)
        End If

        If Trim(prn_HdDt.Rows(0).Item("Terms_And_Condition_6").ToString) <> "" Then
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "6. " & Trim(prn_HdDt.Rows(0).Item("Terms_And_Condition_6").ToString), LMargin + 10, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(10) = CurY

        'CurY = CurY + TxtHgt
        'If Val(Common_Procedures.User.IdNo) <> 1 Then
        '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
        'End If

        CurY = CurY + TxtHgt - 10
        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        If Val(Common_Procedures.User.IdNo) <> 1 Then
            Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 1, 0, pFont)
        End If
        CurY = CurY + TxtHgt + 10
        Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 20, CurY, 1, 0, pFont)



        Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)



    End Sub

    Private Sub Btn_Additional_Details_Click(sender As Object, e As EventArgs) Handles Btn_Additional_Details.Click
        pnl_Additional_Details.Visible = True
        pnl_Additional_Details.BringToFront()
        pnl_Back.Enabled = False
        txt_Payment_Terms_Details.Focus()

        pnl_Additional_Details.Left = (Me.Width - pnl_Additional_Details.Width) \ 2
        pnl_Additional_Details.Top = (Me.Height - pnl_Additional_Details.Height) \ 2
        pnl_Additional_Details.BringToFront()

    End Sub

    Private Sub btn_Terms_Conditions_Click(sender As Object, e As EventArgs) Handles btn_Terms_Conditions.Click
        Pnl_Terms_and_Condition.Visible = True
        Pnl_Terms_and_Condition.BringToFront()
        pnl_Additional_Details.Enabled = False
        txt_TermsCond_1.Focus()

        Pnl_Terms_and_Condition.Left = (Me.Width - Pnl_Terms_and_Condition.Width) \ 2
        Pnl_Terms_and_Condition.Top = (Me.Height - Pnl_Terms_and_Condition.Height) \ 2
        Pnl_Terms_and_Condition.BringToFront()
    End Sub

    Private Sub btn_Additional_Details_Close_Click(sender As Object, e As EventArgs) Handles btn_Additional_Details_Close.Click
        pnl_Additional_Details.Visible = False
        pnl_Back.Enabled = True
        cbo_PartyName.Focus()
    End Sub

    Private Sub btn_pnl_Terms_Condition_Close_Click(sender As Object, e As EventArgs) Handles btn_pnl_Terms_Condition_Close.Click
        Pnl_Terms_and_Condition.Visible = False
        pnl_Additional_Details.Enabled = True
        txt_Payment_Terms_Details.Focus()
    End Sub
    Private Sub txt_Qty_Tolenrance_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Qty_Tolenrance.KeyDown
        If e.KeyCode = 38 Then
            txt_Slevedge_Details.Focus()
        ElseIf e.KeyCode = 40 Then
            btn_Terms_Conditions.Focus()

        End If
    End Sub
    Private Sub txt_Qty_Tolenrance_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Qty_Tolenrance.KeyPress
        If Asc(e.KeyChar) = 13 Then
            btn_Terms_Conditions.Focus()
        End If
    End Sub

    Private Sub txt_TermsCond_6_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_TermsCond_6.KeyDown
        If e.KeyCode = 38 Then
            txt_TermsCond_5.Focus()
        ElseIf e.KeyCode = 40 Then
            btn_pnl_Terms_Condition_Close.Focus()

        End If
    End Sub

    Private Sub txt_TermsCond_6_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_TermsCond_6.KeyPress
        If Asc(e.KeyChar) = 13 Then
            btn_pnl_Terms_Condition_Close.Focus()
        End If
    End Sub

    Private Sub txt_TermsCond_1_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_TermsCond_1.KeyDown
        If e.KeyCode = 38 Then
            btn_pnl_Terms_Condition_Close.Focus()
        ElseIf e.KeyCode = 40 Then
            txt_TermsCond_2.Focus()

        End If
    End Sub

    Private Sub txt_Payment_Terms_Details_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Payment_Terms_Details.KeyDown
        If e.KeyCode = 38 Then
            btn_Additional_Details_Close.Focus()
        ElseIf e.KeyCode = 40 Then
            txt_Delivery_Terms.Focus()
        End If
    End Sub
End Class