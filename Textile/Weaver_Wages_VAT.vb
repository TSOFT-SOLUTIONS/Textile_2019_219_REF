Public Class Weaver_Wages_VAT
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Filter_RowNo As Integer = -1
    Private Pk_Condition As String = "WVWAG-"
    Private Pk_Condition2 As String = "WWAGL-"
    Private PkCondition_WPTDS As String = "WPTDS-"
    Private PkCondition_WPYMT As String = "WPYMT-"
    Private PkCondition_WCLRC As String = "WCLRC-"
    Private PkCondition_WADVP As String = "WADVP-"
    Private PkCondition_WADVD As String = "WADVD-"
    Private PkCondition_WFRGT As String = "WWFRG-"
    Private PkCondition_GST As String = "GWWAG-"

    Private NoCalc_Status As Boolean = False

    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private SaveAll_STS As Boolean = False
    Private LastNo As String = ""

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_HeadIndx As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_PageSize_SetUP_STS As Boolean
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private prn_FromNo As String
    Private prn_ToNo As String

    Private prn_Prev_HeadIndx As Integer

    Private NoFo_STS As Integer = 0
    Private prn_Tot_EBeam_Stk As Single = 0
    Private prn_Tot_Pavu_Stk As Single = 0
    Private prn_Tot_Yarn_Stk As Single = 0
    Private prn_Tot_Amt_Bal As Single = 0
    Private prn_WagesFrmt As String = ""
    Private prn_WagesDontShowToPartyName As Integer = 0

    Private yarnstk, pavstk As Single
    Private yarnnm, pavnm As String
    Private Weight1, Weight2, Currency1, Currency2, Currency3, Currency4, Currency5, Currency6, Currency7, Currency8 As Single

    Private vmskOldText As String = ""
    Private vmskSelStrt As Integer = -1

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
        pnl_Selection.Visible = False
        pnl_PrintOption.Visible = False
        pnl_PrintRange.Visible = False

        lbl_BillNo.Text = ""
        lbl_BillNo.ForeColor = Color.Black

        dtp_Date.Text = ""
        msk_Date.Text = ""
        cbo_Weaver.Text = ""
        lbl_Cloth.Text = ""
        lbl_Ends_Count.Text = ""
        cbo_Grid_Count.Text = ""

        txt_Add_Amount.Text = ""
        txt_Bits_Cooly.Text = ""
        txt_Bits_Meter.Text = ""
        txt_Elogation.Text = ""

        lbl_Excess_Short.Text = ""
        txt_FoldingLess_Perc.Text = ""
        txt_Freight_Charge.Text = ""
        txt_Less_Amount.Text = ""
        lbl_Net_Amount.Text = ""
        txt_Other_Cooly.Text = ""
        txt_Other_Meter.Text = ""
        txt_Paid_Amount.Text = ""
        txt_Pcs.Text = ""
        lbl_PDcNo.Text = ""
        txt_Seconds_Cooly.Text = ""
        txt_Seconds_Meter.Text = ""
        txt_Sound_Cooly.Text = ""
        txt_Sound_Meter.Text = ""
        lbl_LotNoHeading.Text = Common_Procedures.settings.ClothReceipt_LotNo_OR_RollNo_Text
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))

        lbl_Rec_Date.Text = ""
        txt_Rec_Meter.Text = ""
        lbl_RecCode.Text = ""
        lbl_RecNo.Text = ""
        txt_Reject_Cooly.Text = ""
        txt_Reject_Meter.Text = ""
        txt_Tds.Text = ""
        lbl_Tds_Amount.Text = ""
        lbl_Total_Cooly.Text = "0.00"
        lbl_Total_Meter.Text = "0.00"
        txt_Remarks.Text = ""

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        txt_PrintRange_FromNo.Text = ""
        txt_PrintRange_ToNo.Text = ""

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

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim msk As MaskedTextBox
        On Error Resume Next

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Me.ActiveControl Is Button Or TypeOf Me.ActiveControl Is MaskedTextBox Then
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
            msk = Me.ActiveControl
            msk.SelectionStart = 0
        End If

        If Me.ActiveControl.Name <> cbo_Grid_Count.Name Then
            cbo_Grid_Count.Visible = False
        End If

        If Me.ActiveControl.Name <> dgv_Details.Name Then
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
            ElseIf TypeOf Prec_ActCtrl Is Button Then
                Prec_ActCtrl.BackColor = Color.FromArgb(2, 57, 111)
                Prec_ActCtrl.ForeColor = Color.White
            End If
        End If

    End Sub
    Private Sub ControlLostFocus1(ByVal sender As Object, ByVal e As System.EventArgs)

        On Error Resume Next

        If IsNothing(Prec_ActCtrl) = False Then
            If TypeOf Prec_ActCtrl Is TextBox Or TypeOf Prec_ActCtrl Is ComboBox Then
                Prec_ActCtrl.BackColor = Color.LightBlue
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
        'dgv_Filter_Details.CurrentCell.Selected = False
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

                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
                    Exit Sub

                ElseIf pnl_PrintOption.Visible = True Then
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
        FrmLdSTS = True

        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable
        Dim dt6 As New DataTable
        Dim dt7 As New DataTable
        Dim dt8 As New DataTable

        lbl_LotNoHeading.Text = StrConv(Common_Procedures.settings.ClothReceipt_LotNo_OR_RollNo_Text, vbProperCase)
        lbl_ClothType1_Heading.Text = Trim(UCase(Common_Procedures.ClothType.Type1))
        lbl_ClothType2_Heading.Text = Trim(UCase(Common_Procedures.ClothType.Type2))
        lbl_ClothType3_Heading.Text = Trim(UCase(Common_Procedures.ClothType.Type3))
        lbl_ClothType4_Heading.Text = Trim(UCase(Common_Procedures.ClothType.Type4))
        lbl_ClothType5_Heading.Text = Trim(UCase(Common_Procedures.ClothType.Type5))

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1116" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1084" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1089" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1100" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1380" Then '---- k.c cotton mills
            dgv_Details.Columns(6).ReadOnly = False
        End If

        Me.Text = ""

        con.Open()

        Common_Procedures.get_CashPartyName_From_All_Entries(con)

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead Where (Ledger_IdNo = 0 OR ledger_type = 'WEAVER'  or Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1) order by Ledger_DisplayName", con)
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

        da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head order by Count_Name", con)
        da.Fill(dt4)
        cbo_Grid_Count.DataSource = dt4
        cbo_Grid_Count.DisplayMember = "Cloth_Name"

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2
        pnl_Filter.BringToFront()

        pnl_PrintOption.Visible = False
        pnl_PrintOption.BringToFront()
        pnl_PrintOption.Left = (Me.Width - pnl_PrintOption.Width) \ 2
        pnl_PrintOption.Top = (Me.Height - pnl_PrintOption.Height) \ 2

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        pnl_PrintRange.Visible = False
        pnl_PrintRange.Left = (Me.Width - pnl_PrintRange.Width) \ 2
        pnl_PrintRange.Top = (Me.Height - pnl_PrintRange.Height) \ 2
        pnl_PrintRange.BringToFront()


        txt_FoldingLess_Perc.Visible = False
        lbl_FoldingLess_Perc.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Then '---- Asia Textiles (Palladam)
            txt_FoldingLess_Perc.Visible = True
            lbl_FoldingLess_Perc.Visible = True
        End If

        btn_SaveAll.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "--1035--" Then '---- Kalaimagal Textiles (Avinashi)
            btn_SaveAll.Visible = True
        End If

        btn_get_Weft_CountName_from_Master.Visible = False
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1035" Then '---- Kalaimagal Textiles (Avinashi)
            btn_get_Weft_CountName_from_Master.Visible = True
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1049" Then '---- Dhanam Textiles (Avinashi)
            dgv_Details.Columns(6).ReadOnly = False
            lbl_LessAmtCaption.Text = "Seconds Less"
            txt_Less_Amount.Enabled = False
            Lbl_ScdLsCaption.Visible = True
            Lbl_ScdLsRatCaption.Visible = True
            txt_ScdsLsMeter.Visible = True
            txt_ScdsLsRate.Visible = True
        End If

        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Weaver.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_Cloth.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_Ends_Count.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Count.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Add_Amount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Bits_Cooly.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Bits_Meter.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Elogation.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_Excess_Short.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_FoldingLess_Perc.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight_Charge.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Less_Amount.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_Net_Amount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Other_Cooly.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Remarks.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Other_Meter.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Paid_Amount.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Pcs.GotFocus, AddressOf ControlGotFocus

        AddHandler lbl_PDcNo.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_Rec_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_RecNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Rec_Meter.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Reject_Cooly.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Reject_Meter.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Remarks.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Seconds_Cooly.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Seconds_Meter.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Sound_Cooly.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Sound_Meter.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Tds.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_Tds_Amount.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_Total_Cooly.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_Total_Meter.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_CountName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ScdsLsMeter.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_ScdsLsRate.GotFocus, AddressOf ControlGotFocus


        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Filter_Show.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Cancel_PrintOption.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Simple_WithName.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Simple_WithOutName.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_WithStock_WithName.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_WithStock_WithoutName.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_PrintRange_FromNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PrintRange_ToNo.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_PrintRange.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Cancel_PrintRange.GotFocus, AddressOf ControlGotFocus


        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Weaver.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_Cloth.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_Ends_Count.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_Count.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Add_Amount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Bits_Cooly.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Bits_Meter.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Elogation.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_Excess_Short.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_FoldingLess_Perc.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight_Charge.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Less_Amount.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_Net_Amount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Other_Cooly.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Other_Meter.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Paid_Amount.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Pcs.LostFocus, AddressOf ControlLostFocus

        AddHandler lbl_PDcNo.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_Rec_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_RecNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Rec_Meter.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Reject_Cooly.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Reject_Meter.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Seconds_Cooly.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Seconds_Meter.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Sound_Cooly.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Sound_Meter.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Tds.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_Tds_Amount.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_Total_Cooly.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_Total_Meter.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ScdsLsMeter.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_ScdsLsRate.LostFocus, AddressOf ControlLostFocus


        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_CountName.LostFocus, AddressOf ControlLostFocus

        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Filter_Show.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Cancel_PrintOption.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Simple_WithName.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Simple_WithOutName.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_WithStock_WithName.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_WithStock_WithoutName.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_PrintRange_FromNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PrintRange_ToNo.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_PrintRange.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Cancel_PrintRange.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        ' AddHandler txt_Add_Amount.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Bits_Cooly.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Bits_Meter.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Elogation.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_FoldingLess_Perc.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Less_Amount.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler txt_Other_Meter.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Pcs.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler lbl_PDcNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler lbl_Rec_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler lbl_RecNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Rec_Meter.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Reject_Cooly.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Reject_Meter.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_Seconds_Cooly.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Seconds_Meter.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Sound_Cooly.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Sound_Meter.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Tds.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler lbl_Total_Cooly.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler lbl_Total_Meter.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_PrintRange_FromNo.KeyDown, AddressOf TextBoxControlKeyDown


        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        ' AddHandler txt_Add_Amount.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Bits_Cooly.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Bits_Meter.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Elogation.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler lbl_Excess_Short.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_FoldingLess_Perc.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Freight_Charge.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Less_Amount.KeyPress, AddressOf TextBoxControlKeyPress



        AddHandler txt_Other_Meter.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Pcs.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler lbl_PDcNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler lbl_Rec_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler lbl_RecNo.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Rec_Meter.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Reject_Cooly.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Reject_Meter.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler txt_Seconds_Cooly.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Seconds_Meter.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Sound_Cooly.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Sound_Meter.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Tds.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler lbl_Total_Cooly.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler lbl_Total_Meter.KeyPress, AddressOf TextBoxControlKeyPress

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

        If ActiveControl.Name = dgv_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            On Error Resume Next

            dgv1 = dgv_Details

            If IsNothing(dgv1) = False Then

                With dgv1

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                txt_Freight_Charge.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(0)

                            End If

                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 0 And Val(.CurrentRow.Cells(0).Value) = 0 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                                txt_Freight_Charge.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 0 Then
                            If .CurrentCell.RowIndex = 0 Then
                                txt_Other_Cooly.Focus()

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

            da1 = New SqlClient.SqlDataAdapter("Select a.* from Weaver_Wages_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Wages_Code = '" & Trim(NewCode) & "' and a.Weaver_Wages_Code NOT LIKE '" & Trim(PkCondition_GST) & "%'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_BillNo.Text = dt1.Rows(0).Item("Weaver_Wages_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Weaver_Wages_Date").ToString
                msk_Date.Text = dtp_Date.Text

                cbo_Weaver.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                lbl_Cloth.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_IdNo").ToString))
                lbl_Ends_Count.Text = Common_Procedures.EndsCount_IdNoToName(con, Val(dt1.Rows(0).Item("EndsCount_IdNo").ToString))
                'cbo_Grid_Count.Text = Common_Procedures.Count_IdNoToName(con, Val(dt1.Rows(0).Item("Count_IdNo").ToString))

                txt_Add_Amount.Text = dt1.Rows(0).Item("Add_Amount").ToString
                txt_Bits_Cooly.Text = dt1.Rows(0).Item("Bits_Cooly").ToString
                txt_Bits_Meter.Text = dt1.Rows(0).Item("Bits_Meters").ToString
                txt_Elogation.Text = dt1.Rows(0).Item("Elogation").ToString
                lbl_Excess_Short.Text = dt1.Rows(0).Item("Excess_Short").ToString
                txt_FoldingLess_Perc.Text = dt1.Rows(0).Item("Folding_Less").ToString

                txt_Freight_Charge.Text = dt1.Rows(0).Item("Freight_Charge").ToString
                txt_Less_Amount.Text = dt1.Rows(0).Item("Less_Amount").ToString
                lbl_Net_Amount.Text = dt1.Rows(0).Item("Net_Amount").ToString
                txt_Other_Cooly.Text = dt1.Rows(0).Item("Others_Cooly").ToString
                txt_Other_Meter.Text = dt1.Rows(0).Item("Others_Meters").ToString
                txt_Paid_Amount.Text = dt1.Rows(0).Item("Paid_Amount").ToString
                txt_Pcs.Text = dt1.Rows(0).Item("Pcs").ToString
                lbl_PDcNo.Text = dt1.Rows(0).Item("P_Dc_No").ToString
                lbl_Rec_Date.Text = dt1.Rows(0).Item("Rec_Date").ToString
                txt_Rec_Meter.Text = dt1.Rows(0).Item("Receipt_Meters").ToString
                lbl_RecCode.Text = dt1.Rows(0).Item("Weaver_Cloth_Receipt_Code").ToString
                lbl_RecNo.Text = dt1.Rows(0).Item("Rec_No").ToString
                txt_Reject_Cooly.Text = dt1.Rows(0).Item("Reject_Cooly").ToString
                txt_Reject_Meter.Text = dt1.Rows(0).Item("Reject_Meters").ToString
                txt_Seconds_Cooly.Text = dt1.Rows(0).Item("Seconds_Cooly").ToString
                txt_Seconds_Meter.Text = dt1.Rows(0).Item("Seconds_Meters").ToString
                txt_Sound_Cooly.Text = dt1.Rows(0).Item("Sound_Cooly").ToString
                txt_Sound_Meter.Text = dt1.Rows(0).Item("Sound_Meters").ToString
                txt_Tds.Text = dt1.Rows(0).Item("Tds_Perc").ToString
                lbl_Tds_Amount.Text = dt1.Rows(0).Item("Tds_Perc_Calc").ToString
                lbl_Total_Cooly.Text = dt1.Rows(0).Item("Total_Cooly").ToString
                lbl_Total_Meter.Text = dt1.Rows(0).Item("Total_Meters").ToString

                lbl_Total_Amount.Text = dt1.Rows(0).Item("Assesable_Value").ToString

                txt_ScdsLsMeter.Text = dt1.Rows(0).Item("Scecondsless_Meter").ToString
                txt_ScdsLsRate.Text = dt1.Rows(0).Item("Scecondsless_Rate").ToString

                lbl_WeaverBillNo.Text = dt1.Rows(0).Item("Weaver_BillNo").ToString
                txt_Remarks.Text = dt1.Rows(0).Item("Remarks").ToString
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))


                da2 = New SqlClient.SqlDataAdapter("Select a.* , b.Count_Name from Weaver_Wages_Yarn_Details a left outer join count_head b on a.Count_IdNo = b.Count_IdNo Where a.Weaver_Wages_Code = '" & Trim(NewCode) & "' Order by Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_Details

                    .Rows.Clear()
                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()

                            .Rows(n).Cells(0).Value = Format(Val(dt2.Rows(i).Item("Meters").ToString), "########0.00")
                            .Rows(n).Cells(1).Value = dt2.Rows(i).Item("Count_Name").ToString
                            .Rows(n).Cells(2).Value = Val(dt2.Rows(i).Item("Rd_Sp").ToString)
                            .Rows(n).Cells(3).Value = Val(dt2.Rows(i).Item("Pick").ToString)
                            .Rows(n).Cells(4).Value = Val(dt2.Rows(i).Item("Width").ToString)
                            .Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Weight_Meter").ToString), "########0.000000")
                            .Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")


                        Next i

                    End If

                    If .RowCount = 0 Then .Rows.Add()

                End With

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(0).Value = Format(Val(dt1.Rows(0).Item("Total_Dgv_Meters").ToString), "########0.00")
                    .Rows(0).Cells(6).Value = Format(Val(dt1.Rows(0).Item("Total_Dgv_Weight").ToString), "########0.000")

                End With

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

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Weaver_Wages_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Weaver_Wages_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Weaver_Wages_Entry, New_Entry, Me, con, "Weaver_Wages_Head", "Weaver_Wages_Code", NewCode, "Weaver_Wages_Date", "(Weaver_Wages_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub


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

            NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            cmd.Connection = con
            cmd.Transaction = trans

            cmd.CommandText = "Update Stock_Yarn_Processing_Details set Weight = (case when b.Weaver_Piece_Checking_Code <> '' then b.ConsumedYarn_Checking else b.ConsumedYarn_Receipt end) from Stock_Yarn_Processing_Details a, Weaver_Cloth_Receipt_Head b Where b.Weaver_Wages_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(PkCondition_WCLRC) & "' + b.Weaver_ClothReceipt_Code"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Stock_Pavu_Processing_Details set Meters = (case when b.Weaver_Piece_Checking_Code <> '' then b.ConsumedPavu_Checking else b.ConsumedPavu_Receipt end) from Stock_Pavu_Processing_Details a, Weaver_Cloth_Receipt_Head b Where b.Weaver_Wages_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(PkCondition_WCLRC) & "' + b.Weaver_ClothReceipt_Code"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Stock_Cloth_Processing_Details set reference_date = b.Weaver_ClothReceipt_Date, UnChecked_Meters = b.ReceiptMeters_Receipt, Meters_Type1 = 0, Meters_Type2 = 0, Meters_Type3 = 0, Meters_Type4 = 0, Meters_Type5 = 0 from Stock_Cloth_Processing_Details a, Weaver_Cloth_Receipt_Head b Where b.Weaver_Wages_Code = '" & Trim(NewCode) & "' and b.Weaver_Piece_Checking_Code = '' and a.Reference_Code = '" & Trim(PkCondition_WCLRC) & "' + b.Weaver_ClothReceipt_Code"
            cmd.ExecuteNonQuery()

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition_WPYMT) & Trim(NewCode), trans)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition_WADVP) & Trim(NewCode), trans)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition_WADVD) & Trim(NewCode), trans)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), trans)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition_WPTDS) & Trim(NewCode), trans)


            'cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()
            'cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            'cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(PkCondition_WPYMT) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(PkCondition_WPYMT) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()
            'cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(PkCondition_WPYMT) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(PkCondition_WPYMT) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Weaver_Wages_Code = '', Weaver_Wages_Increment = Weaver_Wages_Increment - 1, Weaver_Wages_Date = Null, ReceiptMeters_Wages = 0, Receipt_Meters =  (case when Weaver_Piece_Checking_Code <> '' then ReceiptMeters_Checking else ReceiptMeters_Receipt end), ConsumedYarn_Wages = 0, Consumed_Yarn = (case when Weaver_Piece_Checking_Code <> '' then ConsumedYarn_Checking else ConsumedYarn_Receipt end), ConsumedPavu_Wages = 0, Consumed_Pavu = (case when Weaver_Piece_Checking_Code <> '' then ConsumedPavu_Checking else ConsumedPavu_Receipt end) , Type1_Wages_Meters = 0, Type2_Wages_Meters = 0, Type3_Wages_Meters = 0, Type4_Wages_Meters = 0, Type5_Wages_Meters = 0, Total_Wages_Meters = 0, Report_Particulars_Wages = '', Report_Particulars = Report_Particulars_Receipt  Where Weaver_Wages_Code = '" & Trim(NewCode) & "'"
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

            If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

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

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Wages_No from Weaver_Wages_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  and Weaver_Wages_Code NOT LIKE '" & Trim(PkCondition_GST) & "%' Order by for_Orderby, Weaver_Wages_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Wages_No from Weaver_Wages_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  and Weaver_Wages_Code NOT LIKE '" & Trim(PkCondition_GST) & "%' Order by for_Orderby, Weaver_Wages_No", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Wages_No from Weaver_Wages_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  and Weaver_Wages_Code NOT LIKE '" & Trim(PkCondition_GST) & "%' Order by for_Orderby desc, Weaver_Wages_No desc", con)
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

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Wages_No from Weaver_Wages_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  and Weaver_Wages_Code NOT LIKE '" & Trim(PkCondition_GST) & "%' Order by for_Orderby desc, Weaver_Wages_No desc", con)
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

            lbl_BillNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Wages_Head", "Weaver_Wages_Code", "For_OrderBy", "( Weaver_Wages_Code NOT LIKE '" & Trim(PkCondition_GST) & "%')", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)
            lbl_BillNo.ForeColor = Color.Red

            msk_Date.Text = Date.Today.ToShortDateString
            Da1 = New SqlClient.SqlDataAdapter("select top 1 * from Weaver_Wages_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  and Weaver_Wages_Code NOT LIKE '" & Trim(PkCondition_GST) & "%' Order by for_Orderby desc, Weaver_Wages_No desc", con)
            Dt1 = New DataTable
            Da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If Dt1.Rows(0).Item("Weaver_Wages_Date").ToString <> "" Then msk_Date.Text = Dt1.Rows(0).Item("Weaver_Wages_Date").ToString
                End If
                txt_FoldingLess_Perc.Text = Dt1.Rows(0).Item("Folding_Less").ToString
            End If
            Dt1.Clear()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da1.Dispose()

            If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

        End Try

    End Sub

    Public Sub open_record() Implements Interface_MDIActions.open_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim InvCode As String

        Try

            inpno = InputBox("Enter Bill No.", "FOR FINDING...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Weaver_Wages_No from Weaver_Wages_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code = '" & Trim(InvCode) & "' and Weaver_Wages_Code NOT LIKE '" & Trim(PkCondition_GST) & "%'", con)
            Dt = New DataTable
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

        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Weaver_Wages_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Bill No.", "FOR NEW BILL NO. INSERTION...")

            InvCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Weaver_Wages_No from Weaver_Wages_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code = '" & Trim(InvCode) & "' and Weaver_Wages_Code NOT LIKE '" & Trim(PkCondition_GST) & "%' ", con)
            Dt = New DataTable
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
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim clth_ID As Integer = 0
        Dim Wev_ID As Integer = 0
        Dim Endcnt_ID As Integer = 0
        Dim cunt_ID As Integer = 0
        Dim Sno As Integer = 0
        Dim EntID As String = ""
        Dim PBlNo As String = ""
        Dim Partcls As String = ""
        Dim vTotMtrs As Single, vTotwgt As Single

        Dim Cr_ID As Integer = 0
        Dim Dr_ID As Integer = 0
        Dim TdsAc_ID As Integer = 0
        Dim PcsChkCode As String = ""

        Dim Lm_ID As Integer = 0
        Dim Wdth_Typ As String = ""


        Dim ConsYarn As Single = 0
        Dim ConsPavu As Single = 0

        Dim Clo_Mtrs_Pc As Single = 0
        Dim ClthName As String = ""
        Dim Rep_Partcls_Wages As String = ""

        Dim DateColUpdt As String = ""
        Dim vDat1 As Date = #1/1/2000#
        Dim vDat2 As Date = #2/2/2000#

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Weaver_Wages_Entry, New_Entry) = False Then Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Weaver_Wages_Entry, New_Entry, Me, con, "Weaver_Wages_Head", "Weaver_Wages_Code", NewCode, "Weaver_Wages_Date", "(Weaver_Wages_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Weaver_Wages_No desc", dtp_Date.Value.Date) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()
            Exit Sub
        End If

        If Not (Convert.ToDateTime(msk_Date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_Date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()
            Exit Sub
        End If

        If msk_Date.Visible = True Then

            If Trim(msk_Date.Text) <> "" Then
                If Trim(msk_Date.Text) <> "-  -" Then
                    If IsDate(msk_Date.Text) = True Then
                        vDat1 = Convert.ToDateTime(msk_Date.Text)
                    End If
                End If
            End If

            If Trim(dtp_Date.Text) <> "" Then
                If IsDate(dtp_Date.Text) = True Then
                    vDat2 = dtp_Date.Value.Date
                End If
            End If

            If IsDate(vDat1) = True And IsDate(vDat2) = True Then

                If DateDiff(DateInterval.Day, vDat1, vDat2) <> 0 Then

                    msk_Date.Focus()
                    MessageBox.Show("Invalid Wages Date", "DOES NOT SHOW REPORT....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub

                End If

            End If

        End If

        Wev_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Weaver.Text)
        clth_ID = Common_Procedures.Cloth_NameToIdNo(con, lbl_Cloth.Text)
        Endcnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, lbl_Ends_Count.Text)
        cunt_ID = Common_Procedures.Count_NameToIdNo(con, cbo_Grid_Count.Text)
        lbl_UserName.Text = Common_Procedures.User.IdNo

        For i = 0 To dgv_Details.RowCount - 1

            If Val(dgv_Details.Rows(i).Cells(1).Value) <> 0 Or Val(dgv_Details.Rows(i).Cells(6).Value) <> 0 Then

                cunt_ID = Common_Procedures.Count_NameToIdNo(con, dgv_Details.Rows(i).Cells(1).Value)
                If clth_ID = 0 Then
                    MessageBox.Show("Invalid Count Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_Details.Enabled And dgv_Details.Visible Then
                        dgv_Details.Focus()
                        dgv_Details.CurrentCell = dgv_Details.Rows(i).Cells(1)
                    End If
                    Exit Sub
                End If
            End If
        Next

        Wev_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Weaver.Text)

        If Wev_ID = 0 Then
            MessageBox.Show("Invalid Weaver Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Weaver.Enabled Then cbo_Weaver.Focus()
            Exit Sub
        End If

        If lbl_WeaverBillNo.Visible Then
            If Trim(lbl_WeaverBillNo.Text) <> "" Then
                Da = New SqlClient.SqlDataAdapter("select Weaver_BillNo from Weaver_Wages_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code LIKE '%/" & Trim(Common_Procedures.FnRange) & "' and Ledger_IdNo = " & Str(Val(Wev_ID)) & " and Weaver_BillNo = '" & Trim(lbl_WeaverBillNo.Text) & "'", con)
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                        MessageBox.Show("Duplicate Weaver Bill No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        If cbo_Weaver.Enabled Then cbo_Weaver.Focus()
                        Exit Sub
                    End If
                End If
                Dt1.Clear()
            End If
        End If

        NoCalc_Status = False
        Total_Calculation()

        vTotMtrs = 0 : vTotwgt = 0

        If dgv_Details_Total.RowCount > 0 Then
            vTotMtrs = Val(dgv_Details_Total.Rows(0).Cells(0).Value())
            vTotwgt = Val(dgv_Details_Total.Rows(0).Cells(6).Value())
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

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_BillNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Wages_Head", "Weaver_Wages_Code", "For_OrderBy", "(Weaver_Wages_Code NOT LIKE '" & Trim(PkCondition_GST) & "%')", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@WagesDate", dtp_Date.Value.Date)

            If New_Entry = True Then

                cmd.CommandText = "Insert into Weaver_Wages_Head ( Weaver_Wages_Code ,               Company_IdNo       ,     Weaver_Wages_No  ,                     for_OrderBy                                                           ,                 Weaver_Wages_Date  ,              Ledger_IdNo,      Weaver_Cloth_Receipt_Code    ,             Rec_No              ,   Rec_Date                          ,   Cloth_IdNo                 ,  P_Dc_No                   ,  EndsCount_IdNo             ,                  Pcs                   , Receipt_Meters                      , Folding_Less                            ,  Sound_Meters                            , Sound_Cooly                     ,   Seconds_Meters                        ,  Seconds_Cooly                          ,     Bits_Meters                           , Bits_Cooly                             ,  Reject_Meters                           , Reject_Cooly                       ,             Others_Meters                  ,  Others_Cooly                          ,  Total_Meters                       ,  Total_Cooly                         , Freight_Charge                     ,                 Paid_Amount                       ,   Excess_Short                     ,      Add_Amount                     ,      Tds_Perc                                ,   Tds_Perc_Calc              ,          Elogation                   ,    Less_Amount                         ,             Assesable_Value      ,                   Net_Amount                ,            Total_Dgv_Meters          ,    Total_Dgv_Weight, Weaver_BillNo               ,                                WeaverBillNo_ForOrderBy                                 , Scecondsless_Meter                   ,            Scecondsless_Rate            , Remarks                          ,       User_IdNo ) " & _
                                    "     Values                              (   '" & Trim(NewCode) & "'      , " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_BillNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_BillNo.Text))) & ",  @WagesDate    , " & Str(Val(Wev_ID)) & ",  '" & Trim(lbl_RecCode.Text) & "' ,  '" & Trim(lbl_RecNo.Text) & "' ,  '" & Trim(lbl_Rec_Date.Text) & "'  , " & Str(Val(clth_ID)) & ", '" & Trim(lbl_PDcNo.Text) & "' , " & Str(Val(Endcnt_ID)) & ", " & Str(Val(txt_Pcs.Text)) & ", " & Str(Val(txt_Rec_Meter.Text)) & ",  " & Str(Val(txt_FoldingLess_Perc.Text)) & ",  " & Str(Val(txt_Sound_Meter.Text)) & ",  " & Str(Val(txt_Sound_Cooly.Text)) & ", " & Str(Val(txt_Seconds_Meter.Text)) & ",  " & Str(Val(txt_Seconds_Cooly.Text)) & " , " & Str(Val(txt_Bits_Meter.Text)) & " ,  " & Str(Val(txt_Bits_Cooly.Text)) & ", " & Str(Val(txt_Reject_Meter.Text)) & " , " & Str(Val(txt_Reject_Cooly.Text)) & ", " & Str(Val(txt_Other_Meter.Text)) & ",  " & Str(Val(txt_Other_Cooly.Text)) & ", " & Str(Val(lbl_Total_Meter.Text)) & ", " & Str(Val(lbl_Total_Cooly.Text)) & ",  " & Str(Val(txt_Freight_Charge.Text)) & ",  " & Str(Val(txt_Paid_Amount.Text)) & ",  " & Str(Val(lbl_Excess_Short.Text)) & ",  " & Str(Val(txt_Add_Amount.Text)) & ",  " & Str(Val(txt_Tds.Text)) & ",  " & Str(Val(lbl_Tds_Amount.Text)) & ",  " & Str(Val(txt_Elogation.Text)) & ",  " & Str(Val(txt_Less_Amount.Text)) & ",  " & Str(Val(lbl_Total_Amount.Text)) & ",  " & Str(Val(CSng(lbl_Net_Amount.Text))) & ",  " & Str(Val(vTotMtrs)) & ",  " & Str(Val(vTotwgt)) & ", '" & Trim(lbl_WeaverBillNo.Text) & "',  " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_WeaverBillNo.Text))) & "  , " & Str(Val(txt_ScdsLsMeter.Text)) & " ," & Str(Val(txt_ScdsLsRate.Text)) & " , '" & Trim(txt_Remarks.Text) & "' , " & Val(lbl_UserName.Text) & "  ) "
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Weaver_Wages_Head set Weaver_Wages_Date = @WagesDate, Ledger_IdNo =  " & Str(Val(Wev_ID)) & " ,   Weaver_Cloth_Receipt_Code = '" & Trim(lbl_RecCode.Text) & "' ,   Rec_No = '" & Trim(lbl_RecNo.Text) & "' ,   Rec_Date = '" & Trim(lbl_Rec_Date.Text) & "' ,   Cloth_IdNo  =  " & Str(Val(clth_ID)) & "                                         ,  P_Dc_No = '" & Trim(lbl_PDcNo.Text) & "' ,  EndsCount_IdNo =  " & Str(Val(Endcnt_ID)) & " ,    Pcs = " & Str(Val(txt_Pcs.Text)) & " , Receipt_Meters = " & Str(Val(txt_Rec_Meter.Text)) & " , Folding_Less =  " & Str(Val(txt_FoldingLess_Perc.Text)) & "    ,  Sound_Meters  =  " & Str(Val(txt_Sound_Meter.Text)) & " , Sound_Cooly = " & Str(Val(txt_Sound_Cooly.Text)) & ",   Seconds_Meters = " & Str(Val(txt_Seconds_Meter.Text)) & ",  Seconds_Cooly =  " & Str(Val(txt_Seconds_Cooly.Text)) & ", Bits_Meters =  " & Str(Val(txt_Bits_Meter.Text)) & " , Bits_Cooly = " & Str(Val(txt_Bits_Cooly.Text)) & ",  Reject_Meters =  " & Str(Val(txt_Reject_Meter.Text)) & " , Reject_Cooly = " & Str(Val(txt_Reject_Cooly.Text)) & " , Others_Meters = " & Str(Val(txt_Other_Meter.Text)) & " ,  Others_Cooly =  " & Str(Val(txt_Other_Cooly.Text)) & " ,  Total_Meters = " & Str(Val(lbl_Total_Meter.Text)) & "   ,  Total_Cooly =  " & Str(Val(lbl_Total_Cooly.Text)) & " , Freight_Charge =  " & Str(Val(txt_Freight_Charge.Text)) & " , Paid_Amount =  " & Str(Val(txt_Paid_Amount.Text)) & "  ,   Excess_Short = " & Str(Val(lbl_Excess_Short.Text)) & "  , Add_Amount = " & Str(Val(txt_Add_Amount.Text)) & "  , Tds_Perc =  " & Str(Val(txt_Tds.Text)) & " , Tds_Perc_Calc =  " & Str(Val(lbl_Tds_Amount.Text)) & " , Scecondsless_Meter =" & Str(Val(txt_ScdsLsMeter.Text)) & "  , Remarks = '" & Trim(txt_Remarks.Text) & "' , Scecondsless_Rate =" & Str(Val(txt_ScdsLsRate.Text)) & " ,  Elogation =  " & Str(Val(txt_Elogation.Text)) & " ,    Less_Amount =  " & Str(Val(txt_Less_Amount.Text)) & " , Assesable_Value = " & Str(Val(lbl_Total_Amount.Text)) & ", Net_Amount = " & Str(Val(CSng(lbl_Net_Amount.Text))) & ", Total_Dgv_Meters = " & Str(Val(vTotMtrs)) & " ,    Total_Dgv_Weight = " & Str(Val(vTotwgt)) & ", Weaver_BillNo = '" & Trim(lbl_WeaverBillNo.Text) & "', WeaverBillNo_ForOrderBy = " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_WeaverBillNo.Text))) & " , User_idNo = " & Val(lbl_UserName.Text) & "  Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Stock_Yarn_Processing_Details set Weight = (case when b.Weaver_Piece_Checking_Code <> '' then b.ConsumedYarn_Checking else b.ConsumedYarn_Receipt end) from Stock_Yarn_Processing_Details a, Weaver_Cloth_Receipt_Head b Where b.Weaver_Wages_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(PkCondition_WCLRC) & "' + b.Weaver_ClothReceipt_Code"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Stock_Pavu_Processing_Details set Meters = (case when b.Weaver_Piece_Checking_Code <> '' then b.ConsumedPavu_Checking else b.ConsumedPavu_Receipt end) from Stock_Pavu_Processing_Details a, Weaver_Cloth_Receipt_Head b Where b.Weaver_Wages_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(PkCondition_WCLRC) & "' + b.Weaver_ClothReceipt_Code"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Stock_Cloth_Processing_Details set reference_date = b.Weaver_ClothReceipt_Date, UnChecked_Meters = b.ReceiptMeters_Receipt, Meters_Type1 = 0, Meters_Type2 = 0, Meters_Type3 = 0, Meters_Type4 = 0, Meters_Type5 = 0 from Stock_Cloth_Processing_Details a, Weaver_Cloth_Receipt_Head b Where b.Weaver_Wages_Code = '" & Trim(NewCode) & "' and b.Weaver_Piece_Checking_Code = '' and a.Reference_Code = '" & Trim(PkCondition_WCLRC) & "' + b.Weaver_ClothReceipt_Code"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Weaver_Wages_Code = '', Weaver_Wages_Increment = Weaver_Wages_Increment - 1, Weaver_Wages_Date = Null, ReceiptMeters_Wages = 0, Receipt_Meters =  (case when Weaver_Piece_Checking_Code <> '' then ReceiptMeters_Checking else ReceiptMeters_Receipt end), ConsumedYarn_Wages = 0, Consumed_Yarn = (case when Weaver_Piece_Checking_Code <> '' then ConsumedYarn_Checking else ConsumedYarn_Receipt end), ConsumedPavu_Wages = 0, Consumed_Pavu = (case when Weaver_Piece_Checking_Code <> '' then ConsumedPavu_Checking else ConsumedPavu_Receipt end) , Type1_Wages_Meters = 0, Type2_Wages_Meters = 0, Type3_Wages_Meters = 0, Type4_Wages_Meters = 0, Type5_Wages_Meters = 0, Total_Wages_Meters = 0, Report_Particulars_Wages = '', Report_Particulars = Report_Particulars_Receipt  Where Weaver_Wages_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            Rep_Partcls_Wages = "CloRcpt : LotNo. " & Trim(lbl_RecNo.Text)
            If Trim(lbl_PDcNo.Text) <> "" Then
                Rep_Partcls_Wages = Trim(Rep_Partcls_Wages) & ",  P.Dc.No : " & Trim(lbl_PDcNo.Text)
            End If
            Rep_Partcls_Wages = Trim(Rep_Partcls_Wages) & ", Bill.No. " & Trim(lbl_BillNo.Text)

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1116" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1380" Then
                Rep_Partcls_Wages = ""
                Rep_Partcls_Wages = "CloRcpt :" & Trim(lbl_RecNo.Text)
                If Trim(lbl_PDcNo.Text) <> "" Then
                    Rep_Partcls_Wages = Trim(Rep_Partcls_Wages) & ",P.Dc.No : " & Trim(lbl_PDcNo.Text)
                End If
                Rep_Partcls_Wages = Trim(Rep_Partcls_Wages) & ", Bil.No. " & Trim(lbl_BillNo.Text)
                Rep_Partcls_Wages = Trim(Rep_Partcls_Wages) & ", Cloth :" & Trim(lbl_Cloth.Text) & ",Pcs :" & Trim(lbl_Total_Meter.Text)
            End If

            ConsYarn = Val(vTotwgt)

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1081" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1116" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1380" Then '---- S.Ravichandran Textiles (Erode)

                Clo_Mtrs_Pc = Val(Common_Procedures.get_FieldValue(con, "Cloth_Head", "Meters_Pcs", "(Cloth_idno = " & Str(Val(clth_ID)) & ")", , tr))
                If Val(Clo_Mtrs_Pc) = 0 Then Clo_Mtrs_Pc = 40
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1116" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1380" Then
                    ConsPavu = Format(Val(Clo_Mtrs_Pc) * Val(txt_Pcs.Text), "##########0.00")
                Else
                    ConsPavu = Format(Val(Clo_Mtrs_Pc) * Val(lbl_Total_Meter.Text), "##########0.00")

                End If


            Else
                ConsPavu = Val(Common_Procedures.get_Pavu_Consumption(con, clth_ID, Lm_ID, Val(txt_Rec_Meter.Text), Trim(Wdth_Typ), tr))

            End If

            cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Weaver_Wages_Code = '" & Trim(NewCode) & "', Weaver_Wages_Increment = Weaver_Wages_Increment + 1, Weaver_Wages_Date = @WagesDate, ReceiptMeters_Wages = " & Str(Val(txt_Rec_Meter.Text)) & ", Receipt_Meters = " & Str(Val(txt_Rec_Meter.Text)) & ", ConsumedYarn_Wages = " & Str(Val(ConsYarn)) & ", Consumed_Yarn = " & Str(Val(ConsYarn)) & ", ConsumedPavu_Wages = " & Str(Val(ConsPavu)) & ", Consumed_Pavu = " & Str(Val(ConsPavu)) & ", Type1_Wages_Meters = " & Str(Val(txt_Sound_Meter.Text)) & ", Type2_Wages_Meters = " & Str(Val(txt_Seconds_Meter.Text)) & ", Type3_Wages_Meters = " & Str(Val(txt_Bits_Meter.Text)) & ", Type4_Wages_Meters = " & Str(Val(txt_Reject_Meter.Text)) & ", Type5_Wages_Meters = " & Str(Val(txt_Other_Meter.Text)) & ", Total_Wages_Meters = " & Str(Val(lbl_Total_Meter.Text)) & ", Report_Particulars_Wages = '" & Trim(Rep_Partcls_Wages) & "', Report_Particulars = '" & Trim(Rep_Partcls_Wages) & "'  Where Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Weaver_Wages_Yarn_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Details

                Sno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(0).Value) <> 0 Or Val(.Rows(i).Cells(6).Value) <> 0 Then

                        Sno = Sno + 1

                        cunt_ID = Common_Procedures.Count_NameToIdNo(con, .Rows(i).Cells(1).Value, tr)

                        cmd.CommandText = "Insert into Weaver_Wages_Yarn_Details (       Weaver_Wages_Code  ,             Company_IdNo         ,             Weaver_Wages_No    ,                               for_OrderBy                               , Weaver_Wages_Date,            Sl_No     ,                      Meters              ,            Count_IdNo    ,                        Rd_Sp                ,               Pick                        ,                      Width               ,              Weight_Meter                ,                      Weight               ) " & _
                                            "     Values                         (   '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_BillNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_BillNo.Text))) & ",       @WagesDate , " & Str(Val(Sno)) & ", " & Str(Val(.Rows(i).Cells(0).Value)) & ", " & Str(Val(cunt_ID)) & ", '" & Trim(Val(.Rows(i).Cells(2).Value)) & "',  " & Str(Val(.Rows(i).Cells(3).Value)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & " ) "
                        cmd.ExecuteNonQuery()

                    End If

                Next

            End With

            cmd.CommandText = "Update Stock_Yarn_Processing_Details set Weight = " & Str(Val(ConsYarn)) & " Where Reference_Code = '" & Trim(PkCondition_WCLRC) & Trim(lbl_RecCode.Text) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Stock_Pavu_Processing_Details set Meters = " & Str(Val(ConsPavu)) & " Where Reference_Code = '" & Trim(PkCondition_WCLRC) & Trim(lbl_RecCode.Text) & "'"
            cmd.ExecuteNonQuery()

            If Trim(PcsChkCode) = "" Then
                cmd.CommandText = "Update Stock_Cloth_Processing_Details set reference_date = @WagesDate, UnChecked_Meters = 0, Meters_Type1 = " & Str(Val(txt_Sound_Meter.Text)) & ", Meters_Type2 = " & Str(Val(txt_Seconds_Meter.Text)) & ", Meters_Type3 = " & Str(Val(txt_Bits_Meter.Text)) & ", Meters_Type4 = " & Str(Val(txt_Reject_Meter.Text)) & ", Meters_Type5 = " & Str(Val(txt_Other_Meter.Text)) & " Where Reference_Code = '" & Trim(PkCondition_WCLRC) & Trim(lbl_RecCode.Text) & "'"
                cmd.ExecuteNonQuery()
            End If

            'cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()
            'cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            'cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(PkCondition_WPYMT) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(PkCondition_WPYMT) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()
            'cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(PkCondition_WPYMT) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(PkCondition_WPYMT) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            Cr_ID = Wev_ID
            Dr_ID = Common_Procedures.CommonLedger.Weaving_Wages_Ac
            TdsAc_ID = Common_Procedures.CommonLedger.TDS_Payable_Ac

            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "--1035--" Then '---- Kalaimagal Textiles (Avinashi)
                vLed_IdNos = Val(Common_Procedures.CommonLedger.Weaving_Wages_Ac) & "|" & Wev_ID & "|" & Val(Common_Procedures.CommonLedger.Freight_Charges_Ac) & "|" & Val(Common_Procedures.CommonLedger.WEAVER_LESS_AMOUNT) & "|" & Val(Common_Procedures.CommonLedger.TDS_Payable_Ac)
                vVou_Amts = -1 * (Val(CSng(lbl_Net_Amount.Text)) + Val(txt_Freight_Charge.Text) + Val(lbl_Tds_Amount.Text) + Val(txt_Less_Amount.Text)) & "|" & Val(CSng(lbl_Net_Amount.Text)) & "|" & Val(txt_Freight_Charge.Text) & "|" & Val(txt_Less_Amount.Text) & "|" & Val(lbl_Tds_Amount.Text)

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1049" Then
                vLed_IdNos = Val(Common_Procedures.CommonLedger.Weaving_Wages_Ac) & "|" & Wev_ID & "|" & Val(Common_Procedures.CommonLedger.Freight_Charges_Ac) & "|" & Val(Common_Procedures.CommonLedger.TDS_Payable_Ac)
                vVou_Amts = -1 * (Val(CSng(lbl_Net_Amount.Text)) + Val(txt_Freight_Charge.Text) + Val(lbl_Tds_Amount.Text)) & "|" & Val(CSng(lbl_Net_Amount.Text)) & "|" & Val(txt_Freight_Charge.Text) & "|" & Val(lbl_Tds_Amount.Text)

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "-------" Then '---- Kalaimagal Textiles (Avinashi)
                vLed_IdNos = Val(Common_Procedures.CommonLedger.Weaving_Wages_Ac) & "|" & Wev_ID & "|" & Val(Common_Procedures.CommonLedger.TDS_Payable_Ac)
                vVou_Amts = -1 * (Val(CSng(lbl_Net_Amount.Text)) + Val(lbl_Tds_Amount.Text)) & "|" & Val(CSng(lbl_Net_Amount.Text)) & "|" & Val(lbl_Tds_Amount.Text)

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1100" Then
                vLed_IdNos = Wev_ID & "|" & Val(Common_Procedures.CommonLedger.Weaving_Wages_Ac)
                vVou_Amts = Format(Val(CSng(lbl_Total_Cooly.Text)), "#########0") & "|" & -1 * Format(Val(CSng(lbl_Total_Cooly.Text)), "#########0")

            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1238" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1239" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1249" Then '---- Prakash Textiles (Somanur)
                vLed_IdNos = Wev_ID & "|" & Common_Procedures.CommonLedger.Weaving_Wages_Ac
                vVou_Amts = Format(Val(CSng(lbl_Total_Amount.Text)) + Val(txt_Freight_Charge.Text), "###########0.00") & "|" & Format(-1 * (Val(CSng(lbl_Total_Amount.Text)) + Val(txt_Freight_Charge.Text)), "#########0.00")

            Else
                'vLed_IdNos = Wev_ID & "|" & Val(Common_Procedures.CommonLedger.Weaving_Wages_Ac) & "|" & Val(Common_Procedures.CommonLedger.TDS_Payable_Ac)
                'vVou_Amts = Val(CSng(lbl_Net_Amount.Text)) & "|" & -1 * (Val(CSng(lbl_Net_Amount.Text)) - Val(lbl_Tds_Amount.Text)) & "|" & -1 * Val(lbl_Tds_Amount.Text)
                vLed_IdNos = Wev_ID & "|" & Val(Common_Procedures.CommonLedger.Weaving_Wages_Ac)
                vVou_Amts = Val(CSng(lbl_Total_Amount.Text)) & "|" & -1 * (Val(CSng(lbl_Total_Amount.Text)))

            End If
            If Common_Procedures.Voucher_Updation(con, "Wea.Wages", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_BillNo.Text), msk_Date.Text, "Bill No : " & Trim(lbl_BillNo.Text) & IIf(Trim(lbl_PDcNo.Text) <> "", " , P.Dc.No : " & Trim(lbl_PDcNo.Text), ""), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), tr)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1100" Then
                vLed_IdNos = Val(Common_Procedures.CommonLedger.Weaving_Wages_Ac) & "|" & Wev_ID
                vVou_Amts = Val(CSng(txt_Less_Amount.Text)) & "|" & -1 * (Val(CSng(txt_Less_Amount.Text)))

                If Common_Procedures.Voucher_Updation(con, "Wea.Wages.Less", Val(lbl_Company.Tag), Trim(Pk_Condition2) & Trim(NewCode), Trim(lbl_BillNo.Text), msk_Date.Text, "Bill No : " & Trim(lbl_BillNo.Text) & IIf(Trim(lbl_PDcNo.Text) <> "", " , P.Dc.No : " & Trim(lbl_PDcNo.Text), ""), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                    Throw New ApplicationException(ErrMsg)
                End If
            End If

            '--FReight A/c Posting Separate
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition_WFRGT) & Trim(NewCode), tr)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1238" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1239" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1249" Then '---- Prakash Textiles (Somanur)
                vLed_IdNos = ""
                vVou_Amts = ""
                ErrMsg = ""

                vLed_IdNos = Val(Common_Procedures.CommonLedger.Freight_Charges_Ac) & "|" & Wev_ID
                vVou_Amts = Val(txt_Freight_Charge.Text) & "|" & -1 * Val(txt_Freight_Charge.Text)

                If Common_Procedures.Voucher_Updation(con, "Wea.Wage.Freight", Val(lbl_Company.Tag), Trim(PkCondition_WFRGT) & Trim(NewCode), Trim(lbl_BillNo.Text), msk_Date.Text, "Bill No : " & Trim(lbl_BillNo.Text) & IIf(Trim(lbl_PDcNo.Text) <> "", " , P.Dc.No : " & Trim(lbl_PDcNo.Text), ""), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                    Throw New ApplicationException(ErrMsg)
                End If

            End If


            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition_WPTDS) & Trim(NewCode), tr)

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1049" Then '-------SREE DHANALAXSHMI TEXTILES (AVINASHI)
                'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1035" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1049" Then '---- Kalaimagal Textiles (Avinashi)
                vLed_IdNos = ""
                vVou_Amts = ""
                ErrMsg = ""

                vLed_IdNos = Val(Common_Procedures.CommonLedger.TDS_Payable_Ac) & "|" & Wev_ID
                vVou_Amts = Val(CSng(lbl_Tds_Amount.Text)) & "|" & -1 * Val(CSng(lbl_Tds_Amount.Text))

                If Common_Procedures.Voucher_Updation(con, "Wea.Tds", Val(lbl_Company.Tag), Trim(PkCondition_WPTDS) & Trim(NewCode), Trim(lbl_BillNo.Text), msk_Date.Text, "Bill No : " & Trim(lbl_BillNo.Text) & IIf(Trim(lbl_PDcNo.Text) <> "", " , P.Dc.No : " & Trim(lbl_PDcNo.Text), ""), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                    Throw New ApplicationException(ErrMsg)
                End If
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition_WADVP) & Trim(NewCode), tr)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition_WADVD) & Trim(NewCode), tr)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1081" Then
                vLed_IdNos = ""
                vVou_Amts = ""
                ErrMsg = ""
                If Val(txt_Add_Amount.Text) = 0 Then
                    txt_Add_Amount.Text = 0.0
                End If
                vLed_IdNos = Val(Common_Procedures.CommonLedger.Cash_Ac) & "|" & Wev_ID
                vVou_Amts = Val(CSng(txt_Add_Amount.Text)) & "|" & -1 * Val(CSng(txt_Add_Amount.Text))
                If Common_Procedures.Voucher_Updation(con, "Wea.AdvPymt", Val(lbl_Company.Tag), Trim(PkCondition_WADVP) & Trim(NewCode), Trim(lbl_BillNo.Text), msk_Date.Text, "Bill No : " & Trim(lbl_BillNo.Text) & IIf(Trim(lbl_PDcNo.Text) <> "", " , P.Dc.No : " & Trim(lbl_PDcNo.Text), ""), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                    Throw New ApplicationException(ErrMsg)
                End If

                vLed_IdNos = ""
                vVou_Amts = ""
                ErrMsg = ""
                If Val(txt_Less_Amount.Text) = 0 Then
                    txt_Less_Amount.Text = 0.0
                End If
                vLed_IdNos = Wev_ID & "|" & Val(Common_Procedures.CommonLedger.Cash_Ac)
                vVou_Amts = Val(CSng(txt_Less_Amount.Text)) & "|" & -1 * Val(CSng(txt_Less_Amount.Text))
                If Common_Procedures.Voucher_Updation(con, "Wea.AdvDed", Val(lbl_Company.Tag), Trim(PkCondition_WADVD) & Trim(NewCode), Trim(lbl_BillNo.Text), msk_Date.Text, "Bill No : " & Trim(lbl_BillNo.Text) & IIf(Trim(lbl_PDcNo.Text) <> "", " , P.Dc.No : " & Trim(lbl_PDcNo.Text), ""), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                    Throw New ApplicationException(ErrMsg)
                End If

            End If



            'cmd.CommandText = "Insert into Voucher_Head(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, Debtor_Idno, Creditor_Idno, Total_VoucherAmount, Narration, Indicate, Year_For_Report, Entry_Identification, Voucher_Receipt_Code) " & _
            '                    " Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_BillNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_BillNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_BillNo.Text))) & ", 'Wea.Wages', @WagesDate, " & Str(Val(Dr_ID)) & ", " & Str(Val(Cr_ID)) & ", " & Str(Val(CSng(txt_Net_Amount.Text))) & ", 'Bill No. : " & Trim(lbl_BillNo.Text) & "', 1, " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "', '')"
            'cmd.ExecuteNonQuery()

            'cmd.CommandText = "Insert into Voucher_Details(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, SL_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification ) " & _
            '                  " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_BillNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_BillNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_BillNo.Text))) & ", 'Wea.Wages', @WagesDate, 1, " & Str(Val(Cr_ID)) & ", " & Str(Val(CSng(txt_Net_Amount.Text))) & ", 'Bill No. : " & Trim(lbl_BillNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' )"
            'cmd.ExecuteNonQuery()

            'cmd.CommandText = "Insert into Voucher_Details(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, SL_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification ) " & _
            '                  " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_BillNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_BillNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_BillNo.Text))) & ", 'Wea.Wages', @WagesDate, 2, " & Str(Val(Dr_ID)) & ", " & Str(-1 * (Val(CSng(txt_Net_Amount.Text)) - Val(txt_Tds_Calc.Text))) & ", 'Bill No. : " & Trim(lbl_BillNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' )"
            'cmd.ExecuteNonQuery()

            'If Val(TdsAc_ID) <> 0 And Val(txt_Tds_Calc.Text) <> 0 Then
            '    cmd.CommandText = "Insert into Voucher_Details(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, SL_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification ) " & _
            '                      " Values             ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_BillNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_BillNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_BillNo.Text))) & ", 'Wea.Wages', @WagesDate, 3, " & Str(Val(TdsAc_ID)) & ", " & Str(-1 * Val(txt_Tds_Calc.Text)) & ", 'Bill No. : " & Trim(lbl_BillNo.Text) & "', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "' )"
            '    cmd.ExecuteNonQuery()
            'End If

            If Val(txt_Paid_Amount.Text) <> 0 Then

                vLed_IdNos = Val(Common_Procedures.CommonLedger.Cash_Ac) & "|" & Val(Wev_ID)
                vVou_Amts = Val(txt_Paid_Amount.Text) & "|" & -1 * Val(txt_Paid_Amount.Text)
                If Common_Procedures.Voucher_Updation(con, "Wea.Pymt", Val(lbl_Company.Tag), Trim(PkCondition_WPYMT) & Trim(NewCode), Trim(lbl_BillNo.Text), msk_Date.Text, "Bill No : " & Trim(lbl_BillNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr) = False Then
                    Throw New ApplicationException(ErrMsg)
                End If


                'Cr_ID = Common_Procedures.CommonLedger.Cash_Ac
                'Dr_ID = Wev_ID

                'cmd.CommandText = "Insert into Voucher_Head(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, Debtor_Idno, Creditor_Idno, Total_VoucherAmount, Narration, Indicate, Year_For_Report, Entry_Identification, Voucher_Receipt_Code) " & _
                '                    " Values ('" & Trim(PkCondition_WPYMT) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_BillNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_BillNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_BillNo.Text))) & ", 'Wea.Pymt', @WagesDate, " & Str(Val(Dr_ID)) & ", " & Str(Val(Cr_ID)) & ", " & Str(Val(txt_Paid_Amount.Text)) & ", 'Cash paid', 1, " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(PkCondition_WPYMT) & Trim(NewCode) & "', '')"
                'cmd.ExecuteNonQuery()

                'cmd.CommandText = "Insert into Voucher_Details(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, SL_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification ) " & _
                '                  " Values             ('" & Trim(PkCondition_WPYMT) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_BillNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_BillNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_BillNo.Text))) & ", 'Wea.Pymt', @WagesDate, 1, " & Str(Val(Cr_ID)) & ", " & Str(Val(txt_Paid_Amount.Text)) & ", 'Cash paid', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(PkCondition_WPYMT) & Trim(NewCode) & "' )"
                'cmd.ExecuteNonQuery()

                'cmd.CommandText = "Insert into Voucher_Details(Voucher_Code, For_OrderByCode, Company_IdNo, Voucher_No, For_OrderBy, Voucher_Type, Voucher_Date, SL_No, Ledger_IdNo, Voucher_Amount, Narration, Year_For_Report, Entry_Identification ) " & _
                '                  " Values             ('" & Trim(PkCondition_WPYMT) & Trim(NewCode) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_BillNo.Text))) & ", " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_BillNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_BillNo.Text))) & ", 'Wea.Pymt', @WagesDate, 2, " & Str(Val(Dr_ID)) & ", " & Str(-1 * (Val(txt_Paid_Amount.Text))) & ", 'Cash paid', " & Str(Val(Year(Common_Procedures.Company_FromDate))) & ", '" & Trim(PkCondition_WPYMT) & Trim(NewCode) & "' )"
                'cmd.ExecuteNonQuery()

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

            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()

        End Try

    End Sub

    Private Sub TotalMeter_Calculation()
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt2 As New DataTable
        Dim tlmtr As Single = 0
        Dim TtConsMtrs As Single = 0
        Dim Clo_IdNo As Integer = 0
        Dim Stock_In As String = ""
        Dim cnt_id As Integer = 0
        Dim mtrspcs As Single = 0

        If NoCalc_Status = True Then Exit Sub

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1100" Then
            tlmtr = Val(txt_Sound_Meter.Text)
        Else
            tlmtr = Val(txt_Sound_Meter.Text) + Val(txt_Seconds_Meter.Text) + Val(txt_Reject_Meter.Text) + Val(txt_Bits_Meter.Text) + Val(txt_Other_Meter.Text)
        End If


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1081" Then '---- S.Ravichandran Textiles (Erode)
            'tlmtr = Common_Procedures.Meter_RoundOff(tlmtr)
            lbl_Total_Meter.Text = Format(Val(tlmtr), "#########0.00")
        Else
            lbl_Total_Meter.Text = Format(Val(tlmtr), "#########0.00")
        End If

        TtConsMtrs = 0

        Clo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, lbl_Cloth.Text)

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1089" Then
            If Val(Clo_IdNo) <> 0 And Val(txt_Pcs.Text) <> 0 Then
                Stock_In = ""
                mtrspcs = 0

                Da = New SqlClient.SqlDataAdapter("Select Stock_In , Meters_Pcs from  Cloth_Head Where Cloth_idno = " & Str(Val(Clo_IdNo)), con)
                Dt2 = New DataTable
                Da.Fill(Dt2)
                If Dt2.Rows.Count > 0 Then
                    Stock_In = Dt2.Rows(0)("Stock_In").ToString
                    mtrspcs = Val(Dt2.Rows(0)("Meters_Pcs").ToString)
                End If
                Dt2.Clear()
            End If

            TtConsMtrs = Val(mtrspcs) * Val(lbl_Total_Meter.Text)

            With dgv_Details

                If .Rows.Count = 1 Then
                    .Rows(0).Cells(0).Value = Format(Val(lbl_Total_Meter.Text), "##########0.00")

                Else
                    If TtConsMtrs = 0 Or TtConsMtrs = Val(.Rows(0).Cells(0).Value) Then
                        .Rows(0).Cells(0).Value = Format(Val(TtConsMtrs), "##########0.00")

                    End If

                End If

            End With
        Else
            With dgv_Details_Total
                If .Rows.Count > 0 Then
                    TtConsMtrs = .Rows(0).Cells(0).Value
                End If
            End With


            With dgv_Details

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1035" Then
                    If .Rows.Count = 1 Then
                        .Rows(0).Cells(0).Value = Format(Val(lbl_Total_Meter.Text), "##########0.00")

                    Else
                        If TtConsMtrs = 0 Or TtConsMtrs = Val(.Rows(0).Cells(0).Value) Then
                            .Rows(0).Cells(0).Value = Format(Val(lbl_Total_Meter.Text), "##########0.00")

                        End If

                    End If
                End If

            End With

        End If



        Excess_Short_Calculation()

    End Sub

    Private Sub TotalCooly_Calculation()
        Dim tlcly As Double = 0

        If NoCalc_Status = True Then Exit Sub

        TotalMeter_Calculation()

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1081" Then
            tlcly = (Int(txt_Sound_Meter.Text) * Val(txt_Sound_Cooly.Text)) + (Int(txt_Seconds_Meter.Text) * Val(txt_Seconds_Cooly.Text)) + (Int(txt_Reject_Meter.Text) * Val(txt_Reject_Cooly.Text)) + (Int(txt_Bits_Meter.Text) * Val(txt_Bits_Cooly.Text)) + (Int(txt_Other_Meter.Text) * Val(txt_Other_Cooly.Text))
        Else
            tlcly = (Val(txt_Sound_Meter.Text) * Val(txt_Sound_Cooly.Text)) + (Val(txt_Seconds_Meter.Text) * Val(txt_Seconds_Cooly.Text)) + (Val(txt_Reject_Meter.Text) * Val(txt_Reject_Cooly.Text)) + (Val(txt_Bits_Meter.Text) * Val(txt_Bits_Cooly.Text)) + (Val(txt_Other_Meter.Text) * Val(txt_Other_Cooly.Text))
        End If
       
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1035" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1081" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1055" Then '---- Kalaimagal Textiles (Avinashi) - S.Ravichandran Textiles (Erode)
            lbl_Total_Cooly.Text = Format(Val(tlcly), "#########0")
        Else
            lbl_Total_Cooly.Text = Format(Val(tlcly), "#########0.00")
        End If

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1081" Then '---- S.Ravichandran Textiles (Erode)
        '    lbl_Total_Cooly.Text = Format(Val(lbl_Total_Cooly.Text), "#########0")
        'Else
        '    lbl_Total_Cooly.Text = Format(Val(lbl_Total_Cooly.Text), "#########0.00")
        'End If

        Total_Amount_Calculation()

    End Sub

    Private Sub TdsCommision_Calculation()
        Dim tdsamt As String = 0

        If NoCalc_Status = True Then Exit Sub

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1035" Then
            tdsamt = Format((Val(lbl_Total_Cooly.Text) - Val(txt_Freight_Charge.Text) - Val(txt_Less_Amount.Text)) * Val(txt_Tds.Text) / 100, "########0.00")
        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1098" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1088" Then
            tdsamt = Format(Val(lbl_Total_Cooly.Text) * Val(txt_Tds.Text) / 100, "########0.00")

        Else
            tdsamt = Format(Val(lbl_Total_Amount.Text) * Val(txt_Tds.Text) / 100, "########0.00")

        End If

        lbl_Tds_Amount.Text = Format(Val(tdsamt), "########0")

        NetAmount_Calculation()

    End Sub

    Private Sub Weight_Calculation(ByVal CurRow As Integer, ByVal CurCol As Integer)
        Dim ConsYrn As Double = 0
        Dim ConsYrn_round As Double = 0
        Dim vClo_Mtrs As Double = 0
        Dim Wgt_Mtr As Double = 0
        Dim RdSp As Double = 0
        Dim Pick As Double = 0
        Dim Weft As Double = 0
        Dim FdLessMtrs As Double = 0
        Dim ConsYrn_Int As Integer = 0
        Dim s1 As String = ""
        Dim s2 As String = ""

        On Error Resume Next

        If NoCalc_Status = True Then Exit Sub

        With dgv_Details
            If .Visible Then

                If .Rows.Count > 0 Then

                    If CurCol = 0 Or CurCol = 1 Or CurCol = 2 Or CurCol = 3 Or CurCol = 4 Or CurCol = 5 Then

                        vClo_Mtrs = Val(.Rows(CurRow).Cells(0).Value)

                        FdLessMtrs = 0
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Then '---- Asia Textiles (Tirupur)
                            FdLessMtrs = vClo_Mtrs * Val(txt_FoldingLess_Perc.Text) / 100
                        End If
                        vClo_Mtrs = Format(vClo_Mtrs - FdLessMtrs, "#########0.00")

                        Wgt_Mtr = Val(.Rows(CurRow).Cells(5).Value)

                        ConsYrn = 0
                        If Val(Wgt_Mtr) <> 0 Then
                            ConsYrn = Val(vClo_Mtrs) * Val(Wgt_Mtr)

                        Else

                            RdSp = Val(.Rows(CurRow).Cells(2).Value)
                            Pick = Val(.Rows(CurRow).Cells(3).Value)
                            Weft = Val(Common_Procedures.get_FieldValue(con, "count_head", "Resultant_Count", "(count_name = '" & Trim(.Rows(CurRow).Cells(1).Value) & "')"))
                            If Val(Weft) = 0 Then
                                Weft = Val(.Rows(CurRow).Cells(1).Value)
                            End If

                            If Val(Weft) <> 0 Then
                                ConsYrn = (vClo_Mtrs * RdSp * Pick * 1.0937) / (84 * 22 * Weft)
                            End If

                        End If

                        If Trim(Common_Procedures.settings.CompanyName) = "1009" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Then
                            ConsYrn = Format(Val(ConsYrn), "#########0.0")
                            .Rows(CurRow).Cells(6).Value = Format(Val(ConsYrn), "#########0.000")

                        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1035" Then '---- KALAIMAGAL TEX

                            ConsYrn = Format(Val(ConsYrn), "#########0")
                            .Rows(CurRow).Cells(6).Value = Format(Val(ConsYrn), "#########0.000")

                        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1057" Then
                            s1 = Trim(Format(Val(ConsYrn), "#########0.000"))
                            s2 = Microsoft.VisualBasic.Right(s1, 2)
                            Select Case Val(s2)
                                Case Is < 25
                                    ConsYrn = Val(Microsoft.VisualBasic.Left(s1, Len(s1) - 2))
                                Case Is < 75
                                    ConsYrn = Val(Microsoft.VisualBasic.Left(s1, Len(s1) - 2)) + 0.05
                                Case Else
                                    ConsYrn = Val(Microsoft.VisualBasic.Left(s1, Len(s1) - 2)) + 0.1
                            End Select

                            .Rows(CurRow).Cells(6).Value = Format(Val(ConsYrn), "#########0.000")

                        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1081" Then '---- 
                            ConsYrn = Format(Val(Val(.Rows(CurRow).Cells(0).Value)), "#########0")
                            .Rows(CurRow).Cells(6).Value = Format(Val(ConsYrn), "#########0.00")

                        Else

                            .Rows(CurRow).Cells(6).Value = Format(Val(ConsYrn), "#########0.000")

                        End If

                    End If

                    Total_Calculation()

                End If

            End If

        End With
    End Sub

    Private Sub NetAmount_Calculation()
        Dim NtAmt As Single = 0

        If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

        NtAmt = Val(lbl_Total_Amount.Text) - Val(lbl_Tds_Amount.Text)

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1057" Then '---- Karthikeyan Textiles (mangalam)
            NtAmt = Int(NtAmt)
        End If

        lbl_Net_Amount.Text = Format(Val(NtAmt), "##########0")

        lbl_Net_Amount.Text = Common_Procedures.Currency_Format(Val(CSng(lbl_Net_Amount.Text)))

    End Sub

    Private Sub Excess_Short_Calculation()
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt2 As New DataTable
        Dim Clo_IdNo As Integer = 0
        Dim Stock_In As String
        Dim mtrspcs As Single

        Clo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, lbl_Cloth.Text)

        Stock_In = ""
        mtrspcs = 0
        If Val(Clo_IdNo) <> 0 Then
            Da = New SqlClient.SqlDataAdapter("Select Stock_In , Meters_Pcs from  Cloth_Head Where Cloth_idno = " & Str(Val(Clo_IdNo)), con)
            Dt2 = New DataTable
            Da.Fill(Dt2)
            If Dt2.Rows.Count > 0 Then
                Stock_In = Dt2.Rows(0)("Stock_In").ToString
                mtrspcs = Val(Dt2.Rows(0)("Meters_Pcs").ToString)
            End If
            Dt2.Clear()
        End If

        If NoCalc_Status = True Then Exit Sub

        If Trim(Stock_In) <> "" And Trim(UCase(Stock_In)) = "PCS" Then
            lbl_Excess_Short.Text = Val(lbl_Total_Meter.Text) - Val(txt_Pcs.Text)
        Else
            lbl_Excess_Short.Text = Val(lbl_Total_Meter.Text) - Val(txt_Rec_Meter.Text)
        End If

        If Val(txt_Pcs.Text) > 0 Then
            txt_Elogation.Text = Format(Val(lbl_Excess_Short.Text) / Val(txt_Pcs.Text), "#########0.00")
        Else
            txt_Elogation.Text = ""
        End If

    End Sub

    Private Sub Total_Amount_Calculation()
        Dim tlamt As Single = 0

        If NoCalc_Status = True Then Exit Sub

        '  If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1081" Then '---- S.Ravichandran Textiles (Erode)
        '  tlamt = Val(lbl_Total_Cooly.Text) - Val(txt_Freight_Charge.Text)
        ' Else
        tlamt = Val(lbl_Total_Cooly.Text) - Val(txt_Freight_Charge.Text) + Val(txt_Add_Amount.Text) - Val(txt_Less_Amount.Text)
        ' End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1100" Then
            lbl_Total_Amount.Text = Format(Val(tlamt), "#########0")
        Else
            lbl_Total_Amount.Text = Format(Val(tlamt), "#########0.00")

        End If

        TdsCommision_Calculation()

        NetAmount_Calculation()

    End Sub

    Private Sub Total_Calculation()
        Dim TotMtrs As Single
        Dim TotWgt As Single

        If NoCalc_Status = True Then Exit Sub

        TotMtrs = 0 : TotWgt = 0

        With dgv_Details
            For i = 0 To .RowCount - 1
                If Val(.Rows(i).Cells(6).Value) <> 0 Then

                    TotMtrs = TotMtrs + Val(.Rows(i).Cells(0).Value())
                    TotWgt = TotWgt + Val(.Rows(i).Cells(6).Value())

                End If
            Next i

        End With


        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(0).Value = Format(Val(TotMtrs), "########0.00")
            .Rows(0).Cells(6).Value = Format(Val(TotWgt), "########0.000")
        End With

    End Sub

    Private Sub cbo_Weaver_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Weaver.GotFocus
        cbo_Weaver.Tag = cbo_Weaver.Text
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_type = 'WEAVER'  or Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1)", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Weaver_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Weaver.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Weaver, msk_Date, txt_Sound_Meter, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_type = 'WEAVER'  or Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1)", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Weaver_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Weaver.KeyPress
        Dim q As Single = 0
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim frt_Lm, Frt_Amt, Tds_Perc As Single
        Dim LedID, NoofLm As Integer
        Dim MxId As Long = 0

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Weaver, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_type = 'WEAVER'  or Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1)", "(Ledger_idno = 0)")

        If Asc(e.KeyChar) = 13 Then

            LedID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Weaver.Text)

            If lbl_WeaverBillNo.Visible Then
                If Trim(UCase(cbo_Weaver.Text)) <> "" Then
                    If Trim(UCase(cbo_Weaver.Tag)) <> Trim(UCase(cbo_Weaver.Text)) Then

                        Da = New SqlClient.SqlDataAdapter("select max(WeaverBillNo_ForOrderBy) from Weaver_Wages_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Ledger_IdNo = " & Str(Val(LedID)) & "and Weaver_Wages_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "'", con)
                        Dt = New DataTable
                        Da.Fill(Dt)

                        MxId = 0
                        If Dt.Rows.Count > 0 Then
                            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                                MxId = Int(Val(Dt.Rows(0)(0).ToString))
                            End If
                        End If
                        Dt.Clear()
                        MxId = MxId + 1

                        lbl_WeaverBillNo.Text = Trim(UCase(MxId))

                    End If

                End If
            End If

            If MessageBox.Show("Do you want to select Cloth Receipt :", "FOR CLOTH RECEIPT SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)

            Else
                txt_Sound_Meter.Focus()

            End If

            frt_Lm = 0
            NoofLm = 0
            Frt_Amt = 0
            Tds_Perc = 0

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
            txt_Tds.Text = Val(Tds_Perc)

        End If

    End Sub

    Private Sub cbo_Weaver_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Weaver.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
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

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        dgv_Details_CellLeave(sender, e)
    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim rect As Rectangle
        Dim Tot As Single = 0
        Dim Clo_IdNo As Integer = 0
        Dim Stock_In As String
        Dim mtrspcs As Single

        With dgv_Details

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1089" Then  '----------Abc
                Clo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, lbl_Cloth.Text)

                Stock_In = ""
                mtrspcs = 0

                Da = New SqlClient.SqlDataAdapter("Select Stock_In , Meters_Pcs from  Cloth_Head Where Cloth_idno = " & Str(Val(Clo_IdNo)), con)
                Dt2 = New DataTable
                Da.Fill(Dt2)
                If Dt2.Rows.Count > 0 Then
                    Stock_In = Dt2.Rows(0)("Stock_In").ToString
                    mtrspcs = Val(Dt2.Rows(0)("Meters_Pcs").ToString)
                End If
                Dt2.Clear()

                If Trim(UCase(Stock_In)) = "PCS" Then
                    If (Val(.CurrentRow.Cells(0).Value) = 0 And .CurrentRow.Index = 0) Then
                        .Rows(0).Cells(0).Value = Format(Val(lbl_Total_Meter.Text) * Val(mtrspcs), "########0.00")
                    End If
                End If

            Else

                If (Val(.CurrentRow.Cells(0).Value) = 0 And .CurrentRow.Index = 0) Then
                    .Rows(0).Cells(0).Value = Val(lbl_Total_Meter.Text)
                End If

                If e.ColumnIndex = 0 Then
                    If e.RowIndex > 0 Then

                        Tot = 0
                        For I = 0 To dgv_Details.Rows.Count - 1
                            Tot = Tot + Val(dgv_Details.Rows(I).Cells(0).Value)
                        Next
                        If Val(dgv_Details.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) = 0 And (Val(lbl_Total_Meter.Text) > Val(Tot)) Then
                            dgv_Details.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Format(Val(lbl_Total_Meter.Text) - Val(Tot), "#########0.00")
                        End If

                    End If


                End If
            End If



            'If Trim(.CurrentRow.Cells(0).Value) = "" Then
            '    .Focus()
            '    dgv_Details.CurrentCell = dgv_Details.CurrentRow.Cells(1)
            'End If

            If e.ColumnIndex = 1 Then

                If cbo_Grid_Count.Visible = False Or Val(cbo_Grid_Count.Tag) <> e.RowIndex Then

                    cbo_Grid_Count.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head order by Count_Name", con)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    cbo_Grid_Count.DataSource = Dt1
                    cbo_Grid_Count.DisplayMember = "Count_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_Count.Left = .Left + rect.Left
                    cbo_Grid_Count.Top = .Top + rect.Top

                    cbo_Grid_Count.Width = rect.Width
                    cbo_Grid_Count.Height = rect.Height
                    cbo_Grid_Count.Text = .CurrentCell.Value

                    cbo_Grid_Count.Tag = Val(e.RowIndex)
                    cbo_Grid_Count.Visible = True

                    cbo_Grid_Count.BringToFront()
                    cbo_Grid_Count.Focus()

                End If

            Else
                cbo_Grid_Count.Visible = False

            End If


        End With
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details
            If .CurrentCell.ColumnIndex = 0 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If

            If .CurrentCell.ColumnIndex = 5 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000000")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If

            If .CurrentCell.ColumnIndex = 6 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
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
                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                If .CurrentCell.ColumnIndex = 0 Or .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
                    Excess_Short_Calculation()
                    Weight_Calculation(.CurrentCell.RowIndex, .CurrentCell.ColumnIndex)
                End If

                If .CurrentCell.ColumnIndex = 0 Or .CurrentCell.ColumnIndex = 6 Then
                    Total_Calculation()
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
    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress

        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 0 Or .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                End If
            End If
        End With

    End Sub

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyDown
        With dgv_Details

            If e.KeyCode = Keys.Left Then
                If .CurrentCell.ColumnIndex <= 0 Then
                    If .CurrentCell.RowIndex = 0 Then
                        txt_Other_Cooly.Focus()
                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.ColumnCount - 1)
                    End If
                End If
            End If

            If e.KeyCode = Keys.Right Then
                If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                    If .CurrentCell.RowIndex >= .Rows.Count - 1 Then

                        txt_Freight_Charge.Focus()
                    Else
                        .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(0)
                    End If
                End If
            End If

        End With

    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim i As Integer
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                n = .CurrentRow.Index

                If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                    For i = 0 To .ColumnCount - 1
                        .Rows(n).Cells(i).Value = ""
                    Next

                Else
                    .Rows.RemoveAt(n)

                End If

            End With

        End If
    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        dgv_Details.CurrentCell.Selected = False
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

            da = New SqlClient.SqlDataAdapter("select a.*, c.Count_Name, e.Ledger_Name from Weaver_Wages_Head a left outer join Weaver_Wages_Yarn_Details b on a.Weaver_Wages_Code = b.Weaver_Wages_Code left outer join Count_head c on b.Count_idno = c.Count_idno left outer join Ledger_head e on a.Ledger_idno = e.Ledger_idno where a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.Weaver_Wages_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by Weaver_Wages_Date, for_orderby, Weaver_Wages_No", con)
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
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Paid_Amount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Paid_Amount.KeyPress
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

    Private Sub txt_Add_Amount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Add_Amount.KeyDown
        If e.KeyValue = 38 Then txt_Freight_Charge.Focus()
        If e.KeyValue = 40 Then
            If txt_ScdsLsMeter.Visible = True Then
                txt_ScdsLsMeter.Focus()
            Else
                txt_Less_Amount.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Add_Amount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Add_Amount.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            If txt_ScdsLsMeter.Visible = True Then
                txt_ScdsLsMeter.Focus()
            Else
                txt_Less_Amount.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Bits_Cooly_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Bits_Cooly.KeyPress
        If Common_Procedures.Accept_NegativeNumbers(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Bits_Meter_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Bits_Meter.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Elogation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Elogation.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Folding_Less_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_FoldingLess_Perc.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Freight_Charge_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Freight_Charge.KeyDown
        If e.KeyValue = 38 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(0)
                dgv_Details.CurrentCell.Selected = True

            Else
                txt_Paid_Amount.Focus()

            End If
        End If
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
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

    Private Sub txt_Other_Cooly_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Other_Cooly.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If (e.KeyValue = 40) Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(0)

            Else
                txt_Freight_Charge.Focus()

            End If
        End If
    End Sub

    Private Sub txt_Other_Cooly_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Other_Cooly.KeyPress

        If Common_Procedures.Accept_NegativeNumbers(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(0)

            Else
                txt_Freight_Charge.Focus()

            End If
        End If
    End Sub

    Private Sub txt_Other_Meter_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Other_Meter.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Rec_Meter_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Rec_Meter.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Reject_Cooly_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Reject_Cooly.KeyPress
        If Common_Procedures.Accept_NegativeNumbers(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Reject_Meter_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Reject_Meter.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Seconds_Cooly_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Seconds_Cooly.KeyPress
        If Common_Procedures.Accept_NegativeNumbers(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Seconds_Meter_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Seconds_Meter.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Sound_Cooly_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Sound_Cooly.KeyPress
        If Common_Procedures.Accept_NegativeNumbers(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Sound_Meter_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Sound_Meter.KeyPress
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

        With dgv_Details

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

            With dgv_Details
                If Trim(.Rows(.CurrentRow.Index).Cells(1).Value) = "" And .CurrentRow.Index = .Rows.Count - 1 Then
                    txt_Freight_Charge.Focus()
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

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_Grid_Count_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Count.TextChanged
        Try
            If cbo_Grid_Count.Visible Then
                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If Val(cbo_Grid_Count.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 1 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Count.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub txt_Other_Meter_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Other_Meter.TextChanged
        TotalCooly_Calculation()
    End Sub

    Private Sub txt_Sound_Meter_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Sound_Meter.TextChanged
        TotalCooly_Calculation()
    End Sub

    Private Sub txt_Seconds_Meter_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Seconds_Meter.TextChanged
        TotalCooly_Calculation()
    End Sub

    Private Sub txt_Bits_Meter_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Bits_Meter.TextChanged
        TotalCooly_Calculation()
    End Sub

    Private Sub txt_Reject_Meter_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Reject_Meter.TextChanged
        TotalCooly_Calculation()
    End Sub

    Private Sub txt_Other_Cooly_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Other_Cooly.TextChanged
        TotalCooly_Calculation()
    End Sub

    Private Sub txt_Reject_Cooly_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Reject_Cooly.TextChanged
        TotalCooly_Calculation()
    End Sub

    Private Sub txt_Seconds_Cooly_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Seconds_Cooly.TextChanged
        TotalCooly_Calculation()
    End Sub

    Private Sub txt_Sound_Cooly_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Sound_Cooly.TextChanged
        TotalCooly_Calculation()
    End Sub

    Private Sub txt_Bits_Cooly_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Bits_Cooly.TextChanged
        TotalCooly_Calculation()
    End Sub

    Private Sub txt_Tds_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Tds.TextChanged
        TdsCommision_Calculation()
    End Sub

    Private Sub txt_Total_Meter_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        TdsCommision_Calculation()
    End Sub

    Private Sub txt_Rec_Meter_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Rec_Meter.TextChanged
        Excess_Short_Calculation()
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

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim LedIdNo As Integer
        Dim NewCode As String
        Dim Fldng As Single = 0
        'Dim dAt As Date
        'Dim lckdt As Date

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1081" Then '---- S.Ravichandran Textiles (Erode)
        '    lckdt = #12/12/2016#
        '    dAt = dtp_Date.Value.Date
        '    If DateDiff("d", lckdt, dAt) > 0 Then
        '        MessageBox.Show("Error in loading Dll's", "RECEIPT SELECTION........", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '        Application.Exit()
        '    End If
        'End If


        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Weaver.Text)

        If LedIdNo = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SELECT PAVU...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Weaver.Enabled And cbo_Weaver.Visible Then cbo_Weaver.Focus()
            Exit Sub
        End If

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        With dgv_Selection


            .Rows.Clear()
            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.*, b.*, c.Cloth_Name, d.EndsCount_Name from Weaver_Cloth_Receipt_Head a INNER JOIN Weaver_Wages_Head b ON a.Weaver_Wages_Code = b.Weaver_Wages_Code INNER JOIN  Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo where a.Weaver_Wages_Code = '" & Trim(NewCode) & "' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1


                    Fldng = Val(Dt1.Rows(i).Item("folding").ToString)
                    If Val(Fldng) = 0 Then Fldng = 100

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Weaver_ClothReceipt_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Weaver_ClothReceipt_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Party_DcNo").ToString
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("EndsCount_Name").ToString
                    .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("noof_pcs").ToString)
                    .Rows(n).Cells(7).Value = Format(Val(Dt1.Rows(i).Item("Receipt_Meters").ToString), "#########0.00")
                    .Rows(n).Cells(8).Value = "1"
                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Weaver_ClothReceipt_Code").ToString
                    .Rows(n).Cells(10).Value = Val(Fldng)
                    .Rows(n).Cells(11).Value = Format(Val(Dt1.Rows(i).Item("Sound_Meters").ToString), "#########0.00")
                    .Rows(n).Cells(12).Value = Format(Val(Dt1.Rows(i).Item("Seconds_Meters").ToString), "#########0.00")
                    .Rows(n).Cells(13).Value = Format(Val(Dt1.Rows(i).Item("Bits_Meters").ToString), "#########0.00")
                    .Rows(n).Cells(14).Value = Format(Val(Dt1.Rows(i).Item("Reject_Meters").ToString), "#########0.00")
                    .Rows(n).Cells(15).Value = Format(Val(Dt1.Rows(i).Item("Others_Meters").ToString), "#########0.00")
                    .Rows(n).Cells(16).Value = Format(Val(Dt1.Rows(i).Item("Sound_Cooly").ToString), "#########0.00")
                    .Rows(n).Cells(17).Value = Format(Val(Dt1.Rows(i).Item("Seconds_Cooly").ToString), "#########0.00")
                    .Rows(n).Cells(18).Value = Format(Val(Dt1.Rows(i).Item("Bits_Cooly").ToString), "#########0.00")
                    .Rows(n).Cells(19).Value = Format(Val(Dt1.Rows(i).Item("Reject_Cooly").ToString), "#########0.00")
                    .Rows(n).Cells(20).Value = Format(Val(Dt1.Rows(i).Item("Others_Cooly").ToString), "#########0.00")

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()

            Da = New SqlClient.SqlDataAdapter("select a.*, c.*, d.EndsCount_Name from Weaver_Cloth_Receipt_Head a INNER JOIN  Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo where a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Wages_Code = '' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    Fldng = Val(Dt1.Rows(i).Item("folding").ToString)
                    If Val(Fldng) = 0 Then Fldng = 100

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)

                    .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Weaver_ClothReceipt_No").ToString
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Weaver_ClothReceipt_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Party_DcNo").ToString
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("EndsCount_Name").ToString
                    .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("noof_pcs").ToString)
                    .Rows(n).Cells(7).Value = Format(Val(Dt1.Rows(i).Item("Receipt_Meters").ToString), "#########0.00")
                    .Rows(n).Cells(8).Value = ""
                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Weaver_ClothReceipt_Code").ToString
                    .Rows(n).Cells(10).Value = Val(Fldng)
                    .Rows(n).Cells(11).Value = Format(Val(Dt1.Rows(i).Item("Type1_Checking_Meters").ToString) * Val(Fldng) / 100, "#########0")
                    .Rows(n).Cells(12).Value = Format(Val(Dt1.Rows(i).Item("Type2_Checking_Meters").ToString) * Val(Fldng) / 100, "#########0")
                    .Rows(n).Cells(13).Value = Format(Val(Dt1.Rows(i).Item("Type3_Checking_Meters").ToString) * Val(Fldng) / 100, "#########0")
                    .Rows(n).Cells(14).Value = Format(Val(Dt1.Rows(i).Item("Type4_Checking_Meters").ToString) * Val(Fldng) / 100, "#########0")
                    .Rows(n).Cells(15).Value = Format(Val(Dt1.Rows(i).Item("Type5_Checking_Meters").ToString) * Val(Fldng) / 100, "#########0")
                    .Rows(n).Cells(16).Value = Format(Val(Dt1.Rows(i).Item("Wages_For_Type1").ToString), "#########0.00")
                    .Rows(n).Cells(17).Value = Format(Val(Dt1.Rows(i).Item("Wages_For_Type2").ToString), "#########0.00")
                    .Rows(n).Cells(18).Value = Format(Val(Dt1.Rows(i).Item("Wages_For_Type3").ToString), "#########0.00")
                    .Rows(n).Cells(19).Value = Format(Val(Dt1.Rows(i).Item("Wages_For_Type4").ToString), "#########0.00")
                    .Rows(n).Cells(20).Value = Format(Val(Dt1.Rows(i).Item("Wages_For_Type5").ToString), "#########0.00")

                Next

            End If
            Dt1.Clear()

        End With

        pnl_Selection.Visible = True
        pnl_Back.Enabled = False
        dgv_Selection.Focus()

    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_Pavu(e.RowIndex)
    End Sub

    Private Sub Select_Pavu(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                For i = 0 To dgv_Selection.Rows.Count - 1
                    dgv_Selection.Rows(i).Cells(8).Value = ""
                Next

                .Rows(RwIndx).Cells(8).Value = 1

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

                Select_Pavu(n)

                e.Handled = True

            End If
        End If
    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Close_Receipt_Selection()
    End Sub

    Private Sub Close_Receipt_Selection()
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0

        dgv_Details.Rows.Clear()

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(8).Value) = 1 Then

                NoCalc_Status = True

                lbl_RecCode.Text = dgv_Selection.Rows(i).Cells(9).Value
                lbl_RecNo.Text = dgv_Selection.Rows(i).Cells(1).Value
                lbl_Rec_Date.Text = dgv_Selection.Rows(i).Cells(2).Value
                lbl_PDcNo.Text = dgv_Selection.Rows(i).Cells(3).Value
                lbl_Cloth.Text = dgv_Selection.Rows(i).Cells(4).Value
                lbl_Ends_Count.Text = dgv_Selection.Rows(i).Cells(5).Value
                txt_Pcs.Text = dgv_Selection.Rows(i).Cells(6).Value
                txt_Rec_Meter.Text = dgv_Selection.Rows(i).Cells(7).Value

                txt_Sound_Meter.Text = dgv_Selection.Rows(i).Cells(11).Value
                txt_Seconds_Meter.Text = dgv_Selection.Rows(i).Cells(12).Value
                txt_Bits_Meter.Text = dgv_Selection.Rows(i).Cells(13).Value
                txt_Reject_Meter.Text = dgv_Selection.Rows(i).Cells(14).Value
                txt_Other_Meter.Text = dgv_Selection.Rows(i).Cells(15).Value

                txt_Sound_Cooly.Text = dgv_Selection.Rows(i).Cells(16).Value
                txt_Seconds_Cooly.Text = dgv_Selection.Rows(i).Cells(17).Value
                txt_Bits_Cooly.Text = dgv_Selection.Rows(i).Cells(18).Value
                txt_Reject_Cooly.Text = dgv_Selection.Rows(i).Cells(19).Value
                txt_Other_Cooly.Text = dgv_Selection.Rows(i).Cells(20).Value

                Da1 = New SqlClient.SqlDataAdapter("Select " & Val(lbl_Total_Meter.Text) & " as TotalMeter, b.*, c.Count_Name from Weaver_Cloth_Receipt_Head a, cloth_head b, count_head c where a.Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "' and a.cloth_idno = b.cloth_idno and a.count_idno = c.count_idno ", con)
                Dt1 = New DataTable
                Da1.Fill(Dt1)

                If Dt1.Rows.Count > 0 Then

                    For j = 0 To Dt1.Rows.Count - 1

                        n = dgv_Details.Rows.Add()

                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1035" Then
                            dgv_Details.Rows(n).Cells(0).Value = Format(Val(dgv_Selection.Rows(i).Cells(7).Value), "#########0.00")
                        Else
                            dgv_Details.Rows(n).Cells(0).Value = Format(Val(Dt1.Rows(j).Item("TotalMeter").ToString), "#########0.00")
                        End If

                        dgv_Details.Rows(n).Cells(1).Value = Dt1.Rows(j).Item("Count_Name").ToString
                        dgv_Details.Rows(n).Cells(2).Value = Dt1.Rows(j).Item("Cloth_ReedSpace").ToString
                        dgv_Details.Rows(n).Cells(3).Value = Dt1.Rows(j).Item("Cloth_Pick").ToString
                        dgv_Details.Rows(n).Cells(4).Value = Dt1.Rows(j).Item("Cloth_Width").ToString
                        dgv_Details.Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(j).Item("Weight_Meter_Weft").ToString), "#########0.000000")
                        dgv_Details.Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(j).Item("TotalMeter").ToString) * Val(Dt1.Rows(j).Item("Weight_Meter_Weft").ToString), "#########0.000")

                        NoCalc_Status = False
                        Weight_Calculation(n, 0)
                        NoCalc_Status = True

                    Next

                End If
                Dt1.Clear()

                NoCalc_Status = False

                TotalCooly_Calculation()

                Total_Calculation()

                Total_Amount_Calculation()

                Exit For

            End If

        Next

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        If txt_Sound_Meter.Enabled And txt_Sound_Meter.Visible Then txt_Sound_Meter.Focus()

    End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Weaver_Wages_Entry, New_Entry) = False Then Exit Sub

        prn_FromNo = Trim(lbl_BillNo.Text)
        prn_ToNo = Trim(lbl_BillNo.Text)

        prn_WagesFrmt = Common_Procedures.settings.WeaverWages_Printing_Format
        prn_WagesDontShowToPartyName = Val(Common_Procedures.settings.WeaverWages_Print_Dont_Show_To_PartyName)

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1116" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1049" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1035" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1045" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1125" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1055" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1146" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1380" Then

            pnl_PrintOption.Visible = True
            pnl_Back.Enabled = False
            pnl_PrintRange.Visible = False
            If btn_Print_WithStock_WithName.Enabled And btn_Print_WithStock_WithName.Visible Then
                btn_Print_WithStock_WithName.Focus()
            End If

        Else

            txt_PrintRange_FromNo.Text = prn_FromNo
            txt_PrintRange_ToNo.Text = prn_ToNo

            pnl_PrintRange.Visible = True
            pnl_Back.Enabled = False
            pnl_PrintOption.Visible = False

            If txt_PrintRange_FromNo.Enabled Then txt_PrintRange_FromNo.Focus()

        End If

    End Sub

    Private Sub printing_WeaverWages()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String = ""
        Dim OrdBy_FrmNo As Single = 0, OrdByToNo As Single = 0
        'Dim ps As Printing.PaperSize
        Dim PpSzSTS As Boolean = False

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            OrdBy_FrmNo = Common_Procedures.OrderBy_CodeToValue(prn_FromNo)
            OrdByToNo = Common_Procedures.OrderBy_CodeToValue(prn_ToNo)

            da1 = New SqlClient.SqlDataAdapter("select * from Weaver_Wages_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and for_orderby between " & Str(Format(Val(OrdBy_FrmNo), "########0.00")) & " and " & Str(Format(Val(OrdByToNo), "########0.00")) & " and Weaver_Wages_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_orderby, Weaver_Wages_No, Weaver_Wages_Code", con)
            'da1 = New SqlClient.SqlDataAdapter("select * from Weaver_Wages_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code = '" & Trim(NewCode) & "'", con)
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


        set_PaperSize_For_PrintDocument1()


        If Val(Common_Procedures.Print_OR_Preview_Status) = 1 Then

            Try

                If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then

                    PrintDialog1.PrinterSettings = PrintDocument1.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings

                        set_PaperSize_For_PrintDocument1()

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
        Dim NewCode As String
        Dim OrdBy_FrmNo As Single = 0, OrdByToNo As Single = 0

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetDt.Clear()

        prn_Prev_HeadIndx = -100
        prn_HeadIndx = 0

        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageSize_SetUP_STS = False

        prn_Tot_EBeam_Stk = 0
        prn_Tot_Pavu_Stk = 0
        prn_Tot_Yarn_Stk = 0
        prn_Tot_Amt_Bal = 0

        Try

            OrdBy_FrmNo = Common_Procedures.OrderBy_CodeToValue(prn_FromNo)
            OrdByToNo = Common_Procedures.OrderBy_CodeToValue(prn_ToNo)

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Cloth_Name from Weaver_Wages_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_Idno LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo = d.Cloth_Idno Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.for_orderby between " & Str(Format(Val(OrdBy_FrmNo), "########0.00")) & " and " & Str(Format(Val(OrdByToNo), "########0.00")) & " and a.Weaver_Wages_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by a.for_orderby, a.Weaver_Wages_No, a.Weaver_Wages_Code", con)
            'da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Cloth_Name from Weaver_Wages_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_Idno LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo = d.Cloth_Idno Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Wages_Code = '" & Trim(NewCode) & "'", con)
            prn_HdDt = New DataTable
            da1.Fill(prn_HdDt)

            If prn_HdDt.Rows.Count > 0 Then

                'Weaver_AllStock_Ledger(Val(prn_HdDt.Rows(0).Item("Ledger_IdNo").ToString), prn_HdDt.Rows(0).Item("Weaver_Wages_Date").ToString)

                'da2 = New SqlClient.SqlDataAdapter("select Date1, Int3 as Ent_OrderBy, meters1 as for_OrderBy, name2 as Ref_No, name1 as Ref_Code, name3 as Dc_Rec_No, sum(Int6) as EmptyBeam, sum(Meters6) as PavuMtrs, sum(weight1) as YarnWgt, sum(currency1) as amount from " & Trim(Common_Procedures.ReportTempTable) & " group by Date1, Int3, meters1, name2, name1, name3 having sum(Int6)  <> 0 or sum(Meters6) <> 0 or sum(weight1) <> 0 or sum(currency1) <> 0 Order by Date1, Int3, meters1, name2, name1", con)
                'prn_DetDt = New DataTable
                'da2.Fill(prn_DetDt)

            Else
                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim Da2 As New SqlClient.SqlDataAdapter

        If prn_HdDt.Rows.Count <= 0 Then Exit Sub

        'prn_DetDt.Clear()

        'prn_PageNo = 0

        'prn_DetIndx = 0
        'prn_DetSNo = 0

        'prn_Tot_EBeam_Stk = 0
        'prn_Tot_Pavu_Stk = 0
        'prn_Tot_Yarn_Stk = 0
        'prn_Tot_Amt_Bal = 0

        If prn_Prev_HeadIndx <> prn_HeadIndx Then
            Weaver_AllStock_Ledger(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_IdNo").ToString), prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Date").ToString)
        End If

        Da2 = New SqlClient.SqlDataAdapter("select Date1, Int3 as Ent_OrderBy, meters1 as for_OrderBy, name2 as Ref_No, name1 as Ref_Code, name3 as Dc_Rec_No, sum(Int6) as EmptyBeam, sum(Meters6) as PavuMtrs, sum(weight1) as YarnWgt, sum(currency1) as amount from " & Trim(Common_Procedures.ReportTempTable) & " group by Date1, Int3, meters1, name2, name1, name3 having sum(Int6)  <> 0 or sum(Meters6) <> 0 or sum(weight1) <> 0 or sum(currency1) <> 0 Order by Date1, Int3, meters1, name2, name1", con)
        prn_DetDt = New DataTable
        Da2.Fill(prn_DetDt)

        If Trim(UCase(prn_WagesFrmt)) = "FORMAT-1" Or Trim(UCase(prn_WagesFrmt)) = "FORMAT-1.2" Or Trim(UCase(prn_WagesFrmt)) = "FORMAT-1.3" Then

            Printing_Format1(e)

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1050" Then

            Printing_Format3(e)

        ElseIf Trim(UCase(prn_WagesFrmt)) = "FORMAT-4" Then

            Printing_Format4(e)

        Else

            Printing_Format2(e)

        End If

    End Sub

    Private Sub Printing_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs) ' Simple Format without Stock
        Dim pFont As Font
        Dim p1Font As Font
        Dim p2Font As Font
        'Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        'Dim I As Integer
        Dim NoofItems_PerPage As Integer, NoofDets As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim C1, C2, C3, C4, C5, W1, W2 As Single
        Dim snd, sec, bit, rjt, otr As Single
        Dim vPrint_Count As Integer = 0


        If prn_PageSize_SetUP_STS = False Then
            set_PaperSize_For_PrintDocument1()
            prn_PageSize_SetUP_STS = True
        End If

        p1Font = New Font("Calibri", 11, FontStyle.Bold)


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

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1055" Then
            TxtHgt = 18.75

        Else
            TxtHgt = 19

        End If


GOTOLOOP1:

        vPrint_Count = vPrint_Count + 1

        EntryCode = prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Code").ToString
        'EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        p1Font = New Font("TAM_SC_Suvita", 10, FontStyle.Bold)

        Try

            prn_Prev_HeadIndx = prn_HeadIndx

            If prn_HdDt.Rows.Count > 0 Then


                Printing_Format1_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = NoofDets + 1

                CurY = CurY + TxtHgt
                pFont = New Font("TAM_SC_Suvita", 10, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "óè‹ ", LMargin + 10, CurY, 0, 0, p1Font)
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Cloth_Name").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                snd = Val(prn_HdDt.Rows(prn_HeadIndx).Item("Sound_Meters").ToString) * Val(prn_HdDt.Rows(prn_HeadIndx).Item("Sound_cooly").ToString)
                sec = Val(prn_HdDt.Rows(prn_HeadIndx).Item("Seconds_Meters").ToString) * Val(prn_HdDt.Rows(prn_HeadIndx).Item("Seconds_cooly").ToString)
                bit = Val(prn_HdDt.Rows(prn_HeadIndx).Item("Bits_Meters").ToString) * Val(prn_HdDt.Rows(prn_HeadIndx).Item("Bits_cooly").ToString)
                rjt = Val(prn_HdDt.Rows(prn_HeadIndx).Item("Reject_Meters").ToString) * Val(prn_HdDt.Rows(prn_HeadIndx).Item("Reject_cooly").ToString)
                otr = Val(prn_HdDt.Rows(prn_HeadIndx).Item("Others_Meters").ToString) * Val(prn_HdDt.Rows(prn_HeadIndx).Item("Others_cooly").ToString)

                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1049" Then
                    pFont = New Font("Calibri", 11, FontStyle.Regular)
                    Common_Procedures.Print_To_PrintDocument(e, "I Quality", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "ê¾‡† ", LMargin + C1 + 10, CurY, 0, 0, p1Font)
                End If
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(snd), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " = ", PageWidth - 80, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Sound_Cooly").ToString, PageWidth - 100, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " X ", PageWidth - 170, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Sound_Meters").ToString, PageWidth - 190, CurY, 1, 0, pFont)

                CurY = CurY + TxtHgt
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1049" Then
                    pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                    Common_Procedures.Print_To_PrintDocument(e, "ð£¾ e†ì˜ ", LMargin + 10, CurY, 0, 0, p1Font)
                    'pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                    ' Common_Procedures.Print_To_PrintDocument(e, "õó¾ e†ì˜ ", LMargin + 10, CurY, 0, 0, p1Font)
                    pFont = New Font("Calibri", 11, FontStyle.Regular)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Receipt_Meters").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)
                Else
                    pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                    Common_Procedures.Print_To_PrintDocument(e, "ð£¾ e†ì˜ ", LMargin + 10, CurY, 0, 0, p1Font)
                    pFont = New Font("Calibri", 11, FontStyle.Regular)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Receipt_Meters").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)
                End If

                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1049" Then
                    pFont = New Font("Calibri", 11, FontStyle.Regular)
                    Common_Procedures.Print_To_PrintDocument(e, "II Quality", LMargin + C1 + 10, CurY, 0, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "ªêè‡†v ", LMargin + C1 + 10, CurY, 0, 0, p1Font)
                End If

                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(sec), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " = ", PageWidth - 80, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Seconds_Cooly").ToString, PageWidth - 100, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " X ", PageWidth - 170, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Seconds_Meters").ToString, PageWidth - 190, CurY, 1, 0, pFont)

                CurY = CurY + TxtHgt
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1032" Then '---- Asia Textiles (Tirupur)
                    'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1049" Then
                    '    pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                    '    Common_Procedures.Print_To_PrintDocument(e, "", LMargin + 10, CurY, 0, 0, p1Font)
                    '    pFont = New Font("Calibri", 11, FontStyle.Regular)
                    '    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Cooly").ToString - prn_HdDt.Rows(prn_HeadIndx).Item("Less_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
                    'Else
                    'pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                    'Common_Procedures.Print_To_PrintDocument(e, "õ÷K e†ì˜ ", LMargin + 10, CurY, 0, 0, p1Font)
                    'pFont = New Font("Calibri", 11, FontStyle.Regular)
                    'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Excess_Short").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1049" Then
                        Common_Procedures.Print_To_PrintDocument(e, "ªñ£ˆî e†ì˜ ", LMargin + 10, CurY, 0, 0, p1Font)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_Meters").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "õ÷K e†ì˜ ", LMargin + 10, CurY, 0, 0, p1Font)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Excess_Short").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)
                    End If
                    '' End If
                End If

                If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Bits_Meters").ToString) <> 0 Then
                    pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1049" Then
                        Common_Procedures.Print_To_PrintDocument(e, "H†", LMargin + C1 + 10, CurY, 0, 0, p1Font)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "îQ óè‹ ", LMargin + C1 + 10, CurY, 0, 0, p1Font)
                    End If
                    pFont = New Font("Calibri", 11, FontStyle.Regular)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(bit), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " = ", PageWidth - 80, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Bits_Cooly").ToString, PageWidth - 100, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " X ", PageWidth - 170, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Bits_Meters").ToString, PageWidth - 190, CurY, 1, 0, pFont)

                End If

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1049" Then
                    ' CurY = CurY + TxtHgt
                    If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Scecondsless_Meter").ToString) <> 0 Then
                        CurY = CurY + TxtHgt
                        pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, "ªêè‡†v ", LMargin + C1 + 10, CurY, 0, 0, p1Font)
                        pFont = New Font("Calibri", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, "-" & Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Scecondsless_Rate").ToString) * (prn_HdDt.Rows(prn_HeadIndx).Item("Scecondsless_Meter").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, " = ", PageWidth - 80, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, "-" & prn_HdDt.Rows(prn_HeadIndx).Item("Scecondsless_Rate").ToString, PageWidth - 100, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, " X ", PageWidth - 170, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Scecondsless_Meter").ToString, PageWidth - 190, CurY, 1, 0, pFont)
                    End If
                End If

                CurY = CurY + TxtHgt
                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "ªñ£ˆî ªî£¬è ", LMargin + 10, CurY, 0, 0, p1Font)
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1049" Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Cooly").ToString - prn_HdDt.Rows(prn_HeadIndx).Item("Less_Amount").ToString), LMargin + C1 - 10, CurY, 1, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_cooly").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)
                End If

                If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Reject_Meters").ToString) <> 0 Then
                    pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1032" Then
                        Common_Procedures.Print_To_PrintDocument(e, "èN¾ ", LMargin + C1 + 10, CurY, 0, 0, p1Font)
                    Else
                        pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, "ªê‚AƒÜì£v", LMargin + C1 + 10, CurY, 0, 0, p1Font)
                        pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                    End If
                    pFont = New Font("Calibri", 11, FontStyle.Regular)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(rjt), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " = ", PageWidth - 80, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Reject_Cooly").ToString, PageWidth - 100, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " X ", PageWidth - 170, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Reject_Meters").ToString, PageWidth - 190, CurY, 1, 0, pFont)
                End If

                CurY = CurY + TxtHgt
                If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Freight_Charge").ToString) <> 0 Then

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Then '---- Asia Textiles (Tirupur)
                        p1Font = New Font("Calibri", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, "V.V", LMargin + 10, CurY, 0, 0, p1Font)
                        'Common_Procedures.Print_To_PrintDocument(e, "«õ¡ ", LMargin + 10, CurY, 0, 0, pFont)

                    Else
                        p1Font = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, "õ‡® õ£ì¬è ", LMargin + 10, CurY, 0, 0, p1Font)

                    End If
                    pFont = New Font("Calibri", 11, FontStyle.Regular)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Freight_Charge").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)
                End If
                p1Font = New Font("TAM_SC_Suvita", 11, FontStyle.Bold)
                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)

                If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Others_Meters").ToString) <> 0 Then

                    'If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1032" Then '---- Asia Textiles (Tirupur)
                    pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                    Common_Procedures.Print_To_PrintDocument(e, "Þîó ", LMargin + C1 + 10, CurY, 0, 0, p1Font)
                    pFont = New Font("Calibri", 11, FontStyle.Regular)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(otr), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " = ", PageWidth - 80, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Others_Cooly").ToString, PageWidth - 100, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " X ", PageWidth - 170, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Others_Meters").ToString, PageWidth - 190, CurY, 1, 0, pFont)
                    'End If

                Else

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1018" Then
                        p2Font = New Font("Calibri", 13, FontStyle.Bold)
                        ' Common_Procedures.Print_To_PrintDocument(e, "(Weaving & Sizing Charges include)", LMargin + C1 + 10 + 50, CurY - 10, 0, 0, p2Font)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Remarks").ToString, LMargin + C1 + 10 + 50, CurY - 10, 0, 0, p2Font)

                    End If

                End If

                CurY = CurY + TxtHgt

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1032" Then '---- Asia Textiles (Tirupur)

                    If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Less_Amount").ToString) <> 0 Then
                        pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)

                        '  Common_Procedures.Print_To_PrintDocument(e, "ªêè‡†v H®ˆî‹ ", LMargin + 10, CurY, 0, 0, p1Font)
                        '  Else
                        Common_Procedures.Print_To_PrintDocument(e, "H®ˆî‹ ", LMargin + 10, CurY, 0, 0, p1Font)
                        'End If
                        ' Common_Procedures.Print_To_PrintDocument(e, "H®ˆî‹ ", LMargin + 10, CurY, 0, 0, p1Font)
                        pFont = New Font("Calibri", 11, FontStyle.Regular)
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1049" Then
                            Common_Procedures.Print_To_PrintDocument(e, "0.00", LMargin + C1 - 10, CurY, 1, 0, pFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Less_Amount").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)
                        End If

                    End If

                End If

                e.Graphics.DrawLine(Pens.Black, LMargin + C2 + 50, CurY, PageWidth - 10, CurY)
                p1Font = New Font("TAM_SC_Suvita", 11, FontStyle.Bold)
                CurY = CurY + TxtHgt
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1032" Then '---- Asia Textiles (Tirupur)
                    If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Tds_Perc_Calc").ToString) <> 0 Then
                        pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, "õK H®ˆî‹ ", LMargin + 10, CurY, 0, 0, p1Font)
                        pFont = New Font("Calibri", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Tds_Perc_Calc").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)
                    End If
                End If
                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "ªñ£ˆî‹ ", LMargin + C1 + 10, CurY, 0, 0, p1Font)
                pFont = New Font("Calibri", 11, FontStyle.Regular)

                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_Meters").ToString, PageWidth - 190, CurY, 1, 0, pFont)
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1049" Then
                    Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Cooly").ToString - prn_HdDt.Rows(prn_HeadIndx).Item("Less_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_Cooly").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                End If
                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)


                CurY = CurY + TxtHgt
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1032" Then
                    If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Add_Amount").ToString) <> 0 Then
                        pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, "Ü†õ£¡v ", LMargin + 10, CurY, 0, 0, p1Font)
                        pFont = New Font("Calibri", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Add_Amount").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)
                    End If
                End If

                CurY = CurY + TxtHgt
                e.Graphics.DrawLine(Pens.Black, LMargin + C1 - 100, CurY, LMargin + C1 - 10, CurY)

                e.Graphics.DrawLine(Pens.Black, LMargin + C2 + 50, CurY, PageWidth - 10, CurY)

                CurY = CurY + 10
                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Then
                    Common_Procedures.Print_To_PrintDocument(e, "ªî£¬è ", LMargin + 10, CurY, 0, 0, p1Font)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "eF ªî£¬è ", LMargin + 10, CurY, 0, 0, p1Font)

                End If
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Net_Amount").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)

                W1 = e.Graphics.MeasureString("dv â‡E‚¬è : ", p1Font).Width
                W2 = e.Graphics.MeasureString("Ë™ ªêô¾ : ", p1Font).Width

                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "dv â‡E‚¬è ", LMargin + C1 + 10, CurY, 0, 0, p1Font)
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, " :  " & prn_HdDt.Rows(prn_HeadIndx).Item("Pcs").ToString, LMargin + C1 + W1 + 40, CurY, 0, 0, pFont)
                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1018" Then
                    pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                    Common_Procedures.Print_To_PrintDocument(e, "Ë™ ªêô¾ ", LMargin + C1 + 250, CurY, 0, 0, p1Font)
                    pFont = New Font("Calibri", 11, FontStyle.Regular)
                    Common_Procedures.Print_To_PrintDocument(e, " :  " & prn_HdDt.Rows(prn_HeadIndx).Item("Total_Dgv_Weight").ToString, LMargin + C1 + 250 + W2 + 10, CurY, 0, 0, pFont)

                End If

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

        prn_DetDt.Clear()

        prn_PageNo = 0

        prn_DetIndx = 0
        prn_DetSNo = 0

        prn_Tot_EBeam_Stk = 0
        prn_Tot_Pavu_Stk = 0
        prn_Tot_Yarn_Stk = 0
        prn_Tot_Amt_Bal = 0

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1035" Then                   '---- Kalaimagal Textiles (Avinashi)

            If prn_HeadIndx <= prn_HdDt.Rows.Count - 1 Then

                If vPrint_Count <= 1 Then
                    TMargin = 585
                    GoTo GOTOLOOP1
                Else
                    e.HasMorePages = True
                End If

            Else
                e.HasMorePages = False

            End If

            If prn_HeadIndx <= prn_HdDt.Rows.Count - 1 Then

            End If


        Else
            If prn_HeadIndx <= prn_HdDt.Rows.Count - 1 Then
                e.HasMorePages = True
            Else
                e.HasMorePages = False
            End If

        End If




    End Sub

    Private Sub Printing_Format1_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim Led_Name As String, Led_Add As String, Led_Add1 As String, Led_Add2 As String
        Dim strHeight As Single
        Dim C1, C2, S1, W1, W2 As Single
        Dim strWIDTH As Single
        Dim Y1, Y2 As Single

        PageNo = PageNo + 1

        CurY = TMargin

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

        If Val(Common_Procedures.settings.WeaverWages_Print_Weavers_Name_IN_Heading) = 1 Then
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Name").ToString
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


        If Val(Common_Procedures.settings.WeaverWages_Print_Dont_Show_Company_Heading) <> 1 Then '---- Arul Kumaran Textiles (Somanur)

            CurY = CurY + TxtHgt - 10

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Then
                p1Font = New Font("Calibri", 11, FontStyle.Regular)
            Else
                p1Font = New Font("Calibri", 18, FontStyle.Bold)
            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Then
                Common_Procedures.Print_To_PrintDocument(e, "From", LMargin + 10, CurY, 0, 0, p1Font)
            End If

            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Then
                p1Font = New Font("Calibri", 9, FontStyle.Regular)
            Else
                p1Font = New Font("Calibri", 11, FontStyle.Regular)
            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Then '---- M.S Textiles (Tirupur)

                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                strWIDTH = e.Graphics.MeasureString(Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_No").ToString), p1Font).Width

                Y1 = CurY + 0.5
                Y2 = CurY + strHeight - 1
                Dim br = New SolidBrush(Color.FromArgb(110, 110, 110))
                Common_Procedures.FillRegionRectangle(e, PageWidth - strWIDTH - 30, Y1, PageWidth - 10, Y2, br)

                br = New SolidBrush(Color.FromArgb(255, 255, 255))
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_No").ToString), PageWidth - 20, CurY + 8, 1, 0, p1Font, br)

            End If

            CurY = CurY + strHeight - 1
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, p1Font)
            CurY = CurY + TxtHgt - 1
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, p1Font)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1032" Then
                CurY = CurY + TxtHgt - 1
                Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, p1Font)
                CurY = CurY + TxtHgt - 1
                Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, p1Font)
            End If

            CurY = CurY + strHeight
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        Else
            TxtHgt = TxtHgt + 2

        End If
        LnAr(2) = CurY

        If Val(Common_Procedures.settings.WeaverWages_Print_Weavers_Name_IN_Heading) = 1 Then
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
            Led_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Name").ToString
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

        If Val(prn_WagesDontShowToPartyName) = 1 Then

            CurY = CurY + TxtHgt
            pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
            p1Font = New Font("TAM_SC_Suvita", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "H™ ªï‹. ", LMargin + 10, CurY, 0, 0, p1Font)

            If Val(Common_Procedures.settings.WeaverWages_Print_Weavers_BillNo_SeparateSlNo) = 1 Then
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_BillNo").ToString, LMargin + W1 + 20, CurY, 0, 0, pFont)
            Else
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_No").ToString, LMargin + W1 + 20, CurY, 0, 0, pFont)
            End If

            pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "H™ «îF ", LMargin + 140 + 10, CurY, 0, 0, p1Font)
            pFont = New Font("Calibri", 11, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Date").ToString), "dd-MM-yyyy").ToString, LMargin + 140 + W2 + 20, CurY, 0, 0, pFont)

            If Val(Common_Procedures.settings.WeaverWages_Print_Weavers_BillNo_SeparateSlNo) = 1 Then
                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "ªìLõK ªï‹.", LMargin + C1 + 10, CurY, 0, 0, p1Font)
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, ": " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_No").ToString, LMargin + C1 + W1 + 40, CurY, 0, 0, pFont)

            Else
                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "óC¶ ªï‹. ", LMargin + C1 + 10, CurY, 0, 0, p1Font)
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, ": " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            End If

            pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
            If Val(Common_Procedures.settings.WeaverWages_Print_Weavers_BillNo_SeparateSlNo) = 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "ªìLõK «îF", LMargin + C2 + +10, CurY, 0, 0, p1Font)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "óC¶ «îF ", LMargin + C2 + +10, CurY, 0, 0, p1Font)
            End If
            pFont = New Font("Calibri", 11, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, ": " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Rec_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C2 + W2 + 30, CurY, 0, 0, pFont)

            CurY = CurY + 5

        Else

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("TAM_SC_Suvita", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & Led_Name, LMargin + 10, CurY, 0, 0, pFont)
            pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "H™ ªï‹. ", LMargin + C1 + 10, CurY, 0, 0, p1Font)
            If Val(Common_Procedures.settings.WeaverWages_Print_Weavers_BillNo_SeparateSlNo) = 1 Then
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_BillNo").ToString, LMargin + C1 + W1 + 20, CurY, 0, 0, pFont)
            Else
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_No").ToString, LMargin + C1 + W1 + 20, CurY, 0, 0, pFont)
            End If
            pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "H™ «îF ", LMargin + C2 + 10, CurY, 0, 0, p1Font)
            pFont = New Font("Calibri", 11, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C2 + W2 + 30, CurY, 0, 0, pFont)

            pFont = New Font("Calibri", 11, FontStyle.Regular)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & Led_Add1, LMargin + S1 + 10, CurY, 0, 0, pFont)

            pFont = New Font("Calibri", 11, FontStyle.Regular)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & Led_Add2, LMargin + S1 + 10, CurY, 0, 0, pFont)


            If Val(Common_Procedures.settings.WeaverWages_Print_Weavers_BillNo_SeparateSlNo) = 1 Then
                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "ªìLõK ªï‹.", LMargin + C1 + 10, CurY, 0, 0, p1Font)
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_No").ToString, LMargin + C1 + W1 + 40, CurY, 0, 0, pFont)
            Else
                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "óC¶ ªï‹. ", LMargin + C1 + 10, CurY, 0, 0, p1Font)
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If

            pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
            If Val(Common_Procedures.settings.WeaverWages_Print_Weavers_BillNo_SeparateSlNo) = 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "ªìLõK «îF", LMargin + C2 + +10, CurY, 0, 0, p1Font)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "óC¶ «îF ", LMargin + C2 + +10, CurY, 0, 0, p1Font)
            End If
            pFont = New Font("Calibri", 11, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Rec_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C2 + W2 + 30, CurY, 0, 0, pFont)

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

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Then
            ' CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

            'e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            'e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        End If

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt


        CurY = CurY + 10
        pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
        p1Font = New Font("TAM_SC_Suvita", 11, FontStyle.Bold)

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1032" Then
            Common_Procedures.Print_To_PrintDocument(e, "îPè£ó˜ ¬èªò£Šð‹  ", LMargin + 10, CurY, 0, 0, p1Font)
        End If

        If Val(Common_Procedures.settings.WeaverWages_Print_Weavers_Name_IN_Heading) = 1 Then
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Name").ToString
        Else
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString
        End If


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1032" Then
            If Val(Common_Procedures.settings.WeaverWages_Print_Dont_Show_Company_Heading) = 1 Then
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 15, CurY, 1, 0, p1Font)

            Else
                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

            End If
        End If
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1032" Then
            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

            e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
            e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        End If

    End Sub

    Private Sub Printing_Format2(ByRef e As System.Drawing.Printing.PrintPageEventArgs) ' common Format with Stock
        Dim pFont As Font
        Dim p1Font As Font
        Dim pTFont As Font
        'Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        'Dim I As Integer
        Dim NoofItems_PerPage As Integer, NoofDets As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim C1 As Single = 0, C2 As Single = 0, C3 As Single = 0
        Dim C4 As Single = 0, C5 As Single = 0, C6 As Single = 0
        Dim W1, W2 As Single
        Dim snd, sec, bit, rjt, otr As Single
        Dim Rndoffamt As Single = 0
        Dim Ntamt As Single = 0
        Dim Ntamt_NEW As Single = 0

        If prn_PageSize_SetUP_STS = False Then
            set_PaperSize_For_PrintDocument1()
            prn_PageSize_SetUP_STS = True
        End If


        p1Font = New Font("Calibri", 11, FontStyle.Bold)


        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30
            .Right = 50
            .Top = 1 ' 30
            .Bottom = 25 ' 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 10, FontStyle.Regular)
        pTFont = New Font("TAM_SC_Suvita", 9, FontStyle.Bold)
        p1Font = New Font("TAM_SC_Suvita", 10, FontStyle.Bold)

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

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1009" Then

            ClAr(1) = Val(110) : ClAr(2) = 80 : ClAr(3) = 85 : ClAr(4) = 85 : ClAr(5) = 90 : ClAr(6) = 0 : ClAr(7) = 90 : ClAr(8) = 0 : ClAr(9) = 100
            ClAr(10) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9))

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 140 '285
            C2 = C1 + ClAr(5)  '385

        Else
            ClAr(1) = Val(80) : ClAr(2) = 50 : ClAr(3) = 55 : ClAr(4) = 55 : ClAr(5) = 80 : ClAr(6) = 80 : ClAr(7) = 70 : ClAr(8) = 70 : ClAr(9) = 100
            ClAr(10) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9))

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4)  '285
            C2 = C1 + ClAr(5)  '385

        End If


        '  If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Then
        TxtHgt = 17
        '  Else
        '  TxtHgt = 17.25 ' 18  ' 18.5
        '  End If

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_Prev_HeadIndx = prn_HeadIndx

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format2_PageHeader(e, EntryCode, TxtHgt, pFont, pTFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = NoofDets + 1
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1009" Then
                    NoofItems_PerPage = 9
                Else
                    NoofItems_PerPage = 7
                End If


                If prn_PageNo <= 1 Then

                    CurY = CurY + TxtHgt - 10
                    Common_Procedures.Print_To_PrintDocument(e, "óè‹ ", LMargin + 10, CurY, 0, 0, pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Cloth_Name").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                    snd = Val(prn_HdDt.Rows(prn_HeadIndx).Item("Sound_Meters").ToString) * Val(prn_HdDt.Rows(prn_HeadIndx).Item("Sound_cooly").ToString)
                    sec = Val(prn_HdDt.Rows(prn_HeadIndx).Item("Seconds_Meters").ToString) * Val(prn_HdDt.Rows(prn_HeadIndx).Item("Seconds_cooly").ToString)
                    bit = Val(prn_HdDt.Rows(prn_HeadIndx).Item("Bits_Meters").ToString) * Val(prn_HdDt.Rows(prn_HeadIndx).Item("Bits_cooly").ToString)
                    rjt = Val(prn_HdDt.Rows(prn_HeadIndx).Item("Reject_Meters").ToString) * Val(prn_HdDt.Rows(prn_HeadIndx).Item("Reject_cooly").ToString)
                    otr = Val(prn_HdDt.Rows(prn_HeadIndx).Item("Others_Meters").ToString) * Val(prn_HdDt.Rows(prn_HeadIndx).Item("Others_cooly").ToString)

                    Ntamt = snd + sec + bit + rjt + otr
                    Rndoffamt = Format(Val(Ntamt), "##########0") - Ntamt
                    Ntamt_NEW = Format(Val(Ntamt), "##########0")

                    '  Common_Procedures.Print_To_PrintDocument(e, "ê¾‡† ", LMargin + C1 + 10, CurY, 0, 0, pTFont)
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1049" Then
                        pFont = New Font("Calibri", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, "I Quality", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "ê¾‡† ", LMargin + C1 + 10, CurY, 0, 0, p1Font)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(snd), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " = ", PageWidth - 80, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Sound_Cooly").ToString, PageWidth - 100, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " X ", PageWidth - 170, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Sound_Meters").ToString, PageWidth - 190, CurY, 1, 0, pFont)

                    CurY = CurY + TxtHgt
                    'Common_Procedures.Print_To_PrintDocument(e, "õó¾ e†ì˜ ", LMargin + 10, CurY, 0, 0, pTFont)
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1049" Then
                        pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, "ð£¾ e†ì˜ ", LMargin + 10, CurY, 0, 0, p1Font)
                        'pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                        ' Common_Procedures.Print_To_PrintDocument(e, "õó¾ e†ì˜ ", LMargin + 10, CurY, 0, 0, p1Font)
                        pFont = New Font("Calibri", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Receipt_Meters").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)
                    Else
                        pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, "ð£¾ e†ì˜ ", LMargin + 10, CurY, 0, 0, p1Font)
                        pFont = New Font("Calibri", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Receipt_Meters").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)
                    End If
                    '  Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Receipt_Meters").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                    pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1049" Then
                        pFont = New Font("Calibri", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, "II Quality", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "ªêè‡†v ", LMargin + C1 + 10, CurY, 0, 0, p1Font)
                    End If

                    ' Common_Procedures.Print_To_PrintDocument(e, "ªêè‡†v ", LMargin + C1 + 10, CurY, 0, 0, pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(sec), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " = ", PageWidth - 80, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Seconds_Cooly").ToString, PageWidth - 100, CurY, 1, 0, pFont)
                    pFont = New Font("Calibri", 11, FontStyle.Regular)
                    Common_Procedures.Print_To_PrintDocument(e, " X ", PageWidth - 170, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Seconds_Meters").ToString, PageWidth - 190, CurY, 1, 0, pFont)

                    CurY = CurY + TxtHgt
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1049" Then
                        Common_Procedures.Print_To_PrintDocument(e, "ªñ£ˆî e†ì˜ ", LMargin + 10, CurY, 0, 0, pTFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_Meters").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "õ÷K e†ì˜ ", LMargin + 10, CurY, 0, 0, pTFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Excess_Short").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)
                    End If

                    If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Bits_Meters").ToString) <> 0 Then
                        ' Common_Procedures.Print_To_PrintDocument(e, "îQ óè‹ ", LMargin + C1 + 10, CurY, 0, 0, pTFont)

                        pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1049" Then
                            Common_Procedures.Print_To_PrintDocument(e, "H†", LMargin + C1 + 10, CurY, 0, 0, pTFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, "îQ óè‹ ", LMargin + C1 + 10, CurY, 0, 0, pTFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(bit), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, " = ", PageWidth - 80, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Bits_Cooly").ToString, PageWidth - 100, CurY, 1, 0, pFont)
                        pFont = New Font("Calibri", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, " X ", PageWidth - 170, CurY, 1, 0, pFont)
                        pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Bits_Meters").ToString, PageWidth - 190, CurY, 1, 0, pFont)
                    End If

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1049" Then
                        ' CurY = CurY + TxtHgt
                        If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Scecondsless_Meter").ToString) <> 0 Then
                            CurY = CurY + TxtHgt
                            pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                            Common_Procedures.Print_To_PrintDocument(e, "ªêè‡†v ", LMargin + C1 + 10, CurY, 0, 0, p1Font)
                            pFont = New Font("Calibri", 11, FontStyle.Regular)
                            Common_Procedures.Print_To_PrintDocument(e, "-" & Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Scecondsless_Rate").ToString) * (prn_HdDt.Rows(prn_HeadIndx).Item("Scecondsless_Meter").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, " = ", PageWidth - 80, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, "-" & prn_HdDt.Rows(prn_HeadIndx).Item("Scecondsless_Rate").ToString, PageWidth - 100, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, " X ", PageWidth - 170, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Scecondsless_Meter").ToString, PageWidth - 190, CurY, 1, 0, pFont)
                        End If
                    End If

                    CurY = CurY + TxtHgt
                    pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                    Common_Procedures.Print_To_PrintDocument(e, "ªñ£ˆî ªî£¬è ", LMargin + 10, CurY, 0, 0, p1Font)
                    pFont = New Font("Calibri", 11, FontStyle.Regular)
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1049" Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Cooly").ToString - prn_HdDt.Rows(prn_HeadIndx).Item("Less_Amount").ToString), LMargin + C1 - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(Ntamt_NEW), "##########0.00"), LMargin + C1 - 10, CurY, 1, 0, pFont)

                        ' Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_cooly").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)
                    End If

                    'CurY = CurY + TxtHgt
                    'Common_Procedures.Print_To_PrintDocument(e, "ªñ£ˆî ªî£¬è ", LMargin + 10, CurY, 0, 0, pTFont)
                    'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_cooly").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                    If Val(rjt) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "èN¾ ", LMargin + C1 + 10, CurY, 0, 0, pTFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(rjt), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, " = ", PageWidth - 80, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Reject_Cooly").ToString, PageWidth - 100, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, " X ", PageWidth - 170, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Reject_Meters").ToString, PageWidth - 190, CurY, 1, 0, pFont)
                    End If

                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "õ‡® õ£ì¬è ", LMargin + 10, CurY, 0, 0, pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Freight_Charge").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                    If Val(otr) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "Þîó ", LMargin + C1 + 10, CurY, 0, 0, pTFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(otr), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, " = ", PageWidth - 80, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Others_Cooly").ToString, PageWidth - 100, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, " X ", PageWidth - 170, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Others_Meters").ToString, PageWidth - 190, CurY, 1, 0, pFont)
                    End If

                    CurY = CurY + TxtHgt

                    Common_Procedures.Print_To_PrintDocument(e, "H®ˆî‹ ", LMargin + 10, CurY, 0, 0, pTFont)

                    pFont = New Font("Calibri", 11, FontStyle.Regular)
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1049" Then
                        Common_Procedures.Print_To_PrintDocument(e, "0.00", LMargin + C1 - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Less_Amount").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)
                    End If
                    ' Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Less_Amount").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)



                 
                    If Rndoffamt <> 0 Then
                        pFont = New Font("Calibri", 11, FontStyle.Regular)

                        If Rndoffamt > 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, "Round Off (+)", LMargin + C1 + 10, CurY - 5, 0, 0, pFont)
                        ElseIf Rndoffamt < 0 Then
                            Common_Procedures.Print_To_PrintDocument(e, "Round Off (-)", LMargin + C1 + 10, CurY - 5, 0, 0, pFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, Format(Rndoffamt, "###########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                    End If

                    CurY = CurY + TxtHgt
                    e.Graphics.DrawLine(Pens.Black, LMargin + C2 + 10, CurY + 5, PageWidth - 10, CurY + 5)


                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "õK H®ˆî‹ ", LMargin + 10, CurY, 0, 0, pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Tds_Perc_Calc").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                    Common_Procedures.Print_To_PrintDocument(e, "ªñ£ˆî‹ ", LMargin + C1 + 10, CurY, 0, 0, pTFont)

                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_Meters").ToString, PageWidth - 190, CurY, 1, 0, pFont)
                    '  Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_Cooly").ToString, PageWidth - 10, CurY, 1, 0, pFont)


                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1049" Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Cooly").ToString - prn_HdDt.Rows(prn_HeadIndx).Item("Less_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(Ntamt_NEW), "##########0.00"), PageWidth - 10, CurY, 1, 0, pFont)

                        'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_Cooly").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                    End If



                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "Ü†õ£¡v ", LMargin + 10, CurY, 0, 0, pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Add_Amount").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                    CurY = CurY + TxtHgt
                    e.Graphics.DrawLine(Pens.Black, LMargin + C1 - 100, CurY, LMargin + C1 - 10, CurY)

                    e.Graphics.DrawLine(Pens.Black, LMargin + C2 + 10, CurY - 5, PageWidth - 10, CurY - 5)

                    CurY = CurY + 8
                    Common_Procedures.Print_To_PrintDocument(e, "eF ªî£¬è ", LMargin + 10, CurY, 0, 0, pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Net_Amount").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)


                    W1 = e.Graphics.MeasureString("dv â‡E‚¬è : ", pFont).Width
                    W2 = e.Graphics.MeasureString("Ë™ ªêô¾ : ", pFont).Width

                    Common_Procedures.Print_To_PrintDocument(e, "dv â‡E‚¬è ", LMargin + C1 + 10, CurY, 0, 0, pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, " :  " & prn_HdDt.Rows(prn_HeadIndx).Item("Pcs").ToString, LMargin + C1 + W1 + 80, CurY, 0, 0, pFont)


                    Common_Procedures.Print_To_PrintDocument(e, "Ë™ ªêô¾ ", LMargin + C1 + 250, CurY, 0, 0, pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, " :  " & prn_HdDt.Rows(prn_HeadIndx).Item("Total_Dgv_Weight").ToString, LMargin + C1 + 250 + W2 + 10, CurY, 0, 0, pFont)

                    CurY = CurY + TxtHgt + 8

                    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                    LnAr(4) = CurY
                    e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(3))

                Else
                    NoofItems_PerPage = 14

                End If

                CurY = CurY + TxtHgt - 10
                Common_Procedures.Print_To_PrintDocument(e, "«îF", LMargin, CurY, 2, ClAr(1), pTFont)
                Common_Procedures.Print_To_PrintDocument(e, "ªï‹.", LMargin + ClAr(1), CurY, 2, ClAr(2), pTFont)
                Common_Procedures.Print_To_PrintDocument(e, "d‹ õ/ð", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pTFont)
                Common_Procedures.Print_To_PrintDocument(e, "d‹ Þ¼", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pTFont)
                Common_Procedures.Print_To_PrintDocument(e, "ð£¾ õ/ð", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pTFont)
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1009" Then
                    Common_Procedures.Print_To_PrintDocument(e, "ð£¾ Þ¼", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pTFont)
                End If
                Common_Procedures.Print_To_PrintDocument(e, "Ë™ õ/ð", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pTFont)
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1009" Then
                    Common_Procedures.Print_To_PrintDocument(e, "Ë™ Þ¼", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pTFont)
                End If
                Common_Procedures.Print_To_PrintDocument(e, "ªî£¬è õ/ð", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pTFont)
                Common_Procedures.Print_To_PrintDocument(e, "ªî£¬è Þ¼", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pTFont)

                CurY = CurY + TxtHgt
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
                            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1009" Then
                                If prn_DetIndx <> 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("PavuMtrs").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)
                                End If
                            Else

                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("PavuMtrs").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)
                            End If
                            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1009" Then
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_Tot_Pavu_Stk), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)
                            End If
                        End If
                        If Val(prn_DetDt.Rows(prn_DetIndx).Item("YarnWgt").ToString) <> 0 Then
                            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1009" Then
                                If prn_DetIndx <> 0 Then
                                    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("YarnWgt").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
                                End If
                            Else

                                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("YarnWgt").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
                            End If

                            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1009" Then
                                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_Tot_Yarn_Stk), "#########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, pFont)
                            End If
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
        Dim strHeight As Single, strWIDTH As Single
        Dim C1, C2, C3, S1, W1, W2 As Single
        Dim vPREVBILLNO As String = ""
        Dim vPREVBILLDATE As String = ""
        Dim Y1 As Single = 0, Y2 As Single = 0

        PageNo = PageNo + 1

        CurY = TMargin


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""

        If Val(Common_Procedures.settings.WeaverWages_Print_Weavers_Name_IN_Heading) = 1 Then
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Name").ToString
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





        CurY = CurY + TxtHgt - 15


        If Val(Common_Procedures.settings.WeaverWages_Print_Dont_Show_Company_Heading) <> 1 Then '---- Arul Kumaran Textiles (Somanur)
            p1Font = New Font("Calibri", 18, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Then '---- M.S Textiles (Tirupur)

                p1Font = New Font("Calibri", 12, FontStyle.Bold)
                strWIDTH = e.Graphics.MeasureString(Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_No").ToString), p1Font).Width

                Y1 = CurY + 0.5
                Y2 = CurY + strHeight - 1
                Dim br = New SolidBrush(Color.FromArgb(110, 110, 110))
                Common_Procedures.FillRegionRectangle(e, PageWidth - strWIDTH - 30, Y1, PageWidth - 10, Y2, br)

                br = New SolidBrush(Color.FromArgb(255, 255, 255))
                Common_Procedures.Print_To_PrintDocument(e, Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_No").ToString), PageWidth - 20, CurY + 8, 1, 0, p1Font, br)

            End If


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


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1009" Then
            C2 = 560 ' C1 + ClAr(5) + 70 + 70 + 70
            C3 = 420 'ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 70 + 20
        Else
            C2 = C1 + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + 20
            C3 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + 20
        End If


        S1 = e.Graphics.MeasureString("TO   :    ", pFont).Width
        W1 = e.Graphics.MeasureString("óC¶ ªï‹.   ", pFont).Width
        W2 = e.Graphics.MeasureString("óC¶ «îF   ", pFont).Width

        If Val(Common_Procedures.settings.WeaverWages_Print_Weavers_Name_IN_Heading) = 1 Then
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
            Led_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Name").ToString
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

        If Val(prn_WagesDontShowToPartyName) = 1 Then
            CurY = CurY + TxtHgt
            'pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
            p1Font = New Font("TAM_SC_Suvita", 9, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "H™ ªï‹. ", LMargin + 10, CurY + 5, 0, 0, p1Font)
            If Val(Common_Procedures.settings.WeaverWages_Print_Weavers_BillNo_SeparateSlNo) = 1 Then
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_BillNo").ToString, LMargin + W1 + 20, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_No").ToString, LMargin + W1 + 20, CurY, 0, 0, pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "H™ «îF ", LMargin + 140 + 10, CurY + 5, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Date").ToString), "dd-MM-yyyy").ToString, LMargin + 140 + W2 + 20, CurY, 0, 0, pFont)


            p1Font = New Font("TAM_SC_Suvita", 9, FontStyle.Bold)
            If Val(Common_Procedures.settings.WeaverWages_Print_Weavers_BillNo_SeparateSlNo) = 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "ªìLõK ªï‹.", LMargin + C3 - 65, CurY + 5, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_No").ToString, LMargin + C3 - 65 + W1 + 40, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "óC¶ ªï‹. ", LMargin + C3 - 65, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_No").ToString, LMargin + C3 - 65 + W1 + 30, CurY, 0, 0, pFont)
            End If

            If Val(Common_Procedures.settings.WeaverWages_Print_Weavers_BillNo_SeparateSlNo) = 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "ªìLõK «îF", LMargin + C2 - 20, CurY + 5, 0, 0, p1Font)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "óC¶ «îF ", LMargin + C2 - 20, CurY + 5, 0, 0, p1Font)
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

            p1Font = New Font("TAM_SC_Suvita", 8, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "H™ ªï‹. ", LMargin + C3 + 10, CurY + 5, 0, 0, p1Font)
            If Val(Common_Procedures.settings.WeaverWages_Print_Weavers_BillNo_SeparateSlNo) = 1 Then
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_BillNo").ToString, LMargin + C3 + W1 + 30, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_No").ToString, LMargin + C3 + W1 + 30, CurY, 0, 0, pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "H™ «îF ", LMargin + C2 + 10, CurY + 5, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C2 + W2 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 9, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, " " & Led_Add, LMargin + S1 + 10, CurY, 0, 0, p1Font)

            p1Font = New Font("TAM_SC_Suvita", 8, FontStyle.Bold)
            If Val(Common_Procedures.settings.WeaverWages_Print_Weavers_BillNo_SeparateSlNo) = 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "ªìLõK ªï‹.", LMargin + C3 + 10, CurY + 5, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_No").ToString, LMargin + C3 + W1 + 40, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "ªìLõK «îF", LMargin + C2 + 10, CurY + 5, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Rec_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C2 + W2 + 40, CurY, 0, 0, pFont)

            Else
                Common_Procedures.Print_To_PrintDocument(e, "óC¶ ªï‹. ", LMargin + C3 + 10, CurY + 5, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_No").ToString, LMargin + C3 + W1 + 30, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "óC¶ «îF ", LMargin + C2 + 10, CurY + 5, 0, 0, p1Font)
                'Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Rec_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C2 + W2 + 30, CurY, 0, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_Date").ToString, LMargin + C2 + W2 + 30, CurY, 0, 0, pFont)

            End If

            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY
            LnAr(4) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + C3, CurY, LMargin + C3, LnAr(2))

        End If

    End Sub

    Private Sub Printing_Format2_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal pTFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
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
        Dim vPREVBILLNO_DATE As String = ""

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY + 5, PageWidth, CurY + 5)
        LnAr(6) = CurY

        CurY = CurY + 5

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

            da1 = New SqlClient.SqlDataAdapter("select sum(int6) from " & Trim(Common_Procedures.ReportTempTable) & " where name6 = 'BEAM'", con)
            Dt1 = New DataTable
            da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                    Tot_EBeam_StkSumry = Val(Dt1.Rows(0)(0).ToString)
                End If
            End If
            Dt1.Clear()

            da1 = New SqlClient.SqlDataAdapter("select sum(currency1) from " & Trim(Common_Procedures.ReportTempTable) & " where name6 = 'AMOUNT'", con)
            Dt1 = New DataTable
            da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                    prn_Tot_Amt_BalSumry = Val(Dt1.Rows(0)(0).ToString)
                End If
            End If
            Dt1.Clear()

            da1 = New SqlClient.SqlDataAdapter("select name7 as EndsCount, sum(meters6) as PavuMtrs from " & Trim(Common_Procedures.ReportTempTable) & " where name6 = 'PAVU' GROUP BY name7 having sum(meters6) <> 0 ORDER BY name7", con)
            Dt1 = New DataTable
            da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                For k = 0 To Dt1.Rows.Count - 1
                    prn_Tot_Pavu_StkSumry = Trim(prn_Tot_Pavu_StkSumry) & IIf(Trim(prn_Tot_Pavu_StkSumry) <> "", ", ", "") & Dt1.Rows(k).Item("EndsCount").ToString & " : " & Dt1.Rows(k).Item("PavuMtrs").ToString
                Next
            End If
            Dt1.Clear()

            da1 = New SqlClient.SqlDataAdapter("select name7 as CountName, sum(weight1) as YarnWgt from " & Trim(Common_Procedures.ReportTempTable) & " where name6 = 'YARN' GROUP BY name7 having sum(weight1) <> 0 ORDER BY name7", con)
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

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1009" Then '---- Nithya Textiles (Sedapalayam)


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
            End If

        Else
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt

        End If

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1040" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1055" Then '---- M.S Textiles (Tirupur)
            Common_Procedures.Print_To_PrintDocument(e, "îPè£ó˜ ¬èªò£Šð‹  ", LMargin + 10, CurY, 0, 0, pTFont)

        End If

        If Val(Common_Procedures.settings.WeaverWages_Print_Weavers_Name_IN_Heading) = 1 Then
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Name").ToString
        Else
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString
        End If

        If Val(Common_Procedures.settings.WeaverWages_Print_Dont_Show_Company_Heading) = 1 Then
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 15, CurY, 1, 0, pFont)

        Else
            p1Font = New Font("Calibri", 12, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        End If



        vPREVBILLNO_DATE = ""
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Then '---- M.S Textiles (Tirupur)

            cmd.Connection = con

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@WagesDate", prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Date"))

            cmd.CommandText = "select top 1 a.* from Weaver_Wages_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Ledger_IdNo = " & Str(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_IdNo").ToString)) & " and (a.Weaver_Wages_Date < @WagesDate or ( a.Weaver_Wages_Date = @WagesDate and a.for_orderby < " & Str(Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("for_orderby").ToString), "########0.00")) & " ) ) Order by a.Weaver_Wages_Date Desc, a.for_orderby desc, a.Weaver_Wages_No Desc, a.Weaver_Wages_Code Desc"
            Da1 = New SqlClient.SqlDataAdapter(cmd)
            Dt1 = New DataTable
            Da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                vPREVBILLNO_DATE = Dt1.Rows(0).Item("Weaver_Wages_No").ToString
                vPREVBILLNO_DATE = Trim(vPREVBILLNO_DATE) & " / " & Format(Convert.ToDateTime(Dt1.Rows(0).Item("Weaver_Wages_Date").ToString), "dd-MM-yyyy").ToString
            End If
            Dt1.Clear()

            If Trim(vPREVBILLNO_DATE) <> "" Then
                p1Font = New Font("Calibri", 8, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "Prev.RefNo. : " & vPREVBILLNO_DATE, LMargin + 10, CurY, 0, 0, p1Font)
            End If

        End If

        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

    End Sub
    Private Sub Printing_Format4(ByRef e As System.Drawing.Printing.PrintPageEventArgs) ' common Format with Stock
        Dim pFont As Font
        Dim p1Font As Font
        Dim pTFont As Font
        'Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        'Dim I As Integer
        Dim NoofItems_PerPage As Integer, NoofDets As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim C1 As Single = 0, C2 As Single = 0, C3 As Single = 0
        Dim C4 As Single = 0, C5 As Single = 0, C6 As Single = 0
        Dim W1, W2 As Single
        Dim snd, sec, bit, rjt, otr As Single
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim count_val As Single
        Dim yn_Kgs As Double = 0

        If prn_PageSize_SetUP_STS = False Then
            set_PaperSize_For_PrintDocument1()
            prn_PageSize_SetUP_STS = True
        End If


        p1Font = New Font("Calibri", 11, FontStyle.Bold)


        With PrintDocument1.DefaultPageSettings.Margins
            .Left = 30
            .Right = 50
            .Top = 1 ' 30
            .Bottom = 25 ' 30
            LMargin = .Left
            RMargin = .Right
            TMargin = .Top
            BMargin = .Bottom
        End With

        pFont = New Font("Calibri", 10, FontStyle.Regular)
        pTFont = New Font("TAM_SC_Suvita", 9, FontStyle.Bold)
        p1Font = New Font("TAM_SC_Suvita", 10, FontStyle.Bold)

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

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1009" Then

            ClAr(1) = Val(110) : ClAr(2) = 80 : ClAr(3) = 85 : ClAr(4) = 85 : ClAr(5) = 90 : ClAr(6) = 0 : ClAr(7) = 90 : ClAr(8) = 0 : ClAr(9) = 100
            ClAr(10) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9))

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 140 '285
            C2 = C1 + ClAr(5)  '385

        Else
            ClAr(1) = Val(80) : ClAr(2) = 50 : ClAr(3) = 55 : ClAr(4) = 55 : ClAr(5) = 80 : ClAr(6) = 80 : ClAr(7) = 70 : ClAr(8) = 70 : ClAr(9) = 100
            ClAr(10) = PageWidth - (LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9))

            C1 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + 40 '285
            C2 = C1 + ClAr(5)  '385

        End If


        '  If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Then
        TxtHgt = 17
        '  Else
        '  TxtHgt = 17.25 ' 18  ' 18.5
        '  End If

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_Prev_HeadIndx = prn_HeadIndx

        Try

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format4_PageHeader(e, EntryCode, TxtHgt, pFont, pTFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = NoofDets + 1
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1009" Then
                    NoofItems_PerPage = 9
                Else
                    NoofItems_PerPage = 7
                End If


                If prn_PageNo <= 1 Then

                    CurY = CurY + TxtHgt - 10
                    Common_Procedures.Print_To_PrintDocument(e, "óè‹ ", LMargin + 10, CurY, 0, 0, pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Cloth_Name").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                    snd = Val(prn_HdDt.Rows(prn_HeadIndx).Item("Sound_Meters").ToString) * Val(prn_HdDt.Rows(prn_HeadIndx).Item("Sound_cooly").ToString)
                    sec = Val(prn_HdDt.Rows(prn_HeadIndx).Item("Seconds_Meters").ToString) * Val(prn_HdDt.Rows(prn_HeadIndx).Item("Seconds_cooly").ToString)
                    bit = Val(prn_HdDt.Rows(prn_HeadIndx).Item("Bits_Meters").ToString) * Val(prn_HdDt.Rows(prn_HeadIndx).Item("Bits_cooly").ToString)
                    rjt = Val(prn_HdDt.Rows(prn_HeadIndx).Item("Reject_Meters").ToString) * Val(prn_HdDt.Rows(prn_HeadIndx).Item("Reject_cooly").ToString)
                    otr = Val(prn_HdDt.Rows(prn_HeadIndx).Item("Others_Meters").ToString) * Val(prn_HdDt.Rows(prn_HeadIndx).Item("Others_cooly").ToString)

                    '  Common_Procedures.Print_To_PrintDocument(e, "ê¾‡† ", LMargin + C1 + 10, CurY, 0, 0, pTFont)
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1049" Then
                        pFont = New Font("Calibri", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, "I Quality", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "ê¾‡† ", LMargin + C1 + 10, CurY, 0, 0, p1Font)
                    End If
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(snd), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " = ", PageWidth - 80, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Sound_Cooly").ToString, PageWidth - 100, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " X ", PageWidth - 170, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Sound_Meters").ToString, PageWidth - 190, CurY, 1, 0, pFont)

                    CurY = CurY + TxtHgt
                    'Common_Procedures.Print_To_PrintDocument(e, "õó¾ e†ì˜ ", LMargin + 10, CurY, 0, 0, pTFont)
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1049" Then
                        pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, "ð£¾ e†ì˜ ", LMargin + 10, CurY, 0, 0, p1Font)
                        'pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                        ' Common_Procedures.Print_To_PrintDocument(e, "õó¾ e†ì˜ ", LMargin + 10, CurY, 0, 0, p1Font)
                        pFont = New Font("Calibri", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Receipt_Meters").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)
                    Else
                        pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, "ð£¾ e†ì˜ ", LMargin + 10, CurY, 0, 0, p1Font)
                        pFont = New Font("Calibri", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Receipt_Meters").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)
                    End If
                    '  Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Receipt_Meters").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                    pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1049" Then
                        pFont = New Font("Calibri", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, "II Quality", LMargin + C1 + 10, CurY, 0, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "ªêè‡†v ", LMargin + C1 + 10, CurY, 0, 0, p1Font)
                    End If
                    pFont = New Font("Calibri", 11, FontStyle.Regular)
                    ' Common_Procedures.Print_To_PrintDocument(e, "ªêè‡†v ", LMargin + C1 + 10, CurY, 0, 0, pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(sec), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " = ", PageWidth - 80, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Seconds_Cooly").ToString, PageWidth - 100, CurY, 1, 0, pFont)
                    pFont = New Font("Calibri", 11, FontStyle.Regular)
                    Common_Procedures.Print_To_PrintDocument(e, " X ", PageWidth - 170, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Seconds_Meters").ToString, PageWidth - 190, CurY, 1, 0, pFont)

                    CurY = CurY + TxtHgt
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1049" Then
                        Common_Procedures.Print_To_PrintDocument(e, "ªñ£ˆî e†ì˜ ", LMargin + 10, CurY, 0, 0, pTFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_Meters").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                    Else
                        Common_Procedures.Print_To_PrintDocument(e, "õ÷K e†ì˜ ", LMargin + 10, CurY, 0, 0, pTFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Excess_Short").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)
                    End If

                    If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Bits_Meters").ToString) <> 0 Then
                        ' Common_Procedures.Print_To_PrintDocument(e, "îQ óè‹ ", LMargin + C1 + 10, CurY, 0, 0, pTFont)

                        pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1049" Then
                            Common_Procedures.Print_To_PrintDocument(e, "H†", LMargin + C1 + 10, CurY, 0, 0, pTFont)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, "îQ óè‹ ", LMargin + C1 + 10, CurY, 0, 0, pTFont)
                        End If
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(bit), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, " = ", PageWidth - 80, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Bits_Cooly").ToString, PageWidth - 100, CurY, 1, 0, pFont)
                        pFont = New Font("Calibri", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, " X ", PageWidth - 170, CurY, 1, 0, pFont)
                        pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Bits_Meters").ToString, PageWidth - 190, CurY, 1, 0, pFont)
                    End If

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1049" Then
                        ' CurY = CurY + TxtHgt
                        If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Scecondsless_Meter").ToString) <> 0 Then
                            CurY = CurY + TxtHgt
                            pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                            Common_Procedures.Print_To_PrintDocument(e, "ªêè‡†v ", LMargin + C1 + 10, CurY, 0, 0, p1Font)
                            pFont = New Font("Calibri", 11, FontStyle.Regular)
                            Common_Procedures.Print_To_PrintDocument(e, "-" & Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Scecondsless_Rate").ToString) * (prn_HdDt.Rows(prn_HeadIndx).Item("Scecondsless_Meter").ToString), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, " = ", PageWidth - 80, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, "-" & prn_HdDt.Rows(prn_HeadIndx).Item("Scecondsless_Rate").ToString, PageWidth - 100, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, " X ", PageWidth - 170, CurY, 1, 0, pFont)
                            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Scecondsless_Meter").ToString, PageWidth - 190, CurY, 1, 0, pFont)
                        End If
                    End If

                    CurY = CurY + TxtHgt
                    pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                    Common_Procedures.Print_To_PrintDocument(e, "ªñ£ˆî ªî£¬è ", LMargin + 10, CurY, 0, 0, p1Font)
                    pFont = New Font("Calibri", 11, FontStyle.Regular)
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1049" Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Cooly").ToString - prn_HdDt.Rows(prn_HeadIndx).Item("Less_Amount").ToString), LMargin + C1 - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_cooly").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)
                    End If

                    'CurY = CurY + TxtHgt
                    'Common_Procedures.Print_To_PrintDocument(e, "ªñ£ˆî ªî£¬è ", LMargin + 10, CurY, 0, 0, pTFont)
                    'Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_cooly").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                    If Val(rjt) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "èN¾ ", LMargin + C1 + 10, CurY, 0, 0, pTFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(rjt), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, " = ", PageWidth - 80, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Reject_Cooly").ToString, PageWidth - 100, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, " X ", PageWidth - 170, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Reject_Meters").ToString, PageWidth - 190, CurY, 1, 0, pFont)
                    End If

                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "õ‡® õ£ì¬è ", LMargin + 10, CurY, 0, 0, pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Freight_Charge").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                    If Val(otr) <> 0 Then
                        Common_Procedures.Print_To_PrintDocument(e, "Þîó ", LMargin + C1 + 10, CurY, 0, 0, pTFont)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(otr), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, " = ", PageWidth - 80, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Others_Cooly").ToString, PageWidth - 100, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, " X ", PageWidth - 170, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Others_Meters").ToString, PageWidth - 190, CurY, 1, 0, pFont)
                    End If

                    CurY = CurY + TxtHgt

                    Common_Procedures.Print_To_PrintDocument(e, "H®ˆî‹ ", LMargin + 10, CurY, 0, 0, pTFont)

                    pFont = New Font("Calibri", 11, FontStyle.Regular)
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1049" Then
                        Common_Procedures.Print_To_PrintDocument(e, "0.00", LMargin + C1 - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Less_Amount").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)
                    End If
                    ' Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Less_Amount").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                    e.Graphics.DrawLine(Pens.Black, LMargin + C2 + 10, CurY + 5, PageWidth - 10, CurY + 5)


                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "õK H®ˆî‹ ", LMargin + 10, CurY, 0, 0, pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Tds_Perc_Calc").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                    Common_Procedures.Print_To_PrintDocument(e, "ªñ£ˆî‹ ", LMargin + C1 + 10, CurY, 0, 0, pTFont)

                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_Meters").ToString, PageWidth - 190, CurY, 1, 0, pFont)
                    '  Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_Cooly").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1049" Then
                        Common_Procedures.Print_To_PrintDocument(e, Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Cooly").ToString - prn_HdDt.Rows(prn_HeadIndx).Item("Less_Amount").ToString), PageWidth - 10, CurY, 1, 0, pFont)
                    Else
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_Cooly").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                    End If

                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "Ü†õ£¡v ", LMargin + 10, CurY, 0, 0, pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Add_Amount").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                    CurY = CurY + TxtHgt
                    e.Graphics.DrawLine(Pens.Black, LMargin + C1 - 100, CurY, LMargin + C1 - 10, CurY)

                    e.Graphics.DrawLine(Pens.Black, LMargin + C2 + 10, CurY - 5, PageWidth - 10, CurY - 5)

                    CurY = CurY + 8
                    Common_Procedures.Print_To_PrintDocument(e, "eF ªî£¬è ", LMargin + 10, CurY, 0, 0, pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Net_Amount").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)


                    W1 = e.Graphics.MeasureString("dv â‡E‚¬è : ", pFont).Width
                    W2 = e.Graphics.MeasureString("Ë™ ªêô¾ : ", pFont).Width

                    count_val = 0
                    count_val = Thiri_ToKgs_Count(Trim((prn_HdDt.Rows(prn_HeadIndx).Item("Cloth_Name").ToString())))
                    yn_Kgs = 0

                    If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Dgv_Weight").ToString) <> 0 Then
                        ' yn_Kgs = Format(count_val * 11 / 50 * .Rows(.CurrentRow.Index).Cells(6).Value, "##########0.000")
                        yn_Kgs = Format((Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Dgv_Weight").ToString) / count_val) * 50 / 11, "##########0.000")

                    End If

                    'Common_Procedures.Print_To_PrintDocument(e, "dv â‡E‚¬è ", LMargin + C1 + 10, CurY, 0, 0, pTFont)
                    'Common_Procedures.Print_To_PrintDocument(e, " :  " & prn_HdDt.Rows(prn_HeadIndx).Item("Pcs").ToString, LMargin + C1 + W1 + 80, CurY, 0, 0, pFont)

                    Common_Procedures.Print_To_PrintDocument(e, "Ë™ ªêô¾ ( FK ) ", LMargin + C1 + 10, CurY, 0, 0, pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, " : " & prn_HdDt.Rows(prn_HeadIndx).Item("Total_Dgv_Weight").ToString, LMargin + C1 + W1 + 50, CurY, 0, 0, pFont)

                    Common_Procedures.Print_To_PrintDocument(e, "Ë™ ªêô¾( A«ô£ )", LMargin + C1 + 230, CurY, 0, 0, pTFont)
                    Common_Procedures.Print_To_PrintDocument(e, " : " & Val(yn_Kgs), PageWidth - 10, CurY, 1, 0, pFont)

                    'Common_Procedures.Print_To_PrintDocument(e, "Ë™ ªêô¾ ", LMargin + C1 + 250, CurY, 0, 0, pTFont)
                    'Common_Procedures.Print_To_PrintDocument(e, " :  " & prn_HdDt.Rows(prn_HeadIndx).Item("Total_Dgv_Weight").ToString, LMargin + C1 + 250 + W2 + 10, CurY, 0, 0, pFont)

                    CurY = CurY + TxtHgt + 8

                    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                    LnAr(4) = CurY
                    e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(3))

                Else
                    NoofItems_PerPage = 15

                End If




                '    CurY = CurY + TxtHgt - 10
                '    Common_Procedures.Print_To_PrintDocument(e, "«îF", LMargin, CurY, 2, ClAr(1), pTFont)
                '    Common_Procedures.Print_To_PrintDocument(e, "ªï‹.", LMargin + ClAr(1), CurY, 2, ClAr(2), pTFont)
                '    Common_Procedures.Print_To_PrintDocument(e, "d‹ õ/ð", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pTFont)
                '    Common_Procedures.Print_To_PrintDocument(e, "d‹ Þ¼", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pTFont)
                '    Common_Procedures.Print_To_PrintDocument(e, "ð£¾ õ/ð", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pTFont)
                '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1009" Then
                '        Common_Procedures.Print_To_PrintDocument(e, "ð£¾ Þ¼", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pTFont)
                '    End If
                '    Common_Procedures.Print_To_PrintDocument(e, "Ë™ õ/ð", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, 2, ClAr(7), pTFont)
                '    If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1009" Then
                '        Common_Procedures.Print_To_PrintDocument(e, "Ë™ Þ¼", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, 2, ClAr(8), pTFont)
                '    End If
                '    Common_Procedures.Print_To_PrintDocument(e, "ªî£¬è õ/ð", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, 2, ClAr(9), pTFont)
                '    Common_Procedures.Print_To_PrintDocument(e, "ªî£¬è Þ¼", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, 2, ClAr(10), pTFont)

                '    CurY = CurY + TxtHgt
                '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                '    LnAr(5) = CurY


                '    NoofDets = 0

                '    CurY = CurY - 10

                '    If prn_DetDt.Rows.Count > 0 Then

                '        Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

                '            If NoofDets >= NoofItems_PerPage Then

                '                CurY = CurY + TxtHgt

                '                Common_Procedures.Print_To_PrintDocument(e, "Continued....", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 10, CurY, 1, 0, pFont)

                '                NoofDets = NoofDets + 1

                '                If Val(Common_Procedures.settings.WeaverWages_Print_NoNeed_2nd_Page) = 1 Then
                '                    Printing_Format4_PageFooter(e, EntryCode, TxtHgt, pFont, pTFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

                '                    GoTo LOOP2

                '                Else

                '                    Printing_Format4_PageFooter(e, EntryCode, TxtHgt, pFont, pTFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, False)

                '                    e.HasMorePages = True

                '                    Return

                '                End If

                '            End If

                '            prn_Tot_EBeam_Stk = prn_Tot_EBeam_Stk + Val(prn_DetDt.Rows(prn_DetIndx).Item("EmptyBeam").ToString)
                '            prn_Tot_Pavu_Stk = prn_Tot_Pavu_Stk + Val(prn_DetDt.Rows(prn_DetIndx).Item("PavuMtrs").ToString)
                '            prn_Tot_Yarn_Stk = prn_Tot_Yarn_Stk + Val(prn_DetDt.Rows(prn_DetIndx).Item("YarnWgt").ToString)
                '            prn_Tot_Amt_Bal = prn_Tot_Amt_Bal + Val(prn_DetDt.Rows(prn_DetIndx).Item("amount").ToString)

                '            CurY = CurY + TxtHgt
                '            If IsDate(prn_DetDt.Rows(prn_DetIndx).Item("Date1").ToString) = True Then
                '                Common_Procedures.Print_To_PrintDocument(e, Format(Convert.ToDateTime(prn_DetDt.Rows(prn_DetIndx).Item("Date1").ToString), "dd-MM-yy").ToString, LMargin + 5, CurY, 0, 0, pFont)
                '            Else

                '                If Val(prn_DetDt.Rows(prn_DetIndx).Item("Ent_OrderBy").ToString) = 0 Then
                '                    Common_Procedures.Print_To_PrintDocument(e, "Opening", LMargin + 5, CurY, 0, 0, pFont)
                '                End If

                '            End If

                '            Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("Dc_Rec_No").ToString, LMargin + ClAr(1) + 5, CurY, 0, 0, pFont)
                '            If Val(prn_DetDt.Rows(prn_DetIndx).Item("EmptyBeam").ToString) <> 0 Then
                '                Common_Procedures.Print_To_PrintDocument(e, Val(prn_DetDt.Rows(prn_DetIndx).Item("EmptyBeam").ToString), LMargin + ClAr(1) + ClAr(2) + ClAr(3) - 5, CurY, 1, 0, pFont)
                '                Common_Procedures.Print_To_PrintDocument(e, Val(prn_Tot_EBeam_Stk), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) - 5, CurY, 1, 0, pFont)
                '            End If
                '            If Val(prn_DetDt.Rows(prn_DetIndx).Item("PavuMtrs").ToString) <> 0 Then
                '                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1009" Then
                '                    If prn_DetIndx <> 0 Then
                '                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("PavuMtrs").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)
                '                    End If
                '                Else

                '                    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("PavuMtrs").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 5, CurY, 1, 0, pFont)
                '                End If
                '                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1009" Then
                '                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_Tot_Pavu_Stk), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) - 5, CurY, 1, 0, pFont)
                '                End If
                '            End If
                '            If Val(prn_DetDt.Rows(prn_DetIndx).Item("YarnWgt").ToString) <> 0 Then
                '                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1009" Then
                '                    If prn_DetIndx <> 0 Then
                '                        Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("YarnWgt").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
                '                    End If
                '                Else

                '                    Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("YarnWgt").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) - 5, CurY, 1, 0, pFont)
                '                End If

                '                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1009" Then
                '                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_Tot_Yarn_Stk), "#########0.000"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) - 5, CurY, 1, 0, pFont)
                '                End If
                '            End If
                '            If Val(prn_DetDt.Rows(prn_DetIndx).Item("amount").ToString) <> 0 Then
                '                Common_Procedures.Print_To_PrintDocument(e, prn_DetDt.Rows(prn_DetIndx).Item("amount").ToString, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) - 5, CurY, 1, 0, pFont)
                '                Common_Procedures.Print_To_PrintDocument(e, Format(Val(prn_Tot_Amt_Bal), "#########0.00"), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10) - 5, CurY, 1, 0, pFont)
                '            End If

                '            NoofDets = NoofDets + 1

                '            prn_DetIndx = prn_DetIndx + 1

                '        Loop

                '    End If

            End If

LOOP1:
            CurY = CurY + TxtHgt

            Printing_Format4_PageFooter(e, EntryCode, TxtHgt, pFont, pTFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

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

    Private Function Thiri_ToKgs_Count(ByVal vCloth_Name As String) As Integer

        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim count_val As Single
        Dim CntID As Integer
        Dim yn_Kgs As Double = 0

        CntID = Val(Common_Procedures.get_FieldValue(con, "Cloth_Head", "Cloth_WeftCount_IdNo", "(Cloth_Name = '" & Trim(vCloth_Name) & "')", , ))

        count_val = 0

        'If Common_Procedures.settings.Weaver_YarnStock_InThiri_Status = 1 Then

        Da = New SqlClient.SqlDataAdapter("select (Resultant_Count) from Count_Head where count_idno = " & Str(Val(CntID)), con)
        Da.Fill(Dt)

        If Dt.Rows.Count > 0 Then
            If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
                count_val = Dt.Rows(0).Item("Resultant_Count").ToString
            End If
        End If

        Dt.Clear()
        Dt.Dispose()
        Da.Dispose()

        Thiri_ToKgs_Count = count_val

    End Function

    Private Sub Printing_Format4_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal pTFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
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

        If Val(Common_Procedures.settings.WeaverWages_Print_Weavers_Name_IN_Heading) = 1 Then
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Name").ToString
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

        CurY = CurY + TxtHgt - 15

        If Val(Common_Procedures.settings.WeaverWages_Print_Dont_Show_Company_Heading) <> 1 Then '---- Arul Kumaran Textiles (Somanur)
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


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1009" Then
            C2 = 560 ' C1 + ClAr(5) + 70 + 70 + 70
            C3 = 420 'ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + 70 + 20
        Else
            C2 = C1 + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8)
            C3 = ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6)
        End If


        S1 = e.Graphics.MeasureString("TO   :    ", pFont).Width
        W1 = e.Graphics.MeasureString("óC¶ ªï‹.   ", pFont).Width
        W2 = e.Graphics.MeasureString("óC¶ «îF  ", pFont).Width

        If Val(Common_Procedures.settings.WeaverWages_Print_Weavers_Name_IN_Heading) = 1 Then
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
            Led_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Name").ToString
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

        If Val(prn_WagesDontShowToPartyName) = 1 Then
            CurY = CurY + TxtHgt
            'pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
            p1Font = New Font("TAM_SC_Suvita", 9, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "H™ ªï‹. ", LMargin + 10, CurY + 5, 0, 0, p1Font)
            If Val(Common_Procedures.settings.WeaverWages_Print_Weavers_BillNo_SeparateSlNo) = 1 Then
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_BillNo").ToString, LMargin + W1 + 20, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_No").ToString, LMargin + W1 + 20, CurY, 0, 0, pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "H™ «îF ", LMargin + 140 + 10, CurY + 5, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Date").ToString), "dd-MM-yyyy").ToString, LMargin + 140 + W2 + 20, CurY, 0, 0, pFont)


            p1Font = New Font("TAM_SC_Suvita", 9, FontStyle.Bold)
            If Val(Common_Procedures.settings.WeaverWages_Print_Weavers_BillNo_SeparateSlNo) = 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "ªìLõK ªï‹.", LMargin + C3 - 65, CurY + 5, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_No").ToString, LMargin + C3 - 65 + W1 + 40, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "óC¶ ªï‹. ", LMargin + C3 - 65, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_No").ToString, LMargin + C3 - 65 + W1 + 30, CurY, 0, 0, pFont)
            End If

            If Val(Common_Procedures.settings.WeaverWages_Print_Weavers_BillNo_SeparateSlNo) = 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "ªìLõK «îF", LMargin + C2 - 20, CurY + 5, 0, 0, p1Font)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "óC¶ «îF ", LMargin + C2 - 20, CurY + 5, 0, 0, p1Font)
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

            p1Font = New Font("TAM_SC_Suvita", 8, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "H™ ªï‹. ", LMargin + C3 + 10, CurY + 5, 0, 0, p1Font)
            If Val(Common_Procedures.settings.WeaverWages_Print_Weavers_BillNo_SeparateSlNo) = 1 Then
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_BillNo").ToString, LMargin + C3 + W1 + 30, CurY, 0, 0, pFont)
            Else
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_No").ToString, LMargin + C3 + W1 + 30, CurY, 0, 0, pFont)
            End If

            Common_Procedures.Print_To_PrintDocument(e, "H™ «îF ", LMargin + C2 + 10, CurY + 5, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C2 + W2 + 30, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt
            p1Font = New Font("Calibri", 9, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, " " & Led_Add, LMargin + S1 + 10, CurY, 0, 0, p1Font)

            p1Font = New Font("TAM_SC_Suvita", 8, FontStyle.Bold)
            If Val(Common_Procedures.settings.WeaverWages_Print_Weavers_BillNo_SeparateSlNo) = 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "ªìLõK ªï‹.", LMargin + C3 + 10, CurY + 5, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_No").ToString, LMargin + C3 + W1 + 40, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "ªìLõK «îF", LMargin + C2 + 10, CurY + 5, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Rec_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C2 + W2 + 40, CurY, 0, 0, pFont)

            Else
                Common_Procedures.Print_To_PrintDocument(e, "óC¶ ªï‹. ", LMargin + C3 + 10, CurY + 5, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_No").ToString, LMargin + C3 + W1 + 30, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "óC¶ «îF ", LMargin + C2 + 10, CurY + 5, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Rec_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C2 + W2 + 30, CurY, 0, 0, pFont)

            End If


            CurY = CurY + TxtHgt + 10

            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(3) = CurY
            LnAr(4) = CurY
            e.Graphics.DrawLine(Pens.Black, LMargin + C3, CurY, LMargin + C3, LnAr(2))

        End If

    End Sub

    Private Sub Printing_Format4_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal pTFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim I As Integer = 0
        Dim k As Integer = 0
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim S1, C1, C2 As Single
        Dim Tot_EBeam_StkSumry As Single = 0
        Dim prn_Tot_Pavu_StkSumry As String = ""
        Dim prn_Tot_Yarn_StkSumry As String = ""
        Dim prn_Tot_Amt_BalSumry As Single = 0
        Dim prn_Tot_Yarn_StkSumry_Kg As String = ""
        Dim Count_Val As Double = 0
        Dim Yn_Kg As Double = 0
        Dim ItmNm1 As String, ItmNm2 As String
        Dim ItmNm3 As String, ItmNm4 As String

        'For i = NoofDets + 1 To NoofItems_PerPage
        '    CurY = CurY + TxtHgt
        'Next

        ' e.Graphics.DrawLine(Pens.Black, LMargin, CurY + 5, PageWidth, CurY + 5)
        '  LnAr(6) = CurY

        'CurY = CurY + 5

        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(4))
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(4))
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(4))
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(4))
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(4))
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6), LnAr(4))
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7), LnAr(4))
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8), LnAr(4))
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9), LnAr(4))
        'e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) + ClAr(6) + ClAr(7) + ClAr(8) + ClAr(9) + ClAr(10), LnAr(4))

        CurY = CurY + 10

        If prn_PageNo <= 1 Then

            Tot_EBeam_StkSumry = 0
            prn_Tot_Pavu_StkSumry = ""
            prn_Tot_Yarn_StkSumry = ""
            prn_Tot_Amt_BalSumry = 0

            da1 = New SqlClient.SqlDataAdapter("select sum(int6) from " & Trim(Common_Procedures.ReportTempTable) & " where name6 = 'BEAM'", con)
            Dt1 = New DataTable
            da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                    Tot_EBeam_StkSumry = Val(Dt1.Rows(0)(0).ToString)
                End If
            End If
            Dt1.Clear()

            da1 = New SqlClient.SqlDataAdapter("select sum(currency1) from " & Trim(Common_Procedures.ReportTempTable) & " where name6 = 'AMOUNT'", con)
            Dt1 = New DataTable
            da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                    prn_Tot_Amt_BalSumry = Val(Dt1.Rows(0)(0).ToString)
                End If
            End If
            Dt1.Clear()

            da1 = New SqlClient.SqlDataAdapter("select name7 as EndsCount, sum(meters6) as PavuMtrs from " & Trim(Common_Procedures.ReportTempTable) & " where name6 = 'PAVU' GROUP BY name7 having sum(meters6) <> 0 ORDER BY name7", con)
            Dt1 = New DataTable
            da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                For k = 0 To Dt1.Rows.Count - 1
                    prn_Tot_Pavu_StkSumry = Trim(prn_Tot_Pavu_StkSumry) & "  " & IIf(Trim(prn_Tot_Pavu_StkSumry) <> "", ", ", "") & Dt1.Rows(k).Item("EndsCount").ToString & "  :  " & Dt1.Rows(k).Item("PavuMtrs").ToString
                Next
            End If
            Dt1.Clear()

            da1 = New SqlClient.SqlDataAdapter("select name7 as CountName, sum(weight1) as YarnWgt from " & Trim(Common_Procedures.ReportTempTable) & " where name6 = 'YARN' GROUP BY name7 having sum(weight1) <> 0 ORDER BY name7", con)
            Dt1 = New DataTable
            da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                For k = 0 To Dt1.Rows.Count - 1
                    prn_Tot_Yarn_StkSumry = Trim(prn_Tot_Yarn_StkSumry) & IIf(Trim(prn_Tot_Yarn_StkSumry) <> "", ",  ", "") & Dt1.Rows(k).Item("CountName").ToString & " : " & Dt1.Rows(k).Item("YarnWgt").ToString
                Next
                For j = 0 To Dt1.Rows.Count - 1
                    Yn_Kg = 0
                    Count_Val = Thiri_ToKgs_Count(Trim((prn_HdDt.Rows(0).Item("Cloth_Name").ToString())))
                    If Val(Dt1.Rows(j).Item("YarnWgt").ToString) <> 0 Then

                        Yn_Kg = Format((Val(Dt1.Rows(j).Item("YarnWgt").ToString) / Count_Val) * 50 / 11, "##########0.000")

                    End If

                    prn_Tot_Yarn_StkSumry_Kg = Trim(prn_Tot_Yarn_StkSumry_Kg) & IIf(Trim(prn_Tot_Yarn_StkSumry_Kg) <> "", ",  ", "") & Dt1.Rows(j).Item("CountName").ToString & " : " & Yn_Kg
                Next

            End If
            Dt1.Clear()

            C1 = ClAr(1) + ClAr(2) + ClAr(3) - 40
            C2 = C1 + ClAr(4) + ClAr(5)

            S1 = e.Graphics.MeasureString("Ë™  :  ", pFont).Width
            pTFont = New Font("TAM_SC_Suvita", 13, FontStyle.Regular)
            pFont = New Font("Calibri", 12, FontStyle.Regular)

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1009" Then '---- Nithya Textiles (Sedapalayam)

                Common_Procedures.Print_To_PrintDocument(e, "Þ¼Š¹ Mõó‹ :- ", LMargin + 10, CurY, 0, 0, pTFont)

                Common_Procedures.Print_To_PrintDocument(e, "d‹  ", LMargin + C1 + 10, CurY, 0, 0, pTFont)
                Common_Procedures.Print_To_PrintDocument(e, " : " & Val(Tot_EBeam_StkSumry), LMargin + C1 + S1 + 20, CurY, 0, 0, pFont)

                Common_Procedures.Print_To_PrintDocument(e, "ªî£¬è  ", LMargin + C2 + 30, CurY, 0, 0, pTFont)
                Common_Procedures.Print_To_PrintDocument(e, "  :  " & Format(Math.Abs(Val(prn_Tot_Amt_BalSumry)), "#########0.00") & IIf(Val(prn_Tot_Amt_BalSumry) < 0, " Dr", " Cr"), LMargin + C2 + 90, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt + 5

                Common_Procedures.Print_To_PrintDocument(e, "(" & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Date").ToString), "dd-MM-yyyy").ToString & ")", LMargin + 10, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt + 5

                ItmNm1 = Trim(prn_Tot_Pavu_StkSumry)
                ItmNm2 = ""
                If Len(ItmNm1) > 90 Then
                    For I = 90 To 1 Step -1
                        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 90
                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I)
                    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
                End If

                ItmNm3 = Trim(ItmNm2)
                ItmNm4 = ""
                If Len(ItmNm3) > 100 Then
                    For I = 100 To 1 Step -1
                        If Mid$(Trim(ItmNm3), I, 1) = " " Or Mid$(Trim(ItmNm3), I, 1) = "," Or Mid$(Trim(ItmNm3), I, 1) = "." Or Mid$(Trim(ItmNm3), I, 1) = "-" Or Mid$(Trim(ItmNm3), I, 1) = "/" Or Mid$(Trim(ItmNm3), I, 1) = "_" Or Mid$(Trim(ItmNm3), I, 1) = "(" Or Mid$(Trim(ItmNm3), I, 1) = ")" Or Mid$(Trim(ItmNm3), I, 1) = "\" Or Mid$(Trim(ItmNm3), I, 1) = "[" Or Mid$(Trim(ItmNm3), I, 1) = "]" Or Mid$(Trim(ItmNm3), I, 1) = "{" Or Mid$(Trim(ItmNm3), I, 1) = "}" Then Exit For
                    Next I
                    If I = 0 Then I = 100
                    ItmNm4 = Microsoft.VisualBasic.Right(Trim(ItmNm3), Len(ItmNm3) - I)
                    ItmNm3 = Microsoft.VisualBasic.Left(Trim(ItmNm3), I - 1)
                End If

                Common_Procedures.Print_To_PrintDocument(e, "ð£¾  ", LMargin + 10, CurY, 0, 0, pTFont)
                Common_Procedures.Print_To_PrintDocument(e, "  :  " & ItmNm1, LMargin + S1 + 20, CurY, 0, 0, pFont)

                If Trim(ItmNm3) <> "" Then
                    CurY = CurY + TxtHgt + 5
                    Common_Procedures.Print_To_PrintDocument(e, ItmNm3, LMargin + 10, CurY, 0, 0, pFont)
                End If

                If Trim(ItmNm4) <> "" Then
                    CurY = CurY + TxtHgt + 5
                    Common_Procedures.Print_To_PrintDocument(e, ItmNm4, LMargin + 10, CurY, 0, 0, pFont)
                End If

                CurY = CurY + TxtHgt + 5

                Common_Procedures.Print_To_PrintDocument(e, "Ë™ ( FK ) ", LMargin + 10, CurY, 0, 0, pTFont)
                Common_Procedures.Print_To_PrintDocument(e, "  :  " & prn_Tot_Yarn_StkSumry, LMargin + S1 + 70, CurY, 0, 0, pFont)

                CurY = CurY + TxtHgt + 5
                Common_Procedures.Print_To_PrintDocument(e, "Ë™( A«ô£ ) ", LMargin + 10, CurY, 0, 0, pTFont)
                Common_Procedures.Print_To_PrintDocument(e, "  :  " & prn_Tot_Yarn_StkSumry_Kg, LMargin + S1 + 70, CurY, 0, 0, pFont)

            Else
                CurY = CurY + TxtHgt + 5
            End If

        Else
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt

        End If

        CurY = CurY + TxtHgt + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "(" & Trim(Common_Procedures.User.Name) & ")", LMargin + 300, CurY, 0, 0, pFont)

        CurY = CurY + TxtHgt

        Common_Procedures.Print_To_PrintDocument(e, "îPè£ó˜ ¬èªò£Šð‹  ", LMargin + 10, CurY, 0, 0, pTFont)
        Common_Procedures.Print_To_PrintDocument(e, "Prepared By ", LMargin + 300, CurY, 0, 0, pFont)
        If Val(Common_Procedures.settings.WeaverWages_Print_Weavers_Name_IN_Heading) = 1 Then
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Name").ToString
        Else
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString
        End If

        If Val(Common_Procedures.settings.WeaverWages_Print_Dont_Show_Company_Heading) = 1 Then
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

    Private Sub Printing_Format3(ByRef e As System.Drawing.Printing.PrintPageEventArgs) ' Simple Format without Stock
        Dim pFont As Font
        Dim p1Font As Font
        'Dim ps As Printing.PaperSize
        Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
        Dim PrintWidth As Single, PrintHeight As Single
        Dim PageWidth As Single, PageHeight As Single
        'Dim I As Integer
        Dim NoofItems_PerPage As Integer, NoofDets As Integer
        Dim TxtHgt As Single
        Dim PpSzSTS As Boolean = False
        Dim LnAr(15) As Single, ClAr(15) As Single
        Dim EntryCode As String
        Dim CurY As Single
        Dim C1, C2, C3, C4, C5, W1, W2 As Single
        Dim snd, sec, bit, rjt, otr As Single


        If prn_PageSize_SetUP_STS = False Then
            set_PaperSize_For_PrintDocument1()
            prn_PageSize_SetUP_STS = True
        End If


        p1Font = New Font("Calibri", 10, FontStyle.Bold)


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

        pFont = New Font("Calibri", 10, FontStyle.Regular)

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

        TxtHgt = 18

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        p1Font = New Font("TAM_SC_Suvita", 10, FontStyle.Bold)

        Try

            prn_Prev_HeadIndx = prn_HeadIndx

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format3_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

                NoofDets = NoofDets + 1

                CurY = CurY + TxtHgt
                pFont = New Font("TAM_SC_Suvita", 10, FontStyle.Bold)
                Common_Procedures.Print_To_PrintDocument(e, "óè‹ ", LMargin + 10, CurY, 0, 0, p1Font)
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Cloth_Name").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                snd = Val(prn_HdDt.Rows(prn_HeadIndx).Item("Sound_Meters").ToString) * Val(prn_HdDt.Rows(prn_HeadIndx).Item("Sound_cooly").ToString)
                sec = Val(prn_HdDt.Rows(prn_HeadIndx).Item("Seconds_Meters").ToString) * Val(prn_HdDt.Rows(prn_HeadIndx).Item("Seconds_cooly").ToString)
                bit = Val(prn_HdDt.Rows(prn_HeadIndx).Item("Bits_Meters").ToString) * Val(prn_HdDt.Rows(prn_HeadIndx).Item("Bits_cooly").ToString)
                rjt = Val(prn_HdDt.Rows(prn_HeadIndx).Item("Reject_Meters").ToString) * Val(prn_HdDt.Rows(prn_HeadIndx).Item("Reject_cooly").ToString)
                otr = Val(prn_HdDt.Rows(prn_HeadIndx).Item("Others_Meters").ToString) * Val(prn_HdDt.Rows(prn_HeadIndx).Item("Others_cooly").ToString)

                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "ê¾‡† ", LMargin + C1 + 10, CurY, 0, 0, p1Font)
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(snd), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " = ", PageWidth - 80, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Sound_Cooly").ToString, PageWidth - 100, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " X ", PageWidth - 170, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Sound_Meters").ToString, PageWidth - 190, CurY, 1, 0, pFont)

                CurY = CurY + TxtHgt
                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "õó¾ e†ì˜ ", LMargin + 10, CurY, 0, 0, p1Font)
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Receipt_Meters").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)


                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "ªêè‡†v ", LMargin + C1 + 10, CurY, 0, 0, p1Font)
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, Format(Val(sec), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " = ", PageWidth - 80, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Seconds_Cooly").ToString, PageWidth - 100, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, " X ", PageWidth - 170, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Seconds_Meters").ToString, PageWidth - 190, CurY, 1, 0, pFont)

                CurY = CurY + TxtHgt
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1032" Then '---- Asia Textiles (Tirupur)
                    pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                    Common_Procedures.Print_To_PrintDocument(e, "õ÷K e†ì˜ ", LMargin + 10, CurY, 0, 0, p1Font)
                    pFont = New Font("Calibri", 11, FontStyle.Regular)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Excess_Short").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)
                End If

                If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Bits_Meters").ToString) <> 0 Then
                    pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                    Common_Procedures.Print_To_PrintDocument(e, "îQ óè‹ ", LMargin + C1 + 10, CurY, 0, 0, p1Font)
                    pFont = New Font("Calibri", 11, FontStyle.Regular)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(bit), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " = ", PageWidth - 80, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Bits_Cooly").ToString, PageWidth - 100, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " X ", PageWidth - 170, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Bits_Meters").ToString, PageWidth - 190, CurY, 1, 0, pFont)

                End If

                CurY = CurY + TxtHgt
                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "ªñ£ˆî ªî£¬è ", LMargin + 10, CurY, 0, 0, p1Font)
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_cooly").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Reject_Meters").ToString) <> 0 Then
                    pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1032" Then
                        Common_Procedures.Print_To_PrintDocument(e, "èN¾ ", LMargin + C1 + 10, CurY, 0, 0, p1Font)
                    Else
                        pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, "ªê‚AƒÜì£v", LMargin + C1 + 10, CurY, 0, 0, p1Font)
                        pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                    End If
                    pFont = New Font("Calibri", 11, FontStyle.Regular)
                    Common_Procedures.Print_To_PrintDocument(e, Format(Val(rjt), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " = ", PageWidth - 80, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Reject_Cooly").ToString, PageWidth - 100, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, " X ", PageWidth - 170, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Reject_Meters").ToString, PageWidth - 190, CurY, 1, 0, pFont)
                End If

                CurY = CurY + TxtHgt
                If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Freight_Charge").ToString) <> 0 Then

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Then '---- Asia Textiles (Tirupur)
                        p1Font = New Font("Calibri", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, "V.V", LMargin + 10, CurY, 0, 0, p1Font)
                        'Common_Procedures.Print_To_PrintDocument(e, "«õ¡ ", LMargin + 10, CurY, 0, 0, pFont)

                    Else
                        p1Font = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, "õ‡® õ£ì¬è ", LMargin + 10, CurY, 0, 0, p1Font)

                    End If
                    pFont = New Font("Calibri", 11, FontStyle.Regular)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Freight_Charge").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)
                End If
                p1Font = New Font("TAM_SC_Suvita", 11, FontStyle.Bold)
                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Others_Meters").ToString) <> 0 Then

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1032" Then '---- Asia Textiles (Tirupur)
                        pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, "Þîó ", LMargin + C1 + 10, CurY, 0, 0, p1Font)
                        pFont = New Font("Calibri", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, Format(Val(otr), "########0.00"), PageWidth - 10, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, " = ", PageWidth - 80, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Others_Cooly").ToString, PageWidth - 100, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, " X ", PageWidth - 170, CurY, 1, 0, pFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Others_Meters").ToString, PageWidth - 190, CurY, 1, 0, pFont)
                    End If

                End If

                CurY = CurY + TxtHgt

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1032" Then '---- Asia Textiles (Tirupur)

                    If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Less_Amount").ToString) <> 0 Then
                        pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1049" Then
                            Common_Procedures.Print_To_PrintDocument(e, "ªêè‡†v H®ˆî‹ ", LMargin + 10, CurY, 0, 0, p1Font)
                        Else
                            Common_Procedures.Print_To_PrintDocument(e, "H®ˆî‹ ", LMargin + 10, CurY, 0, 0, p1Font)
                        End If
                        ' Common_Procedures.Print_To_PrintDocument(e, "H®ˆî‹ ", LMargin + 10, CurY, 0, 0, p1Font)
                        pFont = New Font("Calibri", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Less_Amount").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)
                    End If

                End If

                e.Graphics.DrawLine(Pens.Black, LMargin + C2 + 50, CurY, PageWidth - 10, CurY)
                p1Font = New Font("TAM_SC_Suvita", 11, FontStyle.Bold)
                CurY = CurY + TxtHgt
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1032" Then '---- Asia Textiles (Tirupur)
                    If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Tds_Perc_Calc").ToString) <> 0 Then
                        pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, "õK H®ˆî‹ ", LMargin + 10, CurY, 0, 0, p1Font)
                        pFont = New Font("Calibri", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Tds_Perc_Calc").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)
                    End If
                End If
                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "ªñ£ˆî‹ ", LMargin + C1 + 10, CurY, 0, 0, p1Font)
                pFont = New Font("Calibri", 11, FontStyle.Regular)

                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_Meters").ToString, PageWidth - 190, CurY, 1, 0, pFont)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Total_Cooly").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)


                CurY = CurY + TxtHgt
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1032" Then
                    If Val(prn_HdDt.Rows(prn_HeadIndx).Item("Add_Amount").ToString) <> 0 Then
                        pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, "Ü†õ£¡v ", LMargin + 10, CurY, 0, 0, p1Font)
                        pFont = New Font("Calibri", 11, FontStyle.Regular)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Add_Amount").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)
                    End If
                End If

                CurY = CurY + TxtHgt
                e.Graphics.DrawLine(Pens.Black, LMargin + C1 - 100, CurY, LMargin + C1 - 10, CurY)

                e.Graphics.DrawLine(Pens.Black, LMargin + C2 + 50, CurY, PageWidth - 10, CurY)

                CurY = CurY + 10
                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Then
                    Common_Procedures.Print_To_PrintDocument(e, "ªî£¬è ", LMargin + 10, CurY, 0, 0, p1Font)
                Else
                    Common_Procedures.Print_To_PrintDocument(e, "eF ªî£¬è ", LMargin + 10, CurY, 0, 0, p1Font)

                End If
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Net_Amount").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)

                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)

                W1 = e.Graphics.MeasureString("dv â‡E‚¬è : ", p1Font).Width
                W2 = e.Graphics.MeasureString("Ë™ ªêô¾ : ", p1Font).Width

                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "dv â‡E‚¬è ", LMargin + C1 + 10, CurY, 0, 0, p1Font)
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, " :  " & prn_HdDt.Rows(prn_HeadIndx).Item("Pcs").ToString, LMargin + C1 + W1 + 40, CurY, 0, 0, pFont)
                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)

                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "Ë™ ªêô¾ ", LMargin + C1 + 250, CurY, 0, 0, p1Font)
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, " :  " & prn_HdDt.Rows(prn_HeadIndx).Item("Total_Dgv_Weight").ToString, LMargin + C1 + 250 + W2 + 10, CurY, 0, 0, pFont)

                NoofDets = NoofDets + 1

                prn_DetIndx = prn_DetIndx + 1

                CurY = CurY + TxtHgt + 10

                e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
                e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))

            End If

            Printing_Format3_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

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

    Private Sub Printing_Format3_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
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

        If Val(Common_Procedures.settings.WeaverWages_Print_Weavers_Name_IN_Heading) = 1 Then
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Name").ToString
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


        If Val(Common_Procedures.settings.WeaverWages_Print_Dont_Show_Company_Heading) <> 1 Then '---- Arul Kumaran Textiles (Somanur)

            CurY = CurY + TxtHgt - 10

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Then
                p1Font = New Font("Calibri", 11, FontStyle.Regular)
            Else
                p1Font = New Font("Calibri", 18, FontStyle.Bold)
            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Then
                Common_Procedures.Print_To_PrintDocument(e, "From", LMargin + 10, CurY, 0, 0, p1Font)
            End If

            Common_Procedures.Print_To_PrintDocument(e, Cmp_Name, LMargin, CurY, 2, PrintWidth, p1Font)
            strHeight = e.Graphics.MeasureString(Cmp_Name, p1Font).Height

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Then
                p1Font = New Font("Calibri", 9, FontStyle.Regular)
            Else
                p1Font = New Font("Calibri", 11, FontStyle.Regular)
            End If

            CurY = CurY + strHeight - 1
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add1, LMargin, CurY, 2, PrintWidth, p1Font)
            CurY = CurY + TxtHgt - 1
            Common_Procedures.Print_To_PrintDocument(e, Cmp_Add2, LMargin, CurY, 2, PrintWidth, p1Font)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1032" Then
                CurY = CurY + TxtHgt - 1
                Common_Procedures.Print_To_PrintDocument(e, Cmp_PhNo, LMargin, CurY, 2, PrintWidth, p1Font)
                CurY = CurY + TxtHgt - 1
                Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, p1Font)
                Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, p1Font)
            End If

            CurY = CurY + strHeight
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        Else
            TxtHgt = TxtHgt + 2

        End If
        LnAr(2) = CurY

        If Val(Common_Procedures.settings.WeaverWages_Print_Weavers_Name_IN_Heading) = 1 Then
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
            Led_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Name").ToString
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

        If Val(prn_WagesDontShowToPartyName) = 1 Then

            CurY = CurY + TxtHgt
            pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
            p1Font = New Font("TAM_SC_Suvita", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "H™ ªï‹. ", LMargin + 10, CurY, 0, 0, p1Font)

            If Val(Common_Procedures.settings.WeaverWages_Print_Weavers_BillNo_SeparateSlNo) = 1 Then
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_BillNo").ToString, LMargin + W1 + 20, CurY, 0, 0, pFont)
            Else
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_No").ToString, LMargin + W1 + 20, CurY, 0, 0, pFont)
            End If

            pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "H™ «îF ", LMargin + 140 + 10, CurY, 0, 0, p1Font)
            pFont = New Font("Calibri", 11, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Date").ToString), "dd-MM-yyyy").ToString, LMargin + 140 + W2 + 20, CurY, 0, 0, pFont)

            If Val(Common_Procedures.settings.WeaverWages_Print_Weavers_BillNo_SeparateSlNo) = 1 Then
                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "ªìLõK ªï‹.", LMargin + C1 + 10, CurY, 0, 0, p1Font)
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, ": " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_No").ToString, LMargin + C1 + W1 + 40, CurY, 0, 0, pFont)

            Else
                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "óC¶ ªï‹. ", LMargin + C1 + 10, CurY, 0, 0, p1Font)
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, ": " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)

            End If

            pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
            If Val(Common_Procedures.settings.WeaverWages_Print_Weavers_BillNo_SeparateSlNo) = 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "ªìLõK «îF", LMargin + C2 + +10, CurY, 0, 0, p1Font)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "óC¶ «îF ", LMargin + C2 + +10, CurY, 0, 0, p1Font)
            End If
            pFont = New Font("Calibri", 11, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, ": " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Rec_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C2 + W2 + 30, CurY, 0, 0, pFont)

            CurY = CurY + 5

        Else

            CurY = CurY + TxtHgt - 10
            p1Font = New Font("TAM_SC_Suvita", 10, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "TO  :  " & "M/s." & Led_Name, LMargin + 10, CurY, 0, 0, pFont)
            pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "H™ ªï‹. ", LMargin + C1 + 10, CurY, 0, 0, p1Font)
            If Val(Common_Procedures.settings.WeaverWages_Print_Weavers_BillNo_SeparateSlNo) = 1 Then
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_BillNo").ToString, LMargin + C1 + W1 + 20, CurY, 0, 0, pFont)
            Else
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_No").ToString, LMargin + C1 + W1 + 20, CurY, 0, 0, pFont)
            End If
            pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, "H™ «îF ", LMargin + C2 + 10, CurY, 0, 0, p1Font)
            pFont = New Font("Calibri", 11, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C2 + W2 + 30, CurY, 0, 0, pFont)

            pFont = New Font("Calibri", 11, FontStyle.Regular)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & Led_Add1, LMargin + S1 + 10, CurY, 0, 0, pFont)

            pFont = New Font("Calibri", 11, FontStyle.Regular)
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, " " & Led_Add2, LMargin + S1 + 10, CurY, 0, 0, pFont)


            If Val(Common_Procedures.settings.WeaverWages_Print_Weavers_BillNo_SeparateSlNo) = 1 Then
                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "ªìLõK ªï‹.", LMargin + C1 + 10, CurY, 0, 0, p1Font)
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_No").ToString, LMargin + C1 + W1 + 40, CurY, 0, 0, pFont)
            Else
                pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, "óC¶ ªï‹. ", LMargin + C1 + 10, CurY, 0, 0, p1Font)
                pFont = New Font("Calibri", 11, FontStyle.Regular)
                Common_Procedures.Print_To_PrintDocument(e, ":   " & prn_HdDt.Rows(prn_HeadIndx).Item("Rec_No").ToString, LMargin + C1 + W1 + 30, CurY, 0, 0, pFont)
            End If

            pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
            If Val(Common_Procedures.settings.WeaverWages_Print_Weavers_BillNo_SeparateSlNo) = 1 Then
                Common_Procedures.Print_To_PrintDocument(e, "ªìLõK «îF", LMargin + C2 + +10, CurY, 0, 0, p1Font)
            Else
                Common_Procedures.Print_To_PrintDocument(e, "óC¶ «îF ", LMargin + C2 + +10, CurY, 0, 0, p1Font)
            End If
            pFont = New Font("Calibri", 11, FontStyle.Regular)
            Common_Procedures.Print_To_PrintDocument(e, ":   " & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Rec_Date").ToString), "dd-MM-yyyy").ToString, LMargin + C2 + W2 + 30, CurY, 0, 0, pFont)

        End If

        CurY = CurY + TxtHgt + 10

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY

        e.Graphics.DrawLine(Pens.Black, LMargin + C1, CurY, LMargin + C1, LnAr(2))

    End Sub

    Private Sub Printing_Format3_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
        Dim i As Integer
        Dim Cmp_Name As String
        Dim p1Font As Font
        Dim pTFont As Font
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim k As Integer = 0
        Dim S1, C1, C2, C5 As Single
        Dim Tot_EBeam_StkSumry As Single = 0
        Dim prn_Tot_Pavu_StkSumry As String = ""
        Dim prn_Tot_Yarn_StkSumry As String = ""
        Dim prn_Tot_Amt_BalSumry As Single = 0

        For i = NoofDets + 1 To NoofItems_PerPage
            CurY = CurY + TxtHgt
        Next

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Then

        '    e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        '    e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        '    e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)

        'End If

        'CurY = CurY + TxtHgt
        'CurY = CurY + TxtHgt


        'CurY = CurY + 10
        'pFont = New Font("TAM_SC_Suvita", 11, FontStyle.Regular)
        'p1Font = New Font("TAM_SC_Suvita", 11, FontStyle.Bold)


        'Common_Procedures.Print_To_PrintDocument(e, "îPè£ó˜ ¬èªò£Šð‹  ", LMargin + 10, CurY, 0, 0, p1Font)

        'If Val(Common_Procedures.settings.WeaverWages_Print_Weavers_Name_IN_Heading) = 1 Then
        '    Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Name").ToString
        'Else
        '    Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString
        'End If



        'If Val(Common_Procedures.settings.WeaverWages_Print_Dont_Show_Company_Heading) = 1 Then
        '    p1Font = New Font("Calibri", 12, FontStyle.Bold)
        '    Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 15, CurY, 1, 0, p1Font)

        'Else
        '    p1Font = New Font("Calibri", 12, FontStyle.Bold)
        '    Common_Procedures.Print_To_PrintDocument(e, "For " & Cmp_Name, PageWidth - 15, CurY, 1, 0, p1Font)

        'End If

        'CurY = CurY + TxtHgt + 10

        'e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)

        'e.Graphics.DrawLine(Pens.Black, LMargin, LnAr(1), LMargin, CurY)
        'e.Graphics.DrawLine(Pens.Black, PageWidth, LnAr(1), PageWidth, CurY)
        C5 = ClAr(1) + ClAr(2) - 10

        CurY = CurY + 10
        pTFont = New Font("TAM_SC_Suvita", 9, FontStyle.Bold)
        If prn_PageNo <= 1 Then

            Tot_EBeam_StkSumry = 0
            prn_Tot_Pavu_StkSumry = ""
            prn_Tot_Yarn_StkSumry = ""
            prn_Tot_Amt_BalSumry = 0

            da1 = New SqlClient.SqlDataAdapter("select sum(int6) from " & Trim(Common_Procedures.ReportTempTable) & " where name6 = 'BEAM'", con)
            Dt1 = New DataTable
            da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                    Tot_EBeam_StkSumry = Val(Dt1.Rows(0)(0).ToString)
                End If
            End If
            Dt1.Clear()

            da1 = New SqlClient.SqlDataAdapter("select sum(currency1) from " & Trim(Common_Procedures.ReportTempTable) & " where name6 = 'AMOUNT'", con)
            Dt1 = New DataTable
            da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                    prn_Tot_Amt_BalSumry = Val(Dt1.Rows(0)(0).ToString)
                End If
            End If
            Dt1.Clear()

            da1 = New SqlClient.SqlDataAdapter("select name7 as EndsCount, sum(meters6) as PavuMtrs from " & Trim(Common_Procedures.ReportTempTable) & " where name6 = 'PAVU' GROUP BY name7 having sum(meters6) <> 0 ORDER BY name7", con)
            Dt1 = New DataTable
            da1.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                For k = 0 To Dt1.Rows.Count - 1
                    prn_Tot_Pavu_StkSumry = Trim(prn_Tot_Pavu_StkSumry) & IIf(Trim(prn_Tot_Pavu_StkSumry) <> "", ", ", "") & Dt1.Rows(k).Item("EndsCount").ToString & " : " & Dt1.Rows(k).Item("PavuMtrs").ToString
                Next
            End If
            Dt1.Clear()

            da1 = New SqlClient.SqlDataAdapter("select name7 as CountName, sum(weight1) as YarnWgt from " & Trim(Common_Procedures.ReportTempTable) & " where name6 = 'YARN' GROUP BY name7 having sum(weight1) <> 0 ORDER BY name7", con)
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

            Common_Procedures.Print_To_PrintDocument(e, "d‹  ", LMargin + C5 + 10, CurY, 0, 0, pTFont)
            Common_Procedures.Print_To_PrintDocument(e, " : " & Val(Tot_EBeam_StkSumry), LMargin + C5 + S1 + 20, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "ªî£¬è  ", LMargin + C2 + 30, CurY, 0, 0, pTFont)
            Common_Procedures.Print_To_PrintDocument(e, "  :  " & Format(Math.Abs(Val(prn_Tot_Amt_BalSumry)), "#########0.00") & IIf(Val(prn_Tot_Amt_BalSumry) < 0, " Dr", " Cr"), LMargin + C2 + 90, CurY, 0, 0, pFont)

            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "(" & Format(Convert.ToDateTime(prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Date").ToString), "dd-MM-yyyy").ToString & ")", LMargin + 10, CurY, 0, 0, pFont)

            Common_Procedures.Print_To_PrintDocument(e, "ð£¾  ", LMargin + C5 + 10, CurY, 0, 0, pTFont)
            Common_Procedures.Print_To_PrintDocument(e, "  :  " & prn_Tot_Pavu_StkSumry, LMargin + C5 + S1 + 20, CurY, 0, 0, pFont)


            CurY = CurY + TxtHgt

            Common_Procedures.Print_To_PrintDocument(e, "Ë™  ", LMargin + C5 + 10, CurY, 0, 0, pTFont)
            Common_Procedures.Print_To_PrintDocument(e, "  :  " & prn_Tot_Yarn_StkSumry, LMargin + C5 + S1 + 20, CurY, 0, 0, pFont)

        Else
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt

        End If

        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        CurY = CurY + TxtHgt
        Common_Procedures.Print_To_PrintDocument(e, "îPè£ó˜ ¬èªò£Šð‹  ", LMargin + 10, CurY, 0, 0, pTFont)

        If Val(Common_Procedures.settings.WeaverWages_Print_Weavers_Name_IN_Heading) = 1 Then
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Name").ToString
        Else
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString
        End If

        If Val(Common_Procedures.settings.WeaverWages_Print_Dont_Show_Company_Heading) = 1 Then
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

        cmd.CommandText = "Truncate table " & Trim(Common_Procedures.ReportTempTable) & ""
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

        cmd.CommandText = "Truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(Int1) Select (a.Empty_Beam+a.Pavu_Beam) from Stock_Empty_BeamBagCone_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.DeliveryTo_Idno <> 0 and a.DeliveryTo_Idno = tP.Ledger_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date < @fromdate and (a.Empty_Beam <> 0 or a.Pavu_Beam <> 0 )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(Int1) Select -1*(a.Empty_Beam+Pavu_Beam) from Stock_Empty_BeamBagCone_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ReceivedFrom_Idno <> 0 and a.ReceivedFrom_Idno = tP.Ledger_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date < @fromdate and (a.Empty_Beam <> 0 or a.Pavu_Beam <> 0)"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(int3, name5, name6, Int6) Select 0, 'Opening', 'BEAM', sum(Int1) from " & Trim(Common_Procedures.ReportTempSubTable) & " having sum(Int1) <> 0"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(int3, Date1, name1, name2, meters1, name3, name4, name5, name6, Int6) Select 1, a.Reference_Date, a.Reference_Code, a.Reference_No, a.For_OrderBy, a.Party_Bill_No, tP.Ledger_Name, a.Particulars, 'BEAM', (a.Empty_Beam+a.Pavu_Beam) from Stock_Empty_BeamBagCone_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Ledger_Head tP ON a.DeliveryTo_Idno <> 0 and a.DeliveryTo_Idno = tP.Ledger_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date between @fromdate and @todate and (a.Empty_Beam <> 0 or a.Pavu_Beam <> 0 )"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(int3, Date1, name1, name2, meters1, name3, name4, name5, name6, Int6) Select 2, a.Reference_Date, a.Reference_Code, a.Reference_No, a.For_OrderBy, a.Party_Bill_No, tP.Ledger_Name, a.Particulars, 'BEAM', -1*abs(a.Empty_Beam+a.Pavu_Beam) from Stock_Empty_BeamBagCone_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ReceivedFrom_Idno <> 0 and a.ReceivedFrom_Idno = tP.Ledger_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date between @fromdate and @todate and (a.Empty_Beam <> 0 or a.Pavu_Beam <> 0)"
        cmd.ExecuteNonQuery()

        '-------- Pavu 

        cmd.CommandText = "Truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(name1, meters1) Select c.endscount_name, a.Meters from Stock_Pavu_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.DeliveryTo_Idno <> 0 and a.DeliveryTo_Idno = tP.Ledger_IdNo INNER JOIN EndsCount_Head c ON a.EndsCount_IdNo <> 0 and a.EndsCount_IdNo = c.EndsCount_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date < @fromdate and a.Meters <> 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(name1, meters1) Select c.endscount_name, -1*a.Meters from Stock_Pavu_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ReceivedFrom_Idno <> 0 and a.ReceivedFrom_Idno = tP.Ledger_IdNo INNER JOIN EndsCount_Head c ON a.EndsCount_IdNo <> 0 and a.EndsCount_IdNo = c.EndsCount_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date < @fromdate and a.Meters <> 0"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(int3, name5, name6, name7, meters6) Select 0, 'Opening', 'PAVU', name1, sum(meters1) from " & Trim(Common_Procedures.ReportTempSubTable) & " group by name1 having sum(meters1) <> 0"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(int3, Date1, name1, name2, meters1, name3, name4, name5, name6, name7, meters6) Select 1, a.Reference_Date, a.Reference_Code, a.Reference_No, a.For_OrderBy, a.Party_Bill_No, tP.Ledger_Name, a.Particulars, 'PAVU', c.endscount_name, abs(a.Meters) from Stock_Pavu_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Ledger_Head tP ON a.DeliveryTo_Idno <> 0 and a.DeliveryTo_Idno = tP.Ledger_IdNo INNER JOIN EndsCount_Head c ON a.EndsCount_IdNo <> 0 and a.EndsCount_IdNo = c.EndsCount_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date between @fromdate and @todate and a.Meters <> 0 "
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(int3, Date1, name1, name2, meters1, name3, name4, name5, name6, name7, meters6) Select 2, a.Reference_Date, a.Reference_Code, a.Reference_No, a.For_OrderBy, a.Party_Bill_No, tP.Ledger_Name, a.Particulars, 'PAVU', c.endscount_name, -1*abs(a.Meters) from Stock_Pavu_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ReceivedFrom_Idno <> 0 and a.ReceivedFrom_Idno = tP.Ledger_IdNo INNER JOIN EndsCount_Head c ON a.EndsCount_IdNo <> 0 and a.EndsCount_IdNo = c.EndsCount_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date between @fromdate and @todate and a.Meters <> 0 "
        cmd.ExecuteNonQuery()

        '-------- Yarn

        cmd.CommandText = "Truncate table " & Trim(Common_Procedures.ReportTempSubTable) & ""
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(name1, weight1) Select c.count_name, a.Weight from Stock_Yarn_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.DeliveryTo_Idno <> 0 and a.DeliveryTo_Idno = tP.Ledger_IdNo INNER JOIN Count_Head c ON a.Count_IdNo <> 0 and a.Count_IdNo = c.Count_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date < @fromdate and a.Weight <> 0"
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempSubTable) & "(name1, weight1) Select c.count_name, -1*a.Weight from Stock_Yarn_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ReceivedFrom_Idno <> 0 and a.ReceivedFrom_Idno = tP.Ledger_IdNo INNER JOIN Count_Head c ON a.Count_IdNo <> 0 and a.Count_IdNo = c.Count_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date < @fromdate and a.Weight <> 0"
        cmd.ExecuteNonQuery()



        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(int3, name5, name6, name7, weight1) Select 0, 'Opening', 'YARN', name1, sum(Weight1) from " & Trim(Common_Procedures.ReportTempSubTable) & " group by name1 having sum(Weight1) <> 0"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(int3, Date1, name1, name2, meters1, name3, name4, name5, name6, name7, weight1) Select 1, a.Reference_Date, a.Reference_Code, a.Reference_No, a.For_OrderBy, a.Party_Bill_No, tP.Ledger_Name, a.Particulars, 'YARN', c.count_name, abs(a.Weight) from Stock_Yarn_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Ledger_Head tP ON a.DeliveryTo_Idno <> 0 and a.DeliveryTo_Idno = tP.Ledger_IdNo INNER JOIN Count_Head c ON a.Count_IdNo <> 0 and a.Count_IdNo = c.Count_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date between @fromdate and @todate and a.Weight <> 0 "
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(int3, Date1, name1, name2, meters1, name3, name4, name5, name6, name7, weight1) Select 2, a.Reference_Date, a.Reference_Code, a.Reference_No, a.For_OrderBy, a.Party_Bill_No, tP.Ledger_Name, a.Particulars, 'YARN', c.count_name, -1*abs(a.Weight) from Stock_Yarn_Processing_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.ReceivedFrom_Idno <> 0 and a.ReceivedFrom_Idno = tP.Ledger_IdNo INNER JOIN Count_Head c ON a.Count_IdNo <> 0 and a.Count_IdNo = c.Count_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Reference_Date between @fromdate and @todate and a.Weight <> 0 "
        cmd.ExecuteNonQuery()

        '-------- Amount

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(int3, name5, name6, Currency1) Select 0, 'Opening', 'AMOUNT', sum(a.Voucher_Amount) from Voucher_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo LEFT OUTER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = tP.Ledger_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Voucher_Date < @fromdate and a.Voucher_Amount <> 0"
        cmd.ExecuteNonQuery()

        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(int3, Date1, name1, name2, meters1, name3, name4, name5, name6, Currency1) Select 12, a.Voucher_Date, a.Voucher_Code, a.Voucher_No, a.For_OrderBy, a.Voucher_No, tP.Ledger_Name, replace(left(a.Entry_Identification, len(a.Entry_Identification)-6),'-' + cast(a.Company_Idno as varchar) + '-','-') as Particularss, 'AMOUNT', -1*abs(a.Voucher_Amount) from Voucher_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = tP.Ledger_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Voucher_Date between @fromdate and @todate and a.Voucher_Amount < 0 "
        cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(int3, Date1, name1, name2, meters1, name3, name4, name5, name6, Currency1) Select 11, a.Voucher_Date, a.Voucher_Code, a.Voucher_No, a.For_OrderBy, a.Voucher_No, tP.Ledger_Name, replace(left(a.Entry_Identification, len(a.Entry_Identification)-6),'-' + cast(a.Company_Idno as varchar) + '-','-') as Particularss, 'AMOUNT', abs(a.Voucher_Amount) from Voucher_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = tP.Ledger_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Voucher_Date between @fromdate and @todate and a.Voucher_Amount > 0 "
        cmd.ExecuteNonQuery()

    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        Common_Procedures.Print_OR_Preview_Status = 1
        print_record()
    End Sub

    Private Sub cbo_Filter_PartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_PartyName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_type = 'WEAVER'  or Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1 )", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_type = 'WEAVER'  or Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1)", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, cbo_Filter_CountName, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_type = 'WEAVER'  or Ledger_Type = 'GODOWN' or Show_In_All_Entry = 1)", "(Ledger_idno = 0)")
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

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If
    End Sub

    Private Sub btn_Cancel_PrintOption_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel_PrintOption.Click
        btn_Close_PrintOption_Click(sender, e)
    End Sub

    Private Sub btn_Close_PrintOption_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_PrintOption.Click
        pnl_Back.Enabled = True
        pnl_PrintOption.Visible = False
    End Sub

    Private Sub btn_Print_WithStock_WithName_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_WithStock_WithName.Click
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1116" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1380" Then '---- St.LOURDU MATHA TEX (Somanur)
            prn_WagesFrmt = "FORMAT-4"
        Else
            prn_WagesFrmt = "FORMAT-2"
        End If

        prn_WagesDontShowToPartyName = 0

        txt_PrintRange_FromNo.Text = prn_FromNo
        txt_PrintRange_ToNo.Text = prn_ToNo

        pnl_PrintRange.Visible = True
        pnl_Back.Enabled = False
        pnl_PrintOption.Visible = False

        If txt_PrintRange_FromNo.Enabled Then txt_PrintRange_FromNo.Focus()

    End Sub

    Private Sub btn_Print_WithStock_WithoutName_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_WithStock_WithoutName.Click
        prn_WagesFrmt = "FORMAT-2"
        prn_WagesDontShowToPartyName = 1

        txt_PrintRange_FromNo.Text = prn_FromNo
        txt_PrintRange_ToNo.Text = prn_ToNo

        pnl_PrintRange.Visible = True
        pnl_Back.Enabled = False
        pnl_PrintOption.Visible = False

        If txt_PrintRange_FromNo.Enabled Then txt_PrintRange_FromNo.Focus()

    End Sub

    Private Sub btn_Print_Simple_WithName_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Simple_WithName.Click
        prn_WagesFrmt = "FORMAT-1"
        prn_WagesDontShowToPartyName = 0

        txt_PrintRange_FromNo.Text = prn_FromNo
        txt_PrintRange_ToNo.Text = prn_ToNo

        pnl_PrintRange.Visible = True
        pnl_Back.Enabled = False
        pnl_PrintOption.Visible = False

        If txt_PrintRange_FromNo.Enabled Then txt_PrintRange_FromNo.Focus()

    End Sub

    Private Sub btn_Print_Simple_WithOutName_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Simple_WithOutName.Click
        prn_WagesFrmt = "FORMAT-1"
        prn_WagesDontShowToPartyName = 1

        txt_PrintRange_FromNo.Text = prn_FromNo
        txt_PrintRange_ToNo.Text = prn_ToNo

        pnl_PrintRange.Visible = True
        pnl_Back.Enabled = False
        pnl_PrintOption.Visible = False

        If txt_PrintRange_FromNo.Enabled Then txt_PrintRange_FromNo.Focus()

    End Sub

    Private Sub btn_Insert_WeaverBillNo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Insert_WeaverBillNo.Click
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
                lbl_WeaverBillNo.Text = Trim(UCase(inpno))

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Dt.Dispose()
            Da.Dispose()

        End Try

    End Sub

    Private Sub btn_Close_PrintRange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_PrintRange.Click
        pnl_Back.Enabled = True
        pnl_PrintRange.Visible = False
    End Sub

    Private Sub btn_Cancel_PrintRange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Cancel_PrintRange.Click
        btn_Close_PrintRange_Click(sender, e)
    End Sub

    Private Sub btn_Print_PrintRange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_PrintRange.Click
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim I As Integer = 0
        Dim OrdBy_FrmNo As Single = 0, OrdByToNo As Single = 0

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

    Private Sub txt_FoldingLess_Perc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_FoldingLess_Perc.TextChanged
        Dim i As Integer = 0

        With dgv_Details
            If .Visible = True Then

                If .Rows.Count > 0 Then
                    For i = 0 To .Rows.Count - 1
                        Weight_Calculation(i, 0)
                    Next
                End If

            End If

        End With

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
        save_record()
        If Trim(UCase(LastNo)) = Trim(UCase(lbl_BillNo.Text)) Then
            Timer1.Enabled = False
            SaveAll_STS = False
            MessageBox.Show("All entries saved sucessfully", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            movenext_record()
        End If
    End Sub

    Private Sub txt_ScdsLsMeter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_ScdsLsMeter.KeyDown
        If e.KeyValue = 38 Then txt_Add_Amount.Focus()
        If e.KeyValue = 40 Then txt_ScdsLsRate.Focus()
    End Sub

    Private Sub txt_ScdsLsMeter_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ScdsLsMeter.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            txt_ScdsLsRate.Focus()
        End If
    End Sub

    Private Sub txt_ScdsLsRate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_ScdsLsRate.KeyDown
        If e.KeyValue = 38 Then txt_ScdsLsMeter.Focus()
        If e.KeyValue = 40 Then txt_Tds.Focus()
    End Sub

    Private Sub txt_ScdsLsRate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_ScdsLsRate.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then
            txt_Tds.Focus()
        End If
    End Sub

    Private Sub txt_ScdsLsRate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_ScdsLsRate.LostFocus
        txt_Less_Amount.Text = Val(txt_ScdsLsMeter.Text) * Val(txt_ScdsLsRate.Text)
    End Sub

    Private Sub txt_ScdsLsMeter_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_ScdsLsMeter.LostFocus
        txt_Less_Amount.Text = Val(txt_ScdsLsMeter.Text) * Val(txt_ScdsLsRate.Text)
    End Sub



    Private Sub txt_Remarks_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Remarks.KeyDown
        If e.KeyValue = 38 Then
            txt_Tds.Focus()
        End If

        If e.KeyValue = 40 Then
            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Remarks_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Remarks.KeyPress
        If Asc(e.KeyChar) = 13 Then


            If MessageBox.Show("Do you want to save", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                save_record()
            Else
                msk_Date.Focus()
            End If
        End If
    End Sub

    Private Sub set_PaperSize_For_PrintDocument1()
        Dim I As Integer = 0
        Dim PpSzSTS As Boolean = False
        Dim ps As Printing.PaperSize


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1035" Then                   '---- Kalaimagal Textiles (Avinashi)
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    Exit For
                End If
            Next

        ElseIf Val(Common_Procedures.settings.Printing_For_HalfSheet_Set_Custom8X6_As_Default_PaperSize) = 1 Then
            Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
            PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
            PrintDocument1.DefaultPageSettings.Landscape = False

        ElseIf Val(Common_Procedures.settings.Printing_For_HalfSheet_Set_A4_As_Default_PaperSize) = 1 Then
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    Exit For
                End If
            Next

        Else


            PpSzSTS = False

            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                'Debug.Print(ps.PaperName)
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

    End Sub


    Private Sub txt_Pcs_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Pcs.LostFocus
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt2 As New DataTable
        Dim Stock_In As String
        Dim mtrspcs As Single
        Dim No_Of_Pcs As Integer = 0
        Dim q As Single = 0
        Dim Dt As New DataTable
        Dim Clo_Mtrs_Pc As Single = 0
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim Clo_IdNo As Integer = 0
        Dim edscnt_id As Integer = 0
        Dim cnt_id As Integer = 0
        Dim mtrs As Single = 0

        'End_Id = 0
        'End_Id = Common_Procedures.EndsCount_NameToIdNo(con, Trim(cbo_EndsCount.Text))
        No_Of_Pcs = 0
        No_Of_Pcs = Val(txt_Pcs.Text)

        Clo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, lbl_Cloth.Text)
        'Clo_Mtrs_Pc = 0

        'Da = New SqlClient.SqlDataAdapter("select * from Cloth_Head where Cloth_idno = " & Str(Val(Clo_IdNo)), con)
        'Da.Fill(Dt)

        'If Dt.Rows.Count > 0 Then
        '    If IsDBNull(Dt.Rows(0)(0).ToString) = False Then
        '        Clo_Mtrs_Pc = Dt.Rows(0).Item("Meters_Pcs").ToString
        '    End If
        'End If

        'Dt.Clear()
        'Dt.Dispose()
        'Da.Dispose()

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1081" Then '---- S.Ravichandran Textiles (Erode)
        '    lbl_ConsPavu.Text = Format(Val(Clo_Mtrs_Pc) * Val(txt_NoOfPcs.Text), "##########0.00")
        '    ' txt_ReceiptMeters.Text = Format(Val(Clo_Mtrs_Pc) * Val(txt_NoOfPcs.Text), "##########0.00")
        'End If

        ' If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1089" Then
        If Val(Clo_IdNo) <> 0 And Val(No_Of_Pcs) <> 0 Then
            Stock_In = ""
            mtrspcs = 0

            Da = New SqlClient.SqlDataAdapter("Select Stock_In , Meters_Pcs from  Cloth_Head Where Cloth_idno = " & Str(Val(Clo_IdNo)), con)
            Dt2 = New DataTable
            Da.Fill(Dt2)
            If Dt2.Rows.Count > 0 Then
                Stock_In = Dt2.Rows(0)("Stock_In").ToString
                mtrspcs = Val(Dt2.Rows(0)("Meters_Pcs").ToString)
            End If
            Dt2.Clear()

            If Trim(UCase(Stock_In)) = "PCS" Then
                txt_Rec_Meter.Text = Format(Val(No_Of_Pcs) * Val(mtrspcs), "########0.00")
            End If

        End If
        'Else
        '    If Val(End_Id) <> 0 And Val(No_Of_Pcs) <> 0 Then
        '        Stock_In = ""
        '        mtrspcs = 0

        '        Da = New SqlClient.SqlDataAdapter("Select Stock_In , Meters_Pcs from  EndsCount_Head Where EndsCount_IdNo = " & Str(Val(End_Id)), con)
        '        Dt2 = New DataTable
        '        Da.Fill(Dt2)
        '        If Dt2.Rows.Count > 0 Then
        '            Stock_In = Dt2.Rows(0)("Stock_In").ToString
        '            mtrspcs = Val(Dt2.Rows(0)("Meters_Pcs").ToString)
        '        End If
        '        Dt2.Clear()

        '        If Trim(UCase(Stock_In)) = "PCS" Then
        '            txt_ReceiptMeters.Text = Format(Val(No_Of_Pcs) * Val(mtrspcs), "########0.00")
        '        End If

        '    End If

        '  End If


        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1081" AND Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1116" Then '---- S.Ravichandran Textiles (Erode)
        '    ConsumedYarn_Calculation()
        '    ConsumedPavu_Calculation()
        'End If


    End Sub


    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue

        If e.KeyCode = 40 Then
            e.Handled = True
            e.SuppressKeyPress = True
            cbo_Weaver.Focus()
        End If

        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_Date.Text
            vmskSelStrt = msk_Date.SelectionStart
        End If

    End Sub

    Private Sub msk_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_Date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            e.Handled = True
            msk_Date.Text = Date.Today
            msk_Date.SelectionStart = 0
        End If
        If Asc(e.KeyChar) = 13 Then
            e.Handled = True
            cbo_Weaver.Focus()
        End If
    End Sub

    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyUp

        Try
            If IsDate(msk_Date.Text) = True Then
                If e.KeyCode = 107 Then
                    msk_Date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_Date.Text))
                    msk_Date.SelectionStart = 0
                ElseIf e.KeyCode = 109 Then
                    msk_Date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_Date.Text))
                    msk_Date.SelectionStart = 0
                End If
                dtp_Date.Value = Convert.ToDateTime(msk_Date.Text)
            End If

            If e.KeyCode = 46 Or e.KeyCode = 8 Then
                Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)
            End If

        Catch ex As Exception
            '----

        End Try

    End Sub

    Private Sub msk_Date_TextChanged(sender As Object, e As EventArgs) Handles msk_Date.TextChanged
        Try

            If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

            If Me.ActiveControl.Name <> dtp_Date.Name Then

                If IsDate(msk_Date.Text) = True Then
                    dtp_Date.Value = Convert.ToDateTime(msk_Date.Text)
                End If

            End If


        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            '---

        End Try
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
        Try

            If FrmLdSTS = True Or NoCalc_Status = True Then Exit Sub

            If Me.ActiveControl.Name <> msk_Date.Name Then
                If IsDate(dtp_Date.Text) = True Then
                    msk_Date.Text = dtp_Date.Text
                    msk_Date.SelectionStart = 0
                End If
            End If

        Catch ex As NullReferenceException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As ObjectDisposedException
            '---MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXTBOX TEXTCHANGE....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Catch ex As Exception
            '----
        End Try
    End Sub

    Private Sub btn_get_Weft_CountName_from_Master_Click(sender As Object, e As EventArgs) Handles btn_get_Weft_CountName_from_Master.Click
        Dim Clo_IdNo As Integer
        Dim wftcnt_idno As Integer
        Dim cnt_id As Integer = 0
        Dim vCnt_Nm As String

        Clo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, lbl_Cloth.Text)

        wftcnt_idno = Val(Common_Procedures.get_FieldValue(con, "cloth_head", "Cloth_WeftCount_IdNo", "(cloth_idno = " & Str(Val(Clo_IdNo)) & ")"))
        vCnt_Nm = Common_Procedures.Count_IdNoToName(con, wftcnt_idno)
        For i = 0 To dgv_Details.Rows.Count - 1
            If Val(dgv_Details.Rows(i).Cells(0).Value) <> 0 Then
                dgv_Details.Rows(i).Cells(1).Value = vCnt_Nm
            End If
        Next

    End Sub


End Class