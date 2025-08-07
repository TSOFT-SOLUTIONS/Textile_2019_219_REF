Imports System.Drawing.Printing

Public Class Weaver_Wages_ClothReceipt
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Filter_RowNo As Integer = -1
    Private Pk_Condition As String = "GWEWA-"
    Private PkCondition_WPYMT As String = "GWPAM-"
    Private PkCondition_WCLRC As String = "WCREC-"
    Private PkCondition_WFRGT As String = "GWFRE-"
    Private Pk_Condition2 As String = "GWEWL-"
    Private PkCondition_WPTDS As String = "GWATS-"

    Private PkCondition_WADVP As String = "GWEDP-"
    Private PkCondition_WADVD As String = "GWEDU-"
    Private NoCalc_Status As Boolean = False
    Private dgv_ActCtrlName As String = ""
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String
    Private prn_InpOpts As String = ""
    Private Gst_Status As Integer = 0
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_WagesDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_ReceiptDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_BobinDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_KuriDetails As New DataGridViewTextBoxEditingControl
    ' Private prn_InpOpts As String = ""
    Private prn_OriDupTri As String = ""
    Private prn_Count As Integer
    Private prn_Count1 As Integer
    Private prn_Cooly As New DataTable
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_DetDt1 As New DataTable
    Private prn_PageNo As Integer
    Private prn_HeadIndx As Integer
    Private prn_DetIndx As Integer
    Private prn_DetIndx1 As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private prn_FromNo As String
    Private prn_ToNo As String
    Private prn_PageCount As Integer = 0
    Private cnt As Integer = 0
    Private PrntCnt2ndPageSTS As Boolean = False
    Private prn_Prev_HeadIndx As Integer

    Private NoFo_STS As Integer = 0
    Private prn_Tot_EBeam_Stk As Single = 0
    Private prn_Tot_Pavu_Stk As Single = 0
    Private prn_Tot_Yarn_Stk As Single = 0
    Private prn_Tot_Amt_Bal As Single = 0
    Private prn_WagesFrmt As String = ""
    Private prn_Frieght_Sts As Boolean = False

    Private yarnstk, pavstk As Single
    Private yarnnm, pavnm As String
    Private Weight1, Weight2, Currency1, Currency2, Currency3, Currency4, Currency5, Currency6, Currency7, Currency8 As Single

    Private SaveAll_STS As Boolean = False
    Private LastNo As String = ""
    Private Party_DC_No_For_Wages As String = ""
    Private Party_DC_Date_For_Wages As String = ""
    Private WeaverClothType(10) As String
    Private WeaverClothMeters(10) As String
    Private WeaverClothCooly(10) As String
    Private WeaverClothAmount(10) As String

    Private DeliveryMeters(10) As String
    Private DcNo(10) As String
    Private DeliveryDate(10) As String
    Private DeliveryPcs(10) As String

    Private DeliverySend As Single = 0
    Private DeliverySnd As Single = 0
    Private DeliveryBits As Single = 0
    Private DeliveryRjts As Single = 0
    Private DeliveryOthrs As Single = 0
    Private DeliveryMtrs As Single = 0

    Private Fold As Single = 0
    Private vprn_Tot_Sound_Mtr As String = ""
    Private vprn_Tot_Consum_thiri As String = ""
    Private vprn_Tot_Excess_shrt_Mtr As String = ""
    Private vprn_Tot_Damage_Pcs As String = ""
    Private vprn_Tot_Amount As String = ""
    Private vprn_Tot_Damage_Amt As String = ""

    Private prn_DmgAmt_STS As Boolean = False

    Public Sub New()
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_BobinSelection_ToolTip.Visible = False
        pnl_KuriSelection_ToolTip.Visible = False
        pnl_PrintOption2.Visible = False
        pnl_PrintRange.Visible = False
        chk_Tds.Checked = False
        lbl_BillNo.Text = ""
        lbl_BillNo.ForeColor = Color.Black

        lbl_Total_Amount.Text = ""

        chk_Printed.Checked = False
        chk_Printed.Enabled = False
        chk_Printed.Visible = False

        dtp_Date.Text = ""
        cbo_Weaver.Text = ""
        cbo_Grid_Count.Text = ""
        cbo_cloth.Text = ""
        txt_Add_Amount.Text = ""
        cbo_WidthType.Text = ""
        txt_No_Of_Beams.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))

        txt_Folding_Less.Text = ""
        txt_Freight_Charge.Text = ""
        txt_Less_Amount.Text = ""
        lbl_Net_Amount.Text = ""
        txt_Paid_Amount.Text = ""
        lbl_RecCode.Text = ""
        txt_Tds.Text = "2"
        txt_Tds_Amount.Text = ""
        lbl_Cooly_amt.Text = ""
        txt_CGST_Percentage.Text = "2.5"
        txt_SGST_Percentage.Text = "2.5"
        lbl_CGST_Amount.Text = ""
        lbl_SGST_Amount.Text = ""
        lbl_Taxable_Value.Text = ""
        txt_PartyDcNo.Text = ""
        cbo_LoomType.Text = "POWER LOOM"
        If Trim(cbo_LoomType.Text) = "POWER LOOM" Then
            txt_No_Of_Beams.Text = "1"
        Else
            txt_No_Of_Beams.Text = "2"
        End If

        chk_TaxABLEAmount_RoundOff_STS.Checked = True
        chk_TaxAmount_RoundOff_STS.Checked = False

        dgv_ConsYarn_Details.Rows.Clear()
        dgv_ConsYarnDetails_Total.Rows.Clear()
        dgv_ConsYarnDetails_Total.Rows.Add()

        dgv_BobinDetails.Rows.Clear()
        dgv_BobinelectionDetails.Rows.Clear()
        dgv_BobinDetails_Total.Rows.Clear()
        dgv_BobinDetails_Total.Rows.Add()

        dgv_KuriDetails.Rows.Clear()
        dgv_KuriSelection_Details.Rows.Clear()
        dgv_KuriDetails_Total.Rows.Clear()
        dgv_KuriDetails_Total.Rows.Add()

        dgv_Receipt_Details.Rows.Clear()
        dgv_ReceiptDetails_Total.Rows.Clear()
        dgv_ReceiptDetails_Total.Rows.Add()

        txt_PrintRange_FromNo.Text = ""
        txt_PrintRange_ToNo.Text = ""

        dgv_ActCtrlName = ""

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_CountName.Text = ""
            cbo_Filter_CountName.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        Grid_Cell_DeSelect()
        NoCalc_Status = False
        cbo_Grid_Count.Visible = False
        cbo_cloth.Visible = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        If FrmLdSTS = True Then Exit Sub
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

        If Me.ActiveControl.Name <> cbo_Grid_Count.Name Then
            cbo_Grid_Count.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_cloth.Name Then
            cbo_cloth.Visible = False
        End If


        If Me.ActiveControl.Name <> dgv_ConsYarn_Details.Name Then
            Grid_DeSelect()
        End If
        If Me.ActiveControl.Name <> dgv_Receipt_Details.Name And Not (TypeOf ActiveControl Is DataGridViewTextBoxEditingControl) Then
            pnl_BobinSelection_ToolTip.Visible = False
        End If
        If Me.ActiveControl.Name <> dgv_Receipt_Details.Name And Not (TypeOf ActiveControl Is DataGridViewTextBoxEditingControl) Then
            pnl_KuriSelection_ToolTip.Visible = False
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next
        If FrmLdSTS = True Then Exit Sub
        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.FromArgb(2, 57, 111)
                Prec_ActCtrl.ForeColor = Color.White
            End If
        End If

    End Sub
    Private Sub ControlLostFocus1(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next
        If FrmLdSTS = True Then Exit Sub
        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.LightBlue
                Prec_ActCtrl.ForeColor = Color.Blue
            End If
        End If

    End Sub

    Private Sub Grid_DeSelect()

        On Error Resume Next

        If Not IsNothing(dgv_ConsYarn_Details.CurrentCell) Then dgv_ConsYarn_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_ConsYarnDetails_Total.CurrentCell) Then dgv_ConsYarnDetails_Total.CurrentCell.Selected = False

        If Not IsNothing(dgv_Receipt_Details.CurrentCell) Then dgv_Receipt_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_ReceiptDetails_Total.CurrentCell) Then dgv_ReceiptDetails_Total.CurrentCell.Selected = False

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
        If FrmLdSTS = True Then Exit Sub

        If Not IsNothing(dgv_ConsYarn_Details.CurrentCell) Then dgv_ConsYarn_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_ConsYarnDetails_Total.CurrentCell) Then dgv_ConsYarnDetails_Total.CurrentCell.Selected = False

        If Not IsNothing(dgv_Receipt_Details.CurrentCell) Then dgv_Receipt_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_ReceiptDetails_Total.CurrentCell) Then dgv_ReceiptDetails_Total.CurrentCell.Selected = False

        If Not IsNothing(dgv_BobinDetails.CurrentCell) Then dgv_BobinDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_KuriDetails.CurrentCell) Then dgv_KuriDetails.CurrentCell.Selected = False


    End Sub

    Private Sub Weaver_Wages_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Weaver.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Weaver.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            ' If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(lbl_Cloth.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            '   lbl_Cloth.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            ' End If
            ' If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(lbl_Ends_Count.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ENDS COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
            '     lbl_Ends_Count.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            ' End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Grid_Count.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "COUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Grid_Count.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Weaver_Wages_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Weaver_Wages_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try

            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub



                ElseIf pnl_PrintOption2.Visible = True Then
                    btn_Close_PrintOption_Click(sender, e)
                    Exit Sub

                ElseIf pnl_PrintRange.Visible = True Then
                    btn_Close_PrintRange_Click(sender, e)
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

    Private Sub Weaver_Wages_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Dim dt7 As New DataTable
        Dim dt8 As New DataTable

        'lbl_LotNoHeading.Text = StrConv(Common_Procedures.settings.ClothReceipt_LotNo_OR_RollNo_Text, vbProperCase)
        'lbl_ClothType1_Heading.Text = Trim(UCase(Common_Procedures.ClothType.Type1))
        'lbl_ClothType2_Heading.Text = Trim(UCase(Common_Procedures.ClothType.Type2))
        'lbl_ClothType3_Heading.Text = Trim(UCase(Common_Procedures.ClothType.Type3))
        'lbl_ClothType4_Heading.Text = Trim(UCase(Common_Procedures.ClothType.Type4))
        'lbl_ClothType5_Heading.Text = Trim(UCase(Common_Procedures.ClothType.Type5))

        Me.Text = ""

        con.Open()

        Common_Procedures.get_CashPartyName_From_All_Entries(con)

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead Where (Ledger_IdNo = 0 OR ledger_type = 'WEAVER' or Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1) and Close_status = 0 order by Ledger_DisplayName", con)
        da.Fill(dt1)
        cbo_Weaver.DataSource = dt1
        cbo_Weaver.DisplayMember = "Ledger_DisplayName"

        'da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
        'da.Fill(dt2)
        'lbl_Cloth.DataSource = dt2
        'lbl_Cloth.DisplayMember = "Cloth_Name"

        'da = New SqlClient.SqlDataAdapter("select EndsCount_Name from EndsCount_Head order by EndsCount_Name", con)
        'da.Fill(dt3)
        'lbl_Ends_Count.DataSource = dt3
        'lbl_Ends_Count.DisplayMember = "EndsCount_Name"
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1235" Then
            btn_SaveAll.Visible = True
        End If
        da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head order by Count_Name", con)
        da.Fill(dt4)
        cbo_Grid_Count.DataSource = dt4
        cbo_Grid_Count.DisplayMember = "Cloth_Name"
        cbo_LoomType.Items.Clear()
        cbo_LoomType.Items.Add("")
        cbo_LoomType.Items.Add("POWER LOOM")
        cbo_LoomType.Items.Add("AUTO LOOM")

        cbo_WidthType.Items.Clear()
        cbo_WidthType.Items.Add("")
        cbo_WidthType.Items.Add("SINGLE")
        cbo_WidthType.Items.Add("DOUBLE")
        cbo_WidthType.Items.Add("TRIPLE")
        cbo_WidthType.Items.Add("FOURTH")

        chk_Printed.Enabled = False
        If Val(Common_Procedures.User.IdNo) = 1 Then
            chk_Printed.Enabled = True
        End If

        pnl_Bobin_Details.Visible = False
        pnl_Bobin_Details.Left = (Me.Width - pnl_Bobin_Details.Width) \ 2
        pnl_Bobin_Details.Top = (Me.Height - pnl_Bobin_Details.Height) \ 2
        pnl_Bobin_Details.BringToFront()

        dgv_BobinelectionDetails.Visible = False

        pnl_BobinSelection_ToolTip.Visible = False

        pnl_Kuri_Details.Visible = False
        pnl_Kuri_Details.Left = (Me.Width - pnl_Kuri_Details.Width) \ 2
        pnl_Kuri_Details.Top = (Me.Height - pnl_Kuri_Details.Height) \ 2
        pnl_Kuri_Details.BringToFront()

        dgv_KuriSelection_Details.Visible = False

        pnl_KuriSelection_ToolTip.Visible = False

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        pnl_PrintOption2.Visible = False
        pnl_PrintOption2.BringToFront()
        pnl_PrintOption2.Left = (Me.Width - pnl_PrintOption2.Width) \ 2
        pnl_PrintOption2.Top = (Me.Height - pnl_PrintOption2.Height) \ 2



        pnl_PrintRange.Visible = False
        pnl_PrintRange.Left = (Me.Width - pnl_PrintRange.Width) \ 2
        pnl_PrintRange.Top = (Me.Height - pnl_PrintRange.Height) \ 2
        pnl_PrintRange.BringToFront()

        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Weaver.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_WidthType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Count.GotFocus, AddressOf ControlGotFocus


        AddHandler txt_Add_Amount.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_LoomType.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Folding_Less.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight_Charge.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Less_Amount.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_Net_Amount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PartyDcNo.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_cloth.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Paid_Amount.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Tds.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Tds_Amount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Remarks.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_No_Of_Beams.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CGST_Percentage.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SGST_Percentage.GotFocus, AddressOf ControlGotFocus


        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_CountName.GotFocus, AddressOf ControlGotFocus


        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Filter_Show.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print.GotFocus, AddressOf ControlGotFocus
        'AddHandler btn_Print_Cancel.GotFocus, AddressOf ControlGotFocus
        'AddHandler btn_Print_Simple_WithName.GotFocus, AddressOf ControlGotFocus
        'AddHandler btn_Print_Simple_WithOutName.GotFocus, AddressOf ControlGotFocus
        'AddHandler btn_Print_WithStock_WithName.GotFocus, AddressOf ControlGotFocus
        'AddHandler btn_Print_WithStock_WithoutName.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_PrintRange_FromNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PrintRange_ToNo.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_PrintRange.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Cancel_PrintRange.GotFocus, AddressOf ControlGotFocus


        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Weaver.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Count.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_WidthType.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Add_Amount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Remarks.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Folding_Less.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight_Charge.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Less_Amount.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_Net_Amount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CGST_Percentage.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SGST_Percentage.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_PartyDcNo.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_cloth.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Paid_Amount.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_LoomType.LostFocus, AddressOf ControlLostFocus


        AddHandler txt_Tds.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Tds_Amount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_No_Of_Beams.LostFocus, AddressOf ControlLostFocus
        'AddHandler lbl_Total_Meter.LostFocus, AddressOf ControlLostFocus


        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_CountName.LostFocus, AddressOf ControlLostFocus

        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Filter_Show.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print.LostFocus, AddressOf ControlLostFocus
        'AddHandler btn_Print_Cancel.LostFocus, AddressOf ControlLostFocus
        'AddHandler btn_Print_Simple_WithName.LostFocus, AddressOf ControlLostFocus
        'AddHandler btn_Print_Simple_WithOutName.LostFocus, AddressOf ControlLostFocus
        'AddHandler btn_Print_WithStock_WithName.LostFocus, AddressOf ControlLostFocus
        'AddHandler btn_Print_WithStock_WithoutName.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_PrintRange_FromNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PrintRange_ToNo.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_PrintRange.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Cancel_PrintRange.LostFocus, AddressOf ControlLostFocus

        'AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Add_Amount.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_Bits_Cooly.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_Bits_Meter.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Freight_Charge.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Folding_Less.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Less_Amount.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Tds.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CGST_Percentage.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_SGST_Percentage.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Tds_Amount.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler lbl_Total_Meter.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_PrintRange_FromNo.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Add_Amount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Tds_Amount.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_Bits_Meter.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Folding_Less.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Freight_Charge.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Less_Amount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CGST_Percentage.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_SGST_Percentage.KeyPress, AddressOf TextBoxControlKeyPress



        'AddHandler txt_Other_Meter.KeyPress, AddressOf TextBoxControlKeyPress

        'AddHandler txt_Pcs.KeyPress, AddressOf TextBoxControlKeyPress

        'AddHandler lbl_PDcNo.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler lbl_Rec_Date.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler lbl_RecNo.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_Rec_Meter.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_Reject_Cooly.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_Reject_Meter.KeyPress, AddressOf TextBoxControlKeyPress

        'AddHandler txt_Seconds_Cooly.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_Seconds_Meter.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_Sound_Cooly.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_Sound_Meter.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Tds.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler lbl_Total_Cooly.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler lbl_Total_Meter.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_PrintRange_FromNo.KeyPress, AddressOf TextBoxControlKeyPress

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

        If ActiveControl.Name = dgv_Receipt_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_Receipt_Details.Name Then
                dgv1 = dgv_Receipt_Details

            ElseIf dgv_Receipt_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Receipt_Details
            ElseIf pnl_Back.Enabled = True Then
                dgv1 = dgv_Receipt_Details


            End If

            If IsNothing(dgv1) = False Then

                With dgv1


                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= 18 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                txt_CGST_Percentage.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If
                            'ElseIf .CurrentCell.ColumnIndex = 2 Then
                            '    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(4)
                        ElseIf .CurrentCell.ColumnIndex = 4 Then
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(6)
                        ElseIf .CurrentCell.ColumnIndex = 6 Then
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(9)
                        ElseIf .CurrentCell.ColumnIndex = 9 Then
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(11)
                        ElseIf .CurrentCell.ColumnIndex = 11 Then
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(12)
                        ElseIf .CurrentCell.ColumnIndex = 12 Then
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(13)
                        ElseIf .CurrentCell.ColumnIndex = 13 Then
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(18)
                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                cbo_LoomType.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(18)

                            End If
                            'ElseIf .CurrentCell.ColumnIndex = 4 Then
                            '    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(2)
                        ElseIf .CurrentCell.ColumnIndex = 6 Then
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(4)
                        ElseIf .CurrentCell.ColumnIndex = 9 Then
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(6)
                        ElseIf .CurrentCell.ColumnIndex = 11 Then
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(9)
                        ElseIf .CurrentCell.ColumnIndex = 12 Then
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(10)
                        ElseIf .CurrentCell.ColumnIndex = 13 Then
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(12)
                        ElseIf .CurrentCell.ColumnIndex = 18 Then
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(13)
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
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer
        Dim SNo1 As Integer
        Dim SNo2 As Integer
        If Val(no) = 0 Then Exit Sub

        clear()

        NoCalc_Status = True

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from Weaver_Wages_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Wages_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_BillNo.Text = dt1.Rows(0).Item("Weaver_Wages_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Weaver_Wages_Date").ToString

                cbo_Weaver.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))


                txt_Add_Amount.Text = dt1.Rows(0).Item("Add_Amount").ToString
                txt_Folding_Less.Text = dt1.Rows(0).Item("Folding_Less").ToString

                txt_Freight_Charge.Text = dt1.Rows(0).Item("Freight_Charge").ToString
                txt_Less_Amount.Text = dt1.Rows(0).Item("Less_Amount").ToString
                lbl_Net_Amount.Text = dt1.Rows(0).Item("Net_Amount").ToString
                'txt_Other_Cooly.Text = dt1.Rows(0).Item("Others_Cooly").ToString
                'txt_Other_Meter.Text = dt1.Rows(0).Item("Others_Meters").ToString
                txt_Paid_Amount.Text = dt1.Rows(0).Item("Paid_Amount").ToString
                lbl_RecCode.Text = dt1.Rows(0).Item("Weaver_Cloth_Receipt_Code").ToString

                txt_Tds.Text = dt1.Rows(0).Item("Tds_Perc").ToString
                txt_Tds_Amount.Text = dt1.Rows(0).Item("Tds_Perc_Calc").ToString
                lbl_Total_Amount.Text = dt1.Rows(0).Item("Assesable_Value").ToString


                txt_CGST_Percentage.Text = dt1.Rows(0).Item("CGST_Percentage").ToString
                lbl_CGST_Amount.Text = dt1.Rows(0).Item("CGST_Amount").ToString
                txt_SGST_Percentage.Text = dt1.Rows(0).Item("SGST_Percentage").ToString
                lbl_SGST_Amount.Text = dt1.Rows(0).Item("SGST_Amount").ToString






                chk_Printed.Checked = False
                chk_Printed.Enabled = False
                chk_Printed.Visible = False
                If Val(dt1.Rows(0).Item("PrintOut_Status").ToString) = 1 Then
                    chk_Printed.Checked = True
                    chk_Printed.Visible = True
                    If Val(Common_Procedures.User.IdNo) = 1 Then
                        chk_Printed.Enabled = True
                    End If
                End If

                cbo_LoomType.Text = dt1.Rows(0).Item("Loom_Type").ToString
                txt_PartyDcNo.Text = dt1.Rows(0).Item("P_Dc_No").ToString
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))
                txt_Remarks.Text = dt1.Rows(0).Item("Remarks").ToString
                cbo_WidthType.Text = dt1.Rows(0).Item("Width_Type").ToString
                txt_No_Of_Beams.Text = dt1.Rows(0).Item("Noof_Beam").ToString
                chk_Tds.Checked = False
                If IsDBNull(dt1.Rows(0).Item("Tds_Status").ToString) = False Then
                    If Val(dt1.Rows(0).Item("Tds_Status").ToString) = 1 Then chk_Tds.Checked = True Else chk_Tds.Checked = False
                End If

                chk_TaxABLEAmount_RoundOff_STS.Checked = False
                If IsDBNull(dt1.Rows(0).Item("TaxableAmount_RoundOff_Status").ToString) = False Then
                    If Val(dt1.Rows(0).Item("TaxableAmount_RoundOff_Status").ToString) = 1 Then chk_TaxABLEAmount_RoundOff_STS.Checked = True Else chk_TaxABLEAmount_RoundOff_STS.Checked = False
                End If

                chk_TaxAmount_RoundOff_STS.Checked = False
                If IsDBNull(dt1.Rows(0).Item("TaxAmount_RoundOff_Status").ToString) = False Then
                    If Val(dt1.Rows(0).Item("TaxAmount_RoundOff_Status").ToString) = 1 Then chk_TaxAmount_RoundOff_STS.Checked = True Else chk_TaxAmount_RoundOff_STS.Checked = False
                End If


                da2 = New SqlClient.SqlDataAdapter("Select a.* , b.Count_Name from Weaver_Wages_Yarn_Details a left outer join count_head b on a.Count_IdNo = b.Count_IdNo Where a.Weaver_Wages_Code = '" & Trim(NewCode) & "' Order by Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_ConsYarn_Details

                    .Rows.Clear()
                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1
                            SNo = SNo + 1
                            n = .Rows.Add()

                            .Rows(n).Cells(0).Value = Val(SNo)
                            .Rows(n).Cells(1).Value = Common_Procedures.Cloth_IdNoToName(con, Val(dt2.Rows(i).Item("Cloth_IdNo").ToString))
                            .Rows(n).Cells(2).Value = (dt2.Rows(i).Item("Pick").ToString)
                            .Rows(n).Cells(3).Value = (dt2.Rows(i).Item("Width").ToString)
                            .Rows(n).Cells(4).Value = Common_Procedures.EndsCount_IdNoToName(con, Val(dt2.Rows(i).Item("EndsCount_IdNo").ToString))
                            .Rows(n).Cells(5).Value = (dt2.Rows(i).Item("Count_Name").ToString)
                            .Rows(n).Cells(6).Value = Val(dt2.Rows(i).Item("Rec_Pcs").ToString)
                            .Rows(n).Cells(7).Value = Val(dt2.Rows(i).Item("Meter_per_PCs").ToString)
                            .Rows(n).Cells(8).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                            '.Rows(n).Cells(9).Value = Val(dt2.Rows(i).Item("Type1_Pcs").ToString)
                            '.Rows(n).Cells(10).Value = Format(Val(dt2.Rows(i).Item("Type1_Meters").ToString), "########0.000")

                            .Rows(n).Cells(9).Value = Val(dt2.Rows(i).Item("Total_Pcs").ToString)
                            .Rows(n).Cells(10).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.000")
                            .Rows(n).Cells(11).Value = Format(Val(dt2.Rows(i).Item("Cons_Yarn").ToString), "########0.000")
                            .Rows(n).Cells(12).Value = Format(Val(dt2.Rows(i).Item("Cons_Bobin").ToString), "########0.000")
                            .Rows(n).Cells(13).Value = Format(Val(dt2.Rows(i).Item("Cons_Kuri").ToString), "########0.000")
                            .Rows(n).Cells(14).Value = Format(Val(dt2.Rows(i).Item("Excess_Short").ToString), "########0.000")
                            .Rows(n).Cells(15).Value = Format(Val(dt2.Rows(i).Item("Cons_pavu").ToString), "########0.000")
                            .Rows(n).Cells(16).Value = Format(Val(dt2.Rows(i).Item("Meters_Excess_Short").ToString), "########0.000")

                        Next i

                    End If

                    If .RowCount = 0 Then .Rows.Add()

                End With

                'With dgv_ConsYarnDetails_Total
                '    If .RowCount = 0 Then .Rows.Add()
                '    .Rows(0).Cells(13).Value = Format(Val(dt1.Rows(0).Item("Total_Dgv_Meters").ToString), "########0.00")

                '    'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then
                '    '    .Rows(0).Cells(6).Value = Format(Val(dt1.Rows(0).Item("Total_Dgv_Weight").ToString), "########0.00")

                '    'Else
                '    '    .Rows(0).Cells(6).Value = Format(Val(dt1.Rows(0).Item("Total_Dgv_Weight").ToString), "########0.000")

                '    'End If

                'End With
                'dt2.Clear()
                NoCalc_Status = False
                Calculation_Total_ConsumedYarnDetails()
                'Calculation_Total_ReceiptMeter()
                NoCalc_Status = True
                da2 = New SqlClient.SqlDataAdapter("Select a.*  from Weaver_ClothReceipt_Piece_Details a  Where a.Weaver_Wages_Code = '" & Trim(NewCode) & "' Order by fOR_oRDERbY , Weaver_ClothReceipt_No", con)
                dt4 = New DataTable
                da2.Fill(dt4)

                With dgv_Receipt_Details

                    .Rows.Clear()
                    SNo = 0

                    If dt4.Rows.Count > 0 Then

                        For i = 0 To dt4.Rows.Count - 1

                            n = .Rows.Add()
                            SNo = SNo + 1
                            .Rows(n).Cells(0).Value = Val(SNo)
                            .Rows(n).Cells(1).Value = Common_Procedures.Cloth_IdNoToName(con, Val(dt4.Rows(i).Item("Cloth_IdNo").ToString))
                            .Rows(n).Cells(2).Value = Val(dt4.Rows(i).Item("Pick").ToString)
                            .Rows(n).Cells(3).Value = Format(Val(dt4.Rows(i).Item("Width").ToString), "########0.00")
                            .Rows(n).Cells(4).Value = Format(Val(dt4.Rows(i).Item("Receipt_Pcs").ToString), "########0")
                            .Rows(n).Cells(5).Value = Format(Val(dt4.Rows(i).Item("Meter_per_PCs").ToString), "########0.00")
                            .Rows(n).Cells(6).Value = Format(Val(dt4.Rows(i).Item("ReceiptMeters_Receipt").ToString), "########0.00")

                            .Rows(n).Cells(7).Value = Format(Val(dt4.Rows(i).Item("Type1_Pcs").ToString), "########0")
                            .Rows(n).Cells(8).Value = Format(Val(dt4.Rows(i).Item("Type1_Meters").ToString), "########0.00")
                            .Rows(n).Cells(9).Value = Format(Val(dt4.Rows(i).Item("Type1_Rate").ToString), "########0.00")
                            .Rows(n).Cells(10).Value = Format(Val(dt4.Rows(i).Item("Type1_Amount").ToString), "########0.00")
                            .Rows(n).Cells(11).Value = Format(Val(dt4.Rows(i).Item("Type2_Pcs").ToString), "########0")
                            .Rows(n).Cells(12).Value = Format(Val(dt4.Rows(i).Item("Type2_Meters").ToString), "########0.00")
                            .Rows(n).Cells(13).Value = Format(Val(dt4.Rows(i).Item("Type2_Rate").ToString), "########0.00")
                            .Rows(n).Cells(14).Value = Format(Val(dt4.Rows(i).Item("Type2_Amount").ToString), "########0.00")

                            .Rows(n).Cells(15).Value = Format(Val(dt4.Rows(i).Item("Total_Pcs").ToString), "########0")
                            .Rows(n).Cells(16).Value = Format(Val(dt4.Rows(i).Item("Total_Meters").ToString), "########0.00")
                            .Rows(n).Cells(17).Value = Format(Val(dt4.Rows(i).Item("Total_Amount").ToString), "########0")
                            .Rows(n).Cells(18).Value = Format(Val(dt4.Rows(i).Item("Excess_Short").ToString), "########0.00")
                            .Rows(n).Cells(19).Value = dt4.Rows(i).Item("Weaver_ClothReceipt_No").ToString
                            .Rows(n).Cells(20).Value = Format(Convert.ToDateTime(dt4.Rows(i).Item("Weaver_ClothReceipt_Date").ToString), "dd-MM-yyyy")
                            .Rows(n).Cells(21).Value = dt4.Rows(i).Item("Weaver_ClothReceipt_Code").ToString
                            .Rows(n).Cells(22).Value = Format(Val(dt4.Rows(i).Item("Reed").ToString), "########0.000")

                            '.Rows(n).Cells(22).Value = Format(Val(dt4.Rows(i).Item("Cons_Kuri").ToString), "########0.00")
                            '.Rows(n).Cells(23).Value = dt4.Rows(i).Item("Receipt_Details_Slno").ToString
                        Next i

                    End If

                    If .RowCount = 0 Then .Rows.Add()

                    NoCalc_Status = False
                    For i = 0 To .Rows.Count - 1
                        Calculation_Grid_Amount_Calculation(i, 6)
                    Next

                End With
                NoCalc_Status = False

                Calculation_Total_ReceiptMeter()
                NoCalc_Status = True
                'With dgv_ReceiptDetails_Total
                '    If .RowCount = 0 Then .Rows.Add()
                '    .Rows(0).Cells(4).Value = Format(Val(dt1.Rows(0).Item("Pcs").ToString), "########0.00")
                '    .Rows(0).Cells(5).Value = Format(Val(dt1.Rows(0).Item("Receipt_Meters").ToString), "########0.000")
                'End With
                dt4.Clear()

                da1 = New SqlClient.SqlDataAdapter("select a.*, b.EndsCount_Name from Stock_Pavu_Processing_Details a INNER JOIN EndsCount_Head b ON a.EndsCount_IdNo = b.EndsCount_IdNo where a.Reference_Code = '" & Trim(Pk_Condition2) & Trim(NewCode) & "'", con)
                dt3 = New DataTable
                da1.Fill(dt3)

                dgv_BobinelectionDetails.Rows.Clear()
                SNo1 = 0

                If dt3.Rows.Count > 0 Then

                    For i = 0 To dt3.Rows.Count - 1
                        SNo1 = SNo1 + 1
                        n = dgv_BobinelectionDetails.Rows.Add()
                        dgv_BobinelectionDetails.Rows(n).Cells(0).Value = Val(dt3.Rows(i).Item("Detail_SlNo").ToString)
                        dgv_BobinelectionDetails.Rows(n).Cells(1).Value = dt3.Rows(i).Item("EndsCount_Name").ToString
                        dgv_BobinelectionDetails.Rows(n).Cells(2).Value = Format(Val(dt3.Rows(i).Item("Meters").ToString), "########0.000")

                    Next i

                End If
                dt3.Clear()
                dt3.Dispose()

                da1 = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name from Stock_Yarn_Processing_Details a INNER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo where a.Reference_Code = '" & Trim(Pk_Condition2) & Trim(NewCode) & "'", con)
                dt4 = New DataTable
                da1.Fill(dt4)

                dgv_KuriDetails.Rows.Clear()
                SNo2 = 0

                If dt4.Rows.Count > 0 Then

                    For i = 0 To dt4.Rows.Count - 1
                        SNo2 = SNo2 + 1
                        n = dgv_KuriDetails.Rows.Add()

                        dgv_KuriDetails.Rows(n).Cells(0).Value = Val(dt4.Rows(i).Item("Detail_SlNo").ToString)
                        dgv_KuriDetails.Rows(n).Cells(1).Value = dt4.Rows(i).Item("Count_Name").ToString
                        dgv_KuriDetails.Rows(n).Cells(2).Value = Format(Val(dt4.Rows(i).Item("Weight").ToString), "#######0.000")

                    Next i


                End If
                dt4.Clear()
                dt4.Dispose()

                NoCalc_Status = False




            Else

                new_record()

            End If



            Grid_Cell_DeSelect()

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

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim PkCode As String = ""
        Dim Nr As Integer = 0

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        'If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Weaver_Cloth_Rceipt_Wages_Entry, New_Entry, Me, con, "Weaver_Wages_Head", "Weaver_Wages_Code", NewCode, "Weaver_Wages_Date", "(Weaver_Wages_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

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
            PkCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            NewCode = Trim(Pk_Condition) & Trim(PkCode)

            cmd.Connection = con
            cmd.Transaction = trans
            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition2) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition2) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition_WPYMT) & Trim(NewCode), trans)


            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(PkCode), trans)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(PkCode), trans)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition_WPYMT) & Trim(PkCode), trans)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition_WADVP) & Trim(PkCode), trans)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition_WADVD) & Trim(PkCode), trans)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition_WPTDS) & Trim(PkCode), trans)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition_WFRGT) & Trim(PkCode), trans)


            'cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()
            'cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            'cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(PkCondition_WPYMT) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(PkCondition_WPYMT) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()
            'cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(PkCondition_WPYMT) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(PkCondition_WPYMT) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Weaver_ClothReceipt_Piece_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from Weaver_Cloth_Receipt_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Weaver_Wages_Yarn_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Weaver_Wages_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code = '" & Trim(NewCode) & "'"
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

            If dtp_Date.Enabled = True And dtp_Date.Visible = True Then dtp_Date.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (ledger_type = 'WEAVER'  or Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1 ) and Close_status = 0 order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"


            da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head order by Count_Name", con)
            da.Fill(dt2)
            cbo_Filter_PartyName.DataSource = dt2
            cbo_Filter_PartyName.DisplayMember = "Count_Name"

            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_CountName.Text = ""
            cbo_Filter_CountName.SelectedIndex = -1
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

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Wages_No from Weaver_Wages_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Weaver_Wages_Code LIKE '" & Trim(Pk_Condition) & "%' Order by for_Orderby, Weaver_Wages_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_BillNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Wages_No from Weaver_Wages_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Weaver_Wages_Code LIKE '" & Trim(Pk_Condition) & "%' Order by for_Orderby, Weaver_Wages_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_BillNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Wages_No from Weaver_Wages_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Weaver_Wages_Code LIKE '" & Trim(Pk_Condition) & "%' Order by for_Orderby desc, Weaver_Wages_No desc", con)
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
            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Wages_No from Weaver_Wages_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Weaver_Wages_Code LIKE '" & Trim(Pk_Condition) & "%' Order by for_Orderby desc, Weaver_Wages_No desc", con)
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

            lbl_BillNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Wages_Head", "Weaver_Wages_Code", "For_OrderBy", "( Weaver_Wages_Code LIKE '" & Trim(Pk_Condition) & "%' )", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_BillNo.ForeColor = Color.Red


            dtp_Date.Text = Date.Today.ToShortDateString
            Da1 = New SqlClient.SqlDataAdapter("select top 1 * from Weaver_Wages_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  and Weaver_Wages_Code LIKE '" & Trim(Pk_Condition) & "%' Order by for_Orderby desc, Weaver_Wages_No desc", con)
            Dt1 = New DataTable
            Da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If Dt1.Rows(0).Item("Weaver_Wages_Date").ToString <> "" Then dtp_Date.Text = Dt1.Rows(0).Item("Weaver_Wages_Date").ToString
                End If
                If Dt1.Rows(0).Item("Weaver_Wages_Date").ToString <> "" Then dtp_Date.Text = Dt1.Rows(0).Item("Weaver_Wages_Date").ToString
                If Dt1.Rows(0).Item("Loom_Type").ToString <> "" Then cbo_LoomType.Text = Dt1.Rows(0).Item("Loom_Type").ToString
                txt_CGST_Percentage.Text = Dt1.Rows(0).Item("CGST_Percentage").ToString
                txt_SGST_Percentage.Text = Dt1.Rows(0).Item("SGST_Percentage").ToString
                'txt_Tds.Text = Dt1.Rows(0).Item("Tds_Perc").ToString
            End If
            Dt1.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da1.Dispose()

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        Try

            inpno = InputBox("Enter Bill No.", "FOR FINDING...")

            InvCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Weaver_Wages_No from Weaver_Wages_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code = '" & Trim(InvCode) & "'", con)
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
                MessageBox.Show("Bill No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Weaver_Cloth_Rceipt_Wages_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Weaver_Cloth_Rceipt_Wages_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        'If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Weaver_Cloth_Rceipt_Wages_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Bill No.", "FOR NEW BILL NO. INSERTION...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Weaver_Wages_No from Weaver_Wages_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code = '" & Trim(InvCode) & "'", con)
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
                    MessageBox.Show("Invalid BILL No.", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_BillNo.Text = Trim(UCase(inpno))

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
        Dim StkDelvTo_ID As Integer = 0, StkRecFrm_ID As Integer = 0
        Dim Led_type As String = ""
        Dim vStkOf_Pos_IdNo As Integer = 0, StkOff_ID As Integer = 0
        Dim dt As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim clth_ID As Integer = 0
        Dim RecClth_ID As Integer = 0
        Dim Wev_ID As Integer = 0
        Dim Endcnt_ID As Integer = 0
        Dim cunt_ID As Integer = 0
        Dim ECnt_ID As Integer
        Dim KuriCnt_ID As Integer
        Dim clthtyp_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Partcls As String = ""
        Dim vTotConsYrnMtrs As Single, vTotConsYrnWgt As Single
        Dim vTotWgsMtrs As Single, vTotWgsGrsAmt As Single
        Dim vTotRcptMtrs As Single, vTotRcptPcs As Single
        Dim Cr_ID As Integer = 0
        Dim Dr_ID As Integer = 0
        Dim TdsAc_ID As Integer = 0
        Dim PcsChkCode As String = ""
        Dim PkCode As String = ""
        Dim Lm_ID As Integer = 0
        Dim Wdth_Typ As String = ""
        Dim Nr As Integer = 0
        Dim Sno1 As Integer = 0
        Dim Sno2 As Integer = 0
        Dim Sno3 As Integer = 0
        Dim Sno4 As Integer = 0
        Dim RecEdsCnt_Id As Integer = 0
        Dim RecCnt_Id As Integer = 0
        Dim Wgt_Mtr As Double = 0
        Dim ConsYarn As Single = 0
        Dim StkConsPavu As Single = 0
        Dim ConsPavu As Single = 0
        Dim SOUND_MTR As Single = 0
        Dim SECOND_MTR As Single = 0
        Dim BIT_MTR As Single = 0
        Dim REJECT_MTR As Single = 0
        Dim OTHER_MTR As Single = 0
        Dim vNoof_ReceiptCount As Integer = 0
        Dim CloTyp_ID As Integer = 0
        Dim Crimp_Perc As Single = 0
        Dim vRecNo As String = ""
        Dim vRecPDcNo As String = ""
        Dim vRecDt As String = ""
        Dim WidTyp As Single = 0
        Dim ClthName As String = ""
        Dim Rep_Partcls_Wages As String = ""
        Dim TdsSts As Integer = 0
        Dim DateColUpdt As String = ""
        Dim RCM_Sts As String = ""
        Dim WevWages_ROff As Single = 0
        Dim Excess_Shrt As Single = 0
        Dim vSELC_LOTCODE As String = ""

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        'If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Weaver_Cloth_Rceipt_Wages_Entry, New_Entry, Me, con, "Weaver_Wages_Head", "Weaver_Wages_Code", NewCode, "Weaver_Wages_Date", "(Weaver_Wages_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Weaver_Wages_No desc", dtp_Date.Value.Date) = False Then Exit Sub


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

        Wev_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Weaver.Text)
        If Wev_ID = 0 Then
            MessageBox.Show("Invalid Weaver Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Weaver.Enabled Then cbo_Weaver.Focus()
            Exit Sub
        End If
        If Trim(cbo_LoomType.Text) = "" Then
            cbo_LoomType.Text = "POWER LOOM"
        End If

        lbl_UserName.Text = Common_Procedures.User.IdNo

        Dim RndOff_STS As Integer = 0
        Dim TaxableRndOff_STS As Integer = 0

        TaxableRndOff_STS = 0
        If chk_TaxABLEAmount_RoundOff_STS.Checked = True Then TaxableRndOff_STS = 1

        RndOff_STS = 0
        If chk_TaxAmount_RoundOff_STS.Checked = True Then RndOff_STS = 1

        'If Trim(lbl_PartyDcNo.Text) <> "" Then
        '    Da = New SqlClient.SqlDataAdapter("select Weaver_BillNo from Weaver_Wages_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code LIKE '%/" & Trim(Common_Procedures.FnRange) & "' and Ledger_IdNo = " & Str(Val(Wev_ID)) & " and Weaver_BillNo = '" & Trim(lbl_PartyDcNo.Text) & "'", con)
        '    Dt1 = New DataTable
        '    Da.Fill(Dt1)
        '    If Dt1.Rows.Count > 0 Then
        '        If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
        '            MessageBox.Show("Duplicate Weaver Bill No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '            If cbo_Weaver.Enabled Then cbo_Weaver.Focus()
        '            Exit Sub
        '        End If
        '    End If
        '    Dt1.Clear()
        'End If



        'For i = 0 To dgv_ConsYarn_Details.RowCount - 1

        '    If Val(dgv_ConsYarn_Details.Rows(i).Cells(0).Value) <> 0 Or Val(dgv_ConsYarn_Details.Rows(i).Cells(6).Value) <> 0 Then

        '        cunt_ID = Common_Procedures.Count_NameToIdNo(con, dgv_ConsYarn_Details.Rows(i).Cells(1).Value)
        '        If clth_ID = 0 Then
        '            MessageBox.Show("Invalid Count Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '            If dgv_ConsYarn_Details.Enabled And dgv_ConsYarn_Details.Visible Then
        '                dgv_ConsYarn_Details.Focus()
        '                dgv_ConsYarn_Details.CurrentCell = dgv_ConsYarn_Details.Rows(i).Cells(1)
        '            End If
        '            Exit Sub
        '        End If

        '    End If

        'Next
        TdsSts = 0
        If chk_Tds.Checked = True Then TdsSts = 1


        NoCalc_Status = False

        Calculation_Total_ReceiptMeter()
        Calculation_Total_ConsumedYarnDetails()


        vTotConsYrnMtrs = 0 : vTotConsYrnWgt = 0
        If dgv_ConsYarnDetails_Total.RowCount > 0 Then
            vTotConsYrnMtrs = Val(dgv_ConsYarnDetails_Total.Rows(0).Cells(8).Value())
            vTotConsYrnWgt = Val(dgv_ConsYarnDetails_Total.Rows(0).Cells(11).Value())
        End If



        vTotRcptMtrs = 0 : vTotRcptPcs = 0
        If dgv_ReceiptDetails_Total.RowCount > 0 Then
            vTotRcptPcs = Val(dgv_ReceiptDetails_Total.Rows(0).Cells(4).Value())
            vTotRcptMtrs = Val(dgv_ReceiptDetails_Total.Rows(0).Cells(6).Value())
            vTotWgsMtrs = Val(dgv_ReceiptDetails_Total.Rows(0).Cells(16).Value())
            vTotWgsGrsAmt = Val(dgv_ReceiptDetails_Total.Rows(0).Cells(17).Value())
        End If

        vNoof_ReceiptCount = 0
        If dgv_Receipt_Details.Rows.Count > 0 Then
            For I = 0 To dgv_Receipt_Details.Rows.Count - 1
                If Val(dgv_Receipt_Details.Rows(I).Cells(6).Value) <> 0 And dgv_Receipt_Details.Rows(I).Cells(20).Value <> "" Then
                    vNoof_ReceiptCount = vNoof_ReceiptCount + 1
                End If
            Next
        End If

        lbl_RecCode.Text = ""
        vRecPDcNo = ""
        vRecNo = ""
        vRecDt = ""
        If vNoof_ReceiptCount = 1 Then
            If dgv_Receipt_Details.Rows.Count > 0 Then
                lbl_RecCode.Text = dgv_Receipt_Details.Rows(0).Cells(20).Value
                vRecNo = dgv_Receipt_Details.Rows(0).Cells(18).Value
                vRecDt = dgv_Receipt_Details.Rows(0).Cells(19).Value
                vRecPDcNo = txt_PartyDcNo.Text
            End If
        End If

        Da = New SqlClient.SqlDataAdapter("select Weaver_Piece_Checking_Code, Loom_IdNo, Width_Type from Weaver_Cloth_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        PcsChkCode = ""
        Lm_ID = 0
        Wdth_Typ = ""

        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) = False Then
                PcsChkCode = Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString
            End If
            Lm_ID = Val(Dt1.Rows(0).Item("Loom_IdNo").ToString)
            Wdth_Typ = Dt1.Rows(0).Item("Width_Type").ToString
        End If
        Dt1.Clear()
        ConsPavu = Val(Common_Procedures.get_Pavu_Consumption(con, clth_ID, Lm_ID, Val(vTotRcptMtrs), Trim(Wdth_Typ)))
        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                PkCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then
                    lbl_BillNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Wages_Head", "Weaver_Wages_Code", "For_OrderBy", "(Weaver_Wages_Code LIKE '" & Trim(Pk_Condition) & "%' )", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)
                End If

                PkCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            End If

            NewCode = Trim(Pk_Condition) & Trim(PkCode)

            vSELC_LOTCODE = Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_Company.Tag))


            cmd.Connection = con

            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@WagesDate", dtp_Date.Value.Date)

            If New_Entry = True Then

                cmd.CommandText = "Insert into Weaver_Wages_Head (    Weaver_Wages_Code   ,               Company_IdNo       ,     Weaver_Wages_No  ,                     for_OrderBy                                                   ,  Weaver_Wages_Date,              Ledger_IdNo,      Weaver_Cloth_Receipt_Code    ,  Cloth_Idno  , EndsCount_Idno   ,              Rec_No     ,             Rec_Date   ,             P_Dc_No                ,                 Folding_Less           ,                 Freight_Charge           ,                 Paid_Amount           ,                Add_Amount           ,                  Tds_Perc      ,                  Tds_Perc_Calc        ,                  Less_Amount           ,                  Assesable_Value        ,                       Net_Amount            ,               Total_Dgv_Meters    ,               Total_Dgv_Weight   ,              Total_Meters    ,               Total_Cooly       ,                 Pcs          ,               Receipt_Meters   ,               Weaver_BillNo          ,                                WeaverBillNo_ForOrderBy                          ,  user_idNo                    , Total_Taxable_Amount                    ,CGST_Percentage                           ,CGST_Amount                           ,SGST_Percentage                           ,SGST_Amount           , Remarks                           , Loom_Type                        ,Width_Type                         , Noof_Beam                       , Tds_status   ,                 TaxableAmount_RoundOff_Status  , TaxAmount_RoundOff_Status ) " &
                                    "     Values                 ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_BillNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_BillNo.Text))) & ",     @WagesDate    , " & Str(Val(Wev_ID)) & ",  '" & Trim(lbl_RecCode.Text) & "' , 0             ,       0        , '" & Trim(vRecNo) & "',  '" & Trim(vRecDt) & "',  '" & Trim(txt_PartyDcNo.Text) & "', " & Str(Val(txt_Folding_Less.Text)) & ", " & Str(Val(txt_Freight_Charge.Text)) & ", " & Str(Val(txt_Paid_Amount.Text)) & " ,  " & Str(Val(txt_Add_Amount.Text)) & ",  " & Str(Val(txt_Tds.Text)) & ",  " & Str(Val(txt_Tds_Amount.Text)) & ",   " & Str(Val(txt_Less_Amount.Text)) & ",  " & Str(Val(lbl_Total_Amount.Text)) & ",  " & Str(Val(CSng(lbl_Net_Amount.Text))) & ",  " & Str(Val(vTotConsYrnMtrs)) & ",  " & Str(Val(vTotConsYrnWgt)) & ", " & Str(Val(vTotWgsMtrs)) & ",  " & Str(Val(vTotWgsGrsAmt)) & ", " & Str(Val(vTotRcptPcs)) & ",  " & Str(Val(vTotRcptMtrs)) & ", '" & Trim(lbl_BillNo.Text) & "',  " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_BillNo.Text))) & " , " & Val(lbl_UserName.Text) & "," & Str(Val(lbl_Taxable_Value.Text)) & "," & Str(Val(txt_CGST_Percentage.Text)) & "," & Str(Val(lbl_CGST_Amount.Text)) & "," & Str(Val(txt_SGST_Percentage.Text)) & "," & Str(Val(lbl_SGST_Amount.Text)) & " ,'" & Trim(txt_Remarks.Text) & "','" & Trim(cbo_LoomType.Text) & "','" & Trim(cbo_WidthType.Text) & "'," & Val(txt_No_Of_Beams.Text) & ", " & Val(TdsSts) & " , " & Str(Val(TaxableRndOff_STS)) & "  ,   " & Str(Val(RndOff_STS)) & ") "
                cmd.ExecuteNonQuery()
                cmd.CommandText = "Insert into Weaver_Cloth_Receipt_Head ( Receipt_Type, Weaver_ClothReceipt_Code,             Company_IdNo         ,       Weaver_ClothReceipt_No  ,                               for_OrderBy                              , Weaver_ClothReceipt_date,           Ledger_IdNo   ,      Cloth_IdNo    ,            Lot_No         ,            Party_DcNo         ,           EndsCount_IdNo   ,          Count_IdNo        ,             empty_beam          ,           noof_pcs            ,             pcs_fromno         ,              pcs_tono        ,           ReceiptMeters_Receipt        ,            Receipt_Meters               ,          ConsumedYarn_Receipt       ,              Consumed_Yarn         ,         ConsumedPavu_Receipt       ,              Consumed_Pavu         ,       Loom_IdNo        ,              Width_Type           ,       Transport_IdNo      ,     Freight_Amount_Receipt   ,  Folding_Receipt, Folding,       Total_Receipt_Pcs      ,    Total_Receipt_Meters   , BeamConsumption_Receipt, BeamConsumption_Meters, Weaver_Piece_Checking_Code, Weaver_Piece_Checking_Increment, Weaver_Wages_Code, Weaver_Wages_Increment    ,            StockOff_IdNo   ,        User_idNo    , Purchase_Status  ,Loom_Type                                                    ,lotcode_forSelection ) " &
                                  "   Values                             (     'W'     , '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_BillNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_BillNo.Text))) & ",      @WagesDate     , " & Str(Val(Wev_ID)) & ",     0             , ''                            , '" & Trim(txt_PartyDcNo.Text) & "', 0                         , 0              ,                      0                   , " & Val(vTotRcptPcs) & "      , 0                          ,                    0             , " & Str(Val(vTotRcptMtrs)) & ", " & Str(Val(vTotRcptMtrs)) & "                   , " & Str(Val(vTotConsYrnWgt)) & "     , " & Str(Val(vTotConsYrnWgt)) & ",      " & Str(Val(ConsPavu)) & "     , " & Str(Val(ConsPavu)) & ",            0                     ,                  '" & Trim(cbo_WidthType.Text) & "',       0                    , " & Val(txt_Freight_Charge.Text) & ",       100       ,   100  , " & Str(Val(vTotRcptPcs)) & ", " & Str(Val(vTotRcptMtrs)) & ",          0             ,          0            ,          ''               ,             0                  ,         ''       ,           0               ," & Str(Val(StkOff_ID)) & " , " & Val(lbl_UserName.Text) & " , 0   ,'" & Trim(cbo_LoomType.Text) & "', '" & Trim(vSELC_LOTCODE) & "') "
                cmd.ExecuteNonQuery()
            Else

                cmd.CommandText = "Update Weaver_Wages_Head set Weaver_Wages_Date = @WagesDate, Ledger_IdNo = " & Str(Val(Wev_ID)) & ",Cloth_Idno = 0 ,EndsCount_Idno = 0 , Weaver_Cloth_Receipt_Code = '" & Trim(lbl_RecCode.Text) & "',  Rec_No = '" & Trim(vRecNo) & "',  Rec_Date  = '" & Trim(vRecDt) & "', P_Dc_No = '" & Trim(txt_PartyDcNo.Text) & "', Folding_Less =  " & Str(Val(txt_Folding_Less.Text)) & ", Freight_Charge = " & Str(Val(txt_Freight_Charge.Text)) & ", Paid_Amount = " & Str(Val(txt_Paid_Amount.Text)) & ", Add_Amount = " & Str(Val(txt_Add_Amount.Text)) & "  , Tds_Perc =  " & Str(Val(txt_Tds.Text)) & " , Tds_Perc_Calc =  " & Str(Val(txt_Tds_Amount.Text)) & " ,      Less_Amount =  " & Str(Val(txt_Less_Amount.Text)) & " , Assesable_Value = " & Str(Val(lbl_Total_Amount.Text)) & ", Net_Amount = " & Str(Val(CSng(lbl_Net_Amount.Text))) & ", Total_Dgv_Meters = " & Str(Val(vTotConsYrnMtrs)) & " ,    Total_Dgv_Weight = " & Str(Val(vTotConsYrnWgt)) & ", Total_Meters     = " & Str(Val(vTotWgsMtrs)) & "      ,   Total_Cooly = " & Str(Val(vTotWgsGrsAmt)) & ",  Pcs  =  " & Str(Val(vTotRcptPcs)) & "      ,     Receipt_Meters = " & Str(Val(vTotRcptMtrs)) & "   , Weaver_BillNo = '" & Trim(lbl_BillNo.Text) & "', WeaverBillNo_ForOrderBy = " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_BillNo.Text))) & ", User_IdNo = " & Val(lbl_UserName.Text) & ",Total_Taxable_Amount =" & Str(Val(lbl_Taxable_Value.Text)) & ",CGST_Percentage =" & Str(Val(txt_CGST_Percentage.Text)) & " ,CGST_Amount =" & Str(Val(lbl_CGST_Amount.Text)) & " ,SGST_Percentage =" & Str(Val(txt_SGST_Percentage.Text)) & ",SGST_Amount =" & Str(Val(lbl_SGST_Amount.Text)) & ",Remarks= '" & (txt_Remarks.Text) & "',Loom_Type = '" & Trim(cbo_LoomType.Text) & "',Width_Type = '" & Trim(cbo_WidthType.Text) & "',Noof_Beam = " & Val(txt_No_Of_Beams.Text) & ",tds_Status = " & Val(TdsSts) & " , TaxableAmount_RoundOff_Status =" & Str(Val(TaxableRndOff_STS)) & "  , TaxAmount_RoundOff_Status =" & Str(Val(RndOff_STS)) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()
                cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Receipt_Type = 'W', Weaver_ClothReceipt_date = @WagesDate, Ledger_IdNo = " & Str(Val(Wev_ID)) & ", Cloth_IdNo = 0 , Lot_No = '',  Party_DcNo  = '" & Trim(txt_PartyDcNo.Text) & "',  EndsCount_IdNo =  0  , Count_IdNo =  0       , empty_beam = 0      , noof_pcs = " & Val(vTotRcptPcs) & " , pcs_fromno =  0  , pcs_tono = 0 , ReceiptMeters_Receipt = " & Val(vTotRcptMtrs) & ", ConsumedYarn_Receipt = " & Val(vTotConsYrnWgt) & ", ConsumedPavu_Receipt = " & Val(ConsPavu) & ", Loom_IdNo = 0 , Width_Type = '" & Trim(cbo_WidthType.Text) & "', Transport_IdNo = 0, Freight_Amount_Receipt = " & Val(txt_Freight_Charge.Text) & ", Total_Receipt_Pcs = " & Str(Val(vTotRcptPcs)) & ", Total_Receipt_Meters = " & Str(Val(vTotRcptMtrs)) & " , StockOff_IdNo = " & Str(Val(StkOff_ID)) & ",  User_idNo = " & Val(lbl_UserName.Text) & " ,  Purchase_Status = 0 ,Loom_Type = '" & Trim(cbo_LoomType.Text) & "' ,lotcode_forSelection='" & Trim(vSELC_LOTCODE) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Receipt_Meters = " & Val(vTotRcptMtrs) & ", Consumed_Yarn = " & Val(vTotConsYrnWgt) & ", Consumed_Pavu = " & Val(ConsPavu) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and Weaver_Piece_Checking_Code = '' and Weaver_Wages_Code = ''"
                cmd.ExecuteNonQuery()



            End If

            EntID = Trim(Pk_Condition) & Trim(lbl_BillNo.Text)
            PBlNo = Trim(lbl_BillNo.Text)
            Partcls = "Wages : Bill.No. " & Trim(lbl_BillNo.Text)


            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition2) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition2) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Weaver_Wages_Yarn_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Weaver_ClothReceipt_Piece_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            Sno = 0
            Sno1 = 0
            Sno3 = 1000
            Sno4 = 1000
            With dgv_Receipt_Details

                For i = 0 To .RowCount - 1
                    Sno = Sno + 1
                    If Trim(.Rows(i).Cells(1).Value) <> "" Or Val(.Rows(i).Cells(4).Value) <> 0 Then



                        RecClth_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)
                        Da = New SqlClient.SqlDataAdapter("select * from  Cloth_Head  where  Cloth_Idno = " & Val(RecClth_ID) & " ", con)
                        Da.SelectCommand.Transaction = tr
                        dt = New DataTable
                        Da.Fill(dt)
                        If dt.Rows.Count > 0 Then
                            RecEdsCnt_Id = (dt.Rows(0).Item("EndsCount_IdNo").ToString)
                            RecCnt_Id = (dt.Rows(0).Item("Cloth_WeftCount_IdNo").ToString)
                            Wgt_Mtr = Val(dt.Rows(0).Item("Weight_Meter_Weft").ToString)
                        End If
                        StkDelvTo_ID = 0 : StkRecFrm_ID = 0
                        If Val(Wev_ID) = Val(Common_Procedures.CommonLedger.Godown_Ac) Then
                            StkDelvTo_ID = Val(Common_Procedures.CommonLedger.Godown_Ac)
                            StkRecFrm_ID = 0

                        Else
                            StkDelvTo_ID = Val(Common_Procedures.CommonLedger.Godown_Ac)
                            StkRecFrm_ID = Val(Wev_ID)

                        End If

                        Led_type = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(Wev_ID)) & ")", , tr)


                        vStkOf_Pos_IdNo = 0


                        If Trim(UCase(Led_type)) = "JOBWORKER" Then
                            vStkOf_Pos_IdNo = Wev_ID
                        Else
                            vStkOf_Pos_IdNo = Val(Common_Procedures.CommonLedger.Godown_Ac)
                        End If

                        cmd.CommandText = "Insert into Weaver_ClothReceipt_Piece_Details ( Weaver_ClothReceipt_Code  ,            Company_IdNo          ,      Weaver_ClothReceipt_No    ,                               for_OrderBy                                , Weaver_ClothReceipt_date,           Weaver_Wages_Code      ,  Weaver_Wages_Date ,  Lot_Code                       , Lot_No  , PieceNo_OrderBy, Sl_No             , Piece_No                              ,    Cloth_IdNo               , Pick                                     ,            Width                               ,   Receipt_Pcs                             ,Meter_per_PCs                              ,     ReceiptMeters_Receipt                 , Receipt_Meters                            ,  Type1_Pcs                                , Type1_Meters                              ,  Type1_Rate                               , Type1_Amount                              , Type2_pcs                                 , Type2_Meters                                 , Type2_Rate                                 , Type2_Amount                              , Total_Pcs                                  , Total_Meters                              , Total_Amount                              ,Excess_Short                                ,                      Reed               ) " &
                                                                                  "  Values  ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_BillNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_BillNo.Text))) & " ,          @WagesDate     ,  '" & Trim(NewCode) & "'         ,   @WagesDate       ,'" & Trim(NewCode) & "'          ,     ''  ,        0       ," & Val(Sno) & "   ,'" & Trim(.Rows(i).Cells(0).Value) & "', " & Str(Val(RecClth_ID)) & ", " & Str(Val(.Rows(i).Cells(2).Value)) & ",  " & Str(Val(Val(.Rows(i).Cells(3).Value))) & ",  " & Str(Val(.Rows(i).Cells(4).Value)) & ",  " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & " ,  " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(.Rows(i).Cells(7).Value)) & " ,  " & Str(Val(.Rows(i).Cells(8).Value)) & ", " & Str(Val(.Rows(i).Cells(9).Value)) & ",  " & Str(Val(.Rows(i).Cells(10).Value)) & ",  " & Str(Val(.Rows(i).Cells(11).Value)) & " ,  " & Str(Val(.Rows(i).Cells(12).Value)) & ", " & Str(Val(.Rows(i).Cells(13).Value)) & ", " & Str(Val(.Rows(i).Cells(14).Value)) & " ,  " & Str(Val(.Rows(i).Cells(15).Value)) & "," & Str(Val(.Rows(i).Cells(16).Value)) & " , " & Str(Val(.Rows(i).Cells(17).Value)) & ", " & Str(Val(.Rows(i).Cells(18).Value)) & " ," & Str(Val(.Rows(i).Cells(22).Value)) & ")"
                        cmd.ExecuteNonQuery()


                        cmd.CommandText = "Truncate table EntryTemp"
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "Insert into EntryTemp (                    int1               ,      int2              ,           int3      ,            Meters1            ,                      Meters2                           ) " &
                                                "          Values     ( " & Val(RecClth_ID) & "," & Val(RecEdsCnt_Id) & "," & Val(RecCnt_Id) & " , " & Val(.Rows(i).Cells(8).Value) & " ,  " & Str(Val(.Rows(i).Cells(12).Value)) & " )"
                        cmd.ExecuteNonQuery()
                        Da = New SqlClient.SqlDataAdapter("select a.int1 as Cloth_Id,a.int2 as Ends_Id ,a.int3 as Count_Id, sum(a.Meters1) as Ty1_Mtrs,sum(a.Meters2) as Ty2_Mtrs   from EntryTemp a group by a.int1,a.int2,a.int3 having sum(a.Meters1)<>0 or sum(a.Meters2)<>0    ", con)
                        Da.SelectCommand.Transaction = tr
                        Dt1 = New DataTable
                        Da.Fill(Dt1)

                        If Dt1.Rows.Count > 0 Then
                            Sno1 = Sno1 + 1

                            For j = 0 To Dt1.Rows.Count - 1
                                cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code    ,             Company_IdNo         ,             Reference_No      ,                               for_OrderBy                              , Reference_Date,       StockOff_IdNo         ,        DeliveryTo_Idno        ,       ReceivedFrom_Idno   ,         Entry_ID     ,      Party_Bill_No   ,      Particulars       ,                         Sl_No,          Cloth_Idno                                                                                      ,              Folding,   UnChecked_Meters  ,  Meters_Type1                                            , Meters_Type2                                        , Meters_Type3                                                                 , Meters_Type4, Meters_Type5 ) " &
                                                  " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & "            , '" & Trim(lbl_BillNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_BillNo.Text))) & ",   @WagesDate  , " & Str(Val(vStkOf_Pos_IdNo)) & ", " & Str(Val(StkDelvTo_ID)) & ", " & Str(Val(StkRecFrm_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',   " & Val(Sno1) & "  , " & Str(Val(Dt1.Rows(j).Item("Cloth_Id").ToString)) & ",   100    ,                0    , " & Str(Val(Dt1.Rows(j).Item("Ty1_Mtrs").ToString)) & "      ," & Str(Val(Dt1.Rows(j).Item("Ty2_Mtrs").ToString)) & "        ,  0,     0     ,       0      ) "
                                cmd.ExecuteNonQuery()
                            Next
                        End If
                        Dt1.Clear()

                    End If

                Next

            End With

            With dgv_ConsYarn_Details

                Sno = 0
                Sno2 = 0
                For i = 0 To .RowCount - 1

                    If Trim(.Rows(i).Cells(1).Value) <> "" Or Val(.Rows(i).Cells(6).Value) <> 0 Then

                        Sno = Sno + 1
                        clth_ID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)
                        Endcnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, .Rows(i).Cells(4).Value, tr)
                        cunt_ID = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(5).Value, tr)
                        ' 
                        If Trim(UCase(cbo_LoomType.Text)) = "AUTO LOOM" Then
                            Crimp_Perc = Val(Common_Procedures.get_FieldValue(con, "Cloth_Head", "Crimp_Percentage", "(Cloth_IdNo = " & Str(Val(clth_ID)) & ")", , tr))

                            WidTyp = 0
                            If Trim(UCase(cbo_WidthType.Text)) = "FOURTH" Then
                                WidTyp = 4
                            ElseIf Trim(UCase(cbo_WidthType.Text)) = "TRIPLE" Then
                                WidTyp = 3
                            ElseIf Trim(UCase(cbo_WidthType.Text)) = "DOUBLE" Then
                                WidTyp = 2
                            Else
                                WidTyp = 1
                            End If
                            Excess_Shrt = Format(Val(.Rows(i).Cells(8).Value) - Val(.Rows(i).Cells(14).Value), "#################0.00")
                            'Excess_Shrt = Format(Val(.Rows(i).Cells(10).Value) - Val(.Rows(i).Cells(14).Value), "#################0.00")
                            StkConsPavu = (Val(Excess_Shrt) / Val(WidTyp)) * Val(txt_No_Of_Beams.Text)
                            Crimp_Perc = Val(StkConsPavu) * Crimp_Perc / 100
                            StkConsPavu = Format(StkConsPavu + Crimp_Perc, "#########0.00")
                        Else
                            Excess_Shrt = Format(Val(.Rows(i).Cells(8).Value) - Val(.Rows(i).Cells(14).Value), "#################0.00")
                            'Excess_Shrt = Format(Val(.Rows(i).Cells(10).Value) - Val(.Rows(i).Cells(14).Value), "#################0.00")
                            StkConsPavu = Val(Common_Procedures.get_Pavu_Consumption(con, clth_ID, Lm_ID, Val(Excess_Shrt), Trim(Wdth_Typ), tr))
                        End If
                        cmd.CommandText = "Insert into Weaver_Wages_Yarn_Details (       Weaver_Wages_Code  ,             Company_IdNo         ,             Weaver_Wages_No    ,                               for_OrderBy                               , Weaver_Wages_Date,            Sl_No     ,        Cloth_idno             ,Pick                                  ,      Width                         ,   EndsCount_Idno        ,         Count_IdNo       ,                      Rec_Pcs            ,Meter_per_PCs                              ,               Meters                       ,              Total_Pcs                      ,  Total_Meters                           , Cons_Yarn                              , Cons_Bobin                               ,     Cons_Kuri                          , Excess_Short                                   ,  Cons_pavu                           , Meters_Excess_Short            ) " &
                                            "     Values                         (   '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_BillNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_BillNo.Text))) & ",       @WagesDate , " & Str(Val(Sno)) & ",     " & Str(Val(clth_ID)) & "," & Val(.Rows(i).Cells(2).Value) & "  ," & Val(.Rows(i).Cells(3).Value) & " ," & Val(Endcnt_ID) & "    , " & Str(Val(cunt_ID)) & ", " & Val(.Rows(i).Cells(6).Value) & "   , " & Str(Val(.Rows(i).Cells(7).Value)) & ",  " & Str(Val(.Rows(i).Cells(8).Value)) & ", " & Str(Val(.Rows(i).Cells(9).Value)) & ", " & Str(Val(.Rows(i).Cells(10).Value)) & ", " & Str(Val(.Rows(i).Cells(11).Value)) & "," & Str(Val(.Rows(i).Cells(12).Value)) & "," & Str(Val(.Rows(i).Cells(13).Value)) & "," & Str(Val(.Rows(i).Cells(14).Value)) & "  ," & Str(Val(StkConsPavu)) & "," & Str(Val(Excess_Shrt)) & "    ) "
                        cmd.ExecuteNonQuery()
                        cmd.CommandText = "Truncate table EntryTemp"
                        cmd.ExecuteNonQuery()

                        cmd.CommandText = "Insert into EntryTemp (                   int1        ,      int2             ,           int3       ,            Meters1            ,                      Meters2            ) " &
                                                "          Values     ( " & Val(clth_ID) & "," & Val(Endcnt_ID) & ",      " & Val(cunt_ID) & " , " & Val(.Rows(i).Cells(11).Value) & " ,  " & Str(Val(StkConsPavu)) & " )"
                        cmd.ExecuteNonQuery()
                        Da = New SqlClient.SqlDataAdapter("select a.int1 as Cloth_Id,a.int2 as Ends_Id ,a.int3 as Count_Id, sum(a.Meters1) as Cons_Yarn,sum(a.Meters2) as Cons_Pavu  from EntryTemp a group by a.int1,a.int2,a.int3 having sum(a.Meters1)<>0 or sum(a.Meters2)<>0   ", con)
                        Da.SelectCommand.Transaction = tr
                        Dt1 = New DataTable
                        Da.Fill(Dt1)
                        If Dt1.Rows.Count > 0 Then


                            For j = 0 To Dt1.Rows.Count - 1
                                Sno2 = Sno2 + 1
                                cmd.CommandText = "Insert into Stock_Pavu_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Cloth_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, EndsCount_IdNo, Sized_Beam, Meters) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_BillNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_BillNo.Text))) & ", @WagesDate, 0, " & Str(Val(Wev_ID)) & ",  " & Str(Val(Dt1.Rows(j).Item("Cloth_Id").ToString)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno2)) & ", " & Str(Val(Dt1.Rows(j).Item("Ends_Id").ToString)) & " , 0, " & Str(Val(Dt1.Rows(j).Item("Cons_Pavu").ToString)) & "  )"
                                cmd.ExecuteNonQuery()



                                cmd.CommandText = "Insert into Stock_Yarn_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Particulars, Party_Bill_No, Sl_No, Count_IdNo, Yarn_Type, Mill_IdNo, Bags, Cones, Weight) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_BillNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_BillNo.Text))) & ", @WagesDate, 0, " & Str(Val(Wev_ID)) & ", '" & Trim(EntID) & "', '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "', " & Val(Sno2) & ", " & Str(Val(Dt1.Rows(j).Item("Count_Id").ToString)) & ", 'MILL', 0, 0, 0,  " & Str(Val(Dt1.Rows(j).Item("Cons_Yarn").ToString)) & " )"
                                cmd.ExecuteNonQuery()
                            Next
                        End If
                        With dgv_BobinelectionDetails

                            For j = 0 To .RowCount - 1

                                If Val(.Rows(j).Cells(0).Value) = Val(dgv_ConsYarn_Details.Rows(i).Cells(0).Value) And Trim(.Rows(j).Cells(1).Value) <> "" Then

                                    ECnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, .Rows(j).Cells(1).Value, tr)

                                    If Val(ECnt_ID) <> 0 And Val(.Rows(j).Cells(2).Value) <> 0 Then

                                        Sno3 = Sno3 + 1
                                        cmd.CommandText = "Insert into Stock_Pavu_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Cloth_Idno, Entry_ID, Party_Bill_No, Particulars,Detail_SlNo , Sl_No, EndsCount_IdNo, Sized_Beam, Meters) Values ('" & Trim(Pk_Condition2) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_BillNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_BillNo.Text))) & ", @WagesDate, 0, " & Str(Val(Wev_ID)) & ", " & Str(Val(RecClth_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Val(.Rows(j).Cells(0).Value) & ", " & Str(Val(Sno3)) & ", " & Str(Val(ECnt_ID)) & ", 0,  " & Val(.Rows(j).Cells(2).Value) & " )"
                                        cmd.ExecuteNonQuery()

                                    End If

                                End If
                            Next j
                        End With
                        With dgv_KuriSelection_Details

                            For j = 0 To .RowCount - 1

                                If Val(.Rows(j).Cells(0).Value) = Val(dgv_ConsYarn_Details.Rows(i).Cells(0).Value) And Trim(.Rows(j).Cells(1).Value) <> "" Then

                                    KuriCnt_ID = Common_Procedures.Count_NameToIdNo(con, .Rows(j).Cells(1).Value, tr)

                                    If Val(KuriCnt_ID) <> 0 And Val(.Rows(j).Cells(2).Value) <> 0 Then

                                        Sno4 = Sno4 + 1

                                        cmd.CommandText = "Insert into Stock_Yarn_Processing_Details ( Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Entry_ID, Particulars, Party_Bill_No, Detail_SlNo,Sl_No, Count_IdNo, Yarn_Type, Mill_IdNo, Bags, Cones, Weight) Values ('" & Trim(Pk_Condition2) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_BillNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_BillNo.Text))) & ", @WagesDate, 0, " & Str(Val(Wev_ID)) & ", '" & Trim(EntID) & "', '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "'," & Val(.Rows(j).Cells(0).Value) & ", " & Str(Val(Sno4)) & " , " & Str(Val(KuriCnt_ID)) & ", 'MILL', 0, 0, 0, " & Val(.Rows(j).Cells(2).Value) & "  )"
                                        cmd.ExecuteNonQuery()

                                    End If

                                End If
                            Next
                        End With
                    End If

                Next

            End With





            'ConsYarn = Val(vTotConsYrnWgt)
            'ConsPavu = Val(Common_Procedures.get_Pavu_Consumption(con, clth_ID, Lm_ID, Val(vTotRcptMtrs), Trim(Wdth_Typ), tr))

            'cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Weaver_Wages_Code = '" & Trim(NewCode) & "', Weaver_Wages_Increment = Weaver_Wages_Increment + 1, ConsumedYarn_Wages = " & Str(Val(ConsYarn)) & ", Consumed_Yarn = " & Str(Val(ConsYarn)) & ", ConsumedPavu_Wages = " & Str(Val(ConsPavu)) & ", Consumed_Pavu = " & Str(Val(ConsPavu)) & ",Report_Particulars_Wages = '" & Trim(Rep_Partcls_Wages) & "', Report_Particulars = '" & Trim(Rep_Partcls_Wages) & "', Type1_Wages_Meters = " & Str(Val(SOUND_MTR)) & ", Type2_Wages_Meters = " & Str(Val(SECOND_MTR)) & ", Type3_Wages_Meters = " & Str(Val(BIT_MTR)) & ", Type4_Wages_Meters = " & Str(Val(OTHER_MTR)) & ", Type5_Wages_Meters = " & Str(Val(REJECT_MTR)) & ", Total_Wages_Meters = " & Str(Val(vTotWgsMtrs)) & " Where Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "'"
            'cmd.ExecuteNonQuery()

            '--START THANGES - 2018-04-28

            Rep_Partcls_Wages = "Wea.Wages : Bill.No. " & Trim(lbl_BillNo.Text)
            If Trim(txt_PartyDcNo.Text) <> "" Then
                Rep_Partcls_Wages = Trim(Rep_Partcls_Wages) & ",  P.Dc.No : " & Trim(txt_PartyDcNo.Text)
            End If


            cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Report_Particulars_Wages = '" & Trim(Rep_Partcls_Wages) & "', Report_Particulars = '" & Trim(Rep_Partcls_Wages) & "'  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            '--END THANGES - 2018-04-28

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), tr)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition_WPYMT) & Trim(NewCode), tr)


            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(PkCode), tr)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(PkCode), tr)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition_WPYMT) & Trim(PkCode), tr)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition_WADVP) & Trim(PkCode), tr)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition_WADVD) & Trim(PkCode), tr)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition_WPTDS) & Trim(PkCode), tr)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition_WFRGT) & Trim(PkCode), tr)


            Cr_ID = Wev_ID
            Dr_ID = Common_Procedures.CommonLedger.Weaving_Wages_Ac
            TdsAc_ID = Common_Procedures.CommonLedger.TDS_Payable_Ac
            'TdsAc_ID = Common_Procedures.CommonLedger.TDS_Charges_Ac

            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
            Dim Narr As String = ""

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
                Narr = Trim(txt_PartyDcNo.Text)
            Else
                Narr = Trim(lbl_BillNo.Text)
            End If



            'RCM_Sts = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_GSTinNo", "(Ledger_IdNo = " & Wev_ID & ")", 0, tr)

            'If Trim(RCM_Sts) <> "" Then


            vLed_IdNos = Wev_ID & "|" & Val(Common_Procedures.CommonLedger.Weaving_Wages_Ac) & "|24|25"
            vVou_Amts = Format(Val(lbl_Total_Amount.Text), "#########0.00") & "|" & -1 * Format(Val(lbl_Total_Amount.Text) - Val(lbl_CGST_Amount.Text) - Val(lbl_SGST_Amount.Text), "#########0.00") & "|" & -1 * Format(Val(lbl_CGST_Amount.Text), "#########0.00") & "|" & -1 * Format(Val(lbl_SGST_Amount.Text), "#########0.00")

            '  vVou_Amts = Format(Val(CSng(lbl_Total_Amount.Text)), "#########0.00") & "|" & -1 * Format(Val(CSng(lbl_Total_Amount.Text)) - Val(CSng(lbl_CGST_Amount.Text)) - Val(CSng(lbl_SGST_Amount.Text)), "#########0.00") & "|" & -1 * Format(Val(CSng(lbl_CGST_Amount.Text)), "#########0.00") & "|" & -1 * Format(Val(CSng(lbl_SGST_Amount.Text)), "#########0.00")
            'vVou_Amts = Format(Val(CSng(lbl_Total_Amount.Text)), "#########0.00") & "|" & -1 * Format(Val(CSng(lbl_Total_Amount.Text)) - Val(CSng(lbl_CGST_Amount.Text)) - Val(CSng(lbl_SGST_Amount.Text)), "#########0.00") & "|" & -1 * Format(Val(CSng(lbl_CGST_Amount.Text)), "#########0.00") & "|" & -1 * Format(Val(CSng(lbl_SGST_Amount.Text)), "#########0.00")

            If Common_Procedures.Voucher_Updation(con, "WeaWg.Wages", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(PkCode), Trim(lbl_BillNo.Text), dtp_Date.Value.Date, "Bill No : " & Trim(Narr), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1100" Then
                vLed_IdNos = Val(Common_Procedures.CommonLedger.Weaving_Wages_Ac) & "|" & Wev_ID
                vVou_Amts = Val(CSng(txt_Less_Amount.Text)) & "|" & -1 * (Val(CSng(txt_Less_Amount.Text)))

                If Common_Procedures.Voucher_Updation(con, "WeaWg.Wages.Less", Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(PkCode), Trim(lbl_BillNo.Text), dtp_Date.Value.Date, "Bill No : " & Trim(lbl_BillNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                    Throw New ApplicationException(ErrMsg)
                End If
            End If

            'Else


            '    WevWages_ROff = Format(Val(lbl_Taxable_Value.Text) - Val(txt_Freight_Charge.Text) + Val(txt_Add_Amount.Text) - Val(txt_Less_Amount.Text), "#########0")


            '    'WevWages_ROff = Format((Val(lbl_Total_Amount.Text) - Val(lbl_CGST_Amount.Text) - Val(lbl_SGST_Amount.Text)), "#########0")
            '    'With Out Registeration
            '    '27 - RCM CGST
            '    '28 - RCM SGST

            '    vLed_IdNos = Wev_ID & "|27|28|" & Common_Procedures.CommonLedger.Weaving_Wages_Ac & "|24|25"
            '    vVou_Amts = Format(Val(WevWages_ROff), "#########0.00") & "|" & Format(Val(lbl_CGST_Amount.Text), "##########0.00") & "|" & Format(Val(lbl_SGST_Amount.Text), "###########0.00") & "|" & -1 * Format(Val(WevWages_ROff), "#########0.00") & "|" & -1 * Format(Val(lbl_CGST_Amount.Text), "#########0.00") & "|" & -1 * Format(Val(lbl_SGST_Amount.Text), "#########0.00")

            '    'vVou_Amts = Format(Val(CSng(lbl_Total_Amount.Text) - Val(lbl_CGST_Amount.Text) - Val(lbl_SGST_Amount.Text)), "#########0.00") & "|" & Format(Val(CSng(lbl_CGST_Amount.Text)), "##########0.00") & "|" & Format(Val(CSng(lbl_SGST_Amount.Text)), "###########0.00") & "|" & -1 * Format(Val(CSng(lbl_Total_Amount.Text) - Val(lbl_CGST_Amount.Text) - Val(lbl_SGST_Amount.Text)), "#########0.00") & "|" & -1 * Format(Val(CSng(lbl_CGST_Amount.Text)), "#########0.00") & "|" & -1 * Format(Val(CSng(lbl_SGST_Amount.Text)), "#########0.00")

            '    If Common_Procedures.Voucher_Updation(con, "WeaWg.Wages", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(PkCode), Trim(lbl_BillNo.Text), dtp_Date.Value.Date, "Bill No : " & Trim(Narr), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
            '        Throw New ApplicationException(ErrMsg)
            '    End If

            'End If

            '--Tds A/c Posting
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition_WPTDS) & Trim(PkCode), tr)
            vLed_IdNos = ""
            vVou_Amts = ""
            ErrMsg = ""

            vLed_IdNos = Val(Common_Procedures.CommonLedger.TDS_Payable_Ac) & "|" & Wev_ID
            'vLed_IdNos = Val(Common_Procedures.CommonLedger.TDS_Charges_Ac) & "|" & Wev_ID
            vVou_Amts = Val(CSng(txt_Tds_Amount.Text)) & "|" & -1 * Val(CSng(txt_Tds_Amount.Text))

            If Common_Procedures.Voucher_Updation(con, "WeaWg.Tds", Val(lbl_Company.Tag), Trim(PkCondition_WPTDS) & Trim(PkCode), Trim(lbl_BillNo.Text), dtp_Date.Value.Date, "Bill No : " & Trim(lbl_BillNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                Throw New ApplicationException(ErrMsg)
            End If



            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition_WPYMT) & Trim(PkCode), tr)
            If Val(txt_Paid_Amount.Text) <> 0 Then
                vLed_IdNos = Val(Common_Procedures.CommonLedger.Cash_Ac) & "|" & Val(Wev_ID)
                vVou_Amts = Val(txt_Paid_Amount.Text) & "|" & -1 * Val(txt_Paid_Amount.Text)
                If Common_Procedures.Voucher_Updation(con, "WeaWg.Pymt", Val(lbl_Company.Tag), Trim(PkCondition_WPYMT) & Trim(PkCode), Trim(lbl_BillNo.Text), dtp_Date.Value.Date, "Bill No : " & Trim(lbl_BillNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                    Throw New ApplicationException(ErrMsg)
                End If
            End If


            If Val(Common_Procedures.User.IdNo) = 1 Then
                If chk_Printed.Visible = True Then
                    If chk_Printed.Enabled = True Then
                        Update_PrintOut_Status(tr)
                    End If
                End If
            End If

            tr.Commit()

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_BillNo.Text)
                End If
            Else
                move_record(lbl_BillNo.Text)
            End If

            If SaveAll_STS <> True Then
                MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            End If

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_BillNo.Text)
                End If
            Else
                move_record(lbl_BillNo.Text)
            End If


        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "for saving...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            cmd.Dispose()
            tr.Dispose()

            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()

        End Try

    End Sub

    Private Sub TotalMeter_Calculation()
        Dim tlmtr As Single = 0
        Dim TtConsMtrs As Single = 0

        If NoCalc_Status = True Then Exit Sub

        'tlmtr = Val(txt_Sound_Meter.Text) + Val(txt_Seconds_Meter.Text) + Val(txt_Reject_Meter.Text) + Val(txt_Bits_Meter.Text) + Val(txt_Other_Meter.Text)
        'lbl_Total_Meter.Text = Format(Val(tlmtr), "#########0.00")


        TtConsMtrs = 0

        With dgv_ConsYarnDetails_Total
            If .Rows.Count > 0 Then
                TtConsMtrs = .Rows(0).Cells(0).Value
            End If
        End With


        'With dgv_Details

        '    If .Rows.Count = 1 Then
        '        .Rows(0).Cells(0).Value = Format(Val(lbl_Total_Meter.Text), "##########0.00")

        '    Else
        '        If TtConsMtrs = 0 Or TtConsMtrs = Val(.Rows(0).Cells(0).Value) Then
        '            .Rows(0).Cells(0).Value = Format(Val(lbl_Total_Meter.Text), "##########0.00")

        '        End If

        '    End If

        'End With

        ' Excess_Short_Calculation()

    End Sub

    Private Sub TdsCommision_Calculation()
        Dim tdsamt As String = 0

        If FrmLdSTS = True Then Exit Sub
        If NoCalc_Status = True Then Exit Sub
        If chk_Tds.Checked = False Then
            tdsamt = Math.Ceiling((Val(lbl_Taxable_Value.Text) - Val(txt_Add_Amount.Text) - Val(txt_Less_Amount.Text)) * Val(txt_Tds.Text) / 100)

            txt_Tds_Amount.Text = Format(Val(tdsamt), "########0.00")
        End If
        NetAmount_Calculation()

    End Sub

    Private Sub Weight_Calculation(ByVal CurRow As Integer, ByVal CurCol As Integer)
        Dim ConsYrn As String = 0
        Dim vClo_Mtrs As Single = 0
        Dim Wgt_Mtr As Double = 0
        Dim RdSp As Single = 0
        Dim Pick As Single = 0
        Dim Weft As Single = 0
        Dim Width As Single = 0
        Dim Stk As String = 0

        On Error Resume Next

        If NoCalc_Status = True Then Exit Sub

        With dgv_ConsYarn_Details
            If .Visible Then

                If CurCol = 0 Or CurCol = 1 Or CurCol = 4 Or CurCol = 5 Or CurCol = 6 Or CurCol = 8 Then

                    vClo_Mtrs = Val(.Rows(CurRow).Cells(0).Value)

                    Wgt_Mtr = Val(.Rows(CurRow).Cells(8).Value)

                    'If Val(Wgt_Mtr) <> 0 Then
                    'ConsYrn = Format(Val(vClo_Mtrs) * Val(Wgt_Mtr), "###########0.000")

                    'Else
                    Pick = Val(.Rows(CurRow).Cells(2).Value)
                    Width = Val(.Rows(CurRow).Cells(3).Value)
                    Weft = Val(Common_Procedures.get_FieldValue(con, "Count_Head", "Resultant_Count", "(count_name = '" & Trim(.Rows(CurRow).Cells(5).Value) & "')"))
                    RdSp = 2.5 ' Val(Common_Procedures.get_FieldValue(con, "Cloth_Head", "Cloth_ReedSpace", "(cloth_name = '" & Trim(.Rows(CurRow).Cells(1).Value) & "')"))

                    Stk = (Common_Procedures.get_FieldValue(con, "Cloth_Head", "Stock_In", "(cloth_name = '" & Trim(.Rows(CurRow).Cells(1).Value) & "')"))

                    If Val(Weft) = 0 Then
                        Weft = Val(.Rows(CurRow).Cells(5).Value)
                    End If
                    If Trim(UCase(Stk)) = "PCS" Then
                        ConsYrn = Format(Pick * (Width + RdSp) * 100 / 840, "############0.0000000")
                    Else
                        ConsYrn = Format(Pick * (Width + RdSp) * 110 / 840, "############0.0000000")
                    End If
                    ConsYrn = Math.Floor(Val(ConsYrn))
                    ConsYrn = Format(Val(ConsYrn) / 1000, "############0.00000")
                    ConsYrn = Format(vClo_Mtrs * Val(ConsYrn), "############0.0")

                    'ConsYrn = (vClo_Mtrs * RdSp * Pick * 1.0937) / (84 * 22 * Weft)

                    'End If

                    If Trim(Common_Procedures.settings.CustomerCode) = "1009" Or Trim(Common_Procedures.settings.CustomerCode) = "1032" Or Trim(Common_Procedures.settings.CustomerCode) = "1060" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1090" Then
                        ConsYrn = Format(Val(ConsYrn), "#########0.0")
                        .Rows(CurRow).Cells(9).Value = Format(Val(ConsYrn), "#########0.000")

                    ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then

                        .Rows(CurRow).Cells(9).Value = Format(Val(ConsYrn), "#########0.00")

                    Else
                        .Rows(CurRow).Cells(9).Value = Format(Val(ConsYrn), "#########0.000")

                    End If

                End If

                ' Calculation_Total_ReceiptMeter()
                Calculation_Total_ConsumedYarnDetails()

            End If

        End With
    End Sub

    Private Sub NetAmount_Calculation()

        Dim NtAmt As String = ""

        If NoCalc_Status = True Then Exit Sub

        NtAmt = 0

        NtAmt = Val(lbl_Total_Amount.Text) - Val(txt_Tds_Amount.Text)

        lbl_Net_Amount.Text = Format(Val(NtAmt), "#########0")

        lbl_Net_Amount.Text = Common_Procedures.Currency_Format(Val(CSng(lbl_Net_Amount.Text)))



    End Sub

    'Private Sub Excess_Short_Calculation()
    '    Dim TotWgsMtrs As Single
    '    Dim TotRcMtrs As Single
    '    Dim TotRcPcs As Single
    '    If NoCalc_Status = True Then Exit Sub
    '    ' lbl_Excess_Short.Text = Val(TotWgsMtrs) - Val(TotRcMtrs)
    '    If Val(TotRcPcs) > 0 Then
    '        txt_Elogation.Text = Format(Val(lbl_Excess_Short.Text) / Val(TotRcPcs), "#########0.00")
    '    Else
    '        txt_Elogation.Text = ""
    '    End If
    'End Sub

    Private Sub Total_Amount_Calculation()
        Dim tlamt As String = ""
        Dim TaxAmt As String = ""

        If FrmLdSTS = True Then Exit Sub
        If NoCalc_Status = True Then Exit Sub

        TaxAmt = ""

        lbl_CGST_Amount.Text = Format(Val(lbl_Taxable_Value.Text) * Val(txt_CGST_Percentage.Text) / 100, "##########0.00")
        lbl_SGST_Amount.Text = Format(Val(lbl_Taxable_Value.Text) * Val(txt_SGST_Percentage.Text) / 100, "##########0.00")

        TaxAmt = Format(Val(lbl_Taxable_Value.Text) + Val(lbl_CGST_Amount.Text) + Val(lbl_SGST_Amount.Text), "#########0.00")

        tlamt = Format(Val(TaxAmt) - Val(txt_Freight_Charge.Text) + Val(txt_Add_Amount.Text) - Val(txt_Less_Amount.Text), "#########0.00")

        lbl_Total_Amount.Text = Format(Val(tlamt), "##########0")
        lbl_Total_Amount.Text = Format(Val(lbl_Total_Amount.Text), "##########0.00")




        If chk_TaxAmount_RoundOff_STS.Checked = True Then

            lbl_CGST_Amount.Text = Format(Val(lbl_CGST_Amount.Text), "#########0")
            lbl_CGST_Amount.Text = Format(Val(lbl_CGST_Amount.Text), "#########0.00")

            lbl_SGST_Amount.Text = Format(Val(lbl_SGST_Amount.Text), "#########0")
            lbl_SGST_Amount.Text = Format(Val(lbl_SGST_Amount.Text), "#########0.00")

        End If

        TdsCommision_Calculation()

        NetAmount_Calculation()


    End Sub

    Private Sub Calculation_Total_ConsumedYarnDetails()
        Dim TotMtrs As Single
        Dim TotWgt As Single
        Dim tlmtr As Single = 0
        Dim Total_Meter As Single = 0
        Dim TotRcMtrs As Single
        Dim TotRcPcs As Single
        Dim Ty1Mtrs As Single, Ty2Mtrs As Single, Ty3Mtrs As Single
        Dim Ty1Pcs As Single, Ty2Pcs As Single, Ty3Pcs As Single
        Dim tlPcs As Single = 0
        Dim TotAmt As Single = 0
        Dim TotConsYarn As String = 0
        Dim TotConsBobin As String = 0
        Dim TotConsKuri As String = 0
        Dim TotConsPavu As String = 0
        Dim TotMtrsEshrt As String = 0
        TotMtrs = 0 : TotWgt = 0
        If NoCalc_Status = True Then Exit Sub

        TotRcMtrs = 0 : TotRcPcs = 0 : Ty1Mtrs = 0 : Ty1Pcs = 0 : Ty2Mtrs = 0 : Ty2Pcs = 0 : Ty3Mtrs = 0 : Ty3Pcs = 0
        TotConsYarn = 0
        TotConsBobin = 0
        TotConsKuri = 0

        With dgv_ConsYarn_Details
            For i = 0 To .RowCount - 1
                If Val(.Rows(i).Cells(6).Value) <> 0 Or Val(.Rows(i).Cells(8).Value) <> 0 Then

                    TotRcPcs = TotRcPcs + Val(.Rows(i).Cells(6).Value)
                    TotRcMtrs = TotRcMtrs + Val(.Rows(i).Cells(8).Value)

                    tlPcs = tlPcs + Val(.Rows(i).Cells(9).Value)
                    TotMtrs = TotMtrs + Val(.Rows(i).Cells(10).Value)

                    TotConsYarn = Val(TotConsYarn) + Val(.Rows(i).Cells(11).Value)
                    TotConsBobin = Val(TotConsBobin) + Val(.Rows(i).Cells(12).Value)
                    TotConsKuri = Val(TotConsKuri) + Val(.Rows(i).Cells(13).Value)
                    TotConsPavu = Val(TotConsPavu) + Val(.Rows(i).Cells(15).Value)
                    TotMtrsEshrt = Val(TotMtrsEshrt) + Val(.Rows(i).Cells(16).Value)
                End If
            Next i
        End With


        With dgv_ConsYarnDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(6).Value = Format(Val(TotRcPcs), "########0")
            .Rows(0).Cells(8).Value = Format(Val(TotRcMtrs), "########0.00")
            '.Rows(0).Cells(9).Value = Format(Val(Ty1Pcs), "########0")
            '.Rows(0).Cells(10).Value = Format(Val(Ty1Mtrs), "########0.00")
            '.Rows(0).Cells(11).Value = Format(Val(Ty2Pcs), "########0")
            '.Rows(0).Cells(12).Value = Format(Val(Ty2Mtrs), "########0.00")




            .Rows(0).Cells(9).Value = Format(Val(tlPcs), "########0")
            .Rows(0).Cells(10).Value = Format(Val(TotMtrs), "########0.00")
            '.Rows(0).Cells(18).Value = Format(Val(TotAmt), "########0.00")

            .Rows(0).Cells(11).Value = Format(Val(TotConsYarn), "########0.000")
            .Rows(0).Cells(12).Value = Format(Val(TotConsBobin), "########0.000")
            .Rows(0).Cells(13).Value = Format(Val(TotConsKuri), "########0.000")
            .Rows(0).Cells(15).Value = Format(Val(TotConsPavu), "########0.000")
            .Rows(0).Cells(16).Value = Format(Val(TotMtrsEshrt), "########0.000")
        End With


    End Sub



    Private Sub Calculation_Total_ReceiptMeter()

        Dim TotRcMtrs As Single
        Dim TotRcPcs As Single
        Dim Ty1Mtrs As Single, Ty2Mtrs As Single, Ty3Mtrs As Single
        Dim Ty1Pcs As Single, Ty2Pcs As Single, Ty3Pcs As Single
        Dim tlPcs As Single = 0
        Dim TotAmt As Single = 0
        Dim Total_Meter As Single = 0

        If NoCalc_Status = True Then Exit Sub

        TotRcMtrs = 0 : TotRcPcs = 0 : Ty1Mtrs = 0 : Ty1Pcs = 0 : Ty2Mtrs = 0 : Ty2Pcs = 0 : Ty3Mtrs = 0 : Ty3Pcs = 0
        With dgv_Receipt_Details
            For i = 0 To .RowCount - 1
                If Trim(.Rows(i).Cells(1).Value) <> "" Or Val(.Rows(i).Cells(4).Value) <> 0 Or Val(.Rows(i).Cells(6).Value) <> 0 Then

                    TotRcPcs = TotRcPcs + Val(.Rows(i).Cells(4).Value())
                    TotRcMtrs = TotRcMtrs + Val(.Rows(i).Cells(6).Value())
                    Ty1Pcs = Ty1Pcs + Val(.Rows(i).Cells(7).Value())
                    Ty1Mtrs = Ty1Mtrs + Val(.Rows(i).Cells(8).Value())
                    Ty2Pcs = Ty2Pcs + Val(.Rows(i).Cells(11).Value())
                    Ty2Mtrs = Ty2Mtrs + Val(.Rows(i).Cells(12).Value())

                    tlPcs = tlPcs + Val(.Rows(i).Cells(15).Value())
                    Total_Meter = Total_Meter + Val(.Rows(i).Cells(16).Value())
                    TotAmt = TotAmt + Val(.Rows(i).Cells(17).Value())
                End If
            Next i

        End With

        With dgv_ReceiptDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(4).Value = Format(Val(TotRcPcs), "########0")
            .Rows(0).Cells(6).Value = Format(Val(TotRcMtrs), "########0.00")
            .Rows(0).Cells(7).Value = Format(Val(Ty1Pcs), "########0")
            .Rows(0).Cells(8).Value = Format(Val(Ty1Mtrs), "########0.00")
            .Rows(0).Cells(11).Value = Format(Val(Ty2Pcs), "########0")
            .Rows(0).Cells(12).Value = Format(Val(Ty2Mtrs), "########0.00")

            .Rows(0).Cells(15).Value = Format(Val(tlPcs), "########0")
            .Rows(0).Cells(16).Value = Format(Val(Total_Meter), "########0.00")

            If chk_TaxABLEAmount_RoundOff_STS.Checked = True Then
                .Rows(0).Cells(17).Value = Format(Val(TotAmt), "###########0")
                .Rows(0).Cells(17).Value = Format(Val(.Rows(0).Cells(17).Value), "###########0.00")
            Else
                .Rows(0).Cells(17).Value = Format(Val(TotAmt), "###########0.00")
            End If

            lbl_Taxable_Value.Text = .Rows(0).Cells(17).Value

        End With

        Total_Amount_Calculation()
        TdsCommision_Calculation()

    End Sub

    Private Sub cbo_Weaver_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Weaver.GotFocus
        cbo_Weaver.Tag = cbo_Weaver.Text
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_type = 'WEAVER'  or Ledger_Type = 'GODOWN'  ) and Close_status = 0", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Weaver_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Weaver.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Weaver, dtp_Date, txt_PartyDcNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_type = 'WEAVER'  or Ledger_Type = 'GODOWN' ) and Close_status = 0", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Weaver_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Weaver.KeyPress
        Dim q As Single = 0
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim frt_Lm, Frt_Amt, Tds_Perc As Single
        Dim LedID, NoofLm As Integer
        Dim MxId As Long = 0

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Weaver, txt_PartyDcNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_type = 'WEAVER'  or Ledger_Type = 'GODOWN' ) and Close_status = 0", "(Ledger_idno = 0)")

        If Asc(e.KeyChar) = 13 Then



            frt_Lm = 0
            NoofLm = 0
            Frt_Amt = 0
            Tds_Perc = 0
            LedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Weaver.Text)
            Da = New SqlClient.SqlDataAdapter("select Freight_Loom, NoOf_Looms, Tds_Perc from Ledger_Head Where Ledger_IdNo = " & Str(Val(LedID)), con)
            Dt = New DataTable
            Da.Fill(Dt)

            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    frt_Lm = Dt.Rows(0).Item("Freight_Loom").ToString
                    NoofLm = Dt.Rows(0).Item("NoOf_Looms").ToString
                    Tds_Perc = Dt.Rows(0).Item("Tds_Perc").ToString
                End If
            End If

            Dt.Clear()
            Dt.Dispose()
            Da.Dispose()

            Frt_Amt = Val(frt_Lm) * Val(NoofLm)
            txt_Freight_Charge.Text = Val(Frt_Amt)
            If Val(Tds_Perc) <> 0 Then
                txt_Tds.Text = Val(Tds_Perc)
            End If

        End If

    End Sub

    Private Sub cbo_Weaver_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Weaver.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Weaver_Creation
            Common_Procedures.MDI_LedType = "WEAVER"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Weaver.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MdiParent1
            f.Show()

        End If
    End Sub

    Private Sub dgv_ConsYarn_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_ConsYarn_Details.CellEnter
        Dim Rect As Rectangle
        With dgv_ConsYarn_Details
            If (e.ColumnIndex = 12) Then

                Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                pnl_BobinSelection_ToolTip.Left = .Left + Rect.Left - 50
                pnl_BobinSelection_ToolTip.Top = .Top + Rect.Top + Rect.Height + 3

                pnl_BobinSelection_ToolTip.Visible = True

            Else
                pnl_BobinSelection_ToolTip.Visible = False

            End If

            If (e.ColumnIndex = 13) Then

                Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                pnl_KuriSelection_ToolTip.Left = .Left + Rect.Left - 100
                pnl_KuriSelection_ToolTip.Top = .Top + Rect.Top + Rect.Height + 3

                pnl_KuriSelection_ToolTip.Visible = True

            Else
                pnl_KuriSelection_ToolTip.Visible = False

            End If
        End With
    End Sub




    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_ConsYarn_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_ConsYarn_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_ActCtrlName = dgv_ConsYarn_Details.Name
        dgv_ConsYarn_Details.EditingControl.BackColor = Color.Lime
        dgv_ConsYarn_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub
    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress

        With dgv_ConsYarn_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 0 Or .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                End If
            End If
        End With

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
                Condt = "a.Weaver_Wages_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Weaver_Wages_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Weaver_Wages_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_CountName.Text) <> "" Then
                Clth_IdNo = Common_Procedures.Count_NameToIdNo(con, cbo_Filter_CountName.Text)
            End If


            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Ledger_Idno = " & Str(Val(Led_IdNo)) & ")"
            End If

            If Val(Clth_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " b.Count_IdNo = " & Str(Val(Clth_IdNo))
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, c.Count_Name, e.Ledger_Name, e.Ledger_MainName from Weaver_Wages_Head a left outer join Weaver_Wages_Yarn_Details b on a.Weaver_Wages_Code = b.Weaver_Wages_Code left outer join Count_head c on b.Count_idno = c.Count_idno left outer join Ledger_head e on a.Ledger_idno = e.Ledger_idno where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.Weaver_Wages_Code like '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by Weaver_Wages_Date, for_orderby, Weaver_Wages_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Weaver_Wages_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Weaver_Wages_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Rec_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = dt2.Rows(i).Item("Count_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(6).Value = dt2.Rows(i).Item("Rec_Date").ToString
                    dgv_Filter_Details.Rows(n).Cells(7).Value = Format(Val(dt2.Rows(i).Item("Total_Cooly").ToString), "########0.00")
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
    Private Sub dgv_Filter_Details_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub


    Private Sub txt_Excess_Short_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Paid_Amount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Paid_Amount.KeyDown
        If e.KeyValue = 38 Then
            txt_Tds.Focus()
        End If

        If e.KeyValue = 40 Then
            txt_Remarks.Focus()
        End If
    End Sub

    Private Sub txt_Paid_Amount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Paid_Amount.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then

            txt_Remarks.Focus()
        End If

    End Sub

    Private Sub txt_Add_Amount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Add_Amount.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Bits_Cooly_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub



    Private Sub txt_Folding_Less_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Folding_Less.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Freight_Charge_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Freight_Charge.KeyDown
        'If e.KeyValue = 38 Then
        '    If dgv_ConsYarn_Details.Rows.Count > 0 Then
        '        dgv_ConsYarn_Details.Focus()
        '        dgv_ConsYarn_Details.CurrentCell = dgv_ConsYarn_Details.Rows(0).Cells(0)
        '        dgv_ConsYarn_Details.CurrentCell.Selected = True

        '    Else
        '        txt_Paid_Amount.Focus()

        '    End If
        'End If
        'If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_Freight_Charge_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight_Charge.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Less_Amount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Less_Amount.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Net_Amount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Other_Cooly_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If (e.KeyValue = 40) Then
            If dgv_ConsYarn_Details.Rows.Count > 0 Then
                dgv_ConsYarn_Details.Focus()
                dgv_ConsYarn_Details.CurrentCell = dgv_ConsYarn_Details.Rows(0).Cells(0)

            Else
                txt_Freight_Charge.Focus()

            End If
        End If
    End Sub

    Private Sub txt_Other_Cooly_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            If dgv_ConsYarn_Details.Rows.Count > 0 Then
                dgv_ConsYarn_Details.Focus()
                dgv_ConsYarn_Details.CurrentCell = dgv_ConsYarn_Details.Rows(0).Cells(0)

            Else
                txt_Freight_Charge.Focus()

            End If
        End If
    End Sub

    Private Sub txt_Other_Meter_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Rec_Meter_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Reject_Cooly_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Reject_Meter_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Seconds_Cooly_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Seconds_Meter_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Sound_Cooly_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Sound_Meter_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Tds_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Tds.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Total_Cooly_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Total_Meter_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub cbo_Grid_Count_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Count.GotFocus
        vCbo_ItmNm = Trim(cbo_Grid_Count.Text)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub

    Private Sub cbo_Grid_Count_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Count.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Count, Nothing, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

        With dgv_ConsYarn_Details

            If (e.KeyValue = 38 And cbo_Grid_Count.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)

            End If

            If (e.KeyValue = 40 And cbo_Grid_Count.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End If


        End With
    End Sub

    Private Sub cbo_Grid_Count_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_Count.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Count, Nothing, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

        'If Asc(e.KeyChar) = 13 Then

        '    With dgv_Details

        '        .Focus()
        '        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

        '    End With

        'End If
        If Asc(e.KeyChar) = 13 Then

            e.Handled = True

            With dgv_ConsYarn_Details
                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    '  txt_Elogation.Focus()
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If
            End With

        End If
    End Sub

    Private Sub cbo_Grid_Count_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Count.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Count_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Grid_Count.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MdiParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Grid_Count_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Count.TextChanged
        Try
            If cbo_Grid_Count.Visible Then
                With dgv_ConsYarn_Details
                    If Val(cbo_Grid_Count.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Count.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub



    Private Sub txt_Tds_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Tds.TextChanged

        If chk_Tds.Checked = True Then
            txt_Tds_Amount.Enabled = True
        Else
            txt_Tds_Amount.Enabled = False
            TdsCommision_Calculation()
        End If

    End Sub






    Private Sub txt_Freight_Charge_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Freight_Charge.TextChanged
        Total_Amount_Calculation()
    End Sub

    Private Sub txt_Less_Amount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Less_Amount.TextChanged
        Total_Amount_Calculation()
    End Sub

    Private Sub txt_Add_Amount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Add_Amount.TextChanged
        Total_Amount_Calculation()
    End Sub





    Public Sub print_record() Implements Interface_MDIActions.print_record

        'If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Weaver_Cloth_Rceipt_Wages_Entry, New_Entry) = False Then Exit Sub


        prn_FromNo = Trim(lbl_BillNo.Text)
        prn_ToNo = Trim(lbl_BillNo.Text)

        prn_WagesFrmt = Common_Procedures.settings.WeaverWages_Printing_Format

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1087" Then
        '    pnl_PrintOption2.Visible = True
        '    pnl_Back.Enabled = False

        '    If btn_Print_WithStock.Enabled And btn_Print_WithStock.Visible Then
        '        btn_Print_WithStock.Focus()
        '    End If

        'Else

        txt_PrintRange_FromNo.Text = prn_FromNo
        txt_PrintRange_ToNo.Text = prn_ToNo

        pnl_PrintRange.Visible = True
        pnl_Back.Enabled = False
        'pnl_PrintOption2.Visible = False

        If txt_PrintRange_FromNo.Enabled Then txt_PrintRange_FromNo.Focus()
        'printing_WeaverWages()

    End Sub

    Private Sub printing_WeaverWages()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String = ""
        Dim OrdBy_FrmNo As Single = 0, OrdByToNo As Single = 0
        Dim ps As Printing.PaperSize
        Dim PpSzSTS As Boolean = False
        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            OrdBy_FrmNo = Common_Procedures.OrderBy_CodeToValue(prn_FromNo)
            OrdByToNo = Common_Procedures.OrderBy_CodeToValue(prn_ToNo)

            da1 = New SqlClient.SqlDataAdapter("select * from Weaver_Wages_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and for_orderby between " & Str(Format(Val(OrdBy_FrmNo), "########0.00")) & " and " & Str(Format(Val(OrdByToNo), "########0.00")) & " and Weaver_Wages_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "'  and Weaver_Wages_Code LIKE '" & Trim(Pk_Condition) & "%' Order by for_orderby, Weaver_Wages_No, Weaver_Wages_Code", con)
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
        prn_InpOpts = InputBox("Select    -   0. None" & Chr(13) & "                  1. Original" & Chr(13) & "                  2. Duplicate" & Chr(13) & "                  3. Triplicate" & Chr(13) & "                  4. Extra Copy" & Space(10) & "                  5. All", "FOR INVOICE PRINTING...", "123")
        prn_InpOpts = Replace(Trim(prn_InpOpts), "5", "1234")

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


                AddHandler ppd.Shown, AddressOf PrintPreview_Shown

                ppd.ShowDialog()

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try


        End If

    End Sub
    Private Sub PrintDocument1_EndPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.EndPrint
        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            chk_Printed.Checked = True
            Update_PrintOut_Status()
        End If
    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim cmd As New SqlClient.SqlCommand
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String
        Dim OrdBy_FrmNo As Single = 0, OrdByToNo As Single = 0

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetDt1.Clear()

        prn_Prev_HeadIndx = -100
        prn_HeadIndx = 0
        prn_Count = 0
        prn_Count1 = 0
        cnt = 0
        prn_DetIndx = 0
        prn_DetIndx1 = 0
        prn_DetSNo = 0
        prn_PageCount = 0

        prn_Tot_EBeam_Stk = 0
        prn_Tot_Pavu_Stk = 0
        prn_Tot_Yarn_Stk = 0
        prn_Tot_Amt_Bal = 0

        PrntCnt2ndPageSTS = False

        DeliverySend = 0
        DeliverySnd = 0
        DeliveryBits = 0
        DeliveryRjts = 0
        DeliveryOthrs = 0
        DeliveryMtrs = 0

        Fold = 0

        Try

            cmd.Connection = con

            cmd.CommandText = "truncate table reporttemp"
            cmd.ExecuteNonQuery()

            OrdBy_FrmNo = Common_Procedures.OrderBy_CodeToValue(prn_FromNo)
            OrdByToNo = Common_Procedures.OrderBy_CodeToValue(prn_ToNo)

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Cloth_Name, Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code from Weaver_Wages_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN State_Head Csh ON b.Company_State_IdNo = Csh.State_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_Idno LEFT OUTER JOIN State_Head Lsh ON c.Ledger_State_IdNo = Lsh.State_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo = d.Cloth_Idno Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.for_orderby between " & Str(Format(Val(OrdBy_FrmNo), "########0.00")) & " and " & Str(Format(Val(OrdByToNo), "########0.00")) & " and a.Weaver_Wages_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "'  and a.Weaver_Wages_Code LIKE '" & Trim(Pk_Condition) & "%' Order by a.for_orderby, a.Weaver_Wages_No, a.Weaver_Wages_Code", con)
            'da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Cloth_Name from Weaver_Wages_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_Idno LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo = d.Cloth_Idno Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Wages_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            'If prn_HdDt.Rows.Count > 0 Then
            '    da2 = New SqlClient.SqlDataAdapter("Select a.*,b.Cloth_Name  from Weaver_ClothReceipt_Piece_Details a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_Idno  Where  a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Wages_Code = '" & Trim(NewCode) & "' and a.for_orderby between " & Str(Format(Val(OrdBy_FrmNo), "########0.00")) & " and " & Str(Format(Val(OrdByToNo), "########0.00")) & " Order by fOR_oRDERbY , Weaver_ClothReceipt_No", con)
            '    prn_DetDt = New DataTable
            '    da2.Fill(prn_DetDt)
            '    'Weaver_AllStock_Ledger(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString), prn_HdDt.Rows(0).Item("Weaver_Wages_Date").ToString)

            '    'da2 = New SqlClient.SqlDataAdapter("select Date1, Int3 as Ent_OrderBy, meters1 as for_OrderBy, name2 as Ref_No, name1 as Ref_Code, name3 as Dc_Rec_No, sum(Int6) as EmptyBeam, sum(Meters6) as PavuMtrs, sum(weight1) as YarnWgt, sum(currency1) as amount from reporttemp group by Date1, Int3, meters1, name2, name1, name3 having sum(Int6)  <> 0 or sum(Meters6) <> 0 or sum(weight1) <> 0 or sum(currency1) <> 0 Order by Date1, Int3, meters1, name2, name1", con)
            '    'prn_DetDt = New DataTable
            '    'da2.Fill(prn_DetDt)

            'Else
            '    MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            'End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String
        Dim Dt1 As New DataTable
        'cnt = 0
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub


        If prn_Prev_HeadIndx <> prn_HeadIndx Then
            prn_DetIndx = 0
            prn_DetSNo = 0
            prn_PageNo = 0
            prn_DetMxIndx = 0
            prn_NoofBmDets = 0
            prn_Count = 0
            NewCode = prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Code").ToString
            Da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Cloth_Name, Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code from Weaver_Wages_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN State_Head Csh ON b.Company_State_IdNo = Csh.State_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_Idno LEFT OUTER JOIN State_Head Lsh ON c.Ledger_State_IdNo = Lsh.State_IdNo LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo = d.Cloth_Idno Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & "  and a.Weaver_Wages_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "'  and a.Weaver_Wages_Code LIKE '" & Trim(Pk_Condition) & "%' Order by a.for_orderby, a.Weaver_Wages_No, a.Weaver_Wages_Code", con)

            Dt1 = New DataTable
            Da1.Fill(Dt1)


            If prn_HdDt.Rows.Count > 0 Then
                Da2 = New SqlClient.SqlDataAdapter("Select a.*,b.Cloth_Name,dt.Cons_Yarn as consumed_Thiri,dt.Meters_Excess_Short as Cons_Yarn_Excess_short_Meter from Weaver_ClothReceipt_Piece_Details a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_Idno  LEFT outer JOIN Weaver_Wages_Yarn_Details dt on a.sl_nO = dt.sl_no and a.Weaver_Wages_Code=dt.Weaver_Wages_Code and a.Cloth_Idno= dt.Cloth_Idno  Where  a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Wages_Code = '" & Trim(NewCode) & "'  Order by a.fOR_oRDERbY , a.Weaver_ClothReceipt_No", con)
                '  Da2 = New SqlClient.SqlDataAdapter("Select a.*,b.Cloth_Name  from Weaver_ClothReceipt_Piece_Details a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_Idno  Where  a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Wages_Code = '" & Trim(NewCode) & "'  Order by a.fOR_oRDERbY , a.Weaver_ClothReceipt_No", con)
                prn_DetDt = New DataTable
                Da2.Fill(prn_DetDt)
                'Weaver_AllStock_Ledger(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString), prn_HdDt.Rows(0).Item("Weaver_Wages_Date").ToString)

                'da2 = New SqlClient.SqlDataAdapter("select Date1, Int3 as Ent_OrderBy, meters1 as for_OrderBy, name2 as Ref_No, name1 as Ref_Code, name3 as Dc_Rec_No, sum(Int6) as EmptyBeam, sum(Meters6) as PavuMtrs, sum(weight1) as YarnWgt, sum(currency1) as amount from reporttemp group by Date1, Int3, meters1, name2, name1, name3 having sum(Int6)  <> 0 or sum(Meters6) <> 0 or sum(weight1) <> 0 or sum(currency1) <> 0 Order by Date1, Int3, meters1, name2, name1", con)
                'prn_DetDt = New DataTable
                'da2.Fill(prn_DetDt)

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        End If

        'Da2 = New SqlClient.SqlDataAdapter("select Date1, Int3 as Ent_OrderBy, meters1 as for_OrderBy, name2 as Ref_No, name1 as Ref_Code, name3 as Dc_Rec_No, sum(Int6) as EmptyBeam, sum(Meters6) as PavuMtrs, sum(weight1) as YarnWgt, sum(currency1) as amount from reporttemp group by Date1, Int3, meters1, name2, name1, name3 having sum(Int6)  <> 0 or sum(Meters6) <> 0 or sum(weight1) <> 0 or sum(currency1) <> 0 Order by Date1, Int3, meters1, name2, name1", con)
        'prn_DetDt = New DataTable
        'Da2.Fill(prn_DetDt)

        'Da2 = New SqlClient.SqlDataAdapter("select Date1, Int3 as Ent_OrderBy, meters1 as for_OrderBy, name2 as Ref_No , name3 as Dc_Rec_No, sum(Int6) as EmptyBeam, sum(Meters6) as PavuMtrs, sum(weight1) as YarnWgt, sum(currency1) as amount from reporttemp group by Date1, Int3, meters1, name2,  name3 having sum(Int6)  <> 0 or sum(Meters6) <> 0 or sum(weight1) <> 0 or sum(currency1) <> 0 Order by Date1, Int3, meters1, name2", con)
        'prn_DetDt = New DataTable
        'Da2.Fill(prn_DetDt)

        'Da2 = New SqlClient.SqlDataAdapter("Select a.*  from Weaver_Cloth_Receipt_Head a  Where a.Weaver_Wages_Code = '" & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Code").ToString) & "' Order by fOR_oRDERbY , Weaver_ClothReceipt_No", con)
        'prn_DetDt1 = New DataTable
        'Da2.Fill(prn_DetDt1)

        '  If Trim(UCase(Common_Procedures.settings.WeaverWages_Printing_Format)) = "FORMAT-5" Then
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
        '    Get_Party_DC_No(prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Code").ToString)

        '    If cnt = 0 Then
        '        Printing_Format6_GST(e)
        '        e.HasMorePages = True
        '        Return
        '        '  cnt = cnt + 1
        '    ElseIf cnt = 1 Then
        '        ' Printing_Format6_GST(e)
        '        Printing_Format6_GSTDelivery(e)
        '        'End If

        '    End If
        'Else
        '    If prn_WagesFrmt = "FORMAT-1" Then
        '        Printing_Format1_GST(e)
        '    ElseIf prn_WagesFrmt = "FORMAT-2" Then
        '        Printing_Format2_GST(e)
        '    ElseIf prn_WagesFrmt = "FORMAT-3" Then
        '        Printing_Format6_GST(e)
        '    ElseIf prn_WagesFrmt = "FORMAT-4" Then
        '        Printing_Format6_GSTDelivery(e)
        '    Else
        '        Printing_Format2_GST(e)
        '    End If
        'End If
        ''  Else
        prn_OriDupTri = ""

        If Trim(Common_Procedures.settings.CustomerCode) = "1414" Then
            Printing_Format8_GST_1414(e)
        Else
            Printing_Format7GST(e)
        End If

        ' Printing_Format1_GST(e)
        ' End If






    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        Dim p1Font As Font
        'Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        'Dim I As Integer
        Dim NoofItems_PerPage As Integer, NoofDets As Integer
        Dim TxtHgt As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim C1, C2, C3, C4, C5, W1, W2 As Single
        Dim snd, sec, bit, rjt, otr As Single
        Dim ps As Printing.PaperSize

        p1Font = New Font("Calibri", 11, FontStyle.Bold)

        If Val(Common_Procedures.settings.WeaverWages_Print_2Copy_In_SinglePage) = 1 Then

            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    Exit For
                End If
            Next

        Else

            Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
            e.PageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.Landscape = False

        End If

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '    If ps.Width = 800 And ps.Height = 600 Then
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        PpSzSTS = True
        '        Exit For
        '    End If
        'Next

        'If PpSzSTS = False Then

        '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
        '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '            PrintDocument1.DefaultPageSettings.PaperSize = ps
        '            e.PageSettings.PaperSize = ps
        '            PpSzSTS = True
        '            Exit For
        '        End If
        '    Next

        '    If PpSzSTS = False Then
        '        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '                PrintDocument1.DefaultPageSettings.PaperSize = ps
        '                e.PageSettings.PaperSize = ps
        '                Exit For
        '            End If
        '        Next
        '    End If

        'End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 10 ' 30
            .Right = 60 ' 50
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
        'If PrintDocument1.DefaultPageSettings.Landscape = True Then
        '    With PrintDocument1.DefaultPageSettings.PaperSize
        '        PrintWidth = .Height - TMargin - BMargin
        '        PrintHeight = .Width - RMargin - LMargin
        '        PageWidth = .Height - TMargin
        '        PageHeight = .Width - RMargin
        '    End With
        'End If

        'd1 = e.Graphics.MeasureString("Department   : ", pFont).Width
        'i1 = e.Graphics.MeasureString("Item Name : ", pFont).Width
        'b1 = e.Graphics.MeasureString("Brand Name : ", pFont).Width
        'qn1 = e.Graphics.MeasureString("Quantity (NEW ITEM) : ", pFont).Width
        'qo1 = e.Graphics.MeasureString("Quantity (OLD ITEM) - Usable  : ", pFont).Width
        'qo2 = e.Graphics.MeasureString("Quantity (OLD ITEM) - Scrap : ", pFont).Width

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(35) : ClAr(2) = 250 : ClAr(3) = 50 : ClAr(4) = 50 : ClAr(5) = 50
        ClAr(6) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5))


        C1 = ClAr(1) + ClAr(2)
        C2 = C1 + ClAr(3) + ClAr(4)
        C3 = C2 + ClAr(5)
        C4 = C3 + ClAr(6)
        C5 = C4 + ClAr(7)

        TxtHgt = 19

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            prn_Prev_HeadIndx = prn_HeadIndx

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = NoofDets + 1

                CurY = CurY + TxtHgt
                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "óè‹ ", LMargin + 10, CurY, 0, 0, pFont)
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Cloth_Name").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                snd = Val(prn_HdDt.Rows(prn_HeadIndx).Item("Sound_Meters").ToString) * Val(prn_HdDt.Rows(prn_HeadIndx).Item("Sound_cooly").ToString)
                sec = Val(prn_HdDt.Rows(prn_HeadIndx).Item("Seconds_Meters").ToString) * Val(prn_HdDt.Rows(prn_HeadIndx).Item("Seconds_cooly").ToString)
                bit = Val(prn_HdDt.Rows(prn_HeadIndx).Item("Bits_Meters").ToString) * Val(prn_HdDt.Rows(prn_HeadIndx).Item("Bits_cooly").ToString)
                rjt = Val(prn_HdDt.Rows(prn_HeadIndx).Item("Reject_Meters").ToString) * Val(prn_HdDt.Rows(prn_HeadIndx).Item("Reject_cooly").ToString)
                otr = Val(prn_HdDt.Rows(prn_HeadIndx).Item("Others_Meters").ToString) * Val(prn_HdDt.Rows(prn_HeadIndx).Item("Others_cooly").ToString)

                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "ê¾‡† ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(snd), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " = ", PageWidth - 80, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Sound_Cooly").ToString, PageWidth - 100, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " X ", PageWidth - 170, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Sound_Meters").ToString, PageWidth - 190, CurY, 1, 0, pFont)

                CurY = CurY + TxtHgt
                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "õó¾ e†ì˜ ", LMargin + 10, CurY, 0, 0, pFont)
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Receipt_Meters").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)


                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "ªêè‡†v ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(sec), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " = ", PageWidth - 80, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Seconds_Cooly").ToString, PageWidth - 100, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " X ", PageWidth - 170, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Seconds_Meters").ToString, PageWidth - 190, CurY, 1, 0, pFont)

                CurY = CurY + TxtHgt
                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "õ÷K e†ì˜ ", LMargin + 10, CurY, 0, 0, pFont)
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Excess_Short").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)


                If Val(bit) <> 0 Then
                    pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                    Common_Procedures.Print_To_PrintDocument(e, "îQ óè‹ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    pFont = New Font("Calibri", 11, FontStyle.Regular)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(bit), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " = ", PageWidth - 80, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Bits_Cooly").ToString, PageWidth - 100, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " X ", PageWidth - 170, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Bits_Meters").ToString, PageWidth - 190, CurY, 1, 0, pFont)

                End If

                CurY = CurY + TxtHgt
                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "ªñ£ˆî ªî£¬è ", LMargin + 10, CurY, 0, 0, pFont)
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_cooly").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                If Val(rjt) <> 0 Then
                    pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                    Common_Procedures.Print_To_PrintDocument(e, "èN¾ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    pFont = New Font("Calibri", 11, FontStyle.Regular)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(rjt), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " = ", PageWidth - 80, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Reject_Cooly").ToString, PageWidth - 100, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " X ", PageWidth - 170, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Reject_Meters").ToString, PageWidth - 190, CurY, 1, 0, pFont)
                End If

                CurY = CurY + TxtHgt
                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "õ‡® õ£ì¬è ", LMargin + 10, CurY, 0, 0, pFont)
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Freight_Charge").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                If Val(otr) <> 0 Then
                    pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                    Common_Procedures.Print_To_PrintDocument(e, "Þîó ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    pFont = New Font("Calibri", 11, FontStyle.Regular)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(otr), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " = ", PageWidth - 80, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Others_Cooly").ToString, PageWidth - 100, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " X ", PageWidth - 170, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Others_Meters").ToString, PageWidth - 190, CurY, 1, 0, pFont)
                End If

                CurY = CurY + TxtHgt
                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "H®ˆî‹ ", LMargin + 10, CurY, 0, 0, pFont)
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Less_Amount").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                e.Graphics.DrawLine(Pens.Black, LMargin + C2 + 50, CurY, PageWidth - 10, CurY)

                CurY = CurY + TxtHgt
                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "õK H®ˆî‹ ", LMargin + 10, CurY, 0, 0, pFont)
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Tds_Perc_Calc").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "ªñ£ˆî‹ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                pFont = New Font("Calibri", 11, FontStyle.Regular)

                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_Meters").ToString, PageWidth - 190, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_Cooly").ToString, PageWidth - 10, CurY, 1, 0, pFont)

                CurY = CurY + TxtHgt
                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "Ü†õ£¡v ", LMargin + 10, CurY, 0, 0, pFont)
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Add_Amount").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)


                CurY = CurY + TxtHgt
                e.Graphics.DrawLine(Pens.Black, LMargin + C1 - 100, CurY, LMargin + C1 - 10, CurY)

                e.Graphics.DrawLine(Pens.Black, LMargin + C2 + 50, CurY, PageWidth - 10, CurY)

                CurY = CurY + 10
                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "eF ªî£¬è ", LMargin + 10, CurY, 0, 0, pFont)
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Net_Amount").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)


                W1 = e.Graphics.MeasureString("dv â‡E‚¬è : ", pFont).Width
                W2 = e.Graphics.MeasureString("Ë™ ªêô¾ : ", pFont).Width

                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "dv â‡E‚¬è ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, " :  " & prn_HdDt.Rows(prn_HeadIndx).Item("Pcs").ToString, LMargin + C1 + W1 + 80, CurY, 0, 0, pFont)


                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "Ë™ ªêô¾ ", LMargin + C1 + 250, CurY, 0, 0, pFont)
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, " :  " & prn_HdDt.Rows(prn_HeadIndx).Item("Total_Dgv_Weight").ToString, LMargin + C1 + 250 + W2 + 10, CurY, 0, 0, pFont)

                NoofDets = NoofDets + 1

                prn_DetIndx = prn_DetIndx + 1

                CurY = CurY + TxtHgt + 10

                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))

            End If

            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)



        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        prn_Prev_HeadIndx = prn_HeadIndx
        prn_HeadIndx = prn_HeadIndx + 1

        If prn_HeadIndx <= prn_HdDt.Rows.Count - 1 Then
            e.HasMorePages = True
        Else
            e.HasMorePages = False
        End If

        prn_DetDt.Clear()

        prn_PageNo = 0

        prn_DetIndx = 0
        prn_DetSNo = 0

        prn_Tot_EBeam_Stk = 0
        prn_Tot_Pavu_Stk = 0
        prn_Tot_Yarn_Stk = 0
        prn_Tot_Amt_Bal = 0

    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim Led_Name As String, Led_Add As String, Led_Add1 As String, Led_Add2 As String
        Dim strHeight As Single, strWidth As Single = 0
        Dim C1, C2, S1, W1, W2 As Single
        Dim CurX As Single = 0
        PageNo = PageNo + 1

        CurY = TMargin


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_EMail = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""
        If Trim(UCase(prn_WagesFrmt)) = "FORMAT-1.2" Or Trim(UCase(prn_WagesFrmt)) = "FORMAT-1.3" Then
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_MainName").ToString
            Cmp_Add1 = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString
            Cmp_Add2 = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address4").ToString

        Else
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString
            Cmp_Add1 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString
            Cmp_Add2 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address4").ToString
            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_PhoneNo").ToString) <> "" Then
                Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(prn_HeadIndx).Item("Company_PhoneNo").ToString
            End If
            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_TinNo").ToString) <> "" Then
                Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_TinNo").ToString
            End If
            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_CstNo").ToString) <> "" Then
                Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_CstNo").ToString
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
        End If

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & " / " & Cmp_EMail), LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No), pFont).Width
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
        'p1Font = New Font("Calibri", 10, FontStyle.Bold)
        'CurX = CurX + strWidth
        'Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_PanCap, CurX, CurY, 0, PrintWidth, p1Font)
        'strWidth = e.Graphics.MeasureString("     " & Cmp_PanCap, p1Font).Width
        'CurX = CurX + strWidth
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, CurX, CurY, 0, 0, pFont)


        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        If Trim(UCase(prn_WagesFrmt)) = "FORMAT-1.2" Or Trim(UCase(prn_WagesFrmt)) = "FORMAT-1.3" Then
            Led_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString
            Led_Add1 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString
            Led_Add2 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address4").ToString

            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address4").ToString) <> "" Then
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString) & " " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address4").ToString)
            ElseIf Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString) <> "" Then
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString) & " " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString)
            ElseIf Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString) <> "" Then
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address1").ToString) & " " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString)
            Else
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address1").ToString)
            End If

        Else
            Led_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_MainName").ToString
            Led_Add1 = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString
            Led_Add2 = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address4").ToString

            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address4").ToString) <> "" Then
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString) & " " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address4").ToString)
            ElseIf Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString) <> "" Then
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString) & " " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString)
            ElseIf Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString) <> "" Then
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString) & " " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString)
            Else
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString)
            End If

        End If

        C1 = ClAr(1) + ClAr(2) + 75
        C2 = C1 + ClAr(4) + 100
        S1 = e.Graphics.MeasureString("TO   :    ", pFont).Width
        W1 = e.Graphics.MeasureString("óC¶ ªï‹.   ", pFont).Width
        W2 = e.Graphics.MeasureString("óC¶ «îF   ", pFont).Width

        If Trim(UCase(prn_WagesFrmt)) = "FORMAT-1.3" Then

            CurY = CurY + TxtHgt
            pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "H™ ªï‹. ", LMargin + 10, CurY, 0, 0, pFont)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_BillNo").ToString, LMargin + W1 + 20, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_No").ToString, LMargin + W1 + 20, CurY, 0, 0, pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "H™ «îF ", LMargin + 140 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Date").ToString), "dd-MM-yyyy").ToString, LMargin + 140 + W2 + 20, CurY, 0, 0, pFont)

            pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
                Common_Procedures.Print_To_PrintDocument(e, "ªìLõK ªï‹.", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_No").ToString, LMargin + C1 + W1 + 40, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "óC¶ ªï‹. ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
                Common_Procedures.Print_To_PrintDocument(e, "ªìLõK «îF", LMargin + C2 + +10, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "óC¶ «îF ", LMargin + C2 + +10, CurY, 0, 0, pFont)
            End If

            '  Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Rec_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C2 + W2 + 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_Date").ToString, LMargin + C2 + W2 + 30, CurY, 0, 0, pFont)

            CurY = CurY + 5

        Else

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & Led_Name, LMargin + 10, CurY, 0, 0, p1Font)
            pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "H™ ªï‹. ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_BillNo").ToString, LMargin + C1 + W1 + 20, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_No").ToString, LMargin + C1 + W1 + 20, CurY, 0, 0, pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "H™ «îF ", LMargin + C2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C2 + W2 + 30, CurY, 0, 0, pFont)

            pFont = New Font("Calibri", 11, FontStyle.Regular)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & Led_Add1, LMargin + S1 + 10, CurY, 0, 0, pFont)

            pFont = New Font("Calibri", 11, FontStyle.Regular)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & Led_Add2, LMargin + S1 + 10, CurY, 0, 0, pFont)

            pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
                Common_Procedures.Print_To_PrintDocument(e, "ªìLõK ªï‹.", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_No").ToString, LMargin + C1 + W1 + 40, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "óC¶ ªï‹. ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
                Common_Procedures.Print_To_PrintDocument(e, "ªìLõK «îF", LMargin + C2 + +10, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "óC¶ «îF ", LMargin + C2 + +10, CurY, 0, 0, pFont)
            End If

            'Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Rec_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C2 + W2 + 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_Date").ToString, LMargin + C2 + W2 + 30, CurY, 0, 0, pFont)

        End If

        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        'S1 = e.Graphics.MeasureString("ªî£¬è Þ¼Š¹   : ", pFont).Width
        'CurY = CurY + 10
        'pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
        'Common_Procedures.Print_To_PrintDocument(e, "ªî£¬è Þ¼Š¹   : ", LMargin + 10, CurY, 0, 0, pFont)
        'pFont = New Font("Calibri", 11, FontStyle.Regular)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Net_Amount").ToString & "Cr", LMargin + S1 + 70, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt


        CurY = CurY + 10
        pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, "îPè£ó˜ ¬èªò£Šð‹  ", LMargin + 10, CurY, 0, 0, pFont)

        If Trim(UCase(prn_WagesFrmt)) = "FORMAT-1.2" Or Trim(UCase(prn_WagesFrmt)) = "FORMAT-1.3" Then
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_MainName").ToString
        Else
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString
        End If
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub Printing_Format2(ByRef e As System.Drawing.Printing.PrintPageEventArgs)

        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim OrdBy_FrmNo As Single = 0, OrdByToNo As Single = 0
        Dim pFont As Font
        Dim p1Font As Font
        Dim pTFont As Font
        Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim I As Integer = 0
        Dim NoofItems_PerPage As Integer, NoofDets As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim EntryCode As String
        Dim CurY As Single, Cur1Y As Single
        Dim C1 As Single = 0, C2 As Single = 0, C3 As Single = 0
        Dim C4 As Single = 0, C5 As Single = 0, C6 As Single = 0
        Dim W1, W2 As Single
        Dim snd As Single
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0

        p1Font = New Font("Calibri", 11, FontStyle.Bold)

        PrntCnt = 1
        If Val(Common_Procedures.settings.WeaverWages_Print_2Copy_In_SinglePage) = 1 Then

            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    e.PageSettings.PaperSize = ps
                    Exit For
                End If
            Next

            If PrntCnt2ndPageSTS = False Then
                PrntCnt = 2
            End If

        Else

            'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
            'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
            'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
            'e.PageSettings.PaperSize = pkCustomSize1
            'PrintDocument1.DefaultPageSettings.Landscape = False
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


        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '    If ps.Width = 800 And ps.Height = 600 Then
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        PpSzSTS = True
        '        Exit For
        '    End If
        'Next

        'If PpSzSTS = False Then

        '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
        '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '            PrintDocument1.DefaultPageSettings.PaperSize = ps
        '            e.PageSettings.PaperSize = ps
        '            PpSzSTS = True
        '            Exit For
        '        End If
        '    Next

        '    If PpSzSTS = False Then
        '        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '                PrintDocument1.DefaultPageSettings.PaperSize = ps
        '                e.PageSettings.PaperSize = ps
        '                Exit For
        '            End If
        '        Next
        '    End If

        'End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 10
            .Right = 40
            If Val(Common_Procedures.settings.WeaverWages_Print_2Copy_In_SinglePage) = 1 Then
                .Top = 5
            Else
                .Top = 10 ' 30
            End If
            .Bottom = 25 ' 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 10, FontStyle.Regular)
        pTFont = New Font("TAM_SC_Suvita", 10, FontStyle.Regular)

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

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(90) : ClAr(2) = 40 : ClAr(3) = 65 : ClAr(4) = 65 : ClAr(5) = 70 : ClAr(6) = 70 : ClAr(7) = 70 : ClAr(8) = 70 : ClAr(9) = 100
        ClAr(10) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9))


        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) '285
        C2 = C1 + ClAr(5)  '385

        If Val(Common_Procedures.settings.WeaverWages_Print_2Copy_In_SinglePage) = 1 Then
            TxtHgt = 17.2 ' 18  ' 18.5
        Else
            TxtHgt = 17.25 ' 18  ' 18.5
        End If

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        ' NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_Prev_HeadIndx = prn_HeadIndx

        PrntCnt2ndPageSTS = False
        TpMargin = TMargin

        For PCnt = 1 To PrntCnt

            If Val(Common_Procedures.settings.WeaverWages_Print_2Copy_In_SinglePage) = 1 Then

                If PCnt = 1 Then
                    prn_PageNo = 0

                    prn_DetIndx = 0
                    prn_DetSNo = 0

                    prn_Tot_EBeam_Stk = 0
                    prn_Tot_Pavu_Stk = 0
                    prn_Tot_Yarn_Stk = 0
                    prn_Tot_Amt_Bal = 0

                    TpMargin = TMargin

                Else

                    prn_PageNo = 0

                    prn_DetIndx = 0
                    prn_DetSNo = 0

                    prn_Tot_EBeam_Stk = 0
                    prn_Tot_Pavu_Stk = 0
                    prn_Tot_Yarn_Stk = 0
                    prn_Tot_Amt_Bal = 0

                    TpMargin = 580 + TMargin  ' 600 + TMargin

                End If

            End If

            Try

                If prn_HdDt.Rows.Count > 0 Then

                    Printing_Format2_PageHeader(e, EntryCode, TxtHgt, pFont, pTFont, LMargin, RMargin, TpMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                    NoofDets = NoofDets + 1

                    NoofItems_PerPage = 6
                    If Val(Common_Procedures.settings.WeaverWages_Print_2Copy_In_SinglePage) = 1 Then
                        If prn_DetDt.Rows.Count > NoofItems_PerPage Then
                            NoofItems_PerPage = 37
                        End If
                    End If

                    If prn_PageNo <= 1 Then

                        Cur1Y = CurY
                        CurY = CurY + TxtHgt - 10
                        Common_Procedures.Print_To_PrintDocument(e, "óè‹ ", LMargin + 10, CurY, 0, 0, pTFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Cloth_Name").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "õó¾ e†ì˜ ", LMargin + 10, CurY, 0, 0, pTFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Receipt_Meters").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "õ÷K e†ì˜ ", LMargin + 10, CurY, 0, 0, pTFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Excess_Short").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "ªñ£ˆî ªî£¬è ", LMargin + 10, CurY, 0, 0, pTFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_cooly").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "õ‡® õ£ì¬è ", LMargin + 10, CurY, 0, 0, pTFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Freight_Charge").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "H®ˆî‹ ", LMargin + 10, CurY, 0, 0, pTFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Less_Amount").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "õK H®ˆî‹ ", LMargin + 10, CurY, 0, 0, pTFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Tds_Perc_Calc").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "Ü†õ£¡v ", LMargin + 10, CurY, 0, 0, pTFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Add_Amount").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                        CurY = CurY + TxtHgt
                        e.Graphics.DrawLine(Pens.Black, LMargin + C1 - 100, CurY, LMargin + C1 - 10, CurY)


                        CurY = CurY + 8
                        Common_Procedures.Print_To_PrintDocument(e, "eF ªî£¬è ", LMargin + 10, CurY, 0, 0, pTFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Net_Amount").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)


                        da1 = New SqlClient.SqlDataAdapter("select a.*, c.* from Weaver_Wages_Cooly_Details a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN ClothType_Head c ON a.ClothType_IdNo = c.ClothType_Idno Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and  a.Weaver_Wages_Code = '" & Trim(EntryCode) & "' Order by a.for_orderby, a.Sl_No", con)
                        prn_Cooly = New DataTable
                        da1.Fill(prn_Cooly)

                        If prn_Cooly.Rows.Count > 0 Then
                            For I = 0 To prn_Cooly.Rows.Count - 1


                                ' CurY = CurY + TxtHgt - 10

                                snd = Val(prn_Cooly.Rows(I).Item("ClothType_IdNo").ToString)

                                If Val(snd) = 1 Then
                                    Cur1Y = Cur1Y + TxtHgt
                                    Common_Procedures.Print_To_PrintDocument(e, "ê¾‡† ", LMargin + C1 + 10, Cur1Y, 0, 0, pTFont)

                                ElseIf Val(snd) = 2 Then
                                    Cur1Y = Cur1Y + TxtHgt
                                    Common_Procedures.Print_To_PrintDocument(e, "ªêè‡†v ", LMargin + C1 + 10, Cur1Y, 0, 0, pTFont)

                                ElseIf Val(snd) = 3 Then
                                    Cur1Y = Cur1Y + TxtHgt
                                    Common_Procedures.Print_To_PrintDocument(e, "îQ óè‹ ", LMargin + C1 + 10, Cur1Y, 0, 0, pTFont)

                                ElseIf Val(snd) = 4 Then
                                    Cur1Y = Cur1Y + TxtHgt
                                    Common_Procedures.Print_To_PrintDocument(e, "èN¾ ", LMargin + C1 + 10, Cur1Y, 0, 0, pTFont)

                                ElseIf Val(snd) = 5 Then
                                    Cur1Y = Cur1Y + TxtHgt
                                    Common_Procedures.Print_To_PrintDocument(e, "Þîó ", LMargin + C1 + 10, Cur1Y, 0, 0, pTFont)

                                End If

                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_Cooly.Rows(I).Item("Amount").ToString), "########0.00"), PageWidth - 10, Cur1Y, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, " = ", PageWidth - 80, Cur1Y, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_Cooly.Rows(I).Item("cooly").ToString, PageWidth - 100, Cur1Y, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, " X ", PageWidth - 170, Cur1Y, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_Cooly.Rows(I).Item("Meters").ToString, PageWidth - 190, Cur1Y, 1, 0, pFont)

                            Next

                            Cur1Y = Cur1Y + TxtHgt
                            e.Graphics.DrawLine(Pens.Black, LMargin + C2 + 10, Cur1Y + 5, PageWidth - 10, Cur1Y + 5)


                            Cur1Y = Cur1Y + TxtHgt - 10
                            Common_Procedures.Print_To_PrintDocument(e, "ªñ£ˆî‹ ", LMargin + C1 + 10, Cur1Y, 0, 0, pTFont)

                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_Meters").ToString, PageWidth - 190, Cur1Y, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_Cooly").ToString, PageWidth - 10, Cur1Y, 1, 0, pFont)

                            Cur1Y = Cur1Y + TxtHgt + 10

                            e.Graphics.DrawLine(Pens.Black, LMargin + C2 + 10, Cur1Y - 5, PageWidth - 10, Cur1Y - 5)


                            W1 = e.Graphics.MeasureString("dv â‡E‚¬è : ", pFont).Width
                            W2 = e.Graphics.MeasureString("Ë™ ªêô¾ : ", pFont).Width

                            Common_Procedures.Print_To_PrintDocument(e, "dv â‡E‚¬è ", LMargin + C1 + 10, CurY, 0, 0, pTFont)
                            Common_Procedures.Print_To_PrintDocument(e, " :  " & prn_HdDt.Rows(prn_HeadIndx).Item("Pcs").ToString, LMargin + C1 + W1 + 80, CurY, 0, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, "Ë™ ªêô¾ ", LMargin + C1 + 250, CurY, 0, 0, pTFont)
                            Common_Procedures.Print_To_PrintDocument(e, " :  " & prn_HdDt.Rows(prn_HeadIndx).Item("Total_Dgv_Weight").ToString, LMargin + C1 + 250 + W2 + 10, CurY, 0, 0, pFont)

                        End If

                        CurY = CurY + TxtHgt + 8

                        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                        LnAr(4) = CurY
                        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(3))



                    Else
                        NoofItems_PerPage = 15

                    End If

                    CurY = CurY + TxtHgt - 10
                    Common_Procedures.Print_To_PrintDocument(e, "«îF", LMargin, CurY, 2, ClAr(1), pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, "ªï‹.", LMargin + ClAr(1), CurY, 2, ClAr(2), pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, "d‹ õ/ð", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, "d‹ Þ¼", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, "ð£¾ õ/ð", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, "ð£¾ Þ¼", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, "Ë™ õ/ð", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, "Ë™ Þ¼", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, "ªî£¬è õ/ð", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, "ªî£¬è Þ¼", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pTFont)

                    CurY = CurY + TxtHgt + 5 ' 10
                    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                    LnAr(5) = CurY

                    NoofDets = 0

                    CurY = CurY - 10

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                            If NoofDets >= NoofItems_PerPage Then

                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                If Val(Common_Procedures.settings.WeaverWages_Print_NoNeed_2nd_Page) = 1 Then
                                    Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, pTFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

                                    GoTo LOOP2

                                Else

                                    Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, pTFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                                    e.HasMorePages = True

                                    Return

                                End If

                            End If

                            prn_Tot_EBeam_Stk = prn_Tot_EBeam_Stk + Val(prn_DetDt.Rows(prn_DetIndx).Item("EmptyBeam").ToString)
                            prn_Tot_Pavu_Stk = prn_Tot_Pavu_Stk + Val(prn_DetDt.Rows(prn_DetIndx).Item("PavuMtrs").ToString)
                            prn_Tot_Yarn_Stk = prn_Tot_Yarn_Stk + Val(prn_DetDt.Rows(prn_DetIndx).Item("YarnWgt").ToString)
                            prn_Tot_Amt_Bal = prn_Tot_Amt_Bal + Val(prn_DetDt.Rows(prn_DetIndx).Item("amount").ToString)

                            CurY = CurY + TxtHgt
                            If IsDate(prn_DetDt.Rows(prn_DetIndx).Item("Date1").ToString) = True Then
                                Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_DetDt.Rows(prn_DetIndx).Item("Date1").ToString), "dd-MM-yy").ToString, LMargin + 5, CurY, 0, 0, pFont)
                            Else

                                If Val(prn_DetDt.Rows(prn_DetIndx).Item("Ent_OrderBy").ToString) = 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, "Opening", LMargin + 5, CurY, 0, 0, pFont)
                                End If

                            End If
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Dc_Rec_No").ToString, LMargin + ClAr(1) + 5, CurY, 0, 0, pFont)
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("EmptyBeam").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("EmptyBeam").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 5, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_Tot_EBeam_Stk), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 5, CurY, 1, 0, pFont)
                            End If
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("PavuMtrs").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("PavuMtrs").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_Tot_Pavu_Stk), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)
                            End If
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("YarnWgt").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("YarnWgt").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_Tot_Yarn_Stk), "#########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, pFont)
                            End If
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("amount").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 5, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_Tot_Amt_Bal), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 5, CurY, 1, 0, pFont)
                            End If

                            NoofDets = NoofDets + 1

                            prn_DetIndx = prn_DetIndx + 1

                        Loop

                    End If

                End If

LOOP1:
                CurY = CurY + TxtHgt

                Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, pTFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

            Catch ex As Exception
                MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try

            If Val(Common_Procedures.settings.WeaverWages_Print_2Copy_In_SinglePage) = 1 Then
                If PCnt = 1 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then
                    If prn_DetDt.Rows.Count > 6 Then
                        PrntCnt2ndPageSTS = True
                        e.HasMorePages = True
                        Return
                    End If
                End If
            End If

            PrntCnt2ndPageSTS = False

        Next PCnt

LOOP2:

        prn_HeadIndx = prn_HeadIndx + 1

        If prn_HeadIndx <= prn_HdDt.Rows.Count - 1 Then
            e.HasMorePages = True

        Else
            e.HasMorePages = False

        End If

        prn_DetDt.Clear()

        prn_PageNo = 0

        prn_DetIndx = 0
        prn_DetSNo = 0

        prn_Tot_EBeam_Stk = 0
        prn_Tot_Pavu_Stk = 0
        prn_Tot_Yarn_Stk = 0
        prn_Tot_Amt_Bal = 0

    End Sub

    Private Sub Printing_Format2_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal pTFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim Led_Name As String, Led_Add As String, Led_Add1 As String, Led_Add2 As String
        Dim strHeight As Single
        Dim C1, C2, C3, S1, W1, W2 As Single


        PageNo = PageNo + 1

        CurY = TMargin

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

        If Trim(UCase(prn_WagesFrmt)) = "FORMAT-2.2" Or Trim(UCase(prn_WagesFrmt)) = "FORMAT-2.3" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1020" Then '---- Sri Vijayalakshmi Spinners (Udamalpet)
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_MainName").ToString
            Cmp_Add1 = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString
            Cmp_Add2 = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address4").ToString

        Else
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString
            Cmp_Add1 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString
            Cmp_Add2 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address4").ToString
            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_PhoneNo").ToString) <> "" Then
                Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(prn_HeadIndx).Item("Company_PhoneNo").ToString
            End If
            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_TinNo").ToString) <> "" Then
                Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_TinNo").ToString
            End If
            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_CstNo").ToString) <> "" Then
                Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_CstNo").ToString
            End If

        End If

        CurY = CurY + TxtHgt - 10
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1029" Then '---- Arul Kumaran Textiles (Somanur)
            p1Font = New Font("Calibri", 18, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
            CurY = CurY + strHeight - 1
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1 & " " & Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

            'CurY = CurY + strHeight - 1
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)
            'CurY = CurY + TxtHgt - 1
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
            'CurY = CurY + TxtHgt - 1
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
            'CurY = CurY + TxtHgt - 1
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        End If
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) '285
        C2 = C1 + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8)

        C3 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 20

        S1 = e.Graphics.MeasureString("TO   :    ", pFont).Width
        W1 = e.Graphics.MeasureString("óC¶ ªï‹.   ", pFont).Width
        W2 = e.Graphics.MeasureString("óC¶ «îF   ", pFont).Width

        If Trim(UCase(prn_WagesFrmt)) = "FORMAT-2.2" Or Trim(UCase(prn_WagesFrmt)) = "FORMAT-2.3" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1020" Then '---- Sri Vijayalakshmi Spinners (Udamalpet)
            Led_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString
            Led_Add1 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString
            Led_Add2 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address4").ToString

            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address4").ToString) <> "" Then
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString) & " " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address4").ToString)
            ElseIf Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString) <> "" Then
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString) & " " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString)
            ElseIf Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString) <> "" Then
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address1").ToString) & " " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString)
            Else
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address1").ToString)
            End If

        Else
            Led_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_MainName").ToString
            Led_Add1 = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString
            Led_Add2 = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address4").ToString

            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address4").ToString) <> "" Then
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString) & " " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address4").ToString)
            ElseIf Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString) <> "" Then
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString) & " " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString)
            ElseIf Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString) <> "" Then
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString) & " " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString)
            Else
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString)
            End If

        End If

        If Trim(UCase(prn_WagesFrmt)) = "FORMAT-2.3" Then
            CurY = CurY + TxtHgt
            pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "H™ ªï‹. ", LMargin + 10, CurY, 0, 0, pFont)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_BillNo").ToString, LMargin + W1 + 20, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_No").ToString, LMargin + W1 + 20, CurY, 0, 0, pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "H™ «îF ", LMargin + 140 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Date").ToString), "dd-MM-yyyy").ToString, LMargin + 140 + W2 + 20, CurY, 0, 0, pFont)


            pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
                Common_Procedures.Print_To_PrintDocument(e, "ªìLõK ªï‹.", LMargin + C3 - 65, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_No").ToString, LMargin + C3 - 65 + W1 + 40, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "óC¶ ªï‹. ", LMargin + C3 - 65, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_No").ToString, LMargin + C3 - 65 + W1 + 30, CurY, 0, 0, pFont)
            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
                Common_Procedures.Print_To_PrintDocument(e, "ªìLõK «îF", LMargin + C2 - 20, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "óC¶ «îF ", LMargin + C2 - 20, CurY, 0, 0, pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Rec_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C2 + W2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + 5

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY
            LnAr(4) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + C3 - 75, CurY, LMargin + C3 - 75, LnAr(2))


        Else

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & Led_Name, LMargin + 10, CurY, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "H™ ªï‹. ", LMargin + C3 + 10, CurY, 0, 0, pTFont)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_BillNo").ToString, LMargin + C3 + W1 + 30, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_No").ToString, LMargin + C3 + W1 + 30, CurY, 0, 0, pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "H™ «îF ", LMargin + C2 + 10, CurY, 0, 0, pTFont)
            Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C2 + W2 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & Led_Add, LMargin + S1 + 10, CurY, 0, 0, pFont)


            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
                Common_Procedures.Print_To_PrintDocument(e, "ªìLõK ªï‹.", LMargin + C3 + 10, CurY, 0, 0, pTFont)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_No").ToString, LMargin + C3 + W1 + 40, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "ªìLõK «îF", LMargin + C2 + 10, CurY, 0, 0, pTFont)
                'Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Rec_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C2 + W2 + 40, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_Date").ToString, LMargin + C2 + W2 + 30, CurY, 0, 0, pFont)

            Else
                Common_Procedures.Print_To_PrintDocument(e, "óC¶ ªï‹. ", LMargin + C3 + 10, CurY, 0, 0, pTFont)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_No").ToString, LMargin + C3 + W1 + 30, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "óC¶ «îF ", LMargin + C2 + 10, CurY, 0, 0, pTFont)
                ' Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Rec_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C2 + W2 + 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_Date").ToString, LMargin + C2 + W2 + 30, CurY, 0, 0, pFont)

            End If


            CurY = CurY + TxtHgt + 5 ' 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY
            LnAr(4) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + C3, CurY, LMargin + C3, LnAr(2))

        End If

    End Sub

    Private Sub Printing_Format2_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal pTFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer = 0
        Dim k As Integer = 0
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim S1, C1, C2 As Single
        Dim Tot_EBeam_StkSumry As Single = 0
        Dim prn_Tot_Pavu_StkSumry As String = ""
        Dim prn_Tot_Yarn_StkSumry As String = ""
        Dim prn_Tot_Amt_BalSumry As Single = 0

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        'CurY = CurY + 5
        LnAr(6) = CurY

        'CurY = CurY + 5

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(4))

        CurY = CurY + 10

        If prn_PageNo <= 1 Then

            Tot_EBeam_StkSumry = 0
            prn_Tot_Pavu_StkSumry = ""
            prn_Tot_Yarn_StkSumry = ""
            prn_Tot_Amt_BalSumry = 0

            da1 = New SqlClient.SqlDataAdapter("select sum(int6) from reporttemp where name6 = 'BEAM'", con)
            Dt1 = New DataTable
            da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                    Tot_EBeam_StkSumry = Val(Dt1.Rows(0)(0).ToString)
                End If
            End If
            Dt1.Clear()

            da1 = New SqlClient.SqlDataAdapter("select sum(currency1) from reporttemp where name6 = 'AMOUNT'", con)
            Dt1 = New DataTable
            da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                    prn_Tot_Amt_BalSumry = Val(Dt1.Rows(0)(0).ToString)
                End If
            End If
            Dt1.Clear()

            da1 = New SqlClient.SqlDataAdapter("select name7 as EndsCount, sum(meters6) as PavuMtrs from reporttemp where name6 = 'PAVU' GROUP BY name7 having sum(meters6) <> 0 ORDER BY name7", con)
            Dt1 = New DataTable
            da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                For k = 0 To Dt1.Rows.Count - 1
                    prn_Tot_Pavu_StkSumry = Trim(prn_Tot_Pavu_StkSumry) & IIf(Trim(prn_Tot_Pavu_StkSumry) <> "", ", ", "") & Dt1.Rows(k).Item("EndsCount").ToString & " : " & Dt1.Rows(k).Item("PavuMtrs").ToString
                Next
            End If
            Dt1.Clear()

            da1 = New SqlClient.SqlDataAdapter("select name7 as CountName, sum(weight1) as YarnWgt from reporttemp where name6 = 'YARN' GROUP BY name7 having sum(weight1) <> 0 ORDER BY name7", con)
            Dt1 = New DataTable
            da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                For k = 0 To Dt1.Rows.Count - 1
                    prn_Tot_Yarn_StkSumry = Trim(prn_Tot_Yarn_StkSumry) & IIf(Trim(prn_Tot_Yarn_StkSumry) <> "", ", ", "") & Dt1.Rows(k).Item("CountName").ToString & " : " & Dt1.Rows(k).Item("YarnWgt").ToString
                Next
            End If
            Dt1.Clear()


            C1 = ClAr(1) + ClAr(2) + ClAr(3)
            C2 = C1 + ClAr(4) + ClAr(5)

            S1 = e.Graphics.MeasureString("Ë™  :", pFont).Width

            Common_Procedures.Print_To_PrintDocument(e, "Þ¼Š¹ Mõó‹ :- ", LMargin + 10, CurY, 0, 0, pTFont)

            Common_Procedures.Print_To_PrintDocument(e, "d‹  ", LMargin + C1 + 10, CurY, 0, 0, pTFont)
            Common_Procedures.Print_To_PrintDocument(e, " : " & Val(Tot_EBeam_StkSumry), LMargin + C1 + S1 + 20, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "ªî£¬è  ", LMargin + C2 + 30, CurY, 0, 0, pTFont)
            Common_Procedures.Print_To_PrintDocument(e, "  :  " & Format(Math.Abs(Val(prn_Tot_Amt_BalSumry)), "#########0.00") & IIf(Val(prn_Tot_Amt_BalSumry) < 0, " Dr", " Cr"), LMargin + C2 + 90, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "(" & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Date").ToString), "dd-MM-yyyy").ToString & ")", LMargin + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "ð£¾  ", LMargin + C1 + 10, CurY, 0, 0, pTFont)
            Common_Procedures.Print_To_PrintDocument(e, "  :  " & prn_Tot_Pavu_StkSumry, LMargin + C1 + S1 + 20, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Ë™  ", LMargin + C1 + 10, CurY, 0, 0, pTFont)
            Common_Procedures.Print_To_PrintDocument(e, "  :  " & prn_Tot_Yarn_StkSumry, LMargin + C1 + S1 + 20, CurY, 0, 0, pFont)

        Else
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt

        End If

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "ð£˜® ¬èªò£Šð‹  ", LMargin + 10, CurY, 0, 0, pTFont)

        If Trim(UCase(prn_WagesFrmt)) = "FORMAT-2.2" Or Trim(UCase(prn_WagesFrmt)) = "FORMAT-2.3" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1020" Then '---- Sri Vijayalakshmi Spinners (Udamalpet)
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_MainName").ToString
        Else
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1029" Then '---- Arul Kumaran Textiles (Somanur)
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 15, CurY, 1, 0, pFont)
        Else
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
        End If


        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub
    Private Sub Printing_Format3(ByRef e As System.Drawing.Printing.PrintPageEventArgs)

        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim OrdBy_FrmNo As Single = 0, OrdByToNo As Single = 0
        Dim pFont As Font
        Dim p1Font As Font
        Dim pTFont As Font
        Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim I As Integer = 0
        Dim NoofItems_PerPage As Integer, NoofDets As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim EntryCode As String
        Dim CurY As Single, Cur1Y As Single
        Dim C1 As Single = 0, C2 As Single = 0, C3 As Single = 0
        Dim C4 As Single = 0, C5 As Single = 0, C6 As Single = 0
        Dim W1, W2 As Single
        Dim snd As Single
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0

        p1Font = New Font("Calibri", 11, FontStyle.Bold)

        PrntCnt = 1
        If Val(Common_Procedures.settings.WeaverWages_Print_2Copy_In_SinglePage) = 1 Then

            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    e.PageSettings.PaperSize = ps
                    Exit For
                End If
            Next

            If PrntCnt2ndPageSTS = False Then
                PrntCnt = 2
            End If

        Else

            'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
            'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
            'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
            'e.PageSettings.PaperSize = pkCustomSize1
            'PrintDocument1.DefaultPageSettings.Landscape = False
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


        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '    If ps.Width = 800 And ps.Height = 600 Then
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        PpSzSTS = True
        '        Exit For
        '    End If
        'Next

        'If PpSzSTS = False Then

        '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
        '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '            PrintDocument1.DefaultPageSettings.PaperSize = ps
        '            e.PageSettings.PaperSize = ps
        '            PpSzSTS = True
        '            Exit For
        '        End If
        '    Next

        '    If PpSzSTS = False Then
        '        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '                PrintDocument1.DefaultPageSettings.PaperSize = ps
        '                e.PageSettings.PaperSize = ps
        '                Exit For
        '            End If
        '        Next
        '    End If

        'End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 10
            .Right = 40
            If Val(Common_Procedures.settings.WeaverWages_Print_2Copy_In_SinglePage) = 1 Then
                .Top = 5
            Else
                .Top = 10 ' 30
            End If
            .Bottom = 25 ' 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 10, FontStyle.Regular)
        pTFont = New Font("TAM_SC_Suvita", 10, FontStyle.Regular)

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

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(90) : ClAr(2) = 40 : ClAr(3) = 65 : ClAr(4) = 65 : ClAr(5) = 70 : ClAr(6) = 70 : ClAr(7) = 70 : ClAr(8) = 70 : ClAr(9) = 100
        ClAr(10) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9))


        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) '285
        C2 = C1 + ClAr(5)  '385

        If Val(Common_Procedures.settings.WeaverWages_Print_2Copy_In_SinglePage) = 1 Then
            TxtHgt = 17.2 ' 18  ' 18.5
        Else
            TxtHgt = 17.25 ' 18  ' 18.5
        End If

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        ' NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_Prev_HeadIndx = prn_HeadIndx

        PrntCnt2ndPageSTS = False
        TpMargin = TMargin

        For PCnt = 1 To PrntCnt

            If Val(Common_Procedures.settings.WeaverWages_Print_2Copy_In_SinglePage) = 1 Then

                If PCnt = 1 Then
                    prn_PageNo = 0

                    prn_DetIndx = 0
                    prn_DetSNo = 0

                    prn_Tot_EBeam_Stk = 0
                    prn_Tot_Pavu_Stk = 0
                    prn_Tot_Yarn_Stk = 0
                    prn_Tot_Amt_Bal = 0

                    TpMargin = TMargin

                Else

                    prn_PageNo = 0

                    prn_DetIndx = 0
                    prn_DetSNo = 0

                    prn_Tot_EBeam_Stk = 0
                    prn_Tot_Pavu_Stk = 0
                    prn_Tot_Yarn_Stk = 0
                    prn_Tot_Amt_Bal = 0

                    TpMargin = 580 + TMargin  ' 600 + TMargin

                End If

            End If

            Try

                If prn_HdDt.Rows.Count > 0 Then

                    Printing_Format3_PageHeader(e, EntryCode, TxtHgt, pFont, pTFont, LMargin, RMargin, TpMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                    NoofDets = NoofDets + 1

                    NoofItems_PerPage = 9 '6
                    If Val(Common_Procedures.settings.WeaverWages_Print_2Copy_In_SinglePage) = 1 Then
                        If prn_DetDt.Rows.Count > NoofItems_PerPage Then
                            NoofItems_PerPage = 37
                        End If
                    End If

                    If prn_PageNo <= 1 Then

                        Cur1Y = CurY
                        CurY = CurY + TxtHgt - 10
                        Common_Procedures.Print_To_PrintDocument(e, "óè‹ ", LMargin + 10, CurY, 0, 0, pTFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Cloth_Name").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "õó¾ e†ì˜ ", LMargin + 10, CurY, 0, 0, pTFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Receipt_Meters").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "õ÷K e†ì˜ ", LMargin + 10, CurY, 0, 0, pTFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Excess_Short").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "ªñ£ˆî ªî£¬è ", LMargin + 10, CurY, 0, 0, pTFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_cooly").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "õ‡® õ£ì¬è ", LMargin + 10, CurY, 0, 0, pTFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Freight_Charge").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                        'CurY = CurY + TxtHgt
                        'Common_Procedures.Print_To_PrintDocument(e, "H®ˆî‹ ", LMargin + 10, CurY, 0, 0, pTFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Less_Amount").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "õK H®ˆî‹ ", LMargin + 10, CurY, 0, 0, pTFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Tds_Perc_Calc").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                        'CurY = CurY + TxtHgt
                        'Common_Procedures.Print_To_PrintDocument(e, "Ü†õ£¡v ", LMargin + 10, CurY, 0, 0, pTFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Add_Amount").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                        CurY = CurY + TxtHgt
                        e.Graphics.DrawLine(Pens.Black, LMargin + C1 - 100, CurY, LMargin + C1 - 10, CurY)


                        CurY = CurY + 8
                        Common_Procedures.Print_To_PrintDocument(e, "eF ªî£¬è ", LMargin + 10, CurY, 0, 0, pTFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Net_Amount").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)


                        da1 = New SqlClient.SqlDataAdapter("select a.*, c.* from Weaver_Wages_Cooly_Details a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN ClothType_Head c ON a.ClothType_IdNo = c.ClothType_Idno Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and  a.Weaver_Wages_Code = '" & Trim(EntryCode) & "' Order by a.for_orderby, a.Sl_No", con)
                        prn_Cooly = New DataTable
                        da1.Fill(prn_Cooly)

                        If prn_Cooly.Rows.Count > 0 Then
                            For I = 0 To prn_Cooly.Rows.Count - 1


                                ' CurY = CurY + TxtHgt - 10

                                snd = Val(prn_Cooly.Rows(I).Item("ClothType_IdNo").ToString)

                                If Val(snd) = 1 Then
                                    Cur1Y = Cur1Y + TxtHgt
                                    Common_Procedures.Print_To_PrintDocument(e, "ê¾‡† ", LMargin + C1 + 10, Cur1Y, 0, 0, pTFont)

                                ElseIf Val(snd) = 2 Then
                                    Cur1Y = Cur1Y + TxtHgt
                                    Common_Procedures.Print_To_PrintDocument(e, "ªêè‡†v ", LMargin + C1 + 10, Cur1Y, 0, 0, pTFont)

                                ElseIf Val(snd) = 3 Then
                                    Cur1Y = Cur1Y + TxtHgt
                                    Common_Procedures.Print_To_PrintDocument(e, "îQ óè‹ ", LMargin + C1 + 10, Cur1Y, 0, 0, pTFont)

                                ElseIf Val(snd) = 4 Then
                                    Cur1Y = Cur1Y + TxtHgt
                                    Common_Procedures.Print_To_PrintDocument(e, "èN¾ ", LMargin + C1 + 10, Cur1Y, 0, 0, pTFont)

                                ElseIf Val(snd) = 5 Then
                                    Cur1Y = Cur1Y + TxtHgt
                                    Common_Procedures.Print_To_PrintDocument(e, "Þîó ", LMargin + C1 + 10, Cur1Y, 0, 0, pTFont)

                                End If

                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_Cooly.Rows(I).Item("Amount").ToString), "########0.00"), PageWidth - 10, Cur1Y, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, " = ", PageWidth - 80, Cur1Y, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_Cooly.Rows(I).Item("cooly").ToString, PageWidth - 100, Cur1Y, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, " X ", PageWidth - 170, Cur1Y, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_Cooly.Rows(I).Item("Meters").ToString, PageWidth - 190, Cur1Y, 1, 0, pFont)

                            Next

                            Cur1Y = Cur1Y + TxtHgt
                            e.Graphics.DrawLine(Pens.Black, LMargin + C2 + 10, Cur1Y + 5, PageWidth - 10, Cur1Y + 5)


                            Cur1Y = Cur1Y + TxtHgt - 10
                            Common_Procedures.Print_To_PrintDocument(e, "ªñ£ˆî‹ ", LMargin + C1 + 10, Cur1Y, 0, 0, pTFont)

                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_Meters").ToString, PageWidth - 190, Cur1Y, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_Cooly").ToString, PageWidth - 10, Cur1Y, 1, 0, pFont)

                            Cur1Y = Cur1Y + TxtHgt + 10

                            e.Graphics.DrawLine(Pens.Black, LMargin + C2 + 10, Cur1Y - 5, PageWidth - 10, Cur1Y - 5)


                            W1 = e.Graphics.MeasureString("dv â‡E‚¬è : ", pFont).Width
                            W2 = e.Graphics.MeasureString("Ë™ ªêô¾ : ", pFont).Width

                            Common_Procedures.Print_To_PrintDocument(e, "dv â‡E‚¬è ", LMargin + C1 + 10, CurY, 0, 0, pTFont)
                            Common_Procedures.Print_To_PrintDocument(e, " :  " & prn_HdDt.Rows(prn_HeadIndx).Item("Pcs").ToString, LMargin + C1 + W1 + 80, CurY, 0, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, "Ë™ ªêô¾ ", LMargin + C1 + 250, CurY, 0, 0, pTFont)
                            Common_Procedures.Print_To_PrintDocument(e, " :  " & prn_HdDt.Rows(prn_HeadIndx).Item("Total_Dgv_Weight").ToString, LMargin + C1 + 250 + W2 + 10, CurY, 0, 0, pFont)

                        End If

                        CurY = CurY + TxtHgt + 8

                        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                        LnAr(4) = CurY
                        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(3))



                    Else
                        NoofItems_PerPage = 19 '15

                    End If

                    CurY = CurY + TxtHgt - 10
                    Common_Procedures.Print_To_PrintDocument(e, "«îF", LMargin, CurY, 2, ClAr(1), pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, "ªï‹.", LMargin + ClAr(1), CurY, 2, ClAr(2), pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, "d‹ õ/ð", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, "d‹ Þ¼", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, "ð£¾ õ/ð", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, "ð£¾ Þ¼", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, "Ë™ õ/ð", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, "Ë™ Þ¼", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, "ªî£¬è õ/ð", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, "ªî£¬è Þ¼", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pTFont)

                    CurY = CurY + TxtHgt + 5 ' 10
                    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                    LnAr(5) = CurY

                    NoofDets = 0

                    CurY = CurY - 10

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                            If NoofDets >= NoofItems_PerPage Then

                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                If Val(Common_Procedures.settings.WeaverWages_Print_NoNeed_2nd_Page) = 1 Then
                                    Printing_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, pTFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

                                    GoTo LOOP2

                                Else

                                    Printing_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, pTFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                                    e.HasMorePages = True

                                    Return

                                End If

                            End If

                            prn_Tot_EBeam_Stk = prn_Tot_EBeam_Stk + Val(prn_DetDt.Rows(prn_DetIndx).Item("EmptyBeam").ToString)
                            prn_Tot_Pavu_Stk = prn_Tot_Pavu_Stk + Val(prn_DetDt.Rows(prn_DetIndx).Item("PavuMtrs").ToString)
                            prn_Tot_Yarn_Stk = prn_Tot_Yarn_Stk + Val(prn_DetDt.Rows(prn_DetIndx).Item("YarnWgt").ToString)
                            prn_Tot_Amt_Bal = prn_Tot_Amt_Bal + Val(prn_DetDt.Rows(prn_DetIndx).Item("amount").ToString)

                            CurY = CurY + TxtHgt
                            If IsDate(prn_DetDt.Rows(prn_DetIndx).Item("Date1").ToString) = True Then
                                Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_DetDt.Rows(prn_DetIndx).Item("Date1").ToString), "dd-MM-yy").ToString, LMargin + 5, CurY, 0, 0, pFont)
                            Else

                                If Val(prn_DetDt.Rows(prn_DetIndx).Item("Ent_OrderBy").ToString) = 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, "Opening", LMargin + 5, CurY, 0, 0, pFont)
                                End If

                            End If
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Dc_Rec_No").ToString, LMargin + ClAr(1) + 5, CurY, 0, 0, pFont)
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("EmptyBeam").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("EmptyBeam").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 5, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_Tot_EBeam_Stk), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 5, CurY, 1, 0, pFont)
                            End If
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("PavuMtrs").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("PavuMtrs").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_Tot_Pavu_Stk), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)
                            End If
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("YarnWgt").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("YarnWgt").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_Tot_Yarn_Stk), "#########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, pFont)
                            End If
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("amount").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 5, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_Tot_Amt_Bal), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 5, CurY, 1, 0, pFont)
                            End If

                            NoofDets = NoofDets + 1

                            prn_DetIndx = prn_DetIndx + 1

                        Loop

                    End If

                End If

LOOP1:
                CurY = CurY + TxtHgt

                Printing_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, pTFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

            Catch ex As Exception
                MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try

            If Val(Common_Procedures.settings.WeaverWages_Print_2Copy_In_SinglePage) = 1 Then
                If PCnt = 1 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then
                    If prn_DetDt.Rows.Count > 9 Then
                        PrntCnt2ndPageSTS = True
                        e.HasMorePages = True
                        Return
                    End If
                End If
            End If

            PrntCnt2ndPageSTS = False

        Next PCnt

LOOP2:

        prn_HeadIndx = prn_HeadIndx + 1

        If prn_HeadIndx <= prn_HdDt.Rows.Count - 1 Then
            e.HasMorePages = True

        Else
            e.HasMorePages = False

        End If

        prn_DetDt.Clear()

        prn_PageNo = 0

        prn_DetIndx = 0
        prn_DetSNo = 0

        prn_Tot_EBeam_Stk = 0
        prn_Tot_Pavu_Stk = 0
        prn_Tot_Yarn_Stk = 0
        prn_Tot_Amt_Bal = 0

    End Sub

    Private Sub Printing_Format3_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal pTFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim Led_Name As String, Led_Add As String, Led_Add1 As String, Led_Add2 As String
        Dim strHeight As Single
        Dim C1, C2, C3, S1, W1, W2 As Single


        PageNo = PageNo + 1

        CurY = TMargin

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

        If Trim(UCase(prn_WagesFrmt)) = "FORMAT-2.2" Or Trim(UCase(prn_WagesFrmt)) = "FORMAT-2.3" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1020" Then '---- Sri Vijayalakshmi Spinners (Udamalpet)
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_MainName").ToString
            Cmp_Add1 = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString
            Cmp_Add2 = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address4").ToString

        Else
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString
            Cmp_Add1 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString
            Cmp_Add2 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address4").ToString
            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_PhoneNo").ToString) <> "" Then
                Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(prn_HeadIndx).Item("Company_PhoneNo").ToString
            End If
            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_TinNo").ToString) <> "" Then
                Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_TinNo").ToString
            End If
            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_CstNo").ToString) <> "" Then
                Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_CstNo").ToString
            End If

        End If

        CurY = CurY + TxtHgt - 10
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1029" Then '---- Arul Kumaran Textiles (Somanur)
            p1Font = New Font("Calibri", 18, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
            CurY = CurY + strHeight - 1
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1 & " " & Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

            'CurY = CurY + strHeight - 1
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)
            'CurY = CurY + TxtHgt - 1
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
            'CurY = CurY + TxtHgt - 1
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
            'CurY = CurY + TxtHgt - 1
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        End If
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) '285
        C2 = C1 + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8)

        C3 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 20

        S1 = e.Graphics.MeasureString("TO   :    ", pFont).Width
        W1 = e.Graphics.MeasureString("óC¶ ªï‹.   ", pFont).Width
        W2 = e.Graphics.MeasureString("óC¶ «îF   ", pFont).Width

        If Trim(UCase(prn_WagesFrmt)) = "FORMAT-2.2" Or Trim(UCase(prn_WagesFrmt)) = "FORMAT-2.3" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1020" Then '---- Sri Vijayalakshmi Spinners (Udamalpet)
            Led_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString
            Led_Add1 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString
            Led_Add2 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address4").ToString

            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address4").ToString) <> "" Then
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString) & " " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address4").ToString)
            ElseIf Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString) <> "" Then
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString) & " " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString)
            ElseIf Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString) <> "" Then
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address1").ToString) & " " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString)
            Else
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address1").ToString)
            End If

        Else
            Led_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_MainName").ToString
            Led_Add1 = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString
            Led_Add2 = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address4").ToString

            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address4").ToString) <> "" Then
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString) & " " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address4").ToString)
            ElseIf Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString) <> "" Then
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString) & " " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString)
            ElseIf Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString) <> "" Then
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString) & " " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString)
            Else
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString)
            End If

        End If

        If Trim(UCase(prn_WagesFrmt)) = "FORMAT-2.3" Then
            CurY = CurY + TxtHgt
            pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "H™ ªï‹. ", LMargin + 10, CurY, 0, 0, pFont)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_BillNo").ToString, LMargin + W1 + 20, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_No").ToString, LMargin + W1 + 20, CurY, 0, 0, pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "H™ «îF ", LMargin + 140 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Date").ToString), "dd-MM-yyyy").ToString, LMargin + 140 + W2 + 20, CurY, 0, 0, pFont)


            pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
                Common_Procedures.Print_To_PrintDocument(e, "ªìLõK ªï‹.", LMargin + C3 - 65, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_No").ToString, LMargin + C3 - 65 + W1 + 40, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "óC¶ ªï‹. ", LMargin + C3 - 65, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_No").ToString, LMargin + C3 - 65 + W1 + 30, CurY, 0, 0, pFont)
            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
                Common_Procedures.Print_To_PrintDocument(e, "ªìLõK «îF", LMargin + C2 - 20, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "óC¶ «îF ", LMargin + C2 - 20, CurY, 0, 0, pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Rec_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C2 + W2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + 5

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY
            LnAr(4) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + C3 - 75, CurY, LMargin + C3 - 75, LnAr(2))


        Else

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & Led_Name, LMargin + 10, CurY, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "H™ ªï‹. ", LMargin + C3 + 10, CurY, 0, 0, pTFont)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_BillNo").ToString, LMargin + C3 + W1 + 30, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_No").ToString, LMargin + C3 + W1 + 30, CurY, 0, 0, pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "H™ «îF ", LMargin + C2 + 10, CurY, 0, 0, pTFont)
            Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C2 + W2 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & Led_Add, LMargin + S1 + 10, CurY, 0, 0, pFont)


            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
                Common_Procedures.Print_To_PrintDocument(e, "ªìLõK ªï‹.", LMargin + C3 + 10, CurY, 0, 0, pTFont)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_No").ToString, LMargin + C3 + W1 + 40, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "ªìLõK «îF", LMargin + C2 + 10, CurY, 0, 0, pTFont)
                'Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Rec_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C2 + W2 + 40, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_Date").ToString, LMargin + C2 + W2 + 30, CurY, 0, 0, pFont)

            Else
                Common_Procedures.Print_To_PrintDocument(e, "óC¶ ªï‹. ", LMargin + C3 + 10, CurY, 0, 0, pTFont)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_No").ToString, LMargin + C3 + W1 + 30, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "óC¶ «îF ", LMargin + C2 + 10, CurY, 0, 0, pTFont)
                ' Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Rec_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C2 + W2 + 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_Date").ToString, LMargin + C2 + W2 + 30, CurY, 0, 0, pFont)

            End If


            CurY = CurY + TxtHgt + 5 ' 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY
            LnAr(4) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + C3, CurY, LMargin + C3, LnAr(2))

        End If

    End Sub

    Private Sub Printing_Format3_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal pTFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer = 0
        Dim k As Integer = 0
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim S1, C1, C2 As Single
        Dim Tot_EBeam_StkSumry As Single = 0
        Dim prn_Tot_Pavu_StkSumry As String = ""
        Dim prn_Tot_Yarn_StkSumry As String = ""
        Dim prn_Tot_Amt_BalSumry As Single = 0

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        'CurY = CurY + 5
        LnAr(6) = CurY

        'CurY = CurY + 5

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(4))

        CurY = CurY + 10

        If prn_PageNo <= 1 Then

            Tot_EBeam_StkSumry = 0
            prn_Tot_Pavu_StkSumry = ""
            prn_Tot_Yarn_StkSumry = ""
            prn_Tot_Amt_BalSumry = 0

            da1 = New SqlClient.SqlDataAdapter("select sum(int6) from reporttemp where name6 = 'BEAM'", con)
            Dt1 = New DataTable
            da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                    Tot_EBeam_StkSumry = Val(Dt1.Rows(0)(0).ToString)
                End If
            End If
            Dt1.Clear()

            da1 = New SqlClient.SqlDataAdapter("select sum(currency1) from reporttemp where name6 = 'AMOUNT'", con)
            Dt1 = New DataTable
            da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                    prn_Tot_Amt_BalSumry = Val(Dt1.Rows(0)(0).ToString)
                End If
            End If
            Dt1.Clear()

            da1 = New SqlClient.SqlDataAdapter("select name7 as EndsCount, sum(meters6) as PavuMtrs from reporttemp where name6 = 'PAVU' GROUP BY name7 having sum(meters6) <> 0 ORDER BY name7", con)
            Dt1 = New DataTable
            da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                For k = 0 To Dt1.Rows.Count - 1
                    prn_Tot_Pavu_StkSumry = Trim(prn_Tot_Pavu_StkSumry) & IIf(Trim(prn_Tot_Pavu_StkSumry) <> "", ", ", "") & Dt1.Rows(k).Item("EndsCount").ToString & " : " & Dt1.Rows(k).Item("PavuMtrs").ToString
                Next
            End If
            Dt1.Clear()

            da1 = New SqlClient.SqlDataAdapter("select name7 as CountName, sum(weight1) as YarnWgt from reporttemp where name6 = 'YARN' GROUP BY name7 having sum(weight1) <> 0 ORDER BY name7", con)
            Dt1 = New DataTable
            da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                For k = 0 To Dt1.Rows.Count - 1
                    prn_Tot_Yarn_StkSumry = Trim(prn_Tot_Yarn_StkSumry) & IIf(Trim(prn_Tot_Yarn_StkSumry) <> "", ", ", "") & Dt1.Rows(k).Item("CountName").ToString & " : " & Dt1.Rows(k).Item("YarnWgt").ToString
                Next
            End If
            Dt1.Clear()


            C1 = ClAr(1) + ClAr(2) + ClAr(3)
            C2 = C1 + ClAr(4) + ClAr(5)

            S1 = e.Graphics.MeasureString("Ë™  :", pFont).Width

            Common_Procedures.Print_To_PrintDocument(e, "Þ¼Š¹ Mõó‹ :- ", LMargin + 10, CurY, 0, 0, pTFont)

            Common_Procedures.Print_To_PrintDocument(e, "d‹  ", LMargin + C1 + 10, CurY, 0, 0, pTFont)
            Common_Procedures.Print_To_PrintDocument(e, " : " & Val(Tot_EBeam_StkSumry), LMargin + C1 + S1 + 20, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "ªî£¬è  ", LMargin + C2 + 30, CurY, 0, 0, pTFont)
            Common_Procedures.Print_To_PrintDocument(e, "  :  " & Format(Math.Abs(Val(prn_Tot_Amt_BalSumry)), "#########0.00") & IIf(Val(prn_Tot_Amt_BalSumry) < 0, " Dr", " Cr"), LMargin + C2 + 90, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "(" & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Date").ToString), "dd-MM-yyyy").ToString & ")", LMargin + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "ð£¾  ", LMargin + C1 + 10, CurY, 0, 0, pTFont)
            Common_Procedures.Print_To_PrintDocument(e, "  :  " & prn_Tot_Pavu_StkSumry, LMargin + C1 + S1 + 20, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Ë™  ", LMargin + C1 + 10, CurY, 0, 0, pTFont)
            Common_Procedures.Print_To_PrintDocument(e, "  :  " & prn_Tot_Yarn_StkSumry, LMargin + C1 + S1 + 20, CurY, 0, 0, pFont)

        Else
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt

        End If

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "ð£˜® ¬èªò£Šð‹  ", LMargin + 10, CurY, 0, 0, pTFont)

        If Trim(UCase(prn_WagesFrmt)) = "FORMAT-2.2" Or Trim(UCase(prn_WagesFrmt)) = "FORMAT-2.3" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1020" Then '---- Sri Vijayalakshmi Spinners (Udamalpet)
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_MainName").ToString
        Else
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1029" Then '---- Arul Kumaran Textiles (Somanur)
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 15, CurY, 1, 0, pFont)
        Else
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
        End If


        CurY = CurY + TxtHgt

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub Weaver_AllStock_Ledger(ByVal Led_IdNo As Integer, ByVal Wages_Date As Date)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vDate_To As Date, vDate_From As Date
        Dim CompIDCondt As String
        Dim SqlCondt As String = ""

        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Trim(UCase(Common_Procedures.settings.CompanyName)) = "-----~~~" Then
            CompIDCondt = ""
        End If

        cmd.Connection = con
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@WeaWageDate", Wages_Date.Date)

        vDate_From = Common_Procedures.Company_FromDate
        vDate_To = Wages_Date

        cmd.CommandText = "select max(a.weaver_wages_date) from Weaver_Wages_Head a Where " & Trim(CompIDCondt) & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.ledger_idno = " & Str(Val(Led_IdNo)) & " and a.Weaver_Wages_Date < @WeaWageDate"
        Da1 = New SqlClient.SqlDataAdapter(cmd)
        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then

                If IsDate(Dt1.Rows(0)(0).ToString) = True Then
                    vDate_From = Dt1.Rows(0)(0).ToString
                    vDate_From = DateAdd("d", 1, vDate_From.Date)
                End If

            End If

        End If

        Dt1.Clear()

        cmd.Connection = con

        cmd.CommandText = "Truncate table ReportTemp"
        cmd.ExecuteNonQuery()

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@WeaWageDate", Wages_Date.Date)
        cmd.Parameters.AddWithValue("@fromdate", vDate_From.Date)
        cmd.Parameters.AddWithValue("@todate", vDate_To.Date)

        SqlCondt = Trim(CompIDCondt)
        If Val(Led_IdNo) <> 0 Then
            SqlCondt = Trim(SqlCondt) & IIf(Trim(SqlCondt) <> "", " and ", "") & " tP.Ledger_IdNo = " & Str(Val(Led_IdNo))
        End If

        '-------- Empty Beam,  Empty Bag,  Empty Cone

        cmd.CommandText = "Truncate table ReportTempSub"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into ReportTempSub(Int1) Select (a.Empty_Beam+a.Pavu_Beam) from Stock_Empty_BeamBagCone_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.DeliveryTo_Idno <> 0 and a.DeliveryTo_Idno = tP.Ledger_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date < @fromdate and (a.Empty_Beam <> 0 or a.Pavu_Beam <> 0 )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into ReportTempSub(Int1) Select -1*(a.Empty_Beam+Pavu_Beam) from Stock_Empty_BeamBagCone_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ReceivedFrom_Idno <> 0 and a.ReceivedFrom_Idno = tP.Ledger_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date < @fromdate and (a.Empty_Beam <> 0 or a.Pavu_Beam <> 0)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into reporttemp(int3, name5, name6, Int6) Select 0, 'Opening', 'BEAM', sum(Int1) from ReportTempSub having sum(Int1) <> 0"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into reporttemp(int3, Date1, name1, name2, meters1, name3, name4, name5, name6, Int6) Select 1, a.Reference_Date, a.Reference_Code, a.Reference_No, a.For_OrderBy, a.Party_Bill_No, tP.Ledger_Name, a.Particulars, 'BEAM', (a.Empty_Beam+a.Pavu_Beam) from Stock_Empty_BeamBagCone_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Ledger_Head tP ON a.DeliveryTo_Idno <> 0 and a.DeliveryTo_Idno = tP.Ledger_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date between @fromdate and @todate and (a.Empty_Beam <> 0 or a.Pavu_Beam <> 0 )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into reporttemp(int3, Date1, name1, name2, meters1, name3, name4, name5, name6, Int6) Select 2, a.Reference_Date, a.Reference_Code, a.Reference_No, a.For_OrderBy, a.Party_Bill_No, tP.Ledger_Name, a.Particulars, 'BEAM', -1*abs(a.Empty_Beam+a.Pavu_Beam) from Stock_Empty_BeamBagCone_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ReceivedFrom_Idno <> 0 and a.ReceivedFrom_Idno = tP.Ledger_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date between @fromdate and @todate and (a.Empty_Beam <> 0 or a.Pavu_Beam <> 0)"
        cmd.ExecuteNonQuery()

        '-------- Pavu 

        cmd.CommandText = "Truncate table ReportTempSub"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into ReportTempSub(name1, meters1) Select c.endscount_name, a.Meters from Stock_Pavu_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.DeliveryTo_Idno <> 0 and a.DeliveryTo_Idno = tP.Ledger_IdNo INNER JOIN EndsCount_Head c ON a.EndsCount_IdNo <> 0 and a.EndsCount_IdNo = c.EndsCount_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date < @fromdate and a.Meters <> 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into ReportTempSub(name1, meters1) Select c.endscount_name, -1*a.Meters from Stock_Pavu_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ReceivedFrom_Idno <> 0 and a.ReceivedFrom_Idno = tP.Ledger_IdNo INNER JOIN EndsCount_Head c ON a.EndsCount_IdNo <> 0 and a.EndsCount_IdNo = c.EndsCount_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date < @fromdate and a.Meters <> 0"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into reporttemp(int3, name5, name6, name7, meters6) Select 0, 'Opening', 'PAVU', name1, sum(meters1) from ReportTempSub group by name1 having sum(meters1) <> 0"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into reporttemp(int3, Date1, name1, name2, meters1, name3, name4, name5, name6, name7, meters6) Select 1, a.Reference_Date, a.Reference_Code, a.Reference_No, a.For_OrderBy, a.Party_Bill_No, tP.Ledger_Name, a.Particulars, 'PAVU', c.endscount_name, abs(a.Meters) from Stock_Pavu_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Ledger_Head tP ON a.DeliveryTo_Idno <> 0 and a.DeliveryTo_Idno = tP.Ledger_IdNo INNER JOIN EndsCount_Head c ON a.EndsCount_IdNo <> 0 and a.EndsCount_IdNo = c.EndsCount_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date between @fromdate and @todate and a.Meters <> 0 "
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into reporttemp(int3, Date1, name1, name2, meters1, name3, name4, name5, name6, name7, meters6) Select 2, a.Reference_Date, a.Reference_Code, a.Reference_No, a.For_OrderBy, a.Party_Bill_No, tP.Ledger_Name, a.Particulars, 'PAVU', c.endscount_name, -1*abs(a.Meters) from Stock_Pavu_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ReceivedFrom_Idno <> 0 and a.ReceivedFrom_Idno = tP.Ledger_IdNo INNER JOIN EndsCount_Head c ON a.EndsCount_IdNo <> 0 and a.EndsCount_IdNo = c.EndsCount_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date between @fromdate and @todate and a.Meters <> 0 "
        cmd.ExecuteNonQuery()

        '-------- Yarn

        cmd.CommandText = "Truncate table ReportTempSub"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into ReportTempSub(name1, weight1) Select c.count_name, a.Weight from Stock_Yarn_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.DeliveryTo_Idno <> 0 and a.DeliveryTo_Idno = tP.Ledger_IdNo INNER JOIN Count_Head c ON a.Count_IdNo <> 0 and a.Count_IdNo = c.Count_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date < @fromdate and a.Weight <> 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into ReportTempSub(name1, weight1) Select c.count_name, -1*a.Weight from Stock_Yarn_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ReceivedFrom_Idno <> 0 and a.ReceivedFrom_Idno = tP.Ledger_IdNo INNER JOIN Count_Head c ON a.Count_IdNo <> 0 and a.Count_IdNo = c.Count_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date < @fromdate and a.Weight <> 0"
        cmd.ExecuteNonQuery()





        cmd.CommandText = "Insert into reporttemp(int3, name5, name6, name7, weight1) Select 0, 'Opening', 'YARN', name1, sum(Weight1) from ReportTempSub group by name1 having sum(Weight1) <> 0"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into reporttemp(int3, Date1, name1, name2, meters1, name3, name4, name5, name6, name7, weight1) Select 1, a.Reference_Date, a.Reference_Code, a.Reference_No, a.For_OrderBy, a.Party_Bill_No, tP.Ledger_Name, a.Particulars, 'YARN', c.count_name, abs(a.Weight) from Stock_Yarn_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Ledger_Head tP ON a.DeliveryTo_Idno <> 0 and a.DeliveryTo_Idno = tP.Ledger_IdNo INNER JOIN Count_Head c ON a.Count_IdNo <> 0 and a.Count_IdNo = c.Count_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date between @fromdate and @todate and a.Weight <> 0 "
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into reporttemp(int3, Date1, name1, name2, meters1, name3, name4, name5, name6, name7, weight1) Select 2, a.Reference_Date, a.Reference_Code, a.Reference_No, a.For_OrderBy, a.Party_Bill_No, tP.Ledger_Name, a.Particulars, 'YARN', c.count_name, -1*abs(a.Weight) from Stock_Yarn_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ReceivedFrom_Idno <> 0 and a.ReceivedFrom_Idno = tP.Ledger_IdNo INNER JOIN Count_Head c ON a.Count_IdNo <> 0 and a.Count_IdNo = c.Count_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date between @fromdate and @todate and a.Weight <> 0 "
        cmd.ExecuteNonQuery()

        '-------- Amount

        cmd.CommandText = "Insert into reporttemp(int3, name5, name6, Currency1) Select 0, 'Opening', 'AMOUNT', sum(a.Voucher_Amount) from Voucher_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = tP.Ledger_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Voucher_Date < @fromdate and a.Voucher_Amount <> 0"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into reporttemp(int3, Date1, name1, name2, meters1, name3, name4, name5, name6, Currency1) Select 1, a.Voucher_Date, a.Voucher_Code, a.Voucher_No, a.For_OrderBy, a.Voucher_No, tP.Ledger_Name, replace(left(a.Entry_Identification, len(a.Entry_Identification)-6),'-' + cast(a.Company_Idno as varchar) + '-','-') as Particularss, 'AMOUNT', -1*abs(a.Voucher_Amount) from Voucher_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = tP.Ledger_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Voucher_Date between @fromdate and @todate and a.Voucher_Amount < 0 "
        cmd.ExecuteNonQuery()
        'cmd.CommandText = "Insert into reporttemp(int3, Date1, name1, name2, meters1, name3, name4, name5, name6, Currency1) Select 2, a.Voucher_Date, a.Voucher_Code, a.Voucher_No, a.For_OrderBy, a.Voucher_No, tP.Ledger_Name, replace(left(a.Entry_Identification, len(a.Entry_Identification)-6),'-' + cast(a.Company_Idno as varchar) + '-','-') as Particularss, 'AMOUNT', abs(a.Voucher_Amount) from Voucher_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = tP.Ledger_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Voucher_Date between @fromdate and @todate and a.Voucher_Amount > 0 "
        'cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into reporttemp( int3 , Date1         , name1         , name2       , meters1       , name3      , name4          , name5                              , name6                         , Currency1) " &
                                             "Select 2  , a.Voucher_Date, a.Voucher_Code, a.Voucher_No, a.For_OrderBy, a.Voucher_No, tP.Ledger_Name , replace(left(a.Entry_Identification, len(a.Entry_Identification)-6),'-' + cast(a.Company_Idno as varchar) + '-','-') as Particularss, 'AMOUNT', abs(a.Voucher_Amount) from Voucher_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = tP.Ledger_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Voucher_Date between @fromdate and @todate and a.Voucher_Amount > 0 "
        cmd.ExecuteNonQuery()

    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_type = 'WEAVER'  or Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1 ) and  Close_status = 0 ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_type = 'WEAVER'  or Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1 ) and Close_status = 0 ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_type = 'WEAVER'  or Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1 ) and Close_status = 0 ", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_CountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_CountName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_CountName, cbo_Filter_PartyName, btn_Filter_Show, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")

    End Sub

    Private Sub cbo_Filter_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_CountName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_CountName, btn_Filter_Show, "Count_Head", "Count_Name", "", "(Count_IdNo = 0)")
    End Sub

    Private Sub dgv_Filter_Details_CellDoubleClick1(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown1(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter_Details.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
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



    'Private Sub btn_Print_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Cancel.Click
    '    btn_Close_PrintOption_Click(sender, e)
    'End Sub

    Private Sub btn_Close_PrintOption_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel_PrintOption.Click
        pnl_Back.Enabled = True
        pnl_PrintOption2.Visible = False
    End Sub

    'Private Sub btn_Print_WithStock_WithName_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_WithStock_WithName.Click
    '    prn_WagesFrmt = "FORMAT-2.2"
    '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
    '        txt_PrintRange_FromNo.Text = prn_FromNo
    '        txt_PrintRange_ToNo.Text = prn_ToNo

    '        pnl_PrintRange.Visible = True
    '        pnl_Back.Enabled = False
    '        pnl_PrintOption2.Visible = False

    '        If txt_PrintRange_FromNo.Enabled Then txt_PrintRange_FromNo.Focus()

    '    Else
    '        printing_WeaverWages()
    '        btn_Close_PrintOption_Click(sender, e)

    '    End If
    'End Sub

    'Private Sub btn_Print_WithStock_WithoutName_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_WithStock_WithoutName.Click
    '    prn_WagesFrmt = "FORMAT-2.3"

    '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then

    '        txt_PrintRange_FromNo.Text = prn_FromNo
    '        txt_PrintRange_ToNo.Text = prn_ToNo

    '        pnl_PrintRange.Visible = True
    '        pnl_Back.Enabled = False
    '        pnl_PrintOption2.Visible = False

    '        If txt_PrintRange_FromNo.Enabled Then txt_PrintRange_FromNo.Focus()

    '    Else
    '        printing_WeaverWages()
    '        btn_Close_PrintOption_Click(sender, e)

    '    End If

    'End Sub

    'Private Sub btn_Print_Simple_WithName_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Simple_WithName.Click
    '    prn_WagesFrmt = "FORMAT-1.2"
    '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then

    '        txt_PrintRange_FromNo.Text = prn_FromNo
    '        txt_PrintRange_ToNo.Text = prn_ToNo

    '        pnl_PrintRange.Visible = True
    '        pnl_Back.Enabled = False
    '        pnl_PrintOption2.Visible = False

    '        If txt_PrintRange_FromNo.Enabled Then txt_PrintRange_FromNo.Focus()

    '    Else
    '        printing_WeaverWages()
    '        btn_Close_PrintOption_Click(sender, e)

    '    End If
    'End Sub

    'Private Sub btn_Print_Simple_WithOutName_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Simple_WithOutName.Click
    '    prn_WagesFrmt = "FORMAT-1.3"
    '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then

    '        txt_PrintRange_FromNo.Text = prn_FromNo
    '        txt_PrintRange_ToNo.Text = prn_ToNo

    '        pnl_PrintRange.Visible = True
    '        pnl_Back.Enabled = False
    '        pnl_PrintOption2.Visible = False

    '        If txt_PrintRange_FromNo.Enabled Then txt_PrintRange_FromNo.Focus()

    '    Else
    '        printing_WeaverWages()
    '        btn_Close_PrintOption_Click(sender, e)

    '    End If
    'End Sub

    Private Sub btn_Insert_WeaverBillNo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim led_idno As Integer = 0

        Try

            inpno = InputBox("Enter Weaver Bill No.", "FOR INSERTION...")

            led_idno = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Weaver.Text)

            Da = New SqlClient.SqlDataAdapter("select Weaver_Wages_No from Weaver_Wages_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code LIKE '%/" & Trim(Common_Procedures.FnRange) & "' and Ledger_IdNo = " & Str(Val(led_idno)) & " and Weaver_BillNo = '" & Trim(inpno) & "'", con)
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
                txt_PartyDcNo.Text = Trim(UCase(inpno))

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try

    End Sub

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyValue = 38 Then txt_Paid_Amount.Focus()
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub dtp_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_Date.Text = Date.Today
        End If
    End Sub

    Private Sub btn_Close_PrintRange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_PrintRange.Click
        pnl_Back.Enabled = True
        pnl_PrintRange.Visible = False
    End Sub

    Private Sub btn_Cancel_PrintRange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel_PrintRange.Click
        pnl_Back.Enabled = True
        pnl_PrintRange.Visible = False
    End Sub

    Private Sub btn_Print_PrintRange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_PrintRange.Click
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim I As Integer = 0
        Dim OrdBy_FrmNo As Single = 0, OrdByToNo As Single = 0
        Gst_Status = 0

        prn_DmgAmt_STS = True

        Try

            If Val(txt_PrintRange_FromNo.Text) = 0 Then
                MessageBox.Show("Invalid From No", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txt_PrintRange_FromNo.Focus()
                Exit Sub
            End If

            If Val(txt_PrintRange_ToNo.Text) = 0 Then
                MessageBox.Show("Invalid To No", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txt_PrintRange_ToNo.Focus()
                Exit Sub
            End If

            prn_FromNo = Trim(txt_PrintRange_FromNo.Text)
            prn_ToNo = Trim(txt_PrintRange_ToNo.Text)

            btn_Close_PrintRange_Click(sender, e)

            printing_WeaverWages()

        Catch ex As Exception
            MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT PRINT...")

        Finally
            dt1.Dispose()
            da1.Dispose()

        End Try



    End Sub

    Private Sub txt_PrintRange_ToNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PrintRange_ToNo.KeyDown
        If e.KeyValue = 38 Then txt_PrintRange_FromNo.Focus()
        If e.KeyValue = 40 Then btn_Print_PrintRange.Focus()
    End Sub

    Private Sub txt_PrintRange_ToNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PrintRange_ToNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            btn_Print_PrintRange_Click(sender, e)
        End If
    End Sub

    Private Sub dgv_Receipt_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Receipt_Details.CellEndEdit
        dgv_Reeipt_Details_CellLeave(sender, e)
    End Sub

    Private Sub dgv_Receipt_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Receipt_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Rect As Rectangle

        With dgv_Receipt_Details
            ' dgv_Receipt_Details.Tag = .CurrentCell.Value
            'If Val(.Rows(e.RowIndex).Cells(23).Value) = 0 Then
            '    Set_Max_DetailsSlNo(e.RowIndex, 23)
            'End If

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If



            If e.ColumnIndex = 1 Then

                If cbo_cloth.Visible = False Or Val(cbo_cloth.Tag) <> e.RowIndex Then

                    cbo_cloth.Tag = -100
                    Da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_cloth.DataSource = Dt1
                    cbo_cloth.DisplayMember = "Cloth_Name"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_cloth.Left = .Left + Rect.Left
                    cbo_cloth.Top = .Top + Rect.Top

                    cbo_cloth.Width = Rect.Width
                    cbo_cloth.Height = Rect.Height
                    cbo_cloth.Text = .CurrentCell.Value

                    cbo_cloth.Tag = Val(e.RowIndex)
                    cbo_cloth.Visible = True

                    cbo_cloth.BringToFront()
                    cbo_cloth.Focus()

                Else

                    'If cbo_Cloth.Visible = True Then
                    '    cbo_Cloth.BringToFront()
                    '    cbo_Cloth.Focus()
                    'End If

                End If

            Else
                cbo_cloth.Visible = False

            End If
            'If (e.ColumnIndex = 21) Then

            '    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

            '    pnl_BobinSelection_ToolTip.Left = .Left + Rect.Left - 50
            '    pnl_BobinSelection_ToolTip.Top = .Top + Rect.Top + Rect.Height + 3

            '    pnl_BobinSelection_ToolTip.Visible = True

            'Else
            '    pnl_BobinSelection_ToolTip.Visible = False

            'End If
            'If (e.ColumnIndex = 22) Then

            '    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

            '    pnl_KuriSelection_ToolTip.Left = .Left + Rect.Left - 100
            '    pnl_KuriSelection_ToolTip.Top = .Top + Rect.Top + Rect.Height + 3

            '    pnl_KuriSelection_ToolTip.Visible = True

            'Else
            '    pnl_KuriSelection_ToolTip.Visible = False

            'End If
        End With
    End Sub






    Private Sub dgv_ReceiptDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Receipt_Details.EditingControlShowing
        dgtxt_ReceiptDetails = CType(dgv_Receipt_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_ReeiptDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_ReceiptDetails.Enter
        dgv_ActCtrlName = dgv_Receipt_Details.Name
        dgv_Receipt_Details.EditingControl.BackColor = Color.Lime
        dgv_Receipt_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_ReceiptDetails.SelectAll()
    End Sub
    Private Sub dgtxt_ReceiptDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_ReceiptDetails.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        With dgv_Receipt_Details
            If e.KeyValue = Keys.Delete Then

            End If
        End With
    End Sub
    Private Sub dgtxt_ReceiptDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_ReceiptDetails.KeyPress

        With dgv_Receipt_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                End If
            End If
        End With

    End Sub



    Private Sub dgv_Reeipt_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Receipt_Details.CellLeave
        With dgv_Receipt_Details
            If .CurrentCell.ColumnIndex = 7 Or .CurrentCell.ColumnIndex = 8 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
        End With
    End Sub


    Private Sub dgv_Receipt_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Receipt_Details.CellValueChanged


        Calculation_Grid_Amount_Calculation(e.RowIndex, e.ColumnIndex)


        'Dim da As New SqlClient.SqlDataAdapter
        'Dim dt As New DataTable

        'Dim Clo_Mtrs_Pc As Single = 0
        'Dim CloID As Integer = 0
        'Dim Stkin As String = ""
        'Dim Cloth_Id As Integer

        'On Error Resume Next

        'If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

        'With dgv_Receipt_Details
        '    If .Visible Then




        '        If e.ColumnIndex = 2 Or e.ColumnIndex = 3 Or e.ColumnIndex = 4 Or e.ColumnIndex = 5 Or e.ColumnIndex = 6 Or e.ColumnIndex = 7 Or e.ColumnIndex = 8 Or e.ColumnIndex = 9 Or e.ColumnIndex = 10 Or e.ColumnIndex = 11 Or e.ColumnIndex = 12 Or e.ColumnIndex = 13 Or e.ColumnIndex = 18 Then

        '            CloID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(e.RowIndex).Cells(1).Value)
        '            '  Clo_Mtrs_Pc = Val(Common_Procedures.get_FieldValue(con, "Cloth_Head", "Meters_Pcs", "(Cloth_idno = " & Str(Val(CloID)) & ")"))
        '            Clo_Mtrs_Pc = Val(.Rows(e.RowIndex).Cells(5).Value)
        '            Stkin = Trim(Common_Procedures.get_FieldValue(con, "Cloth_Head", "Stock_In", "(Cloth_idno = " & Str(Val(CloID)) & ")"))
        '            .Rows(e.RowIndex).Cells(7).Value = Format(Val(.Rows(e.RowIndex).Cells(4).Value), "##########0.00")
        '            If Trim(UCase(Stkin)) = "PCS" Then
        '                .Rows(e.RowIndex).Cells(6).Value = Format(Val(Clo_Mtrs_Pc) * Val(.Rows(e.RowIndex).Cells(4).Value), "##########0.00")
        '                .Rows(e.RowIndex).Cells(8).Value = Format(Val(Clo_Mtrs_Pc) * Val(.Rows(e.RowIndex).Cells(7).Value), "##########0.00")
        '                .Rows(e.RowIndex).Cells(12).Value = Format(Val(Clo_Mtrs_Pc) * Val(.Rows(e.RowIndex).Cells(11).Value), "##########0.00")
        '            End If



        '            .Rows(e.RowIndex).Cells(8).Value = Format(Val(.Rows(e.RowIndex).Cells(6).Value), "##########0.00")
        '            .Rows(e.RowIndex).Cells(15).Value = Val(.Rows(e.RowIndex).Cells(7).Value)
        '            .Rows(e.RowIndex).Cells(16).Value = Format(Val(.Rows(e.RowIndex).Cells(8).Value), "##########0.00")

        '            If Trim(UCase(Stkin)) = "METER" Then
        '                .Rows(e.RowIndex).Cells(10).Value = Format(Val(.Rows(e.RowIndex).Cells(8).Value) * Val(.Rows(e.RowIndex).Cells(9).Value), "##########0.00")
        '                .Rows(e.RowIndex).Cells(14).Value = Format(Val(.Rows(e.RowIndex).Cells(12).Value) * Val(.Rows(e.RowIndex).Cells(13).Value), "##########0.00")

        '            Else
        '                .Rows(e.RowIndex).Cells(10).Value = Format(Val(.Rows(e.RowIndex).Cells(7).Value) * Val(.Rows(e.RowIndex).Cells(9).Value), "##########0.00")
        '                .Rows(e.RowIndex).Cells(14).Value = Format(Val(.Rows(e.RowIndex).Cells(11).Value) * Val(.Rows(e.RowIndex).Cells(13).Value), "##########0.00")

        '            End If
        '            .Rows(e.RowIndex).Cells(17).Value = Format(Val(.Rows(e.RowIndex).Cells(10).Value) - (Val(.Rows(e.RowIndex).Cells(14).Value)), "###########0")

        '            .Rows(e.RowIndex).Cells(17).Value = Format(Val(.Rows(e.RowIndex).Cells(17).Value), "###########0")

        '            If chk_TaxABLEAmount_RoundOff_STS.Checked = False Then

        '                .Rows(e.RowIndex).Cells(17).Value = Format(Val(.Rows(e.RowIndex).Cells(10).Value) - (Val(.Rows(e.RowIndex).Cells(14).Value)), "###########0.00")

        '                .Rows(e.RowIndex).Cells(17).Value = Format(Val(.Rows(e.RowIndex).Cells(17).Value), "###########0.00")
        '            End If

        '            'Consumption_Calculation()
        '            ' BobinKuriConsumption_Calculation()
        '            Calculation_Total_ReceiptMeter()
        '            ConsYarn_Details()
        '            Calculation_Total_ConsumedYarnDetails()
        '        End If



        '    End If
        'End With

    End Sub

    Private Sub dgtxt_ReceiptDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_ReceiptDetails.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Receipt_Details_KeyUp(sender, e)
        End If
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dgv_Receipt_Details_KeyUp(sender, e)
        End If
    End Sub


    Private Sub dgtxt_ReceiptDetails_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_ReceiptDetails.TextChanged
        Try
            With dgv_Receipt_Details
                If .Rows.Count > 0 Then
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_ReceiptDetails.Text)
                End If
            End With

        Catch ex As Exception
            '---
        End Try

    End Sub



    Private Sub dgv_Receipt_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Receipt_Details.LostFocus
        On Error Resume Next
        dgv_Receipt_Details.CurrentCell.Selected = False
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

        LastNo = lbl_BillNo.Text

        movefirst_record()
        Timer1.Enabled = True

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        ConsYarn_Details()
        save_record()
        If Trim(UCase(LastNo)) = Trim(UCase(lbl_BillNo.Text)) Then
            Timer1.Enabled = False
            SaveAll_STS = False
            MessageBox.Show("All entries saved sucessfully", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Else
            movenext_record()

        End If
    End Sub

    Private Sub Printing_Format4(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        Dim p1Font As Font
        'Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        'Dim I As Integer
        Dim NoofItems_PerPage As Integer, NoofDets As Integer
        Dim TxtHgt As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim C1, C2, C3, C4, C5, W1, W2 As Single

        Dim ps As Printing.PaperSize

        p1Font = New Font("Calibri", 11, FontStyle.Bold)

        If Val(Common_Procedures.settings.WeaverWages_Print_2Copy_In_SinglePage) = 1 Then

            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    Exit For
                End If
            Next

        Else

            Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
            e.PageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.Landscape = False

        End If

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '    If ps.Width = 800 And ps.Height = 600 Then
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        PpSzSTS = True
        '        Exit For
        '    End If
        'Next

        'If PpSzSTS = False Then

        '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
        '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '            PrintDocument1.DefaultPageSettings.PaperSize = ps
        '            e.PageSettings.PaperSize = ps
        '            PpSzSTS = True
        '            Exit For
        '        End If
        '    Next

        '    If PpSzSTS = False Then
        '        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '                PrintDocument1.DefaultPageSettings.PaperSize = ps
        '                e.PageSettings.PaperSize = ps
        '                Exit For
        '            End If
        '        Next
        '    End If

        'End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 10 ' 30
            .Right = 60 ' 50
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
        'If PrintDocument1.DefaultPageSettings.Landscape = True Then
        '    With PrintDocument1.DefaultPageSettings.PaperSize
        '        PrintWidth = .Height - TMargin - BMargin
        '        PrintHeight = .Width - RMargin - LMargin
        '        PageWidth = .Height - TMargin
        '        PageHeight = .Width - RMargin
        '    End With
        'End If

        'd1 = e.Graphics.MeasureString("Department   : ", pFont).Width
        'i1 = e.Graphics.MeasureString("Item Name : ", pFont).Width
        'b1 = e.Graphics.MeasureString("Brand Name : ", pFont).Width
        'qn1 = e.Graphics.MeasureString("Quantity (NEW ITEM) : ", pFont).Width
        'qo1 = e.Graphics.MeasureString("Quantity (OLD ITEM) - Usable  : ", pFont).Width
        'qo2 = e.Graphics.MeasureString("Quantity (OLD ITEM) - Scrap : ", pFont).Width

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(35) : ClAr(2) = 250 : ClAr(3) = 50 : ClAr(4) = 50 : ClAr(5) = 50
        ClAr(6) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5))


        C1 = ClAr(1) + ClAr(2)
        C2 = C1 + ClAr(3) + ClAr(4)
        C3 = C2 + ClAr(5)
        C4 = C3 + ClAr(6)
        C5 = C4 + ClAr(7)

        TxtHgt = 19

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            prn_Prev_HeadIndx = prn_HeadIndx

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format4_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = NoofDets + 1

                pFont = New Font("Calibri", 11, FontStyle.Regular)

                CurY = CurY + TxtHgt
                p1Font = New Font("Calibri", 10, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "Quality :", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Cloth_Name").ToString, LMargin + C1 - 10, CurY, 1, 0, p1Font)

                Common_Procedures.Print_To_PrintDocument(e, WeaverClothType(1), LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(WeaverClothAmount(1)), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " = ", PageWidth - 80, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, WeaverClothCooly(1), PageWidth - 100, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " X ", PageWidth - 170, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, WeaverClothMeters(1), PageWidth - 190, CurY, 1, 0, pFont)

                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Receipt Meters :", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(prn_HdDt.Rows(prn_HeadIndx).Item("Receipt_Meters"), "##########0.000"), LMargin + C1 - 10, CurY, 1, 0, pFont)

                If Val(WeaverClothMeters(2)) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothType(2), LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(WeaverClothAmount(2)), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " = ", PageWidth - 80, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothCooly(2), PageWidth - 100, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " X ", PageWidth - 170, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothMeters(2), PageWidth - 190, CurY, 1, 0, pFont)
                End If


                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Folding Meters :", LMargin + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Meters"), "###########0.000"), LMargin + C1 - 10, CurY, 1, 0, pFont)


                If Val(WeaverClothMeters(3)) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothType(3), LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(WeaverClothAmount(3)), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " = ", PageWidth - 80, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothCooly(3), PageWidth - 100, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " X ", PageWidth - 170, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothMeters(3), PageWidth - 190, CurY, 1, 0, pFont)

                End If
                CurY = CurY + TxtHgt
                'Common_Procedures.Print_To_PrintDocument(e, "Frieght", LMargin + 10, CurY, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Freight_Charge").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                If Val(WeaverClothMeters(4)) <> 0 Then
                    pFont = New Font("Calibri", 11, FontStyle.Regular)
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothType(4), LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(WeaverClothAmount(4)), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " = ", PageWidth - 80, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothCooly(4), PageWidth - 100, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " X ", PageWidth - 170, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothMeters(4), PageWidth - 190, CurY, 1, 0, pFont)

                End If

                CurY = CurY + TxtHgt
                'Common_Procedures.Print_To_PrintDocument(e, "Total", LMargin + 10, CurY, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_cooly").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                If Val(WeaverClothMeters(5)) <> 0 Then
                    pFont = New Font("Calibri", 11, FontStyle.Regular)
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothType(5), LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(WeaverClothAmount(5)), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " = ", PageWidth - 80, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothCooly(5), PageWidth - 100, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " X ", PageWidth - 170, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothMeters(5), PageWidth - 190, CurY, 1, 0, pFont)

                End If

                CurY = CurY + TxtHgt
                'Common_Procedures.Print_To_PrintDocument(e, "Frieght", LMargin + 10, CurY, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Freight_Charge").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)


                If Val(WeaverClothMeters(6)) <> 0 Then
                    pFont = New Font("Calibri", 11, FontStyle.Regular)
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothType(6), LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(WeaverClothAmount(6)), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " = ", PageWidth - 80, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothCooly(6), PageWidth - 100, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " X ", PageWidth - 170, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothMeters(6), PageWidth - 190, CurY, 1, 0, pFont)
                End If

                'CurY = CurY + TxtHgt
                'Common_Procedures.Print_To_PrintDocument(e, "Less", LMargin + 10, CurY, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Less_Amount").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                e.Graphics.DrawLine(Pens.Black, LMargin + C2 + 50, CurY, PageWidth - 10, CurY)

                CurY = CurY + TxtHgt
                'Common_Procedures.Print_To_PrintDocument(e, "Tds", LMargin + 10, CurY, 0, 0, pFont)
                'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Tds_Perc_Calc").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                'pFont = New Font("Calibri", 11, FontStyle.Regular)
                'Common_Procedures.Print_To_PrintDocument(e, "Total", LMargin + C1 + 10, CurY, 0, 0, pFont)
                'pFont = New Font("Calibri", 11, FontStyle.Regular)

                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_Meters").ToString, PageWidth - 190, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_Cooly").ToString, PageWidth - 10, CurY, 1, 0, pFont)

                CurY = CurY + TxtHgt
                'pFont = New Font("Calibri", 11, FontStyle.Regular)
                'Common_Procedures.Print_To_PrintDocument(e, "Add", LMargin + 10, CurY, 0, 0, pFont)
                'pFont = New Font("Calibri", 11, FontStyle.Regular)
                'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Add_Amount").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)


                CurY = CurY + TxtHgt
                e.Graphics.DrawLine(Pens.Black, LMargin + C1 - 100, CurY, LMargin + C1 - 10, CurY)

                e.Graphics.DrawLine(Pens.Black, LMargin + C2 + 50, CurY, PageWidth - 10, CurY)

                CurY = CurY + 10
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + 10, CurY, 0, 0, pFont)
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Net_Amount").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)


                W1 = e.Graphics.MeasureString("Pcs :", pFont).Width
                W2 = e.Graphics.MeasureString("Yarn Cons : ", pFont).Width

                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "Pcs", LMargin + C1 + 10, CurY, 0, 0, pFont)
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, " :  " & prn_HdDt.Rows(prn_HeadIndx).Item("Pcs").ToString, LMargin + C1 + W1 + 80, CurY, 0, 0, pFont)


                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "Yarn Cons", LMargin + C1 + 250, CurY, 0, 0, pFont)
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, " :  " & prn_HdDt.Rows(prn_HeadIndx).Item("Total_Dgv_Weight").ToString, LMargin + C1 + 250 + W2 + 10, CurY, 0, 0, pFont)

                NoofDets = NoofDets + 1

                prn_DetIndx = prn_DetIndx + 1

                CurY = CurY + TxtHgt + 10

                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))

            End If

            Printing_Format4_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)



        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        prn_Prev_HeadIndx = prn_HeadIndx
        prn_HeadIndx = prn_HeadIndx + 1

        If prn_HeadIndx <= prn_HdDt.Rows.Count - 1 Then
            e.HasMorePages = True
        Else
            e.HasMorePages = False
        End If

        prn_DetDt.Clear()

        prn_PageNo = 0

        prn_DetIndx = 0
        prn_DetSNo = 0

        prn_Tot_EBeam_Stk = 0
        prn_Tot_Pavu_Stk = 0
        prn_Tot_Yarn_Stk = 0
        prn_Tot_Amt_Bal = 0

    End Sub

    Private Sub Printing_Format4_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim Led_Name As String, Led_Add As String, Led_Add1 As String, Led_Add2 As String
        Dim strHeight As Single
        Dim C1, C2, S1, W1, W2 As Single


        PageNo = PageNo + 1

        CurY = TMargin


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

        If Trim(UCase(prn_WagesFrmt)) = "FORMAT-1.2" Or Trim(UCase(prn_WagesFrmt)) = "FORMAT-1.3" Then
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_MainName").ToString
            Cmp_Add1 = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString
            Cmp_Add2 = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address4").ToString

        Else
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString
            Cmp_Add1 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString
            Cmp_Add2 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address4").ToString
            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_PhoneNo").ToString) <> "" Then
                Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(prn_HeadIndx).Item("Company_PhoneNo").ToString
            End If
            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_TinNo").ToString) <> "" Then
                Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_TinNo").ToString
            End If
            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_CstNo").ToString) <> "" Then
                Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_CstNo").ToString
            End If

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

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        If Trim(UCase(prn_WagesFrmt)) = "FORMAT-1.2" Or Trim(UCase(prn_WagesFrmt)) = "FORMAT-1.3" Then
            Led_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString
            Led_Add1 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString
            Led_Add2 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address4").ToString

            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address4").ToString) <> "" Then
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString) & " " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address4").ToString)
            ElseIf Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString) <> "" Then
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString) & " " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString)
            ElseIf Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString) <> "" Then
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address1").ToString) & " " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString)
            Else
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address1").ToString)
            End If

        Else
            Led_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_MainName").ToString
            Led_Add1 = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString
            Led_Add2 = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address4").ToString

            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address4").ToString) <> "" Then
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString) & " " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address4").ToString)
            ElseIf Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString) <> "" Then
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString) & " " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString)
            ElseIf Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString) <> "" Then
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString) & " " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString)
            Else
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString)
            End If

        End If

        C1 = ClAr(1) + ClAr(2) + 75
        C2 = C1 + ClAr(4) + 100
        S1 = e.Graphics.MeasureString("TO   :    ", pFont).Width
        W1 = e.Graphics.MeasureString("Bill Date  ", pFont).Width
        W2 = e.Graphics.MeasureString("Bill Date  ", pFont).Width


        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & Led_Name, LMargin + 10, CurY, 0, 0, p1Font)
        pFont = New Font("Calibri", 11, FontStyle.Regular)

        Common_Procedures.Print_To_PrintDocument(e, "Bill No", LMargin + C1 + 10, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_BillNo").ToString, LMargin + C1 + W1 + 20, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "Bill Date", LMargin + C2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C2 + W2 + 30, CurY, 0, 0, pFont)

        pFont = New Font("Calibri", 11, FontStyle.Regular)
        CurY = CurY + TxtHgt

        p1Font = New Font("Calibri", 9, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, " " & Led_Add1, LMargin + S1, CurY, 0, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, "P.Dc.No", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":  " & Party_DC_No_For_Wages, LMargin + C1 + W1 + 20, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, " " & Led_Add2, LMargin + S1, CurY, 0, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, "Dc Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":   " & Party_DC_Date_For_Wages, LMargin + C1 + W1 + 20, CurY, 0, 0, pFont)



        'CurY = CurY + TxtHgt
        'pFont = New Font("Calibri", 11, FontStyle.Regular)
        'Common_Procedures.Print_To_PrintDocument(e, "Bill No", LMargin + C1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_BillNo").ToString, LMargin + C1 + 20, CurY, 0, 0, pFont)


        'Common_Procedures.Print_To_PrintDocument(e, "Bill Date", LMargin + C2 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C2 + W2 + 30, CurY, 0, 0, pFont)

        'CurY = CurY + TxtHgt
        'p1Font = New Font("Calibri", 9, FontStyle.Regular)
        'Common_Procedures.Print_To_PrintDocument(e, " " & Led_Add1, LMargin + S1, CurY, 0, 0, p1Font)

        'Common_Procedures.Print_To_PrintDocument(e, "P.Dc.No", LMargin + C1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":  " & Party_DC_No_For_Wages, LMargin + C1 + W1 + 40, CurY, 0, 0, pFont)

        'pFont = New Font("Calibri", 11, FontStyle.Regular)
        'CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, " " & Led_Add2, LMargin + S1, CurY, 0, 0, p1Font)

        'Common_Procedures.Print_To_PrintDocument(e, "Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":   " & Party_DC_Date_For_Wages, LMargin + C1 + W1 + 40, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

    End Sub

    Private Sub Printing_Format4_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next


        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt


        CurY = CurY + 10
        pFont = New Font("Calibri", 11, FontStyle.Regular)
        'Common_Procedures.Print_To_PrintDocument(e, "Weaver Sign", LMargin + 10, CurY, 0, 0, pFont)

        If Trim(UCase(prn_WagesFrmt)) = "FORMAT-1.2" Or Trim(UCase(prn_WagesFrmt)) = "FORMAT-1.3" Then
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_MainName").ToString
        Else
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString
        End If

        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub Get_Party_DC_No(ByVal WagesCode As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer = 0
        Dim NewCode As String = ""


        If Trim(WagesCode) <> "" Then
            NewCode = Trim(WagesCode)
        Else
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        End If


        da1 = New SqlClient.SqlDataAdapter("Select a.*  from Weaver_Cloth_Receipt_Head a  Where a.Weaver_Wages_Code = '" & Trim(NewCode) & "' Order by fOR_oRDERbY , Weaver_ClothReceipt_No", con)
        dt1 = New DataTable
        da1.Fill(dt1)
        If dt1.Rows.Count > 0 Then
            Party_DC_No_For_Wages = ""
            Party_DC_Date_For_Wages = ""

            For i = 0 To dt1.Rows.Count - 1

                Party_DC_No_For_Wages = Party_DC_No_For_Wages & IIf(Trim(Party_DC_No_For_Wages) <> "", " ,", "") & dt1.Rows(i).Item("Party_DcNo").ToString
                Party_DC_Date_For_Wages = Party_DC_Date_For_Wages & IIf(Trim(Party_DC_Date_For_Wages) <> "", " ,", "") & Format(dt1.Rows(i).Item("Weaver_ClothReceipt_Date"), "dd/MM/yyyy")

            Next i

        End If
        dt1.Clear()
        da1.Dispose()

        Erase WeaverClothMeters
        WeaverClothMeters = New String(10) {}
        Erase WeaverClothCooly
        WeaverClothCooly = New String(10) {}
        Erase WeaverClothAmount
        WeaverClothAmount = New String(10) {}

        da1 = New SqlClient.SqlDataAdapter("Select a.* ,b.* from Weaver_Wages_Cooly_Details a LEFT OUTER JOIN ClothType_Head B on A.ClothType_IdNo = B.ClothType_IdNo Where a.Weaver_Wages_Code = '" & Trim(NewCode) & "' ", con)
        dt2 = New DataTable
        da1.Fill(dt2)
        If dt2.Rows.Count > 0 Then

            For i = 0 To dt2.Rows.Count - 1

                WeaverClothType(i + 1) = dt2.Rows(i).Item("ClothType_Name").ToString
                WeaverClothMeters(i + 1) = dt2.Rows(i).Item("Meters").ToString
                WeaverClothCooly(i + 1) = dt2.Rows(i).Item("cooly").ToString
                WeaverClothAmount(i + 1) = dt2.Rows(i).Item("Amount").ToString

            Next i

        End If
        dt2.Clear()
        da1.Dispose()

    End Sub


    Private Sub Printing_Format5_GST(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        Dim p1Font As Font
        'Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim SNo As Integer, I As Integer
        Dim ItmNm1 As String, ItmNm2 As String
        Dim NoofItems_PerPage As Integer, NoofDets As Integer
        Dim TxtHgt As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim C1, C2, C3, C4, C5 As Single
        ' Dim snd, sec, bit, rjt, otr As Single
        Dim ps As Printing.PaperSize
        Dim rndoff As Double, NetAmt As Double


        p1Font = New Font("Calibri", 11, FontStyle.Bold)

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20 ' 30
            .Right = 60 ' 50
            .Top = 30
            .Bottom = 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With


        pFont = New Font("Calibri", 9, FontStyle.Regular)

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With

        'd1 = e.Graphics.MeasureString("Department   : ", pFont).Width
        'i1 = e.Graphics.MeasureString("Item Name : ", pFont).Width
        'b1 = e.Graphics.MeasureString("Brand Name : ", pFont).Width
        'qn1 = e.Graphics.MeasureString("Quantity (NEW ITEM) : ", pFont).Width
        'qo1 = e.Graphics.MeasureString("Quantity (OLD ITEM) - Usable  : ", pFont).Width
        'qo2 = e.Graphics.MeasureString("Quantity (OLD ITEM) - Scrap : ", pFont).Width

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(35) : ClAr(2) = 300 : ClAr(3) = 65 : ClAr(4) = 90 : ClAr(5) = 90
        ClAr(6) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5))


        C1 = ClAr(1) + ClAr(2)
        C2 = C1 + ClAr(3) + ClAr(4)
        C3 = C2 + ClAr(5)
        C4 = C3 + ClAr(6)
        C5 = C4 + ClAr(7)

        TxtHgt = 17.5

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        NoofItems_PerPage = 15

        Try

            prn_Prev_HeadIndx = prn_HeadIndx

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format5_GST_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = NoofDets + 1

                ItmNm1 = (prn_HdDt.Rows(prn_HeadIndx).Item("Cloth_Name").ToString)
                ItmNm2 = ""
                If Len(ItmNm1) > 35 Then
                    For I = 35 To 1 Step -1
                        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 35
                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                End If

                pFont = New Font("Calibri", 10, FontStyle.Regular)

                CurY = CurY + TxtHgt
                p1Font = New Font("Calibri", 10, FontStyle.Regular)

                SNo = SNo + 1
                Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, p1Font)

                Common_Procedures.Print_To_PrintDocument(e, WeaverClothType(1), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, WeaverClothMeters(1), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, WeaverClothCooly(1), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(WeaverClothAmount(1)), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                NoofDets = NoofDets + 1

                CurY = CurY + TxtHgt
                If Val(WeaverClothMeters(2)) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothType(2), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothMeters(2), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothCooly(2), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(WeaverClothAmount(2)), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If

                If Trim(ItmNm2) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 20, CurY - 5, 0, 0, pFont)
                    NoofDets = NoofDets + 1
                End If

                If Val(WeaverClothMeters(3)) <> 0 Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothType(3), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothMeters(3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothCooly(3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(WeaverClothAmount(3)), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                    NoofDets = NoofDets + 1
                End If


                If Val(WeaverClothMeters(4)) <> 0 Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothType(4), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothMeters(4), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothCooly(4), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(WeaverClothAmount(4)), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                    NoofDets = NoofDets + 1
                End If

                If Val(WeaverClothMeters(5)) <> 0 Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothType(5), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothMeters(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothCooly(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(WeaverClothAmount(5)), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                    NoofDets = NoofDets + 1
                End If


                If Val(WeaverClothMeters(6)) <> 0 Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothType(6), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothMeters(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothCooly(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(WeaverClothAmount(6)), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                    NoofDets = NoofDets + 1
                End If

                CurY = CurY + TxtHgt + 5
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, PageWidth - (ClAr(6) + ClAr(5)), CurY)


                CurY = CurY + 8

                Common_Procedures.Print_To_PrintDocument(e, Format(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Meters"), "###########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)

                CurY = CurY + TxtHgt
                If prn_HdDt.Rows(prn_HeadIndx).Item("Freight_Charge").ToString <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Frieght", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Freight_Charge").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                    NoofDets = NoofDets + 1
                End If
                If prn_HdDt.Rows(prn_HeadIndx).Item("Add_Amount").ToString <> 0 Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "Add", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Add_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                    NoofDets = NoofDets + 1
                End If
                If prn_HdDt.Rows(prn_HeadIndx).Item("Less_Amount").ToString <> 0 Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "Less", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Less_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                    NoofDets = NoofDets + 1
                End If
                If prn_HdDt.Rows(prn_HeadIndx).Item("Tds_Perc_Calc").ToString <> 0 Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "Tds " & prn_HdDt.Rows(prn_HeadIndx).Item("Tds_Perc").ToString & " % ", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Tds_Perc_Calc").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                    NoofDets = NoofDets + 1
                End If
                CurY = CurY + TxtHgt + 2
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)
                CurY = CurY + 5
                If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Assesable_Value").ToString) <> 0 Then

                    'Common_Procedures.Print_To_PrintDocument(e, "õ‡® õ£ì¬è ", LMargin + 10, CurY, 0, 0, pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, "Total  Before Tax", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Assesable_Value").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                    NoofDets = NoofDets + 1
                End If


                If Val(prn_HdDt.Rows(prn_HeadIndx).Item("CGST_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt - 3
                    'Common_Procedures.Print_To_PrintDocument(e, "õ‡® õ£ì¬è ", Margin + 10, CurY, 0, 0, pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ " & prn_HdDt.Rows(prn_HeadIndx).Item("CGST_Percentage").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("CGST_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                    NoofDets = NoofDets + 1
                End If
                If Val(prn_HdDt.Rows(prn_HeadIndx).Item("SGST_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt - 3
                    'Common_Procedures.Print_To_PrintDocument(e, "õ‡® õ£ì¬è ", Margin + 10, CurY, 0, 0, pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ " & prn_HdDt.Rows(prn_HeadIndx).Item("SGST_Percentage").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("SGST_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                    NoofDets = NoofDets + 1
                End If

                NetAmt = Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Cooly").ToString) - Val(prn_HdDt.Rows(prn_HeadIndx).Item("Freight_Charge").ToString) + Val(prn_HdDt.Rows(prn_HeadIndx).Item("Add_Amount").ToString) - Val(prn_HdDt.Rows(prn_HeadIndx).Item("Less_Amount").ToString) - Val(prn_HdDt.Rows(prn_HeadIndx).Item("Tds_Perc_Calc").ToString) + Val(prn_HdDt.Rows(prn_HeadIndx).Item("CGST_AMount").ToString) + Val(prn_HdDt.Rows(prn_HeadIndx).Item("SGST_Amount").ToString), "#########0.00")
                'With dgv_WagesDetails_Total
                '    If .Rows.Count > 0 Then
                '        TotAmt = Val(.Rows(prn_HeadIndx).Cells(5).Value)
                '    End If
                'End With
                'NetAmt = Format(Val(TotAmt) - Val(prn_HdDt.Rows(prn_HeadIndx).Item("Freight_Charge").ToString) + Val(prn_HdDt.Rows(prn_HeadIndx).Item("Add_Amount").ToString) - Val(prn_HdDt.Rows(prn_HeadIndx).Item("Less_Amount").ToString) - Val(prn_HdDt.Rows(prn_HeadIndx).Item("Tds_Perc_Calc").ToString), "#########0.00")

                rndoff = 0
                rndoff = Val(prn_HdDt.Rows(prn_HeadIndx).Item("Net_Amount").ToString) - Val(NetAmt)

                CurY = CurY + TxtHgt + 5
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)
                CurY = CurY + 5
                If Val(rndoff) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Round off", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                    If Val(rndoff) >= 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, " " & Format(Math.Abs(Val(rndoff)), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If

                prn_DetIndx = prn_DetIndx + 1

            End If

            For I = NoofDets + 1 To 3
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))


            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "E & OE", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Net_Amount").ToString, PageWidth - 10, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)


            'Printing_Format5_GST_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

            CurY = CurY + TxtHgt
            'CurY = CurY + TxtHgt
            'CurY = CurY + TxtHgt
            'CurY = CurY + TxtHgt

            Printing_Format5_GSTDelivery(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        prn_Prev_HeadIndx = prn_HeadIndx
        prn_HeadIndx = prn_HeadIndx + 1

        If prn_HeadIndx <= prn_HdDt.Rows.Count - 1 Then
            e.HasMorePages = True
        Else
            e.HasMorePages = False
        End If

        prn_DetDt.Clear()

        prn_PageNo = 0

        prn_DetIndx = 0
        prn_DetSNo = 0

        prn_Tot_EBeam_Stk = 0
        prn_Tot_Pavu_Stk = 0
        prn_Tot_Yarn_Stk = 0
        prn_Tot_Amt_Bal = 0

    End Sub

    Private Sub Printing_Format5_GST_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_panNo As String, Cmp_PanCap As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim Led_Name As String, Led_TinNo As String, Led_Add1 As String = "", Led_Add2 As String = "", Led_Add3 As String = "", Led_Add4 As String = ""


        Dim strHeight As Single
        Dim C1, C2, S1, W1, W2 As Single


        PageNo = PageNo + 1

        CurY = TMargin


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = "" : Cmp_panNo = "" : Cmp_PanCap = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_EMail = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_MainName").ToString
        Cmp_Add1 = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address4").ToString

        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_CstNo").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Pan_No").ToString) <> "" Then
            Cmp_panNo = "PAN NO.: " & prn_HdDt.Rows(prn_HeadIndx).Item("Pan_No").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_No = "GSTIN : " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_GSTinNo").ToString
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
        'CurY = CurY + TxtHgt - 1
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt - 1
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "JOBWORK INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_panNo, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Led_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString
        Led_Add1 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address1").ToString

        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString) <> "" Then
            Led_Add2 = (prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString)
        End If

        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString) <> "" Then
            Led_Add3 = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString)
        End If

        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address4").ToString) <> "" Then
            Led_Add4 = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address4").ToString)
        End If


        C1 = LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
        C2 = C1 + ClAr(4) + 100
        S1 = e.Graphics.MeasureString("TO   :    ", pFont).Width
        W1 = e.Graphics.MeasureString("Bill Date  ", pFont).Width
        W2 = e.Graphics.MeasureString("Bill Date  ", pFont).Width


        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 12, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & Led_Name, LMargin + 10, CurY, 0, 0, p1Font)
        pFont = New Font("Calibri", 11, FontStyle.Regular)

        CurY = CurY + TxtHgt

        p1Font = New Font("Calibri", 9, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, "     " & Led_Add1, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "Invoice No", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_BillNo").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "P.Dc.No", LMargin + C1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":  " & Party_DC_No_For_Wages, LMargin + C1 + W1 + 20, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & Led_Add2, LMargin + 10, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "     " & Led_Add3, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address4").ToString) <> "" Then
            Led_Add4 = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address4").ToString)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "     " & Led_Add4, LMargin + 10, CurY, 0, 0, p1Font)
        End If


        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_GSTinNo").ToString) <> "" Then
            CurY = CurY + TxtHgt
            Led_TinNo = "     GSTIN.: " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_GSTinNo").ToString
            Common_Procedures.Print_To_PrintDocument(e, " " & Led_TinNo, LMargin + 10, CurY, 0, 0, p1Font)
        End If
        CurY = CurY + TxtHgt + 10
        ' e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(2))
        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TYPE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "RATE/MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)

        CurY = CurY + TxtHgt - 10



        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

    End Sub

    Private Sub Printing_Format5_GST_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String = ""
        'Dim p1Font As Font

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(5) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Net_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)
        'CurY = CurY + 10
        'pFont = New Font("Calibri", 11, FontStyle.Regular)
        ''Common_Procedures.Print_To_PrintDocument(e, "Weaver Sign", LMargin + 10, CurY, 0, 0, pFont)

        'If Trim(UCase(prn_WagesFrmt)) = "FORMAT-1.2" Or Trim(UCase(prn_WagesFrmt)) = "FORMAT-1.3" Then
        '    Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_MainName").ToString
        'Else
        '    Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString
        'End If

        'p1Font = New Font("Calibri", 12, FontStyle.Bold)

        'Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        'CurY = CurY + TxtHgt + 10

        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        'e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        'e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub Printing_Format5_GSTDelivery(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Prc_IdNo As Integer = 0
        Dim Yrn_fb_FmNm As String = ""
        Dim Yrn_fb_toNm As String = ""
        'Dim EntryCode As String
        Dim NoofDets As Integer
        'Dim pFont As Font
        'Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        'Dim PrintWidth As Single, PrintHeight As Single
        'Dim PageWidth As Single, PageHeight As Single
        'Dim CurY As Single, TxtHgt As Single
        'Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ClArr(15) As Single
        'Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer
        Dim TotMtrs As Single = 0




        Erase ClArr

        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(80) : ClArr(2) = 60 : ClArr(3) = 55 : ClArr(4) = 85 : ClArr(5) = 80 : ClArr(6) = 80 : ClArr(7) = 80 : ClArr(8) = 80 : ClArr(9) = 80
        ClArr(10) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9))

        Try
            DeliverySend = 0
            DeliverySnd = 0
            DeliveryBits = 0
            DeliveryMtrs = 0
            DeliveryRjts = 0
            DeliveryOthrs = 0

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format5_GSTDelivery_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt1.Rows.Count > 0 Then

                    Do While prn_DetIndx1 <= prn_DetDt1.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format5_GSTDelivery_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

                            e.HasMorePages = True

                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1

                        CurY = CurY + TxtHgt

                        SNo = SNo + 1

                        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_DetDt1.Rows(prn_DetIndx1).Item("Weaver_ClothReceipt_Date").ToString), "dd-MM-yyyy").ToString, LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt1.Rows(prn_DetIndx1).Item("Party_DcNo").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt1.Rows(prn_DetIndx1).Item("Noof_Pcs").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, pFont)
                        If Val(prn_DetDt1.Rows(prn_DetIndx1).Item("ReceiptMeters_Wages").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt1.Rows(prn_DetIndx1).Item("ReceiptMeters_Wages").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt1.Rows(prn_DetIndx1).Item("ReceiptMeters_Receipt").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                        End If
                        Fold = (prn_DetDt1.Rows(prn_DetIndx1).Item("Folding").ToString) / 100
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt1.Rows(prn_DetIndx1).Item("Type1_Checking_Meters").ToString) * Fold, "###########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt1.Rows(prn_DetIndx1).Item("Type2_Checking_Meters").ToString) * Fold, "###########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt1.Rows(prn_DetIndx1).Item("Type3_Checking_Meters").ToString) * Fold, "###########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt1.Rows(prn_DetIndx1).Item("Type4_Checking_Meters").ToString) * Fold, "###########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt1.Rows(prn_DetIndx1).Item("Type5_Checking_Meters").ToString) * Fold, "###########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)

                        TotMtrs = Val(prn_DetDt1.Rows(prn_DetIndx1).Item("Total_Checking_Meters").ToString) * Fold
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(TotMtrs), "###########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                        NoofDets = NoofDets + 1

                        DeliverySend = DeliverySend + Val(prn_DetDt1.Rows(prn_DetIndx1).Item("Type1_Checking_Meters").ToString) * Fold
                        DeliverySnd = DeliverySnd + Val(prn_DetDt1.Rows(prn_DetIndx1).Item("Type2_Checking_Meters").ToString) * Fold
                        DeliveryBits = DeliveryBits + Val(prn_DetDt1.Rows(prn_DetIndx1).Item("Type3_Checking_Meters").ToString) * Fold

                        DeliveryRjts = DeliveryRjts + Val(prn_DetDt1.Rows(prn_DetIndx1).Item("Type4_Checking_Meters").ToString) * Fold
                        DeliveryOthrs = DeliveryOthrs + Val(prn_DetDt1.Rows(prn_DetIndx1).Item("Type5_Checking_Meters").ToString) * Fold
                        DeliveryMtrs = DeliveryMtrs + Val(TotMtrs)

                        prn_DetIndx1 = prn_DetIndx1 + 1

                    Loop

                End If



                Printing_Format5_GSTDelivery_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format5_GSTDelivery_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClArr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt3 As New DataTable
        Dim p1Font As Font
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim C1 As Single

        PageNo = PageNo + 1

        'CurY = TMargin

        Try

            C1 = ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4)

            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "DELIVERY DETAILS", LMargin, CurY, 2, PrintWidth, p1Font)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY
            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, "RECEIVED", LMargin, CurY, 2, C1, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "FOLDING MTRS", LMargin + ClArr(1) + ClArr(2), CurY, 2, PageWidth, pFont)
            CurY = CurY + TxtHgt - 10
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY



            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin, CurY, 2, ClArr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + ClArr(1), CurY, 2, ClArr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClArr(1) + ClArr(2), CurY, 2, ClArr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY, 2, ClArr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SOUND", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), CurY, 2, ClArr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SECOND", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), CurY, 2, ClArr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BITS", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), CurY, 2, ClArr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "REJECT", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), CurY, 2, ClArr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "OTHERS", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8), CurY, 2, ClArr(9), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TOT.MTRS", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9), CurY, 2, ClArr(10), pFont)
            CurY = CurY + TxtHgt - 10

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format5_GSTDelivery_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim W1 As Single = 0
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Prc_IdNo As Integer = 0
        Dim Yrn_fb_FmNm As String = ""
        Dim Yrn_fb_toNm As String = ""
        Dim vprn_PckNos As String = ""
        Dim Tot_FbPrgWgt As Single = 0, Tot_PrgRls As Single = 0, Tot_Bgs As Single = 0, Tot_YnPrgWgt As Single = 0, Tot_YnPrgWgtBg As Single = 0
        Dim BmsInWrds As String

        W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

        Try

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt

                prn_DetIndx1 = prn_DetIndx1 + 1


            Next


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & (prn_HdDt.Rows(prn_HeadIndx).Item("Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & (prn_HdDt.Rows(prn_HeadIndx).Item("Receipt_Meters").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(DeliverySend), "#############0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(DeliverySnd), "#############0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(DeliveryBits), "#############0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(DeliveryRjts), "#############0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(DeliveryOthrs), "#############0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(DeliveryMtrs), "#############0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt - 10
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(8) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(5))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(5))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(5))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(5))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(5))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(5))


            CurY = CurY + TxtHgt
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Net_Amount").ToString))
            BmsInWrds = Replace(Trim(UCase(BmsInWrds)), "", "")

            Common_Procedures.Print_To_PrintDocument(e, "Rupees    :  " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY


            If Val(Common_Procedures.User.IdNo) <> 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt - 5
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_MainName").ToString

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)


            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, 30, LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, 30, PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub txt_CGST_Percentage_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_CGST_Percentage.TextChanged
        Total_Amount_Calculation()
    End Sub

    Private Sub txt_SGST_Percentage_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_SGST_Percentage.TextChanged
        Total_Amount_Calculation()
    End Sub

    Private Sub Printing_Format1_GST(ByRef e As System.Drawing.Printing.PrintPageEventArgs)

        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim OrdBy_FrmNo As Single = 0, OrdByToNo As Single = 0
        Dim pFont As Font
        Dim p1Font As Font
        Dim pTFont As Font
        Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim I As Integer = 0
        Dim NoofItems_PerPage As Integer, NoofDets As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim EntryCode As String
        Dim CurY As Single, Cur1Y As Single
        Dim C1 As Single = 0, C2 As Single = 0, C3 As Single = 0
        Dim C4 As Single = 0, C5 As Single = 0, C6 As Single = 0
        Dim W1, W2 As Single
        Dim snd As Single
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0
        Dim TtlCooleAmt As Single = 0

        p1Font = New Font("Calibri", 11, FontStyle.Bold)

        PrntCnt = 1
        If Val(Common_Procedures.settings.WeaverWages_Print_2Copy_In_SinglePage) = 1 Then

            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    e.PageSettings.PaperSize = ps
                    Exit For
                End If
            Next

            If PrntCnt2ndPageSTS = False Then
                PrntCnt = 2
            End If

        Else

            'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
            'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
            'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
            'e.PageSettings.PaperSize = pkCustomSize1
            'PrintDocument1.DefaultPageSettings.Landscape = False
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


        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '    If ps.Width = 800 And ps.Height = 600 Then
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        PpSzSTS = True
        '        Exit For
        '    End If
        'Next

        'If PpSzSTS = False Then

        '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
        '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '            PrintDocument1.DefaultPageSettings.PaperSize = ps
        '            e.PageSettings.PaperSize = ps
        '            PpSzSTS = True
        '            Exit For
        '        End If
        '    Next

        '    If PpSzSTS = False Then
        '        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '                PrintDocument1.DefaultPageSettings.PaperSize = ps
        '                e.PageSettings.PaperSize = ps
        '                Exit For
        '            End If
        '        Next
        '    End If

        'End If

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 10
            .Right = 40
            If Val(Common_Procedures.settings.WeaverWages_Print_2Copy_In_SinglePage) = 1 Then
                .Top = 5
            Else
                .Top = 10 ' 30
            End If
            .Bottom = 25 ' 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 10, FontStyle.Regular)
        pTFont = New Font("TAM_SC_Suvita", 10, FontStyle.Regular)

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

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(90) : ClAr(2) = 40 : ClAr(3) = 65 : ClAr(4) = 65 : ClAr(5) = 70 : ClAr(6) = 70 : ClAr(7) = 70 : ClAr(8) = 70 : ClAr(9) = 100
        ClAr(10) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9))


        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) '285
        C2 = C1 + ClAr(5)  '385

        If Val(Common_Procedures.settings.WeaverWages_Print_2Copy_In_SinglePage) = 1 Then
            TxtHgt = 17.2 ' 18  ' 18.5
        Else
            TxtHgt = 17.25 ' 18  ' 18.5
        End If

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        ' NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_Prev_HeadIndx = prn_HeadIndx

        PrntCnt2ndPageSTS = False

        TpMargin = TMargin

        For PCnt = 1 To PrntCnt

            If Val(Common_Procedures.settings.WeaverWages_Print_2Copy_In_SinglePage) = 1 Then

                If PCnt = 1 Then
                    prn_PageNo = 0

                    prn_DetIndx = 0
                    prn_DetSNo = 0

                    prn_Tot_EBeam_Stk = 0
                    prn_Tot_Pavu_Stk = 0
                    prn_Tot_Yarn_Stk = 0
                    prn_Tot_Amt_Bal = 0

                    TpMargin = TMargin

                Else

                    prn_PageNo = 0

                    prn_DetIndx = 0
                    prn_DetSNo = 0

                    prn_Tot_EBeam_Stk = 0
                    prn_Tot_Pavu_Stk = 0
                    prn_Tot_Yarn_Stk = 0
                    prn_Tot_Amt_Bal = 0

                    TpMargin = 600 + TMargin  ' 600 + TMargin

                End If

            End If

            Try

                If prn_HdDt.Rows.Count > 0 Then

                    Printing_Format1_GST_PageHeader(e, EntryCode, TxtHgt, pFont, pTFont, LMargin, RMargin, TpMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                    NoofDets = NoofDets + 1
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then
                        NoofItems_PerPage = 9 '6
                    Else
                        NoofItems_PerPage = 7 '6
                    End If

                    If Val(Common_Procedures.settings.WeaverWages_Print_2Copy_In_SinglePage) = 1 Then
                        If prn_DetDt.Rows.Count > NoofItems_PerPage Then
                            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then
                                NoofItems_PerPage = 44
                            Else
                                NoofItems_PerPage = 40
                            End If
                        End If
                    End If

                    If prn_PageNo <= 1 Then

                        Cur1Y = CurY
                        CurY = CurY + TxtHgt - 15
                        Common_Procedures.Print_To_PrintDocument(e, "óè‹ ", LMargin + 10, CurY, 0, 0, pTFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Cloth_Name").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                        CurY = CurY + TxtHgt - 3
                        Common_Procedures.Print_To_PrintDocument(e, "õó¾ e†ì˜ ", LMargin + 10, CurY, 0, 0, pTFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Receipt_Meters").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                        CurY = CurY + TxtHgt - 3
                        Common_Procedures.Print_To_PrintDocument(e, "õ÷K e†ì˜ ", LMargin + 10, CurY, 0, 0, pTFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Excess_Short").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                        CurY = CurY + TxtHgt - 3
                        Common_Procedures.Print_To_PrintDocument(e, "Taxable Amount", LMargin + 10, CurY, 0, 0, pFont)
                        '  Common_Procedures.Print_To_PrintDocument(e, "ªñ£ˆî ªî£¬è ", LMargin + 10, CurY, 0, 0, pTFont)

                        TtlCooleAmt = Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_cooly").ToString), "############0")

                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(TtlCooleAmt), "############0.00"), LMargin + C1 - 10, CurY, 1, 0, pFont)

                        CurY = CurY + TxtHgt - 3
                        Common_Procedures.Print_To_PrintDocument(e, "(Less) TDS @ " & prn_HdDt.Rows(prn_HeadIndx).Item("Tds_Perc").ToString & " %", LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Tds_Perc_Calc").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                        If prn_Frieght_Sts = True Then
                            If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Freight_Charge").ToString) <> 0 Then
                                CurY = CurY + TxtHgt - 3
                                'Common_Procedures.Print_To_PrintDocument(e, "õ‡® õ£ì¬è ", LMargin + 10, CurY, 0, 0, pTFont)
                                Common_Procedures.Print_To_PrintDocument(e, "Freight", LMargin + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Freight_Charge").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)
                            End If
                        End If


                        CurY = CurY + TxtHgt - 3
                        e.Graphics.DrawLine(Pens.Black, LMargin + C1 - 100, CurY, LMargin + C1 - 10, CurY)
                        CurY = CurY + 2


                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then ' -----------------Ganesh Karthick Textiles
                            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_GSTinNo").ToString) = "" Then
                                Common_Procedures.Print_To_PrintDocument(e, "Total Amount", LMargin + 10, CurY, 0, 0, pFont)
                            End If
                        End If




                        If prn_Frieght_Sts = True Then
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(TtlCooleAmt) - Val(prn_HdDt.Rows(prn_HeadIndx).Item("Tds_Perc_Calc").ToString) - Val(prn_HdDt.Rows(prn_HeadIndx).Item("Freight_Charge").ToString), "##############0.00"), LMargin + C1 - 10, CurY, 1, 0, pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(TtlCooleAmt) - Val(prn_HdDt.Rows(prn_HeadIndx).Item("Tds_Perc_Calc").ToString), "##############0.00"), LMargin + C1 - 10, CurY, 1, 0, pFont)
                        End If


                        CurY = CurY + TxtHgt + 2
                        e.Graphics.DrawLine(Pens.Black, LMargin + C1 - 100, CurY, LMargin + C1 - 10, CurY)

                        'CurY = CurY + TxtHgt - 15
                        'Common_Procedures.Print_To_PrintDocument(e, "Taxable Amount", LMargin + 10, CurY, 0, 0, pFont)
                        ''  Common_Procedures.Print_To_PrintDocument(e, "ªñ£ˆî ªî£¬è ", LMargin + 10, CurY, 0, 0, pTFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_cooly").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)


                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then ' -----------------Ganesh Karthick Textiles

                            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_GSTinNo").ToString) <> "" Then

                                CurY = CurY + TxtHgt - 15
                                If Val(prn_HdDt.Rows(prn_HeadIndx).Item("CGST_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(prn_HeadIndx).Item("SGST_Amount").ToString) <> 0 Then
                                    p1Font = New Font("calibri", 9, FontStyle.Bold)
                                    Common_Procedures.Print_To_PrintDocument(e, "Taxable Amount", LMargin + 10, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_cooly").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                                    CurY = CurY + TxtHgt - 3
                                    p1Font = New Font("Calibri", 9, FontStyle.Bold Or FontStyle.Underline)
                                    Common_Procedures.Print_To_PrintDocument(e, "Add", LMargin + 10, CurY, 0, 0, p1Font)
                                End If

                                CurY = CurY + TxtHgt - 3
                                If Val(prn_HdDt.Rows(prn_HeadIndx).Item("CGST_Amount").ToString) <> 0 Then
                                    p1Font = New Font("Calibri", 9, FontStyle.Bold)
                                    'Common_Procedures.Print_To_PrintDocument(e, "Tâv® õK", LMargin + 10, CurY, 0, 0, p1Font)
                                    Common_Procedures.Print_To_PrintDocument(e, "        CGST @ " & Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("CGST_Percentage").ToString), "#########0.0") & " %", LMargin + 10, CurY - 3, 0, 0, p1Font)
                                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("CGST_Amount").ToString), "#########0.00"), LMargin + C1 - 10, CurY - 2, 1, 0, pFont)
                                End If
                                CurY = CurY + TxtHgt - 3
                                If Val(prn_HdDt.Rows(prn_HeadIndx).Item("SGST_Amount").ToString) <> 0 Then
                                    p1Font = New Font("Calibri", 9, FontStyle.Bold)
                                    ' Common_Procedures.Print_To_PrintDocument(e, "Tâv® õK", LMargin + 10, CurY, 0, 0, p1Font)
                                    Common_Procedures.Print_To_PrintDocument(e, "        SGST @ " & Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("SGST_Percentage").ToString), "#########0.0") & " %", LMargin + 10, CurY - 3, 0, 0, p1Font)
                                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("SGST_Amount").ToString), "#########0.00"), LMargin + C1 - 10, CurY - 2, 1, 0, pFont)
                                End If

                                CurY = CurY + TxtHgt - 3
                                e.Graphics.DrawLine(Pens.Black, LMargin + C1 - 100, CurY, LMargin + C1 - 10, CurY)


                                CurY = CurY + 2
                                Common_Procedures.Print_To_PrintDocument(e, "Total Amount", LMargin + 10, CurY, 0, 0, pFont)
                                'Common_Procedures.Print_To_PrintDocument(e, "eF ªî£¬è ", LMargin + 10, CurY, 0, 0, pTFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Net_Amount").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)


                            End If

                        Else

                            CurY = CurY + TxtHgt - 15
                            If Val(prn_HdDt.Rows(prn_HeadIndx).Item("CGST_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(prn_HeadIndx).Item("SGST_Amount").ToString) <> 0 Then
                                p1Font = New Font("calibri", 9, FontStyle.Bold)
                                Common_Procedures.Print_To_PrintDocument(e, "Taxable Amount", LMargin + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_cooly").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                                CurY = CurY + TxtHgt - 3
                                p1Font = New Font("Calibri", 9, FontStyle.Bold Or FontStyle.Underline)
                                Common_Procedures.Print_To_PrintDocument(e, "Add", LMargin + 10, CurY, 0, 0, p1Font)
                            End If

                            CurY = CurY + TxtHgt - 3
                            If Val(prn_HdDt.Rows(prn_HeadIndx).Item("CGST_Amount").ToString) <> 0 Then
                                p1Font = New Font("Calibri", 9, FontStyle.Bold)
                                'Common_Procedures.Print_To_PrintDocument(e, "Tâv® õK", LMargin + 10, CurY, 0, 0, p1Font)
                                Common_Procedures.Print_To_PrintDocument(e, "        CGST @ " & Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("CGST_Percentage").ToString), "#########0.0") & " %", LMargin + 10, CurY - 3, 0, 0, p1Font)
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("CGST_Amount").ToString), "#########0.00"), LMargin + C1 - 10, CurY - 2, 1, 0, pFont)
                            End If
                            CurY = CurY + TxtHgt - 3
                            If Val(prn_HdDt.Rows(prn_HeadIndx).Item("SGST_Amount").ToString) <> 0 Then
                                p1Font = New Font("Calibri", 9, FontStyle.Bold)
                                ' Common_Procedures.Print_To_PrintDocument(e, "Tâv® õK", LMargin + 10, CurY, 0, 0, p1Font)
                                Common_Procedures.Print_To_PrintDocument(e, "        SGST @ " & Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("SGST_Percentage").ToString), "#########0.0") & " %", LMargin + 10, CurY - 3, 0, 0, p1Font)
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("SGST_Amount").ToString), "#########0.00"), LMargin + C1 - 10, CurY - 2, 1, 0, pFont)
                            End If

                            CurY = CurY + TxtHgt - 3
                            e.Graphics.DrawLine(Pens.Black, LMargin + C1 - 100, CurY, LMargin + C1 - 10, CurY)


                            CurY = CurY + 2
                            Common_Procedures.Print_To_PrintDocument(e, "Total Amount", LMargin + 10, CurY, 0, 0, pFont)
                            'Common_Procedures.Print_To_PrintDocument(e, "eF ªî£¬è ", LMargin + 10, CurY, 0, 0, pTFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Net_Amount").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                        End If



                        da1 = New SqlClient.SqlDataAdapter("select a.*, c.* from Weaver_Wages_Cooly_Details a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN ClothType_Head c ON a.ClothType_IdNo = c.ClothType_Idno Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and  a.Weaver_Wages_Code = '" & Trim(EntryCode) & "' Order by a.for_orderby, a.Sl_No", con)
                        prn_Cooly = New DataTable
                        da1.Fill(prn_Cooly)

                        If prn_Cooly.Rows.Count > 0 Then
                            For I = 0 To prn_Cooly.Rows.Count - 1


                                ' CurY = CurY + TxtHgt - 10

                                snd = Val(prn_Cooly.Rows(I).Item("ClothType_IdNo").ToString)

                                If Val(snd) = 1 Then
                                    Cur1Y = Cur1Y + TxtHgt
                                    Common_Procedures.Print_To_PrintDocument(e, "ê¾‡† ", LMargin + C1 + 10, Cur1Y, 0, 0, pTFont)

                                ElseIf Val(snd) = 2 Then
                                    Cur1Y = Cur1Y + TxtHgt
                                    Common_Procedures.Print_To_PrintDocument(e, "ªêè‡†v ", LMargin + C1 + 10, Cur1Y, 0, 0, pTFont)

                                ElseIf Val(snd) = 3 Then
                                    Cur1Y = Cur1Y + TxtHgt
                                    Common_Procedures.Print_To_PrintDocument(e, "îQ óè‹ ", LMargin + C1 + 10, Cur1Y, 0, 0, pTFont)

                                ElseIf Val(snd) = 4 Then
                                    Cur1Y = Cur1Y + TxtHgt
                                    Common_Procedures.Print_To_PrintDocument(e, "èN¾ ", LMargin + C1 + 10, Cur1Y, 0, 0, pTFont)

                                ElseIf Val(snd) = 5 Then
                                    Cur1Y = Cur1Y + TxtHgt
                                    Common_Procedures.Print_To_PrintDocument(e, "Þîó ", LMargin + C1 + 10, Cur1Y, 0, 0, pTFont)

                                End If

                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_Cooly.Rows(I).Item("Amount").ToString), "########0.00"), PageWidth - 10, Cur1Y, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, " = ", PageWidth - 80, Cur1Y, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_Cooly.Rows(I).Item("cooly").ToString, PageWidth - 100, Cur1Y, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, " X ", PageWidth - 170, Cur1Y, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_Cooly.Rows(I).Item("Meters").ToString, PageWidth - 190, Cur1Y, 1, 0, pFont)

                            Next

                            Cur1Y = Cur1Y + TxtHgt
                            e.Graphics.DrawLine(Pens.Black, LMargin + C2 + 10, Cur1Y + 5, PageWidth - 10, Cur1Y + 5)


                            Cur1Y = Cur1Y + TxtHgt - 10
                            Common_Procedures.Print_To_PrintDocument(e, "ªñ£ˆî‹ ", LMargin + C1 + 10, Cur1Y, 0, 0, pTFont)

                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_Meters").ToString, PageWidth - 190, Cur1Y, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_Cooly").ToString, PageWidth - 10, Cur1Y, 1, 0, pFont)

                            Cur1Y = Cur1Y + TxtHgt + 10

                            e.Graphics.DrawLine(Pens.Black, LMargin + C2 + 10, Cur1Y - 5, PageWidth - 10, Cur1Y - 5)


                            W1 = e.Graphics.MeasureString("dv â‡E‚¬è : ", pFont).Width
                            W2 = e.Graphics.MeasureString("Ë™ ªêô¾ : ", pFont).Width

                            Common_Procedures.Print_To_PrintDocument(e, "dv â‡E‚¬è ", LMargin + C1 + 10, CurY, 0, 0, pTFont)
                            Common_Procedures.Print_To_PrintDocument(e, " :  " & prn_HdDt.Rows(prn_HeadIndx).Item("Pcs").ToString, LMargin + C1 + W1 + 80, CurY, 0, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, "Ë™ ªêô¾ ", LMargin + C1 + 250, CurY, 0, 0, pTFont)
                            Common_Procedures.Print_To_PrintDocument(e, " :  " & prn_HdDt.Rows(prn_HeadIndx).Item("Total_Dgv_Weight").ToString, LMargin + C1 + 250 + W2 + 10, CurY, 0, 0, pFont)

                        End If

                        CurY = CurY + TxtHgt + 0

                        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                        LnAr(4) = CurY
                        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(3))



                    Else
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then
                            NoofItems_PerPage = 24 '15
                        Else
                            NoofItems_PerPage = 22 '15
                        End If

                    End If

                    CurY = CurY + TxtHgt - 10
                    Common_Procedures.Print_To_PrintDocument(e, "«îF", LMargin, CurY, 2, ClAr(1), pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, "ªï‹.", LMargin + ClAr(1), CurY, 2, ClAr(2), pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, "d‹ õ/ð", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, "d‹ Þ¼", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, "ð£¾ õ/ð", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, "ð£¾ Þ¼", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, "Ë™ õ/ð", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, "Ë™ Þ¼", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, "ªî£¬è õ/ð", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, "ªî£¬è Þ¼", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pTFont)

                    CurY = CurY + TxtHgt + 5 ' 10
                    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                    LnAr(5) = CurY

                    NoofDets = 0

                    CurY = CurY - 10

                    If prn_DetDt.Rows.Count > 0 Then

                        Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                            If NoofDets >= NoofItems_PerPage Then

                                CurY = CurY + TxtHgt

                                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 10, CurY, 1, 0, pFont)

                                NoofDets = NoofDets + 1

                                If Val(Common_Procedures.settings.WeaverWages_Print_NoNeed_2nd_Page) = 1 Then
                                    Printing_Format1_GST_PageFooter(e, EntryCode, TxtHgt, pFont, pTFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

                                    GoTo LOOP2

                                Else

                                    Printing_Format1_GST_PageFooter(e, EntryCode, TxtHgt, pFont, pTFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                                    e.HasMorePages = True

                                    Return

                                End If

                            End If

                            prn_Tot_EBeam_Stk = prn_Tot_EBeam_Stk + Val(prn_DetDt.Rows(prn_DetIndx).Item("EmptyBeam").ToString)
                            prn_Tot_Pavu_Stk = prn_Tot_Pavu_Stk + Val(prn_DetDt.Rows(prn_DetIndx).Item("PavuMtrs").ToString)
                            prn_Tot_Yarn_Stk = prn_Tot_Yarn_Stk + Val(prn_DetDt.Rows(prn_DetIndx).Item("YarnWgt").ToString)
                            prn_Tot_Amt_Bal = prn_Tot_Amt_Bal + Val(prn_DetDt.Rows(prn_DetIndx).Item("amount").ToString)

                            CurY = CurY + TxtHgt
                            If IsDate(prn_DetDt.Rows(prn_DetIndx).Item("Date1").ToString) = True Then
                                Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_DetDt.Rows(prn_DetIndx).Item("Date1").ToString), "dd-MM-yy").ToString, LMargin + 5, CurY, 0, 0, pFont)
                            Else

                                If Val(prn_DetDt.Rows(prn_DetIndx).Item("Ent_OrderBy").ToString) = 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, "Opening", LMargin + 5, CurY, 0, 0, pFont)
                                End If

                            End If
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Dc_Rec_No").ToString, LMargin + ClAr(1) + 5, CurY, 0, 0, pFont)
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("EmptyBeam").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("EmptyBeam").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 5, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Val(prn_Tot_EBeam_Stk), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 5, CurY, 1, 0, pFont)
                            End If
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("PavuMtrs").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("PavuMtrs").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_Tot_Pavu_Stk), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)
                            End If
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("YarnWgt").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("YarnWgt").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_Tot_Yarn_Stk), "#########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, pFont)
                            End If
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("amount").ToString) <> 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 5, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_Tot_Amt_Bal), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 5, CurY, 1, 0, pFont)
                            End If

                            NoofDets = NoofDets + 1

                            prn_DetIndx = prn_DetIndx + 1

                        Loop

                    End If

                End If

LOOP1:
                CurY = CurY + TxtHgt

                Printing_Format1_GST_PageFooter(e, EntryCode, TxtHgt, pFont, pTFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

            Catch ex As Exception
                MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try

            If Val(Common_Procedures.settings.WeaverWages_Print_2Copy_In_SinglePage) = 1 Then
                If PCnt = 1 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then
                    If prn_DetDt.Rows.Count > 9 Then
                        PrntCnt2ndPageSTS = True
                        e.HasMorePages = True
                        Return
                    End If
                End If
            End If

            PrntCnt2ndPageSTS = False

        Next PCnt

LOOP2:

        prn_HeadIndx = prn_HeadIndx + 1

        If prn_HeadIndx <= prn_HdDt.Rows.Count - 1 Then
            e.HasMorePages = True

        Else
            e.HasMorePages = False

        End If

        prn_DetDt.Clear()

        prn_PageNo = 0

        prn_DetIndx = 0
        prn_DetSNo = 0

        prn_Tot_EBeam_Stk = 0
        prn_Tot_Pavu_Stk = 0
        prn_Tot_Yarn_Stk = 0
        prn_Tot_Amt_Bal = 0

    End Sub

    Private Sub Printing_Format1_GST_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal pTFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_GstNo As String
        Dim Led_Name As String, Led_Add As String, Led_Add1 As String, Led_Add2 As String
        Dim strHeight As Single
        Dim C1, C2, C3, S1, W1, W2 As Single


        PageNo = PageNo + 1

        CurY = TMargin

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_GstNo = ""

        If Trim(UCase(prn_WagesFrmt)) = "FORMAT-2.2" Or Trim(UCase(prn_WagesFrmt)) = "FORMAT-2.3" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1020" Then '---- Sri Vijayalakshmi Spinners (Udamalpet)
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_MainName").ToString
            Cmp_Add1 = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString
            Cmp_Add2 = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address4").ToString & " GSTIN : " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_GSTinNo").ToString

        Else
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString
            Cmp_Add1 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString
            Cmp_Add2 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address4").ToString & " GSTIN : " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_GSTinNo").ToString
            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_PhoneNo").ToString) <> "" Then
                Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(prn_HeadIndx).Item("Company_PhoneNo").ToString
            End If
            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_TinNo").ToString) <> "" Then
                Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_TinNo").ToString
            End If
            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_CstNo").ToString) <> "" Then
                Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_CstNo").ToString
            End If
            'If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_GSTinNo").ToString) <> "" Then
            '    Cmp_GstNo = "GSTIN : " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_GSTinNo").ToString
            'End If

        End If

        CurY = CurY + TxtHgt - 10
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1029" Then '---- Arul Kumaran Textiles (Somanur)
            p1Font = New Font("Calibri", 18, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
            CurY = CurY + strHeight - 1
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1 & " " & Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

            'CurY = CurY + strHeight - 1
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)
            'CurY = CurY + TxtHgt - 1
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
            'CurY = CurY + TxtHgt - 1
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
            'CurY = CurY + TxtHgt - 1
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        End If
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) '285
        C2 = C1 + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8)

        C3 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 20

        S1 = e.Graphics.MeasureString("TO   :    ", pFont).Width
        W1 = e.Graphics.MeasureString("óC¶ ªï‹.   ", pFont).Width
        W2 = e.Graphics.MeasureString("óC¶ «îF   ", pFont).Width

        If Trim(UCase(prn_WagesFrmt)) = "FORMAT-2.2" Or Trim(UCase(prn_WagesFrmt)) = "FORMAT-2.3" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1020" Then '---- Sri Vijayalakshmi Spinners (Udamalpet)
            Led_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString
            Led_Add1 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString
            Led_Add2 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address4").ToString

            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address4").ToString) <> "" Then
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString) & " " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address4").ToString)
            ElseIf Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString) <> "" Then
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString) & " " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString)
            ElseIf Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString) <> "" Then
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address1").ToString) & " " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString)
            Else
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address1").ToString)
            End If

        Else
            Led_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_MainName").ToString
            Led_Add1 = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString
            Led_Add2 = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address4").ToString

            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address4").ToString) <> "" Then
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString) & " " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address4").ToString)
            ElseIf Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString) <> "" Then
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString) & " " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString)
            ElseIf Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString) <> "" Then
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString) & " " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString)
            Else
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString)
            End If

        End If

        If Trim(UCase(prn_WagesFrmt)) = "FORMAT-2.3" Then
            CurY = CurY + TxtHgt
            pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "H™ ªï‹. ", LMargin + 10, CurY, 0, 0, pFont)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_BillNo").ToString, LMargin + W1 + 20, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_No").ToString, LMargin + W1 + 20, CurY, 0, 0, pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "H™ «îF ", LMargin + 140 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Date").ToString), "dd-MM-yyyy").ToString, LMargin + 140 + W2 + 20, CurY, 0, 0, pFont)


            pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
                Common_Procedures.Print_To_PrintDocument(e, "ªìLõK ªï‹.", LMargin + C3 - 65, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_No").ToString, LMargin + C3 - 65 + W1 + 40, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "óC¶ ªï‹. ", LMargin + C3 - 65, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_No").ToString, LMargin + C3 - 65 + W1 + 30, CurY, 0, 0, pFont)
            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
                Common_Procedures.Print_To_PrintDocument(e, "ªìLõK «îF", LMargin + C2 - 20, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "óC¶ «îF ", LMargin + C2 - 20, CurY, 0, 0, pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Rec_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C2 + W2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + 5

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY
            LnAr(4) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + C3 - 75, CurY, LMargin + C3 - 75, LnAr(2))


        Else

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & Led_Name, LMargin + 10, CurY, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, "H™ ªï‹. ", LMargin + C3 + 10, CurY, 0, 0, pTFont)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_BillNo").ToString, LMargin + C3 + W1 + 30, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_No").ToString, LMargin + C3 + W1 + 30, CurY, 0, 0, pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "H™ «îF ", LMargin + C2 + 10, CurY, 0, 0, pTFont)
            Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C2 + W2 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & Led_Add, LMargin + S1 + 10, CurY, 0, 0, pFont)


            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
                Common_Procedures.Print_To_PrintDocument(e, "ªìLõK ªï‹.", LMargin + C3 + 10, CurY, 0, 0, pTFont)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_No").ToString, LMargin + C3 + W1 + 40, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "ªìLõK «îF", LMargin + C2 + 10, CurY, 0, 0, pTFont)
                'Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Rec_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C2 + W2 + 40, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_Date").ToString, LMargin + C2 + W2 + 30, CurY, 0, 0, pFont)

            Else
                Common_Procedures.Print_To_PrintDocument(e, "óC¶ ªï‹. ", LMargin + C3 + 10, CurY, 0, 0, pTFont)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_No").ToString, LMargin + C3 + W1 + 30, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "óC¶ «îF ", LMargin + C2 + 10, CurY, 0, 0, pTFont)
                ' Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Rec_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C2 + W2 + 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_Date").ToString, LMargin + C2 + W2 + 30, CurY, 0, 0, pFont)

            End If


            CurY = CurY + TxtHgt + 5 ' 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY
            LnAr(4) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + C3, CurY, LMargin + C3, LnAr(2))

        End If

    End Sub

    Private Sub Printing_Format1_GST_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal pTFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer = 0
        Dim k As Integer = 0
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim S1, C1, C2 As Single
        Dim Tot_EBeam_StkSumry As Single = 0
        Dim prn_Tot_Pavu_StkSumry As String = ""
        Dim prn_Tot_Yarn_StkSumry As String = ""
        Dim prn_Tot_Amt_BalSumry As Single = 0

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        'CurY = CurY + 5
        LnAr(6) = CurY

        'CurY = CurY + 5

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(4))

        CurY = CurY + 10

        If prn_PageNo <= 1 Then

            Tot_EBeam_StkSumry = 0
            prn_Tot_Pavu_StkSumry = ""
            prn_Tot_Yarn_StkSumry = ""
            prn_Tot_Amt_BalSumry = 0

            da1 = New SqlClient.SqlDataAdapter("select sum(int6) from reporttemp where name6 = 'BEAM'", con)
            Dt1 = New DataTable
            da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                    Tot_EBeam_StkSumry = Val(Dt1.Rows(0)(0).ToString)
                End If
            End If
            Dt1.Clear()

            da1 = New SqlClient.SqlDataAdapter("select sum(currency1) from reporttemp where name6 = 'AMOUNT'", con)
            Dt1 = New DataTable
            da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                    prn_Tot_Amt_BalSumry = Val(Dt1.Rows(0)(0).ToString)
                End If
            End If
            Dt1.Clear()

            da1 = New SqlClient.SqlDataAdapter("select name7 as EndsCount, sum(meters6) as PavuMtrs from reporttemp where name6 = 'PAVU' GROUP BY name7 having sum(meters6) <> 0 ORDER BY name7", con)
            Dt1 = New DataTable
            da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                For k = 0 To Dt1.Rows.Count - 1
                    prn_Tot_Pavu_StkSumry = Trim(prn_Tot_Pavu_StkSumry) & IIf(Trim(prn_Tot_Pavu_StkSumry) <> "", ", ", "") & Dt1.Rows(k).Item("EndsCount").ToString & " : " & Dt1.Rows(k).Item("PavuMtrs").ToString
                Next
            End If
            Dt1.Clear()

            da1 = New SqlClient.SqlDataAdapter("select name7 as CountName, sum(weight1) as YarnWgt from reporttemp where name6 = 'YARN' GROUP BY name7 having sum(weight1) <> 0 ORDER BY name7", con)
            Dt1 = New DataTable
            da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                For k = 0 To Dt1.Rows.Count - 1
                    prn_Tot_Yarn_StkSumry = Trim(prn_Tot_Yarn_StkSumry) & IIf(Trim(prn_Tot_Yarn_StkSumry) <> "", ", ", "") & Dt1.Rows(k).Item("CountName").ToString & " : " & Dt1.Rows(k).Item("YarnWgt").ToString
                Next
            End If
            Dt1.Clear()


            'C1 = ClAr(1) + ClAr(2) + ClAr(3)
            'C2 = C1 + ClAr(4) + ClAr(5)

            'S1 = e.Graphics.MeasureString("Ë™  :", pFont).Width

            'Common_Procedures.Print_To_PrintDocument(e, "Þ¼Š¹ Mõó‹ :- ", LMargin + 10, CurY, 0, 0, pTFont)

            'Common_Procedures.Print_To_PrintDocument(e, "d‹  ", LMargin + C1 + 10, CurY, 0, 0, pTFont)
            'Common_Procedures.Print_To_PrintDocument(e, " : " & Val(Tot_EBeam_StkSumry), LMargin + C1 + S1 + 20, CurY, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "ªî£¬è  ", LMargin + C2 + 30, CurY, 0, 0, pTFont)
            'Common_Procedures.Print_To_PrintDocument(e, "  :  " & Format(Math.Abs(Val(prn_Tot_Amt_BalSumry)), "#########0.00") & IIf(Val(prn_Tot_Amt_BalSumry) < 0, " Dr", " Cr"), LMargin + C2 + 90, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt

            'Common_Procedures.Print_To_PrintDocument(e, "(" & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Date").ToString), "dd-MM-yyyy").ToString & ")", LMargin + 10, CurY, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "ð£¾  ", LMargin + C1 + 10, CurY, 0, 0, pTFont)
            'Common_Procedures.Print_To_PrintDocument(e, "  :  " & prn_Tot_Pavu_StkSumry, LMargin + C1 + S1 + 20, CurY, 0, 0, pFont)


            C1 = ClAr(1) + ClAr(2) + ClAr(3) - 80
            C2 = C1 + ClAr(4) + ClAr(5) - 70

            S1 = e.Graphics.MeasureString("Ë™  :", pFont).Width

            Common_Procedures.Print_To_PrintDocument(e, "Þ¼Š¹ Mõó‹ :- ", LMargin + 10, CurY, 0, 0, pTFont)

            Common_Procedures.Print_To_PrintDocument(e, "d‹  ", LMargin + C1 + 10, CurY, 0, 0, pTFont)
            Common_Procedures.Print_To_PrintDocument(e, " : " & Val(Tot_EBeam_StkSumry), LMargin + C1 + S1 + 20, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "ªî£¬è  ", LMargin + C2 + 30, CurY, 0, 0, pTFont)
            Common_Procedures.Print_To_PrintDocument(e, "  :  " & Format(Math.Abs(Val(prn_Tot_Amt_BalSumry)), "#########0.00") & IIf(Val(prn_Tot_Amt_BalSumry) < 0, " Dr", " Cr"), LMargin + C2 + 90, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "Ë™  ", LMargin + C2 + 200, CurY, 0, 0, pTFont)
            Common_Procedures.Print_To_PrintDocument(e, "  :  " & prn_Tot_Yarn_StkSumry, LMargin + C2 + 230, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "(" & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Date").ToString), "dd-MM-yyyy").ToString & ")", LMargin + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "ð£¾  ", LMargin + C1 + 10, CurY, 0, 0, pTFont)
            Common_Procedures.Print_To_PrintDocument(e, "  :  " & prn_Tot_Pavu_StkSumry, LMargin + C1 + S1 + 20, CurY, 0, 0, pFont)


            'CurY = CurY + TxtHgt

            'Common_Procedures.Print_To_PrintDocument(e, "Ë™  ", LMargin + C1 + 10, CurY, 0, 0, pTFont)
            'Common_Procedures.Print_To_PrintDocument(e, "  :  " & prn_Tot_Yarn_StkSumry, LMargin + C1 + S1 + 20, CurY, 0, 0, pFont)

        Else
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt

        End If

        '  CurY = CurY + TxtHgt
        ' CurY = CurY + TxtHgt

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then
            '  CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "ð£˜® ¬èªò£Šð‹  ", LMargin + 10, CurY, 0, 0, pTFont)
        End If

        If Trim(UCase(prn_WagesFrmt)) = "FORMAT-2.2" Or Trim(UCase(prn_WagesFrmt)) = "FORMAT-2.3" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1020" Then '---- Sri Vijayalakshmi Spinners (Udamalpet)
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_MainName").ToString
        Else
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1029" Then '---- Arul Kumaran Textiles (Somanur)
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 15, CurY, 1, 0, pFont)
        Else
            ' p1Font = New Font("Calibri", 12, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "ð£˜® ¬èªò£Šð‹  ", PageWidth - 15, CurY, 1, 0, pTFont)

            End If
        End If


        CurY = CurY + TxtHgt

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub Printing_Format6_GST(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        Dim p1Font As Font
        'Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim SNo As Integer, I As Integer
        Dim ItmNm1 As String, ItmNm2 As String
        Dim NoofItems_PerPage As Integer, NoofDets As Integer
        Dim TxtHgt As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim C1, C2, C3, C4, C5 As Single
        ' Dim snd, sec, bit, rjt, otr As Single
        Dim ps As Printing.PaperSize
        Dim rndoff As Double, NetAmt As Double


        p1Font = New Font("Calibri", 11, FontStyle.Bold)

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20 ' 30
            .Right = 60 ' 50
            .Top = 30
            .Bottom = 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With


        pFont = New Font("Calibri", 12, FontStyle.Regular)

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument1.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With

        'd1 = e.Graphics.MeasureString("Department   : ", pFont).Width
        'i1 = e.Graphics.MeasureString("Item Name : ", pFont).Width
        'b1 = e.Graphics.MeasureString("Brand Name : ", pFont).Width
        'qn1 = e.Graphics.MeasureString("Quantity (NEW ITEM) : ", pFont).Width
        'qo1 = e.Graphics.MeasureString("Quantity (OLD ITEM) - Usable  : ", pFont).Width
        'qo2 = e.Graphics.MeasureString("Quantity (OLD ITEM) - Scrap : ", pFont).Width

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(35) : ClAr(2) = 295 : ClAr(3) = 75 : ClAr(4) = 90 : ClAr(5) = 90
        ClAr(6) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5))


        C1 = ClAr(1) + ClAr(2)
        C2 = C1 + ClAr(3) + ClAr(4)
        C3 = C2 + ClAr(5)
        C4 = C3 + ClAr(6)
        C5 = C4 + ClAr(7)

        TxtHgt = 24

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        NoofItems_PerPage = 1

        Try

            prn_Prev_HeadIndx = prn_HeadIndx

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format6_GST_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = NoofDets + 1

                ItmNm1 = (prn_HdDt.Rows(prn_HeadIndx).Item("Cloth_Name").ToString)
                ItmNm2 = ""
                If Len(ItmNm1) > 35 Then
                    For I = 35 To 1 Step -1
                        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 35
                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                End If

                CurY = CurY + TxtHgt
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC CODE : 9988", LMargin + ClAr(1) + 10, CurY, 0, 0, p1Font)

                CurY = CurY + TxtHgt + 5
                p1Font = New Font("Calibri", 10, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "(Textile manufactring service (Weaving) )", LMargin + ClAr(1) + 10, CurY, 0, 0, p1Font)

                pFont = New Font("Calibri", 11, FontStyle.Regular)

                CurY = CurY + TxtHgt + 5
                p1Font = New Font("Calibri", 11, FontStyle.Regular)
                SNo = SNo + 1
                Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClAr(1) + 10, CurY, 0, 0, p1Font)

                Common_Procedures.Print_To_PrintDocument(e, WeaverClothType(1), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, WeaverClothMeters(1), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, WeaverClothCooly(1), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(WeaverClothAmount(1)), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                NoofDets = NoofDets + 1

                CurY = CurY + TxtHgt + 5
                If Val(WeaverClothMeters(2)) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothType(2), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothMeters(2), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothCooly(2), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(WeaverClothAmount(2)), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If

                If Trim(ItmNm2) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClAr(1) + 20, CurY - 5, 0, 0, pFont)
                    NoofDets = NoofDets + 1
                End If

                If Val(WeaverClothMeters(3)) <> 0 Then
                    CurY = CurY + TxtHgt + 5
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothType(3), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothMeters(3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothCooly(3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(WeaverClothAmount(3)), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                    NoofDets = NoofDets + 1
                End If


                If Val(WeaverClothMeters(4)) <> 0 Then
                    CurY = CurY + TxtHgt + 5
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothType(4), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothMeters(4), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothCooly(4), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(WeaverClothAmount(4)), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                    NoofDets = NoofDets + 1
                End If

                If Val(WeaverClothMeters(5)) <> 0 Then
                    CurY = CurY + TxtHgt + 5
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothType(5), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothMeters(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothCooly(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(WeaverClothAmount(5)), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                    NoofDets = NoofDets + 1
                End If


                If Val(WeaverClothMeters(6)) <> 0 Then
                    CurY = CurY + TxtHgt + 5
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothType(6), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothMeters(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, WeaverClothCooly(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(WeaverClothAmount(6)), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                    NoofDets = NoofDets + 1
                End If

                CurY = CurY + TxtHgt + 10
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, PageWidth - (ClAr(6) + ClAr(5)), CurY)


                CurY = CurY + 8

                Common_Procedures.Print_To_PrintDocument(e, Format(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Meters"), "###########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                CurY = CurY + TxtHgt
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                'LnAr(5) = CurY
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))

                If prn_HdDt.Rows(prn_HeadIndx).Item("Freight_Charge").ToString <> 0 Then

                    CurY = CurY + TxtHgt + 5
                    Common_Procedures.Print_To_PrintDocument(e, "Frieght", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Freight_Charge").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                    NoofDets = NoofDets + 1
                End If
                If prn_HdDt.Rows(prn_HeadIndx).Item("Add_Amount").ToString <> 0 Then
                    CurY = CurY + TxtHgt + 5
                    Common_Procedures.Print_To_PrintDocument(e, "Add", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Add_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                    NoofDets = NoofDets + 1
                End If
                If prn_HdDt.Rows(prn_HeadIndx).Item("Less_Amount").ToString <> 0 Then
                    CurY = CurY + TxtHgt + 5
                    Common_Procedures.Print_To_PrintDocument(e, "Less", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Less_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                    NoofDets = NoofDets + 1
                End If
                If prn_HdDt.Rows(prn_HeadIndx).Item("Tds_Perc_Calc").ToString <> 0 Then
                    CurY = CurY + TxtHgt + 5
                    Common_Procedures.Print_To_PrintDocument(e, "Tds " & prn_HdDt.Rows(prn_HeadIndx).Item("Tds_Perc").ToString & " % ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Tds_Perc_Calc").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                    NoofDets = NoofDets + 1
                End If
                CurY = CurY + TxtHgt + 5
                'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)
                'CurY = CurY + 5
                If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Taxable_Amount").ToString) <> 0 Then

                    'Common_Procedures.Print_To_PrintDocument(e, "õ‡® õ£ì¬è ", LMargin + 10, CurY, 0, 0, pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_Taxable_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                    NoofDets = NoofDets + 1
                End If


                If Val(prn_HdDt.Rows(prn_HeadIndx).Item("CGST_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt + 5
                    'Common_Procedures.Print_To_PrintDocument(e, "õ‡® õ£ì¬è ", Margin + 10, CurY, 0, 0, pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ " & prn_HdDt.Rows(prn_HeadIndx).Item("CGST_Percentage").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("CGST_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                    NoofDets = NoofDets + 1
                End If
                If Val(prn_HdDt.Rows(prn_HeadIndx).Item("SGST_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt + 5
                    'Common_Procedures.Print_To_PrintDocument(e, "õ‡® õ£ì¬è ", Margin + 10, CurY, 0, 0, pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ " & prn_HdDt.Rows(prn_HeadIndx).Item("SGST_Percentage").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("SGST_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                    NoofDets = NoofDets + 1
                End If

                NetAmt = Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Cooly").ToString) - Val(prn_HdDt.Rows(prn_HeadIndx).Item("Freight_Charge").ToString) + Val(prn_HdDt.Rows(prn_HeadIndx).Item("Add_Amount").ToString) - Val(prn_HdDt.Rows(prn_HeadIndx).Item("Less_Amount").ToString) - Val(prn_HdDt.Rows(prn_HeadIndx).Item("Tds_Perc_Calc").ToString) + Val(prn_HdDt.Rows(prn_HeadIndx).Item("CGST_AMount").ToString) + Val(prn_HdDt.Rows(prn_HeadIndx).Item("SGST_Amount").ToString), "#########0.00")
                'With dgv_WagesDetails_Total
                '    If .Rows.Count > 0 Then
                '        TotAmt = Val(.Rows(prn_HeadIndx).Cells(5).Value)
                '    End If
                'End With
                'NetAmt = Format(Val(TotAmt) - Val(prn_HdDt.Rows(prn_HeadIndx).Item("Freight_Charge").ToString) + Val(prn_HdDt.Rows(prn_HeadIndx).Item("Add_Amount").ToString) - Val(prn_HdDt.Rows(prn_HeadIndx).Item("Less_Amount").ToString) - Val(prn_HdDt.Rows(prn_HeadIndx).Item("Tds_Perc_Calc").ToString), "#########0.00")

                rndoff = 0
                rndoff = Val(prn_HdDt.Rows(prn_HeadIndx).Item("Net_Amount").ToString) - Val(NetAmt)

                CurY = CurY + TxtHgt + 5
                'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)
                'CurY = CurY + 5
                If Val(rndoff) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Round off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)
                    If Val(rndoff) >= 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, " " & Format(Math.Abs(Val(rndoff)), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If

                prn_DetIndx = prn_DetIndx + 1

            End If

            For I = NoofDets + 1 To 3
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY


            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "E & OE", LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Net_Amount").ToString, PageWidth - 10, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)


            Printing_Format6_GST_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)
            If Trim(prn_InpOpts) <> "" Then
                If prn_Count < Len(Trim(prn_InpOpts)) Then


                    If Val(prn_InpOpts) <> "0" Then
                        prn_DetIndx = 0
                        prn_DetSNo = 0
                        prn_PageNo = 0
                        cnt = 0
                        e.HasMorePages = True
                        Return
                    End If

                End If
            End If
            'CurY = CurY + TxtHgt
            'CurY = CurY + TxtHgt
            'CurY = CurY + TxtHgt
            'CurY = CurY + TxtHgt
            'CurY = CurY + TxtHgt + 10

            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            'e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            'e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

            'Printing_Format6_GSTDelivery(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        'prn_Prev_HeadIndx = prn_HeadIndx
        'prn_HeadIndx = prn_HeadIndx + 1

        'If prn_HeadIndx <= prn_HdDt.Rows.Count - 1 Then
        '    e.HasMorePages = True
        'Else
        '    e.HasMorePages = False
        'End If


        'prn_DetDt.Clear()

        'prn_PageNo = 0

        'prn_DetIndx = 0
        'prn_DetSNo = 0
        cnt = cnt + 1
        prn_Tot_EBeam_Stk = 0
        prn_Tot_Pavu_Stk = 0
        prn_Tot_Yarn_Stk = 0
        prn_Tot_Amt_Bal = 0
        e.HasMorePages = False


    End Sub

    Private Sub Printing_Format6_GST_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_panNo As String, Cmp_PanCap As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim Led_Name As String, Led_TinNo As String, Led_Add1 As String = "", Led_Add2 As String = "", Led_Add3 As String = "", Led_Add4 As String = ""
        Dim prn_OriDupTri As String = ""
        Dim S As String = ""

        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim C1, C2, S1, W1, W2 As Single


        PageNo = PageNo + 1

        CurY = TMargin

        prn_Count = prn_Count + 1

        PrintDocument1.DefaultPageSettings.Color = False
        PrintDocument1.PrinterSettings.DefaultPageSettings.Color = False
        e.PageSettings.Color = False

        prn_OriDupTri = ""
        'If Trim(prn_InpOpts) <> "" Then
        '    If prn_Count1 <= Len(Trim(prn_InpOpts)) Then

        '        S = Mid$(Trim(prn_InpOpts), prn_Count1, 1)

        '        If Val(S) = 2 Then
        '            prn_OriDupTri = "ORIGINAL"
        '            PrintDocument1.DefaultPageSettings.Color = True
        '            PrintDocument1.PrinterSettings.DefaultPageSettings.Color = True
        '            e.PageSettings.Color = True
        '            'ElseIf Val(S) = 2 Then
        '            '    prn_OriDupTri = "DUPLICATE"
        '            'ElseIf Val(S) = 3 Then
        '            '    prn_OriDupTri = "TRIPLICATE"
        '        End If

        '    End If
        'End If
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = "" : Cmp_panNo = "" : Cmp_PanCap = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_EMail = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_MainName").ToString
        Cmp_Add1 = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address4").ToString

        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_CstNo").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Pan_No").ToString) <> "" Then
            Cmp_panNo = "PAN NO.: " & prn_HdDt.Rows(prn_HeadIndx).Item("Pan_No").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_No = "GSTIN : " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_GSTinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_State_Name").ToString) <> "" Then
            Cmp_StateNm = "STATE : " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_State_Name").ToString)

            If prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString <> "" Then
                strWidth = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, pFont).Width
                Cmp_StateCode = "CODE : " & (prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString)
                Cmp_StateNm = Cmp_StateNm & "     " & Cmp_StateCode
            End If
        End If
        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, LMargin, CurY, 2, PrintWidth, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "JOBWORK INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, LMargin + 10, CurY, 0, 0, pFont)
        ' Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Led_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString
        Led_Add1 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address1").ToString

        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString) <> "" Then
            Led_Add2 = (prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString)
        End If

        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString) <> "" Then
            Led_Add3 = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString)
        End If

        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address4").ToString) <> "" Then
            Led_Add4 = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address4").ToString)
        End If


        C1 = LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
        C2 = C1 + ClAr(4) + 100
        S1 = e.Graphics.MeasureString("TO   :    ", pFont).Width
        W1 = e.Graphics.MeasureString("Bill Date  ", pFont).Width
        W2 = e.Graphics.MeasureString("Bill Date  ", pFont).Width


        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 13, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & Led_Name, LMargin + 10, CurY, 0, 0, p1Font)
        pFont = New Font("Calibri", 12, FontStyle.Regular)

        CurY = CurY + TxtHgt + 5

        p1Font = New Font("Calibri", 10, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, "     " & Led_Add1, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "Invoice No", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_BillNo").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "P.Dc.No", LMargin + C1 + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, ":  " & Party_DC_No_For_Wages, LMargin + C1 + W1 + 20, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt + 5
        Common_Procedures.Print_To_PrintDocument(e, "     " & Led_Add2, LMargin + 10, CurY, 0, 0, p1Font)
        CurY = CurY + TxtHgt + 5
        Common_Procedures.Print_To_PrintDocument(e, "     " & Led_Add3, LMargin + 10, CurY, 0, 0, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, "Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address4").ToString) <> "" Then
            Led_Add4 = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address4").ToString)
            CurY = CurY + TxtHgt + 5
            Common_Procedures.Print_To_PrintDocument(e, "     " & Led_Add4, LMargin + 10, CurY, 0, 0, p1Font)
        End If


        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_GSTinNo").ToString) <> "" Then
            CurY = CurY + TxtHgt + 5
            Led_TinNo = "     GSTIN.: " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_GSTinNo").ToString
            Common_Procedures.Print_To_PrintDocument(e, " " & Led_TinNo, LMargin + 10, CurY, 0, 0, p1Font)
        End If
        CurY = CurY + TxtHgt + 10
        ' e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(2))
        CurY = CurY + TxtHgt - 12
        pFont = New Font("Calibri", 12, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY + 5, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PARTICULARS", LMargin + ClAr(1), CurY + 5, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TYPE", LMargin + ClAr(1) + ClAr(2), CurY + 5, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY + 5, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "CHARGES/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + 5, 2, ClAr(6), pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        CurY = CurY + TxtHgt - 15



        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

    End Sub

    Private Sub Printing_Format6_GST_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim BmsInWrds As String
        'Dim i As Integer
        Dim Cmp_Name As String = ""
        Dim p1Font As Font

        ''For i = NoofDets + 1 To NoofItems_PerPage
        ''    CurY = CurY + TxtHgt
        ''Next

        'CurY = CurY + TxtHgt
        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        'LnAr(5) = CurY

        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))

        'CurY = CurY + TxtHgt - 10
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Net_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
        'CurY = CurY + TxtHgt + 5
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))

        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)
        'CurY = CurY + 10
        'pFont = New Font("Calibri", 11, FontStyle.Regular)
        ''Common_Procedures.Print_To_PrintDocument(e, "Weaver Sign", LMargin + 10, CurY, 0, 0, pFont)

        'If Trim(UCase(prn_WagesFrmt)) = "FORMAT-1.2" Or Trim(UCase(prn_WagesFrmt)) = "FORMAT-1.3" Then
        '    Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_MainName").ToString
        'Else
        '    Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString
        'End If

        'p1Font = New Font("Calibri", 12, FontStyle.Bold)

        'Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)
        CurY = CurY + TxtHgt + 10
        BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Net_Amount").ToString))
        BmsInWrds = Replace(Trim((BmsInWrds)), "", "")

        Common_Procedures.Print_To_PrintDocument(e, "Rupees    :  " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(10) = CurY

        If Val(Common_Procedures.User.IdNo) <> 1 Then
            Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
        End If
        CurY = CurY + TxtHgt - 5
        Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_MainName").ToString

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
        'Printing_Format6_GSTDelivery(e, NoofItems_PerPage)
        'e.HasMorePages = True

    End Sub
    Private Sub Printing_Format6_GSTDelivery(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Prc_IdNo As Integer = 0
        Dim Yrn_fb_FmNm As String = ""
        Dim Yrn_fb_toNm As String = ""
        Dim NoofItems_PerPage As Integer
        Dim EntryCode As String
        Dim NoofDets As Integer
        Dim pFont As Font
        Dim p1Font As Font
        'Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        ' Dim ClArr(15) As Single
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim W1 As Single = 0
        Dim SNo As Integer
        Dim TotMtrs As Single = 0


        p1Font = New Font("Calibri", 11, FontStyle.Bold)

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20 ' 30
            .Right = 60 ' 50
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

        NoofItems_PerPage = 35

        Erase ClArr
        TxtHgt = 19

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}


        ClArr(1) = Val(100) : ClArr(2) = 60 : ClArr(3) = 55 : ClArr(4) = 85 : ClArr(5) = 80 : ClArr(6) = 80 : ClArr(7) = 80 : ClArr(8) = 80
        ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))
        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Try
            DeliverySend = 0
            DeliverySnd = 0
            DeliveryBits = 0
            DeliveryMtrs = 0
            DeliveryRjts = 0
            DeliveryOthrs = 0

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format6_GSTDelivery_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt1.Rows.Count > 0 Then

                    Do While prn_DetIndx1 <= prn_DetDt1.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then
                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format6_GSTDelivery_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

                            e.HasMorePages = True

                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1

                        CurY = CurY + TxtHgt

                        SNo = SNo + 1

                        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_DetDt1.Rows(prn_DetIndx1).Item("Weaver_ClothReceipt_Date").ToString), "dd-MM-yyyy").ToString, LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt1.Rows(prn_DetIndx1).Item("Party_DcNo").ToString, LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt1.Rows(prn_DetIndx1).Item("Noof_Pcs").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, pFont)
                        If Val(prn_DetDt1.Rows(prn_DetIndx1).Item("ReceiptMeters_Wages").ToString) <> 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt1.Rows(prn_DetIndx1).Item("ReceiptMeters_Wages").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt1.Rows(prn_DetIndx1).Item("ReceiptMeters_Receipt").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                        End If
                        Fold = (prn_DetDt1.Rows(prn_DetIndx1).Item("Folding").ToString) / 100
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt1.Rows(prn_DetIndx1).Item("Type1_Checking_Meters").ToString) * Fold, "###########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt1.Rows(prn_DetIndx1).Item("Type2_Checking_Meters").ToString) * Fold, "###########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt1.Rows(prn_DetIndx1).Item("Type3_Checking_Meters").ToString) * Fold, "###########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt1.Rows(prn_DetIndx1).Item("Type4_Checking_Meters").ToString) * Fold, "###########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt1.Rows(prn_DetIndx1).Item("Type5_Checking_Meters").ToString) * Fold, "###########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)

                        TotMtrs = Val(prn_DetDt1.Rows(prn_DetIndx1).Item("Total_Checking_Meters").ToString) * Fold
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(TotMtrs), "###########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                        NoofDets = NoofDets + 1

                        DeliverySend = DeliverySend + Val(prn_DetDt1.Rows(prn_DetIndx1).Item("Type1_Checking_Meters").ToString) * Fold
                        DeliverySnd = DeliverySnd + Val(prn_DetDt1.Rows(prn_DetIndx1).Item("Type2_Checking_Meters").ToString) * Fold
                        DeliveryBits = DeliveryBits + Val(prn_DetDt1.Rows(prn_DetIndx1).Item("Type3_Checking_Meters").ToString) * Fold

                        DeliveryRjts = DeliveryRjts + Val(prn_DetDt1.Rows(prn_DetIndx1).Item("Type4_Checking_Meters").ToString) * Fold
                        DeliveryOthrs = DeliveryOthrs + Val(prn_DetDt1.Rows(prn_DetIndx1).Item("Type5_Checking_Meters").ToString) * Fold
                        DeliveryMtrs = DeliveryMtrs + Val(TotMtrs)

                        prn_DetIndx1 = prn_DetIndx1 + 1
                        NoofDets = NoofDets + 1
                    Loop

                End If



                Printing_Format6_GSTDelivery_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)

            End If
            If Trim(prn_InpOpts) <> "0" Then
                If prn_Count1 < Len(Trim(prn_InpOpts)) Then


                    If Val(prn_InpOpts) <> "0" Then
                        prn_DetIndx1 = 0
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

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format6_GSTDelivery_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClArr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt3 As New DataTable
        Dim p1Font As Font
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim C1 As Single
        ' Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_panNo As String, Cmp_PanCap As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim Led_Name As String = "", Led_TinNo As String = "", Led_Add1 As String = "", Led_Add2 As String = "", Led_Add3 As String = "", Led_Add4 As String = ""
        Dim prn_OriDupTri As String = ""
        Dim S As String = ""
        Dim strHeight As Single = 0, strWidth As Single = 0


        PageNo = PageNo + 2

        CurY = TMargin


        prn_Count1 = prn_Count1 + 1

        PrintDocument1.DefaultPageSettings.Color = False
        PrintDocument1.PrinterSettings.DefaultPageSettings.Color = False
        e.PageSettings.Color = False

        'prn_OriDupTri = ""
        'If Trim(prn_InpOpts) <> "" Then
        '    If prn_Count <= Len(Trim(prn_InpOpts)) Then

        '        S = Mid$(Trim(prn_InpOpts), prn_Count, 1)

        '        If Val(S) = 1 Then
        '            prn_OriDupTri = "ORIGINAL"
        '            PrintDocument1.DefaultPageSettings.Color = True
        '            PrintDocument1.PrinterSettings.DefaultPageSettings.Color = True
        '            e.PageSettings.Color = True
        '        ElseIf Val(S) = 2 Then
        '            prn_OriDupTri = "DUPLICATE"
        '            'ElseIf Val(S) = 3 Then
        '            '    prn_OriDupTri = "TRIPLICATE"
        '        End If

        '    End If
        'End If

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = "" : Cmp_panNo = "" : Cmp_PanCap = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_EMail = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_MainName").ToString
        Cmp_Add1 = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address4").ToString

        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_CstNo").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Pan_No").ToString) <> "" Then
            Cmp_panNo = "PAN NO.: " & prn_HdDt.Rows(prn_HeadIndx).Item("Pan_No").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_No = "GSTIN : " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_GSTinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_State_Name").ToString) <> "" Then
            Cmp_StateNm = "STATE : " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_State_Name").ToString)

            If prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString <> "" Then
                strWidth = e.Graphics.MeasureString(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, pFont).Width
                Cmp_StateCode = "CODE : " & (prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString)
                Cmp_StateNm = Cmp_StateNm & "     " & Cmp_StateCode
            End If
        End If
        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "JOBWORK INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTIN_No, LMargin + 10, CurY, 0, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_StateNm, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            C1 = ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4)
            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "DELIVERY DETAILS", LMargin, CurY, 2, PrintWidth, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "CLOTH NAME : " & prn_HdDt.Rows(0).Item("Cloth_NAme").ToString, LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY
            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, "RECEIVED", LMargin, CurY, 2, C1, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "FOLDING MTRS", LMargin + ClArr(1) + ClArr(2), CurY, 2, PageWidth, pFont)
            CurY = CurY + TxtHgt - 10
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY



            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin, CurY, 2, ClArr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + ClArr(1), CurY, 2, ClArr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClArr(1) + ClArr(2), CurY, 2, ClArr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClArr(1) + ClArr(2) + ClArr(3), CurY, 2, ClArr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SOUND", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4), CurY, 2, ClArr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SECOND", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5), CurY, 2, ClArr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BITS", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6), CurY, 2, ClArr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "REJECT", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7), CurY, 2, ClArr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TOT.MTRS", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8), CurY, 2, ClArr(9), pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "TOT.MTRS", LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9), CurY, 2, ClArr(10), pFont)
            CurY = CurY + TxtHgt - 10

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format6_GSTDelivery_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim I As Integer
        Dim W1 As Single = 0
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Prc_IdNo As Integer = 0
        Dim Yrn_fb_FmNm As String = ""
        Dim Yrn_fb_toNm As String = ""
        Dim vprn_PckNos As String = ""
        Dim Tot_FbPrgWgt As Single = 0, Tot_PrgRls As Single = 0, Tot_Bgs As Single = 0, Tot_YnPrgWgt As Single = 0, Tot_YnPrgWgtBg As Single = 0


        W1 = e.Graphics.MeasureString("No.of Beams  : ", pFont).Width

        Try

            For I = NoofDets + 1 To NoofItems_PerPage

                CurY = CurY + TxtHgt

                prn_DetIndx1 = prn_DetIndx1 + 1


            Next


            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & (prn_HdDt.Rows(prn_HeadIndx).Item("Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & (prn_HdDt.Rows(prn_HeadIndx).Item("Receipt_Meters").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(DeliverySend), "#############0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(DeliverySnd), "#############0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(DeliveryBits), "#############0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(DeliveryRjts), "#############0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(DeliveryOthrs), "#############0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(DeliveryMtrs), "#############0.00"), PageWidth - 10, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt - 10
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(8) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(5))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(5))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(5))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(5))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(5))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(5))





            'If Val(Common_Procedures.User.IdNo) <> 1 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 400, CurY, 0, 0, pFont)
            'End If
            'CurY = CurY + TxtHgt - 5
            'Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_MainName").ToString

            'p1Font = New Font("Calibri", 12, FontStyle.Bold)
            'Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)


            'CurY = CurY + TxtHgt
            'CurY = CurY + TxtHgt
            'CurY = CurY + TxtHgt + 10
            'Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)



            'Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
            'CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, 30, LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, 30, PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format2_GST(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim pFont As Font
        Dim p1Font As Font
        Dim pTFont As Font
        'Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        'Dim I As Integer
        Dim NoofItems_PerPage As Integer, NoofDets As Integer
        Dim TxtHgt As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim C1, C2, C3, C4, C5 As Single
        Dim snd As Single = 0
        Dim ps As Printing.PaperSize
        Dim Cur1Y As Single = 0
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        PrntCnt = 1
        If Val(Common_Procedures.settings.WeaverWages_Print_2Copy_In_SinglePage) = 1 Then

            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    Exit For
                End If
            Next

            If PrntCnt2ndPageSTS = False Then
                PrntCnt = 2
            End If
        Else

            Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
            e.PageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.Landscape = False

        End If



        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 10 ' 30
            .Right = 60 ' 50
            .Top = 20
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
        'If PrintDocument1.DefaultPageSettings.Landscape = True Then
        '    With PrintDocument1.DefaultPageSettings.PaperSize
        '        PrintWidth = .Height - TMargin - BMargin
        '        PrintHeight = .Width - RMargin - LMargin
        '        PageWidth = .Height - TMargin
        '        PageHeight = .Width - RMargin
        '    End With
        'End If

        'd1 = e.Graphics.MeasureString("Department   : ", pFont).Width
        'i1 = e.Graphics.MeasureString("Item Name : ", pFont).Width
        'b1 = e.Graphics.MeasureString("Brand Name : ", pFont).Width
        'qn1 = e.Graphics.MeasureString("Quantity (NEW ITEM) : ", pFont).Width
        'qo1 = e.Graphics.MeasureString("Quantity (OLD ITEM) - Usable  : ", pFont).Width
        'qo2 = e.Graphics.MeasureString("Quantity (OLD ITEM) - Scrap : ", pFont).Width

        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(35) : ClAr(2) = 250 : ClAr(3) = 50 : ClAr(4) = 50 : ClAr(5) = 50
        ClAr(6) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5))


        C1 = ClAr(1) + ClAr(2)
        C2 = C1 + ClAr(3) + ClAr(4)
        C3 = C2 + ClAr(5)
        C4 = C3 + ClAr(6)
        C5 = C4 + ClAr(7)

        TxtHgt = 19

        TpMargin = TMargin
        PrntCnt2ndPageSTS = False

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        For PCnt = 1 To PrntCnt

            If Val(Common_Procedures.settings.WeaverWages_Print_2Copy_In_SinglePage) = 1 Then

                If PCnt = 1 Then
                    prn_PageNo = 0

                    prn_DetIndx = 0
                    prn_DetSNo = 0

                    prn_Tot_EBeam_Stk = 0
                    prn_Tot_Pavu_Stk = 0
                    prn_Tot_Yarn_Stk = 0
                    prn_Tot_Amt_Bal = 0

                    TpMargin = TMargin

                Else

                    prn_PageNo = 0

                    prn_DetIndx = 0
                    prn_DetSNo = 0

                    prn_Tot_EBeam_Stk = 0
                    prn_Tot_Pavu_Stk = 0
                    prn_Tot_Yarn_Stk = 0
                    prn_Tot_Amt_Bal = 0

                    TpMargin = 560 + TMargin  ' 600 + TMargin

                End If

            End If
            Try

                prn_Prev_HeadIndx = prn_HeadIndx

                If prn_HdDt.Rows.Count > 0 Then

                    Printing_Format2_GST_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TpMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)


                    NoofItems_PerPage = 7 '6
                    If Val(Common_Procedures.settings.WeaverWages_Print_2Copy_In_SinglePage) = 1 Then
                        If prn_DetDt.Rows.Count > NoofItems_PerPage Then
                            NoofItems_PerPage = 8
                        End If
                    End If

                    If prn_PageNo <= 1 Then

                        NoofDets = NoofDets + 1

                        CurY = CurY + TxtHgt - 10
                        pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, "óè‹ ", LMargin + 10, CurY, 0, 0, pFont)
                        pFont = New Font("Calibri", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Cloth_Name").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                        Cur1Y = CurY

                        CurY = CurY + TxtHgt
                        pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, "õó¾ e†ì˜ ", LMargin + 10, CurY, 0, 0, pFont)
                        pFont = New Font("Calibri", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Receipt_Meters").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                        CurY = CurY + TxtHgt
                        pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, "õ÷K e†ì˜ ", LMargin + 10, CurY, 0, 0, pFont)
                        pFont = New Font("Calibri", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Excess_Short").ToString), "############0.000"), LMargin + C1 - 10, CurY, 1, 0, pFont)

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "Taxable Amount", LMargin + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_cooly").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                        'pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                        'Common_Procedures.Print_To_PrintDocument(e, "ªñ£ˆî ªî£¬è ", LMargin + 10, CurY, 0, 0, pFont)
                        'pFont = New Font("Calibri", 11, FontStyle.Regular)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_cooly").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                        CurY = CurY + TxtHgt - 3

                        p1Font = New Font("Calibri", 9, FontStyle.Bold)
                        Common_Procedures.Print_To_PrintDocument(e, "(Less) TDS @ " & prn_HdDt.Rows(prn_HeadIndx).Item("Tds_Perc").ToString & " %", LMargin + 10, CurY, 0, 0, p1Font)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Tds_Perc_Calc").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                        If prn_Frieght_Sts = True Then
                            If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Freight_Charge").ToString) <> 0 Then
                                CurY = CurY + TxtHgt - 3
                                'Common_Procedures.Print_To_PrintDocument(e, "õ‡® õ£ì¬è ", LMargin + 10, CurY, 0, 0, pTFont)
                                Common_Procedures.Print_To_PrintDocument(e, "(Less) Freight", LMargin + 10, CurY, 0, 0, p1Font)
                                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Freight_Charge").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)
                            End If
                        End If


                        CurY = CurY + TxtHgt - 3
                        e.Graphics.DrawLine(Pens.Black, LMargin + C1 - 100, CurY, LMargin + C1 - 10, CurY)
                        CurY = CurY + 2
                        If prn_Frieght_Sts = True Then
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_cooly").ToString) - Val(prn_HdDt.Rows(prn_HeadIndx).Item("Tds_Perc_Calc").ToString) - Val(prn_HdDt.Rows(prn_HeadIndx).Item("Freight_Charge").ToString), "##############0.00"), LMargin + C1 - 10, CurY, 1, 0, pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_cooly").ToString) - Val(prn_HdDt.Rows(prn_HeadIndx).Item("Tds_Perc_Calc").ToString), "##############0.00"), LMargin + C1 - 10, CurY, 1, 0, pFont)
                        End If



                        CurY = CurY + TxtHgt + 2
                        e.Graphics.DrawLine(Pens.Black, LMargin + C1 - 100, CurY, LMargin + C1 - 10, CurY)

                        'CurY = CurY + TxtHgt - 15
                        'Common_Procedures.Print_To_PrintDocument(e, "Taxable Amount", LMargin + 10, CurY, 0, 0, pFont)
                        ''  Common_Procedures.Print_To_PrintDocument(e, "ªñ£ˆî ªî£¬è ", LMargin + 10, CurY, 0, 0, pTFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_cooly").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)


                        CurY = CurY + TxtHgt - 15
                        If Val(prn_HdDt.Rows(prn_HeadIndx).Item("CGST_Amount").ToString) <> 0 Or Val(prn_HdDt.Rows(prn_HeadIndx).Item("SGST_Amount").ToString) <> 0 Then
                            p1Font = New Font("calibri", 9, FontStyle.Bold)
                            Common_Procedures.Print_To_PrintDocument(e, "Taxable Amount", LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_cooly").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                            CurY = CurY + TxtHgt - 3
                            p1Font = New Font("Calibri", 9, FontStyle.Bold Or FontStyle.Underline)
                            Common_Procedures.Print_To_PrintDocument(e, "Add", LMargin + 10, CurY, 0, 0, p1Font)
                        End If
                        CurY = CurY + TxtHgt - 3
                        If Val(prn_HdDt.Rows(prn_HeadIndx).Item("CGST_Amount").ToString) <> 0 Then
                            p1Font = New Font("Calibri", 9, FontStyle.Bold)
                            'Common_Procedures.Print_To_PrintDocument(e, "Tâv® õK", LMargin + 10, CurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, "        CGST @ " & Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("CGST_Percentage").ToString), "#########0.0") & " %", LMargin + 10, CurY - 3, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("CGST_Amount").ToString), "#########0.00"), LMargin + C1 - 10, CurY - 2, 1, 0, pFont)
                        End If
                        CurY = CurY + TxtHgt - 3
                        If Val(prn_HdDt.Rows(prn_HeadIndx).Item("SGST_Amount").ToString) <> 0 Then
                            p1Font = New Font("Calibri", 9, FontStyle.Bold)
                            ' Common_Procedures.Print_To_PrintDocument(e, "Tâv® õK", LMargin + 10, CurY, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, "        SGST @ " & Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("SGST_Percentage").ToString), "#########0.0") & " %", LMargin + 10, CurY - 3, 0, 0, p1Font)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("SGST_Amount").ToString), "#########0.00"), LMargin + C1 - 10, CurY - 2, 1, 0, pFont)
                        End If

                        CurY = CurY + TxtHgt - 3
                        e.Graphics.DrawLine(Pens.Black, LMargin + C1 - 100, CurY, LMargin + C1 - 10, CurY)


                        CurY = CurY + 2
                        Common_Procedures.Print_To_PrintDocument(e, "Total Amount", LMargin + 10, CurY, 0, 0, pFont)
                        'Common_Procedures.Print_To_PrintDocument(e, "eF ªî£¬è ", LMargin + 10, CurY, 0, 0, pTFont)
                        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Net_Amount").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_cooly").ToString) + Val(prn_HdDt.Rows(prn_HeadIndx).Item("CGST_Amount").ToString) + Val(prn_HdDt.Rows(prn_HeadIndx).Item("SGST_Amount").ToString), "###########0")), "############0.00"), LMargin + C1 - 10, CurY, 1, 0, pFont)



                        da1 = New SqlClient.SqlDataAdapter("select a.*, c.* from Weaver_Wages_Cooly_Details a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN ClothType_Head c ON a.ClothType_IdNo = c.ClothType_Idno Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and  a.Weaver_Wages_Code = '" & Trim(EntryCode) & "' Order by a.for_orderby, a.Sl_No", con)
                        prn_Cooly = New DataTable
                        da1.Fill(prn_Cooly)

                        Cur1Y = Cur1Y - TxtHgt

                        If prn_Cooly.Rows.Count > 0 Then
                            For I = 0 To prn_Cooly.Rows.Count - 1

                                pTFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                                ' CurY = CurY + TxtHgt - 10

                                snd = Val(prn_Cooly.Rows(I).Item("ClothType_IdNo").ToString)


                                If Val(snd) = 1 Then
                                    Cur1Y = Cur1Y + TxtHgt
                                    Common_Procedures.Print_To_PrintDocument(e, "ê¾‡† ", LMargin + C1 + 10, Cur1Y, 0, 0, pTFont)

                                ElseIf Val(snd) = 2 Then
                                    Cur1Y = Cur1Y + TxtHgt
                                    Common_Procedures.Print_To_PrintDocument(e, "ªêè‡†v ", LMargin + C1 + 10, Cur1Y, 0, 0, pTFont)

                                ElseIf Val(snd) = 3 Then
                                    Cur1Y = Cur1Y + TxtHgt
                                    Common_Procedures.Print_To_PrintDocument(e, "îQ óè‹ ", LMargin + C1 + 10, Cur1Y, 0, 0, pTFont)

                                ElseIf Val(snd) = 4 Then
                                    Cur1Y = Cur1Y + TxtHgt
                                    Common_Procedures.Print_To_PrintDocument(e, "èN¾ ", LMargin + C1 + 10, Cur1Y, 0, 0, pTFont)

                                ElseIf Val(snd) = 5 Then
                                    Cur1Y = Cur1Y + TxtHgt
                                    Common_Procedures.Print_To_PrintDocument(e, "Þîó ", LMargin + C1 + 10, Cur1Y, 0, 0, pTFont)

                                End If

                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_Cooly.Rows(I).Item("Amount").ToString), "########0.00"), PageWidth - 10, Cur1Y, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, " = ", PageWidth - 80, Cur1Y, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_Cooly.Rows(I).Item("cooly").ToString, PageWidth - 100, Cur1Y, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, " X ", PageWidth - 170, Cur1Y, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, prn_Cooly.Rows(I).Item("Meters").ToString, PageWidth - 190, Cur1Y, 1, 0, pFont)

                            Next
                        End If


                        e.Graphics.DrawLine(Pens.Black, LMargin + C2 + 50, CurY, PageWidth - 10, CurY)


                        pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, "ªñ£ˆî‹ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                        pFont = New Font("Calibri", 11, FontStyle.Regular)

                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_Meters").ToString, PageWidth - 190, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_Cooly").ToString, PageWidth - 10, CurY, 1, 0, pFont)

                        CurY = CurY + TxtHgt
                        e.Graphics.DrawLine(Pens.Black, LMargin + C1 - 100, CurY, LMargin + C1 - 10, CurY)

                        e.Graphics.DrawLine(Pens.Black, LMargin + C2 + 50, CurY, PageWidth - 10, CurY)


                        'W1 = e.Graphics.MeasureString("dv â‡E‚¬è : ", pFont).Width
                        'W2 = e.Graphics.MeasureString("Ë™ ªêô¾ : ", pFont).Width

                        'pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                        'Common_Procedures.Print_To_PrintDocument(e, "dv â‡E‚¬è ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                        'pFont = New Font("Calibri", 11, FontStyle.Regular)
                        'Common_Procedures.Print_To_PrintDocument(e, " :  " & prn_HdDt.Rows(prn_HeadIndx).Item("Pcs").ToString, LMargin + C1 + W1 + 80, CurY, 0, 0, pFont)


                        'pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                        'Common_Procedures.Print_To_PrintDocument(e, "Ë™ ªêô¾ ", LMargin + C1 + 250, CurY, 0, 0, pFont)
                        'pFont = New Font("Calibri", 11, FontStyle.Regular)
                        'Common_Procedures.Print_To_PrintDocument(e, " :  " & prn_HdDt.Rows(prn_HeadIndx).Item("Total_Dgv_Weight").ToString, LMargin + C1 + 250 + W2 + 10, CurY, 0, 0, pFont)

                        NoofDets = NoofDets + 1

                        prn_DetIndx = prn_DetIndx + 1

                        ' CurY = CurY + TxtHgt + 10

                        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))

                    End If

                    Printing_Format2_GST_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)



                Else
                    NoofItems_PerPage = 19 '15

                End If

            Catch ex As Exception
                MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try

            If Val(Common_Procedures.settings.WeaverWages_Print_2Copy_In_SinglePage) = 1 Then
                If PCnt = 1 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then
                    If prn_DetDt.Rows.Count > 9 Then
                        PrntCnt2ndPageSTS = True
                        e.HasMorePages = True
                        Return
                    End If
                End If
            End If

            PrntCnt2ndPageSTS = False

        Next PCnt

        prn_Prev_HeadIndx = prn_HeadIndx
        prn_HeadIndx = prn_HeadIndx + 1

        If prn_HeadIndx <= prn_HdDt.Rows.Count - 1 Then
            e.HasMorePages = True
        Else
            e.HasMorePages = False
        End If

        prn_DetDt.Clear()

        prn_PageNo = 0

        prn_DetIndx = 0
        prn_DetSNo = 0

        prn_Tot_EBeam_Stk = 0
        prn_Tot_Pavu_Stk = 0
        prn_Tot_Yarn_Stk = 0
        prn_Tot_Amt_Bal = 0

    End Sub

    Private Sub Printing_Format2_GST_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim Led_Name As String = "", Led_Add As String = "", Led_Add1 As String = "", Led_Add2 As String = "", Led_Pan As String = "", Led_gst As String = ""
        Dim strHeight As Single, strWidth As Single = 0
        Dim C1, C2, S1, W1, W2 As Single
        Dim CurX As Single = 0

        PageNo = PageNo + 1

        CurY = TMargin


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_EMail = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""
        If Trim(UCase(prn_WagesFrmt)) = "FORMAT-1.2" Or Trim(UCase(prn_WagesFrmt)) = "FORMAT-1.3" Then
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_MainName").ToString
            Cmp_Add1 = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString
            Cmp_Add2 = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address4").ToString

        Else
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString
            Cmp_Add1 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString
            Cmp_Add2 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address4").ToString
            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_PhoneNo").ToString) <> "" Then
                Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(prn_HeadIndx).Item("Company_PhoneNo").ToString
            End If
            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_TinNo").ToString) <> "" Then
                Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_TinNo").ToString
            End If
            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_CstNo").ToString) <> "" Then
                Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_CstNo").ToString
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
        End If

        CurY = CurY + TxtHgt - 15
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & " / " & Cmp_EMail), LMargin, CurY, 2, PrintWidth, pFont)

        CurY = CurY + TxtHgt

        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_GSTIN_Cap), p1Font).Width
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No), pFont).Width
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
        'p1Font = New Font("Calibri", 10, FontStyle.Bold)
        'CurX = CurX + strWidth
        'Common_Procedures.Print_To_PrintDocument(e, "     " & Cmp_PanCap, CurX, CurY, 0, PrintWidth, p1Font)
        'strWidth = e.Graphics.MeasureString("     " & Cmp_PanCap, p1Font).Width
        'CurX = CurX + strWidth
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_PanNo, CurX, CurY, 0, 0, pFont)


        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 11, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "REVERSE CHARGE INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        If Trim(UCase(prn_WagesFrmt)) = "FORMAT-1.2" Or Trim(UCase(prn_WagesFrmt)) = "FORMAT-1.3" Then
            Led_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString
            Led_Add1 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString
            Led_Add2 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address4").ToString

            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address4").ToString) <> "" Then
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString) & " " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address4").ToString)
            ElseIf Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString) <> "" Then
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString) & " " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString)
            ElseIf Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString) <> "" Then
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address1").ToString) & " " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString)
            Else
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address1").ToString)
            End If

        Else
            Led_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_MainName").ToString
            Led_Add1 = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString
            Led_Add2 = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address4").ToString
            Led_Pan = prn_HdDt.Rows(prn_HeadIndx).Item("Pan_No").ToString
            Led_gst = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_GSTinNo").ToString

            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address4").ToString) <> "" Then
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString) & " " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address4").ToString)
            ElseIf Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString) <> "" Then
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString) & " " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString)
            ElseIf Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString) <> "" Then
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString) & " " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString)
            Else
                Led_Add = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString)
            End If

        End If

        C1 = ClAr(1) + ClAr(2) + 75
        C2 = C1 + ClAr(4) + 100
        S1 = e.Graphics.MeasureString("TO   :    ", pFont).Width
        W1 = e.Graphics.MeasureString("óC¶ ªï‹.   ", pFont).Width
        W2 = e.Graphics.MeasureString("óC¶ «îF   ", pFont).Width

        If Trim(UCase(prn_WagesFrmt)) = "FORMAT-1.3" Then

            CurY = CurY + TxtHgt
            pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "H™ ªï‹. ", LMargin + 10, CurY, 0, 0, pFont)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_BillNo").ToString, LMargin + W1 + 20, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_No").ToString, LMargin + W1 + 20, CurY, 0, 0, pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "H™ «îF ", LMargin + 140 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Date").ToString), "dd-MM-yyyy").ToString, LMargin + 140 + W2 + 20, CurY, 0, 0, pFont)

            pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
                Common_Procedures.Print_To_PrintDocument(e, "ªìLõK ªï‹.", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_No").ToString, LMargin + C1 + W1 + 40, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "óC¶ ªï‹. ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
                Common_Procedures.Print_To_PrintDocument(e, "ªìLõK «îF", LMargin + C2 + +10, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "óC¶ «îF ", LMargin + C2 + +10, CurY, 0, 0, pFont)
            End If

            '  Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Rec_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C2 + W2 + 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_Date").ToString, LMargin + C2 + W2 + 30, CurY, 0, 0, pFont)

            CurY = CurY + 5

        Else

            CurY = CurY + TxtHgt - 15
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & Led_Name, LMargin + 10, CurY, 0, 0, p1Font)
            pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "H™ ªï‹. ", LMargin + C1 + 10, CurY, 0, 0, pFont)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_BillNo").ToString, LMargin + C1 + W1 + 20, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_No").ToString, LMargin + C1 + W1 + 20, CurY, 0, 0, pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "H™ «îF ", LMargin + C2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C2 + W2 + 30, CurY, 0, 0, pFont)

            pFont = New Font("Calibri", 11, FontStyle.Regular)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & Led_Add1, LMargin + S1 + 10, CurY, 0, 0, pFont)

            pFont = New Font("Calibri", 11, FontStyle.Regular)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & Led_Add2, LMargin + S1 + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            If Led_Pan <> "" And Led_gst <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " " & "GSTIN : " & Led_gst & "  PAN : " & Led_Pan, LMargin + S1 + 10, CurY, 0, 0, pFont)
            ElseIf Led_Pan <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " " & "PAN : " & Led_Pan, LMargin + S1 + 10, CurY, 0, 0, pFont)
            ElseIf Led_gst <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " " & "GSTIN : " & Led_gst, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If


            pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
                Common_Procedures.Print_To_PrintDocument(e, "ªìLõK ªï‹.", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_No").ToString, LMargin + C1 + W1 + 40, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "óC¶ ªï‹. ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
                Common_Procedures.Print_To_PrintDocument(e, "ªìLõK «îF", LMargin + C2 + +10, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "óC¶ «îF ", LMargin + C2 + +10, CurY, 0, 0, pFont)
            End If

            'Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Rec_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C2 + W2 + 30, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_Date").ToString, LMargin + C2 + W2 + 30, CurY, 0, 0, pFont)

        End If

        CurY = CurY + TxtHgt

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

    End Sub

    Private Sub Printing_Format2_GST_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim Cmp_Name As String
        Dim p1Font As Font

        'For i = NoofDets + 1 To NoofItems_PerPage
        '    CurY = CurY + TxtHgt
        'Next
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        'S1 = e.Graphics.MeasureString("ªî£¬è Þ¼Š¹   : ", pFont).Width
        'CurY = CurY + 10
        'pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
        'Common_Procedures.Print_To_PrintDocument(e, "ªî£¬è Þ¼Š¹   : ", LMargin + 10, CurY, 0, 0, pFont)
        'pFont = New Font("Calibri", 11, FontStyle.Regular)
        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Net_Amount").ToString & "Cr", LMargin + S1 + 70, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        '  CurY = CurY + TxtHgt


        CurY = CurY + 10
        pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, "îPè£ó˜ ¬èªò£Šð‹  ", LMargin + 10, CurY, 0, 0, pFont)

        If Trim(UCase(prn_WagesFrmt)) = "FORMAT-1.2" Or Trim(UCase(prn_WagesFrmt)) = "FORMAT-1.3" Then
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_MainName").ToString
        Else
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString
        End If
        p1Font = New Font("Calibri", 12, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

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

            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Weaver.Text)

            PhNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "MobileNo_Frsms", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")

            'Agnt_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Agent.Text)
            'AgPNo = ""
            'If Val(Agnt_IdNo) <> 0 Then
            '    AgPNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_PhoneNo", "(Ledger_IdNo = " & Str(Val(Agnt_IdNo)) & ")")
            'End If

            'If Trim(AgPNo) <> "" Then
            '    PhNo = Trim(PhNo) & IIf(Trim(PhNo) <> "", ",", "") & Trim(AgPNo)
            'End If

            smstxt = Trim(cbo_Weaver.Text) & Chr(13)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then
                ' smstxt = smstxt & " Bill No : " & Trim(lbl_BillNo.Text) & Chr(13)
                smstxt = smstxt & " Bill Date : " & Trim(dtp_Date.Text) & Chr(13)
            Else
                smstxt = smstxt & " Bill No : " & Trim(lbl_BillNo.Text) & Chr(13)
                smstxt = smstxt & " Date : " & Trim(dtp_Date.Text) & Chr(13)
            End If
            ' smstxt = smstxt & " Quality : " & Trim(lbl_Cloth.Text) & Chr(13)

            If dgv_ReceiptDetails_Total.RowCount > 0 Then
                smstxt = smstxt & " Receipt Meters : " & Val(dgv_ReceiptDetails_Total.Rows(0).Cells(5).Value()) & Chr(13)
            End If



            'If dgv_Details.RowCount > 0 Then
            '    smstxt = smstxt & " No.Of Bales : " & Val((dgv_Details.Rows(0).Cells(4).Value())) & Chr(13)
            '    smstxt = smstxt & " Meters : " & Val((dgv_Details.Rows(0).Cells(7).Value())) & Chr(13)
            'End If
            smstxt = smstxt & " Net Amount : " & Trim(lbl_Net_Amount.Text) & Chr(13)
            smstxt = smstxt & " " & Chr(13)
            smstxt = smstxt & " Thanks! " & Chr(13)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then
                smstxt = smstxt & " GKT"

            Else
                smstxt = smstxt & Common_Procedures.Company_IdNoToName(con, Val(lbl_Company.Tag))

            End If
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
            f1.MdiParent = MdiParent1
            f1.Show()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND SMS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub btn_Print_WithStock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print_WithStock.Click
        prn_WagesFrmt = "FORMAT-1"
        prn_Frieght_Sts = True
        txt_PrintRange_FromNo.Text = prn_FromNo
        txt_PrintRange_ToNo.Text = prn_ToNo

        pnl_PrintRange.Visible = True
        pnl_Back.Enabled = False
        pnl_PrintOption2.Visible = False

        If txt_PrintRange_FromNo.Enabled Then txt_PrintRange_FromNo.Focus()
    End Sub

    Private Sub btn_Print_Simple_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print_Simple.Click
        prn_WagesFrmt = "FORMAT-2"
        prn_Frieght_Sts = True
        txt_PrintRange_FromNo.Text = prn_FromNo
        txt_PrintRange_ToNo.Text = prn_ToNo

        pnl_PrintRange.Visible = True
        pnl_Back.Enabled = False
        pnl_PrintOption2.Visible = False

        If txt_PrintRange_FromNo.Enabled Then txt_PrintRange_FromNo.Focus()
    End Sub

    Private Sub btn_Print_JobWork_Invoice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print_JobWork_Invoice.Click
        prn_WagesFrmt = "FORMAT-3"
        txt_PrintRange_FromNo.Text = prn_FromNo
        txt_PrintRange_ToNo.Text = prn_ToNo

        pnl_PrintRange.Visible = True
        pnl_Back.Enabled = False
        pnl_PrintOption2.Visible = False

        If txt_PrintRange_FromNo.Enabled Then txt_PrintRange_FromNo.Focus()
    End Sub

    Private Sub btn_Print_JobWork_Delivery_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print_JobWork_Delivery.Click
        prn_WagesFrmt = "FORMAT-4"
        txt_PrintRange_FromNo.Text = prn_FromNo
        txt_PrintRange_ToNo.Text = prn_ToNo

        pnl_PrintRange.Visible = True
        pnl_Back.Enabled = False
        pnl_PrintOption2.Visible = False

        If txt_PrintRange_FromNo.Enabled Then txt_PrintRange_FromNo.Focus()
    End Sub

    Private Sub btn_Cancel_PrintOption_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Cancel_PrintOption.Click
        btn_Close_PrintOption_Click(sender, e)
    End Sub


    Private Sub btn_Print_WithStock_Audit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print_WithStock_Audit.Click
        prn_WagesFrmt = "FORMAT-1"
        prn_Frieght_Sts = False
        txt_PrintRange_FromNo.Text = prn_FromNo
        txt_PrintRange_ToNo.Text = prn_ToNo

        pnl_PrintRange.Visible = True
        pnl_Back.Enabled = False
        pnl_PrintOption2.Visible = False

        If txt_PrintRange_FromNo.Enabled Then txt_PrintRange_FromNo.Focus()
    End Sub

    Private Sub btn_Print_Simple_Audit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print_Simple_Audit.Click
        prn_WagesFrmt = "FORMAT-2"
        prn_Frieght_Sts = False
        txt_PrintRange_FromNo.Text = prn_FromNo
        txt_PrintRange_ToNo.Text = prn_ToNo

        pnl_PrintRange.Visible = True
        pnl_Back.Enabled = False
        pnl_PrintOption2.Visible = False

        If txt_PrintRange_FromNo.Enabled Then txt_PrintRange_FromNo.Focus()
    End Sub

    Private Sub ConsYarn_Details()
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim Sno As Integer = 0
        Dim n As Integer = 0
        Dim Clt_IdNo As Integer = 0
        Dim EdsCnt_Nm As String = ""
        Dim Cnt_Nm As String = ""
        Dim Wgt_Mtr As String = 0
        Dim ConsYarn As String = 0
        Dim NewCode As String = ""
        Dim cloth_pick As Double = 0
        Dim cloth_width As Double = 0
        Dim Stk As String = ""
        Dim RdSp As Single = 0
        Dim Pick As Single = 0
        Dim Weft As Single = 0
        Dim vNOOFPCS As String
        Dim Mtrs As Single = 0
        Dim edscnt_id As Integer = 0
        Dim cnt_id As Integer = 0
        Dim slno As Integer = 0
        Dim Consmtrs As Single = 0
        Dim Pcs As Single = 0
        Dim Det_SLNo As Integer
        Dim VtotBobin As Single = 0, vTotKuri As Single = 0
        Dim ConsThri As Single = 0
        Dim vMultiplyr As Integer = 0
        Dim Crimp_Perc As Single = 0
        Dim WidTyp As Single = 0
        Dim Excess_Shrt As Single = 0
        Dim StkConsPavu As Single = 0
        Dim Lm_Id As Integer = 0
        Dim vTotMtrs As String = ""
        Dim vWEFT_CONS_FOR As String = ""


        Try

            If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub
            cmd.Connection = con

            cmd.CommandText = "Truncate table EntryTemp"
            cmd.ExecuteNonQuery()

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            With dgv_Receipt_Details

                If .Rows.Count > 0 Then

                    For i = 0 To .Rows.Count - 1


                        Clt_IdNo = Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(1).Value)
                        If Val(Clt_IdNo) <> 0 Or Val(.Rows(i).Cells(4).Value) <> 0 Then

                            Wgt_Mtr = 0
                            RdSp = 0
                            Crimp_Perc = 0
                            Stk = ""
                            vWEFT_CONS_FOR = ""
                            da = New SqlClient.SqlDataAdapter("select a.* from  Cloth_Head a where  a.Cloth_Idno = " & Val(Clt_IdNo) & " ", con)
                            dt = New DataTable
                            da.Fill(dt)
                            If dt.Rows.Count > 0 Then
                                EdsCnt_Nm = Common_Procedures.EndsCount_IdNoToName(con, Val(dt.Rows(0).Item("EndsCount_IdNo").ToString))
                                Cnt_Nm = Common_Procedures.Count_IdNoToName(con, Val(dt.Rows(0).Item("Cloth_WeftCount_IdNo").ToString))
                                Wgt_Mtr = Val(dt.Rows(0).Item("Weight_Meter_Weft").ToString)
                                RdSp = Val(dt.Rows(0).Item("Cloth_ReedSpace").ToString)
                                Crimp_Perc = Val(dt.Rows(0).Item("Crimp_Percentage").ToString)
                                Stk = dt.Rows(0).Item("Stock_In").ToString
                                vWEFT_CONS_FOR = dt.Rows(0).Item("Weaver_Weft_Consumption").ToString
                            End If

                            'RdSp = Val(Common_Procedures.get_FieldValue(con, "Cloth_Head", "Cloth_ReedSpace", "(cloth_name = '" & Trim(.Rows(i).Cells(1).Value) & "')"))  '2.5
                            If Val(RdSp) = 0 Then RdSp = 2.5
                            ''If Trim(cbo_LoomType.Text) = "AUTO LOOM" Then
                            ''    RdSp = 2.5 ' Val(Common_Procedures.get_FieldValue(con, "Cloth_Head", "Cloth_ReedSpace", "(cloth_name = '" & Trim(.Rows(i).Cells(1).Value) & "')"))
                            ''Else
                            ''    RdSp = 2.5
                            ''End If ' 
                            If Trim(UCase(cbo_LoomType.Text)) = "AUTO LOOM" Or Trim(UCase(cbo_LoomType.Text)) = "AUTOLOOM" Then

                                Crimp_Perc = Val(Common_Procedures.get_FieldValue(con, "Cloth_Head", "Crimp_Percentage", "(Cloth_IdNo = " & Str(Val(Clt_IdNo)) & ")"))

                                WidTyp = 0
                                If Trim(UCase(cbo_WidthType.Text)) = "FOURTH" Then
                                    WidTyp = 4
                                ElseIf Trim(UCase(cbo_WidthType.Text)) = "TRIPLE" Then
                                    WidTyp = 3
                                ElseIf Trim(UCase(cbo_WidthType.Text)) = "DOUBLE" Then
                                    WidTyp = 2
                                Else
                                    WidTyp = 1
                                End If
                                Excess_Shrt = Format(Val(.Rows(i).Cells(16).Value) - Val(.Rows(i).Cells(18).Value), "#################0.00")
                                StkConsPavu = (Val(Excess_Shrt) / Val(WidTyp)) * Val(txt_No_Of_Beams.Text)
                                Crimp_Perc = Val(StkConsPavu) * Crimp_Perc / 100
                                StkConsPavu = Format(StkConsPavu + Crimp_Perc, "#########0.00")

                            Else

                                Excess_Shrt = Format(Val(.Rows(i).Cells(16).Value) - Val(.Rows(i).Cells(18).Value), "#################0.00")
                                StkConsPavu = Val(Common_Procedures.get_Pavu_Consumption(con, Clt_IdNo, Lm_Id, Val(Excess_Shrt), Trim(WidTyp)))

                            End If

                            'Stk = (Common_Procedures.get_FieldValue(con, "Cloth_Head", "Stock_In", "(cloth_name = '" & Trim(.Rows(i).Cells(1).Value) & "')"))
                            If Val(Weft) = 0 Then
                                Weft = Val(.Rows(i).Cells(5).Value)
                            End If
                            cloth_pick = Val(.Rows(i).Cells(2).Value)
                            cloth_width = Val(.Rows(i).Cells(3).Value)
                            vNOOFPCS = Val(.Rows(i).Cells(4).Value)
                            Mtrs = Val(.Rows(i).Cells(16).Value)


                            If Trim(UCase(Stk)) = "PCS" Then
                                vMultiplyr = 100

                            Else
                                vMultiplyr = 110

                            End If

                            ConsYarn = 0
                            ConsThri = 0
                            If Val(Wgt_Mtr) <> 0 Then
                                ConsYarn = Format(Val(Wgt_Mtr), "############0.000")
                                If Trim(UCase(vWEFT_CONS_FOR)) = "PCS" Then
                                    ConsThri = Format(Val(vNOOFPCS) * Val(Wgt_Mtr), "############0.0")
                                Else
                                    ConsThri = Format(Val(Mtrs) * Val(Wgt_Mtr), "############0.0")
                                End If

                            Else

                                ConsYarn = Format(Val(cloth_pick * (cloth_width + RdSp) * vMultiplyr / 840), "############0.00000")
                                ConsYarn = Math.Floor(Val(ConsYarn))
                                ConsYarn = Format(Val(ConsYarn) / 1000, "############0.000")
                                ConsThri = Format(Val(Mtrs * Val(ConsYarn)), "############0.0")

                            End If


                            Pcs = Val(.Rows(i).Cells(15).Value)
                            Det_SLNo = Val(.Rows(i).Cells(0).Value)

                            dgv_BobinDetails.Rows.Clear()
                            slno = 0

                            da = New SqlClient.SqlDataAdapter("select a.*, b.EndsCount_Name  from Cloth_Bobin_Details a INNER JOIN EndsCount_Head b ON a.EndsCount_IdNo = b.EndsCount_IdNo  where a.Cloth_Idno = " & Str(Val(Clt_IdNo)), con)
                            dt3 = New DataTable
                            da.Fill(dt3)

                            If dt3.Rows.Count > 0 Then

                                For K = 0 To dt3.Rows.Count - 1
                                    slno = slno + 1
                                    n = dgv_BobinDetails.Rows.Add()
                                    dgv_BobinDetails.Rows(n).Cells(0).Value = Val(slno)
                                    dgv_BobinDetails.Rows(n).Cells(1).Value = dt3.Rows(K).Item("EndsCount_Name").ToString

                                    If Trim(UCase(Stk)) = "PCS" Then
                                        dgv_BobinDetails.Rows(n).Cells(2).Value = Format(Val(dt3.Rows(K).Item("Cloth_Consumption").ToString) * Val(Pcs), "########0.0")
                                    Else
                                        dgv_BobinDetails.Rows(n).Cells(2).Value = Format(Val(dt3.Rows(K).Item("Cloth_Consumption").ToString) * Val(Mtrs), "########0.0")
                                    End If
                                    dgv_BobinDetails.Rows(n).Cells(2).Value = Format(Val(dgv_BobinDetails.Rows(n).Cells(2).Value), "########0.000")

                                Next K

                            End If
                            dt3.Clear()
                            dt3.Dispose()
                            cmd.Connection = con
                            With dgv_BobinelectionDetails


LOOP1:
                                For j = 0 To .RowCount - 1

                                    If Val(.Rows(j).Cells(0).Value) = Val(Det_SLNo) Then

                                        If j = .Rows.Count - 1 Then
                                            For k = 0 To .ColumnCount - 1
                                                .Rows(j).Cells(k).Value = ""
                                            Next

                                        Else
                                            .Rows.RemoveAt(j)

                                        End If

                                        GoTo LOOP1

                                    End If

                                Next j

                                For j = 0 To dgv_BobinDetails.RowCount - 1

                                    If Trim(dgv_BobinDetails.Rows(j).Cells(1).Value) <> "" Then

                                        n = .Rows.Add()

                                        .Rows(n).Cells(0).Value = Val(Det_SLNo)
                                        .Rows(n).Cells(1).Value = dgv_BobinDetails.Rows(j).Cells(1).Value
                                        .Rows(n).Cells(2).Value = dgv_BobinDetails.Rows(j).Cells(2).Value
                                        '.Rows(n).Cells(3).Value = Val(dgv_StoppageSelection.Rows(I).Cells(3).Value)

                                    End If
                                Next j

                            End With




                            da = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name from Cloth_Kuri_Details a INNER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo where a.Cloth_Idno = " & Str(Val(Clt_IdNo)), con)
                            da.Fill(dt4)

                            dgv_KuriDetails.Rows.Clear()
                            slno = 0

                            If dt4.Rows.Count > 0 Then

                                For j = 0 To dt4.Rows.Count - 1
                                    slno = slno + 1
                                    n = dgv_KuriDetails.Rows.Add()
                                    dgv_KuriDetails.Rows(n).Cells(0).Value = Val(slno)
                                    dgv_KuriDetails.Rows(n).Cells(1).Value = dt4.Rows(j).Item("Count_Name").ToString
                                    If Trim(UCase(Stk)) = "PCS" Then
                                        dgv_KuriDetails.Rows(n).Cells(2).Value = Format(Val(dt4.Rows(j).Item("Cloth_Consumption").ToString) * Val(Pcs), "#########0.0")
                                    Else
                                        dgv_KuriDetails.Rows(n).Cells(2).Value = Format(Val(dt4.Rows(j).Item("Cloth_Consumption").ToString) * Val(Mtrs), "#########0.0")
                                    End If
                                    dgv_KuriDetails.Rows(n).Cells(2).Value = Format(Val(dgv_KuriDetails.Rows(n).Cells(2).Value), "#########0.000")

                                Next j

                            End If
                            dt4.Clear()
                            dt4.Dispose()

                            With dgv_KuriSelection_Details


LOOP2:
                                For j = 0 To .RowCount - 1

                                    If Val(.Rows(j).Cells(0).Value) = Val(Det_SLNo) Then

                                        If j = .Rows.Count - 1 Then
                                            For k = 0 To .ColumnCount - 1
                                                .Rows(j).Cells(k).Value = ""
                                            Next

                                        Else
                                            .Rows.RemoveAt(j)

                                        End If

                                        GoTo LOOP2

                                    End If

                                Next j

                                For j = 0 To dgv_KuriDetails.RowCount - 1

                                    If Trim(dgv_KuriDetails.Rows(j).Cells(1).Value) <> "" Then

                                        n = .Rows.Add()

                                        .Rows(n).Cells(0).Value = Val(Det_SLNo)
                                        .Rows(n).Cells(1).Value = dgv_KuriDetails.Rows(j).Cells(1).Value
                                        .Rows(n).Cells(2).Value = dgv_KuriDetails.Rows(j).Cells(2).Value
                                        '.Rows(n).Cells(3).Value = Val(dgv_StoppageSelection.Rows(I).Cells(3).Value)

                                    End If
                                Next j

                            End With
                            VtotBobin = 0
                            With dgv_BobinDetails
                                For k = 0 To .RowCount - 1

                                    If Val(.Rows(k).Cells(2).Value) <> 0 Then
                                        VtotBobin = VtotBobin + Val(.Rows(k).Cells(2).Value)
                                    End If
                                Next k

                            End With

                            vTotKuri = 0
                            With dgv_KuriDetails
                                For j = 0 To .RowCount - 1
                                    If Val(.Rows(j).Cells(2).Value) <> 0 Then
                                        vTotKuri = vTotKuri + Val(.Rows(j).Cells(2).Value)
                                    End If
                                Next j
                            End With

                            cmd.CommandText = "Insert into EntryTemp (  int1                                ,                  Name1                ,      Name2              ,           Name3       ,Meters7                          ,Meters8                     ,            Meters1                   ,Currency3                            ,                       Meters2                 ,               Meters3              ,              Meters4              ,                Meters5              ,           Meters6                  ,             Meters9                   ,         Meters10                    ,     Weight1       ,         Weight2      ,          Weight4      ,          Weight5    , Weight6                              , Weight7                ,  Weight8  ) " &
                                                "          Values     ( " & Val(.Rows(i).Cells(0).Value) & " ,'" & Trim(.Rows(i).Cells(1).Value) & "','" & Trim(EdsCnt_Nm) & "','" & Trim(Cnt_Nm) & "' ,   " & Val(cloth_pick) & "      ,    " & Val(cloth_width) & " ," & Val(.Rows(i).Cells(4).Value) & " ," & Val(.Rows(i).Cells(5).Value) & " ,  " & Str(Val(.Rows(i).Cells(6).Value)) & "  ," & Val(.Rows(i).Cells(7).Value) & "," & Val(.Rows(i).Cells(8).Value) & ", " & Val(.Rows(i).Cells(11).Value) & "," & Val(.Rows(i).Cells(12).Value) & "," & Val(.Rows(i).Cells(15).Value) & "," & Val(.Rows(i).Cells(16).Value) & "," & Val(Wgt_Mtr) & ", " & Val(ConsThri) & ", " & Val(VtotBobin) & ", " & Val(vTotKuri) & "," & Val(.Rows(i).Cells(18).Value) & ", " & Val(StkConsPavu) & " , " & Val(Excess_Shrt) & ") "
                            cmd.ExecuteNonQuery()

                        End If

                    Next

                End If

            End With



            With dgv_ConsYarn_Details

                .Rows.Clear()
                Sno = 0

                da = New SqlClient.SqlDataAdapter("select a.Name1 as Cloth_Name,a.Name2 as Ends_name ,a.Name3 as Count_Name, a.Meters7 as pick  ,a.Meters8 as width  , a.Meters1 as Rec_Pcs,a.Currency3  as mtr_per_pcs, a.Meters2 as Rec_Mtrs,a.Meters3 as Ty1_Pcs , a.Meters4 as Ty1_Mtrs, a.Meters5 as Ty2_Pcs , a.Meters6 as Ty2_Mtrs , a.Meters9 as Tot_Pcs , a.Meters10 as Tot_Mtrs, a.Weight1 as Wgt_Mtr, a.Weight2 as Consyarn, a.Weight4 as ConsBobin, a.Weight5 as ConsKuri ,a.Weight6 as ExcessShrt ,Weight7 as MtrsEShrt, Weight8 as ConsPavu from EntryTemp a   order by int1", con)
                dt = New DataTable
                da.Fill(dt)

                If dt.Rows.Count > 0 Then


                    For i = 0 To dt.Rows.Count - 1

                        n = .Rows.Add()

                        Sno = Sno + 1

                        .Rows(n).Cells(0).Value = Sno
                        .Rows(n).Cells(1).Value = dt.Rows(i).Item("Cloth_Name").ToString
                        .Rows(n).Cells(2).Value = dt.Rows(i).Item("pick").ToString
                        .Rows(n).Cells(3).Value = dt.Rows(i).Item("width").ToString
                        .Rows(n).Cells(4).Value = dt.Rows(i).Item("Ends_Name").ToString
                        .Rows(n).Cells(5).Value = dt.Rows(i).Item("Count_Name").ToString
                        .Rows(n).Cells(6).Value = Val(dt.Rows(i).Item("Rec_Pcs").ToString)
                        If Val(.Rows(n).Cells(6).Value) = 0 Then .Rows(n).Cells(6).Value = ""


                        .Rows(n).Cells(7).Value = Format(Val(dt.Rows(i).Item("mtr_per_pcs").ToString), "#############0.00")

                        .Rows(n).Cells(8).Value = Format(Val(dt.Rows(i).Item("Rec_Mtrs").ToString), "#############0.00")
                        If Val(.Rows(n).Cells(8).Value) = 0 Then .Rows(n).Cells(8).Value = ""


                        .Rows(n).Cells(9).Value = Val(dt.Rows(i).Item("Tot_Pcs").ToString)
                        If Val(.Rows(n).Cells(9).Value) = 0 Then .Rows(n).Cells(9).Value = ""


                        vTotMtrs = Format(Val(dt.Rows(i).Item("Tot_Mtrs").ToString), "############0.00")
                        If Val(dt.Rows(i).Item("mtr_per_pcs").ToString) <> 0 And Val(dt.Rows(i).Item("mtr_per_pcs").ToString) <> 1 Then
                            vTotMtrs = Format(Val(dt.Rows(i).Item("Tot_Mtrs").ToString) / 6 * 5.5, "############0.00")
                        End If
                        .Rows(n).Cells(10).Value = Format(Val(vTotMtrs), "############0.00")
                        '.Rows(n).Cells(10).Value = Format(Val(dt.Rows(i).Item("Tot_Mtrs").ToString), "############0.00")
                        If Val(.Rows(n).Cells(10).Value) = 0 Then .Rows(n).Cells(10).Value = ""

                        .Rows(n).Cells(11).Value = Format(Val(dt.Rows(i).Item("ConsYarn").ToString), "############0.000")
                        If Val(.Rows(n).Cells(11).Value) = 0 Then .Rows(n).Cells(11).Value = ""

                        .Rows(n).Cells(12).Value = Format(Val(dt.Rows(i).Item("ConsBobin").ToString), "############0.000")
                        If Val(.Rows(n).Cells(12).Value) = 0 Then .Rows(n).Cells(12).Value = ""

                        .Rows(n).Cells(13).Value = Format(Val(dt.Rows(i).Item("ConsKuri").ToString), "############0.000")
                        If Val(.Rows(n).Cells(13).Value) = 0 Then .Rows(n).Cells(13).Value = ""
                        .Rows(n).Cells(14).Value = Format(Val(dt.Rows(i).Item("ExcessShrt").ToString), "############0.00")
                        If Val(.Rows(n).Cells(14).Value) = 0 Then .Rows(n).Cells(14).Value = ""
                        .Rows(n).Cells(15).Value = Format(Val(dt.Rows(i).Item("Conspavu").ToString), "############0.00")
                        If Val(.Rows(n).Cells(15).Value) = 0 Then .Rows(n).Cells(15).Value = ""
                        .Rows(n).Cells(16).Value = Format(Val(dt.Rows(i).Item("MtrsEShrt").ToString), "############0.00")
                        If Val(.Rows(n).Cells(16).Value) = 0 Then .Rows(n).Cells(16).Value = ""
                    Next

                End If
                dt.Clear()

                dt.Dispose()
                da.Dispose()

            End With
            Grid_Cell_DeSelect()
            '   Total_Tax_Calculation()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try
    End Sub

    Private Sub txt_Remarks_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Remarks.KeyDown
        If e.KeyValue = 38 Then
            txt_Tds.Focus()
        End If

        If e.KeyValue = 40 Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Remarks_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then


            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub

    Private Sub dgv_ConsYarnDetails_Total_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_ConsYarnDetails_Total.CellContentClick

    End Sub

    Private Sub txt_PartyDcNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PartyDcNo.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then
            cbo_LoomType.Focus()
        End If
    End Sub

    Private Sub txt_PartyDcNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PartyDcNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_LoomType.Focus()
        End If
    End Sub
    Private Sub cbo_cloth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_cloth.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
        vCbo_ItmNm = cbo_cloth.Text
    End Sub
    Private Sub cbo_cloth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_cloth.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_cloth, Nothing, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

        With dgv_Receipt_Details

            If (e.KeyValue = 38 And cbo_cloth.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If .CurrentCell.RowIndex <= 0 Then
                    cbo_LoomType.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(13)
                    .CurrentCell.Selected = True
                End If

            End If

            If (e.KeyValue = 40 And cbo_cloth.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(1).Value) = "" Then
                    txt_CGST_Percentage.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                    .CurrentCell.Selected = True

                End If

            End If

        End With
    End Sub

    Private Sub cbo_cloth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_cloth.KeyPress

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim cLTH_Idno As Integer = 0
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_cloth, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            e.Handled = True


            BobinKuriConsumption_Calculation()
            'End If
            With dgv_Receipt_Details

                If Trim(UCase(vCbo_ItmNm)) <> Trim(UCase(cbo_cloth.Text)) Then
                    cLTH_Idno = Common_Procedures.Cloth_NameToIdNo(con, Trim(cbo_cloth.Text))

                    da = New SqlClient.SqlDataAdapter("select Cloth_Pick ,Cloth_Width,Meters_Pcs,Wages_For_Type1,Wages_For_Type2 , Cloth_Reed from Cloth_Head where Cloth_Idno = " & Str(Val(cLTH_Idno)) & " ", con)
                    da.Fill(dt)
                    If dt.Rows.Count > 0 Then

                        'If Val(.Rows(.CurrentRow.Index).Cells.Item(2).Value) = 0 Then
                        If IsDBNull(dt.Rows(0).Item("Cloth_Pick").ToString) = False Then
                            .Rows(.CurrentRow.Index).Cells(2).Value = dt.Rows(0).Item("Cloth_Pick").ToString
                        End If
                        'If
                        'If Val(.Rows(.CurrentRow.Index).Cells.Item(3).Value) = 0 Then
                        If IsDBNull(dt.Rows(0).Item("Cloth_Width").ToString) = False Then
                            .Rows(.CurrentRow.Index).Cells(3).Value = dt.Rows(0).Item("Cloth_Width").ToString
                        End If
                        'End If
                        'If Val(.Rows(.CurrentRow.Index).Cells.Item(5).Value) = 0 Then
                        If IsDBNull(dt.Rows(0).Item("Meters_Pcs").ToString) = False Then
                            .Rows(.CurrentRow.Index).Cells(5).Value = dt.Rows(0).Item("Meters_Pcs").ToString
                        End If
                        'End If
                        ' If Val(.Rows(.CurrentRow.Index).Cells.Item(9).Value) = 0 Then
                        If IsDBNull(dt.Rows(0).Item("Wages_For_Type1").ToString) = False Then
                            .Rows(.CurrentRow.Index).Cells(9).Value = dt.Rows(0).Item("Wages_For_Type1").ToString
                        End If
                        ' End If
                        'If Val(.Rows(.CurrentRow.Index).Cells.Item(13).Value) = 0 Then
                        If IsDBNull(dt.Rows(0).Item("Wages_For_Type2").ToString) = False Then
                            .Rows(.CurrentRow.Index).Cells(13).Value = dt.Rows(0).Item("Wages_For_Type2").ToString
                        End If

                        If IsDBNull(dt.Rows(0).Item("Cloth_Reed").ToString) = False Then
                            .Rows(.CurrentRow.Index).Cells(22).Value = dt.Rows(0).Item("Cloth_Reed").ToString
                        End If

                    End If
                End If
                dt.Clear()
            End With
            With dgv_Receipt_Details
                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    txt_CGST_Percentage.Focus()

                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(2)

                End If
            End With



        End If
    End Sub

    Private Sub cbo_cloth_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_cloth.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_cloth.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MdiParent1
            f.Show()
        End If
    End Sub
    Private Sub cbo_cloth_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_cloth.TextChanged

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim cLTH_Idno As Integer = 0

        Try
            If cbo_cloth.Visible Then
                With dgv_Receipt_Details
                    If Val(cbo_cloth.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_cloth.Text)

                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub


    Private Sub dgv_BobinDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BobinDetails.CellEndEdit
        dgv_BobinDetails_CellLeave(sender, e)
    End Sub

    Private Sub dgv_BobinDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BobinDetails.CellLeave
        With dgv_BobinDetails
            If .CurrentCell.ColumnIndex = 2 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
        End With
    End Sub

    Private Sub dgv_BobinDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_BobinDetails.CellValueChanged
        Dim TotCons As Single = 0

        On Error Resume Next
        If Not IsNothing(dgv_BobinDetails.CurrentCell) Then
            With dgv_BobinDetails
                If .Visible Then
                    If .CurrentCell.ColumnIndex = 2 Then
                        Total_BobinConsCalculation()




                    End If
                End If
            End With
        End If

    End Sub

    Private Sub dgv_BobinDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_BobinDetails.EditingControlShowing
        dgtxt_BobinDetails = CType(dgv_BobinDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_BobinDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_BobinDetails.KeyUp
        Dim i As Integer
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_BobinDetails

                n = .CurrentRow.Index

                If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                    For i = 0 To .ColumnCount - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(n)

                End If

            End With
            Total_BobinConsCalculation()
        End If
    End Sub

    Private Sub dgv_BobinDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_BobinDetails.LostFocus
        On Error Resume Next
        dgv_BobinDetails.CurrentCell.Selected = False

    End Sub

    Private Sub dgv_KuriDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_KuriDetails.CellEndEdit
        dgv_KuriDetails_CellLeave(sender, e)
    End Sub

    Private Sub dgv_KuriDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_KuriDetails.CellLeave
        With dgv_KuriDetails
            If .CurrentCell.ColumnIndex = 2 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                End If
            End If
        End With
    End Sub

    Private Sub dgv_KuriDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_KuriDetails.CellValueChanged
        Dim TotCons As Single = 0

        On Error Resume Next

        If Not IsNothing(dgv_KuriDetails.CurrentCell) Then
            With dgv_KuriDetails
                If .Visible Then
                    If .CurrentCell.ColumnIndex = 2 Then
                        Total_BobinConsCalculation()




                    End If
                End If
            End With
        End If

    End Sub

    Private Sub dgv_KuriDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_KuriDetails.EditingControlShowing
        dgtxt_KuriDetails = CType(dgv_KuriDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_KuriDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_KuriDetails.KeyUp
        Dim i As Integer
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_KuriDetails

                n = .CurrentRow.Index

                If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                    For i = 0 To .ColumnCount - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(n)

                End If

            End With
            Total_KuriConsCalculation()
        End If
    End Sub

    Private Sub dgv_KuriDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_KuriDetails.LostFocus
        On Error Resume Next
        dgv_KuriDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgtxt_BobinDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_BobinDetails.Enter
        dgv_ActCtrlName = dgv_BobinDetails.Name
        dgv_BobinDetails.EditingControl.BackColor = Color.Lime
        dgv_BobinDetails.EditingControl.ForeColor = Color.Blue
    End Sub

    Private Sub dgtxt_BobinDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_BobinDetails.KeyPress
        With dgv_BobinDetails
            If .Visible Then
                If .CurrentCell.ColumnIndex = 2 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                End If
            End If
        End With
    End Sub

    Private Sub dgtxt_KuriDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_KuriDetails.Enter
        dgv_ActCtrlName = dgv_KuriDetails.Name
        dgv_KuriDetails.EditingControl.BackColor = Color.Lime
        dgv_KuriDetails.EditingControl.ForeColor = Color.Blue
    End Sub

    Private Sub dgtxt_KuriDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_KuriDetails.KeyPress
        With dgv_KuriDetails
            If .Visible Then
                If .CurrentCell.ColumnIndex = 2 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                End If
            End If
        End With
    End Sub
    Private Sub Total_KuriConsCalculation()
        Dim TotPcs As Single, TotCons As Single

        TotPcs = 0
        TotCons = 0
        With dgv_KuriDetails

            For i = 0 To .RowCount - 1
                If Val(.Rows(i).Cells(2).Value) <> 0 Then
                    '  TotPcs = TotPcs + 1
                    TotCons = TotCons + Val(.Rows(i).Cells(2).Value)
                End If
            Next

        End With

        With dgv_KuriDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            ' .Rows(0).Cells(0).Value = Val(TotPcs)
            .Rows(0).Cells(2).Value = Format(Val(TotCons), "########0.00")
        End With



    End Sub
    Private Sub Total_BobinConsCalculation()
        Dim TotPcs As Single, TotCons As Single

        TotPcs = 0
        TotCons = 0
        With dgv_BobinDetails

            For i = 0 To .RowCount - 1
                If Val(.Rows(i).Cells(2).Value) <> 0 Then
                    '  TotPcs = TotPcs + 1
                    TotCons = TotCons + Val(.Rows(i).Cells(2).Value)
                End If
            Next

        End With

        With dgv_BobinDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            ' .Rows(0).Cells(0).Value = Val(TotPcs)
            .Rows(0).Cells(2).Value = Format(Val(TotCons), "########0.00")
        End With



    End Sub


    Private Sub BobinKuriConsumption_Calculation()
        Dim cmd As New SqlClient.SqlCommand
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim Clt_IdNo As Integer
        Dim edscnt_id As Integer = 0
        Dim cnt_id As Integer = 0
        Dim slno, n As Integer
        Dim mtrs As Single = 0
        Dim Pcs As Single = 0
        Dim Det_SLNo As Integer = 0
        Dim VtotBobin As Single = 0, vTotKuri As Single = 0

        On Error Resume Next

        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

        If IsNothing(dgv_ConsYarn_Details.CurrentCell) Then Exit Sub

        Clt_IdNo = Common_Procedures.Cloth_NameToIdNo(con, dgv_ConsYarn_Details.Rows(dgv_ConsYarn_Details.CurrentCell.RowIndex).Cells(1).Value)

        If Val(Clt_IdNo) <> 0 Then

            mtrs = Val(dgv_ConsYarn_Details.CurrentRow.Cells(10).Value)
            Pcs = Val(dgv_ConsYarn_Details.CurrentRow.Cells(9).Value)

            da = New SqlClient.SqlDataAdapter("select a.*, b.EndsCount_Name  from Cloth_Bobin_Details a INNER JOIN EndsCount_Head b ON a.EndsCount_IdNo = b.EndsCount_IdNo  where a.Cloth_Idno = " & Str(Val(Clt_IdNo)), con)
            da.Fill(dt3)

            dgv_BobinDetails.Rows.Clear()
            slno = 0

            If dt3.Rows.Count > 0 Then

                For i = 0 To dt3.Rows.Count - 1
                    slno = slno + 1
                    n = dgv_BobinDetails.Rows.Add()
                    dgv_BobinDetails.Rows(n).Cells(0).Value = Val(slno)
                    dgv_BobinDetails.Rows(n).Cells(1).Value = dt3.Rows(i).Item("EndsCount_Name").ToString

                    If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then
                        dgv_BobinDetails.Rows(n).Cells(2).Value = Format(Val(dt3.Rows(i).Item("Cloth_Consumption").ToString) * Val(Pcs), "########0.000")
                    Else
                        dgv_BobinDetails.Rows(n).Cells(2).Value = Format(Val(dt3.Rows(i).Item("Cloth_Consumption").ToString) * Val(mtrs), "########0.00")
                    End If

                Next i

            End If
            dt3.Clear()
            dt3.Dispose()
            ' Det_SLNo = Val(dgv_Receipt_Details.CurrentRow.Cells(23).Value)


            da = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name from Cloth_Kuri_Details a INNER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo where a.Cloth_Idno = " & Str(Val(Clt_IdNo)), con)
            da.Fill(dt4)

            dgv_KuriDetails.Rows.Clear()
            slno = 0

            If dt4.Rows.Count > 0 Then

                For i = 0 To dt4.Rows.Count - 1
                    slno = slno + 1
                    n = dgv_KuriDetails.Rows.Add()
                    dgv_KuriDetails.Rows(n).Cells(0).Value = Val(slno)
                    dgv_KuriDetails.Rows(n).Cells(1).Value = dt4.Rows(i).Item("Count_Name").ToString
                    If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then
                        dgv_KuriDetails.Rows(n).Cells(2).Value = Format(Val(dt4.Rows(i).Item("Cloth_Consumption").ToString) * Val(Pcs), "#######0.000")
                    Else
                        dgv_KuriDetails.Rows(n).Cells(2).Value = Format(Val(dt4.Rows(i).Item("Cloth_Consumption").ToString) * Val(mtrs), "#######0.000")
                    End If

                Next i

            End If
            dt4.Clear()
            dt4.Dispose()
            cmd.Connection = con
            '        With dgv_BobinelectionDetails


            'LOOP1:
            '            For I = 0 To .RowCount - 1

            '                If Val(.Rows(I).Cells(0).Value) = Val(Det_SLNo) Then

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

            '            For I = 0 To dgv_BobinDetails.RowCount - 1

            '                If Trim(dgv_BobinDetails.Rows(I).Cells(1).Value) <> "" Then

            '                    n = .Rows.Add()

            '                    .Rows(n).Cells(0).Value = Val(Det_SLNo)
            '                    .Rows(n).Cells(1).Value = dgv_BobinDetails.Rows(I).Cells(1).Value
            '                    .Rows(n).Cells(2).Value = dgv_BobinDetails.Rows(I).Cells(2).Value
            '                    '.Rows(n).Cells(3).Value = Val(dgv_StoppageSelection.Rows(I).Cells(3).Value)

            '                End If
            '            Next I

            '        End With

            '        With dgv_KuriSelection_Details


            'LOOP2:
            '            For I = 0 To .RowCount - 1

            '                If Val(.Rows(I).Cells(0).Value) = Val(Det_SLNo) Then

            '                    If I = .Rows.Count - 1 Then
            '                        For J = 0 To .ColumnCount - 1
            '                            .Rows(I).Cells(J).Value = ""
            '                        Next

            '                    Else
            '                        .Rows.RemoveAt(I)

            '                    End If

            '                    GoTo LOOP2

            '                End If

            '            Next I

            '            For I = 0 To dgv_KuriDetails.RowCount - 1

            '                If Trim(dgv_KuriDetails.Rows(I).Cells(1).Value) <> "" Then

            '                    n = .Rows.Add()

            '                    .Rows(n).Cells(0).Value = Val(Det_SLNo)
            '                    .Rows(n).Cells(1).Value = dgv_KuriDetails.Rows(I).Cells(1).Value
            '                    .Rows(n).Cells(2).Value = dgv_KuriDetails.Rows(I).Cells(2).Value
            '                    '.Rows(n).Cells(3).Value = Val(dgv_StoppageSelection.Rows(I).Cells(3).Value)

            '                End If
            '            Next I

            '        End With

            VtotBobin = 0
            With dgv_BobinDetails
                For I = 0 To .RowCount - 1

                    If Val(.Rows(I).Cells(2).Value) <> 0 Then
                        VtotBobin = VtotBobin + Val(.Rows(I).Cells(2).Value)
                    End If
                Next

            End With
            vTotKuri = 0
            With dgv_KuriDetails
                For I = 0 To .RowCount - 1

                    If Val(.Rows(I).Cells(2).Value) <> 0 Then
                        vTotKuri = vTotKuri + Val(.Rows(I).Cells(2).Value)
                    End If
                Next

            End With
        End If
        'dgv_Receipt_Details.CurrentRow.Cells(21).Value = Val(VtotBobin)
        'dgv_Receipt_Details.CurrentRow.Cells(22).Value = Val(vTotKuri)
    End Sub
    Private Sub Get_BobinDetails()
        ' Dim Det_SLNo As Integer
        Dim n As Integer, SLNo As Integer
        Dim Sht_ID As Integer = 0
        Dim Mch_ID As Integer = 0
        Dim Clt_IdNo As Integer = 0
        Dim mtrs As Single = 0
        Dim Pcs As Single = 0
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt3 As New DataTable
        Dim VtotBobin As Single = 0, vTotKuri As Single = 0
        Try


            If Trim(dgv_ConsYarn_Details.CurrentRow.Cells(1).Value) = "" Then
                MessageBox.Show("Invalid Cloth Name", "DOES NOT SHOW BOBIN DETAILS...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                dgv_ConsYarn_Details.Focus()
                If dgv_ConsYarn_Details.Rows.Count > 0 Then
                    dgv_ConsYarn_Details.CurrentCell = dgv_ConsYarn_Details.Rows(0).Cells(1)
                    dgv_ConsYarn_Details.CurrentCell.Selected = True
                End If
                Exit Sub
            End If
            Clt_IdNo = Common_Procedures.Cloth_NameToIdNo(con, dgv_ConsYarn_Details.CurrentRow.Cells(1).Value)
            mtrs = Val(dgv_ConsYarn_Details.CurrentRow.Cells(10).Value)
            Pcs = Val(dgv_ConsYarn_Details.CurrentRow.Cells(9).Value)

            da = New SqlClient.SqlDataAdapter("select a.*, b.EndsCount_Name  from Cloth_Bobin_Details a INNER JOIN EndsCount_Head b ON a.EndsCount_IdNo = b.EndsCount_IdNo  where a.Cloth_Idno = " & Str(Val(Clt_IdNo)), con)
            da.Fill(dt3)

            dgv_BobinDetails.Rows.Clear()
            slno = 0

            If dt3.Rows.Count > 0 Then

                For i = 0 To dt3.Rows.Count - 1
                    slno = slno + 1
                    n = dgv_BobinDetails.Rows.Add()
                    dgv_BobinDetails.Rows(n).Cells(0).Value = Val(slno)
                    dgv_BobinDetails.Rows(n).Cells(1).Value = dt3.Rows(i).Item("EndsCount_Name").ToString

                    If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then
                        dgv_BobinDetails.Rows(n).Cells(2).Value = Format(Val(dt3.Rows(i).Item("Cloth_Consumption").ToString) * Val(Pcs), "########0.000")
                    Else
                        dgv_BobinDetails.Rows(n).Cells(2).Value = Format(Val(dt3.Rows(i).Item("Cloth_Consumption").ToString) * Val(mtrs), "########0.00")
                    End If

                Next i

            End If

            '  Det_SLNo = Val(dgv_Receipt_Details.CurrentRow.Cells(23).Value)

            'With dgv_BobinDetails

            '    SLNo = 0


            '    For i = 0 To dgv_BobinelectionDetails.RowCount - 1
            '        If Det_SLNo = Val(dgv_BobinelectionDetails.Rows(i).Cells(0).Value) Then

            '            SLNo = SLNo + 1
            '            .Rows.Clear()
            '            n = .Rows.Add()
            '            .Rows(n).Cells(0).Value = SLNo
            '            .Rows(n).Cells(1).Value = Trim(dgv_BobinelectionDetails.Rows(i).Cells(1).Value)
            '            .Rows(n).Cells(2).Value = (dgv_BobinelectionDetails.Rows(i).Cells(2).Value)
            '            '.Rows(n).Cells(3).Value = Val(dgv_StoppageDetails.Rows(i).Cells(3).Value)

            '        End If
            '    Next i
            'End With

            Pnl_Back.Enabled = False
            pnl_Bobin_Details.Visible = True
            pnl_Bobin_Details.BringToFront()


            ' dgv_BobinDetails.Rows.Add()
            If dgv_BobinDetails.Rows.Count > 0 Then
                dgv_BobinDetails.Focus()
                dgv_BobinDetails.CurrentCell = dgv_BobinDetails.Rows(0).Cells(1)
                dgv_BobinDetails.CurrentCell.Selected = True
            End If
            Total_BobinConsCalculation()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT BOBIN...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btn_Close_BobinDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_BobinDetails.Click
        Dim cmd As New SqlClient.SqlCommand
        Dim I As Integer
        Dim Det_SLNo As Integer = 0
        Dim n As Integer
        Dim dgvDet_CurRow As Integer = 0
        Dim dgv_DetSlNo As Integer = 0
        Dim vtot As Long = 0
        Dim Sht_Mns As Long = 0
        Dim Sht_ID As Integer = 0
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable

        Det_SLNo = Val(dgv_ConsYarn_Details.CurrentRow.Cells(0).Value)

        cmd.Connection = con
        With dgv_BobinelectionDetails


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

            For I = 0 To dgv_BobinDetails.RowCount - 1

                If Trim(dgv_BobinDetails.Rows(I).Cells(1).Value) <> "" Then

                    n = .Rows.Add()

                    .Rows(n).Cells(0).Value = Val(Det_SLNo)
                    .Rows(n).Cells(1).Value = dgv_BobinDetails.Rows(I).Cells(1).Value
                    .Rows(n).Cells(2).Value = dgv_BobinDetails.Rows(I).Cells(2).Value
                    '.Rows(n).Cells(3).Value = Val(dgv_StoppageSelection.Rows(I).Cells(3).Value)

                End If
            Next I

        End With




        '        vtot = 0
        '        With dgv_BobinDetails
        '            For I = 0 To .RowCount - 1

        '                If Val(.Rows(I).Cells(2).Value) <> 0 Then
        '                    vtot = vtot + Val(.Rows(I).Cells(2).Value)
        '                End If
        '            Next

        '        End With
        '        With dgv_BobinDetails_Total
        '            If .RowCount = 0 Then .Rows.Add()
        '            ' .Rows(0).Cells(0).Value = Val(TotPcs)
        '            .Rows(0).Cells(2).Value = Format(Val(vtot), "########0.00")
        '        End With
        '        '   BobinKuriConsumption_Calculation()

        pnl_Back.Enabled = True
        pnl_Bobin_Details.Visible = False

        'If dgv_Receipt_Details.Enabled And dgv_Receipt_Details.Visible Then
        '    '
        '    dgv_Receipt_Details.Focus()
        '    If dgv_Receipt_Details.Rows.Count > 0 Then
        '        dgv_Receipt_Details.CurrentRow.Cells(21).Value = Val(vtot)

        '        dgv_Receipt_Details.CurrentCell = dgv_Receipt_Details.Rows(dgv_Receipt_Details.CurrentCell.RowIndex).Cells(22)
        '        dgv_Receipt_Details.CurrentCell.Selected = True

        '    End If

        'End If
    End Sub
    Private Sub Get_KuriDetails()
        Dim n As Integer, SlNo As Integer
        Dim Sht_ID As Integer = 0
        Dim Mch_ID As Integer = 0
        Dim Clt_IdNo As Integer = 0
        Dim dt As New DataTable
        Dim dt4 As New DataTable
        Dim VtotBobin As Single = 0, vTotKuri As Single = 0
        Dim mtrs As Single = 0
        Dim Pcs As Single = 0
        Dim da As New SqlClient.SqlDataAdapter
        Try


            If Trim(dgv_ConsYarn_Details.CurrentRow.Cells(1).Value) = "" Then
                MessageBox.Show("Invalid Cloth Name", "DOES NOT SHOW KURI DETAILS...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                dgv_ConsYarn_Details.Focus()
                If dgv_ConsYarn_Details.Rows.Count > 0 Then
                    dgv_ConsYarn_Details.CurrentCell = dgv_ConsYarn_Details.Rows(0).Cells(1)
                    dgv_ConsYarn_Details.CurrentCell.Selected = True
                End If
                Exit Sub
            End If

            Clt_IdNo = Common_Procedures.Cloth_NameToIdNo(con, dgv_ConsYarn_Details.CurrentRow.Cells(1).Value)
            mtrs = Val(dgv_ConsYarn_Details.CurrentRow.Cells(10).Value)
            Pcs = Val(dgv_ConsYarn_Details.CurrentRow.Cells(9).Value)
            da = New SqlClient.SqlDataAdapter("select a.*, b.Count_Name from Cloth_Kuri_Details a INNER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo where a.Cloth_Idno = " & Str(Val(Clt_IdNo)), con)
            da.Fill(dt4)

            dgv_KuriDetails.Rows.Clear()
            SlNo = 0

            If dt4.Rows.Count > 0 Then

                For i = 0 To dt4.Rows.Count - 1
                    SlNo = SlNo + 1
                    n = dgv_KuriDetails.Rows.Add()
                    dgv_KuriDetails.Rows(n).Cells(0).Value = Val(SlNo)
                    dgv_KuriDetails.Rows(n).Cells(1).Value = dt4.Rows(i).Item("Count_Name").ToString
                    If Common_Procedures.settings.Weaver_Zari_Kuri_Entries_Status = 1 Then
                        dgv_KuriDetails.Rows(n).Cells(2).Value = Format(Val(dt4.Rows(i).Item("Cloth_Consumption").ToString) * Val(Pcs), "#######0.000")
                    Else
                        dgv_KuriDetails.Rows(n).Cells(2).Value = Format(Val(dt4.Rows(i).Item("Cloth_Consumption").ToString) * Val(mtrs), "#######0.000")
                    End If

                Next i

            End If
            ' Det_SLNo = Val(dgv_Receipt_Details.CurrentRow.Cells(23).Value)

            'With dgv_KuriDetails

            '    SlNo = 0


            '    For i = 0 To dgv_KuriSelection_Details.RowCount - 1
            '        If Det_SLNo = Val(dgv_KuriSelection_Details.Rows(i).Cells(0).Value) Then

            '            SlNo = SlNo + 1
            '            .Rows.Clear()
            '            n = .Rows.Add()
            '            .Rows(n).Cells(0).Value = SlNo
            '            .Rows(n).Cells(1).Value = Trim(dgv_KuriSelection_Details.Rows(i).Cells(1).Value)
            '            .Rows(n).Cells(2).Value = Val(dgv_KuriSelection_Details.Rows(i).Cells(2).Value)
            '            '.Rows(n).Cells(3).Value = Val(dgv_StoppageDetails.Rows(i).Cells(3).Value)

            '        End If
            '    Next i
            'End With

            pnl_Back.Enabled = False
            pnl_Kuri_Details.Visible = True
            pnl_Kuri_Details.BringToFront()


            dgv_KuriDetails.Rows.Add()
            If dgv_KuriDetails.Rows.Count > 0 Then
                dgv_KuriDetails.Focus()
                dgv_KuriDetails.CurrentCell = dgv_KuriDetails.Rows(0).Cells(1)
                dgv_KuriDetails.CurrentCell.Selected = True
            End If

            Total_KuriConsCalculation()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SELECT BOBIN...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btn_Close_KuriDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_KuriDetails.Click
        Dim cmd As New SqlClient.SqlCommand
        Dim I As Integer
        Dim Det_SLNo As Integer = 0
        Dim n As Integer
        Dim dgvDet_CurRow As Integer = 0
        Dim dgv_DetSlNo As Integer = 0
        Dim vtot As Long = 0
        Dim Sht_Mns As Long = 0
        Dim Sht_ID As Integer = 0
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable

        Det_SLNo = Val(dgv_ConsYarn_Details.CurrentRow.Cells(0).Value)

        cmd.Connection = con
        With dgv_KuriSelection_Details


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

            For I = 0 To dgv_KuriDetails.RowCount - 1

                If Trim(dgv_KuriDetails.Rows(I).Cells(1).Value) <> "" Then

                    n = .Rows.Add()

                    .Rows(n).Cells(0).Value = Val(Det_SLNo)
                    .Rows(n).Cells(1).Value = dgv_KuriDetails.Rows(I).Cells(1).Value
                    .Rows(n).Cells(2).Value = dgv_KuriDetails.Rows(I).Cells(2).Value
                    '.Rows(n).Cells(3).Value = Val(dgv_StoppageSelection.Rows(I).Cells(3).Value)

                End If
            Next I

        End With

        pnl_Back.Enabled = True
        pnl_Kuri_Details.Visible = False




        'vtot = 0
        'With dgv_KuriDetails
        '    For I = 0 To .RowCount - 1

        '        If Val(.Rows(I).Cells(2).Value) <> 0 Then
        '            vtot = vtot + Val(.Rows(I).Cells(2).Value)
        '        End If
        '    Next

        'End With
        'With dgv_KuriDetails_Total
        '    If .RowCount = 0 Then .Rows.Add()
        '    ' .Rows(0).Cells(0).Value = Val(TotPcs)
        '    .Rows(0).Cells(2).Value = Format(Val(vtot), "########0.00")
        'End With
        'If dgv_Receipt_Details.Enabled And dgv_Receipt_Details.Visible Then
        '    '
        '    dgv_Receipt_Details.Focus()
        '    If dgv_Receipt_Details.Rows.Count > 0 Then
        '        dgv_Receipt_Details.CurrentRow.Cells(22).Value = Val(vtot)

        '        dgv_Receipt_Details.CurrentCell = dgv_Receipt_Details.Rows(dgv_Receipt_Details.CurrentCell.RowIndex + 1).Cells(1)
        '        dgv_Receipt_Details.CurrentCell.Selected = True

        '    End If

        'End If
    End Sub

    Private Sub dgv_Receipt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Receipt_Details.KeyUp
        Dim i As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Receipt_Details

                .Rows.RemoveAt(.CurrentRow.Index)

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(0).Value = i + 1
                Next

            End With

        End If
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            If (dgv_Receipt_Details.CurrentCell.ColumnIndex = 21) Then
                Get_BobinDetails()
            End If
        End If
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            If (dgv_Receipt_Details.CurrentCell.ColumnIndex = 22) Then
                Get_KuriDetails()
            End If
        End If
    End Sub
    Private Sub Set_Max_DetailsSlNo(ByVal RowNo As Integer, ByVal DetSlNo_ColNo As Integer)
        Dim MaxSlNo As Integer = 0
        Dim i As Integer

        With dgv_Receipt_Details
            For i = 0 To .Rows.Count - 1
                If Val(.Rows(i).Cells(DetSlNo_ColNo).Value) > Val(MaxSlNo) Then
                    MaxSlNo = Val(.Rows(i).Cells(DetSlNo_ColNo).Value)
                End If
            Next
            .Rows(RowNo).Cells(DetSlNo_ColNo).Value = Val(MaxSlNo) + 1
        End With

    End Sub

    Private Sub dgv_Receipt_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Receipt_Details.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub
    Private Sub Printing_Format7GST(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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
        If Gst_Status = 1 Then
            NoofItems_PerPage = 14 ' 6
        Else
            NoofItems_PerPage = 17 ' 6
        End If
        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(30) : ClArr(2) = 185 : ClArr(3) = 65 : ClArr(4) = 65 : ClArr(5) = 80 : ClArr(6) = 70 : ClArr(7) = 70 : ClArr(8) = 80
        ClArr(9) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8))

        TxtHgt = 18.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            prn_Prev_HeadIndx = prn_HeadIndx
            If prn_HdDt.Rows.Count > 0 Then
                If prn_HeadIndx <= prn_HdDt.Rows.Count - 1 Then
                    EntryCode = prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Code").ToString

                    Printing_Format7GST_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


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

                                Printing_Format7GST_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                                e.HasMorePages = True
                                Return

                            End If

                            prn_DetSNo = prn_DetSNo + 1


                            ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Name").ToString)
                            ItmNm2 = ""

                            If Len(ItmNm1) > 22 Then
                                For I = 22 To 1 Step -1
                                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 22
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                            End If

                            CurY = CurY + TxtHgt

                            SNo = SNo + 1
                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Type1_Pcs").ToString) > 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Type1_Pcs").ToString, LMargin + ClArr(1) + ClArr(2) + 4, CurY, 0, 0, pFont)
                            Else
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Type1_Meters").ToString, LMargin + ClArr(1) + ClArr(2) + 4, CurY, 0, 0, pFont)
                            End If

                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type1_Rate").ToString), "############0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 5, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, (prn_DetDt.Rows(prn_DetIndx).Item("Type1_Amount").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Type2_Pcs").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Type2_Rate").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Type2_Amount").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Total_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If




                            prn_DetIndx = prn_DetIndx + 1

                        Loop

                    End If


                    Printing_Format7GST_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)
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
            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        prn_HeadIndx = prn_HeadIndx + 1

        If prn_HeadIndx <= prn_HdDt.Rows.Count - 1 Then
            e.HasMorePages = True
        Else
            e.HasMorePages = False
        End If

    End Sub

    Private Sub Printing_Format7GST_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim C1 As Single, C2 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim CurY1 As Single = 0, CurX As Single = 0
        Dim S As String
        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Ledger_Name as Transport_Name from Yarn_Purchase_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Transport_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Purchase_Code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
        da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()
        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)


                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL"
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "DUPLICATE"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "TRIPLICATE"
                ElseIf Val(S) = 4 Then
                    prn_OriDupTri = "OFFICE COPY"
                Else
                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
                        prn_OriDupTri = Trim(prn_InpOpts)
                    End If
                End If
            End If


        End If
        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = "" : Cmp_EMail = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(prn_HeadIndx).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_CstNo").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_PanNo").ToString) <> "" Then
            Cmp_PanNo = "PAN NO.: " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_PanNo").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_EMail").ToString) <> "" Then
            Cmp_EMail = "Email : " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_EMail").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_State_Name").ToString) <> "" Then
            Cmp_StateCap = "STATE : "
            Cmp_StateNm = prn_HdDt.Rows(prn_HeadIndx).Item("Company_State_Name").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_State_Code").ToString) <> "" Then
            Cmp_StateCode = "CODE :" & prn_HdDt.Rows(prn_HeadIndx).Item("Company_State_Code").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt.Rows(prn_HeadIndx).Item("Company_GSTinNo").ToString
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
        Common_Procedures.Print_To_PrintDocument(e, "COOLY BILL", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 40
            C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
            W1 = e.Graphics.MeasureString("PURCHASE NO             : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO :  " & "M/s." & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "Bill No", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Bill Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_State_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_GSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " GSTIN : " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Owner_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " PROPRIETOR NAME : " & prn_HdDt.Rows(prn_HeadIndx).Item("Owner_Name").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Pan_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " PAN NO : " & prn_HdDt.Rows(prn_HeadIndx).Item("Pan_No").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If

            'CurY = CurY + TxtHgt + 10
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            'LnAr(3) = CurY

            'e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))
            'CurY = CurY + TxtHgt

            'Common_Procedures.Print_To_PrintDocument(e, "Agent Name", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)

            ''Common_Procedures.Print_To_PrintDocument(e, "Transport Name : " & Trim(prn_HdDt.Rows(0).Item("Transport_Name").ToString), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "Transport Name", LMargin + C2 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Name").ToString, LMargin + C2 + W1 + 30, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt + 10

            'DelvToName = Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("DeliveryTo_Idno").ToString))

            ''Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Delivery_Address2").ToString), LMargin + S1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "Delivery At", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, DelvToName, LMargin + W1 + 30, CurY, 0, 0, pFont)


            'Common_Procedures.Print_To_PrintDocument(e, "Rec No", LMargin + C2 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Delivery_Receipt_No").ToString, LMargin + C2 + W1 + 30, CurY, 0, 0, pFont)
            ''CurY = CurY + TxtHgt
            ''Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Delivery_Address3").ToString), LMargin + S1 + 10, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(4), LMargin + C1, LnAr(2))
            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "QUALITY", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SOUND", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SOUND", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY + TxtHgt - 5, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DAMAGED", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DAMAGED", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + TxtHgt - 5, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)

            CurY = CurY + TxtHgt + 20
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format7GST_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim C1 As Single, W1 As Single
        Dim vTaxPerc As Single = 0
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

            'CurY = CurY + TxtHgt - 10
            'Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 30, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "" & Val(prn_HdDt.Rows(0).Item("Total_Bags").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Weight").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Total_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)


            'CurY = CurY + TxtHgt - 10

            'CurY = CurY + TxtHgt
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            'LnAr(7) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6)
            W1 = e.Graphics.MeasureString("DISCOUNT    : ", pFont).Width
            CurY = CurY + TxtHgt - 10

            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "Sub Total", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Cooly").ToString), PageWidth - 10, CurY, 1, 0, pFont)

            End If
            If Gst_Status = 1 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Taxable", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Taxable_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)

                End If
                CurY = CurY + TxtHgt

                If Val(prn_HdDt.Rows(prn_HeadIndx).Item("CGST_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ " & Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("CGST_Percentage").ToString), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    If is_LastPage = True Then
                        Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                    End If

                Else
                    Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)

                End If

                CurY = CurY + TxtHgt

                If Val(prn_HdDt.Rows(prn_HeadIndx).Item("SGST_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ " & Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("SGST_Percentage").ToString), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    If is_LastPage = True Then
                        Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("SGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                    End If

                Else
                    Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)

                End If

            End If
            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Freight", LMargin + C1 + 10, CurY, 0, 0, pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Freight_Charge").ToString), PageWidth - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Add", LMargin + C1 + 10, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Add_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Less", LMargin + C1 + 10, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Less_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
            End If


            If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Assesable_Value").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Bill Amount ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Assesable_Value").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If

            End If
            CurY = CurY + TxtHgt

            If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Tds_perc_Calc").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Less : Tds  @ " & Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Tds_perc").ToString), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Tds_perc_Calc").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If

            Else
                Common_Procedures.Print_To_PrintDocument(e, "Less : Tds  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)

            End If


            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
            LnAr(8) = CurY
            CurY = CurY + TxtHgt - 10
            'Common_Procedures.Print_To_PrintDocument(e, "E & O.E", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL COOLY", LMargin + C1 + 10, CurY, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Net_Amount").ToString), PageWidth - 10, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
            LnAr(8) = CurY


            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
            CurY = CurY + TxtHgt - 10
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Net_Amount").ToString))
            BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "", "")

            Common_Procedures.Print_To_PrintDocument(e, "Rupees            : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt + 5
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

            CurY = CurY + TxtHgt - 10
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt


            CurY = CurY + TxtHgt



            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "Weaver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)



            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_ConsYarn_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_ConsYarn_Details.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub



    Private Sub dgv_ConsYarn_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_ConsYarn_Details.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            If (dgv_ConsYarn_Details.CurrentCell.ColumnIndex = 12) Then
                Get_BobinDetails()
            End If
        End If
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            If (dgv_ConsYarn_Details.CurrentCell.ColumnIndex = 13) Then
                Get_KuriDetails()
            End If
        End If
    End Sub

    Private Sub dgtxt_WagesDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_WagesDetails.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        With dgv_ConsYarn_Details
            If e.KeyValue = Keys.Delete Then

            End If
        End With
    End Sub

    Private Sub dgtxt_WagesDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_WagesDetails.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dgv_ConsYarn_Details_KeyUp(sender, e)
        End If
    End Sub
    Private Sub cbo_LoomType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_LoomType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub

    Private Sub cbo_LoomType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_LoomType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_LoomType, txt_PartyDcNo, cbo_WidthType, "", "", "", "")

    End Sub

    Private Sub cbo_LoomType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_LoomType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_LoomType, cbo_WidthType, "", "", "", "")

    End Sub

    Private Sub cbo_WidthType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_WidthType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub

    Private Sub cbo_WidthType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_WidthType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_WidthType, cbo_LoomType, txt_No_Of_Beams, "", "", "", "")

    End Sub

    Private Sub cbo_WidthType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_WidthType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_WidthType, txt_No_Of_Beams, "", "", "", "")

    End Sub

    Private Sub dgv_Receipt_Details_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Receipt_Details.CellContentClick

    End Sub

    Private Sub txt_No_Of_Beams_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_No_Of_Beams.KeyDown
        If (e.KeyValue = 40) Then
            If dgv_Receipt_Details.Rows.Count > 0 Then
                dgv_Receipt_Details.Focus()
                dgv_Receipt_Details.CurrentCell = dgv_Receipt_Details.Rows(0).Cells(1)
                dgv_Receipt_Details.CurrentCell.Selected = True

            Else
                txt_CGST_Percentage.Focus()

            End If
        End If
    End Sub

    Private Sub txt_No_Of_Beams_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_No_Of_Beams.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If dgv_Receipt_Details.Rows.Count > 0 Then
                dgv_Receipt_Details.Focus()
                dgv_Receipt_Details.CurrentCell = dgv_Receipt_Details.Rows(0).Cells(1)
                dgv_Receipt_Details.CurrentCell.Selected = True

            Else
                txt_CGST_Percentage.Focus()

            End If
        End If
    End Sub

    Private Sub cbo_LoomType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_LoomType.TextChanged
        On Error Resume Next

        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub
        If Trim(UCase(cbo_LoomType.Text)) = "AUTO LOOM" Then
            txt_No_Of_Beams.Text = "2"
            dgv_Receipt_Details.Columns(6).HeaderText = "REC METERS"
        Else
            txt_No_Of_Beams.Text = "1"
            dgv_Receipt_Details.Columns(6).HeaderText = "REC YARDS"
        End If
    End Sub

    Private Sub txt_PartyDcNo_TextChanged(sender As Object, e As EventArgs) Handles txt_PartyDcNo.TextChanged

    End Sub

    Private Sub cbo_cloth_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_cloth.SelectedIndexChanged

    End Sub

    Private Sub chk_Tds_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_Tds.CheckedChanged
        If chk_Tds.Checked = True Then
            txt_Tds_Amount.Enabled = True
        Else
            txt_Tds_Amount.Enabled = False
            TdsCommision_Calculation()
        End If
    End Sub

    Private Sub btn_Print_Gst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Gst.Click
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim I As Integer = 0
        Dim OrdBy_FrmNo As Single = 0, OrdByToNo As Single = 0
        Gst_Status = 1
        prn_DmgAmt_STS = False
        Try

            If Val(txt_PrintRange_FromNo.Text) = 0 Then
                MessageBox.Show("Invalid From No", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txt_PrintRange_FromNo.Focus()
                Exit Sub
            End If

            If Val(txt_PrintRange_ToNo.Text) = 0 Then
                MessageBox.Show("Invalid To No", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txt_PrintRange_ToNo.Focus()
                Exit Sub
            End If

            prn_FromNo = Trim(txt_PrintRange_FromNo.Text)
            prn_ToNo = Trim(txt_PrintRange_ToNo.Text)

            btn_Close_PrintRange_Click(sender, e)

            printing_WeaverWages()

        Catch ex As Exception
            MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT PRINT...")

        Finally
            dt1.Dispose()
            da1.Dispose()

        End Try




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


            cmd.Connection = con
            If IsNothing(sqltr) = False Then
                cmd.Transaction = sqltr
            End If

            vPrnSTS = 0
            If chk_Printed.Checked = True Then
                vPrnSTS = 1
            End If

            NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


            cmd.CommandText = "Update Weaver_Wages_Head set PrintOut_Status = " & Str(Val(vPrnSTS)) & " where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code  = '" & Trim(NewCode) & "'"
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


    Private Sub chk_TaxABLEAmount_RoundOff_STS_CheckedChanged(sender As Object, e As System.EventArgs) Handles chk_TaxABLEAmount_RoundOff_STS.CheckedChanged
        With dgv_Receipt_Details
            For i = 0 To .Rows.Count - 1
                Calculation_Grid_Amount_Calculation(i, 6)
            Next
        End With
    End Sub

    Private Sub chk_TaxAmount_RoundOff_STS_CheckedChanged(sender As Object, e As System.EventArgs) Handles chk_TaxAmount_RoundOff_STS.CheckedChanged
        Total_Amount_Calculation()
    End Sub

    Private Sub Calculation_Grid_Amount_Calculation(CurRw As Integer, CurCol As Integer)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim Clo_Mtrs_Pc As Single = 0
        Dim CloID As Integer = 0
        Dim Stkin As String = ""
        Dim Cloth_Id As Integer

        On Error Resume Next

        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

        With dgv_Receipt_Details

            If .Visible Then

                If CurCol = 2 Or CurCol = 3 Or CurCol = 4 Or CurCol = 5 Or CurCol = 6 Or CurCol = 7 Or CurCol = 8 Or CurCol = 9 Or CurCol = 10 Or CurCol = 11 Or CurCol = 12 Or CurCol = 13 Or CurCol = 18 Then

                    CloID = Common_Procedures.Cloth_NameToIdNo(con, .Rows(CurRw).Cells(1).Value)

                    Clo_Mtrs_Pc = Val(.Rows(CurRw).Cells(5).Value)
                    Stkin = Trim(Common_Procedures.get_FieldValue(con, "Cloth_Head", "Stock_In", "(Cloth_idno = " & Str(Val(CloID)) & ")"))
                    .Rows(CurRw).Cells(7).Value = Format(Val(.Rows(CurRw).Cells(4).Value), "##########0.00")
                    If Trim(UCase(Stkin)) = "PCS" Then
                        .Rows(CurRw).Cells(6).Value = Format(Val(Clo_Mtrs_Pc) * Val(.Rows(CurRw).Cells(4).Value), "##########0.00")
                        .Rows(CurRw).Cells(8).Value = Format(Val(Clo_Mtrs_Pc) * Val(.Rows(CurRw).Cells(7).Value), "##########0.00")
                        .Rows(CurRw).Cells(12).Value = Format(Val(Clo_Mtrs_Pc) * Val(.Rows(CurRw).Cells(11).Value), "##########0.00")
                    End If



                    .Rows(CurRw).Cells(8).Value = Format(Val(.Rows(CurRw).Cells(6).Value), "##########0.00")
                    .Rows(CurRw).Cells(15).Value = Val(.Rows(CurRw).Cells(7).Value)
                    .Rows(CurRw).Cells(16).Value = Format(Val(.Rows(CurRw).Cells(8).Value), "##########0.00")

                    If Trim(UCase(Stkin)) = "METER" Then
                        .Rows(CurRw).Cells(10).Value = Format(Val(.Rows(CurRw).Cells(8).Value) * Val(.Rows(CurRw).Cells(9).Value), "##########0.00")
                        .Rows(CurRw).Cells(14).Value = Format(Val(.Rows(CurRw).Cells(12).Value) * Val(.Rows(CurRw).Cells(13).Value), "##########0.00")

                    Else
                        .Rows(CurRw).Cells(10).Value = Format(Val(.Rows(CurRw).Cells(7).Value) * Val(.Rows(CurRw).Cells(9).Value), "##########0.00")
                        .Rows(CurRw).Cells(14).Value = Format(Val(.Rows(CurRw).Cells(11).Value) * Val(.Rows(CurRw).Cells(13).Value), "##########0.00")

                    End If
                    .Rows(CurRw).Cells(17).Value = Format(Val(.Rows(CurRw).Cells(10).Value) - (Val(.Rows(CurRw).Cells(14).Value)), "###########0")

                    .Rows(CurRw).Cells(17).Value = Format(Val(.Rows(CurRw).Cells(17).Value), "###########0.00")



                    If chk_TaxABLEAmount_RoundOff_STS.Checked = False Then
                        .Rows(CurRw).Cells(17).Value = Format(Val(.Rows(CurRw).Cells(10).Value) - (Val(.Rows(CurRw).Cells(14).Value)), "###########0.00")
                    End If

                    Calculation_Total_ReceiptMeter()
                    ConsYarn_Details()
                    Calculation_Total_ConsumedYarnDetails()

                End If

            End If

        End With

    End Sub
    Private Sub Printing_Format8_GST_1414(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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

        pFont = New Font("Calibri", 9, FontStyle.Regular)

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
        If Gst_Status = 1 Then
            NoofItems_PerPage = 14 ' 6
        Else
            NoofItems_PerPage = 17 ' 6
        End If
        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = Val(30) : ClArr(2) = 185 : ClArr(3) = 70 : ClArr(4) = 50 : ClArr(5) = 50 : ClArr(6) = 70 : ClArr(7) = 70 : ClArr(8) = 65 : ClArr(9) = 65
        ClArr(10) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9))

        TxtHgt = 18.5 ' 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        vprn_Tot_Sound_Mtr = 0
        vprn_Tot_Consum_thiri = 0
        vprn_Tot_Excess_shrt_Mtr = 0
        vprn_Tot_Damage_Pcs = 0
        vprn_Tot_Amount = 0
        vprn_Tot_Damage_Amt = 0

        Try
            prn_Prev_HeadIndx = prn_HeadIndx
            If prn_HdDt.Rows.Count > 0 Then
                If prn_HeadIndx <= prn_HdDt.Rows.Count - 1 Then
                    EntryCode = prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Code").ToString

                    Printing_Format8_GST_1414_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)


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

                                Printing_Format8_GST_1414_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, False)

                                e.HasMorePages = True
                                Return

                            End If

                            prn_DetSNo = prn_DetSNo + 1


                            ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Name").ToString)
                            ItmNm2 = ""

                            If Len(ItmNm1) > 22 Then
                                For I = 22 To 1 Step -1
                                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 22
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                            End If

                            CurY = CurY + TxtHgt

                            SNo = SNo + 1
                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(SNo)), LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Type1_Pcs").ToString) > 0 Then
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Type1_Pcs").ToString, LMargin + ClArr(1) + ClArr(2) + 4, CurY, 0, 0, pFont)
                                vprn_Tot_Sound_Mtr = vprn_Tot_Sound_Mtr + Val(prn_DetDt.Rows(prn_DetIndx).Item("Type1_Pcs").ToString)

                            Else
                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Type1_Meters").ToString, LMargin + ClArr(1) + ClArr(2) + 4, CurY, 0, 0, pFont)
                                vprn_Tot_Sound_Mtr = vprn_Tot_Sound_Mtr + Val(prn_DetDt.Rows(prn_DetIndx).Item("Type1_Meters").ToString)
                            End If


                            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Meter_per_PCs").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 5, CurY, 1, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type1_Rate").ToString), "############0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("consumed_Thiri").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                            vprn_Tot_Consum_thiri = vprn_Tot_Consum_thiri + Val(prn_DetDt.Rows(prn_DetIndx).Item("consumed_Thiri").ToString)

                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cons_Yarn_Excess_short_Meter").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                            vprn_Tot_Excess_shrt_Mtr = vprn_Tot_Excess_shrt_Mtr + Val(prn_DetDt.Rows(prn_DetIndx).Item("Cons_Yarn_Excess_short_Meter").ToString)

                            Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Type2_Pcs").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                            vprn_Tot_Damage_Pcs = vprn_Tot_Damage_Pcs + Val(prn_DetDt.Rows(prn_DetIndx).Item("Type2_Pcs").ToString)


                            If prn_DmgAmt_STS = True Then

                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Type2_Amount").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                                vprn_Tot_Damage_Amt = vprn_Tot_Damage_Amt + Val(prn_DetDt.Rows(prn_DetIndx).Item("Type2_Amount").ToString)

                            Else

                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Type2_Rate").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)


                            End If



                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Total_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                            vprn_Tot_Amount = vprn_Tot_Amount + Val(prn_DetDt.Rows(prn_DetIndx).Item("Total_Amount").ToString)


                            'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type1_Rate").ToString), "############0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 5, CurY, 1, 0, pFont)

                            '             Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("consumed_Thiri").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
                            '             vprn_Tot_Consum_thiri = vprn_Tot_Consum_thiri + Val(prn_DetDt.Rows(prn_DetIndx).Item("consumed_Thiri").ToString)


                            '             Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Cons_Yarn_Excess_short_Meter").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                            '             vprn_Tot_Excess_shrt_Mtr = vprn_Tot_Excess_shrt_Mtr + Val(prn_DetDt.Rows(prn_DetIndx).Item("Cons_Yarn_Excess_short_Meter").ToString)

                            '             Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Meter_per_PCs").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)

                            '             Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("Type2_Pcs").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                            '             vprn_Tot_Damage_Pcs = vprn_Tot_Damage_Pcs + Val(prn_DetDt.Rows(prn_DetIndx).Item("Type2_Pcs").ToString)

                            '             Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Type2_Rate").ToString, LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)

                            '             Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Total_Amount").ToString), "#########.00"), PageWidth - 10, CurY, 1, 0, pFont)
                            '             vprn_Tot_Amount = vprn_Tot_Amount + Val(prn_DetDt.Rows(prn_DetIndx).Item("Total_Amount").ToString)

                            NoofDets = NoofDets + 1

                            If Trim(ItmNm2) <> "" Then
                                CurY = CurY + TxtHgt - 5
                                Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                NoofDets = NoofDets + 1
                            End If

                            prn_DetIndx = prn_DetIndx + 1

                        Loop

                    End If


                    Printing_Format8_GST_1414_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, True)
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
            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        prn_HeadIndx = prn_HeadIndx + 1

        If prn_HeadIndx <= prn_HdDt.Rows.Count - 1 Then
            e.HasMorePages = True

            vprn_Tot_Amount = String.Empty
            vprn_Tot_Damage_Pcs = String.Empty
            vprn_Tot_Consum_thiri = String.Empty
            vprn_Tot_Excess_shrt_Mtr = String.Empty
            vprn_Tot_Sound_Mtr = String.Empty

        Else
            e.HasMorePages = False
        End If

    End Sub

    Private Sub Printing_Format8_GST_1414_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single = 0, strWidth As Single = 0
        Dim C1 As Single, C2 As Single, W1 As Single, S1 As Single
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_PanNo As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_EMail As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim CurY1 As Single = 0, CurX As Single = 0
        Dim S As String
        PageNo = PageNo + 1

        CurY = TMargin

        da2 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name, c.Ledger_Name as Transport_Name from Yarn_Purchase_Head a  INNER JOIN Ledger_Head b ON b.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Transport_IdNo  where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Yarn_Purchase_Code = '" & Trim(EntryCode) & "' Order by a.For_OrderBy", con)
                            da2.Fill(dt2)
        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()
        prn_Count = prn_Count + 1

        prn_OriDupTri = ""
        If Trim(prn_InpOpts) <> "" Then
            If prn_Count <= Len(Trim(prn_InpOpts)) Then

                S = Mid$(Trim(prn_InpOpts), prn_Count, 1)


                If Val(S) = 1 Then
                    prn_OriDupTri = "ORIGINAL"
                ElseIf Val(S) = 2 Then
                    prn_OriDupTri = "DUPLICATE"
                ElseIf Val(S) = 3 Then
                    prn_OriDupTri = "TRIPLICATE"
                ElseIf Val(S) = 4 Then
                    prn_OriDupTri = "OFFICE COPY"
                Else
                    If Trim(prn_InpOpts) <> "0" And Val(prn_InpOpts) = "0" And Len(Trim(prn_InpOpts)) > 3 Then
                        prn_OriDupTri = Trim(prn_InpOpts)
                    End If
                End If
            End If


        End If
        If Trim(prn_OriDupTri) <> "" Then
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_OriDupTri), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_PanNo = "" : Cmp_EMail = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString
        Cmp_Add1 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address2").ToString
        Cmp_Add2 = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_Address4").ToString
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(prn_HeadIndx).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_TinNo").ToString) <> "" Then
            Cmp_TinNo = "TIN NO.: " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_TinNo").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_CstNo").ToString) <> "" Then
            Cmp_CstNo = "CST NO.: " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_CstNo").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_PanNo").ToString) <> "" Then
            Cmp_PanNo = "PAN NO.: " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_PanNo").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_EMail").ToString) <> "" Then
            Cmp_EMail = "Email : " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_EMail").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_State_Name").ToString) <> "" Then
            Cmp_StateCap = "STATE : "
            Cmp_StateNm = prn_HdDt.Rows(prn_HeadIndx).Item("Company_State_Name").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_State_Code").ToString) <> "" Then
            Cmp_StateCode = "CODE :" & prn_HdDt.Rows(prn_HeadIndx).Item("Company_State_Code").ToString
        End If
        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTIN_Cap = "GSTIN : "
            Cmp_GSTIN_No = prn_HdDt.Rows(prn_HeadIndx).Item("Company_GSTinNo").ToString
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
        Common_Procedures.Print_To_PrintDocument(e, "COOLY BILL", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height


        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 40
            C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
            W1 = e.Graphics.MeasureString("PURCHASE NO             : ", pFont).Width
            S1 = e.Graphics.MeasureString("TO     :    ", pFont).Width

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO :  " & "M/s." & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "Bill No", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address1").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address2").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Bill Date", LMargin + C1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Address3").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            If Trim(prn_HdDt.Rows(0).Item("P_Dc_No").ToString) <> "" Then


                Common_Procedures.Print_To_PrintDocument(e, " Party DcNo", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("P_Dc_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            End If

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_State_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If



            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_GSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " GSTIN : " & prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_GSTinNo").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If
            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Owner_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " PROPRIETOR NAME : " & prn_HdDt.Rows(prn_HeadIndx).Item("Owner_Name").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Pan_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, " PAN NO : " & prn_HdDt.Rows(prn_HeadIndx).Item("Pan_No").ToString, LMargin + S1 + 10, CurY, 0, 0, pFont)
            End If



            'CurY = CurY + TxtHgt + 10
            'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            'LnAr(3) = CurY

            'e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))
            'CurY = CurY + TxtHgt

            'Common_Procedures.Print_To_PrintDocument(e, "Agent Name", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Agent_Name").ToString, LMargin + W1 + 30, CurY, 0, 0, pFont)

            ''Common_Procedures.Print_To_PrintDocument(e, "Transport Name : " & Trim(prn_HdDt.Rows(0).Item("Transport_Name").ToString), LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "Transport Name", LMargin + C2 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transport_Name").ToString, LMargin + C2 + W1 + 30, CurY, 0, 0, pFont)

            'CurY = CurY + TxtHgt + 10

            'DelvToName = Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("DeliveryTo_Idno").ToString))

            ''Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(0).Item("Delivery_Address2").ToString), LMargin + S1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "Delivery At", LMargin + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, DelvToName, LMargin + W1 + 30, CurY, 0, 0, pFont)


            'Common_Procedures.Print_To_PrintDocument(e, "Rec No", LMargin + C2 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W1 + 10, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Delivery_Receipt_No").ToString, LMargin + C2 + W1 + 30, CurY, 0, 0, pFont)
            ''CurY = CurY + TxtHgt
            ''Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(0).Item("Delivery_Address3").ToString), LMargin + S1 + 10, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(4), LMargin + C1, LnAr(2))
            CurY = CurY + TxtHgt - 5
            Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "QUALITY", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "SOUND", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "MARK", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "SOUND", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + TxtHgt - 5, 2, ClAr(5), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "CONS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "THIRI", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + TxtHgt - 5, 2, ClAr(6), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "EX/SHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "MTR", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY + TxtHgt - 5, 2, ClAr(7), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "DAMAGED", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

            If prn_DmgAmt_STS = True Then

                Common_Procedures.Print_To_PrintDocument(e, "DAMAGED", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
                Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY + TxtHgt - 5, 2, ClAr(9), pFont)

            Else

                Common_Procedures.Print_To_PrintDocument(e, "DAMAGED", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
                Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY + TxtHgt - 5, 2, ClAr(9), pFont)

            End If



            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)

            'Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin, CurY, 2, ClAr(1), pFont)

            '          Common_Procedures.Print_To_PrintDocument(e, "QUALITY", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)

            '          Common_Procedures.Print_To_PrintDocument(e, "SOUND", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)

            '          Common_Procedures.Print_To_PrintDocument(e, "SOUND", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            '          Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY + TxtHgt - 5, 2, ClAr(4), pFont)

            '          Common_Procedures.Print_To_PrintDocument(e, "CONS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            '          Common_Procedures.Print_To_PrintDocument(e, "THIRI", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY + TxtHgt - 5, 2, ClAr(5), pFont)

            '          Common_Procedures.Print_To_PrintDocument(e, "EX/SHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            '          Common_Procedures.Print_To_PrintDocument(e, "MTR", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY + TxtHgt - 5, 2, ClAr(6), pFont)

            '          Common_Procedures.Print_To_PrintDocument(e, "MARK", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)

            '          Common_Procedures.Print_To_PrintDocument(e, "DAMAGED", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

            '          Common_Procedures.Print_To_PrintDocument(e, "DAMAGED", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)

            '          Common_Procedures.Print_To_PrintDocument(e, "RATE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY + TxtHgt - 5, 2, ClAr(9), pFont)

            '          Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)

            CurY = CurY + TxtHgt + 15
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format8_GST_1414_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim I As Integer
        Dim Cmp_Name As String
        Dim C1 As Single, W1 As Single
        Dim vTaxPerc As Single = 0
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

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + 50, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(vprn_Tot_Sound_Mtr), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 5, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(vprn_Tot_Consum_thiri), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(vprn_Tot_Excess_shrt_Mtr), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)


            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(vprn_Tot_Damage_Pcs), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, pFont)

            If prn_DmgAmt_STS = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(vprn_Tot_Damage_Amt), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 5, CurY, 1, 0, pFont)
            End If
            Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(vprn_Tot_Amount), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)


            'Total_Dgv_Weight

            'CurY = CurY + TxtHgt - 10

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

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6)
            W1 = e.Graphics.MeasureString("DISCOUNT    : ", pFont).Width
            CurY = CurY + TxtHgt - 10

            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "Sub Total", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Cooly").ToString), PageWidth - 10, CurY, 1, 0, pFont)

            End If
            If Gst_Status = 1 Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "Taxable", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Taxable_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)

                End If
                CurY = CurY + TxtHgt

                If Val(prn_HdDt.Rows(prn_HeadIndx).Item("CGST_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ " & Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("CGST_Percentage").ToString), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    If is_LastPage = True Then
                        Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(0).Item("CGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                    End If

                Else
                    Common_Procedures.Print_To_PrintDocument(e, "Add : CGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)

                End If

                CurY = CurY + TxtHgt

                If Val(prn_HdDt.Rows(prn_HeadIndx).Item("SGST_Amount").ToString) <> 0 Then
                    Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ " & Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("SGST_Percentage").ToString), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    If is_LastPage = True Then
                        Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("SGST_Amount").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                    End If

                Else
                    Common_Procedures.Print_To_PrintDocument(e, "Add : SGST  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)

                End If

            End If
            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Freight", LMargin + C1 + 10, CurY, 0, 0, pFont)
            ' Common_Procedures.Print_To_PrintDocument(e, "( + )", LMargin + C1 + W1 + 10, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Freight_Charge").ToString), PageWidth - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Add", LMargin + C1 + 10, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Add_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
            End If

            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Less", LMargin + C1 + 10, CurY, 0, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "" & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Less_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
            End If


            If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Assesable_Value").ToString) <> 0 Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Bill Amount ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Assesable_Value").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If

            End If
            CurY = CurY + TxtHgt

            If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Tds_perc_Calc").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Less : Tds  @ " & Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Tds_perc").ToString), "##########0.0") & " %", LMargin + C1 + 10, CurY, 0, 0, pFont)
                If is_LastPage = True Then
                    Common_Procedures.Print_To_PrintDocument(e, "" & Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Tds_perc_Calc").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                End If

            Else
                Common_Procedures.Print_To_PrintDocument(e, "Less : Tds  @ ", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, "0.00", PageWidth - 10, CurY, 1, 0, pFont)

            End If


            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
            LnAr(8) = CurY
            CurY = CurY + TxtHgt - 10
            'Common_Procedures.Print_To_PrintDocument(e, "E & O.E", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL COOLY", LMargin + C1 + 10, CurY, 0, 0, p1Font)

            Common_Procedures.Print_To_PrintDocument(e, " " & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Net_Amount").ToString), PageWidth - 10, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, PageWidth, CurY)
            LnAr(8) = CurY


            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(9) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
            CurY = CurY + TxtHgt - 10
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Net_Amount").ToString))
            BmsInWrds = Replace(Trim(LCase(BmsInWrds)), "", "")

            Common_Procedures.Print_To_PrintDocument(e, "Rupees            : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt + 5
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

            CurY = CurY + TxtHgt - 10
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt


            CurY = CurY + TxtHgt



            CurY = CurY + TxtHgt + 10
            Common_Procedures.Print_To_PrintDocument(e, "Weaver's Signature", LMargin + 20, CurY, 0, 0, pFont)
            'Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 250, CurY, 0, 0, pFont)



            Common_Procedures.Print_To_PrintDocument(e, "AUTHORISED SIGNATORY ", PageWidth - 5, CurY, 1, 0, pFont)
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

End Class