Public Class Weaver_Wages_Format_2
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private Filter_RowNo As Integer = -1
    Private Pk_Condition As String = "WVWAG-"
    Private PkCondition_WPYMT As String = "WPYMT-"
    Private PkCondition_WCLRC As String = "WCLRC-"
    Private PkCondition_WFRGT As String = "WFRGT-"
    Private PkCondition_GST As String = "GWVWA-"
    Private NoCalc_Status As Boolean = False
    Private dgv_ActCtrlName As String = ""
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double
    Private vCbo_ItmNm As String

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_WagesDetails As New DataGridViewTextBoxEditingControl
    Private WithEvents dgtxt_ReceiptDetails As New DataGridViewTextBoxEditingControl

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
    Private PrntCnt2ndPageSTS As Boolean = False
    Private prn_Prev_HeadIndx As Integer

    Private NoFo_STS As Integer = 0
    Private prn_Tot_EBeam_Stk As Single = 0
    Private prn_Tot_Pavu_Stk As Single = 0
    Private prn_Tot_Yarn_Stk As Single = 0
    Private prn_Tot_Amt_Bal As Single = 0
    Private prn_WagesFrmt As String = ""

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
    Private vmskOldText As String = ""
    Private vmskSelStrt As Integer = -1

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

        msk_Date.Text = ""
        dtp_Date.Text = ""
        cbo_Weaver.Text = ""
        lbl_Cloth.Text = ""
        lbl_Ends_Count.Text = ""
        cbo_Grid_Count.Text = ""

        txt_Add_Amount.Text = ""
        txt_Elogation.Text = ""
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        lbl_Excess_Short.Text = ""
        txt_Folding_Less.Text = ""
        txt_Freight_Charge.Text = ""
        txt_Less_Amount.Text = ""
        lbl_Net_Amount.Text = ""
        txt_Paid_Amount.Text = ""
        lbl_RecCode.Text = ""
        txt_Tds.Text = ""
        lbl_Tds_Amount.Text = ""
        lbl_Cooly_amt.Text = ""

        dgv_ConsYarn_Details.Rows.Clear()
        dgv_ConsYarnDetails_Total.Rows.Clear()
        dgv_ConsYarnDetails_Total.Rows.Add()

        dgv_Wages_Details.Rows.Clear()
        dgv_WagesDetails_Total.Rows.Clear()
        dgv_WagesDetails_Total.Rows.Add()

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
        cbo_Grid_Clothtype.Visible = False

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
        If Me.ActiveControl.Name <> cbo_Grid_Clothtype.Name Then
            cbo_Grid_Clothtype.Visible = False
        End If

        If Me.ActiveControl.Name <> dgv_ConsYarn_Details.Name Then
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
        If Not IsNothing(dgv_ConsYarn_Details.CurrentCell) Then dgv_ConsYarn_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_ConsYarnDetails_Total.CurrentCell) Then dgv_ConsYarnDetails_Total.CurrentCell.Selected = False

        If Not IsNothing(dgv_Receipt_Details.CurrentCell) Then dgv_Receipt_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_ReceiptDetails_Total.CurrentCell) Then dgv_ReceiptDetails_Total.CurrentCell.Selected = False

        If Not IsNothing(dgv_Wages_Details.CurrentCell) Then dgv_Wages_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_WagesDetails_Total.CurrentCell) Then dgv_WagesDetails_Total.CurrentCell.Selected = False

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
        If Not IsNothing(dgv_ConsYarn_Details.CurrentCell) Then dgv_ConsYarn_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_ConsYarnDetails_Total.CurrentCell) Then dgv_ConsYarnDetails_Total.CurrentCell.Selected = False

        If Not IsNothing(dgv_Receipt_Details.CurrentCell) Then dgv_Receipt_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_ReceiptDetails_Total.CurrentCell) Then dgv_ReceiptDetails_Total.CurrentCell.Selected = False

        If Not IsNothing(dgv_Wages_Details.CurrentCell) Then dgv_Wages_Details.CurrentCell.Selected = False
        If Not IsNothing(dgv_WagesDetails_Total.CurrentCell) Then dgv_WagesDetails_Total.CurrentCell.Selected = False

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

        da = New SqlClient.SqlDataAdapter("select Count_Name from Count_Head order by Count_Name", con)
        da.Fill(dt4)
        cbo_Grid_Count.DataSource = dt4
        cbo_Grid_Count.DisplayMember = "Cloth_Name"


        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1029" Then '---- Arul kumaran Textiles
            dgv_ConsYarn_Details.Columns(6).ReadOnly = False
        End If

        dgv_Receipt_Details.Columns(1).HeaderText = "LOT NO"
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then '---- Kalaimagal Textiles (Palladam)
            dgv_Receipt_Details.Columns(1).HeaderText = "RECNO / LOTNO"
        End If

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

        AddHandler msk_Date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Weaver.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_Cloth.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_Ends_Count.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Count.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Grid_Clothtype.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Add_Amount.GotFocus, AddressOf ControlGotFocus
        'AddHandler txt_Bits_Cooly.GotFocus, AddressOf ControlGotFocus
        'AddHandler txt_Bits_Meter.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Elogation.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_Excess_Short.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Folding_Less.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Freight_Charge.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Less_Amount.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_Net_Amount.GotFocus, AddressOf ControlGotFocus
        'AddHandler txt_Other_Cooly.GotFocus, AddressOf ControlGotFocus

        'AddHandler txt_Other_Meter.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Paid_Amount.GotFocus, AddressOf ControlGotFocus

        AddHandler txt_Tds.GotFocus, AddressOf ControlGotFocus
        AddHandler lbl_Tds_Amount.GotFocus, AddressOf ControlGotFocus
        'AddHandler lbl_Total_Cooly.GotFocus, AddressOf ControlGotFocus
        'AddHandler lbl_Total_Meter.GotFocus, AddressOf ControlGotFocus

        AddHandler dtp_Filter_Fromdate.GotFocus, AddressOf ControlGotFocus
        AddHandler dtp_Filter_ToDate.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_PartyName.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_CountName.GotFocus, AddressOf ControlGotFocus


        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Filter_Show.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_Print_Cancel.GotFocus, AddressOf ControlGotFocus
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
        AddHandler cbo_Grid_Clothtype.LostFocus, AddressOf ControlLostFocus

        AddHandler txt_Add_Amount.LostFocus, AddressOf ControlLostFocus
        'AddHandler txt_Bits_Cooly.LostFocus, AddressOf ControlLostFocus
        'AddHandler txt_Bits_Meter.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Elogation.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_Excess_Short.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Folding_Less.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Freight_Charge.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Less_Amount.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_Net_Amount.LostFocus, AddressOf ControlLostFocus
        'AddHandler txt_Other_Cooly.LostFocus, AddressOf ControlLostFocus

        'AddHandler txt_Other_Meter.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Paid_Amount.LostFocus, AddressOf ControlLostFocus
        ' AddHandler txt_Pcs.LostFocus, AddressOf ControlLostFocus


        AddHandler txt_Tds.LostFocus, AddressOf ControlLostFocus
        AddHandler lbl_Tds_Amount.LostFocus, AddressOf ControlLostFocus
        'AddHandler lbl_Total_Cooly.LostFocus, AddressOf ControlLostFocus
        'AddHandler lbl_Total_Meter.LostFocus, AddressOf ControlLostFocus


        AddHandler dtp_Filter_Fromdate.LostFocus, AddressOf ControlLostFocus
        AddHandler dtp_Filter_ToDate.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_PartyName.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_CountName.LostFocus, AddressOf ControlLostFocus

        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Filter_Show.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Cancel.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Simple_WithName.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_Simple_WithOutName.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_WithStock_WithName.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_Print_WithStock_WithoutName.LostFocus, AddressOf ControlLostFocus
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
        'AddHandler lbl_Total_Cooly.KeyDown, AddressOf TextBoxControlKeyDown
        'AddHandler lbl_Total_Meter.KeyDown, AddressOf TextBoxControlKeyDown

        AddHandler txt_PrintRange_FromNo.KeyDown, AddressOf TextBoxControlKeyDown


        'AddHandler dtp_Date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Add_Amount.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_Bits_Cooly.KeyPress, AddressOf TextBoxControlKeyPress
        'AddHandler txt_Bits_Meter.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Elogation.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler lbl_Excess_Short.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Folding_Less.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Freight_Charge.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler txt_Less_Amount.KeyPress, AddressOf TextBoxControlKeyPress



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

        If ActiveControl.Name = dgv_Receipt_Details.Name Or ActiveControl.Name = dgv_Wages_Details.Name Or ActiveControl.Name = dgv_ConsYarn_Details.Name Or TypeOf ActiveControl Is DataGridViewTextBoxEditingControl Then

            On Error Resume Next

            If ActiveControl.Name = dgv_Receipt_Details.Name Then
                dgv1 = dgv_Receipt_Details

            ElseIf ActiveControl.Name = dgv_Wages_Details.Name Then
                dgv1 = dgv_Wages_Details

            ElseIf ActiveControl.Name = dgv_ConsYarn_Details.Name Then
                dgv1 = dgv_ConsYarn_Details

            ElseIf dgv_Receipt_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Receipt_Details

            ElseIf dgv_Wages_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_Wages_Details

            ElseIf dgv_ConsYarn_Details.IsCurrentRowDirty = True Then
                dgv1 = dgv_ConsYarn_Details

            ElseIf dgv_ActCtrlName = dgv_Receipt_Details.Name Then
                dgv1 = dgv_Receipt_Details

            ElseIf dgv_ActCtrlName = dgv_Wages_Details.Name Then
                dgv1 = dgv_Wages_Details

            ElseIf dgv_ActCtrlName = dgv_ConsYarn_Details.Name Then
                dgv1 = dgv_ConsYarn_Details

            End If

            With dgv1

                If dgv1.Name = dgv_Receipt_Details.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 2 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                dgv_Wages_Details.Focus()
                                dgv_Wages_Details.CurrentCell = dgv_Wages_Details.Rows(0).Cells(1)

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(4)

                            End If

                        Else
                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And Trim(.CurrentRow.Cells(1).Value) = "" Then
                                dgv_Wages_Details.Focus()
                                dgv_Wages_Details.CurrentCell = dgv_Wages_Details.Rows(0).Cells(1)

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 4 Then
                            If .CurrentCell.RowIndex = 0 Then
                                cbo_Weaver.Focus()
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


                ElseIf dgv1.Name = dgv_Wages_Details.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 2 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                dgv_ConsYarn_Details.Focus()
                                dgv_ConsYarn_Details.CurrentCell = dgv_ConsYarn_Details.Rows(0).Cells(0)

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            End If

                        Else
                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 1 And (Trim(.CurrentRow.Cells(1).Value) = "" Or Val(.CurrentRow.Cells(1).Value) = 0) Then
                                dgv_ConsYarn_Details.Focus()
                                dgv_ConsYarn_Details.CurrentCell = dgv_ConsYarn_Details.Rows(0).Cells(0)

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                'If dgv_Receipt_Details.Rows.Count > 0 Then
                                '    dgv_Receipt_Details.Focus()
                                '    dgv_Receipt_Details.CurrentCell = dgv_Receipt_Details.Rows(0).Cells(4)

                                'Else
                                cbo_Weaver.Focus()

                                'End If

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


                ElseIf dgv1.Name = dgv_ConsYarn_Details.Name Then

                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= .ColumnCount - 1 Then
                            If .CurrentCell.RowIndex = .RowCount - 1 Then

                                txt_Elogation.Focus()

                                '.Rows.Add()
                                '.CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(1)

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(0)

                            End If

                        Else

                            If .CurrentCell.RowIndex = .RowCount - 1 And .CurrentCell.ColumnIndex >= 0 And ((.CurrentCell.ColumnIndex <> 0 And Val(.CurrentRow.Cells(0).Value) = 0) Or (.CurrentCell.ColumnIndex = 0 And Val(dgtxt_Details.Text) = 0)) Then
                                For i = 0 To .Columns.Count - 1
                                    .Rows(.CurrentCell.RowIndex).Cells(i).Value = ""
                                Next
                                txt_Elogation.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                            End If

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 1 Then
                            If .CurrentCell.RowIndex = 0 Then
                                dgv_Wages_Details.Focus()
                                dgv_Wages_Details.CurrentCell = dgv_Wages_Details.Rows(0).Cells(1)

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

        If Val(no) = 0 Then Exit Sub

        clear()

        NoCalc_Status = True

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            da1 = New SqlClient.SqlDataAdapter("select a.* from Weaver_Wages_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Wages_Code = '" & Trim(NewCode) & "'", con)
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
                'txt_Bits_Cooly.Text = dt1.Rows(0).Item("Bits_Cooly").ToString
                'txt_Bits_Meter.Text = dt1.Rows(0).Item("Bits_Meters").ToString
                txt_Elogation.Text = dt1.Rows(0).Item("Elogation").ToString
                lbl_Excess_Short.Text = dt1.Rows(0).Item("Excess_Short").ToString
                txt_Folding_Less.Text = dt1.Rows(0).Item("Folding_Less").ToString

                txt_Freight_Charge.Text = dt1.Rows(0).Item("Freight_Charge").ToString
                txt_Less_Amount.Text = dt1.Rows(0).Item("Less_Amount").ToString
                lbl_Net_Amount.Text = dt1.Rows(0).Item("Net_Amount").ToString
                'txt_Other_Cooly.Text = dt1.Rows(0).Item("Others_Cooly").ToString
                'txt_Other_Meter.Text = dt1.Rows(0).Item("Others_Meters").ToString
                txt_Paid_Amount.Text = dt1.Rows(0).Item("Paid_Amount").ToString
                lbl_RecCode.Text = dt1.Rows(0).Item("Weaver_Cloth_Receipt_Code").ToString

                txt_Tds.Text = dt1.Rows(0).Item("Tds_Perc").ToString
                lbl_Tds_Amount.Text = dt1.Rows(0).Item("Tds_Perc_Calc").ToString
                lbl_Total_Amount.Text = dt1.Rows(0).Item("Assesable_Value").ToString

                lbl_WeaverBillNo.Text = dt1.Rows(0).Item("Weaver_BillNo").ToString
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))

                da2 = New SqlClient.SqlDataAdapter("Select a.* , b.Count_Name from Weaver_Wages_Yarn_Details a left outer join count_head b on a.Count_IdNo = b.Count_IdNo Where a.Weaver_Wages_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
                dt2 = New DataTable
                da2.Fill(dt2)

                With dgv_ConsYarn_Details

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

                            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then

                                .Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.00")

                            Else
                                .Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Weight").ToString), "########0.000")

                            End If
                          

                        Next i

                    End If

                    If .RowCount = 0 Then .Rows.Add()

                End With

                With dgv_ConsYarnDetails_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(0).Value = Format(Val(dt1.Rows(0).Item("Total_Dgv_Meters").ToString), "########0.00")

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then
                        .Rows(0).Cells(6).Value = Format(Val(dt1.Rows(0).Item("Total_Dgv_Weight").ToString), "########0.00")

                    Else
                        .Rows(0).Cells(6).Value = Format(Val(dt1.Rows(0).Item("Total_Dgv_Weight").ToString), "########0.000")

                    End If
                   
                End With
                dt2.Clear()

                da2 = New SqlClient.SqlDataAdapter("Select a.* , c.ClothType_Name from Weaver_Wages_Cooly_Details a LEFT OUTER JOIN ClothType_Head c ON a.ClothType_IdNo = c.ClothType_IdNo Where a.Weaver_Wages_Code = '" & Trim(NewCode) & "' Order by Sl_No", con)
                dt3 = New DataTable
                da2.Fill(dt3)

                With dgv_Wages_Details

                    .Rows.Clear()
                    SNo = 0

                    If dt3.Rows.Count > 0 Then

                        For i = 0 To dt3.Rows.Count - 1

                            n = .Rows.Add()
                            SNo = SNo + 1
                            .Rows(n).Cells(0).Value = Val(SNo)
                            .Rows(n).Cells(1).Value = Format(Val(dt3.Rows(i).Item("Meters").ToString), "########0.00")
                            .Rows(n).Cells(2).Value = dt3.Rows(i).Item("ClothType_Name").ToString
                            .Rows(n).Cells(3).Value = Format(Val(dt3.Rows(i).Item("Pick").ToString), "########0.00")
                            .Rows(n).Cells(4).Value = Format(Val(dt3.Rows(i).Item("Cooly").ToString), "########0.00")
                            .Rows(n).Cells(5).Value = Format(Val(dt3.Rows(i).Item("Amount").ToString), "########0.00")

                        Next i

                    End If

                    If .RowCount = 0 Then .Rows.Add()

                End With

                With dgv_WagesDetails_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(1).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                    .Rows(0).Cells(5).Value = Format(Val(dt1.Rows(0).Item("Total_Cooly").ToString), "########0.00")

                End With
                dt3.Clear()

                da2 = New SqlClient.SqlDataAdapter("Select a.*  from Weaver_Cloth_Receipt_Head a  Where a.Weaver_Wages_Code = '" & Trim(NewCode) & "' Order by fOR_oRDERbY , Weaver_ClothReceipt_No", con)
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
                            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then '---- Kalaimagal Textiles (Palladam)
                                .Rows(n).Cells(1).Value = dt4.Rows(i).Item("Weaver_ClothReceipt_No").ToString & " / " & dt4.Rows(i).Item("Lot_No").ToString
                            Else
                                .Rows(n).Cells(1).Value = dt4.Rows(i).Item("Weaver_ClothReceipt_No").ToString
                            End If
                            .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt4.Rows(i).Item("Weaver_ClothReceipt_Date").ToString), "dd-MM-yyyy")
                            .Rows(n).Cells(3).Value = dt4.Rows(i).Item("Party_DcNo").ToString
                            .Rows(n).Cells(4).Value = Format(Val(dt4.Rows(i).Item("Noof_Pcs").ToString), "########0")
                            If Val(dt4.Rows(i).Item("ReceiptMeters_Wages").ToString) <> 0 Then
                                .Rows(n).Cells(5).Value = Format(Val(dt4.Rows(i).Item("ReceiptMeters_Wages").ToString), "########0.000")
                            Else
                                .Rows(n).Cells(5).Value = Format(Val(dt4.Rows(i).Item("ReceiptMeters_Receipt").ToString), "########0.000")
                            End If

                            .Rows(n).Cells(6).Value = dt4.Rows(i).Item("Weaver_ClothReceipt_Code").ToString
                        Next i

                    End If

                    If .RowCount = 0 Then .Rows.Add()

                End With

                With dgv_ReceiptDetails_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(4).Value = Format(Val(dt1.Rows(0).Item("Pcs").ToString), "########0.00")
                    .Rows(0).Cells(5).Value = Format(Val(dt1.Rows(0).Item("Receipt_Meters").ToString), "########0.000")
                End With
                dt4.Clear()

            End If

            NoCalc_Status = False
            Calculation_Total_Wages()
            NoCalc_Status = True

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

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Weaver_Wages_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Weaver_Wages_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

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

            cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Weaver_Wages_Code = '', Weaver_Wages_Increment = Weaver_Wages_Increment - 1, Weaver_Wages_Date = Null, ReceiptMeters_Wages = 0, Receipt_Meters = (case when Weaver_Piece_Checking_Code <> '' then ReceiptMeters_Checking else ReceiptMeters_Receipt end), ConsumedYarn_Wages = 0, Consumed_Yarn = (case when Weaver_Piece_Checking_Code <> '' then ConsumedYarn_Checking else ConsumedYarn_Receipt end), ConsumedPavu_Wages = 0, Consumed_Pavu = (case when Weaver_Piece_Checking_Code <> '' then ConsumedPavu_Checking else ConsumedPavu_Receipt end) , Type1_Wages_Meters = 0, Type2_Wages_Meters = 0, Type3_Wages_Meters = 0, Type4_Wages_Meters = 0, Type5_Wages_Meters = 0, Total_Wages_Meters = 0, Report_Particulars_Wages = '', Report_Particulars = Report_Particulars_Receipt Where Weaver_Wages_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Stock_Yarn_Processing_Details set Reference_Date = b.Weaver_ClothReceipt_Date, Weight = (case when b.Weaver_Piece_Checking_Code <> '' then b.ConsumedYarn_Checking else b.ConsumedYarn_Receipt end) from Stock_Yarn_Processing_Details a, Weaver_Cloth_Receipt_Head b Where b.Weaver_Wages_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(PkCondition_WCLRC) & "' + b.Weaver_ClothReceipt_Code"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Stock_Pavu_Processing_Details set Reference_Date = b.Weaver_ClothReceipt_Date, Meters = (case when b.Weaver_Piece_Checking_Code <> '' then b.ConsumedPavu_Checking else b.ConsumedPavu_Receipt end) from Stock_Pavu_Processing_Details a, Weaver_Cloth_Receipt_Head b Where b.Weaver_Wages_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(PkCondition_WCLRC) & "' + b.Weaver_ClothReceipt_Code"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Update Stock_Cloth_Processing_Details set reference_date = b.Weaver_ClothReceipt_Date, UnChecked_Meters = b.ReceiptMeters_Receipt, Meters_Type1 = 0, Meters_Type2 = 0, Meters_Type3 = 0, Meters_Type4 = 0, Meters_Type5 = 0 from Stock_Cloth_Processing_Details a, Weaver_Cloth_Receipt_Head b Where b.Weaver_Wages_Code = '" & Trim(NewCode) & "' and b.Weaver_Piece_Checking_Code = '' and a.Reference_Code = '" & Trim(PkCondition_WCLRC) & "' + b.Weaver_ClothReceipt_Code"
            cmd.ExecuteNonQuery()

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), trans)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition_WPYMT) & Trim(NewCode), trans)
            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition_WFRGT) & Trim(NewCode), trans)

            'cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()
            'cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            'cmd.CommandText = "Delete from Voucher_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(PkCondition_WPYMT) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(PkCondition_WPYMT) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()
            'cmd.CommandText = "Delete from Voucher_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Voucher_Code = '" & Trim(PkCondition_WPYMT) & Trim(NewCode) & "' and Entry_Identification = '" & Trim(PkCondition_WPYMT) & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()

            'cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Weaver_Wages_Code = '', Weaver_Wages_Increment = Weaver_Wages_Increment - 1, Weaver_Wages_Date = Null, ReceiptMeters_Wages = 0, Receipt_Meters =  (case when Weaver_Piece_Checking_Code <> '' then ReceiptMeters_Checking else ReceiptMeters_Receipt end), ConsumedYarn_Wages = 0, Consumed_Yarn = (case when Weaver_Piece_Checking_Code <> '' then ConsumedYarn_Checking else ConsumedYarn_Receipt end), ConsumedPavu_Wages = 0, Consumed_Pavu = (case when Weaver_Piece_Checking_Code <> '' then ConsumedPavu_Checking else ConsumedPavu_Receipt end) , Type1_Wages_Meters = 0, Type2_Wages_Meters = 0, Type3_Wages_Meters = 0, Type4_Wages_Meters = 0, Type5_Wages_Meters = 0, Total_Wages_Meters = 0  Where Weaver_Wages_Code = '" & Trim(NewCode) & "'"
            'cmd.ExecuteNonQuery()
            cmd.CommandText = "update  Weaver_Cloth_Receipt_Head set Weaver_Wages_Code = '' where  Weaver_Wages_Code = '" & Trim(NewCode) & "'"
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

            If msk_Date.Enabled = True And msk_Date.Visible = True Then msk_Date.Focus()

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

            da = New SqlClient.SqlDataAdapter("select top 1 Weaver_Wages_No from Weaver_Wages_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "'  and Weaver_Wages_Code NOT LIKE '" & Trim(PkCondition_GST) & "%' Order by for_Orderby, Weaver_Wages_No", con)
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


        Catch ex As Exception
            MessageBox.Show(ex.Message, "FOR NEW RECORD...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            Dt1.Dispose()
            Da1.Dispose()

            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()

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

        If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.Weaver_Wages_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.Weaver_Wages_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OK, MessageBoxIcon.Error) : Exit Sub

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
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim clth_ID As Integer = 0
        Dim Wev_ID As Integer = 0
        Dim Endcnt_ID As Integer = 0
        Dim cunt_ID As Integer = 0
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

        Dim Lm_ID As Integer = 0
        Dim Wdth_Typ As String = ""
        Dim Nr As Integer = 0

        Dim ConsYarn As Single = 0
        Dim ConsPavu As Single = 0
        Dim SOUND_MTR As Single = 0
        Dim SECOND_MTR As Single = 0
        Dim BIT_MTR As Single = 0
        Dim REJECT_MTR As Single = 0
        Dim OTHER_MTR As Single = 0
        Dim vNoof_ReceiptCount As Integer = 0
        Dim CloTyp_ID As Integer = 0

        Dim vRecNo As String = ""
        Dim vRecPDcNo As String = ""
        Dim vRecDt As String = ""

        Dim ClthName As String = ""
        Dim Rep_Partcls_Wages As String = ""

        Dim DateColUpdt As String = ""

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Common_Procedures.UserRight_Check(Common_Procedures.UR.Weaver_Wages_Entry, New_Entry) = False Then Exit Sub

        If pnl_Back.Enabled = False Then
            MessageBox.Show("Close Other Windows", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If IsDate(msk_Date.Text) = False Then
            MessageBox.Show("Invalid Date", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
            Exit Sub
        End If

        If Not (Convert.ToDateTime(msk_Date.Text) >= Common_Procedures.Company_FromDate And Convert.ToDateTime(msk_Date.Text) <= Common_Procedures.Company_ToDate) Then
            MessageBox.Show("Date is out of financial range", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If msk_Date.Enabled And msk_Date.Visible Then msk_Date.Focus()
            Exit Sub
        End If

        Wev_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Weaver.Text)
        If Wev_ID = 0 Then
            MessageBox.Show("Invalid Weaver Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Weaver.Enabled Then cbo_Weaver.Focus()
            Exit Sub
        End If

        clth_ID = Common_Procedures.Cloth_NameToIdNo(con, lbl_Cloth.Text)
        If clth_ID = 0 Then
            MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Weaver.Enabled Then cbo_Weaver.Focus()
            Exit Sub
        End If

        Endcnt_ID = Common_Procedures.EndsCount_NameToIdNo(con, lbl_Ends_Count.Text)
        If Endcnt_ID = 0 Then
            MessageBox.Show("Invalid Ends/Count", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If cbo_Weaver.Enabled Then cbo_Weaver.Focus()
            Exit Sub
        End If

        lbl_UserName.Text = Common_Procedures.User.IdNo

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


        For i = 0 To dgv_ConsYarn_Details.RowCount - 1

            If Val(dgv_ConsYarn_Details.Rows(i).Cells(0).Value) <> 0 Or Val(dgv_ConsYarn_Details.Rows(i).Cells(6).Value) <> 0 Then

                cunt_ID = Common_Procedures.Count_NameToIdNo(con, dgv_ConsYarn_Details.Rows(i).Cells(1).Value)
                If clth_ID = 0 Then
                    MessageBox.Show("Invalid Count Name", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    If dgv_ConsYarn_Details.Enabled And dgv_ConsYarn_Details.Visible Then
                        dgv_ConsYarn_Details.Focus()
                        dgv_ConsYarn_Details.CurrentCell = dgv_ConsYarn_Details.Rows(i).Cells(1)
                    End If
                    Exit Sub
                End If

            End If

        Next


        NoCalc_Status = False

        Calculation_Total_ReceiptMeter()
        Calculation_Total_ConsumedYarnDetails()
        Calculation_Total_Wages()

        vTotConsYrnMtrs = 0 : vTotConsYrnWgt = 0
        If dgv_ConsYarnDetails_Total.RowCount > 0 Then
            vTotConsYrnMtrs = Val(dgv_ConsYarnDetails_Total.Rows(0).Cells(0).Value())
            vTotConsYrnWgt = Val(dgv_ConsYarnDetails_Total.Rows(0).Cells(6).Value())
        End If

        vTotWgsMtrs = 0 : vTotWgsGrsAmt = 0
        If dgv_WagesDetails_Total.RowCount > 0 Then
            vTotWgsMtrs = Val(dgv_WagesDetails_Total.Rows(0).Cells(1).Value())
            vTotWgsGrsAmt = Val(dgv_WagesDetails_Total.Rows(0).Cells(5).Value())
        End If

        vTotRcptMtrs = 0 : vTotRcptPcs = 0
        If dgv_ReceiptDetails_Total.RowCount > 0 Then
            vTotRcptPcs = Val(dgv_ReceiptDetails_Total.Rows(0).Cells(4).Value())
            vTotRcptMtrs = Val(dgv_ReceiptDetails_Total.Rows(0).Cells(5).Value())
        End If

        vNoof_ReceiptCount = 0
        If dgv_Receipt_Details.Rows.Count > 0 Then
            For I = 0 To dgv_Receipt_Details.Rows.Count - 1
                If Val(dgv_Receipt_Details.Rows(I).Cells(5).Value) <> 0 And dgv_Receipt_Details.Rows(I).Cells(6).Value <> "" Then
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
                lbl_RecCode.Text = dgv_Receipt_Details.Rows(0).Cells(6).Value
                vRecNo = dgv_Receipt_Details.Rows(0).Cells(1).Value
                vRecDt = dgv_Receipt_Details.Rows(0).Cells(2).Value
                vRecPDcNo = dgv_Receipt_Details.Rows(0).Cells(3).Value
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

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then
                    lbl_BillNo.Text = Common_Procedures.get_MaxCode(con, "Weaver_Wages_Head", "Weaver_Wages_Code", "For_OrderBy", "(Weaver_Wages_Code NOT LIKE '" & Trim(PkCondition_GST) & "%')", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)
                End If

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@WagesDate", dtp_Date.Value.Date)

            If New_Entry = True Then

                cmd.CommandText = "Insert into Weaver_Wages_Head (    Weaver_Wages_Code   ,               Company_IdNo       ,     Weaver_Wages_No  ,                     for_OrderBy                                                   ,  Weaver_Wages_Date,              Ledger_IdNo,      Weaver_Cloth_Receipt_Code    ,           Cloth_IdNo     ,    EndsCount_IdNo            ,            Rec_No     ,             Rec_Date   ,             P_Dc_No       ,                 Folding_Less           ,                 Freight_Charge           ,                 Paid_Amount           ,                  Excess_Short           ,                  Add_Amount           ,                  Tds_Perc      ,                  Tds_Perc_Calc        ,                  Elogation           ,                  Less_Amount           ,                  Assesable_Value        ,                       Net_Amount            ,               Total_Dgv_Meters    ,               Total_Dgv_Weight   ,              Total_Meters    ,               Total_Cooly       ,                 Pcs          ,               Receipt_Meters   ,               Weaver_BillNo          ,                                WeaverBillNo_ForOrderBy                          ,  user_idNo ) " & _
                                    "     Values                 ( '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_BillNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_BillNo.Text))) & ",     @WagesDate    , " & Str(Val(Wev_ID)) & ",  '" & Trim(lbl_RecCode.Text) & "' ,  " & Str(Val(clth_ID)) & ",  " & Str(Val(Endcnt_ID)) & ", '" & Trim(vRecNo) & "',  '" & Trim(vRecDt) & "',  '" & Trim(vRecPDcNo) & "', " & Str(Val(txt_Folding_Less.Text)) & ", " & Str(Val(txt_Freight_Charge.Text)) & ", " & Str(Val(txt_Paid_Amount.Text)) & ",  " & Str(Val(lbl_Excess_Short.Text)) & ",  " & Str(Val(txt_Add_Amount.Text)) & ",  " & Str(Val(txt_Tds.Text)) & ",  " & Str(Val(lbl_Tds_Amount.Text)) & ",  " & Str(Val(txt_Elogation.Text)) & ",  " & Str(Val(txt_Less_Amount.Text)) & ",  " & Str(Val(lbl_Total_Amount.Text)) & ",  " & Str(Val(CSng(lbl_Net_Amount.Text))) & ",  " & Str(Val(vTotConsYrnMtrs)) & ",  " & Str(Val(vTotConsYrnWgt)) & ", " & Str(Val(vTotWgsMtrs)) & ",  " & Str(Val(vTotWgsGrsAmt)) & ", " & Str(Val(vTotRcptPcs)) & ",  " & Str(Val(vTotRcptMtrs)) & ", '" & Trim(lbl_WeaverBillNo.Text) & "',  " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_WeaverBillNo.Text))) & " , " & Val(lbl_UserName.Text) & ") "
                cmd.ExecuteNonQuery()

            Else

                cmd.CommandText = "Update Weaver_Wages_Head set Weaver_Wages_Date = @WagesDate, Ledger_IdNo = " & Str(Val(Wev_ID)) & ", Weaver_Cloth_Receipt_Code = '" & Trim(lbl_RecCode.Text) & "', Cloth_IdNo = " & Str(Val(clth_ID)) & ", EndsCount_IdNo = " & Str(Val(Endcnt_ID)) & ", Rec_No = '" & Trim(vRecNo) & "',  Rec_Date  = '" & Trim(vRecDt) & "', P_Dc_No = '" & Trim(vRecPDcNo) & "', Folding_Less =  " & Str(Val(txt_Folding_Less.Text)) & ", Freight_Charge = " & Str(Val(txt_Freight_Charge.Text)) & ", Paid_Amount = " & Str(Val(txt_Paid_Amount.Text)) & ", Excess_Short = " & Str(Val(lbl_Excess_Short.Text)) & ", Add_Amount = " & Str(Val(txt_Add_Amount.Text)) & "  , Tds_Perc =  " & Str(Val(txt_Tds.Text)) & " , Tds_Perc_Calc =  " & Str(Val(lbl_Tds_Amount.Text)) & " ,   Elogation =  " & Str(Val(txt_Elogation.Text)) & " ,    Less_Amount =  " & Str(Val(txt_Less_Amount.Text)) & " , Assesable_Value = " & Str(Val(lbl_Total_Amount.Text)) & ", Net_Amount = " & Str(Val(CSng(lbl_Net_Amount.Text))) & ", Total_Dgv_Meters = " & Str(Val(vTotConsYrnMtrs)) & " ,    Total_Dgv_Weight = " & Str(Val(vTotConsYrnWgt)) & ", Total_Meters     = " & Str(Val(vTotWgsMtrs)) & "      ,   Total_Cooly = " & Str(Val(vTotWgsGrsAmt)) & ",  Pcs  =  " & Str(Val(vTotRcptPcs)) & "      ,     Receipt_Meters = " & Str(Val(vTotRcptMtrs)) & "   , Weaver_BillNo = '" & Trim(lbl_WeaverBillNo.Text) & "', WeaverBillNo_ForOrderBy = " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_WeaverBillNo.Text))) & ", User_IdNo = " & Val(lbl_UserName.Text) & " Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Stock_Yarn_Processing_Details set Reference_Date = b.Weaver_ClothReceipt_Date, Weight = (case When b.Weaver_Piece_Checking_Code <> '' then b.ConsumedYarn_Checking else b.ConsumedYarn_Receipt end) from Stock_Yarn_Processing_Details a, Weaver_Cloth_Receipt_Head b Where b.Weaver_Wages_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(PkCondition_WCLRC) & "' + b.Weaver_ClothReceipt_Code"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Stock_Pavu_Processing_Details set Reference_Date = b.Weaver_ClothReceipt_Date, Meters = (case When b.Weaver_Piece_Checking_Code <> '' then b.ConsumedPavu_Checking else b.ConsumedPavu_Receipt end) from Stock_Pavu_Processing_Details a, Weaver_Cloth_Receipt_Head b Where b.Weaver_Wages_Code = '" & Trim(NewCode) & "' and a.Reference_Code = '" & Trim(PkCondition_WCLRC) & "' + b.Weaver_ClothReceipt_Code"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Stock_Cloth_Processing_Details set reference_date = b.Weaver_ClothReceipt_Date, UnChecked_Meters = b.ReceiptMeters_Receipt, Meters_Type1 = 0, Meters_Type2 = 0, Meters_Type3 = 0, Meters_Type4 = 0, Meters_Type5 = 0 from Stock_Cloth_Processing_Details a, Weaver_Cloth_Receipt_Head b Where b.Weaver_Wages_Code = '" & Trim(NewCode) & "' and b.Weaver_Piece_Checking_Code = '' and a.Reference_Code = '" & Trim(PkCondition_WCLRC) & "' + b.Weaver_ClothReceipt_Code"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Weaver_Wages_Code = '', Weaver_Wages_Increment = Weaver_Wages_Increment - 1, Weaver_Wages_Date = Null, ReceiptMeters_Wages = 0, Receipt_Meters = (case when Weaver_Piece_Checking_Code <> '' then ReceiptMeters_Checking else ReceiptMeters_Receipt end), ConsumedYarn_Wages = 0, Consumed_Yarn = (case when Weaver_Piece_Checking_Code <> '' then ConsumedYarn_Checking else ConsumedYarn_Receipt end), ConsumedPavu_Wages = 0, Consumed_Pavu = (case when Weaver_Piece_Checking_Code <> '' then ConsumedPavu_Checking else ConsumedPavu_Receipt end) , Type1_Wages_Meters = 0, Type2_Wages_Meters = 0, Type3_Wages_Meters = 0, Type4_Wages_Meters = 0, Type5_Wages_Meters = 0, Total_Wages_Meters = 0, Report_Particulars_Wages = '', Report_Particulars = Report_Particulars_Receipt Where Weaver_Wages_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

            End If

            EntID = Trim(Pk_Condition) & Trim(lbl_BillNo.Text)
            PBlNo = Trim(lbl_BillNo.Text)
            Partcls = "Wages : Bill.No. " & Trim(lbl_BillNo.Text)


            cmd.CommandText = "Delete from Weaver_Wages_Yarn_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Weaver_Wages_Cooly_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_Wages_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Receipt_Details

                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(5).Value) <> 0 Then

                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then
                            ClthName = Microsoft.VisualBasic.Left(lbl_Cloth.Text, 10)
                            Rep_Partcls_Wages = "CloRcpt :" & Trim(ClthName) & " L.No." & Trim(.Rows(i).Cells(1).Value)
                        Else
                            Rep_Partcls_Wages = "CloRcpt : LotNo. " & Trim(.Rows(i).Cells(1).Value)
                            If Trim(.Rows(i).Cells(3).Value) <> "" Then
                                Rep_Partcls_Wages = Trim(Rep_Partcls_Wages) & ",  P.Dc.No : " & Trim(.Rows(i).Cells(3).Value)
                            End If
                            Rep_Partcls_Wages = Trim(Rep_Partcls_Wages) & ", Bill.No. " & Trim(lbl_BillNo.Text)

                        End If


                        cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set Weaver_Wages_Code = '" & Trim(NewCode) & "', Weaver_Wages_Increment = Weaver_Wages_Increment + 1, Weaver_Wages_Date = @WagesDate, noof_pcs = " & Str(Val(.Rows(i).Cells(4).Value)) & ", ReceiptMeters_Wages = " & Str(Val(Val(.Rows(i).Cells(5).Value))) & ", Receipt_Meters = " & Str(Val(.Rows(i).Cells(5).Value)) & ", Report_Particulars_Wages = '" & Trim(Rep_Partcls_Wages) & "', Report_Particulars = '" & Trim(Rep_Partcls_Wages) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(dgv_Receipt_Details.Rows(i).Cells(6).Value) & "'"
                        Nr = cmd.ExecuteNonQuery()

                    End If

                Next

            End With

            With dgv_Wages_Details

                Sno = 0
                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(1).Value) <> 0 Then

                        Sno = Sno + 1
                        clthtyp_ID = Common_Procedures.ClothType_NameToIdNo(con, .Rows(i).Cells(2).Value, tr)

                        cmd.CommandText = "Insert into Weaver_Wages_Cooly_Details (       Weaver_Wages_Code  ,             Company_IdNo         ,             Weaver_Wages_No    ,                               for_OrderBy                               , Weaver_Wages_Date,            Sl_No     ,                      Meters              ,           ClothType_IdNo    ,       Pick                                ,           Cooly                         ,                     Amount        ) " & _
                                            "     Values                         (   '" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_BillNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_BillNo.Text))) & ",       @WagesDate , " & Str(Val(Sno)) & ", " & Str(Val(.Rows(i).Cells(1).Value)) & ",  " & Str(Val(clthtyp_ID)) & ",  " & Str(Val(.Rows(i).Cells(3).Value)) & "," & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & " ) "
                        cmd.ExecuteNonQuery()

                    End If

                Next

            End With

            With dgv_ConsYarn_Details

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

            If vNoof_ReceiptCount = 1 And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1037" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1059" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1087" Then

                SOUND_MTR = 0
                SECOND_MTR = 0
                BIT_MTR = 0
                OTHER_MTR = 0
                REJECT_MTR = 0
                If dgv_Wages_Details.RowCount > 0 Then
                    For I = 0 To dgv_Wages_Details.RowCount - 1
                        CloTyp_ID = Common_Procedures.ClothType_NameToIdNo(con, dgv_Wages_Details.Rows(I).Cells(2).Value, tr)
                        If Val(CloTyp_ID) = 1 Then
                            SOUND_MTR = SOUND_MTR + dgv_Wages_Details.Rows(I).Cells(1).Value
                        ElseIf Val(CloTyp_ID) = 2 Then
                            SECOND_MTR = SECOND_MTR + dgv_Wages_Details.Rows(I).Cells(1).Value
                        ElseIf Val(CloTyp_ID) = 3 Then
                            BIT_MTR = BIT_MTR + dgv_Wages_Details.Rows(I).Cells(1).Value
                        ElseIf Val(CloTyp_ID) = 4 Then
                            OTHER_MTR = OTHER_MTR + dgv_Wages_Details.Rows(I).Cells(1).Value
                        ElseIf Val(CloTyp_ID) = 5 Then
                            REJECT_MTR = REJECT_MTR + dgv_Wages_Details.Rows(I).Cells(1).Value
                        End If
                    Next
                End If

                ConsYarn = Val(vTotConsYrnWgt)
                ConsPavu = Val(Common_Procedures.get_Pavu_Consumption(con, clth_ID, Lm_ID, Val(vTotRcptMtrs), Trim(Wdth_Typ), tr))

                cmd.CommandText = "Update Weaver_Cloth_Receipt_Head set ConsumedYarn_Wages = " & Str(Val(ConsYarn)) & ", Consumed_Yarn = " & Str(Val(ConsYarn)) & ", ConsumedPavu_Wages = " & Str(Val(ConsPavu)) & ", Consumed_Pavu = " & Str(Val(ConsPavu)) & ", Type1_Wages_Meters = " & Str(Val(SOUND_MTR)) & ", Type2_Wages_Meters = " & Str(Val(SECOND_MTR)) & ", Type3_Wages_Meters = " & Str(Val(BIT_MTR)) & ", Type4_Wages_Meters = " & Str(Val(OTHER_MTR)) & ", Type5_Wages_Meters = " & Str(Val(REJECT_MTR)) & ", Total_Wages_Meters = " & Str(Val(vTotWgsMtrs)) & " Where Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "'"
                cmd.ExecuteNonQuery()

                DateColUpdt = ""
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then '---- Ganesh karthick Textiles (p) Ltd (Somanur)
                    DateColUpdt = "Reference_Date = @WagesDate, "
                End If

                Dim vSTK_REFCODE As String = ""

                If InStr(1, Trim(UCase(lbl_RecCode.Text)), "CRCHK-") > 0 Then
                    vSTK_REFCODE = Trim(lbl_RecCode.Text)
                Else
                    vSTK_REFCODE = Trim(PkCondition_WCLRC) & Trim(lbl_RecCode.Text)
                End If

                cmd.CommandText = "Update Stock_Yarn_Processing_Details set " & DateColUpdt & " Weight = " & Str(Val(ConsYarn)) & " Where Reference_Code = '" & Trim(vSTK_REFCODE) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Update Stock_Pavu_Processing_Details set " & DateColUpdt & " Meters = " & Str(Val(ConsPavu)) & " Where Reference_Code = '" & Trim(vSTK_REFCODE) & "'"
                cmd.ExecuteNonQuery()

                If Trim(PcsChkCode) = "" Then
                    cmd.CommandText = "Update Stock_Cloth_Processing_Details set reference_date = @WagesDate, UnChecked_Meters = 0, Meters_Type1 = " & Str(Val(SOUND_MTR)) & ", Meters_Type2 = " & Str(Val(SECOND_MTR)) & ", Meters_Type3 = " & Str(Val(BIT_MTR)) & ", Meters_Type4 = " & Str(Val(REJECT_MTR)) & ", Meters_Type5 = " & Str(Val(OTHER_MTR)) & " Where Reference_Code = '" & Trim(PkCondition_WCLRC) & Trim(lbl_RecCode.Text) & "'"
                    cmd.ExecuteNonQuery()
                End If

            End If


            Cr_ID = Wev_ID
            Dr_ID = Common_Procedures.CommonLedger.Weaving_Wages_Ac
            TdsAc_ID = Common_Procedures.CommonLedger.TDS_Payable_Ac

            Dim vLed_IdNos As String = "", vVou_Amts As String = "", ErrMsg As String = ""

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1029" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1283" Then '---- Arul Kumaran Textiles (Somanur)
                vLed_IdNos = Wev_ID & "|" & Val(Common_Procedures.CommonLedger.Weaving_Wages_Ac) & "|" & Val(Common_Procedures.CommonLedger.TDS_Payable_Ac)
                vVou_Amts = Val(CSng(lbl_Net_Amount.Text) + Val(txt_Freight_Charge.Text)) & "|" & -1 * (Val(CSng(lbl_Net_Amount.Text) + Val(txt_Freight_Charge.Text)) - Val(lbl_Tds_Amount.Text)) & "|" & -1 * Val(lbl_Tds_Amount.Text)

            Else

                vLed_IdNos = Wev_ID & "|" & Val(Common_Procedures.CommonLedger.Weaving_Wages_Ac) & "|" & Val(Common_Procedures.CommonLedger.TDS_Payable_Ac)
                vVou_Amts = Val(CSng(lbl_Net_Amount.Text)) & "|" & -1 * (Val(CSng(lbl_Net_Amount.Text)) - Val(lbl_Tds_Amount.Text)) & "|" & -1 * Val(lbl_Tds_Amount.Text)

            End If

            If Common_Procedures.Voucher_Updation(con, "Wea.Wages", Val(lbl_Company.Tag), Trim(Pk_Condition) & Trim(NewCode), Trim(lbl_BillNo.Text), msk_Date.Text, "Bill No : " & Trim(lbl_BillNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                Throw New ApplicationException(ErrMsg)
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition_WPYMT) & Trim(NewCode), tr)
            If Val(txt_Paid_Amount.Text) <> 0 Then
                vLed_IdNos = Val(Common_Procedures.CommonLedger.Cash_Ac) & "|" & Val(Wev_ID)
                vVou_Amts = Val(txt_Paid_Amount.Text) & "|" & -1 * Val(txt_Paid_Amount.Text)
                If Common_Procedures.Voucher_Updation(con, "Wea.Pymt", Val(lbl_Company.Tag), Trim(PkCondition_WPYMT) & Trim(NewCode), Trim(lbl_BillNo.Text), msk_Date.Text, "Bill No : " & Trim(lbl_BillNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                    Throw New ApplicationException(ErrMsg)
                End If
            End If

            Common_Procedures.Voucher_Deletion(con, Val(lbl_Company.Tag), Trim(PkCondition_WFRGT) & Trim(NewCode), tr)
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1029" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1283" Then '---- Arul Kumaran Textiles (Somanur)
                If Val(txt_Freight_Charge.Text) <> 0 Then
                    vLed_IdNos = Val(Wev_ID) & "|" & Val(Common_Procedures.CommonLedger.Freight_Charges_Ac)
                    vVou_Amts = -1 * Val(txt_Freight_Charge.Text) & "|" & Val(txt_Freight_Charge.Text)
                    If Common_Procedures.Voucher_Updation(con, "WeaWages.Fregt", Val(lbl_Company.Tag), Trim(PkCondition_WFRGT) & Trim(NewCode), Trim(lbl_BillNo.Text), msk_Date.Text, "Bill No : " & Trim(lbl_BillNo.Text), vLed_IdNos, vVou_Amts, ErrMsg, tr, Common_Procedures.SoftwareTypes.Textile_Software) = False Then
                        Throw New ApplicationException(ErrMsg)
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

            If msk_Date.Visible And msk_Date.Enabled Then msk_Date.Focus()

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
        Dim tlamt As Double = 0
        Dim tdsamt As Double = 0
        Dim Totamt As Double = 0

        If NoCalc_Status = True Then Exit Sub

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1035" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1098" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1088" Then
            Totamt = 0
            If dgv_WagesDetails_Total.Rows.Count > 0 Then
                Totamt = Val(dgv_WagesDetails_Total.Rows(0).Cells(4).Value)
            End If
            tdsamt = Format(Val(Totamt) * Val(txt_Tds.Text) / 100, "########0")

        Else
            tdsamt = Format(Val(lbl_Total_Amount.Text) * Val(txt_Tds.Text) / 100, "########0")

        End If

        lbl_Tds_Amount.Text = Format(Val(tdsamt), "########0.00")

        NetAmount_Calculation()

    End Sub

    Private Sub Weight_Calculation(ByVal CurRow As Integer, ByVal CurCol As Integer)
        Dim ConsYrn As Double = 0
        Dim vClo_Mtrs As Single = 0
        Dim Wgt_Mtr As Double = 0
        Dim RdSp As Single = 0
        Dim Pick As Single = 0
        Dim Weft As Single = 0

        On Error Resume Next

        If NoCalc_Status = True Then Exit Sub

        With dgv_ConsYarn_Details
            If .Visible Then

                If CurCol = 0 Or CurCol = 1 Or CurCol = 2 Or CurCol = 3 Or CurCol = 4 Or CurCol = 5 Then

                    vClo_Mtrs = Val(.Rows(CurRow).Cells(0).Value)

                    Wgt_Mtr = Val(.Rows(CurRow).Cells(5).Value)

                    If Val(Wgt_Mtr) <> 0 Then
                        ConsYrn = Val(vClo_Mtrs) * Val(Wgt_Mtr)
                    Else
                        RdSp = Val(.Rows(CurRow).Cells(2).Value)
                        Pick = Val(.Rows(CurRow).Cells(3).Value)
                        Weft = Val(Common_Procedures.get_FieldValue(con, "count_head", "Resultant_Count", "(count_name = '" & Trim(.Rows(CurRow).Cells(1).Value) & "')"))
                        If Val(Weft) = 0 Then
                            Weft = Val(.Rows(CurRow).Cells(1).Value)
                        End If

                        ConsYrn = (vClo_Mtrs * RdSp * Pick * 1.0937) / (84 * 22 * Weft)

                    End If

                    If Trim(Common_Procedures.settings.CustomerCode) = "1009" Or Trim(Common_Procedures.settings.CustomerCode) = "1032" Or Trim(Common_Procedures.settings.CustomerCode) = "1060" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1065" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1090" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1283" Then
                        ConsYrn = Format(Val(ConsYrn), "#########0.0")
                        .Rows(CurRow).Cells(6).Value = Format(Val(ConsYrn), "#########0.000")

                    ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then

                        .Rows(CurRow).Cells(6).Value = Format(Val(ConsYrn), "#########0.00")

                    Else
                        .Rows(CurRow).Cells(6).Value = Format(Val(ConsYrn), "#########0.000")

                    End If

                End If

                ' Calculation_Total_ReceiptMeter()
                Calculation_Total_ConsumedYarnDetails()

            End If

        End With
    End Sub

    Private Sub NetAmount_Calculation()

        Dim NtAmt As Single

        If NoCalc_Status = True Then Exit Sub

        NtAmt = 0

        NtAmt = Val(lbl_Total_Amount.Text) - Val(lbl_Tds_Amount.Text)

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
        Dim tlamt As Single = 0

        If NoCalc_Status = True Then Exit Sub

        tlamt = Val(lbl_Cooly_amt.Text) - Val(txt_Freight_Charge.Text) + Val(txt_Add_Amount.Text) - Val(txt_Less_Amount.Text)

        lbl_Total_Amount.Text = Val(tlamt)

        TdsCommision_Calculation()

        NetAmount_Calculation()

    End Sub

    Private Sub Calculation_Total_ConsumedYarnDetails()
        Dim TotMtrs As Single
        Dim TotWgt As Single
        Dim tlmtr As Single = 0
        Dim Total_Meter As Single = 0

        If NoCalc_Status = True Then Exit Sub

        TotMtrs = 0 : TotWgt = 0

        With dgv_ConsYarn_Details
            For i = 0 To .RowCount - 1
                If Val(.Rows(i).Cells(6).Value) <> 0 Then

                    TotMtrs = TotMtrs + Val(.Rows(i).Cells(0).Value())
                    TotWgt = TotWgt + Val(.Rows(i).Cells(6).Value())

                End If
            Next i

        End With

        With dgv_ConsYarnDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(0).Value = Format(Val(TotMtrs), "########0.00")
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then
                TotWgt = Format(Val(TotWgt), "########0.00")
                .Rows(0).Cells(6).Value = Format(Val(TotWgt), "########0.00")

            Else
                .Rows(0).Cells(6).Value = Format(Val(TotWgt), "########0.000")

            End If

        End With


    End Sub

    Private Sub Calculation_Total_Wages()
        Dim Totamt As Single
        Dim TotWgsMtrs As Single
        Dim TotRcMtrs As Single
        Dim TotRcPcs As Single
        Dim tlmtr As Single = 0
        Dim TtConsMtrs As Single = 0
        Dim Total_Meter As Single = 0

        If NoCalc_Status = True Then Exit Sub

        TotWgsMtrs = 0 : Totamt = 0
      

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1029" Then
            With dgv_Wages_Details
                For i = 0 To .RowCount - 1
                    If Val(.Rows(i).Cells(1).Value) <> 0 Then
                        If Val(Common_Procedures.ClothType_NameToIdNo(con, Trim(.Rows(i).Cells(2).Value))) = 1 Then
                            TotWgsMtrs = TotWgsMtrs + Val(.Rows(i).Cells(1).Value())
                        End If
                        Totamt = Totamt + Val(.Rows(i).Cells(5).Value())
                    End If
                Next i

            End With
        Else
            With dgv_Wages_Details
                For i = 0 To .RowCount - 1
                    If Val(.Rows(i).Cells(1).Value) <> 0 Then

                        TotWgsMtrs = TotWgsMtrs + Val(.Rows(i).Cells(1).Value())
                        Totamt = Totamt + Val(.Rows(i).Cells(5).Value())

                    End If
                Next i

            End With

        End If


        With dgv_WagesDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(1).Value = Format(Val(TotWgsMtrs), "########0.00")
            .Rows(0).Cells(5).Value = Format(Val(Totamt), "########0.00")
        End With

        lbl_Cooly_amt.Text = Format(Val(Totamt), "########0.00")

        TotRcMtrs = 0 : TotRcPcs = 0
        With dgv_Receipt_Details
            For i = 0 To .RowCount - 1
                If Val(.Rows(i).Cells(5).Value) <> 0 Then

                    TotRcPcs = TotRcPcs + Val(.Rows(i).Cells(4).Value())
                    TotRcMtrs = TotRcMtrs + Val(.Rows(i).Cells(5).Value())

                End If
            Next i

        End With

        With dgv_ReceiptDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(4).Value = Format(Val(TotRcPcs), "########0.00")
            .Rows(0).Cells(5).Value = Format(Val(TotRcMtrs), "########0.000")
        End With

        lbl_Excess_Short.Text = Val(Val(TotWgsMtrs) - Val(TotRcMtrs))
        If Val(TotRcMtrs) > 0 Then
            txt_Elogation.Text = Format(Val(lbl_Excess_Short.Text) / Val(TotRcMtrs) * 100, "#########0.00")
        Else
            txt_Elogation.Text = ""
        End If

        TtConsMtrs = 0

        With dgv_ConsYarnDetails_Total
            If .Rows.Count > 0 Then
                TtConsMtrs = .Rows(0).Cells(0).Value
            End If
        End With

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1035" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1060" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1065" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1090" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1283" Then

            Total_Meter = 0
            If dgv_WagesDetails_Total.RowCount >= 1 Then
                Total_Meter = dgv_WagesDetails_Total.Rows(0).Cells(1).Value
            End If
            With dgv_ConsYarn_Details

                If .Rows.Count = 1 Then

                    .Rows(0).Cells(0).Value = Format(Val(Total_Meter), "##########0.00")

                Else

                    If TtConsMtrs = 0 Or TtConsMtrs = Val(.Rows(0).Cells(0).Value) Then
                        .Rows(0).Cells(0).Value = Format(Val(Total_Meter), "##########0.00")

                    End If

                End If

            End With

        End If

        Total_Amount_Calculation()
        'Excess_Short_Calculation()
        TdsCommision_Calculation()

    End Sub

    Private Sub Calculation_Total_ReceiptMeter()
        Dim TotRcMtrs As Single
        Dim TotRcPcs As Single
        Dim tlmtr As Single = 0
        Dim TtConsMtrs As Single = 0
        Dim Total_Meter As Single = 0

        If NoCalc_Status = True Then Exit Sub

        TotRcMtrs = 0 : TotRcPcs = 0
        With dgv_Receipt_Details
            For i = 0 To .RowCount - 1
                If Val(.Rows(i).Cells(5).Value) <> 0 Then

                    TotRcPcs = TotRcPcs + Val(.Rows(i).Cells(4).Value())
                    TotRcMtrs = TotRcMtrs + Val(.Rows(i).Cells(5).Value())

                End If
            Next i

        End With

        With dgv_ReceiptDetails_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(4).Value = Format(Val(TotRcPcs), "########0.00")
            .Rows(0).Cells(5).Value = Format(Val(TotRcMtrs), "########0.000")
        End With

    End Sub

    Private Sub cbo_Weaver_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Weaver.GotFocus
        cbo_Weaver.Tag = cbo_Weaver.Text
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_type = 'WEAVER'  or Ledger_Type = 'GODOWN'  ) and Close_status = 0", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Weaver_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Weaver.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Weaver, msk_Date, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_type = 'WEAVER'  or Ledger_Type = 'GODOWN' ) and Close_status = 0", "(Ledger_idno = 0)")
    End Sub

    Private Sub cbo_Weaver_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Weaver.KeyPress
        Dim q As Single = 0
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt As New DataTable
        Dim frt_Lm, Frt_Amt, Tds_Perc As Single
        Dim LedID, NoofLm As Integer
        Dim MxId As Long = 0

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Weaver, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "(ledger_type = 'WEAVER'  or Ledger_Type = 'GODOWN' ) and Close_status = 0", "(Ledger_idno = 0)")

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
                If dgv_Wages_Details.Rows.Count > 0 Then
                    dgv_Wages_Details.Focus()
                    dgv_Wages_Details.CurrentCell = dgv_Wages_Details.Rows(0).Cells(1)
                Else
                    txt_Elogation.Focus()
                End If

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

    Private Sub dgv_Details_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_ConsYarn_Details.CellEndEdit
        dgv_Details_CellLeave(sender, e)
    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_ConsYarn_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim rect As Rectangle
        Dim Tot As Single = 0

        With dgv_ConsYarn_Details
            dgv_ActCtrlName = .Name
            If (Val(.CurrentRow.Cells(0).Value) = 0 And .CurrentRow.Index = 0) Then
                If dgv_WagesDetails_Total.RowCount > 0 Then
                    .Rows(0).Cells(0).Value = Val(dgv_WagesDetails_Total.Rows(0).Cells(1).Value)
                End If
            End If

            If e.ColumnIndex = 0 Then
                If e.RowIndex > 0 Then

                    Tot = 0
                    For I = 0 To dgv_ConsYarn_Details.Rows.Count - 1
                        Tot = Tot + Val(dgv_ConsYarn_Details.Rows(I).Cells(0).Value)
                    Next
                    'If Val(dgv_Details.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) = 0 And (Val(lbl_Total_Meter.Text) > Val(Tot)) Then
                    '    dgv_Details.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Format(Val(lbl_Total_Meter.Text) - Val(Tot), "#########0.00")
                    'End If

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

    Private Sub dgv_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_ConsYarn_Details.CellLeave
        With dgv_ConsYarn_Details
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
                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then
                    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                    Else
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                    End If
                Else
                    If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.000")
                    Else
                        .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                    End If
                End If


            End If

        End With
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_ConsYarn_Details.CellValueChanged

        On Error Resume Next


        If IsNothing(dgv_ConsYarn_Details.CurrentCell) Then Exit Sub
        With dgv_ConsYarn_Details
            If .Visible Then

                If .CurrentCell.ColumnIndex = 0 Or .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
                    ' Excess_Short_Calculation()
                    Weight_Calculation(.CurrentCell.RowIndex, .CurrentCell.ColumnIndex)
                End If

                If .CurrentCell.ColumnIndex = 0 Or .CurrentCell.ColumnIndex = 6 Then
                    Calculation_Total_ConsumedYarnDetails()
                End If

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

    Private Sub dgv_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_ConsYarn_Details.KeyDown
        With dgv_ConsYarn_Details

            If e.KeyCode = Keys.Left Then
                If .CurrentCell.ColumnIndex <= 0 Then
                    If .CurrentCell.RowIndex = 0 Then
                        ' txt_Other_Cooly.Focus()
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

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_ConsYarn_Details.KeyUp
        Dim i As Integer
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_ConsYarn_Details

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

    Private Sub dgv_ConsYarn_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_ConsYarn_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_ConsYarn_Details.CurrentCell) Then dgv_ConsYarn_Details.CurrentCell.Selected = False
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

    Private Sub txt_Bits_Meter_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_Elogation_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Elogation.KeyDown
        If e.KeyValue = 38 Then
            If dgv_ConsYarn_Details.Rows.Count > 0 Then
                dgv_ConsYarn_Details.Focus()
                dgv_ConsYarn_Details.CurrentCell = dgv_ConsYarn_Details.Rows(0).Cells(0)
                dgv_ConsYarn_Details.CurrentCell.Selected = True

            ElseIf dgv_Wages_Details.Rows.Count > 0 Then
                dgv_Wages_Details.Focus()
                dgv_Wages_Details.CurrentCell = dgv_Wages_Details.Rows(0).Cells(1)
                dgv_Wages_Details.CurrentCell.Selected = True

            Else
                cbo_Weaver.Focus()

            End If
        End If
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_Elogation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Elogation.KeyPress
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
                    txt_Elogation.Focus()
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

                If IsNothing(dgv_ConsYarn_Details.CurrentCell) Then Exit Sub
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
        TdsCommision_Calculation()
    End Sub

    Private Sub txt_Total_Meter_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        TdsCommision_Calculation()
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
        Dim NewCode As String = ""
        Dim PcsChk_Condt As String = ""
        Dim Fldng As Single = 0

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
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then '---- Kalaimagal Textiles (Palladam)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Weaver_ClothReceipt_No").ToString & " / " & Dt1.Rows(i).Item("Lot_No").ToString
                    Else
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Weaver_ClothReceipt_No").ToString
                    End If
                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Weaver_ClothReceipt_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Party_DcNo").ToString
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("EndsCount_Name").ToString
                    .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("noof_pcs").ToString)
                    If Val(Dt1.Rows(i).Item("ReceiptMeters_Wages").ToString) <> 0 Then
                        .Rows(n).Cells(7).Value = Format(Val(Dt1.Rows(i).Item("ReceiptMeters_Wages").ToString), "########0.000")
                    Else
                        .Rows(n).Cells(7).Value = Format(Val(Dt1.Rows(i).Item("ReceiptMeters_Receipt").ToString), "########0.000")
                    End If

                    '  .Rows(n).Cells(7).Value = Format(Val(Dt1.Rows(i).Item("Receipt_Meters").ToString), "#########0.00")

                    .Rows(n).Cells(8).Value = "1"
                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Weaver_ClothReceipt_Code").ToString
                    .Rows(n).Cells(10).Value = Val(Fldng)

                    '.Rows(n).Cells(11).Value = Val(Dt1.Rows(i).Item("Sound_Meters").ToString)
                    '.Rows(n).Cells(12).Value = Val(Dt1.Rows(i).Item("Seconds_Meters").ToString)
                    '.Rows(n).Cells(13).Value = Val(Dt1.Rows(i).Item("Bits_Meters").ToString)
                    '.Rows(n).Cells(14).Value = Val(Dt1.Rows(i).Item("Reject_Meters").ToString)
                    '.Rows(n).Cells(15).Value = Val(Dt1.Rows(i).Item("Others_Meters").ToString)
                    '.Rows(n).Cells(16).Value = Val(Dt1.Rows(i).Item("Sound_Cooly").ToString)
                    '.Rows(n).Cells(17).Value = Val(Dt1.Rows(i).Item("Seconds_Cooly").ToString)
                    '.Rows(n).Cells(18).Value = Val(Dt1.Rows(i).Item("Bits_Cooly").ToString)
                    '.Rows(n).Cells(19).Value = Val(Dt1.Rows(i).Item("Reject_Cooly").ToString)
                    '.Rows(n).Cells(20).Value = Val(Dt1.Rows(i).Item("Others_Cooly").ToString)

                    For j = 0 To .ColumnCount - 1
                        .Rows(i).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next

            End If
            Dt1.Clear()

            PcsChk_Condt = ""
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1059" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
                PcsChk_Condt = "(Weaver_Piece_Checking_Code <> '')"
            End If

            Da = New SqlClient.SqlDataAdapter("select a.*, c.*, d.EndsCount_Name from Weaver_Cloth_Receipt_Head a INNER JOIN Cloth_Head c ON a.Cloth_IdNo = c.Cloth_IdNo LEFT OUTER JOIN EndsCount_Head d ON a.EndsCount_IdNo = d.EndsCount_IdNo Where " & Trim(PcsChk_Condt) & IIf(Trim(PcsChk_Condt) <> "", " and ", "") & " a.company_idno = " & Str(Val(lbl_Company.Tag)) & " and a.Weaver_Wages_Code = '' and a.ledger_Idno = " & Str(Val(LedIdNo)) & " order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    Fldng = Val(Dt1.Rows(i).Item("folding").ToString)
                    If Val(Fldng) = 0 Then Fldng = 100

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then '---- Kalaimagal Textiles (Palladam)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Weaver_ClothReceipt_No").ToString & " / " & Dt1.Rows(i).Item("Lot_No").ToString
                    Else
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Weaver_ClothReceipt_No").ToString
                    End If

                    .Rows(n).Cells(2).Value = Format(Convert.ToDateTime(Dt1.Rows(i).Item("Weaver_ClothReceipt_Date").ToString), "dd-MM-yyyy")
                    .Rows(n).Cells(3).Value = Dt1.Rows(i).Item("Party_DcNo").ToString
                    .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Cloth_Name").ToString
                    .Rows(n).Cells(5).Value = Dt1.Rows(i).Item("EndsCount_Name").ToString
                    .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("noof_pcs").ToString)

                    If Val(Dt1.Rows(i).Item("Receipt_Meters").ToString) <> 0 Then
                        .Rows(n).Cells(7).Value = Format(Val(Dt1.Rows(i).Item("Receipt_Meters").ToString), "########0.000")
                    Else
                        .Rows(n).Cells(7).Value = Format(Val(Dt1.Rows(i).Item("ReceiptMeters_Receipt").ToString), "########0.000")
                    End If

                    ' .Rows(n).Cells(7).Value = Format(Val(Dt1.Rows(i).Item("Receipt_Meters").ToString), "#########0.00")

                    .Rows(n).Cells(8).Value = ""
                    .Rows(n).Cells(9).Value = Dt1.Rows(i).Item("Weaver_ClothReceipt_Code").ToString
                    .Rows(n).Cells(10).Value = Val(Fldng)

                    '.Rows(n).Cells(11).Value = Format(Val(Dt1.Rows(i).Item("Type1_Checking_Meters").ToString) * Val(Fldng) / 100, "#########0")
                    '.Rows(n).Cells(12).Value = Format(Val(Dt1.Rows(i).Item("Type2_Checking_Meters").ToString) * Val(Fldng) / 100, "#########0")
                    '.Rows(n).Cells(13).Value = Format(Val(Dt1.Rows(i).Item("Type3_Checking_Meters").ToString) * Val(Fldng) / 100, "#########0")
                    '.Rows(n).Cells(14).Value = Format(Val(Dt1.Rows(i).Item("Type4_Checking_Meters").ToString) * Val(Fldng) / 100, "#########0")
                    '.Rows(n).Cells(15).Value = Format(Val(Dt1.Rows(i).Item("Type5_Checking_Meters").ToString) * Val(Fldng) / 100, "#########0")
                    '.Rows(n).Cells(16).Value = Val(Dt1.Rows(i).Item("Wages_For_Type1").ToString)
                    '.Rows(n).Cells(17).Value = Val(Dt1.Rows(i).Item("Wages_For_Type2").ToString)
                    '.Rows(n).Cells(18).Value = Val(Dt1.Rows(i).Item("Wages_For_Type3").ToString)
                    '.Rows(n).Cells(19).Value = Val(Dt1.Rows(i).Item("Wages_For_Type4").ToString)
                    '.Rows(n).Cells(20).Value = Val(Dt1.Rows(i).Item("Wages_For_Type5").ToString)

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
            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1037" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1059" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then

                If .RowCount > 0 And RwIndx >= 0 Then

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

            Else
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

            End If
        End With

    End Sub

    Private Sub dgv_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Selection.KeyDown
        Dim n As Integer

        Try
            If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
                If dgv_Selection.CurrentCell.RowIndex >= 0 Then

                    n = dgv_Selection.CurrentCell.RowIndex

                    Select_Pavu(n)

                    e.Handled = True

                End If
            End If

        Catch ex As Exception
            '---

        End Try


    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Close_Receipt_Selection()
    End Sub

    Private Sub Close_Receipt_Selection()
        Dim cmd As New SqlClient.SqlCommand
        Dim Da1 As New SqlClient.SqlDataAdapter
        Dim Da2 As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim Clo_ID As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim Total_Meter As String = ""

        Dim RateMtrs1 As Double = 0
        Dim RateMtrs2 As Double = 0
        Dim RateMtrs3 As Double = 0
        Dim RateMtrs4 As Double = 0
        Dim RateMtrs5 As Double = 0
        Dim RatePerPick As String = 0
        Dim PickRatMtr As Double = 0
        Dim Wft_Cnt_Nm As String = ""
        Dim Clo_Pick As Double = 0
        Dim Clo_RdSpc As Double = 0
        Dim Clo_Width As Double = 0
        Dim Clo_Wgt_Mtr_Wft As Double = 0
        Dim RatMtr As Double = 0
        Dim vAmt As String = 0
        Dim vCloNm As String = "", vEndsNm As String = ""
        Dim Fldng As Single = 0
        Dim MtrInFld As Double = 0
        Dim MtrInFld_GKT As Integer = 0
        Dim PcsChkCode As String = ""
        Dim WagesCode As String = ""
        Dim NewCode As String = ""
        Dim WagesDetSTS As Boolean = False
        Dim WtPerMtr_Pick As Double = 0, WtPerMtr As Double = 0
        Dim Nr As Long = 0
        Dim LedIdNo As Integer = 0
        Dim DtCondt As String = ""

        NewCode = Trim(Pk_Condition) & Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        LedIdNo = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Weaver.Text)

        vCloNm = "" : vEndsNm = ""

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(8).Value) = 1 Then

                If Trim(vCloNm) = "" Then

                    vCloNm = dgv_Selection.Rows(i).Cells(4).Value
                    vEndsNm = dgv_Selection.Rows(i).Cells(5).Value

                Else

                    If Trim(UCase(vCloNm)) <> Trim(UCase(dgv_Selection.Rows(i).Cells(4).Value)) Then
                        MessageBox.Show("Invalid Selection - Don't Select Different Quality", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        dgv_Selection.Focus()
                        dgv_Selection.CurrentCell = dgv_Selection.Rows(i).Cells(0)
                        Exit Sub
                    End If

                    If Trim(UCase(vEndsNm)) <> Trim(UCase(dgv_Selection.Rows(i).Cells(5).Value)) Then
                        MessageBox.Show("Invalid Selection - Don't Select Different Ends/Count", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        dgv_Selection.Focus()
                        dgv_Selection.CurrentCell = dgv_Selection.Rows(i).Cells(0)
                        Exit Sub
                    End If

                End If

            End If

        Next i

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then
            DtCondt = "'" & Trim(Format(dtp_Date.Value, "MM/dd/yyyy")) & "'  between From_date_time and To_date_time "
        End If

        pnl_Back.Enabled = True
        pnl_Selection.Visible = False

        lbl_RecCode.Text = ""
        lbl_Cloth.Text = ""
        lbl_Ends_Count.Text = ""

        dgv_Receipt_Details.Rows.Clear()
        dgv_Wages_Details.Rows.Clear()
        dgv_ConsYarn_Details.Rows.Clear()

        cmd.Connection = con

        cmd.CommandText = "truncate table " & Trim(Common_Procedures.ReportTempTable) & ""
        cmd.ExecuteNonQuery()

        WagesDetSTS = False

        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(8).Value) = 1 Then

                NoCalc_Status = True

                Fldng = Val(dgv_Selection.Rows(i).Cells(10).Value)
                If Val(Fldng) = 0 Then Fldng = 100

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then
                    lbl_BillNo.Text = dgv_Selection.Rows(i).Cells(1).Value
                End If

                If Trim(lbl_RecCode.Text) = "" Then
                    lbl_RecCode.Text = dgv_Selection.Rows(i).Cells(9).Value
                End If

                If Trim(lbl_Cloth.Text) = "" Then
                    lbl_Cloth.Text = dgv_Selection.Rows(i).Cells(4).Value
                End If

                If Trim(lbl_Ends_Count.Text) = "" Then
                    lbl_Ends_Count.Text = dgv_Selection.Rows(i).Cells(5).Value
                End If

                n = dgv_Receipt_Details.Rows.Add()
                sno = sno + 1
                dgv_Receipt_Details.Rows(n).Cells(0).Value = Val(sno)
                dgv_Receipt_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(1).Value
                dgv_Receipt_Details.Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(2).Value
                dgv_Receipt_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(3).Value
                dgv_Receipt_Details.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(6).Value
                dgv_Receipt_Details.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(7).Value
                dgv_Receipt_Details.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(9).Value
                'dgv_Receipt_Details.Rows(n).Cells(7).Value = Val(dgv_Selection.Rows(i).Cells(11).Value) + Val(dgv_Selection.Rows(i).Cells(12).Value) + Val(dgv_Selection.Rows(i).Cells(13).Value) + Val(dgv_Selection.Rows(i).Cells(14).Value) + Val(dgv_Selection.Rows(i).Cells(15).Value)

                PcsChkCode = ""
                WagesCode = ""
                Da1 = New SqlClient.SqlDataAdapter("select * from Weaver_Cloth_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "'", con)
                Dt1 = New DataTable
                Da1.Fill(Dt1)
                If Dt1.Rows.Count > 0 Then
                    If IsDBNull(Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) = False Then
                        PcsChkCode = Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString
                    End If
                    If IsDBNull(Dt1.Rows(0).Item("Weaver_Wages_Code").ToString) = False Then
                        WagesCode = Dt1.Rows(0).Item("Weaver_Wages_Code").ToString
                    End If
                End If
                Dt1.Clear()

                If WagesDetSTS = False Then

                    If Trim(WagesCode) <> "" Then

                        If WagesDetSTS = False Then
                            cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select a.ClothType_IdNo, b.ClothType_Name, a.Meters, a.Pick from Weaver_Wages_Cooly_Details a INNER JOIN ClothType_Head b ON a.ClothType_IdNo = b.ClothType_IdNo Where a.Weaver_Wages_Code = '" & Trim(NewCode) & "' and a.Meters <> 0"
                            cmd.ExecuteNonQuery()
                        End If

                        WagesDetSTS = True

                    ElseIf Trim(PcsChkCode) <> "" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1267" Then

                        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then

                            cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 1, '" & Trim(Common_Procedures.ClothType.Type1) & "', a.Sound_Meters,  b.Cloth_Pick  from Weaver_Piece_Checking_Head a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Piece_Receipt_Code = '" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and a.Sound_Meters <> 0"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 2, '" & Trim(Common_Procedures.ClothType.Type2) & "', a.Seconds_Meters, b.Cloth_Pick from Weaver_Piece_Checking_Head a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Piece_Receipt_Code = '" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and a.Seconds_Meters <> 0"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 3, '" & Trim(Common_Procedures.ClothType.Type3) & "', a.Bits_Meters,  b.Cloth_Pick  from Weaver_Piece_Checking_Head a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Piece_Receipt_Code = '" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and a.Bits_Meters <> 0"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 4, '" & Trim(Common_Procedures.ClothType.Type4) & "', a.Reject_Meters,  b.Cloth_Pick  from Weaver_Piece_Checking_Head a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Piece_Receipt_Code = '" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and a.Reject_Meters <> 0"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 5, '" & Trim(Common_Procedures.ClothType.Type5) & "', a.Others_Meters, b.Cloth_Pick from Weaver_Piece_Checking_Head a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Piece_Receipt_Code = '" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and a.Others_Meters <> 0"
                            cmd.ExecuteNonQuery()

                        Else



                            'cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 1, '" & Trim(Common_Procedures.ClothType.Type1) & "', sum(a.Type1_Meters), (CASE WHEN a.Pick = 0 THEN b.Cloth_Pick ELSE a.Pick END) from Weaver_ClothReceipt_Piece_Details a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Weaver_ClothReceipt_Code = 'WCLRC-" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and a.Type1_Meters <> 0 group by a.Pick, b.Cloth_Pick Having sum(a.Type1_Meters) <> 0"
                            'Nr = cmd.ExecuteNonQuery()
                            cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 1, '" & Trim(Common_Procedures.ClothType.Type1) & "', a.Type1_Meters, (CASE WHEN a.Pick = 0 THEN b.Cloth_Pick ELSE a.Pick END) from Weaver_ClothReceipt_Piece_Details a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Weaver_ClothReceipt_Code = 'WCLRC-" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and a.Type1_Meters <> 0"
                            Nr = cmd.ExecuteNonQuery()
                            cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 2, '" & Trim(Common_Procedures.ClothType.Type2) & "', a.Type2_Meters, (CASE WHEN a.Pick = 0 THEN b.Cloth_Pick ELSE a.Pick END) from Weaver_ClothReceipt_Piece_Details a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Weaver_ClothReceipt_Code = 'WCLRC-" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and a.Type2_Meters <> 0"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 3, '" & Trim(Common_Procedures.ClothType.Type3) & "', a.Type3_Meters, (CASE WHEN a.Pick = 0 THEN b.Cloth_Pick ELSE a.Pick END) from Weaver_ClothReceipt_Piece_Details a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Weaver_ClothReceipt_Code = 'WCLRC-" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and a.Type3_Meters <> 0"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 4, '" & Trim(Common_Procedures.ClothType.Type4) & "', a.Type4_Meters, (CASE WHEN a.Pick = 0 THEN b.Cloth_Pick ELSE a.Pick END) from Weaver_ClothReceipt_Piece_Details a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Weaver_ClothReceipt_Code = 'WCLRC-" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and a.Type4_Meters <> 0"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 5, '" & Trim(Common_Procedures.ClothType.Type5) & "', a.Type5_Meters, (CASE WHEN a.Pick = 0 THEN b.Cloth_Pick ELSE a.Pick END) from Weaver_ClothReceipt_Piece_Details a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Weaver_ClothReceipt_Code = 'WCLRC-" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and a.Type5_Meters <> 0"
                            cmd.ExecuteNonQuery()

                            cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 1, '" & Trim(Common_Procedures.ClothType.Type1) & "', a.Type1_Meters, (CASE WHEN a.Pick = 0 THEN b.Cloth_Pick ELSE a.Pick END) from Weaver_ClothReceipt_Piece_Details a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Weaver_ClothReceipt_Code = 'PCDOF-" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and a.Type1_Meters <> 0"
                            Nr = cmd.ExecuteNonQuery()
                            cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 2, '" & Trim(Common_Procedures.ClothType.Type2) & "', a.Type2_Meters, (CASE WHEN a.Pick = 0 THEN b.Cloth_Pick ELSE a.Pick END) from Weaver_ClothReceipt_Piece_Details a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Weaver_ClothReceipt_Code = 'PCDOF-" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and a.Type2_Meters <> 0"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 3, '" & Trim(Common_Procedures.ClothType.Type3) & "', a.Type3_Meters, (CASE WHEN a.Pick = 0 THEN b.Cloth_Pick ELSE a.Pick END) from Weaver_ClothReceipt_Piece_Details a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Weaver_ClothReceipt_Code = 'PCDOF-" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and a.Type3_Meters <> 0"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 4, '" & Trim(Common_Procedures.ClothType.Type4) & "', a.Type4_Meters, (CASE WHEN a.Pick = 0 THEN b.Cloth_Pick ELSE a.Pick END) from Weaver_ClothReceipt_Piece_Details a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Weaver_ClothReceipt_Code = 'PCDOF-" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and a.Type4_Meters <> 0"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 5, '" & Trim(Common_Procedures.ClothType.Type5) & "', a.Type5_Meters, (CASE WHEN a.Pick = 0 THEN b.Cloth_Pick ELSE a.Pick END) from Weaver_ClothReceipt_Piece_Details a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Weaver_ClothReceipt_Code = 'PCDOF-" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and a.Type5_Meters <> 0"
                            cmd.ExecuteNonQuery()

                            cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 1, '" & Trim(Common_Procedures.ClothType.Type1) & "', a.Type1_Meters, (CASE WHEN a.Pick = 0 OR a.Pick is Null THEN b.Cloth_Pick ELSE a.Pick END) from Weaver_ClothReceipt_Piece_Details a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Weaver_ClothReceipt_Code = '" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and a.Type1_Meters <> 0"
                            Nr = cmd.ExecuteNonQuery()
                            cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 2, '" & Trim(Common_Procedures.ClothType.Type2) & "', a.Type2_Meters, (CASE WHEN a.Pick = 0 OR a.Pick is Null THEN b.Cloth_Pick ELSE a.Pick END) from Weaver_ClothReceipt_Piece_Details a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Weaver_ClothReceipt_Code = '" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and a.Type2_Meters <> 0"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 3, '" & Trim(Common_Procedures.ClothType.Type3) & "', a.Type3_Meters, (CASE WHEN a.Pick = 0 OR a.Pick is Null THEN b.Cloth_Pick ELSE a.Pick END) from Weaver_ClothReceipt_Piece_Details a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Weaver_ClothReceipt_Code = '" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and a.Type3_Meters <> 0"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 4, '" & Trim(Common_Procedures.ClothType.Type4) & "', a.Type4_Meters, (CASE WHEN a.Pick = 0 OR a.Pick is Null THEN b.Cloth_Pick ELSE a.Pick END) from Weaver_ClothReceipt_Piece_Details a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Weaver_ClothReceipt_Code = '" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and a.Type4_Meters <> 0"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 5, '" & Trim(Common_Procedures.ClothType.Type5) & "', a.Type5_Meters, (CASE WHEN a.Pick = 0 OR a.Pick is Null THEN b.Cloth_Pick ELSE a.Pick END) from Weaver_ClothReceipt_Piece_Details a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Weaver_ClothReceipt_Code = '" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and a.Type5_Meters <> 0"
                            cmd.ExecuteNonQuery()



                        End If


                    Else

                        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 1, '" & Trim(Common_Procedures.ClothType.Type1) & "', (CASE WHEN a.Type1_Wages_Meters <> 0 THEN a.Type1_Wages_Meters ELSE a.Type1_Checking_Meters END), b.Cloth_Pick from Weaver_Cloth_Receipt_Head a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Weaver_ClothReceipt_Code = '" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and (a.Type1_Wages_Meters <> 0 or a.Type1_Checking_Meters <> 0)"
                        cmd.ExecuteNonQuery()
                        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 2, '" & Trim(Common_Procedures.ClothType.Type2) & "', (CASE WHEN a.Type2_Wages_Meters <> 0 THEN a.Type2_Wages_Meters ELSE a.Type2_Checking_Meters END), b.Cloth_Pick from Weaver_Cloth_Receipt_Head a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Weaver_ClothReceipt_Code = '" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and (a.Type2_Wages_Meters <> 0 or a.Type2_Checking_Meters <> 0)"
                        cmd.ExecuteNonQuery()
                        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 3, '" & Trim(Common_Procedures.ClothType.Type3) & "', (CASE WHEN a.Type3_Wages_Meters <> 0 THEN a.Type3_Wages_Meters ELSE a.Type3_Checking_Meters END), b.Cloth_Pick from Weaver_Cloth_Receipt_Head a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Weaver_ClothReceipt_Code = '" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and (a.Type3_Wages_Meters <> 0 or a.Type3_Checking_Meters <> 0)"
                        cmd.ExecuteNonQuery()
                        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 4, '" & Trim(Common_Procedures.ClothType.Type4) & "', (CASE WHEN a.Type4_Wages_Meters <> 0 THEN a.Type4_Wages_Meters ELSE a.Type4_Checking_Meters END), b.Cloth_Pick from Weaver_Cloth_Receipt_Head a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Weaver_ClothReceipt_Code = '" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and (a.Type4_Wages_Meters <> 0 or a.Type4_Checking_Meters <> 0)"
                        cmd.ExecuteNonQuery()
                        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 5, '" & Trim(Common_Procedures.ClothType.Type5) & "', (CASE WHEN a.Type5_Wages_Meters <> 0 THEN a.Type5_Wages_Meters ELSE a.Type5_Checking_Meters END), b.Cloth_Pick from Weaver_Cloth_Receipt_Head a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Weaver_ClothReceipt_Code = '" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and (a.Type5_Wages_Meters <> 0 or a.Type5_Checking_Meters <> 0)"
                        cmd.ExecuteNonQuery()

                    End If

                End If


                If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1037" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1059" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1087" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1195" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1242" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1249" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1267" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1360" Then
                    Exit For
                End If


            End If


        Next

        NoCalc_Status = False
        Calculation_Total_ReceiptMeter()
        NoCalc_Status = True

        Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, lbl_Cloth.Text)

        RateMtrs1 = 0
        RateMtrs2 = 0
        RateMtrs3 = 0
        RateMtrs4 = 0
        RateMtrs5 = 0
        Wft_Cnt_Nm = ""
        Clo_RdSpc = 0
        Clo_Pick = 0
        Clo_Width = 0
        Clo_Wgt_Mtr_Wft = 0

        Da1 = New SqlClient.SqlDataAdapter("Select a.*, Count_Name as Weft_CountName from Cloth_Head a, Count_Head b Where a.Cloth_IdNo = " & Str(Val(Clo_ID)) & " and a.Cloth_WeftCount_IdNo = b.count_idno", con)
        Dt1 = New DataTable
        Da1.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then

            Wft_Cnt_Nm = Dt1.Rows(0).Item("Weft_CountName").ToString
            Clo_Pick = Val(Dt1.Rows(0).Item("Cloth_Pick").ToString)
            Clo_RdSpc = Val(Dt1.Rows(0).Item("Cloth_ReedSpace").ToString)
            Clo_Width = Val(Dt1.Rows(0).Item("Cloth_Width").ToString)
            Clo_Wgt_Mtr_Wft = Val(Dt1.Rows(0).Item("Weight_Meter_Weft").ToString)

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1242" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then

                Da1 = New SqlClient.SqlDataAdapter("select a.* from Ledger_Weaver_Wages_Details a Where a.ledger_Idno = " & Str(Val(LedIdNo)) & " and  a.cloth_Idno = " & Str(Val(Dt1.Rows(0).Item("Cloth_IdNo").ToString)) & IIf(Trim(DtCondt) <> "", " And ", "") & DtCondt & " order by a.Sl_No", con)
                Dt2 = New DataTable
                Da1.Fill(Dt2)
                If Dt2.Rows.Count > 0 Then
                    RateMtrs1 = Format(Val(Dt2.Rows(0).Item("Type1_Wages_Rate").ToString), "#########0.00")
                    RateMtrs2 = Format(Val(Dt2.Rows(0).Item("Type2_Wages_Rate").ToString), "#########0.00")
                    RateMtrs3 = Format(Val(Dt2.Rows(0).Item("Type3_Wages_Rate").ToString), "#########0.00")
                    RateMtrs4 = Format(Val(Dt2.Rows(0).Item("Type4_Wages_Rate").ToString), "#########0.00")
                    RateMtrs5 = Format(Val(Dt2.Rows(0).Item("Type5_Wages_Rate").ToString), "#########0.00")
                End If
                Dt2.Clear()

            Else

                RateMtrs1 = Val(Dt1.Rows(0).Item("Wages_For_Type1").ToString)
                RateMtrs2 = Val(Dt1.Rows(0).Item("Wages_For_Type2").ToString)
                RateMtrs3 = Val(Dt1.Rows(0).Item("Wages_For_Type3").ToString)
                RateMtrs4 = Val(Dt1.Rows(0).Item("Wages_For_Type4").ToString)
                RateMtrs5 = Val(Dt1.Rows(0).Item("Wages_For_Type5").ToString)

            End If

        End If
        Dt1.Clear()

        PcsChkCode = ""
        WagesCode = ""
        Da1 = New SqlClient.SqlDataAdapter("select * from Weaver_Cloth_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "'", con)
        Dt1 = New DataTable
        Da1.Fill(Dt1)
        If Dt1.Rows.Count > 0 Then
            If IsDBNull(Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) = False Then
                PcsChkCode = Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString
            End If
            If IsDBNull(Dt1.Rows(0).Item("Weaver_Wages_Code").ToString) = False Then
                WagesCode = Dt1.Rows(0).Item("Weaver_Wages_Code").ToString
            End If
        End If
        Dt1.Clear()


        sno = 0
        dgv_Wages_Details.Rows.Clear()

        If Trim(WagesCode) <> "" Then

            Da2 = New SqlClient.SqlDataAdapter("Select a.* , c.ClothType_Name from Weaver_Wages_Cooly_Details a LEFT OUTER JOIN ClothType_Head c ON a.ClothType_IdNo = c.ClothType_IdNo Where a.Weaver_Wages_Code = '" & Trim(NewCode) & "' Order by Sl_No", con)
            Dt3 = New DataTable
            Da2.Fill(Dt3)

            With dgv_Wages_Details

                .Rows.Clear()
                sno = 0

                If Dt3.Rows.Count > 0 Then

                    For i = 0 To Dt3.Rows.Count - 1

                        n = .Rows.Add()
                        sno = sno + 1
                        .Rows(n).Cells(0).Value = Val(sno)
                        .Rows(n).Cells(1).Value = Format(Val(Dt3.Rows(i).Item("Meters").ToString), "########0.00")
                        .Rows(n).Cells(2).Value = Dt3.Rows(i).Item("ClothType_Name").ToString
                        .Rows(n).Cells(3).Value = Format(Val(Dt3.Rows(i).Item("Pick").ToString), "########0.00")
                        .Rows(n).Cells(4).Value = Format(Val(Dt3.Rows(i).Item("Cooly").ToString), "########0.00")
                        .Rows(n).Cells(5).Value = Format(Val(Dt3.Rows(i).Item("Amount").ToString), "########0.00")

                    Next i

                End If

                If .RowCount = 0 Then .Rows.Add()

            End With

            Dt3.Clear()

        Else

            Da1 = New SqlClient.SqlDataAdapter("Select Int1, name2, Currency1 as PcsCk_Pick, sum(meters1) as Mtrs from " & Trim(Common_Procedures.ReportTempTable) & " a group by Int1, name2, Currency1 having sum(meters1) <> 0 Order by Int1, name2, Currency1 desc", con)
            Dt1 = New DataTable
            Da1.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For j = 0 To Dt1.Rows.Count - 1

                    NoCalc_Status = True

                    RatMtr = 0
                    vAmt = 0
                    If Val(Dt1.Rows(j).Item("Int1").ToString) = 1 Then
                        RatMtr = RateMtrs1
                    ElseIf Val(Dt1.Rows(j).Item("Int1").ToString) = 2 Then
                        RatMtr = RateMtrs2
                    ElseIf Val(Dt1.Rows(j).Item("Int1").ToString) = 3 Then
                        RatMtr = RateMtrs3
                    ElseIf Val(Dt1.Rows(j).Item("Int1").ToString) = 4 Then
                        RatMtr = RateMtrs4
                    ElseIf Val(Dt1.Rows(j).Item("Int1").ToString) = 5 Then
                        RatMtr = RateMtrs5
                    End If

                    RatePerPick = 0
                    If Clo_Pick <> 0 Then
                        RatePerPick = Format(RatMtr / Clo_Pick, "#########0.00000000")
                    End If

                    PickRatMtr = 0
                    If Clo_Pick <> 0 And Val(Dt1.Rows(j).Item("PcsCk_Pick").ToString) <> 0 Then
                        PickRatMtr = Format(Val(Dt1.Rows(j).Item("PcsCk_Pick").ToString) * Val(RatePerPick), "#########0.00")
                    Else
                        PickRatMtr = RatMtr
                    End If

                    MtrInFld = 0
                    MtrInFld_GKT = 0

                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then
                        MtrInFld_GKT = Val(Dt1.Rows(j).Item("Mtrs").ToString) * Val(Fldng) / 100
                        vAmt = Format(Val(MtrInFld_GKT) * PickRatMtr, "##########0")

                    ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
                        MtrInFld = Format(Val(Dt1.Rows(j).Item("Mtrs").ToString) * Val(Fldng) / 100, "##########0.0")
                        vAmt = Format(Val(MtrInFld) * PickRatMtr, "##########0")

                    ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then
                        MtrInFld = Format(Val(Dt1.Rows(j).Item("Mtrs").ToString) * Val(Fldng) / 100, "##########0.00")
                        vAmt = Format(Val(MtrInFld) * PickRatMtr, "##########0.00")

                    Else
                        MtrInFld = Format(Val(Dt1.Rows(j).Item("Mtrs").ToString) * Val(Fldng) / 100, "##########0")
                        vAmt = Format(Val(MtrInFld) * PickRatMtr, "##########0")

                    End If

                    n = dgv_Wages_Details.Rows.Add()

                    sno = sno + 1

                    dgv_Wages_Details.Rows(n).Cells(0).Value = Val(sno)
                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then
                        dgv_Wages_Details.Rows(n).Cells(1).Value = Format(Val(MtrInFld_GKT), "#########0.00")
                    Else
                        dgv_Wages_Details.Rows(n).Cells(1).Value = Format(Val(MtrInFld), "#########0.00")
                    End If

                    dgv_Wages_Details.Rows(n).Cells(2).Value = Dt1.Rows(j).Item("name2").ToString
                    dgv_Wages_Details.Rows(n).Cells(3).Value = Val(Dt1.Rows(j).Item("PcsCk_Pick").ToString)

                    dgv_Wages_Details.Rows(n).Cells(4).Value = Format(PickRatMtr, "#########0.00")
                    dgv_Wages_Details.Rows(n).Cells(5).Value = Format(Val(vAmt), "#########0.00")

                Next

            End If

            Dt1.Clear()

        End If



        NoCalc_Status = False
        Calculation_Total_Wages()
        NoCalc_Status = True

        Total_Meter = 0
        If dgv_WagesDetails_Total.RowCount > 0 Then
            Total_Meter = Val(dgv_WagesDetails_Total.Rows(0).Cells(1).Value)
        End If

        sno = 0
        dgv_ConsYarn_Details.Rows.Clear()

        If Trim(WagesCode) <> "" Then
            Da1 = New SqlClient.SqlDataAdapter("Select a.Meters, b.Count_Name, a.Rd_Sp, a.Pick, a.Width, a.Weight_Meter, a.Weight from Weaver_Wages_Yarn_Details a INNER JOIN count_head b on a.Count_IdNo = b.Count_IdNo Where a.Weaver_Wages_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
        Else
            Da1 = New SqlClient.SqlDataAdapter("Select '" & Trim(Wft_Cnt_Nm) & "' as Count_Name, " & Str(Val(Clo_RdSpc)) & " as Rd_Sp, " & Str(Val(Clo_Width)) & " as Width, " & Str(Val(Clo_Wgt_Mtr_Wft)) & " as Weight_Meter, Currency1 as Pick, sum(meters1) as Meters from " & Trim(Common_Procedures.ReportTempTable) & " a group by Currency1 having sum(meters1) <> 0 Order by Currency1 desc", con)
        End If

        Dt1 = New DataTable
        Da1.Fill(Dt1)

        If Dt1.Rows.Count > 0 Then

            For j = 0 To Dt1.Rows.Count - 1

                NoCalc_Status = True

                n = dgv_ConsYarn_Details.Rows.Add()

                MtrInFld = 0
                MtrInFld_GKT = 0

                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then
                    MtrInFld_GKT = Val(Dt1.Rows(j).Item("Meters").ToString) * Val(Fldng) / 100
                    dgv_ConsYarn_Details.Rows(n).Cells(0).Value = Format(Val(MtrInFld_GKT), "#########0.00")

                Else
                    MtrInFld = Format(Val(Dt1.Rows(j).Item("Meters").ToString) * Val(Fldng) / 100, "##########0")
                    dgv_ConsYarn_Details.Rows(n).Cells(0).Value = Format(Val(MtrInFld), "#########0.00")

                End If

                dgv_ConsYarn_Details.Rows(n).Cells(1).Value = Dt1.Rows(j).Item("Count_Name").ToString
                dgv_ConsYarn_Details.Rows(n).Cells(2).Value = Val(Dt1.Rows(j).Item("Rd_Sp").ToString)
                dgv_ConsYarn_Details.Rows(n).Cells(3).Value = Val(Dt1.Rows(j).Item("Pick").ToString)
                dgv_ConsYarn_Details.Rows(n).Cells(4).Value = Val(Dt1.Rows(j).Item("Width").ToString)

                If Trim(WagesCode) <> "" Then

                    dgv_ConsYarn_Details.Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(j).Item("Weight_Meter").ToString), "#########0.000000")

                Else

                    WtPerMtr_Pick = 0
                    If Clo_Pick <> 0 Then
                        WtPerMtr_Pick = Val(Dt1.Rows(j).Item("Weight_Meter").ToString) / Clo_Pick
                        WtPerMtr = Format(Val(Dt1.Rows(j).Item("Pick").ToString) * WtPerMtr_Pick, "#########0.000000")
                    Else
                        WtPerMtr = Format(Val(Dt1.Rows(j).Item("Weight_Meter").ToString), "#########0.000000")
                    End If

                    dgv_ConsYarn_Details.Rows(n).Cells(5).Value = Format(Val(WtPerMtr), "#########0.000000")

                End If

                dgv_ConsYarn_Details.Rows(n).Cells(6).Value = Format(Val(dgv_ConsYarn_Details.Rows(n).Cells(0).Value) * Val(dgv_ConsYarn_Details.Rows(n).Cells(5).Value), "#########0.000")

                NoCalc_Status = False
                Weight_Calculation(n, 0)
                NoCalc_Status = True

            Next

        End If
        Dt1.Clear()


        'Da1 = New SqlClient.SqlDataAdapter("Select " & Val(Total_Meter) & " as TotalMeter, b.*, c.Count_Name from Weaver_Cloth_Receipt_Head a, cloth_head b, count_head c where a.Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "' and a.cloth_idno = b.cloth_idno and a.count_idno = c.count_idno", con)
        'Dt1 = New DataTable
        'Da1.Fill(Dt1)

        'If Dt1.Rows.Count > 0 Then

        '    For j = 0 To Dt1.Rows.Count - 1

        '        NoCalc_Status = True

        '        n = dgv_ConsYarn_Details.Rows.Add()

        '        dgv_ConsYarn_Details.Rows(n).Cells(0).Value = Format(Val(Dt1.Rows(j).Item("TotalMeter").ToString), "#########0.00")
        '        dgv_ConsYarn_Details.Rows(n).Cells(1).Value = Dt1.Rows(j).Item("Count_Name").ToString
        '        dgv_ConsYarn_Details.Rows(n).Cells(2).Value = Dt1.Rows(j).Item("Cloth_ReedSpace").ToString
        '        dgv_ConsYarn_Details.Rows(n).Cells(3).Value = Dt1.Rows(j).Item("Cloth_Pick").ToString
        '        dgv_ConsYarn_Details.Rows(n).Cells(4).Value = Dt1.Rows(j).Item("Cloth_Width").ToString
        '        dgv_ConsYarn_Details.Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(j).Item("Weight_Meter_Weft").ToString), "#########0.000000")
        '        dgv_ConsYarn_Details.Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(j).Item("TotalMeter").ToString) * Val(Dt1.Rows(j).Item("Weight_Meter_Weft").ToString), "#########0.000")

        '        NoCalc_Status = False
        '        Weight_Calculation(n, 0)
        '        NoCalc_Status = True

        '    Next

        'End If
        'Dt1.Clear()

        NoCalc_Status = False
        Calculation_Total_ConsumedYarnDetails()
        NoCalc_Status = True

        'get_GST_Percentage()

        Grid_DeSelect()
        pnl_Back.Enabled = True
        pnl_Selection.Visible = False

        If dgv_Wages_Details.Rows.Count > 0 Then
            dgv_Wages_Details.Focus()
            dgv_Wages_Details.CurrentCell = dgv_Wages_Details.Rows(0).Cells(1)
        Else
            txt_Elogation.Focus()
        End If

        NoCalc_Status = False

    End Sub


    'Private Sub Close_Receipt_Selection()
    '    Dim cmd As New SqlClient.SqlCommand
    '    Dim Da1 As New SqlClient.SqlDataAdapter
    '    Dim Da2 As New SqlClient.SqlDataAdapter
    '    Dim Dt1 As New DataTable
    '    Dim Dt2 As New DataTable
    '    Dim Dt3 As New DataTable
    '    Dim n As Integer = 0
    '    Dim m As Integer = 0
    '    Dim sno As Integer = 0
    '    Dim Clo_ID As Integer = 0
    '    Dim i As Integer = 0
    '    Dim j As Integer = 0
    '    Dim Total_Meter As Single = 0
    '    Dim RateMtrs1 As Double = 0
    '    Dim RateMtrs2 As Double = 0
    '    Dim RateMtrs3 As Double = 0
    '    Dim RateMtrs4 As Double = 0
    '    Dim RateMtrs5 As Double = 0
    '    Dim RatePerPick As Double = 0
    '    Dim PickRatMtr As Double = 0
    '    Dim Wft_Cnt_Nm As String = ""
    '    Dim Clo_Pick As Double = 0
    '    Dim Clo_RdSpc As Double = 0
    '    Dim Clo_Width As Double = 0
    '    Dim Clo_Wgt_Mtr_Wft As Double = 0
    '    Dim RatMtr As Double = 0
    '    Dim Amt As Double = 0
    '    Dim vCloNm As String = "", vEndsNm As String = ""
    '    Dim Fldng As Single = 0
    '    Dim MtrInFld As Double = 0
    '    Dim MtrInFld_GKT As Integer = 0
    '    Dim PcsChkCode As String = ""
    '    Dim WagesCode As String = ""
    '    Dim NewCode As String = ""
    '    Dim WagesDetSTS As Boolean = False
    '    Dim WtPerMtr_Pick As Double = 0, WtPerMtr As Double = 0

    '    NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

    '    vCloNm = "" : vEndsNm = ""

    '    For i = 0 To dgv_Selection.RowCount - 1

    '        If Val(dgv_Selection.Rows(i).Cells(8).Value) = 1 Then
    '            If Trim(vCloNm) = "" Then
    '                vCloNm = dgv_Selection.Rows(i).Cells(4).Value
    '                vEndsNm = dgv_Selection.Rows(i).Cells(5).Value

    '            Else

    '                If Trim(UCase(vCloNm)) <> Trim(UCase(dgv_Selection.Rows(i).Cells(4).Value)) Then
    '                    MessageBox.Show("Invalid Selection - Don't Select Different Quality", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '                    dgv_Selection.Focus()
    '                    dgv_Selection.CurrentCell = dgv_Selection.Rows(i).Cells(0)
    '                    Exit Sub
    '                End If

    '                If Trim(UCase(vEndsNm)) <> Trim(UCase(dgv_Selection.Rows(i).Cells(5).Value)) Then
    '                    MessageBox.Show("Invalid Selection - Don't Select Different Ends/Count", "DOES NOT SAVE...", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '                    dgv_Selection.Focus()
    '                    dgv_Selection.CurrentCell = dgv_Selection.Rows(i).Cells(0)
    '                    Exit Sub
    '                End If

    '            End If

    '        End If

    '    Next i


    '    pnl_Back.Enabled = True
    '    pnl_Selection.Visible = False

    '    lbl_RecCode.Text = ""
    '    lbl_Cloth.Text = ""
    '    lbl_Ends_Count.Text = ""

    '    dgv_Receipt_Details.Rows.Clear()
    '    dgv_Wages_Details.Rows.Clear()
    '    dgv_ConsYarn_Details.Rows.Clear()

    '    cmd.Connection = con

    '    cmd.CommandText = "truncate table " & Trim(Common_Procedures.ReportTempTable) & ""
    '    cmd.ExecuteNonQuery()

    '    WagesDetSTS = False

    '    For i = 0 To dgv_Selection.RowCount - 1

    '        If Val(dgv_Selection.Rows(i).Cells(8).Value) = 1 Then

    '            NoCalc_Status = True

    '            Fldng = Val(dgv_Selection.Rows(i).Cells(10).Value)
    '            If Val(Fldng) = 0 Then Fldng = 100

    '            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then
    '                lbl_BillNo.Text = dgv_Selection.Rows(i).Cells(1).Value
    '            End If

    '            If Trim(lbl_RecCode.Text) = "" Then
    '                lbl_RecCode.Text = dgv_Selection.Rows(i).Cells(9).Value
    '            End If

    '            If Trim(lbl_Cloth.Text) = "" Then
    '                lbl_Cloth.Text = dgv_Selection.Rows(i).Cells(4).Value
    '            End If

    '            If Trim(lbl_Ends_Count.Text) = "" Then
    '                lbl_Ends_Count.Text = dgv_Selection.Rows(i).Cells(5).Value
    '            End If

    '            n = dgv_Receipt_Details.Rows.Add()
    '            sno = sno + 1
    '            dgv_Receipt_Details.Rows(n).Cells(0).Value = Val(sno)
    '            dgv_Receipt_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(1).Value
    '            dgv_Receipt_Details.Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(2).Value
    '            dgv_Receipt_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(3).Value
    '            dgv_Receipt_Details.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(6).Value
    '            dgv_Receipt_Details.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(7).Value
    '            dgv_Receipt_Details.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(9).Value

    '            m = dgv_Wages_Details.Rows.Add()
    '            dgv_Wages_Details.Rows(m).Cells(1).Value = dgv_Selection.Rows(i).Cells(7).Value
    '            PcsChkCode = ""
    '            WagesCode = ""
    '            Da1 = New SqlClient.SqlDataAdapter("select * from Weaver_Cloth_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "'", con)
    '            Dt1 = New DataTable
    '            Da1.Fill(Dt1)
    '            If Dt1.Rows.Count > 0 Then
    '                If IsDBNull(Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) = False Then
    '                    PcsChkCode = Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString
    '                End If
    '                If IsDBNull(Dt1.Rows(0).Item("Weaver_Wages_Code").ToString) = False Then
    '                    WagesCode = Dt1.Rows(0).Item("Weaver_Wages_Code").ToString
    '                End If
    '            End If
    '            Dt1.Clear()

    '            If WagesDetSTS = False Then

    '                If Trim(WagesCode) <> "" Then

    '                    If WagesDetSTS = False Then
    '                        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select a.ClothType_IdNo, b.ClothType_Name, a.Meters, a.Pick from Weaver_Wages_Cooly_Details a INNER JOIN ClothType_Head b ON a.ClothType_IdNo = b.ClothType_IdNo Where a.Weaver_Wages_Code = '" & Trim(NewCode) & "' and a.Meters <> 0"
    '                        cmd.ExecuteNonQuery()
    '                    End If

    '                    WagesDetSTS = True

    '                ElseIf Trim(PcsChkCode) <> "" Then

    '                    If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then

    '                        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 1, '" & Trim(Common_Procedures.ClothType.Type1) & "', a.Sound_Meters,  b.Cloth_Pick  from Weaver_Piece_Checking_Head a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Piece_Receipt_Code = '" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and a.Sound_Meters <> 0"
    '                        cmd.ExecuteNonQuery()
    '                        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 2, '" & Trim(Common_Procedures.ClothType.Type2) & "', a.Seconds_Meters, b.Cloth_Pick from Weaver_Piece_Checking_Head a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Piece_Receipt_Code = '" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and a.Seconds_Meters <> 0"
    '                        cmd.ExecuteNonQuery()
    '                        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 3, '" & Trim(Common_Procedures.ClothType.Type3) & "', a.Bits_Meters,  b.Cloth_Pick  from Weaver_Piece_Checking_Head a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Piece_Receipt_Code = '" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and a.Bits_Meters <> 0"
    '                        cmd.ExecuteNonQuery()
    '                        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 4, '" & Trim(Common_Procedures.ClothType.Type4) & "', a.Reject_Meters,  b.Cloth_Pick  from Weaver_Piece_Checking_Head a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Piece_Receipt_Code = '" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and a.Reject_Meters <> 0"
    '                        cmd.ExecuteNonQuery()
    '                        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 5, '" & Trim(Common_Procedures.ClothType.Type5) & "', a.Others_Meters, b.Cloth_Pick from Weaver_Piece_Checking_Head a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Piece_Receipt_Code = '" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and a.Others_Meters <> 0"
    '                        cmd.ExecuteNonQuery()

    '                    Else

    '                        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 1, '" & Trim(Common_Procedures.ClothType.Type1) & "', a.Type1_Meters, (CASE WHEN a.Pick = 0 THEN b.Cloth_Pick ELSE a.Pick END) from Weaver_ClothReceipt_Piece_Details a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Weaver_ClothReceipt_Code = 'WCLRC-" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and a.Type1_Meters <> 0"
    '                        cmd.ExecuteNonQuery()
    '                        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 2, '" & Trim(Common_Procedures.ClothType.Type2) & "', a.Type2_Meters, (CASE WHEN a.Pick = 0 THEN b.Cloth_Pick ELSE a.Pick END) from Weaver_ClothReceipt_Piece_Details a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Weaver_ClothReceipt_Code = 'WCLRC-" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and a.Type2_Meters <> 0"
    '                        cmd.ExecuteNonQuery()
    '                        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 3, '" & Trim(Common_Procedures.ClothType.Type3) & "', a.Type3_Meters, (CASE WHEN a.Pick = 0 THEN b.Cloth_Pick ELSE a.Pick END) from Weaver_ClothReceipt_Piece_Details a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Weaver_ClothReceipt_Code = 'WCLRC-" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and a.Type3_Meters <> 0"
    '                        cmd.ExecuteNonQuery()
    '                        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 4, '" & Trim(Common_Procedures.ClothType.Type4) & "', a.Type4_Meters, (CASE WHEN a.Pick = 0 THEN b.Cloth_Pick ELSE a.Pick END) from Weaver_ClothReceipt_Piece_Details a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Weaver_ClothReceipt_Code = 'WCLRC-" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and a.Type4_Meters <> 0"
    '                        cmd.ExecuteNonQuery()
    '                        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 5, '" & Trim(Common_Procedures.ClothType.Type5) & "', a.Type5_Meters, (CASE WHEN a.Pick = 0 THEN b.Cloth_Pick ELSE a.Pick END) from Weaver_ClothReceipt_Piece_Details a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Weaver_ClothReceipt_Code = 'WCLRC-" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and a.Type5_Meters <> 0"
    '                        cmd.ExecuteNonQuery()
    '                    End If
    '                Else
    '                    cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 1, '" & Trim(Common_Procedures.ClothType.Type1) & "', (CASE WHEN a.Type1_Wages_Meters <> 0 THEN a.Type1_Wages_Meters ELSE a.Type1_Checking_Meters END), b.Cloth_Pick from Weaver_Cloth_Receipt_Head a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Weaver_ClothReceipt_Code = '" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and (a.Type1_Wages_Meters <> 0 or a.Type1_Checking_Meters <> 0)"
    '                    cmd.ExecuteNonQuery()
    '                    cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 2, '" & Trim(Common_Procedures.ClothType.Type2) & "', (CASE WHEN a.Type2_Wages_Meters <> 0 THEN a.Type2_Wages_Meters ELSE a.Type2_Checking_Meters END), b.Cloth_Pick from Weaver_Cloth_Receipt_Head a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Weaver_ClothReceipt_Code = '" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and (a.Type2_Wages_Meters <> 0 or a.Type2_Checking_Meters <> 0)"
    '                    cmd.ExecuteNonQuery()
    '                    cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 3, '" & Trim(Common_Procedures.ClothType.Type3) & "', (CASE WHEN a.Type3_Wages_Meters <> 0 THEN a.Type3_Wages_Meters ELSE a.Type3_Checking_Meters END), b.Cloth_Pick from Weaver_Cloth_Receipt_Head a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Weaver_ClothReceipt_Code = '" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and (a.Type3_Wages_Meters <> 0 or a.Type3_Checking_Meters <> 0)"
    '                    cmd.ExecuteNonQuery()
    '                    cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 4, '" & Trim(Common_Procedures.ClothType.Type4) & "', (CASE WHEN a.Type4_Wages_Meters <> 0 THEN a.Type4_Wages_Meters ELSE a.Type4_Checking_Meters END), b.Cloth_Pick from Weaver_Cloth_Receipt_Head a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Weaver_ClothReceipt_Code = '" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and (a.Type4_Wages_Meters <> 0 or a.Type4_Checking_Meters <> 0)"
    '                    cmd.ExecuteNonQuery()
    '                    cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & " ( Int1, name2, meters1, Currency1 ) select 5, '" & Trim(Common_Procedures.ClothType.Type5) & "', (CASE WHEN a.Type5_Wages_Meters <> 0 THEN a.Type5_Wages_Meters ELSE a.Type5_Checking_Meters END), b.Cloth_Pick from Weaver_Cloth_Receipt_Head a INNER JOIN Cloth_Head b ON a.Cloth_IdNo = b.Cloth_IdNo Where a.Weaver_ClothReceipt_Code = '" & Trim(dgv_Selection.Rows(i).Cells(9).Value) & "' and (a.Type5_Wages_Meters <> 0 or a.Type5_Checking_Meters <> 0)"
    '                    cmd.ExecuteNonQuery()

    '                End If


    '            End If

    '            If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1037" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1059" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1087" Then
    '                Exit For
    '            End If

    '        End If

    '    Next

    '    NoCalc_Status = False
    '    Calculation_Total_ReceiptMeter()
    '    NoCalc_Status = True

    '    Clo_ID = Common_Procedures.Cloth_NameToIdNo(con, lbl_Cloth.Text)

    '    RateMtrs1 = 0
    '    RateMtrs2 = 0
    '    RateMtrs3 = 0
    '    RateMtrs4 = 0
    '    RateMtrs5 = 0
    '    Wft_Cnt_Nm = ""
    '    Clo_RdSpc = 0
    '    Clo_Pick = 0
    '    Clo_Width = 0
    '    Clo_Wgt_Mtr_Wft = 0

    '    Da1 = New SqlClient.SqlDataAdapter("Select a.*, Count_Name as Weft_CountName from Cloth_Head a, Count_Head b Where a.Cloth_IdNo = " & Str(Val(Clo_ID)) & " and a.Cloth_WeftCount_IdNo = b.count_idno", con)
    '    Dt1 = New DataTable
    '    Da1.Fill(Dt1)
    '    If Dt1.Rows.Count > 0 Then
    '        Wft_Cnt_Nm = Dt1.Rows(0).Item("Weft_CountName").ToString
    '        Clo_Pick = Val(Dt1.Rows(0).Item("Cloth_Pick").ToString)
    '        Clo_RdSpc = Val(Dt1.Rows(0).Item("Cloth_ReedSpace").ToString)
    '        Clo_Width = Val(Dt1.Rows(0).Item("Cloth_Width").ToString)
    '        Clo_Wgt_Mtr_Wft = Val(Dt1.Rows(0).Item("Weight_Meter_Weft").ToString)

    '        RateMtrs1 = Val(Dt1.Rows(0).Item("Wages_For_Type1").ToString)
    '        RateMtrs2 = Val(Dt1.Rows(0).Item("Wages_For_Type2").ToString)
    '        RateMtrs3 = Val(Dt1.Rows(0).Item("Wages_For_Type3").ToString)
    '        RateMtrs4 = Val(Dt1.Rows(0).Item("Wages_For_Type4").ToString)
    '        RateMtrs5 = Val(Dt1.Rows(0).Item("Wages_For_Type5").ToString)
    '    End If
    '    Dt1.Clear()

    '    PcsChkCode = ""
    '    WagesCode = ""
    '    Da1 = New SqlClient.SqlDataAdapter("select * from Weaver_Cloth_Receipt_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "'", con)
    '    Dt1 = New DataTable
    '    Da1.Fill(Dt1)
    '    If Dt1.Rows.Count > 0 Then
    '        If IsDBNull(Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString) = False Then
    '            PcsChkCode = Dt1.Rows(0).Item("Weaver_Piece_Checking_Code").ToString
    '        End If
    '        If IsDBNull(Dt1.Rows(0).Item("Weaver_Wages_Code").ToString) = False Then
    '            WagesCode = Dt1.Rows(0).Item("Weaver_Wages_Code").ToString
    '        End If
    '    End If
    '    Dt1.Clear()


    '    sno = 0


    '    If Trim(WagesCode) <> "" Then
    '        da2 = New SqlClient.SqlDataAdapter("Select a.* , c.ClothType_Name from Weaver_Wages_Cooly_Details a LEFT OUTER JOIN ClothType_Head c ON a.ClothType_IdNo = c.ClothType_IdNo Where a.Weaver_Wages_Code = '" & Trim(NewCode) & "' Order by Sl_No", con)
    '        dt3 = New DataTable
    '        da2.Fill(dt3)

    '        With dgv_Wages_Details

    '            .Rows.Clear()
    '            sno = 0

    '            If dt3.Rows.Count > 0 Then

    '                For i = 0 To dt3.Rows.Count - 1

    '                    n = .Rows.Add()
    '                    sno = sno + 1
    '                    .Rows(n).Cells(0).Value = Val(sno)
    '                    .Rows(n).Cells(1).Value = Format(Val(dt3.Rows(i).Item("Meters").ToString), "########0.00")
    '                    .Rows(n).Cells(2).Value = dt3.Rows(i).Item("ClothType_Name").ToString
    '                    .Rows(n).Cells(3).Value = Format(Val(dt3.Rows(i).Item("Pick").ToString), "########0.00")
    '                    .Rows(n).Cells(4).Value = Format(Val(dt3.Rows(i).Item("Cooly").ToString), "########0.00")
    '                    .Rows(n).Cells(5).Value = Format(Val(dt3.Rows(i).Item("Amount").ToString), "########0.00")

    '                Next i

    '            End If

    '            If .RowCount = 0 Then .Rows.Add()

    '        End With

    '        Dt3.Clear()

    '    Else

    '        Da1 = New SqlClient.SqlDataAdapter("Select Int1, name2, Currency1 as PcsCk_Pick, sum(meters1) as Mtrs from " & Trim(Common_Procedures.ReportTempTable) & " a group by Int1, name2, Currency1 having sum(meters1) <> 0 Order by Int1, name2, Currency1 desc", con)
    '        Dt1 = New DataTable
    '        Da1.Fill(Dt1)

    '        If Dt1.Rows.Count > 0 Then

    '            For j = 0 To Dt1.Rows.Count - 1

    '                NoCalc_Status = True

    '                RatMtr = 0
    '                Amt = 0
    '                If Val(Dt1.Rows(j).Item("Int1").ToString) = 1 Then
    '                    RatMtr = RateMtrs1
    '                ElseIf Val(Dt1.Rows(j).Item("Int1").ToString) = 2 Then
    '                    RatMtr = RateMtrs2
    '                ElseIf Val(Dt1.Rows(j).Item("Int1").ToString) = 3 Then
    '                    RatMtr = RateMtrs3
    '                ElseIf Val(Dt1.Rows(j).Item("Int1").ToString) = 4 Then
    '                    RatMtr = RateMtrs4
    '                ElseIf Val(Dt1.Rows(j).Item("Int1").ToString) = 5 Then
    '                    RatMtr = RateMtrs5
    '                End If

    '                RatePerPick = 0
    '                If Clo_Pick <> 0 Then
    '                    RatePerPick = RatMtr / Clo_Pick
    '                End If

    '                PickRatMtr = Format(Val(Dt1.Rows(j).Item("PcsCk_Pick").ToString) * RatePerPick, "#########0.00")

    '                MtrInFld = 0
    '                MtrInFld_GKT = 0

    '                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then
    '                    MtrInFld_GKT = Val(Dt1.Rows(j).Item("Mtrs").ToString) * Val(Fldng) / 100
    '                    Amt = Format(Val(MtrInFld_GKT) * PickRatMtr, "##########0")
    '                elseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
    '                    MtrInFld = Format(Val(Dt1.Rows(j).Item("Mtrs").ToString) * Val(Fldng) / 100, "##########0.00")
    '                    Amt = Format(Val(MtrInFld) * PickRatMtr, "##########0")

    '                Else
    '                    MtrInFld = Format(Val(Dt1.Rows(j).Item("Mtrs").ToString) * Val(Fldng) / 100, "##########0")
    '                    Amt = Format(Val(MtrInFld) * PickRatMtr, "##########0")

    '                End If

    '                n = dgv_Wages_Details.Rows.Add()

    '                sno = sno + 1

    '                dgv_Wages_Details.Rows(n).Cells(0).Value = Val(sno)
    '                If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then
    '                    dgv_Wages_Details.Rows(n).Cells(1).Value = Format(Val(MtrInFld_GKT), "#########0.00")
    '                Else
    '                    dgv_Wages_Details.Rows(n).Cells(1).Value = Format(Val(MtrInFld), "#########0.00")
    '                End If

    '                dgv_Wages_Details.Rows(n).Cells(2).Value = Dt1.Rows(j).Item("name2").ToString
    '                dgv_Wages_Details.Rows(n).Cells(3).Value = Val(Dt1.Rows(j).Item("PcsCk_Pick").ToString)

    '                dgv_Wages_Details.Rows(n).Cells(4).Value = Format(PickRatMtr, "#########0.00")
    '                dgv_Wages_Details.Rows(n).Cells(5).Value = Format(Amt, "#########0.00")

    '            Next

    '        End If
    '        Dt1.Clear()

    '    End If



    '    NoCalc_Status = False
    '    Calculation_Total_Wages()
    '    NoCalc_Status = True

    '    Total_Meter = 0
    '    If dgv_WagesDetails_Total.RowCount > 0 Then
    '        Total_Meter = Val(dgv_WagesDetails_Total.Rows(0).Cells(1).Value)
    '    End If

    '    sno = 0
    '    dgv_ConsYarn_Details.Rows.Clear()

    '    If Trim(WagesCode) <> "" Then
    '        Da1 = New SqlClient.SqlDataAdapter("Select a.Meters, b.Count_Name, a.Rd_Sp, a.Pick, a.Width, a.Weight_Meter, a.Weight from Weaver_Wages_Yarn_Details a INNER JOIN count_head b on a.Count_IdNo = b.Count_IdNo Where a.Weaver_Wages_Code = '" & Trim(NewCode) & "' Order by a.Sl_No", con)
    '    Else
    '        Da1 = New SqlClient.SqlDataAdapter("Select '" & Trim(Wft_Cnt_Nm) & "' as Count_Name, " & Str(Val(Clo_RdSpc)) & " as Rd_Sp, " & Str(Val(Clo_Width)) & " as Width, " & Str(Val(Clo_Wgt_Mtr_Wft)) & " as Weight_Meter, Currency1 as Pick, sum(meters1) as Meters from " & Trim(Common_Procedures.ReportTempTable) & " a group by Currency1 having sum(meters1) <> 0 Order by Currency1 desc", con)
    '    End If

    '    Dt1 = New DataTable
    '    Da1.Fill(Dt1)

    '    If Dt1.Rows.Count > 0 Then

    '        For j = 0 To Dt1.Rows.Count - 1

    '            NoCalc_Status = True

    '            n = dgv_ConsYarn_Details.Rows.Add()

    '            MtrInFld = 0
    '            MtrInFld_GKT = 0

    '            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then
    '                MtrInFld_GKT = Val(Dt1.Rows(j).Item("Meters").ToString) * Val(Fldng) / 100
    '                dgv_ConsYarn_Details.Rows(n).Cells(0).Value = Format(Val(MtrInFld_GKT), "#########0.00")

    '            Else
    '                MtrInFld = Format(Val(Dt1.Rows(j).Item("Meters").ToString) * Val(Fldng) / 100, "##########0")
    '                dgv_ConsYarn_Details.Rows(n).Cells(0).Value = Format(Val(MtrInFld), "#########0.00")

    '            End If

    '            dgv_ConsYarn_Details.Rows(n).Cells(1).Value = Dt1.Rows(j).Item("Count_Name").ToString
    '            dgv_ConsYarn_Details.Rows(n).Cells(2).Value = Val(Dt1.Rows(j).Item("Rd_Sp").ToString)
    '            dgv_ConsYarn_Details.Rows(n).Cells(3).Value = Val(Dt1.Rows(j).Item("Pick").ToString)
    '            dgv_ConsYarn_Details.Rows(n).Cells(4).Value = Val(Dt1.Rows(j).Item("Width").ToString)

    '            If Trim(WagesCode) <> "" Then

    '                dgv_ConsYarn_Details.Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(j).Item("Weight_Meter").ToString), "#########0.000000")

    '            Else

    '                WtPerMtr_Pick = 0
    '                If Clo_Pick <> 0 Then
    '                    WtPerMtr_Pick = Val(Dt1.Rows(j).Item("Weight_Meter").ToString) / Clo_Pick
    '                End If
    '                WtPerMtr = Format(Val(Dt1.Rows(j).Item("Pick").ToString) * WtPerMtr_Pick, "#########0.000000")
    '                dgv_ConsYarn_Details.Rows(n).Cells(5).Value = Format(Val(WtPerMtr), "#########0.000000")

    '            End If

    '            dgv_ConsYarn_Details.Rows(n).Cells(6).Value = Format(Val(dgv_ConsYarn_Details.Rows(n).Cells(0).Value) * Val(dgv_ConsYarn_Details.Rows(n).Cells(5).Value), "#########0.000")

    '            NoCalc_Status = False
    '            Weight_Calculation(n, 0)
    '            NoCalc_Status = True

    '        Next

    '    End If
    '    Dt1.Clear()


    '    'Da1 = New SqlClient.SqlDataAdapter("Select " & Val(Total_Meter) & " as TotalMeter, b.*, c.Count_Name from Weaver_Cloth_Receipt_Head a, cloth_head b, count_head c where a.Weaver_ClothReceipt_Code = '" & Trim(lbl_RecCode.Text) & "' and a.cloth_idno = b.cloth_idno and a.count_idno = c.count_idno", con)
    '    'Dt1 = New DataTable
    '    'Da1.Fill(Dt1)

    '    'If Dt1.Rows.Count > 0 Then

    '    '    For j = 0 To Dt1.Rows.Count - 1

    '    '        NoCalc_Status = True

    '    '        n = dgv_ConsYarn_Details.Rows.Add()

    '    '        dgv_ConsYarn_Details.Rows(n).Cells(0).Value = Format(Val(Dt1.Rows(j).Item("TotalMeter").ToString), "#########0.00")
    '    '        dgv_ConsYarn_Details.Rows(n).Cells(1).Value = Dt1.Rows(j).Item("Count_Name").ToString
    '    '        dgv_ConsYarn_Details.Rows(n).Cells(2).Value = Dt1.Rows(j).Item("Cloth_ReedSpace").ToString
    '    '        dgv_ConsYarn_Details.Rows(n).Cells(3).Value = Dt1.Rows(j).Item("Cloth_Pick").ToString
    '    '        dgv_ConsYarn_Details.Rows(n).Cells(4).Value = Dt1.Rows(j).Item("Cloth_Width").ToString
    '    '        dgv_ConsYarn_Details.Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(j).Item("Weight_Meter_Weft").ToString), "#########0.000000")
    '    '        dgv_ConsYarn_Details.Rows(n).Cells(6).Value = Format(Val(Dt1.Rows(j).Item("TotalMeter").ToString) * Val(Dt1.Rows(j).Item("Weight_Meter_Weft").ToString), "#########0.000")

    '    '        NoCalc_Status = False
    '    '        Weight_Calculation(n, 0)
    '    '        NoCalc_Status = True

    '    '    Next

    '    'End If
    '    'Dt1.Clear()

    '    NoCalc_Status = False
    '    Calculation_Total_ConsumedYarnDetails()
    '    NoCalc_Status = True

    '    Grid_DeSelect()
    '    pnl_Back.Enabled = True
    '    pnl_Selection.Visible = False

    '    If dgv_Wages_Details.Rows.Count > 0 Then
    '        dgv_Wages_Details.Focus()
    '        dgv_Wages_Details.CurrentCell = dgv_Wages_Details.Rows(0).Cells(1)
    '    Else
    '        txt_Elogation.Focus()
    '    End If

    '    NoCalc_Status = False

    'End Sub

    Public Sub print_record() Implements Interface_MDIActions.print_record

        prn_FromNo = Trim(lbl_BillNo.Text)
        prn_ToNo = Trim(lbl_BillNo.Text)

        prn_WagesFrmt = Common_Procedures.settings.WeaverWages_Printing_Format

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1283" Then
            pnl_PrintOption.Visible = True
            pnl_Back.Enabled = False
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
            'printing_WeaverWages()

        End If

    End Sub

    Private Sub printing_WeaverWages()
        Dim da1 As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim NewCode As String = ""
        Dim OrdBy_FrmNo As Single = 0, OrdByToNo As Single = 0
        Dim ps As Printing.PaperSize

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Try

            OrdBy_FrmNo = Common_Procedures.OrderBy_CodeToValue(prn_FromNo)
            OrdByToNo = Common_Procedures.OrderBy_CodeToValue(prn_ToNo)

            da1 = New SqlClient.SqlDataAdapter("select * from Weaver_Wages_Head Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and for_orderby between " & Str(Format(Val(OrdBy_FrmNo), "########0.00")) & " and " & Str(Format(Val(OrdByToNo), "########0.00")) & " and Weaver_Wages_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and Weaver_Wages_Code NOT LIKE '" & Trim(PkCondition_GST) & "%' Order by for_orderby, Weaver_Wages_No, Weaver_Wages_Code", con)
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

            'Dim pkCustomSize1 As New System.Drawing.Printing.PaperSize("PAPER 8X6", 800, 600)
            'PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = pkCustomSize1
            'PrintDocument1.DefaultPageSettings.PaperSize = pkCustomSize1
            'PrintDocument1.DefaultPageSettings.Landscape = False
            For I = 0 To PrintDocument1.PrinterSettings.PaperSizes.Count - 1
                If PrintDocument1.PrinterSettings.PaperSizes(I).Kind = Printing.PaperKind.A4 Then
                    ps = PrintDocument1.PrinterSettings.PaperSizes(I)
                    PrintDocument1.DefaultPageSettings.PaperSize = ps
                    PrintDocument1.PrinterSettings.DefaultPageSettings.PaperSize = ps
                    Exit For
                End If
            Next

        End If

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
        prn_DetDt1.Clear()

        prn_Prev_HeadIndx = -100
        prn_HeadIndx = 0

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

            OrdBy_FrmNo = Common_Procedures.OrderBy_CodeToValue(prn_FromNo)
            OrdByToNo = Common_Procedures.OrderBy_CodeToValue(prn_ToNo)

            da1 = New SqlClient.SqlDataAdapter("select a.*, b.*, c.*, d.Cloth_Name from Weaver_Wages_Head a INNER JOIN Company_Head b ON a.Company_IdNo = b.Company_IdNo LEFT OUTER JOIN Ledger_Head c ON a.Ledger_IdNo = c.Ledger_Idno LEFT OUTER JOIN Cloth_Head d ON a.Cloth_IdNo = d.Cloth_Idno Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.for_orderby between " & Str(Format(Val(OrdBy_FrmNo), "########0.00")) & " and " & Str(Format(Val(OrdByToNo), "########0.00")) & " and a.Weaver_Wages_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "' and Weaver_Wages_Code NOT LIKE '" & Trim(PkCondition_GST) & "%' Order by a.for_orderby, a.Weaver_Wages_No, a.Weaver_Wages_Code", con)
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

        prn_PageCount = prn_PageCount + 1

        'prn_DetDt.Clear()

        'prn_PageNo = 0

        'prn_DetIndx = 0
        'prn_DetSNo = 0

        'prn_Tot_EBeam_Stk = 0
        'prn_Tot_Pavu_Stk = 0
        'prn_Tot_Yarn_Stk = 0
        'prn_Tot_Amt_Bal = 0

        If prn_Prev_HeadIndx <> prn_HeadIndx Then
            If Trim(UCase(Common_Procedures.settings.WeaverWages_Printing_Format)) <> "FORMAT-5" Then
                Weaver_AllStock_Ledger(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_IdNo").ToString), prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Date").ToString)
            End If
            prn_DetIndx1 = 0
        End If

        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then
        '    Da2 = New SqlClient.SqlDataAdapter("select Date1,  meters1 as for_OrderBy, name3 as Dc_Rec_No, sum(Int6) as EmptyBeam, sum(Meters6) as PavuMtrs, sum(weight1) as YarnWgt, sum(currency1) as amount from " & Trim(Common_Procedures.ReportTempTable) & " group by Date1,  meters1,  name3 having  sum(Int6)  <> 0 or sum(Meters6) <> 0 or sum(weight1) <> 0 or sum(currency1) <> 0 Order by Date1,  meters1, name3", con)
        '    prn_DetDt = New DataTable
        '    Da2.Fill(prn_DetDt)

        'Else
        'Da2 = New SqlClient.SqlDataAdapter("select Date1, Int3 as Ent_OrderBy, meters1 as for_OrderBy, name2 as Ref_No, name1 as Ref_Code, name3 as Dc_Rec_No, sum(Int6) as EmptyBeam, sum(Meters6) as PavuMtrs, sum(weight1) as YarnWgt, sum(currency1) as amount from " & Trim(Common_Procedures.ReportTempTable) & " group by Date1, Int3, meters1, name2, name1, name3 having sum(Int6)  <> 0 or sum(Meters6) <> 0 or sum(weight1) <> 0 or sum(currency1) <> 0 Order by Date1, Int3, meters1, name2, name1", con)
        'prn_DetDt = New DataTable
        'Da2.Fill(prn_DetDt)
        ' End If

        Da2 = New SqlClient.SqlDataAdapter("select Date1, Int3 as Ent_OrderBy, meters1 as for_OrderBy, name2 as Ref_No,  name3 as Dc_Rec_No, sum(Int6) as EmptyBeam, sum(Meters6) as PavuMtrs, sum(weight1) as YarnWgt, sum(currency1) as amount from " & Trim(Common_Procedures.ReportTempTable) & " group by Date1, Int3, meters1, name2, name3 having sum(Int6)  <> 0 or sum(Meters6) <> 0 or sum(weight1) <> 0 or sum(currency1) <> 0 Order by Date1, Int3, meters1, name2", con)
        prn_DetDt = New DataTable
        Da2.Fill(prn_DetDt)

        Da2 = New SqlClient.SqlDataAdapter("Select a.*  from Weaver_Cloth_Receipt_Head a  Where a.Weaver_Wages_Code = '" & Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Code").ToString) & "' Order by fOR_oRDERbY , Weaver_ClothReceipt_No", con)
        prn_DetDt1 = New DataTable
        Da2.Fill(prn_DetDt1)

        Debug.Print(prn_DetDt1.Rows.Count)

        If Trim(UCase(Common_Procedures.settings.WeaverWages_Printing_Format)) = "FORMAT-5" Then
            Get_Party_DC_No(prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Code").ToString)
            Printing_Format5(e)

        ElseIf Trim(UCase(prn_WagesFrmt)) = "FORMAT-1" Or Trim(UCase(prn_WagesFrmt)) = "FORMAT-1.2" Or Trim(UCase(prn_WagesFrmt)) = "FORMAT-1.3" Then

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
                Get_Party_DC_No(prn_HdDt.Rows(prn_HeadIndx).Item("Weaver_Wages_Code").ToString)
                Printing_Format4(e)
            Else
                Printing_Format1(e)
            End If


        Else

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then
                Printing_Format3(e)
            Else
                Printing_Format2(e)
            End If

        End If

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

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

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
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Name").ToString
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

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
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
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Name").ToString
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

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
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
                            NoofItems_PerPage = 40
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

                        pFont = New Font("Calibri", 10, FontStyle.Bold)
                        CurY = CurY + 8
                        Common_Procedures.Print_To_PrintDocument(e, "eF ªî£¬è ", LMargin + 10, CurY, 0, 0, pTFont)
                        Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Net_Amount").ToString, LMargin + C1 - 10, CurY, 1, 0, pFont)
                        pFont = New Font("Calibri", 10, FontStyle.Regular)

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
                        NoofItems_PerPage = 22 '15

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


            '   CurY = CurY + TxtHgt

            'Common_Procedures.Print_To_PrintDocument(e, "Ë™  ", LMargin + C1 + 10, CurY, 0, 0, pTFont)
            'Common_Procedures.Print_To_PrintDocument(e, "  :  " & prn_Tot_Yarn_StkSumry, LMargin + C1 + S1 + 20, CurY, 0, 0, pFont)

        Else
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt

        End If

      
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1044" Then
            CurY = CurY + TxtHgt
            CurY = CurY + TxtHgt
            Common_Procedures.Print_To_PrintDocument(e, "ð£˜® ¬èªò£Šð‹  ", LMargin + 10, CurY, 0, 0, pTFont)

        End If

      
        If Trim(UCase(prn_WagesFrmt)) = "FORMAT-2.2" Or Trim(UCase(prn_WagesFrmt)) = "FORMAT-2.3" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1020" Then '---- Sri Vijayalakshmi Spinners (Udamalpet)
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Name").ToString
        Else
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Company_Name").ToString
        End If

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1029" Then '---- Arul Kumaran Textiles (Somanur)
            Common_Procedures.Print_To_PrintDocument(e, "Authorised Signatory", PageWidth - 15, CurY, 1, 0, pFont)
        Else
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
        'cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(int3, Date1, name1, name2, meters1, name3, name4, name5, name6, Currency1) Select 11, a.Voucher_Date, a.Voucher_Code, a.Voucher_No, a.For_OrderBy, a.Voucher_No, tP.Ledger_Name, replace(left(a.Entry_Identification, len(a.Entry_Identification)-6),'-' + cast(a.Company_Idno as varchar) + '-','-') as Particularss, 'AMOUNT', abs(a.Voucher_Amount) from Voucher_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = tP.Ledger_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Voucher_Date between @fromdate and @todate and a.Voucher_Amount > 0 "
        'cmd.ExecuteNonQuery()
        cmd.CommandText = "Insert into " & Trim(Common_Procedures.ReportTempTable) & "(int3, Date1, name1, name2, meters1, name3, name4, name5, name6, Currency1) Select 2, a.Voucher_Date, a.Voucher_Code, a.Voucher_No, a.For_OrderBy, a.Voucher_No, tP.Ledger_Name, replace(left(a.Entry_Identification, len(a.Entry_Identification)-6),'-' + cast(a.Company_Idno as varchar) + '-','-') as Particularss, 'AMOUNT', abs(a.Voucher_Amount) from Voucher_Details a INNER JOIN Company_Head tZ ON a.Company_IdNo <> 0 and a.Company_IdNo = tZ.Company_IdNo INNER JOIN Ledger_Head tP ON a.Ledger_IdNo <> 0 and a.Ledger_IdNo = tP.Ledger_IdNo Where " & SqlCondt & IIf(Trim(SqlCondt) <> "", " and ", "") & " a.Voucher_Date between @fromdate and @todate and a.Voucher_Amount > 0 "
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

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If
    End Sub

    Private Sub btn_Print_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Cancel.Click
        btn_Close_PrintOption_Click(sender, e)
    End Sub

    Private Sub btn_Close_PrintOption_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_PrintOption.Click
        pnl_Back.Enabled = True
        pnl_PrintOption.Visible = False
    End Sub

    Private Sub btn_Print_WithStock_WithName_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_WithStock_WithName.Click
        prn_WagesFrmt = "FORMAT-2.2"

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then
            txt_PrintRange_FromNo.Text = prn_FromNo
            txt_PrintRange_ToNo.Text = prn_ToNo

            pnl_PrintRange.Visible = True
            pnl_Back.Enabled = False
            pnl_PrintOption.Visible = False

            If txt_PrintRange_FromNo.Enabled Then txt_PrintRange_FromNo.Focus()

        ElseIf Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1283" Then '---- ARUL  MURUGAN TEXTILES (SOMANUR)

          
            txt_PrintRange_FromNo.Text = prn_FromNo
            txt_PrintRange_ToNo.Text = prn_ToNo

            pnl_PrintRange.Visible = True
            pnl_Back.Enabled = False
            pnl_PrintOption.Visible = False

            If txt_PrintRange_FromNo.Enabled Then txt_PrintRange_FromNo.Focus()

        Else
            printing_WeaverWages()
            btn_Close_PrintOption_Click(sender, e)

        End If

    End Sub

    Private Sub btn_Print_WithStock_WithoutName_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_WithStock_WithoutName.Click
        prn_WagesFrmt = "FORMAT-2.3"

        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then

            txt_PrintRange_FromNo.Text = prn_FromNo
            txt_PrintRange_ToNo.Text = prn_ToNo

            pnl_PrintRange.Visible = True
            pnl_Back.Enabled = False
            pnl_PrintOption.Visible = False

            If txt_PrintRange_FromNo.Enabled Then txt_PrintRange_FromNo.Focus()

        Else
            printing_WeaverWages()
            btn_Close_PrintOption_Click(sender, e)

        End If

    End Sub

    Private Sub btn_Print_Simple_WithName_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Simple_WithName.Click
        prn_WagesFrmt = "FORMAT-1.2"
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then

            txt_PrintRange_FromNo.Text = prn_FromNo
            txt_PrintRange_ToNo.Text = prn_ToNo

            pnl_PrintRange.Visible = True
            pnl_Back.Enabled = False
            pnl_PrintOption.Visible = False

            If txt_PrintRange_FromNo.Enabled Then txt_PrintRange_FromNo.Focus()

        Else
            printing_WeaverWages()
            btn_Close_PrintOption_Click(sender, e)

        End If
    End Sub

    Private Sub btn_Print_Simple_WithOutName_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Print_Simple_WithOutName.Click
        prn_WagesFrmt = "FORMAT-1.3"
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1040" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1032" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Then

            txt_PrintRange_FromNo.Text = prn_FromNo
            txt_PrintRange_ToNo.Text = prn_ToNo

            pnl_PrintRange.Visible = True
            pnl_Back.Enabled = False
            pnl_PrintOption.Visible = False

            If txt_PrintRange_FromNo.Enabled Then txt_PrintRange_FromNo.Focus()

        Else
            printing_WeaverWages()
            btn_Close_PrintOption_Click(sender, e)

        End If
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

    Private Sub dtp_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtp_Date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
        If e.KeyValue = 38 Then txt_Paid_Amount.Focus()
        If e.KeyValue = 40 Then e.Handled = True : cbo_Weaver.Focus()
    End Sub

    Private Sub dtp_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_Date.KeyPress
        If Asc(e.KeyChar) = 13 Then e.Handled = True : cbo_Weaver.Focus()
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

    Private Sub cbo_Grid_Clothtype_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Clothtype.GotFocus

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothType_Head", "ClothType_Name", "", "(ClothType_IdNo = 0)")

    End Sub

    Private Sub cbo_Grid_Clothtype_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Grid_Clothtype.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Grid_Clothtype, Nothing, Nothing, "ClothType_Head", "ClothType_Name", "", "(ClothType_IdNo = 0)")
        vcbo_KeyDwnVal = e.KeyValue

        With dgv_Wages_Details

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
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim cLTH_Idno As Integer = 0
        Dim Rate As Single = 0
        Dim Pick As Single = 0
        '  Dim trpt_Idno As Integer = 0

        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Grid_Clothtype, Nothing, "ClothType_Head", "ClothType_Name", "", "(ClothType_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            cLTH_Idno = Common_Procedures.Cloth_NameToIdNo(con, Trim(lbl_Cloth.Text))

            da = New SqlClient.SqlDataAdapter("select a.* from cLOTH_hEAD a where a.cLOTH_idno = " & Str(Val(cLTH_Idno)) & "", con)
            dt = New DataTable
            da.Fill(dt)

            Rate = 0
            Pick = 0
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)(0).ToString) = False Then

                    Pick = Val(dt.Rows(0).Item("Cloth_Pick").ToString)

                    If Trim(UCase(cbo_Grid_Clothtype.Text)) = "SOUND" Then
                        Rate = Val(dt.Rows(0).Item("Wages_For_Type1").ToString)
                    ElseIf Trim(UCase(cbo_Grid_Clothtype.Text)) = "SECONDS" Then
                        Rate = Val(dt.Rows(0).Item("Wages_For_Type2").ToString)
                    ElseIf Trim(UCase(cbo_Grid_Clothtype.Text)) = "BITS" Then
                        Rate = Val(dt.Rows(0).Item("Wages_For_Type3").ToString)
                    ElseIf Trim(UCase(cbo_Grid_Clothtype.Text)) = "OTHERS" Then
                        Rate = Val(dt.Rows(0).Item("Wages_For_Type5").ToString)
                    ElseIf Trim(UCase(cbo_Grid_Clothtype.Text)) = "REJECT" Then
                        Rate = Val(dt.Rows(0).Item("Wages_For_Type4").ToString)
                    End If

                End If
            End If

            dt.Dispose()
            da.Dispose()

            If Val(Rate) <> 0 Then
                With dgv_Wages_Details
                    If Val(.Rows(.CurrentRow.Index).Cells(4).Value) = 0 Or Val(.Rows(.CurrentRow.Index).Cells(3).Value) = 0 Then
                        .Rows(.CurrentRow.Index).Cells(3).Value = Pick
                        .Rows(.CurrentRow.Index).Cells(4).Value = Rate
                    End If
                End With
            End If

            With dgv_Wages_Details

                .Focus()
                .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)

            End With

        End If

    End Sub
    Private Sub cbo_Grid_Clothtype_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Grid_Clothtype.TextChanged
        Try
            If cbo_Grid_Clothtype.Visible Then

                If IsNothing(dgv_Wages_Details.CurrentCell) Then Exit Sub
                With dgv_Wages_Details
                    If Val(cbo_Grid_Clothtype.Tag) = Val(.CurrentCell.RowIndex) And .CurrentCell.ColumnIndex = 2 Then
                        .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(cbo_Grid_Clothtype.Text)
                    End If
                End With
            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "FOR MOVING...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub


    Private Sub dgv_Wages_details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Wages_Details.CellEnter
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim rect As Rectangle

        With dgv_Wages_Details

            dgv_ActCtrlName = .Name
            If Val(.CurrentRow.Cells(0).Value) = 0 Then
                .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
            End If

            If e.ColumnIndex = 2 Then


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

    Private Sub dgv_WagesDetails_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Wages_Details.EditingControlShowing
        dgtxt_WagesDetails = CType(dgv_Wages_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgtxt_WagesDetails_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_WagesDetails.Enter
        dgv_ActCtrlName = dgv_Wages_Details.Name
        dgv_Wages_Details.EditingControl.BackColor = Color.Lime
        dgv_Wages_Details.EditingControl.ForeColor = Color.Blue
        dgtxt_WagesDetails.SelectAll()
    End Sub

    Private Sub dgtxt_WagesDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgtxt_WagesDetails.KeyPress

        With dgv_Wages_Details
            If .Visible Then

                If Common_Procedures.Accept_NegativeNumbers(Asc(e.KeyChar)) = 0 Then
                    e.Handled = True
                End If

            End If
        End With

    End Sub

    Private Sub dgv_Receipt_details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Receipt_Details.CellEnter
        With dgv_Wages_Details

            dgv_ActCtrlName = .Name
            'If Val(.CurrentRow.Cells(0).Value) = 0 Then
            '    .CurrentRow.Cells(0).Value = .CurrentRow.Index + 1
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

    Private Sub dgv_Wages_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Wages_Details.CellLeave
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt As New DataTable
        Dim cLTH_Idno As Integer = 0
        Dim Rate As Single = 0
        Dim RatePerPick As Single = 0
        Dim Pick As Single = 0

        With dgv_Wages_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 3 And Val(.CurrentRow.Cells(4).Value) <> 0 Then

                    cLTH_Idno = Common_Procedures.Cloth_NameToIdNo(con, Trim(lbl_Cloth.Text))

                    da = New SqlClient.SqlDataAdapter("select a.* from cLOTH_hEAD a where a.cLOTH_idno = " & Str(Val(cLTH_Idno)) & "", con)
                    dt = New DataTable
                    da.Fill(dt)

                    Rate = 0
                    Pick = 0
                    RatePerPick = 0
                    If dt.Rows.Count > 0 Then
                        If IsDBNull(dt.Rows(0)(0).ToString) = False Then

                            Pick = Val(dt.Rows(0).Item("Cloth_Pick").ToString)

                            If Trim(UCase(cbo_Grid_Clothtype.Text)) = "SOUND" Then
                                Rate = Val(dt.Rows(0).Item("Wages_For_Type1").ToString)
                            ElseIf Trim(UCase(cbo_Grid_Clothtype.Text)) = "SECONDS" Then
                                Rate = Val(dt.Rows(0).Item("Wages_For_Type2").ToString)
                            ElseIf Trim(UCase(cbo_Grid_Clothtype.Text)) = "BITS" Then
                                Rate = Val(dt.Rows(0).Item("Wages_For_Type3").ToString)
                            ElseIf Trim(UCase(cbo_Grid_Clothtype.Text)) = "OTHERS" Then
                                Rate = Val(dt.Rows(0).Item("Wages_For_Type5").ToString)
                            ElseIf Trim(UCase(cbo_Grid_Clothtype.Text)) = "REJECT" Then
                                Rate = Val(dt.Rows(0).Item("Wages_For_Type4").ToString)
                            End If

                        End If
                    End If

                    dt.Dispose()
                    da.Dispose()

                    RatePerPick = 0
                    If Pick <> 0 Then
                        RatePerPick = Rate / Pick
                    End If

                    'If Val(Rate) <> 0 Then
                    '    With dgv_Wages_Details
                    '        If Val(.Rows(.CurrentRow.Index).Cells(4).Value) = 0 Then
                    '            .Rows(.CurrentRow.Index).Cells(4).Value = Rate
                    '        End If
                    '    End With
                    'End If

                    .CurrentRow.Cells(4).Value = Format(Val(.CurrentRow.Cells(3).Value) * Val(RatePerPick), "#########0.00")
                    .CurrentRow.Cells(5).Value = Format(Val(.CurrentRow.Cells(1).Value) * Val(.CurrentRow.Cells(4).Value), "#########0.00")

                End If
            End If

            If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 4 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If

                .CurrentRow.Cells(5).Value = Format(Val(.CurrentRow.Cells(1).Value) * Val(.CurrentRow.Cells(4).Value), "#########0.00")

            End If
        End With
    End Sub

    Private Sub dgv_Wages_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Wages_Details.CellValueChanged
        On Error Resume Next
        If IsNothing(dgv_Wages_Details.CurrentCell) Then Exit Sub
        With dgv_Wages_Details
            If .Visible Then

                If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 4 Then
                    .CurrentRow.Cells(5).Value = Format(Val(.CurrentRow.Cells(1).Value) * Val(.CurrentRow.Cells(4).Value), "#########0.00")
                End If

                If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 4 Then

                    Calculation_Total_Wages()

                End If

            End If
        End With
    End Sub

    Private Sub dgv_Reeipt_Details_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Receipt_Details.CellLeave
        With dgv_Receipt_Details
            If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
                If Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value) <> 0 Then
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = Format(Val(.CurrentRow.Cells(.CurrentCell.ColumnIndex).Value), "#########0.00")
                Else
                    .CurrentRow.Cells(.CurrentCell.ColumnIndex).Value = ""
                End If
            End If
        End With
    End Sub

    Private Sub dgv_Receipt_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Receipt_Details.CellValueChanged

        On Error Resume Next


        If IsNothing(dgv_Receipt_Details.CurrentCell) Then Exit Sub
        With dgv_Receipt_Details
            If .Visible Then

                If .CurrentCell.ColumnIndex = 1 Or .CurrentCell.ColumnIndex = 2 Or .CurrentCell.ColumnIndex = 3 Or .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
                    ' Excess_Short_Calculation()
                    ' Weight_Calculation(.CurrentCell.RowIndex, .CurrentCell.ColumnIndex)
                End If

                If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Then
                    Calculation_Total_ReceiptMeter()
                End If

            End If
        End With
    End Sub

    Private Sub dgtxt_Details_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_Details.TextChanged
        Try
            With dgv_ConsYarn_Details
                If .Rows.Count > 0 Then
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_Details.Text)
                End If
            End With

        Catch ex As Exception
            '---

        End Try
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

    Private Sub dgtxt_WagesDetails_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgtxt_WagesDetails.TextChanged
        Try
            With dgv_Wages_Details
                If .Rows.Count > 0 Then
                    .Rows(.CurrentCell.RowIndex).Cells.Item(.CurrentCell.ColumnIndex).Value = Trim(dgtxt_WagesDetails.Text)
                End If
            End With

        Catch ex As Exception
            '---
        End Try

    End Sub

    Private Sub dgv_Wages_details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Wages_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Wages_Details.CurrentCell) Then dgv_Wages_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Receipt_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Receipt_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Receipt_Details.CurrentCell) Then dgv_Receipt_Details.CurrentCell.Selected = False
    End Sub

    Private Sub btn_SaveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SaveAll.Click
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

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

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
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Name").ToString
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
    Private Sub Printing_Format5_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single)
        Dim p1Font As Font
        Dim Cmp_Name As String, Cmp_Add1 As String, Cmp_Add2 As String, Cmp_panNo As String
        Dim Cmp_PhNo As String, Cmp_TinNo As String, Cmp_CstNo As String
        Dim Led_Name As String, Led_TinNo As String, Led_Add1 As String = "", Led_Add2 As String = "", Led_Add3 As String = "", Led_Add4 As String = ""


        Dim strHeight As Single
        Dim C1, C2, S1, W1, W2 As Single


        PageNo = PageNo + 1

        CurY = TMargin


        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(1) = CurY

        Cmp_Name = "" : Cmp_Add1 = "" : Cmp_Add2 = "" : Cmp_panNo = ""
        Cmp_PhNo = "" : Cmp_TinNo = "" : Cmp_CstNo = ""


        Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Name").ToString
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
        p1Font = New Font("Calibri", 16, FontStyle.Bold)
        Common_Procedures.Print_To_PrintDocument(e, "JOBWORK INVOICE", LMargin, CurY, 2, PrintWidth, p1Font)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_TinNo, LMargin + 10, CurY, 0, 0, pFont)
        Common_Procedures.Print_To_PrintDocument(e, Cmp_panNo, PageWidth - 10, CurY, 1, 0, pFont)
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_CstNo, PageWidth - 10, CurY, 1, 0, pFont)
        'CurY = CurY + TxtHgt - 1
        'Common_Procedures.Print_To_PrintDocument(e, Cmp_panNo, LMargin + 10, CurY, 0, 0, pFont)
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


        If Trim(prn_HdDt.Rows(prn_HeadIndx).Item("Company_TinNo").ToString) <> "" Then
            CurY = CurY + TxtHgt
            Led_TinNo = "     TIN NO.: " & prn_HdDt.Rows(prn_HeadIndx).Item("Company_TinNo").ToString
            Common_Procedures.Print_To_PrintDocument(e, " " & Led_TinNo, LMargin + 10, CurY, 0, 0, p1Font)
        End If
        CurY = CurY + TxtHgt + 10
        ' e.Graphics.DrawLine(Pens.Black, LMargin + C1, LnAr(3), LMargin + C1, LnAr(2))

        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(3) = CurY
        e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3), LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(2))
        CurY = CurY + TxtHgt - 10
        Common_Procedures.Print_To_PrintDocument(e, "SNO", LMargin, CurY, 2, ClAr(1), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "DESCRIPTION", LMargin + ClAr(1), CurY, 2, ClAr(2), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "TYPE", LMargin + ClAr(1) + ClAr(2), CurY, 2, ClAr(3), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, 2, ClAr(4), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "RATE/MTRS", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, 2, ClAr(5), pFont)
        Common_Procedures.Print_To_PrintDocument(e, "AMOUNT", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, 2, ClAr(6), pFont)

        CurY = CurY + TxtHgt - 10



        CurY = CurY + TxtHgt
        e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
        LnAr(4) = CurY

    End Sub

    Private Sub dgv_Wages_Details_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Wages_Details.CellContentClick

    End Sub

    Private Sub Printing_Format5(ByRef e As System.Drawing.Printing.PrintPageEventArgs)
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


        pFont = New Font("Calibri", 11, FontStyle.Regular)

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

        TxtHgt = 19

        EntryCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_BillNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        NoofItems_PerPage = 15

        Try

            prn_Prev_HeadIndx = prn_HeadIndx

            If prn_HdDt.Rows.Count > 0 Then

                Printing_Format5_PageHeader(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, prn_PageNo, NoofItems_PerPage, CurY, LnAr, ClAr)

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
                    Common_Procedures.Print_To_PrintDocument(e, "Frieght", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Freight_Charge").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                    NoofDets = NoofDets + 1
                End If
                If prn_HdDt.Rows(prn_HeadIndx).Item("Add_Amount").ToString <> 0 Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "Add", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Add_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                    NoofDets = NoofDets + 1
                End If
                If prn_HdDt.Rows(prn_HeadIndx).Item("Less_Amount").ToString <> 0 Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "Less", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "(-)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Less_Amount").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                    NoofDets = NoofDets + 1
                End If
                If prn_HdDt.Rows(prn_HeadIndx).Item("Tds_Perc_Calc").ToString <> 0 Then
                    CurY = CurY + TxtHgt
                    Common_Procedures.Print_To_PrintDocument(e, "Tds " & prn_HdDt.Rows(prn_HeadIndx).Item("Tds_Perc").ToString & " % ", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, "(+)", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5) - 10, CurY, 1, 0, pFont)
                    Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Tds_Perc_Calc").ToString, PageWidth - 10, CurY, 1, 0, pFont)
                    NoofDets = NoofDets + 1
                End If

                NetAmt = Format(Val(prn_HdDt.Rows(prn_HeadIndx).Item("Total_Cooly").ToString) - Val(prn_HdDt.Rows(prn_HeadIndx).Item("Freight_Charge").ToString) + Val(prn_HdDt.Rows(prn_HeadIndx).Item("Add_Amount").ToString) - Val(prn_HdDt.Rows(prn_HeadIndx).Item("Less_Amount").ToString) - Val(prn_HdDt.Rows(prn_HeadIndx).Item("Tds_Perc_Calc").ToString), "#########0.00")
                'With dgv_WagesDetails_Total
                '    If .Rows.Count > 0 Then
                '        TotAmt = Val(.Rows(prn_HeadIndx).Cells(5).Value)
                '    End If
                'End With
                'NetAmt = Format(Val(TotAmt) - Val(prn_HdDt.Rows(prn_HeadIndx).Item("Freight_Charge").ToString) + Val(prn_HdDt.Rows(prn_HeadIndx).Item("Add_Amount").ToString) - Val(prn_HdDt.Rows(prn_HeadIndx).Item("Less_Amount").ToString) - Val(prn_HdDt.Rows(prn_HeadIndx).Item("Tds_Perc_Calc").ToString), "#########0.00")

                rndoff = 0
                rndoff = Val(prn_HdDt.Rows(prn_HeadIndx).Item("Net_Amount").ToString) - Val(NetAmt)

                CurY = CurY + TxtHgt + 1
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

            CurY = CurY + TxtHgt + 5
            e.Graphics.DrawLine(Pens.Black, LMargin, CurY, PageWidth, CurY)
            LnAr(5) = CurY

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1), CurY, LMargin + ClAr(1), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2), CurY, LMargin + ClAr(1) + ClAr(2), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3), LnAr(3))
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4), LnAr(3))


            CurY = CurY + TxtHgt - 10
            p1Font = New Font("Calibri", 11, FontStyle.Bold)
            Common_Procedures.Print_To_PrintDocument(e, "Net Amount", LMargin + ClAr(1) + ClAr(2) + ClAr(3) + 10, CurY, 0, 0, p1Font)
            Common_Procedures.Print_To_PrintDocument(e, prn_HdDt.Rows(prn_HeadIndx).Item("Net_Amount").ToString, PageWidth - 10, CurY, 1, 0, p1Font)

            CurY = CurY + TxtHgt + 10
            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), LnAr(3))

            e.Graphics.DrawLine(Pens.Black, LMargin + ClAr(1) + ClAr(2) + ClAr(3) + ClAr(4) + ClAr(5), CurY, PageWidth, CurY)


            'Printing_Format5_PageFooter(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr, ClAr, NoofDets, True)

            CurY = CurY + TxtHgt
            'CurY = CurY + TxtHgt
            'CurY = CurY + TxtHgt
            'CurY = CurY + TxtHgt

            Printing_Format5Delivery(e, EntryCode, TxtHgt, pFont, LMargin, RMargin, TMargin, BMargin, PageWidth, PrintWidth, NoofItems_PerPage, CurY, LnAr)

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
    Private Sub Printing_Format5Delivery_PageHeader(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByRef PageNo As Integer, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClArr() As Single)
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

    Private Sub Printing_Format5Delivery_PageFooter(ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByRef CurY As Single, ByRef LnAr() As Single, ByRef ClAr() As Single, ByVal NoofDets As Integer, ByVal is_LastPage As Boolean)
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
            Cmp_Name = prn_HdDt.Rows(prn_HeadIndx).Item("Ledger_Name").ToString

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
                smstxt = smstxt & " Bill Date : " & Trim(msk_Date.Text) & Chr(13)
            Else
                smstxt = smstxt & " Bill No : " & Trim(lbl_BillNo.Text) & Chr(13)
                smstxt = smstxt & " Date : " & Trim(msk_Date.Text) & Chr(13)
            End If

            smstxt = smstxt & " Quality : " & Trim(lbl_Cloth.Text) & Chr(13)

            If dgv_ReceiptDetails_Total.RowCount > 0 Then
                smstxt = smstxt & " Receipt Meters : " & Val(dgv_ReceiptDetails_Total.Rows(0).Cells(5).Value()) & Chr(13)
            End If

            If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1044" Then
                If dgv_Wages_Details.RowCount > 0 Then
                    For i = 0 To dgv_Wages_Details.RowCount - 1
                        If Trim(dgv_Wages_Details.Rows(i).Cells(2).Value()) = "SOUND" Then
                            smstxt = smstxt & " Bill Meters : " & Val(dgv_Wages_Details.Rows(i).Cells(1).Value()) & Chr(13)
                        End If
                    Next

                End If
                smstxt = smstxt & " Excess/Short Meters : " & Val(lbl_Excess_Short.Text) & Chr(13)
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
            f1.MdiParent = MDIParent1
            f1.Show()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT SEND SMS...", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Printing_Format5Delivery(ByVal e As Printing.PrintPageEventArgs, ByVal EntryCode As String, ByVal TxtHgt As Single, ByVal pFont As Drawing.Font, ByVal LMargin As Single, ByVal RMargin As Single, ByVal TMargin As Single, ByVal BMargin As Single, ByVal PageWidth As Single, ByVal PrintWidth As Single, ByVal NoofItems_PerPage As Integer, ByVal CurY As Single, ByVal LnAr As Single())
        Throw New NotImplementedException
    End Sub

    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyDown
        If e.KeyCode = 38 Then
            e.Handled = True : e.SuppressKeyPress = True
        End If
        If e.KeyCode = 40 Then
            e.SuppressKeyPress = True
            cbo_Weaver.Focus()
        End If

        vmskOldText = ""
        vmskSelStrt = -1
        If e.KeyCode = 47 Or e.KeyCode = 8 Then
            vmskOldText = msk_Date.Text
            vmskSelStrt = msk_Date.SelectionStart
        End If
    End Sub

    Private Sub msk_Date_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles msk_Date.KeyPress
        If Trim(UCase(e.KeyChar)) = "D" Then
            msk_Date.Text = Date.Today
            msk_Date.SelectionStart = 0
        End If
        If Asc(e.KeyChar) = 13 Then e.Handled = True : cbo_Weaver.Focus()
    End Sub

    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_Date.KeyUp
        Dim vmskRetText As String = ""
        Dim vmskRetStrt As Integer = -1

        If e.KeyCode = 107 Then
            msk_Date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_Date.Text))
            msk_Date.SelectionStart = 0
        End If
        If e.KeyCode = 109 Then
            msk_Date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_Date.Text))
            msk_Date.SelectionStart = 0
        End If

        If e.KeyCode = 8 Or e.KeyCode = 46 Then
            Common_Procedures.maskEdit_Date_ON_DelBackSpace(sender, e, vmskOldText, vmskSelStrt)
        End If
    End Sub

    Private Sub msk_Date_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_Date.LostFocus
        If IsDate(msk_Date.Text) = True Then
            If Microsoft.VisualBasic.Day(Convert.ToDateTime(msk_Date.Text)) <= 31 And Microsoft.VisualBasic.Month(Convert.ToDateTime(msk_Date.Text)) <= 31 Then
                If Microsoft.VisualBasic.Year(Convert.ToDateTime(msk_Date.Text)) <= 2050 And Microsoft.VisualBasic.Year(Convert.ToDateTime(msk_Date.Text)) >= 2010 Then
                    dtp_Date.Value = Convert.ToDateTime(msk_Date.Text)
                End If
            End If
        End If
    End Sub

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        If IsDate(dtp_Date.Text) = True Then
            msk_Date.Text = dtp_Date.Text
            msk_Date.SelectionStart = 0
        End If
    End Sub

    Private Sub msk_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles msk_Date.TextChanged
        'If IsDate(msk_Date.Text) = True Then
        '    dtp_Date.Text = msk_Date.Text
        'End If
    End Sub


End Class
