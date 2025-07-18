Public Class Weaver_PieceChecking_PieceWise
    Implements Interface_MDIActions

    Private Con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private PrevAct_Ctrl As New Control
    Private FrmLdSts As Boolean = False
    Private Vcbo_KeyDownVal As Double
    Private Filter_Status As Boolean = False
    Private vRECEIPTTYPE As String = "W"
    Private Pk_Condition As String = ""
    Private PkCondition_CLORCPT As String = ""
    Private PkCondition1_VPCHK As String = "VPCHK-"
    Private PkCondition1_IPCHK As String = "IPCHK-"
    Private PkCondition2_WCLRC As String = "WCLRC-"
    Private PkCondition2_PDOFF As String = "PDOFF-"
    Private vEntryType As String = ""
    Private vmskOldText As String = ""
    Private vmskSelStrt As Integer = -1
    Private vcbo_KeyDwnVal As Double
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private dgv_ActCtrlName As String = ""
    Private dgvDet_CboBx_ColNos_Arr As Integer() = {0}

    'PIECE DETAILS GRID ======================================
    Private Enum dgvCol_Details As Integer
        SNO     '0
        PCSNO   '1
        RECEIPT_DOFF_MTRS   '2
        PCSTYPE1    '3
        PCSTYPE2    '4
        PCSTYPE3    '5
        PCSTYPE4    '6
        PCSTYPE5    '7
        TOTALCHECKINGMTRS   '8
        EXCESS_SHORT_MTRS   '9
        TOTALRECEIPT_EXCESSSHORT_MTRS   '10
        WEIGHT  '11
        WEIGHTPERMETER  '12
        PACKINGSLIPCODETYPE1    '13
        PACKINGSLIPCODETYPE2    '14
        PACKINGSLIPCODETYPE3    '15
        PACKINGSLIPCODETYPE4    '16
        PACKINGSLIPCODETYPE5    '17
        REED                    '18
        PICK                    '19
        WIDTH                   '20
    End Enum

    Private Enum dgvCol_Filter As Integer
        LOTNO     '0
        LOTDATE   '1
        WEAVERNAME   '2
        CLOTHNAME    '3
        ENDSCOUNT    '4
        WEFTCOUNT    '5
        TOTALMETERS    '6
        CONSUMEDYARN    '7
    End Enum

    Public Sub New(ByVal EntryType As String)
        vEntryType = Trim(UCase(EntryType))
        FrmLdSts = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub clear()

        New_Entry = False

        pnl_Back.Enabled = True

        lbl_CheckingRefNo.Text = ""
        lbl_CheckingRefNo.ForeColor = Color.Black
        lbl_WarpConsumption.Text = ""
        txt_PieceNo.Text = ""
        txt_PieceNo.Tag = txt_PieceNo.Text
        lbl_WeftConsumption.Text = ""

        msk_date.Text = ""
        dtp_Date.Text = ""
        lbl_ClothName.Text = ""
        lbl_EndsCount.Text = ""
        Cbo_LoomType.Text = "AUTOLOOM"
        Cbo_LoomType.Enabled = True
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1360" Then '---- Ashoka Textile (63.Velampalayam - Palladam)
            Cbo_LoomType.Text = "POWERLOOM"
            Cbo_LoomType.Enabled = False
        End If
        lbl_WeaverName.Text = ""
        txt_PieceNo.Text = ""
        txt_PieceNo.Tag = txt_PieceNo.Text
        txt_Folding.Text = "100"
        lbl_WeftCount.Text = ""

        lbl_LoomNo.Text = ""
        cbo_WidthType.Text = ""
        cbo_StockOff.Text = Common_Procedures.Ledger_IdNoToName(Con, Val(Common_Procedures.CommonLedger.OwnSort_Ac))
        lbl_Godown_StockIN.Text = Common_Procedures.Ledger_IdNoToName(Con, Val(Common_Procedures.CommonLedger.Godown_Ac))

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        dgv_Details_Total2.Rows.Clear()
        dgv_Details_Total2.Rows.Add()
        dgv_Details_Total2.Rows(0).Cells(dgvCol_Details.SNO).Value = "100%"
        dgv_Details_Total2.Rows(0).Cells(dgvCol_Details.PCSNO).Value = "FOLDING"


        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_Cloth.Text = ""
            cbo_Filter_PartyName.Text = ""
            txt_Filter_RecNo.Text = ""
            txt_Filter_RecNoTo.Text = ""
            cbo_Filter_Cloth.SelectedIndex = -1
            cbo_Filter_PartyName.SelectedIndex = -1

            dgv_Filter_Details.Rows.Clear()
        End If



        lbl_LoomNo.Enabled = True
        lbl_LoomNo.BackColor = Color.White

        lbl_WeaverName.Enabled = True
        lbl_WeaverName.BackColor = Color.White

        lbl_ClothName.Enabled = True
        lbl_ClothName.BackColor = Color.White

        Cbo_LoomType.Enabled = True
        Cbo_LoomType.BackColor = Color.White

        lbl_LoomNo.Enabled = True
        lbl_LoomNo.BackColor = Color.White

        txt_Folding.Enabled = True
        txt_Folding.BackColor = Color.White

        cbo_WidthType.Enabled = True
        cbo_WidthType.BackColor = Color.White

        lbl_EndsCount.Enabled = True
        'lbl_EndsCount.BackColor = Color.White

        lbl_Godown_StockIN.Enabled = True
        lbl_Godown_StockIN.BackColor = Color.White

        cbo_StockOff.Enabled = True
        cbo_StockOff.BackColor = Color.White

        Grid_Cell_DeSelect()

    End Sub

    Private Sub Control_GotFocus(ByVal sender As Object, ByVal e As EventArgs)
        Dim txtbx As TextBox
        Dim mskbx As MaskedTextBox
        Dim cbx As ComboBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is MaskedTextBox Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            cbx = Me.ActiveControl
            cbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is MaskedTextBox Then
            mskbx = Me.ActiveControl
            mskbx.SelectAll()
        End If

        If Me.ActiveControl.Name <> dgv_Details.Name Then
            Grid_DeSelect()
        End If

        If Me.ActiveControl.Name <> dgv_Details_Total.Name Then
            Grid_DeSelect()
        End If

        PrevAct_Ctrl = Me.ActiveControl

    End Sub

    Private Sub Control_LostFocus(ByVal sender As Object, ByVal e As EventArgs)
        On Error Resume Next
        If IsDBNull(PrevAct_Ctrl) = False Then
            If TypeOf PrevAct_Ctrl Is TextBox Or TypeOf PrevAct_Ctrl Is ComboBox Or TypeOf PrevAct_Ctrl Is MaskedTextBox Then
                Me.PrevAct_Ctrl.BackColor = Color.White
                Me.PrevAct_Ctrl.ForeColor = Color.Black
            End If
        End If
    End Sub

    Private Sub TextBoxControl_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyValue = 38 Then e.Handled = True : SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub TextBoxControl_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) = 13 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Filter_Status = True Then Exit Sub

        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False

        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False

        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False

        If Not IsNothing(dgv_Details_Total2.CurrentCell) Then dgv_Details_Total2.CurrentCell.Selected = False
    End Sub

    Private Sub Grid_Cell_DeSelect()
        On Error Resume Next

        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False

        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False

        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False

        If Not IsNothing(dgv_Details_Total2.CurrentCell) Then dgv_Details_Total2.CurrentCell.Selected = False

        dgv_ActCtrlName = ""
    End Sub


    Private Sub Weaver_PieceChecking_PieceWise_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(lbl_WeaverName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(UCase(Common_Procedures.Master_Return.Return_Value)) <> "" Then
                lbl_WeaverName.Text = Trim(UCase(Common_Procedures.Master_Return.Return_Value))
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(lbl_ClothName.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(UCase(Common_Procedures.Master_Return.Return_Value)) <> "" Then
                lbl_ClothName.Text = Trim(UCase(Common_Procedures.Master_Return.Return_Value))
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(lbl_EndsCount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ENDSCOUNT" And Trim(UCase(Common_Procedures.Master_Return.Return_Value)) <> "" Then
                lbl_EndsCount.Text = Trim(UCase(Common_Procedures.Master_Return.Return_Value))
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

                new_record()
            End If

        Catch ex As Exception

        End Try

        FrmLdSts = False
    End Sub

    Private Sub Weaver_PieceChecking_PieceWise_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        Con.Dispose()
        Con.Close()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Weaver_PieceChecking_PieceWise_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If pnl_Filter.Visible = True Then
                btn_Filter_Close_Click(sender, e)
            ElseIf MessageBox.Show("Do you want to Close ?", "FOR CLOSE", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                Me.Close()
            End If
        End If
    End Sub

    Private Sub Weaver_PieceChecking_PieceWise_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim vTotWdth As String = 0

        Con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        Con.Open()


        Pk_Condition = ""
        PkCondition_CLORCPT = ""

        If Trim(UCase(vEntryType)) = "VENDOR" Then
            Label1.Text = "VENDOR PIECE CHECKING"
            Pk_Condition = PkCondition1_VPCHK
            PkCondition_CLORCPT = PkCondition2_WCLRC

            Me.BackColor = Color.LightGray

        Else

            Label1.Text = "INHOUSE PIECE CHECKING"
            Pk_Condition = PkCondition1_IPCHK
            PkCondition_CLORCPT = PkCondition2_PDOFF

        End If


        Cbo_LoomType.Items.Clear()
        Cbo_LoomType.Items.Add("")
        Cbo_LoomType.Items.Add("AUTOLOOM")
        Cbo_LoomType.Items.Add("POWERLOOM")

        cbo_WidthType.Items.Clear()
        cbo_WidthType.Items.Add("")
        cbo_WidthType.Items.Add("SINGLE")
        cbo_WidthType.Items.Add("DOUBLE")
        cbo_WidthType.Items.Add("TRIPLE")
        cbo_WidthType.Items.Add("FOURTH")

        cbo_WidthType.Items.Add("SINGLE FABRIC FROM 1 BEAM")
        cbo_WidthType.Items.Add("SINGLE FABRIC FROM 2 BEAMS")

        cbo_WidthType.Items.Add("DOUBLE FABRIC FROM 1 BEAM")
        cbo_WidthType.Items.Add("DOUBLE FABRIC FROM 2 BEAMS")

        cbo_WidthType.Items.Add("TRIPLE FABRIC FROM 1 BEAM")
        cbo_WidthType.Items.Add("TRIPLE FABRIC FROM 2 BEAMS")

        cbo_WidthType.Items.Add("FOUR FABRIC FROM 1 BEAM")
        cbo_WidthType.Items.Add("FOUR FABRIC FROM 2 BEAMS")


        lbl_StockOff_Caption.Visible = False
        cbo_StockOff.Visible = False
        If Common_Procedures.settings.JOBWORKENTRY_Status = 1 Then
            lbl_StockOff_Caption.Visible = True
            cbo_StockOff.Visible = True
        End If

        lbl_Godown_StockIN.Visible = False
        lbl_Godown_StockIN_Caption.Visible = False
        If Common_Procedures.settings.Multi_Godown_Status = 1 Then
            lbl_Godown_StockIN.Visible = True
            lbl_Godown_StockIN_Caption.Visible = True
        End If

        dgv_Details.Columns(dgvCol_Details.PCSTYPE1).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type1))
        dgv_Details.Columns(dgvCol_Details.PCSTYPE2).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type2))
        dgv_Details.Columns(dgvCol_Details.PCSTYPE3).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type3))
        dgv_Details.Columns(dgvCol_Details.PCSTYPE4).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type4))
        dgv_Details.Columns(dgvCol_Details.PCSTYPE5).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type5))


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "--1360---" Then '---- Ashoka Textile (63.Velampalayam - Palladam)

            lbl_LoomNo_Caption.Visible = False
            lbl_LoomNo.Visible = False
            lbl_WidthType_Caption.Visible = False
            cbo_WidthType.Visible = False

            dgv_Details.Columns(dgvCol_Details.EXCESS_SHORT_MTRS).ReadOnly = False

            dgv_Details.Columns(dgvCol_Details.PCSTYPE1).Visible = False
            dgv_Details.Columns(dgvCol_Details.PCSTYPE2).Visible = False
            dgv_Details.Columns(dgvCol_Details.PCSTYPE3).Visible = False
            dgv_Details.Columns(dgvCol_Details.PCSTYPE4).Visible = False
            dgv_Details.Columns(dgvCol_Details.PCSTYPE5).Visible = False
            dgv_Details.Columns(dgvCol_Details.TOTALCHECKINGMTRS).Visible = False
            dgv_Details.Columns(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Visible = True
            dgv_Details.Columns(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).HeaderText = "TOTAL METERS"

            dgv_Details_Total.Columns(dgvCol_Details.PCSTYPE1).Visible = False
            dgv_Details_Total.Columns(dgvCol_Details.PCSTYPE2).Visible = False
            dgv_Details_Total.Columns(dgvCol_Details.PCSTYPE3).Visible = False
            dgv_Details_Total.Columns(dgvCol_Details.PCSTYPE4).Visible = False
            dgv_Details_Total.Columns(dgvCol_Details.PCSTYPE5).Visible = False
            dgv_Details_Total.Columns(dgvCol_Details.TOTALCHECKINGMTRS).Visible = False
            dgv_Details_Total.Columns(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Visible = True

            dgv_Details_Total2.Columns(dgvCol_Details.PCSTYPE1).Visible = False
            dgv_Details_Total2.Columns(dgvCol_Details.PCSTYPE2).Visible = False
            dgv_Details_Total2.Columns(dgvCol_Details.PCSTYPE3).Visible = False
            dgv_Details_Total2.Columns(dgvCol_Details.PCSTYPE4).Visible = False
            dgv_Details_Total2.Columns(dgvCol_Details.PCSTYPE5).Visible = False
            dgv_Details_Total2.Columns(dgvCol_Details.TOTALCHECKINGMTRS).Visible = False
            dgv_Details_Total2.Columns(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Visible = True

            vTotWdth = dgv_Details.Columns(dgvCol_Details.PCSTYPE1).Width + dgv_Details.Columns(dgvCol_Details.PCSTYPE2).Width + dgv_Details.Columns(dgvCol_Details.PCSTYPE3).Width + dgv_Details.Columns(dgvCol_Details.PCSTYPE4).Width + dgv_Details.Columns(dgvCol_Details.PCSTYPE5).Width + dgv_Details.Columns(dgvCol_Details.TOTALCHECKINGMTRS).Width

            vTotWdth = (Val(vTotWdth) - 88 - 20) / 6

            dgv_Details.Columns(dgvCol_Details.SNO).Width = dgv_Details.Columns(dgvCol_Details.SNO).Width + 20
            dgv_Details.Columns(dgvCol_Details.PCSNO).Width = dgv_Details.Columns(dgvCol_Details.PCSNO).Width + Val(vTotWdth)
            dgv_Details.Columns(dgvCol_Details.RECEIPT_DOFF_MTRS).Width = dgv_Details.Columns(dgvCol_Details.RECEIPT_DOFF_MTRS).Width + Val(vTotWdth)
            dgv_Details.Columns(dgvCol_Details.EXCESS_SHORT_MTRS).Width = dgv_Details.Columns(dgvCol_Details.EXCESS_SHORT_MTRS).Width + Val(vTotWdth)
            dgv_Details.Columns(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Width = dgv_Details.Columns(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Width + Val(vTotWdth)
            dgv_Details.Columns(dgvCol_Details.WEIGHT).Width = dgv_Details.Columns(dgvCol_Details.WEIGHT).Width + Val(vTotWdth)
            dgv_Details.Columns(dgvCol_Details.WEIGHTPERMETER).Width = dgv_Details.Columns(dgvCol_Details.WEIGHTPERMETER).Width + Val(vTotWdth)

            dgv_Details_Total.Columns(dgvCol_Details.SNO).Width = dgv_Details_Total.Columns(dgvCol_Details.SNO).Width + 20
            dgv_Details_Total.Columns(dgvCol_Details.PCSNO).Width = dgv_Details_Total.Columns(dgvCol_Details.PCSNO).Width + Val(vTotWdth)
            dgv_Details_Total.Columns(dgvCol_Details.RECEIPT_DOFF_MTRS).Width = dgv_Details_Total.Columns(dgvCol_Details.RECEIPT_DOFF_MTRS).Width + Val(vTotWdth)
            dgv_Details_Total.Columns(dgvCol_Details.EXCESS_SHORT_MTRS).Width = dgv_Details_Total.Columns(dgvCol_Details.EXCESS_SHORT_MTRS).Width + Val(vTotWdth)
            dgv_Details_Total.Columns(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Width = dgv_Details_Total.Columns(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Width + Val(vTotWdth)
            dgv_Details_Total.Columns(dgvCol_Details.WEIGHT).Width = dgv_Details_Total.Columns(dgvCol_Details.WEIGHT).Width + Val(vTotWdth)
            dgv_Details_Total.Columns(dgvCol_Details.WEIGHTPERMETER).Width = dgv_Details_Total.Columns(dgvCol_Details.WEIGHTPERMETER).Width + Val(vTotWdth)

            dgv_Details_Total2.Columns(dgvCol_Details.SNO).Width = dgv_Details_Total2.Columns(dgvCol_Details.SNO).Width + 20
            dgv_Details_Total2.Columns(dgvCol_Details.PCSNO).Width = dgv_Details_Total2.Columns(dgvCol_Details.PCSNO).Width + Val(vTotWdth)
            dgv_Details_Total2.Columns(dgvCol_Details.RECEIPT_DOFF_MTRS).Width = dgv_Details_Total2.Columns(dgvCol_Details.RECEIPT_DOFF_MTRS).Width + Val(vTotWdth)
            dgv_Details_Total2.Columns(dgvCol_Details.EXCESS_SHORT_MTRS).Width = dgv_Details_Total2.Columns(dgvCol_Details.EXCESS_SHORT_MTRS).Width + Val(vTotWdth)
            dgv_Details_Total2.Columns(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Width = dgv_Details_Total2.Columns(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Width + Val(vTotWdth)
            dgv_Details_Total2.Columns(dgvCol_Details.WEIGHT).Width = dgv_Details_Total2.Columns(dgvCol_Details.WEIGHT).Width + Val(vTotWdth)
            dgv_Details_Total2.Columns(dgvCol_Details.WEIGHTPERMETER).Width = dgv_Details_Total2.Columns(dgvCol_Details.WEIGHTPERMETER).Width + Val(vTotWdth)

        End If

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        AddHandler lbl_ClothName.GotFocus, AddressOf Control_GotFocus
        AddHandler lbl_EndsCount.GotFocus, AddressOf Control_GotFocus
        AddHandler Cbo_LoomType.GotFocus, AddressOf Control_GotFocus
        AddHandler lbl_WeaverName.GotFocus, AddressOf Control_GotFocus
        AddHandler msk_date.GotFocus, AddressOf Control_GotFocus
        AddHandler lbl_WarpConsumption.GotFocus, AddressOf Control_GotFocus
        AddHandler lbl_WeftConsumption.GotFocus, AddressOf Control_GotFocus
        AddHandler txt_PieceNo.GotFocus, AddressOf Control_GotFocus
        AddHandler txt_Folding.GotFocus, AddressOf Control_GotFocus
        AddHandler lbl_WeftCount.GotFocus, AddressOf Control_GotFocus

        AddHandler cbo_Filter_Cloth.GotFocus, AddressOf Control_GotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf Control_GotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf Control_GotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf Control_GotFocus
        AddHandler txt_Filter_RecNo.GotFocus, AddressOf Control_GotFocus
        AddHandler txt_Filter_RecNoTo.GotFocus, AddressOf Control_GotFocus
        AddHandler lbl_LoomNo.GotFocus, AddressOf Control_GotFocus
        AddHandler cbo_WidthType.GotFocus, AddressOf Control_GotFocus
        AddHandler lbl_Godown_StockIN.GotFocus, AddressOf Control_GotFocus
        AddHandler cbo_StockOff.GotFocus, AddressOf Control_GotFocus

        AddHandler cbo_WidthType.LostFocus, AddressOf Control_LostFocus
        AddHandler lbl_LoomNo.LostFocus, AddressOf Control_LostFocus
        AddHandler lbl_Godown_StockIN.LostFocus, AddressOf Control_LostFocus
        AddHandler cbo_StockOff.LostFocus, AddressOf Control_LostFocus

        AddHandler lbl_ClothName.LostFocus, AddressOf Control_LostFocus
        AddHandler lbl_EndsCount.LostFocus, AddressOf Control_LostFocus
        AddHandler Cbo_LoomType.LostFocus, AddressOf Control_LostFocus
        AddHandler lbl_WeaverName.LostFocus, AddressOf Control_LostFocus
        AddHandler msk_date.LostFocus, AddressOf Control_LostFocus
        AddHandler lbl_WarpConsumption.LostFocus, AddressOf Control_LostFocus
        AddHandler lbl_WeftConsumption.LostFocus, AddressOf Control_LostFocus
        AddHandler txt_PieceNo.LostFocus, AddressOf Control_LostFocus
        AddHandler txt_Folding.LostFocus, AddressOf Control_LostFocus
        AddHandler lbl_WeftCount.LostFocus, AddressOf Control_LostFocus

        AddHandler cbo_Filter_Cloth.LostFocus, AddressOf Control_LostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf Control_LostFocus
        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf Control_LostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf Control_LostFocus
        AddHandler txt_Filter_RecNoTo.LostFocus, AddressOf Control_LostFocus
        AddHandler txt_Filter_RecNo.LostFocus, AddressOf Control_LostFocus

        'AddHandler txt_ConsPavu.KeyDown, AddressOf TextBoxControl_KeyDown
        ' AddHandler txt_ConsYarn.KeyDown, AddressOf TextBoxControl_KeyDown
        'AddHandler txt_LotNoCaption.KeyDown, AddressOf TextBoxControl_KeyDown
        'AddHandler txt_TotalMeters.KeyDown, AddressOf TextBoxControl_KeyDown
        'AddHandler msk_date.KeyDown, AddressOf TextBoxControl_KeyDown
        'AddHandler txt_Folding.KeyDown, AddressOf TextBoxControl_KeyDown


        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControl_KeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControl_KeyDown
        AddHandler txt_Filter_RecNo.KeyDown, AddressOf TextBoxControl_KeyDown
        AddHandler txt_Filter_RecNoTo.KeyDown, AddressOf TextBoxControl_KeyDown
        'AddHandler txt_PieceNo.KeyDown, AddressOf TextBoxControl_KeyDown
        ' AddHandler txt_ConsYarn.KeyPress, AddressOf TextBoxControl_KeyPress
        'AddHandler txt_LotNoCaption.KeyPress, AddressOf TextBoxControl_KeyPress
        'AddHandler msk_date.KeyPress, AddressOf TextBoxControl_KeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControl_KeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControl_KeyPress
        'AddHandler txt_Folding.KeyPress, AddressOf TextBoxControl_KeyPress

        'AddHandler txt_PieceNo.KeyPress, AddressOf TextBoxControl_KeyPress
        AddHandler txt_Filter_RecNo.KeyPress, AddressOf TextBoxControl_KeyPress
        AddHandler txt_Filter_RecNoTo.KeyPress, AddressOf TextBoxControl_KeyPress

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView
        Dim PrevCtrl As New Object
        Dim vCURRCELL As Integer = -1

        If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_Details.Name Then
                dgv1 = dgv_Details

            ElseIf dgv_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Details

            Else
                dgv1 = dgv_Details

            End If

            If IsNothing(dgv1) = False And IsNothing(dgv_Details.CurrentCell) = False Then

                With dgv1

                    If keyData = Keys.Enter Or keyData = Keys.Down Or keyData = Keys.Up Then


                        If dgv1.Name = dgv_Details.Name Then

                            vCURRCELL = .CurrentCell.ColumnIndex

                            If keyData = Keys.Enter Or keyData = Keys.Down Then


LOOP1:
                                If vCURRCELL >= dgvCol_Details.WIDTH Then

                                    If .CurrentCell.RowIndex = .RowCount - 1 Then

                                        If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                                            save_record()
                                        Else
                                            dtp_Date.Focus()
                                        End If

                                    Else
                                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                                    End If

                                Else

                                    If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 0 And ((Trim(.CurrentRow.Cells(dgvCol_Details.PCSNO).Value) = "" Or Trim(.CurrentRow.Cells(dgvCol_Details.PCSNO).Value) = "0") And Val(.CurrentRow.Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value) = 0) Then
                                        If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                                            save_record()
                                        Else
                                            msk_date.Focus()
                                        End If

                                    ElseIf .CurrentCell.RowIndex = .RowCount - 2 And .CurrentCell.ColumnIndex >= 0 And ((Trim(.CurrentRow.Cells(dgvCol_Details.PCSNO).Value) = "" Or Trim(.CurrentRow.Cells(dgvCol_Details.PCSNO).Value) = "0") And Val(.CurrentRow.Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value) = 0 And (Trim(.Rows(.RowCount - 1).Cells(dgvCol_Details.PCSNO).Value) = "" Or Trim(.Rows(.RowCount - 1).Cells(dgvCol_Details.PCSNO).Value) = "0")) And Val(.Rows(.RowCount - 1).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value) = 0 Then
                                        If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                                            save_record()
                                        Else
                                            msk_date.Focus()
                                        End If

                                    Else
                                        If .Columns(vCURRCELL + 1).Visible = True And .Columns(vCURRCELL + 1).ReadOnly = False Then
                                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(vCURRCELL + 1)
                                        Else
                                            vCURRCELL = vCURRCELL + 1
                                            GoTo LOOP1
                                        End If


                                    End If

                                End If

                                Return True

                            ElseIf keyData = Keys.Up Then

LOOP2:

                                If vCURRCELL <= 1 Then

                                    If .CurrentCell.RowIndex <= 0 Then

                                        If txt_PieceNo.Enabled And txt_PieceNo.Visible Then
                                            txt_PieceNo.Focus()
                                        ElseIf msk_date.Enabled And msk_date.Visible Then
                                            msk_date.Focus()
                                        ElseIf txt_Folding.Enabled And txt_Folding.Visible Then
                                            txt_Folding.Focus()
                                        Else
                                            dtp_Date.Focus()
                                        End If

                                    Else
                                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(dgvCol_Details.WIDTH)

                                    End If

                                Else

                                    If .Columns(vCURRCELL - 1).Visible = True And .Columns(vCURRCELL - 1).ReadOnly = False Then
                                        .CurrentCell = .Rows(.CurrentRow.Index).Cells(vCURRCELL - 1)
                                    Else
                                        vCURRCELL = vCURRCELL - 1
                                        GoTo LOOP2
                                    End If

                                End If

                                Return True

                            Else
                                Return MyBase.ProcessCmdKey(msg, keyData)

                            End If

                        Else

                            Return MyBase.ProcessCmdKey(msg, keyData)

                        End If

                        'PrevCtrl = Nothing
                        ''If txt_Folding.Visible = True And txt_Folding.Enabled = True Then
                        ''    PrevCtrl = txt_Folding
                        'If txt_PieceNo.Visible = True And txt_PieceNo.Enabled = True Then
                        '    PrevCtrl = txt_PieceNo
                        'Else
                        '    PrevCtrl = msk_date
                        'End If
                        'Common_Procedures.DGVGrid_Cursor_Focus(dgv1, keyData, PrevCtrl, Nothing, dgvDet_CboBx_ColNos_Arr, dgtxt_Details, msk_date)
                        'Return True

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

    Private Sub move_record(ByVal idno As String)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim NewCode As String = ""
        Dim n As Integer = 0
        Dim SNo As Integer = 0
        Dim LockSTS As Boolean = False
        Dim WGSLockSTS As Boolean = False

        clear()

        New_Entry = False

        'Try

        If Val(idno) = 0 Then Exit Sub

        NewCode = Trim(Pk_Condition) & Trim(lbl_Company.Tag) & "-" & Trim(idno) & "/" & Trim(Common_Procedures.FnYearCode)

        da = New SqlClient.SqlDataAdapter("select a.*, e.Ledger_Name as StockOff_Name from Weaver_Piece_Checking_Head a LEFT OUTER JOIN Ledger_Head e ON a.StockOff_IdNo = e.Ledger_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'", Con)
        'da = New SqlClient.SqlDataAdapter("select a.*, e.Ledger_Name as StockOff_Name from Weaver_Piece_Checking_Head a LEFT OUTER JOIN Ledger_Head e ON a.StockOff_IdNo = e.Ledger_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'", Con)
        dt = New DataTable
        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            lbl_CheckingRefNo.Text = dt.Rows(0).Item("Weaver_Piece_Checking_No").ToString
            dtp_Date.Text = dt.Rows(0).Item("Weaver_Piece_Checking_Date")
            msk_date.Text = dtp_Date.Text

            Cbo_LoomType.Text = dt.Rows(0).Item("Loom_Type").ToString

            lbl_WeaverName.Text = Common_Procedures.Ledger_IdNoToName(Con, Val(dt.Rows(0).Item("Ledger_IdNo").ToString))
            lbl_ClothName.Text = Common_Procedures.Cloth_IdNoToName(Con, Val(dt.Rows(0).Item("Cloth_IdNo").ToString))

            txt_PieceNo.Text = dt.Rows(0).Item("Piece_No").ToString
            txt_PieceNo.Tag = txt_PieceNo.Text

            lbl_ReceiptCode.Text = dt.Rows(0).Item("Piece_Receipt_Code").ToString
            lbl_ReceiptNo.Text = dt.Rows(0).Item("Piece_Receipt_No").ToString
            lbl_ReceiptDate.Text = dt.Rows(0).Item("Piece_Receipt_Date").ToString
            lbl_PartyLotNo.Text = dt.Rows(0).Item("Party_DcNo").ToString
            lbl_ReceiptMeters.Text = dt.Rows(0).Item("ReceiptMeters_Receipt").ToString


            txt_Folding.Text = dt.Rows(0).Item("Folding").ToString

            cbo_StockOff.Text = dt.Rows(0).Item("StockOff_Name").ToString
            lbl_LoomNo.Text = dt.Rows(0).Item("Loom_No").ToString

            LockSTS = False
            WGSLockSTS = False

            lbl_Godown_StockIN.Text = ""
            cbo_WidthType.Text = ""
            lbl_EndsCount.Text = ""
            lbl_WeftCount.Text = ""

            da1 = New SqlClient.SqlDataAdapter("Select a.* from Weaver_Cloth_Receipt_Head a Where (a.Weaver_ClothReceipt_Code = '" & Trim(lbl_ReceiptCode.Text) & "' or '" & Trim(PkCondition_CLORCPT) & "' + a.Weaver_ClothReceipt_Code = '" & Trim(lbl_ReceiptCode.Text) & "')", Con)
            dt2 = New DataTable
            da1.Fill(dt2)
            If dt2.Rows.Count > 0 Then
                'lbl_Godown_StockIN.Text = Common_Procedures.Ledger_IdNoToName(Con, Val(dt2.Rows(0).Item("WareHouse_IdNo").ToString))
                'cbo_WidthType.Text = dt2.Rows(0).Item("Width_Type").ToString
                'lbl_EndsCount.Text = Common_Procedures.EndsCount_IdNoToName(Con, Val(dt2.Rows(0).Item("EndsCount_IdNo").ToString))
                'lbl_WeftCount.Text = Common_Procedures.Count_IdNoToName(Con, Val(dt2.Rows(0).Item("Count_IdNo").ToString))

                If Trim(dt2.Rows(0).Item("Weaver_Wages_Code").ToString) <> "" Or Trim(dt2.Rows(0).Item("Weaver_IR_Wages_Code").ToString) <> "" Then
                    LockSTS = True
                    WGSLockSTS = True
                End If

            End If
            dt2.Clear()

            da2 = New SqlClient.SqlDataAdapter("SELECT a.* FROM Weaver_ClothReceipt_Piece_Details a WHERE a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' ORDER BY a.PieceNo_OrderBy, a.Piece_No, a.Sl_No", Con)
            dt2 = New DataTable
            da2.Fill(dt2)

            With dgv_Details


                .Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    lbl_Godown_StockIN.Text = Common_Procedures.Ledger_IdNoToName(Con, Val(dt2.Rows(0).Item("WareHouse_IdNo").ToString))
                    cbo_WidthType.Text = dt2.Rows(0).Item("Width_Type").ToString
                    lbl_EndsCount.Text = Common_Procedures.EndsCount_IdNoToName(Con, Val(dt2.Rows(0).Item("EndsCount_IdNo").ToString))
                    lbl_WeftCount.Text = Common_Procedures.Count_IdNoToName(Con, Val(dt2.Rows(0).Item("Count_IdNo").ToString))

                    For i = 0 To dt2.Rows.Count - 1

                        SNo = SNo + 1

                        n = .Rows.Add

                        .Rows(n).Cells(dgvCol_Details.SNO).Value = Val(SNo)
                        .Rows(n).Cells(dgvCol_Details.PCSNO).Value = dt2.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value = Format(Val(dt2.Rows(i).Item("Receipt_Meters").ToString), "########0.00")

                        If Val(dt2.Rows(i).Item("Type1_Meters").ToString) <> 0 Then
                            .Rows(n).Cells(dgvCol_Details.PCSTYPE1).Value = Format(Val(dt2.Rows(i).Item("Type1_Meters").ToString), "########0.00")
                        End If
                        If Val(dt2.Rows(i).Item("Type2_Meters").ToString) <> 0 Then
                            .Rows(n).Cells(dgvCol_Details.PCSTYPE2).Value = Format(Val(dt2.Rows(i).Item("Type2_Meters").ToString), "########0.00")
                        End If
                        If Val(dt2.Rows(i).Item("Type3_Meters").ToString) <> 0 Then
                            .Rows(n).Cells(dgvCol_Details.PCSTYPE3).Value = Format(Val(dt2.Rows(i).Item("Type3_Meters").ToString), "########0.00")
                        End If
                        If Val(dt2.Rows(i).Item("Type4_Meters").ToString) <> 0 Then
                            .Rows(n).Cells(dgvCol_Details.PCSTYPE4).Value = Format(Val(dt2.Rows(i).Item("Type4_Meters").ToString), "########0.00")
                        End If
                        If Val(dt2.Rows(i).Item("Type5_Meters").ToString) <> 0 Then
                            .Rows(n).Cells(dgvCol_Details.PCSTYPE5).Value = Format(Val(dt2.Rows(i).Item("Type5_Meters").ToString), "########0.00")
                        End If
                        .Rows(n).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value = Format(Val(dt2.Rows(i).Item("Total_Checking_Meters").ToString), "########0.00")

                        .Rows(n).Cells(dgvCol_Details.EXCESS_SHORT_MTRS).Value = Format(Val(dt2.Rows(i).Item("Excess_Short_Meter").ToString), "########0.00")

                        .Rows(n).Cells(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Value = Format(Val(dt2.Rows(i).Item("Total_Checking_Meters").ToString), "########0.00")

                        If Val(dt2.Rows(i).Item("Weight").ToString) <> 0 Then
                            .Rows(n).Cells(dgvCol_Details.WEIGHT).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")
                        End If
                        If Val(dt2.Rows(i).Item("Weight_Meter").ToString) <> 0 Then
                            .Rows(n).Cells(dgvCol_Details.WEIGHTPERMETER).Value = Format(Val(dt2.Rows(i).Item("Weight_Meter").ToString), "########0.000")
                        End If

                        .Rows(n).Cells(dgvCol_Details.PACKINGSLIPCODETYPE1).Value = dt2.Rows(i).Item("PackingSlip_Code_Type1").ToString
                        If Trim(.Rows(n).Cells(dgvCol_Details.PACKINGSLIPCODETYPE1).Value) = "" Then .Rows(n).Cells(dgvCol_Details.PACKINGSLIPCODETYPE1).Value = dt2.Rows(i).Item("Bale_UnPacking_Code_Type1").ToString
                        If Trim(.Rows(n).Cells(dgvCol_Details.PACKINGSLIPCODETYPE1).Value) = "" Then .Rows(n).Cells(dgvCol_Details.PACKINGSLIPCODETYPE1).Value = dt2.Rows(i).Item("BuyerOffer_Code_Type1").ToString

                        .Rows(n).Cells(dgvCol_Details.PACKINGSLIPCODETYPE2).Value = dt2.Rows(i).Item("PackingSlip_Code_Type2").ToString
                        If Trim(.Rows(n).Cells(dgvCol_Details.PACKINGSLIPCODETYPE2).Value) = "" Then .Rows(n).Cells(dgvCol_Details.PACKINGSLIPCODETYPE2).Value = dt2.Rows(i).Item("Bale_UnPacking_Code_Type2").ToString
                        If Trim(.Rows(n).Cells(dgvCol_Details.PACKINGSLIPCODETYPE2).Value) = "" Then .Rows(n).Cells(dgvCol_Details.PACKINGSLIPCODETYPE2).Value = dt2.Rows(i).Item("BuyerOffer_Code_Type2").ToString

                        .Rows(n).Cells(dgvCol_Details.PACKINGSLIPCODETYPE3).Value = dt2.Rows(i).Item("PackingSlip_Code_Type3").ToString
                        If Trim(.Rows(n).Cells(dgvCol_Details.PACKINGSLIPCODETYPE3).Value) = "" Then .Rows(n).Cells(dgvCol_Details.PACKINGSLIPCODETYPE3).Value = dt2.Rows(i).Item("Bale_UnPacking_Code_Type3").ToString
                        If Trim(.Rows(n).Cells(dgvCol_Details.PACKINGSLIPCODETYPE3).Value) = "" Then .Rows(n).Cells(dgvCol_Details.PACKINGSLIPCODETYPE3).Value = dt2.Rows(i).Item("BuyerOffer_Code_Type3").ToString

                        .Rows(n).Cells(dgvCol_Details.PACKINGSLIPCODETYPE4).Value = dt2.Rows(i).Item("PackingSlip_Code_Type4").ToString
                        If Trim(.Rows(n).Cells(dgvCol_Details.PACKINGSLIPCODETYPE4).Value) = "" Then .Rows(n).Cells(dgvCol_Details.PACKINGSLIPCODETYPE4).Value = dt2.Rows(i).Item("Bale_UnPacking_Code_Type4").ToString
                        If Trim(.Rows(n).Cells(dgvCol_Details.PACKINGSLIPCODETYPE4).Value) = "" Then .Rows(n).Cells(dgvCol_Details.PACKINGSLIPCODETYPE4).Value = dt2.Rows(i).Item("BuyerOffer_Code_Type4").ToString

                        .Rows(n).Cells(dgvCol_Details.PACKINGSLIPCODETYPE5).Value = dt2.Rows(i).Item("PackingSlip_Code_Type5").ToString
                        If Trim(.Rows(n).Cells(dgvCol_Details.PACKINGSLIPCODETYPE5).Value) = "" Then .Rows(n).Cells(dgvCol_Details.PACKINGSLIPCODETYPE5).Value = dt2.Rows(i).Item("Bale_UnPacking_Code_Type5").ToString
                        If Trim(.Rows(n).Cells(dgvCol_Details.PACKINGSLIPCODETYPE5).Value) = "" Then .Rows(n).Cells(dgvCol_Details.PACKINGSLIPCODETYPE5).Value = dt2.Rows(i).Item("BuyerOffer_Code_Type5").ToString

                        If Val(dt2.Rows(i).Item("REED").ToString) <> 0 Then
                            .Rows(n).Cells(dgvCol_Details.REED).Value = Val(dt2.Rows(i).Item("REED").ToString)
                        End If
                        If Val(dt2.Rows(i).Item("PICK").ToString) <> 0 Then
                            .Rows(n).Cells(dgvCol_Details.PICK).Value = Val(dt2.Rows(i).Item("PICK").ToString)
                        End If
                        If Val(dt2.Rows(i).Item("WIDTH").ToString) <> 0 Then
                            .Rows(n).Cells(dgvCol_Details.WIDTH).Value = Val(dt2.Rows(i).Item("WIDTH").ToString)
                        End If


                        If Trim(.Rows(n).Cells(dgvCol_Details.PACKINGSLIPCODETYPE1).Value) <> "" Then
                            .Rows(n).Cells(dgvCol_Details.PCSTYPE1).ReadOnly = True
                            .Rows(n).Cells(dgvCol_Details.PCSTYPE1).Style.ForeColor = Color.Red
                            LockSTS = True
                        End If

                        If Trim(.Rows(n).Cells(dgvCol_Details.PACKINGSLIPCODETYPE2).Value) <> "" Then
                            .Rows(n).Cells(dgvCol_Details.PCSTYPE2).ReadOnly = True
                            .Rows(n).Cells(dgvCol_Details.PCSTYPE2).Style.ForeColor = Color.Red
                            LockSTS = True
                        End If

                        If Trim(.Rows(n).Cells(dgvCol_Details.PACKINGSLIPCODETYPE3).Value) <> "" Then
                            .Rows(n).Cells(dgvCol_Details.PCSTYPE3).ReadOnly = True
                            .Rows(n).Cells(dgvCol_Details.PCSTYPE3).Style.ForeColor = Color.Red
                            LockSTS = True
                        End If

                        If Trim(.Rows(n).Cells(dgvCol_Details.PACKINGSLIPCODETYPE4).Value) <> "" Then
                            .Rows(n).Cells(dgvCol_Details.PCSTYPE4).ReadOnly = True
                            .Rows(n).Cells(dgvCol_Details.PCSTYPE4).Style.ForeColor = Color.Red
                            LockSTS = True
                        End If

                        If Trim(.Rows(n).Cells(dgvCol_Details.PACKINGSLIPCODETYPE5).Value) <> "" Then
                            .Rows(n).Cells(dgvCol_Details.PCSTYPE5).ReadOnly = True
                            .Rows(n).Cells(dgvCol_Details.PCSTYPE5).Style.ForeColor = Color.Red
                            LockSTS = True
                        End If

                    Next

                End If
                dt2.Clear()

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(dgvCol_Details.SNO).Value = i + 1
                Next

            End With


            With dgv_Details_Total
                If .RowCount = 0 Then .Rows.Add()
                .Rows(0).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value = ""
                .Rows(0).Cells(dgvCol_Details.PCSTYPE1).Value = Format(Val(dt.Rows(0).Item("Total_Type1_Meters").ToString), "########0.00")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE2).Value = Format(Val(dt.Rows(0).Item("Total_Type2_Meters").ToString), "########0.00")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE3).Value = Format(Val(dt.Rows(0).Item("Total_Type3_Meters").ToString), "########0.00")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE4).Value = Format(Val(dt.Rows(0).Item("Total_Type4_Meters").ToString), "########0.00")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE5).Value = Format(Val(dt.Rows(0).Item("Total_Type5_Meters").ToString), "########0.00")
                .Rows(0).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value = Format(Val(dt.Rows(0).Item("Total_Checking_Meters").ToString), "########0.00")
                .Rows(0).Cells(dgvCol_Details.EXCESS_SHORT_MTRS).Value = ""
                .Rows(0).Cells(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Value = Format(Val(dt.Rows(0).Item("Total_Checking_Meters").ToString), "########0.00")
                .Rows(0).Cells(dgvCol_Details.WEIGHT).Value = Format(Val(dt.Rows(0).Item("Total_Weight").ToString), "########0.00")
                .Rows(0).Cells(dgvCol_Details.WEIGHTPERMETER).Value = ""

            End With


            With dgv_Details_Total2
                If .RowCount = 0 Then .Rows.Add()
                .Rows(0).Cells(dgvCol_Details.SNO).Value = "100%"
                .Rows(0).Cells(dgvCol_Details.PCSNO).Value = "FOLDING"
                .Rows(0).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value = Format(Val(dt.Rows(0).Item("Total_Checking_Receipt_Meters").ToString), "########0.00")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE1).Value = Format(Val(dt.Rows(0).Item("Total_Type1Meters_100Folding").ToString), "########0.00")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE2).Value = Format(Val(dt.Rows(0).Item("Total_Type2Meters_100Folding").ToString), "########0.00")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE3).Value = Format(Val(dt.Rows(0).Item("Total_Type3Meters_100Folding").ToString), "########0.00")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE4).Value = Format(Val(dt.Rows(0).Item("Total_Type4Meters_100Folding").ToString), "########0.00")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE5).Value = Format(Val(dt.Rows(0).Item("Total_Type5Meters_100Folding").ToString), "########0.00")
                .Rows(0).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value = Format(Val(dt.Rows(0).Item("Total_Meters_100Folding").ToString), "########0.00")
                .Rows(0).Cells(dgvCol_Details.EXCESS_SHORT_MTRS).Value = Format(Val(dt.Rows(0).Item("Excess_Short_Meter").ToString), "########0.00")
                .Rows(0).Cells(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Value = Format(Val(dt.Rows(0).Item("Total_Meters_100Folding").ToString), "########0.00")
                .Rows(0).Cells(dgvCol_Details.WEIGHT).Value = ""
                .Rows(0).Cells(dgvCol_Details.WEIGHTPERMETER).Value = ""
            End With

            lbl_WarpConsumption.Text = dt.Rows(0).Item("Total_ConsumedPavu").ToString
            lbl_WeftConsumption.Text = dt.Rows(0).Item("Total_ConsumedYarn").ToString

        End If
        dt.Clear()

        dt.Dispose()
        da.Dispose()

        If LockSTS = True Then


            lbl_WeaverName.Enabled = False
            lbl_WeaverName.BackColor = Color.LightGray

            lbl_ClothName.Enabled = False
            lbl_ClothName.BackColor = Color.LightGray

            Cbo_LoomType.Enabled = False
            Cbo_LoomType.BackColor = Color.LightGray

            'cbo_LoomNo.Enabled = False
            'cbo_LoomNo.BackColor = Color.LightGray

            txt_Folding.Enabled = False
            txt_Folding.BackColor = Color.LightGray

            'cbo_WidthType.Enabled = False
            'cbo_WidthType.BackColor = Color.LightGray

            If WGSLockSTS = True Then
                lbl_EndsCount.Enabled = False
                lbl_EndsCount.BackColor = Color.LightGray
            End If


            lbl_Godown_StockIN.Enabled = False
            lbl_Godown_StockIN.BackColor = Color.LightGray

            cbo_StockOff.Enabled = False
            cbo_StockOff.BackColor = Color.LightGray

        End If

        Grid_Cell_DeSelect()

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT MOVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'Finally
        '    If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        'End Try

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String = ""
        Dim tr As SqlClient.SqlTransaction
        Dim vOrdByNo As String = ""
        Dim WagesCode As String = ""
        Dim vLed_IdNo As Integer = 0
        Dim vClo_IdNo As Integer = 0
        Dim vLOTCODE As String = ""
        Dim Lm_ID As Integer = 0
        Dim SQL1 As String = ""
        Dim vPCSCHKCode As String = "", vPCSCHKDate As String = "", vPCSCHKFOLDPERC As String = 0
        Dim Nr As Long = 0

        Con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        Con.Open()


        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_CheckingRefNo.Text)

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_CheckingRefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Weaver_ClothReceipt_and_PieceChecking_Entry, New_Entry, Me, Con, "Weaver_Cloth_Receipt_Head", "Weaver_ClothReceipt_Code", NewCode, "Weaver_ClothReceipt_Date", "(Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

        If Common_Procedures.settings.Vefified_Status = 1 Then
            If Val(Common_Procedures.get_FieldValue(Con, "Weaver_Cloth_Receipt_Head", "Verified_Status", "(Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "')")) = 1 Then
                MessageBox.Show("Entry Already Verified", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If

        If MessageBox.Show("Do you want to Delete ?..", "FOR DELETE", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            msk_date.Focus()
            Exit Sub
        End If

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows!...", "DOES NOT DELETE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            msk_date.Focus()
            Exit Sub
        End If

        Da = New SqlClient.SqlDataAdapter("select count(*) from Weaver_Cloth_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and  (Weaver_Wages_Code <> '' or Weaver_IR_Wages_Code <> '')", Con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already Wages Prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()

        Da = New SqlClient.SqlDataAdapter("select COUNT(*) from Weaver_ClothReceipt_Piece_Details Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and (PackingSlip_Code_Type1 <> '' or PackingSlip_Code_Type2 <> '' or PackingSlip_Code_Type3 <> '' or PackingSlip_Code_Type4 <> '' or PackingSlip_Code_Type5 <> '' or BuyerOffer_Code_Type1 <> '' or BuyerOffer_Code_Type2 <> '' or BuyerOffer_Code_Type3 <> '' or BuyerOffer_Code_Type4 <> '' or BuyerOffer_Code_Type5 <> '' or Bale_UnPacking_Code_Type1 <> '' or Bale_UnPacking_Code_Type2 <> '' or Bale_UnPacking_Code_Type3 <> '' or Bale_UnPacking_Code_Type4 <> '' or Bale_UnPacking_Code_Type5 <> '')", Con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) <> 0 Then
                    MessageBox.Show("Packing Slip prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()


        tr = Con.BeginTransaction

        Try

            cmd.Connection = Con
            cmd.Transaction = tr

            '---------------------------------------------------------------------------------------------------------------------------------------
            '------- -------  Stock Posting
            '---------------------------------------------------------------------------------------------------------------------------------------
            WagesCode = ""
            vLed_IdNo = 0
            vClo_IdNo = 0
            vLOTCODE = ""
            Lm_ID = 0

            Da = New SqlClient.SqlDataAdapter("select a.* from Weaver_Piece_Checking_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'", Con)
            Da.SelectCommand.Transaction = tr
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                vLed_IdNo = Val(Dt1.Rows(0).Item("Ledger_IdNo").ToString)
                vClo_IdNo = Val(Dt1.Rows(0).Item("Cloth_IdNo").ToString)

                'vLOTCODE = Dt1.Rows(0).Item("Piece_Receipt_Code").ToString
                'vLOTCODE = Replace(Trim(UCase(vLOTCODE)), Trim(UCase(PkCondition_CLORCPT)), "")

                If InStr(1, Trim(UCase(Dt1.Rows(0).Item("Piece_Receipt_Code").ToString)), Trim(UCase(PkCondition2_WCLRC))) Then
                    vLOTCODE = Replace(Trim(UCase(Dt1.Rows(0).Item("Piece_Receipt_Code").ToString)), Trim(UCase(PkCondition_CLORCPT)), "")
                Else
                    vLOTCODE = Trim(UCase(Dt1.Rows(0).Item("Piece_Receipt_Code").ToString))
                End If



                vPCSCHKCode = "" : vPCSCHKDate = "" : vPCSCHKFOLDPERC = 0

                Da = New SqlClient.SqlDataAdapter("select top 1 * from Weaver_ClothReceipt_Piece_Details Where Weaver_Piece_Checking_Code <> '' and Weaver_Piece_Checking_Code <> '" & Trim(NewCode) & "' and Weaver_ClothReceipt_Code = '" & Trim(vLOTCODE) & "' order by Weaver_Piece_Checking_Date desc, Weaver_Piece_Checking_Code desc", Con)
                Da.SelectCommand.Transaction = tr
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    If IsDBNull(Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) = False Then
                        vPCSCHKCode = Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString
                        vPCSCHKDate = Dt1.Rows(0).Item("Weaver_Piece_Checking_Date").ToString
                        vPCSCHKFOLDPERC = Dt1.Rows(0).Item("folding").ToString
                    End If
                End If
                Dt1.Clear()

                Call Stock_Posting(NewCode, vLed_IdNo, vClo_IdNo, vLOTCODE, Lm_ID, WagesCode, vPCSCHKCode, vPCSCHKDate, Val(vPCSCHKFOLDPERC), tr)

            End If
            Dt1.Clear()

            '---- stock Posting

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            SQL1 = "Delete from Weaver_ClothReceipt_Piece_Details Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and Create_Status = 0"
            cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
            'cmd.CommandText = "Delete from Weaver_ClothReceipt_Piece_Details Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and Create_Status = 0"
            NR = cmd.ExecuteNonQuery()

            SQL1 = "Update Weaver_ClothReceipt_Piece_Details set Weaver_Piece_Checking_Code = '',  Weaver_Piece_Checking_No = '', Weaver_Piece_Checking_Date = Null, Receipt_Meters = ReceiptMeters_Receipt, ReceiptMeters_Checking = 0, Pick = 0, Width = 0, Type1_Meters = 0, Type2_Meters = 0, Type3_Meters = 0, Type4_Meters = 0, Type5_Meters = 0, Total_Checking_Meters = 0, Weight = 0, Weight_Meter = 0 Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
            cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
            'cmd.CommandText = "Update Weaver_ClothReceipt_Piece_Details set Weaver_Piece_Checking_Code = '',  Weaver_Piece_Checking_No = '', Weaver_Piece_Checking_Date = Null, Receipt_Meters = ReceiptMeters_Receipt, ReceiptMeters_Checking = 0, Loom_No = '', Loom_IdNo = 0, Pick = 0, Width = 0, Type1_Meters = 0, Type2_Meters = 0, Type3_Meters = 0, Type4_Meters = 0, Type5_Meters = 0, Total_Checking_Meters = 0, Weight = 0, Weight_Meter = 0, Beam_Knotting_Code = '', Beam_Knotting_No = '', Width_Type = '', Crimp_Percentage = 0, Set_Code1 = '', Set_No1 = '', Beam_No1 = '', Balance_Meters1 = 0, Set_Code2 = '', Set_No2 = '', Beam_No2 = '', Balance_Meters2 = 0, BeamConsumption_Meters = 0 Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
            NR = cmd.ExecuteNonQuery()

            cmd.CommandText = "DELETE FROM Weaver_Piece_Checking_Head WHERE Company_IdNo = " & Val(lbl_Company.Tag) & " AND Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            tr.Commit()

            MessageBox.Show("Deleted Successfully", "FOR DELETE", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            new_record()

        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT DELETE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            cmd.Dispose()
            tr.Dispose()

            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        End Try



    End Sub

    Private Sub Open_Filter_Entry()
        Dim move As String = ""
        On Error Resume Next

        move = Trim(dgv_Filter_Details.CurrentRow.Cells(dgvCol_Filter.LOTNO).Value)

        If Val(move) <> 0 Then
            Filter_Status = True
            pnl_Back.Enabled = True
            pnl_Filter.Visible = False
            move_record(move)
        End If
        If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        pnl_Back.Enabled = False
        pnl_Filter.Visible = True
        pnl_Filter.BringToFront()
        Filter_Status = True

        dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
        dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()
    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim move As String, inpno As String
        Dim NewCode As String

        Con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        Con.Open()

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Weaver_ClothReceipt_and_PieceChecking_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Weaver_ClothReceipt_and_PieceChecking_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        Try
            inpno = InputBox("Enter New Lot No.", "FOR INSERTION,..")

            NewCode = Trim(Pk_Condition) & Trim(lbl_Company.Tag) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            da = New SqlClient.SqlDataAdapter("SELECT Weaver_ClothReceipt_No from Weaver_Cloth_Receipt_Head WHERE Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'", Con)
            dt = New DataTable
            da.Fill(dt)

            move = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    move = Trim(dt.Rows(0)(0).ToString)
                End If
            End If

            If Trim(move) <> "" Then
                move_record(move)
            Else
                If Trim(inpno) = "" Then
                    MessageBox.Show("Invalid Lot No!......", "DOES NOT INSERT New Lot No,.....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Else
                    new_record()
                    Insert_Entry = True
                    lbl_CheckingRefNo.Text = Trim(UCase(inpno))
                End If
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT New Lot No,.....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
        If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim MOVE As String = ""


        Try

            Con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            Con.Open()

            Da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Piece_Checking_No from Weaver_Piece_Checking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Weaver_Piece_Checking_No", Con)
            'Da = New SqlClient.SqlDataAdapter("SELECT TOP 1 Weaver_ClothReceipt_No FROM Weaver_Cloth_Receipt_Head WHERE Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " AND Weaver_ClothReceipt_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' ORDER BY for_orderby , Weaver_ClothReceipt_No", Con)
            Dt = New DataTable
            Da.Fill(Dt)

            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    MOVE = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()
            Dt.Dispose()
            Da.Dispose()

            If Trim(MOVE) <> "" Then
                move_record(MOVE)
            Else
                new_record()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim MOVE As String = ""


        Try

            Con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            Con.Open()

            Da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Piece_Checking_No from Weaver_Piece_Checking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Weaver_Piece_Checking_No desc", Con)
            'Da = New SqlClient.SqlDataAdapter("SELECT TOP 1 Weaver_ClothReceipt_No FROM Weaver_Cloth_Receipt_Head WHERE Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' ORDER BY for_orderby DESC , Weaver_ClothReceipt_No DESC", Con)
            Dt = New DataTable
            Da.Fill(Dt)

            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    MOVE = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()
            Dt.Dispose()
            Da.Dispose()

            If Trim(MOVE) <> "" Then
                move_record(MOVE)
            Else
                new_record()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim MOVE As String = ""
        Dim OrdByNo As String = ""


        New_Entry = False
        Try

            Con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            Con.Open()


            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_CheckingRefNo.Text))

            Da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Piece_Checking_No from Weaver_Piece_Checking_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Weaver_Piece_Checking_No", Con)
            Dt = New DataTable
            Da.Fill(Dt)

            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    MOVE = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()
            Dt.Dispose()
            Da.Dispose()

            If Trim(MOVE) <> "" Then
                move_record(MOVE)
            Else
                new_record()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim MOVE As String = ""
        Dim OrdByNo As String = ""

        Try

            Con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            Con.Open()


            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_CheckingRefNo.Text))

            Da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Piece_Checking_No from Weaver_Piece_Checking_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Weaver_Piece_Checking_No desc", Con)
            Dt = New DataTable
            Da.Fill(Dt)

            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    MOVE = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()
            Dt.Dispose()
            Da.Dispose()

            If Trim(MOVE) <> "" Then
                move_record(MOVE)
            Else
                new_record()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable

        Try

            clear()
            New_Entry = True

            lbl_CheckingRefNo.Text = Common_Procedures.get_MaxCode(Con, "Weaver_Piece_Checking_Head", "Weaver_Piece_Checking_Code", "For_OrderBy", "(Weaver_Piece_Checking_Code LIKE '" & Trim(Pk_Condition) & "%')", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            'lbl_CheckingRefNo.Text = Common_Procedures.get_MaxCode(Con, "Weaver_Cloth_Receipt_Head ", "Weaver_ClothReceipt_Code", "for_orderby", "(Weaver_ClothReceipt_Code LIKE '" & Trim(Pk_Condition) & "%')", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_CheckingRefNo.ForeColor = Color.Red
            If dtp_Date.Enabled = True Then msk_date.Text = Date.Today.ToShortDateString

            da = New SqlClient.SqlDataAdapter("SELECT TOP 1 * FROM Weaver_Piece_Checking_Head WHERE Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' ORDER BY for_orderby DESC , Weaver_Piece_Checking_No DESC", Con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt.Rows(0).Item("Weaver_Piece_Checking_Date").ToString <> "" Then dtp_Date.Text = dt.Rows(0).Item("Weaver_Piece_Checking_Date").ToString
                End If
                If dt.Rows(0).Item("Loom_Type").ToString <> "" Then Cbo_LoomType.Text = dt.Rows(0).Item("Loom_Type").ToString
                If dt.Rows(0).Item("Width_Type").ToString <> "" Then cbo_WidthType.Text = dt.Rows(0).Item("Width_Type").ToString
            End If
            dt.Clear()
            dt.Dispose()
            da.Dispose()

            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
            msk_date.SelectionStart = 0

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim move As String, inpno As String
        Dim InvCode As String


        Try

            Con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            Con.Open()


            inpno = InputBox("Enter Lot No.", "FOR FINDING,..")

            InvCode = Trim(Pk_Condition) & Trim(lbl_Company.Tag) & "-" & (inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("SELECT Weaver_ClothReceipt_No FROM Weaver_Cloth_Receipt_Head WHERE Company_IdNo = " & (Val(lbl_Company.Tag)) & " AND Weaver_ClothReceipt_Code = '" & Trim(InvCode) & "'", Con)
            Dt = New DataTable
            Da.Fill(Dt)

            move = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    move = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Da.Dispose()
            Dt.Dispose()
            Dt.Clear()

            If Trim(move) <> "" Then
                move_record(move)
            Else
                MessageBox.Show("Lot No." & Str(Val(inpno)) & " does not exists!...", "DOES NOT FIND", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
        If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        '-------------
    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim vLed_IdNo As Integer = 0
        Dim vClo_IdNo As Integer = 0
        Dim vEndct_IdNo As Integer = 0
        Dim Lm_ID As Integer = 0
        Dim vOrdByNo As String = 0
        Dim vOrdByPcsNo As String = 0
        Dim WftCnt_ID As Integer = 0
        Dim Nr As Integer = 0
        Dim I As Integer = 0, J As Integer = 0, K As Integer = 0
        Dim SNo As Integer = 0
        Dim StkOff_ID As Integer = 0, vStkOf_Pos_IdNo As Integer = 0
        Dim vGod_ID As Integer = 0
        Dim vLm_IdNo As Integer = 0
        Dim vTotTyp1Mtr As String = 0, vTotTyp2Mtr As String = 0, vTotTyp3Mtr As String = 0, vTotTyp4Mtr As String = 0, vTotTyp5Mtr As String = 0, vTotChkMtr As String = 0, vTotWgt As String = 0
        Dim vTotRcptMtr As String = 0, vTot_100Fld_Typ1Mtr As String = 0, vTot_100Fld_Typ2Mtr As String = 0, vTot_100Fld_Typ3Mtr As String = 0, vTot_100Fld_Typ4Mtr As String = 0, vTot_100Fld_Typ5Mtr As String = 0, vTot_100Fld_ChkMtr As String = 0, vTotExcShtMtr As String = 0
        Dim Dup_PcNo As String = ""
        Dim vDelv_ID As Integer = 0, vRec_ID As Integer = 0
        Dim Led_type As String = ""
        Dim EntID As String = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim StkDelvTo_ID As Integer = 0, StkRecFrm_ID As Integer = 0
        Dim UC_Mtrs As String = 0
        Dim vErrMsg As String = ""
        Dim vSELC_LOTCODE As String = ""
        Dim SQL1 As String
        Dim vPCSCODE_FORSELECTION As String
        Dim WagesCode As String = ""
        Dim vPCS_REED As String = 0
        Dim vPCS_PICK As String = 0
        Dim vPCS_WIDTH As String = 0
        Dim vCLOMAS_REED As String = 0
        Dim vCLOMAS_PICK As String = 0
        Dim vCLOMAS_WIDTH As String = 0



        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection ?", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close other windows!....", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        Con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        Con.Open()

        If IsDate(msk_date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            msk_date.Focus()
            Exit Sub
        End If

        If Not (Convert.ToDateTime(msk_date.Text) >= Common_Procedures.Company_FromDate And (msk_date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of Financial Year !...", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            msk_date.Focus()
            Exit Sub
        End If

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_CheckingRefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Weaver_Piece_Checking_Entry, New_Entry, Me, Con, "Weaver_Piece_Checking_Head", "Weaver_Piece_Checking_Code", NewCode, "Weaver_Piece_Checking_Date", "(Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code LIKE '" & Trim(Trim(Pk_Condition)) & "%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Weaver_Piece_Checking_No desc", dtp_Date.Value.Date) = False Then Exit Sub

        If Trim(txt_PieceNo.Text) = "" Or Trim(txt_PieceNo.Text) = "0" Then
            MessageBox.Show("Invalid Piece No.?", "DOESNOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_PieceNo.Enabled And txt_PieceNo.Visible Then txt_PieceNo.Focus()
            Exit Sub
        End If

        vLed_IdNo = Common_Procedures.Ledger_NameToIdNo(Con, lbl_WeaverName.Text)
        If Val(vLed_IdNo) = 0 Then
            MessageBox.Show("Invalid Weaver Name ?", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_PieceNo.Enabled And txt_PieceNo.Visible Then txt_PieceNo.Focus()
            Exit Sub
        End If

        Led_type = Common_Procedures.get_FieldValue(Con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(vLed_IdNo)) & ")")

        vClo_IdNo = Common_Procedures.Cloth_NameToIdNo(Con, lbl_ClothName.Text)
        If Val(vClo_IdNo) = 0 Then
            MessageBox.Show("Invalid Fabric Name?", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_PieceNo.Enabled And txt_PieceNo.Visible Then txt_PieceNo.Focus()
            Exit Sub
        End If

        vEndct_IdNo = Common_Procedures.EndsCount_NameToIdNo(Con, lbl_EndsCount.Text)
        If Val(vEndct_IdNo) = 0 Then
            MessageBox.Show("Invalid Ends/Count?", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_PieceNo.Enabled And txt_PieceNo.Visible Then txt_PieceNo.Focus()
            Exit Sub
        End If

        If Val(txt_Folding.Text) = 0 Then txt_Folding.Text = 100

        If Trim(Cbo_LoomType.Text) = "" Then
            Cbo_LoomType.Text = "AUTOLOOM"
        End If

        WftCnt_ID = Common_Procedures.Count_NameToIdNo(Con, lbl_WeftCount.Text)
        If Val(WftCnt_ID) = 0 Then
            MessageBox.Show("Invalid Weft Count ?", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_PieceNo.Enabled And txt_PieceNo.Visible Then txt_PieceNo.Focus()
            Exit Sub
        End If

        If cbo_WidthType.Visible = True Then
            If Trim(UCase(Cbo_LoomType.Text)) = "AUTOLOOM" Then
                If Trim(cbo_WidthType.Text) = "" Then
                    MessageBox.Show("Invalid Width Type?", "DOESNOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If txt_PieceNo.Enabled And txt_PieceNo.Visible Then txt_PieceNo.Focus()
                    Exit Sub
                End If
            End If
        End If


        If Trim(lbl_ReceiptCode.Text) = "" Then
            MessageBox.Show("Invalid Receipt No.?", "DOESNOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_PieceNo.Enabled And txt_PieceNo.Visible Then txt_PieceNo.Focus()
            Exit Sub
        End If

        vLm_IdNo = Common_Procedures.Loom_NameToIdNo(Con, lbl_LoomNo.Text)

        vGod_ID = Common_Procedures.Ledger_AlaisNameToIdNo(Con, lbl_Godown_StockIN.Text)
        If lbl_Godown_StockIN.Visible = True Then
            If vGod_ID = 0 Then
                MessageBox.Show("Invalid Fabric Godown Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If lbl_Godown_StockIN.Enabled And lbl_Godown_StockIN.Visible Then lbl_Godown_StockIN.Focus()
                Exit Sub
            End If
        End If
        If vGod_ID = 0 Then vGod_ID = Common_Procedures.CommonLedger.Godown_Ac

        StkOff_ID = Common_Procedures.Ledger_AlaisNameToIdNo(Con, cbo_StockOff.Text)
        If cbo_StockOff.Visible = True Then
            If StkOff_ID = 0 Then
                MessageBox.Show("Invalid Stock Off Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_StockOff.Enabled And cbo_StockOff.Visible Then cbo_StockOff.Focus()
                Exit Sub
            End If
        End If
        If StkOff_ID = 0 Then StkOff_ID = Common_Procedures.CommonLedger.OwnSort_Ac


        With dgv_Details

            Dup_PcNo = ""
            For I = 0 To .RowCount - 1

                If Trim(.Rows(I).Cells(dgvCol_Details.PCSNO).Value) <> "" Or Val(dgv_Details.Rows(I).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value) <> 0 Or Val(dgv_Details.Rows(I).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value) <> 0 Or Val(dgv_Details.Rows(I).Cells(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Value) <> 0 Then

                    If Trim(.Rows(I).Cells(dgvCol_Details.PCSNO).Value) = "" Then
                        MessageBox.Show("Invalid Pcs No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .CurrentCell = .Rows(I).Cells(dgvCol_Details.PCSNO)
                            .Focus()
                        End If
                        Exit Sub
                    End If

                    If InStr(1, Trim(UCase(Dup_PcNo)), "~" & Trim(UCase(.Rows(I).Cells(dgvCol_Details.PCSNO).Value)) & "~") > 0 Then
                        MessageBox.Show("Duplicate Pcs No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(I).Cells(dgvCol_Details.PCSNO)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    Dup_PcNo = Trim(Dup_PcNo) & "~" & Trim(UCase(.Rows(I).Cells(dgvCol_Details.PCSNO).Value)) & "~"

                End If

            Next

        End With

        WagesCode = ""
        Da = New SqlClient.SqlDataAdapter("select * from Weaver_Cloth_Receipt_Head Where (Weaver_ClothReceipt_Code = '" & Trim(lbl_ReceiptCode.Text) & "' or '" & Trim(PkCondition_CLORCPT) & "' + Weaver_ClothReceipt_Code = '" & Trim(lbl_ReceiptCode.Text) & "')", Con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("Weaver_Wages_Code").ToString) = False Then
                WagesCode = Dt1.Rows(0).Item("Weaver_Wages_Code").ToString
            End If
        End If
        Dt1.Clear()

        vCLOMAS_REED = 0
        vCLOMAS_PICK = 0
        vCLOMAS_WIDTH = 0
        Da = New SqlClient.SqlDataAdapter("select * from Cloth_Head Where Cloth_IdNo = " & Str(Val(vClo_IdNo)), Con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("Cloth_Reed").ToString) = False Then
                vCLOMAS_REED = Dt1.Rows(0).Item("Cloth_Reed").ToString
            End If
            If IsDBNull(Dt1.Rows(0).Item("Cloth_Pick").ToString) = False Then
                vCLOMAS_PICK = Dt1.Rows(0).Item("Cloth_Pick").ToString
            End If
            If IsDBNull(Dt1.Rows(0).Item("Cloth_Width").ToString) = False Then
                vCLOMAS_WIDTH = Dt1.Rows(0).Item("Cloth_Width").ToString
            End If
        End If
        Dt1.Clear()


        Calculation_TotalMeter()

        vTotTyp1Mtr = 0 : vTotTyp2Mtr = 0 : vTotTyp3Mtr = 0 : vTotTyp4Mtr = 0 : vTotTyp5Mtr = 0 : vTotChkMtr = 0
        vTotWgt = 0

        If dgv_Details_Total.RowCount > 0 Then

            vTotTyp1Mtr = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.PCSTYPE1).Value)
            vTotTyp2Mtr = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.PCSTYPE2).Value)
            vTotTyp3Mtr = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.PCSTYPE3).Value)
            vTotTyp4Mtr = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.PCSTYPE4).Value)
            vTotTyp5Mtr = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.PCSTYPE5).Value)
            vTotChkMtr = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value)

            vTotWgt = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.WEIGHT).Value)

        End If

        vTotRcptMtr = 0
        vTot_100Fld_Typ1Mtr = 0 : vTot_100Fld_Typ2Mtr = 0 : vTot_100Fld_Typ3Mtr = 0 : vTot_100Fld_Typ4Mtr = 0 : vTot_100Fld_Typ5Mtr = 0 : vTot_100Fld_ChkMtr = 0
        vTotExcShtMtr = 0

        If dgv_Details_Total2.RowCount > 0 Then
            vTotRcptMtr = Val(dgv_Details_Total2.Rows(0).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value)
            vTot_100Fld_Typ1Mtr = Val(dgv_Details_Total2.Rows(0).Cells(dgvCol_Details.PCSTYPE1).Value)
            vTot_100Fld_Typ2Mtr = Val(dgv_Details_Total2.Rows(0).Cells(dgvCol_Details.PCSTYPE2).Value)
            vTot_100Fld_Typ3Mtr = Val(dgv_Details_Total2.Rows(0).Cells(dgvCol_Details.PCSTYPE3).Value)
            vTot_100Fld_Typ4Mtr = Val(dgv_Details_Total2.Rows(0).Cells(dgvCol_Details.PCSTYPE4).Value)
            vTot_100Fld_Typ5Mtr = Val(dgv_Details_Total2.Rows(0).Cells(dgvCol_Details.PCSTYPE5).Value)
            vTot_100Fld_ChkMtr = Val(dgv_Details_Total2.Rows(0).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value)
            vTotExcShtMtr = Val(dgv_Details_Total2.Rows(0).Cells(dgvCol_Details.EXCESS_SHORT_MTRS).Value)
        End If

        Calculation_Pavu_Consumed()
        Calculation_Yarn_Consumed()

        tr = Con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_CheckingRefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            Else
                lbl_CheckingRefNo.Text = Common_Procedures.get_MaxCode(Con, "Weaver_Piece_Checking_Head", "Weaver_Piece_Checking_Code", "For_OrderBy", "(Weaver_Piece_Checking_Code LIKE '" & Trim(Pk_Condition) & "%')", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)
                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_CheckingRefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            End If

            cmd.Connection = Con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@checkingdate", Convert.ToDateTime(msk_date.Text))
            cmd.Parameters.AddWithValue("@receiptdate", Convert.ToDateTime(lbl_ReceiptDate.Text))

            vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_CheckingRefNo.Text)
            vSELC_LOTCODE = Trim(lbl_ReceiptNo.Text) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_Company.Tag)) & "/" & Trim(Pk_Condition)

            cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
            cmd.ExecuteNonQuery()

            If New_Entry = True Then

                cmd.CommandText = "Insert into Weaver_Piece_Checking_Head (              Receipt_Type            , Weaver_Piece_Checking_Code,               Company_IdNo       ,     Weaver_Piece_Checking_No          ,           for_OrderBy     , Weaver_Piece_Checking_Date,       Ledger_IdNo          ,           Receipt_PkCondition     ,         Piece_Receipt_Code          ,         Piece_Receipt_No          , Piece_Receipt_Date,             Lot_No                ,          Cloth_IdNo         ,             Party_DcNo             , noof_pcs,             ReceiptMeters_Receipt       ,               Folding              , Total_Checking_Receipt_Meters,           Total_Type1_Meters  ,      Total_Type2_Meters       ,   Total_Type3_Meters         ,     Total_Type4_Meters        ,     Total_Type5_Meters       ,       Total_Checking_Meters ,        Total_Weight      ,  Total_Type1Meters_100Folding          , Total_Type2Meters_100Folding            ,  Total_Type3Meters_100Folding        ,    Total_Type4Meters_100Folding       ,     Total_Type5Meters_100Folding     ,      Total_Meters_100Folding         ,         Excess_Short_Meter     , StockOff_IdNo               ,               Loom_Type           ,               Piece_No           ,                Loom_No          ,                Total_ConsumedPavu          ,              Total_ConsumedYarn            ,                           user_idNo      ) " &
                                            "     Values                  ( '" & Trim(UCase(vRECEIPTTYPE)) & "'  ,   '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_CheckingRefNo.Text) & "', " & Str(Val(vOrdByNo)) & ",        @checkingdate      , " & Str(Val(vLed_IdNo)) & ", '" & Trim(PkCondition_CLORCPT) & "', '" & Trim(lbl_ReceiptCode.Text) & "', '" & Trim(lbl_ReceiptNo.Text) & "',      @receiptdate , '" & Trim(lbl_ReceiptNo.Text) & "',  " & Str(Val(vClo_IdNo)) & ", '" & Trim(lbl_PartyLotNo.Text) & "',    1    , " & Str(Val(lbl_ReceiptMeters.Text)) & ",  " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(vTotRcptMtr)) & ",  " & Str(Val(vTotTyp1Mtr)) & ",  " & Str(Val(vTotTyp2Mtr)) & ", " & Str(Val(vTotTyp3Mtr)) & ",  " & Str(Val(vTotTyp4Mtr)) & ", " & Str(Val(vTotTyp5Mtr)) & ", " & Str(Val(vTotChkMtr)) & ", " & Str(Val(vTotWgt)) & ", " & Str(Val(vTot_100Fld_Typ1Mtr)) & "  ,    " & Str(Val(vTot_100Fld_Typ2Mtr)) & ", " & Str(Val(vTot_100Fld_Typ3Mtr)) & ",  " & Str(Val(vTot_100Fld_Typ4Mtr)) & ", " & Str(Val(vTot_100Fld_Typ5Mtr)) & ",  " & Str(Val(vTot_100Fld_ChkMtr)) & ", " & Str(Val(vTotExcShtMtr)) & ", " & Str(Val(StkOff_ID)) & " , '" & Trim(Cbo_LoomType.Text) & "' , '" & Trim(txt_PieceNo.Text) & "' , '" & Trim(lbl_LoomNo.Text) & "' , " & Str(Val(lbl_WarpConsumption.Text)) & " , " & Str(Val(lbl_WeftConsumption.Text)) & " , " & Val(Common_Procedures.User.IdNo) & " ) "
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Weaver_Piece_Checking_Head set Receipt_Type = '" & Trim(UCase(vRECEIPTTYPE)) & "', Weaver_Piece_Checking_Date = @checkingdate, Ledger_IdNo = " & Str(Val(vLed_IdNo)) & ", Receipt_PkCondition = '" & Trim(PkCondition_CLORCPT) & "', Piece_Receipt_Code = '" & Trim(lbl_ReceiptCode.Text) & "', Piece_Receipt_No = '" & Trim(lbl_ReceiptNo.Text) & "', Piece_Receipt_Date = @receiptdate, Lot_No = '" & Trim(lbl_ReceiptNo.Text) & "', Cloth_IdNo = " & Str(Val(vClo_IdNo)) & ", Party_DcNo = '" & Trim(lbl_PartyLotNo.Text) & "', noof_pcs = 1, ReceiptMeters_Receipt = " & Str(Val(lbl_ReceiptMeters.Text)) & ", Folding =  " & Str(Val(txt_Folding.Text)) & ", Total_Checking_Receipt_Meters =  " & Str(Val(vTotRcptMtr)) & ", Total_Type1_Meters = " & Str(Val(vTotTyp1Mtr)) & ",  Total_Type2_Meters = " & Str(Val(vTotTyp2Mtr)) & ", Total_Type3_Meters = " & Str(Val(vTotTyp3Mtr)) & ", Total_Type4_Meters = " & Str(Val(vTotTyp4Mtr)) & ", Total_Type5_Meters = " & Str(Val(vTotTyp5Mtr)) & ", Total_Checking_Meters = " & Str(Val(vTotChkMtr)) & ", Total_Weight = " & Str(Val(vTotWgt)) & ", Total_Type1Meters_100Folding = " & Str(Val(vTot_100Fld_Typ1Mtr)) & ", Total_Type2Meters_100Folding = " & Str(Val(vTot_100Fld_Typ2Mtr)) & ", Total_Type3Meters_100Folding = " & Str(Val(vTot_100Fld_Typ3Mtr)) & ", Total_Type4Meters_100Folding = " & Str(Val(vTot_100Fld_Typ4Mtr)) & ", Total_Type5Meters_100Folding = " & Str(Val(vTot_100Fld_Typ5Mtr)) & ", Total_Meters_100Folding  =  " & Str(Val(vTot_100Fld_ChkMtr)) & ", Excess_Short_Meter = " & Str(Val(vTotExcShtMtr)) & " , StockOff_IdNo = " & Str(Val(StkOff_ID)) & ", Loom_Type = '" & Trim(Cbo_LoomType.Text) & "', Piece_No = '" & Trim(txt_PieceNo.Text) & "' , Loom_No = '" & Trim(lbl_LoomNo.Text) & "' , Total_ConsumedPavu = " & Str(Val(lbl_WarpConsumption.Text)) & " , Total_ConsumedYarn = " & Str(Val(lbl_WeftConsumption.Text)) & " , User_idNo = " & Val(Common_Procedures.User.IdNo) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                SQL1 = "Delete from Weaver_ClothReceipt_Piece_Details Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and Create_Status = 0 and PackingSlip_Code_Type1 = '' and PackingSlip_Code_Type2 = ''  and PackingSlip_Code_Type3 = ''  and PackingSlip_Code_Type4 = ''  and PackingSlip_Code_Type5 = '' and BuyerOffer_Code_Type1 = '' and BuyerOffer_Code_Type2 = '' and BuyerOffer_Code_Type3 = '' and BuyerOffer_Code_Type4 = '' and BuyerOffer_Code_Type5 = ''"
                cmd.CommandText = "EXEC SP_ExecuteQuery '" & Replace(Trim(SQL1), "'", "''") & "'"
                cmd.ExecuteNonQuery()

                SQL1 = "Update Weaver_ClothReceipt_Piece_Details set Weaver_Piece_Checking_Code = '',  Weaver_Piece_Checking_No = '', Weaver_Piece_Checking_Date = Null, Receipt_Meters = ReceiptMeters_Receipt, ReceiptMeters_Checking = 0, Pick = 0, Width = 0, Type1_Meters = 0, Type2_Meters = 0, Type3_Meters = 0, Type4_Meters = 0, Type5_Meters = 0, Total_Checking_Meters = 0, Weight = 0, Weight_Meter = 0, Total_CheckingMeters_100Folding = 0, ExcessShort_Status_YesNo = '', Excess_Short_Meter = 0, BeamNo_SetCode = '' Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
                cmd.CommandText = "EXEC SP_ExecuteQuery '" & Replace(Trim(SQL1), "'", "''") & "'"
                cmd.ExecuteNonQuery()

            End If

            Dim vLOTCODE As String

            If InStr(1, Trim(UCase(lbl_ReceiptCode.Text)), Trim(UCase(PkCondition2_WCLRC))) Then
                vLOTCODE = Replace(Trim(UCase(lbl_ReceiptCode.Text)), Trim(UCase(PkCondition_CLORCPT)), "")
            Else
                vLOTCODE = Trim(UCase(lbl_ReceiptCode.Text))
            End If


            With dgv_Details

                SNo = 0

                For I = 0 To .Rows.Count - 1

                    If Trim(.Rows(I).Cells(dgvCol_Details.PCSNO).Value) <> "" Or Val(dgv_Details.Rows(I).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value) <> 0 Or Val(dgv_Details.Rows(I).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value) <> 0 Or Val(dgv_Details.Rows(I).Cells(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Value) <> 0 Then

                        SNo = SNo + 1

                        vOrdByPcsNo = Common_Procedures.OrderBy_CodeToValue(dgv_Details.Rows(I).Cells(dgvCol_Details.PCSNO).Value)

                        vPCSCODE_FORSELECTION = Trim(UCase(.Rows(I).Cells(dgvCol_Details.PCSNO).Value)) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_Company.Tag)) & "/" & Trim(PkCondition_CLORCPT)


                        vPCS_REED = 0
                        vPCS_PICK = 0
                        vPCS_WIDTH = 0


                        If Val(.Rows(I).Cells(dgvCol_Details.REED).Value) <> 0 Then
                            vPCS_REED = Val(.Rows(I).Cells(dgvCol_Details.REED).Value)
                        Else
                            vPCS_REED = Val(vCLOMAS_REED)
                        End If
                        If Val(.Rows(I).Cells(dgvCol_Details.PICK).Value) <> 0 Then
                            vPCS_PICK = Val(.Rows(I).Cells(dgvCol_Details.PICK).Value)
                        Else
                            vPCS_PICK = Val(vCLOMAS_PICK)
                        End If
                        If Val(.Rows(I).Cells(dgvCol_Details.WIDTH).Value) <> 0 Then
                            vPCS_WIDTH = Val(.Rows(I).Cells(dgvCol_Details.WIDTH).Value)
                        Else
                            vPCS_WIDTH = Val(vCLOMAS_WIDTH)
                        End If


                        Nr = 0
                        cmd.CommandText = "UPDATE Weaver_ClothReceipt_Piece_Details  SET Weaver_ClothReceipt_Date =  @receiptdate, Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "', Weaver_Piece_Checking_No = '" & Trim(lbl_CheckingRefNo.Text) & "', Weaver_Piece_Checking_Date =  @checkingdate, Ledger_IdNo = " & Str(Val(vLed_IdNo)) & ", StockOff_IdNo = " & Str(Val(StkOff_ID)) & ", WareHouse_IdNo = " & Str(Val(vGod_ID)) & ", Cloth_IdNo = " & Val(vClo_IdNo) & ", Folding_Receipt = " & Val(txt_Folding.Text) & ", Folding_Checking = " & Val(txt_Folding.Text) & ", Folding = " & Val(txt_Folding.Text) & ", Loom_No = '" & Trim(lbl_LoomNo.Text) & "', Loom_IdNo = " & Str(Val(vLm_IdNo)) & ",  Width_Type = '" & Trim(cbo_WidthType.Text) & "', Sl_No =" & Str(Val(SNo)) & ", Main_PieceNo = '" & Trim(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.PCSNO).Value)) & "', PieceNo_OrderBy = " & Str(Val(vOrdByPcsNo)) & ", PieceCode_for_Selection = '" & Trim(vPCSCODE_FORSELECTION) & "', ReceiptMeters_Receipt = " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value)) & " , ReceiptMeters_Checking = " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value)) & " , Receipt_Meters = " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value)) & " , Type1_Meters =" & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.PCSTYPE1).Value)) & " ,Type2_Meters =" & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.PCSTYPE2).Value)) & ", Type3_Meters =" & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.PCSTYPE3).Value)) & ", Type4_Meters =" & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.PCSTYPE4).Value)) & ", Type5_Meters = " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.PCSTYPE5).Value)) & ", Total_Checking_Meters = " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value)) & ", Excess_Short_Meter = " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.EXCESS_SHORT_MTRS).Value)) & ", Weight = " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.WEIGHT).Value)) & ", Weight_Meter = " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.WEIGHTPERMETER).Value)) & " , REED = " & Str(Val(vPCS_REED)) & ", PICK = " & Str(Val(vPCS_PICK)) & ", WIDTH = " & Str(Val(vPCS_WIDTH)) & " Where Weaver_ClothReceipt_Code = '" & Trim(lbl_ReceiptCode.Text) & "' and Lot_Code ='" & Trim(vLOTCODE) & "' and Piece_No = '" & Trim(dgv_Details.Rows(I).Cells(dgvCol_Details.PCSNO).Value) & "'"
                        Nr = cmd.ExecuteNonQuery()
                        If Nr = 0 Then

                            cmd.CommandText = "INSERT INTO Weaver_ClothReceipt_Piece_Details (   Company_IdNo               ,           Lot_Code        ,             Weaver_ClothReceipt_Code  ,       Weaver_ClothReceipt_No       ,        for_orderby         ,   Weaver_ClothReceipt_Date,               Lot_No              ,  Weaver_Piece_Checking_Code,       Weaver_Piece_Checking_No         , Weaver_Piece_Checking_Date,          Ledger_IdNo       ,            StockOff_IdNo     ,         WareHouse_IdNo     ,         Cloth_IdNo    ,         Folding_Receipt      ,        Folding_Checking      ,             Folding          ,               Loom_No          ,           Loom_IdNo       ,               Width_Type           ,            Sl_No     ,                               Piece_No                               ,                                   Main_PieceNo                             ,          PieceNo_OrderBy       ,           PieceCode_for_Selection     ,                                  ReceiptMeters_Receipt                              ,                                 ReceiptMeters_Checking                             ,                                 Receipt_Meters                                      ,                                 Type1_Meters                               ,                                   Type2_Meters                               ,                                    Type3_Meters                               ,                                 Type4_Meters                               ,                                   Type5_Meters                               ,                                  Total_Checking_Meters                              ,                                 Excess_Short_Meter                                 ,                                 Weight                                  ,                                   Weight_Meter                                     ,              REED          ,               PICK         ,                WIDTH         , Create_Status, Remarks, PackingSlip_Code_Type1, PackingSlip_Code_Type2, PackingSlip_Code_Type3, PackingSlip_Code_Type4, PackingSlip_Code_Type5, BuyerOffer_Code_Type1, BuyerOffer_Code_Type2, BuyerOffer_Code_Type3, BuyerOffer_Code_Type4, BuyerOffer_Code_Type5 , Bale_UnPacking_Code_Type1, Bale_UnPacking_Code_Type2, Bale_UnPacking_Code_Type3, Bale_UnPacking_Code_Type4, Bale_UnPacking_Code_Type5 )" &
                                              "             VALUES                           (" & Val(lbl_Company.Tag) & "  , '" & Trim(vLOTCODE) & "'  ,  '" & Trim(lbl_ReceiptCode.Text) & "' ,  '" & Trim(lbl_ReceiptNo.Text) & "', " & Str(Val(vOrdByNo)) & " ,           @receiptdate    , '" & Trim(lbl_ReceiptNo.Text) & "',  '" & Trim(NewCode) & "'   ,  '" & Trim(lbl_CheckingRefNo.Text) & "',       @checkingdate       , " & Str(Val(vLed_IdNo)) & ",   " & Str(Val(StkOff_ID)) & ",   " & Str(Val(vGod_ID)) & ", " & Val(vClo_IdNo) & ", " & Val(txt_Folding.Text) & ", " & Val(txt_Folding.Text) & ", " & Val(txt_Folding.Text) & ", '" & Trim(lbl_LoomNo.Text) & "', " & Str(Val(vLm_IdNo)) & ", '" & Trim(cbo_WidthType.Text) & "' , " & Str(Val(SNo)) & ", '" & Trim(dgv_Details.Rows(I).Cells(dgvCol_Details.PCSNO).Value) & "', '" & Trim(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.PCSNO).Value)) & "' ,  " & Str(Val(vOrdByPcsNo)) & " , '" & Trim(vPCSCODE_FORSELECTION) & "' ,  " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value)) & ", " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value)) & ", " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value)) & " , " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.PCSTYPE1).Value)) & " ,   " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.PCSTYPE2).Value)) & " ,    " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.PCSTYPE3).Value)) & " , " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.PCSTYPE4).Value)) & " ,   " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.PCSTYPE5).Value)) & " ,  " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value)) & ", " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.EXCESS_SHORT_MTRS).Value)) & ", " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.WEIGHT).Value)) & ",   " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.WEIGHTPERMETER).Value)) & " , " & Str(Val(vPCS_REED)) & ", " & Str(Val(vPCS_PICK)) & ", " & Str(Val(vPCS_WIDTH)) & " ,      1       ,    ''  ,               ''      ,              ''       ,            ''         ,            ''         ,           ''          ,            ''        ,          ''          ,             ''       ,            ''        ,          ''           ,            ''            ,          ''              ,             ''           ,            ''            ,          ''               ) "
                            cmd.ExecuteNonQuery()

                        End If

                    End If

                Next I

            End With


            '----------------------------------------------------------------------------------------------------------------------------------------
            '------- -------  Stock Posting
            '----------------------------------------------------------------------------------------------------------------------------------------

            '---- stock Posting
            Call Stock_Posting(NewCode, vLed_IdNo, vClo_IdNo, vLOTCODE, vLm_IdNo, WagesCode, NewCode, msk_date.Text, Val(txt_Folding.Text), tr)


            'cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            'cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            'cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()


            ''-------Pavu Stock Posting
            'cmd.CommandText = "Insert into Stock_Pavu_Processing_Details (      Reference_Code   ,              Company_IdNo        ,          Reference_No         ,             for_OrderBy   , Reference_Date,       DeliveryTo_Idno     ,      ReceivedFrom_Idno   ,         Cloth_Idno         ,        Entry_ID      ,        Party_Bill_No ,       Particulars      ,         Sl_No        ,           EndsCount_IdNo     , Sized_Beam,                Meters               ) " &
            '                    "           Values                       ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_CheckingRefNo.Text) & "', " & Str(Val(vOrdByNo)) & ",    @EntryDate , " & Str(Val(vDelv_ID)) & ", " & Str(Val(vRec_ID)) & ", " & Str(Val(vClo_IdNo)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(SNo)) & ", " & Str(Val(vEndct_IdNo)) & ",      0    , " & Str(Val(lbl_WarpConsumption.Text)) & " ) "
            'cmd.ExecuteNonQuery()


            ''-------Yarn Stock Posting
            'cmd.CommandText = "Insert into Stock_Yarn_Processing_Details (      Reference_Code   ,                Company_IdNo      ,            Reference_No       ,           for_OrderBy     , Reference_Date,        DeliveryTo_Idno    ,       ReceivedFrom_Idno  ,        Entry_ID      ,       Particulars      ,      Party_Bill_No   , Sl_No,          Count_IdNo        , Yarn_Type, Mill_IdNo, Bags, Cones,               Weight                ) " &
            '                    "           Values                       ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_CheckingRefNo.Text) & "', " & Str(Val(vOrdByNo)) & ",    @EntryDate , " & Str(Val(vDelv_ID)) & ", " & Str(Val(vRec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "',   1  , " & Str(Val(WftCnt_ID)) & ",  'MILL'  ,     0    ,   0 ,   0  , " & Str(Val(lbl_WeftConsumption.Text)) & " ) "
            'cmd.ExecuteNonQuery()

            ''-------Cloth Stock Posting

            'UC_Mtrs = 0
            'If Val(vTotChkMtr) = 0 Then UC_Mtrs = Val(lbl_ReceiptMeters.Text)

            'cmd.CommandText = "Insert into Stock_Cloth_Processing_Details (       Reference_Code   ,             Company_IdNo         ,             Reference_No      ,         for_OrderBy       , Reference_Date,             StockOff_IdNo        ,        DeliveryTo_Idno        ,       ReceivedFrom_Idno       ,         Entry_ID     ,      Party_Bill_No   ,      Particulars       , Sl_No,          Cloth_Idno        ,                  Folding           ,        UnChecked_Meters  ,      Meters_Type1            ,         Meters_Type2         ,          Meters_Type3         ,          Meters_Type4        ,        Meters_Type5           ) " &
            '                    "           Values                        ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_CheckingRefNo.Text) & "', " & Str(Val(vOrdByNo)) & ",   @EntryDate  , " & Str(Val(vStkOf_Pos_IdNo)) & ", " & Str(Val(StkDelvTo_ID)) & ", " & Str(Val(StkRecFrm_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',   1  , " & Str(Val(vClo_IdNo)) & ",  " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(UC_Mtrs)) & ", " & Str(Val(vTotTyp1Mtr)) & ", " & Str(Val(vTotTyp2Mtr)) & ",  " & Str(Val(vTotTyp3Mtr)) & ", " & Str(Val(vTotTyp4Mtr)) & ", " & Str(Val(vTotTyp5Mtr)) & " ) "
            'cmd.ExecuteNonQuery()


            EntID = Trim(Pk_Condition) & Trim(lbl_CheckingRefNo.Text)
            Partcls = "Checking : PcsNo. " & Trim(txt_PieceNo.Text)
            Partcls = Trim(Partcls) & ",  Cloth : " & Trim(lbl_ClothName.Text)

            PBlNo = Trim(txt_PieceNo.Text)

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            vDelv_ID = 0 : vRec_ID = 0
            If Trim(UCase(Led_type)) = "JOBWORKER" Then
                vDelv_ID = vLed_IdNo
                vRec_ID = 0

            Else

                vDelv_ID = 0
                vRec_ID = vLed_IdNo

            End If

            '-------Pavu Stock Posting
            cmd.CommandText = "Insert into Stock_Pavu_Processing_Details (      Reference_Code   ,              Company_IdNo        ,          Reference_No         ,             for_OrderBy   , Reference_Date,       DeliveryTo_Idno     ,      ReceivedFrom_Idno   ,         Cloth_Idno         ,        Entry_ID      ,        Party_Bill_No ,       Particulars      ,         Sl_No        ,           EndsCount_IdNo     , Sized_Beam,                Meters               ) " &
                                "           Values                       ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_CheckingRefNo.Text) & "', " & Str(Val(vOrdByNo)) & ",    @checkingdate , " & Str(Val(vDelv_ID)) & ", " & Str(Val(vRec_ID)) & ", " & Str(Val(vClo_IdNo)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(SNo)) & ", " & Str(Val(vEndct_IdNo)) & ",      0    , " & Str(Val(lbl_WarpConsumption.Text)) & " ) "
            cmd.ExecuteNonQuery()


            '-------Yarn Stock Posting
            cmd.CommandText = "Insert into Stock_Yarn_Processing_Details (      Reference_Code   ,                Company_IdNo      ,            Reference_No       ,           for_OrderBy     , Reference_Date,        DeliveryTo_Idno    ,       ReceivedFrom_Idno  ,        Entry_ID      ,       Particulars      ,      Party_Bill_No   , Sl_No,          Count_IdNo        , Yarn_Type, Mill_IdNo, Bags, Cones,               Weight                ) " &
                                "           Values                       ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_CheckingRefNo.Text) & "', " & Str(Val(vOrdByNo)) & ",    @checkingdate , " & Str(Val(vDelv_ID)) & ", " & Str(Val(vRec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "',   1  , " & Str(Val(WftCnt_ID)) & ",  'MILL'  ,     0    ,   0 ,   0  , " & Str(Val(lbl_WeftConsumption.Text)) & " ) "
            cmd.ExecuteNonQuery()


            '-------Cloth Stock Posting

            vStkOf_Pos_IdNo = 0
            If cbo_StockOff.Visible = True Then
                vStkOf_Pos_IdNo = StkOff_ID

            Else

                If Trim(UCase(Led_type)) = "JOBWORKER" Then
                    vStkOf_Pos_IdNo = vLed_IdNo
                Else
                    vStkOf_Pos_IdNo = Common_Procedures.CommonLedger.OwnSort_Ac  ' Val(Common_Procedures.CommonLedger.Godown_Ac)
                End If

            End If

            StkDelvTo_ID = 0 : StkRecFrm_ID = 0
            If Val(vLed_IdNo) = Val(vGod_ID) Then
                StkDelvTo_ID = Val(vGod_ID)
                StkRecFrm_ID = 0

            Else
                StkDelvTo_ID = Val(vGod_ID)
                StkRecFrm_ID = Val(vLed_IdNo)

            End If


            UC_Mtrs = 0
            If Val(vTotChkMtr) = 0 Then UC_Mtrs = vTotRcptMtr

            cmd.CommandText = "Insert into Stock_Cloth_Processing_Details (       Reference_Code   ,             Company_IdNo         ,             Reference_No      ,         for_OrderBy       , Reference_Date,             StockOff_IdNo        ,        DeliveryTo_Idno        ,       ReceivedFrom_Idno       ,         Entry_ID     ,      Party_Bill_No   ,      Particulars       , Sl_No,          Cloth_Idno        ,                  Folding           ,        UnChecked_Meters  ,      Meters_Type1            ,         Meters_Type2         ,          Meters_Type3         ,          Meters_Type4        ,        Meters_Type5           ) " &
                                "           Values                        ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_CheckingRefNo.Text) & "', " & Str(Val(vOrdByNo)) & ",   @checkingdate  , " & Str(Val(vStkOf_Pos_IdNo)) & ", " & Str(Val(StkDelvTo_ID)) & ", " & Str(Val(StkRecFrm_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',   1  , " & Str(Val(vClo_IdNo)) & ",  " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(UC_Mtrs)) & ", " & Str(Val(vTotTyp1Mtr)) & ", " & Str(Val(vTotTyp2Mtr)) & ",  " & Str(Val(vTotTyp3Mtr)) & ", " & Str(Val(vTotTyp4Mtr)) & ", " & Str(Val(vTotTyp5Mtr)) & " ) "
            cmd.ExecuteNonQuery()

            'If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Yarn_Stock) = 1 Then

            '    cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno      , Count_IdNo, Yarn_Type, Mill_IdNo ) " &
            '                              " Select                               'YARN'    , Reference_Code, Reference_Date, Company_IdNo, ReceivedFrom_Idno, Count_IdNo, Yarn_Type, Mill_IdNo from Stock_Yarn_Processing_Details where Reference_Code = '" & Trim(NewCode) & "'"
            '    cmd.ExecuteNonQuery()

            '    'If Common_Procedures.Check_is_Negative_Stock_Status(Con, tr) = True Then Exit Sub

            'End If


            'vErrMsg = ""
            'If Common_Procedures.Cross_Checking_PieceChecking_PackingSlip_Meters(Con, Trim(NewCode), vErrMsg, tr) = False Then
            '    Throw New ApplicationException(vErrMsg)
            '    Exit Sub
            'End If


            tr.Commit()

            MessageBox.Show("Saved Successfully", "FOR SAVING,.....", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_CheckingRefNo.Text)
                End If
            Else
                move_record(lbl_CheckingRefNo.Text)
            End If


        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            cmd.Dispose()
            tr.Dispose()

            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        End Try

    End Sub


    Private Sub Stock_Posting(ByVal NewCode As String, ByVal led_id As Integer, ByVal Clo_ID As Integer, ByVal LotCd As String, ByVal Lm_ID As Integer, ByVal WagesCode As String, ByVal vPCSCHKCode As String, ByVal vPCSCHKDate As String, ByVal vPCSCHKFOLDPERC As String, ByVal tr As SqlClient.SqlTransaction)
        Dim cmd As New SqlClient.SqlCommand
        Dim cmd3 As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer = 0, j As Integer = 0
        Dim Cons_Yarn As String = 0, Cons_Pavu As String = 0, BmConsMtrs As String = 0
        Dim RecMtrs As String, T1_Mtrs As String, T2_Mtrs As String, T3_Mtrs As String
        Dim T4_Mtrs As String, T5_Mtrs As String, UC_Mtrs As String

        Dim Tot_PcsMtr As String = 0, Tot_PcsWt As Single = 0, Wt_Mtr As Single = 0
        Dim SQL1 As String = ""
        Dim vWidType As String = ""
        Dim vENT_WidthType As String = ""




        cmd.Connection = Con

        cmd.Transaction = tr

        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@checkingdate", Convert.ToDateTime(msk_date.Text))
        cmd.Parameters.AddWithValue("@recdate", CDate(lbl_ReceiptDate.Text))


        RecMtrs = 0 : T1_Mtrs = 0 : T2_Mtrs = 0 : T3_Mtrs = 0 : T4_Mtrs = 0 : T5_Mtrs = 0
        UC_Mtrs = 0
        BmConsMtrs = 0


        cmd3.Connection = Con
        cmd3.Transaction = tr
        cmd3.CommandType = CommandType.StoredProcedure
        cmd3.CommandText = "sp_get_weaverclothreceiptpiecedetails_totalmeter_beamconsmeter"
        cmd3.Parameters.Clear()
        cmd3.Parameters.Add("@weaver_clothreceipt_code", SqlDbType.VarChar)

        If InStr(1, Trim(UCase(lbl_ReceiptCode.Text)), Trim(UCase(PkCondition_CLORCPT))) > 0 Then
            cmd3.Parameters("@weaver_clothreceipt_code").Value = Trim(lbl_ReceiptCode.Text)
        Else
            cmd3.Parameters("@weaver_clothreceipt_code").Value = Trim(PkCondition_CLORCPT) & Trim(lbl_ReceiptCode.Text)
        End If

        cmd3.Parameters.Add("@lot_code", SqlDbType.VarChar)
        cmd3.Parameters("@lot_code").Value = Trim(LotCd)
        Da = New SqlClient.SqlDataAdapter(cmd3)
        'SQL1 = "Select sum(Receipt_Meters) as RecMtrs, sum(Type1_Meters) as Type1Mtrs, sum(Type2_Meters) as Type2Mtrs, sum(Type3_Meters) as Type3Mtrs, sum(Type4_Meters) as Type4Mtrs, sum(Type5_Meters) as Type5Mtrs, sum(BeamConsumption_Meters) as BeamCons_Meters from Weaver_ClothReceipt_Piece_Details where Weaver_ClothReceipt_Code = '" & Trim(PkCondition_CLORCPT) & Trim(lbl_ReceiptCode.Text) & "' and Lot_Code = '" & Trim(LotCd) & "'"
        'cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
        'Da = New SqlClient.SqlDataAdapter(cmd)
        ''Da = New SqlClient.SqlDataAdapter("Select sum(Receipt_Meters) as RecMtrs, sum(Type1_Meters) as Type1Mtrs, sum(Type2_Meters) as Type2Mtrs, sum(Type3_Meters) as Type3Mtrs, sum(Type4_Meters) as Type4Mtrs, sum(Type5_Meters) as Type5Mtrs, sum(BeamConsumption_Meters) as BeamCons_Meters from Weaver_ClothReceipt_Piece_Details where Weaver_ClothReceipt_Code = '" & Trim(PkCondition_CLORCPT) & Trim(lbl_ReceiptCode.Text) & "' and Lot_Code = '" & Trim(LotCd) & "'", con)
        Da.SelectCommand.Transaction = tr
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("RecMtrs").ToString) = False Then
                RecMtrs = Format(Val(Dt1.Rows(0).Item("RecMtrs").ToString), "###########0.00")
            End If
            If IsDBNull(Dt1.Rows(0).Item("Type1Mtrs").ToString) = False Then
                T1_Mtrs = Format(Val(Dt1.Rows(0).Item("Type1Mtrs").ToString), "###########0.00")
            End If
            If IsDBNull(Dt1.Rows(0).Item("Type2Mtrs").ToString) = False Then
                T2_Mtrs = Format(Val(Dt1.Rows(0).Item("Type2Mtrs").ToString), "###########0.00")
            End If
            If IsDBNull(Dt1.Rows(0).Item("Type3Mtrs").ToString) = False Then
                T3_Mtrs = Format(Val(Dt1.Rows(0).Item("Type3Mtrs").ToString), "###########0.00")
            End If
            If IsDBNull(Dt1.Rows(0).Item("Type4Mtrs").ToString) = False Then
                T4_Mtrs = Format(Val(Dt1.Rows(0).Item("Type4Mtrs").ToString), "###########0.00")
            End If
            If IsDBNull(Dt1.Rows(0).Item("Type5Mtrs").ToString) = False Then
                T5_Mtrs = Format(Val(Dt1.Rows(0).Item("Type5Mtrs").ToString), "###########0.00")
            End If
            If IsDBNull(Dt1.Rows(0).Item("BeamCons_Meters").ToString) = False Then
                BmConsMtrs = Format(Val(Dt1.Rows(0).Item("BeamCons_Meters").ToString), "###########0.00")
            End If
        End If
        Dt1.Clear()


        cmd3.CommandType = CommandType.StoredProcedure
        cmd3.CommandText = "sp_save_weaverclothreceipthead_update_checking_details"
        cmd3.Parameters.Clear()
        cmd3.Parameters.Add("@weaver_clothreceipt_code", SqlDbType.VarChar)
        cmd3.Parameters("@weaver_clothreceipt_code").Value = Trim(LotCd)
        'cmd3.Parameters("@weaver_clothreceipt_code").Value = Trim(lbl_ReceiptCode.Text)
        cmd3.Parameters.Add("@weaver_piece_checking_code", SqlDbType.VarChar)
        cmd3.Parameters("@weaver_piece_checking_code").Value = Trim(vPCSCHKCode)

        cmd3.Parameters.Add("@weaver_piece_checking_increment", SqlDbType.VarChar)
        cmd3.Parameters.Add("@weaver_piece_checking_date", SqlDbType.DateTime)
        If Trim(vPCSCHKCode) = "" Then
            Dim vDAT As Date = #01/01/1900#
            cmd3.Parameters("@weaver_piece_checking_increment").Value = 0
            cmd3.Parameters("@weaver_piece_checking_date").Value = vDAT

        Else
            cmd3.Parameters("@weaver_piece_checking_increment").Value = 1
            cmd3.Parameters("@weaver_piece_checking_date").Value = Convert.ToDateTime(msk_date.Text)

        End If
        cmd3.Parameters.Add("@folding_checking", SqlDbType.Decimal)
        If Val(vPCSCHKFOLDPERC) = 0 Then
            vPCSCHKFOLDPERC = 100
        End If
        cmd3.Parameters("@folding_checking").Value = Val(vPCSCHKFOLDPERC)
        cmd3.Parameters.Add("@folding", SqlDbType.Decimal)
        cmd3.Parameters("@folding").Value = Val(vPCSCHKFOLDPERC)
        cmd3.Parameters.Add("@receiptmeters_checking", SqlDbType.Decimal)
        cmd3.Parameters("@receiptmeters_checking").Value = Val(RecMtrs)
        cmd3.Parameters.Add("@receipt_meters", SqlDbType.Decimal)
        cmd3.Parameters("@receipt_meters").Value = Val(RecMtrs)
        cmd3.Parameters.Add("@consumedyarn_checking", SqlDbType.Decimal)
        cmd3.Parameters("@consumedyarn_checking").Value = Val(Cons_Yarn)
        cmd3.Parameters.Add("@consumed_yarn", SqlDbType.Decimal)
        cmd3.Parameters("@consumed_yarn").Value = Val(Cons_Yarn)
        cmd3.Parameters.Add("@consumedpavu_checking", SqlDbType.Decimal)
        cmd3.Parameters("@consumedpavu_checking").Value = Val(Cons_Pavu)
        cmd3.Parameters.Add("@consumed_pavu", SqlDbType.Decimal)
        cmd3.Parameters("@consumed_pavu").Value = Val(Cons_Pavu)
        cmd3.Parameters.Add("@beamconsumption_checking", SqlDbType.Decimal)
        cmd3.Parameters("@beamconsumption_checking").Value = Val(BmConsMtrs)
        cmd3.Parameters.Add("@beamconsumption_meters", SqlDbType.Decimal)
        cmd3.Parameters("@beamconsumption_meters").Value = Val(BmConsMtrs)
        cmd3.Parameters.Add("@type1_checking_meters", SqlDbType.Decimal)
        cmd3.Parameters("@type1_checking_meters").Value = Val(T1_Mtrs)
        cmd3.Parameters.Add("@type2_checking_meters", SqlDbType.Decimal)
        cmd3.Parameters("@type2_checking_meters").Value = Val(T2_Mtrs)
        cmd3.Parameters.Add("@type3_checking_meters", SqlDbType.Decimal)
        cmd3.Parameters("@type3_checking_meters").Value = Val(T3_Mtrs)
        cmd3.Parameters.Add("@type4_checking_meters", SqlDbType.Decimal)
        cmd3.Parameters("@type4_checking_meters").Value = Val(T4_Mtrs)
        cmd3.Parameters.Add("@type5_checking_meters", SqlDbType.Decimal)
        cmd3.Parameters("@type5_checking_meters").Value = Val(T5_Mtrs)
        cmd3.Parameters.Add("@total_checking_meters", SqlDbType.Decimal)
        cmd3.Parameters("@total_checking_meters").Value = (Val(T1_Mtrs) + Val(T2_Mtrs) + Val(T3_Mtrs) + Val(T4_Mtrs) + Val(T5_Mtrs))
        cmd3.ExecuteNonQuery()

        'cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set " &
        '                    " Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "', Weaver_Piece_Checking_Increment = 1, Weaver_Piece_Checking_Date = @CheckingDate, " &
        '                    " Folding_Checking = " & Str(Val(txt_Folding.Text)) & ", Folding = " & Str(Val(txt_Folding.Text)) & ", " &
        '                    " ReceiptMeters_Checking = " & Str(Val(RecMtrs)) & ", Receipt_Meters = " & Str(Val(RecMtrs)) & ", " &
        '                    " ConsumedYarn_Checking = " & Str(Val(Cons_Yarn)) & ", Consumed_Yarn = " & Str(Val(Cons_Yarn)) & ", " &
        '                    " ConsumedPavu_Checking = " & Str(Val(Cons_Pavu)) & ", Consumed_Pavu = " & Str(Val(Cons_Pavu)) & ", " &
        '                    " BeamConsumption_Checking = " & Str(Val(BmConsMtrs)) & ", BeamConsumption_Meters = " & Str(Val(BmConsMtrs)) & ", " &
        '                    " Type1_Checking_Meters = " & Str(Val(T1_Mtrs)) & ", Type2_Checking_Meters = " & Str(Val(T2_Mtrs)) & ", Type3_Checking_Meters = " & Str(Val(T3_Mtrs)) & ", Type4_Checking_Meters = " & Str(Val(T4_Mtrs)) & ", Type5_Checking_Meters = " & Str(Val(T5_Mtrs)) & ", Total_Checking_Meters = " & Str(Val(T1_Mtrs) + Val(T2_Mtrs) + Val(T3_Mtrs) + Val(T4_Mtrs) + Val(T5_Mtrs)) & " " &
        '                    " Where Weaver_ClothReceipt_Code = '" & Trim(lbl_ReceiptCode.Text) & "'"
        'cmd.ExecuteNonQuery()


    End Sub
    Private Sub Cbo_LoomType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_LoomType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "", "", "", "")
    End Sub

    Private Sub Cbo_LoomType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_LoomType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, Cbo_LoomType, msk_date, lbl_WeaverName, "", "", "", "")
    End Sub

    Private Sub Cbo_LoomType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_LoomType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, Cbo_LoomType, lbl_WeaverName, "", "", "", "")
    End Sub



    Private Sub msk_date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        If e.KeyCode = 40 Then

            e.Handled = True : e.SuppressKeyPress = True
            If txt_PieceNo.Enabled Then
                txt_PieceNo.Focus()
            ElseIf txt_Folding.Enabled Then
                txt_Folding.Focus()
            ElseIf Cbo_LoomType.Enabled Then
                Cbo_LoomType.Focus()
            ElseIf lbl_WeaverName.Enabled Then
                lbl_WeaverName.Focus()
            Else
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.PCSNO)
                    dgv_Details.CurrentCell.Selected = True
                End If
            End If

        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.PCSNO)
                dgv_Details.CurrentCell.Selected = True

            Else
                txt_Folding.Focus()

            End If

        End If

        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_date.Text
            vmskSelStrt = msk_date.SelectionStart
        End If
    End Sub

    Private Sub msk_date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_date.Text = Date.Today
            msk_date.SelectionStart = 0
        End If
        If Asc(e.KeyChar) = 13 Then

            e.Handled = True
            If txt_PieceNo.Enabled Then
                txt_PieceNo.Focus()

            ElseIf txt_Folding.Enabled Then
                txt_Folding.Focus()

            ElseIf Cbo_LoomType.Enabled Then
                Cbo_LoomType.Focus()

            ElseIf lbl_WeaverName.Enabled Then
                lbl_WeaverName.Focus()

            Else

                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.PCSNO)
                    dgv_Details.CurrentCell.Selected = True
                End If

            End If

        End If

    End Sub

    Private Sub msk_date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyUp

        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        If e.KeyCode = 107 Then
            msk_date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_date.Text))
            msk_date.SelectionStart = 0
        End If
        If e.KeyCode = 109 Then
            msk_date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_date.Text))
            msk_date.SelectionStart = 0
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)
        End If
    End Sub

    Private Sub msk_date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_date.LostFocus
        If IsDate(msk_date.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_date.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_date.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_date.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_date.Text)) <= 2000 Then
                    dtp_Date.Value = Convert.ToDateTime(msk_date.Text)
                End If
            End If
        End If
    End Sub

    Private Sub btn_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        save_record()
    End Sub

    Private Sub btn_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close.Click
        Me.Close()
    End Sub

    Private Sub cbo_Filter_Cloth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Cloth.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_Cloth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Cloth.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Filter_Cloth, cbo_Filter_PartyName, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
        If e.KeyCode = 40 Then
            e.Handled = True
            btn_Filter_Show_Click(sender, e)
        End If
    End Sub

    Private Sub cbo_Filter_Cloth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Cloth.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Filter_Cloth, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            btn_Filter_Show_Click(sender, e)
        End If
    End Sub

    Private Sub cbo_Filter_Cloth_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Cloth.KeyUp
        If e.Control = False And e.KeyCode = 17 Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Filter_Cloth.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'WEAVER'  or Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1) and Close_status = 0", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Filter_PartyName, dtp_Filter_ToDate, txt_Filter_RecNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'WEAVER'  or Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1) and Close_status = 0", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Filter_PartyName, txt_Filter_RecNo, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'WEAVER'  or Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1) and Close_status = 0", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyUp
        If e.Control = False And e.KeyCode = 17 Then
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Filter_PartyName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub txt_Filter_RecNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Filter_RecNo.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub txt_Filter_RecNoTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Filter_RecNoTo.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim n As Integer, i As Integer
        Dim Led_ID As Integer = 0
        Dim Clo_ID As Integer = 0
        Dim Condt As String = ""


        Try
            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Weaver_ClothReceipt_Date between'" & Trim(Format(dtp_Filter_Fromdate.Value, "MM-dd-yyyy")) & "' AND '" & Trim(Format(dtp_Filter_ToDate.Value, "MM-dd-yyyy")) & "'"
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Weaver_ClothReceipt_Date between'" & Trim(Format(dtp_Filter_Fromdate.Value, "MM-dd-yyyy")) & "'"
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Weaver_ClothReceipt_Date between'" & Trim(Format(dtp_Filter_Fromdate.Value, "MM-dd-yyyy")) & "'"
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(Con, cbo_Filter_PartyName.Text)
            End If
            If Trim(cbo_Filter_Cloth.Text) <> "" Then
                Clo_ID = Common_Procedures.Cloth_NameToIdNo(Con, cbo_Filter_Cloth.Text)
            End If

            If Led_ID <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", "AND", "") & "(a.Ledger_IdNo =" & Str(Val(Led_ID)) & ")"
            End If
            If Clo_ID <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", "AND", "") & "(c.Cloth_IdNo =" & Str(Val(Clo_ID)) & ")"
            End If

            If Trim(txt_Filter_RecNo.Text) <> "" And Trim(txt_Filter_RecNoTo.Text) <> "" Then
                Condt = "a.Weaver_ClothReceipt_No between '" & Trim(txt_Filter_RecNo.Text) & "' and '" & Trim(txt_Filter_RecNoTo.Text) & "'"
                'ElseIf Trim(txt_Filter_RecNo.Text) = "" Then
                '    Condt = "a.Weaver_ClothReceipt_No between '" & Trim(txt_Filter_RecNo.Text) & "'"
                'ElseIf Trim(txt_Filter_RecNoTo.Text) = "" Then
                '    Condt = "a.Weaver_ClothReceipt_No between '" & Trim(txt_Filter_RecNoTo.Text) & "'"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as WeaverName , c.* , e.EndsCount_Name from Weaver_Cloth_Receipt_Head a INNER JOIN Ledger_Head b on a.Ledger_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Cloth_Head c ON c.Cloth_Idno = a.Cloth_IdNo LEFT OUTER JOIN EndsCount_Head e ON e.EndsCount_IdNo = a.EndsCount_IdNo WHERE a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_ClothReceipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order By a.for_orderby , a.Weaver_ClothReceipt_No", Con)
            dt = New DataTable
            da.Fill(dt)

            dgv_Filter_Details.Rows.Clear()

            If dt.Rows.Count > 0 Then

                For i = 0 To dt.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(dgvCol_Filter.LOTNO).Value = dt.Rows(i).Item("Weaver_ClothReceipt_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(dgvCol_Filter.LOTDATE).Value = Format(Convert.ToDateTime(dt.Rows(i).Item("Weaver_ClothReceipt_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(dgvCol_Filter.WEAVERNAME).Value = dt.Rows(i).Item("WeaverName").ToString
                    dgv_Filter_Details.Rows(n).Cells(dgvCol_Filter.CLOTHNAME).Value = dt.Rows(i).Item("Cloth_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(dgvCol_Filter.ENDSCOUNT).Value = dt.Rows(i).Item("EndsCount_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(dgvCol_Filter.WEFTCOUNT).Value = dt.Rows(i).Item("Consumed_Pavu").ToString
                    dgv_Filter_Details.Rows(n).Cells(dgvCol_Filter.TOTALMETERS).Value = dt.Rows(i).Item("Total_Meters").ToString
                    dgv_Filter_Details.Rows(n).Cells(dgvCol_Filter.CONSUMEDYARN).Value = dt.Rows(i).Item("Consumed_Yarn").ToString





                Next

            End If

            da.Dispose()
            dt.Dispose()
            dt.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
        If dgv_Filter_Details.Enabled And dgv_Filter_Details.Visible Then dgv_Filter_Details.Focus()
    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()
    End Sub

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown
        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_date.Focus()
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            msk_date.Focus()
        End If
    End Sub

    Private Sub dtp_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            msk_date.Focus()
        End If
    End Sub

    Private Sub dtp_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyUp
        If e.Control = False And e.KeyValue = 17 And Vcbo_KeyDownVal = e.KeyValue Then
            dtp_Date.Text = Date.Today
        End If
    End Sub

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        If IsDate(dtp_Date.Text) = True Then
            msk_date.Text = dtp_Date.Text
            msk_date.SelectionStart = 0
        End If
    End Sub

    Private Sub dgv_Filter_Details_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellDoubleClick
        Open_Filter_Entry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter_Details.KeyDown
        If e.KeyCode = 13 Then
            Open_Filter_Entry()
        End If
    End Sub

    Private Sub txt_Folding_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Folding.KeyDown
        If e.KeyCode = 38 Then e.Handled = True : SendKeys.Send("+{TAB}")
        If e.KeyCode = 40 Then

            e.Handled = True
            e.SuppressKeyPress = True

            If lbl_LoomNo.Visible = True And lbl_LoomNo.Enabled = True Then
                lbl_LoomNo.Focus()
            ElseIf cbo_WidthType.Visible = True And cbo_WidthType.Enabled = True Then
                cbo_WidthType.Focus()
            ElseIf txt_PieceNo.Visible = True And txt_PieceNo.Enabled = True Then
                txt_PieceNo.Focus()
            ElseIf cbo_StockOff.Visible = True And cbo_StockOff.Enabled = True Then
                cbo_StockOff.Focus()
            ElseIf lbl_Godown_StockIN.Visible = True And lbl_Godown_StockIN.Enabled = True Then
                lbl_Godown_StockIN.Focus()
            Else
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.PCSNO)
                    dgv_Details.CurrentCell.Selected = True
                Else
                    lbl_WeftConsumption.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub txt_Folding_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Folding.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If lbl_LoomNo.Visible = True And lbl_LoomNo.Enabled = True Then
                lbl_LoomNo.Focus()
            ElseIf cbo_WidthType.Visible = True And cbo_WidthType.Enabled = True Then
                cbo_WidthType.Focus()
            ElseIf txt_PieceNo.Visible = True And txt_PieceNo.Enabled = True Then
                txt_PieceNo.Focus()
            ElseIf cbo_StockOff.Visible = True And cbo_StockOff.Enabled = True Then
                cbo_StockOff.Focus()
            ElseIf lbl_Godown_StockIN.Visible = True And lbl_Godown_StockIN.Enabled = True Then
                lbl_Godown_StockIN.Focus()
            Else
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.PCSNO)
                    dgv_Details.CurrentCell.Selected = True
                Else
                    lbl_WeftConsumption.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub txt_PieceNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PieceNo.KeyDown
        If e.KeyCode = 38 Then e.Handled = True : msk_date.Focus()
        If e.KeyValue = 40 Then
            e.Handled = True
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(0)
                dgv_Details.CurrentCell.Selected = True
            ElseIf txt_Folding.Enabled And txt_Folding.Visible Then
                txt_Folding.Focus()
            Else
                msk_date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_PieceNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PieceNo.KeyPress
        If Common_Procedures.Accept_AlphaNumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then

            If Trim(txt_PieceNo.Text) <> "" Then

                If Trim(UCase(txt_PieceNo.Tag)) <> Trim(UCase(txt_PieceNo.Text)) Then
                    txt_PieceNo.Tag = txt_PieceNo.Text
                    Get_PieceDetails(txt_PieceNo.Text)
                End If

                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(0)
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)

                ElseIf txt_Folding.Enabled And txt_Folding.Visible Then
                    txt_Folding.Focus()

                Else
                    msk_date.Focus()

                End If

            End If

        End If

    End Sub

    Private Sub dgv_Details_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit

        With dgv_Details

            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

            If e.ColumnIndex = dgvCol_Details.RECEIPT_DOFF_MTRS Or e.ColumnIndex = dgvCol_Details.PCSTYPE1 Or e.ColumnIndex = dgvCol_Details.PCSTYPE2 Or e.ColumnIndex = dgvCol_Details.PCSTYPE3 Or e.ColumnIndex = dgvCol_Details.PCSTYPE4 Or e.ColumnIndex = dgvCol_Details.PCSTYPE5 Or e.ColumnIndex = dgvCol_Details.TOTALCHECKINGMTRS Or e.ColumnIndex = dgvCol_Details.EXCESS_SHORT_MTRS Or e.ColumnIndex = dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS Then

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then '---- Kalaimagal Textiles (Palladam)

                    .Rows(e.RowIndex).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value), "########0.0")
                    '.Rows(e.RowIndex).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value = Format(Val(dgvCol_Details.RECEIPT_DOFF_MTRS), "########0.0")
                    .Rows(e.RowIndex).Cells(dgvCol_Details.PCSTYPE1).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.PCSTYPE1).Value), "########0.0")
                    .Rows(e.RowIndex).Cells(dgvCol_Details.PCSTYPE2).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.PCSTYPE2).Value), "########0.0")
                    .Rows(e.RowIndex).Cells(dgvCol_Details.PCSTYPE3).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.PCSTYPE3).Value), "########0.0")
                    .Rows(e.RowIndex).Cells(dgvCol_Details.PCSTYPE4).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.PCSTYPE4).Value), "########0.0")
                    .Rows(e.RowIndex).Cells(dgvCol_Details.PCSTYPE5).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.PCSTYPE5).Value), "########0.0")
                    .Rows(e.RowIndex).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value), "########0.0")
                    .Rows(e.RowIndex).Cells(dgvCol_Details.EXCESS_SHORT_MTRS).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.EXCESS_SHORT_MTRS).Value), "########0.0")
                    .Rows(e.RowIndex).Cells(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Value), "########0.0")

                    .Rows(e.RowIndex).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value), "########0.00")
                    .Rows(e.RowIndex).Cells(dgvCol_Details.PCSTYPE1).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.PCSTYPE1).Value), "########0.00")
                    .Rows(e.RowIndex).Cells(dgvCol_Details.PCSTYPE2).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.PCSTYPE2).Value), "########0.00")
                    .Rows(e.RowIndex).Cells(dgvCol_Details.PCSTYPE3).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.PCSTYPE3).Value), "########0.00")
                    .Rows(e.RowIndex).Cells(dgvCol_Details.PCSTYPE4).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.PCSTYPE4).Value), "########0.00")
                    .Rows(e.RowIndex).Cells(dgvCol_Details.PCSTYPE5).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.PCSTYPE5).Value), "########0.00")
                    .Rows(e.RowIndex).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value), "########0.00")
                    .Rows(e.RowIndex).Cells(dgvCol_Details.EXCESS_SHORT_MTRS).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.EXCESS_SHORT_MTRS).Value), "########0.00")
                    .Rows(e.RowIndex).Cells(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Value), "########0.00")


                Else

                    .Rows(e.RowIndex).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value), "########0.00")
                    .Rows(e.RowIndex).Cells(dgvCol_Details.PCSTYPE1).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.PCSTYPE1).Value), "########0.00")
                    .Rows(e.RowIndex).Cells(dgvCol_Details.PCSTYPE2).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.PCSTYPE2).Value), "########0.00")
                    .Rows(e.RowIndex).Cells(dgvCol_Details.PCSTYPE3).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.PCSTYPE3).Value), "########0.00")
                    .Rows(e.RowIndex).Cells(dgvCol_Details.PCSTYPE4).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.PCSTYPE4).Value), "########0.00")
                    .Rows(e.RowIndex).Cells(dgvCol_Details.PCSTYPE5).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.PCSTYPE5).Value), "########0.00")
                    .Rows(e.RowIndex).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value), "########0.00")
                    .Rows(e.RowIndex).Cells(dgvCol_Details.EXCESS_SHORT_MTRS).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.EXCESS_SHORT_MTRS).Value), "########0.00")
                    .Rows(e.RowIndex).Cells(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Value), "########0.00")


                End If

            ElseIf e.ColumnIndex = dgvCol_Details.WEIGHT Then
                .Rows(e.RowIndex).Cells(dgvCol_Details.WEIGHT).Value = Format(Val(.Rows(e.RowIndex).Cells(dgvCol_Details.WEIGHT).Value), "########0.000")

            End If

        End With

    End Sub
    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter

        With dgv_Details
            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
            dgv_ActCtrlName = .Name
            If Val(.Rows(e.RowIndex).Cells(dgvCol_Details.SNO).Value) = 0 Then
                .Rows(e.RowIndex).Cells(dgvCol_Details.SNO).Value = e.RowIndex + 1
            End If
        End With

    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Dim TtMrs_in_100Fld As String = 0
        Dim vFldPerc As String = 0

        Try

            If FrmLdSts = True Then Exit Sub

            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

            With dgv_Details

                If .Visible Then

                    If e.ColumnIndex = dgvCol_Details.RECEIPT_DOFF_MTRS Or e.ColumnIndex = dgvCol_Details.PCSTYPE1 Or e.ColumnIndex = dgvCol_Details.PCSTYPE2 Or e.ColumnIndex = dgvCol_Details.PCSTYPE3 Or e.ColumnIndex = dgvCol_Details.PCSTYPE4 Or e.ColumnIndex = dgvCol_Details.PCSTYPE5 Or e.ColumnIndex = dgvCol_Details.WEIGHT Then

                        If .Columns(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Visible = True Then
                            .Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Value = Format(Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value) + Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.EXCESS_SHORT_MTRS).Value), "#########0.00")

                            .Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.PCSTYPE1).Value = Format(Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Value), "#########0.00")
                            .Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value = Format(Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Value), "#########0.00")

                        Else

                            .Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value = Format(Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.PCSTYPE1).Value) + Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.PCSTYPE2).Value) + Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.PCSTYPE3).Value) + Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.PCSTYPE4).Value) + Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.PCSTYPE5).Value), "##########0.00")
                            .Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Value = Format(Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value) + Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value), "#########0.00")

                        End If


                        vFldPerc = Val(txt_Folding.Text)
                        If Val(vFldPerc) = 0 Then vFldPerc = 100
                        TtMrs_in_100Fld = Format(Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value) * Val(vFldPerc) / 100, "########0.00")

                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1428" Then '---- SAKTHI VINAYAGA TEXTILES  (ERODE-PALLIPALAYAM)
                            If .Columns(dgvCol_Details.EXCESS_SHORT_MTRS).ReadOnly = True Then
                                .Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.EXCESS_SHORT_MTRS).Value = Format(Val(TtMrs_in_100Fld) - Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value), "#########0.00")
                            End If
                        End If

                        .Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.WEIGHTPERMETER).Value = Format(Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.WEIGHT).Value) / Val(TtMrs_in_100Fld), "##########0.000")

                        Calculation_TotalMeter()

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
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim i As Integer = 0
        Dim n As Integer = 0

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                n = .CurrentRow.Index

                If Trim(.Rows(n).Cells(dgvCol_Details.PACKINGSLIPCODETYPE1).Value) = "" And Trim(.Rows(n).Cells(dgvCol_Details.PACKINGSLIPCODETYPE2).Value) = "" And Trim(.Rows(n).Cells(dgvCol_Details.PACKINGSLIPCODETYPE3).Value) = "" And Trim(.Rows(n).Cells(dgvCol_Details.PACKINGSLIPCODETYPE4).Value) = "" And Trim(.Rows(n).Cells(dgvCol_Details.PACKINGSLIPCODETYPE5).Value) = "" Then

                    If .CurrentCell.RowIndex = .Rows.Count - 1 Then

                        For i = 0 To .Columns.Count - 1
                            .Rows(n).Cells(i).Value = ""
                        Next

                    Else
                        .Rows.RemoveAt(n)

                    End If

                    Calculation_TotalMeter()

                End If


            End With

        End If

    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Try
            With dgv_Details
                .Rows(e.RowIndex).Cells(dgvCol_Details.SNO).Value = e.RowIndex + 1
            End With

        Catch ex As Exception
            '---
        End Try
    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next

        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
        dgv_ActCtrlName = dgv_Details.Name
    End Sub


    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        With dgv_Details
            If .Visible Then
                If dgv_Details.CurrentCell.ColumnIndex = dgvCol_Details.PCSNO Then
                    If Common_Procedures.Accept_AlphaNumeric_WithOutSpecialCharacters_Only(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If
                Else
                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If
                End If
            End If
        End With
    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        dgv_Details_KeyUp(sender, e)
    End Sub

    Private Sub Calculation_TotalMeter()
        Dim sno As Integer = 0
        Dim ReceiptMtr As String = 0
        Dim tot_sound_Mtr As String = 0, tot_sec_Mtr As String = 0, tot_bit_Mtr As String = 0, tot_rej_Mtr As String = 0, tot_othr_Mtr As String = 0
        Dim total_Mtrs As String = 0
        Dim Excess_shot_Mtr As String = 0
        Dim wgt_mtr As String = 0, tot_weight As String = 0
        Dim vFldPerc As String = 0
        Dim vEXCSHT_MTR As String = 0

        If FrmLdSts = True Then Exit Sub

        vFldPerc = Val(txt_Folding.Text)
        If Val(vFldPerc) = 0 Then vFldPerc = 100

        ReceiptMtr = 0 : total_Mtrs = 0 : Excess_shot_Mtr = 0 : wgt_mtr = 0 : tot_weight = 0

        With dgv_Details


            For i = 0 To .RowCount - 1

                sno = sno + 1

                .Rows(i).Cells(dgvCol_Details.SNO).Value = sno


                If Trim(.Rows(i).Cells(dgvCol_Details.PCSNO).Value) <> "" Or Val(.Rows(i).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.PCSTYPE1).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.PCSTYPE2).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.PCSTYPE3).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.PCSTYPE4).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.PCSTYPE5).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.EXCESS_SHORT_MTRS).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.WEIGHT).Value) <> 0 Then

                    ReceiptMtr = Format(Val(ReceiptMtr) + Val(.Rows(i).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value), "########0.00")

                    tot_sound_Mtr = Format(Val(tot_sound_Mtr) + Val(.Rows(i).Cells(dgvCol_Details.PCSTYPE1).Value), "########0.00")
                    tot_sec_Mtr = Format(Val(tot_sec_Mtr) + Val(.Rows(i).Cells(dgvCol_Details.PCSTYPE2).Value), "########0.00")
                    tot_bit_Mtr = Format(Val(tot_bit_Mtr) + Val(.Rows(i).Cells(dgvCol_Details.PCSTYPE3).Value), "########0.00")
                    tot_rej_Mtr = Format(Val(tot_rej_Mtr) + Val(.Rows(i).Cells(dgvCol_Details.PCSTYPE4).Value), "########0.00")
                    tot_othr_Mtr = Format(Val(tot_othr_Mtr) + Val(.Rows(i).Cells(dgvCol_Details.PCSTYPE5).Value), "########0.00")

                    total_Mtrs = Format(Val(total_Mtrs) + Val(.Rows(i).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value), "########0.00")

                    Excess_shot_Mtr = Format(Val(Excess_shot_Mtr) + Val(.Rows(i).Cells(dgvCol_Details.EXCESS_SHORT_MTRS).Value), "########0.00")

                    tot_weight = Format(Val(tot_weight) + Val(.Rows(i).Cells(dgvCol_Details.WEIGHT).Value), "########0.000")

                End If

            Next

            vEXCSHT_MTR = 0
            If Trim(Common_Procedures.settings.CustomerCode) = "1428" Then
                vEXCSHT_MTR = Format(Val(total_Mtrs) - Val(lbl_ReceiptMeters.Text), "##########0.00")
                For i = 0 To .RowCount - 1
                    .Rows(i).Cells(dgvCol_Details.EXCESS_SHORT_MTRS).Value = ""
                Next i
                If .Rows.Count > 0 Then
                    .Rows(0).Cells(dgvCol_Details.EXCESS_SHORT_MTRS).Value = Format(Val(vEXCSHT_MTR), "########0.00")
                End If
            End If

        End With





        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then '---- Kalaimagal Textiles (Palladam)

                .Rows(0).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value = ""
                .Rows(0).Cells(dgvCol_Details.PCSTYPE1).Value = Format(Val(tot_sound_Mtr), "########0.0")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE2).Value = Format(Val(tot_sec_Mtr), "########0.0")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE3).Value = Format(Val(tot_bit_Mtr), "########0.0")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE4).Value = Format(Val(tot_rej_Mtr), "########0.0")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE5).Value = Format(Val(tot_othr_Mtr), "########0.0")
                .Rows(0).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value = Format(Val(total_Mtrs), "########0.0")
                .Rows(0).Cells(dgvCol_Details.EXCESS_SHORT_MTRS).Value = Format(Val(vEXCSHT_MTR), "########0.00")
                .Rows(0).Cells(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Value = "" ' Format(Val(total_Mtrs), "########0.0")
                .Rows(0).Cells(dgvCol_Details.WEIGHT).Value = Format(Val(tot_weight), "########0.000")

                .Rows(0).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value = ""
                .Rows(0).Cells(dgvCol_Details.PCSTYPE1).Value = Format(Val(.Rows(0).Cells(dgvCol_Details.PCSTYPE1).Value), "########0.00")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE2).Value = Format(Val(.Rows(0).Cells(dgvCol_Details.PCSTYPE2).Value), "########0.00")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE3).Value = Format(Val(.Rows(0).Cells(dgvCol_Details.PCSTYPE3).Value), "########0.00")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE4).Value = Format(Val(.Rows(0).Cells(dgvCol_Details.PCSTYPE4).Value), "########0.00")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE5).Value = Format(Val(.Rows(0).Cells(dgvCol_Details.PCSTYPE5).Value), "########0.00")
                .Rows(0).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value = Format(Val(.Rows(0).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value), "########0.00")
                .Rows(0).Cells(dgvCol_Details.EXCESS_SHORT_MTRS).Value = Format(Val(vEXCSHT_MTR), "########0.00")
                .Rows(0).Cells(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Value = "" ' Format(Val(.Rows(0).Cells(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Value), "########0.00")
                .Rows(0).Cells(dgvCol_Details.WEIGHT).Value = Format(Val(.Rows(0).Cells(dgvCol_Details.WEIGHT).Value), "########0.000")

            Else

                .Rows(0).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value = ""
                .Rows(0).Cells(dgvCol_Details.PCSTYPE1).Value = Format(Val(tot_sound_Mtr), "########0.00")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE2).Value = Format(Val(tot_sec_Mtr), "########0.00")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE3).Value = Format(Val(tot_bit_Mtr), "########0.00")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE4).Value = Format(Val(tot_rej_Mtr), "########0.00")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE5).Value = Format(Val(tot_othr_Mtr), "########0.00")
                .Rows(0).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value = Format(Val(total_Mtrs), "########0.00")
                .Rows(0).Cells(dgvCol_Details.EXCESS_SHORT_MTRS).Value = Format(Val(vEXCSHT_MTR), "########0.00")
                .Rows(0).Cells(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Value = "" ' Format(Val(total_Mtrs), "########0.00")
                .Rows(0).Cells(dgvCol_Details.WEIGHT).Value = Format(Val(tot_weight), "########0.000")

            End If

            .Rows(0).Cells(dgvCol_Details.WEIGHTPERMETER).Value = ""
        End With

        With dgv_Details_Total2
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(dgvCol_Details.SNO).Value = "100%"
            .Rows(0).Cells(dgvCol_Details.PCSNO).Value = "FOLDING"


            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then '---- Kalaimagal Textiles (Palladam)
                .Rows(0).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value = Format(Val(ReceiptMtr), "########0.00")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE1).Value = Format(Val(tot_sound_Mtr) * Val(vFldPerc) / 100, "########0.0")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE2).Value = Format(Val(tot_sec_Mtr) * Val(vFldPerc) / 100, "########0.0")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE3).Value = Format(Val(tot_bit_Mtr) * Val(vFldPerc) / 100, "########0.0")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE4).Value = Format(Val(tot_rej_Mtr) * Val(vFldPerc) / 100, "########0.0")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE5).Value = Format(Val(tot_othr_Mtr) * Val(vFldPerc) / 100, "########0.0")
                .Rows(0).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value = Format(Val(.Rows(0).Cells(dgvCol_Details.PCSTYPE1).Value) + Val(.Rows(0).Cells(dgvCol_Details.PCSTYPE2).Value) + Val(.Rows(0).Cells(dgvCol_Details.PCSTYPE3).Value) + Val(.Rows(0).Cells(dgvCol_Details.PCSTYPE4).Value) + Val(.Rows(0).Cells(dgvCol_Details.PCSTYPE5).Value), "########0.0")
                .Rows(0).Cells(dgvCol_Details.EXCESS_SHORT_MTRS).Value = Format(Val(Excess_shot_Mtr), "########0.0")
                .Rows(0).Cells(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Value = Format(Val(.Rows(0).Cells(dgvCol_Details.PCSTYPE1).Value) + Val(.Rows(0).Cells(dgvCol_Details.PCSTYPE2).Value) + Val(.Rows(0).Cells(dgvCol_Details.PCSTYPE3).Value) + Val(.Rows(0).Cells(dgvCol_Details.PCSTYPE4).Value) + Val(.Rows(0).Cells(dgvCol_Details.PCSTYPE5).Value), "########0.0")

                .Rows(0).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value = Format(Val(.Rows(0).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value), "########0.00")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE1).Value = Format(Val(.Rows(0).Cells(dgvCol_Details.PCSTYPE1).Value), "########0.00")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE2).Value = Format(Val(.Rows(0).Cells(dgvCol_Details.PCSTYPE2).Value), "########0.00")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE3).Value = Format(Val(.Rows(0).Cells(dgvCol_Details.PCSTYPE3).Value), "########0.00")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE4).Value = Format(Val(.Rows(0).Cells(dgvCol_Details.PCSTYPE4).Value), "########0.00")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE5).Value = Format(Val(.Rows(0).Cells(dgvCol_Details.PCSTYPE5).Value), "########0.00")
                .Rows(0).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value = Format(Val(.Rows(0).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value), "########0.00")
                .Rows(0).Cells(dgvCol_Details.EXCESS_SHORT_MTRS).Value = Format(Val(.Rows(0).Cells(dgvCol_Details.EXCESS_SHORT_MTRS).Value), "########0.00")
                .Rows(0).Cells(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Value = Format(Val(.Rows(0).Cells(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Value), "########0.00")

            Else

                .Rows(0).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value = Format(Val(ReceiptMtr), "########0.00")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE1).Value = Format(Val(tot_sound_Mtr) * Val(vFldPerc) / 100, "########0.00")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE2).Value = Format(Val(tot_sec_Mtr) * Val(vFldPerc) / 100, "########0.00")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE3).Value = Format(Val(tot_bit_Mtr) * Val(vFldPerc) / 100, "########0.00")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE4).Value = Format(Val(tot_rej_Mtr) * Val(vFldPerc) / 100, "########0.00")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE5).Value = Format(Val(tot_othr_Mtr) * Val(vFldPerc) / 100, "########0.00")
                .Rows(0).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value = Format(Val(.Rows(0).Cells(dgvCol_Details.PCSTYPE1).Value) + Val(.Rows(0).Cells(dgvCol_Details.PCSTYPE2).Value) + Val(.Rows(0).Cells(dgvCol_Details.PCSTYPE3).Value) + Val(.Rows(0).Cells(dgvCol_Details.PCSTYPE4).Value) + Val(.Rows(0).Cells(dgvCol_Details.PCSTYPE5).Value), "########0.00")
                '.Rows(0).Cells(dgvCol_Details.EXCESS_SHORT_MTRS).Value = Format(Val(Excess_shot_Mtr), "########0.00")
                '.Rows(0).Cells(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Value = Format(Val(.Rows(0).Cells(dgvCol_Details.PCSTYPE1).Value) + Val(.Rows(0).Cells(dgvCol_Details.PCSTYPE2).Value) + Val(.Rows(0).Cells(dgvCol_Details.PCSTYPE3).Value) + Val(.Rows(0).Cells(dgvCol_Details.PCSTYPE4).Value) + Val(.Rows(0).Cells(dgvCol_Details.PCSTYPE5).Value), "########0.00")

            End If

            .Rows(0).Cells(dgvCol_Details.WEIGHT).Value = ""
            .Rows(0).Cells(dgvCol_Details.WEIGHTPERMETER).Value = ""

        End With

        'Calculation_Pavu_Consumed()
        'Calculation_Yarn_Consumed()

    End Sub



    Private Sub cbo_WidthType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_WidthType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "", "", "", "")

    End Sub

    Private Sub cbo_WidthType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_WidthType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_WidthType, lbl_LoomNo, Nothing, "", "", "", "")
        If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            e.Handled = True
            If cbo_StockOff.Visible And cbo_StockOff.Enabled Then
                cbo_StockOff.Focus()
            ElseIf lbl_Godown_StockIN.Visible And lbl_Godown_StockIN.Enabled Then
                lbl_Godown_StockIN.Focus()
            ElseIf dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.PCSNO)
                dgv_Details.CurrentCell.Selected = True
            Else
                lbl_WeftConsumption.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_WidthType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_WidthType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_WidthType, Nothing, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            If cbo_StockOff.Visible And cbo_StockOff.Enabled Then
                cbo_StockOff.Focus()
            ElseIf lbl_Godown_StockIN.Visible And lbl_Godown_StockIN.Enabled Then
                lbl_Godown_StockIN.Focus()
            ElseIf dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.PCSNO)
                dgv_Details.CurrentCell.Selected = True
            Else
                lbl_WeftConsumption.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_StockOff_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_StockOff.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1) and Close_status = 0 ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_StockOff_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_StockOff.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_StockOff, Nothing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN'  or Ledger_Type = 'JOBWORKER'  or Show_In_All_Entry = 1 ) and Close_status = 0 ", "(Ledger_IdNo = 0)")

        If (e.KeyValue = 38 And cbo_StockOff.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then e.Handled = True : SendKeys.Send("+{TAB}")

        If (e.KeyValue = 40 And cbo_StockOff.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            e.Handled = True
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.PCSNO)
                dgv_Details.CurrentCell.Selected = True
            Else
                lbl_WeftConsumption.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_StockOff_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_StockOff.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_StockOff, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN'  or Ledger_Type = 'JOBWORKER'  or Show_In_All_Entry = 1 ) and Close_status = 0 ", "(Ledger_IdNo = 0)")

        If Common_Procedures.Accept_AlphaNumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.PCSNO)
                dgv_Details.CurrentCell.Selected = True
            Else
                lbl_WeftConsumption.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_StockOff_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_StockOff.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = "WEAVER"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_StockOff.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub


    Private Sub dgtxt_Details_TextChanged(sender As Object, e As System.EventArgs) Handles dgtxt_Details.TextChanged
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

    Private Sub txt_Folding_TextChanged(sender As Object, e As System.EventArgs) Handles txt_Folding.TextChanged
        Calculation_TotalMeter()
    End Sub

    Private Sub Calculation_Pavu_Consumed()
        Dim CloID As Integer = 0
        Dim ConsPavu As String = 0
        Dim LmID As Integer = 0
        Dim vWdthType As String = 0
        Dim NoofBeams As Integer = 0
        Dim vTotRcptMtr As String = 0
        Dim vTot_ChkMtrs As String = 0
        Dim vMtrs As String = 0

        'If Common_Procedures.settings.JobWorker_Pavu_Yarn_Stock_Posting_IN_Production = 0 Then
        '    txt_ConsPavu.Text = ""
        '    Exit Sub
        'End If


        vTotRcptMtr = 0 : vTot_ChkMtrs = 0
        If dgv_Details_Total2.RowCount > 0 Then
            vTotRcptMtr = dgv_Details_Total2.Rows(0).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value
            vTot_ChkMtrs = dgv_Details_Total2.Rows(0).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value
        End If

        LmID = 0
        vWdthType = ""
        If Trim(UCase(Cbo_LoomType.Text)) = "AUTOLOOM" Or Trim(UCase(Cbo_LoomType.Text)) = "AUTO LOOM" Then

            CloID = Common_Procedures.Cloth_NameToIdNo(Con, lbl_ClothName.Text)
            LmID = Common_Procedures.Loom_NameToIdNo(Con, lbl_LoomNo.Text)
            vWdthType = Trim(cbo_WidthType.Text)

            vMtrs = Val(vTot_ChkMtrs)
            If Val(vMtrs) = 0 Then vMtrs = Val(vTotRcptMtr)

            ConsPavu = Common_Procedures.get_Pavu_Consumption(Con, CloID, LmID, Val(vMtrs), vWdthType)

        Else

            ConsPavu = vTotRcptMtr
            If Val(ConsPavu) = 0 Then ConsPavu = Val(vTot_ChkMtrs)

        End If

        lbl_WarpConsumption.Text = Format(Val(ConsPavu), "###########0.00")

    End Sub

    Private Sub Calculation_Yarn_Consumed()
        Dim CloID As Integer
        Dim ConsYarn As Single
        Dim vTotRcptMtr As String = 0
        Dim vTot_ChkMtrs As String = 0
        Dim vMtrs As String = 0
        Dim vFldg As String = 0

        'If Common_Procedures.settings.JobWorker_Pavu_Yarn_Stock_Posting_IN_Production = 0 Then
        '    txt_ConsYarn.Text = ""
        '    Exit Sub
        'End If


        vTotRcptMtr = 0 : vTot_ChkMtrs = 0
        If dgv_Details_Total2.RowCount > 0 Then
            vTotRcptMtr = dgv_Details_Total2.Rows(0).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value
            vTot_ChkMtrs = dgv_Details_Total2.Rows(0).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value
        End If

        CloID = Common_Procedures.Cloth_NameToIdNo(Con, lbl_ClothName.Text)

        'If Trim(UCase(Cbo_LoomType.Text)) = "AUTOLOOM" Or Trim(UCase(Cbo_LoomType.Text)) = "AUTO LOOM" Then
        vMtrs = Val(vTot_ChkMtrs)
        If Val(vMtrs) = 0 Then vMtrs = Val(vTotRcptMtr)
        'Else
        '    vMtrs = Val(vTotRcptMtr)

        'End If

        ConsYarn = Common_Procedures.get_Weft_ConsumedYarn(Con, CloID, Val(vMtrs))

        lbl_WeftConsumption.Text = Format(ConsYarn, "#########0.000")

    End Sub

    Private Sub Cbo_LoomType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_LoomType.TextChanged

        If Trim(UCase(Cbo_LoomType.Text)) = "POWERLOOM" Then '---- Ashoka Textile (63.Velampalayam - Palladam)
            lbl_LoomNo.Enabled = False
            cbo_WidthType.Enabled = False

        Else

            lbl_LoomNo.Enabled = True
            cbo_WidthType.Enabled = True

        End If

        'Calculation_Pavu_Consumed()
        'Calculation_Yarn_Consumed()

    End Sub

    Private Sub btn_CalculateConsumption_Click(sender As Object, e As EventArgs) Handles btn_CalculateConsumption.Click
        Calculation_Pavu_Consumed()
        Calculation_Yarn_Consumed()
    End Sub

    Private Sub btn_Selection_Click(sender As Object, e As EventArgs) Handles btn_Selection.Click
        Get_PieceDetails(txt_PieceNo.Text)
        If dgv_Details.Rows.Count > 0 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
        Else
            If txt_Folding.Enabled Then
                txt_Folding.Focus()
            Else
                txt_PieceNo.Focus()
            End If
        End If
    End Sub

    Public Sub Get_PieceDetails(ByVal vPCSNO As String)
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim vPCSCD As String = ""
        Dim ChkNo As String = ""
        Dim n As Integer = 0
        Dim ChkDate As Date
        Dim InsEntry As Boolean = False
        Dim LmID As Integer = 0

        If Val(vPCSNO) = 0 And Trim(vPCSNO) = "0" Then
            MessageBox.Show("Invalid Piece No", "DOES NOT SHOW LOT DETAILS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_PieceNo.Enabled And txt_PieceNo.Visible Then txt_PieceNo.Focus()
            Exit Sub
        End If

        vPCSCD = vPCSNO
        If Not (Trim(vPCSNO) Like "*/??-??/*") Then
            vPCSCD = Trim(vPCSCD) & "/" & Trim(Common_Procedures.FnYearCode)
            vPCSCD = Trim(vPCSCD) & "/" & Trim(Val(lbl_Company.Tag))
            vPCSCD = Trim(vPCSCD) & "/" & Trim(PkCondition_CLORCPT)
        End If

        Da = New SqlClient.SqlDataAdapter("Select Weaver_Piece_Checking_No from Weaver_ClothReceipt_Piece_Details where Weaver_ClothReceipt_Code LIKE '" & Trim(PkCondition_CLORCPT) & "%' and PieceCode_for_Selection = '" & Trim(vPCSCD) & "' and Weaver_Piece_Checking_Code <> ''", Con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            Call move_record(Dt1.Rows(0).Item("Weaver_Piece_Checking_No").ToString)

        Else

            InsEntry = Insert_Entry
            ChkNo = Trim(lbl_CheckingRefNo.Text)
            ChkDate = dtp_Date.Value

            new_record()

            Insert_Entry = InsEntry
            lbl_CheckingRefNo.Text = ChkNo
            dtp_Date.Text = ChkDate
            txt_PieceNo.Text = Trim(vPCSNO)

            Da = New SqlClient.SqlDataAdapter("select a.*, b.Party_DcNo, tW.ledger_name, tQ.cloth_name, tQ.Crimp_Percentage as CrimpPercentage_from_master, tE.EndsCount_Name, tC.Count_Name from Weaver_ClothReceipt_Piece_Details a, Weaver_Cloth_Receipt_Head b, ledger_head tW, cloth_head tQ, EndsCount_Head tE, Count_Head tC where a.Weaver_ClothReceipt_Code LIKE '" & Trim(PkCondition_CLORCPT) & "%' and a.PieceCode_for_Selection = '" & Trim(vPCSCD) & "' and (a.Weaver_ClothReceipt_Code = b.Weaver_ClothReceipt_Code or a.Weaver_ClothReceipt_Code = '" & Trim(PkCondition_CLORCPT) & "' + b.Weaver_ClothReceipt_Code) and a.ledger_idno = tW.ledger_idno and a.cloth_idno = tQ.cloth_idno and a.EndsCount_IdNo = tE.EndsCount_IdNo and a.Count_IdNo = tC.Count_IdNo", Con)
            'Da = New SqlClient.SqlDataAdapter("select a.*, b.Party_DcNo, tW.ledger_name, tQ.cloth_name, tQ.Crimp_Percentage as CrimpPercentage_from_master, tE.EndsCount_Name, tC.Count_Name from Weaver_ClothReceipt_Piece_Details a, Weaver_Cloth_Receipt_Head b, ledger_head tW, cloth_head tQ, EndsCount_Head tE, Count_Head tC where a.Weaver_ClothReceipt_Code LIKE '" & Trim(PkCondition_CLORCPT) & "%' and a.PieceCode_for_Selection = '" & Trim(vPCSCD) & "' and a.Weaver_ClothReceipt_Code = '" & Trim(PkCondition_CLORCPT) & "' + b.Weaver_ClothReceipt_Code and a.ledger_idno = tW.ledger_idno and a.cloth_idno = tQ.cloth_idno and b.EndsCount_IdNo = tE.EndsCount_IdNo and b.Count_IdNo = tC.Count_IdNo", Con)
            Dt2 = New DataTable
            Da.Fill(Dt2)
            If Dt2.Rows.Count > 0 Then
                If IsDBNull(Dt2.Rows(0).Item("Weaver_Piece_Checking_Date").ToString) = False Then
                    If IsDate(Dt2.Rows(0).Item("Weaver_Piece_Checking_Date").ToString) = True Then
                        dtp_Date.Text = Dt2.Rows(0).Item("Weaver_Piece_Checking_Date").ToString
                    End If
                End If

                lbl_ReceiptCode.Text = Dt2.Rows(0).Item("Weaver_ClothReceipt_Code").ToString
                lbl_ReceiptNo.Text = Dt2.Rows(0).Item("Weaver_ClothReceipt_No").ToString
                lbl_ReceiptDate.Text = Dt2.Rows(0).Item("Weaver_ClothReceipt_Date").ToString
                lbl_PartyLotNo.Text = Dt2.Rows(0).Item("Party_DcNo").ToString
                lbl_WeaverName.Text = Dt2.Rows(0).Item("ledger_name").ToString
                lbl_ClothName.Text = Dt2.Rows(0).Item("cloth_name").ToString
                lbl_EndsCount.Text = Dt2.Rows(0).Item("EndsCount_Name").ToString
                lbl_WeftCount.Text = Dt2.Rows(0).Item("Count_Name").ToString
                lbl_LoomNo.Text = Dt2.Rows(0).Item("loom_no").ToString
                lbl_ReceiptMeters.Text = Dt2.Rows(0).Item("Receipt_Meters").ToString
                If Val(Dt2.Rows(0).Item("Crimp_Percentage").ToString) <> 0 Then
                    txt_Crimp.Text = Val(Dt2.Rows(0).Item("Crimp_Percentage").ToString)

                Else
                    txt_Crimp.Text = Val(Dt2.Rows(0).Item("CrimpPercentage_from_master").ToString)

                End If

                txt_Folding.Text = Val(Dt2.Rows(0).Item("Folding").ToString)
                If Val(txt_Folding.Text) = 0 Then txt_Folding.Text = 100

                With dgv_Details

                    .Rows.Clear()

                    n = .Rows.Add()

                    .Rows(n).Cells(0).Value = n + 1
                    .Rows(n).Cells(1).Value = Trim(vPCSNO)
                    .Rows(n).Cells(2).Value = Format(Val(Dt2.Rows(0).Item("Receipt_Meters").ToString), "########0.00")

                    'If Common_Procedures.settings.ClothReceipt_PieceNo_Concept = "1,2,3" Or Trim(UCase(Common_Procedures.settings.ClothReceipt_PieceNo_Concept)) = "CONTINUOUS NO" Then
                    '    .Rows(n).Cells(0).Value = "1"
                    'Else
                    '    .Rows(n).Cells(0).Value = "A"
                    'End If

                    .Rows(n).Cells(3).Value = Format(Val(Dt2.Rows(0).Item("Receipt_Meters").ToString), "########0.00")

                    'n = .Rows.Count - 1
                    'If (Trim(.Rows(n).Cells(1).Value) = "" And Val(.Rows(n).Cells(2).Value) <> 0) Or (.Rows(n).Cells(1).Value = Nothing And .Rows(n).Cells(2).Value = Nothing) Then
                    '    .Rows(n).Cells(0).Value = ""
                    'End If

                    For i = 0 To .Rows.Count - 1
                        .Rows(i).Cells(0).Value = i + 1
                    Next

                End With

                Calculation_TotalMeter()

                Calculation_Pavu_Consumed()
                Calculation_Yarn_Consumed()

            Else

                MessageBox.Show("Piece No does not exists", "DOES NOT SHOW PIECE DETAILS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_PieceNo.Enabled And txt_PieceNo.Visible Then txt_PieceNo.Focus()
                Exit Sub

            End If
            Dt2.Clear()

        End If
        Dt1.Clear()

        Dt1.Dispose()

        Dt2.Dispose()
        Da.Dispose()

    End Sub

    Private Sub txt_PieceNo_GotFocus(sender As Object, e As EventArgs) Handles txt_PieceNo.GotFocus
        txt_PieceNo.Tag = txt_PieceNo.Text
    End Sub

    Private Sub txt_PieceNo_LostFocus(sender As Object, e As EventArgs) Handles txt_PieceNo.LostFocus
        If Trim(txt_PieceNo.Text) <> "" Then
            If Trim(UCase(txt_PieceNo.Tag)) <> Trim(UCase(txt_PieceNo.Text)) Then
                txt_PieceNo.Tag = txt_PieceNo.Text
                Get_PieceDetails(txt_PieceNo.Text)
            End If
        End If
    End Sub

End Class