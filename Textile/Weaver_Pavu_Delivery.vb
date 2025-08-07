Imports System.Drawing.Printing
Imports System.IO

Public Class Weaver_Pavu_Delivery
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "WPVDC-"
    Private Pk_Condition1 As String = "WPDCF-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_EmptyBeamDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_RequirementDetails As New DataGridViewTextBoxEditingControl
    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private prn_TotCopies As Integer = 0
    Private prn_Count As Integer = 0
    Private LastNo As String = ""
    Private PrntCnt2ndPageSTS As Boolean = False
    Private prn_DetDt1 As New DataTable
    Private prn_PageNo1 As Integer
    Private prn_DetIndx1 As Integer
    Private prn_NoofBmDets1 As Integer
    Private prn_DetSNo1 As Integer
    Private prn_Prev_HeadIndx As Integer

    Private GrossWt As Integer = 0
    Private NetWt As Integer = 0
    Private WarpWt As Integer = 0


    Private SaveAll_STS As Boolean = False

    Public vmskOldText As String = ""

    Public vmskSelStrt As Integer = -1
    Private dgv_ActCtrlName As String = ""
    Private prn_HeadIndx As Integer


    Private Prnt_HalfSheet_STS As Boolean = False
    Private vPrnt_2Copy_In_SinglePage As Integer = 0
    Public VEndsCountTag As String

    Private vCLO_CONDT As String = ""
    Private FnYearCode1 As String = ""
    Private FnYearCode2 As String = ""

    Private Sub clear()
        New_Entry = False
        Insert_Entry = False
        chk_SelectAll.Checked = False

        chk_Verified_Status.Checked = False
        Prnt_HalfSheet_STS = False

        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False
        pnl_EmptyBeamDetails.Visible = False
        Grp_EWB.Visible = False


        pnl_OwnOrderSelection.Visible = False
        lbl_DcNo.Text = ""
        lbl_DcNo.ForeColor = Color.Black
        cbo_DCSufixNo.Text = ""
        msk_date.Text = ""

        msk_date.Enabled = True
        dtp_Date.Text = ""
        txt_KuraiPavuBeam.Text = ""
        txt_KuraiPavuMeter.Text = ""
        txt_Freight.Text = ""
        txt_Ewave_Bill_No.Text = ""
        txt_Value.Text = ""
        If cbo_WidthType.Visible Then cbo_WidthType.Text = ""
        cbo_ClothName.Text = ""
        txt_CrimpPerc.Text = ""
        txt_Party_DcNo.Text = ""
        lbl_pavu_weight.Text = ""
        cbo_DelvAt.Text = ""
        cbo_DelvAt.Enabled = True
        cbo_EndsCount.Text = ""
        VEndsCountTag = ""
        cbo_Sales_OrderCode_forSelection.Text = ""

        cbo_Transport.Text = ""
        cbo_VehicleNo.Text = ""
        cbo_RecForm.Text = ""
        lbl_Freight_Pavu.Text = ""
        lbl_Amount.Text = ""
        lbl_OrderCode.Text = ""
        lbl_OrderNo.Text = ""
        txt_rate.Text = ""

        lbl_UserName_CreatedBy.Text = ""
        lbl_UserName_ModifiedBy.Text = ""
        cbo_TransportMode.Text = "BY ROAD"
        txt_DateTime_Of_Supply.Text = ""
        txt_place_Supply.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        dgv_PavuDetails.ReadOnly = True
        dgv_PavuDetails.Rows.Clear()
        dgv_PavuDetails_Total.Rows.Clear()
        dgv_PavuDetails_Total.Rows.Add()
        txt_DCPrefixNo.Text = ""

        If cbo_weaving_job_no.Visible Then cbo_weaving_job_no.Text = ""


        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""
            cbo_Filter_EndsCountName.Text = ""

            dgv_Filter_Details.Rows.Clear()
        End If
        dgv_ActCtrlName = ""

        chk_Loaded.Checked = False
        chk_Loaded.Visible = False

        txt_JO_Date.Text = ""
        txt_JO_No.Text = ""
        txt_Ref_date.Text = ""
        txt_Ref_No.Text = ""
        dtp_Date.Enabled = True
        dtp_Date.BackColor = Color.White

        cbo_DelvAt.Enabled = True
        cbo_DelvAt.BackColor = Color.White

        cbo_RecForm.Enabled = True
        cbo_RecForm.BackColor = Color.White

        cbo_Transport.Enabled = True
        cbo_Transport.BackColor = Color.White

        txt_Party_DcNo.Enabled = True
        txt_Party_DcNo.BackColor = Color.White
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT TEXTILES (SOMANUR)
            txt_Party_DcNo.Enabled = False
        End If

        txt_KuraiPavuBeam.Enabled = True
        txt_KuraiPavuBeam.BackColor = Color.White

        txt_KuraiPavuMeter.Enabled = True
        txt_KuraiPavuMeter.BackColor = Color.White

        msk_date.Enabled = True
        msk_date.BackColor = Color.White

        cbo_WidthType.Enabled = True
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT TEXTILES (SOMANUR)
            cbo_WidthType.Enabled = False
        End If

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


        If Me.ActiveControl.Name <> dgv_PavuDetails_Total.Name Then
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
        If Not IsNothing(dgv_PavuDetails.CurrentCell) Then dgv_PavuDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_PavuDetails_Total.CurrentCell) Then dgv_PavuDetails_Total.CurrentCell.Selected = False
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
        If Not IsNothing(dgv_PavuDetails.CurrentCell) Then dgv_PavuDetails.CurrentCell.Selected = False
        If Not IsNothing(dgv_PavuDetails_Total.CurrentCell) Then dgv_PavuDetails_Total.CurrentCell.Selected = False
        If Not IsNothing(dgv_Filter_Details.CurrentCell) Then dgv_Filter_Details.CurrentCell.Selected = False
    End Sub

    Private Sub Weaver_Pavu_Delivery_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_DelvAt.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_DelvAt.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_EndsCount.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "ENDSCOUNT" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_EndsCount.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Transport.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Transport.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_RecForm.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_RecForm.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

                Me.Text = lbl_Heading.Text & "  -  " & lbl_Company.Text

                new_record()

            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False

    End Sub

    Private Sub Weaver_Pavu_Delivery_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Weaver_Pavu_Delivery_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Text = ""

        con.Open()

        lbl_IONo_Caption.Visible = False
        lbl_OrderNo.Visible = False
        btn_OwnOrderSelection.Visible = False
        If Common_Procedures.settings.Internal_Order_Entry_Status = 1 Then
            lbl_IONo_Caption.Visible = True
            lbl_OrderNo.Visible = True
            btn_OwnOrderSelection.Visible = True
        End If

        lbl_IONo_Caption.Visible = False
        lbl_OrderNo.Visible = False
        btn_OwnOrderSelection.Visible = False
        If Common_Procedures.settings.Show_Sales_OrderNumber_in_ALLEntry_Status = 1 Then
            lbl_Sales_OrderCode_forSelection_Caption.Visible = True
            cbo_Sales_OrderCode_forSelection.Visible = True

            lbl_Sales_OrderCode_forSelection_Caption.Location = lbl_place_Supply_caption.Location
            cbo_Sales_OrderCode_forSelection.Location = txt_place_Supply.Location
            cbo_Sales_OrderCode_forSelection.Size = txt_place_Supply.Size

            cbo_Sales_OrderCode_forSelection.Left = txt_place_Supply.Left + 25
            cbo_Sales_OrderCode_forSelection.Width = txt_place_Supply.Width - 25

            lbl_place_Supply_caption.Visible = False
            txt_place_Supply.Visible = False

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT TEXTILES (SOMANUR)
                cbo_Sales_OrderCode_forSelection.Enabled = False
            End If

            FnYearCode1 = ""
            FnYearCode2 = ""
                Common_Procedures.get_FnYearCode_of_Last_2_Years(FnYearCode1, FnYearCode2)

            End If

        cbo_Grid_RateFor.Visible = True
        cbo_Grid_RateFor.Items.Clear()
        cbo_Grid_RateFor.Items.Add("METER")
        cbo_Grid_RateFor.Items.Add("PAVU")
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1376" Then '  ----  JAGATHGURU TEX
        cbo_Grid_RateFor.Items.Add("KG")
        End If

        txt_Value.Visible = True
        lbl_Amount.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1242" Then '---- SOUTHERN SAREESS (ERODE)
            txt_Value.Visible = False
            lbl_Amount.Visible = True
        End If

        dgv_Selection.Columns(3).Visible = True
        dgv_Selection.Columns(4).Visible = True
        dgv_Selection.Columns(12).Visible = False
        dgv_Selection.Columns(13).Visible = False
        dgv_Selection.Columns(14).Visible = False
        dgv_Selection.Columns(16).Visible = False

        dgv_PavuDetails.Columns(13).Visible = False

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT TEXTILES (SOMANUR)

            dgv_Selection.Columns(3).Visible = False
            dgv_Selection.Columns(4).Visible = False
            dgv_Selection.Columns(12).Visible = True
            dgv_Selection.Columns(13).Visible = True
            dgv_Selection.Columns(14).Visible = True
            dgv_Selection.Columns(16).Visible = True

            dgv_Selection.Columns(1).Width = 50 ' 60
            dgv_Selection.Columns(2).Width = 45 ' 50
            dgv_Selection.Columns(5).Width = 60 ' 65
            dgv_Selection.Columns(6).Width = 90 ' 100
            dgv_Selection.Columns(12).Width = 150 ' 160



            dgv_PavuDetails.Columns(13).Visible = True
            dgv_PavuDetails.Columns(1).Width = dgv_PavuDetails.Columns(1).Width - 10
            dgv_PavuDetails.Columns(2).Width = dgv_PavuDetails.Columns(2).Width - 10
            dgv_PavuDetails.Columns(3).Width = dgv_PavuDetails.Columns(3).Width - 10
            dgv_PavuDetails.Columns(4).Width = dgv_PavuDetails.Columns(4).Width - 10
            dgv_PavuDetails.Columns(5).Width = dgv_PavuDetails.Columns(5).Width - 10
            dgv_PavuDetails.Columns(6).Width = dgv_PavuDetails.Columns(6).Width - 10

        End If

        btn_Jo_details.Visible = False
        If Common_Procedures.settings.CustomerCode = "1376" Then
            btn_Jo_details.Visible = True

        End If

        cbo_DCSufixNo.Items.Clear()
        cbo_DCSufixNo.Items.Add("")
        cbo_DCSufixNo.Items.Add("/" & Common_Procedures.FnYearCode)
        cbo_DCSufixNo.Items.Add("/" & Year(Common_Procedures.Company_FromDate) & "-" & Year(Common_Procedures.Company_ToDate))
        cbo_DCSufixNo.Items.Add("/" & Year(Common_Procedures.Company_FromDate))

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

        cbo_WidthType.Visible = False
        lbl_Widthtype.Visible = False
        If Common_Procedures.settings.AutoLoom_PavuWidthWiseConsumption_IN_Delivery = 1 Then
            cbo_WidthType.Visible = True
            lbl_Widthtype.Visible = True
        End If

        cbo_ClothName.Visible = False
        lbl_ClothName_Caption.Visible = False
        txt_CrimpPerc.Visible = False
        lbl_CrimpPerc_Caption.Visible = False
        If Common_Procedures.settings.AutoLoom_Pavu_CrimpMeters_Consumption_Stock_Posting_In_Delivery_Receipt_Entry = 1 Then
            cbo_ClothName.Visible = True
            lbl_ClothName_Caption.Visible = True
            txt_CrimpPerc.Visible = True
            lbl_CrimpPerc_Caption.Visible = True
        End If

        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        btn_SaveAll.Visible = False
        If Common_Procedures.settings.CustomerCode = "1267" Then
            btn_SaveAll.Visible = True
        End If

        dtp_Date.Text = ""
        msk_date.Text = ""

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2

        pnl_EmptyBeamDetails.Visible = False
        pnl_EmptyBeamDetails.Left = (Me.Width - pnl_EmptyBeamDetails.Width) \ 2
        pnl_EmptyBeamDetails.Top = (Me.Height - pnl_EmptyBeamDetails.Height) \ 2

        pnl_OwnOrderSelection.Visible = False
        pnl_OwnOrderSelection.Left = (Me.Width - pnl_OwnOrderSelection.Width) \ 2
        pnl_OwnOrderSelection.Top = (Me.Height - pnl_OwnOrderSelection.Height) \ 2
        pnl_OwnOrderSelection.BringToFront()

        pnl_job_order_details.Visible = False
        pnl_job_order_details.Left = (Me.Width - pnl_job_order_details.Width) \ 2
        pnl_job_order_details.Top = (Me.Height - pnl_job_order_details.Height) \ 2
        pnl_job_order_details.BringToFront()


        chk_Verified_Status.Visible = False
        If Common_Procedures.settings.Vefified_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_Verified_Status = 1 Then
                chk_Verified_Status.Visible = True
                lbl_verfied_sts.Visible = True
                cbo_Verified_Sts.Visible = True
            End If

        Else

            chk_Verified_Status.Visible = False
            '--Label9.Visible = False
            cbo_Verified_Sts.Visible = False

        End If

        If Common_Procedures.settings.CustomerCode = "1267" Then
            chk_Loaded.Visible = True
        Else
            chk_Loaded.Visible = False

        End If
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1391" Then
            Label8.Text = "Sizing"
        End If

        btn_EmptyBeamOpen.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "2000" Then
            btn_EmptyBeamOpen.Visible = True
        End If

        If Common_Procedures.settings.Show_Weaver_JobCard_Entry_STATUS = 1 Then

            lbl_weaving_job_no.Visible = True
            cbo_weaving_job_no.Visible = True


            cbo_DelvAt.Width = cbo_RecForm.Width
            cbo_weaving_job_no.Width = txt_place_Supply.Width
            cbo_weaving_job_no.BackColor = Color.White

        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1414" Then     ' SRI MAHALAKSHMI TEXTILE (PERIYANAYAKI AMMAN TEXTILES)

            dgv_PavuDetails.Columns(4).HeaderText = "MARK"

        Else

            dgv_PavuDetails.Columns(4).HeaderText = "MTR/PC"

        End If


        AddHandler txt_DCPrefixNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DCPrefixNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DCPrefixNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_DCPrefixNo.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler chk_Verified_Status.GotFocus, AddressOf ControlGotFocus
        AddHandler chk_Verified_Status.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_RateFor.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DCSufixNo.GotFocus, AddressOf ControlGotFocus
        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_DelvAt.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_RecForm.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Transport.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_VehicleNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_EndsCount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_KuraiPavuBeam.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_KuraiPavuMeter.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Value.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Ewave_Bill_No.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_WidthType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_CrimpPerc.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_TransportMode.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_DateTime_Of_Supply.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_place_Supply.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_rate.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Party_DcNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BeamNoSelection.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_SetNoSelection.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_EndsCountName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Sales_OrderCode_forSelection.GotFocus, AddressOf ControlGotFocus



        AddHandler txt_Ref_No.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Ref_date.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_JO_No.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_JO_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_weaving_job_no.GotFocus, AddressOf ControlGotFocus


        AddHandler cbo_Grid_RateFor.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DCSufixNo.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_DelvAt.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_RecForm.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Transport.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_VehicleNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_EndsCount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_KuraiPavuBeam.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_KuraiPavuMeter.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_WidthType.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_CrimpPerc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Party_DcNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BeamNoSelection.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_SetNoSelection.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Ewave_Bill_No.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Value.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_rate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_TransportMode.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_DateTime_Of_Supply.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_place_Supply.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_EndsCountName.LostFocus, AddressOf ControlLostFocus


        AddHandler txt_Ref_No.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Ref_date.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_JO_No.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_JO_Date.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_weaving_job_no.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Sales_OrderCode_forSelection.LostFocus, AddressOf ControlLostFocus



        AddHandler txt_Ref_No.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_JO_Date.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_JO_No.KeyDown, AddressOf TextBoxControlKeyDown

        'AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_KuraiPavuBeam.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_KuraiPavuMeter.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler txt_DateTime_Of_Supply.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_place_Supply.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Party_DcNo.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_CrimpPerc.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler txt_Ref_No.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_JO_Date.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_JO_No.KeyPress, AddressOf TextBoxControlKeyPress


        'AddHandler txt_Ewave_Bill_No.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_KuraiPavuBeam.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_KuraiPavuMeter.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Party_DcNo.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_DateTime_Of_Supply.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_place_Supply.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_CrimpPerc.KeyPress, AddressOf TextBoxControlKeyPress


        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Weaver_Pavu_Delivery_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
                    Exit Sub

                ElseIf pnl_EmptyBeamDetails.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
                    Exit Sub
                ElseIf pnl_job_order_details.Visible = True Then
                    btn_JO_details_close_Click(sender, e)
                    Exit Sub
                ElseIf pnl_OwnOrderSelection.Visible = True Then
                    btn_Close_OwnOrderSelection_Click(sender, e)
                    Exit Sub
                Else
                    Close_Form()

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
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim LockSTS As Boolean = False
        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        'Try

        da1 = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name as DelvName, c.Ledger_Name as TransportName, d.EndsCount_Name, e.Ledger_Name as RecFromName, f.cloth_name from Weaver_Pavu_Delivery_Head a INNER JOIN Ledger_Head b ON a.DeliveryTo_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Transport_IdNo = c.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo LEFT OUTER JOIN Ledger_Head e ON a.ReceivedFrom_IdNo = e.Ledger_IdNO LEFT OUTER JOIN Cloth_Head f ON a.cloth_IdNo = f.cloth_IdNo Where a.Weaver_Pavu_Delivery_Code = '" & Trim(NewCode) & "'", con)
        dt1 = New DataTable
        da1.Fill(dt1)

        If dt1.Rows.Count > 0 Then
            txt_DCPrefixNo.Text = dt1.Rows(0).Item("Weaver_Pavu_Delivery_PrefixNo").ToString
            lbl_DcNo.Text = dt1.Rows(0).Item("Weaver_Pavu_Delivery_RefNo").ToString
            cbo_DCSufixNo.Text = dt1.Rows(0).Item("Weaver_Pavu_Delivery_SuffixNo").ToString
            dtp_Date.Text = dt1.Rows(0).Item("Weaver_Pavu_Delivery_Date")
            msk_date.Text = dtp_Date.Text

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

            cbo_DelvAt.Text = dt1.Rows(0).Item("DelvName").ToString
            txt_KuraiPavuBeam.Text = Val(dt1.Rows(0).Item("Empty_Beam").ToString)
            txt_KuraiPavuMeter.Text = Val(dt1.Rows(0).Item("Pavu_Meters").ToString)
            cbo_EndsCount.Text = dt1.Rows(0).Item("EndsCount_Name").ToString
            cbo_VehicleNo.Text = dt1.Rows(0).Item("Vehicle_No").ToString
            cbo_Transport.Text = dt1.Rows(0).Item("TransportName").ToString
            cbo_RecForm.Text = dt1.Rows(0).Item("RecFromName").ToString
            txt_Freight.Text = Val(dt1.Rows(0).Item("Freight").ToString)
            cbo_WidthType.Text = dt1.Rows(0).Item("Width_Type").ToString
            cbo_ClothName.Text = dt1.Rows(0).Item("Cloth_Name").ToString
            txt_CrimpPerc.Text = dt1.Rows(0).Item("Crimp_Percentage").ToString
            txt_Party_DcNo.Text = dt1.Rows(0).Item("Party_DcNo").ToString
            txt_Ewave_Bill_No.Text = dt1.Rows(0).Item("EWave_Bill_No").ToString

            lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))
            cbo_TransportMode.Text = dt1.Rows(0).Item("Transportation_Mode").ToString
            txt_DateTime_Of_Supply.Text = dt1.Rows(0).Item("Date_Time_Of_Supply").ToString
            txt_place_Supply.Text = dt1.Rows(0).Item("Place_Of_Supply").ToString
            lbl_OrderNo.Text = dt1.Rows(0).Item("Our_Order_No").ToString
            lbl_OrderCode.Text = dt1.Rows(0).Item("Own_Order_Code").ToString
            lbl_pavu_weight.Text = dt1.Rows(0).Item("Total_pavu_Weight").ToString
            cbo_Grid_RateFor.Text = dt1.Rows(0).Item("Rate_for").ToString
            If Val(dt1.Rows(0).Item("Verified_Status").ToString) = 1 Then chk_Verified_Status.Checked = True

            If Val(dt1.Rows(0).Item("Loaded_By_Our_Employee").ToString) = 1 Then chk_Loaded.Checked = True

            txt_JO_No.Text = Trim(dt1.Rows(0).Item("Job_order_No").ToString)

            txt_JO_Date.Text = Trim(dt1.Rows(0).Item("Job_order_Date").ToString)

            txt_Ref_No.Text = Trim(dt1.Rows(0).Item("Party_Ref_No").ToString)

            txt_Ref_date.Text = Trim(dt1.Rows(0).Item("Party_Ref_Date").ToString)

            cbo_weaving_job_no.Text = dt1.Rows(0).Item("Weaving_JobCode_forSelection").ToString
            cbo_Sales_OrderCode_forSelection.Text = dt1.Rows(0).Item("ClothSales_OrderCode_forSelection").ToString

            da2 = New SqlClient.SqlDataAdapter("Select a.*, b.reference_code as set_reference_code, b.Pavu_Delivery_Increment, c.EndsCount_Name, d.Beam_Width_Name from Weaver_Pavu_Delivery_Details a INNER JOIN Stock_SizedPavu_Processing_Details b ON a.Set_Code = b.Set_Code and a.Beam_No = b.Beam_No INNER JOIN EndsCount_Head c ON a.EndsCount_IdNo = c.EndsCount_IdNo LEFT OUTER JOIN Beam_Width_Head d ON a.Beam_Width_Idno = d.Beam_Width_Idno where a.Weaver_Pavu_Delivery_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
            dt2 = New DataTable
            da2.Fill(dt2)

            dgv_PavuDetails.Rows.Clear()
            SNo = 0

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_PavuDetails.Rows.Add()

                    SNo = SNo + 1
                    dgv_PavuDetails.Rows(n).Cells(0).Value = Val(SNo)
                    dgv_PavuDetails.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Set_No").ToString
                    dgv_PavuDetails.Rows(n).Cells(2).Value = dt2.Rows(i).Item("Beam_No").ToString
                    dgv_PavuDetails.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Pcs").ToString
                    If Val(dt2.Rows(i).Item("Meters_Pc").ToString) <> 0 Then
                        dgv_PavuDetails.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Meters_Pc").ToString)
                    End If
                    dgv_PavuDetails.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                    dgv_PavuDetails.Rows(n).Cells(6).Value = dt2.Rows(i).Item("EndsCount_Name").ToString
                    dgv_PavuDetails.Rows(n).Cells(7).Value = dt2.Rows(i).Item("Beam_Width_Name").ToString

                    dgv_PavuDetails.Rows(n).Cells(8).Value = ""
                    dgv_PavuDetails.Rows(n).Cells(9).Value = dt2.Rows(i).Item("Noof_Used").ToString
                    dgv_PavuDetails.Rows(n).Cells(10).Value = dt2.Rows(i).Item("set_code").ToString
                    dgv_PavuDetails.Rows(n).Cells(11).Value = dt2.Rows(i).Item("Pavu_Delivery_Increment").ToString
                    dgv_PavuDetails.Rows(n).Cells(13).Value = dt2.Rows(i).Item("Weaver_LoomNo").ToString

                    If Val(dgv_PavuDetails.Rows(n).Cells(9).Value) > 0 And Val(dgv_PavuDetails.Rows(n).Cells(9).Value) <> Val(dgv_PavuDetails.Rows(n).Cells(11).Value) Then
                        dgv_PavuDetails.Rows(n).Cells(8).Value = "1"
                    End If

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT TEXTILES (SOMANUR)
                        If Trim(dt2.Rows(i).Item("set_reference_code").ToString) <> "" Then
                            If InStr(1, Trim(dt2.Rows(i).Item("set_reference_code").ToString), "SSPDC-") > 0 Then
                                cbo_DelvAt.Enabled = False
                                msk_date.Enabled = False
                            End If
                        End If
                    End If


                    da = New SqlClient.SqlDataAdapter("Select a.Total_meters ,a.Total_beams  from Pavu_Delivery_Selections_Processing_Details a where  a.Reference_Code<>'" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Delivery_Code='" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Total_meters < 0", con)
                    dt = New DataTable
                    da.Fill(dt)
                    If dt.Rows.Count > 0 Then
                        For j = 0 To dgv_PavuDetails.ColumnCount - 1
                            dgv_PavuDetails.Rows(n).Cells(j).Style.BackColor = Color.LightGray
                        Next j
                        LockSTS = True
                    End If
                    dt.Clear()

                Next i


            End If

            With dgv_PavuDetails_Total
                If .RowCount = 0 Then .Rows.Add()
                .Rows(0).Cells(2).Value = Val(dt1.Rows(0).Item("Total_Beam").ToString)
                .Rows(0).Cells(3).Value = Val(dt1.Rows(0).Item("Total_Pcs").ToString)
                .Rows(0).Cells(5).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
            End With

            dt2.Clear()

            txt_rate.Text = Format(Val(dt1.Rows(0).Item("Rate").ToString), "##########0.00")
            txt_Value.Text = Format(Val(dt1.Rows(0).Item("Value").ToString), "##########0.00")
            lbl_Amount.Text = Format(Val(dt1.Rows(0).Item("Amount").ToString), "##########0.00")
            lbl_Freight_Pavu.Text = Format(Val(dt1.Rows(0).Item("Freight_Pavu").ToString), "##########0.00")


        Else

            new_record()

        End If


        dt1.Clear()
        dt1.Dispose()
        da1.Dispose()
        dgv_ActCtrlName = ""


        If LockSTS = True Then



            dtp_Date.Enabled = False
            dtp_Date.BackColor = Color.Gray

            cbo_DelvAt.Enabled = False
            cbo_DelvAt.BackColor = Color.Gray

            cbo_RecForm.Enabled = False
            cbo_RecForm.BackColor = Color.Gray

            cbo_Transport.Enabled = False
            cbo_Transport.BackColor = Color.Gray

            txt_Party_DcNo.Enabled = False
            txt_Party_DcNo.BackColor = Color.Gray

            txt_KuraiPavuBeam.Enabled = False
            txt_KuraiPavuBeam.BackColor = Color.Gray

            txt_KuraiPavuMeter.Enabled = False
            txt_KuraiPavuMeter.BackColor = Color.Gray


            msk_date.Enabled = False
            msk_date.BackColor = Color.Gray

        End If

        Grid_Cell_DeSelect()

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try

        If msk_date.Visible And msk_date.Enabled Then msk_date.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable

        Dim Da2 As New SqlClient.SqlDataAdapter
        Dim Dt2 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Nr As Long = 0
        Dim vOrdByNo As String = ""

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text)

        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Weaver_Pavu_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Weaver_Pavu_Delivery_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Weaver_Pavu_Delivery_Entry, New_Entry, Me, con, " Weaver_Pavu_Delivery_Head", " Weaver_Pavu_Delivery_Code", NewCode, " Weaver_Pavu_Delivery_Date", "( Weaver_Pavu_Delivery_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub

        If Common_Procedures.settings.Vefified_Status = 1 Then
            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            If Val(Common_Procedures.get_FieldValue(con, "Weaver_Pavu_Delivery_Head", "Verified_Status", "(Weaver_Pavu_Delivery_Code = '" & Trim(NewCode) & "')")) = 1 Then
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



        Da2 = New SqlClient.SqlDataAdapter("Select a.Total_meters ,a.Total_beams  from Pavu_Delivery_Selections_Processing_Details a where  a.Reference_Code<>'" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Delivery_Code='" & Trim(Pk_Condition) & Trim(NewCode) & "' and a.Total_meters < 0", con)
        Dt2 = New DataTable
        Da2.Fill(Dt2)
        If Dt2.Rows.Count > 0 Then
            MessageBox.Show("This Dc Already in Receipt ", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub

        End If
        Dt2.Clear()



        If New_Entry = True Then
            MessageBox.Show("This is New Entry", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        trans = con.BeginTransaction

        Try

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con

            cmd.Transaction = trans

            cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
            cmd.ExecuteNonQuery()
            If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Yarn_Stock) = 1 Then
                cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno    , EndsCount_IdNo ) " &
                                      " Select                               'PAVU'    , Reference_Code, Reference_Date, Company_IdNo, DeliveryTo_Idno, EndsCount_IdNo from Stock_Pavu_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()
            End If

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Weaver_Pavu_Delivery_head", "Weaver_Pavu_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Weaver_Pavu_Delivery_Code, Company_IdNo, for_OrderBy", trans)
            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Weaver_Pavu_Delivery_Details", "Weaver_Pavu_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, " Set_No,Beam_No,Pcs ,Meters_Pc,Meters ,EndsCount_IdNo,Beam_Width_IdNo,Noof_Used,Set_Code", "Sl_No", "Weaver_Pavu_Delivery_Code, For_OrderBy, Company_IdNo, Weaver_Pavu_Delivery_No, Weaver_Pavu_Delivery_Date, Ledger_Idno", trans)

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)

            Da = New SqlClient.SqlDataAdapter("Select * from Weaver_Pavu_Delivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Pavu_Delivery_Code = '" & Trim(NewCode) & "'", con)
            Da.SelectCommand.Transaction = trans
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then
                For i = 0 To Dt1.Rows.Count - 1

                    Nr = 0
                    cmd.CommandText = "update Stock_SizedPavu_Processing_Details set " _
                              & " StockAt_IdNo = " & Str(Val(Dt1.Rows(i).Item("ReceivedFrom_IdNo").ToString)) & ", " _
                              & " Pavu_Delivery_Increment = Pavu_Delivery_Increment - 1 " _
                              & " Where " _
                              & " StockAt_IdNo = " & Str(Val(Dt1.Rows(i).Item("DeliveryTo_IdNo").ToString)) & " and " _
                              & " Set_Code = '" & Trim(Dt1.Rows(i).Item("Set_Code").ToString) & "' and " _
                              & " beam_no = '" & Trim(Dt1.Rows(i).Item("Beam_No").ToString) & "' and " _
                              & " Pavu_Delivery_Increment = " & Str(Val(Dt1.Rows(i).Item("Noof_Used").ToString))

                    Nr = cmd.ExecuteNonQuery

                    If Nr = 0 Then
                        Throw New ApplicationException("Some Beams Delivered to Others - Beam No : " & Trim(Dt1.Rows(i).Item("Beam_No").ToString))
                        Exit Sub
                    End If

                Next
            End If
            Dt1.Clear()

            cmd.CommandText = "Update Weaver_PavuBobin_Requirement_Details set Sized_Beam_Return = a.Sized_Beam_Return - (b.Sized_Beam)  from Weaver_PavuBobin_Requirement_Details a, Weaver_Pavu_Delivery_Requirement_Details b Where b.Weaver_Pavu_Delivery_Requirement_Code = '" & Trim(NewCode) & "' and a.Weaver_PavuBobin_Requirement_Code = b.Weaver_PavuBobin_Requirement_Code and a.Weaver_PavuBobin_Requirement_SlNo = b.Weaver_PavuBobin_Requirement_SlNo"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Weaver_Pavu_Delivery_Requirement_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Pavu_Delivery_Requirement_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Weaver_Pavu_Delivery_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Pavu_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Weaver_Pavu_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Pavu_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            cmd.CommandText = "Delete from Pavu_Delivery_Selections_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Pavu_Stock) = 1 Then
                If Common_Procedures.Check_is_Negative_Stock_Status(con, trans) = True Then Exit Sub
            End If

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

        If Filter_Status = False Then

            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where ( Ledger_IdNo = 0 or Ledger_Type = 'WEAVER' ) order by Ledger_DisplayName", con)
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"


            da = New SqlClient.SqlDataAdapter("select distinct(EndsCount_Name) from EndsCount_Head order by EndsCount_Name", con)
            da.Fill(dt3)
            cbo_Filter_EndsCountName.DataSource = dt3
            cbo_Filter_EndsCountName.DisplayMember = "EndsCount_Name"

            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""

            cbo_Filter_EndsCountName.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1

            cbo_Filter_EndsCountName.SelectedIndex = -1

            dgv_Filter_Details.Rows.Clear()

        End If

        pnl_Filter.Visible = True
        pnl_Filter.Enabled = True
        pnl_Filter.BringToFront()
        pnl_Back.Enabled = False
        If dtp_Filter_Fromdate.Enabled And dtp_Filter_Fromdate.Visible Then dtp_Filter_Fromdate.Focus()

    End Sub

    Public Sub movefirst_record() Implements Interface_MDIActions.movefirst_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Pavu_Delivery_refNo from Weaver_Pavu_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Pavu_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Weaver_Pavu_Delivery_No", con)
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

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_DcNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Pavu_Delivery_refNo from Weaver_Pavu_Delivery_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Pavu_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Weaver_Pavu_Delivery_No", con)
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

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_DcNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Pavu_Delivery_refNo from Weaver_Pavu_Delivery_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Pavu_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Weaver_Pavu_Delivery_No desc", con)
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

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        'Try

        da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Pavu_Delivery_refNo from Weaver_Pavu_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Pavu_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Weaver_Pavu_Delivery_No desc", con)
        da.Fill(dt)

        movno = ""
        If dt.Rows.Count > 0 Then
            If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                movno = dt.Rows(0)(0).ToString
            End If
        End If

        If Val(movno) <> 0 Then move_record(movno)

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        'End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True
            cbo_DelvAt.Enabled = True

            cbo_RecForm.Enabled = True
            txt_Party_DcNo.Enabled = True
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT TEXTILES (SOMANUR)
                txt_Party_DcNo.Enabled = False
            End If


            lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Pavu_Delivery_Head", "Weaver_Pavu_Delivery_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_DcNo.ForeColor = Color.Red

            msk_date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Weaver_Pavu_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Pavu_Delivery_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Weaver_Pavu_Delivery_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Weaver_Pavu_Delivery_Date").ToString <> "" Then msk_date.Text = dt1.Rows(0).Item("Weaver_Pavu_Delivery_Date").ToString
                End If
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT TEXTILES (SOMANUR)
                    cbo_RecForm.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("ReceivedFrom_IdNo").ToString))
                End If
                If IsDBNull(dt1.Rows(0).Item("Weaver_Pavu_Delivery_PrefixNo").ToString) = False Then
                    If dt1.Rows(0).Item("Weaver_Pavu_Delivery_PrefixNo").ToString <> "" Then txt_DCPrefixNo.Text = dt1.Rows(0).Item("Weaver_Pavu_Delivery_PrefixNo").ToString
                End If
                If IsDBNull(dt1.Rows(0).Item("Weaver_Pavu_Delivery_SuffixNo").ToString) = False Then
                    If dt1.Rows(0).Item("Weaver_Pavu_Delivery_SuffixNo").ToString <> "" Then cbo_DCSufixNo.Text = dt1.Rows(0).Item("Weaver_Pavu_Delivery_SuffixNo").ToString
                End If
                If IsDBNull(dt1.Rows(0).Item("Rate_for").ToString) = False Then
                    If dt1.Rows(0).Item("Rate_for").ToString <> "" Then cbo_Grid_RateFor.Text = dt1.Rows(0).Item("Rate_for").ToString
                End If

            End If
            dt1.Clear()

            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

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

            inpno = InputBox("Enter Dc.No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Weaver_Pavu_Delivery_RefNo from Weaver_Pavu_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Pavu_Delivery_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Dc No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Weaver_Pavu_Delivery_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Weaver_Pavu_Delivery_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Weaver_Pavu_Delivery_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Dc No.", "FOR NEW RECEIPT INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Weaver_Pavu_Delivery_No from Weaver_Pavu_Delivery_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Pavu_Delivery_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Dc No", "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_DcNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW RECEIPT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim Delv_ID As Integer = 0
        Dim Clo_ID As Integer = 0
        Dim Led_ID As Integer = 0
        Dim Trans_ID As Integer = 0
        Dim KuPvu_EdsCnt_ID As Integer = 0
        Dim SzPvu_EdsCnt_ID As Integer = 0
        Dim vEdsCnt_ID As Integer = 0
        Dim ReqEdsCnt_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim Nr As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim vTotPvuBms As Single, vTotPvuMtrs As Single, vTotPvuPcs As Single
        Dim YCnt_ID As Integer = 0
        Dim Rec_ID As Integer = 0
        Dim YMil_ID As Integer = 0
        Dim EntID As String = ""
        Dim Bw_IdNo As Integer = 0
        Dim Ends_IdNo As Integer = 0
        Dim Pavu_DelvInc As Integer = 0
        Dim Ent_NoofUsed As Integer = 0
        Dim Stock_In As String
        Dim mtrspcs As Integer
        Dim dt2 As New DataTable
        Dim vTotPvuStk As Single = 0, vTotPvuStkAlLoomMtr As Single = 0
        Dim vWdTyp As Single = 0
        Dim Delv_Ledtype As String = ""
        Dim Rec_Ledtype As String = ""
        Dim Stk_DelvMtr As Single, Stk_RecMtr As Single
        Dim OurOrd_No As String = ""
        Dim vMax_PavuStk_Lvl As String = ""
        Dim vCurr_PavuStk As String = ""
        Dim vNoofBeams As Integer = 0
        Dim Verified_STS As String = ""
        Dim vOrdByNo As String = ""
        Dim vCrmp_Mtrs As String = 0
        Dim Load_STS As Single = 0
        Dim vWEAPAVUDCNO As String = ""
        Dim vCOMP_LEDIDNO As Integer = 0
        Dim vDELVLED_COMPIDNO As Integer = 0
        Dim vPVUSTK_ENDSID As Integer = 0
        Dim vWIDTHTYPE As String = ""
        Dim vFIRST_EdsCnt_ID As Integer = 0
        Dim vENTDB_DelvToIDno As String = 0
        Dim vCLO_IDNO As Integer = 0
        Dim vTEX_WEAV_ALL_LOOMSNOS As String = ""
        Dim weaver_job_code As String = ""
        Dim vPAVU_WIDTHMULTIPLIED_MTR As String = 0
        Dim vNOOFENTRY As String = 0
        Dim vCREATED_DTTM_TXT As String = ""
        Dim vMODIFIED_DTTM_TXT As String = ""


        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text)


        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If
        If Val(Common_Procedures.User.IdNo) = 0 Then
            MessageBox.Show("Invalid User Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Weaver_Pavu_Delivery_Entry, New_Entry) = False Then Exit Sub
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        ' If Common_Procedures.UserRight_Check(Common_Procedures.UR.Weaver_Yarn_Delivery_Entry, New_Entry) = False Then Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Weaver_Pavu_Delivery_Entry, New_Entry, Me, con, " Weaver_Pavu_Delivery_Head", " Weaver_Pavu_Delivery_Code", NewCode, " Weaver_Pavu_Delivery_Date", "( Weaver_Pavu_Delivery_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and  Weaver_Pavu_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc,  Weaver_Pavu_Delivery_No desc", dtp_Date.Value.Date) = False Then Exit Sub

        If Common_Procedures.settings.Vefified_Status = 1 Then
            If Not (Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_Verified_Status = 1) Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

                If Val(Common_Procedures.get_FieldValue(con, "Weaver_Pavu_Delivery_Head", "Verified_Status", "(Weaver_Pavu_Delivery_Code = '" & Trim(NewCode) & "')")) = 1 Then
                    MessageBox.Show("Entry Already Verified", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub
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

        If Not (Convert.ToDateTime(msk_date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()
            Exit Sub
        End If

        Delv_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DelvAt.Text)
        If Delv_ID = 0 Then
            MessageBox.Show("Invalid Delivery Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_DelvAt.Enabled And cbo_DelvAt.Visible Then cbo_DelvAt.Focus()
            Exit Sub
        End If


        Rec_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_RecForm.Text)
        If Rec_ID = 0 Then Rec_ID = 4

        If Delv_ID = Rec_ID Then
            MessageBox.Show("Invalid Party Name" & Chr(13) & "Does not accept same party", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_DelvAt.Enabled And cbo_DelvAt.Visible Then cbo_DelvAt.Focus()
            Exit Sub
        End If

        KuPvu_EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, cbo_EndsCount.Text)
        If Val(txt_KuraiPavuMeter.Text) <> 0 Then
            If KuPvu_EdsCnt_ID = 0 Then
                MessageBox.Show("Invalid EndsCount Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_EndsCount.Enabled And cbo_EndsCount.Visible Then cbo_EndsCount.Focus()
                Exit Sub
            End If
        End If
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then
            If KuPvu_EdsCnt_ID = 0 Then
                MessageBox.Show("Invalid EndsCount Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_EndsCount.Enabled And cbo_EndsCount.Visible Then cbo_EndsCount.Focus()
                Exit Sub
            End If
        End If


        'If Trim(lbl_OrderCode.Text) <> "" Then
        '    If Delv_ID <> 0 Then
        '        Da = New SqlClient.SqlDataAdapter("select a.*,b.* from Own_Order_Head a INNER JOIN Own_order_Weaving_Details b ON a.Own_Order_Code =b.Own_Order_Code where a.Own_Order_Code = '" & Trim(lbl_OrderCode.Text) & "' and  b.Ledger_idno = " & Str(Val(Delv_ID)), con)
        '        Dt1 = New DataTable
        '        Da.Fill(Dt1)
        '        If Dt1.Rows.Count > 0 Then
        '            OurOrd_No = Dt1.Rows(0).Item("Order_No").ToString
        '        End If
        '    End If
        '    If Trim(OurOrd_No) <> Trim(lbl_OrderNo.Text) Then
        '        MessageBox.Show("Invalid Mismatch Of Order No for this Party Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        '        If cbo_DelvAt.Enabled And cbo_DelvAt.Visible Then cbo_DelvAt.Focus()
        '        Exit Sub
        '    End If
        'End If

        Trans_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Transport.Text)

        vCLO_IDNO = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text)

        If cbo_ClothName.Visible And vCLO_IDNO = 0 Then
            MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_ClothName.Enabled And cbo_ClothName.Visible Then
                cbo_ClothName.Focus()
            Else
                If cbo_RecForm.Enabled And cbo_RecForm.Visible Then cbo_RecForm.Focus() Else msk_date.Focus()
            End If
            Exit Sub
        End If

        If cbo_WidthType.Visible And cbo_WidthType.Text = "" Then
            MessageBox.Show("Invalid Width Type", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_WidthType.Enabled And cbo_WidthType.Visible Then
                cbo_WidthType.Focus()
            Else
                If cbo_RecForm.Enabled And cbo_RecForm.Visible Then cbo_RecForm.Focus() Else msk_date.Focus()
            End If
            Exit Sub
        End If


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT TEXTILES (SOMANUR)

            If SaveAll_STS = False Then

                If Val(txt_KuraiPavuMeter.Text) <> 0 And KuPvu_EdsCnt_ID <> 0 Then

                    vWIDTHTYPE = Common_Procedures.get_FieldValue(con, "EndsCount_Head", "WidthType_Single_Double_Triple", "(EndsCount_IdNo = " & Str(Val(KuPvu_EdsCnt_ID)) & ")")

                    If Trim(UCase(vWIDTHTYPE)) <> Trim(UCase(cbo_WidthType.Text)) Then

                        MessageBox.Show("The Width Type of Endscount(" & Trim(cbo_EndsCount.Text) & ") is mismatched  -  " & Trim(UCase(vWIDTHTYPE)), "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If cbo_EndsCount.Enabled And cbo_EndsCount.Visible Then
                            cbo_EndsCount.Focus()
                        Else
                            If cbo_RecForm.Enabled And cbo_RecForm.Visible Then cbo_RecForm.Focus() Else msk_date.Focus()
                        End If
                        Exit Sub

                    End If

                End If

            End If

        End If



        vFIRST_EdsCnt_ID = 0
        With dgv_PavuDetails

            For i = 0 To .RowCount - 1

                If Val(dgv_PavuDetails.Rows(i).Cells(5).Value) <> 0 Then

                    If Trim(dgv_PavuDetails.Rows(i).Cells(1).Value) = "" Then
                        MessageBox.Show("Invalid Set No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then
                            dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(i).Cells(1)
                            dgv_PavuDetails.Focus()
                        End If
                        Exit Sub
                    End If

                    If Trim(dgv_PavuDetails.Rows(i).Cells(2).Value) = "" Then
                        MessageBox.Show("Invalid Beam No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then
                            dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(i).Cells(2)
                            dgv_PavuDetails.Focus()
                        End If
                        Exit Sub
                    End If

                    If Trim(dgv_PavuDetails.Rows(i).Cells(10).Value) = "" Then
                        MessageBox.Show("Invalid Set Code", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then
                            dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(i).Cells(1)
                            dgv_PavuDetails.Focus()
                        End If
                        Exit Sub
                    End If

                    vEdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, .Rows(i).Cells(6).Value)
                    If Val(vEdsCnt_ID) = 0 Then
                        MessageBox.Show("Invalid Ends Count", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then
                            dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(i).Cells(6)
                            dgv_PavuDetails.Focus()
                        End If
                        Exit Sub
                    End If

                    If vFIRST_EdsCnt_ID = 0 Then
                        vFIRST_EdsCnt_ID = vEdsCnt_ID
                    End If

                    If SaveAll_STS = False Then
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT TEXTILES (SOMANUR)

                        vWIDTHTYPE = Common_Procedures.get_FieldValue(con, "EndsCount_Head", "WidthType_Single_Double_Triple", "(EndsCount_IdNo = " & Str(Val(vEdsCnt_ID)) & ")")

                        If Trim(UCase(vWIDTHTYPE)) <> Trim(UCase(cbo_WidthType.Text)) Then

                            MessageBox.Show("The Width Type of Endscount(" & Trim(.Rows(i).Cells(6).Value) & ") is mismatched - " & Trim(UCase(vWIDTHTYPE)), "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If cbo_WidthType.Enabled And cbo_WidthType.Visible Then
                                cbo_WidthType.Focus()
                            Else
                                If cbo_RecForm.Enabled And cbo_RecForm.Visible Then cbo_RecForm.Focus() Else msk_date.Focus()
                            End If
                            Exit Sub

                        End If

                    End If

                    If cbo_ClothName.Visible = True Then

                        vNOOFENTRY = Common_Procedures.get_FieldValue(con, "Cloth_EndsCount_Details", "count(*)", "(Cloth_IdNo = " & Str(Val(vCLO_IDNO)) & " and EndsCount_IdNo = " & Str(Val(vEdsCnt_ID)) & ")")

                        If Val(vNOOFENTRY) = 0 Then

                            MessageBox.Show("The endcount (" & Trim(.Rows(i).Cells(6).Value) & ") is wrong For the quality (" & Trim(UCase(cbo_ClothName.Text)) & ")", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then
                                dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(i).Cells(6)
                                dgv_PavuDetails.Focus()
                            End If
                            Exit Sub

                        End If

                    End If

                End If

                End If
            Next
        End With

        If SaveAll_STS = False Then

            If cbo_Sales_OrderCode_forSelection.Visible = True And cbo_ClothName.Visible = True Then

            If cbo_Sales_OrderCode_forSelection.Visible = True Then

                If Trim(cbo_Sales_OrderCode_forSelection.Text) = "" Then
                    MessageBox.Show("Invalid " & lbl_Sales_OrderCode_forSelection_Caption.Text & Chr(13) & "Select {" & lbl_Sales_OrderCode_forSelection_Caption.Text & "} in this Pavu Delivery", "DOES Not SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_Sales_OrderCode_forSelection.Enabled And cbo_Sales_OrderCode_forSelection.Visible Then
                        cbo_Sales_OrderCode_forSelection.Focus()
                    Else
                        If cbo_RecForm.Enabled And cbo_RecForm.Visible Then cbo_RecForm.Focus() Else msk_date.Focus()
                    End If
                    Exit Sub
                End If

                Da = New SqlClient.SqlDataAdapter("Select a.ClothSales_Order_Code from ClothSales_Order_Details a, ClothSales_Order_Head b Where a.Cloth_Idno = " & Str(Val(vCLO_IDNO)) & " And b.ClothSales_OrderCode_forSelection = '" & Trim(cbo_Sales_OrderCode_forSelection.Text) & "' and a.ClothSales_Order_Code = b.ClothSales_Order_Code", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count <= 0 Then
                    MessageBox.Show("Invalid Cloth Name  {" & Trim(cbo_ClothName.Text) & "} " & Chr(13) & "This {Cloth Name} does not belong to this Sales Order Indent No.", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_ClothName.Enabled And cbo_ClothName.Visible Then
                        cbo_ClothName.Focus()
                    ElseIf cbo_Sales_OrderCode_forSelection.Enabled And cbo_Sales_OrderCode_forSelection.Visible Then
                        cbo_Sales_OrderCode_forSelection.Focus()
                    Else
                        If cbo_RecForm.Enabled And cbo_RecForm.Visible Then cbo_RecForm.Focus() Else msk_date.Focus()
                    End If
                    Exit Sub
                End If
                Dt1.Clear()

            End If

            vTEX_WEAV_ALL_LOOMSNOS = ""
            Da = New SqlClient.SqlDataAdapter("select Loom_No from Weaver_Loom_Details Where ledger_idno = " & Str(Val(Delv_ID)) & " and Cloth_Idno = " & Str(Val(vCLO_IDNO)), con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then

                For I = 0 To Dt1.Rows.Count - 1

                    If IsDBNull(Dt1.Rows(I).Item("Loom_No").ToString) = False Then
                        If Trim(Dt1.Rows(I).Item("Loom_No").ToString) <> "" Then
                            vTEX_WEAV_ALL_LOOMSNOS = Trim(vTEX_WEAV_ALL_LOOMSNOS) & "~" & Trim(Dt1.Rows(I).Item("Loom_No").ToString) & "~"
                        End If
                    End If

                Next I

            Else

                MessageBox.Show("Invalid Cloth Name  {" & Trim(cbo_ClothName.Text) & "} " & Chr(13) & "This Weaver does not run this Quality", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_ClothName.Enabled And cbo_ClothName.Visible Then
                    cbo_ClothName.Focus()
                ElseIf cbo_Sales_OrderCode_forSelection.Enabled And cbo_Sales_OrderCode_forSelection.Visible Then
                    cbo_Sales_OrderCode_forSelection.Focus()
                Else
                    If cbo_RecForm.Enabled And cbo_RecForm.Visible Then cbo_RecForm.Focus() Else msk_date.Focus()
                End If
                Exit Sub

            End If
            Dt1.Clear()


            If dgv_PavuDetails.Columns(13).Visible = True Then

                cmd.Connection = con

                cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempSimpleTable)
                cmd.ExecuteNonQuery()

                For i = 0 To dgv_PavuDetails.RowCount - 1

                    If Val(dgv_PavuDetails.Rows(i).Cells(5).Value) <> 0 Then

                        If Trim(dgv_PavuDetails.Rows(i).Cells(13).Value) = "" Then
                            MessageBox.Show("Invalid {Loom No} For Beam No : " & Trim(dgv_PavuDetails.Rows(i).Cells(2).Value), "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then
                                dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(i).Cells(13)
                                dgv_PavuDetails.Focus()
                            End If
                            Exit Sub

                        End If

                        If InStr(1, Trim(UCase(vTEX_WEAV_ALL_LOOMSNOS)), "~" & Trim(dgv_PavuDetails.Rows(i).Cells(13).Value) & "~") <= 0 Then
                            MessageBox.Show("Invalid Loom No {" & Trim(dgv_PavuDetails.Rows(i).Cells(13).Value) & "} in Beam No : " & Trim(dgv_PavuDetails.Rows(i).Cells(2).Value) & Chr(13) & "There are no looms at this weaver for this ClothName.", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                            If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then
                                dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(i).Cells(13)
                                dgv_PavuDetails.Focus()
                            End If
                            Exit Sub
                        End If

                        cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSimpleTable) & " (name1, int1) values ('" & Trim(dgv_PavuDetails.Rows(i).Cells(13).Value) & "', 1)"
                        cmd.ExecuteNonQuery()

                    End If

                Next

                Dim vLOM_NoofBeams As Integer = 0

                vLOM_NoofBeams = 0
                If Trim(cbo_WidthType.Text) <> "" Then
                    If InStr(1, Trim(UCase(cbo_WidthType.Text)), "1 BEAM") > 0 Then
                        vLOM_NoofBeams = 1
                    ElseIf InStr(1, Trim(UCase(cbo_WidthType.Text)), "2 BEAM") > 0 Then
                        vLOM_NoofBeams = 2
                    End If
                End If

                If vLOM_NoofBeams > 1 Then
                    Da = New SqlClient.SqlDataAdapter("select name1 as LoomNo, sum(int1) as noofbeams from " & Trim(Common_Procedures.EntryTempSimpleTable) & " group by name1 Having sum(int1) <> 0", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    If Dt1.Rows.Count > 0 Then

                        For I = 0 To Dt1.Rows.Count - 1

                            If IsDBNull(Dt1.Rows(I).Item("LoomNo").ToString) = False Then
                                If Trim(Dt1.Rows(I).Item("LoomNo").ToString) <> "" Then

                                    If Val(Dt1.Rows(I).Item("noofbeams").ToString) Mod vLOM_NoofBeams <> 0 Then

                                        MessageBox.Show("Invalid Loom No  {" & Trim(Dt1.Rows(I).Item("LoomNo").ToString) & "} " & Chr(13) & "The loom requires " & vLOM_NoofBeams & " beams.", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                                        If dgv_PavuDetails.Enabled And dgv_PavuDetails.Visible Then
                                            dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(I).Cells(13)
                                            dgv_PavuDetails.Focus()
                                        End If
                                        Exit Sub

                                    End If

                                End If
                            End If

                        Next I

                    End If
                    Dt1.Clear()
                End If

            End If

        End If


        If cbo_ClothName.Visible = True Then
            If Val(txt_KuraiPavuMeter.Text) <> 0 And KuPvu_EdsCnt_ID <> 0 Then
                vNOOFENTRY = Common_Procedures.get_FieldValue(con, "Cloth_EndsCount_Details", "count(*)", "(Cloth_IdNo = " & Str(Val(vCLO_IDNO)) & " and EndsCount_IdNo = " & Str(Val(KuPvu_EdsCnt_ID)) & ")")
                If Val(vNOOFENTRY) = 0 Then
                    MessageBox.Show("The endcount (" & Trim(cbo_EndsCount.Text) & ") is wrong For the quality (" & Trim(UCase(cbo_ClothName.Text)) & ")", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_EndsCount.Enabled And cbo_EndsCount.Visible Then cbo_EndsCount.Focus()
                    Exit Sub
                End If
            End If
        End If

        End If

        Verified_STS = 0
        If chk_Verified_Status.Checked = True Then Verified_STS = 1


        vTotPvuBms = 0 : vTotPvuMtrs = 0 : vTotPvuPcs = 0
        If dgv_PavuDetails_Total.RowCount > 0 Then
            vTotPvuBms = Val(dgv_PavuDetails_Total.Rows(0).Cells(2).Value())
            vTotPvuPcs = Val(dgv_PavuDetails_Total.Rows(0).Cells(3).Value())
            vTotPvuMtrs = Val(dgv_PavuDetails_Total.Rows(0).Cells(5).Value())
        End If

        vCOMP_LEDIDNO = 0
        vDELVLED_COMPIDNO = 0

        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
            vCOMP_LEDIDNO = Common_Procedures.get_FieldValue(con, "Company_Head", "Sizing_To_LedgerIdNo", "(Company_idno = " & Str(Val(lbl_Company.Tag)) & ")")
            vDELVLED_COMPIDNO = Val(Common_Procedures.get_FieldValue(con, "Company_Head", "Company_idno", "(Sizing_To_LedgerIdNo = " & Str(Val(Delv_ID)) & ")"))
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT TEXTILES (SOMANUR)

            If SaveAll_STS = False Then
            vCurr_PavuStk = 0

            cmd.Connection = con

            cmd.CommandText = "Truncate table " & Trim(Common_Procedures.EntryTempSubTable) & ""
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & "( meters1 ) Select abs(a.Meters) from Stock_Pavu_Processing_Details a Where a.DeliveryTo_Idno = " & Str(Val(Delv_ID)) & " and a.EndsCount_IdNo <> 0 and a.Meters <> 0"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & "( meters1 ) Select -1*abs(a.Meters) from Stock_Pavu_Processing_Details a Where a.ReceivedFrom_Idno = " & Str(Val(Delv_ID)) & " and a.EndsCount_IdNo <> 0 and a.Meters <> 0"
            cmd.ExecuteNonQuery()

            Da = New SqlClient.SqlDataAdapter("select sum(meters1) from " & Trim(Common_Procedures.EntryTempSubTable) & " having sum(meters1) <> 0", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                    vCurr_PavuStk = Val(Dt1.Rows(0)(0).ToString)
                End If
            End If
            Dt1.Clear()

            vMax_PavuStk_Lvl = Val(Common_Procedures.get_FieldValue(con, "Ledger_Head", "Pavu_Stock_Maximum_Level", "(Ledger_IdNo = " & Str(Val(Delv_ID)) & ")"))
            If Val(vMax_PavuStk_Lvl) <> 0 Then
                If (Val(vCurr_PavuStk) + Val(vTotPvuMtrs) + Val(txt_KuraiPavuMeter.Text)) > Val(vMax_PavuStk_Lvl) Then
                    MessageBox.Show("Invalid Pau Stock : " & Trim(Format(Val(vCurr_PavuStk) + Val(vTotPvuMtrs) + Val(txt_KuraiPavuMeter.Text), "#########0.00")) & vbCrLf & "Greater than allowed maximum limit : " & Trim(Format(Val(vMax_PavuStk_Lvl), "#########0.00")) & vbCrLf & "Current Pavu Stock : " & Trim(Format(Val(vCurr_PavuStk), "#########0.00")), "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    'If txt_KuraiPavuMeter.Enabled Then txt_KuraiPavuMeter.Focus()
                    Exit Sub
                End If
            End If

        End If

        End If
        Delv_Ledtype = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(Delv_ID)) & ")")
        Rec_Ledtype = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(Rec_ID)) & ")")
        lbl_Freight_Pavu.Text = Val(Common_Procedures.get_FieldValue(con, "Ledger_Head", "Freight_Pavu", "(Ledger_IdNo = " & Str(Val(Delv_ID)) & ")"))

        Load_STS = 0
        If chk_Loaded.Checked = True Then Load_STS = 1

        cmd.Connection = con
        cmd.CommandText = "Truncate Table TempTable_For_NegativeStock"
        cmd.ExecuteNonQuery()

        weaver_job_code = ""

        If Trim(cbo_weaving_job_no.Text) <> "" Then
            weaver_job_code = Trim(cbo_weaving_job_no.Text)
        End If



        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_DcNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Pavu_Delivery_Head", "Weaver_Pavu_Delivery_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(msk_date.Text))

            vWEAPAVUDCNO = Trim(txt_DCPrefixNo.Text) & Trim(lbl_DcNo.Text) & Trim(cbo_DCSufixNo.Text)

            vCREATED_DTTM_TXT = ""
            vMODIFIED_DTTM_TXT = ""

            vCREATED_DTTM_TXT = Trim(Format(Now, "dd-MM-yyyy hh:mm tt"))
            cmd.Parameters.AddWithValue("@createddatetime", Now)


            vMODIFIED_DTTM_TXT = Trim(Format(Now, "dd-MM-yyyy hh:mm tt"))
            cmd.Parameters.AddWithValue("@modifieddatetime", Now)

            If New_Entry = True Then

                cmd.CommandText = "Insert into Weaver_Pavu_Delivery_Head ( Weaver_Pavu_Delivery_Code,              Company_IdNo        ,    Weaver_Pavu_Delivery_No  ,                               for_OrderBy                             , Weaver_Pavu_Delivery_Date,         DeliveryTo_IdNo  ,    ReceivedFrom_IdNo ,            EndsCount_IdNo         ,             Pavu_Meters              ,             Empty_Beam             ,                Vehicle_No           ,      Transport_Idno  ,             Total_Beam       ,              Total_Pcs       ,              Total_Meters     ,             Freight           ,                Party_DcNo            ,               Width_Type          ,                           User_idNo     ,                Ewave_Bill_No          ,           Value            ,             Freight_Pavu          ,             Rate          ,             Amount          ,         Transportation_Mode           ,            Date_Time_Of_Supply             ,                Place_Of_Supply       ,               Our_Order_No       ,               Own_Order_Code      ,         Verified_Status  ,             Cloth_IdNo     ,                 Crimp_Percentage    , Loaded_by_Our_employee,   Weaver_Pavu_Delivery_RefNo ,     Weaver_Pavu_Delivery_PrefixNo  ,     Weaver_Pavu_Delivery_SuffixNo  ,Total_pavu_Weight,rate_For,Job_Order_No,Job_Order_Date,Party_Ref_no,Party_Ref_Date ,Weaving_JobCode_forSelection, ClothSales_OrderCode_forSelection ,      created_useridno           ,   created_DateTime,          created_DateTime_Text    , Last_modified_useridno, Last_modified_DateTime, Last_modified_DateTime_Text ) " &
                                    "           Values                   (   '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(vWEAPAVUDCNO) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ",      @EntryDate          , " & Str(Val(Delv_ID)) & ",  " & Val(Rec_ID) & " ,  " & Str(Val(KuPvu_EdsCnt_ID)) & ", " & Val(txt_KuraiPavuMeter.Text) & " , " & Val(txt_KuraiPavuBeam.Text) & ",  '" & Trim(cbo_VehicleNo.Text) & "' , " & Val(Trans_ID) & ", " & Str(Val(vTotPvuBms)) & " , " & Str(Val(vTotPvuPcs)) & " , " & Str(Val(vTotPvuMtrs)) & " , " & Val(txt_Freight.Text) & " ,  '" & Trim(txt_Party_DcNo.Text) & "' , '" & Trim(cbo_WidthType.Text) & "', " & Val(Common_Procedures.User.IdNo) & ", '" & Trim(txt_Ewave_Bill_No.Text) & "', " & Val(txt_Value.Text) & ", " & Val(lbl_Freight_Pavu.Text) & ", " & Val(txt_rate.Text) & ", " & Val(lbl_Amount.Text) & ", '" & Trim(cbo_TransportMode.Text) & "', '" & Trim(txt_DateTime_Of_Supply.Text) & "', '" & Trim(txt_place_Supply.Text) & "', '" & Trim(lbl_OrderNo.Text) & "' , '" & Trim(lbl_OrderCode.Text) & "', " & Val(Verified_STS) & ", " & Str(Val(vCLO_IDNO)) & ", " & Str(Val(txt_CrimpPerc.Text)) & ", " & Val(Load_STS) & " , '" & Trim(lbl_DcNo.Text) & "', '" & Trim(txt_DCPrefixNo.Text) & "', '" & Trim(cbo_DCSufixNo.Text) & "'," & Str(Val(lbl_pavu_weight.Text)) & ",'" & Trim(cbo_Grid_RateFor.Text) & "','" & Trim(txt_JO_No.Text) & "','" & Trim(txt_JO_Date.Text) & "','" & Trim(txt_Ref_No.Text) & "','" & Trim(txt_Ref_date.Text) & "','" & Trim(weaver_job_code) & "', '" & Trim(cbo_Sales_OrderCode_forSelection.Text) & "' ,    " & Str(Val(Common_Procedures.User.IdNo)) & ",  @createddatetime ,  '" & Trim(vCREATED_DTTM_TXT) & "',              0        ,     NUll              ,          ''                 ) "
                cmd.ExecuteNonQuery()

            Else

                If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Pavu_Stock) = 1 Then

                    vENTDB_DelvToIDno = Val(Common_Procedures.get_FieldValue(con, "Weaver_Pavu_Delivery_Head", "DeliveryTo_IdNo", "(Weaver_Pavu_Delivery_Code = '" & Trim(NewCode) & "')", , tr))

                    If Val(vENTDB_DelvToIDno) <> Val(Delv_ID) Then

                        cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno    , EndsCount_IdNo ) " &
                                            " Select                                'PAVU'     , Reference_Code, Reference_Date, Company_IdNo, DeliveryTo_Idno, EndsCount_IdNo from Stock_Pavu_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                        cmd.ExecuteNonQuery()

                    End If

                End If

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Weaver_Pavu_Delivery_head", "Weaver_Pavu_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Weaver_Pavu_Delivery_Code, Company_IdNo, for_OrderBy", tr)
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Weaver_Pavu_Delivery_Details", "Weaver_Pavu_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " Set_No,Beam_No,Pcs ,Meters_Pc,Meters ,EndsCount_IdNo,Beam_Width_IdNo,Noof_Used,Set_Code", "Sl_No", "Weaver_Pavu_Delivery_Code, For_OrderBy, Company_IdNo, Weaver_Pavu_Delivery_No, Weaver_Pavu_Delivery_Date, Ledger_Idno", tr)

                cmd.CommandText = "Update Weaver_Pavu_Delivery_Head set  Weaver_Pavu_Delivery_RefNo = '" & Trim(lbl_DcNo.Text) & "' , Weaver_Pavu_Delivery_No = '" & Trim(vWEAPAVUDCNO) & "', Weaver_Pavu_Delivery_Date = @EntryDate, DeliveryTo_IdNo = " & Str(Val(Delv_ID)) & ", ReceivedFrom_IdNo = " & Str(Val(Rec_ID)) & ", Empty_Beam = " & Str(Val(txt_KuraiPavuBeam.Text)) & ", Pavu_Meters = " & Str(Val(txt_KuraiPavuMeter.Text)) & ", EndsCount_IdNo = " & Str(Val(KuPvu_EdsCnt_ID)) & ", Vehicle_No = '" & Trim(cbo_VehicleNo.Text) & "' , Transport_Idno = " & Str(Val(Trans_ID)) & ", Freight = " & Str(Val(txt_Freight.Text)) & ", Total_Beam = " & Str(Val(vTotPvuBms)) & ", Total_Pcs = " & Str(Val(vTotPvuPcs)) & ", Total_Meters = " & Str(Val(vTotPvuMtrs)) & ", Party_DcNo = '" & Trim(txt_Party_DcNo.Text) & "',Width_Type = '" & Trim(cbo_WidthType.Text) & "', User_IdNo = " & Val(Common_Procedures.User.IdNo) & ",Ewave_Bill_No = '" & Trim(txt_Ewave_Bill_No.Text) & "',Value = " & Val(txt_Value.Text) & ",Freight_Pavu  = " & Val(lbl_Freight_Pavu.Text) & ",Rate = " & Val(txt_rate.Text) & ", Amount = " & Val(lbl_Amount.Text) & ",Transportation_Mode = '" & Trim(cbo_TransportMode.Text) & "' ,Date_Time_Of_Supply = '" & Trim(txt_DateTime_Of_Supply.Text) & "' ,Place_Of_Supply = '" & Trim(txt_place_Supply.Text) & "',Our_Order_No = '" & Trim(lbl_OrderNo.Text) & "',Own_Order_Code = '" & Trim(lbl_OrderCode.Text) & "' ,Verified_Status= " & Val(Verified_STS) & " , Cloth_IdNo = " & Str(Val(vCLO_IDNO)) & ", Crimp_Percentage = " & Str(Val(txt_CrimpPerc.Text)) & " , Loaded_by_Our_employee = " & Val(Load_STS) & " ,Weaver_Pavu_Delivery_PrefixNo ='" & Trim(txt_DCPrefixNo.Text) & "' , Weaver_Pavu_Delivery_SuffixNo = '" & Trim(cbo_DCSufixNo.Text) & "',Total_pavu_Weight=" & Str(Val(lbl_pavu_weight.Text)) & ",Rate_For='" & Trim(cbo_Grid_RateFor.Text) & "',Job_Order_No='" & Trim(txt_JO_No.Text) & "',Job_Order_date='" & Trim(txt_JO_Date.Text) & "',party_ref_no='" & Trim(txt_Ref_No.Text) & "',party_ref_Date='" & Trim(txt_Ref_date.Text) & "', Weaving_JobCode_forSelection = '" & Trim(weaver_job_code) & "' , ClothSales_OrderCode_forSelection = '" & Trim(cbo_Sales_OrderCode_forSelection.Text) & "'  , Last_modified_useridno = " & Str(Val(Common_Procedures.User.IdNo)) & ", Last_modified_DateTime = @modifieddatetime, Last_modified_DateTime_Text = '" & Trim(vMODIFIED_DTTM_TXT) & "'  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Pavu_Delivery_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                Da = New SqlClient.SqlDataAdapter("Select * from Weaver_Pavu_Delivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Pavu_Delivery_Code = '" & Trim(NewCode) & "'", con)
                Da.SelectCommand.Transaction = tr
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then
                    For i = 0 To Dt1.Rows.Count - 1

                        cmd.CommandText = "update Stock_SizedPavu_Processing_Details set " _
                                  & " StockAt_IdNo = " & Str(Val(Dt1.Rows(i).Item("ReceivedFrom_IdNo").ToString)) & ", " _
                                  & " Pavu_Delivery_Increment = Pavu_Delivery_Increment - 1 " _
                                  & " Where " _
                                  & " StockAt_IdNo = " & Str(Val(Dt1.Rows(i).Item("DeliveryTo_IdNo").ToString)) & " and " _
                                  & " Set_Code = '" & Trim(Dt1.Rows(i).Item("Set_Code").ToString) & "' and " _
                                  & " beam_no = '" & Trim(Dt1.Rows(i).Item("Beam_No").ToString) & "' and " _
                                  & " Pavu_Delivery_Increment = " & Str(Val(Dt1.Rows(i).Item("Noof_Used").ToString))

                        cmd.ExecuteNonQuery()

                    Next
                End If
                Dt1.Clear()
                cmd.CommandText = "Update Weaver_PavuBobin_Requirement_Details set Sized_Beam_Return = a.Sized_Beam_Return - (b.Sized_Beam)  from Weaver_PavuBobin_Requirement_Details a, Weaver_Pavu_Delivery_Requirement_Details b Where b.Weaver_Pavu_Delivery_Requirement_Code = '" & Trim(NewCode) & "' and a.Weaver_PavuBobin_Requirement_Code = b.Weaver_PavuBobin_Requirement_Code and a.Weaver_PavuBobin_Requirement_SlNo = b.Weaver_PavuBobin_Requirement_SlNo"
                cmd.ExecuteNonQuery()
            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Weaver_Pavu_Delivery_head", "Weaver_Pavu_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Weaver_Pavu_Delivery_Code, Company_IdNo, for_OrderBy", tr)

            EntID = Trim(Pk_Condition) & Trim(lbl_DcNo.Text)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT TEXTILES (SOMANUR)
                Partcls = "Delv-Pavu: Dc.No. " & Trim(lbl_DcNo.Text) & ", Sizing Dc.No. " & Trim(txt_Party_DcNo.Text)
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1613" Then
                Partcls = "Delv-Pavu: " 'Dc.No. " & Trim(lbl_DcNo.Text)
            Else
                Partcls = "Delv-Pavu: Dc.No. " & Trim(lbl_DcNo.Text)
            End If

            PBlNo = Trim(txt_DCPrefixNo.Text) & Trim(lbl_DcNo.Text)

            Dim vPrtculrs_Setno As String = ""

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1116" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1380" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1446" Then
                Partcls = "Dc.No. " & Trim(lbl_DcNo.Text) & "-" & Val(vTotPvuBms) & "Beams" & "-" & Val(vTotPvuMtrs) & "Mtrs"

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1391" Then
                With dgv_PavuDetails
                    For i = 0 To dgv_PavuDetails.RowCount - 1
                        vPrtculrs_Setno = Trim(.Rows(i).Cells(1).Value)
                    Next

                End With

                Partcls = ""
                Partcls = "Delv-Pavu: Dc.No. " & Trim(lbl_DcNo.Text) & " Set No :" & vPrtculrs_Setno
            End If


            cmd.CommandText = "truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Weaver_Pavu_Delivery_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Pavu_Delivery_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Pavu_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Empty_BeamBagCone_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Delete from Weaver_Pavu_Delivery_Requirement_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Pavu_Delivery_Requirement_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Pavu_Delivery_Selections_Processing_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            If Val(txt_KuraiPavuMeter.Text) <> 0 And Val(KuPvu_EdsCnt_ID) <> 0 Then

                cmd.CommandText = "insert into " & Trim(Common_Procedures.EntryTempTable) & "(Int1, Int2, Meters1) values (" & Str(Val(KuPvu_EdsCnt_ID)) & ", " & Str(Val(txt_KuraiPavuBeam.Text)) & ", " & Str(Val(txt_KuraiPavuMeter.Text)) & ")"
                cmd.ExecuteNonQuery()

            End If

            With dgv_PavuDetails
                Sno = 0
                For i = 0 To dgv_PavuDetails.RowCount - 1

                    If Val(dgv_PavuDetails.Rows(i).Cells(5).Value) <> 0 Then

                        Sno = Sno + 1

                        SzPvu_EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, .Rows(i).Cells(6).Value, tr)
                        Bw_IdNo = Common_Procedures.BeamWidth_NameToIdNo(con, .Rows(i).Cells(7).Value, tr)

                        Ent_NoofUsed = 0
                        If Val(.Rows(i).Cells(9).Value) = 0 Or (Val(.Rows(i).Cells(9).Value) > 0 And Val(.Rows(i).Cells(9).Value) = Val(.Rows(i).Cells(11).Value)) Then

                            Nr = 0
                            cmd.CommandText = "update Stock_SizedPavu_Processing_Details set StockAt_IdNo = " & Str(Val(Delv_ID)) & ", Pavu_Delivery_Increment = Pavu_Delivery_Increment + 1 " &
                                                        " Where  Set_Code = '" & Trim(.Rows(i).Cells(10).Value) & "' and Beam_No = '" & Trim(.Rows(i).Cells(2).Value) & "' and StockAt_IdNo = " & Str(Val(Rec_ID))
                            Nr = cmd.ExecuteNonQuery()

                            If Nr = 0 Then
                                Throw New ApplicationException("Mismath Received From Name and Beam Details")
                                Exit Sub
                            End If

                            Ent_NoofUsed = Val(Common_Procedures.get_FieldValue(con, "Stock_SizedPavu_Processing_Details", "Pavu_Delivery_Increment", "(Set_Code = '" & Trim(.Rows(i).Cells(10).Value) & "' and Beam_No = '" & Trim(.Rows(i).Cells(2).Value) & "' )", , tr))

                        Else
                            Ent_NoofUsed = Val(.Rows(i).Cells(9).Value)

                        End If

                        cmd.CommandText = "Insert into Weaver_Pavu_Delivery_Details ( Weaver_Pavu_Delivery_Code,              Company_IdNo        ,     Weaver_Pavu_Delivery_No     ,                               for_OrderBy                          , Weaver_Pavu_Delivery_Date,         DeliveryTo_IdNo  ,    ReceivedFrom_IdNo     ,          Sl_No        ,                    Set_No              ,                    Beam_No             ,                      Pcs                 ,                      Meters_Pc           ,                      Meters              ,             EndsCount_IdNo       ,      Beam_Width_IdNo     ,              Noof_Used        ,                  Set_Code                 ,                    Weaver_LoomNo         ) " &
                                                    "            Values  (   '" & Trim(NewCode) & "'           , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ",        @EntryDate       ,   " & Str(Val(Delv_ID)) & ",  " & Str(Val(Rec_ID)) & ",  " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(SzPvu_EdsCnt_ID)) & ", " & Str(Val(Bw_IdNo)) & ", " & Str(Val(Ent_NoofUsed)) & ", '" & Trim(.Rows(i).Cells(10).Value) & "' , '" & Trim(.Rows(i).Cells(13).Value) & "' ) "
                        cmd.ExecuteNonQuery()


                        vPVUSTK_ENDSID = SzPvu_EdsCnt_ID

                        If Trim(LCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(LCase(Common_Procedures.settings.CustomerCode)) = "1155" Then
                            If KuPvu_EdsCnt_ID <> 0 Then
                                vPVUSTK_ENDSID = KuPvu_EdsCnt_ID
                            End If
                        End If

                        cmd.CommandText = "insert into " & Trim(Common_Procedures.EntryTempTable) & "(Int1, Int2, Meters1) values (" & Str(Val(vPVUSTK_ENDSID)) & ", 1, " & Str(Val(.Rows(i).Cells(5).Value)) & ")"
                        cmd.ExecuteNonQuery()

                    End If

                Next
                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Weaver_Pavu_Delivery_Details", "Weaver_Pavu_Delivery_Code", Val(lbl_Company.Tag), NewCode, lbl_DcNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, " Set_No,Beam_No,Pcs ,Meters_Pc,Meters ,EndsCount_IdNo,Beam_Width_IdNo,Noof_Used,Set_Code", "Sl_No", "Weaver_Pavu_Delivery_Code, For_OrderBy, Company_IdNo, Weaver_Pavu_Delivery_No, Weaver_Pavu_Delivery_Date, Ledger_Idno", tr)

            End With



            If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 And vDELVLED_COMPIDNO <> 0 Then

                If (Val(vTotPvuMtrs) + Val(txt_KuraiPavuMeter.Text)) <> 0 Then

                    cmd.CommandText = "Insert into Pavu_Delivery_Selections_Processing_Details (              Reference_Code                 ,                 Company_IdNo       ,           Reference_No       , for_OrderBy                                                           , Reference_Date,    Delivery_Code                             ,     Delivery_No               ,  DeliveryTo_Idno          , ReceivedFrom_Idno       ,        EndsCount_Idno        ,               Party_Dc_No           , Beam_Width_IdNo        ,               Total_Beams                                  ,              Total_Pcs       ,                 Total_Meters                                 , Set_No ,  Set_Code ,    Selection_Ledgeridno       ,          Selection_CompanyIdno      ) " &
                                        " Values                                               ('" & Trim(Pk_Condition) & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & "  , '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ", @EntryDate    , '" & Trim(Pk_Condition) & Trim(NewCode) & "' , '" & Trim(lbl_DcNo.Text) & "' , " & Str(Val(Delv_ID)) & " , " & Str(Val(Rec_ID)) & ", " & Str(Val(Ends_IdNo)) & "  , '" & Trim(txt_Party_DcNo.Text) & "' ,  " & Val(Bw_IdNo) & "  , " & Str(Val(vTotPvuBms) + Val(txt_KuraiPavuBeam.Text)) & " , " & Str(Val(vTotPvuPcs)) & " , " & Str(Val(vTotPvuMtrs) + Val(txt_KuraiPavuMeter.Text)) & " ,   ''   ,    ''     ," & Str(Val(vCOMP_LEDIDNO)) & ", " & Str(Val(vDELVLED_COMPIDNO)) & " ) "
                    cmd.ExecuteNonQuery()

                End If

            End If


            Da = New SqlClient.SqlDataAdapter("select Int1 as PavuEndsCount_IdNo, sum(Int2) as PavuBeam, sum(Meters1) as PavuMeters from " & Trim(Common_Procedures.EntryTempTable) & " group by Int1 having sum(Int2) <> 0 or sum(Meters1) <> 0", con)
            Da.SelectCommand.Transaction = tr
            Dt1 = New DataTable
            Da.Fill(Dt1)

            Sno = 0
            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    vTotPvuMtrs = 0
                    vTotPvuMtrs = Str(Val(Dt1.Rows(i).Item("PavuMeters").ToString))

                    Stock_In = ""
                    mtrspcs = 0
                    vPAVU_WIDTHMULTIPLIED_MTR = 0

                    Da = New SqlClient.SqlDataAdapter("Select Stock_In , Meters_Pcs from  EndsCount_Head Where EndsCount_IdNo = " & Str(Val(Dt1.Rows(i).Item("PavuEndsCount_IdNo").ToString)), con)
                    Da.SelectCommand.Transaction = tr
                    dt2 = New DataTable
                    Da.Fill(dt2)

                    If dt2.Rows.Count > 0 Then
                        Stock_In = dt2.Rows(0)("Stock_In").ToString
                        mtrspcs = Val(dt2.Rows(0)("Meters_Pcs").ToString)
                    End If
                    dt2.Clear()

                    Stk_DelvMtr = 0 : Stk_RecMtr = 0
                    If Trim(UCase(Stock_In)) = "PCS" Then
                        If Val(mtrspcs) = 0 Then mtrspcs = 1
                        vTotPvuStk = vTotPvuMtrs / mtrspcs

                        Stk_DelvMtr = vTotPvuStk
                        Stk_RecMtr = vTotPvuStk

                    Else

                        If Common_Procedures.settings.AutoLoom_PavuWidthWiseConsumption_IN_Delivery = 1 Then

                            vNoofBeams = 2
                            If Trim(cbo_WidthType.Text) <> "" Then
                                If InStr(1, Trim(UCase(cbo_WidthType.Text)), "1 BEAM") > 0 Then
                                    vNoofBeams = 1
                                ElseIf InStr(1, Trim(UCase(cbo_WidthType.Text)), "2 BEAM") > 0 Then
                                    vNoofBeams = 2
                                End If
                            End If

                            vWdTyp = 0
                            If Trim(UCase(cbo_WidthType.Text)) = "FOURTH" Or InStr(1, Trim(UCase(cbo_WidthType.Text)), "FOURTH") > 0 Or InStr(1, Trim(UCase(cbo_WidthType.Text)), "FOUR") > 0 Then
                                vWdTyp = 4
                            ElseIf Trim(UCase(cbo_WidthType.Text)) = "TRIPLE" Or InStr(1, Trim(UCase(cbo_WidthType.Text)), "TRIPLE") > 0 Then
                                vWdTyp = 3
                            ElseIf Trim(UCase(cbo_WidthType.Text)) = "DOUBLE" Or InStr(1, Trim(UCase(cbo_WidthType.Text)), "DOUBLE") > 0 Then
                                vWdTyp = 2
                            ElseIf Trim(UCase(cbo_WidthType.Text)) = "SINGLE" Or InStr(1, Trim(UCase(cbo_WidthType.Text)), "SINGLE") > 0 Then
                                vWdTyp = 1
                            End If

                            vTotPvuStkAlLoomMtr = Format(vTotPvuMtrs / vNoofBeams * vWdTyp, "###########0.00")

                            vPAVU_WIDTHMULTIPLIED_MTR = Format(Val(vTotPvuStkAlLoomMtr), "###########0.00")

                            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT TEXTTILES (SOMANUR)

                                If Val(txt_CrimpPerc.Text) <> 0 Then
                                    vTotPvuStkAlLoomMtr = Format(Val(vTotPvuStkAlLoomMtr) / (1 + (Val(txt_CrimpPerc.Text) / 100)), "###########0")
                                End If

                            Else

                                vCrmp_Mtrs = 0

                                If Common_Procedures.settings.AutoLoom_Pavu_CrimpMeters_Consumption_Stock_Posting_In_Delivery_Receipt_Entry = 1 Then
                                    vCrmp_Mtrs = Format(Val(vTotPvuStkAlLoomMtr) * Val(txt_CrimpPerc.Text) / 100, "###########0.00")
                                End If

                                vTotPvuStkAlLoomMtr = Format(Val(vTotPvuStkAlLoomMtr) - Val(vCrmp_Mtrs), "###########0.00")

                            End If



                            'If Trim(UCase(cbo_WidthType.Text)) = "FOURTH" Then
                            '    vWdTyp = 2
                            'ElseIf Trim(UCase(cbo_WidthType.Text)) = "TRIPLE" Then
                            '    vWdTyp = 1.5
                            'ElseIf Trim(UCase(cbo_WidthType.Text)) = "DOUBLE" Then
                            '    vWdTyp = 1
                            'Else
                            '    vWdTyp = 0.5
                            'End If
                            'vTotPvuStkAlLoomMtr = vTotPvuMtrs * vWdTyp

                            If Trim(UCase(Delv_Ledtype)) = "WEAVER" Then
                                Stk_DelvMtr = vTotPvuStkAlLoomMtr
                            Else
                                Stk_DelvMtr = vTotPvuMtrs
                            End If

                            If Trim(UCase(Rec_Ledtype)) = "WEAVER" Then
                                Stk_RecMtr = vTotPvuStkAlLoomMtr
                            Else
                                Stk_RecMtr = vTotPvuMtrs
                            End If

                        Else

                            vTotPvuStk = vTotPvuMtrs

                            Stk_DelvMtr = vTotPvuMtrs
                            Stk_RecMtr = vTotPvuMtrs

                        End If

                    End If

                    Sno = Sno + 1
                    cmd.CommandText = "Insert into Stock_Pavu_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, DeliveryToIdno_ForParticulars,ReceivedFromIdno_ForParticulars, Cloth_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, EndsCount_IdNo, Sized_Beam, Meters , Weaving_JobCode_forSelection, PavuMeter_BeforeCrimp , ClothSales_OrderCode_forSelection) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ", @EntryDate, " & Str(Val(Delv_ID)) & ", 0," & Str(Val(Delv_ID)) & "," & Str(Val(Rec_ID)) & ", 0, '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(Dt1.Rows(i).Item("PavuEndsCount_IdNo").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("PavuBeam").ToString)) & ", " & Str(Val(Stk_DelvMtr)) & " , '" & Trim(weaver_job_code) & "', " & Str(Val(vPAVU_WIDTHMULTIPLIED_MTR)) & ",'" & Trim(cbo_Sales_OrderCode_forSelection.Text) & "' )"
                    cmd.ExecuteNonQuery()

                    Sno = Sno + 1
                    cmd.CommandText = "Insert into Stock_Pavu_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno,DeliveryToIdno_ForParticulars,ReceivedFromIdno_ForParticulars, Cloth_Idno, Entry_ID, Party_Bill_No, Particulars, Sl_No, EndsCount_IdNo, Sized_Beam, Meters,Weaving_JobCode_forSelection ,ClothSales_OrderCode_forSelection ) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ", @EntryDate, 0, " & Str(Val(Rec_ID)) & "," & Str(Val(Delv_ID)) & "," & Str(Val(Rec_ID)) & ", 0, '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "', " & Str(Val(Sno)) & ", " & Str(Val(Dt1.Rows(i).Item("PavuEndsCount_IdNo").ToString)) & ", " & Str(Val(Dt1.Rows(i).Item("PavuBeam").ToString)) & ", " & Str(Val(Stk_RecMtr)) & ",'" & Trim(weaver_job_code) & "' ,'" & Trim(cbo_Sales_OrderCode_forSelection.Text) & "')"
                    cmd.ExecuteNonQuery()

                Next

            End If
            Dt1.Clear()

            If Val(txt_KuraiPavuBeam.Text) <> 0 Or Val(vTotPvuBms) <> 0 Then
                cmd.CommandText = "Insert into Stock_Empty_BeamBagCone_Processing_Details(Reference_Code, Company_IdNo, Reference_No, for_OrderBy, Reference_Date, DeliveryTo_Idno, ReceivedFrom_Idno, Party_Bill_No, Entry_ID, Particulars, Sl_No, Beam_Width_IdNo, Empty_Beam, Pavu_Beam) Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_DcNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_DcNo.Text))) & ", @EntryDate, " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(PBlNo) & "', '" & Trim(EntID) & "', '" & Trim(Partcls) & "', 1, 0, 0, " & Str(Val(txt_KuraiPavuBeam.Text) + Val(vTotPvuBms)) & ")"
                cmd.ExecuteNonQuery()
            End If

            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""
            Dim Tot_bem As Single = 0, frt_Pavu As Single = 0
            vLed_IdNos = Trans_ID & "|" & Val(Common_Procedures.CommonLedger.Transport_Charges_Ac)
            vVou_Amts = Val(txt_Freight.Text) & "|" & -1 * Val(txt_Freight.Text)
            If Common_Procedures.Voucher_Updation(con, "WP.Delv", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_DcNo.Text), Convert.ToDateTime(msk_date.Text), Partcls, vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                Throw New ApplicationException(ErrMsg)
            End If




            Tot_bem = Val(vTotPvuBms) + Val(txt_KuraiPavuBeam.Text)
            frt_Pavu = Format(Val(Tot_bem) * Val(lbl_Freight_Pavu.Text), "##########0.00")
            vLed_IdNos = Delv_ID & "|" & Val(Common_Procedures.CommonLedger.Freight_Charges_Ac)
            vVou_Amts = -1 * Val(frt_Pavu) & "|" & Val(frt_Pavu)
            If Common_Procedures.Voucher_Updation(con, "WP.Delv.Frg", Val(lbl_Company.Tag), Trim(Pk_Condition1) & Trim(NewCode), Trim(lbl_DcNo.Text), Convert.ToDateTime(msk_date.Text), Partcls & ", Beams : " & Tot_bem, vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                Throw New ApplicationException(ErrMsg)
            End If


            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1267" Then '---- BRT TEXTILES (SOMANUR)
                If Val(Common_Procedures.settings.Negative_Stock_Restriction_for_Pavu_Stock) = 1 Then

                    cmd.CommandText = "Insert into TempTable_For_NegativeStock ( Stock_Type, Reference_Code, Reference_Date, Company_Idno, Ledger_Idno      , EndsCount_IdNo ) " &
                                          " Select                               'PAVU'    , Reference_Code, Reference_Date, Company_IdNo, ReceivedFrom_Idno, EndsCount_IdNo from Stock_Pavu_Processing_Details where Reference_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                    cmd.ExecuteNonQuery()

                    If Common_Procedures.Check_is_Negative_Stock_Status(con, tr) = True Then Exit Sub

                End If
            End If



            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()

            If SaveAll_STS <> True Then
                If InStr(1, Err.Description, "CK_Weaver_PavuBobin_Requirement_Details_3") > 0 Then
                    MessageBox.Show("Invalid Delivery Sized Beam, Must be greater than zero", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                ElseIf InStr(1, Err.Description, "CK_Weaver_PavuBobin_Requirement_Details_4") > 0 Then
                    MessageBox.Show("Invalid  Delivery Sized Beam, Delivery Sized Beam must be lesser than Requirement Sized Beam", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Else
                    MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
                End If
            End If

            'MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

            If Val(Common_Procedures.settings.OnSave_MoveTo_NewEntry_Status) = 1 Then
                If New_Entry = True Then
                    new_record()
                Else
                    move_record(lbl_DcNo.Text)
                End If
            Else
                move_record(lbl_DcNo.Text)
            End If


        Catch ex As Exception
            tr.Rollback()
            If InStr(1, Err.Description, "CK_Weaver_PavuBobin_Requirement_Details_3") > 0 Then
                MessageBox.Show("Invalid Delivery Sized Beam, Must be greater than zero", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            ElseIf InStr(1, Err.Description, "CK_Weaver_PavuBobin_Requirement_Details_4") > 0 Then
                MessageBox.Show("Invalid  Delivery Sized Beam, Delivery Sized Beam must be lesser than Requirement Sized Beam", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If

        Finally
            If msk_date.Enabled And msk_date.Visible Then msk_date.Focus()

        End Try

    End Sub

    Private Sub cbo_DelvAt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DelvAt.GotFocus


        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
        End If

    End Sub

    Private Sub cbo_DelvAt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DelvAt.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DelvAt, Nothing, cbo_RecForm, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
        Else

            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_DelvAt, txt_place_Supply, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
        End If

        If (e.KeyCode = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

            If cbo_Sales_OrderCode_forSelection.Enabled And cbo_Sales_OrderCode_forSelection.Visible = True Then
                cbo_Sales_OrderCode_forSelection.Focus()
            ElseIf txt_DateTime_Of_Supply.Enabled And txt_DateTime_Of_Supply.Visible = True Then
                txt_DateTime_Of_Supply.Focus()
            Else
                txt_place_Supply.Focus()
            End If

        End If

            If (e.KeyCode = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If cbo_weaving_job_no.Enabled And cbo_weaving_job_no.Visible = True Then
                cbo_weaving_job_no.Focus()
            Else
                cbo_RecForm.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_DelvAt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DelvAt.KeyPress
        If Common_Procedures.settings.Textile_Sizing_Delivery_receipt_Selection = 1 Then
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DelvAt, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'GODOWN' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_DelvAt, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")
        End If

        If Asc(e.KeyChar) = 13 Then


            If cbo_weaving_job_no.Enabled And cbo_weaving_job_no.Visible = True Then
                cbo_weaving_job_no.Focus()
            Else

                If Common_Procedures.settings.Internal_Order_Entry_Status = 1 Then
                    If MessageBox.Show("Do you want to select Internal Order:", "FOR INTERNAL ORDER SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                        btn_OwnOrderSelection_Click(sender, e)

                    Else
                        cbo_RecForm.Focus()

                    End If

                Else
                    cbo_RecForm.Focus()

                End If
            End If
        End If
    End Sub

    Private Sub cbo_DelvAt_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DelvAt.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = "WEAVER"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_DelvAt.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_RecForm_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_RecForm.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 )  and Close_status = 0)", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_RecForm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_RecForm.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_RecForm, Nothing, cbo_EndsCount, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")

        If (e.KeyCode = 38 And cbo_RecForm.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If cbo_weaving_job_no.Enabled And cbo_weaving_job_no.Visible = True Then
                cbo_weaving_job_no.Focus()
            Else
                cbo_DelvAt.Focus()
            End If
        End If


    End Sub

    Private Sub cbo_RecForm_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_RecForm.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_RecForm, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "( ( Ledger_Type = 'WEAVER' or Ledger_Type = 'SIZING' or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 ) and Close_status = 0 )", "(Ledger_idno = 0)")

        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to Select Pavu", "FOR PAVU SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)

            Else
                cbo_EndsCount.Focus()

            End If

        End If

    End Sub

    Private Sub cbo_RecForm_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_RecForm.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = "SIZING"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_RecForm.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_EndsCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_EndsCount.GotFocus
        With cbo_EndsCount
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_idno = 0)")
            VEndsCountTag = cbo_EndsCount.Text
        End With
        VEndsCountTag = cbo_EndsCount.Text
    End Sub

    Private Sub cbo_EndsCount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EndsCount.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_EndsCount, cbo_RecForm, txt_KuraiPavuMeter, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_idno = 0)")
    End Sub

    Private Sub cbo_EndsCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_EndsCount.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_EndsCount, txt_KuraiPavuMeter, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_idno = 0)")
        If Asc(e.KeyChar) = 13 Then

            If Trim(UCase(VEndsCountTag)) <> Trim(UCase(cbo_EndsCount.Text)) Then
                GET_RATEDETAILS()
            End If
        End If
    End Sub

    Private Sub cbo_EndsCount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_EndsCount.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New EndsCount_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_EndsCount.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_Transport_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Transport.GotFocus


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1391" Then
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'SIZING')", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        End If


    End Sub

    Private Sub cbo_Transport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyDown
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1391" Then
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, cbo_VehicleNo, txt_Freight, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'SIZING')", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Transport, cbo_VehicleNo, txt_Freight, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        End If

    End Sub

    Private Sub cbo_Transport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Transport.KeyPress
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1391" Then
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, txt_Freight, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'SIZING')", "(Ledger_IdNo = 0)")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Transport, txt_Freight, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'TRANSPORT')", "(Ledger_IdNo = 0)")
        End If

    End Sub

    Private Sub cbo_Transport_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Transport.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Ledger_Creation
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1391" Then
                Common_Procedures.MDI_LedType = "SIZING"
            Else
                Common_Procedures.MDI_LedType = "TRANSPORT"
            End If

            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_Transport.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_VehicleNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_VehicleNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Weaver_Pavu_Delivery_Head", "Vehicle_No", "", "")

    End Sub

    Private Sub cbo_Vechile_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_VehicleNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_VehicleNo, txt_KuraiPavuBeam, cbo_Transport, "Weaver_Pavu_Delivery_Head", "Vehicle_No", "", "")
    End Sub

    Private Sub cbo_Vechile_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_VehicleNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_VehicleNo, cbo_Transport, "Weaver_Pavu_Delivery_Head", "Vehicle_No", "", "", False)
    End Sub

    Private Sub txt_Freight_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Freight.KeyDown

        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If (e.KeyValue = 40) Then
            If cbo_ClothName.Visible And cbo_ClothName.Enabled Then
                cbo_ClothName.Focus()
            ElseIf cbo_WidthType.Visible And cbo_WidthType.Enabled Then
                cbo_WidthType.Focus()
            Else
                txt_rate.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Freight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Freight.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If

        If Asc(e.KeyChar) = 13 Then
            If cbo_ClothName.Visible And cbo_ClothName.Enabled Then
                cbo_ClothName.Focus()
            ElseIf cbo_WidthType.Visible And cbo_WidthType.Enabled Then
                cbo_WidthType.Focus()
            Else
                txt_rate.Focus()
            End If
        End If


    End Sub

    Private Sub txt_KuraiPavuMeter_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_KuraiPavuMeter.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_KuraiPavuBeam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_KuraiPavuBeam.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub btn_Filter_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Close.Click
        pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        Filter_Status = False
    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        On Error Resume Next

        If IsNothing(dgv_Filter_Details.CurrentCell) Then Exit Sub

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(7).Value)

        If Val(movno) <> 0 Then
            Filter_Status = True
            move_record(movno)
            pnl_Back.Enabled = True
            pnl_Filter.Visible = False
        End If

    End Sub

    Private Sub btn_Filter_Show_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Filter_Show.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim n As Integer, i As Integer
        Dim Led_IdNo As Integer, EdsCnt_IdNo As Integer
        Dim Condt As String = ""
        Dim EdsEdsCnt_IdNo As Integer, Mil_IdNo As Integer
        Dim Verfied_Sts As Integer = 0

        Try

            Condt = ""
            Led_IdNo = 0
            EdsCnt_IdNo = 0
            Mil_IdNo = 0
            EdsEdsCnt_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Weaver_Pavu_Delivery_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Weaver_Pavu_Delivery_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Weaver_Pavu_Delivery_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_PartyName.Text) <> "" Then
                Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Filter_PartyName.Text)
            End If

            If Trim(cbo_Filter_EndsCountName.Text) <> "" Then
                EdsCnt_IdNo = Common_Procedures.EndsCount_NameToIdNo(con, cbo_Filter_EndsCountName.Text)
            End If

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.DeliveryTo_IdNo = " & Str(Val(Led_IdNo)) & " "
            End If

            If Val(EdsCnt_IdNo) <> 0 Then
                'Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.EndsCount_IdNo = " & Str(Val(EdsCnt_IdNo)) & " or d.EndsCount_IdNo = " & Str(Val(EdsCnt_IdNo)) & ") "
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.EndsCount_IdNo = " & Str(Val(EdsCnt_IdNo)) & " "
            End If

            If Trim(cbo_Verified_Sts.Text) = "YES" Then
                Verfied_Sts = 1
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Weaver_Pavu_Delivery_Code IN ( select z2.Weaver_Pavu_Delivery_Code from Weaver_Pavu_Delivery_Head z2 where z2.Verified_Status = " & Str(Val(Verfied_Sts)) & " )"
            ElseIf Trim(cbo_Verified_Sts.Text) = "NO" Then
                Verfied_Sts = 0
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " a.Weaver_Pavu_Delivery_Code IN ( select z2.Weaver_Pavu_Delivery_Code from Weaver_Pavu_Delivery_Head z2 where z2.Verified_Status = " & Str(Val(Verfied_Sts)) & " )"
            End If

            da = New SqlClient.SqlDataAdapter("select a.*, b.Ledger_Name , c.EndsCount_Name from Weaver_Pavu_Delivery_Head a INNER JOIN Weaver_Pavu_delivery_Details d on a.Weaver_Pavu_delivery_Code = d.Weaver_Pavu_delivery_Code INNER JOIN Ledger_Head b on a.DeliveryTo_IdNo = b.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head c on d.EndsCount_IdNo = c.EndsCount_IdNo  where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Pavu_Delivery_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Weaver_Pavu_Delivery_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Weaver_Pavu_Delivery_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Weaver_Pavu_Delivery_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Total_Pcs").ToString)
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(6).Value = dt2.Rows(i).Item("EndsCount_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(7).Value = dt2.Rows(i).Item("Weaver_Pavu_Delivery_RefNo").ToString


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

    Private Sub dgv_Filter_Details_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter_Details.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub

    Private Sub dtp_Filter_Fromdate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_Fromdate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            dtp_Filter_ToDate.Focus()
        End If
    End Sub

    Private Sub dtp_Filter_ToDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Filter_ToDate.KeyPress
        If Asc(e.KeyChar) = 13 Then
            cbo_Filter_PartyName.Focus()
        End If
    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "( Ledger_Type = 'WEAVER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_EndsCountName, "Ledger_AlaisHead", "Ledger_DisplayName", "( Ledger_Type = 'WEAVER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_EndsCountName, "Ledger_AlaisHead", "Ledger_DisplayName", "( Ledger_Type = 'WEAVER' or (Ledger_Type = '' and Stock_Maintenance_Status = 1) or Show_In_All_Entry = 1 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_EndsCountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_EndsCountName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_idno = 0)")
    End Sub


    Private Sub cbo_Filter_CountName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_EndsCountName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_EndsCountName, cbo_Filter_PartyName, Nothing, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")

        If e.KeyValue = 40 Or (e.Control = True And e.KeyValue = 40) Then
            If Common_Procedures.settings.CustomerCode = "1249" Then
                cbo_Verified_Sts.Focus()
            Else
                btn_Filter_Show.Focus()

            End If

        End If
    End Sub

    Private Sub cbo_Filter_CountName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_EndsCountName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_EndsCountName, Nothing, "EndsCount_Head", "EndsCount_Name", "", "(EndsCount_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            If Common_Procedures.settings.CustomerCode = "1249" Then
                cbo_Verified_Sts.Focus()
            Else
                btn_Filter_Show.Focus()

            End If
        End If
    End Sub

    Private Sub dgv_PavuDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_PavuDetails.EditingControlShowing
        dgtxt_Details = CType(dgv_PavuDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub
    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_PavuDetails.EditingControl.BackColor = Color.Lime
        dgv_PavuDetails.EditingControl.ForeColor = Color.Blue
        dgtxt_Details.SelectAll()
    End Sub
    Private Sub dgv_PavuDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellEndEdit
        TotalPavu_Calculation()
        'SendKeys.Send("{up}")
        'SendKeys.Send("{Tab}")
    End Sub

    Private Sub dgv_PavuDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellEnter
        With dgv_PavuDetails
            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If
        End With
    End Sub

    Private Sub dgv_PavuDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellLeave
        With dgv_PavuDetails
            If .CurrentCell.ColumnIndex = 4 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
        End With
    End Sub

    Private Sub dgv_PavuDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_PavuDetails.CellValueChanged
        Try


            If IsNothing(dgv_PavuDetails.CurrentCell) Then Exit Sub
            With dgv_PavuDetails
                If .Visible Then
                    If .CurrentCell.ColumnIndex = 5 Then
                        TotalPavu_Calculation()
                        Amount_Calculation()
                    End If
                End If
            End With

        Catch ex As Exception
            '---
        End Try
    End Sub

    Private Sub dgv_PavuDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_PavuDetails.KeyDown

        On Error Resume Next

        With dgv_PavuDetails

            If e.KeyCode = Keys.Up Then
                If .CurrentCell.RowIndex = 0 Then
                    .CurrentCell.Selected = False
                    e.SuppressKeyPress = True
                    e.Handled = True

                    If cbo_WidthType.Visible Then
                        cbo_WidthType.Focus()
                    Else
                        txt_Freight.Focus()
                    End If

                End If
            End If

            If e.KeyCode = Keys.Down Then
                If .CurrentCell.RowIndex = .RowCount - 1 Then
                    .CurrentCell.Selected = False
                    e.SuppressKeyPress = True
                    e.Handled = True

                End If
            End If

            If e.KeyCode = Keys.Enter Then
                e.SuppressKeyPress = True
                e.Handled = True

                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                    btn_save.Focus()
                Else
                    SendKeys.Send("{Tab}")

                End If

            End If

        End With

    End Sub

    Private Sub dgv_PavuDetails_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_PavuDetails.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_PavuDetails

                n = .CurrentRow.Index

                If Val(.Rows(n).Cells(8).Value) > 0 And Val(.Rows(n).Cells(8).Value) <> Val(.Rows(n).Cells(10).Value) Then
                    MessageBox.Show("Cannot Delete" & Chr(13) & "Already this pavu delivered to others")
                    Exit Sub
                End If

                If n = .Rows.Count - 1 Then

                    For i = 0 To .Columns.Count - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else

                    .Rows.RemoveAt(n)

                End If

                For i = 0 To .Rows.Count - 1
                    .Rows(i).Cells(0).Value = i + 1
                Next

            End With

            TotalPavu_Calculation()

        End If

    End Sub

    Private Sub dgv_PavuDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_PavuDetails.RowsAdded
        Dim n As Integer


        If IsNothing(dgv_PavuDetails.CurrentCell) Then Exit Sub
        With dgv_PavuDetails
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With
    End Sub

    Private Sub TotalPavu_Calculation()
        Dim Sno As Integer
        Dim TotBms As Single, TotMtrs As Single, TotPcs As Single

        Sno = 0
        TotBms = 0
        TotPcs = 0
        TotMtrs = 0
        With dgv_PavuDetails
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(5).Value) <> 0 Then
                    TotBms = TotBms + 1
                    TotMtrs = TotMtrs + Val(.Rows(i).Cells(5).Value)
                    TotPcs = TotPcs + Val(.Rows(i).Cells(3).Value)
                End If
            Next
        End With

        With dgv_PavuDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(2).Value = Val(TotBms)
            .Rows(0).Cells(3).Value = Val(TotPcs)
            .Rows(0).Cells(5).Value = Format(Val(TotMtrs), "########0.00")
        End With

    End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub btn_print_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String
        Dim PpSzSTS As Boolean = False
        Dim I As Integer = 0

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Weaver_Pavu_Delivery_Entry, New_Entry) = False Then Exit Sub

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* , c.* , d.EndsCount_Name , e.Ledger_Name as Trasport_Name from Weaver_Pavu_Delivery_Head a LEFT OUTER JOIN Ledger_Head b ON a.DeliveryTo_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.ReceivedFrom_IdNo = c.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo  where a.Weaver_Pavu_Delivery_Code = '" & Trim(NewCode) & "'", con)
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

        prn_TotCopies = 1
        Prnt_HalfSheet_STS = False

        vPrnt_2Copy_In_SinglePage = Common_Procedures.settings.WeaverWagesPavuDelivery_Print_2Copy_In_SinglePage

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1116" Then '---- Prakash Sizing (Somanur)

            Dim mymsgbox As New Tsoft_MessageBox("Select Paper Size to Print", "A4,HALF-SHEET,CANCEL", "FOR PRINTING...", "IF A4 is selected, 2 copies of dc will be printed in single A4 sheet," & Chr(13) & "If HALF-SHEET is selected 1 copy of dc will be printed in 8x6 paper size", MesssageBoxIcons.Questions, 2)
            mymsgbox.ShowDialog()

            If mymsgbox.MessageBoxResult = 1 Then
                vPrnt_2Copy_In_SinglePage = 1

            ElseIf mymsgbox.MessageBoxResult = 2 Then
                Prnt_HalfSheet_STS = True

                vPrnt_2Copy_In_SinglePage = 0

            Else

                Exit Sub

            End If

            prn_TotCopies = Val(InputBox("Enter No.of Copies", "FOR DELIVERY PRINTING...", "1"))
            If Val(prn_TotCopies) <= 0 Then
                Exit Sub
            End If

        End If


        set_PaperSize_For_PrintDocument1()
        'If Val(Common_Procedures.settings.WeaverWagesPavuDelivery_Print_2Copy_In_SinglePage) = 1 Then

        '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        'Debug.Print(ps.PaperName)
        '        If ps.Width = 800 And ps.Height = 600 Then
        '            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '            PrintDocument1.DefaultPageSettings.PaperSize = ps
        '            PpSzSTS = True
        '            Exit For
        '        End If
        '    Next

        'Else

        '    If PpSzSTS = False Then
        '        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
        '                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '                PrintDocument1.DefaultPageSettings.PaperSize = ps
        '                PpSzSTS = True
        '                Exit For
        '            End If
        '        Next

        '        If PpSzSTS = False Then
        '            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '                    PrintDocument1.DefaultPageSettings.PaperSize = ps
        '                    Exit For
        '                End If
        '            Next
        '        End If

        '    End If
        'End If


        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try

                If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then

                    'If Common_Procedures.settings.CustomerCode = "1376" Then
                    '    Try

                    '        'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8.25X12", 850, 1200)
                    '        'PrintDocument2.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
                    '        'PrintDocument2.DefaultPageSettings.PaperSize = pkCustomSize1

                    '        For I = 0 To PrintDocument2.PrinterSettings.PaperSizes.Count - 1
                    '            If PrintDocument2.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    '                ps = PrintDocument2.PrinterSettings.PaperSizes(I)
                    '                PrintDocument2.DefaultPageSettings.PaperSize = ps
                    '                Exit For
                    '            End If
                    '        Next

                    '        PrintDialog1.PrinterSettings = PrintDocument2.PrinterSettings
                    '        If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    '            PrintDocument2.PrinterSettings = PrintDialog1.PrinterSettings
                    '            PrintDocument2.Print()
                    '        End If

                    '    Catch ex As Exception
                    '        MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT SHOW PRINT PREVIEW...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                    '    End Try
                    'Else
                    '    PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    '    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    '        PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings



                    If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                        PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                        If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                            PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
                            PrintDocument1.Print()
                        End If

                    Else
                        PrintDocument1.Print()

                    End If

                    'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    '    'Debug.Print(ps.PaperName)
                    '    If ps.Width = 800 And ps.Height = 600 Then
                    '        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    '        PrintDocument1.DefaultPageSettings.PaperSize = ps
                    '        'e.PageSettings.PaperSize = ps
                    '        PpSzSTS = True
                    '        Exit For
                    '    End If
                    'Next

                    'If PpSzSTS = False Then
                    '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                    '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    '            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    '            PrintDocument1.DefaultPageSettings.PaperSize = ps
                    '            'e.PageSettings.PaperSize = ps
                    '            PpSzSTS = True
                    '            Exit For
                    '        End If
                    '    Next

                    '    If PpSzSTS = False Then
                    '        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    '            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    '                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    '                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    '                PrintDocument1.DefaultPageSettings.PaperSize = ps
                    '                'e.PageSettings.PaperSize = ps
                    '                Exit For
                    '            End If
                    '        Next
                    '    End If

                    'End If

                    'PrintDocument1.Print()
                    'End If

                    ' End If

                Else

                    set_PaperSize_For_PrintDocument1()

                    'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    '    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    '    'Debug.Print(ps.PaperName)
                    '    If ps.Width = 800 And ps.Height = 600 Then
                    '        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    '        PrintDocument1.DefaultPageSettings.PaperSize = ps
                    '        PpSzSTS = True
                    '        Exit For
                    '    End If
                    'Next

                    'If PpSzSTS = False Then
                    '    For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    '        If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.GermanStandardFanfold Then
                    '            ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    '            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    '            PrintDocument1.DefaultPageSettings.PaperSize = ps
                    '            PpSzSTS = True
                    '            Exit For
                    '        End If
                    '    Next

                    '    If PpSzSTS = False Then
                    '        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    '            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    '                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    '                PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    '                PrintDocument1.DefaultPageSettings.PaperSize = ps
                    '                Exit For
                    '            End If
                    '        Next
                    '    End If

                    'End If

                    PrintDocument1.Print()

                End If

            Catch ex As Exception
                MessageBox.Show("The printing operation failed" & vbCrLf & ex.Message, "DOES NOT SHOW PRINT PREVIEW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try


        Else
            Try
                If Common_Procedures.settings.CustomerCode = "1376" Then
                    Dim ppd As New PrintPreviewDialog

                    ppd.Document = PrintDocument2

                    ppd.WindowState = FormWindowState.Maximized
                    ppd.StartPosition = FormStartPosition.CenterScreen
                    ppd.PrintPreviewControl.AutoZoom = True
                    ppd.PrintPreviewControl.Zoom = 1.0

                    ppd.ShowDialog()

                Else
                    Dim ppd As New PrintPreviewDialog

                    ppd.Document = PrintDocument1

                    ppd.WindowState = FormWindowState.Maximized
                    ppd.StartPosition = FormStartPosition.CenterScreen
                    ppd.PrintPreviewControl.AutoZoom = True
                    ppd.PrintPreviewControl.Zoom = 1.0

                    ppd.ShowDialog()

                End If


            Catch ex As Exception
                MsgBox("The printing operation failed" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "DOES NOT SHOW PRINT PREVIEW...")

            End Try

        End If

    End Sub

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim DT1 As New DataTable
        Dim NewCode As String
        Dim Cmd As New SqlClient.SqlCommand


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_PageNo = 0
        prn_NoofBmDets = 0
        prn_DetMxIndx = 0
        prn_Count = 0
        prn_Prev_HeadIndx = -100
        prn_HeadIndx = 0

        Erase prn_DetAr

        prn_DetAr = New String(50, 10) {}

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* , c.Ledger_MainName as Receiver_Name ,C.Ledger_Address1 as RecAdd1,C.Ledger_Address2 as RecAdd2,C.Ledger_Address3 as RecAdd3,c.Ledger_Address4 as RecAdd4,c.Area_Idno as RecArea, d.EndsCount_Name , e.Ledger_Name  as Trasport_Name  , f.*,Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code ,ITH.item_gst_percentage from Weaver_Pavu_Delivery_Head a LEFT OUTER JOIN Ledger_Head b ON a.DeliveryTo_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.ReceivedFrom_IdNo = c.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo INNER JOIN Company_Head f ON a.Company_IdNo = f.Company_IdNo  LEFT OUTER JOIN State_Head Csh ON F.Company_State_IdNo = Csh.State_IdNo LEFT OUTER JOIN State_Head Lsh ON b.Ledger_State_IdNo = Lsh.State_IdNo  LEFT OUTER JOIN Count_Head CH ON d.Count_idno = CH.Count_idno LEFT OUTER JOIN itemgroup_head ITH ON CH.itemgroup_idno = ITH.itemgroup_idno   where a.Weaver_Pavu_Delivery_Code = '" & Trim(NewCode) & "'", con)
            'da1 = New SqlClient.SqlDataAdapter("select a.*, b.* , c.Ledger_MainName as Receiver_Name ,C.Ledger_Address1 as RecAdd1,C.Ledger_Address2 as RecAdd2,C.Ledger_Address3 as RecAdd3,c.Ledger_Address4 as RecAdd4,c.Area_Idno as RecArea, d.EndsCount_Name , e.Ledger_Name  as Trasport_Name  , f.*,Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code from Weaver_Pavu_Delivery_Head a LEFT OUTER JOIN Ledger_Head b ON a.DeliveryTo_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.ReceivedFrom_IdNo = c.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo INNER JOIN Company_Head f ON a.Company_IdNo = f.Company_IdNo  LEFT OUTER JOIN State_Head Csh ON F.Company_State_IdNo = Csh.State_IdNo LEFT OUTER JOIN State_Head Lsh ON b.Ledger_State_IdNo = Lsh.State_IdNo  where a.Weaver_Pavu_Delivery_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                Cmd.Connection = con

                Cmd.CommandText = "Truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
                Cmd.ExecuteNonQuery()

                da2 = New SqlClient.SqlDataAdapter("select a.* , b.*, d.*, e.* from Weaver_Pavu_Delivery_Details a LEFT OUTER JOIN EndsCount_Head b ON a.EndsCount_idno = b.EndsCount_idno LEFT OUTER JOIN Count_Head d ON b.Count_idno = d.Count_idno LEFT OUTER JOIN itemgroup_head e ON d.itemgroup_idno = e.itemgroup_idno where Weaver_Pavu_Delivery_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

                If prn_DetDt.Rows.Count > 0 Then
                    For i = 0 To prn_DetDt.Rows.Count - 1
                        If Val(prn_DetDt.Rows(i).Item("Meters").ToString) <> 0 Then
                            prn_DetMxIndx = prn_DetMxIndx + 1
                            prn_DetAr(prn_DetMxIndx, 1) = Trim(prn_DetDt.Rows(i).Item("Set_No").ToString)
                            prn_DetAr(prn_DetMxIndx, 2) = Trim(prn_DetDt.Rows(i).Item("Beam_No").ToString)
                            prn_DetAr(prn_DetMxIndx, 3) = Val(prn_DetDt.Rows(i).Item("Pcs").ToString)
                            prn_DetAr(prn_DetMxIndx, 4) = Format(Val(prn_DetDt.Rows(i).Item("Meters").ToString), "#########0.00")
                            prn_DetAr(prn_DetMxIndx, 5) = Trim(Microsoft.VisualBasic.Left(prn_DetDt.Rows(i).Item("EndsCount_Name").ToString, 15))
                            prn_DetAr(prn_DetMxIndx, 6) = Trim(prn_DetDt.Rows(i).Item("Item_HSN_Code").ToString)
                            prn_DetAr(prn_DetMxIndx, 7) = Format(Val(prn_DetDt.Rows(i).Item("Item_GST_Percentage").ToString), "#########0.00")
                            prn_DetAr(prn_DetMxIndx, 8) = Trim(prn_DetDt.Rows(i).Item("Sl_No").ToString)
                            prn_DetAr(prn_DetMxIndx, 9) = Val(prn_DetDt.Rows(i).Item("Meters_Pc").ToString)

                            Cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Int1, Meters1) values ('" & Trim(Microsoft.VisualBasic.Left(prn_DetDt.Rows(i).Item("EndsCount_Name").ToString, 15)) & "', 1, " & Str(Val(Format(Val(prn_DetDt.Rows(i).Item("Meters").ToString), "#########0.00"))) & ")"
                            Cmd.ExecuteNonQuery()

                        End If
                    Next i
                End If

                If Val(prn_HdDt.Rows(0).Item("Pavu_Meters").ToString) <> 0 Then
                    prn_DetMxIndx = prn_DetMxIndx + 1
                    prn_DetAr(prn_DetMxIndx, 1) = "-"
                    prn_DetAr(prn_DetMxIndx, 2) = "-"
                    prn_DetAr(prn_DetMxIndx, 3) = ""
                    prn_DetAr(prn_DetMxIndx, 4) = Format(Val(prn_HdDt.Rows(0).Item("Pavu_Meters").ToString), "#########0.00")
                    prn_DetAr(prn_DetMxIndx, 5) = ""
                    prn_DetAr(prn_DetMxIndx, 6) = ""
                    prn_DetAr(prn_DetMxIndx, 7) = ""
                    prn_DetAr(prn_DetMxIndx, 8) = Trim(Val(prn_DetMxIndx + 1))
                    prn_DetAr(prn_DetMxIndx, 9) = ""

                    da2 = New SqlClient.SqlDataAdapter("select b.*, d.*, e.* from EndsCount_Head b LEFT OUTER JOIN Count_Head d ON b.Count_idno = d.Count_idno LEFT OUTER JOIN itemgroup_head e ON d.itemgroup_idno = e.itemgroup_idno where b.EndsCount_idno = " & Str(Val(prn_HdDt.Rows(0).Item("EndsCount_IdNo").ToString)), con)
                    DT1 = New DataTable
                    da2.Fill(DT1)
                    If DT1.Rows.Count > 0 Then
                        prn_DetAr(prn_DetMxIndx, 5) = Trim(Microsoft.VisualBasic.Left(DT1.Rows(0).Item("EndsCount_Name").ToString, 15))
                        prn_DetAr(prn_DetMxIndx, 6) = Trim(DT1.Rows(0).Item("Item_HSN_Code").ToString)
                        prn_DetAr(prn_DetMxIndx, 7) = Format(Val(DT1.Rows(0).Item("Item_GST_Percentage").ToString), "#########0.00")

                        Cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Int1, Meters1) values ('" & Trim(Microsoft.VisualBasic.Left(DT1.Rows(0).Item("EndsCount_Name").ToString, 15)) & "', " & Str(Val(Format(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString), "#########0.00"))) & ", " & Str(Val(Format(Val(prn_HdDt.Rows(0).Item("Pavu_Meters").ToString), "#########0.00"))) & ")"
                        Cmd.ExecuteNonQuery()

                    End If
                    DT1.Clear()

                End If

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1242" Then

                    prn_DetMxIndx = 0
                    Erase prn_DetAr
                    prn_DetAr = New String(50, 10) {}

                    da2 = New SqlClient.SqlDataAdapter("Select a.Name1 as EndsCount, sum(Int1) as NoofBeams, sum(Meters1) as PavuMeters, e.Item_HSN_Code, e.Item_GST_Percentage from " & Trim(Common_Procedures.EntryTempTable) & " a LEFT OUTER JOIN EndsCount_Head b ON a.Name1 = b.EndsCount_Name LEFT OUTER JOIN Count_Head d ON b.Count_idno = d.Count_idno LEFT OUTER JOIN itemgroup_head e ON d.itemgroup_idno = e.itemgroup_idno Group by a.Name1, e.Item_HSN_Code, e.Item_GST_Percentage Order by a.Name1", con)
                    DT1 = New DataTable
                    da2.Fill(DT1)

                    If DT1.Rows.Count > 0 Then
                        For i = 0 To DT1.Rows.Count - 1
                            If Val(DT1.Rows(i).Item("PavuMeters").ToString) <> 0 Then

                                prn_DetMxIndx = prn_DetMxIndx + 1

                                prn_DetAr(prn_DetMxIndx, 1) = ""
                                prn_DetAr(prn_DetMxIndx, 2) = ""
                                prn_DetAr(prn_DetMxIndx, 3) = Val(DT1.Rows(i).Item("NoofBeams").ToString)
                                prn_DetAr(prn_DetMxIndx, 4) = Format(Val(DT1.Rows(i).Item("PavuMeters").ToString), "#########0.00")
                                prn_DetAr(prn_DetMxIndx, 5) = Trim(Microsoft.VisualBasic.Left(DT1.Rows(i).Item("EndsCount").ToString, 15))
                                prn_DetAr(prn_DetMxIndx, 6) = Trim(DT1.Rows(i).Item("Item_HSN_Code").ToString)
                                prn_DetAr(prn_DetMxIndx, 7) = Format(Val(DT1.Rows(i).Item("Item_GST_Percentage").ToString), "#########0.00")
                                prn_DetAr(prn_DetMxIndx, 8) = Trim(prn_DetMxIndx)
                                prn_DetAr(prn_DetMxIndx, 9) = ""

                            End If

                        Next i
                    End If

                End If


            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1242" Then
            Printing_Format2Gst(e)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1376" Then
            Printing_Format_1376(e)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1414" Then
            Printing_Format3_1414(e)
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
            Printing_Format1087(e)
        Else
            Printing_Format1(e)
        End If
    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer = 0, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0
        Dim cnt As Integer = 0

        PrntCnt = 1


        set_PaperSize_For_PrintDocument1()

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        Exit For
        '    End If
        'Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20
            .Right = 50
            .Top = 10 ' 20
            .Bottom = 40
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

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

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        pFont = New Font("Calibri", 11, FontStyle.Regular)

        NoofItems_PerPage = 5

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 65 : ClArr(2) = 55 : ClArr(3) = 50 : ClArr(4) = 75 : ClArr(5) = 120
        ClArr(6) = 65 : ClArr(7) = 55 : ClArr(8) = 50 : ClArr(9) = 75
        ClArr(10) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9))

        TxtHgt = 17.3  ' 17.5 ' 18.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20



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


        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_Prev_HeadIndx = prn_HeadIndx

        PrntCnt2ndPageSTS = False
        TpMargin = TMargin

        If vPrnt_2Copy_In_SinglePage = 1 Then
            PCnt = PCnt + 1
            PrntCnt = PrntCnt + 1
        End If

        For PCnt = 1 To PrntCnt

            If vPrnt_2Copy_In_SinglePage = 1 Then

                If PCnt = 1 Then
                    prn_PageNo1 = prn_PageNo

                    prn_DetIndx1 = prn_DetIndx
                    prn_DetSNo1 = prn_DetSNo
                    prn_NoofBmDets1 = prn_NoofBmDets
                    TpMargin = TMargin


                Else

                    prn_PageNo = prn_PageNo1
                    prn_NoofBmDets = prn_NoofBmDets1
                    prn_DetIndx = prn_DetIndx1
                    prn_DetSNo = prn_DetSNo1

                    TpMargin = 560 + TMargin  ' 600 + TMargin

                End If

            End If


            Try

                If prn_HdDt.Rows.Count > 0 Then

                    Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TpMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                    NoofDets = 0

                    NoofItems_PerPage = 5
                    If Val(vPrnt_2Copy_In_SinglePage) = 1 Then
                        If prn_DetDt.Rows.Count > (2 * NoofItems_PerPage) Then
                            If Trim(Common_Procedures.settings.CustomerCode) = "1037" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1116" Then
                                NoofDets = NoofDets + 1
                                NoofItems_PerPage = 6
                            Else
                                NoofItems_PerPage = 35
                            End If
                        End If
                    End If


                    CurY = CurY - 10

                    'If prn_DetDt.Rows.Count > 0 Or prn_DetMxIndx > 0 Then
                    If prn_DetMxIndx > 0 Then

                        Do While prn_NoofBmDets < prn_DetMxIndx

                            If NoofDets >= NoofItems_PerPage Then

                                If Val(vPrnt_2Copy_In_SinglePage) = 1 Then

                                    If PCnt = 2 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then

                                        NoofDets = NoofDets + 1

                                        CurY = CurY + TxtHgt

                                        Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)


                                        Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, PCnt, False)

                                        prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                                        e.HasMorePages = True

                                        Return

                                    End If

                                Else

                                    CurY = CurY + TxtHgt

                                    Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)


                                    Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, PCnt, False)

                                    prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                                    e.HasMorePages = True

                                    Return

                                End If

                            End If

                            prn_DetIndx = prn_DetIndx + 1

                            If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then
                                CurY = CurY + TxtHgt
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 1)), LMargin + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 2)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                If Val(prn_DetAr(prn_DetIndx, 3)) <> 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 3))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, pFont)
                                End If
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 5, CurY, 0, ClArr(5), pFont,, True)

                                prn_NoofBmDets = prn_NoofBmDets + 1

                            End If

                            If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)) <> 0 Then

                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 1)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 12, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 2)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, pFont)
                                If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 3)) <> 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 3))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                                End If

                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + 5, CurY, 0, ClArr(5), pFont,, True)

                                prn_NoofBmDets = prn_NoofBmDets + 1

                            End If

                            NoofDets = NoofDets + 1

                        Loop

                    End If


                    Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, PCnt, True)

                End If

            Catch ex As Exception
                MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try

            If Val(vPrnt_2Copy_In_SinglePage) = 1 Then
                If PCnt = 1 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then
                    If prn_DetDt.Rows.Count > prn_NoofBmDets Then
                        PrntCnt2ndPageSTS = True
                        e.HasMorePages = True
                        cnt = 6
                        Return
                    End If
                End If
            End If

            PrntCnt2ndPageSTS = False

        Next PCnt


LOOP2:

        prn_Count = prn_Count + 1

        If Val(prn_TotCopies) > 1 Then
            If prn_Count < Val(prn_TotCopies) Then

                prn_DetIndx = 0
                'prn_DetSNo = 0
                prn_PageNo = 0
                prn_DetIndx = 0
                prn_NoofBmDets = 0


                e.HasMorePages = True
                Return

            Else
                e.HasMorePages = False

            End If

        Else

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

        End If

    End Sub

    Private Sub Printing_Format1_222(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer = 0, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0
        Dim cnt As Integer = 0

        PrntCnt = 1


        set_PaperSize_For_PrintDocument1()

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        Exit For
        '    End If
        'Next


        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20
            .Right = 50
            .Top = 20
            .Bottom = 40
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

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

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        pFont = New Font("Calibri", 11, FontStyle.Regular)

        NoofItems_PerPage = 5

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 65 : ClArr(2) = 55 : ClArr(3) = 50 : ClArr(4) = 75 : ClArr(5) = 120
        ClArr(6) = 65 : ClArr(7) = 55 : ClArr(8) = 50 : ClArr(9) = 75
        ClArr(10) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9))

        TxtHgt = 18.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_Prev_HeadIndx = prn_HeadIndx

        PrntCnt2ndPageSTS = False
        TpMargin = TMargin

        If Trim(Common_Procedures.settings.CustomerCode) = "1037" And (vPrnt_2Copy_In_SinglePage) = 1 Then
            PCnt = PCnt + 1
            PrntCnt = PrntCnt + 1
        End If


        For PCnt = 1 To PrntCnt

            If Val(vPrnt_2Copy_In_SinglePage) = 1 Then

                If PCnt = 1 Then
                    prn_PageNo1 = prn_PageNo

                    prn_DetIndx1 = prn_DetIndx
                    prn_DetSNo1 = prn_DetSNo
                    prn_NoofBmDets1 = prn_NoofBmDets
                    TpMargin = TMargin


                Else

                    prn_PageNo = prn_PageNo1
                    prn_NoofBmDets = prn_NoofBmDets1
                    prn_DetIndx = prn_DetIndx1
                    prn_DetSNo = prn_DetSNo1

                    TpMargin = 560 + TMargin  ' 600 + TMargin

                End If

            End If


            Try

                If prn_HdDt.Rows.Count > 0 Then

                    Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TpMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                    NoofDets = 0

                    NoofItems_PerPage = 5
                    If Val(vPrnt_2Copy_In_SinglePage) = 1 Then
                        If prn_DetDt.Rows.Count > (2 * NoofItems_PerPage) Then
                            If Trim(Common_Procedures.settings.CustomerCode) = "1037" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1116" Then
                                NoofDets = NoofDets + 1
                                NoofItems_PerPage = 6
                            Else
                                NoofItems_PerPage = 35
                            End If
                        End If
                    End If


                    CurY = CurY - 10

                    'If prn_DetDt.Rows.Count > 0 Or prn_DetMxIndx > 0 Then
                    If prn_DetDt.Rows.Count > 0 Then

                        Do While prn_NoofBmDets < prn_DetMxIndx

                            If NoofDets >= NoofItems_PerPage Then

                                If Val(vPrnt_2Copy_In_SinglePage) = 1 Then

                                    If PCnt = 2 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then

                                        NoofDets = NoofDets + 1

                                        CurY = CurY + TxtHgt

                                        Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)


                                        Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, PCnt, False)

                                        prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                                        e.HasMorePages = True

                                        Return

                                    End If

                                Else

                                    CurY = CurY + TxtHgt

                                    Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)


                                    Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, PCnt, False)

                                    prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                                    e.HasMorePages = True

                                    Return

                                End If

                            End If

                            prn_DetIndx = prn_DetIndx + 1

                            If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then
                                CurY = CurY + TxtHgt
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 1)), LMargin + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 2)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                If Val(prn_DetAr(prn_DetIndx, 3)) <> 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 3))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, pFont)
                                End If
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)

                                prn_NoofBmDets = prn_NoofBmDets + 1

                            End If

                            If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)) <> 0 Then

                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 1)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 12, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 2)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, pFont)
                                If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 3)) <> 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 3))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                                End If

                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + 10, CurY, 0, 0, pFont)

                                prn_NoofBmDets = prn_NoofBmDets + 1

                            End If

                            NoofDets = NoofDets + 1

                        Loop

                    End If


                    Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, PCnt, True)

                End If

            Catch ex As Exception

                MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try


            If Val(vPrnt_2Copy_In_SinglePage) = 1 Then

                If PCnt = 1 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then
                    If prn_DetDt.Rows.Count > prn_NoofBmDets Then
                        PrntCnt2ndPageSTS = True
                        e.HasMorePages = True
                        cnt = 6
                        Return
                    End If
                End If
            End If

            PrntCnt2ndPageSTS = False

        Next PCnt


LOOP2:

        prn_Count = prn_Count + 1


        e.HasMorePages = False

        If Val(prn_TotCopies) > 1 Then
            If prn_Count < Val(prn_TotCopies) Then

                prn_DetIndx = 0
                'prn_DetSNo = 0
                prn_PageNo = 0
                prn_DetIndx = 0
                prn_NoofBmDets = 0


                e.HasMorePages = True
                Return

            End If

        Else

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


        End If


    End Sub

    Private Sub Printing_Format1_111(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ps As Printing.PaperSize
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0
        Dim cnt As Integer = 0

        PrntCnt = 1

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
            .Top = 20
            .Bottom = 40
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

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

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        pFont = New Font("Calibri", 11, FontStyle.Regular)

        NoofItems_PerPage = 5

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 65 : ClArr(2) = 55 : ClArr(3) = 50 : ClArr(4) = 75 : ClArr(5) = 120
        ClArr(6) = 65 : ClArr(7) = 55 : ClArr(8) = 50 : ClArr(9) = 75
        ClArr(10) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9))

        TxtHgt = 18.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_Prev_HeadIndx = prn_HeadIndx

        PrntCnt2ndPageSTS = False
        TpMargin = TMargin

        If Trim(Common_Procedures.settings.CustomerCode) = "1037" And (vPrnt_2Copy_In_SinglePage) = 1 Then
            PCnt = PCnt + 1
            PrntCnt = PrntCnt + 1
        End If


        For PCnt = 1 To PrntCnt

            If Val(vPrnt_2Copy_In_SinglePage) = 1 Then

                If PCnt = 1 Then
                    prn_PageNo1 = prn_PageNo

                    prn_DetIndx1 = prn_DetIndx
                    prn_DetSNo1 = prn_DetSNo
                    prn_NoofBmDets1 = prn_NoofBmDets
                    TpMargin = TMargin


                Else

                    prn_PageNo = prn_PageNo1
                    prn_NoofBmDets = prn_NoofBmDets1
                    prn_DetIndx = prn_DetIndx1
                    prn_DetSNo = prn_DetSNo1

                    TpMargin = 560 + TMargin  ' 600 + TMargin

                End If

            End If


            Try

                If prn_HdDt.Rows.Count > 0 Then

                    Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TpMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                    NoofDets = 0

                    NoofItems_PerPage = 5
                    If Val(vPrnt_2Copy_In_SinglePage) = 1 Then
                        If prn_DetDt.Rows.Count > (2 * NoofItems_PerPage) Then
                            If Trim(Common_Procedures.settings.CustomerCode) = "1037" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1116" Then
                                NoofDets = NoofDets + 1
                                NoofItems_PerPage = 6
                            Else
                                NoofItems_PerPage = 35
                            End If
                        End If
                    End If


                    CurY = CurY - 10

                    'If prn_DetDt.Rows.Count > 0 Or prn_DetMxIndx > 0 Then
                    If prn_DetDt.Rows.Count > 0 Then

                        Do While prn_NoofBmDets < prn_DetMxIndx

                            If NoofDets >= NoofItems_PerPage Then

                                If Val(vPrnt_2Copy_In_SinglePage) = 1 Then

                                    If PCnt = 2 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then

                                        NoofDets = NoofDets + 1

                                        CurY = CurY + TxtHgt

                                        Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)


                                        Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, PCnt, False)

                                        prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                                        e.HasMorePages = True

                                        Return

                                    End If

                                Else

                                    CurY = CurY + TxtHgt

                                    Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)


                                    Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, PCnt, False)

                                    prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                                    e.HasMorePages = True

                                    Return

                                End If

                            End If

                            prn_DetIndx = prn_DetIndx + 1

                            If PCnt <> 2 Then

                                If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then

                                    If PCnt = 1 And NoofDets < NoofItems_PerPage Then

                                        CurY = CurY + TxtHgt
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 1)), LMargin + 10, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 2)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                        If Val(prn_DetAr(prn_DetIndx, 3)) <> 0 Then
                                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 3))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, pFont)
                                        End If
                                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 5, CurY, 0, 0, pFont)

                                    End If

                                    prn_NoofBmDets = prn_NoofBmDets + 1

                                End If

                                If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)) <> 0 Then
                                    If PCnt = 1 And NoofDets < NoofItems_PerPage Then
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 1)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 12, CurY, 0, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 2)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, pFont)
                                        If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 3)) <> 0 Then
                                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 3))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                                        End If

                                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + 5, CurY, 0, 0, pFont)

                                    End If

                                    prn_NoofBmDets = prn_NoofBmDets + 1

                                End If

                                NoofDets = NoofDets + 1

                            End If


                            If PCnt = 2 Then

                                If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then
                                    CurY = CurY + TxtHgt
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 1)), LMargin + 10, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 2)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                    If Val(prn_DetAr(prn_DetIndx, 3)) <> 0 Then
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 3))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, pFont)
                                    End If
                                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)

                                    prn_NoofBmDets = prn_NoofBmDets + 1

                                End If

                                If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)) <> 0 Then

                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 1)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 12, CurY, 0, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 2)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, pFont)
                                    If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 3)) <> 0 Then
                                        Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 3))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                                    End If

                                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + 10, CurY, 0, 0, pFont)

                                    prn_NoofBmDets = prn_NoofBmDets + 1

                                End If

                                NoofDets = NoofDets + 1

                            End If

                        Loop

                    End If


                    Printing_Format1_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, PCnt, True)

                End If

            Catch ex As Exception

                MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try


            If Val(vPrnt_2Copy_In_SinglePage) = 1 Then

                If PCnt = 1 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then
                    If prn_DetDt.Rows.Count >= cnt + 6 Then
                        PrntCnt2ndPageSTS = True
                        e.HasMorePages = True
                        cnt = 6
                        Return
                    End If
                End If
            End If

            PrntCnt2ndPageSTS = False

        Next PCnt


LOOP2:

        prn_Count = prn_Count + 1


        e.HasMorePages = False

        If Val(prn_TotCopies) > 1 Then
            If prn_Count < Val(prn_TotCopies) Then

                prn_DetIndx = 0
                'prn_DetSNo = 0
                prn_PageNo = 0
                prn_DetIndx = 0
                prn_NoofBmDets = 0


                e.HasMorePages = True
                Return

            End If
        Else
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


        End If


    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim W1 As Single, W2 As Single, N1 As Single, M1 As Single
        Dim Arr(300, 5) As String
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_PANNo As String, Cmp_GSTNo As String
        Dim strWidth As Single = 0
        Dim Entry_Date As Date = Convert.ToDateTime(prn_HdDt.Rows(0).Item("Weaver_Pavu_Delivery_Date"))

        PageNo = PageNo + 1

        CurY = TMargin

        If prn_DetMxIndx > (2 * NoofItems_PerPage) Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        CurY = TMargin
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_PANNo = "" : Cmp_GSTNo = ""

        Dim vADD_BOLD_STS As Boolean = False

        vADD_BOLD_STS = False
        If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")
            Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString)

            Dim vADDR1 As String = ""

            vADDR1 = Trim(prn_HdDt.Rows(0).Item("Company_Address1").ToString)
            vADDR1 = Replace(vADDR1, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) & ",", "")
            vADDR1 = Replace(vADDR1, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")

            Cmp_Add2 = vADDR1 & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

            vADD_BOLD_STS = True

        Else

            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
            Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_PANNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTNo = "GSTIN : " & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_logo_Image").ToString) <> "" Then

            If IsDBNull(prn_HdDt.Rows(0).Item("Company_logo_Image")) = False Then

                Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("Company_logo_Image"), Byte())
                If Not imageData Is Nothing Then
                    Using ms As New MemoryStream(imageData, 0, imageData.Length)
                        ms.Write(imageData, 0, imageData.Length)

                        If imageData.Length > 0 Then

                            '.BackgroundImage = Image.FromStream(ms)

                            ' e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 108, CurY + 10, 90, 90)
                            e.Graphics.DrawImage(DirectCast(Image.FromStream(ms), Drawing.Image), LMargin + 15, CurY + 5, 100, 90)

                        End If

                    End Using

                End If

            End If

        End If
        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight - 2
        If vADD_BOLD_STS = True Then    '------(ie) company division name in 2nd line
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)
            strHeight = TxtHgt

        End If

        If Trim(Cmp_Add2) <> "" Then
            CurY = CurY + strHeight - 1
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        End If

        If Trim(Cmp_PhNo) <> "" Then
            CurY = CurY + TxtHgt - 1
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        End If

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, " STATE : " & Trim(Common_Procedures.State_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("company_State_Idno").ToString))) & "         " & Trim(Cmp_GSTNo) & "          " & Cmp_PANNo, LMargin + 10, CurY, 2, PrintWidth, pFont)

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1234" Then
        '    Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTNo & "   /    " & Cmp_PANNo, LMargin, CurY, 2, PrintWidth, pFont)
        '    CurY = CurY + TxtHgt
        '    Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)

        'Else
        '    CurY = CurY + TxtHgt
        '    Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTNo & "   /    " & Cmp_PANNo, LMargin + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, PageWidth - 10, CurY, 1, PrintWidth, pFont)
        'End If


        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "PAVU DELIVERY TO JOBWORK", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight - 3

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1395" Then '---- SANTHA EXPORTS (SOMANUR)

            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC CODE : 998821 ( Textile manufactring service )", LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        Else

            p1Font = New Font("Calibri", 10, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "( NOT FOR SALE )", LMargin + 10, CurY, 0, 0, p1Font)
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC CODE : 998821 ( Textile manufactring service )", PageWidth - 10, CurY, 1, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "( NOT FOR SALE )", LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        End If


        CurY = CurY + strHeight ' + 3
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            N1 = e.Graphics.MeasureString("TO   : ", pFont).Width
            W1 = e.Graphics.MeasureString("SET NO  :  ", pFont).Width
            W2 = e.Graphics.MeasureString("E-Way Bill No  :", pFont).Width

            'If Common_Procedures.settings.CustomerCode = "1391" Then
            M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7)
            'Else
            '    M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
            'End If


            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 11, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "TO :  M/s. " & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W2 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Weaver_Pavu_Delivery_No").ToString, LMargin + M1 + W2 + 25, CurY, 0, 0, p1Font)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont,, True)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Weaver_Pavu_Delivery_Date")), "dd-MM-yyyy"), LMargin + M1 + W2 + 25, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont,, True)
            If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Vehicle No", LMargin + M1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + M1 + W2 + 25, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + N1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + N1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + N1 + W1 + 30, CurY, 0, 0, pFont)

            Else

                If Trim(prn_HdDt.Rows(0).Item("Pan_No").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "PAN", LMargin + N1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + N1 + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Pan_No").ToString, LMargin + N1 + W1 + 30, CurY, 0, 0, pFont)
                End If

            End If

            If Trim(prn_HdDt.Rows(0).Item("Ewave_Bill_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "E-Way Bill No", LMargin + M1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Ewave_Bill_No").ToString), LMargin + M1 + W2 + 25, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + M1, LnAr(3), LMargin + M1, LnAr(2))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BEAM", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "ENDS COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 3, CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BEAM", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "ENDS COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


    End Sub

    Private Sub Printing_Format1_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean, ByVal Pcnt As Integer)
        Dim p1Font As Font
        Dim p2Font As Font
        Dim Cmp_Name As String
        Dim I As Integer
        Dim M1 As Single
        Dim Area_Nm As String = ""
        Dim LedAdd1 As String = ""
        Dim LedAdd2 As String = ""
        Dim LedAdd3 As String = ""
        Dim LedAdd4 As String = ""
        Dim W1 As Single, W2 As Single, W3 As Single
        Dim C1 As Single, C2 As Single
        Dim vTxamt As String = 0
        Dim vNtAMt As String = 0
        Dim vTxPerc As String = 0
        Dim vSgst_amt As String = 0
        Dim vCgst_amt As String = 0
        Dim vIgst_amt As String = 0


        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))

            CurY = CurY + TxtHgt - 10

            If is_LastPage = True Then

                If (prn_DetMxIndx Mod (NoofItems_PerPage * 2)) <= NoofItems_PerPage Then

                    If (Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString) + Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString) + Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString), LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
                    End If
                    If Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
                    End If
                    If (Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) + Val(prn_HdDt.Rows(0).Item("Pavu_Meters").ToString)) Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) + Val(prn_HdDt.Rows(0).Item("Pavu_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                    End If

                Else

                    If (Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString) + Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString) + Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    End If
                    If Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    End If

                    If (Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) + Val(prn_HdDt.Rows(0).Item("Pavu_Meters").ToString)) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) + Val(prn_HdDt.Rows(0).Item("Pavu_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                    End If

                End If

            End If


            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 4, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 4, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))

            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + 20, CurY, LMargin + ClAr(1) + ClAr(2) + 20, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 5, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 5, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 5, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 5, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 50, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 50, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 50, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 50, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + 50, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + 50, LnAr(3))

            M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 30
            If Val(prn_HdDt.Rows(0).Item("RecArea").ToString) <> 0 Then
                Area_Nm = Common_Procedures.Area_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("RecArea").ToString))
            ElseIf Trim(prn_HdDt.Rows(0).Item("RecAdd4").ToString) <> "" Then
                LedAdd1 = Trim(prn_HdDt.Rows(0).Item("RecAdd4").ToString & " " & prn_HdDt.Rows(0).Item("RecAdd3").ToString)
            ElseIf Trim(prn_HdDt.Rows(0).Item("RecAdd3").ToString) <> "" Then
                LedAdd2 = Trim(prn_HdDt.Rows(0).Item("RecAdd3").ToString & " " & prn_HdDt.Rows(0).Item("RecAdd2").ToString)
            ElseIf Trim(prn_HdDt.Rows(0).Item("RecAdd2").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("RecAdd1").ToString) <> "" Then
                LedAdd3 = Trim(prn_HdDt.Rows(0).Item("RecAdd2").ToString & " " & prn_HdDt.Rows(0).Item("RecAdd1").ToString)
            End If


            p2Font = New Font("Calibri", 9, FontStyle.Regular)

            W1 = e.Graphics.MeasureString("Received From  :  ", p2Font).Width
            W2 = e.Graphics.MeasureString("CGST @ 2.5% :", p2Font).Width
            W3 = e.Graphics.MeasureString("Value Of Goods :", p2Font).Width

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
            C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + (ClAr(9) / 2)



            vTxPerc = 0
            vCgst_amt = 0
            vSgst_amt = 0
            vIgst_amt = 0


            If Val(prn_HdDt.Rows(0).Item("Company_State_IdNo").ToString) = Val(prn_HdDt.Rows(0).Item("Ledger_State_IdNo").ToString) Then


                If prn_DetDt.Rows.Count > 0 Then
                    vTxPerc = Format(Val(prn_DetDt.Rows(0).Item("item_gst_percentage").ToString) / 2, "############0.00")
                Else
                    'vTxPerc = 5
                    vTxPerc = Format(Val(prn_HdDt.Rows(0).Item("item_gst_percentage").ToString) / 2, "############0.00")
                End If
                vCgst_amt = Format(Val(prn_HdDt.Rows(0).Item("value").ToString) * Val(vTxPerc) / 100, "############0.00")
                vSgst_amt = Format(Val(prn_HdDt.Rows(0).Item("value").ToString) * Val(vTxPerc) / 100, "############0.00")

            Else


                If prn_DetDt.Rows.Count > 0 Then
                    vTxPerc = prn_DetDt.Rows(0).Item("item_gst_percentage").ToString
                Else
                    'vTxPerc = 5
                    vTxPerc = Format(Val(prn_HdDt.Rows(0).Item("item_gst_percentage").ToString), "############0.00")

                End If

                vIgst_amt = Format(Val(prn_HdDt.Rows(0).Item("value").ToString) * Val(vTxPerc) / 100, "############0.00")

            End If

            CurY = CurY + TxtHgt - 15

            Common_Procedures.Print_To_PrintDocument(e, "Received From ", LMargin + 10, CurY, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Receiver_Name").ToString), LMargin + W1 + 30, CurY, 0, 0, p2Font)


            If Val(prn_HdDt.Rows(0).Item("Rate").ToString) <> 0 Then

                Dim vRATEFOR As String = ""

                If Trim(prn_HdDt.Rows(0).Item("Rate_for").ToString) <> "" Then
                    vRATEFOR = "Rate/" & Trim(StrConv(prn_HdDt.Rows(0).Item("Rate_for").ToString, VbStrConv.ProperCase))
                Else
                    vRATEFOR = "Rate/Mtr"
                End If

                Common_Procedures.Print_To_PrintDocument(e, vRATEFOR, LMargin + C1, CurY, 0, 0, p2Font)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W2, CurY, 0, 0, p2Font)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Rate").ToString), "##########0.00"), LMargin + C1 + W2 + 70, CurY, 1, 0, p2Font)

            End If

            If Val(prn_HdDt.Rows(0).Item("Value").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Taxable Amount", LMargin + C2, CurY, 0, 0, p2Font)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W3, CurY, 0, 0, p2Font)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Value").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, p2Font)
            End If



            CurY = CurY + TxtHgt
            If Trim(Area_Nm) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Area_Nm, LMargin + 30, CurY, 0, 0, p2Font)
            ElseIf Trim(LedAdd1) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, LedAdd1, LMargin + 30, CurY, 0, 0, p2Font)
            ElseIf Trim(LedAdd2) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, LedAdd2, LMargin + 30, CurY, 0, 0, p2Font)
            ElseIf Trim(LedAdd3) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, LedAdd3, LMargin + 30, CurY, 0, 0, p2Font)
            End If

            If Val(prn_HdDt.Rows(0).Item("Value").ToString) <> 0 Then

                If Val(vIgst_amt) <> 0 Then

                    Common_Procedures.Print_To_PrintDocument(e, "IGST @ " & Val(vTxPerc) & " %", LMargin + C2, CurY, 0, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W3, CurY, 0, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(vIgst_amt), "##########0.00"), PageWidth - 10, CurY, 1, 0, p2Font)

                Else

                    Common_Procedures.Print_To_PrintDocument(e, "CGST @ " & Val(vTxPerc) & " %", LMargin + C1, CurY, 0, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W2, CurY, 0, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(vCgst_amt), "##########0.00"), LMargin + C1 + W2 + 70, CurY, 1, 0, p2Font)

                    Common_Procedures.Print_To_PrintDocument(e, "SGST @ " & Val(vTxPerc) & " %", LMargin + C2, CurY, 0, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W3, CurY, 0, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(vSgst_amt), "##########0.00"), PageWidth - 10, CurY, 1, 0, p2Font)

                End If

            End If


            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Transport_IdNo").ToString) <> 0 Then
                If Common_Procedures.settings.CustomerCode = "1391" Then
                    Common_Procedures.Print_To_PrintDocument(e, "Sizing Name ", LMargin + 10, CurY, 0, 0, p2Font)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "Transport Name ", LMargin + 10, CurY, 0, 0, p2Font)
                End If
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, p2Font)
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Transport_IdNo").ToString)), LMargin + W1 + 30, CurY, 0, 0, p2Font)
            End If

            If Val(prn_HdDt.Rows(0).Item("Value").ToString) <> 0 Then
                vNtAMt = Format(Val(prn_HdDt.Rows(0).Item("Value").ToString) + Val(vCgst_amt) + Val(vSgst_amt) + Val(vIgst_amt), "############0")

                Common_Procedures.Print_To_PrintDocument(e, "Value Of Goods", LMargin + C2, CurY, 0, 0, p2Font)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W3, CurY, 0, 0, p2Font)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(vNtAMt), "##########0.00"), PageWidth - 10, CurY, 1, 0, p2Font)
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            Common_Procedures.Print_To_PrintDocument(e, "Received The Beams As Per Above Details.", LMargin + 10, CurY + 5, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If Common_Procedures.settings.CustomerCode <> "1391" Then
                p1Font = New Font("Calibri", 8, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 300, CurY, 0, 0, p1Font)
            End If

            ' End If

            CurY = CurY + TxtHgt

            CurY = CurY + TxtHgt + 5

            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
                Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")
            End If

            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 300, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 5, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
            e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub


    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim Led_IdNo As Integer
        Dim NewCode As String
        Dim CompIDCondt As String = ""
        Dim VENIDCondt As String = ""
        Dim Ven_Condt As String = ""
        Dim Delv_IdNo As Integer = 0


        Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_RecForm.Text)
        If Led_IdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT PAVU...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_RecForm.Enabled And cbo_RecForm.Visible Then cbo_RecForm.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1155" Then
            If Common_Procedures.settings.EntrySelection_Combine_AllCompany = 1 Then
                CompIDCondt = ""
                If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
                    CompIDCondt = "(tZ.Company_Type <> 'UNACCOUNT')"
                End If
            End If
        End If


        With dgv_Selection

            chk_SelectAll.Checked = False

            .Rows.Clear()
            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.noof_used as Ent_NoofUsed, b.*, c.EndsCount_Name, d.Beam_Width_Name from Weaver_Pavu_Delivery_Details a INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno INNER JOIN Stock_SizedPavu_Processing_Details b ON a.Set_Code = b.Set_Code and a.Beam_No = b.Beam_No INNER JOIN EndsCount_Head c ON a.EndsCount_IdNo = c.EndsCount_IdNo LEFT OUTER JOIN Beam_Width_Head d ON b.Beam_Width_Idno = d.Beam_Width_Idno   where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Pavu_Delivery_Code = '" & Trim(NewCode) & "' and a.ReceivedFrom_IdNo = " & Str(Val(Led_IdNo)) & " order by a.for_orderby, a.Set_Code, b.ForOrderBy_BeamNo, a.Beam_No, a.sl_no", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Set_No").ToString
                    .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Beam_No").ToString
                    .Rows(n).Cells(3).Value = Val(Dt1.Rows(i).Item("Noof_Pcs").ToString)
                    .Rows(n).Cells(4).Value = Val(Dt1.Rows(i).Item("Meters_Pc").ToString)
                    .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Meters").ToString), "#########0.00")
                    .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("EndsCount_Name").ToString
                    .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Beam_Width_Name").ToString
                    .Rows(n).Cells(8).Value = "1"
                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Ent_NoofUsed").ToString
                    .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Set_Code").ToString
                    .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Pavu_Delivery_Increment").ToString
                    .Rows(n).Cells(12).Value = Common_Procedures.Ledger_IdNoToName(con, Val(Dt1.Rows(i).Item("Vendor_IdNo").ToString))
                    .Rows(n).Cells(13).Value = Dt1.Rows(i).Item("Reference_No").ToString
                    .Rows(n).Cells(14).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Reference_Date")), "dd-MM-yy")
                    .Rows(n).Cells(15).Value = Dt1.Rows(i).Item("Reference_Code").ToString
                    .Rows(n).Cells(16).Value = Dt1.Rows(i).Item("Vendor_LoomNo").ToString

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                        If Val(.Rows(n).Cells(9).Value) <> Val(.Rows(n).Cells(11).Value) Then
                            .Rows(i).Cells(j).Style.BackColor = Color.LightGray
                        End If
                    Next

                Next

            End If
            Dt1.Clear()

            VENIDCondt = ""
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT TEXTILES (SOMANUR)
                Delv_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DelvAt.Text)
                If Delv_IdNo <> 0 Then
                    VENIDCondt = " (a.Vendor_IdNo = " & Str(Val(Delv_IdNo)) & ") and "
                End If
            End If

            Da = New SqlClient.SqlDataAdapter("select sy.Total_pavu_Warp_Weight,a.*, b.EndsCount_Name, c.Beam_Width_Name from Stock_SizedPavu_Processing_Details a INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno INNER JOIN EndsCount_Head b ON a.EndsCount_IdNo = b.EndsCount_IdNo LEFT OUTER JOIN Beam_Width_Head c ON a.Beam_Width_Idno = c.Beam_Width_Idno Left outer join Sizing_Specification_Head sy On sy.set_code=a.set_code  and left(sy.Sizing_Specification_Code,6) + sy.Sizing_Specification_Code = a.Reference_Code Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & VENIDCondt & " a.StockAt_IdNo = " & Str(Val(Led_IdNo)) & " and  (a.Pavu_Delivery_Code = '' and a.Beam_Knotting_Code = '' and a.Close_Status = 0) order by a.for_orderby, a.Set_Code, a.ForOrderBy_BeamNo, a.Beam_No, a.sl_no", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Set_No").ToString
                    .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Beam_No").ToString
                    .Rows(n).Cells(3).Value = Val(Dt1.Rows(i).Item("Noof_Pcs").ToString)
                    .Rows(n).Cells(4).Value = Val(Dt1.Rows(i).Item("Meters_Pc").ToString)
                    .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Meters").ToString), "#########0.00")
                    .Rows(n).Cells(6).Value = Dt1.Rows(i).Item("EndsCount_Name").ToString
                    .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Beam_Width_Name").ToString
                    .Rows(n).Cells(8).Value = ""
                    .Rows(n).Cells(9).Value = "-9999"
                    .Rows(n).Cells(10).Value = Dt1.Rows(i).Item("Set_Code").ToString
                    .Rows(n).Cells(11).Value = Dt1.Rows(i).Item("Pavu_Delivery_Increment").ToString
                    .Rows(n).Cells(12).Value = Common_Procedures.Ledger_IdNoToName(con, Val(Dt1.Rows(i).Item("Vendor_IdNo").ToString))
                    .Rows(n).Cells(13).Value = Dt1.Rows(i).Item("Reference_No").ToString
                    .Rows(n).Cells(14).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Reference_Date")), "dd-MM-yy")
                    .Rows(n).Cells(15).Value = Dt1.Rows(i).Item("Reference_Code").ToString
                    .Rows(n).Cells(16).Value = Dt1.Rows(i).Item("Vendor_LoomNo").ToString

                    lbl_pavu_weight.Text = Format(Val(Dt1.Rows(0).Item("Total_Pavu_Warp_Weight").ToString), "#########0.00")

                Next

            End If
            Dt1.Clear()

        End With

        Da = New SqlClient.SqlDataAdapter("select sy.* from Weaver_Pavu_Delivery_Details a INNER JOIN Company_Head tZ on a.company_idno = tz.company_idno Left Outer Join Stock_SizedPavu_Processing_Details s On s.Set_no=a.Set_no  and s.Beam_No=a.Beam_No Left outer join Sizing_SpecificationPavu_Details sp On s.Set_no=sp.Set_no  and s.Beam_No=sp.Beam_No Left outer join Sizing_Specification_Head sy On sy.Sizing_Specification_Code=sp.Sizing_Specification_Code Where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Pavu_Delivery_Code = '" & Trim(NewCode) & "' and a.ReceivedFrom_IdNo = " & Str(Val(Led_IdNo)) & "  order by a.for_orderby, a.Set_Code, a.sl_no", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            lbl_pavu_weight.Text = Format(Val(Dt1.Rows(0).Item("Total_Pavu_Warp_Weight").ToString), "#########0.00")
        End If
        Dt1.Clear()

        pnl_Selection.Visible = True
        pnl_Back.Enabled = False
        If dgv_Selection.Rows.Count > 0 Then
            dgv_Selection.Focus()
            dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
        End If

    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_Pavu(e.RowIndex)
    End Sub

    Private Sub Select_Pavu(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                If Val(.Rows(RwIndx).Cells(9).Value) > 0 Then
                    If Val(.Rows(RwIndx).Cells(9).Value) <> Val(.Rows(RwIndx).Cells(11).Value) Then
                        MessageBox.Show("Cannot deselect" & Chr(13) & "Already this pavu delivered to others")
                        Exit Sub
                    End If
                End If

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

    Private Sub dgv_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Selection.KeyDown
        On Error Resume Next

        With dgv_Selection

            If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
                If .CurrentCell.RowIndex >= 0 Then
                    Select_Pavu(.CurrentCell.RowIndex)
                    e.Handled = True
                End If
            End If

            If e.KeyCode = Keys.Back Or e.KeyCode = Keys.Delete Then
                If .CurrentCell.RowIndex >= 0 Then
                    If Val(.Rows(.CurrentCell.RowIndex).Cells(8).Value) = 1 Then
                        Select_Pavu(.CurrentCell.RowIndex)
                        e.Handled = True
                    End If
                End If
            End If

        End With

    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim i As Integer
        Dim n As Integer
        Dim sno As Integer
        Dim vFIRST_EdsCnt_NM As String
        Dim vFIRST_EdsCnt_ID As Integer
        Dim vWIDTHTYPE As String
        Dim vREF_CODE As String = ""
        Dim vVEHICLE_NO As String = ""
        Dim vFIRST_SIZING_DCNO As String = ""
        Dim vFIRST_WEA_NAME As String = ""
        Dim vFIRST_DCDATE As String = ""

        With dgv_PavuDetails

            vFIRST_EdsCnt_NM = ""
            vFIRST_SIZING_DCNO = ""
            vFIRST_WEA_NAME = ""
            vFIRST_DCDATE = ""

            .Rows.Clear()

            For i = 0 To dgv_Selection.RowCount - 1

                If Val(dgv_Selection.Rows(i).Cells(8).Value) = 1 Then

                    n = .Rows.Add()

                    sno = sno + 1
                    .Rows(n).Cells(0).Value = Val(sno)
                    .Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(1).Value
                    .Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(2).Value
                    .Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(3).Value
                    .Rows(n).Cells(4).Value = Format(Val(dgv_Selection.Rows(i).Cells(4).Value), "#########0.00")
                    .Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(5).Value
                    .Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(6).Value
                    .Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(7).Value

                    .Rows(n).Cells(8).Value = ""
                    .Rows(n).Cells(9).Value = ""

                    If Val(dgv_Selection.Rows(i).Cells(9).Value) > 0 Then

                        If Val(dgv_Selection.Rows(i).Cells(9).Value) <> Val(dgv_Selection.Rows(i).Cells(11).Value) Then
                            .Rows(n).Cells(8).Value = "1"
                        Else
                            .Rows(n).Cells(8).Value = ""
                        End If

                        .Rows(n).Cells(9).Value = dgv_Selection.Rows(i).Cells(9).Value

                    End If

                    .Rows(n).Cells(10).Value = dgv_Selection.Rows(i).Cells(10).Value
                    .Rows(n).Cells(11).Value = dgv_Selection.Rows(i).Cells(11).Value

                    .Rows(n).Cells(12).Value = dgv_Selection.Rows(i).Cells(15).Value
                    .Rows(n).Cells(13).Value = dgv_Selection.Rows(i).Cells(16).Value



                    If Trim(vFIRST_EdsCnt_NM) = "" Then
                        vFIRST_EdsCnt_NM = .Rows(n).Cells(6).Value
                    End If

                    If Trim(vREF_CODE) = "" Then
                        vREF_CODE = dgv_Selection.Rows(i).Cells(15).Value
                    End If

                    If Trim(vFIRST_SIZING_DCNO) = "" Then
                        vFIRST_SIZING_DCNO = dgv_Selection.Rows(i).Cells(13).Value

                    Else

                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT TEXTILES (SOMANUR)
                            If Trim(UCase(vFIRST_SIZING_DCNO)) <> Trim(UCase(dgv_Selection.Rows(i).Cells(13).Value)) Then
                                MessageBox.Show("Don't select more than one DC.No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                                If dgv_Selection.Enabled And dgv_Selection.Visible Then
                                    dgv_Selection.CurrentCell = dgv_Selection.Rows(i).Cells(0)
                                    dgv_Selection.Focus()
                                End If
                                Exit Sub
                            End If
                        End If

                    End If

                    If Trim(vFIRST_WEA_NAME) = "" Then
                        If Trim(dgv_Selection.Rows(i).Cells(12).Value) <> "" Then
                            vFIRST_WEA_NAME = dgv_Selection.Rows(i).Cells(12).Value
                        End If

                    Else

                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT TEXTILES (SOMANUR)
                            If Trim(UCase(vFIRST_WEA_NAME)) <> Trim(UCase(dgv_Selection.Rows(i).Cells(12).Value)) Then
                                MessageBox.Show("Select only one weavername at a time", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                                If dgv_Selection.Enabled And dgv_Selection.Visible Then
                                    dgv_Selection.CurrentCell = dgv_Selection.Rows(i).Cells(0)
                                    dgv_Selection.Focus()
                                End If
                                Exit Sub
                            End If
                        End If

                    End If

                    If Trim(vFIRST_DCDATE) = "" Then
                        If Trim(dgv_Selection.Rows(i).Cells(14).Value) <> "" Then
                            If IsDate(dgv_Selection.Rows(i).Cells(14).Value) = True Then
                                vFIRST_DCDATE = dgv_Selection.Rows(i).Cells(14).Value
                            End If
                        End If
                    End If

                End If

            Next

            cbo_DelvAt.Enabled = True
            msk_date.Enabled = True

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then '---- BRT TEXTILES (SOMANUR)

                If dgv_Selection.Columns(13).Visible = True Then
                    txt_Party_DcNo.Text = vFIRST_SIZING_DCNO
                End If
                If dgv_Selection.Columns(12).Visible = True Then
                    If Trim(vFIRST_WEA_NAME) <> "" Then
                        If Trim(cbo_DelvAt.Text) = "" Then
                            cbo_DelvAt.Text = Trim(vFIRST_WEA_NAME)
                            cbo_DelvAt.Enabled = False
                        End If
                    End If
                End If



                If dgv_Selection.Columns(14).Visible = True Then
                    If Trim(vFIRST_DCDATE) <> "" Then
                        If IsDate(vFIRST_DCDATE) = True Then
                            msk_date.Text = vFIRST_DCDATE
                            msk_date.Enabled = False
                        End If
                    End If
                End If


                vFIRST_EdsCnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, vFIRST_EdsCnt_NM)
                vWIDTHTYPE = Common_Procedures.get_FieldValue(con, "EndsCount_Head", "WidthType_Single_Double_Triple", "(EndsCount_IdNo = " & Str(Val(vFIRST_EdsCnt_ID)) & ")")
                cbo_WidthType.Text = vWIDTHTYPE

                If Trim(vREF_CODE) <> "" Then
                    da = New SqlClient.SqlDataAdapter("select b.ClothSales_OrderCode_forSelection, b.Vehicle_No, c.cloth_name  from Stock_SizedPavu_Processing_Details a inner join SizSoft_Pavu_Delivery_Head b on a.Reference_Code='SSPDC-'+b.Pavu_Delivery_Code LEFT OUTER JOIN cloth_head c ON b.Textile_ClothIdNo = c.cloth_idno where a.Reference_Code='" & Trim(vREF_CODE) & "'  ", con)
                    dt = New DataTable
                    da.Fill(dt)
                    If dt.Rows.Count > 0 Then

                        cbo_Sales_OrderCode_forSelection.Text = dt.Rows(0).Item("ClothSales_OrderCode_forSelection").ToString
                        cbo_VehicleNo.Text = dt.Rows(0).Item("Vehicle_No").ToString

                        cbo_ClothName.Tag = ""
                        cbo_ClothName.Text = dt.Rows(0).Item("cloth_name").ToString
                        get_CLOTH_CRIMP_Percentage()

                    Else

                        da = New SqlClient.SqlDataAdapter("select b.ClothSales_OrderCode_forSelection, b.Vehicle_No  from Stock_SizedPavu_Processing_Details a INNER JOIN Sizing_Pavu_Receipt_Head b on a.Reference_Code='SZPRC-'+b.Sizing_Pavu_Receipt_Code where a.Reference_Code='" & Trim(vREF_CODE) & "'  ", con)
                        dt = New DataTable
                        da.Fill(dt)
                        If dt.Rows.Count > 0 Then

                            cbo_Sales_OrderCode_forSelection.Text = dt.Rows(0).Item("ClothSales_OrderCode_forSelection").ToString
                            cbo_VehicleNo.Text = dt.Rows(0).Item("Vehicle_No").ToString

                            'cbo_ClothName.Tag = ""
                            'cbo_ClothName.Text = dt.Rows(0).Item("cloth_name").ToString
                            'get_CLOTH_CRIMP_Percentage()

                        End If
                        dt.Clear()
                        dgv_PavuDetails.ReadOnly = False

                    End If
                    dt.Clear()

                End If

            End If

        End With

        TotalPavu_Calculation()

        Grid_Cell_DeSelect()

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        If cbo_EndsCount.Enabled And cbo_EndsCount.Visible Then cbo_EndsCount.Focus()

    End Sub
    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_PavuDetails_KeyUp(sender, e)
        End If
    End Sub

    Private Sub txt_SetNoSelection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_SetNoSelection.KeyDown
        If (e.KeyValue = 40) Then
            txt_BeamNoSelection.Focus()
        End If
    End Sub

    Private Sub txt_SetNoSelection_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_SetNoSelection.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_BeamNoSelection.Focus()
        End If
    End Sub

    Private Sub txt_BeamNoSelection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_BeamNoSelection.KeyDown
        If e.KeyValue = 40 Then
            If dgv_Selection.Rows.Count > 0 Then
                dgv_Selection.Focus()
                dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
                dgv_Selection.CurrentCell.Selected = True
            End If
        End If
        If (e.KeyValue = 38) Then txt_SetNoSelection.Focus()
    End Sub

    Private Sub txt_BeamNoSelection_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_BeamNoSelection.KeyPress
        If Asc(e.KeyChar) = 13 Then

            If Trim(txt_BeamNoSelection.Text) <> "" Or Trim(txt_SetNoSelection.Text) <> "" Then
                btn_Set_Bm_selection_Click(sender, e)

            Else
                If dgv_Selection.Rows.Count > 0 Then
                    dgv_Selection.Focus()
                    dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
                    dgv_Selection.CurrentCell.Selected = True
                End If

            End If

        End If
    End Sub

    Private Sub btn_Set_Bm_selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Set_Bm_selection.Click
        Dim LtNo As String
        Dim PcsNo As String
        Dim i As Integer

        If Trim(txt_SetNoSelection.Text) <> "" Or Trim(txt_BeamNoSelection.Text) <> "" Then

            LtNo = Trim(txt_SetNoSelection.Text)
            PcsNo = Trim(txt_BeamNoSelection.Text)

            For i = 0 To dgv_Selection.Rows.Count - 1
                If Trim(txt_SetNoSelection.Text) <> "" And Trim(txt_BeamNoSelection.Text) <> "" Then
                    If Trim(UCase(LtNo)) = Trim(UCase(dgv_Selection.Rows(i).Cells(1).Value)) And Trim(UCase(PcsNo)) = Trim(UCase(dgv_Selection.Rows(i).Cells(2).Value)) Then
                        Call Select_Pavu(i)

                        dgv_Selection.CurrentCell = dgv_Selection.Rows(i).Cells(0)
                        If i >= 11 Then dgv_Selection.FirstDisplayedScrollingRowIndex = i - 10

                        Exit For

                    End If

                ElseIf Trim(txt_BeamNoSelection.Text) <> "" Then
                    If Trim(UCase(PcsNo)) = Trim(UCase(dgv_Selection.Rows(i).Cells(2).Value)) Then
                        Call Select_Pavu(i)

                        dgv_Selection.CurrentCell = dgv_Selection.Rows(i).Cells(0)
                        If i >= 11 Then dgv_Selection.FirstDisplayedScrollingRowIndex = i - 10

                        Exit For

                    End If

                End If

            Next

            If Trim(txt_SetNoSelection.Text) <> "" And Trim(txt_BeamNoSelection.Text) <> "" Then
                If txt_SetNoSelection.Enabled = True Then txt_SetNoSelection.Focus()
            ElseIf Trim(txt_BeamNoSelection.Text) <> "" Then
                If txt_BeamNoSelection.Enabled = True Then txt_BeamNoSelection.Focus()
            End If

            txt_SetNoSelection.Text = ""
            txt_BeamNoSelection.Text = ""


        End If
    End Sub

    Private Sub chk_SelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk_SelectAll.CheckedChanged
        Dim i As Integer
        Dim J As Integer

        With dgv_Selection

            For i = 0 To .Rows.Count - 1
                .Rows(i).Cells(8).Value = ""
                For J = 0 To .ColumnCount - 1
                    .Rows(i).Cells(J).Style.ForeColor = Color.Black
                Next J
            Next i

            If chk_SelectAll.Checked = True Then
                For i = 0 To .Rows.Count - 1
                    Select_Pavu(i)
                Next i
            End If

            If .Rows.Count > 0 Then
                .Focus()
                .CurrentCell = .Rows(0).Cells(0)
                .CurrentCell.Selected = True
            End If

        End With

    End Sub

    Private Sub msk_date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_date.Text = Date.Today
            msk_date.SelectionStart = 0
        End If
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            If txt_Party_DcNo.Visible And txt_Party_DcNo.Enabled Then
                txt_Party_DcNo.Focus()
            Else
                cbo_TransportMode.Focus()
            End If

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
    Private Sub msk_Date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_date.LostFocus

        If IsDate(msk_date.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_date.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_date.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_date.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_date.Text)) >= 2000 Then
                    dtp_Date.Value = Convert.ToDateTime(msk_date.Text)
                End If
            End If

        End If
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
    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        If IsDate(dtp_Date.Text) = True Then
            msk_date.Text = dtp_Date.Text
            msk_date.SelectionStart = 0
        End If
    End Sub

    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyDown

        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyCode = 40 Then
            e.Handled = True : e.SuppressKeyPress = True
            If txt_Party_DcNo.Visible And txt_Party_DcNo.Enabled Then
                txt_Party_DcNo.Focus()
            Else
                cbo_TransportMode.Focus()
            End If
        End If

        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
            txt_Ewave_Bill_No.Focus()
        End If

        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_date.Text
            vmskSelStrt = msk_date.SelectionStart
        End If
    End Sub

    Private Sub cbo_WidthType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_WidthType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")
    End Sub

    Private Sub cbo_WidthType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_WidthType.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_WidthType, Nothing, Nothing, "", "", "", "")
        If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If cbo_ClothName.Visible And cbo_ClothName.Enabled Then
                cbo_ClothName.Focus()
            Else
                txt_Freight.Focus()
            End If

        ElseIf (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            txt_rate.Focus()
            'If dgv_PavuDetails.Rows.Count > 0 Then
            '    dgv_PavuDetails.Focus()
            '    dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)

            'Else
            '    btn_save.Focus()

            'End If
        End If

    End Sub

    Private Sub cbo_WidthType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_WidthType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_WidthType, Nothing, "", "", "", "")
        If Asc(e.KeyChar) = 13 Then
            txt_rate.Focus()
            'If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
            '    save_record()
            'Else
            '    msk_date.Focus()
            'End If
        End If
    End Sub

    Private Sub btn_SaveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SaveAll.Click
        Dim g As New Password
        g.ShowDialog()

        If Trim(UCase(Common_Procedures.Password_Input)) <> "TSSA7417" Then
            MessageBox.Show("Invalid Password", "PASSWORD FAILED...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If

        SaveAll_STS = True

        LastNo = ""
        movelast_record()

        LastNo = lbl_DcNo.Text

        movefirst_record()
        Timer1.Enabled = True

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        save_record()
        If Trim(UCase(LastNo)) = Trim(UCase(lbl_DcNo.Text)) Then
            Timer1.Enabled = False
            SaveAll_STS = False
            MessageBox.Show("All entries saved sucessfully", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Else
            movenext_record()

        End If
    End Sub

    Private Sub btn_Close_EmptyBeamDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_EmptyBeamDetails.Click
        pnl_Back.Enabled = True
        pnl_EmptyBeamDetails.Visible = False
    End Sub

    Private Sub btn_EmptyBeamOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_EmptyBeamOpen.Click
        pnl_Back.Enabled = False
        pnl_EmptyBeamDetails.Visible = True
        If dgv_EmptyBeamDetails.Rows.Count > 0 Then
            dgv_EmptyBeamDetails.Focus()
            dgv_EmptyBeamDetails.CurrentCell = dgv_EmptyBeamDetails.Rows(0).Cells(1)
            dgv_EmptyBeamDetails.CurrentCell.Selected = True
        End If
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView

        If ActiveControl.Name = dgv_EmptyBeamDetails.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_EmptyBeamDetails.Name Then
                dgv1 = dgv_EmptyBeamDetails
            ElseIf ActiveControl.Name = dgv_PavuDetails.Name Then
                dgv1 = dgv_PavuDetails
            ElseIf dgv_EmptyBeamDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_EmptyBeamDetails
            ElseIf dgv_PavuDetails.IsCurrentRowDirty = True Then
                dgv1 = dgv_PavuDetails
            ElseIf dgv_ActCtrlName = dgv_EmptyBeamDetails.Name Then
                dgv1 = dgv_EmptyBeamDetails
            ElseIf dgv_ActCtrlName = dgv_PavuDetails.Name Then
                dgv1 = dgv_PavuDetails
            End If


            If IsNothing(dgv1) = False AndAlso IsNothing(dgv1.CurrentCell) = False Then

                With dgv1

                    If dgv1.Name = dgv_EmptyBeamDetails.Name Then
                        If keyData = Keys.Enter Or keyData = Keys.Down Then
                            If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                                If .CurrentCell.RowIndex = .RowCount - 1 Then
                                    cbo_VehicleNo.Focus()

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
                                    cbo_VehicleNo.Focus()

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



                    End If


                End With

            Else

                Return MyBase.ProcessCmdKey(msg, keyData)

            End If

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function
    Private Sub dgv_EmptyBeamDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_EmptyBeamDetails.EditingControlShowing
        dgtxt_EmptyBeamDetails = CType(dgv_EmptyBeamDetails.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub
    Private Sub dgtxt_EmptyBeamDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_EmptyBeamDetails.Enter
        dgv_ActCtrlName = dgv_EmptyBeamDetails.Name
        dgv_EmptyBeamDetails.EditingControl.BackColor = Color.Lime
        dgv_EmptyBeamDetails.EditingControl.ForeColor = Color.Blue
        dgtxt_EmptyBeamDetails.SelectAll()
    End Sub
    Private Sub dgtxt_EmptyBeamDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_EmptyBeamDetails.KeyPress

        With dgv_EmptyBeamDetails

            If Val(dgv_EmptyBeamDetails.CurrentCell.ColumnIndex.ToString) = 1 Or Val(dgv_EmptyBeamDetails.CurrentCell.ColumnIndex.ToString) = 3 Then

                If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                    e.Handled = True
                End If

            End If

        End With

    End Sub

    Private Sub dgv_EmptyBeamDetails_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs)
        Dim n As Integer

        With dgv_EmptyBeamDetails
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With

    End Sub

    Private Sub dgv_EmptyBeamDetails_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        On Error Resume Next
        dgv_EmptyBeamDetails.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_EmptyBeamDetails_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)

        dgv_EmptyBeamDetails_CellLeave(sender, e)

    End Sub

    Private Sub dgv_EmptyBeamDetails_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_EmptyBeamDetails.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Dt4 As New DataTable
        Dim Rect As Rectangle

        With dgv_EmptyBeamDetails

            dgv_ActCtrlName = .Name.ToString

            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If
            If e.ColumnIndex = 2 Then

                If cbo_Vendor.Visible = False Or Val(cbo_Vendor.Tag) <> e.RowIndex Then



                    cbo_Vendor.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Vendor_Name from Vendor_Head Order by Vendor_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Vendor.DataSource = Dt1
                    cbo_Vendor.DisplayMember = "Vendor_Name"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Vendor.Left = .Left + Rect.Left
                    cbo_Vendor.Top = .Top + Rect.Top
                    cbo_Vendor.Width = Rect.Width
                    cbo_Vendor.Height = Rect.Height

                    cbo_Vendor.Text = .CurrentCell.Value

                    cbo_Vendor.Tag = Val(e.RowIndex)
                    cbo_Vendor.Visible = True

                    cbo_Vendor.BringToFront()
                    cbo_Vendor.Focus()



                End If

            Else

                cbo_Vendor.Visible = False

            End If



            If e.ColumnIndex = 3 Then

                If cbo_beamwidth.Visible = False Or Val(cbo_beamwidth.Tag) <> e.RowIndex Then

                    'dgv_ActCtrlName = dgv_Details.Name

                    cbo_beamwidth.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Beam_Width_Name from Beam_Width_Head Order by Beam_Width_Name", con)
                    Dt2 = New DataTable
                    Da.Fill(Dt2)
                    cbo_beamwidth.DataSource = Dt2
                    cbo_beamwidth.DisplayMember = "Beam_Width_Name"

                    Rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_beamwidth.Left = .Left + Rect.Left  '  .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Left
                    cbo_beamwidth.Top = .Top + Rect.Top  ' .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Top
                    cbo_beamwidth.Width = Rect.Width  ' .CurrentCell.Size.Width
                    cbo_beamwidth.Height = Rect.Height  ' rect.Height

                    cbo_beamwidth.Text = .CurrentCell.Value  '  Trim(.CurrentRow.Cells(e.ColumnIndex).Value)

                    cbo_beamwidth.Tag = Val(e.RowIndex)
                    cbo_beamwidth.Visible = True

                    cbo_beamwidth.BringToFront()
                    cbo_beamwidth.Focus()



                End If

            Else

                cbo_beamwidth.Visible = False

            End If



        End With

    End Sub

    Private Sub dgv_EmptyBeamDetails_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_EmptyBeamDetails.CellLeave
        With dgv_EmptyBeamDetails
            If .CurrentCell.ColumnIndex = 1 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0")
                End If
            End If
        End With
        TotalBeam_Calculation()
    End Sub

    Private Sub dgv_EmptyBeamDetails_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_EmptyBeamDetails.CellValueChanged

        On Error Resume Next


        If IsNothing(dgv_EmptyBeamDetails.CurrentCell) Then Exit Sub
        With dgv_EmptyBeamDetails
            If .Visible Then

                If .CurrentCell.ColumnIndex = 1 Then

                    TotalBeam_Calculation()

                End If

            End If
        End With

    End Sub
    Private Sub TotalBeam_Calculation()
        Dim vTotetybm As Single
        Dim i As Integer
        Dim sno As Integer

        vTotetybm = 0
        With dgv_EmptyBeamDetails
            For i = 0 To .Rows.Count - 1

                sno = sno + 1

                .Rows(i).Cells(0).Value = sno

                If Val(.Rows(i).Cells(1).Value) <> 0 Then

                    vTotetybm = vTotetybm + Val(.Rows(i).Cells(1).Value)


                End If
            Next
        End With

        If dgv_EmptyBeamDetails_Total.Rows.Count <= 0 Then dgv_EmptyBeamDetails_Total.Rows.Add()

        dgv_EmptyBeamDetails_Total.Rows(0).Cells(1).Value = Val(vTotetybm)
        ' dgv_etails_Total.Rows(0).Cells(4).Value = Format(Val(vTotMtrs), "#########0.000")
    End Sub

    Private Sub Printing_GST_HSN_Details_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByRef CurY As Single, ByVal LMargin As Integer, ByVal PageWidth As Integer, ByVal PrintWidth As Double, ByVal LnAr As Single)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim I As Integer, Cgst_Perc As Single = 0
        Dim p1Font As Font
        Dim p2Font As Font
        Dim SubClAr(15) As Single
        Dim ItmNm1 As String, ItmNm2 As String
        Dim SNo As Integer = 0
        Dim Sgst_Perc As Single = 0
        Dim Igst_Perc As Single = 0
        Dim Ttl_TaxAmt As Double, Ttl_CGst As Double, Ttl_Sgst As Double, Ttl_igst As Double
        Dim LnAr2 As Single
        Dim BmsInWrds As String
        Dim TaxAmt As Double, CGstAmt As Double, SgstAmt As Double, IgstAmt As Double
        Dim NoofItems_Increment As Integer
        Dim NoofDets As Integer
        Dim LedIdNo As Integer = 0
        Dim Hsn_Code As String = ""
        Dim Ass_value As Double = 0, gst_per As Double = 0
        Dim InterStateStatus As Boolean = False
        ' Dim cmd As New SqlClient.SqlCommand
        Try
            cmd.Connection = con

            cmd.CommandText = "Truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
            cmd.ExecuteNonQuery()

            TxtHgt = TxtHgt - 1

            p2Font = New Font("Calibri", 9, FontStyle.Regular)
            InterStateStatus = False
            Ttl_TaxAmt = 0 : Ttl_CGst = 0 : Ttl_Sgst = 0 : Ttl_igst = 0 : Cgst_Perc = 0 : Sgst_Perc = 0 : Igst_Perc = 0
            TaxAmt = 0 : CGstAmt = 0 : SgstAmt = 0 : IgstAmt = 0
            Erase SubClAr
            LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DelvAt.Text)
            InterStateStatus = Common_Procedures.Is_InterState_Party(con, Val(lbl_Company.Tag), LedIdNo)
            SubClAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

            SubClAr(1) = 110 : SubClAr(2) = 150 : SubClAr(3) = 65 : SubClAr(4) = 80 : SubClAr(5) = 65 : SubClAr(6) = 80 : SubClAr(7) = 55
            SubClAr(8) = PageWidth - (LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7))

            Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC", LMargin, CurY + 5, 2, SubClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "TAXABLE AMOUNT", LMargin + SubClAr(1), CurY + 5, 2, SubClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "CGST", LMargin + SubClAr(1) + SubClAr(2), CurY, 2, SubClAr(3) + SubClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SGST ", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, 2, SubClAr(5) + SubClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "IGST", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, 2, SubClAr(7) + SubClAr(8), pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2), CurY, PageWidth, CurY)
            LnAr2 = CurY
            CurY = CurY + 5

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2), CurY, 2, SubClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), CurY, 2, SubClAr(4), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, 2, SubClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), CurY, 2, SubClAr(6), pFont)

            Common_Procedures.Print_To_PrintDocument(e, "%", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, 2, SubClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Amount", LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), CurY, 2, SubClAr(8), pFont)

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            cmd.CommandText = "Truncate table " & Trim(Common_Procedures.EntryTempSubTable) & ""
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & "(Int1, Int2, Meters1) Select a.EndsCount_IdNo, count(a.Beam_No), sum(a.Meters) from Weaver_Pavu_Delivery_Details a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Pavu_Delivery_Code = '" & Trim(EntryCode) & "' group by a.EndsCount_IdNo"
            cmd.ExecuteNonQuery()
            If Val(prn_HdDt.Rows(0).Item("Pavu_Meters").ToString) <> 0 Then
                cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempSubTable) & "(Int1, Int2, Meters1) values (" & Str(Val(prn_HdDt.Rows(0).Item("EndsCount_IdNo").ToString)) & ", " & Str(Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)) & ", " & Str(Val(prn_HdDt.Rows(0).Item("Pavu_Meters").ToString)) & ")"
                cmd.ExecuteNonQuery()
            End If

            Da1 = New SqlClient.SqlDataAdapter("Select b.Endscount_Name, IG.Item_HSN_Code, IG.Item_GST_Percentage, sum(a.Meters1) as PavuMtrs, sum(a.Int2) as Beams from " & Trim(Common_Procedures.EntryTempSubTable) & " a INNER JOIN EndsCount_Head b ON a.Int1 = b.EndsCount_IdNo INNER JOIN Count_Head c ON b.Count_IdNo = c.Count_IdNo LEFT OUTER JOIN ItemGroup_Head IG ON c.ItemGroup_IdNo = IG.ItemGroup_IdNo  group by b.EndsCount_Name, IG.Item_HSN_Code, IG.Item_GST_Percentage Order by b.EndsCount_Name, IG.Item_HSN_Code, IG.Item_GST_Percentage", con)
            Dt1 = New DataTable
            Da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                For I = 0 To Dt1.Rows.Count - 1
                    Hsn_Code = Trim(Dt1.Rows(I).Item("Item_HSN_Code").ToString)
                    gst_per = (Dt1.Rows(I).Item("Item_GST_Percentage").ToString)
                    Ass_value = Val(Dt1.Rows(I).Item("Beams").ToString) * Val(prn_HdDt.Rows(0).Item("Rate").ToString)

                    cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & " (                    Name1        ,         Currency1            ,   Currency2          ) " &
                                      "            Values    (       '" & Trim(Hsn_Code) & "'   , " & (Val(gst_per)) & ", " & Str(Val(Ass_value)) & " ) "
                    cmd.ExecuteNonQuery()

                Next

            End If

            'Da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, d.* ,e.*  from Weaver_Pavu_Delivery_Details a INNER JOIN EndsCount_Head b ON a.EndsCount_idno = b.EndsCount_idno LEFT OUTER JOIN Count_Head d ON b.Count_idno = d.Count_idno LEFT OUTER JOIN itemgroup_head e ON d.itemgroup_idno = e.itemgroup_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Pavu_Delivery_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
            'Dt1 = New DataTable
            'Da1.Fill(Dt1)
            'If Dt1.Rows.Count > 0 Then
            '    For I = 0 To Dt1.Rows.Count - 1
            '        Hsn_Code = Trim(Dt1.Rows(I).Item("Item_HSN_Code").ToString)
            '        gst_per = (Dt1.Rows(I).Item("Item_GST_Percentage").ToString)
            '        Ass_value = (Dt1.Rows(I).Item("Meters").ToString) * (prn_HdDt.Rows(0).Item("Rate").ToString)

            '        cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & " (                    Name1        ,         Currency1            ,   Currency2          ) " & _
            '                          "            Values    (       '" & Trim(Hsn_Code) & "'   , " & (Val(gst_per)) & ", " & Str(Val(Ass_value)) & " ) "
            '        cmd.ExecuteNonQuery()
            '    Next
            'End If

            Da = New SqlClient.SqlDataAdapter("select Name1 as HSN_Code, Currency1 as GST_Percentage, sum(Currency2) as Assessable_Value from " & Trim(Common_Procedures.EntryTempTable) & " group by name1, Currency1 Having sum(Currency2) <> 0 ", con)
            Dt = New DataTable
            Da.Fill(Dt)

            If Dt.Rows.Count > 0 Then

                prn_DetIndx = 0
                NoofDets = 0
                NoofItems_Increment = 0

                CurY = CurY - 20

                Do While prn_DetIndx <= Dt.Rows.Count - 1

                    ItmNm1 = Trim(Dt.Rows(prn_DetIndx).Item("HSN_Code").ToString)

                    ItmNm2 = ""
                    If Len(ItmNm1) > 40 Then
                        For I = 35 To 1 Step -1
                            If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                        Next I
                        If I = 0 Then I = 40
                        ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                        ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                    End If
                    If InterStateStatus = True Then
                        Igst_Perc = (Dt.Rows(prn_DetIndx).Item("GST_Percentage").ToString)
                    Else
                        Cgst_Perc = Format(Val(Dt.Rows(prn_DetIndx).Item("GST_Percentage").ToString) / 2, "############0.00")
                        Sgst_Perc = Format(Val(Dt.Rows(prn_DetIndx).Item("GST_Percentage").ToString) / 2, "############0.00")

                    End If

                    TaxAmt = (Dt.Rows(prn_DetIndx).Item("Assessable_Value").ToString)
                    CGstAmt = Format(Val(TaxAmt) * (Cgst_Perc) / 100, "###########0.00")
                    SgstAmt = Format(Val(TaxAmt) * (Sgst_Perc) / 100, "###########0.00")
                    IgstAmt = Format(Val(TaxAmt) * (Igst_Perc) / 100, "###########0.00")
                    CurY = CurY + TxtHgt + 3

                    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin + 10, CurY, 0, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(TaxAmt) <> 0, Common_Procedures.Currency_Format(Val(TaxAmt)), ""), LMargin + SubClAr(1) + SubClAr(2) - 10, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Cgst_Perc) <> 0, Common_Procedures.Currency_Format(Val(Cgst_Perc)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) - 10, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(CGstAmt) <> 0, Common_Procedures.Currency_Format(Val(CGstAmt)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) - 5, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Sgst_Perc) <> 0, Common_Procedures.Currency_Format(Val(Sgst_Perc)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) - 10, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(SgstAmt) <> 0, Common_Procedures.Currency_Format(Val(SgstAmt)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) - 5, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Igst_Perc) <> 0, Common_Procedures.Currency_Format(Val(Igst_Perc)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) - 10, CurY, 1, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, IIf(Val(IgstAmt) <> 0, Common_Procedures.Currency_Format(Val(IgstAmt)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) - 5, CurY, 1, 0, p2Font)

                    NoofItems_Increment = NoofItems_Increment + 1

                    NoofDets = NoofDets + 1

                    Ttl_TaxAmt = Ttl_TaxAmt + Val(TaxAmt)
                    Ttl_CGst = Ttl_CGst + Val(CGstAmt)
                    Ttl_Sgst = Ttl_Sgst + Val(SgstAmt)
                    Ttl_igst = Ttl_igst + Val(IgstAmt)
                    prn_DetIndx = prn_DetIndx + 1
                Loop

            End If

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            CurY = CurY + TxtHgt - 15
            Common_Procedures.Print_To_PrintDocument(e, "Total", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_TaxAmt) <> 0, Common_Procedures.Currency_Format(Val(Ttl_TaxAmt)), ""), LMargin + SubClAr(1) + SubClAr(2) - 10, CurY, 1, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_CGst) <> 0, Common_Procedures.Currency_Format(Val(Ttl_CGst)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) - 5, CurY, 1, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_Sgst) <> 0, Common_Procedures.Currency_Format(Val(Ttl_Sgst)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) - 5, CurY, 1, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, IIf(Val(Ttl_igst) <> 0, Common_Procedures.Currency_Format(Val(Ttl_igst)), ""), LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7) + SubClAr(8) - 5, CurY, 1, 0, p2Font)
            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1), CurY, LMargin + SubClAr(1), LnAr)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2), CurY, LMargin + SubClAr(1) + SubClAr(2), LnAr)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3), LnAr2)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4), LnAr)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5), LnAr2)

            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6), LnAr)
            e.Graphics.DrawLine(Pens.Black, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), CurY, LMargin + SubClAr(1) + SubClAr(2) + SubClAr(3) + SubClAr(4) + SubClAr(5) + SubClAr(6) + SubClAr(7), LnAr2)


            CurY = CurY + 5
            p1Font = New Font("Calibri", 12, FontStyle.Regular)
            BmsInWrds = ""
            BmsInWrds = Common_Procedures.Rupees_Converstion(Val(Ttl_CGst) + Val(Ttl_Sgst) + Val(Ttl_igst))
            BmsInWrds = Replace(Trim(BmsInWrds), "", "")


            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Tax Amount (In Words) : " & BmsInWrds & " ", LMargin + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try



    End Sub
    Private Function get_GST_Tax_Percentage_For_Printing(ByVal EntryCode As String) As Single
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim TaxPerc As Single = 0
        Dim LedIdNo As Integer = 0

        Dim InterStateStatus As Boolean = False
        TaxPerc = 0


        LedIdNo = 0
        InterStateStatus = False
        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DelvAt.Text)
        Da = New SqlClient.SqlDataAdapter("select a.*, b.*, d.* ,e.*  from Weaver_Pavu_Delivery_Details a LEFT OUTER JOIN EndsCount_Head b ON a.EndsCount_idno = b.EndsCount_idno LEFT OUTER JOIN Count_Head d ON b.Count_idno = d.Count_idno LEFT OUTER JOIN itemgroup_head e ON d.itemgroup_idno = e.itemgroup_idno Where Weaver_pavu_Delivery_Code = '" & Trim(EntryCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If Dt1.Rows.Count = 1 Then

                Da = New SqlClient.SqlDataAdapter("select a.*, b.*, d.* ,e.*  from Weaver_Pavu_Delivery_Details a LEFT OUTER JOIN EndsCount_Head b ON a.EndsCount_idno = b.EndsCount_idno LEFT OUTER JOIN Count_Head d ON b.Count_idno = d.Count_idno LEFT OUTER JOIN itemgroup_head e ON d.itemgroup_idno = e.itemgroup_idno Where Weaver_Pavu_Delivery_Code = '" & Trim(EntryCode) & "'", con)
                Dt2 = New DataTable
                Da.Fill(Dt2)
                If Dt2.Rows.Count > 0 Then
                    If InterStateStatus = True Then
                        TaxPerc = Val(Dt2.Rows(0).Item("Item_GST_Percentage").ToString)
                    Else
                        TaxPerc = Val(Dt2.Rows(0).Item("Item_GST_Percentage").ToString) / 2
                    End If
                End If
                Dt2.Clear()

            End If
        End If

        Dt2.Dispose()
        Da.Dispose()

        get_GST_Tax_Percentage_For_Printing = Format(Val(TaxPerc), "#########0.00")

    End Function

    Private Sub txt_rate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_rate.KeyDown
        If e.KeyCode = 40 Then
            If txt_Value.Visible = True Then
                txt_Value.Focus()
            Else
                txt_Ewave_Bill_No.Focus()
            End If
        End If

        If e.KeyCode = 38 Then
            'If dgv_PavuDetails.Rows.Count > 0 Then
            '    dgv_PavuDetails.Focus()
            '    dgv_PavuDetails.CurrentCell = dgv_PavuDetails.Rows(0).Cells(1)
            'Else
            If cbo_WidthType.Visible And cbo_WidthType.Enabled Then
                cbo_WidthType.Focus()
            ElseIf cbo_ClothName.Visible And cbo_ClothName.Enabled Then
                cbo_ClothName.Focus()
            ElseIf txt_CrimpPerc.Visible And txt_CrimpPerc.Enabled Then
                txt_CrimpPerc.Focus()
            Else
                txt_Freight.Focus()
            End If
            'End If
        End If

    End Sub

    Private Sub txt_rate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_rate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            If txt_Value.Visible = True Then
                txt_Value.Focus()
            Else
                txt_Ewave_Bill_No.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_DelvTo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_DelvAt.LostFocus
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        da = New SqlClient.SqlDataAdapter("select a.* from Ledger_head a  where a.Ledger_name = '" & Trim(cbo_DelvAt.Text) & "'", con)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then

            If IsDBNull(dt.Rows(0)("Freight_Pavu").ToString) = False Then
                lbl_Freight_Pavu.Text = dt.Rows(0)("Freight_Pavu").ToString
            End If

        End If
        dt.Dispose()
        da.Dispose()

    End Sub

    Private Sub txt_rate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_rate.TextChanged
        Amount_Calculation()
    End Sub

    Private Sub cbo_TransportMode_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_TransportMode.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Weaver_Pavu_Delivery_Head", "Transportation_Mode", "", "")
    End Sub

    Private Sub cbo_TransportMode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_TransportMode.KeyDown
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, Nothing, txt_DateTime_Of_Supply, "Weaver_Pavu_Delivery_Head", "Transportation_Mode", "", "")
            If (e.KeyValue = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                If txt_Party_DcNo.Visible And txt_Party_DcNo.Enabled Then
                    txt_Party_DcNo.Focus()
                Else
                    cbo_TransportMode.Focus()
                End If
            End If


        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbo_TransportMode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_TransportMode.KeyPress
        Try
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_TransportMode, txt_DateTime_Of_Supply, "Weaver_Pavu_Delivery_Head", "Transportation_Mode", "", "", False)
        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SELECT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Printing_Format2Gst(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim Cmp_Name As String
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim ps As Printing.PaperSize

        For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
            If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                PrintDocument1.DefaultPageSettings.PaperSize = ps
                Exit For
            End If
        Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30 ' 30 '60
            .Right = 60
            .Top = 40 ' 60
            .Bottom = 40
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 10, FontStyle.Regular)
        'pFont = New Font("Calibri", 12, FontStyle.Regular)

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

        TxtHgt = 19 ' e.Graphics.MeasureString("A", pFont).Height  ' 20


        NoofItems_PerPage = 12


        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(0) = 0
        ClArr(1) = 35
        ClArr(2) = 235 : ClArr(3) = 80 : ClArr(4) = 55 : ClArr(5) = 70 : ClArr(6) = 85 : ClArr(7) = 75
        ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        ' ClArr(2) = 230 : ClArr(3) = 85 : ClArr(4) = 50 : ClArr(5) = 80 : ClArr(6) = 55 : ClArr(7) = 80
        ' ClArr(8) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7))

        CurY = TMargin

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format2Gst_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                NoofDets = 0
                'DetSNo = 0
                CurY = CurY - 10

                If prn_DetMxIndx > 0 Then

                    Do While prn_DetIndx <= prn_DetMxIndx

                        If NoofDets >= NoofItems_PerPage Then

                            CurY = CurY + TxtHgt

                            Common_Procedures.Print_To_PrintDocument(e, "Continued...", PageWidth - 10, CurY, 1, 0, pFont)

                            NoofDets = NoofDets + 1

                            Printing_Format2Gst_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, False)

                            e.HasMorePages = True
                            Return

                        End If

                        'ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Count_Name").ToString) & "-" & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Mill_Name").ToString)
                        'ItmNm2 = ""
                        'If Len(ItmNm1) > 35 Then
                        '    For I = 35 To 1 Step -1
                        '        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                        '    Next I
                        '    If I = 0 Then I = 35
                        '    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                        '    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                        'End If



                        prn_DetIndx = prn_DetIndx + 1
                        CurY = CurY + TxtHgt

                        If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then

                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 8)), LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 5)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 6))), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 7)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 3)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)

                            Common_Procedures.Print_To_PrintDocument(e, (prn_HdDt.Rows(0).Item("Rate").ToString), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 3)) * Val(prn_HdDt.Rows(0).Item("Rate").ToString), "#########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                            ' prn_NoofBmDets = prn_NoofBmDets + 1

                        End If
                        NoofDets = NoofDets + 1

                        'If Trim(ItmNm2) <> "" Then
                        '    CurY = CurY + TxtHgt - 5
                        '    Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm2), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                        '    NoofDets = NoofDets + 1
                        'End If


                    Loop

                End If

                Printing_Format2Gst_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, Cmp_Name, NoofDets, True)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        e.HasMorePages = False

    End Sub

    Private Sub Printing_Format2Gst_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim Cmp_StateCap As String, Cmp_StateNm As String, Cmp_StateCode As String, Cmp_GSTIN_Cap As String, Cmp_GSTIN_No As String
        Dim strHeight As Single
        Dim Led_Name As String, Led_Add1 As String, Led_Add2 As String, Led_Add3 As String, Led_Add4 As String, Led_TinNo As String
        Dim Led_GSTTinNo As String, Led_State As String, Led_StateCd As String, LedAadhar_No As String
        Dim LedNmAr(10) As String
        Dim Cmp_Desc As String, Cmp_Email As String
        Dim Cen1 As Single = 0
        Dim W1 As Single = 0
        Dim W2 As Single = 0
        Dim LInc As Integer = 0
        Dim prn_OriDupTri As String = ""
        Dim S As String = ""
        Dim Led_PhNo As String
        Dim CurX As Single = 0
        Dim strWidth As Single = 0
        Dim BlockInvNoY As Single = 0
        Dim Trans_Nm As String = ""
        Dim Indx As Integer = 0
        Dim HdWd As Single = 0
        Dim H1 As Single = 0
        Dim W3 As Single = 0
        Dim C1 As Single = 0, C2 As Single = 0, C3 As Single = 0
        Dim i As Integer = 0
        Dim ItmNm1 As String, ItmNm2 As String

        PageNo = PageNo + 1

        CurY = TMargin


        prn_Count = prn_Count + 1


        da2 = New SqlClient.SqlDataAdapter("select a.*, d.EndsCount_name  from Weaver_Pavu_Delivery_Details a  LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_idno = d.EndsCount_idno where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Pavu_Delivery_Code = '" & Trim(EntryCode) & "' Order by a.Sl_No", con)
        da2.Fill(dt2)

        If dt2.Rows.Count > NoofItems_PerPage Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If
        dt2.Clear()

        'p1Font = New Font("Calibri", 12, FontStyle.Regular)
        'Common_Procedures.Print_To_PrintDocument(e, "INVOICE", LMargin, CurY - TxtHgt, 2, PrintWidth, p1Font)
        ' CurY = CurY + TxtHgt '+ 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""
        Cmp_Desc = "" : Cmp_Email = ""
        Cmp_StateCap = "" : Cmp_StateNm = "" : Cmp_StateCode = "" : Cmp_GSTIN_Cap = "" : Cmp_GSTIN_No = ""

        Cmp_Name = Trim(prn_HdDt.Rows(0).Item("Company_Name").ToString)

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

        '***** GST START *****
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
        '***** GST END *****

        CurY = CurY + TxtHgt - 10

        p1Font = New Font("Calibri", 18, FontStyle.Bold)

        If Trim(Common_Procedures.settings.CustomerCode) = "1154" Then
            e.Graphics.DrawString(Cmp_Name, p1Font, Brushes.Green, 266, CurY)
            'Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        End If
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

        If Trim(ItmNm1) <> "" Then
            CurY = CurY + strHeight - 1
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(ItmNm1), LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        End If

        If Trim(ItmNm2) <> "" Then
            CurY = CurY + strHeight - 1
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
        strWidth = e.Graphics.MeasureString(Trim(Cmp_StateCap & Cmp_StateNm & "     " & Cmp_GSTIN_Cap & Cmp_GSTIN_No), pFont).Width
        If PrintWidth > strWidth Then
            CurX = LMargin + (PrintWidth - strWidth) / 2
        Else
            CurX = LMargin
        End If
        pFont = New Font("Calibri", 11, FontStyle.Regular)
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
        Common_Procedures.Print_To_PrintDocument(e, Trim(Cmp_PhNo & "   " & Cmp_Email), LMargin, CurY, 2, PrintWidth, pFont)
        pFont = New Font("Calibri", 10, FontStyle.Regular)
        '***** GST END *****
        CurY = CurY + TxtHgt
        p1Font = New Font("Calibri", 16, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "FORM GST DELIVERY CHALLAN", LMargin, CurY, 2, PrintWidth, p1Font)


        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            Led_Name = "" : Led_Add1 = "" : Led_Add2 = "" : Led_Add3 = "" : Led_Add4 = "" : Led_TinNo = "" : Led_PhNo = "" : Led_GSTTinNo = "" : Led_State = ""

            Led_StateCd = ""
            LedAadhar_No = ""
            Led_Name = "M/s. " & Trim(prn_HdDt.Rows(0).Item("Ledger_MainName").ToString)

            Led_Add1 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address1").ToString)
            Led_Add2 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address2").ToString)
            Led_Add3 = Trim(prn_HdDt.Rows(0).Item("Ledger_Address3").ToString) & " " & Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)
            'Led_Add4 = ""  'Trim(prn_HdDt.Rows(0).Item("Ledger_Address4").ToString)
            '***** GST START *****
            Led_TinNo = "Tin No : " & Trim(prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString)
            If Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString) <> "" Then Led_PhNo = "Phone No : " & Trim(prn_HdDt.Rows(0).Item("Ledger_PhoneNo").ToString)

            Led_State = "State : " & Trim(prn_HdDt.Rows(0).Item("Ledger_State_Name").ToString) & "  Code  :" & Trim(prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString)
            Led_StateCd = Trim(prn_HdDt.Rows(0).Item("Ledger_State_Code").ToString)
            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then Led_GSTTinNo = " GSTIN : " & Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString)
            If Trim(Led_GSTTinNo) = "" Then
                If Trim(prn_HdDt.Rows(0).Item("Pan_No").ToString) <> "" Then Led_GSTTinNo = " Pan No : " & Trim(prn_HdDt.Rows(0).Item("Pan_No").ToString)
            End If
            If Trim(Led_GSTTinNo) = "" Then
                If Trim(prn_HdDt.Rows(0).Item("Aadhar_No").ToString) <> "" Then LedAadhar_No = " Aadhar No : " & Trim(prn_HdDt.Rows(0).Item("Aadhar_No").ToString)
            End If

            '***** GST END *****



            Erase LedNmAr
            LedNmAr = New String(10) {}
            LInc = 0

            LInc = LInc + 1
            LedNmAr(LInc) = Led_Name

            If Trim(Led_Add1) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_Add1
            End If

            If Trim(Led_Add2) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_Add2
            End If

            If Trim(Led_Add3) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_Add3
            End If

            'If Trim(Led_Add4) <> "" Then
            '    LInc = LInc + 1
            '    LedNmAr(LInc) = Led_Add4
            'End If
            '***** GST START *****
            If Trim(Led_State) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_State
            End If

            If Trim(Led_PhNo) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_PhNo
            End If

            If Trim(Led_GSTTinNo) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = Led_GSTTinNo
            End If

            If Trim(LedAadhar_No) <> "" Then
                LInc = LInc + 1
                LedNmAr(LInc) = LedAadhar_No
            End If
            '***** GST END *****

            Cen1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)
            W1 = e.Graphics.MeasureString("Date & Time of Supply : ", pFont).Width
            W2 = e.Graphics.MeasureString("TO :", pFont).Width

            CurY = CurY + TxtHgt
            BlockInvNoY = CurY

            '***** GST START *****
            Common_Procedures.Print_To_PrintDocument(e, "TO : ", LMargin + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(1)), LMargin + W2 + 10, CurY, 0, 0, p1Font)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(2)), LMargin + W2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(3)), LMargin + W2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(4)), LMargin + W2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(5)), LMargin + W2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(6)), LMargin + W2 + 10, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(7)), LMargin + W2 + 10, CurY, 0, 0, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Trim(LedNmAr(8)), LMargin + W2 + 10, CurY, 0, 0, pFont)
            '***** GST END *****


            '------------------- Invoice No Block

            '***** GST START *****
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Delivery Note No.", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Weaver_Pavu_Delivery_No").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, p1Font)

            BlockInvNoY = BlockInvNoY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Delivery Note Date", LMargin + Cen1 + 10, BlockInvNoY + 4, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY + 4, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Weaver_Pavu_Delivery_Date").ToString), "dd-MM-yyyy"), LMargin + Cen1 + W1 + 30, BlockInvNoY + 4, 0, 0, pFont)


            BlockInvNoY = BlockInvNoY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Transportation_Mode").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Mode of Transport", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Transportation_Mode").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, pFont)
            End If


            BlockInvNoY = BlockInvNoY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Trasport_Name").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Transport Name", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Trasport_Name").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, pFont)
            End If



            BlockInvNoY = BlockInvNoY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Vehicle Number", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Vehicle_No").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, pFont)
            End If


            BlockInvNoY = BlockInvNoY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Date_Time_Of_Supply").ToString) <> "" Then
                p1Font = New Font("Calibri", 10, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "Date & Time of Supply", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Weaver_Pavu_Delivery_Date").ToString), "dd-MM-yyyy") & " " & prn_HdDt.Rows(0).Item("Date_Time_Of_Supply").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, p1Font)
            End If
            BlockInvNoY = BlockInvNoY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Place_Of_Supply").ToString) <> "" Then
                p1Font = New Font("Calibri", 10, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "Place of Supply", LMargin + Cen1 + 10, BlockInvNoY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + Cen1 + W1 + 10, BlockInvNoY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Place_Of_Supply").ToString, LMargin + Cen1 + W1 + 30, BlockInvNoY, 0, 0, p1Font)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + Cen1, LnAr(3), LMargin + Cen1, LnAr(2))



            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "S.NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "DESCRIPTION OF GOODS", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "HSN CODE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "GST%", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BEAMS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "RATE/BEAM", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Printing_Format2Gst_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal Cmp_Name As String, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim p1Font As Font
        Dim Rup1 As String, Rup2 As String
        Dim I As Integer
        Dim vTaxPerc As Single = 0
        Dim BnkDetAr() As String
        Dim BankNm1 As String = ""
        Dim BankNm2 As String = ""
        Dim BankNm3 As String = ""
        Dim BankNm4 As String = ""
        Dim BInc As Integer
        Dim w1 As Single = 0
        Dim w2 As Single = 0
        Dim Jurs As String = ""
        Dim vNoofHsnCodes As Integer = 0
        Dim Yax As Single
        Dim LedIdNo As Integer = 0
        Dim ItmIdNo As Integer = 0
        Dim InterStateStatus As Boolean = False
        Dim Rnd_off As Single = 0
        Dim NetAmt As Single = 0
        Dim NtAmt As Single = 0
        Dim CgstAmt As Single = 0
        Dim IgstAmt As Single = 0
        Dim SgstAmt As Single = 0
        LedIdNo = 0
        InterStateStatus = False


        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DelvAt.Text)
        InterStateStatus = Common_Procedures.Is_InterState_Party(con, Val(lbl_Company.Tag), LedIdNo)
        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "TOTAL", LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, (prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, (prn_HdDt.Rows(0).Item("Total_Meters").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Amount").ToString), "########0.00"), PageWidth - 5, CurY, 1, 0, pFont)
            End If
            CurY = CurY + TxtHgt + 5

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY


            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(5), LMargin, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), LnAr(5), LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), LnAr(5), LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(5), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            'CurY = CurY + TxtHgt - 10 + 10
            'Common_Procedures.Print_To_PrintDocument(e, "Assessable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
            'If is_LastPage = True Then
            '    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Assessable_Value").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
            'End If

            If is_LastPage = True Then
                Erase BnkDetAr
                If Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString) <> "" Then
                    BnkDetAr = Split(Trim(prn_HdDt.Rows(0).Item("Company_Bank_Ac_Details").ToString), ",")

                    BInc = -1
                    Yax = CurY

                    Yax = Yax + TxtHgt - 10
                    'If Val(prn_PageNo) = 1 Then
                    p1Font = New Font("Calibri", 12, FontStyle.Bold Or FontStyle.Underline)
                    Common_Procedures.Print_To_PrintDocument(e, "OUR BANK", LMargin + 20, Yax, 0, 0, p1Font)
                    'Common_Procedures.Print_To_PrintDocument(e, BnkDetAr(0), LMargin + 20, CurY, 0, 0, pFont)
                    'End If

                    p1Font = New Font("Calibri", 11, FontStyle.Bold)
                    BInc = BInc + 1
                    If UBound(BnkDetAr) >= BInc Then
                        Yax = Yax + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                    End If

                    BInc = BInc + 1
                    If UBound(BnkDetAr) >= BInc Then
                        Yax = Yax + TxtHgt - 3
                        Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                    End If

                    BInc = BInc + 1
                    If UBound(BnkDetAr) >= BInc Then
                        Yax = Yax + TxtHgt - 3
                        Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                    End If

                    BInc = BInc + 1
                    If UBound(BnkDetAr) >= BInc Then
                        Yax = Yax + TxtHgt - 3
                        Common_Procedures.Print_To_PrintDocument(e, Trim(BnkDetAr(BInc)), LMargin + 20, Yax, 0, 0, p1Font)
                    End If

                End If

            End If


            CurY = CurY + TxtHgt - 10
            If is_LastPage = True Then
                p1Font = New Font("Calibri", 10, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "Taxable Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Amount").ToString), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, p1Font)
            End If



            vTaxPerc = get_GST_Tax_Percentage_For_Printing(EntryCode)
            CgstAmt = Format(Val(prn_HdDt.Rows(0).Item("Amount").ToString) * Val(2.5) / 100, "########0.00")

            SgstAmt = Format(Val(prn_HdDt.Rows(0).Item("Amount").ToString) * Val(2.5) / 100, "########0.00")
            If InterStateStatus = True Then
                CurY = CurY + TxtHgt
                If is_LastPage = True Then
                    IgstAmt = Format(Val(prn_HdDt.Rows(0).Item("Amount").ToString) * Val(5) / 100, "########0.00")
                    Common_Procedures.Print_To_PrintDocument(e, "IGST @ 5 %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Amount").ToString) * Val(5) / 100, "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, pFont)
                End If
            Else
                CurY = CurY + TxtHgt
                If is_LastPage = True Then

                    Common_Procedures.Print_To_PrintDocument(e, "CGST @ 2.5 %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Amount").ToString) * Val(2.5) / 100, "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, pFont)
                End If



                CurY = CurY + TxtHgt
                If is_LastPage = True Then

                    Common_Procedures.Print_To_PrintDocument(e, "SGST @ 2.5 %", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)

                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Amount").ToString) * Val(2.5) / 100, "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, pFont)
                End If
            End If

            NetAmt = Val(lbl_Amount.Text) + Val(SgstAmt) + Val(CgstAmt) + Val(IgstAmt)
            NtAmt = Format(Val(NetAmt), "#########0")
            NtAmt = Common_Procedures.Currency_Format(Val(NtAmt))

            Rnd_off = Format(Val(CSng(NtAmt)) - Val(NetAmt), "#########0.00")
            CurY = CurY + TxtHgt
            ' If Val(prn_HdDt.Rows(0).Item("Round_Off").ToString) <> 0 Then
            If is_LastPage = True Then
                Common_Procedures.Print_To_PrintDocument(e, "Round Off", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(Rnd_off), "########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, pFont)
            End If
            ' End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, PageWidth, CurY)


            CurY = CurY + TxtHgt - 10

            p1Font = New Font("Calibri", 15, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, p1Font)
            If is_LastPage = True Then
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Currency_Format(Val(NtAmt)), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, p1Font)
            End If

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(6), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(5))

            Rup1 = ""
            Rup2 = ""
            If is_LastPage = True Then
                Rup1 = Common_Procedures.Rupees_Converstion(Val(prn_HdDt.Rows(0).Item("Amount").ToString))
                If Len(Rup1) > 80 Then
                    For I = 80 To 1 Step -1
                        If Mid$(Trim(Rup1), I, 1) = " " Then Exit For
                    Next I
                    If I = 0 Then I = 80
                    Rup2 = Microsoft.VisualBasic.Right(Trim(Rup1), Len(Rup1) - I)
                    Rup1 = Microsoft.VisualBasic.Left(Trim(Rup1), I - 1)
                End If
            End If

            '***** GST START *****
            CurY = CurY + TxtHgt - 12
            p1Font = New Font("Calibri", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Amount Chargeable (In Words)  : " & Rup1, LMargin + 10, CurY, 0, 0, p1Font)
            If Trim(Rup2) <> "" Then
                CurY = CurY + TxtHgt - 5
                Common_Procedures.Print_To_PrintDocument(e, "                                " & Rup2, LMargin + 10, CurY, 0, 0, p1Font)
            End If
            '***** GST END *****

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(10) = CurY


            'vNoofHsnCodes = get_GST_Noof_HSN_Codes_For_Printing(EntryCode)
            'If vNoofHsnCodes <> 0 Then
            Printing_GST_HSN_Details_Format1(e, EntryCode, TxtHgt, pFont, CurY, LMargin, PageWidth, PrintWidth, LnAr(10))
            'End If


            CurY = CurY + TxtHgt - 5

            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 10, CurY, 1, 0, p1Font)


            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Prepared by", LMargin + 15, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Checked by", LMargin + ClAr(1) + ClAr(2) - 20, CurY, 1, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 10, CurY, 1, 0, pFont)



            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Function get_GST_Noof_HSN_Codes_For_Printing(ByVal EntryCode As String) As Integer
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim NoofHsnCodes As Integer = 0

        NoofHsnCodes = 0

        Da = New SqlClient.SqlDataAdapter("Select * from Weaver_Pavu_Delivery_Details Where Weaver_Pavu_Delivery_Code = '" & Trim(EntryCode) & "'", con)
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


    Private Sub txt_Ewave_Bill_No_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Ewave_Bill_No.KeyDown
        If e.KeyValue = 38 Then
            If txt_Value.Visible = True Then
                txt_Value.Focus()
            Else
                txt_rate.Focus()
            End If
        End If

        If (e.KeyValue = 40) Then
            btn_save.Focus()
        End If

    End Sub

    Private Sub txt_Value_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Value.KeyDown
        If (e.KeyValue = 38) Then
            txt_rate.Focus()
        End If
        If (e.KeyValue = 40) Then
            txt_Ewave_Bill_No.Focus()
            'If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
            '    save_record()
            'Else
            '    msk_date.Focus()
            'End If
        End If
    End Sub

    Private Sub txt_Value_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Value.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If

        If Asc(e.KeyChar) = 13 Then
            txt_Ewave_Bill_No.Focus()
            'If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
            '    save_record()
            'Else
            '    msk_date.Focus()
            'End If

        End If
    End Sub


    Private Sub btn_OwnOrderSelection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_OwnOrderSelection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim Ent_Rate As Single = 0
        Dim Ent_Wgt As Single = 0
        Dim Ent_Pcs As Single = 0
        Dim NR As Single = 0
        Dim LedCondt As String = ""

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DelvAt.Text)
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        With dgv_OwnOrderSelection
            If Val(LedIdNo) <> 0 Then

                .Rows.Clear()
                SNo = 0

                Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name,c.*  from Own_Order_Head a INNER JOIN CLOTH_Head b ON  b.Cloth_IdNo = a.Cloth_Idno  INNER JOIN Own_Order_Weaving_Details c ON  c.Own_Order_Code = a.Own_Order_Code LEFT OUTER JOIN Weaver_Pavu_Delivery_Head d ON d.Own_Order_Code = a.Own_Order_Code  where a.Weaver_Pavu_Delivery_Code = '" & Trim(NewCode) & "'   and c.Ledger_IdNo = " & Val(LedIdNo) & " order by a.Own_Order_Date, a.for_orderby, a.Own_Order_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        Ent_Rate = 0


                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Own_Order_No").ToString
                        .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Own_Order_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Order_No").ToString


                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Order_Meters").ToString), "#########0.000")
                        .Rows(n).Cells(6).Value = "1"
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Own_Order_Code").ToString

                        For j = 0 To .ColumnCount - 1
                            .Rows(i).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Next

                End If
                Dt1.Clear()

                Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name from Own_Order_Head a INNER JOIN Cloth_Head b ON  b.Cloth_Idno = a.Cloth_Idno INNER JOIN Own_Order_Weaving_Details c ON  c.Own_Order_Code = a.Own_Order_Code   LEFT OUTER JOIN Weaver_Pavu_Delivery_Head d ON d.Weaver_Pavu_Delivery_Code = a.Own_order_Code    where a.Weaver_Pavu_Delivery_Code = ''  and c.Ledger_IdNo = " & Val(LedIdNo) & " order by a.Own_Order_Date, a.for_orderby, a.Own_Order_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("OWn_Order_No").ToString
                        .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Own_Order_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Order_No").ToString


                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Order_Meters").ToString), "#########0.000")
                        .Rows(n).Cells(6).Value = ""
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Own_Order_Code").ToString

                    Next

                End If
                Dt1.Clear()
            Else
                .Rows.Clear()
                SNo = 0

                Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name  from Own_Order_Head a INNER JOIN CLOTH_Head b ON  b.Cloth_IdNo = a.Cloth_Idno   LEFT OUTER JOIN Weaver_Pavu_Delivery_Head d ON d.Own_Order_Code = a.Own_Order_Code  where a.Weaver_Pavu_Delivery_Code = '" & Trim(NewCode) & "'   order by a.Own_Order_Date, a.for_orderby, a.Own_Order_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        Ent_Rate = 0


                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)

                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Own_Order_No").ToString
                        .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Own_Order_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Order_No").ToString

                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Order_Meters").ToString), "#########0.000")
                        .Rows(n).Cells(6).Value = "1"
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Own_Order_Code").ToString

                        For j = 0 To .ColumnCount - 1
                            .Rows(i).Cells(j).Style.ForeColor = Color.Red
                        Next

                    Next

                End If
                Dt1.Clear()

                Da = New SqlClient.SqlDataAdapter("select a.*, b.Cloth_Name from Own_Order_Head a INNER JOIN Cloth_Head b ON  b.Cloth_Idno = a.Cloth_Idno    LEFT OUTER JOIN Weaver_Pavu_Delivery_Head d ON d.Weaver_Pavu_Delivery_Code = a.Own_Order_Code    where a.Weaver_Pavu_Delivery_Code = ''   order by a.Own_Order_Date, a.for_orderby, a.Own_Order_No", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For i = 0 To Dt1.Rows.Count - 1

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Own_Order_No").ToString
                        .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Own_Order_Date").ToString), "dd-MM-yyyy")
                        .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Order_No").ToString

                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Cloth_Name").ToString

                        .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Order_Meters").ToString), "#########0.000")
                        .Rows(n).Cells(6).Value = ""
                        .Rows(n).Cells(7).Value = Dt1.Rows(i).Item("Own_Order_Code").ToString

                    Next

                End If
                Dt1.Clear()
            End If

            pnl_OwnOrderSelection.Visible = True
            pnl_Back.Enabled = False
            dgv_OwnOrderSelection.Focus()
        End With
    End Sub
    Private Sub dgv_OwnOrderSelection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_OwnOrderSelection.CellClick
        Select_OwnOrderPiece(e.RowIndex)
    End Sub

    Private Sub Select_OwnOrderPiece(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_OwnOrderSelection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(6).Value = (Val(.Rows(RwIndx).Cells(6).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(6).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next

                Else
                    .Rows(RwIndx).Cells(6).Value = ""

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Black
                    Next

                End If

            End If

        End With

    End Sub

    Private Sub dgv_OwnOrderSelection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_OwnOrderSelection.KeyDown
        Dim n As Integer = 0

        Try
            If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
                If dgv_OwnOrderSelection.CurrentCell.RowIndex >= 0 Then

                    n = dgv_OwnOrderSelection.CurrentCell.RowIndex

                    Select_OwnOrderPiece(n)

                    e.Handled = True

                End If
            End If


        Catch ex As Exception
            '---
        End Try


    End Sub

    Private Sub btn_Close_OwnOrderSelection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_OwnOrderSelection.Click
        Close_OwnOrder_Selection()
    End Sub

    Private Sub Close_OwnOrder_Selection()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0

        lbl_OrderNo.Text = ""
        lbl_OrderCode.Text = ""

        For i = 0 To dgv_OwnOrderSelection.RowCount - 1

            If Val(dgv_OwnOrderSelection.Rows(i).Cells(6).Value) = 1 Then

                ' lbl_RecCode.Text = dgv_Selection.Rows(i).Cells(8).Value

                lbl_OrderNo.Text = dgv_OwnOrderSelection.Rows(i).Cells(3).Value
                lbl_OrderCode.Text = dgv_OwnOrderSelection.Rows(i).Cells(7).Value

            End If

        Next

        pnl_Back.Enabled = True
        pnl_OwnOrderSelection.Visible = False
        If cbo_RecForm.Enabled And cbo_RecForm.Visible Then cbo_RecForm.Focus()

    End Sub

    Private Sub txt_DateTime_Of_Supply_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_DateTime_Of_Supply.GotFocus
        If Trim(txt_DateTime_Of_Supply.Text) = "" And New_Entry = True Then
            txt_DateTime_Of_Supply.Text = Format(Now, "hh:mm tt")
        End If
    End Sub

    Private Sub Amount_Calculation()
        Dim vTotMtrs As String = ""
        Dim vTotBms As String = ""

        vTotMtrs = 0
        vTotBms = 0
        With dgv_PavuDetails_Total
            If .RowCount > 0 Then
                vTotBms = Val(.Rows(0).Cells(2).Value)
                vTotMtrs = Format(Val(.Rows(0).Cells(5).Value), "########0.00")
            End If
        End With

        vTotBms = Val(vTotBms) + Val(txt_KuraiPavuBeam.Text)
        vTotMtrs = Val(vTotMtrs) + Val(txt_KuraiPavuMeter.Text)

        If cbo_Grid_RateFor.Text = "METER" Then
            txt_Value.Text = Format(Val(vTotMtrs) * Val(txt_rate.Text), "############0.00")
        ElseIf cbo_Grid_RateFor.Text = "KG" Then
            txt_Value.Text = Format(Val(lbl_pavu_weight.Text) * Val(txt_rate.Text), "############0.00")
        ElseIf cbo_Grid_RateFor.Text = "PAVU" Then
            txt_Value.Text = Format(Val(vTotBms) * Val(txt_rate.Text), "############0.00")
        End If


    End Sub

    Private Sub txt_Value_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Value.TextChanged
        lbl_Amount.Text = Format(Val(txt_Value.Text), "############0.00")
    End Sub

    Private Sub txt_KuraiPavuMeter_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_KuraiPavuMeter.TextChanged
        Amount_Calculation()
    End Sub

    Private Sub txt_Ewave_Bill_No_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Ewave_Bill_No.KeyPress
        If Asc(e.KeyChar) = 13 Then

            If MessageBox.Show("Do you want to save ?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                save_record()
            Else
                msk_date.Focus()
            End If

        End If

    End Sub

    Private Sub txt_KuraiPavuBeam_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_KuraiPavuBeam.TextChanged
        Amount_Calculation()
    End Sub

    Private Sub btn_Close_PavuSelection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close_PavuSelection.Click
        btn_Close_Selection_Click(sender, e)
    End Sub

    Private Sub set_PaperSize_For_PrintDocument1()
        Dim I As Integer = 0
        Dim PpSzSTS As Boolean = False
        Dim ps As Printing.PaperSize

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1242" Then
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    Exit For
                End If
            Next


        ElseIf Val(Common_Procedures.settings.Printing_For_HalfSheet_Set_Custom8X6_As_Default_PaperSize) = 1 Or Prnt_HalfSheet_STS = True Then

            Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.Landscape = False

        ElseIf Val(Common_Procedures.settings.Printing_For_HalfSheet_Set_A4_As_Default_PaperSize) = 1 Or Val(Common_Procedures.settings.Printing_For_FullSheet_Set_A4_As_Default_PaperSize) = 1 Or Val(vPrnt_2Copy_In_SinglePage) = 1 Then


            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1376" Then
                For I = 0 To PrintDocument2.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument2.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                        ps = PrintDocument2.PrinterSettings.PaperSizes(I)
                        PrintDocument2.PrinterSettings.DefaultPageSettings.PaperSize = ps
                        PrintDocument2.DefaultPageSettings.PaperSize = ps
                        Exit For
                    End If
                Next
            Else


                For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                        PrintDocument1.DefaultPageSettings.PaperSize = ps
                        Exit For
                    End If
                Next


            End If

        Else

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1376" Then
                Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
                PrintDocument2.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
                PrintDocument2.DefaultPageSettings.PaperSize = pkCustomSize1
                PrintDocument2.DefaultPageSettings.Landscape = False
            Else
                PpSzSTS = False

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
            End If


        End If

    End Sub

    Private Sub btn_SMS_Click(sender As System.Object, e As System.EventArgs) Handles btn_SMS.Click
        Dim i As Integer = 0
        Dim smstxt As String = ""
        Dim PhNo As String = "", EndsCount As String = ""
        Dim Led_IdNo As Integer = 0, Endscount_IdNo As Integer = 0
        Dim SMS_SenderID As String = ""
        Dim SMS_Key As String = ""
        Dim SMS_RouteID As String = ""
        Dim SMS_Type As String = ""
        Dim BlNos As String = ""

        Try

            Led_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_DelvAt.Text)

            PhNo = Common_Procedures.get_FieldValue(con, "Ledger_Head", "MobileNo_Frsms", "(Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")")

            'Endscount_IdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_EndsCount.Text)
            'EndsCount = ""
            'If Val(Endscount_IdNo) <> 0 Then
            '    EndsCount = Common_Procedures.get_FieldValue(con, "EndsCount_Name", "EndsCount_name", "(EndsCount_IdNo = " & Str(Val(Endscount_IdNo)) & ")")
            'End If

            ' If Trim(AgPNo) <> "" Then
            PhNo = Trim(PhNo) & IIf(Trim(PhNo) <> "", "", "")
            ' End If

            ' smstxt = Trim(cbo_.Text) & vbCrLf
            smstxt = smstxt & " DC No : " & Trim(lbl_DcNo.Text) & vbCrLf
            smstxt = smstxt & " Date : " & Trim(msk_date.Text) & vbCrLf

            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then
            '    If Trim(cbo_Transport.Text) <> "" Then
            '        smstxt = smstxt & " Transport : " & Trim(cbo_Transport.Text) & vbCrLf
            '    End If

            'End If
            If dgv_PavuDetails_Total.RowCount > 0 Then
                smstxt = smstxt & " Total Meters: " & Val((dgv_PavuDetails_Total.Rows(0).Cells(5).Value())) & vbCrLf

                smstxt = smstxt & " Total Beam : " & Val(dgv_PavuDetails_Total.Rows(0).Cells(2).Value()) & vbCrLf
            End If

            If dgv_PavuDetails.RowCount > 0 Then
                smstxt = smstxt & " Ends Count : " & Trim((dgv_PavuDetails.Rows(0).Cells(6).Value())) & vbCrLf

            End If
            'smstxt = smstxt & " Ends Count : " & Trim(EndsCount) & vbCrLf
            'smstxt = smstxt & " Tax Amount : " & Val(lbl_CGST_Amount.Text) + Val(lbl_SGST_Amount.Text) + Val(lbl_IGST_Amount.Text) & vbCrLf
            'smstxt = smstxt & " Net Amount : " & Trim(lbl_Net_Amt.Text) & vbCrLf

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
            MessageBox.Show(ex.Message, "DOES NOT SEND SMS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub cbo_Verified_Sts_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Verified_Sts.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "", "", "", "")

    End Sub
    Private Sub cbo_Verified_Sts_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Verified_Sts.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Verified_Sts, cbo_Filter_EndsCountName, btn_Filter_Show, "", "", "", "")
    End Sub

    Private Sub cbo_Verified_Sts_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Verified_Sts.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Verified_Sts, btn_Filter_Show, "", "", "", "")
    End Sub

    Private Sub btn_UserModification_Click(sender As System.Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub

    Private Sub txt_CrimpPerc_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CrimpPerc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub cbo_DCSufixNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_DCSufixNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, txt_Ewave_Bill_No, msk_date, "", "", "", "")
    End Sub

    Private Sub cbo_DCSufixNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_DCSufixNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, msk_date, "", "", "", "")
    End Sub



    Private Sub Printing_Format_1376(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer = 0, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0
        Dim cnt As Integer = 0
        Dim AMount As Integer
        PrntCnt = 1


        set_PaperSize_For_PrintDocument1()

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        Exit For
        '    End If
        'Next


        With PrintDocument2.DefaultPageSettings.Margins
            .Left = 20
            .Right = 50
            .Top = 10 ' 20
            .Bottom = 40
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

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

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        pFont = New Font("Calibri", 9, FontStyle.Regular)

        NoofItems_PerPage = 5

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 35 : ClArr(2) = 75 : ClArr(3) = 60 : ClArr(4) = 130 : ClArr(5) = 55
        ClArr(6) = 75 : ClArr(7) = 75 : ClArr(8) = 70 : ClArr(9) = 75
        ClArr(10) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9))

        TxtHgt = 16.5 ' 18.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20



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


        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_Prev_HeadIndx = prn_HeadIndx

        PrntCnt2ndPageSTS = False
        TpMargin = TMargin

        If Trim(Common_Procedures.settings.CustomerCode) = "1037" And vPrnt_2Copy_In_SinglePage = 1 Then
            PCnt = PCnt + 1
            PrntCnt = PrntCnt + 1
        End If


        For PCnt = 1 To PrntCnt

            If vPrnt_2Copy_In_SinglePage = 1 Then

                If PCnt = 1 Then
                    prn_PageNo1 = prn_PageNo

                    prn_DetIndx1 = prn_DetIndx
                    prn_DetSNo1 = prn_DetSNo
                    prn_NoofBmDets1 = prn_NoofBmDets
                    TpMargin = TMargin


                Else

                    prn_PageNo = prn_PageNo1
                    prn_NoofBmDets = prn_NoofBmDets1
                    prn_DetIndx = prn_DetIndx1
                    prn_DetSNo = prn_DetSNo1

                    TpMargin = 560 + TMargin  ' 600 + TMargin

                End If

            End If


            Try

                If prn_HdDt.Rows.Count > 0 Then

                    Printing_Format_1376_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TpMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                    NoofDets = 0

                    NoofItems_PerPage = 5
                    If Val(vPrnt_2Copy_In_SinglePage) = 1 Then
                        If prn_DetDt.Rows.Count > (2 * NoofItems_PerPage) Then
                            If Trim(Common_Procedures.settings.CustomerCode) = "1037" Then
                                NoofDets = NoofDets + 1
                                NoofItems_PerPage = 6
                            Else
                                NoofItems_PerPage = 35
                            End If
                        End If
                    End If


                    CurY = CurY - 10

                    'If prn_DetDt.Rows.Count > 0 Or prn_DetMxIndx > 0 Then
                    If prn_DetDt.Rows.Count > 0 Then

                        Do While prn_DetIndx <= prn_DetMxIndx

                            If NoofDets >= NoofItems_PerPage Then

                                If Val(vPrnt_2Copy_In_SinglePage) = 1 Then

                                    If PCnt = 2 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then

                                        NoofDets = NoofDets + 1

                                        CurY = CurY + TxtHgt

                                        Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)


                                        Printing_Format_1376_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, PCnt, False)

                                        prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                                        e.HasMorePages = True

                                        Return

                                    End If

                                Else

                                    CurY = CurY + TxtHgt

                                    Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)


                                    Printing_Format_1376_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, PCnt, False)

                                    prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                                    e.HasMorePages = True

                                    Return

                                End If

                            End If

                            prn_DetIndx = prn_DetIndx + 1

                            CurY = CurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetIndx, LMargin + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(prn_DetIndx, 1), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(prn_DetIndx, 2), LMargin + ClArr(1) + ClArr(2) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(prn_DetIndx, 3), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(prn_DetIndx, 4), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 10, CurY, 0, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(prn_DetIndx, 5), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(prn_DetIndx, 6), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(prn_DetIndx, 7), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(prn_DetIndx, 8), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_DetAr(prn_DetIndx, 9), PageWidth - 10, CurY, 1, 0, pFont)


                            NoofDets = NoofDets + 1

                        Loop

                    End If


                    Printing_Format_1376_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, PCnt, True)

                End If

            Catch ex As Exception

                MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try


            If Val(vPrnt_2Copy_In_SinglePage) = 1 Then

                If PCnt = 1 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then
                    If prn_DetDt.Rows.Count > prn_NoofBmDets Then
                        PrntCnt2ndPageSTS = True
                        e.HasMorePages = True
                        cnt = 6
                        Return
                    End If
                End If
            End If

            PrntCnt2ndPageSTS = False

        Next PCnt


LOOP2:

        prn_Count = prn_Count + 1


        e.HasMorePages = False

        If Val(prn_TotCopies) > 1 Then
            If prn_Count < Val(prn_TotCopies) Then

                prn_DetIndx = 0
                'prn_DetSNo = 0
                prn_PageNo = 0
                prn_DetIndx = 0
                prn_NoofBmDets = 0


                e.HasMorePages = True
                Return

            End If

        Else

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

        End If

    End Sub

    Private Sub Printing_Format_1376_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim W1 As Single, N1 As Single, M1 As Single, w2 As Single
        Dim Arr(300, 5) As String
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String, Cmp_GSTNo As String
        Dim strWidth As Single = 0
        Dim Entry_Date As Date = Convert.ToDateTime(prn_HdDt.Rows(0).Item("Weaver_Pavu_Delivery_Date"))

        PageNo = PageNo + 1

        CurY = TMargin

        If prn_DetMxIndx > (2 * NoofItems_PerPage) Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        CurY = TMargin
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = "" : Cmp_GSTNo = ""

        Dim vADD_BOLD_STS As Boolean = False

        vADD_BOLD_STS = False
        If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")
            Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString)

            Dim vADDR1 As String = ""

            vADDR1 = Trim(prn_HdDt.Rows(0).Item("Company_Address1").ToString)
            vADDR1 = Replace(vADDR1, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) & ",", "")
            vADDR1 = Replace(vADDR1, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")

            Cmp_Add2 = vADDR1 & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

            vADD_BOLD_STS = True

        Else

            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
            Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

        End If

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
            Cmp_GSTNo = "GSTIN : " & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If

        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight - 2
        If vADD_BOLD_STS = True Then
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height
        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)
            strHeight = TxtHgt
        End If

        CurY = CurY + strHeight - 1
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        CurY = CurY + TxtHgt
        If Entry_Date >= Common_Procedures.GST_Start_Date Then
            Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTNo, LMargin, CurY, 2, PrintWidth, pFont)
        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin, CurY, 2, PrintWidth, pFont)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, LMargin, CurY, 2, PrintWidth, pFont)
        End If


        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "PAVU DELIVERY TO JOBWORK", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight - 3

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1395" Then '---- SANTHA EXPORTS (SOMANUR)

            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC CODE : 998821 ( Textile manufactring service )", LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        Else

            p1Font = New Font("Calibri", 10, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "( NOT FOR SALE )", LMargin + 10, CurY, 2, PrintWidth, p1Font)
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC CODE : 998821 ( Textile manufactring service )", LMargin, CurY, 2, PrintWidth, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "( NOT FOR SALE )", LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        End If


        CurY = CurY + strHeight ' + 3
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)



        LnAr(2) = CurY



        N1 = e.Graphics.MeasureString("TO   : ", pFont).Width
        W1 = e.Graphics.MeasureString("SET NO  :  ", pFont).Width
        w2 = e.Graphics.MeasureString("COUNT NAME :  ", pFont).Width
        'If Common_Procedures.settings.CustomerCode = "1391" Then
        M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)

        CurY = CurY + 5
        Common_Procedures.Print_To_PrintDocument(e, "TO :  M/s. " & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)

        Common_Procedures.Print_To_PrintDocument(e, "DC.NO  :  " & prn_HdDt.Rows(0).Item("Weaver_Pavu_Delivery_No").ToString & "           DC.Date  :  " & Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Weaver_Pavu_Delivery_Date")), "dd-MM-yyyy"), LMargin + M1 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont,, True)

        Common_Procedures.Print_To_PrintDocument(e, "JO.NO  :  " & prn_HdDt.Rows(0).Item("Job_Order_No").ToString & "            JO.Date  :  " & (prn_HdDt.Rows(0).Item("Job_Order_Date")), LMargin + M1 + 10, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont,, True)

        Common_Procedures.Print_To_PrintDocument(e, "Party Ref.No  :  " & prn_HdDt.Rows(0).Item("Party_Ref_No").ToString & "                 Party Ref.Date  :  " & (prn_HdDt.Rows(0).Item("Party_Ref_Date")), LMargin + M1 + 10, CurY, 0, 0, pFont)
        CurY = CurY + TxtHgt


        If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Pan_No").ToString) <> "" Then

            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + N1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + N1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + N1 + W1 + 30, CurY, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("Pan_No").ToString) <> "" Then
                strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
                Common_Procedures.Print_To_PrintDocument(e, "   PAN : " & prn_HdDt.Rows(0).Item("Pan_No").ToString, LMargin + N1 + W1 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
            End If
        End If


        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(2))
        ' Try


        'Else

        '    M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)

        'End If


        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 11, FontStyle.Bold)

        Common_Procedures.Print_To_PrintDocument(e, "Count Name", LMargin + 10, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + w2 + 10, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Count_name").ToString, LMargin + w2 + 25, CurY, 0, 0, pFont)


        Common_Procedures.Print_To_PrintDocument(e, "Desp.To", LMargin + M1 + 10, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, "M / s." & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + M1 + W1 + 25, CurY, 0, 0, p1Font)

        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "Hsn Code", LMargin + 10, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + w2 + 10, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Item_hsn_Code").ToString, LMargin + w2 + 25, CurY, 0, 0, pFont)


        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + M1 + 10, CurY, 0, 0, pFont,, True)


        CurY = CurY + TxtHgt


        Common_Procedures.Print_To_PrintDocument(e, "Mill Name", LMargin + 10, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + w2 + 10, CurY, 0, 0, pFont)

        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(0).Item("Mill_name").ToString, LMargin + w2 + 25, CurY, 0, 0, pFont)


        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + M1 + 10, CurY, 0, 0, pFont,, True)

        CurY = CurY + TxtHgt
        If Entry_Date >= Common_Procedures.GST_Start_Date Then
            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("Pan_No").ToString) <> "" Then
                If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + M1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + M1 + W1 + 30, CurY, 0, 0, pFont)
                End If

                If Trim(prn_HdDt.Rows(0).Item("Pan_No").ToString) <> "" Then
                    strWidth = e.Graphics.MeasureString(" " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, pFont).Width
                    Common_Procedures.Print_To_PrintDocument(e, "   PAN : " & prn_HdDt.Rows(0).Item("Pan_No").ToString, LMargin + M1 + W1 + 30 + strWidth, CurY, 0, PrintWidth, pFont)
                End If
            End If

        Else
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_TinNo").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont)
        End If

        CurY = CurY + TxtHgt + 5
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + M1, LnAr(3), LMargin + M1, LnAr(2))
        'e.Graphics.DrawLine(Pens.Black, LMargin + M1 + 4, LnAr(3), LMargin + M1 + 4, LnAr(2))

        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "S.No.", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Set Date", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Set No.", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)

        Common_Procedures.Print_To_PrintDocument(e, "No.Of", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Beam No", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Beam Length", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 3, CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Gross Wgt", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Net Wgt", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Warp Wgt", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Approximate", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "Ends", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "In Meters", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 3, CurY, 2, ClAr(6), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "In Kgs.", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "In Kgs.", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "In Kgs.", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "Value", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)

        CurY = CurY + TxtHgt + 10
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

        ' Catch ex As Exception

        'MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        '  End Try

    End Sub

    Private Sub Printing_Format_1376_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean, ByVal Pcnt As Integer)
        Dim p1Font As Font
        Dim Cmp_Name As String
        Dim I As Integer
        Dim M1 As Single
        Dim Area_Nm As String = ""
        Dim LedAdd1 As String = ""
        Dim LedAdd2 As String = ""
        Dim LedAdd3 As String = ""
        Dim LedAdd4 As String = ""
        Dim W1 As Single
        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))

            CurY = CurY + TxtHgt - 10

            If is_LastPage = True Then



                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString), "#######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(GrossWt), "#######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(NetWt), "#######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(WarpWt), "#######0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Value").ToString), "#######0.00"), PageWidth - 10, CurY, 1, 0, pFont)


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
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))

            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + 20, CurY, LMargin + ClAr(1) + ClAr(2) + 20, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 5, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 5, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 5, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 5, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 50, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 50, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 50, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 50, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + 50, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + 50, LnAr(3))
            M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 30
            If Val(prn_HdDt.Rows(0).Item("RecArea").ToString) <> 0 Then
                Area_Nm = Common_Procedures.Area_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("RecArea").ToString))
            ElseIf Trim(prn_HdDt.Rows(0).Item("RecAdd4").ToString) <> "" Then
                LedAdd1 = Trim(prn_HdDt.Rows(0).Item("RecAdd4").ToString & " " & prn_HdDt.Rows(0).Item("RecAdd3").ToString)
            ElseIf Trim(prn_HdDt.Rows(0).Item("RecAdd3").ToString) <> "" Then
                LedAdd2 = Trim(prn_HdDt.Rows(0).Item("RecAdd3").ToString & " " & prn_HdDt.Rows(0).Item("RecAdd2").ToString)
            ElseIf Trim(prn_HdDt.Rows(0).Item("RecAdd2").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("RecAdd1").ToString) <> "" Then
                LedAdd3 = Trim(prn_HdDt.Rows(0).Item("RecAdd2").ToString & " " & prn_HdDt.Rows(0).Item("RecAdd1").ToString)

            End If

            W1 = e.Graphics.MeasureString("Received From  :  ", pFont).Width
            CurY = CurY + TxtHgt - 15

            Common_Procedures.Print_To_PrintDocument(e, "Received From ", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Receiver_Name").ToString), LMargin + W1 + 30, CurY, 0, 0, pFont)

            If Trim(prn_HdDt.Rows(0).Item("Ewave_Bill_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "E-Way Bill No", LMargin + M1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Ewave_Bill_No").ToString), LMargin + M1 + W1 + 30, CurY, 0, 0, pFont)
            End If



            CurY = CurY + TxtHgt
            If Trim(Area_Nm) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Area_Nm, LMargin + 30, CurY, 0, 0, pFont)
            ElseIf Trim(LedAdd1) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, LedAdd1, LMargin + 30, CurY, 0, 0, pFont)
            ElseIf Trim(LedAdd2) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, LedAdd2, LMargin + 30, CurY, 0, 0, pFont)
            ElseIf Trim(LedAdd3) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, LedAdd3, LMargin + 30, CurY, 0, 0, pFont)
            End If

            'If Val(prn_HdDt.Rows(0).Item("Value").ToString) <> 0 Then
            '    Common_Procedures.Print_To_PrintDocument(e, "Value", LMargin + M1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
            '    Common_Procedures.Print_To_PrintDocument(e, (prn_HdDt.Rows(0).Item("Value").ToString), LMargin + M1 + W1 + 30, CurY, 0, 0, pFont)
            'End If



            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Transport_IDnO").ToString) <> 0 Then
                If Common_Procedures.settings.CustomerCode = "1391" Then
                    Common_Procedures.Print_To_PrintDocument(e, "Sizing Name ", LMargin + 10, CurY, 0, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "Transport Name ", LMargin + 10, CurY, 0, 0, pFont)
                End If

                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Transport_IDnO").ToString)), LMargin + W1 + 30, CurY, 0, 0, pFont)
            End If


            If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Vehicle No", LMargin + M1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + M1 + W1 + 30, CurY, 0, 0, pFont)
            End If



            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY
            Common_Procedures.Print_To_PrintDocument(e, "Received The Beams As Per Above Details.", LMargin + 10, CurY + 5, 0, 0, pFont)

            CurY = CurY + TxtHgt
            '   If Val(Common_Procedures.User.IdNo) <> 1 Then
            If Common_Procedures.settings.CustomerCode <> "1391" Then
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 300, CurY, 0, 0, pFont)
            End If

            ' End If

            CurY = CurY + TxtHgt

            CurY = CurY + TxtHgt + 5

            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
                Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")
            End If

            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 300, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 5, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
            e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument2_BeginPrint(sender As Object, e As PrintEventArgs) Handles PrintDocument2.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim DT1 As New DataTable
        Dim NewCode As String
        Dim Cmd As New SqlClient.SqlCommand
        Dim Amount As Integer = 0


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_PageNo = 0
        prn_NoofBmDets = 0
        prn_DetMxIndx = 0
        prn_Count = 0
        prn_Prev_HeadIndx = -100
        prn_HeadIndx = 0
        GrossWt = 0
        NetWt = 0
        WarpWt = 0

        Erase prn_DetAr

        prn_DetAr = New String(50, 10) {}

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.* , c.Ledger_MainName as Receiver_Name ,C.Ledger_Address1 as RecAdd1,C.Ledger_Address2 as RecAdd2,C.Ledger_Address3 as RecAdd3,c.Ledger_Address4 as RecAdd4,c.Area_Idno as RecArea, d.EndsCount_Name , e.Ledger_Name  as Trasport_Name  , f.*,Csh.State_Name as Company_State_Name, Csh.State_Code as Company_State_Code, Lsh.State_Name as Ledger_State_Name, Lsh.State_Code as Ledger_State_Code from Weaver_Pavu_Delivery_Head a LEFT OUTER JOIN Ledger_Head b ON a.DeliveryTo_IdNo = b.Ledger_IdNo LEFT OUTER JOIN Ledger_Head c ON a.ReceivedFrom_IdNo = c.Ledger_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo LEFT OUTER JOIN Ledger_Head e ON a.Transport_IdNo = e.Ledger_IdNo INNER JOIN Company_Head f ON a.Company_IdNo = f.Company_IdNo  LEFT OUTER JOIN State_Head Csh ON F.Company_State_IdNo = Csh.State_IdNo LEFT OUTER JOIN State_Head Lsh ON b.Ledger_State_IdNo = Lsh.State_IdNo  where a.Weaver_Pavu_Delivery_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                Cmd.Connection = con

                Cmd.CommandText = "Truncate table " & Trim(Common_Procedures.EntryTempTable) & ""
                Cmd.ExecuteNonQuery()

                da2 = New SqlClient.SqlDataAdapter("select sp.Sizing_Specification_date as set_date,Sp.set_no, b.EndsCount_name,ml.mill_name,d.count_name,e.Item_hsn_Code,Sp.Gross_weight,Sp.net_weight,Sp.Warp_weight,a.*  from Weaver_Pavu_Delivery_Details a Left Outer Join Stock_SizedPavu_Processing_Details s On s.Set_no=a.Set_no  and s.Beam_No=a.Beam_No Left outer join Sizing_SpecificationPavu_Details sp On s.Set_no=sp.Set_no  and s.Beam_No=sp.Beam_No Left outer join Sizing_SpecificationYarn_Details sy On sy.Sizing_Specification_Code=sp.Sizing_Specification_Code  LEFT OUTER JOIN EndsCount_Head b ON a.EndsCount_idno = b.EndsCount_idno LEFT OUTER JOIN Count_Head d ON b.Count_idno = d.Count_idno LEFT OUTER JOIN mill_Head ml ON sy.mill_idno = ml.mill_idno LEFT OUTER JOIN itemgroup_head e ON d.itemgroup_idno = e.itemgroup_idno where Weaver_Pavu_Delivery_Code ='" & Trim(NewCode) & "' Order by a.Sl_No", con)
                prn_DetDt = New DataTable
                da2.Fill(prn_DetDt)

                If prn_DetDt.Rows.Count > 0 Then
                    For i = 0 To prn_DetDt.Rows.Count - 1
                        If Val(prn_DetDt.Rows(i).Item("Meters").ToString) <> 0 Then
                            prn_DetMxIndx = prn_DetMxIndx + 1
                            prn_DetAr(prn_DetMxIndx, 1) = Format(Convert.ToDateTime((prn_DetDt.Rows(i).Item("set_date").ToString)), "dd-MM-yyyy")
                            prn_DetAr(prn_DetMxIndx, 2) = Trim(prn_DetDt.Rows(i).Item("set_no").ToString)
                            prn_DetAr(prn_DetMxIndx, 3) = Trim(Microsoft.VisualBasic.Left(prn_DetDt.Rows(i).Item("EndsCount_Name").ToString, 15))
                            prn_DetAr(prn_DetMxIndx, 4) = Trim(prn_DetDt.Rows(i).Item("Beam_no").ToString) '
                            prn_DetAr(prn_DetMxIndx, 5) = Format(Val(prn_DetDt.Rows(i).Item("Meters").ToString), "#########0.000") '
                            prn_DetAr(prn_DetMxIndx, 6) = Format(Val(prn_DetDt.Rows(i).Item("gross_weight").ToString), "#########0.000") '
                            prn_DetAr(prn_DetMxIndx, 7) = Format(Val(prn_DetDt.Rows(i).Item("net_weight").ToString), "#########0.000")
                            prn_DetAr(prn_DetMxIndx, 8) = Format(Val(prn_DetDt.Rows(i).Item("Warp_weight").ToString), "#########0.000")
                            ' Amount = Format(Val(prn_HdDt.Rows(0).Item("Rate").ToString) * Val(prn_DetAr(prn_DetIndx, 8)), "##########0.00")

                            prn_DetAr(prn_DetMxIndx, 9) = Format(Val(prn_HdDt.Rows(0).Item("Rate").ToString) * Val(prn_DetDt.Rows(i).Item("Warp_weight").ToString), "#########0.00") 'Format(Val(prn_DetDt.Rows(i).Item("Value").ToString), "#########0.000")

                            Cmd.CommandText = "Insert into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Int1, Meters1) values ('" & Trim(Microsoft.VisualBasic.Left(prn_DetDt.Rows(i).Item("EndsCount_Name").ToString, 15)) & "', 1, " & Str(Val(Format(Val(prn_DetDt.Rows(i).Item("Meters").ToString), "#########0.00"))) & ")"
                            Cmd.ExecuteNonQuery()

                            GrossWt = GrossWt + Format(Val(prn_DetDt.Rows(i).Item("gross_weight").ToString), "#########0.000") '
                            NetWt = NetWt + Format(Val(prn_DetDt.Rows(i).Item("Net_weight").ToString), "#########0.00") '
                            WarpWt = WarpWt + Format(Val(prn_DetDt.Rows(i).Item("Warp_weight").ToString), "#########0.00") '
                        End If
                    Next i
                End If





            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub PrintDocument2_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument2.PrintPage
        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        Printing_Format_1376(e)

    End Sub


    Private Sub cbo_Grid_RateFor_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_RateFor.GotFocus
        vCbo_ItmNm = Trim(cbo_Grid_RateFor.Text)
    End Sub

    Private Sub cbo_Grid_RateFor_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_RateFor.KeyDown
        Dim dep_idno As Integer = 0

        vcbo_KeyDwnVal = e.KeyValue

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_RateFor, Nothing, Nothing, "", "", "", "")




        If (e.KeyValue = 38 And cbo_Grid_RateFor.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            txt_rate.Focus()

        End If

        If (e.KeyValue = 40 And cbo_Grid_RateFor.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            txt_Ewave_Bill_No.Focus()
        End If

    End Sub

    Private Sub cbo_Grid_RateFor_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_RateFor.KeyPress

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_RateFor, Nothing, "", "", "", "")

        If Asc(e.KeyChar) = 13 Then
            txt_Ewave_Bill_No.Focus()


        End If

    End Sub

    Private Sub cbo_Grid_RateFor_TextChanged(sender As Object, e As EventArgs) Handles cbo_Grid_RateFor.TextChanged
        Amount_Calculation()

    End Sub

    Private Sub btn_Jo_details_Click(sender As Object, e As EventArgs) Handles btn_Jo_details.Click
        pnl_job_order_details.Visible = True
        pnl_job_order_details.Enabled = True
        pnl_job_order_details.BringToFront()
        pnl_Back.Enabled = False
        txt_JO_No.Focus()
    End Sub


    Private Sub txt_Ref_date_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_Ref_date.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_JO_No.Focus()

        End If
    End Sub

    Private Sub txt_Ref_date_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_Ref_date.KeyDown
        If e.KeyCode = 40 Then
            txt_JO_No.Focus()


        End If

        If e.KeyCode = 38 Then
            txt_Ref_No.Focus()

        End If
    End Sub

    Private Sub btn_JO_details_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_JO_details_close.Click
        pnl_Back.Enabled = True
        pnl_job_order_details.Visible = False

    End Sub
    Private Sub btn_EWayBIll_Generation_Click(sender As Object, e As EventArgs) Handles btn_EWayBIll_Generation.Click
        btn_GENERATEEWB.Enabled = True
        Grp_EWB.Visible = True
        Grp_EWB.BringToFront()
        Grp_EWB.Left = (Me.Width - pnl_Back.Width) / 2 + 135
        Grp_EWB.Top = (Me.Height - pnl_Back.Height) / 2 + 160
    End Sub
    Private Sub btn_CheckConnectivity_Click(sender As Object, e As EventArgs) Handles btn_CheckConnectivity.Click
        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        'Dim einv As New eInvoice(Val(lbl_Company.Tag))
        'einv.GetAuthToken(rtbEWBResponse)

        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.GetAuthToken(rtbEWBResponse)
    End Sub

    Private Sub btn_GENERATEEWB_Click(sender As Object, e As EventArgs) Handles btn_GENERATEEWB.Click
        Dim dt1 As New DataTable

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        '   Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Dim NewCode As String = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        If Val(txt_rate.Text) = 0 Then
            MessageBox.Show("Invalid Rate", "DOES NOT GENERATE EWB...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If txt_rate.Enabled And txt_rate.Visible Then txt_rate.Focus()
            Exit Sub
        End If


        Dim da As New SqlClient.SqlDataAdapter("Select EWave_Bill_No from Weaver_Pavu_Delivery_Head where Weaver_Pavu_Delivery_Code = '" & NewCode & "'", con)
        Dim dt As New DataTable

        da.Fill(dt)

        If dt.Rows.Count = 0 Then
            MessageBox.Show("Please Save the Invoice before proceeding to generate EWB", "Please SAVE", MessageBoxButtons.OKCancel)
            dt.Clear()
            Exit Sub
        End If

        If Not IsDBNull(dt.Rows(0).Item(0)) Then
            If Len(Trim(dt.Rows(0).Item(0))) > 0 Then
                MessageBox.Show("EWB has been generated for this invoice already", "Redundant Request", MessageBoxButtons.OKCancel)
                dt.Clear()
                Exit Sub
            End If
        End If

        dt.Clear()

        Dim vTaxable_Amt_Cond = ""
        Dim vQty_Cond = ""
        Dim vUnit_Cond = ""
        Dim CMD As New SqlClient.SqlCommand
        CMD.Connection = con

        'Dim vSgst As String = 0
        'Dim vCgst As String = 0
        'Dim vIgst As String = 0

        'vSgst = ("a.TotalInvValue" * 5)

        'vSgst = vCgst

        'vIgst = 0

        CMD.CommandText = "Delete from EWB_Head Where InvCode = '" & NewCode & "'"
        CMD.ExecuteNonQuery()



        CMD.CommandText = "Insert into EWB_Head ([SupplyType]  ,[SubSupplyType]  , [SubSupplyDesc]  ,[DocType]  ,	[EWBGenDocNo]                           ,[EWBDocDate]        ,[FromGSTIN]       ,[FromTradeName]  ,[FromAddress1]   ,[FromAddress2]     ,[FromPlace] ," &
                         "[FromPINCode]     ,	[FromStateCode] ,[ActualFromStateCode] ,[ToGSTIN]       ,[ToTradeName],[ToAddress1]      ,[ToAddress2]    ,[ToPlace]       ,[ToPINCode]       ,[ToStateCode] , [ActualToStateCode] ," &
                         "[TransactionType],[OtherValue]                       ,	[Total_value]       ,	[CGST_Value],[SGST_Value],[IGST_Value]     ,	[CessValue],[CessNonAdvolValue],[TransporterID]    ,[TransporterName]," &
                         "[TransportDOCNo] ,[TransportDOCDate]    ,[TotalInvValue]    ,[TransMode]             ," &
                         "[VehicleNo]      ,[VehicleType]   , [InvCode], [ShippedToGSTIN], [ShippedToTradeName] ) " &
                         " " &
                         " " &
                         "  SELECT               'O'              , '4'             ,   'JOB WORK'              ,    'CHL'    ,  a.Weaver_Pavu_Delivery_No ,a.Weaver_Pavu_Delivery_Date           , C.Company_GSTINNo, C.Company_Name   ,C.Company_Address1+C.Company_Address2,c.Company_Address3+C.Company_Address4,C.Company_City ," &
                         " C.Company_PinCode    , FS.State_Code  ,FS.State_Code    ,L.Ledger_GSTINNo, L.Ledger_MainName, (case when a.DeliveryTo_IdNo <> 0 then tDELV.Ledger_Address1+tDELV.Ledger_Address2 else  L.Ledger_Address1+L.Ledger_Address2 end) as deliveryaddress1,  (case when a.DeliveryTo_IdNo <> 0 then tDELV.Ledger_Address3+tDELV.Ledger_Address4 else  L.Ledger_Address3+L.Ledger_Address4 end) as deliveryaddress2, (case when a.DeliveryTo_IdNo <> 0 then tDELV.City_Town else  L.City_Town end) as city_town_name, (case when a.DeliveryTo_IdNo <> 0 then tDELV.Pincode else  L.Pincode end) as pincodee, TS.State_Code, (case when a.DeliveryTo_IdNo <> 0 then TDCS.State_Code else TS.State_Code end) as actual_StateCode," &
                         " 1                     , 0 , a.Amount     ,   0  ,  0  , 0   ,   0         ,0                   , t.Ledger_GSTINNo  , t.Ledger_Name ," &
                         " ''        ,''            ,  0        ,     '1'  AS TrMode ," &
                         " a.Vehicle_No, 'R', '" & NewCode & "', tDELV.Ledger_GSTINNo as ShippedTo_GSTIN, tDELV.Ledger_MainName as ShippedTo_LedgerName from Weaver_Pavu_Delivery_Head a inner Join Company_Head C on a.Company_IdNo = C.Company_IdNo " &
                         " Inner Join Ledger_Head L ON a.DeliveryTo_IdNo = L.Ledger_IdNo Left Outer Join Ledger_Head tDELV on a.DeliveryTo_IdNo <> 0 and a.DeliveryTo_IdNo = tDELV.Ledger_IdNo left Outer Join Ledger_Head T on a.Transport_IdNo = T.Ledger_IdNo " &
                         " Left Outer Join State_Head FS On C.Company_State_IdNo = fs.State_IdNo left Outer Join State_Head TS on L.Ledger_State_IdNo = TS.State_IdNo  left Outer Join State_Head TDCS on tDELV.Ledger_State_IdNo = TDCS.State_IdNo " &
                          " where a.Weaver_Pavu_Delivery_Code = '" & Trim(NewCode) & "'"
        CMD.ExecuteNonQuery()


        'CMD.CommandText = "Insert into EWB_Head ([SupplyType]  ,[SubSupplyType]  , [SubSupplyDesc]  ,[DocType]  ,	[EWBGenDocNo]                           ,[EWBDocDate]        ,[FromGSTIN]       ,[FromTradeName]  ,[FromAddress1]   ,[FromAddress2]     ,[FromPlace] ," &
        '                 "[FromPINCode]     ,	[FromStateCode] ,[ActualFromStateCode] ,[ToGSTIN]       ,[ToTradeName],[ToAddress1]      ,[ToAddress2]    ,[ToPlace]       ,[ToPINCode]       ,[ToStateCode] , [ActualToStateCode] ," &
        '                 "[TransactionType],[OtherValue]                       ,	[Total_value]       ,	[CGST_Value],[SGST_Value],[IGST_Value]     ,	[CessValue],[CessNonAdvolValue],[TransporterID]    ,[TransporterName]," &
        '                 "[TransportDOCNo] ,[TransportDOCDate]    ,[TotalInvValue]    ,[TransMode]             ," &
        '                 "[VehicleNo]      ,[VehicleType]   , [InvCode], [ShippedToGSTIN], [ShippedToTradeName] ) " &
        '                 " " &
        '                 " " &
        '                 "  SELECT               'O'              , '4'             ,   'JOB WORK'              ,    'CHL'    , a.Weaver_Pavu_Delivery_No ,a.Weaver_Pavu_Delivery_Date          , C.Company_GSTINNo, C.Company_Name, (CASE WHEN a.ReceivedFrom_IdNo <> 0 and a.ReceivedFrom_IdNo <> 4 THEN tREC.Ledger_Address1+tREC.Ledger_Address2 ELSE C.Company_Address1+C.Company_Address2 END) as DispatcherAddress1,  (CASE WHEN a.ReceivedFrom_IdNo <> 0 and a.ReceivedFrom_IdNo <> 4 THEN tREC.Ledger_Address3+tREC.Ledger_Address4 ELSE c.Company_Address3+C.Company_Address4 END ) as Dispatcheraddress2 ,  (CASE WHEN a.ReceivedFrom_IdNo <> 0 and a.ReceivedFrom_IdNo <> 4 THEN tREC.City_Town ELSE C.Company_City END) as Dispatcher_City ," &
        '                 "  (CASE WHEN a.ReceivedFrom_IdNo <> 0 and a.ReceivedFrom_IdNo <> 4 THEN tREC.Pincode ELSE C.Company_PinCode END ) as Dispatcher_Pincode, FS.State_Code  ,  (CASE WHEN a.ReceivedFrom_IdNo <> 0 and a.ReceivedFrom_IdNo <> 4 THEN TRCS.State_Code ELSE FS.State_Code END ) as Dispatcher_Statecode , L.Ledger_GSTINNo, L.Ledger_MainName, (case when a.DeliveryTo_IdNo <> 0 then tDELV.Ledger_Address1+tDELV.Ledger_Address2 else  L.Ledger_Address1+L.Ledger_Address2 end) as deliveryaddress1,  (case when a.DeliveryTo_IdNo <> 0 then tDELV.Ledger_Address3+tDELV.Ledger_Address4 else  L.Ledger_Address3+L.Ledger_Address4 end) as deliveryaddress2, (case when a.DeliveryTo_IdNo <> 0 then tDELV.City_Town else  L.City_Town end) as city_town_name, (case when a.DeliveryTo_IdNo <> 0 then tDELV.Pincode else  L.Pincode end) as pincodee, TS.State_Code, (case when a.DeliveryTo_IdNo <> 0 then TDCS.State_Code else TS.State_Code end) as actual_StateCode," &
        '                 " 1                    , 0 , a.Amount     , 0 ,  0 , 0   ,   0         ,0                   , t.Ledger_GSTINNo  , t.Ledger_Name ," &
        '                 " ''        ,''            ,  0        ,     '1'  AS TrMode ," &
        '                 " a.Vehicle_No, 'R', '" & NewCode & "', tDELV.Ledger_GSTINNo as ShippedTo_GSTIN, tDELV.Ledger_MainName as ShippedTo_LedgerName from Weaver_Pavu_Delivery_Head a inner Join Company_Head C on a.Company_IdNo = C.Company_IdNo " &
        '                 " Inner Join Ledger_Head L ON a.DeliveryTo_IdNo = L.Ledger_IdNo Left Outer Join Ledger_Head tDELV on a.DeliveryTo_IdNo <> 0 and a.DeliveryTo_IdNo = tDELV.Ledger_IdNo " &
        '                 " Left Outer Join Ledger_Head tREC on a.ReceivedFrom_IdNo <> 0 and a.ReceivedFrom_IdNo = tREC.Ledger_IdNo left Outer Join Ledger_Head T on a.Transport_IdNo = T.Ledger_IdNo " &
        '                 " Left Outer Join State_Head FS On C.Company_State_IdNo = fs.State_IdNo left Outer Join State_Head TS on L.Ledger_State_IdNo = TS.State_IdNo  left Outer Join State_Head TDCS on tDELV.Ledger_State_IdNo = TDCS.State_IdNo left Outer Join State_Head TRCS on tREC.Ledger_State_IdNo = TRCS.State_IdNo " &
        '                 " where a.Weaver_Pavu_Delivery_Code = '" & Trim(NewCode) & "'"
        'CMD.ExecuteNonQuery()


        'vSgst = 

        'CMD.CommandText = " Update EWB_Head Set CGST_Value = ( (Total_value * 5 / 100 ) / 2 ) , SGST_Value = ( (Total_value * 5 / 100 ) / 2 ) , TotalInvValue = ( Total_value + SGST_Value + CGST_Value )  where InvCode = '" & Trim(NewCode) & "' "
        'CMD.ExecuteNonQuery()


        'CMD.CommandText = " Update EWB_Head Set TotalInvValue = ( Total_value + SGST_Value + CGST_Value )  where InvCode = '" & Trim(NewCode) & "' "
        'CMD.ExecuteNonQuery()


        CMD.CommandText = "Delete from EWB_Details Where InvCode = '" & NewCode & "'"
        CMD.ExecuteNonQuery()



        'da = New SqlClient.SqlDataAdapter(" Select  I.EndsCount_Name, (I.EndsCount_Name + ' - WARP'  ) , IG.Item_HSN_Code , IG.Item_GST_Percentage , sum(SD.Amount) As TaxableAmt,sum(SD.Total_Meters) as Qty, 1 , 'MTR' AS Units " &
        '                                  " from Weaver_Pavu_Delivery_Head SD Inner Join Weaver_Pavu_Delivery_Details Pd On Pd.Weaver_Pavu_Delivery_Code = Sd.Weaver_Pavu_Delivery_Code  Inner Join EndsCount_Head I On PD.EndsCount_IdNo = I.EndsCount_IdNo INNER JOIN Count_Head Ch On Ch.Count_Idno = I.Count_Idno " &
        '                                  " Inner Join ItemGroup_Head IG on Ch.ItemGroup_IdNo = IG.ItemGroup_IdNo Where SD.Weaver_Pavu_Delivery_Code = '" & Trim(NewCode) & "' Group By " &
        '                                  " I.EndsCount_Name,IG.ItemGroup_Name,IG.Item_HSN_Code, IG.Item_GST_Percentage ", con)


        vTaxable_Amt_Cond = String.Empty
        vQty_Cond = String.Empty
        vUnit_Cond = String.Empty


        ' -------  START DETAILS PART  CONDTION  ------- '


        vTaxable_Amt_Cond = "(case when SD.Rate_for = 'METER' and sum(PD.Meters) <> 0 then sum(PD.Meters * SD.Rate) When SD.Rate_for = 'PAVU' and COUNT(PD.Beam_NO) <> 0 then  COUNT(PD.Beam_NO) * SD.Rate when SD.Rate_for = 'KG' and  sum(SD.Total_pavu_Weight) <> 0 then sum(SD.Total_pavu_Weight * SD.Rate)  else sum(PD.Meters * SD.Rate)  End) "
        vQty_Cond = "(case when SD.Rate_for = 'METER' and sum(PD.Meters) <> 0  then sum(PD.Meters) When SD.Rate_for = 'PAVU' and COUNT(PD.Beam_NO) <> 0 then COUNT(PD.Beam_NO) when SD.Rate_for = 'KG' and  sum(SD.Total_pavu_Weight) <> 0 then sum(SD.Total_pavu_Weight) else sum(PD.Meters) End )"
        vUnit_Cond = "(case when SD.Rate_for = 'METER' then 'MTR' when SD.Rate_for = 'PAVU' then 'NOS' When SD.Rate_for = 'KG' Then 'KGS' Else 'MTR' End)"


        da = New SqlClient.SqlDataAdapter(" Select  I.EndsCount_Name, (I.EndsCount_Name + ' - WARP'  ) , IG.Item_HSN_Code , IG.Item_GST_Percentage , " & vTaxable_Amt_Cond & "  As TaxableAmt , " & vQty_Cond & " as Qty, 1 , " & vUnit_Cond & "  AS Units , tz.Company_State_IdNo , Lh.Ledger_State_Idno  ,SD.Rate_for " &
                                          " from Weaver_Pavu_Delivery_Head SD Inner Join Weaver_Pavu_Delivery_Details Pd On Pd.Weaver_Pavu_Delivery_Code = Sd.Weaver_Pavu_Delivery_Code  Inner Join EndsCount_Head I On PD.EndsCount_IdNo = I.EndsCount_IdNo INNER JOIN Count_Head Ch On Ch.Count_Idno = I.Count_Idno " &
                                          " INNER Join Ledger_Head Lh ON Lh.Ledger_Idno = SD.DeliveryTo_IdNo INNER JOIN Company_Head tz On tz.Company_Idno = SD.Company_Idno  " &
                                         "   Inner Join ItemGroup_Head IG on Ch.ItemGroup_IdNo = IG.ItemGroup_IdNo Where SD.Weaver_Pavu_Delivery_Code = '" & Trim(NewCode) & "' Group By " &
                                          " I.EndsCount_Name,IG.ItemGroup_Name,IG.Item_HSN_Code, IG.Item_GST_Percentage , tz.Company_State_IdNo , Lh.Ledger_State_Idno ,SD.Rate_for ,SD.Rate  ", con)
        dt1 = New DataTable
        da.Fill(dt1)



        Dim vCgst_Amt As String = 0
        Dim vSgst_Amt As String = 0
        Dim vIgst_AMt As String = 0
        Dim vTax_Perc As String = 0

        If dt1.Rows.Count > 0 Then
            For I = 0 To dt1.Rows.Count - 1

                If dt1.Rows(I).Item("Company_State_IdNo") = dt1.Rows(I).Item("Ledger_State_Idno") Then

                    If Val(dt1.Rows(I).Item(3).ToString) <> 0 Then
                        vCgst_Amt = ((dt1.Rows(I).Item(4) * Val(dt1.Rows(I).Item(3).ToString) / 100) / 2)
                        vSgst_Amt = vCgst_Amt
                        vIgst_AMt = 0
                    Else
                        vCgst_Amt = 0
                        vSgst_Amt = 0
                        vIgst_AMt = 0
                    End If
                Else
                    If Val(dt1.Rows(I).Item(3).ToString) <> 0 Then
                        vIgst_AMt = (dt1.Rows(I).Item(4) * Val(dt1.Rows(I).Item(3).ToString) / 100)
                        vCgst_Amt = 0
                        vSgst_Amt = 0
                    Else
                        vIgst_AMt = 0
                        vCgst_Amt = 0
                        vSgst_Amt = 0
                    End If

                End If

                CMD.CommandText = "Insert into EWB_Details ([SlNo]                               , [Product_Name]           ,	[Product_Description]     ,	[HSNCode]                 ,	[Quantity]                                ,[QuantityUnit] ,  Tax_Perc                         ,	[CessRate]         ,	[CessNonAdvol]  ,	[TaxableAmount]               ,InvCode          ,                 Cgst_Value  ,                       Sgst_Value ,                         Igst_Value) " &
                                  " values                 (" & dt1.Rows(I).Item(6).ToString & ",'" & dt1.Rows(I).Item(0) & "'          , '" & dt1.Rows(I).Item(1) & "'     , '" & dt1.Rows(I).Item(2) & "'     , " & dt1.Rows(I).Item(5).ToString & " , '" & dt1.Rows(I).Item(7).ToString & "'  ,   " & dt1.Rows(I).Item(3).ToString & ", 0                  , 0                   ," & dt1.Rows(I).Item(4) & ",'" & NewCode & "',   '" & Str(Val(vCgst_Amt)) & "'        ,   '" & Str(Val(vSgst_Amt)) & "'     , '" & Str(Val(vIgst_AMt)) & "')"

                CMD.ExecuteNonQuery()

            Next
        End If


        ' -------  ENDS DETAILS PART  CONDTION  ------- '



        ' ------ START HEADER PART CONDTION  ----------- '

        vTaxable_Amt_Cond = String.Empty
        vQty_Cond = String.Empty


        vTaxable_Amt_Cond = "(case when SD.Rate_for = 'METER' THEN sum(SD.Pavu_Meters*SD.Rate)  When SD.Rate_for = 'PAVU' THEN sum(SD.Empty_Beam * SD.Rate)   when SD.Rate_for = 'KG' then sum(SD.Total_pavu_Weight * SD.Rate)    else sum(SD.Pavu_Meters*SD.Rate)  End)  "
        vQty_Cond = " (case when SD.Rate_for = 'METER' then sum(SD.Pavu_Meters) When SD.Rate_for = 'PAVU'  then sum(SD.Empty_Beam) when SD.Rate_for = 'KG'  then sum(SD.Total_pavu_Weight) else   sum(SD.Pavu_Meters) End ) "


        da = New SqlClient.SqlDataAdapter(" Select I.EndsCount_Name, (I.EndsCount_Name + ' - WARP'  ) , IG.Item_HSN_Code , IG.Item_GST_Percentage , " & vTaxable_Amt_Cond & " As TaxableAmt,  " & vQty_Cond & " as Qty, 201 as SlNo, " & vUnit_Cond & "  AS Units , tz.Company_State_IdNo , Lh.Ledger_State_Idno  " &
                                          " from Weaver_Pavu_Delivery_Head SD Inner Join EndsCount_Head I On SD.EndsCount_IdNo = I.EndsCount_IdNo INNER JOIN Count_Head Ch On Ch.Count_Idno = I.Count_Idno " &
                                          " INNER Join Ledger_Head Lh ON Lh.Ledger_Idno = SD.DeliveryTo_IdNo INNER JOIN Company_Head tz On tz.Company_Idno = SD.Company_Idno  " &
                                          " Inner Join ItemGroup_Head IG on Ch.ItemGroup_IdNo = IG.ItemGroup_IdNo Where SD.Weaver_Pavu_Delivery_Code = '" & Trim(NewCode) & "' and ( SD.Pavu_Meters > 0 or SD.Empty_Beam > 0 ) Group By " &
                                          " I.EndsCount_Name,IG.ItemGroup_Name,IG.Item_HSN_Code, IG.Item_GST_Percentage,SD.Rate, tz.Company_State_IdNo , Lh.Ledger_State_Idno ,SD.Rate_for ", con)
        dt1 = New DataTable
        da.Fill(dt1)
        If dt1.Rows.Count > 0 Then
            For I = 0 To dt1.Rows.Count - 1
                If dt1.Rows(I).Item("Company_State_IdNo") = dt1.Rows(I).Item("Ledger_State_Idno") Then

                    If Val(dt1.Rows(I).Item(3).ToString) <> 0 Then
                        vCgst_Amt = ((dt1.Rows(I).Item(4) * Val(dt1.Rows(I).Item(3).ToString) / 100) / 2)
                        vSgst_Amt = vCgst_Amt
                        vIgst_AMt = 0
                    Else
                        vCgst_Amt = 0
                        vSgst_Amt = 0
                        vIgst_AMt = 0
                    End If
                Else
                    If Val(dt1.Rows(I).Item(3).ToString) <> 0 Then
                        vIgst_AMt = (dt1.Rows(I).Item(4) * Val(dt1.Rows(I).Item(3).ToString) / 100)
                        vCgst_Amt = 0
                        vSgst_Amt = 0
                    Else
                        vIgst_AMt = 0
                        vCgst_Amt = 0
                        vSgst_Amt = 0
                    End If

                End If
                CMD.CommandText = "Insert into EWB_Details ( [SlNo]                              ,     [Product_Name]           ,	[Product_Description]     ,	[HSNCode]                     ,	[Quantity]                          ,[QuantityUnit] ,  Tax_Perc                           ,	[CessRate]         ,	[CessNonAdvol]  ,	[TaxableAmount]          ,      InvCode      ,                 Cgst_Value  ,                       Sgst_Value ,                         Igst_Value) " &
                                  " values                 ( " & dt1.Rows(I).Item(6).ToString & ", '" & dt1.Rows(I).Item(0) & "', '" & dt1.Rows(I).Item(1) & "', '" & dt1.Rows(I).Item(2) & "', " & dt1.Rows(I).Item(5).ToString & ", '" & dt1.Rows(I).Item(7).ToString & "' , " & dt1.Rows(I).Item(3).ToString & ", 0                  , 0                   ," & dt1.Rows(I).Item(4) & ", '" & NewCode & "' ,   '" & Str(Val(vCgst_Amt)) & "'        ,   '" & Str(Val(vSgst_Amt)) & "'     , '" & Str(Val(vIgst_AMt)) & "')"
                CMD.ExecuteNonQuery()

            Next
        End If




        ' ------ END HEADER PART CONDTION  ----------- '

        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable

        da1 = New SqlClient.SqlDataAdapter(" Select  * from EWB_Details Ewd  Where Ewd.InvCode = '" & Trim(NewCode) & "' and (Ewd.Cgst_Value <> 0 or Ewd.Sgst_Value <> 0 or Ewd.Igst_Value <> 0) ", con)
        dt2 = New DataTable
        da1.Fill(dt2)

        If dt2.Rows.Count > 0 Then

            If dt2.Rows(0).Item("Igst_Value") <> 0 Then

                CMD.CommandText = " Update EWB_Head Set IGST_Value = (select sum(Ed.Igst_Value) from EWB_Details Ed  where Ed.InvCode = '" & Trim(NewCode) & "' and Ed.Igst_Value <> 0) "
                CMD.ExecuteNonQuery()
            Else
                CMD.CommandText = " Update EWB_Head Set CGST_Value = (select sum(Ed.Cgst_Value) from EWB_Details Ed  where Ed.InvCode = '" & Trim(NewCode) & "' and Ed.Cgst_Value <> 0 ) "
                CMD.ExecuteNonQuery()

                CMD.CommandText = " Update EWB_Head Set SGST_Value = (select sum(Ed.Sgst_Value) from EWB_Details Ed where Ed.InvCode = '" & Trim(NewCode) & "' and Ed.Sgst_Value <> 0) "
                CMD.ExecuteNonQuery()
            End If

        End If

        dt2.Clear()



        btn_GENERATEEWB.Enabled = False

        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.GenerateEWB(NewCode, con, rtbEWBResponse, txt_EWBNo, "Weaver_Pavu_Delivery_Head", "EWave_Bill_No", "Weaver_Pavu_Delivery_Code", Pk_Condition)


    End Sub

    Private Sub btn_PRINTEWB_Click(sender As Object, e As EventArgs) Handles btn_PRINTEWB.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.PrintEWB(txt_Ewave_Bill_No.Text, rtbEWBResponse, 0)
    End Sub

    Private Sub btn_CancelEWB_1_Click(sender As Object, e As EventArgs) Handles btn_CancelEWB_1.Click

        'Dim NewCode As String = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Dim NewCode As String = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        Dim c As Integer = MsgBox("Are You Sure To Cancel This EWB ? ", vbYesNo)

        If c = vbNo Then Exit Sub

        con = New SqlClient.SqlConnection(Common_Procedures.Connection_String)
        con.Open()

        Dim ewb As New EWB(Val(lbl_Company.Tag))

        EWB.CancelEWB(txt_Ewave_Bill_No.Text, NewCode, con, rtbEWBResponse, txt_EWBNo, "Weaver_Pavu_Delivery_Head", "EWave_Bill_No", "Weaver_Pavu_Delivery_Code")

    End Sub

    Private Sub btn_Close_EWB_Click(sender As Object, e As EventArgs) Handles btn_Close_EWB.Click
        Grp_EWB.Visible = False
    End Sub
    Private Sub txt_EWBNo_TextChanged(sender As Object, e As EventArgs) Handles txt_EWBNo.TextChanged
        txt_Ewave_Bill_No.Text = txt_EWBNo.Text
    End Sub

    Private Sub btn_Detail_PRINTEWB_Click(sender As Object, e As EventArgs) Handles btn_Detail_PRINTEWB.Click
        Dim ewb As New EWB(Val(lbl_Company.Tag))
        EWB.PrintEWB(txt_Ewave_Bill_No.Text, rtbEWBResponse, 1)
    End Sub
    Private Sub GET_RATEDETAILS()
        Dim vClothRate As String = 0
        vClothRate = 0
        Dim Clo_IdNo As Integer
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As New DataTable


        If Trim(UCase(VEndsCountTag)) <> Trim(UCase(cbo_EndsCount.Text)) Then

            VEndsCountTag = cbo_EndsCount.Text

            Clo_IdNo = Common_Procedures.EndsCount_NameToIdNo(con, cbo_EndsCount.Text)
            txt_rate.Text = ""

            da = New SqlClient.SqlDataAdapter("SELECT RATE, * FROM ENDSCOUNT_HEAD WHERE  ENDSCOUNT_IDNO =  " & Str(Val(Clo_IdNo)) & " ", con)
            dt = New DataTable
            da.Fill(dt)

            If dt.Rows.Count > 0 Then

                txt_rate.Text = dt.Rows(0)("RATE").ToString

            End If

            dt.Clear()


        End If




    End Sub

    Private Sub cbo_EndsCount_LostFocus(sender As Object, e As EventArgs) Handles cbo_EndsCount.LostFocus
        If Trim(UCase(VEndsCountTag)) <> Trim(UCase(cbo_EndsCount.Text)) Then

            GET_RATEDETAILS()

        End If
    End Sub

    Private Sub cbo_ClothName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "cloth_name", "", "(cloth_idno=0)")
        cbo_ClothName.Tag = cbo_ClothName.Text
    End Sub

    Private Sub cbo_ClothName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, txt_Freight, Nothing, "Cloth_Head", "cloth_name", "", "(cloth_idno=0)")
        If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If cbo_WidthType.Visible And cbo_WidthType.Enabled Then
                cbo_WidthType.Focus()
            Else
                txt_rate.Focus()
            End If

        End If
    End Sub

    Private Sub cbo_ClothName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ClothName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "Cloth_Head", "cloth_name", "", "(cloth_idno=0)")

        If Asc(e.KeyChar) = 13 Then

            If Trim(UCase(cbo_ClothName.Text)) <> Trim(UCase(cbo_ClothName.Tag)) Then
                get_CLOTH_CRIMP_Percentage()
            End If

            If cbo_WidthType.Visible And cbo_WidthType.Enabled Then
                cbo_WidthType.Focus()
            Else
                txt_rate.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_ClothName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Common_Procedures.MDI_LedType = "CLOTH"
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ClothName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub


    Private Sub cbo_ClothName_LostFocus(sender As Object, e As EventArgs) Handles cbo_ClothName.LostFocus
        If Trim(UCase(cbo_ClothName.Text)) <> Trim(UCase(cbo_ClothName.Tag)) Then
            get_CLOTH_CRIMP_Percentage()
        End If
    End Sub

    Private Sub get_CLOTH_CRIMP_Percentage()
        Dim vCLO_IDNO As Integer = 0

        If Trim(UCase(cbo_ClothName.Text)) <> Trim(UCase(cbo_ClothName.Tag)) Then
            vCLO_IDNO = Val(Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothName.Text))
            If Val(vCLO_IDNO) <> 0 Then
                txt_CrimpPerc.Text = Common_Procedures.get_FieldValue(con, "Cloth_Head", "Crimp_Percentage", "(Cloth_Idno = " & Str(Val(vCLO_IDNO)) & ")")
            End If
            cbo_ClothName.Tag = cbo_ClothName.Text
        End If

    End Sub

    Private Sub cbo_weaving_job_no_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_weaving_job_no.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_weaving_job_no, Nothing, "Weaving_JobCard_Head", "Weaving_JobCode_forSelection", "", "(Ledger_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then
            cbo_RecForm.Focus()
        End If
    End Sub

    Private Sub cbo_weaving_job_no_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_weaving_job_no.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_weaving_job_no, Nothing, Nothing, "Weaving_JobCard_Head", "Weaving_JobCode_forSelection", "", "(Ledger_IdNo = 0)")

        If (e.KeyCode = 40 And cbo_weaving_job_no.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            cbo_RecForm.Focus()
        End If
        If (e.KeyCode = 38 And cbo_weaving_job_no.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            cbo_DelvAt.Focus()
        End If

    End Sub

    Private Sub cbo_weaving_job_no_GotFocus(sender As Object, e As EventArgs) Handles cbo_weaving_job_no.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Weaving_JobCard_Head", "Weaving_JobCode_forSelection", "", "(Ledger_IdNo = 0)")
    End Sub
    Private Sub Printing_Format3_1414(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer = 0, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0
        Dim cnt As Integer = 0

        PrntCnt = 1


        set_PaperSize_For_PrintDocument1()

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        Exit For
        '    End If
        'Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20
            .Right = 50
            .Top = 10 ' 20
            .Bottom = 40
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

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

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        pFont = New Font("Calibri", 9, FontStyle.Regular)

        NoofItems_PerPage = 5

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 45 : ClArr(2) = 50 : ClArr(3) = 45 : ClArr(4) = 50 : ClArr(5) = 75 : ClArr(6) = 105
        ClArr(7) = 47 : ClArr(8) = 50 : ClArr(9) = 45 : ClArr(10) = 50 : ClArr(11) = 75
        ClArr(12) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11))

        TxtHgt = 17.3  ' 17.5 ' 18.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20





        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_Prev_HeadIndx = prn_HeadIndx

        PrntCnt2ndPageSTS = False
        TpMargin = TMargin

        If vPrnt_2Copy_In_SinglePage = 1 Then
            PCnt = PCnt + 1
            PrntCnt = PrntCnt + 1
        End If

        For PCnt = 1 To PrntCnt

            If vPrnt_2Copy_In_SinglePage = 1 Then

                If PCnt = 1 Then
                    prn_PageNo1 = prn_PageNo

                    prn_DetIndx1 = prn_DetIndx
                    prn_DetSNo1 = prn_DetSNo
                    prn_NoofBmDets1 = prn_NoofBmDets
                    TpMargin = TMargin


                Else

                    prn_PageNo = prn_PageNo1
                    prn_NoofBmDets = prn_NoofBmDets1
                    prn_DetIndx = prn_DetIndx1
                    prn_DetSNo = prn_DetSNo1

                    TpMargin = 560 + TMargin  ' 600 + TMargin

                End If

            End If


            Try

                If prn_HdDt.Rows.Count > 0 Then

                    Printing_Format3_1414_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TpMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                    NoofDets = 0

                    NoofItems_PerPage = 5
                    If Val(vPrnt_2Copy_In_SinglePage) = 1 Then
                        If prn_DetDt.Rows.Count > (2 * NoofItems_PerPage) Then
                            If Trim(Common_Procedures.settings.CustomerCode) = "1037" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1116" Then
                                NoofDets = NoofDets + 1
                                NoofItems_PerPage = 6
                            Else
                                NoofItems_PerPage = 35
                            End If
                        End If
                    End If


                    CurY = CurY - 10

                    'If prn_DetDt.Rows.Count > 0 Or prn_DetMxIndx > 0 Then
                    If prn_DetMxIndx > 0 Then

                        Do While prn_NoofBmDets < prn_DetMxIndx

                            If NoofDets >= NoofItems_PerPage Then

                                If Val(vPrnt_2Copy_In_SinglePage) = 1 Then

                                    If PCnt = 2 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then

                                        NoofDets = NoofDets + 1

                                        CurY = CurY + TxtHgt

                                        Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)


                                        Printing_Format3_1414_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, PCnt, False)

                                        prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                                        e.HasMorePages = True

                                        Return

                                    End If

                                Else

                                    CurY = CurY + TxtHgt

                                    Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)


                                    Printing_Format3_1414_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, PCnt, False)

                                    prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                                    e.HasMorePages = True

                                    Return

                                End If

                            End If

                            prn_DetIndx = prn_DetIndx + 1

                            If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then
                                CurY = CurY + TxtHgt
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 1)), LMargin + 5, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 2)), LMargin + ClArr(1) + 5, CurY, 0, 0, pFont)
                                If Val(prn_DetAr(prn_DetIndx, 3)) <> 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 3))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 5, CurY, 1, 0, pFont)
                                End If

                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 9)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) - 5, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 5, CurY, 0, ClArr(6), pFont,, True)

                                prn_NoofBmDets = prn_NoofBmDets + 1

                            End If

                            If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)) <> 0 Then

                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 1)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 2)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                                If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 3)) <> 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 3))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                                End If

                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 9)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) - 5, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) - 5, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + ClArr(10) + ClArr(11) + 5, CurY, 0, ClArr(12), pFont,, True)

                                prn_NoofBmDets = prn_NoofBmDets + 1

                            End If

                            NoofDets = NoofDets + 1

                        Loop

                    End If


                    Printing_Format3_1414_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, PCnt, True)

                End If

            Catch ex As Exception
                MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try

            If Val(vPrnt_2Copy_In_SinglePage) = 1 Then
                If PCnt = 1 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then
                    If prn_DetDt.Rows.Count > prn_NoofBmDets Then
                        PrntCnt2ndPageSTS = True
                        e.HasMorePages = True
                        cnt = 6
                        Return
                    End If
                End If
            End If

            PrntCnt2ndPageSTS = False

        Next PCnt


LOOP2:

        prn_Count = prn_Count + 1

        If Val(prn_TotCopies) > 1 Then
            If prn_Count < Val(prn_TotCopies) Then

                prn_DetIndx = 0
                'prn_DetSNo = 0
                prn_PageNo = 0
                prn_DetIndx = 0
                prn_NoofBmDets = 0


                e.HasMorePages = True
                Return

            Else
                e.HasMorePages = False

            End If

        Else

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

        End If

    End Sub
    Private Sub Printing_Format3_1414_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim W1 As Single, W2 As Single, N1 As Single, M1 As Single
        Dim Arr(300, 5) As String
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_PANNo As String, Cmp_GSTNo As String
        Dim strWidth As Single = 0
        Dim Entry_Date As Date = Convert.ToDateTime(prn_HdDt.Rows(0).Item("Weaver_Pavu_Delivery_Date"))

        PageNo = PageNo + 1

        CurY = TMargin

        If prn_DetMxIndx > (2 * NoofItems_PerPage) Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        CurY = TMargin
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_PANNo = "" : Cmp_GSTNo = ""

        Dim vADD_BOLD_STS As Boolean = False

        vADD_BOLD_STS = False
        If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")
            Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString)

            Dim vADDR1 As String = ""

            vADDR1 = Trim(prn_HdDt.Rows(0).Item("Company_Address1").ToString)
            vADDR1 = Replace(vADDR1, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) & ",", "")
            vADDR1 = Replace(vADDR1, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")

            Cmp_Add2 = vADDR1 & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

            vADD_BOLD_STS = True

        Else

            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
            Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_PANNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTNo = "GSTIN : " & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_logo_Image").ToString) <> "" Then

            If IsDBNull(prn_HdDt.Rows(0).Item("Company_logo_Image")) = False Then

                Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("Company_logo_Image"), Byte())
                If Not imageData Is Nothing Then
                    Using ms As New MemoryStream(imageData, 0, imageData.Length)
                        ms.Write(imageData, 0, imageData.Length)

                        If imageData.Length > 0 Then

                            '.BackgroundImage = Image.FromStream(ms)

                            ' e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 108, CurY + 10, 90, 90)
                            e.Graphics.DrawImage(DirectCast(Image.FromStream(ms), Drawing.Image), LMargin + 15, CurY + 5, 100, 90)

                        End If

                    End Using

                End If

            End If

        End If
        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Calibri", 18, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight - 2
        If vADD_BOLD_STS = True Then    '------(ie) company division name in 2nd line
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)
            strHeight = TxtHgt

        End If

        If Trim(Cmp_Add2) <> "" Then
            CurY = CurY + strHeight - 1
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        End If

        If Trim(Cmp_PhNo) <> "" Then
            CurY = CurY + TxtHgt - 1
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        End If

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, " STATE : " & Trim(Common_Procedures.State_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("company_State_Idno").ToString))) & "         " & Trim(Cmp_GSTNo) & "          " & Cmp_PANNo, LMargin + 10, CurY, 2, PrintWidth, pFont)

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1234" Then
        '    Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTNo & "   /    " & Cmp_PANNo, LMargin, CurY, 2, PrintWidth, pFont)
        '    CurY = CurY + TxtHgt
        '    Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)

        'Else
        '    CurY = CurY + TxtHgt
        '    Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTNo & "   /    " & Cmp_PANNo, LMargin + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, PageWidth - 10, CurY, 1, PrintWidth, pFont)
        'End If


        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "PAVU DELIVERY TO JOBWORK", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight - 3

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1395" Then '---- SANTHA EXPORTS (SOMANUR)

            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC CODE : 998821 ( Textile manufactring service )", LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        Else

            p1Font = New Font("Calibri", 10, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "( NOT FOR SALE )", LMargin + 10, CurY, 0, 0, p1Font)
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC CODE : 998821 ( Textile manufactring service )", PageWidth - 10, CurY, 1, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "( NOT FOR SALE )", LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        End If


        CurY = CurY + strHeight ' + 3
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            N1 = e.Graphics.MeasureString("TO   : ", pFont).Width
            W1 = e.Graphics.MeasureString("SET NO  :  ", pFont).Width
            W2 = e.Graphics.MeasureString("E-Way Bill No  :", pFont).Width

            'If Common_Procedures.settings.CustomerCode = "1391" Then
            M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7)
            'Else
            '    M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
            'End If


            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 11, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "TO :  M/s. " & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W2 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 14, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Weaver_Pavu_Delivery_No").ToString, LMargin + M1 + W2 + 25, CurY, 0, 0, p1Font)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont,, True)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Weaver_Pavu_Delivery_Date")), "dd-MM-yyyy"), LMargin + M1 + W2 + 25, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont,, True)
            If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Vehicle No", LMargin + M1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + M1 + W2 + 25, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + N1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + N1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + N1 + W1 + 30, CurY, 0, 0, pFont)

            Else

                If Trim(prn_HdDt.Rows(0).Item("Pan_No").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "PAN", LMargin + N1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + N1 + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Pan_No").ToString, LMargin + N1 + W1 + 30, CurY, 0, 0, pFont)
                End If

            End If

            If Trim(prn_HdDt.Rows(0).Item("Ewave_Bill_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "E-Way Bill No", LMargin + M1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Ewave_Bill_No").ToString), LMargin + M1 + W2 + 25, CurY, 0, 0, pFont)
            End If

            If Trim(prn_HdDt.Rows(0).Item("Party_DcNo").ToString) <> "" Then
                CurY = CurY + TxtHgt
                Common_Procedures.Print_To_PrintDocument(e, "Party DcNo", LMargin + M1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Party_DcNo").ToString), LMargin + M1 + W2 + 25, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + M1, LnAr(3), LMargin + M1, LnAr(2))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BEAM", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "MARK", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "ENDS COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 3, CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BEAM", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "MARK", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, 2, ClAr(11), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "ENDS COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, 2, ClAr(12), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


    End Sub

    Private Sub Printing_Format3_1414_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean, ByVal Pcnt As Integer)
        Dim p1Font As Font
        Dim p2Font As Font
        Dim Cmp_Name As String
        Dim I As Integer
        Dim M1 As Single
        Dim Area_Nm As String = ""
        Dim LedAdd1 As String = ""
        Dim LedAdd2 As String = ""
        Dim LedAdd3 As String = ""
        Dim LedAdd4 As String = ""
        Dim W1 As Single, W2 As Single, W3 As Single
        Dim C1 As Single, C2 As Single
        Dim vTxamt As String = 0
        Dim vNtAMt As String = 0
        Dim vTxPerc As String = 0
        Dim vSgst_amt As String = 0
        Dim vCgst_amt As String = 0
        Dim vIgst_amt As String = 0


        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            CurY = CurY + TxtHgt - 10

            If is_LastPage = True Then

                If (prn_DetMxIndx Mod (NoofItems_PerPage * 2)) <= NoofItems_PerPage Then

                    If (Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString) + Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString) + Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString), LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
                    End If
                    If Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 5, CurY, 1, 0, pFont)
                    End If
                    If (Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) + Val(prn_HdDt.Rows(0).Item("Pavu_Meters").ToString)) Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) + Val(prn_HdDt.Rows(0).Item("Pavu_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)
                    End If

                Else

                    If (Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString) + Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString) + Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    End If
                    If Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                    End If

                    If (Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) + Val(prn_HdDt.Rows(0).Item("Pavu_Meters").ToString)) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) + Val(prn_HdDt.Rows(0).Item("Pavu_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11) - 10, CurY, 1, 0, pFont)
                    End If

                End If

            End If


            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 4, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 4, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) + ClAr(11), LnAr(3))


            M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 30
            If Val(prn_HdDt.Rows(0).Item("RecArea").ToString) <> 0 Then
                Area_Nm = Common_Procedures.Area_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("RecArea").ToString))
            ElseIf Trim(prn_HdDt.Rows(0).Item("RecAdd4").ToString) <> "" Then
                LedAdd1 = Trim(prn_HdDt.Rows(0).Item("RecAdd4").ToString & " " & prn_HdDt.Rows(0).Item("RecAdd3").ToString)
            ElseIf Trim(prn_HdDt.Rows(0).Item("RecAdd3").ToString) <> "" Then
                LedAdd2 = Trim(prn_HdDt.Rows(0).Item("RecAdd3").ToString & " " & prn_HdDt.Rows(0).Item("RecAdd2").ToString)
            ElseIf Trim(prn_HdDt.Rows(0).Item("RecAdd2").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("RecAdd1").ToString) <> "" Then
                LedAdd3 = Trim(prn_HdDt.Rows(0).Item("RecAdd2").ToString & " " & prn_HdDt.Rows(0).Item("RecAdd1").ToString)
            End If


            p2Font = New Font("Calibri", 9, FontStyle.Regular)

            W1 = e.Graphics.MeasureString("Received From  :  ", p2Font).Width
            W2 = e.Graphics.MeasureString("CGST @ 2.5% :", p2Font).Width
            W3 = e.Graphics.MeasureString("Value Of Goods :", p2Font).Width

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
            C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 30 + (ClAr(11) / 2)



            vTxPerc = 0
            vCgst_amt = 0
            vSgst_amt = 0
            vIgst_amt = 0


            If Val(prn_HdDt.Rows(0).Item("Company_State_IdNo").ToString) = Val(prn_HdDt.Rows(0).Item("Ledger_State_IdNo").ToString) Then


                If prn_DetDt.Rows.Count > 0 Then
                    vTxPerc = Format(Val(prn_DetDt.Rows(0).Item("item_gst_percentage").ToString) / 2, "############0.00")
                Else
                    'vTxPerc = 5
                    vTxPerc = Format(Val(prn_HdDt.Rows(0).Item("item_gst_percentage").ToString) / 2, "############0.00")
                End If
                vCgst_amt = Format(Val(prn_HdDt.Rows(0).Item("value").ToString) * Val(vTxPerc) / 100, "############0.00")
                vSgst_amt = Format(Val(prn_HdDt.Rows(0).Item("value").ToString) * Val(vTxPerc) / 100, "############0.00")

            Else


                If prn_DetDt.Rows.Count > 0 Then
                    vTxPerc = prn_DetDt.Rows(0).Item("item_gst_percentage").ToString
                Else
                    'vTxPerc = 5
                    vTxPerc = Format(Val(prn_HdDt.Rows(0).Item("item_gst_percentage").ToString), "############0.00")

                End If

                vIgst_amt = Format(Val(prn_HdDt.Rows(0).Item("value").ToString) * Val(vTxPerc) / 100, "############0.00")

            End If

            CurY = CurY + TxtHgt - 15

            Common_Procedures.Print_To_PrintDocument(e, "Received From ", LMargin + 10, CurY, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Receiver_Name").ToString), LMargin + W1 + 30, CurY, 0, 0, p2Font)


            If Val(prn_HdDt.Rows(0).Item("Rate").ToString) <> 0 Then

                Dim vRATEFOR As String = ""

                If Trim(prn_HdDt.Rows(0).Item("Rate_for").ToString) <> "" Then
                    vRATEFOR = "Rate/" & Trim(StrConv(prn_HdDt.Rows(0).Item("Rate_for").ToString, VbStrConv.ProperCase))
                Else
                    vRATEFOR = "Rate/Mtr"
                End If

                Common_Procedures.Print_To_PrintDocument(e, vRATEFOR, LMargin + C1, CurY, 0, 0, p2Font)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W2, CurY, 0, 0, p2Font)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Rate").ToString), "##########0.00"), LMargin + C1 + W2 + 70, CurY, 1, 0, p2Font)

            End If

            If Val(prn_HdDt.Rows(0).Item("Value").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Taxable Amount", LMargin + C2, CurY, 0, 0, p2Font)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W3, CurY, 0, 0, p2Font)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Value").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, p2Font)
            End If



            CurY = CurY + TxtHgt
            If Trim(Area_Nm) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Area_Nm, LMargin + 30, CurY, 0, 0, p2Font)
            ElseIf Trim(LedAdd1) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, LedAdd1, LMargin + 30, CurY, 0, 0, p2Font)
            ElseIf Trim(LedAdd2) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, LedAdd2, LMargin + 30, CurY, 0, 0, p2Font)
            ElseIf Trim(LedAdd3) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, LedAdd3, LMargin + 30, CurY, 0, 0, p2Font)
            End If

            If Val(prn_HdDt.Rows(0).Item("Value").ToString) <> 0 Then

                If Val(vIgst_amt) <> 0 Then

                    Common_Procedures.Print_To_PrintDocument(e, "IGST @ " & Val(vTxPerc) & " %", LMargin + C2, CurY, 0, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W3, CurY, 0, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(vIgst_amt), "##########0.00"), PageWidth - 10, CurY, 1, 0, p2Font)

                Else

                    Common_Procedures.Print_To_PrintDocument(e, "CGST @ " & Val(vTxPerc) & " %", LMargin + C1, CurY, 0, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W2, CurY, 0, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(vCgst_amt), "##########0.00"), LMargin + C1 + W2 + 70, CurY, 1, 0, p2Font)

                    Common_Procedures.Print_To_PrintDocument(e, "SGST @ " & Val(vTxPerc) & " %", LMargin + C2, CurY, 0, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W3, CurY, 0, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(vSgst_amt), "##########0.00"), PageWidth - 10, CurY, 1, 0, p2Font)

                End If

            End If


            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Transport_IdNo").ToString) <> 0 Then
                If Common_Procedures.settings.CustomerCode = "1391" Then
                    Common_Procedures.Print_To_PrintDocument(e, "Sizing Name ", LMargin + 10, CurY, 0, 0, p2Font)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "Transport Name ", LMargin + 10, CurY, 0, 0, p2Font)
                End If
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, p2Font)
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Transport_IdNo").ToString)), LMargin + W1 + 30, CurY, 0, 0, p2Font)
            End If

            If Val(prn_HdDt.Rows(0).Item("Value").ToString) <> 0 Then
                vNtAMt = Format(Val(prn_HdDt.Rows(0).Item("Value").ToString) + Val(vCgst_amt) + Val(vSgst_amt) + Val(vIgst_amt), "############0")

                Common_Procedures.Print_To_PrintDocument(e, "Value Of Goods", LMargin + C2, CurY, 0, 0, p2Font)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W3, CurY, 0, 0, p2Font)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(vNtAMt), "##########0.00"), PageWidth - 10, CurY, 1, 0, p2Font)
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            Common_Procedures.Print_To_PrintDocument(e, "Received The Beams As Per Above Details.", LMargin + 10, CurY + 5, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If Common_Procedures.settings.CustomerCode <> "1391" Then
                p1Font = New Font("Calibri", 8, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 300, CurY, 0, 0, p1Font)
            End If

            ' End If

            CurY = CurY + TxtHgt

            CurY = CurY + TxtHgt + 5

            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
                Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")
            End If

            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 300, CurY, 0, 0, pFont)
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 5, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
            e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub
    Private Sub cbo_Sales_OrderCode_forSelection_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_Sales_OrderCode_forSelection.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, cbo_DelvAt, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")
    End Sub

    Private Sub cbo_Sales_OrderCode_forSelection_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_Sales_OrderCode_forSelection.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, txt_DateTime_Of_Supply, cbo_DelvAt, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")
    End Sub

    Private Sub cbo_Sales_OrderCode_forSelection_Enter(sender As Object, e As EventArgs) Handles cbo_Sales_OrderCode_forSelection.Enter
        vCLO_CONDT = "(ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode1) & "' or ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode2) & "' and Order_Close_Status = 0 )"
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")
    End Sub

    Private Sub txt_DateTime_Of_Supply_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_DateTime_Of_Supply.KeyDown
        If e.KeyCode = 38 Then
            cbo_TransportMode.Focus()
        ElseIf e.KeyCode = 40 Then
            If cbo_Sales_OrderCode_forSelection.Visible And cbo_Sales_OrderCode_forSelection.Enabled Then
                cbo_Sales_OrderCode_forSelection.Focus()
            Else
                txt_place_Supply.Focus()
            End If

        End If
    End Sub
    Private Sub txt_DateTime_Of_Supply_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_DateTime_Of_Supply.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If cbo_Sales_OrderCode_forSelection.Visible And cbo_Sales_OrderCode_forSelection.Enabled Then
                cbo_Sales_OrderCode_forSelection.Focus()
            ElseIf cbo_DelvAt.Visible And cbo_DelvAt.Enabled Then
                cbo_DelvAt.Focus()
            ElseIf cbo_RecForm.Visible And cbo_RecForm.Enabled Then
                cbo_RecForm.Focus()
            ElseIf cbo_EndsCount.Visible And cbo_EndsCount.Enabled Then
                cbo_EndsCount.Focus()
            Else
                txt_place_Supply.Focus()
            End If
        End If
    End Sub


    Private Sub Printing_Format1087(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
        Dim EntryCode As String
        Dim I As Integer = 0, NoofDets As Integer, NoofItems_PerPage As Integer
        Dim pFont As Font
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        Dim CurY As Single, TxtHgt As Single
        Dim LnAr(15) As Single, ClArr(15) As Single
        Dim strHeight As Single = 0
        Dim PpSzSTS As Boolean = False
        Dim PCnt As Integer = 0, PrntCnt As Integer = 0
        Dim TpMargin As Single = 0
        Dim cnt As Integer = 0

        PrntCnt = 1


        set_PaperSize_For_PrintDocument1()

        'For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
        '    If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
        '        ps = PrintDocument1.PrinterSettings.PaperSizes(I)
        '        PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
        '        PrintDocument1.DefaultPageSettings.PaperSize = ps
        '        e.PageSettings.PaperSize = ps
        '        Exit For
        '    End If
        'Next

        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 20
            .Right = 50
            .Top = 10 ' 20
            .Bottom = 40
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

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

        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        pFont = New Font("Arial", 8, FontStyle.Regular)

        NoofItems_PerPage = 5

        Erase LnAr
        Erase ClArr

        LnAr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        ClArr = New Single(15) {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ClArr(1) = 65 : ClArr(2) = 55 : ClArr(3) = 50 : ClArr(4) = 75 : ClArr(5) = 120
        ClArr(6) = 65 : ClArr(7) = 55 : ClArr(8) = 50 : ClArr(9) = 75
        ClArr(10) = PageWidth - (LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9))

        TxtHgt = 17.3  ' 17.5 ' 18.5 ' e.Graphics.MeasureString("A", pFont).Height  ' 20



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


        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_DcNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_Prev_HeadIndx = prn_HeadIndx

        PrntCnt2ndPageSTS = False
        TpMargin = TMargin

        If vPrnt_2Copy_In_SinglePage = 1 Then
            PCnt = PCnt + 1
            PrntCnt = PrntCnt + 1
        End If

        For PCnt = 1 To PrntCnt

            If vPrnt_2Copy_In_SinglePage = 1 Then

                If PCnt = 1 Then
                    prn_PageNo1 = prn_PageNo

                    prn_DetIndx1 = prn_DetIndx
                    prn_DetSNo1 = prn_DetSNo
                    prn_NoofBmDets1 = prn_NoofBmDets
                    TpMargin = TMargin


                Else

                    prn_PageNo = prn_PageNo1
                    prn_NoofBmDets = prn_NoofBmDets1
                    prn_DetIndx = prn_DetIndx1
                    prn_DetSNo = prn_DetSNo1

                    TpMargin = 560 + TMargin  ' 600 + TMargin

                End If

            End If


            Try

                If prn_HdDt.Rows.Count > 0 Then

                    Printing_Format1087_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TpMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClArr)

                    NoofDets = 0

                    NoofItems_PerPage = 5
                    If Val(vPrnt_2Copy_In_SinglePage) = 1 Then
                        If prn_DetDt.Rows.Count > (2 * NoofItems_PerPage) Then
                            If Trim(Common_Procedures.settings.CustomerCode) = "1037" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1116" Then
                                NoofDets = NoofDets + 1
                                NoofItems_PerPage = 6
                            Else
                                NoofItems_PerPage = 35
                            End If
                        End If
                    End If


                    CurY = CurY - 10

                    'If prn_DetDt.Rows.Count > 0 Or prn_DetMxIndx > 0 Then
                    If prn_DetMxIndx > 0 Then

                        Do While prn_NoofBmDets < prn_DetMxIndx

                            If NoofDets >= NoofItems_PerPage Then

                                If Val(vPrnt_2Copy_In_SinglePage) = 1 Then

                                    If PCnt = 2 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then

                                        NoofDets = NoofDets + 1

                                        CurY = CurY + TxtHgt

                                        Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)


                                        Printing_Format1087_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, PCnt, False)

                                        prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                                        e.HasMorePages = True

                                        Return

                                    End If

                                Else

                                    CurY = CurY + TxtHgt

                                    Common_Procedures.Print_To_PrintDocument(e, "Continued....", PageWidth - 10, CurY, 1, 0, pFont)


                                    Printing_Format1087_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, PCnt, False)

                                    prn_DetIndx = prn_DetIndx + NoofItems_PerPage

                                    e.HasMorePages = True

                                    Return

                                End If

                            End If

                            prn_DetIndx = prn_DetIndx + 1

                            If Val(prn_DetAr(prn_DetIndx, 4)) <> 0 Then
                                CurY = CurY + TxtHgt
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 1)), LMargin + 10, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 2)), LMargin + ClArr(1) + 10, CurY, 0, 0, pFont)
                                If Val(prn_DetAr(prn_DetIndx, 3)) <> 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx, 3))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) - 10, CurY, 1, 0, pFont)
                                End If
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + 5, CurY, 0, ClArr(5), pFont,, True)

                                prn_NoofBmDets = prn_NoofBmDets + 1

                            End If

                            If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)) <> 0 Then

                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 1)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + 12, CurY, 0, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 2)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + 10, CurY, 0, 0, pFont)
                                If Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 3)) <> 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, Trim(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 3))), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) - 10, CurY, 1, 0, pFont)
                                End If

                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 4)), "#########0.00"), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) - 10, CurY, 1, 0, pFont)
                                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_DetAr(prn_DetIndx + NoofItems_PerPage, 5)), LMargin + ClArr(1) + ClArr(2) + ClArr(3) + ClArr(4) + ClArr(5) + ClArr(6) + ClArr(7) + ClArr(8) + ClArr(9) + 5, CurY, 0, ClArr(5), pFont,, True)

                                prn_NoofBmDets = prn_NoofBmDets + 1

                            End If

                            NoofDets = NoofDets + 1

                        Loop

                    End If


                    Printing_Format1087_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClArr, NoofDets, PCnt, True)

                End If

            Catch ex As Exception
                MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End Try

            If Val(vPrnt_2Copy_In_SinglePage) = 1 Then
                If PCnt = 1 And PrntCnt = 2 And PrntCnt2ndPageSTS = False Then
                    If prn_DetDt.Rows.Count > prn_NoofBmDets Then
                        PrntCnt2ndPageSTS = True
                        e.HasMorePages = True
                        cnt = 6
                        Return
                    End If
                End If
            End If

            PrntCnt2ndPageSTS = False

        Next PCnt


LOOP2:

        prn_Count = prn_Count + 1

        If Val(prn_TotCopies) > 1 Then
            If prn_Count < Val(prn_TotCopies) Then

                prn_DetIndx = 0
                'prn_DetSNo = 0
                prn_PageNo = 0
                prn_DetIndx = 0
                prn_NoofBmDets = 0


                e.HasMorePages = True
                Return

            Else
                e.HasMorePages = False

            End If

        Else

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

        End If

    End Sub

    Private Sub Printing_Format1087_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim p1Font As Font
        Dim strHeight As Single
        Dim W1 As Single, W2 As Single, N1 As Single, M1 As Single
        Dim Arr(300, 5) As String
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_PANNo As String, Cmp_GSTNo As String
        Dim strWidth As Single = 0
        Dim Entry_Date As Date = Convert.ToDateTime(prn_HdDt.Rows(0).Item("Weaver_Pavu_Delivery_Date"))

        PageNo = PageNo + 1

        CurY = TMargin

        If prn_DetMxIndx > (2 * NoofItems_PerPage) Then
            Common_Procedures.Print_To_PrintDocument(e, "Page : " & Trim(Val(PageNo)), PageWidth - 10, CurY - TxtHgt, 1, 0, pFont)
        End If

        CurY = TMargin
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_PANNo = "" : Cmp_GSTNo = ""

        Dim vADD_BOLD_STS As Boolean = False

        vADD_BOLD_STS = False
        If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")
            Cmp_Add1 = Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString)

            Dim vADDR1 As String = ""

            vADDR1 = Trim(prn_HdDt.Rows(0).Item("Company_Address1").ToString)
            vADDR1 = Replace(vADDR1, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) & ",", "")
            vADDR1 = Replace(vADDR1, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")

            Cmp_Add2 = vADDR1 & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

            vADD_BOLD_STS = True

        Else

            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            Cmp_Add1 = prn_HdDt.Rows(0).Item("Company_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address2").ToString
            Cmp_Add2 = prn_HdDt.Rows(0).Item("Company_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Company_Address4").ToString

        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString) <> "" Then
            Cmp_PhNo = "PHONE NO.:" & prn_HdDt.Rows(0).Item("Company_PhoneNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_PanNo").ToString) <> "" Then
            Cmp_PANNo = "PAN NO.: " & prn_HdDt.Rows(0).Item("Company_PanNo").ToString
        End If
        If Trim(prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString) <> "" Then
            Cmp_GSTNo = "GSTIN : " & prn_HdDt.Rows(0).Item("Company_GSTinNo").ToString
        End If

        If Trim(prn_HdDt.Rows(0).Item("Company_logo_Image").ToString) <> "" Then

            If IsDBNull(prn_HdDt.Rows(0).Item("Company_logo_Image")) = False Then

                Dim imageData As Byte() = DirectCast(prn_HdDt.Rows(0).Item("Company_logo_Image"), Byte())
                If Not imageData Is Nothing Then
                    Using ms As New MemoryStream(imageData, 0, imageData.Length)
                        ms.Write(imageData, 0, imageData.Length)

                        If imageData.Length > 0 Then

                            '.BackgroundImage = Image.FromStream(ms)

                            ' e.Graphics.DrawImage(DirectCast(pic_IRN_QRCode_Image_forPrinting.BackgroundImage, Drawing.Image), PageWidth - 108, CurY + 10, 90, 90)
                            e.Graphics.DrawImage(DirectCast(Image.FromStream(ms), Drawing.Image), LMargin + 5, CurY + 5, 100, 100)

                        End If

                    End Using

                End If

            End If

        End If
        CurY = CurY + TxtHgt - 10
        p1Font = New Font("Americana Std", 20, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, StrConv(Cmp_Name, VbStrConv.ProperCase), LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight - 2
        If vADD_BOLD_STS = True Then    '------(ie) company division name in 2nd line
            p1Font = New Font("Arial", 8, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        Else
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, pFont)
            strHeight = TxtHgt

        End If

        If Trim(Cmp_Add2) <> "" Then
            CurY = CurY + strHeight - 1
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, pFont)
        End If

        If Trim(Cmp_PhNo) <> "" Then
            CurY = CurY + TxtHgt - 1
            Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)
        End If

        CurY = CurY + TxtHgt - 1
        Common_Procedures.Print_To_PrintDocument(e, " STATE : " & Trim(Common_Procedures.State_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("company_State_Idno").ToString))) & "         " & Trim(Cmp_GSTNo) & "          " & Cmp_PANNo, LMargin + 10, CurY, 2, PrintWidth, pFont)

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1234" Then
        '    Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTNo & "   /    " & Cmp_PANNo, LMargin, CurY, 2, PrintWidth, pFont)
        '    CurY = CurY + TxtHgt
        '    Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, pFont)

        'Else
        '    CurY = CurY + TxtHgt
        '    Common_Procedures.Print_To_PrintDocument(e, Cmp_GSTNo & "   /    " & Cmp_PANNo, LMargin + 10, CurY, 0, 0, pFont)
        '    Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, PageWidth - 10, CurY, 1, PrintWidth, pFont)
        'End If


        CurY = CurY + TxtHgt - 5
        p1Font = New Font("Arial", 8, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "PAVU DELIVERY TO JOBWORK", LMargin, CurY, 2, PrintWidth, p1Font)
        strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        CurY = CurY + strHeight - 3

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1395" Then '---- SANTHA EXPORTS (SOMANUR)

            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC CODE : 998821 ( Textile manufactring service )", LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        Else

            p1Font = New Font("Arial", 8, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "( NOT FOR SALE )", LMargin + 10, CurY, 0, 0, p1Font)
            p1Font = New Font("Arial", 8, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "HSN/SAC CODE : 998821 ( Textile manufactring service )", PageWidth - 10, CurY, 1, 0, p1Font)
            'Common_Procedures.Print_To_PrintDocument(e, "( NOT FOR SALE )", LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

        End If


        CurY = CurY + strHeight ' + 3
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(2) = CurY

        Try

            N1 = e.Graphics.MeasureString("TO   : ", pFont).Width
            W1 = e.Graphics.MeasureString("SET NO  :  ", pFont).Width
            W2 = e.Graphics.MeasureString("E-Way Bill No  :", pFont).Width

            'If Common_Procedures.settings.CustomerCode = "1391" Then
            M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7)
            'Else
            '    M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
            'End If


            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Arial", 8, FontStyle.Bold)

            Common_Procedures.Print_To_PrintDocument(e, "TO :  M/s. " & prn_HdDt.Rows(0).Item("Ledger_MainName").ToString, LMargin + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, "DC.NO", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W2 + 10, CurY, 0, 0, pFont)
            p1Font = New Font("Arial", 8, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Weaver_Pavu_Delivery_No").ToString, LMargin + M1 + W2 + 25, CurY, 0, 0, p1Font)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address1").ToString & " " & prn_HdDt.Rows(0).Item("Ledger_Address2").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont,, True)
            Common_Procedures.Print_To_PrintDocument(e, "DATE", LMargin + M1 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W2 + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_HdDt.Rows(0).Item("Weaver_Pavu_Delivery_Date")), "dd-MM-yyyy"), LMargin + M1 + W2 + 25, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(0).Item("Ledger_Address3").ToString & " " & prn_HdDt.Rows(0).Item("Ledger_Address4").ToString, LMargin + N1 + 10, CurY, 0, 0, pFont,, True)
            If Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "Vehicle No", LMargin + M1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Vehicle_No").ToString), LMargin + M1 + W2 + 25, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt
            If Trim(prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "GSTIN", LMargin + N1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + N1 + W1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Ledger_GSTinNo").ToString, LMargin + N1 + W1 + 30, CurY, 0, 0, pFont)

            Else

                If Trim(prn_HdDt.Rows(0).Item("Pan_No").ToString) <> "" Then
                    Common_Procedures.Print_To_PrintDocument(e, "PAN", LMargin + N1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + N1 + W1 + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " " & prn_HdDt.Rows(0).Item("Pan_No").ToString, LMargin + N1 + W1 + 30, CurY, 0, 0, pFont)
                End If

            End If

            If Trim(prn_HdDt.Rows(0).Item("Ewave_Bill_No").ToString) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, "E-Way Bill No", LMargin + M1 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + M1 + W2 + 10, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Ewave_Bill_No").ToString), LMargin + M1 + W2 + 25, CurY, 0, 0, pFont)
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + M1, LnAr(3), LMargin + M1, LnAr(2))

            CurY = CurY + TxtHgt - 10
            Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin, CurY, 2, ClAr(1), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BEAM", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "ENDS COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "SET NO", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 3, CurY, 2, ClAr(6), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "BEAM", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "PCS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "METERS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pFont)
            Common_Procedures.Print_To_PrintDocument(e, "ENDS COUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pFont)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(4) = CurY

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try


    End Sub

    Private Sub Printing_Format1087_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean, ByVal Pcnt As Integer)
        Dim p1Font As Font
        Dim p2Font As Font
        Dim Cmp_Name As String
        Dim I As Integer
        Dim M1 As Single
        Dim Area_Nm As String = ""
        Dim LedAdd1 As String = ""
        Dim LedAdd2 As String = ""
        Dim LedAdd3 As String = ""
        Dim LedAdd4 As String = ""
        Dim W1 As Single, W2 As Single, W3 As Single
        Dim C1 As Single, C2 As Single
        Dim vTxamt As String = 0
        Dim vNtAMt As String = 0
        Dim vTxPerc As String = 0
        Dim vSgst_amt As String = 0
        Dim vCgst_amt As String = 0
        Dim vIgst_amt As String = 0


        Try

            For I = NoofDets + 1 To NoofItems_PerPage
                CurY = CurY + TxtHgt
            Next

            CurY = CurY + TxtHgt
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))

            CurY = CurY + TxtHgt - 10

            If is_LastPage = True Then

                If (prn_DetMxIndx Mod (NoofItems_PerPage * 2)) <= NoofItems_PerPage Then

                    If (Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString) + Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString) + Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString), LMargin + ClAr(1) + ClAr(2) - 10, CurY, 1, 0, pFont)
                    End If
                    If Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 10, CurY, 1, 0, pFont)
                    End If
                    If (Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) + Val(prn_HdDt.Rows(0).Item("Pavu_Meters").ToString)) Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) + Val(prn_HdDt.Rows(0).Item("Pavu_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 10, CurY, 1, 0, pFont)
                    End If

                Else

                    If (Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString) + Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString)) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Beam").ToString) + Val(prn_HdDt.Rows(0).Item("Empty_Beam").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 10, CurY, 1, 0, pFont)
                    End If
                    If Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(0).Item("Total_Pcs").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 10, CurY, 1, 0, pFont)
                    End If

                    If (Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) + Val(prn_HdDt.Rows(0).Item("Pavu_Meters").ToString)) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Total_Meters").ToString) + Val(prn_HdDt.Rows(0).Item("Pavu_Meters").ToString), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 10, CurY, 1, 0, pFont)
                    End If

                End If

            End If


            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(6) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 4, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 4, LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(3))

            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + 20, CurY, LMargin + ClAr(1) + ClAr(2) + 20, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 5, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 5, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 5, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 5, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 50, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + 50, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 50, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 50, LnAr(3))
            'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + 50, CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + 50, LnAr(3))

            M1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 30
            If Val(prn_HdDt.Rows(0).Item("RecArea").ToString) <> 0 Then
                Area_Nm = Common_Procedures.Area_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("RecArea").ToString))
            ElseIf Trim(prn_HdDt.Rows(0).Item("RecAdd4").ToString) <> "" Then
                LedAdd1 = Trim(prn_HdDt.Rows(0).Item("RecAdd4").ToString & " " & prn_HdDt.Rows(0).Item("RecAdd3").ToString)
            ElseIf Trim(prn_HdDt.Rows(0).Item("RecAdd3").ToString) <> "" Then
                LedAdd2 = Trim(prn_HdDt.Rows(0).Item("RecAdd3").ToString & " " & prn_HdDt.Rows(0).Item("RecAdd2").ToString)
            ElseIf Trim(prn_HdDt.Rows(0).Item("RecAdd2").ToString) <> "" Or Trim(prn_HdDt.Rows(0).Item("RecAdd1").ToString) <> "" Then
                LedAdd3 = Trim(prn_HdDt.Rows(0).Item("RecAdd2").ToString & " " & prn_HdDt.Rows(0).Item("RecAdd1").ToString)
            End If


            p2Font = New Font("Arial", 9, FontStyle.Regular)

            W1 = e.Graphics.MeasureString("Received From  :  ", p2Font).Width
            W2 = e.Graphics.MeasureString("CGST @ 2.5% :", p2Font).Width
            W3 = e.Graphics.MeasureString("Value Of Goods :", p2Font).Width

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5)
            C2 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + (ClAr(9) / 2)



            vTxPerc = 0
            vCgst_amt = 0
            vSgst_amt = 0
            vIgst_amt = 0


            If Val(prn_HdDt.Rows(0).Item("Company_State_IdNo").ToString) = Val(prn_HdDt.Rows(0).Item("Ledger_State_IdNo").ToString) Then


                If prn_DetDt.Rows.Count > 0 Then
                    vTxPerc = Format(Val(prn_DetDt.Rows(0).Item("item_gst_percentage").ToString) / 2, "############0.00")
                Else
                    'vTxPerc = 5
                    vTxPerc = Format(Val(prn_HdDt.Rows(0).Item("item_gst_percentage").ToString) / 2, "############0.00")
                End If
                vCgst_amt = Format(Val(prn_HdDt.Rows(0).Item("value").ToString) * Val(vTxPerc) / 100, "############0.00")
                vSgst_amt = Format(Val(prn_HdDt.Rows(0).Item("value").ToString) * Val(vTxPerc) / 100, "############0.00")

            Else


                If prn_DetDt.Rows.Count > 0 Then
                    vTxPerc = prn_DetDt.Rows(0).Item("item_gst_percentage").ToString
                Else
                    'vTxPerc = 5
                    vTxPerc = Format(Val(prn_HdDt.Rows(0).Item("item_gst_percentage").ToString), "############0.00")

                End If

                vIgst_amt = Format(Val(prn_HdDt.Rows(0).Item("value").ToString) * Val(vTxPerc) / 100, "############0.00")

            End If

            CurY = CurY + TxtHgt - 15

            Common_Procedures.Print_To_PrintDocument(e, "Received From ", LMargin + 10, CurY, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, p2Font)
            Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(0).Item("Receiver_Name").ToString), LMargin + W1 + 30, CurY, 0, 0, p2Font)


            If Val(prn_HdDt.Rows(0).Item("Rate").ToString) <> 0 Then

                Dim vRATEFOR As String = ""

                If Trim(prn_HdDt.Rows(0).Item("Rate_for").ToString) <> "" Then
                    vRATEFOR = "Rate/" & Trim(StrConv(prn_HdDt.Rows(0).Item("Rate_for").ToString, VbStrConv.ProperCase))
                Else
                    vRATEFOR = "Rate/Mtr"
                End If

                Common_Procedures.Print_To_PrintDocument(e, vRATEFOR, LMargin + C1, CurY, 0, 0, p2Font)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W2, CurY, 0, 0, p2Font)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Rate").ToString), "##########0.00"), LMargin + C1 + W2 + 70, CurY, 1, 0, p2Font)

            End If

            If Val(prn_HdDt.Rows(0).Item("Value").ToString) <> 0 Then
                Common_Procedures.Print_To_PrintDocument(e, "Taxable Amount", LMargin + C2, CurY, 0, 0, p2Font)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W3, CurY, 0, 0, p2Font)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_HdDt.Rows(0).Item("Value").ToString), "##########0.00"), PageWidth - 10, CurY, 1, 0, p2Font)
            End If



            CurY = CurY + TxtHgt
            If Trim(Area_Nm) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, Area_Nm, LMargin + 30, CurY, 0, 0, p2Font)
            ElseIf Trim(LedAdd1) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, LedAdd1, LMargin + 30, CurY, 0, 0, p2Font)
            ElseIf Trim(LedAdd2) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, LedAdd2, LMargin + 30, CurY, 0, 0, p2Font)
            ElseIf Trim(LedAdd3) <> "" Then
                Common_Procedures.Print_To_PrintDocument(e, LedAdd3, LMargin + 30, CurY, 0, 0, p2Font)
            End If

            If Val(prn_HdDt.Rows(0).Item("Value").ToString) <> 0 Then

                If Val(vIgst_amt) <> 0 Then

                    Common_Procedures.Print_To_PrintDocument(e, "IGST @ " & Val(vTxPerc) & " %", LMargin + C2, CurY, 0, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W3, CurY, 0, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(vIgst_amt), "##########0.00"), PageWidth - 10, CurY, 1, 0, p2Font)

                Else

                    Common_Procedures.Print_To_PrintDocument(e, "CGST @ " & Val(vTxPerc) & " %", LMargin + C1, CurY, 0, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C1 + W2, CurY, 0, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(vCgst_amt), "##########0.00"), LMargin + C1 + W2 + 70, CurY, 1, 0, p2Font)

                    Common_Procedures.Print_To_PrintDocument(e, "SGST @ " & Val(vTxPerc) & " %", LMargin + C2, CurY, 0, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W3, CurY, 0, 0, p2Font)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(vSgst_amt), "##########0.00"), PageWidth - 10, CurY, 1, 0, p2Font)

                End If

            End If


            CurY = CurY + TxtHgt
            If Val(prn_HdDt.Rows(0).Item("Transport_IdNo").ToString) <> 0 Then
                If Common_Procedures.settings.CustomerCode = "1391" Then
                    Common_Procedures.Print_To_PrintDocument(e, "Sizing Name ", LMargin + 10, CurY, 0, 0, p2Font)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "Transport Name ", LMargin + 10, CurY, 0, 0, p2Font)
                End If
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + W1 + 10, CurY, 0, 0, p2Font)
                Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Ledger_IdNoToName(con, Val(prn_HdDt.Rows(0).Item("Transport_IdNo").ToString)), LMargin + W1 + 30, CurY, 0, 0, p2Font)
            End If

            If Val(prn_HdDt.Rows(0).Item("Value").ToString) <> 0 Then
                vNtAMt = Format(Val(prn_HdDt.Rows(0).Item("Value").ToString) + Val(vCgst_amt) + Val(vSgst_amt) + Val(vIgst_amt), "############0")

                Common_Procedures.Print_To_PrintDocument(e, "Value Of Goods", LMargin + C2, CurY, 0, 0, p2Font)
                Common_Procedures.Print_To_PrintDocument(e, ":", LMargin + C2 + W3, CurY, 0, 0, p2Font)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(vNtAMt), "##########0.00"), PageWidth - 10, CurY, 1, 0, p2Font)
            End If

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(7) = CurY

            Common_Procedures.Print_To_PrintDocument(e, "Received The Beams As Per Above Details.", LMargin + 10, CurY + 5, 0, 0, pFont)

            CurY = CurY + TxtHgt
            If Common_Procedures.settings.CustomerCode <> "1391" Then
                p1Font = New Font("Arial", 8, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 300, CurY, 0, 0, p1Font)
            End If

            ' End If

            CurY = CurY + TxtHgt

            CurY = CurY + TxtHgt + 5

            Cmp_Name = prn_HdDt.Rows(0).Item("Company_Name").ToString
            If Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString) <> "" Then
                Cmp_Name = Replace(Cmp_Name, Trim(prn_HdDt.Rows(0).Item("Company_Division_Name").ToString), "")
            End If

            Common_Procedures.Print_To_PrintDocument(e, "Receiver's Signature", LMargin + 10, CurY, 0, 0, pFont)
            Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 300, CurY, 0, 0, pFont)
            p1Font = New Font("Arial", 8, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 5, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, LMargin, LnAr(1))
            e.Graphics.DrawLine(Pens.Black, PageWidth, CurY, PageWidth, LnAr(1))

        Catch ex As Exception

            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub


End Class
