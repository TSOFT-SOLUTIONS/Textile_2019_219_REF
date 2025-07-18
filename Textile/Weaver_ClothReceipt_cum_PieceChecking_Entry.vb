Public Class Weaver_ClothReceipt_cum_PieceChecking_Entry
    Implements Interface_MDIActions

    Private Con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private PrevAct_Ctrl As New Control
    Private FrmLdSts As Boolean = False
    Private Vcbo_KeyDownVal As Double
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "CRCHK-"
    Private vmskOldText As String = ""
    Private vmskSelStrt As Integer = -1
    Private vcbo_KeyDwnVal As Double
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private dgv_ActCtrlName As String = ""
    Private dgvDet_CboBx_ColNos_Arr As Integer() = {0}

    Private SaveAll_STS As Boolean = False
    Private LastNo As String = ""


    'PIECE DETAILS GRID ======================================
    Private Enum dgvCol_Details As Integer
        SNO     '0
        PCSNO   '1
        ROLLNO  '2
        LOOMNO  '3
        RECEIPT_DOFF_MTRS   '4
        PCSTYPE1    '5
        PCSTYPE2    '6
        PCSTYPE3    '7
        PCSTYPE4    '8
        PCSTYPE5    '9
        TOTALCHECKINGMTRS   '10
        EXCESS_SHORT_MTRS   '11
        TOTALRECEIPT_EXCESSSHORT_MTRS   '12
        WEIGHT  '13
        WEIGHTPERMETER  '14
        PACKINGSLIPCODETYPE1    '15
        PACKINGSLIPCODETYPE2    '16
        PACKINGSLIPCODETYPE3    '17
        PACKINGSLIPCODETYPE4    '18
        PACKINGSLIPCODETYPE5    '19
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

    Public Sub New()
        FrmLdSts = True
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub clear()

        New_Entry = False

        pnl_Back.Enabled = True

        lbl_LotNo.Text = ""
        lbl_LotNo.ForeColor = Color.Black
        txt_ConsPavu.Text = ""
        txt_Manual_LotNo.Text = ""
        txt_ConsYarn.Text = ""

        msk_date.Text = ""
        dtp_Date.Text = ""
        cbo_Cloth.Text = ""
        cbo_EndsCount.Text = ""
        Cbo_LoomType.Text = "AUTOLOOM"
        Cbo_LoomType.Enabled = True
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1360" Then '---- Ashoka Textile (63.Velampalayam - Palladam)
            Cbo_LoomType.Text = "POWERLOOM"
            Cbo_LoomType.Enabled = False
        End If
        cbo_Weaver.Text = ""
        txt_Manual_LotNo.Text = ""
        txt_Folding.Text = "100"
        txt_Folding_Receipt.Text = "100"
        txt_WeftCount.Text = ""

        cbo_LoomNo.Text = ""
        Cbo_Grid_LoomNo.Text = ""
        cbo_WidthType.Text = ""
        cbo_StockOff.Text = Common_Procedures.Ledger_IdNoToName(Con, Val(Common_Procedures.CommonLedger.OwnSort_Ac))
        cbo_Godown_StockIN.Text = Common_Procedures.Ledger_IdNoToName(Con, Val(Common_Procedures.CommonLedger.Godown_Ac))

        cbo_weaving_job_no.Text = ""

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



        cbo_LoomNo.Enabled = True
        cbo_LoomNo.BackColor = Color.White

        cbo_Weaver.Enabled = True
        cbo_Weaver.BackColor = Color.White

        cbo_Cloth.Enabled = True
        cbo_Cloth.BackColor = Color.White

        Cbo_LoomType.Enabled = True
        Cbo_LoomType.BackColor = Color.White

        cbo_LoomNo.Enabled = True
        cbo_LoomNo.BackColor = Color.White

        txt_Folding.Enabled = True
        txt_Folding.BackColor = Color.White

        cbo_WidthType.Enabled = True
        cbo_WidthType.BackColor = Color.White

        cbo_EndsCount.Enabled = True
        cbo_EndsCount.BackColor = Color.White

        cbo_Godown_StockIN.Enabled = True
        cbo_Godown_StockIN.BackColor = Color.White

        cbo_StockOff.Enabled = True
        cbo_StockOff.BackColor = Color.White

        cbo_weaving_job_no.Enabled = True
        cbo_weaving_job_no.BackColor = Color.White

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


    Private Sub Cloth_Receipt_cum_Checking_Entry_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Weaver.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(UCase(Common_Procedures.Master_Return.Return_Value)) <> "" Then
                cbo_Weaver.Text = Trim(UCase(Common_Procedures.Master_Return.Return_Value))
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Cloth.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(UCase(Common_Procedures.Master_Return.Return_Value)) <> "" Then
                cbo_Cloth.Text = Trim(UCase(Common_Procedures.Master_Return.Return_Value))
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_EndsCount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ENDSCOUNT" And Trim(UCase(Common_Procedures.Master_Return.Return_Value)) <> "" Then
                cbo_EndsCount.Text = Trim(UCase(Common_Procedures.Master_Return.Return_Value))
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

    Private Sub Cloth_Receipt_cum_Checking_Entry_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        Con.Dispose()
        Con.Close()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Cloth_Receipt_cum_Checking_Entry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = 27 Then
            If pnl_Filter.Visible = True Then
                btn_Filter_Close_Click(sender, e)
            ElseIf MessageBox.Show("Do you want to Close ?", "FOR CLOSE", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                Me.Close()
            End If
        End If
    End Sub

    Private Sub Cloth_Receipt_cum_Checking_Entry_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim vTotWdth As String = 0

        Con.Open()

        Cbo_LoomType.Items.Clear()
        Cbo_LoomType.Items.Add("")
        Cbo_LoomType.Items.Add("AUTOLOOM")
        Cbo_LoomType.Items.Add("POWERLOOM")

        cbo_WidthType.Items.Clear()
        cbo_WidthType.Items.Add("")
        'cbo_WidthType.Items.Add("SINGLE")
        'cbo_WidthType.Items.Add("DOUBLE")
        'cbo_WidthType.Items.Add("TRIPLE")
        'cbo_WidthType.Items.Add("FOURTH")

        cbo_WidthType.Items.Add("SINGLE FABRIC FROM 1 BEAM")
        cbo_WidthType.Items.Add("SINGLE FABRIC FROM 2 BEAMS")

        cbo_WidthType.Items.Add("DOUBLE FABRIC FROM 1 BEAM")
        cbo_WidthType.Items.Add("DOUBLE FABRIC FROM 2 BEAMS")

        cbo_WidthType.Items.Add("TRIPLE FABRIC FROM 1 BEAM")
        cbo_WidthType.Items.Add("TRIPLE FABRIC FROM 2 BEAMS")

        cbo_WidthType.Items.Add("FOUR FABRIC FROM 1 BEAM")
        cbo_WidthType.Items.Add("FOUR FABRIC FROM 2 BEAMS")

        txt_Manual_LotNo.Visible = False
        lbl_Manual_LotNo_Caption.Visible = False

        btn_SaveAll.Visible = False

        lbl_weaving_job_no.Visible = False
        cbo_weaving_job_no.Visible = False

        lbl_Folding_Receipt.Visible = False
        txt_Folding_Receipt.Visible = False

        dgv_Details.Columns(dgvCol_Details.ROLLNO).Visible = False
        dgv_Details.Columns(dgvCol_Details.LOOMNO).Visible = False

        If Common_Procedures.settings.Show_Weaver_JobCard_Entry_STATUS = 1 Then

            lbl_weaving_job_no.Visible = True
            cbo_weaving_job_no.Visible = True
            cbo_weaving_job_no.BackColor = Color.White

        Else

            lbl_weft_Count_Caption.Left = lbl_weaving_job_no.Left
            txt_WeftCount.Left = cbo_weaving_job_no.Left
            txt_WeftCount.Width = cbo_WidthType.Width

        End If


        If cbo_weaving_job_no.Visible = False And lbl_weaving_job_no.Visible = False Then

            lbl_weft_Count_Caption.Left = lbl_LoomType_Caption.Left
            lbl_weft_Count_Caption.Width = lbl_LoomType_Caption.Width

        End If


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
            btn_SaveAll.Visible = True
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1578" Then '------------ Erode Amarnath 

            lbl_LotNo_Caption.Text = "Ref No."
            lbl_Folding_Receipt.Visible = True
            txt_Folding_Receipt.Visible = True
            txt_Folding_Receipt.BackColor = Color.White

            cbo_LoomNo.Visible = False
            lbl_LoomNo_Caption.Visible = False

            dgv_Details.Columns(dgvCol_Details.ROLLNO).Visible = True
            dgv_Details.Columns(dgvCol_Details.LOOMNO).Visible = True
            dgv_Details.Columns(dgvCol_Details.ROLLNO).ReadOnly = True
            dgv_Details.Columns(dgvCol_Details.LOOMNO).ReadOnly = True


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1612" Then '---- ISHANVI TEX (ERODE)

            cbo_LoomNo.Visible = False
            lbl_LoomNo_Caption.Visible = False

            dgv_Details.Columns(dgvCol_Details.PCSNO).HeaderText = "ROLL No"

            dgv_Details.Columns(dgvCol_Details.ROLLNO).Visible = False
            dgv_Details_Total.Columns(dgvCol_Details.ROLLNO).Visible = False
            dgv_Details_Total2.Columns(dgvCol_Details.ROLLNO).Visible = False

            dgv_Details.Columns(dgvCol_Details.LOOMNO).Visible = True
            dgv_Details.Columns(dgvCol_Details.LOOMNO).ReadOnly = False

            dgv_Details.Columns(dgvCol_Details.PCSNO).Width = dgv_Details.Columns(dgvCol_Details.PCSNO).Width + 30
            dgv_Details.Columns(dgvCol_Details.RECEIPT_DOFF_MTRS).Width = dgv_Details.Columns(dgvCol_Details.RECEIPT_DOFF_MTRS).Width + 10
            dgv_Details.Columns(dgvCol_Details.PCSTYPE1).Width = dgv_Details.Columns(dgvCol_Details.PCSTYPE1).Width + 10
            dgv_Details.Columns(dgvCol_Details.PCSTYPE2).Width = dgv_Details.Columns(dgvCol_Details.PCSTYPE2).Width + 10
            dgv_Details.Columns(dgvCol_Details.PCSTYPE3).Width = dgv_Details.Columns(dgvCol_Details.PCSTYPE3).Width + 10
            dgv_Details.Columns(dgvCol_Details.TOTALCHECKINGMTRS).Width = dgv_Details.Columns(dgvCol_Details.TOTALCHECKINGMTRS).Width + 10
            dgv_Details.Columns(dgvCol_Details.EXCESS_SHORT_MTRS).Width = dgv_Details.Columns(dgvCol_Details.EXCESS_SHORT_MTRS).Width + 10
            dgv_Details.Columns(dgvCol_Details.WEIGHT).Width = dgv_Details.Columns(dgvCol_Details.WEIGHT).Width + 10

        Else

            lbl_LotNo_Caption.Text = "Lot No."
            lbl_Folding_Receipt.Visible = False
            txt_Folding_Receipt.Visible = False

            cbo_LoomNo.Visible = True
            lbl_LoomNo_Caption.Visible = True

            dgv_Details.Columns(dgvCol_Details.ROLLNO).Visible = False
            dgv_Details.Columns(dgvCol_Details.LOOMNO).Visible = False

            dgv_Details.Columns(dgvCol_Details.PCSNO).Width = dgv_Details.Columns(dgvCol_Details.PCSNO).Width + 20
            dgv_Details.Columns(dgvCol_Details.RECEIPT_DOFF_MTRS).Width = dgv_Details.Columns(dgvCol_Details.RECEIPT_DOFF_MTRS).Width + 20
            dgv_Details.Columns(dgvCol_Details.PCSTYPE1).Width = dgv_Details.Columns(dgvCol_Details.PCSTYPE1).Width + 20
            dgv_Details.Columns(dgvCol_Details.PCSTYPE2).Width = dgv_Details.Columns(dgvCol_Details.PCSTYPE2).Width + 20
            dgv_Details.Columns(dgvCol_Details.PCSTYPE3).Width = dgv_Details.Columns(dgvCol_Details.PCSTYPE3).Width + 20
            dgv_Details.Columns(dgvCol_Details.PCSTYPE4).Width = dgv_Details.Columns(dgvCol_Details.PCSTYPE4).Width + 20
            dgv_Details.Columns(dgvCol_Details.PCSTYPE5).Width = dgv_Details.Columns(dgvCol_Details.PCSTYPE5).Width + 20
            dgv_Details.Columns(dgvCol_Details.TOTALCHECKINGMTRS).Width = dgv_Details.Columns(dgvCol_Details.TOTALCHECKINGMTRS).Width + 20
            dgv_Details.Columns(dgvCol_Details.EXCESS_SHORT_MTRS).Width = dgv_Details.Columns(dgvCol_Details.EXCESS_SHORT_MTRS).Width + 20
            dgv_Details.Columns(dgvCol_Details.WEIGHT).Width = dgv_Details.Columns(dgvCol_Details.WEIGHT).Width + 20

        End If

        If Trim(Common_Procedures.settings.CustomerCode) = "1087" Then '---- Kalaimagal Textiles (Palladam)

            lbl_LotNo_Caption.Text = "Ref No."
            lbl_Manual_LotNo_Caption.Text = "Lot No."

            lbl_Manual_LotNo_Caption.Visible = True
            txt_Manual_LotNo.Visible = True
            txt_Manual_LotNo.BackColor = Color.White

            lbl_StockOff_Caption.Left = lbl_LoomNo_Caption.Left
            cbo_StockOff.Left = cbo_LoomNo.Left
            cbo_StockOff.Width = cbo_LoomNo.Width
            cbo_StockOff.BackColor = Color.White

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "--1520--" Then
            'lbl_ConsYarn_Caption.Visible = False
            'txt_ConsYarn.Visible = False

            'lbl_ConsPavu_Caption.Visible = False
            'txt_ConsPavu.Visible = False
        End If

        lbl_StockOff_Caption.Visible = False
        cbo_StockOff.Visible = False
        If Common_Procedures.settings.JOBWORKENTRY_Status = 1 Then

            lbl_StockOff_Caption.Visible = True
            cbo_StockOff.Visible = True

        End If

        cbo_Godown_StockIN.Visible = False
        lbl_Godown_StockIN_Caption.Visible = False
        If Common_Procedures.settings.Multi_Godown_Status = 1 Then
            cbo_Godown_StockIN.Visible = True
            lbl_Godown_StockIN_Caption.Visible = True
        End If

        lbl_Manual_LotNo_Caption.Text = StrConv(Common_Procedures.settings.ClothReceipt_LotNo_OR_RollNo_Text, vbProperCase)
        dgv_Details.Columns(dgvCol_Details.PCSTYPE1).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type1))
        dgv_Details.Columns(dgvCol_Details.PCSTYPE2).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type2))
        dgv_Details.Columns(dgvCol_Details.PCSTYPE3).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type3))
        dgv_Details.Columns(dgvCol_Details.PCSTYPE4).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type4))
        dgv_Details.Columns(dgvCol_Details.PCSTYPE5).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type5))


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1360" Then '---- Ashoka Textile (63.Velampalayam - Palladam)

            lbl_LoomNo_Caption.Visible = False
            cbo_LoomNo.Visible = False
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

        AddHandler cbo_Cloth.GotFocus, AddressOf Control_GotFocus
        AddHandler cbo_EndsCount.GotFocus, AddressOf Control_GotFocus
        AddHandler Cbo_LoomType.GotFocus, AddressOf Control_GotFocus
        AddHandler cbo_Weaver.GotFocus, AddressOf Control_GotFocus
        AddHandler msk_date.GotFocus, AddressOf Control_GotFocus
        AddHandler txt_ConsPavu.GotFocus, AddressOf Control_GotFocus
        AddHandler txt_ConsYarn.GotFocus, AddressOf Control_GotFocus
        AddHandler txt_Manual_LotNo.GotFocus, AddressOf Control_GotFocus
        AddHandler txt_Folding.GotFocus, AddressOf Control_GotFocus
        AddHandler txt_Folding_Receipt.GotFocus, AddressOf Control_GotFocus
        AddHandler txt_WeftCount.GotFocus, AddressOf Control_GotFocus
        AddHandler cbo_weaving_job_no.GotFocus, AddressOf Control_GotFocus

        AddHandler cbo_Filter_Cloth.GotFocus, AddressOf Control_GotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf Control_GotFocus
        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf Control_GotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf Control_GotFocus
        AddHandler txt_Filter_RecNo.GotFocus, AddressOf Control_GotFocus
        AddHandler txt_Filter_RecNoTo.GotFocus, AddressOf Control_GotFocus
        AddHandler cbo_LoomNo.GotFocus, AddressOf Control_GotFocus
        AddHandler Cbo_Grid_LoomNo.GotFocus, AddressOf Control_GotFocus
        AddHandler cbo_WidthType.GotFocus, AddressOf Control_GotFocus
        AddHandler cbo_Godown_StockIN.GotFocus, AddressOf Control_GotFocus
        AddHandler cbo_StockOff.GotFocus, AddressOf Control_GotFocus

        AddHandler cbo_WidthType.LostFocus, AddressOf Control_LostFocus
        AddHandler cbo_LoomNo.LostFocus, AddressOf Control_LostFocus
        AddHandler Cbo_Grid_LoomNo.LostFocus, AddressOf Control_LostFocus
        AddHandler cbo_Godown_StockIN.LostFocus, AddressOf Control_LostFocus
        AddHandler cbo_StockOff.LostFocus, AddressOf Control_LostFocus
        AddHandler cbo_weaving_job_no.LostFocus, AddressOf Control_LostFocus

        AddHandler cbo_Cloth.LostFocus, AddressOf Control_LostFocus
        AddHandler cbo_EndsCount.LostFocus, AddressOf Control_LostFocus
        AddHandler Cbo_LoomType.LostFocus, AddressOf Control_LostFocus
        AddHandler cbo_Weaver.LostFocus, AddressOf Control_LostFocus
        AddHandler msk_date.LostFocus, AddressOf Control_LostFocus
        AddHandler txt_ConsPavu.LostFocus, AddressOf Control_LostFocus
        AddHandler txt_ConsYarn.LostFocus, AddressOf Control_LostFocus
        AddHandler txt_Manual_LotNo.LostFocus, AddressOf Control_LostFocus
        AddHandler txt_Folding.LostFocus, AddressOf Control_LostFocus
        AddHandler txt_Folding_Receipt.LostFocus, AddressOf Control_LostFocus
        AddHandler txt_WeftCount.LostFocus, AddressOf Control_LostFocus

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
        AddHandler txt_WeftCount.KeyDown, AddressOf TextBoxControl_KeyDown

        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControl_KeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControl_KeyDown
        AddHandler txt_Filter_RecNo.KeyDown, AddressOf TextBoxControl_KeyDown
        AddHandler txt_Filter_RecNoTo.KeyDown, AddressOf TextBoxControl_KeyDown
        AddHandler txt_Manual_LotNo.KeyDown, AddressOf TextBoxControl_KeyDown
        ' AddHandler txt_ConsYarn.KeyPress, AddressOf TextBoxControl_KeyPress
        'AddHandler txt_LotNoCaption.KeyPress, AddressOf TextBoxControl_KeyPress
        'AddHandler msk_date.KeyPress, AddressOf TextBoxControl_KeyPress
        AddHandler dtp_Filter_Fromdate.KeyPress, AddressOf TextBoxControl_KeyPress
        AddHandler dtp_Filter_ToDate.KeyPress, AddressOf TextBoxControl_KeyPress
        'AddHandler txt_Folding.KeyPress, AddressOf TextBoxControl_KeyPress
        AddHandler txt_WeftCount.KeyPress, AddressOf TextBoxControl_KeyPress
        AddHandler txt_Manual_LotNo.KeyPress, AddressOf TextBoxControl_KeyPress
        AddHandler txt_Filter_RecNo.KeyPress, AddressOf TextBoxControl_KeyPress
        AddHandler txt_Filter_RecNoTo.KeyPress, AddressOf TextBoxControl_KeyPress






    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView
        Dim PrevCtrl As New Object

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


                        PrevCtrl = Nothing

                        If cbo_Godown_StockIN.Visible = True And cbo_Godown_StockIN.Enabled = True Then
                            PrevCtrl = cbo_Godown_StockIN
                        ElseIf cbo_StockOff.Visible = True And cbo_StockOff.Enabled = True Then
                            PrevCtrl = cbo_StockOff
                        ElseIf txt_Manual_LotNo.Visible = True And txt_Manual_LotNo.Enabled = True Then
                            PrevCtrl = txt_Manual_LotNo
                        ElseIf txt_Folding_Receipt.Visible = True And txt_Folding_Receipt.Enabled = True Then
                            PrevCtrl = txt_Folding_Receipt
                        ElseIf cbo_WidthType.Visible = True And cbo_WidthType.Enabled = True Then
                            PrevCtrl = cbo_WidthType
                        ElseIf cbo_LoomNo.Visible = True And cbo_LoomNo.Enabled = True Then
                            PrevCtrl = cbo_LoomNo
                        Else
                            PrevCtrl = txt_Folding

                        End If

                        Common_Procedures.DGVGrid_Cursor_Focus(dgv1, keyData, PrevCtrl, Nothing, dgvDet_CboBx_ColNos_Arr, dgtxt_Details, msk_date)

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

    Private Sub move_record(ByVal idno As String)
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim NewCode As String = ""
        Dim n As Integer = 0
        Dim SNo As Integer = 0
        Dim LockSTS As Boolean = False
        Dim WGSLockSTS As Boolean = False

        clear()
        New_Entry = False
        Try
            If Val(idno) = 0 Then Exit Sub

            NewCode = Trim(Pk_Condition) & Trim(lbl_Company.Tag) & "-" & Trim(idno) & "/" & Trim(Common_Procedures.FnYearCode)

            da = New SqlClient.SqlDataAdapter(" SELECT a.* , e.Ledger_Name as StockOff_Name FROM Weaver_Cloth_Receipt_Head a LEFT OUTER JOIN Ledger_Head e ON a.StockOff_IdNo = e.Ledger_IdNo WHERE a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'", Con)
            dt = New DataTable
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                lbl_LotNo.Text = dt.Rows(0).Item("Weaver_ClothReceipt_No").ToString
                dtp_Date.Text = dt.Rows(0).Item("Weaver_ClothReceipt_Date")
                msk_date.Text = dtp_Date.Text
                Cbo_LoomType.Text = dt.Rows(0).Item("Loom_Type").ToString
                cbo_Weaver.Text = Common_Procedures.Ledger_IdNoToName(Con, Val(dt.Rows(0).Item("Ledger_IdNo").ToString))
                cbo_Cloth.Text = Common_Procedures.Cloth_IdNoToName(Con, Val(dt.Rows(0).Item("Cloth_IdNo").ToString))
                cbo_EndsCount.Text = Common_Procedures.EndsCount_IdNoToName(Con, Val(dt.Rows(0).Item("EndsCount_IdNo").ToString))
                txt_WeftCount.Text = Common_Procedures.Count_IdNoToName(Con, Val(dt.Rows(0).Item("Count_IdNo").ToString))
                ' txt_TotalMeters.Text = dt.Rows(0).Item("Total_Meters").ToString
                txt_Manual_LotNo.Text = dt.Rows(0).Item("Lot_No").ToString


                If Not IsDBNull(dt.Rows(0).Item("Folding")) Then txt_Folding.Text = dt.Rows(0).Item("Folding").ToString


                cbo_Godown_StockIN.Text = Common_Procedures.Ledger_IdNoToName(Con, Val(dt.Rows(0).Item("WareHouse_IdNo").ToString))
                cbo_StockOff.Text = dt.Rows(0).Item("StockOff_Name").ToString
                cbo_LoomNo.Text = Common_Procedures.Loom_IdNoToName(Con, Val(dt.Rows(0).Item("Loom_IdNo").ToString))
                cbo_WidthType.Text = dt.Rows(0).Item("Width_Type").ToString

                LockSTS = False
                WGSLockSTS = False
                If Trim(dt.Rows(0).Item("Weaver_Wages_Code").ToString) <> "" Or Trim(dt.Rows(0).Item("Weaver_IR_Wages_Code").ToString) <> "" Then
                    LockSTS = True
                    WGSLockSTS = True
                End If

                cbo_weaving_job_no.Text = dt.Rows(0).Item("Weaving_JobCode_forSelection").ToString

                    da2 = New SqlClient.SqlDataAdapter("SELECT a.* FROM Weaver_ClothReceipt_Piece_Details a WHERE a.Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' ORDER BY PieceNo_OrderBy , Piece_No , Sl_No", Con)
                    dt2 = New DataTable
                    da2.Fill(dt2)

                    With dgv_Details

                        .Rows.Clear()
                        SNo = 0

                        If dt2.Rows.Count > 0 Then

                            For i = 0 To dt2.Rows.Count - 1

                                SNo = SNo + 1

                                n = .Rows.Add

                                .Rows(n).Cells(dgvCol_Details.SNO).Value = Val(SNo)
                            .Rows(n).Cells(dgvCol_Details.PCSNO).Value = dt2.Rows(i).Item("Piece_No").ToString
                            .Rows(n).Cells(dgvCol_Details.ROLLNO).Value = dt2.Rows(i).Item("Lot_No").ToString
                            .Rows(n).Cells(dgvCol_Details.LOOMNO).Value = Common_Procedures.Loom_IdNoToName(Con, Val(dt2.Rows(i).Item("Loom_IdNo").ToString))
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
                        .Rows(0).Cells(dgvCol_Details.PCSNO).Value = Format(Val(dt.Rows(0).Item("Total_Receipt_Pcs").ToString), "########0")
                        .Rows(0).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value = ""
                        .Rows(0).Cells(dgvCol_Details.PCSTYPE1).Value = Format(Val(dt.Rows(0).Item("Type1_Checking_Meters").ToString), "########0.00")
                        .Rows(0).Cells(dgvCol_Details.PCSTYPE2).Value = Format(Val(dt.Rows(0).Item("Type2_Checking_Meters").ToString), "########0.00")
                        .Rows(0).Cells(dgvCol_Details.PCSTYPE3).Value = Format(Val(dt.Rows(0).Item("Type3_Checking_Meters").ToString), "########0.00")
                        .Rows(0).Cells(dgvCol_Details.PCSTYPE4).Value = Format(Val(dt.Rows(0).Item("Type4_Checking_Meters").ToString), "########0.00")
                        .Rows(0).Cells(dgvCol_Details.PCSTYPE5).Value = Format(Val(dt.Rows(0).Item("Type5_Checking_Meters").ToString), "########0.00")
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
                        .Rows(0).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value = Format(Val(dt.Rows(0).Item("Receipt_Meters").ToString), "########0.00")
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

                    txt_ConsPavu.Text = dt.Rows(0).Item("Consumed_Pavu").ToString
                    txt_ConsYarn.Text = dt.Rows(0).Item("Consumed_Yarn").ToString

                End If
                dt.Clear()

            dt.Dispose()
            da.Dispose()

            If LockSTS = True Then

                'cbo_Weaver.Enabled = False
                'cbo_Weaver.BackColor = Color.LightGray

                cbo_Cloth.Enabled = False
                cbo_Cloth.BackColor = Color.LightGray

                'Cbo_LoomType.Enabled = False
                'Cbo_LoomType.BackColor = Color.LightGray

                'cbo_LoomNo.Enabled = False
                'cbo_LoomNo.BackColor = Color.LightGray

                txt_Folding.Enabled = False
                txt_Folding.BackColor = Color.LightGray

                'cbo_WidthType.Enabled = False
                'cbo_WidthType.BackColor = Color.LightGray

                If WGSLockSTS = True Then
                    'cbo_EndsCount.Enabled = False
                    cbo_EndsCount.BackColor = Color.LightGray
                End If


                cbo_Godown_StockIN.Enabled = False
                cbo_Godown_StockIN.BackColor = Color.LightGray

                cbo_StockOff.Enabled = False
                cbo_StockOff.BackColor = Color.LightGray


                cbo_weaving_job_no.Enabled = False
                cbo_weaving_job_no.BackColor = Color.LightGray

            End If

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        End Try

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim NewCode As String = ""
        Dim tr As SqlClient.SqlTransaction
        Dim vOrdByNo As String = ""

        Con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        Con.Open()


        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_LotNo.Text)

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_LotNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
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

        Da = New SqlClient.SqlDataAdapter("select COUNT(*) from Weaver_ClothReceipt_Piece_Details Where Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and (PackingSlip_Code_Type1 <> '' or PackingSlip_Code_Type2 <> '' or PackingSlip_Code_Type3 <> '' or PackingSlip_Code_Type4 <> '' or PackingSlip_Code_Type5 <> '' or BuyerOffer_Code_Type1 <> '' or BuyerOffer_Code_Type2 <> '' or BuyerOffer_Code_Type3 <> '' or BuyerOffer_Code_Type4 <> '' or BuyerOffer_Code_Type5 <> '' or Bale_UnPacking_Code_Type1 <> '' or Bale_UnPacking_Code_Type2 <> '' or Bale_UnPacking_Code_Type3 <> '' or Bale_UnPacking_Code_Type4 <> '' or Bale_UnPacking_Code_Type5 <> '')", Con)
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

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "DELETE FROM Weaver_ClothReceipt_Piece_Details WHERE Company_IdNo = " & Val(lbl_Company.Tag) & " AND Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Weaver_Piece_Checking_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            cmd.CommandText = "DELETE FROM Weaver_Cloth_Receipt_Head WHERE Company_IdNo = " & Val(lbl_Company.Tag) & " AND Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'"
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
                    lbl_LotNo.Text = Trim(UCase(inpno))
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


            Da = New SqlClient.SqlDataAdapter("SELECT TOP 1 Weaver_ClothReceipt_No FROM Weaver_Cloth_Receipt_Head WHERE Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " AND Weaver_ClothReceipt_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' ORDER BY for_orderby , Weaver_ClothReceipt_No", Con)
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


            Da = New SqlClient.SqlDataAdapter("SELECT TOP 1 Weaver_ClothReceipt_No FROM Weaver_Cloth_Receipt_Head WHERE Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' ORDER BY for_orderby DESC , Weaver_ClothReceipt_No DESC", Con)
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


            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_LotNo.Text))

            Da = New SqlClient.SqlDataAdapter("SELECT TOP 1 Weaver_ClothReceipt_No FROM Weaver_Cloth_Receipt_Head WHERE for_orderby > '" & Trim(OrdByNo) & "' AND Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " AND Weaver_ClothReceipt_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' ORDER BY for_orderby ,Weaver_ClothReceipt_No", Con)
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


            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_LotNo.Text))

            Da = New SqlClient.SqlDataAdapter("SELECT TOP 1 Weaver_ClothReceipt_No FROM Weaver_Cloth_Receipt_Head WHERE for_orderby < '" & Trim(OrdByNo) & "' AND Company_IdNo =" & Str(Val(lbl_Company.Tag)) & " AND Weaver_ClothReceipt_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' ORDER BY for_orderby DESC ,Weaver_ClothReceipt_No DESC", Con)
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

            lbl_LotNo.Text = Common_Procedures.get_MaxCode(Con, "Weaver_Cloth_Receipt_Head ", "Weaver_ClothReceipt_Code", "for_orderby", "(Weaver_ClothReceipt_Code LIKE '" & Trim(Pk_Condition) & "%')", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_LotNo.ForeColor = Color.Red
            If dtp_Date.Enabled = True Then msk_date.Text = Date.Today.ToShortDateString

            da = New SqlClient.SqlDataAdapter("SELECT TOP 1 * FROM Weaver_Cloth_Receipt_Head WHERE Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code LIKE '" & Trim(Pk_Condition) & "%/" & Trim(Common_Procedures.FnYearCode) & "' ORDER BY for_orderby DESC , Weaver_ClothReceipt_No DESC", Con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt.Rows(0).Item("Weaver_ClothReceipt_Date").ToString <> "" Then dtp_Date.Text = dt.Rows(0).Item("Weaver_ClothReceipt_Date").ToString
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
        Dim VRECCODE As String = ""
        Dim vTotRcptPcs As Single, vTotRcptMtrs As Double



        Con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        Con.Open()

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection ?", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close other windows!....", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If
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

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_LotNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Weaver_ClothReceipt_and_PieceChecking_Entry, New_Entry, Me, Con, "Weaver_Cloth_Receipt_Head", "Weaver_ClothReceipt_Code", NewCode, "Weaver_Cloth_Receipt_Date", "(Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Weaver_ClothReceipt_No desc", dtp_Date.Value.Date) = False Then Exit Sub

        vLed_IdNo = Common_Procedures.Ledger_NameToIdNo(Con, cbo_Weaver.Text)
        If Val(vLed_IdNo) = 0 Then
            MessageBox.Show("Invalid Weaver Name ?", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            cbo_Weaver.Focus()
            Exit Sub
        End If
        Led_type = Common_Procedures.get_FieldValue(Con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(vLed_IdNo)) & ")")

        vClo_IdNo = Common_Procedures.Cloth_NameToIdNo(Con, cbo_Cloth.Text)
        If Val(vClo_IdNo) = 0 Then
            MessageBox.Show("Invalid Cloth Name ?", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            cbo_Cloth.Focus()
            Exit Sub
        End If

        vEndct_IdNo = Common_Procedures.EndsCount_NameToIdNo(Con, cbo_EndsCount.Text)
        If Val(vEndct_IdNo) = 0 Then
            MessageBox.Show("Invalid Ends ?", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            cbo_EndsCount.Focus()
            Exit Sub
        End If

        If Val(txt_Folding.Text) = 0 Then txt_Folding.Text = 100
        If Val(txt_Folding_Receipt.Text) = 0 Then txt_Folding_Receipt.Text = 100


        If Trim(Cbo_LoomType.Text) = "" Then
            Cbo_LoomType.Text = "AUTOLOOM"
        End If

        WftCnt_ID = Common_Procedures.Count_NameToIdNo(Con, txt_WeftCount.Text)
        If Val(WftCnt_ID) = 0 Then
            MessageBox.Show("Invalid Count ?", "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            cbo_Cloth.Focus()
            Exit Sub
        End If

        If cbo_LoomNo.Visible = True Then
            vLm_IdNo = Common_Procedures.Loom_NameToIdNo(Con, cbo_LoomNo.Text)
        Else
            vLm_IdNo = Common_Procedures.Loom_NameToIdNo(Con, dgv_Details.Rows(0).Cells(dgvCol_Details.LOOMNO).Value)

        End If




        If cbo_WidthType.Visible = True Then
            If Trim(UCase(Cbo_LoomType.Text)) = "AUTOLOOM" Then
                If Trim(cbo_WidthType.Text) = "" Then
                    MessageBox.Show("Invalid Width Type?", "DOESNOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    cbo_WidthType.Focus()
                    Exit Sub
                End If
            End If
        End If



        If txt_Manual_LotNo.Visible = False Then
            txt_Manual_LotNo.Text = lbl_LotNo.Text

        Else

            If Trim(txt_Manual_LotNo.Text) = "" Then
                MessageBox.Show("Invalid Lot No.?", "DOESNOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                txt_Manual_LotNo.Focus()
                Exit Sub
            End If

        End If

        vGod_ID = Common_Procedures.Ledger_AlaisNameToIdNo(Con, cbo_Godown_StockIN.Text)
        If cbo_Godown_StockIN.Visible = True Then
            If vGod_ID = 0 Then
                MessageBox.Show("Invalid Fabric Godown Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_Godown_StockIN.Enabled And cbo_Godown_StockIN.Visible Then cbo_Godown_StockIN.Focus()
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


                    If dgv_Details.Columns(dgvCol_Details.ROLLNO).Visible = True Then

                        If Trim(txt_Folding_Receipt.Text) = Trim(txt_Folding.Text) Then

                        End If
                        If Trim(.Rows(I).Cells(dgvCol_Details.ROLLNO).Value) = "" Then
                            MessageBox.Show("Invalid Roll No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If .Enabled And .Visible Then
                                .CurrentCell = .Rows(I).Cells(dgvCol_Details.ROLLNO)
                                .Focus()
                            End If
                            Exit Sub
                        End If

                    End If


                    End If


            Next

        End With

        Calculation_TotalMeter()


        vTotTyp1Mtr = 0 : vTotTyp2Mtr = 0 : vTotTyp3Mtr = 0 : vTotTyp4Mtr = 0 : vTotTyp5Mtr = 0 : vTotChkMtr = 0
        vTotWgt = 0
        vTotRcptPcs = 0
        If dgv_Details_Total.RowCount > 0 Then

            vTotRcptPcs = Val(dgv_Details_Total.Rows(0).Cells(dgvCol_Details.PCSNO).Value())

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
                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_LotNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            Else
                lbl_LotNo.Text = Common_Procedures.get_MaxCode(Con, "Weaver_Cloth_Receipt_Head", "Weaver_ClothReceipt_Code", "for_orderby", "(Weaver_ClothReceipt_Code LIKE '" & Trim(Pk_Condition) & "%')", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)
                NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_LotNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            End If

            VRECCODE = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_LotNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = Con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(msk_date.Text))

            vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_LotNo.Text)
            vSELC_LOTCODE = Trim(lbl_LotNo.Text) & "/" & Trim(Common_Procedures.FnYearCode) & "/" & Trim(Val(lbl_Company.Tag)) & "/" & Trim(Pk_Condition)

            cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
            cmd.ExecuteNonQuery()

            If New_Entry = True Then

                cmd.CommandText = "INSERT INTO Weaver_Cloth_Receipt_Head ( Receipt_Type,         Company_IdNo    ,   Weaver_ClothReceipt_Code ,    for_orderby       ,          Weaver_ClothReceipt_No      ,   Weaver_ClothReceipt_Date ,  Weaver_Piece_Checking_Code, Weaver_Piece_Checking_Increment, Weaver_Piece_Checking_Date,               Loom_Type          ,    Ledger_IdNo     ,     Cloth_IdNo        ,                   Folding_Receipt                 ,             Folding_Checking      ,                 Folding           ,   EndsCount_IdNo        ,        Count_IdNo     ,             Width_Type            ,            Loom_IdNo      ,            StockOff_IdNo     ,        WareHouse_IdNo     ,       ReceiptMeters_Receipt  ,       ReceiptMeters_Checking ,         Receipt_Meters       ,     Type1_Checking_Meters    ,       Type2_Checking_Meters   ,       Type3_Checking_Meters   ,      Type4_Checking_Meters    ,      Type5_Checking_Meters    ,      Total_Checking_Meters   ,       Total_Weight      ,       Total_Type1Meters_100Folding    ,      Total_Type2Meters_100Folding    ,      Total_Type3Meters_100Folding    ,       Total_Type4Meters_100Folding   ,      Total_Type5Meters_100Folding    ,      Total_Meters_100Folding        ,            Excess_Short_Meter   ,        ConsumedPavu_Receipt   ,      ConsumedPavu_Checking    ,          Consumed_Pavu        ,         ConsumedYarn_Receipt  ,     ConsumedYarn_Checking     ,            Consumed_Yarn      ,                    Lot_No              ,       lotcode_forSelection   , Weaving_JobCode_forSelection              ,Total_Receipt_Pcs              ) " &
                                  "            VALUES                    (     'W'     , " & Val(lbl_Company.Tag) & ", '" & Trim(NewCode) & "'    , " & Val(vOrdByNo) & ", '" & Trim(lbl_LotNo.Text) & "',       @EntryDate           , '" & Trim(NewCode) & "'    ,              1                 ,          @EntryDate       , '" & Trim(Cbo_LoomType.Text) & "', " & Val(vLed_IdNo) & ", " & Val(vClo_IdNo) & ",      " & Str(Val(txt_Folding_Receipt.Text)) & "   , " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(txt_Folding.Text)) & ", " & Val(vEndct_IdNo) & ", " & Val(WftCnt_ID) & ", '" & Trim(cbo_WidthType.Text) & "', " & Str(Val(vLm_IdNo)) & ",  " & Str(Val(StkOff_ID)) & " , " & Str(Val(vGod_ID)) & " , " & Str(Val(vTotRcptMtr)) & ", " & Str(Val(vTotRcptMtr)) & ", " & Str(Val(vTotRcptMtr)) & ", " & Str(Val(vTotTyp1Mtr)) & ",  " & Str(Val(vTotTyp2Mtr)) & ",  " & Str(Val(vTotTyp3Mtr)) & ",  " & Str(Val(vTotTyp4Mtr)) & ",  " & Str(Val(vTotTyp5Mtr)) & ",  " & Str(Val(vTotChkMtr)) & ", " & Str(Val(vTotWgt)) & ", " & Str(Val(vTot_100Fld_Typ1Mtr)) & ", " & Str(Val(vTot_100Fld_Typ2Mtr)) & ", " & Str(Val(vTot_100Fld_Typ3Mtr)) & ", " & Str(Val(vTot_100Fld_Typ4Mtr)) & ", " & Str(Val(vTot_100Fld_Typ5Mtr)) & ", " & Str(Val(vTot_100Fld_ChkMtr)) & ", " & Str(Val(vTotExcShtMtr)) & " , " & Val(txt_ConsPavu.Text) & ", " & Val(txt_ConsPavu.Text) & ", " & Val(txt_ConsPavu.Text) & ", " & Val(txt_ConsYarn.Text) & ", " & Val(txt_ConsYarn.Text) & ", " & Val(txt_ConsYarn.Text) & ",'" & Trim(txt_Manual_LotNo.Text) & "','" & Trim(vSELC_LOTCODE) & "'   ,  '" & Trim(cbo_weaving_job_no.Text) & "'  ," & Str(Val(vTotRcptPcs)) & "    ) "
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "UPDATE Weaver_Cloth_Receipt_Head SET Receipt_Type = 'W', Weaver_ClothReceipt_Date = @EntryDate , Weaver_Piece_Checking_Date = @EntryDate , Loom_Type = '" & Trim(Cbo_LoomType.Text) & "', Ledger_IdNo = " & Val(vLed_IdNo) & ", Cloth_IdNo=" & Val(vClo_IdNo) & ", EndsCount_IdNo =" & Val(vEndct_IdNo) & ", Count_IdNo = " & Val(WftCnt_ID) & ", Width_Type = '" & Trim(cbo_WidthType.Text) & "', Loom_IdNo =  " & Str(Val(vLm_IdNo)) & ", StockOff_IdNo = " & Str(Val(StkOff_ID)) & ",  WareHouse_IdNo = " & Str(Val(vGod_ID)) & ", Folding_Receipt = " & Str(Val(txt_Folding_Receipt.Text)) & " , Folding_Checking = " & Str(Val(txt_Folding.Text)) & " , Folding = " & Str(Val(txt_Folding.Text)) & " , ReceiptMeters_Receipt = " & Str(Val(vTotRcptMtr)) & ", ReceiptMeters_Checking = " & Str(Val(vTotRcptMtr)) & ", Receipt_Meters = " & Str(Val(vTotRcptMtr)) & " , Type1_Checking_Meters = " & Str(Val(vTotTyp1Mtr)) & ", Type2_Checking_Meters = " & Str(Val(vTotTyp2Mtr)) & ", Type3_Checking_Meters = " & Str(Val(vTotTyp3Mtr)) & ", Type4_Checking_Meters = " & Str(Val(vTotTyp4Mtr)) & ", Type5_Checking_Meters = " & Str(Val(vTotTyp5Mtr)) & ", Total_Checking_Meters = " & Str(Val(vTotChkMtr)) & ", Total_Weight = " & Str(Val(vTotWgt)) & " , Total_Type1Meters_100Folding = " & Str(Val(vTot_100Fld_Typ1Mtr)) & ", Total_Type2Meters_100Folding = " & Str(Val(vTot_100Fld_Typ2Mtr)) & ", Total_Type3Meters_100Folding = " & Str(Val(vTot_100Fld_Typ3Mtr)) & ", Total_Type4Meters_100Folding = " & Str(Val(vTot_100Fld_Typ4Mtr)) & ", Total_Type5Meters_100Folding = " & Str(Val(vTot_100Fld_Typ5Mtr)) & ", Total_Meters_100Folding = " & Str(Val(vTot_100Fld_ChkMtr)) & " , Excess_Short_Meter =" & Str(Val(vTotExcShtMtr)) & " , ConsumedPavu_Receipt = " & Val(txt_ConsPavu.Text) & ", ConsumedPavu_Checking = " & Val(txt_ConsPavu.Text) & ", Consumed_Pavu = " & Val(txt_ConsPavu.Text) & ", ConsumedYarn_Receipt = " & Val(txt_ConsYarn.Text) & ", ConsumedYarn_Checking = " & Val(txt_ConsYarn.Text) & ", Consumed_Yarn = " & Val(txt_ConsYarn.Text) & ",Lot_No='" & Trim(txt_Manual_LotNo.Text) & "' ,lotcode_forSelection='" & Trim(vSELC_LOTCODE) & "' ,Weaving_JobCode_forSelection =  '" & Trim(cbo_weaving_job_no.Text) & "'  ,Total_Receipt_Pcs= " & Str(Val(vTotRcptPcs)) & " WHERE Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Delete from Weaver_Piece_Checking_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into Weaver_Piece_Checking_Head ( Receipt_Type , Weaver_Piece_Checking_Code,               Company_IdNo       ,     Weaver_Piece_Checking_No  ,          for_OrderBy      , Weaver_Piece_Checking_Date,       Ledger_IdNo          ,         Receipt_PkCondition ,      Piece_Receipt_Code ,         Piece_Receipt_No      , Piece_Receipt_Date,             Lot_No            ,          Cloth_IdNo         ,             Party_DcNo        ,      ReceiptMeters_Receipt   ,               Folding              , Total_Checking_Receipt_Meters ,           Total_Type1_Meters  ,      Total_Type2_Meters       ,   Total_Type3_Meters          ,     Total_Type4_Meters        ,     Total_Type5_Meters       ,       Total_Checking_Meters ,        Total_Weight      ,  Total_Type1Meters_100Folding          , Total_Type2Meters_100Folding            ,  Total_Type3Meters_100Folding        ,    Total_Type4Meters_100Folding       ,     Total_Type5Meters_100Folding     ,      Total_Meters_100Folding         ,         Excess_Short_Meter      , StockOff_IdNo               ,                           user_idNo      , Total_Checking_Details_Meters_100Folding ,     Total_ExcessShort_Details_Meters ,        Loom_Type                 ,        Weaving_JobCode_forSelection    ) " &
                                            "     Values              (    'W'       ,   '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_LotNo.Text) & "', " & Str(Val(vOrdByNo)) & ",        @EntryDate         , " & Str(Val(vLed_IdNo)) & ", '" & Trim(Pk_Condition) & "', '" & Trim(VRECCODE) & "', '" & Trim(lbl_LotNo.Text) & "',      @EntryDate   , '" & Trim(lbl_LotNo.Text) & "',  " & Str(Val(vClo_IdNo)) & ", '" & Trim(lbl_LotNo.Text) & "', " & Str(Val(vTotRcptMtr)) & ",  " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(vTotRcptMtr)) & " ,  " & Str(Val(vTotTyp1Mtr)) & ",  " & Str(Val(vTotTyp2Mtr)) & ", " & Str(Val(vTotTyp3Mtr)) & ",  " & Str(Val(vTotTyp4Mtr)) & ", " & Str(Val(vTotTyp5Mtr)) & ", " & Str(Val(vTotChkMtr)) & ", " & Str(Val(vTotWgt)) & ", " & Str(Val(vTot_100Fld_Typ1Mtr)) & "  ,    " & Str(Val(vTot_100Fld_Typ2Mtr)) & ", " & Str(Val(vTot_100Fld_Typ3Mtr)) & ",  " & Str(Val(vTot_100Fld_Typ4Mtr)) & ", " & Str(Val(vTot_100Fld_Typ5Mtr)) & ",  " & Str(Val(vTot_100Fld_ChkMtr)) & ", " & Str(Val(vTotExcShtMtr)) & " , " & Str(Val(StkOff_ID)) & " , " & Val(Common_Procedures.User.IdNo) & " , " & Val(vTot_100Fld_ChkMtr) & "          , " & Val(vTotExcShtMtr) & "           , '" & Trim(Cbo_LoomType.Text) & "', '" & Trim(cbo_weaving_job_no.Text) & "' ) "
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Weaver_ClothReceipt_Piece_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and Lot_Code = '" & Trim(NewCode) & "' and PackingSlip_Code_Type1 = '' and PackingSlip_Code_Type2 = ''  and PackingSlip_Code_Type3 = ''  and PackingSlip_Code_Type4 = ''  and PackingSlip_Code_Type5 = '' and BuyerOffer_Code_Type1 = '' and BuyerOffer_Code_Type2 = '' and BuyerOffer_Code_Type3 = '' and BuyerOffer_Code_Type4 = '' and BuyerOffer_Code_Type5 = '' and Bale_UnPacking_Code_Type1  = '' and Bale_UnPacking_Code_Type2  = '' and Bale_UnPacking_Code_Type3  = '' and Bale_UnPacking_Code_Type4  = '' and Bale_UnPacking_Code_Type5 = ''"
            cmd.ExecuteNonQuery()

            With dgv_Details

                SNo = 0
                Dim LmName = ""
                Dim vDET_LmIdNo = 0
                Dim vdet_RollNo = 0
                For I = 0 To .Rows.Count - 1

                    If Trim(.Rows(I).Cells(dgvCol_Details.PCSNO).Value) <> "" Or Val(dgv_Details.Rows(I).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value) <> 0 Or Val(dgv_Details.Rows(I).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value) <> 0 Or Val(dgv_Details.Rows(I).Cells(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Value) <> 0 Then

                        SNo = SNo + 1

                        vOrdByPcsNo = Common_Procedures.OrderBy_CodeToValue(dgv_Details.Rows(I).Cells(dgvCol_Details.PCSNO).Value)



                        If dgv_Details.Columns(dgvCol_Details.LOOMNO).Visible = True Then

                            LmName = dgv_Details.Rows(I).Cells(dgvCol_Details.LOOMNO).Value
                            vDET_LmIdNo = Common_Procedures.Loom_NameToIdNo(Con, LmName, tr)

                        Else

                            vDET_LmIdNo = vLm_IdNo
                            LmName = Trim(cbo_LoomNo.Text)

                        End If

                        If dgv_Details.Columns(dgvCol_Details.ROLLNO).Visible = True Then
                            vdet_RollNo = dgv_Details.Rows(I).Cells(dgvCol_Details.ROLLNO).Value

                        Else
                            vdet_RollNo = Trim(txt_Manual_LotNo.Text)


                        End If




                        Nr = 0
                        cmd.CommandText = "UPDATE Weaver_ClothReceipt_Piece_Details  SET Weaver_ClothReceipt_Date =  @EntryDate, Weaver_Piece_Checking_Date =  @EntryDate, Ledger_IdNo = " & Str(Val(vLed_IdNo)) & ", StockOff_IdNo = " & Str(Val(StkOff_ID)) & ", WareHouse_IdNo = " & Str(Val(vGod_ID)) & ", Cloth_IdNo = " & Val(vClo_IdNo) & ", Folding_Receipt = " & Val(txt_Folding_Receipt.Text) & ", Folding_Checking = " & Val(txt_Folding.Text) & ", Folding = " & Val(txt_Folding.Text) & ", Loom_No = '" & Trim(LmName) & "', Loom_IdNo = " & Str(Val(vDET_LmIdNo)) & ",  Width_Type = '" & Trim(cbo_WidthType.Text) & "', Sl_No =" & Str(Val(SNo)) & ", Main_PieceNo = '" & Trim(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.PCSNO).Value)) & "', PieceNo_OrderBy = " & Str(Val(vOrdByPcsNo)) & ", ReceiptMeters_Receipt = " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value)) & " , ReceiptMeters_Checking = " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value)) & " , Receipt_Meters = " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value)) & " , Type1_Meters =" & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.PCSTYPE1).Value)) & " ,Type2_Meters =" & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.PCSTYPE2).Value)) & ", Type3_Meters =" & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.PCSTYPE3).Value)) & ", Type4_Meters =" & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.PCSTYPE4).Value)) & ", Type5_Meters = " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.PCSTYPE5).Value)) & ", Total_Checking_Meters = " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value)) & ", Excess_Short_Meter = " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.EXCESS_SHORT_MTRS).Value)) & ", Weight = " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.WEIGHT).Value)) & ", Weight_Meter = " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.WEIGHTPERMETER).Value)) & ",Weaving_JobCode_forSelection ='" & Trim(cbo_weaving_job_no.Text) & "'   Where Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and Lot_Code ='" & Trim(NewCode) & "' and Piece_No = '" & Trim(dgv_Details.Rows(I).Cells(dgvCol_Details.PCSNO).Value) & "'"
                        'cmd.CommandText = "UPDATE Weaver_ClothReceipt_Piece_Details  SET Weaver_ClothReceipt_Date =  @EntryDate, Weaver_Piece_Checking_Date =  @EntryDate, Ledger_IdNo = " & Str(Val(vLed_IdNo)) & ", StockOff_IdNo = " & Str(Val(StkOff_ID)) & ", WareHouse_IdNo = " & Str(Val(vGod_ID)) & ", Cloth_IdNo = " & Val(vClo_IdNo) & ", Folding_Receipt = " & Val(txt_Folding_Receipt.Text) & ", Folding_Checking = " & Val(txt_Folding.Text) & ", Folding = " & Val(txt_Folding.Text) & ", Loom_No = '" & Trim(cbo_LoomNo.Text) & "', Loom_IdNo = " & Str(Val(vLm_IdNo)) & ",  Width_Type = '" & Trim(cbo_WidthType.Text) & "', Sl_No =" & Str(Val(SNo)) & ", Main_PieceNo = '" & Trim(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.PCSNO).Value)) & "', PieceNo_OrderBy = " & Str(Val(vOrdByPcsNo)) & ", ReceiptMeters_Receipt = " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value)) & " , ReceiptMeters_Checking = " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value)) & " , Receipt_Meters = " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value)) & " , Type1_Meters =" & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.PCSTYPE1).Value)) & " ,Type2_Meters =" & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.PCSTYPE2).Value)) & ", Type3_Meters =" & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.PCSTYPE3).Value)) & ", Type4_Meters =" & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.PCSTYPE4).Value)) & ", Type5_Meters = " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.PCSTYPE5).Value)) & ", Total_Checking_Meters = " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value)) & ", Excess_Short_Meter = " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.EXCESS_SHORT_MTRS).Value)) & ", Weight = " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.WEIGHT).Value)) & ", Weight_Meter = " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.WEIGHTPERMETER).Value)) & ",Weaving_JobCode_forSelection ='" & Trim(cbo_weaving_job_no.Text) & "'   Where Weaver_ClothReceipt_Code = '" & Trim(NewCode) & "' and Lot_Code ='" & Trim(NewCode) & "' and Piece_No = '" & Trim(dgv_Details.Rows(I).Cells(dgvCol_Details.PCSNO).Value) & "'"
                        Nr = cmd.ExecuteNonQuery()
                        If Nr = 0 Then

                            cmd.CommandText = "INSERT INTO Weaver_ClothReceipt_Piece_Details (   Company_IdNo               ,        Lot_Code          , Weaver_ClothReceipt_Code ,       Weaver_ClothReceipt_No   ,        for_orderby         ,   Weaver_ClothReceipt_Date  ,                    Lot_No            ,  Weaver_Piece_Checking_Code,       Weaver_Piece_Checking_No , Weaver_Piece_Checking_Date,          Ledger_IdNo    ,            StockOff_IdNo     ,         WareHouse_IdNo     ,         Cloth_IdNo       ,    Folding_Receipt                    ,        Folding_Checking      ,             Folding          ,               Loom_No          ,           Loom_IdNo       ,               Width_Type           ,            Sl_No     ,                               Piece_No                                   ,                                   Main_PieceNo                             ,          PieceNo_OrderBy       ,               ReceiptMeters_Receipt                                                 ,                           ReceiptMeters_Checking                                    ,                                 Receipt_Meters                                      ,                                 Type1_Meters                               ,                                   Type2_Meters                               ,                                    Type3_Meters                               ,                                 Type4_Meters                               ,                                   Type5_Meters                               ,                         Total_Checking_Meters                                       ,                         Excess_Short_Meter                                         ,                                   Weight                                  ,                                   Weight_Meter                                     , Create_Status, Remarks, PackingSlip_Code_Type1, PackingSlip_Code_Type2, PackingSlip_Code_Type3, PackingSlip_Code_Type4, PackingSlip_Code_Type5, BuyerOffer_Code_Type1, BuyerOffer_Code_Type2, BuyerOffer_Code_Type3, BuyerOffer_Code_Type4, BuyerOffer_Code_Type5 , Bale_UnPacking_Code_Type1, Bale_UnPacking_Code_Type2, Bale_UnPacking_Code_Type3, Bale_UnPacking_Code_Type4, Bale_UnPacking_Code_Type5  ,  Weaving_JobCode_forSelection )" &
                                              "             VALUES                           (" & Val(lbl_Company.Tag) & "  , '" & Trim(NewCode) & "'  ,  '" & Trim(NewCode) & "' ,  '" & Trim(lbl_LotNo.Text) & "', " & Str(Val(vOrdByNo)) & " ,           @EntryDate        , '" & Trim(vdet_RollNo) & "',  '" & Trim(NewCode) & "'   ,  '" & Trim(lbl_LotNo.Text) & "',       @EntryDate          , " & Str(Val(vLed_IdNo)) & ",   " & Str(Val(StkOff_ID)) & ",   " & Str(Val(vGod_ID)) & ", " & Val(vClo_IdNo) & ", " & Val(txt_Folding_Receipt.Text) & " , " & Val(txt_Folding.Text) & ", " & Val(txt_Folding.Text) & ", '" & Trim(LmName) & "', " & Str(Val(vDET_LmIdNo)) & ", '" & Trim(cbo_WidthType.Text) & "' , " & Str(Val(SNo)) & ", '" & Trim(dgv_Details.Rows(I).Cells(dgvCol_Details.PCSNO).Value) & "' , '" & Trim(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.PCSNO).Value)) & "' ,  " & Str(Val(vOrdByPcsNo)) & " , " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value)) & " , " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value)) & " , " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value)) & " , " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.PCSTYPE1).Value)) & " ,   " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.PCSTYPE2).Value)) & " ,    " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.PCSTYPE3).Value)) & " , " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.PCSTYPE4).Value)) & " ,   " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.PCSTYPE5).Value)) & " ,  " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value)) & ", " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.EXCESS_SHORT_MTRS).Value)) & ",   " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.WEIGHT).Value)) & ",   " & Str(Val(dgv_Details.Rows(I).Cells(dgvCol_Details.WEIGHTPERMETER).Value)) & " ,      1       ,    ''  ,               ''      ,              ''       ,            ''         ,            ''         ,           ''          ,            ''        ,          ''          ,             ''       ,            ''        ,          ''           ,            ''            ,          ''              ,             ''           ,            ''            ,          ''                    ,   '" & Trim(cbo_weaving_job_no.Text) & "') "
                            cmd.ExecuteNonQuery()

                        End If

                    End If

                Next I


            End With


            '---------------------------------------------------------------------------------------------------------------------------------------------
            '-------Stock Posting
            '---------------------------------------------------------------------------------------------------------------------------------------------

            EntID = Trim(Pk_Condition) & Trim(lbl_LotNo.Text)
            Partcls = "CloRcpt : LotNo. " & Trim(lbl_LotNo.Text)
            'If Trim(txt_PDcNo.Text) <> "" Then
            '    Partcls = Trim(Partcls) & ",  P.Dc.No : " & Trim(txt_PDcNo.Text)
            'End If
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1391" Then
                Partcls = ""
                Partcls = "CloRcpt : LotNo. " & Trim(lbl_LotNo.Text)
                'If Trim(txt_PDcNo.Text) <> "" Then
                '    Partcls = Trim(Partcls) & ",  P.Dc.No : " & Trim(txt_PDcNo.Text)
                'End If
                Partcls = Trim(Partcls) & ",  Cloth : " & Trim(cbo_Cloth.Text)
            End If

            PBlNo = Trim(lbl_LotNo.Text)

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            'Dim vPvuStock_In As String = ""

            'vPvuStock_In = ""

            'Da = New SqlClient.SqlDataAdapter("Select * from EndsCount_Head Where EndsCount_IdNo = " & Str(Val(vEndct_IdNo)), Con)
            'Da.SelectCommand.Transaction = tr
            'dt2 = New DataTable
            'Da.Fill(dt2)
            'If dt2.Rows.Count > 0 Then
            '    vPvuStock_In = Dt2.Rows(0)("Stock_In").ToString
            'End If
            'dt2.Clear()

            'If Trim(UCase(vPvuStock_In)) = "PCS" Then
            '    'lbl_ConsPavu.Text = Val(txt_NoOfPcs.Text)
            'End If



            vDelv_ID = 0 : vRec_ID = 0
            If Trim(UCase(Led_type)) = "JOBWORKER" Then
                vDelv_ID = vLed_IdNo
                vRec_ID = 0

            Else

                vDelv_ID = 0
                vRec_ID = vLed_IdNo

            End If


            '-------Pavu Stock Posting
            If txt_ConsPavu.Visible = True And Val(txt_ConsPavu.Text) <> 0 Then

                cmd.CommandText = "Insert into Stock_Pavu_Processing_Details (      Reference_Code   ,              Company_IdNo        ,          Reference_No         ,             for_OrderBy   , Reference_Date,       DeliveryTo_Idno     ,      ReceivedFrom_Idno   ,         Cloth_Idno         ,        Entry_ID      ,        Party_Bill_No ,       Particulars      ,         Sl_No        ,           EndsCount_IdNo     , Sized_Beam,                Meters             ,Weaving_JobCode_forSelection   ) " &
                                "           Values                       ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_LotNo.Text) & "', " & Str(Val(vOrdByNo)) & ",    @EntryDate , " & Str(Val(vDelv_ID)) & ", " & Str(Val(vRec_ID)) & ", " & Str(Val(vClo_IdNo)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(SNo)) & ", " & Str(Val(vEndct_IdNo)) & ",      0    , " & Str(Val(txt_ConsPavu.Text)) & " ,'" & Trim(cbo_weaving_job_no.Text) & "' ) "
                cmd.ExecuteNonQuery()

            End If


            '-------Yarn Stock Posting
            If txt_ConsYarn.Visible = True And Val(txt_ConsYarn.Text) <> 0 Then

                cmd.CommandText = "Insert into Stock_Yarn_Processing_Details (      Reference_Code   ,                Company_IdNo      ,            Reference_No       ,           for_OrderBy     , Reference_Date,        DeliveryTo_Idno    ,       ReceivedFrom_Idno  ,        Entry_ID      ,       Particulars      ,      Party_Bill_No   , Sl_No,          Count_IdNo        , Yarn_Type, Mill_IdNo, Bags, Cones,               Weight             ,Weaving_JobCode_forSelection    ) " &
                                "           Values                       ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_LotNo.Text) & "', " & Str(Val(vOrdByNo)) & ",    @EntryDate , " & Str(Val(vDelv_ID)) & ", " & Str(Val(vRec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "',   1  , " & Str(Val(WftCnt_ID)) & ",  'MILL'  ,     0    ,   0 ,   0  , " & Str(Val(txt_ConsYarn.Text)) & " ,'" & Trim(cbo_weaving_job_no.Text) & "' ) "
                cmd.ExecuteNonQuery()

            End If



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

            cmd.CommandText = "Insert into Stock_Cloth_Processing_Details (       Reference_Code   ,             Company_IdNo         ,             Reference_No      ,         for_OrderBy       , Reference_Date,             StockOff_IdNo        ,        DeliveryTo_Idno        ,       ReceivedFrom_Idno       ,         Entry_ID     ,      Party_Bill_No   ,      Particulars       , Sl_No,          Cloth_Idno        ,                  Folding           ,        UnChecked_Meters  ,      Meters_Type1            ,         Meters_Type2         ,          Meters_Type3         ,          Meters_Type4        ,        Meters_Type5           ,Weaving_JobCode_forSelection ) " &
                                "           Values                        ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_LotNo.Text) & "', " & Str(Val(vOrdByNo)) & ",   @EntryDate  , " & Str(Val(vStkOf_Pos_IdNo)) & ", " & Str(Val(StkDelvTo_ID)) & ", " & Str(Val(StkRecFrm_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',   1  , " & Str(Val(vClo_IdNo)) & ",  " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(UC_Mtrs)) & ", " & Str(Val(vTotTyp1Mtr)) & ", " & Str(Val(vTotTyp2Mtr)) & ",  " & Str(Val(vTotTyp3Mtr)) & ", " & Str(Val(vTotTyp4Mtr)) & ", " & Str(Val(vTotTyp5Mtr)) & " ,'" & Trim(cbo_weaving_job_no.Text) & "' ) "
            cmd.ExecuteNonQuery()

            If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Yarn_Stock) = 1 Then

                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno      , Count_IdNo, Yarn_Type, Mill_IdNo ) " &
                                          " Select                               'YARN'    , Reference_Code, Reference_Date, Company_IdNo, ReceivedFrom_Idno, Count_IdNo, Yarn_Type, Mill_IdNo from Stock_Yarn_Processing_Details where Reference_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1155" Then '----KRG TEXTILE MILLS (PALLADAM)
                    If Common_Procedures.Check_is_Negative_Stock_Status(Con, tr) = True Then Exit Sub
                End If

            End If


            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1155" Then '----KRG TEXTILE MILLS (PALLADAM)
                vErrMsg = ""
                If Common_Procedures.Cross_Checking_PieceChecking_PackingSlip_Meters(Con, Trim(NewCode), vErrMsg, tr) = False Then
                    Throw New ApplicationException(vErrMsg)
                    Exit Sub
                End If
            End If


            tr.Commit()

            If SaveAll_STS <> True Then
                MessageBox.Show("Saved Successfully", "FOR SAVING,.....", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            End If


            If New_Entry = False Then
                move_record(lbl_LotNo.Text)
            Else
                new_record()
            End If

        Catch ex As Exception
            tr.Rollback()

            Timer1.Enabled = False
            SaveAll_STS = False

            MessageBox.Show(ex.Message, "DOES NOT SAVE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            cmd.Dispose()
            tr.Dispose()

            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        End Try

    End Sub

    Private Sub cbo_Cloth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Cloth.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
    End Sub

    Private Sub cbo_Cloth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Cloth.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Cloth, cbo_Weaver, cbo_EndsCount, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
    End Sub

    Private Sub cbo_Cloth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Cloth.KeyPress
        Dim EndscntIdno As Integer
        Dim WfCntNo As Integer
        Dim Clo_IdNo As Integer

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Cloth, Nothing, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            Clo_IdNo = Common_Procedures.Cloth_NameToIdNo(Con, cbo_Cloth.Text)

            WfCntNo = Val(Common_Procedures.get_FieldValue(Con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_IdNo)) & ")"))
            txt_WeftCount.Text = Common_Procedures.Count_IdNoToName(Con, WfCntNo)

            'If Trim(cbo_EndsCount.Text) = "" Then
            EndscntIdno = Val(Common_Procedures.get_FieldValue(Con, "Cloth_EndsCount_Details", "EndsCount_IdNo", "(cloth_idno = " & Str(Val(Clo_IdNo)) & " and Sl_No = 1 )"))
            cbo_EndsCount.Text = Common_Procedures.EndsCount_IdNoToName(Con, EndscntIdno)
            'End If

            If cbo_EndsCount.Enabled And cbo_EndsCount.Visible Then cbo_EndsCount.Focus()
        End If

    End Sub

    Private Sub cbo_Cloth_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Cloth.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Cloth.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If

    End Sub

    Private Sub cbo_EndsCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EndsCount.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
    End Sub

    Private Sub cbo_EndsCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EndsCount.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_EndsCount, cbo_Cloth, Nothing, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
        If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If cbo_weaving_job_no.Visible = True Then
                cbo_weaving_job_no.Focus()
            Else
                txt_Folding.Focus()
                'txt_WeftCount.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_EndsCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_EndsCount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_EndsCount, Nothing, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            If cbo_weaving_job_no.Visible = True Then
                cbo_weaving_job_no.Focus()

            Else
                txt_Folding.Focus()
                'txt_WeftCount.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_EndsCount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EndsCount.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            e.Handled = True
            Dim f As New EndsCount_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_EndsCount.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub Cbo_LoomType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_LoomType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "", "", "", "")
    End Sub

    Private Sub Cbo_LoomType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Cbo_LoomType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, Cbo_LoomType, msk_date, cbo_Weaver, "", "", "", "")
    End Sub

    Private Sub Cbo_LoomType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_LoomType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, Cbo_LoomType, cbo_Weaver, "", "", "", "")
    End Sub

    Private Sub cbo_Weaver_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Weaver.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER'  or Ledger_Type = 'GODOWN'  or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Weaver_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Weaver.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Weaver, Cbo_LoomType, cbo_Cloth, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER'  or Ledger_Type = 'GODOWN'  or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Weaver_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Weaver.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Weaver, cbo_Cloth, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER'  or Ledger_Type = 'GODOWN'  or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Weaver_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Weaver.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            e.Handled = True

            Common_Procedures.MDI_LedType = "WEAVER"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Weaver.Name
            Common_Procedures.Master_Return.Master_Type = ""
            Common_Procedures.Master_Return.Return_Value = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub txt_ConsPavu_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_ConsPavu.KeyDown
        If e.KeyValue = 38 Then e.Handled = True : SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then
            e.Handled = True
            If MessageBox.Show("Do you want to Save ?", "FOR SAVE", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
                Exit Sub
            End If
        End If
    End Sub

    Private Sub txt_ConsPavu_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ConsPavu.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            If MessageBox.Show("Do you want to Save ?", "FOR SAVE", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
                Exit Sub
            End If
        End If
    End Sub


    Private Sub txt_ConsYarn_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_ConsYarn.KeyDown
        If e.KeyCode = 38 Then
            e.Handled = True
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.PCSNO)
                dgv_Details.CurrentCell.Selected = True
            End If
        End If
        If e.KeyCode = 40 Then
            txt_ConsPavu.Focus()
        End If
    End Sub

    Private Sub txt_ConsYarn_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ConsYarn.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            txt_ConsPavu.Focus()
        End If
    End Sub

    Private Sub msk_date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            If Cbo_LoomType.Enabled Then
                Cbo_LoomType.Focus()
            ElseIf cbo_Weaver.Enabled Then
                cbo_Weaver.Focus()
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
            If Cbo_LoomType.Enabled Then
                Cbo_LoomType.Focus()
            ElseIf cbo_Weaver.Enabled Then
                cbo_Weaver.Focus()
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
        If e.KeyCode = 38 Then 'e.Handled = True : SendKeys.Send("+{TAB}")
            If cbo_weaving_job_no.Visible = True Then
                cbo_weaving_job_no.Focus()
            Else
                cbo_EndsCount.Focus()

            End If

        End If
        If e.KeyCode = 40 Then

            e.Handled = True
            e.SuppressKeyPress = True

            If cbo_LoomNo.Visible = True And cbo_LoomNo.Enabled = True Then
                cbo_LoomNo.Focus()
            ElseIf cbo_WidthType.Visible = True And cbo_WidthType.Enabled = True Then
                cbo_WidthType.Focus()
            ElseIf txt_Manual_LotNo.Visible = True And txt_Manual_LotNo.Enabled = True Then
                txt_Manual_LotNo.Focus()
            ElseIf cbo_StockOff.Visible = True And cbo_StockOff.Enabled = True Then
                cbo_StockOff.Focus()
            ElseIf cbo_Godown_StockIN.Visible = True And cbo_Godown_StockIN.Enabled = True Then
                cbo_Godown_StockIN.Focus()
            Else
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.PCSNO)
                    dgv_Details.CurrentCell.Selected = True
                Else
                    txt_ConsYarn.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub txt_Folding_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Folding.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If cbo_LoomNo.Visible = True And cbo_LoomNo.Enabled = True Then
                cbo_LoomNo.Focus()
            ElseIf cbo_WidthType.Visible = True And cbo_WidthType.Enabled = True Then
                cbo_WidthType.Focus()
            ElseIf txt_Manual_LotNo.Visible = True And txt_Manual_LotNo.Enabled = True Then
                txt_Manual_LotNo.Focus()
            ElseIf cbo_StockOff.Visible = True And cbo_StockOff.Enabled = True Then
                cbo_StockOff.Focus()
            ElseIf cbo_Godown_StockIN.Visible = True And cbo_Godown_StockIN.Enabled = True Then
                cbo_Godown_StockIN.Focus()
            Else
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.PCSNO)
                    dgv_Details.CurrentCell.Selected = True
                Else
                    txt_ConsYarn.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub txt_LotNoCaption_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Manual_LotNo.KeyDown
        'If e.KeyCode = 38 Then e.Handled = True : SendKeys.Send("+{TAB}")
        'If e.KeyValue = 40 Then
        '    e.Handled = True
        '    If dgv_Details.Rows.Count > 0 Then
        '        dgv_Details.Focus()
        '        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(0)
        '        dgv_Details.CurrentCell.Selected = True
        '    Else
        '        txt_TotalMeters.Focus()
        '    End If
        'End If
    End Sub

    Private Sub txt_LotNoCaption_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Manual_LotNo.KeyPress
        'If Common_Procedures.Accept_AlphaNumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        'If Asc(e.KeyChar) = 13 Then
        '    If dgv_Details.Rows.Count > 0 Then
        '        dgv_Details.Focus()
        '        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
        '        dgv_Details.CurrentCell.Selected = True
        '    Else
        '        txt_TotalMeters.Focus()
        '    End If
        'End If
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
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Rect As Rectangle

        With dgv_Details
            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
            dgv_ActCtrlName = .Name
            If Val(.Rows(e.RowIndex).Cells(dgvCol_Details.SNO).Value) = 0 Then
                .Rows(e.RowIndex).Cells(dgvCol_Details.SNO).Value = e.RowIndex + 1
            End If

            If e.ColumnIndex = dgvCol_Details.LOOMNO And dgv_Details.Columns(dgvCol_Details.LOOMNO).ReadOnly = True Then

                If Cbo_Grid_LoomNo.Visible = False Or Val(Cbo_Grid_LoomNo.Tag) <> e.RowIndex Then

                    Cbo_Grid_LoomNo.Tag = -100
                    Da = New SqlClient.SqlDataAdapter("select Loom_Name from Loom_Head order by Loom_Name", Con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    Cbo_Grid_LoomNo.DataSource = Dt1
                    Cbo_Grid_LoomNo.DisplayMember = "Loom_Name"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    Cbo_Grid_LoomNo.Left = .Left + Rect.Left
                    Cbo_Grid_LoomNo.Top = .Top + Rect.Top

                    Cbo_Grid_LoomNo.Width = Rect.Width
                    Cbo_Grid_LoomNo.Height = Rect.Height
                    Cbo_Grid_LoomNo.Text = .CurrentCell.Value

                    Cbo_Grid_LoomNo.Tag = Val(e.RowIndex)
                    Cbo_Grid_LoomNo.Visible = True

                    Cbo_Grid_LoomNo.BringToFront()
                    Cbo_Grid_LoomNo.Focus()

                Else

                    'If cbo_Grid_ClothName.Visible = True Then
                    '    cbo_Grid_ClothName.BringToFront()
                    '    cbo_Grid_ClothName.Focus()
                    'End If

                End If

            Else
                Cbo_Grid_LoomNo.Visible = False

            End If

        End With

    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Dim TtMrs_in_100Fld As String = 0
        Dim vFldPerc As String = 0
        Dim vLess_FldPerc As String = 0
        Dim vFlding_Value As String = 0

        Try

            If FrmLdSts = True Then Exit Sub

            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

            With dgv_Details

                If .Visible Then

                    If .CurrentCell.ColumnIndex = dgvCol_Details.RECEIPT_DOFF_MTRS Or .CurrentCell.ColumnIndex = dgvCol_Details.PCSTYPE1 Or .CurrentCell.ColumnIndex = dgvCol_Details.PCSTYPE2 Or .CurrentCell.ColumnIndex = dgvCol_Details.PCSTYPE3 Or .CurrentCell.ColumnIndex = dgvCol_Details.PCSTYPE4 Or .CurrentCell.ColumnIndex = dgvCol_Details.PCSTYPE5 Or .CurrentCell.ColumnIndex = dgvCol_Details.EXCESS_SHORT_MTRS Or .CurrentCell.ColumnIndex = dgvCol_Details.WEIGHT Then

                        If .Columns(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Visible = True Then
                            .Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Value = Format(Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value) + Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.EXCESS_SHORT_MTRS).Value), "#########0.00")

                            .Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.PCSTYPE1).Value = Format(Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Value), "#########0.00")
                            .Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value = Format(Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Value), "#########0.00")

                        Else

                            .Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value = Format(Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.PCSTYPE1).Value) + Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.PCSTYPE2).Value) + Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.PCSTYPE3).Value) + Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.PCSTYPE4).Value) + Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.PCSTYPE5).Value), "##########0.00")
                            .Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Value = Format(Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value) + Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value), "#########0.00")

                        End If


                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1578" Then


                            If Val(txt_Folding_Receipt.Text) = Val(txt_Folding.Text) Then ' ---------for bale


                                '-------- if folding Percentage of checking and reecipt it is considered as bale 


                                vFldPerc = Val(txt_Folding.Text)
                                If Val(vFldPerc) = 0 Then vFldPerc = 100

                                .Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.PCSTYPE1).Value = Format(Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value) * Val(vFldPerc) / Val(txt_Folding.Text), "########0.00")


                            Else

                                vFldPerc = Val(txt_Folding.Text)
                                If Val(vFldPerc) = 0 Then vFldPerc = 100      ' ROLL

                                vLess_FldPerc = "3.85"

                                vFlding_Value = Format(Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value) * Val(vLess_FldPerc) / Val(vFldPerc), "########0.00")

                                .Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.PCSTYPE1).Value = Format(Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value) + Val(vFlding_Value), "########0")


                            End If


                        End If


                        vFldPerc = Val(txt_Folding.Text)
                        If Val(vFldPerc) = 0 Then vFldPerc = 100
                        TtMrs_in_100Fld = Format(Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value) * Val(vFldPerc) / 100, "########0.00")

                        If .Columns(dgvCol_Details.EXCESS_SHORT_MTRS).ReadOnly = True Then

                            .Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.EXCESS_SHORT_MTRS).Value = Format(Val(TtMrs_in_100Fld) - Val(.Rows(.CurrentCell.RowIndex).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value), "#########0.00")

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
                If dgv_Details.CurrentCell.ColumnIndex <> dgvCol_Details.PCSNO Then
                    If Common_Procedures.Accept_AlphaNumericOnly(Asc(e.KeyChar)) = 0 Then
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
        Dim TotPcs As String = 0

        If FrmLdSts = True Then Exit Sub

        vFldPerc = Val(txt_Folding.Text)
        If Val(vFldPerc) = 0 Then vFldPerc = 100

        ReceiptMtr = 0 : total_Mtrs = 0 : Excess_shot_Mtr = 0 : wgt_mtr = 0 : tot_weight = 0

        With dgv_Details


            For i = 0 To .RowCount - 1

                sno = sno + 1

                .Rows(i).Cells(dgvCol_Details.SNO).Value = sno

                If Trim(.Rows(i).Cells(dgvCol_Details.PCSNO).Value) <> "" Or Val(.Rows(i).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.PCSTYPE1).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.PCSTYPE2).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.PCSTYPE3).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.PCSTYPE4).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.PCSTYPE5).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.EXCESS_SHORT_MTRS).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Value) <> 0 Or Val(.Rows(i).Cells(dgvCol_Details.WEIGHT).Value) <> 0 Then

                    TotPcs = TotPcs + 1

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

        End With


        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then '---- Kalaimagal Textiles (Palladam)

                .Rows(0).Cells(dgvCol_Details.PCSNO).Value = Val(TotPcs)

                .Rows(0).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value = ""
                .Rows(0).Cells(dgvCol_Details.PCSTYPE1).Value = Format(Val(tot_sound_Mtr), "########0.0")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE2).Value = Format(Val(tot_sec_Mtr), "########0.0")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE3).Value = Format(Val(tot_bit_Mtr), "########0.0")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE4).Value = Format(Val(tot_rej_Mtr), "########0.0")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE5).Value = Format(Val(tot_othr_Mtr), "########0.0")
                .Rows(0).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value = Format(Val(total_Mtrs), "########0.0")
                .Rows(0).Cells(dgvCol_Details.EXCESS_SHORT_MTRS).Value = ""
                .Rows(0).Cells(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Value = Format(Val(total_Mtrs), "########0.0")
                .Rows(0).Cells(dgvCol_Details.WEIGHT).Value = Format(Val(tot_weight), "########0.000")

                .Rows(0).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value = ""
                .Rows(0).Cells(dgvCol_Details.PCSTYPE1).Value = Format(Val(.Rows(0).Cells(dgvCol_Details.PCSTYPE1).Value), "########0.00")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE2).Value = Format(Val(.Rows(0).Cells(dgvCol_Details.PCSTYPE2).Value), "########0.00")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE3).Value = Format(Val(.Rows(0).Cells(dgvCol_Details.PCSTYPE3).Value), "########0.00")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE4).Value = Format(Val(.Rows(0).Cells(dgvCol_Details.PCSTYPE4).Value), "########0.00")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE5).Value = Format(Val(.Rows(0).Cells(dgvCol_Details.PCSTYPE5).Value), "########0.00")
                .Rows(0).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value = Format(Val(.Rows(0).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value), "########0.00")
                .Rows(0).Cells(dgvCol_Details.EXCESS_SHORT_MTRS).Value = ""
                .Rows(0).Cells(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Value = Format(Val(.Rows(0).Cells(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Value), "########0.00")
                .Rows(0).Cells(dgvCol_Details.WEIGHT).Value = Format(Val(.Rows(0).Cells(dgvCol_Details.WEIGHT).Value), "########0.000")

            Else
                .Rows(0).Cells(dgvCol_Details.PCSNO).Value = Val(TotPcs)

                .Rows(0).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value = ""
                .Rows(0).Cells(dgvCol_Details.PCSTYPE1).Value = Format(Val(tot_sound_Mtr), "########0.00")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE2).Value = Format(Val(tot_sec_Mtr), "########0.00")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE3).Value = Format(Val(tot_bit_Mtr), "########0.00")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE4).Value = Format(Val(tot_rej_Mtr), "########0.00")
                .Rows(0).Cells(dgvCol_Details.PCSTYPE5).Value = Format(Val(tot_othr_Mtr), "########0.00")
                .Rows(0).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value = Format(Val(total_Mtrs), "########0.00")
                .Rows(0).Cells(dgvCol_Details.EXCESS_SHORT_MTRS).Value = ""
                .Rows(0).Cells(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Value = Format(Val(total_Mtrs), "########0.00")
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
                .Rows(0).Cells(dgvCol_Details.EXCESS_SHORT_MTRS).Value = Format(Val(Excess_shot_Mtr), "########0.00")
                .Rows(0).Cells(dgvCol_Details.TOTALRECEIPT_EXCESSSHORT_MTRS).Value = Format(Val(.Rows(0).Cells(dgvCol_Details.PCSTYPE1).Value) + Val(.Rows(0).Cells(dgvCol_Details.PCSTYPE2).Value) + Val(.Rows(0).Cells(dgvCol_Details.PCSTYPE3).Value) + Val(.Rows(0).Cells(dgvCol_Details.PCSTYPE4).Value) + Val(.Rows(0).Cells(dgvCol_Details.PCSTYPE5).Value), "########0.00")

            End If

            .Rows(0).Cells(dgvCol_Details.WEIGHT).Value = ""
            .Rows(0).Cells(dgvCol_Details.WEIGHTPERMETER).Value = ""

        End With

        'Calculation_Pavu_Consumed()
        'Calculation_Yarn_Consumed()

    End Sub


    Private Sub cbo_LoomNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_LoomNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0)")
    End Sub

    Private Sub cbo_LoomNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_LoomNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_LoomNo, txt_Folding, cbo_WidthType, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0)")
    End Sub

    Private Sub cbo_LoomNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_LoomNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_LoomNo, cbo_WidthType, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0)")
    End Sub

    Private Sub cbo_LoomNo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_LoomNo.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New LoomNo_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_LoomNo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_WidthType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_WidthType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub

    Private Sub cbo_WidthType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_WidthType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_WidthType, cbo_LoomNo, Nothing, "", "", "", "")
        If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            e.Handled = True
            If cbo_StockOff.Visible And cbo_StockOff.Enabled Then
                cbo_StockOff.Focus()
            ElseIf cbo_Godown_StockIN.Visible And cbo_Godown_StockIN.Enabled Then
                cbo_Godown_StockIN.Focus()
            ElseIf txt_Folding_Receipt.Visible = True Then
                txt_Folding_Receipt.Focus()
            ElseIf dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.PCSNO)
                dgv_Details.CurrentCell.Selected = True

            Else
                txt_ConsYarn.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_WidthType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_WidthType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_WidthType, Nothing, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            If cbo_StockOff.Visible And cbo_StockOff.Enabled Then
                cbo_StockOff.Focus()
            ElseIf cbo_Godown_StockIN.Visible And cbo_Godown_StockIN.Enabled Then
                cbo_Godown_StockIN.Focus()
            ElseIf txt_Folding_Receipt.Visible = True Then
                txt_Folding_Receipt.Focus()
            ElseIf dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.PCSNO)
                dgv_Details.CurrentCell.Selected = True


            Else
                txt_ConsYarn.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_StockOff_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_StockOff.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'OWNSORT' or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1) and Close_status = 0 ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_StockOff_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_StockOff.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_StockOff, Nothing, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'OWNSORT' or Ledger_Type = 'GODOWN'  or Ledger_Type = 'JOBWORKER'  or Show_In_All_Entry = 1 ) and Close_status = 0 ", "(Ledger_IdNo = 0)")

        If (e.KeyValue = 38 And cbo_StockOff.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then e.Handled = True : SendKeys.Send("+{TAB}")

        If (e.KeyValue = 40 And cbo_StockOff.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            e.Handled = True
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.PCSNO)
                dgv_Details.CurrentCell.Selected = True
            Else
                txt_ConsYarn.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_StockOff_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_StockOff.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_StockOff, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'OWNSORT' or Ledger_Type = 'GODOWN'  or Ledger_Type = 'JOBWORKER'  or Show_In_All_Entry = 1 ) and Close_status = 0 ", "(Ledger_IdNo = 0)")

        If Common_Procedures.Accept_AlphaNumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.PCSNO)
                dgv_Details.CurrentCell.Selected = True
            Else
                txt_ConsYarn.Focus()
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

    Private Sub cbo_Godown_StockIN_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Godown_StockIN.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1) and Close_status = 0 ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Godown_StockIN_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Godown_StockIN.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_Godown_StockIN, txt_Folding, cbo_LoomNo, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1) and Close_status = 0 )", "(Ledger_IdNo = 0)")


    End Sub

    Private Sub cbo_Godown_StockIN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Godown_StockIN.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_Godown_StockIN, cbo_LoomNo, "Ledger_AlaisHead", "Ledger_DisplayName", "( (Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Godown_StockIN_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Godown_StockIN.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            Common_Procedures.MDI_LedType = "GODOWN"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Godown_StockIN.Name
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

        txt_ConsPavu.Text = 0
        If txt_ConsPavu.Visible = False Then Exit Sub

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
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1612" Then '---- ISHANVI TEX (ERODE)
            ConsPavu = vTotRcptMtr
            If Val(ConsPavu) = 0 Then ConsPavu = Val(vTot_ChkMtrs)

        ElseIf Trim(UCase(Cbo_LoomType.Text)) = "AUTOLOOM" Or Trim(UCase(Cbo_LoomType.Text)) = "AUTO LOOM" Then

            CloID = Common_Procedures.Cloth_NameToIdNo(Con, cbo_Cloth.Text)
            LmID = Common_Procedures.Loom_NameToIdNo(Con, cbo_LoomNo.Text)
            vWdthType = Trim(cbo_WidthType.Text)

            vMtrs = Val(vTot_ChkMtrs)
            If Val(vMtrs) = 0 Then vMtrs = Val(vTotRcptMtr)

            ConsPavu = Common_Procedures.get_Pavu_Consumption(Con, CloID, LmID, Val(vMtrs), vWdthType)

        Else

            ConsPavu = vTotRcptMtr
            If Val(ConsPavu) = 0 Then ConsPavu = Val(vTot_ChkMtrs)

        End If

        txt_ConsPavu.Text = Format(Val(ConsPavu), "###########0.00")

    End Sub

    Private Sub Calculation_Yarn_Consumed()
        Dim CloID As Integer
        Dim ConsYarn As Single
        Dim vTotRcptMtr As String = 0
        Dim vTot_ChkMtrs As String = 0
        Dim vMtrs As String = 0
        Dim vFldg As String = 0

        txt_ConsYarn.Text = 0
        If txt_ConsYarn.Visible = False Then Exit Sub

        'If Common_Procedures.settings.JobWorker_Pavu_Yarn_Stock_Posting_IN_Production = 0 Then
        '    txt_ConsYarn.Text = ""
        '    Exit Sub
        'End If


        vTotRcptMtr = 0 : vTot_ChkMtrs = 0
        If dgv_Details_Total2.RowCount > 0 Then
            vTotRcptMtr = dgv_Details_Total2.Rows(0).Cells(dgvCol_Details.RECEIPT_DOFF_MTRS).Value
            vTot_ChkMtrs = dgv_Details_Total2.Rows(0).Cells(dgvCol_Details.TOTALCHECKINGMTRS).Value
        End If

        CloID = Common_Procedures.Cloth_NameToIdNo(Con, cbo_Cloth.Text)

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1612" Then '---- ISHANVI TEX (ERODE)
            vMtrs = Val(vTotRcptMtr)
            If Val(vMtrs) = 0 Then vMtrs = Val(vTot_ChkMtrs)

        Else
            'If Trim(UCase(Cbo_LoomType.Text)) = "AUTOLOOM" Or Trim(UCase(Cbo_LoomType.Text)) = "AUTO LOOM" Then
            vMtrs = Val(vTot_ChkMtrs)
            If Val(vMtrs) = 0 Then vMtrs = Val(vTotRcptMtr)
            'Else
            '    vMtrs = Val(vTotRcptMtr)

            'End If

        End If


        ConsYarn = Common_Procedures.get_Weft_ConsumedYarn(Con, CloID, Val(vMtrs))

        txt_ConsYarn.Text = Format(ConsYarn, "#########0.000")

    End Sub

    Private Sub Cbo_LoomType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_LoomType.TextChanged

        If Trim(UCase(Cbo_LoomType.Text)) = "POWERLOOM" Then '---- Ashoka Textile (63.Velampalayam - Palladam)
            cbo_LoomNo.Enabled = False
            cbo_WidthType.Enabled = False

        Else

            cbo_LoomNo.Enabled = True
            cbo_WidthType.Enabled = True

        End If

        'Calculation_Pavu_Consumed()
        'Calculation_Yarn_Consumed()

    End Sub

    Private Sub cbo_Weaver_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Weaver.TextChanged
        'Calculation_Pavu_Consumed()
        'Calculation_Yarn_Consumed()
    End Sub

    Private Sub cbo_WidthType_TextChanged(sender As Object, e As System.EventArgs) Handles cbo_WidthType.TextChanged
        'Calculation_Pavu_Consumed()
        'Calculation_Yarn_Consumed()
    End Sub

    Private Sub btn_CalculateConsumption_Click(sender As Object, e As EventArgs) Handles btn_CalculateConsumption.Click
        Calculation_Pavu_Consumed()
        Calculation_Yarn_Consumed()
    End Sub

    Private Sub cbo_Godown_StockIN_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Godown_StockIN.SelectedIndexChanged

    End Sub

    Private Sub cbo_StockOff_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_StockOff.SelectedIndexChanged

    End Sub

    Private Sub cbo_WidthType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_WidthType.SelectedIndexChanged

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

        LastNo = lbl_LotNo.Text

        movefirst_record()
        Timer1.Enabled = True

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        save_record()
        If Trim(UCase(LastNo)) = Trim(UCase(lbl_LotNo.Text)) Then
            Timer1.Enabled = False
            SaveAll_STS = False
            MessageBox.Show("All entries saved sucessfully", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Else
            movenext_record()

        End If
    End Sub
    Private Sub cbo_weaving_job_no_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_weaving_job_no.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, cbo_weaving_job_no, cbo_EndsCount, txt_Folding, "Weaving_JobCard_Head", "Weaving_JobCode_forSelection", "", "(Ledger_IdNo = 0)")

        'If (e.KeyCode = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

        'End If
    End Sub
    Private Sub cbo_weaving_job_no_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_weaving_job_no.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, cbo_weaving_job_no, txt_Folding, "Weaving_JobCard_Head", "Weaving_JobCode_forSelection", "", "(Ledger_IdNo = 0)")

    End Sub
    Private Sub cbo_weaving_job_no_GotFocus(sender As Object, e As EventArgs) Handles cbo_weaving_job_no.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, Con, "Weaving_JobCard_Head", "Weaving_JobCode_forSelection", "", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_EndsCount_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_EndsCount.SelectedIndexChanged

    End Sub

    Private Sub txt_WeftCount_TextChanged(sender As Object, e As EventArgs) Handles txt_WeftCount.TextChanged

    End Sub

    Private Sub lbl_LotNo_Caption_Click(sender As Object, e As EventArgs) Handles lbl_LotNo_Caption.Click

    End Sub

    Private Sub cbo_LoomNo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_LoomNo.SelectedIndexChanged

    End Sub

    Private Sub Cbo_Grid_LoomNo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Cbo_Grid_LoomNo.SelectedIndexChanged

    End Sub

    Private Sub Cbo_Grid_LoomNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Cbo_Grid_LoomNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, Con, Cbo_Grid_LoomNo, Nothing, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            With dgv_Details
                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If



    End Sub

    Private Sub Cbo_Grid_LoomNo_KeyDown(sender As Object, e As KeyEventArgs) Handles Cbo_Grid_LoomNo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, Con, Cbo_Grid_LoomNo, Nothing, Nothing, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0)")

        With dgv_Details

            If (e.KeyValue = 38 And Cbo_Grid_LoomNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex - 1)
            End If

            If (e.KeyValue = 40 And Cbo_Grid_LoomNo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
            End If

        End With

    End Sub

    Private Sub Cbo_Grid_LoomNo_KeyUp(sender As Object, e As KeyEventArgs) Handles Cbo_Grid_LoomNo.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New LoomNo_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_LoomNo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub Cbo_Grid_LoomNo_TextChanged(sender As Object, e As EventArgs) Handles Cbo_Grid_LoomNo.TextChanged
        Try
            If Cbo_Grid_LoomNo.Visible Then
                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(Cbo_Grid_LoomNo.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 3 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(Cbo_Grid_LoomNo.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub lbl_Folding_Checking_Click(sender As Object, e As EventArgs) Handles lbl_Folding_Checking.Click

    End Sub

    Private Sub txt_Folding_Receipt_TextChanged(sender As Object, e As EventArgs) Handles txt_Folding_Receipt.TextChanged

    End Sub

    Private Sub txt_Folding_Receipt_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Folding_Receipt.KeyPress

        If Asc(e.KeyChar) = 13 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.PCSNO)
                dgv_Details.CurrentCell.Selected = True
            End If
        End If

    End Sub

    Private Sub txt_Folding_Receipt_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Folding_Receipt.KeyDown


        If e.KeyCode = 40 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCol_Details.PCSNO)
                dgv_Details.CurrentCell.Selected = True
            End If
        End If


        If e.KeyCode = 38 Then
            cbo_WidthType.Focus()
        End If

    End Sub

    Private Sub txt_Folding_LostFocus(sender As Object, e As EventArgs) Handles txt_Folding.LostFocus

    End Sub

    Private Sub txt_Folding_Leave(sender As Object, e As EventArgs) Handles txt_Folding.Leave
        If Val(txt_Folding.Text) = 0 Then txt_Folding.Text = 100
    End Sub

    Private Sub txt_Folding_Receipt_Leave(sender As Object, e As EventArgs) Handles txt_Folding_Receipt.Leave
        If Val(txt_Folding_Receipt.Text) = 0 Then txt_Folding_Receipt.Text = 100
    End Sub
End Class