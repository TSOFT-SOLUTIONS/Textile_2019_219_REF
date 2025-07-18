Imports System.IO
Imports System.IO.Ports
Imports System.Threading
Imports System.Globalization
Imports System.Drawing.Printing
Imports Excel = Microsoft.Office.Interop.Excel
Public Class PackingSlip_Direct
    Implements Interface_MDIActions

    Dim DefaultPortNo As String = "4"

    Public Event DataReceived As SerialDataReceivedEventHandler
    'Usage
    Dim instance As SerialPort
    Dim handler As SerialDataReceivedEventHandler

    Enum CommStatus
        ExpectingAuthentication
        ExpectingDriverAuthentication
        ExpectingCardAuthentication
        ExpectingDispensingStatus
        ExpectingDeliveredQuantity
    End Enum

    Dim Status As CommStatus = CommStatus.ExpectingAuthentication

    Public vEntry_BaleGroupIdNo As Integer = 0
    Private vEntry_BaleGroupName As String = ""

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private FrmCls_Sts As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Other_Condition As String = ""
    Private PkCondition_Entry As String = ""
    Private PkCondition_Sample As String = "SPKSL-"
    Private PkCondition_Direct As String = "PASLD-"
    Private PkCondition_BaleGroupWiseEntry As String = "PBG"
    Private PkCondition_RollPacking As String = "RLPCK-"
    Private PkCondition_BaleDirectEntry As String = "BALES-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String
    Private vEntryType As String = ""
    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private Prn_BarcodeSticker As Boolean = False
    Private prn_NoofBmDets As Integer
    Private SaveAll_STS As Boolean = False
    Private LastNo As String = ""

    Private Print_PDF_Status As Boolean = False
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_HdAr(500, 10) As String
    Private prn_DetAr(1000, 100, 10) As String
    Private prn_DetAr1(1000, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_HdMxIndx As Integer
    Private prn_HdIndx As Integer
    Private prn_DetSNo As Integer
    Private prn_Count As Integer = 0
    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1
    Private prn_DetBarCdStkr As Integer

    Private prn_HeadIndx As Integer

    Private PrtOp_sts As Boolean = True
    Private PrtOp_Inc As Integer = 0
    Private PrevMachWgt1 As String = ""
    Private PrevMachWgt2 As String = ""
    Private PrevMachWgt3 As String = ""

    Private vPrnt_WeightColumn_Status As Integer = 0
    Private vtot_pcs As Integer = 0
    Private vtot_wgt As String = 0
    Private lst_prnt As Boolean = False

    Private prn_TotalBales As Integer = 0
    Private prn_TotalPcs As String = ""
    Private prn_TotalMtrs As String = ""
    Private prn_TotalWgt As String = ""
    Private Total_mtrs As Single = 0
    Private Format_2_Status As Integer = 0

    Private prn_meters As String = ""
    Private prn_Pcs As String = ""


    Public Sub New(ByVal EntryType As String)
        vEntryType = Trim(UCase(EntryType))
        FrmLdSTS = True
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub clear()

        NoCalc_Status = True

        Print_PDF_Status = False
        New_Entry = False
        Insert_Entry = False
        Prn_BarcodeSticker = False
        Pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Print.Visible = False
        pnl_OpenRecord.Visible = False

        vmskOldText = ""
        vmskSelStrt = -1
        Lbl_DelvCode.Text = ""

        txt_SlNo.Text = "1"
        txt_LotNo.Text = ""
        txt_PcsNo.Text = ""
        cbo_Type.Text = Common_Procedures.ClothType_IdNoToName(con, 1)
        txt_Meters.Text = ""
        txt_Weight.Text = ""
        lbl_wgtmtr.Text = ""

        chk_Verified_Status.Checked = False

        chk_Stk_Posting_Sts.Checked = False
        chk_Bale_Close_Sts.Checked = False


        lbl_BaleRefNo.Text = ""
        lbl_BaleRefNo.ForeColor = Color.Black

        txt_BalePrefixNo.Text = ""
        txt_BalePrefixNo.Enabled = True
        If Trim(UCase(vEntryType)) = "BALEGROUPWISE" Then
            txt_BalePrefixNo.Text = vEntry_BaleGroupName
            txt_BalePrefixNo.Enabled = False
        End If
        txt_BaleSuffixNo.Text = ""

        msk_date.Text = ""
        dtp_Date.Text = ""
        cbo_PartyName_StockOF.Text = Common_Procedures.Ledger_IdNoToName(con, Val(Common_Procedures.CommonLedger.OwnSort_Ac))
        'cbo_PartyName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(Common_Procedures.CommonLedger.Godown_Ac))
        ' cbo_Cloth.Text = ""
        cbo_ClothType.Text = Common_Procedures.ClothType_IdNoToName(con, 1)
        If PkCondition_Entry = PkCondition_Sample Then
            cbo_Bale_Bundle.Text = "BUNDLE"
        Else
            cbo_Bale_Bundle.Text = "BALE"
        End If

        cbo_Godown_StockIN.Text = Common_Procedures.Ledger_IdNoToName(con, Val(Common_Procedures.CommonLedger.Godown_Ac))
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))

        txt_Folding.Text = 100
        txt_Note.Text = ""

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()


        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_Cloth.Text = ""
            cbo_Filter_ClothType.Text = ""
            cbo_Filter_Cloth.SelectedIndex = -1
            cbo_Filter_ClothType.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()
        End If

        cbo_Cloth.Enabled = True
        cbo_Cloth.BackColor = Color.White

        cbo_ClothType.Enabled = True
        cbo_ClothType.BackColor = Color.White

        txt_Folding.Enabled = True
        txt_Folding.BackColor = Color.White

        txt_net_weight.Text = ""
        txt_gross_weight.Text = ""
        txt_Tare_weight.Text = ""

        cbo_Stamping.Text = ""
        cbo_Transport.Text = ""
        cbo_Despatch_To.Text = ""
        txt_LR_No.Text = ""
        msk_LRDate.Text = ""
        txt_LrNo_Open.Text = ""


        txt_Filter_BaleNo.Text = ""
        txt_Filter_Lr_No.Text = ""
        txt_Filter_Width.Text = ""
        cbo_Filter_InvNo.Text = ""
        cbo_Filter_Stamping.Text = ""


        txt_net_weight.ReadOnly = False

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1530" Then

            cbo_Transport.Visible = True
            cbo_Stamping.Visible = True
            cbo_Despatch_To.Visible = True
            txt_LR_No.Visible = True
            dtp_LR_Date.Visible = True
            msk_LRDate.Visible = True

            lbl_Transport.Visible = True
            lbl_Stamping.Visible = True
            lbl_Despatch_To.Visible = True
            lbl_LR_No.Visible = True

            dgv_Details.Height = 92
            dgv_Details_Total.Top = 253

        Else

            cbo_Transport.Visible = False
            cbo_Stamping.Visible = False
            cbo_Despatch_To.Visible = False
            txt_LR_No.Visible = False
            dtp_LR_Date.Visible = False
            msk_LRDate.Visible = False

            lbl_Transport.Visible = False
            lbl_Stamping.Visible = False
            lbl_Despatch_To.Visible = False
            lbl_LR_No.Visible = False

            dgv_Details.Height = 151
            dgv_Details_Total.Top = 310

        End If


        Grid_Cell_DeSelect()
        NoCalc_Status = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim Msktxbx As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is CheckBox Or TypeOf Me.ActiveControl Is Button Or TypeOf Me.ActiveControl Is MaskedTextBox Then
            Me.ActiveControl.BackColor = Color.SpringGreen
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

        If Me.ActiveControl.Name <> dgv_Details_Total.Name Then
            Grid_DeSelect()
        End If

        Prec_ActCtrl = Me.ActiveControl

    End Sub

    Private Sub ControlLostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Or TypeOf Prec_ActCtrl Is CheckBox Or TypeOf Prec_ActCtrl Is MaskedTextBox Then
                Prec_ActCtrl.BackColor = Color.White
                Prec_ActCtrl.ForeColor = Color.Black
            ElseIf TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.FromArgb(2, 57, 111)
                Prec_ActCtrl.ForeColor = Color.White
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

    Private Sub Packing_Slip_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer
        Dim CompCondt As String

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyName_StockOF.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyName_StockOF.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Cloth.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Cloth.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ClothType.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH TYPE" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ClothType.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Stamping.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "STAMPING" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Stamping.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If


            Common_Procedures.Master_Return.Form_Name = ""
            Common_Procedures.Master_Return.Control_Name = ""
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            If FrmLdSTS = True Then

                lbl_Company.Text = ""
                lbl_Company.Tag = 0
                Common_Procedures.CompIdNo = 0

                Me.Text = ""

                CompCondt = ""
                If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
                    CompCondt = "Company_Type = 'ACCOUNT'"
                End If

                da = New SqlClient.SqlDataAdapter("select count(*) from Company_Head Where " & CompCondt & IIf(Trim(CompCondt) <> "", " and ", "") & " Company_IdNo <> 0", con)
                dt1 = New DataTable
                da.Fill(dt1)

                NoofComps = 0
                If dt1.Rows.Count > 0 Then
                    If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                        NoofComps = Val(dt1.Rows(0)(0).ToString)
                    End If
                End If
                dt1.Clear()

                If Val(NoofComps) = 1 Then

                    da = New SqlClient.SqlDataAdapter("select Company_IdNo, Company_Name from Company_Head Where " & CompCondt & IIf(Trim(CompCondt) <> "", " and ", "") & " Company_IdNo <> 0 Order by Company_IdNo", con)
                    dt1 = New DataTable
                    da.Fill(dt1)

                    If dt1.Rows.Count > 0 Then
                        If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                            Common_Procedures.CompIdNo = Val(dt1.Rows(0)(0).ToString)
                        End If

                    End If
                    dt1.Clear()

                Else

                    Dim f As New Company_Selection
                    f.ShowDialog()

                End If

                If Val(Common_Procedures.CompIdNo) <> 0 Then

                    da = New SqlClient.SqlDataAdapter("select Company_IdNo, Company_Name from Company_Head where Company_IdNo = " & Str(Val(Common_Procedures.CompIdNo)), con)
                    dt1 = New DataTable
                    da.Fill(dt1)

                    If dt1.Rows.Count > 0 Then
                        If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                            lbl_Company.Tag = Val(dt1.Rows(0)(0).ToString)
                            lbl_Company.Text = Trim(dt1.Rows(0)(1).ToString)
                            Me.Text = Trim(dt1.Rows(0)(1).ToString)
                        End If
                    End If
                    dt1.Clear()

                    new_record()

                Else
                    MessageBox.Show("Invalid Company Selection", "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    'Me.Close()
                    Exit Sub

                End If

            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False
    End Sub


    Private Sub Packing_Slip_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        con.Open()

        PkCondition_Entry = ""

        txt_BalePrefixNo.Visible = False
        txt_BaleSuffixNo.Visible = False
        vEntry_BaleGroupName = ""
        If Trim(UCase(vEntryType)) = "SAMPLE" Then
            Label1.Text = "PACKING SLIP (SAMPLE)"
            PkCondition_Entry = PkCondition_Sample
            Other_Condition = "(Packing_Slip_Code LIKE '" & Trim(PkCondition_Sample) & "%')"
            Me.BackColor = Color.LightGray

            lbl_BaleRefNo.Left = cbo_PartyName_StockOF.Left
            lbl_BaleRefNo.Width = cbo_PartyName_StockOF.Width


        ElseIf Trim(UCase(vEntryType)) = "DIRECT" Then
            Label1.Text = "PACKING SLIP (DIRECT)"
            PkCondition_Entry = PkCondition_Direct
            Other_Condition = "(Packing_Slip_Code LIKE '" & Trim(PkCondition_Direct) & "%')"
            Me.BackColor = Color.LightGray


        ElseIf Trim(UCase(vEntryType)) = "BALEGROUPWISE" Then
            Label1.Text = "PACKING SLIP DIRECT (BALEGROUPWISE)"
            PkCondition_Entry = Trim(PkCondition_BaleGroupWiseEntry) & Trim(Format(Val(vEntry_BaleGroupIdNo), "00")) & "-"
            Other_Condition = "(Packing_Slip_Code LIKE '" & Trim(PkCondition_Entry) & "%' and BaleGroup_IdNo = " & Str(Val(vEntry_BaleGroupIdNo)) & " )"
            Me.BackColor = Color.WhiteSmoke
            txt_BalePrefixNo.Enabled = False
            vEntry_BaleGroupName = Common_Procedures.ClothSet_IdNoToName(con, vEntry_BaleGroupIdNo)
            txt_BalePrefixNo.Visible = True
            txt_BaleSuffixNo.Visible = True

        Else

            Label1.Text = "PACKING SLIP"
            PkCondition_Entry = ""
            Other_Condition = "(Packing_Slip_Code NOT LIKE '" & Trim(PkCondition_Sample) & "%' and Packing_Slip_Code NOT LIKE '" & Trim(PkCondition_RollPacking) & "%' and Packing_Slip_Code NOT LIKE '" & Trim(PkCondition_BaleDirectEntry) & "%' and Packing_Slip_Code NOT LIKE '" & Trim(PkCondition_BaleGroupWiseEntry) & "%')"

            lbl_BaleRefNo.Left = cbo_PartyName_StockOF.Left
            lbl_BaleRefNo.Width = cbo_PartyName_StockOF.Width

        End If

        dgv_Details.Columns(1).HeaderText = Trim(UCase(Common_Procedures.settings.ClothReceipt_LotNo_OR_RollNo_Text))

        Me.Text = ""

        cbo_PartyName_StockOF.Visible = False
        lbl_PartyName_StockOF_Caption.Visible = False
        'If Common_Procedures.settings.JOBWORKENTRY_Status = 1 Then
        '    cbo_PartyName_StockOF.Visible = True
        '    lbl_PartyName_StockOF_Caption.Visible = True
        'End If

        cbo_Godown_StockIN.Visible = False
        lbl_Godown_StockIN_Caption.Visible = False
        If Common_Procedures.settings.Multi_Godown_Status = 1 Then
            cbo_Godown_StockIN.Visible = True
            lbl_Godown_StockIN_Caption.Visible = True

            If Common_Procedures.settings.JOBWORKENTRY_Status = 0 Then
                lbl_Godown_StockIN_Caption.Left = lbl_PartyName_StockOF_Caption.Left
                cbo_Godown_StockIN.Left = cbo_PartyName_StockOF.Left
                cbo_Godown_StockIN.Width = cbo_PartyName_StockOF.Width
            End If

        End If


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1307" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1387" Then
            lbl_lotno_caption.Visible = False
            txt_LotNo.Visible = False

            lbl_pcsno_caption.Visible = False
            txt_PcsNo.Visible = False

            lbl_type_caption.Visible = False
            cbo_Type.Visible = False

            lbl_meters_caption.Left = 50
            txt_Meters.Left = 50
            txt_Meters.Width = 150

            lbl_weight_caption.Left = 220
            txt_Weight.Left = 220
            txt_Weight.Width = 150

            lbl_wgtmtr_caption.Left = 400
            lbl_wgtmtr.Left = 400
            lbl_wgtmtr.Width = 150

            dgv_Details.Columns(1).Visible = False
            dgv_Details.Columns(2).Visible = False
            dgv_Details.Columns(3).Visible = False

            dgv_Details.Columns(4).Width = dgv_Details.Columns(1).Width + dgv_Details.Columns(4).Width
            dgv_Details.Columns(5).Width = dgv_Details.Columns(2).Width + dgv_Details.Columns(5).Width
            dgv_Details.Columns(6).Width = dgv_Details.Columns(3).Width + dgv_Details.Columns(6).Width

            dgv_Details_Total.Columns(1).Visible = False
            dgv_Details_Total.Columns(2).Visible = False
            dgv_Details_Total.Columns(3).Visible = False

            dgv_Details_Total.Columns(4).Width = dgv_Details_Total.Columns(1).Width + dgv_Details_Total.Columns(4).Width
            dgv_Details_Total.Columns(5).Width = dgv_Details_Total.Columns(2).Width + dgv_Details_Total.Columns(5).Width
            dgv_Details_Total.Columns(6).Width = dgv_Details_Total.Columns(3).Width + dgv_Details_Total.Columns(6).Width


        End If

        chk_Stk_Posting_Sts.Visible = True
        chk_Bale_Close_Sts.Visible = True

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1381" Then
            btn_excel.Visible = True
            btn_SaveAll.Visible = True
        End If

        dtp_Date.Text = ""
        msk_date.Text = ""

        cbo_Bale_Bundle.Items.Clear()
        cbo_Bale_Bundle.Items.Add("BALE")
        cbo_Bale_Bundle.Items.Add("BUNDLE")
        cbo_Bale_Bundle.Items.Add("ROLL")

        cbo_Type.Items.Clear()
        cbo_Type.Items.Add("SOUND")
        cbo_Type.Items.Add("SECONDS")
        cbo_Type.Items.Add("BITS")
        cbo_Type.Items.Add("REJECT")
        cbo_Type.Items.Add("OTHERS")

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Print.Visible = False
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2
        pnl_Print.BringToFront()


        '----------------------

        pnl_OpenRecord.Visible = False
        pnl_OpenRecord.Left = (Me.Width - pnl_OpenRecord.Width) \ 2
        pnl_OpenRecord.Top = (Me.Height - pnl_OpenRecord.Height) \ 2
        pnl_OpenRecord.BringToFront()

        Dim R As Byte = CByte(Date.Now.Hour / 23 * 255)
        Dim G As Byte = CByte(Date.Now.Minute / 59 * 255)
        Dim B As Byte = CByte(Date.Now.Second / 59 * 255)

        pnl_OpenRecord.BackColor = ColorTranslator.FromHtml("#" & DateAndTime.Now.ToString("HHmmss"))

        pnl_OpenRecord.BackColor = Color.FromArgb(R, G, B)

        '---------------------




        If Common_Procedures.settings.CustomerCode = "1530" Then

            dgv_Details.Columns(2).Visible = False
            dgv_Details.Columns(3).Visible = False

            dgv_Details.Columns(1).Width = 200
            dgv_Details_Total.Columns(1).Width = 200

            txt_PcsNo.Visible = False
            cbo_Type.Visible = False
            lbl_pcsno_caption.Visible = False
            lbl_type_caption.Visible = False

            txt_Meters.Left = 254
            txt_Meters.Width = 106
            lbl_meters_caption.Left = 260

            txt_Weight.Left = 366
            txt_Weight.Width = 106
            lbl_weight_caption.Left = 373

            lbl_wgtmtr.Left = 480
            lbl_wgtmtr.Width = 127
            lbl_wgtmtr_caption.Left = 487

            txt_LotNo.Width = 200

            dgv_Details_Total.Columns(2).Visible = False
            dgv_Details_Total.Columns(3).Visible = False

            btn_LrNo_Find.Visible = True
            lbl_Filter_LrNO.Visible = True
            lbl_Filter_Stamping.Visible = True
            txt_Filter_Lr_No.Visible = True
            cbo_Filter_Stamping.Visible = True

        Else

            btn_LrNo_Find.Visible = False
            lbl_Filter_LrNO.Visible = False
            lbl_Filter_Stamping.Visible = False
            txt_Filter_Lr_No.Visible = False
            cbo_Filter_Stamping.Visible = False


        End If


        chk_Verified_Status.Visible = False
        If Common_Procedures.settings.Vefified_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_Verified_Status = 1 Then chk_Verified_Status.Visible = True
        End If


        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        AddHandler chk_Verified_Status.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_Verified_Status.LostFocus, AddressOf ControlLostFocus

        AddHandler chk_Stk_Posting_Sts.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_Stk_Posting_Sts.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_BalePrefixNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BaleSuffixNo.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Cloth.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Bale_Bundle.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Folding.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Note.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Cloth.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ClothType.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PrintFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PrintTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyName_StockOF.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Ok.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Cancel.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Godown_StockIN.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_PDF.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_net_weight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_gross_weight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Tare_weight.GotFocus, AddressOf ControlGotFocus

        AddHandler btn_PDF.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SlNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LotNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PcsNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Type.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Meters.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Weight.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_wgtmtr.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Despatch_To.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Stamping.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LR_No.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_LRDate.GotFocus, AddressOf ControlGotFocus




        AddHandler txt_SlNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LotNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PcsNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Type.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Meters.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Weight.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_wgtmtr.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_BalePrefixNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BaleSuffixNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyName_StockOF.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Cloth.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Bale_Bundle.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Folding.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Note.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Cloth.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ClothType.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PrintFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PrintTo.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Ok.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Cancel.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Godown_StockIN.LostFocus, AddressOf ControlLostFocus


        AddHandler txt_net_weight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Tare_weight.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_gross_weight.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Despatch_To.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Stamping.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LR_No.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_LRDate.LostFocus, AddressOf ControlLostFocus

        'AddHandler txt_net_weight.KeyDown, AddressOf TextBoxControlKeyDown
        '  AddHandler txt_tare_weight.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_gross_weight.KeyDown, AddressOf TextBoxControlKeyDown

        ' AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_BalePrefixNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_BaleSuffixNo.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_SlNo.KeyDown, AddressOf TextBoxControlKeyDown
        '  AddHandler txt_LotNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PcsNo.KeyDown, AddressOf TextBoxControlKeyDown
        '     AddHandler txt_Meters.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_Weight.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_SlNo.KeyPress, AddressOf TextBoxControlKeyPress
        '  AddHandler txt_LotNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PcsNo.KeyPress, AddressOf TextBoxControlKeyPress
        '     AddHandler txt_Meters.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_Weight.KeyPress, AddressOf TextBoxControlKeyPress

        '  AddHandler txt_net_weight.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler txt_tare_weight.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_gross_weight.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BalePrefixNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BaleSuffixNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PrintFrom.KeyPress, AddressOf TextBoxControlKeyPress
        '  AddHandler txt_LR_No.KeyPress, AddressOf TextBoxControlKeyPress

        'AddHandler msk_LRDate.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Filter_BaleNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Filter_Lr_No.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Filter_Width.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_InvNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Stamping.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Filter_BaleNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Filter_Lr_No.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Filter_Width.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_InvNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_Stamping.LostFocus, AddressOf ControlLostFocus


        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Try

            lbl_PortConnection.Text = ""
            lbl_PortConnection.Visible = False
            btn_Open_Port.Visible = False
            btn_Close_Port.Visible = False

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1307" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1387" Then '---- Sri Sugam Textile (Karumanthampatti)

                lbl_PortConnection.Visible = True
                btn_Open_Port.Visible = True
                btn_Close_Port.Visible = True

                btn_Open_Port.Text = "OPEN COM" & DefaultPortNo

                Dim ServerName() As String
                ServerName = Split(Common_Procedures.ServerName, "\")
                If Val(Common_Procedures.User.IdNo) <> 1 And Trim(UCase(ServerName(0))) <> Trim(UCase(Environment.MachineName)) And Common_Procedures.is_OfficeSystem() = False Then
                    PrtOp_sts = False

                    lbl_PortConnection.Text = "Port Not Found in Client"
                    lbl_PortConnection.ForeColor = Color.Red

                Else
                    ComPort_Open()

                End If

            End If


        Catch ex As Exception
            '-----
        End Try

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Packing_Slip_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Print.Visible = True Then
                    btn_print_Close_Click(sender, e)
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

    Private Sub Packing_Slip_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error Resume Next
        FrmCls_Sts = True
        con.Close()
        con.Dispose()
        Common_Procedures.Last_Closed_FormName = Me.Name
    End Sub

    Private Sub Weight_Bridge_Entry_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If Common_Procedures.SerialPort1.IsOpen = True Then
            e.Cancel = True

            Dim CloseDown As New Thread(New ThreadStart(AddressOf CloseSerialOnExit)) ' //close port in new thread to avoid hang

            System.Threading.Thread.Sleep(1000)
            CloseDown.Start() '; //close port in new thread to avoid hang

            System.Threading.Thread.Sleep(1000)
            e.Cancel = False

        End If

    End Sub

    Private Sub CloseSerialOnExit()

        Try

            ComPort_Close()

            'Application.DoEvents()
            'System.Threading.Thread.Sleep(1000)

            'Common_Procedures.SerialPort1.Close() ' //close the serial port
            'Threading.Thread.Sleep(5000)

            Me.BeginInvoke(New Action(Of String)(AddressOf NowClose))

        Catch ex As Exception
            '---MessageBox.Show(ex.Message) '//catch any serial port closing error messages

        End Try

    End Sub

    Private Sub NowClose()
        Me.Close()
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            On Error Resume Next

            dgv1 = Nothing

            If ActiveControl.Name = dgv_Details.Name Then
                dgv1 = dgv_Details

            ElseIf dgv_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Details

            End If

            If IsNothing(dgv1) = False Then

                With dgv1


                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                txt_Note.Focus()

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
                                txt_Folding.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)

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
    Private Sub Close_Form()
        Dim da As SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NoofComps As Integer
        Dim CompCondt As String

        Try

            lbl_Company.Tag = 0
            lbl_Company.Text = ""
            Me.Text = ""
            Common_Procedures.CompIdNo = 0

            CompCondt = ""
            If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
                CompCondt = "Company_Type = 'ACCOUNT'"
            End If

            da = New SqlClient.SqlDataAdapter("select count(*) from Company_Head where " & CompCondt & IIf(Trim(CompCondt) <> "", " and ", "") & " Company_IdNo <> 0", con)
            dt1 = New DataTable
            da.Fill(dt1)

            NoofComps = 0
            If dt1.Rows.Count > 0 Then
                If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                    NoofComps = Val(dt1.Rows(0)(0).ToString)
                End If
            End If
            dt1.Clear()

            If Val(NoofComps) > 1 Then

                Dim f As New Company_Selection
                f.ShowDialog()

                If Val(Common_Procedures.CompIdNo) <> 0 Then

                    da = New SqlClient.SqlDataAdapter("select Company_IdNo, Company_Name from Company_Head where Company_IdNo = " & Str(Val(Common_Procedures.CompIdNo)), con)
                    dt1 = New DataTable
                    da.Fill(dt1)

                    If dt1.Rows.Count > 0 Then
                        If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                            lbl_Company.Tag = Val(dt1.Rows(0)(0).ToString)
                            lbl_Company.Text = Trim(dt1.Rows(0)(1).ToString)
                            Me.Text = Trim(dt1.Rows(0)(1).ToString)
                        End If
                    End If
                    dt1.Clear()
                    dt1.Dispose()
                    da.Dispose()

                    new_record()

                Else
                    Me.Close()

                End If

            Else

                Me.Close()

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub move_record(ByVal no As String)

        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim SNo As Integer
        Dim LockSTS As Boolean = False
        Dim Other_Condtn2 As String = ""

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)
        NewCode = Replace(NewCode, "'", "''")

        Other_Condtn2 = "(select count(tsq1.lot_code) from Packing_Slip_Details tsq1 where tsq1.Packing_Slip_Code = '" & Trim(NewCode) & "' and tsq1.lot_code = '') > 0"

        Try
            da1 = New SqlClient.SqlDataAdapter("select a.* from Packing_Slip_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Packing_Slip_Code = '" & Trim(NewCode) & "' and " & Other_Condition & " and " & Other_Condtn2, con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_BaleRefNo.Text = dt1.Rows(0).Item("Packing_Slip_RefNo").ToString
                txt_BalePrefixNo.Text = dt1.Rows(0).Item("Packing_Slip_PrefixNo").ToString
                txt_BaleSuffixNo.Text = dt1.Rows(0).Item("Packing_Slip_SuffixNo").ToString

                dtp_Date.Text = dt1.Rows(0).Item("Packing_Slip_Date").ToString
                msk_date.Text = dtp_Date.Text
                cbo_PartyName_StockOF.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                cbo_Cloth.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_IdNo").ToString))
                cbo_ClothType.Text = Common_Procedures.ClothType_IdNoToName(con, Val(dt1.Rows(0).Item("ClothType_IdNo").ToString))
                cbo_Godown_StockIN.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("WareHouse_IdNo").ToString))
                cbo_Bale_Bundle.Text = dt1.Rows(0).Item("Bale_Bundle").ToString


                txt_Tare_weight.Text = dt1.Rows(0).Item("Tare_Weight").ToString
                txt_net_weight.Text = dt1.Rows(0).Item("Net_Weight").ToString
                txt_gross_weight.Text = dt1.Rows(0).Item("Gross_Weight").ToString


                If Val(dt1.Rows(0).Item("Verified_Status").ToString) = 1 Then chk_Verified_Status.Checked = True

                If Val(dt1.Rows(0).Item("Stock_Posting_Sts").ToString) = 1 Then chk_Stk_Posting_Sts.Checked = True

                txt_Note.Text = dt1.Rows(0).Item("Note").ToString
                txt_Folding.Text = Format(Val(dt1.Rows(0).Item("Folding").ToString), "########0.00")
                Lbl_DelvCode.Text = dt1.Rows(0).Item("Delivery_Code").ToString
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))
                If IsDBNull(dt1.Rows(0).Item("Delivery_Code").ToString) = False Then
                    If Trim(dt1.Rows(0).Item("Delivery_Code").ToString) <> "" Then
                        LockSTS = True
                    End If
                End If

                'If Not IsDBNull(dt1.Rows(0).Item("Stamping_IdNo")) Then
                '    If dt1.Rows(0).Item("Stamping_IdNo") > 0 Then
                '        cbo_Stamping.Text = Common_Procedures.Cloth_IdNoToName(con, dt1.Rows(0).Item("Stamping_IdNo"))
                '    End If
                'End If

                If Not IsDBNull(dt1.Rows(0).Item("Stamping_IdNo")) Then
                    If dt1.Rows(0).Item("Stamping_IdNo") > 0 Then
                        cbo_Stamping.Text = Common_Procedures.Stamping_IdnoToName(con, dt1.Rows(0).Item("Stamping_IdNo"))
                    End If
                End If

                If Not IsDBNull(dt1.Rows(0).Item("Despatch_To_IdNo")) Then
                    If dt1.Rows(0).Item("Despatch_To_IdNo") > 0 Then
                        cbo_Despatch_To.Text = Common_Procedures.Area_IdNoToName(con, dt1.Rows(0).Item("Despatch_To_IdNo"))
                    End If
                End If

                If Not IsDBNull(dt1.Rows(0).Item("Transporter_IdNo")) Then
                    If dt1.Rows(0).Item("Transporter_IdNo") > 0 Then
                        cbo_Transport.Text = Common_Procedures.Transport_IdNoToName(con, dt1.Rows(0).Item("Transporter_IdNo"))
                    End If
                End If

                msk_LRDate.Text = ""

                If Not IsDBNull(dt1.Rows(0).Item("LR_No")) Then
                    If Len(Trim(dt1.Rows(0).Item("LR_No"))) > 0 Then
                        txt_LR_No.Text = dt1.Rows(0).Item("LR_No")
                        If Not IsDBNull(dt1.Rows(0).Item("LR_Date")) Then
                            'If dt1.Rows(0).Item("LR_Date") > 0 Then
                            dtp_LR_Date.Value = dt1.Rows(0).Item("LR_Date")
                            msk_LRDate.Text = dtp_LR_Date.Value
                            'End If
                        End If
                    End If
                End If



                If Val(dt1.Rows(0).Item("Bale_Close_Sts").ToString) = 1 Then chk_Bale_Close_Sts.Checked = True

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.clothtype_name , c.Ledger_Name, d.cloth_name from Packing_Slip_Details a LEFT OUTER JOIN ClothType_Head b ON a.ClothType_IdNo <> 0 and a.ClothType_IdNo = b.ClothType_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Party_Idno <> 0 and a.Party_Idno = c.Ledger_Idno LEFT OUTER JOIN Cloth_Head d ON a.Cloth_Idno <> 0 and a.Cloth_Idno = d.Cloth_Idno where a.Packing_Slip_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                dgv_Details.Rows.Clear()
                SNo = 0

                If dt2.Rows.Count > 0 Then

                    For i = 0 To dt2.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        SNo = SNo + 1
                        dgv_Details.Rows(n).Cells(0).Value = Val(SNo)
                        dgv_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Lot_No").ToString
                        dgv_Details.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Pcs_NO").ToString
                        dgv_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("clothtype_name").ToString
                        dgv_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                        If Val(dt2.Rows(i).Item("Weight").ToString) <> 0 Then
                            dgv_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")
                        End If
                        If Val(dt2.Rows(i).Item("Weight_Meter").ToString) <> 0 Then
                            dgv_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Weight_Meter").ToString), "########0.000")
                        End If

                        dgv_Details.Rows(n).Cells(7).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                        dgv_Details.Rows(n).Cells(8).Value = dt2.Rows(i).Item("lot_code").ToString
                        dgv_Details.Rows(n).Cells(9).Value = dt2.Rows(i).Item("cloth_name").ToString

                        If Val(dt2.Rows(i).Item("Loom_IdNo").ToString) <> 0 Then
                            dgv_Details.Rows(n).Cells(10).Value = Common_Procedures.Loom_IdNoToName(con, Val(dt2.Rows(i).Item("Loom_IdNo").ToString))
                        Else
                            dgv_Details.Rows(n).Cells(10).Value = dt2.Rows(i).Item("Loom_No").ToString
                        End If



                    Next i

                End If

                SNo = SNo + 1
                txt_SlNo.Text = Val(SNo)

                With dgv_Details_Total

                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(2).Value = Val(dt1.Rows(0).Item("Total_Pcs").ToString)
                    .Rows(0).Cells(4).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                    .Rows(0).Cells(5).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")

                End With



                dt2.Clear()
                dt2.Dispose()
                da2.Dispose()


            Else

                new_record()

            End If



            If LockSTS = True And chk_Bale_Close_Sts.Checked = False Then
                If Trim(UCase(dt1.Rows(0).Item("Delivery_Code").ToString)) <> Trim(UCase("999999/CLOSE")) Then

                    cbo_Cloth.Enabled = False
                    cbo_Cloth.BackColor = Color.LightGray

                    cbo_ClothType.Enabled = False
                    cbo_ClothType.BackColor = Color.LightGray

                    txt_Folding.Enabled = False
                    txt_Folding.BackColor = Color.LightGray

                End If

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If msk_date.Visible And msk_date.Enabled Then msk_date.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_BaleNo.Text)


        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.PackinSlip_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.PackinSlip_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BaleRefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Packing_slip_Entry, New_Entry, Me, con, "Packing_Slip_Head", "Packing_Slip_Code", NewCode, "Packing_Slip_Date", "(Packing_Slip_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

        If Common_Procedures.settings.Vefified_Status = 1 Then
            NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BaleRefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            If Val(Common_Procedures.get_FieldValue(con, "Packing_Slip_Head", "Verified_Status", "(Packing_Slip_Code = '" & Trim(NewCode) & "')")) = 1 Then
                MessageBox.Show("Entry Already Verified", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Pnl_Back.Enabled = False Then
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

        NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BaleRefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        NewCode = Replace(NewCode, "'", "''")

        Da = New SqlClient.SqlDataAdapter("select count(*) from Packing_Slip_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Packing_Slip_Code = '" & Trim(NewCode) & "' and Delivery_Code like  'UNPAK-%' and  Delivery_Code <> '' and Bale_Close_Sts = 0", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already this bale UnPacked", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If
        Dt1.Clear()


        Da = New SqlClient.SqlDataAdapter("select count(*)  from Packing_Slip_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Packing_Slip_Code = '" & Trim(NewCode) & "' and Delivery_Code <> '' and Delivery_Code NOT LIKE '%/CLOSE' and Bale_Close_Sts = 0", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            ' If Trim(UCase(Dt1.Rows(0).Item("Delivery_Code").ToString)) <> Trim(UCase("999999/CLOSE")) Then
            If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                    MessageBox.Show("Already this bale delivered", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
            '  End If
        End If
        Dt1.Clear()

        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans
            'Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Packing_Slip_head", "Packing_Slip_Code", Val(lbl_Company.Tag), NewCode, lbl_BaleNo.Text, Val(vOrdByNo), PkCondition_Entry, "", "", New_Entry, True, "", "", "Packing_Slip_Code, Company_IdNo, for_OrderBy", trans)
            'Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Packing_Slip_Details", "Packing_Slip_Code", Val(lbl_Company.Tag), NewCode, lbl_BaleNo.Text, Val(vOrdByNo), PkCondition_Entry, "", "", New_Entry, True, "Lot_No,Pcs_No,ClothType_IdNo,Meters,Weight,Weight_Meter,Party_IdNo,Lot_Code,Loom_IdNo,Loom_No", "Sl_No", "Packing_Slip_Code, For_OrderBy, Company_IdNo, Packing_Slip_No, Packing_Slip_Date, Ledger_Idno", trans)


            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type1 = '', PackingSlip_Inc_Type1 = PackingSlip_Inc_Type1 - 1 Where PackingSlip_Code_Type1 = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type2 = '', PackingSlip_Inc_Type2 = PackingSlip_Inc_Type2 - 1 Where PackingSlip_Code_Type2 = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type3 = '', PackingSlip_Inc_Type3 = PackingSlip_Inc_Type3 - 1 Where PackingSlip_Code_Type3 = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type4 = '', PackingSlip_Inc_Type4 = PackingSlip_Inc_Type4 - 1 Where PackingSlip_Code_Type4 = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type5 = '', PackingSlip_Inc_Type5 = PackingSlip_Inc_Type5 - 1 Where PackingSlip_Code_Type5 = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Packing_Slip_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Packing_Slip_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from Packing_Slip_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Packing_Slip_Code = '" & Trim(NewCode) & "'"
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

            If msk_date.Enabled = True And msk_date.Visible = True Then msk_date.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record
        Dim Cmd As New SqlClient.SqlCommand
        Dim Nr As Long
        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable
            Dim dt4 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
            da.Fill(dt1)
            cbo_Filter_Cloth.DataSource = dt1
            cbo_Filter_Cloth.DisplayMember = "Cloth_Name"

            da = New SqlClient.SqlDataAdapter("select ClothType_Name from ClothType_head order by ClothType_Name", con)
            da.Fill(dt2)
            cbo_Filter_ClothType.DataSource = dt2
            cbo_Filter_ClothType.DisplayMember = "ClothType_Name"


            da = New SqlClient.SqlDataAdapter("select Stamping_name from Stamping_head order by Stamping_name", con)
            da.Fill(dt3)
            cbo_Filter_Stamping.DataSource = dt3
            cbo_Filter_Stamping.DisplayMember = "Stamping_name"
            cbo_Filter_Stamping.SelectedIndex = -1





            Cmd.Connection = con

            Cmd.CommandText = "Update ClothSales_Invoice_Head set InvoiceNo_forSelection = ClothSales_Invoice_RefNo + '/' + right(ClothSales_Invoice_Code,5)+ '/' + cast(company_idno as varchar) Where ClothSales_Invoice_No <> ''"
            Nr = Cmd.ExecuteNonQuery()


            da = New SqlClient.SqlDataAdapter("select InvoiceNo_forSelection from ClothSales_Invoice_Head order by InvoiceNo_forSelection", con)
            da.Fill(dt4)
            cbo_Filter_Stamping.DataSource = dt4
            cbo_Filter_Stamping.DisplayMember = "InvoiceNo_forSelection"
            cbo_Filter_Stamping.SelectedIndex = -1


            'da = New SqlClient.SqlDataAdapter("select delivery_code from Packing_Slip_Head order by delivery_code", con)
            'da.Fill(dt4)
            'cbo_Filter_Stamping.DataSource = dt4
            'cbo_Filter_Stamping.DisplayMember = "delivery_code"
            'cbo_Filter_Stamping.SelectedIndex = -1


            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_Cloth.Text = ""
            cbo_Filter_ClothType.Text = ""
            cbo_Filter_Stamping.Text = ""


            cbo_Filter_Cloth.SelectedIndex = -1
            cbo_Filter_ClothType.SelectedIndex = -1
            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Filter.BringToFront()
        Pnl_Back.Enabled = False


        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String
        Dim Other_Condtn2 As String

        Try

            Other_Condtn2 = "(select count(tsq1.lot_code) from Packing_Slip_Details tsq1 where tsq1.Packing_Slip_Code = a.Packing_Slip_Code and tsq1.lot_code = '') > 0"

            da = New SqlClient.SqlDataAdapter("select top 1 a.Packing_Slip_RefNo from Packing_Slip_Head a where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Packing_Slip_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and " & Other_Condition & " and " & Other_Condtn2 & " Order by a.Packing_Slip_Date, a.for_Orderby, a.Packing_Slip_RefNo, a.Packing_Slip_No, a.Packing_Slip_Code", con)
            dt = New DataTable
            da.Fill(dt)

            movno = ""
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(dt.Rows(0)(0).ToString)
                End If
            End If

            dt.Clear()
            dt.Dispose()
            da.Dispose()

            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0
        Dim NewCode As String = ""
        Dim MtchSTS As Boolean = False
        Dim BalNo As String = ""
        Dim L As Integer = -1
        Dim Other_Condtn2 As String

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_BaleRefNo.Text))

            BalNo = Val(lbl_BaleRefNo.Text)
            L = Len(Trim(BalNo))

            Other_Condtn2 = "(select count(tsq1.lot_code) from Packing_Slip_Details tsq1 where tsq1.Packing_Slip_Code = a.Packing_Slip_Code and tsq1.lot_code = '') > 0"

            If Trim(UCase(BalNo)) <> Trim(UCase(lbl_BaleRefNo.Text)) And Len(Trim(BalNo)) <> (Len(Trim(lbl_BaleRefNo.Text)) + 1) Then

                NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BaleRefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

                da = New SqlClient.SqlDataAdapter("select a.* from Packing_Slip_Head a where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Packing_Slip_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and " & Other_Condition & " Order by a.Packing_Slip_Date, a.for_Orderby, a.Packing_Slip_RefNo, a.Packing_Slip_No, a.Packing_Slip_Code", con)
                dt = New DataTable
                da.Fill(dt)

                movno = ""
                MtchSTS = False
                If dt.Rows.Count > 0 Then
                    For i = 0 To dt.Rows.Count - 1
                        If MtchSTS = True Then
                            movno = dt.Rows(i).Item("Packing_Slip_RefNo").ToString
                            Exit For

                        Else
                            If Trim(UCase(dt.Rows(i).Item("Packing_Slip_Code").ToString)) = Trim(UCase(NewCode)) Then
                                MtchSTS = True
                            End If

                        End If

                    Next

                End If

            Else

                da = New SqlClient.SqlDataAdapter("select top 1 a.Packing_Slip_RefNo from Packing_Slip_Head a where a.for_orderby > " & Str(Val(OrdByNo)) & " and a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Packing_Slip_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and  " & Other_Condition & " and  " & Other_Condtn2 & " Order by a.Packing_Slip_Date, a.for_Orderby, a.Packing_Slip_RefNo, a.Packing_Slip_No", con)
                dt = New DataTable
                da.Fill(dt)

                movno = ""
                If dt.Rows.Count > 0 Then
                    If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                        movno = dt.Rows(0)(0).ToString
                    End If
                End If

            End If


            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0
        Dim NewCode As String = ""
        Dim MtchSTS As Boolean = False
        Dim BalNo As String = ""
        Dim L As Integer = -1
        Dim Other_Condtn2 As String

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_BaleRefNo.Text))


            BalNo = Val(lbl_BaleRefNo.Text)
            L = Len(Trim(BalNo))

            Other_Condtn2 = "(select count(tsq1.lot_code) from Packing_Slip_Details tsq1 where tsq1.Packing_Slip_Code = a.Packing_Slip_Code and tsq1.lot_code = '') > 0"

            If Trim(UCase(BalNo)) <> Trim(UCase(lbl_BaleRefNo.Text)) And Len(Trim(BalNo)) <> (Len(Trim(lbl_BaleRefNo.Text)) + 1) Then

                NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BaleRefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

                da = New SqlClient.SqlDataAdapter("select a.* from Packing_Slip_Head a where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Packing_Slip_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and " & Other_Condition & " and " & Other_Condtn2 & " Order by a.Packing_Slip_Date desc, a.for_Orderby desc, a.Packing_Slip_RefNo desc, a.Packing_Slip_No desc, a.Packing_Slip_Code desc", con)
                dt = New DataTable
                da.Fill(dt)

                movno = ""
                MtchSTS = False
                If dt.Rows.Count > 0 Then
                    For i = 0 To dt.Rows.Count - 1
                        If MtchSTS = True Then
                            movno = dt.Rows(i).Item("Packing_Slip_RefNo").ToString
                            Exit For

                        Else
                            If Trim(UCase(dt.Rows(i).Item("Packing_Slip_Code").ToString)) = Trim(UCase(NewCode)) Then
                                MtchSTS = True
                            End If

                        End If

                    Next

                End If

            Else

                'old
                'da = New SqlClient.SqlDataAdapter("select top 1 Packing_Slip_RefNo from Packing_Slip_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Packing_Slip_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and " & Other_Condition & " Order by for_Orderby desc, Packing_Slip_RefNo desc, Packing_Slip_No desc", con)

                da = New SqlClient.SqlDataAdapter("select top 1 Packing_Slip_RefNo from Packing_Slip_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Packing_Slip_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and " & Other_Condition & "  Order by Packing_Slip_Date desc, for_Orderby desc, Packing_Slip_RefNo desc, Packing_Slip_No desc", con)
                da.Fill(dt)

                movno = ""
                If dt.Rows.Count > 0 Then
                    If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                        movno = dt.Rows(0)(0).ToString
                    End If
                End If

            End If



            If Val(movno) <> 0 Then move_record(movno)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim Other_Condtn2 As String


        Try

            Other_Condtn2 = "(select count(tsq1.lot_code) from Packing_Slip_Details tsq1 where tsq1.Packing_Slip_Code = a.Packing_Slip_Code and tsq1.lot_code = '') > 0"

            da = New SqlClient.SqlDataAdapter("select top 1 a.Packing_Slip_RefNo from Packing_Slip_Head a where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Packing_Slip_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and  " & Other_Condition & " and " & Other_Condtn2 & " Order by a.Packing_Slip_Date desc, a.for_Orderby desc, a.Packing_Slip_RefNo desc, a.Packing_Slip_No desc, a.Packing_Slip_Code desc", con)
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

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewID As Integer = 0
        Dim vORDBY As String = ""
        Dim movno As String = ""
        Dim Other_Condtn2 As String

        Try

            clear()

            New_Entry = True

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1381" Then '---KRG WEAVES

                Other_Condtn2 = "(select count(tsq1.lot_code) from Packing_Slip_Details tsq1 where tsq1.Packing_Slip_Code = a.Packing_Slip_Code and tsq1.lot_code = '') > 0"

                da = New SqlClient.SqlDataAdapter("select top 1 a.Packing_Slip_RefNo from Packing_Slip_Head a where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Packing_Slip_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and  " & Other_Condition & " and " & Other_Condtn2 & " Order by a.Packing_Slip_Date desc, a.for_Orderby desc, a.Packing_Slip_RefNo desc, a.Packing_Slip_No desc, a.Packing_Slip_Code desc", con)
                dt1 = New DataTable
                da.Fill(dt1)

                movno = ""
                If dt1.Rows.Count > 0 Then
                    If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                        movno = dt1.Rows(0)(0).ToString
                    End If
                End If

                If Val(movno) <> 0 Then
                    lbl_BaleRefNo.Text = Format(Val(movno), "#########0") + 1
                Else
                    lbl_BaleRefNo.Text = Common_Procedures.get_MaxCode(con, "Packing_Slip_Head", "Packing_Slip_Code", "For_OrderBy", Other_Condition, Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
                End If

            Else

                lbl_BaleRefNo.Text = Common_Procedures.get_MaxCode(con, "Packing_Slip_Head", "Packing_Slip_Code", "For_OrderBy", Other_Condition, Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            End If


            lbl_BaleRefNo.ForeColor = Color.Red

            msk_date.Text = Date.Today.ToShortDateString
            vORDBY = ""
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1381" Then 'KRG WEAVES
                vORDBY = "Packing_Slip_Date desc, "
            End If
            da = New SqlClient.SqlDataAdapter("select top 1 * from Packing_Slip_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Packing_Slip_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and " & Other_Condition & " Order by " & vORDBY & " for_Orderby desc, Packing_Slip_RefNo desc, Packing_Slip_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Packing_Slip_Date").ToString <> "" Then msk_date.Text = dt1.Rows(0).Item("Packing_Slip_Date").ToString
                End If

                If cbo_PartyName_StockOF.Visible Then
                    cbo_PartyName_StockOF.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                End If

                cbo_Cloth.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_IdNo").ToString))
                cbo_ClothType.Text = Common_Procedures.ClothType_IdNoToName(con, Val(dt1.Rows(0).Item("ClothType_IdNo").ToString))
                cbo_Bale_Bundle.Text = dt1.Rows(0).Item("Bale_Bundle").ToString

                txt_Folding.Text = Format(Val(dt1.Rows(0).Item("Folding").ToString), "########0.00")
                If cbo_Godown_StockIN.Visible Then
                    cbo_Godown_StockIN.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("WareHouse_IdNo").ToString))
                End If

                If txt_BalePrefixNo.Visible = True Then
                    txt_BalePrefixNo.Text = dt1.Rows(0).Item("Packing_Slip_PrefixNo").ToString
                End If
                If txt_BaleSuffixNo.Visible = True Then
                    txt_BaleSuffixNo.Text = dt1.Rows(0).Item("Packing_Slip_SuffixNo").ToString
                End If

                If IsDBNull(dt1.Rows(0).Item("Stock_Posting_Sts").ToString) = False Then
                    If Val(dt1.Rows(0).Item("Stock_Posting_Sts").ToString) = 1 Then chk_Stk_Posting_Sts.Checked = True Else chk_Stk_Posting_Sts.Checked = False
                End If

            End If
            dt1.Clear()

            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus() : msk_date.SelectionStart = 0

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        dt1.Dispose()
        da.Dispose()

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String

        Try




            inpno = InputBox("Enter Bale.No.", "FOR FINDING...")

            RecCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)
            RecCode = Replace(RecCode, "'", "''")

            Da = New SqlClient.SqlDataAdapter("select Packing_Slip_RefNo from Packing_Slip_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Packing_Slip_Code = '" & Trim(RecCode) & "'", con)
            Dt = New DataTable
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()
            Dt.Dispose()
            Da.Dispose()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                MessageBox.Show("Rec No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.PackinSlip_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.PackinSlip_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Packing_slip_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Bale No.", "FOR NEW BALE INSERTION...")
            If InStr(1, inpno, "'") > 0 Or InStr(1, inpno, """") > 0 Then
                MessageBox.Show("Invalid Bale No - Does not accept special characters", "DOES NOT INSERT NEW Bale NO...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If
            If Trim(UCase(vEntryType)) = "BALEGROUPWISE" Then
                If Trim(UCase(Val(inpno))) <> Trim(UCase(inpno)) Then
                    MessageBox.Show("Invalid Bale No - Does not accept characters", "DOES NOT INSERT NEW Bale NO...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If

            RecCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Packing_Slip_RefNo from Packing_Slip_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Packing_Slip_Code = '" & Trim(RecCode) & "' and " & Trim(Other_Condition), con)
            Dt = New DataTable
            Da.Fill(Dt)

            movno = ""
            If Dt.Rows.Count > 0 Then
                If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                    movno = Trim(Dt.Rows(0)(0).ToString)
                End If
            End If

            Dt.Clear()
            Dt.Dispose()
            Da.Dispose()

            If Val(movno) <> 0 Then
                move_record(movno)

            Else
                If Val(inpno) = 0 Then
                    MessageBox.Show("Invalid Bale No", "DOES NOT INSERT NEW Bale NO...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_BaleRefNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW Bale No...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Clth_ID As Integer = 0
        Dim Clthty_ID As Integer = 0
        Dim dCloTyp_ID As Integer = 0
        Dim dClo_ID As Integer = 0
        Dim Trans_ID As Integer = 0
        Dim vLed_IdNo As Integer = 0
        Dim EdsCnt_ID As Integer = 0
        Dim DispTo_IdNo As Integer = 0
        Dim Stamping_IdNo As Integer = 0
        Dim Sno As Integer = 0
        Dim vTotMtrs As Single, vTotPcs As Single, vTotWgt As Single
        Dim party_ID As Integer = 0
        Dim vLmIdNo As Integer = 0
        Dim vLmNo As String = ""
        Dim vGdwn_IdNo As Integer = 0
        Dim vLRDATE As String = ""
        Dim Verified_STS As String = ""
        Dim vOrdByNo As String = ""


        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then '----KRG TEXTILE MILLS (PALLADAM)
        '    MessageBox.Show("Invalid Entry" & Chr(13) & "It is blocked by system admin", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '    Exit Sub
        'End If

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_BaleNo.Text)


        NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BaleRefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Packing_slip_Entry, New_Entry, Me, con, "Packing_Slip_Head", "Packing_Slip_Code", NewCode, "Packing_Slip_Date", "(Packing_Slip_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Packing_Slip_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Packing_Slip_No desc", dtp_Date.Value.Date) = False Then Exit Sub


        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.PackinSlip_Entry, New_Entry) = False Then Exit Sub


        If Common_Procedures.settings.Vefified_Status = 1 Then
            If Not (Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_Verified_Status = 1) Then
                NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BaleRefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


                If Val(Common_Procedures.get_FieldValue(con, "Packing_Slip_Head", "Verified_Status", "(Packing_Slip_Code = '" & Trim(NewCode) & "')")) = 1 Then
                    MessageBox.Show("Entry Already Verified", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If
        End If


        If Pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
            Exit Sub
        End If

        If Not (Convert.ToDateTime(msk_date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
            Exit Sub
        End If

        vLed_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyName_StockOF.Text)
        If cbo_PartyName_StockOF.Visible = True Then
            If vLed_IdNo = 0 Then
                MessageBox.Show("Invalid Stock Of Name (OwnSort / JobWork )", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_PartyName_StockOF.Enabled And cbo_PartyName_StockOF.Visible Then cbo_PartyName_StockOF.Focus()
                Exit Sub
            End If
        End If
        If vLed_IdNo = 0 Then vLed_IdNo = Common_Procedures.CommonLedger.OwnSort_Ac

        vGdwn_IdNo = Common_Procedures.Ledger_NameToIdNo(con, cbo_Godown_StockIN.Text)
        If cbo_Godown_StockIN.Visible = True Then
            If vGdwn_IdNo = 0 Then
                MessageBox.Show("Invalid Fabric Godown Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_Godown_StockIN.Enabled And cbo_Godown_StockIN.Visible Then cbo_Godown_StockIN.Focus()
                Exit Sub
            End If
        End If
        If vGdwn_IdNo = 0 Then vGdwn_IdNo = Common_Procedures.CommonLedger.Godown_Ac

        Clth_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_Cloth.Text)
        If Clth_ID = 0 Then
            MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_Cloth.Enabled And cbo_Cloth.Visible Then cbo_Cloth.Focus()
            Exit Sub
        End If

        Clthty_ID = Common_Procedures.ClothType_NameToIdNo(con, cbo_ClothType.Text)
        If Clthty_ID = 0 Then
            MessageBox.Show("Invalid Cloth Type Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_ClothType.Enabled And cbo_ClothType.Visible Then cbo_ClothType.Focus()
            Exit Sub
        End If

        If Val(txt_Folding.Text) = 0 Then
            txt_Folding.Text = 100
            'MessageBox.Show("Invalid Folding", "DOES NOT SELECT PAVU...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            'If txt_Folding.Enabled And txt_Folding.Visible Then txt_Folding.Focus()
            'Exit Sub
        End If

        If Len(Trim(cbo_Stamping.Text)) > 0 Then
            Stamping_IdNo = Common_Procedures.Stamping_NameToIdno(con, cbo_Stamping.Text)

        End If

        If Len(Trim(cbo_Transport.Text)) > 0 Then
            Trans_ID = Common_Procedures.Transport_NameToIdNo(con, cbo_Transport.Text)
        End If

        If Len(Trim(cbo_Despatch_To.Text)) > 0 Then
            DispTo_IdNo = Common_Procedures.Area_NameToIdNo(con, cbo_Despatch_To.Text)
        End If

        Verified_STS = 0
        If chk_Verified_Status.Checked = True Then Verified_STS = 1

        Dim StockPos_STS As String = ""

        StockPos_STS = 0
        If chk_Stk_Posting_Sts.Checked = True Then StockPos_STS = 1

        Dim BaleCls_STS As String = ""

        BaleCls_STS = 0
        If chk_Bale_Close_Sts.Checked = True Then BaleCls_STS = 1

        vTotMtrs = 0 : vTotPcs = 0 : vTotWgt = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotMtrs = Val(dgv_Details_Total.Rows(0).Cells(4).Value())
            vTotPcs = Val(dgv_Details_Total.Rows(0).Cells(2).Value())
            vTotWgt = Val(dgv_Details_Total.Rows(0).Cells(5).Value())
        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1381" Then

                NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BaleRefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_BaleRefNo.Text = Common_Procedures.get_MaxCode(con, "Packing_Slip_Head", "Packing_Slip_Code", "For_OrderBy", Other_Condition, Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BaleRefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            If Common_Procedures.settings.CustomerCode = "1155" Then
                lbl_BaleNo.Text = Trim(lbl_BaleRefNo.Text) & Trim(txt_BaleSuffixNo.Text)
            Else
                lbl_BaleNo.Text = Trim(txt_BalePrefixNo.Text) & Trim(lbl_BaleRefNo.Text) & Trim(txt_BaleSuffixNo.Text)
            End If



            ' If Common_Procedures.settings.CustomerCode <> "1357" Then
            Da = New SqlClient.SqlDataAdapter("Select  count(*) from Packing_Slip_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Packing_Slip_Code = '" & Trim(NewCode) & "' and Delivery_Code <> '' and Delivery_Code NOT LIKE '%/CLOSE'  and Bale_Close_Sts = 0 ", con)
            Da.SelectCommand.Transaction = tr
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                'If Trim(UCase(Dt1.Rows(0).Item("Delivery_Code").ToString)) <> Trim(UCase("999999/CLOSE")) Then
                If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                    If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                        Throw New ApplicationException("Already this bale delivered")
                        Exit Sub
                    End If
                End If
                'End If
            End If
            Dt1.Clear()
            '  End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@RecDate", Convert.ToDateTime(msk_date.Text))



            vLRDATE = ""
            If Len(Trim(txt_LR_No.Text)) > 0 Then
                If Trim(msk_LRDate.Text) <> "" Then
                    If IsDate(msk_LRDate.Text) = True Then
                        vLRDATE = msk_LRDate.Text
                    End If
                End If
            End If

            If Len(Trim(vLRDATE)) > 0 And IsDate(vLRDATE) = True Then
                cmd.Parameters.AddWithValue("@LRDate", Convert.ToDateTime(msk_LRDate.Text))
            Else
                cmd.Parameters.AddWithValue("@LRDate", DBNull.Value)
            End If

            If Val(vTotMtrs) = 0 Then
                MessageBox.Show("Invalid PackingSlip Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If txt_LotNo.Enabled And txt_LotNo.Visible Then txt_LotNo.Focus()
                Exit Sub
            End If

            If New_Entry = True Then

                cmd.CommandText = "Insert into Packing_Slip_Head(   Packing_Slip_Code   ,                 Company_IdNo     ,        Packing_Slip_PrefixNo         ,            Packing_Slip_RefNo     ,             Packing_Slip_SuffixNo    ,            Packing_Slip_No     ,                               for_OrderBy                                   , Packing_Slip_Date  ,              Cloth_IdNo   ,           ClothType_IdNo    ,                    Bale_Bundle       ,                  Folding            ,              Total_Pcs    ,              Total_Meters  ,           Total_Weight    ,               Note            ,           Ledger_IdNo      ,            User_IdNo                    ,     WareHouse_IdNo    ,              BaleGroup_IdNo           ,Verified_Status            ,Net_Weight                             ,Tare_Weight                            ,               Gross_Weight            ,       Stock_Posting_Sts   ,    Bale_Close_Sts                ,       Stamping_IdNo           ,     Despatch_To_IdNo        ,     Transporter_IdNo     ,          LR_No          , LR_Date  ) " &
                                                "Values                       ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(txt_BalePrefixNo.Text) & "', '" & Trim(lbl_BaleRefNo.Text) & "', '" & Trim(txt_BaleSuffixNo.Text) & "', '" & Trim(lbl_BaleNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_BaleRefNo.Text))) & " ,        @RecDate    ,  " & Str(Val(Clth_ID)) & ", " & Str(Val(Clthty_ID)) & " , '" & Trim(cbo_Bale_Bundle.Text) & "' ,  " & Str(Val(txt_Folding.Text)) & " , " & Str(Val(vTotPcs)) & " , " & Str(Val(vTotMtrs)) & " , " & Str(Val(vTotWgt)) & " , '" & Trim(txt_Note.Text) & "' , " & Str(Val(vLed_IdNo)) & "," & Val(Common_Procedures.User.IdNo) & " ," & Val(vGdwn_IdNo) & ",  " & Str(Val(vEntry_BaleGroupIdNo)) & "," & Val(Verified_STS) & ", " & Str(Val(txt_net_weight.Text)) & ", " & Str(Val(txt_Tare_weight.Text)) & ", " & Str(Val(txt_gross_weight.Text)) & " ," & Val(StockPos_STS) & " , " & Val(BaleCls_STS) & ", " & Stamping_IdNo.ToString & ", " & DispTo_IdNo.ToString & ", " & Trans_ID.ToString & ", '" & txt_LR_No.Text & "', @LRDate  ) "
                cmd.ExecuteNonQuery()

            Else
                'Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Packing_Slip_head", "Packing_Slip_Code", Val(lbl_Company.Tag), NewCode, lbl_BaleNo.Text, Val(vOrdByNo), PkCondition_Entry, "", "", New_Entry, False, "", "", "Packing_Slip_Code, Company_IdNo, for_OrderBy", tr)
                'Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Packing_Slip_Details", "Packing_Slip_Code", Val(lbl_Company.Tag), NewCode, lbl_BaleNo.Text, Val(vOrdByNo), PkCondition_Entry, "", "", New_Entry, False, "Lot_No,Pcs_No,ClothType_IdNo,Meters,Weight,Weight_Meter,Party_IdNo,Lot_Code,Loom_IdNo,Loom_No", "Sl_No", "Packing_Slip_Code, For_OrderBy, Company_IdNo, Packing_Slip_No, Packing_Slip_Date, Ledger_Idno", tr)

                cmd.CommandText = "Update Packing_Slip_Head set Delivery_Code = '', Delivery_No = '', Delivery_DetailsSlNo = 0, Delivery_Increment = 0, Delivery_Date = Null Where Packing_Slip_Code = '" & Trim(NewCode) & "' and Delivery_Code = '999999/CLOSE' and Bale_Close_Sts = 1 "
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Packing_Slip_Head set Packing_Slip_PrefixNo = '" & Trim(txt_BalePrefixNo.Text) & "', Packing_Slip_RefNo = '" & Trim(lbl_BaleRefNo.Text) & "', Packing_Slip_SuffixNo = '" & Trim(txt_BaleSuffixNo.Text) & "', Packing_Slip_No = '" & Trim(lbl_BaleNo.Text) & "', Packing_Slip_Date = @RecDate, Cloth_IdNo = " & Str(Val(Clth_ID)) & " , ClothType_IdNo = " & Str(Val(Clthty_ID)) & " , Bale_Bundle = '" & Trim(cbo_Bale_Bundle.Text) & "'   ,   Folding = " & Str(Val(txt_Folding.Text)) & ", Total_Pcs = " & Str(Val(vTotPcs)) & ", Total_Weight = " & Str(Val(vTotWgt)) & " , Total_Meters = " & Str(Val(vTotMtrs)) & " , Note = '" & Trim(txt_Note.Text) & "' , Ledger_IdNo = " & Str(Val(vLed_IdNo)) & " , User_IdNo = " & Val(Common_Procedures.User.IdNo) & ", WareHouse_IdNo = " & Val(vGdwn_IdNo) & " , BaleGroup_IdNo = " & Str(Val(vEntry_BaleGroupIdNo)) & ",Verified_Status= " & Val(Verified_STS) & " ,Net_Weight=" & Str(Val(txt_net_weight.Text)) & ", Tare_Weight=" & Str(Val(txt_Tare_weight.Text)) & ",Gross_Weight= " & Str(Val(txt_gross_weight.Text)) & " ,  Stock_Posting_Sts = " & Val(StockPos_STS) & " , Bale_Close_Sts =  " & Val(BaleCls_STS) & "   Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Packing_Slip_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Packing_Slip_Head  set Stamping_IdNo  = " & Stamping_IdNo.ToString & ", Despatch_To_IdNo  = " & DispTo_IdNo.ToString & ", Transporter_IdNo = " & Trans_ID.ToString & ", LR_No = '" & txt_LR_No.Text.ToString & "', LR_Date = @LRDate where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Packing_Slip_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type1 = '', PackingSlip_Inc_Type1 = PackingSlip_Inc_Type1 - 1 Where PackingSlip_Code_Type1 = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type2 = '', PackingSlip_Inc_Type2 = PackingSlip_Inc_Type2 - 1 Where PackingSlip_Code_Type2 = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type3 = '', PackingSlip_Inc_Type3 = PackingSlip_Inc_Type3 - 1 Where PackingSlip_Code_Type3 = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type4 = '', PackingSlip_Inc_Type4 = PackingSlip_Inc_Type4 - 1 Where PackingSlip_Code_Type4 = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type5 = '', PackingSlip_Inc_Type5 = PackingSlip_Inc_Type5 - 1 Where PackingSlip_Code_Type5 = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            'Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Packing_Slip_head", "Packing_Slip_Code", Val(lbl_Company.Tag), NewCode, lbl_BaleNo.Text, Val(vOrdByNo), PkCondition_Entry, "", "", New_Entry, False, "", "", "Packing_Slip_Code, Company_IdNo, for_OrderBy", tr)


            If chk_Bale_Close_Sts.Checked = True Then
                cmd.CommandText = "Update Packing_Slip_Head set Delivery_Code = '999999/CLOSE', Delivery_No = '999999', Delivery_DetailsSlNo = 0, Delivery_Increment = 1, Delivery_Date = @RecDate Where Packing_Slip_Code = '" & Trim(NewCode) & "'  "
                cmd.ExecuteNonQuery()
            End If

            cmd.CommandText = "Delete from Packing_Slip_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Packing_Slip_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Details
                Sno = 0

                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(4).Value) <> 0 Then

                        dCloTyp_ID = Common_Procedures.ClothType_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)
                        party_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, .Rows(i).Cells(7).Value, tr)
                        dClo_ID = Clth_ID  ' Common_Procedures.Cloth_NameToIdNo(con, .Rows(i).Cells(9).Value, tr)

                        vLmNo = .Rows(i).Cells(10).Value
                        vLmIdNo = Common_Procedures.Loom_NameToIdNo(con, .Rows(i).Cells(10).Value, tr)

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into Packing_Slip_Details (   Packing_Slip_Code   ,              Company_IdNo        ,            Packing_Slip_No     ,                               for_OrderBy                                  , Packing_Slip_Date,          Cloth_IdNo      ,                  Folding           ,           Sl_No      ,                     Lot_No              ,                    Pcs_No              ,           ClothType_IdNo    ,                      Meters              ,                      Weight              ,                      Weight_Meter        ,             Party_IdNo     ,                    Lot_Code             ,             Loom_IdNo     ,          Loom_No      ) " &
                                            "          Values               ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_BaleNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_BaleRefNo.Text))) & ",    @RecDate      , " & Str(Val(dClo_ID)) & ",  " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(Sno)) & ",  '" & Trim(.Rows(i).Cells(1).Value) & "', '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(dCloTyp_ID)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & ", " & Str(Val(party_ID)) & " , '" & Trim(.Rows(i).Cells(8).Value) & "' , " & Str(Val(vLmIdNo)) & " , '" & Trim(vLmNo) & "' ) "
                        cmd.ExecuteNonQuery()

                        If dCloTyp_ID = 1 Then
                            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type1 = '" & Trim(NewCode) & "', PackingSlip_Inc_Type1 = PackingSlip_Inc_Type1 + 1 Where lot_code = '" & Trim(.Rows(i).Cells(8).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "'"
                            cmd.ExecuteNonQuery()

                        ElseIf dCloTyp_ID = 2 Then
                            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type2 = '" & Trim(NewCode) & "', PackingSlip_Inc_Type2 = PackingSlip_Inc_Type2 + 1 Where lot_code = '" & Trim(.Rows(i).Cells(8).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "'"
                            cmd.ExecuteNonQuery()

                        ElseIf dCloTyp_ID = 3 Then
                            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type3 = '" & Trim(NewCode) & "', PackingSlip_Inc_Type3 = PackingSlip_Inc_Type3 + 1 Where lot_code = '" & Trim(.Rows(i).Cells(8).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "'"
                            cmd.ExecuteNonQuery()

                        ElseIf dCloTyp_ID = 4 Then
                            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type4 = '" & Trim(NewCode) & "', PackingSlip_Inc_Type4 = PackingSlip_Inc_Type4 + 1 Where lot_code = '" & Trim(.Rows(i).Cells(8).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "'"
                            cmd.ExecuteNonQuery()

                        ElseIf dCloTyp_ID = 5 Then
                            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type5 = '" & Trim(NewCode) & "', PackingSlip_Inc_Type5 = PackingSlip_Inc_Type5 + 1 Where lot_code = '" & Trim(.Rows(i).Cells(8).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "'"
                            cmd.ExecuteNonQuery()

                        End If

                    End If

                Next

                'Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Packing_Slip_Details", "Packing_Slip_Code", Val(lbl_Company.Tag), NewCode, lbl_BaleNo.Text, Val(vOrdByNo), PkCondition_Entry, "", "", New_Entry, False, "Lot_No,Pcs_No,ClothType_IdNo,Meters,Weight,Weight_Meter,Party_IdNo,Lot_Code,Loom_IdNo,Loom_No", "Sl_No", "Packing_Slip_Code, For_OrderBy, Company_IdNo, Packing_Slip_No, Packing_Slip_Date, Ledger_Idno", tr)

            End With

            '-------------------------
            Dim stkof_idno As Integer = 0
            Dim Led_type As String = 0

            Led_type = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Val(vLed_IdNo) & ")", , tr)
            stkof_idno = 0
            If Trim(UCase(Led_type)) = "JOBWORKER" Then
                stkof_idno = vLed_IdNo
            Else
                stkof_idno = Val(Common_Procedures.CommonLedger.OwnSort_Ac)
            End If


            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Truncate table " & Trim(Common_Procedures.ReportTempTable) & ""
            cmd.ExecuteNonQuery()


            'If Common_Procedures.settings.CustomerCode <> "1391" And Common_Procedures.settings.CustomerCode <> "1155" Then
            If chk_Stk_Posting_Sts.Checked = True Then
                If Val(vTotMtrs) <> 0 Then
                    cmd.CommandText = "Insert into Stock_Cloth_Processing_Details ( Reference_Code    ,             Company_IdNo         ,              Reference_No      ,                               for_OrderBy                               ,    Reference_Date,         DeliveryTo_Idno     , ReceivedFrom_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No,          Cloth_Idno     ,                 Folding           ,             Meters_Type" & Trim(Val(Clthty_ID)) & ", UnChecked_Meters  , StockOff_IdNo ) " &
                                                                        " Values ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_BaleNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_BaleNo.Text))) & ",     @RecDate   , " & Str(Val(vGdwn_IdNo)) & ",         0        ,    ''   ,       ''     ,     ''     ,   1  , " & Str(Val(Clth_ID)) & ", " & Str(Val(txt_Folding.Text)) & ",         " & Str(Val(vTotMtrs)) & "  ,             0        ,    " & Val(stkof_idno) & "  ) "
                    cmd.ExecuteNonQuery()
                End If
            End If
            'End If
            '----------------------------

            tr.Commit()

            If SaveAll_STS <> True Then
                MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            End If

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_BaleRefNo.Text)
                End If

            Else
                move_record(lbl_BaleRefNo.Text)

            End If


        Catch ex As Exception
            tr.Rollback()
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            Dt1.Dispose()
            Da.Dispose()
            tr.Dispose()
            cmd.Dispose()

            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        End Try

    End Sub

    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotMtrs As String, TotPcs As String, TotWgt As String, tot_grs_wgt As String, tot_net_wgt As String, tot_tare_wgt As String

        If FrmLdSTS = True Then Exit Sub

        Sno = 0
        TotPcs = 0
        TotMtrs = 0
        TotWgt = 0

        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(4).Value) <> 0 Then
                    TotPcs = TotPcs + 1
                    TotMtrs = TotMtrs + Val(.Rows(i).Cells(4).Value)
                    TotWgt = TotWgt + Val(.Rows(i).Cells(5).Value)

                End If
            Next
        End With

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(2).Value = Val(TotPcs)
            .Rows(0).Cells(4).Value = Format(Val(TotMtrs), "########0.00")
            .Rows(0).Cells(5).Value = Format(Val(TotWgt), "########0.000")

        End With

        txt_net_weight.ReadOnly = False
        If Val(TotWgt) <> 0 Then
            txt_net_weight.Text = Format(Val(TotWgt), "########0.000")
            txt_net_weight.ReadOnly = True
        End If

        tot_net_wgt = Val(txt_net_weight.Text)
        tot_tare_wgt = Val(txt_Tare_weight.Text)

        txt_gross_weight.Text = Format(Val(tot_net_wgt) + Val(tot_tare_wgt), "########0.000")



    End Sub

    Private Sub Weight_Meter()
        Dim Tot_Mtr As String = 0

        lbl_wgtmtr.Text = ""
        If Val(txt_Meters.Text) <> 0 Then

            Tot_Mtr = txt_Meters.Text
            If Val(txt_Folding.Text) <> 0 Then
                Tot_Mtr = Format(Val(txt_Meters.Text) * Val(txt_Folding.Text) / 100, "######0.00")
            End If

            lbl_wgtmtr.Text = Format(Val(txt_Weight.Text) / Val(Tot_Mtr), "##########0.000")

        End If


    End Sub

    Private Sub cbo_Cloth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Cloth.GotFocus
        Dim vCondt As String = ""

        vCondt = ""
        'If Trim(UCase(vEntryType)) = "BALEGROUPWISE" Then
        '    vCondt = "(ClothSet_IdNo = " & Str(Val(vEntry_BaleGroupIdNo)) & ")"
        'End If
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", vCondt, "(Cloth_IdNo = 0)")

    End Sub

    Private Sub cbo_Cloth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Cloth.KeyDown
        Dim vCondt As String = ""

        vCondt = ""
        'If Trim(UCase(vEntryType)) = "BALEGROUPWISE" Then
        '    vCondt = "(ClothSet_IdNo = " & Str(Val(vEntry_BaleGroupIdNo)) & ")"
        'End If
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Cloth, cbo_Godown_StockIN, cbo_ClothType, "Cloth_Head", "Cloth_Name", vCondt, "(Cloth_IdNo = 0)")
        If e.KeyCode = 38 And cbo_Cloth.DroppedDown = False Or (e.Control = True And e.KeyCode = 38) Then
            If cbo_Godown_StockIN.Visible = True Then
                cbo_Godown_StockIN.Focus()
            ElseIf cbo_PartyName_StockOF.Visible = True Then
                cbo_PartyName_StockOF.Focus()
            Else
                msk_date.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_Cloth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Cloth.KeyPress
        Dim vCondt As String = ""

        vCondt = ""
        'If Trim(UCase(vEntryType)) = "BALEGROUPWISE" Then
        '    vCondt = "(ClothSet_IdNo = " & Str(Val(vEntry_BaleGroupIdNo)) & ")"
        'End If
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Cloth, cbo_ClothType, "Cloth_Head", "Cloth_Name", vCondt, "(Cloth_IdNo = 0)")
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

    Private Sub cbo_ClothType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothType_Head", "ClothType_Name", "", "ClothType_Name")

    End Sub
    Private Sub cbo_ClothType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothType, cbo_Cloth, cbo_Bale_Bundle, "ClothType_Head", "ClothType_Name", "", "ClothType_Name")

    End Sub

    Private Sub cbo_ClothType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ClothType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothType, cbo_Bale_Bundle, "ClothType_Head", "ClothType_Name", "", "ClothType_Name")
    End Sub

    Private Sub cbo_Bale_Bundle_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Bale_Bundle.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub
    Private Sub cbo_Bale_Bundle_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Bale_Bundle.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Bale_Bundle, cbo_ClothType, txt_Folding, "", "", "", "")

    End Sub

    Private Sub cbo_Bale_Bundle_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Bale_Bundle.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Bale_Bundle, txt_Folding, "", "", "", "")
    End Sub

    Private Sub txt_Folding_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Folding.KeyDown
        If e.KeyValue = 38 Then e.Handled = True : SendKeys.Send("+{TAB}")
        If e.KeyValue = 40 Then
            e.Handled = True

            If txt_LotNo.Enabled = True And txt_LotNo.Visible = True Then
                txt_LotNo.Focus()
            Else
                txt_Meters.Focus()
            End If




        End If
    End Sub

    Private Sub txt_Folding_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Folding.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            If txt_LotNo.Enabled = True And txt_LotNo.Visible = True Then
                txt_LotNo.Focus()
            Else
                txt_Meters.Focus()
            End If

        End If
    End Sub

    Private Sub txt_Note_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Note.KeyDown
        If (e.KeyValue = 40) Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If
        If (e.KeyValue = 38) Then
            txt_Tare_weight.Focus()
        End If

    End Sub

    Private Sub txt_Note_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Note.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If
        End If
    End Sub

    'Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit

    '    If dgv_Details.CurrentCell.ColumnIndex = 2 Or dgv_Details.CurrentCell.ColumnIndex = 4 Or dgv_Details.CurrentCell.ColumnIndex = 5 Then
    '        Total_Calculation()
    '    End If

    'End Sub

    'Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
    '    With dgv_Details
    '        If .CurrentCell.ColumnIndex = 6 And .CurrentCell.ColumnIndex = 5 Then
    '            If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
    '                .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
    '            End If
    '        End If
    '    End With
    'End Sub

    'Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged

    '    Try
    '        If FrmLdSTS = True Then Exit Sub
    '        With dgv_Details
    '            If .Visible Then
    '                If .Rows.Count > 0 Then
    '                    If .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 5 Then
    '                        Total_Calculation()
    '                    End If
    '                End If
    '            End If
    '        End With

    '    Catch ex As Exception
    '        '---
    '    End Try
    'End Sub



    'Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
    '    dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    'End Sub

    'Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
    '    With dgv_Details

    '        If e.KeyCode = Keys.Up Then
    '            If .CurrentCell.RowIndex = 0 Then
    '                .CurrentCell.Selected = False
    '                txt_Folding.Focus()
    '            End If
    '        End If
    '        If e.KeyCode = Keys.Left Then
    '            If .CurrentCell.RowIndex = 0 And .CurrentCell.ColumnIndex <= 0 Then
    '                .CurrentCell.Selected = False
    '                txt_Folding.Focus()
    '                'SendKeys.Send("{RIGHT}")
    '            End If
    '        End If

    '        If e.KeyCode = Keys.Enter Then
    '            e.SuppressKeyPress = True
    '            e.Handled = True

    '            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then

    '                txt_Note.Focus()

    '            Else
    '                SendKeys.Send("{Tab}")

    '            End If


    '        End If

    '    End With

    'End Sub

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

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(0).Value = i + 1
                Next

            End With


            Total_Calculation()

            txt_SlNo.Text = dgv_Details.Rows.Count + 1
            txt_LotNo.Text = ""
            txt_PcsNo.Text = ""
            cbo_Type.Text = Common_Procedures.ClothType_IdNoToName(con, 1)
            txt_Meters.Text = ""
            txt_Weight.Text = ""
            lbl_wgtmtr.Text = ""


            If txt_LotNo.Enabled And txt_LotNo.Visible Then txt_LotNo.Focus()

        End If
    End Sub

    'Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
    '    On Error Resume Next
    '    dgv_Details.CurrentCell.Selected = False

    'End Sub

    'Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
    '    Dim n As Integer

    '    With dgv_Details
    '        n = .RowCount
    '        .Rows(n - 1).Cells(0).Value = Val(n)

    '    End With
    'End Sub
    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub
    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        'With dgv_Details
        '    If .Visible Then
        '        If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Then

        '            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
        '                e.Handled = True
        '            End If

        '        End If
        '    End If
        'End With
    End Sub
    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        Pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer, i As Integer
        Dim Led_IdNo As Integer, Cnt_IdNo As Integer
        Dim Condt As String = ""
        Dim EdsCnt_IdNo As Integer, Mil_IdNo As Integer
        Dim Other_Condtn2 As String


        Dim Vstamp_idno As Integer
        Dim Vwidth As Integer


        Try

            Condt = ""
            Led_IdNo = 0
            Cnt_IdNo = 0
            Mil_IdNo = 0
            EdsCnt_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Packing_Slip_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Packing_Slip_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Packing_Slip_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_Cloth.Text) <> "" Then
                Led_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Filter_Cloth.Text)
            End If

            If Trim(cbo_Filter_ClothType.Text) <> "" Then
                Cnt_IdNo = Common_Procedures.ClothType_NameToIdNo(con, cbo_Filter_ClothType.Text)
            End If

            If Trim(cbo_Filter_Stamping.Text) <> "" Then
                Vstamp_idno = Common_Procedures.Stamping_NameToIdno(con, cbo_Filter_Stamping.Text)
            End If





            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Cloth_IdNo = " & Str(Val(Led_IdNo))
            End If

            If Val(Cnt_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.ClothType_IdNo = " & Str(Val(Cnt_IdNo))
            End If

            If Val(Vstamp_idno) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.stamping_IdNo = " & Str(Val(Vstamp_idno))
            End If


            If Trim(txt_Filter_BaleNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Packing_Slip_RefNo = '" & Trim(txt_Filter_BaleNo.Text) & "'"
            End If


            If Trim(cbo_Filter_InvNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.Packing_Slip_Code  IN (select  sq1.Packing_Slip_Code from Packing_Slip_Head sq1, ClothSales_Invoice_Head sq2 where sq2.InvoiceNo_forSelection = '" & Trim(cbo_Filter_InvNo.Text) & "' and sq1.delivery_code = sq2.ClothSales_Invoice_Code)"
            End If

            If Trim(txt_Filter_Lr_No.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "a.lr_no = '" & Trim(txt_Filter_Lr_No.Text) & "'"
            End If

            If Val(txt_Filter_Width.Text) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "b.Cloth_Width = " & Val(txt_Filter_Width.Text) & ""
            End If



            Other_Condtn2 = "(select count(tsq1.lot_code) from Packing_Slip_Details tsq1 where tsq1.Packing_Slip_Code = a.Packing_Slip_Code And tsq1.lot_code = '') > 0"
            Condt = Trim(Condt) & IIf(Trim(Condt) <> "", " and ", "") & Other_Condtn2



            da = New SqlClient.SqlDataAdapter("select a.* , b.Cloth_name from Packing_Slip_Head a Inner join Cloth_Head b on a.cloth_idno = b.cloth_idno where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Packing_Slip_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "'  and  " & Other_Condition & IIf(Trim(Condt) <> "", " and ", "") & Condt & "  Order by a.for_orderby, a.Packing_Slip_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Packing_Slip_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Packing_Slip_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Total_Pcs").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Total_Weight").ToString), "########0.000")
                    dgv_Filter_Details.Rows(n).Cells(7).Value = dt2.Rows(i).Item("Packing_Slip_RefNo").ToString

                Next i

            End If

            dt2.Clear()
            dt2.Dispose()
            da.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()

    End Sub


    Private Sub dtp_Filter_Fromdate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_Fromdate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            dtp_Filter_ToDate.Focus()
        End If

    End Sub

    Private Sub dtp_Filter_ToDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_ToDate.KeyPress

        If Asc(e.KeyChar) = 13 Then
            cbo_Filter_Cloth.Focus()
        End If
    End Sub

    Private Sub cbo_Filter_Cloth_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_Cloth.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")

    End Sub

    Private Sub cbo_Filter_Cloth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_Cloth.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Cloth, dtp_Filter_ToDate, cbo_Filter_ClothType, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")

    End Sub

    Private Sub cbo_Filter_Cloth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_Cloth.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Cloth, cbo_Filter_ClothType, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")

    End Sub

    Private Sub cbo_Filter_ClothType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_ClothType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothType_Head", "ClothType_Name", "", "ClothType_Name")

    End Sub

    Private Sub cbo_Filter_ClothType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_ClothType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ClothType, cbo_Filter_Cloth, txt_Filter_BaleNo, "ClothType_Head", "ClothType_Name", "", "ClothType_Name")

    End Sub

    Private Sub cbo_Filter_ClothType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ClothType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ClothType, txt_Filter_BaleNo, "ClothType_Head", "ClothType_Name", "", "ClothType_Name")

    End Sub
    Private Sub txt_Filter_BaleNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Filter_BaleNo.KeyDown
        If e.KeyCode = 38 Then


            cbo_Filter_ClothType.Focus()
        ElseIf e.KeyCode = 40 Then
            cbo_Filter_InvNo.Focus()

        End If
    End Sub

    Private Sub txt_Filter_BaleNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Filter_BaleNo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_Filter_InvNo.Focus()
        End If
    End Sub
    Private Sub txt_Filter_Width_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Filter_Width.KeyDown
        If e.KeyCode = 38 Then
            cbo_Filter_InvNo.Focus()
        ElseIf e.KeyCode = 40 Then
            txt_Filter_Lr_No.Focus()

        End If
    End Sub

    Private Sub txt_Filter_Width_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Filter_Width.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Filter_Lr_No.Focus()
        End If
    End Sub

    Private Sub cbo_Filter_InvNo_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Filter_InvNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_InvNo, txt_Filter_BaleNo, txt_Filter_Width, "ClothSales_Invoice_Head", "InvoiceNo_forSelection", "", "InvoiceNo_forSelection")
    End Sub

    Private Sub cbo_Filter_InvNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Filter_InvNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_InvNo, txt_Filter_Width, "ClothSales_Invoice_Head", "InvoiceNo_forSelection", "", "InvoiceNo_forSelection")
    End Sub
    Private Sub cbo_Filter_InvNov_GotFocus(sender As Object, e As EventArgs) Handles cbo_Filter_InvNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothSales_Invoice_Head", "InvoiceNo_forSelection", "", "InvoiceNo_forSelection")
    End Sub
    Private Sub txt_Filter_Lr_No_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Filter_Lr_No.KeyDown

        If e.KeyCode = 38 Then
            txt_Filter_Width.Focus()
        ElseIf e.KeyCode = 40 Then
            cbo_Filter_Stamping.Focus()
        End If

    End Sub
    Private Sub txt_Filter_Lr_No_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Filter_Lr_No.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_Filter_Stamping.Focus()
        End If
    End Sub
    Private Sub cbo_Filter_Stamping_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Filter_Stamping.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_Stamping, txt_Filter_Lr_No, btn_Filter_Show, "Stamping_Head", "Stamping_Name", "", "Stamping_Name")
    End Sub

    Private Sub cbo_Filter_Stamping_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Filter_Stamping.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_Stamping, btn_Filter_Show, "Stamping_Head", "Stamping_Name", "", "Stamping_Name")
    End Sub
    Private Sub cbo_Filter_Stamping_GotFocus(sender As Object, e As EventArgs) Handles cbo_Filter_Stamping.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Stamping_Head", "Stamping_Name", "", "Stamping_Name")
    End Sub
    Private Sub Open_FilterEntry()
        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(7).Value)

        If Val(movno) <> 0 Then
            Filter_Status = True
            move_record(movno)
            Pnl_Back.Enabled = True
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



    Private Sub btn_Print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = False
        print_record()
    End Sub

    Private Sub btn_print_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Print.Click
        Pnl_Back.Enabled = True
        pnl_Print.Visible = False
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record

        'If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Packing_slip_Entry, New_Entry) = False Then Exit Sub

        Prn_BarcodeSticker = False

        vPrnt_WeightColumn_Status = 0

        If Common_Procedures.settings.CustomerCode = "1391" Then '---- Saktidaran Spinning Mills

            Dim mymsgbox As New Tsoft_MessageBox("Select Weight Option for Packing Slip Print", "WITHOUT WEIGHT,WITH WEIGHT,CANCEL", "FOR PACKINGSLIP PRINTING...", "IF `WITH WEIGHT` is selected, Weight & Weight/Mtr columns will be printed in packing slip," & Chr(13) & "If `WITHOUT WEIGHT` is selected, Weight & Weight/Mtr columns will not be printed in packing slip", MesssageBoxIcons.Questions, 1)
            mymsgbox.ShowDialog()

            If mymsgbox.MessageBoxResult = 1 Then
                vPrnt_WeightColumn_Status = 0

            ElseIf mymsgbox.MessageBoxResult = 2 Then
                vPrnt_WeightColumn_Status = 1

            Else

                Exit Sub

            End If

            txt_PrintFrom.Text = lbl_BaleRefNo.Text
            txt_PrintTo.Text = lbl_BaleRefNo.Text
            Printing_Bale()

        Else

            pnl_Print.Visible = True
            Pnl_Back.Enabled = False
            txt_PrintFrom.Text = lbl_BaleRefNo.Text
            txt_PrintTo.Text = lbl_BaleRefNo.Text
            If txt_PrintFrom.Enabled And txt_PrintFrom.Visible Then
                txt_PrintFrom.Focus()
                txt_PrintFrom.SelectAll()
            End If

        End If

    End Sub

    Private Sub btn_Print_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Cancel.Click
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_Print_Ok_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Ok.Click
        Printing_Bale()
    End Sub

    Private Sub txt_PrintFrom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PrintFrom.KeyDown
        If e.KeyCode = Keys.Down Then
            txt_PrintTo.Focus()
        End If
    End Sub

    Private Sub txt_PrintTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PrintTo.KeyDown
        If e.KeyCode = Keys.Down Then
            btn_Print_Ok.Focus()
        End If
        If e.KeyCode = Keys.Up Then
            txt_PrintFrom.Focus()
        End If
    End Sub

    Private Sub txt_PrintTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PrintTo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            Printing_Bale()
        End If
    End Sub

    Public Sub Printing_Bale()
        Dim prtFrm As Single = 0
        Dim prtTo As Single = 0
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim Condt As String = ""
        Dim PpSzSTS As Boolean = False
        Dim ps As Printing.PaperSize

        If Val(txt_PrintFrom.Text) = 0 Then Exit Sub
        If Val(txt_PrintTo.Text) = 0 Then Exit Sub

        prtFrm = Val(Common_Procedures.OrderBy_CodeToValue(txt_PrintFrom.Text))
        prtTo = Val(Common_Procedures.OrderBy_CodeToValue(txt_PrintTo.Text))

        Condt = ""
        If Val(txt_PrintFrom.Text) <> 0 And Val(txt_PrintTo.Text) <> 0 Then
            Condt = " a.for_OrderBy between " & Str(Val(prtFrm)) & " and " & Trim(prtTo)

        ElseIf Val(txt_PrintFrom.Text) <> 0 Then
            Condt = " a.for_OrderBy = " & Str(Val(prtFrm))

        Else
            Exit Sub

        End If

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name from Packing_Slip_Head a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Packing_Slip_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and " & Other_Condition & IIf(Trim(Condt) <> "", " and ", "") & Condt, con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count <= 0 Then

                MessageBox.Show("No Entry Found", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub

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
                'e.PageSettings.PaperSize = ps
                Exit For
            End If
        Next

        PrintDocument1.DefaultPageSettings.Landscape = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1307" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1387" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1052" Then '---- '---- Sri Sugam Textile (Karumanthampatti)
            PrintDocument1.DefaultPageSettings.Landscape = True
        End If

        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then
            Try
                If Print_PDF_Status = True Then
                    '--This is actual & correct 
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
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT SHOW PRINT PREVIEW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try

        Else

            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument1

                ppd.WindowState = FormWindowState.Normal
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.ClientSize = New Size(600, 600)

                ppd.ShowDialog()
                'If PageSetupDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                '    PrintDocument1.DefaultPageSettings = PageSetupDialog1.PageSettings
                '    ppd.ShowDialog()
                'End If

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If

        Pnl_Back.Enabled = True
        pnl_Print.Visible = False

    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim prtFrm As Single = 0
        Dim prtTo As Single = 0
        Dim Condt As String = ""
        Dim Clthname As String = ""

        Dim prn_NoofBmDets As Integer
        prn_HeadIndx = 0


        If Val(txt_PrintFrom.Text) = 0 Then Exit Sub
        If Val(txt_PrintTo.Text) = 0 Then Exit Sub

        prtFrm = Val(Common_Procedures.OrderBy_CodeToValue(txt_PrintFrom.Text))
        prtTo = Val(Common_Procedures.OrderBy_CodeToValue(txt_PrintTo.Text))

        Condt = ""
        If Val(txt_PrintFrom.Text) <> 0 And Val(txt_PrintTo.Text) <> 0 Then
            Condt = " a.for_OrderBy between " & Str(Val(prtFrm)) & " and " & Trim(prtTo)

        ElseIf Val(txt_PrintFrom.Text) <> 0 Then
            Condt = " a.for_OrderBy = " & Str(Val(prtFrm))

        Else
            Exit Sub

        End If

        prn_HdDt.Clear()
        prn_DetDt.Clear()

        prn_PageNo = 0
        prn_HdIndx = 1
        prn_DetIndx = 0
        prn_HdMxIndx = 0
        prn_DetMxIndx = 0
        prn_Count = 1
        prn_DetBarCdStkr = 1

        Erase prn_DetAr
        Erase prn_DetAr1
        Erase prn_HdAr

        prn_HdAr = New String(500, 10) {}

        prn_DetAr = New String(1000, 900, 10) {}

        prn_DetAr1 = New String(2000, 10) {}

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, tZ.*, c.Cloth_Name,c.Cloth_Description from Packing_Slip_Head a INNER JOIN Company_head tZ ON a.company_idno = tZ.Company_Idno INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo Where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Packing_Slip_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "'  and " & Trim(Other_Condition) & IIf(Trim(Condt) <> "", " and ", "") & Condt & " order by a.for_orderby, a.Packing_Slip_Code", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then
                For i = 0 To prn_HdDt.Rows.Count - 1

                    prn_HdMxIndx = prn_HdMxIndx + 1

                    prn_HdAr(prn_HdMxIndx, 1) = Trim(prn_HdDt.Rows(i).Item("Packing_Slip_No").ToString)

                    If Trim(prn_HdDt.Rows(i).Item("Cloth_Description").ToString) <> "" Then
                        Clthname = Trim(prn_HdDt.Rows(i).Item("Cloth_Description").ToString)

                    Else
                        Clthname = Trim(prn_HdDt.Rows(i).Item("Cloth_Name").ToString)
                    End If

                    prn_HdAr(prn_HdMxIndx, 2) = Trim(Clthname)
                    prn_HdAr(prn_HdMxIndx, 3) = Val(prn_HdDt.Rows(i).Item("Total_Pcs").ToString)
                    prn_HdAr(prn_HdMxIndx, 4) = Format(Val(prn_HdDt.Rows(i).Item("Total_Meters").ToString), "#########0.00")
                    prn_HdAr(prn_HdMxIndx, 5) = Format(Val(prn_HdDt.Rows(i).Item("Total_Weight").ToString), "#########0.000")
                    prn_HdAr(prn_HdMxIndx, 6) = Trim(prn_HdDt.Rows(i).Item("Note").ToString)
                    prn_HdAr(prn_HdMxIndx, 7) = Format(Val(prn_HdDt.Rows(i).Item("Folding").ToString), "########0.00")
                    prn_DetMxIndx = 0






                    da2 = New SqlClient.SqlDataAdapter("select a.* from Packing_Slip_Details a where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Packing_Slip_Code = '" & Trim(prn_HdDt.Rows(i).Item("Packing_Slip_Code").ToString) & "' order by a.Sl_No", con)
                    prn_DetDt = New DataTable
                    da2.Fill(prn_DetDt)

                    If prn_DetDt.Rows.Count > 0 Then
                        For j = 0 To prn_DetDt.Rows.Count - 1
                            If Val(prn_DetDt.Rows(j).Item("Meters").ToString) <> 0 Then

                                prn_DetMxIndx = prn_DetMxIndx + 1

                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 0) = j + 1
                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 1) = Trim(prn_DetDt.Rows(j).Item("Lot_No").ToString)
                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 2) = Trim(prn_DetDt.Rows(j).Item("Pcs_No").ToString)
                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 3) = Format(Val(prn_DetDt.Rows(j).Item("Meters").ToString), "#########0.00")
                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 4) = Format(Val(prn_DetDt.Rows(j).Item("Weight").ToString), "#########0.000")
                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 5) = Format(Val(prn_DetDt.Rows(j).Item("Weight_Meter").ToString), "#########0.000")

                                If Common_Procedures.settings.CustomerCode = "1391" Then '---- Saktidaran Spinning Mills

                                    prn_DetAr1(prn_DetMxIndx, 1) = j + 1
                                    prn_DetAr1(prn_DetMxIndx, 2) = Trim(prn_DetDt.Rows(j).Item("Lot_No").ToString)
                                    prn_DetAr1(prn_DetMxIndx, 3) = Trim(prn_DetDt.Rows(j).Item("Pcs_No").ToString)
                                    prn_DetAr1(prn_DetMxIndx, 4) = Format(Val(prn_DetDt.Rows(j).Item("Meters").ToString), "#########0.00")
                                    prn_DetAr1(prn_DetMxIndx, 5) = Format(Val(prn_DetDt.Rows(j).Item("Weight").ToString), "#########0.000")
                                    prn_DetAr1(prn_DetMxIndx, 6) = Format(Val(prn_DetDt.Rows(j).Item("Weight_Meter").ToString), "#########0.000")

                                End If


                            End If
                        Next j
                    End If

                Next i

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim EntryCode As String
        vtot_pcs = 0
        vtot_wgt = 0
        lst_prnt = False

        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BaleRefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Prn_BarcodeSticker = True Then
            Printing_BarCode_Sticker_Format1(e)

        Else

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1307" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1052" Then '---- '---- Sri Sugam Textile (Karumanthampatti)
                Common_Procedures.Printing_PackingSlip_Format4(PrintDocument1, e, prn_HdDt, prn_HdMxIndx, prn_DetMxIndx, prn_HdAr, prn_DetAr, prn_PageNo, prn_Count, prn_HdIndx, prn_DetIndx)
                'Common_Procedures.Printing_PackingSlip_Format3(PrintDocument1, e, prn_HdDt, prn_HdMxIndx, prn_DetMxIndx, prn_HdAr, prn_DetAr, prn_PageNo, prn_Count, prn_HdIndx, prn_DetIndx)

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1249" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1516" Then '---- VAIPAV TEXTILES PVT LTD (SOMANUR) AND ---- VIPIN TEXTILES (SOMANUR) 
                Common_Procedures.Printing_PackingSlip_Format2(PrintDocument1, e, prn_HdDt, prn_HdMxIndx, prn_DetMxIndx, prn_HdAr, prn_DetAr, prn_PageNo, prn_Count, prn_HdIndx, prn_DetIndx)

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1266" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1516" Then '---- VAIPAV TEXTILES PVT LTD (SOMANUR) AND ---- VIPIN TEXTILES (SOMANUR) 
                Common_Procedures.Printing_PackingSlip_Format_1266(PrintDocument1, e, prn_HdDt, prn_HdMxIndx, prn_DetMxIndx, prn_HdAr, prn_DetAr, prn_PageNo, prn_Count, prn_HdIndx, prn_DetIndx)

            ElseIf Common_Procedures.settings.CustomerCode = "1391" Then '---- Saktidaran Spinning Mills
                Dim vPartyName As String = ""
                Dim vPartyCityName As String = ""
                Dim vPacking_SlipNo As String = ""
                Dim vClothSales_Inv_No As String = ""
                Dim vClothSales_Inv_Date As String = ""
                Dim vCloth_name As String = ""
                Dim vDeliveryTo_Name As String = ""
                Dim vPack_Type_Name As String = ""
                Dim vWeight_Column_Status As Integer = 0
                Dim vTot_Pcs As String = ""
                Dim vTot_Mtrs As String = ""
                Dim vTot_Wgt As String = ""
                Dim vVehicle_No As String = ""
                Dim vFold As String = ""

                vPartyName = ""
                vPartyCityName = ""
                vPack_Type_Name = prn_HdDt.Rows(0).Item("Bale_Bundle").ToString
                vClothSales_Inv_No = ""
                vClothSales_Inv_Date = Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Packing_Slip_Date").ToString), "dd-MM-yyyy").ToString
                vCloth_name = prn_HdDt.Rows(0).Item("Cloth_name").ToString

                vDeliveryTo_Name = ""

                vPacking_SlipNo = prn_HdDt.Rows(0).Item("Packing_Slip_No").ToString

                vWeight_Column_Status = vPrnt_WeightColumn_Status

                vTot_Pcs = Val(prn_HdDt.Rows(0).Item("total_pcs").ToString)
                vTot_Mtrs = Format(Val(prn_HdDt.Rows(0).Item("total_Meters").ToString), "########0.00")
                vTot_Wgt = Format(Val(prn_HdDt.Rows(0).Item("total_Weight").ToString), "########0.000")
                vVehicle_No = ""
                vFold = Format(Val(prn_HdDt.Rows(0).Item("Folding").ToString), "########0.00")

                Common_Procedures.Printing_PackingSlip_Format_1391(PrintDocument1, e, prn_HdDt, prn_DetDt, prn_DetMxIndx, prn_DetAr1, prn_PageNo, prn_Count, prn_HdIndx, prn_DetIndx, prn_NoofBmDets, vWeight_Column_Status, vPacking_SlipNo, vClothSales_Inv_No, vClothSales_Inv_Date, vPartyName, vPartyCityName, vDeliveryTo_Name, vCloth_name, vPack_Type_Name, vTot_Pcs, vTot_Mtrs, vTot_Wgt, vVehicle_No, vFold)

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1155" Then
                Common_Procedures.Printing_PackingSlip_Format_1155(PrintDocument1, e, prn_HdDt, prn_HdMxIndx, prn_DetMxIndx, prn_HdAr, prn_DetAr, prn_PageNo, prn_Count, prn_HdIndx, prn_DetIndx, vtot_wgt, vtot_pcs, lst_prnt)

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1381" Then 'KRG WEAVES
                Common_Procedures.Printing_PackingSlip_Format_1381(PrintDocument1, e, prn_HdDt, prn_HdMxIndx, prn_DetMxIndx, prn_HdAr, prn_DetAr, prn_PageNo, prn_Count, prn_HdIndx, prn_DetIndx, vtot_wgt, vtot_pcs, lst_prnt)

            Else
                Common_Procedures.Printing_PackingSlip_Format1(PrintDocument1, e, prn_HdDt, prn_HdMxIndx, prn_DetMxIndx, prn_HdAr, prn_DetAr, prn_PageNo, prn_Count, prn_HdIndx, prn_DetIndx)

            End If

        End If

    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        'If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
        '    dgv_Details_KeyUp(sender, e)
        'End If
    End Sub

    Private Sub cbo_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyName_StockOF.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'OWNSORT' or Ledger_Type = 'JOBWORKER')", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName_StockOF.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyName_StockOF, msk_date, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( Ledger_Type = 'OWNSORT' or Ledger_Type = 'JOBWORKER')", "(Ledger_IdNo = 0)")
        If e.KeyCode = 40 And cbo_PartyName_StockOF.DroppedDown = False Or (e.Control = True And e.KeyCode = 40) Then
            If cbo_Godown_StockIN.Visible = True Then
                cbo_Godown_StockIN.Focus()
            Else
                cbo_Cloth.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyName_StockOF.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyName_StockOF, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( Ledger_Type = 'OWNSORT' or Ledger_Type = 'JOBWORKER')", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If cbo_Godown_StockIN.Visible = True Then
                cbo_Godown_StockIN.Focus()
            Else
                cbo_Cloth.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_PartyName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyName_StockOF.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.MDI_LedType = "JOBWORKER"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PartyName_StockOF.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
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

        If e.KeyValue = 38 Then
            txt_Note.Focus()
        End If

        If e.KeyCode = 40 Then
            cbo_Cloth.Focus()
        End If

    End Sub

    Private Sub msk_date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_date.KeyPress
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

    Private Sub dtp_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_Date.Text = Date.Today
        End If
    End Sub


    Private Sub cbo_Godown_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Godown_StockIN.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1) and Close_status = 0 ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Godown_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Godown_StockIN.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Godown_StockIN, Nothing, cbo_Cloth, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1) and Close_status = 0 ", "(Ledger_IdNo = 0)")
        If (e.KeyValue = 38 And cbo_Godown_StockIN.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If cbo_PartyName_StockOF.Visible Then
                cbo_PartyName_StockOF.Focus()
            Else
                msk_date.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Godown_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Godown_StockIN.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Godown_StockIN, cbo_Cloth, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1) and Close_status = 0 ", "(Ledger_IdNo = 0)")
    End Sub



    Private Sub txt_BalePrefixNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_BalePrefixNo.KeyPress
        If Asc(e.KeyChar) = 34 Or Asc(e.KeyChar) = 39 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_BaleSuffixNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_BaleSuffixNo.KeyPress
        If Asc(e.KeyChar) = 34 Or Asc(e.KeyChar) = 39 Then
            e.Handled = True
        End If
    End Sub

    Private Sub btn_BarcodePrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_BarcodePrint.Click
        Common_Procedures.Print_OR_Preview_Status = 0
        Prn_BarcodeSticker = True
        Printing_BarCode_Sticker()
    End Sub


    Private Sub Printing_BarCode_Sticker()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String


        If Prn_BarcodeSticker = False Then Exit Sub


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BaleRefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("Select a.*, c.Cloth_Name from Packing_Slip_Details a INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Packing_Slip_Code = '" & Trim(NewCode) & "'", con)
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
        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1

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
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try


        Else

            Try

                Dim ppd As New PrintPreviewDialog

                ppd.Document = PrintDocument1

                ppd.WindowState = FormWindowState.Maximized
                ppd.StartPosition = FormStartPosition.CenterScreen
                ppd.PrintPreviewControl.AutoZoom = True
                ppd.PrintPreviewControl.Zoom = 1.0

                ppd.ShowDialog()

            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If

    End Sub


    Private Sub Printing_BarCode_Sticker_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim pFont As Font, BarFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim I As Integer
        Dim NoofItems_PerPage As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim CurY As Single
        Dim CurX As Single
        Dim BrCdX As Single = 20
        Dim BrCdY As Single = 100
        Dim vBarCode As String = ""
        Dim vFldMtrs As String = "", vPcs As String = ""
        Dim ItmNm1 As String, ItmNm2 As String


        Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 3.25X1.18", 325, 118)
        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
        PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
        e.PageSettings.PaperSize = pkCustomSize1

        With PrintDocument1.DefaultPageSettings.Margins
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

        NoofItems_PerPage = 2

        TxtHgt = 13.5

        Try

            If prn_HdDt.Rows.Count > 0 Then

                'NoofDets = 1
                For noofitems = 1 To NoofItems_PerPage


                    'prn_HeadIndx
                    vFldMtrs = Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Meters").ToString), "##########0.00")
                    vPcs = Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Pcs").ToString), "##########0.00")
                    'vBarCode = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Bar_Code").ToString)

                    If Val(vFldMtrs) <> 0 Then

                        'If NoofDets >= NoofItems_PerPage Then
                        '    e.HasMorePages = True
                        '    Return
                        'End If

                        CurY = TMargin

                        CurX = LMargin - 1
                        If noofitems Mod 2 = 0 Then
                            CurX = CurX + ((PageWidth + RMargin) \ 2)
                        End If

                        ItmNm1 = Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Cloth_Name").ToString)

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
                        Common_Procedures.Print_To_PrintDocument(e, "Sort : " & ItmNm1, CurX, CurY, 0, PrintWidth, pFont, , True)
                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 2
                            Common_Procedures.Print_To_PrintDocument(e, ItmNm2, CurX + 100, CurY, 1, PrintWidth, pFont, , True)
                        End If

                        pFont = New Font("Calibri", 9, FontStyle.Bold)

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "Bale No  : " & prn_HdDt.Rows(prn_HeadIndx).Item("Packing_Slip_No").ToString, CurX, CurY, 0, PrintWidth, pFont, , True)

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "Pcs          : " & vPcs, CurX, CurY, 0, PrintWidth, pFont, , True)

                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "Meters   : " & vFldMtrs, CurX, CurY, 0, PrintWidth, pFont, , True)

                        Dim vYrCode As String = ""

                        vYrCode = Microsoft.VisualBasic.Right(prn_HdDt.Rows(prn_HeadIndx).Item("Packing_Slip_Code").ToString, 5)
                        vBarCode = Microsoft.VisualBasic.Left(vYrCode, 2) & Trim(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Company_IdNo").ToString)) & Trim(UCase(prn_HdDt.Rows(prn_HeadIndx).Item("Packing_Slip_No").ToString))
                        vBarCode = "*" & Trim(UCase(vBarCode)) & "*"
                        BarFont = New Font("Free 3 of 9", 18, FontStyle.Regular)

                        CurY = CurY + TxtHgt + 5
                        e.Graphics.DrawString(Trim(vBarCode), BarFont, Brushes.Black, CurX, CurY)

                        pFont = New Font("Calibri", 14, FontStyle.Bold)
                        CurY = CurY + TxtHgt + TxtHgt - 6
                        Common_Procedures.Print_To_PrintDocument(e, Trim(vBarCode), CurX + 5, CurY, 0, PrintWidth, pFont, , True)

                        'NoofDets = NoofDets + 1

                    End If

                    'prn_DetBarCdStkr = prn_DetBarCdStkr + 1
                    'prn_DetBarCdStkr = 1
                    'prn_DetIndx = prn_DetIndx + 1


                    prn_HeadIndx = prn_HeadIndx + 1

                    If prn_HeadIndx > prn_HdDt.Rows.Count - 1 Then
                        Exit For
                    End If



                Next


            End If '' end of  If prn_HdDt.Rows.Count > 0 Then


            'prn_HeadIndx = prn_HeadIndx + 1

            If prn_HeadIndx <= prn_HdDt.Rows.Count - 1 Then
                e.HasMorePages = True
            Else
                'e.HasMorePages = False
                e.HasMorePages = False

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT BARCODE STICKER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub


    Private Sub cbo_Bale_Bundle_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Bale_Bundle.TextChanged
        If Common_Procedures.settings.CustomerCode = "1357" Then
            If cbo_Bale_Bundle.Text = "BALE" Then
                txt_Tare_weight.Text = "0.400"
            ElseIf cbo_Bale_Bundle.Text = "BUNDLE" Then
                txt_Tare_weight.Text = "1.400"
            Else

                txt_Tare_weight.Text = ""
            End If
        End If
    End Sub

    Private Sub txt_Tare_weight_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Tare_weight.KeyDown
        If e.KeyValue = 40 Then
            txt_Note.Focus()
        End If
        If e.KeyValue = 38 Then
            txt_net_weight.Focus()
        End If
    End Sub

    Private Sub txt_Tare_weight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Tare_weight.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Note.Focus()
        End If
    End Sub

    Private Sub txt_Tare_weight_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Tare_weight.TextChanged
        'If Common_Procedures.settings.CustomerCode = "1357" Then
        '    If dgv_Details_Total.Rows(0).Cells(5).Value <> 0 Then

        '        txt_gross_weight.Text = Format(Val(dgv_Details_Total.Rows(0).Cells(5).Value) + Val(txt_Tare_weight.Text), "######00.000")
        '    Else
        '        txt_gross_weight.Text = Format(Val(txt_net_weight.Text) + Val(txt_Tare_weight.Text), "######00.000")

        '    End If
        'End If

        Total_Calculation()
    End Sub

    Private Sub btn_UserModification_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            'Dim f1 As New User_Modifications
            'f1.Entry_Name = Me.Name
            'f1.Entry_PkValue = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BaleNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            'f1.ShowDialog()
        End If
    End Sub

    Private Sub btn_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Add.Click
        Dim n As Integer
        Dim MtchSTS As Boolean
        Dim itm_id As Integer = 0

        If txt_LotNo.Visible = True Then
            If Val(txt_LotNo.Text) = 0 Then
                MessageBox.Show("Invalid LotNo", "DOES NOT ADD...", MessageBoxButtons.OK)
                If txt_LotNo.Enabled Then txt_LotNo.Focus()
                Exit Sub
            End If

        Else
            txt_LotNo.Text = txt_SlNo.Text

        End If
        'If Common_Procedures.settings.CustomerCode = "1391" Then

        'txt_PcsNo.Text = txt_SlNo.Text

        'Else

        If txt_PcsNo.Visible = True Then

            If Common_Procedures.settings.CustomerCode = "1274" Then
                If Val(txt_PcsNo.Text) = 0 Then
                    txt_PcsNo.Text = txt_LotNo.Text
                End If

            ElseIf Common_Procedures.settings.CustomerCode <> "1391" Then
                If Val(txt_PcsNo.Text) = 0 Then
                    MessageBox.Show("Invalid PcsNo", "DOES NOT ADD...", MessageBoxButtons.OK)
                    If txt_PcsNo.Enabled Then txt_PcsNo.Focus()
                    Exit Sub
                End If
            End If

        Else
            txt_PcsNo.Text = txt_SlNo.Text

        End If




        ' End If



        If cbo_Type.Visible = True Then
            If Trim(cbo_Type.Text) = "" Then
                MessageBox.Show("Invalid Type", "DOES NOT ADD...", MessageBoxButtons.OK)
                If cbo_Type.Enabled Then cbo_Type.Focus()
                Exit Sub
            End If
        Else
            cbo_Type.Text = cbo_ClothType.Text ' Common_Procedures.ClothType_IdNoToName(con, 1)
        End If


        If Val(txt_Meters.Text) = 0 Then
            MessageBox.Show("Invalid Meters", "DOES NOT ADD...", MessageBoxButtons.OK)
            If txt_Meters.Enabled Then txt_Meters.Focus()
            Exit Sub
        End If

        'If Val(txt_Weight.Text) = 0 Then
        '    MessageBox.Show("Invalid weight", "DOES NOT ADD...", MessageBoxButtons.OK)
        '    If txt_Weight.Enabled Then txt_Weight.Focus()
        '    Exit Sub
        'End If

        MtchSTS = False

        With dgv_Details

            For i = 0 To .Rows.Count - 1
                If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                    .Rows(i).Cells(1).Value = txt_LotNo.Text
                    .Rows(i).Cells(2).Value = txt_PcsNo.Text
                    .Rows(n).Cells(3).Value = cbo_Type.Text
                    .Rows(i).Cells(4).Value = Format(Val(txt_Meters.Text), "########0.00")
                    .Rows(i).Cells(5).Value = Format(Val(txt_Weight.Text), "########0.000")
                    .Rows(i).Cells(6).Value = Format(Val(lbl_wgtmtr.Text), "########0.000")

                    .Rows(i).Selected = True

                    MtchSTS = True

                    If i >= 10 Then .FirstDisplayedScrollingRowIndex = i - 9

                    Exit For

                End If

            Next

            If MtchSTS = False Then

                n = .Rows.Add()
                .Rows(n).Cells(0).Value = txt_SlNo.Text
                .Rows(n).Cells(1).Value = txt_LotNo.Text
                .Rows(n).Cells(2).Value = txt_PcsNo.Text
                .Rows(n).Cells(3).Value = cbo_Type.Text
                .Rows(n).Cells(4).Value = Format(Val(txt_Meters.Text), "########0.00")
                .Rows(n).Cells(5).Value = Format(Val(txt_Weight.Text), "########0.000")
                .Rows(n).Cells(6).Value = Format(Val(lbl_wgtmtr.Text), "########0.000")


                If Common_Procedures.settings.CustomerCode = "1391" Then
                    txt_LotNo.Text = dgv_Details.Rows(n).Cells(1).Value
                    txt_LotNo.Text = dgv_Details.Rows(n).Cells(1).Value + 1
                End If


                .Rows(n).Selected = True

                If n >= 10 Then .FirstDisplayedScrollingRowIndex = n - 9

            End If

        End With

        ' GrossAmount_Calculation()
        Total_Calculation()

        txt_SlNo.Text = dgv_Details.Rows.Count + 1

        txt_PcsNo.Text = ""
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1530" Then
            txt_LotNo.Text = ""
        End If
        cbo_Type.Text = Common_Procedures.ClothType_IdNoToName(con, 1)
        txt_Meters.Text = ""
        txt_Weight.Text = ""
        lbl_wgtmtr.Text = ""

        If txt_LotNo.Enabled And txt_LotNo.Visible = True Then
            txt_LotNo.Focus()
        ElseIf txt_PcsNo.Enabled And txt_PcsNo.Visible = True Then
            txt_PcsNo.Focus()
        Else
            txt_Meters.Focus()
        End If

    End Sub

    Private Sub dgv_Details_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.DoubleClick
        If Trim(dgv_Details.CurrentRow.Cells(1).Value) <> "" Or Trim(dgv_Details.CurrentRow.Cells(4).Value) <> "" Then

            txt_SlNo.Text = Val(dgv_Details.CurrentRow.Cells(0).Value)
            txt_LotNo.Text = Trim(dgv_Details.CurrentRow.Cells(1).Value)
            txt_PcsNo.Text = Trim(dgv_Details.CurrentRow.Cells(2).Value)
            cbo_Type.Text = Trim(dgv_Details.CurrentRow.Cells(3).Value)
            txt_Meters.Text = Format(Val(dgv_Details.CurrentRow.Cells(4).Value), "########0.00")
            txt_Weight.Text = Format(Val(dgv_Details.CurrentRow.Cells(5).Value), "########0.000")
            lbl_wgtmtr.Text = Format(Val(dgv_Details.CurrentRow.Cells(6).Value), "########0.00")

            If txt_SlNo.Enabled And txt_SlNo.Visible Then txt_SlNo.Focus()

        End If
    End Sub

    Private Sub btn_Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Delete.Click
        Dim n As Integer
        Dim MtchSTS As Boolean

        MtchSTS = False

        With dgv_Details

            For i = 0 To .Rows.Count - 1
                If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                    .Rows.RemoveAt(i)

                    MtchSTS = True

                    Exit For

                End If

            Next

            If MtchSTS = True Then
                For i = 0 To .Rows.Count - 1
                    .Rows(n).Cells(0).Value = i + 1
                Next
            End If

        End With


        Total_Calculation()

        txt_SlNo.Text = dgv_Details.Rows.Count + 1
        txt_LotNo.Text = ""
        txt_PcsNo.Text = ""
        cbo_Type.Text = Common_Procedures.ClothType_IdNoToName(con, 1)
        txt_Meters.Text = ""
        txt_Weight.Text = ""
        lbl_wgtmtr.Text = ""


        If txt_LotNo.Enabled And txt_LotNo.Visible Then
            txt_LotNo.Focus()
        Else
            txt_Meters.Focus()
        End If


    End Sub
    Private Sub txt_SlNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_SlNo.KeyPress
        Dim i As Integer

        If Asc(e.KeyChar) = 13 Then

            With dgv_Details

                For i = 0 To .Rows.Count - 1
                    If Val(dgv_Details.Rows(i).Cells(0).Value) = Val(txt_SlNo.Text) Then

                        txt_LotNo.Text = Trim(.Rows(i).Cells(1).Value)
                        txt_PcsNo.Text = Trim(.Rows(i).Cells(2).Value)
                        cbo_Type.Text = Trim(.Rows(i).Cells(3).Value)
                        txt_Meters.Text = Format(Val(.Rows(i).Cells(4).Value), "########0.00")
                        txt_Weight.Text = Format(Val(.Rows(i).Cells(5).Value), "########0.000")
                        lbl_wgtmtr.Text = Format(Val(.Rows(i).Cells(6).Value), "########0.00")


                        Exit For

                    End If

                Next

            End With

            SendKeys.Send("{TAB}")

        End If
    End Sub
    Private Sub cbo_Type_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Type.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Type.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Type, txt_PcsNo, txt_Meters, "", "", "", "")
    End Sub

    Private Sub cbo_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Type.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Type, txt_Meters, "", "", "", "")

    End Sub

    Private Sub txt_LotNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_LotNo.KeyDown

        If e.KeyValue = 40 Then

            If Len(Trim(txt_LotNo.Text)) > 0 Then

                e.Handled = True
                e.SuppressKeyPress = True
                SendKeys.Send("{Tab}")

            Else
                If cbo_Stamping.Enabled And cbo_Stamping.Visible = True Then
                    e.Handled = True
                    e.SuppressKeyPress = True
                    cbo_Stamping.Focus()
                Else
                    txt_net_weight.Focus()
                End If

            End If
        End If

        If e.KeyValue = 38 Then

            e.Handled = True
            e.SuppressKeyPress = True
            '  SendKeys.Send("+{TAB}")
            cbo_Bale_Bundle.Focus()

        End If

    End Sub

    Private Sub txt_LotNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_LotNo.KeyPress

        If Asc(e.KeyChar) = 13 Then

            If Len(Trim(txt_LotNo.Text)) > 0 Then

                SendKeys.Send("{Tab}")

            Else

                If cbo_Stamping.Visible And cbo_Stamping.Enabled Then
                    cbo_Stamping.Focus()
                ElseIf txt_net_weight.Visible And txt_net_weight.Enabled Then
                    txt_net_weight.Focus()
                End If

            End If

        End If

    End Sub

    Private Sub txt_Weight_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Weight.KeyDown

        If e.KeyValue = 40 Then

            e.Handled = True
            e.SuppressKeyPress = True
            SendKeys.Send("{Tab}")
            btn_Add_Click(sender, e)

        End If

        If e.KeyValue = 38 Then

            e.Handled = True
            e.SuppressKeyPress = True
            SendKeys.Send("+{TAB}")

        End If

        'If e.KeyValue = 38 Then
        '    e.Handled = True
        '    e.SuppressKeyPress = True
        '    txt_Meters.Focus()
        'End If
        'If e.KeyValue = 40 Then
        '    e.Handled = True
        '    e.SuppressKeyPress = True
        '    btn_Add_Click(sender, e)
        'End If

    End Sub

    Private Sub txt_Weight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Weight.KeyPress
        Dim vLTNO As String, vPCNO As String
        Dim VMTRS As String, vWGT As String



        If Asc(e.KeyChar) = 13 Then

            vLTNO = ""
            vPCNO = ""
            VMTRS = ""
            vWGT = ""

            If txt_LotNo.Visible = True Then
                vLTNO = txt_LotNo.Text
            End If
            If txt_PcsNo.Visible = True Then
                vPCNO = txt_PcsNo.Text
            End If
            If txt_Meters.Visible = True Then
                VMTRS = txt_Meters.Text
            End If
            If txt_Weight.Visible = True Then
                vWGT = txt_Weight.Text
            End If

            e.Handled = True
            If Trim(vLTNO) = "" And Trim(vPCNO) = "" And Val(VMTRS) = 0 And Val(vWGT) = 0 Then
                If cbo_Stamping.Visible And cbo_Stamping.Enabled Then
                    cbo_Stamping.Focus()
                ElseIf txt_net_weight.Visible And txt_net_weight.Enabled Then
                    txt_net_weight.Focus()
                End If
            Else
                btn_Add_Click(sender, e)
            End If

        End If

    End Sub


    Private Sub txt_Weight_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Weight.TextChanged
        Weight_Meter()
    End Sub

    Private Sub txt_Meters_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Meters.KeyDown

        'If e.KeyValue = 40 Then
        '    e.Handled = True
        '    e.SuppressKeyPress = True
        '    If txt_LotNo.Visible = False Then
        '        If Val(txt_Meters.Text) = 0 Then
        '            txt_net_weight.Focus()
        '        Else
        '            txt_Weight.Focus()
        '        End If
        '    Else
        '        txt_Weight.Focus()
        '    End If
        'End If

        'If e.KeyValue = 38 Then
        '    e.Handled = True
        '    e.SuppressKeyPress = True
        '    If cbo_Type.Enabled And cbo_Type.Visible = True Then
        '        cbo_Type.Focus()
        '    Else
        '        txt_Folding.Focus()
        '    End If
        'End If


        If e.KeyValue = 40 Then

            e.Handled = True
            e.SuppressKeyPress = True
            SendKeys.Send("{Tab}")

        End If

        If e.KeyValue = 38 Then

            e.Handled = True
            e.SuppressKeyPress = True
            SendKeys.Send("+{TAB}")

        End If

    End Sub

    Private Sub txt_Meters_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Meters.KeyPress
        If Asc(e.KeyChar) = 13 Then

            SendKeys.Send("{Tab}")

            'If txt_LotNo.Visible = False Then
            '    If Val(txt_Meters.Text) = 0 Then
            '        txt_net_weight.Focus()
            '    Else
            '        txt_Weight.Focus()
            '    End If

            'Else
            '    txt_Weight.Focus()

            'End If

        End If
    End Sub

    Private Sub txt_Meters_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Meters.TextChanged
        Weight_Meter()
    End Sub

    Private Sub txt_net_weight_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_net_weight.KeyDown
        If e.KeyValue = 40 Then
            txt_Tare_weight.Focus()
        End If
        If e.KeyValue = 38 Then

            If msk_LRDate.Enabled And msk_LRDate.Visible = True Then
                msk_LRDate.Focus()

            ElseIf txt_LotNo.Enabled And txt_LotNo.Visible = True Then
                txt_LotNo.Focus()

            Else
                txt_Meters.Focus()
            End If

        End If
    End Sub


    Private Sub txt_net_weight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_net_weight.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_Tare_weight.Focus()
        End If
    End Sub

    Private Sub ComPort_Open()
        Dim ServerName() As String

        Try

            Common_Procedures.SerialPort1.Dispose()
            Common_Procedures.SerialPort1 = New SerialPort
            Threading.Thread.Sleep(100)

            'If Not Common_Procedures.SerialPort1.IsOpen Then
            Common_Procedures.SerialPort1.PortName = "COM" + DefaultPortNo
            Common_Procedures.SerialPort1.BaudRate = 9600
            Common_Procedures.SerialPort1.Parity = IO.Ports.Parity.None
            Common_Procedures.SerialPort1.StopBits = IO.Ports.StopBits.One
            Common_Procedures.SerialPort1.DataBits = 8

            Common_Procedures.SerialPort1.Open()

            AddHandler Common_Procedures.SerialPort1.DataReceived, New SerialDataReceivedEventHandler(AddressOf DataReceivedHandler)

            'MessageBox.Show("PORT OPENED (COM" + DefaultPortNo.ToString & ")")
            PrtOp_sts = True
            'lbl_PortConnection.Text = "Port Connected"
            'lbl_PortConnection.ForeColor = Color.LimeGreen
            PrtOp_Inc = 0

            Pnl_Back.Enabled = True
            dgv_Filter_Details.Enabled = True

            'Else

            '    If Common_Procedures.SerialPort1.IsOpen Then
            '        Common_Procedures.SerialPort1.Dispose()
            '        Common_Procedures.SerialPort1 = New SerialPort
            '        Threading.Thread.Sleep(5000)
            '    End If

            '    AddHandler Common_Procedures.SerialPort1.DataReceived, New SerialDataReceivedEventHandler(AddressOf DataReceivedHandler)

            '    lbl_PortConnection.Text = "Port Connected."

            'End If


        Catch ex As Exception

            PrtOp_Inc = PrtOp_Inc + 1
            If PrtOp_Inc <= 5 Then
                ComPort_Open()
            Else

                lbl_PortConnection.ForeColor = Color.Red
                lbl_PortConnection.Text = "Port Not Found"

                ServerName = Split(Common_Procedures.ServerName, "\")
                If Val(Common_Procedures.User.IdNo) <> 1 And Trim(UCase(ServerName(0))) = Trim(UCase(Environment.MachineName)) And Common_Procedures.is_OfficeSystem() = False Then
                    PrtOp_sts = False
                End If

            End If

            'lbl_PortStatus.Text = "UNABLE TO OPEN PORT COM" & DefaultPortNo
            'MsgBox(ex.Message)

        End Try
    End Sub

    Private Sub btn_Open_Port_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Open_Port.Click
        ComPort_Open()
    End Sub

    Private Sub btn_Close_Port_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close_Port.Click
        ComPort_Close()
    End Sub

    Private Sub DataReceivedHandler(ByVal sender As Object, ByVal e As SerialDataReceivedEventArgs)
        Dim i As Long = 0

        Try


            System.Threading.Thread.Sleep(50)

            Dim sp As SerialPort = CType(sender, SerialPort)

            Dim indata As String = sp.ReadExisting()

            If Len(Trim(indata)) > 1 Then
                UpdateTextBox(indata)
            End If

        Catch ex As Exception
            '------

        End Try

    End Sub

    Private Sub UpdateTextBox(ByVal wgt As String)

        Try

            Dim i As Integer = 0
            Dim k As Integer = 0
            Dim MachWgt As String = ""
            Dim valsts As Boolean = False

            If Me.InvokeRequired Then
                Dim args() As String = {wgt}
                Me.BeginInvoke(New Action(Of String)(AddressOf UpdateTextBox), args)
                Return
            End If

            k = InStr(1, wgt, ":")

            MachWgt = ""
            valsts = False
            For i = k + 1 To Len(Trim(wgt))
                If valsts = False Then
                    If Val(Mid(wgt, i, 1)) <> 0 Then
                        MachWgt = MachWgt & Mid(wgt, i, 1)
                        valsts = True
                    Else
                        MachWgt = 0
                    End If

                Else
                    If Mid(wgt, i, 1) <> " " Then
                        MachWgt = MachWgt & Mid(wgt, i, 1)
                    Else
                        Exit For
                    End If

                End If

            Next

            If Val(MachWgt) >= 0.01 Then
                txt_Weight.Text = Val(MachWgt) ' wgt
            Else
                If Val(MachWgt) = Val(PrevMachWgt1) And Val(MachWgt) = Val(PrevMachWgt2) Then
                    txt_Weight.Text = 0
                End If

                'If Val(MachWgt) = Val(PrevMachWgt1) And Val(MachWgt) = Val(PrevMachWgt2) And Val(MachWgt) = Val(PrevMachWgt3) Then
                '    txt_Weight.Text = 0
                'End If

            End If

            PrevMachWgt3 = PrevMachWgt2
            PrevMachWgt2 = PrevMachWgt1
            PrevMachWgt1 = MachWgt

        Catch ex As Exception
            '------

        End Try

    End Sub

    Function ReceivedBytes_To_Text(ByVal buf() As Byte) As String
        Dim s As String = ""

        ReceivedBytes_To_Text = ""

        Try


            'If (radASCII.Checked) Then
            s = System.Text.Encoding.ASCII.GetString(buf)
            'ElseIf (radHex.Checked) Then
            's = BitConverter.ToString(buf)
            'ElseIf (radUnicode.Checked) Then
            's = System.Text.Encoding.Unicode.GetString(buf)
            'End If

            ReceivedBytes_To_Text = s

        Catch ex As Exception
            '------

        End Try

    End Function

    Private Sub ComPort_Close()
        Try

            If Common_Procedures.SerialPort1.IsOpen Then

                Common_Procedures.SerialPort1.Close()
                Threading.Thread.Sleep(1000)

                Common_Procedures.SerialPort1.Dispose()
                Threading.Thread.Sleep(100)

            End If

        Catch ex As Exception
            '----

        End Try

    End Sub

    Private Sub btn_SaveAll_Click(sender As System.Object, e As System.EventArgs) Handles btn_SaveAll.Click
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

        LastNo = lbl_BaleRefNo.Text

        movefirst_record()
        Timer1.Enabled = True

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        save_record()
        If Trim(UCase(LastNo)) = Trim(UCase(lbl_BaleRefNo.Text)) Then
            Timer1.Enabled = False
            SaveAll_STS = False
            MessageBox.Show("All entries saved sucessfully", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Else
            movenext_record()

        End If
    End Sub

    Private Sub Printing_Format1_Excel()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim Indx As Integer = 0
        Dim RndOffAmt As String = ""
        Dim NtAmt As String = ""
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim AmtInWrds As String
        Dim FlName1 As String = "", FlName2 As String = ""
        Dim NewCode As String = ""
        Dim n As Integer = 0
        Dim DetRwsCnt As Integer = 0
        Dim xlCurRow As Integer = 0
        Dim xlApp As Excel.Application = New Microsoft.Office.Interop.Excel.Application()

        Dim prtFrm As Single = 0
        Dim prtTo As Single = 0
        Dim Condt As String = ""
        Dim Clthname As String = ""

        Dim prn_NoofBmDets As Integer
        prn_HeadIndx = 0


        If Val(txt_PrintFrom.Text) = 0 Then Exit Sub
        If Val(txt_PrintTo.Text) = 0 Then Exit Sub

        prtFrm = Val(Common_Procedures.OrderBy_CodeToValue(txt_PrintFrom.Text))
        prtTo = Val(Common_Procedures.OrderBy_CodeToValue(txt_PrintTo.Text))

        Condt = ""
        If Val(txt_PrintFrom.Text) <> 0 And Val(txt_PrintTo.Text) <> 0 Then
            Condt = " a.for_OrderBy between " & Str(Val(prtFrm)) & " and " & Trim(prtTo)

        ElseIf Val(txt_PrintFrom.Text) <> 0 Then
            Condt = " a.for_OrderBy = " & Str(Val(prtFrm))

        Else
            Exit Sub

        End If















        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_meters = 0
        prn_Pcs = 0
        prn_PageNo = 0
        prn_HdIndx = 1
        prn_DetIndx = 0
        prn_HdMxIndx = 0
        prn_DetMxIndx = 0
        prn_Count = 1
        prn_TotalBales = 0
        prn_TotalPcs = 0
        prn_TotalMtrs = 0
        prn_TotalWgt = 0
        Erase prn_DetAr

        Erase prn_HdAr

        prn_HdAr = New String(500, 500) {}

        prn_DetAr = New String(500, 500, 10) {}

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BaleNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*,a.Packing_Slip_No, a.cloth_idno, a.Total_Pcs as Pak_Pcs, a.Packing_Slip_Code, a.Total_Weight, a.Total_Meters as Pak_Mtrs, tZ.*, c.Cloth_Name,c.Cloth_Description from Packing_Slip_Head a INNER JOIN Company_head tZ ON a.company_idno = tZ.Company_Idno INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo Where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Packing_Slip_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "'  and " & Trim(Other_Condition) & IIf(Trim(Condt) <> "", " and ", "") & Condt & " order by a.for_orderby, a.Packing_Slip_Code", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then
                For i = 0 To prn_HdDt.Rows.Count - 1

                    prn_HdMxIndx = prn_HdMxIndx + 1

                    prn_HdAr(prn_HdMxIndx, 1) = Trim(prn_HdDt.Rows(i).Item("Packing_Slip_No").ToString)
                    prn_HdAr(prn_HdMxIndx, 2) = Trim(prn_HdDt.Rows(i).Item("Cloth_Name").ToString)
                    prn_HdAr(prn_HdMxIndx, 3) = Val(prn_HdDt.Rows(i).Item("Pak_Pcs").ToString)
                    prn_HdAr(prn_HdMxIndx, 4) = Format(Val(prn_HdDt.Rows(i).Item("Pak_Mtrs").ToString), "#########0.00")

                    prn_TotalBales = prn_TotalBales + 1

                    prn_meters = prn_meters + Val(prn_HdDt.Rows(i).Item("Pak_Mtrs").ToString)

                    prn_Pcs = prn_Pcs + Val(prn_HdDt.Rows(i).Item("Pak_Pcs").ToString)

                    prn_DetMxIndx = 0

                    da2 = New SqlClient.SqlDataAdapter("select a.* from Packing_Slip_Details a where a.Packing_Slip_Code = '" & Trim(prn_HdDt.Rows(i).Item("Packing_Slip_Code").ToString) & "' order by a.Sl_No", con)
                    prn_DetDt = New DataTable
                    da2.Fill(prn_DetDt)

                    If prn_DetDt.Rows.Count > 0 Then
                        For j = 0 To prn_DetDt.Rows.Count - 1
                            If Val(prn_DetDt.Rows(j).Item("Meters").ToString) <> 0 Then
                                prn_DetMxIndx = prn_DetMxIndx + 1
                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 5) = Trim(prn_HdDt.Rows(i).Item("Packing_Slip_No").ToString)
                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 0) = Trim(prn_DetDt.Rows(j).Item("Sl_No").ToString)
                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 1) = Trim(prn_DetDt.Rows(j).Item("Lot_No").ToString)
                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 2) = Trim(prn_DetDt.Rows(j).Item("Pcs_No").ToString)
                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 3) = Format(Val(prn_DetDt.Rows(j).Item("Meters").ToString), "#########0.00")
                                prn_DetAr(prn_HdMxIndx, prn_DetMxIndx, 4) = Format(Val(prn_DetDt.Rows(j).Item("Weight").ToString), "#########0.00")

                                prn_TotalPcs = Val(prn_TotalPcs) + 1
                                prn_TotalMtrs = Format(Val(prn_TotalMtrs) + Format(Val(prn_DetDt.Rows(j).Item("Meters").ToString), "#########0.00"), "##########0.00")
                                prn_TotalWgt = Format(Val(prn_TotalWgt) + Format(Val(prn_DetDt.Rows(j).Item("Weight").ToString), "#########0.000"), "##########0.000")

                            End If
                        Next j
                    End If

                Next i

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try











        'da1 = New SqlClient.SqlDataAdapter("select a.*, tZ.*, c.Cloth_Name,c.Cloth_Description from Packing_Slip_Head a INNER JOIN Company_head tZ ON a.company_idno = tZ.Company_Idno INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo Where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Packing_Slip_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "'  and " & Trim(Other_Condition) & " order by a.for_orderby, a.Packing_Slip_Code", Con)
        'prn_HdDt = New DataTable
        'da1.Fill(prn_HdDt)
        If prn_HdDt.Rows.Count > 0 Then

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

            'FlName1 = Microsoft.VisualBasic.Left(Trim(Common_Procedures.AppPath), 2) & "\Packing_excel.xlsx"


            FlName1 = Trim(Common_Procedures.AppPath) & "\Packing_List_excel_Copy.xlsx"


            Dim xlWorkBook As Excel.Workbook
            Dim xlWorkSheet As Excel.Worksheet

            Dim misValue As Object = System.Reflection.Missing.Value


            FlName2 = Trim(FlName2)

            If File.Exists(FlName1) = False Then
                MessageBox.Show("Invalid  " & Chr(13) & FlName1, "DOES NOT SHOW REPORT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Exit Sub
            End If


            SaveFileDialog1.ShowDialog()
            FlName2 = SaveFileDialog1.FileName


            FlName2 = Trim(FlName2) & ".xlsx"

            For Each workbook In xlApp.Workbooks
                If workbook.Name.ToString.ToLower = FlName2.ToString.ToLower Then
                    MsgBox("Close the Excell Workbook Named " & FlName2 & ". It needs to be closed to proceed further")
                    Exit Sub
                End If
            Next

            If File.Exists(FlName2) = True Then
                File.Delete(FlName2)
            End If

            File.Copy(FlName1, FlName2, True)



            xlWorkBook = xlApp.Workbooks.Open(FlName2)
            'xlWorkBook = xlApp.Workbooks.Add(misValue)
            xlWorkSheet = xlWorkBook.Sheets(1)

            Try


                xlCurRow = 2
                xlWorkSheet.Cells(xlCurRow, 1) = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)
                xlWorkSheet.Cells((xlCurRow + 1), 1) = "PACKING LIST"
                'xlWorkSheet.Cells(xlCurRow + 3, 1) = "PARTY NAME : " & Trim(prn_HdDt.Rows(0).Item("Ledger_name").ToString)

                ' xlWorkSheet.Cells(xlCurRow + 3, 7) = "INVOICE No. : " & Trim(prn_HdDt.Rows(0).Item("ClothSales_Invoice_RefNo").ToString)
                'xlWorkSheet.Cells(xlCurRow + 4, 7) = "INVOICE DATE. : " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("ClothSales_Invoice_Date").ToString), "dd-MM-yyyy").ToString
                'xlWorkSheet.Cells(xlCurRow + 5, 7) = "FOLDING : " & Trim(prn_HdDt.Rows(0).Item("Folding").ToString)
                xlWorkSheet.Cells(xlCurRow + 3, 1) = "CLOTHNAME : " & Trim(prn_HdDt.Rows(0).Item("Cloth_name").ToString)


                xlWorkSheet.Cells(xlCurRow + 4, 1) = "TOTAL BALES : " & prn_TotalBales
                xlWorkSheet.Cells(xlCurRow + 5, 1) = "TOTAL METERS : " & prn_meters

                xlWorkSheet.Cells(xlCurRow + 6, 1) = "TOTAL PIECES : " & prn_Pcs




                xlCurRow = 11

                'da1 = New SqlClient.SqlDataAdapter("select a.* from Packing_Slip_Details a where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Packing_Slip_Code = '" & Trim(prn_HdDt.Rows(0).Item("Packing_Slip_Code").ToString) & "' order by a.Sl_No", Con)
                'prn_DetDt = New DataTable
                'da1.Fill(prn_DetDt)

                'If prn_DetDt.Rows.Count > 0 Then






                Do While prn_HdIndx <= prn_HdMxIndx

                    prn_DetIndx = prn_DetIndx + 1

                    xlCurRow = xlCurRow + 1
                    xlWorkSheet.Cells(xlCurRow, 1) = Val(prn_HdIndx).ToString


                    xlWorkSheet.Cells(xlCurRow, 2) = Trim(prn_HdAr(prn_HdIndx, 1))
                    xlWorkSheet.Cells(xlCurRow, 3) = Trim(prn_HdAr(prn_HdIndx, 3))
                    xlWorkSheet.Cells(xlCurRow, 4) = Trim(prn_HdAr(prn_HdIndx, 4))

                    xlWorkSheet.Cells(xlCurRow, 5) = Val(prn_DetAr(prn_HdIndx, 1, 3))

                    xlWorkSheet.Cells(xlCurRow, 6) = Val(prn_DetAr(prn_HdIndx, 2, 3))

                    xlWorkSheet.Cells(xlCurRow, 7) = Val(prn_DetAr(prn_HdIndx, 3, 3))

                    xlWorkSheet.Cells(xlCurRow, 8) = Val(prn_DetAr(prn_HdIndx, 4, 3))

                    xlWorkSheet.Cells(xlCurRow, 9) = Val(prn_DetAr(prn_HdIndx, 5, 3))

                    xlWorkSheet.Cells(xlCurRow, 10) = Val(prn_DetAr(prn_HdIndx, 6, 3))

                    xlWorkSheet.Cells(xlCurRow, 11) = Val(prn_DetAr(prn_HdIndx, 7, 3))

                    prn_HdIndx = prn_HdIndx + 1
                    prn_Count = prn_Count + 1
                    prn_DetIndx = 0
                Loop

                'End If

                DetRwsCnt = 0



                xlWorkBook.Save()



            Catch ex As Exception
                MessageBox.Show(ex.Message, "DOES NOT PRINT INVOICE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            Finally

            End Try

        End If


        da1.Dispose()
    End Sub



    Private Sub btn_excel_Click(sender As Object, e As EventArgs) Handles btn_excel.Click
        Printing_Format1_Excel()
        btn_print_Close_Click(sender, e)
    End Sub

    Private Sub btn_PDF_Click(sender As Object, e As EventArgs) Handles btn_PDF.Click
        'PrintDocument1.DocumentName = "Packing Slip"
        'PrintDocument1.PrinterSettings.PrinterName = "doPDF v7"
        'PrintDocument1.PrinterSettings.PrintFileName = "c:\Packing_slip.pdf"
        'PrintDocument1.Print()
        Common_Procedures.Print_OR_Preview_Status = 1
        Print_PDF_Status = True
        print_record()

    End Sub

    Private Sub txt_net_weight_TextChanged(sender As Object, e As EventArgs) Handles txt_net_weight.TextChanged
        Total_Calculation()
    End Sub

    Private Sub txt_gross_weight_TextChanged(sender As Object, e As EventArgs) Handles txt_gross_weight.TextChanged
        Total_Calculation()
    End Sub


    Private Sub msk_LRDate_KeyDown(sender As Object, e As KeyEventArgs) Handles msk_LRDate.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_date.Text
            vmskSelStrt = msk_date.SelectionStart
        End If

        If e.KeyValue = 38 Then
            txt_LR_No.Focus()
        End If
        If e.KeyValue = 40 Then
            txt_net_weight.Focus()

        End If
    End Sub

    Private Sub msk_LRDate_KeyPress(sender As Object, e As KeyPressEventArgs) Handles msk_LRDate.KeyPress

        If Asc(e.KeyChar) = 13 Then
            txt_net_weight.Focus()
        End If

        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_LRDate.Text = Date.Today
            msk_LRDate.SelectionStart = 0
        End If
    End Sub

    Private Sub msk_LRDate_KeyUp(sender As Object, e As KeyEventArgs) Handles msk_LRDate.KeyUp

        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    msk_date.Text = Date.Today
        'End If
        If e.KeyCode = 107 Then
            msk_LRDate.Text = DateAdd("D", 1, Convert.ToDateTime(msk_LRDate.Text))
        ElseIf e.KeyCode = 109 Then
            msk_LRDate.Text = DateAdd("D", -1, Convert.ToDateTime(msk_LRDate.Text))
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then

            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)

        End If

    End Sub

    Private Sub msk_LRDate_LostFocus(sender As Object, e As EventArgs) Handles msk_LRDate.LostFocus

        If IsDate(msk_LRDate.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_LRDate.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_LRDate.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_LRDate.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_LRDate.Text)) >= 2000 Then
                    dtp_LR_Date.Value = Convert.ToDateTime(msk_LRDate.Text)
                End If
            End If

        End If

    End Sub

    Private Sub cbo_Stamping_Enter(sender As Object, e As EventArgs) Handles cbo_Stamping.Enter
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Stamping_Head", "Stamping_Name", "", "Stamping_IdNo=0")
    End Sub

    Private Sub cbo_Stamping_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Stamping.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Stamping, txt_LotNo, cbo_Despatch_To, "Stamping_Head", "Stamping_Name", "", "Stamping_IdNo=0")
    End Sub

    Private Sub cbo_Stamping_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Stamping.KeyPress
        'Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Stamping, cbo_Despatch_To, "Cloth_Head", "Cloth_Name", "", "Cloth_IdNo=0")
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Stamping, cbo_Despatch_To, "Stamping_Head", "Stamping_Name", "", "Stamping_IdNo=0")
    End Sub

    Private Sub cbo_Cloth_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_Cloth.SelectedIndexChanged

    End Sub

    Private Sub cbo_Stamping_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_Stamping.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            'Dim f As New Cloth_Creation
            Dim f As New Stamping_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Stamping.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""
            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Despatch_To_Enter(sender As Object, e As EventArgs) Handles cbo_Despatch_To.Enter
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Area_Head", "Area_Name", "", "Area_IdNo=0")
    End Sub



    Private Sub cbo_Despatch_To_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Despatch_To.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Despatch_To, cbo_Stamping, cbo_Transport, "Area_Head", "Area_Name", "", "Area_IdNo=0")
    End Sub

    Private Sub cbo_Despatch_To_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Despatch_To.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Despatch_To, cbo_Transport, "Area_Head", "Area_Name", "", "Area_IdNo=0")
    End Sub


    Private Sub cbo_Despatch_To_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_Despatch_To.KeyUp

        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Area_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Despatch_To.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""
            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub


    Private Sub cbo_Transport_Enter(sender As Object, e As EventArgs) Handles cbo_Transport.Enter
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_Head", "Ledger_Name", "Ledger_Type = 'TRANSPORTER'", "Ledger_IdNo=0")
    End Sub

    Private Sub cbo_Transport_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Transport.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, cbo_Despatch_To, txt_LR_No, "Ledger_Head", "Ledger_Name", "Ledger_Type = 'TRANSPORTER'", "Ledger_IdNo=0")
    End Sub

    Private Sub cbo_Transport_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Transport.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, txt_LR_No, "Ledger_Head", "Ledger_Name", "Ledger_Type = 'TRANSPORTER'", "Ledger_IdNo=0")
    End Sub

    Private Sub cbo_Transport_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_Transport.KeyUp

        If e.Control = False And e.KeyValue = 17 Then

            Common_Procedures.MDI_LedType = "TRANSPORTER"

            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Transport.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""
            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub
    Private Sub btn_OpenRecord_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_OpenRecord.Click
        Pnl_Back.Enabled = True
        pnl_OpenRecord.Visible = False
        FindRecord()
    End Sub

    Private Sub btn_CloseOpenRecord_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_CloseOpenRecord.Click
        Pnl_Back.Enabled = True
        pnl_OpenRecord.Visible = False
    End Sub

    Private Sub txt_LrNo_Open_Open_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_LrNo_Open.KeyPress
        If Asc(e.KeyChar) = 13 Then
            Pnl_Back.Enabled = True
            pnl_OpenRecord.Visible = False
            FindRecord()
        End If
    End Sub
    Private Sub FindRecord()
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String


        Try

            If Trim(txt_LrNo_Open.Text) <> "" Then
                InvCode = ""

                inpno = Trim(txt_LrNo_Open.Text)

                InvCode = Trim(PkCondition_Entry) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

                Da = New SqlClient.SqlDataAdapter("select Packing_Slip_RefNo from Packing_Slip_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and LR_No = '" & Trim(inpno) & "'  and Packing_Slip_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' ", con)
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
                    MessageBox.Show("LR No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                End If
            Else

                If Trim(txt_LrNo_Open.Text) = "" Then
                    MessageBox.Show("Invalid Lr No... ", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
                End If

            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try


    End Sub

    Private Sub btn_Find_Click(sender As Object, e As EventArgs) Handles btn_LrNo_Find.Click

        Pnl_Back.Enabled = False
        pnl_OpenRecord.Visible = True

        pnl_OpenRecord.BringToFront()
        txt_LrNo_Open.Text = ""
        txt_LrNo_Open.Focus()


    End Sub

    Private Sub btn_Close_PrintRange_Click(sender As Object, e As EventArgs) Handles btn_Close_PrintRange.Click
        Pnl_Back.Enabled = True
        pnl_OpenRecord.Visible = False
    End Sub

    Private Sub txt_LR_No_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_LR_No.KeyDown
        If e.KeyCode = 38 Then
            cbo_Transport.Focus()
        ElseIf e.KeyCode = 40 Then
            msk_LRDate.Focus()

        End If
    End Sub

    Private Sub txt_LR_No_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_LR_No.KeyPress
        If Asc(e.KeyChar) = 13 Then
            msk_LRDate.Focus()
        End If
    End Sub

End Class