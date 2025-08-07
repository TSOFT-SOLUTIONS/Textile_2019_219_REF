Public Class Piece_Transfer
    Implements Interface_MDIActions

    Private con As New SqlClient.SqlConnection(Common_Procedures.Connection_String)
    Private con1 As New SqlClient.SqlConnection(Common_Procedures.ConnectionString_CompanyGroupdetails)
    Private FrmLdSTS As Boolean = False
    Private New_Entry As Boolean = False
    Private Insert_Entry As Boolean = False
    Private Filter_Status As Boolean = False
    Private SaveAll_STS As Boolean = False
    Private Pk_Condition As String = "PCSTR-"
    Private NoCalc_Status As Boolean = False
    Private Prec_ActCtrl As New Control
    Private vcbo_KeyDwnVal As Double

    Private LastNo As String = ""

    Private vCLO_CONDT As String = ""
    Private FnYearCode1 As String = ""
    Private FnYearCode2 As String = ""

    Private WithEvents dgtxt_Details As New DataGridViewTextBoxEditingControl

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub clear()

        NoCalc_Status = True

        New_Entry = False
        Insert_Entry = False

        Pnl_Back.Enabled = True
        pnl_Filter.Visible = False
        pnl_Selection.Visible = False
        chk_SelectAll.Checked = False

        lbl_RefNo.Text = ""
        lbl_RefNo.ForeColor = Color.Black

        msk_date.Text = ""
        dtp_Date.Text = ""
        cbo_ClothFrom.Text = ""
        cbo_ClothTypeTo.Text = Common_Procedures.ClothType_IdNoToName(con, 1)
        cbo_ClothFrom.Text = ""
        cbo_ClothTo.Text = ""
        chk_SelectAll.Checked = False
        lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Common_Procedures.User.IdNo)))
        txt_FoldingFrom.Text = 100
        txt_FoldingTo.Text = 100
        txt_Note.Text = ""

        txt_LotSelction.Text = ""
        txt_PcsSelction.Text = ""

        cbo_ClothSales_OrderCode_forSelection_From.Text = ""
        cbo_ClothSales_OrderCode_forSelection_To.Text = ""

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

        msk_date.Enabled = True
        msk_date.BackColor = Color.White
        dtp_Date.Enabled = True


        cbo_PartyFrom.Enabled = True
        cbo_PartyFrom.BackColor = Color.White

        cbo_PartyTo.Enabled = True
        cbo_PartyTo.BackColor = Color.White

        cbo_Godown_StockFROM.Enabled = True
        cbo_Godown_StockFROM.BackColor = Color.White

        cbo_Godown_StockTO.Enabled = True
        cbo_Godown_StockTO.BackColor = Color.White

        cbo_ClothFrom.Enabled = True
        cbo_ClothFrom.BackColor = Color.White

        cbo_ClothTo.Enabled = True
        cbo_ClothTo.BackColor = Color.White

        cbo_ClothTypeTo.Enabled = True
        cbo_ClothTypeTo.BackColor = Color.White

        txt_FoldingFrom.Enabled = True
        txt_FoldingFrom.BackColor = Color.White

        txt_FoldingTo.Enabled = True
        txt_FoldingTo.BackColor = Color.White

        btn_Selection.Enabled = True

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
            Msktxbx.SelectAll()
        End If

        If Me.ActiveControl.Name <> dgv_Details_Total.Name Then
            Grid_Cell_DeSelect()
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

    Private Sub Piece_Transfer_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyFrom.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyFrom.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_PartyTo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_PartyTo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Godown_StockFROM.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Godown_StockFROM.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_Godown_StockTO.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "LEDGER" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_Godown_StockTO.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ClothFrom.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ClothFrom.Text = Trim(Common_Procedures.Master_Return.Return_Value)
            End If

            If Trim(UCase(Common_Procedures.Master_Return.Form_Name)) = Trim(UCase(Me.Name)) And Trim(UCase(Common_Procedures.Master_Return.Control_Name)) = Trim(UCase(cbo_ClothTo.Name)) And Trim(UCase(Common_Procedures.Master_Return.Master_Type)) = "CLOTH" And Trim(Common_Procedures.Master_Return.Return_Value) <> "" Then
                cbo_ClothTo.Text = Trim(Common_Procedures.Master_Return.Return_Value)
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

                lbl_Company.Text = Common_Procedures.get_Company_From_CompanySelection(con)
                lbl_Company.Tag = Val(Common_Procedures.CompIdNo)

                Me.Text = lbl_Company.Text

                new_record()

            End If

        Catch ex As Exception
            'MessageBox.Show(ex.Message, "DOES NOT SHOW...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try

        FrmLdSTS = False
    End Sub

    Private Sub Piece_Transfer_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        con.Close()
        con.Dispose()
    End Sub

    Private Sub Piece_Transfer_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If Asc(e.KeyChar) = 27 Then

                If pnl_Filter.Visible = True Then
                    btn_Filter_Close_Click(sender, e)
                    Exit Sub

                ElseIf pnl_Selection.Visible = True Then
                    btn_Close_Selection_Click(sender, e)
                    Exit Sub

                Else
                    Close_Form()

                End If

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT CLOSE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub Piece_Transfer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim dt5 As New DataTable

        dgv_Details.Columns(1).HeaderText = Trim(UCase(Common_Procedures.settings.ClothReceipt_LotNo_OR_RollNo_Text))
        dgv_Details.Columns(8).HeaderText = "NEW " & Trim(UCase(Common_Procedures.settings.ClothReceipt_LotNo_OR_RollNo_Text))
        dgv_Selection.Columns(1).HeaderText = Trim(UCase(Common_Procedures.settings.ClothReceipt_LotNo_OR_RollNo_Text))

        Me.Text = ""

        con.Open()

        da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
        da.Fill(dt1)
        cbo_ClothFrom.DataSource = dt1
        cbo_ClothFrom.DisplayMember = "Cloth_Name"

        da = New SqlClient.SqlDataAdapter("select ClothType_Name from ClothType_Head WHERE (ClothType_IdNo = 0 or ClothType_IdNo Between 1 and 5) order by ClothType_Name", con)
        da.Fill(dt2)
        cbo_ClothTypeTo.DataSource = dt2
        cbo_ClothTypeTo.DisplayMember = "ClothType_Name"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1) order by Ledger_DisplayName", con)
        da.Fill(dt3)
        cbo_PartyFrom.DataSource = dt3
        cbo_PartyFrom.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Ledger_DisplayName from Ledger_AlaisHead where (Ledger_IdNo = 0 or Ledger_Type = 'GODOWN' or Ledger_Type = 'JOBWORKER' or Show_In_All_Entry = 1) order by Ledger_DisplayName", con)
        da.Fill(dt4)
        cbo_PartyFrom.DataSource = dt4
        cbo_PartyFrom.DisplayMember = "Ledger_DisplayName"

        da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
        da.Fill(dt5)
        cbo_ClothTo.DataSource = dt5
        cbo_ClothTo.DisplayMember = "Cloth_Name"

        dtp_Date.Text = ""
        msk_date.Text = ""

        pnl_Filter.Visible = False
        pnl_Filter.Left = (Me.Width - pnl_Filter.Width) \ 2
        pnl_Filter.Top = (Me.Height - pnl_Filter.Height) \ 2

        pnl_Selection.Visible = False
        pnl_Selection.Left = (Me.Width - pnl_Selection.Width) \ 2
        pnl_Selection.Top = (Me.Height - pnl_Selection.Height) \ 2
        pnl_Selection.BringToFront()

        btn_SaveAll.Visible = False
        'If Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1087" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1234" Or Trim(UCase(Common_Procedures.settings.CustomerCode)) = "1267" Then
        '    btn_SaveAll.Visible = True
        'End If

        btn_UserModification.Visible = False
        If Common_Procedures.settings.User_Modifications_Show_Status = 1 Then
            If Val(Common_Procedures.User.IdNo) = 1 Or Common_Procedures.User.Show_UserModification_Status = 1 Then
                btn_UserModification.Visible = True
            End If
        End If


        cbo_PartyFrom.Visible = False
        lbl_PartyFrom_Caption1.Visible = False
        lbl_PartyFrom_Caption2.Visible = False
        lbl_PartyFrom_Asterisk.Visible = False

        cbo_PartyTo.Visible = False
        lbl_PartyTo_Caption1.Visible = False
        lbl_PartyTo_Caption2.Visible = False
        lbl_PartyTo_Asterisk.Visible = False

        cbo_Godown_StockFROM.Visible = False
        lbl_Godown_StockFROM_Caption.Visible = False
        lbl_GodownFrom_Asterisk.Visible = False

        cbo_Godown_StockTO.Visible = False
        lbl_Godown_StockTO_Caption.Visible = False
        lbl_GodownTo_Asterisk.Visible = False


        If Common_Procedures.settings.JOBWORKENTRY_Status = 1 Then

            cbo_PartyFrom.Visible = True
            lbl_PartyFrom_Caption1.Visible = True
            lbl_PartyFrom_Caption2.Visible = True
            lbl_PartyFrom_Asterisk.Visible = True
            cbo_PartyTo.Visible = True
            lbl_PartyTo_Caption1.Visible = True
            lbl_PartyTo_Caption2.Visible = True
            lbl_PartyTo_Asterisk.Visible = True

            'lbl_PartyTo_Caption1.Size = New Size(94, 50)
            'lbl_PartyFrom_Caption1.Size = New Size(94, 50)

            'lbl_PartyFrom_Caption1.Text = "Party From  " &
            '               "(Ownsort / " &
            '               " Jobworker)"
            'lbl_PartyTo_Caption1.Text = "Party To " &
            '               "(Ownsort / " &
            '               " Jobworker)"

        Else

            If Common_Procedures.settings.Multi_Godown_Status = 0 Then

                lbl_ClothTo_Asterisk.Visible = False
                lbl_ClothTo_Caption.Location = New Point(5, 71)
                cbo_ClothTo.Location = New Point(113, 68)

                lbl_ClothTypeTo_Caption.Location = New Point(445, 70)
                cbo_ClothTypeTo.Location = New Point(548, 68)

                lbl_ClothFrom_Asterisk.Visible = False
                lbl_ClothFrom_Caption.Location = New Point(7, 42)
                cbo_ClothFrom.Location = New Point(113, 38)


                lbl_FoldingTo_Caption.Location = New Point(679, 42)
                txt_FoldingTo.Location = New Point(752, 38)
                btn_Selection.Location = New Point(834, 38)

                lbl_FoldingFrom_Caption.Location = New Point(442, 42)
                txt_FoldingFrom.Location = New Point(548, 38)

                dgv_Details.Location = New Point(16, 96)

                dgv_Details.Size = New Size(836, 206)

            End If

        End If


        If Common_Procedures.settings.Multi_Godown_Status = 1 Then

            cbo_Godown_StockTO.Visible = True
            lbl_Godown_StockTO_Caption.Visible = True
            lbl_GodownTo_Asterisk.Visible = True

            cbo_Godown_StockFROM.Visible = True
            lbl_Godown_StockFROM_Caption.Visible = True
            lbl_GodownFrom_Asterisk.Visible = True

            cbo_Godown_StockTO.TabIndex = 4
            cbo_Godown_StockFROM.TabIndex = 3

        End If


        If Common_Procedures.settings.Show_Sales_OrderNumber_in_ALLEntry_Status = 1 Then

            lbl_Sales_OrderNo_From.Visible = True
            lbl_Sales_OrderNo_To.Visible = True
            cbo_ClothSales_OrderCode_forSelection_From.Visible = True
            cbo_ClothSales_OrderCode_forSelection_To.Visible = True

            FnYearCode1 = ""
            FnYearCode2 = ""
            Common_Procedures.get_FnYearCode_of_Last_2_Years(FnYearCode1, FnYearCode2)


            'If cbo_Sizing_JobCardNo.Visible = False Then

            '    lbl_Sales_OrderNo_From.Top = Label12.Bottom + 20
            '    lbl_Sales_OrderNo_To.Top = Label3.Bottom + 20
            '    cbo_ClothSales_OrderCode_forSelection_From.Top = txt_MetersFrom.Bottom + 20
            '    cbo_ClothSales_OrderCode_forSelection_To.Top = txt_MetersTo.Bottom + 20


            '    lbl_remarks.Top = lbl_weaving_job_no.Bottom + 20
            '    txt_remarks.Top = cbo_weaving_job_no.Bottom + 15

            'End If

        Else

            lbl_Sales_OrderNo_From.Visible = False
            lbl_Sales_OrderNo_To.Visible = False
            cbo_ClothSales_OrderCode_forSelection_From.Visible = False
            cbo_ClothSales_OrderCode_forSelection_To.Visible = False

        End If

        AddHandler msk_date.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothTypeTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothTo.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_FoldingFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_Note.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_Cloth.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Filter_ClothType.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyFrom.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PartyTo.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_save.GotFocus, AddressOf ControlGotFocus
        AddHandler btn_close.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_LotSelction.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_PcsSelction.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_PieceType_From_Selection.GotFocus, AddressOf ControlGotFocus
        AddHandler txt_FoldingTo.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Godown_StockFROM.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_Godown_StockTO.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothSales_OrderCode_forSelection_From.GotFocus, AddressOf ControlGotFocus
        AddHandler cbo_ClothSales_OrderCode_forSelection_To.GotFocus, AddressOf ControlGotFocus

        AddHandler cbo_PartyFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PartyTo.LostFocus, AddressOf ControlLostFocus
        AddHandler msk_date.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothTypeTo.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothTo.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_FoldingFrom.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_Note.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_PieceType_From_Selection.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_LotSelction.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_PcsSelction.LostFocus, AddressOf ControlLostFocus
        AddHandler txt_FoldingTo.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_ClothSales_OrderCode_forSelection_From.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_ClothSales_OrderCode_forSelection_To.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Godown_StockFROM.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Godown_StockTO.LostFocus, AddressOf ControlLostFocus

        AddHandler cbo_Filter_Cloth.LostFocus, AddressOf ControlLostFocus
        AddHandler cbo_Filter_ClothType.LostFocus, AddressOf ControlLostFocus

        AddHandler btn_save.LostFocus, AddressOf ControlLostFocus
        AddHandler btn_close.LostFocus, AddressOf ControlLostFocus

        AddHandler msk_date.KeyDown, AddressOf TextBoxControlKeyDown
        AddHandler msk_date.KeyPress, AddressOf TextBoxControlKeyPress
        AddHandler btn_Set_Bm_selection.KeyDown, AddressOf TextBoxControlKeyDown

        lbl_Company.Text = ""
        lbl_Company.Tag = 0
        lbl_Company.Visible = False
        Common_Procedures.CompIdNo = 0

        Filter_Status = False
        FrmLdSTS = True
        new_record()

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

            ElseIf Pnl_Back.Enabled = True Then
                dgv1 = dgv_Details

            End If

            If IsNothing(dgv1) = False Then

                With dgv1


                    If keyData = Keys.Enter Or keyData = Keys.Down Then
                        If .CurrentCell.ColumnIndex >= 13 Then

                            If .CurrentCell.RowIndex = .RowCount - 1 Then
                                txt_Note.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(8)

                            End If

                        ElseIf .CurrentCell.ColumnIndex = 9 Then
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 3)


                        ElseIf .CurrentCell.ColumnIndex = 6 Then
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 2)

                        Else
                            .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex + 1)

                        End If

                        Return True

                    ElseIf keyData = Keys.Up Then

                        If .CurrentCell.ColumnIndex <= 8 Then
                            If .CurrentCell.RowIndex = 0 Then

                                cbo_ClothTypeTo.Focus()

                            Else
                                .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(13)

                            End If

                        ElseIf .CurrentCell.ColumnIndex = 12 Then
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(9)


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
            If Trim(UCase(Common_Procedures.User.Type)) <> "UNACCOUNT" Then
                CompCondt = "Company_Type <> 'UNACCOUNT'"
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
        Dim dt3 As New DataTable
        Dim NewCode As String
        Dim n As Integer
        Dim j As Integer
        Dim SNo As Integer
        Dim LockSTS As Boolean = False

        If Val(no) = 0 Then Exit Sub

        clear()

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(no) & "/" & Trim(Common_Procedures.FnYearCode)

        Try
            da1 = New SqlClient.SqlDataAdapter("select a.* from Piece_Transfer_Head a Where a.Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and a.Piece_Transfer_Code = '" & Trim(NewCode) & "'", con)
            dt1 = New DataTable
            da1.Fill(dt1)

            If dt1.Rows.Count > 0 Then

                lbl_RefNo.Text = dt1.Rows(0).Item("Piece_Transfer_No").ToString
                dtp_Date.Text = dt1.Rows(0).Item("Piece_Transfer_Date").ToString
                msk_date.Text = dtp_Date.Text
                cbo_PartyFrom.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("LedgerFrom_IdNo").ToString))
                cbo_PartyTo.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("LedgerTo_IdNo").ToString))
                cbo_ClothFrom.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("ClothFrom_IdNo").ToString))
                cbo_ClothTo.Text = Common_Procedures.Cloth_IdNoToName(con, Val(dt1.Rows(0).Item("ClothTo_IdNo").ToString))
                cbo_ClothTypeTo.Text = Common_Procedures.ClothType_IdNoToName(con, Val(dt1.Rows(0).Item("ClothTypeTo_IdNo").ToString))

                cbo_Godown_StockFROM.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("WareHouseFrom_IdNo").ToString))
                cbo_Godown_StockTO.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("WareHouseTo_IdNo").ToString))

                cbo_ClothSales_OrderCode_forSelection_To.Text = dt1.Rows(0).Item("ClothSales_OrderCode_forSelection_To").ToString
                cbo_ClothSales_OrderCode_forSelection_From.Text = dt1.Rows(0).Item("ClothSales_OrderCode_forSelection_From").ToString


                txt_Note.Text = dt1.Rows(0).Item("Note").ToString
                txt_FoldingFrom.Text = Val(dt1.Rows(0).Item("Folding").ToString)
                txt_FoldingTo.Text = Val(dt1.Rows(0).Item("Folding_To").ToString)
                lbl_UserName.Text = "USER : " & Trim(UCase(Common_Procedures.User_IdNoToName(con1, Val(dt1.Rows(0).Item("User_IdNo").ToString))))
                LockSTS = False

                da2 = New SqlClient.SqlDataAdapter("select a.*, b.clothtype_name from Piece_Transfer_Details a INNER JOIN ClothType_Head b ON a.ClothType_IdNo = b.ClothType_IdNo where a.Piece_Transfer_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
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
                        dgv_Details.Rows(n).Cells(7).Value = dt2.Rows(i).Item("lot_code").ToString
                        dgv_Details.Rows(n).Cells(8).Value = dt2.Rows(i).Item("New_LotNo").ToString
                        dgv_Details.Rows(n).Cells(9).Value = dt2.Rows(i).Item("New_PcsNo").ToString
                        dgv_Details.Rows(n).Cells(10).Value = dt2.Rows(i).Item("New_LotCode").ToString

                        dgv_Details.Rows(n).Cells(11).Value = ""
                        da1 = New SqlClient.SqlDataAdapter("select PackingSlip_Code_Type1 , PackingSlip_Code_Type2 , PackingSlip_Code_Type3 , PackingSlip_Code_Type4 , PackingSlip_Code_Type5 from Weaver_ClothReceipt_Piece_Details Where Weaver_Piece_Checking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Lot_Code = '" & Trim(dt2.Rows(i).Item("New_LotCode").ToString) & "' and Piece_No = '" & Trim(dt2.Rows(i).Item("New_PcsNo").ToString) & "' and (PackingSlip_Code_Type1 <> '' or PackingSlip_Code_Type2 <> ''or PackingSlip_Code_Type3 <> ''or PackingSlip_Code_Type4 <> ''or PackingSlip_Code_Type5 <> '')", con)
                        'da1 = New SqlClient.SqlDataAdapter("select count(*) from Weaver_ClothReceipt_Piece_Details Where Weaver_Piece_Checking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Lot_Code = '" & Trim(dt2.Rows(i).Item("New_LotCode").ToString) & "' and Piece_No = '" & Trim(dt2.Rows(i).Item("New_PcsNo").ToString) & "' and (PackingSlip_Code_Type1 <> '' or PackingSlip_Code_Type2 <> ''or PackingSlip_Code_Type3 <> ''or PackingSlip_Code_Type4 <> ''or PackingSlip_Code_Type5 <> '')", con)
                        dt3 = New DataTable
                        da1.Fill(dt3)
                        If dt3.Rows.Count > 0 Then
                            'If IsDBNull(dt3.Rows(0)(0).ToString) = False Then
                            'If Val(dt3.Rows(0)(0).ToString) <> 0 Then

                            If Val(dt1.Rows(0).Item("ClothTypeTo_IdNo").ToString) = 1 Then
                                dgv_Details.Rows(n).Cells(11).Value = dt3.Rows(0).Item("PackingSlip_Code_Type1").ToString
                            ElseIf Val(dt1.Rows(0).Item("ClothTypeTo_IdNo").ToString) = 2 Then
                                dgv_Details.Rows(n).Cells(11).Value = dt3.Rows(0).Item("PackingSlip_Code_Type2").ToString
                            ElseIf Val(dt1.Rows(0).Item("ClothTypeTo_IdNo").ToString) = 3 Then
                                dgv_Details.Rows(n).Cells(11).Value = dt3.Rows(0).Item("PackingSlip_Code_Type3").ToString
                            ElseIf Val(dt1.Rows(0).Item("ClothTypeTo_IdNo").ToString) = 4 Then
                                dgv_Details.Rows(n).Cells(11).Value = dt3.Rows(0).Item("PackingSlip_Code_Type4").ToString
                            ElseIf Val(dt1.Rows(0).Item("ClothTypeTo_IdNo").ToString) = 5 Then
                                dgv_Details.Rows(n).Cells(11).Value = dt3.Rows(0).Item("PackingSlip_Code_Type5").ToString
                            End If

                            'dgv_Details.Rows(n).Cells(11).Value = "1"

                            For j = 0 To dgv_Details.ColumnCount - 1
                                dgv_Details.Rows(n).Cells(j).Style.ForeColor = Color.Red
                            Next

                            LockSTS = True

                            'End If

                            'End If
                        End If
                        dt3.Clear()

                        dgv_Details.Rows(n).Cells(12).Value = Format(Val(dt2.Rows(i).Item("New_Meters").ToString), "########0.00")
                        If Val(dt2.Rows(i).Item("New_Weight").ToString) <> 0 Then
                            dgv_Details.Rows(n).Cells(13).Value = Format(Val(dt2.Rows(i).Item("New_Weight").ToString), "########0.000")
                        End If

                    Next i

                End If


                With dgv_Details_Total
                    If .RowCount = 0 Then .Rows.Add()
                    .Rows(0).Cells(2).Value = Val(dt1.Rows(0).Item("Total_Pcs").ToString)
                    .Rows(0).Cells(4).Value = Format(Val(dt1.Rows(0).Item("Total_Meters").ToString), "########0.00")
                    .Rows(0).Cells(5).Value = Format(Val(dt1.Rows(0).Item("Total_Weight").ToString), "########0.000")
                End With

                dt2.Clear()

            End If

            dt1.Clear()

            If LockSTS = True Then

                msk_date.Enabled = False
                msk_date.BackColor = Color.LightGray
                dtp_Date.Enabled = False

                cbo_PartyFrom.Enabled = False
                cbo_PartyFrom.BackColor = Color.LightGray

                cbo_PartyTo.Enabled = False
                cbo_PartyTo.BackColor = Color.LightGray

                cbo_Godown_StockFROM.Enabled = False
                cbo_Godown_StockFROM.BackColor = Color.LightGray

                cbo_Godown_StockTO.Enabled = False
                cbo_Godown_StockTO.BackColor = Color.LightGray

                cbo_ClothFrom.Enabled = False
                cbo_ClothFrom.BackColor = Color.LightGray

                cbo_ClothTo.Enabled = False
                cbo_ClothTo.BackColor = Color.LightGray

                cbo_ClothTypeTo.Enabled = False
                cbo_ClothTypeTo.BackColor = Color.LightGray

                txt_FoldingFrom.Enabled = False
                txt_FoldingFrom.BackColor = Color.LightGray

                txt_FoldingTo.Enabled = False
                txt_FoldingTo.BackColor = Color.LightGray

                btn_Selection.Enabled = False

            End If

            Grid_Cell_DeSelect()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "DOES NOT MOVE RECORDS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

        Finally

            dt1.Dispose()
            da1.Dispose()
            dt2.Dispose()
            da2.Dispose()
            If msk_date.Visible And msk_date.Enabled Then msk_date.Focus()

        End Try



    End Sub

    Public Sub delete_record() Implements Interface_MDIActions.delete_record
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim cmd As New SqlClient.SqlCommand
        Dim trans As SqlClient.SqlTransaction
        Dim NewCode As String = ""

        Dim vOrdByNo As String = ""
        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        'If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.PackinSlip_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.PackinSlip_Entry, "~D~") = 0 Then MessageBox.Show("You have No Rights to Delete", "DOES NOT DELETE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub


        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.DeletingEntry, Common_Procedures.UR.Piece_Transfer_Entry, New_Entry, Me, con, "Piece_Transfer_Head", "Piece_Transfer_Code", NewCode, "Piece_Transfer_Date", "(Piece_Transfer_Code = '" & Trim(NewCode) & "')") = False Then Exit Sub


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

        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        Da = New SqlClient.SqlDataAdapter("select count(*) from Weaver_ClothReceipt_Piece_Details Where Weaver_Piece_Checking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and (PackingSlip_Code_Type1 <> '' or PackingSlip_Code_Type2 <> ''or PackingSlip_Code_Type3 <> ''or PackingSlip_Code_Type4 <> ''or PackingSlip_Code_Type5 <> '' or BuyerOffer_Code_Type1 <> '' or BuyerOffer_Code_Type2 <> '' or BuyerOffer_Code_Type3 <> '' or BuyerOffer_Code_Type4 <> '' or BuyerOffer_Code_Type5 <> '')", con)
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

        trans = con.BeginTransaction

        Try

            cmd.Connection = con
            cmd.Transaction = trans


            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "DELETE", "Piece_Transfer_Head", "Piece_Transfer_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "", "", "Piece_Transfer_Code, Company_IdNo, for_OrderBy", trans)

            Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "DELETE", "Piece_Transfer_Details", "Piece_Transfer_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, True, "Lot_No ,  Pcs_No  ,ClothType_IdNo,  Meters  ,  Weight  ,   Weight_Meter, Lot_Code   ,New_LotCode ,   New_LotNo ,  New_PcsNo  ,  New_Meters  ,  New_Weight    ", "Sl_No", "Piece_Transfer_Code, For_OrderBy, Company_IdNo, Piece_Transfer_No, Piece_Transfer_Date, LedgerFrom_IdNo,LedgerTo_IdNo", trans)



            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type1 = '', PackingSlip_Inc_Type1 = PackingSlip_Inc_Type1 - 1 Where PackingSlip_Code_Type1 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type2 = '', PackingSlip_Inc_Type2 = PackingSlip_Inc_Type2 - 1 Where PackingSlip_Code_Type2 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type3 = '', PackingSlip_Inc_Type3 = PackingSlip_Inc_Type3 - 1 Where PackingSlip_Code_Type3 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type4 = '', PackingSlip_Inc_Type4 = PackingSlip_Inc_Type4 - 1 Where PackingSlip_Code_Type4 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type5 = '', PackingSlip_Inc_Type5 = PackingSlip_Inc_Type5 - 1 Where PackingSlip_Code_Type5 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete Weaver_ClothReceipt_Piece_Details Where Weaver_Piece_Checking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and reference_code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Piece_Transfer_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Piece_Transfer_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "delete from Piece_Transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Piece_Transfer_Code = '" & Trim(NewCode) & "'"
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
        Dim da As New SqlClient.SqlDataAdapter
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable

        If Filter_Status = False Then

            da = New SqlClient.SqlDataAdapter("select Cloth_Name from Cloth_Head order by Cloth_Name", con)
            da.Fill(dt1)
            cbo_Filter_Cloth.DataSource = dt1
            cbo_Filter_Cloth.DisplayMember = "Cloth_Name"

            da = New SqlClient.SqlDataAdapter("select ClothType_Name from ClothType_head where ClothType_IdNo = 0 or ClothType_IdNo Between 1 and 5 order by ClothType_Name", con)
            da.Fill(dt2)
            cbo_Filter_ClothType.DataSource = dt2
            cbo_Filter_ClothType.DisplayMember = "ClothType_Name"

            dtp_Filter_Fromdate.Text = Common_Procedures.Company_FromDate
            dtp_Filter_ToDate.Text = Common_Procedures.Company_ToDate
            cbo_Filter_Cloth.Text = ""
            cbo_Filter_ClothType.Text = ""

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

        Try

            da = New SqlClient.SqlDataAdapter("select top 1 Piece_Transfer_No from Piece_Transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Piece_Transfer_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Piece_Transfer_No", con)
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

        Try

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Piece_Transfer_No from Piece_Transfer_Head where for_orderby > " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Piece_Transfer_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby, Piece_Transfer_No", con)
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

            OrdByNo = Common_Procedures.OrderBy_CodeToValue(Trim(lbl_RefNo.Text))

            da = New SqlClient.SqlDataAdapter("select top 1 Piece_Transfer_No from Piece_Transfer_Head where for_orderby < " & Str(OrdByNo) & " and company_idno = " & Str(Val(lbl_Company.Tag)) & " and Piece_Transfer_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Piece_Transfer_No desc", con)
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

        Try
            da = New SqlClient.SqlDataAdapter("select top 1 Piece_Transfer_No from Piece_Transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Piece_Transfer_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Piece_Transfer_No desc", con)
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

        Try
            clear()

            New_Entry = True

            lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Piece_Transfer_Head", "Piece_Transfer_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode)

            lbl_RefNo.ForeColor = Color.Red


            msk_date.Text = Date.Today.ToShortDateString
            da = New SqlClient.SqlDataAdapter("select top 1 * from Piece_Transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Piece_Transfer_Code like '%/" & Trim(Common_Procedures.FnYearCode) & "' Order by for_Orderby desc, Piece_Transfer_No desc", con)
            dt1 = New DataTable
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If Val(Common_Procedures.settings.PreviousEntryDate_ByDefault) = 1 Then '---- M.S Textiles (Tirupur)
                    If dt1.Rows(0).Item("Piece_Transfer_Date").ToString <> "" Then msk_date.Text = dt1.Rows(0).Item("Piece_Transfer_Date").ToString
                End If

                'If cbo_Godown_StockFROM.Visible Then
                '    cbo_Godown_StockFROM.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("WareHouseFrom_IdNo").ToString))
                'End If

                'If cbo_Godown_StockTO.Visible Then
                '    cbo_Godown_StockTO.Text = Common_Procedures.Ledger_IdNoToName(con, Val(dt1.Rows(0).Item("WareHouseTo_IdNo").ToString))
                'End If

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

            inpno = InputBox("Enter Ref No.", "FOR FINDING...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Piece_Transfer_No from Piece_Transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Piece_Transfer_Code = '" & Trim(RecCode) & "'", con)
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
                MessageBox.Show("Ref No. does not exists", "DOES NOT FIND...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

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

        '  If Val(Common_Procedures.User.IdNo) <> 1 And InStr(Common_Procedures.UR.PackinSlip_Entry, "~L~") = 0 And InStr(Common_Procedures.UR.PackinSlip_Entry, "~I~") = 0 Then MessageBox.Show("You have No Rights to Insert", "DOES NOT INSERT...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) : Exit Sub
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.InsertingEntry, Common_Procedures.UR.Piece_Transfer_Entry, New_Entry, Me) = False Then Exit Sub

        Try

            inpno = InputBox("Enter New Ref No.", "FOR NEW RECEIPT INSERTION...")

            RecCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(inpno) & "/" & Trim(Common_Procedures.FnYearCode)

            Da = New SqlClient.SqlDataAdapter("select Piece_Transfer_No from Piece_Transfer_Head where company_idno = " & Str(Val(lbl_Company.Tag)) & " and Piece_Transfer_Code = '" & Trim(RecCode) & "'", con)
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
                    MessageBox.Show("Invalid Ref No", "DOES NOT INSERT NEW REF NO...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)

                Else
                    new_record()
                    Insert_Entry = True
                    lbl_RefNo.Text = Trim(UCase(inpno))

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
        Dim Dt2 As New DataTable
        Dim tr As SqlClient.SqlTransaction
        Dim NewCode As String = ""
        Dim ClthTypTo_ID As Integer = 0
        Dim vClthTypFrm_ID As Integer = 0
        Dim CloFrm_ID As Integer = 0, CloTo_ID As Integer
        Dim LedFrm_ID As Integer = 0, LedTo_ID As Integer = 0
        Dim EdsCnt_ID As Integer = 0
        Dim Nr As Long = 0
        Dim Sno As Integer = 0
        Dim Partcls As String = ""
        Dim PBlNo As String = ""
        Dim PN As String = ""
        Dim vTotMtrs As Single, vTotPcs As Single, vTotWgt As Single
        Dim EntID As String = ""
        Dim Lot_Cd_To As String, Lot_No_To As String, Pcs_No_To As String
        Dim eXmSG As String = ""
        Dim vTotNewMtrs As Single = 0, vTotNewWgt As Single = 0
        Dim vWgt_Mtr As Single = 0
        Dim Dup_SetNoBmNo As String = ""
        Dim vOrdByNo As String = ""
        Dim vErrMsg As String = ""
        Dim vGdwnFrom_IdNo As Integer = 0
        Dim vGdwnTo_IdNo As Integer = 0

        vOrdByNo = Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text)

        If Val(lbl_Company.Tag) = 0 Then
            MessageBox.Show("Invalid Company Selection", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Exit Sub
        End If




        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.SavingEntry, Common_Procedures.UR.Piece_Transfer_Entry, New_Entry, Me, con, "Piece_Transfer_Head", "Piece_Transfer_Code", NewCode, "Piece_Transfer_Date", "(Piece_Transfer_Code = '" & Trim(NewCode) & "')", "(Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Piece_Transfer_Code LIKE '%/" & Trim(Common_Procedures.FnYearCode) & "')", "for_Orderby desc, Piece_Transfer_No desc", dtp_Date.Value.Date) = False Then Exit Sub


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

        If Val(txt_FoldingFrom.Text) = 0 Then txt_FoldingFrom.Text = "100"
        If Val(txt_FoldingTo.Text) = 0 Then txt_FoldingTo.Text = "100"

        LedFrm_ID = 0
        LedTo_ID = 0
        If Common_Procedures.settings.JOBWORKENTRY_Status = 1 Then

            LedFrm_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyFrom.Text)
            If LedFrm_ID = 0 Then
                MessageBox.Show("Invalid Party From", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_PartyFrom.Enabled Then cbo_PartyFrom.Focus()
                Exit Sub
            End If
            If LedFrm_ID = 0 Then LedFrm_ID = Common_Procedures.CommonLedger.OwnSort_Ac

            LedTo_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyTo.Text)
            If LedTo_ID = 0 Then
                MessageBox.Show("Invalid Party To", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_PartyTo.Enabled Then cbo_PartyTo.Focus()
                Exit Sub
            End If
            If LedTo_ID = 0 Then LedTo_ID = Common_Procedures.CommonLedger.OwnSort_Ac


        Else
            LedFrm_ID = Common_Procedures.CommonLedger.OwnSort_Ac
            LedTo_ID = Common_Procedures.CommonLedger.OwnSort_Ac

        End If


        If Common_Procedures.settings.Sales_OrderNumber_compulsory_in_ALLEntry_Status = 1 Then
            If Trim(cbo_ClothSales_OrderCode_forSelection_From.Text) = "" Then
                MessageBox.Show("Invalid Sales Order No From", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_ClothSales_OrderCode_forSelection_From.Enabled And cbo_ClothSales_OrderCode_forSelection_From.Visible Then cbo_ClothSales_OrderCode_forSelection_From.Focus()
                Exit Sub
            End If
        End If

        If Common_Procedures.settings.Sales_OrderNumber_compulsory_in_ALLEntry_Status = 1 Then
            If Trim(cbo_ClothSales_OrderCode_forSelection_To.Text) = "" Then
                MessageBox.Show("Invalid Sales Order No To", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_ClothSales_OrderCode_forSelection_To.Enabled And cbo_ClothSales_OrderCode_forSelection_To.Visible Then cbo_ClothSales_OrderCode_forSelection_To.Focus()
                Exit Sub
            End If
        End If


        vGdwnFrom_IdNo = 0
        vGdwnTo_IdNo = 0
        If Common_Procedures.settings.Multi_Godown_Status = 1 Then

            vGdwnFrom_IdNo = Common_Procedures.Ledger_NameToIdNo(con, cbo_Godown_StockFROM.Text)
            If cbo_Godown_StockFROM.Visible = True Then
                If vGdwnFrom_IdNo = 0 Then
                    MessageBox.Show("Invalid Fabric Godown From Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_Godown_StockFROM.Enabled And cbo_Godown_StockFROM.Visible Then cbo_Godown_StockFROM.Focus()
                    Exit Sub
                End If
            End If
            If vGdwnFrom_IdNo = 0 Then vGdwnFrom_IdNo = Common_Procedures.CommonLedger.Godown_Ac


            vGdwnTo_IdNo = Common_Procedures.Ledger_NameToIdNo(con, cbo_Godown_StockTO.Text)
            If cbo_Godown_StockTO.Visible = True Then
                If vGdwnTo_IdNo = 0 Then
                    MessageBox.Show("Invalid Fabric Godown To Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    If cbo_Godown_StockTO.Enabled And cbo_Godown_StockTO.Visible Then cbo_Godown_StockTO.Focus()
                    Exit Sub
                End If
            End If
            If vGdwnTo_IdNo = 0 Then vGdwnTo_IdNo = Common_Procedures.CommonLedger.Godown_Ac

        Else
            vGdwnFrom_IdNo = Common_Procedures.CommonLedger.Godown_Ac
            vGdwnTo_IdNo = Common_Procedures.CommonLedger.Godown_Ac

        End If

        CloFrm_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothFrom.Text)
        If CloFrm_ID = 0 Then
            MessageBox.Show("Invalid Cloth Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_ClothFrom.Enabled And cbo_ClothFrom.Visible Then cbo_ClothFrom.Focus()
            Exit Sub
        End If

        CloTo_ID = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothTo.Text)
        If CloTo_ID = 0 Then
            MessageBox.Show("Invalid ClothTo Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_ClothTo.Enabled And cbo_ClothTo.Visible Then cbo_ClothTo.Focus()
            Exit Sub
        End If

        ClthTypTo_ID = Common_Procedures.ClothType_NameToIdNo(con, cbo_ClothTypeTo.Text)
        lbl_UserName.Text = Common_Procedures.User.IdNo
        If ClthTypTo_ID = 0 Then
            MessageBox.Show("Invalid Cloth Type Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_ClothTypeTo.Enabled And cbo_ClothTypeTo.Visible Then cbo_ClothTypeTo.Focus()
            Exit Sub
        End If




        With dgv_Details

            cmd.Connection = con

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(msk_date.Text))

            Dup_SetNoBmNo = ""
            For i = 0 To .RowCount - 1

                If Val(.Rows(i).Cells(4).Value) <> 0 Or Val(.Rows(i).Cells(12).Value) <> 0 Then

                    If Val(.Rows(i).Cells(4).Value) = 0 Then
                        MessageBox.Show("Invalid Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(4)
                        End If
                        Exit Sub
                    End If

                    If Trim(.Rows(i).Cells(8).Value) = "" Then
                        MessageBox.Show("Invalid New Lot.No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(8)
                        End If
                        Exit Sub
                    End If

                    If Trim(.Rows(i).Cells(9).Value) = "" Then
                        MessageBox.Show("Invalid New Pcs.No", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(9)
                        End If
                        Exit Sub
                    End If

                    If Val(.Rows(i).Cells(12).Value) = 0 Then
                        MessageBox.Show("Invalid New Meters", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(12)
                        End If
                        Exit Sub
                    End If

                    If InStr(1, Trim(UCase(Dup_SetNoBmNo)), "~" & Trim(UCase(.Rows(i).Cells(8).Value)) & "||" & Trim(UCase(.Rows(i).Cells(9).Value)) & "~") > 0 Then
                        MessageBox.Show("Duplicate Piece No - " & Trim(UCase(.Rows(i).Cells(9).Value)) & " for this Lot : " & Trim(UCase(.Rows(i).Cells(8).Value)), "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        If .Enabled And .Visible Then
                            .Focus()
                            .CurrentCell = .Rows(i).Cells(9)
                            .CurrentCell.Selected = True
                        End If
                        Exit Sub
                    End If

                    Dup_SetNoBmNo = Trim(Dup_SetNoBmNo) & "~" & Trim(UCase(.Rows(i).Cells(8).Value)) & "||" & Trim(UCase(.Rows(i).Cells(9).Value)) & "~"

                    Lot_No_To = Trim(.Rows(i).Cells(8).Value)
                    Pcs_No_To = Trim(.Rows(i).Cells(9).Value)
                    Lot_Cd_To = Trim(Val(lbl_Company.Tag)) & "-" & Trim(Lot_No_To) & "/" & Microsoft.VisualBasic.Right(Trim(.Rows(i).Cells(7).Value), 5).ToString

                    cmd.CommandText = "Select count(*) from Weaver_ClothReceipt_Piece_Details Where lot_code = '" & Trim(Lot_Cd_To) & "' and Piece_No = '" & Trim(Pcs_No_To) & "' and Weaver_Piece_Checking_Code <> '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Weaver_ClothReceipt_Code <> '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                    Da = New SqlClient.SqlDataAdapter(cmd)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    If Dt1.Rows.Count > 0 Then
                        If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                            If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                                MessageBox.Show("Duplicate Piece No for this Lot : " & Trim(UCase(.Rows(i).Cells(8).Value)) & Chr(13) & "Already entered in another entry", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                                If .Enabled And .Visible Then
                                    .Focus()
                                    .CurrentCell = .Rows(i).Cells(1)
                                End If
                                Exit Sub
                            End If
                        End If
                    End If
                    Dt1.Clear()

                    cmd.CommandText = "Select count(*) from Weaver_ClothReceipt_Piece_Details Where lot_code = '" & Trim(.Rows(i).Cells(7).Value) & "' and Weaver_Piece_Checking_Date > @EntryDate"
                    Da = New SqlClient.SqlDataAdapter(cmd)
                    Dt1 = New DataTable
                    Da.Fill(Dt1)
                    If Dt1.Rows.Count > 0 Then
                        If IsDBNull(Dt1.Rows(0)(0).ToString) = False Then
                            If Val(Dt1.Rows(0)(0).ToString) > 0 Then
                                MessageBox.Show("Invalid Piece Selection - Lot No. : " & Trim(.Rows(i).Cells(1).Value) & " ,   Piece_No : " & Trim(.Rows(i).Cells(2).Value) & Chr(13) & " Piece Checking date should be lesser than transfer Date", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                                If .Enabled And .Visible Then
                                    .Focus()
                                    .CurrentCell = .Rows(i).Cells(1)
                                End If
                                Exit Sub
                            End If
                        End If
                    End If
                    Dt1.Clear()



                End If

            Next

        End With


        Total_Calculation()

        vTotMtrs = 0 : vTotPcs = 0 : vTotWgt = 0 : vTotNewMtrs = 0 : vTotNewWgt = 0
        If dgv_Details_Total.RowCount > 0 Then
            vTotPcs = Val(dgv_Details_Total.Rows(0).Cells(2).Value)
            vTotMtrs = Val(dgv_Details_Total.Rows(0).Cells(4).Value)
            vTotWgt = Val(dgv_Details_Total.Rows(0).Cells(5).Value)

            vTotNewMtrs = Val(dgv_Details_Total.Rows(0).Cells(12).Value)
            vTotNewWgt = Val(dgv_Details_Total.Rows(0).Cells(13).Value)
        End If

        tr = con.BeginTransaction

        Try

            If Insert_Entry = True Or New_Entry = False Then
                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            Else

                lbl_RefNo.Text = Common_Procedures.get_MaxCode(con, "Piece_Transfer_Head", "Piece_Transfer_Code", "For_OrderBy", "", Val(lbl_Company.Tag), Common_Procedures.FnYearCode, tr)

                NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

            End If

            cmd.Connection = con
            cmd.Transaction = tr

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(msk_date.Text))

            If New_Entry = True Then

                cmd.CommandText = "Insert into Piece_Transfer_Head (  Piece_Transfer_Code  ,                 Company_IdNo     ,         Piece_Transfer_No     ,                               for_OrderBy                              , Piece_Transfer_Date,         LedgerFrom_IdNo     ,         LedgerTo_IdNo     ,        ClothFrom_IdNo      ,         ClothTo_IdNo      ,         ClothTypeTo_IdNo      ,                 Folding           ,              Total_Pcs   ,         Total_Meters       ,         Total_Weight      ,         Total_New_Meters          ,  Total_New_Weight            ,               Note           ,             User_idno               ,   Folding_To             ,         WareHouseFrom_IdNo      ,        WareHouseTo_IdNo        , ClothSales_OrderCode_forSelection_From , ClothSales_OrderCode_forSelection_TO) " &
                                    "          Values              ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",     @EntryDate     ,  " & Str(Val(LedFrm_ID)) & ", " & Str(Val(LedTo_ID)) & ", " & Str(Val(CloFrm_ID)) & ", " & Str(Val(CloTo_ID)) & ", " & Str(Val(ClthTypTo_ID)) & ", " & Str(Val(txt_FoldingFrom.Text)) & ", " & Str(Val(vTotPcs)) & ", " & Str(Val(vTotMtrs)) & " , " & Str(Val(vTotWgt)) & " , " & Str(Val(vTotNewMtrs)) & " , " & Str(Val(vTotNewWgt)) & " , '" & Trim(txt_Note.Text) & "', " & Val(lbl_UserName.Text) & " ," & Val(txt_FoldingTo.Text) & ", " & Str(Val(vGdwnFrom_IdNo)) & ", " & Str(Val(vGdwnTo_IdNo)) & " , '" & Trim(cbo_ClothSales_OrderCode_forSelection_From.Text) & "', '" & Trim(cbo_ClothSales_OrderCode_forSelection_To.Text) & "') "
                cmd.ExecuteNonQuery()

            Else



                Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "OLD", "Piece_Transfer_Head", "Piece_Transfer_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Piece_Transfer_Code, Company_IdNo, for_OrderBy", tr)

                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "OLD", "Piece_Transfer_Details", "Piece_Transfer_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Lot_No ,  Pcs_No  ,ClothType_IdNo,  Meters  ,  Weight  ,   Weight_Meter, Lot_Code   ,New_LotCode ,   New_LotNo ,  New_PcsNo  ,  New_Meters  ,  New_Weight    ", "Sl_No", "Piece_Transfer_Code, For_OrderBy, Company_IdNo, Piece_Transfer_No, Piece_Transfer_Date, LedgerFrom_IdNo,LedgerTo_IdNo", tr)


                cmd.CommandText = "Update Piece_Transfer_Head set Piece_Transfer_Date = @EntryDate, LedgerFrom_IdNo = " & Str(Val(LedFrm_ID)) & ", LedgerTo_IdNo = " & Str(Val(LedTo_ID)) & ", ClothFrom_IdNo = " & Str(Val(CloFrm_ID)) & ", ClothTo_IdNo = " & Str(Val(CloTo_ID)) & ", ClothTypeTo_IdNo = " & Str(Val(ClthTypTo_ID)) & ", Folding = " & Str(Val(txt_FoldingFrom.Text)) & ", Folding_To = " & Val(txt_FoldingTo.Text) & " , Total_Pcs = " & Str(Val(vTotPcs)) & ", Total_Meters = " & Str(Val(vTotMtrs)) & ", Total_Weight = " & Str(Val(vTotWgt)) & ", Total_New_Meters = " & Str(Val(vTotNewMtrs)) & ", Total_New_Weight = " & Str(Val(vTotNewWgt)) & ", Note = '" & Trim(txt_Note.Text) & "', user_idNo = " & Val(lbl_UserName.Text) & " , WareHouseFrom_IdNo = " & Str(Val(vGdwnFrom_IdNo)) & ", WareHouseTo_IdNo = " & Str(Val(vGdwnTo_IdNo)) & " , ClothSales_OrderCode_forSelection_From = '" & Trim(cbo_ClothSales_OrderCode_forSelection_From.Text) & "' , ClothSales_OrderCode_forSelection_To = '" & Trim(cbo_ClothSales_OrderCode_forSelection_To.Text) & "' Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Piece_Transfer_Code = '" & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type1 = '', PackingSlip_Inc_Type1 = PackingSlip_Inc_Type1 - 1 Where PackingSlip_Code_Type1 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type2 = '', PackingSlip_Inc_Type2 = PackingSlip_Inc_Type2 - 1 Where PackingSlip_Code_Type2 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type3 = '', PackingSlip_Inc_Type3 = PackingSlip_Inc_Type3 - 1 Where PackingSlip_Code_Type3 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type4 = '', PackingSlip_Inc_Type4 = PackingSlip_Inc_Type4 - 1 Where PackingSlip_Code_Type4 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type5 = '', PackingSlip_Inc_Type5 = PackingSlip_Inc_Type5 - 1 Where PackingSlip_Code_Type5 = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "Delete Weaver_ClothReceipt_Piece_Details Where Weaver_Piece_Checking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and (PackingSlip_Code_Type1 = '' and PackingSlip_Code_Type2 = '' and PackingSlip_Code_Type3 = '' and PackingSlip_Code_Type4 = '' and PackingSlip_Code_Type5 = '' and BuyerOffer_Code_Type1 = '' and BuyerOffer_Code_Type2 = '' and BuyerOffer_Code_Type3 = '' and BuyerOffer_Code_Type4 = '' and BuyerOffer_Code_Type5 = '')"
                cmd.ExecuteNonQuery()

            End If
            Call Common_Procedures.User_Modification_Updation(con, "HEAD", Me.Name, "NEW", "Piece_Transfer_Head", "Piece_Transfer_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "", "", "Piece_Transfer_Code, Company_IdNo, for_OrderBy", tr)

            EntID = Trim(Pk_Condition) & Trim(lbl_RefNo.Text)
            Partcls = "PcsTrans : Ref.No. " & Trim(lbl_RefNo.Text)
            PBlNo = Trim(lbl_RefNo.Text)

            cmd.CommandText = "Delete from Piece_Transfer_Details Where Company_IdNo = " & Str(Val(lbl_Company.Tag)) & " and Piece_Transfer_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Delete from Stock_Cloth_Processing_Details where company_idno = " & Str(Val(lbl_Company.Tag)) & " and reference_code = '" & Trim(Pk_Condition) & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()

            With dgv_Details
                Sno = 0

                For i = 0 To .RowCount - 1

                    If Val(.Rows(i).Cells(4).Value) <> 0 And Trim(.Rows(i).Cells(7).Value) <> "" And Val(.Rows(i).Cells(12).Value) <> 0 Then

                        vClthTypFrm_ID = Common_Procedures.ClothType_NameToIdNo(con, .Rows(i).Cells(3).Value, tr)

                        Lot_No_To = Trim(.Rows(i).Cells(8).Value)
                        Pcs_No_To = Trim(.Rows(i).Cells(9).Value)
                        Lot_Cd_To = Trim(Val(lbl_Company.Tag)) & "-" & Trim(Lot_No_To) & "/" & Microsoft.VisualBasic.Right(Trim(.Rows(i).Cells(7).Value), 5).ToString
                        'Lot_Cd_To = Trim(.Rows(i).Cells(10).Value)

                        If Trim(Lot_Cd_To) = "" Then

                            If Trim(.Rows(i).Cells(8).Value) <> "" Then
                                Lot_No_To = Trim(.Rows(i).Cells(8).Value)
                                Lot_Cd_To = Trim(Val(lbl_Company.Tag)) & "-" & Trim(Lot_No_To) & "/" & Microsoft.VisualBasic.Right(Trim(.Rows(i).Cells(7).Value), 5).ToString
                                'Lot_Cd_To = Trim(Val(lbl_Company.Tag)) & "-" & Trim(Lot_No_To) & "/" & Trim(Common_Procedures.FnYearCode)
                                Pcs_No_To = Trim(.Rows(i).Cells(9).Value)

                            Else
                                Lot_No_To = Trim(.Rows(i).Cells(1).Value) & "/T"
                                Lot_Cd_To = Trim(.Rows(i).Cells(7).Value) & "/T"
                                Pcs_No_To = Trim(.Rows(i).Cells(2).Value)
                            End If

                        End If

                        Sno = Sno + 1

                        cmd.CommandText = "Insert into Piece_Transfer_Details ( Piece_Transfer_Code   ,            Company_IdNo          ,           Piece_Transfer_No   ,                               for_OrderBy                              , Piece_Transfer_Date,           Sl_No      ,                    Lot_No              ,                    Pcs_No              ,             ClothType_IdNo      ,                      Meters              ,                      Weight              ,                      Weight_Meter        ,                    Lot_Code            ,      New_LotCode         ,           New_LotNo      ,           New_PcsNo       ,                      New_Meters           ,                      New_Weight            ) " &
                                            "          Values                 ('" & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",      @EntryDate    , " & Str(Val(Sno)) & ", '" & Trim(.Rows(i).Cells(1).Value) & "', '" & Trim(.Rows(i).Cells(2).Value) & "', " & Str(Val(vClthTypFrm_ID)) & ", " & Str(Val(.Rows(i).Cells(4).Value)) & ", " & Str(Val(.Rows(i).Cells(5).Value)) & ", " & Str(Val(.Rows(i).Cells(6).Value)) & ", '" & Trim(.Rows(i).Cells(7).Value) & "', '" & Trim(Lot_Cd_To) & "', '" & Trim(Lot_No_To) & "', '" & Trim(Pcs_No_To) & "' , " & Str(Val(.Rows(i).Cells(12).Value)) & ", " & Str(Val(.Rows(i).Cells(13).Value)) & " ) "
                        cmd.ExecuteNonQuery()

                        If vClthTypFrm_ID = 1 Then
                            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type1 = '" & Trim(Pk_Condition) & Trim(NewCode) & "', PackingSlip_Inc_Type1 = PackingSlip_Inc_Type1 + 1 Where lot_code = '" & Trim(.Rows(i).Cells(7).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "'"
                            cmd.ExecuteNonQuery()

                        ElseIf vClthTypFrm_ID = 2 Then
                            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type2 = '" & Trim(Pk_Condition) & Trim(NewCode) & "', PackingSlip_Inc_Type2 = PackingSlip_Inc_Type2 + 1 Where lot_code = '" & Trim(.Rows(i).Cells(7).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "'"
                            cmd.ExecuteNonQuery()

                        ElseIf vClthTypFrm_ID = 3 Then
                            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type3 = '" & Trim(Pk_Condition) & Trim(NewCode) & "', PackingSlip_Inc_Type3 = PackingSlip_Inc_Type3 + 1 Where lot_code = '" & Trim(.Rows(i).Cells(7).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "'"
                            cmd.ExecuteNonQuery()

                        ElseIf vClthTypFrm_ID = 4 Then
                            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type4 = '" & Trim(Pk_Condition) & Trim(NewCode) & "', PackingSlip_Inc_Type4 = PackingSlip_Inc_Type4 + 1 Where lot_code = '" & Trim(.Rows(i).Cells(7).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "'"
                            cmd.ExecuteNonQuery()

                        ElseIf vClthTypFrm_ID = 5 Then
                            cmd.CommandText = "update Weaver_ClothReceipt_Piece_Details set PackingSlip_Code_Type5 = '" & Trim(Pk_Condition) & Trim(NewCode) & "', PackingSlip_Inc_Type5 = PackingSlip_Inc_Type5 + 1 Where lot_code = '" & Trim(.Rows(i).Cells(7).Value) & "' and Piece_No = '" & Trim(.Rows(i).Cells(2).Value) & "'"
                            cmd.ExecuteNonQuery()

                        End If

                        vWgt_Mtr = Val(.Rows(i).Cells(13).Value) / Val(.Rows(i).Cells(12).Value)

                        Nr = 0
                        cmd.CommandText = "Update Weaver_ClothReceipt_Piece_Details set Weaver_Piece_Checking_Date = @EntryDate, Ledger_Idno = " & Str(Val(LedTo_ID)) & ", StockOff_IdNo = " & Str(Val(LedTo_ID)) & ", Folding_Checking = " & Str(Val(txt_FoldingTo.Text)) & ", Folding = " & Str(Val(txt_FoldingTo.Text)) & ", Sl_No = " & Str(Val(Pcs_No_To)) & ", Main_PieceNo = '" & Trim(Val(Pcs_No_To)) & "', PieceNo_OrderBy = " & Str(Val(Common_Procedures.OrderBy_CodeToValue(Pcs_No_To))) & ", ReceiptMeters_Checking = 0, Receipt_Meters = 0, Loom_No = '', Pick = 0, Width = 0, Type" & Trim(Val(ClthTypTo_ID)) & "_Meters = " & Str(Val(.Rows(i).Cells(12).Value)) & ", Total_Checking_Meters = " & Str(Val(.Rows(i).Cells(12).Value)) & ", Weight = " & Str(Val(.Rows(i).Cells(13).Value)) & ", Weight_Meter = " & Str(Val(vWgt_Mtr)) & ", " &
                                            " Beam_Knotting_Code = '', Beam_Knotting_No = '', Loom_IdNo = 0, Width_Type = '', Crimp_Percentage = 0, Set_Code1 = '', Set_No1 = '', Beam_No1 = '', Balance_Meters1 = 0, Set_Code2 = '', Set_No2 = '', Beam_No2 = '', Balance_Meters2 = 0, BeamConsumption_Meters = 0, warehouse_idno = " & Str(Val(vGdwnTo_IdNo)) & " " &
                                            " Where Weaver_Piece_Checking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Lot_Code = '" & Trim(Lot_Cd_To) & "' and Piece_No = '" & Trim(Pcs_No_To) & "'"
                        Nr = cmd.ExecuteNonQuery()

                        If Nr = 0 Then
                            cmd.CommandText = "Insert into Weaver_ClothReceipt_Piece_Details (          Weaver_Piece_Checking_Code         ,             Company_IdNo         ,     Weaver_Piece_Checking_No   ,  Weaver_Piece_Checking_Date,         Ledger_Idno       ,         StockOff_IdNo     ,                Weaver_ClothReceipt_Code     ,      Weaver_ClothReceipt_No     ,                                for_orderby                              , Weaver_ClothReceipt_Date,        Lot_Code          ,           Lot_No         ,           Cloth_IdNo      ,              Folding_Checking       ,               Folding               ,                 Sl_No      ,                 Piece_No ,           Main_PieceNo        ,                        PieceNo_OrderBy                                  , ReceiptMeters_Checking,  Receipt_Meters, Loom_No, Pick , Width ,   Type" & Trim(Val(ClthTypTo_ID)) & "_Meters ,                   Total_Checking_Meters   ,                     Weight                ,          Weight_Meter     , Beam_Knotting_Code, Beam_Knotting_No, Loom_IdNo, Width_Type, Crimp_Percentage, Set_Code1, Set_No1, Beam_No1, Balance_Meters1, Set_Code2, Set_No2 , Beam_No2, Balance_Meters2, BeamConsumption_Meters ,          warehouse_idno        ) " &
                                                "     Values                                 ( '" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ",  '" & Trim(lbl_RefNo.Text) & "',        @EntryDate          , " & Str(Val(LedTo_ID)) & ", " & Str(Val(LedTo_ID)) & ", '" & Trim(Pk_Condition) & Trim(NewCode) & "',   '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",      @EntryDate          , '" & Trim(Lot_Cd_To) & "', '" & Trim(Lot_No_To) & "', " & Str(Val(CloTo_ID)) & ", " & Str(Val(txt_FoldingTo.Text)) & ", " & Str(Val(txt_FoldingTo.Text)) & ", " & Str(Val(Pcs_No_To)) & ", '" & Trim(Pcs_No_To) & "', '" & Trim(Val(Pcs_No_To)) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(Trim(Pcs_No_To)))) & ",          0            ,         0      ,    ''  ,   0  ,   0   , " & Str(Val(.Rows(i).Cells(12).Value)) & "   , " & Str(Val(.Rows(i).Cells(12).Value)) & ", " & Str(Val(.Rows(i).Cells(13).Value)) & ", " & Str(Val(vWgt_Mtr)) & ",       ''          ,        ''       ,     0    ,    ''     ,       0         ,     ''   ,    ''  ,    ''   ,          0     ,     ''   ,    ''   ,    ''   ,        0       ,          0             , " & Str(Val(vGdwnTo_IdNo)) & " ) "
                            cmd.ExecuteNonQuery()
                        End If

                        cmd.CommandText = "Insert into Stock_Cloth_Processing_Details  ( Reference_Code    ,             Company_IdNo         ,             Reference_No      ,                               for_OrderBy                              , Reference_Date,       StockOff_IdNo        , DeliveryTo_Idno,          ReceivedFrom_Idno      ,         Entry_ID     ,      Party_Bill_No   ,      Particulars       ,            Sl_No      ,          Cloth_Idno        ,                 Folding             , UnChecked_Meters, Meters_Type" & Trim(Val(vClthTypFrm_ID)) & "    , ClothSales_OrderCode_forSelection ) " &
                                                    "  Values ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",   @EntryDate  , " & Str(Val(LedFrm_ID)) & ",        0       , " & Str(Val(vGdwnFrom_IdNo)) & ", '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',  " & Str(Val(Sno)) & ", " & Str(Val(CloFrm_ID)) & ",   " & Str(Val(txt_FoldingFrom.Text)) & ",      0          ,  " & Str(Val(.Rows(i).Cells(4).Value)) & "  , '" & Trim(cbo_ClothSales_OrderCode_forSelection_From.Text) & "' ) "
                        cmd.ExecuteNonQuery()

                    End If

                Next

                Call Common_Procedures.User_Modification_Updation(con, "DETAILS", Me.Name, "NEW", "Piece_Transfer_Details", "Piece_Transfer_Code", Val(lbl_Company.Tag), NewCode, lbl_RefNo.Text, Val(vOrdByNo), Pk_Condition, "", "", New_Entry, False, "Lot_No ,  Pcs_No  ,ClothType_IdNo,  Meters  ,  Weight  ,   Weight_Meter, Lot_Code   ,New_LotCode ,   New_LotNo ,  New_PcsNo  ,  New_Meters  ,  New_Weight    ", "Sl_No", "Piece_Transfer_Code, For_OrderBy, Company_IdNo, Piece_Transfer_No, Piece_Transfer_Date, LedgerFrom_IdNo,LedgerTo_IdNo", tr)

            End With

            cmd.CommandText = "Insert into Stock_Cloth_Processing_Details (                 Reference_Code             ,             Company_IdNo         ,             Reference_No      ,                               for_OrderBy                              , Reference_Date,       StockOff_IdNo       ,          DeliveryTo_Idno       , ReceivedFrom_Idno,         Entry_ID      ,      Party_Bill_No   ,      Particulars       , Sl_No,          Cloth_Idno        ,                 Folding               , UnChecked_Meters ,  Meters_Type" & Trim(Val(ClthTypTo_ID)) & " , ClothSales_OrderCode_forSelection) " &
                                "          Values                         ('" & Trim(Pk_Condition) & Trim(NewCode) & "', " & Str(Val(lbl_Company.Tag)) & ", '" & Trim(lbl_RefNo.Text) & "', " & Str(Val(Common_Procedures.OrderBy_CodeToValue(lbl_RefNo.Text))) & ",   @EntryDate  , " & Str(Val(LedTo_ID)) & ",  " & Str(Val(vGdwnTo_IdNo)) & ",           0      ,  '" & Trim(EntID) & "', '" & Trim(PBlNo) & "', '" & Trim(Partcls) & "',  -1  , " & Str(Val(CloTo_ID)) & " ,   " & Str(Val(txt_FoldingTo.Text)) & ",      0           ,         " & Str(Val(vTotNewMtrs)) & "       ,  '" & Trim(cbo_ClothSales_OrderCode_forSelection_To.Text) & "') "
            cmd.ExecuteNonQuery()



            '----- Saving Cross Checking - 1

            cmd.CommandText = "Truncate Table " & Trim(Common_Procedures.EntryTempTable) & ""
            cmd.ExecuteNonQuery()

            cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Meters1) select Total_Meters from Piece_Transfer_Head where Piece_Transfer_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Meters1) select -1*Type1_Meters from Weaver_ClothReceipt_Piece_Details Where PackingSlip_Code_Type1 = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Type1_Meters <> 0"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Meters1) select -1*Type2_Meters from Weaver_ClothReceipt_Piece_Details Where PackingSlip_Code_Type2 = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Type2_Meters <> 0"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Meters1) select -1*Type3_Meters from Weaver_ClothReceipt_Piece_Details Where PackingSlip_Code_Type3 = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Type3_Meters <> 0"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Meters1) select -1*Type4_Meters from Weaver_ClothReceipt_Piece_Details Where PackingSlip_Code_Type4 = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Type4_Meters <> 0"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Meters1) select -1*Type5_Meters from Weaver_ClothReceipt_Piece_Details Where PackingSlip_Code_Type5 = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Type5_Meters <> 0"
            cmd.ExecuteNonQuery()

            Da = New SqlClient.SqlDataAdapter("select sum(Meters1) as ProdMtrs from " & Trim(Common_Procedures.EntryTempTable) & " having sum(Meters1) <> 0", con)
            Da.SelectCommand.Transaction = tr
            Dt2 = New DataTable
            Da.Fill(Dt2)
            If Dt2.Rows.Count > 0 Then
                If IsDBNull(Dt2.Rows(0)(0).ToString) = False Then
                    If Val(Dt2.Rows(0)(0).ToString) <> 0 Then
                        Throw New ApplicationException("Invalid Piece Selection : Mismatch of Transfer && Piece Meters")
                        Exit Sub
                    End If
                End If
            End If
            Dt2.Clear()

            '----- Saving Cross Checking - 2


            cmd.CommandText = "Truncate Table " & Trim(Common_Procedures.EntryTempTable) & ""
            cmd.ExecuteNonQuery()

            'cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Int1, Meters1) select a.Lot_Code, a.Pcs_No, a.ClothType_IdNo, -1*a.Meters from Piece_Transfer_Details a, Piece_Transfer_Details b, Piece_Transfer_Head c Where b.Piece_Transfer_Code = '" & Trim(NewCode) & "' and b.Piece_Transfer_Code = c.Piece_Transfer_Code and a.Lot_Code = b.New_LotCode and a.Pcs_No = b.New_PcsNo and a.ClothType_IdNo = c.ClothTypeTo_IdNo"

            cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Int1, Meters1) select Lot_Code, Pcs_No, ClothType_IdNo, Meters from Piece_Transfer_Details Where Piece_Transfer_Code = '" & Trim(NewCode) & "'"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Int1, Meters1) select a.Lot_Code, a.Piece_No, 1, -1*a.Type1_Meters from Weaver_ClothReceipt_Piece_Details a, Piece_Transfer_Details b Where b.Piece_Transfer_Code = '" & Trim(NewCode) & "' and b.ClothType_IdNo = 1 and a.Type1_Meters <> 0 and a.lot_code = b.lot_code and a.piece_no = b.pcs_no"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Int1, Meters1) select a.Lot_Code, a.Piece_No, 2, -1*a.Type2_Meters from Weaver_ClothReceipt_Piece_Details a, Piece_Transfer_Details b Where b.Piece_Transfer_Code = '" & Trim(NewCode) & "' and b.ClothType_IdNo = 2 and a.Type2_Meters <> 0 and a.lot_code = b.lot_code and a.piece_no = b.pcs_no"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Int1, Meters1) select a.Lot_Code, a.Piece_No, 3, -1*a.Type3_Meters from Weaver_ClothReceipt_Piece_Details a, Piece_Transfer_Details b Where b.Piece_Transfer_Code = '" & Trim(NewCode) & "' and b.ClothType_IdNo = 3 and a.Type3_Meters <> 0 and a.lot_code = b.lot_code and a.piece_no = b.pcs_no"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Int1, Meters1) select a.Lot_Code, a.Piece_No, 4, -1*a.Type4_Meters from Weaver_ClothReceipt_Piece_Details a, Piece_Transfer_Details b Where b.Piece_Transfer_Code = '" & Trim(NewCode) & "' and b.ClothType_IdNo = 4 and a.Type4_Meters <> 0 and a.lot_code = b.lot_code and a.piece_no = b.pcs_no"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Int1, Meters1) select a.Lot_Code, a.Piece_No, 5, -1*a.Type5_Meters from Weaver_ClothReceipt_Piece_Details a, Piece_Transfer_Details b Where b.Piece_Transfer_Code = '" & Trim(NewCode) & "' and b.ClothType_IdNo = 5 and a.Type5_Meters <> 0 and a.lot_code = b.lot_code and a.piece_no = b.pcs_no"
            cmd.ExecuteNonQuery()

            Da = New SqlClient.SqlDataAdapter("select Name1, Name2, Int1, sum(Meters1) as ProdMtrs from " & Trim(Common_Procedures.EntryTempTable) & " Group by Name1, Name2, Int1 having sum(Meters1) <> 0", con)
            Da.SelectCommand.Transaction = tr
            Dt2 = New DataTable
            Da.Fill(Dt2)
            If Dt2.Rows.Count > 0 Then
                If IsDBNull(Dt2.Rows(0)(3).ToString) = False Then
                    If Val(Dt2.Rows(0)(3).ToString) <> 0 Then
                        Throw New ApplicationException("Invalid Piece Selection : Mismatch of Transfer && Piece Meters")
                        Exit Sub
                    End If
                End If
            End If
            Dt2.Clear()


            '----- Saving Cross Checking - 3

            cmd.CommandText = "Truncate Table " & Trim(Common_Procedures.EntryTempTable) & ""
            cmd.ExecuteNonQuery()

            '---Piece Checking
            cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Int1, Meters1) select Lot_Code, Piece_No, 1, Type1_Meters from Weaver_ClothReceipt_Piece_Details Where Weaver_Piece_Checking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and PackingSlip_Code_Type1 <> '' and Type1_Meters <> 0"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Int1, Meters1) select Lot_Code, Piece_No, 2, Type2_Meters from Weaver_ClothReceipt_Piece_Details Where Weaver_Piece_Checking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and PackingSlip_Code_Type2 <> '' and Type2_Meters <> 0 "
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Int1, Meters1) select Lot_Code, Piece_No, 3, Type3_Meters from Weaver_ClothReceipt_Piece_Details Where Weaver_Piece_Checking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and PackingSlip_Code_Type3 <> '' and Type3_Meters <> 0 "
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Int1, Meters1) select Lot_Code, Piece_No, 4, Type4_Meters from Weaver_ClothReceipt_Piece_Details Where Weaver_Piece_Checking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and PackingSlip_Code_Type4 <> '' and Type4_Meters <> 0 "
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Int1, Meters1) select Lot_Code, Piece_No, 5, Type5_Meters from Weaver_ClothReceipt_Piece_Details Where Weaver_Piece_Checking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and PackingSlip_Code_Type5 <> '' and Type5_Meters <> 0 "
            cmd.ExecuteNonQuery()


            '---Packing Slip
            cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Int1, Meters1) select a.Lot_Code, a.Pcs_No, a.ClothType_IdNo, -1*a.Meters from Packing_Slip_Details a, Piece_Transfer_Details b, Piece_Transfer_Head c Where b.Piece_Transfer_Code = '" & Trim(NewCode) & "' and b.Piece_Transfer_Code = c.Piece_Transfer_Code and a.Lot_Code = b.New_LotCode and a.Pcs_No = b.New_PcsNo and a.ClothType_IdNo = c.ClothTypeTo_IdNo"
            cmd.ExecuteNonQuery()
            '---Piece Transfer
            cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Int1, Meters1) select a.Lot_Code, a.Pcs_No, a.ClothType_IdNo, -1*a.Meters from Piece_Transfer_Details a, Piece_Transfer_Details b, Piece_Transfer_Head c Where b.Piece_Transfer_Code = '" & Trim(NewCode) & "' and b.Piece_Transfer_Code = c.Piece_Transfer_Code and a.Lot_Code = b.New_LotCode and a.Pcs_No = b.New_PcsNo and a.ClothType_IdNo = c.ClothTypeTo_IdNo"
            cmd.ExecuteNonQuery()
            '---Jobwork Piece Delivery
            cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Int1, Meters1) select a.Lot_Code, a.Pcs_No, a.ClothType_IdNo, -1*a.Meters from JobWork_Piece_Delivery_Details a, Piece_Transfer_Details b, Piece_Transfer_Head c Where b.Piece_Transfer_Code = '" & Trim(NewCode) & "' and b.Piece_Transfer_Code = c.Piece_Transfer_Code and a.Lot_Code = b.New_LotCode and a.Pcs_No = b.New_PcsNo and a.ClothType_IdNo = c.ClothTypeTo_IdNo"
            cmd.ExecuteNonQuery()
            '---Cloth Sales Piece Delivery
            cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Int1, Meters1) select a.Lot_Code, a.Piece_No, a.PieceType_IdNo, -1*a.Meters from ClothSales_Delivery_Piece_Details a, Piece_Transfer_Details b, Piece_Transfer_Head c Where b.Piece_Transfer_Code = '" & Trim(NewCode) & "' and b.Piece_Transfer_Code = c.Piece_Transfer_Code and a.Lot_Code = b.New_LotCode and a.Piece_No = b.New_PcsNo and a.PieceType_IdNo = c.ClothTypeTo_IdNo"
            cmd.ExecuteNonQuery()
            '---Piece Excess/Short
            cmd.CommandText = "Insert Into " & Trim(Common_Procedures.EntryTempTable) & "(Name1, Name2, Int1, Meters1) select a.Lot_Code, a.Pcs_No, a.ClothType_IdNo, -1*a.Meters from Piece_Excess_Short_Details a, Piece_Transfer_Details b, Piece_Transfer_Head c Where b.Piece_Transfer_Code = '" & Trim(NewCode) & "' and b.Piece_Transfer_Code = c.Piece_Transfer_Code and a.Lot_Code = b.New_LotCode and a.Pcs_No = b.New_PcsNo and a.ClothType_IdNo = c.ClothTypeTo_IdNo"
            cmd.ExecuteNonQuery()

            Da = New SqlClient.SqlDataAdapter("Select Name1, Name2, Int1, sum(Meters1) as ProdMtrs from " & Trim(Common_Procedures.EntryTempTable) & " Group by Name1, Name2, Int1 having sum(Meters1) <> 0", con)
            Da.SelectCommand.Transaction = tr
            Dt2 = New DataTable
            Da.Fill(Dt2)
            If Dt2.Rows.Count > 0 Then
                If IsDBNull(Dt2.Rows(0)(3).ToString) = False Then
                    If Val(Dt2.Rows(0)(3).ToString) <> 0 Then
                        Throw New ApplicationException("Invalid Piece Details : Mismatch of Transfer && Piece Meters for Piece No : " & Trim(Dt2.Rows(0)(1).ToString))
                        Exit Sub
                    End If
                End If
            End If
            Dt2.Clear()

            tr.Commit()

            If SaveAll_STS <> True Then
                MessageBox.Show("Saved Sucessfully!!!", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            End If

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

            If InStr(1, Trim(LCase(ex.Message)), Trim(LCase("PK_Weaver_ClothReceipt_Piece_Details"))) > 0 Then
                MessageBox.Show("Lot No (Or) Piece No Already Exists - " & (eXmSG), "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message, "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            End If

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
        Dim TotMtrs As Single, TotPcs As Single, TotWgt As Single
        Dim vTotNewMtrs As Single = 0, vTotNewWgt As Single = 0

        If FrmLdSTS = True Then Exit Sub

        Sno = 0
        TotPcs = 0
        TotMtrs = 0
        TotWgt = 0
        vTotNewMtrs = 0 : vTotNewWgt = 0

        With dgv_Details
            For i = 0 To .RowCount - 1
                Sno = Sno + 1
                .Rows(i).Cells(0).Value = Sno
                If Val(.Rows(i).Cells(4).Value) <> 0 And Trim(.Rows(i).Cells(7).Value) <> "" Then
                    TotPcs = TotPcs + 1
                    TotMtrs = TotMtrs + Val(.Rows(i).Cells(4).Value)
                    TotWgt = TotWgt + Val(.Rows(i).Cells(5).Value)
                    vTotNewMtrs = vTotNewMtrs + Val(.Rows(i).Cells(12).Value)
                    vTotNewWgt = vTotNewWgt + Val(.Rows(i).Cells(13).Value)
                End If
            Next
        End With

        With dgv_Details_Total
            If .RowCount = 0 Then .Rows.Add()
            .Rows(0).Cells(2).Value = Val(TotPcs)
            .Rows(0).Cells(4).Value = Format(Val(TotMtrs), "########0.00")
            .Rows(0).Cells(5).Value = Format(Val(TotWgt), "########0.000")
            .Rows(0).Cells(12).Value = Format(Val(vTotNewMtrs), "########0.00")
            .Rows(0).Cells(13).Value = Format(Val(vTotNewWgt), "########0.000")
        End With

    End Sub

    Private Sub cbo_ClothFrom_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothFrom.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")
    End Sub

    Private Sub cbo_ClothFrom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothFrom.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothFrom, Nothing, txt_FoldingFrom, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")

        If (e.KeyValue = 38 And cbo_ClothFrom.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If cbo_Godown_StockTO.Visible = True Then
                cbo_Godown_StockTO.Focus()
            Else
                cbo_PartyTo.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_ClothFrom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ClothFrom.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothFrom, txt_FoldingFrom, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")
    End Sub

    Private Sub cbo_ClothFrom_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothFrom.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ClothFrom.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""
            f.MdiParent = MDIParent1
            f.Show()

        End If
    End Sub

    Private Sub cbo_ClothTypeTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothTypeTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothType_Head", "ClothType_Name", "(ClothType_IdNo Between 1 and 5)", "(ClothType_IdNo = 0)")
    End Sub

    Private Sub cbo_ClothTypeTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothTypeTo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothTypeTo, cbo_ClothTo, txt_Note, "ClothType_Head", "ClothType_Name", "(ClothType_IdNo Between 1 and 5)", "(ClothType_IdNo = 0)")


    End Sub

    Private Sub cbo_ClothTypeTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ClothTypeTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothTypeTo, Nothing, "ClothType_Head", "ClothType_Name", "(ClothType_IdNo Between 1 and 5)", "(ClothType_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then

            If cbo_ClothSales_OrderCode_forSelection_From.Visible = True Then
                cbo_ClothSales_OrderCode_forSelection_From.Focus()
            Else

                If dgv_Details.Rows.Count > 0 Then
                    dgv_Details.Focus()
                    dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(8)
                    dgv_Details.CurrentCell.Selected = True
                Else
                    txt_Note.Focus()
                End If
            End If

        End If

    End Sub

    Private Sub txt_Folding_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_FoldingFrom.KeyDown
        If (e.KeyValue = 38) Then
            If cbo_ClothFrom.Visible = True Then
                cbo_ClothFrom.Focus()

            End If
        End If
        If e.KeyValue = 40 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txt_Folding_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_FoldingFrom.KeyPress
        If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 13 Then e.Handled = True : SendKeys.Send("{TAB}")
    End Sub

    Private Sub dgv_Details_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellEnter
        Try

            If FrmLdSTS = True Then Exit Sub

            With dgv_Details
                If .Visible Then
                    If .Rows.Count > 0 Then
                        If e.ColumnIndex = 8 Or e.ColumnIndex = 9 Or e.ColumnIndex = 12 Or e.ColumnIndex = 13 Then
                            If Trim(.Rows(e.RowIndex).Cells(11).Value) <> "" Then
                                .Rows(e.RowIndex).Cells(e.ColumnIndex).ReadOnly = True
                            Else
                                .Rows(e.RowIndex).Cells(e.ColumnIndex).ReadOnly = False
                            End If
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            '---

        End Try
    End Sub

    Private Sub dgv_Details_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Details.CellValueChanged

        Try

            If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
            With dgv_Details
                If .Visible Then
                    If .Rows.Count > 0 Then
                        If e.ColumnIndex = 2 Or e.ColumnIndex = 4 Or e.ColumnIndex = 5 Or e.ColumnIndex = 12 Or e.ColumnIndex = 13 Then
                            Total_Calculation()
                        End If
                    End If
                End If
            End With

        Catch ex As Exception
            '-----

        End Try
    End Sub

    Private Sub dgv_Details_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgv_Details.EditingControlShowing
        dgtxt_Details = CType(dgv_Details.EditingControl, DataGridViewTextBoxEditingControl)
    End Sub

    Private Sub dgv_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Details.KeyUp
        Dim n As Integer

        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then

            With dgv_Details

                If Trim(.CurrentRow.Cells(11).Value) = "" Then

                    n = .CurrentRow.Index

                    If .CurrentCell.RowIndex = .Rows.Count - 1 Then
                        For J = 1 To .ColumnCount - 1
                            .Rows(n).Cells(J).Value = ""
                        Next

                    Else
                        .Rows.RemoveAt(n)

                    End If

                    Total_Calculation()

                End If
            End With
        End If
    End Sub

    Private Sub dgv_Details_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Details.LostFocus
        On Error Resume Next
        If Not IsNothing(dgv_Details.CurrentCell) Then dgv_Details.CurrentCell.Selected = False
    End Sub

    Private Sub dgv_Details_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles dgv_Details.RowsAdded
        Dim n As Integer

        If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
        With dgv_Details
            n = .RowCount
            .Rows(n - 1).Cells(0).Value = Val(n)
        End With

    End Sub

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
        Dim vClo_IdNo As Integer, vCloTyp_IdNo As Integer
        Dim Condt As String = ""


        Try

            Condt = ""
            vClo_IdNo = 0
            vCloTyp_IdNo = 0

            If IsDate(dtp_Filter_Fromdate.Value) = True And IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Piece_Transfer_Date between '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' and '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_Fromdate.Value) = True Then
                Condt = "a.Piece_Transfer_Date = '" & Trim(Format(dtp_Filter_Fromdate.Value, "MM/dd/yyyy")) & "' "
            ElseIf IsDate(dtp_Filter_ToDate.Value) = True Then
                Condt = "a.Piece_Transfer_Date = '" & Trim(Format(dtp_Filter_ToDate.Value, "MM/dd/yyyy")) & "' "
            End If

            If Trim(cbo_Filter_Cloth.Text) <> "" Then
                vClo_IdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_Filter_Cloth.Text)
            End If

            If Trim(cbo_Filter_ClothType.Text) <> "" Then
                vCloTyp_IdNo = Common_Procedures.ClothType_NameToIdNo(con, cbo_Filter_ClothType.Text)
            End If



            If Val(vClo_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " and ", "") & "(a.ClothFrom_IdNo = " & Str(Val(vClo_IdNo)) & " or a.ClothTo_IdNo = " & Str(Val(vClo_IdNo)) & ") "
            End If

            If Val(vCloTyp_IdNo) <> 0 Then
                Condt = Condt & IIf(Trim(Condt) <> "", " And ", "") & " a.ClothType_IdNo = " & Str(Val(vCloTyp_IdNo))
            End If



            da = New SqlClient.SqlDataAdapter("Select a.* , b.Cloth_name from Piece_Transfer_Head a Inner join Cloth_Head b On a.ClothTo_IdNo = b.cloth_idno where a.company_IdNo = " & Str(Val(lbl_Company.Tag)) & " And a.Piece_Transfer_Code Like '%/" & Trim(Common_Procedures.FnYearCode) & "' " & IIf(Trim(Condt) <> "", " and ", "") & Condt & " Order by a.for_orderby, a.Piece_Transfer_No", con)
            da.Fill(dt2)

            dgv_Filter_Details.Rows.Clear()

            If dt2.Rows.Count > 0 Then

                For i = 0 To dt2.Rows.Count - 1

                    n = dgv_Filter_Details.Rows.Add()

                    dgv_Filter_Details.Rows(n).Cells(0).Value = i + 1
                    dgv_Filter_Details.Rows(n).Cells(1).Value = dt2.Rows(i).Item("Piece_Transfer_No").ToString
                    dgv_Filter_Details.Rows(n).Cells(2).Value = Format(Convert.ToDateTime(dt2.Rows(i).Item("Piece_Transfer_Date").ToString), "dd-MM-yyyy")
                    dgv_Filter_Details.Rows(n).Cells(3).Value = dt2.Rows(i).Item("Cloth_Name").ToString
                    dgv_Filter_Details.Rows(n).Cells(4).Value = dt2.Rows(i).Item("Total_Pcs").ToString
                    dgv_Filter_Details.Rows(n).Cells(5).Value = Format(Val(dt2.Rows(i).Item("Total_Meters").ToString), "########0.00")
                    dgv_Filter_Details.Rows(n).Cells(6).Value = Format(Val(dt2.Rows(i).Item("Total_Weight").ToString), "########0.000")

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
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Filter_ClothType, cbo_Filter_Cloth, btn_Filter_Show, "ClothType_Head", "ClothType_Name", "", "ClothType_Name")
    End Sub

    Private Sub cbo_Filter_ClothType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Filter_ClothType.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Filter_ClothType, btn_Filter_Show, "ClothType_Head", "ClothType_Name", "", "ClothType_Name")
    End Sub

    Private Sub Open_FilterEntry()
        Dim movno As String

        Try

            movno = Trim(dgv_Filter_Details.CurrentRow.Cells(1).Value)

            If Val(movno) <> 0 Then
                Filter_Status = True
                move_record(movno)
                Pnl_Back.Enabled = True
                pnl_Filter.Visible = False
            End If

        Catch ex As Exception
            '-----

        End Try

    End Sub

    Private Sub dgv_Filter_Details_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Filter_Details.CellDoubleClick
        Open_FilterEntry()
    End Sub

    Private Sub dgv_Filter_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgv_Filter_Details.KeyDown
        If e.KeyCode = 13 Then
            Open_FilterEntry()
        End If
    End Sub
    Private Sub chk_SelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk_SelectAll.CheckedChanged
        Dim i As Integer
        Dim J As Integer
        Dim v1stVisiRow As Integer = 0

        With dgv_Selection

            For i = 0 To .Rows.Count - 1
                If .Rows(i).Visible = True Then

                    .Rows(i).Cells(7).Value = ""
                    For J = 0 To .ColumnCount - 1
                        .Rows(i).Cells(J).Style.ForeColor = Color.Black
                    Next J

                End If

            Next i

            v1stVisiRow = -1
            If chk_SelectAll.Checked = True Then

                For i = 0 To .Rows.Count - 1
                    If .Rows(i).Visible = True Then
                        Select_Piece(i)
                        If v1stVisiRow = -1 Then v1stVisiRow = i
                    End If
                Next i


            End If

            If .Rows.Count > 0 Then
                If v1stVisiRow >= 0 Then
                    .Focus()
                    .CurrentCell = .Rows(v1stVisiRow).Cells(0)
                    .CurrentCell.Selected = True
                Else
                    txt_LotSelction.Focus()
                End If
            End If

        End With

    End Sub

    Private Sub btn_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Selection.Click
        Dim Da As New SqlClient.SqlDataAdapter
        Dim Dt1 As New DataTable
        Dim Dt2 As New DataTable
        Dim Dt3 As New DataTable
        Dim Cmd As New SqlClient.SqlCommand
        Dim i As Integer, j As Integer, n As Integer, SNo As Integer
        Dim CloIdNo As Integer = 0, CloTypIdNo As Integer = 0
        Dim NewCode As String = ""
        Dim CompIDCondt As String = ""
        Dim led_id As Integer = 0
        Dim RptCondt As String = ""
        Dim vGod_ID As Integer = 0


        led_id = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_PartyFrom.Text)
        If cbo_PartyFrom.Visible = True Then
            If led_id = 0 Then
                MessageBox.Show("Invalid Party From Name", "DOES NOT SELECT PIECE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_PartyFrom.Enabled And cbo_PartyFrom.Visible Then cbo_PartyFrom.Focus()
                Exit Sub
            End If
        End If
        If led_id = 0 Then led_id = Common_Procedures.CommonLedger.OwnSort_Ac

        vGod_ID = Common_Procedures.Ledger_AlaisNameToIdNo(con, cbo_Godown_StockFROM.Text)
        If cbo_Godown_StockFROM.Visible = True Then
            If vGod_ID = 0 Then
                MessageBox.Show("Invalid Fabric Godown From Name", "DOES NOT SAVE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If cbo_Godown_StockFROM.Enabled And cbo_Godown_StockFROM.Visible Then cbo_Godown_StockFROM.Focus()
                Exit Sub
            End If
        End If
        If vGod_ID = 0 Then vGod_ID = Common_Procedures.CommonLedger.Godown_Ac

        CloIdNo = Common_Procedures.Cloth_NameToIdNo(con, cbo_ClothFrom.Text)
        If CloIdNo = 0 Then
            MessageBox.Show("Invalid Cloth Name", "DOES NOT SELECT PIECE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If cbo_ClothFrom.Enabled And cbo_ClothFrom.Visible Then cbo_ClothFrom.Focus()
            Exit Sub
        End If

        If Val(txt_FoldingFrom.Text) = 0 Then
            txt_FoldingFrom.Text = 100
            'MessageBox.Show("Invalid Folding", "DOES NOT SELECT PIECE...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            'If txt_Folding.Enabled And txt_Folding.Visible Then txt_Folding.Focus()
            'Exit Sub
        End If

        CompIDCondt = "(a.company_idno = " & Str(Val(lbl_Company.Tag)) & ")"
        If Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1155" And Trim(UCase(Common_Procedures.settings.CustomerCode)) <> "1204" Then '----KRG TEXTILE MILLS (PALLADAM)
            If Common_Procedures.settings.EntrySelection_Combine_AllCompany = 1 Then
                CompIDCondt = ""
                If Trim(UCase(Common_Procedures.User.Type)) = "ACCOUNT" Then
                    CompIDCondt = "(tZ.Company_Type <> 'UNACCOUNT')"
                End If
            End If
        End If

        If Trim(txt_LotSelction.Text) <> "" Then
            CompIDCondt = Trim(CompIDCondt) & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Lot_No = '" & Trim(txt_LotSelction.Text) & "'"
        End If

        If Trim(txt_PcsSelction.Text) <> "" Then
            CompIDCondt = Trim(CompIDCondt) & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.Piece_No = '" & Trim(txt_PcsSelction.Text) & "'"
        End If



        NewCode = Trim(Val(lbl_Company.Tag)) & "-" & Trim(lbl_RefNo.Text) & "/" & Trim(Common_Procedures.FnYearCode)

        With dgv_Selection

            chk_SelectAll.Checked = False

            .Rows.Clear()
            SNo = 0

            Da = New SqlClient.SqlDataAdapter("select a.*, b.clothtype_name from Piece_Transfer_Details a  INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno INNER JOIN ClothType_Head b ON a.ClothType_IdNo = b.ClothType_IdNo where a.Piece_Transfer_Code = '" & Trim(NewCode) & "' Order by a.sl_no", con)
            Dt2 = New DataTable
            Da.Fill(Dt2)

            dgv_Details.Rows.Clear()
            SNo = 0

            If Dt2.Rows.Count > 0 Then

                For i = 0 To Dt2.Rows.Count - 1

                    n = .Rows.Add()

                    SNo = SNo + 1
                    .Rows(n).Cells(0).Value = Val(SNo)
                    .Rows(n).Cells(1).Value = Dt2.Rows(i).Item("Lot_No").ToString
                    .Rows(n).Cells(2).Value = Dt2.Rows(i).Item("Pcs_NO").ToString
                    .Rows(n).Cells(3).Value = Dt2.Rows(i).Item("clothtype_name").ToString
                    .Rows(n).Cells(4).Value = Format(Val(Dt2.Rows(i).Item("Meters").ToString), "########0.00")
                    If Val(Dt2.Rows(i).Item("Weight").ToString) <> 0 Then
                        .Rows(n).Cells(5).Value = Format(Val(Dt2.Rows(i).Item("Weight").ToString), "########0.000")
                    End If
                    If Val(Dt2.Rows(i).Item("Weight_Meter").ToString) <> 0 Then
                        .Rows(n).Cells(6).Value = Format(Val(Dt2.Rows(i).Item("Weight_Meter").ToString), "########0.000")
                    End If
                    .Rows(n).Cells(7).Value = "1"
                    .Rows(n).Cells(8).Value = Dt2.Rows(i).Item("lot_code").ToString
                    .Rows(n).Cells(9).Value = Dt2.Rows(i).Item("New_LotCode").ToString
                    .Rows(n).Cells(10).Value = Dt2.Rows(i).Item("New_LotNo").ToString
                    .Rows(n).Cells(11).Value = Dt2.Rows(i).Item("New_PcsNo").ToString

                    .Rows(n).Cells(12).Value = ""
                    Da = New SqlClient.SqlDataAdapter("select count(*) from Weaver_ClothReceipt_Piece_Details Where Weaver_Piece_Checking_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Weaver_ClothReceipt_Code = '" & Trim(Pk_Condition) & Trim(NewCode) & "' and Lot_Code = '" & Trim(Dt2.Rows(i).Item("New_LotCode").ToString) & "' and Piece_No = '" & Trim(Dt2.Rows(i).Item("New_PcsNo").ToString) & "' and (PackingSlip_Code_Type1 <> '' or PackingSlip_Code_Type2 <> ''or PackingSlip_Code_Type3 <> ''or PackingSlip_Code_Type4 <> ''or PackingSlip_Code_Type5 <> '')", con)
                    Dt3 = New DataTable
                    Da.Fill(Dt3)
                    If Dt3.Rows.Count > 0 Then
                        If IsDBNull(Dt3.Rows(0)(0).ToString) = False Then
                            If Val(Dt3.Rows(0)(0).ToString) <> 0 Then
                                .Rows(n).Cells(12).Value = "1"
                            End If
                        End If
                    End If
                    Dt3.Clear()

                    .Rows(n).Cells(13).Value = Dt2.Rows(i).Item("New_Meters").ToString
                    .Rows(n).Cells(14).Value = Dt2.Rows(i).Item("New_Weight").ToString


                    For j = 0 To .ColumnCount - 1
                        .Rows(n).Cells(j).Style.ForeColor = Color.Red
                    Next

                Next i

            End If


            If led_id = 4 Or led_id = 5 Then
                CompIDCondt = CompIDCondt & IIf(CompIDCondt <> "", " and ", " ") & "(a.StockOff_IdNo = 4 or a.StockOff_IdNo = 5)"
            Else
                CompIDCondt = CompIDCondt & IIf(CompIDCondt <> "", " and ", " ") & "(a.StockOff_IdNo = " & Str(led_id) & ")"
            End If

            If cbo_Godown_StockFROM.Visible = True Then
                CompIDCondt = CompIDCondt & IIf(CompIDCondt <> "", " and ", " ") & "(a.WareHouse_idno = " & Str(vGod_ID) & ")"
            End If

            Da = New SqlClient.SqlDataAdapter("Select a.* from Weaver_ClothReceipt_Piece_Details a INNER JOIN Company_Head tZ ON a.company_idno = tZ.company_idno where " & CompIDCondt & IIf(Trim(CompIDCondt) <> "", " and ", "") & " a.cloth_Idno = " & Str(Val(CloIdNo)) & " and a.Folding = " & Str(Val(txt_FoldingFrom.Text)) & " and ((a.Type1_Meters <> 0 and a.PackingSlip_Code_Type1 = '') or (a.Type2_Meters <> 0 and a.PackingSlip_Code_Type2 = '')  or (a.Type3_Meters <> 0 and a.PackingSlip_Code_Type3 = '')  or (a.Type4_Meters <> 0 and a.PackingSlip_Code_Type4 = '')  or (a.Type5_Meters <> 0 and a.PackingSlip_Code_Type5 = '') ) order by a.Weaver_ClothReceipt_Date, a.for_orderby, a.Weaver_ClothReceipt_No, a.PieceNo_OrderBy, a.Piece_No", con)
            Dt1 = New DataTable
            Da.Fill(Dt1)

            If Dt1.Rows.Count > 0 Then

                For i = 0 To Dt1.Rows.Count - 1

                    If Val(Dt1.Rows(i).Item("Type1_Meters").ToString) <> 0 And Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type1").ToString) = "" Then

                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(3).Value = Common_Procedures.ClothType.Type1
                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Type1_Meters").ToString
                        If Val(Dt1.Rows(i).Item("Weight_Meter").ToString) <> 0 Then
                            .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Type1_Meters").ToString) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                            .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)
                        End If
                        .Rows(n).Cells(7).Value = ""
                        .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(9).Value = ""
                        .Rows(n).Cells(10).Value = ""
                        .Rows(n).Cells(11).Value = ""
                        .Rows(n).Cells(12).Value = ""

                    End If

                    If Val(Dt1.Rows(i).Item("Type2_Meters").ToString) <> 0 And Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type2").ToString) = "" Then
                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(3).Value = Common_Procedures.ClothType.Type2
                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Type2_Meters").ToString
                        If Val(Dt1.Rows(i).Item("Weight_Meter").ToString) <> 0 Then
                            .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Type2_Meters").ToString) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                            .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)
                        End If
                        .Rows(n).Cells(7).Value = ""
                        .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(9).Value = ""
                        .Rows(n).Cells(10).Value = ""
                        .Rows(n).Cells(11).Value = ""
                        .Rows(n).Cells(12).Value = ""

                    End If


                    If Val(Dt1.Rows(i).Item("Type3_Meters").ToString) <> 0 And Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type3").ToString) = "" Then
                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(3).Value = Common_Procedures.ClothType.Type3
                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Type3_Meters").ToString
                        If Val(Dt1.Rows(i).Item("Weight_Meter").ToString) <> 0 Then
                            .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Type3_Meters").ToString) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                            .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)
                        End If
                        .Rows(n).Cells(7).Value = ""
                        .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(9).Value = ""
                        .Rows(n).Cells(10).Value = ""
                        .Rows(n).Cells(11).Value = ""
                        .Rows(n).Cells(12).Value = ""

                    End If


                    If Val(Dt1.Rows(i).Item("Type4_Meters").ToString) <> 0 And Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type4").ToString) = "" Then
                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(3).Value = Common_Procedures.ClothType.Type4
                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Type4_Meters").ToString
                        If Val(Dt1.Rows(i).Item("Weight_Meter").ToString) <> 0 Then
                            .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Type4_Meters").ToString) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                            .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)
                        End If
                        .Rows(n).Cells(7).Value = ""
                        .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(9).Value = ""
                        .Rows(n).Cells(10).Value = ""
                        .Rows(n).Cells(11).Value = ""
                        .Rows(n).Cells(12).Value = ""

                    End If

                    If Val(Dt1.Rows(i).Item("Type5_Meters").ToString) <> 0 And Trim(Dt1.Rows(i).Item("PackingSlip_Code_Type5").ToString) = "" Then
                        n = .Rows.Add()

                        SNo = SNo + 1
                        .Rows(n).Cells(0).Value = Val(SNo)
                        .Rows(n).Cells(1).Value = Dt1.Rows(i).Item("Lot_No").ToString
                        .Rows(n).Cells(2).Value = Dt1.Rows(i).Item("Piece_No").ToString
                        .Rows(n).Cells(3).Value = Common_Procedures.ClothType.Type5
                        .Rows(n).Cells(4).Value = Dt1.Rows(i).Item("Type5_Meters").ToString
                        If Val(Dt1.Rows(i).Item("Weight_Meter").ToString) <> 0 Then
                            .Rows(n).Cells(5).Value = Format(Val(Dt1.Rows(i).Item("Type5_Meters").ToString) * Val(Dt1.Rows(i).Item("Weight_Meter").ToString), "#########0.000")
                            .Rows(n).Cells(6).Value = Val(Dt1.Rows(i).Item("Weight_Meter").ToString)
                        End If
                        .Rows(n).Cells(7).Value = ""
                        .Rows(n).Cells(8).Value = Dt1.Rows(i).Item("Lot_Code").ToString
                        .Rows(n).Cells(9).Value = ""
                        .Rows(n).Cells(10).Value = ""
                        .Rows(n).Cells(11).Value = ""
                        .Rows(n).Cells(12).Value = ""

                    End If

                Next

            End If
            Dt1.Clear()

        End With

        txt_LotSelction.Text = ""
        txt_PcsSelction.Text = ""
        cbo_PieceType_From_Selection.Text = ""

        pnl_Selection.Visible = True
        Pnl_Back.Enabled = False
        'dgv_Selection.Focus()
        txt_LotSelction.Focus()

    End Sub

    Private Sub dgv_Selection_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_Selection.CellClick
        Select_Piece(e.RowIndex)
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

    Private Sub Select_Piece(ByVal RwIndx As Integer)
        Dim i As Integer

        With dgv_Selection

            If .RowCount > 0 And RwIndx >= 0 Then

                .Rows(RwIndx).Cells(7).Value = (Val(.Rows(RwIndx).Cells(7).Value) + 1) Mod 2

                If Val(.Rows(RwIndx).Cells(7).Value) = 1 Then

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Red
                    Next

                Else

                    .Rows(RwIndx).Cells(7).Value = ""

                    For i = 0 To .ColumnCount - 1
                        .Rows(RwIndx).Cells(i).Style.ForeColor = Color.Blue
                    Next

                End If

            End If

        End With

    End Sub

    Private Sub btn_Close_Selection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Close_Selection.Click
        Piece_Selection()
    End Sub

    Private Sub Piece_Selection()
        Dim n As Integer = 0
        Dim sno As Integer = 0
        Dim i As Integer = 0


        Pnl_Back.Enabled = True
        pnl_Selection.Visible = False

        dgv_Details.Rows.Clear()

        sno = 0
        For i = 0 To dgv_Selection.RowCount - 1

            If Val(dgv_Selection.Rows(i).Cells(7).Value) = 1 Then

                n = dgv_Details.Rows.Add()

                sno = sno + 1
                dgv_Details.Rows(n).Cells(0).Value = sno
                dgv_Details.Rows(n).Cells(1).Value = dgv_Selection.Rows(i).Cells(1).Value
                dgv_Details.Rows(n).Cells(2).Value = dgv_Selection.Rows(i).Cells(2).Value
                dgv_Details.Rows(n).Cells(3).Value = dgv_Selection.Rows(i).Cells(3).Value
                dgv_Details.Rows(n).Cells(4).Value = dgv_Selection.Rows(i).Cells(4).Value
                If Val(dgv_Selection.Rows(i).Cells(5).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(5).Value = dgv_Selection.Rows(i).Cells(5).Value
                End If
                If Val(dgv_Selection.Rows(i).Cells(6).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(6).Value = dgv_Selection.Rows(i).Cells(6).Value
                End If
                dgv_Details.Rows(n).Cells(7).Value = dgv_Selection.Rows(i).Cells(8).Value

                If Trim(dgv_Selection.Rows(i).Cells(10).Value) <> "" Then
                    dgv_Details.Rows(n).Cells(8).Value = dgv_Selection.Rows(i).Cells(10).Value
                Else
                    dgv_Details.Rows(n).Cells(8).Value = dgv_Selection.Rows(i).Cells(1).Value & "T"
                End If

                If Trim(dgv_Selection.Rows(i).Cells(11).Value) <> "" Then
                    dgv_Details.Rows(n).Cells(9).Value = dgv_Selection.Rows(i).Cells(11).Value
                Else
                    dgv_Details.Rows(n).Cells(9).Value = dgv_Selection.Rows(i).Cells(2).Value
                End If

                dgv_Details.Rows(n).Cells(10).Value = dgv_Selection.Rows(i).Cells(9).Value
                dgv_Details.Rows(n).Cells(11).Value = dgv_Selection.Rows(i).Cells(12).Value

                If Val(dgv_Selection.Rows(i).Cells(13).Value) <> 0 Then
                    dgv_Details.Rows(n).Cells(12).Value = dgv_Selection.Rows(i).Cells(13).Value

                Else
                    If Val(txt_FoldingFrom.Text) <> 0 And Val(txt_FoldingTo.Text) <> 0 Then
                        dgv_Details.Rows(n).Cells(12).Value = Common_Procedures.Meter_RoundOff(Format(Val(dgv_Selection.Rows(i).Cells(4).Value) * Val(txt_FoldingFrom.Text) / Val(txt_FoldingTo.Text), "##########0.00"))
                    End If

                End If

                dgv_Details.Rows(n).Cells(13).Value = dgv_Selection.Rows(i).Cells(14).Value



            End If

        Next i

        Total_Calculation()

        Pnl_Back.Enabled = True
        pnl_Selection.Visible = False
        If cbo_ClothTo.Enabled And cbo_ClothTo.Visible Then cbo_ClothTo.Focus()

    End Sub

    Private Sub dgtxt_Details_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyDown
        With dgv_Details
            If .Visible Then
                If .CurrentCell.ColumnIndex = 4 Or .CurrentCell.ColumnIndex = 5 Or .CurrentCell.ColumnIndex = 6 Or .CurrentCell.ColumnIndex = 8 Or .CurrentCell.ColumnIndex = 9 Or .CurrentCell.ColumnIndex = 12 Or .CurrentCell.ColumnIndex = 13 Then

                    If Trim(.CurrentRow.Cells(11).Value) <> "" Then
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
                If .CurrentCell.ColumnIndex = 12 Or .CurrentCell.ColumnIndex = 13 Then

                    If Common_Procedures.Accept_NumericOnly(Asc(e.KeyChar)) = 0 Then
                        e.Handled = True
                    End If

                End If

            End If

        End With

    End Sub

    Private Sub dgtxt_Details_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgtxt_Details.KeyUp
        If e.Control = True And UCase(Chr(e.KeyCode)) = "D" Then
            dgv_Details_KeyUp(sender, e)
        End If
    End Sub
    Private Sub cbo_PartyFrom_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyFrom.GotFocus, cbo_PartyFrom.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "((Ledger_Type = 'OWNSORT' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'GODOWN') and Close_status = 0)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PartyFrom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyFrom.KeyDown, cbo_PartyFrom.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyFrom, msk_date, cbo_PartyTo, "Ledger_AlaisHead", "Ledger_DisplayName", "((Ledger_Type = 'OWNSORT' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'GODOWN') and Close_status = 0)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PartyFrom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyFrom.KeyPress, cbo_PartyFrom.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyFrom, cbo_PartyTo, "Ledger_AlaisHead", "Ledger_DisplayName", "((Ledger_Type = 'OWNSORT' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'GODOWN') and Close_status = 0)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PartyFrom_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyFrom.KeyUp, cbo_PartyFrom.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.JOBWORKER_Creation
            Common_Procedures.MDI_LedType = "JOBWORKER"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PartyFrom.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_PartyTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PartyTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "((Ledger_Type = 'OWNSORT' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'GODOWN') and Close_status = 0)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_PartyTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyTo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_PartyTo, cbo_PartyFrom, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "((Ledger_Type = 'OWNSORT' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'GODOWN') and Close_status = 0)", "(Ledger_IdNo = 0)")

        If (e.KeyValue = 40 And cbo_PartyTo.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If cbo_Godown_StockFROM.Visible = True Then
                cbo_Godown_StockFROM.Focus()
            Else
                cbo_ClothFrom.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_PartyTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PartyTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_PartyTo, Nothing, "Ledger_AlaisHead", "Ledger_DisplayName", "((Ledger_Type = 'OWNSORT' or Ledger_Type = 'JOBWORKER' or Ledger_Type = 'GODOWN') and Close_status = 0)", "(Ledger_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            If cbo_Godown_StockFROM.Visible = True Then
                cbo_Godown_StockFROM.Focus()
            Else
                cbo_ClothFrom.Focus()
            End If
        End If

    End Sub

    Private Sub cbo_PartyTo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PartyTo.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.JOBWORKER_Creation
            Common_Procedures.MDI_LedType = "JOBWORKER"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_PartyTo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_ClothTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ClothTo.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")
    End Sub

    Private Sub cbo_ClothTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothTo.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_ClothTo, txt_FoldingTo, cbo_ClothTypeTo, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")
    End Sub

    Private Sub cbo_ClothTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_ClothTo.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_ClothTo, cbo_ClothTypeTo, "Cloth_Head", "Cloth_Name", "", "Cloth_Name")
    End Sub

    Private Sub cbo_ClothTo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_ClothTo.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Dim f As New Cloth_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = cbo_ClothTo.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""
            f.MdiParent = MDIParent1
            f.Show()

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
        If (e.KeyValue = 38) Then cbo_ClothTypeTo.Focus()


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

    Public Sub print_record() Implements Interface_MDIActions.print_record
        If Common_Procedures.UserRight_NEWCheck(Common_Procedures.UserRightsCheckFor.PrintEntry, Common_Procedures.UR.Piece_Transfer_Entry, New_Entry) = False Then Exit Sub

        PrintDocument1.Print()
    End Sub

    Private Sub btn_Select_ItemDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Set_Bm_selection.Click, btn_lot_Pcs_selection.Click
        btn_Selection_Click(sender, e)
        dgv_Selection.Focus()
    End Sub
    Private Sub txt_SetNoSelection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_LotSelction.KeyDown
        If (e.KeyValue = 40) Then
            txt_PcsSelction.Focus()
        End If
    End Sub

    Private Sub txt_SetNoSelection_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_LotSelction.KeyPress
        If Asc(e.KeyChar) = 13 Then
            txt_PcsSelction.Focus()
        End If
    End Sub

    Private Sub txt_BeamNoSelection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_PcsSelction.KeyDown
        If e.KeyValue = 40 Then
            If dgv_Selection.Rows.Count > 0 Then
                dgv_Selection.Focus()
                dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
                dgv_Selection.CurrentCell.Selected = True
            End If
        End If
        If (e.KeyValue = 38) Then txt_LotSelction.Focus()
    End Sub

    Private Sub txt_BeamNoSelection_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_PcsSelction.KeyPress
        If Asc(e.KeyChar) = 13 Then

            If Trim(txt_PcsSelction.Text) <> "" Or Trim(txt_LotSelction.Text) <> "" Then
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

        If Trim(txt_PcsSelction.Text) <> "" Or Trim(txt_LotSelction.Text) <> "" Then

            LtNo = Trim(txt_LotSelction.Text)
            PcsNo = Trim(txt_PcsSelction.Text)

            For i = 0 To dgv_Selection.Rows.Count - 1
                If Trim(UCase(LtNo)) = Trim(UCase(dgv_Selection.Rows(i).Cells(1).Value)) And Trim(UCase(PcsNo)) = Trim(UCase(dgv_Selection.Rows(i).Cells(2).Value)) Then
                    Call Select_Piece(i)

                    dgv_Selection.CurrentCell = dgv_Selection.Rows(i).Cells(0)
                    If i >= 9 Then dgv_Selection.FirstDisplayedScrollingRowIndex = i - 8

                    Exit For

                End If
            Next

            txt_LotSelction.Text = ""
            txt_PcsSelction.Text = ""
            If txt_LotSelction.Enabled = True Then txt_LotSelction.Focus()

        End If
    End Sub

    'Private Sub chk_SelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk_SelectAll.CheckedChanged
    '    Dim i As Integer
    '    Dim J As Integer

    '    With dgv_Selection

    '        For i = 0 To .Rows.Count - 1
    '            .Rows(i).Cells(8).Value = ""
    '            For J = 0 To .ColumnCount - 1
    '                .Rows(i).Cells(J).Style.ForeColor = Color.Black
    '            Next J
    '        Next i

    '        If chk_SelectAll.Checked = True Then
    '            For i = 0 To .Rows.Count - 1
    '                Select_Piece(i)
    '            Next i
    '        End If

    '        If .Rows.Count > 0 Then
    '            .Focus()
    '            .CurrentCell = .Rows(0).Cells(0)
    '            .CurrentCell.Selected = True
    '        End If

    '    End With

    'End Sub

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

        LastNo = lbl_RefNo.Text

        movefirst_record()
        Timer1.Enabled = True

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        save_record()
        If Trim(UCase(LastNo)) = Trim(UCase(lbl_RefNo.Text)) Then
            Timer1.Enabled = False
            SaveAll_STS = False
            MessageBox.Show("All entries saved sucessfully", "FOR SAVING...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

        Else
            movenext_record()

        End If
    End Sub
    Private Sub msk_Date_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyUp
        If e.Control = False And e.KeyValue = 17 And vcbo_KeyDwnVal = e.KeyValue Then
            msk_Date.Text = Date.Today
        End If
        If IsDate(msk_date.Text) = True Then
            If e.KeyCode = 107 Then
                msk_date.Text = DateAdd("D", 1, Convert.ToDateTime(msk_date.Text))
            ElseIf e.KeyCode = 109 Then
                msk_date.Text = DateAdd("D", -1, Convert.ToDateTime(msk_date.Text))
            End If
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

    Private Sub dtp_Date_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtp_Date.TextChanged
        If IsDate(dtp_Date.Text) = True Then
            msk_date.Text = dtp_Date.Text
        End If
    End Sub

    Private Sub msk_Date_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles msk_date.KeyDown
        vcbo_KeyDwnVal = e.KeyValue
    End Sub

    Private Sub txt_FoldingTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_FoldingTo.KeyDown
        If e.KeyCode = 38 Then e.Handled = True : SendKeys.Send("+{TAB}")
        If e.KeyCode = 40 Then
            If MessageBox.Show("Do you want to select Piece", "FOR PIECE SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)
            Else
                cbo_ClothTo.Focus()
            End If
        End If
    End Sub

    Private Sub txt_FoldingTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_FoldingTo.KeyPress
        If Asc(e.KeyChar) = 13 Then
            If MessageBox.Show("Do you want to select Piece", "FOR PIECE SELECTION...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = DialogResult.Yes Then
                btn_Selection_Click(sender, e)
            Else
                cbo_ClothTo.Focus()

            End If
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

    Private Sub cbo_PieceType_From_Selection_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_PieceType_From_Selection.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothType_Head", "ClothType_Name", "(ClothType_IdNo Between 1 and 5)", "(ClothType_IdNo = 0)")
    End Sub

    Private Sub cbo_PieceType_From_Selection_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_PieceType_From_Selection.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, txt_LotSelction, Nothing, "ClothType_Head", "ClothType_Name", "(ClothType_IdNo Between 1 and 5)", "(ClothType_IdNo = 0)")
        If (e.KeyValue = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If dgv_Selection.Rows.Count > 0 Then
                dgv_Selection.Focus()
                dgv_Selection.CurrentCell = dgv_Selection.Rows(0).Cells(0)
                dgv_Selection.CurrentCell.Selected = True
            Else
                txt_LotSelction.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_PieceType_From_Selection_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_PieceType_From_Selection.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "ClothType_Head", "ClothType_Name", "(ClothType_IdNo Between 1 and 5)", "(ClothType_IdNo = 0)")

        If Asc(e.KeyChar) = 13 Then
            btn_Show_PieceType_From_Selection_Click(sender, e)
        End If

    End Sub

    Private Sub btn_Show_PieceType_From_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Show_PieceType_From_Selection.Click
        Dim vFirstRowNo As Integer = -1
        Dim i As Integer
        Dim vClthTypFrm_ID As Integer

        vClthTypFrm_ID = Common_Procedures.ClothType_NameToIdNo(con, cbo_PieceType_From_Selection.Text)

        If Val(vClthTypFrm_ID) <> 0 Then

            For i = 0 To dgv_Selection.Rows.Count - 1
                dgv_Selection.Rows(i).Visible = False
            Next

            vFirstRowNo = -1
            For i = 0 To dgv_Selection.Rows.Count - 1
                If Trim(UCase(dgv_Selection.Rows(i).Cells(3).Value)) = Trim(UCase(cbo_PieceType_From_Selection.Text)) Then
                    dgv_Selection.Rows(i).Visible = True
                    If vFirstRowNo = -1 Then vFirstRowNo = i
                End If
            Next

            If vFirstRowNo >= 0 Then
                dgv_Selection.Focus()
                dgv_Selection.CurrentCell = dgv_Selection.Rows(vFirstRowNo).Cells(0)
                dgv_Selection.CurrentCell.Selected = True

            Else
                cbo_PieceType_From_Selection.SelectAll()
                If cbo_PieceType_From_Selection.Enabled = True Then cbo_PieceType_From_Selection.Focus()

            End If

        Else

            btn_ShowAll_Selection_Click(sender, e)

        End If

    End Sub

    Private Sub btn_ShowAll_Selection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_ShowAll_Selection.Click
        Dim i As Integer = 0
        Dim CurRow As Integer = 0

        Try
            For i = 0 To dgv_Selection.Rows.Count - 1
                dgv_Selection.Rows(i).Visible = True
            Next
            cbo_PieceType_From_Selection.Text = ""
        Catch ex As Exception
            '---
        End Try
    End Sub

    Private Sub btn_Calculation_Click(sender As Object, e As EventArgs) Handles btn_Calculation.Click
        Total_Calculation()
    End Sub

    Private Sub dgv_Details_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_Details.CellDoubleClick
        With dgv_Details

            If .Visible Then

                If IsNothing(dgv_Details.CurrentCell) Then Exit Sub
                If .CurrentCell.ColumnIndex >= 8 And .CurrentCell.ColumnIndex <= 13 Then

                    If Trim(.Rows(.CurrentCell.RowIndex).Cells(11).Value) <> "" Then
                        MessageBox.Show("BALE No. / DC NO.  : " & Trim(.Rows(.CurrentCell.RowIndex).Cells(11).Value), "PIECE DELIVERED STATUS...", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
                    End If

                End If

            End If

        End With
    End Sub

    Private Sub cbo_Godown_Stockfrom_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Godown_StockFROM.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' and Close_status = 0)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Godown_Stockfrom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Godown_StockFROM.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Godown_StockFROM, Nothing, cbo_Godown_StockTO, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' and Close_status = 0)  ", "(Ledger_IdNo = 0)")
        If (e.KeyValue = 38 And cbo_Godown_StockFROM.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then
            If cbo_PartyTo.Visible = True Then
                cbo_PartyTo.Focus()
            Else
                msk_date.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_Godown_Stockfrom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Godown_StockFROM.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Godown_StockFROM, cbo_Godown_StockTO, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' and Close_status = 0)", "(Ledger_IdNo = 0)")
    End Sub


    Private Sub cbo_Godown_StockTo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Godown_StockTO.GotFocus
        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' and Close_status = 0)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Godown_StockTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cbo_Godown_StockTO.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, cbo_Godown_StockTO, cbo_Godown_StockFROM, cbo_ClothFrom, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' and Close_status = 0)  ", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Godown_StockTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Godown_StockTO.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, cbo_Godown_StockTO, cbo_ClothFrom, "Ledger_AlaisHead", "Ledger_DisplayName", "(Ledger_Type = 'GODOWN' and Close_status = 0)", "(Ledger_IdNo = 0)")
    End Sub

    Private Sub cbo_Godown_StockFROM_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_Godown_StockFROM.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Godown_Creation
            Common_Procedures.MDI_LedType = "GODOWN"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = sender.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub


    Private Sub cbo_Godown_StockTO_KeyUp(sender As Object, e As KeyEventArgs) Handles cbo_Godown_StockTO.KeyUp
        If e.Control = False And e.KeyValue = 17 Then
            Common_Procedures.UR.Ledr_Wea_Siz_Rw_Trans_JbWrk_Creation = Common_Procedures.UR.Godown_Creation
            Common_Procedures.MDI_LedType = "GODOWN"
            Dim f As New Ledger_Creation

            Common_Procedures.Master_Return.Form_Name = Me.Name
            Common_Procedures.Master_Return.Control_Name = sender.Name
            Common_Procedures.Master_Return.Return_Value = ""
            Common_Procedures.Master_Return.Master_Type = ""

            f.MdiParent = MDIParent1
            f.Show()
        End If
    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_From_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_ClothSales_OrderCode_forSelection_From.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, cbo_ClothSales_OrderCode_forSelection_To, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")
    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_From_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_ClothSales_OrderCode_forSelection_From.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, Nothing, cbo_ClothSales_OrderCode_forSelection_To, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")

        If (e.KeyCode = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then

            cbo_ClothSales_OrderCode_forSelection_To.Focus()

        End If


        If (e.KeyCode = 38 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 38) Then

            If cbo_ClothTypeTo.Visible = True Then
                cbo_ClothTypeTo.Focus()

            Else

                txt_FoldingTo.Focus()

            End If

        End If

    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_From_Enter(sender As Object, e As EventArgs) Handles cbo_ClothSales_OrderCode_forSelection_From.Enter
        vCLO_CONDT = "(ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode1) & "' or ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode2) & "' and Order_Close_Status = 0 )"

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")
    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_To_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_ClothSales_OrderCode_forSelection_To.SelectedIndexChanged

    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_To_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cbo_ClothSales_OrderCode_forSelection_To.KeyPress
        Common_Procedures.ComboBox_ItemSelection_KeyPress(sender, e, con, sender, Nothing, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")


        If Asc(e.KeyChar) = 13 Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(8)
                dgv_Details.CurrentCell.Selected = True
            Else
                txt_Note.Focus()
            End If
        End If


    End Sub

    Private Sub cbo_ClothTypeTo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbo_ClothTypeTo.SelectedIndexChanged

    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_To_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_ClothSales_OrderCode_forSelection_To.KeyDown
        Common_Procedures.ComboBox_ItemSelection_KeyDown(sender, e, con, sender, cbo_ClothSales_OrderCode_forSelection_From, Nothing, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")

        If (e.KeyCode = 40 And sender.DroppedDown = False) Or (e.Control = True And e.KeyValue = 40) Then
            If dgv_Details.Rows.Count > 0 Then
                dgv_Details.Focus()
                dgv_Details.CurrentCell = dgv_Details.Rows(0).Cells(8)
                dgv_Details.CurrentCell.Selected = True
            Else
                txt_Note.Focus()
            End If
        End If
    End Sub

    Private Sub cbo_ClothSales_OrderCode_forSelection_To_Enter(sender As Object, e As EventArgs) Handles cbo_ClothSales_OrderCode_forSelection_To.Enter
        vCLO_CONDT = "(ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode1) & "' or ClothSales_Order_Code LIKE '%/" & Trim(FnYearCode2) & "' and Order_Close_Status = 0 )"

        Common_Procedures.ComboBox_ItemSelection_SetDataSource(sender, con, "ClothSales_Order_Head", "ClothSales_OrderCode_forSelection", vCLO_CONDT, "(ClothSales_Order_Code = '999999/00-00')")

    End Sub
End Class