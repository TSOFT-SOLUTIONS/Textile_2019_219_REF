Imports System.IO
Public Class Weaver_Piece_Checking
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Other_Condition As String = ""

    Private PkCondition_Entry As String = ""

    'Private Pk_Condition As String = "PCCHK-"

    Private PkCondition_Weaver As String = "WCLRC-"
    Private PkCondition_Purchase As String = "CPREC-"
    Private PkCondition_DelvRet As String = "CLDRT-"
    Private PkCondition_SalRetVAT As String = "CLSRT-"
    Private PkCondition_SalRetGST As String = "GCLSR-"
    Private PkCondition_PROFABRCPT As String = "FPRRC-"

    Private Print_PDF_Status As Boolean = False
    Private EMAIL_Status As Boolean = False
    Private WHATSAPP_Status As Boolean = False
    Private vEMAIL_Attachment_FileName As String

    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String
    Private vEntryType As String = ""
    Private vRcptType As String = ""

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private NoFo_STS As Integer = 0
    Dim Type1, Type2, Type3, Type4, Type5 As String
    Dim Type11, Type22, Type33, Type44, Type55 As String
    Dim vType1, vType2, vType3, vType4, vType5 As Single
    Dim vTotType1, vTotType2, vTotType3, vTotType4, vTotType5, vTotType23, vTotChck, vTotRecMtr As Single

    Private SaveAll_STS As Boolean = False
    Private LastNo As String = ""

    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Private vPrntOnly_PageNo As Integer = 0

    Private prn_DetBarCdStkr As Integer

    Private fs As FileStream
    Private sw As StreamWriter
    Private is_FOOTR_PRINT_PART1_STS As Boolean = False
    Private is_FOOTR_PRINT_FULLY_STS As Boolean = False

    Private vBARCDPRNT_PCSNO As String = ""
    Private vBARCDPRNT_COLNO As String = ""

    Private vWARP_WEFT_STOCK_UPDATION_STATUS As Boolean = False

    Private Enum dgvCOL_PCSDETAILS As Integer
        PCSNO                       '0
        RECEIPTMETER                '1
        LOOMNO                      '2
        PICK                        '3
        WIDTH                       '4
        TYPE1METER                  '5
        TYPE2METER                  '6
        TYPE3METER                  '7
        TYPE4METER                  '8
        TYPE5METER                  '9
        TOTALMETER                  '10
        TOTALMETERIN100FOLDING      '11
        EXCESSHORTSTATUSYESSORNO    '12
        EXCESSHORTMETER             '13
        WEIGHT                      '14
        WEIGHTPERMETER              '15
        PACKINGSLIPCODETYPE1        '16
        PACKINGSLIPCODETYPE2        '17
        PACKINGSLIPCODETYPE3        '18
        PACKINGSLIPCODETYPE4        '19
        PACKINGSLIPCODETYPE5        '20
        BEAMNO_SETCODE_FORSELECTION '21
        REMARKS                     '22
        CHECKERNAME                 '23
        FOLDERNAME                  '24
        CHECKERWAGESRATEPERMETER    '25
        FOLDERERWAGESRATEPERMETER   '26
    End Enum

    Private Enum dgvCOL_SELECTION As Integer
        SNO     '0
        RECEIPTNO   '1
        RECEIPTDATE     '2
        PARTYDCNO   '3
        CLOTHNAME   '4
        ENDSCOUNT   '5
        PCS         '6
        METERS      '7
        STS         '8
        WEAVERCLOTHRECEIPTCODE  '9
        FOLDING     '10
        RECEIPTPKCONDITION  '11
        PIECEFROMNUMBER     '12
        PIECETONUMBER       '13
    End Enum

    Private Enum dgvCOL_FILTERDETAILS As Integer
        SNO             '0
        CHECKINGNO      '1
        CHECKINGDATE    '2
        PARTYNAME       '3
        RECEIPTNO       '4
        CLOTHNAME       '5
        RECEIPTDATE     '6
        RECEIPTMETER    '7
        TOTALMETER      '8
    End Enum

    Public Sub New(ByVal EntryType As String)
        vEntryType = Trim(UCase(EntryType))
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub clear()
        chk_Verified_Status.Checked = False
        NoCalc_Status = True
        vWARP_WEFT_STOCK_UPDATION_STATUS = False
        New_Entry = False
        Insert_Entry = False
        pnl_CheckingDetails.Visible = False
        Print_PDF_Status = False
        EMAIL_Status = False
        WHATSAPP_Status = False
        vEMAIL_Attachment_FileName = ""
        pnl_Back.Enabled = True
        pnl_Print.Visible = False
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False
        cbo_Cloth_TransferTo.Text = ""
        lbl_ChkNo.Text = ""
        lbl_ChkNo.ForeColor = Color.Black

        msk_date.Text = ""
        dtp_Date.Text = ""

        Chk_Approved_Sts.Checked = False
        set_Approved_Status_Visibility()

        pnl_OpenRecord.Visible = False

        msk_date.Enabled = True
        dtp_Date.Enabled = True
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT TEXTTILES (SOMANUR)
            If Common_Procedures.Office_System_Status = False Then  '---[THANGES]  - REMOVE IT AFTER FINISHING THE Correcting Old Entries
                msk_date.Enabled = False
                dtp_Date.Enabled = False
            End If
        End If
        cbo_Weaver.Text = ""
        cbo_Quality.Text = ""
        Lbl_StockOff.Text = ""
        lbl_Godown.Text = ""
        cbo_LoomType.Text = ""

        txt_LotNo_Open.Text = ""
        txt_ChkNoOpen.Text = ""
        txt_filter_Chkno.Text = ""

        txt_Excess_Short.Text = ""
        txt_Folding.Text = "100"
        txt_PDcNo.Text = ""
        txt_No_Pcs.Text = ""
        txt_Rec_Meter.Text = ""
        txt_RecNo.Text = ""
        lbl_LotNo.Text = ""
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then '---- Kalaimagal Textiles (Palladam)
            lbl_LotNoCaption.Text = "REC.NO / LOTNO"
        Else
            lbl_LotNoCaption.Text = Common_Procedures.settings.ClothReceipt_LotNo_OR_RollNo_Text
        End If

        'lbl_UserName_CreatedBy.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        lbl_UserName_CreatedBy.Text = ""
        lbl_UserName_ModifiedBy.Text = ""
        lbl_UserName_ApprovedBy.Text = ""

        dgv_Details.Rows.Clear()
        dgv_Details_Total1.Rows.Clear()
        dgv_Details_Total1.Rows.Add()

        dgv_Details.Rows.Clear()
        dgv_Details_Total2.Rows.Clear()
        dgv_Details_Total2.Rows.Add()
        dgv_Details_Total2.Rows(0).Cells(dgvCOL_PCSDETAILS.PCSNO).Value = "100%"
        dgv_Details_Total2.Rows(0).Cells(dgvCOL_PCSDETAILS.RECEIPTMETER).Value = "FOLDING"

        cbo_Weaver.Enabled = True
        cbo_Weaver.BackColor = Color.White

        txt_Folding.Enabled = True
        txt_Folding.BackColor = Color.White

        btn_Selection.Enabled = True

        cbo_Cloth_TransferTo.Enabled = True
        cbo_Cloth_TransferTo.BackColor = Color.White

        cbo_Grid_CountName.Text = ""
        cbo_Grid_CountName.Visible = False

        cbo_Grid_RateFor.Text = ""
        cbo_Grid_RateFor.Visible = False

        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_ClothName.Text = ""
            cbo_Filter_ClothName.SelectedIndex = -1
            txt_Filter_LotNo.Text = ""
            dgv_Filter_Details.Rows.Clear()
        End If

        Grid_Cell_DeSelect()
        NoCalc_Status = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim Msktxbx As MaskedTextBox
        On Error Resume Next

        If FrmLdSTS = True Then Exit Sub

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is MaskedTextBox Or TypeOf Me.ActiveControl Is Button Then
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

        If Me.ActiveControl.Name <> cbo_Grid_CountName.Name Then
            cbo_Grid_CountName.Visible = False
        End If
        If Me.ActiveControl.Name <> cbo_Grid_RateFor.Name Then
            cbo_Grid_RateFor.Visible = False
        End If
        If Me.ActiveControl.Name <> dgv_Details.Name Then
            Grid_DeSelect()
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If FrmLdSTS = True Then Exit Sub

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.FromArgb(41, 57, 85)
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
        If FrmLdSTS = True Then Exit Sub
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total1.CurrentCell) Then dgv_Details_Total1.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total2.CurrentCell) Then dgv_Details_Total2.CurrentCell.Selected = False
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
        If Not IsNothing(dgv_Details_Total1.CurrentCell) Then dgv_Details_Total1.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total2.CurrentCell) Then dgv_Details_Total2.CurrentCell.Selected = False
        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub Weaver_Piece_Checking_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Weaver.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Weaver.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Quality.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Quality.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If
            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Cloth_TransferTo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Cloth_TransferTo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

                Me.Text = lbl_Heading.Text & "  -  " & lbl_Company.Text

                new_record()

            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Weaver_Piece_Checking_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        FrmLdSTS = True

        PkCondition_Entry = ""
        If Trim(UCase(vEntryType)) = "SALES" Then
            lbl_Heading.Text = "PIECE CHECKING (SALES)"
            PkCondition_Entry = "SPCCK-"
            Other_Condition = "(Receipt_Type = 'S')"
            vRcptType = "S"
            Me.BackColor = Color.LightGray

        ElseIf Trim(UCase(vEntryType)) = "PURCHASE" Then
            lbl_Heading.Text = "PIECE CHECKING (PURCHASE)"
            PkCondition_Entry = "PPCCK-"
            Other_Condition = "(Receipt_Type = 'P')"
            vRcptType = "P"
            'ElseIf Trim(UCase(vEntryType)) = "WEAVER" Then
            '    Label1.Text = "PIECE CHECKING (WEAVER)"
            '    PkCondition_Entry = ""
            Me.BackColor = Color.LightCyan

        ElseIf Trim(UCase(vEntryType)) = "PROCESS-RECEIPT" Then
            lbl_Heading.Text = "PIECE CHECKING (PROCESS RECEIPT)"
            PkCondition_Entry = "PRCCK-"
            Other_Condition = "(Receipt_Type = 'PR')"
            vRcptType = "PR"
            Me.BackColor = Color.AntiqueWhite
            'Me.BackColor = Color.WhiteSmoke

        Else
            lbl_Heading.Text = "PIECE CHECKING (WEAVER)"
            PkCondition_Entry = ""
            Other_Condition = "(Receipt_Type = '' or Receipt_Type = 'W')"
            vRcptType = "W"

        End If

        lbl_LotNoCaption.Text = StrConv(Common_Procedures.settings.ClothReceipt_LotNo_OR_RollNo_Text, vbProperCase)
        dgv_Details.Columns(dgvCOL_PCSDETAILS.TYPE1METER).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type1))
        dgv_Details.Columns(dgvCOL_PCSDETAILS.TYPE2METER).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type2))
        dgv_Details.Columns(dgvCOL_PCSDETAILS.TYPE3METER).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type3))
        dgv_Details.Columns(dgvCOL_PCSDETAILS.TYPE4METER).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type4))
        dgv_Details.Columns(dgvCOL_PCSDETAILS.TYPE5METER).HeaderText = Trim(UCase(Common_Procedures.ClothType.Type5))

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1234" Then   '---- ARULJOTHI EXPORTS PVT LTD (SOMANUR)

            dgv_Details.Columns(dgvCOL_PCSDETAILS.TOTALMETERIN100FOLDING).Visible = True
            dgv_Details.Columns(dgvCOL_PCSDETAILS.BEAMNO_SETCODE_FORSELECTION).Visible = True
            dgv_Details.Columns(dgvCOL_PCSDETAILS.EXCESSHORTMETER).Visible = True

            dgv_Details.Columns(dgvCOL_PCSDETAILS.TYPE5METER).Visible = False

            dgv_Details.Columns(dgvCOL_PCSDETAILS.RECEIPTMETER).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.RECEIPTMETER).Width - 10
            dgv_Details.Columns(dgvCOL_PCSDETAILS.TYPE1METER).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.TYPE1METER).Width - 10
            dgv_Details.Columns(dgvCOL_PCSDETAILS.TYPE2METER).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.TYPE2METER).Width - 10
            dgv_Details.Columns(dgvCOL_PCSDETAILS.TYPE3METER).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.TYPE3METER).Width - 10
            dgv_Details.Columns(dgvCOL_PCSDETAILS.TYPE4METER).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.TYPE4METER).Width - 5
            dgv_Details.Columns(dgvCOL_PCSDETAILS.TOTALMETER).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.TOTALMETER).Width - 5
            dgv_Details.Columns(dgvCOL_PCSDETAILS.WEIGHT).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.WEIGHT).Width - 5
            dgv_Details.Columns(dgvCOL_PCSDETAILS.WEIGHTPERMETER).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.WEIGHTPERMETER).Width - 5


            dgv_Details.Columns(dgvCOL_PCSDETAILS.TOTALMETERIN100FOLDING).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.TOTALMETER).Width
            dgv_Details.Columns(dgvCOL_PCSDETAILS.BEAMNO_SETCODE_FORSELECTION).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.TYPE5METER).Width
            dgv_Details.Columns(dgvCOL_PCSDETAILS.REMARKS).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.REMARKS).Width - dgv_Details.Columns(dgvCOL_PCSDETAILS.TOTALMETERIN100FOLDING).Width
            dgv_Details.Columns(dgvCOL_PCSDETAILS.EXCESSHORTMETER).Width = 60


            dgv_Details_Total1.Columns(dgvCOL_PCSDETAILS.TOTALMETERIN100FOLDING).Visible = True
            dgv_Details_Total1.Columns(dgvCOL_PCSDETAILS.BEAMNO_SETCODE_FORSELECTION).Visible = True
            dgv_Details_Total1.Columns(dgvCOL_PCSDETAILS.EXCESSHORTMETER).Visible = True
            dgv_Details_Total1.Columns(dgvCOL_PCSDETAILS.TYPE5METER).Visible = False
            dgv_Details_Total1.Columns(dgvCOL_PCSDETAILS.TOTALMETERIN100FOLDING).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.TOTALMETERIN100FOLDING).Width
            dgv_Details_Total1.Columns(dgvCOL_PCSDETAILS.BEAMNO_SETCODE_FORSELECTION).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.BEAMNO_SETCODE_FORSELECTION).Width
            dgv_Details_Total1.Columns(dgvCOL_PCSDETAILS.REMARKS).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.REMARKS).Width
            dgv_Details_Total1.Columns(dgvCOL_PCSDETAILS.EXCESSHORTMETER).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.EXCESSHORTMETER).Width

            dgv_Details_Total1.Columns(dgvCOL_PCSDETAILS.RECEIPTMETER).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.RECEIPTMETER).Width
            dgv_Details_Total1.Columns(dgvCOL_PCSDETAILS.TYPE1METER).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.TYPE1METER).Width
            dgv_Details_Total1.Columns(dgvCOL_PCSDETAILS.TYPE2METER).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.TYPE2METER).Width
            dgv_Details_Total1.Columns(dgvCOL_PCSDETAILS.TYPE3METER).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.TYPE3METER).Width
            dgv_Details_Total1.Columns(dgvCOL_PCSDETAILS.TYPE4METER).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.TYPE4METER).Width
            dgv_Details_Total1.Columns(dgvCOL_PCSDETAILS.TOTALMETER).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.TOTALMETER).Width
            dgv_Details_Total1.Columns(dgvCOL_PCSDETAILS.WEIGHT).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.WEIGHT).Width
            dgv_Details_Total1.Columns(dgvCOL_PCSDETAILS.WEIGHTPERMETER).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.WEIGHTPERMETER).Width

            dgv_Details_Total2.Columns(dgvCOL_PCSDETAILS.TOTALMETERIN100FOLDING).Visible = True
            dgv_Details_Total2.Columns(dgvCOL_PCSDETAILS.BEAMNO_SETCODE_FORSELECTION).Visible = True
            dgv_Details_Total2.Columns(dgvCOL_PCSDETAILS.EXCESSHORTMETER).Visible = True
            dgv_Details_Total2.Columns(dgvCOL_PCSDETAILS.TYPE5METER).Visible = False
            dgv_Details_Total2.Columns(dgvCOL_PCSDETAILS.TOTALMETERIN100FOLDING).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.TOTALMETERIN100FOLDING).Width
            dgv_Details_Total2.Columns(dgvCOL_PCSDETAILS.BEAMNO_SETCODE_FORSELECTION).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.BEAMNO_SETCODE_FORSELECTION).Width
            dgv_Details_Total2.Columns(dgvCOL_PCSDETAILS.REMARKS).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.REMARKS).Width
            dgv_Details_Total2.Columns(dgvCOL_PCSDETAILS.EXCESSHORTMETER).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.EXCESSHORTMETER).Width

            dgv_Details_Total2.Columns(dgvCOL_PCSDETAILS.RECEIPTMETER).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.RECEIPTMETER).Width
            dgv_Details_Total2.Columns(dgvCOL_PCSDETAILS.TYPE1METER).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.TYPE1METER).Width
            dgv_Details_Total2.Columns(dgvCOL_PCSDETAILS.TYPE2METER).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.TYPE2METER).Width
            dgv_Details_Total2.Columns(dgvCOL_PCSDETAILS.TYPE3METER).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.TYPE3METER).Width
            dgv_Details_Total2.Columns(dgvCOL_PCSDETAILS.TYPE4METER).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.TYPE4METER).Width
            dgv_Details_Total2.Columns(dgvCOL_PCSDETAILS.TOTALMETER).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.TOTALMETER).Width
            dgv_Details_Total2.Columns(dgvCOL_PCSDETAILS.WEIGHT).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.WEIGHT).Width
            dgv_Details_Total2.Columns(dgvCOL_PCSDETAILS.WEIGHTPERMETER).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.WEIGHTPERMETER).Width

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then  '---- BRT TEXTILES(SOMANUR)

            lbl_Rec_Meter_Caption.Text = "Receipt Meters (In DC)"

            txt_Rec_Meter.Enabled = True
            If Common_Procedures.Office_System_Status = False Then  '---[THANGES]  - REMOVE IT AFTER FINISHING THE Correcting Old Entries
                txt_Rec_Meter.Enabled = False ' True
            End If

            dgv_Details.Columns(dgvCOL_PCSDETAILS.TYPE3METER).ReadOnly = True


        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1395" Then   '---- SANTHA EXPORTS (SOMANUR)

            dgv_Details.Columns(dgvCOL_PCSDETAILS.EXCESSHORTSTATUSYESSORNO).Visible = True
            dgv_Details.Columns(dgvCOL_PCSDETAILS.EXCESSHORTMETER).Visible = True

            dgv_Details.Columns(dgvCOL_PCSDETAILS.TYPE4METER).Visible = False
            dgv_Details.Columns(dgvCOL_PCSDETAILS.TYPE5METER).Visible = False

            dgv_Details.Columns(dgvCOL_PCSDETAILS.EXCESSHORTSTATUSYESSORNO).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.TYPE4METER).Width
            dgv_Details.Columns(dgvCOL_PCSDETAILS.EXCESSHORTMETER).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.TYPE5METER).Width

            dgv_Details_Total1.Columns(dgvCOL_PCSDETAILS.EXCESSHORTSTATUSYESSORNO).Visible = True
            dgv_Details_Total1.Columns(dgvCOL_PCSDETAILS.EXCESSHORTMETER).Visible = True
            dgv_Details_Total1.Columns(dgvCOL_PCSDETAILS.TYPE4METER).Visible = False
            dgv_Details_Total1.Columns(dgvCOL_PCSDETAILS.TYPE5METER).Visible = False

            dgv_Details_Total1.Columns(dgvCOL_PCSDETAILS.EXCESSHORTSTATUSYESSORNO).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.EXCESSHORTSTATUSYESSORNO).Width
            dgv_Details_Total1.Columns(dgvCOL_PCSDETAILS.EXCESSHORTMETER).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.EXCESSHORTMETER).Width

            dgv_Details_Total2.Columns(dgvCOL_PCSDETAILS.EXCESSHORTSTATUSYESSORNO).Visible = True
            dgv_Details_Total2.Columns(dgvCOL_PCSDETAILS.EXCESSHORTMETER).Visible = True
            dgv_Details_Total2.Columns(dgvCOL_PCSDETAILS.TYPE4METER).Visible = False
            dgv_Details_Total2.Columns(dgvCOL_PCSDETAILS.TYPE5METER).Visible = False

            dgv_Details_Total2.Columns(dgvCOL_PCSDETAILS.EXCESSHORTSTATUSYESSORNO).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.EXCESSHORTSTATUSYESSORNO).Width
            dgv_Details_Total2.Columns(dgvCOL_PCSDETAILS.EXCESSHORTMETER).Width = dgv_Details.Columns(dgvCOL_PCSDETAILS.EXCESSHORTMETER).Width


        End If

        Me.Text = ""

        con.Open()

        Update_BeamNo_SetCode_forSelection_Fields()

        lbl_LotNo.Visible = False
        txt_RecNo.Visible = True

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then '---- Kalaimagal Textiles (Palladam)

            txt_RecNo.Width = txt_RecNo.Width \ 2

            lbl_LotNo.Visible = True
            lbl_LotNo.Left = txt_RecNo.Left + txt_RecNo.Width + 2
            lbl_LotNo.Top = txt_RecNo.Top
            lbl_LotNo.Width = txt_RecNo.Width

            lbl_LotNoCaption.Text = "REC.NO / LOTNO"
            lbl_LotNoCaption.Top = txt_RecNo.Top - 1

        End If



        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then
            pnl_CheckingDetails.Visible = True
            pnl_CheckingDetails.Left = (Me.Width - pnl_CheckingDetails.Width) \ 2
            pnl_CheckingDetails.Top = (Me.Height - pnl_CheckingDetails.Height) \ 2
            pnl_CheckingDetails.BringToFront()
            btn_ChkDetails.Visible = True
            cbo_Cloth_TransferTo.Enabled = True
            cbo_Cloth_TransferTo.Width = Lbl_StockOff.Width

            lbl_Cloth_TransferTo_Caption.Visible = True
            cbo_Cloth_TransferTo.Visible = True

            btn_WARP_WEFT_STOCK_UPDATION.Visible = True

        Else

            pnl_CheckingDetails.Visible = False

            btn_ChkDetails.Visible = False


        End If

        btn_BarCodePrint_SinglePieces.Visible = False
        btn_BarCodePrint_AllPieces.Visible = False
        If Val(Common_Procedures.User.IdNo) = 1 Or Trim(Common_Procedures.UR.Weaver_Piece_Checking_Entry_BarCode_Print_Status) <> "" Then
            btn_BarCodePrint_SinglePieces.Visible = True
            btn_BarCodePrint_AllPieces.Visible = True
        End If

        btn_SaveAll.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
            btn_SaveAll.Visible = True
        End If

        lbl_Godown.Visible = False
        lbl_Godown_Caption.Visible = False
        If Common_Procedures.settings.Multi_Godown_Status = 1 Then
            lbl_Godown.Visible = True
            lbl_Godown_Caption.Visible = True
        End If

        Lbl_StockOff.Visible = False
        lbl_StockOff_Caption.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1019" Then
            Lbl_StockOff.Visible = True
            lbl_StockOff_Caption.Visible = True
        End If

        btn_WARP_WEFT_STOCK_UPDATION.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Trim(Common_Procedures.UR.Weaver_Piece_Checking_Entry_Warp_Weft_Stock_Updation) <> "" Then
                btn_WARP_WEFT_STOCK_UPDATION.Visible = True
            End If
        End If



        If Common_Procedures.settings.Multi_Godown_Status <> 1 And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1019" Then
            Dim vTopDiffVal As Single

            vTopDiffVal = dgv_Details.Top - lbl_Godown.Top

            dgv_Details.Top = lbl_Godown.Top
            dgv_Details.Height = dgv_Details.Height + vTopDiffVal

        End If


        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        pnl_Print.Visible = False
        pnl_Print.BringToFront()
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2

        pnl_OpenRecord.Visible = False
        pnl_OpenRecord.Left = (Me.Width - pnl_OpenRecord.Width) \ 2
        pnl_OpenRecord.Top = (Me.Height - pnl_OpenRecord.Height) \ 2
        pnl_OpenRecord.BringToFront()

        cbo_LoomType.Items.Clear()
        cbo_LoomType.Items.Add("")
        cbo_LoomType.Items.Add("POWERLOOM")
        cbo_LoomType.Items.Add("AUTOLOOM")


        cbo_Grid_RateFor.Items.Clear()
        cbo_Grid_RateFor.Items.Add("")
        cbo_Grid_RateFor.Items.Add("YES")
        cbo_Grid_RateFor.Items.Add("NO")

        chk_Verified_Status.Visible = False

        If Common_Procedures.settings.Vefified_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_Verified_Status = 1 Then
                chk_Verified_Status.Visible = True
                lbl_verfied_sts.Visible = True
                cbo_Verified_Sts.Visible = True
            End If

        Else

            chk_Verified_Status.Visible = False
            lbl_verfied_sts.Visible = False
            cbo_Verified_Sts.Visible = False

        End If

        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If


        Chk_Approved_Sts.Visible = False
        btn_SaveApprovedStatus.Visible = False
        set_Approved_Status_Visibility()

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1494" Then
            dgv_Details.Columns(1).HeaderText = "REC.PCS"
        End If

        AddHandler chk_Verified_Status.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_Verified_Status.LostFocus, AddressOf ControlLostFocus

        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Weaver.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Quality.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Filter_LotNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_checker.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Folder.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Cloth_TransferTo.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Folding.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Excess_Short.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PDcNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_RecNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rec_Meter.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_No_Pcs.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_LoomType.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Print_PageNo.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_SaveAll.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_SinglePage.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Cancel.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_RateFor.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler Chk_Approved_Sts.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LotNo_Open.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ChkNoOpen.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_filter_Chkno.GotFocus, AddressOf ControlGotFocus


        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Weaver.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Quality.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Filter_LotNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Folder.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_checker.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Excess_Short.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PDcNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_RecNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rec_Meter.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Folding.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_No_Pcs.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Cloth_TransferTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_LoomType.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Print_PageNo.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_SaveAll.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_SinglePage.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Cancel.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_RateFor.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_CountName.LostFocus, AddressOf ControlLostFocus
        AddHandler Chk_Approved_Sts.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_EMail.Leave, AddressOf ControlLostFocus

        AddHandler txt_LotNo_Open.Leave, AddressOf ControlLostFocus
        AddHandler txt_ChkNoOpen.Leave, AddressOf ControlLostFocus
        AddHandler txt_filter_Chkno.Leave, AddressOf ControlLostFocus

        'AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PDcNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_RecNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Rec_Meter.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_No_Pcs.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Print_PageNo.KeyDown, AddressOf TextBoxControlKeyDown


        'AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PDcNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_RecNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Rec_Meter.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_No_Pcs.KeyPress, AddressOf TextBoxControlKeyPress


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

    Private Sub Weaver_Piece_Checking_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        On Error Resume Next
        Common_Procedures.Last_Closed_FormName = Me.Name
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Weaver_Piece_Checking_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
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
                ElseIf pnl_CheckingDetails.Visible = True Then
                    btn_Close_Chk_Click(sender, e)
                    Exit Sub

                ElseIf pnl_OpenRecord.Visible = True Then
                    btn_CloseOpenRecord_Click(sender, e)
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
        Dim vKEY_PRESS_DATA As System.Windows.Forms.Keys

        If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            On Error Resume Next

            dgv1 = Nothing

            If ActiveControl.Name = dgv_Details.Name Then
                dgv1 = dgv_Details

            ElseIf dgv_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Details

            Else
                dgv1 = dgv_Details

            End If

            If IsNothing(dgv1) = False Then

                If IsNothing(dgv1.CurrentCell) = False Then

                    With dgv1

                        If keyData = Keys.Enter Or keyData = Keys.Down Then

                            If Trim(Common_Procedures.settings.CustomerCode) = "1490" Then
                                If keyData = Keys.Down Then
                                    If .CurrentCell.RowIndex = .RowCount - 1 Then
                                        If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                                            save_record()
                                        Else
                                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(.CurrentCell.ColumnIndex)
                                        End If
                                    Else
                                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(.CurrentCell.ColumnIndex)

                                    End If

                                Else
                                    GoTo loop1
                                End If
                                Return True
                            End If

loop1:
                            If .CurrentCell.ColumnIndex >= dgvCOL_PCSDETAILS.REMARKS Then

                                If .CurrentCell.RowIndex = .RowCount - 1 Then
                                    If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                                        save_record()
                                    Else
                                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(dgvCOL_PCSDETAILS.RECEIPTMETER)
                                    End If

                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(dgvCOL_PCSDETAILS.RECEIPTMETER)

                                End If

                            ElseIf .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE2METER Then

                                If .Columns(dgvCOL_PCSDETAILS.TYPE3METER).Visible = True Then
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.TYPE3METER)
                                ElseIf .Columns(dgvCOL_PCSDETAILS.TYPE4METER).Visible = True Then
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.TYPE4METER)
                                ElseIf .Columns(dgvCOL_PCSDETAILS.TYPE5METER).Visible = True Then
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.TYPE5METER)
                                ElseIf .Columns(dgvCOL_PCSDETAILS.EXCESSHORTSTATUSYESSORNO).Visible = True Then
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.EXCESSHORTSTATUSYESSORNO)
                                ElseIf .Columns(dgvCOL_PCSDETAILS.EXCESSHORTMETER).Visible = True Then
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.EXCESSHORTMETER)
                                Else
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.WEIGHT)
                                End If

                            ElseIf .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE3METER Then

                                If .Columns(dgvCOL_PCSDETAILS.TYPE4METER).Visible = True Then
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.TYPE4METER)
                                ElseIf .Columns(dgvCOL_PCSDETAILS.TYPE5METER).Visible = True Then
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.TYPE5METER)
                                ElseIf .Columns(dgvCOL_PCSDETAILS.EXCESSHORTSTATUSYESSORNO).Visible = True Then
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.EXCESSHORTSTATUSYESSORNO)
                                ElseIf .Columns(dgvCOL_PCSDETAILS.EXCESSHORTMETER).Visible = True Then
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.EXCESSHORTMETER)
                                Else
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.WEIGHT)
                                End If


                            ElseIf .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE4METER Then
                                If .Columns(dgvCOL_PCSDETAILS.TYPE5METER).Visible = True Then
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.TYPE5METER)
                                ElseIf .Columns(dgvCOL_PCSDETAILS.EXCESSHORTSTATUSYESSORNO).Visible = True Then
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.EXCESSHORTSTATUSYESSORNO)
                                Else
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.WEIGHT)
                                End If


                            ElseIf .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE5METER Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TOTALMETER Then
                                If .Columns(dgvCOL_PCSDETAILS.EXCESSHORTSTATUSYESSORNO).Visible = True Then
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.EXCESSHORTSTATUSYESSORNO)
                                Else
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.WEIGHT)
                                End If


                            ElseIf .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.EXCESSHORTSTATUSYESSORNO Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.EXCESSHORTMETER Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.WEIGHT)


                            ElseIf .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.WEIGHT Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.WEIGHTPERMETER Then
                                'If Common_Procedures.settings.CustomerCode = "1267" Then
                                '    pnl_CheckingDetails.Show()
                                '    GetChecker_details()

                                If .Columns(dgvCOL_PCSDETAILS.BEAMNO_SETCODE_FORSELECTION).Visible = True Then
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.BEAMNO_SETCODE_FORSELECTION)

                                Else
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.REMARKS)

                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If


                            Return True



                        ElseIf keyData = Keys.Up Or keyData = Keys.Left Then

                            If Trim(Common_Procedures.settings.CustomerCode) = "1490" Then '-- SRI LAKSHMI SARASWATHI EXPORTS
                                If keyData = Keys.Up Then
                                    If .CurrentCell.RowIndex = 0 Then
                                        txt_Folding.Focus()
                                    Else
                                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.CurrentCell.ColumnIndex)
                                    End If
                                Else
                                        GoTo loop2
                                End If
                                Return True
                            End If
loop2:
                            If .CurrentCell.ColumnIndex <= dgvCOL_PCSDETAILS.RECEIPTMETER Then

                                If .CurrentCell.RowIndex = 0 Then
                                    If cbo_Cloth_TransferTo.Visible = True And cbo_Cloth_TransferTo.Enabled = True Then
                                        cbo_Cloth_TransferTo.Focus()
                                    ElseIf txt_Folding.Visible And txt_Folding.Enabled Then
                                        txt_Folding.Focus()
                                    ElseIf cbo_LoomType.Visible And cbo_LoomType.Enabled Then
                                        cbo_LoomType.Focus()
                                    Else
                                        msk_date.Focus()
                                    End If


                                Else

                                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(dgvCOL_PCSDETAILS.REMARKS)

                                End If

                            ElseIf .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.EXCESSHORTSTATUSYESSORNO Then
                                If .Columns(dgvCOL_PCSDETAILS.TYPE5METER).Visible = True Then
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.TYPE5METER)
                                ElseIf .Columns(dgvCOL_PCSDETAILS.TYPE4METER).Visible = True Then
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.TYPE4METER)
                                ElseIf .Columns(dgvCOL_PCSDETAILS.TYPE3METER).Visible = True Then
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.TYPE3METER)
                                Else
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.TYPE2METER)
                                End If


                            ElseIf .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.WEIGHT Then
                                If .Columns(dgvCOL_PCSDETAILS.EXCESSHORTSTATUSYESSORNO).Visible = True Then
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.EXCESSHORTSTATUSYESSORNO)
                                ElseIf .Columns(dgvCOL_PCSDETAILS.TYPE5METER).Visible = True Then
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.TYPE5METER)
                                ElseIf .Columns(dgvCOL_PCSDETAILS.TYPE4METER).Visible = True Then
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.TYPE4METER)
                                ElseIf .Columns(dgvCOL_PCSDETAILS.TYPE3METER).Visible = True Then
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.TYPE3METER)
                                Else
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.TYPE2METER)
                                End If

                            ElseIf .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE4METER Then
                                If .Columns(dgvCOL_PCSDETAILS.TYPE3METER).Visible = True Then
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.TYPE3METER)
                                Else
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.TYPE2METER)
                                End If

                            ElseIf .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.BEAMNO_SETCODE_FORSELECTION Then
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.WEIGHT)



                            ElseIf .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.REMARKS Then
                                If .Columns(dgvCOL_PCSDETAILS.BEAMNO_SETCODE_FORSELECTION).Visible = True Then
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.BEAMNO_SETCODE_FORSELECTION)
                                Else
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.WEIGHT)
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

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Private Sub move_record(ByVal no As String)
        Dim cmd As New SqlClient.SqlCommand
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer
        Dim SQL1 As String = ""
        Dim LockSTS As Boolean = False


        If Val(no) = 0 Then Exit Sub

        clear()

        NoCalc_Status = True

        NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            con.Open()



            da1 = New SqlClient.SqlDataAdapter("select a.*, a.Loom_Type as Autoloom_Powerloom_Type , e.Ledger_Name as StockOff_Name from Weaver_Piece_Checking_Head a LEFT OUTER JOIN Ledger_Head e ON a.StockOff_IdNo = e.Ledger_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and " & Other_Condition, con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_ChkNo.Text = dt1.Rows(0).Item("Weaver_Piece_Checking_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Weaver_Piece_Checking_Date")
                msk_date.Text = dtp_Date.Text

                cbo_Weaver.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                cbo_Quality.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_IdNo").ToString))
                cbo_Cloth_TransferTo.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_TransferTo_Idno").ToString))
                dtp_Rec_Date.Text = dt1.Rows(0).Item("Piece_Receipt_Date").ToString
                msk_Rec_Date.Text = dtp_Rec_Date.Text
                txt_Excess_Short.Text = Format(Val(dt1.Rows(0).Item("Excess_Short_Meter").ToString), "#######0.00")
                txt_Folding.Text = Format(Val(dt1.Rows(0).Item("Folding").ToString), "#######0.00")
                txt_PDcNo.Text = dt1.Rows(0).Item("Party_DcNo").ToString
                txt_Rec_Meter.Text = Format(Val(dt1.Rows(0).Item("ReceiptMeters_Receipt").ToString), "#######0.00")
                lbl_RecPkCondition.Text = dt1.Rows(0).Item("Receipt_PkCondition").ToString
                lbl_RecCode.Text = dt1.Rows(0).Item("Piece_Receipt_Code").ToString
                txt_RecNo.Text = dt1.Rows(0).Item("Piece_Receipt_No").ToString
                lbl_LotNo.Text = dt1.Rows(0).Item("Lot_No").ToString
                txt_No_Pcs.Text = Format(Val(dt1.Rows(0).Item("noof_pcs").ToString), "#######0")
                Lbl_StockOff.Text = dt1.Rows(0).Item("StockOff_Name").ToString

                If Val(dt1.Rows(0).Item("Verified_Status").ToString) = 1 Then chk_Verified_Status.Checked = True

                If Val(dt1.Rows(0).Item("Approved_Status").ToString) = 1 Then Chk_Approved_Sts.Checked = True
                set_Approved_Status_Visibility()

                cbo_checker.Text = Common_Procedures.Employee_Simple_IdNoToName(con, Val(dt1.Rows(0).Item("Checker_Idno").ToString))
                cbo_Folder.Text = Common_Procedures.Employee_Simple_IdNoToName(con, Val(dt1.Rows(0).Item("Folder_Idno").ToString))

                cbo_LoomType.Text = dt1.Rows(0).Item("Autoloom_Powerloom_Type").ToString


                'lbl_UserName_CreatedBy.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))
                lbl_UserName_CreatedBy.Text = ""
                lbl_UserName_ModifiedBy.Text = ""
                lbl_UserName_ApprovedBy.Text = ""
                If Val(dt1.Rows(0).Item("created_useridno").ToString) <> 0 Then
                    If IsDate(dt1.Rows(0).Item("created_DateTime").ToString) = True And Trim(dt1.Rows(0).Item("created_DateTime_Text").ToString) <> "" Then
                        lbl_UserName_CreatedBy.Text = "Created by " & Trim(UCase(Common_Procedures.User_IdNoToName(con, Val(dt1.Rows(0).Item("created_useridno").ToString)))) & " @ " & Trim(dt1.Rows(0).Item("created_DateTime_Text").ToString)
                    Else
                        lbl_UserName_CreatedBy.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con, Val(dt1.Rows(0).Item("created_useridno").ToString))))
                    End If
                End If
                If Val(dt1.Rows(0).Item("Last_modified_useridno").ToString) <> 0 Then
                    If IsDate(dt1.Rows(0).Item("Last_modified_DateTime").ToString) = True And Trim(dt1.Rows(0).Item("Last_modified_DateTime_Text").ToString) <> "" Then
                        lbl_UserName_ModifiedBy.Text = "Last Modified by " & Trim(UCase(Common_Procedures.User_IdNoToName(con, Val(dt1.Rows(0).Item("Last_modified_useridno").ToString)))) & " @ " & Trim(dt1.Rows(0).Item("Last_modified_DateTime_Text").ToString)
                    End If
                End If
                If Val(dt1.Rows(0).Item("approvedby_useridno").ToString) <> 0 Then
                    If IsDate(dt1.Rows(0).Item("approvedby_DateTime").ToString) = True And Trim(dt1.Rows(0).Item("approvedby_DateTime_Text").ToString) <> "" Then
                        lbl_UserName_ApprovedBy.Text = "Approved by " & Trim(UCase(Common_Procedures.User_IdNoToName(con, Val(dt1.Rows(0).Item("approvedby_useridno").ToString)))) & " @ " & Trim(dt1.Rows(0).Item("approvedby_DateTime_Text").ToString)
                    End If
                End If


                ' ********************* CODE BY GOPI 2024-12-26
                '*********** NEW

                If Trim(UCase(lbl_RecPkCondition.Text)) = "CPREC-" Then


                    da1 = New SqlClient.SqlDataAdapter("Select a.* from Cloth_Purchase_Receipt_Head a Where a.Cloth_Purchase_Receipt_Code = '" & Trim(lbl_RecCode.Text) & "'", con)
                    dt2 = New DataTable
                    da1.Fill(dt2)
                    If dt2.Rows.Count > 0 Then
                        lbl_Godown.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt2.Rows(0).Item("Deliver_At_IdNo").ToString))
                        txt_Rec_Meter.Text = Format(Val(dt2.Rows(0).Item("ReceiptMeters_Receipt").ToString), "#######0.00")

                        If Trim(cbo_LoomType.Text) = "" Then

                            cbo_LoomType.Text = dt2.Rows(0).Item("Loom_Type").ToString

                            cmd.Connection = con
                            cmd.CommandText = "Update Weaver_Piece_Checking_Head set Loom_Type = '" & Trim(cbo_LoomType.Text) & "' Where (Loom_Type is Null or Loom_Type = '') and Piece_Receipt_Code = '" & Trim(lbl_RecCode.Text) & "' and Receipt_PkCondition = 'CPREC-'"
                            cmd.ExecuteNonQuery()

                        End If

                    End If

                    da1.Dispose()
                    dt2.Dispose()
                    dt2.Clear()

                ElseIf Trim(UCase(lbl_RecPkCondition.Text)) = "CLDRT-" Then

                    lbl_Godown.Text = Common_Procedures.Ledger_IdNoToName(con, Common_Procedures.CommonLedger.Godown_Ac)

                ElseIf Trim(UCase(lbl_RecPkCondition.Text)) = "CLSRT-" Or Trim(UCase(lbl_RecPkCondition.Text)) = "GCLSR-" Then

                    lbl_Godown.Text = Common_Procedures.Ledger_IdNoToName(con, Common_Procedures.CommonLedger.Godown_Ac)

                ElseIf Trim(UCase(lbl_RecPkCondition.Text)) = Trim(UCase(PkCondition_PROFABRCPT)) Then

                    lbl_Godown.Text = Common_Procedures.Ledger_IdNoToName(con, Common_Procedures.CommonLedger.Godown_Ac)

                Else
                    da1 = New SqlClient.SqlDataAdapter("Select a.* from Weaver_Cloth_Receipt_Head a Where a.Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "'", con)
                    dt2 = New DataTable
                    da1.Fill(dt2)
                    If dt2.Rows.Count > 0 Then
                        lbl_Godown.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt2.Rows(0).Item("WareHouse_IdNo").ToString))
                        txt_Rec_Meter.Text = Format(Val(dt2.Rows(0).Item("ReceiptMeters_Receipt").ToString), "#######0.00")
                        If Trim(cbo_LoomType.Text) = "" Then

                            cbo_LoomType.Text = dt2.Rows(0).Item("Loom_Type").ToString

                            cmd.Connection = con
                            cmd.CommandText = "Update Weaver_Piece_Checking_Head set Loom_Type = '" & Trim(cbo_LoomType.Text) & "' Where (Loom_Type is Null or Loom_Type = '') and Piece_Receipt_Code = '" & Trim(lbl_RecCode.Text) & "' and Receipt_PkCondition = 'WCLRC-'"
                            cmd.ExecuteNonQuery()

                        End If


                    End If

                    da1.Dispose()
                    dt2.Dispose()
                    dt2.Clear()

                End If


                ' ********************* COMMAND BY GOPI 2024-12-26
                '*********** OLD

                'da1 = New SqlClient.SqlDataAdapter("Select a.* from Weaver_Cloth_Receipt_Head a Where a.Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "'", con)
                'dt2 = New DataTable
                'da1.Fill(dt2)
                'If dt2.Rows.Count > 0 Then
                '    lbl_Godown.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt2.Rows(0).Item("WareHouse_IdNo").ToString))
                '    txt_Rec_Meter.Text = Format(Val(dt2.Rows(0).Item("ReceiptMeters_Receipt").ToString), "#######0.00")
                '    If Trim(cbo_LoomType.Text) = "" Then

                '        cbo_LoomType.Text = dt2.Rows(0).Item("Loom_Type").ToString

                '        cmd.Connection = con
                '        cmd.CommandText = "Update Weaver_Piece_Checking_Head set Loom_Type = '" & Trim(cbo_LoomType.Text) & "' Where (Loom_Type is Null or Loom_Type = '') and Piece_Receipt_Code = '" & Trim(lbl_RecCode.Text) & "' and Receipt_PkCondition = 'WCLRC-'"
                '        cmd.ExecuteNonQuery()

                '    End If


                'Else

                '    da1 = New SqlClient.SqlDataAdapter("Select a.* from Cloth_Purchase_Receipt_Head a Where a.Cloth_Purchase_Receipt_Code = '" & Trim(lbl_RecCode.Text) & "'", con)
                '    dt2 = New DataTable
                '    da1.Fill(dt2)
                '    If dt2.Rows.Count > 0 Then
                '        lbl_Godown.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt2.Rows(0).Item("Deliver_At_IdNo").ToString))
                '        txt_Rec_Meter.Text = Format(Val(dt2.Rows(0).Item("ReceiptMeters_Receipt").ToString), "#######0.00")

                '        If Trim(cbo_LoomType.Text) = "" Then

                '            cbo_LoomType.Text = dt2.Rows(0).Item("Loom_Type").ToString

                '            cmd.Connection = con
                '            cmd.CommandText = "Update Weaver_Piece_Checking_Head set Loom_Type = '" & Trim(cbo_LoomType.Text) & "' Where (Loom_Type is Null or Loom_Type = '') and Piece_Receipt_Code = '" & Trim(lbl_RecCode.Text) & "' and Receipt_PkCondition = 'CPREC-'"
                '            cmd.ExecuteNonQuery()

                '        End If

                '    End If

                'End If
                'dt2.Clear()

                cmd.Connection = con


                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "sp_get_weaverclothreceiptpiecedetails_for_moving1"
                cmd.Parameters.Clear()
                cmd.Parameters.Add("@weaver_piece_checking_code", SqlDbType.VarChar)
                cmd.Parameters("@weaver_piece_checking_code").Value = Trim(NewCode)
                da2 = New SqlClient.SqlDataAdapter(cmd)


                'SQL1 = "Select a.*, b.employee_name as checkername, c.employee_name as foldername from Weaver_ClothReceipt_Piece_Details a  LEFT OUTER JOIN Employee_Head b ON a.Checker_IdNo <> 0 and a.Checker_IdNo = b.Employee_IdNo LEFT OUTER JOIN Employee_Head c ON a.folder_idno <> 0 and a.folder_idno = c.Employee_IdNo Where a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' Order by a.PieceNo_OrderBy, a.Piece_No , a.Sl_No"
                'cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
                'da2 = New SqlClient.SqlDataAdapter(cmd)
                ''da2 = New SqlClient.SqlDataAdapter("Select a.*, b.employee_name as checkername, c.employee_name as foldername from Weaver_ClothReceipt_Piece_Details a  LEFT OUTER JOIN Employee_Head b ON a.Checker_IdNo <> 0 and a.Checker_IdNo = b.Employee_IdNo LEFT OUTER JOIN Employee_Head c ON a.folder_idno <> 0 and a.folder_idno = c.Employee_IdNo Where a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' Order by a.PieceNo_OrderBy, a.Piece_No , a.Sl_No", con)
                ''da2 = New SqlClient.SqlDataAdapter("Select a.* ,b.employee_name as checkername from Weaver_ClothReceipt_Piece_Details a  LEFT OUTER JOIN Employee_Head b ON a.Checker_IdNo = b.Employee_IdNo Where a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' Order by PieceNo_OrderBy, Sl_No, Piece_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_Details

                    .Rows.Clear()
                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()

                            .Rows(n).Cells(dgvCOL_PCSDETAILS.PCSNO).Value = dt2.Rows(i).Item("Piece_No").ToString
                            If Val(dt2.Rows(i).Item("ReceiptMeters_Checking").ToString) <> 0 Then
                                .Rows(n).Cells(dgvCOL_PCSDETAILS.RECEIPTMETER).Value = Format(Val(dt2.Rows(i).Item("ReceiptMeters_Checking").ToString), "########0.00")
                            End If
                            .Rows(n).Cells(dgvCOL_PCSDETAILS.LOOMNO).Value = dt2.Rows(i).Item("Loom_No").ToString
                            If Val(dt2.Rows(i).Item("Pick").ToString) <> 0 Then
                                .Rows(n).Cells(dgvCOL_PCSDETAILS.PICK).Value = Val(dt2.Rows(i).Item("Pick").ToString)
                            End If
                            If Val(dt2.Rows(i).Item("Width").ToString) <> 0 Then
                                .Rows(n).Cells(dgvCOL_PCSDETAILS.WIDTH).Value = Val(dt2.Rows(i).Item("Width").ToString)
                            End If
                            If Val(dt2.Rows(i).Item("Type1_Meters").ToString) <> 0 Then
                                .Rows(n).Cells(dgvCOL_PCSDETAILS.TYPE1METER).Value = Format(Val(dt2.Rows(i).Item("Type1_Meters").ToString), "########0.00")
                            End If
                            If Val(dt2.Rows(i).Item("Type2_Meters").ToString) <> 0 Then
                                .Rows(n).Cells(dgvCOL_PCSDETAILS.TYPE2METER).Value = Format(Val(dt2.Rows(i).Item("Type2_Meters").ToString), "########0.00")
                            End If
                            If Val(dt2.Rows(i).Item("Type3_Meters").ToString) <> 0 Then
                                .Rows(n).Cells(dgvCOL_PCSDETAILS.TYPE3METER).Value = Format(Val(dt2.Rows(i).Item("Type3_Meters").ToString), "########0.00")
                            End If
                            If Val(dt2.Rows(i).Item("Type4_Meters").ToString) <> 0 Then
                                .Rows(n).Cells(dgvCOL_PCSDETAILS.TYPE4METER).Value = Format(Val(dt2.Rows(i).Item("Type4_Meters").ToString), "########0.00")
                            End If
                            If Val(dt2.Rows(i).Item("Type5_Meters").ToString) <> 0 Then
                                .Rows(n).Cells(dgvCOL_PCSDETAILS.TYPE5METER).Value = Format(Val(dt2.Rows(i).Item("Type5_Meters").ToString), "########0.00")
                            End If

                            .Rows(n).Cells(dgvCOL_PCSDETAILS.TOTALMETER).Value = Format(Val(dt2.Rows(i).Item("Total_Checking_Meters").ToString), "########0.00")
                            If Val(dt2.Rows(i).Item("Weight").ToString) <> 0 Then
                                .Rows(n).Cells(dgvCOL_PCSDETAILS.WEIGHT).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")
                            End If
                            If Val(dt2.Rows(i).Item("Weight_Meter").ToString) <> 0 Then
                                .Rows(n).Cells(dgvCOL_PCSDETAILS.WEIGHTPERMETER).Value = Format(Val(dt2.Rows(i).Item("Weight_Meter").ToString), "########0.000")
                            End If

                            .Rows(n).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE1).Value = dt2.Rows(i).Item("PackingSlip_Code_Type1").ToString
                            If Trim(.Rows(n).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE1).Value) = "" Then .Rows(n).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE1).Value = dt2.Rows(i).Item("Bale_UnPacking_Code_Type1").ToString
                            If Trim(.Rows(n).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE1).Value) = "" Then .Rows(n).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE1).Value = dt2.Rows(i).Item("BuyerOffer_Code_Type1").ToString

                            .Rows(n).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE2).Value = dt2.Rows(i).Item("PackingSlip_Code_Type2").ToString
                            If Trim(.Rows(n).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE2).Value) = "" Then .Rows(n).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE2).Value = dt2.Rows(i).Item("Bale_UnPacking_Code_Type2").ToString
                            If Trim(.Rows(n).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE2).Value) = "" Then .Rows(n).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE2).Value = dt2.Rows(i).Item("BuyerOffer_Code_Type2").ToString

                            .Rows(n).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE3).Value = dt2.Rows(i).Item("PackingSlip_Code_Type3").ToString
                            If Trim(.Rows(n).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE3).Value) = "" Then .Rows(n).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE3).Value = dt2.Rows(i).Item("Bale_UnPacking_Code_Type3").ToString
                            If Trim(.Rows(n).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE3).Value) = "" Then .Rows(n).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE3).Value = dt2.Rows(i).Item("BuyerOffer_Code_Type3").ToString

                            .Rows(n).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE4).Value = dt2.Rows(i).Item("PackingSlip_Code_Type4").ToString
                            If Trim(.Rows(n).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE4).Value) = "" Then .Rows(n).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE4).Value = dt2.Rows(i).Item("Bale_UnPacking_Code_Type4").ToString
                            If Trim(.Rows(n).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE4).Value) = "" Then .Rows(n).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE4).Value = dt2.Rows(i).Item("BuyerOffer_Code_Type4").ToString

                            .Rows(n).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE5).Value = dt2.Rows(i).Item("PackingSlip_Code_Type5").ToString
                            If Trim(.Rows(n).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE5).Value) = "" Then .Rows(n).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE5).Value = dt2.Rows(i).Item("Bale_UnPacking_Code_Type5").ToString
                            If Trim(.Rows(n).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE5).Value) = "" Then .Rows(n).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE5).Value = dt2.Rows(i).Item("BuyerOffer_Code_Type5").ToString

                            .Rows(n).Cells(dgvCOL_PCSDETAILS.REMARKS).Value = dt2.Rows(i).Item("Remarks").ToString

                            If IsDBNull(dt2.Rows(i).Item("checkername").ToString) = False Then
                                .Rows(n).Cells(dgvCOL_PCSDETAILS.CHECKERNAME).Value = dt2.Rows(i).Item("checkername").ToString
                            End If
                            If IsDBNull(dt2.Rows(i).Item("foldername").ToString) = False Then
                                .Rows(n).Cells(dgvCOL_PCSDETAILS.FOLDERNAME).Value = dt2.Rows(i).Item("foldername").ToString
                            End If
                            '.Rows(n).Cells(dgvCOL_PCSDETAILS.CHECKERNAME).Value = Common_Procedures.Employee_Simple_IdNoToName(con, Val(dt2.Rows(i).Item("Checker_idno").ToString))
                            '.Rows(n).Cells(dgvCOL_PCSDETAILS.FOLDERNAME).Value = Common_Procedures.Employee_Simple_IdNoToName(con, Val(dt2.Rows(i).Item("folder_idno").ToString))

                            'Total_CheckingMeters_100Folding    , ExcessShort_Status_YesNo  Excess_Short_Meter  BeamNo_SetCode                                                   

                            .Rows(n).Cells(dgvCOL_PCSDETAILS.TOTALMETERIN100FOLDING).Value = dt2.Rows(i).Item("Total_CheckingMeters_100Folding").ToString
                            .Rows(n).Cells(dgvCOL_PCSDETAILS.EXCESSHORTSTATUSYESSORNO).Value = dt2.Rows(i).Item("ExcessShort_Status_YesNo").ToString
                            If Val(dt2.Rows(i).Item("Excess_Short_Meter").ToString) <> 0 Then
                                .Rows(n).Cells(dgvCOL_PCSDETAILS.EXCESSHORTMETER).Value = dt2.Rows(i).Item("Excess_Short_Meter").ToString
                            End If
                            .Rows(n).Cells(dgvCOL_PCSDETAILS.BEAMNO_SETCODE_FORSELECTION).Value = dt2.Rows(i).Item("BeamNo_SetCode").ToString

                            If Trim(.Rows(n).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE1).Value) <> "" Then
                                .Rows(n).Cells(dgvCOL_PCSDETAILS.TYPE1METER).ReadOnly = True
                                .Rows(n).Cells(dgvCOL_PCSDETAILS.TYPE1METER).Style.ForeColor = Color.Red
                                LockSTS = True
                            End If

                            If Trim(.Rows(n).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE2).Value) <> "" Then
                                .Rows(n).Cells(dgvCOL_PCSDETAILS.TYPE2METER).ReadOnly = True
                                .Rows(n).Cells(dgvCOL_PCSDETAILS.TYPE2METER).Style.ForeColor = Color.Red
                                LockSTS = True
                            End If

                            If Trim(.Rows(n).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE3).Value) <> "" Then
                                .Rows(n).Cells(dgvCOL_PCSDETAILS.TYPE3METER).ReadOnly = True
                                .Rows(n).Cells(dgvCOL_PCSDETAILS.TYPE3METER).Style.ForeColor = Color.Red
                                LockSTS = True
                            End If

                            If Trim(.Rows(n).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE4).Value) <> "" Then
                                .Rows(n).Cells(dgvCOL_PCSDETAILS.TYPE4METER).ReadOnly = True
                                .Rows(n).Cells(dgvCOL_PCSDETAILS.TYPE4METER).Style.ForeColor = Color.Red
                                LockSTS = True
                            End If

                            If Trim(.Rows(n).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE5).Value) <> "" Then
                                .Rows(n).Cells(dgvCOL_PCSDETAILS.TYPE5METER).ReadOnly = True
                                .Rows(n).Cells(dgvCOL_PCSDETAILS.TYPE5METER).Style.ForeColor = Color.Red
                                LockSTS = True
                            End If

                        Next i

                    End If

                    If .RowCount = 0 Then .Rows.Add()

                End With

                With dgv_Details_Total1
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(dgvCOL_PCSDETAILS.RECEIPTMETER).Value = Format(Val(dt1.Rows(0).Item("Total_Checking_Receipt_Meters").ToString), "########0.00")
                    .Rows(0).Cells(dgvCOL_PCSDETAILS.TYPE1METER).Value = Format(Val(dt1.Rows(0).Item("Total_Type1_Meters").ToString), "########0.00")
                    .Rows(0).Cells(dgvCOL_PCSDETAILS.TYPE2METER).Value = Format(Val(dt1.Rows(0).Item("Total_Type2_Meters").ToString), "########0.00")
                    .Rows(0).Cells(dgvCOL_PCSDETAILS.TYPE3METER).Value = Format(Val(dt1.Rows(0).Item("Total_Type3_Meters").ToString), "########0.00")
                    .Rows(0).Cells(dgvCOL_PCSDETAILS.TYPE4METER).Value = Format(Val(dt1.Rows(0).Item("Total_Type4_Meters").ToString), "########0.00")
                    .Rows(0).Cells(dgvCOL_PCSDETAILS.TYPE5METER).Value = Format(Val(dt1.Rows(0).Item("Total_Type5_Meters").ToString), "########0.00")
                    .Rows(0).Cells(dgvCOL_PCSDETAILS.TOTALMETER).Value = Format(Val(dt1.Rows(0).Item("Total_Checking_Meters").ToString), "########0.00")
                    .Rows(0).Cells(dgvCOL_PCSDETAILS.WEIGHT).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")
                    .Rows(0).Cells(dgvCOL_PCSDETAILS.EXCESSHORTMETER).Value = Format(Val(dt1.Rows(0).Item("Total_ExcessShort_Details_Meters").ToString), "########0.00")
                End With

                With dgv_Details_Total2
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(dgvCOL_PCSDETAILS.PCSNO).Value = "100%"
                    .Rows(0).Cells(dgvCOL_PCSDETAILS.RECEIPTMETER).Value = "FOLDING"
                    .Rows(0).Cells(dgvCOL_PCSDETAILS.TYPE1METER).Value = Format(Val(dt1.Rows(0).Item("Total_Type1Meters_100Folding").ToString), "########0.00")
                    .Rows(0).Cells(dgvCOL_PCSDETAILS.TYPE2METER).Value = Format(Val(dt1.Rows(0).Item("Total_Type2Meters_100Folding").ToString), "########0.00")
                    .Rows(0).Cells(dgvCOL_PCSDETAILS.TYPE3METER).Value = Format(Val(dt1.Rows(0).Item("Total_Type3Meters_100Folding").ToString), "########0.00")
                    .Rows(0).Cells(dgvCOL_PCSDETAILS.TYPE4METER).Value = Format(Val(dt1.Rows(0).Item("Total_Type4Meters_100Folding").ToString), "########0.00")
                    .Rows(0).Cells(dgvCOL_PCSDETAILS.TYPE5METER).Value = Format(Val(dt1.Rows(0).Item("Total_Type5Meters_100Folding").ToString), "########0.00")
                    .Rows(0).Cells(dgvCOL_PCSDETAILS.TOTALMETER).Value = Format(Val(dt1.Rows(0).Item("Total_Meters_100Folding").ToString), "########0.00")

                    .Rows(0).Cells(dgvCOL_PCSDETAILS.TOTALMETERIN100FOLDING).Value = Format(Val(dt1.Rows(0).Item("Total_Checking_Details_Meters_100Folding").ToString), "########0.00")


                End With


                da2 = New SqlClient.SqlDataAdapter("select Weaver_Wages_Code, Weaver_IR_Wages_Code from Weaver_Cloth_Receipt_Head Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                If dt2.Rows.Count > 0 Then
                    If IsDBNull(dt2.Rows(0).Item("Weaver_Wages_Code").ToString) = False Then
                        If Trim(dt2.Rows(0).Item("Weaver_Wages_Code").ToString) <> "" Then
                            LockSTS = True
                        End If
                    End If
                    If IsDBNull(dt2.Rows(0).Item("Weaver_IR_Wages_Code").ToString) = False Then
                        If Trim(dt2.Rows(0).Item("Weaver_IR_Wages_Code").ToString) <> "" Then
                            LockSTS = True
                        End If
                    End If
                End If
                dt1.Clear()


                If LockSTS = True Then

                    cbo_Weaver.Enabled = False
                    cbo_Weaver.BackColor = Color.LightGray
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT TEXTILES(SOMANUR)
                        If Common_Procedures.User.IdNo = 1 Or Trim(Common_Procedures.UR.Weaver_ClothRceipt_Entry_Edit_FABRICNAME_AFTERLOCK) <> "" Then
                            cbo_Weaver.Enabled = True
                        End If
                    End If


                    txt_Folding.Enabled = False
                    txt_Folding.BackColor = Color.LightGray

                    btn_Selection.Enabled = False

                    cbo_Cloth_TransferTo.Enabled = False
                    cbo_Cloth_TransferTo.BackColor = Color.LightGray


                End If

            Else
                new_record()

            End If

            Grid_Cell_DeSelect()

            Me.Refresh()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()

            dt2.Dispose()
            da2.Dispose()


            If msk_date.Visible And msk_date.Enabled Then
                msk_date.Focus()
                msk_date.SelectionStart = 0

            ElseIf cbo_Weaver.Enabled And cbo_Weaver.Visible Then
                cbo_Weaver.Focus()

            ElseIf cbo_LoomType.Visible And cbo_LoomType.Enabled Then
                cbo_LoomType.Focus()

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
        Dim vOrdByNo As String = ""
        Dim SQL1 As String = ""
        Dim vPCSCHK_APPSTS As String = 0
        Dim Nr As Long = 0

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_ChkNo.Text)

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Weaver_Piece_Checking_Entry, New_Entry, Me, con, "Weaver_Piece_Checking_Head", "Weaver_Piece_Checking_Code", NewCode, "Weaver_Piece_Checking_Date", "(Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

        If Common_Procedures.settings.Vefified_Status = 1 Then
            NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            If Val(Common_Procedures.get_FieldValue(con, "Weaver_Piece_Checking_Head", "Verified_Status", "(Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "')")) = 1 Then
                MessageBox.Show("Entry Already Verified", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If

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

        NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        cmd.Connection = con

        SQL1 = "select COUNT(*) from Weaver_ClothReceipt_Piece_Details Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and (PackingSlip_Code_Type1 <> '' or PackingSlip_Code_Type2 <> '' or PackingSlip_Code_Type3 <> '' or PackingSlip_Code_Type4 <> '' or PackingSlip_Code_Type5 <> '' or BuyerOffer_Code_Type1 <> '' or BuyerOffer_Code_Type2 <> '' or BuyerOffer_Code_Type3 <> '' or BuyerOffer_Code_Type4 <> '' or BuyerOffer_Code_Type5 <> '' or Bale_UnPacking_Code_Type1 <> '' or Bale_UnPacking_Code_Type2 <> '' or Bale_UnPacking_Code_Type3 <> '' or Bale_UnPacking_Code_Type4 <> '' or Bale_UnPacking_Code_Type5 <> '')"
        cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
        Da = New SqlClient.SqlDataAdapter(cmd)
        'Da = New SqlClient.SqlDataAdapter("select COUNT(*) from Weaver_ClothReceipt_Piece_Details Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and (PackingSlip_Code_Type1 <> '' or PackingSlip_Code_Type2 <> '' or PackingSlip_Code_Type3 <> '' or PackingSlip_Code_Type4 <> '' or PackingSlip_Code_Type5 <> '' or BuyerOffer_Code_Type1 <> '' or BuyerOffer_Code_Type2 <> '' or BuyerOffer_Code_Type3 <> '' or BuyerOffer_Code_Type4 <> '' or BuyerOffer_Code_Type5 <> '' or Bale_UnPacking_Code_Type1 <> '' or Bale_UnPacking_Code_Type2 <> '' or Bale_UnPacking_Code_Type3 <> '' or Bale_UnPacking_Code_Type4 <> '' or Bale_UnPacking_Code_Type5 <> '')", con)
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

        Da = New SqlClient.SqlDataAdapter("select Weaver_Wages_Code, Weaver_IR_Wages_Code from Weaver_Cloth_Receipt_Head Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("Weaver_Wages_Code").ToString) = False Then
                If Trim(Dt1.Rows(0).Item("Weaver_Wages_Code").ToString) <> "" Then
                    MessageBox.Show("Weaver Wages prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub

                End If
            End If
            If IsDBNull(Dt1.Rows(0).Item("Weaver_IR_Wages_Code").ToString) = False Then
                If Trim(Dt1.Rows(0).Item("Weaver_IR_Wages_Code").ToString) <> "" Then
                    MessageBox.Show("Weaver Wages prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub

                End If
            End If
        End If
        Dt1.Clear()


        NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("Select Approved_Status from Weaver_Piece_Checking_Head a where a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' ", con)
        Dt1.Clear()
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            vPCSCHK_APPSTS = Dt1.Rows(0).Item("Approved_Status").ToString
        End If
        Dt1.Clear()
        If Val(vPCSCHK_APPSTS) = 1 Then
            MessageBox.Show("Invalid Deleting : Already Approved", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Weaver.Enabled Then
                cbo_Weaver.Focus()

            Else
                If dgv_Details.Enabled And dgv_Details.Visible Then
                    If dgv_Details.Rows.Count > 0 Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCOL_PCSDETAILS.PCSNO)
                    End If

                End If

            End If

            Exit Sub
        End If

        cmd.Connection = con
        cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
        cmd.ExecuteNonQuery()

        trans = con.BeginTransaction

        Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", " Weaver_Piece_Checking_Head", " Weaver_Piece_Checking_Code", Val(lbl_Company.Tag), NewCode, lbl_ChkNo.Text, Val(vOrdByNo), PkCondition_Entry, "", "", New_Entry, True, "", "", " Weaver_Piece_Checking_Code, Company_IdNo, for_OrderBy", trans)

        Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", " Weaver_ClothReceipt_Piece_Details", " Weaver_Piece_Checking_Code", Val(lbl_Company.Tag), NewCode, lbl_ChkNo.Text, Val(vOrdByNo), PkCondition_Entry, "", "", New_Entry, True, " Piece_No,ReceiptMeters_Checking,Receipt_Meters,Loom_No,Pick,Width,Type1_Meters,Type2_Meters,Type3_Meters,Type4_Meters,Type5_Meters,Total_Checking_Meters,Weight,Weight_Meter,Remarks,WareHouse_IdNo,Checked_Pcs_Barcode_Type1 ,Checked_Pcs_Barcode_Type2 ,   Checked_Pcs_Barcode_Type3 ,   Checked_Pcs_Barcode_Type4 ,   Checked_Pcs_Barcode_Type5  ,CHecker_idno,Folder_idno ,Checker_Wgs_per_Mtr,Folder_Wgs_per_Mtr", "Sl_No", " Weaver_Piece_Checking_Code, For_OrderBy, Company_IdNo,  Weaver_Piece_Checking_No,  Weaver_Piece_Checking_Date, Ledger_Idno", trans)


        Try

            cmd.Connection = con
            cmd.Transaction = trans

            If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Cloth_Stock) = 1 Then

                '----WEAAVER CLOTH RECEIPT
                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type,   Reference_Code,   Reference_Date,   Company_Idno,   Ledger_Idno    ,   StockOff_IdNo,   Cloth_IdNo, ClothType_IdNo    , Folding ) " &
                                      " Select                               'CLOTH'   , a.Reference_Code, a.Reference_Date, a.Company_IdNo, a.DeliveryTo_Idno, a.StockOff_IdNo, a.Cloth_IdNo, 1 as ClothtypeIDNO, a.Folding from Stock_Cloth_Processing_Details a, Weaver_Cloth_Receipt_Head b Where a.Meters_Type1 <> 0 and b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(PkCondition_Weaver) & "' + b.Weaver_ClothReceipt_Code"
                Nr = cmd.ExecuteNonQuery()

                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type,   Reference_Code,   Reference_Date,   Company_Idno,   Ledger_Idno    ,   StockOff_IdNo,   Cloth_IdNo, ClothType_IdNo    , Folding ) " &
                      " Select                               'CLOTH'   , a.Reference_Code, a.Reference_Date, a.Company_IdNo, a.DeliveryTo_Idno, a.StockOff_IdNo, a.Cloth_IdNo, 2 as ClothtypeIDNO, a.Folding from Stock_Cloth_Processing_Details a, Weaver_Cloth_Receipt_Head b Where a.Meters_Type2 <> 0 and b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(PkCondition_Weaver) & "' + b.Weaver_ClothReceipt_Code"
                Nr = cmd.ExecuteNonQuery()

                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type,   Reference_Code,   Reference_Date,   Company_Idno,   Ledger_Idno    ,   StockOff_IdNo,   Cloth_IdNo, ClothType_IdNo    , Folding ) " &
                      " Select                               'CLOTH'   , a.Reference_Code, a.Reference_Date, a.Company_IdNo, a.DeliveryTo_Idno, a.StockOff_IdNo, a.Cloth_IdNo, 3 as ClothtypeIDNO, a.Folding from Stock_Cloth_Processing_Details a, Weaver_Cloth_Receipt_Head b Where a.Meters_Type3 <> 0 and b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(PkCondition_Weaver) & "' + b.Weaver_ClothReceipt_Code"
                Nr = cmd.ExecuteNonQuery()

                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type,   Reference_Code,   Reference_Date,   Company_Idno,   Ledger_Idno    ,   StockOff_IdNo,   Cloth_IdNo, ClothType_IdNo    , Folding ) " &
                      " Select                               'CLOTH'   , a.Reference_Code, a.Reference_Date, a.Company_IdNo, a.DeliveryTo_Idno, a.StockOff_IdNo, a.Cloth_IdNo, 4 as ClothtypeIDNO, a.Folding from Stock_Cloth_Processing_Details a, Weaver_Cloth_Receipt_Head b Where a.Meters_Type4 <> 0 and b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(PkCondition_Weaver) & "' + b.Weaver_ClothReceipt_Code"
                Nr = cmd.ExecuteNonQuery()

                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type,   Reference_Code,   Reference_Date,   Company_Idno,   Ledger_Idno    ,   StockOff_IdNo,   Cloth_IdNo, ClothType_IdNo    , Folding ) " &
                      " Select                               'CLOTH'   , a.Reference_Code, a.Reference_Date, a.Company_IdNo, a.DeliveryTo_Idno, a.StockOff_IdNo, a.Cloth_IdNo, 5 as ClothtypeIDNO, a.Folding from Stock_Cloth_Processing_Details a, Weaver_Cloth_Receipt_Head b Where a.Meters_Type5 <> 0 and b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(PkCondition_Weaver) & "' + b.Weaver_ClothReceipt_Code"
                Nr = cmd.ExecuteNonQuery()


                '----CLOTH PURCHASE RECEIPT
                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type,   Reference_Code,   Reference_Date,   Company_Idno,   Ledger_Idno    ,   StockOff_IdNo,   Cloth_IdNo, ClothType_IdNo    , Folding ) " &
                      " Select                               'CLOTH'   , a.Reference_Code, a.Reference_Date, a.Company_IdNo, a.DeliveryTo_Idno, a.StockOff_IdNo, a.Cloth_IdNo, 1 as ClothtypeIDNO, a.Folding from Stock_Cloth_Processing_Details a, Cloth_Purchase_Receipt_Head b Where a.Meters_Type1 <> 0 and b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(PkCondition_Purchase) & "' + b.Cloth_Purchase_Receipt_Code"
                Nr = cmd.ExecuteNonQuery()
                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type,   Reference_Code,   Reference_Date,   Company_Idno,   Ledger_Idno    ,   StockOff_IdNo,   Cloth_IdNo, ClothType_IdNo    , Folding ) " &
                      " Select                               'CLOTH'   , a.Reference_Code, a.Reference_Date, a.Company_IdNo, a.DeliveryTo_Idno, a.StockOff_IdNo, a.Cloth_IdNo, 2 as ClothtypeIDNO, a.Folding from Stock_Cloth_Processing_Details a, Cloth_Purchase_Receipt_Head b Where a.Meters_Type2 <> 0 and b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(PkCondition_Purchase) & "' + b.Cloth_Purchase_Receipt_Code"
                Nr = cmd.ExecuteNonQuery()
                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type,   Reference_Code,   Reference_Date,   Company_Idno,   Ledger_Idno    ,   StockOff_IdNo,   Cloth_IdNo, ClothType_IdNo    , Folding ) " &
                      " Select                               'CLOTH'   , a.Reference_Code, a.Reference_Date, a.Company_IdNo, a.DeliveryTo_Idno, a.StockOff_IdNo, a.Cloth_IdNo, 3 as ClothtypeIDNO, a.Folding from Stock_Cloth_Processing_Details a, Cloth_Purchase_Receipt_Head b Where a.Meters_Type3 <> 0 and b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(PkCondition_Purchase) & "' + b.Cloth_Purchase_Receipt_Code"
                Nr = cmd.ExecuteNonQuery()
                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type,   Reference_Code,   Reference_Date,   Company_Idno,   Ledger_Idno    ,   StockOff_IdNo,   Cloth_IdNo, ClothType_IdNo    , Folding ) " &
                      " Select                               'CLOTH'   , a.Reference_Code, a.Reference_Date, a.Company_IdNo, a.DeliveryTo_Idno, a.StockOff_IdNo, a.Cloth_IdNo, 4 as ClothtypeIDNO, a.Folding from Stock_Cloth_Processing_Details a, Cloth_Purchase_Receipt_Head b Where a.Meters_Type4 <> 0 and b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(PkCondition_Purchase) & "' + b.Cloth_Purchase_Receipt_Code"
                Nr = cmd.ExecuteNonQuery()
                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type,   Reference_Code,   Reference_Date,   Company_Idno,   Ledger_Idno    ,   StockOff_IdNo,   Cloth_IdNo, ClothType_IdNo    , Folding ) " &
                      " Select                               'CLOTH'   , a.Reference_Code, a.Reference_Date, a.Company_IdNo, a.DeliveryTo_Idno, a.StockOff_IdNo, a.Cloth_IdNo, 5 as ClothtypeIDNO, a.Folding from Stock_Cloth_Processing_Details a, Cloth_Purchase_Receipt_Head b Where a.Meters_Type5 <> 0 and b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(PkCondition_Purchase) & "' + b.Cloth_Purchase_Receipt_Code"
                Nr = cmd.ExecuteNonQuery()


                '----PROCESSING RECEIPT
                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type,   Reference_Code,   Reference_Date,   Company_Idno,   Ledger_Idno    ,   StockOff_IdNo,   Cloth_IdNo, ClothType_IdNo    , Folding ) " &
                      " Select                               'CLOTH'   , a.Reference_Code, a.Reference_Date, a.Company_IdNo, a.DeliveryTo_Idno, a.StockOff_IdNo, a.Cloth_IdNo, 1 as ClothtypeIDNO, a.Folding from Stock_Cloth_Processing_Details a, Textile_Processing_Receipt_Head b Where a.Meters_Type1 <> 0 and b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(PkCondition_PROFABRCPT) & "' + b.ClothProcess_Receipt_Code"
                Nr = cmd.ExecuteNonQuery()
                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type,   Reference_Code,   Reference_Date,   Company_Idno,   Ledger_Idno    ,   StockOff_IdNo,   Cloth_IdNo, ClothType_IdNo    , Folding ) " &
                      " Select                               'CLOTH'   , a.Reference_Code, a.Reference_Date, a.Company_IdNo, a.DeliveryTo_Idno, a.StockOff_IdNo, a.Cloth_IdNo, 2 as ClothtypeIDNO, a.Folding from Stock_Cloth_Processing_Details a, Textile_Processing_Receipt_Head b Where a.Meters_Type2 <> 0 and b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(PkCondition_PROFABRCPT) & "' + b.ClothProcess_Receipt_Code"
                Nr = cmd.ExecuteNonQuery()
                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type,   Reference_Code,   Reference_Date,   Company_Idno,   Ledger_Idno    ,   StockOff_IdNo,   Cloth_IdNo, ClothType_IdNo    , Folding ) " &
                      " Select                               'CLOTH'   , a.Reference_Code, a.Reference_Date, a.Company_IdNo, a.DeliveryTo_Idno, a.StockOff_IdNo, a.Cloth_IdNo, 3 as ClothtypeIDNO, a.Folding from Stock_Cloth_Processing_Details a, Textile_Processing_Receipt_Head b Where a.Meters_Type3 <> 0 and b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(PkCondition_PROFABRCPT) & "' + b.ClothProcess_Receipt_Code"
                Nr = cmd.ExecuteNonQuery()
                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type,   Reference_Code,   Reference_Date,   Company_Idno,   Ledger_Idno    ,   StockOff_IdNo,   Cloth_IdNo, ClothType_IdNo    , Folding ) " &
                      " Select                               'CLOTH'   , a.Reference_Code, a.Reference_Date, a.Company_IdNo, a.DeliveryTo_Idno, a.StockOff_IdNo, a.Cloth_IdNo, 4 as ClothtypeIDNO, a.Folding from Stock_Cloth_Processing_Details a, Textile_Processing_Receipt_Head b Where a.Meters_Type4 <> 0 and b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(PkCondition_PROFABRCPT) & "' + b.ClothProcess_Receipt_Code"
                Nr = cmd.ExecuteNonQuery()
                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type,   Reference_Code,   Reference_Date,   Company_Idno,   Ledger_Idno    ,   StockOff_IdNo,   Cloth_IdNo, ClothType_IdNo    , Folding ) " &
                      " Select                               'CLOTH'   , a.Reference_Code, a.Reference_Date, a.Company_IdNo, a.DeliveryTo_Idno, a.StockOff_IdNo, a.Cloth_IdNo, 5 as ClothtypeIDNO, a.Folding from Stock_Cloth_Processing_Details a, Textile_Processing_Receipt_Head b Where a.Meters_Type5 <> 0 and b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(PkCondition_PROFABRCPT) & "' + b.ClothProcess_Receipt_Code"
                Nr = cmd.ExecuteNonQuery()



                'cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type,   Reference_Code,   Reference_Date,   Company_Idno,   Ledger_Idno    ,   StockOff_IdNo,   Cloth_IdNo,                             ClothType_IdNo                                                                                                                                                           , Folding ) " &
                '                      " Select                               'CLOTH'   , a.Reference_Code, a.Reference_Date, a.Company_IdNo, a.DeliveryTo_Idno, a.StockOff_IdNo, a.Cloth_IdNo, (CASE WHEN a.Meters_Type1 <> 0 THEN 1  WHEN a.Meters_Type2 <> 0 THEN 2 WHEN a.Meters_Type3 <> 0 THEN 3 WHEN a.Meters_Type4 <> 0 THEN 4 WHEN a.Meters_Type5 <> 0 THEN 5 ELSE 0 END  ) as ClothtypeIDNO, Folding from Stock_Cloth_Processing_Details a, Weaver_Cloth_Receipt_Head b Where b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(PkCondition_Weaver) & "' + b.Weaver_ClothReceipt_Code"
                'Nr = cmd.ExecuteNonQuery()

            End If

            cmd.CommandText = "Update Stock_Yarn_Processing_Details set Weight = b.ConsumedYarn_Receipt from Stock_Yarn_Processing_Details a, Weaver_Cloth_Receipt_Head b Where b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(PkCondition_Weaver) & "' + b.Weaver_ClothReceipt_Code and b.Weaver_Wages_Code = ''"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Stock_Pavu_Processing_Details set Meters = b.ConsumedPavu_Receipt from Stock_Pavu_Processing_Details a, Weaver_Cloth_Receipt_Head b Where b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(PkCondition_Weaver) & "' + b.Weaver_ClothReceipt_Code and b.Weaver_Wages_Code = ''"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Stock_Cloth_Processing_Details set reference_date = (case when b.Weaver_Wages_Code <> '' then b.Weaver_Wages_Date else b.Weaver_ClothReceipt_Date end), Cloth_IdNo = b.Cloth_IdNo, Folding = b.Folding_Receipt, UnChecked_Meters = (case when b.Weaver_Wages_Code = '' then b.ReceiptMeters_Receipt else 0 end), Meters_Type1 = (case when b.Weaver_Wages_Code <> '' then b.Type1_Wages_Meters else 0 end), Meters_Type2 = (case when b.Weaver_Wages_Code <> '' then b.Type2_Wages_Meters else 0 end), Meters_Type3 = (case when b.Weaver_Wages_Code <> '' then b.Type3_Wages_Meters else 0 end), Meters_Type4 = (case when b.Weaver_Wages_Code <> '' then b.Type4_Wages_Meters else 0 end), Meters_Type5 = (case when b.Weaver_Wages_Code <> '' then b.Type5_Wages_Meters else 0 end) from Stock_Cloth_Processing_Details a, Weaver_Cloth_Receipt_Head b Where b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(PkCondition_Weaver) & "' + b.Weaver_ClothReceipt_Code"
            cmd.ExecuteNonQuery()


            cmd.CommandText = "Update Stock_Cloth_Processing_Details set reference_date = b.Cloth_Purchase_Receipt_Date , Cloth_IdNo = b.Cloth_IdNo, Folding = b.Folding_Receipt, UnChecked_Meters = b.ReceiptMeters_Receipt ,  Meters_Type1 = 0, Meters_Type2 = 0 , Meters_Type3 = 0, Meters_Type4 = 0, Meters_Type5 = 0 from Stock_Cloth_Processing_Details a, Cloth_Purchase_Receipt_Head b Where b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(PkCondition_Purchase) & "' + b.Cloth_Purchase_Receipt_Code"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Stock_Cloth_Processing_Details set reference_date = b.ClothSales_Delivery_Return_Date , Cloth_IdNo = b.Cloth_IdNo, Folding = b.Folding_Return, UnChecked_Meters = b.ReturnMeters_Return ,  Meters_Type1 = 0, Meters_Type2 = 0 , Meters_Type3 = 0, Meters_Type4 = 0, Meters_Type5 = 0 from Stock_Cloth_Processing_Details a, ClothSales_Delivery_Return_Head b Where b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(PkCondition_DelvRet) & "' + b.ClothSales_Delivery_Return_Code"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Stock_Cloth_Processing_Details set reference_date = b.ClothSales_Return_Date , Cloth_IdNo = b.Cloth_IdNo, Folding = b.Folding_Return, UnChecked_Meters = b.ReturnMeters_Return ,  Meters_Type1 = 0, Meters_Type2 = 0 , Meters_Type3 = 0, Meters_Type4 = 0, Meters_Type5 = 0 from Stock_Cloth_Processing_Details a, ClothSales_Return_Head b Where b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = (case when a.Reference_Code LIKE '" & Trim(PkCondition_SalRetGST) & "%' then '' else '" & Trim(PkCondition_SalRetVAT) & "' end)  + b.ClothSales_Return_Code"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Stock_Cloth_Processing_Details set reference_date= c.ClothProcess_Receipt_Date, Cloth_IdNo = b.Item_To_Idno, Folding = b.Folding,UnChecked_Meters=b.Receipt_Meters,Meters_Type1=0,Meters_Type2=0,Meters_Type3 = 0, Meters_Type4 = 0, Meters_Type5 = 0 from Stock_Cloth_Processing_Details a, Textile_Processing_Receipt_Details b, Textile_Processing_Receipt_Head c Where c.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and b.Cloth_Processing_Receipt_Code = c.ClothProcess_Receipt_Code and a.Reference_Code = '" & Trim(PkCondition_PROFABRCPT) & "' + c.ClothProcess_Receipt_Code"
            cmd.ExecuteNonQuery()
            'cmd.CommandText = "Update Stock_Cloth_Processing_Details set reference_date = c.ClothProcess_Receipt_Date, Cloth_IdNo = b.Cloth_IdNo, Folding = b.Folding, UnChecked_Meters = b.Receipt_Meters , Meters_Type1 = 0, Meters_Type2 = 0 , Meters_Type3 = 0, Meters_Type4 = 0, Meters_Type5 = 0 from Stock_Cloth_Processing_Details a, Textile_Processing_Receipt_Details b, Textile_Processing_Receipt_Head c Where c.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and b.ClothProcess_Receipt_Code = c.ClothProcess_Receipt_Code and a.Reference_Code = '" & Trim(PkCondition_PROFABRCPT) & "' + c.ClothProcess_Receipt_Code "
            'cmd.ExecuteNonQuery()


            If Common_Procedures.settings.Multi_Godown_Status = 1 Then

                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@DelDate", dtp_Rec_Date.Value)

                If Len(Trim(txt_RecNo.Text)) >= 2 Then
                    If Microsoft.VisualBasic.Right(Trim(txt_RecNo.Text), 2) = "/P" Then
                        txt_RecNo.Text = Microsoft.VisualBasic.Left(Trim(txt_RecNo.Text), Len(Trim(txt_RecNo.Text)) - 2)
                    End If
                End If

                cmd.CommandText = "Update Textile_Processing_Delivery_Head set Total_Meters = " & Val(txt_Rec_Meter.Text).ToString & " where ClothProcess_Delivery_No = '" & txt_RecNo.Text & "' " &
                                              " and ClothProcess_Delivery_Date = @DelDate"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Textile_Processing_Delivery_Details set Delivery_Meters = " & Val(txt_Rec_Meter.Text).ToString & " where Cloth_Processing_Delivery_No = '" & txt_RecNo.Text & "' " &
                                              " and Cloth_Processing_Delivery_Date = @DelDate"
                cmd.ExecuteNonQuery()

            End If
            SQL1 = "Delete from Weaver_ClothReceipt_Piece_Details Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and Create_Status = 0"
            cmd.CommandText = "EXEC SP_ExecuteQuery '" & Replace(Trim(SQL1), "'", "''") & "'"
            cmd.ExecuteNonQuery()

            SQL1 = "Update Weaver_ClothReceipt_Piece_Details set Weaver_Piece_Checking_Code = '',  Weaver_Piece_Checking_No = '', Weaver_Piece_Checking_Date = Null, Receipt_Meters = ReceiptMeters_Receipt, ReceiptMeters_Checking = 0, Loom_No = '', Pick = 0, Width = 0, Type1_Meters = 0, Type2_Meters = 0, Type3_Meters = 0, Type4_Meters  = 0, Type5_Meters = 0, Total_Checking_Meters = 0, Weight = 0, Weight_Meter = 0 Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
            cmd.CommandText = "EXEC SP_ExecuteQuery '" & Replace(Trim(SQL1), "'", "''") & "'"
            cmd.ExecuteNonQuery()
            ''''cmd.CommandText = "Update Weaver_ClothReceipt_Piece_Details set Weaver_Piece_Checking_Code = '',  Weaver_Piece_Checking_No = '', Weaver_Piece_Checking_Date = Null, Receipt_Meters = ReceiptMeters_Receipt, ReceiptMeters_Checking = 0, Loom_No = '', Pick = 0, Width = 0, Type1_Meters = 0, Type2_Meters = 0, Type3_Meters = 0, Type4_Meters  = 0, Type5_Meters = 0, Total_Checking_Meters = 0, Weight = 0, Weight_Meter = 0 Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
            ''''cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Weaver_Piece_Checking_Code = '', Weaver_Piece_Checking_Increment = Weaver_Piece_Checking_Increment - 1, Weaver_Piece_Checking_Date = Null, Receipt_Meters = ReceiptMeters_Receipt, Folding = Folding_Receipt, Consumed_Yarn = ConsumedYarn_Receipt, Consumed_Pavu = ConsumedPavu_Receipt, Folding_Checking = 0, ReceiptMeters_Checking = 0, ConsumedYarn_Checking = 0, ConsumedPavu_Checking = 0, Type1_Checking_Meters = 0, Type2_Checking_Meters = 0, Type3_Checking_Meters = 0, Type4_Checking_Meters = 0, Type5_Checking_Meters = 0, Total_Checking_Meters = 0 Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            cmd.CommandText = "Update Cloth_Purchase_Receipt_Head set Weaver_Piece_Checking_Code = '', Weaver_Piece_Checking_Increment = Weaver_Piece_Checking_Increment - 1, Weaver_Piece_Checking_Date = Null, Receipt_Meters = ReceiptMeters_Receipt, Folding = Folding_Receipt, Folding_Checking = 0, ReceiptMeters_Checking = 0 , Type1_Checking_Meters = 0, Type2_Checking_Meters = 0, Type3_Checking_Meters = 0, Type4_Checking_Meters = 0, Type5_Checking_Meters = 0, Total_Checking_Meters = 0 Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update ClothSales_Return_Head set Weaver_Piece_Checking_Code = '', Weaver_Piece_Checking_Increment = Weaver_Piece_Checking_Increment - 1, Weaver_Piece_Checking_Date = Null, Return_Meters = ReturnMeters_Return, Folding = Folding_Return, Folding_Checking = 0, ReceiptMeters_Checking = 0 , Type1_Checking_Meters = 0, Type2_Checking_Meters = 0, Type3_Checking_Meters = 0, Type4_Checking_Meters = 0, Type5_Checking_Meters = 0, Total_Checking_Meters = 0 Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update ClothSales_Delivery_Return_Head set Weaver_Piece_Checking_Code = '', Weaver_Piece_Checking_Increment = Weaver_Piece_Checking_Increment - 1, Weaver_Piece_Checking_Date = Null, Return_Meters = ReturnMeters_Return, Folding = Folding_Return, Folding_Checking = 0, ReceiptMeters_Checking = 0 , Type1_Checking_Meters = 0, Type2_Checking_Meters = 0, Type3_Checking_Meters = 0, Type4_Checking_Meters = 0, Type5_Checking_Meters = 0, Total_Checking_Meters = 0 Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Textile_Processing_Receipt_Head set Weaver_Piece_Checking_Code = '', Folding_Checking = 0, ReceiptMeters_Checking = 0 , Type1_Checking_Meters = 0, Type2_Checking_Meters = 0, Type3_Checking_Meters = 0, Type4_Checking_Meters = 0, Type5_Checking_Meters = 0, Total_Checking_Meters = 0 Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            cmd.CommandText = "delete from Weaver_Piece_Checking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Cloth_Stock) = 1 Then
                If Common_Procedures.Check_is_Negative_Stock_Status(con, trans) = True Then Exit Sub
            End If

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

            If msk_date.Visible And msk_date.Enabled Then
                msk_date.Focus()
                msk_date.SelectionStart = 0

            ElseIf cbo_Weaver.Enabled And cbo_Weaver.Visible Then
                cbo_Weaver.Focus()

            ElseIf cbo_LoomType.Visible And cbo_LoomType.Enabled Then
                cbo_LoomType.Focus()

            End If

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead order by Ledger_DisplayName", con)
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
            txt_Filter_LotNo.Text = ""
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

            con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            con.Open()

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Piece_Checking_No from Weaver_Piece_Checking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and " & Other_Condition & " Order by for_Orderby, Weaver_Piece_Checking_No", con)
            dt = New DataTable
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As String = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_ChkNo.Text))

            con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            con.Open()

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Piece_Checking_No from Weaver_Piece_Checking_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and " & Other_Condition & " Order by for_Orderby, Weaver_Piece_Checking_No", con)
            dt = New DataTable
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

        Finally
            dt.Dispose()
            da.Dispose()

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As String = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_ChkNo.Text))

            con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            con.Open()

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Piece_Checking_No from Weaver_Piece_Checking_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and " & Other_Condition & " Order by for_Orderby desc, Weaver_Piece_Checking_No desc", con)
            dt = New DataTable
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

            con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            con.Open()

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Piece_Checking_No from Weaver_Piece_Checking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and " & Other_Condition & " Order by for_Orderby desc, Weaver_Piece_Checking_No desc", con)
            dt = New DataTable
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

            con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            con.Open()

            lbl_ChkNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Piece_Checking_Head", "Weaver_Piece_Checking_Code", "For_OrderBy", "(Receipt_Type <> 'L')", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_ChkNo.ForeColor = Color.Red

            dtp_Date.Text = Common_Procedures.get_Server_Date(con) ' Date.Today.ToShortDateString
            msk_date.Text = dtp_Date.Text
            Da = New SqlClient.SqlDataAdapter("select top 1 * from Weaver_Piece_Checking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Weaver_Piece_Checking_No desc", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1267" Then '---- BRT TEXTTILES (SOMANUR)
                    If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                        If Dt1.Rows(0).Item("Weaver_Piece_Checking_Date").ToString <> "" Then dtp_Date.Text = Dt1.Rows(0).Item("Weaver_Piece_Checking_Date").ToString
                    End If
                End If
            End If
            Dt1.Clear()


        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()

            If msk_date.Visible And msk_date.Enabled Then
                msk_date.Focus()
                msk_date.SelectionStart = 0

            ElseIf cbo_Weaver.Enabled And cbo_Weaver.Visible Then
                cbo_Weaver.Focus()

            ElseIf cbo_LoomType.Visible And cbo_LoomType.Enabled Then
                cbo_LoomType.Focus()

            End If

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String


        Try



            pnl_Back.Enabled = False
            pnl_OpenRecord.Visible = True
            pnl_OpenRecord.BringToFront()
            txt_ChkNoOpen.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            '---

        End Try


    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Weaver_Piece_Checking_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Weaver_Piece_Checking_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Weaver_Piece_Checking_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Chk No.", "FOR NEW CHK NO. INSERTION...")

            InvCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Weaver_Piece_Checking_No from Weaver_Piece_Checking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code = '" & Trim(InvCode) & "'", con)
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
                    MessageBox.Show("Invalid Chk No.", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_ChkNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT ...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim Da3 As New SqlClient.SqlDataAdapter
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim LotCd As String = ""
        Dim LotNo As String = ""
        Dim vClth_IdNo As Integer = 0
        Dim Wev_ID As Integer = 0
        Dim vChkr_ID As Integer = 0
        Dim vFoldr_ID As Integer = 0
        Dim Lm_ID As Integer = 0
        Dim Wdth_Typ As String = ""
        Dim vClth_TransTo_IdNo As Integer = 0
        Dim vSTKClth_IdNo As Integer = 0
        Dim Sno As Integer = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Partcls As String = ""
        Dim vDelv_ID As Integer, vRec_ID As Integer
        Dim vTot_RecMtrs As String

        Dim vTot_Typ1Mtrs As String
        Dim vTot_Typ2Mtrs As String
        Dim vTot_Typ3Mtrs As String
        Dim vTot_Typ5Mtrs As String
        Dim vTot_Typ4Mtrs As String
        Dim vTot_ChkMtrs As String
        Dim vTot_Wgt As String

        Dim vTot_100Fld_Typ1Mtrs As String
        Dim vTot_100Fld_Typ2Mtrs As String
        Dim vTot_100Fld_Typ3Mtrs As String
        Dim vTot_100Fld_Typ4Mtrs As String
        Dim vTot_100Fld_Typ5Mtrs As String
        Dim vTot_100Fld_ChkMtr As String
        Dim vTot_100Fld_ChkDetMtr2 As String = 0, vTot_100Fld_ExcShtDetMtr As String = 0

        Dim StkOff_ID As Integer = 0
        Dim Nr As Integer = 0
        Dim WagesCode As String = ""
        Dim ConsYarn As String = 0
        Dim ConsPavu As String = 0
        Dim vStkOf_Pos_IdNo As Integer = 0
        Dim Led_type As String = 0
        Dim vCloRec_Code As String = ""
        Dim vGod_ID As Integer = 0
        Dim vBrCode_Typ1 As String = "", vBrCode_Typ2 As String = "", vBrCode_Typ3 As String = "", vBrCode_Typ4 As String = "", vBrCode_Typ5 As String = ""
        Dim Verified_STS As String = ""
        Dim vOrdByNo As String = ""
        Dim SQL1 As String = ""
        Dim vOrdByRecNo As String = ""
        Dim vOrdByPieceNo As String = ""
        Dim vERRMSG As String = ""
        Dim Approved_Sts As Integer = 0
        Dim vCheck_id As Integer = 0
        Dim vFolder_id As Integer = 0
        Dim vPCSCHK_APPSTS As String = 0
        Dim vCREATED_DTTM_TXT As String = ""
        Dim vMODIFIED_DTTM_TXT As String = ""
        Dim WftCnt_ID As Integer
        Dim EdsCnt_ID As Integer




        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_ChkNo.Text)

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        '  If Common_Procedures.UserRight_Check(Common_Procedures.UR.Weaver_Piece_Checking_Entry, New_Entry) = False Then Exit Sub
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If vWARP_WEFT_STOCK_UPDATION_STATUS = False Then

            If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Weaver_Piece_Checking_Entry, New_Entry, Me, con, "Weaver_Piece_Checking_Head", "Weaver_Piece_Checking_Code", NewCode, "Weaver_Piece_Checking_Date", "(Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Weaver_Piece_Checking_No desc", dtp_Date.Value.Date) = False Then Exit Sub

            If Common_Procedures.settings.Vefified_Status = 1 Then
                If Not (Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_Verified_Status = 1) Then
                    NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

                    If Val(Common_Procedures.get_FieldValue(con, "Weaver_Piece_Checking_Head", "Verified_Status", "(Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "')")) = 1 Then
                        MessageBox.Show("Entry Already Verified", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub
                    End If

                End If

            End If

        End If


        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
            Exit Sub
        End If

        If IsDate(dtp_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If

        If Not (Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" And Trim(Common_Procedures.FnYearCode) = "19-20") Then
            If Not (Convert.ToDateTime(msk_date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_date.Text) <= Common_Procedures.Company_ToDate) Then
                MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
                Exit Sub
            End If
        End If

        If Convert.ToDateTime(msk_date.Text) < Convert.ToDateTime(msk_Rec_Date.Text) Then
            MessageBox.Show("Invalid Checking Date - Should not be lesse than Receipt Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
            Exit Sub
        End If

        If Val(txt_Folding.Text) = 0 Then
            txt_Folding.Text = 100
        End If

        Wev_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Weaver.Text)
        If Wev_ID = 0 Then
            MessageBox.Show("Invalid Weaver Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Weaver.Enabled Then cbo_Weaver.Focus()
            Exit Sub
        End If

        vClth_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Quality.Text)
        If vClth_IdNo = 0 Then
            MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Quality.Enabled Then cbo_Quality.Focus()
            Exit Sub
        End If


        vClth_TransTo_IdNo = 0
        If cbo_Cloth_TransferTo.Visible = True Then
            vClth_TransTo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth_TransferTo.Text)
            'If vClth_TransTo_IdNo = 0 Then
            '    MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            '    If cbo_clothTo.Enabled Then cbo_clothTo.Focus()
            '    Exit Sub
            'End If
        End If

        vSTKClth_IdNo = vClth_IdNo
        If vClth_TransTo_IdNo <> 0 Then vSTKClth_IdNo = vClth_TransTo_IdNo


        Dim vChkr_Wgs_per_Mtr As String = ""
        Dim vFldr_Wgs_per_Mtr As String = ""

        Da3 = New SqlClient.SqlDataAdapter("select a.Checking_Wages_Meter, a.Folding_Wages_Meter from LoomType_Head a INNER JOIN cloth_head b ON a.loomType_idno = b.loom_Type_idno where b.cloth_idno = " & vClth_IdNo & " ", con)
        dt3 = New DataTable
        Da3.Fill(dt3)
        If dt3.Rows.Count > 0 Then
            vChkr_Wgs_per_Mtr = Val(dt3.Rows(0).Item("Checking_Wages_Meter").ToString)
            vFldr_Wgs_per_Mtr = Val(dt3.Rows(0).Item("Folding_Wages_Meter").ToString)
        End If
        dt3.Clear()

        If Common_Procedures.settings.CustomerCode = "1516" Then

            StkOff_ID = Common_Procedures.CommonLedger.OwnSort_Ac  ' Val(Common_Procedures.CommonLedger.Godown_Ac)

        Else
        StkOff_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, Lbl_StockOff.Text)
        If Lbl_StockOff.Visible = True Then
            If StkOff_ID = 0 Then
                MessageBox.Show("Invalid StockOf Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_Weaver.Enabled Then cbo_Weaver.Focus()
                Exit Sub
            End If
        End If

        End If
        vGod_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, lbl_Godown.Text)
        If lbl_Godown.Visible = True Then
            If vGod_ID = 0 Then
                lbl_Godown.Text = Common_Procedures.Ledger_IdNoToName(con, Val(Common_Procedures.CommonLedger.Godown_Ac))
            End If
        End If
        If vGod_ID = 0 Then vGod_ID = Common_Procedures.CommonLedger.Godown_Ac

        If Trim(UCase(lbl_RecPkCondition.Text)) = "WCLRC-" Then
            If cbo_LoomType.Visible = True Then
                If Trim(cbo_LoomType.Text) = "" Then
                    MessageBox.Show("Invalid Loom Type", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_LoomType.Enabled Then cbo_LoomType.Focus()
                    Exit Sub
                End If
            End If
        End If


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then
            If Val(txt_Rec_Meter.Text) = 0 Then
                MessageBox.Show("Invalid Receipt Meters in Cloth Receipt", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_Rec_Meter.Enabled Then
                    txt_Rec_Meter.Focus()
                ElseIf cbo_Weaver.Enabled Then
                    cbo_Weaver.Focus()
                Else
                    msk_date.Focus()
                End If
                Exit Sub
            End If
        End If


        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then

        '    If Trim(Common_Procedures.FnYearCode) <> "19-20" Then

        '        Dim vPcsCk_Mnth = Month(Convert.ToDateTime(msk_date.Text))
        '        Dim vPcsCk_Yr = Year(Convert.ToDateTime(msk_date.Text))
        '        Dim vCloRec_Mnth = Month(Convert.ToDateTime(msk_Rec_Date.Text))
        '        Dim vCloRec_yr = Year(Convert.ToDateTime(msk_Rec_Date.Text))
        '        If vPcsCk_Mnth <> vCloRec_Mnth Or vPcsCk_Yr <> vCloRec_yr Then
        '            MessageBox.Show("Invalid Checking Date " & vbCrLf & " Month of Checking Date and Cloth Receipt Date should be equal", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
        '            Exit Sub
        '        End If

        '    End If

        'End If


        With dgv_Details

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.TOTALMETER).Value) <> 0 Then

                    If Trim(.Rows(i).Cells(dgvCOL_PCSDETAILS.PCSNO).Value) = "" Then
                        MessageBox.Show("Invalid Piece No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(dgvCOL_PCSDETAILS.PCSNO)
                        End If
                        Exit Sub
                    End If

                    Check_Meter_Range_Condition_for_ClothTYpes(i)

                End If

            Next

        End With


        Verified_STS = 0
        If chk_Verified_Status.Checked = True Then Verified_STS = 1

        Approved_Sts = 0
        If Chk_Approved_Sts.Checked = True Then Approved_Sts = 1

        NoCalc_Status = False

        Total_Calculation1()

        vTot_RecMtrs = 0 : vTot_Typ1Mtrs = 0 : vTot_Typ2Mtrs = 0 : vTot_Typ3Mtrs = 0 : vTot_Typ5Mtrs = 0 : vTot_Typ4Mtrs = 0 : vTot_ChkMtrs = 0 : vTot_Wgt = 0
        vTot_100Fld_ExcShtDetMtr = 0
        With dgv_Details_Total1

            If .RowCount > 0 Then

                vTot_RecMtrs = Format(Val(.Rows(0).Cells(dgvCOL_PCSDETAILS.RECEIPTMETER).Value), "##########0.00")
                vTot_Typ1Mtrs = Format(Val(.Rows(0).Cells(dgvCOL_PCSDETAILS.TYPE1METER).Value), "##########0.00")
                vTot_Typ2Mtrs = Format(Val(.Rows(0).Cells(dgvCOL_PCSDETAILS.TYPE2METER).Value), "##########0.00")
                vTot_Typ3Mtrs = Format(Val(.Rows(0).Cells(dgvCOL_PCSDETAILS.TYPE3METER).Value), "##########0.00")
                vTot_Typ4Mtrs = Format(Val(.Rows(0).Cells(dgvCOL_PCSDETAILS.TYPE4METER).Value), "##########0.00")
                vTot_Typ5Mtrs = Format(Val(.Rows(0).Cells(dgvCOL_PCSDETAILS.TYPE5METER).Value), "##########0.00")
                vTot_ChkMtrs = Format(Val(.Rows(0).Cells(dgvCOL_PCSDETAILS.TOTALMETER).Value), "##########0.00")
                vTot_Wgt = Format(Val(.Rows(0).Cells(dgvCOL_PCSDETAILS.WEIGHT).Value), "##########0.000")
                vTot_100Fld_ExcShtDetMtr = Format(Val(.Rows(0).Cells(dgvCOL_PCSDETAILS.EXCESSHORTMETER).Value), "##########0.00")
            End If

        End With


        vTot_100Fld_Typ1Mtrs = 0 : vTot_100Fld_Typ2Mtrs = 0 : vTot_100Fld_Typ3Mtrs = 0 : vTot_100Fld_Typ4Mtrs = 0 : vTot_100Fld_Typ5Mtrs = 0 : vTot_100Fld_ChkMtr = 0
        vTot_100Fld_ChkDetMtr2 = 0
        With dgv_Details_Total2
            If .RowCount > 0 Then

                vTot_100Fld_Typ1Mtrs = Format(Val(.Rows(0).Cells(dgvCOL_PCSDETAILS.TYPE1METER).Value), "##########0.00")
                vTot_100Fld_Typ2Mtrs = Format(Val(.Rows(0).Cells(dgvCOL_PCSDETAILS.TYPE2METER).Value), "##########0.00")
                vTot_100Fld_Typ3Mtrs = Format(Val(.Rows(0).Cells(dgvCOL_PCSDETAILS.TYPE3METER).Value), "##########0.00")
                vTot_100Fld_Typ4Mtrs = Format(Val(.Rows(0).Cells(dgvCOL_PCSDETAILS.TYPE4METER).Value), "##########0.00")
                vTot_100Fld_Typ5Mtrs = Format(Val(.Rows(0).Cells(dgvCOL_PCSDETAILS.TYPE5METER).Value), "##########0.00")
                vTot_100Fld_ChkMtr = Format(Val(.Rows(0).Cells(dgvCOL_PCSDETAILS.TOTALMETER).Value), "##########0.00")

                vTot_100Fld_ChkDetMtr2 = Format(Val(.Rows(0).Cells(dgvCOL_PCSDETAILS.TOTALMETERIN100FOLDING).Value), "##########0.00")


            End If

        End With

        WftCnt_ID = 0
        EdsCnt_ID = 0

        Da = New SqlClient.SqlDataAdapter("select Weaver_IR_Wages_Code, Weaver_Wages_Code, Loom_IdNo, Width_Type, Count_IdNo, EndsCount_IdNo from Weaver_Cloth_Receipt_Head Where Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        WagesCode = ""
        Lm_ID = 0
        Wdth_Typ = ""
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("Weaver_Wages_Code").ToString) = False Then
                WagesCode = Dt1.Rows(0).Item("Weaver_Wages_Code").ToString
            End If
            If Trim(WagesCode) = "" Then
                If IsDBNull(Dt1.Rows(0).Item("Weaver_IR_Wages_Code").ToString) = False Then
                    WagesCode = Dt1.Rows(0).Item("Weaver_IR_Wages_Code").ToString
                End If
            End If
            Lm_ID = Val(Dt1.Rows(0).Item("Loom_IdNo").ToString)
            Wdth_Typ = Dt1.Rows(0).Item("Width_Type").ToString
            If IsDBNull(Dt1.Rows(0).Item("Count_IdNo").ToString) = False Then
                WftCnt_ID = Val(Dt1.Rows(0).Item("Count_IdNo").ToString)
            End If
            If IsDBNull(Dt1.Rows(0).Item("EndsCount_IdNo").ToString) = False Then
                EdsCnt_ID = Val(Dt1.Rows(0).Item("EndsCount_IdNo").ToString)
            End If
        End If
        Dt1.Clear()


        If lbl_LotNo.Visible = False Then
            lbl_LotNo.Text = txt_RecNo.Text
        End If

        vCloRec_Code = ""
        If InStr(1, Trim(UCase(lbl_RecCode.Text)), Trim(UCase(lbl_RecPkCondition.Text))) > 0 Then
            vCloRec_Code = Trim(lbl_RecCode.Text)
        Else
            vCloRec_Code = Trim(lbl_RecPkCondition.Text) & Trim(lbl_RecCode.Text)
        End If

        NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        If vWARP_WEFT_STOCK_UPDATION_STATUS = False Then

            vPCSCHK_APPSTS = 0
            Da = New SqlClient.SqlDataAdapter("Select Approved_Status from Weaver_Piece_Checking_Head a where a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' ", con)
            Dt2.Clear()
            Da.Fill(Dt2)
            If Dt2.Rows.Count > 0 Then
                vPCSCHK_APPSTS = Dt2.Rows(0).Item("Approved_Status").ToString
            End If
            Dt2.Clear()
            If Val(vPCSCHK_APPSTS) = 1 Then
                MessageBox.Show("Invalid editing : Already Approved", "DOES NOT EDIT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_Weaver.Enabled Then
                    cbo_Weaver.Focus()

                Else

                    If dgv_Details.Enabled And dgv_Details.Visible Then

                        If dgv_Details.Rows.Count > 0 Then
                            dgv_Details.Focus()
                            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCOL_PCSDETAILS.PCSNO)
                        End If

                    End If

                End If

                Exit Sub

            End If

        End If

        vCheck_id = Common_Procedures.Employee_Simple_NameToIdNo(con, cbo_checker.Text)
        vFolder_id = Common_Procedures.Employee_Simple_NameToIdNo(con, cbo_Folder.Text)

        vCREATED_DTTM_TXT = ""
        vMODIFIED_DTTM_TXT = ""

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_ChkNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Piece_Checking_Head", "Weaver_Piece_Checking_Code", "For_OrderBy", "(Receipt_Type <> 'L')", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)
                'lbl_ChkNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Piece_Checking_Head", "Weaver_Piece_Checking_Code", "For_OrderBy", Other_Condition, Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@CheckingDate", Convert.ToDateTime(msk_date.Text))
            cmd.Parameters.AddWithValue("@RecDate", Convert.ToDateTime(msk_Rec_Date.Text))
            cmd.Parameters.AddWithValue("@DelDate", Convert.ToDateTime(msk_Rec_Date.Text))

            cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
            cmd.ExecuteNonQuery()


            If New_Entry = True Then

                vCREATED_DTTM_TXT = Trim(Format(Now, "dd-MM-yyyy hh:mm tt"))
                cmd.Parameters.AddWithValue("@createddatetime", Now)

                cmd.CommandText = "Insert into Weaver_Piece_Checking_Head (              Receipt_Type         , Weaver_Piece_Checking_Code,               Company_IdNo       ,     Weaver_Piece_Checking_No  ,                               for_OrderBy                              , Weaver_Piece_Checking_Date,       Ledger_IdNo       ,           Receipt_PkCondition          ,         Piece_Receipt_Code     ,         Piece_Receipt_No       , Piece_Receipt_Date,             Lot_No            ,          Cloth_IdNo       ,             Party_DcNo        ,             noof_pcs             ,             ReceiptMeters_Receipt   ,               Folding              , Total_Checking_Receipt_Meters ,           Total_Type1_Meters    ,      Total_Type2_Meters         ,   Total_Type3_Meters           ,     Total_Type4_Meters          ,     Total_Type5_Meters         ,       Total_Checking_Meters   ,        Total_Weight       ,  Total_Type1Meters_100Folding           , Total_Type2Meters_100Folding             ,  Total_Type3Meters_100Folding         ,    Total_Type4Meters_100Folding        ,     Total_Type5Meters_100Folding      ,      Total_Meters_100Folding         ,         Excess_Short_Meter              , StockOff_IdNo               ,                           user_idNo      ,       Verified_Status    ,  Total_Checking_Details_Meters_100Folding ,     Total_ExcessShort_Details_Meters ,        Approved_Status         ,   Checker_Idno    ,               Folder_Idno            ,          Cloth_TransferTo_Idno      ,               Loom_Type          ,                    created_useridno           ,   created_DateTime,          created_DateTime_Text    , Last_modified_useridno, Last_modified_DateTime, Last_modified_DateTime_Text ) " &
                                            "     Values                  ( '" & Trim(UCase(vRcptType)) & "'  ,   '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_ChkNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_ChkNo.Text))) & ",        @CheckingDate      , " & Str(Val(Wev_ID)) & ", '" & Trim(lbl_RecPkCondition.Text) & "', '" & Trim(lbl_RecCode.Text) & "', '" & Trim(txt_RecNo.Text) & "',      @RecDate     , '" & Trim(lbl_LotNo.Text) & "',  " & Str(Val(vClth_IdNo)) & ", '" & Trim(txt_PDcNo.Text) & "', " & Str(Val(txt_No_Pcs.Text)) & ", " & Str(Val(txt_Rec_Meter.Text)) & ",  " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(vTot_RecMtrs)) & ",  " & Str(Val(vTot_Typ1Mtrs)) & ",  " & Str(Val(vTot_Typ2Mtrs)) & ", " & Str(Val(vTot_Typ3Mtrs)) & ",  " & Str(Val(vTot_Typ4Mtrs)) & ", " & Str(Val(vTot_Typ5Mtrs)) & ", " & Str(Val(vTot_ChkMtrs)) & ", " & Str(Val(vTot_Wgt)) & ", " & Str(Val(vTot_100Fld_Typ1Mtrs)) & "  ,    " & Str(Val(vTot_100Fld_Typ2Mtrs)) & ", " & Str(Val(vTot_100Fld_Typ3Mtrs)) & ",  " & Str(Val(vTot_100Fld_Typ4Mtrs)) & ", " & Str(Val(vTot_100Fld_Typ5Mtrs)) & ",  " & Str(Val(vTot_100Fld_ChkMtr)) & ", " & Str(Val(txt_Excess_Short.Text)) & " , " & Str(Val(StkOff_ID)) & " , " & Val(Common_Procedures.User.IdNo) & " , " & Val(Verified_STS) & ",     " & Val(vTot_100Fld_ChkDetMtr2) & "   , " & Val(vTot_100Fld_ExcShtDetMtr) & " , " & Val(Approved_Sts) & " ,  " & Str(Val(vCheck_id)) & ", " & Str(Val(vFolder_id)) & ", " & Str(Val(vClth_TransTo_IdNo)) & ", '" & Trim(cbo_LoomType.Text) & "',  " & Str(Val(Common_Procedures.User.IdNo)) & ",  @createddatetime ,  '" & Trim(vCREATED_DTTM_TXT) & "',              0        ,     NUll              ,          ''                 ) "
                cmd.ExecuteNonQuery()

            Else

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", " Weaver_Piece_Checking_Head", " Weaver_Piece_Checking_Code", Val(lbl_Company.Tag), NewCode, lbl_ChkNo.Text, Val(vOrdByNo), PkCondition_Entry, "", "", New_Entry, False, "", "", " Weaver_Piece_Checking_Code, Company_IdNo, for_OrderBy", tr)

                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", " Weaver_ClothReceipt_Piece_Details", " Weaver_Piece_Checking_Code", Val(lbl_Company.Tag), NewCode, lbl_ChkNo.Text, Val(vOrdByNo), PkCondition_Entry, "", "", New_Entry, False, " Piece_No,ReceiptMeters_Checking,Receipt_Meters,Loom_No,Pick,Width,Type1_Meters,Type2_Meters,Type3_Meters,Type4_Meters,Type5_Meters,Total_Checking_Meters,Weight,Weight_Meter,Remarks,WareHouse_IdNo,Checked_Pcs_Barcode_Type1 ,Checked_Pcs_Barcode_Type2 ,   Checked_Pcs_Barcode_Type3 ,   Checked_Pcs_Barcode_Type4 ,   Checked_Pcs_Barcode_Type5  ,CHecker_idno,Folder_idno ,Checker_Wgs_per_Mtr,Folder_Wgs_per_Mtr", "Sl_No", " Weaver_Piece_Checking_Code, For_OrderBy, Company_IdNo,  Weaver_Piece_Checking_No,  Weaver_Piece_Checking_Date, Ledger_Idno", tr)

                vMODIFIED_DTTM_TXT = Trim(Format(Now, "dd-MM-yyyy hh:mm tt"))
                cmd.Parameters.AddWithValue("@modifieddatetime", Now)

                cmd.CommandText = "Update Weaver_Piece_Checking_Head set Receipt_Type = '" & Trim(UCase(vRcptType)) & "', Weaver_Piece_Checking_Date = @CheckingDate, Ledger_IdNo = " & Str(Val(Wev_ID)) & ", Receipt_PkCondition = '" & Trim(lbl_RecPkCondition.Text) & "', Piece_Receipt_Code = '" & Trim(lbl_RecCode.Text) & "', Piece_Receipt_No = '" & Trim(txt_RecNo.Text) & "', Piece_Receipt_Date = @RecDate, Lot_No = '" & Trim(lbl_LotNo.Text) & "', Cloth_IdNo = " & Str(Val(vClth_IdNo)) & ", Party_DcNo = '" & Trim(txt_PDcNo.Text) & "', noof_pcs = " & Str(Val(txt_No_Pcs.Text)) & ", ReceiptMeters_Receipt = " & Str(Val(txt_Rec_Meter.Text)) & ", Folding =  " & Str(Val(txt_Folding.Text)) & ", Total_Checking_Receipt_Meters =  " & Str(Val(vTot_RecMtrs)) & ", Total_Type1_Meters = " & Str(Val(vTot_Typ1Mtrs)) & ",  Total_Type2_Meters = " & Str(Val(vTot_Typ2Mtrs)) & ", Total_Type3_Meters = " & Str(Val(vTot_Typ3Mtrs)) & ", Total_Type4_Meters = " & Str(Val(vTot_Typ4Mtrs)) & ", Total_Type5_Meters = " & Str(Val(vTot_Typ5Mtrs)) & ", Total_Checking_Meters = " & Str(Val(vTot_ChkMtrs)) & ", Total_Weight = " & Str(Val(vTot_Wgt)) & ", Total_Type1Meters_100Folding = " & Str(Val(vTot_100Fld_Typ1Mtrs)) & ", Total_Type2Meters_100Folding = " & Str(Val(vTot_100Fld_Typ2Mtrs)) & ", Total_Type3Meters_100Folding = " & Str(Val(vTot_100Fld_Typ3Mtrs)) & ", Total_Type4Meters_100Folding = " & Str(Val(vTot_100Fld_Typ4Mtrs)) & ", Total_Type5Meters_100Folding = " & Str(Val(vTot_100Fld_Typ5Mtrs)) & ", Total_Meters_100Folding  =  " & Str(Val(vTot_100Fld_ChkMtr)) & ", Excess_Short_Meter = " & Str(Val(txt_Excess_Short.Text)) & " , StockOff_IdNo = " & Str(Val(StkOff_ID)) & ", User_idNo = " & Val(Common_Procedures.User.IdNo) & ", Verified_Status= " & Val(Verified_STS) & ", Total_Checking_Details_Meters_100Folding = " & Val(vTot_100Fld_ChkDetMtr2) & ", Total_ExcessShort_Details_Meters = " & Val(vTot_100Fld_ExcShtDetMtr) & " , Approved_Status = " & Val(Approved_Sts) & " ,  Checker_Idno  = " & Str(Val(vCheck_id)) & "  ,   Folder_Idno = " & Str(Val(vFolder_id)) & ", Cloth_TransferTo_Idno = " & Str(Val(vClth_TransTo_IdNo)) & " , Loom_Type = '" & Trim(cbo_LoomType.Text) & "', Last_modified_useridno = " & Str(Val(Common_Procedures.User.IdNo)) & ", Last_modified_DateTime = @modifieddatetime, Last_modified_DateTime_Text = '" & Trim(vMODIFIED_DTTM_TXT) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Stock_Yarn_Processing_Details set Weight = b.ConsumedYarn_Receipt from Stock_Yarn_Processing_Details a, Weaver_Cloth_Receipt_Head b Where b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(PkCondition_Weaver) & "' + b.Weaver_ClothReceipt_Code and b.Weaver_Wages_Code = '' and b.Weaver_IR_Wages_Code = ''"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Stock_Pavu_Processing_Details set Meters = b.ConsumedPavu_Receipt from Stock_Pavu_Processing_Details a, Weaver_Cloth_Receipt_Head b Where b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(PkCondition_Weaver) & "' + b.Weaver_ClothReceipt_Code and b.Weaver_Wages_Code = '' and b.Weaver_IR_Wages_Code = ''"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Stock_Cloth_Processing_Details set reference_date = (case when b.Weaver_Wages_Code <> '' then b.Weaver_Wages_Date else b.Weaver_ClothReceipt_Date end), Cloth_IdNo = b.Cloth_IdNo, Folding = b.Folding_Receipt, UnChecked_Meters = (case when b.Weaver_Wages_Code = '' then b.ReceiptMeters_Receipt else 0 end), Meters_Type1 = (case when b.Weaver_Wages_Code <> '' then b.Type1_Wages_Meters else 0 end), Meters_Type2 = (case when b.Weaver_Wages_Code <> '' then b.Type2_Wages_Meters else 0 end), Meters_Type3 = (case when b.Weaver_Wages_Code <> '' then b.Type3_Wages_Meters else 0 end), Meters_Type4 = (case when b.Weaver_Wages_Code <> '' then b.Type4_Wages_Meters else 0 end), Meters_Type5 = (case when b.Weaver_Wages_Code <> '' then b.Type5_Wages_Meters else 0 end) from Stock_Cloth_Processing_Details a, Weaver_Cloth_Receipt_Head b Where b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(PkCondition_Weaver) & "' + b.Weaver_ClothReceipt_Code"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Stock_Cloth_Processing_Details set reference_date = b.Cloth_Purchase_Receipt_Date , Cloth_IdNo = b.Cloth_IdNo, Folding = b.Folding_Receipt, UnChecked_Meters = b.ReceiptMeters_Receipt ,  Meters_Type1 = 0, Meters_Type2 = 0 , Meters_Type3 = 0, Meters_Type4 = 0, Meters_Type5 = 0 from Stock_Cloth_Processing_Details a, Cloth_Purchase_Receipt_Head b Where b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(PkCondition_Purchase) & "' + b.Cloth_Purchase_Receipt_Code"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Stock_Cloth_Processing_Details set reference_date = b.ClothSales_Delivery_Return_Date , Cloth_IdNo = b.Cloth_IdNo, Folding = b.Folding_Return, UnChecked_Meters = b.ReturnMeters_Return ,  Meters_Type1 = 0, Meters_Type2 = 0 , Meters_Type3 = 0, Meters_Type4 = 0, Meters_Type5 = 0 from Stock_Cloth_Processing_Details a, ClothSales_Delivery_Return_Head b Where b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(PkCondition_DelvRet) & "' + b.ClothSales_Delivery_Return_Code"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Stock_Cloth_Processing_Details set reference_date = b.ClothSales_Return_Date , Cloth_IdNo = b.Cloth_IdNo, Folding = b.Folding_Return, UnChecked_Meters = b.ReturnMeters_Return ,  Meters_Type1 = 0, Meters_Type2 = 0 , Meters_Type3 = 0, Meters_Type4 = 0, Meters_Type5 = 0 from Stock_Cloth_Processing_Details a, ClothSales_Return_Head b Where b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = (case when a.Reference_Code LIKE '" & Trim(PkCondition_SalRetGST) & "%' then '' else '" & Trim(PkCondition_SalRetVAT) & "' end)  + b.ClothSales_Return_Code"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Stock_Cloth_Processing_Details set reference_date= c.ClothProcess_Receipt_Date, Folding = b.Folding,UnChecked_Meters=b.Receipt_Meters,Meters_Type1=0,Meters_Type2=0,Meters_Type3 = 0, Meters_Type4 = 0, Meters_Type5 = 0 from Stock_Cloth_Processing_Details a, Textile_Processing_Receipt_Details b, Textile_Processing_Receipt_Head c Where c.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and b.Cloth_Processing_Receipt_Code = c.ClothProcess_Receipt_Code and a.Reference_Code = '" & Trim(PkCondition_PROFABRCPT) & "' + c.ClothProcess_Receipt_Code"
                cmd.ExecuteNonQuery()

                SQL1 = "Delete from Weaver_ClothReceipt_Piece_Details Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and Create_Status = 0 and PackingSlip_Code_Type1 = '' and PackingSlip_Code_Type2 = ''  and PackingSlip_Code_Type3 = ''  and PackingSlip_Code_Type4 = ''  and PackingSlip_Code_Type5 = '' and BuyerOffer_Code_Type1 = '' and BuyerOffer_Code_Type2 = '' and BuyerOffer_Code_Type3 = '' and BuyerOffer_Code_Type4 = '' and BuyerOffer_Code_Type5 = ''"
                cmd.CommandText = "EXEC SP_ExecuteQuery '" & Replace(Trim(SQL1), "'", "''") & "'"
                cmd.ExecuteNonQuery()

                'cmd.CommandText = "Delete from Weaver_ClothReceipt_Piece_Details Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and Create_Status = 0 and PackingSlip_Code_Type1 = '' and PackingSlip_Code_Type2 = ''  and PackingSlip_Code_Type3 = ''  and PackingSlip_Code_Type4 = ''  and PackingSlip_Code_Type5 = '' and BuyerOffer_Code_Type1 = '' and BuyerOffer_Code_Type2 = '' and BuyerOffer_Code_Type3 = '' and BuyerOffer_Code_Type4 = '' and BuyerOffer_Code_Type5 = ''"
                'cmd.ExecuteNonQuery()

                SQL1 = "Update Weaver_ClothReceipt_Piece_Details set Weaver_Piece_Checking_Code = '',  Weaver_Piece_Checking_No = '', Weaver_Piece_Checking_Date = Null, Receipt_Meters = ReceiptMeters_Receipt, ReceiptMeters_Checking = 0, Loom_No = '', Pick = 0, Width = 0, Type1_Meters = 0, Type2_Meters = 0, Type3_Meters = 0, Type4_Meters  = 0, Type5_Meters = 0, Total_Checking_Meters = 0, Weight = 0, Weight_Meter = 0, Total_CheckingMeters_100Folding = 0, ExcessShort_Status_YesNo = '', Excess_Short_Meter = 0, BeamNo_SetCode = '' Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
                cmd.CommandText = "EXEC SP_ExecuteQuery '" & Replace(Trim(SQL1), "'", "''") & "'"
                cmd.ExecuteNonQuery()

                'cmd.CommandText = "Update Weaver_ClothReceipt_Piece_Details set Weaver_Piece_Checking_Code = '',  Weaver_Piece_Checking_No = '', Weaver_Piece_Checking_Date = Null, Receipt_Meters = ReceiptMeters_Receipt, ReceiptMeters_Checking = 0, Loom_No = '', Pick = 0, Width = 0, Type1_Meters = 0, Type2_Meters = 0, Type3_Meters = 0, Type4_Meters  = 0, Type5_Meters = 0, Total_Checking_Meters = 0, Weight = 0, Weight_Meter = 0 Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
                'cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Weaver_Piece_Checking_Code = '', Weaver_Piece_Checking_Increment = Weaver_Piece_Checking_Increment - 1, Weaver_Piece_Checking_Date = Null, Receipt_Meters = ReceiptMeters_Receipt, Folding = Folding_Receipt, Consumed_Yarn = ConsumedYarn_Receipt, Consumed_Pavu = ConsumedPavu_Receipt, Folding_Checking = 0, ReceiptMeters_Checking = 0, ConsumedYarn_Checking = 0, ConsumedPavu_Checking = 0, Type1_Checking_Meters = 0, Type2_Checking_Meters = 0, Type3_Checking_Meters = 0, Type4_Checking_Meters = 0, Type5_Checking_Meters = 0, Total_Checking_Meters = 0 Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Cloth_Purchase_Receipt_Head set Weaver_Piece_Checking_Code = '', Weaver_Piece_Checking_Increment = Weaver_Piece_Checking_Increment - 1, Weaver_Piece_Checking_Date = Null, Receipt_Meters = ReceiptMeters_Receipt, Folding = Folding_Receipt, Folding_Checking = 0, ReceiptMeters_Checking = 0 , Type1_Checking_Meters = 0, Type2_Checking_Meters = 0, Type3_Checking_Meters = 0, Type4_Checking_Meters = 0, Type5_Checking_Meters = 0, Total_Checking_Meters = 0 Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update ClothSales_Return_Head set Weaver_Piece_Checking_Code = '', Weaver_Piece_Checking_Increment = Weaver_Piece_Checking_Increment - 1, Weaver_Piece_Checking_Date = Null, Return_Meters = ReturnMeters_Return, Folding = Folding_Return, Folding_Checking = 0, ReceiptMeters_Checking = 0 , Type1_Checking_Meters = 0, Type2_Checking_Meters = 0, Type3_Checking_Meters = 0, Type4_Checking_Meters = 0, Type5_Checking_Meters = 0, Total_Checking_Meters = 0 Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update ClothSales_Delivery_Return_Head set Weaver_Piece_Checking_Code = '', Weaver_Piece_Checking_Increment = Weaver_Piece_Checking_Increment - 1, Weaver_Piece_Checking_Date = Null, Return_Meters = ReturnMeters_Return, Folding = Folding_Return, Folding_Checking = 0, ReceiptMeters_Checking = 0 , Type1_Checking_Meters = 0, Type2_Checking_Meters = 0, Type3_Checking_Meters = 0, Type4_Checking_Meters = 0, Type5_Checking_Meters = 0, Total_Checking_Meters = 0 Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Textile_Processing_Receipt_Head set Weaver_Piece_Checking_Code = '', Folding_Checking = 0, ReceiptMeters_Checking = 0 , Type1_Checking_Meters = 0, Type2_Checking_Meters = 0, Type3_Checking_Meters = 0, Type4_Checking_Meters = 0, Type5_Checking_Meters = 0, Total_Checking_Meters = 0 Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", " Weaver_Piece_Checking_Head", " Weaver_Piece_Checking_Code", Val(lbl_Company.Tag), NewCode, lbl_ChkNo.Text, Val(vOrdByNo), PkCondition_Entry, "", "", New_Entry, False, "", "", " Weaver_Piece_Checking_Code, Company_IdNo, for_OrderBy", tr)

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1234" And (Trim(UCase(cbo_LoomType.Text)) = "AUTO LOOM" Or Trim(UCase(cbo_LoomType.Text)) = "AUTOLOOM") Then '---- ARULJOTHI EXPORTS PVT LTD (SOMANUR)
                ConsYarn = Val(Common_Procedures.get_Weft_ConsumedYarn(con, vClth_IdNo, Val(vTot_100Fld_ChkDetMtr2), tr))
                ConsPavu = Val(Common_Procedures.get_Pavu_Consumption(con, vClth_IdNo, Lm_ID, Val(vTot_100Fld_ChkDetMtr2), Trim(Wdth_Typ), tr))

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT Textiles (Somanur)
                ConsYarn = Val(Common_Procedures.get_Weft_ConsumedYarn(con, vClth_IdNo, Val(vTot_ChkMtrs), tr))
                If Trim(UCase(cbo_LoomType.Text)) = "POWER LOOM" Or Trim(UCase(cbo_LoomType.Text)) = "POWERLOOM" Then
                    ConsPavu = Val(Common_Procedures.get_Pavu_Consumption(con, vClth_IdNo, 0, Val(vTot_RecMtrs), "", tr))
                Else
                    ConsPavu = Val(Common_Procedures.get_Pavu_Consumption(con, vClth_IdNo, Lm_ID, Val(vTot_ChkMtrs), Trim(Wdth_Typ), tr))
                End If

            ElseIf Trim(UCase(cbo_LoomType.Text)) = "AUTO LOOM" Or Trim(UCase(cbo_LoomType.Text)) = "AUTOLOOM" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1059" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1490" Then '---- Lakshmi Saraswathi Textiles (Thiruchengodu) & LS EXPORTS
                ConsYarn = Val(Common_Procedures.get_Weft_ConsumedYarn(con, vClth_IdNo, Val(vTot_ChkMtrs), tr))
                ConsPavu = Val(Common_Procedures.get_Pavu_Consumption(con, vClth_IdNo, Lm_ID, Val(vTot_ChkMtrs), Trim(Wdth_Typ), tr))

            Else

                '********************* CODE BY GOPI 2025-01-04
                '*********** NEW

                '********* ACTUALLY CORRECT CONSUMPTION ***********

                ConsYarn = Val(Common_Procedures.get_Weft_ConsumedYarn(con, vClth_IdNo, Val(vTot_ChkMtrs), tr))

                If Trim(UCase(cbo_LoomType.Text)) = "POWER LOOM" Or Trim(UCase(cbo_LoomType.Text)) = "POWERLOOM" Then
                    ConsPavu = Val(Common_Procedures.get_Pavu_Consumption(con, vClth_IdNo, 0, Val(vTot_RecMtrs), "", tr))
                Else
                    ConsPavu = Val(Common_Procedures.get_Pavu_Consumption(con, vClth_IdNo, Lm_ID, Val(vTot_ChkMtrs), Trim(Wdth_Typ), tr))
                End If

                ' ********************* COMMAND BY GOPI 2025-01-04
                '*********** OLD

                'ConsYarn = Val(Common_Procedures.get_Weft_ConsumedYarn(con, vClth_IdNo, Val(vTot_RecMtrs), tr))
                'ConsPavu = Val(Common_Procedures.get_Pavu_Consumption(con, vClth_IdNo, Lm_ID, Val(vTot_RecMtrs), Trim(Wdth_Typ), tr))



            End If


            If Trim(UCase(lbl_RecPkCondition.Text)) = "CPREC-" Then

                LotCd = ""
                LotNo = txt_RecNo.Text

                Da = New SqlClient.SqlDataAdapter("select * from Cloth_Purchase_Receipt_Head Where Cloth_Purchase_Receipt_Code = '" & Trim(lbl_RecCode.Text) & "'", con)
                Da.SelectCommand.Transaction = tr
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    LotCd = Trim(Dt1.Rows(0).Item("Company_IdNo").ToString) & "-" & Trim(Dt1.Rows(0).Item("Cloth_Purchase_Receipt_No").ToString) & "/" & Trim(Common_Procedures.LotCode.Purchase_Cloth_Receipt) & "/" & Trim(Microsoft.VisualBasic.Right(Dt1.Rows(0).Item("Cloth_Purchase_Receipt_Code").ToString, 5))
                End If
                Dt1.Clear()

                'LotCd = lbl_RecCode.Text & "/P"
                'LotNo = txt_RecNo.Text & "/P"

                cmd.CommandText = "Update Cloth_Purchase_Receipt_Head set Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "', Weaver_Piece_Checking_Increment = Weaver_Piece_Checking_Increment + 1, Weaver_Piece_Checking_Date = @CheckingDate, Folding_Checking = " & Str(Val(txt_Folding.Text)) & ", Folding = " & Str(Val(txt_Folding.Text)) & ", ReceiptMeters_Checking = " & Str(Val(vTot_RecMtrs)) & ", Receipt_Meters = " & Str(Val(vTot_RecMtrs)) & " , Type1_Checking_Meters = " & Str(Val(vTot_Typ1Mtrs)) & ", Type2_Checking_Meters = " & Str(Val(vTot_Typ2Mtrs)) & ", Type3_Checking_Meters = " & Str(Val(vTot_Typ3Mtrs)) & ", Type4_Checking_Meters = " & Str(Val(vTot_Typ4Mtrs)) & ", Type5_Checking_Meters = " & Str(Val(vTot_Typ5Mtrs)) & ", Total_Checking_Meters = " & Str(Val(vTot_ChkMtrs)) & " Where Cloth_Purchase_Receipt_Code = '" & Trim(lbl_RecCode.Text) & "'"
                Nr = cmd.ExecuteNonQuery()

            ElseIf Trim(UCase(lbl_RecPkCondition.Text)) = "CLDRT-" Then
                LotCd = ""
                LotNo = txt_RecNo.Text

                Da = New SqlClient.SqlDataAdapter("Select * from ClothSales_Delivery_Return_Head Where ClothSales_Delivery_Return_Code = '" & Trim(lbl_RecCode.Text) & "'", con)
                Da.SelectCommand.Transaction = tr
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    LotCd = Trim(Dt1.Rows(0).Item("Company_IdNo").ToString) & "-" & Trim(Dt1.Rows(0).Item("ClothSales_Delivery_Return_No").ToString) & "/" & Trim(Common_Procedures.LotCode.Delivery_Return) & "/" & Trim(Microsoft.VisualBasic.Right(Dt1.Rows(0).Item("ClothSales_Delivery_Return_Code").ToString, 5))
                End If
                Dt1.Clear()

                'LotCd = lbl_RecCode.Text & "/D"
                'LotNo = txt_RecNo.Text & "/D"

                cmd.CommandText = "Update ClothSales_Delivery_Return_Head set Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "', Weaver_Piece_Checking_Increment = Weaver_Piece_Checking_Increment + 1, Weaver_Piece_Checking_Date = @CheckingDate, Folding_Return = " & Str(Val(txt_Folding.Text)) & ", Folding = " & Str(Val(txt_Folding.Text)) & ", ReturnMeters_Return = " & Str(Val(vTot_RecMtrs)) & ", Return_Meters = " & Str(Val(vTot_RecMtrs)) & " , Type1_Checking_Meters = " & Str(Val(vTot_Typ1Mtrs)) & ", Type2_Checking_Meters = " & Str(Val(vTot_Typ2Mtrs)) & ", Type3_Checking_Meters = " & Str(Val(vTot_Typ3Mtrs)) & ", Type4_Checking_Meters = " & Str(Val(vTot_Typ4Mtrs)) & ", Type5_Checking_Meters = " & Str(Val(vTot_Typ5Mtrs)) & ", Total_Checking_Meters = " & Str(Val(vTot_ChkMtrs)) & " Where ClothSales_Delivery_Return_Code = '" & Trim(lbl_RecCode.Text) & "'"
                Nr = cmd.ExecuteNonQuery()

            ElseIf Trim(UCase(lbl_RecPkCondition.Text)) = "CLSRT-" Or Trim(UCase(lbl_RecPkCondition.Text)) = "GCLSR-" Then

                LotCd = ""
                LotNo = txt_RecNo.Text

                Da = New SqlClient.SqlDataAdapter("Select * from ClothSales_Return_Head Where ClothSales_Return_Code = '" & Trim(lbl_RecCode.Text) & "'", con)
                Da.SelectCommand.Transaction = tr
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    If Trim(UCase(lbl_RecPkCondition.Text)) = "GCLSR-" Then
                        LotCd = Trim(Dt1.Rows(0).Item("Company_IdNo").ToString) & "-" & Trim(Dt1.Rows(0).Item("ClothSales_Return_No").ToString) & "/" & Trim(Common_Procedures.LotCode.Sales_Return_GST) & "/" & Trim(Microsoft.VisualBasic.Right(Dt1.Rows(0).Item("ClothSales_Return_Code").ToString, 5))
                    Else
                        LotCd = Trim(Dt1.Rows(0).Item("Company_IdNo").ToString) & "-" & Trim(Dt1.Rows(0).Item("ClothSales_Return_No").ToString) & "/" & Trim(Common_Procedures.LotCode.Sales_Return) & "/" & Trim(Microsoft.VisualBasic.Right(Dt1.Rows(0).Item("ClothSales_Return_Code").ToString, 5))
                    End If
                End If
                Dt1.Clear()

                'LotCd = lbl_RecCode.Text & "/S"
                'LotNo = txt_RecNo.Text & "/S"

                cmd.CommandText = "Update ClothSales_Return_Head set Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "', Weaver_Piece_Checking_Increment = Weaver_Piece_Checking_Increment + 1, Weaver_Piece_Checking_Date = @CheckingDate, Folding_Checking = " & Str(Val(txt_Folding.Text)) & ", Folding = " & Str(Val(txt_Folding.Text)) & ", ReceiptMeters_Checking = " & Str(Val(vTot_RecMtrs)) & ", Return_Meters = " & Str(Val(vTot_RecMtrs)) & " , Type1_Checking_Meters = " & Str(Val(vTot_Typ1Mtrs)) & ", Type2_Checking_Meters = " & Str(Val(vTot_Typ2Mtrs)) & ", Type3_Checking_Meters = " & Str(Val(vTot_Typ3Mtrs)) & ", Type4_Checking_Meters = " & Str(Val(vTot_Typ4Mtrs)) & ", Type5_Checking_Meters = " & Str(Val(vTot_Typ5Mtrs)) & ", Total_Checking_Meters = " & Str(Val(vTot_ChkMtrs)) & " Where ClothSales_Return_Code = '" & Trim(lbl_RecCode.Text) & "'"
                Nr = cmd.ExecuteNonQuery()


            ElseIf Trim(UCase(lbl_RecPkCondition.Text)) = Trim(UCase(PkCondition_PROFABRCPT)) Then
                LotCd = ""
                LotNo = txt_RecNo.Text

                Da = New SqlClient.SqlDataAdapter("Select * from Textile_Processing_Receipt_Head Where ClothProcess_Receipt_Code = '" & Trim(lbl_RecCode.Text) & "'", con)
                Da.SelectCommand.Transaction = tr
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    LotCd = Trim(Dt1.Rows(0).Item("Company_IdNo").ToString) & "-" & Trim(Dt1.Rows(0).Item("ClothProcess_Receipt_No").ToString) & "/" & Trim(Common_Procedures.LotCode.Processed_Fabric_Receipt) & "/" & Trim(Microsoft.VisualBasic.Right(Dt1.Rows(0).Item("ClothProcess_Receipt_Code").ToString, 5))
                End If
                Dt1.Clear()

                cmd.CommandText = "Update Textile_Processing_Receipt_Head set Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "', Folding_Checking = " & Str(Val(txt_Folding.Text)) & ", ReceiptMeters_Checking = " & Str(Val(vTot_RecMtrs)) & ", Type1_Checking_Meters = " & Str(Val(vTot_Typ1Mtrs)) & ", Type2_Checking_Meters = " & Str(Val(vTot_Typ2Mtrs)) & ", Type3_Checking_Meters = " & Str(Val(vTot_Typ3Mtrs)) & ", Type4_Checking_Meters = " & Str(Val(vTot_Typ4Mtrs)) & ", Type5_Checking_Meters = " & Str(Val(vTot_Typ5Mtrs)) & ", Total_Checking_Meters = " & Str(Val(vTot_ChkMtrs)) & " Where ClothProcess_Receipt_Code = '" & Trim(lbl_RecCode.Text) & "'"
                Nr = cmd.ExecuteNonQuery()


            Else

                LotCd = lbl_RecCode.Text
                If lbl_LotNo.Visible = True Then
                    LotNo = lbl_LotNo.Text
                Else
                    LotNo = txt_RecNo.Text
                End If

                cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "', Weaver_Piece_Checking_Increment = Weaver_Piece_Checking_Increment + 1, Weaver_Piece_Checking_Date = @CheckingDate, Folding_Checking = " & Str(Val(txt_Folding.Text)) & ", Folding = " & Str(Val(txt_Folding.Text)) & ", ReceiptMeters_Checking = " & Str(Val(vTot_RecMtrs)) & ", Receipt_Meters = " & Str(Val(vTot_RecMtrs)) & ", ConsumedYarn_Checking = " & Str(Val(ConsYarn)) & ", Consumed_Yarn = " & Str(Val(ConsYarn)) & ", ConsumedPavu_Checking = " & Str(Val(ConsPavu)) & ", Consumed_Pavu = " & Str(Val(ConsPavu)) & ", Type1_Checking_Meters = " & Str(Val(vTot_Typ1Mtrs)) & ", Type2_Checking_Meters = " & Str(Val(vTot_Typ2Mtrs)) & ", Type3_Checking_Meters = " & Str(Val(vTot_Typ3Mtrs)) & ", Type4_Checking_Meters = " & Str(Val(vTot_Typ4Mtrs)) & ", Type5_Checking_Meters = " & Str(Val(vTot_Typ5Mtrs)) & ", Total_Checking_Meters = " & Str(Val(vTot_ChkMtrs)) & " , Loom_Type = '" & Trim(cbo_LoomType.Text) & "' Where Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "'"
                Nr = cmd.ExecuteNonQuery()

                If txt_Rec_Meter.Enabled = True Then
                    cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set ReceiptMeters_Receipt = " & Str(Val(txt_Rec_Meter.Text)) & " Where Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "'"
                    Nr = cmd.ExecuteNonQuery()
                End If

            End If

            If Trim(LotCd) = "" Then
                Throw New ApplicationException("Invalid LotCode")
                Exit Sub
            End If

            Led_type = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(Wev_ID)) & ")", , tr)

            vStkOf_Pos_IdNo = 0
            If Lbl_StockOff.Visible = True Then
                vStkOf_Pos_IdNo = StkOff_ID

            Else
                If Trim(UCase(Led_type)) = "JOBWORKER" Then
                    vStkOf_Pos_IdNo = Wev_ID
                Else
                    vStkOf_Pos_IdNo = Val(Common_Procedures.CommonLedger.OwnSort_Ac)    '--- Val(Common_Procedures.CommonLedger.Godown_Ac)
                End If

            End If

            vOrdByRecNo = Val(Common_Procedures.OrderBy_CodeToValue(txt_RecNo.Text))

            With dgv_Details

                Sno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.PCSNO).Value) <> 0 Then

                        Sno = Sno + 1


                        vBrCode_Typ1 = ""
                        vBrCode_Typ2 = ""
                        vBrCode_Typ3 = ""
                        vBrCode_Typ4 = ""
                        vBrCode_Typ5 = ""

                        If Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.TYPE1METER).Value) <> 0 Then
                            vBrCode_Typ1 = Microsoft.VisualBasic.Left(Common_Procedures.FnYearCode, 2) & Trim(Val(lbl_Company.Tag)) & Trim(UCase(txt_RecNo.Text)) & Trim(UCase((.Rows(i).Cells(dgvCOL_PCSDETAILS.PCSNO).Value))) & "1"
                        End If
                        If Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.TYPE2METER).Value) <> 0 Then
                            vBrCode_Typ2 = Microsoft.VisualBasic.Left(Common_Procedures.FnYearCode, 2) & Trim(Val(lbl_Company.Tag)) & Trim(UCase(txt_RecNo.Text)) & Trim(UCase((.Rows(i).Cells(dgvCOL_PCSDETAILS.PCSNO).Value))) & "2"
                        End If
                        If Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.TYPE3METER).Value) <> 0 Then
                            vBrCode_Typ3 = Microsoft.VisualBasic.Left(Common_Procedures.FnYearCode, 2) & Trim(Val(lbl_Company.Tag)) & Trim(UCase(txt_RecNo.Text)) & Trim(UCase((.Rows(i).Cells(dgvCOL_PCSDETAILS.PCSNO).Value))) & "3"
                        End If
                        If Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.TYPE4METER).Value) <> 0 Then
                            vBrCode_Typ4 = Microsoft.VisualBasic.Left(Common_Procedures.FnYearCode, 2) & Trim(Val(lbl_Company.Tag)) & Trim(UCase(txt_RecNo.Text)) & Trim(UCase((.Rows(i).Cells(dgvCOL_PCSDETAILS.PCSNO).Value))) & "4"
                        End If
                        If Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.TYPE5METER).Value) <> 0 Then
                            vBrCode_Typ5 = Microsoft.VisualBasic.Left(Common_Procedures.FnYearCode, 2) & Trim(Val(lbl_Company.Tag)) & Trim(UCase(txt_RecNo.Text)) & Trim(UCase((.Rows(i).Cells(dgvCOL_PCSDETAILS.PCSNO).Value))) & "5"
                        End If

                        vChkr_ID = 0
                        If Trim(dgv_Details.Rows(i).Cells(dgvCOL_PCSDETAILS.CHECKERNAME).Value) <> "" Then
                            vChkr_ID = Common_Procedures.Employee_Simple_NameToIdNo(con, dgv_Details.Rows(i).Cells(dgvCOL_PCSDETAILS.CHECKERNAME).Value, tr)
                        End If

                        vFoldr_ID = 0
                        If Trim(dgv_Details.Rows(i).Cells(dgvCOL_PCSDETAILS.FOLDERNAME).Value) <> "" Then
                            vFoldr_ID = Common_Procedures.Employee_Simple_NameToIdNo(con, dgv_Details.Rows(i).Cells(dgvCOL_PCSDETAILS.FOLDERNAME).Value, tr)
                        End If

                        If Val(vCheck_id) <> 0 Then
                            vChkr_ID = vCheck_id
                        End If

                        If Val(vFolder_id) <> 0 Then
                            vFoldr_ID = vFolder_id
                        End If

                        vOrdByPieceNo = Val(Common_Procedures.OrderBy_CodeToValue(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.PCSNO).Value)))


                        Nr = 0
                        SQL1  = "Update Weaver_ClothReceipt_Piece_Details set  Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "',  Weaver_Piece_Checking_No = '" & Trim(lbl_ChkNo.Text) & "', Weaver_Piece_Checking_Date = '" & Trim(Format(dtp_Date.Value, "MM/dd/yyyy")) & "', for_orderby = " & Str(Val(vOrdByRecNo)) & ", Lot_Code = '" & Trim(LotCd) & "' , Lot_No = '" & Trim(LotNo) & "' , Ledger_Idno = " & Str(Val(Wev_ID)) & ", StockOff_IdNo = " & Str(Val(vStkOf_Pos_IdNo)) & ", Cloth_IdNo = " & Str(Val(vSTKClth_IdNo)) & ", Folding_Checking = " & Str(Val(txt_Folding.Text)) & ", Folding = " & Str(Val(txt_Folding.Text)) & ", Sl_No = " & Str(Val(Sno)) & ",  main_pieceno = '" & Trim(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.PCSNO).Value)) & "', PieceNo_OrderBy = " & Str(Val(vOrdByPieceNo)) & ", ReceiptMeters_Checking = " & Str(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.RECEIPTMETER).Value)) & ", Receipt_Meters = " & Str(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.RECEIPTMETER).Value)) & ", Loom_No = '" & Trim(.Rows(i).Cells(dgvCOL_PCSDETAILS.LOOMNO).Value) & "', Pick = " & Str(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.PICK).Value)) & ", Width = " & Str(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.WIDTH).Value)) & ", Type1_Meters = " & Str(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.TYPE1METER).Value)) & ", Type2_Meters = " & Str(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.TYPE2METER).Value)) & ", Type3_Meters = " & Str(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.TYPE3METER).Value)) & ", Type4_Meters  = " & Str(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.TYPE4METER).Value)) & ", Type5_Meters = " & Str(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.TYPE5METER).Value)) & ", Total_Checking_Meters = " & Str(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.TOTALMETER).Value)) & ", Weight = " & Str(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.WEIGHT).Value)) & ", Weight_Meter = " & Str(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.WEIGHTPERMETER).Value)) & ", Remarks = '" & Trim(.Rows(i).Cells(dgvCOL_PCSDETAILS.REMARKS).Value) & "', WareHouse_IdNo = " & Str(Val(vGod_ID)) & ", Checked_Pcs_Barcode_Type1 = '" & Trim(vBrCode_Typ1) & "', Checked_Pcs_Barcode_Type2 = '" & Trim(vBrCode_Typ2) & "', Checked_Pcs_Barcode_Type3 = '" & Trim(vBrCode_Typ3) & "', Checked_Pcs_Barcode_Type4 = '" & Trim(vBrCode_Typ4) & "', Checked_Pcs_Barcode_Type5 = '" & Trim(vBrCode_Typ5) & "', Checker_Idno = " & (Val(vChkr_ID)) & " , Folder_idno = " & (Val(vFoldr_ID)) & ", Checker_Wgs_per_Mtr = " & Val(vChkr_Wgs_per_Mtr) & ", Folder_Wgs_per_Mtr = " & Val(vFldr_Wgs_per_Mtr) & ", Total_CheckingMeters_100Folding = " & Str(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.TOTALMETERIN100FOLDING).Value)) & ", ExcessShort_Status_YesNo = '" & Trim(.Rows(i).Cells(dgvCOL_PCSDETAILS.EXCESSHORTSTATUSYESSORNO).Value) & "', Excess_Short_Meter = " & Str(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.EXCESSHORTMETER).Value)) & ", BeamNo_SetCode = '" & Trim(.Rows(i).Cells(dgvCOL_PCSDETAILS.BEAMNO_SETCODE_FORSELECTION).Value) & "'   Where Weaver_ClothReceipt_Code = '" & Trim(vCloRec_Code) & "' and Piece_No = '" & Trim(.Rows(i).Cells(dgvCOL_PCSDETAILS.PCSNO).Value) & "'"
                        cmd.CommandText = "EXEC SP_ExecuteQuery '" & Replace(Trim(SQL1), "'", "''") & "'"
                        Nr = cmd.ExecuteNonQuery()

                        If Nr = 0 Then

                            SQL1 = "Insert into Weaver_ClothReceipt_Piece_Details ( Weaver_Piece_Checking_Code ,             Company_IdNo         ,     Weaver_Piece_Checking_No   ,              Weaver_Piece_Checking_Date               ,   Weaver_ClothReceipt_Code  ,      Weaver_ClothReceipt_No     ,          for_orderby         ,                     Weaver_ClothReceipt_Date             ,        Lot_Code      ,       Lot_No         ,           Ledger_Idno   ,            StockOff_IdNo    ,                    Cloth_IdNo       ,            Folding_Checking       ,             Folding               ,           Sl_No      ,                         Piece_No                             ,                         main_pieceno                               ,          PieceNo_OrderBy        ,                       ReceiptMeters_Checking                           ,                       Receipt_Meters                                   ,                    Loom_No                                    ,                      Pick                                      ,                       Width                                     ,                      Type1_Meters                                   ,                      Type2_Meters                                   ,                      Type3_Meters                                    ,                      Type4_Meters                                   ,                      Type5_Meters                                   ,                      Total_Checking_Meters                          ,                      Weight                                     ,                      Weight_Meter                                        ,                    Remarks                                      ,        WareHouse_IdNo     ,   Checked_Pcs_Barcode_Type1 ,   Checked_Pcs_Barcode_Type2 ,   Checked_Pcs_Barcode_Type3 ,   Checked_Pcs_Barcode_Type4 ,   Checked_Pcs_Barcode_Type5  ,          CHecker_idno     ,          Folder_idno        ,        Checker_Wgs_per_Mtr    ,        Folder_Wgs_per_Mtr      ,                      Total_CheckingMeters_100Folding                            ,                    ExcessShort_Status_YesNo                                     ,                      Excess_Short_Meter                                  ,                    BeamNo_SetCode                                                   ) "
                            SQL1 = SQL1 & "     Values                            (    '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ",  '" & Trim(lbl_ChkNo.Text) & "',  '" & Trim(Format(dtp_Date.Value, "MM/dd/yyyy")) & "' , '" & Trim(vCloRec_Code) & "',   '" & Trim(txt_RecNo.Text) & "', " & Str(Val(vOrdByRecNo)) & ", '" & Trim(Format(dtp_Rec_Date.Value, "MM/dd/yyyy")) & "' , '" & Trim(LotCd) & "', '" & Trim(LotNo) & "', " & Str(Val(Wev_ID)) & ", " & Str(Val(vStkOf_Pos_IdNo)) & ", " & Str(Val(vSTKClth_IdNo)) & ", " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(dgvCOL_PCSDETAILS.PCSNO).Value) & "',  '" & Trim(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.PCSNO).Value)) & "',  " & Str(Val(vOrdByPieceNo)) & ",  " & Str(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.RECEIPTMETER).Value)) & ",  " & Str(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.RECEIPTMETER).Value)) & ", '" & Trim(.Rows(i).Cells(dgvCOL_PCSDETAILS.LOOMNO).Value) & "', " & Str(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.PICK).Value)) & " ,  " & Str(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.WIDTH).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.TYPE1METER).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.TYPE2METER).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.TYPE3METER).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.TYPE4METER).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.TYPE5METER).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.TOTALMETER).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.WEIGHT).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.WEIGHTPERMETER).Value)) & " , '" & Trim(.Rows(i).Cells(dgvCOL_PCSDETAILS.REMARKS).Value) & "' , " & Str(Val(vGod_ID)) & " , '" & Trim(vBrCode_Typ1) & "', '" & Trim(vBrCode_Typ2) & "', '" & Trim(vBrCode_Typ3) & "', '" & Trim(vBrCode_Typ4) & "', '" & Trim(vBrCode_Typ5) & "' ," & Str(Val(vChkr_ID)) & " , " & Str(Val(vFoldr_ID)) & " , " & Val(vChkr_Wgs_per_Mtr) & ", " & Val(vFldr_Wgs_per_Mtr) & " , " & Str(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.TOTALMETERIN100FOLDING).Value)) & ", '" & Trim(.Rows(i).Cells(dgvCOL_PCSDETAILS.EXCESSHORTSTATUSYESSORNO).Value) & "', " & Str(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.EXCESSHORTMETER).Value)) & ", '" & Trim(.Rows(i).Cells(dgvCOL_PCSDETAILS.BEAMNO_SETCODE_FORSELECTION).Value) & "' ) "
                            cmd.CommandText = "EXEC SP_ExecuteQuery '" & Replace(Trim(SQL1), "'", "''") & "'"
                            Nr = cmd.ExecuteNonQuery()

                        ElseIf Nr > 1 Then
                            Throw New ApplicationException("Invalid Piece Details Updation")
                            Exit Sub

                        End If

                        'If i = 0 Then

                        '    If Common_Procedures.settings.Multi_Godown_Status = 1 Then

                        '        If Len(Trim(txt_RecNo.Text)) >= 2 Then
                        '            If Microsoft.VisualBasic.Right(Trim(txt_RecNo.Text), 2) = "/P" Then
                        '                txt_RecNo.Text = Microsoft.VisualBasic.Left(Trim(txt_RecNo.Text), Len(Trim(txt_RecNo.Text)) - 2)
                        '            End If
                        '        End If

                        '        cmd.CommandText = "Update Textile_Processing_Delivery_Head set Total_Meters = " & Str(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.TYPE1METER).Value)) & " where ClothProcess_Delivery_No = '" & txt_RecNo.Text & "' " &
                        '                      " and ClothProcess_Delivery_Date = @DelDate"
                        '        cmd.ExecuteNonQuery()

                        '        cmd.CommandText = "Update Textile_Processing_Delivery_Details set Delivery_Meters = " & Str(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.TYPE1METER).Value)) & " where Cloth_Processing_Delivery_No = '" & txt_RecNo.Text & "' " &
                        '                      " and Cloth_Processing_Delivery_Date = @DelDate"
                        '        cmd.ExecuteNonQuery()

                        '    End If

                        'End If

                    End If

                Next

                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", " Weaver_ClothReceipt_Piece_Details", "Weaver_Piece_Checking_Code", Val(lbl_Company.Tag), NewCode, lbl_ChkNo.Text, Val(vOrdByNo), PkCondition_Entry, "", "", New_Entry, False, " Piece_No,ReceiptMeters_Checking,Receipt_Meters,Loom_No,Pick,Width,Type1_Meters,Type2_Meters,Type3_Meters,Type4_Meters,Type5_Meters,Total_Checking_Meters,Weight,Weight_Meter,Remarks,WareHouse_IdNo,Checked_Pcs_Barcode_Type1 ,Checked_Pcs_Barcode_Type2 ,   Checked_Pcs_Barcode_Type3 ,   Checked_Pcs_Barcode_Type4 ,   Checked_Pcs_Barcode_Type5  ,CHecker_idno,Folder_idno ,Checker_Wgs_per_Mtr,Folder_Wgs_per_Mtr", "Sl_No", "Weaver_Piece_Checking_Code, For_OrderBy, Company_IdNo,  Weaver_Piece_Checking_No,  Weaver_Piece_Checking_Date, Ledger_Idno", tr)

            End With


            If Trim(WagesCode) = "" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then

                Dim vDatFld_UPDTN_STS As Boolean = False
                Dim vDateFld_Nam_Val As String = ""

                vDatFld_UPDTN_STS = False
                vDateFld_Nam_Val = ""

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then

                    Dim DtTM1 As Date
                    Dim DtTM2 As Date

                    DtTM1 = #12/31/2020#  '----from 01-Jan-2021 STOCK POSTING will done only ON piece checking date
                    DtTM2 = Convert.ToDateTime(msk_date.Text)

                    If DateDiff(DateInterval.Day, DtTM1, DtTM2) > 0 Then
                        vDatFld_UPDTN_STS = True
                    End If

                End If

                If vDatFld_UPDTN_STS = True Then
                    vDateFld_Nam_Val = " reference_date = @CheckingDate, "
                End If

                EntID = Trim(lbl_RecPkCondition.Text) & Trim(lbl_RecCode.Text)
                Partcls = "CloRcpt : LotNo. " & Trim(lbl_LotNo.Text)
                If Trim(txt_PDcNo.Text) <> "" Then
                    Partcls = Trim(Partcls) & ",  P.Dc.No : " & Trim(txt_PDcNo.Text)
                End If

                vDelv_ID = 0 : vRec_ID = 0
                If Trim(UCase(Led_type)) = "JOBWORKER" Then
                    vDelv_ID = Wev_ID
                    vRec_ID = 0
                Else
                    vDelv_ID = 0
                    vRec_ID = Wev_ID
                End If


                Nr = 0
                cmd.CommandText = "Update Stock_Yarn_Processing_Details set " & vDateFld_Nam_Val & " Weight = " & Str(Val(ConsYarn)) & " Where Reference_Code = '" & Trim(lbl_RecPkCondition.Text) & Trim(lbl_RecCode.Text) & "'"
                Nr = cmd.ExecuteNonQuery()
                If Nr = 0 Then
                    cmd.CommandText = "Insert into Stock_Yarn_Processing_Details (                               Reference_Code                   ,                 Company_IdNo     ,               Reference_No    ,                               for_OrderBy                              , Reference_Date,         DeliveryTo_Idno   ,       ReceivedFrom_Idno  ,          Entry_ID    ,         Particulars    ,       Party_Bill_No  , Sl_No,           Count_IdNo       , Yarn_Type, Mill_IdNo, Bags, Cones,            Weight          ) " &
                                            "          Values                    ('" & Trim(lbl_RecPkCondition.Text) & Trim(lbl_RecCode.Text) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_LotNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_LotNo.Text))) & ",  @CheckingDate, " & Str(Val(vDelv_ID)) & ", " & Str(Val(vRec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(Partcls) & "', '" & Trim(PBlNo) & "',   1  , " & Str(Val(WftCnt_ID)) & ",    'MILL',    0     ,  0  ,    0 , " & Str(Val(ConsYarn)) & " ) "
                    cmd.ExecuteNonQuery()
                End If


                Nr = 0
                cmd.CommandText = "Update Stock_Pavu_Processing_Details set " & vDateFld_Nam_Val & " Meters = " & Str(Val(ConsPavu)) & " Where Reference_Code = '" & Trim(lbl_RecPkCondition.Text) & Trim(lbl_RecCode.Text) & "'"
                Nr = cmd.ExecuteNonQuery()
                If Nr = 0 Then
                    cmd.CommandText = "Insert into Stock_Pavu_Processing_Details (                 Reference_Code             ,                 Company_IdNo     ,             Reference_No      ,                               for_OrderBy                              , Reference_Date ,         DeliveryTo_Idno   ,      ReceivedFrom_Idno   ,          Cloth_Idno         ,           Entry_ID   ,     Party_Bill_No    ,         Particulars    ,            Sl_No     ,            EndsCount_IdNo  , Sized_Beam,               Meters       ) " &
                    "           Values                       ('" & Trim(lbl_RecPkCondition.Text) & Trim(lbl_RecCode.Text) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_LotNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_LotNo.Text))) & ",   @CheckingDate, " & Str(Val(vDelv_ID)) & ", " & Str(Val(vRec_ID)) & ", " & Str(Val(vClth_IdNo)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(EdsCnt_ID)) & ",     0     , " & Str(Val(ConsPavu)) & " ) "
                    cmd.ExecuteNonQuery()
                End If

            End If

            If Val(vTot_Typ1Mtrs) <> 0 Or Val(vTot_Typ2Mtrs) <> 0 Or Val(vTot_Typ3Mtrs) <> 0 Or Val(vTot_Typ4Mtrs) <> 0 Or Val(vTot_Typ5Mtrs) <> 0 Then
                cmd.CommandText = "Update Stock_Cloth_Processing_Details set reference_date = @CheckingDate,  Cloth_IdNo = " & Str(Val(vSTKClth_IdNo)) & ", Folding = " & Str(Val(txt_Folding.Text)) & ", UnChecked_Meters = 0, Meters_Type1 = " & Str(Val(vTot_Typ1Mtrs)) & ", Meters_Type2 = " & Str(Val(vTot_Typ2Mtrs)) & ", Meters_Type3 = " & Str(Val(vTot_Typ3Mtrs)) & ", Meters_Type4 = " & Str(Val(vTot_Typ4Mtrs)) & ", Meters_Type5 = " & Str(Val(vTot_Typ5Mtrs)) & " Where Reference_Code = '" & Trim(vCloRec_Code) & "'"
                cmd.ExecuteNonQuery()
            End If

            '----- Saving Cross Checking
            vERRMSG = ""
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then
                'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1155" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1087" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "--1267--" Then '----KRG TEXTILE MILLS (PALLADAM)
                Dim vFAB_LOTCODE As String
                vFAB_LOTCODE = "~" & LotCd & "~"
                If Common_Procedures.Cross_Checking_PieceChecking_PackingSlip_Meters(con, vFAB_LOTCODE, vERRMSG, tr) = False Then
                    Throw New ApplicationException(vERRMSG)
                    Exit Sub
                End If
            End If

            'cmd.CommandText = "Truncate Table " & Trim(Common_Procedures.EntryTempTable) & ""
            'cmd.ExecuteNonQuery()

            ''---Piece Checking
            'cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Int1, Meters1) select Lot_Code, Piece_No, 1, Type1_Meters from Weaver_ClothReceipt_Piece_Details Where Lot_Code = '" & Trim(LotCd) & "' and PackingSlip_Code_Type1 <> '' and Type1_Meters <> 0"
            'cmd.ExecuteNonQuery()
            'cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Int1, Meters1) select Lot_Code, Piece_No, 2, Type2_Meters from Weaver_ClothReceipt_Piece_Details Where Lot_Code = '" & Trim(LotCd) & "' and PackingSlip_Code_Type2 <> '' and Type2_Meters <> 0 "
            'cmd.ExecuteNonQuery()
            'cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Int1, Meters1) select Lot_Code, Piece_No, 3, Type3_Meters from Weaver_ClothReceipt_Piece_Details Where Lot_Code = '" & Trim(LotCd) & "' and PackingSlip_Code_Type3 <> '' and Type3_Meters <> 0 "
            'cmd.ExecuteNonQuery()
            'cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Int1, Meters1) select Lot_Code, Piece_No, 4, Type4_Meters from Weaver_ClothReceipt_Piece_Details Where Lot_Code = '" & Trim(LotCd) & "' and PackingSlip_Code_Type4 <> '' and Type4_Meters <> 0 "
            'cmd.ExecuteNonQuery()
            'cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Int1, Meters1) select Lot_Code, Piece_No, 5, Type5_Meters from Weaver_ClothReceipt_Piece_Details Where Lot_Code = '" & Trim(LotCd) & "' and PackingSlip_Code_Type5 <> '' and Type5_Meters <> 0 "
            'cmd.ExecuteNonQuery()

            ''---Packing Slip
            'cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Int1, Meters1) select Lot_Code, Pcs_No, ClothType_IdNo, -1*Meters from Packing_Slip_Details Where Lot_Code = '" & Trim(LotCd) & "'"
            'cmd.ExecuteNonQuery()
            ''---Piece Transfer
            'cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Int1, Meters1) select Lot_Code, Pcs_No, ClothType_IdNo, -1*Meters from Piece_Transfer_Details Where Lot_Code = '" & Trim(LotCd) & "'"
            'cmd.ExecuteNonQuery()
            ''---Jobwork Piece Delivery
            'cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Int1, Meters1) select Lot_Code, Pcs_No, ClothType_IdNo, -1*Meters from JobWork_Piece_Delivery_Details Where Lot_Code = '" & Trim(LotCd) & "'"
            'cmd.ExecuteNonQuery()
            ''---Cloth Sales Piece Delivery
            'cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Int1, Meters1) select Lot_Code, Piece_No, PieceType_IdNo, -1*Meters from ClothSales_Delivery_Piece_Details Where Lot_Code = '" & Trim(LotCd) & "'"
            'cmd.ExecuteNonQuery()
            ''---Piece Excess/Short
            'cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Int1, Meters1) select Lot_Code, Pcs_No, ClothType_IdNo, -1*Meters from Piece_Excess_Short_Details Where Lot_Code = '" & Trim(LotCd) & "'"
            'cmd.ExecuteNonQuery()

            'Da = New SqlClient.SqlDataAdapter("Select Name1, Name2, Int1, sum(Meters1) as ProdMtrs from " & Trim(Common_Procedures.EntryTempTable) & " Group by Name1, Name2, Int1 having sum(Meters1) <> 0", con)
            'Da.SelectCommand.Transaction = tr
            'Dt2 = New DataTable
            'Da.Fill(Dt2)
            'If Dt2.Rows.Count > 0 Then
            '    If IsDBNull(Dt2.Rows(0)(3).ToString) = False Then
            '        If Val(Dt2.Rows(0)(3).ToString) <> 0 Then
            '            Throw New ApplicationException("Invalid Piece Details : Mismatch of Bale && Piece Meters for Piece No : " & Trim(Dt2.Rows(0)(1).ToString))
            '            Exit Sub
            '        End If
            '    End If
            'End If
            'Dt2.Clear()


            If vWARP_WEFT_STOCK_UPDATION_STATUS = False Then

                If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Yarn_Stock) = 1 Or Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Pavu_Stock) = 1 Then

                    If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Yarn_Stock) = 1 Then

                        cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno      , Count_IdNo, Yarn_Type, Mill_IdNo ) " &
                                                  " Select                               'YARN'    , Reference_Code, Reference_Date, Company_IdNo, ReceivedFrom_Idno, Count_IdNo, Yarn_Type, Mill_IdNo from Stock_Yarn_Processing_Details where Reference_Code = '" & Trim(lbl_RecPkCondition.Text) & Trim(lbl_RecCode.Text) & "'"
                        cmd.ExecuteNonQuery()

                    End If


                    If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Pavu_Stock) = 1 Then

                        cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno      , EndsCount_IdNo ) " &
                                                  " Select                               'PAVU', Reference_Code, Reference_Date, Company_IdNo, ReceivedFrom_Idno, EndsCount_IdNo from Stock_Pavu_Processing_Details where Reference_Code = '" & Trim(lbl_RecPkCondition.Text) & Trim(lbl_RecCode.Text) & "'"
                        cmd.ExecuteNonQuery()

                    End If

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "--1155--" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1267" Then '----KRG TEXTILE MILLS (PALLADAM)
                        'If Common_Procedures.Office_System_Status = False Then  '---[THANGES]  - REMOVE IT AFTER FINISHING THE Correcting Old Entries
                        If Common_Procedures.Check_is_Negative_Stock_Status(con, tr) = True Then Exit Sub
                        'End If
                    End If

                End If

            End If

            tr.Commit()

            If SaveAll_STS <> True Then
                MessageBox.Show("Saved Successfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            End If

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_ChkNo.Text)
                End If

            Else
                move_record(lbl_ChkNo.Text)

            End If

        Catch ex As Exception
            tr.Rollback()

            Timer1.Enabled = False
            SaveAll_STS = False

            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da.Dispose()
            cmd.Dispose()
            tr.Dispose()

            If msk_date.Visible And msk_date.Enabled Then
                msk_date.Focus()
                msk_date.SelectionStart = 0

            ElseIf cbo_Weaver.Enabled And cbo_Weaver.Visible Then
                cbo_Weaver.Focus()

            ElseIf cbo_LoomType.Visible And cbo_LoomType.Enabled Then
                cbo_LoomType.Focus()

            End If
        End Try


    End Sub

    Private Sub Total_Calculation1()
        Dim TotRec As String
        Dim Totsnd As String
        Dim Totsec As String
        Dim Totbit As String
        Dim Totrej As String
        Dim Tototr As String
        Dim Tottlmr As String
        Dim Totwgt As String
        Dim Tottlmr_100fd As String
        Dim TotExcShtMtr As String
        Dim fldperc As String

        If FrmLdSTS = True Then Exit Sub
        If NoCalc_Status = True Then Exit Sub

        TotRec = 0 : Totsnd = 0 : Totsec = 0 : Totbit = 0 : Totrej = 0 : Tototr = 0 : Tottlmr = 0 : Totwgt = 0
        Tottlmr_100fd = 0 : TotExcShtMtr = 0

        With dgv_Details

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.RECEIPTMETER).Value) <> 0 Or Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.TOTALMETER).Value) <> 0 Then

                    TotRec = Format(Val(TotRec) + Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.RECEIPTMETER).Value), "##########0.00")
                    Totsnd = Format(Val(Totsnd) + Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.TYPE1METER).Value), "##########0.00")
                    Totsec = Format(Val(Totsec) + Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.TYPE2METER).Value), "##########0.00")
                    Totbit = Format(Val(Totbit) + Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.TYPE3METER).Value), "##########0.00")
                    Totrej = Format(Val(Totrej) + Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.TYPE4METER).Value), "##########0.00")
                    Tototr = Format(Val(Tototr) + Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.TYPE5METER).Value), "##########0.00")
                    Tottlmr = Format(Val(Tottlmr) + Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.TOTALMETER).Value), "##########0.00")
                    Totwgt = Format(Val(Totwgt) + Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.WEIGHT).Value), "##########0.000")

                    Tottlmr_100fd = Format(Val(Tottlmr_100fd) + Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.TOTALMETERIN100FOLDING).Value), "##########0.00")
                    TotExcShtMtr = Format(Val(TotExcShtMtr) + Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.EXCESSHORTMETER).Value), "##########0.00")

                End If

            Next i

        End With


        With dgv_Details_Total1
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(dgvCOL_PCSDETAILS.RECEIPTMETER).Value = Format(Val(TotRec), "########0.00")
            .Rows(0).Cells(dgvCOL_PCSDETAILS.TYPE1METER).Value = Format(Val(Totsnd), "########0.00")
            .Rows(0).Cells(dgvCOL_PCSDETAILS.TYPE2METER).Value = Format(Val(Totsec), "########0.00")
            .Rows(0).Cells(dgvCOL_PCSDETAILS.TYPE3METER).Value = Format(Val(Totbit), "########0.00")
            .Rows(0).Cells(dgvCOL_PCSDETAILS.TYPE4METER).Value = Format(Val(Totrej), "########0.00")
            .Rows(0).Cells(dgvCOL_PCSDETAILS.TYPE5METER).Value = Format(Val(Tototr), "########0.00")
            .Rows(0).Cells(dgvCOL_PCSDETAILS.TOTALMETER).Value = Format(Val(Tottlmr), "########0.00")
            .Rows(0).Cells(dgvCOL_PCSDETAILS.WEIGHT).Value = Format(Val(Totwgt), "########0.000")

            .Rows(0).Cells(dgvCOL_PCSDETAILS.EXCESSHORTMETER).Value = Format(Val(TotExcShtMtr), "#########0.00")

        End With

        fldperc = Format(Val(txt_Folding.Text), "##########0.00")
        If Val(fldperc) <= 0 Then fldperc = 100

        With dgv_Details_Total2
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(dgvCOL_PCSDETAILS.PCSNO).Value = "100%"
            .Rows(0).Cells(dgvCOL_PCSDETAILS.RECEIPTMETER).Value = "FOLDING"

            .Rows(0).Cells(dgvCOL_PCSDETAILS.TYPE1METER).Value = Format(Val(Totsnd) * Val(fldperc) / 100, "########0.00")
            .Rows(0).Cells(dgvCOL_PCSDETAILS.TYPE2METER).Value = Format(Val(Totsec) * Val(fldperc) / 100, "########0.00")
            .Rows(0).Cells(dgvCOL_PCSDETAILS.TYPE3METER).Value = Format(Val(Totbit) * Val(fldperc) / 100, "########0.00")
            .Rows(0).Cells(dgvCOL_PCSDETAILS.TYPE4METER).Value = Format(Val(Totrej) * Val(fldperc) / 100, "########0.00")
            .Rows(0).Cells(dgvCOL_PCSDETAILS.TYPE5METER).Value = Format(Val(Tototr) * Val(fldperc) / 100, "########0.00")
            .Rows(0).Cells(dgvCOL_PCSDETAILS.TOTALMETER).Value = Format(Val(Tottlmr) * Val(fldperc) / 100, "########0.00")

            .Rows(0).Cells(dgvCOL_PCSDETAILS.TOTALMETERIN100FOLDING).Value = Format(Val(Tottlmr_100fd), "#########0.00")


        End With

        Excess_Short_Meter_Calculation()

    End Sub

    Private Sub Excess_Short_Meter_Calculation()
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim vRCPT_FOLDPERC As String = 0
        Dim vCHK_fldperc As String = 0
        Dim vTOT_CHKMTRS As String = 0
        Dim vTOT_RCPTMTRS As String = 0


        If FrmLdSTS = True Then Exit Sub
        If NoCalc_Status = True Then Exit Sub

        vRCPT_FOLDPERC = 0



        Da = New SqlClient.SqlDataAdapter("select Folding_Receipt from Weaver_Cloth_Receipt_Head Where Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("Folding_Receipt").ToString) = False Then
                vRCPT_FOLDPERC = Dt1.Rows(0).Item("Folding_Receipt").ToString
            End If
        Else
            Da = New SqlClient.SqlDataAdapter("select Folding from Cloth_Purchase_Receipt_Head Where Cloth_Purchase_Receipt_Code = '" & Trim(lbl_RecCode.Text) & "'", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0).Item("Folding").ToString) = False Then
                    vRCPT_FOLDPERC = Dt1.Rows(0).Item("Folding").ToString
                End If
            End If
        End If
        Dt1.Clear()


        If Val(vRCPT_FOLDPERC) <= 0 Then vRCPT_FOLDPERC = 100

        vCHK_fldperc = Format(Val(txt_Folding.Text), "##########0.00")
        If Val(vCHK_fldperc) <= 0 Then vCHK_fldperc = 100


        vTOT_RCPTMTRS = 0
        If dgv_Details_Total1.Rows.Count > 0 Then
            If Val(dgv_Details_Total1.Rows(0).Cells(dgvCOL_PCSDETAILS.RECEIPTMETER).Value) <> 0 Then
                vTOT_RCPTMTRS = dgv_Details_Total1.Rows(0).Cells(dgvCOL_PCSDETAILS.RECEIPTMETER).Value
            Else
                vTOT_RCPTMTRS = txt_Rec_Meter.Text
            End If
        Else
            vTOT_RCPTMTRS = txt_Rec_Meter.Text
        End If

        vTOT_RCPTMTRS = Format(Val(vTOT_RCPTMTRS) * Val(vRCPT_FOLDPERC) / 100, "########0.00")

        txt_Excess_Short.Text = ""



        vTOT_CHKMTRS = 0
        If dgv_Details_Total2.Rows.Count > 0 Then
            vTOT_CHKMTRS = Val(dgv_Details_Total2.Rows(0).Cells(dgvCOL_PCSDETAILS.TOTALMETER).Value)
        End If

        txt_Excess_Short.Text = Format(Val(vTOT_CHKMTRS) - Val(vTOT_RCPTMTRS), "########0.00")

    End Sub


    Private Sub TotalMeter_Calculation()
        Dim fldmtr As Integer = 0
        Dim Tot_Pc_Mtrs As String = 0, Tot_Pc_Wt As String = 0
        Dim fldperc As String = 0
        Dim Wgt_Mtr As String = 0
        Dim k As Integer = 0

        On Error Resume Next

        If FrmLdSTS = True Then Exit Sub
        If NoCalc_Status = True Then Exit Sub


        With dgv_Details

            If .Visible Then

                If IsNothing(.CurrentCell) Then Exit Sub

                If .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.RECEIPTMETER Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE1METER Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE2METER Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE3METER Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE4METER Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE5METER Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.WEIGHT Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.EXCESSHORTSTATUSYESSORNO Then

                    .CurrentRow.Cells(dgvCOL_PCSDETAILS.TOTALMETER).Value = Format(Val(.CurrentRow.Cells(dgvCOL_PCSDETAILS.TYPE1METER).Value) + Val(.CurrentRow.Cells(dgvCOL_PCSDETAILS.TYPE2METER).Value) + Val(.CurrentRow.Cells(dgvCOL_PCSDETAILS.TYPE3METER).Value) + Val(.CurrentRow.Cells(dgvCOL_PCSDETAILS.TYPE4METER).Value) + Val(.CurrentRow.Cells(dgvCOL_PCSDETAILS.TYPE5METER).Value), "#########0.00")

                    fldperc = Format(Val(txt_Folding.Text), "##########0.00")
                    If Val(fldperc) <= 0 Then fldperc = 100

                    .CurrentRow.Cells(dgvCOL_PCSDETAILS.TOTALMETERIN100FOLDING).Value = Format(Val(.CurrentRow.Cells(dgvCOL_PCSDETAILS.TOTALMETER).Value) * Val(fldperc) / 100, "#########0.00")

                    If .Columns(dgvCOL_PCSDETAILS.EXCESSHORTSTATUSYESSORNO).Visible = True Then
                        If Trim(UCase(.CurrentRow.Cells(dgvCOL_PCSDETAILS.EXCESSHORTSTATUSYESSORNO).Value)) = "YES" Then
                            .CurrentRow.Cells(dgvCOL_PCSDETAILS.EXCESSHORTMETER).Value = Format(Val(.CurrentRow.Cells(dgvCOL_PCSDETAILS.TOTALMETER).Value) - 100, "#########0.00")
                        End If

                    Else
                        .CurrentRow.Cells(dgvCOL_PCSDETAILS.EXCESSHORTMETER).Value = Format(Val(.CurrentRow.Cells(dgvCOL_PCSDETAILS.TOTALMETERIN100FOLDING).Value) - Val(.CurrentRow.Cells(dgvCOL_PCSDETAILS.RECEIPTMETER).Value), "#########0.00")

                    End If

                    Tot_Pc_Mtrs = 0 : Tot_Pc_Wt = 0
                    For k = 0 To .Rows.Count - 1

                        If Val(.CurrentRow.Cells(dgvCOL_PCSDETAILS.PCSNO).Value) = Val(.Rows(k).Cells(dgvCOL_PCSDETAILS.PCSNO).Value) Then
                            Tot_Pc_Mtrs = Format(Val(Tot_Pc_Mtrs) + Val(.Rows(k).Cells(dgvCOL_PCSDETAILS.TYPE1METER).Value) + Val(.Rows(k).Cells(dgvCOL_PCSDETAILS.TYPE2METER).Value) + Val(.Rows(k).Cells(dgvCOL_PCSDETAILS.TYPE3METER).Value) + Val(.Rows(k).Cells(dgvCOL_PCSDETAILS.TYPE4METER).Value) + Val(.Rows(k).Cells(dgvCOL_PCSDETAILS.TYPE5METER).Value), "##########0.00")
                            Tot_Pc_Wt = Format(Val(Tot_Pc_Wt) + Val(.Rows(k).Cells(dgvCOL_PCSDETAILS.WEIGHT).Value), "##########0.000")
                        End If

                    Next


                    Wgt_Mtr = 0
                    If Val(Tot_Pc_Mtrs) <> 0 Then Wgt_Mtr = Format(Val(Tot_Pc_Wt) / (Val(Tot_Pc_Mtrs) * Val(fldperc) / 100), "#########0.000")

                    For k = 0 To .Rows.Count - 1
                        If Val(.CurrentRow.Cells(dgvCOL_PCSDETAILS.PCSNO).Value) = Val(.Rows(k).Cells(dgvCOL_PCSDETAILS.PCSNO).Value) Then
                            If Val(Wgt_Mtr) <> 0 Then
                                .Rows(k).Cells(dgvCOL_PCSDETAILS.WEIGHTPERMETER).Value = Format(Val(Wgt_Mtr), "#########0.000")
                            Else
                                .Rows(k).Cells(dgvCOL_PCSDETAILS.WEIGHTPERMETER).Value = ""
                            End If

                        End If
                    Next

                    Total_Calculation1()

                End If

            End If
        End With
    End Sub

    Private Sub cbo_Weaver_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Weaver.GotFocus
        Dim vCONDT As String = ""

        If Trim(UCase(vEntryType)) = "WEAVER" Then
            vCONDT = "((ledger_type = 'WEAVER' or Ledger_Type = 'GODOWN' Or Ledger_Type = 'JOBWORKER'  OR Show_In_All_Entry = 1) and Close_Status = 0 )"
        Else
            vCONDT = "( ((Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Ledger_Type = 'GODOWN' OR Show_In_All_Entry = 1) and Close_Status = 0 )"
        End If
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", vCONDT, "(Ledger_idno = 0)")

        cbo_Weaver.Tag = cbo_Weaver.Text
    End Sub

    Private Sub cbo_Weaver_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Weaver.KeyDown
        Dim vCONDT As String = ""


        If Trim(UCase(vEntryType)) = "WEAVER" Then
            vCONDT = "((ledger_type = 'WEAVER' or Ledger_Type = 'GODOWN' Or Ledger_Type = 'JOBWORKER'   OR Show_In_All_Entry = 1) and Close_Status = 0 )"
        Else
            vCONDT = "( ((Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Ledger_Type = 'GODOWN' OR Show_In_All_Entry = 1) and Close_Status = 0 )"
        End If


        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Weaver, msk_date, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", vCONDT, "(Ledger_idno = 0)")
        If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If cbo_LoomType.Visible And cbo_LoomType.Enabled Then
                cbo_LoomType.Focus()
            ElseIf txt_Folding.Visible And txt_Folding.Enabled Then
                txt_Folding.Focus()
            ElseIf dgv_Details.Rows.Count > 1 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            End If
        End If


    End Sub

    Private Sub cbo_Weaver_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Weaver.KeyPress
        Dim vCONDT As String = ""


        If Trim(UCase(vEntryType)) = "WEAVER" Then
            vCONDT = "((ledger_type = 'WEAVER' or Ledger_Type = 'GODOWN' Or Ledger_Type = 'JOBWORKER' OR Show_In_All_Entry = 1) and Close_Status = 0 )"
        Else
            vCONDT = "( ((Ledger_Type = '' and ( AccountsGroup_IdNo = 10 or AccountsGroup_IdNo = 14) ) or Ledger_Type = 'GODOWN' OR Show_In_All_Entry = 1) and Close_Status = 0 )"
        End If

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Weaver, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", vCONDT, "(Ledger_idno = 0)", False)


        If Asc(e.KeyChar) = 13 Then

            If MessageBox.Show("Do you want to select Cloth Receipt :", "FOR CLOTH RECEIPT SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)

            Else
                If cbo_LoomType.Visible And cbo_LoomType.Enabled Then
                    cbo_LoomType.Focus()
                ElseIf txt_Folding.Visible And txt_Folding.Enabled Then
                    txt_Folding.Focus()
                ElseIf dgv_Details.Rows.Count > 1 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
                End If

            End If

        End If

    End Sub

    Private Sub cbo_Weaver_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Weaver.KeyUp
        If e.Control = False And e.KeyValue = 17 Then

            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Weaver_Creation
            Common_Procedures.MDI_LedType = "WEAVER"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Weaver.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Quality_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Quality.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_Quality_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Quality.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Quality, msk_Rec_Date, txt_PDcNo, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_Quality_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Quality.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Quality, txt_PDcNo, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_Quality_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Quality.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Quality.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub cbo_clothto_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Cloth_TransferTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "(Close_Status = 0)", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_clothto_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Cloth_TransferTo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Cloth_TransferTo, txt_Folding, Nothing, "Cloth_Head", "Cloth_Name", "(Close_Status = 0)", "(Cloth_idno = 0)")

        If (e.KeyValue = 40 And cbo_Cloth_TransferTo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If btn_ChkDetails.Visible = True Then

                btn_ChkDetails_Click(sender, e)

            Else

                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCOL_PCSDETAILS.RECEIPTMETER)
                    dgv_Details.CurrentCell.Selected = True
                Else
                    msk_date.Focus()
                End If

            End If

        End If


    End Sub

    Private Sub cbo_clothto_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Cloth_TransferTo.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Cloth_TransferTo, Nothing, "Cloth_Head", "Cloth_Name", "(Close_Status = 0)", "(Cloth_idno = 0)")

        If Asc(e.KeyChar) = 13 Then

            If btn_ChkDetails.Visible = True Then

                btn_ChkDetails_Click(sender, e)

            Else

                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCOL_PCSDETAILS.RECEIPTMETER)
                    dgv_Details.CurrentCell.Selected = True
                Else
                    msk_date.Focus()
                End If
            End If

        End If


    End Sub

    Private Sub cbo_Cloth_TransferTo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Cloth_TransferTo.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Cloth_TransferTo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub dgv_Details_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim rect As Rectangle

        Try

            If FrmLdSTS = True Then Exit Sub

            With dgv_Details

                If .Visible = True Then

                    If IsNothing(.CurrentCell) Then Exit Sub

                    If .Rows.Count > 0 Then


                        If .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE1METER Then
                            If Trim(.Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE1).Value) <> "" Then
                                .Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.TYPE1METER).ReadOnly = True
                            Else
                                .Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.TYPE1METER).ReadOnly = False
                            End If
                        End If

                        If .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE2METER Then
                            If Trim(.Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE2).Value) <> "" Then
                                .Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.TYPE2METER).ReadOnly = True
                            Else
                                .Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.TYPE2METER).ReadOnly = False
                            End If
                        End If

                        If .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE3METER Then
                            If Trim(.Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE3).Value) <> "" Then
                                .Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.TYPE3METER).ReadOnly = True

                            Else

                                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then  '---- BRT TEXTILES(SOMANUR)

                                    dgv_Details.Rows(e.RowIndex).Cells(dgvCOL_PCSDETAILS.TYPE3METER).ReadOnly = True


                                    If IsDate(dtp_Date.Text) = True Then

                                        Dim vTYPE3_StOPedDate As Date = #6/23/2021#

                                        If DateDiff("d", vTYPE3_StOPedDate.Date, dtp_Date.Value.Date) < 0 Then

                                            dgv_Details.Rows(e.RowIndex).Cells(dgvCOL_PCSDETAILS.TYPE3METER).ReadOnly = False

                                        End If

                                    End If

                                Else
                                    .Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.TYPE3METER).ReadOnly = False

                                End If


                            End If

                        End If

                        If .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE4METER Then
                            If Trim(.Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE4).Value) <> "" Then
                                .Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.TYPE4METER).ReadOnly = True
                            Else
                                .Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.TYPE4METER).ReadOnly = False
                            End If
                        End If

                        If .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE5METER Then
                            If Trim(.Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE5).Value) <> "" Then
                                .Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.TYPE5METER).ReadOnly = True
                            Else
                                .Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.TYPE5METER).ReadOnly = False
                            End If
                        End If

                        If e.ColumnIndex = dgvCOL_PCSDETAILS.EXCESSHORTSTATUSYESSORNO Then

                            If cbo_Grid_RateFor.Visible = False Or Val(cbo_Grid_RateFor.Tag) <> e.RowIndex Then

                                rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                                cbo_Grid_RateFor.Left = .Left + rect.Left
                                cbo_Grid_RateFor.Top = .Top + rect.Top

                                cbo_Grid_RateFor.Width = rect.Width
                                cbo_Grid_RateFor.Height = rect.Height
                                cbo_Grid_RateFor.Text = .CurrentCell.Value

                                cbo_Grid_RateFor.Tag = Val(e.RowIndex)
                                cbo_Grid_RateFor.Visible = True

                                cbo_Grid_RateFor.BringToFront()
                                cbo_Grid_RateFor.Focus()

                            End If

                        Else
                            cbo_Grid_RateFor.Visible = False

                        End If


                        If e.ColumnIndex = dgvCOL_PCSDETAILS.BEAMNO_SETCODE_FORSELECTION Then

                            If cbo_Grid_CountName.Visible = False Or Val(cbo_Grid_CountName.Tag) <> e.RowIndex Then

                                cbo_Grid_CountName.Tag = -1
                                Da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head order by Count_Name", con)
                                Dt1 = New DataTable
                                Da.Fill(Dt1)
                                cbo_Grid_CountName.DataSource = Dt1
                                cbo_Grid_CountName.DisplayMember = "Count_Name"

                                rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                                cbo_Grid_CountName.Left = .Left + rect.Left
                                cbo_Grid_CountName.Top = .Top + rect.Top

                                cbo_Grid_CountName.Width = rect.Width
                                cbo_Grid_CountName.Height = rect.Height
                                cbo_Grid_CountName.Text = .CurrentCell.Value

                                cbo_Grid_CountName.Tag = Val(e.RowIndex)
                                cbo_Grid_CountName.Visible = True

                                cbo_Grid_CountName.BringToFront()
                                cbo_Grid_CountName.Focus()

                            End If

                        Else
                            cbo_Grid_CountName.Visible = False

                        End If

                    End If

                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL ENTER....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

        If e.ColumnIndex >= dgvCOL_PCSDETAILS.TYPE1METER Or e.ColumnIndex <= dgvCOL_PCSDETAILS.TYPE5METER Then
            Check_Meter_Range_Condition_for_ClothTYpes(e.RowIndex, e.ColumnIndex)
        End If
        dgv_Details_CellLeave(sender, e)

    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        Try

            If FrmLdSTS = True Then Exit Sub

            With dgv_Details

                If IsNothing(.CurrentCell) Then Exit Sub

                If .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.RECEIPTMETER Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.PICK Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.WIDTH Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE1METER Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE2METER Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE3METER Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE4METER Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE5METER Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TOTALMETER Then
                    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                    Else
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                    End If

                ElseIf .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.WEIGHT Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.WEIGHTPERMETER Then
                    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                    Else
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                    End If
                End If

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS CELL LEAVE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged

        On Error Resume Next

        If FrmLdSTS = True Then Exit Sub
        If NoCalc_Status = True Then Exit Sub

        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.RECEIPTMETER Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE1METER Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE2METER Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE3METER Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE4METER Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE5METER Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.WEIGHT Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.EXCESSHORTSTATUSYESSORNO Then

                    TotalMeter_Calculation()

                End If
            End If
        End With
    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub

    Private Sub dgtxt_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyDown
        With dgv_Details

            If e.KeyValue = Keys.Delete Then

                If .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE1METER Then
                    If Trim(.Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE1).Value) <> "" Then
                        e.Handled = True
                        e.SuppressKeyPress = True
                    End If
                End If

                If .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE2METER Then
                    If Trim(.Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE2).Value) <> "" Then
                        e.Handled = True
                        e.SuppressKeyPress = True
                    End If
                End If

                If .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE3METER Then
                    If Trim(.Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE3).Value) <> "" Then
                        e.Handled = True
                        e.SuppressKeyPress = True
                    End If
                End If
                If .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE4METER Then
                    If Trim(.Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE4).Value) <> "" Then
                        e.Handled = True
                        e.SuppressKeyPress = True
                    End If
                End If
                If .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE5METER Then
                    If Trim(.Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE5).Value) <> "" Then
                        e.Handled = True
                        e.SuppressKeyPress = True
                    End If
                End If

            End If

        End With

    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress

        On Error Resume Next

        With dgv_Details

            If .Visible Then

                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

                If .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.RECEIPTMETER Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.PICK Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.WIDTH Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE1METER Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE2METER Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE3METER Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE4METER Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE5METER Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TOTALMETER Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.WEIGHT Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.WEIGHTPERMETER Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                    If .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE1METER Then
                        If Trim(.Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE1).Value) <> "" Then
                            e.Handled = True
                        End If
                    End If

                    If .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE2METER Then
                        If Trim(.Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE2).Value) <> "" Then
                            e.Handled = True
                        End If
                    End If

                    If .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE3METER Then
                        If Trim(.Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE3).Value) <> "" Then
                            e.Handled = True
                        End If
                    End If
                    If .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE4METER Then
                        If Trim(.Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE4).Value) <> "" Then
                            e.Handled = True
                        End If
                    End If
                    If .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE5METER Then
                        If Trim(.Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE5).Value) <> "" Then
                            e.Handled = True
                        End If
                    End If

                End If

            End If

        End With

    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim i As Integer = 0
        Dim n As Integer = 0
        Dim nrw As Integer = 0
        Dim PNO As String = ""
        Dim S As String = ""

        If e.Control = True And (UCase(Chr(e.KeyCode)) = "A" Or UCase(Chr(e.KeyCode)) = "I" Or e.KeyCode = Keys.Insert) Then

            With dgv_Details

                n = .CurrentRow.Index

                PNO = Trim(UCase(.Rows(n).Cells(dgvCOL_PCSDETAILS.PCSNO).Value))


                S = Replace(Trim(PNO), Val(PNO), "")
                PNO = Val(PNO)

                If Trim(UCase(S)) <> "Z" Then
                    S = Trim(UCase(S))
                    If Trim(S) = "" Then S = "A" Else S = Trim(Chr(Asc(S) + 1))
                End If


                If n <> .Rows.Count - 1 Then
                    If Trim(UCase(PNO)) & Trim(UCase(S)) = Trim(UCase(.Rows(n + 1).Cells(dgvCOL_PCSDETAILS.PCSNO).Value)) Then
                        MessageBox.Show("Already Piece Inserted", "DES NOT INSERT NEW PIECE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If

                nrw = n + 1

                dgv_Details.Rows.Insert(nrw, 1)

                dgv_Details.Rows(nrw).Cells(dgvCOL_PCSDETAILS.PCSNO).Value = Trim(UCase(PNO)) & S

                dgv_Details.Rows(nrw).Cells(dgvCOL_PCSDETAILS.LOOMNO).Value = .Rows(n).Cells(dgvCOL_PCSDETAILS.LOOMNO).Value
                If Val(.Rows(n).Cells(dgvCOL_PCSDETAILS.PICK).Value) <> 0 Then
                    dgv_Details.Rows(nrw).Cells(dgvCOL_PCSDETAILS.PICK).Value = Val(.Rows(n).Cells(dgvCOL_PCSDETAILS.PICK).Value)
                End If
                If Val(.Rows(n).Cells(dgvCOL_PCSDETAILS.WIDTH).Value) <> 0 Then
                    dgv_Details.Rows(nrw).Cells(dgvCOL_PCSDETAILS.WIDTH).Value = Val(.Rows(n).Cells(dgvCOL_PCSDETAILS.WIDTH).Value)
                End If

            End With

        End If

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                n = .CurrentRow.Index

                If Trim(Val(.Rows(n).Cells(dgvCOL_PCSDETAILS.PCSNO).Value)) = Trim(.Rows(n).Cells(dgvCOL_PCSDETAILS.PCSNO).Value) Then
                    MessageBox.Show("cannot remove this piece", "DOES NOT REMOVE DETAILS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If


                If Trim(.Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE1).Value) = "" And Trim(.Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE2).Value) = "" And Trim(.Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE3).Value) = "" And Trim(.Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE4).Value) = "" And Trim(.Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE5).Value) = "" Then

                    If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                        For i = 0 To .ColumnCount - 1
                            .Rows(n).Cells(i).Value = ""
                        Next

                    Else
                        .Rows.RemoveAt(n)

                    End If

                    Total_Calculation1()

                End If

            End With

        End If
    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub


    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub txt_Folding_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Folding.KeyDown
        If e.KeyValue = 38 Then
            If cbo_LoomType.Visible And cbo_LoomType.Enabled Then
                cbo_LoomType.Focus()
            ElseIf cbo_Weaver.Visible And cbo_Weaver.Enabled Then
                cbo_Weaver.Focus()
            Else
                msk_date.Focus()
            End If
        End If

        If (e.KeyValue = 40) Then
            If Common_Procedures.settings.CustomerCode = "1267" Then
                If cbo_Cloth_TransferTo.Visible = True Then
                    cbo_Cloth_TransferTo.Focus()

                End If

            Else
                If btn_ChkDetails.Visible = True Then

                    btn_ChkDetails_Click(sender, e)
                Else

                    If dgv_Details.Rows.Count > 0 Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCOL_PCSDETAILS.RECEIPTMETER)
                        dgv_Details.CurrentCell.Selected = True
                    Else
                        msk_date.Focus()
                    End If
                End If
            End If


        End If


    End Sub
    Private Sub cbo_Filter_CountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_ClothName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")

    End Sub
    Private Sub cbo_Filter_ClothName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ClothName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ClothName, cbo_Filter_PartyName, txt_Filter_LotNo, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub

    Private Sub cbo_Filter_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ClothName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ClothName, txt_Filter_LotNo, "Cloth_Head", "Cloth_Name", "", "(Cloth_idno = 0)")
    End Sub


    Private Sub cbo_Verified_Sts_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Verified_Sts.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub
    Private Sub cbo_Verified_Sts_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Verified_Sts.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Verified_Sts, txt_Filter_LotNo, btn_Filter_Show, "", "", "", "")
    End Sub

    Private Sub cbo_Verified_Sts_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Verified_Sts.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Verified_Sts, btn_Filter_Show, "", "", "", "")
    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")

    End Sub
    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_ClothName, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_ClothName, "Ledger_AlaisHead", "Ledger_DisplayName", "", "(Ledger_idno = 0)")
    End Sub
    Private Sub btn_Filter_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub
    Private Sub dgv_Filter_Details_CellDoubleClick1(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown1(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter_Details.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer
        Dim Led_IdNo As Integer, Clth_IdNo As Integer
        Dim Condt As String = ""
        Dim Verfied_Sts As Integer = 0

        Try

            con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            con.Open()

            Condt = ""
            Led_IdNo = 0
            Clth_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Weaver_Piece_Checking_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Weaver_Piece_Checking_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Weaver_Piece_Checking_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
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
            If Trim(txt_Filter_LotNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Lot_No = '" & Trim(txt_Filter_LotNo.Text) & "'"
            End If

            If Trim(txt_filter_Chkno.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Weaver_Piece_Checking_No = '" & Trim(txt_filter_Chkno.Text) & "'"
            End If

            If Trim(cbo_Verified_Sts.Text) = "YES" Then
                Verfied_Sts = 1
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Weaver_Piece_Checking_Code IN ( select z2.Weaver_Piece_Checking_Code from Weaver_Piece_Checking_Head z2 where z2.Verified_Status = " & Str(Val(Verfied_Sts)) & " )"
            ElseIf Trim(cbo_Verified_Sts.Text) = "NO" Then
                Verfied_Sts = 0
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Weaver_Piece_Checking_Code IN ( select z2.Weaver_Piece_Checking_Code from Weaver_Piece_Checking_Head z2 where z2.Verified_Status = " & Str(Val(Verfied_Sts)) & " )"
            End If

            'left outer join Weaver_ClothReceipt_Piece_Details b on a.Weaver_Piece_Checking_Code = b.Weaver_Piece_Checking_Code

            da = New SqlClient.SqlDataAdapter("select a.*, c.Cloth_Name, e.Ledger_Name from Weaver_Piece_Checking_Head a  left outer join Cloth_head c on a.Cloth_idno = c.Cloth_idno left outer join Ledger_head e on a.Ledger_idno = e.Ledger_idno where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.Weaver_Piece_Checking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " and " & Other_Condition & " Order by Weaver_Piece_Checking_Date, for_orderby, Weaver_Piece_Checking_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(dgvCOL_FILTERDETAILS.SNO).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(dgvCOL_FILTERDETAILS.CHECKINGNO).Value = dt2.Rows(i).Item("Weaver_Piece_Checking_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(dgvCOL_FILTERDETAILS.CHECKINGDATE).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Weaver_Piece_Checking_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(dgvCOL_FILTERDETAILS.PARTYNAME).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then '---- Kalaimagal Textiles (Palladam)
                        dgv_Filter_Details.Rows(n).Cells(dgvCOL_FILTERDETAILS.RECEIPTNO).Value = dt2.Rows(i).Item("Piece_Receipt_No").ToString & " / " & dt2.Rows(i).Item("Lot_No").ToString
                    Else
                        dgv_Filter_Details.Rows(n).Cells(dgvCOL_FILTERDETAILS.RECEIPTNO).Value = dt2.Rows(i).Item("Piece_Receipt_No").ToString
                    End If
                    dgv_Filter_Details.Rows(n).Cells(dgvCOL_FILTERDETAILS.CLOTHNAME).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(dgvCOL_FILTERDETAILS.RECEIPTDATE).Value = dt2.Rows(i).Item("Piece_Receipt_Date").ToString
                    dgv_Filter_Details.Rows(n).Cells(dgvCOL_FILTERDETAILS.RECEIPTMETER).Value = Format(Val(dt2.Rows(i).Item("ReceiptMeters_Receipt").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(dgvCOL_FILTERDETAILS.TOTALMETER).Value = Format(Val(dt2.Rows(i).Item("Total_Checking_Meters").ToString), "########0.00")

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

    Private Sub Open_FilterEntry()
        Dim movno As String

        On Error Resume Next

        If IsNothing(dgv_Filter_Details.CurrentCell) Then Exit Sub

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(dgvCOL_FILTERDETAILS.CHECKINGNO).Value)

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
    Private Sub txt_Folding_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Folding.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            If Common_Procedures.settings.CustomerCode = "1267" Then
                If cbo_Cloth_TransferTo.Visible = True Then
                    cbo_Cloth_TransferTo.Focus()

                End If
            Else
                If btn_ChkDetails.Visible = True Then
                    btn_ChkDetails_Click(sender, e)
                Else

                    If dgv_Details.Rows.Count > 0 Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCOL_PCSDETAILS.RECEIPTMETER)
                        dgv_Details.CurrentCell.Selected = True
                    Else
                        msk_date.Focus()
                    End If
                End If

            End If
        End If
    End Sub
    Private Sub txt_Rec_Meter_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rec_Meter.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        Excess_Short_Meter_Calculation()
    End Sub

    Private Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = False
        EMAIL_Status = False
        WHATSAPP_Status = False
        print_record()
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        vPrntOnly_PageNo = 0
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Weaver_Piece_Checking_Entry, New_Entry) = False Then Exit Sub

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then
            pnl_Print.Visible = True
            pnl_Back.Enabled = False

            opt_SinglePage.Checked = True
            txt_Print_PageNo.Enabled = True
            txt_Print_PageNo.Text = ""
            If txt_Print_PageNo.Enabled And txt_Print_PageNo.Visible Then
                txt_Print_PageNo.Focus()
            Else
                opt_SinglePage.Focus()
            End If

        Else
            Printing_CheckingReport()

        End If

    End Sub

    Private Sub Printing_CheckingReport()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim ps As Printing.PaperSize
        Dim I As Integer
        Dim Def_PrntrNm As String = ""
        Dim vFILNm As String = ""
        Dim vFLPATH As String = ""
        Dim vPDFFLPATH_and_NAME As String = ""
        Dim vPRNTRNAME As String
        Dim vLOTNO As String
        Dim vWEANM As String


        NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            vWEANM = ""
            vLOTNO = ""

            da1 = New SqlClient.SqlDataAdapter("select * from Weaver_Piece_Checking_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)
            If dt1.Rows.Count <= 0 Then

                If EMAIL_Status = False And WHATSAPP_Status = False Then
                    MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                End If
                Exit Sub

            Else
                vWEANM = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                vLOTNO = dt1.Rows(0).Item("Piece_Receipt_No").ToString

            End If

            dt1.Dispose()
            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

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


                    vFLPATH = ""
                    vPRNTRNAME = Common_Procedures.get_PDF_PrinterName(EMAIL_Status, WHATSAPP_Status, vFLPATH)

                    If Trim(vPRNTRNAME) = "" Then
                        Exit Sub
                    End If

                    Def_PrntrNm = PrintDocument1.PrinterSettings.PrinterName

                    vWEANM = Trim(Replace(vWEANM, "   ", "_"))
                    vWEANM = Trim(Replace(vWEANM, "  ", "_"))
                    vWEANM = Trim(Replace(vWEANM, " ", "_"))
                    vWEANM = Trim(Replace(vWEANM, ".", "_"))
                    vWEANM = Trim(Replace(vWEANM, "/", "_"))
                    vWEANM = Trim(Replace(vWEANM, "\", "_"))
                    vWEANM = Trim(Replace(vWEANM, "&", "_"))
                    vWEANM = Trim(Replace(vWEANM, """", ""))


                    vLOTNO = Trim(Replace(vLOTNO, "   ", "_"))
                    vLOTNO = Trim(Replace(vLOTNO, "  ", "_"))
                    vLOTNO = Trim(Replace(vLOTNO, " ", "_"))
                    vLOTNO = Trim(Replace(vLOTNO, ".", "_"))
                    vLOTNO = Trim(Replace(vLOTNO, "/", "_"))
                    vLOTNO = Trim(Replace(vLOTNO, "\", "_"))
                    vLOTNO = Trim(Replace(vLOTNO, "&", "_"))
                    vLOTNO = Trim(Replace(vLOTNO, """", ""))

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT SPINNERS PRIVATE LIMITED (FABRIC DIVISION)
                        vFILNm = Trim("IR_LotNo_" & Trim(vLOTNO) & "_" & Trim(vWEANM) & ".pdf")
                    Else
                        vFILNm = Trim("FoldingReport_LotNo_" & Trim(vLOTNO) & ".pdf")
                    End If

                    vFILNm = StrConv(vFILNm, vbProperCase)
                    vPDFFLPATH_and_NAME = Trim(vFLPATH) & "\" & Trim(vFILNm)
                    vEMAIL_Attachment_FileName = Trim(vPDFFLPATH_and_NAME)

                    PrintDocument1.DocumentName = Trim(vFILNm)
                    PrintDocument1.PrinterSettings.PrinterName = Trim(vPRNTRNAME)    ' "Microsoft Print to PDF"
                    'PrintDocument1.PrinterSettings.PrinterName = "doPDF v7"
                    PrintDocument1.PrinterSettings.PrintToFile = True
                    PrintDocument1.PrinterSettings.PrintFileName = Trim(vPDFFLPATH_and_NAME)
                    PrintDocument1.Print()

                    'Debug.Print(PrintDocument1.PrinterSettings.PrintFileName)

                    PrintDocument1.PrinterSettings.PrinterName = Trim(Def_PrntrNm)
                    Print_PDF_Status = False

                    ''--This is actual & correct
                    'PrintDocument1.DocumentName = "CheckingReport"
                    'PrintDocument1.PrinterSettings.PrinterName = "doPDF v7"
                    'PrintDocument1.PrinterSettings.PrintFileName = "c:\CheckingReport.pdf"
                    'PrintDocument1.Print()

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
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try


        Else
            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument1

                ppd.WindowState = FormWindowState.Maximized
                ppd.StartPosition = FormStartPosition.CenterScreen
                'ppd.ClientSize = New Size(600, 600)
                ppd.PrintPreviewControl.AutoZoom = True
                ppd.PrintPreviewControl.Zoom = 1.0

                ppd.ShowDialog()

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If

        Print_PDF_Status = False

    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim cmd As New SqlClient.SqlCommand
        Dim SQL1 As String
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String

        is_FOOTR_PRINT_PART1_STS = False
        is_FOOTR_PRINT_FULLY_STS = False

        NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0

        Try

            con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
            con.Open()

            cmd.Connection = con

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* ,c.* , d.Cloth_Name from Weaver_Piece_Checking_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Cloth_Head d ON d.Cloth_IdNo = a.Cloth_Idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)


            If prn_HdDt.Rows.Count > 0 Then

                SQL1 = "select a.* from Weaver_ClothReceipt_Piece_Details a where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' ORDER BY PieceNo_OrderBy ASC"
                cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
                da2 = New SqlClient.SqlDataAdapter(cmd)
                'da2 = New SqlClient.SqlDataAdapter("select a.* from Weaver_ClothReceipt_Piece_Details a where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' ORDER BY PieceNo_OrderBy ASC", con)
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

    Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1152" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1370" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1569" Then '----J.P.R Textile (PALLADAM)
            Printing_Format2(e)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then
            Printing_Format3(e)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1395" Then '---- SANTHA EXPORTS (SOMANUR)
            Printing_Format4(e)
        Else
            Printing_Format1(e)
        End If

    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        Dim p1Font As Font
        Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim I As Integer
        Dim NoofItems_PerPage As Integer, NoofDets, sno As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim C1 As Single, C2 As Single

        p1Font = New Font("Calibri", 10, FontStyle.Bold)

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
            .Bottom = 30
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



        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = Val(30) : ClAr(2) = 60 : ClAr(3) = 75 : ClAr(4) = 75 : ClAr(5) = 75 : ClAr(6) = 75 : ClAr(7) = 80 : ClAr(8) = 80 : ClAr(9) = 70 : ClAr(10) = 70
        ClAr(11) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10))


        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7)
        C2 = C1 + ClAr(8)

        TxtHgt = 18

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        NoofItems_PerPage = 15

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            e.HasMorePages = True

                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1


                        'NoofDets = NoofDets + 1

                        sno = sno + 1
                        vType1 = Val(prn_DetDt.Rows(prn_DetIndx).Item("Type1_Meters").ToString())
                        vType2 = Val(prn_DetDt.Rows(prn_DetIndx).Item("Type2_Meters").ToString())
                        vType3 = Val(prn_DetDt.Rows(prn_DetIndx).Item("Type3_Meters").ToString())
                        vType4 = Val(prn_DetDt.Rows(prn_DetIndx).Item("Type4_Meters").ToString())
                        vType5 = Val(prn_DetDt.Rows(prn_DetIndx).Item("Type5_Meters").ToString())


                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetSNo), LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Piece_No").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Receipt_Meters").ToString), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(vType1), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(vType2), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(vType3), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(vType4), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(vType5), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Total_Checking_Meters").ToString), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), "#######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight_Meter").ToString), "#######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, pFont)

                        CurY = CurY + TxtHgt + 10
                        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

                        NoofDets = NoofDets + 1

                        prn_DetIndx = prn_DetIndx + 1

                    Loop
                End If

                Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim p1Font As Font
        Dim Cmp_Name As String
        Dim strHeight As Single
        Dim C1, w1, w2 As Single

        PageNo = PageNo + 1

        CurY = TMargin


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = ""

        Type1 = "" : Type2 = "" : Type3 = "" : Type4 = "" : Type5 = ""
        Type11 = "" : Type22 = "" : Type33 = "" : Type44 = "" : Type55 = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString



        Type1 = Trim(UCase(Common_Procedures.ClothType.Type1)) : Type2 = Trim(UCase(Common_Procedures.ClothType.Type2)) : Type3 = Trim(UCase(Common_Procedures.ClothType.Type3)) : Type4 = Trim(UCase(Common_Procedures.ClothType.Type4)) : Type5 = Trim(UCase(Common_Procedures.ClothType.Type5))
        'Type1 = "SOUND" : Type2 = "SECONDS" : Type3 = "BITS" : Type4 = "REJECT" : Type5 = "OTHERS"
        Type11 = "MTRS" : Type22 = "MTRS" : Type33 = "MTRS" : Type44 = "MTRS" : Type55 = "MTRS"


        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + TxtHgt + 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "FOLDING REPORT", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 20

        w1 = e.Graphics.MeasureString("CHECKING NO.OF PCS  : ", pFont).Width
        w2 = e.Graphics.MeasureString("RECEIVED MTRS ", pFont).Width


        CurY = CurY + TxtHgt - 10
        pFont = New Font("Calibri", 10, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, "NAME   :  " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "QUALITY", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cloth_Name").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "CHECKING DATE", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + w1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Weaver_Piece_Checking_Date").ToString), "dd-MM-yyyy").ToString, LMargin + w1 + 30, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "RECEIVED NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Piece_Receipt_No").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "DATE :  " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Piece_Receipt_Date").ToString), "dd-MM-yyyy").ToString, PageWidth - 10, CurY, 1, 0, pFont)



        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "CHECKING NO.OF PCS", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + w1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("noof_pcs").ToString), "#######0"), LMargin + w1 + 30, CurY, 0, 0, pFont)

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1273" Then
            Common_Procedures.Print_To_PrintDocument(e, "FOLDING :  " & Format(Val(prn_HdDt.Rows(0).Item("Folding").ToString), "#######0"), LMargin + C1 - 20, CurY, 1, 0, pFont)
        End If
        Common_Procedures.Print_To_PrintDocument(e, "RECEIVED MTRS", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("ReceiptMeters_Receipt").ToString), "#######0.00"), LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "PARTY DC.NO :  " & prn_HdDt.Rows(0).Item("Party_DcNo").ToString, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        CurY = CurY + 10

        Common_Procedures.Print_To_PrintDocument(e, "SL.", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PIECE", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "RECD", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Type1), LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Type2), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Type3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Type4), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Type5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WGT/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "NO.", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Type11), LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Type22), LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Type33), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Type44), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Type55), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTR", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY




    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Bilno As String = "", BilDt As String = ""
        Dim reccode As String
        Dim w1 As Single = 0


        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        reccode = Trim(Val(lbl_Company.Tag)) & "-" & prn_HdDt.Rows(0).Item("Piece_Receipt_No").ToString & "/" & Trim(Common_Procedures.FnYearCode)

        da3 = New SqlClient.SqlDataAdapter("select b.weaver_wages_no, b.weaver_wages_date from weaver_cloth_receipt_head a INNER JOIN weaver_wages_head b ON A.WEAVER_WAGES_CODE = B.WEAVER_WAGes_code where a.weaver_clothreceipt_code = '" & Trim(reccode) & "'", con)
        Dt1 = New DataTable
        da3.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            BilDt = Format(Convert.ToDateTime(Dt1.Rows(0).Item("Weaver_Wages_Date").ToString), "dd-MM-yyyy").ToString
            Bilno = Dt1.Rows(0).Item("Weaver_Wages_No").ToString
        End If


        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), LnAr(3))

        CurY = CurY + 10

        Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2), CurY, 0, 0, pFont)


        vTotType1 = Val(prn_HdDt.Rows(0).Item("Total_Type1_Meters").ToString)
        vTotType2 = Val(prn_HdDt.Rows(0).Item("Total_Type2_Meters").ToString)
        vTotType3 = Val(prn_HdDt.Rows(0).Item("Total_Type3_Meters").ToString)
        vTotType4 = Val(prn_HdDt.Rows(0).Item("Total_Type4_Meters").ToString)
        vTotType5 = Val(prn_HdDt.Rows(0).Item("Total_Type5_Meters").ToString)

        Common_Procedures.Print_To_PrintDocument(e, Format(Val(vTotType1), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(vTotType2), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(vTotType3), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(vTotType4), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(vTotType5), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY)
        LnAr(5) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))

        CurY = CurY + 10

        w1 = e.Graphics.MeasureString("RECEIVED ", pFont).Width
        Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + w1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Checking_Meters").ToString), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "BILL NO   :   " & Bilno, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt + 10
        Common_Procedures.Print_To_PrintDocument(e, "RECEIVED ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + w1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("ReceiptMeters_Receipt").ToString), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "DATE   :    " & BilDt, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY)


        CurY = CurY + 10


        Common_Procedures.Print_To_PrintDocument(e, "EXCESS METERS ", LMargin + 10, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Excess_Short_Meter").ToString), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(5))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5))


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub


    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, K As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim vLOTAPP_Condt As String = ""
        Dim vSQLCONDT As String = ""


        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Weaver.Text)

        If LedIdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT PAVU...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Weaver.Enabled And cbo_Weaver.Visible Then cbo_Weaver.Focus()
            Exit Sub
        End If

        NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        With dgv_Selection

            .Rows.Clear()
            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.*, c.Cloth_Name, d.EndsCount_Name from Weaver_Cloth_Receipt_Head a INNER JOIN  Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo where a.Receipt_Type <> 'L' and a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.ledger_Idno = " & Str(Val(LedIdNo)) & "  and a.Return_Status = 0 order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(dgvCOL_SELECTION.SNO).Value = Val(SNo)

                    .Rows(n).Cells(dgvCOL_SELECTION.RECEIPTNO).Value = Dt1.Rows(i).Item("Weaver_ClothReceipt_No").ToString
                    .Rows(n).Cells(dgvCOL_SELECTION.RECEIPTDATE).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Weaver_ClothReceipt_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(dgvCOL_SELECTION.PARTYDCNO).Value = Dt1.Rows(i).Item("Party_DcNo").ToString
                    .Rows(n).Cells(dgvCOL_SELECTION.CLOTHNAME).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                    .Rows(n).Cells(dgvCOL_SELECTION.ENDSCOUNT).Value = Dt1.Rows(i).Item("EndsCount_Name").ToString
                    .Rows(n).Cells(dgvCOL_SELECTION.PCS).Value = Val(Dt1.Rows(i).Item("noof_pcs").ToString)


                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then  '---- BRT TEXTILES(SOMANUR)
                        If Val(Dt1.Rows(i).Item("Dc_Receipt_Meters").ToString) > 0 Then
                            .Rows(n).Cells(dgvCOL_SELECTION.METERS).Value = Format(Val(Dt1.Rows(i).Item("Dc_Receipt_Meters").ToString), "#########0.00")
                        Else
                            .Rows(n).Cells(dgvCOL_SELECTION.METERS).Value = Format(Val(Dt1.Rows(i).Item("Receipt_Meters").ToString), "#########0.00")
                        End If

                    Else
                        .Rows(n).Cells(dgvCOL_SELECTION.METERS).Value = Format(Val(Dt1.Rows(i).Item("Receipt_Meters").ToString), "#########0.00")

                    End If

                    .Rows(n).Cells(dgvCOL_SELECTION.STS).Value = "1"
                    .Rows(n).Cells(dgvCOL_SELECTION.WEAVERCLOTHRECEIPTCODE).Value = Dt1.Rows(i).Item("Weaver_ClothReceipt_Code").ToString
                    .Rows(n).Cells(dgvCOL_SELECTION.FOLDING).Value = Val(Dt1.Rows(i).Item("folding").ToString)
                    .Rows(n).Cells(dgvCOL_SELECTION.RECEIPTPKCONDITION).Value = "WCLRC-"
                    .Rows(n).Cells(dgvCOL_SELECTION.PIECEFROMNUMBER).Value = Val(Dt1.Rows(i).Item("pcs_fromno").ToString)
                    .Rows(n).Cells(dgvCOL_SELECTION.PIECETONUMBER).Value = Val(Dt1.Rows(i).Item("pcs_tono").ToString)

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()

            vLOTAPP_Condt = ""
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT TEXTTILES (SOMANUR)
                vLOTAPP_Condt = " and a.Lot_Approved_Status = 1 "
            End If

            Da = New SqlClient.SqlDataAdapter("select a.*, c.*, d.EndsCount_Name from Weaver_Cloth_Receipt_Head a INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo where a.Receipt_Type <> 'L' and a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Piece_Checking_Code = '' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " and a.Return_Status = 0 " & vLOTAPP_Condt & " order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(dgvCOL_SELECTION.SNO).Value = Val(SNo)

                    .Rows(n).Cells(dgvCOL_SELECTION.RECEIPTNO).Value = Dt1.Rows(i).Item("Weaver_ClothReceipt_No").ToString
                    .Rows(n).Cells(dgvCOL_SELECTION.RECEIPTDATE).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Weaver_ClothReceipt_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(dgvCOL_SELECTION.PARTYDCNO).Value = Dt1.Rows(i).Item("Party_DcNo").ToString
                    .Rows(n).Cells(dgvCOL_SELECTION.CLOTHNAME).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                    .Rows(n).Cells(dgvCOL_SELECTION.ENDSCOUNT).Value = Dt1.Rows(i).Item("EndsCount_Name").ToString
                    .Rows(n).Cells(dgvCOL_SELECTION.PCS).Value = Val(Dt1.Rows(i).Item("noof_pcs").ToString)

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then  '---- BRT TEXTILES(SOMANUR)
                        If Val(Dt1.Rows(i).Item("Dc_Receipt_Meters").ToString) > 0 Then
                            .Rows(n).Cells(dgvCOL_SELECTION.METERS).Value = Format(Val(Dt1.Rows(i).Item("Dc_Receipt_Meters").ToString), "#########0.00")
                        Else
                            .Rows(n).Cells(dgvCOL_SELECTION.METERS).Value = Format(Val(Dt1.Rows(i).Item("Receipt_Meters").ToString), "#########0.00")
                        End If

                    Else
                        .Rows(n).Cells(dgvCOL_SELECTION.METERS).Value = Format(Val(Dt1.Rows(i).Item("Receipt_Meters").ToString), "#########0.00")

                    End If

                    .Rows(n).Cells(dgvCOL_SELECTION.STS).Value = ""
                    .Rows(n).Cells(dgvCOL_SELECTION.WEAVERCLOTHRECEIPTCODE).Value = Dt1.Rows(i).Item("Weaver_ClothReceipt_Code").ToString
                    .Rows(n).Cells(dgvCOL_SELECTION.FOLDING).Value = Val(Dt1.Rows(i).Item("folding").ToString)
                    .Rows(n).Cells(dgvCOL_SELECTION.RECEIPTPKCONDITION).Value = "WCLRC-"
                    .Rows(n).Cells(dgvCOL_SELECTION.PIECEFROMNUMBER).Value = Val(Dt1.Rows(i).Item("pcs_fromno").ToString)
                    .Rows(n).Cells(dgvCOL_SELECTION.PIECETONUMBER).Value = Val(Dt1.Rows(i).Item("pcs_tono").ToString)

                Next

            End If
            Dt1.Clear()

            '---------------------------
            'CLOTH PURCHASE RECEIPT
            '---------------------------

            Da = New SqlClient.SqlDataAdapter("select a.*, c.Cloth_Name from Cloth_Purchase_Receipt_Head a INNER JOIN  Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo where a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.Cloth_Purchase_Receipt_Date, a.for_orderby, a.Cloth_Purchase_Receipt_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(dgvCOL_SELECTION.SNO).Value = Val(SNo)

                    .Rows(n).Cells(dgvCOL_SELECTION.RECEIPTNO).Value = Dt1.Rows(i).Item("Cloth_Purchase_Receipt_No").ToString & "/" & Trim(Common_Procedures.LotCode.Purchase_Cloth_Receipt)
                    .Rows(n).Cells(dgvCOL_SELECTION.RECEIPTDATE).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Cloth_Purchase_Receipt_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(dgvCOL_SELECTION.PARTYDCNO).Value = Dt1.Rows(i).Item("Bill_No").ToString
                    .Rows(n).Cells(dgvCOL_SELECTION.CLOTHNAME).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                    .Rows(n).Cells(dgvCOL_SELECTION.ENDSCOUNT).Value = ""
                    .Rows(n).Cells(dgvCOL_SELECTION.PCS).Value = Val(Dt1.Rows(i).Item("noof_pcs").ToString)
                    .Rows(n).Cells(dgvCOL_SELECTION.METERS).Value = Format(Val(Dt1.Rows(i).Item("Receipt_Meters").ToString), "#########0.00")
                    .Rows(n).Cells(dgvCOL_SELECTION.STS).Value = "1"
                    .Rows(n).Cells(dgvCOL_SELECTION.WEAVERCLOTHRECEIPTCODE).Value = Dt1.Rows(i).Item("Cloth_Purchase_Receipt_Code").ToString
                    .Rows(n).Cells(dgvCOL_SELECTION.FOLDING).Value = Val(Dt1.Rows(i).Item("Folding").ToString)
                    .Rows(n).Cells(dgvCOL_SELECTION.RECEIPTPKCONDITION).Value = "CPREC-"
                    .Rows(n).Cells(dgvCOL_SELECTION.PIECEFROMNUMBER).Value = Val(Dt1.Rows(i).Item("pcs_fromno").ToString)
                    .Rows(n).Cells(dgvCOL_SELECTION.PIECETONUMBER).Value = Val(Dt1.Rows(i).Item("pcs_tono").ToString)

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()

            If Common_Procedures.settings.CustomerCode = "1516" Then
                '/deva
                Da = New SqlClient.SqlDataAdapter("select a.*, c.Cloth_Name from Cloth_Purchase_Receipt_Head a INNER JOIN  Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo where a.Weaver_Piece_Checking_Code = '' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " and a.Company_IdNo = " & Val(lbl_Company.Tag).ToString & " order by a.Cloth_Purchase_Receipt_Date, a.for_orderby, a.Cloth_Purchase_Receipt_No", con)
            Else
                'old code - not altered by deva
                Da = New SqlClient.SqlDataAdapter("select a.*, c.Cloth_Name from Cloth_Purchase_Receipt_Head a INNER JOIN  Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo where a.Weaver_Piece_Checking_Code = '' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.Cloth_Purchase_Receipt_Date, a.for_orderby, a.Cloth_Purchase_Receipt_No", con)
            End If
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(dgvCOL_SELECTION.SNO).Value = Val(SNo)

                    .Rows(n).Cells(dgvCOL_SELECTION.RECEIPTNO).Value = Dt1.Rows(i).Item("Cloth_Purchase_Receipt_No").ToString & "/" & Trim(Common_Procedures.LotCode.Purchase_Cloth_Receipt)
                    .Rows(n).Cells(dgvCOL_SELECTION.RECEIPTDATE).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Cloth_Purchase_Receipt_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(dgvCOL_SELECTION.PARTYDCNO).Value = Dt1.Rows(i).Item("Bill_No").ToString
                    .Rows(n).Cells(dgvCOL_SELECTION.CLOTHNAME).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                    .Rows(n).Cells(dgvCOL_SELECTION.ENDSCOUNT).Value = ""
                    .Rows(n).Cells(dgvCOL_SELECTION.PCS).Value = Val(Dt1.Rows(i).Item("noof_pcs").ToString)
                    .Rows(n).Cells(dgvCOL_SELECTION.METERS).Value = Format(Val(Dt1.Rows(i).Item("Receipt_Meters").ToString), "#########0.00")
                    .Rows(n).Cells(dgvCOL_SELECTION.STS).Value = ""
                    .Rows(n).Cells(dgvCOL_SELECTION.WEAVERCLOTHRECEIPTCODE).Value = Dt1.Rows(i).Item("Cloth_Purchase_Receipt_Code").ToString
                    .Rows(n).Cells(dgvCOL_SELECTION.FOLDING).Value = Val(Dt1.Rows(i).Item("Folding").ToString)
                    .Rows(n).Cells(dgvCOL_SELECTION.RECEIPTPKCONDITION).Value = "CPREC-"
                    .Rows(n).Cells(dgvCOL_SELECTION.PIECEFROMNUMBER).Value = Val(Dt1.Rows(i).Item("pcs_fromno").ToString)
                    .Rows(n).Cells(dgvCOL_SELECTION.PIECETONUMBER).Value = Val(Dt1.Rows(i).Item("pcs_tono").ToString)

                Next

            End If
            Dt1.Clear()



            '---------------------------
            'CLOTH DELIVERY RETURN
            '---------------------------

            Da = New SqlClient.SqlDataAdapter("select a.*, c.Cloth_Name from ClothSales_Delivery_Return_Head a INNER JOIN  Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo where a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.ClothSales_Delivery_Return_Date, a.for_orderby, a.ClothSales_Delivery_Return_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(dgvCOL_SELECTION.SNO).Value = Val(SNo)

                    .Rows(n).Cells(dgvCOL_SELECTION.RECEIPTNO).Value = Dt1.Rows(i).Item("ClothSales_Delivery_Return_No").ToString & "/" & Trim(Common_Procedures.LotCode.Delivery_Return)
                    .Rows(n).Cells(dgvCOL_SELECTION.RECEIPTDATE).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("ClothSales_Delivery_Return_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(dgvCOL_SELECTION.PARTYDCNO).Value = Dt1.Rows(i).Item("Dc_No").ToString
                    .Rows(n).Cells(dgvCOL_SELECTION.CLOTHNAME).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                    .Rows(n).Cells(dgvCOL_SELECTION.ENDSCOUNT).Value = ""
                    .Rows(n).Cells(dgvCOL_SELECTION.PCS).Value = Val(Dt1.Rows(i).Item("noof_pcs").ToString)
                    .Rows(n).Cells(dgvCOL_SELECTION.METERS).Value = Format(Val(Dt1.Rows(i).Item("Return_Meters").ToString), "#########0.00")
                    .Rows(n).Cells(dgvCOL_SELECTION.STS).Value = "1"
                    .Rows(n).Cells(dgvCOL_SELECTION.WEAVERCLOTHRECEIPTCODE).Value = Dt1.Rows(i).Item("ClothSales_Delivery_Return_Code").ToString
                    .Rows(n).Cells(dgvCOL_SELECTION.FOLDING).Value = Val(Dt1.Rows(i).Item("Folding").ToString)
                    .Rows(n).Cells(dgvCOL_SELECTION.RECEIPTPKCONDITION).Value = "CLDRT-"
                    .Rows(n).Cells(dgvCOL_SELECTION.PIECEFROMNUMBER).Value = Val(Dt1.Rows(i).Item("pcs_fromno").ToString)
                    .Rows(n).Cells(dgvCOL_SELECTION.PIECETONUMBER).Value = Val(Dt1.Rows(i).Item("pcs_tono").ToString)

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()

            Da = New SqlClient.SqlDataAdapter("select a.*, c.Cloth_Name from ClothSales_Delivery_Return_Head a INNER JOIN  Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo where a.Weaver_Piece_Checking_Code = '' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.ClothSales_Delivery_Return_Date, a.for_orderby, a.ClothSales_Delivery_Return_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(dgvCOL_SELECTION.SNO).Value = Val(SNo)

                    .Rows(n).Cells(dgvCOL_SELECTION.RECEIPTNO).Value = Dt1.Rows(i).Item("ClothSales_Delivery_Return_No").ToString & "/" & Trim(Common_Procedures.LotCode.Delivery_Return)
                    .Rows(n).Cells(dgvCOL_SELECTION.RECEIPTDATE).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("ClothSales_Delivery_Return_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(dgvCOL_SELECTION.PARTYDCNO).Value = Dt1.Rows(i).Item("Dc_No").ToString
                    .Rows(n).Cells(dgvCOL_SELECTION.CLOTHNAME).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                    .Rows(n).Cells(dgvCOL_SELECTION.ENDSCOUNT).Value = ""
                    .Rows(n).Cells(dgvCOL_SELECTION.PCS).Value = Val(Dt1.Rows(i).Item("noof_pcs").ToString)
                    .Rows(n).Cells(dgvCOL_SELECTION.METERS).Value = Format(Val(Dt1.Rows(i).Item("Return_Meters").ToString), "#########0.00")
                    .Rows(n).Cells(dgvCOL_SELECTION.STS).Value = ""
                    .Rows(n).Cells(dgvCOL_SELECTION.WEAVERCLOTHRECEIPTCODE).Value = Dt1.Rows(i).Item("ClothSales_Delivery_Return_Code").ToString
                    .Rows(n).Cells(dgvCOL_SELECTION.FOLDING).Value = Val(Dt1.Rows(i).Item("Folding").ToString)
                    .Rows(n).Cells(dgvCOL_SELECTION.RECEIPTPKCONDITION).Value = "CLDRT-"
                    .Rows(n).Cells(dgvCOL_SELECTION.PIECEFROMNUMBER).Value = Val(Dt1.Rows(i).Item("pcs_fromno").ToString)
                    .Rows(n).Cells(dgvCOL_SELECTION.PIECETONUMBER).Value = Val(Dt1.Rows(i).Item("pcs_tono").ToString)


                Next

            End If
            Dt1.Clear()

            '--------------------------------
            '      CLOTH SALES RETURN      
            '--------------------------------

            Da = New SqlClient.SqlDataAdapter("select a.*, c.Cloth_Name from ClothSales_Return_Head a INNER JOIN  Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo where a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.ClothSales_Return_Date, a.for_orderby, a.ClothSales_Return_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(dgvCOL_SELECTION.SNO).Value = Val(SNo)

                    If InStr(1, Trim(UCase(Dt1.Rows(i).Item("ClothSales_Return_Code").ToString)), "GCLSR-") > 0 Then
                        .Rows(n).Cells(dgvCOL_SELECTION.RECEIPTNO).Value = Dt1.Rows(i).Item("ClothSales_Return_No").ToString & "/" & Trim(Common_Procedures.LotCode.Sales_Return_GST)
                    Else
                        .Rows(n).Cells(dgvCOL_SELECTION.RECEIPTNO).Value = Dt1.Rows(i).Item("ClothSales_Return_No").ToString & "/" & Trim(Common_Procedures.LotCode.Sales_Return)
                    End If
                    .Rows(n).Cells(dgvCOL_SELECTION.RECEIPTDATE).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("ClothSales_Return_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(dgvCOL_SELECTION.PARTYDCNO).Value = Dt1.Rows(i).Item("Invoice_No").ToString
                    .Rows(n).Cells(dgvCOL_SELECTION.CLOTHNAME).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                    .Rows(n).Cells(dgvCOL_SELECTION.ENDSCOUNT).Value = ""
                    .Rows(n).Cells(dgvCOL_SELECTION.PCS).Value = Val(Dt1.Rows(i).Item("noof_pcs").ToString)
                    .Rows(n).Cells(dgvCOL_SELECTION.METERS).Value = Format(Val(Dt1.Rows(i).Item("Return_Meters").ToString), "#########0.00")
                    .Rows(n).Cells(dgvCOL_SELECTION.STS).Value = "1"
                    .Rows(n).Cells(dgvCOL_SELECTION.WEAVERCLOTHRECEIPTCODE).Value = Dt1.Rows(i).Item("ClothSales_Return_Code").ToString
                    .Rows(n).Cells(dgvCOL_SELECTION.FOLDING).Value = 0
                    If InStr(1, Trim(UCase(Dt1.Rows(i).Item("ClothSales_Return_Code").ToString)), "GCLSR-") > 0 Then
                        .Rows(n).Cells(dgvCOL_SELECTION.RECEIPTPKCONDITION).Value = "GCLSR-"
                    Else
                        .Rows(n).Cells(dgvCOL_SELECTION.RECEIPTPKCONDITION).Value = "CLSRT-"
                    End If
                    .Rows(n).Cells(dgvCOL_SELECTION.PIECEFROMNUMBER).Value = Val(Dt1.Rows(i).Item("pcs_fromno").ToString)
                    .Rows(n).Cells(dgvCOL_SELECTION.PIECETONUMBER).Value = Val(Dt1.Rows(i).Item("pcs_tono").ToString)

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()


            Da = New SqlClient.SqlDataAdapter("select a.*, c.Cloth_Name from ClothSales_Return_Head a INNER JOIN  Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo where a.Weaver_Piece_Checking_Code = '' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.ClothSales_Return_Date, a.for_orderby, a.ClothSales_Return_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(dgvCOL_SELECTION.SNO).Value = Val(SNo)
                    If InStr(1, Trim(UCase(Dt1.Rows(i).Item("ClothSales_Return_Code").ToString)), "GCLSR-") > 0 Then
                        .Rows(n).Cells(dgvCOL_SELECTION.RECEIPTNO).Value = Dt1.Rows(i).Item("ClothSales_Return_No").ToString & "/" & Trim(Common_Procedures.LotCode.Sales_Return_GST)
                    Else
                        .Rows(n).Cells(dgvCOL_SELECTION.RECEIPTNO).Value = Dt1.Rows(i).Item("ClothSales_Return_No").ToString & "/" & Trim(Common_Procedures.LotCode.Sales_Return)
                    End If
                    .Rows(n).Cells(dgvCOL_SELECTION.RECEIPTDATE).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("ClothSales_Return_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(dgvCOL_SELECTION.PARTYDCNO).Value = Dt1.Rows(i).Item("Invoice_No").ToString
                    .Rows(n).Cells(dgvCOL_SELECTION.CLOTHNAME).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                    .Rows(n).Cells(dgvCOL_SELECTION.ENDSCOUNT).Value = ""
                    .Rows(n).Cells(dgvCOL_SELECTION.PCS).Value = Val(Dt1.Rows(i).Item("noof_pcs").ToString)
                    .Rows(n).Cells(dgvCOL_SELECTION.METERS).Value = Format(Val(Dt1.Rows(i).Item("Return_Meters").ToString), "#########0.00")
                    .Rows(n).Cells(dgvCOL_SELECTION.STS).Value = ""
                    .Rows(n).Cells(dgvCOL_SELECTION.WEAVERCLOTHRECEIPTCODE).Value = Dt1.Rows(i).Item("ClothSales_Return_Code").ToString
                    .Rows(n).Cells(dgvCOL_SELECTION.FOLDING).Value = 0
                    If InStr(1, Trim(UCase(Dt1.Rows(i).Item("ClothSales_Return_Code").ToString)), "GCLSR-") > 0 Then
                        .Rows(n).Cells(dgvCOL_SELECTION.RECEIPTPKCONDITION).Value = "GCLSR-"
                    Else
                        .Rows(n).Cells(dgvCOL_SELECTION.RECEIPTPKCONDITION).Value = "CLSRT-"
                    End If
                    .Rows(n).Cells(dgvCOL_SELECTION.PIECEFROMNUMBER).Value = Val(Dt1.Rows(i).Item("pcs_fromno").ToString)
                    .Rows(n).Cells(dgvCOL_SELECTION.PIECETONUMBER).Value = Val(Dt1.Rows(i).Item("pcs_tono").ToString)

                Next

            End If
            Dt1.Clear()



            '---------------------------
            ' PROCESSED FABRIC RECEIPT
            '---------------------------

            vSQLCONDT = ""
            For K = 1 To 2

                If K = 1 Then
                    vSQLCONDT = " a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and "
                Else
                    vSQLCONDT = " a.Weaver_Piece_Checking_Code = '' and  "
                End If

                Da = New SqlClient.SqlDataAdapter("select b.*, tC.Cloth_Name from Textile_Processing_Receipt_Head a INNER JOIN  Textile_Processing_Receipt_Details b ON a.ClothProcess_Receipt_Code = b.Cloth_Processing_Receipt_Code INNER JOIN Cloth_Head tC ON tC.Cloth_IdNo = b.Item_To_Idno Where " & Trim(vSQLCONDT) & " a.DeliveryTo_IdNo = " & Str(Val(Common_Procedures.CommonLedger.Godown_Ac)) & " order by b.Cloth_Processing_Receipt_Date, b.for_orderby, b.Cloth_Processing_Receipt_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(dgvCOL_SELECTION.SNO).Value = Val(SNo)

                        .Rows(n).Cells(dgvCOL_SELECTION.RECEIPTNO).Value = Dt1.Rows(i).Item("Cloth_Processing_Receipt_No").ToString & "/" & Trim(Common_Procedures.LotCode.Processed_Fabric_Receipt)
                        .Rows(n).Cells(dgvCOL_SELECTION.RECEIPTDATE).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Cloth_Processing_Receipt_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(dgvCOL_SELECTION.PARTYDCNO).Value = Dt1.Rows(i).Item("Dc_Rc_No").ToString
                        .Rows(n).Cells(dgvCOL_SELECTION.CLOTHNAME).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                        .Rows(n).Cells(dgvCOL_SELECTION.ENDSCOUNT).Value = ""
                        .Rows(n).Cells(dgvCOL_SELECTION.PCS).Value = Val(Dt1.Rows(i).Item("Receipt_Pcs").ToString)
                        .Rows(n).Cells(dgvCOL_SELECTION.METERS).Value = Format(Val(Dt1.Rows(i).Item("Receipt_Meters").ToString), "#########0.00")

                        If K = 1 Then
                            .Rows(n).Cells(dgvCOL_SELECTION.STS).Value = "1"
                        Else
                            .Rows(n).Cells(dgvCOL_SELECTION.STS).Value = ""
                        End If

                        .Rows(n).Cells(dgvCOL_SELECTION.WEAVERCLOTHRECEIPTCODE).Value = Dt1.Rows(i).Item("Cloth_Processing_Receipt_Code").ToString
                        .Rows(n).Cells(dgvCOL_SELECTION.FOLDING).Value = Val(Dt1.Rows(i).Item("Folding").ToString)
                        .Rows(n).Cells(dgvCOL_SELECTION.RECEIPTPKCONDITION).Value = PkCondition_PROFABRCPT
                        .Rows(n).Cells(dgvCOL_SELECTION.PIECEFROMNUMBER).Value = 1
                        If Val(Dt1.Rows(i).Item("Receipt_Pcs").ToString) <> 0 Then
                            .Rows(n).Cells(dgvCOL_SELECTION.PIECETONUMBER).Value = Val(Dt1.Rows(i).Item("Receipt_Pcs").ToString)
                        Else
                            .Rows(n).Cells(dgvCOL_SELECTION.PIECETONUMBER).Value = 1
                        End If

                        If K = 1 Then
                            For j = 0 To .ColumnCount - 1
                                .Rows(i).Cells(j).Style.ForeColor = Color.Red
                            Next
                        End If

                    Next

                End If
                Dt1.Clear()

            Next K

        End With

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        pnl_Selection.Visible = True
        pnl_Selection.BringToFront()
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

                For i = 0 To dgv_Selection.Rows.Count - 1
                    dgv_Selection.Rows(i).Cells(dgvCOL_SELECTION.STS).Value = ""
                Next

                .Rows(RwIndx).Cells(dgvCOL_SELECTION.STS).Value = 1

                If Val(.Rows(RwIndx).Cells(dgvCOL_SELECTION.STS).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next


                Else
                    .Rows(RwIndx).Cells(dgvCOL_SELECTION.STS).Value = ""

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
                    Next

                End If

                Close_Receipt_Selection()

            End If

        End With

    End Sub

    Private Sub dgv_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Selection.KeyDown
        Dim n As Integer

        On Error Resume Next

        If IsNothing(dgv_Selection.CurrentCell) Then Exit Sub

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            If dgv_Selection.CurrentCell.RowIndex >= 0 Then
                n = dgv_Selection.CurrentCell.RowIndex
                Select_Piece(n)
                e.Handled = True
            End If
        End If

    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Close_Receipt_Selection()
    End Sub

    Private Sub Close_Receipt_Selection()
        Dim cmd As New SqlClient.SqlCommand
        Dim SQL1 As String
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim K As Integer = 0
        Dim M As Integer = 0
        Dim Clo_Pck As Single = 0
        Dim Clo_Wdth As Single = 0
        Dim vCloRec_Code As String = ""


        pnl_Back.Enabled = True
        pnl_Selection.Visible = False

        dgv_Details.Rows.Clear()

        cmd.Connection = con

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(dgvCOL_SELECTION.STS).Value) = 1 Then

                lbl_RecCode.Text = dgv_Selection.Rows(i).Cells(dgvCOL_SELECTION.WEAVERCLOTHRECEIPTCODE).Value
                txt_RecNo.Text = dgv_Selection.Rows(i).Cells(dgvCOL_SELECTION.RECEIPTNO).Value
                lbl_LotNo.Text = txt_RecNo.Text
                msk_Rec_Date.Text = dgv_Selection.Rows(i).Cells(dgvCOL_SELECTION.RECEIPTDATE).Value
                txt_PDcNo.Text = dgv_Selection.Rows(i).Cells(dgvCOL_SELECTION.PARTYDCNO).Value
                cbo_Quality.Text = dgv_Selection.Rows(i).Cells(dgvCOL_SELECTION.CLOTHNAME).Value
                txt_No_Pcs.Text = dgv_Selection.Rows(i).Cells(dgvCOL_SELECTION.PCS).Value
                txt_Rec_Meter.Text = dgv_Selection.Rows(i).Cells(dgvCOL_SELECTION.METERS).Value
                txt_Folding.Text = dgv_Selection.Rows(i).Cells(dgvCOL_SELECTION.FOLDING).Value

                If Val(txt_Folding.Text) = 0 Then txt_Folding.Text = 100
                lbl_RecPkCondition.Text = dgv_Selection.Rows(i).Cells(dgvCOL_SELECTION.RECEIPTPKCONDITION).Value

                Da1 = New SqlClient.SqlDataAdapter("Select a.* from Weaver_Cloth_Receipt_Head a Where a.Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "'", con)
                Dt1 = New DataTable
                Da1.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    Lbl_StockOff.Text = Common_Procedures.Ledger_IdNoToName(con, Val(Dt1.Rows(0).Item("StockOff_IdNo").ToString))
                    lbl_Godown.Text = Common_Procedures.Ledger_IdNoToName(con, Val(Dt1.Rows(0).Item("WareHouse_IdNo").ToString))
                    If lbl_LotNo.Visible = True Then
                        lbl_LotNo.Text = Dt1.Rows(0).Item("Lot_No").ToString
                    End If
                    cbo_LoomType.Text = Dt1.Rows(0).Item("Loom_Type").ToString
                End If
                If Dt1.Rows.Count = 0 Then

                    Da1 = New SqlClient.SqlDataAdapter("Select a.* from CLOTH_PURCHASE_RECEIPT_HEAD a Where a.CLOTH_PURCHASE_RECEIPT_CODE = '" & Trim(lbl_RecCode.Text) & "'", con)
                    Dt1 = New DataTable
                    Da1.Fill(Dt1)
                    If Dt1.Rows.Count > 0 Then
                        'Lbl_StockOff.Text = Common_Procedures.Ledger_IdNoToName(con, Val(Dt1.Rows(0).Item("StockOff_IdNo").ToString))
                        lbl_Godown.Text = Common_Procedures.Ledger_IdNoToName(con, Val(Dt1.Rows(0).Item("Deliver_At_IdNo").ToString))
                        'If lbl_LotNo.Visible = True Then
                        '    lbl_LotNo.Text = Dt1.Rows(0).Item("Lot_No").ToString
                        'End If
                        'cbo_LoomType.Text = Dt1.Rows(0).Item("Loom_Type").ToString
                    End If

                End If
                Dt1.Clear()

                If Trim(Lbl_StockOff.Text) = "" Then
                    Lbl_StockOff.Text = Common_Procedures.Ledger_IdNoToName(con, 4)
                End If

                vCloRec_Code = ""
                If InStr(1, Trim(UCase(lbl_RecCode.Text)), Trim(UCase(lbl_RecPkCondition.Text))) > 0 Then
                    vCloRec_Code = Trim(lbl_RecCode.Text)
                Else
                    vCloRec_Code = Trim(lbl_RecPkCondition.Text) & Trim(lbl_RecCode.Text)
                End If

                SQL1 = "Select a.*, b.* from Weaver_ClothReceipt_Piece_Details a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Weaver_ClothReceipt_Code = '" & Trim(vCloRec_Code) & "' Order by a.sl_no"
                cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
                Da1 = New SqlClient.SqlDataAdapter(cmd)
                Dt1 = New DataTable
                Da1.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    Clo_Pck = Val(Dt1.Rows(0).Item("Cloth_Pick").ToString)
                    Clo_Wdth = Val(Dt1.Rows(0).Item("Cloth_Width").ToString)

                    For j = 0 To Dt1.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        dgv_Details.Rows(n).Cells(dgvCOL_PCSDETAILS.PCSNO).Value = Dt1.Rows(j).Item("Piece_No").ToString
                        If Val(Dt1.Rows(j).Item("Receipt_Meters").ToString) <> 0 Then
                            dgv_Details.Rows(n).Cells(dgvCOL_PCSDETAILS.RECEIPTMETER).Value = Dt1.Rows(j).Item("Receipt_Meters").ToString
                        End If
                        dgv_Details.Rows(n).Cells(dgvCOL_PCSDETAILS.LOOMNO).Value = Dt1.Rows(j).Item("Loom_No").ToString
                        If Val(Dt1.Rows(j).Item("Pick").ToString) <> 0 Then
                            dgv_Details.Rows(n).Cells(dgvCOL_PCSDETAILS.PICK).Value = Val(Dt1.Rows(j).Item("Pick").ToString)
                        Else
                            If Val(Dt1.Rows(j).Item("Cloth_Pick").ToString) <> 0 Then
                                dgv_Details.Rows(n).Cells(dgvCOL_PCSDETAILS.PICK).Value = Val(Dt1.Rows(j).Item("Cloth_Pick").ToString)
                            End If
                        End If
                        If Val(Dt1.Rows(j).Item("Width").ToString) <> 0 Then
                            dgv_Details.Rows(n).Cells(dgvCOL_PCSDETAILS.WIDTH).Value = Val(Dt1.Rows(j).Item("Width").ToString)
                        Else
                            If Val(Dt1.Rows(j).Item("Cloth_Width").ToString) <> 0 Then
                                dgv_Details.Rows(n).Cells(dgvCOL_PCSDETAILS.WIDTH).Value = Val(Dt1.Rows(j).Item("Cloth_Width").ToString)
                            End If
                        End If
                        If Val(Dt1.Rows(j).Item("Type1_Meters").ToString) <> 0 Then
                            dgv_Details.Rows(n).Cells(dgvCOL_PCSDETAILS.TYPE1METER).Value = Format(Val(Dt1.Rows(j).Item("Type1_Meters").ToString), "#########0.00")
                        End If
                        If Val(Dt1.Rows(j).Item("Type2_Meters").ToString) <> 0 Then
                            dgv_Details.Rows(n).Cells(dgvCOL_PCSDETAILS.TYPE2METER).Value = Format(Val(Dt1.Rows(j).Item("Type2_Meters").ToString), "#########0.00")
                        End If
                        If Val(Dt1.Rows(j).Item("Type3_Meters").ToString) <> 0 Then
                            dgv_Details.Rows(n).Cells(dgvCOL_PCSDETAILS.TYPE3METER).Value = Format(Val(Dt1.Rows(j).Item("Type3_Meters").ToString), "#########0.00")
                        End If
                        If Val(Dt1.Rows(j).Item("Type4_Meters").ToString) <> 0 Then
                            dgv_Details.Rows(n).Cells(dgvCOL_PCSDETAILS.TYPE4METER).Value = Format(Val(Dt1.Rows(j).Item("Type4_Meters").ToString), "#########0.00")
                        End If
                        If Val(Dt1.Rows(j).Item("Type5_Meters").ToString) <> 0 Then
                            dgv_Details.Rows(n).Cells(dgvCOL_PCSDETAILS.TYPE5METER).Value = Format(Val(Dt1.Rows(j).Item("Type5_Meters").ToString), "#########0.00")
                        End If
                        If (Val(Dt1.Rows(j).Item("Type1_Meters").ToString) + Val(Dt1.Rows(j).Item("Type2_Meters").ToString) + Val(Dt1.Rows(j).Item("Type3_Meters").ToString) + Val(Dt1.Rows(j).Item("Type4_Meters").ToString) + Val(Dt1.Rows(j).Item("Type5_Meters").ToString)) <> 0 Then
                            dgv_Details.Rows(n).Cells(dgvCOL_PCSDETAILS.TOTALMETER).Value = Format(Val(Dt1.Rows(j).Item("Type1_Meters").ToString) + Val(Dt1.Rows(j).Item("Type2_Meters").ToString) + Val(Dt1.Rows(j).Item("Type3_Meters").ToString) + Val(Dt1.Rows(j).Item("Type4_Meters").ToString) + Val(Dt1.Rows(j).Item("Type5_Meters").ToString), "#########0.00")
                        End If
                        If Val(Dt1.Rows(j).Item("Weight").ToString) <> 0 Then
                            dgv_Details.Rows(n).Cells(dgvCOL_PCSDETAILS.WEIGHT).Value = Format(Val(Dt1.Rows(j).Item("Weight").ToString), "#########0.000")
                        End If
                        If Val(Dt1.Rows(j).Item("Weight_Meter").ToString) <> 0 Then
                            dgv_Details.Rows(n).Cells(dgvCOL_PCSDETAILS.WEIGHTPERMETER).Value = Format(Val(Dt1.Rows(j).Item("Weight_Meter").ToString), "#########0.000")
                        End If

                    Next

                    For K = Val(dgv_Selection.Rows(i).Cells(dgvCOL_SELECTION.PIECEFROMNUMBER).Value) To (Val(dgv_Selection.Rows(i).Cells(dgvCOL_SELECTION.PIECEFROMNUMBER).Value) + Val(txt_No_Pcs.Text) - 1)

                        For M = 0 To dgv_Details.Rows.Count - 1
                            If K = Val(dgv_Details.Rows(M).Cells(dgvCOL_PCSDETAILS.PCSNO).Value) Then
                                GoTo LOOOP1
                            End If
                        Next

                        For j = 0 To dgv_Details.Rows.Count - 1
                            If K < Val(dgv_Details.Rows(j).Cells(dgvCOL_PCSDETAILS.PCSNO).Value) Then
                                dgv_Details.Rows.Insert(j, 1)
                                dgv_Details.Rows(j).Cells(dgvCOL_PCSDETAILS.PCSNO).Value = K
                                If Val(Clo_Pck) <> 0 Then
                                    dgv_Details.Rows(j).Cells(dgvCOL_PCSDETAILS.PICK).Value = Val(Clo_Pck)
                                End If
                                If Val(Clo_Wdth) <> 0 Then
                                    dgv_Details.Rows(j).Cells(dgvCOL_PCSDETAILS.WIDTH).Value = Val(Clo_Wdth)
                                End If
                                GoTo LOOOP1
                            End If
                        Next

                        n = dgv_Details.Rows.Add()
                        dgv_Details.Rows(n).Cells(dgvCOL_PCSDETAILS.PCSNO).Value = K
                        If Val(Clo_Pck) <> 0 Then
                            dgv_Details.Rows(n).Cells(dgvCOL_PCSDETAILS.PICK).Value = Val(Clo_Pck)
                        End If
                        If Val(Clo_Wdth) <> 0 Then
                            dgv_Details.Rows(n).Cells(dgvCOL_PCSDETAILS.WIDTH).Value = Val(Clo_Wdth)
                        End If

LOOOP1:

                    Next

                Else

                    For K = Val(dgv_Selection.Rows(i).Cells(dgvCOL_SELECTION.PIECEFROMNUMBER).Value) To (Val(dgv_Selection.Rows(i).Cells(dgvCOL_SELECTION.PIECEFROMNUMBER).Value) + Val(txt_No_Pcs.Text) - 1)

                        n = dgv_Details.Rows.Add()

                        dgv_Details.Rows(n).Cells(dgvCOL_PCSDETAILS.PCSNO).Value = K
                        If Val(Clo_Pck) <> 0 Then
                            dgv_Details.Rows(n).Cells(dgvCOL_PCSDETAILS.PICK).Value = Val(Clo_Pck)
                        End If
                        If Val(Clo_Wdth) <> 0 Then
                            dgv_Details.Rows(n).Cells(dgvCOL_PCSDETAILS.WIDTH).Value = Val(Clo_Wdth)
                        End If

                    Next

                End If
                Dt1.Clear()

                Total_Calculation1()

                Exit For

            End If

        Next i

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False

        If cbo_LoomType.Visible And cbo_LoomType.Enabled Then
            cbo_LoomType.Focus()
        ElseIf txt_Folding.Visible And txt_Folding.Enabled Then
            txt_Folding.Focus()
        ElseIf dgv_Details.Rows.Count > 1 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
        End If

    End Sub

    Private Sub txt_Folding_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Folding.TextChanged
        Total_Calculation1()
    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If
        If e.Control = True And (UCase(Chr(e.KeyCode)) = "A" Or UCase(Chr(e.KeyCode)) = "I" Or e.KeyCode = Keys.Insert) Then
            dgv_Details_KeyUp(sender, e)
        End If
    End Sub

    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        If e.KeyCode = 107 Then
            msk_date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_date.Text))
            msk_date.SelectionStart = 0
        ElseIf e.KeyCode = 109 Then
            msk_date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_date.Text))
            msk_date.SelectionStart = 0
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)
        End If

    End Sub

    Private Sub msk_date_TextChanged(sender As Object, e As EventArgs) Handles msk_date.TextChanged
        Try

            If FrmLdSTS = True Then Exit Sub

            If Me.ActiveControl.Name <> dtp_Date.Name Then

                If IsDate(msk_date.Text) = True Then
                    dtp_Date.Value = Convert.ToDateTime(msk_date.Text)
                End If

            End If

            'End If



        Catch ex As Exception
            '---

        End Try

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

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        Try

            If FrmLdSTS = True Then Exit Sub

            If Me.ActiveControl.Name <> msk_date.Name Then
                If IsDate(dtp_Date.Text) = True Then
                    msk_date.Text = dtp_Date.Text
                    msk_date.SelectionStart = 0
                End If
            End If

        Catch ex As Exception
            '----
        End Try

    End Sub

    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            If cbo_Weaver.Enabled And cbo_Weaver.Visible Then
                cbo_Weaver.Focus()
            ElseIf cbo_LoomType.Visible And cbo_LoomType.Enabled Then
                cbo_LoomType.Focus()
            Else
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCOL_PCSDETAILS.RECEIPTMETER)
                End If

            End If


        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCOL_PCSDETAILS.RECEIPTMETER)
            Else
                If txt_Folding.Visible And txt_Folding.Enabled Then
                    txt_Folding.Focus()
                ElseIf cbo_LoomType.Visible And cbo_LoomType.Enabled Then
                    cbo_LoomType.Focus()
                End If
            End If

        End If

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

        LastNo = lbl_ChkNo.Text

        movefirst_record()
        Timer1.Enabled = True

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        save_record()
        If Trim(UCase(LastNo)) = Trim(UCase(lbl_ChkNo.Text)) Then
            Timer1.Enabled = False
            SaveAll_STS = False
            MessageBox.Show("All entries saved Successfully", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Else
            movenext_record()

        End If
    End Sub

    Private Sub txt_Filter_LotNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Filter_LotNo.KeyDown
        On Error Resume Next
        If e.KeyValue = 38 Then e.Handled = True : SendKeys.Send("+{TAB}")

        If e.KeyValue = 40 Or (e.Control = True And e.KeyValue = 40) Then
            If Common_Procedures.settings.CustomerCode = "1249" Then
                cbo_Verified_Sts.Focus()
            Else
                'btn_Filter_Show.Focus()
                txt_filter_Chkno.Focus()

            End If

        End If
        'If e.KeyValue = 40 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_Filter_LotNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Filter_LotNo.KeyPress
        On Error Resume Next
        If Asc(e.KeyChar) = 13 Then
            If Common_Procedures.settings.CustomerCode = "1249" Then
                cbo_Verified_Sts.Focus()
            Else
                'btn_Filter_Show.Focus()
                txt_filter_Chkno.Focus()

            End If
        End If
        'If Asc(e.KeyChar) = 13 Then e.Handled = True : btn_Filter_Show_Click(sender, e)
    End Sub

    Private Sub msk_date_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles msk_date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_date.Text = Date.Today
            msk_date.SelectionStart = 0
        End If
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            If cbo_Weaver.Enabled And cbo_Weaver.Enabled Then
                cbo_Weaver.Focus()

            ElseIf cbo_LoomType.Visible And cbo_LoomType.Enabled Then
                cbo_LoomType.Focus()


            Else
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCOL_PCSDETAILS.RECEIPTMETER)
                    dgv_Details.CurrentCell.Selected = True
                End If

            End If

        End If

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

    Private Sub Printing_Format2(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        Dim p1Font As Font
        Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim I As Integer
        Dim NoofItems_PerPage As Integer, NoofDets, sno As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim C1 As Single, C2 As Single

        p1Font = New Font("Calibri", 10, FontStyle.Bold)

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
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

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30
            .Right = 50
            .Top = 30
            .Bottom = 30
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



        Erase LnAr
        Erase ClAr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}


        ClAr(1) = Val(30) : ClAr(2) = 30 : ClAr(3) = 50 : ClAr(4) = 60 : ClAr(5) = 60 : ClAr(6) = 70 : ClAr(7) = 70 : ClAr(8) = 60 : ClAr(9) = 60 : ClAr(10) = 55 : ClAr(11) = 45 : ClAr(12) = 60 : ClAr(13) = 35
        ClAr(14) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13))


        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7)
        C2 = C1 + ClAr(8)

        TxtHgt = 18

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        sno = 0

        Try
            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format2_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1


                        NoofDets = NoofDets + 1

                        sno = sno + 1
                        vType1 = Val(prn_DetDt.Rows(prn_DetIndx).Item("Type1_Meters").ToString())
                        vType2 = Val(prn_DetDt.Rows(prn_DetIndx).Item("Type2_Meters").ToString())
                        vType3 = Val(prn_DetDt.Rows(prn_DetIndx).Item("Type3_Meters").ToString())
                        vType4 = Val(prn_DetDt.Rows(prn_DetIndx).Item("Type4_Meters").ToString())
                        vType5 = Val(prn_DetDt.Rows(prn_DetIndx).Item("Type5_Meters").ToString())


                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Piece_No").ToString, LMargin + 5, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, "", LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Loom_No").ToString), LMargin + ClAr(1) + ClAr(2) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, IIf(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString) <> 0, Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, IIf(Val(prn_DetDt.Rows(prn_DetIndx).Item("Receipt_Meters").ToString) <> 0, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Receipt_Meters").ToString), "#######0.00"), ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, IIf(Val(vType1) <> 0, Format(Val(vType1), "#######0.00"), ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, IIf(Val(vType2) <> 0, Format(Val(vType2), "#######0.00"), ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, IIf(Val(vType3) <> 0, Format(Val(vType3), "#######0.00"), ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, IIf(Val(vType4) <> 0, Format(Val(vType4), "#######0.00"), ""), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Pick").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Width").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 5, CurY, 1, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Weight_Meter").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) - 5, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetDt.Rows(prn_DetIndx).Item("Remarks").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + ClAr(13) - 15, CurY, 0, 0, pFont)


                        CurY = CurY + TxtHgt + 10
                        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

                        NoofDets = NoofDets + 1

                        prn_DetIndx = prn_DetIndx + 1

                    Loop
                End If

                Printing_Format2_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format2_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim p1Font As Font
        Dim Cmp_Name As String
        Dim strHeight As Single
        Dim C1, w1, w2 As Single

        PageNo = PageNo + 1

        CurY = TMargin


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = ""

        Type1 = "" : Type2 = "" : Type3 = "" : Type4 = "" : Type5 = ""
        Type11 = "" : Type22 = "" : Type33 = "" : Type44 = "" : Type55 = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString



        Type1 = "I" : Type2 = "II" : Type3 = "III" : Type4 = "IV" : Type5 = "V"
        Type11 = "QUALITY" : Type22 = "QUALITY" : Type33 = "QUALITY" : Type44 = "QUALITY" : Type55 = "QUALITY"




        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + TxtHgt + 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "FOLDING REPORT", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 20

        w1 = e.Graphics.MeasureString("CHECKING DATE : ", pFont).Width
        w2 = e.Graphics.MeasureString("RECEIVED MTRS ", pFont).Width


        CurY = CurY + TxtHgt - 10
        pFont = New Font("Calibri", 10, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, "NAME   :  " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "QUALITY", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cloth_Name").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)



        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "CHECKING DATE", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + w1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Weaver_Piece_Checking_Date").ToString), "dd-MM-yyyy").ToString, LMargin + w1 + 30, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "RECEIVED NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Piece_Receipt_No").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "DATE :  " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Piece_Receipt_Date").ToString), "dd-MM-yyyy").ToString, PageWidth - 10, CurY, 1, 0, pFont)



        '"select b.weaver_wages_no, b.weaver_wages_date from weaver_cloth_receipt_head a INNER JOIN weaver_wages_head b ON A.WEAVER_WAGES_CODE = B.WEAVER_WAGes_code where a.weaver_clothreceipt_code = '" & trim(reccode) & "'

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "CHECKING noof_pcs", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + w1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("noof_pcs").ToString), "#######0"), LMargin + w1 + 30, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "RECEIVED MTRS", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("ReceiptMeters_Receipt").ToString), "#######0.00"), LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "FOLDING :  " & Format(Val(prn_HdDt.Rows(0).Item("Folding").ToString), "#######0"), PageWidth - 10, CurY, 1, 0, pFont)


        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        CurY = CurY + 10

        Common_Procedures.Print_To_PrintDocument(e, "PC", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "CHE", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "LOOM", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "SIZE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)

        Common_Procedures.Print_To_PrintDocument(e, Trim(Type1), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Type2), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Type3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Type4), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        ' Common_Procedures.Print_To_PrintDocument(e, Trim(Type5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)



        Common_Procedures.Print_To_PrintDocument(e, "PICK", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WIDTH", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, 2, ClAr(12), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "REMARKS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12) + 5, CurY, 2, ClAr(13), pFont)


        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "(PCS)", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "MTR", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, 2, ClAr(12), pFont)


        Common_Procedures.Print_To_PrintDocument(e, Trim(Type11), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Type22), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Type33), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Type44), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        '  Common_Procedures.Print_To_PrintDocument(e, Trim(Type55), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)



        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY




    End Sub

    Private Sub Printing_Format2_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Bilno As String = "", BilDt As String = ""
        Dim reccode As String


        Dim w1 As Single

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        reccode = Trim(Val(lbl_Company.Tag)) & "-" & prn_HdDt.Rows(0).Item("Piece_Receipt_No").ToString & "/" & Trim(Common_Procedures.FnYearCode)

        da3 = New SqlClient.SqlDataAdapter("select b.weaver_wages_no, b.weaver_wages_date from weaver_cloth_receipt_head a INNER JOIN weaver_wages_head b ON A.WEAVER_WAGES_CODE = B.WEAVER_WAGes_code where a.weaver_clothreceipt_code = '" & Trim(reccode) & "'", con)
        prn_DetDt = New DataTable
        da3.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            BilDt = Format(Convert.ToDateTime(Dt1.Rows(0).Item("Weaver_Wages_Date").ToString), "dd-MM-yyyy").ToString
            Bilno = Dt1.Rows(0).Item("Weaver_Wages_No").ToString
        End If


        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), LnAr(3))

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) + ClAr(12), LnAr(3))

        CurY = CurY + 10

        Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2), CurY, 0, 0, pFont)


        vTotType1 = Val(prn_HdDt.Rows(0).Item("Total_Type1_Meters").ToString)
        vTotType2 = Val(prn_HdDt.Rows(0).Item("Total_Type2_Meters").ToString)
        vTotType3 = Val(prn_HdDt.Rows(0).Item("Total_Type3_Meters").ToString)
        vTotType4 = Val(prn_HdDt.Rows(0).Item("Total_Type4_Meters").ToString)
        vTotType5 = Val(prn_HdDt.Rows(0).Item("Total_Type5_Meters").ToString)

        Common_Procedures.Print_To_PrintDocument(e, Format(Val(vTotType1), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(vTotType2), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(vTotType3), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(vTotType4), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(vTotType5), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY)
        LnAr(5) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(4))

        CurY = CurY + 10

        w1 = e.Graphics.MeasureString("RECEIVED ", pFont).Width
        Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + w1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Checking_Meters").ToString), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "BILL NO   :   " & Bilno, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt + 10
        Common_Procedures.Print_To_PrintDocument(e, "RECEIVED ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + w1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("ReceiptMeters_Receipt").ToString), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "DATE   :    " & BilDt, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY)


        CurY = CurY + 10


        Common_Procedures.Print_To_PrintDocument(e, "EXCESS METERS ", LMargin + 10, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Excess_Short_Meter").ToString), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(5))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5))


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub Printing_Format3(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        Dim p1Font As Font
        Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim I As Integer
        Dim NoofItems_PerPage As Integer, vActNoofItms_PerPg As Integer, NoofDets As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(18) As Single, ClAr(15) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim C1 As Single, C2 As Single
        Dim ItmNm1 As String, ItmNm2 As String, ItmNm3 As String


LABEL_PAGETOP_1:

        p1Font = New Font("Calibri", 10, FontStyle.Bold)

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
            .Bottom = 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 10, FontStyle.Regular)
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

        Erase LnAr
        Erase ClAr

        LnAr = New Single(18) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClAr(1) = 35 : ClAr(2) = 55 : ClAr(3) = 70 : ClAr(4) = 75 : ClAr(5) = 75 : ClAr(6) = 70 : ClAr(7) = 65 : ClAr(8) = 60 : ClAr(9) = 80
        ClAr(10) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9))

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7)
        C2 = C1 + ClAr(8)

        TxtHgt = e.Graphics.MeasureString("A", pFont).Height
        TxtHgt = 16.5

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        vActNoofItms_PerPg = 28 '15
        NoofItems_PerPage = vActNoofItms_PerPg

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format3_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    If (prn_DetDt.Rows.Count - 1 - prn_DetIndx) > (vActNoofItms_PerPg + 3) Then
                        NoofItems_PerPage = vActNoofItms_PerPg + 3
                    Else
                        NoofItems_PerPage = vActNoofItms_PerPg
                    End If

                    CurY = CurY + 8

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Or CurY > 1100 Then

                            If vPrntOnly_PageNo = 0 Or prn_PageNo = vPrntOnly_PageNo Then
                                e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
                                e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

                                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), LnAr(5), LMargin + ClAr(1), LnAr(3))
                                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), LnAr(5), LMargin + ClAr(1) + ClAr(2), LnAr(3))
                                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
                                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
                                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
                                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
                                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
                                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
                                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))
                                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(3))
                            End If


                            CurY = CurY + TxtHgt

                            p1Font = New Font("Calibri", 12, FontStyle.Bold)
                            If vPrntOnly_PageNo = 0 Or prn_PageNo = vPrntOnly_PageNo Then Print_To_PrintDoc_BRT(e, "Continued....", PageWidth - 10, CurY, 1, 0, p1Font, prn_PageNo)

                            NoofDets = NoofDets + 1


                            If vPrntOnly_PageNo = 0 Or prn_PageNo = vPrntOnly_PageNo Then
                                CurY = CurY + TxtHgt
                                Print_To_PrintDoc_BRT(e, "Page : " & Trim(Val(prn_PageNo)), LMargin, CurY, 2, PrintWidth, pFont, prn_PageNo)
                            End If

                            'Printing_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            If vPrntOnly_PageNo <> 0 Then
                                If vPrntOnly_PageNo > prn_PageNo Then
                                    GoTo LABEL_PAGETOP_1
                                Else
                                    e.HasMorePages = False
                                End If

                            Else
                                e.HasMorePages = True

                            End If

                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1


                        vType1 = Val(prn_DetDt.Rows(prn_DetIndx).Item("Type1_Meters").ToString) ' sounds
                        vType2 = Val(prn_DetDt.Rows(prn_DetIndx).Item("Type2_Meters").ToString) ' second
                        vType3 = Val(prn_DetDt.Rows(prn_DetIndx).Item("Type3_Meters").ToString)
                        vType4 = Val(prn_DetDt.Rows(prn_DetIndx).Item("Type4_Meters").ToString)
                        vType5 = Val(prn_DetDt.Rows(prn_DetIndx).Item("Type5_Meters").ToString)


                        CurY = CurY + TxtHgt - 12

                        If vPrntOnly_PageNo = 0 Or prn_PageNo = vPrntOnly_PageNo Then

                            Print_To_PrintDoc_BRT(e, Val(prn_DetSNo), LMargin + 8, CurY, 0, 0, pFont, prn_PageNo)
                            Print_To_PrintDoc_BRT(e, prn_DetDt.Rows(prn_DetIndx).Item("Piece_No").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont, prn_PageNo)
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Receipt_Meters").ToString) <> 0 Then Print_To_PrintDoc_BRT(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Receipt_Meters").ToString), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont, prn_PageNo)
                            If Val(prn_DetDt.Rows(prn_DetIndx).Item("Total_Checking_Meters").ToString) <> 0 Then Print_To_PrintDoc_BRT(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Total_Checking_Meters").ToString), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont, prn_PageNo)
                            If Val(vType1) <> 0 Then Print_To_PrintDoc_BRT(e, Format(Val(vType1), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont, prn_PageNo)
                            If Val(vType2) <> 0 Then Print_To_PrintDoc_BRT(e, Format(Val(vType2), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont, prn_PageNo)
                            If (Val(vType3) + Val(vType4)) <> 0 Then Print_To_PrintDoc_BRT(e, Format(Val(vType3) + Val(vType4), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont, prn_PageNo)
                            If Val(vType5) <> 0 Then Print_To_PrintDoc_BRT(e, Format(Val(vType5), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont, prn_PageNo)
                            Print_To_PrintDoc_BRT(e, prn_DetDt.Rows(prn_DetIndx).Item("Loom_No").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 10, CurY, 0, 0, pFont, prn_PageNo)


                            ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Remarks").ToString)
                            ItmNm2 = ""
                            If Len(ItmNm1) > 24 Then
                                For I = 24 To 1 Step -1
                                    If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                Next I
                                If I = 0 Then I = 24
                                ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                                ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                            End If

                            p1Font = New Font("Calibri", 8, FontStyle.Regular)

                            Print_To_PrintDoc_BRT(e, Trim(ItmNm1), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + 10, CurY, 0, 0, p1Font, prn_PageNo)

LOOP1:
                            If Trim(ItmNm2) <> "" Then

                                ItmNm3 = ""
                                If Len(ItmNm2) > 24 Then
                                    For I = 24 To 1 Step -1
                                        If Mid$(Trim(ItmNm2), I, 1) = " " Or Mid$(Trim(ItmNm2), I, 1) = "," Or Mid$(Trim(ItmNm2), I, 1) = "." Or Mid$(Trim(ItmNm2), I, 1) = "-" Or Mid$(Trim(ItmNm2), I, 1) = "/" Or Mid$(Trim(ItmNm2), I, 1) = "_" Or Mid$(Trim(ItmNm2), I, 1) = "(" Or Mid$(Trim(ItmNm2), I, 1) = ")" Or Mid$(Trim(ItmNm2), I, 1) = "\" Or Mid$(Trim(ItmNm2), I, 1) = "[" Or Mid$(Trim(ItmNm2), I, 1) = "]" Or Mid$(Trim(ItmNm2), I, 1) = "{" Or Mid$(Trim(ItmNm2), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 24
                                    ItmNm3 = Microsoft.VisualBasic.Right(Trim(ItmNm2), Len(ItmNm2) - I)
                                    ItmNm2 = Microsoft.VisualBasic.Left(Trim(ItmNm2), I - 1)
                                End If


                                CurY = CurY + TxtHgt - 5
                                Print_To_PrintDoc_BRT(e, Trim(ItmNm2), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + 10, CurY, 0, 0, p1Font, prn_PageNo)
                                NoofDets = NoofDets + 1

                                If Trim(ItmNm3) <> "" Then
                                    If Len(ItmNm3) > 24 Then
                                        ItmNm2 = ItmNm3
                                        GoTo LOOP1
                                    End If
                                    CurY = CurY + TxtHgt - 5
                                    Print_To_PrintDoc_BRT(e, Trim(ItmNm3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + 10, CurY, 0, 0, p1Font, prn_PageNo)
                                    NoofDets = NoofDets + 1
                                End If

                            End If

                        End If


                        CurY = CurY + TxtHgt + 4
                        If vPrntOnly_PageNo = 0 Or prn_PageNo = vPrntOnly_PageNo Then e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                        LnAr(5) = CurY

                        NoofDets = NoofDets + 1

                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If



                'is_FOOTR_PRINT_PART1_STS = False


                Printing_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If vPrntOnly_PageNo <> 0 Then
            If vPrntOnly_PageNo > prn_PageNo Then
                GoTo LABEL_PAGETOP_1
            Else
                e.HasMorePages = False
            End If

        Else

            If is_FOOTR_PRINT_FULLY_STS = True Then
                e.HasMorePages = False
            Else
                e.HasMorePages = True
            End If


        End If

    End Sub

    Private Sub Printing_Format3_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim p1Font As Font
        Dim Cmp_Name As String
        Dim Cmp_Add As String
        Dim Cmp_PhNo As String
        Dim Cmp_gst As String
        Dim strHeight As Single
        Dim C1, w1, w2 As Single

        PageNo = PageNo + 1

        CurY = TMargin


        If vPrntOnly_PageNo = 0 Or prn_PageNo = vPrntOnly_PageNo Then e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = ""
        Cmp_Add = ""
        Cmp_PhNo = ""
        Cmp_gst = ""
        Type1 = "" : Type2 = "" : Type3 = "" : Type4 = "" : Type5 = ""
        Type11 = "" : Type22 = "" : Type33 = "" : Type44 = "" : Type55 = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
        If Trim(prn_HdDt.Rows(0).Item("Company_Address4").ToString) <> "" Then
            Cmp_Add = prn_HdDt.Rows(0).Item("Company_Address4").ToString
        ElseIf Trim(prn_HdDt.Rows(0).Item("Company_Address3").ToString) <> "" Then
            Cmp_Add = prn_HdDt.Rows(0).Item("Company_Address3").ToString
        ElseIf Trim(prn_HdDt.Rows(0).Item("Company_Address2").ToString) <> "" Then
            Cmp_Add = prn_HdDt.Rows(0).Item("Company_Address2").ToString
        Else
            Cmp_Add = prn_HdDt.Rows(0).Item("Company_Address1").ToString
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT TEXTTILES (SOMANUR)
            Cmp_PhNo = "PHONE : 73735 32551, 93444 15141"
            Cmp_gst = "GSTIN:   " & Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString)
        Else
            If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then Cmp_PhNo = "PHONE : " & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If




        Type1 = Trim(UCase(Common_Procedures.ClothType.Type1)) : Type2 = Trim(UCase(Common_Procedures.ClothType.Type2)) : Type3 = Trim(UCase(Common_Procedures.ClothType.Type3)) : Type4 = Trim(UCase(Common_Procedures.ClothType.Type4)) : Type5 = Trim(UCase(Common_Procedures.ClothType.Type5))
        'Type1 = "SOUND" : Type2 = "SECONDS" : Type3 = "BITS" : Type4 = "REJECT" : Type5 = "OTHERS"
        Type11 = "MTRS" : Type22 = "MTRS" : Type33 = "MTRS" : Type44 = "MTRS" : Type55 = "MTRS"


        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        If vPrntOnly_PageNo = 0 Or prn_PageNo = vPrntOnly_PageNo Then Print_To_PrintDoc_BRT(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font, PageNo)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        If vPrntOnly_PageNo = 0 Or prn_PageNo = vPrntOnly_PageNo Then Print_To_PrintDoc_BRT(e, Cmp_Add, LMargin, CurY, 2, PrintWidth, pFont, PageNo)

        CurY = CurY + TxtHgt
        If vPrntOnly_PageNo = 0 Or prn_PageNo = vPrntOnly_PageNo Then Print_To_PrintDoc_BRT(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont, PageNo)
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT TEXTTILES (SOMANUR)
            CurY = CurY + TxtHgt
            If vPrntOnly_PageNo = 0 Or prn_PageNo = vPrntOnly_PageNo Then Print_To_PrintDoc_BRT(e, Cmp_gst, LMargin, CurY, 2, PrintWidth, pFont, PageNo)

        End If

        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        If vPrntOnly_PageNo = 0 Or prn_PageNo = vPrntOnly_PageNo Then Print_To_PrintDoc_BRT(e, "FOLDING REPORT", LMargin, CurY, 2, PrintWidth, p1Font, PageNo)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight + 1
        If vPrntOnly_PageNo = 0 Or prn_PageNo = vPrntOnly_PageNo Then e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 20

        w1 = e.Graphics.MeasureString("BRT METERS : ", pFont).Width
        w2 = e.Graphics.MeasureString("RECEIVED MTRS ", pFont).Width


        CurY = CurY + TxtHgt - 10
        If vPrntOnly_PageNo = 0 Or prn_PageNo = vPrntOnly_PageNo Then
            pFont = New Font("Calibri", 10, FontStyle.Regular)
            Print_To_PrintDoc_BRT(e, "IR NO ", LMargin + 10, CurY, 0, 0, pFont, PageNo)
            Print_To_PrintDoc_BRT(e, ":", LMargin + w1 + 10, CurY, 0, 0, pFont, PageNo)
            Print_To_PrintDoc_BRT(e, Val(prn_HdDt.Rows(0).Item("Weaver_Piece_Checking_No").ToString), LMargin + w1 + 30, CurY, 0, 0, pFont, PageNo)

            Print_To_PrintDoc_BRT(e, "LOT NO ", LMargin + C1 + 10, CurY, 0, 0, pFont, PageNo)
            Print_To_PrintDoc_BRT(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont, PageNo)
            Print_To_PrintDoc_BRT(e, prn_HdDt.Rows(0).Item("Piece_Receipt_No").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont, PageNo)
        End If


        CurY = CurY + TxtHgt
        If vPrntOnly_PageNo = 0 Or prn_PageNo = vPrntOnly_PageNo Then
            Print_To_PrintDoc_BRT(e, "PARTY ", LMargin + 10, CurY, 0, 0, pFont, PageNo)
            Print_To_PrintDoc_BRT(e, ":", LMargin + w1 + 10, CurY, 0, 0, pFont, PageNo)
            Print_To_PrintDoc_BRT(e, Trim(prn_HdDt.Rows(0).Item("Ledger_Name").ToString), LMargin + w1 + 30, CurY, 0, 0, pFont, PageNo)

            Print_To_PrintDoc_BRT(e, "ITEM", LMargin + C1 + 10, CurY, 0, 0, pFont, PageNo)
            Print_To_PrintDoc_BRT(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont, PageNo)
            Print_To_PrintDoc_BRT(e, prn_HdDt.Rows(0).Item("Cloth_Name").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont, PageNo)
        End If


        CurY = CurY + TxtHgt
        If vPrntOnly_PageNo = 0 Or prn_PageNo = vPrntOnly_PageNo Then
            Print_To_PrintDoc_BRT(e, "IR DATE", LMargin + 10, CurY, 0, 0, pFont, PageNo)
            Print_To_PrintDoc_BRT(e, ":", LMargin + w1 + 10, CurY, 0, 0, pFont, PageNo)
            Print_To_PrintDoc_BRT(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Weaver_Piece_Checking_Date").ToString), "dd-MM-yyyy").ToString, LMargin + w1 + 30, CurY, 0, 0, pFont, PageNo)

            Print_To_PrintDoc_BRT(e, "PARTY DC.NO", LMargin + C1 + 10, CurY, 0, 0, pFont, PageNo)
            Print_To_PrintDoc_BRT(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont, PageNo)
            Print_To_PrintDoc_BRT(e, prn_HdDt.Rows(0).Item("Party_DcNo").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont, PageNo)

            Print_To_PrintDoc_BRT(e, "DATE :  " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Piece_Receipt_Date").ToString), "dd-MM-yyyy").ToString, PageWidth - 10, CurY, 1, 0, pFont, PageNo)
        End If



        CurY = CurY + TxtHgt
        If vPrntOnly_PageNo = 0 Or prn_PageNo = vPrntOnly_PageNo Then
            Print_To_PrintDoc_BRT(e, "BRT MTRS", LMargin + 10, CurY, 0, 0, pFont, PageNo)
            Print_To_PrintDoc_BRT(e, ":", LMargin + w1 + 10, CurY, 0, 0, pFont, PageNo)
            Print_To_PrintDoc_BRT(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Checking_Meters").ToString), "#######0.00"), LMargin + w1 + 30, CurY, 0, 0, pFont, PageNo)

            Print_To_PrintDoc_BRT(e, "RECEIVED MTRS", LMargin + C1 + 10, CurY, 0, 0, pFont, PageNo)
            Print_To_PrintDoc_BRT(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont, PageNo)
            Print_To_PrintDoc_BRT(e, Format(Val(prn_HdDt.Rows(0).Item("ReceiptMeters_Receipt").ToString), "#######0.00"), LMargin + C1 + w2 + 30, CurY, 0, 0, pFont, PageNo)

            Print_To_PrintDoc_BRT(e, "FOLDING :  " & Format(Val(prn_HdDt.Rows(0).Item("Folding").ToString), "#######0"), PageWidth - 10, CurY, 1, 0, pFont, PageNo)
        End If


        CurY = CurY + TxtHgt + 10
        If vPrntOnly_PageNo = 0 Or prn_PageNo = vPrntOnly_PageNo Then e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        If vPrntOnly_PageNo = 0 Or prn_PageNo = vPrntOnly_PageNo Then e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        CurY = CurY + 10

        If vPrntOnly_PageNo = 0 Or prn_PageNo = vPrntOnly_PageNo Then
            Print_To_PrintDoc_BRT(e, "SL.", LMargin, CurY, 2, ClAr(1), pFont, PageNo)
            Print_To_PrintDoc_BRT(e, "BRT", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont, PageNo)
            Print_To_PrintDoc_BRT(e, "RCPT", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont, PageNo)
            Print_To_PrintDoc_BRT(e, "PINNING", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont, PageNo)
            Print_To_PrintDoc_BRT(e, "A", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont, PageNo)
            Print_To_PrintDoc_BRT(e, "B", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont, PageNo)
            Print_To_PrintDoc_BRT(e, "C", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont, PageNo)
            Print_To_PrintDoc_BRT(e, "BITS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont, PageNo)
            Print_To_PrintDoc_BRT(e, "LOOM", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont, PageNo)
            Print_To_PrintDoc_BRT(e, "REMARKS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont, PageNo)
        End If


        CurY = CurY + TxtHgt
        If vPrntOnly_PageNo = 0 Or prn_PageNo = vPrntOnly_PageNo Then
            Print_To_PrintDoc_BRT(e, "NO.", LMargin, CurY, 2, ClAr(1), pFont, PageNo)
            Print_To_PrintDoc_BRT(e, "PCS NO", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont, PageNo)
            Print_To_PrintDoc_BRT(e, Trim(Type11), LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont, PageNo)
            Print_To_PrintDoc_BRT(e, Trim(Type22), LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont, PageNo)
            Print_To_PrintDoc_BRT(e, "GRADE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont, PageNo)
            Print_To_PrintDoc_BRT(e, "GRADE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont, PageNo)
            Print_To_PrintDoc_BRT(e, "GRADE", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont, PageNo)
            Print_To_PrintDoc_BRT(e, "NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont, PageNo)
        End If

        CurY = CurY + TxtHgt + 10
        If vPrntOnly_PageNo = 0 Or prn_PageNo = vPrntOnly_PageNo Then e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY
        LnAr(5) = CurY

    End Sub

    Private Sub Printing_Format3_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer = 0
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Bilno As String = "", BilDt As String = ""
        Dim reccode As String = ""
        Dim w1 As Single = 0
        Dim vPass_Perc As String = ""
        Dim vRejct_Perc As String = ""
        Dim vUSRNAME As String = ""
        Dim Prnda As New SqlClient.SqlDataAdapter
        Dim Prndt As New DataTable
        Dim p1font As Font
        Dim vSndAmt As Single = 0, vSecAmt As Single = 0
        Dim vBitAmt As Single = 0
        Dim vRjtAmt As Single = 0
        Dim vOtrAmt As Single = 0
        Dim vGrsAmt As Single = 0
        Dim vRndoffamt As String = ""
        Dim vTtAmt As String = ""
        Dim vNtAmt As String = ""
        Dim NewCode As String = ""



        Dim vCloRec_Code As String = ""
        Dim vCloRec_PkCondt As String = ""

        reccode = ""
        vCloRec_Code = prn_HdDt.Rows(0).Item("Piece_Receipt_Code").ToString
        vCloRec_PkCondt = prn_HdDt.Rows(0).Item("Receipt_PkCondition").ToString

        If InStr(1, Trim(UCase(vCloRec_Code)), Trim(UCase(vCloRec_PkCondt))) > 0 Then
            reccode = Trim(vCloRec_Code)
        Else
            reccode = Trim(vCloRec_PkCondt) & Trim(vCloRec_Code)
        End If


        If vPrntOnly_PageNo = 0 Or prn_PageNo = vPrntOnly_PageNo Then
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), LnAr(5), LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), LnAr(5), LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), LnAr(3))
        End If



        vTotType1 = Val(prn_HdDt.Rows(0).Item("Total_Type1_Meters").ToString)
        vTotType2 = Val(prn_HdDt.Rows(0).Item("Total_Type2_Meters").ToString)
        vTotType3 = Val(prn_HdDt.Rows(0).Item("Total_Type3_Meters").ToString)
        vTotType4 = Val(prn_HdDt.Rows(0).Item("Total_Type4_Meters").ToString)
        vTotType5 = Val(prn_HdDt.Rows(0).Item("Total_Type5_Meters").ToString)
        vTotChck = Val(prn_HdDt.Rows(0).Item("Total_Checking_Meters").ToString)

        vTotRecMtr = Val(prn_HdDt.Rows(0).Item("Total_Checking_Receipt_Meters").ToString)

        If is_FOOTR_PRINT_PART1_STS = False Then
            CurY = CurY + 10
            If vPrntOnly_PageNo = 0 Or prn_PageNo = vPrntOnly_PageNo Then
                'Print_To_PrintDoc_BRT(e, "TOTAL", LMargin + ClAr(1) + ClAr(2), CurY, 0, 0, pFont, prn_PageNo)

                Print_To_PrintDoc_BRT(e, "TOTAL", LMargin + ClAr(1) - 20, CurY, 0, 0, pFont, prn_PageNo)

                Print_To_PrintDoc_BRT(e, Format(Val(vTotRecMtr), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont, prn_PageNo)

                Print_To_PrintDoc_BRT(e, Format(Val(vTotChck), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont, prn_PageNo)
                Print_To_PrintDoc_BRT(e, Format(Val(vTotType1), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont, prn_PageNo)
                Print_To_PrintDoc_BRT(e, Format(Val(vTotType2), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont, prn_PageNo)
                Print_To_PrintDoc_BRT(e, Format(Val(vTotType3) + Val(vTotType4), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont, prn_PageNo)
                Print_To_PrintDoc_BRT(e, Format(Val(vTotType5), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont, prn_PageNo)
            End If

            CurY = CurY + TxtHgt + 10
            If vPrntOnly_PageNo = 0 Or prn_PageNo = vPrntOnly_PageNo Then
                'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY)
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY)
            End If


            LnAr(6) = CurY

            If vPrntOnly_PageNo = 0 Or prn_PageNo = vPrntOnly_PageNo Then
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))
            End If


            Bilno = ""
            BilDt = ""
            da3 = New SqlClient.SqlDataAdapter("select b.weaver_wages_no, b.weaver_wages_date from weaver_cloth_receipt_head a INNER JOIN weaver_wages_head b ON A.WEAVER_WAGES_CODE = B.WEAVER_WAGes_code where a.weaver_clothreceipt_code = '" & Trim(reccode) & "'", con)
            Dt1 = New DataTable
            da3.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                BilDt = Format(Convert.ToDateTime(Dt1.Rows(0).Item("Weaver_Wages_Date").ToString), "dd-MM-yyyy").ToString
                Bilno = Dt1.Rows(0).Item("Weaver_Wages_No").ToString
            End If
            Dt1.Clear()


            CurY = CurY + 10
            vTotType23 = Val(prn_HdDt.Rows(0).Item("Total_Type2_Meters").ToString) + Val(prn_HdDt.Rows(0).Item("Total_Type3_Meters").ToString)

            w1 = e.Graphics.MeasureString("RECEIVED ", pFont, prn_PageNo).Width

            If vPrntOnly_PageNo = 0 Or prn_PageNo = vPrntOnly_PageNo Then
                '  Print_To_PrintDoc_BRT(e, "RECPT MTRS", LMargin + 10, CurY, 0, 0, pFont, prn_PageNo)

                Print_To_PrintDoc_BRT(e, "TOTAL PCS RECEIPT METERS", LMargin + 10, CurY, 0, 0, pFont, prn_PageNo)
                Print_To_PrintDoc_BRT(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Checking_Receipt_Meters").ToString), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont, prn_PageNo)


                Print_To_PrintDoc_BRT(e, "FOLDING MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + (ClAr(8) / 3), CurY, 1, 0, pFont, prn_PageNo)

                Print_To_PrintDoc_BRT(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Checking_Meters").ToString), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont, prn_PageNo)

            End If

            CurY = CurY + TxtHgt + 10

            Dim vExcssMtrs As Single

            vExcssMtrs = Val(prn_HdDt.Rows(0).Item("Total_Checking_Receipt_Meters").ToString) - Val(prn_HdDt.Rows(0).Item("Total_Checking_Meters").ToString)

            If vPrntOnly_PageNo = 0 Or prn_PageNo = vPrntOnly_PageNo Then
                ' Print_To_PrintDoc_BRT(e, "FOLDING MTRS", LMargin + 10, CurY, 0, 0, pFont, prn_PageNo)
                Print_To_PrintDoc_BRT(e, "TOTAL FOLDING METERS", LMargin + 10, CurY, 0, 0, pFont, prn_PageNo)
                Print_To_PrintDoc_BRT(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Checking_Meters").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont, prn_PageNo)


                Print_To_PrintDoc_BRT(e, "PASS MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + (ClAr(8) / 3), CurY, 1, 0, pFont, prn_PageNo)
                Print_To_PrintDoc_BRT(e, Format(Val(vTotType1) + Val(vTotType2), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont, prn_PageNo)

                vPass_Perc = Format((Val(vTotType1) + Val(vTotType2)) / Val(prn_HdDt.Rows(0).Item("Total_Checking_Meters").ToString) * 100, "########0.00")
                Print_To_PrintDoc_BRT(e, Format(Val(vPass_Perc), "########0.00") & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(9) - 10, CurY, 1, 0, pFont, prn_PageNo)
            End If


            CurY = CurY + TxtHgt + 10
            If vPrntOnly_PageNo = 0 Or prn_PageNo = vPrntOnly_PageNo Then
                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY)
            End If

            CurY = CurY + 10
            If vPrntOnly_PageNo = 0 Or prn_PageNo = vPrntOnly_PageNo Then
                Print_To_PrintDoc_BRT(e, "DIFFERENCE MTRS ", LMargin + 10, CurY, 0, 0, pFont, prn_PageNo)
                Print_To_PrintDoc_BRT(e, vExcssMtrs, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont, prn_PageNo)

                Print_To_PrintDoc_BRT(e, "REJECTED MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + (ClAr(8) / 3), CurY, 1, 0, pFont, prn_PageNo)
                Print_To_PrintDoc_BRT(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Type3_Meters").ToString) + Val(prn_HdDt.Rows(0).Item("Total_Type4_Meters").ToString) + Val(prn_HdDt.Rows(0).Item("Total_Type5_Meters").ToString), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont, prn_PageNo)

                vRejct_Perc = Format((Val(prn_HdDt.Rows(0).Item("Total_Type3_Meters").ToString) + Val(prn_HdDt.Rows(0).Item("Total_Type4_Meters").ToString) + Val(prn_HdDt.Rows(0).Item("Total_Type5_Meters").ToString)) / Val(prn_HdDt.Rows(0).Item("Total_Checking_Meters").ToString) * 100, "########0.00")
                Print_To_PrintDoc_BRT(e, Format(Val(vRejct_Perc), "########0.00") & "%", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(9) - 10, CurY, 1, 0, pFont, prn_PageNo)


            End If



            If vPrntOnly_PageNo = 0 Or prn_PageNo = vPrntOnly_PageNo Then
                CurY = CurY + TxtHgt + 10
                '   e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(6))
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(6))
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(6))

                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + +ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(6))
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(6))

                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

                e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
                e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)


            End If
            is_FOOTR_PRINT_PART1_STS = True
        End If
        Dim vWeaCloRcpt_WagesCode_FldName As String = ""

        vWeaCloRcpt_WagesCode_FldName = "Weaver_Wages_Code"
        If (Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" And Val(Common_Procedures.FnYearCode) >= 20) Then '---BRT
            vWeaCloRcpt_WagesCode_FldName = "Weaver_IR_Wages_Code"
        End If

        If CurY > 870 Then

            CurY = CurY + TxtHgt

            p1font = New Font("Calibri", 12, FontStyle.Bold)
            If vPrntOnly_PageNo = 0 Or prn_PageNo = vPrntOnly_PageNo Then Print_To_PrintDoc_BRT(e, "Continued....", PageWidth - 10, CurY, 1, 0, p1font, prn_PageNo)

            NoofDets = NoofDets + 1


            If vPrntOnly_PageNo = 0 Or prn_PageNo = vPrntOnly_PageNo Then
                CurY = CurY + TxtHgt
                Print_To_PrintDoc_BRT(e, "Page : " & Trim(Val(prn_PageNo)), LMargin, CurY, 2, PrintWidth, pFont, prn_PageNo)
            End If


            e.HasMorePages = True

            Return


        End If
        If vPrntOnly_PageNo = 0 Or prn_PageNo = vPrntOnly_PageNo Then

            NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Prnda = New SqlClient.SqlDataAdapter("Select a.*, b.*, c.*, d.Cloth_Name, e.Loom_Type from Weaver_Wages_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_Idno LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo = d.Cloth_Idno INNER JOIN Weaver_Cloth_Receipt_Head e ON a.Weaver_Wages_Code = e." & Trim(vWeaCloRcpt_WagesCode_FldName) & " Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & "  and e.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'   Order by a.for_orderby, a.Weaver_Wages_No, a.Weaver_Wages_Code", con)
            'Prnda = New SqlClient.SqlDataAdapter("Select a.*, b.*, c.*, d.Cloth_Name, e.Loom_Type from Weaver_Wages_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_Idno LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo = d.Cloth_Idno INNER JOIN Weaver_Cloth_Receipt_Head e ON a.Weaver_Wages_Code = e." & Trim(vWeaCloRcpt_WagesCode_FldName) & " Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & "  and e.Weaver_Piece_Checking_Code = '" & Trim(reccode) & "'   Order by a.for_orderby, a.Weaver_Wages_No, a.Weaver_Wages_Code", con)
            Prndt = New DataTable
            Prnda.Fill(Prndt)
            If Prndt.Rows.Count > 0 Then
                vSndAmt = Format(Val(Prndt.Rows(0).Item("Sound_Meters").ToString) * Val(Prndt.Rows(0).Item("Sound_cooly").ToString), "##########0.00")
                vSecAmt = Format(Val(Prndt.Rows(0).Item("Seconds_Meters").ToString) * Val(Prndt.Rows(0).Item("Seconds_cooly").ToString), "##########0.00")
                vBitAmt = Format(Val(Prndt.Rows(0).Item("Bits_Meters").ToString) * Val(Prndt.Rows(0).Item("Bits_cooly").ToString), "##########0.00")
                vRjtAmt = Format(Val(Prndt.Rows(0).Item("Reject_Meters").ToString) * Val(Prndt.Rows(0).Item("Reject_cooly").ToString), "##########0.00")
                vOtrAmt = Format(Val(Prndt.Rows(0).Item("Others_Meters").ToString) * Val(Prndt.Rows(0).Item("Others_cooly").ToString), "##########0.00")
                vGrsAmt = Format(Val(vSndAmt) + Val(vSecAmt) + Val(vBitAmt) + Val(vRjtAmt) + Val(vOtrAmt), "##########0.00")
                p1font = New Font("Calibri", 12, FontStyle.Bold)
                CurY = CurY + 20
                Print_To_PrintDoc_BRT(e, "WAGES DETAILS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, 0, p1font, prn_PageNo)
                CurY = CurY + 20
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, PageWidth, CurY)
                LnAr(16) = CurY

                p1font = New Font("Calibri", 10, FontStyle.Bold)
                If Val(Prndt.Rows(0).Item("Sound_Meters").ToString) <> 0 Then

                    Print_To_PrintDoc_BRT(e, Common_Procedures.ClothType.Type1, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, p1font, prn_PageNo) '---Sound   Meters * rate = Amount

                    Print_To_PrintDoc_BRT(e, Prndt.Rows(0).Item("Sound_Meters").ToString, PageWidth - 190, CurY, 1, 0, pFont, prn_PageNo)
                    Print_To_PrintDoc_BRT(e, " X ", PageWidth - 170, CurY, 1, 0, pFont, prn_PageNo)
                    Print_To_PrintDoc_BRT(e, Prndt.Rows(0).Item("Sound_Cooly").ToString, PageWidth - 100, CurY, 1, 0, pFont, prn_PageNo)
                    Print_To_PrintDoc_BRT(e, " = ", PageWidth - 80, CurY, 1, 0, pFont, prn_PageNo)
                    Print_To_PrintDoc_BRT(e, Format(Val(vSndAmt), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont, prn_PageNo)
                End If



                If Val(Prndt.Rows(0).Item("Seconds_Meters").ToString) <> 0 Then
                    CurY = CurY + TxtHgt

                    Print_To_PrintDoc_BRT(e, Common_Procedures.ClothType.Type2, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, p1font, prn_PageNo) '---Sound   Meters * rate = Amount

                    Print_To_PrintDoc_BRT(e, Prndt.Rows(0).Item("Seconds_Meters").ToString, PageWidth - 190, CurY, 1, 0, pFont, prn_PageNo)
                    Print_To_PrintDoc_BRT(e, " X ", PageWidth - 170, CurY, 1, 0, pFont, prn_PageNo)
                    Print_To_PrintDoc_BRT(e, Prndt.Rows(0).Item("Seconds_Cooly").ToString, PageWidth - 100, CurY, 1, 0, pFont, prn_PageNo)
                    Print_To_PrintDoc_BRT(e, " = ", PageWidth - 80, CurY, 1, 0, pFont, prn_PageNo)
                    Print_To_PrintDoc_BRT(e, Format(Val(vSecAmt), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont, prn_PageNo)
                End If
                If Val(Prndt.Rows(0).Item("Bits_Meters").ToString) <> 0 Then
                    CurY = CurY + TxtHgt

                    Print_To_PrintDoc_BRT(e, Common_Procedures.ClothType.Type3, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, p1font, prn_PageNo) '---Sound   Meters * rate = Amount

                    Print_To_PrintDoc_BRT(e, Prndt.Rows(0).Item("Bits_Meters").ToString, PageWidth - 190, CurY, 1, 0, pFont, prn_PageNo)
                    Print_To_PrintDoc_BRT(e, " X ", PageWidth - 170, CurY, 1, 0, pFont, prn_PageNo)
                    Print_To_PrintDoc_BRT(e, Prndt.Rows(0).Item("Bits_Cooly").ToString, PageWidth - 100, CurY, 1, 0, pFont, prn_PageNo)
                    Print_To_PrintDoc_BRT(e, " = ", PageWidth - 80, CurY, 1, 0, pFont, prn_PageNo)
                    Print_To_PrintDoc_BRT(e, Format(Val(vBitAmt), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont, prn_PageNo)
                End If

                If Val(Prndt.Rows(0).Item("Reject_Meters").ToString) <> 0 Then
                    CurY = CurY + TxtHgt

                    Print_To_PrintDoc_BRT(e, Common_Procedures.ClothType.Type4, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, p1font, prn_PageNo) '---Sound   Meters * rate = Amount

                    Print_To_PrintDoc_BRT(e, Prndt.Rows(0).Item("Reject_Meters").ToString, PageWidth - 190, CurY, 1, 0, pFont, prn_PageNo)
                    Print_To_PrintDoc_BRT(e, " X ", PageWidth - 170, CurY, 1, 0, pFont, prn_PageNo)
                    Print_To_PrintDoc_BRT(e, Prndt.Rows(0).Item("Reject_Cooly").ToString, PageWidth - 100, CurY, 1, 0, pFont, prn_PageNo)
                    Print_To_PrintDoc_BRT(e, " = ", PageWidth - 80, CurY, 1, 0, pFont, prn_PageNo)
                    Print_To_PrintDoc_BRT(e, Format(Val(vRjtAmt), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont, prn_PageNo)
                End If

                If Val(Prndt.Rows(0).Item("Others_Meters").ToString) <> 0 Then
                    CurY = CurY + TxtHgt

                    Print_To_PrintDoc_BRT(e, Common_Procedures.ClothType.Type5, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, p1font, prn_PageNo) '---Sound   Meters * rate = Amount

                    Print_To_PrintDoc_BRT(e, Prndt.Rows(0).Item("Others_Meters").ToString, PageWidth - 190, CurY, 1, 0, pFont, prn_PageNo)
                    Print_To_PrintDoc_BRT(e, " X ", PageWidth - 170, CurY, 1, 0, pFont, prn_PageNo)
                    Print_To_PrintDoc_BRT(e, Prndt.Rows(0).Item("Others_Cooly").ToString, PageWidth - 100, CurY, 1, 0, pFont, prn_PageNo)
                    Print_To_PrintDoc_BRT(e, " = ", PageWidth - 80, CurY, 1, 0, pFont, prn_PageNo)
                    Print_To_PrintDoc_BRT(e, Format(Val(vOtrAmt), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont, prn_PageNo)
                End If
                ' 

                CurY = CurY + TxtHgt
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 50, CurY, PageWidth - 10, CurY)
                CurY = CurY - 10

                vRndoffamt = Format(Val(Prndt.Rows(0).Item("Total_cooly").ToString), "#############0") - Format(Val(vGrsAmt), "##########0.00")
                If Val(vRndoffamt) <> 0 Then
                    CurY = CurY + TxtHgt
                    If Val(vRndoffamt) > 0 Then
                        Print_To_PrintDoc_BRT(e, "Round Off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, p1font, prn_PageNo)
                        Print_To_PrintDoc_BRT(e, "(+)", PageWidth - 80, CurY, 1, 0, pFont, prn_PageNo)
                    ElseIf Val(vRndoffamt) < 0 Then
                        Print_To_PrintDoc_BRT(e, "Round Off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, p1font, prn_PageNo)
                        Print_To_PrintDoc_BRT(e, "(-)", PageWidth - 80, CurY, 1, 0, pFont, prn_PageNo)
                    End If
                    Print_To_PrintDoc_BRT(e, Format(Math.Abs(Val(vRndoffamt)), "###########0.00"), PageWidth - 10, CurY, 1, 0, pFont, prn_PageNo)
                End If
                CurY = CurY + TxtHgt

                Print_To_PrintDoc_BRT(e, "Wages Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, p1font, prn_PageNo)


                Print_To_PrintDoc_BRT(e, Prndt.Rows(0).Item("Total_cooly").ToString, PageWidth - 10, CurY, 1, 0, pFont, prn_PageNo)


                If Val(Prndt.Rows(0).Item("Freight_Charge").ToString) <> 0 Then
                    CurY = CurY + TxtHgt
                    Print_To_PrintDoc_BRT(e, "Handling Charges", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, p1font, prn_PageNo)


                    Print_To_PrintDoc_BRT(e, "(-)", PageWidth - 80, CurY, 1, 0, pFont, prn_PageNo)
                    Print_To_PrintDoc_BRT(e, Prndt.Rows(0).Item("Freight_Charge").ToString, PageWidth - 10, CurY, 1, 0, pFont, prn_PageNo)
                End If


                CurY = CurY + TxtHgt
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 50, CurY, PageWidth - 10, CurY)

                If Val(Prndt.Rows(0).Item("Total_Taxable_Amount").ToString) <> 0 Then
                    'CurY = CurY + TxtHgt
                    Print_To_PrintDoc_BRT(e, "Taxable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, p1font, prn_PageNo)

                    Print_To_PrintDoc_BRT(e, Prndt.Rows(0).Item("Total_Taxable_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont, prn_PageNo)
                End If



                If Val(Prndt.Rows(0).Item("CGST_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt

                    Print_To_PrintDoc_BRT(e, "CGST  @  " & Format(Val(Prndt.Rows(0).Item("CGST_Percentage").ToString), "#########0.0") & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, p1font, prn_PageNo)
                    Print_To_PrintDoc_BRT(e, "(+)", PageWidth - 80, CurY, 1, 0, pFont, prn_PageNo)
                    Print_To_PrintDoc_BRT(e, Format(Val(Prndt.Rows(0).Item("CGST_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont, prn_PageNo)
                End If

                If Val(Prndt.Rows(0).Item("SGST_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt
                    p1font = New Font("Calibri", 10, FontStyle.Bold)
                    Print_To_PrintDoc_BRT(e, "SGST  @  " & Format(Val(Prndt.Rows(0).Item("SGST_Percentage").ToString), "#########0.0") & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, p1font, prn_PageNo)
                    Print_To_PrintDoc_BRT(e, "(+)", PageWidth - 80, CurY, 1, 0, pFont, prn_PageNo)
                    Print_To_PrintDoc_BRT(e, Format(Val(Prndt.Rows(0).Item("SGST_Amount").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont, prn_PageNo)
                End If




                If Val(Prndt.Rows(0).Item("Less_Amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt

                    Print_To_PrintDoc_BRT(e, "(Less) Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, p1font, prn_PageNo)



                    Print_To_PrintDoc_BRT(e, "(-)", PageWidth - 80, CurY, 1, 0, pFont, prn_PageNo)
                    Print_To_PrintDoc_BRT(e, Prndt.Rows(0).Item("Less_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont, prn_PageNo)
                End If

                If Val(Prndt.Rows(0).Item("add_amount").ToString) <> 0 Then
                    CurY = CurY + TxtHgt
                    Print_To_PrintDoc_BRT(e, "(Add) Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, p1font, prn_PageNo)
                    Print_To_PrintDoc_BRT(e, "(+)", PageWidth - 80, CurY, 1, 0, pFont, prn_PageNo)
                    Print_To_PrintDoc_BRT(e, Prndt.Rows(0).Item("add_amount").ToString, PageWidth - 10, CurY, 1, 0, pFont, prn_PageNo)
                End If

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1267" Or (Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" And (Trim(UCase(prn_HdDt.Rows(0).Item("Loom_Type").ToString)) = "POWER LOOM" Or Trim(UCase(prn_HdDt.Rows(0).Item("Loom_Type").ToString)) = "POWERLOOM")) Then
                    If Val(Prndt.Rows(0).Item("Tds_Perc_Calc").ToString) <> 0 Then
                        CurY = CurY + TxtHgt
                        Print_To_PrintDoc_BRT(e, "(Less) TDS @ " & Val(Prndt.Rows(0).Item("Tds_Perc").ToString) & " %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, p1font, prn_PageNo)
                        Print_To_PrintDoc_BRT(e, "(-)", PageWidth - 80, CurY, 1, 0, pFont, prn_PageNo)
                        Print_To_PrintDoc_BRT(e, Prndt.Rows(0).Item("Tds_Perc_Calc").ToString, PageWidth - 10, CurY, 1, 0, pFont, prn_PageNo)
                    End If
                End If

                vNtAmt = Format(Val(Prndt.Rows(0).Item("Total_cooly").ToString) + Val(Prndt.Rows(0).Item("CGST_Amount").ToString) + Val(Prndt.Rows(0).Item("SGST_Amount").ToString) - Val(Prndt.Rows(0).Item("Tds_Perc_Calc").ToString) - Val(Prndt.Rows(0).Item("Freight_Charge").ToString) - Val(Prndt.Rows(0).Item("Less_Amount").ToString) + Val(Prndt.Rows(0).Item("add_amount").ToString), "###########0.00")
                vRndoffamt = Format(Val(Prndt.Rows(0).Item("Net_Amount").ToString), "#############0") - Format(Val(vNtAmt), "###########0.00")

                If Val(vRndoffamt) <> 0 Then
                    CurY = CurY + TxtHgt
                    If Val(vRndoffamt) > 0 Then
                        Print_To_PrintDoc_BRT(e, "Round Off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, p1font, prn_PageNo)
                        Print_To_PrintDoc_BRT(e, "(+)", PageWidth - 80, CurY, 1, 0, pFont, prn_PageNo)
                    ElseIf Val(vRndoffamt) < 0 Then
                        Print_To_PrintDoc_BRT(e, "Round Off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, p1font, prn_PageNo)
                        Print_To_PrintDoc_BRT(e, "(-)", PageWidth - 80, CurY, 1, 0, pFont, prn_PageNo)
                    End If
                    Print_To_PrintDoc_BRT(e, Format(Math.Abs(Val(vRndoffamt)), "###########0.00"), PageWidth - 10, CurY, 1, 0, pFont, prn_PageNo)
                End If





                CurY = CurY + TxtHgt
                    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 50, CurY, PageWidth - 10, CurY)
                    CurY = CurY + TxtHgt - 10
                    Print_To_PrintDoc_BRT(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 10, CurY, 0, 0, p1font, prn_PageNo)

                p1font = New Font("Calibri", 11, FontStyle.Bold)


                vTtAmt = Format(Val(Prndt.Rows(0).Item("Assesable_Value").ToString), "##########0.00")
                Print_To_PrintDoc_BRT(e, Common_Procedures.Currency_Format(Val(vTtAmt)), PageWidth - 10, CurY, 1, 0, p1font, prn_PageNo)




                'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" And (Trim(UCase(Prndt.Rows(0).Item("Loom_Type").ToString)) = "AUTO LOOM" Or Trim(UCase(Prndt.Rows(0).Item("Loom_Type").ToString)) = "AUTOLOOM") Then
                '        vTtAmt = Format(Val(Prndt.Rows(0).Item("Assesable_Value").ToString), "##########0.00")
                '        Print_To_PrintDoc_BRT(e, Common_Procedures.Currency_Format(Val(vTtAmt)), PageWidth - 10, CurY, 1, 0, p1font, prn_PageNo)
                '    Else
                '        Print_To_PrintDoc_BRT(e, Common_Procedures.Currency_Format(Val(Prndt.Rows(0).Item("Net_Amount").ToString)), PageWidth - 10, CurY, 1, 0, p1font, prn_PageNo)
                '    End If



                CurY = CurY + TxtHgt
                    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, PageWidth, CurY)

                    e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(16))
                    e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(16))
                End If
                Prndt.Clear()

        End If



        If vPrntOnly_PageNo = 0 Or prn_PageNo = vPrntOnly_PageNo Then

            e.Graphics.DrawImage(DirectCast(Global.Textile.My.Resources.Resources.Digitally_Verified, Drawing.Image), PageWidth - 130, CurY + TxtHgt - 12, 112, 110)

            If Val(prn_HdDt.Rows(0).Item("approvedby_useridno").ToString) <> 0 Then
                vUSRNAME = Trim(UCase(Common_Procedures.User_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("approvedby_useridno").ToString))))
            Else
                vUSRNAME = Trim(UCase(Common_Procedures.User_IdNoToName(con, Common_Procedures.User.IdNo)))
            End If

            CurY = CurY + TxtHgt

            CurY = CurY + TxtHgt
            Print_To_PrintDoc_BRT(e, "Digitally signed by " & Trim(vUSRNAME), PageWidth - 10, CurY, 1, 0, pFont, prn_PageNo)


            CurY = CurY + TxtHgt
            Print_To_PrintDoc_BRT(e, "Date : " & Trim(Format(Now, "dd-MM-yyyy hh:mm tt").ToString), PageWidth - 10, CurY, 1, 0, pFont, prn_PageNo)
            CurY = CurY + TxtHgt
            Print_To_PrintDoc_BRT(e, "Prepared By", LMargin + 25, CurY, 0, 0, pFont, prn_PageNo)

            CurY = CurY + 10
            Print_To_PrintDoc_BRT(e, "Page : " & Trim(Val(prn_PageNo)), LMargin, CurY, 2, PrintWidth, pFont, prn_PageNo)
        End If
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)
        If is_LastPage = True Then
            If vPrntOnly_PageNo = 0 Or prn_PageNo = vPrntOnly_PageNo Then
                CurY = CurY + TxtHgt - 10
                Print_To_PrintDoc_BRT(e, "*This is computer generated report, So sign not required.", LMargin, CurY, 0, 0, pFont, prn_PageNo)
            End If
        End If

        is_FOOTR_PRINT_FULLY_STS = True

    End Sub

    Private Sub Print_To_PrintDoc_BRT(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal PrintText As String, ByVal Xaxis As Decimal, ByVal Yaxis As Decimal, ByVal AlignMent As Integer, ByVal DataWidth As Decimal, ByVal DataFont As Font, ByVal vCur_PageNo As Integer, Optional ByVal BrushColor As Brush = Nothing)
        Dim X As Decimal, Y As Decimal
        Dim strWidth As Decimal, strHeight As Decimal = 0
        Dim vbrushcolor As Brush

        If vPrntOnly_PageNo <> 0 And vCur_PageNo <> vPrntOnly_PageNo Then
            Exit Sub
        End If

        strWidth = e.Graphics.MeasureString(PrintText, DataFont).Width

        If AlignMent = 1 Then
            X = Xaxis - strWidth

        ElseIf AlignMent = 2 Then
            If DataWidth > strWidth Then
                X = Xaxis + (DataWidth - strWidth) / 2
            Else
                X = Xaxis
            End If

        Else
            X = Xaxis

        End If
        Y = Yaxis

        If IsNothing(BrushColor) = False Then
            vbrushcolor = BrushColor
        Else
            vbrushcolor = Brushes.Black
        End If

        e.Graphics.DrawString(PrintText, DataFont, vbrushcolor, X, Y)

    End Sub

    Private Sub btn_Print_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Cancel.Click
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_print_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Print.Click
        pnl_Back.Enabled = True
        pnl_Print.Visible = False
    End Sub

    Private Sub btn_Print_SinglePage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_SinglePage.Click
        vPrntOnly_PageNo = 0
        If opt_SinglePage.Checked = True Then
            vPrntOnly_PageNo = Val(txt_Print_PageNo.Text)
        End If

        If opt_AllPages.Checked = True Then
            btn_print_Close_Click(sender, e)
        Else
            txt_Print_PageNo.SelectAll()
            txt_Print_PageNo.Focus()
        End If

        Printing_CheckingReport()

    End Sub

    Private Sub opt_AllPages_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles opt_AllPages.CheckedChanged
        If opt_AllPages.Checked = True Then
            txt_Print_PageNo.Text = ""
            txt_Print_PageNo.Enabled = False
        Else
            txt_Print_PageNo.Enabled = True
        End If
    End Sub

    Private Sub opt_SinglePage_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles opt_SinglePage.CheckedChanged
        If opt_SinglePage.Checked = True Then
            txt_Print_PageNo.Enabled = True
            txt_Print_PageNo.Focus()
        Else
            txt_Print_PageNo.Enabled = False
        End If

    End Sub

    Private Sub txt_Print_PageNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Print_PageNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            btn_Print_SinglePage_Click(sender, e)
        End If
    End Sub

    Private Sub btn_Close_Print_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Print.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub btn_PDF_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_PDF.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = True
        EMAIL_Status = False
        WHATSAPP_Status = False
        print_record()
    End Sub

    Private Sub btn_BarCodePrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_BarCodePrint_AllPieces.Click
        vBARCDPRNT_PCSNO = ""
        vBARCDPRNT_COLNO = -1

        Common_Procedures.Print_OR_Preview_Status = 1
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT TEXTILES (Somanur)
            Printing_BarCode_Sticker_Format4_DosPrint()
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1608" Then  '---- SAMANTH Textiles (Somanur)
            Printing_BarCode_Sticker_Format5_DosPrint_1608()
        End If

        'Printing_BarCode_Sticker_Format3_DosPrint()

        '///////OLD FORMAT two barcdeo in one sticker
        'Common_Procedures.Print_OR_Preview_Status = 0
        'Printing_BarCode_Sticker()

        '///////OLD FORMAT two barcdeo in one sticker
        '//////Common_Procedures.Print_OR_Preview_Status = 1
        '//////Printing_BarCode_Sticker_Format2_DosPrint()

    End Sub

    Private Sub btn_BarCodePrint_SinglePieces_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_BarCodePrint_SinglePieces.Click
        vBARCDPRNT_PCSNO = ""
        vBARCDPRNT_COLNO = -1

        Try
            If IsNothing(dgv_Details.CurrentCell) Then

                MessageBox.Show("Invalid Piece No Selection", "DOES NOT PRINT BARCODE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub

            End If

            vBARCDPRNT_PCSNO = Trim(dgv_Details.CurrentRow.Cells(0).Value)
            vBARCDPRNT_COLNO = dgv_Details.CurrentCell.ColumnIndex

        Catch ex As Exception
            '-----
        End Try

        If Trim(vBARCDPRNT_PCSNO) <> "" Then

            Common_Procedures.Print_OR_Preview_Status = 1
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT TEXTILES (Somanur)
                Printing_BarCode_Sticker_Format4_DosPrint()
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1608" Then  '---- SAMANTH Textiles (Somanur)
                Printing_BarCode_Sticker_Format5_DosPrint_1608()
            End If

        End If


    End Sub

    Private Sub Printing_BarCode_Sticker()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String

        NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select * from Weaver_Piece_Checking_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'", con)
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

        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 3.25X1.18", 325, 118)
        'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 3.25X1.5", 325, 150)
        PrintDocument2.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument2.DefaultPageSettings.PaperSize = pkCustomSize1

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try
                If Print_PDF_Status = True Then
                    '--This is actual & correct 
                    PrintDocument2.DocumentName = "CheckingReport"
                    PrintDocument2.PrinterSettings.PrinterName = "doPDF v7"
                    PrintDocument2.PrinterSettings.PrintFileName = "c:\CheckingReport.pdf"
                    PrintDocument2.Print()

                Else
                    If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                        PrintDialog1.PrinterSettings = PrintDocument2.PrinterSettings
                        If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                            PrintDocument2.PrinterSettings = PrintDialog1.PrinterSettings
                            PrintDocument2.Print()
                        End If

                    Else
                        PrintDocument2.Print()

                    End If

                End If
            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try


        Else

            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument2

                ppd.WindowState = FormWindowState.Maximized
                ppd.StartPosition = FormStartPosition.CenterScreen
                'ppd.ClientSize = New Size(600, 600)
                ppd.PrintPreviewControl.AutoZoom = True
                ppd.PrintPreviewControl.Zoom = 1.0

                ppd.ShowDialog()

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If

        Print_PDF_Status = False

    End Sub

    Private Sub PrintDocument2_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument2.BeginPrint
        Dim cmd As New SqlClient.SqlCommand
        Dim SQL1 As String
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String

        NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_DetBarCdStkr = 1

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* ,c.* , d.Cloth_Name from Weaver_Piece_Checking_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Cloth_Head d ON d.Cloth_IdNo = a.Cloth_Idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)



            If prn_HdDt.Rows.Count > 0 Then

                SQL1 = "select a.* from Weaver_ClothReceipt_Piece_Details a where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' ORDER BY PieceNo_OrderBy ASC"
                cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
                da2 = New SqlClient.SqlDataAdapter(cmd)
                'da2 = New SqlClient.SqlDataAdapter("select a.* from Weaver_ClothReceipt_Piece_Details a where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' ORDER BY PieceNo_OrderBy ASC", con)
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

    Private Sub PrintDocument2_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument2.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        Printing_BarCode_Sticker_Format1(e)

    End Sub

    Private Sub Printing_BarCode_Sticker_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font, BarFont As Font
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
        Dim CurX As Single
        Dim BrCdX As Single = 20
        Dim BrCdY As Single = 100
        Dim vBarCode As String = ""
        Dim vFldMtrs As String = ""
        Dim ItmNm1 As String, ItmNm2 As String



        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 3.25X1.18", 325, 118)
        PrintDocument2.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument2.DefaultPageSettings.PaperSize = pkCustomSize1
        e.PageSettings.PaperSize = pkCustomSize1

        With PrintDocument2.DefaultPageSettings.Margins
            .Left = 5
            .Right = 2
            .Top = 5 ' 40
            .Bottom = 2
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        With PrintDocument2.DefaultPageSettings.PaperSize
            PrintWidth = .Width - RMargin - LMargin
            PrintHeight = .Height - TMargin - BMargin
            PageWidth = .Width - RMargin
            PageHeight = .Height - BMargin
        End With
        If PrintDocument2.DefaultPageSettings.Landscape = True Then
            With PrintDocument2.DefaultPageSettings.PaperSize
                PrintWidth = .Height - TMargin - BMargin
                PrintHeight = .Width - RMargin - LMargin
                PageWidth = .Height - TMargin
                PageHeight = .Width - RMargin
            End With
        End If

        NoofItems_PerPage = 2

        TxtHgt = 13.5

        EntryCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                NoofDets = 0

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        Do While prn_DetBarCdStkr <= 5

                            vFldMtrs = 0
                            vBarCode = ""
                            If prn_DetBarCdStkr = 1 Then
                                vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type1_Meters").ToString), "##########0.00")
                                vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type1").ToString)
                            ElseIf prn_DetBarCdStkr = 2 Then
                                vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type2_Meters").ToString), "##########0.00")
                                vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type2").ToString)
                            ElseIf prn_DetBarCdStkr = 3 Then
                                vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type3_Meters").ToString), "##########0.00")
                                vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type3").ToString)
                            ElseIf prn_DetBarCdStkr = 4 Then
                                vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type4_Meters").ToString), "##########0.00")
                                vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type4").ToString)
                            ElseIf prn_DetBarCdStkr = 5 Then
                                vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type5_Meters").ToString), "##########0.00")
                                vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type5").ToString)
                            End If

                            If Val(vFldMtrs) <> 0 Then

                                If NoofDets >= NoofItems_PerPage Then
                                    e.HasMorePages = True
                                    Return
                                End If

                                CurY = TMargin

                                CurX = LMargin - 1
                                If NoofDets = 1 Then
                                    CurX = CurX + ((PageWidth + RMargin) \ 2)
                                End If

                                ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Cloth_Name").ToString)

                                ItmNm2 = ""
                                If Len(ItmNm1) > 21 Then
                                    For I = 21 To 1 Step -1
                                        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 21

                                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I + 1)
                                    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                                End If

                                pFont = New Font("Calibri", 9, FontStyle.Bold)
                                Common_Procedures.Print_To_PrintDocument(e, ItmNm1, CurX, CurY, 0, PrintWidth, pFont, , True)
                                If Trim(ItmNm2) <> "" Then
                                    CurY = CurY + TxtHgt - 2
                                    Common_Procedures.Print_To_PrintDocument(e, ItmNm2, CurX + 120, CurY, 1, PrintWidth, pFont, , True)
                                End If

                                pFont = New Font("Calibri", 9, FontStyle.Bold)

                                CurY = CurY + TxtHgt
                                Common_Procedures.Print_To_PrintDocument(e, "L.NO: " & prn_HdDt.Rows(0).Item("Piece_Receipt_No").ToString & "      P.NO: " & prn_DetDt.Rows(prn_DetIndx).Item("Piece_No").ToString, CurX, CurY, 0, PrintWidth, pFont, , True)

                                CurY = CurY + TxtHgt
                                Common_Procedures.Print_To_PrintDocument(e, "METERS : " & vFldMtrs, CurX, CurY, 0, PrintWidth, pFont, , True)



                                vBarCode = "*" & Trim(UCase(vBarCode)) & "*"
                                'BarFont = New Font("Free 3 of 9", 24, FontStyle.Regular)
                                BarFont = New Font("Free 3 of 9", 18, FontStyle.Regular)

                                CurY = CurY + TxtHgt + 5
                                'CurY = CurY + TxtHgt + 2
                                'CurY = CurY + TxtHgt - 2
                                e.Graphics.DrawString(Trim(vBarCode), BarFont, Brushes.Black, CurX, CurY)

                                pFont = New Font("Calibri", 14, FontStyle.Bold)
                                'CurY = CurY + TxtHgt + TxtHgt + 5
                                CurY = CurY + TxtHgt + TxtHgt - 6
                                Common_Procedures.Print_To_PrintDocument(e, Trim(vBarCode), CurX + 5, CurY, 0, PrintWidth, pFont, , True)

                                NoofDets = NoofDets + 1

                            End If

                            prn_DetBarCdStkr = prn_DetBarCdStkr + 1

                        Loop

                        prn_DetBarCdStkr = 1
                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT BARCODE STICKER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Function get_Code128_CheckSum_Character(ByVal DataString As String)


        'Dim DataString As String
        'DataString = {Table1.DonorNumberField}
        'DataString must equal to the data being evaluated for the check digit, 
        'Example: DataString = "W0000 07 123456"
        Dim CorrectData As String = ""
        Dim WeightedSum As Integer
        Dim StringLength As Integer
        Dim AscValue As Integer
        Dim CharValue As Integer
        Dim CheckDigitAscVal As Integer
        Dim CheckDigitAsc As Integer
        Dim StringLen As Integer
        Dim I As Integer

        CorrectData = ""
        StringLen = Len(DataString)
        For I = 1 To StringLen
            AscValue = Asc(Mid(DataString, I, 1))
            If AscValue < 58 And AscValue > 47 Then CorrectData = CorrectData & Mid(DataString, I, 1) '0-9
            If AscValue < 91 And AscValue > 64 Then CorrectData = CorrectData & Mid(DataString, I, 1) 'A-Z
        Next I
        DataString = CorrectData
        CorrectData = ""
        WeightedSum = 0
        StringLength = Len(DataString)
        For I = 1 To StringLength
            AscValue = Asc(Mid(DataString, I, 1))
            If AscValue < 58 And AscValue > 47 Then CharValue = AscValue - 48
            '0-9 = values 
            If AscValue < 91 And AscValue > 64 Then CharValue = AscValue - 55
            'A-Z = values 10-35
            WeightedSum = ((WeightedSum + CharValue) * 2) Mod 37
        Next I

        CheckDigitAscVal = (38 - WeightedSum) Mod 37
        If CheckDigitAscVal < 10 Then CheckDigitAsc = CheckDigitAscVal + 48 '0-9
        If CheckDigitAscVal < 36 And CheckDigitAscVal > 9 Then CheckDigitAsc = CheckDigitAscVal + 55 'A-Z

        If CheckDigitAscVal = 36 Then CheckDigitAsc = 42

        Return Chr(CheckDigitAsc)

    End Function

    Private Sub Printing_BarCode_Sticker_Format2_DosPrint()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String = ""
        Dim PrnTxt As String = ""
        Dim LnCnt As Integer = 0
        Dim I As Integer = 0
        Dim NoofItems_PerPage As Integer, NoofDets As Integer
        Dim vBarCode As String = ""
        Dim vFldMtrs As String = ""
        Dim ItmNm1 As String, ItmNm2 As String

        NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_DetBarCdStkr = 1

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* ,c.* , d.Cloth_Name from Weaver_Piece_Checking_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Cloth_Head d ON d.Cloth_IdNo = a.Cloth_Idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.* from Weaver_ClothReceipt_Piece_Details a where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' ORDER BY PieceNo_OrderBy ASC", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


        NoofItems_PerPage = 2

        LnCnt = 0

        fs = New FileStream(Common_Procedures.Dos_Printing_FileName_Path, FileMode.Create)
        sw = New StreamWriter(fs, System.Text.Encoding.Default)

        PrnTxt = "<xpml><page quantity='0' pitch='30.0 mm'></xpml>I8,1,001"
        sw.WriteLine(PrnTxt)
        PrnTxt = "ZN"
        sw.WriteLine(PrnTxt)
        PrnTxt = "q660"
        sw.WriteLine(PrnTxt)
        PrnTxt = "O"
        sw.WriteLine(PrnTxt)
        PrnTxt = "*D5T"
        sw.WriteLine(PrnTxt)
        PrnTxt = "JF"
        sw.WriteLine(PrnTxt)
        PrnTxt = "H10"
        sw.WriteLine(PrnTxt)
        PrnTxt = "ZT"
        sw.WriteLine(PrnTxt)
        PrnTxt = "Q240,25"
        sw.WriteLine(PrnTxt)
        PrnTxt = "<xpml></page></xpml><xpml><page quantity='1' pitch='30.0 mm'></xpml>N"
        sw.WriteLine(PrnTxt)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                NoofDets = 0

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        Do While prn_DetBarCdStkr <= 5

                            vFldMtrs = 0
                            vBarCode = ""
                            If prn_DetBarCdStkr = 1 Then
                                vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type1_Meters").ToString), "##########0.00")
                                vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type1").ToString)
                            ElseIf prn_DetBarCdStkr = 2 Then
                                vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type2_Meters").ToString), "##########0.00")
                                vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type2").ToString)
                            ElseIf prn_DetBarCdStkr = 3 Then
                                vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type3_Meters").ToString), "##########0.00")
                                vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type3").ToString)
                            ElseIf prn_DetBarCdStkr = 4 Then
                                vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type4_Meters").ToString), "##########0.00")
                                vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type4").ToString)
                            ElseIf prn_DetBarCdStkr = 5 Then
                                vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type5_Meters").ToString), "##########0.00")
                                vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type5").ToString)
                            End If

                            If Val(vFldMtrs) <> 0 Then

                                If NoofDets >= NoofItems_PerPage Then
                                    PrnTxt = "W1"
                                    sw.WriteLine(PrnTxt)
                                    PrnTxt = "<xpml></page></xpml><xpml><page quantity='1' pitch='30.0 mm'></xpml>N"
                                    sw.WriteLine(PrnTxt)
                                    NoofDets = 0
                                End If


                                'If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString) <> "" Then
                                '    ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString)
                                'Else
                                ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Cloth_Name").ToString)
                                'End If

                                ItmNm2 = ""
                                If Len(ItmNm1) > 16 Then
                                    For I = 16 To 1 Step -1
                                        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 16

                                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I + 1)
                                    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                                End If

                                ItmNm1 = Replace(ItmNm1, """", """""")

                                ItmNm2 = Replace(ItmNm2, """", """""")

                                If NoofDets = 1 Then

                                    PrnTxt = "A324,231,2,4,1,1,N,""Sort:"""
                                    sw.WriteLine(PrnTxt)
                                    PrnTxt = "A324,181,2,4,1,1,N,""Mtrs  :"""
                                    sw.WriteLine(PrnTxt)
                                    PrnTxt = "A323,155,2,4,1,1,N,""Lot No:"""
                                    sw.WriteLine(PrnTxt)
                                    PrnTxt = "A323,124,2,4,1,1,N,""Pcs No:"""
                                    sw.WriteLine(PrnTxt)
                                    PrnTxt = "A206,180,2,4,1,1,N,""" & Trim(vFldMtrs) & """"
                                    sw.WriteLine(PrnTxt)
                                    PrnTxt = "A207,150,2,4,1,1,N,""" & prn_HdDt.Rows(0).Item("Piece_Receipt_No").ToString & """"
                                    sw.WriteLine(PrnTxt)
                                    PrnTxt = "A207,124,2,4,1,1,N,""" & prn_DetDt.Rows(prn_DetIndx).Item("Piece_No").ToString & """"
                                    sw.WriteLine(PrnTxt)
                                    PrnTxt = "B305,98,2,1C,3,6,55,N,""" & Trim(vBarCode) & """"
                                    sw.WriteLine(PrnTxt)
                                    PrnTxt = "A251,37,2,4,1,1,N,""" & Trim(vBarCode) & """"
                                    sw.WriteLine(PrnTxt)
                                    PrnTxt = "A240,231,2,3,1,1,N,""" & Trim(ItmNm1) & """"
                                    sw.WriteLine(PrnTxt)
                                    PrnTxt = "A240,202,2,3,1,1,N,""" & Trim(ItmNm2) & """"
                                    sw.WriteLine(PrnTxt)

                                Else

                                    PrnTxt = "A644,231,2,4,1,1,N,""Sort:"""
                                    sw.WriteLine(PrnTxt)
                                    PrnTxt = "A644,181,2,4,1,1,N,""Mtrs  :"""
                                    sw.WriteLine(PrnTxt)
                                    PrnTxt = "A643,155,2,4,1,1,N,""Lot No:"""
                                    sw.WriteLine(PrnTxt)
                                    PrnTxt = "A643,124,2,4,1,1,N,""Pcs No:"""
                                    sw.WriteLine(PrnTxt)
                                    PrnTxt = "A526,180,2,4,1,1,N,""" & Trim(vFldMtrs) & """"
                                    sw.WriteLine(PrnTxt)
                                    PrnTxt = "A527,150,2,4,1,1,N,""" & prn_HdDt.Rows(0).Item("Piece_Receipt_No").ToString & """"
                                    sw.WriteLine(PrnTxt)
                                    PrnTxt = "A527,124,2,4,1,1,N,""" & prn_DetDt.Rows(prn_DetIndx).Item("Piece_No").ToString & """"
                                    sw.WriteLine(PrnTxt)
                                    PrnTxt = "B625,98,2,1C,3,6,55,N,""" & Trim(vBarCode) & """"
                                    sw.WriteLine(PrnTxt)
                                    PrnTxt = "A571,37,2,4,1,1,N,""" & Trim(vBarCode) & """"
                                    sw.WriteLine(PrnTxt)
                                    PrnTxt = "A560,231,2,3,1,1,N,""" & Trim(ItmNm1) & """"
                                    sw.WriteLine(PrnTxt)
                                    PrnTxt = "A560,202,2,3,1,1,N,""" & Trim(ItmNm2) & """"
                                    sw.WriteLine(PrnTxt)

                                End If

                                NoofDets = NoofDets + 1

                            End If

                            prn_DetBarCdStkr = prn_DetBarCdStkr + 1

                        Loop

                        prn_DetBarCdStkr = 1
                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

            End If

            PrnTxt = "W1"
            sw.WriteLine(PrnTxt)
            PrnTxt = "<xpml></page></xpml><xpml><end/></xpml>"
            sw.WriteLine(PrnTxt)


            If Val(Common_Procedures.Print_OR_Preview_Status) = 2 Then
                Dim p1 As New System.Diagnostics.Process
                p1.EnableRaisingEvents = False
                p1.StartInfo.FileName = Common_Procedures.Dos_PrintPreView_BatchFileName_Path
                p1.StartInfo.WindowStyle = ProcessWindowStyle.Maximized
                p1.Start()
            Else
                Dim p2 As New System.Diagnostics.Process
                p2.EnableRaisingEvents = False
                p2.StartInfo.FileName = Common_Procedures.Dos_Print_BatchFileName_Path
                p2.StartInfo.CreateNoWindow = True
                p2.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                p2.Start()
            End If

            MessageBox.Show("BarCode Sticker Printed", "FOR BARCODE STICKER PRINTING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT BARCODE STICKER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)


        Finally
            sw.Close()
            fs.Close()
            sw.Dispose()
            fs.Dispose()


        End Try

    End Sub


    Private Sub btn_Close_Chk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close_Chk.Click

        'If dgv_Details.Rows.Count > 0 Then
        '    dgv_Details.CurrentRow.Cells(dgvCOL_PCSDETAILS.CHECKERNAME).Value = Trim(cbo_checker.Text)
        '    dgv_Details.CurrentRow.Cells(dgvCOL_PCSDETAILS.FOLDERNAME).Value = Trim(cbo_Folder.Text)
        'End If
        'cbo_checker.Text = ""
        'cbo_Folder.Text = ""

        pnl_Back.Enabled = True
        pnl_CheckingDetails.Visible = False

        'If dgv_Details.Rows.Count > 0 Then
        '    dgv_Details.Focus()
        '    dgv_Details.CurrentCell = dgv_Details.CurrentRow.Cells(dgvCOL_PCSDETAILS.REMARKS) '--to remarks

        'Else
        '    txt_Excess_Short.Focus()

        'End If
        If dgv_Details.Rows.Count > 0 Then
            dgv_Details.Focus()
            dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCOL_PCSDETAILS.RECEIPTMETER)
            dgv_Details.CurrentCell.Selected = True
        Else
            msk_date.Focus()
        End If

    End Sub



    Private Sub GetChecker_details()

        'cbo_checker.Text = ""
        'cbo_Folder.Text = ""
        'If dgv_Details.Rows.Count > 0 Then
        '    cbo_checker.Text = (dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.CHECKERNAME).Value)
        '    cbo_Folder.Text = (dgv_Details.Rows(dgv_Details.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.FOLDERNAME).Value)
        'End If

        pnl_Back.Enabled = False
        pnl_CheckingDetails.BringToFront()
        pnl_CheckingDetails.Visible = True

        If cbo_Folder.Visible And cbo_Folder.Enabled Then cbo_Folder.Focus()

    End Sub

    Private Sub cbo_Folder_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Folder.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Employee_Head", "Employee_Name", "", "(Employee_idno = 0)")

    End Sub

    Private Sub cbo_Folder_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Folder.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Folder, cbo_checker, Nothing, "Employee_Head", "Employee_Name", "", "(Employee_idno = 0)")
        If (e.KeyValue = 40 And cbo_Folder.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            btn_Close_Chk_Click(sender, e)
        End If
    End Sub

    Private Sub cbo_Folder_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Folder.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Folder, Nothing, "Employee_Head", "Employee_Name", "", "(Employee_idno = 0)")
        If Asc(e.KeyChar) = 13 Then
            btn_Close_Chk_Click(sender, e)
        End If

    End Sub

    Private Sub cbo_Folder_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Folder.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New EmployeeCreation_Simple

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Folder.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub btn_ChkDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ChkDetails.Click
        GetChecker_details()
    End Sub

    Private Sub cbo_checker_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_checker.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Employee_Head", "Employee_Name", "", "(Employee_idno = 0)")

    End Sub

    Private Sub cbo_checker_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_checker.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_checker, Nothing, cbo_Folder, "Employee_Head", "Employee_Name", "", "(Employee_idno = 0)")

    End Sub


    Private Sub cbo_checker_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_checker.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_checker, cbo_Folder, "Employee_Head", "Employee_Name", "", "(Employee_idno = 0)")

    End Sub

    Private Sub btn_EMail_Click(sender As Object, e As EventArgs) Handles btn_EMail.Click
        Dim Led_IdNo As Integer
        Dim MailTxt As String

        Try


            Common_Procedures.Print_OR_Preview_Status = 1
            Print_PDF_Status = True
            EMAIL_Status = True
            WHATSAPP_Status = False
            Printing_CheckingReport()

            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Weaver.Text)

            MailTxt = "PIECE CHECKING " & vbCrLf & vbCrLf
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT TEXTILES(SOMANUR)
                MailTxt = MailTxt & "IR No.-" & Trim(lbl_ChkNo.Text) & vbCrLf & "Date-" & Trim(msk_date.Text)
            Else
                MailTxt = MailTxt & "Checking No.-" & Trim(lbl_ChkNo.Text) & vbCrLf & "Date-" & Trim(msk_date.Text)
            End If
            MailTxt = MailTxt & vbCrLf & "Qualitiy-" & Trim(cbo_Quality.Text)
            MailTxt = MailTxt & vbCrLf & "Rec-Meters-" & Trim(txt_Rec_Meter.Text)

            If System.IO.File.Exists(vEMAIL_Attachment_FileName) = True Then
                MailTxt = MailTxt & vbCrLf
                MailTxt = MailTxt & vbCrLf
                MailTxt = MailTxt & "Please find the following attachment(s):"
                MailTxt = MailTxt & "        " & Trim(Path.GetFileName(vEMAIL_Attachment_FileName))
            End If

            EMAIL_Entry.vMailID = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Mail", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT TEXTILES(SOMANUR)
                EMAIL_Entry.vSubJect = "IR : " & Trim(lbl_ChkNo.Text)
            Else
                EMAIL_Entry.vSubJect = "Invocie : " & Trim(lbl_ChkNo.Text)
            End If

            EMAIL_Entry.vMessage = Trim(MailTxt)
            EMAIL_Entry.vAttchFilepath = ""
            If System.IO.File.Exists(vEMAIL_Attachment_FileName) = True Then
                EMAIL_Entry.vAttchFilepath = Trim(vEMAIL_Attachment_FileName)
            End If

            Dim f1 As New EMAIL_Entry
            f1.MdiParent = MDIParent1
            f1.Show()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND MAIL...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub



    Private Sub cbo_checker_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_checker.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New EmployeeCreation_Simple

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_checker.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Weaver_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Weaver.SelectedIndexChanged

    End Sub

    Private Sub btn_SMS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SMS.Click
        Dim i As Integer = 0
        Dim smstxt As String = ""
        Dim PhNo As String = "", EndsCount As String = "", Cloth As String = ""
        Dim Led_IdNo As Integer = 0, Endscount_IdNo As Integer = 0, Cloth_IdNo As Integer = 0
        Dim SMS_SenderID As String = ""
        Dim SMS_Key As String = ""
        Dim SMS_RouteID As String = ""
        Dim SMS_Type As String = ""
        Dim BlNos As String = ""

        Try

            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Weaver.Text)

            PhNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "MobileNo_Frsms", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")

            Cloth_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Quality.Text)
            Cloth = ""
            If Val(Cloth_IdNo) <> 0 Then
                Cloth = Common_Procedures.get_FieldValue(con, "Cloth_Head", "Cloth_name", "(Cloth_IdNo = " & Str(Val(Cloth_IdNo)) & ")")
            End If

            PhNo = Trim(PhNo) & IIf(Trim(PhNo) <> "", "", "")

            smstxt = smstxt & " chk No : " & Trim(lbl_ChkNo.Text) & vbCrLf
            smstxt = smstxt & " Date : " & Trim(msk_date.Text) & vbCrLf
            smstxt = smstxt & " Rec Mtrs: " & Val(txt_Rec_Meter.Text) & vbCrLf
            smstxt = smstxt & " Cloth : " & Trim(Cloth) & vbCrLf
            If dgv_Details_Total2.RowCount > 0 Then

                smstxt = smstxt & " Total  : " & Val(dgv_Details_Total2.Rows(0).Cells(dgvCOL_PCSDETAILS.TOTALMETER).Value()) & vbCrLf

            End If

            smstxt = smstxt & " " & vbCrLf
            smstxt = smstxt & " Thanks! " & vbCrLf
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
            MessageBox.Show(ex.Message, "DOES NOT SEND SMS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub


    Private Sub btn_UserModification_Click(sender As System.Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub

    Private Sub dgtxt_Details_TextChanged(sender As Object, e As System.EventArgs) Handles dgtxt_Details.TextChanged
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

    Private Sub Printing_Format4(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font
        Dim p1Font As Font
        Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim I As Integer
        Dim NoofItems_PerPage As Integer, NoofDets, sno As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim C1 As Single, C2 As Single


        p1Font = New Font("Calibri", 10, FontStyle.Bold)

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
            .Bottom = 30
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



        Erase LnAr
        Erase ClAr


        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}


        'ClAr(1) = 30 : ClAr(2) = 60 : ClAr(3) = 75 : ClAr(4) = 75 : ClAr(5) = 75 : ClAr(6) = 75 : ClAr(7) = 80 : ClAr(8) = 70 : ClAr(9) = 70
        'ClAr(10) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9))

        ClAr(1) = 35 : ClAr(2) = 60 : ClAr(3) = 75 : ClAr(4) = 75 : ClAr(5) = 75 : ClAr(6) = 75 : ClAr(7) = 65 : ClAr(8) = 70 : ClAr(9) = 70 : ClAr(10) = 70
        ClAr(11) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10))


        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7)
        C2 = C1 + ClAr(8)

        TxtHgt = 18

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        NoofItems_PerPage = 15

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format4_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = 0

                CurY = CurY - 10

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format4_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                            e.HasMorePages = True

                            Return

                        End If

                        prn_DetSNo = prn_DetSNo + 1


                        'NoofDets = NoofDets + 1

                        sno = sno + 1
                        vType1 = Val(prn_DetDt.Rows(prn_DetIndx).Item("Type1_Meters").ToString())
                        vType2 = Val(prn_DetDt.Rows(prn_DetIndx).Item("Type2_Meters").ToString())
                        vType3 = Val(prn_DetDt.Rows(prn_DetIndx).Item("Type3_Meters").ToString())
                        vType4 = Val(prn_DetDt.Rows(prn_DetIndx).Item("Type4_Meters").ToString())
                        vType5 = Val(prn_DetDt.Rows(prn_DetIndx).Item("Type5_Meters").ToString())


                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetSNo), LMargin + 15, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Piece_No").ToString, LMargin + ClAr(1) + 10, CurY, 0, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Receipt_Meters").ToString), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(vType1), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(vType2), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(vType3), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)

                        Dim vExcSht As String = 0

                        vExcSht = Val(prn_DetDt.Rows(prn_DetIndx).Item("Total_Checking_Meters").ToString) - Val(prn_DetDt.Rows(prn_DetIndx).Item("Receipt_Meters").ToString)

                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("ExcessShort_Status_YesNo").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 10, CurY, 0, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(vExcSht), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Total_Checking_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), "########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 10, CurY, 1, 0, pFont)

                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight_Meter").ToString), "#######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, pFont)

                        CurY = CurY + TxtHgt + 10

                        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

                        NoofDets = NoofDets + 1

                        prn_DetIndx = prn_DetIndx + 1

                    Loop
                End If

                Printing_Format4_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format4_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim p1Font As Font
        Dim Cmp_Name As String
        Dim strHeight As Single
        Dim C1, w1, w2 As Single

        PageNo = PageNo + 1

        CurY = TMargin

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = ""

        Type1 = "" : Type2 = "" : Type3 = "" : Type4 = "" : Type5 = ""
        Type11 = "" : Type22 = "" : Type33 = "" : Type44 = "" : Type55 = ""

        Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString



        Type1 = Trim(UCase(Common_Procedures.ClothType.Type1)) : Type2 = Trim(UCase(Common_Procedures.ClothType.Type2)) : Type3 = Trim(UCase(Common_Procedures.ClothType.Type3)) : Type4 = Trim(UCase(Common_Procedures.ClothType.Type4)) : Type5 = Trim(UCase(Common_Procedures.ClothType.Type5))
        'Type1 = "SOUND" : Type2 = "SECONDS" : Type3 = "BITS" : Type4 = "REJECT" : Type5 = "OTHERS"
        Type11 = "MTRS" : Type22 = "MTRS" : Type33 = "MTRS" : Type44 = "MTRS" : Type55 = "MTRS"


        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + TxtHgt + 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "FOLDING REPORT", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 20

        w1 = e.Graphics.MeasureString("CHECKING NO.OF PCS  : ", pFont).Width
        w2 = e.Graphics.MeasureString("RECEIVED MTRS ", pFont).Width


        CurY = CurY + TxtHgt - 10
        pFont = New Font("Calibri", 10, FontStyle.Regular)
        Common_Procedures.Print_To_PrintDocument(e, "NAME   :  " & prn_HdDt.Rows(0).Item("Ledger_Name").ToString, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "QUALITY", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Cloth_Name").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "CHECKING DATE", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + w1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Weaver_Piece_Checking_Date").ToString), "dd-MM-yyyy").ToString, LMargin + w1 + 30, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "RECEIVED NO", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Piece_Receipt_No").ToString, LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "DATE :  " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Piece_Receipt_Date").ToString), "dd-MM-yyyy").ToString, PageWidth - 10, CurY, 1, 0, pFont)



        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "CHECKING NO.OF PCS", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + w1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("noof_pcs").ToString), "#######0"), LMargin + w1 + 30, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "FOLDING :  " & Format(Val(prn_HdDt.Rows(0).Item("Folding").ToString), "#######0"), LMargin + C1 - 20, CurY, 1, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "RECEIVED MTRS", LMargin + C1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + w2 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("ReceiptMeters_Receipt").ToString), "#######0.00"), LMargin + C1 + w2 + 30, CurY, 0, 0, pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "PARTY DC.NO :  " & prn_HdDt.Rows(0).Item("Party_DcNo").ToString, PageWidth - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))
        CurY = CurY + 10

        Common_Procedures.Print_To_PrintDocument(e, "SL.", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "PIECE", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "RECD", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Type1), LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Type2), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Type3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "EXC/SHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "EXC/SHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WEIGHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "WGT/", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "No.", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "No.", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Type11), LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Type22), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, Trim(Type33), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "(Y/N)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTR", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)

        'Common_Procedures.Print_To_PrintDocument(e, "NO.", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "NO", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Trim(Type11), LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Trim(Type22), LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(5), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Trim(Type33), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(6), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "/SHT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        ''Common_Procedures.Print_To_PrintDocument(e, Trim(Type55), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        'Common_Procedures.Print_To_PrintDocument(e, "MTR", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

    End Sub

    Private Sub Printing_Format4_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Bilno As String = "", BilDt As String = ""
        Dim reccode As String
        Dim w1 As Single = 0


        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        reccode = Trim(Val(lbl_Company.Tag)) & "-" & prn_HdDt.Rows(0).Item("Piece_Receipt_No").ToString & "/" & Trim(Common_Procedures.FnYearCode)

        da3 = New SqlClient.SqlDataAdapter("select b.weaver_wages_no, b.weaver_wages_date from weaver_cloth_receipt_head a INNER JOIN weaver_wages_head b ON A.WEAVER_WAGES_CODE = B.WEAVER_WAGes_code where a.weaver_clothreceipt_code = '" & Trim(reccode) & "'", con)
        Dt1 = New DataTable
        da3.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            BilDt = Format(Convert.ToDateTime(Dt1.Rows(0).Item("Weaver_Wages_Date").ToString), "dd-MM-yyyy").ToString
            Bilno = Dt1.Rows(0).Item("Weaver_Wages_No").ToString
        End If


        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), LnAr(3))

        CurY = CurY + 10

        Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2), CurY, 0, 0, pFont)


        vTotType1 = Val(prn_HdDt.Rows(0).Item("Total_Type1_Meters").ToString)
        vTotType2 = Val(prn_HdDt.Rows(0).Item("Total_Type2_Meters").ToString)
        vTotType3 = Val(prn_HdDt.Rows(0).Item("Total_Type3_Meters").ToString)
        vTotType4 = Val(prn_HdDt.Rows(0).Item("Total_Type4_Meters").ToString)
        vTotType5 = Val(prn_HdDt.Rows(0).Item("Total_Type5_Meters").ToString)

        Common_Procedures.Print_To_PrintDocument(e, Format(Val(vTotType1), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(vTotType2), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(vTotType3), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_ExcessShort_Details_Meters").ToString), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(vTotType1) + Val(vTotType2) + Val(vTotType3) + Val(vTotType4) + Val(vTotType5), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY)
        LnAr(5) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(4))

        CurY = CurY + 10

        w1 = e.Graphics.MeasureString("RECEIVED ", pFont).Width
        Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + w1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Checking_Meters").ToString), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "BILL NO   :   " & Bilno, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt + 10
        Common_Procedures.Print_To_PrintDocument(e, "RECEIVED ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + w1 + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("ReceiptMeters_Receipt").ToString), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "DATE   :    " & BilDt, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY)


        CurY = CurY + 10


        Common_Procedures.Print_To_PrintDocument(e, "EXCESS METERS ", LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_ExcessShort_Details_Meters").ToString), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Excess_Short_Meter").ToString), "#######0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(5))
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5))


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub

    Private Sub cbo_Grid_RateFor_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_RateFor.GotFocus
        vCbo_ItmNm = Trim(cbo_Grid_RateFor.Text)
    End Sub

    Private Sub cbo_Grid_RateFor_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_RateFor.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, Nothing, Nothing, "", "", "", "")


        With dgv_Details

            If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.TYPE3METER)
            End If

            If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.REMARKS)
            End If

        End With
    End Sub

    Private Sub cbo_Grid_RateFor_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_RateFor.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "", "", "", "")

        If Asc(e.KeyChar) = 13 Then
            With dgv_Details

                .Focus()
                .Rows(.CurrentCell.RowIndex).Cells.Item(dgvCOL_PCSDETAILS.EXCESSHORTSTATUSYESSORNO).Value = Trim(cbo_Grid_RateFor.Text)
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.WEIGHT)

            End With

        End If

    End Sub

    Private Sub cbo_Grid_RateFor_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_RateFor.TextChanged
        Try

            If FrmLdSTS = True Then Exit Sub

            If cbo_Grid_RateFor.Visible Then
                With dgv_Details
                    If Val(cbo_Grid_RateFor.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.EXCESSHORTSTATUSYESSORNO Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_RateFor.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub cbo_Grid_ItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_CountName.GotFocus
        If FrmLdSTS = True Then Exit Sub




        vCbo_ItmNm = Trim(cbo_Grid_CountName.Text)
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Stock_SizedPavu_Processing_Details", "BeamNo_SetCode_forSelection", "(BeamNo_SetCode_forSelection <> '')", "(Reference_Code = '')")
    End Sub

    Private Sub cbo_Grid_ItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_CountName.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_CountName, Nothing, Nothing, "Stock_SizedPavu_Processing_Details", "BeamNo_SetCode_forSelection", "(BeamNo_SetCode_forSelection <> '')", "(Reference_Code = '')")

        With dgv_Details

            If (e.KeyValue = 38 And cbo_Grid_CountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

                .Focus()
                .CurrentCell = dgv_Details.Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.WEIGHT)

            End If

            If (e.KeyValue = 40 And cbo_Grid_CountName.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.REMARKS)

            End If

        End With

    End Sub


    Private Sub cbo_Grid_ItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_CountName.KeyPress


        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_CountName, Nothing, "Stock_SizedPavu_Processing_Details", "BeamNo_SetCode_forSelection", "(BeamNo_SetCode_forSelection <> '')", "(Reference_Code = '')")

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                .Focus()

                .Rows(.CurrentCell.RowIndex).Cells.Item(dgvCOL_PCSDETAILS.BEAMNO_SETCODE_FORSELECTION).Value = Trim(cbo_Grid_CountName.Text)

                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.REMARKS)

            End With

        End If


    End Sub

    Private Sub cbo_Grid_CountName_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_Grid_CountName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Update_BeamNo_SetCode_forSelection_Fields()
        End If
    End Sub


    Private Sub cbo_Grid_CountName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_CountName.TextChanged
        Try

            If FrmLdSTS = True Then Exit Sub

            If cbo_Grid_CountName.Visible Then
                With dgv_Details

                    If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

                    If Val(cbo_Grid_CountName.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.BEAMNO_SETCODE_FORSELECTION Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_CountName.Text)
                    End If

                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub btn_WARP_WEFT_STOCK_UPDATION_Click(sender As Object, e As EventArgs) Handles btn_WARP_WEFT_STOCK_UPDATION.Click
        vWARP_WEFT_STOCK_UPDATION_STATUS = True
        save_record()
        vWARP_WEFT_STOCK_UPDATION_STATUS = False
    End Sub

    Private Sub Update_BeamNo_SetCode_forSelection_Fields()

        Common_Procedures.Update_SizedPavu_BeamNo_for_Selection(con)

        'Dim Cmd As New SqlClient.SqlCommand
        'Cmd.Connection = con
        'Cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set BeamNo_SetCode_forSelection = Beam_No + ' | ' + setcode_forSelection Where Beam_No <> ''"
        'Cmd.ExecuteNonQuery()

    End Sub

    Private Sub dgv_Details_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Details.CellDoubleClick
        On Error Resume Next

        With dgv_Details

            If .Visible Then

                If .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE1METER Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE2METER Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE3METER Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE4METER Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE5METER Then

                    If .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE1METER Then
                        If Trim(.Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE1).Value) <> "" Then
                            MessageBox.Show("BALE No. / DC NO.  : " & Trim(.Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE1).Value), "PIECE DELIVERED STATUS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
                        End If
                    End If

                    If .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE2METER Then
                        If Trim(.Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE2).Value) <> "" Then
                            MessageBox.Show("BALE No. / DC NO.  : " & Trim(.Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE2).Value), "PIECE DELIVERED STATUS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
                        End If
                    End If

                    If .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE3METER Then
                        If Trim(.Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE3).Value) <> "" Then
                            MessageBox.Show("BALE No. / DC NO.  : " & Trim(.Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE3).Value), "PIECE DELIVERED STATUS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
                        End If
                    End If
                    If .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE4METER Then
                        If Trim(.Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE4).Value) <> "" Then
                            MessageBox.Show("BALE No. / DC NO.  : " & Trim(.Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE4).Value), "PIECE DELIVERED STATUS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
                        End If
                    End If
                    If .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.TYPE5METER Then
                        If Trim(.Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE5).Value) <> "" Then
                            MessageBox.Show("BALE No. / DC NO.  : " & Trim(.Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.PACKINGSLIPCODETYPE5).Value), "PIECE DELIVERED STATUS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
                        End If
                    End If

                End If

            End If

        End With

    End Sub

    Private Sub Check_Meter_Range_Condition_for_ClothTYpes(ByVal vRowNo As Integer, Optional ByVal vColNo As Integer = -1)
        Dim j As Integer
        Dim vFROMno As Integer
        Dim vTOno As Integer



        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT TEXTILES(SOMANUR)

            If vColNo <> -1 And (vColNo >= dgvCOL_PCSDETAILS.TYPE1METER And vColNo <= dgvCOL_PCSDETAILS.TYPE5METER) Then
                vFROMno = vColNo
                vTOno = vColNo

            Else
                vFROMno = dgvCOL_PCSDETAILS.TYPE1METER
                vTOno = dgvCOL_PCSDETAILS.TYPE5METER

            End If

            For j = vFROMno To vTOno

                If Val(dgv_Details.Rows(vRowNo).Cells(j).Value) <> 0 Then

                    If j = dgvCOL_PCSDETAILS.TYPE1METER Then

                        If Val(dgv_Details.Rows(vRowNo).Cells(j).Value) < 30 Then
                            MessageBox.Show("Invalid " & Trim(Common_Procedures.ClothType.Type1) & " Meters" & Chr(13) & "Should be greater than or equal to 30", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If dgv_Details.Enabled And dgv_Details.Visible Then
                                dgv_Details.Focus()
                                dgv_Details.CurrentCell = dgv_Details.Rows(vRowNo).Cells(j)
                            End If
                            Exit Sub
                        End If


                    ElseIf j = dgvCOL_PCSDETAILS.TYPE2METER Then

                        If Val(dgv_Details.Rows(vRowNo).Cells(j).Value) < 30 Then
                            MessageBox.Show("Invalid " & Trim(Common_Procedures.ClothType.Type2) & " Meters" & Chr(13) & "Should be greater than or equal to 30", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If dgv_Details.Enabled And dgv_Details.Visible Then
                                dgv_Details.Focus()
                                dgv_Details.CurrentCell = dgv_Details.Rows(vRowNo).Cells(j)
                            End If
                            Exit Sub
                        End If

                    ElseIf j = dgvCOL_PCSDETAILS.TYPE5METER Then

                        If Val(dgv_Details.Rows(vRowNo).Cells(j).Value) >= 30 Then
                            MessageBox.Show("Invalid " & Trim(Common_Procedures.ClothType.Type5) & " Meters" & Chr(13) & "Should be Lesser than 30", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If dgv_Details.Enabled And dgv_Details.Visible Then
                                dgv_Details.Focus()
                                dgv_Details.CurrentCell = dgv_Details.Rows(vRowNo).Cells(j)
                            End If
                            Exit Sub
                        End If

                    End If

                End If

            Next

        End If

    End Sub

    Private Sub cbo_LoomType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_LoomType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub
    Private Sub cbo_LoomType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_LoomType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_LoomType, Nothing, Nothing, "", "", "", "")
        If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            e.Handled = True
            If cbo_Weaver.Visible And cbo_Weaver.Enabled Then
                cbo_Weaver.Focus()
            ElseIf msk_date.Visible And msk_date.Enabled Then
                msk_date.Focus()
            ElseIf dgv_Details.Rows.Count > 1 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            End If

        ElseIf (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            e.Handled = True
            If txt_Folding.Visible And txt_Folding.Enabled Then
                txt_Folding.Focus()
            ElseIf dgv_Details.Rows.Count > 1 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            End If
        End If

    End Sub

    Private Sub cbo_LoomType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_LoomType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_LoomType, Nothing, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            If txt_Folding.Visible And txt_Folding.Enabled Then
                txt_Folding.Focus()
            ElseIf dgv_Details.Rows.Count > 1 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(1)
            End If
        End If
    End Sub
    Private Sub FindRecord()
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String


        Try


            If Trim(txt_ChkNoOpen.Text) = "" And Trim(txt_LotNo_Open.Text) = "" Then
                MessageBox.Show("Invalid Chk/Lot No... ", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            ElseIf Trim(txt_ChkNoOpen.Text) <> "" Then
                InvCode = ""

                inpno = Trim(txt_ChkNoOpen.Text)

                InvCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

                Da = New SqlClient.SqlDataAdapter("select Weaver_Piece_Checking_No from Weaver_Piece_Checking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code = '" & Trim(InvCode) & "'", con)
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
                    MessageBox.Show("Ref No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                End If

            ElseIf Trim(txt_LotNo_Open.Text) <> "" Then
                InvCode = ""

                inpno = Trim(txt_LotNo_Open.Text)

                InvCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

                Da = New SqlClient.SqlDataAdapter("select Weaver_Piece_Checking_No from Weaver_Piece_Checking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Piece_Receipt_No = '" & Trim(inpno) & "' and Weaver_Piece_Checking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
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
                    'MessageBox.Show(Trim(cbo_InOrOutOpen.Text) & " No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                End If

            ElseIf Trim(txt_ChkNoOpen.Text) = "" Or Trim(txt_LotNo_Open.Text) = "" Then
                MessageBox.Show("Invalid Chk/Lot No... ", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try


    End Sub
    Private Sub btn_OpenRecord_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_OpenRecord.Click
        pnl_Back.Enabled = True
        pnl_OpenRecord.Visible = False
        FindRecord()
    End Sub

    Private Sub btn_CloseOpenRecord_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_CloseOpenRecord.Click
        pnl_Back.Enabled = True
        pnl_OpenRecord.Visible = False
    End Sub

    Private Sub txt_LotNo_Open_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_LotNo_Open.KeyPress
        If Asc(e.KeyChar) = 13 Then
            pnl_Back.Enabled = True
            pnl_OpenRecord.Visible = False
            FindRecord()
        End If
    End Sub

    Private Sub txt_ChkNoOpen_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_ChkNoOpen.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If Trim(txt_ChkNoOpen.Text) <> "" Then
                pnl_Back.Enabled = True
                pnl_OpenRecord.Visible = False
                FindRecord()
            Else
                txt_LotNo_Open.Focus()
            End If

        End If
    End Sub

    Private Sub txt_filter_Chkno_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_filter_Chkno.KeyDown
        On Error Resume Next
        If e.KeyValue = 38 Then
            txt_Filter_LotNo.Focus()
        End If 'e.Handled = True : SendKeys.Send("+{TAB}")

        If e.KeyValue = 40 Or (e.Control = True And e.KeyValue = 40) Then
            If Common_Procedures.settings.CustomerCode = "1249" Then
                cbo_Verified_Sts.Focus()
            Else
                btn_Filter_Show.Focus()

            End If

        End If
    End Sub

    Private Sub txt_filter_Chkno_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_filter_Chkno.KeyPress
        On Error Resume Next
        If Asc(e.KeyChar) = 13 Then
            If Common_Procedures.settings.CustomerCode = "1249" Then
                cbo_Verified_Sts.Focus()
            Else
                btn_Filter_Show.Focus()

            End If
        End If

    End Sub

    Private Sub Printing_BarCode_Sticker_Format3_DosPrint()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String = ""
        Dim PrnTxt As String = ""
        Dim LnCnt As Integer = 0
        Dim I As Integer = 0
        Dim NoofItems_PerPage As Integer, NoofDets As Integer
        Dim vBarCode As String = ""
        Dim vFldMtrs As String = ""
        Dim ItmNm1 As String, ItmNm2 As String

        NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_DetBarCdStkr = 1

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* ,c.* , d.Cloth_Name from Weaver_Piece_Checking_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Cloth_Head d ON d.Cloth_IdNo = a.Cloth_Idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.* from Weaver_ClothReceipt_Piece_Details a where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' ORDER BY PieceNo_OrderBy ASC, Piece_No", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


        NoofItems_PerPage = 1

        LnCnt = 0

        fs = New FileStream(Common_Procedures.Dos_Printing_FileName_Path, FileMode.Create)
        sw = New StreamWriter(fs, System.Text.Encoding.Default)


        Try

            If prn_HdDt.Rows.Count > 0 Then

                NoofDets = 0

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        Do While prn_DetBarCdStkr <= 5

                            vFldMtrs = 0
                            vBarCode = ""
                            If prn_DetBarCdStkr = 1 Then
                                vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type1_Meters").ToString), "##########0.00")
                                vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type1").ToString)
                            ElseIf prn_DetBarCdStkr = 2 Then
                                vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type2_Meters").ToString), "##########0.00")
                                vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type2").ToString)
                            ElseIf prn_DetBarCdStkr = 3 Then
                                vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type3_Meters").ToString), "##########0.00")
                                vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type3").ToString)
                            ElseIf prn_DetBarCdStkr = 4 Then
                                vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type4_Meters").ToString), "##########0.00")
                                vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type4").ToString)
                            ElseIf prn_DetBarCdStkr = 5 Then
                                vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type5_Meters").ToString), "##########0.00")
                                vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type5").ToString)
                            End If

                            If Val(vFldMtrs) <> 0 Then

                                'If NoofDets >= NoofItems_PerPage Then
                                '    PrnTxt = "W1"
                                '    sw.WriteLine(PrnTxt)
                                '    PrnTxt = "<xpml></page></xpml><xpml><page quantity='1' pitch='30.0 mm'></xpml>N"
                                '    sw.WriteLine(PrnTxt)
                                '    NoofDets = 0
                                'End If


                                'If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString) <> "" Then
                                '    ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString)
                                'Else
                                ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Cloth_Name").ToString)
                                'End If

                                ItmNm2 = ""
                                If Len(ItmNm1) > 21 Then
                                    For I = 21 To 1 Step -1
                                        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 21

                                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I + 1)
                                    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                                End If

                                ItmNm1 = Replace(ItmNm1, """", """""")
                                ItmNm2 = Replace(ItmNm2, """", """""")

                                PrnTxt = "I8,1,001"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "ZN"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "q580"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "S30"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "O"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "*D5F"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "JF"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "H11"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "ZT"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "Q240,25"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "N"
                                sw.WriteLine(PrnTxt)


                                PrnTxt = "A540,227,2,2,2,2,N,""" & Trim(ItmNm1) & """"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "A540,187,2,2,2,2,N,""" & Trim(ItmNm2) & """"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "A540,146,2,2,2,2,N,""L.NO:"""
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "A540,94,2,2,2,2,N,""P.NO:"""
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "A422,145,2,2,2,2,N,""" & prn_HdDt.Rows(0).Item("Piece_Receipt_No").ToString & """"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "A422,94,2,2,2,2,N,""" & prn_DetDt.Rows(prn_DetIndx).Item("Piece_No").ToString & """"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "A540,54,2,2,2,2,N,""MTRS:"""
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "A422,54,2,2,2,2,N,""" & Trim(Val(vFldMtrs)) & """"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "B279,150,2,1,2,4,96,N,""*" & Trim(vBarCode) & "*"""
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "A256,48,2,1,2,2,N,""*" & Trim(vBarCode) & "*"""
                                sw.WriteLine(PrnTxt)

                                PrnTxt = "W4"
                                sw.WriteLine(PrnTxt)

                                NoofDets = NoofDets + 1

                            End If

                            prn_DetBarCdStkr = prn_DetBarCdStkr + 1

                        Loop

                        prn_DetBarCdStkr = 1
                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

            End If

            sw.Close()
            fs.Close()
            sw.Dispose()
            fs.Dispose()

            If Val(Common_Procedures.Print_OR_Preview_Status) = 2 Then
                Dim p1 As New System.Diagnostics.Process
                p1.EnableRaisingEvents = False
                p1.StartInfo.FileName = Common_Procedures.Dos_PrintPreView_BatchFileName_Path
                p1.StartInfo.WindowStyle = ProcessWindowStyle.Maximized
                p1.Start()

            Else
                Dim p2 As New System.Diagnostics.Process
                p2.EnableRaisingEvents = False
                p2.StartInfo.FileName = Common_Procedures.Dos_Print_BatchFileName_Path
                p2.StartInfo.CreateNoWindow = True
                p2.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                p2.Start()

            End If

            MessageBox.Show("BarCode Sticker Printed", "FOR BARCODE STICKER PRINTING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT BARCODE STICKER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)


        Finally

            Try
                sw.Close()
                fs.Close()
                sw.Dispose()
                fs.Dispose()
            Catch ex As Exception
                '-----

            End Try

        End Try

    End Sub

    Private Sub Printing_BarCode_Sticker_Format4_DosPrint()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String = ""
        Dim PrnTxt As String = ""
        Dim LnCnt As Integer = 0
        Dim I As Integer = 0
        Dim NoofItems_PerPage As Integer, NoofDets As Integer
        Dim vBarCode As String = ""
        Dim vFldMtrs As String = ""
        Dim ItmNm1 As String, ItmNm2 As String
        Dim vPCSNO_CONDT As String
        Dim vBARCDPRNT_STS As Boolean = True

        NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        vPCSNO_CONDT = ""
        If Trim(vBARCDPRNT_PCSNO) <> "" Then
            vPCSNO_CONDT = " and (a.Piece_No = '" & Trim(vBARCDPRNT_PCSNO) & "')"
        End If

        prn_HdDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_DetBarCdStkr = 1

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* ,c.* , d.Cloth_Name, d.Cloth_Description from Weaver_Piece_Checking_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Cloth_Head d ON d.Cloth_IdNo = a.Cloth_Idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                da2 = New SqlClient.SqlDataAdapter("select a.* from Weaver_ClothReceipt_Piece_Details a where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' " & vPCSNO_CONDT & " ORDER BY PieceNo_OrderBy ASC, Piece_No", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


        NoofItems_PerPage = 1

        LnCnt = 0

        fs = New FileStream(Common_Procedures.Dos_Printing_FileName_Path, FileMode.Create)
        sw = New StreamWriter(fs, System.Text.Encoding.Default)


        Try

            If prn_HdDt.Rows.Count > 0 Then

                NoofDets = 0

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        Do While prn_DetBarCdStkr <= 5


                            vFldMtrs = 0
                            vBarCode = ""
                            vBARCDPRNT_STS = True

                            If prn_DetBarCdStkr = 1 Then

                                If Trim(vBARCDPRNT_PCSNO) <> "" Then
                                    vBARCDPRNT_STS = False
                                    If Val(vBARCDPRNT_COLNO) = 0 Or Val(vBARCDPRNT_COLNO) = 5 Or (Val(vBARCDPRNT_COLNO) >= 0 And Val(vBARCDPRNT_COLNO) <> 6 And Val(vBARCDPRNT_COLNO) <> 7 And Val(vBARCDPRNT_COLNO) <> 8 And Val(vBARCDPRNT_COLNO) <> 9) Then
                                        vBARCDPRNT_STS = True
                                    End If
                                End If

                                If vBARCDPRNT_STS = True Then
                                    vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type1_Meters").ToString), "##########0.00")
                                    vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type1").ToString)
                                End If


                            ElseIf prn_DetBarCdStkr = 2 Then
                                If Trim(vBARCDPRNT_PCSNO) <> "" Then
                                    vBARCDPRNT_STS = False
                                    If Val(vBARCDPRNT_COLNO) = 0 Or Val(vBARCDPRNT_COLNO) = 6 Or (Val(vBARCDPRNT_COLNO) >= 0 And Val(vBARCDPRNT_COLNO) <> 5 And Val(vBARCDPRNT_COLNO) <> 7 And Val(vBARCDPRNT_COLNO) <> 8 And Val(vBARCDPRNT_COLNO) <> 9) Then
                                        vBARCDPRNT_STS = True
                                    End If
                                End If

                                If vBARCDPRNT_STS = True Then
                                    vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type2_Meters").ToString), "##########0.00")
                                    vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type2").ToString)
                                End If


                            ElseIf prn_DetBarCdStkr = 3 Then
                                If Trim(vBARCDPRNT_PCSNO) <> "" Then
                                    vBARCDPRNT_STS = False
                                    If Val(vBARCDPRNT_COLNO) = 0 Or Val(vBARCDPRNT_COLNO) = 7 Or (Val(vBARCDPRNT_COLNO) >= 0 And Val(vBARCDPRNT_COLNO) <> 5 And Val(vBARCDPRNT_COLNO) <> 6 And Val(vBARCDPRNT_COLNO) <> 8 And Val(vBARCDPRNT_COLNO) <> 9) Then
                                        vBARCDPRNT_STS = True
                                    End If
                                End If

                                If vBARCDPRNT_STS = True Then
                                    vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type3_Meters").ToString), "##########0.00")
                                    vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type3").ToString)
                                End If


                            ElseIf prn_DetBarCdStkr = 4 Then
                                If Trim(vBARCDPRNT_PCSNO) <> "" Then
                                    vBARCDPRNT_STS = False
                                    If Val(vBARCDPRNT_COLNO) = 0 Or Val(vBARCDPRNT_COLNO) = 8 Or (Val(vBARCDPRNT_COLNO) >= 0 And Val(vBARCDPRNT_COLNO) <> 5 And Val(vBARCDPRNT_COLNO) <> 6 And Val(vBARCDPRNT_COLNO) <> 7 And Val(vBARCDPRNT_COLNO) <> 9) Then
                                        vBARCDPRNT_STS = True
                                    End If
                                End If

                                If vBARCDPRNT_STS = True Then
                                    vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type4_Meters").ToString), "##########0.00")
                                    vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type4").ToString)
                                End If


                            ElseIf prn_DetBarCdStkr = 5 Then
                                If Trim(vBARCDPRNT_PCSNO) <> "" Then
                                    vBARCDPRNT_STS = False
                                    If Val(vBARCDPRNT_COLNO) = 0 Or Val(vBARCDPRNT_COLNO) = 9 Or (Val(vBARCDPRNT_COLNO) >= 0 And Val(vBARCDPRNT_COLNO) <> 5 And Val(vBARCDPRNT_COLNO) <> 6 And Val(vBARCDPRNT_COLNO) <> 7 And Val(vBARCDPRNT_COLNO) <> 8) Then
                                        vBARCDPRNT_STS = True
                                    End If
                                End If

                                If vBARCDPRNT_STS = True Then
                                    vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type5_Meters").ToString), "##########0.00")
                                    vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type5").ToString)
                                End If


                            End If


                            If Val(vFldMtrs) <> 0 Then

                                If Trim(prn_HdDt.Rows(0).Item("Cloth_Description").ToString) <> "" Then
                                    ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Cloth_Description").ToString)
                                Else
                                    ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Cloth_Name").ToString)
                                End If

                                ItmNm2 = ""
                                If Len(ItmNm1) > 21 Then
                                    For I = 21 To 1 Step -1
                                        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 21

                                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I + 1)
                                    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                                End If

                                ItmNm1 = Replace(ItmNm1, """", """""")
                                ItmNm2 = Replace(ItmNm2, """", """""")

                                PrnTxt = "I8,1,001"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "ZN"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "q580"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "S30"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "O"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "*D5F"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "JF"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "H11"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "ZT"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "Q240,25"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "N"
                                sw.WriteLine(PrnTxt)


                                PrnTxt = "A556,227,2,2,2,2,N,""" & Trim(ItmNm1) & """"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "A556,185,2,2,2,2,N,""" & Trim(ItmNm2) & """"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "A567,145,2,2,2,2,N,""L.NO:"""
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "A567,99,2,2,2,2,N,""P.NO:"""
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "A453,139,2,2,2,2,N,""" & prn_HdDt.Rows(0).Item("Piece_Receipt_No").ToString & """"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "A447,99,2,2,2,2,N,""" & prn_DetDt.Rows(prn_DetIndx).Item("Piece_No").ToString & """"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "A564,46,2,2,2,2,N,""MTRS:"""
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "A453,46,2,2,2,2,N,""" & Trim(Val(vFldMtrs)) & """"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "B329,150,2,1,2,4,73,N,""" & Trim(UCase(vBarCode)) & """"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "A326,71,2,1,2,2,N,""" & Trim(UCase(vBarCode)) & """"
                                sw.WriteLine(PrnTxt)

                                PrnTxt = "W1"
                                sw.WriteLine(PrnTxt)

                                NoofDets = NoofDets + 1

                            End If

                            prn_DetBarCdStkr = prn_DetBarCdStkr + 1

                        Loop

                        prn_DetBarCdStkr = 1
                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

            End If

            sw.Close()
            fs.Close()
            sw.Dispose()
            fs.Dispose()

            If Val(Common_Procedures.Print_OR_Preview_Status) = 2 Then
                Dim p1 As New System.Diagnostics.Process
                p1.EnableRaisingEvents = False
                p1.StartInfo.FileName = Common_Procedures.Dos_PrintPreView_BatchFileName_Path
                p1.StartInfo.WindowStyle = ProcessWindowStyle.Maximized
                p1.Start()

            Else
                Dim p2 As New System.Diagnostics.Process
                p2.EnableRaisingEvents = False
                p2.StartInfo.FileName = Common_Procedures.Dos_Print_BatchFileName_Path
                p2.StartInfo.CreateNoWindow = True
                p2.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                p2.Start()

            End If

            MessageBox.Show("BarCode Sticker Printed", "FOR BARCODE STICKER PRINTING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT BARCODE STICKER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)


        Finally

            Try
                sw.Close()
                fs.Close()
                sw.Dispose()
                fs.Dispose()
            Catch ex As Exception
                '-----

            End Try

        End Try

    End Sub

    Private Sub Chk_Approved_Sts_Click(sender As Object, e As EventArgs) Handles Chk_Approved_Sts.Click
        btn_SaveApprovedStatus.Visible = True
    End Sub

    Private Sub Chk_Approved_Sts_CheckedChanged(sender As Object, e As EventArgs) Handles Chk_Approved_Sts.CheckedChanged
        btn_SaveApprovedStatus.Visible = True
    End Sub

    Private Sub btn_SaveApprovedStatus_Click(sender As Object, e As EventArgs) Handles btn_SaveApprovedStatus.Click
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim ConsYARN As String
        Dim ConsPAVU As String
        Dim NewCode As String
        Dim Approved_Sts As Integer
        Dim vAPPRVD_DTTM_TXT As String = ""
        Dim vREF_LOTNos As String = ""

        Try

            NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Approved_Sts = 0
            If Chk_Approved_Sts.Checked = True Then Approved_Sts = 1

            cmd.Connection = con

            cmd.Parameters.Clear()

            vAPPRVD_DTTM_TXT = Trim(Format(Now, "dd-MM-yyyy hh:mm tt"))
            cmd.Parameters.AddWithValue("@approveddatetime", Now)

            If Approved_Sts = 1 Then

                ConsPAVU = 0
                Da = New SqlClient.SqlDataAdapter("Select sum(a.Meters) from Stock_Pavu_Processing_Details a where a.Reference_Code = '" & Trim(lbl_RecPkCondition.Text) & Trim(lbl_RecCode.Text) & "' ", con)
                Dt2 = New DataTable
                Da.Fill(Dt2)
                If Dt2.Rows.Count > 0 Then
                    If IsDBNull(Dt2.Rows(0)(0).ToString) = False Then
                        ConsPAVU = Val(Dt2.Rows(0)(0).ToString)
                    End If
                End If
                Dt2.Clear()

                If Val(ConsPAVU) = 0 Then
                    MessageBox.Show("Invalid Pavu Consumption for this Lot", "FOR SAVING APPROVAL...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If

                ConsYARN = 0
                Da = New SqlClient.SqlDataAdapter("Select sum(a.Weight) from Stock_Yarn_Processing_Details a where a.Reference_Code = '" & Trim(lbl_RecPkCondition.Text) & Trim(lbl_RecCode.Text) & "' ", con)
                Dt2 = New DataTable
                Da.Fill(Dt2)
                If Dt2.Rows.Count > 0 Then
                    If IsDBNull(Dt2.Rows(0)(0).ToString) = False Then
                        ConsYarn = Val(Dt2.Rows(0)(0).ToString)
                    End If
                End If
                Dt2.Clear()

                If Val(ConsYarn) = 0 Then
                    MessageBox.Show("Invalid Weft Yarn Consumption for this Lot", "FOR SAVING APPROVAL...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If


                vREF_LOTNos = ""
                Da = New SqlClient.SqlDataAdapter("Select a.Weaver_ClothReceipt_Code from Weaver_Cloth_Receipt_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Piece_Checking_Code <> '' and 'WCLRC-' + a.Weaver_ClothReceipt_Code NOT IN ( Select z.Reference_Code from Stock_Yarn_Processing_Details z WHERE z.Reference_Code LIKE 'WCLRC-%' )", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1
                        If IsDBNull(Dt1.Rows(i).Item("Weaver_ClothReceipt_Code").ToString) = False Then
                            vREF_LOTNos = Trim(vREF_LOTNos) & IIf(Trim(vREF_LOTNos) <> "", Chr(13), "") & Trim(Dt1.Rows(i).Item("Weaver_ClothReceipt_Code").ToString)
                        End If
                    Next i

                    If Trim(vREF_LOTNos) <> "" Then
                        MessageBox.Show("Invalid WEFT Yarn Consumption for this Following Lot.Nos : " & Chr(13) & Chr(13) & vREF_LOTNos, "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub
                    End If

                End If
                Dt1.Clear()


                vREF_LOTNos = ""
                Da = New SqlClient.SqlDataAdapter("Select a.Weaver_ClothReceipt_Code from Weaver_Cloth_Receipt_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Piece_Checking_Code <> '' and 'WCLRC-' + a.Weaver_ClothReceipt_Code NOT IN ( Select z.Reference_Code from Stock_Pavu_Processing_Details z WHERE z.Reference_Code LIKE 'WCLRC-%' )", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1
                        If IsDBNull(Dt1.Rows(i).Item("Weaver_ClothReceipt_Code").ToString) = False Then
                            vREF_LOTNos = Trim(vREF_LOTNos) & IIf(Trim(vREF_LOTNos) <> "", Chr(13), "") & Trim(Dt1.Rows(i).Item("Weaver_ClothReceipt_Code").ToString)
                        End If
                    Next i

                    If Trim(vREF_LOTNos) <> "" Then
                        MessageBox.Show("Invalid WARP Consumption for this Following Lot.Nos : " & Chr(13) & Chr(13) & vREF_LOTNos, "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub
                    End If

                End If
                Dt1.Clear()

            End If


            cmd.CommandText = "Update Weaver_Piece_Checking_Head set Approved_Status = " & Val(Approved_Sts) & ", approvedby_useridno = " & Str(Val(Common_Procedures.User.IdNo)) & ", approvedby_DateTime = @approveddatetime, approvedby_DateTime_Text = '" & Trim(vAPPRVD_DTTM_TXT) & "'  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            MessageBox.Show("Saved Successfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)


        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR SAVING APPROVAL...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub set_Approved_Status_Visibility()
        Chk_Approved_Sts.Visible = False
        btn_SaveApprovedStatus.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then


            If Val(Common_Procedures.User.IdNo) = 1 Or Trim(Common_Procedures.UR.Weaver_Piece_Checking_Entry_ApprovalStatus) <> "" Then
                Chk_Approved_Sts.Visible = True
                btn_SaveApprovedStatus.Visible = False
                Chk_Approved_Sts.Enabled = True

            Else

                If Chk_Approved_Sts.Checked = True Then Chk_Approved_Sts.Visible = True
                btn_SaveApprovedStatus.Visible = False
                Chk_Approved_Sts.Enabled = False

            End If

        End If


    End Sub

    Private Sub cbo_Weaver_Enter(sender As Object, e As EventArgs) Handles cbo_Weaver.Enter
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER'  or Ledger_Type = 'GODOWN'  or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub Printing_BarCode_Sticker_Format5_DosPrint_1608()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String = ""
        Dim PrnTxt As String = ""
        Dim LnCnt As Integer = 0
        Dim I As Integer = 0
        Dim NoofItems_PerPage As Integer, NoofDets As Integer
        Dim vBarCode As String = ""
        Dim vFldMtrs As String = ""
        Dim vPcMtrs_100cm As String = ""
        Dim vTotMtrs As String = 0
        Dim vTotMtr_100cm As String = 0
        Dim vPcs_No As String = ""
        Dim vPcWgt As String = ""
        Dim vTotWgt As String = 0
        Dim vTot_Wgt_per_Mtr As String = 0
        Dim vWgt_Mtr As String = 0
        Dim vLOOMNO As String = ""
        Dim vFldPerc As String = ""
        Dim ItmNm1 As String, ItmNm2 As String
        Dim vPCSNO_CONDT As String
        Dim vBARCDPRNT_STS As Boolean = True


        NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        vPCSNO_CONDT = ""
        If Trim(vBARCDPRNT_PCSNO) <> "" Then
            vPCSNO_CONDT = " and (a.Piece_No = '" & Trim(vBARCDPRNT_PCSNO) & "')"
        End If

        prn_HdDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_DetBarCdStkr = 1

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* ,c.* , d.Cloth_Name, d.Cloth_Description from Weaver_Piece_Checking_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Ledger_Idno LEFT OUTER JOIN Cloth_Head d ON d.Cloth_IdNo = a.Cloth_Idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)
            If prn_HdDt.Rows.Count > 0 Then
                da2 = New SqlClient.SqlDataAdapter("select a.* from Weaver_ClothReceipt_Piece_Details a where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' " & vPCSNO_CONDT & " ORDER BY PieceNo_OrderBy ASC, Piece_No", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


        NoofItems_PerPage = 1

        LnCnt = 0

        fs = New FileStream(Common_Procedures.Dos_Printing_FileName_Path, FileMode.Create)
        sw = New StreamWriter(fs, System.Text.Encoding.Default)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                NoofDets = 0

                If prn_DetDt.Rows.Count > 0 Then

                    Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                        Do While prn_DetBarCdStkr <= 5


                            vFldMtrs = 0
                            vPcWgt = 0
                            vBarCode = ""
                            vBARCDPRNT_STS = True

                            vTotMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type1_Meters").ToString) + Val(prn_DetDt.Rows(prn_DetIndx).Item("Type2_Meters").ToString) + Val(prn_DetDt.Rows(prn_DetIndx).Item("Type3_Meters").ToString) + Val(prn_DetDt.Rows(prn_DetIndx).Item("Type4_Meters").ToString) + Val(prn_DetDt.Rows(prn_DetIndx).Item("Type5_Meters").ToString), "##########0.00")
                            vTotWgt = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString), "##########0.000")
                            vTot_Wgt_per_Mtr = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight_Meter").ToString), "##########0.000")

                            If prn_DetBarCdStkr = 1 Then

                                If Trim(vBARCDPRNT_PCSNO) <> "" Then
                                    vBARCDPRNT_STS = False
                                    If Val(vBARCDPRNT_COLNO) = 0 Or Val(vBARCDPRNT_COLNO) = 5 Or (Val(vBARCDPRNT_COLNO) >= 0 And Val(vBARCDPRNT_COLNO) <> 6 And Val(vBARCDPRNT_COLNO) <> 7 And Val(vBARCDPRNT_COLNO) <> 8 And Val(vBARCDPRNT_COLNO) <> 9) Then
                                        vBARCDPRNT_STS = True
                                    End If
                                End If

                                If vBARCDPRNT_STS = True Then
                                    vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type1_Meters").ToString), "##########0.00")
                                    vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type1").ToString)
                                End If


                            ElseIf prn_DetBarCdStkr = 2 Then
                                If Trim(vBARCDPRNT_PCSNO) <> "" Then
                                    vBARCDPRNT_STS = False
                                    If Val(vBARCDPRNT_COLNO) = 0 Or Val(vBARCDPRNT_COLNO) = 6 Or (Val(vBARCDPRNT_COLNO) >= 0 And Val(vBARCDPRNT_COLNO) <> 5 And Val(vBARCDPRNT_COLNO) <> 7 And Val(vBARCDPRNT_COLNO) <> 8 And Val(vBARCDPRNT_COLNO) <> 9) Then
                                        vBARCDPRNT_STS = True
                                    End If
                                End If

                                If vBARCDPRNT_STS = True Then
                                    vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type2_Meters").ToString), "##########0.00")
                                    vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type2").ToString)
                                End If


                            ElseIf prn_DetBarCdStkr = 3 Then
                                If Trim(vBARCDPRNT_PCSNO) <> "" Then
                                    vBARCDPRNT_STS = False
                                    If Val(vBARCDPRNT_COLNO) = 0 Or Val(vBARCDPRNT_COLNO) = 7 Or (Val(vBARCDPRNT_COLNO) >= 0 And Val(vBARCDPRNT_COLNO) <> 5 And Val(vBARCDPRNT_COLNO) <> 6 And Val(vBARCDPRNT_COLNO) <> 8 And Val(vBARCDPRNT_COLNO) <> 9) Then
                                        vBARCDPRNT_STS = True
                                    End If
                                End If

                                If vBARCDPRNT_STS = True Then
                                    vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type3_Meters").ToString), "##########0.00")
                                    vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type3").ToString)
                                End If


                            ElseIf prn_DetBarCdStkr = 4 Then
                                If Trim(vBARCDPRNT_PCSNO) <> "" Then
                                    vBARCDPRNT_STS = False
                                    If Val(vBARCDPRNT_COLNO) = 0 Or Val(vBARCDPRNT_COLNO) = 8 Or (Val(vBARCDPRNT_COLNO) >= 0 And Val(vBARCDPRNT_COLNO) <> 5 And Val(vBARCDPRNT_COLNO) <> 6 And Val(vBARCDPRNT_COLNO) <> 7 And Val(vBARCDPRNT_COLNO) <> 9) Then
                                        vBARCDPRNT_STS = True
                                    End If
                                End If

                                If vBARCDPRNT_STS = True Then
                                    vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type4_Meters").ToString), "##########0.00")
                                    vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type4").ToString)
                                End If


                            ElseIf prn_DetBarCdStkr = 5 Then
                                If Trim(vBARCDPRNT_PCSNO) <> "" Then
                                    vBARCDPRNT_STS = False
                                    If Val(vBARCDPRNT_COLNO) = 0 Or Val(vBARCDPRNT_COLNO) = 9 Or (Val(vBARCDPRNT_COLNO) >= 0 And Val(vBARCDPRNT_COLNO) <> 5 And Val(vBARCDPRNT_COLNO) <> 6 And Val(vBARCDPRNT_COLNO) <> 7 And Val(vBARCDPRNT_COLNO) <> 8) Then
                                        vBARCDPRNT_STS = True
                                    End If
                                End If

                                If vBARCDPRNT_STS = True Then
                                    vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type5_Meters").ToString), "##########0.00")
                                    vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type5").ToString)
                                End If


                            End If


                            If Val(vFldMtrs) <> 0 Then

                                vPcs_No = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Piece_No").ToString)

                                vFldPerc = Trim(prn_DetDt.Rows(prn_DetIndx).Item("folding").ToString)
                                If Val(vFldPerc) = 0 Then vFldPerc = 100

                                vLOOMNO = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Loom_No").ToString)

                                vTotMtr_100cm = Format(Val(vTotMtrs) * Val(vFldPerc) / 100, "##########0.00")
                                vPcMtrs_100cm = Format(Val(vFldMtrs) * Val(vFldPerc) / 100, "##########0.00")

                                vWgt_Mtr = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString) / Val(vTotMtr_100cm), "##########0.000")

                                If Val(vWgt_Mtr) = Val(vTot_Wgt_per_Mtr) Then
                                    vPcWgt = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Weight").ToString) * Val(vFldMtrs) / Val(vTotMtrs), "##########0.000")
                                Else
                                    vPcWgt = Format(Val(vTot_Wgt_per_Mtr) * Val(vPcMtrs_100cm), "##########0.000")
                                End If


                                If Trim(prn_HdDt.Rows(0).Item("Cloth_Description").ToString) <> "" Then
                                    ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Cloth_Description").ToString)
                                Else
                                    ItmNm1 = Trim(prn_HdDt.Rows(0).Item("Cloth_Name").ToString)
                                End If

                                ItmNm2 = ""
                                If Len(ItmNm1) > 15 Then
                                    For I = 15 To 1 Step -1
                                        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                                    Next I
                                    If I = 0 Then I = 15
                                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I + 1)
                                    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                                End If

                                ItmNm1 = Replace(ItmNm1, """", "\[""]")
                                ItmNm2 = Replace(ItmNm2, """", "\[""]")

                                PrnTxt = "SIZE 82.5 mm, 40 mm"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "DIRECTION 0,0"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "REFERENCE 0,0"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "OFFSET 0 mm"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "SET PEEL OFF"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "SET CUTTER OFF"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "SET PARTIAL_CUTTER OFF"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "SET TEAR ON"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "CLS"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "BOX 35,9,632,308,3"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "CODEPAGE 1252"
                                sw.WriteLine(PrnTxt)

                                If Trim(vLOOMNO) <> "" Then
                                    PrnTxt = "TEXT 617,287,""ROMAN.TTF"",180,1,14,""LOOM NO"""
                                    sw.WriteLine(PrnTxt)
                                End If
                                PrnTxt = "TEXT 617,242,""ROMAN.TTF"",180,1,14,""PCS NO"""
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "TEXT 617,193,""ROMAN.TTF"",180,1,14,""METER"""
                                sw.WriteLine(PrnTxt)
                                If Val(vPcWgt) <> 0 Then
                                    PrnTxt = "TEXT 617,146,""ROMAN.TTF"",180,1,14,""WEIGHT"""
                                    sw.WriteLine(PrnTxt)
                                End If
                                PrnTxt = "TEXT 618,94,""0"",180,13,14,""CLOTH NAME"""
                                sw.WriteLine(PrnTxt)
                                If Trim(vLOOMNO) <> "" Then
                                    PrnTxt = "TEXT 394,289,""ROMAN.TTF"",180,1,14,"":"""
                                    sw.WriteLine(PrnTxt)
                                End If
                                PrnTxt = "TEXT 394,242,""ROMAN.TTF"",180,1,14,"":"""
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "TEXT 394,196,""ROMAN.TTF"",180,1,14,"":"""
                                sw.WriteLine(PrnTxt)
                                If Val(vPcWgt) <> 0 Then
                                    PrnTxt = "TEXT 394,148,""ROMAN.TTF"",180,1,14,"":"""
                                    sw.WriteLine(PrnTxt)
                                End If
                                PrnTxt = "TEXT 394,100,""ROMAN.TTF"",180,1,14,"":"""
                                sw.WriteLine(PrnTxt)
                                If Trim(vLOOMNO) <> "" Then
                                    PrnTxt = "TEXT 375,286,""ROMAN.TTF"",180,1,14,""" & Trim(vLOOMNO) & """"
                                    sw.WriteLine(PrnTxt)
                                End If
                                PrnTxt = "TEXT 375,240,""ROMAN.TTF"",180,1,14,""" & Trim(vPcs_No) & """"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "TEXT 375,195,""ROMAN.TTF"",180,1,14,""" & Format(Val(vFldMtrs), "#########0.00") & """"
                                sw.WriteLine(PrnTxt)
                                If Val(vPcWgt) <> 0 Then
                                    PrnTxt = "TEXT 375,146,""ROMAN.TTF"",180,1,14,""" & Format(Val(vPcWgt), "#########0.000") & """"
                                    sw.WriteLine(PrnTxt)
                                End If
                                PrnTxt = "QRCODE 189,294,L,6,A,180,M2,S7,""" & Trim(vBarCode) & """"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "TEXT 202,149,""ROMAN.TTF"",180,1,14,""" & Trim(vBarCode) & """"
                                sw.WriteLine(PrnTxt)

                                PrnTxt = "TEXT 375,100,""ROMAN.TTF"",180,1,14,""" & Trim(ItmNm1) & """"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "TEXT 375,54,""ROMAN.TTF"",180,1,14,""" & Trim(ItmNm2) & """"
                                sw.WriteLine(PrnTxt)
                                PrnTxt = "PRINT 1,1"
                                sw.WriteLine(PrnTxt)

                                NoofDets = NoofDets + 1

                            End If

                            prn_DetBarCdStkr = prn_DetBarCdStkr + 1

                        Loop

                        prn_DetBarCdStkr = 1
                        prn_DetIndx = prn_DetIndx + 1

                    Loop

                End If

            End If

            sw.Close()
            fs.Close()
            sw.Dispose()
            fs.Dispose()

            If Val(Common_Procedures.Print_OR_Preview_Status) = 2 Then
                Dim p1 As New System.Diagnostics.Process
                p1.EnableRaisingEvents = False
                p1.StartInfo.FileName = Common_Procedures.Dos_PrintPreView_BatchFileName_Path
                p1.StartInfo.WindowStyle = ProcessWindowStyle.Maximized
                p1.Start()

            Else
                Dim p2 As New System.Diagnostics.Process
                p2.EnableRaisingEvents = False
                p2.StartInfo.FileName = Common_Procedures.Dos_Print_BatchFileName_Path
                p2.StartInfo.CreateNoWindow = True
                p2.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                p2.Start()

            End If

            MessageBox.Show("BarCode Sticker Printed", "FOR BARCODE STICKER PRINTING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT BARCODE STICKER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)


        Finally

            Try
                sw.Close()
                fs.Close()
                sw.Dispose()
                fs.Dispose()
            Catch ex As Exception
                '-----

            End Try

        End Try

    End Sub

End Class