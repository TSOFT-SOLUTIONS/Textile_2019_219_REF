Imports System.IO

Public Class Piece_Checking_InHouse_Format1
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private SaveAll_STS As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Pk_Condition As String = "PCSCH-"
    Private Pk_Condition2 As String = "PCDOF-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String
    Private LastNo As String = ""

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Private WithEvents dgtxt_WagesDetails As New DataGridViewTextBoxEditingControl

    Private prn_HdDt As New DataTable
    Private prn_DetDt As New DataTable
    Private prn_PageNo As Integer
    Private prn_DetIndx As Integer
    Private prn_DetAr(50, 10) As String
    Private prn_DetMxIndx As Integer
    Private prn_NoofBmDets As Integer
    Private prn_DetSNo As Integer
    Private prn_DetBarCdStkr As Integer
    Private _NewCode As String
    Private prn_Det__Indx As Integer

    Public vmskOldText As String = ""
    Public vmskSelStrt As Integer = -1

    Private vCLO_CONDT As String = ""
    Private FnYearCode1 As String = ""
    Private FnYearCode2 As String = ""
    Private prn_HeadIndx As Integer

    Private fs As FileStream
    Private sw As StreamWriter


    Private Enum dgvCOL_PCSDETAILS As Integer
        PCSNO                   '0
        CLOTHTYPE               '1
        METERS                  '2
        GROSSWEIGHT             '3
        NETWEIGHT               '4
        WEIGHTPERMETER          '5
        STS                     '6
        BALENO                  '7
    End Enum

    Private Enum dgvCOL_PRODUCTIONDETAILS As Integer
        SLNO            '0
        EMPLOYEENAME    '1
        METERS          '2
        RATE            '3
        AMOUNT          '4
    End Enum

    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        Pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Weft_Consumption_Details.Visible = False
        btn_Show_WeftConsumption_Details.Visible = False

        lbl_ChkNo.Text = ""
        lbl_ChkNo.ForeColor = Color.Black

        msk_Date.Text = ""
        dtp_Date.Text = ""
        cbo_LoomNo.Text = ""
        lbl_PartyName.Text = ""
        txt_Folding.Text = ""
        cbo_Grid_ClothType.Text = ""
        lbl_ClothName.Text = ""
        txt_Crimp.Text = ""
        txt_TareWeight.Text = ""
        lbl_FabricLotNo.Text = ""
        txt_BarCode.Text = ""
        txt_BarCode.Tag = ""
        cbo_Filter_PartyName.Text = ""
        cbo_grid_employee.Text = ""
        lbl_RecMtrs.Text = ""
        'txt_RollNo.Text = ""
        'txt_RollNo.Tag = ""
        lbl_RecDate.Text = ""
        lbl_RecCode.Text = ""

        lbl_ConsPavu.Text = ""
        lbl_ConsWeftYarn.Text = ""
        lbl_ExcSht.Text = ""

        cbo_KnotterName.Text = ""
        txt_Wages.Text = ""
        lbl_Wages_Amount.Text = ""

        cbo_LotNo.Text = ""
        Cbo_Filter_ClothName.Text = ""
        Cbo_Filter_FerNo.Text = ""

        dgv_Details.Rows.Clear()
        dgv_Details_Total.Rows.Clear()
        dgv_Details_Total.Rows.Add()

        dgv_Production_Wages_Details.Rows.Clear()
        dgv_production_wages_Total.Rows.Clear()
        dgv_production_wages_Total.Rows.Add()

        pnl_Weft_Consumption_Details.Visible = False
        dgv_Weft_Consumption_Details.Rows.Clear()

        cbo_ClothSales_OrderCode_forSelection.Text = ""


        If Filter_Status = False Then
            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1

            dgv_Filter_Details.Rows.Clear()

        End If

        Grid_Cell_DeSelect()

        cbo_LotNo.Enabled = True
        cbo_LotNo.BackColor = Color.White

        txt_Crimp.Enabled = True
        txt_Crimp.BackColor = Color.White

        txt_Folding.Enabled = True
        txt_Folding.BackColor = Color.White

        btn_Selection.Enabled = True

        cbo_Grid_ClothType.Visible = False
        cbo_grid_employee.Visible = False

        NoCalc_Status = False

    End Sub

    Private Sub ControlGotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtbx As TextBox
        Dim combobx As ComboBox
        Dim msktxbx As MaskedTextBox
        On Error Resume Next

        If FrmLdSTS = True Then Exit Sub

        If TypeOf Me.ActiveControl Is TextBox Or TypeOf Me.ActiveControl Is ComboBox Or TypeOf Prec_ActCtrl Is Button Or TypeOf Me.ActiveControl Is MaskedTextBox Then
            Me.ActiveControl.BackColor = Color.Lime
            Me.ActiveControl.ForeColor = Color.Blue
        End If

        If TypeOf Me.ActiveControl Is TextBox Then
            txtbx = Me.ActiveControl
            txtbx.SelectAll()
        ElseIf TypeOf Me.ActiveControl Is MaskedTextBox Then
            msktxbx = Me.ActiveControl
            msktxbx.SelectionStart = 0
        ElseIf TypeOf Me.ActiveControl Is ComboBox Then
            combobx = Me.ActiveControl
            combobx.SelectAll()
        End If

        If Me.ActiveControl.Name <> cbo_Grid_ClothType.Name Then
            cbo_Grid_ClothType.Visible = False
        End If

        If Me.ActiveControl.Name <> cbo_grid_employee.Name Then
            cbo_grid_employee.Visible = False
        End If

        If Me.ActiveControl.Name <> dgv_Details.Name Then
            Grid_DeSelect()
        End If

        If Me.ActiveControl.Name <> dgv_Production_Wages_Details.Name Then
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

    Private Sub Grid_DeSelect()
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_Details_Total.CurrentCell) Then dgv_Details_Total.CurrentCell.Selected = False

        If Not IsNothing(dgv_Production_Wages_Details.CurrentCell) Then dgv_Production_Wages_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_production_wages_Total.CurrentCell) Then dgv_production_wages_Total.CurrentCell.Selected = False

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

        If Not IsNothing(dgv_Production_Wages_Details.CurrentCell) Then dgv_Production_Wages_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_production_wages_Total.CurrentCell) Then dgv_production_wages_Total.CurrentCell.Selected = False
    End Sub

    Private Sub Piece_Checking_InHouse_Format1_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_LoomNo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LOOMNO" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_LoomNo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

    Private Sub Piece_Checking_InHouse_Format1_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Piece_Checking_InHouse_Format1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable

        Me.Text = ""

        con.Open()

        lbl_RollNo_Heading.Text = StrConv(Common_Procedures.settings.ClothReceipt_LotNo_OR_RollNo_Text, vbProperCase)
        lbl_PrintFrom_Caption.Text = lbl_RollNo_Heading.Text
        dgv_Details.Columns(dgvCOL_PCSDETAILS.PCSNO).HeaderText = StrConv(Common_Procedures.settings.ClothReceipt_PieceNo_OR_RollNo_Text, vbUpperCase)

        If Trim(UCase(Common_Procedures.settings.ClothReceipt_PieceNo_Concept)) = Trim(UCase("CONTINUOUS NO")) Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1041" Or Common_Procedures.settings.CustomerCode = "1410" Or Common_Procedures.settings.CustomerCode = "1608" Then
            dgv_Details.Columns(dgvCOL_PCSDETAILS.PCSNO).ReadOnly = False
            dgv_Details.Columns(dgvCOL_PCSDETAILS.PCSNO).DefaultCellStyle.Alignment = 0
        Else
            dgv_Details.Columns(dgvCOL_PCSDETAILS.PCSNO).ReadOnly = True
            dgv_Details.Columns(dgvCOL_PCSDETAILS.PCSNO).DefaultCellStyle.Alignment = 2
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1234" Then  '-------ARULJOTHI TEXTILES
            lbl_ClothName.Width = lbl_PartyName.Width
            txt_BarCode.Visible = True
            txt_BarCode.BackColor = Color.White
            lbl_BarCode_Caption.Visible = True
            'btn_SaveAll.Visible = True
        End If

        cbo_Grid_ClothType.Visible = False
        cbo_grid_employee.Visible = False

        dtp_Date.Text = ""

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Print.Visible = False
        pnl_Print.Left = (Me.Width - pnl_Print.Width) \ 2
        pnl_Print.Top = (Me.Height - pnl_Print.Height) \ 2
        pnl_Print.BringToFront()

        pnl_Weft_Consumption_Details.Visible = False
        pnl_Weft_Consumption_Details.Left = (Me.Width - pnl_Weft_Consumption_Details.Width) \ 2
        pnl_Weft_Consumption_Details.Top = (Me.Height - pnl_Weft_Consumption_Details.Height) \ 2

        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If

        dgv_Production_Wages_Details.Visible = False
        dgv_production_wages_Total.Visible = False
        cbo_grid_employee.Visible = False

        If Common_Procedures.settings.Show_Sales_OrderNumber_in_ALLEntry_Status = 1 Then

            cbo_ClothSales_OrderCode_forSelection.Visible = True
            lbl_ClothSales_OrderCode_forSelection_Caption.Visible = True

            FnYearCode1 = ""
            FnYearCode2 = ""
            Common_Procedures.get_FnYearCode_of_Last_2_Years(FnYearCode1, FnYearCode2)

        Else

            cbo_ClothSales_OrderCode_forSelection.Visible = False
            lbl_ClothSales_OrderCode_forSelection_Caption.Visible = False

        End If


        If Common_Procedures.settings.CustomerCode = "--1370--" Then   'akill or samanth

            Lbl_caption_checker.Visible = False
            lbl_caption_wagespick.Visible = False
            lbl_caption_amt.Visible = False

            cbo_KnotterName.Visible = False
            txt_Wages.Visible = False
            lbl_Wages_Amount.Visible = False

            cbo_grid_employee.Visible = True

            dgv_Production_Wages_Details.Visible = True
            dgv_production_wages_Total.Visible = True

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '---- MANI OMEGA FABRICS (THIRUCHENKODU)

            Lbl_caption_checker.Visible = False
            lbl_caption_wagespick.Visible = False
            lbl_caption_amt.Visible = False

            cbo_KnotterName.Visible = False
            txt_Wages.Visible = False
            lbl_Wages_Amount.Visible = False

            lbl_FabricLotNo_Caption.Visible = True
            lbl_FabricLotNo.Visible = True
            lbl_FabricLotNo.BackColor = Color.White
            lbl_FabricLotNo.Left = lbl_RecMtrs.Left
            lbl_FabricLotNo.Top = lbl_RecMtrs.Top + lbl_RecMtrs.Height + (lbl_RecMtrs.Top - (lbl_PartyName.Top + lbl_PartyName.Height) + 20)
            lbl_FabricLotNo.Width = lbl_ClothName.Width
            lbl_FabricLotNo_Caption.Left = lbl_RecMtrs_Caption.Left
            lbl_FabricLotNo_Caption.Top = lbl_FabricLotNo.Top + 4

            dgv_Details_Total.Top = dgv_production_wages_Total.Top
            dgv_Details.Top = lbl_FabricLotNo.Top + lbl_FabricLotNo.Height + +(lbl_RecMtrs.Top - (lbl_PartyName.Top + lbl_PartyName.Height) + 10)
            dgv_Details.Height = dgv_production_wages_Total.Top - dgv_Details.Top + 1

            lbl_RecMtrs_Caption.Text = "Roll Doff Meters"
            lbl_RecDate_Caption.Text = "Doff Date"

            lbl_Fer_No_Caption.Text = "FER No"

            dgv_Details.Columns(dgvCOL_PCSDETAILS.PCSNO).HeaderText = "FER No."
            dgv_Details.Columns(dgvCOL_PCSDETAILS.GROSSWEIGHT).HeaderText = "GROSS WEIGHT"
            dgv_Details.Columns(dgvCOL_PCSDETAILS.NETWEIGHT).HeaderText = "NET WEIGHT"
            dgv_Details.Columns(dgvCOL_PCSDETAILS.BALENO).HeaderText = "ROLL NO"

            dgv_Details.Columns(dgvCOL_PCSDETAILS.PCSNO).Width = 100
            dgv_Details.Columns(dgvCOL_PCSDETAILS.CLOTHTYPE).Width = 105
            dgv_Details.Columns(dgvCOL_PCSDETAILS.METERS).Width = 75
            dgv_Details.Columns(dgvCOL_PCSDETAILS.GROSSWEIGHT).Width = 100
            dgv_Details.Columns(dgvCOL_PCSDETAILS.NETWEIGHT).Width = 80
            dgv_Details.Columns(dgvCOL_PCSDETAILS.WEIGHTPERMETER).Width = 75
            dgv_Details.Columns(dgvCOL_PCSDETAILS.STS).Width = 0
            dgv_Details.Columns(dgvCOL_PCSDETAILS.BALENO).Width = 120

            dgv_Details_Total.Columns(dgvCOL_PCSDETAILS.PCSNO).Width = 100
            dgv_Details_Total.Columns(dgvCOL_PCSDETAILS.CLOTHTYPE).Width = 105
            dgv_Details_Total.Columns(dgvCOL_PCSDETAILS.METERS).Width = 75
            dgv_Details_Total.Columns(dgvCOL_PCSDETAILS.GROSSWEIGHT).Width = 100
            dgv_Details_Total.Columns(dgvCOL_PCSDETAILS.NETWEIGHT).Width = 80
            dgv_Details_Total.Columns(dgvCOL_PCSDETAILS.WEIGHTPERMETER).Width = 75
            dgv_Details_Total.Columns(dgvCOL_PCSDETAILS.STS).Width = 0
            dgv_Details_Total.Columns(dgvCOL_PCSDETAILS.BALENO).Width = 120

            dgv_Details.Columns(dgvCOL_PCSDETAILS.GROSSWEIGHT).Visible = True
            dgv_Details_Total.Columns(dgvCOL_PCSDETAILS.GROSSWEIGHT).Visible = True

            dgv_Details.Columns(dgvCOL_PCSDETAILS.GROSSWEIGHT).ReadOnly = False
            dgv_Details.Columns(dgvCOL_PCSDETAILS.NETWEIGHT).ReadOnly = True

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1544" Then '---- SRI SRINIVASA TEXTILES (PALLADAM)
            dgv_Details.Columns(dgvCOL_PCSDETAILS.PCSNO).HeaderText = "ROLL No."
            lbl_RecMtrs_Caption.Text = "Roll Doff Meters"
            lbl_RecDate_Caption.Text = "Doff Date"

            Lbl_caption_checker.Visible = False
            lbl_caption_wagespick.Visible = False
            lbl_caption_amt.Visible = False

            cbo_KnotterName.Visible = False
            txt_Wages.Visible = False
            lbl_Wages_Amount.Visible = False

            dgv_Details_Total.Top = dgv_production_wages_Total.Top
            'dgv_Details.Top = lbl_FabricLotNo.Top + lbl_FabricLotNo.Height + (lbl_RecMtrs.Top - (lbl_PartyName.Top + lbl_PartyName.Height) + 10)
            dgv_Details.Height = dgv_production_wages_Total.Top - dgv_Details.Top + 1

        ElseIf Common_Procedures.settings.CustomerCode = "1608" Then   'samanth

            Lbl_caption_checker.Visible = True
            lbl_caption_wagespick.Visible = True
            lbl_caption_amt.Visible = True

            cbo_KnotterName.Visible = True
            txt_Wages.Visible = True
            lbl_Wages_Amount.Visible = True


        Else

            Lbl_caption_checker.Visible = False
            lbl_caption_wagespick.Visible = False
            lbl_caption_amt.Visible = False

            cbo_KnotterName.Visible = False
            txt_Wages.Visible = False
            lbl_Wages_Amount.Visible = False


            dgv_Details.Height = dgv_Details.Height + dgv_Production_Wages_Details.Height
            dgv_Details_Total.Top = dgv_Details.Top + dgv_Details.Height
            'dgv_Details.Top = lbl_FabricLotNo.Top + lbl_FabricLotNo.Height + +(lbl_RecMtrs.Top - (lbl_PartyName.Top + lbl_PartyName.Height) + 10)
            'dgv_Details.Height = dgv_production_wages_Total.Top - dgv_Details.Top + 1

        End If

        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Folding.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_ClothType.GotFocus, AddressOf ControlGotFocus
        ' AddHandler txt_RollNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_LoomNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Folding.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Crimp.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_KnotterName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Wages.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_LoomNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_BeamNo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_BarCode.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PrintFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PrintTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_grid_employee.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_LotNo.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_Filter_ClothName.GotFocus, AddressOf ControlGotFocus
        AddHandler Cbo_Filter_FerNo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_TareWeight.GotFocus, AddressOf ControlGotFocus



        AddHandler msk_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_LoomNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Folding.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Grid_ClothType.LostFocus, AddressOf ControlLostFocus
        ' AddHandler txt_RollNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Folding.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Crimp.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_KnotterName.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Wages.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_BarCode.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_TareWeight.LostFocus, AddressOf ControlLostFocus


        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_LoomNo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_BeamNo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PrintFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PrintTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_grid_employee.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_LotNo.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_Filter_ClothName.LostFocus, AddressOf ControlLostFocus
        AddHandler Cbo_Filter_FerNo.LostFocus, AddressOf ControlLostFocus

        AddHandler dtp_Date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_Fromdate.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler dtp_Filter_ToDate.KeyDown, AddressOf TextBoxControlKeyDown
        '  AddHandler txt_RollNo.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_Crimp.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_BarCode.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler txt_PrintFrom.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Crimp.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_BarCode.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_PrintFrom.KeyPress, AddressOf TextBoxControlKeyPress

        AddHandler cbo_ClothSales_OrderCode_forSelection.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothSales_OrderCode_forSelection.LostFocus, AddressOf ControlLostFocus

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

    End Sub

    Private Sub Piece_Checking_InHouse_Format1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Print.Visible = True Then
                    btn_Close_Print_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Weft_Consumption_Details.Visible Then
                    Call btn_Close_Weft_Consumption_Details_Click(sender, e)
                    Exit Sub

                ElseIf MessageBox.Show("Do you want to Close?", "FOR CLOSE", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    Close_Form()

                Else
                    Exit Sub

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim dgv1 As New DataGridView
        Dim FCol As Integer = 0
        Dim LCol As Integer = 0

        On Error Resume Next

        If ActiveControl.Name = dgv_Details.Name Or ActiveControl.Name = dgv_Production_Wages_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            dgv1 = Nothing

            If ActiveControl.Name = dgv_Details.Name Then
                dgv1 = dgv_Details

            ElseIf dgv_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Details

            ElseIf ActiveControl.Name = dgv_Production_Wages_Details.Name Then
                dgv1 = dgv_Production_Wages_Details

            ElseIf dgv_Production_Wages_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Production_Wages_Details

            ElseIf Pnl_Back.Enabled = True Then
                dgv1 = dgv_Details

            End If

            If IsNothing(dgv1) = False Then


                With dgv1


                    If dgv1.Name = dgv_Details.Name Then

                        If .Columns(dgvCOL_PCSDETAILS.PCSNO).ReadOnly = True Then
                            FCol = 1
                        Else
                            FCol = 0
                        End If

                        If .Columns(dgvCOL_PCSDETAILS.GROSSWEIGHT).Visible = True Then
                            LCol = dgvCOL_PCSDETAILS.GROSSWEIGHT
                        Else
                            LCol = dgvCOL_PCSDETAILS.NETWEIGHT
                        End If


                        If keyData = Keys.Enter Or keyData = Keys.Down Then

                            If .CurrentCell.ColumnIndex >= LCol Then

                                If .CurrentCell.RowIndex = .RowCount - 1 Then

                                    If dgv_Production_Wages_Details.Visible = True Then
                                        dgv_Production_Wages_Details.Focus()
                                        dgv_Production_Wages_Details.CurrentCell = dgv_Production_Wages_Details.Rows(0).Cells(dgvCOL_PRODUCTIONDETAILS.EMPLOYEENAME)

                                    Else

                                        If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                                            save_record()
                                        Else
                                            dtp_Date.Focus()
                                        End If

                                    End If


                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(FCol)

                                End If

                            Else

                                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= dgvCOL_PCSDETAILS.CLOTHTYPE And Trim(.CurrentRow.Cells(dgvCOL_PCSDETAILS.CLOTHTYPE).Value) = "" Then

                                    If dgv_Production_Wages_Details.Visible = True Then
                                        dgv_Production_Wages_Details.Focus()
                                        dgv_Production_Wages_Details.CurrentCell = dgv_Production_Wages_Details.Rows(0).Cells(dgvCOL_PRODUCTIONDETAILS.EMPLOYEENAME)

                                    Else

                                        If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                                            save_record()
                                        Else
                                            dtp_Date.Focus()
                                        End If

                                    End If

                                ElseIf .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.METERS Then
                                    If .Columns(dgvCOL_PCSDETAILS.GROSSWEIGHT).Visible = True Then
                                        .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.GROSSWEIGHT)
                                    Else
                                        .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.NETWEIGHT)
                                    End If

                                Else
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                                End If

                            End If

                            Return True

                        ElseIf keyData = Keys.Up Then

                            If .CurrentCell.ColumnIndex <= FCol Then
                                If .CurrentCell.RowIndex = 0 Then

                                    If cbo_ClothSales_OrderCode_forSelection.Visible = True Then
                                        cbo_ClothSales_OrderCode_forSelection.Focus()
                                    Else
                                        If txt_Folding.Enabled Then txt_Folding.Focus() Else dtp_Date.Focus()
                                    End If

                                Else
                                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(LCol)

                                End If

                            ElseIf .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.NETWEIGHT Then
                                If .Columns(dgvCOL_PCSDETAILS.GROSSWEIGHT).Visible = True Then
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.GROSSWEIGHT)
                                Else
                                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(dgvCOL_PCSDETAILS.METERS)
                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)

                            End If

                            Return True

                        Else
                            Return MyBase.ProcessCmdKey(msg, keyData)

                        End If


                    ElseIf dgv1.Name = dgv_Production_Wages_Details.Name Then

                        If keyData = Keys.Enter Or keyData = Keys.Down Then

                            If .CurrentCell.ColumnIndex >= .ColumnCount - 2 Then

                                If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                                    save_record()
                                Else
                                    dtp_Date.Focus()
                                End If

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If

                            Return True

                        ElseIf keyData = Keys.Up Then

                            If .CurrentCell.ColumnIndex <= dgvCOL_PRODUCTIONDETAILS.EMPLOYEENAME Then

                                If dgv_Details.Rows.Count > 0 Then
                                    dgv_Details.Focus()
                                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCOL_PRODUCTIONDETAILS.EMPLOYEENAME)
                                Else
                                    txt_Folding.Focus()
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

    Private Sub move_record(ByVal no As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim da3 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim NewCode As String
        Dim n As Integer, i As Integer, j As Integer
        Dim SNo As Integer = 0
        Dim Lm_ID As Integer = 0
        Dim LockSTS As Boolean = False
        Dim vMINWGTperMTR As String, vMAXWGTperMTR As String


        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            vMINWGTperMTR = 0 : vMAXWGTperMTR = 0
            da1 = New SqlClient.SqlDataAdapter("select a.*, tQ.Cloth_Name, tQ.Weight_Meter_Min, tQ.Weight_Meter_Max, tQ.Multiple_WeftCount_Status  from Weaver_Piece_Checking_Head a INNER JOIN cloth_Head tQ ON a.cloth_IdNo = tQ.Cloth_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Receipt_Type = 'L'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_ChkNo.Text = dt1.Rows(0).Item("Weaver_Piece_Checking_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Weaver_Piece_Checking_Date").ToString
                msk_Date.Text = dtp_Date.Text
                'txt_RollNo.Text = dt1.Rows(0).Item("Piece_Receipt_No").ToString
                'txt_RollNo.Tag = txt_RollNo.Text
                cbo_LotNo.Text = dt1.Rows(0).Item("Piece_Receipt_No").ToString
                lbl_RecCode.Text = dt1.Rows(0).Item("Piece_Receipt_Code").ToString
                lbl_RecDate.Text = Format(Convert.ToDateTime(dt1.Rows(0).Item("Piece_Receipt_Date").ToString), "dd-MM-yyyy").ToString
                lbl_PartyName.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("Ledger_IdNo").ToString))
                lbl_ClothName.Text = dt1.Rows(0).Item("cloth_Name").ToString  ' Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("Cloth_IdNo").ToString))
                lbl_RecMtrs.Text = dt1.Rows(0).Item("ReceiptMeters_Receipt").ToString
                txt_Crimp.Text = Val(dt1.Rows(0).Item("Crimp_Percentage").ToString)
                txt_Folding.Text = Val(dt1.Rows(0).Item("Folding").ToString)
                cbo_KnotterName.Text = Common_Procedures.Employee_IdNoToName(con, (dt1.Rows(0).Item("Employee_IdNo").ToString))
                txt_Wages.Text = dt1.Rows(0).Item("Wages_pICK").ToString
                lbl_Wages_Amount.Text = dt1.Rows(0).Item("Wages_Amount").ToString
                txt_BarCode.Text = dt1.Rows(0).Item("Bar_Code").ToString
                txt_TareWeight.Text = dt1.Rows(0).Item("Roll_Tare_Weight").ToString
                lbl_FabricLotNo.Text = dt1.Rows(0).Item("Fabric_LotNo").ToString
                lbl_ExcSht.Text = dt1.Rows(0).Item("Excess_Short_Meter").ToString


                vMINWGTperMTR = Format(Val(dt1.Rows(0).Item("Weight_Meter_Min").ToString), "##########0.000")
                vMAXWGTperMTR = Format(Val(dt1.Rows(0).Item("Weight_Meter_Max").ToString), "##########0.000")

                cbo_ClothSales_OrderCode_forSelection.Text = dt1.Rows(0).Item("ClothSales_OrderCode_forSelection").ToString

                If Val(dt1.Rows(0).Item("Multiple_WeftCount_Status").ToString) = 1 Then
                    btn_Show_WeftConsumption_Details.Visible = True
                    btn_Show_WeftConsumption_Details.BringToFront()
                Else
                    btn_Show_WeftConsumption_Details.Visible = False
                End If

                Lm_ID = 0
                lbl_ConsPavu.Text = ""
                lbl_ConsWeftYarn.Text = ""
                da1 = New SqlClient.SqlDataAdapter("select Loom_IdNo, ConsumedPavu_Checking, ConsumedYarn_Checking from Weaver_Cloth_Receipt_Head Where Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "'", con)
                dt2 = New DataTable
                da1.Fill(dt2)
                If dt2.Rows.Count > 0 Then
                    Lm_ID = Val(dt2.Rows(0).Item("Loom_IdNo").ToString)
                    lbl_ConsPavu.Text = dt2.Rows(0).Item("ConsumedPavu_Checking").ToString
                    lbl_ConsWeftYarn.Text = dt2.Rows(0).Item("ConsumedYarn_Checking").ToString
                End If
                dt2.Clear()

                cbo_LoomNo.Text = Common_Procedures.Loom_IdNoToName(con, Val(Lm_ID))

                LockSTS = False

                da2 = New SqlClient.SqlDataAdapter("Select a.*, b.Loom_Name from Weaver_ClothReceipt_Piece_Details a LEFT OUTER JOIN Loom_Head b ON a.Loom_IdNo = b.Loom_IdNo where a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' Order by a.Piece_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_Details

                    .Rows.Clear()
                    SNo = 0

                    If dt2.Rows.Count > 0 Then

                        For i = 0 To dt2.Rows.Count - 1

                            n = .Rows.Add()

                            .Rows(n).Cells(dgvCOL_PCSDETAILS.PCSNO).Value = dt2.Rows(i).Item("Piece_No").ToString
                            .Rows(n).Cells(dgvCOL_PCSDETAILS.STS).Value = ""
                            .Rows(n).Cells(dgvCOL_PCSDETAILS.BALENO).Value = ""

                            If Val(dt2.Rows(i).Item("Type1_Meters").ToString) <> 0 Then
                                .Rows(n).Cells(dgvCOL_PCSDETAILS.CLOTHTYPE).Value = Common_Procedures.ClothType.Type1
                                .Rows(n).Cells(dgvCOL_PCSDETAILS.METERS).Value = Format(Val(dt2.Rows(i).Item("Type1_Meters").ToString), "########0.00")
                                .Rows(n).Cells(dgvCOL_PCSDETAILS.BALENO).Value = dt2.Rows(i).Item("PackingSlip_Code_Type1").ToString
                                If Trim(.Rows(n).Cells(dgvCOL_PCSDETAILS.BALENO).Value) = "" Then .Rows(n).Cells(dgvCOL_PCSDETAILS.BALENO).Value = dt2.Rows(i).Item("BuyerOffer_Code_Type1").ToString
                                If Trim(.Rows(n).Cells(dgvCOL_PCSDETAILS.BALENO).Value) <> "" Then
                                    .Rows(n).Cells(dgvCOL_PCSDETAILS.STS).Value = "1"

                                    LockSTS = True
                                    For j = 0 To .ColumnCount - 1
                                        .Rows(n).Cells(j).Style.BackColor = Color.Gainsboro
                                        .Rows(n).Cells(j).Style.ForeColor = Color.Red
                                    Next

                                End If


                            ElseIf Val(dt2.Rows(i).Item("Type2_Meters").ToString) <> 0 Then

                                .Rows(n).Cells(dgvCOL_PCSDETAILS.CLOTHTYPE).Value = Common_Procedures.ClothType.Type2
                                .Rows(n).Cells(dgvCOL_PCSDETAILS.METERS).Value = Format(Val(dt2.Rows(i).Item("Type2_Meters").ToString), "########0.00")
                                .Rows(n).Cells(dgvCOL_PCSDETAILS.BALENO).Value = dt2.Rows(i).Item("PackingSlip_Code_Type2").ToString
                                If Trim(.Rows(n).Cells(dgvCOL_PCSDETAILS.BALENO).Value) = "" Then .Rows(n).Cells(dgvCOL_PCSDETAILS.BALENO).Value = dt2.Rows(i).Item("BuyerOffer_Code_Type2").ToString
                                If Trim(.Rows(n).Cells(dgvCOL_PCSDETAILS.BALENO).Value) <> "" Then
                                    .Rows(n).Cells(dgvCOL_PCSDETAILS.STS).Value = "1"
                                    .Rows(n).Cells(dgvCOL_PCSDETAILS.CLOTHTYPE).Style.ForeColor = Color.Red
                                    LockSTS = True
                                    For j = 0 To .ColumnCount - 1
                                        .Rows(n).Cells(j).Style.BackColor = Color.Gainsboro
                                        .Rows(n).Cells(j).Style.ForeColor = Color.Red
                                    Next
                                End If

                            ElseIf Val(dt2.Rows(i).Item("Type3_Meters").ToString) <> 0 Then


                                .Rows(n).Cells(dgvCOL_PCSDETAILS.CLOTHTYPE).Value = Common_Procedures.ClothType.Type3
                                .Rows(n).Cells(dgvCOL_PCSDETAILS.METERS).Value = Format(Val(dt2.Rows(i).Item("Type3_Meters").ToString), "########0.00")
                                .Rows(n).Cells(dgvCOL_PCSDETAILS.BALENO).Value = dt2.Rows(i).Item("PackingSlip_Code_Type3").ToString
                                If Trim(.Rows(n).Cells(dgvCOL_PCSDETAILS.BALENO).Value) = "" Then .Rows(n).Cells(dgvCOL_PCSDETAILS.BALENO).Value = dt2.Rows(i).Item("BuyerOffer_Code_Type3").ToString
                                If Trim(.Rows(n).Cells(dgvCOL_PCSDETAILS.BALENO).Value) <> "" Then
                                    .Rows(n).Cells(dgvCOL_PCSDETAILS.STS).Value = "1"
                                    .Rows(n).Cells(dgvCOL_PCSDETAILS.CLOTHTYPE).Style.ForeColor = Color.Red
                                    LockSTS = True
                                    For j = 0 To .ColumnCount - 1
                                        .Rows(n).Cells(j).Style.BackColor = Color.Gainsboro
                                        .Rows(n).Cells(j).Style.ForeColor = Color.Red
                                    Next
                                End If

                            ElseIf Val(dt2.Rows(i).Item("Type4_Meters").ToString) <> 0 Then
                                .Rows(n).Cells(dgvCOL_PCSDETAILS.CLOTHTYPE).Value = Common_Procedures.ClothType.Type4
                                .Rows(n).Cells(dgvCOL_PCSDETAILS.METERS).Value = Format(Val(dt2.Rows(i).Item("Type4_Meters").ToString), "########0.00")
                                .Rows(n).Cells(dgvCOL_PCSDETAILS.BALENO).Value = dt2.Rows(i).Item("PackingSlip_Code_Type4").ToString
                                If Trim(.Rows(n).Cells(dgvCOL_PCSDETAILS.BALENO).Value) = "" Then .Rows(n).Cells(dgvCOL_PCSDETAILS.BALENO).Value = dt2.Rows(i).Item("BuyerOffer_Code_Type4").ToString
                                If Trim(.Rows(n).Cells(dgvCOL_PCSDETAILS.BALENO).Value) <> "" Then
                                    .Rows(n).Cells(dgvCOL_PCSDETAILS.STS).Value = "1"
                                    .Rows(n).Cells(dgvCOL_PCSDETAILS.CLOTHTYPE).Style.ForeColor = Color.Red
                                    LockSTS = True
                                    For j = 0 To .ColumnCount - 1
                                        .Rows(n).Cells(j).Style.BackColor = Color.Gainsboro
                                        .Rows(n).Cells(j).Style.ForeColor = Color.Red
                                    Next
                                End If

                            ElseIf Val(dt2.Rows(i).Item("Type5_Meters").ToString) <> 0 Then
                                .Rows(n).Cells(dgvCOL_PCSDETAILS.CLOTHTYPE).Value = Common_Procedures.ClothType.Type5
                                .Rows(n).Cells(dgvCOL_PCSDETAILS.METERS).Value = Format(Val(dt2.Rows(i).Item("Type5_Meters").ToString), "########0.00")
                                .Rows(n).Cells(dgvCOL_PCSDETAILS.BALENO).Value = dt2.Rows(i).Item("PackingSlip_Code_Type5").ToString
                                If Trim(.Rows(n).Cells(dgvCOL_PCSDETAILS.BALENO).Value) = "" Then .Rows(n).Cells(dgvCOL_PCSDETAILS.BALENO).Value = dt2.Rows(i).Item("BuyerOffer_Code_Type5").ToString
                                If Trim(.Rows(n).Cells(dgvCOL_PCSDETAILS.BALENO).Value) <> "" Then
                                    .Rows(n).Cells(dgvCOL_PCSDETAILS.STS).Value = "1"
                                    .Rows(n).Cells(dgvCOL_PCSDETAILS.CLOTHTYPE).Style.ForeColor = Color.Red
                                    LockSTS = True
                                    For j = 0 To .ColumnCount - 1
                                        .Rows(n).Cells(j).Style.BackColor = Color.Gainsboro
                                        .Rows(n).Cells(j).Style.ForeColor = Color.Red
                                    Next
                                End If

                            End If

                            .Rows(n).Cells(dgvCOL_PCSDETAILS.GROSSWEIGHT).Value = Val(dt2.Rows(i).Item("Roll_Gross_Weight").ToString)
                            .Rows(n).Cells(dgvCOL_PCSDETAILS.NETWEIGHT).Value = Val(dt2.Rows(i).Item("Weight").ToString)
                            .Rows(n).Cells(dgvCOL_PCSDETAILS.WEIGHTPERMETER).Value = Format(Val(dt2.Rows(i).Item("Weight_Meter").ToString), "########0.000")


                            If Val(vMINWGTperMTR) > 0 And Val(vMAXWGTperMTR) > 0 And Val(.Rows(n).Cells(dgvCOL_PCSDETAILS.WEIGHTPERMETER).Value) > 0 Then

                                If Not (Val(.Rows(n).Cells(dgvCOL_PCSDETAILS.WEIGHTPERMETER).Value) >= Val(vMINWGTperMTR) And Val(.Rows(n).Cells(dgvCOL_PCSDETAILS.WEIGHTPERMETER).Value) <= Val(vMAXWGTperMTR)) Then

                                    .Rows(n).Cells(dgvCOL_PCSDETAILS.GROSSWEIGHT).Style.BackColor = Color.Maroon
                                    .Rows(n).Cells(dgvCOL_PCSDETAILS.GROSSWEIGHT).Style.ForeColor = Color.Red

                                    .Rows(n).Cells(dgvCOL_PCSDETAILS.NETWEIGHT).Style.BackColor = Color.Maroon
                                    .Rows(n).Cells(dgvCOL_PCSDETAILS.NETWEIGHT).Style.ForeColor = Color.Red

                                    .Rows(n).Cells(dgvCOL_PCSDETAILS.WEIGHTPERMETER).Style.BackColor = Color.Maroon
                                    .Rows(n).Cells(dgvCOL_PCSDETAILS.WEIGHTPERMETER).Style.ForeColor = Color.Red

                                End If

                            End If

                        Next i

                    End If

                    n = .Rows.Count - 1
                    If (Trim(.Rows(n).Cells(dgvCOL_PCSDETAILS.CLOTHTYPE).Value) = "" And Val(.Rows(n).Cells(dgvCOL_PCSDETAILS.METERS).Value) <> 0) Or (.Rows(n).Cells(dgvCOL_PCSDETAILS.CLOTHTYPE).Value = Nothing And .Rows(n).Cells(dgvCOL_PCSDETAILS.METERS).Value = Nothing) Then
                        .Rows(n).Cells(dgvCOL_PCSDETAILS.PCSNO).Value = ""
                    End If

                End With



                da3 = New SqlClient.SqlDataAdapter("Select a.*, b.Employee_Name from Weaver_Production_Wages_Details a inner join PayRoll_Employee_Head b On a.Employee_idno = b.Employee_idno Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'", con)
                dt3 = New DataTable
                da3.Fill(dt3)
                With dgv_Production_Wages_Details
                    .Rows.Clear()
                    SNo = 0

                    If dt3.Rows.Count > 0 Then

                        For i = 0 To dt3.Rows.Count - 1

                            n = .Rows.Add()

                            SNo = SNo + 1

                            .Rows(n).Cells(dgvCOL_PRODUCTIONDETAILS.SLNO).Value = SNo
                            .Rows(n).Cells(dgvCOL_PRODUCTIONDETAILS.EMPLOYEENAME).Value = dt3.Rows(i).Item("Employee_Name").ToString
                            .Rows(n).Cells(dgvCOL_PRODUCTIONDETAILS.METERS).Value = Format(Val(dt3.Rows(i).Item("Wages_Meters").ToString), "########0.00")
                            .Rows(n).Cells(dgvCOL_PRODUCTIONDETAILS.RATE).Value = Format(Val(dt3.Rows(i).Item("Wages_Rate").ToString), "########0.00")
                            .Rows(n).Cells(dgvCOL_PRODUCTIONDETAILS.AMOUNT).Value = Format(Val(dt3.Rows(i).Item("Wages_Amount").ToString), "########0.00")

                        Next i

                    End If
                End With

                With dgv_production_wages_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(dgvCOL_PRODUCTIONDETAILS.METERS).Value = Format(Val(dt1.Rows(0).Item("Total_Wages_Meters").ToString), "########0.00")
                    .Rows(0).Cells(dgvCOL_PRODUCTIONDETAILS.AMOUNT).Value = Format(Val(dt1.Rows(0).Item("Total_Wages_Amount").ToString), "########0.00")
                End With

                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(dgvCOL_PCSDETAILS.METERS).Value = Format(Val(dt1.Rows(0).Item("Total_Checking_Meters").ToString), "########0.00")
                    .Rows(0).Cells(dgvCOL_PCSDETAILS.GROSSWEIGHT).Value = Format(Val(dt1.Rows(0).Item("Total_Gross_Weight").ToString), "########0.000")
                    .Rows(0).Cells(dgvCOL_PCSDETAILS.NETWEIGHT).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")
                End With

                dt2.Clear()


                dgv_Weft_Consumption_Details.Rows.Clear()
                da1 = New SqlClient.SqlDataAdapter("Select a.*, b.count_name from Weaver_ClothReceipt_Consumed_Yarn_Details a INNER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo Where a.Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'", con)
                dt4 = New DataTable
                da1.Fill(dt4)
                If dt4.Rows.Count > 0 Then
                    For i = 0 To dt4.Rows.Count - 1
                        n = dgv_Weft_Consumption_Details.Rows.Add()
                        dgv_Weft_Consumption_Details.Rows(n).Cells(0).Value = dt4.Rows(i).Item("count_name").ToString
                        dgv_Weft_Consumption_Details.Rows(n).Cells(1).Value = dt4.Rows(i).Item("Gram_Perc_Type").ToString
                        dgv_Weft_Consumption_Details.Rows(n).Cells(2).Value = Val(dt4.Rows(i).Item("Consumption_Gram_Perc").ToString)
                        dgv_Weft_Consumption_Details.Rows(n).Cells(3).Value = Format(Val(dt4.Rows(i).Item("Consumed_Yarn_Weight").ToString), "#########0.000")
                    Next

                End If
                dt4.Clear()



                da2 = New SqlClient.SqlDataAdapter("select Weaver_Wages_Code from Weaver_Cloth_Receipt_Head Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                If dt2.Rows.Count > 0 Then
                    If IsDBNull(dt2.Rows(0).Item("Weaver_Wages_Code").ToString) = False Then
                        If Trim(dt2.Rows(0).Item("Weaver_Wages_Code").ToString) <> "" Then
                            LockSTS = True
                        End If
                    End If
                End If
                dt1.Clear()

                If LockSTS = True Then

                    cbo_LotNo.Enabled = False
                    cbo_LotNo.BackColor = Color.Gainsboro

                    txt_Crimp.Enabled = False
                    txt_Crimp.BackColor = Color.Gainsboro

                    txt_Folding.Enabled = False
                    txt_Folding.BackColor = Color.Gainsboro

                    btn_Selection.Enabled = False

                End If

                dt2.Dispose()
                da2.Dispose()

            Else
                new_record()

            End If

            dt1.Clear()
            dt1.Dispose()
            da1.Dispose()

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim vOrdByNo As String = ""
        Dim Old_SetCd1 As String, Old_Beam1 As String
        Dim Old_SetCd2 As String, Old_Beam2 As String
        Dim Old_BMKNOTCd As String
        Dim Old_CLTH_Idno As Integer
        Dim vBEAM_ProdMeters As String
        Dim vErrMsg As String
        Dim SQL1 As String
        Dim vMULTIWFT_STS As Integer
        Dim vOLD_PCSDOFCODE As String

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_ChkNo.Text)

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Inhouse_Piece_Checking_Entry, New_Entry, Me, con, "Weaver_Piece_Checking_Head", "Weaver_Piece_Checking_Code", NewCode, "Weaver_Piece_Checking_Date", "(Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub


        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Inhouse_Piece_Checking_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Inhouse_Piece_Checking_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select COUNT(*) from Weaver_ClothReceipt_Piece_Details Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and (PackingSlip_Code_Type1 <> '' or PackingSlip_Code_Type2 <> '' or PackingSlip_Code_Type3 <> '' or PackingSlip_Code_Type4 <> '' or PackingSlip_Code_Type5 <> '' or BuyerOffer_Code_Type1 <> '' or BuyerOffer_Code_Type2 <> '' or BuyerOffer_Code_Type3 <> '' or BuyerOffer_Code_Type4 <> '' or BuyerOffer_Code_Type5 <> '')", con)
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

        Da = New SqlClient.SqlDataAdapter("select Weaver_Wages_Code from Weaver_Cloth_Receipt_Head Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("Weaver_Wages_Code").ToString) = False Then
                If Trim(Dt1.Rows(0).Item("Weaver_Wages_Code").ToString) <> "" Then
                    MessageBox.Show("Weaver Wages prepared", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Exit Sub

                End If
            End If
        End If
        Dt1.Clear()

        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans

            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Weaver_Piece_Checking_Head", "Weaver_Piece_Checking_Code", Val(lbl_Company.Tag), NewCode, lbl_ChkNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Weaver_Piece_Checking_Code, Company_IdNo, for_OrderBy", trans)

            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Weaver_ClothReceipt_Piece_Details", "Weaver_Piece_Checking_Code", Val(lbl_Company.Tag), NewCode, lbl_ChkNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "Piece_No,PieceNo_OrderBy,ReceiptMeters_Checking", "Sl_No", "Weaver_Piece_Checking_Code, For_OrderBy, Company_IdNo, Weaver_Piece_Checking_No, Weaver_Piece_Checking_Date, Ledger_Idno", trans)


            Old_SetCd1 = ""
            Old_Beam1 = ""
            Old_SetCd2 = ""
            Old_Beam2 = ""
            Old_BMKNOTCd = ""
            Old_CLTH_Idno = 0
            vOLD_PCSDOFCODE = ""
            vMULTIWFT_STS = 0
            Da = New SqlClient.SqlDataAdapter("Select a.Weaver_ClothReceipt_Code, a.set_code1, a.beam_no1, a.set_code2, a.beam_no2, a.Beam_Knotting_Code, tQ.Cloth_IdNo, tQ.Multiple_WeftCount_Status from Weaver_Cloth_Receipt_Head a INNER JOIN Cloth_Head tQ ON a.Cloth_IdNo = tQ.Cloth_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'", con)
            'Da = New SqlClient.SqlDataAdapter("Select * from Weaver_Cloth_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'", con)
            Da.SelectCommand.Transaction = trans
            Dt1 = New DataTable
            Da.Fill(Dt1)
            If Dt1.Rows.Count > 0 Then
                Old_SetCd1 = Dt1.Rows(0).Item("set_code1").ToString
                Old_Beam1 = Dt1.Rows(0).Item("beam_no1").ToString
                Old_SetCd2 = Dt1.Rows(0).Item("set_code2").ToString
                Old_Beam2 = Dt1.Rows(0).Item("beam_no2").ToString
                Old_BMKNOTCd = Dt1.Rows(0).Item("Beam_Knotting_Code").ToString
                Old_CLTH_Idno = Val(Dt1.Rows(0).Item("Cloth_IdNo").ToString)
                vOLD_PCSDOFCODE = Dt1.Rows(0).Item("Weaver_ClothReceipt_Code").ToString
                vMULTIWFT_STS = Val(Dt1.Rows(0).Item("Multiple_WeftCount_Status").ToString)

            End If





            ''----- Less Checking Meters (Consumption)
            'cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters - b.BeamConsumption_Checking from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and  b.set_code1 <> '' and b.Beam_No1 <> '' and a.set_code = b.Set_code1 and a.Beam_No = b.Beam_No1"
            'cmd.ExecuteNonQuery()
            'cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters - b.BeamConsumption_Checking from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and  b.set_code2 <> '' and b.Beam_No2 <> '' and a.set_code = b.Set_code2 and a.Beam_No = b.Beam_No2"
            'cmd.ExecuteNonQuery()
            ''----- Add Doffing Meters (Consumption)
            'cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters + b.BeamConsumption_Receipt from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and  b.set_code1 <> '' and b.Beam_No1 <> '' and a.set_code = b.Set_code1 and a.Beam_No = b.Beam_No1"
            'cmd.ExecuteNonQuery()
            'cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters + b.BeamConsumption_Receipt from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and  b.set_code2 <> '' and b.Beam_No2 <> '' and a.set_code = b.Set_code2 and a.Beam_No = b.Beam_No2"
            'cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Weaver_ClothReceipt_Consumed_Yarn_Details where Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If vMULTIWFT_STS = 1 Then

                cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Reference_Code = '" & Trim(Pk_Condition2) & Trim(vOLD_PCSDOFCODE) & "' "
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Insert into Stock_Yarn_Processing_Details (              Reference_Code                ,   Company_IdNo,   Reference_No          ,   for_OrderBy,   Reference_Date          ,                                               DeliveryTo_Idno         ,                                              ReceivedFrom_Idno         ,                  Entry_ID                               ,          Particulars                                                                                                                             ,   Party_Bill_No         ,         Sl_No,   Count_IdNo, Yarn_Type, Mill_IdNo, Bags, Cones,   Weight              ,   ClothSales_OrderCode_forSelection  ) " &
                                                "           Select  '" & Trim(Pk_Condition2) & "' + b.Weaver_ClothReceipt_Code, b.Company_IdNo, b.Weaver_ClothReceipt_No, b.for_OrderBy, b.Weaver_ClothReceipt_date, (CASE WHEN tP.Ledger_Type = 'JOBWORKER' THEN b.Ledger_IdNo ELSE 0 END), (CASE WHEN tP.Ledger_Type <> 'JOBWORKER' THEN b.Ledger_IdNo ELSE 0 END), '" & Trim(Pk_Condition2) & "' + b.Weaver_ClothReceipt_No, 'Doff : Roll.No. ' + b.Weaver_ClothReceipt_No +  ', Cloth : ' + tQ.Cloth_Name + ', Meters : ' + cast(ROUND(b.ReceiptMeters_Receipt,2) as varchar), b.Weaver_ClothReceipt_No,  1000+a.Sl_No, a.Count_IdNo,   'MILL' ,     0    ,   0 ,    0 , a.Consumed_Yarn_Weight, b.ClothSales_OrderCode_forSelection  from Weaver_ClothReceipt_Consumed_Yarn_Details a INNER JOIN Weaver_Cloth_Receipt_Head b ON a.Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition2) & "' + b.Weaver_ClothReceipt_Code INNER JOIN Ledger_Head tP ON tP.ledger_idno = b.ledger_idno INNER JOIN Cloth_Head tQ ON tQ.cloth_idno = b.cloth_idno where a.Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition2) & Trim(vOLD_PCSDOFCODE) & "' "
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Stock_Yarn_Processing_Details set Particulars = 'Doff : Roll.No. ' + b.Weaver_ClothReceipt_No +  ', Cloth : ' + c.Cloth_Name + ', Meters : ' +  cast(ROUND(b.ReceiptMeters_Receipt,2) as varchar), Weight = b.ConsumedYarn_Receipt from Stock_Yarn_Processing_Details a, Weaver_Cloth_Receipt_Head b, Cloth_Head c Where b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(Pk_Condition2) & "' + b.Weaver_ClothReceipt_Code and b.Weaver_Wages_Code = '' and b.cloth_idno = c.cloth_idno"
                cmd.ExecuteNonQuery()

            End If


            cmd.CommandText = "Update Stock_Pavu_Processing_Details set Meters = b.ConsumedPavu_Receipt from Stock_Pavu_Processing_Details a, Weaver_Cloth_Receipt_Head b Where b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(Pk_Condition2) & "' + b.Weaver_ClothReceipt_Code and b.Weaver_Wages_Code = ''"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Stock_Cloth_Processing_Details set reference_date = (case when b.Weaver_Wages_Code <> '' then b.Weaver_Wages_Date else b.Weaver_ClothReceipt_Date end), Folding = b.Folding_Receipt, UnChecked_Meters = (case when b.Weaver_Wages_Code = '' then b.ReceiptMeters_Receipt else 0 end), Meters_Type1 = (case when b.Weaver_Wages_Code <> '' then b.Type1_Wages_Meters else 0 end), Meters_Type2 = (case when b.Weaver_Wages_Code <> '' then b.Type2_Wages_Meters else 0 end), Meters_Type3 = (case when b.Weaver_Wages_Code <> '' then b.Type3_Wages_Meters else 0 end), Meters_Type4 = (case when b.Weaver_Wages_Code <> '' then b.Type4_Wages_Meters else 0 end), Meters_Type5 = (case when b.Weaver_Wages_Code <> '' then b.Type5_Wages_Meters else 0 end) from Stock_Cloth_Processing_Details a, Weaver_Cloth_Receipt_Head b Where b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(Pk_Condition2) & "' + b.Weaver_ClothReceipt_Code"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Weaver_Piece_Checking_Code = '', Weaver_Piece_Checking_Increment = Weaver_Piece_Checking_Increment - 1, Weaver_Piece_Checking_Date = Null, Receipt_Meters = ReceiptMeters_Receipt, Folding = Folding_Receipt, Consumed_Yarn = ConsumedYarn_Receipt, Consumed_Pavu = ConsumedPavu_Receipt, BeamConsumption_Meters = BeamConsumption_Receipt, Folding_Checking = 0, ReceiptMeters_Checking = 0, ConsumedYarn_Checking = 0, ConsumedPavu_Checking = 0, BeamConsumption_Checking = 0, Type1_Checking_Meters = 0, Type2_Checking_Meters = 0, Type3_Checking_Meters = 0, Type4_Checking_Meters = 0, Type5_Checking_Meters = 0, Total_Checking_Meters = 0 Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()


            cmd.CommandText = "delete from Weaver_ClothReceipt_Piece_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "delete from Weaver_Piece_Checking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            If Trim(Old_SetCd1) <> "" And Trim(Old_Beam1) <> "" Then


                vBEAM_ProdMeters = 0
                vErrMsg = ""
                '----- Checking for negative beam meters
                If Common_Procedures.Check_SizedBeam_Doffing_Meters(con, Old_CLTH_Idno, Old_SetCd1, Old_Beam1, vBEAM_ProdMeters, vErrMsg, trans) = True Then
                    Throw New ApplicationException(vErrMsg)
                    Exit Sub

                Else

                    SQL1 = "Update Stock_SizedPavu_Processing_Details set Production_Meters = " & Str(Val(vBEAM_ProdMeters)) & " Where set_code = '" & Trim(Old_SetCd1) & "' and beam_no = '" & Trim(Old_Beam1) & "'"
                    cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
                    cmd.ExecuteNonQuery()

                End If

            End If

            If Trim(Old_SetCd2) <> "" And Trim(Old_Beam2) <> "" Then

                vBEAM_ProdMeters = 0
                vErrMsg = ""
                '----- Checking for negative beam meters
                If Common_Procedures.Check_SizedBeam_Doffing_Meters(con, Old_CLTH_Idno, Old_SetCd2, Old_Beam2, vBEAM_ProdMeters, vErrMsg, trans) = True Then
                    Throw New ApplicationException(vErrMsg)
                    Exit Sub

                Else

                    SQL1 = "Update Stock_SizedPavu_Processing_Details set Production_Meters = " & Str(Val(vBEAM_ProdMeters)) & " Where set_code = '" & Trim(Old_SetCd2) & "' and beam_no = '" & Trim(Old_Beam2) & "'"
                    cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
                    cmd.ExecuteNonQuery()

                End If

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

            If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

        End Try

    End Sub

    Public Sub filter_record() Implements Interface_MDIActions.filter_record

        If Filter_Status = False Then
            Dim Cmd As New SqlClient.SqlCommand
            Dim da As New SqlClient.SqlDataAdapter
            Dim dt1 As New DataTable
            Dim dt2 As New DataTable
            Dim dt3 As New DataTable

            da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'WEAVER') order by Ledger_DisplayName", con)
            dt1 = New DataTable
            da.Fill(dt1)
            cbo_Filter_PartyName.DataSource = dt1
            cbo_Filter_PartyName.DisplayMember = "Ledger_DisplayName"

            da = New SqlClient.SqlDataAdapter("select Loom_Name from Loom_head order by Loom_Name", con)
            dt2 = New DataTable
            da.Fill(dt2)
            cbo_Filter_LoomNo.DataSource = dt2
            cbo_Filter_LoomNo.DisplayMember = "Loom_Name"

            da = New SqlClient.SqlDataAdapter("select BeamNo_SetCode_forSelection from Stock_SizedPavu_Processing_Details order by BeamNo_SetCode_forSelection", con)
            dt3 = New DataTable
            da.Fill(dt3)
            cbo_Filter_BeamNo.DataSource = dt3
            cbo_Filter_BeamNo.DisplayMember = "BeamNo_SetCode_forSelection"

            Common_Procedures.Update_SizedPavu_BeamNo_for_Selection(con)
            'Cmd.Connection = con
            'Cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set BeamNo_SetCode_forSelection = Beam_No + ' | ' + setcode_forSelection Where Beam_No <> ''"
            'Cmd.ExecuteNonQuery()

            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_PartyName.Text = ""

            cbo_Filter_PartyName.SelectedIndex = -1
            cbo_Filter_LoomNo.SelectedIndex = -1
            cbo_Filter_BeamNo.SelectedIndex = -1

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

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Piece_Checking_No from Weaver_Piece_Checking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Receipt_Type = 'L' Order by for_Orderby, Weaver_Piece_Checking_No", con)
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
            MessageBox.Show(ex.Message, "FOR  MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub movenext_record() Implements Interface_MDIActions.movenext_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_ChkNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Piece_Checking_No from Weaver_Piece_Checking_Head where for_orderby > " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Receipt_Type = 'L' Order by for_Orderby, Weaver_Piece_Checking_No", con)
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

        End Try

    End Sub

    Public Sub moveprevious_record() Implements Interface_MDIActions.moveprevious_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""
        Dim OrdByNo As Single = 0

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_ChkNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Piece_Checking_No from Weaver_Piece_Checking_Head where for_orderby < " & Str(Val(OrdByNo)) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Receipt_Type = 'L'  Order by for_Orderby desc, Weaver_Piece_Checking_No desc", con)
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

        End Try

    End Sub

    Public Sub movelast_record() Implements Interface_MDIActions.movelast_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim movno As String = ""

        Try
            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Piece_Checking_No from Weaver_Piece_Checking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' and Receipt_Type = 'L' Order by for_Orderby desc, Weaver_Piece_Checking_No desc", con)
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

        End Try

    End Sub

    Public Sub new_record() Implements Interface_MDIActions.new_record
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewID As Integer = 0

        Try
            clear()

            New_Entry = True

            lbl_ChkNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Piece_Checking_Head", "Weaver_Piece_Checking_Code", "For_OrderBy", "(Receipt_Type = 'L')", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_ChkNo.ForeColor = Color.Red


            msk_Date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Weaver_Piece_Checking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Weaver_Piece_Checking_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Weaver_Piece_Checking_Date").ToString <> "" Then msk_Date.Text = dt1.Rows(0).Item("Weaver_Piece_Checking_Date").ToString
                End If
                If dt1.Rows(0).Item("Folding").ToString <> "" Then txt_Folding.Text = dt1.Rows(0).Item("Folding").ToString
            End If
            dt1.Clear()


            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

            inpno = InputBox("Enter Checking No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Weaver_Piece_Checking_No from Weaver_Piece_Checking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code = '" & Trim(RecCode) & "' and Receipt_Type = 'L'", con)
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
                MessageBox.Show("Checking No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FIND...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub insert_record() Implements Interface_MDIActions.insert_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim movno As String, inpno As String
        Dim RecCode As String

        ' If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Inhouse_Piece_Checking_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Inhouse_Piece_Checking_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Inhouse_Piece_Checking_Entry, New_Entry, Me) = False Then Exit Sub




        Try

            inpno = InputBox("Enter New Checking No.", "FOR NEW CHECKING INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Weaver_Piece_Checking_No from Weaver_Piece_Checking_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Checking No", "DOES NOT INSERT NEW CHECKING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_ChkNo.Text = Trim(UCase(inpno))

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT INSERT NEW CHECKING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Sub save_record() Implements Interface_MDIActions.save_record
        Dim cmd As New SqlClient.SqlCommand
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        Dim Led_ID As Integer = 0
        Dim Clo_ID As Integer = 0
        Dim CloTyp_ID As Integer = 0

        Dim Sno As Integer = 0
        Dim Nr As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim EntID As String = ""

        Dim vTot_Typ1Mtrs As Single
        Dim vTot_Typ2Mtrs As Single
        Dim vTot_Typ3Mtrs As Single
        Dim vTot_Typ5Mtrs As Single
        Dim vTot_Typ4Mtrs As Single
        Dim vTot_ChkMtrs As Single
        Dim vTot_Wgt As Single
        Dim vTot_GRSWgt As String = 0

        Dim vTot_100Fld_Typ1Mtrs As Single
        Dim vTot_100Fld_Typ2Mtrs As Single
        Dim vTot_100Fld_Typ3Mtrs As Single
        Dim vTot_100Fld_Typ4Mtrs As Single
        Dim vTot_100Fld_Typ5Mtrs As Single
        Dim vTot_100Fld_ChkMtr As Single

        Dim Lm_ID As Integer = 0
        Dim Wdth_Typ As String = ""
        Dim WagesCode As String = ""

        Dim ConsYarn As Single = 0
        Dim ConsPavu As Single = 0
        Dim BeamConPavu As Single = 0

        Dim StkOf_IdNo As Integer = 0
        Dim Led_type As String = 0
        Dim YrnPartcls As String = ""
        Dim Emp_id As Integer = 0

        Dim WftCnt_IDNo As Integer = 0
        Dim WftCnt_FldNmVal As String = ""

        Dim EdsCnt_IDNo As Integer = 0
        Dim Delv_ID As Integer = 0, Rec_ID As Integer = 0

        Dim vBrCode_Typ1 As String = "", vBrCode_Typ2 As String = "", vBrCode_Typ3 As String = "", vBrCode_Typ4 As String = "", vBrCode_Typ5 As String = ""
        Dim vYrCd As String = ""

        Dim vErrMsg As String = ""

        Dim vSetCD1 As String = ""
        Dim vBmNo1 As String = ""
        Dim vSetCD2 As String = ""
        Dim vBmNo2 As String = ""
        Dim vCLTH_Idno As Integer = 0
        Dim vOrdByNo As String = ""
        Dim vBEAM_ProdMeters As String = 0
        Dim SQL1 As String = ""
        Dim Old_SetCd1 As String, Old_Beam1 As String
        Dim Old_SetCd2 As String, Old_Beam2 As String
        Dim Old_BMKNOTCd As String
        Dim Old_CLTH_Idno As Integer

        Dim vWFTCNTIDno As Integer

        Dim vMULTIWFT_STS As Integer = 0
        Dim vOLD_PCSDOFCODE As String = ""


        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_ChkNo.Text)
        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)


        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Inhouse_Piece_Checking_Entry, New_Entry, Me, con, "Weaver_Piece_Checking_Head", "Weaver_Piece_Checking_Code", NewCode, "Weaver_Piece_Checking_Date", "(Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Weaver_Piece_Checking_No desc", dtp_Date.Value.Date) = False Then Exit Sub



        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'If Common_Procedures.UserRight_Check(Common_Procedures.UR.Inhouse_Piece_Checking_Entry, New_Entry) = False Then Exit Sub

        If Pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
            Exit Sub
        End If

        If Not (Convert.ToDateTime(msk_Date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_Date.Text) <= Common_Procedures.Company_ToDate) Then
            'If Not (dtp_Date.Value.Date >= Common_Procedures.Company_FromDate And dtp_Date.Value.Date <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If dtp_Date.Enabled And dtp_Date.Visible Then dtp_Date.Focus()
            Exit Sub
        End If

        Led_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, lbl_PartyName.Text)
        If Led_ID = 0 Then
            MessageBox.Show("Invalid Party Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_LotNo.Enabled And cbo_LotNo.Visible Then cbo_LotNo.Focus()
            Exit Sub
        End If

        Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, lbl_ClothName.Text)
        If Clo_ID = 0 Then
            MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_LotNo.Enabled And cbo_LotNo.Visible Then cbo_LotNo.Focus()
            Exit Sub
        End If

        Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)
        If Val(Lm_ID) = 0 Then
            MessageBox.Show("Invalid Loom No", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_LoomNo.Enabled And cbo_LoomNo.Visible Then cbo_LoomNo.Focus()
            Exit Sub
        End If

        If Val(txt_Folding.Text) = 0 Then
            MessageBox.Show("Invalid Folding", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_Folding.Enabled And txt_Folding.Visible Then txt_Folding.Focus()
            Exit Sub
        End If

        Emp_id = Common_Procedures.Employee_NameToIdNo(con, cbo_KnotterName.Text)


        With dgv_Details

            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.METERS).Value) <> 0 Then

                    If Trim(.Rows(i).Cells(dgvCOL_PCSDETAILS.CLOTHTYPE).Value) = "" Then
                        MessageBox.Show("Invalid Cloth Type", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        .Focus()
                        .CurrentCell = .Rows(0).Cells(dgvCOL_PCSDETAILS.PCSNO)
                        Exit Sub
                    End If

                    CloTyp_ID = Common_Procedures.ClothType_NameToIdNo(con, .Rows(i).Cells(dgvCOL_PCSDETAILS.CLOTHTYPE).Value)
                    If CloTyp_ID = 0 Then
                        MessageBox.Show("Invalid Cloth Type", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        .Focus()
                        .CurrentCell = .Rows(0).Cells(dgvCOL_PCSDETAILS.PCSNO)
                        Exit Sub
                    End If

                End If

            Next

        End With

        Total_Calculation()

        vTot_Typ1Mtrs = 0 : vTot_Typ2Mtrs = 0 : vTot_Typ3Mtrs = 0 : vTot_Typ4Mtrs = 0 : vTot_Typ5Mtrs = 0
        With dgv_Details
            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.METERS).Value) <> 0 Then

                    CloTyp_ID = Common_Procedures.ClothType_NameToIdNo(con, .Rows(i).Cells(dgvCOL_PCSDETAILS.CLOTHTYPE).Value)
                    If CloTyp_ID <> 0 Then

                        If CloTyp_ID = 1 Then
                            vTot_Typ1Mtrs = vTot_Typ1Mtrs + Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.METERS).Value)

                        ElseIf CloTyp_ID = 2 Then
                            vTot_Typ2Mtrs = vTot_Typ2Mtrs + Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.METERS).Value)

                        ElseIf CloTyp_ID = 3 Then
                            vTot_Typ3Mtrs = vTot_Typ3Mtrs + Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.METERS).Value)

                        ElseIf CloTyp_ID = 4 Then
                            vTot_Typ4Mtrs = vTot_Typ4Mtrs + Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.METERS).Value)

                        ElseIf CloTyp_ID = 5 Then
                            vTot_Typ5Mtrs = vTot_Typ5Mtrs + Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.METERS).Value)

                        End If

                    End If

                End If

            Next

        End With

        vTot_ChkMtrs = 0 : vTot_Wgt = 0
        vTot_GRSWgt = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTot_ChkMtrs = Val(dgv_Details_Total.Rows(0).Cells(dgvCOL_PCSDETAILS.METERS).Value())
            vTot_GRSWgt = Val(dgv_Details_Total.Rows(0).Cells(dgvCOL_PCSDETAILS.GROSSWEIGHT).Value())
            vTot_Wgt = Val(dgv_Details_Total.Rows(0).Cells(dgvCOL_PCSDETAILS.NETWEIGHT).Value())
        End If

        Dim vTot_wagMtr As Single
        Dim vTot_WagAmt As Single
        vTot_wagMtr = 0 : vTot_WagAmt = 0

        If dgv_production_wages_Total.RowCount > 0 Then
            vTot_wagMtr = Val(dgv_production_wages_Total.Rows(0).Cells(dgvCOL_PRODUCTIONDETAILS.METERS).Value())
            vTot_WagAmt = Val(dgv_production_wages_Total.Rows(0).Cells(dgvCOL_PRODUCTIONDETAILS.AMOUNT).Value())
        End If

        If Val(vTot_ChkMtrs) = 0 Then
            MessageBox.Show("Invalid Checking Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCOL_PCSDETAILS.PCSNO)
            Else
                txt_Folding.Focus()
            End If

            Exit Sub

        End If

        vTot_100Fld_Typ1Mtrs = Format(Val(vTot_Typ1Mtrs) * Val(txt_Folding.Text) / 100, "########0.00")
        vTot_100Fld_Typ2Mtrs = Format(Val(vTot_Typ2Mtrs) * Val(txt_Folding.Text) / 100, "########0.00")
        vTot_100Fld_Typ3Mtrs = Format(Val(vTot_Typ3Mtrs) * Val(txt_Folding.Text) / 100, "########0.00")
        vTot_100Fld_Typ4Mtrs = Format(Val(vTot_Typ4Mtrs) * Val(txt_Folding.Text) / 100, "########0.00")
        vTot_100Fld_Typ5Mtrs = Format(Val(vTot_Typ5Mtrs) * Val(txt_Folding.Text) / 100, "########0.00")
        vTot_100Fld_ChkMtr = Format(Val(vTot_ChkMtrs) * Val(txt_Folding.Text) / 100, "########0.00")

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_ChkNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Piece_Checking_Head", "Weaver_Piece_Checking_Code", "For_OrderBy", "(Receipt_Type = 'L')", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If


            WagesCode = ""
            Wdth_Typ = ""
            vSetCD1 = ""
            vBmNo1 = ""
            vSetCD2 = ""
            vBmNo2 = ""
            vCLTH_Idno = 0

            Da = New SqlClient.SqlDataAdapter("select * from Weaver_Cloth_Receipt_Head Where Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "'", con)
            Da.SelectCommand.Transaction = tr
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then
                If IsDBNull(Dt1.Rows(0).Item("Weaver_Wages_Code").ToString) = False Then
                    WagesCode = Dt1.Rows(0).Item("Weaver_Wages_Code").ToString
                End If
                'Lm_ID = Val(Dt1.Rows(0).Item("Loom_IdNo").ToString)
                Wdth_Typ = Dt1.Rows(0).Item("Width_Type").ToString
                vSetCD1 = Dt1.Rows(0).Item("set_code1").ToString
                vBmNo1 = Dt1.Rows(0).Item("Beam_No1").ToString
                vSetCD2 = Dt1.Rows(0).Item("set_code2").ToString
                vBmNo2 = Dt1.Rows(0).Item("Beam_No2").ToString
                vCLTH_Idno = Val(Dt1.Rows(0).Item("Cloth_IdNo").ToString)
            End If
            Dt1.Clear()

            Old_SetCd1 = ""
            Old_Beam1 = ""
            Old_SetCd2 = ""
            Old_Beam2 = ""
            Old_BMKNOTCd = ""
            Old_CLTH_Idno = 0

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@ChkDate", dtp_Date.Value.Date)
            cmd.Parameters.AddWithValue("@RecDate", CDate(lbl_RecDate.Text))

            If New_Entry = True Then

                cmd.CommandText = "Insert into Weaver_Piece_Checking_Head ( Receipt_Type, Weaver_Piece_Checking_Code,             Company_IdNo         ,      Weaver_Piece_Checking_No ,                               for_OrderBy                              , Weaver_Piece_Checking_Date,      Receipt_PkCondition     ,           Piece_Receipt_Code    ,  Loom_IdNo        ,        Piece_Receipt_No       , Piece_Receipt_Date,         Ledger_IdNo     ,         Cloth_IdNo ,             ReceiptMeters_Receipt ,         Crimp_Percentage   ,         Folding              ,     Total_Checking_Receipt_Meters ,          Total_Type1_Meters    ,      Total_Type2_Meters         ,   Total_Type3_Meters           ,     Total_Type4_Meters          ,     Total_Type5_Meters         ,     Total_Checking_Meters     ,     Total_Weight          ,     Total_Type1Meters_100Folding      ,     Total_Type2Meters_100Folding      ,     Total_Type3Meters_100Folding      ,      Total_Type4Meters_100Folding      ,     Total_Type5Meters_100Folding      ,      Total_Meters_100Folding         ,         Excess_Short_Meter        ,    Employee_IdNo  ,            Wages_Amount           ,            Wages_Pick      ,            Bar_Code      ,             Total_Wages_Meters   ,          Total_Wages_Amount       ,           Roll_Tare_Weight           ,      Total_Gross_Weight      ,               Fabric_LotNo           , ClothSales_OrderCode_forSelection ) " &
                                    "          Values                     (     'L'     ,    '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_ChkNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_ChkNo.Text))) & ",      @ChkDate             , '" & Trim(Pk_Condition2) & "', '" & Trim(lbl_RecCode.Text) & "', " & Val(Lm_ID) & ",'" & Trim(cbo_LotNo.Text) & "',      @RecDate     , " & Str(Val(Led_ID)) & ", " & Val(Clo_ID) & ", " & Str(Val(lbl_RecMtrs.Text)) & ", " & Val(txt_Crimp.Text) & ", " & Val(txt_Folding.Text) & ", " & Str(Val(lbl_RecMtrs.Text)) & ", " & Str(Val(vTot_Typ1Mtrs)) & ",  " & Str(Val(vTot_Typ2Mtrs)) & ", " & Str(Val(vTot_Typ3Mtrs)) & ",  " & Str(Val(vTot_Typ4Mtrs)) & ", " & Str(Val(vTot_Typ5Mtrs)) & ", " & Str(Val(vTot_ChkMtrs)) & ", " & Str(Val(vTot_Wgt)) & ", " & Str(Val(vTot_100Fld_Typ1Mtrs)) & ", " & Str(Val(vTot_100Fld_Typ2Mtrs)) & ", " & Str(Val(vTot_100Fld_Typ3Mtrs)) & ",  " & Str(Val(vTot_100Fld_Typ4Mtrs)) & ", " & Str(Val(vTot_100Fld_Typ5Mtrs)) & ",  " & Str(Val(vTot_100Fld_ChkMtr)) & ", " & Str(Val(lbl_ExcSht.Text)) & " ," & Val(Emp_id) & "," & Val(lbl_Wages_Amount.Text) & " ," & Val(txt_Wages.Text) & " , '" & Trim(txt_BarCode.Text) & "',   " & Str(Val(vTot_wagMtr)) & " , " & Str(Val(vTot_WagAmt)) & ", " & Str(Val(txt_TareWeight.Text)) & ", " & Str(Val(vTot_GRSWgt)) & ", '" & Trim(lbl_FabricLotNo.Text) & "' , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "'  ) "
                cmd.ExecuteNonQuery()


            Else

                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Weaver_Piece_Checking_Head", "Weaver_Piece_Checking_Code", Val(lbl_Company.Tag), NewCode, lbl_ChkNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Weaver_Piece_Checking_Code, Company_IdNo, for_OrderBy", tr)

                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Weaver_ClothReceipt_Piece_Details", "Weaver_Piece_Checking_Code", Val(lbl_Company.Tag), NewCode, lbl_ChkNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Piece_No,PieceNo_OrderBy,ReceiptMeters_Checking", "Sl_No", "Weaver_Piece_Checking_Code, For_OrderBy, Company_IdNo, Weaver_Piece_Checking_No, Weaver_Piece_Checking_Date, Ledger_Idno", tr)


                vOLD_PCSDOFCODE = ""
                vMULTIWFT_STS = 0
                Da = New SqlClient.SqlDataAdapter("Select a.Weaver_ClothReceipt_Code, a.set_code1, a.beam_no1, a.set_code2, a.beam_no2, a.Beam_Knotting_Code, tQ.Cloth_IdNo, tQ.Multiple_WeftCount_Status from Weaver_Cloth_Receipt_Head a INNER JOIN Cloth_Head tQ ON a.Cloth_IdNo = tQ.Cloth_IdNo Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'", con)
                Da.SelectCommand.Transaction = tr
                Dt1 = New DataTable
                Da.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    vOLD_PCSDOFCODE = Dt1.Rows(0).Item("Weaver_ClothReceipt_Code").ToString
                    Old_SetCd1 = Dt1.Rows(0).Item("set_code1").ToString
                    Old_Beam1 = Dt1.Rows(0).Item("beam_no1").ToString
                    Old_SetCd2 = Dt1.Rows(0).Item("set_code2").ToString
                    Old_Beam2 = Dt1.Rows(0).Item("beam_no2").ToString
                    Old_BMKNOTCd = Dt1.Rows(0).Item("Beam_Knotting_Code").ToString
                    Old_CLTH_Idno = Val(Dt1.Rows(0).Item("Cloth_IdNo").ToString)
                    vMULTIWFT_STS = Val(Dt1.Rows(0).Item("Multiple_WeftCount_Status").ToString)
                End If

                cmd.CommandText = "Update Weaver_Piece_Checking_Head set Weaver_Piece_Checking_Date = @ChkDate, Ledger_IdNo = " & Str(Val(Led_ID)) & ", Piece_Receipt_Code = '" & Trim(lbl_RecCode.Text) & "', Piece_Receipt_No = '" & Trim(cbo_LotNo.Text) & "', Piece_Receipt_Date = @chkDate, Loom_IdNo = " & Val(Lm_ID) & ", Cloth_IdNo = " & Val(Clo_ID) & ", ReceiptMeters_Receipt = " & Str(Val(lbl_RecMtrs.Text)) & ", Crimp_Percentage = " & Val(txt_Crimp.Text) & ", Folding = " & Val(txt_Folding.Text) & ", Total_Type1_Meters = " & Str(Val(vTot_Typ1Mtrs)) & ",  Total_Type2_Meters = " & Str(Val(vTot_Typ2Mtrs)) & ", Total_Type3_Meters = " & Str(Val(vTot_Typ3Mtrs)) & ", Total_Type4_Meters = " & Str(Val(vTot_Typ4Mtrs)) & ", Total_Type5_Meters = " & Str(Val(vTot_Typ5Mtrs)) & ", Total_Checking_Receipt_Meters = " & Str(Val(vTot_ChkMtrs)) & ", Total_Checking_Meters = " & Str(Val(vTot_ChkMtrs)) & ", Total_Weight = " & Str(Val(vTot_Wgt)) & ", Total_Type1Meters_100Folding = " & Str(Val(vTot_100Fld_Typ1Mtrs)) & ", Total_Type2Meters_100Folding = " & Str(Val(vTot_100Fld_Typ2Mtrs)) & ",Employee_IdNo = " & Str(Val(Emp_id)) & " , Wages_Amount = " & Val(lbl_Wages_Amount.Text) & ",Wages_Pick = " & Str(Val(txt_Wages.Text)) & " ,  Total_Type3Meters_100Folding = " & Str(Val(vTot_100Fld_Typ3Mtrs)) & ", Total_Type4Meters_100Folding = " & Str(Val(vTot_100Fld_Typ4Mtrs)) & ", Total_Type5Meters_100Folding = " & Str(Val(vTot_100Fld_Typ5Mtrs)) & ", Total_Meters_100Folding  =  " & Str(Val(vTot_100Fld_ChkMtr)) & ", Excess_Short_Meter = " & Str(Val(lbl_ExcSht.Text)) & " , Bar_Code = '" & Trim(txt_BarCode.Text) & "' , Total_Wages_Meters =" & Str(Val(vTot_wagMtr)) & " ,  Total_Wages_Amount = " & Str(Val(vTot_WagAmt)) & " , Roll_Tare_Weight = " & Str(Val(txt_TareWeight.Text)) & ", Total_Gross_Weight = " & Str(Val(vTot_GRSWgt)) & " , Fabric_LotNo = '" & Trim(lbl_FabricLotNo.Text) & "' , ClothSales_OrderCode_forSelection = '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                If vMULTIWFT_STS = 1 Then

                    cmd.CommandText = "Delete from Stock_Yarn_Processing_Details Where Reference_Code = '" & Trim(Pk_Condition2) & Trim(vOLD_PCSDOFCODE) & "' "
                    cmd.ExecuteNonQuery()

                    cmd.CommandText = "Insert into Stock_Yarn_Processing_Details (              Reference_Code                ,   Company_IdNo,   Reference_No          ,   for_OrderBy,   Reference_Date          ,                                               DeliveryTo_Idno         ,                                              ReceivedFrom_Idno         ,                  Entry_ID                               ,          Particulars                                                                                                                             ,   Party_Bill_No         ,         Sl_No,   Count_IdNo, Yarn_Type, Mill_IdNo, Bags, Cones,   Weight              ,   ClothSales_OrderCode_forSelection  ) " &
                                                "           Select  '" & Trim(Pk_Condition2) & "' + b.Weaver_ClothReceipt_Code, b.Company_IdNo, b.Weaver_ClothReceipt_No, b.for_OrderBy, b.Weaver_ClothReceipt_date, (CASE WHEN tP.Ledger_Type = 'JOBWORKER' THEN b.Ledger_IdNo ELSE 0 END), (CASE WHEN tP.Ledger_Type <> 'JOBWORKER' THEN b.Ledger_IdNo ELSE 0 END), '" & Trim(Pk_Condition2) & "' + b.Weaver_ClothReceipt_No, 'Doff : Roll.No. ' + b.Weaver_ClothReceipt_No +  ', Cloth : ' + tQ.Cloth_Name + ', Meters : ' + cast(ROUND(b.ReceiptMeters_Receipt,2) as varchar), b.Weaver_ClothReceipt_No,  1000+a.Sl_No, a.Count_IdNo,   'MILL' ,     0    ,   0 ,    0 , a.Consumed_Yarn_Weight, b.ClothSales_OrderCode_forSelection  from Weaver_ClothReceipt_Consumed_Yarn_Details a INNER JOIN Weaver_Cloth_Receipt_Head b ON a.Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition2) & "' + b.Weaver_ClothReceipt_Code INNER JOIN Ledger_Head tP ON tP.ledger_idno = b.ledger_idno INNER JOIN Cloth_Head tQ ON tQ.cloth_idno = b.cloth_idno where a.Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition2) & Trim(vOLD_PCSDOFCODE) & "' "
                    cmd.ExecuteNonQuery()

                Else

                    cmd.CommandText = "Update Stock_Yarn_Processing_Details set Particulars = 'Doff : Roll.No. ' + b.Weaver_ClothReceipt_No +  ', Cloth : ' + c.Cloth_Name + ', Meters : ' + cast(ROUND(b.ReceiptMeters_Receipt,2) as varchar), Weight = b.ConsumedYarn_Receipt from Stock_Yarn_Processing_Details a, Weaver_Cloth_Receipt_Head b, Cloth_Head c Where b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(Pk_Condition2) & "' + b.Weaver_ClothReceipt_Code and b.Weaver_Wages_Code = '' and b.cloth_idno = c.cloth_idno"
                    cmd.ExecuteNonQuery()

                End If


                cmd.CommandText = "Update Stock_Pavu_Processing_Details set Meters = b.ConsumedPavu_Receipt from Stock_Pavu_Processing_Details a, Weaver_Cloth_Receipt_Head b Where b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(Pk_Condition2) & "' + b.Weaver_ClothReceipt_Code and b.Weaver_Wages_Code = ''"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Stock_Cloth_Processing_Details set reference_date = (case when b.Weaver_Wages_Code <> '' then b.Weaver_Wages_Date else b.Weaver_ClothReceipt_Date end), Folding = b.Folding_Receipt, UnChecked_Meters = (case when b.Weaver_Wages_Code = '' then b.ReceiptMeters_Receipt else 0 end), Meters_Type1 = (case when b.Weaver_Wages_Code <> '' then b.Type1_Wages_Meters else 0 end), Meters_Type2 = (case when b.Weaver_Wages_Code <> '' then b.Type2_Wages_Meters else 0 end), Meters_Type3 = (case when b.Weaver_Wages_Code <> '' then b.Type3_Wages_Meters else 0 end), Meters_Type4 = (case when b.Weaver_Wages_Code <> '' then b.Type4_Wages_Meters else 0 end), Meters_Type5 = (case when b.Weaver_Wages_Code <> '' then b.Type5_Wages_Meters else 0 end) from Stock_Cloth_Processing_Details a, Weaver_Cloth_Receipt_Head b Where b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(Pk_Condition2) & "' + b.Weaver_ClothReceipt_Code"
                cmd.ExecuteNonQuery()

                ''----- Less Checking Meters (Consumption)
                'cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters - b.BeamConsumption_Checking from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and  b.set_code1 <> '' and b.Beam_No1 <> '' and a.set_code = b.Set_code1 and a.Beam_No = b.Beam_No1"
                'cmd.ExecuteNonQuery()
                'cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters - b.BeamConsumption_Checking from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and  b.set_code2 <> '' and b.Beam_No2 <> '' and a.set_code = b.Set_code2 and a.Beam_No = b.Beam_No2"
                'cmd.ExecuteNonQuery()
                ''----- Add Doffing Meters (Consumption)
                'cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters + b.BeamConsumption_Receipt from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and  b.set_code1 <> '' and b.Beam_No1 <> '' and a.set_code = b.Set_code1 and a.Beam_No = b.Beam_No1"
                'cmd.ExecuteNonQuery()
                'cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters + b.BeamConsumption_Receipt from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and  b.set_code2 <> '' and b.Beam_No2 <> '' and a.set_code = b.Set_code2 and a.Beam_No = b.Beam_No2"
                'cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Weaver_Piece_Checking_Code = '', Weaver_Piece_Checking_Increment = Weaver_Piece_Checking_Increment - 1, Weaver_Piece_Checking_Date = Null, Receipt_Meters = ReceiptMeters_Receipt, Folding = Folding_Receipt, Consumed_Yarn = ConsumedYarn_Receipt, Consumed_Pavu = ConsumedPavu_Receipt, BeamConsumption_Meters = BeamConsumption_Receipt, Folding_Checking = 0, ReceiptMeters_Checking = 0, ConsumedYarn_Checking = 0, ConsumedPavu_Checking = 0, BeamConsumption_Checking = 0, Type1_Checking_Meters = 0, Type2_Checking_Meters = 0, Type3_Checking_Meters = 0, Type4_Checking_Meters = 0, Type5_Checking_Meters = 0, Total_Checking_Meters = 0 Where Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Weaver_Piece_Checking_Head", "Weaver_Piece_Checking_Code", Val(lbl_Company.Tag), NewCode, lbl_ChkNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Weaver_Piece_Checking_Code, Company_IdNo, for_OrderBy", tr)


            EntID = Trim(Pk_Condition2) & Trim(cbo_LotNo.Text)
            Partcls = "Doff : Roll.No. " & Trim(cbo_LotNo.Text)
            PBlNo = Trim(cbo_LotNo.Text)

            ConsYarn = Val(lbl_ConsWeftYarn.Text)
            'ConsYarn = Val(Common_Procedures.get_Weft_ConsumedYarn(con, Clo_ID, Val(vTot_ChkMtrs), tr))
            'ConsYarn = Val(Common_Procedures.get_Weft_ConsumedYarn(con, Clo_ID, Val(lbl_RecMtrs.Text), tr))

            ConsPavu = 0
            BeamConPavu = 0
            ConsumedPavu_Calculation(Clo_ID, Lm_ID, Val(vTot_ChkMtrs), Trim(Wdth_Typ), ConsPavu, BeamConPavu, tr)
            'ConsPavu = Val(Common_Procedures.get_Pavu_Consumption(con, Clo_ID, Lm_ID, Val(vTot_ChkMtrs), Trim(Wdth_Typ), tr))
            'ConsPavu = Val(Common_Procedures.get_Pavu_Consumption(con, Clo_ID, Lm_ID, Val(lbl_RecMtrs.Text), Trim(Wdth_Typ), tr))

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1370" Then '---- AKIL IMPEX (ANNUR)
                ConsPavu = ConsPavu * Val(txt_Folding.Text) / 100
            End If

            WftCnt_FldNmVal = ""
            'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1019" Then '---- Subham Textiles
            '    WftCnt_IDNo = Common_Procedures.get_FieldValue(con, "Cloth_Head", "Cloth_WeftCount_IdNo", "(Cloth_IdNo = " & Str(Val(Clo_ID)) & ")", , tr)
            '    WftCnt_FldNmVal = ", Count_IdNo = " & Str(Val(WftCnt_IDNo))
            'End If

            Nr = 0
            cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "', Weaver_Piece_Checking_Increment = Weaver_Piece_Checking_Increment + 1, Weaver_Piece_Checking_Date = @ChkDate, Folding_Checking = " & Str(Val(txt_Folding.Text)) & ", Folding = " & Str(Val(txt_Folding.Text)) & ", ReceiptMeters_Checking = " & Str(Val(lbl_RecMtrs.Text)) & ", Receipt_Meters = " & Str(Val(lbl_RecMtrs.Text)) & ", ConsumedYarn_Checking = " & Str(Val(ConsYarn)) & ", Consumed_Yarn = " & Str(Val(ConsYarn)) & ", ConsumedPavu_Checking = " & Str(Val(ConsPavu)) & ", Consumed_Pavu = " & Str(Val(ConsPavu)) & ", BeamConsumption_Checking = " & Str(Val(BeamConPavu)) & ", BeamConsumption_Meters = " & Str(Val(BeamConPavu)) & ", Type1_Checking_Meters = " & Str(Val(vTot_Typ1Mtrs)) & ", Type2_Checking_Meters = " & Str(Val(vTot_Typ2Mtrs)) & ", Type3_Checking_Meters = " & Str(Val(vTot_Typ3Mtrs)) & ", Type4_Checking_Meters = " & Str(Val(vTot_Typ4Mtrs)) & ", Type5_Checking_Meters = " & Str(Val(vTot_Typ5Mtrs)) & ", Total_Checking_Meters = " & Str(Val(vTot_ChkMtrs)) & " " & WftCnt_FldNmVal & "  Where Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "' and Loom_IdNo = " & Str(Val(Lm_ID))
            Nr = cmd.ExecuteNonQuery()
            If Nr = 0 Then
                Throw New ApplicationException("invalid LotNo, Mismatch of LoomNo and LotNo")
                Exit Sub
            End If


            If Trim(vSetCD1) <> "" And Trim(vBmNo1) <> "" Then

                vBEAM_ProdMeters = 0
                vErrMsg = ""
                '----- Checking for negative beam meters
                If Common_Procedures.Check_SizedBeam_Doffing_Meters(con, vCLTH_Idno, vSetCD1, vBmNo1, vBEAM_ProdMeters, vErrMsg, tr) = True Then
                    Throw New ApplicationException(vErrMsg)
                    Exit Sub

                Else

                    SQL1 = "Update Stock_SizedPavu_Processing_Details set Production_Meters = " & Str(Val(vBEAM_ProdMeters)) & " Where set_code = '" & Trim(vSetCD1) & "' and beam_no = '" & Trim(vBmNo1) & "'"
                    cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
                    cmd.ExecuteNonQuery()

                End If

            End If



            If Trim(vSetCD2) <> "" And Trim(vBmNo2) <> "" Then

                vBEAM_ProdMeters = 0
                vErrMsg = ""
                '----- Checking for negative beam meters
                If Common_Procedures.Check_SizedBeam_Doffing_Meters(con, vCLTH_Idno, vSetCD2, vBmNo2, vBEAM_ProdMeters, vErrMsg, tr) = True Then
                    Throw New ApplicationException(vErrMsg)
                    Exit Sub

                Else
                    SQL1 = "Update Stock_SizedPavu_Processing_Details set Production_Meters = " & Str(Val(vBEAM_ProdMeters)) & " Where set_code = '" & Trim(vSetCD2) & "' and beam_no = '" & Trim(vBmNo2) & "'"
                    cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
                    cmd.ExecuteNonQuery()

                End If

            End If


            If Trim(Old_SetCd1) <> "" And Trim(Old_Beam1) <> "" Then

                If Not (Trim(UCase(Old_SetCd1)) = Trim(UCase(vSetCD1)) And Trim(UCase(Old_Beam1)) = Trim(UCase(vBmNo1))) Then

                    vBEAM_ProdMeters = 0
                    vErrMsg = ""
                    '----- Checking for negative beam meters
                    If Common_Procedures.Check_SizedBeam_Doffing_Meters(con, Old_CLTH_Idno, Old_SetCd1, Old_Beam1, vBEAM_ProdMeters, vErrMsg, tr) = True Then
                        Throw New ApplicationException(vErrMsg)
                        Exit Sub

                    Else

                        SQL1 = "Update Stock_SizedPavu_Processing_Details set Production_Meters = " & Str(Val(vBEAM_ProdMeters)) & " Where set_code = '" & Trim(Old_SetCd1) & "' and beam_no = '" & Trim(Old_Beam1) & "'"
                        cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
                        cmd.ExecuteNonQuery()

                    End If

                End If

            End If


            If Trim(Old_SetCd2) <> "" And Trim(Old_Beam2) <> "" Then

                If Not (Trim(UCase(Old_SetCd2)) = Trim(UCase(vSetCD2)) And Trim(UCase(Old_Beam2)) = Trim(UCase(vBmNo2))) Then

                    vBEAM_ProdMeters = 0
                    vErrMsg = ""
                    '----- Checking for negative beam meters
                    If Common_Procedures.Check_SizedBeam_Doffing_Meters(con, Old_CLTH_Idno, Old_SetCd2, Old_Beam2, vBEAM_ProdMeters, vErrMsg, tr) = True Then
                        Throw New ApplicationException(vErrMsg)
                        Exit Sub

                    Else
                        SQL1 = "Update Stock_SizedPavu_Processing_Details set Production_Meters = " & Str(Val(vBEAM_ProdMeters)) & " Where set_code = '" & Trim(Old_SetCd2) & "' and beam_no = '" & Trim(Old_Beam2) & "'"
                        cmd.CommandText = "EXEC [SP_ExecuteQuery] '" & Replace(Trim(SQL1), "'", "''") & "'"
                        cmd.ExecuteNonQuery()

                    End If

                End If

            End If


            ''----- Less Doffing Meters (Consumption)
            'cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters - b.BeamConsumption_Receipt from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "' and b.set_code1 <> '' and b.Beam_No1 <> '' and a.set_code = b.Set_code1 and a.Beam_No = b.Beam_No1"
            'cmd.ExecuteNonQuery()
            'cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters - b.BeamConsumption_Receipt from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "' and b.set_code2 <> '' and b.Beam_No2 <> '' and a.set_code = b.Set_code2 and a.Beam_No = b.Beam_No2"
            'cmd.ExecuteNonQuery()

            ''----- Less Doffing Meters (Consumption)
            'cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters + b.BeamConsumption_Checking from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "' and b.set_code1 <> '' and b.Beam_No1 <> '' and a.set_code = b.Set_code1 and a.Beam_No = b.Beam_No1"
            'cmd.ExecuteNonQuery()
            'cmd.CommandText = "Update Stock_SizedPavu_Processing_Details set Production_Meters = a.Production_Meters + b.BeamConsumption_Checking from Stock_SizedPavu_Processing_Details a, Weaver_Cloth_Receipt_Head b where b.Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "' and b.set_code2 <> '' and b.Beam_No2 <> '' and a.set_code = b.Set_code2 and a.Beam_No = b.Beam_No2"
            'cmd.ExecuteNonQuery()



            cmd.CommandText = "Delete from Weaver_ClothReceipt_Piece_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "' and Lot_Code = '" & Trim(lbl_RecCode.Text) & "' and PackingSlip_Code_Type1 = '' and PackingSlip_Code_Type2 = ''  and PackingSlip_Code_Type3 = ''  and PackingSlip_Code_Type4 = ''  and PackingSlip_Code_Type5 = '' and BuyerOffer_Code_Type1 = '' and BuyerOffer_Code_Type2 = '' and BuyerOffer_Code_Type3 = '' and BuyerOffer_Code_Type4 = '' and BuyerOffer_Code_Type5 = ''"
            Nr = cmd.ExecuteNonQuery()

            Led_type = Common_Procedures.get_FieldValue(con, "Ledger_Head", "Ledger_Type", "(Ledger_IdNo = " & Str(Val(Led_ID)) & ")", , tr)

            StkOf_IdNo = 0
            If Trim(UCase(Led_type)) = "JOBWORKER" Then
                StkOf_IdNo = Led_ID
            Else
                StkOf_IdNo = Val(Common_Procedures.CommonLedger.Godown_Ac)
            End If

            With dgv_Details
                Sno = 0

                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.METERS).Value) <> 0 Then

                        Sno = Sno + 1

                        CloTyp_ID = Common_Procedures.ClothType_NameToIdNo(con, .Rows(i).Cells(dgvCOL_PCSDETAILS.CLOTHTYPE).Value, tr)

                        vBrCode_Typ1 = ""
                        vBrCode_Typ2 = ""
                        vBrCode_Typ3 = ""
                        vBrCode_Typ4 = ""
                        vBrCode_Typ5 = ""

                        vYrCd = Microsoft.VisualBasic.Right(Trim(lbl_RecCode.Text), 5)

                        If CloTyp_ID = 1 Then
                            vBrCode_Typ1 = Microsoft.VisualBasic.Left(vYrCd, 2) & Trim(Val(lbl_Company.Tag)) & Trim(UCase(cbo_LotNo.Text)) & Trim(UCase((.Rows(i).Cells(dgvCOL_PCSDETAILS.PCSNO).Value))) & "1"
                        ElseIf CloTyp_ID = 2 Then
                            vBrCode_Typ2 = Microsoft.VisualBasic.Left(vYrCd, 2) & Trim(Val(lbl_Company.Tag)) & Trim(UCase(cbo_LotNo.Text)) & Trim(UCase((.Rows(i).Cells(dgvCOL_PCSDETAILS.PCSNO).Value))) & "2"
                        ElseIf CloTyp_ID = 3 Then
                            vBrCode_Typ3 = Microsoft.VisualBasic.Left(vYrCd, 2) & Trim(Val(lbl_Company.Tag)) & Trim(UCase(cbo_LotNo.Text)) & Trim(UCase((.Rows(i).Cells(dgvCOL_PCSDETAILS.PCSNO).Value))) & "3"
                        ElseIf CloTyp_ID = 4 Then
                            vBrCode_Typ4 = Microsoft.VisualBasic.Left(vYrCd, 2) & Trim(Val(lbl_Company.Tag)) & Trim(UCase(cbo_LotNo.Text)) & Trim(UCase((.Rows(i).Cells(dgvCOL_PCSDETAILS.PCSNO).Value))) & "4"
                        ElseIf CloTyp_ID = 5 Then
                            vBrCode_Typ5 = Microsoft.VisualBasic.Left(vYrCd, 2) & Trim(Val(lbl_Company.Tag)) & Trim(UCase(cbo_LotNo.Text)) & Trim(UCase((.Rows(i).Cells(dgvCOL_PCSDETAILS.PCSNO).Value))) & "5"
                        End If

                        Nr = 0
                        cmd.CommandText = "Update Weaver_ClothReceipt_Piece_Details set Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "',  Weaver_Piece_Checking_No = '" & Trim(lbl_ChkNo.Text) & "', Weaver_Piece_Checking_Date = @ChkDate, StockOff_IdNo = " & Str(Val(StkOf_IdNo)) & ", Ledger_IdNo = " & Str(Val(Led_ID)) & ", Loom_IdNo = " & Str(Val(Lm_ID)) & ", Folding_Receipt = " & Str(Val(txt_Folding.Text)) & ", Folding_Checking = " & Str(Val(txt_Folding.Text)) & ", Folding = " & Str(Val(txt_Folding.Text)) & ", Sl_No = " & Str(Val(Sno)) & ", PieceNo_OrderBy = " & Str(Val(Common_Procedures.OrderBy_CodeToValue(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.PCSNO).Value)))) & ", ReceiptMeters_Checking = " & Str(Val(lbl_RecMtrs.Text)) & ", Receipt_Meters = " & Str(Val(lbl_RecMtrs.Text)) & ", Type" & Trim(Val(CloTyp_ID)) & "_Meters = " & Str(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.METERS).Value)) & ", Total_Checking_Meters = " & Str(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.METERS).Value)) & ", Roll_Gross_Weight = " & Str(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.GROSSWEIGHT).Value)) & ", Roll_Tare_Weight = " & Str(Val(txt_TareWeight.Text)) & ", Weight = " & Str(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.NETWEIGHT).Value)) & ", Weight_Meter = " & Str(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.WEIGHTPERMETER).Value)) & " , Checked_Pcs_Barcode_Type1 = '" & Trim(vBrCode_Typ1) & "', Checked_Pcs_Barcode_Type2 = '" & Trim(vBrCode_Typ2) & "', Checked_Pcs_Barcode_Type3 = '" & Trim(vBrCode_Typ3) & "', Checked_Pcs_Barcode_Type4 = '" & Trim(vBrCode_Typ4) & "', Checked_Pcs_Barcode_Type5 = '" & Trim(vBrCode_Typ5) & "' , Fabric_LotNo = '" & Trim(lbl_FabricLotNo.Text) & "' Where Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition2) & Trim(lbl_RecCode.Text) & "' and Lot_Code = '" & Trim(lbl_RecCode.Text) & "' and Piece_No = '" & Trim(.Rows(i).Cells(dgvCOL_PCSDETAILS.PCSNO).Value) & "'"
                        Nr = cmd.ExecuteNonQuery()

                        If Nr = 0 Then

                            cmd.CommandText = "Insert into Weaver_ClothReceipt_Piece_Details ( Weaver_Piece_Checking_Code ,             Company_IdNo         ,     Weaver_Piece_Checking_No   ,  Weaver_Piece_Checking_Date,               Weaver_ClothReceipt_Code                ,    Weaver_ClothReceipt_No     ,                               for_orderby                              , Weaver_ClothReceipt_Date,           Lot_Code              ,               Lot_No           ,           StockOff_IdNo    ,         Ledger_IdNo     ,           Cloth_IdNo    ,            Loom_IdNo   ,            Folding_Receipt        ,             Folding_Checking      ,             Folding               ,           Sl_No      ,                    Piece_No                                  ,                                PieceNo_OrderBy                                                               ,            ReceiptMeters_Checking  ,                Receipt_Meters      ,   Type" & Trim(Val(CloTyp_ID)) & "_Meters                       ,                   Total_Checking_Meters                         ,                       Roll_Gross_Weight                               ,              Roll_Tare_Weight         ,                      Weight                                         ,                      Weight_Meter                                        ,   Checked_Pcs_Barcode_Type1 ,   Checked_Pcs_Barcode_Type2 ,   Checked_Pcs_Barcode_Type3 ,   Checked_Pcs_Barcode_Type4 ,   Checked_Pcs_Barcode_Type5  ,            Fabric_LotNo              ) " &
                                                "     Values                                 (    '" & Trim(NewCode) & "' , " & Str(Val(lbl_Company.Tag)) & ",  '" & Trim(lbl_ChkNo.Text) & "',            @ChkDate        , '" & Trim(Pk_Condition2) & Trim(lbl_RecCode.Text) & "', '" & Trim(cbo_LotNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(cbo_LotNo.Text))) & ",      @RecDate           , '" & Trim(lbl_RecCode.Text) & "', '" & Trim(cbo_LotNo.Text) & "', " & Str(Val(StkOf_IdNo)) & ", " & Str(Val(Led_ID)) & ", " & Str(Val(Clo_ID)) & ", " & Str(Val(Lm_ID)) & ", " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(txt_Folding.Text)) & ", " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(dgvCOL_PCSDETAILS.PCSNO).Value) & "',  " & Str(Val(Common_Procedures.OrderBy_CodeToValue(Trim(.Rows(i).Cells(dgvCOL_PCSDETAILS.PCSNO).Value)))) & ",  " & Str(Val(lbl_RecMtrs.Text)) & ",  " & Str(Val(lbl_RecMtrs.Text)) & ", " & Str(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.METERS).Value)) & ", " & Str(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.METERS).Value)) & ",  " & Str(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.GROSSWEIGHT).Value)) & ",  " & Str(Val(txt_TareWeight.Text)) & ", " & Str(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.NETWEIGHT).Value)) & " , " & Str(Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.WEIGHTPERMETER).Value)) & " , '" & Trim(vBrCode_Typ1) & "', '" & Trim(vBrCode_Typ2) & "', '" & Trim(vBrCode_Typ3) & "', '" & Trim(vBrCode_Typ4) & "', '" & Trim(vBrCode_Typ5) & "' , '" & Trim(lbl_FabricLotNo.Text) & "' ) "
                            Nr = cmd.ExecuteNonQuery()

                        End If

                    End If

                Next

                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Weaver_ClothReceipt_Piece_Details", "Weaver_Piece_Checking_Code", Val(lbl_Company.Tag), NewCode, lbl_ChkNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Piece_No,PieceNo_OrderBy,ReceiptMeters_Checking", "Sl_No", "Weaver_Piece_Checking_Code, For_OrderBy, Company_IdNo, Weaver_Piece_Checking_No, Weaver_Piece_Checking_Date, Ledger_Idno", tr)

            End With

            cmd.CommandText = "Delete from Weaver_Production_Wages_Details where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Piece_Checking_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Production_Wages_Details
                Sno = 0

                For i = 0 To .RowCount - 1
                    If Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.METERS).Value) <> 0 Then
                        Sno = Sno + 1

                        Emp_id = Common_Procedures.Employee_NameToIdNo(con, .Rows(i).Cells(dgvCOL_PRODUCTIONDETAILS.EMPLOYEENAME).Value, tr)

                        If Nr = 0 Then

                            cmd.CommandText = "Insert into Weaver_Production_Wages_Details ( Weaver_Piece_Checking_Code ,             Company_IdNo         ,     Weaver_Piece_Checking_No   ,  Weaver_Piece_Checking_Date,                                         for_orderby                    ,       Ledger_IdNo       ,           Cloth_IdNo    ,         Employee_IdNo   ,          Sl_No        ,                      Wages_Meters                                      ,                        Wages_Rate                                       ,                       Wages_Amount                                       ) " &
                                             "     Values                                 (    '" & Trim(NewCode) & "'  , " & Str(Val(lbl_Company.Tag)) & ",  '" & Trim(lbl_ChkNo.Text) & "',            @ChkDate        , " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_ChkNo.Text))) & ", " & Str(Val(Led_ID)) & ", " & Str(Val(Clo_ID)) & ", " & Str(Val(Emp_id)) & ",  " & Str(Val(Sno)) & ", " & Str(Val(.Rows(i).Cells(dgvCOL_PRODUCTIONDETAILS.METERS).Value)) & ",   " & Str(Val(.Rows(i).Cells(dgvCOL_PRODUCTIONDETAILS.RATE).Value)) & " ,  " & Str(Val(.Rows(i).Cells(dgvCOL_PRODUCTIONDETAILS.AMOUNT).Value)) & " ) "
                            cmd.ExecuteNonQuery()

                        End If

                    End If

                Next i

            End With

            YrnPartcls = Partcls & ", Cloth : " & Trim(lbl_ClothName.Text) & ", Meters : " & Str(Val(vTot_ChkMtrs))

            Delv_ID = 0 : Rec_ID = 0
            If Trim(UCase(Led_type)) = "JOBWORKER" Then
                Delv_ID = Led_ID
                Rec_ID = 0
            Else
                Delv_ID = 0
                Rec_ID = Led_ID
            End If

            If Trim(WagesCode) = "" Then

                WftCnt_FldNmVal = ""
                'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1019" Then '---- Subham Textiles
                '    WftCnt_IDNo = Common_Procedures.get_FieldValue(con, "Cloth_Head", "Cloth_WeftCount_IdNo", "(Cloth_IdNo = " & Str(Val(Clo_ID)) & ")", , tr)
                '    WftCnt_FldNmVal = ", Count_IdNo = " & Str(Val(WftCnt_IDNo))
                'End If


                If Trim(UCase(Led_type)) <> "JOBWORKER" Or (Common_Procedures.settings.JobWorker_Pavu_Yarn_Stock_Posting_IN_Production = 1 And Trim(UCase(Led_type)) = "JOBWORKER") Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1192" Then


                    If btn_Show_WeftConsumption_Details.Visible = False Then

                        Nr = 0
                        cmd.CommandText = "Update Stock_Yarn_Processing_Details set Particulars = '" & Trim(YrnPartcls) & "', Weight = " & Str(Val(ConsYarn)) & " " & WftCnt_FldNmVal & " Where Reference_Code = '" & Trim(Pk_Condition2) & Trim(lbl_RecCode.Text) & "'"
                        Nr = cmd.ExecuteNonQuery()
                        If Nr = 0 Then

                            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1192" Then


                                WftCnt_IDNo = Common_Procedures.get_FieldValue(con, "Weaver_Cloth_Receipt_Head", "Count_IdNo", "(Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "' and Loom_IdNo = " & Str(Val(Lm_ID)) & ")", , tr)

                                cmd.CommandText = "Insert into Stock_Yarn_Processing_Details (           Reference_Code                             ,                 Company_IdNo     ,            Reference_No        ,                               for_OrderBy                               , Reference_Date,        DeliveryTo_Idno   ,    ReceivedFrom_Idno    ,        Entry_ID      ,            Particulars    ,       Party_Bill_No  , Sl_No,          Count_IdNo          , Yarn_Type, Mill_IdNo, Bags, Cones,              Weight      , ClothSales_OrderCode_forSelection   ) " &
                                                    "          Values                        ('" & Trim(Pk_Condition2) & Trim(lbl_RecCode.Text) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(cbo_LotNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(cbo_LotNo.Text))) & ",     @ChkDate  , " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(YrnPartcls) & "', '" & Trim(PBlNo) & "',    1 , " & Str(Val(WftCnt_IDNo)) & ",    'MILL',     0    ,    0 ,   0 , " & Str(Val(ConsYarn)) & " ,  '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "') "
                                cmd.ExecuteNonQuery()

                            End If

                        End If

                    End If

                    '----Multi WeftCount Yarn consumption posting

                    cmd.CommandText = "delete from Weaver_ClothReceipt_Consumed_Yarn_Details where Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                    cmd.ExecuteNonQuery()

                    If btn_Show_WeftConsumption_Details.Visible = True Then

                        cmd.CommandText = "delete from Stock_Yarn_Processing_Details Where Reference_Code = '" & Trim(Pk_Condition2) & Trim(lbl_RecCode.Text) & "'"
                        cmd.ExecuteNonQuery()

                        With dgv_Weft_Consumption_Details

                            Sno = 0
                            For i = 0 To .RowCount - 1

                                If Trim(.Rows(i).Cells(0).Value) <> "" Then

                                    vWFTCNTIDno = Common_Procedures.Count_NameToIdNo(con, Trim(.Rows(i).Cells(0).Value), tr)

                                    If Val(vWFTCNTIDno) <> 0 Then

                                        Sno = Sno + 1

                                        cmd.CommandText = "Insert into Weaver_ClothReceipt_Consumed_Yarn_Details (               Weaver_ClothReceipt_Code   ,           Company_IdNo           ,           Sl_No      ,             Count_IdNo       ,                    Gram_Perc_Type       ,                    Consumption_Gram_Perc  ,                Consumed_Yarn_Weight        )  " &
                                                            " Values                                         (  '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", " & Str(Val(Sno)) & ", " & Str(Val(vWFTCNTIDno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "' , " & Str(Val(.Rows(i).Cells(2).Value)) & " ,  " & Str(Val(.Rows(i).Cells(3).Value)) & " ) "
                                        cmd.ExecuteNonQuery()


                                        If Val(.Rows(i).Cells(3).Value) <> 0 Then

                                            cmd.CommandText = "Insert into Stock_Yarn_Processing_Details (              Reference_Code                ,                Company_IdNo      ,                Reference_No         ,                               for_OrderBy                              , Reference_Date,      DeliveryTo_Idno     ,       ReceivedFrom_Idno ,         Entry_ID     ,         Particulars       ,      Party_Bill_No   ,              Sl_No           ,           Count_IdNo         , Yarn_Type, Mill_IdNo, Bags, Cones,                 Weight                 ,                   ClothSales_OrderCode_forSelection            ) " &
                                                                "           Values                   ('" & Trim(Pk_Condition2) & Trim(lbl_RecCode.Text) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(cbo_LotNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(cbo_LotNo.Text))) & ",   @RecDate    , " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", '" & Trim(EntID) & "', '" & Trim(YrnPartcls) & "', '" & Trim(PBlNo) & "',  " & Str(Val(1000 + Sno)) & " , " & Str(Val(vWFTCNTIDno)) & ",   'MILL' ,     0    ,   0 ,    0 , " & Str(Val(.Rows(i).Cells(3).Value)) & " , '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "' ) "
                                            cmd.ExecuteNonQuery()

                                        End If

                                    End If

                                End If

                            Next

                        End With

                    End If



                    Nr = 0
                    cmd.CommandText = "Update Stock_Pavu_Processing_Details set Meters = " & Str(Val(ConsPavu)) & " Where Reference_Code = '" & Trim(Pk_Condition2) & Trim(lbl_RecCode.Text) & "'"
                    Nr = cmd.ExecuteNonQuery()
                    If Nr = 0 Then

                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1192" Then

                            EdsCnt_IDNo = Common_Procedures.get_FieldValue(con, "Weaver_Cloth_Receipt_Head", "EndsCount_Idno", "(Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "' and Loom_IdNo = " & Str(Val(Lm_ID)) & ")", , tr)

                            cmd.CommandText = "Insert into Stock_Pavu_Processing_Details (                Reference_Code              ,                 Company_IdNo     ,              Reference_No      ,                               for_OrderBy                               , Reference_Date,        DeliveryTo_Idno    ,    ReceivedFrom_Idno   ,      Cloth_Idno         ,         Entry_ID     ,     Party_Bill_No    ,       Particulars      ,  Sl_No,        EndsCount_IdNo        , Sized_Beam,          Meters           ,  ClothSales_OrderCode_forSelection              ) " &
                                                "          Values                        ('" & Trim(Pk_Condition2) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(cbo_LotNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(cbo_LotNo.Text))) & ",   @ChkDate    , " & Str(Val(Delv_ID)) & ", " & Str(Val(Rec_ID)) & ", " & Str(Val(Clo_ID)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',    1  , " & Str(Val(EdsCnt_IDNo)) & ",      0    , " & Str(Val(ConsPavu)) & " ,  '" & Trim(cbo_ClothSales_OrderCode_forSelection.Text) & "'            )"
                            cmd.ExecuteNonQuery()

                        End If

                    End If

                End If

            End If

            If Val(vTot_Typ1Mtrs) <> 0 Or Val(vTot_Typ2Mtrs) <> 0 Or Val(vTot_Typ3Mtrs) <> 0 Or Val(vTot_Typ4Mtrs) <> 0 Or Val(vTot_Typ5Mtrs) <> 0 Then
                cmd.CommandText = "Update Stock_Cloth_Processing_Details set reference_date = @ChkDate,  StockOff_IdNo = " & Str(Val(StkOf_IdNo)) & ", Folding = " & Str(Val(txt_Folding.Text)) & ", UnChecked_Meters = 0, Meters_Type1 = " & Str(Val(vTot_Typ1Mtrs)) & ", Meters_Type2 = " & Str(Val(vTot_Typ2Mtrs)) & ", Meters_Type3 = " & Str(Val(vTot_Typ3Mtrs)) & ", Meters_Type4 = " & Str(Val(vTot_Typ4Mtrs)) & ", Meters_Type5 = " & Str(Val(vTot_Typ5Mtrs)) & " Where Reference_Code = '" & Trim(Pk_Condition2) & Trim(lbl_RecCode.Text) & "'"
                cmd.ExecuteNonQuery()
            End If

            '---***********************************************************COMMEMTED BY THANGES FOR -  KVP WEAVES - TODAY-ONLY(06-10-2023)
            'If New_Entry = True Then

            '    If Common_Procedures.Check_SizedBeam_Doffing_Meters(con, Clo_ID, vSetCD1, vBmNo1, vErrMsg, tr) = True Then
            '        Throw New ApplicationException(vErrMsg)
            '        Exit Sub
            '    End If

            '    If Common_Procedures.Check_SizedBeam_Doffing_Meters(con, Clo_ID, vSetCD2, vBmNo2, vErrMsg, tr) = True Then
            '        Throw New ApplicationException(vErrMsg)
            '        Exit Sub
            '    End If

            'End If

            ''----- Saving Cross Checking
            'vErrMsg = ""
            'Dim vFAB_LOTCODE As String
            'vFAB_LOTCODE = "~" & Trim(lbl_RecCode.Text) & "~"
            'If Common_Procedures.Cross_Checking_PieceChecking_PackingSlip_Meters(con, vFAB_LOTCODE, vErrMsg, tr) = False Then
            '    Throw New ApplicationException(vErrMsg)
            '    Exit Sub
            'End If



            tr.Commit()

            Dt1.Dispose()
            Da.Dispose()

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
            MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()

        End Try

    End Sub


    Private Sub txt_Folding_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Folding.KeyDown
        If e.KeyValue = 38 Then SendKeys.Send("+{TAB}")
        If (e.KeyValue = 40) Then

            If txt_TareWeight.Visible = True Then
                txt_TareWeight.Focus()

            ElseIf cbo_ClothSales_OrderCode_forSelection.Visible = True Then

                cbo_ClothSales_OrderCode_forSelection.Focus()
            Else

                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    If dgv_Details.Columns(dgvCOL_PCSDETAILS.PCSNO).ReadOnly = True Then
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCOL_PCSDETAILS.PCSNO)
                    Else
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCOL_PCSDETAILS.PCSNO)
                    End If

                Else
                    btn_save.Focus()

                End If

            End If

        End If
    End Sub

    Private Sub txt_Folding_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Folding.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then

            If txt_TareWeight.Visible = True Then
                txt_TareWeight.Focus()

            ElseIf cbo_ClothSales_OrderCode_forSelection.Visible = True Then

                cbo_ClothSales_OrderCode_forSelection.Focus()
            Else

                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    If dgv_Details.Columns(dgvCOL_PCSDETAILS.PCSNO).ReadOnly = True Then
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCOL_PCSDETAILS.PCSNO)
                    Else
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCOL_PCSDETAILS.PCSNO)
                    End If
                Else
                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        dtp_Date.Focus()
                    End If

                End If

            End If
        End If
    End Sub

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEndEdit
        dgv_Details_CellLeave(sender, e)
    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim rect As Rectangle
        Dim n As Integer = 0

        If FrmLdSTS = True Then Exit Sub
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

        With dgv_Details

            If Trim(.CurrentRow.Cells(dgvCOL_PCSDETAILS.PCSNO).Value) = "" Then

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1544" Or Common_Procedures.settings.CustomerCode = "1608" Then '---- SRI SRINIVASA TEXTILES (PALLADAM)

                    If e.RowIndex = 0 Then

                        Dim LtNo As String

                        LtNo = cbo_LotNo.Text
                        If (Trim(LtNo) Like "*/??-??") Then LtNo = Microsoft.VisualBasic.Left(LtNo, Len(LtNo) - 6)

                        .Rows(e.RowIndex).Cells(dgvCOL_PCSDETAILS.PCSNO).Value = LtNo


                    Else

                        Dim vPcNo As String
                        Dim vRolNo As String
                        Dim vPCSUBNO As String

                        vPcNo = Trim(.Rows(e.RowIndex - 1).Cells(dgvCOL_PCSDETAILS.PCSNO).Value)
                        vRolNo = Val(vPcNo)

                        vPCSUBNO = ""
                        For k = Len(vPcNo) To 1 Step -1
                            If IsNumeric(Mid(vPcNo, k, 1)) = False Then
                                vPCSUBNO = Chr(Asc(Mid(vPcNo, k, 1)) + 1)
                                Exit For
                            End If
                        Next k
                        If Trim(vPCSUBNO) = "" Then vPCSUBNO = "A"

                        .Rows(e.RowIndex).Cells(dgvCOL_PCSDETAILS.PCSNO).Value = Trim(vRolNo) & Trim(vPCSUBNO)

                        '.Rows(e.RowIndex).Cells(dgvCOL_PCSDETAILS.PCSNO).Value = Val(.Rows(e.RowIndex - 1).Cells(dgvCOL_PCSDETAILS.PCSNO).Value) + 1

                    End If

                ElseIf Common_Procedures.settings.ClothReceipt_PieceNo_Concept = "1,2,3" Or Trim(UCase(Common_Procedures.settings.ClothReceipt_PieceNo_Concept)) = Trim(UCase("CONTINUOUS NO")) Then
                    If e.RowIndex = 0 Then
                        .Rows(e.RowIndex).Cells(dgvCOL_PCSDETAILS.PCSNO).Value = 1
                    Else
                        .Rows(e.RowIndex).Cells(dgvCOL_PCSDETAILS.PCSNO).Value = Val(.Rows(e.RowIndex - 1).Cells(dgvCOL_PCSDETAILS.PCSNO).Value) + 1
                    End If

                Else
                    If e.RowIndex = 0 Then
                        .Rows(e.RowIndex).Cells(dgvCOL_PCSDETAILS.PCSNO).Value = "A"
                    Else
                        .Rows(e.RowIndex).Cells(dgvCOL_PCSDETAILS.PCSNO).Value = Chr(Asc(.Rows(e.RowIndex - 1).Cells(dgvCOL_PCSDETAILS.PCSNO).Value) + 1)
                    End If

                End If

            End If

            If Trim(.CurrentRow.Cells(dgvCOL_PCSDETAILS.CLOTHTYPE).Value) = "" Then
                .CurrentRow.Cells(dgvCOL_PCSDETAILS.CLOTHTYPE).Value = Common_Procedures.ClothType_IdNoToName(con, 1)
            End If

            If e.ColumnIndex = dgvCOL_PCSDETAILS.CLOTHTYPE And Trim(.CurrentRow.Cells(dgvCOL_PCSDETAILS.BALENO).Value) = "" Then

                If cbo_Grid_ClothType.Visible = False Or Val(cbo_Grid_ClothType.Tag) <> e.RowIndex Then

                    cbo_Grid_ClothType.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select ClothType_Name from ClothType_Head where ClothType_Idno Between 0 and 5 order by ClothType_Name", con)
                    Dt2 = New DataTable
                    Da.Fill(Dt2)
                    cbo_Grid_ClothType.DataSource = Dt2
                    cbo_Grid_ClothType.DisplayMember = "ClothType_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_Grid_ClothType.Left = .Left + rect.Left
                    cbo_Grid_ClothType.Top = .Top + rect.Top
                    cbo_Grid_ClothType.Width = rect.Width
                    cbo_Grid_ClothType.Height = rect.Height

                    cbo_Grid_ClothType.Text = .CurrentCell.Value

                    cbo_Grid_ClothType.Tag = Val(e.RowIndex)
                    cbo_Grid_ClothType.Visible = True

                    cbo_Grid_ClothType.BringToFront()
                    cbo_Grid_ClothType.Focus()

                End If

            Else

                cbo_Grid_ClothType.Visible = False

            End If

        End With
    End Sub

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellLeave
        With dgv_Details

            If FrmLdSTS = True Then Exit Sub
            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

            If .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.METERS Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If

            If .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.GROSSWEIGHT Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.NETWEIGHT Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.WEIGHTPERMETER Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If

        End With

    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged
        Dim TtMtrs_100Fld As String = 0
        Dim FldPerc As String = 0
        Dim vNETWGT As String = 0
        Dim vGRSWGT As String = 0

        Try

            If FrmLdSTS = True Then Exit Sub
            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
            With dgv_Details

                If .Visible Then

                    If .Rows.Count > 0 Then

                        If e.ColumnIndex = dgvCOL_PCSDETAILS.METERS Or e.ColumnIndex = dgvCOL_PCSDETAILS.GROSSWEIGHT Or e.ColumnIndex = dgvCOL_PCSDETAILS.NETWEIGHT Then

                            TtMtrs_100Fld = Format(Val(.CurrentRow.Cells(dgvCOL_PCSDETAILS.METERS).Value), "#########0.00")

                            If Common_Procedures.settings.CustomerCode = "1370" Then   'akill

                                FldPerc = Val(txt_Folding.Text)
                                If Val(FldPerc) = 0 Then FldPerc = 100

                                TtMtrs_100Fld = Format(Val(.CurrentRow.Cells(dgvCOL_PCSDETAILS.METERS).Value) * Val(FldPerc) / 100, "#########0.00")

                            End If

                            If dgv_Details.Columns(dgvCOL_PCSDETAILS.GROSSWEIGHT).Visible = True And dgv_Details.CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.GROSSWEIGHT Then

                                vNETWGT = 0

                                If Val(.CurrentRow.Cells(dgvCOL_PCSDETAILS.GROSSWEIGHT).Value) > 0 Then
                                    vNETWGT = Format(Val(.CurrentRow.Cells(dgvCOL_PCSDETAILS.GROSSWEIGHT).Value) - Val(txt_TareWeight.Text), "#########0.000")
                                End If

                                .CurrentRow.Cells(dgvCOL_PCSDETAILS.NETWEIGHT).Value = Format(Val(vNETWGT), "#########0.000")

                            End If

                            If dgv_Details.Columns(dgvCOL_PCSDETAILS.NETWEIGHT).Visible = True And dgv_Details.CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.NETWEIGHT Then

                                vGRSWGT = 0

                                If Val(.CurrentRow.Cells(dgvCOL_PCSDETAILS.NETWEIGHT).Value) > 0 Then
                                    vGRSWGT = Format(Val(.CurrentRow.Cells(dgvCOL_PCSDETAILS.NETWEIGHT).Value) + Val(txt_TareWeight.Text), "#########0.000")
                                End If

                                .CurrentRow.Cells(dgvCOL_PCSDETAILS.GROSSWEIGHT).Value = Format(Val(vGRSWGT), "#########0.000")

                            End If

                            If Val(.CurrentRow.Cells(dgvCOL_PCSDETAILS.METERS).Value) <> 0 Then
                                .CurrentRow.Cells(dgvCOL_PCSDETAILS.WEIGHTPERMETER).Value = Format(Val(.CurrentRow.Cells(dgvCOL_PCSDETAILS.NETWEIGHT).Value) / Val(TtMtrs_100Fld), "#########0.000")
                            Else
                                .CurrentRow.Cells(dgvCOL_PCSDETAILS.WEIGHTPERMETER).Value = 0
                            End If

                            Check_Weight_and_Change_Colour(e.RowIndex)

                            Total_Calculation()

                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            '------

        End Try
    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
        dgtxt_Details.CharacterCasing = CharacterCasing.Upper
    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim i As Integer
        Dim n As Integer
        Dim nrw As Integer
        Dim PNO As String
        Dim S As String

        With dgv_Details

            '-- Insert a row  with next no  (1, 2, 3  or   A, B, C  )
            If e.Control = True And (UCase(Chr(e.KeyCode)) = "I" Or e.KeyCode = Keys.Insert) Then

                n = .CurrentRow.Index

                PNO = Trim(UCase(.Rows(n).Cells(dgvCOL_PCSDETAILS.PCSNO).Value))

                If Common_Procedures.settings.ClothReceipt_PieceNo_Concept = "1,2,3" Or Trim(UCase(Common_Procedures.settings.ClothReceipt_PieceNo_Concept)) = "CONTINUOUS NO" Then

                    S = Val(PNO) + 1

                Else

                    S = Chr(Asc(PNO) + 1)

                End If

                If n <> .Rows.Count - 1 Then
                    If Trim(UCase(S)) = Trim(UCase(.Rows(n + 1).Cells(dgvCOL_PCSDETAILS.PCSNO).Value)) Then
                        MessageBox.Show("Already Piece Inserted", "DOES NOT INSERT NEW PIECE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If

                nrw = n + 1

                dgv_Details.Rows.Insert(nrw, 1)

                dgv_Details.Rows(nrw).Cells(dgvCOL_PCSDETAILS.PCSNO).Value = S

            End If

            '-- add a new row  (1, 1A, 1B  or   A, A1, A2, A3  )
            If e.Control = True And UCase(Chr(e.KeyCode)) = "A" Then


                n = .CurrentRow.Index

                PNO = Trim(UCase(.Rows(n).Cells(dgvCOL_PCSDETAILS.PCSNO).Value))

                If Common_Procedures.settings.ClothReceipt_PieceNo_Concept = "1,2,3" Or Trim(UCase(Common_Procedures.settings.ClothReceipt_PieceNo_Concept)) = "CONTINUOUS NO" Then

                    S = Replace(Trim(PNO), Val(PNO), "")
                    PNO = Val(PNO)

                    If Trim(UCase(S)) <> "Z" Then
                        S = Trim(UCase(S))
                        If Trim(S) = "" Then S = "A" Else S = Trim(Chr(Asc(S) + 1))
                    End If

                Else


                    If Len(PNO) = 1 Then
                        S = "1"

                    Else

                        S = Microsoft.VisualBasic.Right(PNO, Len(PNO) - 1)
                        S = Val(S) + 1

                        PNO = Microsoft.VisualBasic.Left(PNO, 1)

                    End If

                End If

                If n <> .Rows.Count - 1 Then
                    If Trim(UCase(PNO)) & Trim(UCase(S)) = Trim(UCase(.Rows(n + 1).Cells(dgvCOL_PCSDETAILS.PCSNO).Value)) Then
                        MessageBox.Show("Already Piece Added", "DOES NOT ADD NEW PIECE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End If

                nrw = n + 1

                dgv_Details.Rows.Insert(nrw, 1)

                dgv_Details.Rows(nrw).Cells(dgvCOL_PCSDETAILS.PCSNO).Value = Trim(UCase(PNO)) & S


            End If


            '-- Row Delete
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then



                If Trim(.CurrentRow.Cells(dgvCOL_PCSDETAILS.BALENO).Value) = "" Then
                    n = .CurrentRow.Index

                    If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                        For i = 0 To .ColumnCount - 1
                            .Rows(n).Cells(i).Value = ""
                        Next

                    Else
                        .Rows.RemoveAt(n)

                    End If

                    Total_Calculation()

                End If

            End If

        End With

    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1544" And Common_Procedures.settings.CustomerCode <> "1608" Then '---- SRI SRINIVASA TEXTILES (PALLADAM)

                If Common_Procedures.settings.ClothReceipt_PieceNo_Concept = "1,2,3" Then
                    If e.RowIndex = 0 Then
                        .Rows(e.RowIndex).Cells(dgvCOL_PCSDETAILS.PCSNO).Value = 1
                    Else
                        .Rows(e.RowIndex).Cells(dgvCOL_PCSDETAILS.PCSNO).Value = Val(.Rows(e.RowIndex - 1).Cells(dgvCOL_PCSDETAILS.PCSNO).Value) + 1
                    End If

                ElseIf Trim(UCase(Common_Procedures.settings.ClothReceipt_PieceNo_Concept)) = "A,B,C" Then
                    If e.RowIndex = 0 Then
                        .Rows(e.RowIndex).Cells(dgvCOL_PCSDETAILS.PCSNO).Value = "A"
                    Else
                        .Rows(e.RowIndex).Cells(dgvCOL_PCSDETAILS.PCSNO).Value = Chr(Asc(.Rows(e.RowIndex - 1).Cells(dgvCOL_PCSDETAILS.PCSNO).Value) + 1)
                    End If

                End If

            End If

        End With

    End Sub

    Private Sub Total_Calculation()
        Dim Sno As Integer
        Dim TotMtrs As String = 0, TotWgt As String = 0, TtMtrs_100Fld As String = 0
        Dim FldPerc As String = 0
        Dim TotGRSWgt As String

        Sno = -1
        TotMtrs = 0
        TotWgt = 0
        TotGRSWgt = 0
        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1

                If Trim(.Rows(i).Cells(dgvCOL_PCSDETAILS.CLOTHTYPE).Value) <> "" Or Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.METERS).Value) <> 0 Then
                    TotMtrs = Val(TotMtrs) + Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.METERS).Value)
                    TotGRSWgt = Val(TotGRSWgt) + Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.GROSSWEIGHT).Value)
                    TotWgt = Val(TotWgt) + Val(.Rows(i).Cells(dgvCOL_PCSDETAILS.NETWEIGHT).Value)
                End If
            Next
        End With

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(dgvCOL_PCSDETAILS.METERS).Value = Format(Val(TotMtrs), "#########0.00")
            .Rows(0).Cells(dgvCOL_PCSDETAILS.GROSSWEIGHT).Value = Format(Val(TotGRSWgt), "#########0.000")
            .Rows(0).Cells(dgvCOL_PCSDETAILS.NETWEIGHT).Value = Format(Val(TotWgt), "#########0.000")
        End With

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1370" Then '---- AKIL IMPEX (ANNUR)
            FldPerc = Val(txt_Folding.Text)
            If Val(FldPerc) = 0 Then FldPerc = 100

            TtMtrs_100Fld = Format(Val(TotMtrs) * Val(FldPerc) / 100, "#########0.00")

            lbl_ExcSht.Text = Format(Val(TotMtrs) - Val(TtMtrs_100Fld), "#########0.00")

        Else

            lbl_ExcSht.Text = Format(Val(TotMtrs) - Val(lbl_RecMtrs.Text), "#########0.00")

        End If

        ConsumedPavu_Calculation()
        ConsumedYarn_Calculation()

        wages_calculation()


    End Sub

    Private Sub cbo_Grid_ClothType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_ClothType.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothType_Head", "ClothType_Name", "(ClothType_IdNo BetWeen 1 and 5)", "(ClothType_IdNo = 0)")
    End Sub

    Private Sub cbo_Grid_ClothType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_ClothType.KeyDown

        With dgv_Details

            If .Rows.Count > 0 Then

                Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, Nothing, Nothing, "ClothType_Head", "ClothType_Name", "(ClothType_IdNo BetWeen 1 and 5)", "(ClothType_IdNo = 0)")

                If (e.KeyValue = 38 And cbo_Grid_ClothType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                    If dgv_Details.Columns(dgvCOL_PCSDETAILS.PCSNO).ReadOnly = True Then
                        If .CurrentRow.Index <= 0 Then

                            If cbo_ClothSales_OrderCode_forSelection.Visible Then
                                cbo_ClothSales_OrderCode_forSelection.Focus()
                            Else
                                If txt_Folding.Enabled Then txt_Folding.Focus() Else dtp_Date.Focus()
                            End If



                        Else
                            .Focus()
                            .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(dgvCOL_PCSDETAILS.WEIGHTPERMETER)

                            .CurrentCell.Selected = True

                        End If

                    Else
                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(dgvCOL_PCSDETAILS.PCSNO)
                        .CurrentCell.Selected = True

                    End If

                End If

                If (e.KeyValue = 40 And cbo_Grid_ClothType.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                    .Focus()
                    .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)
                End If

            End If

        End With


    End Sub

    Private Sub cbo_Grid_Type_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Grid_ClothType.KeyPress

        With dgv_Details

            If .Rows.Count > 0 Then

                Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_ClothType, Nothing, "ClothType_Head", "ClothType_Name", "(ClothType_IdNo BetWeen 1 and 5)", "(ClothType_IdNo = 0)")

                If Asc(e.KeyChar) = 13 Then

                    If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(dgvCOL_PCSDETAILS.CLOTHTYPE).Value) = "" Then
                        If dgv_Production_Wages_Details.Visible = True Then
                            dgv_Production_Wages_Details.Focus()
                            dgv_Production_Wages_Details.CurrentCell = dgv_Production_Wages_Details.Rows(0).Cells(dgvCOL_PRODUCTIONDETAILS.EMPLOYEENAME)

                        Else

                            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                save_record()
                            Else
                                dtp_Date.Focus()
                            End If

                        End If

                    Else

                        .Focus()
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                    End If

                End If

            End If

        End With
    End Sub

    Private Sub cbo_Grid_Type_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_ClothType.TextChanged
        Try

            If FrmLdSTS = True Then Exit Sub
            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub

            If cbo_Grid_ClothType.Visible Then
                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                With dgv_Details
                    If .Rows.Count > 0 Then
                        If Val(cbo_Grid_ClothType.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.CLOTHTYPE Then
                            .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_ClothType.Text)
                        End If
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub btn_close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        save_record()
    End Sub

    Private Sub dgtxt_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyDown
        With dgv_Details
            If .Visible Then
                If .Rows.Count > 0 Then
                    If Trim(.CurrentRow.Cells(dgvCOL_PCSDETAILS.BALENO).Value) <> "" Then
                        e.Handled = True
                        e.SuppressKeyPress = True
                    End If
                End If
            End If
        End With
    End Sub

    Private Sub dgtxt_Details_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_Details.KeyPress
        With dgv_Details
            If .Visible Then
                If .Rows.Count > 0 Then
                    If Trim(.CurrentRow.Cells(dgvCOL_PCSDETAILS.BALENO).Value) <> "" Then
                        e.Handled = True

                    Else
                        If .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.METERS Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.GROSSWEIGHT Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.NETWEIGHT Or .CurrentCell.ColumnIndex = dgvCOL_PCSDETAILS.WEIGHTPERMETER Then
                            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                                e.Handled = True
                            End If
                        End If

                    End If
                End If
            End If
        End With
    End Sub

    Private Sub dgtxt_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.Enter
        dgv_Details.EditingControl.BackColor = Color.Lime
        dgv_Details.EditingControl.ForeColor = Color.Blue
        dgv_Details.SelectAll()
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
        Dim n As Integer
        Dim Led_IdNo As Integer, Del_IdNo As Integer, Cnt_IdNo As Integer, Cloth_Idno As Integer, Fer_No As Integer
        Dim Condt As String = ""
        Dim Join1 As String = ""
        Dim cHK_Mtr As Double = 0
        Dim cHK_wGT As Double = 0
        Dim Lom_IdNo As Integer = 0
        Dim StCode As String = "", BmNo As String = ""

        Try

            Condt = ""
            Del_IdNo = 0
            Cnt_IdNo = 0

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

            If Val(Led_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Ledger_IdNo = " & Str(Val(Led_IdNo)) & ")"
            End If

            If Trim(Cbo_Filter_ClothName.Text) <> "" Then
                Cloth_Idno = Common_Procedures.Cloth_NameToIdNo(con, Cbo_Filter_ClothName.Text)
            End If

            If Val(Cloth_Idno) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Cloth_IdNo = " & Str(Val(Cloth_Idno)) & ")"
            End If

            If Trim(Cbo_Filter_FerNo.Text) <> "" Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (a.Weaver_Piece_Checking_Code IN (select ts1.Weaver_Piece_Checking_Code from Weaver_ClothReceipt_Piece_Details ts1 Where ts1.Piece_No = '" & Trim(Cbo_Filter_FerNo.Text) & "' and ts1.Weaver_Piece_Checking_Code = a.Weaver_Piece_Checking_Code ) )"
            End If

            Lom_IdNo = 0
            If Trim(cbo_Filter_LoomNo.Text) <> "" Then
                Lom_IdNo = Common_Procedures.Loom_NameToIdNo(con, cbo_Filter_LoomNo.Text)
            End If
            If Val(Lom_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & " (b.Loom_Idno = " & Str(Val(Lom_IdNo)) & ")"
            End If

            StCode = "" : BmNo = ""
            If Trim(cbo_Filter_BeamNo.Text) <> "" Then
                da = New SqlClient.SqlDataAdapter("select * from Stock_SizedPavu_Processing_Details where BeamNo_SetCode_forSelection = '" & Trim(cbo_Filter_BeamNo.Text) & "'", con)
                dt2 = New DataTable
                da.Fill(dt2)
                If dt2.Rows.Count > 0 Then
                    StCode = dt2.Rows(0).Item("set_code").ToString
                    BmNo = dt2.Rows(0).Item("beam_no").ToString
                End If

                If Trim(StCode) <> "" And Trim(BmNo) <> "" Then
                    Condt = Trim(Condt) & IIf(Trim(Condt) <> "", " and ", "") & "  ( (b.Set_Code1 = '" & Trim(StCode) & "' and b.Beam_No1 = '" & Trim(BmNo) & "') or (b.Set_Code2 = '" & Trim(StCode) & "' and b.Beam_No2 = '" & Trim(BmNo) & "') ) "
                End If

            End If

            'Join1 = ""
            'If Trim(cbo_Filter_BeamNo.Text) <> "" Then
            '    Condt = Trim(Condt) & IIf(Trim(Condt) <> "", " and ", "") & " tSPP.BeamNo_SetCode_forSelection = '" & Trim(cbo_Filter_BeamNo.Text) & "'"
            '    Join1 = " LEFT OUTER JOIN Stock_SizedPavu_Processing_Details tSPP ON tSPP.BeamNo_SetCode_forSelection = '" & Trim(cbo_Filter_BeamNo.Text) & "' and ( (tSPP.Set_Code = B.Set_Code1 and tSPP.Beam_No = B.Beam_No1) or (tSPP.Set_Code = B.Set_Code2 and tSPP.Beam_No = B.Beam_No2) ) "
            'End If



            da = New SqlClient.SqlDataAdapter("select a.*, e.Ledger_Name  from Weaver_Piece_Checking_Head a iNNER JOIN Weaver_Cloth_Receipt_Head B ON A.Weaver_Piece_Checking_Code = B.Weaver_Piece_Checking_Code inner join Ledger_head e on a.Ledger_IdNo = e.Ledger_idno Where a.Receipt_Type = 'L' and a.company_idno =" & Str(Val(lbl_Company.Tag)) & "and a.Weaver_Piece_Checking_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.Weaver_Piece_Checking_Date, a.for_orderby, a.Weaver_Piece_Checking_No", con)
            dt2 = New DataTable
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = dt2.Rows(i).Item("Weaver_Piece_Checking_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Piece_Receipt_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Weaver_Piece_Checking_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Ledger_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = Format(Val(dt2.Rows(i).Item("Total_Checking_Meters").ToString), "########0.000")
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Val(dt2.Rows(i).Item("Total_Weight").ToString)

                    cHK_Mtr = cHK_Mtr + Format(Val(dt2.Rows(i).Item("Total_Checking_Meters").ToString), "########0.000")
                    cHK_wGT = cHK_wGT + Val(dt2.Rows(i).Item("Total_Weight").ToString)

                Next i

            End If

            dt2.Clear()


            dgv_fILTER_Total.Rows.Add()
            dgv_fILTER_Total.Rows(0).Cells(2).Value = "TOTAL"
            dgv_fILTER_Total.Rows(0).Cells(4).Value = Format(Val(cHK_Mtr), "########0.00")
            dgv_fILTER_Total.Rows(0).Cells(5).Value = Format(Val(cHK_wGT), "########0.000")


        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT FILTER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally
            dt2.Dispose()
            da.Dispose()

            If dgv_Filter_Details.Visible And dgv_Filter_Details.Enabled Then dgv_Filter_Details.Focus()

        End Try

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
    Private Sub cbo_Filter_BeamNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_BeamNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Stock_SizedPavu_Processing_Details", "BeamNo_SetCode_forSelection", "", "(BeamNo_SetCode_forSelection = '')")
    End Sub

    Private Sub cbo_Filter_BeamNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_BeamNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_BeamNo, cbo_Filter_LoomNo, btn_Filter_Show, "Stock_SizedPavu_Processing_Details", "BeamNo_SetCode_forSelection", "", "(BeamNo_SetCode_forSelection = '')")
    End Sub

    Private Sub cbo_Filter_BeamNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_BeamNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_BeamNo, btn_Filter_Show, "Stock_SizedPavu_Processing_Details", "BeamNo_SetCode_forSelection", "", "(BeamNo_SetCode_forSelection = '')")
    End Sub
    Private Sub cbo_Filter_PartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_PartyName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_PartyName, dtp_Filter_ToDate, Cbo_Filter_ClothName, "Ledger_AlaisHead", "Ledger_DisplayName", " (  Ledger_Type = 'WEAVER' OR Ledger_Type = 'JOBWORKER')", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Filter_PartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_PartyName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_PartyName, Cbo_Filter_ClothName, "Ledger_AlaisHead", "Ledger_DisplayName", " (  Ledger_Type = 'WEAVER' OR Ledger_Type = 'JOBWORKER')", "(Ledger_idno = 0)")
    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        On Error Resume Next

        movno = Trim(dgv_Filter_Details.CurrentRow.Cells(0).Value)

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



    Private Sub cbo_QualityName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = lbl_ClothName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Public Sub Get_LotDetails(ByVal LtNo As String)
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim LtCd As String = ""
        Dim ChkNo As String = ""
        Dim n As Integer = 0
        Dim ChkDate As Date
        Dim InsEntry As Boolean = False
        Dim LmID As Integer = 0

        If Trim(LtNo) = "" Then
            MessageBox.Show("Invalid Lot No", "DOES NOT SHOW LOT DETAILS...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_LotNo.Enabled And cbo_LotNo.Visible Then cbo_LotNo.Focus()
            Exit Sub
        End If

        LtCd = LtNo
        If Not (Trim(LtNo) Like "*/??-??") Then LtCd = LtCd & "/" & Trim(Common_Procedures.FnYearCode)
        LtCd = Trim(Val(lbl_Company.Tag)) & "-" & Trim(LtCd)

        Da = New SqlClient.SqlDataAdapter("Select Weaver_Piece_Checking_No from Weaver_Piece_Checking_Head where Receipt_PkCondition = '" & Trim(Pk_Condition2) & "' and Piece_Receipt_Code = '" & Trim(LtCd) & "' and Receipt_Type = 'L'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            Call move_record(Dt1.Rows(0).Item("Weaver_Piece_Checking_No").ToString)

        Else

            LmID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)
            If Val(LmID) = 0 Then
                MessageBox.Show("Invalid LoomNo", "DOES NOT SHOW LOT DETAILS...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If cbo_LoomNo.Enabled And cbo_LoomNo.Visible Then cbo_LoomNo.Focus()
                Exit Sub
            End If

            InsEntry = Insert_Entry
            ChkDate = dtp_Date.Value
            ChkNo = Trim(lbl_ChkNo.Text)

            new_record()

            Insert_Entry = InsEntry
            dtp_Date.Text = ChkDate
            cbo_LotNo.Text = Trim(LtNo)
            cbo_LoomNo.Text = Common_Procedures.Loom_IdNoToName(con, LmID)
            lbl_ChkNo.Text = ChkNo

            Da = New SqlClient.SqlDataAdapter("select a.*, b.ledger_name, c.cloth_name, c.Multiple_WeftCount_Status, c.RollTube_Wgt, d.Loom_Name from Weaver_Cloth_Receipt_Head a, ledger_head b, cloth_head c, Loom_Head d where a.Weaver_ClothReceipt_Code = '" & Trim(LtCd) & "' and a.Loom_Idno = " & Str(Val(LmID)) & " and a.Receipt_Type = 'L' and a.ledger_idno = b.ledger_idno and a.cloth_idno = c.cloth_idno and a.Loom_IdNo = d.Loom_IdNo", con)
            Dt2 = New DataTable
            Da.Fill(Dt2)
            If Dt2.Rows.Count > 0 Then
                If IsDBNull(Dt2.Rows(0).Item("Weaver_Piece_Checking_Date").ToString) = False Then
                    If IsDate(Dt2.Rows(0).Item("Weaver_Piece_Checking_Date").ToString) = True Then
                        dtp_Date.Text = Dt2.Rows(0).Item("Weaver_Piece_Checking_Date").ToString
                    End If
                End If

                lbl_RecCode.Text = Dt2.Rows(0).Item("Weaver_ClothReceipt_Code").ToString
                lbl_RecDate.Text = Dt2.Rows(0).Item("Weaver_ClothReceipt_Date").ToString
                lbl_PartyName.Text = Dt2.Rows(0).Item("ledger_name").ToString
                lbl_ClothName.Text = Dt2.Rows(0).Item("cloth_name").ToString
                cbo_LoomNo.Text = Dt2.Rows(0).Item("loom_name").ToString
                lbl_RecMtrs.Text = Dt2.Rows(0).Item("Receipt_Meters").ToString
                txt_Crimp.Text = Val(Dt2.Rows(0).Item("Crimp_Percentage").ToString)
                txt_Folding.Text = Val(Dt2.Rows(0).Item("Folding").ToString)
                txt_BarCode.Text = Val(Dt2.Rows(0).Item("Bar_Code").ToString)
                txt_TareWeight.Text = Val(Dt2.Rows(0).Item("RollTube_Wgt").ToString)
                lbl_FabricLotNo.Text = Dt2.Rows(0).Item("Fabric_LotNo").ToString
                cbo_ClothSales_OrderCode_forSelection.Text = Dt2.Rows(0).Item("ClothSales_OrderCode_forSelection").ToString

                If Val(Dt2.Rows(0).Item("Multiple_WeftCount_Status").ToString) = 1 Then
                    btn_Show_WeftConsumption_Details.Visible = True
                    btn_Show_WeftConsumption_Details.BringToFront()
                    get_Multiple_WeftYarn_Consumption_Count_Details(Val(Dt2.Rows(0).Item("Cloth_IdNo").ToString))
                Else
                    btn_Show_WeftConsumption_Details.Visible = False
                    dgv_Weft_Consumption_Details.Rows.Clear()
                End If

                With dgv_Details

                    .Rows.Clear()

                    n = .Rows.Add()

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1544" Or Common_Procedures.settings.CustomerCode = "1608" Then '---- SRI SRINIVASA TEXTILES (PALLADAM)
                        Dim LtNo1 As String = ""

                        LtNo1 = LtNo
                        If (Trim(LtNo1) Like "*/??-??") Then LtNo1 = Microsoft.VisualBasic.Left(LtNo1, Len(LtNo1) - 6)
                        .Rows(n).Cells(dgvCOL_PCSDETAILS.PCSNO).Value = LtNo1   ' cbo_LotNo.Text

                    ElseIf Common_Procedures.settings.ClothReceipt_PieceNo_Concept = "1,2,3" Or Trim(UCase(Common_Procedures.settings.ClothReceipt_PieceNo_Concept)) = "CONTINUOUS NO" Then
                        .Rows(n).Cells(dgvCOL_PCSDETAILS.PCSNO).Value = "1"
                    Else
                        .Rows(n).Cells(dgvCOL_PCSDETAILS.PCSNO).Value = "A"
                    End If

                    .Rows(n).Cells(dgvCOL_PCSDETAILS.CLOTHTYPE).Value = Common_Procedures.ClothType.Type1
                    .Rows(n).Cells(dgvCOL_PCSDETAILS.METERS).Value = Format(Val(Dt2.Rows(0).Item("Receipt_Meters").ToString), "########0.00")
                    .Rows(n).Cells(dgvCOL_PCSDETAILS.GROSSWEIGHT).Value = ""
                    .Rows(n).Cells(dgvCOL_PCSDETAILS.NETWEIGHT).Value = ""
                    .Rows(n).Cells(dgvCOL_PCSDETAILS.WEIGHTPERMETER).Value = ""
                    .Rows(n).Cells(dgvCOL_PCSDETAILS.STS).Value = ""
                    .Rows(n).Cells(dgvCOL_PCSDETAILS.BALENO).Value = ""


                    n = .Rows.Count - 1
                    If (Trim(.Rows(n).Cells(dgvCOL_PCSDETAILS.CLOTHTYPE).Value) = "" And Val(.Rows(n).Cells(dgvCOL_PCSDETAILS.METERS).Value) <> 0) Or (.Rows(n).Cells(dgvCOL_PCSDETAILS.CLOTHTYPE).Value = Nothing And .Rows(n).Cells(dgvCOL_PCSDETAILS.METERS).Value = Nothing) Then
                        .Rows(n).Cells(dgvCOL_PCSDETAILS.PCSNO).Value = ""
                    End If

                End With

            Else
                MessageBox.Show("LotNo does not exists (or) LoomNo/LotNo does not Match", "DOES NOT SHOW LOT DETAILS...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If cbo_LotNo.Enabled And cbo_LotNo.Visible Then cbo_LotNo.Focus()
                Exit Sub

            End If
            Dt2.Clear()

        End If
        Dt1.Clear()

        Dt1.Dispose()
        Dt2.Dispose()
        Da.Dispose()

    End Sub

    Private Sub btn_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Get_LotDetails(cbo_LotNo.Text)
        If txt_Folding.Enabled Then txt_Folding.Focus()
    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        dgv_Details_KeyUp(sender, e)
    End Sub

    Private Sub ConsumedPavu_Calculation(Optional ByVal Clo_ID As Integer = 0, Optional ByVal Lm_ID As Integer = 0, Optional ByVal CloChkMtrs As Single = 0, Optional ByVal Wdth_Typ As String = "", Optional ByRef ConsPavu As Single = 0, Optional ByRef BeamConPavu As Single = 0, Optional ByVal tr As SqlClient.SqlTransaction = Nothing)
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim NoofBeams As Integer = 0
        Dim BmNo1 As String = ""
        Dim BmNo2 As String = ""
        Dim vTot_ChkWGT As String = 0

        BeamConPavu = 0
        vTot_ChkWGT = 0
        If Val(CloChkMtrs) = 0 Then
            With dgv_Details_Total
                If .RowCount > 0 Then
                    CloChkMtrs = Val(.Rows(0).Cells(dgvCOL_PCSDETAILS.METERS).Value)
                    vTot_ChkWGT = Val(.Rows(0).Cells(dgvCOL_PCSDETAILS.NETWEIGHT).Value)
                End If
            End With
        End If

        If Val(CloChkMtrs) = 0 Then
            lbl_ConsPavu.Text = ""
            Exit Sub
        End If
        If Clo_ID = 0 Then
            Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, lbl_ClothName.Text)
        End If

        If Val(Clo_ID) = 0 Then
            lbl_ConsPavu.Text = ""
            Exit Sub
        End If

        If Lm_ID = 0 Then
            Lm_ID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)
        End If

        If Val(Lm_ID) = 0 Then
            lbl_ConsPavu.Text = ""
            Exit Sub
        End If
        BmNo1 = ""
        BmNo2 = ""
        Da1 = New SqlClient.SqlDataAdapter("Select Beam_No1, Beam_No2, Width_Type from Weaver_Cloth_Receipt_Head Where Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "'", con)
        If IsNothing(tr) = False Then
            Da1.SelectCommand.Transaction = tr
        End If
        Dt1 = New DataTable
        Da1.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            BmNo1 = Dt1.Rows(0).Item("Beam_No1").ToString
            BmNo2 = Dt1.Rows(0).Item("Beam_No2").ToString
            If Trim(Wdth_Typ) = "" Then
                Wdth_Typ = Dt1.Rows(0).Item("Width_Type").ToString
            End If
        End If
        Dt1.Clear()

        ConsPavu = Val(Common_Procedures.get_Pavu_Consumption(con, Clo_ID, Lm_ID, Val(CloChkMtrs), Trim(Wdth_Typ), tr))
        ConsPavu = Format(Val(ConsPavu), "#########0.00")
        lbl_ConsPavu.Text = Format(Val(ConsPavu), "#########0.00")


        If Trim(BmNo1) <> "" And Trim(BmNo2) <> "" Then
            NoofBeams = 2
        Else
            NoofBeams = 1
        End If
        If Val(NoofBeams) = 0 Then NoofBeams = 1

        BeamConPavu = Format(Val(ConsPavu) / NoofBeams, "#########0.00")

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        save_record()
        If Trim(UCase(LastNo)) = Trim(UCase(lbl_ChkNo.Text)) Then
            Timer1.Enabled = False
            SaveAll_STS = False
            MessageBox.Show("All entries saved sucessfully", "FOR SAVING...", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            movenext_record()
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

    Private Sub cbo_LoomNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_LoomNo.GotFocus
        If Trim(Common_Procedures.settings.CustomerCode) = "1608" Then
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Loom_Head", "Loom_Name", "( Loom_CompanyIdno = " & Val(lbl_Company.Tag) & " ) ", "(Loom_IdNo = 0 )")
        Else
            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0)")
        End If


    End Sub

    Private Sub cbo_LoomNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_LoomNo.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If Trim(Common_Procedures.settings.CustomerCode) = "1608" Then
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_LoomNo, msk_Date, cbo_LotNo, "Loom_Head", "Loom_Name", "( Loom_CompanyIdno = " & Val(lbl_Company.Tag) & " ) ", "(Loom_IdNo = 0 )")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_LoomNo, msk_Date, cbo_LotNo, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0)")
        End If
    End Sub

    Private Sub cbo_LoomNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_LoomNo.KeyPress
        If Trim(Common_Procedures.settings.CustomerCode) = "1608" Then
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_LoomNo, cbo_LotNo, "Loom_Head", "Loom_Name", "( Loom_CompanyIdno = " & Val(lbl_Company.Tag) & " ) ", "(Loom_IdNo = 0 )")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_LoomNo, cbo_LotNo, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0 )")
        End If
    End Sub

    Private Sub cbo_LoomNo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_LoomNo.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New LoomNo_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_LoomNo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If

    End Sub

    Private Sub cbo_KnotterName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_KnotterName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
    End Sub

    Private Sub cbo_KnotterName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_KnotterName.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_KnotterName, txt_Folding, txt_Wages, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
    End Sub

    Private Sub cbo_KnotterName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_KnotterName.KeyPress
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim New_Rate As Double = 0
        Dim clth_pick As Double = 0
        Dim chk_meter As Double = 0
        Dim Emp_idno As Integer = 0
        Dim Clth_idno As Integer = 0

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_KnotterName, txt_Wages, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
        If Asc(e.KeyChar) = 13 Then

            Emp_idno = Common_Procedures.Employee_NameToIdNo(con, Trim(cbo_KnotterName.Text))


            da = New SqlClient.SqlDataAdapter("select a.* from PayRoll_Employee_Head a Where a.Employee_IdNo = " & Str(Val(Emp_idno)), con)
            da.Fill(dt)

            New_Rate = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then
                    New_Rate = Val(dt.Rows(0).Item("Wages_Amount").ToString)
                End If
            End If

            dt.Dispose()
            da.Dispose()

            txt_Wages.Text = Val(New_Rate)

            wages_calculation()
        End If
    End Sub
    Private Sub wages_calculation()
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim New_Rate As Double = 0
        Dim clth_pick As Double = 0
        Dim chk_meter As Double = 0
        Dim Emp_idno As Integer = 0
        Dim Clth_idno As Integer = 0

        Clth_idno = Common_Procedures.Cloth_NameToIdNo(con, Trim(lbl_ClothName.Text))
        da = New SqlClient.SqlDataAdapter("select a.* from Cloth_Head a Where a.Cloth_Idno = " & Str(Val(Clth_idno)), con)
        da.Fill(dt1)

        clth_pick = 0
        If dt1.Rows.Count > 0 Then
            If IsDBNull(dt1.Rows(0)(0).ToString) = False Then
                clth_pick = Val(dt1.Rows(0).Item("Cloth_Pick").ToString)
            End If
        End If

        dt1.Dispose()
        da.Dispose()

        chk_meter = Val(dgv_Details_Total.Rows(0).Cells(dgvCOL_PCSDETAILS.METERS).Value)

        lbl_Wages_Amount.Text = Val(clth_pick * Val(txt_Wages.Text) * chk_meter)

    End Sub
    Private Sub cbo_KnotterName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_KnotterName.KeyUp
        If e.Control = False And e.KeyValue = 17 Then

            Dim f As New Payroll_Employee_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_KnotterName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub
    Private Sub txt_Wages_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Wages.KeyDown
        If e.KeyValue = 40 Then
            btn_save.Focus()
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If

        End If

        If (e.KeyValue = 38) Then cbo_KnotterName.Focus()
    End Sub

    Private Sub txt_Wages_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Wages.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then e.Handled = True
        If Asc(e.KeyChar) = 13 Then
            btn_save.Focus()
            If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                save_record()
            Else
                dtp_Date.Focus()
            End If
        End If
    End Sub

    Private Sub txt_Wages_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Wages.TextChanged
        wages_calculation()
    End Sub

    Private Sub cbo_Filter_LoomNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Filter_LoomNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0 )")
    End Sub

    Private Sub cbo_Filter_LoomNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Filter_LoomNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_LoomNo, Cbo_Filter_FerNo, cbo_Filter_BeamNo, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0)")
    End Sub

    Private Sub cbo_Filter_LoomNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_LoomNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_LoomNo, cbo_Filter_BeamNo, "Loom_Head", "Loom_Name", "", "(Loom_IdNo = 0 )")
    End Sub

    Public Sub Get_LotDetails_Using_Barcode(ByVal BrcdNo As String)
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim LtCd As String = ""
        Dim BrcdCd As String = ""
        Dim ChkNo As String = ""
        Dim n As Integer = 0
        Dim ChkDate As Date
        Dim InsEntry As Boolean = False
        Dim LmID As Integer = 0

        If Trim(BrcdNo) = "" Then
            MessageBox.Show("Invalid BarCode No", "DOES NOT SHOW LOT DETAILS...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If txt_BarCode.Enabled And txt_BarCode.Visible Then txt_BarCode.Focus()
            Exit Sub
        End If

        'BrcdCd = BrcdNo
        'If Not (Trim(BrcdNo) Like "*/??-??") Then BrcdCd = BrcdCd & "/" & Trim(Common_Procedures.FnYearCode)
        'BrcdCd = Trim(Val(lbl_Company.Tag)) & "-" & Trim(BrcdCd)
        BrcdCd = Trim(txt_BarCode.Text)

        Da = New SqlClient.SqlDataAdapter("Select Weaver_Piece_Checking_No from Weaver_Piece_Checking_Head where Receipt_PkCondition = '" & Trim(Pk_Condition2) & "' and Bar_Code = '" & Trim(BrcdCd) & "' and Receipt_Type = 'L'", con)
        Dt1 = New DataTable
        Da.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then
            Call move_record(Dt1.Rows(0).Item("Weaver_Piece_Checking_No").ToString)

        Else

            'LmID = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)
            'If Val(LmID) = 0 Then
            '    MessageBox.Show("Invalid LoomNo", "DOES NOT SHOW LOT DETAILS...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '    If cbo_LoomNo.Enabled And cbo_LoomNo.Visible Then cbo_LoomNo.Focus()
            '    Exit Sub
            'End If

            InsEntry = Insert_Entry
            ChkDate = dtp_Date.Value
            ChkNo = Trim(lbl_ChkNo.Text)

            new_record()

            Insert_Entry = InsEntry
            dtp_Date.Text = ChkDate
            'txt_RollNo.Text = Trim(LtNo)
            txt_BarCode.Text = Trim(BrcdCd)
            'cbo_LoomNo.Text = Common_Procedures.Loom_IdNoToName(con, LmID)
            lbl_ChkNo.Text = ChkNo

            Da = New SqlClient.SqlDataAdapter("select a.*, b.ledger_name, c.cloth_name, c.RollTube_Wgt, d.Loom_Name from Weaver_Cloth_Receipt_Head a, ledger_head b, cloth_head c, Loom_Head d where a.Bar_Code = '" & Trim(BrcdCd) & "'  and a.Receipt_Type = 'L' and a.ledger_idno = b.ledger_idno and a.cloth_idno = c.cloth_idno and a.Loom_IdNo = d.Loom_IdNo", con)
            Dt2 = New DataTable
            Da.Fill(Dt2)
            If Dt2.Rows.Count > 0 Then
                If IsDBNull(Dt2.Rows(0).Item("Weaver_Piece_Checking_Date").ToString) = False Then
                    If IsDate(Dt2.Rows(0).Item("Weaver_Piece_Checking_Date").ToString) = True Then
                        dtp_Date.Text = Dt2.Rows(0).Item("Weaver_Piece_Checking_Date").ToString
                    End If
                End If
                'txt_RollNo.Text = Trim(Dt1.Rows(0).Item("Lot_No"))
                lbl_RecCode.Text = Dt2.Rows(0).Item("Weaver_ClothReceipt_Code").ToString
                lbl_RecDate.Text = Dt2.Rows(0).Item("Weaver_ClothReceipt_Date").ToString
                lbl_PartyName.Text = Dt2.Rows(0).Item("ledger_name").ToString
                lbl_ClothName.Text = Dt2.Rows(0).Item("cloth_name").ToString
                cbo_LoomNo.Text = Dt2.Rows(0).Item("loom_name").ToString
                lbl_RecMtrs.Text = Dt2.Rows(0).Item("Receipt_Meters").ToString
                txt_Crimp.Text = Val(Dt2.Rows(0).Item("Crimp_Percentage").ToString)
                txt_Folding.Text = Val(Dt2.Rows(0).Item("Folding").ToString)
                cbo_LotNo.Text = Val(Dt2.Rows(0).Item("Weaver_ClothReceipt_No").ToString)
                txt_TareWeight.Text = Val(Dt2.Rows(0).Item("RollTube_Wgt").ToString)
                lbl_FabricLotNo.Text = Dt2.Rows(0).Item("Fabric_LotNo").ToString

                With dgv_Details

                    .Rows.Clear()

                    n = .Rows.Add()

                    If Common_Procedures.settings.ClothReceipt_PieceNo_Concept = "1,2,3" Or Trim(UCase(Common_Procedures.settings.ClothReceipt_PieceNo_Concept)) = "CONTINUOUS NO" Then
                        .Rows(n).Cells(dgvCOL_PCSDETAILS.PCSNO).Value = "1"
                    Else
                        .Rows(n).Cells(dgvCOL_PCSDETAILS.PCSNO).Value = "A"
                    End If

                    .Rows(n).Cells(dgvCOL_PCSDETAILS.CLOTHTYPE).Value = Common_Procedures.ClothType.Type1
                    .Rows(n).Cells(dgvCOL_PCSDETAILS.METERS).Value = Format(Val(Dt2.Rows(0).Item("Receipt_Meters").ToString), "########0.00")
                    .Rows(n).Cells(dgvCOL_PCSDETAILS.GROSSWEIGHT).Value = ""
                    .Rows(n).Cells(dgvCOL_PCSDETAILS.NETWEIGHT).Value = ""
                    .Rows(n).Cells(dgvCOL_PCSDETAILS.WEIGHTPERMETER).Value = ""
                    .Rows(n).Cells(dgvCOL_PCSDETAILS.STS).Value = ""
                    .Rows(n).Cells(dgvCOL_PCSDETAILS.BALENO).Value = ""


                    n = .Rows.Count - 1
                    If (Trim(.Rows(n).Cells(dgvCOL_PCSDETAILS.CLOTHTYPE).Value) = "" And Val(.Rows(n).Cells(dgvCOL_PCSDETAILS.METERS).Value) <> 0) Or (.Rows(n).Cells(dgvCOL_PCSDETAILS.CLOTHTYPE).Value = Nothing And .Rows(n).Cells(dgvCOL_PCSDETAILS.METERS).Value = Nothing) Then
                        .Rows(n).Cells(dgvCOL_PCSDETAILS.PCSNO).Value = ""
                    End If

                End With

            Else
                MessageBox.Show("Barcode does not exists (or) LoomNo/Barcode does not Match", "DOES NOT SHOW LOT DETAILS...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                If cbo_LotNo.Enabled And cbo_LotNo.Visible Then cbo_LotNo.Focus()
                Exit Sub

            End If
            Dt2.Clear()

        End If
        Dt1.Clear()

        Dt1.Dispose()
        Dt2.Dispose()
        Da.Dispose()

    End Sub

    Private Sub txt_BarCode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_BarCode.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If Trim(txt_BarCode.Text) <> "" Then
                If Trim(txt_BarCode.Text) <> Trim(txt_BarCode.Tag) Then
                    Get_LotDetails_Using_Barcode(txt_BarCode.Text)
                    txt_BarCode.Tag = txt_BarCode.Text
                End If
            End If
            If txt_Crimp.Visible = True Then
                txt_Crimp.Focus()
            Else
                txt_Crimp.Focus()
            End If
        End If
    End Sub

    Private Sub btn_Close_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close_Print.Click

        Pnl_Back.Enabled = True
        pnl_Print.Visible = False

    End Sub

    Private Sub btn_Print_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print_Cancel.Click
        btn_Close_Print_Click(sender, e)
    End Sub

    Private Sub btn_BarcodePrint_prnpnl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_BarcodePrint_prnpnl.Click
        Common_Procedures.Print_OR_Preview_Status = 0

        _NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(txt_PrintFrom.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Trim(Common_Procedures.settings.CustomerCode) = "1608" Then
            Printing_BarCode_Sticker_Format2_DosPrint_1608()
        Else
            Printing_BarCode_Sticker(_NewCode)
        End If
        btn_Close_Print_Click(sender, e)
    End Sub

    Private Sub btn_BarCodePrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_BarCodePrint.Click
        Common_Procedures.Print_OR_Preview_Status = 0
        'Printing_BarCode_Sticker()
        print_record()
    End Sub


    Public Sub print_record() Implements Interface_MDIActions.print_record
        pnl_Print.Visible = True
        Pnl_Back.Enabled = False
        txt_PrintFrom.Text = lbl_ChkNo.Text
        txt_PrintTo.Text = lbl_ChkNo.Text
        If txt_PrintFrom.Enabled And txt_PrintFrom.Visible Then
            txt_PrintFrom.Focus()
            txt_PrintFrom.SelectAll()
        End If
    End Sub

    Private Sub Printing_BarCode_Sticker(ByVal NewCode As String)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable

        Try
            If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Inhouse_Piece_Checking_Entry, New_Entry) = False Then Exit Sub

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

                If Val(Common_Procedures.settings.Printing_Show_PrintDialogue) = 1 Then
                    PrintDialog1.PrinterSettings = PrintDocument2.PrinterSettings
                    If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDocument2.PrinterSettings = PrintDialog1.PrinterSettings
                        PrintDocument2.Print()
                    End If

                Else
                    PrintDocument2.Print()

                End If

                'End If

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

        'Print_PDF_Status = False

    End Sub

    Private Sub PrintDocument2_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument2.BeginPrint
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim prtFrm As Single = 0
        Dim prtTo As Single = 0
        Dim Condt As String = ""

        prn_Det__Indx = 0

        '_NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_DetBarCdStkr = 1
        Dim SQL As String = ""

        Try

            If Val(txt_PrintFrom.Text) = 0 Then Exit Sub
            If Val(txt_PrintTo.Text) = 0 Then Exit Sub

            prtFrm = Val(Common_Procedures.OrderBy_CodeToValue(txt_PrintFrom.Text))
            prtTo = Val(Common_Procedures.OrderBy_CodeToValue(txt_PrintTo.Text))

            Condt = ""
            If Val(txt_PrintFrom.Text) <> 0 And Val(txt_PrintTo.Text) <> 0 Then
                Condt = " b.for_OrderBy between " & Str(Val(prtFrm)) & " and " & Trim(prtTo)

            ElseIf Val(txt_PrintFrom.Text) <> 0 Then
                Condt = " b.for_OrderBy = " & Str(Val(prtFrm))
            Else
                Exit Sub
            End If



            'SQL = "select a.*, b.* ,c.* , d.Cloth_Name from Weaver_Piece_Checking_Head a " & _
            '    " INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo  " & _
            '    " LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Ledger_Idno  " & _
            '    " LEFT OUTER JOIN Cloth_Head d ON d.Cloth_IdNo = a.Cloth_Idno  " & _
            '    " where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & _
            '     " and a.Weaver_Piece_Checking_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "'" & _
            '    IIf(Trim(Condt) <> "", " and ", "") & Condt & _
            '    " ORDER BY Weaver_Piece_Checking_No ASC "


            'da1 = New SqlClient.SqlDataAdapter(SQL, con)
            'prn_HdDt = New DataTable
            'da1.Fill(prn_HdDt)

            SQL = "Select a.*, b.Piece_Receipt_No, d.Cloth_Name,d.CLoth_Description from Weaver_ClothReceipt_Piece_Details a " &
                " INNER JOIN Weaver_Piece_Checking_Head b on  a.Weaver_Piece_Checking_code= b.Weaver_Piece_Checking_code " &
                 " LEFT OUTER JOIN Cloth_Head d ON d.Cloth_IdNo = a.Cloth_Idno " &
                " where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) &
                " and a.Weaver_Piece_Checking_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "'" &
                IIf(Trim(Condt) <> "", " and ", "") & Condt &
                " ORDER BY Weaver_Piece_Checking_No ASC "


            'SQL = "select a.*, tZ.*, c.Cloth_Name from Packing_Slip_Head a " & _
            '    " INNER JOIN Company_head tZ ON a.company_idno = tZ.Company_Idno  " & _
            '    " INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo  " & _
            '    " Where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & _
            '    " and a.Packing_Slip_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & _
            '    "'  and " & Trim(Other_Condition) & IIf(Trim(Condt) <> "", " and ", "") & Condt & _
            '    " order by a.for_orderby, a.Packing_Slip_Code", con)


            da2 = New SqlClient.SqlDataAdapter(SQL, con)
            prn_DetDt = New DataTable
            da2.Fill(prn_DetDt)

            If prn_DetDt.Rows.Count = 0 Then

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub PrintDocument2_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument2.PrintPage
        If prn_DetDt.Rows.Count <= 0 Then Exit Sub

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
        Dim CurY As Single
        Dim CurX As Single
        Dim BrCdX As Single = 20
        Dim BrCdY As Single = 100
        Dim vBarCode As String = ""
        Dim vFldMtrs As String = ""
        Dim ItmNm1 As String, ItmNm2 As String
        Dim No_of_Pages As Integer = 0

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
        prn_DetIndx = 0

        TxtHgt = 13.5

        Try
            If prn_DetDt.Rows.Count > 0 Then


                'Do While prn_DetBarCdStkr <= 5

                For noofitems = 1 To NoofItems_PerPage

                    vFldMtrs = 0
                    vBarCode = ""

                    If Val(prn_DetDt.Rows(prn_Det__Indx).Item("Type5_Meters").ToString) <> 0 Then
                        vFldMtrs = Format(Val(prn_DetDt.Rows(prn_Det__Indx).Item("Type5_Meters").ToString), "##########0.00")
                        vBarCode = Trim(prn_DetDt.Rows(prn_Det__Indx).Item("Checked_Pcs_Barcode_Type5").ToString)

                    ElseIf Val(prn_DetDt.Rows(prn_Det__Indx).Item("Type4_Meters").ToString) <> 0 Then
                        vFldMtrs = Format(Val(prn_DetDt.Rows(prn_Det__Indx).Item("Type4_Meters").ToString), "##########0.00")
                        vBarCode = Trim(prn_DetDt.Rows(prn_Det__Indx).Item("Checked_Pcs_Barcode_Type4").ToString)

                    ElseIf Val(prn_DetDt.Rows(prn_Det__Indx).Item("Type3_Meters").ToString) <> 0 Then
                        vFldMtrs = Format(Val(prn_DetDt.Rows(prn_Det__Indx).Item("Type3_Meters").ToString), "##########0.00")
                        vBarCode = Trim(prn_DetDt.Rows(prn_Det__Indx).Item("Checked_Pcs_Barcode_Type3").ToString)

                    ElseIf Val(prn_DetDt.Rows(prn_Det__Indx).Item("Type2_Meters").ToString) <> 0 Then
                        vFldMtrs = Format(Val(prn_DetDt.Rows(prn_Det__Indx).Item("Type2_Meters").ToString), "##########0.00")
                        vBarCode = Trim(prn_DetDt.Rows(prn_Det__Indx).Item("Checked_Pcs_Barcode_Type2").ToString)

                    Else

                        vFldMtrs = Format(Val(prn_DetDt.Rows(prn_Det__Indx).Item("Type1_Meters").ToString), "##########0.00")
                        vBarCode = Trim(prn_DetDt.Rows(prn_Det__Indx).Item("Checked_Pcs_Barcode_Type1").ToString)

                    End If

                    'If prn_DetBarCdStkr = 1 Then
                    '    vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type1_Meters").ToString), "##########0.00")
                    '    vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type1").ToString)
                    'ElseIf prn_DetBarCdStkr = 2 Then
                    '    vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type2_Meters").ToString), "##########0.00")
                    '    vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type2").ToString)
                    'ElseIf prn_DetBarCdStkr = 3 Then
                    '    vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type3_Meters").ToString), "##########0.00")
                    '    vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type3").ToString)
                    'ElseIf prn_DetBarCdStkr = 4 Then
                    '    vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type4_Meters").ToString), "##########0.00")
                    '    vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type4").ToString)
                    'ElseIf prn_DetBarCdStkr = 5 Then
                    '    vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type5_Meters").ToString), "##########0.00")
                    '    vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type5").ToString)
                    'End If

                    'vFldMtrs = Format(Val(prn_DetDt.Rows(prn_Det__Indx).Item("Type1_Meters").ToString), "##########0.00")
                    'vBarCode = Trim(prn_DetDt.Rows(prn_Det__Indx).Item("Checked_Pcs_Barcode_Type1").ToString)

                    If Val(vFldMtrs) <> 0 Then

                        'If NoofDets >= NoofItems_PerPage Then
                        '    e.HasMorePages = True
                        '    Return
                        'End If

                        CurY = TMargin

                        'CurX = LMargin - 1
                        'If NoofDets = 1 Then
                        '    CurX = CurX + ((PageWidth + RMargin) \ 2)
                        'End If

                        If noofitems Mod 2 = 0 Then
                            CurX = CurX + ((PageWidth + RMargin) \ 2)
                        End If

                        If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString) <> "" Then
                            ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString)
                        Else
                            ItmNm1 = Trim(prn_DetDt.Rows(prn_Det__Indx).Item("Cloth_Name").ToString)
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

                        pFont = New Font("Calibri", 9, FontStyle.Bold)
                        Common_Procedures.Print_To_PrintDocument(e, ItmNm1, CurX, CurY, 0, PrintWidth, pFont, , True)
                        If Trim(ItmNm2) <> "" Then
                            CurY = CurY + TxtHgt - 2
                            Common_Procedures.Print_To_PrintDocument(e, ItmNm2, CurX + 120, CurY, 1, PrintWidth, pFont, , True)
                        End If

                        pFont = New Font("Calibri", 9, FontStyle.Bold)

                        CurY = CurY + TxtHgt
                        'Common_Procedures.Print_To_PrintDocument(e, "L.NO: " & prn_HdDt.Rows(0).Item("Piece_Receipt_No").ToString & "      P.NO: " & prn_DetDt.Rows(prn_HdDt).Item("Piece_No").ToString, CurX, CurY, 0, PrintWidth, pFont, , True)
                        Common_Procedures.Print_To_PrintDocument(e, "L.NO: " & prn_DetDt.Rows(prn_Det__Indx).Item("Piece_Receipt_No").ToString & "      P.NO: " & prn_DetDt.Rows(prn_Det__Indx).Item("Piece_No").ToString, CurX, CurY, 0, PrintWidth, pFont, , True)

                        'Common_Procedures.Print_To_PrintDocument(e, "LOT NO : " & prn_HdDt.Rows(0).Item("Piece_Receipt_No").ToString, CurX, CurY, 0, PrintWidth, pFont, , True)
                        'CurY = CurY + TxtHgt
                        'Common_Procedures.Print_To_PrintDocument(e, "PCS NO : " & prn_DetDt.Rows(prn_DetIndx).Item("Piece_No").ToString, CurX, CurY, 0, PrintWidth, pFont, , True)
                        CurY = CurY + TxtHgt
                        Common_Procedures.Print_To_PrintDocument(e, "METERS : " & vFldMtrs, CurX, CurY, 0, PrintWidth, pFont, , True)

                        If Trim(Common_Procedures.settings.CustomerCode) = "1234" Then
                            CurY = CurY + TxtHgt
                            Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Loom_IdNoToName(con, prn_DetDt.Rows(prn_Det__Indx).Item("Loom_IdNo").ToString), CurX, CurY, 0, PrintWidth, pFont, , True)
                        End If

                        'vBarCode = Microsoft.VisualBasic.Left(Common_Procedures.FnYearCode, 2) & Val(lbl_Company.Tag) & Trim(prn_HdDt.Rows(0).Item("Piece_Receipt_No").ToString) & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Piece_No").ToString) & Trim(Val(prn_DetBarCdStkr))

                        'vBarCode = Chr(204) & Trim(UCase(vBarCode)) & "g" & Chr(206)
                        'BarFont = New Font("Code 128", 36, FontStyle.Regular)
                        'BarFont = New Font("Code 128", 24, FontStyle.Regular)

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

                    prn_DetIndx = prn_DetIndx + 1
                    prn_Det__Indx = prn_Det__Indx + 1

                    If prn_Det__Indx > prn_DetDt.Rows.Count - 1 Then
                        Exit For
                    End If

                Next



                'Loop


                'prn_DetBarCdStkr = 1

            End If

            If prn_Det__Indx <= prn_DetDt.Rows.Count - 1 Then
                e.HasMorePages = True
            Else
                'e.HasMorePages = False
                e.HasMorePages = False

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT BARCODE STICKER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try



    End Sub

    'Private Sub Printing_BarCode_Sticker_Format1(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
    '    Dim pFont As Font, BarFont As Font
    '    Dim LMargin As Single, RMargin As Single, TMargin As Single, BMargin As Single
    '    Dim PrintWidth As Single, PrintHeight As Single
    '    Dim PageWidth As Single, PageHeight As Single
    '    Dim I As Integer
    '    Dim NoofItems_PerPage As Integer, NoofDets As Integer
    '    Dim TxtHgt As Single
    '    Dim PpSzSTS As Boolean = False
    '    Dim LnAr(15) As Single, ClAr(15) As Single
    '    Dim CurY As Single
    '    Dim CurX As Single
    '    Dim BrCdX As Single = 20
    '    Dim BrCdY As Single = 100
    '    Dim vBarCode As String = ""
    '    Dim vFldMtrs As String = ""
    '    Dim ItmNm1 As String, ItmNm2 As String
    '    Dim No_of_Pages As Int16

    '    Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 3.25X1.18", 325, 118)
    '    PrintDocument2.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
    '    PrintDocument2.DefaultPageSettings.PaperSize = pkCustomSize1
    '    e.PageSettings.PaperSize = pkCustomSize1

    '    With PrintDocument2.DefaultPageSettings.Margins
    '        .Left = 5
    '        .Right = 2
    '        .Top = 5 ' 40
    '        .Bottom = 2
    '        LMargin = .Left
    '        RMargin = .Right
    '        TMargin = .Top
    '        BMargin = .Bottom
    '    End With

    '    e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

    '    With PrintDocument2.DefaultPageSettings.PaperSize
    '        PrintWidth = .Width - RMargin - LMargin
    '        PrintHeight = .Height - TMargin - BMargin
    '        PageWidth = .Width - RMargin
    '        PageHeight = .Height - BMargin
    '    End With
    '    If PrintDocument2.DefaultPageSettings.Landscape = True Then
    '        With PrintDocument2.DefaultPageSettings.PaperSize
    '            PrintWidth = .Height - TMargin - BMargin
    '            PrintHeight = .Width - RMargin - LMargin
    '            PageWidth = .Height - TMargin
    '            PageHeight = .Width - RMargin
    '        End With
    '    End If

    '    NoofItems_PerPage = 2

    '    TxtHgt = 13.5

    '    Try

    '        If prn_DetDt.Rows.Count > 0 Then

    '            NoofDets = 0

    '            prn_HeadIndx = 0
    '            No_of_Pages = (prn_DetDt.Rows.Count / 2) + (prn_DetDt.Rows.Count Mod 2)

    '            If prn_DetDt.Rows.Count > 0 Then

    '                Do While prn_DetIndx <= prn_DetDt.Rows.Count - 1

    '                    For noofitems = 1 To NoofItems_PerPage

    '                        Do While prn_DetBarCdStkr <= 5

    '                            vFldMtrs = 0
    '                            vBarCode = ""
    '                            If prn_DetBarCdStkr = 1 Then
    '                                vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type1_Meters").ToString), "##########0.00")
    '                                vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type1").ToString)
    '                            ElseIf prn_DetBarCdStkr = 2 Then
    '                                vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type2_Meters").ToString), "##########0.00")
    '                                vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type2").ToString)
    '                            ElseIf prn_DetBarCdStkr = 3 Then
    '                                vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type3_Meters").ToString), "##########0.00")
    '                                vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type3").ToString)
    '                            ElseIf prn_DetBarCdStkr = 4 Then
    '                                vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type4_Meters").ToString), "##########0.00")
    '                                vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type4").ToString)
    '                            ElseIf prn_DetBarCdStkr = 5 Then
    '                                vFldMtrs = Format(Val(prn_DetDt.Rows(prn_DetIndx).Item("Type5_Meters").ToString), "##########0.00")
    '                                vBarCode = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Checked_Pcs_Barcode_Type5").ToString)
    '                            End If

    '                            If Val(vFldMtrs) <> 0 Then

    '                                'If NoofDets >= NoofItems_PerPage Then
    '                                '    e.HasMorePages = True
    '                                '    Return
    '                                'End If

    '                                CurY = TMargin

    '                                'CurX = LMargin - 1
    '                                'If NoofDets = 1 Then
    '                                '    CurX = CurX + ((PageWidth + RMargin) \ 2)
    '                                'End If

    '                                If noofitems Mod 2 = 0 Then
    '                                    CurX = CurX + ((PageWidth + RMargin) \ 2)
    '                                End If

    '                                'If Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString) <> "" Then
    '                                '    ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Description").ToString)
    '                                'Else
    '                                ItmNm1 = Trim(prn_DetDt.Rows(prn_DetIndx).Item("Cloth_Name").ToString)
    '                                'End If

    '                                ItmNm2 = ""
    '                                If Len(ItmNm1) > 21 Then
    '                                    For I = 21 To 1 Step -1
    '                                        If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "/" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "\" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
    '                                    Next I
    '                                    If I = 0 Then I = 21

    '                                    ItmNm2 = Microsoft.VisualBasic.Right(Trim(ItmNm1), Len(ItmNm1) - I + 1)
    '                                    ItmNm1 = Microsoft.VisualBasic.Left(Trim(ItmNm1), I - 1)
    '                                End If

    '                                pFont = New Font("Calibri", 9, FontStyle.Bold)
    '                                Common_Procedures.Print_To_PrintDocument(e, ItmNm1, CurX, CurY, 0, PrintWidth, pFont, , True)
    '                                If Trim(ItmNm2) <> "" Then
    '                                    CurY = CurY + TxtHgt - 2
    '                                    Common_Procedures.Print_To_PrintDocument(e, ItmNm2, CurX + 120, CurY, 1, PrintWidth, pFont, , True)
    '                                End If

    '                                pFont = New Font("Calibri", 9, FontStyle.Bold)

    '                                CurY = CurY + TxtHgt
    '                                'Common_Procedures.Print_To_PrintDocument(e, "L.NO: " & prn_HdDt.Rows(0).Item("Piece_Receipt_No").ToString & "      P.NO: " & prn_DetDt.Rows(prn_HdDt).Item("Piece_No").ToString, CurX, CurY, 0, PrintWidth, pFont, , True)
    '                                Common_Procedures.Print_To_PrintDocument(e, "L.NO: " & prn_DetDt.Rows(prn_DetIndx).Item("Piece_Receipt_No").ToString & "      P.NO: " & prn_DetDt.Rows(prn_DetIndx).Item("Piece_No").ToString, CurX, CurY, 0, PrintWidth, pFont, , True)

    '                                'Common_Procedures.Print_To_PrintDocument(e, "LOT NO : " & prn_HdDt.Rows(0).Item("Piece_Receipt_No").ToString, CurX, CurY, 0, PrintWidth, pFont, , True)
    '                                'CurY = CurY + TxtHgt
    '                                'Common_Procedures.Print_To_PrintDocument(e, "PCS NO : " & prn_DetDt.Rows(prn_DetIndx).Item("Piece_No").ToString, CurX, CurY, 0, PrintWidth, pFont, , True)
    '                                CurY = CurY + TxtHgt
    '                                Common_Procedures.Print_To_PrintDocument(e, "METERS : " & vFldMtrs, CurX, CurY, 0, PrintWidth, pFont, , True)

    '                                If Trim(Common_Procedures.settings.CustomerCode) = "1234" Then
    '                                    CurY = CurY + TxtHgt
    '                                    Common_Procedures.Print_To_PrintDocument(e, Common_Procedures.Loom_IdNoToName(con, prn_DetDt.Rows(prn_DetIndx).Item("Loom_IdNo").ToString), CurX, CurY, 0, PrintWidth, pFont, , True)
    '                                End If

    '                                'vBarCode = Microsoft.VisualBasic.Left(Common_Procedures.FnYearCode, 2) & Val(lbl_Company.Tag) & Trim(prn_HdDt.Rows(0).Item("Piece_Receipt_No").ToString) & Trim(prn_DetDt.Rows(prn_DetIndx).Item("Piece_No").ToString) & Trim(Val(prn_DetBarCdStkr))

    '                                'vBarCode = Chr(204) & Trim(UCase(vBarCode)) & "g" & Chr(206)
    '                                'BarFont = New Font("Code 128", 36, FontStyle.Regular)
    '                                'BarFont = New Font("Code 128", 24, FontStyle.Regular)

    '                                vBarCode = "*" & Trim(UCase(vBarCode)) & "*"
    '                                'BarFont = New Font("Free 3 of 9", 24, FontStyle.Regular)
    '                                BarFont = New Font("Free 3 of 9", 18, FontStyle.Regular)

    '                                CurY = CurY + TxtHgt + 5
    '                                'CurY = CurY + TxtHgt + 2
    '                                'CurY = CurY + TxtHgt - 2
    '                                e.Graphics.DrawString(Trim(vBarCode), BarFont, Brushes.Black, CurX, CurY)

    '                                pFont = New Font("Calibri", 14, FontStyle.Bold)
    '                                'CurY = CurY + TxtHgt + TxtHgt + 5
    '                                CurY = CurY + TxtHgt + TxtHgt - 6
    '                                Common_Procedures.Print_To_PrintDocument(e, Trim(vBarCode), CurX + 5, CurY, 0, PrintWidth, pFont, , True)

    '                                NoofDets = NoofDets + 1

    '                            End If

    '                            prn_DetBarCdStkr = prn_DetBarCdStkr + 1

    '                        Loop

    '                        prn_DetBarCdStkr = 1
    '                        prn_DetIndx = prn_DetIndx + 1



    '                    Next

    '                    If prn_DetIndx Mod 2 = 0 Then
    '                        prn_HeadIndx = prn_HeadIndx + 1
    '                    End If



    '                    'prn_HeadIndx = 0
    '                    'No_of_Pages = prn_DetDt.Rows.Count / 2
    '                    If prn_HeadIndx < No_of_Pages Then
    '                        e.HasMorePages = True
    '                    Else
    '                        e.HasMorePages = False
    '                    End If

    '                Loop

    '            End If

    '        End If




    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "DOES NOT PRINT BARCODE STICKER...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

    '    End Try

    '    'e.HasMorePages = False

    'End Sub

    Private Sub txt_PrintTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PrintTo.KeyDown
        If e.KeyCode = Keys.Down Then
            btn_BarcodePrint_prnpnl.Focus()
        End If
        If e.KeyCode = Keys.Up Then
            txt_PrintFrom.Focus()
        End If
    End Sub

    Private Sub txt_PrintTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PrintTo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            btn_BarcodePrint_prnpnl_Click(sender, e)
        End If
    End Sub


    Private Sub btn_UserModification_Click(sender As Object, e As System.EventArgs) Handles btn_UserModification.Click
        If Val(Common_Procedures.User.IdNo) = 1 Then
            Dim f1 As New User_Modifications
            f1.Entry_Name = Me.Name
            f1.Entry_PkValue = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
            f1.ShowDialog()
        End If
    End Sub

    Private Sub dgv_Production_Wages_CellEndEdit(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Production_Wages_Details.CellEndEdit
        dgv_Production_Wages_CellLeave(sender, e)
    End Sub

    Private Sub dgv_Production_Wages_Details_RowsAdded(sender As Object, e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Production_Wages_Details.RowsAdded
        Dim n As Integer

        With dgv_Production_Wages_Details
            n = .RowCount
            .Rows(n - 1).Cells(dgvCOL_PRODUCTIONDETAILS.SLNO).Value = Val(n)
        End With
    End Sub

    Private Sub dgv_Production_Wages_CellEnter(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Production_Wages_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim rect As Rectangle
        Dim n As Integer = 0

        If FrmLdSTS = True Then Exit Sub
        If IsNothing(dgv_Production_Wages_Details.CurrentCell) Then Exit Sub

        With dgv_Production_Wages_Details

            If e.ColumnIndex = dgvCOL_PRODUCTIONDETAILS.EMPLOYEENAME Then

                If cbo_grid_employee.Visible = False Or Val(cbo_grid_employee.Tag) <> e.RowIndex Then

                    cbo_grid_employee.Tag = -1
                    Da = New SqlClient.SqlDataAdapter("select Employee_Name from PayRoll_Employee_Head  order by Employee_Name", con)
                    Dt2 = New DataTable
                    Da.Fill(Dt2)
                    cbo_grid_employee.DataSource = Dt2
                    cbo_grid_employee.DisplayMember = "Employee_Name"

                    rect = .GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False)

                    cbo_grid_employee.Left = .Left + rect.Left
                    cbo_grid_employee.Top = .Top + rect.Top
                    cbo_grid_employee.Width = rect.Width
                    cbo_grid_employee.Height = rect.Height

                    cbo_grid_employee.Text = .CurrentCell.Value

                    cbo_grid_employee.Tag = Val(e.RowIndex)
                    cbo_grid_employee.Visible = True

                    cbo_grid_employee.BringToFront()
                    cbo_grid_employee.Focus()

                End If


            Else

                cbo_grid_employee.Visible = False

            End If

        End With

    End Sub

    Private Sub dgv_Production_Wages_CellLeave(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Production_Wages_Details.CellLeave
        With dgv_Production_Wages_Details
            If FrmLdSTS = True Then Exit Sub
            If IsNothing(dgv_Production_Wages_Details.CurrentCell) Then Exit Sub
            If .CurrentCell.ColumnIndex = dgvCOL_PRODUCTIONDETAILS.METERS Or .CurrentCell.ColumnIndex = dgvCOL_PRODUCTIONDETAILS.RATE Or .CurrentCell.ColumnIndex = dgvCOL_PRODUCTIONDETAILS.AMOUNT Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                End If
            End If
        End With

    End Sub

    Private Sub dgv_Production_Wages_CellValueChanged(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Production_Wages_Details.CellValueChanged
        Try

            If FrmLdSTS = True Then Exit Sub
            If IsNothing(dgv_Production_Wages_Details.CurrentCell) Then Exit Sub

            With dgv_Production_Wages_Details

                If IsNothing(dgv_Production_Wages_Details.CurrentCell) Then Exit Sub

                If .Visible Then
                    If .Rows.Count > 0 Then

                        If e.ColumnIndex = dgvCOL_PRODUCTIONDETAILS.METERS Or e.ColumnIndex = dgvCOL_PRODUCTIONDETAILS.RATE Then

                            If Val(.CurrentRow.Cells(dgvCOL_PRODUCTIONDETAILS.RATE).Value) <> 0 Then
                                .CurrentRow.Cells(dgvCOL_PRODUCTIONDETAILS.AMOUNT).Value = Format(Val(.CurrentRow.Cells(dgvCOL_PRODUCTIONDETAILS.METERS).Value) * Val(.CurrentRow.Cells(dgvCOL_PRODUCTIONDETAILS.RATE).Value), "#########0.00")

                            End If

                            Total_ProductionWages_Calculation()
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            '------

        End Try

    End Sub

    Private Sub dgv_Production_Wages_EditingControlShowing(sender As Object, e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Production_Wages_Details.EditingControlShowing
        dgtxt_WagesDetails = CType(dgv_Production_Wages_Details.EditingControl, DataGridViewTextBoxEditingControl)
        dgtxt_WagesDetails.CharacterCasing = CharacterCasing.Upper
    End Sub

    Private Sub dgv_Production_Wages_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles dgv_Production_Wages_Details.KeyUp
        Dim i As Integer
        Dim n As Integer
        Try
            If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

                With dgv_Production_Wages_Details
                    If .Rows.Count > 0 Then

                        n = .CurrentRow.Index

                        If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                            For i = 0 To .ColumnCount - 1
                                .Rows(n).Cells(i).Value = ""
                            Next

                        Else
                            .Rows.RemoveAt(n)

                        End If

                        Total_ProductionWages_Calculation()

                    End If

                End With

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS KEYUP....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub dgv_Production_Wages_LostFocus(sender As Object, e As System.EventArgs) Handles dgv_Production_Wages_Details.LostFocus
        On Error Resume Next
        If IsNothing(dgv_Production_Wages_Details.CurrentCell) Then Exit Sub
        dgv_Production_Wages_Details.CurrentCell.Selected = False
    End Sub

    Private Sub Total_ProductionWages_Calculation()
        Dim Sno As Integer
        Dim Tot_WagAmt As Single
        Dim Tot_WagMtrs As Single


        With dgv_Production_Wages_Details
            For i = 0 To .RowCount - 1

                Sno = Sno + 1
                .Rows(i).Cells(dgvCOL_PRODUCTIONDETAILS.SLNO).Value = Sno

                If Val(.Rows(i).Cells(dgvCOL_PRODUCTIONDETAILS.METERS).Value) <> 0 Or Val(.Rows(i).Cells(dgvCOL_PRODUCTIONDETAILS.AMOUNT).Value) <> 0 Then
                    Tot_WagMtrs = Tot_WagMtrs + Val(.Rows(i).Cells(dgvCOL_PRODUCTIONDETAILS.METERS).Value)
                    Tot_WagAmt = Tot_WagAmt + Val(.Rows(i).Cells(dgvCOL_PRODUCTIONDETAILS.AMOUNT).Value)
                End If

            Next

        End With

        With dgv_production_wages_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(dgvCOL_PRODUCTIONDETAILS.METERS).Value = Format(Val(Tot_WagMtrs), "#########0.00")
            .Rows(0).Cells(dgvCOL_PRODUCTIONDETAILS.AMOUNT).Value = Format(Val(Tot_WagAmt), "#########0.00")
        End With

    End Sub

    Private Sub dgtxt_WagesDetails_Enter(sender As Object, e As System.EventArgs) Handles dgtxt_WagesDetails.Enter
        Try
            dgv_Production_Wages_Details.EditingControl.BackColor = Color.Lime
            dgv_Production_Wages_Details.EditingControl.ForeColor = Color.Blue
            dgtxt_WagesDetails.SelectAll()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT ENTER....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgtxt_WagesDetails_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_WagesDetails.KeyPress
        Try
            With dgv_Production_Wages_Details
                If .Visible Then

                    If .Rows.Count > 0 Then

                        If .CurrentCell.ColumnIndex = dgvCOL_PRODUCTIONDETAILS.METERS Or .CurrentCell.ColumnIndex = dgvCOL_PRODUCTIONDETAILS.RATE Or .CurrentCell.ColumnIndex = dgvCOL_PRODUCTIONDETAILS.AMOUNT Then

                            If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                                e.Handled = True
                            End If

                        End If

                    End If

                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT KEYPRESS....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgtxt_WagesDetails_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_WagesDetails.KeyUp
        Try
            With dgv_Production_Wages_Details
                If .Visible = True Then
                    If .Rows.Count > 0 Then
                        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
                            dgv_Production_Wages_KeyUp(sender, e)
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR WHILE DETAILS TEXT KEYUP....", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub dgtxt_WagesDetails_TextChanged(sender As Object, e As System.EventArgs) Handles dgtxt_WagesDetails.TextChanged
        Try
            With dgv_Production_Wages_Details

                If .Visible Then
                    If .Rows.Count > 0 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_WagesDetails.Text)
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

    Private Sub cbo_grid_employee_GotFocus(sender As Object, e As System.EventArgs) Handles cbo_grid_employee.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
    End Sub

    Private Sub cbo_grid_employee_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles cbo_grid_employee.KeyDown

        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_grid_employee, Nothing, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")
        vcbo_KeyDwnVal = e.KeyValue

        With dgv_Production_Wages_Details

            If (e.KeyValue = 38 And cbo_grid_employee.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
                .Focus()
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCOL_PCSDETAILS.PCSNO)
                Else
                    txt_Folding.Focus()
                End If

            End If
            If (e.KeyValue = 40 And cbo_grid_employee.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
                .Focus()
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)


                If .CurrentCell.RowIndex = .Rows.Count - 1 And Trim(.Rows(.CurrentCell.RowIndex).Cells.Item(dgvCOL_PRODUCTIONDETAILS.EMPLOYEENAME).Value) = "" Then
                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        dtp_Date.Focus()
                    End If
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

                End If
            End If
        End With

    End Sub

    Private Sub cbo_grid_employee_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_grid_employee.KeyPress
        Dim da As SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim led_id As Integer = 0

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_grid_employee, Nothing, "PayRoll_Employee_Head", "Employee_Name", "", "(Employee_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then


            With dgv_Production_Wages_Details
                e.Handled = True
                If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= dgvCOL_PCSDETAILS.CLOTHTYPE And Trim(.CurrentRow.Cells(dgvCOL_PCSDETAILS.CLOTHTYPE).Value) = "" Then
                    If MessageBox.Show("Do you want to save?", "FOR SAVING...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                        save_record()
                    Else
                        dtp_Date.Focus()
                    End If
                Else
                    .Focus()
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If

            End With


            led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_grid_employee.Text)

            da = New SqlClient.SqlDataAdapter("select  a.Wages_Amount from PayRoll_Employee_Head a  where a.Employee_IdNo = " & Str(Val(led_id)), con)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                With dgv_Production_Wages_Details
                    If .Rows.Count > 0 Then
                        .CurrentRow.Cells(dgvCOL_PRODUCTIONDETAILS.RATE).Value = dt.Rows(0)("Wages_Amount").ToString
                    End If
                End With

            End If

        End If

    End Sub

    Private Sub cbo_grid_employee_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles cbo_grid_employee.KeyUp
        If e.Control = False And e.KeyValue = 17 Then

            Dim f As New Payroll_Employee_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_grid_employee.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_grid_employee_TextChanged(sender As Object, e As System.EventArgs) Handles cbo_grid_employee.TextChanged
        Try

            If FrmLdSTS = True Then Exit Sub
            If IsNothing(dgv_Production_Wages_Details.CurrentCell) Then Exit Sub

            If cbo_grid_employee.Visible Then
                With dgv_Production_Wages_Details
                    If Val(cbo_grid_employee.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = dgvCOL_PRODUCTIONDETAILS.EMPLOYEENAME Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_grid_employee.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub


    Private Sub cbo_LotNo_GotFocus(sender As Object, e As System.EventArgs) Handles cbo_LotNo.GotFocus
        Dim cmd As New SqlClient.SqlCommand
        Dim Lm_id As Integer = 0

        Lm_id = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)

        cmd.Connection = con

        cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Temp_LotCode_forSelection_forChecking  = (CASE WHEN Weaver_ClothReceipt_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' THEN Weaver_ClothReceipt_No ELSE Weaver_ClothReceipt_No +  RIGHT(Weaver_ClothReceipt_Code,6) END)"
        cmd.ExecuteNonQuery()

        If Lm_id <> 0 Then

            Dim vCONDT As String = ""
            vCONDT = "(Loom_idno = " & Str(Val(Lm_id)) & ")"
            If New_Entry = True Then
                vCONDT = "(Loom_idno = " & Str(Val(Lm_id)) & " and Weaver_Piece_Checking_Code = '')"
            End If

            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Weaver_Cloth_Receipt_Head", "Temp_LotCode_forSelection_forChecking", vCONDT, "(Weaver_ClothReceipt_No = '')")

        Else

            Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Weaver_Cloth_Receipt_Head", "Temp_LotCode_forSelection_forChecking", "", "(Weaver_ClothReceipt_No = '')")

        End If


        cbo_LotNo.Tag = cbo_LotNo.Text

    End Sub

    Private Sub cbo_LotNo_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles cbo_LotNo.KeyDown

        vcbo_KeyDwnVal = e.KeyValue

        Dim Lm_id As Integer = 0
        Lm_id = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)
        If Lm_id <> 0 Then
            Dim vCONDT As String = ""
            vCONDT = "(Loom_idno = " & Str(Val(Lm_id)) & ")"
            If New_Entry = True Then
                vCONDT = "(Loom_idno = " & Str(Val(Lm_id)) & " and Weaver_Piece_Checking_Code = '')"
            End If

            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_LotNo, cbo_LoomNo, Nothing, "Weaver_Cloth_Receipt_Head", "Temp_LotCode_forSelection_forChecking", vCONDT, "(Weaver_ClothReceipt_No = '')")
        Else
            Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_LotNo, cbo_LoomNo, Nothing, "Weaver_Cloth_Receipt_Head", "Temp_LotCode_forSelection_forChecking", "", "(Weaver_ClothReceipt_No = '')")
        End If

        If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            btn_Selection_Click(sender, e)
        End If

    End Sub

    Private Sub cbo_LotNo_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_LotNo.KeyPress

        Dim Lm_id As Integer = 0
        Lm_id = Common_Procedures.Loom_NameToIdNo(con, cbo_LoomNo.Text)

        If Lm_id <> 0 Then

            Dim vCONDT As String = ""
            vCONDT = "(Loom_idno = " & Str(Val(Lm_id)) & ")"
            If New_Entry = True Then
                vCONDT = "(Loom_idno = " & Str(Val(Lm_id)) & " and Weaver_Piece_Checking_Code = '')"
            End If

            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_LotNo, Nothing, "Weaver_Cloth_Receipt_Head", "Temp_LotCode_forSelection_forChecking", vCONDT, "(Weaver_ClothReceipt_No = '' )")

        Else
            Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_LotNo, Nothing, "Weaver_Cloth_Receipt_Head", "Temp_LotCode_forSelection_forChecking", "", "(Weaver_ClothReceipt_No = '' )")

        End If

        'If Asc(e.KeyChar) = 13 Then
        '    If Trim(cbo_LotNo.Text) <> "" Then
        '        btn_Selection_Click(sender, e)
        '    End If

        'End If
        If Asc(e.KeyChar) = 13 Then
            If Trim(cbo_LotNo.Text) <> "" Then
                If Trim(cbo_LotNo.Text) <> Trim(cbo_LotNo.Tag) Then
                    Get_LotDetails(cbo_LotNo.Text)
                    cbo_LotNo.Tag = cbo_LotNo.Text
                End If
            End If
            If Trim(Common_Procedures.settings.CustomerCode) = "1234" Then
                txt_Folding.Focus()
            ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1464" Then '---- MANI OMEGA FABRICS (THIRUCHENKODU)
                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    If dgv_Details.Columns(dgvCOL_PCSDETAILS.PCSNO).ReadOnly = True Then
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCOL_PCSDETAILS.CLOTHTYPE)
                    Else
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCOL_PCSDETAILS.PCSNO)
                    End If
                Else
                    txt_Folding.Focus()
                End If

            Else
                If txt_BarCode.Visible = True And txt_BarCode.Enabled Then
                    txt_BarCode.Focus()
                Else
                    txt_Folding.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub dgtxt_Details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.TextChanged
        Try

            If FrmLdSTS = True Then Exit Sub

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

    Private Sub Check_Weight_and_Change_Colour(vCURROW As Integer)
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim Clo_ID As Integer
        Dim vMINWGTperMTR As String, vMAXWGTperMTR As String

        Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, lbl_ClothName.Text)
        If Clo_ID = 0 Then
            Exit Sub
        End If

        vMINWGTperMTR = 0 : vMAXWGTperMTR = 0

        da1 = New SqlClient.SqlDataAdapter("select tQ.Weight_Meter_Min, tQ.Weight_Meter_Max from cloth_Head tQ Where tQ.Cloth_IdNo = " & Str(Val(Clo_ID)), con)
        dt1 = New DataTable
        da1.Fill(dt1)
        If dt1.Rows.Count > 0 Then
            vMINWGTperMTR = Format(Val(dt1.Rows(0).Item("Weight_Meter_Min").ToString), "##########0.000")
            vMAXWGTperMTR = Format(Val(dt1.Rows(0).Item("Weight_Meter_Max").ToString), "##########0.000")
        End If
        dt1.Clear()

        If Val(vMINWGTperMTR) > 0 And Val(vMAXWGTperMTR) > 0 And Val(dgv_Details.Rows(vCURROW).Cells(dgvCOL_PCSDETAILS.WEIGHTPERMETER).Value) > 0 Then

            If Not (Val(dgv_Details.Rows(vCURROW).Cells(dgvCOL_PCSDETAILS.WEIGHTPERMETER).Value) >= Val(vMINWGTperMTR) And Val(dgv_Details.Rows(vCURROW).Cells(dgvCOL_PCSDETAILS.WEIGHTPERMETER).Value) <= Val(vMAXWGTperMTR)) Then

                dgv_Details.Rows(vCURROW).Cells(dgvCOL_PCSDETAILS.GROSSWEIGHT).Style.BackColor = Color.Maroon
                dgv_Details.Rows(vCURROW).Cells(dgvCOL_PCSDETAILS.GROSSWEIGHT).Style.ForeColor = Color.Red

                dgv_Details.Rows(vCURROW).Cells(dgvCOL_PCSDETAILS.NETWEIGHT).Style.BackColor = Color.Maroon
                dgv_Details.Rows(vCURROW).Cells(dgvCOL_PCSDETAILS.NETWEIGHT).Style.ForeColor = Color.Red

                dgv_Details.Rows(vCURROW).Cells(dgvCOL_PCSDETAILS.WEIGHTPERMETER).Style.BackColor = Color.Maroon
                dgv_Details.Rows(vCURROW).Cells(dgvCOL_PCSDETAILS.WEIGHTPERMETER).Style.ForeColor = Color.Red

            Else

                dgv_Details.Rows(vCURROW).Cells(dgvCOL_PCSDETAILS.GROSSWEIGHT).Style.BackColor = Color.White
                dgv_Details.Rows(vCURROW).Cells(dgvCOL_PCSDETAILS.GROSSWEIGHT).Style.ForeColor = Color.Black

                dgv_Details.Rows(vCURROW).Cells(dgvCOL_PCSDETAILS.NETWEIGHT).Style.BackColor = Color.White
                dgv_Details.Rows(vCURROW).Cells(dgvCOL_PCSDETAILS.NETWEIGHT).Style.ForeColor = Color.Black

                dgv_Details.Rows(vCURROW).Cells(dgvCOL_PCSDETAILS.WEIGHTPERMETER).Style.BackColor = Color.White
                dgv_Details.Rows(vCURROW).Cells(dgvCOL_PCSDETAILS.WEIGHTPERMETER).Style.ForeColor = Color.Black

            End If

        Else

            dgv_Details.Rows(vCURROW).Cells(dgvCOL_PCSDETAILS.GROSSWEIGHT).Style.BackColor = Color.White
            dgv_Details.Rows(vCURROW).Cells(dgvCOL_PCSDETAILS.GROSSWEIGHT).Style.ForeColor = Color.Black

            dgv_Details.Rows(vCURROW).Cells(dgvCOL_PCSDETAILS.NETWEIGHT).Style.BackColor = Color.White
            dgv_Details.Rows(vCURROW).Cells(dgvCOL_PCSDETAILS.NETWEIGHT).Style.ForeColor = Color.Black

            dgv_Details.Rows(vCURROW).Cells(dgvCOL_PCSDETAILS.WEIGHTPERMETER).Style.BackColor = Color.White
            dgv_Details.Rows(vCURROW).Cells(dgvCOL_PCSDETAILS.WEIGHTPERMETER).Style.ForeColor = Color.Black

        End If

    End Sub

    Private Sub Cbo_Filter_ClothName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Cbo_Filter_ClothName.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_Filter_ClothName, Cbo_Filter_FerNo, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
    End Sub

    Private Sub Cbo_Filter_ClothName_KeyDown(sender As Object, e As KeyEventArgs) Handles Cbo_Filter_ClothName.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_Filter_ClothName, cbo_Filter_PartyName, Cbo_Filter_FerNo, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")

    End Sub

    Private Sub Cbo_Filter_ClothName_KeyUp(sender As Object, e As KeyEventArgs) Handles Cbo_Filter_ClothName.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = Cbo_Filter_ClothName.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub Cbo_Filter_ClothName_GotFocus(sender As Object, e As EventArgs) Handles Cbo_Filter_ClothName.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "(Cloth_IdNo = 0)")
    End Sub

    Private Sub Cbo_Filter_FerNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Cbo_Filter_FerNo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, Cbo_Filter_FerNo, cbo_Filter_LoomNo, "Weaver_ClothReceipt_Piece_Details", "Piece_No", "", "", False)
    End Sub

    Private Sub Cbo_Filter_FerNo_KeyDown(sender As Object, e As KeyEventArgs) Handles Cbo_Filter_FerNo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, Cbo_Filter_FerNo, Cbo_Filter_ClothName, cbo_Filter_LoomNo, "Weaver_ClothReceipt_Piece_Details", "Piece_No", "", "")
    End Sub

    Private Sub Cbo_Filter_FerNo_GotFocus(sender As Object, e As EventArgs) Handles Cbo_Filter_FerNo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Weaver_ClothReceipt_Piece_Details", "Piece_No", "", "")
    End Sub



    Private Sub cbo_ClothSales_OrderCode_forSelection_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_ClothSales_OrderCode_forSelection.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")

        If Asc(e.KeyChar) = 13 Then

            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                If dgv_Details.Columns(dgvCOL_PCSDETAILS.PCSNO).ReadOnly = True Then
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCOL_PCSDETAILS.CLOTHTYPE)
                Else
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCOL_PCSDETAILS.PCSNO)
                End If

            Else
                btn_save.Focus()

            End If

        End If



    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_ClothSales_OrderCode_forSelection.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, Nothing, Nothing, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")

        If (e.KeyCode = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                If dgv_Details.Columns(dgvCOL_PCSDETAILS.PCSNO).ReadOnly = True Then
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCOL_PCSDETAILS.CLOTHTYPE)
                Else
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCOL_PCSDETAILS.PCSNO)
                End If

            Else
                btn_save.Focus()

            End If

        End If

        If (e.KeyCode = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

            txt_TareWeight.Focus()



        End If


    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_Enter(sender As Object, e As EventArgs) Handles cbo_ClothSales_OrderCode_forSelection.Enter
        vCLO_CONDT = "(ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode1) & "' or ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode2) & "' and Order_Close_Status = 0 )"
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")
    End Sub



    Private Sub txt_TareWeight_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_TareWeight.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If cbo_ClothSales_OrderCode_forSelection.Visible Then
                cbo_ClothSales_OrderCode_forSelection.Focus()
            Else

                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    If dgv_Details.Columns(dgvCOL_PCSDETAILS.PCSNO).ReadOnly = True Then
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCOL_PCSDETAILS.CLOTHTYPE)
                    Else
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCOL_PCSDETAILS.PCSNO)
                    End If

                Else
                    btn_save.Focus()

                End If

            End If
        End If
    End Sub

    Private Sub txt_TareWeight_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_TareWeight.KeyDown
        If e.KeyCode = 40 Then
            If cbo_ClothSales_OrderCode_forSelection.Visible Then
                cbo_ClothSales_OrderCode_forSelection.Focus()
            Else

                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    If dgv_Details.Columns(dgvCOL_PCSDETAILS.PCSNO).ReadOnly = True Then
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCOL_PCSDETAILS.CLOTHTYPE)
                    Else
                        dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(dgvCOL_PCSDETAILS.PCSNO)
                    End If

                Else
                    btn_save.Focus()

                End If

            End If
        End If

        If e.KeyCode = 38 Then
            txt_Folding.Focus()
        End If

    End Sub

    Private Sub msk_Date_KeyDown(sender As Object, e As KeyEventArgs) Handles msk_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue


        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 46 Or e.KeyCode = 8 Then
            vmskOldText = msk_Date.Text
            vmskSelStrt = msk_Date.SelectionStart
        End If


        If e.KeyCode = 40 Then
            cbo_LoomNo.Focus()
        End If
    End Sub

    Private Sub msk_Date_KeyPress(sender As Object, e As KeyPressEventArgs) Handles msk_Date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_Date.Text = Date.Today
            msk_Date.SelectionStart = 0
        End If


        If Asc(e.KeyChar) = 13 Then
            cbo_LoomNo.Focus()
        End If
    End Sub

    Private Sub msk_Date_KeyUp(sender As Object, e As KeyEventArgs) Handles msk_Date.KeyUp
        Dim vmRetTxt As String = ""
        Dim vmRetSelStrt As Integer = -1

        'If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
        '    msk_Date.Text = Date.Today
        'End If
        If e.KeyCode = 107 Then
            msk_Date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_Date.Text))
        ElseIf e.KeyCode = 109 Then
            msk_Date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_Date.Text))
        End If

        If e.KeyCode = 46 Or e.KeyCode = 8 Then

            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)

        End If
    End Sub

    Private Sub dtp_Date_TextChanged(sender As Object, e As EventArgs) Handles dtp_Date.TextChanged
        If IsDate(dtp_Date.Text) = True Then

            msk_Date.Text = dtp_Date.Text
            msk_Date.SelectionStart = 0
        End If
    End Sub

    Private Sub msk_Date_LostFocus(sender As Object, e As EventArgs) Handles msk_Date.LostFocus
        If IsDate(msk_Date.Text) = True Then
            If Microsoft.VisualBasic.DateAndTime.Day(Convert.ToDateTime(msk_Date.Text)) <= 31 Or Microsoft.VisualBasic.DateAndTime.Month(Convert.ToDateTime(msk_Date.Text)) <= 31 Then
                If Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Date.Text)) <= 2050 And Microsoft.VisualBasic.DateAndTime.Year(Convert.ToDateTime(msk_Date.Text)) >= 2000 Then
                    dtp_Date.Value = Convert.ToDateTime(msk_Date.Text)
                End If
            End If

        End If
    End Sub

    Private Sub dtp_Date_KeyUp(sender As Object, e As KeyEventArgs) Handles dtp_Date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            dtp_Date.Text = Date.Today
        End If
    End Sub

    Private Sub btn_Show_WeftConsumption_Details_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Show_WeftConsumption_Details.Click
        Pnl_Back.Enabled = False
        pnl_Weft_Consumption_Details.Visible = True
        If dgv_Weft_Consumption_Details.Rows.Count > 0 Then
            dgv_Weft_Consumption_Details.Focus()
            dgv_Weft_Consumption_Details.CurrentCell = dgv_Weft_Consumption_Details.Rows(0).Cells(0)
        End If
    End Sub

    Private Sub btn_Close_Weft_Consumption_Details_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Close_Weft_Consumption_Details.Click
        Pnl_Back.Enabled = True
        pnl_Weft_Consumption_Details.Visible = False
    End Sub

    Private Sub get_Multiple_WeftYarn_Consumption_Count_Details(ByVal CloID As Integer)
        Dim Da4 As New SqlClient.SqlDataAdapter
        Dim Dt4 As New DataTable
        Dim n, I As Integer
        Dim vMULTIWEFTSTS As String = 0

        dgv_Weft_Consumption_Details.Rows.Clear()

        If Common_Procedures.settings.Cloth_WeftConsumption_Multiple_WeftCount_Status = 1 Then '---- SOTEXPA QUALIDIS TEXTILE (SULUR)

            If CloID = 0 Then
                If Trim(lbl_ClothName.Text) <> "" Then
                    CloID = Common_Procedures.Cloth_NameToIdNo(con, lbl_ClothName.Text)
                End If
            End If

            vMULTIWEFTSTS = Common_Procedures.get_FieldValue(con, "cloth_Head", "Multiple_WeftCount_Status", "(cloth_idno = " & Str(Val(CloID)) & ")")
            If Val(vMULTIWEFTSTS) = 1 Then


                Da4 = New SqlClient.SqlDataAdapter("Select a.*, b.count_name from Cloth_Additional_Weft_Details a INNER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo Where Cloth_Idno = " & Str(Val(CloID)), con)
                Dt4 = New DataTable
                Da4.Fill(Dt4)
                If Dt4.Rows.Count > 0 Then
                    For I = 0 To Dt4.Rows.Count - 1

                        n = dgv_Weft_Consumption_Details.Rows.Add()
                        dgv_Weft_Consumption_Details.Rows(n).Cells(0).Value = Dt4.Rows(I).Item("count_name").ToString
                        dgv_Weft_Consumption_Details.Rows(n).Cells(1).Value = Dt4.Rows(I).Item("Gram_Perc_Type").ToString
                        dgv_Weft_Consumption_Details.Rows(n).Cells(2).Value = Val(Dt4.Rows(I).Item("Consumption_Gram_Perc").ToString)
                        dgv_Weft_Consumption_Details.Rows(n).Cells(3).Value = ""

                    Next

                End If
                Dt4.Clear()

            End If

        End If

    End Sub

    Private Sub ConsumedYarn_Calculation()
        Dim Da4 As New SqlClient.SqlDataAdapter
        Dim Dt4 As New DataTable
        Dim CloID As Integer
        Dim ConsYarn As String = 0, vTOTConsYarn As String = 0
        Dim vTot_ChkMtrs As String = 0
        Dim vTot_ChkWGT As String = 0
        Dim n, I As Integer
        Dim vWEFT_CONSFOR_MTRS_OR_WGT As String = 0

        CloID = Common_Procedures.Cloth_NameToIdNo(con, lbl_ClothName.Text)

        vTot_ChkMtrs = 0
        vTot_ChkWGT = 0
        With dgv_Details_Total
            If .RowCount > 0 Then
                vTot_ChkMtrs = Val(.Rows(0).Cells(dgvCOL_PCSDETAILS.METERS).Value)
                vTot_ChkWGT = Val(.Rows(0).Cells(dgvCOL_PCSDETAILS.NETWEIGHT).Value)
            End If
        End With


        dgv_Weft_Consumption_Details.Rows.Clear()

        If Common_Procedures.settings.Cloth_WeftConsumption_Multiple_WeftCount_Status = 1 Then '---- SOTEXPA QUALIDIS TEXTILE (SULUR)

            Dim vMULTIWEFTSTS As String = 0

            vMULTIWEFTSTS = Common_Procedures.get_FieldValue(con, "cloth_Head", "Multiple_WeftCount_Status", "(cloth_idno = " & Str(Val(CloID)) & ")")
            If Val(vMULTIWEFTSTS) = 1 Then

                vTOTConsYarn = 0
                Da4 = New SqlClient.SqlDataAdapter("Select a.*, b.count_name from Cloth_Additional_Weft_Details a INNER JOIN Count_Head b ON a.Count_IdNo = b.Count_IdNo Where Cloth_Idno = " & Str(Val(CloID)), con)
                Dt4 = New DataTable
                Da4.Fill(Dt4)
                If Dt4.Rows.Count > 0 Then
                    For I = 0 To Dt4.Rows.Count - 1

                        If Trim(UCase(Dt4.Rows(I).Item("ConsumptionFor_Meters_Weight").ToString)) = "WEIGHT" Then
                            vWEFT_CONSFOR_MTRS_OR_WGT = Val(vTot_ChkWGT)
                        Else
                            vWEFT_CONSFOR_MTRS_OR_WGT = Val(vTot_ChkMtrs)
                        End If

                        If Trim(UCase(Dt4.Rows(I).Item("Gram_Perc_Type").ToString)) = "%" Then
                            ConsYarn = Format(Val(vWEFT_CONSFOR_MTRS_OR_WGT) * Val(Dt4.Rows(I).Item("Consumption_Gram_Perc").ToString) / 100, "##########0.000")
                        Else
                            ConsYarn = Format(Val(vWEFT_CONSFOR_MTRS_OR_WGT) * Val(Dt4.Rows(I).Item("Consumption_Gram_Perc").ToString), "##########0.000")
                        End If

                        n = dgv_Weft_Consumption_Details.Rows.Add()
                        dgv_Weft_Consumption_Details.Rows(n).Cells(0).Value = Dt4.Rows(I).Item("count_name").ToString
                        dgv_Weft_Consumption_Details.Rows(n).Cells(1).Value = Dt4.Rows(I).Item("Gram_Perc_Type").ToString
                        dgv_Weft_Consumption_Details.Rows(n).Cells(2).Value = Val(Dt4.Rows(I).Item("Consumption_Gram_Perc").ToString)
                        dgv_Weft_Consumption_Details.Rows(n).Cells(3).Value = Format(Val(ConsYarn), "#########0.000")

                        vTOTConsYarn = Val(vTOTConsYarn) + Val(ConsYarn)

                    Next

                End If
                Dt4.Clear()

                lbl_ConsWeftYarn.Text = Format(Val(vTOTConsYarn), "#########0.000")

            Else
                GoTo GOTOLOOP1

            End If

        Else

GOTOLOOP1:
            ConsYarn = Common_Procedures.get_Weft_ConsumedYarn(con, CloID, Val(vTot_ChkMtrs))
            lbl_ConsWeftYarn.Text = Format(Val(ConsYarn), "#########0.000")

            dgv_Weft_Consumption_Details.Rows.Clear()

            If btn_Show_WeftConsumption_Details.Visible = True Then

                If Val(lbl_ConsWeftYarn.Text) <> 0 Then

                    Dim vCLO_WGTPERMTR As String = 0
                    Dim vWFTCNT_NM As String = ""
                    'vCLO_WGTPERMTR = Common_Procedures.get_FieldValue(con, "cloth_Head", "Weight_Meter_Weft", "(cloth_idno = " & Str(Val(CloID)) & ")")

                    vCLO_WGTPERMTR = 0
                    vWFTCNT_NM = ""
                    Da4 = New SqlClient.SqlDataAdapter("Select a.Weight_Meter_Weft, b.count_name from cloth_Head a INNER JOIN Count_Head b ON a.Cloth_WeftCount_IdNo = b.Count_IdNo Where a.Cloth_Idno = " & Str(Val(CloID)), con)
                    Dt4 = New DataTable
                    Da4.Fill(Dt4)
                    If Dt4.Rows.Count > 0 Then
                        vCLO_WGTPERMTR = Dt4.Rows(0).Item("Weight_Meter_Weft").ToString
                        vWFTCNT_NM = Dt4.Rows(0).Item("count_name").ToString
                    End If
                    Dt4.Clear()

                    n = dgv_Weft_Consumption_Details.Rows.Add()
                    dgv_Weft_Consumption_Details.Rows(n).Cells(0).Value = vWFTCNT_NM
                    dgv_Weft_Consumption_Details.Rows(n).Cells(1).Value = "GRAM"
                    dgv_Weft_Consumption_Details.Rows(n).Cells(2).Value = vCLO_WGTPERMTR
                    dgv_Weft_Consumption_Details.Rows(n).Cells(3).Value = lbl_ConsWeftYarn.Text

                End If

            End If

        End If


    End Sub

    Private Sub Printing_BarCode_Sticker_Format2_DosPrint_1608()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim da2 As New SqlClient.SqlDataAdapter
        Dim NewCode As String = ""
        Dim PrnTxt As String = ""
        Dim LnCnt As Integer = 0
        Dim I As Integer = 0
        Dim vBarCode As String = ""
        Dim Mtrs As String = "", vPcs_No As String = ""
        Dim ItmNm1 As String, ItmNm2 As String
        Dim vYrCode As String = ""
        Dim prtFrm As String = ""
        Dim prtTo As String = ""
        Dim Condt As String = ""


        prn_Det__Indx = 0

        '_NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_ChkNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        prn_HdDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_DetDt.Clear()
        prn_DetIndx = 0
        prn_DetSNo = 0
        prn_PageNo = 0
        prn_DetBarCdStkr = 1
        prn_HeadIndx = 0
        Dim SQL As String = ""

        Try

            If Val(txt_PrintFrom.Text) = 0 Then Exit Sub
            If Val(txt_PrintTo.Text) = 0 Then Exit Sub

            prtFrm = Val(Common_Procedures.OrderBy_CodeToValue(txt_PrintFrom.Text))
            prtTo = Val(Common_Procedures.OrderBy_CodeToValue(txt_PrintTo.Text))

            Condt = ""
            If Val(txt_PrintFrom.Text) <> 0 And Val(txt_PrintTo.Text) <> 0 Then
                Condt = " b.for_OrderBy between " & Str(Val(prtFrm)) & " and " & Trim(prtTo)

            ElseIf Val(txt_PrintFrom.Text) <> 0 Then
                Condt = " b.for_OrderBy = " & Str(Val(prtFrm))
            Else
                Exit Sub
            End If



            'SQL = "select a.*, b.* ,c.* , d.Cloth_Name from Weaver_Piece_Checking_Head a " & _
            '    " INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo  " & _
            '    " LEFT OUTER JOIN Ledger_Head c ON c.Ledger_IdNo = a.Ledger_Idno  " & _
            '    " LEFT OUTER JOIN Cloth_Head d ON d.Cloth_IdNo = a.Cloth_Idno  " & _
            '    " where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & _
            '     " and a.Weaver_Piece_Checking_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "'" & _
            '    IIf(Trim(Condt) <> "", " and ", "") & Condt & _
            '    " ORDER BY Weaver_Piece_Checking_No ASC "


            'da1 = New SqlClient.SqlDataAdapter(SQL, con)
            'prn_HdDt = New DataTable
            'da1.Fill(prn_HdDt)

            SQL = "Select a.*, b.Piece_Receipt_No, d.Cloth_Name,d.CLoth_Description from Weaver_ClothReceipt_Piece_Details a " &
                " INNER JOIN Weaver_Piece_Checking_Head b on  a.Weaver_Piece_Checking_code= b.Weaver_Piece_Checking_code " &
                 " LEFT OUTER JOIN Cloth_Head d ON d.Cloth_IdNo = a.Cloth_Idno " &
                " where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) &
                " and a.Weaver_Piece_Checking_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "'" &
                IIf(Trim(Condt) <> "", " and ", "") & Condt &
                " ORDER BY Weaver_Piece_Checking_No ASC "


            'SQL = "select a.*, tZ.*, c.Cloth_Name from Packing_Slip_Head a " & _
            '    " INNER JOIN Company_head tZ ON a.company_idno = tZ.Company_Idno  " & _
            '    " INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo  " & _
            '    " Where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & _
            '    " and a.Packing_Slip_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & _
            '    "'  and " & Trim(Other_Condition) & IIf(Trim(Condt) <> "", " and ", "") & Condt & _
            '    " order by a.for_orderby, a.Packing_Slip_Code", con)


            da2 = New SqlClient.SqlDataAdapter(SQL, con)
            prn_DetDt = New DataTable
            da2.Fill(prn_DetDt)

            If prn_DetDt.Rows.Count = 0 Then

                MessageBox.Show("This is New Entry", "DOES NOT PRINT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

            End If

            da1.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT PRINT...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

        fs = New FileStream(Common_Procedures.Dos_Printing_FileName_Path, FileMode.Create)
        sw = New StreamWriter(fs, System.Text.Encoding.Default)


        Try

            If prn_DetDt.Rows.Count > 0 Then


                Do While prn_HeadIndx <= prn_DetDt.Rows.Count - 1


                    If Val(prn_DetDt.Rows(prn_HeadIndx).Item("Type5_Meters").ToString) <> 0 Then
                        Mtrs = Format(Val(prn_DetDt.Rows(prn_HeadIndx).Item("Type5_Meters").ToString), "##########0.00")
                        vBarCode = Trim(prn_DetDt.Rows(prn_HeadIndx).Item("Checked_Pcs_Barcode_Type5").ToString)

                    ElseIf Val(prn_DetDt.Rows(prn_HeadIndx).Item("Type4_Meters").ToString) <> 0 Then
                        Mtrs = Format(Val(prn_DetDt.Rows(prn_HeadIndx).Item("Type4_Meters").ToString), "##########0.00")
                        vBarCode = Trim(prn_DetDt.Rows(prn_HeadIndx).Item("Checked_Pcs_Barcode_Type4").ToString)

                    ElseIf Val(prn_DetDt.Rows(prn_HeadIndx).Item("Type3_Meters").ToString) <> 0 Then
                        Mtrs = Format(Val(prn_DetDt.Rows(prn_HeadIndx).Item("Type3_Meters").ToString), "##########0.00")
                        vBarCode = Trim(prn_DetDt.Rows(prn_HeadIndx).Item("Checked_Pcs_Barcode_Type3").ToString)

                    ElseIf Val(prn_DetDt.Rows(prn_HeadIndx).Item("Type2_Meters").ToString) <> 0 Then
                        Mtrs = Format(Val(prn_DetDt.Rows(prn_HeadIndx).Item("Type2_Meters").ToString), "##########0.00")
                        vBarCode = Trim(prn_DetDt.Rows(prn_HeadIndx).Item("Checked_Pcs_Barcode_Type2").ToString)

                    Else

                        Mtrs = Format(Val(prn_DetDt.Rows(prn_HeadIndx).Item("Type1_Meters").ToString), "##########0.00")
                        vBarCode = Trim(prn_DetDt.Rows(prn_HeadIndx).Item("Checked_Pcs_Barcode_Type1").ToString)

                    End If

                    If Trim(prn_DetDt.Rows(prn_HeadIndx).Item("Cloth_Description").ToString) <> "" Then
                        ItmNm1 = Trim(prn_DetDt.Rows(prn_HeadIndx).Item("Cloth_Description").ToString)
                    Else
                        ItmNm1 = Trim(prn_DetDt.Rows(prn_HeadIndx).Item("Cloth_Name").ToString)
                    End If

                    vPcs_No = Trim(prn_DetDt.Rows(prn_HeadIndx).Item("Piece_No").ToString)
                    'vPcs_No = Trim(prn_DetDt.Rows(prn_HeadIndx).Item("Piece_Receipt_No").ToString) & "-" & Trim(prn_DetDt.Rows(prn_HeadIndx).Item("Piece_No").ToString)

                    If Val(Mtrs) <> 0 Then


                        If Trim(prn_DetDt.Rows(prn_HeadIndx).Item("Cloth_Description").ToString) <> "" Then
                            ItmNm1 = Trim(prn_DetDt.Rows(prn_HeadIndx).Item("Cloth_Description").ToString)
                        Else
                            ItmNm1 = Trim(prn_DetDt.Rows(prn_HeadIndx).Item("Cloth_Name").ToString)
                        End If

                        ItmNm2 = ""
                        If Len(ItmNm1) > 15 Then
                            For I = 15 To 1 Step -1
                                If Mid$(Trim(ItmNm1), I, 1) = " " Or Mid$(Trim(ItmNm1), I, 1) = "," Or Mid$(Trim(ItmNm1), I, 1) = "." Or Mid$(Trim(ItmNm1), I, 1) = "-" Or Mid$(Trim(ItmNm1), I, 1) = "_" Or Mid$(Trim(ItmNm1), I, 1) = "(" Or Mid$(Trim(ItmNm1), I, 1) = ")" Or Mid$(Trim(ItmNm1), I, 1) = "[" Or Mid$(Trim(ItmNm1), I, 1) = "]" Or Mid$(Trim(ItmNm1), I, 1) = "{" Or Mid$(Trim(ItmNm1), I, 1) = "}" Then Exit For
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


                        PrnTxt = "TEXT 617,287,""ROMAN.TTF"",180,1,14,""LOOM NO"""
                        sw.WriteLine(PrnTxt)
                        PrnTxt = "TEXT 617,242,""ROMAN.TTF"",180,1,14,""PCS NO"""
                        sw.WriteLine(PrnTxt)
                        PrnTxt = "TEXT 617,193,""ROMAN.TTF"",180,1,14,""METER"""
                        sw.WriteLine(PrnTxt)
                        PrnTxt = "TEXT 617,146,""ROMAN.TTF"",180,1,14,""WEIGHT"""
                        sw.WriteLine(PrnTxt)
                        PrnTxt = "TEXT 618,94,""0"",180,13,14,""CLOTH NAME"""
                        sw.WriteLine(PrnTxt)
                        PrnTxt = "TEXT 394,289,""ROMAN.TTF"",180,1,14,"":"""
                        sw.WriteLine(PrnTxt)
                        PrnTxt = "TEXT 394,242,""ROMAN.TTF"",180,1,14,"":"""
                        sw.WriteLine(PrnTxt)
                        PrnTxt = "TEXT 394,196,""ROMAN.TTF"",180,1,14,"":"""
                        sw.WriteLine(PrnTxt)

                        PrnTxt = "TEXT 394,148,""ROMAN.TTF"",180,1,14,"":"""
                        sw.WriteLine(PrnTxt)
                        PrnTxt = "TEXT 394,100,""ROMAN.TTF"",180,1,14,"":"""
                        sw.WriteLine(PrnTxt)
                        PrnTxt = "TEXT 375,286,""ROMAN.TTF"",180,1,14,""" & Common_Procedures.Loom_IdNoToName(con, prn_DetDt.Rows(prn_HeadIndx).Item("Loom_IdNo").ToString) & """"
                        sw.WriteLine(PrnTxt)
                        PrnTxt = "TEXT 375,240,""ROMAN.TTF"",180,1,14,""" & Trim(vPcs_No) & """"
                        sw.WriteLine(PrnTxt)
                        PrnTxt = "TEXT 375,195,""ROMAN.TTF"",180,1,14,""" & Format(Val(Mtrs), "#########0.00") & """"
                        sw.WriteLine(PrnTxt)
                        PrnTxt = "TEXT 375,146,""ROMAN.TTF"",180,1,14,""" & Format(Val(prn_DetDt.Rows(prn_HeadIndx).Item("Weight").ToString), "#########0.000") & """"
                        sw.WriteLine(PrnTxt)
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



                    End If

                    prn_HeadIndx = prn_HeadIndx + 1

                Loop

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

    Private Sub cbo_KnotterName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_KnotterName.SelectedIndexChanged

    End Sub

    Private Sub cbo_LoomNo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_LoomNo.SelectedIndexChanged

    End Sub
End Class